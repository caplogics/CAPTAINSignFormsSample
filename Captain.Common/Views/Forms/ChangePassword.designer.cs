using Wisej.Web;

namespace Captain
{
    partial class ChangePassword
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

        #region Visual WebGui Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangePassword));
            this.pnlResetPswrd = new Wisej.Web.Panel();
            this.pnlResetChb = new Wisej.Web.Panel();
            this.chkForcepwd = new Wisej.Web.CheckBox();
            this.lblDays = new Wisej.Web.Label();
            this.txtpwdDays = new Wisej.Web.TextBox();
            this.label2 = new Wisej.Web.Label();
            this.pnlSave = new Wisej.Web.Panel();
            this.btnPwdUpdate = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.pnlChangePswrd = new Wisej.Web.Panel();
            this.pnlChangePwsrd = new Wisej.Web.Panel();
            this.btnChangePassword = new Wisej.Web.Button();
            this.txtOldPassword = new Wisej.Web.TextBox();
            this.txtNewPassword = new Wisej.Web.TextBox();
            this.lblOldPassword = new Wisej.Web.Label();
            this.lblNewPassword = new Wisej.Web.Label();
            this.lblMessage = new Wisej.Web.Label();
            this.lblConfirmPassword = new Wisej.Web.Label();
            this.txtConfirmPassword = new Wisej.Web.TextBox();
            this.label3 = new Wisej.Web.Label();
            this.pnlResetPswrd.SuspendLayout();
            this.pnlResetChb.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.pnlChangePswrd.SuspendLayout();
            this.pnlChangePwsrd.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlResetPswrd
            // 
            this.pnlResetPswrd.Controls.Add(this.pnlResetChb);
            this.pnlResetPswrd.Controls.Add(this.pnlSave);
            this.pnlResetPswrd.Dock = Wisej.Web.DockStyle.Top;
            this.pnlResetPswrd.Location = new System.Drawing.Point(0, 0);
            this.pnlResetPswrd.Name = "pnlResetPswrd";
            this.pnlResetPswrd.Size = new System.Drawing.Size(421, 116);
            this.pnlResetPswrd.TabIndex = 6;
            this.pnlResetPswrd.Visible = false;
            // 
            // pnlResetChb
            // 
            this.pnlResetChb.Controls.Add(this.chkForcepwd);
            this.pnlResetChb.Controls.Add(this.lblDays);
            this.pnlResetChb.Controls.Add(this.txtpwdDays);
            this.pnlResetChb.Controls.Add(this.label2);
            this.pnlResetChb.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlResetChb.Location = new System.Drawing.Point(0, 0);
            this.pnlResetChb.Name = "pnlResetChb";
            this.pnlResetChb.Size = new System.Drawing.Size(421, 81);
            this.pnlResetChb.TabIndex = 6;
            // 
            // chkForcepwd
            // 
            this.chkForcepwd.AutoSize = false;
            this.chkForcepwd.Location = new System.Drawing.Point(96, 9);
            this.chkForcepwd.Name = "chkForcepwd";
            this.chkForcepwd.Size = new System.Drawing.Size(200, 20);
            this.chkForcepwd.TabIndex = 0;
            this.chkForcepwd.Text = "Force User to reset Password";
            // 
            // lblDays
            // 
            this.lblDays.AutoSize = true;
            this.lblDays.Location = new System.Drawing.Point(199, 43);
            this.lblDays.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblDays.Name = "lblDays";
            this.lblDays.Size = new System.Drawing.Size(32, 18);
            this.lblDays.TabIndex = 5;
            this.lblDays.Text = "Days";
            // 
            // txtpwdDays
            // 
            this.txtpwdDays.Location = new System.Drawing.Point(161, 40);
            this.txtpwdDays.MaxLength = 3;
            this.txtpwdDays.Name = "txtpwdDays";
            this.txtpwdDays.Size = new System.Drawing.Size(29, 25);
            this.txtpwdDays.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(100, 43);
            this.label2.MinimumSize = new System.Drawing.Size(0, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "For every";
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnPwdUpdate);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 81);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(421, 35);
            this.pnlSave.TabIndex = 7;
            // 
            // btnPwdUpdate
            // 
            this.btnPwdUpdate.AppearanceKey = "button-ok";
            this.btnPwdUpdate.Dock = Wisej.Web.DockStyle.Right;
            this.btnPwdUpdate.Location = new System.Drawing.Point(253, 5);
            this.btnPwdUpdate.Name = "btnPwdUpdate";
            this.btnPwdUpdate.Size = new System.Drawing.Size(75, 25);
            this.btnPwdUpdate.TabIndex = 4;
            this.btnPwdUpdate.Text = "&Save";
            this.btnPwdUpdate.Click += new System.EventHandler(this.btnPwdUpdate_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(328, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(331, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.button1_Click);
            // 
            // pnlChangePswrd
            // 
            this.pnlChangePswrd.Controls.Add(this.pnlChangePwsrd);
            this.pnlChangePswrd.Controls.Add(this.txtOldPassword);
            this.pnlChangePswrd.Controls.Add(this.txtNewPassword);
            this.pnlChangePswrd.Controls.Add(this.lblOldPassword);
            this.pnlChangePswrd.Controls.Add(this.lblNewPassword);
            this.pnlChangePswrd.Controls.Add(this.lblMessage);
            this.pnlChangePswrd.Controls.Add(this.lblConfirmPassword);
            this.pnlChangePswrd.Controls.Add(this.txtConfirmPassword);
            this.pnlChangePswrd.Controls.Add(this.label3);
            this.pnlChangePswrd.CssStyle = "border:1px solid #f2f2f2;";
            this.pnlChangePswrd.Dock = Wisej.Web.DockStyle.Top;
            this.pnlChangePswrd.Location = new System.Drawing.Point(0, 116);
            this.pnlChangePswrd.Name = "pnlChangePswrd";
            this.pnlChangePswrd.Size = new System.Drawing.Size(421, 169);
            this.pnlChangePswrd.TabIndex = 0;
            // 
            // pnlChangePwsrd
            // 
            this.pnlChangePwsrd.AppearanceKey = "panel-grdo";
            this.pnlChangePwsrd.Controls.Add(this.btnChangePassword);
            this.pnlChangePwsrd.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlChangePwsrd.Location = new System.Drawing.Point(0, 134);
            this.pnlChangePwsrd.Name = "pnlChangePwsrd";
            this.pnlChangePwsrd.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlChangePwsrd.Size = new System.Drawing.Size(421, 35);
            this.pnlChangePwsrd.TabIndex = 6;
            // 
            // btnChangePassword
            // 
            this.btnChangePassword.Dock = Wisej.Web.DockStyle.Right;
            this.btnChangePassword.Location = new System.Drawing.Point(285, 5);
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Size = new System.Drawing.Size(121, 25);
            this.btnChangePassword.TabIndex = 3;
            this.btnChangePassword.Text = "&Change Password";
            this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
            // 
            // txtOldPassword
            // 
            this.txtOldPassword.InputType.Type = Wisej.Web.TextBoxType.Password;
            this.txtOldPassword.Location = new System.Drawing.Point(126, 24);
            this.txtOldPassword.Name = "txtOldPassword";
            this.txtOldPassword.PasswordChar = '*';
            this.txtOldPassword.Size = new System.Drawing.Size(272, 25);
            this.txtOldPassword.TabIndex = 0;
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.InputType.Type = Wisej.Web.TextBoxType.Password;
            this.txtNewPassword.Location = new System.Drawing.Point(126, 54);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.PasswordChar = '*';
            this.txtNewPassword.Size = new System.Drawing.Size(271, 25);
            this.txtNewPassword.TabIndex = 1;
            this.txtNewPassword.Leave += new System.EventHandler(this.txtNewPassword_Leave);
            // 
            // lblOldPassword
            // 
            this.lblOldPassword.AutoSize = true;
            this.lblOldPassword.Location = new System.Drawing.Point(11, 27);
            this.lblOldPassword.Name = "lblOldPassword";
            this.lblOldPassword.Size = new System.Drawing.Size(79, 14);
            this.lblOldPassword.TabIndex = 2;
            this.lblOldPassword.Text = "Old Password";
            // 
            // lblNewPassword
            // 
            this.lblNewPassword.AutoSize = true;
            this.lblNewPassword.Location = new System.Drawing.Point(11, 57);
            this.lblNewPassword.Name = "lblNewPassword";
            this.lblNewPassword.Size = new System.Drawing.Size(84, 14);
            this.lblNewPassword.TabIndex = 3;
            this.lblNewPassword.Text = "New Password";
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Location = new System.Drawing.Point(129, 5);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(4, 14);
            this.lblMessage.TabIndex = 5;
            // 
            // lblConfirmPassword
            // 
            this.lblConfirmPassword.Location = new System.Drawing.Point(11, 87);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(110, 22);
            this.lblConfirmPassword.TabIndex = 3;
            this.lblConfirmPassword.Text = "Confirm Password";
            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.InputType.Type = Wisej.Web.TextBoxType.Password;
            this.txtConfirmPassword.Location = new System.Drawing.Point(126, 84);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.PasswordChar = '*';
            this.txtConfirmPassword.Size = new System.Drawing.Size(271, 25);
            this.txtConfirmPassword.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(130, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(4, 14);
            this.label3.TabIndex = 5;
            // 
            // ChangePassword
            // 
            this.ClientSize = new System.Drawing.Size(421, 284);
            this.Controls.Add(this.pnlChangePswrd);
            this.Controls.Add(this.pnlResetPswrd);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangePassword";
            this.Text = "Change Password";
            this.pnlResetPswrd.ResumeLayout(false);
            this.pnlResetChb.ResumeLayout(false);
            this.pnlResetChb.PerformLayout();
            this.pnlSave.ResumeLayout(false);
            this.pnlChangePswrd.ResumeLayout(false);
            this.pnlChangePswrd.PerformLayout();
            this.pnlChangePwsrd.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Panel pnlChangePswrd;
        private TextBox txtOldPassword;
        private TextBox txtNewPassword;
        private Label lblOldPassword;
        private Label lblNewPassword;
        private Label lblMessage;
        private Label lblConfirmPassword;
        private TextBox txtConfirmPassword;
        private Button btnChangePassword;
        private Label label3;
        private Panel pnlResetPswrd;
        private Button btnCancel;
        private Button btnPwdUpdate;
        private Label label2;
        private TextBox txtpwdDays;
        private CheckBox chkForcepwd;
        private Label lblDays;
        private Panel pnlSave;
        private Panel pnlResetChb;
        private Spacer spacer1;
        private Panel pnlChangePwsrd;
    }
}