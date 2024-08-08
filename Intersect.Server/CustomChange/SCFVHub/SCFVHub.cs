#pragma warning disable CA1822 // Mark members as static

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
        var filename = $"resources-custom/database/presence-{date:yyyy-MM-dd}.json";

        if (!Directory.Exists("resources-custom") || !Directory.Exists("resources-custom/database"))
        {
            if (!Directory.Exists("resources-custom"))
            {
                _ = Directory.CreateDirectory("resources-custom");
            }

            if (!Directory.Exists("resources-custom/database"))
            {
                _ = Directory.CreateDirectory("resources-custom/database");
            }
        }

        if (!File.Exists(filename))
        {
            return [];
        }

        var json = File.ReadAllText(filename);
        return JsonConvert.DeserializeObject<List<SCFVUser>>(json) ?? [];
    }

    public async Task UpdatePresenceList(string workerId, string username, bool isAdding)
    {
        var date = DateTimeOffset.Now;
        var filename = $"resources-custom/database/presence-{date:yyyy-MM-dd}.json";
        var presenceList = GetPresenceList(null);

        lock (_lock)
        {
            if (isAdding)
            {
                presenceList.Add(new SCFVUser { ID = workerId, Name = username, LastModified = date.ToString("yyyy-MM-dd HH:mm:ss") });
            }
            else
            {
                _ = presenceList.RemoveAll(u => u.Name == username);
            }

            File.WriteAllText(
                filename,
                JsonConvert.SerializeObject(
                    presenceList,
                    Formatting.Indented
                )
            );
        }

        await Clients.All.SendAsync("PresenceListUpdated", presenceList);
    }
}
