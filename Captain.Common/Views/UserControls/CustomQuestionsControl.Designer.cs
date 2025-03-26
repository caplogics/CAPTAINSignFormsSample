using Wisej.Web;


namespace Captain.Common.Views.UserControls
{
    partial class CustomQuestionsControl
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
            this.cmbQuestionAccess = new Wisej.Web.ComboBox();
            this.label44 = new Wisej.Web.Label();
            this.cmbQuestionType = new Wisej.Web.ComboBox();
            this.label43 = new Wisej.Web.Label();
            this.cmbSEQ = new Wisej.Web.ComboBox();
            this.label42 = new Wisej.Web.Label();
            this.gvwCustomQuestions = new Wisej.Web.DataGridView();
            this.gvtRequire = new Wisej.Web.DataGridViewTextBoxColumn();
            this.ImgSave = new Wisej.Web.DataGridViewImageColumn();
            this.Question = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Response = new Wisej.Web.DataGridViewTextBoxColumn();
            this.contextMenu2 = new Wisej.Web.ContextMenu();
            this.ResponseCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.FamilySeq = new Wisej.Web.DataGridViewTextBoxColumn();
            this.ResponceSeq = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.ResponceDelete = new Wisej.Web.DataGridViewImageColumn();
            this.gvtResponseQType = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel1 = new Wisej.Web.Panel();
            this.picMax = new Wisej.Web.PictureBox();
            this.panel3 = new Wisej.Web.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gvwCustomQuestions)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMax)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbQuestionAccess
            // 
            this.cmbQuestionAccess.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbQuestionAccess.FormattingEnabled = true;
            this.cmbQuestionAccess.Location = new System.Drawing.Point(678, 7);
            this.cmbQuestionAccess.Name = "cmbQuestionAccess";
            this.cmbQuestionAccess.Size = new System.Drawing.Size(221, 25);
            this.cmbQuestionAccess.TabIndex = 11;
            this.cmbQuestionAccess.SelectedIndexChanged += new System.EventHandler(this.cmbQuestionAccess_SelectedIndexChanged);
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(575, 10);
            this.label44.MinimumSize = new System.Drawing.Size(0, 18);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(98, 18);
            this.label44.TabIndex = 10;
            this.label44.Text = "Question Access:";
            // 
            // cmbQuestionType
            // 
            this.cmbQuestionType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbQuestionType.FormattingEnabled = true;
            this.cmbQuestionType.Location = new System.Drawing.Point(384, 8);
            this.cmbQuestionType.Name = "cmbQuestionType";
            this.cmbQuestionType.Size = new System.Drawing.Size(176, 25);
            this.cmbQuestionType.TabIndex = 9;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(297, 10);
            this.label43.MinimumSize = new System.Drawing.Size(0, 18);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(83, 18);
            this.label43.TabIndex = 8;
            this.label43.Text = "QuestionType:";
            // 
            // cmbSEQ
            // 
            this.cmbSEQ.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbSEQ.FormattingEnabled = true;
            this.cmbSEQ.Location = new System.Drawing.Point(124, 7);
            this.cmbSEQ.Name = "cmbSEQ";
            this.cmbSEQ.Size = new System.Drawing.Size(153, 25);
            this.cmbSEQ.TabIndex = 7;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(3, 10);
            this.label42.MinimumSize = new System.Drawing.Size(120, 18);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(120, 18);
            this.label42.TabIndex = 6;
            this.label42.Text = "Question Sequence:";
            // 
            // gvwCustomQuestions
            // 
            this.gvwCustomQuestions.AllowUserToResizeColumns = false;
            this.gvwCustomQuestions.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwCustomQuestions.BackColor = System.Drawing.Color.White;
            this.gvwCustomQuestions.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwCustomQuestions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwCustomQuestions.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwCustomQuestions.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtRequire,
            this.ImgSave,
            this.Question,
            this.Response,
            this.ResponseCode,
            this.FamilySeq,
            this.ResponceSeq,
            this.Code,
            this.ResponceDelete,
            this.gvtResponseQType});
            this.gvwCustomQuestions.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwCustomQuestions.Location = new System.Drawing.Point(0, 0);
            this.gvwCustomQuestions.MultiSelect = false;
            this.gvwCustomQuestions.Name = "gvwCustomQuestions";
            this.gvwCustomQuestions.RowHeadersVisible = false;
            this.gvwCustomQuestions.RowHeadersWidth = 5;
            this.gvwCustomQuestions.RowTemplate.Height = 30;
            this.gvwCustomQuestions.RowTemplate.Resizable = Wisej.Web.DataGridViewTriState.True;
            this.gvwCustomQuestions.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwCustomQuestions.ShowColumnVisibilityMenu = false;
            this.gvwCustomQuestions.Size = new System.Drawing.Size(963, 226);
            this.gvwCustomQuestions.TabIndex = 5;
            this.gvwCustomQuestions.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwCustomQuestions_CellClick);
            this.gvwCustomQuestions.DataError += new Wisej.Web.DataGridViewDataErrorEventHandler(this.gvwCustomQuestions_DataError);
            // 
            // gvtRequire
            // 
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Red;
            this.gvtRequire.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvtRequire.HeaderText = "  ";
            this.gvtRequire.MinimumWidth = 15;
            this.gvtRequire.Name = "gvtRequire";
            this.gvtRequire.ReadOnly = true;
            this.gvtRequire.Width = 15;
            // 
            // ImgSave
            // 
            this.ImgSave.CellImageAlignment = Wisej.Web.DataGridViewContentAlignment.NotSet;
            this.ImgSave.CellImageSource = "icon-save";
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = null;
            this.ImgSave.DefaultCellStyle = dataGridViewCellStyle3;
            this.ImgSave.HeaderText = " ";
            this.ImgSave.Name = "ImgSave";
            this.ImgSave.Width = 40;
            // 
            // Question
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Question.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Question.HeaderStyle = dataGridViewCellStyle5;
            this.Question.HeaderText = "Question Description";
            this.Question.Name = "Question";
            this.Question.Width = 380;
            // 
            // Response
            // 
            this.Response.ContextMenu = this.contextMenu2;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Response.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Response.HeaderStyle = dataGridViewCellStyle7;
            this.Response.HeaderText = "Response";
            this.Response.Name = "Response";
            this.Response.Width = 300;
            // 
            // contextMenu2
            // 
            this.contextMenu2.Name = "contextMenu2";
            this.contextMenu2.MenuItemClicked += new Wisej.Web.MenuItemEventHandler(this.gvwCustomQuestions_MenuClick);
            // 
            // ResponseCode
            // 
            this.ResponseCode.Name = "ResponseCode";
            this.ResponseCode.Visible = false;
            this.ResponseCode.Width = 10;
            // 
            // FamilySeq
            // 
            this.FamilySeq.Name = "FamilySeq";
            this.FamilySeq.Visible = false;
            this.FamilySeq.Width = 10;
            // 
            // ResponceSeq
            // 
            this.ResponceSeq.Name = "ResponceSeq";
            this.ResponceSeq.Visible = false;
            this.ResponceSeq.Width = 10;
            // 
            // Code
            // 
            this.Code.Name = "Code";
            this.Code.Visible = false;
            this.Code.Width = 10;
            // 
            // ResponceDelete
            // 
            this.ResponceDelete.CellImageAlignment = Wisej.Web.DataGridViewContentAlignment.NotSet;
            this.ResponceDelete.CellImageSource = "captain-delete";
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.NullValue = null;
            this.ResponceDelete.DefaultCellStyle = dataGridViewCellStyle8;
            this.ResponceDelete.HeaderText = "Delete";
            this.ResponceDelete.Name = "ResponceDelete";
            this.ResponceDelete.Width = 40;
            // 
            // gvtResponseQType
            // 
            this.gvtResponseQType.Name = "gvtResponseQType";
            this.gvtResponseQType.Visible = false;
            this.gvtResponseQType.Width = 10;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(14, 244, 244, 244);
            this.panel1.Controls.Add(this.picMax);
            this.panel1.Controls.Add(this.cmbQuestionAccess);
            this.panel1.Controls.Add(this.label44);
            this.panel1.Controls.Add(this.label42);
            this.panel1.Controls.Add(this.cmbQuestionType);
            this.panel1.Controls.Add(this.cmbSEQ);
            this.panel1.Controls.Add(this.label43);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(963, 39);
            this.panel1.TabIndex = 12;
            this.panel1.TabStop = true;
            // 
            // picMax
            // 
            this.picMax.Cursor = Wisej.Web.Cursors.Hand;
            this.picMax.ImageSource = "minimize?color=captainBlue";
            this.picMax.Location = new System.Drawing.Point(927, 7);
            this.picMax.Name = "picMax";
            this.picMax.Size = new System.Drawing.Size(24, 24);
            this.picMax.Visible = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gvwCustomQuestions);
            this.panel3.Dock = Wisej.Web.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 39);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(963, 226);
            this.panel3.TabIndex = 14;
            this.panel3.TabStop = true;
            // 
            // CustomQuestionsControl
            // 
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "CustomQuestionsControl";
            this.Size = new System.Drawing.Size(963, 265);
            ((System.ComponentModel.ISupportInitialize)(this.gvwCustomQuestions)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMax)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComboBox cmbQuestionAccess;
        private Label label44;
        private ComboBox cmbQuestionType;
        private Label label43;
        private ComboBox cmbSEQ;
        private Label label42;
        private DataGridView gvwCustomQuestions;
        private DataGridViewImageColumn ImgSave;
        private DataGridViewTextBoxColumn Question;
        private DataGridViewTextBoxColumn Response;
        private Panel panel1;
        private PictureBox picMax;
        private ContextMenu contextMenu2;
        private DataGridViewTextBoxColumn ResponseCode;
        private Panel panel3;
        private DataGridViewImageColumn ResponceDelete;
        private DataGridViewTextBoxColumn FamilySeq;
        private DataGridViewTextBoxColumn ResponceSeq;
        private DataGridViewTextBoxColumn Code;
        private DataGridViewTextBoxColumn gvtRequire;
        private DataGridViewTextBoxColumn gvtResponseQType;



    }
}