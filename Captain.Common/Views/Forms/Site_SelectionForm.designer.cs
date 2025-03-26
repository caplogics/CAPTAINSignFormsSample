using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class Site_SelectionForm
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

        #region Visual WebGui Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Site_SelectionForm));
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlgvsite = new Wisej.Web.Panel();
            this.gvsite = new Wisej.Web.DataGridView();
            this.Sel_Img = new Wisej.Web.DataGridViewImageColumn();
            this.Site_No = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Room = new Wisej.Web.DataGridViewTextBoxColumn();
            this.AMPM = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Site_Name = new Wisej.Web.DataGridViewTextBoxColumn();
            this.City = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Selected = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlSelect = new Wisej.Web.Panel();
            this.btnSelect = new Wisej.Web.Button();
            this.Filter = new Wisej.Web.Ext.ColumnFilter.ColumnFilter(this.components);
            this.pnlCompleteForm.SuspendLayout();
            this.pnlgvsite.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvsite)).BeginInit();
            this.pnlSelect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Filter)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlgvsite);
            this.pnlCompleteForm.Controls.Add(this.pnlSelect);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(648, 380);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlgvsite
            // 
            this.pnlgvsite.Controls.Add(this.gvsite);
            this.pnlgvsite.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvsite.Location = new System.Drawing.Point(0, 0);
            this.pnlgvsite.Name = "pnlgvsite";
            this.pnlgvsite.Size = new System.Drawing.Size(648, 345);
            this.pnlgvsite.TabIndex = 1;
            // 
            // gvsite
            // 
            this.gvsite.AllowUserToResizeColumns = false;
            this.gvsite.AllowUserToResizeRows = false;
            this.gvsite.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvsite.BackColor = System.Drawing.Color.FromArgb(251, 251, 251);
            this.gvsite.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvsite.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvsite.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvsite.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Sel_Img,
            this.Site_No,
            this.Room,
            this.AMPM,
            this.Site_Name,
            this.City,
            this.Selected});
            this.gvsite.Dock = Wisej.Web.DockStyle.Fill;
            this.gvsite.Location = new System.Drawing.Point(0, 0);
            this.gvsite.Name = "gvsite";
            this.gvsite.RowHeadersWidth = 25;
            this.gvsite.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvsite.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvsite.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvsite.Size = new System.Drawing.Size(648, 345);
            this.gvsite.TabIndex = 2;
            this.gvsite.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvsite_CellClick);
            this.gvsite.DataError += new Wisej.Web.DataGridViewDataErrorEventHandler(this.gvsite_DataError);
            // 
            // Sel_Img
            // 
            this.Sel_Img.CellImageLayout = Wisej.Web.DataGridViewImageCellLayout.None;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Sel_Img.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Sel_Img.HeaderStyle = dataGridViewCellStyle3;
            this.Sel_Img.HeaderText = " ";
            this.Sel_Img.Name = "Sel_Img";
            this.Sel_Img.ShowInVisibilityMenu = false;
            this.Sel_Img.Width = 38;
            // 
            // Site_No
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Site_No.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Site_No.HeaderStyle = dataGridViewCellStyle5;
            this.Site_No.HeaderText = "Site No";
            this.Site_No.Name = "Site_No";
            this.Site_No.ReadOnly = true;
            this.Filter.SetShowFilter(this.Site_No, true);
            this.Site_No.ValueType = typeof(string);
            this.Site_No.Width = 90;
            // 
            // Room
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Room.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Room.HeaderStyle = dataGridViewCellStyle7;
            this.Room.HeaderText = "Room";
            this.Room.Name = "Room";
            this.Room.ReadOnly = true;
            this.Filter.SetShowFilter(this.Room, true);
            this.Room.ShowInVisibilityMenu = false;
            this.Room.ValueType = typeof(string);
            this.Room.Width = 60;
            // 
            // AMPM
            // 
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.AMPM.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.AMPM.HeaderStyle = dataGridViewCellStyle9;
            this.AMPM.HeaderText = "AM/PM";
            this.AMPM.Name = "AMPM";
            this.AMPM.ReadOnly = true;
            this.Filter.SetShowFilter(this.AMPM, true);
            this.AMPM.ShowInVisibilityMenu = false;
            this.AMPM.ValueType = typeof(string);
            this.AMPM.Width = 70;
            // 
            // Site_Name
            // 
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle10.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Site_Name.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Site_Name.HeaderStyle = dataGridViewCellStyle11;
            this.Site_Name.HeaderText = "Site Name";
            this.Site_Name.Name = "Site_Name";
            this.Site_Name.ReadOnly = true;
            this.Filter.SetShowFilter(this.Site_Name, true);
            this.Site_Name.ValueType = typeof(string);
            this.Site_Name.Width = 240;
            // 
            // City
            // 
            dataGridViewCellStyle12.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle12.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.City.DefaultCellStyle = dataGridViewCellStyle12;
            dataGridViewCellStyle13.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.City.HeaderStyle = dataGridViewCellStyle13;
            this.City.HeaderText = "City";
            this.City.Name = "City";
            this.City.ReadOnly = true;
            this.Filter.SetShowFilter(this.City, true);
            this.City.ValueType = typeof(string);
            this.City.Width = 110;
            // 
            // Selected
            // 
            this.Selected.HeaderText = "Selected";
            this.Selected.Name = "Selected";
            this.Selected.ShowInVisibilityMenu = false;
            this.Selected.Visible = false;
            this.Selected.Width = 20;
            // 
            // pnlSelect
            // 
            this.pnlSelect.AppearanceKey = "panel-grdo";
            this.pnlSelect.Controls.Add(this.btnSelect);
            this.pnlSelect.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSelect.Location = new System.Drawing.Point(0, 345);
            this.pnlSelect.Name = "pnlSelect";
            this.pnlSelect.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSelect.Size = new System.Drawing.Size(648, 35);
            this.pnlSelect.TabIndex = 3;
            // 
            // btnSelect
            // 
            this.btnSelect.Dock = Wisej.Web.DockStyle.Right;
            this.btnSelect.Location = new System.Drawing.Point(568, 5);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(65, 25);
            this.btnSelect.TabIndex = 4;
            this.btnSelect.Text = "&Select";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // Filter
            // 
            this.Filter.FilterPanelType = typeof(Wisej.Web.Ext.ColumnFilter.WhereColumnFilterPanel);
            this.Filter.ImageSource = "grid-filter";
            // 
            // Site_SelectionForm
            // 
            this.ClientSize = new System.Drawing.Size(648, 380);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Site_SelectionForm";
            this.Text = "Site Selection Form";
            this.Load += new System.EventHandler(this.Site_SelectionForm_Load);
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlgvsite.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvsite)).EndInit();
            this.pnlSelect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Filter)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel pnlCompleteForm;
        private DataGridView gvsite;
        private DataGridViewTextBoxColumn Site_No;
        private DataGridViewTextBoxColumn Room;
        private DataGridViewTextBoxColumn AMPM;
        private DataGridViewTextBoxColumn Site_Name;
        private DataGridViewTextBoxColumn City;
        private Button btnSelect;
        private DataGridViewImageColumn Sel_Img;
        private DataGridViewTextBoxColumn Selected;
        private Panel pnlgvsite;
        private Panel pnlSelect;
        private Wisej.Web.Ext.ColumnFilter.ColumnFilter Filter;
    }
}