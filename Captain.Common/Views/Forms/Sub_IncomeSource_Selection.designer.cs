using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class Sub_IncomeSource_Selection
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

        #region Wisej web Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Sub_IncomeSource_Selection));
            this.panel1 = new Wisej.Web.Panel();
            this.IncSourceGrid = new Wisej.Web.DataGridView();
            this.Check = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.IncDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.IncCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel2 = new Wisej.Web.Panel();
            this.BtnSave = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.BtnCancel = new Wisej.Web.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IncSourceGrid)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.IncSourceGrid);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(445, 438);
            this.panel1.TabIndex = 0;
            // 
            // IncSourceGrid
            // 
            this.IncSourceGrid.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.IncSourceGrid.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.IncSourceGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.IncSourceGrid.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.IncSourceGrid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Check,
            this.IncDesc,
            this.IncCode});
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.IncSourceGrid.DefaultCellStyle = dataGridViewCellStyle5;
            this.IncSourceGrid.Dock = Wisej.Web.DockStyle.Fill;
            this.IncSourceGrid.Location = new System.Drawing.Point(0, 0);
            this.IncSourceGrid.Name = "IncSourceGrid";
            this.IncSourceGrid.RowHeadersVisible = false;
            this.IncSourceGrid.RowHeadersWidth = 25;
            this.IncSourceGrid.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.IncSourceGrid.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.IncSourceGrid.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.IncSourceGrid.Size = new System.Drawing.Size(445, 403);
            this.IncSourceGrid.TabIndex = 0;
            this.IncSourceGrid.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.IncSourceGrid_CellClick);
            // 
            // Check
            // 
            this.Check.HeaderText = " ";
            this.Check.Name = "Check";
            this.Check.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.Check.ShowInVisibilityMenu = false;
            this.Check.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.Check.Width = 40;
            // 
            // IncDesc
            // 
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.IncDesc.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.IncDesc.HeaderStyle = dataGridViewCellStyle3;
            this.IncDesc.HeaderText = "Source Description";
            this.IncDesc.Name = "IncDesc";
            this.IncDesc.ReadOnly = true;
            this.IncDesc.Width = 307;
            // 
            // IncCode
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.IncCode.HeaderStyle = dataGridViewCellStyle4;
            this.IncCode.HeaderText = "Code";
            this.IncCode.Name = "IncCode";
            this.IncCode.ReadOnly = true;
            this.IncCode.Width = 53;
            // 
            // panel2
            // 
            this.panel2.AppearanceKey = "panel-grdo";
            this.panel2.Controls.Add(this.BtnSave);
            this.panel2.Controls.Add(this.spacer1);
            this.panel2.Controls.Add(this.BtnCancel);
            this.panel2.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 403);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.panel2.Size = new System.Drawing.Size(445, 35);
            this.panel2.TabIndex = 1;
            // 
            // BtnSave
            // 
            this.BtnSave.AppearanceKey = "button-ok";
            this.BtnSave.Dock = Wisej.Web.DockStyle.Right;
            this.BtnSave.Location = new System.Drawing.Point(275, 5);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 25);
            this.BtnSave.TabIndex = 0;
            this.BtnSave.Text = "&Save";
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(350, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 25);
            // 
            // BtnCancel
            // 
            this.BtnCancel.AppearanceKey = "button-error";
            this.BtnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.BtnCancel.Location = new System.Drawing.Point(355, 5);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 25);
            this.BtnCancel.TabIndex = 0;
            this.BtnCancel.Text = "&Cancel";
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // Sub_IncomeSource_Selection
            // 
            this.ClientSize = new System.Drawing.Size(445, 438);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Sub_IncomeSource_Selection";
            this.Text = "Sub Income Source Selection";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.IncSourceGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private DataGridView IncSourceGrid;
        private DataGridViewCheckBoxColumn Check;
        private DataGridViewTextBoxColumn IncDesc;
        private DataGridViewTextBoxColumn IncCode;
        private Panel panel2;
        private Button BtnCancel;
        private Button BtnSave;
        private Spacer spacer1;
    }
}