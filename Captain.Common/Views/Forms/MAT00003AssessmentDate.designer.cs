//using Wisej.Web;
//using Gizmox.WebGUI.Common;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class MAT00003AssessmentDate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MAT00003AssessmentDate));
            this.pnlCaseworkers = new Wisej.Web.Panel();
            this.lblReqWorker = new Wisej.Web.Label();
            this.label3 = new Wisej.Web.Label();
            this.lblCaseWorker = new Wisej.Web.Label();
            this.cmbCaseWorker = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.pnlassDate = new Wisej.Web.Panel();
            this.dtAssessmentDate = new Wisej.Web.DateTimePicker();
            this.lblAssessmentDate = new Wisej.Web.Label();
            this.picAddAssDate = new Wisej.Web.PictureBox();
            this.lblEjobTitleReq = new Wisej.Web.Label();
            this.label1 = new Wisej.Web.Label();
            this.pnlDate = new Wisej.Web.Panel();
            this.lblReqCompDte = new Wisej.Web.Label();
            this.label5 = new Wisej.Web.Label();
            this.lblReqFolUpDte = new Wisej.Web.Label();
            this.label4 = new Wisej.Web.Label();
            this.dtCompleted = new Wisej.Web.DateTimePicker();
            this.lblFon = new Wisej.Web.Label();
            this.lblCompdate = new Wisej.Web.Label();
            this.dtFollowup = new Wisej.Web.DateTimePicker();
            this.txtTotalScore = new Wisej.Web.TextBox();
            this.lblTot = new Wisej.Web.Label();
            this.lblParitalScoreDesc = new Wisej.Web.Label();
            this.lblPartial = new Wisej.Web.Label();
            this.lblTotalScoreDesc = new Wisej.Web.Label();
            this.txtPatialScore = new Wisej.Web.TextBox();
            this.gvwAssessmentDetails = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.Date = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.CaseWorker = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Delete = new Wisej.Web.DataGridViewImageColumn();
            this.Mat_Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.btnCalNxtAssDate = new Wisej.Web.Button();
            this.pnlDaysInterval = new Wisej.Web.Panel();
            this.txtIntervalIndays = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblIntervalDays = new Wisej.Web.Label();
            this.btnCancel = new Wisej.Web.Button();
            this.btnSave = new Wisej.Web.Button();
            this.pnlgvwAssDetails = new Wisej.Web.Panel();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlAssDateScores = new Wisej.Web.Panel();
            this.pnlScores = new Wisej.Web.Panel();
            this.pnlCaseandFollup = new Wisej.Web.Panel();
            this.pnlSave = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlCaseworkers.SuspendLayout();
            this.lblReqWorker.SuspendLayout();
            this.pnlassDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAddAssDate)).BeginInit();
            this.lblEjobTitleReq.SuspendLayout();
            this.pnlDate.SuspendLayout();
            this.lblReqCompDte.SuspendLayout();
            this.lblReqFolUpDte.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwAssessmentDetails)).BeginInit();
            this.pnlDaysInterval.SuspendLayout();
            this.pnlgvwAssDetails.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlAssDateScores.SuspendLayout();
            this.pnlScores.SuspendLayout();
            this.pnlCaseandFollup.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCaseworkers
            // 
            this.pnlCaseworkers.Controls.Add(this.lblReqWorker);
            this.pnlCaseworkers.Controls.Add(this.lblCaseWorker);
            this.pnlCaseworkers.Controls.Add(this.cmbCaseWorker);
            this.pnlCaseworkers.Dock = Wisej.Web.DockStyle.Left;
            this.pnlCaseworkers.Location = new System.Drawing.Point(0, 0);
            this.pnlCaseworkers.Name = "pnlCaseworkers";
            this.pnlCaseworkers.Size = new System.Drawing.Size(323, 66);
            this.pnlCaseworkers.TabIndex = 10;
            // 
            // lblReqWorker
            // 
            this.lblReqWorker.Controls.Add(this.label3);
            this.lblReqWorker.ForeColor = System.Drawing.Color.Red;
            this.lblReqWorker.Location = new System.Drawing.Point(89, 6);
            this.lblReqWorker.Name = "lblReqWorker";
            this.lblReqWorker.Size = new System.Drawing.Size(7, 10);
            this.lblReqWorker.TabIndex = 5;
            this.lblReqWorker.Text = "*";
            this.lblReqWorker.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(-11, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(9, 14);
            this.label3.TabIndex = 0;
            this.label3.Text = "*";
            this.label3.Visible = false;
            // 
            // lblCaseWorker
            // 
            this.lblCaseWorker.Location = new System.Drawing.Point(15, 7);
            this.lblCaseWorker.Name = "lblCaseWorker";
            this.lblCaseWorker.Size = new System.Drawing.Size(73, 14);
            this.lblCaseWorker.TabIndex = 4;
            this.lblCaseWorker.Text = "Case Worker";
            // 
            // cmbCaseWorker
            // 
            this.cmbCaseWorker.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbCaseWorker.FormattingEnabled = true;
            this.cmbCaseWorker.Location = new System.Drawing.Point(123, 3);
            this.cmbCaseWorker.Name = "cmbCaseWorker";
            this.cmbCaseWorker.Size = new System.Drawing.Size(193, 25);
            this.cmbCaseWorker.TabIndex = 4;
            // 
            // pnlassDate
            // 
            this.pnlassDate.Controls.Add(this.dtAssessmentDate);
            this.pnlassDate.Controls.Add(this.lblAssessmentDate);
            this.pnlassDate.Controls.Add(this.picAddAssDate);
            this.pnlassDate.Controls.Add(this.lblEjobTitleReq);
            this.pnlassDate.Dock = Wisej.Web.DockStyle.Top;
            this.pnlassDate.Location = new System.Drawing.Point(0, 0);
            this.pnlassDate.Name = "pnlassDate";
            this.pnlassDate.Size = new System.Drawing.Size(577, 35);
            this.pnlassDate.TabIndex = 9;
            // 
            // dtAssessmentDate
            // 
            this.dtAssessmentDate.AutoSize = false;
            this.dtAssessmentDate.CustomFormat = "MM/dd/yyyy";
            this.dtAssessmentDate.Enabled = false;
            this.dtAssessmentDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtAssessmentDate.Location = new System.Drawing.Point(123, 6);
            this.dtAssessmentDate.Name = "dtAssessmentDate";
            this.dtAssessmentDate.ShowCheckBox = true;
            this.dtAssessmentDate.ShowToolTips = false;
            this.dtAssessmentDate.Size = new System.Drawing.Size(116, 25);
            this.dtAssessmentDate.TabIndex = 3;
            // 
            // lblAssessmentDate
            // 
            this.lblAssessmentDate.Location = new System.Drawing.Point(15, 10);
            this.lblAssessmentDate.Name = "lblAssessmentDate";
            this.lblAssessmentDate.Size = new System.Drawing.Size(98, 14);
            this.lblAssessmentDate.TabIndex = 4;
            this.lblAssessmentDate.Text = "Assessment Date";
            // 
            // picAddAssDate
            // 
            this.picAddAssDate.BackgroundImageLayout = Wisej.Web.ImageLayout.BestFit;
            this.picAddAssDate.Cursor = Wisej.Web.Cursors.Hand;
            this.picAddAssDate.ImageSource = "captain-add";
            this.picAddAssDate.Location = new System.Drawing.Point(268, 7);
            this.picAddAssDate.Name = "picAddAssDate";
            this.picAddAssDate.Size = new System.Drawing.Size(24, 22);
            this.picAddAssDate.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.picAddAssDate.Visible = false;
            this.picAddAssDate.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // lblEjobTitleReq
            // 
            this.lblEjobTitleReq.Controls.Add(this.label1);
            this.lblEjobTitleReq.ForeColor = System.Drawing.Color.Red;
            this.lblEjobTitleReq.Location = new System.Drawing.Point(8, 6);
            this.lblEjobTitleReq.Name = "lblEjobTitleReq";
            this.lblEjobTitleReq.Size = new System.Drawing.Size(7, 10);
            this.lblEjobTitleReq.TabIndex = 0;
            this.lblEjobTitleReq.Text = "*";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(-11, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(9, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "*";
            this.label1.Visible = false;
            // 
            // pnlDate
            // 
            this.pnlDate.Controls.Add(this.lblReqCompDte);
            this.pnlDate.Controls.Add(this.lblReqFolUpDte);
            this.pnlDate.Controls.Add(this.dtCompleted);
            this.pnlDate.Controls.Add(this.lblFon);
            this.pnlDate.Controls.Add(this.lblCompdate);
            this.pnlDate.Controls.Add(this.dtFollowup);
            this.pnlDate.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlDate.Location = new System.Drawing.Point(323, 0);
            this.pnlDate.Name = "pnlDate";
            this.pnlDate.Size = new System.Drawing.Size(254, 66);
            this.pnlDate.TabIndex = 9;
            // 
            // lblReqCompDte
            // 
            this.lblReqCompDte.Controls.Add(this.label5);
            this.lblReqCompDte.ForeColor = System.Drawing.Color.Red;
            this.lblReqCompDte.Location = new System.Drawing.Point(93, 37);
            this.lblReqCompDte.Name = "lblReqCompDte";
            this.lblReqCompDte.Size = new System.Drawing.Size(7, 10);
            this.lblReqCompDte.TabIndex = 10;
            this.lblReqCompDte.Text = "*";
            this.lblReqCompDte.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(-11, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(9, 14);
            this.label5.TabIndex = 0;
            this.label5.Text = "*";
            this.label5.Visible = false;
            // 
            // lblReqFolUpDte
            // 
            this.lblReqFolUpDte.Controls.Add(this.label4);
            this.lblReqFolUpDte.ForeColor = System.Drawing.Color.Red;
            this.lblReqFolUpDte.Location = new System.Drawing.Point(86, 8);
            this.lblReqFolUpDte.Name = "lblReqFolUpDte";
            this.lblReqFolUpDte.Size = new System.Drawing.Size(7, 10);
            this.lblReqFolUpDte.TabIndex = 9;
            this.lblReqFolUpDte.Text = "*";
            this.lblReqFolUpDte.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(-11, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(9, 14);
            this.label4.TabIndex = 0;
            this.label4.Text = "*";
            this.label4.Visible = false;
            // 
            // dtCompleted
            // 
            this.dtCompleted.AutoSize = false;
            this.dtCompleted.Checked = false;
            this.dtCompleted.CustomFormat = "MM/dd/yyyy";
            this.dtCompleted.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtCompleted.Location = new System.Drawing.Point(109, 38);
            this.dtCompleted.Name = "dtCompleted";
            this.dtCompleted.ShowCheckBox = true;
            this.dtCompleted.ShowToolTips = false;
            this.dtCompleted.Size = new System.Drawing.Size(116, 25);
            this.dtCompleted.TabIndex = 8;
            this.dtCompleted.Visible = false;
            // 
            // lblFon
            // 
            this.lblFon.Location = new System.Drawing.Point(10, 10);
            this.lblFon.Name = "lblFon";
            this.lblFon.Size = new System.Drawing.Size(75, 16);
            this.lblFon.TabIndex = 0;
            this.lblFon.Text = "Follow up On";
            this.lblFon.Visible = false;
            // 
            // lblCompdate
            // 
            this.lblCompdate.Location = new System.Drawing.Point(10, 40);
            this.lblCompdate.Name = "lblCompdate";
            this.lblCompdate.Size = new System.Drawing.Size(85, 16);
            this.lblCompdate.TabIndex = 0;
            this.lblCompdate.Text = "Complete Date";
            this.lblCompdate.Visible = false;
            // 
            // dtFollowup
            // 
            this.dtFollowup.AutoSize = false;
            this.dtFollowup.Checked = false;
            this.dtFollowup.CustomFormat = "MM/dd/yyyy";
            this.dtFollowup.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtFollowup.Location = new System.Drawing.Point(109, 6);
            this.dtFollowup.Name = "dtFollowup";
            this.dtFollowup.ShowCheckBox = true;
            this.dtFollowup.ShowToolTips = false;
            this.dtFollowup.Size = new System.Drawing.Size(116, 25);
            this.dtFollowup.TabIndex = 7;
            this.dtFollowup.Visible = false;
            // 
            // txtTotalScore
            // 
            this.txtTotalScore.Enabled = false;
            this.txtTotalScore.Location = new System.Drawing.Point(123, 5);
            this.txtTotalScore.MaxLength = 4;
            this.txtTotalScore.Name = "txtTotalScore";
            this.txtTotalScore.ReadOnly = true;
            this.txtTotalScore.Size = new System.Drawing.Size(43, 25);
            this.txtTotalScore.TabIndex = 5;
            this.txtTotalScore.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtTotalScore.Visible = false;
            // 
            // lblTot
            // 
            this.lblTot.Location = new System.Drawing.Point(15, 9);
            this.lblTot.Name = "lblTot";
            this.lblTot.Size = new System.Drawing.Size(65, 14);
            this.lblTot.TabIndex = 1;
            this.lblTot.Text = "Total Score";
            this.lblTot.Visible = false;
            // 
            // lblParitalScoreDesc
            // 
            this.lblParitalScoreDesc.Location = new System.Drawing.Point(180, 40);
            this.lblParitalScoreDesc.Name = "lblParitalScoreDesc";
            this.lblParitalScoreDesc.Size = new System.Drawing.Size(368, 16);
            this.lblParitalScoreDesc.TabIndex = 1;
            this.lblParitalScoreDesc.Visible = false;
            // 
            // lblPartial
            // 
            this.lblPartial.Location = new System.Drawing.Point(15, 41);
            this.lblPartial.Name = "lblPartial";
            this.lblPartial.Size = new System.Drawing.Size(72, 14);
            this.lblPartial.TabIndex = 1;
            this.lblPartial.Text = "Partial Score";
            this.lblPartial.Visible = false;
            // 
            // lblTotalScoreDesc
            // 
            this.lblTotalScoreDesc.Location = new System.Drawing.Point(180, 9);
            this.lblTotalScoreDesc.Name = "lblTotalScoreDesc";
            this.lblTotalScoreDesc.Size = new System.Drawing.Size(368, 16);
            this.lblTotalScoreDesc.TabIndex = 1;
            this.lblTotalScoreDesc.Visible = false;
            // 
            // txtPatialScore
            // 
            this.txtPatialScore.Enabled = false;
            this.txtPatialScore.Location = new System.Drawing.Point(123, 37);
            this.txtPatialScore.MaxLength = 4;
            this.txtPatialScore.Name = "txtPatialScore";
            this.txtPatialScore.ReadOnly = true;
            this.txtPatialScore.Size = new System.Drawing.Size(43, 25);
            this.txtPatialScore.TabIndex = 6;
            this.txtPatialScore.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtPatialScore.Visible = false;
            // 
            // gvwAssessmentDetails
            // 
            this.gvwAssessmentDetails.AllowUserToResizeColumns = false;
            this.gvwAssessmentDetails.AllowUserToResizeRows = false;
            this.gvwAssessmentDetails.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwAssessmentDetails.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwAssessmentDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwAssessmentDetails.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwAssessmentDetails.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Date,
            this.CaseWorker,
            this.Delete,
            this.Mat_Code});
            this.gvwAssessmentDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwAssessmentDetails.Name = "gvwAssessmentDetails";
            this.gvwAssessmentDetails.RowHeadersWidth = 14;
            this.gvwAssessmentDetails.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwAssessmentDetails.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwAssessmentDetails.ShowColumnVisibilityMenu = false;
            this.gvwAssessmentDetails.Size = new System.Drawing.Size(577, 158);
            this.gvwAssessmentDetails.TabIndex = 2;
            this.gvwAssessmentDetails.SelectionChanged += new System.EventHandler(this.gvwAssessmentDetails_SelectionChanged);
            this.gvwAssessmentDetails.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwAssessmentDetails_CellClick);
            // 
            // Date
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Date.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Date.HeaderStyle = dataGridViewCellStyle3;
            this.Date.HeaderText = "Assessment Date";
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            this.Date.ShowInVisibilityMenu = false;
            this.Date.Width = 120;
            // 
            // CaseWorker
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.CaseWorker.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.CaseWorker.HeaderStyle = dataGridViewCellStyle5;
            this.CaseWorker.HeaderText = "Case Worker";
            this.CaseWorker.Name = "CaseWorker";
            this.CaseWorker.ReadOnly = true;
            this.CaseWorker.ShowInVisibilityMenu = false;
            this.CaseWorker.Width = 330;
            // 
            // Delete
            // 
            this.Delete.CellImageSource = "captain-delete";
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.NullValue = null;
            this.Delete.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Delete.HeaderStyle = dataGridViewCellStyle7;
            this.Delete.HeaderText = "Delete";
            this.Delete.Name = "Delete";
            this.Delete.ShowInVisibilityMenu = false;
            this.Delete.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.Delete.Width = 45;
            // 
            // Mat_Code
            // 
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Mat_Code.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Mat_Code.HeaderStyle = dataGridViewCellStyle9;
            this.Mat_Code.HeaderText = "MatCode";
            this.Mat_Code.Name = "Mat_Code";
            this.Mat_Code.ReadOnly = true;
            this.Mat_Code.ShowInVisibilityMenu = false;
            this.Mat_Code.Visible = false;
            this.Mat_Code.Width = 50;
            // 
            // btnCalNxtAssDate
            // 
            this.btnCalNxtAssDate.Location = new System.Drawing.Point(181, 11);
            this.btnCalNxtAssDate.Name = "btnCalNxtAssDate";
            this.btnCalNxtAssDate.Size = new System.Drawing.Size(209, 25);
            this.btnCalNxtAssDate.TabIndex = 2;
            this.btnCalNxtAssDate.Text = "Calculate Next &Assessment Date";
            this.btnCalNxtAssDate.Click += new System.EventHandler(this.button1_Click);
            // 
            // pnlDaysInterval
            // 
            this.pnlDaysInterval.Controls.Add(this.txtIntervalIndays);
            this.pnlDaysInterval.Controls.Add(this.lblIntervalDays);
            this.pnlDaysInterval.Controls.Add(this.btnCalNxtAssDate);
            this.pnlDaysInterval.Dock = Wisej.Web.DockStyle.Top;
            this.pnlDaysInterval.Location = new System.Drawing.Point(0, 0);
            this.pnlDaysInterval.Name = "pnlDaysInterval";
            this.pnlDaysInterval.Size = new System.Drawing.Size(577, 42);
            this.pnlDaysInterval.TabIndex = 2;
            // 
            // txtIntervalIndays
            // 
            this.txtIntervalIndays.Location = new System.Drawing.Point(122, 11);
            this.txtIntervalIndays.MaxLength = 4;
            this.txtIntervalIndays.Name = "txtIntervalIndays";
            this.txtIntervalIndays.Size = new System.Drawing.Size(33, 25);
            this.txtIntervalIndays.TabIndex = 1;
            this.txtIntervalIndays.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // lblIntervalDays
            // 
            this.lblIntervalDays.Location = new System.Drawing.Point(15, 15);
            this.lblIntervalDays.Name = "lblIntervalDays";
            this.lblIntervalDays.Size = new System.Drawing.Size(96, 16);
            this.lblIntervalDays.TabIndex = 1;
            this.lblIntervalDays.Text = "Interval (In Days)";
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(487, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(409, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlgvwAssDetails
            // 
            this.pnlgvwAssDetails.Controls.Add(this.gvwAssessmentDetails);
            this.pnlgvwAssDetails.Dock = Wisej.Web.DockStyle.Top;
            this.pnlgvwAssDetails.Location = new System.Drawing.Point(0, 42);
            this.pnlgvwAssDetails.Name = "pnlgvwAssDetails";
            this.pnlgvwAssDetails.Size = new System.Drawing.Size(577, 158);
            this.pnlgvwAssDetails.TabIndex = 11;
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlAssDateScores);
            this.pnlCompleteForm.Controls.Add(this.pnlgvwAssDetails);
            this.pnlCompleteForm.Controls.Add(this.pnlDaysInterval);
            this.pnlCompleteForm.Controls.Add(this.pnlSave);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(577, 404);
            this.pnlCompleteForm.TabIndex = 12;
            // 
            // pnlAssDateScores
            // 
            this.pnlAssDateScores.Controls.Add(this.pnlScores);
            this.pnlAssDateScores.Controls.Add(this.pnlCaseandFollup);
            this.pnlAssDateScores.Controls.Add(this.pnlassDate);
            this.pnlAssDateScores.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlAssDateScores.Location = new System.Drawing.Point(0, 200);
            this.pnlAssDateScores.Name = "pnlAssDateScores";
            this.pnlAssDateScores.Size = new System.Drawing.Size(577, 169);
            this.pnlAssDateScores.TabIndex = 1;
            // 
            // pnlScores
            // 
            this.pnlScores.Controls.Add(this.lblParitalScoreDesc);
            this.pnlScores.Controls.Add(this.txtTotalScore);
            this.pnlScores.Controls.Add(this.lblPartial);
            this.pnlScores.Controls.Add(this.lblTot);
            this.pnlScores.Controls.Add(this.txtPatialScore);
            this.pnlScores.Controls.Add(this.lblTotalScoreDesc);
            this.pnlScores.Dock = Wisej.Web.DockStyle.Top;
            this.pnlScores.Location = new System.Drawing.Point(0, 101);
            this.pnlScores.Name = "pnlScores";
            this.pnlScores.Size = new System.Drawing.Size(577, 68);
            this.pnlScores.TabIndex = 7;
            // 
            // pnlCaseandFollup
            // 
            this.pnlCaseandFollup.Controls.Add(this.pnlDate);
            this.pnlCaseandFollup.Controls.Add(this.pnlCaseworkers);
            this.pnlCaseandFollup.Dock = Wisej.Web.DockStyle.Top;
            this.pnlCaseandFollup.Location = new System.Drawing.Point(0, 35);
            this.pnlCaseandFollup.Name = "pnlCaseandFollup";
            this.pnlCaseandFollup.Size = new System.Drawing.Size(577, 66);
            this.pnlCaseandFollup.TabIndex = 10;
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnSave);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 369);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(577, 35);
            this.pnlSave.TabIndex = 0;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(484, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // MAT00003AssessmentDate
            // 
            this.ClientSize = new System.Drawing.Size(577, 404);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MAT00003AssessmentDate";
            this.Text = "MAT00003AssessmentDate";
            this.Load += new System.EventHandler(this.MAT00003AssessmentDate_Load);
            this.pnlCaseworkers.ResumeLayout(false);
            this.pnlCaseworkers.PerformLayout();
            this.lblReqWorker.ResumeLayout(false);
            this.lblReqWorker.PerformLayout();
            this.pnlassDate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picAddAssDate)).EndInit();
            this.lblEjobTitleReq.ResumeLayout(false);
            this.lblEjobTitleReq.PerformLayout();
            this.pnlDate.ResumeLayout(false);
            this.lblReqCompDte.ResumeLayout(false);
            this.lblReqCompDte.PerformLayout();
            this.lblReqFolUpDte.ResumeLayout(false);
            this.lblReqFolUpDte.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwAssessmentDetails)).EndInit();
            this.pnlDaysInterval.ResumeLayout(false);
            this.pnlDaysInterval.PerformLayout();
            this.pnlgvwAssDetails.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlAssDateScores.ResumeLayout(false);
            this.pnlScores.ResumeLayout(false);
            this.pnlScores.PerformLayout();
            this.pnlCaseandFollup.ResumeLayout(false);
            this.pnlSave.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Button btnCalNxtAssDate;
        private Panel pnlDaysInterval;
        private Label lblAssessmentDate;
        private Label lblCaseWorker;
        private DataGridViewEx gvwAssessmentDetails;
        private DataGridViewTextBoxColumn CaseWorker;
        private DateTimePicker dtAssessmentDate;
        private TextBoxWithValidation txtIntervalIndays;
        private Label lblIntervalDays;
        private Button btnCancel;
        private Button btnSave;
        private DataGridViewImageColumn Delete;
        private Label lblEjobTitleReq;
        private Label label1;
        private DataGridViewTextBoxColumn Mat_Code;
        private PictureBox picAddAssDate;
        private TextBox txtPatialScore;
        private Label lblPartial;
        private Label lblTot;
        private TextBox txtTotalScore;
        private Label lblParitalScoreDesc;
        private Label lblTotalScoreDesc;
        private Label lblFon;
        private DateTimePicker dtFollowup;
        private DateTimePicker dtCompleted;
        private Label lblCompdate;
        private Panel pnlgvwAssDetails;
        private Panel pnlCompleteForm;
        private Panel pnlSave;
        private Spacer spacer1;
        private ComboBoxEx cmbCaseWorker;
        private Panel pnlDate;
        private Panel pnlCaseworkers;
        private Panel pnlassDate;
        private DataGridViewDateTimeColumn Date;
        private Panel pnlAssDateScores;
        private Panel pnlScores;
        private Panel pnlCaseandFollup;
        private Label lblReqWorker;
        private Label label3;
        private Label lblReqCompDte;
        private Label label5;
        private Label lblReqFolUpDte;
        private Label label4;
    }
}