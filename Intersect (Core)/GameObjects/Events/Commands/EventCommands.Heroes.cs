using System;

namespace Intersect.GameObjects.Events.Commands
{
    public class SetProfessionLevelCommand : EventCommand
    {
        public override EventCommandType Type { get; } = EventCommandType.SetProfessionLevel;

        public int Level { get; set; }

        public Guid ProfessionId { get; set; }
    }
}
