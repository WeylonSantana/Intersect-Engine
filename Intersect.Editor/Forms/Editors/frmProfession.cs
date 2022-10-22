using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DarkUI.Forms;
using Intersect.Editor.Content;
using Intersect.Editor.General;
using Intersect.Editor.Localization;
using Intersect.Editor.Networking;
using Intersect.Enums;
using Intersect.GameObjects;
using Intersect.Utilities;

namespace Intersect.Editor.Forms.Editors
{
    public partial class FrmProfession : EditorForm
    {
        private List<ProfessionBase> mChanged = new List<ProfessionBase>();

        private string mCopiedItem;

        private ProfessionBase mEditorItem;

        private List<string> mKnownFolders = new List<string>();

        public FrmProfession()
        {
            ApplyHooks();
            InitializeComponent();

            lstGameObjects.Init(UpdateToolStripItems, AssignEditorItem, toolStripItemNew_Click, toolStripItemCopy_Click, toolStripItemUndo_Click, toolStripItemPaste_Click, toolStripItemDelete_Click);
        }

        private void AssignEditorItem(Guid id)
        {
            mEditorItem = ProfessionBase.Get(id);
            UpdateEditor();
        }

        protected override void GameObjectUpdatedDelegate(GameObjectType type)
        {
            if (type == GameObjectType.Professions)
            {
                InitEditor();
                if (mEditorItem != null && !ProfessionBase.Lookup.Values.Contains(mEditorItem))
                {
                    mEditorItem = null;
                    UpdateEditor();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            foreach (var item in mChanged)
            {
                item.RestoreBackup();
                item.DeleteBackup();
            }

            Hide();
            Globals.CurrentEditor = -1;
            Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Send Changed items
            foreach (var item in mChanged)
            {
                PacketSender.SendSaveObject(item);
                item.DeleteBackup();
            }

            Hide();
            Globals.CurrentEditor = -1;
            Dispose();
        }

        private void FrmProfession_Load(object sender, EventArgs e)
        {
            InitLocalization();
            UpdateEditor();

            cmbIcon.Items.Clear();
            cmbIcon.Items.Add(Strings.General.None);
            var professionIcons = GameContentManager.GetSmartSortedTextureNames(GameContentManager.TextureType.Spell);
            cmbIcon.Items.AddRange(professionIcons);
        }

        private void InitLocalization()
        {
            Text = Strings.ProfessionEditor.Title;
            toolStripItemNew.Text = Strings.ProfessionEditor.New;
            toolStripItemDelete.Text = Strings.ProfessionEditor.Delete;
            toolStripItemCopy.Text = Strings.ProfessionEditor.Copy;
            toolStripItemPaste.Text = Strings.ProfessionEditor.Paste;
            toolStripItemUndo.Text = Strings.ProfessionEditor.Undo;

            grpProfessions.Text = Strings.ProfessionEditor.Professions;

            //Searching/Sorting
            btnAlphabetical.ToolTipText = Strings.ProfessionEditor.Sortalphabetically;
            txtSearch.Text = Strings.ProfessionEditor.Searchplaceholder;
            lblFolder.Text = Strings.ProfessionEditor.Folderlabel;

            lblName.Text = Strings.ProfessionEditor.Name;
            lblFolder.Text = Strings.ProfessionEditor.Folderlabel;
            lblIcon.Text = Strings.ProfessionEditor.Icon;
            lblDesc.Text = Strings.ProfessionEditor.Description;
            lblMaxLevel.Text = Strings.ProfessionEditor.Maxlevel;
            lblExpBase.Text = Strings.ProfessionEditor.Expbase;
            lblExpIncrease.Text = Strings.ProfessionEditor.Expincrease;

            grpExpGrid.Text = Strings.ProfessionEditor.Expgrid;
            btnUpdateExpGrid.Text = Strings.ProfessionEditor.Updategrid;

            //Create EXP Grid...
            var levelCol = new DataGridViewTextBoxColumn();
            levelCol.HeaderText = Strings.ProfessionEditor.Gridlevel;
            levelCol.ReadOnly = true;
            levelCol.SortMode = DataGridViewColumnSortMode.NotSortable;

            var tnlCol = new DataGridViewTextBoxColumn();
            tnlCol.HeaderText = Strings.ProfessionEditor.Gridtnl;
            tnlCol.SortMode = DataGridViewColumnSortMode.NotSortable;

            var totalCol = new DataGridViewTextBoxColumn();
            totalCol.HeaderText = Strings.ProfessionEditor.Gridtotalexp;
            totalCol.ReadOnly = true;
            totalCol.SortMode = DataGridViewColumnSortMode.NotSortable;

            expGrid.Columns.Clear();
            expGrid.Columns.Add(levelCol);
            expGrid.Columns.Add(tnlCol);
            expGrid.Columns.Add(totalCol);

            btnSave.Text = Strings.ProfessionEditor.Save;
            btnCancel.Text = Strings.ProfessionEditor.Cancel;
        }

        private void UpdateEditor()
        {
            if (mEditorItem != null)
            {
                pnlContainer.Show();

                txtName.Text = mEditorItem.Name;
                cmbFolder.Text = mEditorItem.Folder;
                txtDesc.Text = mEditorItem.Description;
                nudMaxLevel.Value = mEditorItem.mMaxLevel;
                nudExpBase.Value = mEditorItem.BaseExp;
                nudExpIncrease.Value = mEditorItem.ExpIncrease;

                cmbIcon.SelectedIndex = cmbIcon.FindString(TextUtils.NullToNone(mEditorItem.Icon));
                picProfession.BackgroundImage?.Dispose();
                picProfession.BackgroundImage = null;
                if (cmbIcon.SelectedIndex > 0)
                {
                    picProfession.BackgroundImage = Image.FromFile("resources/spells/" + cmbIcon.Text);
                }

                UpdateExpGrid();

                if (mChanged.IndexOf(mEditorItem) == -1)
                {
                    mChanged.Add(mEditorItem);
                    mEditorItem.MakeBackup();
                }
            }
            else
            {
                pnlContainer.Hide();
            }

            UpdateToolStripItems();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            mEditorItem.Name = txtName.Text;
            lstGameObjects.UpdateText(txtName.Text);
        }

        private void cmbIcon_SelectedIndexChanged(object sender, EventArgs e)
        {
            mEditorItem.Icon = cmbIcon.Text;
            picProfession.BackgroundImage?.Dispose();
            picProfession.BackgroundImage = null;
            picProfession.BackgroundImage = cmbIcon.SelectedIndex > 0
                ? Image.FromFile("resources/spells/" + cmbIcon.Text)
                : null;
        }

        private void txtDesc_TextChanged(object sender, EventArgs e)
        {

        }

        private void nudMaxLevel_ValueChanged(object sender, EventArgs e)
        {
            mEditorItem.mMaxLevel = (int)nudMaxLevel.Value;
        }

        private void nudExpBase_ValueChanged(object sender, EventArgs e)
        {
            mEditorItem.BaseExp = (int)nudExpBase.Value;
        }

        private void nudExpIncrease_ValueChanged(object sender, EventArgs e)
        {
            mEditorItem.ExpIncrease = (int)nudExpIncrease.Value;
        }

        private void btnUpdateExpGrid_Click(object sender, EventArgs e)
        {
            UpdateExpGrid();
        }

        private void toolStripItemNew_Click(object sender, EventArgs e)
        {
            PacketSender.SendCreateObject(GameObjectType.Professions);
        }

        private void toolStripItemDelete_Click(object sender, EventArgs e)
        {
            if (mEditorItem != null && lstGameObjects.Focused)
            {
                if (DarkMessageBox.ShowWarning(
                        Strings.ProfessionEditor.Deleteprompt, Strings.ProfessionEditor.Deletetitle,
                        DarkDialogButton.YesNo
                    ) ==
                    DialogResult.Yes)
                {
                    PacketSender.SendDeleteObject(mEditorItem);
                }
            }
        }

        private void toolStripItemCopy_Click(object sender, EventArgs e)
        {
            if (mEditorItem != null && lstGameObjects.Focused)
            {
                mCopiedItem = mEditorItem.JsonData;
                toolStripItemPaste.Enabled = true;
            }
        }

        private void toolStripItemPaste_Click(object sender, EventArgs e)
        {
            if (mEditorItem != null && mCopiedItem != null && lstGameObjects.Focused)
            {
                mEditorItem.Load(mCopiedItem, true);
                UpdateEditor();
            }
        }

        private void toolStripItemUndo_Click(object sender, EventArgs e)
        {
            if (mChanged.Contains(mEditorItem) && mEditorItem != null)
            {
                if (DarkMessageBox.ShowWarning(
                        Strings.ProfessionEditor.Undoprompt, Strings.ProfessionEditor.Undotitle, DarkDialogButton.YesNo
                    ) ==
                    DialogResult.Yes)
                {
                    mEditorItem.RestoreBackup();
                    UpdateEditor();
                }
            }
        }

        private void UpdateToolStripItems()
        {
            toolStripItemCopy.Enabled = mEditorItem != null && lstGameObjects.Focused;
            toolStripItemPaste.Enabled = mEditorItem != null && mCopiedItem != null && lstGameObjects.Focused;
            toolStripItemDelete.Enabled = mEditorItem != null && lstGameObjects.Focused;
            toolStripItemUndo.Enabled = mEditorItem != null && lstGameObjects.Focused;
        }

        #region "Exp Grid"

        private void UpdateExpGrid()
        {
            expGrid.Rows.Clear();

            for (var i = 1; i <= mEditorItem.mMaxLevel; i++)
            {
                var index = expGrid.Rows.Add(i.ToString(), "", "");
                var row = expGrid.Rows[index];
                row.Cells[0].Style.SelectionBackColor = row.Cells[0].Style.BackColor;
                row.Cells[2].Style.SelectionBackColor = row.Cells[2].Style.BackColor;
            }

            UpdateExpGridValues(1);
        }

        private void UpdateExpGridValues(int start, int end = -1)
        {
            if (end == -1)
            {
                end = mEditorItem.mMaxLevel;
            }

            if (start > end)
            {
                return;
            }

            if (start < 1)
            {
                start = 1;
            }

            for (var i = start; i <= end; i++)
            {
                if (i < mEditorItem.mMaxLevel)
                {
                    if (mEditorItem.ExperienceOverrides.ContainsKey(i))
                    {
                        expGrid.Rows[i - 1].Cells[1].Value = Convert.ChangeType(
                            mEditorItem.ExperienceOverrides[i], expGrid.Rows[i - 1].Cells[1].ValueType
                        );

                        var style = expGrid.Rows[i - 1].Cells[1].InheritedStyle;
                        style.Font = new Font(style.Font, FontStyle.Bold);
                        expGrid.Rows[i - 1].Cells[1].Style.ApplyStyle(style);
                    }
                    else
                    {
                        expGrid.Rows[i - 1].Cells[1].Value = Convert.ChangeType(
                            mEditorItem.ExperienceCurve.Calculate(i), expGrid.Rows[i - 1].Cells[1].ValueType
                        );

                        expGrid.Rows[i - 1].Cells[1].Style.ApplyStyle(expGrid.Rows[i - 1].Cells[0].InheritedStyle);
                    }
                }
                else
                {
                    expGrid.Rows[i - 1].Cells[1].Value = Convert.ChangeType(0, expGrid.Rows[i - 1].Cells[1].ValueType);
                    expGrid.Rows[i - 1].Cells[1].ReadOnly = true;
                }

                if (i == 1)
                {
                    expGrid.Rows[i - 1].Cells[2].Value = Convert.ChangeType(0, expGrid.Rows[i - 1].Cells[1].ValueType);
                }
                else
                {
                    expGrid.Rows[i - 1].Cells[2].Value = Convert.ChangeType(
                        long.Parse(expGrid.Rows[i - 2].Cells[2].Value.ToString()) +
                        long.Parse(expGrid.Rows[i - 2].Cells[1].Value.ToString()),
                        expGrid.Rows[i - 1].Cells[2].ValueType
                    );
                }
            }
        }

        #endregion

        #region "Item List - Folders, Searching, Sorting, Etc"

        public void InitEditor()
        {
            //Collect folders
            var mFolders = new List<string>();
            foreach (var itm in ProfessionBase.Lookup)
            {
                if (!string.IsNullOrEmpty(((ProfessionBase)itm.Value).Folder) &&
                    !mFolders.Contains(((ProfessionBase)itm.Value).Folder))
                {
                    mFolders.Add(((ProfessionBase)itm.Value).Folder);
                    if (!mKnownFolders.Contains(((ProfessionBase)itm.Value).Folder))
                    {
                        mKnownFolders.Add(((ProfessionBase)itm.Value).Folder);
                    }
                }
            }

            mFolders.Sort();
            mKnownFolders.Sort();
            cmbFolder.Items.Clear();
            cmbFolder.Items.Add("");
            cmbFolder.Items.AddRange(mKnownFolders.ToArray());

            var items = ProfessionBase.Lookup.OrderBy(p => p.Value?.Name).Select(pair => new KeyValuePair<Guid, KeyValuePair<string, string>>(pair.Key,
                new KeyValuePair<string, string>(((ProfessionBase)pair.Value)?.Name ?? Models.DatabaseObject<ProfessionBase>.Deleted, ((ProfessionBase)pair.Value)?.Folder ?? ""))).ToArray();
            lstGameObjects.Repopulate(items, mFolders, btnAlphabetical.Checked, CustomSearch(), txtSearch.Text);
        }

        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            var folderName = "";
            var result = DarkInputBox.ShowInformation(
                Strings.ProfessionEditor.Folderprompt, Strings.ProfessionEditor.Foldertitle, ref folderName,
                DarkDialogButton.OkCancel
            );

            if (result == DialogResult.OK && !string.IsNullOrEmpty(folderName))
            {
                if (!cmbFolder.Items.Contains(folderName))
                {
                    mEditorItem.Folder = folderName;
                    lstGameObjects.ExpandFolder(folderName);
                    InitEditor();
                    cmbFolder.Text = folderName;
                }
            }
        }

        private void cmbFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            mEditorItem.Folder = cmbFolder.Text;
            InitEditor();
        }

        private void btnAlphabetical_Click(object sender, EventArgs e)
        {
            btnAlphabetical.Checked = !btnAlphabetical.Checked;
            InitEditor();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            InitEditor();
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = Strings.ProfessionEditor.Searchplaceholder;
            }
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            txtSearch.SelectAll();
            txtSearch.Focus();
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = Strings.ProfessionEditor.Searchplaceholder;
        }

        private bool CustomSearch()
        {
            return !string.IsNullOrWhiteSpace(txtSearch.Text) &&
                   txtSearch.Text != Strings.ProfessionEditor.Searchplaceholder;
        }

        private void txtSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text == Strings.ProfessionEditor.Searchplaceholder)
            {
                txtSearch.SelectAll();
            }
        }

        #endregion
    }
}
