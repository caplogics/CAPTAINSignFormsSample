using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class AssignUserReportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssignUserReportForm));
            this.panel1 = new Wisej.Web.Panel();
            this.cmbQuestionType = new Wisej.Web.ComboBox();
            this.lblPQuestionType = new Wisej.Web.Label();
            this.lblHie = new Wisej.Web.Label();
            this.cmbHie = new Wisej.Web.ComboBox();
            this.BtnGenPdf = new Wisej.Web.Button();
            this.panel2 = new Wisej.Web.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmbQuestionType);
            this.panel1.Controls.Add(this.lblPQuestionType);
            this.panel1.Controls.Add(this.lblHie);
            this.panel1.Controls.Add(this.cmbHie);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(502, 118);
            this.panel1.TabIndex = 1;
            // 
            // cmbQuestionType
            // 
            this.cmbQuestionType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbQuestionType.FormattingEnabled = true;
            this.cmbQuestionType.Location = new System.Drawing.Point(144, 62);
            this.cmbQuestionType.Name = "cmbQuestionType";
            this.cmbQuestionType.Size = new System.Drawing.Size(202, 25);
            this.cmbQuestionType.TabIndex = 9;
            // 
            // lblPQuestionType
            // 
            this.lblPQuestionType.AutoSize = true;
            this.lblPQuestionType.Location = new System.Drawing.Point(83, 63);
            this.lblPQuestionType.MinimumSize = new System.Drawing.Size(0, 21);
            this.lblPQuestionType.Name = "lblPQuestionType";
            this.lblPQuestionType.Size = new System.Drawing.Size(39, 21);
            this.lblPQuestionType.TabIndex = 8;
            this.lblPQuestionType.Text = "Status";
            // 
            // lblHie
            // 
            this.lblHie.AutoSize = true;
            this.lblHie.Location = new System.Drawing.Point(77, 24);
            this.lblHie.MinimumSize = new System.Drawing.Size(0, 21);
            this.lblHie.Name = "lblHie";
            this.lblHie.Size = new System.Drawing.Size(45, 21);
            this.lblHie.TabIndex = 1;
            this.lblHie.Text = "Agency";
            // 
            // cmbHie
            // 
            this.cmbHie.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbHie.FormattingEnabled = true;
            this.cmbHie.Location = new System.Drawing.Point(144, 24);
            this.cmbHie.Name = "cmbHie";
            this.cmbHie.Size = new System.Drawing.Size(202, 25);
            this.cmbHie.TabIndex = 3;
            // 
            // BtnGenPdf
            // 
            this.BtnGenPdf.Dock = Wisej.Web.DockStyle.Right;
            this.BtnGenPdf.Location = new System.Drawing.Point(388, 5);
            this.BtnGenPdf.Name = "BtnGenPdf";
            this.BtnGenPdf.Size = new System.Drawing.Size(99, 25);
            this.BtnGenPdf.TabIndex = 1;
            this.BtnGenPdf.Text = "G&enerate Excel";
            this.BtnGenPdf.Click += new System.EventHandler(this.BtnGenFile_Click);
            // 
            // panel2
            // 
            this.panel2.AppearanceKey = "panel-grdo";
            this.panel2.Controls.Add(this.BtnGenPdf);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 118);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.panel2.Size = new System.Drawing.Size(502, 35);
            this.panel2.TabIndex = 2;
            // 
            // AssignUserReportForm
            // 
            this.ClientSize = new System.Drawing.Size(502, 152);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AssignUserReportForm";
            this.Text = "User Account & Privileges Report";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion
        private Panel panel1;
        private Button BtnGenPdf;
        private Panel panel2;
        private Label lblHie;
        private ComboBox cmbHie;
        private ComboBox cmbQuestionType;
        private Label lblPQuestionType;
    }
}