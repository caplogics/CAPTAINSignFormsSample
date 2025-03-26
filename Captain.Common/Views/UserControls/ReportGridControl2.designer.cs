using Wisej.Web;

namespace Captain.Common.Views.UserControls
{
    partial class ReportGridControl2
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            this.btnSummary = new Wisej.Web.Button();
            this.btnUserId = new Wisej.Web.Button();
            this.btnGetReport = new Wisej.Web.Button();
            this.panel2 = new Wisej.Web.Panel();
            this.panel4 = new Wisej.Web.Panel();
            this.dashboard1 = new Wisej.Web.Ext.DevExpress.Dashboard.Dashboard();
            this.flowLayoutPanel1 = new Wisej.Web.FlowLayoutPanel();
            this.dtStartDate = new Wisej.Web.DateTimePicker();
            this.dtEndDate = new Wisej.Web.DateTimePicker();
            this.gvwData = new Wisej.Web.DataGridView();
            this.gviImg = new Wisej.Web.DataGridViewImageColumn();
            this.gvtTableName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvcDatetypes = new Wisej.Web.DataGridViewComboBoxColumn();
            this.gvtSelect = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvwUserData = new Wisej.Web.DataGridView();
            this.gvIData = new Wisej.Web.DataGridViewImageColumn();
            this.gvtprogramCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtProgram1 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtSelectuser = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel6 = new Wisej.Web.Panel();
            this.checkBox1 = new Wisej.Web.CheckBox();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwUserData)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSummary
            // 
            this.flowLayoutPanel1.SetFillWeight(this.btnSummary, 1);
            this.flowLayoutPanel1.SetFlowBreak(this.btnSummary, true);
            this.btnSummary.Location = new System.Drawing.Point(3, 645);
            this.btnSummary.Name = "btnSummary";
            this.btnSummary.Size = new System.Drawing.Size(294, 33);
            this.btnSummary.TabIndex = 10;
            this.btnSummary.Text = "Get Summary";
            this.btnSummary.Visible = false;
            this.btnSummary.Click += new System.EventHandler(this.btnSummary_Click);
            // 
            // btnUserId
            // 
            this.flowLayoutPanel1.SetFillWeight(this.btnUserId, 1);
            this.flowLayoutPanel1.SetFlowBreak(this.btnUserId, true);
            this.btnUserId.Location = new System.Drawing.Point(3, 606);
            this.btnUserId.Name = "btnUserId";
            this.btnUserId.Size = new System.Drawing.Size(294, 33);
            this.btnUserId.TabIndex = 11;
            this.btnUserId.Text = "Select Parameters";
            this.btnUserId.Visible = false;
            this.btnUserId.Click += new System.EventHandler(this.btnUserId_Click);
            // 
            // btnGetReport
            // 
            this.btnGetReport.BackColor = System.Drawing.Color.FromName("@captainBlue");
            this.flowLayoutPanel1.SetFillWeight(this.btnGetReport, 1);
            this.flowLayoutPanel1.SetFlowBreak(this.btnGetReport, true);
            this.btnGetReport.ForeColor = System.Drawing.Color.FromName("@control");
            this.btnGetReport.Location = new System.Drawing.Point(3, 567);
            this.btnGetReport.Name = "btnGetReport";
            this.btnGetReport.Size = new System.Drawing.Size(294, 33);
            this.btnGetReport.TabIndex = 10;
            this.btnGetReport.Text = "Get Report";
            this.btnGetReport.Click += new System.EventHandler(this.btnGetReport_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.flowLayoutPanel1);
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1340, 814);
            this.panel2.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dashboard1);
            this.panel4.Dock = Wisej.Web.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(328, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1012, 814);
            this.panel4.TabIndex = 1;
            // 
            // dashboard1
            // 
            this.dashboard1.AllowCreateNewDashboard = true;
            this.dashboard1.AllowExportDashboard = true;
            this.dashboard1.AllowExportDashboardItems = true;
            this.dashboard1.AllowInspectRawData = true;
            this.dashboard1.AllowOpenDashboard = true;
            this.dashboard1.Dock = Wisej.Web.DockStyle.Fill;
            this.dashboard1.Name = "dashboard1";
            this.dashboard1.Size = new System.Drawing.Size(1012, 814);
            this.dashboard1.TabIndex = 0;
            this.dashboard1.Text = "";
            this.dashboard1.WorkingMode = DevExpress.DashboardWeb.WorkingMode.ViewerOnly;
            this.dashboard1.DataLoading += new DevExpress.DashboardWeb.DataLoadingWebEventHandler(this.dashboard1_DataLoading);
            this.dashboard1.ConfigureDataReloadingTimeout += new DevExpress.DashboardWeb.ConfigureDataReloadingTimeoutWebEventHandler(this.Dashboard1_ConfigureDataReloadingTimeout);
            this.dashboard1.DashboardSaving += new DevExpress.DashboardWeb.DashboardSavingWebEventHandler(this.dashboard1_DashboardSaving);
            this.dashboard1.Init += new System.EventHandler(this.dashboard1_Init);
            this.dashboard1.Load += new System.EventHandler(this.dashboard1_Load);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.CollapseSide = Wisej.Web.HeaderPosition.Left;
            this.flowLayoutPanel1.Controls.Add(this.dtStartDate);
            this.flowLayoutPanel1.Controls.Add(this.dtEndDate);
            this.flowLayoutPanel1.Controls.Add(this.gvwData);
            this.flowLayoutPanel1.Controls.Add(this.gvwUserData);
            this.flowLayoutPanel1.Controls.Add(this.btnGetReport);
            this.flowLayoutPanel1.Controls.Add(this.btnUserId);
            this.flowLayoutPanel1.Controls.Add(this.btnSummary);
            this.flowLayoutPanel1.Controls.Add(this.checkBox1);
            this.flowLayoutPanel1.Dock = Wisej.Web.DockStyle.Left;
            this.flowLayoutPanel1.Font = new System.Drawing.Font("@defaultBold", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.flowLayoutPanel1.HeaderPosition = Wisej.Web.HeaderPosition.Left;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.ShowHeader = true;
            this.flowLayoutPanel1.Size = new System.Drawing.Size(328, 814);
            this.flowLayoutPanel1.TabIndex = 1;
            this.flowLayoutPanel1.Text = "Configuration";
            // 
            // dtStartDate
            // 
            this.dtStartDate.CustomFormat = "MM/dd/yyyy";
            this.flowLayoutPanel1.SetFillWeight(this.dtStartDate, 1);
            this.flowLayoutPanel1.SetFlowBreak(this.dtStartDate, true);
            this.dtStartDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtStartDate.LabelText = "From";
            this.dtStartDate.Location = new System.Drawing.Point(3, 3);
            this.dtStartDate.Name = "dtStartDate";
            this.dtStartDate.ShowCheckBox = true;
            this.dtStartDate.Size = new System.Drawing.Size(294, 48);
            this.dtStartDate.TabIndex = 9;
            this.dtStartDate.Value = new System.DateTime(2022, 3, 31, 17, 44, 28, 168);
            // 
            // dtEndDate
            // 
            this.dtEndDate.CustomFormat = "MM/dd/yyyy";
            this.flowLayoutPanel1.SetFillWeight(this.dtEndDate, 1);
            this.flowLayoutPanel1.SetFlowBreak(this.dtEndDate, true);
            this.dtEndDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtEndDate.LabelText = "To";
            this.dtEndDate.Location = new System.Drawing.Point(3, 57);
            this.dtEndDate.Name = "dtEndDate";
            this.dtEndDate.ShowCheckBox = true;
            this.dtEndDate.Size = new System.Drawing.Size(294, 48);
            this.dtEndDate.TabIndex = 9;
            this.dtEndDate.Value = new System.DateTime(2022, 3, 31, 17, 44, 28, 159);
            // 
            // gvwData
            // 
            this.gvwData.AllowUserToOrderColumns = true;
            this.gvwData.AllowUserToResizeColumns = false;
            this.gvwData.AllowUserToResizeRows = false;
            this.gvwData.BackColor = System.Drawing.Color.White;
            this.gvwData.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gviImg,
            this.gvtTableName,
            this.gvcDatetypes,
            this.gvtSelect});
            this.flowLayoutPanel1.SetFillWeight(this.gvwData, 1);
            this.flowLayoutPanel1.SetFlowBreak(this.gvwData, true);
            this.gvwData.Location = new System.Drawing.Point(3, 111);
            this.gvwData.Name = "gvwData";
            this.gvwData.RowHeadersVisible = false;
            this.gvwData.RowHeadersWidth = 4;
            this.gvwData.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwData.Size = new System.Drawing.Size(294, 300);
            this.gvwData.TabIndex = 0;
            this.gvwData.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwData_CellClick);
            // 
            // gviImg
            // 
            this.gviImg.CellImageAlignment = Wisej.Web.DataGridViewContentAlignment.NotSet;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = null;
            this.gviImg.DefaultCellStyle = dataGridViewCellStyle3;
            this.gviImg.HeaderText = " ";
            this.gviImg.Name = "gviImg";
            this.gviImg.Width = 35;
            // 
            // gvtTableName
            // 
            this.gvtTableName.HeaderText = "Description";
            this.gvtTableName.Name = "gvtTableName";
            this.gvtTableName.ReadOnly = true;
            this.gvtTableName.Width = 160;
            // 
            // gvcDatetypes
            // 
            this.gvcDatetypes.HeaderText = "Date";
            this.gvcDatetypes.Name = "gvcDatetypes";
            this.gvcDatetypes.Width = 80;
            // 
            // gvtSelect
            // 
            this.gvtSelect.HeaderText = "gvtSelect";
            this.gvtSelect.Name = "gvtSelect";
            this.gvtSelect.Visible = false;
            this.gvtSelect.Width = 10;
            // 
            // gvwUserData
            // 
            this.gvwUserData.AllowUserToOrderColumns = true;
            this.gvwUserData.AllowUserToResizeColumns = false;
            this.gvwUserData.AllowUserToResizeRows = false;
            this.gvwUserData.BackColor = System.Drawing.Color.White;
            this.gvwUserData.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvIData,
            this.gvtprogramCode,
            this.gvtProgram1,
            this.gvtSelectuser});
            this.flowLayoutPanel1.SetFillWeight(this.gvwUserData, 1);
            this.flowLayoutPanel1.SetFlowBreak(this.gvwUserData, true);
            this.gvwUserData.Location = new System.Drawing.Point(3, 417);
            this.gvwUserData.Name = "gvwUserData";
            this.gvwUserData.RowHeadersVisible = false;
            this.gvwUserData.RowHeadersWidth = 4;
            this.gvwUserData.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwUserData.Size = new System.Drawing.Size(294, 144);
            this.gvwUserData.TabIndex = 0;
            this.gvwUserData.Visible = false;
            this.gvwUserData.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwUserData_CellClick);
            // 
            // gvIData
            // 
            this.gvIData.CellImageAlignment = Wisej.Web.DataGridViewContentAlignment.NotSet;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.NullValue = null;
            this.gvIData.DefaultCellStyle = dataGridViewCellStyle4;
            this.gvIData.HeaderText = "  ";
            this.gvIData.Name = "gvIData";
            this.gvIData.Width = 35;
            // 
            // gvtprogramCode
            // 
            this.gvtprogramCode.HeaderText = "Code";
            this.gvtprogramCode.Name = "gvtprogramCode";
            this.gvtprogramCode.Width = 70;
            // 
            // gvtProgram1
            // 
            this.gvtProgram1.HeaderText = "Program";
            this.gvtProgram1.Name = "gvtProgram1";
            this.gvtProgram1.Width = 175;
            // 
            // gvtSelectuser
            // 
            this.gvtSelectuser.HeaderText = "  ";
            this.gvtSelectuser.Name = "gvtSelectuser";
            this.gvtSelectuser.Visible = false;
            // 
            // panel6
            // 
            this.panel6.Location = new System.Drawing.Point(492, 6);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(144, 26);
            this.panel6.TabIndex = 12;
            // 
            // checkBox1
            // 
            this.checkBox1.Location = new System.Drawing.Point(3, 684);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(108, 25);
            this.checkBox1.TabIndex = 12;
            this.checkBox1.Text = "Design Mode";
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // ReportGridControl2
            // 
            this.Controls.Add(this.panel2);
            this.Name = "ReportGridControl2";
            this.Size = new System.Drawing.Size(1340, 814);
            this.Load += new System.EventHandler(this.ReportGridControl2_Load);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwUserData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Panel panel2;
        private Panel panel4;
        private DataGridView gvwData;
        private DataGridViewImageColumn gviImg;
        private DataGridViewTextBoxColumn gvtTableName;
        private DataGridViewComboBoxColumn gvcDatetypes;
        private DataGridViewTextBoxColumn gvtSelect;
        private Button btnGetReport;
        private DataGridView gvwUserData;
        private DataGridViewTextBoxColumn gvtprogramCode;
        private DataGridViewTextBoxColumn gvtProgram1;
        //private Gizmox.WebGUI.Reporting.ReportViewer reportViewer2;
        private DataGridViewImageColumn gvIData;
        private Button btnUserId;
        private DataGridViewTextBoxColumn gvtSelectuser;
        //private Gizmox.WebGUI.Reporting.ReportViewer reportViewer1;
        private Button btnSummary;
        private Panel panel6;
        private Wisej.Web.Ext.DevExpress.Dashboard.Dashboard dashboard1;
        private FlowLayoutPanel flowLayoutPanel1;
        private DateTimePicker dtStartDate;
        private DateTimePicker dtEndDate;
        private CheckBox checkBox1;
    }
}