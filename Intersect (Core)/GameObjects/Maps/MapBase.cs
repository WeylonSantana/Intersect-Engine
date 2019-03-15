﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Intersect.Collections;
using Intersect.Enums;
using Intersect.GameObjects.Events;
using Intersect.Models;
using Intersect.Utilities;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Intersect.GameObjects.Maps
{
    public class MapBase : DatabaseObject<MapBase>
    {
        //SyncLock
        [JsonIgnore]
        [NotMapped]
        protected object mMapLock = new object();

        //Client/Editor Only
        [JsonIgnore]
        [NotMapped]
        public MapAutotiles Autotiles;

        //Temporary Values
        [JsonIgnore]
        [NotMapped]
        public bool IsClient;

        //Core Data
        [JsonIgnore]
        [NotMapped]
        public TileArray[] Layers = new TileArray[Options.LayerCount];

        //For server only
        [JsonIgnore]
        public byte[] TileData { get; set; }

        public Guid Up { get; set; }
        public Guid Down { get; set; }
        public Guid Left { get; set; }
        public Guid Right { get; set; }
        public int Revision { get; set; }

        //Cached Att Data
        private byte[] mCachedAttributeData = null;

        [Column("Attributes")]
        [JsonIgnore]
        public byte[] AttributeData
        {
            get => mCachedAttributeData;
            set => Attributes = JsonConvert.DeserializeObject<MapAttribute[,]>(System.Text.Encoding.UTF8.GetString(Compression.DecompressPacket(value)), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, ObjectCreationHandling = ObjectCreationHandling.Replace });
        }

        //Map Attributes
        private MapAttribute[,] mAttributes = new MapAttribute[Options.MapWidth, Options.MapHeight];

        [NotMapped]
        [JsonIgnore]
        public MapAttribute[,] Attributes
        {
            get => mAttributes;
            set
            {
                mAttributes = value;
                mCachedAttributeData = Compression.CompressPacket(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Attributes, new JsonSerializerSettings() {TypeNameHandling = TypeNameHandling.Auto, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, ObjectCreationHandling = ObjectCreationHandling.Replace})));
            }
        }

        [Column("Lights")]
        [JsonIgnore]
        public string LightsJson
        {
            get => JsonConvert.SerializeObject(Lights);
            set
            {
                Lights.Clear();
                var lights = JsonConvert.DeserializeObject<List<LightBase>>(value);
                if (lights != null)
                {
                    Lights.AddRange(lights);
                }
            }
        }
        [NotMapped]
        [NotNull]
        public readonly List<LightBase> Lights = new List<LightBase>();

        [Column("Events")]
        [JsonIgnore]
        public string EventIdsJson
        {
            get => JsonConvert.SerializeObject(EventIds);
            set => EventIds = JsonConvert.DeserializeObject<List<Guid>>(value);
        }
        [NotMapped]
        public List<Guid> EventIds = new List<Guid>();

        [NotMapped]
        public string LocalEventsJson
        {
            get => JsonConvert.SerializeObject(LocalEvents, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, ObjectCreationHandling = ObjectCreationHandling.Replace });
            set => JsonConvert.PopulateObject(value, LocalEvents, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, ObjectCreationHandling = ObjectCreationHandling.Replace });
        }
        [NotMapped]
        [JsonIgnore]
        [NotNull]
        public readonly Dictionary<Guid, EventBase> LocalEvents = new Dictionary<Guid, EventBase>();

        [Column("NpcSpawns")]
        [JsonIgnore]
        public string NpcSpawnsJson
        {
            get => JsonConvert.SerializeObject(Spawns);
            set
            {
                Spawns.Clear();

                var spawns = JsonConvert.DeserializeObject<List<NpcSpawn>>(value);
                if (spawns != null)
                {
                    Spawns.AddRange(spawns);
                }
            }
        }
        [NotMapped]
        [NotNull]
        public readonly List<NpcSpawn> Spawns  = new List<NpcSpawn>();

        //Properties
        public string Music { get; set; } = null;
        public string Sound { get; set; } = null;
        public bool IsIndoors { get; set; }
        public string Panorama { get; set; } = null;
        public string Fog { get; set; } = null;
        public int FogXSpeed { get; set; }
        public int FogYSpeed { get; set; }
        public int FogTransparency { get; set; }
        public int RHue { get; set; }
        public int GHue { get; set; }
        public int BHue { get; set; }
        public int AHue { get; set; }
        public int Brightness { get; set; } = 100;
        public MapZones ZoneType { get; set; } = MapZones.Normal;
        public int PlayerLightSize { get; set; } = 300;
        public byte PlayerLightIntensity { get; set; } = 255;
        public float PlayerLightExpand { get; set; }

        [Column("PlayerLightColor")]
        [JsonIgnore]
        public string PlayerLightColorJson
        {
            get => JsonConvert.SerializeObject(PlayerLightColor);
            set => PlayerLightColor = JsonConvert.DeserializeObject<Color>(value);
        }
        [NotMapped]
        public Color PlayerLightColor { get; set; } = Color.White;
        public string OverlayGraphic { get; set; } = null;

        //Weather
        [Column("WeatherAnimation")]
        [JsonProperty]
        public Guid WeatherAnimationId { get; protected set; }
        [NotMapped]
        [JsonIgnore]
        public AnimationBase WeatherAnimation
        {
            get => AnimationBase.Get(WeatherAnimationId);
            set => WeatherAnimationId = value?.Id ?? Guid.Empty;
        }
        public int WeatherXSpeed { get; set;}
        public int WeatherYSpeed { get; set; }
        public int WeatherIntensity { get; set; }
        
        [NotMapped]
        [JsonIgnore]
        public object MapLock => mMapLock;
        
        [JsonConstructor]
        public MapBase(Guid id, bool isClient) : base(id)
        {
            Name = "New Map";

            //Fill Tile Data with Nulled/Empty Data
            /* Each tile has a Guid for tileset, x and y integer for source tile in the set, and a byte for the tile type.
             * A Guid is 16 bytes, and the integers are both 4 bytes each, and the final byte is just 1 byte.
             * So the blob size is going to be LayerCount * MapWidth * MapHeight * (16 + 4 + 4 + 1)*/
            TileData = Compression.CompressPacket(new byte[Options.LayerCount * Options.MapWidth * Options.MapHeight * 25]);
            mCachedAttributeData = Compression.CompressPacket(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Attributes, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, ObjectCreationHandling = ObjectCreationHandling.Replace })));


            IsClient = isClient;
        }

        //EF Constructor
        public MapBase()
        {
            Name = "New Map";
            IsClient = false;
        }

        public MapBase(MapBase mapBase)
            : base(mapBase?.Id ?? Guid.Empty)
        {
            if (mapBase == null)
            {
                return;
            }

            lock (MapLock ?? throw new ArgumentNullException(nameof(MapLock), @"this"))
            {
                lock (mapBase.MapLock ?? throw new ArgumentNullException(nameof(mapBase.MapLock), nameof(mapBase)))
                {
                    var bf = new ByteBuffer();
                    Name = mapBase.Name;
                    Brightness = mapBase.Brightness;
                    IsIndoors = mapBase.IsIndoors;
                    if (Layers != null && mapBase.Layers != null)
                    {
                        if (Layers.Length < Options.LayerCount) Layers = new TileArray[Options.LayerCount];
                        for (var i = 0; i < Options.LayerCount; i++)
                        {
                            Layers[i].Tiles = new Tile[Options.MapWidth, Options.MapHeight];
                            for (var x = 0; x < Options.MapWidth; x++)
                            {
                                for (var y = 0; y < Options.MapHeight; y++)
                                {
                                    Layers[i].Tiles[x, y] = new Tile
                                    {
                                        TilesetId = mapBase.Layers[i].Tiles[x, y].TilesetId,
                                        X = mapBase.Layers[i].Tiles[x, y].X,
                                        Y = mapBase.Layers[i].Tiles[x, y].Y,
                                        Autotile = mapBase.Layers[i].Tiles[x, y].Autotile
                                    };
                                }
                            }
                        }
                    }
                    for (var x = 0; x < Options.MapWidth; x++)
                    {
                        for (var y = 0; y < Options.MapHeight; y++)
                        {
                            if (Attributes == null)
                            {
                                continue;
                            }

                            if (mapBase.Attributes?[x, y] == null)
                            {
                                Attributes[x, y] = null;
                            }
                            else
                            {
                                Attributes[x, y] = mapBase.Attributes[x, y].Clone();
                            }
                        }
                    }
                    for (var i = 0; i < mapBase.Spawns?.Count; i++)
                    {
                        Spawns.Add(new NpcSpawn(mapBase.Spawns[i]));
                    }
                    for (var i = 0; i < mapBase.Lights?.Count; i++)
                    {
                        Lights.Add(new LightBase(mapBase.Lights[i]));
                    }



                    foreach (var record in mapBase.LocalEvents)
                    {
                        var evt = new EventBase(record.Key, record.Value?.JsonData);
                        LocalEvents?.Add(record.Key, evt);
                    }

                    EventIds?.Clear();
                    EventIds?.AddRange(mapBase.EventIds?.ToArray() ?? new Guid[] {});
                }
            }
        }

        public class MapInstances : DatabaseObjectLookup
        {
            private readonly DatabaseObjectLookup mBaseLookup;

            public MapInstances(DatabaseObjectLookup baseLookup)
            {
                if (baseLookup == null) throw new ArgumentNullException();
                mBaseLookup = baseLookup;
            }

            internal override bool InternalSet(IDatabaseObject value, bool overwrite)
            {
                mBaseLookup?.InternalSet(value, overwrite);
                return base.InternalSet(value, overwrite);
            }

            public override bool Delete(IDatabaseObject value)
            {
                mBaseLookup?.Delete(value);
                return base.Delete(value);
            }

            public override void Clear()
            {
                mBaseLookup?.Clear();
                base.Clear();
            }
        }
    }
}