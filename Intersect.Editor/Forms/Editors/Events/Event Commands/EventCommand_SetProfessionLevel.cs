using Intersect.Editor.Localization;
using Intersect.GameObjects;
using Intersect.GameObjects.Events.Commands;
using System;
using System.Windows.Forms;

namespace Intersect.Editor.Forms.Editors.Events.Event_Commands
{
    public partial class EventCommandSetProfessionLevel : UserControl
    {
        private readonly FrmEvent mEventEditor;

        private SetProfessionLevelCommand mMyCommand;

        private ProfessionBase mProfession;

        public EventCommandSetProfessionLevel(SetProfessionLevelCommand refCommand, FrmEvent editor)
        {
            InitializeComponent();
            mMyCommand = refCommand;
            mEventEditor = editor;

            InitLocalization();

            if (mMyCommand.ProfessionId != Guid.Empty)
            {
                mProfession = ProfessionBase.Get(mMyCommand.ProfessionId);
            }

            cmbProfession.Items.Clear();
            cmbProfession.Items.AddRange(ProfessionBase.Names);
            cmbProfession.SelectedIndex = ProfessionBase.ListIndex(mMyCommand.ProfessionId);

            if(mProfession != null)
            {
                if (mMyCommand.Level <= 0 || mMyCommand.Level > mProfession.MaxLevel)
                {
                    mMyCommand.Level = 1;
                }

                nudLevel.Maximum = mMyCommand.ProfessionId != Guid.Empty
                    ? mProfession.MaxLevel
                    : ProfessionBase.Get(ProfessionBase.IdFromList(cmbProfession.SelectedIndex)).MaxLevel;

                nudLevel.Value = mMyCommand.Level;
            }
        }

        private void InitLocalization()
        {
            grpSetProfessionLevel.Text = Strings.EventSetProfessionLevel.title;
            lblProfession.Text = Strings.EventSetProfessionLevel.profession;
            lblLevel.Text = Strings.EventSetProfessionLevel.level;
            btnSave.Text = Strings.EventSetProfessionLevel.okay;
            btnCancel.Text = Strings.EventChangeLevel.cancel;
        }

        private void cmbProfession_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProfession.SelectedIndex > -1)
            {
                nudLevel.Maximum = ProfessionBase.Get(ProfessionBase.IdFromList(cmbProfession.SelectedIndex)).MaxLevel;
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            mMyCommand.ProfessionId = ProfessionBase.IdFromList(cmbProfession.SelectedIndex);
            mMyCommand.Level = (int)nudLevel.Value;
            mEventEditor.FinishCommandEdit();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            mEventEditor.CancelCommandEdit();
        }
    }
}
