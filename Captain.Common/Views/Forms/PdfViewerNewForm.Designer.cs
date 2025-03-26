using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class PdfViewerNewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PdfViewerNewForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.pnlDetails = new Wisej.Web.Panel();
            this.Btn_Bypass = new Wisej.Web.Button();
            this.spacer2 = new Wisej.Web.Spacer();
            this.Btn_SNP_Details = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.Btn_MST_Details = new Wisej.Web.Button();
            this.panel1 = new Wisej.Web.Panel();
            this.panel3 = new Wisej.Web.Panel();
            this.htmlPanel1 = new Wisej.Web.HtmlPanel();
            this.pdfViewer = new Wisej.Web.PdfViewer();
            this.pnlDetails.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDetails
            // 
            this.pnlDetails.AppearanceKey = "panel-grdo";
            this.pnlDetails.Controls.Add(this.Btn_Bypass);
            this.pnlDetails.Controls.Add(this.spacer2);
            this.pnlDetails.Controls.Add(this.Btn_SNP_Details);
            this.pnlDetails.Controls.Add(this.spacer1);
            this.pnlDetails.Controls.Add(this.Btn_MST_Details);
            this.pnlDetails.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlDetails.Location = new System.Drawing.Point(0, 409);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Padding = new Wisej.Web.Padding(15, 5, 5, 5);
            this.pnlDetails.Size = new System.Drawing.Size(775, 35);
            this.pnlDetails.TabIndex = 1;
            this.pnlDetails.Visible = false;
            // 
            // Btn_Bypass
            // 
            this.Btn_Bypass.Dock = Wisej.Web.DockStyle.Left;
            this.Btn_Bypass.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Btn_Bypass.Location = new System.Drawing.Point(374, 5);
            this.Btn_Bypass.Name = "Btn_Bypass";
            this.Btn_Bypass.Size = new System.Drawing.Size(119, 25);
            this.Btn_Bypass.TabIndex = 1;
            this.Btn_Bypass.Text = "Get &Bypass Report";
            this.Btn_Bypass.Visible = false;
            this.Btn_Bypass.Click += new System.EventHandler(this.Btn_Bypass_Click);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(371, 5);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(3, 25);
            // 
            // Btn_SNP_Details
            // 
            this.Btn_SNP_Details.Dock = Wisej.Web.DockStyle.Left;
            this.Btn_SNP_Details.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Btn_SNP_Details.Location = new System.Drawing.Point(193, 5);
            this.Btn_SNP_Details.Name = "Btn_SNP_Details";
            this.Btn_SNP_Details.Size = new System.Drawing.Size(178, 25);
            this.Btn_SNP_Details.TabIndex = 1;
            this.Btn_SNP_Details.Text = "Get &Individual Details Report";
            this.Btn_SNP_Details.Visible = false;
            this.Btn_SNP_Details.Click += new System.EventHandler(this.Btn_SNP_Details_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(190, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // Btn_MST_Details
            // 
            this.Btn_MST_Details.Dock = Wisej.Web.DockStyle.Left;
            this.Btn_MST_Details.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Btn_MST_Details.Location = new System.Drawing.Point(15, 5);
            this.Btn_MST_Details.Name = "Btn_MST_Details";
            this.Btn_MST_Details.Size = new System.Drawing.Size(175, 25);
            this.Btn_MST_Details.TabIndex = 1;
            this.Btn_MST_Details.Text = "&Household Details Report";
            this.Btn_MST_Details.Visible = false;
            this.Btn_MST_Details.Click += new System.EventHandler(this.Btn_MST_Details_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.pnlDetails);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(775, 444);
            this.panel1.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.htmlPanel1);
            this.panel3.Controls.Add(this.pdfViewer);
            this.panel3.Dock = Wisej.Web.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(775, 409);
            this.panel3.TabIndex = 2;
            // 
            // htmlPanel1
            // 
            this.htmlPanel1.Dock = Wisej.Web.DockStyle.Fill;
            this.htmlPanel1.Focusable = false;
            this.htmlPanel1.Location = new System.Drawing.Point(0, 0);
            this.htmlPanel1.Name = "htmlPanel1";
            this.htmlPanel1.Size = new System.Drawing.Size(775, 409);
            this.htmlPanel1.TabIndex = 2;
            this.htmlPanel1.TabStop = false;
            this.htmlPanel1.Visible = false;
            // 
            // pdfViewer
            // 
            this.pdfViewer.Dock = Wisej.Web.DockStyle.Fill;
            this.pdfViewer.Location = new System.Drawing.Point(0, 0);
            this.pdfViewer.Name = "pdfViewer";
            this.pdfViewer.Size = new System.Drawing.Size(775, 409);
            this.pdfViewer.TabIndex = 0;
            this.pdfViewer.Visible = false;
            // 
            // PdfViewerNewForm
            // 
            this.ClientSize = new System.Drawing.Size(775, 444);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "PdfViewerNewForm";
            this.Text = "Viewer";
            componentTool1.ImageSource = "icon-excel";
            componentTool1.Name = "excel-button";
            componentTool1.ToolTipText = "Detail Report";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.WindowState = Wisej.Web.FormWindowState.Maximized;
            this.FormClosing += new Wisej.Web.FormClosingEventHandler(this.PdfViewerNewForm_FormClosing);
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.PdfViewerNewForm_ToolClick);
            this.pnlDetails.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Panel pnlDetails;
        private Button Btn_Bypass;
        private Button Btn_SNP_Details;
        private Button Btn_MST_Details;
        private Panel panel1;
        private Panel panel3;
        private PdfViewer pdfViewer;
        private Spacer spacer2;
        private Spacer spacer1;
        private HtmlPanel htmlPanel1;
    }
}