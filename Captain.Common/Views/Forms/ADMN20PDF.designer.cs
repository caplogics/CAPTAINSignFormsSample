using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class ADMN20PDF
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ADMN20PDF));
            this.lblRnk = new Wisej.Web.Label();
            this.lblHie = new Wisej.Web.Label();
            this.cmbHie = new Wisej.Web.ComboBox();
            this.btnPDFprev = new Wisej.Web.Button();
            this.btnGenPdf = new Wisej.Web.Button();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlDetails = new Wisej.Web.Panel();
            this.pnlAgency = new Wisej.Web.Panel();
            this.panel2 = new Wisej.Web.Panel();
            this.chkbPDF = new Wisej.Web.CheckBox();
            this.chkbExcel = new Wisej.Web.CheckBox();
            this.lblRepFormat = new Wisej.Web.Label();
            this.panel5 = new Wisej.Web.Panel();
            this.chkbSPFielCntl = new Wisej.Web.CheckBox();
            this.pnlRptfrmtcntrl = new Wisej.Web.Panel();
            this.panel4 = new Wisej.Web.Panel();
            this.panel1 = new Wisej.Web.Panel();
            this.label1 = new Wisej.Web.Label();
            this.pnlPrgProcess = new Wisej.Web.Panel();
            this.chkPrgProcess = new Wisej.Web.CheckBox();
            this.panel3 = new Wisej.Web.Panel();
            this.label2 = new Wisej.Web.Label();
            this.chkbTargets = new Wisej.Web.CheckBox();
            this.pnlDateRange = new Wisej.Web.Panel();
            this.cmbDteRnge = new Wisej.Web.ComboBox();
            this.lblDteRnge = new Wisej.Web.Label();
            this.pnlReportSel = new Wisej.Web.Panel();
            this.cmbSPOptions = new Wisej.Web.ComboBox();
            this.pnlHie = new Wisej.Web.Panel();
            this.pnlGenerate = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.pnlAgency.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.pnlRptfrmtcntrl.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlPrgProcess.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnlDateRange.SuspendLayout();
            this.pnlReportSel.SuspendLayout();
            this.pnlHie.SuspendLayout();
            this.pnlGenerate.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblRnk
            // 
            this.lblRnk.Location = new System.Drawing.Point(13, 8);
            this.lblRnk.Name = "lblRnk";
            this.lblRnk.Size = new System.Drawing.Size(72, 16);
            this.lblRnk.TabIndex = 1;
            this.lblRnk.Text = "Service Plan";
            // 
            // lblHie
            // 
            this.lblHie.Location = new System.Drawing.Point(13, 7);
            this.lblHie.Name = "lblHie";
            this.lblHie.Size = new System.Drawing.Size(41, 16);
            this.lblHie.TabIndex = 1;
            this.lblHie.Text = "Agency";
            // 
            // cmbHie
            // 
            this.cmbHie.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbHie.FormattingEnabled = true;
            this.cmbHie.Location = new System.Drawing.Point(106, 3);
            this.cmbHie.Name = "cmbHie";
            this.cmbHie.Size = new System.Drawing.Size(367, 25);
            this.cmbHie.TabIndex = 1;
            this.cmbHie.SelectedIndexChanged += new System.EventHandler(this.cmbHie_SelectedIndexChanged);
            // 
            // btnPDFprev
            // 
            this.btnPDFprev.AppearanceKey = "button-reports";
            this.btnPDFprev.Dock = Wisej.Web.DockStyle.Right;
            this.btnPDFprev.Location = new System.Drawing.Point(405, 5);
            this.btnPDFprev.Name = "btnPDFprev";
            this.btnPDFprev.Size = new System.Drawing.Size(80, 25);
            this.btnPDFprev.TabIndex = 17;
            this.btnPDFprev.Text = "Pre&view";
            this.btnPDFprev.Click += new System.EventHandler(this.btnPDFprev_Click);
            // 
            // btnGenPdf
            // 
            this.btnGenPdf.AppearanceKey = "button-reports";
            this.btnGenPdf.Dock = Wisej.Web.DockStyle.Right;
            this.btnGenPdf.Location = new System.Drawing.Point(322, 5);
            this.btnGenPdf.Name = "btnGenPdf";
            this.btnGenPdf.Size = new System.Drawing.Size(80, 25);
            this.btnGenPdf.TabIndex = 16;
            this.btnGenPdf.Text = "&Generate";
            this.btnGenPdf.Click += new System.EventHandler(this.btnGenPdf_Click);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlDetails);
            this.pnlCompleteForm.Controls.Add(this.pnlGenerate);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(500, 243);
            this.pnlCompleteForm.TabIndex = 6;
            // 
            // pnlDetails
            // 
            this.pnlDetails.Controls.Add(this.pnlAgency);
            this.pnlDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlDetails.Location = new System.Drawing.Point(0, 0);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Size = new System.Drawing.Size(500, 208);
            this.pnlDetails.TabIndex = 1;
            // 
            // pnlAgency
            // 
            this.pnlAgency.Controls.Add(this.panel2);
            this.pnlAgency.Controls.Add(this.panel5);
            this.pnlAgency.Controls.Add(this.pnlRptfrmtcntrl);
            this.pnlAgency.Controls.Add(this.pnlPrgProcess);
            this.pnlAgency.Controls.Add(this.panel3);
            this.pnlAgency.Controls.Add(this.pnlDateRange);
            this.pnlAgency.Controls.Add(this.pnlReportSel);
            this.pnlAgency.Controls.Add(this.pnlHie);
            this.pnlAgency.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlAgency.Location = new System.Drawing.Point(0, 0);
            this.pnlAgency.Name = "pnlAgency";
            this.pnlAgency.Size = new System.Drawing.Size(500, 208);
            this.pnlAgency.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkbPDF);
            this.panel2.Controls.Add(this.chkbExcel);
            this.panel2.Controls.Add(this.lblRepFormat);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 174);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(500, 33);
            this.panel2.TabIndex = 12;
            // 
            // chkbPDF
            // 
            this.chkbPDF.CheckState = Wisej.Web.CheckState.Checked;
            this.chkbPDF.Location = new System.Drawing.Point(106, 7);
            this.chkbPDF.Name = "chkbPDF";
            this.chkbPDF.Size = new System.Drawing.Size(56, 21);
            this.chkbPDF.TabIndex = 13;
            this.chkbPDF.Text = "PDF";
            // 
            // chkbExcel
            // 
            this.chkbExcel.Location = new System.Drawing.Point(164, 8);
            this.chkbExcel.Name = "chkbExcel";
            this.chkbExcel.Size = new System.Drawing.Size(62, 21);
            this.chkbExcel.TabIndex = 14;
            this.chkbExcel.Text = "Excel";
            // 
            // lblRepFormat
            // 
            this.lblRepFormat.Location = new System.Drawing.Point(13, 9);
            this.lblRepFormat.Name = "lblRepFormat";
            this.lblRepFormat.Size = new System.Drawing.Size(86, 18);
            this.lblRepFormat.TabIndex = 1;
            this.lblRepFormat.Text = "Output";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.chkbSPFielCntl);
            this.panel5.Dock = Wisej.Web.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 149);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(500, 25);
            this.panel5.TabIndex = 10;
            // 
            // chkbSPFielCntl
            // 
            this.chkbSPFielCntl.AutoSize = false;
            this.chkbSPFielCntl.Location = new System.Drawing.Point(106, 2);
            this.chkbSPFielCntl.Name = "chkbSPFielCntl";
            this.chkbSPFielCntl.Size = new System.Drawing.Size(150, 21);
            this.chkbSPFielCntl.TabIndex = 11;
            this.chkbSPFielCntl.Text = "Field Control Settings";
            this.chkbSPFielCntl.CheckedChanged += new System.EventHandler(this.rbtnRptFormat3_Click);
            // 
            // pnlRptfrmtcntrl
            // 
            this.pnlRptfrmtcntrl.Controls.Add(this.panel4);
            this.pnlRptfrmtcntrl.Controls.Add(this.panel1);
            this.pnlRptfrmtcntrl.Dock = Wisej.Web.DockStyle.Top;
            this.pnlRptfrmtcntrl.Location = new System.Drawing.Point(0, 149);
            this.pnlRptfrmtcntrl.Name = "pnlRptfrmtcntrl";
            this.pnlRptfrmtcntrl.Size = new System.Drawing.Size(500, 0);
            this.pnlRptfrmtcntrl.TabIndex = 15;
            // 
            // panel4
            // 
            this.panel4.Dock = Wisej.Web.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(105, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(395, 0);
            this.panel4.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = Wisej.Web.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(105, 0);
            this.panel1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Report Format";
            // 
            // pnlPrgProcess
            // 
            this.pnlPrgProcess.Controls.Add(this.chkPrgProcess);
            this.pnlPrgProcess.Dock = Wisej.Web.DockStyle.Top;
            this.pnlPrgProcess.Location = new System.Drawing.Point(0, 124);
            this.pnlPrgProcess.Name = "pnlPrgProcess";
            this.pnlPrgProcess.Size = new System.Drawing.Size(500, 25);
            this.pnlPrgProcess.TabIndex = 8;
            // 
            // chkPrgProcess
            // 
            this.chkPrgProcess.AutoSize = false;
            this.chkPrgProcess.Location = new System.Drawing.Point(106, 2);
            this.chkPrgProcess.Name = "chkPrgProcess";
            this.chkPrgProcess.Size = new System.Drawing.Size(125, 21);
            this.chkPrgProcess.TabIndex = 9;
            this.chkPrgProcess.Text = "Program Process";
            this.chkPrgProcess.Click += new System.EventHandler(this.rbtnRptFormat2_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.chkbTargets);
            this.panel3.Dock = Wisej.Web.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 99);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(500, 25);
            this.panel3.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "Include";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkbTargets
            // 
            this.chkbTargets.AutoSize = false;
            this.chkbTargets.Location = new System.Drawing.Point(106, 0);
            this.chkbTargets.Name = "chkbTargets";
            this.chkbTargets.Size = new System.Drawing.Size(71, 21);
            this.chkbTargets.TabIndex = 7;
            this.chkbTargets.Text = "Targets";
            this.chkbTargets.CheckedChanged += new System.EventHandler(this.chkbTargets_CheckedChanged);
            // 
            // pnlDateRange
            // 
            this.pnlDateRange.Controls.Add(this.cmbDteRnge);
            this.pnlDateRange.Controls.Add(this.lblDteRnge);
            this.pnlDateRange.Dock = Wisej.Web.DockStyle.Top;
            this.pnlDateRange.Location = new System.Drawing.Point(0, 66);
            this.pnlDateRange.Name = "pnlDateRange";
            this.pnlDateRange.Size = new System.Drawing.Size(500, 33);
            this.pnlDateRange.TabIndex = 4;
            // 
            // cmbDteRnge
            // 
            this.cmbDteRnge.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbDteRnge.Location = new System.Drawing.Point(106, 3);
            this.cmbDteRnge.Name = "cmbDteRnge";
            this.cmbDteRnge.Size = new System.Drawing.Size(367, 25);
            this.cmbDteRnge.TabIndex = 5;
            // 
            // lblDteRnge
            // 
            this.lblDteRnge.Location = new System.Drawing.Point(13, 7);
            this.lblDteRnge.Name = "lblDteRnge";
            this.lblDteRnge.Size = new System.Drawing.Size(66, 16);
            this.lblDteRnge.TabIndex = 6;
            this.lblDteRnge.Text = "Date Range";
            this.lblDteRnge.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlReportSel
            // 
            this.pnlReportSel.Controls.Add(this.cmbSPOptions);
            this.pnlReportSel.Controls.Add(this.lblRnk);
            this.pnlReportSel.Dock = Wisej.Web.DockStyle.Top;
            this.pnlReportSel.Location = new System.Drawing.Point(0, 33);
            this.pnlReportSel.Name = "pnlReportSel";
            this.pnlReportSel.Size = new System.Drawing.Size(500, 33);
            this.pnlReportSel.TabIndex = 2;
            // 
            // cmbSPOptions
            // 
            this.cmbSPOptions.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbSPOptions.Location = new System.Drawing.Point(106, 4);
            this.cmbSPOptions.Name = "cmbSPOptions";
            this.cmbSPOptions.Size = new System.Drawing.Size(367, 25);
            this.cmbSPOptions.TabIndex = 3;
            this.cmbSPOptions.SelectedIndexChanged += new System.EventHandler(this.cmbSPOptions_SelectedIndexChanged);
            // 
            // pnlHie
            // 
            this.pnlHie.Controls.Add(this.lblHie);
            this.pnlHie.Controls.Add(this.cmbHie);
            this.pnlHie.Dock = Wisej.Web.DockStyle.Top;
            this.pnlHie.Enabled = false;
            this.pnlHie.Location = new System.Drawing.Point(0, 0);
            this.pnlHie.Name = "pnlHie";
            this.pnlHie.Size = new System.Drawing.Size(500, 33);
            this.pnlHie.TabIndex = 0;
            // 
            // pnlGenerate
            // 
            this.pnlGenerate.AppearanceKey = "panel-grdo";
            this.pnlGenerate.Controls.Add(this.btnGenPdf);
            this.pnlGenerate.Controls.Add(this.spacer1);
            this.pnlGenerate.Controls.Add(this.btnPDFprev);
            this.pnlGenerate.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlGenerate.Location = new System.Drawing.Point(0, 208);
            this.pnlGenerate.Name = "pnlGenerate";
            this.pnlGenerate.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlGenerate.Size = new System.Drawing.Size(500, 35);
            this.pnlGenerate.TabIndex = 15;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(402, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // ADMN20PDF
            // 
            this.ClientSize = new System.Drawing.Size(500, 243);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ADMN20PDF";
            this.Text = "ADMN20PDF";
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.pnlAgency.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.pnlRptfrmtcntrl.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlPrgProcess.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.pnlDateRange.ResumeLayout(false);
            this.pnlDateRange.PerformLayout();
            this.pnlReportSel.ResumeLayout(false);
            this.pnlReportSel.PerformLayout();
            this.pnlHie.ResumeLayout(false);
            this.pnlHie.PerformLayout();
            this.pnlGenerate.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Label lblRnk;
        private Label lblHie;
        private ComboBox cmbHie;
        private Button btnPDFprev;
        private Button btnGenPdf;
        private Panel pnlCompleteForm;
        private Panel pnlDetails;
        private Panel pnlAgency;
        private Panel pnlDateRange;
        private Panel pnlGenerate;
        private Spacer spacer1;
        private Panel pnlHie;
        private CheckBox chkbExcel;
        private CheckBox chkbPDF;
        private Label lblRepFormat;
        private ComboBox cmbDteRnge;
        private Label lblDteRnge;
        private CheckBox chkbTargets;
        private Panel pnlPrgProcess;
        private Panel pnlReportSel;
        private CheckBox chkbSPFielCntl;
        private Panel panel2;
        private Panel pnlRptfrmtcntrl;
        private Label label1;
        private Panel panel3;
        private CheckBox chkPrgProcess;
        private ComboBox cmbSPOptions;
        private Panel panel4;
        private Panel panel1;
        private Panel panel5;
        private Label label2;
    }
}