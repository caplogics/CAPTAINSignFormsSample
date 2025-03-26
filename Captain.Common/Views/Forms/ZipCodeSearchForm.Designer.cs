using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class ZipCodeSearchForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle12 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle13 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle10 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle11 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZipCodeSearchForm));
            this.onSearch = new Wisej.Web.Button();
            this.lblZipCode = new Wisej.Web.Label();
            this.cmbCounty = new Wisej.Web.ComboBox();
            this.lblCounty = new Wisej.Web.Label();
            this.lblCity = new Wisej.Web.Label();
            this.cmbTownship = new Wisej.Web.ComboBox();
            this.lblTownship = new Wisej.Web.Label();
            this.groupBox1 = new Wisej.Web.GroupBox();
            this.txtZipCode = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtCity = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.gvwCustomer = new Wisej.Web.DataGridView();
            this.ZCRZIP = new Wisej.Web.DataGridViewTextBoxColumn();
            this.ZCRCITY = new Wisej.Web.DataGridViewTextBoxColumn();
            this.ZCRSTATE = new Wisej.Web.DataGridViewTextBoxColumn();
            this.ZCRCITYCODE = new Wisej.Web.DataGridViewTextBoxColumn();
            this.ZCRCOUNTY = new Wisej.Web.DataGridViewTextBoxColumn();
            this.btnSelect = new Wisej.Web.Button();
            this.pnlZIPCode = new Wisej.Web.Panel();
            this.pnlGridCustomer = new Wisej.Web.Panel();
            this.pnlZIPSearch = new Wisej.Web.Panel();
            this.flowLayoutPanel1 = new Wisej.Web.Panel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwCustomer)).BeginInit();
            this.pnlZIPCode.SuspendLayout();
            this.pnlGridCustomer.SuspendLayout();
            this.pnlZIPSearch.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // onSearch
            // 
            this.onSearch.AppearanceKey = "button";
            this.onSearch.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.onSearch.Location = new System.Drawing.Point(493, 58);
            this.onSearch.Name = "onSearch";
            this.onSearch.Size = new System.Drawing.Size(75, 25);
            this.onSearch.TabIndex = 5;
            this.onSearch.Text = "S&earch";
            this.onSearch.Click += new System.EventHandler(this.onSearch_Click);
            // 
            // lblZipCode
            // 
            this.lblZipCode.AutoSize = true;
            this.lblZipCode.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblZipCode.Location = new System.Drawing.Point(15, 30);
            this.lblZipCode.Name = "lblZipCode";
            this.lblZipCode.Size = new System.Drawing.Size(55, 14);
            this.lblZipCode.TabIndex = 1;
            this.lblZipCode.Text = "ZIP Code";
            // 
            // cmbCounty
            // 
            this.cmbCounty.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbCounty.FormattingEnabled = true;
            this.cmbCounty.Location = new System.Drawing.Point(318, 58);
            this.cmbCounty.Name = "cmbCounty";
            this.cmbCounty.Size = new System.Drawing.Size(164, 25);
            this.cmbCounty.TabIndex = 4;
            // 
            // lblCounty
            // 
            this.lblCounty.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCounty.Location = new System.Drawing.Point(240, 62);
            this.lblCounty.Name = "lblCounty";
            this.lblCounty.Size = new System.Drawing.Size(48, 16);
            this.lblCounty.TabIndex = 7;
            this.lblCounty.Text = "County";
            // 
            // lblCity
            // 
            this.lblCity.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCity.Location = new System.Drawing.Point(16, 62);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(24, 16);
            this.lblCity.TabIndex = 3;
            this.lblCity.Text = "City";
            // 
            // cmbTownship
            // 
            this.cmbTownship.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbTownship.FormattingEnabled = true;
            this.cmbTownship.Location = new System.Drawing.Point(318, 26);
            this.cmbTownship.Name = "cmbTownship";
            this.cmbTownship.Size = new System.Drawing.Size(164, 25);
            this.cmbTownship.TabIndex = 3;
            // 
            // lblTownship
            // 
            this.lblTownship.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblTownship.Location = new System.Drawing.Point(240, 30);
            this.lblTownship.Name = "lblTownship";
            this.lblTownship.Size = new System.Drawing.Size(58, 16);
            this.lblTownship.TabIndex = 5;
            this.lblTownship.Text = "Township";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtZipCode);
            this.groupBox1.Controls.Add(this.onSearch);
            this.groupBox1.Controls.Add(this.lblZipCode);
            this.groupBox1.Controls.Add(this.cmbCounty);
            this.groupBox1.Controls.Add(this.lblCounty);
            this.groupBox1.Controls.Add(this.lblCity);
            this.groupBox1.Controls.Add(this.txtCity);
            this.groupBox1.Controls.Add(this.cmbTownship);
            this.groupBox1.Controls.Add(this.lblTownship);
            this.groupBox1.Dock = Wisej.Web.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(587, 90);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.Text = "ZIP Code Search";
            // 
            // txtZipCode
            // 
            this.txtZipCode.Location = new System.Drawing.Point(77, 26);
            this.txtZipCode.MaxLength = 9;
            this.txtZipCode.Name = "txtZipCode";
            this.txtZipCode.Size = new System.Drawing.Size(72, 25);
            this.txtZipCode.TabIndex = 1;
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(77, 58);
            this.txtCity.MaxLength = 30;
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(140, 25);
            this.txtCity.TabIndex = 2;
            // 
            // gvwCustomer
            // 
            this.gvwCustomer.AllowUserToResizeColumns = false;
            this.gvwCustomer.AllowUserToResizeRows = false;
            this.gvwCustomer.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwCustomer.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("default", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(4, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwCustomer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwCustomer.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.ZCRZIP,
            this.ZCRCITY,
            this.ZCRSTATE,
            this.ZCRCITYCODE,
            this.ZCRCOUNTY});
            dataGridViewCellStyle12.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvwCustomer.DefaultCellStyle = dataGridViewCellStyle12;
            this.gvwCustomer.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwCustomer.Location = new System.Drawing.Point(0, 0);
            this.gvwCustomer.MultiSelect = false;
            this.gvwCustomer.Name = "gvwCustomer";
            this.gvwCustomer.ReadOnly = true;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvwCustomer.RowHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.gvwCustomer.RowHeadersWidth = 15;
            this.gvwCustomer.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwCustomer.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwCustomer.Size = new System.Drawing.Size(587, 313);
            this.gvwCustomer.StandardTab = true;
            this.gvwCustomer.TabIndex = 6;
            this.gvwCustomer.DoubleClick += new System.EventHandler(this.gvwCustomer_DoubleClick);
            // 
            // ZCRZIP
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ZCRZIP.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ZCRZIP.HeaderStyle = dataGridViewCellStyle3;
            this.ZCRZIP.HeaderText = "ZIP Code*";
            this.ZCRZIP.MinimumWidth = 100;
            this.ZCRZIP.Name = "ZCRZIP";
            this.ZCRZIP.ReadOnly = true;
            // 
            // ZCRCITY
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.ZCRCITY.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ZCRCITY.HeaderStyle = dataGridViewCellStyle5;
            this.ZCRCITY.HeaderText = "City*";
            this.ZCRCITY.MinimumWidth = 150;
            this.ZCRCITY.Name = "ZCRCITY";
            this.ZCRCITY.ReadOnly = true;
            this.ZCRCITY.Width = 150;
            // 
            // ZCRSTATE
            // 
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ZCRSTATE.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ZCRSTATE.HeaderStyle = dataGridViewCellStyle7;
            this.ZCRSTATE.HeaderText = "State*";
            this.ZCRSTATE.MinimumWidth = 60;
            this.ZCRSTATE.Name = "ZCRSTATE";
            this.ZCRSTATE.ReadOnly = true;
            this.ZCRSTATE.Width = 60;
            // 
            // ZCRCITYCODE
            // 
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle8.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.ZCRCITYCODE.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ZCRCITYCODE.HeaderStyle = dataGridViewCellStyle9;
            this.ZCRCITYCODE.HeaderText = "Township";
            this.ZCRCITYCODE.MinimumWidth = 150;
            this.ZCRCITYCODE.Name = "ZCRCITYCODE";
            this.ZCRCITYCODE.ReadOnly = true;
            this.ZCRCITYCODE.Width = 150;
            // 
            // ZCRCOUNTY
            // 
            dataGridViewCellStyle10.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle10.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.ZCRCOUNTY.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ZCRCOUNTY.HeaderStyle = dataGridViewCellStyle11;
            this.ZCRCOUNTY.HeaderText = "County";
            this.ZCRCOUNTY.MinimumWidth = 150;
            this.ZCRCOUNTY.Name = "ZCRCOUNTY";
            this.ZCRCOUNTY.ReadOnly = true;
            this.ZCRCOUNTY.Width = 150;
            // 
            // btnSelect
            // 
            this.btnSelect.AppearanceKey = "button";
            this.btnSelect.Dock = Wisej.Web.DockStyle.Right;
            this.btnSelect.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSelect.Location = new System.Drawing.Point(507, 5);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 25);
            this.btnSelect.TabIndex = 12;
            this.btnSelect.Text = "&Select";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // pnlZIPCode
            // 
            this.pnlZIPCode.Controls.Add(this.pnlGridCustomer);
            this.pnlZIPCode.Controls.Add(this.pnlZIPSearch);
            this.pnlZIPCode.Dock = Wisej.Web.DockStyle.Top;
            this.pnlZIPCode.Location = new System.Drawing.Point(0, 0);
            this.pnlZIPCode.Name = "pnlZIPCode";
            this.pnlZIPCode.Padding = new Wisej.Web.Padding(5);
            this.pnlZIPCode.Size = new System.Drawing.Size(597, 413);
            this.pnlZIPCode.TabIndex = 13;
            // 
            // pnlGridCustomer
            // 
            this.pnlGridCustomer.Controls.Add(this.gvwCustomer);
            this.pnlGridCustomer.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlGridCustomer.Location = new System.Drawing.Point(5, 95);
            this.pnlGridCustomer.Name = "pnlGridCustomer";
            this.pnlGridCustomer.Size = new System.Drawing.Size(587, 313);
            this.pnlGridCustomer.TabIndex = 14;
            // 
            // pnlZIPSearch
            // 
            this.pnlZIPSearch.Controls.Add(this.groupBox1);
            this.pnlZIPSearch.Dock = Wisej.Web.DockStyle.Top;
            this.pnlZIPSearch.Location = new System.Drawing.Point(5, 5);
            this.pnlZIPSearch.Name = "pnlZIPSearch";
            this.pnlZIPSearch.Size = new System.Drawing.Size(587, 90);
            this.pnlZIPSearch.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AppearanceKey = "panel-grdo";
            this.flowLayoutPanel1.Controls.Add(this.btnSelect);
            this.flowLayoutPanel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 413);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(597, 35);
            this.flowLayoutPanel1.TabIndex = 15;
            // 
            // ZipCodeSearchForm
            // 
            this.ClientSize = new System.Drawing.Size(597, 448);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.pnlZIPCode);
            this.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ZipCodeSearchForm";
            this.Text = "ZIP Code Search";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwCustomer)).EndInit();
            this.pnlZIPCode.ResumeLayout(false);
            this.pnlGridCustomer.ResumeLayout(false);
            this.pnlZIPSearch.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Button onSearch;
        private Label lblZipCode;
        private ComboBox cmbCounty;
        private Label lblCounty;
        private Label lblCity;
        private TextBoxWithValidation txtCity;
        private ComboBox cmbTownship;
        private Label lblTownship;
        private GroupBox groupBox1;
        private DataGridView gvwCustomer;
        private DataGridViewTextBoxColumn ZCRZIP;
        private DataGridViewTextBoxColumn ZCRCITY;
        private DataGridViewTextBoxColumn ZCRSTATE;
        private DataGridViewTextBoxColumn ZCRCITYCODE;
        private DataGridViewTextBoxColumn ZCRCOUNTY;
        private Button btnSelect;
        private TextBoxWithValidation txtZipCode;
        private Panel pnlZIPCode;
        private Panel pnlGridCustomer;
        private Panel flowLayoutPanel1;
        private Panel pnlZIPSearch;
    }
}