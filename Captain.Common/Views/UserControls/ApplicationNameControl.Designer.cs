using Captain.Common.Views.Controls.Compatibility;
using System;
using Wisej.Web;


namespace Captain.Common.Views.UserControls
{
    partial class ApplicationNameControl
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

        #region Wisej UserControl Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new Wisej.Web.Panel();
            this.panelInfo = new Wisej.Web.Panel();
            this.panel3 = new Wisej.Web.Panel();
            this.labelPhone = new Wisej.Web.Label();
            this.labelAddress = new Wisej.Web.Label();
            this.panel2 = new Wisej.Web.Panel();
            this.pictureBox1 = new Wisej.Web.PictureBox();
            this.line2 = new Wisej.Web.Line();
            this.panel4 = new Wisej.Web.Panel();
            this.lblMsg = new Wisej.Web.Label();
            this.panelNavButtons = new Wisej.Web.Panel();
            this.btnAdvance = new Wisej.Web.Button();
            this.spacer7 = new Wisej.Web.Spacer();
            this.Btn_First = new Wisej.Web.Button();
            this.spacer6 = new Wisej.Web.Spacer();
            this.BtnP10 = new Wisej.Web.Button();
            this.spacer5 = new Wisej.Web.Spacer();
            this.BtnPrev = new Wisej.Web.Button();
            this.spacer4 = new Wisej.Web.Spacer();
            this.BtnNxt = new Wisej.Web.Button();
            this.spacer3 = new Wisej.Web.Spacer();
            this.BtnN10 = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.spacer2 = new Wisej.Web.Spacer();
            this.line1 = new Wisej.Web.Line();
            this.BtnLast = new Wisej.Web.Button();
            this.spacer9 = new Wisej.Web.Spacer();
            this.line3 = new Wisej.Web.Line();
            this.panelApplication = new Wisej.Web.Panel();
            this.lblApplicationName = new Wisej.Web.Label();
            this.spacer8 = new Wisej.Web.Spacer();
            this.pnlApptextbox = new Wisej.Web.Panel();
            this.txtAppNo = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblAppHeader = new Wisej.Web.Label();
            this.spacer10 = new Wisej.Web.Spacer();
            this.panel1.SuspendLayout();
            this.panelInfo.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel4.SuspendLayout();
            this.panelNavButtons.SuspendLayout();
            this.panelApplication.SuspendLayout();
            this.pnlApptextbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.panelInfo);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panelNavButtons);
            this.panel1.Controls.Add(this.panelApplication);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1002, 29);
            this.panel1.TabIndex = 0;
            this.panel1.TabStop = true;
            // 
            // panelInfo
            // 
            this.panelInfo.Controls.Add(this.panel3);
            this.panelInfo.Controls.Add(this.panel2);
            this.panelInfo.Controls.Add(this.line2);
            this.panelInfo.CssStyle = "transition:300ms linear;";
            this.panelInfo.Dock = Wisej.Web.DockStyle.Fill;
            this.panelInfo.Location = new System.Drawing.Point(327, 0);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(123, 29);
            this.panelInfo.TabIndex = 19;
            this.panelInfo.TabStop = true;
            this.panelInfo.Visible = false;
            this.panelInfo.Resize += new System.EventHandler(this.panelInfo_Resize);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labelPhone);
            this.panel3.Controls.Add(this.labelAddress);
            this.panel3.Dock = Wisej.Web.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(18, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(105, 29);
            this.panel3.TabIndex = 18;
            this.panel3.Text = "`";
            // 
            // labelPhone
            // 
            this.labelPhone.AllowHtml = true;
            this.labelPhone.AutoEllipsis = true;
            this.labelPhone.Dock = Wisej.Web.DockStyle.Top;
            this.labelPhone.Font = new System.Drawing.Font("default", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.labelPhone.Location = new System.Drawing.Point(0, 14);
            this.labelPhone.Name = "labelPhone";
            this.labelPhone.Size = new System.Drawing.Size(105, 14);
            this.labelPhone.TabIndex = 16;
            this.labelPhone.Text = "...";
            // 
            // labelAddress
            // 
            this.labelAddress.AutoEllipsis = true;
            this.labelAddress.Dock = Wisej.Web.DockStyle.Top;
            this.labelAddress.Font = new System.Drawing.Font("default", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.labelAddress.Location = new System.Drawing.Point(0, 0);
            this.labelAddress.Margin = new Wisej.Web.Padding(0);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(105, 14);
            this.labelAddress.TabIndex = 13;
            this.labelAddress.TextChanged += new System.EventHandler(this.labelAddress_TextChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = Wisej.Web.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(3, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(15, 29);
            this.panel2.TabIndex = 17;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = Wisej.Web.DockStyle.Top;
            this.pictureBox1.ImageSource = "contact-info?color=#496C91";
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(15, 15);
            this.pictureBox1.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            // 
            // line2
            // 
            this.line2.Dock = Wisej.Web.DockStyle.Left;
            this.line2.Location = new System.Drawing.Point(0, 0);
            this.line2.Name = "line2";
            this.line2.Orientation = Wisej.Web.Orientation.Vertical;
            this.line2.Size = new System.Drawing.Size(3, 29);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.lblMsg);
            this.panel4.Dock = Wisej.Web.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(450, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(324, 29);
            this.panel4.TabIndex = 21;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Dock = Wisej.Web.DockStyle.Fill;
            this.lblMsg.Font = new System.Drawing.Font("default", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(0, 0);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(324, 29);
            this.lblMsg.TabIndex = 0;
            this.lblMsg.Text = "label1";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMsg.Visible = false;
            // 
            // panelNavButtons
            // 
            this.panelNavButtons.Controls.Add(this.btnAdvance);
            this.panelNavButtons.Controls.Add(this.spacer7);
            this.panelNavButtons.Controls.Add(this.Btn_First);
            this.panelNavButtons.Controls.Add(this.spacer6);
            this.panelNavButtons.Controls.Add(this.BtnP10);
            this.panelNavButtons.Controls.Add(this.spacer5);
            this.panelNavButtons.Controls.Add(this.BtnPrev);
            this.panelNavButtons.Controls.Add(this.spacer4);
            this.panelNavButtons.Controls.Add(this.BtnNxt);
            this.panelNavButtons.Controls.Add(this.spacer3);
            this.panelNavButtons.Controls.Add(this.BtnN10);
            this.panelNavButtons.Controls.Add(this.spacer1);
            this.panelNavButtons.Controls.Add(this.spacer2);
            this.panelNavButtons.Controls.Add(this.line1);
            this.panelNavButtons.Controls.Add(this.BtnLast);
            this.panelNavButtons.Controls.Add(this.spacer9);
            this.panelNavButtons.Controls.Add(this.line3);
            this.panelNavButtons.Dock = Wisej.Web.DockStyle.Right;
            this.panelNavButtons.Location = new System.Drawing.Point(774, 0);
            this.panelNavButtons.Name = "panelNavButtons";
            this.panelNavButtons.Padding = new Wisej.Web.Padding(5, 3, 0, 3);
            this.panelNavButtons.Size = new System.Drawing.Size(228, 29);
            this.panelNavButtons.TabIndex = 20;
            this.panelNavButtons.TabStop = true;
            // 
            // btnAdvance
            // 
            this.btnAdvance.AppearanceKey = "buttonNav";
            this.btnAdvance.Display = Wisej.Web.Display.Icon;
            this.btnAdvance.Dock = Wisej.Web.DockStyle.Right;
            this.btnAdvance.ImageSource = "captain-more?color=white";
            this.btnAdvance.Location = new System.Drawing.Point(32, 3);
            this.btnAdvance.Name = "btnAdvance";
            this.btnAdvance.Size = new System.Drawing.Size(23, 23);
            this.btnAdvance.TabIndex = 3;
            this.btnAdvance.TextImageRelation = Wisej.Web.TextImageRelation.Overlay;
            this.btnAdvance.ToolTipText = "Advanced Mainmenu Search";
            this.btnAdvance.Click += new System.EventHandler(this.btnAdvance_Click);
            // 
            // spacer7
            // 
            this.spacer7.Dock = Wisej.Web.DockStyle.Right;
            this.spacer7.Location = new System.Drawing.Point(55, 3);
            this.spacer7.Name = "spacer7";
            this.spacer7.Size = new System.Drawing.Size(15, 23);
            // 
            // Btn_First
            // 
            this.Btn_First.AppearanceKey = "buttonNav";
            this.Btn_First.BorderStyle = Wisej.Web.BorderStyle.None;
            this.Btn_First.Display = Wisej.Web.Display.Icon;
            this.Btn_First.Dock = Wisej.Web.DockStyle.Right;
            this.Btn_First.ImageSource = "captain-first?color=white";
            this.Btn_First.Location = new System.Drawing.Point(70, 3);
            this.Btn_First.Name = "Btn_First";
            this.Btn_First.Size = new System.Drawing.Size(26, 23);
            this.Btn_First.TabIndex = 4;
            this.Btn_First.Text = "&f";
            this.Btn_First.TextImageRelation = Wisej.Web.TextImageRelation.Overlay;
            this.Btn_First.ToolTipText = "First Record";
            this.Btn_First.Click += new System.EventHandler(this.Navigation_Click);
            // 
            // spacer6
            // 
            this.spacer6.Dock = Wisej.Web.DockStyle.Right;
            this.spacer6.Location = new System.Drawing.Point(96, 3);
            this.spacer6.Name = "spacer6";
            this.spacer6.Size = new System.Drawing.Size(1, 23);
            // 
            // BtnP10
            // 
            this.BtnP10.AppearanceKey = "buttonNav";
            this.BtnP10.BorderStyle = Wisej.Web.BorderStyle.None;
            this.BtnP10.Display = Wisej.Web.Display.Icon;
            this.BtnP10.Dock = Wisej.Web.DockStyle.Right;
            this.BtnP10.ImageSource = "captain-backward?color=white";
            this.BtnP10.Location = new System.Drawing.Point(97, 3);
            this.BtnP10.Name = "BtnP10";
            this.BtnP10.Size = new System.Drawing.Size(23, 23);
            this.BtnP10.TabIndex = 5;
            this.BtnP10.TextImageRelation = Wisej.Web.TextImageRelation.Overlay;
            this.BtnP10.ToolTipText = "10 Recs Backward";
            this.BtnP10.Click += new System.EventHandler(this.Navigation_Click);
            // 
            // spacer5
            // 
            this.spacer5.Dock = Wisej.Web.DockStyle.Right;
            this.spacer5.Location = new System.Drawing.Point(120, 3);
            this.spacer5.Name = "spacer5";
            this.spacer5.Size = new System.Drawing.Size(1, 23);
            // 
            // BtnPrev
            // 
            this.BtnPrev.AppearanceKey = "buttonNav";
            this.BtnPrev.BorderStyle = Wisej.Web.BorderStyle.None;
            this.BtnPrev.Display = Wisej.Web.Display.Icon;
            this.BtnPrev.Dock = Wisej.Web.DockStyle.Right;
            this.BtnPrev.ImageSource = "captain-previous?color=white";
            this.BtnPrev.Location = new System.Drawing.Point(121, 3);
            this.BtnPrev.Name = "BtnPrev";
            this.BtnPrev.Size = new System.Drawing.Size(23, 23);
            this.BtnPrev.TabIndex = 6;
            this.BtnPrev.Text = "&p";
            this.BtnPrev.TextImageRelation = Wisej.Web.TextImageRelation.Overlay;
            this.BtnPrev.ToolTipText = "Previous Record";
            this.BtnPrev.Click += new System.EventHandler(this.Navigation_Click);
            // 
            // spacer4
            // 
            this.spacer4.Dock = Wisej.Web.DockStyle.Right;
            this.spacer4.Location = new System.Drawing.Point(144, 3);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(1, 23);
            // 
            // BtnNxt
            // 
            this.BtnNxt.AppearanceKey = "buttonNav";
            this.BtnNxt.BorderStyle = Wisej.Web.BorderStyle.None;
            this.BtnNxt.Display = Wisej.Web.Display.Icon;
            this.BtnNxt.Dock = Wisej.Web.DockStyle.Right;
            this.BtnNxt.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnNxt.ImageSource = "captain-next?color=white";
            this.BtnNxt.Location = new System.Drawing.Point(145, 3);
            this.BtnNxt.Name = "BtnNxt";
            this.BtnNxt.Size = new System.Drawing.Size(23, 23);
            this.BtnNxt.TabIndex = 7;
            this.BtnNxt.Text = "&n";
            this.BtnNxt.TextImageRelation = Wisej.Web.TextImageRelation.Overlay;
            this.BtnNxt.ToolTipText = "Next Record";
            this.BtnNxt.Click += new System.EventHandler(this.Navigation_Click);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Right;
            this.spacer3.Location = new System.Drawing.Point(168, 3);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(1, 23);
            // 
            // BtnN10
            // 
            this.BtnN10.AppearanceKey = "buttonNav";
            this.BtnN10.BorderStyle = Wisej.Web.BorderStyle.None;
            this.BtnN10.Display = Wisej.Web.Display.Icon;
            this.BtnN10.Dock = Wisej.Web.DockStyle.Right;
            this.BtnN10.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnN10.ImageSource = "captain-forward?color=white";
            this.BtnN10.Location = new System.Drawing.Point(169, 3);
            this.BtnN10.Name = "BtnN10";
            this.BtnN10.Size = new System.Drawing.Size(23, 23);
            this.BtnN10.TabIndex = 8;
            this.BtnN10.TextImageRelation = Wisej.Web.TextImageRelation.Overlay;
            this.BtnN10.ToolTipText = "10 Recs Forward";
            this.BtnN10.Click += new System.EventHandler(this.Navigation_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(192, 3);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(1, 23);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Right;
            this.spacer2.Location = new System.Drawing.Point(193, 3);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(1, 23);
            // 
            // line1
            // 
            this.line1.Dock = Wisej.Web.DockStyle.Left;
            this.line1.Location = new System.Drawing.Point(5, 3);
            this.line1.Name = "line1";
            this.line1.Orientation = Wisej.Web.Orientation.Vertical;
            this.line1.Size = new System.Drawing.Size(3, 23);
            // 
            // BtnLast
            // 
            this.BtnLast.AppearanceKey = "buttonNav";
            this.BtnLast.BorderStyle = Wisej.Web.BorderStyle.None;
            this.BtnLast.Display = Wisej.Web.Display.Icon;
            this.BtnLast.Dock = Wisej.Web.DockStyle.Right;
            this.BtnLast.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnLast.ImageSource = "captain-last?color=white";
            this.BtnLast.Location = new System.Drawing.Point(194, 3);
            this.BtnLast.Name = "BtnLast";
            this.BtnLast.Size = new System.Drawing.Size(23, 23);
            this.BtnLast.TabIndex = 9;
            this.BtnLast.Text = "&l";
            this.BtnLast.TextImageRelation = Wisej.Web.TextImageRelation.Overlay;
            this.BtnLast.ToolTipText = "Last Record";
            this.BtnLast.Click += new System.EventHandler(this.Navigation_Click);
            // 
            // spacer9
            // 
            this.spacer9.Dock = Wisej.Web.DockStyle.Right;
            this.spacer9.Location = new System.Drawing.Point(217, 3);
            this.spacer9.Name = "spacer9";
            this.spacer9.Size = new System.Drawing.Size(8, 23);
            // 
            // line3
            // 
            this.line3.Dock = Wisej.Web.DockStyle.Right;
            this.line3.Location = new System.Drawing.Point(225, 3);
            this.line3.Name = "line3";
            this.line3.Orientation = Wisej.Web.Orientation.Vertical;
            this.line3.Size = new System.Drawing.Size(3, 23);
            // 
            // panelApplication
            // 
            this.panelApplication.AutoSize = true;
            this.panelApplication.Controls.Add(this.lblApplicationName);
            this.panelApplication.Controls.Add(this.spacer8);
            this.panelApplication.Controls.Add(this.pnlApptextbox);
            this.panelApplication.Controls.Add(this.lblAppHeader);
            this.panelApplication.Controls.Add(this.spacer10);
            this.panelApplication.Dock = Wisej.Web.DockStyle.Left;
            this.panelApplication.Location = new System.Drawing.Point(0, 0);
            this.panelApplication.Name = "panelApplication";
            this.panelApplication.Size = new System.Drawing.Size(327, 29);
            this.panelApplication.TabIndex = 18;
            this.panelApplication.TabStop = true;
            // 
            // lblApplicationName
            // 
            this.lblApplicationName.AutoEllipsis = true;
            this.lblApplicationName.Dock = Wisej.Web.DockStyle.Left;
            this.lblApplicationName.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblApplicationName.ForeColor = System.Drawing.Color.FromName("@captainBlue");
            this.lblApplicationName.Location = new System.Drawing.Point(133, 0);
            this.lblApplicationName.Margin = new Wisej.Web.Padding(3, 3, 5, 3);
            this.lblApplicationName.Name = "lblApplicationName";
            this.lblApplicationName.Padding = new Wisej.Web.Padding(0, 2, 0, 5);
            this.lblApplicationName.Size = new System.Drawing.Size(194, 29);
            this.lblApplicationName.TabIndex = 0;
            this.lblApplicationName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // spacer8
            // 
            this.spacer8.Dock = Wisej.Web.DockStyle.Left;
            this.spacer8.Location = new System.Drawing.Point(125, 0);
            this.spacer8.Name = "spacer8";
            this.spacer8.Size = new System.Drawing.Size(8, 29);
            // 
            // pnlApptextbox
            // 
            this.pnlApptextbox.Controls.Add(this.txtAppNo);
            this.pnlApptextbox.Dock = Wisej.Web.DockStyle.Left;
            this.pnlApptextbox.Location = new System.Drawing.Point(50, 0);
            this.pnlApptextbox.Name = "pnlApptextbox";
            this.pnlApptextbox.Padding = new Wisej.Web.Padding(0, 2, 0, 2);
            this.pnlApptextbox.Size = new System.Drawing.Size(75, 29);
            this.pnlApptextbox.TabIndex = 5;
            // 
            // txtAppNo
            // 
            this.txtAppNo.Dock = Wisej.Web.DockStyle.Fill;
            this.txtAppNo.Font = new System.Drawing.Font("default", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtAppNo.Location = new System.Drawing.Point(0, 2);
            this.txtAppNo.Margin = new Wisej.Web.Padding(3, 5, 3, 3);
            this.txtAppNo.Name = "txtAppNo";
            this.txtAppNo.Size = new System.Drawing.Size(75, 25);
            this.txtAppNo.TabIndex = 2;
            this.txtAppNo.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtAppNo.LostFocus += new System.EventHandler(this.txtAppNo_LostFocus);
            this.txtAppNo.KeyDown += new Wisej.Web.KeyEventHandler(this.txtAppNo_EnterKeyDown);
            // 
            // lblAppHeader
            // 
            this.lblAppHeader.Dock = Wisej.Web.DockStyle.Left;
            this.lblAppHeader.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblAppHeader.ForeColor = System.Drawing.Color.FromName("@captainBlue");
            this.lblAppHeader.Location = new System.Drawing.Point(16, 0);
            this.lblAppHeader.Name = "lblAppHeader";
            this.lblAppHeader.Size = new System.Drawing.Size(34, 29);
            this.lblAppHeader.TabIndex = 1;
            this.lblAppHeader.Text = "App#";
            this.lblAppHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spacer10
            // 
            this.spacer10.Dock = Wisej.Web.DockStyle.Left;
            this.spacer10.Location = new System.Drawing.Point(0, 0);
            this.spacer10.Name = "spacer10";
            this.spacer10.Size = new System.Drawing.Size(16, 29);
            // 
            // ApplicationNameControl
            // 
            this.Controls.Add(this.panel1);
            this.Name = "ApplicationNameControl";
            this.Padding = new Wisej.Web.Padding(3, 0, 3, 0);
            this.Size = new System.Drawing.Size(1008, 29);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelInfo.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panelNavButtons.ResumeLayout(false);
            this.panelApplication.ResumeLayout(false);
            this.pnlApptextbox.ResumeLayout(false);
            this.pnlApptextbox.PerformLayout();
            this.ResumeLayout(false);

        }

		#endregion

		private Panel panel1;
        public Label lblApplicationName;
        public Label lblAppHeader;
        private Button btnAdvance;
        public Button Btn_First;
        public Button BtnPrev;
        public Button BtnNxt;
        public Button BtnN10;
        public Button BtnLast;
        public Button BtnP10;
        public TextBoxWithValidation txtAppNo;
		private Line line2;
		private PictureBox pictureBox1;
		private Line line1;
		private Line line3;
		public Label labelAddress;
		public Label labelPhone;
		private Panel panelNavButtons;
		private Panel panelInfo;
		private Panel panelApplication;
        private Panel panel3;
        private Panel panel2;
        private Spacer spacer2;
        private Spacer spacer7;
        private Spacer spacer6;
        private Spacer spacer5;
        private Spacer spacer4;
        private Spacer spacer3;
        private Spacer spacer1;
        private Spacer spacer8;
        private Spacer spacer9;
        private Spacer spacer10;
        private Panel pnlApptextbox;
        private Panel panel4;
        public Label lblMsg;
    }
}