using System.ComponentModel.DataAnnotations.Schema;
using Intersect.CustomChange.SCFVHub;
using Intersect.Extensions;
using Intersect.Models;
using Newtonsoft.Json;

namespace Intersect.CustomChange;

public partial class SCFVPresenceBase : DatabaseObject<SCFVPresenceBase>, IFolderable
{
    /// <inheritdoc />
    public string Folder { get; set; } = "";

    [NotMapped]
    public List<SCFVUser> PresenceList { get; set; } = [];

    [JsonIgnore]
    [Column("PresenceList")]
    public string PresenceListJson
    {
        get => JsonConvert.SerializeObject(PresenceList);
        set => PresenceList = JsonConvert.DeserializeObject<List<SCFVUser>>(value) ?? [];
    }

    [JsonConstructor]
    public SCFVPresenceBase(Guid id) : base(id)
    {
        Name = $"Presence-{DateTimeOffset.Now:dd-MM-yyyy}";
    }

    //Parameterless constructor for EF
    public SCFVPresenceBase()
    {
    }

    public static SCFVPresenceBase? GetPresenceByName(string name = default)
    {
        name ??= $"Presence-{DateTimeOffset.Now:dd-MM-yyyy}";

        if (!Names.Contains(name))
        {
            return default;
        }

        return Get(IdFromList(Names.IndexOf(name)));
    }
}
