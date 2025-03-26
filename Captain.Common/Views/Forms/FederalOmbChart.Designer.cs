using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class FederalOmbChart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FederalOmbChart));
            this.panel1 = new Wisej.Web.Panel();
            this.dataGridFed = new Wisej.Web.DataGridView();
            this.PovBase = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Increment = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel2 = new Wisej.Web.Panel();
            this.lblEndDate = new Wisej.Web.Label();
            this.lblEDate = new Wisej.Web.Label();
            this.lblStartDate = new Wisej.Web.Label();
            this.lblSDate = new Wisej.Web.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridFed)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridFed);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(648, 260);
            this.panel1.TabIndex = 0;
            this.panel1.TabStop = true;
            // 
            // dataGridFed
            // 
            this.dataGridFed.BackColor = System.Drawing.Color.White;
            this.dataGridFed.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridFed.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.PovBase,
            this.Increment,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7});
            this.dataGridFed.Dock = Wisej.Web.DockStyle.Fill;
            this.dataGridFed.Location = new System.Drawing.Point(0, 35);
            this.dataGridFed.Name = "dataGridFed";
            this.dataGridFed.ReadOnly = true;
            this.dataGridFed.RowHeadersVisible = false;
            this.dataGridFed.RowHeadersWidth = 14;
            this.dataGridFed.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.dataGridFed.ShowColumnVisibilityMenu = false;
            this.dataGridFed.Size = new System.Drawing.Size(648, 225);
            this.dataGridFed.TabIndex = 4;
            // 
            // PovBase
            // 
            this.PovBase.HeaderText = "Pov.Base";
            this.PovBase.Name = "PovBase";
            this.PovBase.ReadOnly = true;
            this.PovBase.Width = 70;
            // 
            // Increment
            // 
            this.Increment.Name = "Increment";
            this.Increment.ReadOnly = true;
            this.Increment.Width = 70;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "  ";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 70;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "  ";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 70;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "  ";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 70;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "  ";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 70;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "  ";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 70;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblEndDate);
            this.panel2.Controls.Add(this.lblEDate);
            this.panel2.Controls.Add(this.lblStartDate);
            this.panel2.Controls.Add(this.lblSDate);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(648, 35);
            this.panel2.TabIndex = 5;
            this.panel2.TabStop = true;
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblEndDate.Location = new System.Drawing.Point(227, 7);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(56, 14);
            this.lblEndDate.TabIndex = 2;
            this.lblEndDate.Text = "End Date";
            // 
            // lblEDate
            // 
            this.lblEDate.AutoSize = true;
            this.lblEDate.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblEDate.Location = new System.Drawing.Point(300, 7);
            this.lblEDate.Name = "lblEDate";
            this.lblEDate.Size = new System.Drawing.Size(4, 14);
            this.lblEDate.TabIndex = 3;
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblStartDate.Location = new System.Drawing.Point(14, 7);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(61, 14);
            this.lblStartDate.TabIndex = 0;
            this.lblStartDate.Text = "Start Date";
            // 
            // lblSDate
            // 
            this.lblSDate.AutoSize = true;
            this.lblSDate.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblSDate.Location = new System.Drawing.Point(95, 7);
            this.lblSDate.Name = "lblSDate";
            this.lblSDate.Size = new System.Drawing.Size(4, 14);
            this.lblSDate.TabIndex = 1;
            // 
            // FederalOmbChart
            // 
            this.ClientSize = new System.Drawing.Size(648, 260);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FederalOmbChart";
            this.Text = "Federal Omb Chart";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridFed)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private DataGridView dataGridFed;
        private DataGridViewTextBoxColumn PovBase;
        private DataGridViewTextBoxColumn Increment;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private Label lblEDate;
        private Label lblEndDate;
        private Label lblSDate;
        private Label lblStartDate;
        private Panel panel2;
    }
}