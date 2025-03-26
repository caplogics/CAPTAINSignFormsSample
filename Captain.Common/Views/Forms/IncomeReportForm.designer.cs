using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class IncomeReportForm
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
            this.BtnGenPdf = new Wisej.Web.Button();
            this.label3 = new Wisej.Web.Label();
            this.TxtFileName = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.pictureBox1 = new Wisej.Web.PictureBox();
            this.panel1 = new Wisej.Web.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnGenPdf
            // 
            this.BtnGenPdf.AppearanceKey = "button-ok";
            this.BtnGenPdf.Location = new System.Drawing.Point(3, 2);
            this.BtnGenPdf.Name = "BtnGenPdf";
            this.BtnGenPdf.Size = new System.Drawing.Size(510, 33);
            this.BtnGenPdf.TabIndex = 0;
            this.BtnGenPdf.Text = "G&enerate PDF";
            this.BtnGenPdf.Click += new System.EventHandler(this.BtnGenPdf_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("@labelText", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.Location = new System.Drawing.Point(19, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Save as";
            // 
            // TxtFileName
            // 
            this.TxtFileName.Location = new System.Drawing.Point(74, 66);
            this.TxtFileName.Name = "TxtFileName";
            this.TxtFileName.Size = new System.Drawing.Size(431, 25);
            this.TxtFileName.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.ImageSource = "icon-header";
            this.pictureBox1.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(230, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(56, 57);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromName("@control");
            this.panel1.Controls.Add(this.BtnGenPdf);
            this.panel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 108);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(516, 37);
            this.panel1.TabIndex = 4;
            this.panel1.TabStop = true;
            // 
            // IncomeReportForm
            // 
            this.ClientSize = new System.Drawing.Size(516, 145);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.TxtFileName);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Name = "IncomeReportForm";
            this.Text = "IncomeReportForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button BtnGenPdf;
        private Label label3;
        private TextBoxWithValidation TxtFileName;
        private PictureBox pictureBox1;
        private Panel panel1;
    }
}