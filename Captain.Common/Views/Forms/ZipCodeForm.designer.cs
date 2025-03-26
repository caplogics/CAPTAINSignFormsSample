//using Wisej.Web;
//using Gizmox.WebGUI.Common;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class ZipCodeForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZipCodeForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.pnlUser = new Wisej.Web.Panel();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlGrid = new Wisej.Web.Panel();
            this.grpApplication = new Wisej.Web.GroupBox();
            this.grdApplications = new Wisej.Web.DataGridView();
            this.chkAppcode = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.AppCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlZipcode = new Wisej.Web.Panel();
            this.chkbInActive = new Wisej.Web.CheckBox();
            this.lblreqState = new Wisej.Web.Label();
            this.lblreqCity = new Wisej.Web.Label();
            this.lblreqZip = new Wisej.Web.Label();
            this.cmbYear = new Wisej.Web.ComboBox();
            this.cmbDay = new Wisej.Web.ComboBox();
            this.cmbMonth = new Wisej.Web.ComboBox();
            this.lblYear = new Wisej.Web.Label();
            this.lblDay = new Wisej.Web.Label();
            this.lblMonth = new Wisej.Web.Label();
            this.txtIntakecode = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblIntakecode = new Wisej.Web.Label();
            this.lblZipCode = new Wisej.Web.Label();
            this.lblTownShip = new Wisej.Web.Label();
            this.lblCity = new Wisej.Web.Label();
            this.lblState = new Wisej.Web.Label();
            this.lblHeadStartDate = new Wisej.Web.Label();
            this.lblCountry = new Wisej.Web.Label();
            this.txtZipCode = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtZipCode1 = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.cmbTownship = new Wisej.Web.ComboBox();
            this.txtCity = new Wisej.Web.TextBox();
            this.txtState = new Wisej.Web.TextBox();
            this.cmbCountry = new Wisej.Web.ComboBox();
            this.pnlSave = new Wisej.Web.Panel();
            this.btnSubmit = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.listBox1 = new Wisej.Web.ListBox();
            this.button1 = new Wisej.Web.Button();
            this.button2 = new Wisej.Web.Button();
            this.pnlUser.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            this.grpApplication.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdApplications)).BeginInit();
            this.pnlZipcode.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlUser
            // 
            this.pnlUser.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.pnlUser.Controls.Add(this.pnlCompleteForm);
            this.pnlUser.Controls.Add(this.pnlSave);
            this.pnlUser.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlUser.Location = new System.Drawing.Point(0, 0);
            this.pnlUser.Name = "pnlUser";
            this.pnlUser.Size = new System.Drawing.Size(624, 532);
            this.pnlUser.TabIndex = 0;
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlGrid);
            this.pnlCompleteForm.Controls.Add(this.pnlZipcode);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Padding = new Wisej.Web.Padding(5);
            this.pnlCompleteForm.Size = new System.Drawing.Size(622, 495);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.grpApplication);
            this.pnlGrid.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(5, 199);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new Wisej.Web.Padding(8, 0, 8, 0);
            this.pnlGrid.Size = new System.Drawing.Size(612, 291);
            this.pnlGrid.TabIndex = 2;
            // 
            // grpApplication
            // 
            this.grpApplication.Controls.Add(this.grdApplications);
            this.grpApplication.Dock = Wisej.Web.DockStyle.Fill;
            this.grpApplication.Location = new System.Drawing.Point(8, 0);
            this.grpApplication.Name = "grpApplication";
            this.grpApplication.Size = new System.Drawing.Size(596, 291);
            this.grpApplication.TabIndex = 11;
            this.grpApplication.Text = "Applications";
            // 
            // grdApplications
            // 
            this.grdApplications.AllowUserToResizeColumns = false;
            this.grdApplications.AllowUserToResizeRows = false;
            this.grdApplications.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.grdApplications.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdApplications.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.grdApplications.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.chkAppcode,
            this.AppCode,
            this.dataGridViewTextBoxColumn1});
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdApplications.DefaultCellStyle = dataGridViewCellStyle8;
            this.grdApplications.Dock = Wisej.Web.DockStyle.Fill;
            this.grdApplications.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdApplications.Location = new System.Drawing.Point(3, 17);
            this.grdApplications.Name = "grdApplications";
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdApplications.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.grdApplications.RowHeadersWidth = 15;
            this.grdApplications.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grdApplications.ShowCellToolTips = false;
            this.grdApplications.ShowColumnVisibilityMenu = false;
            this.grdApplications.Size = new System.Drawing.Size(590, 271);
            this.grdApplications.TabIndex = 12;
            // 
            // chkAppcode
            // 
            this.chkAppcode.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = false;
            this.chkAppcode.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkAppcode.HeaderStyle = dataGridViewCellStyle3;
            this.chkAppcode.HeaderText = "  ";
            this.chkAppcode.MinimumWidth = 40;
            this.chkAppcode.Name = "chkAppcode";
            this.chkAppcode.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.chkAppcode.ShowInVisibilityMenu = false;
            this.chkAppcode.Width = 40;
            // 
            // AppCode
            // 
            this.AppCode.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.AppCode.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.AppCode.HeaderStyle = dataGridViewCellStyle5;
            this.AppCode.HeaderText = " ";
            this.AppCode.Name = "AppCode";
            this.AppCode.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.AppCode.ShowInVisibilityMenu = false;
            this.AppCode.Visible = false;
            this.AppCode.Width = 10;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dataGridViewTextBoxColumn1.HeaderStyle = dataGridViewCellStyle7;
            this.dataGridViewTextBoxColumn1.HeaderText = "Applications";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 344;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.ShowInVisibilityMenu = false;
            this.dataGridViewTextBoxColumn1.Width = 510;
            // 
            // pnlZipcode
            // 
            this.pnlZipcode.Controls.Add(this.chkbInActive);
            this.pnlZipcode.Controls.Add(this.lblreqState);
            this.pnlZipcode.Controls.Add(this.lblreqCity);
            this.pnlZipcode.Controls.Add(this.lblreqZip);
            this.pnlZipcode.Controls.Add(this.cmbYear);
            this.pnlZipcode.Controls.Add(this.cmbDay);
            this.pnlZipcode.Controls.Add(this.cmbMonth);
            this.pnlZipcode.Controls.Add(this.lblYear);
            this.pnlZipcode.Controls.Add(this.lblDay);
            this.pnlZipcode.Controls.Add(this.lblMonth);
            this.pnlZipcode.Controls.Add(this.txtIntakecode);
            this.pnlZipcode.Controls.Add(this.lblIntakecode);
            this.pnlZipcode.Controls.Add(this.lblZipCode);
            this.pnlZipcode.Controls.Add(this.lblTownShip);
            this.pnlZipcode.Controls.Add(this.lblCity);
            this.pnlZipcode.Controls.Add(this.lblState);
            this.pnlZipcode.Controls.Add(this.lblHeadStartDate);
            this.pnlZipcode.Controls.Add(this.lblCountry);
            this.pnlZipcode.Controls.Add(this.txtZipCode);
            this.pnlZipcode.Controls.Add(this.txtZipCode1);
            this.pnlZipcode.Controls.Add(this.cmbTownship);
            this.pnlZipcode.Controls.Add(this.txtCity);
            this.pnlZipcode.Controls.Add(this.txtState);
            this.pnlZipcode.Controls.Add(this.cmbCountry);
            this.pnlZipcode.Dock = Wisej.Web.DockStyle.Top;
            this.pnlZipcode.Location = new System.Drawing.Point(5, 5);
            this.pnlZipcode.Name = "pnlZipcode";
            this.pnlZipcode.Size = new System.Drawing.Size(612, 194);
            this.pnlZipcode.TabIndex = 1;
            // 
            // chkbInActive
            // 
            this.chkbInActive.Appearance = Wisej.Web.Appearance.Switch;
            this.chkbInActive.AutoSize = false;
            this.chkbInActive.Location = new System.Drawing.Point(422, 8);
            this.chkbInActive.Name = "chkbInActive";
            this.chkbInActive.Size = new System.Drawing.Size(79, 20);
            this.chkbInActive.TabIndex = 4;
            this.chkbInActive.Text = "Inactive";
            // 
            // lblreqState
            // 
            this.lblreqState.ForeColor = System.Drawing.Color.Red;
            this.lblreqState.Location = new System.Drawing.Point(44, 103);
            this.lblreqState.Name = "lblreqState";
            this.lblreqState.Size = new System.Drawing.Size(10, 11);
            this.lblreqState.TabIndex = 14;
            this.lblreqState.Text = "*";
            // 
            // lblreqCity
            // 
            this.lblreqCity.ForeColor = System.Drawing.Color.Red;
            this.lblreqCity.Location = new System.Drawing.Point(37, 72);
            this.lblreqCity.Name = "lblreqCity";
            this.lblreqCity.Size = new System.Drawing.Size(10, 10);
            this.lblreqCity.TabIndex = 14;
            this.lblreqCity.Text = "*";
            // 
            // lblreqZip
            // 
            this.lblreqZip.ForeColor = System.Drawing.Color.Red;
            this.lblreqZip.Location = new System.Drawing.Point(64, 7);
            this.lblreqZip.Name = "lblreqZip";
            this.lblreqZip.Size = new System.Drawing.Size(10, 10);
            this.lblreqZip.TabIndex = 14;
            this.lblreqZip.Text = "*";
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(461, 166);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(133, 25);
            this.cmbYear.TabIndex = 11;
            // 
            // cmbDay
            // 
            this.cmbDay.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbDay.FormattingEnabled = true;
            this.cmbDay.Location = new System.Drawing.Point(368, 166);
            this.cmbDay.Name = "cmbDay";
            this.cmbDay.Size = new System.Drawing.Size(44, 25);
            this.cmbDay.TabIndex = 10;
            // 
            // cmbMonth
            // 
            this.cmbMonth.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Location = new System.Drawing.Point(235, 166);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(91, 25);
            this.cmbMonth.TabIndex = 9;
            this.cmbMonth.SelectedIndexChanged += new System.EventHandler(this.cmbMonth_SelectedIndexChanged);
            // 
            // lblYear
            // 
            this.lblYear.Location = new System.Drawing.Point(424, 170);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(26, 14);
            this.lblYear.TabIndex = 12;
            this.lblYear.Text = "Year";
            // 
            // lblDay
            // 
            this.lblDay.Location = new System.Drawing.Point(337, 170);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(22, 16);
            this.lblDay.TabIndex = 12;
            this.lblDay.Text = "Day";
            // 
            // lblMonth
            // 
            this.lblMonth.Location = new System.Drawing.Point(187, 170);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(38, 14);
            this.lblMonth.TabIndex = 12;
            this.lblMonth.Text = "Month";
            // 
            // txtIntakecode
            // 
            this.txtIntakecode.Location = new System.Drawing.Point(316, 6);
            this.txtIntakecode.MaxLength = 4;
            this.txtIntakecode.Name = "txtIntakecode";
            this.txtIntakecode.Size = new System.Drawing.Size(35, 25);
            this.txtIntakecode.TabIndex = 3;
            this.txtIntakecode.Visible = false;
            // 
            // lblIntakecode
            // 
            this.lblIntakecode.Location = new System.Drawing.Point(235, 10);
            this.lblIntakecode.Name = "lblIntakecode";
            this.lblIntakecode.Size = new System.Drawing.Size(68, 14);
            this.lblIntakecode.TabIndex = 10;
            this.lblIntakecode.Text = "Intake Code";
            this.lblIntakecode.Visible = false;
            // 
            // lblZipCode
            // 
            this.lblZipCode.Location = new System.Drawing.Point(15, 10);
            this.lblZipCode.Name = "lblZipCode";
            this.lblZipCode.Size = new System.Drawing.Size(50, 14);
            this.lblZipCode.TabIndex = 0;
            this.lblZipCode.Text = "ZIP Code";
            // 
            // lblTownShip
            // 
            this.lblTownShip.Location = new System.Drawing.Point(15, 42);
            this.lblTownShip.Name = "lblTownShip";
            this.lblTownShip.Size = new System.Drawing.Size(55, 16);
            this.lblTownShip.TabIndex = 0;
            this.lblTownShip.Text = "Township";
            // 
            // lblCity
            // 
            this.lblCity.Location = new System.Drawing.Point(15, 75);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(22, 16);
            this.lblCity.TabIndex = 0;
            this.lblCity.Text = "City";
            // 
            // lblState
            // 
            this.lblState.Location = new System.Drawing.Point(15, 106);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(30, 14);
            this.lblState.TabIndex = 0;
            this.lblState.Text = "State";
            // 
            // lblHeadStartDate
            // 
            this.lblHeadStartDate.Location = new System.Drawing.Point(15, 170);
            this.lblHeadStartDate.Name = "lblHeadStartDate";
            this.lblHeadStartDate.Size = new System.Drawing.Size(169, 14);
            this.lblHeadStartDate.TabIndex = 0;
            this.lblHeadStartDate.Text = "Head Start School Start Date :";
            // 
            // lblCountry
            // 
            this.lblCountry.Location = new System.Drawing.Point(15, 138);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(41, 16);
            this.lblCountry.TabIndex = 0;
            this.lblCountry.Text = "County";
            // 
            // txtZipCode
            // 
            this.txtZipCode.Location = new System.Drawing.Point(87, 6);
            this.txtZipCode.MaxLength = 5;
            this.txtZipCode.Name = "txtZipCode";
            this.txtZipCode.Size = new System.Drawing.Size(45, 25);
            this.txtZipCode.TabIndex = 1;
            this.txtZipCode.Leave += new System.EventHandler(this.txtZipCode_Leave);
            // 
            // txtZipCode1
            // 
            this.txtZipCode1.Location = new System.Drawing.Point(157, 6);
            this.txtZipCode1.MaxLength = 4;
            this.txtZipCode1.Name = "txtZipCode1";
            this.txtZipCode1.Size = new System.Drawing.Size(37, 25);
            this.txtZipCode1.TabIndex = 2;
            this.txtZipCode1.Leave += new System.EventHandler(this.txtZipCode1_Leave);
            // 
            // cmbTownship
            // 
            this.cmbTownship.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbTownship.FormattingEnabled = true;
            this.cmbTownship.Location = new System.Drawing.Point(87, 38);
            this.cmbTownship.Name = "cmbTownship";
            this.cmbTownship.Size = new System.Drawing.Size(140, 25);
            this.cmbTownship.TabIndex = 5;
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(87, 70);
            this.txtCity.MaxLength = 30;
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(140, 25);
            this.txtCity.TabIndex = 6;
            // 
            // txtState
            // 
            this.txtState.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtState.Location = new System.Drawing.Point(87, 102);
            this.txtState.MaxLength = 2;
            this.txtState.Name = "txtState";
            this.txtState.Size = new System.Drawing.Size(34, 25);
            this.txtState.TabIndex = 7;
            // 
            // cmbCountry
            // 
            this.cmbCountry.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbCountry.FormattingEnabled = true;
            this.cmbCountry.Location = new System.Drawing.Point(86, 134);
            this.cmbCountry.Name = "cmbCountry";
            this.cmbCountry.Size = new System.Drawing.Size(140, 25);
            this.cmbCountry.TabIndex = 8;
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnSubmit);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 495);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(622, 35);
            this.pnlSave.TabIndex = 3;
            // 
            // btnSubmit
            // 
            this.btnSubmit.AppearanceKey = "button-ok";
            this.btnSubmit.Dock = Wisej.Web.DockStyle.Right;
            this.btnSubmit.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSubmit.Location = new System.Drawing.Point(454, 5);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 25);
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Text = "&Save";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(529, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnCancel.Location = new System.Drawing.Point(532, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_1);
            // 
            // listBox1
            // 
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(100, 100);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(0, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            // 
            // ZipCodeForm
            // 
            this.ClientSize = new System.Drawing.Size(624, 532);
            this.Controls.Add(this.pnlUser);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ZipCodeForm";
            this.Text = "ZIP Code File Maintenance";
            componentTool1.ImageSource = "icon-help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.ZipCodeForm_ToolClick);
            this.Load += new System.EventHandler(this.ZipCodeForm_Load);
            this.pnlUser.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlGrid.ResumeLayout(false);
            this.grpApplication.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdApplications)).EndInit();
            this.pnlZipcode.ResumeLayout(false);
            this.pnlZipcode.PerformLayout();
            this.pnlSave.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel pnlUser;
        private ListBox listBox1;
        private Button button1;
        private Button button2;
        private Panel pnlZipcode;
        private Label lblZipCode;
        private Label lblTownShip;
        private Label lblCity;
        private Label lblState;
        private Label lblHeadStartDate;
        private Label lblCountry;
        private TextBoxWithValidation txtZipCode;
        private TextBoxWithValidation txtZipCode1;
        private ComboBox cmbTownship;
        private TextBox txtCity;
        private TextBox txtState;
        private ComboBox cmbCountry;
        private GroupBox grpApplication;
        private DataGridView grdApplications;
        private DataGridViewCheckBoxColumn chkAppcode;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private Button btnSubmit;
        private Button btnCancel;
        private DataGridViewTextBoxColumn AppCode;
        private TextBoxWithValidation txtIntakecode;
        private Label lblIntakecode;
        private ComboBox cmbYear;
        private ComboBox cmbDay;
        private ComboBox cmbMonth;
        private Label lblYear;
        private Label lblDay;
        private Label lblMonth;
        private Label lblreqState;
        private Label lblreqCity;
        private Label lblreqZip;
        private CheckBox chkbInActive;
        private Panel pnlSave;
        private Spacer spacer1;
        private Panel pnlCompleteForm;
        private Panel pnlGrid;
    }
}