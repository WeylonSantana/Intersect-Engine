using Intersect.Localization;
using Newtonsoft.Json;

namespace Intersect.Client.Localization;

public static partial class Strings
{
    public partial struct General
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString FpsLabelFormat = @"{0}fps";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString Information = @"Information";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString MapItemStackable = @"{01} {00}";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString None = @"None";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString Okay = @"Okay";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString PingLabelFormat = @"{0}ms";
    }
}
