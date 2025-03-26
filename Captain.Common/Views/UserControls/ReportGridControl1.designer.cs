using Wisej.Web;

namespace Captain.Common.Views.UserControls
{
    partial class ReportGridControl1
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

        #region Visual WebGui UserControl Designer generated code

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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle10 = new Wisej.Web.DataGridViewCellStyle();
            this.pnlDates = new Wisej.Web.Panel();
            this.btnSummary = new Wisej.Web.Button();
            this.btnUserId = new Wisej.Web.Button();
            this.dtEndDate = new Wisej.Web.DateTimePicker();
            this.lblTodate = new Wisej.Web.Label();
            this.lblStart = new Wisej.Web.Label();
            this.btnGetReport = new Wisej.Web.Button();
            this.dtStartDate = new Wisej.Web.DateTimePicker();
            this.pnlData = new Wisej.Web.Panel();
            this.pnlRightTabs = new Wisej.Web.Panel();
            this.tabControl1 = new Wisej.Web.TabControl();
            this.tbpUserProgView = new Wisej.Web.TabPage();
            this.pdfUserorProgram = new Wisej.Web.PdfViewer();
            this.tbpSumChart = new Wisej.Web.TabPage();
            this.pnlLeftGrids = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlgvwUserData = new Wisej.Web.Panel();
            this.gvwUserData = new Wisej.Web.DataGridView();
            this.gvIData = new Wisej.Web.DataGridViewImageColumn();
            this.gvtprogramCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtProgram1 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtSelectuser = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlgvwData = new Wisej.Web.Panel();
            this.gvwData = new Wisej.Web.DataGridView();
            this.gviImg = new Wisej.Web.DataGridViewImageColumn();
            this.gvtTableName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvcDatetypes = new Wisej.Web.DataGridViewComboBoxColumn();
            this.gvtSelect = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel6 = new Wisej.Web.Panel();
            this.pnlDatesback = new Wisej.Web.Panel();
            this.pnlControl = new Wisej.Web.Panel();
            this.pnlDates.SuspendLayout();
            this.pnlData.SuspendLayout();
            this.pnlRightTabs.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tbpUserProgView.SuspendLayout();
            this.pnlLeftGrids.SuspendLayout();
            this.pnlgvwUserData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwUserData)).BeginInit();
            this.pnlgvwData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwData)).BeginInit();
            this.pnlDatesback.SuspendLayout();
            this.pnlControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDates
            // 
            this.pnlDates.Controls.Add(this.btnSummary);
            this.pnlDates.Controls.Add(this.btnUserId);
            this.pnlDates.Controls.Add(this.dtEndDate);
            this.pnlDates.Controls.Add(this.lblTodate);
            this.pnlDates.Controls.Add(this.lblStart);
            this.pnlDates.Controls.Add(this.btnGetReport);
            this.pnlDates.Controls.Add(this.dtStartDate);
            this.pnlDates.CssStyle = "border-radius:8px; border:1px solid #ececec; ";
            this.pnlDates.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlDates.Location = new System.Drawing.Point(5, 5);
            this.pnlDates.Name = "pnlDates";
            this.pnlDates.Size = new System.Drawing.Size(1308, 37);
            this.pnlDates.TabIndex = 1;
            // 
            // btnSummary
            // 
            this.btnSummary.Location = new System.Drawing.Point(626, 6);
            this.btnSummary.MinimumSize = new System.Drawing.Size(80, 25);
            this.btnSummary.Name = "btnSummary";
            this.btnSummary.Size = new System.Drawing.Size(110, 25);
            this.btnSummary.TabIndex = 5;
            this.btnSummary.Text = "Get Summary";
            this.btnSummary.Visible = false;
            this.btnSummary.Click += new System.EventHandler(this.btnSummary_Click);
            // 
            // btnUserId
            // 
            this.btnUserId.Location = new System.Drawing.Point(450, 6);
            this.btnUserId.MinimumSize = new System.Drawing.Size(125, 25);
            this.btnUserId.Name = "btnUserId";
            this.btnUserId.Size = new System.Drawing.Size(125, 25);
            this.btnUserId.TabIndex = 4;
            this.btnUserId.Text = "&Select Parameters";
            this.btnUserId.Visible = false;
            this.btnUserId.Click += new System.EventHandler(this.btnUserId_Click);
            // 
            // dtEndDate
            // 
            this.dtEndDate.CustomFormat = "MM/dd/yyyy";
            this.dtEndDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtEndDate.Location = new System.Drawing.Point(218, 6);
            this.dtEndDate.MinimumSize = new System.Drawing.Size(116, 25);
            this.dtEndDate.Name = "dtEndDate";
            this.dtEndDate.ShowCheckBox = true;
            this.dtEndDate.ShowToolTips = false;
            this.dtEndDate.Size = new System.Drawing.Size(116, 25);
            this.dtEndDate.TabIndex = 2;
            // 
            // lblTodate
            // 
            this.lblTodate.AutoSize = true;
            this.lblTodate.Location = new System.Drawing.Point(193, 10);
            this.lblTodate.MinimumSize = new System.Drawing.Size(0, 16);
            this.lblTodate.Name = "lblTodate";
            this.lblTodate.Size = new System.Drawing.Size(19, 16);
            this.lblTodate.TabIndex = 0;
            this.lblTodate.Text = "To";
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point(8, 10);
            this.lblStart.MinimumSize = new System.Drawing.Size(0, 16);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(32, 16);
            this.lblStart.TabIndex = 0;
            this.lblStart.Text = "From";
            // 
            // btnGetReport
            // 
            this.btnGetReport.Location = new System.Drawing.Point(358, 6);
            this.btnGetReport.Name = "btnGetReport";
            this.btnGetReport.Size = new System.Drawing.Size(80, 25);
            this.btnGetReport.TabIndex = 3;
            this.btnGetReport.Text = "&Get Report";
            this.btnGetReport.Click += new System.EventHandler(this.btnGetReport_Click);
            // 
            // dtStartDate
            // 
            this.dtStartDate.CustomFormat = "MM/dd/yyyy";
            this.dtStartDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtStartDate.Location = new System.Drawing.Point(47, 5);
            this.dtStartDate.MinimumSize = new System.Drawing.Size(116, 25);
            this.dtStartDate.Name = "dtStartDate";
            this.dtStartDate.ShowCheckBox = true;
            this.dtStartDate.ShowToolTips = false;
            this.dtStartDate.Size = new System.Drawing.Size(116, 25);
            this.dtStartDate.TabIndex = 1;
            // 
            // pnlData
            // 
            this.pnlData.Controls.Add(this.pnlRightTabs);
            this.pnlData.Controls.Add(this.pnlLeftGrids);
            this.pnlData.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlData.Location = new System.Drawing.Point(0, 47);
            this.pnlData.Name = "pnlData";
            this.pnlData.Size = new System.Drawing.Size(1318, 639);
            this.pnlData.TabIndex = 2;
            // 
            // pnlRightTabs
            // 
            this.pnlRightTabs.Controls.Add(this.tabControl1);
            this.pnlRightTabs.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlRightTabs.Location = new System.Drawing.Point(453, 0);
            this.pnlRightTabs.Name = "pnlRightTabs";
            this.pnlRightTabs.Padding = new Wisej.Web.Padding(0, 5, 5, 5);
            this.pnlRightTabs.Size = new System.Drawing.Size(865, 639);
            this.pnlRightTabs.TabIndex = 2;
            this.pnlRightTabs.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.BorderStyle = Wisej.Web.BorderStyle.None;
            this.tabControl1.Controls.Add(this.tbpUserProgView);
            this.tabControl1.Controls.Add(this.tbpSumChart);
            this.tabControl1.CssStyle = "border-radius:8px; border:1px solid #ececec; ";
            this.tabControl1.Dock = Wisej.Web.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.PageInsets = new Wisej.Web.Padding(0, 27, 0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(860, 629);
            this.tabControl1.TabIndex = 0;
            // 
            // tbpUserProgView
            // 
            this.tbpUserProgView.Controls.Add(this.pdfUserorProgram);
            this.tbpUserProgView.Location = new System.Drawing.Point(0, 27);
            this.tbpUserProgView.Name = "tbpUserProgView";
            this.tbpUserProgView.Size = new System.Drawing.Size(860, 602);
            this.tbpUserProgView.Text = "User/Program View";
            // 
            // pdfUserorProgram
            // 
            this.pdfUserorProgram.Dock = Wisej.Web.DockStyle.Fill;
            this.pdfUserorProgram.Location = new System.Drawing.Point(0, 0);
            this.pdfUserorProgram.Name = "pdfUserorProgram";
            this.pdfUserorProgram.Size = new System.Drawing.Size(860, 602);
            this.pdfUserorProgram.TabIndex = 0;
            // 
            // tbpSumChart
            // 
            this.tbpSumChart.Location = new System.Drawing.Point(0, 27);
            this.tbpSumChart.Name = "tbpSumChart";
            this.tbpSumChart.Size = new System.Drawing.Size(860, 602);
            this.tbpSumChart.Text = "Summary Chart";
            // 
            // pnlLeftGrids
            // 
            this.pnlLeftGrids.Controls.Add(this.spacer1);
            this.pnlLeftGrids.Controls.Add(this.pnlgvwUserData);
            this.pnlLeftGrids.Controls.Add(this.pnlgvwData);
            this.pnlLeftGrids.Dock = Wisej.Web.DockStyle.Left;
            this.pnlLeftGrids.Location = new System.Drawing.Point(0, 0);
            this.pnlLeftGrids.Name = "pnlLeftGrids";
            this.pnlLeftGrids.Size = new System.Drawing.Size(453, 639);
            this.pnlLeftGrids.TabIndex = 1;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Top;
            this.spacer1.Location = new System.Drawing.Point(0, 295);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(453, 3);
            // 
            // pnlgvwUserData
            // 
            this.pnlgvwUserData.Controls.Add(this.gvwUserData);
            this.pnlgvwUserData.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwUserData.Location = new System.Drawing.Point(0, 295);
            this.pnlgvwUserData.Name = "pnlgvwUserData";
            this.pnlgvwUserData.Padding = new Wisej.Web.Padding(5, 0, 5, 5);
            this.pnlgvwUserData.Size = new System.Drawing.Size(453, 344);
            this.pnlgvwUserData.TabIndex = 2;
            // 
            // gvwUserData
            // 
            this.gvwUserData.AllowUserToResizeColumns = false;
            this.gvwUserData.AllowUserToResizeRows = false;
            this.gvwUserData.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwUserData.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvwUserData.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvIData,
            this.gvtprogramCode,
            this.gvtProgram1,
            this.gvtSelectuser});
            this.gvwUserData.CssStyle = "border-radius:8px; border:1px solid #ececec; ";
            this.gvwUserData.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwUserData.Location = new System.Drawing.Point(5, 0);
            this.gvwUserData.Name = "gvwUserData";
            this.gvwUserData.RowHeadersVisible = false;
            this.gvwUserData.RowHeadersWidth = 10;
            this.gvwUserData.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwUserData.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwUserData.Size = new System.Drawing.Size(443, 339);
            this.gvwUserData.TabIndex = 0;
            this.gvwUserData.TabStop = false;
            this.gvwUserData.Visible = false;
            this.gvwUserData.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwUserData_CellClick);
            // 
            // gvIData
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = null;
            this.gvIData.DefaultCellStyle = dataGridViewCellStyle1;
            this.gvIData.HeaderText = "  ";
            this.gvIData.Name = "gvIData";
            this.gvIData.ShowInVisibilityMenu = false;
            this.gvIData.Width = 40;
            // 
            // gvtprogramCode
            // 
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtprogramCode.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtprogramCode.HeaderStyle = dataGridViewCellStyle3;
            this.gvtprogramCode.HeaderText = "Code";
            this.gvtprogramCode.Name = "gvtprogramCode";
            this.gvtprogramCode.Width = 80;
            // 
            // gvtProgram1
            // 
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtProgram1.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtProgram1.HeaderStyle = dataGridViewCellStyle5;
            this.gvtProgram1.HeaderText = "Program";
            this.gvtProgram1.Name = "gvtProgram1";
            this.gvtProgram1.Width = 200;
            // 
            // gvtSelectuser
            // 
            this.gvtSelectuser.HeaderText = "  ";
            this.gvtSelectuser.Name = "gvtSelectuser";
            this.gvtSelectuser.ShowInVisibilityMenu = false;
            this.gvtSelectuser.Visible = false;
            // 
            // pnlgvwData
            // 
            this.pnlgvwData.Controls.Add(this.gvwData);
            this.pnlgvwData.Dock = Wisej.Web.DockStyle.Top;
            this.pnlgvwData.Location = new System.Drawing.Point(0, 0);
            this.pnlgvwData.Name = "pnlgvwData";
            this.pnlgvwData.Padding = new Wisej.Web.Padding(5, 5, 5, 0);
            this.pnlgvwData.Size = new System.Drawing.Size(453, 295);
            this.pnlgvwData.TabIndex = 1;
            // 
            // gvwData
            // 
            this.gvwData.AllowUserToResizeColumns = false;
            this.gvwData.AllowUserToResizeRows = false;
            this.gvwData.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwData.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvwData.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gviImg,
            this.gvtTableName,
            this.gvcDatetypes,
            this.gvtSelect});
            this.gvwData.CssStyle = "border-radius:8px; border:1px solid #ececec; ";
            this.gvwData.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwData.Location = new System.Drawing.Point(5, 5);
            this.gvwData.Name = "gvwData";
            this.gvwData.RowHeadersVisible = false;
            this.gvwData.RowHeadersWidth = 10;
            this.gvwData.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwData.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwData.Size = new System.Drawing.Size(443, 290);
            this.gvwData.TabIndex = 0;
            this.gvwData.TabStop = false;
            this.gvwData.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwData_CellClick);
            // 
            // gviImg
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.NullValue = null;
            this.gviImg.DefaultCellStyle = dataGridViewCellStyle6;
            this.gviImg.HeaderText = " ";
            this.gviImg.Name = "gviImg";
            this.gviImg.ShowInVisibilityMenu = false;
            this.gviImg.Width = 40;
            // 
            // gvtTableName
            // 
            dataGridViewCellStyle7.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtTableName.DefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtTableName.HeaderStyle = dataGridViewCellStyle8;
            this.gvtTableName.HeaderText = "Description";
            this.gvtTableName.Name = "gvtTableName";
            this.gvtTableName.ReadOnly = true;
            this.gvtTableName.Width = 270;
            // 
            // gvcDatetypes
            // 
            dataGridViewCellStyle9.BackgroundImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            dataGridViewCellStyle9.BackgroundImageSource = "combo-arrow";
            this.gvcDatetypes.DefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvcDatetypes.HeaderStyle = dataGridViewCellStyle10;
            this.gvcDatetypes.HeaderText = "Date";
            this.gvcDatetypes.Name = "gvcDatetypes";
            this.gvcDatetypes.ValueType = typeof(object);
            this.gvcDatetypes.Width = 110;
            // 
            // gvtSelect
            // 
            this.gvtSelect.HeaderText = "gvtSelect";
            this.gvtSelect.Name = "gvtSelect";
            this.gvtSelect.ShowInVisibilityMenu = false;
            this.gvtSelect.Visible = false;
            this.gvtSelect.Width = 10;
            // 
            // panel6
            // 
            this.panel6.Location = new System.Drawing.Point(492, 6);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(144, 26);
            this.panel6.TabIndex = 12;
            // 
            // pnlDatesback
            // 
            this.pnlDatesback.Controls.Add(this.pnlDates);
            this.pnlDatesback.Dock = Wisej.Web.DockStyle.Top;
            this.pnlDatesback.Location = new System.Drawing.Point(0, 0);
            this.pnlDatesback.Name = "pnlDatesback";
            this.pnlDatesback.Padding = new Wisej.Web.Padding(5);
            this.pnlDatesback.Size = new System.Drawing.Size(1318, 47);
            this.pnlDatesback.TabIndex = 1;
            // 
            // pnlControl
            // 
            this.pnlControl.Controls.Add(this.pnlData);
            this.pnlControl.Controls.Add(this.pnlDatesback);
            this.pnlControl.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlControl.Location = new System.Drawing.Point(0, 0);
            this.pnlControl.Name = "pnlControl";
            this.pnlControl.Size = new System.Drawing.Size(1318, 686);
            this.pnlControl.TabIndex = 0;
            // 
            // ReportGridControl1
            // 
            this.Controls.Add(this.pnlControl);
            this.Name = "ReportGridControl1";
            this.Size = new System.Drawing.Size(1318, 686);
            this.pnlDates.ResumeLayout(false);
            this.pnlDates.PerformLayout();
            this.pnlData.ResumeLayout(false);
            this.pnlRightTabs.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tbpUserProgView.ResumeLayout(false);
            this.pnlLeftGrids.ResumeLayout(false);
            this.pnlgvwUserData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwUserData)).EndInit();
            this.pnlgvwData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwData)).EndInit();
            this.pnlDatesback.ResumeLayout(false);
            this.pnlControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Panel pnlData;
        private Panel pnlRightTabs;
        private Panel pnlLeftGrids;
        private DateTimePicker dtStartDate;
        private Label lblStart;
        private DataGridView gvwData;
        private DataGridViewImageColumn gviImg;
        private DataGridViewTextBoxColumn gvtTableName;
        private DataGridViewTextBoxColumn gvtSelect;
        private Button btnGetReport;
        private Panel pnlDates;
        private DateTimePicker dtEndDate;
        private Label lblTodate;
        private DataGridView gvwUserData;
        private DataGridViewTextBoxColumn gvtprogramCode;
        private DataGridViewTextBoxColumn gvtProgram1;
        private TabControl tabControl1;
        private TabPage tbpUserProgView;
        //private Gizmox.WebGUI.Reporting.ReportViewer reportViewer2;
        private DataGridViewImageColumn gvIData;
        private Button btnUserId;
        private DataGridViewTextBoxColumn gvtSelectuser;
        private TabPage tbpSumChart;
        //private Gizmox.WebGUI.Reporting.ReportViewer reportViewer1;
        private Button btnSummary;
        private Panel panel6;
        private Panel pnlDatesback;
        private Panel pnlControl;
        private Panel pnlgvwData;
        private Panel pnlgvwUserData;
        private Spacer spacer1;
        private DataGridViewComboBoxColumn gvcDatetypes;
        private PdfViewer pdfUserorProgram;
    }
}