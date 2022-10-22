using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intersect.Config
{
    public partial class PartyOptions
    {
        /// <summary>
        /// Will multiply the exp of all npcs by the defined number
        /// </summary>
        public float GlobalBonusExperience = 1f;

        /// <summary>
        /// Sets a percentage of damage required for <see cref="BonusExperiencePercentPerMember"/> to be applied
        /// </summary>
        public int DefaultBonusDamageExperience = 20;
    }
}
