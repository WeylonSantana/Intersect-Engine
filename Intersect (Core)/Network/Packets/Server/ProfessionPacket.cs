using System;
using System.Collections.Generic;
using MessagePack;

namespace Intersect.Network.Packets.Server
{
    [MessagePackObject]
    public partial class ProfessionPacket : IntersectPacket
    {
        //Parameterless Constructor for MessagePack
        public ProfessionPacket()
        {
        }

        public ProfessionPacket(Guid id, Guid playerId, List<ProfessionData> professions)
        {
            Id = id;
            PlayerId = playerId;
            Professions = professions;
        }

        [Key(1)]
        public Guid Id { get; set; }

        [Key(2)]
        public Guid PlayerId { get; set; }

        [Key(3)]
        public List<ProfessionData> Professions { get; set; }
    }


    [MessagePackObject]
    public partial class ProfessionData
    {
        public ProfessionData()
        {
            ProfessionBaseId = default;
            Level = 1;
            Exp = 0;
            NextLevelExp = 1;
            MaxLevel = 1;
            Name = default;
            Description = default;
        }

        [Key(1)]
        public Guid ProfessionBaseId { get; set; }

        [Key(2)]
        public int Level { get; set; }

        [Key(3)]
        public long Exp { get; set; }

        [Key(4)]
        public long NextLevelExp { get; set; }

        [Key(5)]
        public long MaxLevel { get; set; }

        [Key(6)]
        public string Name { get; set; }

        [Key(7)]
        public string Description { get; set; }

        [Key(8)]
        public string Icon { get; set; }
    }
}
