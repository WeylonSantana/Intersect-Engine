﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Web.UI;
using Intersect.Enums;
using Intersect.GameObjects;
using Intersect.GameObjects.Events;
using Intersect.GameObjects.Maps;
using Intersect.Logging;
using Intersect.Network.Packets.Server;
using Intersect.Server.Database;
using Intersect.Server.Database.PlayerData.Players;
using Intersect.Server.Entities.Combat;
using Intersect.Server.Entities.Events;
using Intersect.Server.General;
using Intersect.Server.Localization;
using Intersect.Server.Maps;
using Intersect.Server.Networking;
using Intersect.Utilities;

using Newtonsoft.Json;

namespace Intersect.Server.Entities
{

    public partial class Entity : IDisposable
    {

        //Instance Values
        private Guid _id;

        public Guid MapInstanceId = Guid.Empty;

        [JsonProperty("MaxVitals"), NotMapped] private int[] _maxVital = new int[(int) Vitals.VitalCount];

        [NotMapped, JsonIgnore] public Stat[] Stat = new Stat[(int) Stats.StatCount];

        [NotMapped, JsonIgnore] public Entity Target { get; set; } = null;

        public Entity() : this(Guid.NewGuid(), Guid.Empty)
        {
        }

        //Initialization
        public Entity(Guid instanceId, Guid mapInstanceId)
        {
            if (!(this is EventPageInstance) && !(this is Projectile))
            {
                for (var i = 0; i < (int)Stats.StatCount; i++)
                {
                    Stat[i] = new Stat((Stats)i, this);
                }
            }
            MapInstanceId = mapInstanceId;

            Id = instanceId;
        }

        [Column(Order = 1), JsonProperty(Order = -2)]
        public string Name { get; set; }

        public Guid MapId { get; set; }

        [NotMapped]
        public string MapName => MapController.GetName(MapId);

        [JsonIgnore]
        [NotMapped]
        public MapController Map => MapController.Get(MapId);

        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

        public int Dir { get; set; }

        public string Sprite { get; set; }

        /// <summary>
        /// The database compatible version of <see cref="Color"/>
        /// </summary>
        [JsonIgnore, Column(nameof(Color))]
        public string JsonColor
        {
            get => JsonConvert.SerializeObject(Color);
            set => Color = !string.IsNullOrWhiteSpace(value) ? JsonConvert.DeserializeObject<Color>(value) : Color.White;
        }

        /// <summary>
        /// Defines the ARGB color settings for this Entity.
        /// </summary>
        [NotMapped]
        public Color Color { get; set; } = new Color(255, 255, 255, 255);

        public string Face { get; set; }

        public int Level { get; set; }

        [JsonIgnore, Column("Vitals")]
        public string VitalsJson
        {
            get => DatabaseUtils.SaveIntArray(mVitals, (int) Enums.Vitals.VitalCount);
            set => mVitals = DatabaseUtils.LoadIntArray(value, (int) Enums.Vitals.VitalCount);
        }

        [JsonProperty("Vitals"), NotMapped]
        private int[] mVitals { get; set; } = new int[(int) Enums.Vitals.VitalCount];

        [JsonIgnore, NotMapped]
        private int[] mOldVitals { get; set; } = new int[(int)Enums.Vitals.VitalCount];

        [JsonIgnore, NotMapped]
        private int[] mOldMaxVitals { get; set; } = new int[(int)Enums.Vitals.VitalCount];

        //Stats based on npc settings, class settings, etc for quick calculations
        [JsonIgnore, Column(nameof(BaseStats))]
        public string StatsJson
        {
            get => DatabaseUtils.SaveIntArray(BaseStats, (int) Enums.Stats.StatCount);
            set => BaseStats = DatabaseUtils.LoadIntArray(value, (int) Enums.Stats.StatCount);
        }

        [NotMapped]
        public int[] BaseStats { get; set; } =
            new int[(int) Enums.Stats
                .StatCount]; // TODO: Why can this be BaseStats while Vitals is _vital and MaxVitals is _maxVital?

        [JsonIgnore, Column(nameof(StatPointAllocations))]
        public string StatPointsJson
        {
            get => DatabaseUtils.SaveIntArray(StatPointAllocations, (int) Enums.Stats.StatCount);
            set => StatPointAllocations = DatabaseUtils.LoadIntArray(value, (int) Enums.Stats.StatCount);
        }

        [NotMapped]
        public int[] StatPointAllocations { get; set; } = new int[(int) Enums.Stats.StatCount];

        //Inventory
        [JsonIgnore]
        public virtual List<InventorySlot> Items { get; set; } = new List<InventorySlot>();

        //Spells
        [JsonIgnore]
        public virtual List<SpellSlot> Spells { get; set; } = new List<SpellSlot>();

        [JsonIgnore, Column(nameof(NameColor))]
        public string NameColorJson
        {
            get => DatabaseUtils.SaveColor(NameColor);
            set => NameColor = DatabaseUtils.LoadColor(value);
        }

        [NotMapped]
        public Color NameColor { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 0)]
        public Guid Id { get => _id; set => _id = value; }

        [NotMapped]
        public Label HeaderLabel { get; set; }

        [JsonIgnore, Column(nameof(HeaderLabel))]
        public string HeaderLabelJson
        {
            get => JsonConvert.SerializeObject(HeaderLabel);
            set => HeaderLabel = value != null ? JsonConvert.DeserializeObject<Label>(value) : new Label();
        }

        [NotMapped]
        public Label FooterLabel { get; set; }

        [JsonIgnore, Column(nameof(FooterLabel))]
        public string FooterLabelJson
        {
            get => JsonConvert.SerializeObject(FooterLabel);
            set => FooterLabel = value != null ? JsonConvert.DeserializeObject<Label>(value) : new Label();
        }

        [NotMapped]
        public bool Dead { get; set; }

        //Combat
        [NotMapped, JsonIgnore]
        public long CastTime { get; set; }

        [NotMapped, JsonIgnore]
        public long AttackTimer { get; set; }

        [NotMapped, JsonIgnore]
        public bool Blocking { get; set; }

        [NotMapped, JsonIgnore]
        public Entity CastTarget { get; set; }

        [NotMapped, JsonIgnore]
        public Guid CollisionIndex { get; set; }

        [NotMapped, JsonIgnore]
        public long CombatTimer { get; set; }

        //Visuals
        [NotMapped, JsonIgnore]
        public bool HideName { get; set; }

        [NotMapped, JsonIgnore]
        public bool HideEntity { get; set; } = false;

        [NotMapped, JsonIgnore]
        public List<Guid> Animations { get; set; } = new List<Guid>();

        //DoT/HoT Spells
        [NotMapped, JsonIgnore]
        public ConcurrentDictionary<Guid, DoT> DoT { get; set; } = new ConcurrentDictionary<Guid, DoT>();

        [NotMapped, JsonIgnore]
        public DoT[] CachedDots { get; set; } = new DoT[0];

        [NotMapped, JsonIgnore]
        public EventMoveRoute MoveRoute { get; set; } = null;

        [NotMapped, JsonIgnore]
        public EventPageInstance MoveRouteSetter { get; set; } = null;

        [NotMapped, JsonIgnore]
        public long MoveTimer { get; set; }

        [NotMapped, JsonIgnore]
        public bool Passable { get; set; } = false;

        [NotMapped, JsonIgnore]
        public long RegenTimer { get; set; } = Globals.Timing.Milliseconds;

        [NotMapped, JsonIgnore]
        public int SpellCastSlot { get; set; } = 0;

        //Status effects
        [NotMapped, JsonIgnore]
        public ConcurrentDictionary<SpellBase, Status> Statuses { get; } = new ConcurrentDictionary<SpellBase, Status>();

        [JsonIgnore, NotMapped]
        public Status[] CachedStatuses = new Status[0];

        [JsonIgnore, NotMapped]
        private Status[] mOldStatuses = new Status[0];

        [JsonIgnore, NotMapped]
        public Dictionary<Immunities, bool> ImmuneTo = new Dictionary<Immunities, bool>();

        [NotMapped, JsonIgnore]
        public bool IsDisposed { get; protected set; }

        [NotMapped, JsonIgnore]
        public object EntityLock = new object();

        [NotMapped, JsonIgnore]
        public Guid DeathAnimation = Guid.Empty;

        [NotMapped, JsonIgnore]
        public bool VitalsUpdated
        {
            get => !GetVitals().SequenceEqual(mOldVitals) || !GetMaxVitals().SequenceEqual(mOldMaxVitals);

            set
            {
                if (value == false)
                {
                    mOldVitals = GetVitals();
                    mOldMaxVitals = GetMaxVitals();
                }
            }
        }

        [NotMapped, JsonIgnore]
        public bool StatusesUpdated
        {
            get => CachedStatuses != mOldStatuses; //The whole CachedStatuses assignment gets changed when a status is added, removed, or updated (time remaining changes, so we only check for reference equivity here)

            set
            {
                if (value == false)
                {
                    mOldStatuses = CachedStatuses;
                }
            }
        }

        public virtual void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
            }
        }

        public virtual void Update(long timeMs)
        {
            var lockObtained = false;
            try
            {
                Monitor.TryEnter(EntityLock, ref lockObtained);
                if (lockObtained)
                {
                    //Cast timers
                    if (CastTime != 0 && CastTime < timeMs)
                    {
                        CastTime = 0;
                        CastSpell(Spells[SpellCastSlot].SpellId, SpellCastSlot);
                        CastTarget = null;
                    }

                    //DoT/HoT timers
                    foreach (var dot in CachedDots)
                    {
                        dot.Tick();
                    }

                    var statsUpdated = false;
                    var statTime = Globals.Timing.Milliseconds;
                    for (var i = 0; i < (int)Stats.StatCount; i++)
                    {
                        statsUpdated |= Stat[i].Update(statTime);
                    }

                    if (statsUpdated)
                    {
                        PacketSender.SendEntityStats(this);
                    }

                    //Regen Timers
                    if (timeMs > CombatTimer && timeMs > RegenTimer)
                    {
                        ProcessRegen();
                        RegenTimer = timeMs + Options.RegenTime;
                    }

                    //Status timers
                    var statusArray = CachedStatuses;
                    foreach (var status in statusArray)
                    {
                        status.TryRemoveStatus();
                    }
                }
            }
            finally
            {
                if (lockObtained)
                {
                    Monitor.Exit(EntityLock);
                }
            }
        }

        //Movement
        /// <summary>
        ///     Determines if this entity can move in the direction given.
        ///     Returns -5 if the tile is completely out of bounds.
        ///     Returns -3 if a tile is blocked because of a Z dimension tile
        ///     Returns -2 if a tile is blocked by a map attribute.
        ///     Returns -1 for clear.
        ///     Returns the type of entity that is blocking the way (if one exists)
        /// </summary>
        /// <param name="moveDir"></param>
        /// <returns></returns>
        public virtual int CanMove(int moveDir)
        {
            var xOffset = 0;
            var yOffset = 0;

            // If this is an Npc that has the Static behaviour, it can NEVER move.
            if (this is Npc npc)
            {
                if (npc.Base.Movement == (byte) NpcMovement.Static)
                {
                    return -2;
                }
            }

            var tile = new TileHelper(MapId, X, Y);
            switch (moveDir)
            {
                case 0: //Up
                    yOffset--;

                    break;
                case 1: //Down
                    yOffset++;

                    break;
                case 2: //Left
                    xOffset--;

                    break;
                case 3: //Right
                    xOffset++;

                    break;
                case 4: //NW
                    yOffset--;
                    xOffset--;

                    break;
                case 5: //NE
                    yOffset--;
                    xOffset++;

                    break;
                case 6: //SW
                    yOffset++;
                    xOffset--;

                    break;
                case 7: //SE
                    yOffset++;
                    xOffset++;

                    break;
            }

            MapController mapController = null;
            int tileX = 0;
            int tileY = 0;

            if (tile.Translate(xOffset, yOffset))
            {
                mapController = MapController.Get(tile.GetMapId());
                tileX = tile.GetX();
                tileY = tile.GetY();
                var tileAttribute = mapController.Attributes[tileX, tileY];
                if (tileAttribute != null)
                {
                    if (tileAttribute.Type == MapAttributes.Blocked || (tileAttribute.Type == MapAttributes.Animation && ((MapAnimationAttribute)tileAttribute).IsBlock))
                    {
                        return -2;
                    }

                    if (tileAttribute.Type == MapAttributes.NpcAvoid && this is Npc)
                    {
                        return -2;
                    }

                    if (tileAttribute.Type == MapAttributes.ZDimension &&
                        ((MapZDimensionAttribute) tileAttribute).BlockedLevel > 0 &&
                        ((MapZDimensionAttribute) tileAttribute).BlockedLevel - 1 == Z)
                    {
                        return -3;
                    }

                    if (tileAttribute.Type == MapAttributes.Slide)
                    {
                        if (this is EventPage)
                        {
                            return -4;
                        }

                        switch (((MapSlideAttribute) tileAttribute).Direction)
                        {
                            case 1:
                                if (moveDir == 1)
                                {
                                    return -4;
                                }

                                break;
                            case 2:
                                if (moveDir == 0)
                                {
                                    return -4;
                                }

                                break;
                            case 3:
                                if (moveDir == 3)
                                {
                                    return -4;
                                }

                                break;
                            case 4:
                                if (moveDir == 2)
                                {
                                    return -4;
                                }

                                break;
                        }
                    }
                }
            }
            else
            {
                return -5; //Out of Bounds
            }

            if (!Passable)
            {
                var targetMap = mapController;
                var mapEntities = new List<Entity>();
                if (mapController.TryGetInstance(MapInstanceId, out var mapInstance))
                {
                    mapEntities.AddRange(mapInstance.GetCachedEntities());
                }
                foreach (var en in mapEntities)
                {
                    if (en != null && en.X == tileX && en.Y == tileY && en.Z == Z && !en.Passable)
                    {
                        //Set a target if a projectile
                        CollisionIndex = en.Id;
                        if (en is Player)
                        {
                            if (this is Player)
                            {
                                //Check if this target player is passable....
                                if (!Options.Instance.Passability.Passable[(int)targetMap.ZoneType])
                                {
                                    return (int)EntityTypes.Player;
                                }
                            }
                            else
                            {
                                return (int)EntityTypes.Player;
                            }
                        }
                        else if (en is Npc)
                        {
                            return (int)EntityTypes.Player;
                        }
                        else if (en is Resource resource)
                        {
                            //If determine if we should walk
                            if (!resource.IsPassable())
                            {
                                return (int)EntityTypes.Resource;
                            }
                        }
                    }
                }

                //If this is an npc or other event.. if any global page exists that isn't passable then don't walk here!
                if (!(this is Player) && mapInstance != null)
                {
                    foreach (var evt in mapInstance.GlobalEventInstances)
                    {
                        foreach (var en in evt.Value.GlobalPageInstance)
                        {
                            if (en != null && en.X == tileX && en.Y == tileY && en.Z == Z && !en.Passable)
                            {
                                return (int)EntityTypes.Event;
                            }
                        }
                    }
                }
            }

            return IsTileWalkable(tile.GetMap(), tile.GetX(), tile.GetY(), Z);
        }

        protected virtual int IsTileWalkable(MapController map, int x, int y, int z)
        {
            //Out of bounds if no map
            if (map == null)
            {
                return -5;
            }

            //Otherwise fine
            return -1;
        }

        protected virtual bool ProcessMoveRoute(Player forPlayer, long timeMs)
        {
            var moved = false;
            byte lookDir = 0, moveDir = 0;
            if (MoveRoute.ActionIndex < MoveRoute.Actions.Count)
            {
                switch (MoveRoute.Actions[MoveRoute.ActionIndex].Type)
                {
                    case MoveRouteEnum.MoveUp:
                        if (CanMove((int) Directions.Up) == -1)
                        {
                            Move((int) Directions.Up, forPlayer, false, true);
                            moved = true;
                        }

                        break;
                    case MoveRouteEnum.MoveDown:
                        if (CanMove((int) Directions.Down) == -1)
                        {
                            Move((int) Directions.Down, forPlayer, false, true);
                            moved = true;
                        }

                        break;
                    case MoveRouteEnum.MoveLeft:
                        if (CanMove((int) Directions.Left) == -1)
                        {
                            Move((int) Directions.Left, forPlayer, false, true);
                            moved = true;
                        }

                        break;
                    case MoveRouteEnum.MoveRight:
                        if (CanMove((int) Directions.Right) == -1)
                        {
                            Move((int) Directions.Right, forPlayer, false, true);
                            moved = true;
                        }

                        break;
                    case MoveRouteEnum.MoveRandomly:
                        var dir = (byte)Randomization.Next(0, 4);
                        if (CanMove(dir) == -1)
                        {
                            Move(dir, forPlayer);
                            moved = true;
                        }

                        break;
                    case MoveRouteEnum.StepForward:
                        if (CanMove(Dir) > -1)
                        {
                            Move((byte) Dir, forPlayer);
                            moved = true;
                        }

                        break;
                    case MoveRouteEnum.StepBack:
                        switch (Dir)
                        {
                            case (int) Directions.Up:
                                moveDir = (int) Directions.Down;

                                break;
                            case (int) Directions.Down:
                                moveDir = (int) Directions.Up;

                                break;
                            case (int) Directions.Left:
                                moveDir = (int) Directions.Right;

                                break;
                            case (int) Directions.Right:
                                moveDir = (int) Directions.Left;

                                break;
                        }

                        if (CanMove(moveDir) > -1)
                        {
                            Move(moveDir, forPlayer);
                            moved = true;
                        }

                        break;
                    case MoveRouteEnum.FaceUp:
                        ChangeDir((int) Directions.Up);
                        moved = true;

                        break;
                    case MoveRouteEnum.FaceDown:
                        ChangeDir((int) Directions.Down);
                        moved = true;

                        break;
                    case MoveRouteEnum.FaceLeft:
                        ChangeDir((int) Directions.Left);
                        moved = true;

                        break;
                    case MoveRouteEnum.FaceRight:
                        ChangeDir((int) Directions.Right);
                        moved = true;

                        break;
                    case MoveRouteEnum.Turn90Clockwise:
                        switch (Dir)
                        {
                            case (int) Directions.Up:
                                lookDir = (int) Directions.Right;

                                break;
                            case (int) Directions.Down:
                                lookDir = (int) Directions.Left;

                                break;
                            case (int) Directions.Left:
                                lookDir = (int) Directions.Up;

                                break;
                            case (int) Directions.Right:
                                lookDir = (int) Directions.Down;

                                break;
                        }

                        ChangeDir(lookDir);
                        moved = true;

                        break;
                    case MoveRouteEnum.Turn90CounterClockwise:
                        switch (Dir)
                        {
                            case (int)Directions.Up:
                                lookDir = (int)Directions.Left;

                                break;
                            case (int)Directions.Down:
                                lookDir = (int)Directions.Right;

                                break;
                            case (int)Directions.Left:
                                lookDir = (int)Directions.Down;

                                break;
                            case (int)Directions.Right:
                                lookDir = (int)Directions.Up;

                                break;
                        }

                        ChangeDir(lookDir);
                        moved = true;

                        break;
                    case MoveRouteEnum.Turn180:
                        switch (Dir)
                        {
                            case (int) Directions.Up:
                                lookDir = (int) Directions.Down;

                                break;
                            case (int) Directions.Down:
                                lookDir = (int) Directions.Up;

                                break;
                            case (int) Directions.Left:
                                lookDir = (int) Directions.Right;

                                break;
                            case (int) Directions.Right:
                                lookDir = (int) Directions.Left;

                                break;
                        }

                        ChangeDir(lookDir);
                        moved = true;

                        break;
                    case MoveRouteEnum.TurnRandomly:
                        ChangeDir((byte)Randomization.Next(0, 4));
                        moved = true;

                        break;
                    case MoveRouteEnum.Wait100:
                        MoveTimer = Globals.Timing.Milliseconds + 100;
                        moved = true;

                        break;
                    case MoveRouteEnum.Wait500:
                        MoveTimer = Globals.Timing.Milliseconds + 500;
                        moved = true;

                        break;
                    case MoveRouteEnum.Wait1000:
                        MoveTimer = Globals.Timing.Milliseconds + 1000;
                        moved = true;

                        break;
                    default:
                        //Gonna end up returning false because command not found
                        return false;
                }

                if (moved || MoveRoute.IgnoreIfBlocked)
                {
                    MoveRoute.ActionIndex++;
                    if (MoveRoute.ActionIndex >= MoveRoute.Actions.Count)
                    {
                        if (MoveRoute.RepeatRoute)
                        {
                            MoveRoute.ActionIndex = 0;
                        }

                        MoveRoute.Complete = true;
                    }
                }

                if (moved && MoveTimer < Globals.Timing.Milliseconds)
                {
                    MoveTimer = Globals.Timing.Milliseconds + (long) GetMovementTime();
                }
            }

            return true;
        }

        public virtual bool IsPassable()
        {
            return Passable;
        }

        //Returns the amount of time required to traverse 1 tile
        public virtual float GetMovementTime()
        {
            var speed = Stat[(int)Stats.Speed].Value();
            if (this is Player player && player.InVehicle && player.VehicleSpeed > 0L)
            {
                speed = (int) player.VehicleSpeed;
            }

            var time = 1000f / (float)(1 + Math.Log(speed * Options.AgilityMovementSpeedModifier));
            if (Blocking)
            {
                time += time * (float)Options.BlockingSlow;
            }

            time *= (float)Options.SpeedModifier;

            return Math.Min(1000f, time);
        }

        public virtual EntityTypes GetEntityType()
        {
            return EntityTypes.GlobalEntity;
        }

        public virtual void Move(int moveDir, Player forPlayer, bool doNotUpdate = false, bool correction = false)
        {
            if (Globals.Timing.Milliseconds < MoveTimer || (!Options.Combat.MovementCancelsCast && CastTime > 0))
            {
                return;
            }

            lock (EntityLock)
            {
                if (this is Player && CastTime > 0 && Options.Combat.MovementCancelsCast)
                {
                    CastTime = 0;
                    CastTarget = null;
                }

                var xOffset = 0;
                var yOffset = 0;
                switch (moveDir)
                {
                    case 0: //Up
                        --yOffset;

                        break;
                    case 1: //Down
                        ++yOffset;

                        break;
                    case 2: //Left
                        --xOffset;

                        break;
                    case 3: //Right
                        ++xOffset;

                        break;

                    default:
                        Log.Warn(
                            new ArgumentOutOfRangeException(nameof(moveDir), $@"Bogus move attempt in direction {moveDir}.")
                        );

                        return;
                }

                Dir = moveDir;


                var tile = new TileHelper(MapId, X, Y);

                // ReSharper disable once InvertIf
                if (tile.Translate(xOffset, yOffset))
                {
                    X = tile.GetX();
                    Y = tile.GetY();

                    var currentMap = MapController.Get(tile.GetMapId());
                    if (MapId != tile.GetMapId())
                    {
                        var oldMap = MapController.Get(MapId);
                        if (oldMap.TryGetInstance(MapInstanceId, out var oldInstance)) {
                            oldInstance.RemoveEntity(this);
                        }

                        if (currentMap.TryGetInstance(MapInstanceId, out var newInstance))
                        {
                            newInstance.AddEntity(this);
                        }

                        //Send Left Map Packet To the Maps that we are no longer with
                        var oldMaps = oldMap?.GetSurroundingMaps(true);
                        var newMaps = currentMap?.GetSurroundingMaps(true);

                        MapId = tile.GetMapId();

                        if (oldMaps != null)
                        {
                            foreach (var map in oldMaps)
                            {
                                if (newMaps == null || !newMaps.Contains(map))
                                {
                                    PacketSender.SendEntityLeaveMap(this, map.Id);
                                }
                            }
                        }


                        if (newMaps != null)
                        {
                            foreach (var map in newMaps)
                            {
                                if (oldMaps == null || !oldMaps.Contains(map))
                                {
                                    PacketSender.SendEntityDataToMap(this, map, this as Player);
                                }
                            }
                        }

                    }



                    if (doNotUpdate == false)
                    {
                        if (this is EventPageInstance)
                        {
                            if (forPlayer != null)
                            {
                                PacketSender.SendEntityMoveTo(forPlayer, this, correction);
                            }
                            else
                            {
                                PacketSender.SendEntityMove(this, correction);
                            }
                        }
                        else
                        {
                            PacketSender.SendEntityMove(this, correction);
                        }

                        //Check if moving into a projectile.. if so this npc needs to be hit
                        if (currentMap != null)
                        {
                            foreach (var instance in MapController.GetSurroundingMapInstances(currentMap.Id, MapInstanceId, true))
                            {
                                var projectiles = instance.MapProjectilesCached;
                                foreach (var projectile in projectiles)
                                {
                                    var spawns = projectile?.Spawns?.ToArray() ?? Array.Empty<ProjectileSpawn>();
                                    foreach (var spawn in spawns)
                                    {
                                        // TODO: Filter in Spawns variable, there should be no nulls. See #78 for evidence it is null.
                                        if (spawn == null)
                                        {
                                            continue;
                                        }

                                        if (spawn.IsAtLocation(MapId, X, Y, Z) && spawn.HitEntity(this))
                                        {
                                            spawn.Dead = true;
                                        }
                                    }
                                }
                            }
                        }

                        MoveTimer = Globals.Timing.Milliseconds + (long)GetMovementTime();
                    }

                    if (TryToChangeDimension() && doNotUpdate == true)
                    {
                        PacketSender.UpdateEntityZDimension(this, (byte)Z);
                    }

                    //Check for traps
                    if (MapController.TryGetInstanceFromMap(currentMap.Id, MapInstanceId, out var mapInstance))
                    {
                        foreach (var trap in mapInstance.MapTrapsCached)
                        {
                            trap.CheckEntityHasDetonatedTrap(this);
                        }
                    }

                    // TODO: Why was this scoped to only Event entities?
                    //                if (currentMap != null && this is EventPageInstance)
                    var attribute = currentMap?.Attributes[X, Y];

                    // ReSharper disable once InvertIf
                    //Check for slide tiles
                    if (attribute?.Type == MapAttributes.Slide)
                    {
                        // If sets direction, set it.
                        if (((MapSlideAttribute)attribute).Direction > 0)
                        {
                            //Check for slide tiles
                            if (attribute != null && attribute.Type == MapAttributes.Slide)
                            {
                                if (((MapSlideAttribute)attribute).Direction > 0)
                                {
                                    Dir = (byte)(((MapSlideAttribute)attribute).Direction - 1);
                                }
                            }
                        }

                        var dash = new Dash(this, 1, (byte)Dir);
                    }
                }
            }
        }

        public void ChangeDir(int dir)
        {
            if (dir == -1)
            {
                return;
            }

            if (Dir != dir)
            {
                Dir = dir;

                if (this is EventPageInstance eventPageInstance && eventPageInstance.Player != null)
                {
                    if (((EventPageInstance)this).Player != null)
                    {
                        PacketSender.SendEntityDirTo(((EventPageInstance)this).Player, this);
                    }
                    else
                    {
                        PacketSender.SendEntityDir(this);
                    }
                }
                else
                {
                    PacketSender.SendEntityDir(this);
                }
            }

            if (this is Player player && player.resourceLock != null)
            {
                player.setResourceLock(false);
            }
        }

        // Change the dimension if the player is on a gateway
        public bool TryToChangeDimension()
        {
            if (X < Options.MapWidth && X >= 0)
            {
                if (Y < Options.MapHeight && Y >= 0)
                {
                    var attribute = MapController.Get(MapId).Attributes[X, Y];
                    if (attribute != null && attribute.Type == MapAttributes.ZDimension)
                    {
                        if (((MapZDimensionAttribute) attribute).GatewayTo > 0)
                        {
                            Z = (byte) (((MapZDimensionAttribute) attribute).GatewayTo - 1);

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        //Misc
        public int GetDirectionTo(Entity target)
        {
            int xDiff = 0, yDiff = 0;

            var map = MapController.Get(MapId);
            var gridId = map.MapGrid;
            var grid = DbInterface.GetGrid(gridId);

            //Loop through surrouding maps to generate a array of open and blocked points.
            for (var x = map.MapGridX - 1; x <= map.MapGridX + 1; x++)
            {
                if (x == -1 || x >= grid.Width)
                {
                    continue;
                }

                for (var y = map.MapGridY - 1; y <= map.MapGridY + 1; y++)
                {
                    if (y == -1 || y >= grid.Height)
                    {
                        continue;
                    }

                    if (grid.MyGrid[x, y] != Guid.Empty &&
                        grid.MyGrid[x, y] == target.MapId)
                    {
                        xDiff = (x - map.MapGridX) * Options.MapWidth + target.X - X;
                        yDiff = (y - map.MapGridY) * Options.MapHeight + target.Y - Y;
                        if (Math.Abs(xDiff) > Math.Abs(yDiff))
                        {
                            if (xDiff < 0)
                            {
                                return (int) Directions.Left;
                            }

                            if (xDiff > 0)
                            {
                                return (int) Directions.Right;
                            }
                        }
                        else
                        {
                            if (yDiff < 0)
                            {
                                return (int) Directions.Up;
                            }

                            if (yDiff > 0)
                            {
                                return (int) Directions.Down;
                            }
                        }
                    }
                }
            }

            return -1;
        }

        //Combat
        public virtual int CalculateAttackTime()
        {
            return (int) (Options.MaxAttackRate +
                          (float) ((Options.MinAttackRate - Options.MaxAttackRate) *
                                   (((float) Options.MaxStatValue - Stat[(int) Stats.Speed].Value()) /
                                    (float) Options.MaxStatValue)));
        }

        public void TryBlock(bool blocking)
        {
            // Seeing as blocking doesn't... do anything, let's just get rid of this.
            /*if (AttackTimer < Globals.Timing.Milliseconds)
            {
                if (blocking && !Blocking && AttackTimer < Globals.Timing.Milliseconds)
                {
                    Blocking = true;
                    PacketSender.SendEntityAttack(this, -1);
                }
                else if (!blocking && Blocking)
                {
                    Blocking = false;
                    AttackTimer = Globals.Timing.Milliseconds + CalculateAttackTime();
                    PacketSender.SendEntityAttack(this, 0);
                }
            }*/
        }

        public virtual int GetWeaponDamage()
        {
            return 0;
        }

        public virtual bool CanAttack(Entity entity, SpellBase spell)
        {
            return CastTime <= 0;
        }

        public virtual void ProcessRegen()
        {
        }

        public int GetVital(int vital)
        {
            return mVitals[vital];
        }

        public int[] GetVitals()
        {
            var vitals = new int[(int) Vitals.VitalCount];
            Array.Copy(mVitals, 0, vitals, 0, (int) Vitals.VitalCount);

            return vitals;
        }

        public int GetVital(Vitals vital)
        {
            return GetVital((int) vital);
        }

        public void SetVital(int vital, int value)
        {
            if (value < 0)
            {
                value = 0;
            }

            if (GetMaxVital(vital) < value)
            {
                value = GetMaxVital(vital);
            }

            if (vital == (int)Vitals.Health && this is Player player)
            {
                player.CheckForHPWarning(value);
            }

            mVitals[vital] = value;
        }

        

        public void SetVital(Vitals vital, int value)
        {
            SetVital((int) vital, value);
        }

        public virtual int GetMaxVital(int vital)
        {
            return _maxVital[vital];
        }

        public virtual int GetMaxVital(Vitals vital)
        {
            return GetMaxVital((int) vital);
        }

        public int[] GetMaxVitals()
        {
            var vitals = new int[(int) Vitals.VitalCount];
            for (var vitalIndex = 0; vitalIndex < vitals.Length; ++vitalIndex)
            {
                vitals[vitalIndex] = GetMaxVital(vitalIndex);
            }

            return vitals;
        }

        public void SetMaxVital(int vital, int value)
        {
            if (value <= 0 && vital == (int) Vitals.Health)
            {
                value = 1; //Must have at least 1 hp
            }

            if (value < 0 && vital == (int) Vitals.Mana)
            {
                value = 0; //Can't have less than 0 mana
            }

            _maxVital[vital] = value;
            if (value < GetVital(vital))
            {
                SetVital(vital, value);
            }
        }

        public void SetMaxVital(Vitals vital, int value)
        {
            SetMaxVital((int) vital, value);
        }

        public bool HasVital(Vitals vital)
        {
            return GetVital(vital) > 0;
        }

        public bool IsFullVital(Vitals vital)
        {
            return GetVital(vital) == GetMaxVital(vital);
        }

        //Vitals
        public void RestoreVital(Vitals vital)
        {
            SetVital(vital, GetMaxVital(vital));
        }

        public void AddVital(Vitals vital, int amount)
        {
            if (vital >= Vitals.VitalCount)
            {
                return;
            }

            var vitalId = (int) vital;
            var maxVitalValue = GetMaxVital(vitalId);
            var safeAmount = Math.Min(amount, int.MaxValue - maxVitalValue);
            SetVital(vital, GetVital(vital) + safeAmount);
        }

        public void SubVital(Vitals vital, int amount)
        {
            if (vital >= Vitals.VitalCount)
            {
                return;
            }

            //Check for any shields.
            foreach (var status in CachedStatuses)
            {
                if (status.Type == StatusTypes.Shield)
                {
                    status.DamageShield(vital, ref amount);
                }
            }

            var vitalId = (int) vital;
            var maxVitalValue = GetMaxVital(vitalId);
            var safeAmount = Math.Min(amount, GetVital(vital));
            SetVital(vital, GetVital(vital) - safeAmount);
        }

        public virtual int[] GetStatValues()
        {
            var stats = new int[(int) Stats.StatCount];
            for (var i = 0; i < (int) Stats.StatCount; i++)
            {
                stats[i] = Stat[i].Value();
            }

            return stats;
        }

        public virtual bool IsAllyOf(Entity otherEntity)
        {
            return this == otherEntity;
        }

        //Attacking with projectile
        public virtual void TryAttack(
            Entity target,
            ProjectileBase projectile,
            SpellBase parentSpell,
            ItemBase parentItem,
            byte projectileDir
        )
        {
            if (target is Resource && parentSpell != null)
            {
                return;
            }

            //Check for taunt status and trying to attack a target that has not taunted you.
            foreach (var status in CachedStatuses)
            {
                if (status.Type == StatusTypes.Taunt)
                {
                    if (Target != target)
                    {
                        PacketSender.SendActionMsg(this, Strings.Combat.miss, CustomColors.Combat.Missed);

                        return;
                    }
                }
            }

            bool parentSpellMissed = false;
            if (parentSpell != null && target != null)
            {
                TryAttackSpell(target, parentSpell, out bool spellMissed, out bool spellBlocked, (sbyte) projectileDir, true);
                parentSpellMissed = spellMissed;
            }

            var targetPlayer = target as Player;
            var targetResource = target as Resource;

            if (this is Player player && targetPlayer != null)
            {
                //Player interaction common events
                if (projectile == null && parentSpell == null)
                {
                    targetPlayer.StartCommonEventsWithTrigger(CommonEventTrigger.PlayerInteract, "", this.Name);
                }

                if (MapController.Get(MapId).ZoneType == MapZones.Safe)
                {
                    return;
                }

                if (MapController.Get(target.MapId).ZoneType == MapZones.Safe)
                {
                    return;
                }

                if (player.InParty(targetPlayer))
                {
                    return;
                }
            }

            Dictionary<AttackFailures, bool> attackFailures = new Dictionary<AttackFailures, bool>();
            if (parentSpell == null && parentItem != null)
            {
                var deadAnimations = new List<KeyValuePair<Guid, sbyte>>();
                var aliveAnimations = new List<KeyValuePair<Guid, sbyte>>();
                
                if (parentItem.AttackAnimationId != Guid.Empty)
                {
                    deadAnimations.Add(new KeyValuePair<Guid, sbyte>(parentItem.AttackAnimationId, (sbyte)projectileDir));
                    aliveAnimations.Add(new KeyValuePair<Guid, sbyte>(parentItem.AttackAnimationId, (sbyte)projectileDir));
                }

                int damage = CalculateSpecialDamage(parentItem.Damage, parentItem, target);
                attackFailures = Attack(
                    target, damage, 0, (DamageType) parentItem.DamageType, (Stats) parentItem.ScalingStat,
                    parentItem.Scaling, parentItem.CritChance, parentItem.CritMultiplier, deadAnimations, aliveAnimations, true, false, true
                );
            }

            if ((attackFailures.TryGetValue(AttackFailures.MISSED, out var val) && val) || parentSpellMissed)
            {
                return; // do not further process the projectile, it missed.
            }

            //If projectile, check if a splash spell is applied
            if (projectile == null)
            {
                return;
            }

            if (projectile.SpellId != Guid.Empty)
            {
                var s = projectile.Spell;
                if (s != null)
                {
                    HandleAoESpell(projectile.SpellId, s.Combat.HitRadius, target.MapId, target.X, target.Y, null, true);
                }

                //Check that the npc has not been destroyed by the splash spell
                //TODO: Actually implement this, since null check is wrong.
                if (target == null)
                {
                    return;
                }
            }

            if (targetPlayer == null && !(target is Npc) || target.IsDead())
            {
                return;
            }

            //If there is a knockback, knock them backwards and make sure its linear (diagonal player movement not coded).
            if (projectile.Knockback > 0 && projectileDir < 4 && !target.IsImmuneTo(Immunities.Knockback))
            {
                var dash = new Dash(target, projectile.Knockback, projectileDir, false, false, false, false);
            }
        }

        private enum DamageBonus
        {
            None = 0,
            Backstab,
            Stealth
        };
        public int CalculateSpecialDamage(int baseDamage, ItemBase item, Entity target)
        {
            if (target is Resource) return baseDamage;
            if (item == null || target == null) return baseDamage;

            var damageBonus = DamageBonus.None;
            if (target.Dir == Dir) // Player is hitting something from behind
            {
                if (item.CanBackstab)
                {
                    baseDamage = (int)Math.Floor(baseDamage * item.BackstabMultiplier);
                    damageBonus = DamageBonus.Backstab;
                }
                if (this is Player player && player.StealthAttack && item.ProjectileId == Guid.Empty) // Melee weapons only for stealth attacks
                {
                    baseDamage += player.CalculateStealthDamage(baseDamage, item);
                    damageBonus = DamageBonus.Stealth;
                }

                if (damageBonus == DamageBonus.Backstab)
                {
                    PacketSender.SendActionMsg(target, Strings.Combat.backstab, CustomColors.Combat.Backstab);
                }
                else if (damageBonus == DamageBonus.Stealth)
                {
                    PacketSender.SendActionMsg(target, Strings.Combat.stealthattack, CustomColors.Combat.Backstab);
                }
            }

            return baseDamage;
        }

        //Attacking with spell
        /* Alex - Okay, so this WAS a `TryAttack` override. I had to add two out vars to it to handle spell missing and blocking
         * This was ONLY DONE to support the fact that a Projectile with a parent spell would call this method with the parent's spell base,
         * but then we needed to know the RESULT of the try attack, and I didn't want to rework try attack everywhere to support failures the
         * same way the Attack() method does, so... now we have _this_. HOWEVER, if we ever DO need to know attack missed/blocked anywhere else,
         * we can now!
        */
        public virtual void TryAttackSpell(
            Entity target,
            SpellBase spellBase,
            out bool spellMissed,
            out bool spellBlocked,
            sbyte attackAnimDir = (sbyte)Directions.Up,
            bool fromProjectile = false,
            bool onHitTrigger = false,
            bool trapTrigger = false
        )
        {
            spellMissed = false;
            spellBlocked = false;

            if (target is Resource)
            {
                return;
            }

            if (spellBase == null)
            {
                return;
            }

            //Check for taunt status and trying to attack a target that has not taunted you.
            if (!trapTrigger) //Traps ignore taunts.
            {
                foreach (var status in CachedStatuses)
                {
                    if (status.Type == StatusTypes.Taunt)
                    {
                        if (Target != target)
                        {
                            PacketSender.SendActionMsg(this, Strings.Combat.miss, CustomColors.Combat.Missed);

                            return;
                        }
                    }
                }
            }

            var deadAnimations = new List<KeyValuePair<Guid, sbyte>>();
            var aliveAnimations = new List<KeyValuePair<Guid, sbyte>>();

            //Only count safe zones and friendly fire if its a dangerous spell! (If one has been used)
            if (!spellBase.Combat.Friendly &&
                (spellBase.Combat.TargetType != (int) SpellTargetTypes.Self || onHitTrigger))
            {
                //If about to hit self with an unfriendly spell (maybe aoe?) return
                if (target == this && spellBase.Combat.Effect != StatusTypes.OnHit)
                {
                    return;
                }

                //Check for parties and safe zones, friendly fire off (unless its healing)
                if (target is Npc && this is Npc npc)
                {
                    if (!npc.CanNpcCombat(target, spellBase.Combat.Friendly))
                    {
                        return;
                    }
                }

                if (target is Player targetPlayer && this is Player player)
                {
                    if (player.IsAllyOf(targetPlayer))
                    {
                        return;
                    }

                    // Check if either the attacker or the defender is in a "safe zone" (Only apply if combat is PVP)
                    if (MapController.Get(MapId).ZoneType == MapZones.Safe)
                    {
                        return;
                    }

                    if (MapController.Get(target.MapId).ZoneType == MapZones.Safe)
                    {
                        return;
                    }
                }

                if (!CanAttack(target, spellBase))
                {
                    return;
                }
            }
            else
            {
                // Friendly Spell! Do not attack other players/npcs around us.
                /* switch (target)
                 {
                     case Player targetPlayer
                         when this is Player player && (!IsAllyOf(targetPlayer) || (MapController.Get(target.MapId)?.ZoneType == MapZones.Safe && MapController.Get(MapId)?.ZoneType == MapZones.Safe) ) && this != target:
                     case Npc _ when this is Npc npc && !npc.CanNpcCombat(target, spellBase.Combat.Friendly):
                         return;
                 }*/

                if (target is Player targetPlayer)
                {
                    if (! (MapController.Get(target.MapId)?.ZoneType == MapZones.Safe && MapController.Get(MapId)?.ZoneType == MapZones.Safe) )
                    {
                        if (!IsAllyOf(targetPlayer) && this != target)
                        {
                            return;
                        }
                    }
                } else if (target is Npc npc)
                {
                    if (!npc.CanNpcCombat(target, spellBase.Combat.Friendly))
                    {
                        return;
                    }
                }

                if (target?.GetType() != GetType())
                {
                    return; // Don't let players aoe heal npcs. Don't let npcs aoe heal players.
                }
            }

            if (spellBase.HitAnimationId != Guid.Empty &&
                (spellBase.Combat.Effect != StatusTypes.OnHit || onHitTrigger))
            {
                deadAnimations.Add(new KeyValuePair<Guid, sbyte>(spellBase.HitAnimationId, attackAnimDir));
                aliveAnimations.Add(new KeyValuePair<Guid, sbyte>(spellBase.HitAnimationId, attackAnimDir));
            }

            var statBuffTime = -1;
            var expireTime = Globals.Timing.Milliseconds + spellBase.Combat.Duration;
            for (var i = 0; i < (int) Stats.StatCount; i++)
            {
                target.Stat[i]
                    .AddBuff(
                        new Buff(spellBase, spellBase.Combat.StatDiff[i], spellBase.Combat.PercentageStatDiff[i], expireTime)
                    );

                if (spellBase.Combat.StatDiff[i] != 0 || spellBase.Combat.PercentageStatDiff[i] != 0)
                {
                    statBuffTime = spellBase.Combat.Duration;
                }
            }

            if (statBuffTime == -1)
            {
                if (spellBase.Combat.HoTDoT && spellBase.Combat.HotDotInterval > 0)
                {
                    statBuffTime = spellBase.Combat.Duration;
                }
            }

            var damageHealth = spellBase.Combat.VitalDiff[(int)Vitals.Health];
            var damageMana = spellBase.Combat.VitalDiff[(int)Vitals.Mana];

            Dictionary<AttackFailures, bool> attackFailures = new Dictionary<AttackFailures, bool>();
            var scaling = spellBase.Combat.Scaling;
            Stats scalingStat = (Stats)spellBase.Combat.ScalingStat;
            DamageType damageType = (DamageType)spellBase.Combat.DamageType;
            var critChance = spellBase.Combat.CritChance;
            var critMultiplier = spellBase.Combat.CritMultiplier;

            if ((spellBase.Combat.Effect != StatusTypes.OnHit || onHitTrigger) &&
                spellBase.Combat.Effect != StatusTypes.Shield)
            {
                if (this is Player player && spellBase.WeaponSpell && player.CastingWeapon != null ) // add on weapon stats if needed
                {
                    damageHealth += CalculateSpecialDamage(player.CastingWeapon.Damage, player.CastingWeapon, target);
                    scaling += player.CastingWeapon.Scaling;
                    scalingStat = (Stats) player.CastingWeapon.ScalingStat;
                    damageType = (DamageType) player.CastingWeapon.DamageType;
                    critChance += player.CastingWeapon.CritChance;
                    critMultiplier += player.CastingWeapon.CritMultiplier;
                }

                attackFailures = Attack(
                    target, damageHealth, damageMana, damageType,
                    scalingStat, scaling, critChance,
                    critMultiplier, deadAnimations, aliveAnimations, false, spellBase.Combat.Friendly, fromProjectile
                );
            }

            if (attackFailures.TryGetValue(AttackFailures.MISSED, out bool miss) && miss)
            {
                spellMissed = miss;
                return;
            }
            if (attackFailures.TryGetValue(AttackFailures.BLOCKED, out bool blocked) && blocked)
            {
                spellBlocked = blocked;
                // We don't return on a blocked - the spell can still do its status effects
            }

            if (spellBase.Combat.Effect > 0) //Handle status effects
            {
                //Check for onhit effect to avoid the onhit effect recycling.
                if (!(onHitTrigger && spellBase.Combat.Effect == StatusTypes.OnHit))
                {
                    // If the entity is immune to some status, then just inform the client of such
                    if (target.IsImmuneTo(StatusToImmunity(spellBase.Combat.Effect))) {
                        PacketSender.SendActionMsg(
                            target, Strings.Combat.immunetoeffect, CustomColors.Combat.Status
                        );
                    } else
                    {
                        // Else, apply the status
                        new Status(
                            target, this, spellBase, spellBase.Combat.Effect, spellBase.Combat.Duration,
                            spellBase.Combat.TransformSprite
                        );

                        PacketSender.SendActionMsg(
                            target, Strings.Combat.status[(int)spellBase.Combat.Effect], CustomColors.Combat.Status
                        );

                        //If an onhit or shield status bail out as we don't want to do any damage.
                        if (spellBase.Combat.Effect == StatusTypes.OnHit || spellBase.Combat.Effect == StatusTypes.Shield)
                        {
                            Animate(target, aliveAnimations);

                            return;
                        }
                    }
                }
            }
            else
            {
                if (statBuffTime > -1)
                {
                    if (!target.IsImmuneTo(StatusToImmunity(spellBase.Combat.Effect)))
                    {
                        new Status(target, this, spellBase, spellBase.Combat.Effect, statBuffTime, "");
                    } else
                    {
                        PacketSender.SendActionMsg(target, Strings.Combat.immunetoeffect, CustomColors.Combat.Status);
                    }
                }
            }

            //Handle DoT/HoT spells]
            // Alex: This was the old way.
            /*
            if (spellBase.Combat.HoTDoT)
            {
                var doTFound = false;
                foreach (var dot in target.CachedDots)
                {
                    if (dot.SpellBase.Id == spellBase.Id && dot.Target == this)
                    {
                        doTFound = true;
                    }
                }

                if (doTFound == false) //no duplicate DoT/HoT spells.
                {
                    new DoT(this, spellBase.Id, target);
                }
            }
            */
            
            if (spellBase.Combat.HoTDoT)
            {
                target.CachedDots.ToList()
                    .FindAll((DoT dot) => dot.SpellBase.Id == spellBase.Id && dot.Attacker == this)
                    .ForEach((DoT dot) => dot.Expire());

                new DoT(this, spellBase.Id, target);
            }
        }

        private void Animate(Entity target, List<KeyValuePair<Guid, sbyte>> animations, bool fromProjectile = false)
        {
            foreach (var anim in animations)
            {
                PacketSender.SendAnimationToProximity(anim.Key, 1, target.Id, target.MapId, 0, 0, anim.Value, MapInstanceId, fromProjectile);
            }
        }

        //Attacking with weapon or unarmed.
        public virtual void TryAttack(Entity target)
        {
            //See player and npc override of this virtual void
        }

        //Attack using a weapon or unarmed
        public virtual void TryAttack(
            Entity target,
            int baseDamage,
            DamageType damageType,
            Stats scalingStat,
            int scaling,
            int critChance,
            double critMultiplier,
            List<KeyValuePair<Guid, sbyte>> deadAnimations = null,
            List<KeyValuePair<Guid, sbyte>> aliveAnimations = null,
            ItemBase weapon = null
        )
        {
            if (AttackTimer > Globals.Timing.Milliseconds)
            {
                return;
            }

            //Check for parties and safe zones, friendly fire off (unless its healing)
            if (target is Player targetPlayer && this is Player player)
            {
                if (player.InParty(targetPlayer))
                {
                    return;
                }

                //Check if either the attacker or the defender is in a "safe zone" (Only apply if combat is PVP)
                //Player interaction common events
                targetPlayer.StartCommonEventsWithTrigger(CommonEventTrigger.PlayerInteract, "", this.Name);

                if (MapController.Get(MapId)?.ZoneType == MapZones.Safe)
                {
                    return;
                }

                if (MapController.Get(target.MapId)?.ZoneType == MapZones.Safe)
                {
                    return;
                }
            }

            //Check for taunt status and trying to attack a target that has not taunted you.
            foreach (var status in CachedStatuses)
            {
                if (status.Type == StatusTypes.Taunt)
                {
                    if (Target != target)
                    {
                        PacketSender.SendActionMsg(this, Strings.Combat.miss, CustomColors.Combat.Missed);

                        return;
                    }
                }
            }

            AttackTimer = Globals.Timing.Milliseconds + CalculateAttackTime();

            //Check if the attacker is blinded.
            if (IsOneBlockAway(target))
            {
                foreach (var status in CachedStatuses)
                {
                    if (status.Type == StatusTypes.Stun ||
                        status.Type == StatusTypes.Blind ||
                        status.Type == StatusTypes.Sleep)
                    {
                        PacketSender.SendActionMsg(this, Strings.Combat.miss, CustomColors.Combat.Missed);
                        PacketSender.SendEntityAttack(this, CalculateAttackTime());

                        return;
                    }
                }
            }

            if (weapon != null)
            {
                baseDamage = CalculateSpecialDamage(weapon.Damage, weapon, target);
            }
            Attack(
                target, baseDamage, 0, damageType, scalingStat, scaling, critChance, critMultiplier, deadAnimations,
                aliveAnimations, true
            );
        }

        public enum AttackFailures
        {
            BLOCKED,
            MISSED
        }

        public Dictionary<AttackFailures, bool> Attack(
            Entity enemy,
            int baseDamage,
            int secondaryDamage,
            DamageType damageType,
            Stats scalingStat,
            int scaling,
            int critChance,
            double critMultiplier,
            List<KeyValuePair<Guid, sbyte>> deadAnimations = null,
            List<KeyValuePair<Guid, sbyte>> aliveAnimations = null,
            bool isAutoAttack = false,
            bool ignoreEvasion = false,
            bool fromProjectile = false
        )
        {
            var originalBaseDamage = baseDamage;
            var damagingAttack = baseDamage > 0;
            if (enemy == null)
            {
                return new Dictionary<AttackFailures, bool>();
            }

            var invulnerable = enemy.CachedStatuses.Any(status => status.Type == StatusTypes.Invulnerable);

            bool isCrit = false;
            //Is this a critical hit?
            if (Randomization.Next(1, 101) > critChance)
            {
                critMultiplier = 1;
            }
            else
            {
                isCrit = true;
            }

            //Calculate Damages
            var attackMissed = false;
            if (!ignoreEvasion)
            {
                attackMissed = Formulas.AttackEvaded(baseDamage, damageType, this, enemy, critMultiplier, scaling, scalingStat);
            }
            if (!attackMissed)
            {
                baseDamage = Formulas.CalculateDamage(baseDamage, damageType, scalingStat, scaling, critMultiplier, this, enemy);
            }
            else
            {
                baseDamage = 0;
            }
            var wasBlocked = (originalBaseDamage != 0 && baseDamage == 0) && !attackMissed;

            if (originalBaseDamage != 0)
            {
                if (enemy is Resource)
                {
                    baseDamage = originalBaseDamage;
                }
                
                if (baseDamage < 0 && damagingAttack)
                {
                    baseDamage = 0;
                }

                if (baseDamage > 0 && enemy.HasVital(Vitals.Health) && !invulnerable)
                {
                    if (isCrit)
                    {
                        PacketSender.SendActionMsg(enemy, Strings.Combat.critical, CustomColors.Combat.Critical);
                    }

                    enemy.SubVital(Vitals.Health, (int) baseDamage);
                    switch (damageType)
                    {
                        case DamageType.Physical:
                            PacketSender.SendActionMsg(
                                enemy, Strings.Combat.removesymbol + (int) baseDamage,
                                CustomColors.Combat.PhysicalDamage
                            );

                            break;
                        case DamageType.Magic:
                            PacketSender.SendActionMsg(
                                enemy, Strings.Combat.removesymbol + (int) baseDamage, CustomColors.Combat.MagicDamage
                            );

                            break;
                        case DamageType.True:
                            PacketSender.SendActionMsg(
                                enemy, Strings.Combat.removesymbol + (int) baseDamage, CustomColors.Combat.TrueDamage
                            );

                            break;
                    }

                    var toRemove = new List<Status>();
                    foreach (var status in enemy.CachedStatuses.ToArray())  // ToArray the Array since removing a status will.. you know, change the collection.
                    {
                        //Wake up any sleeping targets targets and take stealthed entities out of stealth
                        if (status.Type == StatusTypes.Sleep || status.Type == StatusTypes.Stealth)
                        {
                            status.RemoveStatus();
                        }
                    }

                    // Add the attacker to the Npcs threat and loot table.
                    if (enemy is Npc enemyNpc)
                    {
                        var dmgMap = enemyNpc.DamageMap;
                        dmgMap.TryGetValue(this, out var damage);
                        dmgMap[this] = damage + baseDamage;

                        enemyNpc.LootMap.TryAdd(Id, true);
                        enemyNpc.LootMapCache = enemyNpc.LootMap.Keys.ToArray();
                        enemyNpc.TryFindNewTarget(Timing.Global.Milliseconds, default, false, this);
                    }

                    enemy.NotifySwarm(this);
                }
                else if (baseDamage < 0 && !enemy.IsFullVital(Vitals.Health))
                {
                    enemy.AddVital(Vitals.Health, (int) -baseDamage);
                    PacketSender.SendActionMsg(
                        enemy, Strings.Combat.addsymbol + (int) Math.Abs(baseDamage), CustomColors.Combat.Heal
                    );
                } else if (attackMissed || wasBlocked)
                {
                    // Add the attacker to the Npcs threat table only
                    if (enemy is Npc enemyNpc)
                    {
                        var dmgMap = enemyNpc.DamageMap;
                        dmgMap.TryGetValue(this, out var damage);
                        dmgMap[this] = damage + baseDamage;

                        enemyNpc.TryFindNewTarget(Timing.Global.Milliseconds, default, false, this);
                    }

                    enemy.NotifySwarm(this);
                }
            }

            if (secondaryDamage != 0 && !attackMissed)
            {
                secondaryDamage = Formulas.CalculateDamage(
                    secondaryDamage, damageType, scalingStat, scaling, critMultiplier, this, enemy
                );

                if (secondaryDamage < 0 && damagingAttack && !(enemy is Player))
                {
                    secondaryDamage = 0;
                }

                if (secondaryDamage > 0 && enemy.HasVital(Vitals.Mana) && !invulnerable)
                {
                    //If we took damage lets reset our combat timer
                    enemy.SubVital(Vitals.Mana, (int) secondaryDamage);
                    PacketSender.SendActionMsg(
                        enemy, Strings.Combat.removesymbol + (int) secondaryDamage, CustomColors.Combat.RemoveMana
                    );

                    //No Matter what, if we attack the entitiy, make them chase us
                    if (enemy is Npc enemyNpc)
                    {
                        enemyNpc.TryFindNewTarget(Timing.Global.Milliseconds, default, false, this);
                    }

                    enemy.NotifySwarm(this);
                }
                else if (secondaryDamage < 0 && !enemy.IsFullVital(Vitals.Mana))
                {
                    enemy.AddVital(Vitals.Mana, (int) -secondaryDamage);
                    PacketSender.SendActionMsg(
                        enemy, Strings.Combat.addsymbol + (int) Math.Abs(secondaryDamage), CustomColors.Combat.AddMana
                    );
                }
            }

            // Set combat timers!
            enemy.CombatTimer = Globals.Timing.Milliseconds + Options.CombatTime;
            CombatTimer = Globals.Timing.Milliseconds + Options.CombatTime;

            //Check for lifesteal
            if (GetType() == typeof(Player) && enemy.GetType() != typeof(Resource))
            {
                var lifesteal = ((Player) this).GetEquipmentBonusEffect(EffectType.Lifesteal) / 100f;
                var healthRecovered = lifesteal * baseDamage;
                if (healthRecovered > 0) //Don't send any +0 msg's.
                {
                    AddVital(Vitals.Health, (int) healthRecovered);
                    PacketSender.SendActionMsg(
                        this, Strings.Combat.addsymbol + (int) healthRecovered, CustomColors.Combat.Heal
                    );
                }
            }

            //Dead entity check
            if (enemy.GetVital(Vitals.Health) <= 0)
            {
                if (enemy.GetType() == typeof(Npc) || enemy.GetType() == typeof(Resource))
                {
                    lock (enemy.EntityLock)
                    {
                        enemy.Die(true, this);
                    }
                }
                else
                {
                    //PVP Kill common events
                    if (!enemy.Dead && enemy is Player && this is Player)
                    {
                        ((Player)this).StartCommonEventsWithTrigger(CommonEventTrigger.PVPKill, "", enemy.Name);
                        ((Player)enemy).StartCommonEventsWithTrigger(CommonEventTrigger.PVPDeath, "", this.Name);
                    }

                    lock (enemy.EntityLock)
                    {
                        enemy.Die(true, this);
                    }
                }

                if (deadAnimations != null)
                {
                    foreach (var anim in deadAnimations)
                    {
                        PacketSender.SendAnimationToProximity(
                            anim.Key, -1, Id, enemy.MapId, (byte) enemy.X, (byte) enemy.Y, anim.Value, MapInstanceId
                        );
                    }
                }
            }
            else
            {
                //Hit him, make him mad and send the vital update.
                if (aliveAnimations?.Count > 0)
                {
                    Animate(enemy, aliveAnimations, fromProjectile);
                }

                //Check for any onhit damage bonus effects!
                CheckForOnhitAttack(enemy, isAutoAttack);
            }

            // Add a timer before able to make the next move.
            if (GetType() == typeof(Npc))
            {
                ((Npc) this).MoveTimer = Globals.Timing.Milliseconds + (long) GetMovementTime();
            }

            if (!wasBlocked && !attackMissed && !invulnerable)
            {
                SendCombatEffects(enemy, isCrit, baseDamage);
            }

            if (damageType != DamageType.True && !(enemy is Resource)) // Alex - dumb fix
            {
                if (wasBlocked) 
                {
                    SendBlockedAttackMessage(this, enemy);
                }
                else if (attackMissed)
                {
                    SendMissedAttackMessage(this, enemy, damageType);
                }
            }
            

            var failures = new Dictionary<AttackFailures, bool>();
            failures.Add(AttackFailures.BLOCKED, wasBlocked);
            failures.Add(AttackFailures.MISSED, attackMissed);
            return failures;
        }

        private void SendMissedAttackMessage(Entity attacker, Entity defender, DamageType damageType)
        {
            if (defender is Player def)
            {
                PacketSender.SendPlaySound(def, Options.MissSound);
            }
            if (attacker is Player att)
            {
                PacketSender.SendPlaySound(att, Options.MissSound);
            }
            switch(damageType)
            {
                case DamageType.Magic:
                    PacketSender.SendActionMsg(defender, Strings.Combat.resist, CustomColors.Combat.Missed);
                    break;
                default:
                    PacketSender.SendActionMsg(defender, Strings.Combat.miss, CustomColors.Combat.Missed);
                    break;
            }
        }

        private void SendBlockedAttackMessage(Entity attacker, Entity defender)
        {
            if (defender is Player def)
            {
                PacketSender.SendPlaySound(def, Options.BlockSound);
            }
            if (attacker is Player att)
            {
                PacketSender.SendPlaySound(att, Options.BlockSound);
            }
            PacketSender.SendActionMsg(defender, Strings.Combat.blocked, CustomColors.Combat.Blocked);
        }

        void CheckForOnhitAttack(Entity enemy, bool isAutoAttack)
        {
            if (isAutoAttack) //Ignore spell damage.
            {
                foreach (var status in CachedStatuses)
                {
                    if (status.Type == StatusTypes.OnHit)
                    {
                        TryAttackSpell(enemy, status.Spell, out bool miss, out bool blocked, (sbyte) Directions.Up, true);
                        status.RemoveStatus();
                    }
                }
            }
        }

        private void SendCombatEffects(Entity enemy, bool isCrit, int damage)
        {
            // Calculate combat special effects (entity/screen flash, screen shake, extra sounds)
            // Define vars that will be used for combat effects
            Color flashColor = null;
            Color entityFlashColor = CustomColors.Combat.GenericDamageGiveEntityFlashColor;
            float flashIntensity = 0.0f;
            float flashDuration = Options.HitFlashDuration;
            string damageSound = "";

            if (this is Player player && enemy.Id != player.Id) // if the player is targeting themselves don't bother with any of this, just skip to the part where we send to the enemy
            {
                // Calc distances so shakes aren't as violent when further away
                int enemyDistance = GetDistanceTo(enemy);
                float shakeModifier = 1.0f;
                if (enemyDistance > 1)
                {
                    shakeModifier = shakeModifier - (enemyDistance / Options.MaxDamageShakeDistance); // Don't shake if more than 6 tiles away
                    shakeModifier = (float)MathHelper.Clamp(shakeModifier, 0.0f, 1.0f);
                }

                if (string.IsNullOrEmpty(damageSound) && damage > 0)
                {
                    damageSound = Options.GenericDamageGivenSound;
                }

                if (player.Client != null && !(enemy is Resource))
                {
                    var shakeAmount = Options.DamageGivenShakeAmount * shakeModifier;
                    if (isCrit)
                    {
                        flashColor = CustomColors.Combat.CriticalHitDealtColor;
                        flashIntensity = Options.CriticalHitFlashIntensity;
                        damageSound = Options.CriticalHitDealtSound;
                    }

                    if (damage < 0) // healing exceptions
                    {
                        entityFlashColor = CustomColors.Combat.GenericHealingReceivedEntityFlashColor;
                        flashColor = CustomColors.Combat.HealingFlashColor;
                        entityFlashColor = CustomColors.Combat.GenericHealingReceivedEntityFlashColor;
                        flashColor = CustomColors.Combat.HealingFlashColor;
                        shakeAmount = 0.0f;
                    }

                    PacketSender.SendCombatEffectPacket(player.Client,
                        enemy.Id,
                        shakeAmount,
                        entityFlashColor,
                        damageSound,
                        flashIntensity,
                        flashDuration,
                        flashColor);
                }
                else if (enemy is Resource)
                {
                    if (enemy.GetVital(Vitals.Health) <= 0)
                    {
                        // Only shake for an exhausted resource
                        PacketSender.SendCombatEffectPacket(player.Client,
                            enemy.Id,
                            Options.ResourceDestroyedShakeAmount * shakeModifier,
                            null, // Don't want a resource to flash
                            "", // Don't want a resource to make a hit sound
                            0.0f,
                            0.0f,
                            null);
                    }
                }
            }

            // Send damaged effects to enemy - this will make their screen flash if critted, play THEIR hurt sound, and also make THEIR entity sprite flash
            if (enemy.GetVital(Vitals.Health) > 0 && enemy is Player en)
            {
                var shakeAmount = Options.DamageTakenShakeAmount;
                flashIntensity = Options.DamageTakenFlashIntensity;
                
                if (damage < 0) // healing exceptions
                {
                    entityFlashColor = CustomColors.Combat.GenericHealingReceivedEntityFlashColor;
                    flashColor = CustomColors.Combat.HealingFlashColor;
                    shakeAmount = 0.0f;
                }
                else if (damage > 0)
                {
                    flashColor = CustomColors.Combat.DamageTakenFlashColor;
                    flashIntensity = Options.DamageTakenFlashIntensity;
                    damageSound = Options.GenericDamageReceivedSound;

                    if (isCrit)
                    {
                        flashColor = CustomColors.Combat.CriticalHitReceivedColor;
                        flashIntensity = Options.CriticalHitFlashIntensity;
                        damageSound = Options.CriticalHitReceivedSound;
                    }
                }
                
                PacketSender.SendCombatEffectPacket(en.Client,
                        en.Id,
                        shakeAmount,
                        entityFlashColor,
                        damageSound,
                        flashIntensity,
                        flashDuration,
                        flashColor);
            }
        }

        public virtual void KilledEntity(Entity entity)
        {
        }

        public bool CanCastSpell(Guid spellId, Entity target)
        {
            return CanCastSpell(SpellBase.Get(spellId), target);
        }

        public virtual bool CanCastSpell(SpellBase spellDescriptor, Entity target)
        {
            if (spellDescriptor == null)
            {
                return false;
            }

            var spellCombat = spellDescriptor.Combat;
            if (spellDescriptor.SpellType != SpellTypes.CombatSpell && spellDescriptor.SpellType != SpellTypes.Event ||
                spellCombat == null)
            {
                return true;
            }

            if (spellCombat.TargetType == SpellTargetTypes.Single)
            {
                return target == null || InRangeOf(target, spellCombat.CastRange);
            }

            return true;
        }

        public virtual void CastSpell(Guid spellId, int spellSlot = -1, bool prayerSpell = false, Entity prayerTarget = null, int prayerSpellDir = -1)
        {
            var spellBase = SpellBase.Get(spellId);
            if (spellBase == null)
            {
                return;
            }

            if (!CanCastSpell(spellBase, CastTarget))
            {
                return;
            }

            if (spellBase.VitalCost[(int)Vitals.Mana] > 0)
            {
                if (!prayerSpell) // prayer spells dont cost anything
                {
                    SubVital(Vitals.Mana, spellBase.VitalCost[(int)Vitals.Mana]);
                }
            }
            else
            {
                AddVital(Vitals.Mana, -spellBase.VitalCost[(int)Vitals.Mana]);
            }

            if (spellBase.VitalCost[(int)Vitals.Health] > 0)
            {
                if (!prayerSpell) // prayer spells dont cost anything
                {
                    SubVital(Vitals.Health, spellBase.VitalCost[(int)Vitals.Health]);
                }
            }
            else
            {
                AddVital(Vitals.Health, -spellBase.VitalCost[(int)Vitals.Health]);
            }

            switch (spellBase.SpellType)
            {
                case SpellTypes.CombatSpell:
                case SpellTypes.Event:

                    switch (spellBase.Combat.TargetType)
                    {
                        case SpellTargetTypes.Self:
                            if (spellBase.HitAnimationId != Guid.Empty && spellBase.Combat.Effect != StatusTypes.OnHit)
                            {
                                PacketSender.SendAnimationToProximity(
                                    spellBase.HitAnimationId, 1, Id, MapId, 0, 0, (sbyte) Dir, MapInstanceId
                                ); //Target Type 1 will be global entity
                            }

                            TryAttackSpell(this, spellBase, out bool miss, out bool blocked);

                            break;
                        case SpellTargetTypes.Single:
                            if (CastTarget == null || prayerSpell)
                            {
                                return;
                            }

                            //If target has stealthed we cannot hit the spell.
                            foreach (var status in CastTarget.CachedStatuses)
                            {
                                if (status.Type == StatusTypes.Stealth)
                                {
                                    return;
                                }
                            }

                            if (spellBase.Combat.HitRadius > 0) //Single target spells with AoE hit radius'
                            {
                                HandleAoESpell(
                                    spellId, spellBase.Combat.HitRadius, CastTarget.MapId, CastTarget.X, CastTarget.Y,
                                    null
                                );
                            }
                            else
                            {
                                TryAttackSpell(CastTarget, spellBase, out bool spellMissed, out bool spellBlocked);
                            }

                            break;
                        case SpellTargetTypes.AoE:
                            if (prayerSpell) return;
                            HandleAoESpell(spellId, spellBase.Combat.HitRadius, MapId, X, Y, null);
                            break;
                        case SpellTargetTypes.Projectile:
                            var projectileBase = spellBase.Combat.Projectile;
                            if (projectileBase != null)
                            {
                                if (MapController.TryGetInstanceFromMap(MapId, MapInstanceId, out var mapInstance))
                                {
                                    if (prayerSpell && prayerTarget != null && prayerSpellDir >= 0)
                                    {
                                        mapInstance.SpawnMapProjectile(
                                                this, projectileBase, spellBase, null, prayerTarget.MapId, (byte)prayerTarget.X, (byte)prayerTarget.Y, (byte)prayerTarget.Z,
                                                (byte)prayerSpellDir, CastTarget
                                            );
                                    }
                                    else
                                    {
                                        mapInstance.SpawnMapProjectile(
                                            this, projectileBase, spellBase, null, MapId, (byte)X, (byte)Y, (byte)Z,
                                            (byte)Dir, CastTarget
                                        );
                                    }
                                }   
                            }

                            break;
                        case SpellTargetTypes.OnHit:
                            if (spellBase.Combat.Effect == StatusTypes.OnHit)
                            {
                                new Status(
                                    this, this, spellBase, StatusTypes.OnHit, spellBase.Combat.OnHitDuration,
                                    spellBase.Combat.TransformSprite
                                );

                                PacketSender.SendActionMsg(
                                    this, Strings.Combat.status[(int) spellBase.Combat.Effect],
                                    CustomColors.Combat.Status
                                );
                            }

                            break;
                        case SpellTargetTypes.Trap:
                            if (MapController.TryGetInstanceFromMap(MapId, MapInstanceId, out var instance)) 
                            {
                                instance.SpawnTrap(this, spellBase, (byte)X, (byte)Y, (byte)Z);
                            }

                            break;
                        default:
                            break;
                    }

                    break;
                case SpellTypes.Warp:
                    if (this is Player)
                    {
                        Warp(
                            spellBase.Warp.MapId, spellBase.Warp.X, spellBase.Warp.Y,
                            spellBase.Warp.Dir - 1 == -1 ? (byte) this.Dir : (byte) (spellBase.Warp.Dir - 1)
                        );
                    }

                    break;
                case SpellTypes.WarpTo:
                    if (CastTarget != null)
                    {
                        HandleAoESpell(spellId, spellBase.Combat.CastRange, MapId, X, Y, CastTarget);
                    }
                    break;
                case SpellTypes.Dash:
                    PacketSender.SendActionMsg(this, Strings.Combat.dash, CustomColors.Combat.Dash);
                    var dash = new Dash(
                        this, spellBase.Combat.CastRange, (byte) Dir, Convert.ToBoolean(spellBase.Dash.IgnoreMapBlocks),
                        Convert.ToBoolean(spellBase.Dash.IgnoreActiveResources),
                        Convert.ToBoolean(spellBase.Dash.IgnoreInactiveResources),
                        Convert.ToBoolean(spellBase.Dash.IgnoreZDimensionAttributes)
                    );

                    break;
                default:
                    break;
            }

            if (spellSlot >= 0 && spellSlot < Options.MaxPlayerSkills)
            {
                // Player cooldown handling is done elsewhere!
                if (this is Player player)
                {
                    player.UpdateCooldown(spellBase);

                    // Trigger the global cooldown, if we're allowed to.
                    if (!spellBase.IgnoreGlobalCooldown)
                    {
                        player.UpdateGlobalCooldown();
                    }
                }
                else
                {
                    SpellCooldowns[Spells[spellSlot].SpellId] =
                    Globals.Timing.MillisecondsUTC + (int)(spellBase.CooldownDuration);
                }
            }

            if (GetVital(Vitals.Health) <= 0) // if the spell has killed the entity
            {
                Die();
            }
        }

        private void HandleAoESpell(
            Guid spellId,
            int range,
            Guid startMapId,
            int startX,
            int startY,
            Entity spellTarget,
            bool ignoreEvasion = false
        )
        {
            var spellBase = SpellBase.Get(spellId);
            if (spellBase != null)
            {
                var startMap = MapController.Get(startMapId);
                foreach (var instance in MapController.GetSurroundingMapInstances(startMapId, MapInstanceId, true))
                {
                    foreach (var entity in instance.GetCachedEntities())
                    {
                        if (entity != null && (entity is Player || entity is Npc))
                        {
                            if (spellTarget == null || spellTarget == entity)
                            {
                                if (entity.GetDistanceTo(startMap, startX, startY) <= range)
                                {
                                    //Check to handle a warp to spell
                                    if (spellBase.SpellType == SpellTypes.WarpTo)
                                    {
                                        if (spellTarget != null)
                                        {
                                            //Spelltarget used to be Target. I don't know if this is correct or not.
                                            int[] position = GetPositionNearTarget(spellTarget.MapId, spellTarget.X, spellTarget.Y);
                                            Warp(spellTarget.MapId, (byte)position[0], (byte)position[1], (byte)Dir);
                                            ChangeDir(DirToEnemy(spellTarget));
                                        }
                                    }

                                    TryAttackSpell(entity, spellBase, out bool miss, out bool blocked, (sbyte)Directions.Up, ignoreEvasion); //Handle damage
                                }
                            }
                        }
                    }
                }
            }
        }

        private int[] GetPositionNearTarget(Guid mapId, int x, int y)
        {
            if (MapController.TryGetInstanceFromMap(mapId, MapInstanceId, out var instance))
            {
                List<int[]> validPosition = new List<int[]>();

                // Start by north, west, est and south
                for (int col = -1; col < 2; col++)
                {
                    for (int row = -1; row < 2; row++)
                    {
                        if (Math.Abs(col % 2) != Math.Abs(row % 2))
                        {
                            int newX = x + row;
                            int newY = y + col;

                            if (newX >= 0 && newX <= Options.MapWidth &&
                                newY >= 0 && newY <= Options.MapHeight &&
                                !instance.TileBlocked(newX, newY))
                            {
                                validPosition.Add(new int[] { newX, newY });
                            }
                        }
                    }
                }

                if (validPosition.Count > 0)
                {
                    return validPosition[Randomization.Next(0, validPosition.Count)];
                }

                // If nothing found, diagonal direction
                for (int col = -1; col < 2; col++)
                {
                    for (int row = -1; row < 2; row++)
                    {
                        if (Math.Abs(col % 2) == Math.Abs(row % 2))
                        {
                            int newX = x + row;
                            int newY = y + col;

                            // Tile must not be the target position
                            if (newX >= 0 && newX <= Options.MapWidth &&
                                newY >= 0 && newY <= Options.MapHeight &&
                                !(x + row == x && y + col == y) &&
                                !instance.TileBlocked(newX, newY))
                            {
                                validPosition.Add(new int[] { newX, newY });
                            }
                        }
                    }
                }

                if (validPosition.Count > 0)
                {
                    return validPosition[Randomization.Next(0, validPosition.Count)];
                }

                // If nothing found, return target position
                return new int[] { x, y };
            } else
            {
                return new int[] { x, y };
            }
        }

        //Check if the target is either up, down, left or right of the target on the correct Z dimension.
        protected bool IsOneBlockAway(Entity target)
        {
            var myTile = new TileHelper(MapId, X, Y);
            var enemyTile = new TileHelper(target.MapId, target.X, target.Y);
            if (Z == target.Z)
            {
                myTile.Translate(0, -1); // Target Up
                if (myTile.Matches(enemyTile))
                {
                    return true;
                }

                myTile.Translate(0, 2); // Target Down
                if (myTile.Matches(enemyTile))
                {
                    return true;
                }

                myTile.Translate(-1, -1); // Target Left
                if (myTile.Matches(enemyTile))
                {
                    return true;
                }

                myTile.Translate(2, 0); // Target Right 
                if (myTile.Matches(enemyTile))
                {
                    return true;
                }
            }

            return false;
        }

        //These functions only work when one block away.
        protected bool IsFacingTarget(Entity target)
        {
            if (IsOneBlockAway(target))
            {
                var myTile = new TileHelper(MapId, X, Y);
                var enemyTile = new TileHelper(target.MapId, target.X, target.Y);
                myTile.Translate(0, -1);
                if (myTile.Matches(enemyTile) && Dir == (int) Directions.Up)
                {
                    return true;
                }

                myTile.Translate(0, 2);
                if (myTile.Matches(enemyTile) && Dir == (int) Directions.Down)
                {
                    return true;
                }

                myTile.Translate(-1, -1);
                if (myTile.Matches(enemyTile) && Dir == (int) Directions.Left)
                {
                    return true;
                }

                myTile.Translate(2, 0);
                if (myTile.Matches(enemyTile) && Dir == (int) Directions.Right)
                {
                    return true;
                }
            }

            return false;
        }

        public int GetDistanceTo(Entity target)
        {
            if (target != null)
            {
                return GetDistanceTo(target.Map, target.X, target.Y);
            }
            //Something is null.. return a value that is out of range :) 
            return 9999;
        }

        public int GetDistanceTo(MapController targetMap, int targetX, int targetY)
        {
            return GetDistanceBetween(Map, targetMap, X, targetX, Y, targetY);
        }

        public int GetDistanceBetween(MapController mapA, MapController mapB, int xTileA, int xTileB, int yTileA, int yTileB)
        {
            if (mapA != null && mapB != null && mapA.MapGrid == mapB.MapGrid
            ) //Make sure both maps exist and that they are in the same dimension
            {
                //Calculate World Tile of Me
                var x1 = xTileA + mapA.MapGridX * Options.MapWidth;
                var y1 = yTileA + mapA.MapGridY * Options.MapHeight;

                //Calculate world tile of target
                var x2 = xTileB + mapB.MapGridX * Options.MapWidth;
                var y2 = yTileB + mapB.MapGridY * Options.MapHeight;

                return (int)Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
            }

            //Something is null.. return a value that is out of range :) 
            return 9999;
        }

        public bool InRangeOf(Entity target, int range)
        {
            var dist = GetDistanceTo(target);
            if (dist == 9999)
            {
                return false;
            }

            if (dist <= range)
            {
                return true;
            }

            return false;
        }

        public virtual void NotifySwarm(Entity attacker)
        {
        }

        protected byte DirToEnemy(Entity target)
        {
            //Calculate World Tile of Me
            var x1 = X + MapController.Get(MapId).MapGridX * Options.MapWidth;
            var y1 = Y + MapController.Get(MapId).MapGridY * Options.MapHeight;

            //Calculate world tile of target
            var x2 = target.X + MapController.Get(target.MapId).MapGridX * Options.MapWidth;
            var y2 = target.Y + MapController.Get(target.MapId).MapGridY * Options.MapHeight;


            if (Math.Abs(x1 - x2) > Math.Abs(y1 - y2))
            {
                //Left or Right
                if (x1 - x2 < 0)
                {
                    return (byte) Directions.Right;
                }

                return (byte) Directions.Left;
            }

            //Left or Right
            if (y1 - y2 < 0)
            {
                return (byte) Directions.Down;
            }

            return (byte) Directions.Up;
        }

        // Outdated : Check if the target is either up, down, left or right of the target on the correct Z dimension.
        // Check for 8 directions
        protected bool IsOneBlockAway(Guid mapId, int x, int y, int z = 0)
        {
            //Calculate World Tile of Me
            var x1 = X + MapController.Get(MapId).MapGridX * Options.MapWidth;
            var y1 = Y + MapController.Get(MapId).MapGridY * Options.MapHeight;

            //Calculate world tile of target
            var x2 = x + MapController.Get(mapId).MapGridX * Options.MapWidth;
            var y2 = y + MapController.Get(mapId).MapGridY * Options.MapHeight;
            if (z == Z)
            {
                if (y1 == y2)
                {
                    if (x1 == x2 - 1)
                    {
                        return true;
                    }
                    else if (x1 == x2 + 1)
                    {
                        return true;
                    }
                }

                if (x1 == x2)
                {
                    if (y1 == y2 - 1)
                    {
                        return true;
                    }
                    else if (y1 == y2 + 1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        protected void PlayDeathAnimation()
        {
            if (DeathAnimation != Guid.Empty)
            {
                PacketSender.SendAnimationToProximity(DeathAnimation, -1, Id, MapId, (byte) X, (byte) Y, (sbyte)Directions.Up, MapInstanceId);
            }
            if (this is Player)
            {
                PacketSender.SendAnimationToProximity(new Guid(Options.PlayerDeathAnimationId), -1, Id, MapId, (byte) X, (byte) Y, (sbyte)Directions.Up, MapInstanceId);
            }
        }

        //Spawning/Dying
        public virtual void Die(bool dropItems = true, Entity killer = null)
        {
            if (IsDead() || Items == null)
            {
                return;
            }

            if (dropItems)
            {
                PlayDeathAnimation();
            }

            // Run events and other things.
            killer?.KilledEntity(this);

            if (dropItems)
            {
                var lootGenerated = new List<Player>();
                // If this is an NPC, drop loot for every single player that participated in the fight.
                if (this is Npc npc && npc.Base.IndividualizedLoot)
                {
                    // Generate loot for every player that has helped damage this monster, as well as their party members.
                    // Keep track of who already got loot generated for them though, or this gets messy!
                    foreach (var entityEntry in npc.LootMapCache)
                    {
                        var player = Player.FindOnline(entityEntry);
                        if (player != null)
                        {
                            // is this player in a party?
                            if (player.Party.Count > 0 && Options.Instance.LootOpts.IndividualizedLootAutoIncludePartyMembers)
                            {
                                // They are, so check for all party members and drop if still eligible!
                                foreach (var partyMember in player.Party)
                                {
                                    if (!lootGenerated.Contains(partyMember))
                                    {
                                        DropItems(partyMember);
                                        lootGenerated.Add(partyMember);
                                    }
                                }
                            }
                            else
                            {
                                // They're not in a party, so drop the item if still eligible!
                                if (!lootGenerated.Contains(player))
                                {
                                    DropItems(player);
                                    lootGenerated.Add(player);
                                }
                            }
                        }
                    }

                    // Clear their loot table and threat table.
                    npc.DamageMap.Clear();
                    npc.LootMap.Clear();
                    npc.LootMapCache = Array.Empty<Guid>();
                }
                else
                {
                    // Drop as normal.
                    DropItems(killer);
                }
            }
            
            foreach (var instance in MapController.GetSurroundingMapInstances(MapId, MapInstanceId, true))
            {
                instance.ClearEntityTargetsOf(this);
            }

            DoT?.Clear();
            CachedDots = new DoT[0];
            Statuses?.Clear();
            CachedStatuses = new Status[0];
            Stat?.ToList().ForEach(stat => stat?.Reset());

            Dead = true;
        }

        private void DropItems(Entity killer, bool sendUpdate = true)
        {
            // Drop items
            if (Map.ZoneType != MapZones.Normal && this is Player) return; // Only drop items in PVP
            for (var n = 0; n < Items.Count; n++)
            {
                if (Items[n] == null)
                {
                    continue;
                }

                // Don't mess with the actual object.
                var item = Items[n].Clone();
                
                var itemBase = ItemBase.Get(item.ItemId);
                if (itemBase == null)
                {
                    continue;
                }

                //Don't lose bound items on death for players.
                if (this.GetType() == typeof(Player))
                {
                    if (itemBase.DropChanceOnDeath == 0)
                    {
                        continue;
                    }
                }

                //Calculate the killers luck (If they are a player)
                var playerKiller = killer as Player;
                var luck = 1 + playerKiller?.GetEquipmentBonusEffect(EffectType.Luck) / 100f;

                Guid lootOwner = Guid.Empty;
                if (this is Player)
                {
                    //Player drop rates
                    if (Randomization.Next(1, 101) >= itemBase.DropChanceOnDeath * luck)
                    {
                        continue;
                    }

                    // It's a player, try and set ownership to the player that killed them.. If it was a player.
                    // Otherwise set to self, so they can come and reclaim their items.
                    lootOwner = playerKiller?.Id ?? Id;
                }
                else
                {
                    //Npc drop rates
                    var randomChance = Randomization.Next(1, 100001);
                    if (randomChance >= (item.DropChance * 1000) * luck)
                    {
                        continue;
                    }

                    // Set owner to player that killed this, if there is any.
                    if (playerKiller != null && this is Npc thisNpc)
                    {
                        // Yes, so set the owner to the player that killed it.
                        lootOwner = playerKiller.Id;
                    }

                    // Set the attributes for this item.
                    item.Set(new Item(item.ItemId, item.Quantity, true));
                }

                // Spawn the actual item!
                if (MapController.TryGetInstanceFromMap(MapId, MapInstanceId, out var instance))
                {
                    instance.SpawnItem(X, Y, item, item.Quantity, lootOwner, sendUpdate);
                }

                // Remove the item from inventory if a player.
                var player = this as Player;
                player?.TryTakeItem(Items[n], item.Quantity);
            }


        }

        public virtual bool IsDead()
        {
            return Dead;
        }

        public void Reset()
        {
            for (var i = 0; i < (int) Vitals.VitalCount; i++)
            {
                RestoreVital((Vitals) i);
            }

            Dead = false;
        }

        //Empty virtual functions for players
        public virtual void Warp(Guid newMapId, float newX, float newY, bool adminWarp = false)
        {
            Warp(newMapId, newX, newY, (byte) Dir, adminWarp);
        }

        public virtual void Warp(
            Guid newMapId,
            float newX,
            float newY,
            byte newDir,
            bool adminWarp = false,
            byte zOverride = 0,
            bool mapSave = false,
            bool fromWarpEvent = false,
            MapInstanceType mapInstanceType = MapInstanceType.NoChange,
            bool fromLogin = false
        )
        {
        }

        public virtual EntityPacket EntityPacket(EntityPacket packet = null, Player forPlayer = null)
        {
            if (packet == null)
            {
                throw new Exception("No packet to populate!");
            }

            packet.EntityId = Id;
            packet.MapId = MapId;
            packet.Name = Name;
            packet.Sprite = Sprite;
            packet.Color = Color;
            packet.Face = Face;
            packet.Level = Level;
            packet.X = (byte) X;
            packet.Y = (byte) Y;
            packet.Z = (byte) Z;
            packet.Dir = (byte) Dir;
            packet.Passable = Passable;
            packet.HideName = HideName;
            packet.HideEntity = HideEntity;
            packet.Animations = Animations.ToArray();
            packet.Vital = GetVitals();
            packet.MaxVital = GetMaxVitals();
            packet.Stats = GetStatValues();
            packet.StatusEffects = StatusPackets();
            packet.NameColor = NameColor;
            packet.HeaderLabel = new LabelPacket(HeaderLabel.Text, HeaderLabel.Color);
            packet.FooterLabel = new LabelPacket(FooterLabel.Text, FooterLabel.Color);

            return packet;
        }

        public StatusPacket[] StatusPackets()
        {
            var statuses = CachedStatuses;
            var statusPackets = new StatusPacket[statuses.Length];
            for (var i = 0; i < statuses.Length; i++)
            {
                var status = statuses[i];
                int[] vitalShields = null;
                if (status.Type == StatusTypes.Shield)
                {
                    vitalShields = new int[(int) Vitals.VitalCount];
                    for (var x = 0; x < (int) Vitals.VitalCount; x++)
                    {
                        vitalShields[x] = status.shield[x];
                    }
                }

                statusPackets[i] = new StatusPacket(
                    status.Spell.Id, status.Type, status.Data, (int) (status.Duration - Globals.Timing.Milliseconds),
                    (int) (status.Duration - status.StartTime), vitalShields
                );
            }

            return statusPackets;
        }

        Immunities StatusToImmunity(StatusTypes status)
        {
            switch (status)
            {
                case StatusTypes.Stun:
                    return Immunities.Stun;
                case StatusTypes.Silence:
                    return Immunities.Silence;
                case StatusTypes.Sleep:
                    return Immunities.Sleep;
                case StatusTypes.Blind:
                    return Immunities.Blind;
                case StatusTypes.Snare:
                    return Immunities.Snare;
                case StatusTypes.Taunt:
                    return Immunities.Taunt;
                case StatusTypes.Transform:
                    return Immunities.Transform;
                default:
                    return Immunities.None;
            }
        }

        bool IsImmuneTo(Immunities effect)
        {
            if (effect == Immunities.None)
            {
                return false;
            } else
            {
                return ImmuneTo.TryGetValue(effect, out var value) ? value : false;
            }
        }

        #region Spell Cooldowns

        [JsonIgnore, Column("SpellCooldowns")]
        public string SpellCooldownsJson
        {
            get => JsonConvert.SerializeObject(SpellCooldowns);
            set => SpellCooldowns = JsonConvert.DeserializeObject<ConcurrentDictionary<Guid, long>>(value ?? "{}");
        }

        [NotMapped] public ConcurrentDictionary<Guid, long> SpellCooldowns = new ConcurrentDictionary<Guid, long>();

        #endregion

    }

}
