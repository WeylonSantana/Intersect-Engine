using System;
using Intersect.Editor.Localization;
using Intersect.Enums;

namespace Intersect.Editor.Forms.Editors
{
    public partial class FrmSpell
    {
        private void ExtraFrmLoad()
        {
            nudAccuracy.Maximum = Options.MaxStatValue;
            nudAccuracy.Minimum = -Options.MaxStatValue;
            nudEvasion.Maximum = Options.MaxStatValue;
            nudEvasion.Minimum = -Options.MaxStatValue;
        }

        private void InitExtraLocalization()
        {
            lblAccuracy.Text = Strings.SpellEditor.Accuracy;
            lblEvasion.Text = Strings.SpellEditor.Evasion;
            lblPercentageEffect.Text = Strings.SpellEditor.EffectPercentageValue;
        }

        private void UpdateExtraEditor()
        {
            nudAccuracy.Value = mEditorItem.Combat.StatDiff[(int) Stats.Accuracy];
            nudEvasion.Value = mEditorItem.Combat.StatDiff[(int) Stats.Evasion];
            nudAccuracyPercentage.Value = mEditorItem.Combat.PercentageStatDiff[(int) Stats.Accuracy];
            nudEvasionPercentage.Value = mEditorItem.Combat.PercentageStatDiff[(int) Stats.Evasion];
            nudPercentageEffect.Value = mEditorItem.Combat.EffectPercentageValue;
        }

        private void HasteFormSetup()
        {
            if (cmbExtraEffect.SelectedIndex == 13) //Haste
            {
                lblPercentageEffect.Show();
                nudPercentageEffect.Show();
            }
            else
            {
                lblPercentageEffect.Hide();
                nudPercentageEffect.Hide();
            }
        }

        private void nudAccuracy_ValueChanged(object sender, EventArgs e)
        {
            mEditorItem.Combat.StatDiff[(int)Stats.Accuracy] = (int)nudAccuracy.Value;
        }

        private void nudEvasion_ValueChanged(object sender, EventArgs e)
        {
            mEditorItem.Combat.StatDiff[(int)Stats.Evasion] = (int)nudEvasion.Value;
        }

        private void nudAccuracyPercentage_ValueChanged(object sender, EventArgs e)
        {
            mEditorItem.Combat.PercentageStatDiff[(int)Stats.Accuracy] = (int)nudAccuracyPercentage.Value;
        }

        private void nudEvasionPercentage_ValueChanged(object sender, EventArgs e)
        {
            mEditorItem.Combat.PercentageStatDiff[(int)Stats.Evasion] = (int)nudEvasionPercentage.Value;
        }

        private void nudHasteChange_ValueChanged(object sender, EventArgs e)
        {
            mEditorItem.Combat.EffectPercentageValue = (int)nudPercentageEffect.Value;
        }
    }
}
