//using Wisej.Web;
//using Gizmox.WebGUI.Common;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class ADMN0022Form
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ADMN0022Form));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlQuestions = new Wisej.Web.Panel();
            this.label17 = new Wisej.Web.Label();
            this.lblquestype = new Wisej.Web.Label();
            this.chkbQuesReq = new Wisej.Web.CheckBox();
            this.CustRespPanel = new Wisej.Web.Panel();
            this.CustRespGrid = new Wisej.Web.DataGridView();
            this.RespCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.RespDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Type = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Changed = new Wisej.Web.DataGridViewTextBoxColumn();
            this.label2 = new Wisej.Web.Label();
            this.txtQseq = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblQseq = new Wisej.Web.Label();
            this.lblQDesc = new Wisej.Web.Label();
            this.label6 = new Wisej.Web.Label();
            this.TxtQuesDesc = new Wisej.Web.TextBox();
            this.cmbQuestionType = new Wisej.Web.ComboBox();
            this.pnlQuestionType = new Wisej.Web.Panel();
            this.pnlsave = new Wisej.Web.Panel();
            this.btnSubmit = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.pnlGroup = new Wisej.Web.Panel();
            this.label3 = new Wisej.Web.Label();
            this.label1 = new Wisej.Web.Label();
            this.lblGSeq = new Wisej.Web.Label();
            this.txtGSeq = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblGDesc = new Wisej.Web.Label();
            this.txtGDesc = new Wisej.Web.TextBox();
            this.pnlService = new Wisej.Web.Panel();
            this.pnlBoiler = new Wisej.Web.Panel();
            this.chkbQuestion = new Wisej.Web.CheckBox();
            this.txtBoilerplate = new Wisej.Web.TextBox();
            this.chkSignature = new Wisej.Web.CheckBox();
            this.lblBiolerplate = new Wisej.Web.Label();
            this.pnlserviceplan = new Wisej.Web.Panel();
            this.txtServiceplan = new Wisej.Web.TextBox();
            this.lblServiceplan = new Wisej.Web.Label();
            this.lblServices = new Wisej.Web.Label();
            this.txtServices = new Wisej.Web.TextBox();
            this.btnserviceplan = new Wisej.Web.Button();
            this.btnservices = new Wisej.Web.Button();
            this.label7 = new Wisej.Web.Label();
            this.label5 = new Wisej.Web.Label();
            this.label4 = new Wisej.Web.Label();
            this.chkActive = new Wisej.Web.CheckBox();
            this.txtName = new Wisej.Web.TextBox();
            this.txt_Hierachies = new Wisej.Web.TextBox();
            this.Pb_MS_Prog = new Wisej.Web.PictureBox();
            this.lblName = new Wisej.Web.Label();
            this.lblHierchy = new Wisej.Web.Label();
            this.cmbType = new Wisej.Web.ComboBox();
            this.lblType = new Wisej.Web.Label();
            this.pnlName = new Wisej.Web.Panel();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlQuestions.SuspendLayout();
            this.CustRespPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CustRespGrid)).BeginInit();
            this.pnlsave.SuspendLayout();
            this.pnlGroup.SuspendLayout();
            this.pnlService.SuspendLayout();
            this.pnlBoiler.SuspendLayout();
            this.pnlserviceplan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_MS_Prog)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlQuestions);
            this.pnlCompleteForm.Controls.Add(this.pnlsave);
            this.pnlCompleteForm.Controls.Add(this.pnlGroup);
            this.pnlCompleteForm.Controls.Add(this.pnlService);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(728, 677);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlQuestions
            // 
            this.pnlQuestions.Controls.Add(this.label17);
            this.pnlQuestions.Controls.Add(this.lblquestype);
            this.pnlQuestions.Controls.Add(this.chkbQuesReq);
            this.pnlQuestions.Controls.Add(this.CustRespPanel);
            this.pnlQuestions.Controls.Add(this.label2);
            this.pnlQuestions.Controls.Add(this.txtQseq);
            this.pnlQuestions.Controls.Add(this.lblQseq);
            this.pnlQuestions.Controls.Add(this.lblQDesc);
            this.pnlQuestions.Controls.Add(this.label6);
            this.pnlQuestions.Controls.Add(this.TxtQuesDesc);
            this.pnlQuestions.Controls.Add(this.cmbQuestionType);
            this.pnlQuestions.Controls.Add(this.pnlQuestionType);
            this.pnlQuestions.Dock = Wisej.Web.DockStyle.Top;
            this.pnlQuestions.Location = new System.Drawing.Point(0, 321);
            this.pnlQuestions.Name = "pnlQuestions";
            this.pnlQuestions.Padding = new Wisej.Web.Padding(5);
            this.pnlQuestions.Size = new System.Drawing.Size(728, 321);
            this.pnlQuestions.TabIndex = 0;
            // 
            // label17
            // 
            this.label17.ForeColor = System.Drawing.Color.Red;
            this.label17.Location = new System.Drawing.Point(41, 58);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(10, 9);
            this.label17.TabIndex = 33;
            this.label17.Text = "*";
            // 
            // lblquestype
            // 
            this.lblquestype.Location = new System.Drawing.Point(15, 61);
            this.lblquestype.Name = "lblquestype";
            this.lblquestype.Size = new System.Drawing.Size(28, 16);
            this.lblquestype.TabIndex = 0;
            this.lblquestype.Text = "Type";
            // 
            // chkbQuesReq
            // 
            this.chkbQuesReq.AutoSize = false;
            this.chkbQuesReq.Location = new System.Drawing.Point(259, 90);
            this.chkbQuesReq.Name = "chkbQuesReq";
            this.chkbQuesReq.Size = new System.Drawing.Size(81, 20);
            this.chkbQuesReq.TabIndex = 21;
            this.chkbQuesReq.Text = "Required";
            // 
            // CustRespPanel
            // 
            this.CustRespPanel.Controls.Add(this.CustRespGrid);
            this.CustRespPanel.Dock = Wisej.Web.DockStyle.Fill;
            this.CustRespPanel.Location = new System.Drawing.Point(5, 119);
            this.CustRespPanel.Name = "CustRespPanel";
            this.CustRespPanel.Size = new System.Drawing.Size(718, 197);
            this.CustRespPanel.TabIndex = 22;
            this.CustRespPanel.Visible = false;
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
            this.Changed});
            this.CustRespGrid.Dock = Wisej.Web.DockStyle.Fill;
            this.CustRespGrid.Location = new System.Drawing.Point(0, 0);
            this.CustRespGrid.MultiSelect = false;
            this.CustRespGrid.Name = "CustRespGrid";
            this.CustRespGrid.RowHeadersWidth = 15;
            this.CustRespGrid.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.CustRespGrid.Size = new System.Drawing.Size(718, 197);
            this.CustRespGrid.TabIndex = 23;
            this.CustRespGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.CustRespGrid_CellValueChanged);
            this.CustRespGrid.SelectionChanged += new System.EventHandler(this.CustRespGrid_SelectionChanged);
            // 
            // RespCode
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.RespCode.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.RespCode.HeaderStyle = dataGridViewCellStyle3;
            this.RespCode.HeaderText = "Code";
            this.RespCode.MaxInputLength = 2;
            this.RespCode.Name = "RespCode";
            this.RespCode.Width = 55;
            // 
            // RespDesc
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.RespDesc.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.RespDesc.HeaderStyle = dataGridViewCellStyle5;
            this.RespDesc.HeaderText = "Response";
            this.RespDesc.MaxInputLength = 100;
            this.RespDesc.Name = "RespDesc";
            this.RespDesc.Width = 240;
            // 
            // Type
            // 
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Type.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Type.HeaderStyle = dataGridViewCellStyle7;
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ShowInVisibilityMenu = false;
            this.Type.Visible = false;
            // 
            // Changed
            // 
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Changed.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Changed.HeaderStyle = dataGridViewCellStyle9;
            this.Changed.HeaderText = "Changed";
            this.Changed.Name = "Changed";
            this.Changed.ShowInVisibilityMenu = false;
            this.Changed.Visible = false;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(69, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 11);
            this.label2.TabIndex = 33;
            this.label2.Text = "*";
            // 
            // txtQseq
            // 
            this.txtQseq.Location = new System.Drawing.Point(113, 89);
            this.txtQseq.MaxLength = 3;
            this.txtQseq.Name = "txtQseq";
            this.txtQseq.Size = new System.Drawing.Size(49, 25);
            this.txtQseq.TabIndex = 20;
            this.txtQseq.Leave += new System.EventHandler(this.txtQseq_Leave);
            // 
            // lblQseq
            // 
            this.lblQseq.Location = new System.Drawing.Point(15, 93);
            this.lblQseq.Name = "lblQseq";
            this.lblQseq.Size = new System.Drawing.Size(59, 16);
            this.lblQseq.TabIndex = 0;
            this.lblQseq.Text = "Sequence";
            // 
            // lblQDesc
            // 
            this.lblQDesc.Location = new System.Drawing.Point(15, 22);
            this.lblQDesc.Name = "lblQDesc";
            this.lblQDesc.Size = new System.Drawing.Size(52, 16);
            this.lblQDesc.TabIndex = 0;
            this.lblQDesc.Text = "Question";
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(66, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(10, 10);
            this.label6.TabIndex = 33;
            this.label6.Text = "*";
            // 
            // TxtQuesDesc
            // 
            this.TxtQuesDesc.Location = new System.Drawing.Point(113, 11);
            this.TxtQuesDesc.MaxLength = 100;
            this.TxtQuesDesc.Multiline = true;
            this.TxtQuesDesc.Name = "TxtQuesDesc";
            this.TxtQuesDesc.Size = new System.Drawing.Size(553, 39);
            this.TxtQuesDesc.TabIndex = 18;
            // 
            // cmbQuestionType
            // 
            this.cmbQuestionType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbQuestionType.FormattingEnabled = true;
            this.cmbQuestionType.Location = new System.Drawing.Point(113, 57);
            this.cmbQuestionType.Name = "cmbQuestionType";
            this.cmbQuestionType.Size = new System.Drawing.Size(223, 25);
            this.cmbQuestionType.TabIndex = 19;
            this.cmbQuestionType.SelectedIndexChanged += new System.EventHandler(this.cmbQuestionType_SelectedIndexChanged);
            // 
            // pnlQuestionType
            // 
            this.pnlQuestionType.Dock = Wisej.Web.DockStyle.Top;
            this.pnlQuestionType.Location = new System.Drawing.Point(5, 5);
            this.pnlQuestionType.Name = "pnlQuestionType";
            this.pnlQuestionType.Size = new System.Drawing.Size(718, 114);
            this.pnlQuestionType.TabIndex = 17;
            // 
            // pnlsave
            // 
            this.pnlsave.AppearanceKey = "panel-grdo";
            this.pnlsave.Controls.Add(this.btnSubmit);
            this.pnlsave.Controls.Add(this.spacer1);
            this.pnlsave.Controls.Add(this.btnCancel);
            this.pnlsave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlsave.Location = new System.Drawing.Point(0, 642);
            this.pnlsave.Name = "pnlsave";
            this.pnlsave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlsave.Size = new System.Drawing.Size(728, 35);
            this.pnlsave.TabIndex = 24;
            // 
            // btnSubmit
            // 
            this.btnSubmit.AppearanceKey = "button-ok";
            this.btnSubmit.Dock = Wisej.Web.DockStyle.Right;
            this.btnSubmit.Location = new System.Drawing.Point(560, 5);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 25);
            this.btnSubmit.TabIndex = 25;
            this.btnSubmit.Text = "&Save";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(635, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(638, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "&Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlGroup
            // 
            this.pnlGroup.Controls.Add(this.label3);
            this.pnlGroup.Controls.Add(this.label1);
            this.pnlGroup.Controls.Add(this.lblGSeq);
            this.pnlGroup.Controls.Add(this.txtGSeq);
            this.pnlGroup.Controls.Add(this.lblGDesc);
            this.pnlGroup.Controls.Add(this.txtGDesc);
            this.pnlGroup.Dock = Wisej.Web.DockStyle.Top;
            this.pnlGroup.Location = new System.Drawing.Point(0, 246);
            this.pnlGroup.Name = "pnlGroup";
            this.pnlGroup.Padding = new Wisej.Web.Padding(5);
            this.pnlGroup.Size = new System.Drawing.Size(728, 75);
            this.pnlGroup.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(80, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(10, 10);
            this.label3.TabIndex = 33;
            this.label3.Text = "*";
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(70, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 12);
            this.label1.TabIndex = 33;
            this.label1.Text = "*";
            // 
            // lblGSeq
            // 
            this.lblGSeq.Location = new System.Drawing.Point(15, 10);
            this.lblGSeq.Name = "lblGSeq";
            this.lblGSeq.Size = new System.Drawing.Size(56, 16);
            this.lblGSeq.TabIndex = 0;
            this.lblGSeq.Text = "Sequence";
            // 
            // txtGSeq
            // 
            this.txtGSeq.AutoSize = false;
            this.txtGSeq.Location = new System.Drawing.Point(113, 6);
            this.txtGSeq.MaxLength = 3;
            this.txtGSeq.Name = "txtGSeq";
            this.txtGSeq.Size = new System.Drawing.Size(49, 25);
            this.txtGSeq.TabIndex = 15;
            // 
            // lblGDesc
            // 
            this.lblGDesc.Location = new System.Drawing.Point(15, 42);
            this.lblGDesc.Name = "lblGDesc";
            this.lblGDesc.Size = new System.Drawing.Size(66, 16);
            this.lblGDesc.TabIndex = 0;
            this.lblGDesc.Text = "Description";
            // 
            // txtGDesc
            // 
            this.txtGDesc.Location = new System.Drawing.Point(113, 38);
            this.txtGDesc.MaxLength = 100;
            this.txtGDesc.Name = "txtGDesc";
            this.txtGDesc.Size = new System.Drawing.Size(506, 25);
            this.txtGDesc.TabIndex = 16;
            // 
            // pnlService
            // 
            this.pnlService.Controls.Add(this.pnlBoiler);
            this.pnlService.Controls.Add(this.pnlserviceplan);
            this.pnlService.Controls.Add(this.label7);
            this.pnlService.Controls.Add(this.label5);
            this.pnlService.Controls.Add(this.label4);
            this.pnlService.Controls.Add(this.chkActive);
            this.pnlService.Controls.Add(this.txtName);
            this.pnlService.Controls.Add(this.txt_Hierachies);
            this.pnlService.Controls.Add(this.Pb_MS_Prog);
            this.pnlService.Controls.Add(this.lblName);
            this.pnlService.Controls.Add(this.lblHierchy);
            this.pnlService.Controls.Add(this.cmbType);
            this.pnlService.Controls.Add(this.lblType);
            this.pnlService.Controls.Add(this.pnlName);
            this.pnlService.Dock = Wisej.Web.DockStyle.Top;
            this.pnlService.Location = new System.Drawing.Point(0, 0);
            this.pnlService.Name = "pnlService";
            this.pnlService.Padding = new Wisej.Web.Padding(5);
            this.pnlService.Size = new System.Drawing.Size(728, 246);
            this.pnlService.TabIndex = 0;
            // 
            // pnlBoiler
            // 
            this.pnlBoiler.Controls.Add(this.chkbQuestion);
            this.pnlBoiler.Controls.Add(this.txtBoilerplate);
            this.pnlBoiler.Controls.Add(this.chkSignature);
            this.pnlBoiler.Controls.Add(this.lblBiolerplate);
            this.pnlBoiler.Dock = Wisej.Web.DockStyle.Top;
            this.pnlBoiler.Location = new System.Drawing.Point(5, 133);
            this.pnlBoiler.Name = "pnlBoiler";
            this.pnlBoiler.Size = new System.Drawing.Size(718, 107);
            this.pnlBoiler.TabIndex = 10;
            // 
            // chkbQuestion
            // 
            this.chkbQuestion.AutoSize = false;
            this.chkbQuestion.Location = new System.Drawing.Point(539, 83);
            this.chkbQuestion.Name = "chkbQuestion";
            this.chkbQuestion.Size = new System.Drawing.Size(76, 20);
            this.chkbQuestion.TabIndex = 13;
            this.chkbQuestion.Text = "5 Quests";
            this.chkbQuestion.Visible = false;
            this.chkbQuestion.CheckedChanged += new System.EventHandler(this.chkbQuestion_CheckedChanged);
            // 
            // txtBoilerplate
            // 
            this.txtBoilerplate.Location = new System.Drawing.Point(108, 4);
            this.txtBoilerplate.Multiline = true;
            this.txtBoilerplate.Name = "txtBoilerplate";
            this.txtBoilerplate.Size = new System.Drawing.Size(507, 70);
            this.txtBoilerplate.TabIndex = 11;
            // 
            // chkSignature
            // 
            this.chkSignature.AutoSize = false;
            this.chkSignature.Location = new System.Drawing.Point(3, 82);
            this.chkSignature.Name = "chkSignature";
            this.chkSignature.Size = new System.Drawing.Size(142, 20);
            this.chkSignature.TabIndex = 12;
            this.chkSignature.Text = "Signature Required";
            // 
            // lblBiolerplate
            // 
            this.lblBiolerplate.Location = new System.Drawing.Point(10, 9);
            this.lblBiolerplate.Name = "lblBiolerplate";
            this.lblBiolerplate.Size = new System.Drawing.Size(66, 14);
            this.lblBiolerplate.TabIndex = 35;
            this.lblBiolerplate.Text = "Boiler Plate";
            // 
            // pnlserviceplan
            // 
            this.pnlserviceplan.Controls.Add(this.txtServiceplan);
            this.pnlserviceplan.Controls.Add(this.lblServiceplan);
            this.pnlserviceplan.Controls.Add(this.lblServices);
            this.pnlserviceplan.Controls.Add(this.txtServices);
            this.pnlserviceplan.Controls.Add(this.btnserviceplan);
            this.pnlserviceplan.Controls.Add(this.btnservices);
            this.pnlserviceplan.Dock = Wisej.Web.DockStyle.Top;
            this.pnlserviceplan.Location = new System.Drawing.Point(5, 70);
            this.pnlserviceplan.Name = "pnlserviceplan";
            this.pnlserviceplan.Size = new System.Drawing.Size(718, 63);
            this.pnlserviceplan.TabIndex = 5;
            // 
            // txtServiceplan
            // 
            this.txtServiceplan.Location = new System.Drawing.Point(108, 3);
            this.txtServiceplan.MaxLength = 100;
            this.txtServiceplan.Name = "txtServiceplan";
            this.txtServiceplan.ReadOnly = true;
            this.txtServiceplan.Size = new System.Drawing.Size(507, 25);
            this.txtServiceplan.TabIndex = 6;
            // 
            // lblServiceplan
            // 
            this.lblServiceplan.Location = new System.Drawing.Point(10, 12);
            this.lblServiceplan.Name = "lblServiceplan";
            this.lblServiceplan.Size = new System.Drawing.Size(83, 14);
            this.lblServiceplan.TabIndex = 0;
            this.lblServiceplan.Text = "Service Plan(s)";
            // 
            // lblServices
            // 
            this.lblServices.Location = new System.Drawing.Point(10, 39);
            this.lblServices.Name = "lblServices";
            this.lblServices.Size = new System.Drawing.Size(56, 14);
            this.lblServices.TabIndex = 0;
            this.lblServices.Text = "Service(s)";
            // 
            // txtServices
            // 
            this.txtServices.Location = new System.Drawing.Point(108, 35);
            this.txtServices.MaxLength = 500;
            this.txtServices.Name = "txtServices";
            this.txtServices.ReadOnly = true;
            this.txtServices.Size = new System.Drawing.Size(507, 25);
            this.txtServices.TabIndex = 8;
            // 
            // btnserviceplan
            // 
            this.btnserviceplan.Location = new System.Drawing.Point(638, 3);
            this.btnserviceplan.Name = "btnserviceplan";
            this.btnserviceplan.Size = new System.Drawing.Size(40, 25);
            this.btnserviceplan.TabIndex = 7;
            this.btnserviceplan.Text = "....";
            this.btnserviceplan.Click += new System.EventHandler(this.btnserviceplan_Click);
            // 
            // btnservices
            // 
            this.btnservices.Location = new System.Drawing.Point(638, 35);
            this.btnservices.Name = "btnservices";
            this.btnservices.Size = new System.Drawing.Size(40, 25);
            this.btnservices.TabIndex = 9;
            this.btnservices.Text = "....";
            this.btnservices.Click += new System.EventHandler(this.btnservices_Click);
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(49, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(10, 11);
            this.label7.TabIndex = 33;
            this.label7.Text = "*";
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(528, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(10, 10);
            this.label5.TabIndex = 33;
            this.label5.Text = "*";
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(42, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(10, 11);
            this.label4.TabIndex = 33;
            this.label4.Text = "*";
            // 
            // chkActive
            // 
            this.chkActive.Appearance = Wisej.Web.Appearance.Switch;
            this.chkActive.AutoSize = false;
            this.chkActive.Location = new System.Drawing.Point(643, 11);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(69, 20);
            this.chkActive.TabIndex = 2;
            this.chkActive.Text = "Active";
            // 
            // txtName
            // 
            this.txtName.AutoSize = false;
            this.txtName.Location = new System.Drawing.Point(113, 9);
            this.txtName.MaxLength = 100;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(507, 25);
            this.txtName.TabIndex = 1;
            // 
            // txt_Hierachies
            // 
            this.txt_Hierachies.Enabled = false;
            this.txt_Hierachies.Location = new System.Drawing.Point(550, 41);
            this.txt_Hierachies.MaxLength = 100;
            this.txt_Hierachies.Name = "txt_Hierachies";
            this.txt_Hierachies.ReadOnly = true;
            this.txt_Hierachies.Size = new System.Drawing.Size(70, 25);
            this.txt_Hierachies.TabIndex = 4;
            // 
            // Pb_MS_Prog
            // 
            this.Pb_MS_Prog.Cursor = Wisej.Web.Cursors.Hand;
            this.Pb_MS_Prog.ImageSource = "captain-filter";
            this.Pb_MS_Prog.Location = new System.Drawing.Point(647, 45);
            this.Pb_MS_Prog.Name = "Pb_MS_Prog";
            this.Pb_MS_Prog.Size = new System.Drawing.Size(20, 20);
            this.Pb_MS_Prog.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.Pb_MS_Prog.Click += new System.EventHandler(this.Pb_MS_Prog_Click);
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(15, 13);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 14);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            // 
            // lblHierchy
            // 
            this.lblHierchy.Location = new System.Drawing.Point(472, 45);
            this.lblHierchy.Name = "lblHierchy";
            this.lblHierchy.Size = new System.Drawing.Size(56, 16);
            this.lblHierchy.TabIndex = 0;
            this.lblHierchy.Text = "Hierarchy";
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(113, 41);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(170, 25);
            this.cmbType.TabIndex = 3;
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged_1);
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(15, 45);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(28, 16);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "Type";
            // 
            // pnlName
            // 
            this.pnlName.Dock = Wisej.Web.DockStyle.Top;
            this.pnlName.Location = new System.Drawing.Point(5, 5);
            this.pnlName.Name = "pnlName";
            this.pnlName.Size = new System.Drawing.Size(718, 65);
            this.pnlName.TabIndex = 1;
            // 
            // ADMN0022Form
            // 
            this.ClientSize = new System.Drawing.Size(728, 677);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ADMN0022Form";
            this.Text = "Contact and Service Activity Custom Questions";
            componentTool1.ImageSource = "icon-help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlQuestions.ResumeLayout(false);
            this.pnlQuestions.PerformLayout();
            this.CustRespPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CustRespGrid)).EndInit();
            this.pnlsave.ResumeLayout(false);
            this.pnlGroup.ResumeLayout(false);
            this.pnlGroup.PerformLayout();
            this.pnlService.ResumeLayout(false);
            this.pnlService.PerformLayout();
            this.pnlBoiler.ResumeLayout(false);
            this.pnlBoiler.PerformLayout();
            this.pnlserviceplan.ResumeLayout(false);
            this.pnlserviceplan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_MS_Prog)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion
        private Panel pnlCompleteForm;
        private Panel pnlGroup;
        private Panel pnlService;
        private Label lblType;
        private Label lblServices;
        private Label lblServiceplan;
        private Label lblHierchy;
        private ComboBox cmbType;
        private Label lblName;
        private CheckBox chkActive;
        private TextBox txtName;
        private TextBox txtServices;
        private TextBox txtServiceplan;
        private TextBox txt_Hierachies;
        private PictureBox Pb_MS_Prog;
        private Panel pnlsave;
        private Button btnCancel;
        private Button btnSubmit;
        private Button btnservices;
        private Button btnserviceplan;
        private Label lblGSeq;
        private TextBoxWithValidation txtGSeq;
        private Label lblGDesc;
        private TextBox txtGDesc;
        private Panel pnlQuestions;
        private Label lblQDesc;
        private Label lblquestype;
        private Label label17;
        private Label label6;
        private TextBox TxtQuesDesc;
        private ComboBox cmbQuestionType;
        private DataGridView CustRespGrid;
        private DataGridViewTextBoxColumn RespCode;
        private DataGridViewTextBoxColumn RespDesc;
        private DataGridViewTextBoxColumn Type;
        private DataGridViewTextBoxColumn Changed;
        private Label label2;
        private TextBoxWithValidation txtQseq;
        private Label lblQseq;
        private Label label3;
        private Label label1;
        private Label label7;
        private Label label5;
        private Label label4;
        private Panel CustRespPanel;
        private Panel pnlserviceplan;
        private Label lblBiolerplate;
        private TextBox txtBoilerplate;
        private CheckBox chkbQuesReq;
        private Spacer spacer1;
        private Panel pnlName;
        private Panel pnlBoiler;
        private Panel pnlQuestionType;
        private CheckBox chkbQuestion;
        private CheckBox chkSignature;
    }
}