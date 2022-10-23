using System;
using System.Collections.Generic;
using Intersect.Network.Packets.Server;

namespace Intersect.Client.Professions
{
    public partial class Profession
    {
        public Guid Id { get; set; }

        public Guid PlayerId { get; set; }

        public List<ProfessionData> Professions { get; set; }

        public void Load(Guid id, Guid playerId, List<ProfessionData> professions)
        {
            Id = id;
            PlayerId = playerId;
            Professions = professions;
        }
    }
}
