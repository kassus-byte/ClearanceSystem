namespace SchoolClearanceSystem
{
    partial class Registration
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
            this.txtUserID = new DevExpress.XtraEditors.TextEdit();
            this.txtFullName = new DevExpress.XtraEditors.TextEdit();
            this.btnRegister = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.cmbProgram = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cmbYear = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.lblctrLogin = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnUpload = new DevExpress.XtraEditors.SimpleButton();
            this.txtUploadPath = new DevExpress.XtraEditors.TextEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFullName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProgram.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbYear.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUploadPath.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(1217, 368);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Properties.AutoHeight = false;
            this.txtUserID.Size = new System.Drawing.Size(262, 40);
            this.txtUserID.TabIndex = 0;
            // 
            // txtFullName
            // 
            this.txtFullName.Location = new System.Drawing.Point(1217, 451);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Properties.AutoHeight = false;
            this.txtFullName.Size = new System.Drawing.Size(262, 40);
            this.txtFullName.TabIndex = 1;
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(1217, 884);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(262, 47);
            this.btnRegister.TabIndex = 2;
            this.btnRegister.Text = "REGISTER";
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click_1);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(1166, 193);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(398, 50);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "DMC COLLEGE FOUNDATION, INC.\r\n\r\n";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(1280, 249);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(118, 25);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "REGISTER";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(1217, 344);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(51, 18);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "User ID";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(1217, 427);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(64, 18);
            this.labelControl4.TabIndex = 6;
            this.labelControl4.Text = "Full Name";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(1367, 841);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(115, 18);
            this.labelControl5.TabIndex = 7;
            this.labelControl5.Text = "Forgot Password?";
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl6.Appearance.Options.UseFont = true;
            this.labelControl6.Location = new System.Drawing.Point(1217, 509);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(55, 18);
            this.labelControl6.TabIndex = 9;
            this.labelControl6.Text = "Program";
            // 
            // cmbProgram
            // 
            this.cmbProgram.Location = new System.Drawing.Point(1217, 533);
            this.cmbProgram.Name = "cmbProgram";
            this.cmbProgram.Properties.AutoHeight = false;
            this.cmbProgram.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbProgram.Properties.Items.AddRange(new object[] {
            "BSIT"});
            this.cmbProgram.Size = new System.Drawing.Size(262, 40);
            this.cmbProgram.TabIndex = 10;
            // 
            // cmbYear
            // 
            this.cmbYear.Location = new System.Drawing.Point(1217, 615);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Properties.AutoHeight = false;
            this.cmbYear.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbYear.Properties.Items.AddRange(new object[] {
            "I",
            "II",
            "III",
            "IV"});
            this.cmbYear.Size = new System.Drawing.Size(262, 40);
            this.cmbYear.TabIndex = 11;
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl7.Appearance.Options.UseFont = true;
            this.labelControl7.Location = new System.Drawing.Point(1217, 591);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(31, 18);
            this.labelControl7.TabIndex = 12;
            this.labelControl7.Text = "Year";
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl8.Appearance.Options.UseFont = true;
            this.labelControl8.Location = new System.Drawing.Point(1220, 670);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(61, 18);
            this.labelControl8.TabIndex = 13;
            this.labelControl8.Text = "Password";
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl9.Appearance.Options.UseFont = true;
            this.labelControl9.Location = new System.Drawing.Point(1229, 954);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(169, 18);
            this.labelControl9.TabIndex = 14;
            this.labelControl9.Text = "Already have an account?";
            // 
            // lblctrLogin
            // 
            this.lblctrLogin.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblctrLogin.Appearance.Options.UseFont = true;
            this.lblctrLogin.Location = new System.Drawing.Point(1404, 954);
            this.lblctrLogin.Name = "lblctrLogin";
            this.lblctrLogin.Size = new System.Drawing.Size(44, 18);
            this.lblctrLogin.TabIndex = 15;
            this.lblctrLogin.Text = "LOGIN";
            this.lblctrLogin.Click += new System.EventHandler(this.lblctrLogin_Click_1);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(1220, 694);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.AutoHeight = false;
            this.txtPassword.Size = new System.Drawing.Size(262, 40);
            this.txtPassword.TabIndex = 8;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(1217, 770);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(262, 39);
            this.btnUpload.TabIndex = 16;
            this.btnUpload.Text = "Upload Verified ID";
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // txtUploadPath
            // 
            this.txtUploadPath.Location = new System.Drawing.Point(1485, 776);
            this.txtUploadPath.Name = "txtUploadPath";
            this.txtUploadPath.Size = new System.Drawing.Size(100, 28);
            this.txtUploadPath.TabIndex = 17;
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(80, 144);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(33, 13);
            this.labelControl10.TabIndex = 18;
            this.labelControl10.Text = "testing";
            // 
            // Registration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1918, 1046);
            this.Controls.Add(this.labelControl10);
            this.Controls.Add(this.txtUploadPath);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.lblctrLogin);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.cmbYear);
            this.Controls.Add(this.cmbProgram);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.txtFullName);
            this.Controls.Add(this.txtUserID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Registration";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFullName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProgram.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbYear.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUploadPath.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtUserID;
        private DevExpress.XtraEditors.TextEdit txtFullName;
        private DevExpress.XtraEditors.SimpleButton btnRegister;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.ComboBoxEdit cmbProgram;
        private DevExpress.XtraEditors.ComboBoxEdit cmbYear;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.HyperlinkLabelControl lblctrLogin;
        private DevExpress.XtraEditors.TextEdit txtPassword;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private DevExpress.XtraEditors.SimpleButton btnUpload;
        private DevExpress.XtraEditors.TextEdit txtUploadPath;
        private DevExpress.XtraEditors.LabelControl labelControl10;
    }
}

