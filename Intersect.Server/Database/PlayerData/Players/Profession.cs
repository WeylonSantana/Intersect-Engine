using System;
using System.ComponentModel.DataAnnotations.Schema;
using Intersect.Server.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using Intersect.Network.Packets.Server;

namespace Intersect.Server.Database.PlayerData.Players
{
    public partial class Profession
    {
        public Profession()
        {
        }

        public Player Player { get; set; }

        /// <summary>
        /// The database Id of the profession.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), JsonIgnore]
        public Guid Id { get; protected set; }

        /// <summary>
        /// The id of the player.
        /// </summary>
        public Guid PlayerId { get; set; }

        /// <summary>
        /// Contains a record of all player professions
        /// </summary>
        [NotMapped][JsonIgnore]
        public List<ProfessionData> Professions { get; set; } = new List<ProfessionData>();

        [Column("Professions")][JsonIgnore]
        public string ProfessionsJson
        {
            get => JsonConvert.SerializeObject(Professions);
            set => Professions = JsonConvert.DeserializeObject<List<ProfessionData>>(value ?? "") ?? new List<ProfessionData>();
        }
    }
}
