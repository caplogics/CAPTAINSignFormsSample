namespace Captain.Common.Views.Forms
{
    partial class AdminHierarchySelection
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

        #region Wisej Designer generated code

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminHierarchySelection));
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlgvwHierarchie = new Wisej.Web.Panel();
            this.gvwHierarchie = new Wisej.Web.DataGridView();
            this.Select = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Desc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1 = new Wisej.Web.Panel();
            this.btnOk = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlgvwHierarchie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwHierarchie)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlgvwHierarchie);
            this.pnlCompleteForm.Controls.Add(this.flowLayoutPanel1);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(480, 373);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlgvwHierarchie
            // 
            this.pnlgvwHierarchie.Controls.Add(this.gvwHierarchie);
            this.pnlgvwHierarchie.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwHierarchie.Location = new System.Drawing.Point(0, 0);
            this.pnlgvwHierarchie.Name = "pnlgvwHierarchie";
            this.pnlgvwHierarchie.Padding = new Wisej.Web.Padding(5);
            this.pnlgvwHierarchie.Size = new System.Drawing.Size(480, 338);
            this.pnlgvwHierarchie.TabIndex = 11;
            // 
            // gvwHierarchie
            // 
            this.gvwHierarchie.AllowUserToResizeColumns = false;
            this.gvwHierarchie.AllowUserToResizeRows = false;
            this.gvwHierarchie.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwHierarchie.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvwHierarchie.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwHierarchie.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwHierarchie.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Select,
            this.Code,
            this.Desc});
            this.gvwHierarchie.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwHierarchie.Location = new System.Drawing.Point(5, 5);
            this.gvwHierarchie.Margin = new Wisej.Web.Padding(3, 0, 3, 3);
            this.gvwHierarchie.MultiSelect = false;
            this.gvwHierarchie.Name = "gvwHierarchie";
            this.gvwHierarchie.RowHeadersVisible = false;
            this.gvwHierarchie.RowHeadersWidth = 15;
            this.gvwHierarchie.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwHierarchie.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwHierarchie.Size = new System.Drawing.Size(470, 328);
            this.gvwHierarchie.TabIndex = 7;
            this.gvwHierarchie.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwHierarchie_CellClick);
            // 
            // Select
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.NullValue = false;
            this.Select.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Select.HeaderStyle = dataGridViewCellStyle3;
            this.Select.HeaderText = "Select";
            this.Select.Name = "Select";
            this.Select.ShowInVisibilityMenu = false;
            this.Select.Width = 70;
            // 
            // Code
            // 
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            this.Code.Resizable = Wisej.Web.DataGridViewTriState.True;
            this.Code.Width = 80;
            // 
            // Desc
            // 
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Desc.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Desc.HeaderStyle = dataGridViewCellStyle5;
            this.Desc.HeaderText = "Agency Name";
            this.Desc.Name = "Desc";
            this.Desc.ReadOnly = true;
            this.Desc.ShowInVisibilityMenu = false;
            this.Desc.Width = 305;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AppearanceKey = "panel-grdo";
            this.flowLayoutPanel1.Controls.Add(this.btnOk);
            this.flowLayoutPanel1.Controls.Add(this.spacer1);
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 338);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new Wisej.Web.Padding(3, 5, 15, 5);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(480, 35);
            this.flowLayoutPanel1.TabIndex = 10;
            this.flowLayoutPanel1.TabStop = true;
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnOk.Location = new System.Drawing.Point(327, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(60, 25);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "&OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(387, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnCancel.Location = new System.Drawing.Point(390, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AdminHierarchySelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 373);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdminHierarchySelection";
            this.Text = "Agency Selection";
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlgvwHierarchie.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwHierarchie)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Panel pnlCompleteForm;
        private Wisej.Web.Panel flowLayoutPanel1;
        private Wisej.Web.Button btnOk;
        private Wisej.Web.Spacer spacer1;
        private Wisej.Web.Button btnCancel;
        private Wisej.Web.Panel pnlgvwHierarchie;
        private Wisej.Web.DataGridView gvwHierarchie;
        private Wisej.Web.DataGridViewCheckBoxColumn Select;
        private Wisej.Web.DataGridViewTextBoxColumn Code;
        private Wisej.Web.DataGridViewTextBoxColumn Desc;
    }
}