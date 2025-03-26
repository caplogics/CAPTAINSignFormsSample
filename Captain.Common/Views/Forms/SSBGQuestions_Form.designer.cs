using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
using Captain.Common.Utilities;

namespace Captain.Common.Views.Forms
{
    partial class SSBGQuestions_Form
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle10 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle11 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle12 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle13 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle14 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle15 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle16 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle17 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle18 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle19 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SSBGQuestions_Form));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.lblType = new Wisej.Web.Label();
            this.cmbType = new Wisej.Web.ComboBox();
            this.pnlOutcomes = new Wisej.Web.Panel();
            this.pnlgvwQuestions = new Wisej.Web.Panel();
            this.gvwQuestions = new Wisej.Web.DataGridView();
            this.QuestionDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Responce = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Conjunction = new Wisej.Web.DataGridViewTextBoxColumn();
            this.QTypeCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.QuestionId = new Wisej.Web.DataGridViewTextBoxColumn();
            this.QSeq = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Edit = new Wisej.Web.DataGridViewImageColumn();
            this.Delete = new Wisej.Web.DataGridViewImageColumn();
            this.pnlQTop = new Wisej.Web.Panel();
            this.label6 = new Wisej.Web.Label();
            this.cmbQues_Group = new Wisej.Web.ComboBox();
            this.pnlQFields = new Wisej.Web.Panel();
            this.pnlQuesAdd = new Wisej.Web.Panel();
            this.picAddQues = new Wisej.Web.PictureBox();
            this.pnlQuestions = new Wisej.Web.Panel();
            this.lblName = new Wisej.Web.Label();
            this.txtName = new Wisej.Web.TextBox();
            this.label9 = new Wisej.Web.Label();
            this.labelNResp = new Wisej.Web.Label();
            this.label7 = new Wisej.Web.Label();
            this.label1 = new Wisej.Web.Label();
            this.label2 = new Wisej.Web.Label();
            this.lblQuestion = new Wisej.Web.Label();
            this.cmbQuestions = new Wisej.Web.ComboBox();
            this.lblMemAccess = new Wisej.Web.Label();
            this.cmbMemAccess = new Wisej.Web.ComboBox();
            this.cmbResponce = new Wisej.Web.ComboBox();
            this.lblResponse = new Wisej.Web.Label();
            this.lblEqualTo = new Wisej.Web.Label();
            this.txtEqualTo = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtGreaterthan = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblGreaterthan = new Wisej.Web.Label();
            this.txtLessthan = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.cmbQuestionConjuction = new Wisej.Web.ComboBox();
            this.label4 = new Wisej.Web.Label();
            this.lblLessthan = new Wisej.Web.Label();
            this.pnlQSave = new Wisej.Web.Panel();
            this.btnQSave = new Wisej.Web.Button();
            this.spacer2 = new Wisej.Web.Spacer();
            this.btnQCancel = new Wisej.Web.Button();
            this.picGroupAdd = new Wisej.Web.PictureBox();
            this.btnCancel = new Wisej.Web.Button();
            this.btnSave = new Wisej.Web.Button();
            this.label3 = new Wisej.Web.Label();
            this.label8 = new Wisej.Web.Label();
            this.lblSeq = new Wisej.Web.Label();
            this.txtSeq = new Wisej.Web.TextBox();
            this.txtGrpCode = new Wisej.Web.TextBox();
            this.lblGrp_code = new Wisej.Web.Label();
            this.pnlFields = new Wisej.Web.Panel();
            this.cmbConjunction = new Wisej.Web.ComboBox();
            this.lblConjunction = new Wisej.Web.Label();
            this.txtGroupDesc = new Wisej.Web.TextBox();
            this.lblGroupDesc = new Wisej.Web.Label();
            this.label5 = new Wisej.Web.Label();
            this.gvMat_Groups = new Wisej.Web.DataGridView();
            this.GrpCd = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Group = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Grp_Conj = new Wisej.Web.DataGridViewTextBoxColumn();
            this.GSeq = new Wisej.Web.DataGridViewTextBoxColumn();
            this.GrpEdit = new Wisej.Web.DataGridViewImageColumn();
            this.GrpDel = new Wisej.Web.DataGridViewImageColumn();
            this.Groups = new Wisej.Web.TabPage();
            this.pnlGroups = new Wisej.Web.Panel();
            this.pnlgvMat_Groups = new Wisej.Web.Panel();
            this.pnlGSave = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlType = new Wisej.Web.Panel();
            this.lblGrpType = new Wisej.Web.Label();
            this.cmbGrpType = new Wisej.Web.ComboBox();
            this.Questions = new Wisej.Web.TabPage();
            this.tabControlGQ = new Wisej.Web.TabControl();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlOutcomes.SuspendLayout();
            this.pnlgvwQuestions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwQuestions)).BeginInit();
            this.pnlQTop.SuspendLayout();
            this.pnlQFields.SuspendLayout();
            this.pnlQuesAdd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAddQues)).BeginInit();
            this.pnlQuestions.SuspendLayout();
            this.pnlQSave.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGroupAdd)).BeginInit();
            this.pnlFields.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvMat_Groups)).BeginInit();
            this.Groups.SuspendLayout();
            this.pnlGroups.SuspendLayout();
            this.pnlgvMat_Groups.SuspendLayout();
            this.pnlGSave.SuspendLayout();
            this.pnlType.SuspendLayout();
            this.Questions.SuspendLayout();
            this.tabControlGQ.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(15, 15);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(27, 17);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "Type";
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(70, 11);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(520, 25);
            this.cmbType.TabIndex = 9;
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // pnlOutcomes
            // 
            this.pnlOutcomes.Controls.Add(this.pnlgvwQuestions);
            this.pnlOutcomes.Controls.Add(this.pnlQTop);
            this.pnlOutcomes.Controls.Add(this.pnlQFields);
            this.pnlOutcomes.Controls.Add(this.pnlQSave);
            this.pnlOutcomes.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlOutcomes.Location = new System.Drawing.Point(0, 0);
            this.pnlOutcomes.Name = "pnlOutcomes";
            this.pnlOutcomes.Size = new System.Drawing.Size(621, 441);
            this.pnlOutcomes.TabIndex = 1;
            // 
            // pnlgvwQuestions
            // 
            this.pnlgvwQuestions.Controls.Add(this.gvwQuestions);
            this.pnlgvwQuestions.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwQuestions.Location = new System.Drawing.Point(0, 76);
            this.pnlgvwQuestions.Name = "pnlgvwQuestions";
            this.pnlgvwQuestions.Size = new System.Drawing.Size(621, 192);
            this.pnlgvwQuestions.TabIndex = 12;
            // 
            // gvwQuestions
            // 
            this.gvwQuestions.AllowUserToResizeColumns = false;
            this.gvwQuestions.AllowUserToResizeRows = false;
            this.gvwQuestions.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwQuestions.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwQuestions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwQuestions.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwQuestions.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.QuestionDesc,
            this.Responce,
            this.Conjunction,
            this.QTypeCode,
            this.QuestionId,
            this.QSeq,
            this.Edit,
            this.Delete});
            this.gvwQuestions.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwQuestions.Location = new System.Drawing.Point(0, 0);
            this.gvwQuestions.MultiSelect = false;
            this.gvwQuestions.Name = "gvwQuestions";
            this.gvwQuestions.ReadOnly = true;
            this.gvwQuestions.RowHeadersWidth = 14;
            this.gvwQuestions.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwQuestions.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvwQuestions.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwQuestions.Size = new System.Drawing.Size(621, 192);
            this.gvwQuestions.TabIndex = 0;
            this.gvwQuestions.SelectionChanged += new System.EventHandler(this.gvwQuestions_SelectionChanged);
            this.gvwQuestions.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwQuestions_CellClick);
            // 
            // QuestionDesc
            // 
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.QuestionDesc.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.QuestionDesc.HeaderStyle = dataGridViewCellStyle3;
            this.QuestionDesc.HeaderText = "Question";
            this.QuestionDesc.Name = "QuestionDesc";
            this.QuestionDesc.ReadOnly = true;
            this.QuestionDesc.Width = 250;
            // 
            // Responce
            // 
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Responce.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Responce.HeaderStyle = dataGridViewCellStyle5;
            this.Responce.HeaderText = "Response";
            this.Responce.Name = "Responce";
            this.Responce.ReadOnly = true;
            this.Responce.Width = 150;
            // 
            // Conjunction
            // 
            dataGridViewCellStyle6.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Conjunction.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Conjunction.HeaderStyle = dataGridViewCellStyle7;
            this.Conjunction.HeaderText = "Conjunction";
            this.Conjunction.Name = "Conjunction";
            this.Conjunction.ReadOnly = true;
            this.Conjunction.Width = 90;
            // 
            // QTypeCode
            // 
            this.QTypeCode.HeaderText = "QTypeCode";
            this.QTypeCode.Name = "QTypeCode";
            this.QTypeCode.ReadOnly = true;
            this.QTypeCode.ShowInVisibilityMenu = false;
            this.QTypeCode.Visible = false;
            this.QTypeCode.Width = 20;
            // 
            // QuestionId
            // 
            this.QuestionId.HeaderText = "QuestionId";
            this.QuestionId.Name = "QuestionId";
            this.QuestionId.ReadOnly = true;
            this.QuestionId.ShowInVisibilityMenu = false;
            this.QuestionId.Visible = false;
            this.QuestionId.Width = 20;
            // 
            // QSeq
            // 
            this.QSeq.HeaderText = "QSeq";
            this.QSeq.Name = "QSeq";
            this.QSeq.ReadOnly = true;
            this.QSeq.ShowInVisibilityMenu = false;
            this.QSeq.Visible = false;
            this.QSeq.Width = 20;
            // 
            // Edit
            // 
            this.Edit.CellImageAlignment = Wisej.Web.DataGridViewContentAlignment.NotSet;
            this.Edit.CellImageSource = "captain-edit";
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.NullValue = null;
            this.Edit.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.Edit.HeaderStyle = dataGridViewCellStyle9;
            this.Edit.HeaderText = "Edit";
            this.Edit.Name = "Edit";
            this.Edit.ReadOnly = true;
            this.Edit.ShowInVisibilityMenu = false;
            this.Edit.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.Edit.Width = 40;
            // 
            // Delete
            // 
            this.Delete.CellImageAlignment = Wisej.Web.DataGridViewContentAlignment.NotSet;
            this.Delete.CellImageSource = "captain-delete";
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.NullValue = null;
            this.Delete.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.Delete.HeaderStyle = dataGridViewCellStyle11;
            this.Delete.HeaderText = "Delete";
            this.Delete.Name = "Delete";
            this.Delete.ReadOnly = true;
            this.Delete.ShowInVisibilityMenu = false;
            this.Delete.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.Delete.Width = 40;
            // 
            // pnlQTop
            // 
            this.pnlQTop.Controls.Add(this.label6);
            this.pnlQTop.Controls.Add(this.cmbType);
            this.pnlQTop.Controls.Add(this.lblType);
            this.pnlQTop.Controls.Add(this.cmbQues_Group);
            this.pnlQTop.Dock = Wisej.Web.DockStyle.Top;
            this.pnlQTop.Location = new System.Drawing.Point(0, 0);
            this.pnlQTop.Name = "pnlQTop";
            this.pnlQTop.Size = new System.Drawing.Size(621, 76);
            this.pnlQTop.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(15, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 16);
            this.label6.TabIndex = 0;
            this.label6.Text = "Group";
            // 
            // cmbQues_Group
            // 
            this.cmbQues_Group.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbQues_Group.FormattingEnabled = true;
            this.cmbQues_Group.Location = new System.Drawing.Point(70, 42);
            this.cmbQues_Group.Name = "cmbQues_Group";
            this.cmbQues_Group.Size = new System.Drawing.Size(520, 25);
            this.cmbQues_Group.TabIndex = 1;
            this.cmbQues_Group.SelectedIndexChanged += new System.EventHandler(this.cmbQues_Group_SelectedIndexChanged_1);
            // 
            // pnlQFields
            // 
            this.pnlQFields.Controls.Add(this.pnlQuesAdd);
            this.pnlQFields.Controls.Add(this.pnlQuestions);
            this.pnlQFields.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlQFields.Location = new System.Drawing.Point(0, 268);
            this.pnlQFields.Name = "pnlQFields";
            this.pnlQFields.Size = new System.Drawing.Size(621, 138);
            this.pnlQFields.TabIndex = 11;
            // 
            // pnlQuesAdd
            // 
            this.pnlQuesAdd.Controls.Add(this.picAddQues);
            this.pnlQuesAdd.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlQuesAdd.Location = new System.Drawing.Point(576, 0);
            this.pnlQuesAdd.Name = "pnlQuesAdd";
            this.pnlQuesAdd.Padding = new Wisej.Web.Padding(0, 11, 15, 100);
            this.pnlQuesAdd.Size = new System.Drawing.Size(45, 138);
            this.pnlQuesAdd.TabIndex = 3;
            // 
            // picAddQues
            // 
            this.picAddQues.Cursor = Wisej.Web.Cursors.Hand;
            this.picAddQues.Dock = Wisej.Web.DockStyle.Left;
            this.picAddQues.ImageSource = "captain-add";
            this.picAddQues.Location = new System.Drawing.Point(0, 11);
            this.picAddQues.Name = "picAddQues";
            this.picAddQues.Size = new System.Drawing.Size(20, 27);
            this.picAddQues.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.picAddQues.ToolTipText = "Add Question";
            this.picAddQues.Click += new System.EventHandler(this.picAddQues_Click);
            // 
            // pnlQuestions
            // 
            this.pnlQuestions.Controls.Add(this.lblName);
            this.pnlQuestions.Controls.Add(this.txtName);
            this.pnlQuestions.Controls.Add(this.label9);
            this.pnlQuestions.Controls.Add(this.labelNResp);
            this.pnlQuestions.Controls.Add(this.label7);
            this.pnlQuestions.Controls.Add(this.label1);
            this.pnlQuestions.Controls.Add(this.label2);
            this.pnlQuestions.Controls.Add(this.lblQuestion);
            this.pnlQuestions.Controls.Add(this.cmbQuestions);
            this.pnlQuestions.Controls.Add(this.lblMemAccess);
            this.pnlQuestions.Controls.Add(this.cmbMemAccess);
            this.pnlQuestions.Controls.Add(this.cmbResponce);
            this.pnlQuestions.Controls.Add(this.lblResponse);
            this.pnlQuestions.Controls.Add(this.lblEqualTo);
            this.pnlQuestions.Controls.Add(this.txtEqualTo);
            this.pnlQuestions.Controls.Add(this.txtGreaterthan);
            this.pnlQuestions.Controls.Add(this.lblGreaterthan);
            this.pnlQuestions.Controls.Add(this.txtLessthan);
            this.pnlQuestions.Controls.Add(this.cmbQuestionConjuction);
            this.pnlQuestions.Controls.Add(this.label4);
            this.pnlQuestions.Controls.Add(this.lblLessthan);
            this.pnlQuestions.Dock = Wisej.Web.DockStyle.Left;
            this.pnlQuestions.Enabled = false;
            this.pnlQuestions.Location = new System.Drawing.Point(0, 0);
            this.pnlQuestions.Name = "pnlQuestions";
            this.pnlQuestions.Size = new System.Drawing.Size(576, 138);
            this.pnlQuestions.TabIndex = 0;
            this.pnlQuestions.Visible = false;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(15, 108);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(37, 16);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(105, 104);
            this.txtName.MaxLength = 15;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(183, 25);
            this.txtName.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(69, 75);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(9, 14);
            this.label9.TabIndex = 28;
            this.label9.Text = "*";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label9.Visible = false;
            // 
            // labelNResp
            // 
            this.labelNResp.Location = new System.Drawing.Point(15, 77);
            this.labelNResp.Name = "labelNResp";
            this.labelNResp.Size = new System.Drawing.Size(59, 16);
            this.labelNResp.TabIndex = 3;
            this.labelNResp.Text = "Response";
            this.labelNResp.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(295, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(9, 14);
            this.label7.TabIndex = 28;
            this.label7.Text = "*";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label7.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(85, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(9, 14);
            this.label1.TabIndex = 28;
            this.label1.Text = "*";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(64, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(9, 14);
            this.label2.TabIndex = 28;
            this.label2.Text = "*";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblQuestion
            // 
            this.lblQuestion.Location = new System.Drawing.Point(13, 15);
            this.lblQuestion.Name = "lblQuestion";
            this.lblQuestion.Size = new System.Drawing.Size(54, 16);
            this.lblQuestion.TabIndex = 1;
            this.lblQuestion.Text = "Question";
            // 
            // cmbQuestions
            // 
            this.cmbQuestions.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbQuestions.FormattingEnabled = true;
            this.cmbQuestions.Location = new System.Drawing.Point(105, 11);
            this.cmbQuestions.Name = "cmbQuestions";
            this.cmbQuestions.Size = new System.Drawing.Size(440, 25);
            this.cmbQuestions.TabIndex = 7;
            this.cmbQuestions.SelectedIndexChanged += new System.EventHandler(this.cmbQuestions_SelectedIndexChanged);
            // 
            // lblMemAccess
            // 
            this.lblMemAccess.Location = new System.Drawing.Point(15, 46);
            this.lblMemAccess.Name = "lblMemAccess";
            this.lblMemAccess.Size = new System.Drawing.Size(71, 16);
            this.lblMemAccess.TabIndex = 3;
            this.lblMemAccess.Text = "Mem Access";
            // 
            // cmbMemAccess
            // 
            this.cmbMemAccess.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbMemAccess.FormattingEnabled = true;
            this.cmbMemAccess.Location = new System.Drawing.Point(105, 42);
            this.cmbMemAccess.Name = "cmbMemAccess";
            this.cmbMemAccess.Size = new System.Drawing.Size(105, 25);
            this.cmbMemAccess.TabIndex = 8;
            this.cmbMemAccess.SelectedIndexChanged += new System.EventHandler(this.cmbMemAccess_SelectedIndexChanged);
            // 
            // cmbResponce
            // 
            this.cmbResponce.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbResponce.FormattingEnabled = true;
            this.cmbResponce.Location = new System.Drawing.Point(315, 42);
            this.cmbResponce.Name = "cmbResponce";
            this.cmbResponce.Size = new System.Drawing.Size(230, 25);
            this.cmbResponce.TabIndex = 9;
            this.cmbResponce.Visible = false;
            // 
            // lblResponse
            // 
            this.lblResponse.Location = new System.Drawing.Point(241, 46);
            this.lblResponse.Name = "lblResponse";
            this.lblResponse.Size = new System.Drawing.Size(54, 16);
            this.lblResponse.TabIndex = 3;
            this.lblResponse.Text = "Response";
            this.lblResponse.Visible = false;
            // 
            // lblEqualTo
            // 
            this.lblEqualTo.Font = new System.Drawing.Font("defaultBold", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblEqualTo.Location = new System.Drawing.Point(106, 75);
            this.lblEqualTo.Name = "lblEqualTo";
            this.lblEqualTo.Size = new System.Drawing.Size(10, 15);
            this.lblEqualTo.TabIndex = 3;
            this.lblEqualTo.Text = "=";
            this.lblEqualTo.Visible = false;
            // 
            // txtEqualTo
            // 
            this.txtEqualTo.Location = new System.Drawing.Point(129, 73);
            this.txtEqualTo.MaxLength = 10;
            this.txtEqualTo.Name = "txtEqualTo";
            this.txtEqualTo.Size = new System.Drawing.Size(55, 25);
            this.txtEqualTo.TabIndex = 10;
            this.txtEqualTo.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtEqualTo.Visible = false;
            this.txtEqualTo.Leave += new System.EventHandler(this.txtEqualTo_Leave);
            // 
            // txtGreaterthan
            // 
            this.txtGreaterthan.Location = new System.Drawing.Point(225, 73);
            this.txtGreaterthan.MaxLength = 10;
            this.txtGreaterthan.Name = "txtGreaterthan";
            this.txtGreaterthan.Size = new System.Drawing.Size(55, 25);
            this.txtGreaterthan.TabIndex = 11;
            this.txtGreaterthan.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtGreaterthan.Visible = false;
            this.txtGreaterthan.Leave += new System.EventHandler(this.txtGreaterthan_Leave);
            // 
            // lblGreaterthan
            // 
            this.lblGreaterthan.Font = new System.Drawing.Font("defaultBold", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblGreaterthan.Location = new System.Drawing.Point(205, 75);
            this.lblGreaterthan.Name = "lblGreaterthan";
            this.lblGreaterthan.Size = new System.Drawing.Size(9, 15);
            this.lblGreaterthan.TabIndex = 3;
            this.lblGreaterthan.Text = ">";
            this.lblGreaterthan.Visible = false;
            // 
            // txtLessthan
            // 
            this.txtLessthan.Location = new System.Drawing.Point(312, 73);
            this.txtLessthan.MaxLength = 10;
            this.txtLessthan.Name = "txtLessthan";
            this.txtLessthan.Size = new System.Drawing.Size(54, 25);
            this.txtLessthan.TabIndex = 12;
            this.txtLessthan.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtLessthan.Visible = false;
            this.txtLessthan.Leave += new System.EventHandler(this.txtLessthan_Leave);
            // 
            // cmbQuestionConjuction
            // 
            this.cmbQuestionConjuction.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbQuestionConjuction.FormattingEnabled = true;
            this.cmbQuestionConjuction.Location = new System.Drawing.Point(485, 73);
            this.cmbQuestionConjuction.Name = "cmbQuestionConjuction";
            this.cmbQuestionConjuction.Size = new System.Drawing.Size(60, 25);
            this.cmbQuestionConjuction.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(402, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Conjunction";
            // 
            // lblLessthan
            // 
            this.lblLessthan.Font = new System.Drawing.Font("defaultBold", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblLessthan.Location = new System.Drawing.Point(292, 75);
            this.lblLessthan.Name = "lblLessthan";
            this.lblLessthan.Size = new System.Drawing.Size(11, 15);
            this.lblLessthan.TabIndex = 2;
            this.lblLessthan.Text = "<";
            this.lblLessthan.Visible = false;
            // 
            // pnlQSave
            // 
            this.pnlQSave.AppearanceKey = "panel-grdo";
            this.pnlQSave.Controls.Add(this.btnQSave);
            this.pnlQSave.Controls.Add(this.spacer2);
            this.pnlQSave.Controls.Add(this.btnQCancel);
            this.pnlQSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlQSave.Location = new System.Drawing.Point(0, 406);
            this.pnlQSave.Name = "pnlQSave";
            this.pnlQSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlQSave.Size = new System.Drawing.Size(621, 35);
            this.pnlQSave.TabIndex = 10;
            // 
            // btnQSave
            // 
            this.btnQSave.AppearanceKey = "button-ok";
            this.btnQSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnQSave.Location = new System.Drawing.Point(453, 5);
            this.btnQSave.Name = "btnQSave";
            this.btnQSave.Size = new System.Drawing.Size(75, 25);
            this.btnQSave.TabIndex = 1;
            this.btnQSave.Text = "&Save";
            this.btnQSave.Visible = false;
            this.btnQSave.Click += new System.EventHandler(this.btnQSave_Click);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Right;
            this.spacer2.Location = new System.Drawing.Point(528, 5);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(3, 25);
            // 
            // btnQCancel
            // 
            this.btnQCancel.AppearanceKey = "button-error";
            this.btnQCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnQCancel.Location = new System.Drawing.Point(531, 5);
            this.btnQCancel.Name = "btnQCancel";
            this.btnQCancel.Size = new System.Drawing.Size(75, 25);
            this.btnQCancel.TabIndex = 2;
            this.btnQCancel.Text = "&Cancel";
            this.btnQCancel.Visible = false;
            this.btnQCancel.Click += new System.EventHandler(this.btnQCancel_Click);
            // 
            // picGroupAdd
            // 
            this.picGroupAdd.Cursor = Wisej.Web.Cursors.Hand;
            this.picGroupAdd.ImageSource = "captain-add";
            this.picGroupAdd.Location = new System.Drawing.Point(546, 13);
            this.picGroupAdd.Name = "picGroupAdd";
            this.picGroupAdd.Size = new System.Drawing.Size(20, 20);
            this.picGroupAdd.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.picGroupAdd.ToolTipText = "Add Group";
            this.picGroupAdd.Click += new System.EventHandler(this.picGroupAdd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(531, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(453, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save";
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(36, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(8, 11);
            this.label3.TabIndex = 1;
            this.label3.Text = "*";
            this.label3.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(231, 76);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(9, 14);
            this.label8.TabIndex = 1;
            this.label8.Text = "*";
            this.label8.Visible = false;
            // 
            // lblSeq
            // 
            this.lblSeq.Location = new System.Drawing.Point(15, 77);
            this.lblSeq.Name = "lblSeq";
            this.lblSeq.Size = new System.Drawing.Size(23, 16);
            this.lblSeq.TabIndex = 1;
            this.lblSeq.Text = "Seq";
            this.lblSeq.Visible = false;
            // 
            // txtSeq
            // 
            this.txtSeq.Location = new System.Drawing.Point(105, 73);
            this.txtSeq.MaxLength = 2;
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.Size = new System.Drawing.Size(58, 25);
            this.txtSeq.TabIndex = 2;
            this.txtSeq.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtSeq.Visible = false;
            this.txtSeq.LostFocus += new System.EventHandler(this.txtSeq_LostFocus);
            // 
            // txtGrpCode
            // 
            this.txtGrpCode.Location = new System.Drawing.Point(250, 73);
            this.txtGrpCode.MaxLength = 3;
            this.txtGrpCode.Name = "txtGrpCode";
            this.txtGrpCode.Size = new System.Drawing.Size(58, 25);
            this.txtGrpCode.TabIndex = 2;
            this.txtGrpCode.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtGrpCode.Visible = false;
            this.txtGrpCode.LostFocus += new System.EventHandler(this.txtGrpCode_LostFocus);
            // 
            // lblGrp_code
            // 
            this.lblGrp_code.Location = new System.Drawing.Point(202, 77);
            this.lblGrp_code.Name = "lblGrp_code";
            this.lblGrp_code.Size = new System.Drawing.Size(30, 16);
            this.lblGrp_code.TabIndex = 1;
            this.lblGrp_code.Text = "Code";
            this.lblGrp_code.Visible = false;
            // 
            // pnlFields
            // 
            this.pnlFields.Controls.Add(this.cmbConjunction);
            this.pnlFields.Controls.Add(this.lblConjunction);
            this.pnlFields.Controls.Add(this.txtGroupDesc);
            this.pnlFields.Controls.Add(this.lblGroupDesc);
            this.pnlFields.Controls.Add(this.label5);
            this.pnlFields.Controls.Add(this.picGroupAdd);
            this.pnlFields.Controls.Add(this.label3);
            this.pnlFields.Controls.Add(this.label8);
            this.pnlFields.Controls.Add(this.lblSeq);
            this.pnlFields.Controls.Add(this.txtSeq);
            this.pnlFields.Controls.Add(this.txtGrpCode);
            this.pnlFields.Controls.Add(this.lblGrp_code);
            this.pnlFields.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlFields.Location = new System.Drawing.Point(0, 300);
            this.pnlFields.Name = "pnlFields";
            this.pnlFields.Size = new System.Drawing.Size(621, 106);
            this.pnlFields.TabIndex = 2;
            // 
            // cmbConjunction
            // 
            this.cmbConjunction.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbConjunction.FormattingEnabled = true;
            this.cmbConjunction.Location = new System.Drawing.Point(105, 42);
            this.cmbConjunction.Name = "cmbConjunction";
            this.cmbConjunction.Size = new System.Drawing.Size(60, 25);
            this.cmbConjunction.TabIndex = 6;
            // 
            // lblConjunction
            // 
            this.lblConjunction.Location = new System.Drawing.Point(15, 46);
            this.lblConjunction.Name = "lblConjunction";
            this.lblConjunction.Size = new System.Drawing.Size(70, 16);
            this.lblConjunction.TabIndex = 3;
            this.lblConjunction.Text = "Conjunction";
            // 
            // txtGroupDesc
            // 
            this.txtGroupDesc.Location = new System.Drawing.Point(105, 11);
            this.txtGroupDesc.MaxLength = 100;
            this.txtGroupDesc.Name = "txtGroupDesc";
            this.txtGroupDesc.Size = new System.Drawing.Size(425, 25);
            this.txtGroupDesc.TabIndex = 4;
            // 
            // lblGroupDesc
            // 
            this.lblGroupDesc.Location = new System.Drawing.Point(15, 15);
            this.lblGroupDesc.Name = "lblGroupDesc";
            this.lblGroupDesc.Size = new System.Drawing.Size(68, 16);
            this.lblGroupDesc.TabIndex = 1;
            this.lblGroupDesc.Text = "Group Desc";
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(80, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 11);
            this.label5.TabIndex = 28;
            this.label5.Text = "*";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // gvMat_Groups
            // 
            this.gvMat_Groups.AllowUserToResizeColumns = false;
            this.gvMat_Groups.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvMat_Groups.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvMat_Groups.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvMat_Groups.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.GrpCd,
            this.Group,
            this.Grp_Conj,
            this.GSeq,
            this.GrpEdit,
            this.GrpDel});
            this.gvMat_Groups.Dock = Wisej.Web.DockStyle.Fill;
            this.gvMat_Groups.Location = new System.Drawing.Point(0, 0);
            this.gvMat_Groups.Name = "gvMat_Groups";
            this.gvMat_Groups.ReadOnly = true;
            this.gvMat_Groups.RowHeadersWidth = 14;
            this.gvMat_Groups.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvMat_Groups.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvMat_Groups.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvMat_Groups.Size = new System.Drawing.Size(621, 254);
            this.gvMat_Groups.TabIndex = 0;
            this.gvMat_Groups.SelectionChanged += new System.EventHandler(this.gvMat_Groups_SelectionChanged);
            this.gvMat_Groups.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvMat_Groups_CellClick);
            // 
            // GrpCd
            // 
            this.GrpCd.HeaderText = "GrpCd";
            this.GrpCd.Name = "GrpCd";
            this.GrpCd.ReadOnly = true;
            this.GrpCd.ShowInVisibilityMenu = false;
            this.GrpCd.Visible = false;
            this.GrpCd.Width = 20;
            // 
            // Group
            // 
            dataGridViewCellStyle12.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Group.DefaultCellStyle = dataGridViewCellStyle12;
            dataGridViewCellStyle13.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Group.HeaderStyle = dataGridViewCellStyle13;
            this.Group.HeaderText = "Group";
            this.Group.Name = "Group";
            this.Group.ReadOnly = true;
            this.Group.Width = 350;
            // 
            // Grp_Conj
            // 
            dataGridViewCellStyle14.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Grp_Conj.DefaultCellStyle = dataGridViewCellStyle14;
            dataGridViewCellStyle15.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Grp_Conj.HeaderStyle = dataGridViewCellStyle15;
            this.Grp_Conj.HeaderText = "Conjunction";
            this.Grp_Conj.Name = "Grp_Conj";
            this.Grp_Conj.ReadOnly = true;
            this.Grp_Conj.Width = 120;
            // 
            // GSeq
            // 
            this.GSeq.HeaderText = "GSeq";
            this.GSeq.Name = "GSeq";
            this.GSeq.ReadOnly = true;
            this.GSeq.ShowInVisibilityMenu = false;
            this.GSeq.Visible = false;
            this.GSeq.Width = 20;
            // 
            // GrpEdit
            // 
            this.GrpEdit.CellImageAlignment = Wisej.Web.DataGridViewContentAlignment.NotSet;
            this.GrpEdit.CellImageSource = "captain-edit";
            dataGridViewCellStyle16.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle16.NullValue = null;
            this.GrpEdit.DefaultCellStyle = dataGridViewCellStyle16;
            dataGridViewCellStyle17.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.GrpEdit.HeaderStyle = dataGridViewCellStyle17;
            this.GrpEdit.HeaderText = "Edit";
            this.GrpEdit.Name = "GrpEdit";
            this.GrpEdit.ReadOnly = true;
            this.GrpEdit.ShowInVisibilityMenu = false;
            this.GrpEdit.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.GrpEdit.Width = 55;
            // 
            // GrpDel
            // 
            this.GrpDel.CellImageAlignment = Wisej.Web.DataGridViewContentAlignment.NotSet;
            this.GrpDel.CellImageSource = "captain-delete";
            dataGridViewCellStyle18.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle18.NullValue = null;
            this.GrpDel.DefaultCellStyle = dataGridViewCellStyle18;
            dataGridViewCellStyle19.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.GrpDel.HeaderStyle = dataGridViewCellStyle19;
            this.GrpDel.HeaderText = "Delete";
            this.GrpDel.Name = "GrpDel";
            this.GrpDel.ReadOnly = true;
            this.GrpDel.ShowInVisibilityMenu = false;
            this.GrpDel.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.GrpDel.Width = 55;
            // 
            // Groups
            // 
            this.Groups.Controls.Add(this.pnlGroups);
            this.Groups.Location = new System.Drawing.Point(0, 27);
            this.Groups.Name = "Groups";
            this.Groups.Size = new System.Drawing.Size(621, 441);
            this.Groups.Text = "Groups";
            // 
            // pnlGroups
            // 
            this.pnlGroups.Controls.Add(this.pnlgvMat_Groups);
            this.pnlGroups.Controls.Add(this.pnlFields);
            this.pnlGroups.Controls.Add(this.pnlGSave);
            this.pnlGroups.Controls.Add(this.pnlType);
            this.pnlGroups.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlGroups.Location = new System.Drawing.Point(0, 0);
            this.pnlGroups.Name = "pnlGroups";
            this.pnlGroups.Size = new System.Drawing.Size(621, 441);
            this.pnlGroups.TabIndex = 10;
            // 
            // pnlgvMat_Groups
            // 
            this.pnlgvMat_Groups.Controls.Add(this.gvMat_Groups);
            this.pnlgvMat_Groups.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvMat_Groups.Location = new System.Drawing.Point(0, 46);
            this.pnlgvMat_Groups.Name = "pnlgvMat_Groups";
            this.pnlgvMat_Groups.Size = new System.Drawing.Size(621, 254);
            this.pnlgvMat_Groups.TabIndex = 1;
            // 
            // pnlGSave
            // 
            this.pnlGSave.AppearanceKey = "panel-grdo";
            this.pnlGSave.Controls.Add(this.btnSave);
            this.pnlGSave.Controls.Add(this.spacer1);
            this.pnlGSave.Controls.Add(this.btnCancel);
            this.pnlGSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlGSave.Location = new System.Drawing.Point(0, 406);
            this.pnlGSave.Name = "pnlGSave";
            this.pnlGSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlGSave.Size = new System.Drawing.Size(621, 35);
            this.pnlGSave.TabIndex = 6;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(528, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // pnlType
            // 
            this.pnlType.Controls.Add(this.lblGrpType);
            this.pnlType.Controls.Add(this.cmbGrpType);
            this.pnlType.Dock = Wisej.Web.DockStyle.Top;
            this.pnlType.Location = new System.Drawing.Point(0, 0);
            this.pnlType.Name = "pnlType";
            this.pnlType.Size = new System.Drawing.Size(621, 46);
            this.pnlType.TabIndex = 0;
            // 
            // lblGrpType
            // 
            this.lblGrpType.Location = new System.Drawing.Point(15, 15);
            this.lblGrpType.Name = "lblGrpType";
            this.lblGrpType.Size = new System.Drawing.Size(30, 16);
            this.lblGrpType.TabIndex = 0;
            this.lblGrpType.Text = "Type";
            // 
            // cmbGrpType
            // 
            this.cmbGrpType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbGrpType.FormattingEnabled = true;
            this.cmbGrpType.Location = new System.Drawing.Point(55, 11);
            this.cmbGrpType.Name = "cmbGrpType";
            this.cmbGrpType.Size = new System.Drawing.Size(532, 25);
            this.cmbGrpType.TabIndex = 9;
            this.cmbGrpType.SelectedIndexChanged += new System.EventHandler(this.cmbGrpType_SelectedIndexChanged);
            // 
            // Questions
            // 
            this.Questions.Controls.Add(this.pnlOutcomes);
            this.Questions.Location = new System.Drawing.Point(0, 27);
            this.Questions.Name = "Questions";
            this.Questions.Size = new System.Drawing.Size(621, 441);
            this.Questions.Text = "Questions";
            // 
            // tabControlGQ
            // 
            this.tabControlGQ.Controls.Add(this.Groups);
            this.tabControlGQ.Controls.Add(this.Questions);
            this.tabControlGQ.Dock = Wisej.Web.DockStyle.Fill;
            this.tabControlGQ.Location = new System.Drawing.Point(0, 0);
            this.tabControlGQ.Name = "tabControlGQ";
            this.tabControlGQ.PageInsets = new Wisej.Web.Padding(0, 27, 0, 0);
            this.tabControlGQ.SelectedIndex = 0;
            this.tabControlGQ.Size = new System.Drawing.Size(621, 468);
            this.tabControlGQ.TabIndex = 1;
            this.tabControlGQ.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.tabControlGQ);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(621, 468);
            this.pnlCompleteForm.TabIndex = 2;
            // 
            // SSBGQuestions_Form
            // 
            this.ClientSize = new System.Drawing.Size(621, 468);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SSBGQuestions_Form";
            this.Text = "SSBGQuestions_Form";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "tlHelp";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.pnlOutcomes.ResumeLayout(false);
            this.pnlgvwQuestions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwQuestions)).EndInit();
            this.pnlQTop.ResumeLayout(false);
            this.pnlQTop.PerformLayout();
            this.pnlQFields.ResumeLayout(false);
            this.pnlQuesAdd.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picAddQues)).EndInit();
            this.pnlQuestions.ResumeLayout(false);
            this.pnlQuestions.PerformLayout();
            this.pnlQSave.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picGroupAdd)).EndInit();
            this.pnlFields.ResumeLayout(false);
            this.pnlFields.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvMat_Groups)).EndInit();
            this.Groups.ResumeLayout(false);
            this.pnlGroups.ResumeLayout(false);
            this.pnlgvMat_Groups.ResumeLayout(false);
            this.pnlGSave.ResumeLayout(false);
            this.pnlType.ResumeLayout(false);
            this.pnlType.PerformLayout();
            this.Questions.ResumeLayout(false);
            this.tabControlGQ.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Label lblType;
        private ComboBox cmbType;
        private Panel pnlOutcomes;
        private DataGridView gvwQuestions;
        private DataGridViewTextBoxColumn QuestionDesc;
        private DataGridViewTextBoxColumn Responce;
        private DataGridViewTextBoxColumn Conjunction;
        private DataGridViewTextBoxColumn QTypeCode;
        private DataGridViewTextBoxColumn QuestionId;
        private DataGridViewTextBoxColumn QSeq;
        private DataGridViewImageColumn Edit;
        private DataGridViewImageColumn Delete;
        private Panel pnlQuestions;
        private Button btnQSave;
        private Button btnQCancel;
        private Label label9;
        private Label labelNResp;
        private Label label7;
        private Label label1;
        private Label label2;
        private Label lblQuestion;
        private ComboBox cmbQuestions;
        private Label lblMemAccess;
        private ComboBox cmbMemAccess;
        private ComboBox cmbResponce;
        private Label lblResponse;
        private Label lblEqualTo;
        private TextBoxWithValidation txtEqualTo;
        private TextBoxWithValidation txtGreaterthan;
        private Label lblGreaterthan;
        private TextBoxWithValidation txtLessthan;
        private ComboBox cmbQuestionConjuction;
        private Label label4;
        private Label lblLessthan;
        private PictureBox picAddQues;
        private PictureBox picGroupAdd;
        private Button btnCancel;
        private Button btnSave;
        private Label label3;
        private Label label8;
        private Label lblSeq;
        private TextBox txtSeq;
        private TextBox txtGrpCode;
        private Label lblGrp_code;
        private Panel pnlFields;
        private DataGridView gvMat_Groups;
        private TabPage Groups;
        private TabPage Questions;
        private TabControl tabControlGQ;
        private ComboBox cmbGrpType;
        private Label lblGrpType;
        private TextBox txtGroupDesc;
        private Label lblGroupDesc;
        private Label label5;
        private Label label6;
        private ComboBox cmbQues_Group;
        private ComboBox cmbConjunction;
        private Label lblConjunction;
        private DataGridViewTextBoxColumn GrpCd;
        private DataGridViewTextBoxColumn Group;
        private DataGridViewTextBoxColumn Grp_Conj;
        private DataGridViewImageColumn GrpEdit;
        private DataGridViewImageColumn GrpDel;
        private DataGridViewTextBoxColumn GSeq;
        private Label lblName;
        private TextBox txtName;
        private Panel pnlCompleteForm;
        private Panel pnlGroups;
        private Panel pnlgvMat_Groups;
        private Panel pnlType;
        private Panel pnlGSave;
        private Spacer spacer1;
        private Panel pnlQSave;
        private Spacer spacer2;
        private Panel pnlQFields;
        private Panel pnlQuesAdd;
        private Panel pnlgvwQuestions;
        private Panel pnlQTop;
    }
}