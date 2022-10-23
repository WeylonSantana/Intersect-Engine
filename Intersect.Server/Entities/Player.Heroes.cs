using System;
using System.Text.Json.Serialization;
using Intersect.GameObjects;
using Intersect.Network.Packets.Server;
using Intersect.Server.Database.PlayerData.Players;
using Intersect.Server.Networking;
using Intersect.Utilities;

namespace Intersect.Server.Entities
{
    public partial class Player
    {

        [JsonIgnore]
        public virtual Profession PlayerProfessions { get; set; } = new Profession();

        public long CalculateExperienceFactor(long exp, int enemyLevel, int playerLevel)
        {
            double levelFactor = MathHelper.Clamp(enemyLevel / (double)playerLevel, 0.0, 2.0);
            return (long)((exp * Options.Party.GlobalBonusExperience) * levelFactor);
        }

        //Professions Manangement
        public Profession AddPlayerProfession(Guid professionBaseId)
        {
            if (professionBaseId == null || professionBaseId == Guid.Empty)
            {
                return null;
            }

            if (ProfessionBase.Get(professionBaseId) == null)
            {
                return null;
            }

            var professionData = new ProfessionData(professionBaseId);
            PlayerProfessions.Professions.Add(professionData);

            return PlayerProfessions;
        }

        public Profession GetPlayerProfession(Guid professionBaseId)
        {
            foreach (var p in PlayerProfessions.Professions)
            {
                if (p.ProfessionBaseId == professionBaseId)
                {
                    return PlayerProfessions;
                }
            }

            return null;
        }

        public void SetProfessionLevel(Guid professionBaseId, int value)
        {
            if (professionBaseId == null || professionBaseId == Guid.Empty)
            {
                return;
            }

            var profession = GetPlayerProfession(professionBaseId);

            if (profession != null)
            {
                foreach(var p in profession.Professions)
                {
                    if(p.ProfessionBaseId == professionBaseId)
                    {
                        p.Level = value;
                        PacketSender.SendPlayerProfessions(this);
                    }
                }
            }
            else
            {
                var newProfession = AddPlayerProfession(professionBaseId);
                if (newProfession != null)
                {
                    SetProfessionLevel(professionBaseId, value);
                }
            }
        }
    }
}
