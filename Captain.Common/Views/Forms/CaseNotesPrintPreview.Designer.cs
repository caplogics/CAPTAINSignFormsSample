using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class CaseNotesPrintPreview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaseNotesPrintPreview));
            this.lblApplicationNo = new Wisej.Web.Label();
            this.lblApplicationNon = new Wisej.Web.Label();
            this.lblClientNameD = new Wisej.Web.Label();
            this.lblClientName = new Wisej.Web.Label();
            this.pnlData = new Wisej.Web.Panel();
            this.pnlDesc = new Wisej.Web.Panel();
            this.pnlTxtDesc = new Wisej.Web.Panel();
            this.pnlCaseNotes = new Wisej.Web.Panel();
            this.lblCaseNotes = new Wisej.Web.Label();
            this.pnlRdbBtns = new Wisej.Web.Panel();
            this.rdbSel = new Wisej.Web.RadioButton();
            this.rdbAll = new Wisej.Web.RadioButton();
            this.pnlExit = new Wisej.Web.Panel();
            this.btnPrint = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.cmbsize = new Wisej.Web.ComboBox();
            this.lblSize = new Wisej.Web.Label();
            this.chkPrintName = new Wisej.Web.CheckBox();
            this.btnExit = new Wisej.Web.Button();
            this.pnlAppNoName = new Wisej.Web.Panel();
            this.spacer5 = new Wisej.Web.Spacer();
            this.spacer4 = new Wisej.Web.Spacer();
            this.spacer3 = new Wisej.Web.Spacer();
            this.spacer2 = new Wisej.Web.Spacer();
            this.lblHierarchy = new Wisej.Web.Label();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.dataGridCaseNotes = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.categorychk = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.ScreenName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dgvDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dgvDate = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.dataGridViewTextBoxColumn1 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.ReceiveDate = new Wisej.Web.DataGridViewTextBoxColumn();
            this.txtDesc = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.pnlData.SuspendLayout();
            this.pnlDesc.SuspendLayout();
            this.pnlTxtDesc.SuspendLayout();
            this.pnlCaseNotes.SuspendLayout();
            this.pnlRdbBtns.SuspendLayout();
            this.pnlExit.SuspendLayout();
            this.pnlAppNoName.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCaseNotes)).BeginInit();
            this.SuspendLayout();
            // 
            // lblApplicationNo
            // 
            this.lblApplicationNo.Dock = Wisej.Web.DockStyle.Left;
            this.lblApplicationNo.Font = new System.Drawing.Font("default", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblApplicationNo.Location = new System.Drawing.Point(314, 0);
            this.lblApplicationNo.Name = "lblApplicationNo";
            this.lblApplicationNo.Size = new System.Drawing.Size(49, 40);
            this.lblApplicationNo.TabIndex = 0;
            this.lblApplicationNo.Text = "App# :";
            this.lblApplicationNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblApplicationNon
            // 
            this.lblApplicationNon.Dock = Wisej.Web.DockStyle.Left;
            this.lblApplicationNon.Font = new System.Drawing.Font("default", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblApplicationNon.Location = new System.Drawing.Point(373, 0);
            this.lblApplicationNon.Name = "lblApplicationNon";
            this.lblApplicationNon.Size = new System.Drawing.Size(112, 40);
            this.lblApplicationNon.TabIndex = 1;
            this.lblApplicationNon.Text = "000000000000";
            this.lblApplicationNon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblClientNameD
            // 
            this.lblClientNameD.Dock = Wisej.Web.DockStyle.Left;
            this.lblClientNameD.Font = new System.Drawing.Font("default", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblClientNameD.Location = new System.Drawing.Point(561, 0);
            this.lblClientNameD.Name = "lblClientNameD";
            this.lblClientNameD.Size = new System.Drawing.Size(318, 40);
            this.lblClientNameD.TabIndex = 3;
            this.lblClientNameD.Text = "abcdef ghi";
            this.lblClientNameD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblClientName
            // 
            this.lblClientName.AutoSize = true;
            this.lblClientName.Dock = Wisej.Web.DockStyle.Left;
            this.lblClientName.Font = new System.Drawing.Font("default", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblClientName.Location = new System.Drawing.Point(505, 0);
            this.lblClientName.Name = "lblClientName";
            this.lblClientName.Size = new System.Drawing.Size(46, 40);
            this.lblClientName.TabIndex = 2;
            this.lblClientName.Text = "Name:";
            this.lblClientName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlData
            // 
            this.pnlData.Controls.Add(this.dataGridCaseNotes);
            this.pnlData.Controls.Add(this.pnlDesc);
            this.pnlData.Controls.Add(this.pnlRdbBtns);
            this.pnlData.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlData.Location = new System.Drawing.Point(0, 40);
            this.pnlData.Name = "pnlData";
            this.pnlData.Size = new System.Drawing.Size(888, 440);
            this.pnlData.TabIndex = 5;
            this.pnlData.TabStop = true;
            // 
            // pnlDesc
            // 
            this.pnlDesc.Controls.Add(this.pnlTxtDesc);
            this.pnlDesc.Controls.Add(this.pnlCaseNotes);
            this.pnlDesc.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlDesc.Location = new System.Drawing.Point(0, 220);
            this.pnlDesc.Name = "pnlDesc";
            this.pnlDesc.Size = new System.Drawing.Size(888, 220);
            this.pnlDesc.TabIndex = 3;
            this.pnlDesc.TabStop = true;
            // 
            // pnlTxtDesc
            // 
            this.pnlTxtDesc.Controls.Add(this.txtDesc);
            this.pnlTxtDesc.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlTxtDesc.Location = new System.Drawing.Point(0, 25);
            this.pnlTxtDesc.Name = "pnlTxtDesc";
            this.pnlTxtDesc.Size = new System.Drawing.Size(888, 195);
            this.pnlTxtDesc.TabIndex = 6;
            // 
            // pnlCaseNotes
            // 
            this.pnlCaseNotes.Controls.Add(this.lblCaseNotes);
            this.pnlCaseNotes.Dock = Wisej.Web.DockStyle.Top;
            this.pnlCaseNotes.Location = new System.Drawing.Point(0, 0);
            this.pnlCaseNotes.Name = "pnlCaseNotes";
            this.pnlCaseNotes.Size = new System.Drawing.Size(888, 25);
            this.pnlCaseNotes.TabIndex = 5;
            // 
            // lblCaseNotes
            // 
            this.lblCaseNotes.BackColor = System.Drawing.Color.FromArgb(244, 247, 249);
            this.lblCaseNotes.Dock = Wisej.Web.DockStyle.Fill;
            this.lblCaseNotes.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblCaseNotes.ForeColor = System.Drawing.Color.FromName("@buttonFace");
            this.lblCaseNotes.Location = new System.Drawing.Point(0, 0);
            this.lblCaseNotes.Name = "lblCaseNotes";
            this.lblCaseNotes.Size = new System.Drawing.Size(888, 25);
            this.lblCaseNotes.TabIndex = 4;
            this.lblCaseNotes.Text = "Case/Progress Note Details";
            this.lblCaseNotes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlRdbBtns
            // 
            this.pnlRdbBtns.Controls.Add(this.rdbSel);
            this.pnlRdbBtns.Controls.Add(this.rdbAll);
            this.pnlRdbBtns.Dock = Wisej.Web.DockStyle.Top;
            this.pnlRdbBtns.Location = new System.Drawing.Point(0, 0);
            this.pnlRdbBtns.Name = "pnlRdbBtns";
            this.pnlRdbBtns.Padding = new Wisej.Web.Padding(3);
            this.pnlRdbBtns.Size = new System.Drawing.Size(888, 31);
            this.pnlRdbBtns.TabIndex = 1;
            this.pnlRdbBtns.TabStop = true;
            // 
            // rdbSel
            // 
            this.rdbSel.AutoSize = false;
            this.rdbSel.Checked = true;
            this.rdbSel.Font = new System.Drawing.Font("@labelText", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.rdbSel.Location = new System.Drawing.Point(137, 4);
            this.rdbSel.Name = "rdbSel";
            this.rdbSel.Size = new System.Drawing.Size(141, 20);
            this.rdbSel.TabIndex = 0;
            this.rdbSel.TabStop = true;
            this.rdbSel.Text = "Selected Case Notes";
            this.rdbSel.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // rdbAll
            // 
            this.rdbAll.AutoSize = false;
            this.rdbAll.Checked = true;
            this.rdbAll.Font = new System.Drawing.Font("@labelText", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.rdbAll.Location = new System.Drawing.Point(15, 4);
            this.rdbAll.Name = "rdbAll";
            this.rdbAll.Size = new System.Drawing.Size(107, 20);
            this.rdbAll.TabIndex = 0;
            this.rdbAll.TabStop = true;
            this.rdbAll.Text = "All Case Notes";
            this.rdbAll.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // pnlExit
            // 
            this.pnlExit.AppearanceKey = "panel-grdo";
            this.pnlExit.Controls.Add(this.btnPrint);
            this.pnlExit.Controls.Add(this.spacer1);
            this.pnlExit.Controls.Add(this.cmbsize);
            this.pnlExit.Controls.Add(this.lblSize);
            this.pnlExit.Controls.Add(this.chkPrintName);
            this.pnlExit.Controls.Add(this.btnExit);
            this.pnlExit.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlExit.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.pnlExit.Location = new System.Drawing.Point(0, 480);
            this.pnlExit.Name = "pnlExit";
            this.pnlExit.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlExit.Size = new System.Drawing.Size(888, 35);
            this.pnlExit.TabIndex = 6;
            this.pnlExit.TabStop = true;
            // 
            // btnPrint
            // 
            this.btnPrint.AutoEllipsis = true;
            this.btnPrint.Dock = Wisej.Web.DockStyle.Right;
            this.btnPrint.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnPrint.Location = new System.Drawing.Point(750, 5);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(60, 25);
            this.btnPrint.TabIndex = 6;
            this.btnPrint.Text = "&Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click_1);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(810, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // cmbsize
            // 
            this.cmbsize.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbsize.FormattingEnabled = true;
            this.cmbsize.Location = new System.Drawing.Point(60, 5);
            this.cmbsize.Name = "cmbsize";
            this.cmbsize.Size = new System.Drawing.Size(54, 25);
            this.cmbsize.TabIndex = 2;
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSize.Location = new System.Drawing.Point(24, 10);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(28, 14);
            this.lblSize.TabIndex = 13;
            this.lblSize.Text = "Size";
            // 
            // chkPrintName
            // 
            this.chkPrintName.AutoSize = false;
            this.chkPrintName.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkPrintName.Location = new System.Drawing.Point(243, 5);
            this.chkPrintName.Name = "chkPrintName";
            this.chkPrintName.Size = new System.Drawing.Size(134, 24);
            this.chkPrintName.TabIndex = 8;
            this.chkPrintName.Text = "Print Client Name";
            // 
            // btnExit
            // 
            this.btnExit.AppearanceKey = "button-cancel";
            this.btnExit.AutoEllipsis = true;
            this.btnExit.Dock = Wisej.Web.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnExit.Location = new System.Drawing.Point(813, 5);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(60, 25);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "&Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // pnlAppNoName
            // 
            this.pnlAppNoName.BackColor = System.Drawing.Color.FromArgb(248, 244, 244, 244);
            this.pnlAppNoName.Controls.Add(this.lblClientNameD);
            this.pnlAppNoName.Controls.Add(this.spacer5);
            this.pnlAppNoName.Controls.Add(this.lblClientName);
            this.pnlAppNoName.Controls.Add(this.spacer4);
            this.pnlAppNoName.Controls.Add(this.lblApplicationNon);
            this.pnlAppNoName.Controls.Add(this.spacer3);
            this.pnlAppNoName.Controls.Add(this.lblApplicationNo);
            this.pnlAppNoName.Controls.Add(this.spacer2);
            this.pnlAppNoName.Controls.Add(this.lblHierarchy);
            this.pnlAppNoName.Dock = Wisej.Web.DockStyle.Top;
            this.pnlAppNoName.Location = new System.Drawing.Point(0, 0);
            this.pnlAppNoName.Name = "pnlAppNoName";
            this.pnlAppNoName.Size = new System.Drawing.Size(888, 40);
            this.pnlAppNoName.TabIndex = 4;
            this.pnlAppNoName.TabStop = true;
            // 
            // spacer5
            // 
            this.spacer5.Dock = Wisej.Web.DockStyle.Left;
            this.spacer5.Location = new System.Drawing.Point(551, 0);
            this.spacer5.Name = "spacer5";
            this.spacer5.Size = new System.Drawing.Size(10, 40);
            // 
            // spacer4
            // 
            this.spacer4.Dock = Wisej.Web.DockStyle.Left;
            this.spacer4.Location = new System.Drawing.Point(485, 0);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(20, 40);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Left;
            this.spacer3.Location = new System.Drawing.Point(363, 0);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(10, 40);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(294, 0);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(20, 40);
            // 
            // lblHierarchy
            // 
            this.lblHierarchy.Dock = Wisej.Web.DockStyle.Left;
            this.lblHierarchy.Font = new System.Drawing.Font("default", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblHierarchy.Location = new System.Drawing.Point(0, 0);
            this.lblHierarchy.Name = "lblHierarchy";
            this.lblHierarchy.Padding = new Wisej.Web.Padding(15, 0, 0, 0);
            this.lblHierarchy.Size = new System.Drawing.Size(294, 40);
            this.lblHierarchy.TabIndex = 4;
            this.lblHierarchy.Text = "Hierarchy";
            this.lblHierarchy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlData);
            this.pnlCompleteForm.Controls.Add(this.pnlAppNoName);
            this.pnlCompleteForm.Controls.Add(this.pnlExit);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(888, 515);
            this.pnlCompleteForm.TabIndex = 1;
            this.pnlCompleteForm.TabStop = true;
            // 
            // dataGridCaseNotes
            // 
            this.dataGridCaseNotes.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridCaseNotes.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.dataGridCaseNotes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridCaseNotes.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridCaseNotes.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.categorychk,
            this.ScreenName,
            this.dgvDesc,
            this.dgvDate,
            this.dataGridViewTextBoxColumn1,
            this.ReceiveDate});
            this.dataGridCaseNotes.Dock = Wisej.Web.DockStyle.Fill;
            this.dataGridCaseNotes.Location = new System.Drawing.Point(0, 31);
            this.dataGridCaseNotes.MultiSelect = false;
            this.dataGridCaseNotes.Name = "dataGridCaseNotes";
            this.dataGridCaseNotes.RowHeadersVisible = false;
            this.dataGridCaseNotes.RowHeadersWidth = 14;
            this.dataGridCaseNotes.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.dataGridCaseNotes.Size = new System.Drawing.Size(888, 189);
            this.dataGridCaseNotes.TabIndex = 2;
            this.dataGridCaseNotes.SelectionChanged += new System.EventHandler(this.dataGridCaseNotes_SelectionChanged);
            // 
            // categorychk
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = false;
            this.categorychk.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.categorychk.HeaderStyle = dataGridViewCellStyle3;
            this.categorychk.HeaderText = " ";
            this.categorychk.Name = "categorychk";
            this.categorychk.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.categorychk.ShowInVisibilityMenu = false;
            this.categorychk.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.categorychk.Width = 25;
            // 
            // ScreenName
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Padding = new Wisej.Web.Padding(15, 0, 0, 0);
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.ScreenName.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Padding = new Wisej.Web.Padding(15, 0, 0, 0);
            this.ScreenName.HeaderStyle = dataGridViewCellStyle5;
            this.ScreenName.HeaderText = "Screen Name";
            this.ScreenName.Name = "ScreenName";
            this.ScreenName.ReadOnly = true;
            this.ScreenName.Width = 220;
            // 
            // dgvDesc
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.dgvDesc.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.WrapMode = Wisej.Web.DataGridViewTriState.NotSet;
            this.dgvDesc.HeaderStyle = dataGridViewCellStyle7;
            this.dgvDesc.HeaderText = "Description";
            this.dgvDesc.Name = "dgvDesc";
            this.dgvDesc.Width = 420;
            // 
            // dgvDate
            // 
            this.dgvDate.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.dgvDate.HeaderStyle = dataGridViewCellStyle9;
            this.dgvDate.HeaderText = "Date";
            this.dgvDate.Name = "dgvDate";
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridViewTextBoxColumn1.HeaderStyle = dataGridViewCellStyle10;
            this.dataGridViewTextBoxColumn1.HeaderText = "User";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 120;
            // 
            // ReceiveDate
            // 
            dataGridViewCellStyle11.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.ReceiveDate.HeaderStyle = dataGridViewCellStyle11;
            this.ReceiveDate.HeaderText = "On";
            this.ReceiveDate.Name = "ReceiveDate";
            this.ReceiveDate.ReadOnly = true;
            this.ReceiveDate.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.ReceiveDate.ShowInVisibilityMenu = false;
            this.ReceiveDate.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.ReceiveDate.Visible = false;
            this.ReceiveDate.Width = 80;
            // 
            // txtDesc
            // 
            this.txtDesc.BorderStyle = Wisej.Web.BorderStyle.None;
            this.txtDesc.Dock = Wisej.Web.DockStyle.Fill;
            this.txtDesc.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtDesc.MaxLength = 150000;
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.ReadOnly = true;
            this.txtDesc.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.txtDesc.Size = new System.Drawing.Size(888, 195);
            this.txtDesc.TabIndex = 3;
            // 
            // CaseNotesPrintPreview
            // 
            this.ClientSize = new System.Drawing.Size(888, 515);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CaseNotesPrintPreview";
            this.Text = "Case Notes Print Preview";
            this.Load += new System.EventHandler(this.CaseNotesPrintPreview_Load);
            this.pnlData.ResumeLayout(false);
            this.pnlDesc.ResumeLayout(false);
            this.pnlTxtDesc.ResumeLayout(false);
            this.pnlTxtDesc.PerformLayout();
            this.pnlCaseNotes.ResumeLayout(false);
            this.pnlRdbBtns.ResumeLayout(false);
            this.pnlExit.ResumeLayout(false);
            this.pnlExit.PerformLayout();
            this.pnlAppNoName.ResumeLayout(false);
            this.pnlAppNoName.PerformLayout();
            this.pnlCompleteForm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCaseNotes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Label lblApplicationNo;
        private Label lblApplicationNon;
        private Label lblClientNameD;
        private Label lblClientName;
        private Panel pnlData;
        private Panel pnlAppNoName;
        private Panel pnlCompleteForm;
        private DataGridViewEx dataGridCaseNotes;
        private DataGridViewCheckBoxColumn categorychk;
        private DataGridViewTextBoxColumn ScreenName;
        private DataGridViewTextBoxColumn ReceiveDate;
        private Panel pnlExit;
        private Button btnExit;
        private Button btnPrint;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private CheckBox chkPrintName;
        private ComboBox cmbsize;
        private Label lblSize;
        private Panel pnlDesc;
        private Label lblCaseNotes;
        private TextBoxWithValidation txtDesc;
        private Panel pnlRdbBtns;
        private RadioButton rdbSel;
        private RadioButton rdbAll;
        private Spacer spacer1;
        private Panel pnlTxtDesc;
        private Panel pnlCaseNotes;
        private DataGridViewTextBoxColumn dgvDesc;
        private DataGridViewDateTimeColumn dgvDate;
        private Label lblHierarchy;
        private Spacer spacer5;
        private Spacer spacer4;
        private Spacer spacer3;
        private Spacer spacer2;
    }
}