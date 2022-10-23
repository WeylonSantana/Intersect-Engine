using System;
using Intersect.Enums;

namespace Intersect.GameObjects.Events.Commands
{
    public class SetProfessionLevelCommand : EventCommand
    {
        public override EventCommandType Type { get; } = EventCommandType.SetProfessionLevel;

        public int Level { get; set; }

        public Guid ProfessionId { get; set; }
    }

    public class GiveProfessionExpCommand : EventCommand
    {
        public override EventCommandType Type { get; } = EventCommandType.GiveProfessionExp;

        public long Exp { get; set; }

        public Guid ProfessionId { get; set; }

        /// <summary>
        /// Defines whether this event command will use a variable for processing or not.
        /// </summary>
        public bool UseVariable { get; set; } = false;

        /// <summary>
        /// Defines whether the variable used is a Player or Global variable.
        /// </summary>
        public VariableTypes VariableType { get; set; } = VariableTypes.PlayerVariable;

        /// <summary>
        /// The Variable Id to use.
        /// </summary>
        public Guid VariableId { get; set; }
    }
}
