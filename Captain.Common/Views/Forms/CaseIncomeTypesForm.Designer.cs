using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class CaseIncomeTypesForm
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
			Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
			this.gvtInterval = new Wisej.Web.DataGridViewTextBoxColumn();
			this.Item = new Wisej.Web.DataGridViewTextBoxColumn();
			this.contextMenu1 = new Wisej.Web.ContextMenu();
			this.Select = new Wisej.Web.DataGridViewCheckBoxColumn();
			this.gvwIncomeTypes = new Wisej.Web.DataGridView();
			this.AlertCode = new Wisej.Web.DataGridViewTextBoxColumn();
			this.flowLayoutPanel1 = new Wisej.Web.FlowLayoutPanel();
			this.btnClose = new Wisej.Web.Button();
			this.btnOk = new Wisej.Web.Button();
			((System.ComponentModel.ISupportInitialize)(this.gvwIncomeTypes)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// gvtInterval
			// 
			this.gvtInterval.HeaderText = "Interval";
			this.gvtInterval.Name = "gvtInterval";
			this.gvtInterval.ReadOnly = true;
			// 
			// Item
			// 
			this.Item.ContextMenu = this.contextMenu1;
			this.Item.HeaderText = "Income Types";
			this.Item.Name = "Item";
			this.Item.ReadOnly = true;
			this.Item.Resizable = Wisej.Web.DataGridViewTriState.True;
			this.Item.Width = 200;
			// 
			// contextMenu1
			// 
			this.contextMenu1.Name = "contextMenu1";
			// 
			// Select
			// 
			this.Select.HeaderText = "  ";
			this.Select.Name = "Select";
			this.Select.Width = 40;
			// 
			// gvwIncomeTypes
			// 
			this.gvwIncomeTypes.AllowUserToResizeRows = false;
			this.gvwIncomeTypes.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("default", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-IN");
			dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
			dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
			this.gvwIncomeTypes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.gvwIncomeTypes.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Select,
            this.Item,
            this.AlertCode,
            this.gvtInterval});
			this.gvwIncomeTypes.Dock = Wisej.Web.DockStyle.Fill;
			this.gvwIncomeTypes.Location = new System.Drawing.Point(0, 0);
			this.gvwIncomeTypes.MultiSelect = false;
			this.gvwIncomeTypes.Name = "gvwIncomeTypes";
			this.gvwIncomeTypes.RowHeadersVisible = false;
			this.gvwIncomeTypes.RowHeadersWidth = 15;
			this.gvwIncomeTypes.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-IN");
			this.gvwIncomeTypes.ScrollBars = Wisej.Web.ScrollBars.Vertical;
			this.gvwIncomeTypes.Size = new System.Drawing.Size(351, 289);
			this.gvwIncomeTypes.TabIndex = 0;
			// 
			// AlertCode
			// 
			this.AlertCode.Name = "AlertCode";
			this.AlertCode.Visible = false;
			this.AlertCode.Width = 50;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromName("@control");
			this.flowLayoutPanel1.Controls.Add(this.btnClose);
			this.flowLayoutPanel1.Controls.Add(this.btnOk);
			this.flowLayoutPanel1.Dock = Wisej.Web.DockStyle.Bottom;
			this.flowLayoutPanel1.FlowDirection = Wisej.Web.FlowDirection.RightToLeft;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 245);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Padding = new Wisej.Web.Padding(3);
			this.flowLayoutPanel1.Size = new System.Drawing.Size(351, 44);
			this.flowLayoutPanel1.TabIndex = 7;
			this.flowLayoutPanel1.TabStop = true;
			// 
			// btnClose
			// 
			this.btnClose.Font = new System.Drawing.Font("@buttonText", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.btnClose.Location = new System.Drawing.Point(242, 6);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(100, 33);
			this.btnClose.TabIndex = 8;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnOk
			// 
			this.btnOk.Enabled = false;
			this.btnOk.Font = new System.Drawing.Font("@buttonText", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.btnOk.Location = new System.Drawing.Point(136, 6);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(100, 33);
			this.btnOk.TabIndex = 7;
			this.btnOk.Text = "OK";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// CaseIncomeTypesForm
			// 
			this.ClientSize = new System.Drawing.Size(351, 289);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.gvwIncomeTypes);
			this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CaseIncomeTypesForm";
			this.Text = "CaseIncomeTypesForm";
			((System.ComponentModel.ISupportInitialize)(this.gvwIncomeTypes)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private DataGridViewTextBoxColumn gvtInterval;
        private DataGridViewTextBoxColumn Item;
        private DataGridViewCheckBoxColumn Select;
        private DataGridView gvwIncomeTypes;
        private DataGridViewTextBoxColumn AlertCode;
        private ContextMenu contextMenu1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnClose;
        private Button btnOk;
    }
}