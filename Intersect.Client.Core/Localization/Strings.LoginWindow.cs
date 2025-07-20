using Intersect.Localization;
using Newtonsoft.Json;

namespace Intersect.Client.Localization;

public static partial class Strings
{
    public partial struct LoginWindow
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString ForgotPassword = @"Forgot Password?";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString Login = @"Login";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString Password = @"Password";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString Register = @"Register";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString SavePassword = @"Save Password";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString Title = @"Login";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString Username = @"Username";
    }
}
