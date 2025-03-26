using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class HSSB2111ReportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSSB2111ReportForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.rdoSummaryO = new Wisej.Web.RadioButton();
            this.lblSitefor = new Wisej.Web.Label();
            this.Txt_HieDesc = new Wisej.Web.TextBox();
            this.cmbSiteforHome = new Wisej.Web.ComboBox();
            this.Pb_Search_Hie = new Wisej.Web.PictureBox();
            this.rdoSelected = new Wisej.Web.RadioButton();
            this.btnGeneratePdf = new Wisej.Web.Button();
            this.btnPdfPreview = new Wisej.Web.Button();
            this.btnPrint = new Wisej.Web.Button();
            this.btnSaveParameters = new Wisej.Web.Button();
            this.btnGetParameters = new Wisej.Web.Button();
            this.pnlGenerate = new Wisej.Web.Panel();
            this.spacer4 = new Wisej.Web.Spacer();
            this.spacer3 = new Wisej.Web.Spacer();
            this.btnExcel = new Wisej.Web.Button();
            this.spacer2 = new Wisej.Web.Spacer();
            this.spacer1 = new Wisej.Web.Spacer();
            this.CmbYear = new Wisej.Web.ComboBox();
            this.label8 = new Wisej.Web.Label();
            this.label9 = new Wisej.Web.Label();
            this.label7 = new Wisej.Web.Label();
            this.lblEjobTitleReq = new Wisej.Web.Label();
            this.pnlHie = new Wisej.Web.Panel();
            this.spacer5 = new Wisej.Web.Spacer();
            this.rdoFSourceAll = new Wisej.Web.RadioButton();
            this.pnlPrintDetails = new Wisej.Web.Panel();
            this.lblFundingSource = new Wisej.Web.Label();
            this.lblAllFunded = new Wisej.Web.Label();
            this.rdoAllFundNo = new Wisej.Web.RadioButton();
            this.rdoAllFundYes = new Wisej.Web.RadioButton();
            this.panel6 = new Wisej.Web.Panel();
            this.label5 = new Wisej.Web.Label();
            this.rdoDetail = new Wisej.Web.RadioButton();
            this.rdoDetailBoth = new Wisej.Web.RadioButton();
            this.pnlRepType = new Wisej.Web.Panel();
            this.lblReportType = new Wisej.Web.Label();
            this.dtTodate = new Wisej.Web.DateTimePicker();
            this.dtForm = new Wisej.Web.DateTimePicker();
            this.lblToDate = new Wisej.Web.Label();
            this.lblReportFormdt = new Wisej.Web.Label();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlParams = new Wisej.Web.Panel();
            this.panel1 = new Wisej.Web.Panel();
            this.label3 = new Wisej.Web.Label();
            this.pnlDtes = new Wisej.Web.Panel();
            this.chkDetailReport = new Wisej.Web.CheckBox();
            this.pnlSite = new Wisej.Web.Panel();
            this.rdoMultipleSites = new Wisej.Web.RadioButton();
            this.rdoAllSite = new Wisej.Web.RadioButton();
            this.lblSite = new Wisej.Web.Label();
            this.pnlHieFilter = new Wisej.Web.Panel();
            this.pnlFilter = new Wisej.Web.Panel();
            this.spacer6 = new Wisej.Web.Spacer();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).BeginInit();
            this.pnlGenerate.SuspendLayout();
            this.label9.SuspendLayout();
            this.lblEjobTitleReq.SuspendLayout();
            this.pnlHie.SuspendLayout();
            this.pnlPrintDetails.SuspendLayout();
            this.panel6.SuspendLayout();
            this.pnlRepType.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlParams.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlDtes.SuspendLayout();
            this.pnlSite.SuspendLayout();
            this.pnlHieFilter.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdoSummaryO
            // 
            this.rdoSummaryO.AutoSize = false;
            this.rdoSummaryO.Location = new System.Drawing.Point(413, 4);
            this.rdoSummaryO.Name = "rdoSummaryO";
            this.rdoSummaryO.Size = new System.Drawing.Size(113, 21);
            this.rdoSummaryO.TabIndex = 3;
            this.rdoSummaryO.Text = "Summary Only";
            // 
            // lblSitefor
            // 
            this.lblSitefor.Location = new System.Drawing.Point(15, 7);
            this.lblSitefor.Name = "lblSitefor";
            this.lblSitefor.Size = new System.Drawing.Size(116, 16);
            this.lblSitefor.TabIndex = 0;
            this.lblSitefor.Text = "Site for Home Based";
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
            this.Txt_HieDesc.Size = new System.Drawing.Size(710, 25);
            this.Txt_HieDesc.TabIndex = 77;
            this.Txt_HieDesc.TabStop = false;
            this.Txt_HieDesc.TextAlign = Wisej.Web.HorizontalAlignment.Center;
            // 
            // cmbSiteforHome
            // 
            this.cmbSiteforHome.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbSiteforHome.FormattingEnabled = true;
            this.cmbSiteforHome.Location = new System.Drawing.Point(150, 3);
            this.cmbSiteforHome.Name = "cmbSiteforHome";
            this.cmbSiteforHome.Size = new System.Drawing.Size(297, 25);
            this.cmbSiteforHome.TabIndex = 1;
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
            // rdoSelected
            // 
            this.rdoSelected.AutoSize = false;
            this.rdoSelected.Location = new System.Drawing.Point(210, 4);
            this.rdoSelected.Name = "rdoSelected";
            this.rdoSelected.Size = new System.Drawing.Size(76, 21);
            this.rdoSelected.TabIndex = 2;
            this.rdoSelected.Text = "Selected";
            this.rdoSelected.Click += new System.EventHandler(this.rdoSelected_Click);
            // 
            // btnGeneratePdf
            // 
            this.btnGeneratePdf.AppearanceKey = "button-reports";
            this.btnGeneratePdf.Dock = Wisej.Web.DockStyle.Right;
            this.btnGeneratePdf.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnGeneratePdf.Location = new System.Drawing.Point(687, 5);
            this.btnGeneratePdf.Name = "btnGeneratePdf";
            this.btnGeneratePdf.Size = new System.Drawing.Size(85, 25);
            this.btnGeneratePdf.TabIndex = 5;
            this.btnGeneratePdf.Text = "&Generate";
            this.btnGeneratePdf.Click += new System.EventHandler(this.btnGeneratePdf_Click);
            // 
            // btnPdfPreview
            // 
            this.btnPdfPreview.AppearanceKey = "button-reports";
            this.btnPdfPreview.Dock = Wisej.Web.DockStyle.Right;
            this.btnPdfPreview.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnPdfPreview.Location = new System.Drawing.Point(775, 5);
            this.btnPdfPreview.Name = "btnPdfPreview";
            this.btnPdfPreview.Size = new System.Drawing.Size(85, 25);
            this.btnPdfPreview.TabIndex = 6;
            this.btnPdfPreview.Text = "Pre&view";
            this.btnPdfPreview.Click += new System.EventHandler(this.btnPdfPreview_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.AppearanceKey = "button-reports";
            this.btnPrint.Dock = Wisej.Web.DockStyle.Right;
            this.btnPrint.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnPrint.Location = new System.Drawing.Point(511, 5);
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
            this.btnSaveParameters.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
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
            this.btnGetParameters.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
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
            this.pnlGenerate.Controls.Add(this.spacer4);
            this.pnlGenerate.Controls.Add(this.btnPrint);
            this.pnlGenerate.Controls.Add(this.spacer3);
            this.pnlGenerate.Controls.Add(this.btnExcel);
            this.pnlGenerate.Controls.Add(this.spacer2);
            this.pnlGenerate.Controls.Add(this.btnGeneratePdf);
            this.pnlGenerate.Controls.Add(this.spacer1);
            this.pnlGenerate.Controls.Add(this.btnPdfPreview);
            this.pnlGenerate.Controls.Add(this.btnGetParameters);
            this.pnlGenerate.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlGenerate.Location = new System.Drawing.Point(0, 268);
            this.pnlGenerate.Name = "pnlGenerate";
            this.pnlGenerate.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlGenerate.Size = new System.Drawing.Size(875, 35);
            this.pnlGenerate.TabIndex = 2;
            // 
            // spacer4
            // 
            this.spacer4.Dock = Wisej.Web.DockStyle.Left;
            this.spacer4.Location = new System.Drawing.Point(125, 5);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(3, 25);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Right;
            this.spacer3.Location = new System.Drawing.Point(571, 5);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(3, 25);
            // 
            // btnExcel
            // 
            this.btnExcel.AppearanceKey = "button-reports";
            this.btnExcel.Dock = Wisej.Web.DockStyle.Right;
            this.btnExcel.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnExcel.Location = new System.Drawing.Point(574, 5);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(110, 25);
            this.btnExcel.TabIndex = 4;
            this.btnExcel.Text = "Generate &EXCEL";
            this.btnExcel.Visible = false;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Right;
            this.spacer2.Location = new System.Drawing.Point(684, 5);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(3, 25);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(772, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // CmbYear
            // 
            this.CmbYear.Dock = Wisej.Web.DockStyle.Left;
            this.CmbYear.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbYear.FormattingEnabled = true;
            this.CmbYear.Location = new System.Drawing.Point(725, 0);
            this.CmbYear.Name = "CmbYear";
            this.CmbYear.Size = new System.Drawing.Size(65, 25);
            this.CmbYear.TabIndex = 66;
            this.CmbYear.TabStop = false;
            this.CmbYear.Visible = false;
            this.CmbYear.SelectedIndexChanged += new System.EventHandler(this.CmbYear_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(-11, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(9, 14);
            this.label8.TabIndex = 0;
            this.label8.Text = "*";
            this.label8.Visible = false;
            // 
            // label9
            // 
            this.label9.Controls.Add(this.label8);
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(344, 4);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(7, 11);
            this.label9.TabIndex = 0;
            this.label9.Text = "*";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(-11, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(9, 14);
            this.label7.TabIndex = 0;
            this.label7.Text = "*";
            this.label7.Visible = false;
            // 
            // lblEjobTitleReq
            // 
            this.lblEjobTitleReq.Controls.Add(this.label7);
            this.lblEjobTitleReq.ForeColor = System.Drawing.Color.Red;
            this.lblEjobTitleReq.Location = new System.Drawing.Point(117, 4);
            this.lblEjobTitleReq.Name = "lblEjobTitleReq";
            this.lblEjobTitleReq.Size = new System.Drawing.Size(8, 11);
            this.lblEjobTitleReq.TabIndex = 0;
            this.lblEjobTitleReq.Text = "*";
            // 
            // pnlHie
            // 
            this.pnlHie.BackColor = System.Drawing.Color.FromArgb(11, 70, 117);
            this.pnlHie.Controls.Add(this.CmbYear);
            this.pnlHie.Controls.Add(this.spacer5);
            this.pnlHie.Controls.Add(this.Txt_HieDesc);
            this.pnlHie.Dock = Wisej.Web.DockStyle.Left;
            this.pnlHie.Location = new System.Drawing.Point(15, 9);
            this.pnlHie.Name = "pnlHie";
            this.pnlHie.Size = new System.Drawing.Size(790, 25);
            this.pnlHie.TabIndex = 88;
            // 
            // spacer5
            // 
            this.spacer5.Dock = Wisej.Web.DockStyle.Left;
            this.spacer5.Location = new System.Drawing.Point(710, 0);
            this.spacer5.Name = "spacer5";
            this.spacer5.Size = new System.Drawing.Size(15, 25);
            // 
            // rdoFSourceAll
            // 
            this.rdoFSourceAll.AutoSize = false;
            this.rdoFSourceAll.Checked = true;
            this.rdoFSourceAll.Location = new System.Drawing.Point(147, 4);
            this.rdoFSourceAll.Name = "rdoFSourceAll";
            this.rdoFSourceAll.Size = new System.Drawing.Size(43, 21);
            this.rdoFSourceAll.TabIndex = 1;
            this.rdoFSourceAll.TabStop = true;
            this.rdoFSourceAll.Text = "All";
            this.rdoFSourceAll.CheckedChanged += new System.EventHandler(this.rdoFSourceAll_CheckedChanged);
            // 
            // pnlPrintDetails
            // 
            this.pnlPrintDetails.Controls.Add(this.rdoFSourceAll);
            this.pnlPrintDetails.Controls.Add(this.rdoSelected);
            this.pnlPrintDetails.Controls.Add(this.lblFundingSource);
            this.pnlPrintDetails.Dock = Wisej.Web.DockStyle.Top;
            this.pnlPrintDetails.Location = new System.Drawing.Point(0, 70);
            this.pnlPrintDetails.Name = "pnlPrintDetails";
            this.pnlPrintDetails.Size = new System.Drawing.Size(875, 31);
            this.pnlPrintDetails.TabIndex = 3;
            // 
            // lblFundingSource
            // 
            this.lblFundingSource.Location = new System.Drawing.Point(15, 7);
            this.lblFundingSource.Name = "lblFundingSource";
            this.lblFundingSource.Size = new System.Drawing.Size(95, 16);
            this.lblFundingSource.TabIndex = 37;
            this.lblFundingSource.Text = "Funding Sources";
            // 
            // lblAllFunded
            // 
            this.lblAllFunded.Location = new System.Drawing.Point(15, 34);
            this.lblAllFunded.Name = "lblAllFunded";
            this.lblAllFunded.Size = new System.Drawing.Size(91, 16);
            this.lblAllFunded.TabIndex = 5;
            this.lblAllFunded.Text = "All Funded Days";
            // 
            // rdoAllFundNo
            // 
            this.rdoAllFundNo.AutoSize = false;
            this.rdoAllFundNo.Checked = true;
            this.rdoAllFundNo.Location = new System.Drawing.Point(147, 31);
            this.rdoAllFundNo.Name = "rdoAllFundNo";
            this.rdoAllFundNo.Size = new System.Drawing.Size(44, 21);
            this.rdoAllFundNo.TabIndex = 1;
            this.rdoAllFundNo.TabStop = true;
            this.rdoAllFundNo.Text = "No";
            // 
            // rdoAllFundYes
            // 
            this.rdoAllFundYes.AutoSize = false;
            this.rdoAllFundYes.Location = new System.Drawing.Point(210, 31);
            this.rdoAllFundYes.Name = "rdoAllFundYes";
            this.rdoAllFundYes.Size = new System.Drawing.Size(49, 21);
            this.rdoAllFundYes.TabIndex = 2;
            this.rdoAllFundYes.Text = "Yes";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.rdoAllFundNo);
            this.panel6.Controls.Add(this.rdoAllFundYes);
            this.panel6.Controls.Add(this.lblAllFunded);
            this.panel6.Controls.Add(this.label5);
            this.panel6.Dock = Wisej.Web.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 163);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(875, 62);
            this.panel6.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(150, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(207, 16);
            this.label5.TabIndex = 0;
            this.label5.Text = "Include all funded days in averages?";
            // 
            // rdoDetail
            // 
            this.rdoDetail.AutoSize = false;
            this.rdoDetail.Location = new System.Drawing.Point(305, 4);
            this.rdoDetail.Name = "rdoDetail";
            this.rdoDetail.Size = new System.Drawing.Size(90, 21);
            this.rdoDetail.TabIndex = 2;
            this.rdoDetail.Text = "Detail Only";
            // 
            // rdoDetailBoth
            // 
            this.rdoDetailBoth.AutoSize = false;
            this.rdoDetailBoth.Checked = true;
            this.rdoDetailBoth.Location = new System.Drawing.Point(147, 4);
            this.rdoDetailBoth.Name = "rdoDetailBoth";
            this.rdoDetailBoth.Size = new System.Drawing.Size(143, 21);
            this.rdoDetailBoth.TabIndex = 1;
            this.rdoDetailBoth.TabStop = true;
            this.rdoDetailBoth.Text = "Detail and Summary";
            // 
            // pnlRepType
            // 
            this.pnlRepType.Controls.Add(this.rdoSummaryO);
            this.pnlRepType.Controls.Add(this.lblReportType);
            this.pnlRepType.Controls.Add(this.rdoDetail);
            this.pnlRepType.Controls.Add(this.rdoDetailBoth);
            this.pnlRepType.Dock = Wisej.Web.DockStyle.Top;
            this.pnlRepType.Location = new System.Drawing.Point(0, 39);
            this.pnlRepType.Name = "pnlRepType";
            this.pnlRepType.Size = new System.Drawing.Size(875, 31);
            this.pnlRepType.TabIndex = 2;
            // 
            // lblReportType
            // 
            this.lblReportType.Location = new System.Drawing.Point(15, 7);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new System.Drawing.Size(69, 16);
            this.lblReportType.TabIndex = 5;
            this.lblReportType.Text = "Report Type";
            // 
            // dtTodate
            // 
            this.dtTodate.AutoSize = false;
            this.dtTodate.CustomFormat = "MM/dd/yyyy";
            this.dtTodate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtTodate.Location = new System.Drawing.Point(366, 3);
            this.dtTodate.Name = "dtTodate";
            this.dtTodate.ShowCheckBox = true;
            this.dtTodate.ShowToolTips = false;
            this.dtTodate.Size = new System.Drawing.Size(116, 25);
            this.dtTodate.TabIndex = 2;
            // 
            // dtForm
            // 
            this.dtForm.AutoSize = false;
            this.dtForm.CustomFormat = "MM/dd/yyyy";
            this.dtForm.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtForm.Location = new System.Drawing.Point(150, 3);
            this.dtForm.Name = "dtForm";
            this.dtForm.ShowCheckBox = true;
            this.dtForm.ShowToolTips = false;
            this.dtForm.Size = new System.Drawing.Size(116, 25);
            this.dtForm.TabIndex = 1;
            // 
            // lblToDate
            // 
            this.lblToDate.Location = new System.Drawing.Point(300, 7);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(44, 16);
            this.lblToDate.TabIndex = 2;
            this.lblToDate.Text = "To Date";
            // 
            // lblReportFormdt
            // 
            this.lblReportFormdt.Location = new System.Drawing.Point(15, 7);
            this.lblReportFormdt.Name = "lblReportFormdt";
            this.lblReportFormdt.Size = new System.Drawing.Size(102, 16);
            this.lblReportFormdt.TabIndex = 2;
            this.lblReportFormdt.Text = "Report From Date";
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlParams);
            this.pnlCompleteForm.Controls.Add(this.pnlHieFilter);
            this.pnlCompleteForm.Controls.Add(this.pnlGenerate);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(875, 303);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlParams
            // 
            this.pnlParams.Controls.Add(this.panel6);
            this.pnlParams.Controls.Add(this.panel1);
            this.pnlParams.Controls.Add(this.pnlDtes);
            this.pnlParams.Controls.Add(this.pnlPrintDetails);
            this.pnlParams.Controls.Add(this.pnlRepType);
            this.pnlParams.Controls.Add(this.pnlSite);
            this.pnlParams.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlParams.Location = new System.Drawing.Point(0, 43);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new System.Drawing.Size(875, 225);
            this.pnlParams.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblSitefor);
            this.panel1.Controls.Add(this.cmbSiteforHome);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 132);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(875, 31);
            this.panel1.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(478, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(164, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "To skip home based children";
            // 
            // pnlDtes
            // 
            this.pnlDtes.Controls.Add(this.lblReportFormdt);
            this.pnlDtes.Controls.Add(this.lblToDate);
            this.pnlDtes.Controls.Add(this.chkDetailReport);
            this.pnlDtes.Controls.Add(this.lblEjobTitleReq);
            this.pnlDtes.Controls.Add(this.dtForm);
            this.pnlDtes.Controls.Add(this.dtTodate);
            this.pnlDtes.Controls.Add(this.label9);
            this.pnlDtes.Dock = Wisej.Web.DockStyle.Top;
            this.pnlDtes.Location = new System.Drawing.Point(0, 101);
            this.pnlDtes.Name = "pnlDtes";
            this.pnlDtes.Size = new System.Drawing.Size(875, 31);
            this.pnlDtes.TabIndex = 4;
            // 
            // chkDetailReport
            // 
            this.chkDetailReport.AutoSize = false;
            this.chkDetailReport.Location = new System.Drawing.Point(531, 4);
            this.chkDetailReport.Name = "chkDetailReport";
            this.chkDetailReport.Size = new System.Drawing.Size(135, 21);
            this.chkDetailReport.TabIndex = 3;
            this.chkDetailReport.Text = "Detail Excel Report";
            // 
            // pnlSite
            // 
            this.pnlSite.Controls.Add(this.rdoMultipleSites);
            this.pnlSite.Controls.Add(this.rdoAllSite);
            this.pnlSite.Controls.Add(this.lblSite);
            this.pnlSite.Dock = Wisej.Web.DockStyle.Top;
            this.pnlSite.Location = new System.Drawing.Point(0, 0);
            this.pnlSite.Name = "pnlSite";
            this.pnlSite.Size = new System.Drawing.Size(875, 39);
            this.pnlSite.TabIndex = 1;
            // 
            // rdoMultipleSites
            // 
            this.rdoMultipleSites.AutoSize = false;
            this.rdoMultipleSites.Location = new System.Drawing.Point(265, 12);
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
            this.rdoAllSite.Location = new System.Drawing.Point(147, 12);
            this.rdoAllSite.Name = "rdoAllSite";
            this.rdoAllSite.Size = new System.Drawing.Size(95, 21);
            this.rdoAllSite.TabIndex = 1;
            this.rdoAllSite.TabStop = true;
            this.rdoAllSite.Text = "For All Sites";
            this.rdoAllSite.CheckedChanged += new System.EventHandler(this.rdoAllSite_CheckedChanged);
            // 
            // lblSite
            // 
            this.lblSite.Location = new System.Drawing.Point(15, 15);
            this.lblSite.Name = "lblSite";
            this.lblSite.Size = new System.Drawing.Size(22, 16);
            this.lblSite.TabIndex = 36;
            this.lblSite.Text = "Site";
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
            this.pnlHieFilter.Size = new System.Drawing.Size(875, 43);
            this.pnlHieFilter.TabIndex = 99;
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.Pb_Search_Hie);
            this.pnlFilter.Controls.Add(this.spacer6);
            this.pnlFilter.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlFilter.Location = new System.Drawing.Point(805, 9);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(61, 25);
            this.pnlFilter.TabIndex = 44;
            // 
            // spacer6
            // 
            this.spacer6.Dock = Wisej.Web.DockStyle.Left;
            this.spacer6.Location = new System.Drawing.Point(0, 0);
            this.spacer6.Name = "spacer6";
            this.spacer6.Size = new System.Drawing.Size(15, 25);
            // 
            // HSSB2111ReportForm
            // 
            this.ClientSize = new System.Drawing.Size(875, 303);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSSB2111ReportForm";
            this.Text = "HSSB2111ReportForm";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "tlHelp";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.HSSB2111ReportForm_ToolClick);
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).EndInit();
            this.pnlGenerate.ResumeLayout(false);
            this.label9.ResumeLayout(false);
            this.label9.PerformLayout();
            this.lblEjobTitleReq.ResumeLayout(false);
            this.lblEjobTitleReq.PerformLayout();
            this.pnlHie.ResumeLayout(false);
            this.pnlHie.PerformLayout();
            this.pnlPrintDetails.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.pnlRepType.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlParams.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlDtes.ResumeLayout(false);
            this.pnlSite.ResumeLayout(false);
            this.pnlHieFilter.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private RadioButton rdoSummaryO;
        private Label lblSitefor;
        private TextBox Txt_HieDesc;
        private ComboBox cmbSiteforHome;
        private PictureBox Pb_Search_Hie;
        private RadioButton rdoSelected;
        private Button btnGeneratePdf;
        private Button btnPdfPreview;
        private Button btnPrint;
        private Button btnSaveParameters;
        private Button btnGetParameters;
        private Panel pnlGenerate;
        private ComboBox CmbYear;
        private Label label8;
        private Label label9;
        private Label label7;
        private Label lblEjobTitleReq;
        private Panel pnlHie;
        private RadioButton rdoFSourceAll;
        private Panel pnlPrintDetails;
        private Label lblAllFunded;
        private RadioButton rdoAllFundNo;
        private RadioButton rdoAllFundYes;
        private Panel panel6;
        private RadioButton rdoDetail;
        private RadioButton rdoDetailBoth;
        private Panel pnlRepType;
        private Label lblReportType;
        private DateTimePicker dtTodate;
        private DateTimePicker dtForm;
        private Label lblToDate;
        private Label lblReportFormdt;
        private Panel pnlCompleteForm;
        private Panel pnlSite;
        private RadioButton rdoMultipleSites;
        private RadioButton rdoAllSite;
        private Label lblSite;
        private Label lblFundingSource;
        private Label label5;
        private Label label3;
        private Button btnExcel;
        private CheckBox chkDetailReport;
        private Spacer spacer1;
        private Spacer spacer3;
        private Spacer spacer2;
        private Spacer spacer4;
        private Panel pnlHieFilter;
        private Spacer spacer5;
        private Panel pnlFilter;
        private Spacer spacer6;
        private Panel pnlParams;
        private Panel pnlDtes;
        private Panel panel1;
    }
}