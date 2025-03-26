using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class CASB0008
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle10 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle11 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle12 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle13 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CASB0008));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.CmbYear = new Wisej.Web.ComboBox();
            this.Txt_HieDesc = new Wisej.Web.TextBox();
            this.pnlHie = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.Pb_Search_Hie = new Wisej.Web.PictureBox();
            this.pnlIntake = new Wisej.Web.Panel();
            this.pnlgvwApplicants = new Wisej.Web.Panel();
            this.gvwApplicants = new Wisej.Web.DataGridView();
            this.gvDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvYes = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvNo = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Selected = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Col_Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.chkbCurrProg = new Wisej.Web.CheckBox();
            this.pnlIntakeDates = new Wisej.Web.Panel();
            this.cmbCaseType = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.lblCaseType = new Wisej.Web.Label();
            this.btnProcess = new Wisej.Web.Button();
            this.btnClear = new Wisej.Web.Button();
            this.lblDate = new Wisej.Web.Label();
            this.dtpFrmDate = new Wisej.Web.DateTimePicker();
            this.dtpToDt = new Wisej.Web.DateTimePicker();
            this.lblToDt = new Wisej.Web.Label();
            this.chkbExcel = new Wisej.Web.CheckBox();
            this.btnSaveParameters = new Wisej.Web.Button();
            this.btnGetParameters = new Wisej.Web.Button();
            this.btnGenPdf = new Wisej.Web.Button();
            this.BtnPdfPrev = new Wisej.Web.Button();
            this.pnlGeneratePDF = new Wisej.Web.Panel();
            this.spacer4 = new Wisej.Web.Spacer();
            this.spacer3 = new Wisej.Web.Spacer();
            this.spacer2 = new Wisej.Web.Spacer();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlHieandFilter = new Wisej.Web.Panel();
            this.pnlFilter = new Wisej.Web.Panel();
            this.SCheck1 = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.pnlHie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).BeginInit();
            this.pnlIntake.SuspendLayout();
            this.pnlgvwApplicants.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwApplicants)).BeginInit();
            this.gvwApplicants.SuspendLayout();
            this.pnlIntakeDates.SuspendLayout();
            this.pnlGeneratePDF.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlHieandFilter.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // CmbYear
            // 
            this.CmbYear.Dock = Wisej.Web.DockStyle.Left;
            this.CmbYear.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbYear.FormattingEnabled = true;
            this.CmbYear.Location = new System.Drawing.Point(728, 9);
            this.CmbYear.Name = "CmbYear";
            this.CmbYear.Size = new System.Drawing.Size(58, 25);
            this.CmbYear.TabIndex = 0;
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
            this.Txt_HieDesc.Location = new System.Drawing.Point(15, 9);
            this.Txt_HieDesc.Name = "Txt_HieDesc";
            this.Txt_HieDesc.ReadOnly = true;
            this.Txt_HieDesc.Size = new System.Drawing.Size(699, 25);
            this.Txt_HieDesc.TabIndex = 0;
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
            this.pnlHie.Location = new System.Drawing.Point(0, 0);
            this.pnlHie.Name = "pnlHie";
            this.pnlHie.Padding = new Wisej.Web.Padding(15, 9, 9, 9);
            this.pnlHie.Size = new System.Drawing.Size(795, 43);
            this.pnlHie.TabIndex = 0;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(714, 9);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(14, 25);
            // 
            // Pb_Search_Hie
            // 
            this.Pb_Search_Hie.BackColor = System.Drawing.Color.FromArgb(244, 244, 244);
            this.Pb_Search_Hie.CssStyle = "border-radius:25px";
            this.Pb_Search_Hie.Cursor = Wisej.Web.Cursors.Hand;
            this.Pb_Search_Hie.Dock = Wisej.Web.DockStyle.Right;
            this.Pb_Search_Hie.ForeColor = System.Drawing.Color.FromName("@windowText");
            this.Pb_Search_Hie.ImageSource = "captain-filter";
            this.Pb_Search_Hie.Location = new System.Drawing.Point(14, 9);
            this.Pb_Search_Hie.Name = "Pb_Search_Hie";
            this.Pb_Search_Hie.Padding = new Wisej.Web.Padding(4, 5, 4, 4);
            this.Pb_Search_Hie.Size = new System.Drawing.Size(25, 25);
            this.Pb_Search_Hie.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.Pb_Search_Hie.ToolTipText = "Select Hierarchy";
            this.Pb_Search_Hie.Click += new System.EventHandler(this.Pb_Search_Hie_Click);
            // 
            // pnlIntake
            // 
            this.pnlIntake.Controls.Add(this.pnlgvwApplicants);
            this.pnlIntake.Controls.Add(this.pnlIntakeDates);
            this.pnlIntake.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlIntake.Location = new System.Drawing.Point(0, 43);
            this.pnlIntake.Name = "pnlIntake";
            this.pnlIntake.Size = new System.Drawing.Size(843, 313);
            this.pnlIntake.TabIndex = 0;
            // 
            // pnlgvwApplicants
            // 
            this.pnlgvwApplicants.Controls.Add(this.gvwApplicants);
            this.pnlgvwApplicants.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwApplicants.Location = new System.Drawing.Point(0, 68);
            this.pnlgvwApplicants.Name = "pnlgvwApplicants";
            this.pnlgvwApplicants.Size = new System.Drawing.Size(843, 245);
            this.pnlgvwApplicants.TabIndex = 6;
            // 
            // gvwApplicants
            // 
            this.gvwApplicants.AllowUserToOrderColumns = true;
            this.gvwApplicants.AllowUserToResizeColumns = false;
            this.gvwApplicants.AllowUserToResizeRows = false;
            this.gvwApplicants.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwApplicants.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwApplicants.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.SCheck1,
            this.gvDesc,
            this.gvYes,
            this.gvNo,
            this.Selected,
            this.Col_Code});
            this.gvwApplicants.Controls.Add(this.chkbCurrProg);
            this.gvwApplicants.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwApplicants.Location = new System.Drawing.Point(0, 0);
            this.gvwApplicants.Name = "gvwApplicants";
            this.gvwApplicants.RowHeadersWidth = 20;
            this.gvwApplicants.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwApplicants.Size = new System.Drawing.Size(843, 245);
            this.gvwApplicants.TabIndex = 7;
            this.gvwApplicants.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwApplicants_CellClick);
            // 
            // gvDesc
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvDesc.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvDesc.HeaderStyle = dataGridViewCellStyle5;
            this.gvDesc.HeaderText = "Description";
            this.gvDesc.Name = "gvDesc";
            this.gvDesc.ReadOnly = true;
            this.gvDesc.Width = 400;
            // 
            // gvYes
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvYes.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvYes.HeaderStyle = dataGridViewCellStyle7;
            this.gvYes.HeaderText = "Yes";
            this.gvYes.Name = "gvYes";
            this.gvYes.ReadOnly = true;
            // 
            // gvNo
            // 
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvNo.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvNo.HeaderStyle = dataGridViewCellStyle9;
            this.gvNo.HeaderText = "No";
            this.gvNo.Name = "gvNo";
            this.gvNo.ReadOnly = true;
            // 
            // Selected
            // 
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Selected.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Selected.HeaderStyle = dataGridViewCellStyle11;
            this.Selected.HeaderText = "Selected";
            this.Selected.Name = "Selected";
            this.Selected.ShowInVisibilityMenu = false;
            this.Selected.Visible = false;
            this.Selected.Width = 20;
            // 
            // Col_Code
            // 
            dataGridViewCellStyle12.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Col_Code.DefaultCellStyle = dataGridViewCellStyle12;
            dataGridViewCellStyle13.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Col_Code.HeaderStyle = dataGridViewCellStyle13;
            this.Col_Code.HeaderText = "Col_Code";
            this.Col_Code.Name = "Col_Code";
            this.Col_Code.ShowInVisibilityMenu = false;
            this.Col_Code.Visible = false;
            this.Col_Code.Width = 25;
            // 
            // chkbCurrProg
            // 
            this.chkbCurrProg.AutoSize = false;
            this.chkbCurrProg.Location = new System.Drawing.Point(696, 248);
            this.chkbCurrProg.Name = "chkbCurrProg";
            this.chkbCurrProg.Size = new System.Drawing.Size(199, 20);
            this.chkbCurrProg.TabIndex = 0;
            this.chkbCurrProg.TabStop = false;
            this.chkbCurrProg.Text = "Counts from Current Program";
            this.chkbCurrProg.Visible = false;
            // 
            // pnlIntakeDates
            // 
            this.pnlIntakeDates.Controls.Add(this.cmbCaseType);
            this.pnlIntakeDates.Controls.Add(this.lblCaseType);
            this.pnlIntakeDates.Controls.Add(this.btnProcess);
            this.pnlIntakeDates.Controls.Add(this.btnClear);
            this.pnlIntakeDates.Controls.Add(this.lblDate);
            this.pnlIntakeDates.Controls.Add(this.dtpFrmDate);
            this.pnlIntakeDates.Controls.Add(this.dtpToDt);
            this.pnlIntakeDates.Controls.Add(this.lblToDt);
            this.pnlIntakeDates.Dock = Wisej.Web.DockStyle.Top;
            this.pnlIntakeDates.Location = new System.Drawing.Point(0, 0);
            this.pnlIntakeDates.Name = "pnlIntakeDates";
            this.pnlIntakeDates.Padding = new Wisej.Web.Padding(5);
            this.pnlIntakeDates.Size = new System.Drawing.Size(843, 68);
            this.pnlIntakeDates.TabIndex = 1;
            // 
            // cmbCaseType
            // 
            this.cmbCaseType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbCaseType.Location = new System.Drawing.Point(121, 37);
            this.cmbCaseType.Name = "cmbCaseType";
            this.cmbCaseType.Size = new System.Drawing.Size(203, 25);
            this.cmbCaseType.TabIndex = 7;
            this.cmbCaseType.SelectedIndexChanged += new System.EventHandler(this.cmbCaseType_SelectedIndexChanged);
            // 
            // lblCaseType
            // 
            this.lblCaseType.Location = new System.Drawing.Point(15, 42);
            this.lblCaseType.Name = "lblCaseType";
            this.lblCaseType.Size = new System.Drawing.Size(58, 16);
            this.lblCaseType.TabIndex = 6;
            this.lblCaseType.Text = "Case Type";
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(433, 5);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(80, 25);
            this.btnProcess.TabIndex = 4;
            this.btnProcess.Text = "&Process";
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(517, 5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 25);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "&Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblDate
            // 
            this.lblDate.Location = new System.Drawing.Point(15, 9);
            this.lblDate.Name = "lblDate";
            this.lblDate.RightToLeft = Wisej.Web.RightToLeft.No;
            this.lblDate.Size = new System.Drawing.Size(97, 14);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "Intake Start Date";
            // 
            // dtpFrmDate
            // 
            this.dtpFrmDate.AutoSize = false;
            this.dtpFrmDate.Checked = false;
            this.dtpFrmDate.CustomFormat = "MM/dd/yyyy";
            this.dtpFrmDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpFrmDate.Location = new System.Drawing.Point(121, 5);
            this.dtpFrmDate.Name = "dtpFrmDate";
            this.dtpFrmDate.ShowToolTips = false;
            this.dtpFrmDate.Size = new System.Drawing.Size(100, 25);
            this.dtpFrmDate.TabIndex = 2;
            // 
            // dtpToDt
            // 
            this.dtpToDt.AutoSize = false;
            this.dtpToDt.Checked = false;
            this.dtpToDt.CustomFormat = " MM/dd/yyyy";
            this.dtpToDt.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpToDt.Location = new System.Drawing.Point(307, 5);
            this.dtpToDt.Name = "dtpToDt";
            this.dtpToDt.ShowToolTips = false;
            this.dtpToDt.Size = new System.Drawing.Size(100, 25);
            this.dtpToDt.TabIndex = 3;
            // 
            // lblToDt
            // 
            this.lblToDt.Location = new System.Drawing.Point(247, 9);
            this.lblToDt.Name = "lblToDt";
            this.lblToDt.RightToLeft = Wisej.Web.RightToLeft.No;
            this.lblToDt.Size = new System.Drawing.Size(52, 14);
            this.lblToDt.TabIndex = 0;
            this.lblToDt.Text = "End Date";
            // 
            // chkbExcel
            // 
            this.chkbExcel.AutoSize = false;
            this.chkbExcel.Dock = Wisej.Web.DockStyle.Left;
            this.chkbExcel.Location = new System.Drawing.Point(418, 5);
            this.chkbExcel.Name = "chkbExcel";
            this.chkbExcel.Size = new System.Drawing.Size(112, 25);
            this.chkbExcel.TabIndex = 0;
            this.chkbExcel.TabStop = false;
            this.chkbExcel.Text = "Generate Excel";
            this.chkbExcel.Visible = false;
            // 
            // btnSaveParameters
            // 
            this.btnSaveParameters.AppearanceKey = "button-reports";
            this.btnSaveParameters.Dock = Wisej.Web.DockStyle.Left;
            this.btnSaveParameters.Location = new System.Drawing.Point(128, 5);
            this.btnSaveParameters.Name = "btnSaveParameters";
            this.btnSaveParameters.Size = new System.Drawing.Size(110, 25);
            this.btnSaveParameters.TabIndex = 10;
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
            this.btnGetParameters.TabIndex = 9;
            this.btnGetParameters.Text = "&Get Parameters";
            this.btnGetParameters.Click += new System.EventHandler(this.btnGetParameters_Click);
            // 
            // btnGenPdf
            // 
            this.btnGenPdf.AppearanceKey = "button-reports";
            this.btnGenPdf.Dock = Wisej.Web.DockStyle.Right;
            this.btnGenPdf.Location = new System.Drawing.Point(665, 5);
            this.btnGenPdf.Name = "btnGenPdf";
            this.btnGenPdf.Size = new System.Drawing.Size(80, 25);
            this.btnGenPdf.TabIndex = 11;
            this.btnGenPdf.Text = "G&enerate";
            this.btnGenPdf.Click += new System.EventHandler(this.BtnGenPdf_Click);
            // 
            // BtnPdfPrev
            // 
            this.BtnPdfPrev.AppearanceKey = "button-reports";
            this.BtnPdfPrev.Dock = Wisej.Web.DockStyle.Right;
            this.BtnPdfPrev.Location = new System.Drawing.Point(748, 5);
            this.BtnPdfPrev.Name = "BtnPdfPrev";
            this.BtnPdfPrev.Size = new System.Drawing.Size(80, 25);
            this.BtnPdfPrev.TabIndex = 12;
            this.BtnPdfPrev.Text = "Pre&view";
            this.BtnPdfPrev.Click += new System.EventHandler(this.BtnPdfPrev_Click);
            // 
            // pnlGeneratePDF
            // 
            this.pnlGeneratePDF.AppearanceKey = "panel-grdo";
            this.pnlGeneratePDF.Controls.Add(this.chkbExcel);
            this.pnlGeneratePDF.Controls.Add(this.spacer4);
            this.pnlGeneratePDF.Controls.Add(this.btnSaveParameters);
            this.pnlGeneratePDF.Controls.Add(this.spacer3);
            this.pnlGeneratePDF.Controls.Add(this.btnGenPdf);
            this.pnlGeneratePDF.Controls.Add(this.spacer2);
            this.pnlGeneratePDF.Controls.Add(this.btnGetParameters);
            this.pnlGeneratePDF.Controls.Add(this.BtnPdfPrev);
            this.pnlGeneratePDF.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlGeneratePDF.Location = new System.Drawing.Point(0, 356);
            this.pnlGeneratePDF.Name = "pnlGeneratePDF";
            this.pnlGeneratePDF.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlGeneratePDF.Size = new System.Drawing.Size(843, 35);
            this.pnlGeneratePDF.TabIndex = 8;
            // 
            // spacer4
            // 
            this.spacer4.Dock = Wisej.Web.DockStyle.Left;
            this.spacer4.Location = new System.Drawing.Point(238, 5);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(180, 25);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Left;
            this.spacer3.Location = new System.Drawing.Point(125, 5);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(3, 25);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Right;
            this.spacer2.Location = new System.Drawing.Point(745, 5);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(3, 25);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlIntake);
            this.pnlCompleteForm.Controls.Add(this.pnlHieandFilter);
            this.pnlCompleteForm.Controls.Add(this.pnlGeneratePDF);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(843, 391);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlHieandFilter
            // 
            this.pnlHieandFilter.Controls.Add(this.pnlFilter);
            this.pnlHieandFilter.Controls.Add(this.pnlHie);
            this.pnlHieandFilter.Dock = Wisej.Web.DockStyle.Top;
            this.pnlHieandFilter.Location = new System.Drawing.Point(0, 0);
            this.pnlHieandFilter.Name = "pnlHieandFilter";
            this.pnlHieandFilter.Size = new System.Drawing.Size(843, 43);
            this.pnlHieandFilter.TabIndex = 0;
            // 
            // pnlFilter
            // 
            this.pnlFilter.BackColor = System.Drawing.Color.FromArgb(11, 70, 117);
            this.pnlFilter.Controls.Add(this.Pb_Search_Hie);
            this.pnlFilter.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlFilter.Location = new System.Drawing.Point(795, 0);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Padding = new Wisej.Web.Padding(9);
            this.pnlFilter.Size = new System.Drawing.Size(48, 43);
            this.pnlFilter.TabIndex = 0;
            // 
            // SCheck1
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.NullValue = null;
            this.SCheck1.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.SCheck1.HeaderStyle = dataGridViewCellStyle3;
            this.SCheck1.HeaderText = " ";
            this.SCheck1.Name = "SCheck1";
            this.SCheck1.ReadOnly = true;
            this.SCheck1.ShowInVisibilityMenu = false;
            this.SCheck1.Width = 30;
            // 
            // CASB0008
            // 
            this.ClientSize = new System.Drawing.Size(843, 391);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CASB0008";
            this.Text = "Customer Intake Quality Control";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "tlHelp";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.CASB0008_ToolClick);
            this.pnlHie.ResumeLayout(false);
            this.pnlHie.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).EndInit();
            this.pnlIntake.ResumeLayout(false);
            this.pnlgvwApplicants.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwApplicants)).EndInit();
            this.gvwApplicants.ResumeLayout(false);
            this.pnlIntakeDates.ResumeLayout(false);
            this.pnlIntakeDates.PerformLayout();
            this.pnlGeneratePDF.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlHieandFilter.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private ComboBox CmbYear;
        private TextBox Txt_HieDesc;
        private Panel pnlHie;
        private PictureBox Pb_Search_Hie;
        private Panel pnlIntake;
        private Label lblToDt;
        private DateTimePicker dtpToDt;
        private DateTimePicker dtpFrmDate;
        private Label lblDate;
        private Button btnProcess;
        private Button btnClear;
        private DataGridView gvwApplicants;
        private DataGridViewTextBoxColumn gvDesc;
        private DataGridViewTextBoxColumn gvYes;
        private DataGridViewTextBoxColumn gvNo;
        private CheckBox chkbExcel;
        private Button btnSaveParameters;
        private Button btnGetParameters;
        private Button btnGenPdf;
        private Button BtnPdfPrev;
        private Panel pnlGeneratePDF;
        private DataGridViewTextBoxColumn Selected;
        private DataGridViewTextBoxColumn Col_Code;
        private CheckBox chkbCurrProg;
        private Panel pnlCompleteForm;
        private Panel pnlHieandFilter;
        private Spacer spacer1;
        private Panel pnlgvwApplicants;
        private Panel pnlIntakeDates;
        private Spacer spacer4;
        private Spacer spacer3;
        private Spacer spacer2;
        private Panel pnlFilter;
        private ComboBoxEx cmbCaseType;
        private Label lblCaseType;
        private DataGridViewCheckBoxColumn SCheck1;
    }
}