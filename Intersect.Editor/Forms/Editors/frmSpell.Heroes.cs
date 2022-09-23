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
            chkStatusPersist.Text = Strings.SpellEditor.StatusPersist;
        }

        private void UpdateExtraEditor()
        {
            nudAccuracy.Value = mEditorItem.Combat.StatDiff[(int) Stats.Accuracy];
            nudEvasion.Value = mEditorItem.Combat.StatDiff[(int) Stats.Evasion];
            nudAccuracyPercentage.Value = mEditorItem.Combat.PercentageStatDiff[(int) Stats.Accuracy];
            nudEvasionPercentage.Value = mEditorItem.Combat.PercentageStatDiff[(int) Stats.Evasion];
            nudPercentageEffect.Value = mEditorItem.Combat.EffectPercentageValue;
            chkStatusPersist.Checked = mEditorItem.Combat.StatusPersist;
        }

        void ExtraStatusSetup()
        {
            switch (cmbExtraEffect.SelectedIndex)
            {
                case (int) StatusTypes.Haste:
                case (int) StatusTypes.Swift:
                case (int) StatusTypes.CooldownChange:
                case (int) StatusTypes.ExpChange:
                case (int) StatusTypes.LuckChange:
                case (int) StatusTypes.TenacityChange:
                case (int) StatusTypes.LifestealChange:
                case (int) StatusTypes.ManastealChange:
                    lblPercentageEffect.Show();
                    nudPercentageEffect.Show();

                    break;

                default:
                    lblPercentageEffect.Hide();
                    nudPercentageEffect.Hide();
                    break;
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

        private void nudPercentageEffect_ValueChanged(object sender, EventArgs e)
        {
            mEditorItem.Combat.EffectPercentageValue = (int) nudPercentageEffect.Value;
        }

        private void chkStatusPersist_CheckedChanged(object sender, EventArgs e)
        {
            mEditorItem.Combat.StatusPersist = chkStatusPersist.Checked;
        }
    }
}
