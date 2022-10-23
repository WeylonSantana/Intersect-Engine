using System;
using System.Text.Json.Serialization;
using Intersect.GameObjects;
using Intersect.Server.Localization;
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

        /// <summary>
        /// Function that seeks a profession, if it did not exist, it would add it
        /// </summary>
        /// <param name="professionBaseId">ProfessionBase Id</param>
        /// <returns>Profession or Null</returns>
        public Profession GetOrAddProfession(Guid professionBaseId)
        {
            if (professionBaseId == null || professionBaseId == Guid.Empty)
            {
                return null;
            }

            if (ProfessionBase.Get(professionBaseId) == null)
            {
                return null;
            }

            //try find the profession and return to user
            foreach (var p in PlayerProfessions.Professions)
            {
                if (p.ProfessionBaseId == professionBaseId)
                {
                    return PlayerProfessions;
                }
            }

            //If the profession was not found above, then add one in the first empty slot it finds.
            var professionData = new ProfessionData(professionBaseId);
            PlayerProfessions.Professions.Add(professionData);

            return PlayerProfessions;
        }

        /// <summary>
        /// Set the profession level
        /// </summary>
        /// <param name="professionBaseId"></param>
        /// <param name="value"></param>
        public void SetProfessionLevel(Guid professionBaseId, int value)
        {
            var profession = GetOrAddProfession(professionBaseId);

            if (profession == null)
            {
                return;
            }

            foreach(var p in profession.Professions)
            {
                if (p.ProfessionBaseId == professionBaseId)
                {
                    var descriptor = ProfessionBase.Get(professionBaseId);
                    p.Level = (int) Math.Min(value, descriptor.MaxLevel);

                    PacketSender.SendChatMsg(
                        this,
                        Strings.Player.LevelUpProfession.ToString(descriptor.Name, p.Level),
                        Enums.ChatMessageType.Experience
                    );

                    PacketSender.SendPlayerProfessions(this);
                }
            }
        }

        /// <summary>
        /// Give profession exp
        /// </summary>
        /// <param name="professionBaseId"></param>
        /// <param name="amount"></param>
        public void GiveProfessionExp(Guid professionBaseId, long amount)
        {
            var profession = GetOrAddProfession(professionBaseId);

            if (profession == null)
            {
                return;
            }

            foreach(var p in profession.Professions)
            {
                if(p.ProfessionBaseId == professionBaseId)
                {
                    var descriptor = ProfessionBase.Get(professionBaseId);

                    if(p.Level == descriptor.MaxLevel)
                    {
                        PacketSender.SendChatMsg(this, "Profession in max level", Enums.ChatMessageType.Experience);
                        return;
                    }

                    p.Exp += amount;
                    if (p.Exp < 0)
                    {
                        p.Exp = 0;
                    }

                    var levelCount = 0;
                    while (p.Exp >= descriptor.ExperienceToNextLevel(p.Level + levelCount) &&
                        descriptor.ExperienceToNextLevel(p.Level + levelCount) > 0)
                    {
                        p.Exp -= descriptor.ExperienceToNextLevel(p.Level + levelCount);
                        levelCount++;
                    }

                    if(levelCount > 0)
                    {
                        p.Level += levelCount;
                        p.Level = (int) Math.Min(p.Level, descriptor.MaxLevel);
                        PacketSender.SendChatMsg(this, Strings.Player.LevelUpProfession.ToString(descriptor.Name, p.Level), Enums.ChatMessageType.Experience);
                    }

                    PacketSender.SendPlayerProfessions(this);
                }
            }
        }
    }
}
