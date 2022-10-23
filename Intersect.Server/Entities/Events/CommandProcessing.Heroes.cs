using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intersect.GameObjects.Events.Commands;
using Intersect.Server.Database.PlayerData.Players;

namespace Intersect.Server.Entities.Events
{
    public partial class CommandProcessing
    {
        //Change Profession Level Command
        private static void ProcessCommand(
            SetProfessionLevelCommand command,
            Player player,
            Event instance,
            CommandInstance stackInfo,
            Stack<CommandInstance> callStack
        )
        {
            player.SetProfessionLevel(command.ProfessionId, command.Level);
        }
    }
}
