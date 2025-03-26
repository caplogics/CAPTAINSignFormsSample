//using Wisej.Web;
//using Gizmox.WebGUI.Common;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.UserControls
{
    partial class ADMN0013
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

        #region Visual WebGui UserControl Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle14 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle15 = new Wisej.Web.DataGridViewCellStyle();
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle12 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle13 = new Wisej.Web.DataGridViewCellStyle();
            this.groupBox1 = new Wisej.Web.GroupBox();
            this.txtZipCode = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.onSearch = new Wisej.Web.Button();
            this.lblZipCode = new Wisej.Web.Label();
            this.cmbCounty = new Wisej.Web.ComboBox();
            this.lblCounty = new Wisej.Web.Label();
            this.lblCity = new Wisej.Web.Label();
            this.txtCity = new Wisej.Web.TextBox();
            this.cmbTownship = new Wisej.Web.ComboBox();
            this.lblTownship = new Wisej.Web.Label();
            this.gvwCustomer = new Wisej.Web.DataGridView();
            this.ZCRZIP = new Wisej.Web.DataGridViewTextBoxColumn();
            this.ZCRCITY = new Wisej.Web.DataGridViewTextBoxColumn();
            this.ZCRSTATE = new Wisej.Web.DataGridViewTextBoxColumn();
            this.ZCRCITYCODE = new Wisej.Web.DataGridViewTextBoxColumn();
            this.ZCRCOUNTY = new Wisej.Web.DataGridViewTextBoxColumn();
            this.ZCRINTAKECODE = new Wisej.Web.DataGridViewTextBoxColumn();
            this.cmbImageTypes = new Captain.Common.Views.UserControls.ListBoxWithDropDownControl();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlGridZip = new Wisej.Web.Panel();
            this.pnlSearch = new Wisej.Web.Panel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwCustomer)).BeginInit();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlGridZip.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.SuspendLayout();
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
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new Wisej.Web.Padding(0);
            this.groupBox1.Size = new System.Drawing.Size(684, 85);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.Text = "ZIP Code Search";
            // 
            // txtZipCode
            // 
            this.txtZipCode.Location = new System.Drawing.Point(73, 23);
            this.txtZipCode.MaxLength = 9;
            this.txtZipCode.Name = "txtZipCode";
            this.txtZipCode.Size = new System.Drawing.Size(72, 25);
            this.txtZipCode.TabIndex = 1;
            // 
            // onSearch
            // 
            this.onSearch.BackColor = System.Drawing.Color.FromName("@window");
            this.onSearch.Location = new System.Drawing.Point(434, 54);
            this.onSearch.Name = "onSearch";
            this.onSearch.Size = new System.Drawing.Size(75, 25);
            this.onSearch.TabIndex = 5;
            this.onSearch.Text = "&Search";
            this.onSearch.Click += new System.EventHandler(this.onSearch_Click);
            // 
            // lblZipCode
            // 
            this.lblZipCode.Location = new System.Drawing.Point(15, 27);
            this.lblZipCode.Name = "lblZipCode";
            this.lblZipCode.Size = new System.Drawing.Size(51, 16);
            this.lblZipCode.TabIndex = 1;
            this.lblZipCode.Text = "ZIP Code";
            // 
            // cmbCounty
            // 
            this.cmbCounty.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbCounty.FormattingEnabled = true;
            this.cmbCounty.Location = new System.Drawing.Point(298, 54);
            this.cmbCounty.Name = "cmbCounty";
            this.cmbCounty.Size = new System.Drawing.Size(110, 25);
            this.cmbCounty.TabIndex = 4;
            // 
            // lblCounty
            // 
            this.lblCounty.Location = new System.Drawing.Point(236, 58);
            this.lblCounty.Name = "lblCounty";
            this.lblCounty.Size = new System.Drawing.Size(42, 16);
            this.lblCounty.TabIndex = 7;
            this.lblCounty.Text = "County";
            // 
            // lblCity
            // 
            this.lblCity.Location = new System.Drawing.Point(15, 58);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(25, 16);
            this.lblCity.TabIndex = 3;
            this.lblCity.Text = "City";
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(73, 54);
            this.txtCity.MaxLength = 30;
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(140, 25);
            this.txtCity.TabIndex = 2;
            // 
            // cmbTownship
            // 
            this.cmbTownship.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbTownship.FormattingEnabled = true;
            this.cmbTownship.Location = new System.Drawing.Point(298, 23);
            this.cmbTownship.Name = "cmbTownship";
            this.cmbTownship.Size = new System.Drawing.Size(110, 25);
            this.cmbTownship.TabIndex = 3;
            // 
            // lblTownship
            // 
            this.lblTownship.Location = new System.Drawing.Point(236, 27);
            this.lblTownship.Name = "lblTownship";
            this.lblTownship.Size = new System.Drawing.Size(56, 16);
            this.lblTownship.TabIndex = 5;
            this.lblTownship.Text = "Township";
            // 
            // gvwCustomer
            // 
            this.gvwCustomer.AllowUserToResizeColumns = false;
            this.gvwCustomer.AllowUserToResizeRows = false;
            this.gvwCustomer.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwCustomer.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvwCustomer.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(4, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwCustomer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwCustomer.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.ZCRZIP,
            this.ZCRCITY,
            this.ZCRSTATE,
            this.ZCRCITYCODE,
            this.ZCRCOUNTY,
            this.ZCRINTAKECODE});
            dataGridViewCellStyle14.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvwCustomer.DefaultCellStyle = dataGridViewCellStyle14;
            this.gvwCustomer.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwCustomer.Font = new System.Drawing.Font("default", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvwCustomer.Location = new System.Drawing.Point(0, 0);
            this.gvwCustomer.MultiSelect = false;
            this.gvwCustomer.Name = "gvwCustomer";
            dataGridViewCellStyle15.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvwCustomer.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.gvwCustomer.RowHeadersWidth = 15;
            this.gvwCustomer.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwCustomer.Size = new System.Drawing.Size(684, 416);
            this.gvwCustomer.StandardTab = true;
            this.gvwCustomer.TabIndex = 6;
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
            this.ZCRCITYCODE.MinimumWidth = 160;
            this.ZCRCITYCODE.Name = "ZCRCITYCODE";
            this.ZCRCITYCODE.ReadOnly = true;
            this.ZCRCITYCODE.Width = 160;
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
            this.ZCRCOUNTY.MinimumWidth = 160;
            this.ZCRCOUNTY.Name = "ZCRCOUNTY";
            this.ZCRCOUNTY.ReadOnly = true;
            this.ZCRCOUNTY.Width = 160;
            // 
            // ZCRINTAKECODE
            // 
            dataGridViewCellStyle12.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ZCRINTAKECODE.DefaultCellStyle = dataGridViewCellStyle12;
            dataGridViewCellStyle13.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ZCRINTAKECODE.HeaderStyle = dataGridViewCellStyle13;
            this.ZCRINTAKECODE.HeaderText = "Intake Code";
            this.ZCRINTAKECODE.MinimumWidth = 80;
            this.ZCRINTAKECODE.Name = "ZCRINTAKECODE";
            this.ZCRINTAKECODE.ReadOnly = true;
            this.ZCRINTAKECODE.ShowInVisibilityMenu = false;
            this.ZCRINTAKECODE.Visible = false;
            this.ZCRINTAKECODE.Width = 80;
            // 
            // cmbImageTypes
            // 
            this.cmbImageTypes.AutoValidate = Wisej.Web.AutoValidate.EnablePreventFocusChange;
            this.cmbImageTypes.Location = new System.Drawing.Point(121, 148);
            this.cmbImageTypes.Name = "cmbImageTypes";
            this.cmbImageTypes.Size = new System.Drawing.Size(196, 86);
            this.cmbImageTypes.TabIndex = 24;
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlGridZip);
            this.pnlCompleteForm.Controls.Add(this.pnlSearch);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 25);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Padding = new Wisej.Web.Padding(10, 6, 10, 10);
            this.pnlCompleteForm.Size = new System.Drawing.Size(704, 522);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlGridZip
            // 
            this.pnlGridZip.Controls.Add(this.gvwCustomer);
            this.pnlGridZip.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.pnlGridZip.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlGridZip.Location = new System.Drawing.Point(10, 96);
            this.pnlGridZip.Name = "pnlGridZip";
            this.pnlGridZip.Size = new System.Drawing.Size(684, 416);
            this.pnlGridZip.TabIndex = 2;
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.groupBox1);
            this.pnlSearch.Dock = Wisej.Web.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(10, 6);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Padding = new Wisej.Web.Padding(0, 0, 0, 5);
            this.pnlSearch.Size = new System.Drawing.Size(684, 90);
            this.pnlSearch.TabIndex = 1;
            // 
            // ADMN0013
            // 
            this.BackgroundImageLayout = Wisej.Web.ImageLayout.Stretch;
            this.Controls.Add(this.pnlCompleteForm);
            this.Name = "ADMN0013";
            this.Size = new System.Drawing.Size(704, 547);
            this.Controls.SetChildIndex(this.pnlCompleteForm, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwCustomer)).EndInit();
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlGridZip.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DataGridView gvwCustomer;
        private DataGridViewTextBoxColumn ZCRZIP;
        private DataGridViewTextBoxColumn ZCRCITY;
        private DataGridViewTextBoxColumn ZCRSTATE;
        private ListBoxWithDropDownControl cmbImageTypes;
        private DataGridViewTextBoxColumn ZCRCITYCODE;
        private DataGridViewTextBoxColumn ZCRCOUNTY;
        private DataGridViewTextBoxColumn ZCRINTAKECODE;
        private Label lblZipCode;
        private Button onSearch;
        private ComboBox cmbCounty;
        private Label lblCounty;
        private ComboBox cmbTownship;
        private Label lblTownship;
        private TextBox txtCity;
        private Label lblCity;
        private GroupBox groupBox1;
        private TextBoxWithValidation txtZipCode;
        private Panel pnlCompleteForm;
        private Panel pnlGridZip;
        private Panel pnlSearch;
    }
}