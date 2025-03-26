using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class VendBrowseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VendBrowseForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.pnlSearch = new Wisej.Web.Panel();
            this.btnEdit = new Wisej.Web.PictureBox();
            this.btnAdd = new Wisej.Web.PictureBox();
            this.btnSearch = new Wisej.Web.Button();
            this.cmbSource = new Wisej.Web.ComboBox();
            this.lblSource = new Wisej.Web.Label();
            this.txtName = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblSearch = new Wisej.Web.Label();
            this.gvwVendor = new Wisej.Web.DataGridView();
            this.gvchkSel = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvtNumber = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtName2 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtAddress = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtActive = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlSelect = new Wisej.Web.Panel();
            this.lblTotal = new Wisej.Web.Label();
            this.btnSelect = new Wisej.Web.Button();
            this.lblTotNoRec = new Wisej.Web.Label();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlGvwVendor = new Wisej.Web.Panel();
            this.ActiveFilter = new Wisej.Web.Ext.ColumnFilter.ColumnFilter(this.components);
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwVendor)).BeginInit();
            this.pnlSelect.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlGvwVendor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ActiveFilter)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.btnEdit);
            this.pnlSearch.Controls.Add(this.btnAdd);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.cmbSource);
            this.pnlSearch.Controls.Add(this.lblSource);
            this.pnlSearch.Controls.Add(this.txtName);
            this.pnlSearch.Controls.Add(this.lblSearch);
            this.pnlSearch.Dock = Wisej.Web.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(879, 74);
            this.pnlSearch.TabIndex = 1;
            this.pnlSearch.TabStop = true;
            // 
            // btnEdit
            // 
            this.btnEdit.Cursor = Wisej.Web.Cursors.Hand;
            this.btnEdit.ImageSource = "captain-edit";
            this.btnEdit.Location = new System.Drawing.Point(826, 44);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(20, 20);
            this.btnEdit.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.btnEdit.Visible = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Cursor = Wisej.Web.Cursors.Hand;
            this.btnAdd.ImageSource = "captain-add";
            this.btnAdd.Location = new System.Drawing.Point(826, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(20, 20);
            this.btnAdd.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.btnAdd.Visible = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSearch.Location = new System.Drawing.Point(745, 41);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 25);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "S&earch";
            this.btnSearch.ToolTipText = "Search Vendor";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cmbSource
            // 
            this.cmbSource.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbSource.FormattingEnabled = true;
            this.cmbSource.Location = new System.Drawing.Point(79, 41);
            this.cmbSource.Name = "cmbSource";
            this.cmbSource.Size = new System.Drawing.Size(662, 25);
            this.cmbSource.TabIndex = 2;
            // 
            // lblSource
            // 
            this.lblSource.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSource.Location = new System.Drawing.Point(15, 44);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(40, 16);
            this.lblSource.TabIndex = 4;
            this.lblSource.Text = "Source";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(79, 9);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(740, 25);
            this.txtName.TabIndex = 1;
            this.txtName.Enter += new System.EventHandler(this.txtName_Enter);
            this.txtName.Leave += new System.EventHandler(this.txtName_Leave);
            this.txtName.KeyDown += new Wisej.Web.KeyEventHandler(this.txtName_EnterKeyDown);
            // 
            // lblSearch
            // 
            this.lblSearch.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSearch.Location = new System.Drawing.Point(15, 13);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(56, 16);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Search By";
            // 
            // gvwVendor
            // 
            this.gvwVendor.AllowUserToResizeColumns = false;
            this.gvwVendor.AllowUserToResizeRows = false;
            this.gvwVendor.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwVendor.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("default", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwVendor.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwVendor.ColumnHeadersHeight = 25;
            this.gvwVendor.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvchkSel,
            this.gvtNumber,
            this.gvtName,
            this.gvtName2,
            this.gvtAddress,
            this.gvtActive});
            this.gvwVendor.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwVendor.Location = new System.Drawing.Point(0, 0);
            this.gvwVendor.MultiSelect = false;
            this.gvwVendor.Name = "gvwVendor";
            this.gvwVendor.RowHeadersWidth = 14;
            this.gvwVendor.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwVendor.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwVendor.Size = new System.Drawing.Size(879, 364);
            this.gvwVendor.TabIndex = 2;
            this.gvwVendor.DataError += new Wisej.Web.DataGridViewDataErrorEventHandler(this.gvwVendor_DataError);
            // 
            // gvchkSel
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.NullValue = false;
            this.gvchkSel.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvchkSel.HeaderStyle = dataGridViewCellStyle3;
            this.gvchkSel.HeaderText = "   ";
            this.gvchkSel.Name = "gvchkSel";
            this.gvchkSel.ShowInVisibilityMenu = false;
            this.gvchkSel.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.gvchkSel.Visible = false;
            this.gvchkSel.Width = 40;
            // 
            // gvtNumber
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtNumber.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtNumber.HeaderStyle = dataGridViewCellStyle5;
            this.gvtNumber.HeaderText = "Number";
            this.gvtNumber.Name = "gvtNumber";
            this.gvtNumber.ReadOnly = true;
            this.ActiveFilter.SetShowFilter(this.gvtNumber, true);
            this.gvtNumber.ValueType = typeof(string);
            // 
            // gvtName
            // 
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtName.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtName.HeaderStyle = dataGridViewCellStyle7;
            this.gvtName.HeaderText = "Name";
            this.gvtName.Name = "gvtName";
            this.gvtName.ReadOnly = true;
            this.ActiveFilter.SetShowFilter(this.gvtName, true);
            this.gvtName.ValueType = typeof(string);
            this.gvtName.Width = 230;
            // 
            // gvtName2
            // 
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle8.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtName2.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtName2.HeaderStyle = dataGridViewCellStyle9;
            this.gvtName2.HeaderText = "Name2";
            this.gvtName2.Name = "gvtName2";
            this.gvtName2.ReadOnly = true;
            this.ActiveFilter.SetShowFilter(this.gvtName2, true);
            this.gvtName2.ValueType = typeof(string);
            this.gvtName2.Width = 230;
            // 
            // gvtAddress
            // 
            dataGridViewCellStyle10.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle10.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtAddress.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtAddress.HeaderStyle = dataGridViewCellStyle11;
            this.gvtAddress.HeaderText = "Address";
            this.gvtAddress.Name = "gvtAddress";
            this.gvtAddress.ReadOnly = true;
            this.ActiveFilter.SetShowFilter(this.gvtAddress, true);
            this.gvtAddress.ValueType = typeof(string);
            this.gvtAddress.Width = 250;
            // 
            // gvtActive
            // 
            dataGridViewCellStyle12.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtActive.DefaultCellStyle = dataGridViewCellStyle12;
            dataGridViewCellStyle13.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtActive.HeaderStyle = dataGridViewCellStyle13;
            this.gvtActive.HeaderText = "gvtActive";
            this.gvtActive.Name = "gvtActive";
            this.gvtActive.ReadOnly = true;
            this.gvtActive.ShowInVisibilityMenu = false;
            this.gvtActive.Visible = false;
            this.gvtActive.Width = 10;
            // 
            // pnlSelect
            // 
            this.pnlSelect.AppearanceKey = "panel-grdo";
            this.pnlSelect.Controls.Add(this.lblTotal);
            this.pnlSelect.Controls.Add(this.btnSelect);
            this.pnlSelect.Controls.Add(this.lblTotNoRec);
            this.pnlSelect.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSelect.Location = new System.Drawing.Point(0, 438);
            this.pnlSelect.Name = "pnlSelect";
            this.pnlSelect.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSelect.Size = new System.Drawing.Size(879, 35);
            this.pnlSelect.TabIndex = 3;
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblTotal.Location = new System.Drawing.Point(15, 11);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(84, 16);
            this.lblTotal.TabIndex = 4;
            this.lblTotal.Text = "Total Records :";
            this.lblTotal.Visible = false;
            // 
            // btnSelect
            // 
            this.btnSelect.Dock = Wisej.Web.DockStyle.Right;
            this.btnSelect.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSelect.Location = new System.Drawing.Point(789, 5);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 25);
            this.btnSelect.TabIndex = 4;
            this.btnSelect.Text = "&Select";
            this.btnSelect.Visible = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // lblTotNoRec
            // 
            this.lblTotNoRec.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblTotNoRec.Location = new System.Drawing.Point(104, 11);
            this.lblTotNoRec.Name = "lblTotNoRec";
            this.lblTotNoRec.Size = new System.Drawing.Size(102, 16);
            this.lblTotNoRec.TabIndex = 2;
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlGvwVendor);
            this.pnlCompleteForm.Controls.Add(this.pnlSelect);
            this.pnlCompleteForm.Controls.Add(this.pnlSearch);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(879, 473);
            this.pnlCompleteForm.TabIndex = 2;
            // 
            // pnlGvwVendor
            // 
            this.pnlGvwVendor.Controls.Add(this.gvwVendor);
            this.pnlGvwVendor.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlGvwVendor.Location = new System.Drawing.Point(0, 74);
            this.pnlGvwVendor.Name = "pnlGvwVendor";
            this.pnlGvwVendor.Size = new System.Drawing.Size(879, 364);
            this.pnlGvwVendor.TabIndex = 8;
            // 
            // ActiveFilter
            // 
            this.ActiveFilter.FilterPanelType = typeof(Wisej.Web.Ext.ColumnFilter.WhereColumnFilterPanel);
            this.ActiveFilter.ImageSource = "grid-filter";
            // 
            // VendBrowseForm
            // 
            this.ClientSize = new System.Drawing.Size(879, 473);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VendBrowseForm";
            this.Text = "Vendor Browser";
            componentTool1.ImageSource = "icon-help";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwVendor)).EndInit();
            this.pnlSelect.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlGvwVendor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ActiveFilter)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel pnlSearch;
        private Label lblSearch;
        private TextBoxWithValidation txtName;
        private ComboBox cmbSource;
        private Label lblSource;
        private Button btnSearch;
        private Label lblTotNoRec;
        private PictureBox btnEdit;
        private PictureBox btnAdd;
        private DataGridView gvwVendor;
        private DataGridViewCheckBoxColumn gvchkSel;
        private DataGridViewTextBoxColumn gvtName;
        private DataGridViewTextBoxColumn gvtName2;
        private DataGridViewTextBoxColumn gvtAddress;
        private DataGridViewTextBoxColumn gvtActive;
        private Panel pnlSelect;
        private Button btnSelect;
        private Label lblTotal;
        private Panel pnlCompleteForm;
        private Panel pnlGvwVendor;
        private DataGridViewTextBoxColumn gvtNumber;
        private Wisej.Web.Ext.ColumnFilter.ColumnFilter ActiveFilter;
    }
}