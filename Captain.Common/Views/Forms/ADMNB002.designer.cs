using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class ADMNB002
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ADMNB002));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.panel1 = new Wisej.Web.Panel();
            this.label4 = new Wisej.Web.Label();
            this.cmbReportfor = new Wisej.Web.ComboBox();
            this.Pb_Search_Hie = new Wisej.Web.PictureBox();
            this.txtHierchy = new Wisej.Web.TextBox();
            this.rdoUserAccess = new Wisej.Web.RadioButton();
            this.rdoUserstructure = new Wisej.Web.RadioButton();
            this.label3 = new Wisej.Web.Label();
            this.lblHierequired = new Wisej.Web.Label();
            this.label1 = new Wisej.Web.Label();
            this.CmbApp = new Wisej.Web.ComboBox();
            this.CmbUser = new Wisej.Web.ComboBox();
            this.LblApp = new Wisej.Web.Label();
            this.LblUser = new Wisej.Web.Label();
            this.btnPdfPrev = new Wisej.Web.Button();
            this.btnGenPdf = new Wisej.Web.Button();
            this.panel2 = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cmbReportfor);
            this.panel1.Controls.Add(this.Pb_Search_Hie);
            this.panel1.Controls.Add(this.txtHierchy);
            this.panel1.Controls.Add(this.rdoUserAccess);
            this.panel1.Controls.Add(this.rdoUserstructure);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lblHierequired);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.CmbApp);
            this.panel1.Controls.Add(this.CmbUser);
            this.panel1.Controls.Add(this.LblApp);
            this.panel1.Controls.Add(this.LblUser);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(482, 152);
            this.panel1.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label4.Location = new System.Drawing.Point(50, 110);
            this.label4.MinimumSize = new System.Drawing.Size(0, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "Report for";
            // 
            // cmbReportfor
            // 
            this.cmbReportfor.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbReportfor.FormattingEnabled = true;
            this.cmbReportfor.Location = new System.Drawing.Point(136, 106);
            this.cmbReportfor.Name = "cmbReportfor";
            this.cmbReportfor.Size = new System.Drawing.Size(244, 25);
            this.cmbReportfor.TabIndex = 6;
            // 
            // Pb_Search_Hie
            // 
            this.Pb_Search_Hie.Cursor = Wisej.Web.Cursors.Hand;
            this.Pb_Search_Hie.ImageSource = "captain-filter";
            this.Pb_Search_Hie.Location = new System.Drawing.Point(221, 78);
            this.Pb_Search_Hie.Name = "Pb_Search_Hie";
            this.Pb_Search_Hie.Size = new System.Drawing.Size(17, 20);
            this.Pb_Search_Hie.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.Pb_Search_Hie.Visible = false;
            this.Pb_Search_Hie.Click += new System.EventHandler(this.Pb_Search_Hie_Click);
            // 
            // txtHierchy
            // 
            this.txtHierchy.Anchor = Wisej.Web.AnchorStyles.Top;
            this.txtHierchy.Enabled = false;
            this.txtHierchy.Location = new System.Drawing.Point(136, 76);
            this.txtHierchy.Name = "txtHierchy";
            this.txtHierchy.ReadOnly = true;
            this.txtHierchy.Size = new System.Drawing.Size(81, 25);
            this.txtHierchy.TabIndex = 5;
            this.txtHierchy.Visible = false;
            // 
            // rdoUserAccess
            // 
            this.rdoUserAccess.Location = new System.Drawing.Point(283, 20);
            this.rdoUserAccess.Name = "rdoUserAccess";
            this.rdoUserAccess.Size = new System.Drawing.Size(105, 22);
            this.rdoUserAccess.TabIndex = 3;
            this.rdoUserAccess.Text = "User Access";
            this.rdoUserAccess.Click += new System.EventHandler(this.rdoUserAccess_Click);
            // 
            // rdoUserstructure
            // 
            this.rdoUserstructure.Checked = true;
            this.rdoUserstructure.Location = new System.Drawing.Point(136, 20);
            this.rdoUserstructure.Name = "rdoUserstructure";
            this.rdoUserstructure.Size = new System.Drawing.Size(147, 22);
            this.rdoUserstructure.TabIndex = 2;
            this.rdoUserstructure.TabStop = true;
            this.rdoUserstructure.Text = "User Tree Structure";
            this.rdoUserstructure.Click += new System.EventHandler(this.rdoUserAccess_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.Location = new System.Drawing.Point(50, 24);
            this.label3.MinimumSize = new System.Drawing.Size(0, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "Report Type";
            // 
            // lblHierequired
            // 
            this.lblHierequired.AutoSize = true;
            this.lblHierequired.ForeColor = System.Drawing.Color.Red;
            this.lblHierequired.Location = new System.Drawing.Point(114, 81);
            this.lblHierequired.Name = "lblHierequired";
            this.lblHierequired.Size = new System.Drawing.Size(10, 15);
            this.lblHierequired.TabIndex = 2;
            this.lblHierequired.Text = "*";
            this.lblHierequired.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(93, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "*";
            this.label1.Visible = false;
            // 
            // CmbApp
            // 
            this.CmbApp.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbApp.FormattingEnabled = true;
            this.CmbApp.Location = new System.Drawing.Point(136, 76);
            this.CmbApp.Name = "CmbApp";
            this.CmbApp.Size = new System.Drawing.Size(244, 25);
            this.CmbApp.TabIndex = 5;
            this.CmbApp.SelectedIndexChanged += new System.EventHandler(this.CmbApp_SelectedIndexChanged);
            // 
            // CmbUser
            // 
            this.CmbUser.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbUser.FormattingEnabled = true;
            this.CmbUser.Location = new System.Drawing.Point(136, 47);
            this.CmbUser.Name = "CmbUser";
            this.CmbUser.Size = new System.Drawing.Size(244, 25);
            this.CmbUser.TabIndex = 4;
            this.CmbUser.SelectedIndexChanged += new System.EventHandler(this.CmbUser_SelectedIndexChanged);
            // 
            // LblApp
            // 
            this.LblApp.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.LblApp.Location = new System.Drawing.Point(50, 82);
            this.LblApp.Name = "LblApp";
            this.LblApp.Size = new System.Drawing.Size(72, 17);
            this.LblApp.TabIndex = 0;
            this.LblApp.Text = "Application";
            // 
            // LblUser
            // 
            this.LblUser.AutoSize = true;
            this.LblUser.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.LblUser.Location = new System.Drawing.Point(50, 55);
            this.LblUser.Name = "LblUser";
            this.LblUser.Size = new System.Drawing.Size(49, 15);
            this.LblUser.TabIndex = 0;
            this.LblUser.Text = "User ID";
            // 
            // btnPdfPrev
            // 
            this.btnPdfPrev.AppearanceKey = "button-reports";
            this.btnPdfPrev.Dock = Wisej.Web.DockStyle.Right;
            this.btnPdfPrev.Location = new System.Drawing.Point(397, 5);
            this.btnPdfPrev.Name = "btnPdfPrev";
            this.btnPdfPrev.Size = new System.Drawing.Size(80, 25);
            this.btnPdfPrev.TabIndex = 2;
            this.btnPdfPrev.Text = "Pre&view";
            this.btnPdfPrev.Click += new System.EventHandler(this.BtnPdfPrev_Click);
            // 
            // btnGenPdf
            // 
            this.btnGenPdf.AppearanceKey = "button-reports";
            this.btnGenPdf.Dock = Wisej.Web.DockStyle.Right;
            this.btnGenPdf.Location = new System.Drawing.Point(312, 5);
            this.btnGenPdf.Name = "btnGenPdf";
            this.btnGenPdf.Size = new System.Drawing.Size(80, 25);
            this.btnGenPdf.TabIndex = 1;
            this.btnGenPdf.Text = "&Generate";
            this.btnGenPdf.Click += new System.EventHandler(this.BtnGenFile_Click);
            // 
            // panel2
            // 
            this.panel2.AppearanceKey = "panel-grdo";
            this.panel2.Controls.Add(this.btnGenPdf);
            this.panel2.Controls.Add(this.spacer1);
            this.panel2.Controls.Add(this.btnPdfPrev);
            this.panel2.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 152);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(5);
            this.panel2.Size = new System.Drawing.Size(482, 35);
            this.panel2.TabIndex = 2;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(392, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 25);
            // 
            // ADMNB002
            // 
            this.ClientSize = new System.Drawing.Size(482, 187);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ADMNB002";
            this.Text = "ADMNB002";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "Pb_Help";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.ADMNB002_ToolClick);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Panel panel1;
        private ComboBox CmbApp;
        private ComboBox CmbUser;
        private Label LblApp;
        private Label LblUser;
        private Button btnPdfPrev;
        private Button btnGenPdf;
        private Panel panel2;
        private Label lblHierequired;
        private Label label1;
        private RadioButton rdoUserAccess;
        private RadioButton rdoUserstructure;
        private Label label3;
        private TextBox txtHierchy;
        private PictureBox Pb_Search_Hie;
        private Spacer spacer1;
        private ComboBox cmbReportfor;
        private Label label4;
    }
}