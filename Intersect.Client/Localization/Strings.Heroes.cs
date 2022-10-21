using Intersect.Localization;
using Newtonsoft.Json;

namespace Intersect.Client.Localization
{
    public partial class Strings
    {
        public partial struct Character
        {
            public static LocalizedString stat5 = @"{00}: {01}";

            public static LocalizedString stat6 = @"{00}: {01}";
        }

        public partial struct Combat
        {
            public static LocalizedString stat5 = @"Accuracy";

            public static LocalizedString stat6 = @"Evasion";
        }

        public partial struct SpellDescription
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public static LocalizedString HastePositiveEffect = @"Haste Change:";

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public static LocalizedString HasteNegativeEffect = @"Slowness Change:";

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public static LocalizedString SwiftChange = @"Swift Change:";

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public static LocalizedString CooldownChange = @"Cooldown Change:";

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public static LocalizedString ExpChange = @"Exp Bonus Change:";

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public static LocalizedString LuckChange = @"Luck Bonus Change:";

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public static LocalizedString TenacityChange = @"Tenacity Change:";

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public static LocalizedString LifestealChange = @"Lifesteal Change:";
        }
    }
}
