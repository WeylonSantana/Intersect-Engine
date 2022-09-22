using System;
using Intersect.Editor.Localization;
using Intersect.Enums;

namespace Intersect.Editor.Forms.Editors
{
    public partial class FrmNpc
    {
        private void ExtraFrmLoad()
        {
            nudAccuracy.Maximum = Options.MaxStatValue;
            nudEvasion.Maximum = Options.MaxStatValue;
        }

        private void InitExtraLocalization()
        {
            lblAccuracy.Text = Strings.NpcEditor.Accuracy;
            lblEvasion.Text = Strings.NpcEditor.Evasion;
            chkHaste.Text = Strings.NpcEditor.Immunities[StatusTypes.Haste];
            chkSwift.Text = Strings.NpcEditor.Immunities[StatusTypes.Swift];
            chkConfused.Text = Strings.NpcEditor.Immunities[StatusTypes.Confused];
        }

        private void UpdateExtraEditor()
        {
            nudAccuracy.Value = mEditorItem.Stats[(int) Stats.Accuracy];
            nudEvasion.Value = mEditorItem.Stats[(int) Stats.Evasion];
            chkHaste.Checked = mEditorItem.Immunities.Contains(StatusTypes.Haste);
            chkSwift.Checked = mEditorItem.Immunities.Contains(StatusTypes.Swift);
            chkConfused.Checked = mEditorItem.Immunities.Contains(StatusTypes.Confused);
        }

        private void nudAccuracy_ValueChanged(object sender, EventArgs e)
        {
            mEditorItem.Stats[(int)Stats.Accuracy] = (int)nudAccuracy.Value;
        }

        private void nudEvasion_ValueChanged(object sender, EventArgs e)
        {
            mEditorItem.Stats[(int)Stats.Evasion] = (int)nudEvasion.Value;
        }
        
        private void chkHaste_CheckedChanged(object sender, EventArgs e)
        {
            ChangeImmunity(StatusTypes.Haste, chkHaste.Checked);
        }

        private void chkSwift_CheckedChanged(object sender, EventArgs e)
        {
            ChangeImmunity(StatusTypes.Swift, chkSwift.Checked);
        }

        private void chkConfused_CheckedChanged(object sender, EventArgs e)
        {
            ChangeImmunity(StatusTypes.Confused, chkConfused.Checked);
        }
    }
}
