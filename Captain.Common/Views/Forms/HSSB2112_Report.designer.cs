using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class HSSB2112_Report
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSSB2112_Report));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.CmbYear = new Wisej.Web.ComboBox();
            this.Txt_HieDesc = new Wisej.Web.TextBox();
            this.pnlHie = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.Pb_Search_Hie = new Wisej.Web.PictureBox();
            this.pnlParams = new Wisej.Web.Panel();
            this.pnlDtes = new Wisej.Web.Panel();
            this.lblReportFormdt = new Wisej.Web.Label();
            this.lblToDate = new Wisej.Web.Label();
            this.dtTodate = new Wisej.Web.DateTimePicker();
            this.lblEjobTitleReq = new Wisej.Web.Label();
            this.label7 = new Wisej.Web.Label();
            this.dtFrom = new Wisej.Web.DateTimePicker();
            this.label9 = new Wisej.Web.Label();
            this.label8 = new Wisej.Web.Label();
            this.panel7 = new Wisej.Web.Panel();
            this.rdoFundingAll = new Wisej.Web.RadioButton();
            this.rdoFundingSelect = new Wisej.Web.RadioButton();
            this.label14 = new Wisej.Web.Label();
            this.panel4 = new Wisej.Web.Panel();
            this.rbPaid = new Wisej.Web.RadioButton();
            this.lblMeals = new Wisej.Web.Label();
            this.rbReduced = new Wisej.Web.RadioButton();
            this.rbAllmeals = new Wisej.Web.RadioButton();
            this.rbFree = new Wisej.Web.RadioButton();
            this.panel3 = new Wisej.Web.Panel();
            this.rdoMultipleSites = new Wisej.Web.RadioButton();
            this.rdoAllSite = new Wisej.Web.RadioButton();
            this.label4 = new Wisej.Web.Label();
            this.btnGeneratePdf = new Wisej.Web.Button();
            this.btnPdfPreview = new Wisej.Web.Button();
            this.btnPrint = new Wisej.Web.Button();
            this.btnSaveParameters = new Wisej.Web.Button();
            this.btnGetParameters = new Wisej.Web.Button();
            this.pnlGenerate = new Wisej.Web.Panel();
            this.spacer5 = new Wisej.Web.Spacer();
            this.spacer4 = new Wisej.Web.Spacer();
            this.spacer3 = new Wisej.Web.Spacer();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.panel5 = new Wisej.Web.Panel();
            this.pnlFilter = new Wisej.Web.Panel();
            this.spacer2 = new Wisej.Web.Spacer();
            this.pnlHie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).BeginInit();
            this.pnlParams.SuspendLayout();
            this.pnlDtes.SuspendLayout();
            this.lblEjobTitleReq.SuspendLayout();
            this.label9.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnlGenerate.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.panel5.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // CmbYear
            // 
            this.CmbYear.Dock = Wisej.Web.DockStyle.Left;
            this.CmbYear.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbYear.FormattingEnabled = true;
            this.CmbYear.Location = new System.Drawing.Point(565, 0);
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
            this.Txt_HieDesc.Size = new System.Drawing.Size(550, 25);
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
            this.pnlHie.Size = new System.Drawing.Size(630, 25);
            this.pnlHie.TabIndex = 88;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(550, 0);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(15, 25);
            // 
            // Pb_Search_Hie
            // 
            this.Pb_Search_Hie.BackColor = System.Drawing.Color.FromArgb(244, 244, 244);
            this.Pb_Search_Hie.CssStyle = "border-radius:25px ";
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
            // pnlParams
            // 
            this.pnlParams.Controls.Add(this.pnlDtes);
            this.pnlParams.Controls.Add(this.panel7);
            this.pnlParams.Controls.Add(this.panel4);
            this.pnlParams.Controls.Add(this.panel3);
            this.pnlParams.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlParams.Location = new System.Drawing.Point(0, 43);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new System.Drawing.Size(716, 140);
            this.pnlParams.TabIndex = 1;
            // 
            // pnlDtes
            // 
            this.pnlDtes.Controls.Add(this.lblReportFormdt);
            this.pnlDtes.Controls.Add(this.lblToDate);
            this.pnlDtes.Controls.Add(this.dtTodate);
            this.pnlDtes.Controls.Add(this.lblEjobTitleReq);
            this.pnlDtes.Controls.Add(this.dtFrom);
            this.pnlDtes.Controls.Add(this.label9);
            this.pnlDtes.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlDtes.Location = new System.Drawing.Point(0, 101);
            this.pnlDtes.Name = "pnlDtes";
            this.pnlDtes.Size = new System.Drawing.Size(716, 39);
            this.pnlDtes.TabIndex = 4;
            // 
            // lblReportFormdt
            // 
            this.lblReportFormdt.Location = new System.Drawing.Point(15, 7);
            this.lblReportFormdt.Name = "lblReportFormdt";
            this.lblReportFormdt.Size = new System.Drawing.Size(102, 16);
            this.lblReportFormdt.TabIndex = 2;
            this.lblReportFormdt.Text = "Report From Date";
            // 
            // lblToDate
            // 
            this.lblToDate.Location = new System.Drawing.Point(295, 7);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(45, 16);
            this.lblToDate.TabIndex = 2;
            this.lblToDate.Text = "To Date";
            // 
            // dtTodate
            // 
            this.dtTodate.AutoSize = false;
            this.dtTodate.CustomFormat = "MM/dd/yyyy";
            this.dtTodate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtTodate.Location = new System.Drawing.Point(364, 3);
            this.dtTodate.Name = "dtTodate";
            this.dtTodate.ShowCheckBox = true;
            this.dtTodate.ShowToolTips = false;
            this.dtTodate.Size = new System.Drawing.Size(116, 25);
            this.dtTodate.TabIndex = 2;
            // 
            // lblEjobTitleReq
            // 
            this.lblEjobTitleReq.Controls.Add(this.label7);
            this.lblEjobTitleReq.ForeColor = System.Drawing.Color.Red;
            this.lblEjobTitleReq.Location = new System.Drawing.Point(117, 5);
            this.lblEjobTitleReq.Name = "lblEjobTitleReq";
            this.lblEjobTitleReq.Size = new System.Drawing.Size(8, 12);
            this.lblEjobTitleReq.TabIndex = 0;
            this.lblEjobTitleReq.Text = "*";
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
            // dtFrom
            // 
            this.dtFrom.AutoSize = false;
            this.dtFrom.CustomFormat = "MM/dd/yyyy";
            this.dtFrom.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtFrom.Location = new System.Drawing.Point(145, 3);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.ShowCheckBox = true;
            this.dtFrom.ShowToolTips = false;
            this.dtFrom.Size = new System.Drawing.Size(116, 25);
            this.dtFrom.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.Controls.Add(this.label8);
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(341, 5);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(7, 11);
            this.label9.TabIndex = 0;
            this.label9.Text = "*";
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
            // panel7
            // 
            this.panel7.Controls.Add(this.rdoFundingAll);
            this.panel7.Controls.Add(this.rdoFundingSelect);
            this.panel7.Controls.Add(this.label14);
            this.panel7.Dock = Wisej.Web.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 70);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(716, 31);
            this.panel7.TabIndex = 3;
            // 
            // rdoFundingAll
            // 
            this.rdoFundingAll.AutoSize = false;
            this.rdoFundingAll.Checked = true;
            this.rdoFundingAll.Location = new System.Drawing.Point(142, 4);
            this.rdoFundingAll.Name = "rdoFundingAll";
            this.rdoFundingAll.Size = new System.Drawing.Size(42, 21);
            this.rdoFundingAll.TabIndex = 1;
            this.rdoFundingAll.TabStop = true;
            this.rdoFundingAll.Text = "All";
            this.rdoFundingAll.CheckedChanged += new System.EventHandler(this.rdoFundingAll_CheckedChanged);
            // 
            // rdoFundingSelect
            // 
            this.rdoFundingSelect.AutoSize = false;
            this.rdoFundingSelect.Location = new System.Drawing.Point(207, 4);
            this.rdoFundingSelect.Name = "rdoFundingSelect";
            this.rdoFundingSelect.Size = new System.Drawing.Size(76, 21);
            this.rdoFundingSelect.TabIndex = 2;
            this.rdoFundingSelect.Text = "Selected";
            this.rdoFundingSelect.Click += new System.EventHandler(this.rdoFundingSelect_Click);
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(15, 7);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(102, 16);
            this.label14.TabIndex = 37;
            this.label14.Text = "Funding Sources";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.rbPaid);
            this.panel4.Controls.Add(this.lblMeals);
            this.panel4.Controls.Add(this.rbReduced);
            this.panel4.Controls.Add(this.rbAllmeals);
            this.panel4.Controls.Add(this.rbFree);
            this.panel4.Dock = Wisej.Web.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 39);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(716, 31);
            this.panel4.TabIndex = 2;
            // 
            // rbPaid
            // 
            this.rbPaid.AutoSize = false;
            this.rbPaid.Location = new System.Drawing.Point(307, 4);
            this.rbPaid.Name = "rbPaid";
            this.rbPaid.Size = new System.Drawing.Size(52, 21);
            this.rbPaid.TabIndex = 3;
            this.rbPaid.Text = "Paid";
            // 
            // lblMeals
            // 
            this.lblMeals.Location = new System.Drawing.Point(15, 7);
            this.lblMeals.Name = "lblMeals";
            this.lblMeals.Size = new System.Drawing.Size(34, 16);
            this.lblMeals.TabIndex = 37;
            this.lblMeals.Text = "Meals";
            // 
            // rbReduced
            // 
            this.rbReduced.AutoSize = false;
            this.rbReduced.Location = new System.Drawing.Point(207, 4);
            this.rbReduced.Name = "rbReduced";
            this.rbReduced.Size = new System.Drawing.Size(77, 21);
            this.rbReduced.TabIndex = 2;
            this.rbReduced.Text = "Reduced";
            // 
            // rbAllmeals
            // 
            this.rbAllmeals.AutoSize = false;
            this.rbAllmeals.Checked = true;
            this.rbAllmeals.Location = new System.Drawing.Point(142, 4);
            this.rbAllmeals.Name = "rbAllmeals";
            this.rbAllmeals.Size = new System.Drawing.Size(42, 21);
            this.rbAllmeals.TabIndex = 1;
            this.rbAllmeals.TabStop = true;
            this.rbAllmeals.Text = "All";
            // 
            // rbFree
            // 
            this.rbFree.AutoSize = false;
            this.rbFree.Location = new System.Drawing.Point(382, 4);
            this.rbFree.Name = "rbFree";
            this.rbFree.Size = new System.Drawing.Size(53, 21);
            this.rbFree.TabIndex = 4;
            this.rbFree.Text = "Free";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rdoMultipleSites);
            this.panel3.Controls.Add(this.rdoAllSite);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Dock = Wisej.Web.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(716, 39);
            this.panel3.TabIndex = 1;
            // 
            // rdoMultipleSites
            // 
            this.rdoMultipleSites.AutoSize = false;
            this.rdoMultipleSites.Location = new System.Drawing.Point(252, 12);
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
            this.rdoAllSite.Location = new System.Drawing.Point(142, 12);
            this.rdoAllSite.Name = "rdoAllSite";
            this.rdoAllSite.Size = new System.Drawing.Size(93, 21);
            this.rdoAllSite.TabIndex = 1;
            this.rdoAllSite.TabStop = true;
            this.rdoAllSite.Text = "For All Sites";
            this.rdoAllSite.CheckedChanged += new System.EventHandler(this.rdoAllSite_CheckedChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(15, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 16);
            this.label4.TabIndex = 36;
            this.label4.Text = "Site";
            // 
            // btnGeneratePdf
            // 
            this.btnGeneratePdf.AppearanceKey = "button-reports";
            this.btnGeneratePdf.Dock = Wisej.Web.DockStyle.Right;
            this.btnGeneratePdf.Location = new System.Drawing.Point(528, 5);
            this.btnGeneratePdf.Name = "btnGeneratePdf";
            this.btnGeneratePdf.Size = new System.Drawing.Size(85, 25);
            this.btnGeneratePdf.TabIndex = 3;
            this.btnGeneratePdf.Text = "&Generate";
            this.btnGeneratePdf.Click += new System.EventHandler(this.btnGeneratePdf_Click);
            // 
            // btnPdfPreview
            // 
            this.btnPdfPreview.AppearanceKey = "button-reports";
            this.btnPdfPreview.Dock = Wisej.Web.DockStyle.Right;
            this.btnPdfPreview.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnPdfPreview.Location = new System.Drawing.Point(616, 5);
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
            this.btnPrint.Location = new System.Drawing.Point(465, 5);
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
            this.btnSaveParameters.Text = "Save Parameters";
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
            this.pnlGenerate.Controls.Add(this.spacer5);
            this.pnlGenerate.Controls.Add(this.btnPrint);
            this.pnlGenerate.Controls.Add(this.spacer4);
            this.pnlGenerate.Controls.Add(this.btnGeneratePdf);
            this.pnlGenerate.Controls.Add(this.spacer3);
            this.pnlGenerate.Controls.Add(this.btnPdfPreview);
            this.pnlGenerate.Controls.Add(this.btnGetParameters);
            this.pnlGenerate.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlGenerate.Location = new System.Drawing.Point(0, 183);
            this.pnlGenerate.Name = "pnlGenerate";
            this.pnlGenerate.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlGenerate.Size = new System.Drawing.Size(716, 35);
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
            this.spacer4.Location = new System.Drawing.Point(525, 5);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(3, 25);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Right;
            this.spacer3.Location = new System.Drawing.Point(613, 5);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(3, 25);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlParams);
            this.pnlCompleteForm.Controls.Add(this.panel5);
            this.pnlCompleteForm.Controls.Add(this.pnlGenerate);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(716, 218);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(11, 70, 117);
            this.panel5.Controls.Add(this.pnlFilter);
            this.panel5.Controls.Add(this.pnlHie);
            this.panel5.Dock = Wisej.Web.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new Wisej.Web.Padding(15, 9, 9, 9);
            this.panel5.Size = new System.Drawing.Size(716, 43);
            this.panel5.TabIndex = 99;
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.Pb_Search_Hie);
            this.pnlFilter.Controls.Add(this.spacer2);
            this.pnlFilter.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlFilter.Location = new System.Drawing.Point(645, 9);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(62, 25);
            this.pnlFilter.TabIndex = 55;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(0, 0);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(15, 25);
            // 
            // HSSB2112_Report
            // 
            this.ClientSize = new System.Drawing.Size(716, 218);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSSB2112_Report";
            this.Text = "HSSB2112_Report";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "tlHelp";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.HSSB2112_Report_ToolClick);
            this.pnlHie.ResumeLayout(false);
            this.pnlHie.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).EndInit();
            this.pnlParams.ResumeLayout(false);
            this.pnlDtes.ResumeLayout(false);
            this.lblEjobTitleReq.ResumeLayout(false);
            this.lblEjobTitleReq.PerformLayout();
            this.label9.ResumeLayout(false);
            this.label9.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.pnlGenerate.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComboBox CmbYear;
        private TextBox Txt_HieDesc;
        private Panel pnlHie;
        private PictureBox Pb_Search_Hie;
        private Panel pnlParams;
        private Label lblEjobTitleReq;
        private Label label7;
        private Label label9;
        private Label label8;
        private Label lblReportFormdt;
        private Label label4;
        private Label lblToDate;
        private DateTimePicker dtFrom;
        private Panel panel3;
        private RadioButton rdoMultipleSites;
        private RadioButton rdoAllSite;
        private DateTimePicker dtTodate;
        private Label label14;
        private Panel panel7;
        private RadioButton rdoFundingAll;
        private RadioButton rdoFundingSelect;
        private Panel panel4;
        private RadioButton rbPaid;
        private RadioButton rbReduced;
        private RadioButton rbAllmeals;
        private RadioButton rbFree;
        private Label lblMeals;
        private Button btnGeneratePdf;
        private Button btnPdfPreview;
        private Button btnPrint;
        private Button btnSaveParameters;
        private Button btnGetParameters;
        private Panel pnlGenerate;
        private Panel pnlCompleteForm;
        private Panel panel5;
        private Spacer spacer1;
        private Panel pnlFilter;
        private Spacer spacer2;
        private Spacer spacer3;
        private Spacer spacer4;
        private Spacer spacer5;
        private Panel pnlDtes;
    }
}