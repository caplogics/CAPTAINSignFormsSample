namespace Captain
{
    partial class ForgotPassword
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

        #region Wisej Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ForgotPassword));
            this.panel1 = new Wisej.Web.Panel();
            this.btnSetpBack1 = new Wisej.Web.Button();
            this.pnlStep3 = new Wisej.Web.Panel();
            this.lblNewPassword = new Wisej.Web.Label();
            this.lblConfirmPassword = new Wisej.Web.Label();
            this.btnSavePassword = new Wisej.Web.Button();
            this.label5 = new Wisej.Web.Label();
            this.txtConfirmPassword = new Wisej.Web.TextBox();
            this.txtNewPassword = new Wisej.Web.TextBox();
            this.pnlStep2 = new Wisej.Web.Panel();
            this.lblEntertext = new Wisej.Web.Label();
            this.linkresend = new Wisej.Web.LinkLabel();
            this.lblTimerLeft = new Wisej.Web.Label();
            this.btnValidCaptcher = new Wisej.Web.Button();
            this.txtverifytext = new Wisej.Web.TextBox();
            this.lblOnetime2 = new Wisej.Web.Label();
            this.pnlStep1 = new Wisej.Web.Panel();
            this.panel4 = new Wisej.Web.Panel();
            this.btnSendVerifCode = new Wisej.Web.Button();
            this.pnlSt1Tryanother = new Wisej.Web.Panel();
            this.lblCapclose = new Wisej.Web.Label();
            this.label3 = new Wisej.Web.Label();
            this.line2 = new Wisej.Web.Line();
            this.txtCAPMobile = new Wisej.Web.TextBox();
            this.txtCAPemail = new Wisej.Web.TextBox();
            this.panel2 = new Wisej.Web.Panel();
            this.lblCapemailmoblmsg = new Wisej.Web.Label();
            this.lnkST1Try = new Wisej.Web.LinkLabel();
            this.txtCAPUserName = new Wisej.Web.TextBox();
            this.lblhead = new Wisej.Web.Label();
            this.pnlLogo = new Wisej.Web.Panel();
            this.pictureBox1 = new Wisej.Web.PictureBox();
            this.timer1 = new Wisej.Web.Timer(this.components);
            this.panel1.SuspendLayout();
            this.pnlStep3.SuspendLayout();
            this.pnlStep2.SuspendLayout();
            this.pnlStep1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.pnlSt1Tryanother.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSetpBack1);
            this.panel1.Controls.Add(this.pnlStep3);
            this.panel1.Controls.Add(this.pnlStep2);
            this.panel1.Controls.Add(this.pnlStep1);
            this.panel1.Controls.Add(this.pnlLogo);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(0, 20, 0, 0);
            this.panel1.Size = new System.Drawing.Size(650, 789);
            this.panel1.TabIndex = 0;
            // 
            // btnSetpBack1
            // 
            this.btnSetpBack1.AppearanceKey = "button-error";
            this.btnSetpBack1.Location = new System.Drawing.Point(621, 0);
            this.btnSetpBack1.Name = "btnSetpBack1";
            this.btnSetpBack1.Size = new System.Drawing.Size(27, 26);
            this.btnSetpBack1.TabIndex = 1;
            this.btnSetpBack1.Text = "X";
            this.btnSetpBack1.Click += new System.EventHandler(this.btnSetpBack1_Click);
            // 
            // pnlStep3
            // 
            this.pnlStep3.Controls.Add(this.lblNewPassword);
            this.pnlStep3.Controls.Add(this.lblConfirmPassword);
            this.pnlStep3.Controls.Add(this.btnSavePassword);
            this.pnlStep3.Controls.Add(this.label5);
            this.pnlStep3.Controls.Add(this.txtConfirmPassword);
            this.pnlStep3.Controls.Add(this.txtNewPassword);
            this.pnlStep3.Dock = Wisej.Web.DockStyle.Top;
            this.pnlStep3.Location = new System.Drawing.Point(0, 616);
            this.pnlStep3.Name = "pnlStep3";
            this.pnlStep3.Size = new System.Drawing.Size(650, 173);
            this.pnlStep3.TabIndex = 47;
            this.pnlStep3.Visible = false;
            // 
            // lblNewPassword
            // 
            this.lblNewPassword.AutoSize = true;
            this.lblNewPassword.Location = new System.Drawing.Point(135, 65);
            this.lblNewPassword.Name = "lblNewPassword";
            this.lblNewPassword.Size = new System.Drawing.Size(88, 14);
            this.lblNewPassword.TabIndex = 8;
            this.lblNewPassword.Text = "New Password:";
            // 
            // lblConfirmPassword
            // 
            this.lblConfirmPassword.Location = new System.Drawing.Point(135, 95);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(110, 22);
            this.lblConfirmPassword.TabIndex = 9;
            this.lblConfirmPassword.Text = "Confirm Password:";
            // 
            // btnSavePassword
            // 
            this.btnSavePassword.Location = new System.Drawing.Point(292, 125);
            this.btnSavePassword.Name = "btnSavePassword";
            this.btnSavePassword.Size = new System.Drawing.Size(115, 27);
            this.btnSavePassword.TabIndex = 7;
            this.btnSavePassword.Text = "Save Password";
            this.btnSavePassword.Click += new System.EventHandler(this.btnSavePassword_Click);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("default", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label5.ForeColor = System.Drawing.Color.FromName("@buttonHighlight");
            this.label5.Location = new System.Drawing.Point(290, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(174, 27);
            this.label5.TabIndex = 6;
            this.label5.Text = "Create New Password";
            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.AutoSize = false;
            this.txtConfirmPassword.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtConfirmPassword.Location = new System.Drawing.Point(249, 92);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.Size = new System.Drawing.Size(233, 27);
            this.txtConfirmPassword.TabIndex = 5;
            this.txtConfirmPassword.TextAlign = Wisej.Web.HorizontalAlignment.Center;
            this.txtConfirmPassword.Watermark = "* * * *";
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.AutoSize = false;
            this.txtNewPassword.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtNewPassword.Location = new System.Drawing.Point(249, 59);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.Size = new System.Drawing.Size(233, 27);
            this.txtNewPassword.TabIndex = 4;
            this.txtNewPassword.TextAlign = Wisej.Web.HorizontalAlignment.Center;
            this.txtNewPassword.Watermark = "* * * *";
            this.txtNewPassword.Leave += new System.EventHandler(this.txtNewPassword_Leave);
            // 
            // pnlStep2
            // 
            this.pnlStep2.Controls.Add(this.lblEntertext);
            this.pnlStep2.Controls.Add(this.linkresend);
            this.pnlStep2.Controls.Add(this.lblTimerLeft);
            this.pnlStep2.Controls.Add(this.btnValidCaptcher);
            this.pnlStep2.Controls.Add(this.txtverifytext);
            this.pnlStep2.Controls.Add(this.lblOnetime2);
            this.pnlStep2.Dock = Wisej.Web.DockStyle.Top;
            this.pnlStep2.Location = new System.Drawing.Point(0, 440);
            this.pnlStep2.Name = "pnlStep2";
            this.pnlStep2.Size = new System.Drawing.Size(650, 176);
            this.pnlStep2.TabIndex = 46;
            this.pnlStep2.Visible = false;
            // 
            // lblEntertext
            // 
            this.lblEntertext.AutoSize = true;
            this.lblEntertext.Font = new System.Drawing.Font("@loginText", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblEntertext.Location = new System.Drawing.Point(169, 89);
            this.lblEntertext.Name = "lblEntertext";
            this.lblEntertext.Size = new System.Drawing.Size(60, 14);
            this.lblEntertext.TabIndex = 52;
            this.lblEntertext.Text = "Enter Text";
            this.lblEntertext.Visible = false;
            // 
            // linkresend
            // 
            this.linkresend.AutoSize = true;
            this.linkresend.BackColor = System.Drawing.Color.Transparent;
            this.linkresend.Font = new System.Drawing.Font("@loginText", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.linkresend.ForeColor = System.Drawing.Color.Blue;
            this.linkresend.LinkColor = System.Drawing.Color.Blue;
            this.linkresend.Location = new System.Drawing.Point(452, 122);
            this.linkresend.MinimumSize = new System.Drawing.Size(0, 20);
            this.linkresend.Name = "linkresend";
            this.linkresend.Size = new System.Drawing.Size(72, 20);
            this.linkresend.TabIndex = 50;
            this.linkresend.Text = "Resend Text";
            this.linkresend.Visible = false;
            this.linkresend.LinkClicked += new Wisej.Web.LinkLabelLinkClickedEventHandler(this.linkresend_LinkClicked);
            // 
            // lblTimerLeft
            // 
            this.lblTimerLeft.ForeColor = System.Drawing.Color.FromArgb(15, 109, 180);
            this.lblTimerLeft.Location = new System.Drawing.Point(451, 90);
            this.lblTimerLeft.Name = "lblTimerLeft";
            this.lblTimerLeft.Size = new System.Drawing.Size(96, 18);
            this.lblTimerLeft.TabIndex = 51;
            this.lblTimerLeft.Text = "(Valid for 2 mins)";
            this.lblTimerLeft.Visible = false;
            // 
            // btnValidCaptcher
            // 
            this.btnValidCaptcher.Location = new System.Drawing.Point(268, 125);
            this.btnValidCaptcher.Name = "btnValidCaptcher";
            this.btnValidCaptcher.Size = new System.Drawing.Size(115, 27);
            this.btnValidCaptcher.TabIndex = 4;
            this.btnValidCaptcher.Text = "Submit";
            this.btnValidCaptcher.Click += new System.EventHandler(this.btnValidCaptcher_Click);
            // 
            // txtverifytext
            // 
            this.txtverifytext.AutoSize = false;
            this.txtverifytext.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtverifytext.CssStyle = "letter-spacing: 8px; word-spacing: 16px;";
            this.txtverifytext.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtverifytext.Location = new System.Drawing.Point(235, 84);
            this.txtverifytext.MaxLength = 6;
            this.txtverifytext.Name = "txtverifytext";
            this.txtverifytext.Size = new System.Drawing.Size(208, 27);
            this.txtverifytext.TabIndex = 3;
            this.txtverifytext.TextAlign = Wisej.Web.HorizontalAlignment.Center;
            this.txtverifytext.Watermark = "* * * * * *";
            // 
            // lblOnetime2
            // 
            this.lblOnetime2.AllowHtml = true;
            this.lblOnetime2.Dock = Wisej.Web.DockStyle.Top;
            this.lblOnetime2.Font = new System.Drawing.Font("default", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblOnetime2.ForeColor = System.Drawing.Color.FromName("@captainBlue");
            this.lblOnetime2.Location = new System.Drawing.Point(0, 0);
            this.lblOnetime2.Name = "lblOnetime2";
            this.lblOnetime2.Padding = new Wisej.Web.Padding(120, 5, 120, 5);
            this.lblOnetime2.Size = new System.Drawing.Size(650, 70);
            this.lblOnetime2.TabIndex = 1;
            this.lblOnetime2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // pnlStep1
            // 
            this.pnlStep1.Controls.Add(this.panel4);
            this.pnlStep1.Controls.Add(this.pnlSt1Tryanother);
            this.pnlStep1.Controls.Add(this.panel2);
            this.pnlStep1.Controls.Add(this.lblhead);
            this.pnlStep1.Dock = Wisej.Web.DockStyle.Top;
            this.pnlStep1.Location = new System.Drawing.Point(0, 131);
            this.pnlStep1.Name = "pnlStep1";
            this.pnlStep1.Size = new System.Drawing.Size(650, 309);
            this.pnlStep1.TabIndex = 45;
            this.pnlStep1.Visible = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnSendVerifCode);
            this.panel4.Dock = Wisej.Web.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 264);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(650, 42);
            this.panel4.TabIndex = 6;
            // 
            // btnSendVerifCode
            // 
            this.btnSendVerifCode.Location = new System.Drawing.Point(204, 3);
            this.btnSendVerifCode.Name = "btnSendVerifCode";
            this.btnSendVerifCode.Size = new System.Drawing.Size(223, 26);
            this.btnSendVerifCode.TabIndex = 0;
            this.btnSendVerifCode.Text = "Send Verification Code";
            this.btnSendVerifCode.Click += new System.EventHandler(this.btnSendVerifCode_Click);
            // 
            // pnlSt1Tryanother
            // 
            this.pnlSt1Tryanother.Controls.Add(this.lblCapclose);
            this.pnlSt1Tryanother.Controls.Add(this.label3);
            this.pnlSt1Tryanother.Controls.Add(this.line2);
            this.pnlSt1Tryanother.Controls.Add(this.txtCAPMobile);
            this.pnlSt1Tryanother.Controls.Add(this.txtCAPemail);
            this.pnlSt1Tryanother.Dock = Wisej.Web.DockStyle.Top;
            this.pnlSt1Tryanother.Location = new System.Drawing.Point(0, 162);
            this.pnlSt1Tryanother.Name = "pnlSt1Tryanother";
            this.pnlSt1Tryanother.Size = new System.Drawing.Size(650, 102);
            this.pnlSt1Tryanother.TabIndex = 5;
            this.pnlSt1Tryanother.Visible = false;
            // 
            // lblCapclose
            // 
            this.lblCapclose.AllowHtml = true;
            this.lblCapclose.AutoSize = true;
            this.lblCapclose.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.lblCapclose.CssStyle = "border:1px solid #ccc; cursor:hand; ";
            this.lblCapclose.ForeColor = System.Drawing.Color.FromArgb(103, 103, 103);
            this.lblCapclose.Location = new System.Drawing.Point(516, 3);
            this.lblCapclose.Margin = new Wisej.Web.Padding(3, 0, 3, 3);
            this.lblCapclose.Name = "lblCapclose";
            this.lblCapclose.Padding = new Wisej.Web.Padding(2, 0, 2, 2);
            this.lblCapclose.Size = new System.Drawing.Size(16, 18);
            this.lblCapclose.TabIndex = 6;
            this.lblCapclose.Text = "x";
            this.lblCapclose.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblCapclose.ToolTipText = "Close Try another way block";
            this.lblCapclose.Click += new System.EventHandler(this.lblCapclose_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.label3.Location = new System.Drawing.Point(308, 39);
            this.label3.Name = "label3";
            this.label3.Padding = new Wisej.Web.Padding(8, 0, 8, 0);
            this.label3.Size = new System.Drawing.Size(38, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "OR";
            // 
            // line2
            // 
            this.line2.LineColor = System.Drawing.Color.FromArgb(183, 183, 183);
            this.line2.LineSize = 1;
            this.line2.LineStyle = Wisej.Web.LineStyle.Dotted;
            this.line2.Location = new System.Drawing.Point(140, 43);
            this.line2.Name = "line2";
            this.line2.Size = new System.Drawing.Size(350, 10);
            // 
            // txtCAPMobile
            // 
            this.txtCAPMobile.AutoSize = false;
            this.txtCAPMobile.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtCAPMobile.Location = new System.Drawing.Point(225, 63);
            this.txtCAPMobile.Name = "txtCAPMobile";
            this.txtCAPMobile.Size = new System.Drawing.Size(180, 30);
            this.txtCAPMobile.TabIndex = 2;
            this.txtCAPMobile.TextAlign = Wisej.Web.HorizontalAlignment.Center;
            this.txtCAPMobile.Watermark = "@Phone Number";
            // 
            // txtCAPemail
            // 
            this.txtCAPemail.AutoSize = false;
            this.txtCAPemail.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtCAPemail.Location = new System.Drawing.Point(154, 3);
            this.txtCAPemail.Name = "txtCAPemail";
            this.txtCAPemail.Size = new System.Drawing.Size(322, 30);
            this.txtCAPemail.TabIndex = 1;
            this.txtCAPemail.TextAlign = Wisej.Web.HorizontalAlignment.Center;
            this.txtCAPemail.Watermark = "@Email";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblCapemailmoblmsg);
            this.panel2.Controls.Add(this.lnkST1Try);
            this.panel2.Controls.Add(this.txtCAPUserName);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 66);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(650, 96);
            this.panel2.TabIndex = 2;
            // 
            // lblCapemailmoblmsg
            // 
            this.lblCapemailmoblmsg.ForeColor = System.Drawing.Color.FromArgb(79, 166, 55);
            this.lblCapemailmoblmsg.Location = new System.Drawing.Point(84, 44);
            this.lblCapemailmoblmsg.Name = "lblCapemailmoblmsg";
            this.lblCapemailmoblmsg.Size = new System.Drawing.Size(482, 20);
            this.lblCapemailmoblmsg.TabIndex = 52;
            this.lblCapemailmoblmsg.Text = "-";
            this.lblCapemailmoblmsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lnkST1Try
            // 
            this.lnkST1Try.BackColor = System.Drawing.Color.Transparent;
            this.lnkST1Try.Font = new System.Drawing.Font("@loginText", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lnkST1Try.ForeColor = System.Drawing.Color.Blue;
            this.lnkST1Try.LinkColor = System.Drawing.Color.Blue;
            this.lnkST1Try.Location = new System.Drawing.Point(84, 70);
            this.lnkST1Try.MinimumSize = new System.Drawing.Size(0, 20);
            this.lnkST1Try.Name = "lnkST1Try";
            this.lnkST1Try.Size = new System.Drawing.Size(482, 20);
            this.lnkST1Try.TabIndex = 51;
            this.lnkST1Try.Text = "Try another way to send verfication code";
            this.lnkST1Try.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkST1Try.LinkClicked += new Wisej.Web.LinkLabelLinkClickedEventHandler(this.lnkST1Try_LinkClicked);
            // 
            // txtCAPUserName
            // 
            this.txtCAPUserName.AutoSize = false;
            this.txtCAPUserName.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtCAPUserName.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtCAPUserName.Location = new System.Drawing.Point(225, 10);
            this.txtCAPUserName.Name = "txtCAPUserName";
            this.txtCAPUserName.Size = new System.Drawing.Size(180, 30);
            this.txtCAPUserName.TabIndex = 1;
            this.txtCAPUserName.TextAlign = Wisej.Web.HorizontalAlignment.Center;
            this.txtCAPUserName.Watermark = "@UserID";
            this.txtCAPUserName.KeyPress += new Wisej.Web.KeyPressEventHandler(this.txtCAPUserName_KeyPress);
            // 
            // lblhead
            // 
            this.lblhead.Dock = Wisej.Web.DockStyle.Top;
            this.lblhead.Font = new System.Drawing.Font("default", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblhead.ForeColor = System.Drawing.Color.FromName("@captainBlue");
            this.lblhead.Location = new System.Drawing.Point(0, 0);
            this.lblhead.Name = "lblhead";
            this.lblhead.Padding = new Wisej.Web.Padding(80, 5, 80, 0);
            this.lblhead.Size = new System.Drawing.Size(650, 66);
            this.lblhead.TabIndex = 0;
            this.lblhead.Text = "Please enter your CAPTAIN Username, we will send a verification code to your regi" +
    "stered Email or Mobile Number";
            this.lblhead.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // pnlLogo
            // 
            this.pnlLogo.Controls.Add(this.pictureBox1);
            this.pnlLogo.Dock = Wisej.Web.DockStyle.Top;
            this.pnlLogo.Location = new System.Drawing.Point(0, 20);
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(650, 111);
            this.pnlLogo.TabIndex = 44;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = Wisej.Web.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(650, 111);
            this.pictureBox1.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ForgotPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(650, 789);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.None;
            this.Name = "ForgotPassword";
            this.Text = "ForgotPassword";
            this.panel1.ResumeLayout(false);
            this.pnlStep3.ResumeLayout(false);
            this.pnlStep3.PerformLayout();
            this.pnlStep2.ResumeLayout(false);
            this.pnlStep2.PerformLayout();
            this.pnlStep1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.pnlSt1Tryanother.ResumeLayout(false);
            this.pnlSt1Tryanother.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.pnlLogo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Panel panel1;
        private Wisej.Web.Panel pnlLogo;
        private Wisej.Web.PictureBox pictureBox1;
        private Wisej.Web.Panel pnlStep3;
        private Wisej.Web.Panel pnlStep2;
        private Wisej.Web.Panel pnlStep1;
        private Wisej.Web.Label lblhead;
        private Wisej.Web.TextBox txtCAPUserName;
        private Wisej.Web.Panel panel2;
        private Wisej.Web.Panel panel4;
        private Wisej.Web.Button btnSetpBack1;
        private Wisej.Web.Button btnSendVerifCode;
        private Wisej.Web.Panel pnlSt1Tryanother;
        private Wisej.Web.TextBox txtCAPMobile;
        private Wisej.Web.TextBox txtCAPemail;
        private Wisej.Web.TextBox txtverifytext;
        private Wisej.Web.Label lblOnetime2;
        private Wisej.Web.Button btnValidCaptcher;
        private Wisej.Web.Button btnSavePassword;
        private Wisej.Web.Label label5;
        private Wisej.Web.TextBox txtConfirmPassword;
        private Wisej.Web.TextBox txtNewPassword;
        private Wisej.Web.LinkLabel linkresend;
        private Wisej.Web.Label lblTimerLeft;
        private Wisej.Web.Timer timer1;
        private Wisej.Web.Label lblEntertext;
        private Wisej.Web.Label lblNewPassword;
        private Wisej.Web.Label lblConfirmPassword;
        private Wisej.Web.Label label3;
        private Wisej.Web.Line line2;
        private Wisej.Web.LinkLabel lnkST1Try;
        private Wisej.Web.Label lblCapemailmoblmsg;
        private Wisej.Web.Label lblCapclose;
    }
}