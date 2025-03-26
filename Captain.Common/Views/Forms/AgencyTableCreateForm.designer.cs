//using Wisej.Web;
//using Gizmox.WebGUI.Common;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class AgencyTableCreateForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgencyTableCreateForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.BtnCancel = new Wisej.Web.Button();
            this.BtnSubmit = new Wisej.Web.Button();
            this.lblReqDesc = new Wisej.Web.Label();
            this.lblReqTC = new Wisej.Web.Label();
            this.gvwApplication = new Wisej.Web.DataGridView();
            this.cbSelect = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Applications = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Star9 = new Wisej.Web.Label();
            this.Star8 = new Wisej.Web.Label();
            this.Star7 = new Wisej.Web.Label();
            this.Star6 = new Wisej.Web.Label();
            this.Star5 = new Wisej.Web.Label();
            this.Star4 = new Wisej.Web.Label();
            this.Star3 = new Wisej.Web.Label();
            this.Star2 = new Wisej.Web.Label();
            this.Star1 = new Wisej.Web.Label();
            this.CbCell4 = new Wisej.Web.CheckBox();
            this.CbCell2 = new Wisej.Web.CheckBox();
            this.CbCell3 = new Wisej.Web.CheckBox();
            this.CbCell5 = new Wisej.Web.CheckBox();
            this.CbCell6 = new Wisej.Web.CheckBox();
            this.CbCell7 = new Wisej.Web.CheckBox();
            this.CbCell8 = new Wisej.Web.CheckBox();
            this.CbCell9 = new Wisej.Web.CheckBox();
            this.CbCell1 = new Wisej.Web.CheckBox();
            this.CmbCode = new Wisej.Web.ComboBox();
            this.label3 = new Wisej.Web.Label();
            this.label4 = new Wisej.Web.Label();
            this.CmbDesc = new Wisej.Web.ComboBox();
            this.CmbSortBy = new Wisej.Web.ComboBox();
            this.label2 = new Wisej.Web.Label();
            this.groupBox1 = new Wisej.Web.GroupBox();
            this.TxtDesc = new Wisej.Web.TextBox();
            this.lblDesc = new Wisej.Web.Label();
            this.lblTC = new Wisej.Web.Label();
            this.pnlTCodeNDesc = new Wisej.Web.Panel();
            this.TxtCode = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlgvwApplications = new Wisej.Web.Panel();
            this.pnlReqCells = new Wisej.Web.Panel();
            this.pnlSave = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            ((System.ComponentModel.ISupportInitialize)(this.gvwApplication)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.pnlTCodeNDesc.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlgvwApplications.SuspendLayout();
            this.pnlReqCells.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnCancel
            // 
            this.BtnCancel.AppearanceKey = "button-error";
            this.BtnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.BtnCancel.Location = new System.Drawing.Point(576, 5);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 25);
            this.BtnCancel.TabIndex = 21;
            this.BtnCancel.Text = "&Cancel";
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnSubmit
            // 
            this.BtnSubmit.AppearanceKey = "button-ok";
            this.BtnSubmit.Dock = Wisej.Web.DockStyle.Right;
            this.BtnSubmit.Location = new System.Drawing.Point(498, 5);
            this.BtnSubmit.Name = "BtnSubmit";
            this.BtnSubmit.Size = new System.Drawing.Size(75, 25);
            this.BtnSubmit.TabIndex = 20;
            this.BtnSubmit.Text = "&Save";
            this.BtnSubmit.Click += new System.EventHandler(this.BtnSubmit_Click);
            // 
            // lblReqDesc
            // 
            this.lblReqDesc.ForeColor = System.Drawing.Color.Red;
            this.lblReqDesc.Location = new System.Drawing.Point(257, 12);
            this.lblReqDesc.Name = "lblReqDesc";
            this.lblReqDesc.Size = new System.Drawing.Size(6, 10);
            this.lblReqDesc.TabIndex = 27;
            this.lblReqDesc.Text = "*";
            // 
            // lblReqTC
            // 
            this.lblReqTC.ForeColor = System.Drawing.Color.Red;
            this.lblReqTC.Location = new System.Drawing.Point(78, 12);
            this.lblReqTC.Name = "lblReqTC";
            this.lblReqTC.Size = new System.Drawing.Size(8, 9);
            this.lblReqTC.TabIndex = 27;
            this.lblReqTC.Text = "*";
            // 
            // gvwApplication
            // 
            this.gvwApplication.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwApplication.BackColor = System.Drawing.Color.White;
            this.gvwApplication.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwApplication.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwApplication.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwApplication.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.cbSelect,
            this.Code,
            this.Applications});
            this.gvwApplication.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwApplication.Location = new System.Drawing.Point(0, 0);
            this.gvwApplication.Name = "gvwApplication";
            this.gvwApplication.RowHeadersWidth = 18;
            this.gvwApplication.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwApplication.Size = new System.Drawing.Size(666, 209);
            this.gvwApplication.TabIndex = 18;
            // 
            // cbSelect
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.NullValue = false;
            this.cbSelect.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cbSelect.HeaderStyle = dataGridViewCellStyle3;
            this.cbSelect.HeaderText = "Select";
            this.cbSelect.Name = "cbSelect";
            this.cbSelect.Width = 60;
            // 
            // Code
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Code.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Code.HeaderStyle = dataGridViewCellStyle5;
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            this.Code.Width = 55;
            // 
            // Applications
            // 
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Applications.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Applications.HeaderStyle = dataGridViewCellStyle7;
            this.Applications.HeaderText = "Applications";
            this.Applications.Name = "Applications";
            this.Applications.ReadOnly = true;
            this.Applications.Width = 500;
            // 
            // Star9
            // 
            this.Star9.ForeColor = System.Drawing.Color.Red;
            this.Star9.Location = new System.Drawing.Point(309, 85);
            this.Star9.Name = "Star9";
            this.Star9.Size = new System.Drawing.Size(7, 10);
            this.Star9.TabIndex = 27;
            this.Star9.Text = "*";
            this.Star9.Visible = false;
            // 
            // Star8
            // 
            this.Star8.ForeColor = System.Drawing.Color.Red;
            this.Star8.Location = new System.Drawing.Point(309, 55);
            this.Star8.Name = "Star8";
            this.Star8.Size = new System.Drawing.Size(7, 10);
            this.Star8.TabIndex = 27;
            this.Star8.Text = "*";
            this.Star8.Visible = false;
            // 
            // Star7
            // 
            this.Star7.ForeColor = System.Drawing.Color.Red;
            this.Star7.Location = new System.Drawing.Point(309, 25);
            this.Star7.Name = "Star7";
            this.Star7.Size = new System.Drawing.Size(8, 9);
            this.Star7.TabIndex = 27;
            this.Star7.Text = "*";
            this.Star7.Visible = false;
            // 
            // Star6
            // 
            this.Star6.ForeColor = System.Drawing.Color.Red;
            this.Star6.Location = new System.Drawing.Point(211, 85);
            this.Star6.Name = "Star6";
            this.Star6.Size = new System.Drawing.Size(7, 10);
            this.Star6.TabIndex = 27;
            this.Star6.Text = "*";
            this.Star6.Visible = false;
            // 
            // Star5
            // 
            this.Star5.ForeColor = System.Drawing.Color.Red;
            this.Star5.Location = new System.Drawing.Point(211, 55);
            this.Star5.Name = "Star5";
            this.Star5.Size = new System.Drawing.Size(6, 10);
            this.Star5.TabIndex = 27;
            this.Star5.Text = "*";
            this.Star5.Visible = false;
            // 
            // Star4
            // 
            this.Star4.ForeColor = System.Drawing.Color.Red;
            this.Star4.Location = new System.Drawing.Point(211, 25);
            this.Star4.Name = "Star4";
            this.Star4.Size = new System.Drawing.Size(7, 9);
            this.Star4.TabIndex = 27;
            this.Star4.Text = "*";
            this.Star4.Visible = false;
            // 
            // Star3
            // 
            this.Star3.ForeColor = System.Drawing.Color.Red;
            this.Star3.Location = new System.Drawing.Point(120, 85);
            this.Star3.Name = "Star3";
            this.Star3.Size = new System.Drawing.Size(6, 10);
            this.Star3.TabIndex = 27;
            this.Star3.Text = "*";
            this.Star3.Visible = false;
            // 
            // Star2
            // 
            this.Star2.ForeColor = System.Drawing.Color.Red;
            this.Star2.Location = new System.Drawing.Point(120, 55);
            this.Star2.Name = "Star2";
            this.Star2.Size = new System.Drawing.Size(8, 9);
            this.Star2.TabIndex = 27;
            this.Star2.Text = "*";
            this.Star2.Visible = false;
            // 
            // Star1
            // 
            this.Star1.ForeColor = System.Drawing.Color.Red;
            this.Star1.Location = new System.Drawing.Point(120, 25);
            this.Star1.Name = "Star1";
            this.Star1.Size = new System.Drawing.Size(6, 9);
            this.Star1.TabIndex = 27;
            this.Star1.Text = "*";
            this.Star1.Visible = false;
            // 
            // CbCell4
            // 
            this.CbCell4.AutoSize = false;
            this.CbCell4.Location = new System.Drawing.Point(155, 28);
            this.CbCell4.Name = "CbCell4";
            this.CbCell4.Size = new System.Drawing.Size(57, 20);
            this.CbCell4.TabIndex = 8;
            this.CbCell4.Text = "Cell 4";
            // 
            // CbCell2
            // 
            this.CbCell2.AutoSize = false;
            this.CbCell2.Location = new System.Drawing.Point(65, 58);
            this.CbCell2.Name = "CbCell2";
            this.CbCell2.Size = new System.Drawing.Size(57, 20);
            this.CbCell2.TabIndex = 6;
            this.CbCell2.Text = "Cell 2";
            // 
            // CbCell3
            // 
            this.CbCell3.AutoSize = false;
            this.CbCell3.Location = new System.Drawing.Point(65, 88);
            this.CbCell3.Name = "CbCell3";
            this.CbCell3.Size = new System.Drawing.Size(57, 20);
            this.CbCell3.TabIndex = 7;
            this.CbCell3.Text = "Cell 3";
            // 
            // CbCell5
            // 
            this.CbCell5.AutoSize = false;
            this.CbCell5.Location = new System.Drawing.Point(155, 58);
            this.CbCell5.Name = "CbCell5";
            this.CbCell5.Size = new System.Drawing.Size(57, 20);
            this.CbCell5.TabIndex = 9;
            this.CbCell5.Text = "Cell 5";
            // 
            // CbCell6
            // 
            this.CbCell6.AutoSize = false;
            this.CbCell6.Location = new System.Drawing.Point(155, 88);
            this.CbCell6.Name = "CbCell6";
            this.CbCell6.Size = new System.Drawing.Size(57, 20);
            this.CbCell6.TabIndex = 10;
            this.CbCell6.Text = "Cell 6";
            // 
            // CbCell7
            // 
            this.CbCell7.AutoSize = false;
            this.CbCell7.Location = new System.Drawing.Point(252, 28);
            this.CbCell7.Name = "CbCell7";
            this.CbCell7.Size = new System.Drawing.Size(57, 20);
            this.CbCell7.TabIndex = 11;
            this.CbCell7.Text = "Cell 7";
            // 
            // CbCell8
            // 
            this.CbCell8.AutoSize = false;
            this.CbCell8.Location = new System.Drawing.Point(252, 58);
            this.CbCell8.Name = "CbCell8";
            this.CbCell8.Size = new System.Drawing.Size(57, 20);
            this.CbCell8.TabIndex = 12;
            this.CbCell8.Text = "Cell 8";
            // 
            // CbCell9
            // 
            this.CbCell9.AutoSize = false;
            this.CbCell9.Location = new System.Drawing.Point(252, 89);
            this.CbCell9.Name = "CbCell9";
            this.CbCell9.Size = new System.Drawing.Size(57, 20);
            this.CbCell9.TabIndex = 13;
            this.CbCell9.Text = "Cell 9";
            // 
            // CbCell1
            // 
            this.CbCell1.AutoSize = false;
            this.CbCell1.Location = new System.Drawing.Point(65, 28);
            this.CbCell1.Name = "CbCell1";
            this.CbCell1.Size = new System.Drawing.Size(57, 20);
            this.CbCell1.TabIndex = 5;
            this.CbCell1.Text = "Cell 1";
            // 
            // CmbCode
            // 
            this.CmbCode.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbCode.FormattingEnabled = true;
            this.CmbCode.Location = new System.Drawing.Point(478, 27);
            this.CmbCode.Name = "CmbCode";
            this.CmbCode.Size = new System.Drawing.Size(95, 25);
            this.CmbCode.TabIndex = 14;
            this.CmbCode.SelectedIndexChanged += new System.EventHandler(this.CmbCode_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(351, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 14);
            this.label3.TabIndex = 6;
            this.label3.Text = "Cell for Code";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(351, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Cell for Description";
            // 
            // CmbDesc
            // 
            this.CmbDesc.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbDesc.FormattingEnabled = true;
            this.CmbDesc.Location = new System.Drawing.Point(478, 59);
            this.CmbDesc.Name = "CmbDesc";
            this.CmbDesc.Size = new System.Drawing.Size(95, 25);
            this.CmbDesc.TabIndex = 15;
            this.CmbDesc.SelectedIndexChanged += new System.EventHandler(this.CmbDesc_SelectedIndexChanged);
            // 
            // CmbSortBy
            // 
            this.CmbSortBy.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbSortBy.FormattingEnabled = true;
            this.CmbSortBy.Location = new System.Drawing.Point(478, 91);
            this.CmbSortBy.Name = "CmbSortBy";
            this.CmbSortBy.Size = new System.Drawing.Size(95, 25);
            this.CmbSortBy.TabIndex = 16;
            this.CmbSortBy.SelectedIndexChanged += new System.EventHandler(this.CmbSortBy_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(351, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Sort Order";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Star9);
            this.groupBox1.Controls.Add(this.Star8);
            this.groupBox1.Controls.Add(this.Star7);
            this.groupBox1.Controls.Add(this.Star6);
            this.groupBox1.Controls.Add(this.Star5);
            this.groupBox1.Controls.Add(this.Star4);
            this.groupBox1.Controls.Add(this.Star3);
            this.groupBox1.Controls.Add(this.Star2);
            this.groupBox1.Controls.Add(this.Star1);
            this.groupBox1.Controls.Add(this.CbCell4);
            this.groupBox1.Controls.Add(this.CbCell2);
            this.groupBox1.Controls.Add(this.CbCell3);
            this.groupBox1.Controls.Add(this.CbCell5);
            this.groupBox1.Controls.Add(this.CbCell6);
            this.groupBox1.Controls.Add(this.CbCell7);
            this.groupBox1.Controls.Add(this.CbCell8);
            this.groupBox1.Controls.Add(this.CbCell9);
            this.groupBox1.Controls.Add(this.CbCell1);
            this.groupBox1.Controls.Add(this.CmbCode);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.CmbDesc);
            this.groupBox1.Controls.Add(this.CmbSortBy);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = Wisej.Web.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(7, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(652, 126);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.Text = "Select Required Cells";
            // 
            // TxtDesc
            // 
            this.TxtDesc.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.TxtDesc.Location = new System.Drawing.Point(273, 11);
            this.TxtDesc.MaxLength = 50;
            this.TxtDesc.Name = "TxtDesc";
            this.TxtDesc.Size = new System.Drawing.Size(366, 25);
            this.TxtDesc.TabIndex = 3;
            this.TxtDesc.GotFocus += new System.EventHandler(this.TxtDesc_GotFocus);
            this.TxtDesc.LostFocus += new System.EventHandler(this.TxtDesc_LostFocus);
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(192, 15);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(66, 16);
            this.lblDesc.TabIndex = 6;
            this.lblDesc.Text = "Description";
            // 
            // lblTC
            // 
            this.lblTC.Location = new System.Drawing.Point(15, 15);
            this.lblTC.Name = "lblTC";
            this.lblTC.Size = new System.Drawing.Size(64, 14);
            this.lblTC.TabIndex = 6;
            this.lblTC.Text = "Table Code";
            // 
            // pnlTCodeNDesc
            // 
            this.pnlTCodeNDesc.Controls.Add(this.lblReqDesc);
            this.pnlTCodeNDesc.Controls.Add(this.lblReqTC);
            this.pnlTCodeNDesc.Controls.Add(this.TxtCode);
            this.pnlTCodeNDesc.Controls.Add(this.TxtDesc);
            this.pnlTCodeNDesc.Controls.Add(this.lblDesc);
            this.pnlTCodeNDesc.Controls.Add(this.lblTC);
            this.pnlTCodeNDesc.Dock = Wisej.Web.DockStyle.Top;
            this.pnlTCodeNDesc.Location = new System.Drawing.Point(0, 0);
            this.pnlTCodeNDesc.Name = "pnlTCodeNDesc";
            this.pnlTCodeNDesc.Size = new System.Drawing.Size(666, 38);
            this.pnlTCodeNDesc.TabIndex = 1;
            // 
            // TxtCode
            // 
            this.TxtCode.AutoSize = false;
            this.TxtCode.Location = new System.Drawing.Point(94, 11);
            this.TxtCode.MaxLength = 5;
            this.TxtCode.Name = "TxtCode";
            this.TxtCode.RightToLeft = Wisej.Web.RightToLeft.No;
            this.TxtCode.Size = new System.Drawing.Size(64, 25);
            this.TxtCode.TabIndex = 2;
            this.TxtCode.LostFocus += new System.EventHandler(this.TxtCode_LostFocus);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlgvwApplications);
            this.pnlCompleteForm.Controls.Add(this.pnlReqCells);
            this.pnlCompleteForm.Controls.Add(this.pnlSave);
            this.pnlCompleteForm.Controls.Add(this.pnlTCodeNDesc);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(666, 422);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlgvwApplications
            // 
            this.pnlgvwApplications.Controls.Add(this.gvwApplication);
            this.pnlgvwApplications.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwApplications.Location = new System.Drawing.Point(0, 178);
            this.pnlgvwApplications.Name = "pnlgvwApplications";
            this.pnlgvwApplications.Size = new System.Drawing.Size(666, 209);
            this.pnlgvwApplications.TabIndex = 17;
            // 
            // pnlReqCells
            // 
            this.pnlReqCells.Controls.Add(this.groupBox1);
            this.pnlReqCells.Dock = Wisej.Web.DockStyle.Top;
            this.pnlReqCells.Location = new System.Drawing.Point(0, 38);
            this.pnlReqCells.Name = "pnlReqCells";
            this.pnlReqCells.Padding = new Wisej.Web.Padding(7);
            this.pnlReqCells.Size = new System.Drawing.Size(666, 140);
            this.pnlReqCells.TabIndex = 4;
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.BtnSubmit);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.BtnCancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 387);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(666, 35);
            this.pnlSave.TabIndex = 19;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(573, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // AgencyTableCreateForm
            // 
            this.ClientSize = new System.Drawing.Size(666, 422);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AgencyTableCreateForm";
            this.Text = "Agency Tabble Codes";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "TL_HELP";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.AgencyTableCreateForm_ToolClick);
            ((System.ComponentModel.ISupportInitialize)(this.gvwApplication)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlTCodeNDesc.ResumeLayout(false);
            this.pnlTCodeNDesc.PerformLayout();
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlgvwApplications.ResumeLayout(false);
            this.pnlReqCells.ResumeLayout(false);
            this.pnlSave.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Button BtnCancel;
        private Button BtnSubmit;
        private Label lblReqDesc;
        private Label lblReqTC;
        private TextBoxWithValidation TxtCode;
        private DataGridView gvwApplication;
        private DataGridViewCheckBoxColumn cbSelect;
        private DataGridViewTextBoxColumn Code;
        private DataGridViewTextBoxColumn Applications;
        private Label Star9;
        private Label Star8;
        private Label Star7;
        private Label Star6;
        private Label Star5;
        private Label Star4;
        private Label Star3;
        private Label Star2;
        private Label Star1;
        private CheckBox CbCell4;
        private CheckBox CbCell2;
        private CheckBox CbCell3;
        private CheckBox CbCell5;
        private CheckBox CbCell6;
        private CheckBox CbCell7;
        private CheckBox CbCell8;
        private CheckBox CbCell9;
        private CheckBox CbCell1;
        private ComboBox CmbCode;
        private Label label3;
        private Label label4;
        private ComboBox CmbDesc;
        private ComboBox CmbSortBy;
        private Label label2;
        private GroupBox groupBox1;
        private TextBox TxtDesc;
        private Label lblDesc;
        private Label lblTC;
        private Panel pnlTCodeNDesc;
        private Panel pnlCompleteForm;
        private Panel pnlSave;
        private Spacer spacer1;
        private Panel pnlgvwApplications;
        private Panel pnlReqCells;
    }
}