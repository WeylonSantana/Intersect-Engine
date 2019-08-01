﻿using System.Collections.Generic;
using System.IO;
using Intersect.Config;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Intersect
{
    public class Options
    {
        #region Constants

        // TODO: Clean these up
        //Values that cannot easily be changed:
        public const int LayerCount = 5;
        public const int MaxStats = 5;
        public const int MaxHotbar = 10;

        public const string DEFAULT_GAME_NAME = "Intersect";
        public const int DEFAULT_SERVER_PORT = 5400;

        #endregion

        [NotNull]
        public static Options Instance { get; private set; }

        [JsonIgnore]
        public bool SendingToClient { get; set; } = true;

        //Public Getters
        public static ushort ServerPort { get => Instance._serverPort; set => Instance._serverPort = value; }
        public static int MaxStatValue => Instance.PlayerOpts.MaxStat;
        public static int MaxLevel => Instance.PlayerOpts.MaxLevel;
        public static int MaxInvItems => Instance.PlayerOpts.MaxInventory;
        public static int MaxPlayerSkills => Instance.PlayerOpts.MaxSpells;
        public static int MaxBankSlots => Instance.PlayerOpts.MaxBank;
        public static int MaxCharacters => Instance.PlayerOpts.MaxCharacters;
        public static int ItemDropChance => Instance.PlayerOpts.ItemDropChance;
        public static int WeaponIndex => Instance.EquipmentOpts.WeaponSlot;
        public static int ShieldIndex => Instance.EquipmentOpts.ShieldSlot;
        public static List<string> EquipmentSlots => Instance.EquipmentOpts.Slots;
        public static List<string>[] PaperdollOrder => Instance.EquipmentOpts.Paperdoll.Directions;
        public static List<string> ToolTypes => Instance.EquipmentOpts.ToolTypes;
        public static List<string> AnimatedSprites => Instance._animatedSprites;
        public static int RegenTime => Instance.CombatOpts.RegenTime;
        public static int MinAttackRate => Instance.CombatOpts.MinAttackRate;
        public static int MaxAttackRate => Instance.CombatOpts.MaxAttackRate;
        public static int BlockingSlow => Instance.CombatOpts.BlockingSlow;
        public static int MaxDashSpeed => Instance.CombatOpts.MaxDashSpeed;
        public static int GameBorderStyle => Instance.MapOpts.GameBorderStyle;
        public static int ItemRepawnTime => Instance.MapOpts.ItemSpawnTime;
        public static int ItemDespawnTime => Instance.MapOpts.ItemDespawnTime;
        public static bool ZDimensionVisible => Instance.MapOpts.ZDimensionVisible;
        public static int MapWidth => Instance?.MapOpts?.Width ?? 32;
        public static int MapHeight => Instance?.MapOpts?.Height ?? 26;
        public static int TileWidth => Instance.MapOpts.TileWidth;
        public static int TileHeight => Instance.MapOpts.TileHeight;
        public static int EventWatchdogKillThreshhold => Instance.EventKillTheshhold;

        public static bool UPnP => Instance._upnp;

        public static bool NoPunchthrough { get; set; }
        public static bool NoNetworkCheck { get; set; }

        public static bool OpenPortChecker => Instance._portChecker;
        public static SmtpSettings Smtp => Instance.SmtpSettings;
        public static int PasswordResetExpirationMinutes => Instance._passResetExpirationMin;
        public static bool AdminOnly { get => Instance._adminOnly; set => Instance._adminOnly = value; }

        public static DatabaseOptions PlayerDb
        {
            get => Instance.PlayerDatabase;
            set => Instance.PlayerDatabase = value;
        }

        public static DatabaseOptions GameDb
        {
            get => Instance.GameDatabase;
            set => Instance.GameDatabase = value;
        }


        [NotNull]
        public static PlayerOptions Player => Instance.PlayerOpts;

        [NotNull]
        public static EquipmentOptions Equipment => Instance.EquipmentOpts;

        [NotNull]
        public static CombatOptions Combat => Instance.CombatOpts;

        [NotNull]
        public static MapOptions Map => Instance.MapOpts;

        public static bool Loaded => Instance != null;

        [JsonProperty("GameName", Order = -5)]
        public string GameName { get; set; } = DEFAULT_GAME_NAME;

        [JsonProperty("ServerPort", Order = -4)]
        public ushort _serverPort { get; set; } = DEFAULT_SERVER_PORT;

        [JsonProperty("AdminOnly", Order = -3)]
        protected bool _adminOnly = false;

        [JsonProperty("UPnP", Order = -2)]
        protected bool _upnp = true;

        [JsonProperty("OpenPortChecker", Order = -1)]
        protected bool _portChecker = true;

        [JsonProperty ("Player")]
        public PlayerOptions PlayerOpts = new PlayerOptions();

        /// <summary>
        /// Passability configuration by map zone
        /// </summary>
        [NotNull] public Passability Passability { get; } = new Passability();

        [JsonProperty("Equipment")]
        public EquipmentOptions EquipmentOpts = new EquipmentOptions();


        //Constantly Animated Sprites
        [JsonProperty("AnimatedSprites")]
        protected List<string> _animatedSprites = new List<string>();

        [JsonProperty("Combat")]
        public CombatOptions CombatOpts = new CombatOptions();

        [JsonProperty("Map")]
        public MapOptions MapOpts = new MapOptions();

        [JsonProperty("EventWatchdogKillThreshold")]
        public int EventKillTheshhold = 5000;

        [JsonProperty("ValidPasswordResetTimeMinutes")]
        protected ushort _passResetExpirationMin = 30;

        public bool SmtpValid => Smtp.IsValid();
        public SmtpSettings SmtpSettings = new SmtpSettings();
        public DatabaseOptions PlayerDatabase = new DatabaseOptions();
        public DatabaseOptions GameDatabase = new DatabaseOptions();

        public void FixAnimatedSprites()
        {
            for (int i = 0; i < _animatedSprites.Count; i++)
                _animatedSprites[i] = _animatedSprites[i].ToLower();
        }


        //Caching Json
        private static string optionsCompressed = "";

        public static bool LoadFromDisk()
        {
            Instance = new Options();
            if (!Directory.Exists("resources")) Directory.CreateDirectory("resources");
            if (File.Exists("resources/config.json"))
            {
                Instance = JsonConvert.DeserializeObject<Options>(File.ReadAllText("resources/config.json"));
            }
            Instance.SendingToClient = false;
            Instance.FixAnimatedSprites();
            File.WriteAllText("resources/config.json", JsonConvert.SerializeObject(Instance,Formatting.Indented));
            Instance.SendingToClient = true;
            optionsCompressed = JsonConvert.SerializeObject(Instance);
            return true;
        }

        public static void SaveToDisk()
        {
            Instance.SendingToClient = false;
            File.WriteAllText("resources/config.json", JsonConvert.SerializeObject(Instance, Formatting.Indented));
            Instance.SendingToClient = true;
            optionsCompressed = JsonConvert.SerializeObject(Instance);
        }

        public static string OptionsData => optionsCompressed;

        public static void LoadFromServer(string data)
        {
            Instance = JsonConvert.DeserializeObject<Options>(data);
        }

        // ReSharper disable once UnusedMember.Global
        public bool ShouldSerializePlayerDatabase()
        {
            return !SendingToClient;
        }

        // ReSharper disable once UnusedMember.Global
        public bool ShouldSerializeGameDatabase()
        {
            return !SendingToClient;
        }

        // ReSharper disable once UnusedMember.Global
        public bool ShouldSerializeSmtpSettings()
        {
            return !SendingToClient;
        }

        // ReSharper disable once UnusedMember.Global
        public bool ShouldSerializeSmtpValid()
        {
            return SendingToClient;
        }
    }
}