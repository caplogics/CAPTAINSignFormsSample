using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class PipCustomquestionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PipCustomquestionsForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.LblHeader = new Wisej.Web.Label();
            this.pnlHeader = new Wisej.Web.Panel();
            this.pnlQuestions = new Wisej.Web.Panel();
            this.gvwSubdetails = new Wisej.Web.DataGridView();
            this.gvtQuestion = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtSnpAnswers = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtLeanAnswers = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlServices = new Wisej.Web.Panel();
            this.panel2 = new Wisej.Web.Panel();
            this.gvwLeanServices = new Wisej.Web.DataGridView();
            this.gvtLeanServices = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtLeanCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel1 = new Wisej.Web.Panel();
            this.gvwIntakeServices = new Wisej.Web.DataGridView();
            this.gvtIntakeServices = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtIntakeCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlHeader.SuspendLayout();
            this.pnlQuestions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSubdetails)).BeginInit();
            this.pnlServices.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwLeanServices)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwIntakeServices)).BeginInit();
            this.SuspendLayout();
            // 
            // LblHeader
            // 
            this.LblHeader.AutoSize = true;
            this.LblHeader.Dock = Wisej.Web.DockStyle.Left;
            this.LblHeader.Font = new System.Drawing.Font("default", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.LblHeader.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            this.LblHeader.Location = new System.Drawing.Point(0, 0);
            this.LblHeader.MaximumSize = new System.Drawing.Size(0, 30);
            this.LblHeader.MinimumSize = new System.Drawing.Size(0, 30);
            this.LblHeader.Name = "LblHeader";
            this.LblHeader.Padding = new Wisej.Web.Padding(15, 0, 0, 0);
            this.LblHeader.Size = new System.Drawing.Size(187, 30);
            this.LblHeader.TabIndex = 1;
            this.LblHeader.Text = "Custom Questions Details";
            this.LblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(235, 244, 248);
            this.pnlHeader.Controls.Add(this.LblHeader);
            this.pnlHeader.Dock = Wisej.Web.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(929, 34);
            this.pnlHeader.TabIndex = 0;
            // 
            // pnlQuestions
            // 
            this.pnlQuestions.Controls.Add(this.gvwSubdetails);
            this.pnlQuestions.Dock = Wisej.Web.DockStyle.Top;
            this.pnlQuestions.Location = new System.Drawing.Point(0, 34);
            this.pnlQuestions.Name = "pnlQuestions";
            this.pnlQuestions.Padding = new Wisej.Web.Padding(5);
            this.pnlQuestions.Size = new System.Drawing.Size(929, 266);
            this.pnlQuestions.TabIndex = 1;
            // 
            // gvwSubdetails
            // 
            this.gvwSubdetails.AllowUserToResizeColumns = false;
            this.gvwSubdetails.AllowUserToResizeRows = false;
            this.gvwSubdetails.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-IN");
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwSubdetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwSubdetails.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwSubdetails.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtQuestion,
            this.gvtSnpAnswers,
            this.gvtLeanAnswers});
            this.gvwSubdetails.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.gvwSubdetails.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwSubdetails.Location = new System.Drawing.Point(5, 5);
            this.gvwSubdetails.MultiSelect = false;
            this.gvwSubdetails.Name = "gvwSubdetails";
            this.gvwSubdetails.ReadOnly = true;
            this.gvwSubdetails.RowHeadersWidth = 15;
            this.gvwSubdetails.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwSubdetails.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-IN");
            this.gvwSubdetails.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwSubdetails.Size = new System.Drawing.Size(919, 256);
            this.gvwSubdetails.TabIndex = 0;
            // 
            // gvtQuestion
            // 
            this.gvtQuestion.HeaderText = "Question";
            this.gvtQuestion.Name = "gvtQuestion";
            this.gvtQuestion.ReadOnly = true;
            this.gvtQuestion.Width = 400;
            // 
            // gvtSnpAnswers
            // 
            this.gvtSnpAnswers.HeaderText = "Intake Answers";
            this.gvtSnpAnswers.Name = "gvtSnpAnswers";
            this.gvtSnpAnswers.ReadOnly = true;
            this.gvtSnpAnswers.Width = 240;
            // 
            // gvtLeanAnswers
            // 
            this.gvtLeanAnswers.HeaderText = "PIP Answers";
            this.gvtLeanAnswers.Name = "gvtLeanAnswers";
            this.gvtLeanAnswers.ReadOnly = true;
            this.gvtLeanAnswers.Width = 240;
            // 
            // pnlServices
            // 
            this.pnlServices.Controls.Add(this.panel2);
            this.pnlServices.Controls.Add(this.panel1);
            this.pnlServices.Dock = Wisej.Web.DockStyle.Top;
            this.pnlServices.Location = new System.Drawing.Point(0, 300);
            this.pnlServices.Name = "pnlServices";
            this.pnlServices.Size = new System.Drawing.Size(929, 266);
            this.pnlServices.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gvwLeanServices);
            this.panel2.Dock = Wisej.Web.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(459, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(5);
            this.panel2.Size = new System.Drawing.Size(466, 266);
            this.panel2.TabIndex = 2;
            // 
            // gvwLeanServices
            // 
            this.gvwLeanServices.AllowUserToResizeColumns = false;
            this.gvwLeanServices.AllowUserToResizeRows = false;
            this.gvwLeanServices.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.FormatProvider = new System.Globalization.CultureInfo("en-IN");
            dataGridViewCellStyle2.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwLeanServices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gvwLeanServices.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwLeanServices.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtLeanServices,
            this.gvtLeanCode});
            this.gvwLeanServices.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.gvwLeanServices.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwLeanServices.Location = new System.Drawing.Point(5, 5);
            this.gvwLeanServices.MultiSelect = false;
            this.gvwLeanServices.Name = "gvwLeanServices";
            this.gvwLeanServices.ReadOnly = true;
            this.gvwLeanServices.RowHeadersWidth = 15;
            this.gvwLeanServices.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwLeanServices.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-IN");
            this.gvwLeanServices.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwLeanServices.Size = new System.Drawing.Size(456, 256);
            this.gvwLeanServices.TabIndex = 0;
            // 
            // gvtLeanServices
            // 
            this.gvtLeanServices.HeaderText = "Services Inquired on PIP";
            this.gvtLeanServices.Name = "gvtLeanServices";
            this.gvtLeanServices.ReadOnly = true;
            this.gvtLeanServices.Width = 400;
            // 
            // gvtLeanCode
            // 
            this.gvtLeanCode.HeaderText = "gvtLeanCode";
            this.gvtLeanCode.Name = "gvtLeanCode";
            this.gvtLeanCode.ReadOnly = true;
            this.gvtLeanCode.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gvwIntakeServices);
            this.panel1.Dock = Wisej.Web.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(5);
            this.panel1.Size = new System.Drawing.Size(459, 266);
            this.panel1.TabIndex = 1;
            // 
            // gvwIntakeServices
            // 
            this.gvwIntakeServices.AllowUserToResizeColumns = false;
            this.gvwIntakeServices.AllowUserToResizeRows = false;
            this.gvwIntakeServices.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.FormatProvider = new System.Globalization.CultureInfo("en-IN");
            dataGridViewCellStyle3.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle3.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwIntakeServices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gvwIntakeServices.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwIntakeServices.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtIntakeServices,
            this.gvtIntakeCode});
            this.gvwIntakeServices.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.gvwIntakeServices.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwIntakeServices.Location = new System.Drawing.Point(5, 5);
            this.gvwIntakeServices.MultiSelect = false;
            this.gvwIntakeServices.Name = "gvwIntakeServices";
            this.gvwIntakeServices.ReadOnly = true;
            this.gvwIntakeServices.RowHeadersWidth = 15;
            this.gvwIntakeServices.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwIntakeServices.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-IN");
            this.gvwIntakeServices.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwIntakeServices.Size = new System.Drawing.Size(449, 256);
            this.gvwIntakeServices.TabIndex = 0;
            this.gvwIntakeServices.Click += new System.EventHandler(this.gvwIntakeServices_Click);
            // 
            // gvtIntakeServices
            // 
            this.gvtIntakeServices.HeaderText = "Services Inquired in CAPTAIN Data";
            this.gvtIntakeServices.Name = "gvtIntakeServices";
            this.gvtIntakeServices.ReadOnly = true;
            this.gvtIntakeServices.Width = 400;
            // 
            // gvtIntakeCode
            // 
            this.gvtIntakeCode.HeaderText = "gvtIntakeCode";
            this.gvtIntakeCode.Name = "gvtIntakeCode";
            this.gvtIntakeCode.ReadOnly = true;
            this.gvtIntakeCode.Visible = false;
            // 
            // PipCustomquestionsForm
            // 
            this.ClientSize = new System.Drawing.Size(929, 567);
            this.Controls.Add(this.pnlServices);
            this.Controls.Add(this.pnlQuestions);
            this.Controls.Add(this.pnlHeader);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PipCustomquestionsForm";
            this.Text = "Update Intake from PIP";
            componentTool1.ImageSource = "icon-help";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlQuestions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwSubdetails)).EndInit();
            this.pnlServices.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwLeanServices)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwIntakeServices)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion
        private Label LblHeader;
        private Panel pnlHeader;
        private Panel pnlQuestions;
        private DataGridView gvwSubdetails;
        private DataGridViewTextBoxColumn gvtQuestion;
        private DataGridViewTextBoxColumn gvtSnpAnswers;
        private DataGridViewTextBoxColumn gvtLeanAnswers;
        private Panel pnlServices;
        private DataGridView gvwIntakeServices;
        private DataGridViewTextBoxColumn gvtIntakeServices;
        private DataGridViewTextBoxColumn gvtIntakeCode;
        private DataGridView gvwLeanServices;
        private DataGridViewTextBoxColumn gvtLeanServices;
        private DataGridViewTextBoxColumn gvtLeanCode;
        private Panel panel2;
        private Panel panel1;
    }
}