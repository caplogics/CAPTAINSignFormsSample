using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class ReRun_Dates_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReRun_Dates_Form));
            this.listDates = new Wisej.Web.ListView();
            this.RanDates = new Wisej.Web.ColumnHeader();
            this.RanBy = new Wisej.Web.ColumnHeader();
            this.Seq = new Wisej.Web.ColumnHeader();
            this.AppCount = new Wisej.Web.ColumnHeader();
            this.btnSelect = new Wisej.Web.Button();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnldgvReRun = new Wisej.Web.Panel();
            this.dgvReRun = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.gvDteRan = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.gvRanBy = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlSelect = new Wisej.Web.Panel();
            this.gvNoApp = new Captain.Common.Views.Controls.Compatibility.DataGridViewNumberColumn();
            this.gvSeq = new Captain.Common.Views.Controls.Compatibility.DataGridViewNumberColumn();
            this.pnlCompleteForm.SuspendLayout();
            this.pnldgvReRun.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReRun)).BeginInit();
            this.dgvReRun.SuspendLayout();
            this.pnlSelect.SuspendLayout();
            this.SuspendLayout();
            // 
            // listDates
            // 
            this.listDates.Columns.AddRange(new Wisej.Web.ColumnHeader[] {
            this.RanDates,
            this.RanBy,
            this.Seq,
            this.AppCount});
            this.listDates.Location = new System.Drawing.Point(3, 260);
            this.listDates.Name = "listDates";
            this.listDates.Size = new System.Drawing.Size(427, 23);
            this.listDates.TabIndex = 0;
            this.listDates.View = Wisej.Web.View.Details;
            this.listDates.Visible = false;
            // 
            // RanDates
            // 
            this.RanDates.Name = "RanDates";
            this.RanDates.Text = "Date Ran";
            this.RanDates.Width = 100;
            // 
            // RanBy
            // 
            this.RanBy.Name = "RanBy";
            this.RanBy.Text = "Ran By";
            this.RanBy.Width = 135;
            // 
            // Seq
            // 
            this.Seq.Name = "Seq";
            this.Seq.Text = "Seq";
            this.Seq.Width = 90;
            // 
            // AppCount
            // 
            this.AppCount.Name = "AppCount";
            this.AppCount.Text = "No. of App#";
            this.AppCount.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.AppCount.Width = 85;
            // 
            // btnSelect
            // 
            this.btnSelect.Dock = Wisej.Web.DockStyle.Right;
            this.btnSelect.Location = new System.Drawing.Point(413, 5);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(60, 25);
            this.btnSelect.TabIndex = 3;
            this.btnSelect.Text = "&Select";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnldgvReRun);
            this.pnlCompleteForm.Controls.Add(this.pnlSelect);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(488, 322);
            this.pnlCompleteForm.TabIndex = 4;
            // 
            // pnldgvReRun
            // 
            this.pnldgvReRun.Controls.Add(this.dgvReRun);
            this.pnldgvReRun.Dock = Wisej.Web.DockStyle.Fill;
            this.pnldgvReRun.Location = new System.Drawing.Point(0, 0);
            this.pnldgvReRun.Name = "pnldgvReRun";
            this.pnldgvReRun.Size = new System.Drawing.Size(488, 287);
            this.pnldgvReRun.TabIndex = 5;
            // 
            // dgvReRun
            // 
            this.dgvReRun.AllowUserToResizeColumns = false;
            this.dgvReRun.AllowUserToResizeRows = false;
            this.dgvReRun.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvReRun.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.dgvReRun.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvDteRan,
            this.gvRanBy,
            this.gvSeq,
            this.gvNoApp});
            this.dgvReRun.Controls.Add(this.listDates);
            this.dgvReRun.Dock = Wisej.Web.DockStyle.Fill;
            this.dgvReRun.Name = "dgvReRun";
            this.dgvReRun.RowHeadersWidth = 15;
            this.dgvReRun.Size = new System.Drawing.Size(488, 287);
            this.dgvReRun.TabIndex = 0;
            // 
            // gvDteRan
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvDteRan.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvDteRan.HeaderStyle = dataGridViewCellStyle2;
            this.gvDteRan.HeaderText = "Date Ran";
            this.gvDteRan.Name = "gvDteRan";
            // 
            // gvRanBy
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvRanBy.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvRanBy.HeaderStyle = dataGridViewCellStyle4;
            this.gvRanBy.HeaderText = "Ran By";
            this.gvRanBy.Name = "gvRanBy";
            this.gvRanBy.Width = 150;
            // 
            // pnlSelect
            // 
            this.pnlSelect.AppearanceKey = "panel-grdo";
            this.pnlSelect.Controls.Add(this.btnSelect);
            this.pnlSelect.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSelect.Location = new System.Drawing.Point(0, 287);
            this.pnlSelect.Name = "pnlSelect";
            this.pnlSelect.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSelect.Size = new System.Drawing.Size(488, 35);
            this.pnlSelect.TabIndex = 6;
            // 
            // gvNoApp
            // 
            this.gvNoApp.HeaderText = "No. Of App#";
            this.gvNoApp.Name = "gvNoApp";
            // 
            // gvSeq
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            this.gvSeq.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.gvSeq.HeaderStyle = dataGridViewCellStyle6;
            this.gvSeq.HeaderText = "Sequence";
            this.gvSeq.Name = "gvSeq";
            // 
            // ReRun_Dates_Form
            // 
            this.ClientSize = new System.Drawing.Size(488, 322);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReRun_Dates_Form";
            this.Text = "ReRun_Dates_Form";
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnldgvReRun.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReRun)).EndInit();
            this.dgvReRun.ResumeLayout(false);
            this.pnlSelect.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ListView listDates;
        private ColumnHeader RanDates;
        private ColumnHeader RanBy;
        private ColumnHeader Seq;
        private ColumnHeader AppCount;
        private Button btnSelect;
        private Panel pnlCompleteForm;
        private Panel pnlSelect;
        private Panel pnldgvReRun;
        private DataGridViewEx dgvReRun;
        private Controls.Compatibility.DataGridViewDateTimeColumn gvDteRan;
        private DataGridViewTextBoxColumn gvRanBy;
        private DataGridViewNumberColumn gvSeq;
        private DataGridViewNumberColumn gvNoApp;
    }
}