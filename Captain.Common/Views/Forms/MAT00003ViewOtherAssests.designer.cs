using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class MAT00003ViewOtherAssests
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle10 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle11 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MAT00003ViewOtherAssests));
            this.btnSelect = new Wisej.Web.Button();
            this.dateTimePicker2 = new Wisej.Web.DateTimePicker();
            this.lblHeader = new Wisej.Web.Label();
            this.panel7 = new Wisej.Web.Panel();
            this.btnSSNSelect = new Wisej.Web.Button();
            this.gvwAssessts = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.Hierarchy = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Scale = new Wisej.Web.DataGridViewTextBoxColumn();
            this.BenchMark = new Wisej.Web.DataGridViewTextBoxColumn();
            this.ColScore = new Captain.Common.Views.Controls.Compatibility.DataGridViewNumberColumn();
            this.AssessmentDate = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.pnlSelect = new Wisej.Web.Panel();
            this.pnlgvwAssessts = new Wisej.Web.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gvwAssessts)).BeginInit();
            this.pnlSelect.SuspendLayout();
            this.pnlgvwAssessts.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(389, 368);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 7;
            this.btnSelect.Text = "&Select";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(417, -219);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(79, 21);
            this.dateTimePicker2.TabIndex = 15;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(66, 14);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(35, 13);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "View Other Assessments";
            // 
            // panel7
            // 
            this.panel7.Location = new System.Drawing.Point(124, -249);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(415, 64);
            this.panel7.TabIndex = 0;
            // 
            // btnSSNSelect
            // 
            this.btnSSNSelect.Dock = Wisej.Web.DockStyle.Right;
            this.btnSSNSelect.Enabled = false;
            this.btnSSNSelect.Location = new System.Drawing.Point(738, 5);
            this.btnSSNSelect.Name = "btnSSNSelect";
            this.btnSSNSelect.Size = new System.Drawing.Size(71, 25);
            this.btnSSNSelect.TabIndex = 4;
            this.btnSSNSelect.Text = "&Select";
            this.btnSSNSelect.Visible = false;
            this.btnSSNSelect.Click += new System.EventHandler(this.btnSSNSelect_Click);
            // 
            // gvwAssessts
            // 
            this.gvwAssessts.AllowUserToResizeColumns = false;
            this.gvwAssessts.AllowUserToResizeRows = false;
            this.gvwAssessts.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwAssessts.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwAssessts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwAssessts.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwAssessts.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Hierarchy,
            this.Scale,
            this.BenchMark,
            this.ColScore,
            this.AssessmentDate});
            this.gvwAssessts.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwAssessts.MultiSelect = false;
            this.gvwAssessts.Name = "gvwAssessts";
            this.gvwAssessts.ReadOnly = true;
            this.gvwAssessts.RowHeadersWidth = 14;
            this.gvwAssessts.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwAssessts.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvwAssessts.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwAssessts.Size = new System.Drawing.Size(824, 262);
            this.gvwAssessts.TabIndex = 0;
            // 
            // Hierarchy
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Hierarchy.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Hierarchy.HeaderStyle = dataGridViewCellStyle3;
            this.Hierarchy.HeaderText = "Hierarchy/Year/App#";
            this.Hierarchy.Name = "Hierarchy";
            this.Hierarchy.ReadOnly = true;
            this.Hierarchy.Width = 200;
            // 
            // Scale
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Scale.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Scale.HeaderStyle = dataGridViewCellStyle5;
            this.Scale.HeaderText = "Scale";
            this.Scale.Name = "Scale";
            this.Scale.ReadOnly = true;
            this.Scale.Width = 250;
            // 
            // BenchMark
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.BenchMark.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.BenchMark.HeaderStyle = dataGridViewCellStyle7;
            this.BenchMark.HeaderText = "Benchmark";
            this.BenchMark.Name = "BenchMark";
            this.BenchMark.ReadOnly = true;
            this.BenchMark.Width = 145;
            // 
            // ColScore
            // 
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.ColScore.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.ColScore.HeaderStyle = dataGridViewCellStyle9;
            this.ColScore.HeaderText = "Score";
            this.ColScore.Name = "ColScore";
            this.ColScore.ReadOnly = true;
            this.ColScore.Width = 70;
            // 
            // AssessmentDate
            // 
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.AssessmentDate.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.AssessmentDate.HeaderStyle = dataGridViewCellStyle11;
            this.AssessmentDate.HeaderText = "Assessment Date";
            this.AssessmentDate.Name = "AssessmentDate";
            this.AssessmentDate.ReadOnly = true;
            this.AssessmentDate.Width = 126;
            // 
            // pnlSelect
            // 
            this.pnlSelect.AppearanceKey = "panel-grdo";
            this.pnlSelect.Controls.Add(this.btnSSNSelect);
            this.pnlSelect.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSelect.Location = new System.Drawing.Point(0, 262);
            this.pnlSelect.Name = "pnlSelect";
            this.pnlSelect.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSelect.Size = new System.Drawing.Size(824, 35);
            this.pnlSelect.TabIndex = 0;
            this.pnlSelect.Text = "`";
            // 
            // pnlgvwAssessts
            // 
            this.pnlgvwAssessts.Controls.Add(this.gvwAssessts);
            this.pnlgvwAssessts.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwAssessts.Location = new System.Drawing.Point(0, 0);
            this.pnlgvwAssessts.Name = "pnlgvwAssessts";
            this.pnlgvwAssessts.Size = new System.Drawing.Size(824, 262);
            this.pnlgvwAssessts.TabIndex = 1;
            // 
            // MAT00003ViewOtherAssests
            // 
            this.ClientSize = new System.Drawing.Size(824, 297);
            this.Controls.Add(this.pnlgvwAssessts);
            this.Controls.Add(this.pnlSelect);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MAT00003ViewOtherAssests";
            this.Text = "MAT00003ViewOtherAssests";
            ((System.ComponentModel.ISupportInitialize)(this.gvwAssessts)).EndInit();
            this.pnlSelect.ResumeLayout(false);
            this.pnlgvwAssessts.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnSelect;
        private DateTimePicker dateTimePicker2;
        private Label lblHeader;
        private Panel panel7;
        private Button btnSSNSelect;
        private DataGridViewEx gvwAssessts;
        private DataGridViewTextBoxColumn Hierarchy;
        private DataGridViewTextBoxColumn Scale;
        private DataGridViewTextBoxColumn BenchMark;
        private Panel pnlSelect;
        private Panel pnlgvwAssessts;
        private Controls.Compatibility.DataGridViewDateTimeColumn AssessmentDate;
        private Controls.Compatibility.DataGridViewNumberColumn ColScore;
    }
}