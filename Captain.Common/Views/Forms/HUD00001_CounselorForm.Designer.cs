namespace Captain.Common.Views.Forms
{
    partial class HUD00001_CounselorForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HUD00001_CounselorForm));
            this.pnlForm = new Wisej.Web.Panel();
            this.pnldgvCounselors = new Wisej.Web.Panel();
            this.dgvCounselors = new Wisej.Web.DataGridView();
            this.Column0 = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.Column1 = new Wisej.Web.DataGridViewColumn();
            this.Column2 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Column3 = new Wisej.Web.DataGridViewColumn();
            this.pnlOK = new Wisej.Web.Panel();
            this.btnOK = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.pnlForm.SuspendLayout();
            this.pnldgvCounselors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCounselors)).BeginInit();
            this.pnlOK.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlForm
            // 
            this.pnlForm.Controls.Add(this.pnldgvCounselors);
            this.pnlForm.Controls.Add(this.pnlOK);
            this.pnlForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlForm.Location = new System.Drawing.Point(0, 0);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(467, 386);
            this.pnlForm.TabIndex = 0;
            // 
            // pnldgvCounselors
            // 
            this.pnldgvCounselors.Controls.Add(this.dgvCounselors);
            this.pnldgvCounselors.Dock = Wisej.Web.DockStyle.Fill;
            this.pnldgvCounselors.Location = new System.Drawing.Point(0, 0);
            this.pnldgvCounselors.Name = "pnldgvCounselors";
            this.pnldgvCounselors.Padding = new Wisej.Web.Padding(5);
            this.pnldgvCounselors.Size = new System.Drawing.Size(467, 351);
            this.pnldgvCounselors.TabIndex = 0;
            // 
            // dgvCounselors
            // 
            this.dgvCounselors.AllowUserToResizeColumns = false;
            this.dgvCounselors.AllowUserToResizeRows = false;
            this.dgvCounselors.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvCounselors.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.dgvCounselors.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Column0,
            this.Column1,
            this.Column2,
            this.Column3});
            this.dgvCounselors.CssStyle = "border-radius:8px; border:1px solid #ececec; ";
            this.dgvCounselors.Dock = Wisej.Web.DockStyle.Fill;
            this.dgvCounselors.EditMode = Wisej.Web.DataGridViewEditMode.EditOnEnter;
            this.dgvCounselors.Location = new System.Drawing.Point(5, 5);
            this.dgvCounselors.Name = "dgvCounselors";
            this.dgvCounselors.RowHeadersWidth = 10;
            this.dgvCounselors.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvCounselors.Size = new System.Drawing.Size(457, 341);
            this.dgvCounselors.TabIndex = 0;
            this.dgvCounselors.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.dgvCounselors_CellClick);
            // 
            // Column0
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = false;
            this.Column0.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.Column0.HeaderStyle = dataGridViewCellStyle2;
            this.Column0.HeaderText = "Select";
            this.Column0.Name = "Column0";
            this.Column0.Width = 60;
            // 
            // Column1
            // 
            dataGridViewCellStyle3.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Column1.HeaderStyle = dataGridViewCellStyle4;
            this.Column1.HeaderText = "Counselor Name";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 150;
            // 
            // Column2
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Column2.HeaderStyle = dataGridViewCellStyle6;
            this.Column2.HeaderText = "Certificate Received";
            this.Column2.Name = "Column2";
            this.Column2.Width = 210;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Staff Code";
            this.Column3.Name = "Column3";
            this.Column3.ShowInVisibilityMenu = false;
            this.Column3.Visible = false;
            // 
            // pnlOK
            // 
            this.pnlOK.AppearanceKey = "panel-grdo";
            this.pnlOK.Controls.Add(this.btnOK);
            this.pnlOK.Controls.Add(this.spacer1);
            this.pnlOK.Controls.Add(this.btnCancel);
            this.pnlOK.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlOK.Location = new System.Drawing.Point(0, 351);
            this.pnlOK.Name = "pnlOK";
            this.pnlOK.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlOK.Size = new System.Drawing.Size(467, 35);
            this.pnlOK.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.AppearanceKey = "button-ok";
            this.btnOK.Dock = Wisej.Web.DockStyle.Right;
            this.btnOK.Location = new System.Drawing.Point(314, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(60, 25);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "O&K";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(374, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(377, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // HUD00001_CounselorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 386);
            this.Controls.Add(this.pnlForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HUD00001_CounselorForm";
            this.Text = "Select Counselor(s) Attending Training Session";
            this.pnlForm.ResumeLayout(false);
            this.pnldgvCounselors.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCounselors)).EndInit();
            this.pnlOK.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Panel pnlForm;
        private Wisej.Web.Panel pnldgvCounselors;
        private Wisej.Web.DataGridView dgvCounselors;
        private Wisej.Web.Panel pnlOK;
        private Wisej.Web.Button btnCancel;
        private Wisej.Web.Button btnOK;
        private Wisej.Web.Spacer spacer1;
        private Wisej.Web.DataGridViewCheckBoxColumn Column0;
        private Wisej.Web.DataGridViewColumn Column1;
        private Wisej.Web.DataGridViewTextBoxColumn Column2;
        private Wisej.Web.DataGridViewColumn Column3;
    }
}