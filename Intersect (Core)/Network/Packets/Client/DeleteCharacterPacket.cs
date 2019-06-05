﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intersect.Network.Packets.Client
{
    public class DeleteCharacterPacket : CerasPacket
    {
        public Guid CharacterId { get; set; }

        public DeleteCharacterPacket(Guid charId)
        {
            CharacterId = charId;
        }
    }
}
