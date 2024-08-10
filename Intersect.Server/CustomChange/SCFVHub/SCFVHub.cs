#pragma warning disable CA1822 // Mark members as static

using Intersect.CustomChange;
using Intersect.CustomChange.SCFVHub;
using Intersect.Enums;
using Intersect.Logging;
using Intersect.Server.Core;
using Intersect.Server.Database;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Intersect.Server.CustomChange.SCFVHub;

public class SCFVHub : Hub
{
    private static readonly object _lock = new();

    private Logger _logger => ServerContext.Instance.Logger;

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    // This method is called by the client to get the current state of the event
    public SCFVConfig GetConfig()
    {
        if (!Directory.Exists("resources-custom"))
        {
            _ = Directory.CreateDirectory("resources-custom");
            _saveConfig(new SCFVConfig());
        }

        var json = File.ReadAllText("resources-custom/config.json");
        return JsonConvert.DeserializeObject<SCFVConfig>(json) ?? new SCFVConfig();
    }

    private void _saveConfig(SCFVConfig config)
    {
        File.WriteAllText(
            "resources-custom/config.json",
            JsonConvert.SerializeObject(
                config,
                Formatting.Indented
            )
        );
    }

    // This method is called by the client to get the current state of the event
    public List<string> GetUserList(SCFVUserType type)
    {
        if (!Directory.Exists("resources-custom"))
        {
            _ = Directory.CreateDirectory("resources-custom");
            _saveUserList(type, new List<string>());
        }

        var json = File.ReadAllText($"resources-custom/users-{type.ToString().ToLowerInvariant()}.json");
        return JsonConvert.DeserializeObject<List<string>>(json) ?? [];
    }

    private void _saveUserList(SCFVUserType type, List<string> userList)
    {
        File.WriteAllText(
            $"resources-custom/users-{type.ToString().ToLowerInvariant()}.json",
            JsonConvert.SerializeObject(
                userList.Distinct().ToList(),
                Formatting.Indented
            )
        );
    }

    // This method is called by the client to get the current presence list
    public List<SCFVUser> GetPresenceList(long? timestamp)
    {
        var date = timestamp.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(timestamp.Value) : DateTimeOffset.Now;
        var presenceName = $"Presence-{date:dd-MM-yyyy}";

        var presence = SCFVPresenceBase.GetPresenceByName(presenceName);
        return presence?.PresenceList ?? [];
    }

    // This method is called by the client to update the presence list
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

                _logger.Info($"The worker {workerId} has added {username} to the presence list.");
            }
            else
            {
                _ = presence.PresenceList.RemoveAll(u => u.Name == username);
                _logger.Info($"The worker {workerId} has removed {username} from the presence list.");
            }

            presence.PresenceList = presence.PresenceList.Distinct().ToList();
            DbInterface.SaveGameObject(presence);
        }

        await Clients.All.SendAsync("update");
    }

    // This method is called by the admin to toggle the event
    public async Task ToggleEvent()
    {
        var config = GetConfig();
        config.IsOpen = !config.IsOpen;
        _saveConfig(config);

        await Clients.All.SendAsync("update");
    }

    // This method is called by the admin to add a user to the event
    public async Task AddUser(string username)
    {
        var userList = GetUserList(SCFVUserType.Night);
        if (string.IsNullOrWhiteSpace(username) || userList.Contains(username))
        {
            return;
        }

        userList.Add(username);
        _saveUserList(SCFVUserType.Night, userList);

        await Clients.All.SendAsync("update");
    }

    // This method is called by the admin to remove a user from the event
    public async Task RemoveUser(string username)
    {
        var userList = GetUserList(SCFVUserType.Night);
        if (string.IsNullOrWhiteSpace(username) || !userList.Contains(username))
        {
            return;
        }

        _ = userList.Remove(username);
        _saveUserList(SCFVUserType.Night, userList);

        await Clients.All.SendAsync("update");
    }
}
