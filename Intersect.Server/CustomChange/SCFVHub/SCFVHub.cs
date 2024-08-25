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
            _saveUserList(type, []);
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
    public List<SCFVUser> GetPresenceList(string date)
    {
        return SCFVPresenceBase.GetPresenceByName($"Presence-{date}")?.PresenceList ?? [];
    }

    // This method is called by the client to update the presence list
    public void UpdatePresenceList(string workerId, string username, bool isAdding)
    {
        var presence = SCFVPresenceBase.GetPresenceByName();
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

        // remove duplicates
        presence.PresenceList = presence.PresenceList
            .GroupBy(u => u.Name)
            .Select(g => g.First())
            .ToList();

        DbInterface.SaveGameObject(presence);
        _ = Clients.All.SendAsync("update");
    }

    // This method is called by the admin to toggle the event
    public void ToggleEvent()
    {
        var config = GetConfig();
        config.IsOpen = !config.IsOpen;
        _saveConfig(config);

        _ = Clients.All.SendAsync("update");
    }

    // This method is called by the admin to add a user to the event
    public void AddUser(string username)
    {
        var userList = GetUserList(SCFVUserType.Night);
        if (string.IsNullOrWhiteSpace(username) || userList.Contains(username))
        {
            return;
        }

        userList.Add(username);
        _saveUserList(SCFVUserType.Night, userList);

        _ = Clients.All.SendAsync("update");
    }

    // This method is called by the admin to remove a user from the event
    public void RemoveUser(string username)
    {
        var userList = GetUserList(SCFVUserType.Night);
        if (string.IsNullOrWhiteSpace(username) || !userList.Contains(username))
        {
            return;
        }

        _ = userList.Remove(username);
        _saveUserList(SCFVUserType.Night, userList);

        _ = Clients.All.SendAsync("update");
    }

    public void UpdateAll(string date, SCFVUserType userType, string worker, List<string> userList, List<string> presenceNames)
    {
        _saveUserList(userType, userList);

        var presence = SCFVPresenceBase.GetPresenceByName($"Presence-{date}");
        if (presence == default)
        {
            _ = DbInterface.AddGameObject(GameObjectType.SCFVPresence);
            presence = SCFVPresenceBase.GetPresenceByName();
        }

        presence.PresenceList.Clear();
        foreach (var name in presenceNames)
        {
            presence.PresenceList.Add(
                new SCFVUser
                {
                    ID = worker,
                    Name = name,
                    LastModified = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss")
                }
            );
        }

        // remove duplicates
        presence.PresenceList = presence.PresenceList
            .GroupBy(u => u.Name)
            .Select(g => g.First())
            .ToList();

        DbInterface.SaveGameObject(presence);

        _logger.Info($"The user and today's presence list for {userType} has been updated.");
    }
}
