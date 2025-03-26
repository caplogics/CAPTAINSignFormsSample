namespace Captain.Common.Views.Forms
{
    partial class Paid_Bundle_Sel_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Paid_Bundle_Sel_Form));
            this.pnlForm = new Wisej.Web.Panel();
            this.pnldgvBundles = new Wisej.Web.Panel();
            this.dgvBundles = new Wisej.Web.DataGridView();
            this.gvSelect = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvBundleNo = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvCode = new Wisej.Web.DataGridViewColumn();
            this.pnlOK = new Wisej.Web.Panel();
            this.btnOK = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.pnlForm.SuspendLayout();
            this.pnldgvBundles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBundles)).BeginInit();
            this.pnlOK.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlForm
            // 
            this.pnlForm.Controls.Add(this.pnldgvBundles);
            this.pnlForm.Controls.Add(this.pnlOK);
            this.pnlForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlForm.Location = new System.Drawing.Point(0, 0);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(418, 303);
            this.pnlForm.TabIndex = 0;
            // 
            // pnldgvBundles
            // 
            this.pnldgvBundles.Controls.Add(this.dgvBundles);
            this.pnldgvBundles.Dock = Wisej.Web.DockStyle.Fill;
            this.pnldgvBundles.Location = new System.Drawing.Point(0, 0);
            this.pnldgvBundles.Name = "pnldgvBundles";
            this.pnldgvBundles.Size = new System.Drawing.Size(418, 268);
            this.pnldgvBundles.TabIndex = 1;
            // 
            // dgvBundles
            // 
            this.dgvBundles.AllowUserToResizeColumns = false;
            this.dgvBundles.AllowUserToResizeRows = false;
            this.dgvBundles.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvBundles.BackColor = System.Drawing.Color.White;
            this.dgvBundles.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.dgvBundles.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBundles.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvSelect,
            this.gvBundleNo,
            this.gvCode});
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dgvBundles.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvBundles.Dock = Wisej.Web.DockStyle.Fill;
            this.dgvBundles.Location = new System.Drawing.Point(0, 0);
            this.dgvBundles.MultiSelect = false;
            this.dgvBundles.Name = "dgvBundles";
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dgvBundles.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvBundles.RowHeadersVisible = false;
            this.dgvBundles.RowHeadersWidth = 15;
            this.dgvBundles.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvBundles.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.dgvBundles.Size = new System.Drawing.Size(418, 268);
            this.dgvBundles.TabIndex = 0;
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
            // gvBundleNo
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvBundleNo.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvBundleNo.HeaderStyle = dataGridViewCellStyle5;
            this.gvBundleNo.HeaderText = "Bundle Number";
            this.gvBundleNo.Name = "gvBundleNo";
            this.gvBundleNo.ReadOnly = true;
            this.gvBundleNo.Resizable = Wisej.Web.DataGridViewTriState.True;
            this.gvBundleNo.Width = 250;
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
            this.pnlOK.Controls.Add(this.btnOK);
            this.pnlOK.Controls.Add(this.spacer1);
            this.pnlOK.Controls.Add(this.btnCancel);
            this.pnlOK.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlOK.Location = new System.Drawing.Point(0, 268);
            this.pnlOK.Name = "pnlOK";
            this.pnlOK.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlOK.Size = new System.Drawing.Size(418, 35);
            this.pnlOK.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.AppearanceKey = "button-ok";
            this.btnOK.Dock = Wisej.Web.DockStyle.Right;
            this.btnOK.Location = new System.Drawing.Point(290, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(50, 25);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(340, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(343, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 25);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // Paid_Bundle_Sel_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 303);
            this.Controls.Add(this.pnlForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Paid_Bundle_Sel_Form";
            this.Text = "Select Bundle(s)";
            this.pnlForm.ResumeLayout(false);
            this.pnldgvBundles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBundles)).EndInit();
            this.pnlOK.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Panel pnlForm;
        private Wisej.Web.Panel pnlOK;
        private Wisej.Web.Button btnOK;
        private Wisej.Web.Spacer spacer1;
        private Wisej.Web.Button btnCancel;
        private Wisej.Web.Panel pnldgvBundles;
        private Wisej.Web.DataGridView dgvBundles;
        private Wisej.Web.DataGridViewCheckBoxColumn gvSelect;
        private Wisej.Web.DataGridViewTextBoxColumn gvBundleNo;
        private Wisej.Web.DataGridViewColumn gvCode;
    }
}