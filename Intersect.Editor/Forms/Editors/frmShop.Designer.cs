﻿using DarkUI.Controls;

namespace Intersect.Editor.Forms.Editors
{
    partial class FrmShop
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmShop));
            this.grpShops = new DarkUI.Controls.DarkGroupBox();
            this.btnClearSearch = new DarkUI.Controls.DarkButton();
            this.txtSearch = new DarkUI.Controls.DarkTextBox();
            this.lstGameObjects = new Intersect.Editor.Forms.Controls.GameObjectList();
            this.grpGeneral = new DarkUI.Controls.DarkGroupBox();
            this.nudBuyMultiplier = new DarkUI.Controls.DarkNumericUpDown();
            this.lblBuyMultiplier = new System.Windows.Forms.Label();
            this.lblSellSound = new System.Windows.Forms.Label();
            this.cmbSellSound = new DarkUI.Controls.DarkComboBox();
            this.lblBuySound = new System.Windows.Forms.Label();
            this.cmbBuySound = new DarkUI.Controls.DarkComboBox();
            this.btnAddFolder = new DarkUI.Controls.DarkButton();
            this.lblFolder = new System.Windows.Forms.Label();
            this.cmbFolder = new DarkUI.Controls.DarkComboBox();
            this.cmbDefaultCurrency = new DarkUI.Controls.DarkComboBox();
            this.lblDefaultCurrency = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new DarkUI.Controls.DarkTextBox();
            this.grpItemsSold = new DarkUI.Controls.DarkGroupBox();
            this.lblBuyItemPriceNum = new System.Windows.Forms.Label();
            this.lblSellItemCost = new System.Windows.Forms.Label();
            this.btnItemDown = new DarkUI.Controls.DarkButton();
            this.btnItemUp = new DarkUI.Controls.DarkButton();
            this.nudSellCost = new DarkUI.Controls.DarkNumericUpDown();
            this.cmbSellFor = new DarkUI.Controls.DarkComboBox();
            this.lblSellFor = new System.Windows.Forms.Label();
            this.lblSellCost = new System.Windows.Forms.Label();
            this.btnDelSoldItem = new DarkUI.Controls.DarkButton();
            this.btnAddSoldItem = new DarkUI.Controls.DarkButton();
            this.cmbAddSoldItem = new DarkUI.Controls.DarkComboBox();
            this.lblAddSoldItem = new System.Windows.Forms.Label();
            this.lstSoldItems = new System.Windows.Forms.ListBox();
            this.grpItemsBought = new DarkUI.Controls.DarkGroupBox();
            this.lblSellItemPriceNum = new System.Windows.Forms.Label();
            this.lblBuyItemCost = new System.Windows.Forms.Label();
            this.nudBuyAmount = new DarkUI.Controls.DarkNumericUpDown();
            this.cmbBuyFor = new DarkUI.Controls.DarkComboBox();
            this.lblBuyFor = new System.Windows.Forms.Label();
            this.lblBuyAmount = new System.Windows.Forms.Label();
            this.btnDelBoughtItem = new DarkUI.Controls.DarkButton();
            this.btnAddBoughtItem = new DarkUI.Controls.DarkButton();
            this.cmbAddBoughtItem = new DarkUI.Controls.DarkComboBox();
            this.lblItemBought = new System.Windows.Forms.Label();
            this.lstBoughtItems = new System.Windows.Forms.ListBox();
            this.rdoBuyBlacklist = new DarkUI.Controls.DarkRadioButton();
            this.rdoBuyWhitelist = new DarkUI.Controls.DarkRadioButton();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.grpTagsBought = new DarkUI.Controls.DarkGroupBox();
            this.btnRemoveTag = new DarkUI.Controls.DarkButton();
            this.btnAddTag = new DarkUI.Controls.DarkButton();
            this.cmbTags = new DarkUI.Controls.DarkComboBox();
            this.lblTag = new System.Windows.Forms.Label();
            this.lstBoughtTags = new System.Windows.Forms.ListBox();
            this.tdoTagBlacklist = new DarkUI.Controls.DarkRadioButton();
            this.rdoTagWhitelist = new DarkUI.Controls.DarkRadioButton();
            this.btnCancel = new DarkUI.Controls.DarkButton();
            this.btnSave = new DarkUI.Controls.DarkButton();
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
            this.btnNuke = new DarkUI.Controls.DarkButton();
            this.grpShops.SuspendLayout();
            this.grpGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBuyMultiplier)).BeginInit();
            this.grpItemsSold.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSellCost)).BeginInit();
            this.grpItemsBought.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBuyAmount)).BeginInit();
            this.pnlContainer.SuspendLayout();
            this.grpTagsBought.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpShops
            // 
            this.grpShops.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.grpShops.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.grpShops.Controls.Add(this.btnClearSearch);
            this.grpShops.Controls.Add(this.txtSearch);
            this.grpShops.Controls.Add(this.lstGameObjects);
            this.grpShops.ForeColor = System.Drawing.Color.Gainsboro;
            this.grpShops.Location = new System.Drawing.Point(12, 34);
            this.grpShops.Name = "grpShops";
            this.grpShops.Size = new System.Drawing.Size(203, 620);
            this.grpShops.TabIndex = 15;
            this.grpShops.TabStop = false;
            this.grpShops.Text = "Shops";
            // 
            // btnClearSearch
            // 
            this.btnClearSearch.Location = new System.Drawing.Point(179, 20);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Padding = new System.Windows.Forms.Padding(5);
            this.btnClearSearch.Size = new System.Drawing.Size(18, 20);
            this.btnClearSearch.TabIndex = 37;
            this.btnClearSearch.Text = "X";
            this.btnClearSearch.Click += new System.EventHandler(this.btnClearSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtSearch.Location = new System.Drawing.Point(6, 20);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(167, 20);
            this.txtSearch.TabIndex = 36;
            this.txtSearch.Text = "Search...";
            this.txtSearch.Click += new System.EventHandler(this.txtSearch_Click);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.Enter += new System.EventHandler(this.txtSearch_Enter);
            this.txtSearch.Leave += new System.EventHandler(this.txtSearch_Leave);
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
            this.lstGameObjects.Location = new System.Drawing.Point(6, 46);
            this.lstGameObjects.Name = "lstGameObjects";
            this.lstGameObjects.SelectedImageIndex = 0;
            this.lstGameObjects.Size = new System.Drawing.Size(191, 565);
            this.lstGameObjects.TabIndex = 35;
            // 
            // grpGeneral
            // 
            this.grpGeneral.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.grpGeneral.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.grpGeneral.Controls.Add(this.nudBuyMultiplier);
            this.grpGeneral.Controls.Add(this.lblBuyMultiplier);
            this.grpGeneral.Controls.Add(this.lblSellSound);
            this.grpGeneral.Controls.Add(this.cmbSellSound);
            this.grpGeneral.Controls.Add(this.lblBuySound);
            this.grpGeneral.Controls.Add(this.cmbBuySound);
            this.grpGeneral.Controls.Add(this.btnAddFolder);
            this.grpGeneral.Controls.Add(this.lblFolder);
            this.grpGeneral.Controls.Add(this.cmbFolder);
            this.grpGeneral.Controls.Add(this.cmbDefaultCurrency);
            this.grpGeneral.Controls.Add(this.lblDefaultCurrency);
            this.grpGeneral.Controls.Add(this.lblName);
            this.grpGeneral.Controls.Add(this.txtName);
            this.grpGeneral.ForeColor = System.Drawing.Color.Gainsboro;
            this.grpGeneral.Location = new System.Drawing.Point(-1, 2);
            this.grpGeneral.Name = "grpGeneral";
            this.grpGeneral.Size = new System.Drawing.Size(787, 104);
            this.grpGeneral.TabIndex = 16;
            this.grpGeneral.TabStop = false;
            this.grpGeneral.Text = "General";
            // 
            // nudBuyMultiplier
            // 
            this.nudBuyMultiplier.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.nudBuyMultiplier.DecimalPlaces = 2;
            this.nudBuyMultiplier.ForeColor = System.Drawing.Color.Gainsboro;
            this.nudBuyMultiplier.Location = new System.Drawing.Point(610, 13);
            this.nudBuyMultiplier.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            131072});
            this.nudBuyMultiplier.Name = "nudBuyMultiplier";
            this.nudBuyMultiplier.Size = new System.Drawing.Size(171, 20);
            this.nudBuyMultiplier.TabIndex = 50;
            this.nudBuyMultiplier.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.nudBuyMultiplier.ValueChanged += new System.EventHandler(this.darkNumericUpDown1_ValueChanged);
            // 
            // lblBuyMultiplier
            // 
            this.lblBuyMultiplier.AutoSize = true;
            this.lblBuyMultiplier.Location = new System.Drawing.Point(535, 17);
            this.lblBuyMultiplier.Name = "lblBuyMultiplier";
            this.lblBuyMultiplier.Size = new System.Drawing.Size(72, 13);
            this.lblBuyMultiplier.TabIndex = 50;
            this.lblBuyMultiplier.Text = "Buy Multiplier:";
            // 
            // lblSellSound
            // 
            this.lblSellSound.AutoSize = true;
            this.lblSellSound.Location = new System.Drawing.Point(270, 76);
            this.lblSellSound.Name = "lblSellSound";
            this.lblSellSound.Size = new System.Drawing.Size(61, 13);
            this.lblSellSound.TabIndex = 59;
            this.lblSellSound.Text = "Sell Sound:";
            // 
            // cmbSellSound
            // 
            this.cmbSellSound.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.cmbSellSound.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.cmbSellSound.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.cmbSellSound.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.cmbSellSound.DrawDropdownHoverOutline = false;
            this.cmbSellSound.DrawFocusRectangle = false;
            this.cmbSellSound.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSellSound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSellSound.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbSellSound.ForeColor = System.Drawing.Color.Gainsboro;
            this.cmbSellSound.FormattingEnabled = true;
            this.cmbSellSound.Items.AddRange(new object[] {
            "None"});
            this.cmbSellSound.Location = new System.Drawing.Point(338, 72);
            this.cmbSellSound.Name = "cmbSellSound";
            this.cmbSellSound.Size = new System.Drawing.Size(179, 21);
            this.cmbSellSound.TabIndex = 58;
            this.cmbSellSound.Text = "None";
            this.cmbSellSound.TextPadding = new System.Windows.Forms.Padding(2);
            this.cmbSellSound.SelectedIndexChanged += new System.EventHandler(this.cmbSellSound_SelectedIndexChanged);
            // 
            // lblBuySound
            // 
            this.lblBuySound.AutoSize = true;
            this.lblBuySound.Location = new System.Drawing.Point(270, 49);
            this.lblBuySound.Name = "lblBuySound";
            this.lblBuySound.Size = new System.Drawing.Size(62, 13);
            this.lblBuySound.TabIndex = 57;
            this.lblBuySound.Text = "Buy Sound:";
            // 
            // cmbBuySound
            // 
            this.cmbBuySound.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.cmbBuySound.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.cmbBuySound.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.cmbBuySound.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.cmbBuySound.DrawDropdownHoverOutline = false;
            this.cmbBuySound.DrawFocusRectangle = false;
            this.cmbBuySound.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbBuySound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBuySound.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbBuySound.ForeColor = System.Drawing.Color.Gainsboro;
            this.cmbBuySound.FormattingEnabled = true;
            this.cmbBuySound.Items.AddRange(new object[] {
            "None"});
            this.cmbBuySound.Location = new System.Drawing.Point(338, 45);
            this.cmbBuySound.Name = "cmbBuySound";
            this.cmbBuySound.Size = new System.Drawing.Size(179, 21);
            this.cmbBuySound.TabIndex = 56;
            this.cmbBuySound.Text = "None";
            this.cmbBuySound.TextPadding = new System.Windows.Forms.Padding(2);
            this.cmbBuySound.SelectedIndexChanged += new System.EventHandler(this.cmbBuySound_SelectedIndexChanged);
            // 
            // btnAddFolder
            // 
            this.btnAddFolder.Location = new System.Drawing.Point(197, 41);
            this.btnAddFolder.Name = "btnAddFolder";
            this.btnAddFolder.Padding = new System.Windows.Forms.Padding(5);
            this.btnAddFolder.Size = new System.Drawing.Size(18, 21);
            this.btnAddFolder.TabIndex = 55;
            this.btnAddFolder.Text = "+";
            this.btnAddFolder.Click += new System.EventHandler(this.btnAddFolder_Click);
            // 
            // lblFolder
            // 
            this.lblFolder.AutoSize = true;
            this.lblFolder.Location = new System.Drawing.Point(6, 44);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(39, 13);
            this.lblFolder.TabIndex = 54;
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
            this.cmbFolder.Location = new System.Drawing.Point(60, 41);
            this.cmbFolder.Name = "cmbFolder";
            this.cmbFolder.Size = new System.Drawing.Size(131, 21);
            this.cmbFolder.TabIndex = 53;
            this.cmbFolder.Text = null;
            this.cmbFolder.TextPadding = new System.Windows.Forms.Padding(2);
            this.cmbFolder.SelectedIndexChanged += new System.EventHandler(this.cmbFolder_SelectedIndexChanged);
            // 
            // cmbDefaultCurrency
            // 
            this.cmbDefaultCurrency.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.cmbDefaultCurrency.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.cmbDefaultCurrency.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.cmbDefaultCurrency.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.cmbDefaultCurrency.DrawDropdownHoverOutline = false;
            this.cmbDefaultCurrency.DrawFocusRectangle = false;
            this.cmbDefaultCurrency.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDefaultCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDefaultCurrency.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbDefaultCurrency.ForeColor = System.Drawing.Color.Gainsboro;
            this.cmbDefaultCurrency.FormattingEnabled = true;
            this.cmbDefaultCurrency.Location = new System.Drawing.Point(338, 17);
            this.cmbDefaultCurrency.Name = "cmbDefaultCurrency";
            this.cmbDefaultCurrency.Size = new System.Drawing.Size(179, 21);
            this.cmbDefaultCurrency.TabIndex = 8;
            this.cmbDefaultCurrency.Text = null;
            this.cmbDefaultCurrency.TextPadding = new System.Windows.Forms.Padding(2);
            this.cmbDefaultCurrency.SelectedIndexChanged += new System.EventHandler(this.cmbDefaultCurrency_SelectedIndexChanged);
            // 
            // lblDefaultCurrency
            // 
            this.lblDefaultCurrency.AutoSize = true;
            this.lblDefaultCurrency.Location = new System.Drawing.Point(247, 20);
            this.lblDefaultCurrency.Name = "lblDefaultCurrency";
            this.lblDefaultCurrency.Size = new System.Drawing.Size(92, 13);
            this.lblDefaultCurrency.TabIndex = 2;
            this.lblDefaultCurrency.Text = "Default Currency: ";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(6, 17);
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
            this.txtName.Location = new System.Drawing.Point(60, 16);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(155, 20);
            this.txtName.TabIndex = 0;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // grpItemsSold
            // 
            this.grpItemsSold.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.grpItemsSold.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.grpItemsSold.Controls.Add(this.lblBuyItemPriceNum);
            this.grpItemsSold.Controls.Add(this.lblSellItemCost);
            this.grpItemsSold.Controls.Add(this.btnItemDown);
            this.grpItemsSold.Controls.Add(this.btnItemUp);
            this.grpItemsSold.Controls.Add(this.nudSellCost);
            this.grpItemsSold.Controls.Add(this.cmbSellFor);
            this.grpItemsSold.Controls.Add(this.lblSellFor);
            this.grpItemsSold.Controls.Add(this.lblSellCost);
            this.grpItemsSold.Controls.Add(this.btnDelSoldItem);
            this.grpItemsSold.Controls.Add(this.btnAddSoldItem);
            this.grpItemsSold.Controls.Add(this.cmbAddSoldItem);
            this.grpItemsSold.Controls.Add(this.lblAddSoldItem);
            this.grpItemsSold.Controls.Add(this.lstSoldItems);
            this.grpItemsSold.ForeColor = System.Drawing.Color.Gainsboro;
            this.grpItemsSold.Location = new System.Drawing.Point(0, 112);
            this.grpItemsSold.Name = "grpItemsSold";
            this.grpItemsSold.Size = new System.Drawing.Size(258, 437);
            this.grpItemsSold.TabIndex = 17;
            this.grpItemsSold.TabStop = false;
            this.grpItemsSold.Text = "Items Sold";
            // 
            // lblBuyItemPriceNum
            // 
            this.lblBuyItemPriceNum.AutoSize = true;
            this.lblBuyItemPriceNum.Location = new System.Drawing.Point(102, 365);
            this.lblBuyItemPriceNum.Name = "lblBuyItemPriceNum";
            this.lblBuyItemPriceNum.Size = new System.Drawing.Size(13, 13);
            this.lblBuyItemPriceNum.TabIndex = 48;
            this.lblBuyItemPriceNum.Text = "0";
            // 
            // lblSellItemCost
            // 
            this.lblSellItemCost.AutoSize = true;
            this.lblSellItemCost.Location = new System.Drawing.Point(6, 365);
            this.lblSellItemCost.Name = "lblSellItemCost";
            this.lblSellItemCost.Size = new System.Drawing.Size(57, 13);
            this.lblSellItemCost.TabIndex = 47;
            this.lblSellItemCost.Text = "Item Price:";
            // 
            // btnItemDown
            // 
            this.btnItemDown.Location = new System.Drawing.Point(233, 190);
            this.btnItemDown.Name = "btnItemDown";
            this.btnItemDown.Padding = new System.Windows.Forms.Padding(5);
            this.btnItemDown.Size = new System.Drawing.Size(22, 40);
            this.btnItemDown.TabIndex = 46;
            this.btnItemDown.Text = "▼";
            this.btnItemDown.Click += new System.EventHandler(this.btnItemDown_Click);
            // 
            // btnItemUp
            // 
            this.btnItemUp.Location = new System.Drawing.Point(233, 20);
            this.btnItemUp.Name = "btnItemUp";
            this.btnItemUp.Padding = new System.Windows.Forms.Padding(5);
            this.btnItemUp.Size = new System.Drawing.Size(22, 40);
            this.btnItemUp.TabIndex = 45;
            this.btnItemUp.Text = "▲";
            this.btnItemUp.Click += new System.EventHandler(this.btnItemUp_Click);
            // 
            // nudSellCost
            // 
            this.nudSellCost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.nudSellCost.ForeColor = System.Drawing.Color.Gainsboro;
            this.nudSellCost.Location = new System.Drawing.Point(81, 325);
            this.nudSellCost.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudSellCost.Name = "nudSellCost";
            this.nudSellCost.Size = new System.Drawing.Size(171, 20);
            this.nudSellCost.TabIndex = 44;
            this.nudSellCost.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // cmbSellFor
            // 
            this.cmbSellFor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.cmbSellFor.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.cmbSellFor.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.cmbSellFor.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.cmbSellFor.DrawDropdownHoverOutline = false;
            this.cmbSellFor.DrawFocusRectangle = false;
            this.cmbSellFor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSellFor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSellFor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbSellFor.ForeColor = System.Drawing.Color.Gainsboro;
            this.cmbSellFor.FormattingEnabled = true;
            this.cmbSellFor.Location = new System.Drawing.Point(6, 296);
            this.cmbSellFor.Name = "cmbSellFor";
            this.cmbSellFor.Size = new System.Drawing.Size(246, 21);
            this.cmbSellFor.TabIndex = 11;
            this.cmbSellFor.Text = null;
            this.cmbSellFor.TextPadding = new System.Windows.Forms.Padding(2);
            // 
            // lblSellFor
            // 
            this.lblSellFor.AutoSize = true;
            this.lblSellFor.Location = new System.Drawing.Point(7, 280);
            this.lblSellFor.Name = "lblSellFor";
            this.lblSellFor.Size = new System.Drawing.Size(45, 13);
            this.lblSellFor.TabIndex = 10;
            this.lblSellFor.Text = "Sell For:";
            // 
            // lblSellCost
            // 
            this.lblSellCost.AutoSize = true;
            this.lblSellCost.Location = new System.Drawing.Point(7, 327);
            this.lblSellCost.Name = "lblSellCost";
            this.lblSellCost.Size = new System.Drawing.Size(51, 13);
            this.lblSellCost.TabIndex = 5;
            this.lblSellCost.Text = "Sell Cost:";
            // 
            // btnDelSoldItem
            // 
            this.btnDelSoldItem.Location = new System.Drawing.Point(135, 396);
            this.btnDelSoldItem.Name = "btnDelSoldItem";
            this.btnDelSoldItem.Padding = new System.Windows.Forms.Padding(5);
            this.btnDelSoldItem.Size = new System.Drawing.Size(117, 23);
            this.btnDelSoldItem.TabIndex = 4;
            this.btnDelSoldItem.Text = "Remove Selected";
            this.btnDelSoldItem.Click += new System.EventHandler(this.btnDelSoldItem_Click);
            // 
            // btnAddSoldItem
            // 
            this.btnAddSoldItem.Location = new System.Drawing.Point(6, 396);
            this.btnAddSoldItem.Name = "btnAddSoldItem";
            this.btnAddSoldItem.Padding = new System.Windows.Forms.Padding(5);
            this.btnAddSoldItem.Size = new System.Drawing.Size(117, 23);
            this.btnAddSoldItem.TabIndex = 3;
            this.btnAddSoldItem.Text = "Add Selected";
            this.btnAddSoldItem.Click += new System.EventHandler(this.btnAddSoldItem_Click);
            // 
            // cmbAddSoldItem
            // 
            this.cmbAddSoldItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.cmbAddSoldItem.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.cmbAddSoldItem.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.cmbAddSoldItem.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.cmbAddSoldItem.DrawDropdownHoverOutline = false;
            this.cmbAddSoldItem.DrawFocusRectangle = false;
            this.cmbAddSoldItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbAddSoldItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAddSoldItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbAddSoldItem.ForeColor = System.Drawing.Color.Gainsboro;
            this.cmbAddSoldItem.FormattingEnabled = true;
            this.cmbAddSoldItem.Location = new System.Drawing.Point(6, 254);
            this.cmbAddSoldItem.Name = "cmbAddSoldItem";
            this.cmbAddSoldItem.Size = new System.Drawing.Size(246, 21);
            this.cmbAddSoldItem.TabIndex = 2;
            this.cmbAddSoldItem.Text = null;
            this.cmbAddSoldItem.TextPadding = new System.Windows.Forms.Padding(2);
            this.cmbAddSoldItem.SelectedIndexChanged += new System.EventHandler(this.cmbAddSoldItem_SelectedIndexChanged);
            // 
            // lblAddSoldItem
            // 
            this.lblAddSoldItem.AutoSize = true;
            this.lblAddSoldItem.Location = new System.Drawing.Point(7, 238);
            this.lblAddSoldItem.Name = "lblAddSoldItem";
            this.lblAddSoldItem.Size = new System.Drawing.Size(108, 13);
            this.lblAddSoldItem.TabIndex = 1;
            this.lblAddSoldItem.Text = "Add Item To Be Sold:";
            // 
            // lstSoldItems
            // 
            this.lstSoldItems.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.lstSoldItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstSoldItems.ForeColor = System.Drawing.Color.Gainsboro;
            this.lstSoldItems.FormattingEnabled = true;
            this.lstSoldItems.Location = new System.Drawing.Point(7, 20);
            this.lstSoldItems.Name = "lstSoldItems";
            this.lstSoldItems.Size = new System.Drawing.Size(223, 210);
            this.lstSoldItems.TabIndex = 0;
            // 
            // grpItemsBought
            // 
            this.grpItemsBought.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.grpItemsBought.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.grpItemsBought.Controls.Add(this.btnNuke);
            this.grpItemsBought.Controls.Add(this.lblSellItemPriceNum);
            this.grpItemsBought.Controls.Add(this.lblBuyItemCost);
            this.grpItemsBought.Controls.Add(this.nudBuyAmount);
            this.grpItemsBought.Controls.Add(this.cmbBuyFor);
            this.grpItemsBought.Controls.Add(this.lblBuyFor);
            this.grpItemsBought.Controls.Add(this.lblBuyAmount);
            this.grpItemsBought.Controls.Add(this.btnDelBoughtItem);
            this.grpItemsBought.Controls.Add(this.btnAddBoughtItem);
            this.grpItemsBought.Controls.Add(this.cmbAddBoughtItem);
            this.grpItemsBought.Controls.Add(this.lblItemBought);
            this.grpItemsBought.Controls.Add(this.lstBoughtItems);
            this.grpItemsBought.Controls.Add(this.rdoBuyBlacklist);
            this.grpItemsBought.Controls.Add(this.rdoBuyWhitelist);
            this.grpItemsBought.ForeColor = System.Drawing.Color.Gainsboro;
            this.grpItemsBought.Location = new System.Drawing.Point(264, 112);
            this.grpItemsBought.Name = "grpItemsBought";
            this.grpItemsBought.Size = new System.Drawing.Size(258, 437);
            this.grpItemsBought.TabIndex = 18;
            this.grpItemsBought.TabStop = false;
            this.grpItemsBought.Text = "Items Bought (Whitelist - Buy Listed Items)";
            // 
            // lblSellItemPriceNum
            // 
            this.lblSellItemPriceNum.AutoSize = true;
            this.lblSellItemPriceNum.Location = new System.Drawing.Point(126, 365);
            this.lblSellItemPriceNum.Name = "lblSellItemPriceNum";
            this.lblSellItemPriceNum.Size = new System.Drawing.Size(13, 13);
            this.lblSellItemPriceNum.TabIndex = 49;
            this.lblSellItemPriceNum.Text = "0";
            // 
            // lblBuyItemCost
            // 
            this.lblBuyItemCost.AutoSize = true;
            this.lblBuyItemCost.Location = new System.Drawing.Point(7, 365);
            this.lblBuyItemCost.Name = "lblBuyItemCost";
            this.lblBuyItemCost.Size = new System.Drawing.Size(57, 13);
            this.lblBuyItemCost.TabIndex = 48;
            this.lblBuyItemCost.Text = "Item Price:";
            // 
            // nudBuyAmount
            // 
            this.nudBuyAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.nudBuyAmount.ForeColor = System.Drawing.Color.Gainsboro;
            this.nudBuyAmount.Location = new System.Drawing.Point(82, 325);
            this.nudBuyAmount.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudBuyAmount.Name = "nudBuyAmount";
            this.nudBuyAmount.Size = new System.Drawing.Size(171, 20);
            this.nudBuyAmount.TabIndex = 45;
            this.nudBuyAmount.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // cmbBuyFor
            // 
            this.cmbBuyFor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.cmbBuyFor.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.cmbBuyFor.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.cmbBuyFor.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.cmbBuyFor.DrawDropdownHoverOutline = false;
            this.cmbBuyFor.DrawFocusRectangle = false;
            this.cmbBuyFor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbBuyFor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBuyFor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbBuyFor.ForeColor = System.Drawing.Color.Gainsboro;
            this.cmbBuyFor.FormattingEnabled = true;
            this.cmbBuyFor.Location = new System.Drawing.Point(8, 296);
            this.cmbBuyFor.Name = "cmbBuyFor";
            this.cmbBuyFor.Size = new System.Drawing.Size(246, 21);
            this.cmbBuyFor.TabIndex = 12;
            this.cmbBuyFor.Text = null;
            this.cmbBuyFor.TextPadding = new System.Windows.Forms.Padding(2);
            // 
            // lblBuyFor
            // 
            this.lblBuyFor.AutoSize = true;
            this.lblBuyFor.Location = new System.Drawing.Point(6, 280);
            this.lblBuyFor.Name = "lblBuyFor";
            this.lblBuyFor.Size = new System.Drawing.Size(46, 13);
            this.lblBuyFor.TabIndex = 11;
            this.lblBuyFor.Text = "Buy For:";
            // 
            // lblBuyAmount
            // 
            this.lblBuyAmount.AutoSize = true;
            this.lblBuyAmount.Location = new System.Drawing.Point(6, 327);
            this.lblBuyAmount.Name = "lblBuyAmount";
            this.lblBuyAmount.Size = new System.Drawing.Size(66, 13);
            this.lblBuyAmount.TabIndex = 6;
            this.lblBuyAmount.Text = "Sell Amount:";
            // 
            // btnDelBoughtItem
            // 
            this.btnDelBoughtItem.Location = new System.Drawing.Point(129, 396);
            this.btnDelBoughtItem.Name = "btnDelBoughtItem";
            this.btnDelBoughtItem.Padding = new System.Windows.Forms.Padding(5);
            this.btnDelBoughtItem.Size = new System.Drawing.Size(117, 23);
            this.btnDelBoughtItem.TabIndex = 6;
            this.btnDelBoughtItem.Text = "Remove Selected";
            this.btnDelBoughtItem.Click += new System.EventHandler(this.btnDelBoughtItem_Click);
            // 
            // btnAddBoughtItem
            // 
            this.btnAddBoughtItem.Location = new System.Drawing.Point(6, 396);
            this.btnAddBoughtItem.Name = "btnAddBoughtItem";
            this.btnAddBoughtItem.Padding = new System.Windows.Forms.Padding(5);
            this.btnAddBoughtItem.Size = new System.Drawing.Size(117, 23);
            this.btnAddBoughtItem.TabIndex = 5;
            this.btnAddBoughtItem.Text = "Add Selected";
            this.btnAddBoughtItem.Click += new System.EventHandler(this.btnAddBoughtItem_Click);
            // 
            // cmbAddBoughtItem
            // 
            this.cmbAddBoughtItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.cmbAddBoughtItem.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.cmbAddBoughtItem.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.cmbAddBoughtItem.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.cmbAddBoughtItem.DrawDropdownHoverOutline = false;
            this.cmbAddBoughtItem.DrawFocusRectangle = false;
            this.cmbAddBoughtItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbAddBoughtItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAddBoughtItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbAddBoughtItem.ForeColor = System.Drawing.Color.Gainsboro;
            this.cmbAddBoughtItem.FormattingEnabled = true;
            this.cmbAddBoughtItem.Location = new System.Drawing.Point(8, 254);
            this.cmbAddBoughtItem.Name = "cmbAddBoughtItem";
            this.cmbAddBoughtItem.Size = new System.Drawing.Size(246, 21);
            this.cmbAddBoughtItem.TabIndex = 3;
            this.cmbAddBoughtItem.Text = null;
            this.cmbAddBoughtItem.TextPadding = new System.Windows.Forms.Padding(2);
            this.cmbAddBoughtItem.SelectedIndexChanged += new System.EventHandler(this.cmbAddBoughtItem_SelectedIndexChanged);
            // 
            // lblItemBought
            // 
            this.lblItemBought.AutoSize = true;
            this.lblItemBought.Location = new System.Drawing.Point(6, 240);
            this.lblItemBought.Name = "lblItemBought";
            this.lblItemBought.Size = new System.Drawing.Size(52, 13);
            this.lblItemBought.TabIndex = 2;
            this.lblItemBought.Text = "Add Item:";
            // 
            // lstBoughtItems
            // 
            this.lstBoughtItems.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.lstBoughtItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstBoughtItems.ForeColor = System.Drawing.Color.Gainsboro;
            this.lstBoughtItems.FormattingEnabled = true;
            this.lstBoughtItems.Location = new System.Drawing.Point(8, 20);
            this.lstBoughtItems.Name = "lstBoughtItems";
            this.lstBoughtItems.Size = new System.Drawing.Size(245, 210);
            this.lstBoughtItems.TabIndex = 2;
            // 
            // rdoBuyBlacklist
            // 
            this.rdoBuyBlacklist.AutoSize = true;
            this.rdoBuyBlacklist.Location = new System.Drawing.Point(189, 236);
            this.rdoBuyBlacklist.Name = "rdoBuyBlacklist";
            this.rdoBuyBlacklist.Size = new System.Drawing.Size(64, 17);
            this.rdoBuyBlacklist.TabIndex = 1;
            this.rdoBuyBlacklist.Text = "Blacklist";
            this.rdoBuyBlacklist.CheckedChanged += new System.EventHandler(this.rdoBuyBlacklist_CheckedChanged);
            // 
            // rdoBuyWhitelist
            // 
            this.rdoBuyWhitelist.AutoSize = true;
            this.rdoBuyWhitelist.Checked = true;
            this.rdoBuyWhitelist.Location = new System.Drawing.Point(118, 236);
            this.rdoBuyWhitelist.Name = "rdoBuyWhitelist";
            this.rdoBuyWhitelist.Size = new System.Drawing.Size(65, 17);
            this.rdoBuyWhitelist.TabIndex = 0;
            this.rdoBuyWhitelist.TabStop = true;
            this.rdoBuyWhitelist.Text = "Whitelist";
            this.rdoBuyWhitelist.CheckedChanged += new System.EventHandler(this.rdoBuyWhitelist_CheckedChanged);
            // 
            // pnlContainer
            // 
            this.pnlContainer.Controls.Add(this.grpTagsBought);
            this.pnlContainer.Controls.Add(this.grpItemsBought);
            this.pnlContainer.Controls.Add(this.grpGeneral);
            this.pnlContainer.Controls.Add(this.grpItemsSold);
            this.pnlContainer.Location = new System.Drawing.Point(222, 34);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(845, 565);
            this.pnlContainer.TabIndex = 19;
            this.pnlContainer.Visible = false;
            // 
            // grpTagsBought
            // 
            this.grpTagsBought.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.grpTagsBought.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.grpTagsBought.Controls.Add(this.btnRemoveTag);
            this.grpTagsBought.Controls.Add(this.btnAddTag);
            this.grpTagsBought.Controls.Add(this.cmbTags);
            this.grpTagsBought.Controls.Add(this.lblTag);
            this.grpTagsBought.Controls.Add(this.lstBoughtTags);
            this.grpTagsBought.Controls.Add(this.tdoTagBlacklist);
            this.grpTagsBought.Controls.Add(this.rdoTagWhitelist);
            this.grpTagsBought.ForeColor = System.Drawing.Color.Gainsboro;
            this.grpTagsBought.Location = new System.Drawing.Point(528, 112);
            this.grpTagsBought.Name = "grpTagsBought";
            this.grpTagsBought.Size = new System.Drawing.Size(258, 437);
            this.grpTagsBought.TabIndex = 50;
            this.grpTagsBought.TabStop = false;
            this.grpTagsBought.Text = "Tags Bought";
            // 
            // btnRemoveTag
            // 
            this.btnRemoveTag.Location = new System.Drawing.Point(129, 281);
            this.btnRemoveTag.Name = "btnRemoveTag";
            this.btnRemoveTag.Padding = new System.Windows.Forms.Padding(5);
            this.btnRemoveTag.Size = new System.Drawing.Size(117, 23);
            this.btnRemoveTag.TabIndex = 6;
            this.btnRemoveTag.Text = "Remove Selected";
            this.btnRemoveTag.Click += new System.EventHandler(this.btnRemoveTag_Click);
            // 
            // btnAddTag
            // 
            this.btnAddTag.Location = new System.Drawing.Point(6, 281);
            this.btnAddTag.Name = "btnAddTag";
            this.btnAddTag.Padding = new System.Windows.Forms.Padding(5);
            this.btnAddTag.Size = new System.Drawing.Size(117, 23);
            this.btnAddTag.TabIndex = 5;
            this.btnAddTag.Text = "Add Selected";
            this.btnAddTag.Click += new System.EventHandler(this.btnAddTag_Click);
            // 
            // cmbTags
            // 
            this.cmbTags.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.cmbTags.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.cmbTags.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.cmbTags.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.cmbTags.DrawDropdownHoverOutline = false;
            this.cmbTags.DrawFocusRectangle = false;
            this.cmbTags.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbTags.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTags.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbTags.ForeColor = System.Drawing.Color.Gainsboro;
            this.cmbTags.FormattingEnabled = true;
            this.cmbTags.Location = new System.Drawing.Point(8, 254);
            this.cmbTags.Name = "cmbTags";
            this.cmbTags.Size = new System.Drawing.Size(246, 21);
            this.cmbTags.TabIndex = 3;
            this.cmbTags.Text = null;
            this.cmbTags.TextPadding = new System.Windows.Forms.Padding(2);
            // 
            // lblTag
            // 
            this.lblTag.AutoSize = true;
            this.lblTag.Location = new System.Drawing.Point(6, 240);
            this.lblTag.Name = "lblTag";
            this.lblTag.Size = new System.Drawing.Size(74, 13);
            this.lblTag.TabIndex = 2;
            this.lblTag.Text = "Add Item Tag:";
            // 
            // lstBoughtTags
            // 
            this.lstBoughtTags.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.lstBoughtTags.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstBoughtTags.ForeColor = System.Drawing.Color.Gainsboro;
            this.lstBoughtTags.FormattingEnabled = true;
            this.lstBoughtTags.Location = new System.Drawing.Point(8, 20);
            this.lstBoughtTags.Name = "lstBoughtTags";
            this.lstBoughtTags.Size = new System.Drawing.Size(245, 210);
            this.lstBoughtTags.TabIndex = 2;
            // 
            // tdoTagBlacklist
            // 
            this.tdoTagBlacklist.AutoSize = true;
            this.tdoTagBlacklist.Location = new System.Drawing.Point(189, 236);
            this.tdoTagBlacklist.Name = "tdoTagBlacklist";
            this.tdoTagBlacklist.Size = new System.Drawing.Size(64, 17);
            this.tdoTagBlacklist.TabIndex = 1;
            this.tdoTagBlacklist.Text = "Blacklist";
            this.tdoTagBlacklist.CheckedChanged += new System.EventHandler(this.tdoTagBlacklist_CheckedChanged);
            // 
            // rdoTagWhitelist
            // 
            this.rdoTagWhitelist.AutoSize = true;
            this.rdoTagWhitelist.Checked = true;
            this.rdoTagWhitelist.Location = new System.Drawing.Point(118, 236);
            this.rdoTagWhitelist.Name = "rdoTagWhitelist";
            this.rdoTagWhitelist.Size = new System.Drawing.Size(65, 17);
            this.rdoTagWhitelist.TabIndex = 0;
            this.rdoTagWhitelist.TabStop = true;
            this.rdoTagWhitelist.Text = "Whitelist";
            this.rdoTagWhitelist.CheckedChanged += new System.EventHandler(this.rdoTagWhitelist_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(548, 618);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(5);
            this.btnCancel.Size = new System.Drawing.Size(190, 27);
            this.btnCancel.TabIndex = 49;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(357, 618);
            this.btnSave.Name = "btnSave";
            this.btnSave.Padding = new System.Windows.Forms.Padding(5);
            this.btnSave.Size = new System.Drawing.Size(190, 27);
            this.btnSave.TabIndex = 46;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
            this.toolStrip.Size = new System.Drawing.Size(1079, 25);
            this.toolStrip.TabIndex = 50;
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
            // btnNuke
            // 
            this.btnNuke.Location = new System.Drawing.Point(171, 360);
            this.btnNuke.Name = "btnNuke";
            this.btnNuke.Padding = new System.Windows.Forms.Padding(5);
            this.btnNuke.Size = new System.Drawing.Size(75, 23);
            this.btnNuke.TabIndex = 50;
            this.btnNuke.Text = "Nuke";
            this.btnNuke.Click += new System.EventHandler(this.btnNuke_Click);
            // 
            // FrmShop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.ClientSize = new System.Drawing.Size(1079, 657);
            this.ControlBox = false;
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpShops);
            this.Controls.Add(this.pnlContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "FrmShop";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Shop Editor";
            this.Load += new System.EventHandler(this.frmShop_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.form_KeyDown);
            this.grpShops.ResumeLayout(false);
            this.grpShops.PerformLayout();
            this.grpGeneral.ResumeLayout(false);
            this.grpGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBuyMultiplier)).EndInit();
            this.grpItemsSold.ResumeLayout(false);
            this.grpItemsSold.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSellCost)).EndInit();
            this.grpItemsBought.ResumeLayout(false);
            this.grpItemsBought.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBuyAmount)).EndInit();
            this.pnlContainer.ResumeLayout(false);
            this.grpTagsBought.ResumeLayout(false);
            this.grpTagsBought.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DarkGroupBox grpShops;
        private DarkGroupBox grpGeneral;
        private System.Windows.Forms.Label lblName;
        private DarkTextBox txtName;
        private DarkGroupBox grpItemsSold;
        private System.Windows.Forms.ListBox lstSoldItems;
        private DarkGroupBox grpItemsBought;
        private System.Windows.Forms.ListBox lstBoughtItems;
        private DarkRadioButton rdoBuyBlacklist;
        private DarkRadioButton rdoBuyWhitelist;
        private DarkButton btnDelSoldItem;
        private DarkButton btnAddSoldItem;
        private DarkComboBox cmbAddSoldItem;
        private System.Windows.Forms.Label lblAddSoldItem;
        private DarkButton btnDelBoughtItem;
        private DarkButton btnAddBoughtItem;
        private DarkComboBox cmbAddBoughtItem;
        private System.Windows.Forms.Label lblItemBought;
        private System.Windows.Forms.Label lblBuyAmount;
        private System.Windows.Forms.Label lblSellCost;
        private DarkComboBox cmbDefaultCurrency;
        private System.Windows.Forms.Label lblDefaultCurrency;
        private DarkComboBox cmbSellFor;
        private System.Windows.Forms.Label lblSellFor;
        private DarkComboBox cmbBuyFor;
        private System.Windows.Forms.Label lblBuyFor;
        private System.Windows.Forms.Panel pnlContainer;
        private DarkButton btnSave;
        private DarkButton btnCancel;
        private DarkToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripItemNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripItemDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.ToolStripButton toolStripItemCopy;
        public System.Windows.Forms.ToolStripButton toolStripItemPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        public System.Windows.Forms.ToolStripButton toolStripItemUndo;
        private DarkNumericUpDown nudSellCost;
        private DarkNumericUpDown nudBuyAmount;
        private DarkButton btnClearSearch;
        private DarkTextBox txtSearch;
        private DarkButton btnAddFolder;
        private System.Windows.Forms.Label lblFolder;
        private DarkComboBox cmbFolder;
        private System.Windows.Forms.ToolStripButton btnAlphabetical;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private Controls.GameObjectList lstGameObjects;
        private DarkButton btnItemDown;
        private DarkButton btnItemUp;
        private System.Windows.Forms.Label lblSellSound;
        private DarkComboBox cmbSellSound;
        private System.Windows.Forms.Label lblBuySound;
        private DarkComboBox cmbBuySound;
        private System.Windows.Forms.Label lblBuyItemPriceNum;
        private System.Windows.Forms.Label lblSellItemCost;
        private System.Windows.Forms.Label lblSellItemPriceNum;
        private System.Windows.Forms.Label lblBuyItemCost;
        private System.Windows.Forms.Label lblBuyMultiplier;
        private DarkGroupBox grpTagsBought;
        private DarkButton btnRemoveTag;
        private DarkButton btnAddTag;
        private DarkComboBox cmbTags;
        private System.Windows.Forms.Label lblTag;
        private System.Windows.Forms.ListBox lstBoughtTags;
        private DarkRadioButton tdoTagBlacklist;
        private DarkRadioButton rdoTagWhitelist;
        private DarkNumericUpDown nudBuyMultiplier;
        private DarkButton btnNuke;
    }
}