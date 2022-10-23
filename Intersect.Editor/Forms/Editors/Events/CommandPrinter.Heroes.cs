using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intersect.Editor.Localization;
using Intersect.Editor.Maps;
using Intersect.Enums;
using Intersect.GameObjects;
using Intersect.GameObjects.Events.Commands;

namespace Intersect.Editor.Forms.Editors.Events
{
    public partial class CommandPrinter
    {
        private static string GetCommandText(SetProfessionLevelCommand command, MapInstance map)
        {
            return Strings.EventCommandList.setprofessionlevel.ToString(ProfessionBase.GetName(command.ProfessionId), command.Level);
        }

        private static string GetCommandText(GiveProfessionExpCommand command, MapInstance map)
        {
            if (command.UseVariable)
            {
                var exp = string.Empty;
                switch (command.VariableType)
                {
                    case VariableTypes.PlayerVariable:
                        exp = string.Format(@"({0}: {1})", Strings.EventGiveProfessionExp.PlayerVariable, PlayerVariableBase.GetName(command.VariableId));
                        break;
                    case VariableTypes.ServerVariable:
                        exp = string.Format(@"({0}: {1})", Strings.EventGiveProfessionExp.ServerVariable, ServerVariableBase.GetName(command.VariableId));
                        break;
                }

                return Strings.EventCommandList.giveprofessionexp.ToString(exp, ProfessionBase.GetName(command.ProfessionId));
            }
            else
            {
                return Strings.EventCommandList.giveprofessionexp.ToString(command.Exp, ProfessionBase.GetName(command.ProfessionId));
            }
        }
    }
}
