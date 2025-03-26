using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class CASB0009_Report
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CASB0009_Report));
            this.Pb_Search_Hie = new Wisej.Web.PictureBox();
            this.CmbYear = new Wisej.Web.ComboBox();
            this.txtHieDesc = new Wisej.Web.TextBox();
            this.pnlHie = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlDetails = new Wisej.Web.Panel();
            this.pnlDateRange = new Wisej.Web.Panel();
            this.Rep_To_Date = new Wisej.Web.DateTimePicker();
            this.lblReportPeriodDate = new Wisej.Web.Label();
            this.lblReportTo = new Wisej.Web.Label();
            this.Rep_From_Date = new Wisej.Web.DateTimePicker();
            this.pnlName = new Wisej.Web.Panel();
            this.rdoSelected = new Wisej.Web.RadioButton();
            this.rdoAll = new Wisej.Web.RadioButton();
            this.lblName = new Wisej.Web.Label();
            this.pnlType = new Wisej.Web.Panel();
            this.chkHeader = new Wisej.Web.CheckBox();
            this.rbCAL = new Wisej.Web.RadioButton();
            this.rbSAL = new Wisej.Web.RadioButton();
            this.lblType = new Wisej.Web.Label();
            this.chkbExcel = new Wisej.Web.CheckBox();
            this.btnSaveParameters = new Wisej.Web.Button();
            this.btnGetParameters = new Wisej.Web.Button();
            this.BtnGenPdf = new Wisej.Web.Button();
            this.BtnPdfPrev = new Wisej.Web.Button();
            this.pnlGenerate = new Wisej.Web.Panel();
            this.spacer3 = new Wisej.Web.Spacer();
            this.spacer2 = new Wisej.Web.Spacer();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.PnlHieandFilter = new Wisej.Web.Panel();
            this.pnlHieFilter = new Wisej.Web.Panel();
            this.spacer4 = new Wisej.Web.Spacer();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).BeginInit();
            this.pnlHie.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.pnlDateRange.SuspendLayout();
            this.pnlName.SuspendLayout();
            this.pnlType.SuspendLayout();
            this.pnlGenerate.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.PnlHieandFilter.SuspendLayout();
            this.pnlHieFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // Pb_Search_Hie
            // 
            this.Pb_Search_Hie.BackColor = System.Drawing.Color.FromArgb(244, 244, 244);
            this.Pb_Search_Hie.CssStyle = "border-radius:25px";
            this.Pb_Search_Hie.Cursor = Wisej.Web.Cursors.Hand;
            this.Pb_Search_Hie.Dock = Wisej.Web.DockStyle.Left;
            this.Pb_Search_Hie.ImageSource = "captain-filter";
            this.Pb_Search_Hie.Location = new System.Drawing.Point(29, 9);
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
            this.CmbYear.Location = new System.Drawing.Point(637, 9);
            this.CmbYear.Name = "CmbYear";
            this.CmbYear.Size = new System.Drawing.Size(60, 25);
            this.CmbYear.TabIndex = 0;
            this.CmbYear.TabStop = false;
            this.CmbYear.Visible = false;
            this.CmbYear.SelectedIndexChanged += new System.EventHandler(this.CmbYear_SelectedIndexChanged);
            // 
            // txtHieDesc
            // 
            this.txtHieDesc.BackColor = System.Drawing.Color.Transparent;
            this.txtHieDesc.BorderStyle = Wisej.Web.BorderStyle.None;
            this.txtHieDesc.Dock = Wisej.Web.DockStyle.Left;
            this.txtHieDesc.Font = new System.Drawing.Font("defaultBold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.txtHieDesc.ForeColor = System.Drawing.Color.White;
            this.txtHieDesc.Location = new System.Drawing.Point(15, 9);
            this.txtHieDesc.Name = "txtHieDesc";
            this.txtHieDesc.ReadOnly = true;
            this.txtHieDesc.Size = new System.Drawing.Size(606, 25);
            this.txtHieDesc.TabIndex = 0;
            this.txtHieDesc.TabStop = false;
            this.txtHieDesc.TextAlign = Wisej.Web.HorizontalAlignment.Center;
            // 
            // pnlHie
            // 
            this.pnlHie.BackColor = System.Drawing.Color.FromArgb(11, 70, 117);
            this.pnlHie.Controls.Add(this.CmbYear);
            this.pnlHie.Controls.Add(this.spacer1);
            this.pnlHie.Controls.Add(this.txtHieDesc);
            this.pnlHie.Dock = Wisej.Web.DockStyle.Left;
            this.pnlHie.Location = new System.Drawing.Point(0, 0);
            this.pnlHie.Name = "pnlHie";
            this.pnlHie.Padding = new Wisej.Web.Padding(15, 9, 9, 9);
            this.pnlHie.Selectable = true;
            this.pnlHie.Size = new System.Drawing.Size(703, 43);
            this.pnlHie.TabIndex = 0;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(621, 9);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(16, 25);
            // 
            // pnlDetails
            // 
            this.pnlDetails.Controls.Add(this.pnlDateRange);
            this.pnlDetails.Controls.Add(this.pnlName);
            this.pnlDetails.Controls.Add(this.pnlType);
            this.pnlDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlDetails.Location = new System.Drawing.Point(0, 43);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Size = new System.Drawing.Size(773, 93);
            this.pnlDetails.TabIndex = 0;
            // 
            // pnlDateRange
            // 
            this.pnlDateRange.Controls.Add(this.Rep_To_Date);
            this.pnlDateRange.Controls.Add(this.lblReportPeriodDate);
            this.pnlDateRange.Controls.Add(this.lblReportTo);
            this.pnlDateRange.Controls.Add(this.Rep_From_Date);
            this.pnlDateRange.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlDateRange.Location = new System.Drawing.Point(0, 56);
            this.pnlDateRange.Name = "pnlDateRange";
            this.pnlDateRange.Size = new System.Drawing.Size(773, 37);
            this.pnlDateRange.TabIndex = 3;
            // 
            // Rep_To_Date
            // 
            this.Rep_To_Date.AutoSize = false;
            this.Rep_To_Date.Checked = false;
            this.Rep_To_Date.CustomFormat = "MM/dd/yyyy";
            this.Rep_To_Date.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.Rep_To_Date.Location = new System.Drawing.Point(302, 5);
            this.Rep_To_Date.Name = "Rep_To_Date";
            this.Rep_To_Date.ShowCheckBox = true;
            this.Rep_To_Date.ShowToolTips = false;
            this.Rep_To_Date.Size = new System.Drawing.Size(116, 25);
            this.Rep_To_Date.TabIndex = 2;
            // 
            // lblReportPeriodDate
            // 
            this.lblReportPeriodDate.Location = new System.Drawing.Point(15, 9);
            this.lblReportPeriodDate.Name = "lblReportPeriodDate";
            this.lblReportPeriodDate.Size = new System.Drawing.Size(99, 16);
            this.lblReportPeriodDate.TabIndex = 2;
            this.lblReportPeriodDate.Text = "Date Range From";
            // 
            // lblReportTo
            // 
            this.lblReportTo.Location = new System.Drawing.Point(272, 9);
            this.lblReportTo.Name = "lblReportTo";
            this.lblReportTo.Size = new System.Drawing.Size(14, 14);
            this.lblReportTo.TabIndex = 8;
            this.lblReportTo.Text = "To";
            // 
            // Rep_From_Date
            // 
            this.Rep_From_Date.AutoSize = false;
            this.Rep_From_Date.Checked = false;
            this.Rep_From_Date.CustomFormat = "MM/dd/yyyy";
            this.Rep_From_Date.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.Rep_From_Date.Location = new System.Drawing.Point(130, 5);
            this.Rep_From_Date.Name = "Rep_From_Date";
            this.Rep_From_Date.ShowCheckBox = true;
            this.Rep_From_Date.ShowToolTips = false;
            this.Rep_From_Date.Size = new System.Drawing.Size(116, 25);
            this.Rep_From_Date.TabIndex = 1;
            // 
            // pnlName
            // 
            this.pnlName.Controls.Add(this.rdoSelected);
            this.pnlName.Controls.Add(this.rdoAll);
            this.pnlName.Controls.Add(this.lblName);
            this.pnlName.Dock = Wisej.Web.DockStyle.Top;
            this.pnlName.Location = new System.Drawing.Point(0, 28);
            this.pnlName.Name = "pnlName";
            this.pnlName.Size = new System.Drawing.Size(773, 28);
            this.pnlName.TabIndex = 2;
            // 
            // rdoSelected
            // 
            this.rdoSelected.AutoSize = false;
            this.rdoSelected.Location = new System.Drawing.Point(184, 6);
            this.rdoSelected.Name = "rdoSelected";
            this.rdoSelected.Size = new System.Drawing.Size(123, 20);
            this.rdoSelected.TabIndex = 2;
            this.rdoSelected.Text = "Multiple SAL/CAL";
            this.rdoSelected.Click += new System.EventHandler(this.rdoSelected_Click);
            // 
            // rdoAll
            // 
            this.rdoAll.AutoSize = false;
            this.rdoAll.Checked = true;
            this.rdoAll.Location = new System.Drawing.Point(130, 6);
            this.rdoAll.Name = "rdoAll";
            this.rdoAll.Size = new System.Drawing.Size(41, 20);
            this.rdoAll.TabIndex = 1;
            this.rdoAll.TabStop = true;
            this.rdoAll.Text = "All ";
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(15, 9);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(85, 14);
            this.lblName.TabIndex = 36;
            this.lblName.Text = "SAL/CAL Name";
            // 
            // pnlType
            // 
            this.pnlType.Controls.Add(this.chkHeader);
            this.pnlType.Controls.Add(this.rbCAL);
            this.pnlType.Controls.Add(this.rbSAL);
            this.pnlType.Controls.Add(this.lblType);
            this.pnlType.Dock = Wisej.Web.DockStyle.Top;
            this.pnlType.Location = new System.Drawing.Point(0, 0);
            this.pnlType.Name = "pnlType";
            this.pnlType.Size = new System.Drawing.Size(773, 28);
            this.pnlType.TabIndex = 1;
            // 
            // chkHeader
            // 
            this.chkHeader.AutoSize = false;
            this.chkHeader.Location = new System.Drawing.Point(245, 6);
            this.chkHeader.Name = "chkHeader";
            this.chkHeader.Size = new System.Drawing.Size(99, 20);
            this.chkHeader.TabIndex = 3;
            this.chkHeader.Text = "Print Header";
            // 
            // rbCAL
            // 
            this.rbCAL.AutoSize = false;
            this.rbCAL.Location = new System.Drawing.Point(130, 6);
            this.rbCAL.Name = "rbCAL";
            this.rbCAL.Size = new System.Drawing.Size(48, 20);
            this.rbCAL.TabIndex = 1;
            this.rbCAL.Text = "CAL";
            this.rbCAL.Click += new System.EventHandler(this.rbSAL_Click);
            // 
            // rbSAL
            // 
            this.rbSAL.AutoSize = false;
            this.rbSAL.Checked = true;
            this.rbSAL.Location = new System.Drawing.Point(187, 6);
            this.rbSAL.Name = "rbSAL";
            this.rbSAL.Size = new System.Drawing.Size(47, 20);
            this.rbSAL.TabIndex = 2;
            this.rbSAL.TabStop = true;
            this.rbSAL.Text = "SAL";
            this.rbSAL.Click += new System.EventHandler(this.rbSAL_Click);
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(15, 9);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(28, 16);
            this.lblType.TabIndex = 36;
            this.lblType.Text = "Type";
            // 
            // chkbExcel
            // 
            this.chkbExcel.AutoSize = false;
            this.chkbExcel.Location = new System.Drawing.Point(327, 7);
            this.chkbExcel.Name = "chkbExcel";
            this.chkbExcel.Size = new System.Drawing.Size(112, 20);
            this.chkbExcel.TabIndex = 3;
            this.chkbExcel.Text = "Generate Excel";
            this.chkbExcel.Visible = false;
            // 
            // btnSaveParameters
            // 
            this.btnSaveParameters.AppearanceKey = "button-reports";
            this.btnSaveParameters.Dock = Wisej.Web.DockStyle.Left;
            this.btnSaveParameters.Location = new System.Drawing.Point(123, 5);
            this.btnSaveParameters.Name = "btnSaveParameters";
            this.btnSaveParameters.Size = new System.Drawing.Size(105, 26);
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
            this.btnGetParameters.Size = new System.Drawing.Size(105, 26);
            this.btnGetParameters.TabIndex = 1;
            this.btnGetParameters.Text = "Get &Parameters";
            this.btnGetParameters.Click += new System.EventHandler(this.btnGetParameters_Click);
            // 
            // BtnGenPdf
            // 
            this.BtnGenPdf.AppearanceKey = "button-reports";
            this.BtnGenPdf.Dock = Wisej.Web.DockStyle.Right;
            this.BtnGenPdf.Location = new System.Drawing.Point(625, 5);
            this.BtnGenPdf.Name = "BtnGenPdf";
            this.BtnGenPdf.Size = new System.Drawing.Size(65, 26);
            this.BtnGenPdf.TabIndex = 4;
            this.BtnGenPdf.Text = "&Generate";
            this.BtnGenPdf.Click += new System.EventHandler(this.BtnGenPdf_Click);
            // 
            // BtnPdfPrev
            // 
            this.BtnPdfPrev.AppearanceKey = "button-reports";
            this.BtnPdfPrev.Dock = Wisej.Web.DockStyle.Right;
            this.BtnPdfPrev.Location = new System.Drawing.Point(693, 5);
            this.BtnPdfPrev.Name = "BtnPdfPrev";
            this.BtnPdfPrev.Size = new System.Drawing.Size(65, 26);
            this.BtnPdfPrev.TabIndex = 5;
            this.BtnPdfPrev.Text = "Pre&view";
            this.BtnPdfPrev.Click += new System.EventHandler(this.BtnPdfPrev_Click);
            // 
            // pnlGenerate
            // 
            this.pnlGenerate.AppearanceKey = "panel-grdo";
            this.pnlGenerate.Controls.Add(this.btnSaveParameters);
            this.pnlGenerate.Controls.Add(this.spacer3);
            this.pnlGenerate.Controls.Add(this.BtnGenPdf);
            this.pnlGenerate.Controls.Add(this.spacer2);
            this.pnlGenerate.Controls.Add(this.chkbExcel);
            this.pnlGenerate.Controls.Add(this.btnGetParameters);
            this.pnlGenerate.Controls.Add(this.BtnPdfPrev);
            this.pnlGenerate.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlGenerate.Location = new System.Drawing.Point(0, 136);
            this.pnlGenerate.Name = "pnlGenerate";
            this.pnlGenerate.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlGenerate.Size = new System.Drawing.Size(773, 36);
            this.pnlGenerate.TabIndex = 4;
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Left;
            this.spacer3.Location = new System.Drawing.Point(120, 5);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(3, 26);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Right;
            this.spacer2.Location = new System.Drawing.Point(690, 5);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(3, 26);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlDetails);
            this.pnlCompleteForm.Controls.Add(this.PnlHieandFilter);
            this.pnlCompleteForm.Controls.Add(this.pnlGenerate);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(773, 172);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // PnlHieandFilter
            // 
            this.PnlHieandFilter.Controls.Add(this.pnlHieFilter);
            this.PnlHieandFilter.Controls.Add(this.pnlHie);
            this.PnlHieandFilter.Dock = Wisej.Web.DockStyle.Top;
            this.PnlHieandFilter.Location = new System.Drawing.Point(0, 0);
            this.PnlHieandFilter.Name = "PnlHieandFilter";
            this.PnlHieandFilter.Size = new System.Drawing.Size(773, 43);
            this.PnlHieandFilter.TabIndex = 0;
            // 
            // pnlHieFilter
            // 
            this.pnlHieFilter.BackColor = System.Drawing.Color.FromArgb(11, 70, 117);
            this.pnlHieFilter.Controls.Add(this.Pb_Search_Hie);
            this.pnlHieFilter.Controls.Add(this.spacer4);
            this.pnlHieFilter.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlHieFilter.Location = new System.Drawing.Point(703, 0);
            this.pnlHieFilter.Name = "pnlHieFilter";
            this.pnlHieFilter.Padding = new Wisej.Web.Padding(9);
            this.pnlHieFilter.Size = new System.Drawing.Size(70, 43);
            this.pnlHieFilter.TabIndex = 0;
            // 
            // spacer4
            // 
            this.spacer4.Dock = Wisej.Web.DockStyle.Left;
            this.spacer4.Location = new System.Drawing.Point(9, 9);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(20, 25);
            // 
            // CASB0009_Report
            // 
            this.ClientSize = new System.Drawing.Size(773, 172);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CASB0009_Report";
            this.Text = "CASB0009_Report";
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).EndInit();
            this.pnlHie.ResumeLayout(false);
            this.pnlHie.PerformLayout();
            this.pnlDetails.ResumeLayout(false);
            this.pnlDateRange.ResumeLayout(false);
            this.pnlName.ResumeLayout(false);
            this.pnlType.ResumeLayout(false);
            this.pnlGenerate.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.PnlHieandFilter.ResumeLayout(false);
            this.pnlHieFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private PictureBox Pb_Search_Hie;
        private ComboBox CmbYear;
        private TextBox txtHieDesc;
        private Panel pnlHie;
        private Panel pnlDetails;
        private Panel pnlName;
        private RadioButton rdoSelected;
        private RadioButton rdoAll;
        private Label lblName;
        private Panel pnlType;
        private RadioButton rbCAL;
        private RadioButton rbSAL;
        private Label lblType;
        private Label lblReportPeriodDate;
        private DateTimePicker Rep_From_Date;
        private Label lblReportTo;
        private DateTimePicker Rep_To_Date;
        private CheckBox chkbExcel;
        private Button btnSaveParameters;
        private Button btnGetParameters;
        private Button BtnGenPdf;
        private Button BtnPdfPrev;
        private Panel pnlGenerate;
        private CheckBox chkHeader;
        private Panel pnlCompleteForm;
        private Panel PnlHieandFilter;
        private Panel pnlHieFilter;
        private Panel pnlDateRange;
        private Spacer spacer3;
        private Spacer spacer2;
        private Spacer spacer1;
        private Spacer spacer4;
    }
}