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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserInfoForm));
            this.txtLastName = new DevExpress.XtraEditors.TextEdit();
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
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.chkShowPassword = new DevExpress.XtraEditors.CheckEdit();
            this.txtFirstName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.txtMiddleName = new DevExpress.XtraEditors.TextEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.cbProgram = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbYear = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbRole = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtLastName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl15)).BeginInit();
            this.panelControl15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDateCreated.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkShowPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFirstName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMiddleName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbProgram.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbYear.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbRole.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(525, 81);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtLastName.Properties.Appearance.Options.UseFont = true;
            this.txtLastName.Size = new System.Drawing.Size(513, 34);
            this.txtLastName.TabIndex = 25;
            this.txtLastName.EditValueChanged += new System.EventHandler(this.txtFullName_EditValueChanged);
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(34, 81);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtUserID.Properties.Appearance.Options.UseFont = true;
            this.txtUserID.Size = new System.Drawing.Size(464, 34);
            this.txtUserID.TabIndex = 23;
            // 
            // btnSave
            // 
            this.btnSave.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.btnSave.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnSave.Appearance.Options.UseBackColor = true;
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Location = new System.Drawing.Point(862, 403);
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
            this.btnCancel.Location = new System.Drawing.Point(701, 403);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(141, 43);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_1);
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.labelControl7.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl7.Appearance.Options.UseFont = true;
            this.labelControl7.Appearance.Options.UseForeColor = true;
            this.labelControl7.Location = new System.Drawing.Point(34, 221);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(32, 21);
            this.labelControl7.TabIndex = 19;
            this.labelControl7.Text = "Year";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.labelControl5.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Appearance.Options.UseForeColor = true;
            this.labelControl5.Location = new System.Drawing.Point(525, 221);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(63, 21);
            this.labelControl5.TabIndex = 17;
            this.labelControl5.Text = "Program";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.labelControl4.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Appearance.Options.UseForeColor = true;
            this.labelControl4.Location = new System.Drawing.Point(525, 58);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(76, 21);
            this.labelControl4.TabIndex = 16;
            this.labelControl4.Text = "Last Name";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Appearance.Options.UseForeColor = true;
            this.labelControl3.Location = new System.Drawing.Point(34, 58);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(53, 21);
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
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(34, 290);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(96, 21);
            this.labelControl1.TabIndex = 32;
            this.labelControl1.Text = "Date Created";
            // 
            // txtDateCreated
            // 
            this.txtDateCreated.Location = new System.Drawing.Point(34, 313);
            this.txtDateCreated.Name = "txtDateCreated";
            this.txtDateCreated.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtDateCreated.Properties.Appearance.Options.UseFont = true;
            this.txtDateCreated.Size = new System.Drawing.Size(464, 34);
            this.txtDateCreated.TabIndex = 33;
            // 
            // lbl
            // 
            this.lbl.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbl.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.lbl.Appearance.Options.UseFont = true;
            this.lbl.Appearance.Options.UseForeColor = true;
            this.lbl.Location = new System.Drawing.Point(34, 137);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(33, 21);
            this.lbl.TabIndex = 34;
            this.lbl.Text = "Role";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(525, 313);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtPassword.Properties.Appearance.Options.UseFont = true;
            this.txtPassword.Size = new System.Drawing.Size(513, 34);
            this.txtPassword.TabIndex = 37;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.Location = new System.Drawing.Point(525, 293);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(69, 21);
            this.labelControl2.TabIndex = 36;
            this.labelControl2.Text = "Password";
            // 
            // chkShowPassword
            // 
            this.chkShowPassword.Location = new System.Drawing.Point(525, 351);
            this.chkShowPassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Properties.Caption = "Show Password";
            this.chkShowPassword.Size = new System.Drawing.Size(134, 22);
            this.chkShowPassword.TabIndex = 44;
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(525, 164);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtFirstName.Properties.Appearance.Options.UseFont = true;
            this.txtFirstName.Size = new System.Drawing.Size(446, 34);
            this.txtFirstName.TabIndex = 45;
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.labelControl8.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl8.Appearance.Options.UseFont = true;
            this.labelControl8.Appearance.Options.UseForeColor = true;
            this.labelControl8.Location = new System.Drawing.Point(977, 141);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(28, 21);
            this.labelControl8.TabIndex = 47;
            this.labelControl8.Text = "M. I";
            // 
            // txtMiddleName
            // 
            this.txtMiddleName.Location = new System.Drawing.Point(977, 164);
            this.txtMiddleName.Name = "txtMiddleName";
            this.txtMiddleName.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtMiddleName.Properties.Appearance.Options.UseFont = true;
            this.txtMiddleName.Size = new System.Drawing.Size(61, 34);
            this.txtMiddleName.TabIndex = 48;
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.ContentImage = ((System.Drawing.Image)(resources.GetObject("panelControl1.ContentImage")));
            this.panelControl1.Controls.Add(this.cbProgram);
            this.panelControl1.Controls.Add(this.cbYear);
            this.panelControl1.Controls.Add(this.cbRole);
            this.panelControl1.Controls.Add(this.txtPassword);
            this.panelControl1.Controls.Add(this.txtFirstName);
            this.panelControl1.Controls.Add(this.labelControl9);
            this.panelControl1.Controls.Add(this.labelControl8);
            this.panelControl1.Controls.Add(this.txtMiddleName);
            this.panelControl1.Controls.Add(this.txtLastName);
            this.panelControl1.Controls.Add(this.chkShowPassword);
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Controls.Add(this.btnSave);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.txtUserID);
            this.panelControl1.Controls.Add(this.lbl);
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Controls.Add(this.labelControl5);
            this.panelControl1.Controls.Add(this.labelControl7);
            this.panelControl1.Controls.Add(this.txtDateCreated);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Location = new System.Drawing.Point(12, 94);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1071, 475);
            this.panelControl1.TabIndex = 49;
            // 
            // cbProgram
            // 
            this.cbProgram.Location = new System.Drawing.Point(525, 248);
            this.cbProgram.Name = "cbProgram";
            this.cbProgram.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cbProgram.Properties.Appearance.Options.UseFont = true;
            this.cbProgram.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbProgram.Properties.Items.AddRange(new object[] {
            "BSIT"});
            this.cbProgram.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbProgram.Size = new System.Drawing.Size(513, 34);
            this.cbProgram.TabIndex = 53;
            // 
            // cbYear
            // 
            this.cbYear.Location = new System.Drawing.Point(34, 248);
            this.cbYear.Name = "cbYear";
            this.cbYear.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cbYear.Properties.Appearance.Options.UseFont = true;
            this.cbYear.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbYear.Properties.Items.AddRange(new object[] {
            "1st Year",
            "2nd Year",
            "3rd Year",
            "4th Year",
            "Irregular"});
            this.cbYear.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbYear.Size = new System.Drawing.Size(464, 34);
            this.cbYear.TabIndex = 52;
            // 
            // cbRole
            // 
            this.cbRole.Location = new System.Drawing.Point(34, 164);
            this.cbRole.Name = "cbRole";
            this.cbRole.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cbRole.Properties.Appearance.Options.UseFont = true;
            this.cbRole.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbRole.Properties.Items.AddRange(new object[] {
            "Student",
            "Admin",
            "Treasurer",
            "Technical Office",
            "SSG"});
            this.cbRole.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbRole.Size = new System.Drawing.Size(464, 34);
            this.cbRole.TabIndex = 51;
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.labelControl9.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl9.Appearance.Options.UseFont = true;
            this.labelControl9.Appearance.Options.UseForeColor = true;
            this.labelControl9.Location = new System.Drawing.Point(525, 141);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(78, 21);
            this.labelControl9.TabIndex = 50;
            this.labelControl9.Text = "First Name";
            // 
            // UserInfoForm
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 603);
            this.Controls.Add(this.panelControl15);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UserInfoForm";
            this.Text = "UserInfoForm";
            this.Load += new System.EventHandler(this.UserInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtLastName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl15)).EndInit();
            this.panelControl15.ResumeLayout(false);
            this.panelControl15.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDateCreated.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkShowPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFirstName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMiddleName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbProgram.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbYear.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbRole.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.TextEdit txtLastName;
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
        private DevExpress.XtraEditors.TextEdit txtPassword;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CheckEdit chkShowPassword;
        private DevExpress.XtraEditors.TextEdit txtFirstName;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.TextEdit txtMiddleName;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.ComboBoxEdit cbProgram;
        private DevExpress.XtraEditors.ComboBoxEdit cbYear;
        private DevExpress.XtraEditors.ComboBoxEdit cbRole;
    }
}