using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class SSBG_Report
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SSBG_Report));
            this.Pb_Search_Hie = new Wisej.Web.PictureBox();
            this.CmbYear = new Wisej.Web.ComboBox();
            this.Txt_HieDesc = new Wisej.Web.TextBox();
            this.pnlHie = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlParams = new Wisej.Web.Panel();
            this.pnlAuditRep = new Wisej.Web.Panel();
            this.label1 = new Wisej.Web.Label();
            this.rbGreen = new Wisej.Web.RadioButton();
            this.rbGred = new Wisej.Web.RadioButton();
            this.pnlSite = new Wisej.Web.Panel();
            this.Txt_Sel_Site = new Wisej.Web.TextBox();
            this.Rb_Site_No = new Wisej.Web.RadioButton();
            this.Rb_Site_Sel = new Wisej.Web.RadioButton();
            this.lblSite = new Wisej.Web.Label();
            this.Rb_Site_All = new Wisej.Web.RadioButton();
            this.pnlDemoCount = new Wisej.Web.Panel();
            this.lblDemCount = new Wisej.Web.Label();
            this.cmbDemo = new Wisej.Web.ComboBox();
            this.txtTo1 = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtfrom1 = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.label2 = new Wisej.Web.Label();
            this.lblTo1 = new Wisej.Web.Label();
            this.label7 = new Wisej.Web.Label();
            this.lblAge1 = new Wisej.Web.Label();
            this.txtTo2 = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblAge2 = new Wisej.Web.Label();
            this.txtFrom2 = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblTo2 = new Wisej.Web.Label();
            this.pnlTopLefRig = new Wisej.Web.Panel();
            this.pnltxtDesc = new Wisej.Web.Panel();
            this.txtDesc = new Wisej.Web.TextBox();
            this.pnlTopLeft = new Wisej.Web.Panel();
            this.pnlRepPeriod = new Wisej.Web.Panel();
            this.lblRepDate = new Wisej.Web.Label();
            this.label6 = new Wisej.Web.Label();
            this.dtpFrom = new Wisej.Web.DateTimePicker();
            this.dtpTo = new Wisej.Web.DateTimePicker();
            this.pnlRefPeriod = new Wisej.Web.Panel();
            this.lblReferenceperiodate = new Wisej.Web.Label();
            this.Ref_From_Date = new Wisej.Web.DateTimePicker();
            this.lblReferenceTo = new Wisej.Web.Label();
            this.Ref_To_Date = new Wisej.Web.DateTimePicker();
            this.pnlRepSel = new Wisej.Web.Panel();
            this.lblSSBG = new Wisej.Web.Label();
            this.cmbSSBG = new Wisej.Web.ComboBox();
            this.lblreferenceFrom = new Wisej.Web.Label();
            this.lblRepFrom = new Wisej.Web.Label();
            this.chkbExcel = new Wisej.Web.CheckBox();
            this.btnGeneratePdf = new Wisej.Web.Button();
            this.btnPdfPreview = new Wisej.Web.Button();
            this.btnSaveParameters = new Wisej.Web.Button();
            this.btnGetParameters = new Wisej.Web.Button();
            this.panel9 = new Wisej.Web.Panel();
            this.spacer5 = new Wisej.Web.Spacer();
            this.spacer4 = new Wisej.Web.Spacer();
            this.spacer3 = new Wisej.Web.Spacer();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlHieFilter = new Wisej.Web.Panel();
            this.pnlFilter = new Wisej.Web.Panel();
            this.spacer2 = new Wisej.Web.Spacer();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).BeginInit();
            this.pnlHie.SuspendLayout();
            this.pnlParams.SuspendLayout();
            this.pnlAuditRep.SuspendLayout();
            this.pnlSite.SuspendLayout();
            this.pnlDemoCount.SuspendLayout();
            this.pnlTopLefRig.SuspendLayout();
            this.pnltxtDesc.SuspendLayout();
            this.pnlTopLeft.SuspendLayout();
            this.pnlRepPeriod.SuspendLayout();
            this.pnlRefPeriod.SuspendLayout();
            this.pnlRepSel.SuspendLayout();
            this.panel9.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlHieFilter.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // Pb_Search_Hie
            // 
            this.Pb_Search_Hie.BackColor = System.Drawing.Color.FromArgb(244, 244, 244);
            this.Pb_Search_Hie.CssStyle = "border-radius:25px";
            this.Pb_Search_Hie.Cursor = Wisej.Web.Cursors.Hand;
            this.Pb_Search_Hie.Dock = Wisej.Web.DockStyle.Left;
            this.Pb_Search_Hie.ImageSource = "captain-filter";
            this.Pb_Search_Hie.Location = new System.Drawing.Point(15, 0);
            this.Pb_Search_Hie.Name = "Pb_Search_Hie";
            this.Pb_Search_Hie.Padding = new Wisej.Web.Padding(4, 5, 4, 4);
            this.Pb_Search_Hie.Size = new System.Drawing.Size(25, 25);
            this.Pb_Search_Hie.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.Pb_Search_Hie.ToolTipText = "Select Hierarchy";
            this.Pb_Search_Hie.Click += new System.EventHandler(this.Pb_Search_Hie_Click);
            // 
            // CmbYear
            // 
            this.CmbYear.Dock = Wisej.Web.DockStyle.Left;
            this.CmbYear.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbYear.FormattingEnabled = true;
            this.CmbYear.Location = new System.Drawing.Point(835, 0);
            this.CmbYear.Name = "CmbYear";
            this.CmbYear.Size = new System.Drawing.Size(65, 25);
            this.CmbYear.TabIndex = 66;
            this.CmbYear.TabStop = false;
            this.CmbYear.Visible = false;
            this.CmbYear.SelectedIndexChanged += new System.EventHandler(this.CmbYear_SelectedIndexChanged);
            // 
            // Txt_HieDesc
            // 
            this.Txt_HieDesc.BackColor = System.Drawing.Color.Transparent;
            this.Txt_HieDesc.BorderStyle = Wisej.Web.BorderStyle.None;
            this.Txt_HieDesc.Dock = Wisej.Web.DockStyle.Left;
            this.Txt_HieDesc.Font = new System.Drawing.Font("defaultBold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.Txt_HieDesc.ForeColor = System.Drawing.Color.White;
            this.Txt_HieDesc.Location = new System.Drawing.Point(0, 0);
            this.Txt_HieDesc.Name = "Txt_HieDesc";
            this.Txt_HieDesc.ReadOnly = true;
            this.Txt_HieDesc.Size = new System.Drawing.Size(820, 25);
            this.Txt_HieDesc.TabIndex = 77;
            this.Txt_HieDesc.TabStop = false;
            this.Txt_HieDesc.TextAlign = Wisej.Web.HorizontalAlignment.Center;
            // 
            // pnlHie
            // 
            this.pnlHie.BackColor = System.Drawing.Color.FromArgb(11, 70, 117);
            this.pnlHie.Controls.Add(this.CmbYear);
            this.pnlHie.Controls.Add(this.spacer1);
            this.pnlHie.Controls.Add(this.Txt_HieDesc);
            this.pnlHie.Dock = Wisej.Web.DockStyle.Left;
            this.pnlHie.Location = new System.Drawing.Point(15, 9);
            this.pnlHie.Name = "pnlHie";
            this.pnlHie.Size = new System.Drawing.Size(900, 25);
            this.pnlHie.TabIndex = 88;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(820, 0);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(15, 25);
            // 
            // pnlParams
            // 
            this.pnlParams.Controls.Add(this.pnlAuditRep);
            this.pnlParams.Controls.Add(this.pnlSite);
            this.pnlParams.Controls.Add(this.pnlDemoCount);
            this.pnlParams.Controls.Add(this.pnlTopLefRig);
            this.pnlParams.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlParams.Location = new System.Drawing.Point(0, 43);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new System.Drawing.Size(988, 199);
            this.pnlParams.TabIndex = 1;
            // 
            // pnlAuditRep
            // 
            this.pnlAuditRep.Controls.Add(this.label1);
            this.pnlAuditRep.Controls.Add(this.rbGreen);
            this.pnlAuditRep.Controls.Add(this.rbGred);
            this.pnlAuditRep.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlAuditRep.Location = new System.Drawing.Point(0, 163);
            this.pnlAuditRep.Name = "pnlAuditRep";
            this.pnlAuditRep.Size = new System.Drawing.Size(988, 36);
            this.pnlAuditRep.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(15, 7);
            this.label1.Name = "label1";
            this.label1.RightToLeft = Wisej.Web.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(120, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Audit Report Control";
            // 
            // rbGreen
            // 
            this.rbGreen.AutoSize = false;
            this.rbGreen.Location = new System.Drawing.Point(150, 4);
            this.rbGreen.Name = "rbGreen";
            this.rbGreen.Size = new System.Drawing.Size(93, 21);
            this.rbGreen.TabIndex = 1;
            this.rbGreen.Text = "Green Only";
            // 
            // rbGred
            // 
            this.rbGred.AutoSize = false;
            this.rbGred.Checked = true;
            this.rbGred.Location = new System.Drawing.Point(260, 4);
            this.rbGred.Name = "rbGred";
            this.rbGred.Size = new System.Drawing.Size(112, 21);
            this.rbGred.TabIndex = 2;
            this.rbGred.TabStop = true;
            this.rbGred.Text = "Green and Red";
            // 
            // pnlSite
            // 
            this.pnlSite.Controls.Add(this.Txt_Sel_Site);
            this.pnlSite.Controls.Add(this.Rb_Site_No);
            this.pnlSite.Controls.Add(this.Rb_Site_Sel);
            this.pnlSite.Controls.Add(this.lblSite);
            this.pnlSite.Controls.Add(this.Rb_Site_All);
            this.pnlSite.Dock = Wisej.Web.DockStyle.Top;
            this.pnlSite.Location = new System.Drawing.Point(0, 132);
            this.pnlSite.Name = "pnlSite";
            this.pnlSite.Size = new System.Drawing.Size(988, 31);
            this.pnlSite.TabIndex = 3;
            // 
            // Txt_Sel_Site
            // 
            this.Txt_Sel_Site.AcceptsTab = true;
            this.Txt_Sel_Site.Location = new System.Drawing.Point(447, 3);
            this.Txt_Sel_Site.Name = "Txt_Sel_Site";
            this.Txt_Sel_Site.ReadOnly = true;
            this.Txt_Sel_Site.Size = new System.Drawing.Size(39, 25);
            this.Txt_Sel_Site.TabIndex = 4;
            this.Txt_Sel_Site.Visible = false;
            this.Txt_Sel_Site.WordWrap = false;
            // 
            // Rb_Site_No
            // 
            this.Rb_Site_No.AutoSize = false;
            this.Rb_Site_No.Location = new System.Drawing.Point(360, 4);
            this.Rb_Site_No.Name = "Rb_Site_No";
            this.Rb_Site_No.Size = new System.Drawing.Size(75, 21);
            this.Rb_Site_No.TabIndex = 3;
            this.Rb_Site_No.Text = "No Sites";
            this.Rb_Site_No.CheckedChanged += new System.EventHandler(this.Rb_Site_No_CheckedChanged);
            // 
            // Rb_Site_Sel
            // 
            this.Rb_Site_Sel.AutoSize = false;
            this.Rb_Site_Sel.Location = new System.Drawing.Point(235, 4);
            this.Rb_Site_Sel.Name = "Rb_Site_Sel";
            this.Rb_Site_Sel.Size = new System.Drawing.Size(107, 21);
            this.Rb_Site_Sel.TabIndex = 2;
            this.Rb_Site_Sel.Text = "Selected Sites";
            this.Rb_Site_Sel.CheckedChanged += new System.EventHandler(this.Rb_Site_Sel_CheckedChanged);
            this.Rb_Site_Sel.Click += new System.EventHandler(this.rdoSelectedSites_Click);
            // 
            // lblSite
            // 
            this.lblSite.Location = new System.Drawing.Point(15, 7);
            this.lblSite.Name = "lblSite";
            this.lblSite.Size = new System.Drawing.Size(62, 16);
            this.lblSite.TabIndex = 2;
            this.lblSite.Text = "Intake Site";
            // 
            // Rb_Site_All
            // 
            this.Rb_Site_All.AutoSize = false;
            this.Rb_Site_All.Checked = true;
            this.Rb_Site_All.Location = new System.Drawing.Point(150, 4);
            this.Rb_Site_All.Name = "Rb_Site_All";
            this.Rb_Site_All.Size = new System.Drawing.Size(73, 21);
            this.Rb_Site_All.TabIndex = 1;
            this.Rb_Site_All.TabStop = true;
            this.Rb_Site_All.Text = "All Sites";
            this.Rb_Site_All.Click += new System.EventHandler(this.Rb_Site_All_Click);
            // 
            // pnlDemoCount
            // 
            this.pnlDemoCount.Controls.Add(this.lblDemCount);
            this.pnlDemoCount.Controls.Add(this.cmbDemo);
            this.pnlDemoCount.Controls.Add(this.txtTo1);
            this.pnlDemoCount.Controls.Add(this.txtfrom1);
            this.pnlDemoCount.Controls.Add(this.label2);
            this.pnlDemoCount.Controls.Add(this.lblTo1);
            this.pnlDemoCount.Controls.Add(this.label7);
            this.pnlDemoCount.Controls.Add(this.lblAge1);
            this.pnlDemoCount.Controls.Add(this.txtTo2);
            this.pnlDemoCount.Controls.Add(this.lblAge2);
            this.pnlDemoCount.Controls.Add(this.txtFrom2);
            this.pnlDemoCount.Controls.Add(this.lblTo2);
            this.pnlDemoCount.Dock = Wisej.Web.DockStyle.Top;
            this.pnlDemoCount.Location = new System.Drawing.Point(0, 101);
            this.pnlDemoCount.Name = "pnlDemoCount";
            this.pnlDemoCount.Size = new System.Drawing.Size(988, 31);
            this.pnlDemoCount.TabIndex = 2;
            // 
            // lblDemCount
            // 
            this.lblDemCount.Location = new System.Drawing.Point(15, 7);
            this.lblDemCount.Name = "lblDemCount";
            this.lblDemCount.RightToLeft = Wisej.Web.RightToLeft.No;
            this.lblDemCount.Size = new System.Drawing.Size(116, 16);
            this.lblDemCount.TabIndex = 0;
            this.lblDemCount.Text = "Demographic Count";
            // 
            // cmbDemo
            // 
            this.cmbDemo.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbDemo.FormattingEnabled = true;
            this.cmbDemo.Location = new System.Drawing.Point(153, 3);
            this.cmbDemo.Name = "cmbDemo";
            this.cmbDemo.Size = new System.Drawing.Size(333, 25);
            this.cmbDemo.TabIndex = 1;
            // 
            // txtTo1
            // 
            this.txtTo1.Location = new System.Drawing.Point(690, 3);
            this.txtTo1.MaxLength = 3;
            this.txtTo1.Name = "txtTo1";
            this.txtTo1.Size = new System.Drawing.Size(35, 25);
            this.txtTo1.TabIndex = 3;
            this.txtTo1.Text = "59";
            this.txtTo1.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtTo1.LostFocus += new System.EventHandler(this.txtTo1_LostFocus);
            // 
            // txtfrom1
            // 
            this.txtfrom1.Location = new System.Drawing.Point(625, 3);
            this.txtfrom1.MaxLength = 3;
            this.txtfrom1.Name = "txtfrom1";
            this.txtfrom1.Size = new System.Drawing.Size(35, 25);
            this.txtfrom1.TabIndex = 2;
            this.txtfrom1.Text = "18";
            this.txtfrom1.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtfrom1.LostFocus += new System.EventHandler(this.txtfrom1_LostFocus);
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(825, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(7, 12);
            this.label2.TabIndex = 28;
            this.label2.Text = "*";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblTo1
            // 
            this.lblTo1.Location = new System.Drawing.Point(673, 6);
            this.lblTo1.Name = "lblTo1";
            this.lblTo1.Size = new System.Drawing.Size(7, 13);
            this.lblTo1.TabIndex = 3;
            this.lblTo1.Text = "-";
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(606, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(6, 12);
            this.label7.TabIndex = 28;
            this.label7.Text = "*";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblAge1
            // 
            this.lblAge1.Location = new System.Drawing.Point(540, 7);
            this.lblAge1.Name = "lblAge1";
            this.lblAge1.Size = new System.Drawing.Size(67, 16);
            this.lblAge1.TabIndex = 3;
            this.lblAge1.Text = "Age Range1";
            // 
            // txtTo2
            // 
            this.txtTo2.Location = new System.Drawing.Point(918, 3);
            this.txtTo2.MaxLength = 3;
            this.txtTo2.Name = "txtTo2";
            this.txtTo2.Size = new System.Drawing.Size(35, 25);
            this.txtTo2.TabIndex = 5;
            this.txtTo2.Text = "999";
            this.txtTo2.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtTo2.LostFocus += new System.EventHandler(this.txtTo2_LostFocus);
            // 
            // lblAge2
            // 
            this.lblAge2.Location = new System.Drawing.Point(759, 7);
            this.lblAge2.Name = "lblAge2";
            this.lblAge2.Size = new System.Drawing.Size(68, 16);
            this.lblAge2.TabIndex = 3;
            this.lblAge2.Text = "Age Range2";
            // 
            // txtFrom2
            // 
            this.txtFrom2.Location = new System.Drawing.Point(848, 3);
            this.txtFrom2.MaxLength = 3;
            this.txtFrom2.Name = "txtFrom2";
            this.txtFrom2.Size = new System.Drawing.Size(35, 25);
            this.txtFrom2.TabIndex = 4;
            this.txtFrom2.Text = "60";
            this.txtFrom2.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtFrom2.LostFocus += new System.EventHandler(this.txtFrom2_LostFocus);
            this.txtFrom2.Leave += new System.EventHandler(this.txtFrom2_Leave);
            // 
            // lblTo2
            // 
            this.lblTo2.Location = new System.Drawing.Point(900, 6);
            this.lblTo2.Name = "lblTo2";
            this.lblTo2.Size = new System.Drawing.Size(6, 12);
            this.lblTo2.TabIndex = 3;
            this.lblTo2.Text = "-";
            // 
            // pnlTopLefRig
            // 
            this.pnlTopLefRig.Controls.Add(this.pnltxtDesc);
            this.pnlTopLefRig.Controls.Add(this.pnlTopLeft);
            this.pnlTopLefRig.Dock = Wisej.Web.DockStyle.Top;
            this.pnlTopLefRig.Location = new System.Drawing.Point(0, 0);
            this.pnlTopLefRig.Name = "pnlTopLefRig";
            this.pnlTopLefRig.Size = new System.Drawing.Size(988, 101);
            this.pnlTopLefRig.TabIndex = 1;
            // 
            // pnltxtDesc
            // 
            this.pnltxtDesc.Controls.Add(this.txtDesc);
            this.pnltxtDesc.Dock = Wisej.Web.DockStyle.Fill;
            this.pnltxtDesc.Location = new System.Drawing.Point(524, 0);
            this.pnltxtDesc.Name = "pnltxtDesc";
            this.pnltxtDesc.Padding = new Wisej.Web.Padding(12);
            this.pnltxtDesc.Size = new System.Drawing.Size(464, 101);
            this.pnltxtDesc.TabIndex = 2;
            // 
            // txtDesc
            // 
            this.txtDesc.Dock = Wisej.Web.DockStyle.Fill;
            this.txtDesc.Enabled = false;
            this.txtDesc.Location = new System.Drawing.Point(12, 12);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(440, 77);
            this.txtDesc.TabIndex = 1;
            // 
            // pnlTopLeft
            // 
            this.pnlTopLeft.Controls.Add(this.pnlRepPeriod);
            this.pnlTopLeft.Controls.Add(this.pnlRefPeriod);
            this.pnlTopLeft.Controls.Add(this.pnlRepSel);
            this.pnlTopLeft.Dock = Wisej.Web.DockStyle.Left;
            this.pnlTopLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlTopLeft.Name = "pnlTopLeft";
            this.pnlTopLeft.Size = new System.Drawing.Size(524, 101);
            this.pnlTopLeft.TabIndex = 1;
            // 
            // pnlRepPeriod
            // 
            this.pnlRepPeriod.Controls.Add(this.lblRepDate);
            this.pnlRepPeriod.Controls.Add(this.label6);
            this.pnlRepPeriod.Controls.Add(this.dtpFrom);
            this.pnlRepPeriod.Controls.Add(this.dtpTo);
            this.pnlRepPeriod.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlRepPeriod.Location = new System.Drawing.Point(0, 70);
            this.pnlRepPeriod.Name = "pnlRepPeriod";
            this.pnlRepPeriod.Size = new System.Drawing.Size(524, 31);
            this.pnlRepPeriod.TabIndex = 3;
            // 
            // lblRepDate
            // 
            this.lblRepDate.Location = new System.Drawing.Point(15, 7);
            this.lblRepDate.Name = "lblRepDate";
            this.lblRepDate.Size = new System.Drawing.Size(112, 16);
            this.lblRepDate.TabIndex = 1;
            this.lblRepDate.Text = "Report Period From";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(340, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 16);
            this.label6.TabIndex = 1;
            this.label6.Text = "To";
            // 
            // dtpFrom
            // 
            this.dtpFrom.AutoSize = false;
            this.dtpFrom.CustomFormat = "MM/dd/yyyy";
            this.dtpFrom.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(190, 3);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.ShowCheckBox = true;
            this.dtpFrom.ShowToolTips = false;
            this.dtpFrom.Size = new System.Drawing.Size(116, 25);
            this.dtpFrom.TabIndex = 1;
            this.dtpFrom.Leave += new System.EventHandler(this.dtpTo_Leave);
            // 
            // dtpTo
            // 
            this.dtpTo.AutoSize = false;
            this.dtpTo.CustomFormat = "MM/dd/yyyy";
            this.dtpTo.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(370, 3);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.ShowCheckBox = true;
            this.dtpTo.ShowToolTips = false;
            this.dtpTo.Size = new System.Drawing.Size(116, 25);
            this.dtpTo.TabIndex = 2;
            this.dtpTo.Leave += new System.EventHandler(this.dtpTo_Leave);
            // 
            // pnlRefPeriod
            // 
            this.pnlRefPeriod.Controls.Add(this.lblReferenceperiodate);
            this.pnlRefPeriod.Controls.Add(this.Ref_From_Date);
            this.pnlRefPeriod.Controls.Add(this.lblReferenceTo);
            this.pnlRefPeriod.Controls.Add(this.Ref_To_Date);
            this.pnlRefPeriod.Dock = Wisej.Web.DockStyle.Top;
            this.pnlRefPeriod.Location = new System.Drawing.Point(0, 39);
            this.pnlRefPeriod.Name = "pnlRefPeriod";
            this.pnlRefPeriod.Size = new System.Drawing.Size(524, 31);
            this.pnlRefPeriod.TabIndex = 2;
            // 
            // lblReferenceperiodate
            // 
            this.lblReferenceperiodate.Location = new System.Drawing.Point(15, 7);
            this.lblReferenceperiodate.Name = "lblReferenceperiodate";
            this.lblReferenceperiodate.Size = new System.Drawing.Size(161, 16);
            this.lblReferenceperiodate.TabIndex = 2;
            this.lblReferenceperiodate.Text = "Reference Period Date From";
            // 
            // Ref_From_Date
            // 
            this.Ref_From_Date.AutoSize = false;
            this.Ref_From_Date.CustomFormat = "MM/dd/yyyy";
            this.Ref_From_Date.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.Ref_From_Date.Location = new System.Drawing.Point(190, 3);
            this.Ref_From_Date.Name = "Ref_From_Date";
            this.Ref_From_Date.ShowCheckBox = true;
            this.Ref_From_Date.ShowToolTips = false;
            this.Ref_From_Date.Size = new System.Drawing.Size(116, 25);
            this.Ref_From_Date.TabIndex = 1;
            // 
            // lblReferenceTo
            // 
            this.lblReferenceTo.Location = new System.Drawing.Point(340, 7);
            this.lblReferenceTo.Name = "lblReferenceTo";
            this.lblReferenceTo.Size = new System.Drawing.Size(15, 16);
            this.lblReferenceTo.TabIndex = 8;
            this.lblReferenceTo.Text = "To";
            // 
            // Ref_To_Date
            // 
            this.Ref_To_Date.AutoSize = false;
            this.Ref_To_Date.CustomFormat = "MM/dd/yyyy";
            this.Ref_To_Date.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.Ref_To_Date.Location = new System.Drawing.Point(370, 3);
            this.Ref_To_Date.Name = "Ref_To_Date";
            this.Ref_To_Date.ShowCheckBox = true;
            this.Ref_To_Date.ShowToolTips = false;
            this.Ref_To_Date.Size = new System.Drawing.Size(116, 25);
            this.Ref_To_Date.TabIndex = 2;
            // 
            // pnlRepSel
            // 
            this.pnlRepSel.Controls.Add(this.lblSSBG);
            this.pnlRepSel.Controls.Add(this.cmbSSBG);
            this.pnlRepSel.Controls.Add(this.lblreferenceFrom);
            this.pnlRepSel.Controls.Add(this.lblRepFrom);
            this.pnlRepSel.Dock = Wisej.Web.DockStyle.Top;
            this.pnlRepSel.Location = new System.Drawing.Point(0, 0);
            this.pnlRepSel.Name = "pnlRepSel";
            this.pnlRepSel.Size = new System.Drawing.Size(524, 39);
            this.pnlRepSel.TabIndex = 1;
            // 
            // lblSSBG
            // 
            this.lblSSBG.Location = new System.Drawing.Point(15, 15);
            this.lblSSBG.Name = "lblSSBG";
            this.lblSSBG.RightToLeft = Wisej.Web.RightToLeft.No;
            this.lblSSBG.Size = new System.Drawing.Size(95, 16);
            this.lblSSBG.TabIndex = 0;
            this.lblSSBG.Text = "Report Selection";
            // 
            // cmbSSBG
            // 
            this.cmbSSBG.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbSSBG.FormattingEnabled = true;
            this.cmbSSBG.Location = new System.Drawing.Point(153, 11);
            this.cmbSSBG.Name = "cmbSSBG";
            this.cmbSSBG.Size = new System.Drawing.Size(333, 25);
            this.cmbSSBG.TabIndex = 1;
            this.cmbSSBG.SelectedIndexChanged += new System.EventHandler(this.cmbSSBG_SelectedIndexChanged);
            // 
            // lblreferenceFrom
            // 
            this.lblreferenceFrom.AutoSize = true;
            this.lblreferenceFrom.Location = new System.Drawing.Point(582, 11);
            this.lblreferenceFrom.Name = "lblreferenceFrom";
            this.lblreferenceFrom.Size = new System.Drawing.Size(32, 14);
            this.lblreferenceFrom.TabIndex = 6;
            this.lblreferenceFrom.Text = "From";
            this.lblreferenceFrom.Visible = false;
            // 
            // lblRepFrom
            // 
            this.lblRepFrom.AutoSize = true;
            this.lblRepFrom.Location = new System.Drawing.Point(528, 11);
            this.lblRepFrom.Name = "lblRepFrom";
            this.lblRepFrom.Size = new System.Drawing.Size(32, 14);
            this.lblRepFrom.TabIndex = 6;
            this.lblRepFrom.Text = "From";
            this.lblRepFrom.Visible = false;
            // 
            // chkbExcel
            // 
            this.chkbExcel.Dock = Wisej.Web.DockStyle.Right;
            this.chkbExcel.Location = new System.Drawing.Point(453, 5);
            this.chkbExcel.Name = "chkbExcel";
            this.chkbExcel.Size = new System.Drawing.Size(115, 25);
            this.chkbExcel.TabIndex = 3;
            this.chkbExcel.Text = "Generate Excel";
            this.chkbExcel.Visible = false;
            // 
            // btnGeneratePdf
            // 
            this.btnGeneratePdf.AppearanceKey = "button-reports";
            this.btnGeneratePdf.Dock = Wisej.Web.DockStyle.Right;
            this.btnGeneratePdf.Location = new System.Drawing.Point(800, 5);
            this.btnGeneratePdf.Name = "btnGeneratePdf";
            this.btnGeneratePdf.Size = new System.Drawing.Size(85, 25);
            this.btnGeneratePdf.TabIndex = 4;
            this.btnGeneratePdf.Text = "&Generate";
            this.btnGeneratePdf.Click += new System.EventHandler(this.btnGeneratePdf_Click);
            // 
            // btnPdfPreview
            // 
            this.btnPdfPreview.AppearanceKey = "button-reports";
            this.btnPdfPreview.Dock = Wisej.Web.DockStyle.Right;
            this.btnPdfPreview.Location = new System.Drawing.Point(888, 5);
            this.btnPdfPreview.Name = "btnPdfPreview";
            this.btnPdfPreview.Size = new System.Drawing.Size(85, 25);
            this.btnPdfPreview.TabIndex = 5;
            this.btnPdfPreview.Text = "Pre&view";
            this.btnPdfPreview.Click += new System.EventHandler(this.btnPdfPreview_Click);
            // 
            // btnSaveParameters
            // 
            this.btnSaveParameters.AppearanceKey = "button-reports";
            this.btnSaveParameters.Dock = Wisej.Web.DockStyle.Left;
            this.btnSaveParameters.Location = new System.Drawing.Point(128, 5);
            this.btnSaveParameters.Name = "btnSaveParameters";
            this.btnSaveParameters.Size = new System.Drawing.Size(110, 25);
            this.btnSaveParameters.TabIndex = 2;
            this.btnSaveParameters.Text = "&Save Parameters";
            this.btnSaveParameters.Click += new System.EventHandler(this.btnSaveParameters_Click);
            // 
            // btnGetParameters
            // 
            this.btnGetParameters.AppearanceKey = "button-reports";
            this.btnGetParameters.Dock = Wisej.Web.DockStyle.Left;
            this.btnGetParameters.Location = new System.Drawing.Point(15, 5);
            this.btnGetParameters.Name = "btnGetParameters";
            this.btnGetParameters.Size = new System.Drawing.Size(110, 25);
            this.btnGetParameters.TabIndex = 1;
            this.btnGetParameters.Text = "Get &Parameters";
            this.btnGetParameters.Click += new System.EventHandler(this.btnGetParameters_Click);
            // 
            // panel9
            // 
            this.panel9.AppearanceKey = "panel-grdo";
            this.panel9.Controls.Add(this.btnSaveParameters);
            this.panel9.Controls.Add(this.spacer5);
            this.panel9.Controls.Add(this.chkbExcel);
            this.panel9.Controls.Add(this.spacer4);
            this.panel9.Controls.Add(this.btnGeneratePdf);
            this.panel9.Controls.Add(this.spacer3);
            this.panel9.Controls.Add(this.btnPdfPreview);
            this.panel9.Controls.Add(this.btnGetParameters);
            this.panel9.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel9.Location = new System.Drawing.Point(0, 242);
            this.panel9.Name = "panel9";
            this.panel9.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.panel9.Size = new System.Drawing.Size(988, 35);
            this.panel9.TabIndex = 2;
            // 
            // spacer5
            // 
            this.spacer5.Dock = Wisej.Web.DockStyle.Left;
            this.spacer5.Location = new System.Drawing.Point(125, 5);
            this.spacer5.Name = "spacer5";
            this.spacer5.Size = new System.Drawing.Size(3, 25);
            // 
            // spacer4
            // 
            this.spacer4.Dock = Wisej.Web.DockStyle.Right;
            this.spacer4.Location = new System.Drawing.Point(568, 5);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(232, 25);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Right;
            this.spacer3.Location = new System.Drawing.Point(885, 5);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(3, 25);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlParams);
            this.pnlCompleteForm.Controls.Add(this.pnlHieFilter);
            this.pnlCompleteForm.Controls.Add(this.panel9);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(988, 277);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlHieFilter
            // 
            this.pnlHieFilter.BackColor = System.Drawing.Color.FromArgb(11, 70, 117);
            this.pnlHieFilter.Controls.Add(this.pnlFilter);
            this.pnlHieFilter.Controls.Add(this.pnlHie);
            this.pnlHieFilter.Dock = Wisej.Web.DockStyle.Top;
            this.pnlHieFilter.Location = new System.Drawing.Point(0, 0);
            this.pnlHieFilter.Name = "pnlHieFilter";
            this.pnlHieFilter.Padding = new Wisej.Web.Padding(15, 9, 9, 9);
            this.pnlHieFilter.Size = new System.Drawing.Size(988, 43);
            this.pnlHieFilter.TabIndex = 99;
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.Pb_Search_Hie);
            this.pnlFilter.Controls.Add(this.spacer2);
            this.pnlFilter.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlFilter.Location = new System.Drawing.Point(915, 9);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(64, 25);
            this.pnlFilter.TabIndex = 55;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(0, 0);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(15, 25);
            // 
            // SSBG_Report
            // 
            this.ClientSize = new System.Drawing.Size(988, 277);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SSBG_Report";
            this.Text = "SSBG_Report";
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).EndInit();
            this.pnlHie.ResumeLayout(false);
            this.pnlHie.PerformLayout();
            this.pnlParams.ResumeLayout(false);
            this.pnlAuditRep.ResumeLayout(false);
            this.pnlSite.ResumeLayout(false);
            this.pnlSite.PerformLayout();
            this.pnlDemoCount.ResumeLayout(false);
            this.pnlDemoCount.PerformLayout();
            this.pnlTopLefRig.ResumeLayout(false);
            this.pnltxtDesc.ResumeLayout(false);
            this.pnltxtDesc.PerformLayout();
            this.pnlTopLeft.ResumeLayout(false);
            this.pnlRepPeriod.ResumeLayout(false);
            this.pnlRefPeriod.ResumeLayout(false);
            this.pnlRepSel.ResumeLayout(false);
            this.pnlRepSel.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlHieFilter.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox Pb_Search_Hie;
        private ComboBox CmbYear;
        private TextBox Txt_HieDesc;
        private Panel pnlHie;
        private Panel pnlParams;
        private Label lblSSBG;
        private ComboBox cmbSSBG;
        private TextBox txtDesc;
        private DateTimePicker dtpTo;
        private DateTimePicker dtpFrom;
        private Label lblRepDate;
        private Label label6;
        private CheckBox chkbExcel;
        private Button btnGeneratePdf;
        private Button btnPdfPreview;
        private Button btnSaveParameters;
        private Button btnGetParameters;
        private Panel panel9;
        private Label lblRepFrom;
        private Label lblReferenceperiodate;
        private Label lblreferenceFrom;
        private DateTimePicker Ref_To_Date;
        private Label lblReferenceTo;
        private DateTimePicker Ref_From_Date;
        private ComboBox cmbDemo;
        private Label lblDemCount;
        private RadioButton rbGred;
        private RadioButton rbGreen;
        private Label label1;
        private TextBoxWithValidation txtTo2;
        private TextBoxWithValidation txtFrom2;
        private Label lblTo2;
        private Label lblAge2;
        private Label lblAge1;
        private Label lblTo1;
        private TextBoxWithValidation txtfrom1;
        private TextBoxWithValidation txtTo1;
        private Label label2;
        private Label label7;
        private Panel pnlSite;
        private TextBox Txt_Sel_Site;
        private RadioButton Rb_Site_No;
        private RadioButton Rb_Site_Sel;
        private RadioButton Rb_Site_All;
        private Label lblSite;
        private Panel pnlCompleteForm;
        private Panel pnlHieFilter;
        private Spacer spacer1;
        private Panel pnlFilter;
        private Spacer spacer2;
        private Spacer spacer3;
        private Spacer spacer4;
        private Spacer spacer5;
        private Panel pnlTopLefRig;
        private Panel pnlTopLeft;
        private Panel pnltxtDesc;
        private Panel pnlRepSel;
        private Panel pnlRefPeriod;
        private Panel pnlRepPeriod;
        private Panel pnlDemoCount;
        private Panel pnlAuditRep;
    }
}