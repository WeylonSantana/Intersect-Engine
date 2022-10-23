
namespace Intersect.Editor.Forms.Editors.Events.Event_Commands
{
    partial class EventCommandSetProfessionLevel
    {
        /// <summary> 
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpSetProfessionLevel = new DarkUI.Controls.DarkGroupBox();
            this.cmbProfession = new DarkUI.Controls.DarkComboBox();
            this.lblProfession = new System.Windows.Forms.Label();
            this.nudLevel = new DarkUI.Controls.DarkNumericUpDown();
            this.lblLevel = new System.Windows.Forms.Label();
            this.btnCancel = new DarkUI.Controls.DarkButton();
            this.btnSave = new DarkUI.Controls.DarkButton();
            this.grpSetProfessionLevel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // grpSetProfessionLevel
            // 
            this.grpSetProfessionLevel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.grpSetProfessionLevel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.grpSetProfessionLevel.Controls.Add(this.cmbProfession);
            this.grpSetProfessionLevel.Controls.Add(this.lblProfession);
            this.grpSetProfessionLevel.Controls.Add(this.nudLevel);
            this.grpSetProfessionLevel.Controls.Add(this.lblLevel);
            this.grpSetProfessionLevel.Controls.Add(this.btnCancel);
            this.grpSetProfessionLevel.Controls.Add(this.btnSave);
            this.grpSetProfessionLevel.ForeColor = System.Drawing.Color.Gainsboro;
            this.grpSetProfessionLevel.Location = new System.Drawing.Point(5, 5);
            this.grpSetProfessionLevel.Name = "grpSetProfessionLevel";
            this.grpSetProfessionLevel.Size = new System.Drawing.Size(259, 108);
            this.grpSetProfessionLevel.TabIndex = 18;
            this.grpSetProfessionLevel.TabStop = false;
            this.grpSetProfessionLevel.Text = "Set Profession Level:";
            // 
            // cmbProfession
            // 
            this.cmbProfession.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.cmbProfession.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.cmbProfession.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.cmbProfession.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.cmbProfession.DrawDropdownHoverOutline = false;
            this.cmbProfession.DrawFocusRectangle = false;
            this.cmbProfession.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbProfession.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProfession.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbProfession.ForeColor = System.Drawing.Color.Gainsboro;
            this.cmbProfession.FormattingEnabled = true;
            this.cmbProfession.Location = new System.Drawing.Point(89, 22);
            this.cmbProfession.Name = "cmbProfession";
            this.cmbProfession.Size = new System.Drawing.Size(164, 21);
            this.cmbProfession.TabIndex = 24;
            this.cmbProfession.Text = null;
            this.cmbProfession.TextPadding = new System.Windows.Forms.Padding(2);
            this.cmbProfession.SelectedIndexChanged += new System.EventHandler(this.cmbProfession_SelectedIndexChanged);
            // 
            // lblProfession
            // 
            this.lblProfession.AutoSize = true;
            this.lblProfession.Location = new System.Drawing.Point(4, 25);
            this.lblProfession.Name = "lblProfession";
            this.lblProfession.Size = new System.Drawing.Size(59, 13);
            this.lblProfession.TabIndex = 23;
            this.lblProfession.Text = "Profession:";
            // 
            // nudLevel
            // 
            this.nudLevel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.nudLevel.ForeColor = System.Drawing.Color.Gainsboro;
            this.nudLevel.Location = new System.Drawing.Point(89, 49);
            this.nudLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLevel.Name = "nudLevel";
            this.nudLevel.Size = new System.Drawing.Size(164, 20);
            this.nudLevel.TabIndex = 22;
            this.nudLevel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Location = new System.Drawing.Point(6, 51);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(55, 13);
            this.lblLevel.TabIndex = 21;
            this.lblLevel.Text = "Set Level:";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(89, 75);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(5);
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(7, 75);
            this.btnSave.Name = "btnSave";
            this.btnSave.Padding = new System.Windows.Forms.Padding(5);
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "Ok";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // EventCommandSetProfessionLevel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.Controls.Add(this.grpSetProfessionLevel);
            this.Name = "EventCommandSetProfessionLevel";
            this.Size = new System.Drawing.Size(268, 120);
            this.grpSetProfessionLevel.ResumeLayout(false);
            this.grpSetProfessionLevel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLevel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkGroupBox grpSetProfessionLevel;
        private DarkUI.Controls.DarkComboBox cmbProfession;
        private System.Windows.Forms.Label lblProfession;
        private DarkUI.Controls.DarkNumericUpDown nudLevel;
        private System.Windows.Forms.Label lblLevel;
        private DarkUI.Controls.DarkButton btnCancel;
        private DarkUI.Controls.DarkButton btnSave;
    }
}