namespace Captain.Common.Views.Forms
{
    partial class MasterPovertyCopyFrom
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterPovertyCopyFrom));
            this.pnlMain = new Wisej.Web.Panel();
            this.pnldgvCounty = new Wisej.Web.Panel();
            this.dgvCounty = new Wisej.Web.DataGridView();
            this.gvSelect = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.pnlOK = new Wisej.Web.Panel();
            this.btnOK = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.gvCounty = new Wisej.Web.DataGridViewColumn();
            this.gvStartDte = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.gvEndDte = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.gvPresentCounty = new Wisej.Web.DataGridViewColumn();
            this.gvSelCounty = new Wisej.Web.DataGridViewColumn();
            this.gvAgency = new Wisej.Web.DataGridViewColumn();
            this.gvDept = new Wisej.Web.DataGridViewColumn();
            this.gvProg = new Wisej.Web.DataGridViewColumn();
            this.pnlMain.SuspendLayout();
            this.pnldgvCounty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCounty)).BeginInit();
            this.pnlOK.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnldgvCounty);
            this.pnlMain.Controls.Add(this.pnlOK);
            this.pnlMain.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(454, 452);
            this.pnlMain.TabIndex = 0;
            // 
            // pnldgvCounty
            // 
            this.pnldgvCounty.Controls.Add(this.dgvCounty);
            this.pnldgvCounty.Dock = Wisej.Web.DockStyle.Fill;
            this.pnldgvCounty.Location = new System.Drawing.Point(0, 0);
            this.pnldgvCounty.Name = "pnldgvCounty";
            this.pnldgvCounty.Size = new System.Drawing.Size(454, 417);
            this.pnldgvCounty.TabIndex = 0;
            // 
            // dgvCounty
            // 
            this.dgvCounty.AllowUserToResizeColumns = false;
            this.dgvCounty.AllowUserToResizeRows = false;
            this.dgvCounty.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvCounty.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.dgvCounty.ColumnHeadersHeight = 25;
            this.dgvCounty.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvSelect,
            this.gvCounty,
            this.gvStartDte,
            this.gvEndDte,
            this.gvPresentCounty,
            this.gvSelCounty,
            this.gvAgency,
            this.gvDept,
            this.gvProg});
            this.dgvCounty.Dock = Wisej.Web.DockStyle.Fill;
            this.dgvCounty.Location = new System.Drawing.Point(0, 0);
            this.dgvCounty.Name = "dgvCounty";
            this.dgvCounty.RowHeadersWidth = 10;
            this.dgvCounty.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvCounty.Size = new System.Drawing.Size(454, 417);
            this.dgvCounty.TabIndex = 0;
            this.dgvCounty.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.dgvCounty_CellClick);
            // 
            // gvSelect
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = false;
            this.gvSelect.DefaultCellStyle = dataGridViewCellStyle1;
            this.gvSelect.HeaderText = "Select";
            this.gvSelect.Name = "gvSelect";
            // 
            // pnlOK
            // 
            this.pnlOK.AppearanceKey = "panel-grdo";
            this.pnlOK.Controls.Add(this.btnOK);
            this.pnlOK.Controls.Add(this.spacer1);
            this.pnlOK.Controls.Add(this.btnCancel);
            this.pnlOK.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlOK.Location = new System.Drawing.Point(0, 417);
            this.pnlOK.Name = "pnlOK";
            this.pnlOK.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlOK.Size = new System.Drawing.Size(454, 35);
            this.pnlOK.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.AppearanceKey = "button-ok";
            this.btnOK.Dock = Wisej.Web.DockStyle.Right;
            this.btnOK.Location = new System.Drawing.Point(301, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(60, 25);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "O&K";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(361, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(364, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gvCounty
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvCounty.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvCounty.HeaderStyle = dataGridViewCellStyle3;
            this.gvCounty.HeaderText = "County";
            this.gvCounty.Name = "gvCounty";
            // 
            // gvStartDte
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvStartDte.HeaderStyle = dataGridViewCellStyle4;
            this.gvStartDte.HeaderText = "Start Date";
            this.gvStartDte.Name = "gvStartDte";
            this.gvStartDte.ReadOnly = true;
            // 
            // gvEndDte
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvEndDte.HeaderStyle = dataGridViewCellStyle5;
            this.gvEndDte.HeaderText = "End Date";
            this.gvEndDte.Name = "gvEndDte";
            this.gvEndDte.ReadOnly = true;
            // 
            // gvPresentCounty
            // 
            this.gvPresentCounty.HeaderText = "Present County";
            this.gvPresentCounty.Name = "gvPresentCounty";
            this.gvPresentCounty.ShowInVisibilityMenu = false;
            this.gvPresentCounty.Visible = false;
            // 
            // gvSelCounty
            // 
            this.gvSelCounty.HeaderText = "County";
            this.gvSelCounty.Name = "gvSelCounty";
            this.gvSelCounty.ShowInVisibilityMenu = false;
            this.gvSelCounty.Visible = false;
            // 
            // gvAgency
            // 
            this.gvAgency.HeaderText = "Agency";
            this.gvAgency.Name = "gvAgency";
            this.gvAgency.ShowInVisibilityMenu = false;
            this.gvAgency.Visible = false;
            // 
            // gvDept
            // 
            this.gvDept.HeaderText = "Dept";
            this.gvDept.Name = "gvDept";
            this.gvDept.ShowInVisibilityMenu = false;
            this.gvDept.Visible = false;
            // 
            // gvProg
            // 
            this.gvProg.HeaderText = "Program";
            this.gvProg.Name = "gvProg";
            this.gvProg.ShowInVisibilityMenu = false;
            this.gvProg.Visible = false;
            // 
            // MasterPovertyCopyFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 452);
            this.Controls.Add(this.pnlMain);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MasterPovertyCopyFrom";
            this.Text = "Copy From";
            this.pnlMain.ResumeLayout(false);
            this.pnldgvCounty.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCounty)).EndInit();
            this.pnlOK.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Panel pnlMain;
        private Wisej.Web.Panel pnldgvCounty;
        private Wisej.Web.DataGridView dgvCounty;
        private Wisej.Web.Panel pnlOK;
        private Wisej.Web.Button btnCancel;
        private Wisej.Web.Button btnOK;
        private Wisej.Web.DataGridViewCheckBoxColumn gvSelect;
        private Wisej.Web.DataGridViewColumn gvCounty;
        private Controls.Compatibility.DataGridViewDateTimeColumn gvStartDte;
        private Controls.Compatibility.DataGridViewDateTimeColumn gvEndDte;
        private Wisej.Web.DataGridViewColumn gvPresentCounty;
        private Wisej.Web.Spacer spacer1;
        private Wisej.Web.DataGridViewColumn gvSelCounty;
        private Wisej.Web.DataGridViewColumn gvAgency;
        private Wisej.Web.DataGridViewColumn gvDept;
        private Wisej.Web.DataGridViewColumn gvProg;
    }
}