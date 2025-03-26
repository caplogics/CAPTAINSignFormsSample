using Wisej.Web;
using Wisej.Design;

namespace Captain.Common.Views.Forms
{
    partial class RankDefPDF
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RankDefPDF));
            this.lblRnk = new Wisej.Web.Label();
            this.cmbRnk = new Wisej.Web.ComboBox();
            this.btnGenPdf = new Wisej.Web.Button();
            this.btnPDFprev = new Wisej.Web.Button();
            this.cmbHie = new Wisej.Web.ComboBox();
            this.lblHie = new Wisej.Web.Label();
            this.pnlHie = new Wisej.Web.Panel();
            this.pnlGeneratePDF = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlHie.SuspendLayout();
            this.pnlGeneratePDF.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblRnk
            // 
            this.lblRnk.Location = new System.Drawing.Point(15, 47);
            this.lblRnk.Name = "lblRnk";
            this.lblRnk.Size = new System.Drawing.Size(35, 14);
            this.lblRnk.TabIndex = 1;
            this.lblRnk.Text = "Rank";
            // 
            // cmbRnk
            // 
            this.cmbRnk.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbRnk.FormattingEnabled = true;
            this.cmbRnk.Location = new System.Drawing.Point(81, 43);
            this.cmbRnk.Name = "cmbRnk";
            this.cmbRnk.Size = new System.Drawing.Size(303, 25);
            this.cmbRnk.TabIndex = 2;
            this.cmbRnk.SelectedIndexChanged += new System.EventHandler(this.cmbRnk_SelectedIndexChanged);
            // 
            // btnGenPdf
            // 
            this.btnGenPdf.Dock = Wisej.Web.DockStyle.Right;
            this.btnGenPdf.Location = new System.Drawing.Point(240, 5);
            this.btnGenPdf.Name = "btnGenPdf";
            this.btnGenPdf.Size = new System.Drawing.Size(80, 25);
            this.btnGenPdf.TabIndex = 3;
            this.btnGenPdf.Text = "&Generate";
            this.btnGenPdf.Click += new System.EventHandler(this.btnGenPdf_Click);
            // 
            // btnPDFprev
            // 
            this.btnPDFprev.Dock = Wisej.Web.DockStyle.Right;
            this.btnPDFprev.Location = new System.Drawing.Point(323, 5);
            this.btnPDFprev.Name = "btnPDFprev";
            this.btnPDFprev.Size = new System.Drawing.Size(80, 25);
            this.btnPDFprev.TabIndex = 3;
            this.btnPDFprev.Text = "Pre&view";
            this.btnPDFprev.Click += new System.EventHandler(this.btnPDFprev_Click);
            // 
            // cmbHie
            // 
            this.cmbHie.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbHie.FormattingEnabled = true;
            this.cmbHie.Location = new System.Drawing.Point(81, 11);
            this.cmbHie.Name = "cmbHie";
            this.cmbHie.Size = new System.Drawing.Size(303, 25);
            this.cmbHie.TabIndex = 2;
            this.cmbHie.SelectedIndexChanged += new System.EventHandler(this.cmbHie_SelectedIndexChanged);
            // 
            // lblHie
            // 
            this.lblHie.Location = new System.Drawing.Point(15, 15);
            this.lblHie.Name = "lblHie";
            this.lblHie.Size = new System.Drawing.Size(57, 16);
            this.lblHie.TabIndex = 1;
            this.lblHie.Text = "Hierarchy";
            // 
            // pnlHie
            // 
            this.pnlHie.Controls.Add(this.cmbHie);
            this.pnlHie.Controls.Add(this.lblHie);
            this.pnlHie.Controls.Add(this.lblRnk);
            this.pnlHie.Controls.Add(this.cmbRnk);
            this.pnlHie.Dock = Wisej.Web.DockStyle.Top;
            this.pnlHie.Location = new System.Drawing.Point(0, 0);
            this.pnlHie.Name = "pnlHie";
            this.pnlHie.Size = new System.Drawing.Size(418, 77);
            this.pnlHie.TabIndex = 4;
            // 
            // pnlGeneratePDF
            // 
            this.pnlGeneratePDF.AppearanceKey = "panel-grdo";
            this.pnlGeneratePDF.Controls.Add(this.btnGenPdf);
            this.pnlGeneratePDF.Controls.Add(this.spacer1);
            this.pnlGeneratePDF.Controls.Add(this.btnPDFprev);
            this.pnlGeneratePDF.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlGeneratePDF.Location = new System.Drawing.Point(0, 77);
            this.pnlGeneratePDF.Name = "pnlGeneratePDF";
            this.pnlGeneratePDF.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlGeneratePDF.Size = new System.Drawing.Size(418, 35);
            this.pnlGeneratePDF.TabIndex = 5;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(320, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // RankDefPDF
            // 
            this.ClientSize = new System.Drawing.Size(418, 112);
            this.Controls.Add(this.pnlGeneratePDF);
            this.Controls.Add(this.pnlHie);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RankDefPDF";
            this.Text = "Rank Selection for PDF";
            this.pnlHie.ResumeLayout(false);
            this.pnlHie.PerformLayout();
            this.pnlGeneratePDF.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Label lblRnk;
        private ComboBox cmbRnk;
        private Button btnGenPdf;
        private Button btnPDFprev;
        private ComboBox cmbHie;
        private Label lblHie;
        private Panel pnlHie;
        private Panel pnlGeneratePDF;
        private Spacer spacer1;
    }
}