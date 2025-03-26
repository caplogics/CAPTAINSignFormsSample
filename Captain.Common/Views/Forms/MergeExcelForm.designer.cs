using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class MergeExcelForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MergeExcelForm));
            this.gvExcelMerge = new Wisej.Web.DataGridView();
            this.gvtName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtSeq = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtFileName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.btnMerge = new Wisej.Web.Button();
            this.btnClose = new Wisej.Web.Button();
            this.pnlCompForm = new Wisej.Web.Panel();
            this.pnlgvExcelMerge = new Wisej.Web.Panel();
            this.panel1 = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            ((System.ComponentModel.ISupportInitialize)(this.gvExcelMerge)).BeginInit();
            this.pnlCompForm.SuspendLayout();
            this.pnlgvExcelMerge.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvExcelMerge
            // 
            this.gvExcelMerge.AllowDrop = true;
            this.gvExcelMerge.AllowUserToResizeColumns = false;
            this.gvExcelMerge.AllowUserToResizeRows = false;
            this.gvExcelMerge.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvExcelMerge.BorderStyle = Wisej.Web.BorderStyle.None;
            this.gvExcelMerge.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtName,
            this.gvtSeq,
            this.gvtFileName});
            this.gvExcelMerge.Dock = Wisej.Web.DockStyle.Fill;
            this.gvExcelMerge.Location = new System.Drawing.Point(0, 0);
            this.gvExcelMerge.Name = "gvExcelMerge";
            this.gvExcelMerge.RowHeadersWidth = 14;
            this.gvExcelMerge.Size = new System.Drawing.Size(504, 261);
            this.gvExcelMerge.TabIndex = 0;
            // 
            // gvtName
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtName.HeaderStyle = dataGridViewCellStyle1;
            this.gvtName.HeaderText = "Name";
            this.gvtName.Name = "gvtName";
            this.gvtName.ReadOnly = true;
            this.gvtName.Width = 280;
            // 
            // gvtSeq
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtSeq.HeaderStyle = dataGridViewCellStyle2;
            this.gvtSeq.HeaderText = "Order";
            this.gvtSeq.Name = "gvtSeq";
            this.gvtSeq.Width = 50;
            // 
            // gvtFileName
            // 
            this.gvtFileName.HeaderText = "gvtFileName";
            this.gvtFileName.Name = "gvtFileName";
            this.gvtFileName.ShowInVisibilityMenu = false;
            this.gvtFileName.Visible = false;
            // 
            // btnMerge
            // 
            this.btnMerge.Dock = Wisej.Web.DockStyle.Right;
            this.btnMerge.Location = new System.Drawing.Point(326, 5);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(85, 25);
            this.btnMerge.TabIndex = 1;
            this.btnMerge.Text = "&Merge Files";
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // btnClose
            // 
            this.btnClose.Dock = Wisej.Web.DockStyle.Right;
            this.btnClose.Location = new System.Drawing.Point(414, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 25);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlCompForm
            // 
            this.pnlCompForm.Controls.Add(this.pnlgvExcelMerge);
            this.pnlCompForm.Controls.Add(this.panel1);
            this.pnlCompForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompForm.Name = "pnlCompForm";
            this.pnlCompForm.Size = new System.Drawing.Size(504, 296);
            this.pnlCompForm.TabIndex = 2;
            // 
            // pnlgvExcelMerge
            // 
            this.pnlgvExcelMerge.Controls.Add(this.gvExcelMerge);
            this.pnlgvExcelMerge.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvExcelMerge.Location = new System.Drawing.Point(0, 0);
            this.pnlgvExcelMerge.Name = "pnlgvExcelMerge";
            this.pnlgvExcelMerge.Size = new System.Drawing.Size(504, 261);
            this.pnlgvExcelMerge.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.AppearanceKey = "panel-grdo";
            this.panel1.Controls.Add(this.btnMerge);
            this.panel1.Controls.Add(this.spacer1);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 261);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.panel1.Size = new System.Drawing.Size(504, 35);
            this.panel1.TabIndex = 1;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(411, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // MergeExcelForm
            // 
            this.ClientSize = new System.Drawing.Size(504, 296);
            this.Controls.Add(this.pnlCompForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MergeExcelForm";
            this.Text = "MergeExcelForm";
            ((System.ComponentModel.ISupportInitialize)(this.gvExcelMerge)).EndInit();
            this.pnlCompForm.ResumeLayout(false);
            this.pnlgvExcelMerge.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView gvExcelMerge;
        private DataGridViewTextBoxColumn gvtName;
        private DataGridViewTextBoxColumn gvtSeq;
        private Button btnMerge;
        private DataGridViewTextBoxColumn gvtFileName;
        private Button btnClose;
        private Panel pnlCompForm;
        private Panel pnlgvExcelMerge;
        private Panel panel1;
        private Spacer spacer1;
    }
}