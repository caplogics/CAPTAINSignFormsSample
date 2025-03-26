using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;


namespace Captain
{
    partial class LoginForm
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

        #region Wisej Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            Wisej.Web.JavaScript.ClientEvent clientEvent1 = new Wisej.Web.JavaScript.ClientEvent();
            Wisej.Web.JavaScript.ClientEvent clientEvent2 = new Wisej.Web.JavaScript.ClientEvent();
            this.pnlLogin = new Wisej.Web.Panel();
            this.pnlLoginBox = new Wisej.Web.Panel();
            this.pnlAuthentication = new Wisej.Web.Panel();
            this.pnlauthblock = new Wisej.Web.Panel();
            this.lblSendCodeby = new Wisej.Web.Label();
            this.lblverifyicon = new Wisej.Web.Label();
            this.btnBack = new Wisej.Web.Button();
            this.label7 = new Wisej.Web.Label();
            this.panel1 = new Wisej.Web.Panel();
            this.lblgetcodebyemail = new Wisej.Web.LinkLabel();
            this.linkresend = new Wisej.Web.LinkLabel();
            this.linktryanotheruser = new Wisej.Web.LinkLabel();
            this.lblOTPsentdate = new Wisej.Web.Label();
            this.label6 = new Wisej.Web.Label();
            this.lblOTPUserID = new Wisej.Web.Label();
            this.label4 = new Wisej.Web.Label();
            this.lblOTPMessage = new Wisej.Web.Label();
            this.lblOnetime2 = new Wisej.Web.Label();
            this.spacer1 = new Wisej.Web.Spacer();
            this.chktrusted = new Wisej.Web.CheckBox();
            this.label3 = new Wisej.Web.Label();
            this.lblTimerLeft = new Wisej.Web.Label();
            this.txtverifytext = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblEntertext = new Wisej.Web.Label();
            this.btnValidCaptcher = new Wisej.Web.Button();
            this.pnlUserlogin = new Wisej.Web.Panel();
            this.lnkForgetpassword = new Wisej.Web.LinkLabel();
            this.chkRememberUserName = new Wisej.Web.CheckBox();
            this.lblMessage = new Wisej.Web.Label();
            this.btnLogin = new Wisej.Web.Button();
            this.label2 = new Wisej.Web.Label();
            this.label1 = new Wisej.Web.Label();
            this.txtPassword = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtUserName = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.pnlLogo = new Wisej.Web.Panel();
            this.pictureBox1 = new Wisej.Web.PictureBox();
            this.timer1 = new Wisej.Web.Timer(this.components);
            this.pnlLogin.SuspendLayout();
            this.pnlLoginBox.SuspendLayout();
            this.pnlAuthentication.SuspendLayout();
            this.pnlauthblock.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlUserlogin.SuspendLayout();
            this.pnlLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlLogin
            // 
            this.pnlLogin.BackColor = System.Drawing.Color.Transparent;
            this.pnlLogin.BackgroundImageLayout = Wisej.Web.ImageLayout.Zoom;
            this.pnlLogin.Controls.Add(this.pnlLoginBox);
            this.pnlLogin.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlLogin.Location = new System.Drawing.Point(6, 6);
            this.pnlLogin.Name = "pnlLogin";
            this.pnlLogin.Size = new System.Drawing.Size(618, 553);
            this.pnlLogin.TabIndex = 3;
            this.pnlLogin.TabStop = true;
            this.pnlLogin.Text = "`";
            // 
            // pnlLoginBox
            // 
            this.pnlLoginBox.BackColor = System.Drawing.Color.Transparent;
            this.pnlLoginBox.Controls.Add(this.pnlAuthentication);
            this.pnlLoginBox.Controls.Add(this.pnlUserlogin);
            this.pnlLoginBox.Controls.Add(this.pnlLogo);
            this.pnlLoginBox.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlLoginBox.Location = new System.Drawing.Point(0, 0);
            this.pnlLoginBox.Name = "pnlLoginBox";
            this.pnlLoginBox.Padding = new Wisej.Web.Padding(0, 20, 0, 0);
            this.pnlLoginBox.Size = new System.Drawing.Size(618, 553);
            this.pnlLoginBox.TabIndex = 0;
            this.pnlLoginBox.TabStop = true;
            // 
            // pnlAuthentication
            // 
            this.pnlAuthentication.Controls.Add(this.pnlauthblock);
            this.pnlAuthentication.Dock = Wisej.Web.DockStyle.Top;
            this.pnlAuthentication.Location = new System.Drawing.Point(0, 285);
            this.pnlAuthentication.Name = "pnlAuthentication";
            this.pnlAuthentication.Size = new System.Drawing.Size(618, 267);
            this.pnlAuthentication.TabIndex = 40;
            this.pnlAuthentication.TabStop = true;
            this.pnlAuthentication.Visible = false;
            // 
            // pnlauthblock
            // 
            this.pnlauthblock.Controls.Add(this.lblSendCodeby);
            this.pnlauthblock.Controls.Add(this.lblverifyicon);
            this.pnlauthblock.Controls.Add(this.btnBack);
            this.pnlauthblock.Controls.Add(this.label7);
            this.pnlauthblock.Controls.Add(this.panel1);
            this.pnlauthblock.Controls.Add(this.lblOTPsentdate);
            this.pnlauthblock.Controls.Add(this.label6);
            this.pnlauthblock.Controls.Add(this.lblOTPUserID);
            this.pnlauthblock.Controls.Add(this.label4);
            this.pnlauthblock.Controls.Add(this.lblOTPMessage);
            this.pnlauthblock.Controls.Add(this.lblOnetime2);
            this.pnlauthblock.Controls.Add(this.spacer1);
            this.pnlauthblock.Controls.Add(this.chktrusted);
            this.pnlauthblock.Controls.Add(this.label3);
            this.pnlauthblock.Controls.Add(this.lblTimerLeft);
            this.pnlauthblock.Controls.Add(this.txtverifytext);
            this.pnlauthblock.Controls.Add(this.lblEntertext);
            this.pnlauthblock.Controls.Add(this.btnValidCaptcher);
            this.pnlauthblock.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlauthblock.Location = new System.Drawing.Point(0, 0);
            this.pnlauthblock.Name = "pnlauthblock";
            this.pnlauthblock.Padding = new Wisej.Web.Padding(15, 8, 15, 0);
            this.pnlauthblock.Size = new System.Drawing.Size(618, 267);
            this.pnlauthblock.TabIndex = 40;
            this.pnlauthblock.TabStop = true;
            this.pnlauthblock.Visible = false;
            // 
            // lblSendCodeby
            // 
            this.lblSendCodeby.Location = new System.Drawing.Point(302, 228);
            this.lblSendCodeby.Name = "lblSendCodeby";
            this.lblSendCodeby.Size = new System.Drawing.Size(97, 17);
            this.lblSendCodeby.TabIndex = 63;
            this.lblSendCodeby.Text = "Phone missing ?";
            // 
            // lblverifyicon
            // 
            this.lblverifyicon.AutoSize = true;
            this.lblverifyicon.Location = new System.Drawing.Point(102, 192);
            this.lblverifyicon.Name = "lblverifyicon";
            this.lblverifyicon.Size = new System.Drawing.Size(4, 14);
            this.lblverifyicon.TabIndex = 61;
            // 
            // btnBack
            // 
            this.btnBack.AppearanceKey = "button-error";
            this.btnBack.Font = new System.Drawing.Font("@loginText", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnBack.Location = new System.Drawing.Point(31, 210);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(96, 27);
            this.btnBack.TabIndex = 59;
            this.btnBack.Text = "&Back";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label7.Location = new System.Drawing.Point(31, 108);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 18);
            this.label7.TabIndex = 58;
            this.label7.Text = "To : ";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblgetcodebyemail);
            this.panel1.Controls.Add(this.linkresend);
            this.panel1.Controls.Add(this.linktryanotheruser);
            this.panel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(15, 243);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(588, 24);
            this.panel1.TabIndex = 57;
            // 
            // lblgetcodebyemail
            // 
            this.lblgetcodebyemail.BackColor = System.Drawing.Color.Transparent;
            this.lblgetcodebyemail.Font = new System.Drawing.Font("@loginText", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblgetcodebyemail.ForeColor = System.Drawing.Color.Blue;
            this.lblgetcodebyemail.LinkColor = System.Drawing.Color.Blue;
            this.lblgetcodebyemail.Location = new System.Drawing.Point(268, 2);
            this.lblgetcodebyemail.MinimumSize = new System.Drawing.Size(0, 15);
            this.lblgetcodebyemail.Name = "lblgetcodebyemail";
            this.lblgetcodebyemail.Size = new System.Drawing.Size(130, 19);
            this.lblgetcodebyemail.TabIndex = 49;
            this.lblgetcodebyemail.Text = "Get code via email";
            this.lblgetcodebyemail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblgetcodebyemail.LinkClicked += new Wisej.Web.LinkLabelLinkClickedEventHandler(this.lblgetcodebyemail_LinkClicked);
            // 
            // linkresend
            // 
            this.linkresend.AutoSize = true;
            this.linkresend.BackColor = System.Drawing.Color.Transparent;
            this.linkresend.Dock = Wisej.Web.DockStyle.Left;
            this.linkresend.Font = new System.Drawing.Font("@loginText", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.linkresend.ForeColor = System.Drawing.Color.Blue;
            this.linkresend.LinkColor = System.Drawing.Color.Blue;
            this.linkresend.Location = new System.Drawing.Point(0, 0);
            this.linkresend.MinimumSize = new System.Drawing.Size(0, 20);
            this.linkresend.Name = "linkresend";
            this.linkresend.Size = new System.Drawing.Size(72, 24);
            this.linkresend.TabIndex = 45;
            this.linkresend.Text = "Resend Text";
            this.linkresend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkresend.Visible = false;
            this.linkresend.LinkClicked += new Wisej.Web.LinkLabelLinkClickedEventHandler(this.linkresend_LinkClicked);
            // 
            // linktryanotheruser
            // 
            this.linktryanotheruser.BackColor = System.Drawing.Color.Transparent;
            this.linktryanotheruser.Dock = Wisej.Web.DockStyle.Right;
            this.linktryanotheruser.Font = new System.Drawing.Font("@loginText", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.linktryanotheruser.ForeColor = System.Drawing.Color.Blue;
            this.linktryanotheruser.LinkColor = System.Drawing.Color.Blue;
            this.linktryanotheruser.Location = new System.Drawing.Point(458, 0);
            this.linktryanotheruser.MinimumSize = new System.Drawing.Size(0, 15);
            this.linktryanotheruser.Name = "linktryanotheruser";
            this.linktryanotheruser.Size = new System.Drawing.Size(130, 24);
            this.linktryanotheruser.TabIndex = 48;
            this.linktryanotheruser.Text = "Try with Another User";
            this.linktryanotheruser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linktryanotheruser.Visible = false;
            this.linktryanotheruser.LinkClicked += new Wisej.Web.LinkLabelLinkClickedEventHandler(this.linktryanotheruser_LinkClicked);
            // 
            // lblOTPsentdate
            // 
            this.lblOTPsentdate.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblOTPsentdate.Location = new System.Drawing.Point(81, 87);
            this.lblOTPsentdate.Name = "lblOTPsentdate";
            this.lblOTPsentdate.Size = new System.Drawing.Size(276, 18);
            this.lblOTPsentdate.TabIndex = 56;
            this.lblOTPsentdate.Text = "----";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label6.Location = new System.Drawing.Point(31, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 18);
            this.label6.TabIndex = 55;
            this.label6.Text = "Sent at : ";
            // 
            // lblOTPUserID
            // 
            this.lblOTPUserID.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblOTPUserID.Location = new System.Drawing.Point(108, 65);
            this.lblOTPUserID.Name = "lblOTPUserID";
            this.lblOTPUserID.Size = new System.Drawing.Size(158, 18);
            this.lblOTPUserID.TabIndex = 54;
            this.lblOTPUserID.Text = "----";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label4.Location = new System.Drawing.Point(31, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 18);
            this.label4.TabIndex = 53;
            this.label4.Text = "CAPTAIN ID :";
            // 
            // lblOTPMessage
            // 
            this.lblOTPMessage.Location = new System.Drawing.Point(31, 41);
            this.lblOTPMessage.Name = "lblOTPMessage";
            this.lblOTPMessage.Size = new System.Drawing.Size(215, 20);
            this.lblOTPMessage.TabIndex = 52;
            this.lblOTPMessage.Text = "Check your phone for the code";
            // 
            // lblOnetime2
            // 
            this.lblOnetime2.BackColor = System.Drawing.Color.Transparent;
            this.lblOnetime2.CssStyle = "border-radius:15px;";
            this.lblOnetime2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblOnetime2.ForeColor = System.Drawing.Color.FromArgb(157, 51, 70);
            this.lblOnetime2.Location = new System.Drawing.Point(62, 108);
            this.lblOnetime2.Name = "lblOnetime2";
            this.lblOnetime2.Size = new System.Drawing.Size(306, 18);
            this.lblOnetime2.TabIndex = 40;
            this.lblOnetime2.Text = "-----";
            this.lblOnetime2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblOnetime2.Visible = false;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Top;
            this.spacer1.Location = new System.Drawing.Point(15, 33);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(588, 1);
            // 
            // chktrusted
            // 
            this.chktrusted.AutoSize = false;
            this.chktrusted.Location = new System.Drawing.Point(31, 173);
            this.chktrusted.Name = "chktrusted";
            this.chktrusted.Size = new System.Drawing.Size(337, 21);
            this.chktrusted.TabIndex = 51;
            this.chktrusted.Text = "I trust this device. Don\'t ask me for a code next time";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromName("@captainBrown");
            this.label3.CssStyle = "border-radius:15px;";
            this.label3.Dock = Wisej.Web.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("default", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.label3.Location = new System.Drawing.Point(15, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(588, 25);
            this.label3.TabIndex = 50;
            this.label3.Text = "Enter Security Code to Login";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTimerLeft
            // 
            this.lblTimerLeft.ForeColor = System.Drawing.Color.FromArgb(15, 109, 180);
            this.lblTimerLeft.Location = new System.Drawing.Point(242, 148);
            this.lblTimerLeft.Name = "lblTimerLeft";
            this.lblTimerLeft.Size = new System.Drawing.Size(96, 18);
            this.lblTimerLeft.TabIndex = 49;
            this.lblTimerLeft.Text = "(Valid for 2 mins)";
            this.lblTimerLeft.Visible = false;
            // 
            // txtverifytext
            // 
            this.txtverifytext.AutoSize = false;
            this.txtverifytext.BackColor = System.Drawing.Color.White;
            this.txtverifytext.Location = new System.Drawing.Point(95, 143);
            this.txtverifytext.MaxLength = 6;
            this.txtverifytext.Name = "txtverifytext";
            this.txtverifytext.Padding = new Wisej.Web.Padding(7, 0, 0, 0);
            this.txtverifytext.Size = new System.Drawing.Size(137, 26);
            this.txtverifytext.TabIndex = 46;
            this.txtverifytext.Visible = false;
            this.txtverifytext.Watermark = "Six-digit code";
            // 
            // lblEntertext
            // 
            this.lblEntertext.AutoSize = true;
            this.lblEntertext.Font = new System.Drawing.Font("@loginText", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblEntertext.Location = new System.Drawing.Point(31, 148);
            this.lblEntertext.Name = "lblEntertext";
            this.lblEntertext.Size = new System.Drawing.Size(60, 14);
            this.lblEntertext.TabIndex = 47;
            this.lblEntertext.Text = "Enter Text";
            this.lblEntertext.Visible = false;
            // 
            // btnValidCaptcher
            // 
            this.btnValidCaptcher.AppearanceKey = "button-ok";
            this.btnValidCaptcher.Font = new System.Drawing.Font("@loginText", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnValidCaptcher.Location = new System.Drawing.Point(358, 143);
            this.btnValidCaptcher.Name = "btnValidCaptcher";
            this.btnValidCaptcher.Size = new System.Drawing.Size(168, 27);
            this.btnValidCaptcher.TabIndex = 44;
            this.btnValidCaptcher.Text = "&Proceed with Verification";
            this.btnValidCaptcher.Visible = false;
            this.btnValidCaptcher.Click += new System.EventHandler(this.btnValidCaptcher_Click);
            // 
            // pnlUserlogin
            // 
            this.pnlUserlogin.Controls.Add(this.lnkForgetpassword);
            this.pnlUserlogin.Controls.Add(this.chkRememberUserName);
            this.pnlUserlogin.Controls.Add(this.lblMessage);
            this.pnlUserlogin.Controls.Add(this.btnLogin);
            this.pnlUserlogin.Controls.Add(this.label2);
            this.pnlUserlogin.Controls.Add(this.label1);
            this.pnlUserlogin.Controls.Add(this.txtPassword);
            this.pnlUserlogin.Controls.Add(this.txtUserName);
            this.pnlUserlogin.Dock = Wisej.Web.DockStyle.Top;
            this.pnlUserlogin.Location = new System.Drawing.Point(0, 131);
            this.pnlUserlogin.Name = "pnlUserlogin";
            this.pnlUserlogin.Size = new System.Drawing.Size(618, 154);
            this.pnlUserlogin.TabIndex = 42;
            this.pnlUserlogin.TabStop = true;
            // 
            // lnkForgetpassword
            // 
            this.lnkForgetpassword.Location = new System.Drawing.Point(389, 129);
            this.lnkForgetpassword.Name = "lnkForgetpassword";
            this.lnkForgetpassword.Size = new System.Drawing.Size(106, 18);
            this.lnkForgetpassword.TabIndex = 39;
            this.lnkForgetpassword.Text = "Forgot Password?";
            this.lnkForgetpassword.Visible = false;
            this.lnkForgetpassword.LinkClicked += new Wisej.Web.LinkLabelLinkClickedEventHandler(this.lnkForgetpassword_LinkClicked);
            // 
            // chkRememberUserName
            // 
            this.chkRememberUserName.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkRememberUserName.Location = new System.Drawing.Point(127, 88);
            this.chkRememberUserName.Name = "chkRememberUserName";
            this.chkRememberUserName.Size = new System.Drawing.Size(136, 21);
            this.chkRememberUserName.TabIndex = 38;
            this.chkRememberUserName.Text = "Remember User ID";
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = Wisej.Web.AnchorStyles.None;
            this.lblMessage.AutoSize = true;
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Location = new System.Drawing.Point(486, 192);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(4, 14);
            this.lblMessage.TabIndex = 36;
            // 
            // btnLogin
            // 
            this.btnLogin.AppearanceKey = "button-login";
            this.btnLogin.DialogResult = Wisej.Web.DialogResult.OK;
            this.btnLogin.Font = new System.Drawing.Font("@loginText", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnLogin.Location = new System.Drawing.Point(342, 88);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(98, 27);
            this.btnLogin.TabIndex = 35;
            this.btnLogin.Text = "Login";
            this.btnLogin.Click += new System.EventHandler(this.LoginClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(16, 62, 109);
            this.label2.Location = new System.Drawing.Point(62, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 14);
            this.label2.TabIndex = 34;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(16, 62, 109);
            this.label1.Location = new System.Drawing.Point(62, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 14);
            this.label1.TabIndex = 32;
            this.label1.Text = "User ID";
            // 
            // txtPassword
            // 
            this.txtPassword.AutoSize = false;
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.txtPassword.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPassword.ForeColor = System.Drawing.Color.FromArgb(16, 62, 109);
            this.txtPassword.InputType.Type = Wisej.Web.TextBoxType.Password;
            this.txtPassword.Location = new System.Drawing.Point(127, 45);
            this.txtPassword.MaxLength = 20;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Padding = new Wisej.Web.Padding(7, 0, 0, 0);
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(313, 30);
            this.txtPassword.TabIndex = 31;
            this.txtPassword.Enter += new System.EventHandler(this.txtPassword_GotFocus);
            // 
            // txtUserName
            // 
            this.txtUserName.AutoSize = false;
            this.txtUserName.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.txtUserName.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtUserName.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtUserName.ForeColor = System.Drawing.Color.FromArgb(16, 62, 109);
            this.txtUserName.Location = new System.Drawing.Point(127, 10);
            this.txtUserName.MaxLength = 20;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Padding = new Wisej.Web.Padding(7, 0, 0, 0);
            this.txtUserName.Size = new System.Drawing.Size(313, 30);
            this.txtUserName.TabIndex = 30;
            // 
            // pnlLogo
            // 
            this.pnlLogo.Controls.Add(this.pictureBox1);
            this.pnlLogo.Dock = Wisej.Web.DockStyle.Top;
            this.pnlLogo.Location = new System.Drawing.Point(0, 20);
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(618, 111);
            this.pnlLogo.TabIndex = 43;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = Wisej.Web.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(618, 111);
            this.pictureBox1.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // LoginForm
            // 
            this.AcceptButton = this.btnLogin;
            this.BackColor = System.Drawing.Color.FromArgb(237, 255, 255, 255);
            clientEvent1.Event = "appear";
            clientEvent1.JavaScript = resources.GetString("clientEvent1.JavaScript");
            clientEvent2.Event = "disappear";
            clientEvent2.JavaScript = "var root = qx.core.Init.getApplication().getRoot();\r\nroot.getBlocker().resetBackg" +
    "roundImage();\r\n";
            this.ClientEvents.Add(clientEvent1);
            this.ClientEvents.Add(clientEvent2);
            this.ClientSize = new System.Drawing.Size(630, 565);
            this.Controls.Add(this.pnlLogin);
            this.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.None;
            this.KeepCentered = true;
            this.Name = "LoginForm";
            this.Padding = new Wisej.Web.Padding(6);
            this.StartPosition = Wisej.Web.FormStartPosition.CenterScreen;
            this.Text = "LoginForm";
            this.Load += new System.EventHandler(this.LoginFormLoad);
            this.pnlLogin.ResumeLayout(false);
            this.pnlLoginBox.ResumeLayout(false);
            this.pnlAuthentication.ResumeLayout(false);
            this.pnlauthblock.ResumeLayout(false);
            this.pnlauthblock.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlUserlogin.ResumeLayout(false);
            this.pnlUserlogin.PerformLayout();
            this.pnlLogo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel pnlLogin;
        private Panel pnlLoginBox;
        private Timer timer1;
        private Panel pnlAuthentication;
        private Panel pnlauthblock;
        private LinkLabel linktryanotheruser;
        private TextBoxWithValidation txtverifytext;
        private LinkLabel linkresend;
        private Label lblEntertext;
        private Button btnValidCaptcher;
        private Label lblMessage;
        private Button btnLogin;
        private Label label2;
        private Label label1;
        private TextBoxWithValidation txtPassword;
        private TextBoxWithValidation txtUserName;
        private PictureBox pictureBox1;
        private Panel pnlUserlogin;
        private CheckBox chkRememberUserName;
        private Label lblOnetime2;
        private Panel pnlLogo;
        private Label lblTimerLeft;
        private Label label3;
        private CheckBox chktrusted;
        private Spacer spacer1;
        private LinkLabel lnkForgetpassword;
        private Label lblOTPMessage;
        private Label label4;
        private Label lblOTPsentdate;
        private Label label6;
        private Label lblOTPUserID;
        private Panel panel1;
        private Label label7;
        private Button btnBack;
        private Label lblverifyicon;
        private Label lblSendCodeby;
        private LinkLabel lblgetcodebyemail;
    }
}