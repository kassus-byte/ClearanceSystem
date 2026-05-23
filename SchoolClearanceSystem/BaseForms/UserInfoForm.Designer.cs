namespace SchoolClearanceSystem
{
    partial class UserInfoForm
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
            this.cbYear = new System.Windows.Forms.ComboBox();
            this.txtFullName = new DevExpress.XtraEditors.TextEdit();
            this.cbProgram = new System.Windows.Forms.ComboBox();
            this.txtUserID = new DevExpress.XtraEditors.TextEdit();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl15 = new DevExpress.XtraEditors.PanelControl();
            this.lblTitle = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtDateCreated = new DevExpress.XtraEditors.TextEdit();
            this.lbl = new DevExpress.XtraEditors.LabelControl();
            this.cbRole = new System.Windows.Forms.ComboBox();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.chkShowPassword = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFullName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl15)).BeginInit();
            this.panelControl15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDateCreated.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkShowPassword.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // cbYear
            // 
            this.cbYear.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.cbYear.FormattingEnabled = true;
            this.cbYear.Items.AddRange(new object[] {
            "1st Year",
            "2nd Year",
            "3rd Year",
            "4th Year",
            "Irregular"});
            this.cbYear.Location = new System.Drawing.Point(26, 343);
            this.cbYear.Name = "cbYear";
            this.cbYear.Size = new System.Drawing.Size(464, 23);
            this.cbYear.TabIndex = 26;
            // 
            // txtFullName
            // 
            this.txtFullName.Location = new System.Drawing.Point(567, 179);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.txtFullName.Properties.Appearance.Options.UseFont = true;
            this.txtFullName.Size = new System.Drawing.Size(448, 30);
            this.txtFullName.TabIndex = 25;
            // 
            // cbProgram
            // 
            this.cbProgram.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.cbProgram.FormattingEnabled = true;
            this.cbProgram.Items.AddRange(new object[] {
            "BSIT"});
            this.cbProgram.Location = new System.Drawing.Point(567, 276);
            this.cbProgram.Name = "cbProgram";
            this.cbProgram.Size = new System.Drawing.Size(448, 23);
            this.cbProgram.TabIndex = 24;
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(26, 179);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.txtUserID.Properties.Appearance.Options.UseFont = true;
            this.txtUserID.Size = new System.Drawing.Size(464, 30);
            this.txtUserID.TabIndex = 23;
            // 
            // btnSave
            // 
            this.btnSave.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.btnSave.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnSave.Appearance.Options.UseBackColor = true;
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Location = new System.Drawing.Point(917, 94);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(141, 43);
            this.btnSave.TabIndex = 22;
            this.btnSave.Text = "Save ";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.Appearance.Options.UseForeColor = true;
            this.btnCancel.Location = new System.Drawing.Point(758, 94);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(141, 43);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_1);
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl7.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl7.Appearance.Options.UseFont = true;
            this.labelControl7.Appearance.Options.UseForeColor = true;
            this.labelControl7.Location = new System.Drawing.Point(26, 320);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(27, 17);
            this.labelControl7.TabIndex = 19;
            this.labelControl7.Text = "Year";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl5.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Appearance.Options.UseForeColor = true;
            this.labelControl5.Location = new System.Drawing.Point(567, 253);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(53, 17);
            this.labelControl5.TabIndex = 17;
            this.labelControl5.Text = "Program";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Appearance.Options.UseForeColor = true;
            this.labelControl4.Location = new System.Drawing.Point(567, 156);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(36, 17);
            this.labelControl4.TabIndex = 16;
            this.labelControl4.Text = "Name";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Appearance.Options.UseForeColor = true;
            this.labelControl3.Location = new System.Drawing.Point(26, 156);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(44, 17);
            this.labelControl3.TabIndex = 15;
            this.labelControl3.Text = "User ID";
            // 
            // panelControl15
            // 
            this.panelControl15.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panelControl15.Appearance.Options.UseBackColor = true;
            this.panelControl15.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl15.Controls.Add(this.lblTitle);
            this.panelControl15.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl15.Location = new System.Drawing.Point(0, 0);
            this.panelControl15.Name = "panelControl15";
            this.panelControl15.Size = new System.Drawing.Size(1114, 78);
            this.panelControl15.TabIndex = 30;
            // 
            // lblTitle
            // 
            this.lblTitle.Appearance.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.lblTitle.Appearance.Options.UseFont = true;
            this.lblTitle.Appearance.Options.UseForeColor = true;
            this.lblTitle.Location = new System.Drawing.Point(36, 19);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(212, 37);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Edit Information";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(26, 398);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(79, 17);
            this.labelControl1.TabIndex = 32;
            this.labelControl1.Text = "Date Created";
            // 
            // txtDateCreated
            // 
            this.txtDateCreated.Location = new System.Drawing.Point(26, 421);
            this.txtDateCreated.Name = "txtDateCreated";
            this.txtDateCreated.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.txtDateCreated.Properties.Appearance.Options.UseFont = true;
            this.txtDateCreated.Size = new System.Drawing.Size(464, 30);
            this.txtDateCreated.TabIndex = 33;
            // 
            // lbl
            // 
            this.lbl.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.lbl.Appearance.Options.UseFont = true;
            this.lbl.Appearance.Options.UseForeColor = true;
            this.lbl.Location = new System.Drawing.Point(26, 249);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(26, 17);
            this.lbl.TabIndex = 34;
            this.lbl.Text = "Role";
            // 
            // cbRole
            // 
            this.cbRole.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.cbRole.FormattingEnabled = true;
            this.cbRole.Items.AddRange(new object[] {
            "Student",
            "Treasurer",
            "Technical Office",
            "SSG",
            "Admin"});
            this.cbRole.Location = new System.Drawing.Point(26, 276);
            this.cbRole.Name = "cbRole";
            this.cbRole.Size = new System.Drawing.Size(464, 23);
            this.cbRole.TabIndex = 35;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(567, 339);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.txtPassword.Properties.Appearance.Options.UseFont = true;
            this.txtPassword.Size = new System.Drawing.Size(448, 30);
            this.txtPassword.TabIndex = 37;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.Location = new System.Drawing.Point(567, 316);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(58, 17);
            this.labelControl2.TabIndex = 36;
            this.labelControl2.Text = "Password";
            // 
            // chkShowPassword
            // 
            this.chkShowPassword.Location = new System.Drawing.Point(567, 374);
            this.chkShowPassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Properties.Caption = "Show Password";
            this.chkShowPassword.Size = new System.Drawing.Size(134, 22);
            this.chkShowPassword.TabIndex = 44;
            // 
            // UserInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 603);
            this.Controls.Add(this.chkShowPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.cbRole);
            this.Controls.Add(this.lbl);
            this.Controls.Add(this.txtDateCreated);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.panelControl15);
            this.Controls.Add(this.cbYear);
            this.Controls.Add(this.txtFullName);
            this.Controls.Add(this.cbProgram);
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UserInfoForm";
            this.Text = "UserInfoForm";
            this.Load += new System.EventHandler(this.UserInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtFullName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl15)).EndInit();
            this.panelControl15.ResumeLayout(false);
            this.panelControl15.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDateCreated.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkShowPassword.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cbYear;
        private DevExpress.XtraEditors.TextEdit txtFullName;
        private System.Windows.Forms.ComboBox cbProgram;
        private DevExpress.XtraEditors.TextEdit txtUserID;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.PanelControl panelControl15;
        private DevExpress.XtraEditors.LabelControl lblTitle;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtDateCreated;
        private DevExpress.XtraEditors.LabelControl lbl;
        private System.Windows.Forms.ComboBox cbRole;
        private DevExpress.XtraEditors.TextEdit txtPassword;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CheckEdit chkShowPassword;
    }
}