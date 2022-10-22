namespace Intersect.Config
{
    public partial class PartyOptions
    {
        
        /// <summary>
        /// Defines the maximum amount of members a party can have.
        /// </summary>
        public int MaximumMembers = 4;

        public int SharedXpRange = 40;

        public int NpcDeathCommonEventStartRange = 0;

        /// <summary>
        /// It will determine a bonus to add to the base npc exp per member
        /// Eg:10. A monster that would give 100 XP, now gives 120, when 2 in the party, 130 when 3, and 140 when 4. (10% extra by party member)
        /// PS: I, Weylon, removed the division of exp for reasons of my own, so the above calculation does not consider division by exp
        /// Each member receives the quoted values.
        /// </summary>
        public float BonusExperiencePercentPerMember = 10;

    }
}
