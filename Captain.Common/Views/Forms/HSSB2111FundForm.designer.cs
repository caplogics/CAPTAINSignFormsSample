using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class HSSB2111FundForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSSB2111FundForm));
            this.btnSelect = new Wisej.Web.Button();
            this.gvwFundSource = new Wisej.Web.DataGridView();
            this.Sel_Img = new Wisej.Web.DataGridViewImageColumn();
            this.gvtCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Selected = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlgvwFundSource = new Wisej.Web.Panel();
            this.pnlOK = new Wisej.Web.Panel();
            this.chkUnselectAll = new Wisej.Web.CheckBox();
            this.spacer2 = new Wisej.Web.Spacer();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.chkSelectAll = new Wisej.Web.CheckBox();
            this.Filter = new Wisej.Web.Ext.ColumnFilter.ColumnFilter(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gvwFundSource)).BeginInit();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlgvwFundSource.SuspendLayout();
            this.pnlOK.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Filter)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelect
            // 
            this.btnSelect.AppearanceKey = "button-ok";
            this.btnSelect.Dock = Wisej.Web.DockStyle.Right;
            this.btnSelect.Location = new System.Drawing.Point(367, 5);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(60, 25);
            this.btnSelect.TabIndex = 5;
            this.btnSelect.Text = "&OK";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // gvwFundSource
            // 
            this.gvwFundSource.AllowUserToResizeColumns = false;
            this.gvwFundSource.AllowUserToResizeRows = false;
            this.gvwFundSource.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwFundSource.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvwFundSource.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwFundSource.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwFundSource.ColumnHeadersHeight = 25;
            this.gvwFundSource.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwFundSource.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Sel_Img,
            this.gvtCode,
            this.gvtDesc,
            this.Selected});
            this.gvwFundSource.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwFundSource.Location = new System.Drawing.Point(0, 0);
            this.gvwFundSource.Name = "gvwFundSource";
            this.gvwFundSource.RowHeadersWidth = 10;
            this.gvwFundSource.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwFundSource.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwFundSource.Size = new System.Drawing.Size(520, 390);
            this.gvwFundSource.TabIndex = 1;
            this.gvwFundSource.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvsite_CellClick);
            this.gvwFundSource.DataError += new Wisej.Web.DataGridViewDataErrorEventHandler(this.gvwFundSource_DataError);
            // 
            // Sel_Img
            // 
            this.Sel_Img.CellImageLayout = Wisej.Web.DataGridViewImageCellLayout.Center;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Sel_Img.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Sel_Img.HeaderStyle = dataGridViewCellStyle3;
            this.Sel_Img.HeaderText = " Select";
            this.Sel_Img.Name = "Sel_Img";
            this.Sel_Img.ShowInVisibilityMenu = false;
            this.Sel_Img.Width = 45;
            // 
            // gvtCode
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtCode.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtCode.HeaderStyle = dataGridViewCellStyle5;
            this.gvtCode.HeaderText = "Code";
            this.gvtCode.Name = "gvtCode";
            this.gvtCode.ReadOnly = true;
            this.Filter.SetShowFilter(this.gvtCode, true);
            this.gvtCode.ValueType = typeof(string);
            this.gvtCode.Width = 90;
            // 
            // gvtDesc
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtDesc.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtDesc.HeaderStyle = dataGridViewCellStyle7;
            this.gvtDesc.HeaderText = "Description";
            this.gvtDesc.Name = "gvtDesc";
            this.gvtDesc.ReadOnly = true;
            this.Filter.SetShowFilter(this.gvtDesc, true);
            this.gvtDesc.ValueType = typeof(string);
            this.gvtDesc.Width = 330;
            // 
            // Selected
            // 
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Selected.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Selected.HeaderStyle = dataGridViewCellStyle9;
            this.Selected.HeaderText = "Selected";
            this.Selected.Name = "Selected";
            this.Selected.ShowInVisibilityMenu = false;
            this.Selected.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.Selected.Visible = false;
            this.Selected.Width = 20;
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlgvwFundSource);
            this.pnlCompleteForm.Controls.Add(this.pnlOK);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(520, 425);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlgvwFundSource
            // 
            this.pnlgvwFundSource.Controls.Add(this.gvwFundSource);
            this.pnlgvwFundSource.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwFundSource.Location = new System.Drawing.Point(0, 0);
            this.pnlgvwFundSource.Name = "pnlgvwFundSource";
            this.pnlgvwFundSource.Size = new System.Drawing.Size(520, 390);
            this.pnlgvwFundSource.TabIndex = 0;
            // 
            // pnlOK
            // 
            this.pnlOK.AppearanceKey = "panel-grdo";
            this.pnlOK.Controls.Add(this.chkUnselectAll);
            this.pnlOK.Controls.Add(this.spacer2);
            this.pnlOK.Controls.Add(this.btnSelect);
            this.pnlOK.Controls.Add(this.spacer1);
            this.pnlOK.Controls.Add(this.btnCancel);
            this.pnlOK.Controls.Add(this.chkSelectAll);
            this.pnlOK.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlOK.Location = new System.Drawing.Point(0, 390);
            this.pnlOK.Name = "pnlOK";
            this.pnlOK.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlOK.Size = new System.Drawing.Size(520, 35);
            this.pnlOK.TabIndex = 2;
            // 
            // chkUnselectAll
            // 
            this.chkUnselectAll.AutoSize = false;
            this.chkUnselectAll.Dock = Wisej.Web.DockStyle.Left;
            this.chkUnselectAll.Location = new System.Drawing.Point(100, 5);
            this.chkUnselectAll.Name = "chkUnselectAll";
            this.chkUnselectAll.Size = new System.Drawing.Size(93, 25);
            this.chkUnselectAll.TabIndex = 4;
            this.chkUnselectAll.Text = "Unselect All";
            this.chkUnselectAll.CheckedChanged += new System.EventHandler(this.chkUnselectAll_CheckedChanged);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(93, 5);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(7, 25);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(427, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(430, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = false;
            this.chkSelectAll.Dock = Wisej.Web.DockStyle.Left;
            this.chkSelectAll.Location = new System.Drawing.Point(15, 5);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(78, 25);
            this.chkSelectAll.TabIndex = 3;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // Filter
            // 
            this.Filter.FilterPanelType = typeof(Wisej.Web.Ext.ColumnFilter.WhereColumnFilterPanel);
            this.Filter.ImageSource = "grid-filter";
            // 
            // HSSB2111FundForm
            // 
            this.ClientSize = new System.Drawing.Size(520, 425);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSSB2111FundForm";
            this.Text = "Fund Form";
            ((System.ComponentModel.ISupportInitialize)(this.gvwFundSource)).EndInit();
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlgvwFundSource.ResumeLayout(false);
            this.pnlOK.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Filter)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnSelect;
        private DataGridView gvwFundSource;
        private DataGridViewImageColumn Sel_Img;
        private DataGridViewTextBoxColumn gvtCode;
        private DataGridViewTextBoxColumn gvtDesc;
        private DataGridViewTextBoxColumn Selected;
        private Panel pnlCompleteForm;
        private Button btnCancel;
        private CheckBox chkUnselectAll;
        private CheckBox chkSelectAll;
        private Panel pnlgvwFundSource;
        private Panel pnlOK;
        private Spacer spacer2;
        private Spacer spacer1;
        private Wisej.Web.Ext.ColumnFilter.ColumnFilter Filter;
    }
}