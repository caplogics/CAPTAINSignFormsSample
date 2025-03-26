using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class ADMNB005
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ADMNB005));
            this.BtnPdfPrev = new Wisej.Web.Button();
            this.btnGenPdf = new Wisej.Web.Button();
            this.pnlSave = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlHieFilter = new Wisej.Web.Panel();
            this.pnlFilter = new Wisej.Web.Panel();
            this.Pb_Search_Hie = new Wisej.Web.PictureBox();
            this.spacer3 = new Wisej.Web.Spacer();
            this.pnlHie = new Wisej.Web.Panel();
            this.CmbYear = new Wisej.Web.ComboBox();
            this.spacer2 = new Wisej.Web.Spacer();
            this.Txt_HieDesc = new Wisej.Web.TextBox();
            this.pnlParameters = new Wisej.Web.Panel();
            this.pnlDteRange = new Wisej.Web.Panel();
            this.dtpToDte = new Wisej.Web.DateTimePicker();
            this.lblToDte = new Wisej.Web.Label();
            this.dtpFrmDte = new Wisej.Web.DateTimePicker();
            this.lblFrmDte = new Wisej.Web.Label();
            this.pnlReportFormt = new Wisej.Web.Panel();
            this.cmbReport = new Wisej.Web.ComboBox();
            this.lblReportFormat = new Wisej.Web.Label();
            this.pnlAgy = new Wisej.Web.Panel();
            this.cmbAgy = new Wisej.Web.ComboBox();
            this.lblAgy = new Wisej.Web.Label();
            this.pnlSave.SuspendLayout();
            this.pnlHieFilter.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).BeginInit();
            this.pnlHie.SuspendLayout();
            this.pnlParameters.SuspendLayout();
            this.pnlDteRange.SuspendLayout();
            this.pnlReportFormt.SuspendLayout();
            this.pnlAgy.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnPdfPrev
            // 
            this.BtnPdfPrev.AppearanceKey = "button-reports";
            this.BtnPdfPrev.Dock = Wisej.Web.DockStyle.Right;
            this.BtnPdfPrev.Location = new System.Drawing.Point(662, 5);
            this.BtnPdfPrev.Name = "BtnPdfPrev";
            this.BtnPdfPrev.Size = new System.Drawing.Size(80, 25);
            this.BtnPdfPrev.TabIndex = 2;
            this.BtnPdfPrev.Text = "Pre&view";
            this.BtnPdfPrev.Click += new System.EventHandler(this.BtnPdfPrev_Click);
            // 
            // btnGenPdf
            // 
            this.btnGenPdf.AppearanceKey = "button-reports";
            this.btnGenPdf.Dock = Wisej.Web.DockStyle.Right;
            this.btnGenPdf.Location = new System.Drawing.Point(579, 5);
            this.btnGenPdf.Name = "btnGenPdf";
            this.btnGenPdf.Size = new System.Drawing.Size(80, 25);
            this.btnGenPdf.TabIndex = 1;
            this.btnGenPdf.Text = "G&enerate";
            this.btnGenPdf.Click += new System.EventHandler(this.BtnGenFile_Click);
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnGenPdf);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.BtnPdfPrev);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 154);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(757, 35);
            this.pnlSave.TabIndex = 2;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(659, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
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
            this.pnlHieFilter.Size = new System.Drawing.Size(757, 43);
            this.pnlHieFilter.TabIndex = 99;
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.Pb_Search_Hie);
            this.pnlFilter.Controls.Add(this.spacer3);
            this.pnlFilter.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlFilter.Location = new System.Drawing.Point(691, 9);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(57, 25);
            this.pnlFilter.TabIndex = 55;
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
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Left;
            this.spacer3.Location = new System.Drawing.Point(0, 0);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(15, 25);
            // 
            // pnlHie
            // 
            this.pnlHie.Controls.Add(this.CmbYear);
            this.pnlHie.Controls.Add(this.spacer2);
            this.pnlHie.Controls.Add(this.Txt_HieDesc);
            this.pnlHie.Dock = Wisej.Web.DockStyle.Left;
            this.pnlHie.Location = new System.Drawing.Point(15, 9);
            this.pnlHie.Name = "pnlHie";
            this.pnlHie.Size = new System.Drawing.Size(676, 25);
            this.pnlHie.TabIndex = 88;
            // 
            // CmbYear
            // 
            this.CmbYear.Dock = Wisej.Web.DockStyle.Left;
            this.CmbYear.Location = new System.Drawing.Point(610, 0);
            this.CmbYear.Name = "CmbYear";
            this.CmbYear.Size = new System.Drawing.Size(65, 25);
            this.CmbYear.TabIndex = 66;
            this.CmbYear.TabStop = false;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(595, 0);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(15, 25);
            // 
            // Txt_HieDesc
            // 
            this.Txt_HieDesc.BackColor = System.Drawing.Color.Transparent;
            this.Txt_HieDesc.BorderStyle = Wisej.Web.BorderStyle.None;
            this.Txt_HieDesc.Dock = Wisej.Web.DockStyle.Left;
            this.Txt_HieDesc.Font = new System.Drawing.Font("defaultBold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.Txt_HieDesc.ForeColor = System.Drawing.Color.FromName("@window");
            this.Txt_HieDesc.Location = new System.Drawing.Point(0, 0);
            this.Txt_HieDesc.Name = "Txt_HieDesc";
            this.Txt_HieDesc.Size = new System.Drawing.Size(595, 25);
            this.Txt_HieDesc.TabIndex = 77;
            this.Txt_HieDesc.TabStop = false;
            this.Txt_HieDesc.TextAlign = Wisej.Web.HorizontalAlignment.Center;
            // 
            // pnlParameters
            // 
            this.pnlParameters.Controls.Add(this.pnlDteRange);
            this.pnlParameters.Controls.Add(this.pnlReportFormt);
            this.pnlParameters.Controls.Add(this.pnlAgy);
            this.pnlParameters.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlParameters.Location = new System.Drawing.Point(0, 43);
            this.pnlParameters.Name = "pnlParameters";
            this.pnlParameters.Size = new System.Drawing.Size(757, 111);
            this.pnlParameters.TabIndex = 100;
            // 
            // pnlDteRange
            // 
            this.pnlDteRange.Controls.Add(this.dtpToDte);
            this.pnlDteRange.Controls.Add(this.lblToDte);
            this.pnlDteRange.Controls.Add(this.dtpFrmDte);
            this.pnlDteRange.Controls.Add(this.lblFrmDte);
            this.pnlDteRange.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlDteRange.Location = new System.Drawing.Point(0, 75);
            this.pnlDteRange.Name = "pnlDteRange";
            this.pnlDteRange.Size = new System.Drawing.Size(757, 36);
            this.pnlDteRange.TabIndex = 2;
            // 
            // dtpToDte
            // 
            this.dtpToDte.AutoSize = false;
            this.dtpToDte.CustomFormat = "MM/dd/yyyy";
            this.dtpToDte.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpToDte.Location = new System.Drawing.Point(395, 2);
            this.dtpToDte.Name = "dtpToDte";
            this.dtpToDte.ShowCheckBox = true;
            this.dtpToDte.Size = new System.Drawing.Size(116, 25);
            this.dtpToDte.TabIndex = 6;
            // 
            // lblToDte
            // 
            this.lblToDte.AutoSize = true;
            this.lblToDte.Location = new System.Drawing.Point(375, 6);
            this.lblToDte.MinimumSize = new System.Drawing.Size(0, 16);
            this.lblToDte.Name = "lblToDte";
            this.lblToDte.Size = new System.Drawing.Size(19, 16);
            this.lblToDte.TabIndex = 5;
            this.lblToDte.Text = "To";
            // 
            // dtpFrmDte
            // 
            this.dtpFrmDte.AutoSize = false;
            this.dtpFrmDte.CustomFormat = "MM/dd/yyyy";
            this.dtpFrmDte.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpFrmDte.Location = new System.Drawing.Point(228, 2);
            this.dtpFrmDte.Name = "dtpFrmDte";
            this.dtpFrmDte.ShowCheckBox = true;
            this.dtpFrmDte.Size = new System.Drawing.Size(116, 25);
            this.dtpFrmDte.TabIndex = 4;
            // 
            // lblFrmDte
            // 
            this.lblFrmDte.AutoSize = true;
            this.lblFrmDte.Location = new System.Drawing.Point(189, 6);
            this.lblFrmDte.MinimumSize = new System.Drawing.Size(0, 16);
            this.lblFrmDte.Name = "lblFrmDte";
            this.lblFrmDte.Size = new System.Drawing.Size(32, 16);
            this.lblFrmDte.TabIndex = 3;
            this.lblFrmDte.Text = "From";
            // 
            // pnlReportFormt
            // 
            this.pnlReportFormt.Controls.Add(this.cmbReport);
            this.pnlReportFormt.Controls.Add(this.lblReportFormat);
            this.pnlReportFormt.Dock = Wisej.Web.DockStyle.Top;
            this.pnlReportFormt.Location = new System.Drawing.Point(0, 45);
            this.pnlReportFormt.Name = "pnlReportFormt";
            this.pnlReportFormt.Size = new System.Drawing.Size(757, 30);
            this.pnlReportFormt.TabIndex = 1;
            // 
            // cmbReport
            // 
            this.cmbReport.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbReport.Location = new System.Drawing.Point(190, 2);
            this.cmbReport.Name = "cmbReport";
            this.cmbReport.Size = new System.Drawing.Size(321, 25);
            this.cmbReport.TabIndex = 3;
            this.cmbReport.SelectedIndexChanged += new System.EventHandler(this.cmbReport_SelectedIndexChanged);
            // 
            // lblReportFormat
            // 
            this.lblReportFormat.AutoSize = true;
            this.lblReportFormat.Location = new System.Drawing.Point(95, 6);
            this.lblReportFormat.MinimumSize = new System.Drawing.Size(85, 16);
            this.lblReportFormat.Name = "lblReportFormat";
            this.lblReportFormat.Size = new System.Drawing.Size(85, 16);
            this.lblReportFormat.TabIndex = 2;
            this.lblReportFormat.Text = "Report Format";
            // 
            // pnlAgy
            // 
            this.pnlAgy.Controls.Add(this.cmbAgy);
            this.pnlAgy.Controls.Add(this.lblAgy);
            this.pnlAgy.Dock = Wisej.Web.DockStyle.Top;
            this.pnlAgy.Location = new System.Drawing.Point(0, 0);
            this.pnlAgy.Name = "pnlAgy";
            this.pnlAgy.Size = new System.Drawing.Size(757, 45);
            this.pnlAgy.TabIndex = 0;
            this.pnlAgy.Visible = false;
            // 
            // cmbAgy
            // 
            this.cmbAgy.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbAgy.Location = new System.Drawing.Point(190, 17);
            this.cmbAgy.Name = "cmbAgy";
            this.cmbAgy.Size = new System.Drawing.Size(417, 25);
            this.cmbAgy.TabIndex = 1;
            this.cmbAgy.Visible = false;
            // 
            // lblAgy
            // 
            this.lblAgy.AutoSize = true;
            this.lblAgy.Location = new System.Drawing.Point(95, 21);
            this.lblAgy.MinimumSize = new System.Drawing.Size(0, 16);
            this.lblAgy.Name = "lblAgy";
            this.lblAgy.Size = new System.Drawing.Size(45, 16);
            this.lblAgy.TabIndex = 0;
            this.lblAgy.Text = "Agency";
            this.lblAgy.Visible = false;
            // 
            // ADMNB005
            // 
            this.ClientSize = new System.Drawing.Size(757, 189);
            this.Controls.Add(this.pnlParameters);
            this.Controls.Add(this.pnlHieFilter);
            this.Controls.Add(this.pnlSave);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ADMNB005";
            this.Text = "Image Types Report";
            this.pnlSave.ResumeLayout(false);
            this.pnlHieFilter.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).EndInit();
            this.pnlHie.ResumeLayout(false);
            this.pnlHie.PerformLayout();
            this.pnlParameters.ResumeLayout(false);
            this.pnlDteRange.ResumeLayout(false);
            this.pnlDteRange.PerformLayout();
            this.pnlReportFormt.ResumeLayout(false);
            this.pnlReportFormt.PerformLayout();
            this.pnlAgy.ResumeLayout(false);
            this.pnlAgy.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion
        private Button BtnPdfPrev;
        private Button btnGenPdf;
        private Panel pnlSave;
        private Spacer spacer1;
        private Panel pnlHieFilter;
        private Panel pnlFilter;
        private Panel pnlHie;
        private TextBox Txt_HieDesc;
        private ComboBox CmbYear;
        private Spacer spacer2;
        private PictureBox Pb_Search_Hie;
        private Spacer spacer3;
        private Panel pnlParameters;
        private Panel pnlDteRange;
        private Panel pnlReportFormt;
        private Panel pnlAgy;
        private ComboBox cmbReport;
        private Label lblReportFormat;
        private ComboBox cmbAgy;
        private Label lblAgy;
        private DateTimePicker dtpToDte;
        private Label lblToDte;
        private DateTimePicker dtpFrmDte;
        private Label lblFrmDte;
    }
}