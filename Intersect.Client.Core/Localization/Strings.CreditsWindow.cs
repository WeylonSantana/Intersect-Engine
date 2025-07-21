using Intersect.Localization;
using Newtonsoft.Json;

namespace Intersect.Client.Localization;

public static partial class Strings
{
    public partial struct CreditsWindow
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString Back = @"Back to Main Menu";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString Title = @"Credits";
    }
}
