using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class MAT00003ReasonCodes
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
            this.components = new System.ComponentModel.Container();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MAT00003ReasonCodes));
            this.contextMenu1 = new Wisej.Web.ContextMenu(this.components);
            this.btnCancel = new Wisej.Web.Button();
            this.CaseWorker = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dtAssessmentDate = new Wisej.Web.DateTimePicker();
            this.lblMatrix = new Wisej.Web.Label();
            this.cmbMatrix = new Wisej.Web.ComboBox();
            this.cmbScale = new Wisej.Web.ComboBox();
            this.lblScale = new Wisej.Web.Label();
            this.gvwReasons = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.Date = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.Score = new Wisej.Web.DataGridViewTextBoxColumn();
            this.lblAssessmentDate = new Wisej.Web.Label();
            this.lblViewBenchmark = new Wisej.Web.Label();
            this.btnSave = new Wisej.Web.Button();
            this.cmbReason3 = new Wisej.Web.ComboBox();
            this.cmbReason1 = new Wisej.Web.ComboBox();
            this.cmbReason4 = new Wisej.Web.ComboBox();
            this.cmbReason2 = new Wisej.Web.ComboBox();
            this.cmbReason6 = new Wisej.Web.ComboBox();
            this.cmbReason5 = new Wisej.Web.ComboBox();
            this.lblChangeDimesion = new Wisej.Web.Label();
            this.lblChangeDTitle = new Wisej.Web.Label();
            this.pnlMatrix = new Wisej.Web.Panel();
            this.pnlgvwReasons = new Wisej.Web.Panel();
            this.pnlReasons = new Wisej.Web.Panel();
            this.pnlSave = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            ((System.ComponentModel.ISupportInitialize)(this.gvwReasons)).BeginInit();
            this.pnlMatrix.SuspendLayout();
            this.pnlgvwReasons.SuspendLayout();
            this.pnlReasons.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenu1
            // 
            this.contextMenu1.Name = "contextMenu1";
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-cancel";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(530, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // CaseWorker
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.CaseWorker.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.CaseWorker.HeaderStyle = dataGridViewCellStyle2;
            this.CaseWorker.HeaderText = "CaseWorker";
            this.CaseWorker.Name = "CaseWorker";
            this.CaseWorker.ReadOnly = true;
            this.CaseWorker.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.CaseWorker.Width = 400;
            // 
            // dtAssessmentDate
            // 
            this.dtAssessmentDate.CustomFormat = "MM/dd/yyyy";
            this.dtAssessmentDate.Enabled = false;
            this.dtAssessmentDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtAssessmentDate.Location = new System.Drawing.Point(440, 12);
            this.dtAssessmentDate.MinimumSize = new System.Drawing.Size(0, 25);
            this.dtAssessmentDate.Name = "dtAssessmentDate";
            this.dtAssessmentDate.ShowCheckBox = true;
            this.dtAssessmentDate.ShowToolTips = false;
            this.dtAssessmentDate.Size = new System.Drawing.Size(125, 25);
            this.dtAssessmentDate.TabIndex = 9;
            // 
            // lblMatrix
            // 
            this.lblMatrix.AutoSize = true;
            this.lblMatrix.Location = new System.Drawing.Point(12, 15);
            this.lblMatrix.Name = "lblMatrix";
            this.lblMatrix.Size = new System.Drawing.Size(37, 14);
            this.lblMatrix.TabIndex = 0;
            this.lblMatrix.Text = "Matrix ";
            // 
            // cmbMatrix
            // 
            this.cmbMatrix.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbMatrix.FormattingEnabled = true;
            this.cmbMatrix.Location = new System.Drawing.Point(66, 13);
            this.cmbMatrix.Name = "cmbMatrix";
            this.cmbMatrix.Size = new System.Drawing.Size(219, 25);
            this.cmbMatrix.TabIndex = 1;
            this.cmbMatrix.SelectedIndexChanged += new System.EventHandler(this.cmbMatrix_SelectedIndexChanged);
            // 
            // cmbScale
            // 
            this.cmbScale.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbScale.FormattingEnabled = true;
            this.cmbScale.Location = new System.Drawing.Point(66, 43);
            this.cmbScale.Name = "cmbScale";
            this.cmbScale.Size = new System.Drawing.Size(219, 25);
            this.cmbScale.TabIndex = 1;
            // 
            // lblScale
            // 
            this.lblScale.AutoSize = true;
            this.lblScale.Location = new System.Drawing.Point(12, 46);
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(35, 14);
            this.lblScale.TabIndex = 0;
            this.lblScale.Text = "Scale";
            // 
            // gvwReasons
            // 
            this.gvwReasons.AllowUserToResizeColumns = false;
            this.gvwReasons.AllowUserToResizeRows = false;
            this.gvwReasons.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwReasons.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gvwReasons.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwReasons.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.CaseWorker,
            this.Date,
            this.Score});
            this.gvwReasons.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwReasons.MultiSelect = false;
            this.gvwReasons.Name = "gvwReasons";
            this.gvwReasons.ReadOnly = true;
            this.gvwReasons.RowHeadersWidth = 14;
            this.gvwReasons.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwReasons.Size = new System.Drawing.Size(618, 121);
            this.gvwReasons.TabIndex = 0;
            // 
            // Date
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Date.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Date.HeaderStyle = dataGridViewCellStyle5;
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            this.Date.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.Date.Width = 120;
            // 
            // Score
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Score.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Score.HeaderStyle = dataGridViewCellStyle7;
            this.Score.HeaderText = "Score";
            this.Score.Name = "Score";
            this.Score.ReadOnly = true;
            this.Score.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.Score.Width = 80;
            // 
            // lblAssessmentDate
            // 
            this.lblAssessmentDate.AutoSize = true;
            this.lblAssessmentDate.Location = new System.Drawing.Point(337, 17);
            this.lblAssessmentDate.Name = "lblAssessmentDate";
            this.lblAssessmentDate.Size = new System.Drawing.Size(98, 14);
            this.lblAssessmentDate.TabIndex = 4;
            this.lblAssessmentDate.Text = "Assessment Date";
            // 
            // lblViewBenchmark
            // 
            this.lblViewBenchmark.AutoSize = true;
            this.lblViewBenchmark.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblViewBenchmark.ForeColor = System.Drawing.Color.FromArgb(0, 0, 192);
            this.lblViewBenchmark.Location = new System.Drawing.Point(351, 134);
            this.lblViewBenchmark.Name = "lblViewBenchmark";
            this.lblViewBenchmark.Size = new System.Drawing.Size(4, 14);
            this.lblViewBenchmark.TabIndex = 36;
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(452, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cmbReason3
            // 
            this.cmbReason3.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbReason3.FormattingEnabled = true;
            this.cmbReason3.Location = new System.Drawing.Point(12, 60);
            this.cmbReason3.Name = "cmbReason3";
            this.cmbReason3.Size = new System.Drawing.Size(228, 25);
            this.cmbReason3.TabIndex = 3;
            // 
            // cmbReason1
            // 
            this.cmbReason1.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbReason1.FormattingEnabled = true;
            this.cmbReason1.Location = new System.Drawing.Point(12, 31);
            this.cmbReason1.Name = "cmbReason1";
            this.cmbReason1.Size = new System.Drawing.Size(228, 25);
            this.cmbReason1.TabIndex = 1;
            // 
            // cmbReason4
            // 
            this.cmbReason4.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbReason4.FormattingEnabled = true;
            this.cmbReason4.Location = new System.Drawing.Point(337, 60);
            this.cmbReason4.Name = "cmbReason4";
            this.cmbReason4.Size = new System.Drawing.Size(228, 25);
            this.cmbReason4.TabIndex = 4;
            // 
            // cmbReason2
            // 
            this.cmbReason2.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbReason2.FormattingEnabled = true;
            this.cmbReason2.Location = new System.Drawing.Point(337, 31);
            this.cmbReason2.Name = "cmbReason2";
            this.cmbReason2.Size = new System.Drawing.Size(228, 25);
            this.cmbReason2.TabIndex = 2;
            // 
            // cmbReason6
            // 
            this.cmbReason6.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbReason6.FormattingEnabled = true;
            this.cmbReason6.Location = new System.Drawing.Point(337, 89);
            this.cmbReason6.Name = "cmbReason6";
            this.cmbReason6.Size = new System.Drawing.Size(228, 25);
            this.cmbReason6.TabIndex = 6;
            // 
            // cmbReason5
            // 
            this.cmbReason5.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbReason5.FormattingEnabled = true;
            this.cmbReason5.Location = new System.Drawing.Point(12, 89);
            this.cmbReason5.Name = "cmbReason5";
            this.cmbReason5.Size = new System.Drawing.Size(228, 25);
            this.cmbReason5.TabIndex = 5;
            // 
            // lblChangeDimesion
            // 
            this.lblChangeDimesion.AutoSize = true;
            this.lblChangeDimesion.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblChangeDimesion.Location = new System.Drawing.Point(228, 192);
            this.lblChangeDimesion.Name = "lblChangeDimesion";
            this.lblChangeDimesion.Size = new System.Drawing.Size(4, 14);
            this.lblChangeDimesion.TabIndex = 0;
            this.lblChangeDimesion.Text = " ";
            // 
            // lblChangeDTitle
            // 
            this.lblChangeDTitle.BackColor = System.Drawing.Color.FromArgb(244, 244, 244);
            this.lblChangeDTitle.Dock = Wisej.Web.DockStyle.Top;
            this.lblChangeDTitle.ForeColor = System.Drawing.Color.FromName("@buttonFace");
            this.lblChangeDTitle.Location = new System.Drawing.Point(0, 0);
            this.lblChangeDTitle.Name = "lblChangeDTitle";
            this.lblChangeDTitle.Padding = new Wisej.Web.Padding(5);
            this.lblChangeDTitle.Size = new System.Drawing.Size(620, 21);
            this.lblChangeDTitle.TabIndex = 0;
            this.lblChangeDTitle.Text = "Reason for change(s) in Dimension";
            this.lblChangeDTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlMatrix
            // 
            this.pnlMatrix.Controls.Add(this.cmbScale);
            this.pnlMatrix.Controls.Add(this.dtAssessmentDate);
            this.pnlMatrix.Controls.Add(this.lblMatrix);
            this.pnlMatrix.Controls.Add(this.cmbMatrix);
            this.pnlMatrix.Controls.Add(this.lblScale);
            this.pnlMatrix.Controls.Add(this.lblAssessmentDate);
            this.pnlMatrix.Dock = Wisej.Web.DockStyle.Top;
            this.pnlMatrix.Location = new System.Drawing.Point(0, 0);
            this.pnlMatrix.Name = "pnlMatrix";
            this.pnlMatrix.Size = new System.Drawing.Size(620, 71);
            this.pnlMatrix.TabIndex = 37;
            // 
            // pnlgvwReasons
            // 
            this.pnlgvwReasons.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.pnlgvwReasons.Controls.Add(this.gvwReasons);
            this.pnlgvwReasons.Dock = Wisej.Web.DockStyle.Top;
            this.pnlgvwReasons.Location = new System.Drawing.Point(0, 71);
            this.pnlgvwReasons.Name = "pnlgvwReasons";
            this.pnlgvwReasons.Size = new System.Drawing.Size(620, 123);
            this.pnlgvwReasons.TabIndex = 38;
            // 
            // pnlReasons
            // 
            this.pnlReasons.Controls.Add(this.cmbReason6);
            this.pnlReasons.Controls.Add(this.cmbReason3);
            this.pnlReasons.Controls.Add(this.cmbReason1);
            this.pnlReasons.Controls.Add(this.lblChangeDTitle);
            this.pnlReasons.Controls.Add(this.cmbReason4);
            this.pnlReasons.Controls.Add(this.cmbReason2);
            this.pnlReasons.Controls.Add(this.cmbReason5);
            this.pnlReasons.Dock = Wisej.Web.DockStyle.Top;
            this.pnlReasons.Location = new System.Drawing.Point(0, 194);
            this.pnlReasons.Name = "pnlReasons";
            this.pnlReasons.Size = new System.Drawing.Size(620, 122);
            this.pnlReasons.TabIndex = 39;
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnSave);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 316);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(620, 35);
            this.pnlSave.TabIndex = 40;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(527, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // MAT00003ReasonCodes
            // 
            this.ClientSize = new System.Drawing.Size(620, 351);
            this.Controls.Add(this.pnlSave);
            this.Controls.Add(this.pnlReasons);
            this.Controls.Add(this.pnlgvwReasons);
            this.Controls.Add(this.pnlMatrix);
            this.Controls.Add(this.lblChangeDimesion);
            this.Controls.Add(this.lblViewBenchmark);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MAT00003ReasonCodes";
            this.Text = "MAT0003ReasonCodes";
            this.Load += new System.EventHandler(this.MAT00003ReasonCodes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvwReasons)).EndInit();
            this.pnlMatrix.ResumeLayout(false);
            this.pnlMatrix.PerformLayout();
            this.pnlgvwReasons.ResumeLayout(false);
            this.pnlReasons.ResumeLayout(false);
            this.pnlReasons.PerformLayout();
            this.pnlSave.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ContextMenu contextMenu1;
        private Button btnCancel;
        private DataGridViewTextBoxColumn CaseWorker;
        private DateTimePicker dtAssessmentDate;
        private Label lblMatrix;
        private ComboBox cmbMatrix;
        private ComboBox cmbScale;
        private Label lblScale;
        private DataGridViewEx gvwReasons;
        private Label lblAssessmentDate;
        private Label lblViewBenchmark;
        private Button btnSave;
        private ComboBox cmbReason3;
        private ComboBox cmbReason1;
        private ComboBox cmbReason4;
        private ComboBox cmbReason2;
        private ComboBox cmbReason6;
        private ComboBox cmbReason5;
        private Label lblChangeDimesion;
        private Label lblChangeDTitle;
        private DataGridViewTextBoxColumn Score;
        private Panel pnlMatrix;
        private Panel pnlgvwReasons;
        private Panel pnlReasons;
        private Panel pnlSave;
        private Spacer spacer1;
        private Controls.Compatibility.DataGridViewDateTimeColumn Date;
    }
}