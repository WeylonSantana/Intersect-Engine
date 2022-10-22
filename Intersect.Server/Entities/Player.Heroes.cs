using Intersect.Utilities;

namespace Intersect.Server.Entities
{
    public partial class Player
    {
        public long CalculateExperienceFactor(long exp, int enemyLevel, int playerLevel)
        {
            double levelFactor = MathHelper.Clamp(enemyLevel / (double)playerLevel, 0.0, 2.0);
            return (long)((exp * Options.Party.GlobalBonusExperience) * levelFactor);
        }
    }
}
