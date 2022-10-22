namespace Intersect.Editor.Forms.Editors
{
    partial class FrmProfession
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmProfession));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip = new DarkUI.Controls.DarkToolStrip();
            this.toolStripItemNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripItemDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAlphabetical = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripItemCopy = new System.Windows.Forms.ToolStripButton();
            this.toolStripItemPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripItemUndo = new System.Windows.Forms.ToolStripButton();
            this.grpProfessions = new DarkUI.Controls.DarkGroupBox();
            this.btnClearSearch = new DarkUI.Controls.DarkButton();
            this.txtSearch = new DarkUI.Controls.DarkTextBox();
            this.lstGameObjects = new Intersect.Editor.Forms.Controls.GameObjectList();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.grpExpGrid = new DarkUI.Controls.DarkGroupBox();
            this.btnUpdateExpGrid = new DarkUI.Controls.DarkButton();
            this.expGrid = new System.Windows.Forms.DataGridView();
            this.grpGeneral = new DarkUI.Controls.DarkGroupBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.txtDesc = new DarkUI.Controls.DarkTextBox();
            this.lblExpIncrease = new System.Windows.Forms.Label();
            this.lblExpBase = new System.Windows.Forms.Label();
            this.lblMaxLevel = new System.Windows.Forms.Label();
            this.nudExpIncrease = new DarkUI.Controls.DarkNumericUpDown();
            this.nudExpBase = new DarkUI.Controls.DarkNumericUpDown();
            this.nudMaxLevel = new DarkUI.Controls.DarkNumericUpDown();
            this.btnAddFolder = new DarkUI.Controls.DarkButton();
            this.lblFolder = new System.Windows.Forms.Label();
            this.cmbFolder = new DarkUI.Controls.DarkComboBox();
            this.cmbIcon = new DarkUI.Controls.DarkComboBox();
            this.lblIcon = new System.Windows.Forms.Label();
            this.picProfession = new System.Windows.Forms.PictureBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new DarkUI.Controls.DarkTextBox();
            this.btnCancel = new DarkUI.Controls.DarkButton();
            this.btnSave = new DarkUI.Controls.DarkButton();
            this.toolStrip.SuspendLayout();
            this.grpProfessions.SuspendLayout();
            this.pnlContainer.SuspendLayout();
            this.grpExpGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.expGrid)).BeginInit();
            this.grpGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudExpIncrease)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExpBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picProfession)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.AutoSize = false;
            this.toolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.toolStrip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripItemNew,
            this.toolStripSeparator1,
            this.toolStripItemDelete,
            this.toolStripSeparator2,
            this.btnAlphabetical,
            this.toolStripSeparator4,
            this.toolStripItemCopy,
            this.toolStripItemPaste,
            this.toolStripSeparator3,
            this.toolStripItemUndo});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
            this.toolStrip.Size = new System.Drawing.Size(734, 25);
            this.toolStrip.TabIndex = 44;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripItemNew
            // 
            this.toolStripItemNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripItemNew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripItemNew.Image = ((System.Drawing.Image)(resources.GetObject("toolStripItemNew.Image")));
            this.toolStripItemNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripItemNew.Name = "toolStripItemNew";
            this.toolStripItemNew.Size = new System.Drawing.Size(23, 22);
            this.toolStripItemNew.Text = "New";
            this.toolStripItemNew.Click += new System.EventHandler(this.toolStripItemNew_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripItemDelete
            // 
            this.toolStripItemDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripItemDelete.Enabled = false;
            this.toolStripItemDelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripItemDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolStripItemDelete.Image")));
            this.toolStripItemDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripItemDelete.Name = "toolStripItemDelete";
            this.toolStripItemDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripItemDelete.Text = "Delete";
            this.toolStripItemDelete.Click += new System.EventHandler(this.toolStripItemDelete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator2.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAlphabetical
            // 
            this.btnAlphabetical.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAlphabetical.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnAlphabetical.Image = ((System.Drawing.Image)(resources.GetObject("btnAlphabetical.Image")));
            this.btnAlphabetical.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAlphabetical.Name = "btnAlphabetical";
            this.btnAlphabetical.Size = new System.Drawing.Size(23, 22);
            this.btnAlphabetical.Text = "Order Chronologically";
            this.btnAlphabetical.Click += new System.EventHandler(this.btnAlphabetical_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator4.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripItemCopy
            // 
            this.toolStripItemCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripItemCopy.Enabled = false;
            this.toolStripItemCopy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripItemCopy.Image = ((System.Drawing.Image)(resources.GetObject("toolStripItemCopy.Image")));
            this.toolStripItemCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripItemCopy.Name = "toolStripItemCopy";
            this.toolStripItemCopy.Size = new System.Drawing.Size(23, 22);
            this.toolStripItemCopy.Text = "Copy";
            this.toolStripItemCopy.Click += new System.EventHandler(this.toolStripItemCopy_Click);
            // 
            // toolStripItemPaste
            // 
            this.toolStripItemPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripItemPaste.Enabled = false;
            this.toolStripItemPaste.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripItemPaste.Image = ((System.Drawing.Image)(resources.GetObject("toolStripItemPaste.Image")));
            this.toolStripItemPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripItemPaste.Name = "toolStripItemPaste";
            this.toolStripItemPaste.Size = new System.Drawing.Size(23, 22);
            this.toolStripItemPaste.Text = "Paste";
            this.toolStripItemPaste.Click += new System.EventHandler(this.toolStripItemPaste_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator3.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripItemUndo
            // 
            this.toolStripItemUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripItemUndo.Enabled = false;
            this.toolStripItemUndo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripItemUndo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripItemUndo.Image")));
            this.toolStripItemUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripItemUndo.Name = "toolStripItemUndo";
            this.toolStripItemUndo.Size = new System.Drawing.Size(23, 22);
            this.toolStripItemUndo.Text = "Undo";
            this.toolStripItemUndo.Click += new System.EventHandler(this.toolStripItemUndo_Click);
            // 
            // grpProfessions
            // 
            this.grpProfessions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.grpProfessions.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.grpProfessions.Controls.Add(this.btnClearSearch);
            this.grpProfessions.Controls.Add(this.txtSearch);
            this.grpProfessions.Controls.Add(this.lstGameObjects);
            this.grpProfessions.ForeColor = System.Drawing.Color.Gainsboro;
            this.grpProfessions.Location = new System.Drawing.Point(12, 28);
            this.grpProfessions.Name = "grpProfessions";
            this.grpProfessions.Size = new System.Drawing.Size(203, 483);
            this.grpProfessions.TabIndex = 45;
            this.grpProfessions.TabStop = false;
            this.grpProfessions.Text = "Professions";
            // 
            // btnClearSearch
            // 
            this.btnClearSearch.Location = new System.Drawing.Point(179, 13);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Padding = new System.Windows.Forms.Padding(5);
            this.btnClearSearch.Size = new System.Drawing.Size(18, 20);
            this.btnClearSearch.TabIndex = 28;
            this.btnClearSearch.Text = "X";
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtSearch.Location = new System.Drawing.Point(6, 13);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(167, 20);
            this.txtSearch.TabIndex = 27;
            this.txtSearch.Text = "Search...";
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lstGameObjects
            // 
            this.lstGameObjects.AllowDrop = true;
            this.lstGameObjects.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.lstGameObjects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstGameObjects.ForeColor = System.Drawing.Color.Gainsboro;
            this.lstGameObjects.HideSelection = false;
            this.lstGameObjects.ImageIndex = 0;
            this.lstGameObjects.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.lstGameObjects.Location = new System.Drawing.Point(6, 39);
            this.lstGameObjects.Name = "lstGameObjects";
            this.lstGameObjects.SelectedImageIndex = 0;
            this.lstGameObjects.Size = new System.Drawing.Size(191, 438);
            this.lstGameObjects.TabIndex = 26;
            // 
            // pnlContainer
            // 
            this.pnlContainer.AutoScroll = true;
            this.pnlContainer.Controls.Add(this.grpExpGrid);
            this.pnlContainer.Controls.Add(this.grpGeneral);
            this.pnlContainer.Location = new System.Drawing.Point(221, 28);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(501, 443);
            this.pnlContainer.TabIndex = 46;
            this.pnlContainer.Visible = false;
            // 
            // grpExpGrid
            // 
            this.grpExpGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.grpExpGrid.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.grpExpGrid.Controls.Add(this.btnUpdateExpGrid);
            this.grpExpGrid.Controls.Add(this.expGrid);
            this.grpExpGrid.ForeColor = System.Drawing.Color.Gainsboro;
            this.grpExpGrid.Location = new System.Drawing.Point(2, 230);
            this.grpExpGrid.Margin = new System.Windows.Forms.Padding(2);
            this.grpExpGrid.Name = "grpExpGrid";
            this.grpExpGrid.Padding = new System.Windows.Forms.Padding(2);
            this.grpExpGrid.Size = new System.Drawing.Size(496, 211);
            this.grpExpGrid.TabIndex = 38;
            this.grpExpGrid.TabStop = false;
            this.grpExpGrid.Text = "Experience Overrides";
            // 
            // btnUpdateExpGrid
            // 
            this.btnUpdateExpGrid.Location = new System.Drawing.Point(409, 186);
            this.btnUpdateExpGrid.Margin = new System.Windows.Forms.Padding(2);
            this.btnUpdateExpGrid.Name = "btnUpdateExpGrid";
            this.btnUpdateExpGrid.Padding = new System.Windows.Forms.Padding(5);
            this.btnUpdateExpGrid.Size = new System.Drawing.Size(83, 21);
            this.btnUpdateExpGrid.TabIndex = 39;
            this.btnUpdateExpGrid.Text = "Update Grid";
            this.btnUpdateExpGrid.Click += new System.EventHandler(this.btnUpdateExpGrid_Click);
            // 
            // expGrid
            // 
            this.expGrid.AllowUserToAddRows = false;
            this.expGrid.AllowUserToDeleteRows = false;
            this.expGrid.AllowUserToResizeColumns = false;
            this.expGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(53)))), ((int)(((byte)(55)))));
            this.expGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.expGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.expGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.expGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.expGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.expGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.expGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.expGrid.ColumnHeadersHeight = 24;
            this.expGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.expGrid.EnableHeadersVisualStyles = false;
            this.expGrid.Location = new System.Drawing.Point(7, 18);
            this.expGrid.MultiSelect = false;
            this.expGrid.Name = "expGrid";
            this.expGrid.RowHeadersVisible = false;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.expGrid.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.expGrid.Size = new System.Drawing.Size(485, 163);
            this.expGrid.TabIndex = 0;
            // 
            // grpGeneral
            // 
            this.grpGeneral.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.grpGeneral.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.grpGeneral.Controls.Add(this.lblDesc);
            this.grpGeneral.Controls.Add(this.txtDesc);
            this.grpGeneral.Controls.Add(this.lblExpIncrease);
            this.grpGeneral.Controls.Add(this.lblExpBase);
            this.grpGeneral.Controls.Add(this.lblMaxLevel);
            this.grpGeneral.Controls.Add(this.nudExpIncrease);
            this.grpGeneral.Controls.Add(this.nudExpBase);
            this.grpGeneral.Controls.Add(this.nudMaxLevel);
            this.grpGeneral.Controls.Add(this.btnAddFolder);
            this.grpGeneral.Controls.Add(this.lblFolder);
            this.grpGeneral.Controls.Add(this.cmbFolder);
            this.grpGeneral.Controls.Add(this.cmbIcon);
            this.grpGeneral.Controls.Add(this.lblIcon);
            this.grpGeneral.Controls.Add(this.picProfession);
            this.grpGeneral.Controls.Add(this.lblName);
            this.grpGeneral.Controls.Add(this.txtName);
            this.grpGeneral.ForeColor = System.Drawing.Color.Gainsboro;
            this.grpGeneral.Location = new System.Drawing.Point(2, 1);
            this.grpGeneral.Name = "grpGeneral";
            this.grpGeneral.Size = new System.Drawing.Size(496, 224);
            this.grpGeneral.TabIndex = 14;
            this.grpGeneral.TabStop = false;
            this.grpGeneral.Text = "General";
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Location = new System.Drawing.Point(10, 113);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(35, 13);
            this.lblDesc.TabIndex = 80;
            this.lblDesc.Text = "Desc:";
            // 
            // txtDesc
            // 
            this.txtDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtDesc.Location = new System.Drawing.Point(11, 129);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(481, 85);
            this.txtDesc.TabIndex = 79;
            this.txtDesc.TextChanged += new System.EventHandler(this.txtDesc_TextChanged);
            // 
            // lblExpIncrease
            // 
            this.lblExpIncrease.AutoSize = true;
            this.lblExpIncrease.Location = new System.Drawing.Point(230, 75);
            this.lblExpIncrease.Name = "lblExpIncrease";
            this.lblExpIncrease.Size = new System.Drawing.Size(89, 13);
            this.lblExpIncrease.TabIndex = 77;
            this.lblExpIncrease.Text = "Exp Increase (%):";
            // 
            // lblExpBase
            // 
            this.lblExpBase.AutoSize = true;
            this.lblExpBase.Location = new System.Drawing.Point(230, 49);
            this.lblExpBase.Name = "lblExpBase";
            this.lblExpBase.Size = new System.Drawing.Size(55, 13);
            this.lblExpBase.TabIndex = 76;
            this.lblExpBase.Text = "Exp Base:";
            // 
            // lblMaxLevel
            // 
            this.lblMaxLevel.AutoSize = true;
            this.lblMaxLevel.Location = new System.Drawing.Point(230, 20);
            this.lblMaxLevel.Name = "lblMaxLevel";
            this.lblMaxLevel.Size = new System.Drawing.Size(59, 13);
            this.lblMaxLevel.TabIndex = 75;
            this.lblMaxLevel.Text = "Max Level:";
            // 
            // nudExpIncrease
            // 
            this.nudExpIncrease.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.nudExpIncrease.ForeColor = System.Drawing.Color.Gainsboro;
            this.nudExpIncrease.Location = new System.Drawing.Point(319, 71);
            this.nudExpIncrease.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudExpIncrease.Name = "nudExpIncrease";
            this.nudExpIncrease.Size = new System.Drawing.Size(173, 20);
            this.nudExpIncrease.TabIndex = 73;
            this.nudExpIncrease.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudExpIncrease.ValueChanged += new System.EventHandler(this.nudExpIncrease_ValueChanged);
            // 
            // nudExpBase
            // 
            this.nudExpBase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.nudExpBase.ForeColor = System.Drawing.Color.Gainsboro;
            this.nudExpBase.Location = new System.Drawing.Point(319, 45);
            this.nudExpBase.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudExpBase.Name = "nudExpBase";
            this.nudExpBase.Size = new System.Drawing.Size(173, 20);
            this.nudExpBase.TabIndex = 72;
            this.nudExpBase.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudExpBase.ValueChanged += new System.EventHandler(this.nudExpBase_ValueChanged);
            // 
            // nudMaxLevel
            // 
            this.nudMaxLevel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.nudMaxLevel.ForeColor = System.Drawing.Color.Gainsboro;
            this.nudMaxLevel.Location = new System.Drawing.Point(319, 19);
            this.nudMaxLevel.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudMaxLevel.Name = "nudMaxLevel";
            this.nudMaxLevel.Size = new System.Drawing.Size(173, 20);
            this.nudMaxLevel.TabIndex = 71;
            this.nudMaxLevel.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudMaxLevel.ValueChanged += new System.EventHandler(this.nudMaxLevel_ValueChanged);
            // 
            // btnAddFolder
            // 
            this.btnAddFolder.Location = new System.Drawing.Point(177, 45);
            this.btnAddFolder.Name = "btnAddFolder";
            this.btnAddFolder.Padding = new System.Windows.Forms.Padding(5);
            this.btnAddFolder.Size = new System.Drawing.Size(18, 21);
            this.btnAddFolder.TabIndex = 67;
            this.btnAddFolder.Text = "+";
            this.btnAddFolder.Click += new System.EventHandler(this.btnAddFolder_Click);
            // 
            // lblFolder
            // 
            this.lblFolder.AutoSize = true;
            this.lblFolder.Location = new System.Drawing.Point(6, 49);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(39, 13);
            this.lblFolder.TabIndex = 66;
            this.lblFolder.Text = "Folder:";
            // 
            // cmbFolder
            // 
            this.cmbFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.cmbFolder.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.cmbFolder.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.cmbFolder.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.cmbFolder.DrawDropdownHoverOutline = false;
            this.cmbFolder.DrawFocusRectangle = false;
            this.cmbFolder.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbFolder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbFolder.ForeColor = System.Drawing.Color.Gainsboro;
            this.cmbFolder.FormattingEnabled = true;
            this.cmbFolder.Location = new System.Drawing.Point(60, 45);
            this.cmbFolder.Name = "cmbFolder";
            this.cmbFolder.Size = new System.Drawing.Size(113, 21);
            this.cmbFolder.TabIndex = 65;
            this.cmbFolder.Text = null;
            this.cmbFolder.TextPadding = new System.Windows.Forms.Padding(2);
            this.cmbFolder.SelectedIndexChanged += new System.EventHandler(this.cmbFolder_SelectedIndexChanged);
            // 
            // cmbIcon
            // 
            this.cmbIcon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.cmbIcon.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.cmbIcon.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.cmbIcon.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.cmbIcon.DrawDropdownHoverOutline = false;
            this.cmbIcon.DrawFocusRectangle = false;
            this.cmbIcon.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbIcon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbIcon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbIcon.ForeColor = System.Drawing.Color.Gainsboro;
            this.cmbIcon.FormattingEnabled = true;
            this.cmbIcon.Items.AddRange(new object[] {
            "None"});
            this.cmbIcon.Location = new System.Drawing.Point(60, 87);
            this.cmbIcon.Name = "cmbIcon";
            this.cmbIcon.Size = new System.Drawing.Size(135, 21);
            this.cmbIcon.TabIndex = 11;
            this.cmbIcon.Text = "None";
            this.cmbIcon.TextPadding = new System.Windows.Forms.Padding(2);
            this.cmbIcon.SelectedIndexChanged += new System.EventHandler(this.cmbIcon_SelectedIndexChanged);
            // 
            // lblIcon
            // 
            this.lblIcon.AutoSize = true;
            this.lblIcon.Location = new System.Drawing.Point(57, 71);
            this.lblIcon.Name = "lblIcon";
            this.lblIcon.Size = new System.Drawing.Size(31, 13);
            this.lblIcon.TabIndex = 6;
            this.lblIcon.Text = "Icon:";
            // 
            // picProfession
            // 
            this.picProfession.BackColor = System.Drawing.Color.Black;
            this.picProfession.Location = new System.Drawing.Point(11, 76);
            this.picProfession.Name = "picProfession";
            this.picProfession.Size = new System.Drawing.Size(32, 32);
            this.picProfession.TabIndex = 4;
            this.picProfession.TabStop = false;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(6, 20);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtName.Location = new System.Drawing.Point(60, 19);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(135, 20);
            this.txtName.TabIndex = 0;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(532, 477);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(5);
            this.btnCancel.Size = new System.Drawing.Size(190, 28);
            this.btnCancel.TabIndex = 82;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(336, 477);
            this.btnSave.Name = "btnSave";
            this.btnSave.Padding = new System.Windows.Forms.Padding(5);
            this.btnSave.Size = new System.Drawing.Size(190, 28);
            this.btnSave.TabIndex = 81;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // FrmProfession
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.ClientSize = new System.Drawing.Size(734, 520);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpProfessions);
            this.Controls.Add(this.toolStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmProfession";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Professions Editor";
            this.Load += new System.EventHandler(this.FrmProfession_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.grpProfessions.ResumeLayout(false);
            this.grpProfessions.PerformLayout();
            this.pnlContainer.ResumeLayout(false);
            this.grpExpGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.expGrid)).EndInit();
            this.grpGeneral.ResumeLayout(false);
            this.grpGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudExpIncrease)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExpBase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picProfession)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripItemNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripItemDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnAlphabetical;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        public System.Windows.Forms.ToolStripButton toolStripItemCopy;
        public System.Windows.Forms.ToolStripButton toolStripItemPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        public System.Windows.Forms.ToolStripButton toolStripItemUndo;
        private DarkUI.Controls.DarkGroupBox grpProfessions;
        private DarkUI.Controls.DarkButton btnClearSearch;
        private DarkUI.Controls.DarkTextBox txtSearch;
        private Controls.GameObjectList lstGameObjects;
        private System.Windows.Forms.Panel pnlContainer;
        private DarkUI.Controls.DarkGroupBox grpGeneral;
        private System.Windows.Forms.Label lblExpIncrease;
        private System.Windows.Forms.Label lblExpBase;
        private System.Windows.Forms.Label lblMaxLevel;
        private DarkUI.Controls.DarkNumericUpDown nudExpIncrease;
        private DarkUI.Controls.DarkNumericUpDown nudExpBase;
        private DarkUI.Controls.DarkNumericUpDown nudMaxLevel;
        private DarkUI.Controls.DarkButton btnAddFolder;
        private System.Windows.Forms.Label lblFolder;
        private DarkUI.Controls.DarkComboBox cmbFolder;
        private DarkUI.Controls.DarkComboBox cmbIcon;
        private System.Windows.Forms.Label lblIcon;
        private System.Windows.Forms.PictureBox picProfession;
        private System.Windows.Forms.Label lblName;
        private DarkUI.Controls.DarkTextBox txtName;
        private System.Windows.Forms.Label lblDesc;
        private DarkUI.Controls.DarkTextBox txtDesc;
        private DarkUI.Controls.DarkButton btnCancel;
        private DarkUI.Controls.DarkButton btnSave;
        private DarkUI.Controls.DarkGroupBox grpExpGrid;
        private DarkUI.Controls.DarkButton btnUpdateExpGrid;
        private System.Windows.Forms.DataGridView expGrid;
    }
}
