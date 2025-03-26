using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.UserControls
{
    partial class CASE0026Control
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

        #region WISEJ UserControl Designer generated code

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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            this.gvwBudget = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.gvtfcode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtfuncode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtStartDt = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.gvtEndDt = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.gvBDC_Year = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvBDC_ID = new Wisej.Web.DataGridViewTextBoxColumn();
            this.onSearch = new Wisej.Web.Button();
            this.txtName = new Wisej.Web.TextBox();
            this.groupBox1 = new Wisej.Web.GroupBox();
            this.rdoName = new Wisej.Web.RadioButton();
            this.rdoFund = new Wisej.Web.RadioButton();
            this.pnlBudgetSearch = new Wisej.Web.Panel();
            this.panel2 = new Wisej.Web.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gvwBudget)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.pnlBudgetSearch.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvwBudget
            // 
            this.gvwBudget.AllowUserToResizeColumns = false;
            this.gvwBudget.AllowUserToResizeRows = false;
            this.gvwBudget.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwBudget.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(4, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwBudget.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwBudget.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtfcode,
            this.gvtfuncode,
            this.gvDesc,
            this.gvtStartDt,
            this.gvtEndDt,
            this.gvBDC_Year,
            this.gvBDC_ID});
            this.gvwBudget.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.gvwBudget.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwBudget.Location = new System.Drawing.Point(5, 5);
            this.gvwBudget.MultiSelect = false;
            this.gvwBudget.Name = "gvwBudget";
            this.gvwBudget.ReadOnly = true;
            this.gvwBudget.RowHeadersWidth = 15;
            this.gvwBudget.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwBudget.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwBudget.Size = new System.Drawing.Size(1023, 509);
            this.gvwBudget.StandardTab = true;
            this.gvwBudget.TabIndex = 1;
            this.gvwBudget.DoubleClick += new System.EventHandler(this.gvwBudget_DoubleClick);
            // 
            // gvtfcode
            // 
            this.gvtfcode.HeaderText = "Fund";
            this.gvtfcode.Name = "gvtfcode";
            this.gvtfcode.ReadOnly = true;
            this.gvtfcode.Width = 90;
            // 
            // gvtfuncode
            // 
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtfuncode.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtfuncode.HeaderStyle = dataGridViewCellStyle3;
            this.gvtfuncode.HeaderText = "Fund Description";
            this.gvtfuncode.MinimumWidth = 100;
            this.gvtfuncode.Name = "gvtfuncode";
            this.gvtfuncode.ReadOnly = true;
            this.gvtfuncode.Width = 260;
            // 
            // gvDesc
            // 
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvDesc.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvDesc.HeaderStyle = dataGridViewCellStyle5;
            this.gvDesc.HeaderText = "Description";
            this.gvDesc.Name = "gvDesc";
            this.gvDesc.ReadOnly = true;
            this.gvDesc.Width = 200;
            // 
            // gvtStartDt
            // 
            this.gvtStartDt.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtStartDt.HeaderStyle = dataGridViewCellStyle7;
            this.gvtStartDt.HeaderText = "Start Date";
            this.gvtStartDt.Name = "gvtStartDt";
            this.gvtStartDt.ReadOnly = true;
            // 
            // gvtEndDt
            // 
            this.gvtEndDt.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtEndDt.HeaderStyle = dataGridViewCellStyle9;
            this.gvtEndDt.HeaderText = "End Date";
            this.gvtEndDt.Name = "gvtEndDt";
            this.gvtEndDt.ReadOnly = true;
            // 
            // gvBDC_Year
            // 
            this.gvBDC_Year.HeaderText = "Year";
            this.gvBDC_Year.Name = "gvBDC_Year";
            this.gvBDC_Year.ShowInVisibilityMenu = false;
            this.gvBDC_Year.Visible = false;
            this.gvBDC_Year.Width = 40;
            // 
            // gvBDC_ID
            // 
            this.gvBDC_ID.HeaderText = "ID";
            this.gvBDC_ID.Name = "gvBDC_ID";
            this.gvBDC_ID.ShowInVisibilityMenu = false;
            this.gvBDC_ID.Visible = false;
            this.gvBDC_ID.Width = 40;
            // 
            // onSearch
            // 
            this.onSearch.Location = new System.Drawing.Point(419, 24);
            this.onSearch.Name = "onSearch";
            this.onSearch.Size = new System.Drawing.Size(75, 25);
            this.onSearch.TabIndex = 4;
            this.onSearch.Text = "&Search";
            this.onSearch.Click += new System.EventHandler(this.onSearch_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(161, 24);
            this.txtName.MaxLength = 6;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(246, 25);
            this.txtName.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoName);
            this.groupBox1.Controls.Add(this.rdoFund);
            this.groupBox1.Controls.Add(this.onSearch);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Dock = Wisej.Web.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(7, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1011, 63);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.Text = "Budget Search";
            // 
            // rdoName
            // 
            this.rdoName.Location = new System.Drawing.Point(93, 25);
            this.rdoName.Name = "rdoName";
            this.rdoName.Size = new System.Drawing.Size(65, 21);
            this.rdoName.TabIndex = 2;
            this.rdoName.Text = "Name";
            // 
            // rdoFund
            // 
            this.rdoFund.Checked = true;
            this.rdoFund.Location = new System.Drawing.Point(24, 25);
            this.rdoFund.Name = "rdoFund";
            this.rdoFund.Size = new System.Drawing.Size(60, 21);
            this.rdoFund.TabIndex = 1;
            this.rdoFund.TabStop = true;
            this.rdoFund.Text = "Fund";
            this.rdoFund.CheckedChanged += new System.EventHandler(this.rdoFund_CheckedChanged);
            // 
            // pnlBudgetSearch
            // 
            this.pnlBudgetSearch.Controls.Add(this.groupBox1);
            this.pnlBudgetSearch.Dock = Wisej.Web.DockStyle.Top;
            this.pnlBudgetSearch.Location = new System.Drawing.Point(0, 25);
            this.pnlBudgetSearch.Name = "pnlBudgetSearch";
            this.pnlBudgetSearch.Padding = new Wisej.Web.Padding(7, 5, 15, 5);
            this.pnlBudgetSearch.Size = new System.Drawing.Size(1033, 73);
            this.pnlBudgetSearch.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gvwBudget);
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 98);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(5, 5, 5, 15);
            this.panel2.Size = new System.Drawing.Size(1033, 529);
            this.panel2.TabIndex = 2;
            // 
            // CASE0026Control
            // 
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnlBudgetSearch);
            this.Name = "CASE0026Control";
            this.Size = new System.Drawing.Size(1033, 627);
            this.Controls.SetChildIndex(this.pnlBudgetSearch, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gvwBudget)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlBudgetSearch.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridViewEx gvwBudget;
        private DataGridViewTextBoxColumn gvtfuncode;
        private Button onSearch;
        private TextBox txtName;
        private GroupBox groupBox1;
        private RadioButton rdoName;
        private RadioButton rdoFund;
        private DataGridViewTextBoxColumn gvtfcode;
        private DataGridViewTextBoxColumn gvDesc;
        private Panel pnlBudgetSearch;
        private Panel panel2;
        private Controls.Compatibility.DataGridViewDateTimeColumn gvtStartDt;
        private Controls.Compatibility.DataGridViewDateTimeColumn gvtEndDt;
        private DataGridViewTextBoxColumn gvBDC_Year;
        private DataGridViewTextBoxColumn gvBDC_ID;
    }
}