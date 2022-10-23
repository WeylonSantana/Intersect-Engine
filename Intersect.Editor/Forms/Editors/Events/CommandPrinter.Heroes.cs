using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intersect.Editor.Localization;
using Intersect.Editor.Maps;
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
    }
}
