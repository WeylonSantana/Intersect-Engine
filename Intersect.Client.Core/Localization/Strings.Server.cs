using Intersect.Localization;
using Newtonsoft.Json;

namespace Intersect.Client.Localization;

public static partial class Strings
{
    public partial struct Server
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString Connecting = @"Connecting...";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString Failed = @"Network Error";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString HandshakeFailure = @"Handshake Error";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString Offline = @"Offline";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString Online = @"Online";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString ServerFull = @"Server is Full";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString StatusLabel = @"Server Status: {00}";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString Unknown = @"Unknown";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString VersionMismatch = @"Version Mismatch";
    }
}
