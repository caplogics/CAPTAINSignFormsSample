using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class HSSB2106_ChildTrackReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSSB2106_ChildTrackReport));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.Pb_Search_Hie = new Wisej.Web.PictureBox();
            this.CmbYear = new Wisej.Web.ComboBox();
            this.Txt_HieDesc = new Wisej.Web.TextBox();
            this.pnlHie = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlParams = new Wisej.Web.Panel();
            this.panel7 = new Wisej.Web.Panel();
            this.rdoKindergartenDate = new Wisej.Web.RadioButton();
            this.rdoTodayDate = new Wisej.Web.RadioButton();
            this.lblBaseAge = new Wisej.Web.Label();
            this.panel2 = new Wisej.Web.Panel();
            this.label21 = new Wisej.Web.Label();
            this.lblChildAge = new Wisej.Web.Label();
            this.txtFrom = new Wisej.Web.TextBox();
            this.lblForm = new Wisej.Web.Label();
            this.label18 = new Wisej.Web.Label();
            this.txtTo = new Wisej.Web.TextBox();
            this.panel6 = new Wisej.Web.Panel();
            this.rbFundSel = new Wisej.Web.RadioButton();
            this.rbFundAll = new Wisej.Web.RadioButton();
            this.lblFundSource = new Wisej.Web.Label();
            this.lblActive = new Wisej.Web.Label();
            this.cmbActive = new Wisej.Web.ComboBox();
            this.panel1 = new Wisej.Web.Panel();
            this.lblSBCB = new Wisej.Web.Label();
            this.dtpSBCB = new Wisej.Web.DateTimePicker();
            this.lblEnrlStatus = new Wisej.Web.Label();
            this.cmbEnrlStatus = new Wisej.Web.ComboBox();
            this.panel5 = new Wisej.Web.Panel();
            this.rbTaskCompPY = new Wisej.Web.RadioButton();
            this.rbtaskscmplete = new Wisej.Web.RadioButton();
            this.rbAdderssed = new Wisej.Web.RadioButton();
            this.rbEvryTsk = new Wisej.Web.RadioButton();
            this.lblShowTsk = new Wisej.Web.Label();
            this.pnlTracCoCode = new Wisej.Web.Panel();
            this.rbSelTasks = new Wisej.Web.RadioButton();
            this.rbAllTasks = new Wisej.Web.RadioButton();
            this.lblTrckCompCode = new Wisej.Web.Label();
            this.CmbTrackComp = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.pnlSites = new Wisej.Web.Panel();
            this.rdoMultipleSites = new Wisej.Web.RadioButton();
            this.rdoAllSite = new Wisej.Web.RadioButton();
            this.lblSite = new Wisej.Web.Label();
            this.pnlApp = new Wisej.Web.Panel();
            this.lblAppNo = new Wisej.Web.Label();
            this.txtAppNo = new Wisej.Web.TextBox();
            this.label1 = new Wisej.Web.Label();
            this.btnBrowse = new Wisej.Web.Button();
            this.pnlRepSeq = new Wisej.Web.Panel();
            this.lblRepSeq = new Wisej.Web.Label();
            this.CmbRepSeq = new Wisej.Web.ComboBox();
            this.btnGeneratePdf = new Wisej.Web.Button();
            this.btnPdfPreview = new Wisej.Web.Button();
            this.btnPrint = new Wisej.Web.Button();
            this.btnSaveParameters = new Wisej.Web.Button();
            this.btnGetParameters = new Wisej.Web.Button();
            this.pnlGenerate = new Wisej.Web.Panel();
            this.chkbExcel = new Wisej.Web.CheckBox();
            this.spacer6 = new Wisej.Web.Spacer();
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
            this.panel7.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.pnlTracCoCode.SuspendLayout();
            this.pnlSites.SuspendLayout();
            this.pnlApp.SuspendLayout();
            this.pnlRepSeq.SuspendLayout();
            this.pnlGenerate.SuspendLayout();
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
            this.CmbYear.Location = new System.Drawing.Point(765, 0);
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
            this.Txt_HieDesc.Size = new System.Drawing.Size(750, 25);
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
            this.pnlHie.HeaderAlignment = Wisej.Web.HorizontalAlignment.Right;
            this.pnlHie.Location = new System.Drawing.Point(15, 9);
            this.pnlHie.Name = "pnlHie";
            this.pnlHie.Size = new System.Drawing.Size(830, 25);
            this.pnlHie.TabIndex = 88;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(750, 0);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(15, 25);
            // 
            // pnlParams
            // 
            this.pnlParams.Controls.Add(this.panel7);
            this.pnlParams.Controls.Add(this.panel2);
            this.pnlParams.Controls.Add(this.panel6);
            this.pnlParams.Controls.Add(this.panel1);
            this.pnlParams.Controls.Add(this.panel5);
            this.pnlParams.Controls.Add(this.pnlTracCoCode);
            this.pnlParams.Controls.Add(this.pnlSites);
            this.pnlParams.Controls.Add(this.pnlApp);
            this.pnlParams.Controls.Add(this.pnlRepSeq);
            this.pnlParams.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlParams.Location = new System.Drawing.Point(0, 43);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new System.Drawing.Size(923, 291);
            this.pnlParams.TabIndex = 1;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.rdoKindergartenDate);
            this.panel7.Controls.Add(this.rdoTodayDate);
            this.panel7.Controls.Add(this.lblBaseAge);
            this.panel7.Dock = Wisej.Web.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 256);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(923, 35);
            this.panel7.TabIndex = 9;
            // 
            // rdoKindergartenDate
            // 
            this.rdoKindergartenDate.AutoSize = false;
            this.rdoKindergartenDate.Location = new System.Drawing.Point(262, 4);
            this.rdoKindergartenDate.Name = "rdoKindergartenDate";
            this.rdoKindergartenDate.Size = new System.Drawing.Size(163, 21);
            this.rdoKindergartenDate.TabIndex = 2;
            this.rdoKindergartenDate.Text = "Kindergarten Start Date";
            // 
            // rdoTodayDate
            // 
            this.rdoTodayDate.AutoSize = false;
            this.rdoTodayDate.Checked = true;
            this.rdoTodayDate.Location = new System.Drawing.Point(147, 4);
            this.rdoTodayDate.Name = "rdoTodayDate";
            this.rdoTodayDate.Size = new System.Drawing.Size(103, 21);
            this.rdoTodayDate.TabIndex = 1;
            this.rdoTodayDate.TabStop = true;
            this.rdoTodayDate.Text = "Today\'s Date";
            // 
            // lblBaseAge
            // 
            this.lblBaseAge.Location = new System.Drawing.Point(15, 7);
            this.lblBaseAge.Name = "lblBaseAge";
            this.lblBaseAge.Size = new System.Drawing.Size(78, 16);
            this.lblBaseAge.TabIndex = 36;
            this.lblBaseAge.Text = "Base Ages On";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label21);
            this.panel2.Controls.Add(this.lblChildAge);
            this.panel2.Controls.Add(this.txtFrom);
            this.panel2.Controls.Add(this.lblForm);
            this.panel2.Controls.Add(this.label18);
            this.panel2.Controls.Add(this.txtTo);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 225);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(923, 31);
            this.panel2.TabIndex = 8;
            // 
            // label21
            // 
            this.label21.ForeColor = System.Drawing.Color.Red;
            this.label21.Location = new System.Drawing.Point(87, 5);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(8, 10);
            this.label21.TabIndex = 28;
            this.label21.Text = "*";
            this.label21.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblChildAge
            // 
            this.lblChildAge.Location = new System.Drawing.Point(15, 7);
            this.lblChildAge.Name = "lblChildAge";
            this.lblChildAge.Size = new System.Drawing.Size(74, 16);
            this.lblChildAge.TabIndex = 0;
            this.lblChildAge.Text = "Children Age";
            // 
            // txtFrom
            // 
            this.txtFrom.Location = new System.Drawing.Point(188, 3);
            this.txtFrom.MaxLength = 3;
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(34, 25);
            this.txtFrom.TabIndex = 1;
            this.txtFrom.Text = "0";
            this.txtFrom.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // lblForm
            // 
            this.lblForm.Location = new System.Drawing.Point(149, 7);
            this.lblForm.Name = "lblForm";
            this.lblForm.Size = new System.Drawing.Size(30, 16);
            this.lblForm.TabIndex = 30;
            this.lblForm.Text = "From";
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(251, 7);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(15, 16);
            this.label18.TabIndex = 30;
            this.label18.Text = "To";
            // 
            // txtTo
            // 
            this.txtTo.Location = new System.Drawing.Point(277, 3);
            this.txtTo.MaxLength = 3;
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(34, 25);
            this.txtTo.TabIndex = 2;
            this.txtTo.Text = "999";
            this.txtTo.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.rbFundSel);
            this.panel6.Controls.Add(this.rbFundAll);
            this.panel6.Controls.Add(this.lblFundSource);
            this.panel6.Controls.Add(this.lblActive);
            this.panel6.Controls.Add(this.cmbActive);
            this.panel6.Dock = Wisej.Web.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 194);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(923, 31);
            this.panel6.TabIndex = 7;
            // 
            // rbFundSel
            // 
            this.rbFundSel.AutoSize = false;
            this.rbFundSel.Location = new System.Drawing.Point(713, 4);
            this.rbFundSel.Name = "rbFundSel";
            this.rbFundSel.Size = new System.Drawing.Size(79, 21);
            this.rbFundSel.TabIndex = 3;
            this.rbFundSel.Text = "Selected";
            this.rbFundSel.Click += new System.EventHandler(this.rbFundSel_Click);
            // 
            // rbFundAll
            // 
            this.rbFundAll.AutoSize = false;
            this.rbFundAll.Checked = true;
            this.rbFundAll.Location = new System.Drawing.Point(652, 4);
            this.rbFundAll.Name = "rbFundAll";
            this.rbFundAll.Size = new System.Drawing.Size(45, 21);
            this.rbFundAll.TabIndex = 2;
            this.rbFundAll.TabStop = true;
            this.rbFundAll.Text = "All";
            // 
            // lblFundSource
            // 
            this.lblFundSource.Location = new System.Drawing.Point(549, 7);
            this.lblFundSource.Name = "lblFundSource";
            this.lblFundSource.Size = new System.Drawing.Size(90, 16);
            this.lblFundSource.TabIndex = 36;
            this.lblFundSource.Text = "Funding Source";
            // 
            // lblActive
            // 
            this.lblActive.Location = new System.Drawing.Point(15, 7);
            this.lblActive.Name = "lblActive";
            this.lblActive.Size = new System.Drawing.Size(84, 16);
            this.lblActive.TabIndex = 0;
            this.lblActive.Text = "Active/Inactive";
            // 
            // cmbActive
            // 
            this.cmbActive.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbActive.FormattingEnabled = true;
            this.cmbActive.Location = new System.Drawing.Point(150, 3);
            this.cmbActive.Name = "cmbActive";
            this.cmbActive.Size = new System.Drawing.Size(116, 25);
            this.cmbActive.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblSBCB);
            this.panel1.Controls.Add(this.dtpSBCB);
            this.panel1.Controls.Add(this.lblEnrlStatus);
            this.panel1.Controls.Add(this.cmbEnrlStatus);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 163);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(923, 31);
            this.panel1.TabIndex = 6;
            // 
            // lblSBCB
            // 
            this.lblSBCB.Location = new System.Drawing.Point(15, 7);
            this.lblSBCB.Name = "lblSBCB";
            this.lblSBCB.Size = new System.Drawing.Size(108, 16);
            this.lblSBCB.TabIndex = 0;
            this.lblSBCB.Text = "SBCB Dates Before";
            // 
            // dtpSBCB
            // 
            this.dtpSBCB.AutoSize = false;
            this.dtpSBCB.Checked = false;
            this.dtpSBCB.CustomFormat = "MM/dd/yyyy";
            this.dtpSBCB.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpSBCB.Location = new System.Drawing.Point(150, 3);
            this.dtpSBCB.Name = "dtpSBCB";
            this.dtpSBCB.ShowCheckBox = true;
            this.dtpSBCB.ShowToolTips = false;
            this.dtpSBCB.Size = new System.Drawing.Size(116, 25);
            this.dtpSBCB.TabIndex = 1;
            // 
            // lblEnrlStatus
            // 
            this.lblEnrlStatus.Location = new System.Drawing.Point(549, 7);
            this.lblEnrlStatus.Name = "lblEnrlStatus";
            this.lblEnrlStatus.Size = new System.Drawing.Size(73, 16);
            this.lblEnrlStatus.TabIndex = 0;
            this.lblEnrlStatus.Text = "Enroll Status";
            // 
            // cmbEnrlStatus
            // 
            this.cmbEnrlStatus.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbEnrlStatus.FormattingEnabled = true;
            this.cmbEnrlStatus.Location = new System.Drawing.Point(656, 3);
            this.cmbEnrlStatus.Name = "cmbEnrlStatus";
            this.cmbEnrlStatus.Size = new System.Drawing.Size(131, 25);
            this.cmbEnrlStatus.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.rbTaskCompPY);
            this.panel5.Controls.Add(this.rbtaskscmplete);
            this.panel5.Controls.Add(this.rbAdderssed);
            this.panel5.Controls.Add(this.rbEvryTsk);
            this.panel5.Controls.Add(this.lblShowTsk);
            this.panel5.Dock = Wisej.Web.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 132);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(923, 31);
            this.panel5.TabIndex = 5;
            // 
            // rbTaskCompPY
            // 
            this.rbTaskCompPY.AutoSize = false;
            this.rbTaskCompPY.Location = new System.Drawing.Point(652, 4);
            this.rbTaskCompPY.Name = "rbTaskCompPY";
            this.rbTaskCompPY.Size = new System.Drawing.Size(234, 21);
            this.rbTaskCompPY.TabIndex = 4;
            this.rbTaskCompPY.Text = "Only Tasks completed during this PY";
            // 
            // rbtaskscmplete
            // 
            this.rbtaskscmplete.AutoSize = false;
            this.rbtaskscmplete.Checked = true;
            this.rbtaskscmplete.Location = new System.Drawing.Point(147, 4);
            this.rbtaskscmplete.Name = "rbtaskscmplete";
            this.rbtaskscmplete.Size = new System.Drawing.Size(240, 21);
            this.rbtaskscmplete.TabIndex = 1;
            this.rbtaskscmplete.TabStop = true;
            this.rbtaskscmplete.Text = "Only Tasks that have been completed";
            // 
            // rbAdderssed
            // 
            this.rbAdderssed.AutoSize = false;
            this.rbAdderssed.Location = new System.Drawing.Point(405, 4);
            this.rbAdderssed.Name = "rbAdderssed";
            this.rbAdderssed.Size = new System.Drawing.Size(122, 21);
            this.rbAdderssed.TabIndex = 2;
            this.rbAdderssed.Text = "To be Addressed";
            // 
            // rbEvryTsk
            // 
            this.rbEvryTsk.AutoSize = false;
            this.rbEvryTsk.Location = new System.Drawing.Point(546, 4);
            this.rbEvryTsk.Name = "rbEvryTsk";
            this.rbEvryTsk.Size = new System.Drawing.Size(92, 21);
            this.rbEvryTsk.TabIndex = 3;
            this.rbEvryTsk.Text = "Every Task";
            // 
            // lblShowTsk
            // 
            this.lblShowTsk.Location = new System.Drawing.Point(15, 7);
            this.lblShowTsk.Name = "lblShowTsk";
            this.lblShowTsk.Size = new System.Drawing.Size(61, 16);
            this.lblShowTsk.TabIndex = 36;
            this.lblShowTsk.Text = "Show Task";
            // 
            // pnlTracCoCode
            // 
            this.pnlTracCoCode.Controls.Add(this.rbSelTasks);
            this.pnlTracCoCode.Controls.Add(this.rbAllTasks);
            this.pnlTracCoCode.Controls.Add(this.lblTrckCompCode);
            this.pnlTracCoCode.Controls.Add(this.CmbTrackComp);
            this.pnlTracCoCode.Dock = Wisej.Web.DockStyle.Top;
            this.pnlTracCoCode.Location = new System.Drawing.Point(0, 101);
            this.pnlTracCoCode.Name = "pnlTracCoCode";
            this.pnlTracCoCode.Size = new System.Drawing.Size(923, 31);
            this.pnlTracCoCode.TabIndex = 4;
            // 
            // rbSelTasks
            // 
            this.rbSelTasks.AutoSize = false;
            this.rbSelTasks.Location = new System.Drawing.Point(652, 4);
            this.rbSelTasks.Name = "rbSelTasks";
            this.rbSelTasks.Size = new System.Drawing.Size(111, 21);
            this.rbSelTasks.TabIndex = 3;
            this.rbSelTasks.Text = "Selected Tasks";
            this.rbSelTasks.Click += new System.EventHandler(this.rbSelTasks_Click);
            // 
            // rbAllTasks
            // 
            this.rbAllTasks.AutoSize = false;
            this.rbAllTasks.Checked = true;
            this.rbAllTasks.Location = new System.Drawing.Point(546, 4);
            this.rbAllTasks.Name = "rbAllTasks";
            this.rbAllTasks.Size = new System.Drawing.Size(81, 21);
            this.rbAllTasks.TabIndex = 2;
            this.rbAllTasks.TabStop = true;
            this.rbAllTasks.Text = "All Tasks";
            // 
            // lblTrckCompCode
            // 
            this.lblTrckCompCode.Location = new System.Drawing.Point(15, 7);
            this.lblTrckCompCode.Name = "lblTrckCompCode";
            this.lblTrckCompCode.Size = new System.Drawing.Size(118, 16);
            this.lblTrckCompCode.TabIndex = 0;
            this.lblTrckCompCode.Text = "Tracking Comp Code";
            // 
            // CmbTrackComp
            // 
            this.CmbTrackComp.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbTrackComp.FormattingEnabled = true;
            this.CmbTrackComp.Location = new System.Drawing.Point(150, 3);
            this.CmbTrackComp.Name = "CmbTrackComp";
            this.CmbTrackComp.Size = new System.Drawing.Size(263, 25);
            this.CmbTrackComp.TabIndex = 1;
            this.CmbTrackComp.SelectedIndexChanged += new System.EventHandler(this.CmbTrackComp_SelectedIndexChanged);
            // 
            // pnlSites
            // 
            this.pnlSites.Controls.Add(this.rdoMultipleSites);
            this.pnlSites.Controls.Add(this.rdoAllSite);
            this.pnlSites.Controls.Add(this.lblSite);
            this.pnlSites.Dock = Wisej.Web.DockStyle.Top;
            this.pnlSites.Location = new System.Drawing.Point(0, 70);
            this.pnlSites.Name = "pnlSites";
            this.pnlSites.Size = new System.Drawing.Size(923, 31);
            this.pnlSites.TabIndex = 3;
            // 
            // rdoMultipleSites
            // 
            this.rdoMultipleSites.AutoSize = false;
            this.rdoMultipleSites.Location = new System.Drawing.Point(262, 4);
            this.rdoMultipleSites.Name = "rdoMultipleSites";
            this.rdoMultipleSites.Size = new System.Drawing.Size(151, 21);
            this.rdoMultipleSites.TabIndex = 2;
            this.rdoMultipleSites.Text = "Multiple Sites/Classes";
            this.rdoMultipleSites.Click += new System.EventHandler(this.rdoMultipleSites_Click);
            // 
            // rdoAllSite
            // 
            this.rdoAllSite.AutoSize = false;
            this.rdoAllSite.Checked = true;
            this.rdoAllSite.Location = new System.Drawing.Point(147, 4);
            this.rdoAllSite.Name = "rdoAllSite";
            this.rdoAllSite.Size = new System.Drawing.Size(97, 21);
            this.rdoAllSite.TabIndex = 1;
            this.rdoAllSite.TabStop = true;
            this.rdoAllSite.Text = "For All Sites";
            this.rdoAllSite.Click += new System.EventHandler(this.rdoMultipleSites_Click);
            // 
            // lblSite
            // 
            this.lblSite.Location = new System.Drawing.Point(15, 7);
            this.lblSite.Name = "lblSite";
            this.lblSite.Size = new System.Drawing.Size(31, 16);
            this.lblSite.TabIndex = 36;
            this.lblSite.Text = "Sites";
            // 
            // pnlApp
            // 
            this.pnlApp.Controls.Add(this.lblAppNo);
            this.pnlApp.Controls.Add(this.txtAppNo);
            this.pnlApp.Controls.Add(this.label1);
            this.pnlApp.Controls.Add(this.btnBrowse);
            this.pnlApp.Dock = Wisej.Web.DockStyle.Top;
            this.pnlApp.Location = new System.Drawing.Point(0, 39);
            this.pnlApp.Name = "pnlApp";
            this.pnlApp.Size = new System.Drawing.Size(923, 31);
            this.pnlApp.TabIndex = 2;
            // 
            // lblAppNo
            // 
            this.lblAppNo.Location = new System.Drawing.Point(15, 7);
            this.lblAppNo.Name = "lblAppNo";
            this.lblAppNo.Size = new System.Drawing.Size(104, 16);
            this.lblAppNo.TabIndex = 0;
            this.lblAppNo.Text = "Applicant Number";
            // 
            // txtAppNo
            // 
            this.txtAppNo.Enabled = false;
            this.txtAppNo.Location = new System.Drawing.Point(150, 3);
            this.txtAppNo.MaxLength = 8;
            this.txtAppNo.Name = "txtAppNo";
            this.txtAppNo.Size = new System.Drawing.Size(100, 25);
            this.txtAppNo.TabIndex = 1;
            this.txtAppNo.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtAppNo.Leave += new System.EventHandler(this.txtAppNo_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(118, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(9, 14);
            this.label1.TabIndex = 28;
            this.label1.Text = "*";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.Visible = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(266, 3);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(65, 25);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "&Browse";
            this.btnBrowse.ToolTipText = "Select Applicant";
            this.btnBrowse.Visible = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // pnlRepSeq
            // 
            this.pnlRepSeq.Controls.Add(this.lblRepSeq);
            this.pnlRepSeq.Controls.Add(this.CmbRepSeq);
            this.pnlRepSeq.Dock = Wisej.Web.DockStyle.Top;
            this.pnlRepSeq.Location = new System.Drawing.Point(0, 0);
            this.pnlRepSeq.Name = "pnlRepSeq";
            this.pnlRepSeq.Size = new System.Drawing.Size(923, 39);
            this.pnlRepSeq.TabIndex = 1;
            // 
            // lblRepSeq
            // 
            this.lblRepSeq.Location = new System.Drawing.Point(15, 15);
            this.lblRepSeq.Name = "lblRepSeq";
            this.lblRepSeq.Size = new System.Drawing.Size(98, 16);
            this.lblRepSeq.TabIndex = 0;
            this.lblRepSeq.Text = "Report Sequence";
            // 
            // CmbRepSeq
            // 
            this.CmbRepSeq.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbRepSeq.FormattingEnabled = true;
            this.CmbRepSeq.Location = new System.Drawing.Point(150, 11);
            this.CmbRepSeq.Name = "CmbRepSeq";
            this.CmbRepSeq.Size = new System.Drawing.Size(181, 25);
            this.CmbRepSeq.TabIndex = 1;
            this.CmbRepSeq.SelectedIndexChanged += new System.EventHandler(this.CmbRepSeq_SelectedIndexChanged);
            // 
            // btnGeneratePdf
            // 
            this.btnGeneratePdf.AppearanceKey = "button-reports";
            this.btnGeneratePdf.Dock = Wisej.Web.DockStyle.Right;
            this.btnGeneratePdf.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnGeneratePdf.Location = new System.Drawing.Point(735, 5);
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
            this.btnPdfPreview.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnPdfPreview.Location = new System.Drawing.Point(823, 5);
            this.btnPdfPreview.Name = "btnPdfPreview";
            this.btnPdfPreview.Size = new System.Drawing.Size(85, 25);
            this.btnPdfPreview.TabIndex = 5;
            this.btnPdfPreview.Text = "Pre&view";
            this.btnPdfPreview.Click += new System.EventHandler(this.btnPdfPreview_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.AppearanceKey = "button-reports";
            this.btnPrint.Dock = Wisej.Web.DockStyle.Right;
            this.btnPrint.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnPrint.Location = new System.Drawing.Point(672, 5);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(60, 25);
            this.btnPrint.TabIndex = 3;
            this.btnPrint.Text = "Prin&t";
            this.btnPrint.Visible = false;
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
            // pnlGenerate
            // 
            this.pnlGenerate.AppearanceKey = "panel-grdo";
            this.pnlGenerate.Controls.Add(this.chkbExcel);
            this.pnlGenerate.Controls.Add(this.spacer6);
            this.pnlGenerate.Controls.Add(this.btnSaveParameters);
            this.pnlGenerate.Controls.Add(this.spacer5);
            this.pnlGenerate.Controls.Add(this.btnPrint);
            this.pnlGenerate.Controls.Add(this.spacer4);
            this.pnlGenerate.Controls.Add(this.btnGeneratePdf);
            this.pnlGenerate.Controls.Add(this.spacer3);
            this.pnlGenerate.Controls.Add(this.btnPdfPreview);
            this.pnlGenerate.Controls.Add(this.btnGetParameters);
            this.pnlGenerate.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlGenerate.Location = new System.Drawing.Point(0, 334);
            this.pnlGenerate.Name = "pnlGenerate";
            this.pnlGenerate.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlGenerate.Size = new System.Drawing.Size(923, 35);
            this.pnlGenerate.TabIndex = 2;
            // 
            // chkbExcel
            // 
            this.chkbExcel.Dock = Wisej.Web.DockStyle.Left;
            this.chkbExcel.Location = new System.Drawing.Point(387, 5);
            this.chkbExcel.Name = "chkbExcel";
            this.chkbExcel.Size = new System.Drawing.Size(115, 25);
            this.chkbExcel.TabIndex = 6;
            this.chkbExcel.Text = "Generate Excel";
            // 
            // spacer6
            // 
            this.spacer6.Dock = Wisej.Web.DockStyle.Left;
            this.spacer6.Location = new System.Drawing.Point(238, 5);
            this.spacer6.Name = "spacer6";
            this.spacer6.Size = new System.Drawing.Size(149, 25);
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
            this.spacer4.Location = new System.Drawing.Point(732, 5);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(3, 25);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Right;
            this.spacer3.Location = new System.Drawing.Point(820, 5);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(3, 25);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlParams);
            this.pnlCompleteForm.Controls.Add(this.pnlHieFilter);
            this.pnlCompleteForm.Controls.Add(this.pnlGenerate);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(923, 369);
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
            this.pnlHieFilter.Size = new System.Drawing.Size(923, 43);
            this.pnlHieFilter.TabIndex = 99;
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.Pb_Search_Hie);
            this.pnlFilter.Controls.Add(this.spacer2);
            this.pnlFilter.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlFilter.Location = new System.Drawing.Point(845, 9);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(69, 25);
            this.pnlFilter.TabIndex = 55;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(0, 0);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(15, 25);
            // 
            // HSSB2106_ChildTrackReport
            // 
            this.ClientSize = new System.Drawing.Size(923, 369);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSSB2106_ChildTrackReport";
            this.Text = "HSSB2106_ChildTrackReport";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "tlHelp";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.HSSB2106_ChildTrackReport_ToolClick);
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).EndInit();
            this.pnlHie.ResumeLayout(false);
            this.pnlHie.PerformLayout();
            this.pnlParams.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.pnlTracCoCode.ResumeLayout(false);
            this.pnlTracCoCode.PerformLayout();
            this.pnlSites.ResumeLayout(false);
            this.pnlApp.ResumeLayout(false);
            this.pnlApp.PerformLayout();
            this.pnlRepSeq.ResumeLayout(false);
            this.pnlRepSeq.PerformLayout();
            this.pnlGenerate.ResumeLayout(false);
            this.pnlGenerate.PerformLayout();
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
        private TextBox txtAppNo;
        private Label lblAppNo;
        private ComboBox CmbRepSeq;
        private Label lblRepSeq;
        private Button btnBrowse;
        private Label lblSite;
        private Panel pnlSites;
        private RadioButton rdoMultipleSites;
        private RadioButton rdoAllSite;
        private Label lblTrckCompCode;
        private ComboBoxEx CmbTrackComp;
        private Panel pnlTracCoCode;
        private RadioButton rbSelTasks;
        private RadioButton rbAllTasks;
        private Panel panel5;
        private RadioButton rbAdderssed;
        private RadioButton rbEvryTsk;
        private Label lblShowTsk;
        private DateTimePicker dtpSBCB;
        private Label lblSBCB;
        private ComboBox cmbActive;
        private Label lblActive;
        private ComboBox cmbEnrlStatus;
        private Label lblEnrlStatus;
        private Label lblFundSource;
        private Panel panel6;
        private RadioButton rbFundSel;
        private RadioButton rbFundAll;
        private TextBox txtTo;
        private Label label18;
        private Label lblForm;
        private TextBox txtFrom;
        private Label lblChildAge;
        private Label label21;
        private Label lblBaseAge;
        private Panel panel7;
        private RadioButton rdoKindergartenDate;
        private RadioButton rdoTodayDate;
        private Button btnGeneratePdf;
        private Button btnPdfPreview;
        private Button btnPrint;
        private Button btnSaveParameters;
        private Button btnGetParameters;
        private Panel pnlGenerate;
        private RadioButton rbtaskscmplete;
        private Label label1;
        private RadioButton rbTaskCompPY;
        private Panel pnlCompleteForm;
        private Panel pnlHieFilter;
        private Spacer spacer1;
        private Panel pnlFilter;
        private Spacer spacer2;
        private Spacer spacer4;
        private Spacer spacer3;
        private Spacer spacer5;
        private Panel pnlRepSeq;
        private Panel pnlApp;
        private Panel panel1;
        private Panel panel2;
        private CheckBox chkbExcel;
        private Spacer spacer6;
    }
}