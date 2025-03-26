using Wisej.Web;
using Wisej.Design;

namespace Captain.Common.Views.UserControls
{
    partial class Vendor_Maintainance
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

        #region Wisej UserControl Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle16 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle17 = new Wisej.Web.DataGridViewCellStyle();
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle14 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle15 = new Wisej.Web.DataGridViewCellStyle();
            this.gvVendor = new Wisej.Web.DataGridView();
            this.Number = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Vendor_Maintenance = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Name_Add = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvCity = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvState = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvZip = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Index_by = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlgrdVendor = new Wisej.Web.Panel();
            this.btnSearch = new Wisej.Web.Button();
            this.pnlVendorSearch = new Wisej.Web.Panel();
            this.cmbSource = new Wisej.Web.ComboBox();
            this.CmbFuelType = new Wisej.Web.ComboBox();
            this.label1 = new Wisej.Web.Label();
            this.lblSearch = new Wisej.Web.Label();
            this.lblSource = new Wisej.Web.Label();
            this.txtNum = new Wisej.Web.TextBox();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlBottom = new Wisej.Web.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gvVendor)).BeginInit();
            this.pnlgrdVendor.SuspendLayout();
            this.pnlVendorSearch.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvVendor
            // 
            this.gvVendor.AllowUserToResizeColumns = false;
            this.gvVendor.AllowUserToResizeRows = false;
            this.gvVendor.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvVendor.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvVendor.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvVendor.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvVendor.ColumnHeadersHeight = 25;
            this.gvVendor.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Number,
            this.Vendor_Maintenance,
            this.Name_Add,
            this.gvCity,
            this.gvState,
            this.gvZip,
            this.Index_by});
            dataGridViewCellStyle16.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvVendor.DefaultCellStyle = dataGridViewCellStyle16;
            this.gvVendor.Dock = Wisej.Web.DockStyle.Fill;
            this.gvVendor.Location = new System.Drawing.Point(0, 5);
            this.gvVendor.MultiSelect = false;
            this.gvVendor.Name = "gvVendor";
            dataGridViewCellStyle17.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvVendor.RowHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.gvVendor.RowHeadersWidth = 14;
            this.gvVendor.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvVendor.Size = new System.Drawing.Size(1082, 522);
            this.gvVendor.TabIndex = 6;
            // 
            // Number
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Number.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Number.HeaderStyle = dataGridViewCellStyle3;
            this.Number.HeaderText = "Number";
            this.Number.Name = "Number";
            this.Number.ReadOnly = true;
            this.Number.Width = 100;
            // 
            // Vendor_Maintenance
            // 
            this.Vendor_Maintenance.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.DoubleClick;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Vendor_Maintenance.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Vendor_Maintenance.HeaderStyle = dataGridViewCellStyle5;
            this.Vendor_Maintenance.HeaderText = "Name";
            this.Vendor_Maintenance.Name = "Vendor_Maintenance";
            this.Vendor_Maintenance.ReadOnly = true;
            this.Vendor_Maintenance.Width = 350;
            // 
            // Name_Add
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Name_Add.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name_Add.HeaderStyle = dataGridViewCellStyle7;
            this.Name_Add.HeaderText = "Address";
            this.Name_Add.Name = "Name_Add";
            this.Name_Add.ReadOnly = true;
            this.Name_Add.Width = 320;
            // 
            // gvCity
            // 
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvCity.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvCity.HeaderStyle = dataGridViewCellStyle9;
            this.gvCity.HeaderText = "City";
            this.gvCity.Name = "gvCity";
            this.gvCity.Width = 120;
            // 
            // gvState
            // 
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvState.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvState.HeaderStyle = dataGridViewCellStyle11;
            this.gvState.HeaderText = "State";
            this.gvState.Name = "gvState";
            this.gvState.Width = 60;
            // 
            // gvZip
            // 
            dataGridViewCellStyle12.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvZip.DefaultCellStyle = dataGridViewCellStyle12;
            dataGridViewCellStyle13.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvZip.HeaderStyle = dataGridViewCellStyle13;
            this.gvZip.HeaderText = "ZIP";
            this.gvZip.Name = "gvZip";
            this.gvZip.Width = 70;
            // 
            // Index_by
            // 
            dataGridViewCellStyle14.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Index_by.DefaultCellStyle = dataGridViewCellStyle14;
            dataGridViewCellStyle15.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Index_by.HeaderStyle = dataGridViewCellStyle15;
            this.Index_by.HeaderText = "Index By";
            this.Index_by.Name = "Index_by";
            this.Index_by.ReadOnly = true;
            this.Index_by.ShowInVisibilityMenu = false;
            this.Index_by.Visible = false;
            this.Index_by.Width = 10;
            // 
            // pnlgrdVendor
            // 
            this.pnlgrdVendor.AutoScroll = true;
            this.pnlgrdVendor.Controls.Add(this.gvVendor);
            this.pnlgrdVendor.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.pnlgrdVendor.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgrdVendor.Location = new System.Drawing.Point(0, 0);
            this.pnlgrdVendor.Name = "pnlgrdVendor";
            this.pnlgrdVendor.Padding = new Wisej.Web.Padding(0, 5, 0, 0);
            this.pnlgrdVendor.ScrollBars = Wisej.Web.ScrollBars.Horizontal;
            this.pnlgrdVendor.Size = new System.Drawing.Size(1082, 527);
            this.pnlgrdVendor.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(509, 41);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 25);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "&Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // pnlVendorSearch
            // 
            this.pnlVendorSearch.Controls.Add(this.cmbSource);
            this.pnlVendorSearch.Controls.Add(this.CmbFuelType);
            this.pnlVendorSearch.Controls.Add(this.label1);
            this.pnlVendorSearch.Controls.Add(this.lblSearch);
            this.pnlVendorSearch.Controls.Add(this.lblSource);
            this.pnlVendorSearch.Controls.Add(this.txtNum);
            this.pnlVendorSearch.Controls.Add(this.btnSearch);
            this.pnlVendorSearch.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.pnlVendorSearch.Dock = Wisej.Web.DockStyle.Top;
            this.pnlVendorSearch.Location = new System.Drawing.Point(10, 10);
            this.pnlVendorSearch.Name = "pnlVendorSearch";
            this.pnlVendorSearch.Size = new System.Drawing.Size(1082, 75);
            this.pnlVendorSearch.TabIndex = 0;
            // 
            // cmbSource
            // 
            this.cmbSource.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbSource.FormattingEnabled = true;
            this.cmbSource.Location = new System.Drawing.Point(345, 41);
            this.cmbSource.Name = "cmbSource";
            this.cmbSource.Size = new System.Drawing.Size(141, 25);
            this.cmbSource.TabIndex = 3;
            // 
            // CmbFuelType
            // 
            this.CmbFuelType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbFuelType.FormattingEnabled = true;
            this.CmbFuelType.Location = new System.Drawing.Point(97, 41);
            this.CmbFuelType.Name = "CmbFuelType";
            this.CmbFuelType.Size = new System.Drawing.Size(129, 25);
            this.CmbFuelType.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(258, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Service Type";
            // 
            // lblSearch
            // 
            this.lblSearch.Location = new System.Drawing.Point(15, 13);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(58, 16);
            this.lblSearch.TabIndex = 1;
            this.lblSearch.Text = "Search By";
            // 
            // lblSource
            // 
            this.lblSource.Location = new System.Drawing.Point(15, 46);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(73, 16);
            this.lblSource.TabIndex = 1;
            this.lblSource.Text = "Vendor Type";
            // 
            // txtNum
            // 
            this.txtNum.Location = new System.Drawing.Point(97, 9);
            this.txtNum.MaxLength = 10;
            this.txtNum.Name = "txtNum";
            this.txtNum.Size = new System.Drawing.Size(486, 25);
            this.txtNum.TabIndex = 1;
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlBottom);
            this.pnlCompleteForm.Controls.Add(this.pnlVendorSearch);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 25);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Padding = new Wisej.Web.Padding(10);
            this.pnlCompleteForm.Size = new System.Drawing.Size(1102, 622);
            this.pnlCompleteForm.TabIndex = 2;
            // 
            // pnlBottom
            // 
            this.pnlBottom.AutoScroll = true;
            this.pnlBottom.Controls.Add(this.pnlgrdVendor);
            this.pnlBottom.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(10, 85);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.ScrollBars = Wisej.Web.ScrollBars.Horizontal;
            this.pnlBottom.Size = new System.Drawing.Size(1082, 527);
            this.pnlBottom.TabIndex = 2;
            // 
            // Vendor_Maintainance
            // 
            this.Controls.Add(this.pnlCompleteForm);
            this.Name = "Vendor_Maintainance";
            this.Size = new System.Drawing.Size(1102, 647);
            this.Controls.SetChildIndex(this.pnlCompleteForm, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gvVendor)).EndInit();
            this.pnlgrdVendor.ResumeLayout(false);
            this.pnlVendorSearch.ResumeLayout(false);
            this.pnlVendorSearch.PerformLayout();
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView gvVendor;
        private DataGridViewTextBoxColumn Number;
        private DataGridViewTextBoxColumn Index_by;
        private DataGridViewTextBoxColumn Name_Add;
        private DataGridViewTextBoxColumn Vendor_Maintenance;
        private Panel pnlgrdVendor;
        private Button btnSearch;
        private Panel pnlVendorSearch;
        private ComboBox CmbFuelType;
        private Label label1;
        private Label lblSearch;
        private Label lblSource;
        private TextBox txtNum;
        private ComboBox cmbSource;
        private Panel pnlCompleteForm;
        private DataGridViewTextBoxColumn gvCity;
        private DataGridViewTextBoxColumn gvState;
        private DataGridViewTextBoxColumn gvZip;
        private Panel pnlBottom;
    }
}