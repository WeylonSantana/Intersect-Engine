using Intersect.Localization;
using Newtonsoft.Json;

namespace Intersect.Server.Localization
{
    public static partial class Strings
    {
        public sealed partial class CombatNamespace
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public readonly LocalizedString ConfusedFailedCast = @"Failed cast!";

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public readonly LocalizedString ConfusedFailedAttack = @"Failed attack!";
        }

        public sealed partial class PlayerNamespace
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public readonly LocalizedString LevelUpProfession = @"The profession {00} is now level {01}!";
        }
    }
}
