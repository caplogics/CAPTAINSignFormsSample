using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class CASE0006CLOSESCREEN
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

        #region Wisej Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            this.panel1 = new Wisej.Web.Panel();
            this.lblMSDate = new Wisej.Web.Label();
            this.dtpMSdate = new Wisej.Web.DateTimePicker();
            this.cmbResult = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.lblResult = new Wisej.Web.Label();
            this.btnMSUpdate = new Wisej.Web.Button();
            this.gvwMSDetails = new Wisej.Web.DataGridView();
            this.gvcMsSelect = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvtMSDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtMSResultDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtMSDate = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtMsCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtMSResultCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtMsBranch = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtMsGroup = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtMSSpmSeq = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtMsSpmplan = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtMsApplicant = new Wisej.Web.DataGridViewTextBoxColumn();
            this.chkSelectAll = new Wisej.Web.CheckBox();
            this.gvwService = new Wisej.Web.DataGridView();
            this.gvtCheckbox = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvtAppNo = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtClientName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtSpmName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtIntakedt = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtStartdt = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtCACount = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtMACount = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtSpmId = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtSpmSeq = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtMSSwitch = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel2 = new Wisej.Web.Panel();
            this.lblMSResult = new Wisej.Web.Label();
            this.cmbMsResult = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.panel3 = new Wisej.Web.Panel();
            this.btnMassfill = new Wisej.Web.Button();
            this.lblCompletiondt = new Wisej.Web.Label();
            this.dtcompletion = new Wisej.Web.DateTimePicker();
            this.btnSearch = new Wisej.Web.Button();
            this.rdoServicePlan = new Wisej.Web.RadioButton();
            this.dtpFrmDate = new Wisej.Web.DateTimePicker();
            this.rdoIntake = new Wisej.Web.RadioButton();
            this.dtpToDt = new Wisej.Web.DateTimePicker();
            this.lblToDt = new Wisej.Web.Label();
            this.lblFrmDt = new Wisej.Web.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwMSDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwService)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblMSDate);
            this.panel1.Controls.Add(this.dtpMSdate);
            this.panel1.Controls.Add(this.cmbResult);
            this.panel1.Controls.Add(this.lblResult);
            this.panel1.Controls.Add(this.btnMSUpdate);
            this.panel1.Controls.Add(this.gvwMSDetails);
            this.panel1.Controls.Add(this.chkSelectAll);
            this.panel1.Controls.Add(this.gvwService);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(643, 429);
            this.panel1.TabIndex = 2;
            this.panel1.Click += new System.EventHandler(this.panel1_Click);
            // 
            // lblMSDate
            // 
            this.lblMSDate.AutoSize = true;
            this.lblMSDate.Location = new System.Drawing.Point(316, 463);
            this.lblMSDate.Name = "lblMSDate";
            this.lblMSDate.RightToLeft = Wisej.Web.RightToLeft.No;
            this.lblMSDate.Size = new System.Drawing.Size(30, 14);
            this.lblMSDate.TabIndex = 0;
            this.lblMSDate.Text = "Date";
            this.lblMSDate.Visible = false;
            // 
            // dtpMSdate
            // 
            this.dtpMSdate.Checked = false;
            this.dtpMSdate.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.dtpMSdate.Location = new System.Drawing.Point(354, 459);
            this.dtpMSdate.Name = "dtpMSdate";
            this.dtpMSdate.ShowCheckBox = true;
            this.dtpMSdate.Size = new System.Drawing.Size(102, 22);
            this.dtpMSdate.TabIndex = 2;
            this.dtpMSdate.Visible = false;
            // 
            // cmbResult
            // 
            this.cmbResult.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbResult.FormattingEnabled = true;
            this.cmbResult.Location = new System.Drawing.Point(45, 459);
            this.cmbResult.Name = "cmbResult";
            this.cmbResult.Size = new System.Drawing.Size(240, 25);
            this.cmbResult.TabIndex = 1;
            this.cmbResult.Visible = false;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(8, 463);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(39, 14);
            this.lblResult.TabIndex = 1;
            this.lblResult.Text = "Result";
            this.lblResult.Visible = false;
            // 
            // btnMSUpdate
            // 
            this.btnMSUpdate.Location = new System.Drawing.Point(534, 457);
            this.btnMSUpdate.Name = "btnMSUpdate";
            this.btnMSUpdate.Size = new System.Drawing.Size(101, 23);
            this.btnMSUpdate.TabIndex = 3;
            this.btnMSUpdate.Text = "Update Result";
            this.btnMSUpdate.Visible = false;
            this.btnMSUpdate.Click += new System.EventHandler(this.btnMSUpdate_Click);
            // 
            // gvwMSDetails
            // 
            this.gvwMSDetails.AllowUserToOrderColumns = true;
            this.gvwMSDetails.AllowUserToResizeColumns = false;
            this.gvwMSDetails.AllowUserToResizeRows = false;
            this.gvwMSDetails.BackColor = System.Drawing.Color.White;
            this.gvwMSDetails.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvcMsSelect,
            this.gvtMSDesc,
            this.gvtMSResultDesc,
            this.gvtMSDate,
            this.gvtMsCode,
            this.gvtMSResultCode,
            this.gvtMsBranch,
            this.gvtMsGroup,
            this.gvtMSSpmSeq,
            this.gvtMsSpmplan,
            this.gvtMsApplicant});
            this.gvwMSDetails.Location = new System.Drawing.Point(2, 347);
            this.gvwMSDetails.MultiSelect = false;
            this.gvwMSDetails.Name = "gvwMSDetails";
            this.gvwMSDetails.RowHeadersWidth = 14;
            this.gvwMSDetails.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwMSDetails.Size = new System.Drawing.Size(638, 106);
            this.gvwMSDetails.TabIndex = 4;
            // 
            // gvcMsSelect
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = false;
            this.gvcMsSelect.DefaultCellStyle = dataGridViewCellStyle1;
            this.gvcMsSelect.HeaderText = "  ";
            this.gvcMsSelect.Name = "gvcMsSelect";
            this.gvcMsSelect.Width = 30;
            // 
            // gvtMSDesc
            // 
            this.gvtMSDesc.HeaderText = "MS Description";
            this.gvtMSDesc.Name = "gvtMSDesc";
            this.gvtMSDesc.ReadOnly = true;
            this.gvtMSDesc.Width = 370;
            // 
            // gvtMSResultDesc
            // 
            this.gvtMSResultDesc.HeaderText = "Result";
            this.gvtMSResultDesc.Name = "gvtMSResultDesc";
            this.gvtMSResultDesc.ReadOnly = true;
            this.gvtMSResultDesc.Width = 120;
            // 
            // gvtMSDate
            // 
            this.gvtMSDate.HeaderText = "MS Date";
            this.gvtMSDate.Name = "gvtMSDate";
            this.gvtMSDate.ReadOnly = true;
            // 
            // gvtMsCode
            // 
            this.gvtMsCode.Name = "gvtMsCode";
            this.gvtMsCode.ReadOnly = true;
            this.gvtMsCode.Visible = false;
            this.gvtMsCode.Width = 20;
            // 
            // gvtMSResultCode
            // 
            this.gvtMSResultCode.Name = "gvtMSResultCode";
            this.gvtMSResultCode.ReadOnly = true;
            this.gvtMSResultCode.Visible = false;
            this.gvtMSResultCode.Width = 20;
            // 
            // gvtMsBranch
            // 
            this.gvtMsBranch.Name = "gvtMsBranch";
            this.gvtMsBranch.Visible = false;
            // 
            // gvtMsGroup
            // 
            this.gvtMsGroup.Name = "gvtMsGroup";
            this.gvtMsGroup.Visible = false;
            // 
            // gvtMSSpmSeq
            // 
            this.gvtMSSpmSeq.Name = "gvtMSSpmSeq";
            this.gvtMSSpmSeq.Visible = false;
            // 
            // gvtMsSpmplan
            // 
            this.gvtMsSpmplan.Name = "gvtMsSpmplan";
            this.gvtMsSpmplan.Visible = false;
            // 
            // gvtMsApplicant
            // 
            this.gvtMsApplicant.Name = "gvtMsApplicant";
            this.gvtMsApplicant.Visible = false;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Location = new System.Drawing.Point(26, 90);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(83, 21);
            this.chkSelectAll.TabIndex = 5;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // gvwService
            // 
            this.gvwService.AllowUserToOrderColumns = true;
            this.gvwService.AllowUserToResizeColumns = false;
            this.gvwService.AllowUserToResizeRows = false;
            this.gvwService.BackColor = System.Drawing.Color.White;
            this.gvwService.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtCheckbox,
            this.gvtAppNo,
            this.gvtClientName,
            this.gvtSpmName,
            this.gvtIntakedt,
            this.gvtStartdt,
            this.gvtCACount,
            this.gvtMACount,
            this.gvtSpmId,
            this.gvtSpmSeq,
            this.gvtMSSwitch});
            this.gvwService.Location = new System.Drawing.Point(2, 109);
            this.gvwService.Name = "gvwService";
            this.gvwService.RowHeadersWidth = 14;
            this.gvwService.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwService.Size = new System.Drawing.Size(638, 235);
            this.gvwService.TabIndex = 4;
            this.gvwService.SelectionChanged += new System.EventHandler(this.gvwService_SelectionChanged);
            this.gvwService.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwService_CellClick);
            // 
            // gvtCheckbox
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = false;
            this.gvtCheckbox.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvtCheckbox.HeaderText = "  ";
            this.gvtCheckbox.Name = "gvtCheckbox";
            this.gvtCheckbox.Width = 30;
            // 
            // gvtAppNo
            // 
            this.gvtAppNo.HeaderText = "App #";
            this.gvtAppNo.Name = "gvtAppNo";
            this.gvtAppNo.ReadOnly = true;
            this.gvtAppNo.Width = 60;
            // 
            // gvtClientName
            // 
            this.gvtClientName.HeaderText = "Client Name";
            this.gvtClientName.Name = "gvtClientName";
            this.gvtClientName.ReadOnly = true;
            this.gvtClientName.Width = 120;
            // 
            // gvtSpmName
            // 
            this.gvtSpmName.HeaderText = "Service";
            this.gvtSpmName.Name = "gvtSpmName";
            this.gvtSpmName.ReadOnly = true;
            this.gvtSpmName.Width = 140;
            // 
            // gvtIntakedt
            // 
            this.gvtIntakedt.HeaderText = "Intake Dt";
            this.gvtIntakedt.Name = "gvtIntakedt";
            this.gvtIntakedt.ReadOnly = true;
            this.gvtIntakedt.Width = 65;
            // 
            // gvtStartdt
            // 
            this.gvtStartdt.HeaderText = "Service Dt";
            this.gvtStartdt.Name = "gvtStartdt";
            this.gvtStartdt.ReadOnly = true;
            this.gvtStartdt.Width = 65;
            // 
            // gvtCACount
            // 
            this.gvtCACount.HeaderText = "CA COUNT";
            this.gvtCACount.Name = "gvtCACount";
            this.gvtCACount.ReadOnly = true;
            this.gvtCACount.Width = 65;
            // 
            // gvtMACount
            // 
            this.gvtMACount.HeaderText = "MS COUNT";
            this.gvtMACount.Name = "gvtMACount";
            this.gvtMACount.ReadOnly = true;
            this.gvtMACount.Width = 68;
            // 
            // gvtSpmId
            // 
            this.gvtSpmId.Name = "gvtSpmId";
            this.gvtSpmId.ReadOnly = true;
            this.gvtSpmId.Visible = false;
            // 
            // gvtSpmSeq
            // 
            this.gvtSpmSeq.Name = "gvtSpmSeq";
            this.gvtSpmSeq.ReadOnly = true;
            this.gvtSpmSeq.Visible = false;
            // 
            // gvtMSSwitch
            // 
            this.gvtMSSwitch.HeaderText = "MSSwitch";
            this.gvtMSSwitch.Name = "gvtMSSwitch";
            this.gvtMSSwitch.ReadOnly = true;
            this.gvtMSSwitch.Visible = false;
            this.gvtMSSwitch.Width = 10;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblMSResult);
            this.panel2.Controls.Add(this.cmbMsResult);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.rdoServicePlan);
            this.panel2.Controls.Add(this.dtpFrmDate);
            this.panel2.Controls.Add(this.rdoIntake);
            this.panel2.Controls.Add(this.dtpToDt);
            this.panel2.Controls.Add(this.lblToDt);
            this.panel2.Controls.Add(this.lblFrmDt);
            this.panel2.Location = new System.Drawing.Point(-1, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(645, 89);
            this.panel2.TabIndex = 1;
            // 
            // lblMSResult
            // 
            this.lblMSResult.AutoSize = true;
            this.lblMSResult.Location = new System.Drawing.Point(8, 66);
            this.lblMSResult.Name = "lblMSResult";
            this.lblMSResult.Size = new System.Drawing.Size(39, 14);
            this.lblMSResult.TabIndex = 1;
            this.lblMSResult.Text = "Result";
            this.lblMSResult.Click += new System.EventHandler(this.lblResult_Click);
            // 
            // cmbMsResult
            // 
            this.cmbMsResult.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbMsResult.FormattingEnabled = true;
            this.cmbMsResult.Location = new System.Drawing.Point(45, 62);
            this.cmbMsResult.Name = "cmbMsResult";
            this.cmbMsResult.Size = new System.Drawing.Size(240, 25);
            this.cmbMsResult.TabIndex = 4;
            this.cmbMsResult.SelectedIndexChanged += new System.EventHandler(this.cmbMsResult_SelectedIndexChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnMassfill);
            this.panel3.Controls.Add(this.lblCompletiondt);
            this.panel3.Controls.Add(this.dtcompletion);
            this.panel3.Location = new System.Drawing.Point(399, -2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(245, 90);
            this.panel3.TabIndex = 3;
            // 
            // btnMassfill
            // 
            this.btnMassfill.Enabled = false;
            this.btnMassfill.Location = new System.Drawing.Point(148, 34);
            this.btnMassfill.Name = "btnMassfill";
            this.btnMassfill.Size = new System.Drawing.Size(75, 23);
            this.btnMassfill.TabIndex = 9;
            this.btnMassfill.Text = "Mass Close";
            this.btnMassfill.Click += new System.EventHandler(this.btnMassfill_Click);
            // 
            // lblCompletiondt
            // 
            this.lblCompletiondt.AutoSize = true;
            this.lblCompletiondt.Location = new System.Drawing.Point(1, 9);
            this.lblCompletiondt.Name = "lblCompletiondt";
            this.lblCompletiondt.RightToLeft = Wisej.Web.RightToLeft.No;
            this.lblCompletiondt.Size = new System.Drawing.Size(130, 14);
            this.lblCompletiondt.TabIndex = 0;
            this.lblCompletiondt.Text = "Actual Completion Date";
            // 
            // dtcompletion
            // 
            this.dtcompletion.Checked = false;
            this.dtcompletion.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.dtcompletion.Location = new System.Drawing.Point(120, 5);
            this.dtcompletion.Name = "dtcompletion";
            this.dtcompletion.ShowCheckBox = true;
            this.dtcompletion.ShowToolTips = false;
            this.dtcompletion.Size = new System.Drawing.Size(103, 22);
            this.dtcompletion.TabIndex = 8;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(304, 61);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // rdoServicePlan
            // 
            this.rdoServicePlan.Location = new System.Drawing.Point(165, 11);
            this.rdoServicePlan.Name = "rdoServicePlan";
            this.rdoServicePlan.Size = new System.Drawing.Size(128, 21);
            this.rdoServicePlan.TabIndex = 1;
            this.rdoServicePlan.Text = "Service Plan Date";
            // 
            // dtpFrmDate
            // 
            this.dtpFrmDate.Checked = false;
            this.dtpFrmDate.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.dtpFrmDate.Location = new System.Drawing.Point(37, 36);
            this.dtpFrmDate.Name = "dtpFrmDate";
            this.dtpFrmDate.ShowCheckBox = true;
            this.dtpFrmDate.ShowToolTips = false;
            this.dtpFrmDate.Size = new System.Drawing.Size(102, 22);
            this.dtpFrmDate.TabIndex = 2;
            // 
            // rdoIntake
            // 
            this.rdoIntake.Checked = true;
            this.rdoIntake.Location = new System.Drawing.Point(10, 11);
            this.rdoIntake.Name = "rdoIntake";
            this.rdoIntake.Size = new System.Drawing.Size(94, 21);
            this.rdoIntake.TabIndex = 0;
            this.rdoIntake.TabStop = true;
            this.rdoIntake.Text = "Intake Date";
            // 
            // dtpToDt
            // 
            this.dtpToDt.Checked = false;
            this.dtpToDt.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.dtpToDt.Location = new System.Drawing.Point(181, 36);
            this.dtpToDt.Name = "dtpToDt";
            this.dtpToDt.ShowCheckBox = true;
            this.dtpToDt.ShowToolTips = false;
            this.dtpToDt.Size = new System.Drawing.Size(103, 22);
            this.dtpToDt.TabIndex = 3;
            // 
            // lblToDt
            // 
            this.lblToDt.AutoSize = true;
            this.lblToDt.Location = new System.Drawing.Point(162, 40);
            this.lblToDt.Name = "lblToDt";
            this.lblToDt.RightToLeft = Wisej.Web.RightToLeft.No;
            this.lblToDt.Size = new System.Drawing.Size(19, 14);
            this.lblToDt.TabIndex = 0;
            this.lblToDt.Text = "To";
            // 
            // lblFrmDt
            // 
            this.lblFrmDt.AutoSize = true;
            this.lblFrmDt.Location = new System.Drawing.Point(7, 40);
            this.lblFrmDt.Name = "lblFrmDt";
            this.lblFrmDt.RightToLeft = Wisej.Web.RightToLeft.No;
            this.lblFrmDt.Size = new System.Drawing.Size(32, 14);
            this.lblFrmDt.TabIndex = 0;
            this.lblFrmDt.Text = "From";
            // 
            // CASE0006CLOSESCREEN
            // 
            this.ClientSize = new System.Drawing.Size(643, 429);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CASE0006CLOSESCREEN";
            this.Text = "CASE0006CLOSESCREEN";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwMSDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwService)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion
        private Panel panel1;
        private Panel panel2;
        private RadioButton rdoServicePlan;
        private DateTimePicker dtpFrmDate;
        private RadioButton rdoIntake;
        private DateTimePicker dtpToDt;
        private Label lblToDt;
        private Label lblFrmDt;
        private DataGridView gvwService;
        private DataGridViewTextBoxColumn gvtClientName;
        private DataGridViewTextBoxColumn gvtSpmName;
        private DataGridViewTextBoxColumn gvtIntakedt;
        private DataGridViewTextBoxColumn gvtCACount;
        private DataGridViewTextBoxColumn gvtMACount;
        private Button btnSearch;
        private DataGridViewTextBoxColumn gvtStartdt;
        private DataGridViewTextBoxColumn gvtSpmId;
        private DataGridViewTextBoxColumn gvtSpmSeq;
        private DataGridViewTextBoxColumn gvtAppNo;
        private DataGridViewCheckBoxColumn gvtCheckbox;
        private Label lblCompletiondt;
        private Button btnMassfill;
        private DateTimePicker dtcompletion;
        private Panel panel3;
        private CheckBox chkSelectAll;
        private Label lblMSResult;
        private ComboBoxEx cmbMsResult;
        private Button btnMSUpdate;
        private DataGridView gvwMSDetails;
        private DataGridViewTextBoxColumn gvtMSSwitch;
        private DataGridViewCheckBoxColumn gvcMsSelect;
        private DataGridViewTextBoxColumn gvtMSDesc;
        private DataGridViewTextBoxColumn gvtMSResultDesc;
        private DataGridViewTextBoxColumn gvtMSDate;
        private DataGridViewTextBoxColumn gvtMsCode;
        private DataGridViewTextBoxColumn gvtMSResultCode;
        private DataGridViewTextBoxColumn gvtMsBranch;
        private DataGridViewTextBoxColumn gvtMsGroup;
        private DataGridViewTextBoxColumn gvtMSSpmSeq;
        private DataGridViewTextBoxColumn gvtMsSpmplan;
        private DataGridViewTextBoxColumn gvtMsApplicant;
        private Label lblMSDate;
        private DateTimePicker dtpMSdate;
        private ComboBoxEx cmbResult;
        private Label lblResult;
    }
}