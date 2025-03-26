using Wisej.Web;

namespace Captain.Common.Views.UserControls
{
    partial class CustomFieldsControl
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

        #region Visual WebGui UserControl Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            this.cmbQuestionAccess = new Wisej.Web.ComboBox();
            this.label44 = new Wisej.Web.Label();
            this.cmbQuestionType = new Wisej.Web.ComboBox();
            this.label43 = new Wisej.Web.Label();
            this.cmbSEQ = new Wisej.Web.ComboBox();
            this.label42 = new Wisej.Web.Label();
            this.gvwCustomQuestions = new Wisej.Web.DataGridView();
            this.ImgSave = new Wisej.Web.DataGridViewImageColumn();
            this.Question = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Response = new Wisej.Web.DataGridViewTextBoxColumn();
            this.ResponseCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.contextMenu2 = new Wisej.Web.ContextMenu();
            this.panel1 = new Wisej.Web.Panel();
            this.picMax = new Wisej.Web.PictureBox();
            this.panel2 = new Wisej.Web.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gvwCustomQuestions)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMax)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbQuestionAccess
            // 
            this.cmbQuestionAccess.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbQuestionAccess.FormattingEnabled = true;
            this.cmbQuestionAccess.Location = new System.Drawing.Point(518, 5);
            this.cmbQuestionAccess.Name = "cmbQuestionAccess";
            this.cmbQuestionAccess.Size = new System.Drawing.Size(147, 25);
            this.cmbQuestionAccess.TabIndex = 11;
            this.cmbQuestionAccess.SelectedIndexChanged += new System.EventHandler(this.OnAccessSelectedIndexChanged);
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(426, 8);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(98, 14);
            this.label44.TabIndex = 10;
            this.label44.Text = "Question Access:";
            // 
            // cmbQuestionType
            // 
            this.cmbQuestionType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbQuestionType.FormattingEnabled = true;
            this.cmbQuestionType.Location = new System.Drawing.Point(313, 4);
            this.cmbQuestionType.Name = "cmbQuestionType";
            this.cmbQuestionType.Size = new System.Drawing.Size(113, 25);
            this.cmbQuestionType.TabIndex = 9;
            this.cmbQuestionType.SelectedIndexChanged += new System.EventHandler(this.OnTypeSelectedIndexChanged);
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(235, 8);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(83, 14);
            this.label43.TabIndex = 8;
            this.label43.Text = "QuestionType:";
            // 
            // cmbSEQ
            // 
            this.cmbSEQ.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbSEQ.FormattingEnabled = true;
            this.cmbSEQ.Location = new System.Drawing.Point(108, 4);
            this.cmbSEQ.Name = "cmbSEQ";
            this.cmbSEQ.Size = new System.Drawing.Size(115, 25);
            this.cmbSEQ.TabIndex = 7;
            this.cmbSEQ.SelectedIndexChanged += new System.EventHandler(this.OnSequenceSelectedIndexChanged);
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(3, 7);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(113, 14);
            this.label42.TabIndex = 6;
            this.label42.Text = "Question Sequence:";
            // 
            // gvwCustomQuestions
            // 
            this.gvwCustomQuestions.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwCustomQuestions.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwCustomQuestions.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.ImgSave,
            this.Question,
            this.Response,
            this.ResponseCode});
            this.gvwCustomQuestions.ContextMenu = this.contextMenu2;
            this.gvwCustomQuestions.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwCustomQuestions.Location = new System.Drawing.Point(0, 0);
            this.gvwCustomQuestions.MultiSelect = false;
            this.gvwCustomQuestions.Name = "gvwCustomQuestions";
            this.gvwCustomQuestions.RowHeadersWidth = 5;
            this.gvwCustomQuestions.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-IN");
            this.gvwCustomQuestions.RowTemplate.Resizable = Wisej.Web.DataGridViewTriState.True;
            this.gvwCustomQuestions.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwCustomQuestions.Size = new System.Drawing.Size(700, 185);
            this.gvwCustomQuestions.TabIndex = 5;
            this.gvwCustomQuestions.DataError += new Wisej.Web.DataGridViewDataErrorEventHandler(this.gvwCustomQuestions_DataError);
            // 
            // ImgSave
            // 
            this.ImgSave.CellImageAlignment = Wisej.Web.DataGridViewContentAlignment.NotSet;
            this.ImgSave.HeaderText = " ";
            this.ImgSave.Name = "ImgSave";
            this.ImgSave.Width = 40;
            // 
            // Question
            // 
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-IN");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Question.DefaultCellStyle = dataGridViewCellStyle1;
            this.Question.HeaderText = "Question Description";
            this.Question.Name = "Question";
            this.Question.Width = 300;
            // 
            // Response
            // 
            this.Response.Name = "Response";
            this.Response.Width = 355;
            // 
            // ResponseCode
            // 
            this.ResponseCode.Name = "ResponseCode";
            this.ResponseCode.Visible = false;
            this.ResponseCode.Width = 10;
            // 
            // contextMenu2
            // 
            this.contextMenu2.Name = "contextMenu2";
            this.contextMenu2.RightToLeft = Wisej.Web.RightToLeft.No;
            // 
            // panel1
            // 
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
            this.panel1.Size = new System.Drawing.Size(700, 29);
            this.panel1.TabIndex = 12;
            // 
            // picMax
            // 
            this.picMax.Cursor = Wisej.Web.Cursors.Hand;
            this.picMax.ImageSource = "16X16.DropDownArrowUp.gif";
            this.picMax.Location = new System.Drawing.Point(681, 3);
            this.picMax.Name = "picMax";
            this.picMax.Size = new System.Drawing.Size(19, 18);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gvwCustomQuestions);
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(700, 185);
            this.panel2.TabIndex = 13;
            // 
            // CustomFieldsControl
            // 
            this.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "CustomFieldsControl";
            this.Size = new System.Drawing.Size(702, 187);
            ((System.ComponentModel.ISupportInitialize)(this.gvwCustomQuestions)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMax)).EndInit();
            this.panel2.ResumeLayout(false);
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
        private Panel panel2;
        private PictureBox picMax;
        private ContextMenu contextMenu2;
        private DataGridViewTextBoxColumn ResponseCode;



    }
}