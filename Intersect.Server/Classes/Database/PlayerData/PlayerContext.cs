﻿using Intersect.Server.Classes.Database;
using Intersect.Server.Classes.Database.PlayerData.Api;
using Intersect.Server.Database.PlayerData.Players;
using Intersect.Server.Database.PlayerData.SeedData;
using Intersect.Server.Entities;
using Intersect.Utilities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Intersect.Server.Database.PlayerData
{
    public class PlayerContext : DbContext, ISeedableContext
    {
        public static PlayerContext Current { get; private set; }

        [NotNull]
        public static PlayerContext Temporary => new PlayerContext(Current?.mConnection ?? default(DatabaseUtils.DbProvider), Current?.mConnectionString, true);

        [NotNull] public DbSet<User> Users { get; set; }

        [NotNull] public DbSet<Mute> Mutes { get; set; }
        [NotNull] public DbSet<Ban> Bans { get; set; }

        [NotNull] public DbSet<RefreshToken> RefreshTokens { get; set; }

        [NotNull] public DbSet<Player> Players { get; set; }
        [NotNull] public DbSet<BankSlot> Player_Bank { get; set; }
        [NotNull] public DbSet<Friend> Player_Friends { get; set; }
        [NotNull] public DbSet<HotbarSlot> Player_Hotbar { get; set; }
        [NotNull] public DbSet<InventorySlot> Player_Items { get; set; }
        [NotNull] public DbSet<Quest> Player_Quests { get; set; }
        [NotNull] public DbSet<SpellSlot> Player_Spells { get; set; }
        [NotNull] public DbSet<Variable> Player_Variables { get; set; }

        [NotNull] public DbSet<Bag> Bags { get; set; }
        [NotNull] public DbSet<BagSlot> Bag_Items { get; set; }

        private DatabaseUtils.DbProvider mConnection = DatabaseUtils.DbProvider.Sqlite;
        private string mConnectionString = @"Data Source=resources/playerdata.db";

        public PlayerContext()
        {
            Current = this;
        }

        public PlayerContext(DatabaseUtils.DbProvider connection, string connectionString)
            : this(connection, connectionString, false)
        {
        }

        private PlayerContext(DatabaseUtils.DbProvider connection, string connectionString, bool isTemporary)
        {
            mConnection = connection;
            mConnectionString = connectionString;

            if (!isTemporary)
            {
                Current = this;
            }
        }

        internal async ValueTask Commit(bool commit = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!commit)
            {
                return;
            }

            var task = SaveChangesAsync(cancellationToken);
            if (task != null)
            {
                await task;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (mConnection)
            {
                case DatabaseUtils.DbProvider.Sqlite:
                    optionsBuilder.UseSqlite(mConnectionString);
                    break;
                case DatabaseUtils.DbProvider.MySql:
                    optionsBuilder.UseMySql(mConnectionString);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefreshToken>().HasOne(token => token.User);

            modelBuilder.Entity<Ban>().HasOne(b => b.User);
            modelBuilder.Entity<Mute>().HasOne(b => b.User);

            modelBuilder.Entity<User>().HasMany(b => b.Players).WithOne(p => p.User);

            modelBuilder.Entity<Player>().HasMany(b => b.Friends).WithOne(p => p.Owner);

            modelBuilder.Entity<Player>().HasMany(b => b.Spells).WithOne(p => p.Player);

            modelBuilder.Entity<Player>().HasMany(b => b.Items).WithOne(p => p.Player);

            modelBuilder.Entity<Player>().HasMany(b => b.Variables).WithOne(p => p.Player);
            modelBuilder.Entity<Variable>().HasIndex(p => new { p.VariableId, CharacterId = p.PlayerId }).IsUnique();

            modelBuilder.Entity<Player>().HasMany(b => b.Hotbar).WithOne(p => p.Player);

            modelBuilder.Entity<Player>().HasMany(b => b.Quests).WithOne(p => p.Player);
            modelBuilder.Entity<Quest>().HasIndex(p => new { p.QuestId, CharacterId = p.PlayerId }).IsUnique();

            modelBuilder.Entity<Player>().HasMany(b => b.Bank).WithOne(p => p.Player);

            modelBuilder.Entity<Bag>().HasMany(b => b.Slots).WithOne(p => p.ParentBag).HasForeignKey(p => p.ParentBagId);

            modelBuilder.Entity<InventorySlot>().HasOne(b => b.Bag);
            modelBuilder.Entity<BagSlot>().HasOne(b => b.Bag);
            modelBuilder.Entity<BankSlot>().HasOne(b => b.Bag);
        }

        public bool IsEmpty()
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                if (mConnection == DatabaseUtils.DbProvider.MySql)
                {
                    command.CommandText = "show tables;";
                }
                else if (mConnection == DatabaseUtils.DbProvider.Sqlite)
                {
                    command.CommandText = "SELECT name FROM sqlite_master WHERE type='table';";
                }
                command.CommandType = CommandType.Text;

                Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    return !result.HasRows;
                }
            }
        }

        public DbSet<TType> GetDbSet<TType>() where TType : class
        {
            var searchType = typeof(DbSet<TType>);
            var property = GetType()
                .GetProperties()
                .FirstOrDefault(propertyInfo => searchType == propertyInfo?.PropertyType);
            return property?.GetValue(this) as DbSet<TType>;
        }

        public void Seed()
        {
#if DEBUG
            new SeedUsers().SeedIfEmpty(this);

            SaveChanges();
#endif
        }

        public void MigrationsProcessed(string[] migrations)
        {

        }
    }
}