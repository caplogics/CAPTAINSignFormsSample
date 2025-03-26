using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.UserControls
{
    partial class ADMN0020control
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
            this.pnlSearch = new Wisej.Web.Panel();
            this.label2 = new Wisej.Web.Label();
            this.Cmb_Status = new Wisej.Web.ComboBox();
            this.CmbSPValid = new Wisej.Web.ComboBox();
            this.label1 = new Wisej.Web.Label();
            this.btnSearch = new Wisej.Web.Button();
            this.cmbBranch = new Wisej.Web.ComboBox();
            this.lblBranch = new Wisej.Web.Label();
            this.lblSerplnDesc = new Wisej.Web.Label();
            this.TxtSPDesc = new Wisej.Web.TextBox();
            this.Txt_SPCode = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblSerPlnCd = new Wisej.Web.Label();
            this.lblchngdate = new Wisej.Web.Label();
            this.LstcDate = new Wisej.Web.DateTimePicker();
            this.AddDate = new Wisej.Web.DateTimePicker();
            this.lblAddDt = new Wisej.Web.Label();
            this.pnlSPGrid = new Wisej.Web.Panel();
            this.SPGrid = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.PlanDescription = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Column0 = new Wisej.Web.DataGridViewColumn();
            this.Category = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Add_Date = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.Lstc_Date = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.ValidSW = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel3 = new Wisej.Web.Panel();
            this.BtnValidate = new Wisej.Web.Button();
            this.Btn_Update_Grps = new Wisej.Web.Button();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlGridBtns = new Wisej.Web.Panel();
            this.pnlScrolls = new Wisej.Web.Panel();
            this.pnlTop = new Wisej.Web.Panel();
            this.pnlSearch.SuspendLayout();
            this.pnlSPGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SPGrid)).BeginInit();
            this.panel3.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlGridBtns.SuspendLayout();
            this.pnlScrolls.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.label2);
            this.pnlSearch.Controls.Add(this.Cmb_Status);
            this.pnlSearch.Controls.Add(this.CmbSPValid);
            this.pnlSearch.Controls.Add(this.label1);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.cmbBranch);
            this.pnlSearch.Controls.Add(this.lblBranch);
            this.pnlSearch.Controls.Add(this.lblSerplnDesc);
            this.pnlSearch.Controls.Add(this.TxtSPDesc);
            this.pnlSearch.Controls.Add(this.Txt_SPCode);
            this.pnlSearch.Controls.Add(this.lblSerPlnCd);
            this.pnlSearch.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.pnlSearch.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(809, 71);
            this.pnlSearch.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(198, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 14);
            this.label2.TabIndex = 0;
            this.label2.Text = "Status";
            // 
            // Cmb_Status
            // 
            this.Cmb_Status.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.Cmb_Status.FormattingEnabled = true;
            this.Cmb_Status.Location = new System.Drawing.Point(240, 6);
            this.Cmb_Status.Name = "Cmb_Status";
            this.Cmb_Status.Size = new System.Drawing.Size(94, 25);
            this.Cmb_Status.TabIndex = 2;
            // 
            // CmbSPValid
            // 
            this.CmbSPValid.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbSPValid.FormattingEnabled = true;
            this.CmbSPValid.Location = new System.Drawing.Point(636, 6);
            this.CmbSPValid.Name = "CmbSPValid";
            this.CmbSPValid.Size = new System.Drawing.Size(94, 25);
            this.CmbSPValid.TabIndex = 4;
            this.CmbSPValid.SelectedIndexChanged += new System.EventHandler(this.CmbSPValid_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(552, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "SP Validation";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSearch.Location = new System.Drawing.Point(741, 41);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(70, 23);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "&Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cmbBranch
            // 
            this.cmbBranch.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbBranch.FormattingEnabled = true;
            this.cmbBranch.Location = new System.Drawing.Point(406, 6);
            this.cmbBranch.Name = "cmbBranch";
            this.cmbBranch.Size = new System.Drawing.Size(126, 25);
            this.cmbBranch.TabIndex = 3;
            // 
            // lblBranch
            // 
            this.lblBranch.AutoSize = true;
            this.lblBranch.Location = new System.Drawing.Point(353, 10);
            this.lblBranch.Name = "lblBranch";
            this.lblBranch.Size = new System.Drawing.Size(43, 14);
            this.lblBranch.TabIndex = 0;
            this.lblBranch.Text = "Branch";
            this.lblBranch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSerplnDesc
            // 
            this.lblSerplnDesc.AutoSize = true;
            this.lblSerplnDesc.Location = new System.Drawing.Point(15, 46);
            this.lblSerplnDesc.Name = "lblSerplnDesc";
            this.lblSerplnDesc.Size = new System.Drawing.Size(108, 14);
            this.lblSerplnDesc.TabIndex = 0;
            this.lblSerplnDesc.Text = "Service Plan DESC";
            // 
            // TxtSPDesc
            // 
            this.TxtSPDesc.Location = new System.Drawing.Point(125, 41);
            this.TxtSPDesc.MaxLength = 600;
            this.TxtSPDesc.Name = "TxtSPDesc";
            this.TxtSPDesc.Size = new System.Drawing.Size(605, 25);
            this.TxtSPDesc.TabIndex = 5;
            // 
            // Txt_SPCode
            // 
            this.Txt_SPCode.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Txt_SPCode.Location = new System.Drawing.Point(125, 6);
            this.Txt_SPCode.MaxLength = 6;
            this.Txt_SPCode.Name = "Txt_SPCode";
            this.Txt_SPCode.Size = new System.Drawing.Size(51, 25);
            this.Txt_SPCode.TabIndex = 1;
            this.Txt_SPCode.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.Txt_SPCode.LostFocus += new System.EventHandler(this.Txt_SPCode_LostFocus);
            // 
            // lblSerPlnCd
            // 
            this.lblSerPlnCd.AutoSize = true;
            this.lblSerPlnCd.Location = new System.Drawing.Point(15, 10);
            this.lblSerPlnCd.Name = "lblSerPlnCd";
            this.lblSerPlnCd.Size = new System.Drawing.Size(104, 14);
            this.lblSerPlnCd.TabIndex = 0;
            this.lblSerPlnCd.Text = "Service Plan Code";
            // 
            // lblchngdate
            // 
            this.lblchngdate.AutoSize = true;
            this.lblchngdate.Location = new System.Drawing.Point(411, 8);
            this.lblchngdate.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblchngdate.Name = "lblchngdate";
            this.lblchngdate.Size = new System.Drawing.Size(75, 18);
            this.lblchngdate.TabIndex = 0;
            this.lblchngdate.Text = "Change Date";
            this.lblchngdate.Visible = false;
            // 
            // LstcDate
            // 
            this.LstcDate.AutoSize = false;
            this.LstcDate.Checked = false;
            this.LstcDate.CustomFormat = "MM/dd/yyyy";
            this.LstcDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.LstcDate.Location = new System.Drawing.Point(493, 4);
            this.LstcDate.Name = "LstcDate";
            this.LstcDate.ShowCheckBox = true;
            this.LstcDate.ShowToolTips = false;
            this.LstcDate.Size = new System.Drawing.Size(116, 25);
            this.LstcDate.TabIndex = 3;
            this.LstcDate.Visible = false;
            // 
            // AddDate
            // 
            this.AddDate.AutoSize = false;
            this.AddDate.Checked = false;
            this.AddDate.CustomFormat = "MM/dd/yyyy";
            this.AddDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.AddDate.Location = new System.Drawing.Point(264, 4);
            this.AddDate.Name = "AddDate";
            this.AddDate.ShowCheckBox = true;
            this.AddDate.ShowToolTips = false;
            this.AddDate.Size = new System.Drawing.Size(116, 25);
            this.AddDate.TabIndex = 3;
            this.AddDate.Visible = false;
            // 
            // lblAddDt
            // 
            this.lblAddDt.AutoSize = true;
            this.lblAddDt.Location = new System.Drawing.Point(203, 10);
            this.lblAddDt.Name = "lblAddDt";
            this.lblAddDt.Size = new System.Drawing.Size(54, 14);
            this.lblAddDt.TabIndex = 0;
            this.lblAddDt.Text = "Add Date";
            this.lblAddDt.Visible = false;
            // 
            // pnlSPGrid
            // 
            this.pnlSPGrid.AutoScroll = true;
            this.pnlSPGrid.Controls.Add(this.SPGrid);
            this.pnlSPGrid.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlSPGrid.Location = new System.Drawing.Point(0, 0);
            this.pnlSPGrid.Name = "pnlSPGrid";
            this.pnlSPGrid.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.pnlSPGrid.Size = new System.Drawing.Size(809, 424);
            this.pnlSPGrid.TabIndex = 5;
            // 
            // SPGrid
            // 
            this.SPGrid.AllowUserToResizeColumns = false;
            this.SPGrid.AllowUserToResizeRows = false;
            this.SPGrid.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.SPGrid.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.MenuText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.SPGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.SPGrid.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SPGrid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Code,
            this.PlanDescription,
            this.Column0,
            this.Category,
            this.Add_Date,
            this.Lstc_Date,
            this.ValidSW});
            this.SPGrid.Dock = Wisej.Web.DockStyle.Fill;
            this.SPGrid.Name = "SPGrid";
            this.SPGrid.RowHeadersWidth = 25;
            this.SPGrid.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.SPGrid.Size = new System.Drawing.Size(809, 424);
            this.SPGrid.TabIndex = 1;
            this.SPGrid.SelectionChanged += new System.EventHandler(this.SPGrid_SelectionChanged);
            // 
            // Code
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.Code.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.Code.HeaderStyle = dataGridViewCellStyle3;
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            this.Code.Width = 80;
            // 
            // PlanDescription
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.PlanDescription.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.PlanDescription.HeaderStyle = dataGridViewCellStyle5;
            this.PlanDescription.HeaderText = "Service Plan Description";
            this.PlanDescription.Name = "PlanDescription";
            this.PlanDescription.ReadOnly = true;
            this.PlanDescription.Width = 480;
            // 
            // Column0
            // 
            dataGridViewCellStyle6.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Column0.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Column0.HeaderStyle = dataGridViewCellStyle7;
            this.Column0.HeaderText = "Department";
            this.Column0.Name = "Column0";
            this.Column0.Width = 250;
            // 
            // Category
            // 
            this.Category.HeaderText = "Category";
            this.Category.Name = "Category";
            this.Category.ShowInVisibilityMenu = false;
            this.Category.Visible = false;
            this.Category.Width = 60;
            // 
            // Add_Date
            // 
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Add_Date.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Add_Date.HeaderStyle = dataGridViewCellStyle9;
            this.Add_Date.HeaderText = "AddDate";
            this.Add_Date.Name = "Add_Date";
            this.Add_Date.ReadOnly = true;
            this.Add_Date.Width = 80;
            // 
            // Lstc_Date
            // 
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.Padding = new Wisej.Web.Padding(5, 0, 0, 0);
            this.Lstc_Date.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.Padding = new Wisej.Web.Padding(5, 0, 0, 0);
            this.Lstc_Date.HeaderStyle = dataGridViewCellStyle11;
            this.Lstc_Date.HeaderText = "ChgDate";
            this.Lstc_Date.Name = "Lstc_Date";
            this.Lstc_Date.ReadOnly = true;
            this.Lstc_Date.Width = 85;
            // 
            // ValidSW
            // 
            this.ValidSW.HeaderText = "ValidSW";
            this.ValidSW.Name = "ValidSW";
            this.ValidSW.ReadOnly = true;
            this.ValidSW.ShowInVisibilityMenu = false;
            this.ValidSW.Visible = false;
            // 
            // panel3
            // 
            this.panel3.AppearanceKey = "panel-grdo";
            this.panel3.Controls.Add(this.BtnValidate);
            this.panel3.Controls.Add(this.AddDate);
            this.panel3.Controls.Add(this.Btn_Update_Grps);
            this.panel3.Controls.Add(this.lblAddDt);
            this.panel3.Controls.Add(this.lblchngdate);
            this.panel3.Controls.Add(this.LstcDate);
            this.panel3.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 424);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.panel3.Size = new System.Drawing.Size(809, 35);
            this.panel3.TabIndex = 4;
            // 
            // BtnValidate
            // 
            this.BtnValidate.Dock = Wisej.Web.DockStyle.Right;
            this.BtnValidate.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.BtnValidate.Location = new System.Drawing.Point(650, 5);
            this.BtnValidate.Name = "BtnValidate";
            this.BtnValidate.Size = new System.Drawing.Size(144, 25);
            this.BtnValidate.TabIndex = 2;
            this.BtnValidate.Text = "&Validate Service Plan";
            this.BtnValidate.Visible = false;
            this.BtnValidate.Click += new System.EventHandler(this.BtnValidate_Click);
            // 
            // Btn_Update_Grps
            // 
            this.Btn_Update_Grps.Dock = Wisej.Web.DockStyle.Left;
            this.Btn_Update_Grps.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Btn_Update_Grps.Location = new System.Drawing.Point(15, 5);
            this.Btn_Update_Grps.Name = "Btn_Update_Grps";
            this.Btn_Update_Grps.Size = new System.Drawing.Size(157, 25);
            this.Btn_Update_Grps.TabIndex = 2;
            this.Btn_Update_Grps.Text = "&Update Groups in Posting";
            this.Btn_Update_Grps.Visible = false;
            this.Btn_Update_Grps.Click += new System.EventHandler(this.Btn_Update_Grps_Click);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlGridBtns);
            this.pnlCompleteForm.Controls.Add(this.pnlTop);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 25);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Padding = new Wisej.Web.Padding(10);
            this.pnlCompleteForm.Size = new System.Drawing.Size(829, 555);
            this.pnlCompleteForm.TabIndex = 2;
            // 
            // pnlGridBtns
            // 
            this.pnlGridBtns.Controls.Add(this.pnlScrolls);
            this.pnlGridBtns.Controls.Add(this.panel3);
            this.pnlGridBtns.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.pnlGridBtns.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlGridBtns.Location = new System.Drawing.Point(10, 86);
            this.pnlGridBtns.Name = "pnlGridBtns";
            this.pnlGridBtns.Size = new System.Drawing.Size(809, 459);
            this.pnlGridBtns.TabIndex = 6;
            // 
            // pnlScrolls
            // 
            this.pnlScrolls.AutoScroll = true;
            this.pnlScrolls.Controls.Add(this.pnlSPGrid);
            this.pnlScrolls.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlScrolls.Location = new System.Drawing.Point(0, 0);
            this.pnlScrolls.Name = "pnlScrolls";
            this.pnlScrolls.ScrollBars = Wisej.Web.ScrollBars.Horizontal;
            this.pnlScrolls.Size = new System.Drawing.Size(809, 424);
            this.pnlScrolls.TabIndex = 6;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.pnlSearch);
            this.pnlTop.Dock = Wisej.Web.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(10, 10);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new Wisej.Web.Padding(0, 0, 0, 5);
            this.pnlTop.Size = new System.Drawing.Size(809, 76);
            this.pnlTop.TabIndex = 7;
            // 
            // ADMN0020control
            // 
            this.Controls.Add(this.pnlCompleteForm);
            this.Name = "ADMN0020control";
            this.Size = new System.Drawing.Size(829, 580);
            this.Load += new System.EventHandler(this.ADMN0020control_Load);
            this.Controls.SetChildIndex(this.pnlCompleteForm, 0);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlSPGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SPGrid)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlGridBtns.ResumeLayout(false);
            this.pnlScrolls.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel pnlSearch;
        private Button btnSearch;
        private Label lblchngdate;
        private DateTimePicker LstcDate;
        private DateTimePicker AddDate;
        private Label lblAddDt;
        private ComboBox cmbBranch;
        private Label lblBranch;
        private Label lblSerplnDesc;
        private TextBox TxtSPDesc;
        private TextBoxWithValidation Txt_SPCode;
        private Label lblSerPlnCd;
        private DataGridViewEx SPGrid;
        private DataGridViewTextBoxColumn Code;
        private DataGridViewTextBoxColumn PlanDescription;
        private Button BtnValidate;
        private ComboBox CmbSPValid;
        private Label label1;
        private DataGridViewTextBoxColumn ValidSW;
        private Label label2;
        private ComboBox Cmb_Status;
        private Button Btn_Update_Grps;
        private DataGridViewTextBoxColumn Category;
        private Panel pnlSPGrid;
        private Panel panel3;
        private DataGridViewDateTimeColumn Add_Date;
        private DataGridViewDateTimeColumn Lstc_Date;
        private Panel pnlCompleteForm;
        private Panel pnlGridBtns;
        private Panel pnlTop;
        private Panel pnlScrolls;
        private DataGridViewColumn Column0;
    }
}