using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class HSS00133QuestionView
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSS00133QuestionView));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.panel1 = new Wisej.Web.Panel();
            this.pnlAllControls = new Wisej.Web.Panel();
            this.CbActive = new Wisej.Web.CheckBox();
            this.MtxtSeq = new Wisej.Web.MaskedTextBox();
            this.label13 = new Wisej.Web.Label();
            this.CmbType = new Wisej.Web.ComboBox();
            this.TxtQuesDesc = new Wisej.Web.TextBox();
            this.FurtreDtPanel = new Wisej.Web.Panel();
            this.CbFDate = new Wisej.Web.CheckBox();
            this.label6 = new Wisej.Web.Label();
            this.label17 = new Wisej.Web.Label();
            this.TxtAccess = new Wisej.Web.TextBox();
            this.TxtType = new Wisej.Web.TextBox();
            this.checkBox5 = new Wisej.Web.CheckBox();
            this.label4 = new Wisej.Web.Label();
            this.label9 = new Wisej.Web.Label();
            this.CustRespPanel = new Wisej.Web.Panel();
            this.CustRespGrid = new Wisej.Web.DataGridView();
            this.panel2 = new Wisej.Web.Panel();
            this.Btn_Cancel = new Wisej.Web.Button();
            this.spacer3 = new Wisej.Web.Spacer();
            this.Btn_Save = new Wisej.Web.Button();
            this.spacer2 = new Wisej.Web.Spacer();
            this.btnSave = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnAdd = new Wisej.Web.Button();
            this.btnCancel = new Wisej.Web.Button();
            this.gvwQuestion = new Wisej.Web.DataGridView();
            this.gvcheckdetails = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gviEdit = new Wisej.Web.DataGridViewImageColumn();
            this.gviDel = new Wisej.Web.DataGridViewImageColumn();
            this.label2 = new Wisej.Web.Label();
            this.label3 = new Wisej.Web.Label();
            this.TxtAbbr = new Wisej.Web.TextBox();
            this.label10 = new Wisej.Web.Label();
            this.TxtCode = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.RespCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.RespDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Type = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Changed = new Wisej.Web.DataGridViewTextBoxColumn();
            this.CustSeq = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Empty_Row = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvQuestionDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvQuestioncode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtQueType1 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtActive = new Wisej.Web.DataGridViewTextBoxColumn();
            this.CustomKey = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtQType = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.pnlAllControls.SuspendLayout();
            this.FurtreDtPanel.SuspendLayout();
            this.CustRespPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CustRespGrid)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwQuestion)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.panel1.Controls.Add(this.pnlAllControls);
            this.panel1.Controls.Add(this.CustRespPanel);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.gvwQuestion);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(785, 453);
            this.panel1.TabIndex = 0;
            // 
            // pnlAllControls
            // 
            this.pnlAllControls.Controls.Add(this.CbActive);
            this.pnlAllControls.Controls.Add(this.MtxtSeq);
            this.pnlAllControls.Controls.Add(this.label13);
            this.pnlAllControls.Controls.Add(this.CmbType);
            this.pnlAllControls.Controls.Add(this.TxtQuesDesc);
            this.pnlAllControls.Controls.Add(this.FurtreDtPanel);
            this.pnlAllControls.Controls.Add(this.label6);
            this.pnlAllControls.Controls.Add(this.label17);
            this.pnlAllControls.Controls.Add(this.TxtAccess);
            this.pnlAllControls.Controls.Add(this.TxtType);
            this.pnlAllControls.Controls.Add(this.TxtCode);
            this.pnlAllControls.Controls.Add(this.checkBox5);
            this.pnlAllControls.Controls.Add(this.label4);
            this.pnlAllControls.Controls.Add(this.label9);
            this.pnlAllControls.Location = new System.Drawing.Point(3, 298);
            this.pnlAllControls.Name = "pnlAllControls";
            this.pnlAllControls.Size = new System.Drawing.Size(479, 116);
            this.pnlAllControls.TabIndex = 34;
            // 
            // CbActive
            // 
            this.CbActive.Location = new System.Drawing.Point(196, 9);
            this.CbActive.Name = "CbActive";
            this.CbActive.Size = new System.Drawing.Size(65, 21);
            this.CbActive.TabIndex = 3;
            this.CbActive.Text = "Active";
            // 
            // MtxtSeq
            // 
            this.MtxtSeq.Enabled = false;
            this.MtxtSeq.Location = new System.Drawing.Point(12, 123);
            this.MtxtSeq.Name = "MtxtSeq";
            this.MtxtSeq.Size = new System.Drawing.Size(49, 25);
            this.MtxtSeq.TabIndex = 1;
            this.MtxtSeq.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.MtxtSeq.Visible = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(413, 12);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(8, 14);
            this.label13.TabIndex = 34;
            this.label13.Text = ".";
            this.label13.Visible = false;
            // 
            // CmbType
            // 
            this.CmbType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbType.FormattingEnabled = true;
            this.CmbType.Location = new System.Drawing.Point(73, 6);
            this.CmbType.Name = "CmbType";
            this.CmbType.Size = new System.Drawing.Size(98, 25);
            this.CmbType.TabIndex = 2;
            this.CmbType.SelectedIndexChanged += new System.EventHandler(this.CmbType_SelectedIndexChanged);
            // 
            // TxtQuesDesc
            // 
            this.TxtQuesDesc.Location = new System.Drawing.Point(73, 42);
            this.TxtQuesDesc.MaxLength = 150;
            this.TxtQuesDesc.Multiline = true;
            this.TxtQuesDesc.Name = "TxtQuesDesc";
            this.TxtQuesDesc.Size = new System.Drawing.Size(305, 69);
            this.TxtQuesDesc.TabIndex = 7;
            // 
            // FurtreDtPanel
            // 
            this.FurtreDtPanel.Controls.Add(this.CbFDate);
            this.FurtreDtPanel.Location = new System.Drawing.Point(266, 7);
            this.FurtreDtPanel.Name = "FurtreDtPanel";
            this.FurtreDtPanel.Size = new System.Drawing.Size(143, 26);
            this.FurtreDtPanel.TabIndex = 5;
            this.FurtreDtPanel.Visible = false;
            // 
            // CbFDate
            // 
            this.CbFDate.Location = new System.Drawing.Point(2, 3);
            this.CbFDate.Name = "CbFDate";
            this.CbFDate.Size = new System.Drawing.Size(128, 21);
            this.CbFDate.TabIndex = 6;
            this.CbFDate.Text = "Allow Future Date";
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(60, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(10, 14);
            this.label6.TabIndex = 33;
            this.label6.Text = "*";
            // 
            // label17
            // 
            this.label17.ForeColor = System.Drawing.Color.Red;
            this.label17.Location = new System.Drawing.Point(62, 4);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(9, 14);
            this.label17.TabIndex = 33;
            this.label17.Text = "*";
            // 
            // TxtAccess
            // 
            this.TxtAccess.Enabled = false;
            this.TxtAccess.Location = new System.Drawing.Point(97, 123);
            this.TxtAccess.Name = "TxtAccess";
            this.TxtAccess.Size = new System.Drawing.Size(17, 25);
            this.TxtAccess.TabIndex = 2;
            this.TxtAccess.Visible = false;
            // 
            // TxtType
            // 
            this.TxtType.Enabled = false;
            this.TxtType.Location = new System.Drawing.Point(65, 123);
            this.TxtType.Name = "TxtType";
            this.TxtType.Size = new System.Drawing.Size(26, 25);
            this.TxtType.TabIndex = 2;
            this.TxtType.Visible = false;
            // 
            // checkBox5
            // 
            this.checkBox5.Location = new System.Drawing.Point(183, 122);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(81, 21);
            this.checkBox5.TabIndex = 1;
            this.checkBox5.Text = "Required";
            this.checkBox5.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 9);
            this.label4.MinimumSize = new System.Drawing.Size(0, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 18);
            this.label4.TabIndex = 0;
            this.label4.Text = "Type";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 40);
            this.label9.MinimumSize = new System.Drawing.Size(0, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 18);
            this.label9.TabIndex = 0;
            this.label9.Text = "Question";
            // 
            // CustRespPanel
            // 
            this.CustRespPanel.Controls.Add(this.CustRespGrid);
            this.CustRespPanel.Location = new System.Drawing.Point(490, 298);
            this.CustRespPanel.Name = "CustRespPanel";
            this.CustRespPanel.Size = new System.Drawing.Size(271, 116);
            this.CustRespPanel.TabIndex = 9;
            // 
            // CustRespGrid
            // 
            this.CustRespGrid.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.CustRespGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.CustRespGrid.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CustRespGrid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.RespCode,
            this.RespDesc,
            this.Type,
            this.Changed,
            this.CustSeq,
            this.Empty_Row});
            this.CustRespGrid.Dock = Wisej.Web.DockStyle.Fill;
            this.CustRespGrid.Location = new System.Drawing.Point(0, 0);
            this.CustRespGrid.MultiSelect = false;
            this.CustRespGrid.Name = "CustRespGrid";
            this.CustRespGrid.RowHeadersWidth = 15;
            this.CustRespGrid.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.CustRespGrid.ShowColumnVisibilityMenu = false;
            this.CustRespGrid.Size = new System.Drawing.Size(271, 116);
            this.CustRespGrid.TabIndex = 1;
            this.CustRespGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.CustRespGrid_CellValueChanged);
            this.CustRespGrid.SelectionChanged += new System.EventHandler(this.CustRespGrid_SelectionChanged);
            // 
            // panel2
            // 
            this.panel2.AppearanceKey = "panel-grdo";
            this.panel2.Controls.Add(this.Btn_Cancel);
            this.panel2.Controls.Add(this.spacer3);
            this.panel2.Controls.Add(this.Btn_Save);
            this.panel2.Controls.Add(this.spacer2);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.spacer1);
            this.panel2.Controls.Add(this.btnAdd);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 416);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(4);
            this.panel2.Size = new System.Drawing.Size(783, 35);
            this.panel2.TabIndex = 10;
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.AppearanceKey = "button-cancel";
            this.Btn_Cancel.Dock = Wisej.Web.DockStyle.Left;
            this.Btn_Cancel.Location = new System.Drawing.Point(134, 4);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(60, 27);
            this.Btn_Cancel.TabIndex = 11;
            this.Btn_Cancel.Text = "&Cancel";
            this.Btn_Cancel.Visible = false;
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Left;
            this.spacer3.Location = new System.Drawing.Point(129, 4);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(5, 27);
            // 
            // Btn_Save
            // 
            this.Btn_Save.AppearanceKey = "button-ok";
            this.Btn_Save.Dock = Wisej.Web.DockStyle.Left;
            this.Btn_Save.Location = new System.Drawing.Point(69, 4);
            this.Btn_Save.Name = "Btn_Save";
            this.Btn_Save.Size = new System.Drawing.Size(60, 27);
            this.Btn_Save.TabIndex = 10;
            this.Btn_Save.Text = "&Save";
            this.Btn_Save.Visible = false;
            this.Btn_Save.Click += new System.EventHandler(this.Btn_Save_Click);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(64, 4);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(5, 27);
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(654, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 27);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "&OK";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(714, 4);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 27);
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = Wisej.Web.DockStyle.Left;
            this.btnAdd.Location = new System.Drawing.Point(4, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(60, 27);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "&Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(719, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 27);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "C&lose";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gvwQuestion
            // 
            this.gvwQuestion.AllowUserToResizeColumns = false;
            this.gvwQuestion.AllowUserToResizeRows = false;
            this.gvwQuestion.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwQuestion.BackColor = System.Drawing.Color.White;
            this.gvwQuestion.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwQuestion.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvcheckdetails,
            this.gvQuestionDesc,
            this.gvQuestioncode,
            this.gvtQueType1,
            this.gvtActive,
            this.CustomKey,
            this.gviEdit,
            this.gviDel});
            this.gvwQuestion.Dock = Wisej.Web.DockStyle.Top;
            this.gvwQuestion.Location = new System.Drawing.Point(0, 0);
            this.gvwQuestion.Name = "gvwQuestion";
            this.gvwQuestion.RowHeadersWidth = 14;
            this.gvwQuestion.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwQuestion.ShowColumnVisibilityMenu = false;
            this.gvwQuestion.Size = new System.Drawing.Size(783, 272);
            this.gvwQuestion.TabIndex = 7;
            this.gvwQuestion.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvwQuestion_CellValueChanged);
            this.gvwQuestion.SelectionChanged += new System.EventHandler(this.gvwQuestion_SelectionChanged);
            this.gvwQuestion.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwQuestion_CellClick);
            // 
            // gvcheckdetails
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = false;
            this.gvcheckdetails.DefaultCellStyle = dataGridViewCellStyle3;
            this.gvcheckdetails.HeaderText = "    ";
            this.gvcheckdetails.Name = "gvcheckdetails";
            this.gvcheckdetails.ShowInVisibilityMenu = false;
            this.gvcheckdetails.Width = 25;
            // 
            // gviEdit
            // 
            this.gviEdit.CellImageSource = "captain-edit";
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.NullValue = null;
            this.gviEdit.DefaultCellStyle = dataGridViewCellStyle6;
            this.gviEdit.HeaderText = "Edit";
            this.gviEdit.Name = "gviEdit";
            this.gviEdit.ShowInVisibilityMenu = false;
            this.gviEdit.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.gviEdit.Width = 45;
            // 
            // gviDel
            // 
            this.gviDel.CellImageSource = "captain-delete";
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.NullValue = null;
            this.gviDel.DefaultCellStyle = dataGridViewCellStyle7;
            this.gviDel.HeaderText = "Del";
            this.gviDel.Name = "gviDel";
            this.gviDel.ShowInVisibilityMenu = false;
            this.gviDel.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.gviDel.Width = 45;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(213, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Code";
            this.label2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(188, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Sequence";
            this.label3.Visible = false;
            // 
            // TxtAbbr
            // 
            this.TxtAbbr.Location = new System.Drawing.Point(69, 51);
            this.TxtAbbr.MaxLength = 25;
            this.TxtAbbr.Name = "TxtAbbr";
            this.TxtAbbr.Size = new System.Drawing.Size(203, 19);
            this.TxtAbbr.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1, 53);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(68, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Abbreviation";
            // 
            // TxtCode
            // 
            this.TxtCode.Enabled = false;
            this.TxtCode.Location = new System.Drawing.Point(124, 122);
            this.TxtCode.Name = "TxtCode";
            this.TxtCode.Size = new System.Drawing.Size(47, 25);
            this.TxtCode.TabIndex = 2;
            this.TxtCode.Visible = false;
            // 
            // RespCode
            // 
            this.RespCode.HeaderText = "Code";
            this.RespCode.MaxInputLength = 2;
            this.RespCode.Name = "RespCode";
            this.RespCode.Width = 45;
            // 
            // RespDesc
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.RespDesc.DefaultCellStyle = dataGridViewCellStyle2;
            this.RespDesc.HeaderText = "Response";
            this.RespDesc.MaxInputLength = 100;
            this.RespDesc.Name = "RespDesc";
            this.RespDesc.Width = 200;
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.Visible = false;
            // 
            // Changed
            // 
            this.Changed.HeaderText = "Changed";
            this.Changed.Name = "Changed";
            this.Changed.Visible = false;
            // 
            // CustSeq
            // 
            this.CustSeq.HeaderText = "CustSeq";
            this.CustSeq.Name = "CustSeq";
            this.CustSeq.Visible = false;
            // 
            // Empty_Row
            // 
            this.Empty_Row.HeaderText = "Empty_Row";
            this.Empty_Row.Name = "Empty_Row";
            this.Empty_Row.Visible = false;
            // 
            // gvQuestionDesc
            // 
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvQuestionDesc.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.WrapMode = Wisej.Web.DataGridViewTriState.NotSet;
            this.gvQuestionDesc.HeaderStyle = dataGridViewCellStyle5;
            this.gvQuestionDesc.HeaderText = "Question Description";
            this.gvQuestionDesc.Name = "gvQuestionDesc";
            this.gvQuestionDesc.ReadOnly = true;
            this.gvQuestionDesc.Width = 540;
            // 
            // gvQuestioncode
            // 
            this.gvQuestioncode.HeaderText = " ";
            this.gvQuestioncode.Name = "gvQuestioncode";
            this.gvQuestioncode.ShowInVisibilityMenu = false;
            this.gvQuestioncode.Visible = false;
            this.gvQuestioncode.Width = 10;
            // 
            // gvtQueType1
            // 
            this.gvtQueType1.HeaderText = " ";
            this.gvtQueType1.Name = "gvtQueType1";
            this.gvtQueType1.ShowInVisibilityMenu = false;
            this.gvtQueType1.Visible = false;
            this.gvtQueType1.Width = 10;
            // 
            // gvtActive
            // 
            this.gvtActive.HeaderText = " ";
            this.gvtActive.Name = "gvtActive";
            this.gvtActive.ShowInVisibilityMenu = false;
            this.gvtActive.Visible = false;
            this.gvtActive.Width = 10;
            // 
            // CustomKey
            // 
            this.CustomKey.HeaderText = " ";
            this.CustomKey.Name = "CustomKey";
            this.CustomKey.ShowInVisibilityMenu = false;
            this.CustomKey.Visible = false;
            this.CustomKey.Width = 10;
            // 
            // gvtQType
            // 
            this.gvtQType.HeaderText = "Type";
            this.gvtQType.Name = "gvtQType";
            this.gvtQType.ReadOnly = true;
            this.gvtQType.Width = 80;
            // 
            // HSS00133QuestionView
            // 
            this.ClientSize = new System.Drawing.Size(785, 453);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSS00133QuestionView";
            this.Text = "HSS00133QuestionView";
            componentTool1.ImageSource = "captain-excel";
            componentTool1.Name = "tlExcel";
            componentTool1.ToolTipText = "Questions in Excel";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.HSS00133QuestionView_ToolClick);
            this.panel1.ResumeLayout(false);
            this.pnlAllControls.ResumeLayout(false);
            this.pnlAllControls.PerformLayout();
            this.FurtreDtPanel.ResumeLayout(false);
            this.FurtreDtPanel.PerformLayout();
            this.CustRespPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CustRespGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwQuestion)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private DataGridView gvwQuestion;
        private DataGridViewCheckBoxColumn gvcheckdetails;
        private DataGridViewTextBoxColumn gvQuestionDesc;
        private DataGridViewTextBoxColumn gvQuestioncode;
        private Button btnCancel;
        private Button btnSave;
        private Panel panel2;
        private DataGridViewTextBoxColumn gvtQueType1;
        private DataGridViewTextBoxColumn gvtActive;
        private Panel FurtreDtPanel;
        private CheckBox CbFDate;
        private ComboBox CmbType;
        private TextBoxWithValidation TxtCode;
        private CheckBox checkBox5;
        private Label label9;
        private Label label4;
        private TextBox TxtType;
        private TextBox TxtAccess;
        private Label label17;
        private Label label6;
        private TextBox TxtQuesDesc;
        private Panel pnlAllControls;
        private Panel CustRespPanel;
        private DataGridView CustRespGrid;
        private DataGridViewTextBoxColumn RespCode;
        private DataGridViewTextBoxColumn RespDesc;
        private DataGridViewTextBoxColumn Type;
        private DataGridViewTextBoxColumn Changed;
        private DataGridViewTextBoxColumn CustSeq;
        private DataGridViewTextBoxColumn Empty_Row;
        private Label label13;
        private Button btnAdd;
        private Button Btn_Save;
        private Button Btn_Cancel;
        private MaskedTextBox MtxtSeq;
        private DataGridViewTextBoxColumn CustomKey;
        private DataGridViewImageColumn gviEdit;
        private DataGridViewImageColumn gviDel;      
        private Label label2;
        private Label label3;
        private TextBox TxtAbbr;
        private Label label10;
        private DataGridViewTextBoxColumn gvtQType;
        private CheckBox CbActive;
        private Spacer spacer3;
        private Spacer spacer2;
        private Spacer spacer1;
    }
}