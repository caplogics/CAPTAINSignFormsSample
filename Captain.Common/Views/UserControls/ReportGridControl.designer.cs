using Wisej.Web;

namespace Captain.Common.Views.UserControls
{
    partial class ReportGridControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportGridControl));
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            this.panel1 = new Wisej.Web.Panel();
            this.panel5 = new Wisej.Web.Panel();
            this.label6 = new Wisej.Web.Label();
            this.cmbRepUserId = new Wisej.Web.ComboBox();
            this.dtEndDate = new Wisej.Web.DateTimePicker();
            this.lblTodate = new Wisej.Web.Label();
            this.lblStart = new Wisej.Web.Label();
            this.cmbChartType = new Wisej.Web.ComboBox();
            this.lblChartType = new Wisej.Web.Label();
            this.btnGetReport = new Wisej.Web.Button();
            this.dtStartDate = new Wisej.Web.DateTimePicker();
            this.panel2 = new Wisej.Web.Panel();
            this.panel4 = new Wisej.Web.Panel();
            this.tabControl1 = new Wisej.Web.TabControl();
            this.tabPage1 = new Wisej.Web.TabPage();
            //this.reportViewer1 = new Gizmox.WebGUI.Reporting.ReportViewer();
            this.tabPage2 = new Wisej.Web.TabPage();
            //this.reportViewer2 = new Gizmox.WebGUI.Reporting.ReportViewer();
            this.panel3 = new Wisej.Web.Panel();
            this.gvwUserData = new Wisej.Web.DataGridView();
            this.gvtUserId = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtProgram1 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvwData = new Wisej.Web.DataGridView();
            this.gviImg = new Wisej.Web.DataGridViewImageColumn();
            this.gvtTableName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvcDatetypes = new Wisej.Web.DataGridViewComboBoxColumn();
            this.gvtSelect = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwUserData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwData)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(824, 42);
            this.panel1.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.cmbRepUserId);
            this.panel5.Controls.Add(this.dtEndDate);
            this.panel5.Controls.Add(this.lblTodate);
            this.panel5.Controls.Add(this.lblStart);
            this.panel5.Controls.Add(this.cmbChartType);
            this.panel5.Controls.Add(this.lblChartType);
            this.panel5.Controls.Add(this.btnGetReport);
            this.panel5.Controls.Add(this.dtStartDate);
            this.panel5.Dock = Wisej.Web.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(824, 42);
            this.panel5.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(282, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "UserID";
            // 
            // cmbRepUserId
            // 
            this.cmbRepUserId.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.cmbRepUserId.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbRepUserId.FormattingEnabled = true;
            this.cmbRepUserId.Location = new System.Drawing.Point(329, 9);
            this.cmbRepUserId.Name = "cmbRepUserId";
            this.cmbRepUserId.Size = new System.Drawing.Size(115, 21);
            this.cmbRepUserId.TabIndex = 1;
            // 
            // dtEndDate
            // 
            this.dtEndDate.Checked = false;
            this.dtEndDate.CustomFormat = "MM/dd/yyyy";
            this.dtEndDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            //this.dtEndDate.IsEmpty = false;
            //this.dtEndDate.IsEmptyAllowed = false;
            this.dtEndDate.Location = new System.Drawing.Point(170, 9);
            this.dtEndDate.Name = "dtEndDate";
            this.dtEndDate.ShowCheckBox = true;
            this.dtEndDate.Size = new System.Drawing.Size(106, 21);
            this.dtEndDate.TabIndex = 9;
            // 
            // lblTodate
            // 
            this.lblTodate.AutoSize = true;
            this.lblTodate.Location = new System.Drawing.Point(152, 12);
            this.lblTodate.Name = "lblTodate";
            this.lblTodate.Size = new System.Drawing.Size(30, 13);
            this.lblTodate.TabIndex = 0;
            this.lblTodate.Text = "To";
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point(7, 12);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(30, 13);
            this.lblStart.TabIndex = 0;
            this.lblStart.Text = "From";
            // 
            // cmbChartType
            // 
            this.cmbChartType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbChartType.FormattingEnabled = true;
            this.cmbChartType.Location = new System.Drawing.Point(662, 9);
            this.cmbChartType.Name = "cmbChartType";
            this.cmbChartType.Size = new System.Drawing.Size(76, 21);
            this.cmbChartType.TabIndex = 0;
            // 
            // lblChartType
            // 
            this.lblChartType.AutoSize = true;
            this.lblChartType.Location = new System.Drawing.Point(597, 13);
            this.lblChartType.Name = "lblChartType";
            this.lblChartType.Size = new System.Drawing.Size(61, 13);
            this.lblChartType.TabIndex = 1;
            this.lblChartType.Text = "Chart Type";
            // 
            // btnGetReport
            // 
            this.btnGetReport.Location = new System.Drawing.Point(741, 7);
            this.btnGetReport.Name = "btnGetReport";
            this.btnGetReport.Size = new System.Drawing.Size(75, 23);
            this.btnGetReport.TabIndex = 10;
            this.btnGetReport.Text = "Get Report";
            this.btnGetReport.Click += new System.EventHandler(this.btnGetReport_Click);
            // 
            // dtStartDate
            // 
            this.dtStartDate.Checked = false;
            this.dtStartDate.CustomFormat = "MM/dd/yyyy";
            this.dtStartDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            //this.dtStartDate.IsEmpty = false;
            //this.dtStartDate.IsEmptyAllowed = false;
            this.dtStartDate.Location = new System.Drawing.Point(38, 9);
            this.dtStartDate.Name = "dtStartDate";
            this.dtStartDate.ShowCheckBox = true;
            this.dtStartDate.Size = new System.Drawing.Size(106, 21);
            this.dtStartDate.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 42);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(824, 482);
            this.panel2.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tabControl1);
            this.panel4.Dock = Wisej.Web.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(291, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(533, 482);
            this.panel4.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = Wisej.Web.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(533, 482);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            //this.tabPage1.Controls.Add(this.reportViewer1);
            this.tabPage1.Dock = Wisej.Web.DockStyle.Fill;
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(525, 456);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Summary View";
            // 
            // reportViewer1
            // 
            //this.reportViewer1.AutoScroll = false;
            //this.reportViewer1.ControlCode = resources.GetString("reportViewer1.ControlCode");
            //this.reportViewer1.Dock = Wisej.Web.DockStyle.Fill;
            //this.reportViewer1.Location = new System.Drawing.Point(3, 3);
            //this.reportViewer1.Name = "reportViewer1";
            //this.reportViewer1.Size = new System.Drawing.Size(525, 456);
            //this.reportViewer1.TabIndex = 0;
            //this.reportViewer1.HostedPageLoad += new Wisej.Web.Hosts.AspPageEventHandler(this.reportViewer1_HostedPageLoad);
            // 
            // tabPage2
            // 
            //this.tabPage2.Controls.Add(this.reportViewer2);
            this.tabPage2.Dock = Wisej.Web.DockStyle.Fill;
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(525, 456);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "User/Program View";
            // 
            // reportViewer2
            // 
            //this.reportViewer2.AsyncRendering = false;
            //this.reportViewer2.AutoScroll = false;
            //this.reportViewer2.ControlCode = resources.GetString("reportViewer2.ControlCode");
            //this.reportViewer2.Dock = Wisej.Web.DockStyle.Fill;
            //this.reportViewer2.Location = new System.Drawing.Point(3, 3);
            //this.reportViewer2.Name = "reportViewer2";
            //this.reportViewer2.Size = new System.Drawing.Size(525, 456);
            //this.reportViewer2.TabIndex = 0;
            //this.reportViewer2.HostedPageLoad += new Wisej.Web.Hosts.AspPageEventHandler(this.reportViewer2_HostedPageLoad);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.panel3.Controls.Add(this.gvwUserData);
            this.panel3.Controls.Add(this.gvwData);
            this.panel3.Dock = Wisej.Web.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(291, 482);
            this.panel3.TabIndex = 0;
            // 
            // gvwUserData
            // 
            this.gvwUserData.AllowDrag = false;
            this.gvwUserData.AllowUserToAddRows = false;
            this.gvwUserData.AllowUserToDeleteRows = false;
            this.gvwUserData.AllowUserToOrderColumns = true;
            this.gvwUserData.AllowUserToResizeColumns = false;
            this.gvwUserData.AllowUserToResizeRows = false;
            this.gvwUserData.BackColor = System.Drawing.Color.White;
            this.gvwUserData.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtUserId,
            this.gvtProgram1});
            //this.gvwUserData.ItemsPerPage = 200;
            this.gvwUserData.Location = new System.Drawing.Point(-2, 320);
            this.gvwUserData.Name = "gvwUserData";
            this.gvwUserData.RowHeadersVisible = false;
            this.gvwUserData.RowHeadersWidth = 4;
            this.gvwUserData.SelectionMode = Wisej.Web.DataGridViewSelectionMode.FullRowSelect;
            this.gvwUserData.Size = new System.Drawing.Size(250, 133);
            this.gvwUserData.TabIndex = 0;
            this.gvwUserData.Visible = false;
            // 
            // gvtUserId
            // 
            this.gvtUserId.DefaultCellStyle = dataGridViewCellStyle1;
            this.gvtUserId.HeaderText = "User Name";
            this.gvtUserId.Name = "gvtUserId";
            this.gvtUserId.Width = 120;
            // 
            // gvtProgram1
            // 
            this.gvtProgram1.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvtProgram1.HeaderText = "Program";
            this.gvtProgram1.Name = "gvtProgram1";
            // 
            // gvwData
            // 
            this.gvwData.AllowDrag = false;
            this.gvwData.AllowUserToAddRows = false;
            this.gvwData.AllowUserToDeleteRows = false;
            this.gvwData.AllowUserToOrderColumns = true;
            this.gvwData.AllowUserToResizeColumns = false;
            this.gvwData.AllowUserToResizeRows = false;
            this.gvwData.BackColor = System.Drawing.Color.White;
            this.gvwData.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gviImg,
            this.gvtTableName,
            this.gvcDatetypes,
            this.gvtSelect});
            this.gvwData.Location = new System.Drawing.Point(-1, -1);
            this.gvwData.Name = "gvwData";
            this.gvwData.RowHeadersVisible = false;
            this.gvwData.RowHeadersWidth = 4;
            this.gvwData.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwData.SelectionMode = Wisej.Web.DataGridViewSelectionMode.FullRowSelect;
            this.gvwData.Size = new System.Drawing.Size(284, 284);
            this.gvwData.TabIndex = 0;
            this.gvwData.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwData_CellClick);
            // 
            // gviImg
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = null;
            this.gviImg.DefaultCellStyle = dataGridViewCellStyle3;
            this.gviImg.HeaderText = " ";
            this.gviImg.Name = "gviImg";
            this.gviImg.Width = 35;
            // 
            // gvtTableName
            // 
            this.gvtTableName.DefaultCellStyle = dataGridViewCellStyle4;
            this.gvtTableName.HeaderText = "Description";
            this.gvtTableName.Name = "gvtTableName";
            this.gvtTableName.ReadOnly = true;
            this.gvtTableName.Width = 160;
            // 
            // gvcDatetypes
            // 
            this.gvcDatetypes.DefaultCellStyle = dataGridViewCellStyle5;
            this.gvcDatetypes.HeaderText = "Date";
            this.gvcDatetypes.Name = "gvcDatetypes";
            this.gvcDatetypes.Width = 80;
            // 
            // gvtSelect
            // 
            this.gvtSelect.DefaultCellStyle = dataGridViewCellStyle6;
            this.gvtSelect.Name = "gvtSelect";
            this.gvtSelect.Visible = false;
            this.gvtSelect.Width = 10;
            // 
            // ReportGridControl
            // 
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Size = new System.Drawing.Size(824, 524);
            this.Text = "ReportGridControl";
            this.panel1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwUserData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Panel panel4;
        private Panel panel3;
        private DateTimePicker dtStartDate;
        private Label lblStart;
        private DataGridView gvwData;
        private DataGridViewImageColumn gviImg;
        private DataGridViewTextBoxColumn gvtTableName;
        private DataGridViewComboBoxColumn gvcDatetypes;
        private DataGridViewTextBoxColumn gvtSelect;
        private Button btnGetReport;
        private ComboBox cmbChartType;
        private Label lblChartType;
        private Panel panel5;
        private DateTimePicker dtEndDate;
        private Label lblTodate;
        private Label label6;
        private ComboBox cmbRepUserId;
        private DataGridView gvwUserData;
        private DataGridViewTextBoxColumn gvtUserId;
        private DataGridViewTextBoxColumn gvtProgram1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        //private Gizmox.WebGUI.Reporting.ReportViewer reportViewer1;
        private TabPage tabPage2;
        //private Gizmox.WebGUI.Reporting.ReportViewer reportViewer2;


    }
}