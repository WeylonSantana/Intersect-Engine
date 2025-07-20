using Intersect.Localization;
using Newtonsoft.Json;

namespace Intersect.Client.Localization;

public static partial class Strings
{
    public partial struct Errors
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString DisconnectionEvent = @"Please provide this message to your administrator: {0}";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString DisplayNotSupported = @"Invalid Display Configuration!";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString DisplayNotSupportedError = @"Fullscreen {00} resolution is not supported on this device!";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString HostNotFound = @"DNS resolution error, please report this to the game administrator.";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString InvalidStateReturnToMainMenu = @"An invalid state was detected and we had to return to the main menu.";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString LoadFile = @"Failed to load a {00}. Please send the game administrator a copy of your errors log file in the logs directory.";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString LostConnection = @"Lost connection to the game server. Please make sure you're connected to the internet and try again!";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString NotConnected = @"Not connected to the game server. Is it online?";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString OpenAllLink = @"https://goo.gl/Nbx6hx";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString OpenGlLink = @"https://goo.gl/RSP3ts";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString PasswordInvalid = @"Password is invalid. Please use alphanumeric characters with a length between 4 and 20.";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString ResourcesNotFound = @"The resources directory could not be found! Intersect will now close.";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString Title = @"Error!";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString UsernameInvalid = @"Username is invalid. Please use alphanumeric characters with a length between 2 and 20.";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public static LocalizedString WaitingForServer = @"Still waiting for the server to respond. If this message persists, please try restarting the game or contact the game administrator.";
    }
}
