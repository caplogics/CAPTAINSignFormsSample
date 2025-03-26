using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class VoterSerachForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VoterSerachForm));
            this.panel1 = new Wisej.Web.Panel();
            this.gvwvoters = new Wisej.Web.DataGridView();
            this.gvtBlock = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtEo = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtCity = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtDirection = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtStreet = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtSuffix = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtPreint = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel2 = new Wisej.Web.Panel();
            this.onSearch = new Wisej.Web.Button();
            this.txtCity = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.cmbstreet = new Wisej.Web.ComboBox();
            this.flowLayoutPanel1 = new Wisej.Web.FlowLayoutPanel();
            this.btnSelect = new Wisej.Web.Button();
            this.lblTotal = new Wisej.Web.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwvoters)).BeginInit();
            this.panel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gvwvoters);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Controls.Add(this.lblTotal);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(484, 349);
            this.panel1.TabIndex = 0;
            this.panel1.TabStop = true;
            // 
            // gvwvoters
            // 
            this.gvwvoters.AllowUserToResizeColumns = false;
            this.gvwvoters.AllowUserToResizeRows = false;
            this.gvwvoters.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(4, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwvoters.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwvoters.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtBlock,
            this.gvtEo,
            this.gvtCity,
            this.gvtDirection,
            this.gvtStreet,
            this.gvtSuffix,
            this.gvtPreint});
            this.gvwvoters.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwvoters.Location = new System.Drawing.Point(0, 41);
            this.gvwvoters.MultiSelect = false;
            this.gvwvoters.Name = "gvwvoters";
            this.gvwvoters.ReadOnly = true;
            this.gvwvoters.RowHeadersWidth = 10;
            this.gvwvoters.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwvoters.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwvoters.Size = new System.Drawing.Size(484, 273);
            this.gvwvoters.StandardTab = true;
            this.gvwvoters.TabIndex = 6;
            // 
            // gvtBlock
            // 
            this.gvtBlock.HeaderText = "Block";
            this.gvtBlock.Name = "gvtBlock";
            this.gvtBlock.ReadOnly = true;
            this.gvtBlock.Width = 60;
            // 
            // gvtEo
            // 
            this.gvtEo.HeaderText = "  ";
            this.gvtEo.Name = "gvtEo";
            this.gvtEo.ReadOnly = true;
            this.gvtEo.Width = 15;
            // 
            // gvtCity
            // 
            this.gvtCity.HeaderText = "City";
            this.gvtCity.Name = "gvtCity";
            this.gvtCity.ReadOnly = true;
            // 
            // gvtDirection
            // 
            this.gvtDirection.HeaderText = "Direction";
            this.gvtDirection.Name = "gvtDirection";
            this.gvtDirection.ReadOnly = true;
            this.gvtDirection.Width = 80;
            // 
            // gvtStreet
            // 
            this.gvtStreet.HeaderText = "Street";
            this.gvtStreet.Name = "gvtStreet";
            this.gvtStreet.ReadOnly = true;
            // 
            // gvtSuffix
            // 
            this.gvtSuffix.HeaderText = "Suffix";
            this.gvtSuffix.Name = "gvtSuffix";
            this.gvtSuffix.ReadOnly = true;
            this.gvtSuffix.Width = 40;
            // 
            // gvtPreint
            // 
            this.gvtPreint.HeaderText = "Precinct";
            this.gvtPreint.Name = "gvtPreint";
            this.gvtPreint.ReadOnly = true;
            this.gvtPreint.Width = 53;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.onSearch);
            this.panel2.Controls.Add(this.txtCity);
            this.panel2.Controls.Add(this.cmbstreet);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(484, 41);
            this.panel2.TabIndex = 13;
            this.panel2.TabStop = true;
            // 
            // onSearch
            // 
            this.onSearch.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.onSearch.Location = new System.Drawing.Point(400, 8);
            this.onSearch.Name = "onSearch";
            this.onSearch.Size = new System.Drawing.Size(79, 25);
            this.onSearch.TabIndex = 8;
            this.onSearch.Text = "&Search";
            this.onSearch.Click += new System.EventHandler(this.onSearch_Click);
            // 
            // txtCity
            // 
            this.txtCity.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtCity.Location = new System.Drawing.Point(150, 8);
            this.txtCity.MaxLength = 30;
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(244, 25);
            this.txtCity.TabIndex = 6;
            // 
            // cmbstreet
            // 
            this.cmbstreet.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbstreet.FormattingEnabled = true;
            this.cmbstreet.Location = new System.Drawing.Point(4, 8);
            this.cmbstreet.Name = "cmbstreet";
            this.cmbstreet.Size = new System.Drawing.Size(140, 25);
            this.cmbstreet.TabIndex = 7;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AppearanceKey = "panel-grdo";
            this.flowLayoutPanel1.Controls.Add(this.btnSelect);
            this.flowLayoutPanel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = Wisej.Web.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 314);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new Wisej.Web.Padding(2);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(484, 35);
            this.flowLayoutPanel1.TabIndex = 12;
            this.flowLayoutPanel1.TabStop = true;
            // 
            // btnSelect
            // 
            this.btnSelect.AutoSize = true;
            this.btnSelect.Dock = Wisej.Web.DockStyle.Right;
            this.btnSelect.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSelect.Location = new System.Drawing.Point(417, 5);
            this.btnSelect.MinimumSize = new System.Drawing.Size(0, 25);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(60, 25);
            this.btnSelect.TabIndex = 12;
            this.btnSelect.Text = "S&elect";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(9, 266);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(4, 14);
            this.lblTotal.TabIndex = 1;
            // 
            // VoterSerachForm
            // 
            this.ClientSize = new System.Drawing.Size(484, 349);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VoterSerachForm";
            this.Text = "Voter Registration Street Lookup";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwvoters)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private DataGridView gvwvoters;
        private Label lblTotal;
        private DataGridViewTextBoxColumn gvtBlock;
        private DataGridViewTextBoxColumn gvtEo;
        private DataGridViewTextBoxColumn gvtCity;
        private DataGridViewTextBoxColumn gvtDirection;
        private DataGridViewTextBoxColumn gvtStreet;
        private DataGridViewTextBoxColumn gvtSuffix;
        private DataGridViewTextBoxColumn gvtPreint;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnSelect;
        private Panel panel2;
        private Button onSearch;
        private TextBoxWithValidation txtCity;
        private ComboBox cmbstreet;
    }
}