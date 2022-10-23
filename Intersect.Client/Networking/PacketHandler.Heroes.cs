using Intersect.Client.General;
using Intersect.Client.Professions;
using Intersect.Network;
using Intersect.Network.Packets.Server;

namespace Intersect.Client.Networking
{
    internal sealed partial class PacketHandler
    {
        //ProfessionPacket
        public void HandlePacket(IPacketSender packetSender, ProfessionPacket packet)
        {
            if (Globals.Me != null)
            {
                Globals.Me.PlayerProfessions = new Profession()
                {
                    Id = packet.Id,
                    PlayerId = packet.PlayerId,
                    Professions = packet.Professions,
                };
            }
        }
    }
}
