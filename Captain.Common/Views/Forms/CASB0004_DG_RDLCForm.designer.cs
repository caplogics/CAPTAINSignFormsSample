using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class CASB0004_DG_RDLCForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CASB0004_DG_RDLCForm));
            this.Btn_Bypass = new Wisej.Web.Button();
            this.Btn_SNP_Details = new Wisej.Web.Button();
            this.Btn_MST_Details = new Wisej.Web.Button();
            this.pnlReportBtns = new Wisej.Web.Panel();
            this.spacer2 = new Wisej.Web.Spacer();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlReportBtns.SuspendLayout();
            this.SuspendLayout();
            // 
            // Btn_Bypass
            // 
            this.Btn_Bypass.Dock = Wisej.Web.DockStyle.Left;
            this.Btn_Bypass.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Btn_Bypass.Location = new System.Drawing.Point(381, 5);
            this.Btn_Bypass.Name = "Btn_Bypass";
            this.Btn_Bypass.Size = new System.Drawing.Size(130, 25);
            this.Btn_Bypass.TabIndex = 1;
            this.Btn_Bypass.Text = "Get &Bypass Report";
            this.Btn_Bypass.Visible = false;
            this.Btn_Bypass.Click += new System.EventHandler(this.Btn_Bypass_Click);
            // 
            // Btn_SNP_Details
            // 
            this.Btn_SNP_Details.Dock = Wisej.Web.DockStyle.Left;
            this.Btn_SNP_Details.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Btn_SNP_Details.Location = new System.Drawing.Point(15, 5);
            this.Btn_SNP_Details.Name = "Btn_SNP_Details";
            this.Btn_SNP_Details.Size = new System.Drawing.Size(190, 25);
            this.Btn_SNP_Details.TabIndex = 1;
            this.Btn_SNP_Details.Text = "Get &Individual Details Report";
            this.Btn_SNP_Details.Visible = false;
            this.Btn_SNP_Details.Click += new System.EventHandler(this.Btn_SNP_Details_Click);
            // 
            // Btn_MST_Details
            // 
            this.Btn_MST_Details.Dock = Wisej.Web.DockStyle.Left;
            this.Btn_MST_Details.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Btn_MST_Details.Location = new System.Drawing.Point(208, 5);
            this.Btn_MST_Details.Name = "Btn_MST_Details";
            this.Btn_MST_Details.Size = new System.Drawing.Size(170, 25);
            this.Btn_MST_Details.TabIndex = 1;
            this.Btn_MST_Details.Text = "Get &Family Details Report";
            this.Btn_MST_Details.Visible = false;
            this.Btn_MST_Details.Click += new System.EventHandler(this.Btn_MST_Details_Click);
            // 
            // pnlReportBtns
            // 
            this.pnlReportBtns.AppearanceKey = "panel-grdo";
            this.pnlReportBtns.Controls.Add(this.Btn_Bypass);
            this.pnlReportBtns.Controls.Add(this.spacer2);
            this.pnlReportBtns.Controls.Add(this.Btn_MST_Details);
            this.pnlReportBtns.Controls.Add(this.spacer1);
            this.pnlReportBtns.Controls.Add(this.Btn_SNP_Details);
            this.pnlReportBtns.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlReportBtns.Location = new System.Drawing.Point(0, 512);
            this.pnlReportBtns.Name = "pnlReportBtns";
            this.pnlReportBtns.Padding = new Wisej.Web.Padding(15, 5, 5, 5);
            this.pnlReportBtns.Size = new System.Drawing.Size(798, 35);
            this.pnlReportBtns.TabIndex = 2;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(378, 5);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(3, 25);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(205, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // CASB0004_DG_RDLCForm
            // 
            this.ClientSize = new System.Drawing.Size(798, 547);
            this.Controls.Add(this.pnlReportBtns);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.Name = "CASB0004_DG_RDLCForm";
            this.Text = "CASB0004_DG_RDLCForm";
            this.pnlReportBtns.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

       // private Gizmox.WebGUI.Reporting.ReportViewer rvViewer;
        private Button Btn_Bypass;
        private Button Btn_SNP_Details;
        private Button Btn_MST_Details;
        private Panel pnlReportBtns;
        private Spacer spacer2;
        private Spacer spacer1;
    }
}