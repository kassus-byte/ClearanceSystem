namespace SchoolClearanceSystem
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtUserID = new DevExpress.XtraEditors.TextEdit();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.btnLogin = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.lnkRegister = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.chkShowPassword = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkShowPassword.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Appearance.Options.UseForeColor = true;
            this.labelControl3.Location = new System.Drawing.Point(1325, 371);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(52, 20);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "User ID";
            // 
            // txtUserID
            // 
            this.txtUserID.EditValue = "";
            this.txtUserID.Location = new System.Drawing.Point(1325, 398);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Properties.AutoHeight = false;
            this.txtUserID.Size = new System.Drawing.Size(342, 40);
            this.txtUserID.TabIndex = 6;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(1325, 506);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.AutoHeight = false;
            this.txtPassword.Properties.UseSystemPasswordChar = true;
            this.txtPassword.Size = new System.Drawing.Size(342, 40);
            this.txtPassword.TabIndex = 8;
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelControl8.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl8.Appearance.Options.UseFont = true;
            this.labelControl8.Appearance.Options.UseForeColor = true;
            this.labelControl8.Location = new System.Drawing.Point(1325, 480);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(67, 20);
            this.labelControl8.TabIndex = 14;
            this.labelControl8.Text = "Password";
            // 
            // btnLogin
            // 
            this.btnLogin.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.btnLogin.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.Appearance.Options.UseBackColor = true;
            this.btnLogin.Appearance.Options.UseFont = true;
            this.btnLogin.Location = new System.Drawing.Point(1325, 625);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(342, 47);
            this.btnLogin.TabIndex = 35;
            this.btnLogin.Text = "LOGIN";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.ContentImage = ((System.Drawing.Image)(resources.GetObject("panelControl1.ContentImage")));
            this.panelControl1.Controls.Add(this.labelControl5);
            this.panelControl1.Controls.Add(this.lnkRegister);
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.chkShowPassword);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.labelControl8);
            this.panelControl1.Controls.Add(this.btnLogin);
            this.panelControl1.Controls.Add(this.txtPassword);
            this.panelControl1.Controls.Add(this.txtUserID);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1920, 1080);
            this.panelControl1.TabIndex = 38;
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelControl5.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Appearance.Options.UseForeColor = true;
            this.labelControl5.Location = new System.Drawing.Point(1393, 224);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(195, 17);
            this.labelControl5.TabIndex = 41;
            this.labelControl5.Text = "Please enter your details to login.";
            // 
            // lnkRegister
            // 
            this.lnkRegister.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lnkRegister.Appearance.Options.UseFont = true;
            this.lnkRegister.Location = new System.Drawing.Point(1537, 678);
            this.lnkRegister.Name = "lnkRegister";
            this.lnkRegister.Size = new System.Drawing.Size(48, 17);
            this.lnkRegister.TabIndex = 40;
            this.lnkRegister.Text = "Register";
            this.lnkRegister.Click += new System.EventHandler(this.lnkRegister_Click);
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(1375, 678);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(135, 17);
            this.labelControl4.TabIndex = 39;
            this.labelControl4.Text = "Don\'t have an account?";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.Location = new System.Drawing.Point(1354, 792);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(288, 17);
            this.labelControl2.TabIndex = 38;
            this.labelControl2.Text = "© 2026 ClearEase . College of Computer Studies ";
            // 
            // chkShowPassword
            // 
            this.chkShowPassword.Location = new System.Drawing.Point(1325, 552);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkShowPassword.Properties.Appearance.Options.UseFont = true;
            this.chkShowPassword.Properties.Caption = "Show Password";
            this.chkShowPassword.Size = new System.Drawing.Size(126, 22);
            this.chkShowPassword.TabIndex = 37;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(1438, 182);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(101, 45);
            this.labelControl1.TabIndex = 36;
            this.labelControl1.Text = "LOGIN";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Login";
            this.Text = "Login";
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkShowPassword.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtUserID;
        private DevExpress.XtraEditors.TextEdit txtPassword;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.SimpleButton btnLogin;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit chkShowPassword;
        private DevExpress.XtraEditors.HyperlinkLabelControl lnkRegister;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl5;
    }
}