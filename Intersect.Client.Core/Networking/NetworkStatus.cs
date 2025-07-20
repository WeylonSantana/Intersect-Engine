using Intersect.Client.Localization;
using Intersect.Network;

namespace Intersect.Client.Networking;

public static partial class NetworkStatusExtensions
{
    public static string ToLocalizedString(this NetworkStatus networkStatus)
    {
        return networkStatus switch
        {
            NetworkStatus.Unknown => (string)Strings.ServerStatus.Unknown,
            NetworkStatus.Connecting => (string)Strings.ServerStatus.Connecting,
            NetworkStatus.Online => (string)Strings.ServerStatus.Online,
            NetworkStatus.Offline => (string)Strings.ServerStatus.Offline,
            NetworkStatus.Failed => (string)Strings.ServerStatus.Failed,
            NetworkStatus.VersionMismatch => (string)Strings.ServerStatus.VersionMismatch,
            NetworkStatus.ServerFull => (string)Strings.ServerStatus.ServerFull,
            NetworkStatus.HandshakeFailure => (string)Strings.ServerStatus.HandshakeFailure,
            NetworkStatus.Quitting => string.Empty,
            _ => throw new ArgumentOutOfRangeException(nameof(networkStatus), networkStatus, null),
        };
    }
}
