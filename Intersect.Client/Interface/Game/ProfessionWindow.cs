using Intersect.Client.Core;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.General;
using Intersect.Client.Localization;
using Intersect.GameObjects;
using Intersect.Network.Packets.Server;

namespace Intersect.Client.Interface.Game
{
    public partial class ProfessionWindow
    {
        private WindowControl mProfessionWindow;

        private bool mInitialized = false;

        private ListBox mProfessions;

        private ImagePanel mIcon;

        private Label mProfessionName;

        private Label mProfessionLevel;

        private Label mProfessionMaxLevel;

        private Label mProfessionExp;

        private Label mProfessionDescriptionTitle;

        private Label mProfessionDescription;

        public ProfessionWindow(Canvas gameCanvas)
        {
            mProfessionWindow = new WindowControl(gameCanvas, Strings.GameMenu.Professions, false, "ProfessionsWindow");
            mProfessionWindow.DisableResizing();

            mProfessions = new ListBox(mProfessionWindow, "ProfessionList");

            mIcon = new ImagePanel(mProfessionWindow, "ProfessionIcon");

            //Labels
            mProfessionName = new Label(mProfessionWindow, "ProfessionName");
            mProfessionLevel = new Label(mProfessionWindow, "ProfessionLevel");
            mProfessionMaxLevel = new Label(mProfessionWindow, "ProfessionMaxLevel");
            mProfessionExp = new Label(mProfessionWindow, "ProfessionExp");
            mProfessionDescriptionTitle = new Label(mProfessionWindow, "ProfessionDescriptionTitle");
            mProfessionDescriptionTitle.Text = Strings.Profession.DescriptionTitle.ToString();
            mProfessionDescription = new Label(mProfessionWindow, "ProfessionDescription");

            mProfessionWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
        }

        public bool IsVisible()
        {
            return !mProfessionWindow.IsHidden;
        }

        public void Hide()
        {
            mProfessionWindow.IsHidden = true;
        }

        public void Show()
        {
            mProfessionWindow.IsHidden = false;
        }

        public void Update()
        {
            if(mProfessionWindow.IsHidden)
            {
                return;
            }

            if (!mInitialized)
            {
                mInitialized = true;
                mProfessions.RemoveAllRows();

                for (var i = 0; i < Globals.Me?.PlayerProfessions?.Professions?.Count; ++i)
                {
                    var profession = Globals.Me?.PlayerProfessions?.Professions[i];
                    var descriptor = ProfessionBase.Get(profession.ProfessionBaseId);

                    if (profession == null || descriptor == null)
                    {
                        continue;
                    }

                    var tmpRow = mProfessions?.AddRow(i + 1 + ") " + descriptor.Name);
                    if (tmpRow == null)
                    {
                        continue;
                    }

                    tmpRow.UserData = Globals.Me.PlayerProfessions.Professions[i];
                    tmpRow.Selected += tmpNode_Update;
                }

                //Load the profession data
                if (Globals.Me?.PlayerProfessions?.Professions?.Count > 0)
                {
                    mProfessions.SelectRow(0);
                    LoadProfessionItems(Globals.Me?.PlayerProfessions?.Professions[0]);
                }
            }

            UpdateCurrentProfession();
        }

        void tmpNode_Update(Base sender, ItemSelectedEventArgs arguments)
        {
            var index = mProfessions.SelectedRowIndex;
            LoadProfessionItems(Globals.Me?.PlayerProfessions?.Professions[index]);
        }

        private void UpdateCurrentProfession()
        {
            var index = mProfessions.SelectedRowIndex;

            if(mProfessions.RowCount != Globals.Me?.PlayerProfessions?.Professions?.Count)
            {
                mInitialized = false;
            }

            LoadProfessionItems(Globals.Me?.PlayerProfessions?.Professions[index]);
        }

        private void LoadProfessionItems(ProfessionData data)
        {
            var itemTex = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Spell, data.Icon);
            if (itemTex != null)
            {
                mIcon.Texture = itemTex;
            }
            else
            {
                if (mIcon.Texture != null)
                {
                    mIcon.Texture = null;
                }
            }

            mProfessionName.Text = data.Name;
            mProfessionLevel.Text = Strings.Profession.Level.ToString(data.Level);
            mProfessionMaxLevel.Text = Strings.Profession.MaxLevel.ToString(data.MaxLevel);
            mProfessionExp.Text = Strings.Profession.Exp.ToString(data.Exp, data.NextLevelExp);
            mProfessionDescription.Text = data.Description;
        }
    }
}
