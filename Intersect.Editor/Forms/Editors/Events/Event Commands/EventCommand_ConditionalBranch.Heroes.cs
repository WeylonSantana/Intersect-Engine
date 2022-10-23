using System;
using Intersect.Editor.Localization;
using Intersect.Enums;
using Intersect.GameObjects;
using Intersect.GameObjects.Events;

namespace Intersect.Editor.Forms.Editors.Events.Event_Commands
{
    public partial class EventCommandConditionalBranch
    {
        public void InitExtraLocalization()
        {
            cmbProfessionComparator.Items.Clear();
            for (var i = 0; i < Strings.EventConditional.comparators.Count; i++)
            {
                cmbProfessionComparator.Items.Add(Strings.EventConditional.comparators[i]);
            }

            grpProfessionLevelIs.Text = Strings.EventConditional.professiontitle;
            lblProfession.Text = Strings.EventConditional.profession;
            lblProfessionComparator.Text = Strings.EventConditional.comparator;
            lblProfessionValue.Text = Strings.EventConditional.professionvalue;
        }

        private void cmbProfessions_SelectedIndexChanged(object sender, EventArgs e)
        {
            nudProfessionValue.Maximum = ProfessionBase.Get(ProfessionBase.IdFromList(cmbProfessions.SelectedIndex)).MaxLevel;
        }

        private void SetupFormValues(ProfessionLevelIs condition)
        {
            nudProfessionValue.Value = condition.Value;
            cmbProfessions.SelectedIndex = ProfessionBase.ListIndex(condition.ProfessionId);
            cmbProfessionComparator.SelectedIndex = (int)condition.Comparator;
        }

        private void SaveFormValues(ProfessionLevelIs condition)
        {
            condition.ProfessionId = ProfessionBase.IdFromList(cmbProfessions.SelectedIndex);
            condition.Comparator = (VariableComparators)cmbProfessionComparator.SelectedIndex;
            condition.Value = (int)nudProfessionValue.Value;
        }
    }
}
