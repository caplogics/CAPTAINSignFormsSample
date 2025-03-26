using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class LPMQ0001_Report
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LPMQ0001_Report));
            this.CmbYear = new Wisej.Web.ComboBox();
            this.Txt_HieDesc = new Wisej.Web.TextBox();
            this.pnlHie = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.Pb_Search_Hie = new Wisej.Web.PictureBox();
            this.pnlParams = new Wisej.Web.Panel();
            this.pnlHSource = new Wisej.Web.Panel();
            this.rbHeatBoth = new Wisej.Web.RadioButton();
            this.rbUtilities = new Wisej.Web.RadioButton();
            this.rbDeliverables = new Wisej.Web.RadioButton();
            this.lblHeatSource = new Wisej.Web.Label();
            this.pnlReportType = new Wisej.Web.Panel();
            this.rbDetail = new Wisej.Web.RadioButton();
            this.rbSummary = new Wisej.Web.RadioButton();
            this.lblRepType = new Wisej.Web.Label();
            this.pnlReport = new Wisej.Web.Panel();
            this.rbLIHWAP = new Wisej.Web.RadioButton();
            this.lblRep = new Wisej.Web.Label();
            this.rbLPMQ = new Wisej.Web.RadioButton();
            this.chkbExcel = new Wisej.Web.CheckBox();
            this.btnGeneratePdf = new Wisej.Web.Button();
            this.btnPdfPreview = new Wisej.Web.Button();
            this.btnSaveParameters = new Wisej.Web.Button();
            this.btnGetParameters = new Wisej.Web.Button();
            this.pnlGenerate = new Wisej.Web.Panel();
            this.spacer5 = new Wisej.Web.Spacer();
            this.spacer4 = new Wisej.Web.Spacer();
            this.spacer3 = new Wisej.Web.Spacer();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlHieFilter = new Wisej.Web.Panel();
            this.pnlFilter = new Wisej.Web.Panel();
            this.spacer2 = new Wisej.Web.Spacer();
            this.pnlHie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).BeginInit();
            this.pnlParams.SuspendLayout();
            this.pnlHSource.SuspendLayout();
            this.pnlReportType.SuspendLayout();
            this.pnlReport.SuspendLayout();
            this.pnlGenerate.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlHieFilter.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // CmbYear
            // 
            this.CmbYear.Dock = Wisej.Web.DockStyle.Left;
            this.CmbYear.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbYear.FormattingEnabled = true;
            this.CmbYear.Location = new System.Drawing.Point(675, 0);
            this.CmbYear.Name = "CmbYear";
            this.CmbYear.Size = new System.Drawing.Size(65, 25);
            this.CmbYear.TabIndex = 66;
            this.CmbYear.TabStop = false;
            this.CmbYear.Visible = false;
            this.CmbYear.SelectedIndexChanged += new System.EventHandler(this.CmbYear_SelectedIndexChanged);
            // 
            // Txt_HieDesc
            // 
            this.Txt_HieDesc.BackColor = System.Drawing.Color.Transparent;
            this.Txt_HieDesc.BorderStyle = Wisej.Web.BorderStyle.None;
            this.Txt_HieDesc.Dock = Wisej.Web.DockStyle.Left;
            this.Txt_HieDesc.Font = new System.Drawing.Font("defaultBold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.Txt_HieDesc.ForeColor = System.Drawing.Color.White;
            this.Txt_HieDesc.Location = new System.Drawing.Point(0, 0);
            this.Txt_HieDesc.Name = "Txt_HieDesc";
            this.Txt_HieDesc.ReadOnly = true;
            this.Txt_HieDesc.Size = new System.Drawing.Size(660, 25);
            this.Txt_HieDesc.TabIndex = 55;
            this.Txt_HieDesc.TabStop = false;
            this.Txt_HieDesc.TextAlign = Wisej.Web.HorizontalAlignment.Center;
            // 
            // pnlHie
            // 
            this.pnlHie.BackColor = System.Drawing.Color.FromArgb(11, 70, 117);
            this.pnlHie.Controls.Add(this.CmbYear);
            this.pnlHie.Controls.Add(this.spacer1);
            this.pnlHie.Controls.Add(this.Txt_HieDesc);
            this.pnlHie.Dock = Wisej.Web.DockStyle.Left;
            this.pnlHie.Location = new System.Drawing.Point(15, 9);
            this.pnlHie.Name = "pnlHie";
            this.pnlHie.Size = new System.Drawing.Size(743, 25);
            this.pnlHie.TabIndex = 77;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(660, 0);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(15, 25);
            // 
            // Pb_Search_Hie
            // 
            this.Pb_Search_Hie.BackColor = System.Drawing.Color.FromArgb(244, 244, 244);
            this.Pb_Search_Hie.CssStyle = "border-radius:25px  ";
            this.Pb_Search_Hie.Cursor = Wisej.Web.Cursors.Hand;
            this.Pb_Search_Hie.Dock = Wisej.Web.DockStyle.Left;
            this.Pb_Search_Hie.ImageSource = "captain-filter";
            this.Pb_Search_Hie.Location = new System.Drawing.Point(15, 0);
            this.Pb_Search_Hie.Name = "Pb_Search_Hie";
            this.Pb_Search_Hie.Padding = new Wisej.Web.Padding(4, 5, 4, 4);
            this.Pb_Search_Hie.Size = new System.Drawing.Size(25, 25);
            this.Pb_Search_Hie.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.Pb_Search_Hie.ToolTipText = "Select Hierarchy";
            this.Pb_Search_Hie.Click += new System.EventHandler(this.Pb_Search_Hie_Click);
            // 
            // pnlParams
            // 
            this.pnlParams.Controls.Add(this.pnlHSource);
            this.pnlParams.Controls.Add(this.pnlReportType);
            this.pnlParams.Controls.Add(this.pnlReport);
            this.pnlParams.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlParams.Location = new System.Drawing.Point(0, 43);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new System.Drawing.Size(813, 92);
            this.pnlParams.TabIndex = 1;
            // 
            // pnlHSource
            // 
            this.pnlHSource.Controls.Add(this.rbHeatBoth);
            this.pnlHSource.Controls.Add(this.rbUtilities);
            this.pnlHSource.Controls.Add(this.rbDeliverables);
            this.pnlHSource.Controls.Add(this.lblHeatSource);
            this.pnlHSource.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlHSource.Location = new System.Drawing.Point(0, 62);
            this.pnlHSource.Name = "pnlHSource";
            this.pnlHSource.Size = new System.Drawing.Size(813, 30);
            this.pnlHSource.TabIndex = 3;
            // 
            // rbHeatBoth
            // 
            this.rbHeatBoth.AutoSize = false;
            this.rbHeatBoth.Checked = true;
            this.rbHeatBoth.Location = new System.Drawing.Point(571, 2);
            this.rbHeatBoth.Name = "rbHeatBoth";
            this.rbHeatBoth.Size = new System.Drawing.Size(54, 21);
            this.rbHeatBoth.TabIndex = 3;
            this.rbHeatBoth.TabStop = true;
            this.rbHeatBoth.Text = "Both";
            // 
            // rbUtilities
            // 
            this.rbUtilities.AutoSize = false;
            this.rbUtilities.Location = new System.Drawing.Point(446, 2);
            this.rbUtilities.Name = "rbUtilities";
            this.rbUtilities.Size = new System.Drawing.Size(70, 21);
            this.rbUtilities.TabIndex = 2;
            this.rbUtilities.Text = "Utilities";
            // 
            // rbDeliverables
            // 
            this.rbDeliverables.AutoSize = false;
            this.rbDeliverables.Checked = true;
            this.rbDeliverables.Location = new System.Drawing.Point(282, 2);
            this.rbDeliverables.Name = "rbDeliverables";
            this.rbDeliverables.Size = new System.Drawing.Size(98, 21);
            this.rbDeliverables.TabIndex = 1;
            this.rbDeliverables.TabStop = true;
            this.rbDeliverables.Text = "Deliverables";
            // 
            // lblHeatSource
            // 
            this.lblHeatSource.Location = new System.Drawing.Point(194, 5);
            this.lblHeatSource.Name = "lblHeatSource";
            this.lblHeatSource.RightToLeft = Wisej.Web.RightToLeft.No;
            this.lblHeatSource.Size = new System.Drawing.Size(71, 16);
            this.lblHeatSource.TabIndex = 0;
            this.lblHeatSource.Text = "Heat Source";
            // 
            // pnlReportType
            // 
            this.pnlReportType.Controls.Add(this.rbDetail);
            this.pnlReportType.Controls.Add(this.rbSummary);
            this.pnlReportType.Controls.Add(this.lblRepType);
            this.pnlReportType.Dock = Wisej.Web.DockStyle.Top;
            this.pnlReportType.Location = new System.Drawing.Point(0, 35);
            this.pnlReportType.Name = "pnlReportType";
            this.pnlReportType.Size = new System.Drawing.Size(813, 27);
            this.pnlReportType.TabIndex = 2;
            // 
            // rbDetail
            // 
            this.rbDetail.AutoSize = false;
            this.rbDetail.Location = new System.Drawing.Point(446, 2);
            this.rbDetail.Name = "rbDetail";
            this.rbDetail.Size = new System.Drawing.Size(63, 21);
            this.rbDetail.TabIndex = 2;
            this.rbDetail.Text = "Detail";
            // 
            // rbSummary
            // 
            this.rbSummary.AutoSize = false;
            this.rbSummary.Checked = true;
            this.rbSummary.Location = new System.Drawing.Point(282, 2);
            this.rbSummary.Name = "rbSummary";
            this.rbSummary.Size = new System.Drawing.Size(81, 21);
            this.rbSummary.TabIndex = 1;
            this.rbSummary.TabStop = true;
            this.rbSummary.Text = "Summary";
            // 
            // lblRepType
            // 
            this.lblRepType.Location = new System.Drawing.Point(194, 5);
            this.lblRepType.Name = "lblRepType";
            this.lblRepType.RightToLeft = Wisej.Web.RightToLeft.No;
            this.lblRepType.Size = new System.Drawing.Size(69, 16);
            this.lblRepType.TabIndex = 0;
            this.lblRepType.Text = "Report Type";
            // 
            // pnlReport
            // 
            this.pnlReport.Controls.Add(this.rbLIHWAP);
            this.pnlReport.Controls.Add(this.lblRep);
            this.pnlReport.Controls.Add(this.rbLPMQ);
            this.pnlReport.Dock = Wisej.Web.DockStyle.Top;
            this.pnlReport.Location = new System.Drawing.Point(0, 0);
            this.pnlReport.Name = "pnlReport";
            this.pnlReport.Size = new System.Drawing.Size(813, 35);
            this.pnlReport.TabIndex = 1;
            // 
            // rbLIHWAP
            // 
            this.rbLIHWAP.AutoSize = false;
            this.rbLIHWAP.Location = new System.Drawing.Point(446, 12);
            this.rbLIHWAP.Name = "rbLIHWAP";
            this.rbLIHWAP.Size = new System.Drawing.Size(113, 21);
            this.rbLIHWAP.TabIndex = 2;
            this.rbLIHWAP.Text = "LIHWAP Report";
            this.rbLIHWAP.Click += new System.EventHandler(this.rbLIHWAP_Click);
            // 
            // lblRep
            // 
            this.lblRep.Location = new System.Drawing.Point(194, 15);
            this.lblRep.Name = "lblRep";
            this.lblRep.RightToLeft = Wisej.Web.RightToLeft.No;
            this.lblRep.Size = new System.Drawing.Size(40, 16);
            this.lblRep.TabIndex = 0;
            this.lblRep.Text = "Report";
            // 
            // rbLPMQ
            // 
            this.rbLPMQ.AutoSize = false;
            this.rbLPMQ.Checked = true;
            this.rbLPMQ.Location = new System.Drawing.Point(282, 12);
            this.rbLPMQ.Name = "rbLPMQ";
            this.rbLPMQ.Size = new System.Drawing.Size(144, 21);
            this.rbLPMQ.TabIndex = 1;
            this.rbLPMQ.TabStop = true;
            this.rbLPMQ.Text = "LIHEAP Performance";
            this.rbLPMQ.Click += new System.EventHandler(this.rbLIHWAP_Click);
            // 
            // chkbExcel
            // 
            this.chkbExcel.Dock = Wisej.Web.DockStyle.Right;
            this.chkbExcel.Location = new System.Drawing.Point(366, 5);
            this.chkbExcel.Name = "chkbExcel";
            this.chkbExcel.Size = new System.Drawing.Size(115, 25);
            this.chkbExcel.TabIndex = 3;
            this.chkbExcel.Text = "Generate Excel";
            // 
            // btnGeneratePdf
            // 
            this.btnGeneratePdf.AppearanceKey = "button-reports";
            this.btnGeneratePdf.Dock = Wisej.Web.DockStyle.Right;
            this.btnGeneratePdf.Location = new System.Drawing.Point(625, 5);
            this.btnGeneratePdf.Name = "btnGeneratePdf";
            this.btnGeneratePdf.Size = new System.Drawing.Size(85, 25);
            this.btnGeneratePdf.TabIndex = 4;
            this.btnGeneratePdf.Text = "&Generate";
            this.btnGeneratePdf.Click += new System.EventHandler(this.btnGeneratePdf_Click);
            // 
            // btnPdfPreview
            // 
            this.btnPdfPreview.AppearanceKey = "button-reports";
            this.btnPdfPreview.Dock = Wisej.Web.DockStyle.Right;
            this.btnPdfPreview.Location = new System.Drawing.Point(713, 5);
            this.btnPdfPreview.Name = "btnPdfPreview";
            this.btnPdfPreview.Size = new System.Drawing.Size(85, 25);
            this.btnPdfPreview.TabIndex = 5;
            this.btnPdfPreview.Text = "Pre&view";
            this.btnPdfPreview.Click += new System.EventHandler(this.btnPdfPreview_Click);
            // 
            // btnSaveParameters
            // 
            this.btnSaveParameters.AppearanceKey = "button-reports";
            this.btnSaveParameters.Dock = Wisej.Web.DockStyle.Left;
            this.btnSaveParameters.Location = new System.Drawing.Point(128, 5);
            this.btnSaveParameters.Name = "btnSaveParameters";
            this.btnSaveParameters.Size = new System.Drawing.Size(110, 25);
            this.btnSaveParameters.TabIndex = 2;
            this.btnSaveParameters.Text = "&Save Parameters";
            this.btnSaveParameters.Click += new System.EventHandler(this.btnSaveParameters_Click);
            // 
            // btnGetParameters
            // 
            this.btnGetParameters.AppearanceKey = "button-reports";
            this.btnGetParameters.Dock = Wisej.Web.DockStyle.Left;
            this.btnGetParameters.Location = new System.Drawing.Point(15, 5);
            this.btnGetParameters.Name = "btnGetParameters";
            this.btnGetParameters.Size = new System.Drawing.Size(110, 25);
            this.btnGetParameters.TabIndex = 1;
            this.btnGetParameters.Text = "Get &Parameters";
            this.btnGetParameters.Click += new System.EventHandler(this.btnGetParameters_Click);
            // 
            // pnlGenerate
            // 
            this.pnlGenerate.AppearanceKey = "panel-grdo";
            this.pnlGenerate.Controls.Add(this.btnSaveParameters);
            this.pnlGenerate.Controls.Add(this.spacer5);
            this.pnlGenerate.Controls.Add(this.chkbExcel);
            this.pnlGenerate.Controls.Add(this.spacer4);
            this.pnlGenerate.Controls.Add(this.btnGeneratePdf);
            this.pnlGenerate.Controls.Add(this.spacer3);
            this.pnlGenerate.Controls.Add(this.btnPdfPreview);
            this.pnlGenerate.Controls.Add(this.btnGetParameters);
            this.pnlGenerate.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlGenerate.Location = new System.Drawing.Point(0, 135);
            this.pnlGenerate.Name = "pnlGenerate";
            this.pnlGenerate.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlGenerate.Size = new System.Drawing.Size(813, 35);
            this.pnlGenerate.TabIndex = 2;
            // 
            // spacer5
            // 
            this.spacer5.Dock = Wisej.Web.DockStyle.Left;
            this.spacer5.Location = new System.Drawing.Point(125, 5);
            this.spacer5.Name = "spacer5";
            this.spacer5.Size = new System.Drawing.Size(3, 25);
            // 
            // spacer4
            // 
            this.spacer4.Dock = Wisej.Web.DockStyle.Right;
            this.spacer4.Location = new System.Drawing.Point(481, 5);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(144, 25);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Right;
            this.spacer3.Location = new System.Drawing.Point(710, 5);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(3, 25);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlParams);
            this.pnlCompleteForm.Controls.Add(this.pnlHieFilter);
            this.pnlCompleteForm.Controls.Add(this.pnlGenerate);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(813, 170);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlHieFilter
            // 
            this.pnlHieFilter.BackColor = System.Drawing.Color.FromArgb(11, 70, 117);
            this.pnlHieFilter.Controls.Add(this.pnlFilter);
            this.pnlHieFilter.Controls.Add(this.pnlHie);
            this.pnlHieFilter.Dock = Wisej.Web.DockStyle.Top;
            this.pnlHieFilter.Location = new System.Drawing.Point(0, 0);
            this.pnlHieFilter.Name = "pnlHieFilter";
            this.pnlHieFilter.Padding = new Wisej.Web.Padding(15, 9, 9, 9);
            this.pnlHieFilter.Size = new System.Drawing.Size(813, 43);
            this.pnlHieFilter.TabIndex = 22;
            // 
            // pnlFilter
            // 
            this.pnlFilter.BackColor = System.Drawing.Color.FromArgb(11, 70, 117);
            this.pnlFilter.Controls.Add(this.Pb_Search_Hie);
            this.pnlFilter.Controls.Add(this.spacer2);
            this.pnlFilter.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlFilter.Location = new System.Drawing.Point(758, 9);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(46, 25);
            this.pnlFilter.TabIndex = 99;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(0, 0);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(15, 25);
            // 
            // LPMQ0001_Report
            // 
            this.ClientSize = new System.Drawing.Size(813, 170);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LPMQ0001_Report";
            this.Text = "LPMQ0001_Report";
            this.pnlHie.ResumeLayout(false);
            this.pnlHie.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).EndInit();
            this.pnlParams.ResumeLayout(false);
            this.pnlHSource.ResumeLayout(false);
            this.pnlReportType.ResumeLayout(false);
            this.pnlReport.ResumeLayout(false);
            this.pnlGenerate.ResumeLayout(false);
            this.pnlGenerate.PerformLayout();
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlHieFilter.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComboBox CmbYear;
        private TextBox Txt_HieDesc;
        private Panel pnlHie;
        private PictureBox Pb_Search_Hie;
        private Panel pnlParams;
        private Label lblRepType;
        private Panel pnlReportType;
        private RadioButton rbDetail;
        private RadioButton rbSummary;
        private Panel pnlHSource;
        private RadioButton rbHeatBoth;
        private RadioButton rbUtilities;
        private RadioButton rbDeliverables;
        private Label lblHeatSource;
        private CheckBox chkbExcel;
        private Button btnGeneratePdf;
        private Button btnPdfPreview;
        private Button btnSaveParameters;
        private Button btnGetParameters;
        private Panel pnlGenerate;
        private Panel pnlReport;
        private RadioButton rbLIHWAP;
        private RadioButton rbLPMQ;
        private Label lblRep;
        private Panel pnlCompleteForm;
        private Panel pnlHieFilter;
        private Panel pnlFilter;
        private Spacer spacer1;
        private Spacer spacer2;
        private Spacer spacer3;
        private Spacer spacer5;
        private Spacer spacer4;
    }
}