//using Wisej.Web;
//using Gizmox.WebGUI.Common;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class ADMNB001
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ADMNB001));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.btnPDFPreview = new Wisej.Web.Button();
            this.btnGeneratePDF = new Wisej.Web.Button();
            this.pnlPDF = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.lblReqFormat = new Wisej.Web.Label();
            this.lblReqType = new Wisej.Web.Label();
            this.CmbAgyTab = new Wisej.Web.ComboBox();
            this.LblAgyTab = new Wisej.Web.Label();
            this.pnlTypes = new Wisej.Web.Panel();
            this.pnlType = new Wisej.Web.Panel();
            this.rbStandard = new Wisej.Web.RadioButton();
            this.rbBoth = new Wisej.Web.RadioButton();
            this.rbCustom = new Wisej.Web.RadioButton();
            this.lblType = new Wisej.Web.Label();
            this.pnlHiendMod = new Wisej.Web.Panel();
            this.cmbModule = new Wisej.Web.ComboBox();
            this.lblReqHie = new Wisej.Web.Label();
            this.lblHie = new Wisej.Web.Label();
            this.HieFilter = new Wisej.Web.PictureBox();
            this.txtHie = new Wisej.Web.TextBox();
            this.pnlAgyTbl = new Wisej.Web.Panel();
            this.lblReqAgyTable = new Wisej.Web.Label();
            this.pnlFormat = new Wisej.Web.Panel();
            this.rbCounty = new Wisej.Web.RadioButton();
            this.rbTown = new Wisej.Web.RadioButton();
            this.RblCaseMgmt = new Wisej.Web.RadioButton();
            this.RblEmrServ = new Wisej.Web.RadioButton();
            this.LblFormat = new Wisej.Web.Label();
            this.pnlAccessndRepList = new Wisej.Web.Panel();
            this.pnlRepList = new Wisej.Web.Panel();
            this.cmbRepCode = new Wisej.Web.ComboBox();
            this.lblReportCode = new Wisej.Web.Label();
            this.cmbRepModule = new Wisej.Web.ComboBox();
            this.lblModule = new Wisej.Web.Label();
            this.lblUserID = new Wisej.Web.Label();
            this.cmbRepUserId = new Wisej.Web.ComboBox();
            this.dtRepDate = new Wisej.Web.DateTimePicker();
            this.rdoRepDate = new Wisej.Web.RadioButton();
            this.rdoRepAll = new Wisej.Web.RadioButton();
            this.lblRepListType = new Wisej.Web.Label();
            this.pnlAccess = new Wisej.Web.Panel();
            this.lblTodate = new Wisej.Web.Label();
            this.dtSelectToDate = new Wisej.Web.DateTimePicker();
            this.LblUser = new Wisej.Web.Label();
            this.CmbUser = new Wisej.Web.ComboBox();
            this.dtselect = new Wisej.Web.DateTimePicker();
            this.rdoSeletedt = new Wisej.Web.RadioButton();
            this.rdoAll = new Wisej.Web.RadioButton();
            this.lblAccessType = new Wisej.Web.Label();
            this.pnlReportType = new Wisej.Web.Panel();
            this.CmbRepType = new Wisej.Web.ComboBox();
            this.LblRepType = new Wisej.Web.Label();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlPDF.SuspendLayout();
            this.pnlTypes.SuspendLayout();
            this.pnlType.SuspendLayout();
            this.pnlHiendMod.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HieFilter)).BeginInit();
            this.pnlAgyTbl.SuspendLayout();
            this.pnlFormat.SuspendLayout();
            this.pnlAccessndRepList.SuspendLayout();
            this.pnlRepList.SuspendLayout();
            this.pnlAccess.SuspendLayout();
            this.pnlReportType.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPDFPreview
            // 
            this.btnPDFPreview.AppearanceKey = "button-reports";
            this.btnPDFPreview.Dock = Wisej.Web.DockStyle.Right;
            this.btnPDFPreview.Location = new System.Drawing.Point(909, 5);
            this.btnPDFPreview.Name = "btnPDFPreview";
            this.btnPDFPreview.Size = new System.Drawing.Size(80, 25);
            this.btnPDFPreview.TabIndex = 34;
            this.btnPDFPreview.Text = "&Preview";
            this.btnPDFPreview.Click += new System.EventHandler(this.BtnPdfPrev_Click);
            // 
            // btnGeneratePDF
            // 
            this.btnGeneratePDF.AppearanceKey = "button-reports";
            this.btnGeneratePDF.Dock = Wisej.Web.DockStyle.Right;
            this.btnGeneratePDF.Location = new System.Drawing.Point(826, 5);
            this.btnGeneratePDF.Name = "btnGeneratePDF";
            this.btnGeneratePDF.Size = new System.Drawing.Size(80, 25);
            this.btnGeneratePDF.TabIndex = 33;
            this.btnGeneratePDF.Text = "&Generate";
            this.btnGeneratePDF.Click += new System.EventHandler(this.BtnGenFile_Click);
            // 
            // pnlPDF
            // 
            this.pnlPDF.AppearanceKey = "panel-grdo";
            this.pnlPDF.Controls.Add(this.btnGeneratePDF);
            this.pnlPDF.Controls.Add(this.spacer1);
            this.pnlPDF.Controls.Add(this.btnPDFPreview);
            this.pnlPDF.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlPDF.Location = new System.Drawing.Point(0, 295);
            this.pnlPDF.Name = "pnlPDF";
            this.pnlPDF.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlPDF.Size = new System.Drawing.Size(1004, 35);
            this.pnlPDF.TabIndex = 32;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(906, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // lblReqFormat
            // 
            this.lblReqFormat.Enabled = false;
            this.lblReqFormat.ForeColor = System.Drawing.Color.Red;
            this.lblReqFormat.Location = new System.Drawing.Point(57, 5);
            this.lblReqFormat.Name = "lblReqFormat";
            this.lblReqFormat.Size = new System.Drawing.Size(7, 11);
            this.lblReqFormat.TabIndex = 2;
            this.lblReqFormat.Text = "*";
            this.lblReqFormat.Visible = false;
            // 
            // lblReqType
            // 
            this.lblReqType.ForeColor = System.Drawing.Color.Red;
            this.lblReqType.Location = new System.Drawing.Point(83, 11);
            this.lblReqType.Name = "lblReqType";
            this.lblReqType.Size = new System.Drawing.Size(7, 11);
            this.lblReqType.TabIndex = 2;
            this.lblReqType.Text = "*";
            this.lblReqType.Visible = false;
            // 
            // CmbAgyTab
            // 
            this.CmbAgyTab.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbAgyTab.Enabled = false;
            this.CmbAgyTab.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.CmbAgyTab.FormattingEnabled = true;
            this.CmbAgyTab.Location = new System.Drawing.Point(110, 1);
            this.CmbAgyTab.Name = "CmbAgyTab";
            this.CmbAgyTab.Size = new System.Drawing.Size(379, 25);
            this.CmbAgyTab.TabIndex = 24;
            this.CmbAgyTab.SelectedIndexChanged += new System.EventHandler(this.CmbAgyTab_SelectedIndexChanged);
            // 
            // LblAgyTab
            // 
            this.LblAgyTab.Enabled = false;
            this.LblAgyTab.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.LblAgyTab.Location = new System.Drawing.Point(15, 5);
            this.LblAgyTab.Name = "LblAgyTab";
            this.LblAgyTab.Size = new System.Drawing.Size(76, 16);
            this.LblAgyTab.TabIndex = 0;
            this.LblAgyTab.Text = "Agency Table";
            // 
            // pnlTypes
            // 
            this.pnlTypes.Controls.Add(this.pnlType);
            this.pnlTypes.Controls.Add(this.pnlHiendMod);
            this.pnlTypes.Controls.Add(this.pnlAgyTbl);
            this.pnlTypes.Controls.Add(this.pnlFormat);
            this.pnlTypes.Controls.Add(this.pnlAccessndRepList);
            this.pnlTypes.Controls.Add(this.pnlReportType);
            this.pnlTypes.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlTypes.Location = new System.Drawing.Point(0, 0);
            this.pnlTypes.Name = "pnlTypes";
            this.pnlTypes.Size = new System.Drawing.Size(1004, 295);
            this.pnlTypes.TabIndex = 1;
            // 
            // pnlType
            // 
            this.pnlType.Controls.Add(this.rbStandard);
            this.pnlType.Controls.Add(this.rbBoth);
            this.pnlType.Controls.Add(this.rbCustom);
            this.pnlType.Controls.Add(this.lblType);
            this.pnlType.Dock = Wisej.Web.DockStyle.Top;
            this.pnlType.Location = new System.Drawing.Point(0, 268);
            this.pnlType.Name = "pnlType";
            this.pnlType.Size = new System.Drawing.Size(1004, 28);
            this.pnlType.TabIndex = 28;
            // 
            // rbStandard
            // 
            this.rbStandard.AutoSize = false;
            this.rbStandard.Checked = true;
            this.rbStandard.Location = new System.Drawing.Point(106, 5);
            this.rbStandard.Name = "rbStandard";
            this.rbStandard.Size = new System.Drawing.Size(79, 20);
            this.rbStandard.TabIndex = 29;
            this.rbStandard.TabStop = true;
            this.rbStandard.Text = "Standard";
            // 
            // rbBoth
            // 
            this.rbBoth.AutoSize = false;
            this.rbBoth.Location = new System.Drawing.Point(335, 6);
            this.rbBoth.Name = "rbBoth";
            this.rbBoth.Size = new System.Drawing.Size(54, 20);
            this.rbBoth.TabIndex = 31;
            this.rbBoth.Text = "Both";
            // 
            // rbCustom
            // 
            this.rbCustom.AutoSize = false;
            this.rbCustom.Location = new System.Drawing.Point(193, 5);
            this.rbCustom.Name = "rbCustom";
            this.rbCustom.Size = new System.Drawing.Size(131, 20);
            this.rbCustom.TabIndex = 30;
            this.rbCustom.Text = "Custom Questions";
            // 
            // lblType
            // 
            this.lblType.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblType.Location = new System.Drawing.Point(15, 8);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(28, 16);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "Type";
            // 
            // pnlHiendMod
            // 
            this.pnlHiendMod.Controls.Add(this.cmbModule);
            this.pnlHiendMod.Controls.Add(this.lblReqHie);
            this.pnlHiendMod.Controls.Add(this.lblHie);
            this.pnlHiendMod.Controls.Add(this.HieFilter);
            this.pnlHiendMod.Controls.Add(this.txtHie);
            this.pnlHiendMod.Dock = Wisej.Web.DockStyle.Top;
            this.pnlHiendMod.Location = new System.Drawing.Point(0, 237);
            this.pnlHiendMod.Name = "pnlHiendMod";
            this.pnlHiendMod.Size = new System.Drawing.Size(1004, 31);
            this.pnlHiendMod.TabIndex = 25;
            // 
            // cmbModule
            // 
            this.cmbModule.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbModule.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbModule.FormattingEnabled = true;
            this.cmbModule.Location = new System.Drawing.Point(244, 4);
            this.cmbModule.Name = "cmbModule";
            this.cmbModule.Size = new System.Drawing.Size(253, 25);
            this.cmbModule.TabIndex = 27;
            // 
            // lblReqHie
            // 
            this.lblReqHie.ForeColor = System.Drawing.Color.Red;
            this.lblReqHie.Location = new System.Drawing.Point(71, 6);
            this.lblReqHie.Name = "lblReqHie";
            this.lblReqHie.Size = new System.Drawing.Size(6, 9);
            this.lblReqHie.TabIndex = 2;
            this.lblReqHie.Text = "*";
            // 
            // lblHie
            // 
            this.lblHie.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblHie.Location = new System.Drawing.Point(15, 8);
            this.lblHie.Name = "lblHie";
            this.lblHie.Size = new System.Drawing.Size(59, 16);
            this.lblHie.TabIndex = 0;
            this.lblHie.Text = "Hierarchy";
            // 
            // HieFilter
            // 
            this.HieFilter.ImageSource = "captain-filter";
            this.HieFilter.Location = new System.Drawing.Point(187, 7);
            this.HieFilter.Name = "HieFilter";
            this.HieFilter.Size = new System.Drawing.Size(20, 20);
            this.HieFilter.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.HieFilter.Visible = false;
            this.HieFilter.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // txtHie
            // 
            this.txtHie.Enabled = false;
            this.txtHie.Location = new System.Drawing.Point(110, 4);
            this.txtHie.Name = "txtHie";
            this.txtHie.Size = new System.Drawing.Size(69, 25);
            this.txtHie.TabIndex = 26;
            // 
            // pnlAgyTbl
            // 
            this.pnlAgyTbl.Controls.Add(this.lblReqAgyTable);
            this.pnlAgyTbl.Controls.Add(this.CmbAgyTab);
            this.pnlAgyTbl.Controls.Add(this.LblAgyTab);
            this.pnlAgyTbl.Dock = Wisej.Web.DockStyle.Top;
            this.pnlAgyTbl.Location = new System.Drawing.Point(0, 208);
            this.pnlAgyTbl.Name = "pnlAgyTbl";
            this.pnlAgyTbl.Size = new System.Drawing.Size(1004, 29);
            this.pnlAgyTbl.TabIndex = 23;
            // 
            // lblReqAgyTable
            // 
            this.lblReqAgyTable.ForeColor = System.Drawing.Color.Red;
            this.lblReqAgyTable.Location = new System.Drawing.Point(90, 5);
            this.lblReqAgyTable.Name = "lblReqAgyTable";
            this.lblReqAgyTable.Size = new System.Drawing.Size(6, 9);
            this.lblReqAgyTable.TabIndex = 2;
            this.lblReqAgyTable.Text = "*";
            // 
            // pnlFormat
            // 
            this.pnlFormat.Controls.Add(this.rbCounty);
            this.pnlFormat.Controls.Add(this.rbTown);
            this.pnlFormat.Controls.Add(this.RblCaseMgmt);
            this.pnlFormat.Controls.Add(this.RblEmrServ);
            this.pnlFormat.Controls.Add(this.LblFormat);
            this.pnlFormat.Controls.Add(this.lblReqFormat);
            this.pnlFormat.Dock = Wisej.Web.DockStyle.Top;
            this.pnlFormat.Location = new System.Drawing.Point(0, 179);
            this.pnlFormat.Name = "pnlFormat";
            this.pnlFormat.Size = new System.Drawing.Size(1004, 29);
            this.pnlFormat.TabIndex = 18;
            // 
            // rbCounty
            // 
            this.rbCounty.AutoSize = false;
            this.rbCounty.Location = new System.Drawing.Point(475, 3);
            this.rbCounty.Name = "rbCounty";
            this.rbCounty.Size = new System.Drawing.Size(67, 20);
            this.rbCounty.TabIndex = 22;
            this.rbCounty.Text = "County";
            this.rbCounty.Visible = false;
            // 
            // rbTown
            // 
            this.rbTown.AutoSize = false;
            this.rbTown.Location = new System.Drawing.Point(389, 3);
            this.rbTown.Name = "rbTown";
            this.rbTown.Size = new System.Drawing.Size(81, 20);
            this.rbTown.TabIndex = 21;
            this.rbTown.Text = "Township";
            this.rbTown.Visible = false;
            // 
            // RblCaseMgmt
            // 
            this.RblCaseMgmt.AutoSize = false;
            this.RblCaseMgmt.Enabled = false;
            this.RblCaseMgmt.Location = new System.Drawing.Point(106, 3);
            this.RblCaseMgmt.Name = "RblCaseMgmt";
            this.RblCaseMgmt.Size = new System.Drawing.Size(132, 20);
            this.RblCaseMgmt.TabIndex = 19;
            this.RblCaseMgmt.Text = "Case Management";
            this.RblCaseMgmt.CheckedChanged += new System.EventHandler(this.RblEmrServ_CheckedChanged);
            // 
            // RblEmrServ
            // 
            this.RblEmrServ.AutoSize = false;
            this.RblEmrServ.Enabled = false;
            this.RblEmrServ.Location = new System.Drawing.Point(245, 3);
            this.RblEmrServ.Name = "RblEmrServ";
            this.RblEmrServ.Size = new System.Drawing.Size(139, 20);
            this.RblEmrServ.TabIndex = 20;
            this.RblEmrServ.Text = "Emergency Services";
            this.RblEmrServ.CheckedChanged += new System.EventHandler(this.RblEmrServ_CheckedChanged);
            // 
            // LblFormat
            // 
            this.LblFormat.Enabled = false;
            this.LblFormat.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.LblFormat.Location = new System.Drawing.Point(15, 7);
            this.LblFormat.Name = "LblFormat";
            this.LblFormat.Size = new System.Drawing.Size(42, 16);
            this.LblFormat.TabIndex = 0;
            this.LblFormat.Text = "Format";
            // 
            // pnlAccessndRepList
            // 
            this.pnlAccessndRepList.Controls.Add(this.pnlRepList);
            this.pnlAccessndRepList.Controls.Add(this.pnlAccess);
            this.pnlAccessndRepList.Dock = Wisej.Web.DockStyle.Top;
            this.pnlAccessndRepList.Location = new System.Drawing.Point(0, 40);
            this.pnlAccessndRepList.Name = "pnlAccessndRepList";
            this.pnlAccessndRepList.Size = new System.Drawing.Size(1004, 139);
            this.pnlAccessndRepList.TabIndex = 4;
            // 
            // pnlRepList
            // 
            this.pnlRepList.Controls.Add(this.cmbRepCode);
            this.pnlRepList.Controls.Add(this.lblReportCode);
            this.pnlRepList.Controls.Add(this.cmbRepModule);
            this.pnlRepList.Controls.Add(this.lblModule);
            this.pnlRepList.Controls.Add(this.lblUserID);
            this.pnlRepList.Controls.Add(this.cmbRepUserId);
            this.pnlRepList.Controls.Add(this.dtRepDate);
            this.pnlRepList.Controls.Add(this.rdoRepDate);
            this.pnlRepList.Controls.Add(this.rdoRepAll);
            this.pnlRepList.Controls.Add(this.lblRepListType);
            this.pnlRepList.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlRepList.Location = new System.Drawing.Point(597, 0);
            this.pnlRepList.Name = "pnlRepList";
            this.pnlRepList.Size = new System.Drawing.Size(407, 139);
            this.pnlRepList.TabIndex = 11;
            this.pnlRepList.Visible = false;
            // 
            // cmbRepCode
            // 
            this.cmbRepCode.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbRepCode.FormattingEnabled = true;
            this.cmbRepCode.Location = new System.Drawing.Point(110, 99);
            this.cmbRepCode.Name = "cmbRepCode";
            this.cmbRepCode.Size = new System.Drawing.Size(201, 25);
            this.cmbRepCode.TabIndex = 17;
            // 
            // lblReportCode
            // 
            this.lblReportCode.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblReportCode.Location = new System.Drawing.Point(15, 103);
            this.lblReportCode.Name = "lblReportCode";
            this.lblReportCode.Size = new System.Drawing.Size(71, 16);
            this.lblReportCode.TabIndex = 0;
            this.lblReportCode.Text = "Report Code";
            // 
            // cmbRepModule
            // 
            this.cmbRepModule.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbRepModule.FormattingEnabled = true;
            this.cmbRepModule.Location = new System.Drawing.Point(110, 67);
            this.cmbRepModule.Name = "cmbRepModule";
            this.cmbRepModule.Size = new System.Drawing.Size(201, 25);
            this.cmbRepModule.TabIndex = 16;
            // 
            // lblModule
            // 
            this.lblModule.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblModule.Location = new System.Drawing.Point(15, 71);
            this.lblModule.Name = "lblModule";
            this.lblModule.Size = new System.Drawing.Size(44, 14);
            this.lblModule.TabIndex = 0;
            this.lblModule.Text = "Module";
            // 
            // lblUserID
            // 
            this.lblUserID.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblUserID.Location = new System.Drawing.Point(15, 39);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(40, 14);
            this.lblUserID.TabIndex = 0;
            this.lblUserID.Text = "UserID";
            // 
            // cmbRepUserId
            // 
            this.cmbRepUserId.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbRepUserId.FormattingEnabled = true;
            this.cmbRepUserId.Location = new System.Drawing.Point(110, 35);
            this.cmbRepUserId.Name = "cmbRepUserId";
            this.cmbRepUserId.Size = new System.Drawing.Size(201, 25);
            this.cmbRepUserId.TabIndex = 15;
            // 
            // dtRepDate
            // 
            this.dtRepDate.AutoSize = false;
            this.dtRepDate.CustomFormat = "MM/dd/yyyy";
            this.dtRepDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtRepDate.Location = new System.Drawing.Point(278, 3);
            this.dtRepDate.Name = "dtRepDate";
            this.dtRepDate.ShowCheckBox = true;
            this.dtRepDate.ShowToolTips = false;
            this.dtRepDate.Size = new System.Drawing.Size(116, 25);
            this.dtRepDate.TabIndex = 14;
            this.dtRepDate.Visible = false;
            // 
            // rdoRepDate
            // 
            this.rdoRepDate.AutoSize = false;
            this.rdoRepDate.Location = new System.Drawing.Point(164, 4);
            this.rdoRepDate.Name = "rdoRepDate";
            this.rdoRepDate.Size = new System.Drawing.Size(105, 20);
            this.rdoRepDate.TabIndex = 13;
            this.rdoRepDate.Text = "Selected Date";
            this.rdoRepDate.CheckedChanged += new System.EventHandler(this.rdoRepAll_CheckedChanged);
            // 
            // rdoRepAll
            // 
            this.rdoRepAll.AutoSize = false;
            this.rdoRepAll.Checked = true;
            this.rdoRepAll.Location = new System.Drawing.Point(107, 4);
            this.rdoRepAll.Name = "rdoRepAll";
            this.rdoRepAll.Size = new System.Drawing.Size(41, 20);
            this.rdoRepAll.TabIndex = 12;
            this.rdoRepAll.TabStop = true;
            this.rdoRepAll.Text = "All";
            this.rdoRepAll.CheckedChanged += new System.EventHandler(this.rdoRepAll_CheckedChanged);
            // 
            // lblRepListType
            // 
            this.lblRepListType.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblRepListType.Location = new System.Drawing.Point(15, 7);
            this.lblRepListType.Name = "lblRepListType";
            this.lblRepListType.Size = new System.Drawing.Size(28, 16);
            this.lblRepListType.TabIndex = 0;
            this.lblRepListType.Text = "Type";
            // 
            // pnlAccess
            // 
            this.pnlAccess.Controls.Add(this.lblTodate);
            this.pnlAccess.Controls.Add(this.dtSelectToDate);
            this.pnlAccess.Controls.Add(this.LblUser);
            this.pnlAccess.Controls.Add(this.CmbUser);
            this.pnlAccess.Controls.Add(this.dtselect);
            this.pnlAccess.Controls.Add(this.rdoSeletedt);
            this.pnlAccess.Controls.Add(this.rdoAll);
            this.pnlAccess.Controls.Add(this.lblAccessType);
            this.pnlAccess.Dock = Wisej.Web.DockStyle.Left;
            this.pnlAccess.Location = new System.Drawing.Point(0, 0);
            this.pnlAccess.Name = "pnlAccess";
            this.pnlAccess.Size = new System.Drawing.Size(597, 139);
            this.pnlAccess.TabIndex = 5;
            this.pnlAccess.Visible = false;
            // 
            // lblTodate
            // 
            this.lblTodate.Location = new System.Drawing.Point(430, 7);
            this.lblTodate.Name = "lblTodate";
            this.lblTodate.Size = new System.Drawing.Size(14, 14);
            this.lblTodate.TabIndex = 10;
            this.lblTodate.Text = "To";
            this.lblTodate.Visible = false;
            // 
            // dtSelectToDate
            // 
            this.dtSelectToDate.AutoSize = false;
            this.dtSelectToDate.CustomFormat = "MM/dd/yyyy";
            this.dtSelectToDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtSelectToDate.Location = new System.Drawing.Point(452, 3);
            this.dtSelectToDate.Name = "dtSelectToDate";
            this.dtSelectToDate.ShowCheckBox = true;
            this.dtSelectToDate.ShowToolTips = false;
            this.dtSelectToDate.Size = new System.Drawing.Size(116, 25);
            this.dtSelectToDate.TabIndex = 9;
            this.dtSelectToDate.Visible = false;
            // 
            // LblUser
            // 
            this.LblUser.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.LblUser.Location = new System.Drawing.Point(15, 39);
            this.LblUser.Name = "LblUser";
            this.LblUser.Size = new System.Drawing.Size(40, 14);
            this.LblUser.TabIndex = 0;
            this.LblUser.Text = "UserID";
            // 
            // CmbUser
            // 
            this.CmbUser.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbUser.FormattingEnabled = true;
            this.CmbUser.Location = new System.Drawing.Point(110, 35);
            this.CmbUser.Name = "CmbUser";
            this.CmbUser.Size = new System.Drawing.Size(300, 25);
            this.CmbUser.TabIndex = 10;
            // 
            // dtselect
            // 
            this.dtselect.AutoSize = false;
            this.dtselect.CustomFormat = "MM/dd/yyyy";
            this.dtselect.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtselect.Location = new System.Drawing.Point(289, 3);
            this.dtselect.Name = "dtselect";
            this.dtselect.ShowCheckBox = true;
            this.dtselect.ShowToolTips = false;
            this.dtselect.Size = new System.Drawing.Size(116, 25);
            this.dtselect.TabIndex = 8;
            this.dtselect.Visible = false;
            // 
            // rdoSeletedt
            // 
            this.rdoSeletedt.AutoSize = false;
            this.rdoSeletedt.Location = new System.Drawing.Point(154, 4);
            this.rdoSeletedt.Name = "rdoSeletedt";
            this.rdoSeletedt.Size = new System.Drawing.Size(125, 20);
            this.rdoSeletedt.TabIndex = 7;
            this.rdoSeletedt.Text = "Date Range From";
            this.rdoSeletedt.CheckedChanged += new System.EventHandler(this.rdoAll_CheckedChanged);
            // 
            // rdoAll
            // 
            this.rdoAll.AutoSize = false;
            this.rdoAll.Checked = true;
            this.rdoAll.Location = new System.Drawing.Point(106, 4);
            this.rdoAll.Name = "rdoAll";
            this.rdoAll.Size = new System.Drawing.Size(41, 20);
            this.rdoAll.TabIndex = 6;
            this.rdoAll.TabStop = true;
            this.rdoAll.Text = "All";
            this.rdoAll.CheckedChanged += new System.EventHandler(this.rdoAll_CheckedChanged);
            // 
            // lblAccessType
            // 
            this.lblAccessType.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAccessType.Location = new System.Drawing.Point(15, 7);
            this.lblAccessType.Name = "lblAccessType";
            this.lblAccessType.Size = new System.Drawing.Size(28, 16);
            this.lblAccessType.TabIndex = 0;
            this.lblAccessType.Text = "Type";
            // 
            // pnlReportType
            // 
            this.pnlReportType.Controls.Add(this.CmbRepType);
            this.pnlReportType.Controls.Add(this.lblReqType);
            this.pnlReportType.Controls.Add(this.LblRepType);
            this.pnlReportType.Dock = Wisej.Web.DockStyle.Top;
            this.pnlReportType.Location = new System.Drawing.Point(0, 0);
            this.pnlReportType.Name = "pnlReportType";
            this.pnlReportType.Size = new System.Drawing.Size(1004, 40);
            this.pnlReportType.TabIndex = 2;
            // 
            // CmbRepType
            // 
            this.CmbRepType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbRepType.FormattingEnabled = true;
            this.CmbRepType.Location = new System.Drawing.Point(110, 11);
            this.CmbRepType.Name = "CmbRepType";
            this.CmbRepType.Size = new System.Drawing.Size(300, 25);
            this.CmbRepType.TabIndex = 3;
            this.CmbRepType.SelectedIndexChanged += new System.EventHandler(this.CmbRepType_SelectedIndexChanged);
            // 
            // LblRepType
            // 
            this.LblRepType.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LblRepType.Location = new System.Drawing.Point(15, 15);
            this.LblRepType.Name = "LblRepType";
            this.LblRepType.Size = new System.Drawing.Size(75, 16);
            this.LblRepType.TabIndex = 0;
            this.LblRepType.Text = "Report Type";
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlTypes);
            this.pnlCompleteForm.Controls.Add(this.pnlPDF);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(1004, 330);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // ADMNB001
            // 
            this.ClientSize = new System.Drawing.Size(1004, 330);
            this.Controls.Add(this.pnlCompleteForm);
            this.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.HeaderBackColor = System.Drawing.Color.FromArgb(244, 244, 244);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ADMNB001";
            this.Text = "Master Tables List";
            componentTool1.ImageSource = "icon-help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.ADMNB001_ToolClick);
            this.pnlPDF.ResumeLayout(false);
            this.pnlTypes.ResumeLayout(false);
            this.pnlType.ResumeLayout(false);
            this.pnlHiendMod.ResumeLayout(false);
            this.pnlHiendMod.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HieFilter)).EndInit();
            this.pnlAgyTbl.ResumeLayout(false);
            this.pnlAgyTbl.PerformLayout();
            this.pnlFormat.ResumeLayout(false);
            this.pnlAccessndRepList.ResumeLayout(false);
            this.pnlRepList.ResumeLayout(false);
            this.pnlRepList.PerformLayout();
            this.pnlAccess.ResumeLayout(false);
            this.pnlAccess.PerformLayout();
            this.pnlReportType.ResumeLayout(false);
            this.pnlReportType.PerformLayout();
            this.pnlCompleteForm.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Button btnPDFPreview;
        private Button btnGeneratePDF;
        private Panel pnlPDF;
        private Label lblReqFormat;
        private Label lblReqType;
        private ComboBox CmbAgyTab;
        private Label LblAgyTab;
        private Panel pnlTypes;
        private Label LblRepType;
        private ComboBox CmbRepType;
        private Label lblReqAgyTable;
        private Label LblFormat;
        private RadioButton RblEmrServ;
        private RadioButton RblCaseMgmt;
        private PictureBox HieFilter;
        private Label lblHie;
        private Label lblReqHie;
        private TextBox txtHie;
        private RadioButton rbBoth;
        private RadioButton rbStandard;
        private RadioButton rbCustom;
        private Label lblType;
        private Panel pnlType;
        private Panel pnlAccess;
        private RadioButton rdoSeletedt;
        private RadioButton rdoAll;
        private Label lblAccessType;
        private DateTimePicker dtselect;
        private Label LblUser;
        private ComboBox CmbUser;
        private Panel pnlFormat;
        private RadioButton rbTown;
        private RadioButton rbCounty;
        private Panel pnlRepList;
        private ComboBox cmbRepCode;
        private Label lblReportCode;
        private ComboBox cmbRepModule;
        private Label lblModule;
        private Label lblUserID;
        private ComboBox cmbRepUserId;
        private DateTimePicker dtRepDate;
        private RadioButton rdoRepDate;
        private RadioButton rdoRepAll;
        private Label lblRepListType;
        private DateTimePicker dtSelectToDate;
        private Label lblTodate;
        private ComboBox cmbModule;
        private Panel pnlCompleteForm;
        private Spacer spacer1;
        private Panel pnlReportType;
        private Panel pnlAccessndRepList;
        private Panel pnlHiendMod;
        private Panel pnlAgyTbl;
    }
}