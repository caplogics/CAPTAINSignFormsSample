using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
namespace Captain.Common.Views.Forms
{
    partial class HSSB2103ReportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSSB2103ReportForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.btnGeneratePdf = new Wisej.Web.Button();
            this.rdoSelectask = new Wisej.Web.RadioButton();
            this.rdoKindergartenDate = new Wisej.Web.RadioButton();
            this.btnPdfPreview = new Wisej.Web.Button();
            this.rdoClass = new Wisej.Web.RadioButton();
            this.cmbMaxoftask = new Wisej.Web.ComboBox();
            this.rdoOnlyLutd = new Wisej.Web.RadioButton();
            this.label21 = new Wisej.Web.Label();
            this.lblChildAge = new Wisej.Web.Label();
            this.lblApplicantReq = new Wisej.Web.Label();
            this.lblForm = new Wisej.Web.Label();
            this.label18 = new Wisej.Web.Label();
            this.rdoZipAll = new Wisej.Web.RadioButton();
            this.rdoSkipSelected = new Wisej.Web.RadioButton();
            this.rdoZipSelected = new Wisej.Web.RadioButton();
            this.pnlZIP = new Wisej.Web.Panel();
            this.label3 = new Wisej.Web.Label();
            this.label16 = new Wisej.Web.Label();
            this.rdoBoth = new Wisej.Web.RadioButton();
            this.pnlChildWith = new Wisej.Web.Panel();
            this.rdoNoTask = new Wisej.Web.RadioButton();
            this.lblcompTask = new Wisej.Web.Label();
            this.txtCompleTask = new Wisej.Web.TextBox();
            this.rdoFundingAll = new Wisej.Web.RadioButton();
            this.rdoFundingSelect = new Wisej.Web.RadioButton();
            this.pnlFunds = new Wisej.Web.Panel();
            this.label14 = new Wisej.Web.Label();
            this.rdoTodayDate = new Wisej.Web.RadioButton();
            this.pnlBaseAge = new Wisej.Web.Panel();
            this.label13 = new Wisej.Web.Label();
            this.cmbActive = new Wisej.Web.ComboBox();
            this.label12 = new Wisej.Web.Label();
            this.btnTasks = new Wisej.Web.Button();
            this.label1 = new Wisej.Web.Label();
            this.lblShow = new Wisej.Web.Label();
            this.rdoMultipleSites = new Wisej.Web.RadioButton();
            this.rdoAllSite = new Wisej.Web.RadioButton();
            this.pnlSites = new Wisej.Web.Panel();
            this.label4 = new Wisej.Web.Label();
            this.CmbYear = new Wisej.Web.ComboBox();
            this.Txt_HieDesc = new Wisej.Web.TextBox();
            this.pnlHie = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.Pb_Search_Hie = new Wisej.Web.PictureBox();
            this.btnPrint = new Wisej.Web.Button();
            this.btnSaveParameters = new Wisej.Web.Button();
            this.btnGetParameters = new Wisej.Web.Button();
            this.pnlGenerate = new Wisej.Web.Panel();
            this.spacer5 = new Wisej.Web.Spacer();
            this.spacer4 = new Wisej.Web.Spacer();
            this.spacer3 = new Wisej.Web.Spacer();
            this.rdoAlltaskDate = new Wisej.Web.RadioButton();
            this.pnAbsentReason = new Wisej.Web.Panel();
            this.label6 = new Wisej.Web.Label();
            this.rdoReportChildName = new Wisej.Web.RadioButton();
            this.rdoApplicant = new Wisej.Web.RadioButton();
            this.pnlRepSeq = new Wisej.Web.Panel();
            this.rdoChildren = new Wisej.Web.RadioButton();
            this.rdoWaitList = new Wisej.Web.RadioButton();
            this.rdoEnrolled = new Wisej.Web.RadioButton();
            this.pnlEnroll = new Wisej.Web.Panel();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlParams = new Wisej.Web.Panel();
            this.pnlEnrolFS = new Wisej.Web.Panel();
            this.pnlShwSites = new Wisej.Web.Panel();
            this.pnlBaseAgZIP = new Wisej.Web.Panel();
            this.pnlChildAge = new Wisej.Web.Panel();
            this.txtTo = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtFrom = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.pnlTask = new Wisej.Web.Panel();
            this.chkIncludeNotes = new Wisej.Web.CheckBox();
            this.pnlRepType = new Wisej.Web.Panel();
            this.txtApplicant = new Wisej.Web.TextBox();
            this.btnBrowse = new Wisej.Web.Button();
            this.label2 = new Wisej.Web.Label();
            this.label5 = new Wisej.Web.Label();
            this.rdoSelectApplicant = new Wisej.Web.RadioButton();
            this.rdoAllAplicants = new Wisej.Web.RadioButton();
            this.pnlHieFilter = new Wisej.Web.Panel();
            this.pnlFilter = new Wisej.Web.Panel();
            this.spacer2 = new Wisej.Web.Spacer();
            this.pnlZIP.SuspendLayout();
            this.pnlChildWith.SuspendLayout();
            this.pnlFunds.SuspendLayout();
            this.pnlBaseAge.SuspendLayout();
            this.pnlSites.SuspendLayout();
            this.pnlHie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).BeginInit();
            this.pnlGenerate.SuspendLayout();
            this.pnAbsentReason.SuspendLayout();
            this.pnlRepSeq.SuspendLayout();
            this.pnlEnroll.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlParams.SuspendLayout();
            this.pnlEnrolFS.SuspendLayout();
            this.pnlShwSites.SuspendLayout();
            this.pnlBaseAgZIP.SuspendLayout();
            this.pnlChildAge.SuspendLayout();
            this.pnlTask.SuspendLayout();
            this.pnlRepType.SuspendLayout();
            this.pnlHieFilter.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGeneratePdf
            // 
            this.btnGeneratePdf.AppearanceKey = "button-reports";
            this.btnGeneratePdf.Dock = Wisej.Web.DockStyle.Right;
            this.btnGeneratePdf.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnGeneratePdf.Location = new System.Drawing.Point(756, 5);
            this.btnGeneratePdf.Name = "btnGeneratePdf";
            this.btnGeneratePdf.Size = new System.Drawing.Size(85, 25);
            this.btnGeneratePdf.TabIndex = 4;
            this.btnGeneratePdf.Text = "&Generate";
            this.btnGeneratePdf.Click += new System.EventHandler(this.btnGeneratePdf_Click);
            // 
            // rdoSelectask
            // 
            this.rdoSelectask.AutoSize = false;
            this.rdoSelectask.Location = new System.Drawing.Point(215, 4);
            this.rdoSelectask.Name = "rdoSelectask";
            this.rdoSelectask.Size = new System.Drawing.Size(110, 21);
            this.rdoSelectask.TabIndex = 2;
            this.rdoSelectask.Text = "Selected Tasks";
            this.rdoSelectask.CheckedChanged += new System.EventHandler(this.rdoSelectask_CheckedChanged);
            // 
            // rdoKindergartenDate
            // 
            this.rdoKindergartenDate.AutoSize = false;
            this.rdoKindergartenDate.Location = new System.Drawing.Point(269, 4);
            this.rdoKindergartenDate.Name = "rdoKindergartenDate";
            this.rdoKindergartenDate.Size = new System.Drawing.Size(162, 21);
            this.rdoKindergartenDate.TabIndex = 2;
            this.rdoKindergartenDate.Text = "Kindergarten Start Date";
            // 
            // btnPdfPreview
            // 
            this.btnPdfPreview.AppearanceKey = "button-reports";
            this.btnPdfPreview.Dock = Wisej.Web.DockStyle.Right;
            this.btnPdfPreview.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnPdfPreview.Location = new System.Drawing.Point(844, 5);
            this.btnPdfPreview.Name = "btnPdfPreview";
            this.btnPdfPreview.Size = new System.Drawing.Size(85, 25);
            this.btnPdfPreview.TabIndex = 5;
            this.btnPdfPreview.Text = "Pre&view";
            this.btnPdfPreview.Click += new System.EventHandler(this.btnPdfPreview_Click);
            // 
            // rdoClass
            // 
            this.rdoClass.AutoSize = false;
            this.rdoClass.Location = new System.Drawing.Point(361, 12);
            this.rdoClass.Name = "rdoClass";
            this.rdoClass.Size = new System.Drawing.Size(55, 21);
            this.rdoClass.TabIndex = 3;
            this.rdoClass.Text = "Class";
            // 
            // cmbMaxoftask
            // 
            this.cmbMaxoftask.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbMaxoftask.FormattingEnabled = true;
            this.cmbMaxoftask.Location = new System.Drawing.Point(145, 3);
            this.cmbMaxoftask.Name = "cmbMaxoftask";
            this.cmbMaxoftask.Size = new System.Drawing.Size(197, 25);
            this.cmbMaxoftask.TabIndex = 1;
            this.cmbMaxoftask.SelectedIndexChanged += new System.EventHandler(this.cmbMaxoftask_SelectedIndexChanged);
            // 
            // rdoOnlyLutd
            // 
            this.rdoOnlyLutd.AutoSize = false;
            this.rdoOnlyLutd.Checked = true;
            this.rdoOnlyLutd.Location = new System.Drawing.Point(142, 4);
            this.rdoOnlyLutd.Name = "rdoOnlyLutd";
            this.rdoOnlyLutd.Size = new System.Drawing.Size(198, 21);
            this.rdoOnlyLutd.TabIndex = 1;
            this.rdoOnlyLutd.TabStop = true;
            this.rdoOnlyLutd.Text = "Only Last Updated Task Dates";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.ForeColor = System.Drawing.Color.Red;
            this.label21.Location = new System.Drawing.Point(87, 5);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(9, 14);
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
            // lblApplicantReq
            // 
            this.lblApplicantReq.AutoSize = true;
            this.lblApplicantReq.ForeColor = System.Drawing.Color.Red;
            this.lblApplicantReq.Location = new System.Drawing.Point(102, 4);
            this.lblApplicantReq.Name = "lblApplicantReq";
            this.lblApplicantReq.Size = new System.Drawing.Size(9, 14);
            this.lblApplicantReq.TabIndex = 28;
            this.lblApplicantReq.Text = "*";
            this.lblApplicantReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblApplicantReq.Visible = false;
            // 
            // lblForm
            // 
            this.lblForm.Location = new System.Drawing.Point(145, 7);
            this.lblForm.Name = "lblForm";
            this.lblForm.Size = new System.Drawing.Size(32, 16);
            this.lblForm.TabIndex = 30;
            this.lblForm.Text = "From";
            this.lblForm.Visible = false;
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(256, 7);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(15, 16);
            this.label18.TabIndex = 30;
            this.label18.Text = "To";
            // 
            // rdoZipAll
            // 
            this.rdoZipAll.AutoSize = false;
            this.rdoZipAll.Checked = true;
            this.rdoZipAll.Location = new System.Drawing.Point(120, 4);
            this.rdoZipAll.Name = "rdoZipAll";
            this.rdoZipAll.Size = new System.Drawing.Size(40, 21);
            this.rdoZipAll.TabIndex = 1;
            this.rdoZipAll.TabStop = true;
            this.rdoZipAll.Text = "All";
            this.rdoZipAll.CheckedChanged += new System.EventHandler(this.rdoZipAll_CheckedChanged);
            // 
            // rdoSkipSelected
            // 
            this.rdoSkipSelected.AutoSize = false;
            this.rdoSkipSelected.Location = new System.Drawing.Point(277, 4);
            this.rdoSkipSelected.Name = "rdoSkipSelected";
            this.rdoSkipSelected.Size = new System.Drawing.Size(102, 21);
            this.rdoSkipSelected.TabIndex = 3;
            this.rdoSkipSelected.Text = "Skip Selected";
            this.rdoSkipSelected.CheckedChanged += new System.EventHandler(this.rdoSkipSelected_CheckedChanged);
            this.rdoSkipSelected.Click += new System.EventHandler(this.rdoSkipSelected_Click);
            // 
            // rdoZipSelected
            // 
            this.rdoZipSelected.AutoSize = false;
            this.rdoZipSelected.Location = new System.Drawing.Point(180, 4);
            this.rdoZipSelected.Name = "rdoZipSelected";
            this.rdoZipSelected.Size = new System.Drawing.Size(77, 21);
            this.rdoZipSelected.TabIndex = 2;
            this.rdoZipSelected.Text = "Selected";
            this.rdoZipSelected.CheckedChanged += new System.EventHandler(this.rdoZipSelected_CheckedChanged);
            this.rdoZipSelected.Click += new System.EventHandler(this.rdoZipSelected_Click);
            // 
            // pnlZIP
            // 
            this.pnlZIP.Controls.Add(this.label3);
            this.pnlZIP.Controls.Add(this.rdoZipAll);
            this.pnlZIP.Controls.Add(this.rdoSkipSelected);
            this.pnlZIP.Controls.Add(this.rdoZipSelected);
            this.pnlZIP.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlZIP.Location = new System.Drawing.Point(473, 0);
            this.pnlZIP.Name = "pnlZIP";
            this.pnlZIP.Size = new System.Drawing.Size(471, 31);
            this.pnlZIP.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 16);
            this.label3.TabIndex = 44;
            this.label3.Text = "ZIP Codes";
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(15, 7);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(78, 16);
            this.label16.TabIndex = 5;
            this.label16.Text = "Children With";
            // 
            // rdoBoth
            // 
            this.rdoBoth.AutoSize = false;
            this.rdoBoth.Checked = true;
            this.rdoBoth.Location = new System.Drawing.Point(142, 4);
            this.rdoBoth.Name = "rdoBoth";
            this.rdoBoth.Size = new System.Drawing.Size(54, 21);
            this.rdoBoth.TabIndex = 1;
            this.rdoBoth.TabStop = true;
            this.rdoBoth.Text = "Both";
            // 
            // pnlChildWith
            // 
            this.pnlChildWith.Controls.Add(this.rdoNoTask);
            this.pnlChildWith.Controls.Add(this.rdoSelectask);
            this.pnlChildWith.Controls.Add(this.lblcompTask);
            this.pnlChildWith.Controls.Add(this.rdoBoth);
            this.pnlChildWith.Controls.Add(this.label16);
            this.pnlChildWith.Controls.Add(this.txtCompleTask);
            this.pnlChildWith.Dock = Wisej.Web.DockStyle.Top;
            this.pnlChildWith.Location = new System.Drawing.Point(0, 70);
            this.pnlChildWith.Name = "pnlChildWith";
            this.pnlChildWith.Size = new System.Drawing.Size(944, 31);
            this.pnlChildWith.TabIndex = 3;
            // 
            // rdoNoTask
            // 
            this.rdoNoTask.AutoSize = false;
            this.rdoNoTask.Location = new System.Drawing.Point(345, 4);
            this.rdoNoTask.Name = "rdoNoTask";
            this.rdoNoTask.Size = new System.Drawing.Size(80, 21);
            this.rdoNoTask.TabIndex = 3;
            this.rdoNoTask.Text = "No Tasks";
            // 
            // lblcompTask
            // 
            this.lblcompTask.Location = new System.Drawing.Point(483, 7);
            this.lblcompTask.Name = "lblcompTask";
            this.lblcompTask.Size = new System.Drawing.Size(241, 16);
            this.lblcompTask.TabIndex = 43;
            this.lblcompTask.Text = "Completed/ Not Completed Tasks for Year";
            this.lblcompTask.Visible = false;
            // 
            // txtCompleTask
            // 
            this.txtCompleTask.Enabled = false;
            this.txtCompleTask.Location = new System.Drawing.Point(743, 3);
            this.txtCompleTask.MaxLength = 4;
            this.txtCompleTask.Name = "txtCompleTask";
            this.txtCompleTask.Size = new System.Drawing.Size(45, 25);
            this.txtCompleTask.TabIndex = 4;
            this.txtCompleTask.Text = "****";
            this.txtCompleTask.Visible = false;
            this.txtCompleTask.Leave += new System.EventHandler(this.txtCompleTask_Leave);
            // 
            // rdoFundingAll
            // 
            this.rdoFundingAll.AutoSize = false;
            this.rdoFundingAll.Checked = true;
            this.rdoFundingAll.Location = new System.Drawing.Point(120, 4);
            this.rdoFundingAll.Name = "rdoFundingAll";
            this.rdoFundingAll.Size = new System.Drawing.Size(41, 21);
            this.rdoFundingAll.TabIndex = 1;
            this.rdoFundingAll.TabStop = true;
            this.rdoFundingAll.Text = "All";
            this.rdoFundingAll.CheckedChanged += new System.EventHandler(this.rdoFundingAll_CheckedChanged);
            // 
            // rdoFundingSelect
            // 
            this.rdoFundingSelect.AutoSize = false;
            this.rdoFundingSelect.Location = new System.Drawing.Point(180, 4);
            this.rdoFundingSelect.Name = "rdoFundingSelect";
            this.rdoFundingSelect.Size = new System.Drawing.Size(125, 21);
            this.rdoFundingSelect.TabIndex = 2;
            this.rdoFundingSelect.Text = "Selected Funding";
            this.rdoFundingSelect.Click += new System.EventHandler(this.rdoFundingSelect_Click);
            // 
            // pnlFunds
            // 
            this.pnlFunds.Controls.Add(this.rdoFundingAll);
            this.pnlFunds.Controls.Add(this.rdoFundingSelect);
            this.pnlFunds.Controls.Add(this.label14);
            this.pnlFunds.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlFunds.Location = new System.Drawing.Point(473, 0);
            this.pnlFunds.Name = "pnlFunds";
            this.pnlFunds.Size = new System.Drawing.Size(471, 37);
            this.pnlFunds.TabIndex = 2;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(10, 7);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(95, 16);
            this.label14.TabIndex = 37;
            this.label14.Text = "Funding Sources";
            // 
            // rdoTodayDate
            // 
            this.rdoTodayDate.AutoSize = false;
            this.rdoTodayDate.Checked = true;
            this.rdoTodayDate.Location = new System.Drawing.Point(142, 4);
            this.rdoTodayDate.Name = "rdoTodayDate";
            this.rdoTodayDate.Size = new System.Drawing.Size(102, 21);
            this.rdoTodayDate.TabIndex = 1;
            this.rdoTodayDate.TabStop = true;
            this.rdoTodayDate.Text = "Today\'s Date";
            // 
            // pnlBaseAge
            // 
            this.pnlBaseAge.Controls.Add(this.rdoKindergartenDate);
            this.pnlBaseAge.Controls.Add(this.rdoTodayDate);
            this.pnlBaseAge.Controls.Add(this.label13);
            this.pnlBaseAge.Dock = Wisej.Web.DockStyle.Left;
            this.pnlBaseAge.Location = new System.Drawing.Point(0, 0);
            this.pnlBaseAge.Name = "pnlBaseAge";
            this.pnlBaseAge.Size = new System.Drawing.Size(473, 31);
            this.pnlBaseAge.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(15, 7);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(78, 16);
            this.label13.TabIndex = 36;
            this.label13.Text = "Base Ages On";
            // 
            // cmbActive
            // 
            this.cmbActive.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbActive.FormattingEnabled = true;
            this.cmbActive.Location = new System.Drawing.Point(596, 3);
            this.cmbActive.Name = "cmbActive";
            this.cmbActive.Size = new System.Drawing.Size(100, 25);
            this.cmbActive.TabIndex = 3;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(483, 7);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(85, 16);
            this.label12.TabIndex = 32;
            this.label12.Text = "Active/Inactive";
            // 
            // btnTasks
            // 
            this.btnTasks.Location = new System.Drawing.Point(357, 3);
            this.btnTasks.Name = "btnTasks";
            this.btnTasks.Size = new System.Drawing.Size(75, 25);
            this.btnTasks.TabIndex = 2;
            this.btnTasks.Text = "&Tasks";
            this.btnTasks.ToolTipText = "Select Tasks";
            this.btnTasks.Click += new System.EventHandler(this.btnTask_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(15, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Max. # of Tasks";
            // 
            // lblShow
            // 
            this.lblShow.Location = new System.Drawing.Point(15, 7);
            this.lblShow.Name = "lblShow";
            this.lblShow.Size = new System.Drawing.Size(32, 16);
            this.lblShow.TabIndex = 37;
            this.lblShow.Text = "Show";
            // 
            // rdoMultipleSites
            // 
            this.rdoMultipleSites.AutoSize = false;
            this.rdoMultipleSites.Location = new System.Drawing.Point(180, 4);
            this.rdoMultipleSites.Name = "rdoMultipleSites";
            this.rdoMultipleSites.Size = new System.Drawing.Size(107, 21);
            this.rdoMultipleSites.TabIndex = 2;
            this.rdoMultipleSites.Text = "Selected Sites";
            this.rdoMultipleSites.Click += new System.EventHandler(this.rdoMultipleSites_Click);
            // 
            // rdoAllSite
            // 
            this.rdoAllSite.AutoSize = false;
            this.rdoAllSite.Checked = true;
            this.rdoAllSite.Location = new System.Drawing.Point(120, 4);
            this.rdoAllSite.Name = "rdoAllSite";
            this.rdoAllSite.Size = new System.Drawing.Size(43, 21);
            this.rdoAllSite.TabIndex = 1;
            this.rdoAllSite.TabStop = true;
            this.rdoAllSite.Text = "All";
            this.rdoAllSite.CheckedChanged += new System.EventHandler(this.rdoAllSite_CheckedChanged);
            // 
            // pnlSites
            // 
            this.pnlSites.Controls.Add(this.rdoMultipleSites);
            this.pnlSites.Controls.Add(this.rdoAllSite);
            this.pnlSites.Controls.Add(this.label4);
            this.pnlSites.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlSites.Location = new System.Drawing.Point(473, 0);
            this.pnlSites.Name = "pnlSites";
            this.pnlSites.Size = new System.Drawing.Size(471, 31);
            this.pnlSites.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 16);
            this.label4.TabIndex = 36;
            this.label4.Text = "Sites";
            // 
            // CmbYear
            // 
            this.CmbYear.Dock = Wisej.Web.DockStyle.Left;
            this.CmbYear.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbYear.FormattingEnabled = true;
            this.CmbYear.Location = new System.Drawing.Point(790, 0);
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
            this.Txt_HieDesc.Size = new System.Drawing.Size(775, 25);
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
            this.pnlHie.Size = new System.Drawing.Size(855, 25);
            this.pnlHie.TabIndex = 88;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(775, 0);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(15, 25);
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
            // btnPrint
            // 
            this.btnPrint.AppearanceKey = "button-reports";
            this.btnPrint.Dock = Wisej.Web.DockStyle.Right;
            this.btnPrint.Location = new System.Drawing.Point(693, 5);
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
            this.pnlGenerate.Controls.Add(this.btnSaveParameters);
            this.pnlGenerate.Controls.Add(this.spacer5);
            this.pnlGenerate.Controls.Add(this.btnPrint);
            this.pnlGenerate.Controls.Add(this.spacer4);
            this.pnlGenerate.Controls.Add(this.btnGeneratePdf);
            this.pnlGenerate.Controls.Add(this.spacer3);
            this.pnlGenerate.Controls.Add(this.btnPdfPreview);
            this.pnlGenerate.Controls.Add(this.btnGetParameters);
            this.pnlGenerate.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlGenerate.Location = new System.Drawing.Point(0, 305);
            this.pnlGenerate.Name = "pnlGenerate";
            this.pnlGenerate.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlGenerate.Size = new System.Drawing.Size(944, 35);
            this.pnlGenerate.TabIndex = 2;
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
            this.spacer4.Location = new System.Drawing.Point(753, 5);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(3, 25);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Right;
            this.spacer3.Location = new System.Drawing.Point(841, 5);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(3, 25);
            // 
            // rdoAlltaskDate
            // 
            this.rdoAlltaskDate.AutoSize = false;
            this.rdoAlltaskDate.Location = new System.Drawing.Point(355, 4);
            this.rdoAlltaskDate.Name = "rdoAlltaskDate";
            this.rdoAlltaskDate.Size = new System.Drawing.Size(106, 21);
            this.rdoAlltaskDate.TabIndex = 2;
            this.rdoAlltaskDate.Text = "All Task Dates";
            // 
            // pnAbsentReason
            // 
            this.pnAbsentReason.Controls.Add(this.rdoOnlyLutd);
            this.pnAbsentReason.Controls.Add(this.rdoAlltaskDate);
            this.pnAbsentReason.Controls.Add(this.lblShow);
            this.pnAbsentReason.Dock = Wisej.Web.DockStyle.Left;
            this.pnAbsentReason.Location = new System.Drawing.Point(0, 0);
            this.pnAbsentReason.Name = "pnAbsentReason";
            this.pnAbsentReason.Size = new System.Drawing.Size(473, 31);
            this.pnAbsentReason.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(15, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 16);
            this.label6.TabIndex = 5;
            this.label6.Text = "Report Sequence";
            // 
            // rdoReportChildName
            // 
            this.rdoReportChildName.AutoSize = false;
            this.rdoReportChildName.Checked = true;
            this.rdoReportChildName.Location = new System.Drawing.Point(142, 12);
            this.rdoReportChildName.Name = "rdoReportChildName";
            this.rdoReportChildName.Size = new System.Drawing.Size(94, 21);
            this.rdoReportChildName.TabIndex = 1;
            this.rdoReportChildName.TabStop = true;
            this.rdoReportChildName.Text = "Child Name";
            // 
            // rdoApplicant
            // 
            this.rdoApplicant.AutoSize = false;
            this.rdoApplicant.Location = new System.Drawing.Point(253, 12);
            this.rdoApplicant.Name = "rdoApplicant";
            this.rdoApplicant.Size = new System.Drawing.Size(82, 21);
            this.rdoApplicant.TabIndex = 2;
            this.rdoApplicant.Text = "Applicant";
            // 
            // pnlRepSeq
            // 
            this.pnlRepSeq.Controls.Add(this.rdoClass);
            this.pnlRepSeq.Controls.Add(this.rdoReportChildName);
            this.pnlRepSeq.Controls.Add(this.rdoApplicant);
            this.pnlRepSeq.Controls.Add(this.label6);
            this.pnlRepSeq.Dock = Wisej.Web.DockStyle.Top;
            this.pnlRepSeq.Location = new System.Drawing.Point(0, 0);
            this.pnlRepSeq.Name = "pnlRepSeq";
            this.pnlRepSeq.Size = new System.Drawing.Size(944, 39);
            this.pnlRepSeq.TabIndex = 1;
            // 
            // rdoChildren
            // 
            this.rdoChildren.AutoSize = false;
            this.rdoChildren.Location = new System.Drawing.Point(355, 4);
            this.rdoChildren.Name = "rdoChildren";
            this.rdoChildren.Size = new System.Drawing.Size(93, 21);
            this.rdoChildren.TabIndex = 3;
            this.rdoChildren.Text = "All Children";
            // 
            // rdoWaitList
            // 
            this.rdoWaitList.AutoSize = false;
            this.rdoWaitList.Location = new System.Drawing.Point(241, 4);
            this.rdoWaitList.Name = "rdoWaitList";
            this.rdoWaitList.Size = new System.Drawing.Size(94, 21);
            this.rdoWaitList.TabIndex = 2;
            this.rdoWaitList.Text = "Waiting List";
            // 
            // rdoEnrolled
            // 
            this.rdoEnrolled.AutoSize = false;
            this.rdoEnrolled.Checked = true;
            this.rdoEnrolled.Location = new System.Drawing.Point(142, 4);
            this.rdoEnrolled.Name = "rdoEnrolled";
            this.rdoEnrolled.Size = new System.Drawing.Size(75, 21);
            this.rdoEnrolled.TabIndex = 1;
            this.rdoEnrolled.TabStop = true;
            this.rdoEnrolled.Text = "Enrolled";
            // 
            // pnlEnroll
            // 
            this.pnlEnroll.Controls.Add(this.rdoChildren);
            this.pnlEnroll.Controls.Add(this.rdoWaitList);
            this.pnlEnroll.Controls.Add(this.rdoEnrolled);
            this.pnlEnroll.Dock = Wisej.Web.DockStyle.Left;
            this.pnlEnroll.Location = new System.Drawing.Point(0, 0);
            this.pnlEnroll.Name = "pnlEnroll";
            this.pnlEnroll.Size = new System.Drawing.Size(473, 37);
            this.pnlEnroll.TabIndex = 1;
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlParams);
            this.pnlCompleteForm.Controls.Add(this.pnlHieFilter);
            this.pnlCompleteForm.Controls.Add(this.pnlGenerate);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(944, 340);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlParams
            // 
            this.pnlParams.Controls.Add(this.pnlEnrolFS);
            this.pnlParams.Controls.Add(this.pnlShwSites);
            this.pnlParams.Controls.Add(this.pnlBaseAgZIP);
            this.pnlParams.Controls.Add(this.pnlChildAge);
            this.pnlParams.Controls.Add(this.pnlTask);
            this.pnlParams.Controls.Add(this.pnlChildWith);
            this.pnlParams.Controls.Add(this.pnlRepType);
            this.pnlParams.Controls.Add(this.pnlRepSeq);
            this.pnlParams.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlParams.Location = new System.Drawing.Point(0, 43);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new System.Drawing.Size(944, 262);
            this.pnlParams.TabIndex = 1;
            // 
            // pnlEnrolFS
            // 
            this.pnlEnrolFS.Controls.Add(this.pnlFunds);
            this.pnlEnrolFS.Controls.Add(this.pnlEnroll);
            this.pnlEnrolFS.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlEnrolFS.Location = new System.Drawing.Point(0, 225);
            this.pnlEnrolFS.Name = "pnlEnrolFS";
            this.pnlEnrolFS.Size = new System.Drawing.Size(944, 37);
            this.pnlEnrolFS.TabIndex = 8;
            // 
            // pnlShwSites
            // 
            this.pnlShwSites.Controls.Add(this.pnlSites);
            this.pnlShwSites.Controls.Add(this.pnAbsentReason);
            this.pnlShwSites.Dock = Wisej.Web.DockStyle.Top;
            this.pnlShwSites.Location = new System.Drawing.Point(0, 194);
            this.pnlShwSites.Name = "pnlShwSites";
            this.pnlShwSites.Size = new System.Drawing.Size(944, 31);
            this.pnlShwSites.TabIndex = 7;
            // 
            // pnlBaseAgZIP
            // 
            this.pnlBaseAgZIP.Controls.Add(this.pnlZIP);
            this.pnlBaseAgZIP.Controls.Add(this.pnlBaseAge);
            this.pnlBaseAgZIP.Dock = Wisej.Web.DockStyle.Top;
            this.pnlBaseAgZIP.Location = new System.Drawing.Point(0, 163);
            this.pnlBaseAgZIP.Name = "pnlBaseAgZIP";
            this.pnlBaseAgZIP.Size = new System.Drawing.Size(944, 31);
            this.pnlBaseAgZIP.TabIndex = 6;
            // 
            // pnlChildAge
            // 
            this.pnlChildAge.Controls.Add(this.lblChildAge);
            this.pnlChildAge.Controls.Add(this.txtTo);
            this.pnlChildAge.Controls.Add(this.label21);
            this.pnlChildAge.Controls.Add(this.label18);
            this.pnlChildAge.Controls.Add(this.lblForm);
            this.pnlChildAge.Controls.Add(this.txtFrom);
            this.pnlChildAge.Controls.Add(this.cmbActive);
            this.pnlChildAge.Controls.Add(this.label12);
            this.pnlChildAge.Dock = Wisej.Web.DockStyle.Top;
            this.pnlChildAge.Location = new System.Drawing.Point(0, 132);
            this.pnlChildAge.Name = "pnlChildAge";
            this.pnlChildAge.Size = new System.Drawing.Size(944, 31);
            this.pnlChildAge.TabIndex = 5;
            // 
            // txtTo
            // 
            this.txtTo.Location = new System.Drawing.Point(287, 3);
            this.txtTo.MaxLength = 3;
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(40, 25);
            this.txtTo.TabIndex = 2;
            this.txtTo.Text = "999";
            this.txtTo.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // txtFrom
            // 
            this.txtFrom.Location = new System.Drawing.Point(190, 3);
            this.txtFrom.MaxLength = 3;
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(40, 25);
            this.txtFrom.TabIndex = 1;
            this.txtFrom.Text = "0";
            this.txtFrom.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // pnlTask
            // 
            this.pnlTask.Controls.Add(this.label1);
            this.pnlTask.Controls.Add(this.btnTasks);
            this.pnlTask.Controls.Add(this.cmbMaxoftask);
            this.pnlTask.Controls.Add(this.chkIncludeNotes);
            this.pnlTask.Controls.Add(this.lblApplicantReq);
            this.pnlTask.Dock = Wisej.Web.DockStyle.Top;
            this.pnlTask.Location = new System.Drawing.Point(0, 101);
            this.pnlTask.Name = "pnlTask";
            this.pnlTask.Size = new System.Drawing.Size(944, 31);
            this.pnlTask.TabIndex = 4;
            // 
            // chkIncludeNotes
            // 
            this.chkIncludeNotes.AutoSize = false;
            this.chkIncludeNotes.Enabled = false;
            this.chkIncludeNotes.Location = new System.Drawing.Point(479, 4);
            this.chkIncludeNotes.Name = "chkIncludeNotes";
            this.chkIncludeNotes.Size = new System.Drawing.Size(135, 21);
            this.chkIncludeNotes.TabIndex = 5;
            this.chkIncludeNotes.Text = "Include Case Notes";
            this.chkIncludeNotes.Visible = false;
            // 
            // pnlRepType
            // 
            this.pnlRepType.Controls.Add(this.txtApplicant);
            this.pnlRepType.Controls.Add(this.btnBrowse);
            this.pnlRepType.Controls.Add(this.label2);
            this.pnlRepType.Controls.Add(this.label5);
            this.pnlRepType.Controls.Add(this.rdoSelectApplicant);
            this.pnlRepType.Controls.Add(this.rdoAllAplicants);
            this.pnlRepType.Dock = Wisej.Web.DockStyle.Top;
            this.pnlRepType.Location = new System.Drawing.Point(0, 39);
            this.pnlRepType.Name = "pnlRepType";
            this.pnlRepType.Size = new System.Drawing.Size(944, 31);
            this.pnlRepType.TabIndex = 2;
            // 
            // txtApplicant
            // 
            this.txtApplicant.Enabled = false;
            this.txtApplicant.Location = new System.Drawing.Point(365, 3);
            this.txtApplicant.MaxLength = 9;
            this.txtApplicant.Name = "txtApplicant";
            this.txtApplicant.Size = new System.Drawing.Size(71, 25);
            this.txtApplicant.TabIndex = 3;
            this.txtApplicant.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtApplicant.Leave += new System.EventHandler(this.txtApplicant_Leave);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(447, 3);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 25);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "&Browse";
            this.btnBrowse.ToolTipText = "Select Applicant";
            this.btnBrowse.Visible = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click_1);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(15, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Report Type";
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(344, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(9, 11);
            this.label5.TabIndex = 28;
            this.label5.Text = "*";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label5.Visible = false;
            // 
            // rdoSelectApplicant
            // 
            this.rdoSelectApplicant.AutoSize = false;
            this.rdoSelectApplicant.Location = new System.Drawing.Point(215, 4);
            this.rdoSelectApplicant.Name = "rdoSelectApplicant";
            this.rdoSelectApplicant.Size = new System.Drawing.Size(132, 21);
            this.rdoSelectApplicant.TabIndex = 2;
            this.rdoSelectApplicant.Text = "Selected Applicant";
            this.rdoSelectApplicant.CheckedChanged += new System.EventHandler(this.rdoSelectApplicant_CheckedChanged);
            // 
            // rdoAllAplicants
            // 
            this.rdoAllAplicants.AutoSize = false;
            this.rdoAllAplicants.Checked = true;
            this.rdoAllAplicants.Location = new System.Drawing.Point(142, 4);
            this.rdoAllAplicants.Name = "rdoAllAplicants";
            this.rdoAllAplicants.Size = new System.Drawing.Size(41, 21);
            this.rdoAllAplicants.TabIndex = 1;
            this.rdoAllAplicants.TabStop = true;
            this.rdoAllAplicants.Text = "All";
            this.rdoAllAplicants.CheckedChanged += new System.EventHandler(this.rdoSelectApplicant_CheckedChanged);
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
            this.pnlHieFilter.Size = new System.Drawing.Size(944, 43);
            this.pnlHieFilter.TabIndex = 99;
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.Pb_Search_Hie);
            this.pnlFilter.Controls.Add(this.spacer2);
            this.pnlFilter.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlFilter.Location = new System.Drawing.Point(870, 9);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(65, 25);
            this.pnlFilter.TabIndex = 55;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(0, 0);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(15, 25);
            // 
            // HSSB2103ReportForm
            // 
            this.ClientSize = new System.Drawing.Size(944, 340);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSSB2103ReportForm";
            this.Text = "HSSB2103ReportForm";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "tlHelp";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.HSSB2103ReportForm_ToolClick);
            this.pnlZIP.ResumeLayout(false);
            this.pnlChildWith.ResumeLayout(false);
            this.pnlChildWith.PerformLayout();
            this.pnlFunds.ResumeLayout(false);
            this.pnlBaseAge.ResumeLayout(false);
            this.pnlSites.ResumeLayout(false);
            this.pnlHie.ResumeLayout(false);
            this.pnlHie.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).EndInit();
            this.pnlGenerate.ResumeLayout(false);
            this.pnAbsentReason.ResumeLayout(false);
            this.pnlRepSeq.ResumeLayout(false);
            this.pnlEnroll.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlParams.ResumeLayout(false);
            this.pnlEnrolFS.ResumeLayout(false);
            this.pnlShwSites.ResumeLayout(false);
            this.pnlBaseAgZIP.ResumeLayout(false);
            this.pnlChildAge.ResumeLayout(false);
            this.pnlChildAge.PerformLayout();
            this.pnlTask.ResumeLayout(false);
            this.pnlTask.PerformLayout();
            this.pnlRepType.ResumeLayout(false);
            this.pnlRepType.PerformLayout();
            this.pnlHieFilter.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnGeneratePdf;
        private RadioButton rdoSelectask;
        private RadioButton rdoKindergartenDate;
        private Button btnPdfPreview;
        private RadioButton rdoClass;
        private TextBoxWithValidation txtFrom;
        private ComboBox cmbMaxoftask;
        private RadioButton rdoOnlyLutd;
        private Label label21;
        private Label lblChildAge;
        private Label lblApplicantReq;
        private Label lblForm;
        private Label label18;
        private TextBoxWithValidation txtTo;
        private RadioButton rdoZipAll;
        private RadioButton rdoSkipSelected;
        private RadioButton rdoZipSelected;
        private Panel pnlZIP;
        private Label label16;
        private RadioButton rdoBoth;
        private Panel pnlChildWith;
        private RadioButton rdoFundingAll;
        private RadioButton rdoFundingSelect;
        private Panel pnlFunds;
        private Label label14;
        private RadioButton rdoTodayDate;
        private Panel pnlBaseAge;
        private Label label13;
        private ComboBox cmbActive;
        private Label label12;
        private TextBox txtCompleTask;
        private Button btnTasks;
        private Label label1;
        private Label lblShow;
        private RadioButton rdoMultipleSites;
        private RadioButton rdoAllSite;
        private Panel pnlSites;
        private Label label4;
        private ComboBox CmbYear;
        private TextBox Txt_HieDesc;
        private Panel pnlHie;
        private PictureBox Pb_Search_Hie;
        private Button btnPrint;
        private Button btnSaveParameters;
        private Button btnGetParameters;
        private Panel pnlGenerate;
        private RadioButton rdoAlltaskDate;
        private Panel pnAbsentReason;
        private Label label6;
        private RadioButton rdoReportChildName;
        private RadioButton rdoApplicant;
        private Panel pnlRepSeq;
        private RadioButton rdoChildren;
        private RadioButton rdoWaitList;
        private RadioButton rdoEnrolled;
        private Panel pnlEnroll;
        private Panel pnlCompleteForm;
        private Label lblcompTask;
        private CheckBox chkIncludeNotes;
        private Label label3;
        private RadioButton rdoNoTask;
        private Panel pnlRepType;
        private RadioButton rdoAllAplicants;
        private Label label2;
        private RadioButton rdoSelectApplicant;
        private TextBox txtApplicant;
        private Button btnBrowse;
        private Label label5;
        private Panel pnlHieFilter;
        private Spacer spacer1;
        private Panel pnlFilter;
        private Spacer spacer2;
        private Spacer spacer3;
        private Spacer spacer4;
        private Spacer spacer5;
        private Panel pnlParams;
        private Panel pnlBaseAgZIP;
        private Panel pnlChildAge;
        private Panel pnlTask;
        private Panel pnlEnrolFS;
        private Panel pnlShwSites;
    }
}