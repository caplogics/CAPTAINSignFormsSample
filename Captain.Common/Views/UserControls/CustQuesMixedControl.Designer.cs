using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.UserControls
{
    partial class CustQuesMixedControl
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

        #region Wisej Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            this.panel3 = new Wisej.Web.Panel();
            this.gvwCustomQuestions = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.contextMenu2 = new Wisej.Web.ContextMenu(this.components);
            this.panel1 = new Wisej.Web.Panel();
            this.cmbQuestionAccess = new Wisej.Web.ComboBox();
            this.label44 = new Wisej.Web.Label();
            this.spacer2 = new Wisej.Web.Spacer();
            this.cmbQuestionType = new Wisej.Web.ComboBox();
            this.label43 = new Wisej.Web.Label();
            this.spacer1 = new Wisej.Web.Spacer();
            this.picMax = new Wisej.Web.PictureBox();
            this.cmbSEQ = new Wisej.Web.ComboBox();
            this.label42 = new Wisej.Web.Label();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwCustomQuestions)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMax)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gvwCustomQuestions);
            this.panel3.Dock = Wisej.Web.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 43);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(963, 222);
            this.panel3.TabIndex = 16;
            this.panel3.TabStop = true;
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
            dataGridViewCellStyle2.CssStyle = "";
            this.gvwCustomQuestions.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvwCustomQuestions.DefaultRowHeight = 25;
            this.gvwCustomQuestions.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwCustomQuestions.EditMode = Wisej.Web.DataGridViewEditMode.EditOnEnter;
            this.gvwCustomQuestions.MultiSelect = false;
            this.gvwCustomQuestions.Name = "gvwCustomQuestions";
            this.gvwCustomQuestions.RowHeadersVisible = false;
            this.gvwCustomQuestions.RowHeadersWidth = 10;
            this.gvwCustomQuestions.RowTemplate.Height = 30;
            this.gvwCustomQuestions.RowTemplate.Resizable = Wisej.Web.DataGridViewTriState.True;
            this.gvwCustomQuestions.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwCustomQuestions.ShowColumnVisibilityMenu = false;
            this.gvwCustomQuestions.Size = new System.Drawing.Size(963, 222);
            this.gvwCustomQuestions.TabIndex = 5;
            this.gvwCustomQuestions.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwCustomQuestions_CellClick);
            this.gvwCustomQuestions.DataError += new Wisej.Web.DataGridViewDataErrorEventHandler(this.gvwCustomQuestions_DataError_1);
            // 
            // contextMenu2
            // 
            this.contextMenu2.Name = "contextMenu2";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(14, 244, 244, 244);
            this.panel1.Controls.Add(this.cmbQuestionAccess);
            this.panel1.Controls.Add(this.label44);
            this.panel1.Controls.Add(this.spacer2);
            this.panel1.Controls.Add(this.cmbQuestionType);
            this.panel1.Controls.Add(this.label43);
            this.panel1.Controls.Add(this.spacer1);
            this.panel1.Controls.Add(this.picMax);
            this.panel1.Controls.Add(this.cmbSEQ);
            this.panel1.Controls.Add(this.label42);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(10);
            this.panel1.Size = new System.Drawing.Size(963, 43);
            this.panel1.TabIndex = 15;
            this.panel1.TabStop = true;
            // 
            // cmbQuestionAccess
            // 
            this.cmbQuestionAccess.Dock = Wisej.Web.DockStyle.Left;
            this.cmbQuestionAccess.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbQuestionAccess.FormattingEnabled = true;
            this.cmbQuestionAccess.Location = new System.Drawing.Point(650, 10);
            this.cmbQuestionAccess.Name = "cmbQuestionAccess";
            this.cmbQuestionAccess.Size = new System.Drawing.Size(221, 23);
            this.cmbQuestionAccess.TabIndex = 11;
            this.cmbQuestionAccess.SelectedIndexChanged += new System.EventHandler(this.cmbQuestionAccess_SelectedIndexChanged);
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Dock = Wisej.Web.DockStyle.Left;
            this.label44.Location = new System.Drawing.Point(552, 10);
            this.label44.MinimumSize = new System.Drawing.Size(0, 18);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(98, 23);
            this.label44.TabIndex = 10;
            this.label44.Text = "Question Access:";
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(547, 10);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(5, 23);
            // 
            // cmbQuestionType
            // 
            this.cmbQuestionType.Dock = Wisej.Web.DockStyle.Left;
            this.cmbQuestionType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbQuestionType.FormattingEnabled = true;
            this.cmbQuestionType.Location = new System.Drawing.Point(371, 10);
            this.cmbQuestionType.Name = "cmbQuestionType";
            this.cmbQuestionType.Size = new System.Drawing.Size(176, 23);
            this.cmbQuestionType.TabIndex = 9;
            this.cmbQuestionType.Visible = false;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Dock = Wisej.Web.DockStyle.Left;
            this.label43.Location = new System.Drawing.Point(288, 10);
            this.label43.MinimumSize = new System.Drawing.Size(0, 18);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(83, 23);
            this.label43.TabIndex = 8;
            this.label43.Text = "QuestionType:";
            this.label43.Visible = false;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(283, 10);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 23);
            this.spacer1.Visible = false;
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
            // cmbSEQ
            // 
            this.cmbSEQ.Dock = Wisej.Web.DockStyle.Left;
            this.cmbSEQ.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbSEQ.FormattingEnabled = true;
            this.cmbSEQ.Location = new System.Drawing.Point(130, 10);
            this.cmbSEQ.Name = "cmbSEQ";
            this.cmbSEQ.Size = new System.Drawing.Size(153, 23);
            this.cmbSEQ.TabIndex = 7;
            this.cmbSEQ.Visible = false;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Dock = Wisej.Web.DockStyle.Left;
            this.label42.Location = new System.Drawing.Point(10, 10);
            this.label42.MinimumSize = new System.Drawing.Size(120, 18);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(120, 23);
            this.label42.TabIndex = 6;
            this.label42.Text = "Question Sequence:";
            this.label42.Visible = false;
            // 
            // CustQuesMixedControl
            // 
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "CustQuesMixedControl";
            this.Size = new System.Drawing.Size(963, 265);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwCustomQuestions)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMax)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Panel panel3;
        private Captain.Common.Views.Controls.Compatibility.DataGridViewEx gvwCustomQuestions;
        private Wisej.Web.ContextMenu contextMenu2;
        private Wisej.Web.Panel panel1;
        private Wisej.Web.PictureBox picMax;
        private Wisej.Web.ComboBox cmbQuestionAccess;
        private Wisej.Web.Label label44;
        private Wisej.Web.Label label42;
        private Wisej.Web.ComboBox cmbQuestionType;
        private Wisej.Web.ComboBox cmbSEQ;
        private Wisej.Web.Label label43;
        private Wisej.Web.Spacer spacer2;
        private Wisej.Web.Spacer spacer1;
    }
}
