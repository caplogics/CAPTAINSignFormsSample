using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class CASB0013Form
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CASB0013Form));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlgvwProgramData = new Wisej.Web.Panel();
            this.gvwProgramData = new Wisej.Web.DataGridView();
            this.gvtProgramDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtServicecode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtReportcount = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtRefcount = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel4 = new Wisej.Web.Panel();
            this.btnProcess = new Wisej.Web.Button();
            this.chkbUndupTable = new Wisej.Web.CheckBox();
            this.chkPrintAuditreport = new Wisej.Web.CheckBox();
            this.panel3 = new Wisej.Web.Panel();
            this.rdoProgcount = new Wisej.Web.RadioButton();
            this.rdoServiceCount = new Wisej.Web.RadioButton();
            this.lblRepType = new Wisej.Web.Label();
            this.label1 = new Wisej.Web.Label();
            this.panel2 = new Wisej.Web.Panel();
            this.Rep_To_Date = new Wisej.Web.DateTimePicker();
            this.lblReportfromDate = new Wisej.Web.Label();
            this.lblReportTo = new Wisej.Web.Label();
            this.Rep_From_Date = new Wisej.Web.DateTimePicker();
            this.lblReportPeriodDate = new Wisej.Web.Label();
            this.panel1 = new Wisej.Web.Panel();
            this.Ref_To_Date = new Wisej.Web.DateTimePicker();
            this.Ref_From_Date = new Wisej.Web.DateTimePicker();
            this.lblReferenceTo = new Wisej.Web.Label();
            this.lblreferenceFrom = new Wisej.Web.Label();
            this.lblReferenceperiodate = new Wisej.Web.Label();
            this.Date_Panel = new Wisej.Web.Panel();
            this.Rb_MS_Date = new Wisej.Web.RadioButton();
            this.Rb_MS_AddDate = new Wisej.Web.RadioButton();
            this.lblDateSelection = new Wisej.Web.Label();
            this.label3 = new Wisej.Web.Label();
            this.pnlHieFilter = new Wisej.Web.Panel();
            this.pnlFilter = new Wisej.Web.Panel();
            this.Pb_Search_Hie = new Wisej.Web.PictureBox();
            this.spacer4 = new Wisej.Web.Spacer();
            this.pnlHie = new Wisej.Web.Panel();
            this.CmbYear = new Wisej.Web.ComboBox();
            this.spacer3 = new Wisej.Web.Spacer();
            this.Txt_HieDesc = new Wisej.Web.TextBox();
            this.pnlGenerate = new Wisej.Web.Panel();
            this.btnPrint = new Wisej.Web.Button();
            this.spacer2 = new Wisej.Web.Spacer();
            this.btnGeneratePdf = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.chkbExcel = new Wisej.Web.CheckBox();
            this.btnPdfPreview = new Wisej.Web.Button();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlgvwProgramData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwProgramData)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.lblRepType.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.Date_Panel.SuspendLayout();
            this.lblDateSelection.SuspendLayout();
            this.pnlHieFilter.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).BeginInit();
            this.pnlHie.SuspendLayout();
            this.pnlGenerate.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlgvwProgramData);
            this.pnlCompleteForm.Controls.Add(this.panel4);
            this.pnlCompleteForm.Controls.Add(this.panel3);
            this.pnlCompleteForm.Controls.Add(this.panel2);
            this.pnlCompleteForm.Controls.Add(this.panel1);
            this.pnlCompleteForm.Controls.Add(this.Date_Panel);
            this.pnlCompleteForm.Controls.Add(this.pnlHieFilter);
            this.pnlCompleteForm.Controls.Add(this.pnlGenerate);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(799, 419);
            this.pnlCompleteForm.TabIndex = 1;
            // 
            // pnlgvwProgramData
            // 
            this.pnlgvwProgramData.Controls.Add(this.gvwProgramData);
            this.pnlgvwProgramData.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwProgramData.Location = new System.Drawing.Point(0, 195);
            this.pnlgvwProgramData.Name = "pnlgvwProgramData";
            this.pnlgvwProgramData.Size = new System.Drawing.Size(799, 189);
            this.pnlgvwProgramData.TabIndex = 23;
            // 
            // gvwProgramData
            // 
            this.gvwProgramData.AllowUserToOrderColumns = true;
            this.gvwProgramData.AllowUserToResizeColumns = false;
            this.gvwProgramData.AllowUserToResizeRows = false;
            this.gvwProgramData.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwProgramData.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwProgramData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwProgramData.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtProgramDesc,
            this.gvtServicecode,
            this.gvtReportcount,
            this.gvtRefcount});
            this.gvwProgramData.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwProgramData.Location = new System.Drawing.Point(0, 0);
            this.gvwProgramData.MultiSelect = false;
            this.gvwProgramData.Name = "gvwProgramData";
            this.gvwProgramData.ReadOnly = true;
            this.gvwProgramData.RowHeadersWidth = 14;
            this.gvwProgramData.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwProgramData.RowTemplate.Height = 26;
            this.gvwProgramData.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwProgramData.Size = new System.Drawing.Size(799, 189);
            this.gvwProgramData.TabIndex = 24;
            this.gvwProgramData.TabStop = false;
            // 
            // gvtProgramDesc
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtProgramDesc.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtProgramDesc.HeaderStyle = dataGridViewCellStyle3;
            this.gvtProgramDesc.HeaderText = "Program";
            this.gvtProgramDesc.Name = "gvtProgramDesc";
            this.gvtProgramDesc.ReadOnly = true;
            this.gvtProgramDesc.Width = 215;
            // 
            // gvtServicecode
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtServicecode.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtServicecode.HeaderStyle = dataGridViewCellStyle5;
            this.gvtServicecode.HeaderText = "Service";
            this.gvtServicecode.Name = "gvtServicecode";
            this.gvtServicecode.ReadOnly = true;
            this.gvtServicecode.Width = 215;
            // 
            // gvtReportcount
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtReportcount.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtReportcount.HeaderStyle = dataGridViewCellStyle7;
            this.gvtReportcount.HeaderText = "   ";
            this.gvtReportcount.Name = "gvtReportcount";
            this.gvtReportcount.ReadOnly = true;
            this.gvtReportcount.ShowInVisibilityMenu = false;
            this.gvtReportcount.Width = 160;
            // 
            // gvtRefcount
            // 
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtRefcount.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtRefcount.HeaderStyle = dataGridViewCellStyle9;
            this.gvtRefcount.HeaderText = "  ";
            this.gvtRefcount.Name = "gvtRefcount";
            this.gvtRefcount.ReadOnly = true;
            this.gvtRefcount.ShowInVisibilityMenu = false;
            this.gvtRefcount.Width = 160;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnProcess);
            this.panel4.Controls.Add(this.chkbUndupTable);
            this.panel4.Controls.Add(this.chkPrintAuditreport);
            this.panel4.Dock = Wisej.Web.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 163);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(799, 32);
            this.panel4.TabIndex = 19;
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(555, 3);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(80, 25);
            this.btnProcess.TabIndex = 22;
            this.btnProcess.Text = "&Process";
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // chkbUndupTable
            // 
            this.chkbUndupTable.AutoSize = false;
            this.chkbUndupTable.Location = new System.Drawing.Point(162, 4);
            this.chkbUndupTable.Name = "chkbUndupTable";
            this.chkbUndupTable.Size = new System.Drawing.Size(202, 20);
            this.chkbUndupTable.TabIndex = 20;
            this.chkbUndupTable.Text = "Unduplicated Program count";
            // 
            // chkPrintAuditreport
            // 
            this.chkPrintAuditreport.AutoSize = false;
            this.chkPrintAuditreport.Location = new System.Drawing.Point(377, 4);
            this.chkPrintAuditreport.Name = "chkPrintAuditreport";
            this.chkPrintAuditreport.Size = new System.Drawing.Size(129, 20);
            this.chkPrintAuditreport.TabIndex = 21;
            this.chkPrintAuditreport.Text = "Print Audit Report";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rdoProgcount);
            this.panel3.Controls.Add(this.rdoServiceCount);
            this.panel3.Controls.Add(this.lblRepType);
            this.panel3.Dock = Wisej.Web.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 133);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(799, 30);
            this.panel3.TabIndex = 16;
            // 
            // rdoProgcount
            // 
            this.rdoProgcount.AutoSize = false;
            this.rdoProgcount.Checked = true;
            this.rdoProgcount.Location = new System.Drawing.Point(162, 5);
            this.rdoProgcount.Name = "rdoProgcount";
            this.rdoProgcount.Size = new System.Drawing.Size(158, 20);
            this.rdoProgcount.TabIndex = 17;
            this.rdoProgcount.TabStop = true;
            this.rdoProgcount.Text = "Service Program Count";
            this.rdoProgcount.Click += new System.EventHandler(this.rdoServiceCount_Click);
            // 
            // rdoServiceCount
            // 
            this.rdoServiceCount.AutoSize = false;
            this.rdoServiceCount.Location = new System.Drawing.Point(342, 5);
            this.rdoServiceCount.Name = "rdoServiceCount";
            this.rdoServiceCount.Size = new System.Drawing.Size(203, 20);
            this.rdoServiceCount.TabIndex = 18;
            this.rdoServiceCount.Text = "Service Program/Service Count";
            this.rdoServiceCount.Click += new System.EventHandler(this.rdoServiceCount_Click);
            // 
            // lblRepType
            // 
            this.lblRepType.Controls.Add(this.label1);
            this.lblRepType.Location = new System.Drawing.Point(15, 8);
            this.lblRepType.Name = "lblRepType";
            this.lblRepType.Size = new System.Drawing.Size(69, 16);
            this.lblRepType.TabIndex = 2;
            this.lblRepType.Text = "Report Type";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-296, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.Rep_To_Date);
            this.panel2.Controls.Add(this.lblReportfromDate);
            this.panel2.Controls.Add(this.lblReportTo);
            this.panel2.Controls.Add(this.Rep_From_Date);
            this.panel2.Controls.Add(this.lblReportPeriodDate);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 103);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(799, 30);
            this.panel2.TabIndex = 13;
            // 
            // Rep_To_Date
            // 
            this.Rep_To_Date.AutoSize = false;
            this.Rep_To_Date.CustomFormat = "MM/dd/yyyy";
            this.Rep_To_Date.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.Rep_To_Date.Location = new System.Drawing.Point(370, 3);
            this.Rep_To_Date.Name = "Rep_To_Date";
            this.Rep_To_Date.ShowCheckBox = true;
            this.Rep_To_Date.ShowToolTips = false;
            this.Rep_To_Date.Size = new System.Drawing.Size(116, 25);
            this.Rep_To_Date.TabIndex = 15;
            // 
            // lblReportfromDate
            // 
            this.lblReportfromDate.Location = new System.Drawing.Point(165, 7);
            this.lblReportfromDate.Name = "lblReportfromDate";
            this.lblReportfromDate.Size = new System.Drawing.Size(30, 14);
            this.lblReportfromDate.TabIndex = 6;
            this.lblReportfromDate.Text = "From";
            // 
            // lblReportTo
            // 
            this.lblReportTo.Location = new System.Drawing.Point(346, 7);
            this.lblReportTo.Name = "lblReportTo";
            this.lblReportTo.Size = new System.Drawing.Size(14, 14);
            this.lblReportTo.TabIndex = 8;
            this.lblReportTo.Text = "To";
            // 
            // Rep_From_Date
            // 
            this.Rep_From_Date.AutoSize = false;
            this.Rep_From_Date.CustomFormat = "MM/dd/yyyy";
            this.Rep_From_Date.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.Rep_From_Date.Location = new System.Drawing.Point(205, 3);
            this.Rep_From_Date.Name = "Rep_From_Date";
            this.Rep_From_Date.ShowCheckBox = true;
            this.Rep_From_Date.ShowToolTips = false;
            this.Rep_From_Date.Size = new System.Drawing.Size(116, 25);
            this.Rep_From_Date.TabIndex = 14;
            // 
            // lblReportPeriodDate
            // 
            this.lblReportPeriodDate.Location = new System.Drawing.Point(15, 7);
            this.lblReportPeriodDate.Name = "lblReportPeriodDate";
            this.lblReportPeriodDate.Size = new System.Drawing.Size(109, 16);
            this.lblReportPeriodDate.TabIndex = 2;
            this.lblReportPeriodDate.Text = "Report Period Date";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Ref_To_Date);
            this.panel1.Controls.Add(this.Ref_From_Date);
            this.panel1.Controls.Add(this.lblReferenceTo);
            this.panel1.Controls.Add(this.lblreferenceFrom);
            this.panel1.Controls.Add(this.lblReferenceperiodate);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 73);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(799, 30);
            this.panel1.TabIndex = 10;
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
            this.Ref_To_Date.TabIndex = 12;
            // 
            // Ref_From_Date
            // 
            this.Ref_From_Date.AutoSize = false;
            this.Ref_From_Date.CustomFormat = "MM/dd/yyyy";
            this.Ref_From_Date.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.Ref_From_Date.Location = new System.Drawing.Point(205, 3);
            this.Ref_From_Date.Name = "Ref_From_Date";
            this.Ref_From_Date.ShowCheckBox = true;
            this.Ref_From_Date.ShowToolTips = false;
            this.Ref_From_Date.Size = new System.Drawing.Size(116, 25);
            this.Ref_From_Date.TabIndex = 11;
            // 
            // lblReferenceTo
            // 
            this.lblReferenceTo.Location = new System.Drawing.Point(346, 7);
            this.lblReferenceTo.Name = "lblReferenceTo";
            this.lblReferenceTo.Size = new System.Drawing.Size(14, 14);
            this.lblReferenceTo.TabIndex = 8;
            this.lblReferenceTo.Text = "To";
            // 
            // lblreferenceFrom
            // 
            this.lblreferenceFrom.Location = new System.Drawing.Point(165, 7);
            this.lblreferenceFrom.Name = "lblreferenceFrom";
            this.lblreferenceFrom.Size = new System.Drawing.Size(30, 14);
            this.lblreferenceFrom.TabIndex = 6;
            this.lblreferenceFrom.Text = "From";
            // 
            // lblReferenceperiodate
            // 
            this.lblReferenceperiodate.Location = new System.Drawing.Point(15, 7);
            this.lblReferenceperiodate.Name = "lblReferenceperiodate";
            this.lblReferenceperiodate.Size = new System.Drawing.Size(128, 16);
            this.lblReferenceperiodate.TabIndex = 2;
            this.lblReferenceperiodate.Text = "Reference Period Date";
            // 
            // Date_Panel
            // 
            this.Date_Panel.Controls.Add(this.Rb_MS_Date);
            this.Date_Panel.Controls.Add(this.Rb_MS_AddDate);
            this.Date_Panel.Controls.Add(this.lblDateSelection);
            this.Date_Panel.Dock = Wisej.Web.DockStyle.Top;
            this.Date_Panel.Location = new System.Drawing.Point(0, 43);
            this.Date_Panel.Name = "Date_Panel";
            this.Date_Panel.Size = new System.Drawing.Size(799, 30);
            this.Date_Panel.TabIndex = 7;
            // 
            // Rb_MS_Date
            // 
            this.Rb_MS_Date.AutoSize = false;
            this.Rb_MS_Date.Checked = true;
            this.Rb_MS_Date.Location = new System.Drawing.Point(162, 5);
            this.Rb_MS_Date.Name = "Rb_MS_Date";
            this.Rb_MS_Date.Size = new System.Drawing.Size(97, 20);
            this.Rb_MS_Date.TabIndex = 8;
            this.Rb_MS_Date.TabStop = true;
            this.Rb_MS_Date.Text = "Service Date";
            // 
            // Rb_MS_AddDate
            // 
            this.Rb_MS_AddDate.AutoSize = false;
            this.Rb_MS_AddDate.Location = new System.Drawing.Point(278, 5);
            this.Rb_MS_AddDate.Name = "Rb_MS_AddDate";
            this.Rb_MS_AddDate.Size = new System.Drawing.Size(123, 20);
            this.Rb_MS_AddDate.TabIndex = 9;
            this.Rb_MS_AddDate.Text = "Service Add Date";
            // 
            // lblDateSelection
            // 
            this.lblDateSelection.Controls.Add(this.label3);
            this.lblDateSelection.Location = new System.Drawing.Point(15, 8);
            this.lblDateSelection.Name = "lblDateSelection";
            this.lblDateSelection.Size = new System.Drawing.Size(82, 14);
            this.lblDateSelection.TabIndex = 2;
            this.lblDateSelection.Text = "Date Selection";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-296, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "label3";
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
            this.pnlHieFilter.Size = new System.Drawing.Size(799, 43);
            this.pnlHieFilter.TabIndex = 2;
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.Pb_Search_Hie);
            this.pnlFilter.Controls.Add(this.spacer4);
            this.pnlFilter.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlFilter.Location = new System.Drawing.Point(740, 9);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(50, 25);
            this.pnlFilter.TabIndex = 6;
            // 
            // Pb_Search_Hie
            // 
            this.Pb_Search_Hie.BackColor = System.Drawing.Color.FromArgb(244, 244, 244);
            this.Pb_Search_Hie.CssStyle = "border-radius:25px";
            this.Pb_Search_Hie.Cursor = Wisej.Web.Cursors.Hand;
            this.Pb_Search_Hie.Dock = Wisej.Web.DockStyle.Left;
            this.Pb_Search_Hie.ImageSource = "captain-filter";
            this.Pb_Search_Hie.Location = new System.Drawing.Point(14, 0);
            this.Pb_Search_Hie.Name = "Pb_Search_Hie";
            this.Pb_Search_Hie.Padding = new Wisej.Web.Padding(4, 5, 4, 4);
            this.Pb_Search_Hie.Size = new System.Drawing.Size(25, 25);
            this.Pb_Search_Hie.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.Pb_Search_Hie.ToolTipText = "Select Hierarchy";
            this.Pb_Search_Hie.Click += new System.EventHandler(this.Pb_Search_Hie_Click);
            // 
            // spacer4
            // 
            this.spacer4.Dock = Wisej.Web.DockStyle.Left;
            this.spacer4.Location = new System.Drawing.Point(0, 0);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(14, 25);
            // 
            // pnlHie
            // 
            this.pnlHie.BackColor = System.Drawing.Color.FromArgb(11, 70, 117);
            this.pnlHie.Controls.Add(this.CmbYear);
            this.pnlHie.Controls.Add(this.spacer3);
            this.pnlHie.Controls.Add(this.Txt_HieDesc);
            this.pnlHie.Dock = Wisej.Web.DockStyle.Left;
            this.pnlHie.Location = new System.Drawing.Point(15, 9);
            this.pnlHie.Name = "pnlHie";
            this.pnlHie.Size = new System.Drawing.Size(725, 25);
            this.pnlHie.TabIndex = 3;
            // 
            // CmbYear
            // 
            this.CmbYear.Dock = Wisej.Web.DockStyle.Left;
            this.CmbYear.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbYear.FormattingEnabled = true;
            this.CmbYear.Location = new System.Drawing.Point(658, 0);
            this.CmbYear.Name = "CmbYear";
            this.CmbYear.Size = new System.Drawing.Size(65, 25);
            this.CmbYear.TabIndex = 5;
            this.CmbYear.Visible = false;
            this.CmbYear.SelectedIndexChanged += new System.EventHandler(this.CmbYear_SelectedIndexChanged);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Left;
            this.spacer3.Location = new System.Drawing.Point(644, 0);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(14, 25);
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
            this.Txt_HieDesc.Size = new System.Drawing.Size(644, 25);
            this.Txt_HieDesc.TabIndex = 4;
            this.Txt_HieDesc.TextAlign = Wisej.Web.HorizontalAlignment.Center;
            // 
            // pnlGenerate
            // 
            this.pnlGenerate.AppearanceKey = "panel-grdo";
            this.pnlGenerate.Controls.Add(this.btnPrint);
            this.pnlGenerate.Controls.Add(this.spacer2);
            this.pnlGenerate.Controls.Add(this.btnGeneratePdf);
            this.pnlGenerate.Controls.Add(this.spacer1);
            this.pnlGenerate.Controls.Add(this.chkbExcel);
            this.pnlGenerate.Controls.Add(this.btnPdfPreview);
            this.pnlGenerate.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlGenerate.Location = new System.Drawing.Point(0, 384);
            this.pnlGenerate.Name = "pnlGenerate";
            this.pnlGenerate.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlGenerate.Size = new System.Drawing.Size(799, 35);
            this.pnlGenerate.TabIndex = 25;
            // 
            // btnPrint
            // 
            this.btnPrint.AppearanceKey = "button-reports";
            this.btnPrint.Dock = Wisej.Web.DockStyle.Right;
            this.btnPrint.Location = new System.Drawing.Point(538, 5);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(80, 25);
            this.btnPrint.TabIndex = 27;
            this.btnPrint.Text = "&Print";
            this.btnPrint.Visible = false;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Right;
            this.spacer2.Location = new System.Drawing.Point(618, 5);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(3, 25);
            // 
            // btnGeneratePdf
            // 
            this.btnGeneratePdf.AppearanceKey = "button-reports";
            this.btnGeneratePdf.Dock = Wisej.Web.DockStyle.Right;
            this.btnGeneratePdf.Location = new System.Drawing.Point(621, 5);
            this.btnGeneratePdf.Name = "btnGeneratePdf";
            this.btnGeneratePdf.Size = new System.Drawing.Size(80, 25);
            this.btnGeneratePdf.TabIndex = 28;
            this.btnGeneratePdf.Text = "&Generate";
            this.btnGeneratePdf.Click += new System.EventHandler(this.btnGeneratePdf_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(701, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // chkbExcel
            // 
            this.chkbExcel.AutoSize = false;
            this.chkbExcel.Dock = Wisej.Web.DockStyle.Left;
            this.chkbExcel.Location = new System.Drawing.Point(15, 5);
            this.chkbExcel.Name = "chkbExcel";
            this.chkbExcel.Size = new System.Drawing.Size(112, 25);
            this.chkbExcel.TabIndex = 26;
            this.chkbExcel.Text = "Generate Excel";
            // 
            // btnPdfPreview
            // 
            this.btnPdfPreview.AppearanceKey = "button-reports";
            this.btnPdfPreview.Dock = Wisej.Web.DockStyle.Right;
            this.btnPdfPreview.Location = new System.Drawing.Point(704, 5);
            this.btnPdfPreview.Name = "btnPdfPreview";
            this.btnPdfPreview.Size = new System.Drawing.Size(80, 25);
            this.btnPdfPreview.TabIndex = 29;
            this.btnPdfPreview.Text = "Pre&view";
            this.btnPdfPreview.Click += new System.EventHandler(this.btnPdfPreview_Click);
            // 
            // CASB0013Form
            // 
            this.ClientSize = new System.Drawing.Size(799, 419);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CASB0013Form";
            this.Text = "CASB0013Form";
            componentTool1.ImageSource = "icon-help";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.CASB0013Form_ToolClick);
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlgvwProgramData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwProgramData)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.lblRepType.ResumeLayout(false);
            this.lblRepType.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.Date_Panel.ResumeLayout(false);
            this.lblDateSelection.ResumeLayout(false);
            this.lblDateSelection.PerformLayout();
            this.pnlHieFilter.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).EndInit();
            this.pnlHie.ResumeLayout(false);
            this.pnlHie.PerformLayout();
            this.pnlGenerate.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private Panel pnlCompleteForm;
        private PictureBox Pb_Search_Hie;
        private Panel pnlHie;
        private ComboBox CmbYear;
        private TextBox Txt_HieDesc;
        private Panel pnlGenerate;
        private CheckBox chkbExcel;
        private Button btnGeneratePdf;
        private Button btnPdfPreview;
        private Button btnPrint;
        private Label lblDateSelection;
        private Label label3;
        private Panel Date_Panel;
        private RadioButton Rb_MS_Date;
        private RadioButton Rb_MS_AddDate;
        private Label lblReferenceperiodate;
        private Label lblReportPeriodDate;
        private Label lblreferenceFrom;
        private DateTimePicker Ref_To_Date;
        private Label lblReferenceTo;
        private DateTimePicker Ref_From_Date;
        private DateTimePicker Rep_From_Date;
        private Label lblReportTo;
        private DateTimePicker Rep_To_Date;
        private Label lblReportfromDate;
        private Button btnProcess;
        private Panel panel3;
        private RadioButton rdoProgcount;
        private RadioButton rdoServiceCount;
        private Label lblRepType;
        private Label label1;
        private CheckBox chkbUndupTable;
        private DataGridView gvwProgramData;
        private DataGridViewTextBoxColumn gvtProgramDesc;
        private DataGridViewTextBoxColumn gvtServicecode;
        private DataGridViewTextBoxColumn gvtReportcount;
        private DataGridViewTextBoxColumn gvtRefcount;
        private CheckBox chkPrintAuditreport;
        private Spacer spacer1;
        private Spacer spacer2;
        private Panel pnlHieFilter;
        private Panel pnlFilter;
        private Panel pnlgvwProgramData;
        private Panel panel4;
        private Panel panel2;
        private Panel panel1;
        private Spacer spacer3;
        private Spacer spacer4;
    }
}