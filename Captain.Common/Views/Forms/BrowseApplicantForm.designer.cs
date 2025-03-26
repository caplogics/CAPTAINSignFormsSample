using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class BrowseApplicantForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle11 = new Wisej.Web.DataGridViewCellStyle();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowseApplicantForm));
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlgvwMstDetails = new Wisej.Web.Panel();
            this.gvwMstDetails = new Wisej.Web.DataGridView();
            this.gvtAppNo = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtAppName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtAddress = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtCity = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtState = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlSearch = new Wisej.Web.Panel();
            this.txtApplicantNo = new Wisej.Web.TextBox();
            this.btnSearch = new Wisej.Web.Button();
            this.label1 = new Wisej.Web.Label();
            this.txtFirstName = new Wisej.Web.TextBox();
            this.lblFirstName = new Wisej.Web.Label();
            this.lblSSN = new Wisej.Web.Label();
            this.mskSSN = new Wisej.Web.MaskedTextBox();
            this.pnlSelect = new Wisej.Web.Panel();
            this.btnSelect = new Wisej.Web.Button();
            this.Filter = new Wisej.Web.Ext.ColumnFilter.ColumnFilter(this.components);
            this.pnlCompleteForm.SuspendLayout();
            this.pnlgvwMstDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwMstDetails)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.pnlSelect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Filter)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlgvwMstDetails);
            this.pnlCompleteForm.Controls.Add(this.pnlSearch);
            this.pnlCompleteForm.Controls.Add(this.pnlSelect);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(698, 326);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlgvwMstDetails
            // 
            this.pnlgvwMstDetails.Controls.Add(this.gvwMstDetails);
            this.pnlgvwMstDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwMstDetails.Location = new System.Drawing.Point(0, 43);
            this.pnlgvwMstDetails.Name = "pnlgvwMstDetails";
            this.pnlgvwMstDetails.Size = new System.Drawing.Size(698, 251);
            this.pnlgvwMstDetails.TabIndex = 6;
            // 
            // gvwMstDetails
            // 
            this.gvwMstDetails.AllowUserToResizeColumns = false;
            this.gvwMstDetails.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwMstDetails.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvwMstDetails.BorderStyle = Wisej.Web.BorderStyle.None;
            this.gvwMstDetails.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtAppNo,
            this.gvtAppName,
            this.gvtAddress,
            this.gvtCity,
            this.gvtState});
            this.gvwMstDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwMstDetails.Location = new System.Drawing.Point(0, 0);
            this.gvwMstDetails.MultiSelect = false;
            this.gvwMstDetails.Name = "gvwMstDetails";
            this.gvwMstDetails.ReadOnly = true;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvwMstDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.gvwMstDetails.RowHeadersWidth = 14;
            this.gvwMstDetails.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwMstDetails.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvwMstDetails.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwMstDetails.Size = new System.Drawing.Size(698, 251);
            this.gvwMstDetails.TabIndex = 7;
            this.gvwMstDetails.DataError += new Wisej.Web.DataGridViewDataErrorEventHandler(this.gvwMstDetails_DataError);
            // 
            // gvtAppNo
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtAppNo.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtAppNo.HeaderStyle = dataGridViewCellStyle2;
            this.gvtAppNo.HeaderText = "Applicant Number";
            this.gvtAppNo.Name = "gvtAppNo";
            this.gvtAppNo.ReadOnly = true;
            this.Filter.SetShowFilter(this.gvtAppNo, true);
            this.gvtAppNo.ValueType = typeof(string);
            this.gvtAppNo.Width = 153;
            // 
            // gvtAppName
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtAppName.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtAppName.HeaderStyle = dataGridViewCellStyle4;
            this.gvtAppName.HeaderText = "Name";
            this.gvtAppName.Name = "gvtAppName";
            this.gvtAppName.ReadOnly = true;
            this.Filter.SetShowFilter(this.gvtAppName, true);
            this.gvtAppName.ValueType = typeof(string);
            this.gvtAppName.Width = 160;
            // 
            // gvtAddress
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle5.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtAddress.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtAddress.HeaderStyle = dataGridViewCellStyle6;
            this.gvtAddress.HeaderText = "Address";
            this.gvtAddress.Name = "gvtAddress";
            this.gvtAddress.ReadOnly = true;
            this.Filter.SetShowFilter(this.gvtAddress, true);
            this.gvtAddress.ValueType = typeof(string);
            this.gvtAddress.Width = 160;
            // 
            // gvtCity
            // 
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle7.Padding = new Wisej.Web.Padding(10, 0, 0, 0);
            dataGridViewCellStyle7.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtCity.DefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle8.Padding = new Wisej.Web.Padding(10, 0, 0, 0);
            this.gvtCity.HeaderStyle = dataGridViewCellStyle8;
            this.gvtCity.HeaderText = "City";
            this.gvtCity.Name = "gvtCity";
            this.gvtCity.ReadOnly = true;
            this.Filter.SetShowFilter(this.gvtCity, true);
            this.gvtCity.ValueType = typeof(string);
            this.gvtCity.Width = 120;
            // 
            // gvtState
            // 
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtState.DefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtState.HeaderStyle = dataGridViewCellStyle10;
            this.gvtState.HeaderText = "State";
            this.gvtState.Name = "gvtState";
            this.gvtState.ReadOnly = true;
            this.Filter.SetShowFilter(this.gvtState, true);
            this.gvtState.ValueType = typeof(string);
            this.gvtState.Width = 75;
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.txtApplicantNo);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.label1);
            this.pnlSearch.Controls.Add(this.txtFirstName);
            this.pnlSearch.Controls.Add(this.lblFirstName);
            this.pnlSearch.Controls.Add(this.lblSSN);
            this.pnlSearch.Controls.Add(this.mskSSN);
            this.pnlSearch.Dock = Wisej.Web.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(698, 43);
            this.pnlSearch.TabIndex = 1;
            // 
            // txtApplicantNo
            // 
            this.txtApplicantNo.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtApplicantNo.Location = new System.Drawing.Point(230, 11);
            this.txtApplicantNo.MaxLength = 40;
            this.txtApplicantNo.Name = "txtApplicantNo";
            this.txtApplicantNo.Size = new System.Drawing.Size(70, 25);
            this.txtApplicantNo.TabIndex = 3;
            this.txtApplicantNo.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtApplicantNo.Leave += new System.EventHandler(this.txtApplicantNo_Leave);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(535, 11);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 25);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "S&earch";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(149, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Applicant No";
            // 
            // txtFirstName
            // 
            this.txtFirstName.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtFirstName.Location = new System.Drawing.Point(384, 11);
            this.txtFirstName.MaxLength = 40;
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(143, 25);
            this.txtFirstName.TabIndex = 4;
            // 
            // lblFirstName
            // 
            this.lblFirstName.Location = new System.Drawing.Point(313, 15);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(61, 14);
            this.lblFirstName.TabIndex = 0;
            this.lblFirstName.Text = "Last Name";
            // 
            // lblSSN
            // 
            this.lblSSN.Location = new System.Drawing.Point(15, 15);
            this.lblSSN.Name = "lblSSN";
            this.lblSSN.Size = new System.Drawing.Size(31, 14);
            this.lblSSN.TabIndex = 0;
            this.lblSSN.Text = "SSN#";
            // 
            // mskSSN
            // 
            this.mskSSN.Location = new System.Drawing.Point(56, 11);
            this.mskSSN.Mask = "000-00-0000";
            this.mskSSN.Name = "mskSSN";
            this.mskSSN.Size = new System.Drawing.Size(79, 25);
            this.mskSSN.TabIndex = 2;
            this.mskSSN.TextMaskFormat = Wisej.Web.MaskFormat.ExcludePromptAndLiterals;
            // 
            // pnlSelect
            // 
            this.pnlSelect.AppearanceKey = "panel-grdo";
            this.pnlSelect.Controls.Add(this.btnSelect);
            this.pnlSelect.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSelect.Location = new System.Drawing.Point(0, 294);
            this.pnlSelect.Name = "pnlSelect";
            this.pnlSelect.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSelect.Size = new System.Drawing.Size(698, 32);
            this.pnlSelect.TabIndex = 8;
            // 
            // btnSelect
            // 
            this.btnSelect.AppearanceKey = "button-reports";
            this.btnSelect.Dock = Wisej.Web.DockStyle.Right;
            this.btnSelect.Location = new System.Drawing.Point(618, 5);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(65, 22);
            this.btnSelect.TabIndex = 9;
            this.btnSelect.Text = "&Select";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // Filter
            // 
            this.Filter.FilterPanelType = typeof(Wisej.Web.Ext.ColumnFilter.WhereColumnFilterPanel);
            this.Filter.ImageSource = "grid-filter";
            // 
            // BrowseApplicantForm
            // 
            this.ClientSize = new System.Drawing.Size(698, 326);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.Name = "BrowseApplicantForm";
            this.Text = "Browse Applicant Form";
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlgvwMstDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwMstDetails)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlSelect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Filter)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel pnlCompleteForm;
        private DataGridView gvwMstDetails;
        private Panel pnlSelect;
        private Button btnSelect;
        private DataGridViewTextBoxColumn gvtAppNo;
        private DataGridViewTextBoxColumn gvtAppName;
        private DataGridViewTextBoxColumn gvtAddress;
        private DataGridViewTextBoxColumn gvtCity;
        private DataGridViewTextBoxColumn gvtState;
        private TextBox txtApplicantNo;
        private Label label1;
        private Label lblFirstName;
        private MaskedTextBox mskSSN;
        private Label lblSSN;
        private TextBox txtFirstName;
        private Button btnSearch;
        private Panel pnlgvwMstDetails;
        private Panel pnlSearch;
        private Wisej.Web.Ext.ColumnFilter.ColumnFilter Filter;
    }
}