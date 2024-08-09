#pragma warning disable CA1822 // Mark members as static

using Intersect.CustomChange;
using Intersect.CustomChange.SCFVHub;
using Intersect.Enums;
using Intersect.Extensions;
using Intersect.Server.Database;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Intersect.Server.CustomChange.SCFVHub;

public class SCFVHub : Hub
{
    private static readonly object _lock = new();

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    public SCFVConfig GetConfig()
    {
        if (!Directory.Exists("resources-custom"))
        {
            _ = Directory.CreateDirectory("resources-custom");
            File.WriteAllText(
                "resources-custom/config.json",
                JsonConvert.SerializeObject(
                    new SCFVConfig(),
                    Formatting.Indented
                )
            );
        }

        var json = File.ReadAllText("resources-custom/config.json");
        return JsonConvert.DeserializeObject<SCFVConfig>(json) ?? new SCFVConfig();
    }

    public List<string> GetUserList(SCFVUserType type)
    {
        if (!Directory.Exists("resources-custom"))
        {
            _ = Directory.CreateDirectory("resources-custom");
            File.WriteAllText(
                $"resources-custom/users-{type.ToString().ToLowerInvariant()}.json",
                JsonConvert.SerializeObject(
                    new List<SCFVUser>(),
                    Formatting.Indented
                )
            );
        }

        var json = File.ReadAllText($"resources-custom/users-{type.ToString().ToLowerInvariant()}.json");
        return JsonConvert.DeserializeObject<List<string>>(json) ?? [];
    }

    public List<SCFVUser> GetPresenceList(long? timestamp)
    {
        var date = timestamp.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(timestamp.Value) : DateTimeOffset.Now;
        var presenceName = $"Presence-{date:dd-MM-yyyy}";

        var presence = SCFVPresenceBase.GetPresenceByName(presenceName);
        return presence?.PresenceList ?? [];
    }

    public async Task UpdatePresenceList(string workerId, string username, bool isAdding)
    {
        var presence = SCFVPresenceBase.GetPresenceByName();

        lock (_lock)
        {
            if (presence == default)
            {
                _ = DbInterface.AddGameObject(GameObjectType.SCFVPresence);
                presence = SCFVPresenceBase.GetPresenceByName();
            }

            if (isAdding)
            {
                presence.PresenceList.Add(
                    new SCFVUser
                    {
                        ID = workerId,
                        Name = username,
                        LastModified = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    }
                );
            }
            else
            {
                _ = presence.PresenceList.RemoveAll(u => u.Name == username);
            }

            DbInterface.SaveGameObject(presence);
        }

        await Clients.All.SendAsync("PresenceListUpdated", presence.PresenceList);
    }
}
