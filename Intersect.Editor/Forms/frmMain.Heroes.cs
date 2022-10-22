using System;
using Intersect.Editor.Networking;
using Intersect.Enums;

namespace Intersect.Editor.Forms
{
    public partial class FrmMain
    {
        private void professionsEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PacketSender.SendOpenEditor(GameObjectType.Professions);
        }
    }
}
