using Intersect.Client.Localization;
using Intersect.Network;

namespace Intersect.Client.Networking;

public static partial class NetworkStatusExtensions
{
    public static string ToLocalizedString(this NetworkStatus networkStatus)
    {
        return networkStatus switch
        {
            NetworkStatus.Unknown => (string)Strings.Server.Unknown,
            NetworkStatus.Connecting => (string)Strings.Server.Connecting,
            NetworkStatus.Online => (string)Strings.Server.Online,
            NetworkStatus.Offline => (string)Strings.Server.Offline,
            NetworkStatus.Failed => (string)Strings.Server.Failed,
            NetworkStatus.VersionMismatch => (string)Strings.Server.VersionMismatch,
            NetworkStatus.ServerFull => (string)Strings.Server.ServerFull,
            NetworkStatus.HandshakeFailure => (string)Strings.Server.HandshakeFailure,
            NetworkStatus.Quitting => string.Empty,
            _ => throw new ArgumentOutOfRangeException(nameof(networkStatus), networkStatus, null),
        };
    }
}
