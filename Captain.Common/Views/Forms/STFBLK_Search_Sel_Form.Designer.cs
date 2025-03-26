namespace Captain.Common.Views.Forms
{
    partial class STFBLK_Search_Sel_Form
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

        #region Wisej.NET Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(STFBLK_Search_Sel_Form));
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlGrdIncometypes = new Wisej.Web.Panel();
            this.dgvStaffBulkPost = new Wisej.Web.DataGridView();
            this.gvSelect = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvCode = new Wisej.Web.DataGridViewColumn();
            this.pnlOK = new Wisej.Web.Panel();
            this.btnOk = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnClose = new Wisej.Web.Button();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlGrdIncometypes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStaffBulkPost)).BeginInit();
            this.pnlOK.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlGrdIncometypes);
            this.pnlCompleteForm.Controls.Add(this.pnlOK);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(432, 341);
            this.pnlCompleteForm.TabIndex = 1;
            // 
            // pnlGrdIncometypes
            // 
            this.pnlGrdIncometypes.Controls.Add(this.dgvStaffBulkPost);
            this.pnlGrdIncometypes.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlGrdIncometypes.Location = new System.Drawing.Point(0, 0);
            this.pnlGrdIncometypes.Name = "pnlGrdIncometypes";
            this.pnlGrdIncometypes.Size = new System.Drawing.Size(432, 306);
            this.pnlGrdIncometypes.TabIndex = 0;
            // 
            // dgvStaffBulkPost
            // 
            this.dgvStaffBulkPost.AllowUserToResizeColumns = false;
            this.dgvStaffBulkPost.AllowUserToResizeRows = false;
            this.dgvStaffBulkPost.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvStaffBulkPost.BackColor = System.Drawing.Color.White;
            this.dgvStaffBulkPost.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.dgvStaffBulkPost.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvStaffBulkPost.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvSelect,
            this.gvDesc,
            this.gvCode});
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dgvStaffBulkPost.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvStaffBulkPost.Dock = Wisej.Web.DockStyle.Fill;
            this.dgvStaffBulkPost.Location = new System.Drawing.Point(0, 0);
            this.dgvStaffBulkPost.MultiSelect = false;
            this.dgvStaffBulkPost.Name = "dgvStaffBulkPost";
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dgvStaffBulkPost.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvStaffBulkPost.RowHeadersVisible = false;
            this.dgvStaffBulkPost.RowHeadersWidth = 15;
            this.dgvStaffBulkPost.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvStaffBulkPost.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.dgvStaffBulkPost.Size = new System.Drawing.Size(432, 306);
            this.dgvStaffBulkPost.TabIndex = 0;
            this.dgvStaffBulkPost.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.dgvStaffBulkPost_CellClick);
            // 
            // gvSelect
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.NullValue = false;
            this.gvSelect.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvSelect.HeaderStyle = dataGridViewCellStyle3;
            this.gvSelect.HeaderText = "  ";
            this.gvSelect.Name = "gvSelect";
            this.gvSelect.ShowInVisibilityMenu = false;
            this.gvSelect.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.gvSelect.Width = 33;
            // 
            // gvDesc
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvDesc.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvDesc.HeaderStyle = dataGridViewCellStyle5;
            this.gvDesc.HeaderText = "Description";
            this.gvDesc.Name = "gvDesc";
            this.gvDesc.ReadOnly = true;
            this.gvDesc.Resizable = Wisej.Web.DataGridViewTriState.True;
            this.gvDesc.Width = 235;
            // 
            // gvCode
            // 
            this.gvCode.HeaderText = "Code";
            this.gvCode.Name = "gvCode";
            this.gvCode.ShowInVisibilityMenu = false;
            this.gvCode.Visible = false;
            // 
            // pnlOK
            // 
            this.pnlOK.AppearanceKey = "panel-grdo";
            this.pnlOK.Controls.Add(this.btnOk);
            this.pnlOK.Controls.Add(this.spacer1);
            this.pnlOK.Controls.Add(this.btnClose);
            this.pnlOK.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlOK.Location = new System.Drawing.Point(0, 306);
            this.pnlOK.Name = "pnlOK";
            this.pnlOK.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlOK.Size = new System.Drawing.Size(432, 35);
            this.pnlOK.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnOk.Location = new System.Drawing.Point(279, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(60, 25);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "&OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(339, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnClose
            // 
            this.btnClose.AppearanceKey = "button-error";
            this.btnClose.Dock = Wisej.Web.DockStyle.Right;
            this.btnClose.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnClose.Location = new System.Drawing.Point(342, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 25);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // STFBLK_Search_Sel_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 341);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "STFBLK_Search_Sel_Form";
            this.Text = "Staff Bulk Post Search Form";
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlGrdIncometypes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStaffBulkPost)).EndInit();
            this.pnlOK.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Panel pnlCompleteForm;
        private Wisej.Web.Panel pnlGrdIncometypes;
        private Wisej.Web.DataGridView dgvStaffBulkPost;
        private Wisej.Web.DataGridViewCheckBoxColumn gvSelect;
        private Wisej.Web.DataGridViewTextBoxColumn gvDesc;
        private Wisej.Web.Panel pnlOK;
        private Wisej.Web.Button btnOk;
        private Wisej.Web.Spacer spacer1;
        private Wisej.Web.Button btnClose;
        private Wisej.Web.DataGridViewColumn gvCode;
    }
}