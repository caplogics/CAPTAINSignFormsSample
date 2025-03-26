namespace Captain.Common.Views
{
    partial class SPTargetHierarchyTarget
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPTargetHierarchyTarget));
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlOK = new Wisej.Web.Panel();
            this.btnOK = new Wisej.Web.Button();
            this.btnCancel = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.dgvHie = new Wisej.Web.DataGridView();
            this.Hie_Select = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.Hie_Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Hie_Name = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlOK.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHie)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.dgvHie);
            this.pnlCompleteForm.Controls.Add(this.pnlOK);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(508, 391);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlOK
            // 
            this.pnlOK.AppearanceKey = "panel-grdo";
            this.pnlOK.Controls.Add(this.btnOK);
            this.pnlOK.Controls.Add(this.spacer1);
            this.pnlOK.Controls.Add(this.btnCancel);
            this.pnlOK.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlOK.Location = new System.Drawing.Point(0, 356);
            this.pnlOK.Name = "pnlOK";
            this.pnlOK.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlOK.Size = new System.Drawing.Size(508, 35);
            this.pnlOK.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.AppearanceKey = "button-ok";
            this.btnOK.Dock = Wisej.Web.DockStyle.Right;
            this.btnOK.Location = new System.Drawing.Point(365, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(60, 25);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(428, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(425, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // dgvHie
            // 
            this.dgvHie.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvHie.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.dgvHie.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Hie_Select,
            this.Hie_Code,
            this.Hie_Name});
            this.dgvHie.Dock = Wisej.Web.DockStyle.Fill;
            this.dgvHie.Location = new System.Drawing.Point(0, 0);
            this.dgvHie.Name = "dgvHie";
            this.dgvHie.RowHeadersWidth = 15;
            this.dgvHie.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvHie.Size = new System.Drawing.Size(508, 356);
            this.dgvHie.TabIndex = 1;
            // 
            // Hie_Select
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = false;
            this.Hie_Select.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.Hie_Select.HeaderStyle = dataGridViewCellStyle2;
            this.Hie_Select.HeaderText = "Select";
            this.Hie_Select.Name = "Hie_Select";
            this.Hie_Select.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.Hie_Select.Width = 35;
            // 
            // Hie_Code
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Padding = new Wisej.Web.Padding(15, 0, 0, 0);
            this.Hie_Code.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Padding = new Wisej.Web.Padding(15, 0, 0, 0);
            this.Hie_Code.HeaderStyle = dataGridViewCellStyle4;
            this.Hie_Code.HeaderText = "Code";
            this.Hie_Code.Name = "Hie_Code";
            this.Hie_Code.Width = 110;
            // 
            // Hie_Name
            // 
            dataGridViewCellStyle5.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Hie_Name.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Hie_Name.HeaderStyle = dataGridViewCellStyle6;
            this.Hie_Name.HeaderText = "Hierarchy Name";
            this.Hie_Name.Name = "Hie_Name";
            this.Hie_Name.Width = 300;
            // 
            // SPTargetHierarchyTarget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 391);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SPTargetHierarchyTarget";
            this.Text = "Select SP Target Hierarchy";
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlOK.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHie)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Panel pnlCompleteForm;
        private Wisej.Web.Panel pnlOK;
        private Wisej.Web.Button btnOK;
        private Wisej.Web.Spacer spacer1;
        private Wisej.Web.Button btnCancel;
        private Wisej.Web.DataGridView dgvHie;
        private Wisej.Web.DataGridViewCheckBoxColumn Hie_Select;
        private Wisej.Web.DataGridViewTextBoxColumn Hie_Code;
        private Wisej.Web.DataGridViewTextBoxColumn Hie_Name;
    }
}