using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class SelectZipSiteCountyForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectZipSiteCountyForm));
            this.btnOk = new Wisej.Web.Button();
            this.btnCancel = new Wisej.Web.Button();
            this.gvwZipSiteCounty = new Wisej.Web.DataGridView();
            this.Select = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Description = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Active = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlgvwZipSiteCounty = new Wisej.Web.Panel();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlOK = new Wisej.Web.Panel();
            this.chkbInactive = new Wisej.Web.CheckBox();
            this.chkbActive = new Wisej.Web.CheckBox();
            this.spacer1 = new Wisej.Web.Spacer();
            this.ActiveFilter = new Wisej.Web.Ext.ColumnFilter.ColumnFilter(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gvwZipSiteCounty)).BeginInit();
            this.pnlgvwZipSiteCounty.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlOK.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ActiveFilter)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(319, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(60, 25);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "&OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(382, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gvwZipSiteCounty
            // 
            this.gvwZipSiteCounty.AllowUserToResizeColumns = false;
            this.gvwZipSiteCounty.AllowUserToResizeRows = false;
            this.gvwZipSiteCounty.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwZipSiteCounty.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwZipSiteCounty.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwZipSiteCounty.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Select,
            this.Code,
            this.Description,
            this.Active});
            this.gvwZipSiteCounty.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwZipSiteCounty.Location = new System.Drawing.Point(0, 0);
            this.gvwZipSiteCounty.MultiSelect = false;
            this.gvwZipSiteCounty.Name = "gvwZipSiteCounty";
            this.gvwZipSiteCounty.RowHeadersVisible = false;
            this.gvwZipSiteCounty.RowHeadersWidth = 14;
            this.gvwZipSiteCounty.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwZipSiteCounty.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwZipSiteCounty.Size = new System.Drawing.Size(472, 323);
            this.gvwZipSiteCounty.TabIndex = 2;
            this.gvwZipSiteCounty.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvwZipSiteCounty_CellValueChanged);
            this.gvwZipSiteCounty.DataError += new Wisej.Web.DataGridViewDataErrorEventHandler(this.gvwZipSiteCounty_DataError);
            // 
            // Select
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.NullValue = false;
            this.Select.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Select.HeaderStyle = dataGridViewCellStyle3;
            this.Select.HeaderText = "Select";
            this.Select.Name = "Select";
            this.Select.ShowInVisibilityMenu = false;
            this.Select.Width = 70;
            // 
            // Code
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Code.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Code.HeaderStyle = dataGridViewCellStyle5;
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            this.Code.Resizable = Wisej.Web.DataGridViewTriState.True;
            this.ActiveFilter.SetShowFilter(this.Code, true);
            this.Code.ValueType = typeof(string);
            this.Code.Width = 75;
            // 
            // Description
            // 
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Description.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Description.HeaderStyle = dataGridViewCellStyle7;
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.ActiveFilter.SetShowFilter(this.Description, true);
            this.Description.ValueType = typeof(string);
            this.Description.Width = 310;
            // 
            // Active
            // 
            this.Active.HeaderText = "Status";
            this.Active.Name = "Active";
            this.ActiveFilter.SetShowFilter(this.Active, true);
            this.Active.ShowInVisibilityMenu = false;
            this.Active.ValueType = typeof(string);
            this.Active.Visible = false;
            this.Active.Width = 130;
            // 
            // pnlgvwZipSiteCounty
            // 
            this.pnlgvwZipSiteCounty.Controls.Add(this.gvwZipSiteCounty);
            this.pnlgvwZipSiteCounty.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwZipSiteCounty.Location = new System.Drawing.Point(0, 0);
            this.pnlgvwZipSiteCounty.Name = "pnlgvwZipSiteCounty";
            this.pnlgvwZipSiteCounty.Size = new System.Drawing.Size(472, 323);
            this.pnlgvwZipSiteCounty.TabIndex = 1;
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlgvwZipSiteCounty);
            this.pnlCompleteForm.Controls.Add(this.pnlOK);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(472, 358);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlOK
            // 
            this.pnlOK.AppearanceKey = "panel-grdo";
            this.pnlOK.Controls.Add(this.chkbInactive);
            this.pnlOK.Controls.Add(this.chkbActive);
            this.pnlOK.Controls.Add(this.btnOk);
            this.pnlOK.Controls.Add(this.spacer1);
            this.pnlOK.Controls.Add(this.btnCancel);
            this.pnlOK.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlOK.Location = new System.Drawing.Point(0, 323);
            this.pnlOK.Name = "pnlOK";
            this.pnlOK.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlOK.Size = new System.Drawing.Size(472, 35);
            this.pnlOK.TabIndex = 3;
            // 
            // chkbInactive
            // 
            this.chkbInactive.BackColor = System.Drawing.Color.Transparent;
            this.chkbInactive.Location = new System.Drawing.Point(139, 8);
            this.chkbInactive.Name = "chkbInactive";
            this.chkbInactive.Size = new System.Drawing.Size(127, 21);
            this.chkbInactive.TabIndex = 7;
            this.chkbInactive.Text = "Select All Inactive";
            this.chkbInactive.Visible = false;
            this.chkbInactive.CheckedChanged += new System.EventHandler(this.chkbInactive_CheckedChanged);
            // 
            // chkbActive
            // 
            this.chkbActive.BackColor = System.Drawing.Color.Transparent;
            this.chkbActive.Location = new System.Drawing.Point(3, 8);
            this.chkbActive.Name = "chkbActive";
            this.chkbActive.Size = new System.Drawing.Size(119, 21);
            this.chkbActive.TabIndex = 6;
            this.chkbActive.Text = "Select All Active";
            this.chkbActive.Visible = false;
            this.chkbActive.CheckedChanged += new System.EventHandler(this.chkbActive_CheckedChanged);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(379, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // ActiveFilter
            // 
            this.ActiveFilter.FilterPanelType = typeof(Wisej.Web.Ext.ColumnFilter.WhereColumnFilterPanel);
            this.ActiveFilter.ImageSource = "grid-filter";
            // 
            // SelectZipSiteCountyForm
            // 
            this.ClientSize = new System.Drawing.Size(472, 358);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectZipSiteCountyForm";
            this.Text = "Select County";
            ((System.ComponentModel.ISupportInitialize)(this.gvwZipSiteCounty)).EndInit();
            this.pnlgvwZipSiteCounty.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlOK.ResumeLayout(false);
            this.pnlOK.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ActiveFilter)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnOk;
        private Button btnCancel;
        private DataGridView gvwZipSiteCounty;
        private DataGridViewCheckBoxColumn Select;
        private DataGridViewTextBoxColumn Code;
        private DataGridViewTextBoxColumn Description;
        private Panel pnlgvwZipSiteCounty;
        private Panel pnlCompleteForm;
        private Panel pnlOK;
        private Spacer spacer1;
        private DataGridViewTextBoxColumn Active;
        private Wisej.Web.Ext.ColumnFilter.ColumnFilter ActiveFilter;
        private CheckBox chkbInactive;
        private CheckBox chkbActive;
    }
}