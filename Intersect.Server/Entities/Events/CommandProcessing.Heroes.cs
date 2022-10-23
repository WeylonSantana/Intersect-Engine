using System.Collections.Generic;
using Intersect.GameObjects.Events.Commands;
using Intersect.Enums;
using Intersect.GameObjects;

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

        //Give Profession Experience Command
        private static void ProcessCommand(
            GiveProfessionExpCommand command,
            Player player,
            Event instance,
            CommandInstance stackInfo,
            Stack<CommandInstance> callStack
        )
        {
            var quantity = command.Exp;
            if (command.UseVariable)
            {
                switch (command.VariableType)
                {
                    case VariableTypes.PlayerVariable:
                        quantity = player.GetVariableValue(command.VariableId).Integer;

                        break;
                    case VariableTypes.ServerVariable:
                        quantity = (long) ServerVariableBase.Get(command.VariableId)?.Value.Integer;
                        break;
                }
            }

            player.GiveProfessionExp(command.ProfessionId, quantity);
        }
    }
}
