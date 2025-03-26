using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class HSSB2102_TrackReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSSB2102_TrackReport));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.CmbYear = new Wisej.Web.ComboBox();
            this.Txt_HieDesc = new Wisej.Web.TextBox();
            this.pnlHie = new Wisej.Web.Panel();
            this.Pb_Search_Hie = new Wisej.Web.PictureBox();
            this.pnlParams = new Wisej.Web.Panel();
            this.btnSaveParameters = new Wisej.Web.Button();
            this.btnGetParameters = new Wisej.Web.Button();
            this.BtnPdfPrev = new Wisej.Web.Button();
            this.BtnGenPdf = new Wisej.Web.Button();
            this.lblTo = new Wisej.Web.Label();
            this.cmbCompTo = new ComboBoxEx();
            this.cmbCompFrom = new ComboBoxEx();
            this.lblFrom = new Wisej.Web.Label();
            this.lblComp = new Wisej.Web.Label();
            this.cmbSeq = new Wisej.Web.ComboBox();
            this.lblSeq = new Wisej.Web.Label();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlHieFilter = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlFilter = new Wisej.Web.Panel();
            this.spacer2 = new Wisej.Web.Spacer();
            this.pnlGenerate = new Wisej.Web.Panel();
            this.spacer3 = new Wisej.Web.Spacer();
            this.spacer4 = new Wisej.Web.Spacer();
            this.pnlHie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).BeginInit();
            this.pnlParams.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlHieFilter.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.pnlGenerate.SuspendLayout();
            this.SuspendLayout();
            // 
            // CmbYear
            // 
            this.CmbYear.Dock = Wisej.Web.DockStyle.Left;
            this.CmbYear.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbYear.FormattingEnabled = true;
            this.CmbYear.Location = new System.Drawing.Point(655, 0);
            this.CmbYear.Name = "CmbYear";
            this.CmbYear.Size = new System.Drawing.Size(65, 25);
            this.CmbYear.TabIndex = 44;
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
            this.Txt_HieDesc.Size = new System.Drawing.Size(640, 25);
            this.Txt_HieDesc.TabIndex = 77;
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
            this.pnlHie.Size = new System.Drawing.Size(720, 25);
            this.pnlHie.TabIndex = 88;
            // 
            // Pb_Search_Hie
            // 
            this.Pb_Search_Hie.BackColor = System.Drawing.Color.FromArgb(244, 244, 244);
            this.Pb_Search_Hie.CssStyle = "border-radius:25px ";
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
            this.pnlParams.Controls.Add(this.lblTo);
            this.pnlParams.Controls.Add(this.cmbCompTo);
            this.pnlParams.Controls.Add(this.cmbCompFrom);
            this.pnlParams.Controls.Add(this.lblFrom);
            this.pnlParams.Controls.Add(this.lblComp);
            this.pnlParams.Controls.Add(this.cmbSeq);
            this.pnlParams.Controls.Add(this.lblSeq);
            this.pnlParams.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlParams.Location = new System.Drawing.Point(0, 43);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new System.Drawing.Size(808, 79);
            this.pnlParams.TabIndex = 1;
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
            // BtnPdfPrev
            // 
            this.BtnPdfPrev.AppearanceKey = "button-reports";
            this.BtnPdfPrev.Dock = Wisej.Web.DockStyle.Right;
            this.BtnPdfPrev.Location = new System.Drawing.Point(708, 5);
            this.BtnPdfPrev.Name = "BtnPdfPrev";
            this.BtnPdfPrev.Size = new System.Drawing.Size(85, 25);
            this.BtnPdfPrev.TabIndex = 4;
            this.BtnPdfPrev.Text = "Pre&view";
            this.BtnPdfPrev.Click += new System.EventHandler(this.BtnPdfPrev_Click);
            // 
            // BtnGenPdf
            // 
            this.BtnGenPdf.AppearanceKey = "button-reports";
            this.BtnGenPdf.Dock = Wisej.Web.DockStyle.Right;
            this.BtnGenPdf.Location = new System.Drawing.Point(620, 5);
            this.BtnGenPdf.Name = "BtnGenPdf";
            this.BtnGenPdf.Size = new System.Drawing.Size(85, 25);
            this.BtnGenPdf.TabIndex = 3;
            this.BtnGenPdf.Text = "&Generate";
            this.BtnGenPdf.Click += new System.EventHandler(this.BtnGenPdf_Click);
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(451, 47);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(15, 16);
            this.lblTo.TabIndex = 0;
            this.lblTo.Text = "To";
            // 
            // cmbCompTo
            // 
            this.cmbCompTo.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbCompTo.FormattingEnabled = true;
            this.cmbCompTo.Location = new System.Drawing.Point(478, 43);
            this.cmbCompTo.Name = "cmbCompTo";
            this.cmbCompTo.Size = new System.Drawing.Size(300, 25);
            this.cmbCompTo.TabIndex = 3;
            // 
            // cmbCompFrom
            // 
            this.cmbCompFrom.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbCompFrom.FormattingEnabled = true;
            this.cmbCompFrom.Location = new System.Drawing.Point(128, 43);
            this.cmbCompFrom.Name = "cmbCompFrom";
            this.cmbCompFrom.Size = new System.Drawing.Size(300, 25);
            this.cmbCompFrom.TabIndex = 2;
            this.cmbCompFrom.SelectedIndexChanged += new System.EventHandler(this.cmbCompFrom_SelectedIndexChanged);
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(763, 11);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(30, 16);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "From";
            this.lblFrom.Visible = false;
            // 
            // lblComp
            // 
            this.lblComp.Location = new System.Drawing.Point(15, 47);
            this.lblComp.Name = "lblComp";
            this.lblComp.Size = new System.Drawing.Size(101, 16);
            this.lblComp.TabIndex = 0;
            this.lblComp.Text = "Component From";
            // 
            // cmbSeq
            // 
            this.cmbSeq.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbSeq.FormattingEnabled = true;
            this.cmbSeq.Location = new System.Drawing.Point(128, 11);
            this.cmbSeq.Name = "cmbSeq";
            this.cmbSeq.Size = new System.Drawing.Size(300, 25);
            this.cmbSeq.TabIndex = 1;
            // 
            // lblSeq
            // 
            this.lblSeq.Location = new System.Drawing.Point(15, 15);
            this.lblSeq.Name = "lblSeq";
            this.lblSeq.Size = new System.Drawing.Size(56, 16);
            this.lblSeq.TabIndex = 0;
            this.lblSeq.Text = "Sequence";
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlParams);
            this.pnlCompleteForm.Controls.Add(this.pnlGenerate);
            this.pnlCompleteForm.Controls.Add(this.pnlHieFilter);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(808, 157);
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
            this.pnlHieFilter.Size = new System.Drawing.Size(808, 43);
            this.pnlHieFilter.TabIndex = 99;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(640, 0);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(15, 25);
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.Pb_Search_Hie);
            this.pnlFilter.Controls.Add(this.spacer2);
            this.pnlFilter.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlFilter.Location = new System.Drawing.Point(735, 9);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(64, 25);
            this.pnlFilter.TabIndex = 55;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(0, 0);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(15, 25);
            // 
            // pnlGenerate
            // 
            this.pnlGenerate.AppearanceKey = "panel-grdo";
            this.pnlGenerate.Controls.Add(this.btnSaveParameters);
            this.pnlGenerate.Controls.Add(this.spacer4);
            this.pnlGenerate.Controls.Add(this.BtnGenPdf);
            this.pnlGenerate.Controls.Add(this.btnGetParameters);
            this.pnlGenerate.Controls.Add(this.spacer3);
            this.pnlGenerate.Controls.Add(this.BtnPdfPrev);
            this.pnlGenerate.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlGenerate.Location = new System.Drawing.Point(0, 122);
            this.pnlGenerate.Name = "pnlGenerate";
            this.pnlGenerate.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlGenerate.Size = new System.Drawing.Size(808, 35);
            this.pnlGenerate.TabIndex = 2;
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Right;
            this.spacer3.Location = new System.Drawing.Point(705, 5);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(3, 25);
            // 
            // spacer4
            // 
            this.spacer4.Dock = Wisej.Web.DockStyle.Left;
            this.spacer4.Location = new System.Drawing.Point(125, 5);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(3, 25);
            // 
            // HSSB2102_TrackReport
            // 
            this.ClientSize = new System.Drawing.Size(808, 157);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSSB2102_TrackReport";
            this.Text = "HSSB2102_TrackReport";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "tlHelp";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.HSSB2102_TrackReport_ToolClick);
            this.pnlHie.ResumeLayout(false);
            this.pnlHie.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).EndInit();
            this.pnlParams.ResumeLayout(false);
            this.pnlParams.PerformLayout();
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlHieFilter.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            this.pnlGenerate.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComboBox CmbYear;
        private TextBox Txt_HieDesc;
        private Panel pnlHie;
        private PictureBox Pb_Search_Hie;
        private Panel pnlParams;
        private ComboBox cmbSeq;
        private Label lblSeq;
        private Label lblTo;
        private ComboBoxEx cmbCompTo;
        private ComboBoxEx cmbCompFrom;
        private Label lblFrom;
        private Label lblComp;
        private Button BtnPdfPrev;
        private Button BtnGenPdf;
        private Button btnSaveParameters;
        private Button btnGetParameters;
        private Panel pnlCompleteForm;
        private Panel pnlHieFilter;
        private Spacer spacer1;
        private Panel pnlFilter;
        private Spacer spacer2;
        private Panel pnlGenerate;
        private Spacer spacer3;
        private Spacer spacer4;
    }
}