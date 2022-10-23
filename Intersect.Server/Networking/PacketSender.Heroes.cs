using Intersect.Network.Packets.Server;
using Intersect.Server.Entities;

namespace Intersect.Server.Networking
{
    public partial class PacketSender
    {
        //ProfessionsPacket
        public static void SendPlayerProfessions(Player player)
        {
            if (player == null)
            {
                return;
            }

            var professions = new ProfessionPacket(
                    player.PlayerProfessions.Id,
                    player.PlayerProfessions.PlayerId,
                    player.PlayerProfessions.Professions
                );

            player.SendPacket(professions);
        }
    }
}
