using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class APPTB001_Report
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

        #region Wisej Web Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(APPTB001_Report));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.chkbExcel = new Wisej.Web.CheckBox();
            this.btnGeneratePdf = new Wisej.Web.Button();
            this.btnPdfPreview = new Wisej.Web.Button();
            this.btnSaveParameters = new Wisej.Web.Button();
            this.btnGetParameters = new Wisej.Web.Button();
            this.panel9 = new Wisej.Web.Panel();
            this.spacer2 = new Wisej.Web.Spacer();
            this.spacer1 = new Wisej.Web.Spacer();
            this.rbAll = new Wisej.Web.RadioButton();
            this.rbFuture = new Wisej.Web.RadioButton();
            this.dtpStartDate = new Wisej.Web.DateTimePicker();
            this.dtpEndDate = new Wisej.Web.DateTimePicker();
            this.panel1 = new Wisej.Web.Panel();
            this.lblEndDate = new Wisej.Web.Label();
            this.chkbSummary = new Wisej.Web.CheckBox();
            this.rbDateRange = new Wisej.Web.RadioButton();
            this.cmbSite = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.lblSite = new Wisej.Web.Label();
            this.CmbYear = new Wisej.Web.ComboBox();
            this.Txt_HieDesc = new Wisej.Web.TextBox();
            this.panel4 = new Wisej.Web.Panel();
            this.spacer4 = new Wisej.Web.Spacer();
            this.Pb_Search_Hie = new Wisej.Web.PictureBox();
            this.pnlHieFil = new Wisej.Web.Panel();
            this.panel2 = new Wisej.Web.Panel();
            this.spacer3 = new Wisej.Web.Spacer();
            this.panel9.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).BeginInit();
            this.pnlHieFil.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkbExcel
            // 
            this.chkbExcel.Location = new System.Drawing.Point(400, 7);
            this.chkbExcel.Name = "chkbExcel";
            this.chkbExcel.Size = new System.Drawing.Size(115, 21);
            this.chkbExcel.TabIndex = 28;
            this.chkbExcel.Text = "Generate Excel";
            this.chkbExcel.Visible = false;
            // 
            // btnGeneratePdf
            // 
            this.btnGeneratePdf.AppearanceKey = "button-reports";
            this.btnGeneratePdf.Dock = Wisej.Web.DockStyle.Right;
            this.btnGeneratePdf.Location = new System.Drawing.Point(672, 5);
            this.btnGeneratePdf.Name = "btnGeneratePdf";
            this.btnGeneratePdf.Size = new System.Drawing.Size(80, 25);
            this.btnGeneratePdf.TabIndex = 3;
            this.btnGeneratePdf.Text = "G&enerate";
            this.btnGeneratePdf.Click += new System.EventHandler(this.btnGeneratePdf_Click);
            // 
            // btnPdfPreview
            // 
            this.btnPdfPreview.AppearanceKey = "button-reports";
            this.btnPdfPreview.Dock = Wisej.Web.DockStyle.Right;
            this.btnPdfPreview.Location = new System.Drawing.Point(757, 5);
            this.btnPdfPreview.Name = "btnPdfPreview";
            this.btnPdfPreview.Size = new System.Drawing.Size(80, 25);
            this.btnPdfPreview.TabIndex = 4;
            this.btnPdfPreview.Text = "Pre&view";
            this.btnPdfPreview.Click += new System.EventHandler(this.btnPdfPreview_Click);
            // 
            // btnSaveParameters
            // 
            this.btnSaveParameters.AppearanceKey = "button-reports";
            this.btnSaveParameters.Dock = Wisej.Web.DockStyle.Left;
            this.btnSaveParameters.Location = new System.Drawing.Point(125, 5);
            this.btnSaveParameters.Name = "btnSaveParameters";
            this.btnSaveParameters.Size = new System.Drawing.Size(105, 25);
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
            this.btnGetParameters.Size = new System.Drawing.Size(105, 25);
            this.btnGetParameters.TabIndex = 1;
            this.btnGetParameters.Text = "&Get Parameters";
            this.btnGetParameters.Click += new System.EventHandler(this.btnGetParameters_Click);
            // 
            // panel9
            // 
            this.panel9.AppearanceKey = "panel-grdo";
            this.panel9.Controls.Add(this.btnSaveParameters);
            this.panel9.Controls.Add(this.spacer2);
            this.panel9.Controls.Add(this.btnGeneratePdf);
            this.panel9.Controls.Add(this.spacer1);
            this.panel9.Controls.Add(this.chkbExcel);
            this.panel9.Controls.Add(this.btnPdfPreview);
            this.panel9.Controls.Add(this.btnGetParameters);
            this.panel9.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel9.Location = new System.Drawing.Point(0, 191);
            this.panel9.Name = "panel9";
            this.panel9.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.panel9.Size = new System.Drawing.Size(852, 35);
            this.panel9.TabIndex = 2;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(120, 5);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(5, 25);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(752, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 25);
            // 
            // rbAll
            // 
            this.rbAll.Location = new System.Drawing.Point(115, 72);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(81, 21);
            this.rbAll.TabIndex = 5;
            this.rbAll.Text = "All Dates";
            this.rbAll.CheckedChanged += new System.EventHandler(this.rbFuture_CheckedChanged);
            // 
            // rbFuture
            // 
            this.rbFuture.AutoSize = false;
            this.rbFuture.Location = new System.Drawing.Point(115, 96);
            this.rbFuture.Name = "rbFuture";
            this.rbFuture.Size = new System.Drawing.Size(189, 24);
            this.rbFuture.TabIndex = 6;
            this.rbFuture.Text = "Today Through Future Dates";
            this.rbFuture.CheckedChanged += new System.EventHandler(this.rbFuture_CheckedChanged);
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Checked = false;
            this.dtpStartDate.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(245, 46);
            this.dtpStartDate.MinimumSize = new System.Drawing.Size(0, 25);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.ShowToolTips = false;
            this.dtpStartDate.Size = new System.Drawing.Size(95, 25);
            this.dtpStartDate.TabIndex = 3;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Checked = false;
            this.dtpEndDate.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(388, 47);
            this.dtpEndDate.MinimumSize = new System.Drawing.Size(0, 25);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.ShowToolTips = false;
            this.dtpEndDate.Size = new System.Drawing.Size(95, 25);
            this.dtpEndDate.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblEndDate);
            this.panel1.Controls.Add(this.chkbSummary);
            this.panel1.Controls.Add(this.rbDateRange);
            this.panel1.Controls.Add(this.cmbSite);
            this.panel1.Controls.Add(this.lblSite);
            this.panel1.Controls.Add(this.rbAll);
            this.panel1.Controls.Add(this.rbFuture);
            this.panel1.Controls.Add(this.dtpStartDate);
            this.panel1.Controls.Add(this.dtpEndDate);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 43);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(852, 148);
            this.panel1.TabIndex = 1;
            // 
            // lblEndDate
            // 
            this.lblEndDate.Location = new System.Drawing.Point(368, 51);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(16, 14);
            this.lblEndDate.TabIndex = 9;
            this.lblEndDate.Text = "To";
            // 
            // chkbSummary
            // 
            this.chkbSummary.Location = new System.Drawing.Point(115, 122);
            this.chkbSummary.Name = "chkbSummary";
            this.chkbSummary.Size = new System.Drawing.Size(84, 21);
            this.chkbSummary.TabIndex = 7;
            this.chkbSummary.Text = "Summary";
            // 
            // rbDateRange
            // 
            this.rbDateRange.AutoSize = false;
            this.rbDateRange.Checked = true;
            this.rbDateRange.Location = new System.Drawing.Point(115, 46);
            this.rbDateRange.Name = "rbDateRange";
            this.rbDateRange.Size = new System.Drawing.Size(125, 21);
            this.rbDateRange.TabIndex = 2;
            this.rbDateRange.TabStop = true;
            this.rbDateRange.Text = "Date Range From";
            this.rbDateRange.CheckedChanged += new System.EventHandler(this.rbFuture_CheckedChanged);
            // 
            // cmbSite
            // 
            this.cmbSite.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbSite.FormattingEnabled = true;
            this.cmbSite.Location = new System.Drawing.Point(120, 10);
            this.cmbSite.Name = "cmbSite";
            this.cmbSite.Size = new System.Drawing.Size(363, 25);
            this.cmbSite.TabIndex = 1;
            // 
            // lblSite
            // 
            this.lblSite.AutoSize = true;
            this.lblSite.Location = new System.Drawing.Point(83, 15);
            this.lblSite.Name = "lblSite";
            this.lblSite.Size = new System.Drawing.Size(25, 14);
            this.lblSite.TabIndex = 0;
            this.lblSite.Text = "Site";
            // 
            // CmbYear
            // 
            this.CmbYear.Dock = Wisej.Web.DockStyle.Left;
            this.CmbYear.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbYear.FormattingEnabled = true;
            this.CmbYear.Location = new System.Drawing.Point(709, 0);
            this.CmbYear.Name = "CmbYear";
            this.CmbYear.Size = new System.Drawing.Size(63, 25);
            this.CmbYear.TabIndex = 6;
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
            this.Txt_HieDesc.Size = new System.Drawing.Size(689, 25);
            this.Txt_HieDesc.TabIndex = 5;
            this.Txt_HieDesc.TabStop = false;
            this.Txt_HieDesc.TextAlign = Wisej.Web.HorizontalAlignment.Center;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(11, 70, 117);
            this.panel4.Controls.Add(this.CmbYear);
            this.panel4.Controls.Add(this.spacer4);
            this.panel4.Controls.Add(this.Txt_HieDesc);
            this.panel4.Dock = Wisej.Web.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(15, 9);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(777, 25);
            this.panel4.TabIndex = 1;
            // 
            // spacer4
            // 
            this.spacer4.Dock = Wisej.Web.DockStyle.Left;
            this.spacer4.Location = new System.Drawing.Point(689, 0);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(20, 25);
            // 
            // Pb_Search_Hie
            // 
            this.Pb_Search_Hie.BackColor = System.Drawing.Color.FromArgb(244, 244, 244);
            this.Pb_Search_Hie.CssStyle = "border-radius:25px";
            this.Pb_Search_Hie.Cursor = Wisej.Web.Cursors.Hand;
            this.Pb_Search_Hie.Dock = Wisej.Web.DockStyle.Left;
            this.Pb_Search_Hie.ForeColor = System.Drawing.Color.FromName("@windowText");
            this.Pb_Search_Hie.ImageSource = "captain-filter";
            this.Pb_Search_Hie.Location = new System.Drawing.Point(20, 0);
            this.Pb_Search_Hie.Name = "Pb_Search_Hie";
            this.Pb_Search_Hie.Padding = new Wisej.Web.Padding(4, 5, 4, 4);
            this.Pb_Search_Hie.Size = new System.Drawing.Size(25, 25);
            this.Pb_Search_Hie.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.Pb_Search_Hie.ToolTipText = "Select Hierarchy";
            this.Pb_Search_Hie.Click += new System.EventHandler(this.Pb_Search_Hie_Click);
            // 
            // pnlHieFil
            // 
            this.pnlHieFil.BackColor = System.Drawing.Color.FromArgb(11, 70, 117);
            this.pnlHieFil.Controls.Add(this.panel2);
            this.pnlHieFil.Controls.Add(this.panel4);
            this.pnlHieFil.Dock = Wisej.Web.DockStyle.Top;
            this.pnlHieFil.Location = new System.Drawing.Point(0, 0);
            this.pnlHieFil.Name = "pnlHieFil";
            this.pnlHieFil.Padding = new Wisej.Web.Padding(15, 9, 9, 9);
            this.pnlHieFil.Size = new System.Drawing.Size(852, 43);
            this.pnlHieFil.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(11, 70, 117);
            this.panel2.Controls.Add(this.Pb_Search_Hie);
            this.panel2.Controls.Add(this.spacer3);
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(792, 9);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(51, 25);
            this.panel2.TabIndex = 2;
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Left;
            this.spacer3.Location = new System.Drawing.Point(0, 0);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(20, 25);
            // 
            // APPTB001_Report
            // 
            this.ClientSize = new System.Drawing.Size(852, 226);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlHieFil);
            this.Controls.Add(this.panel9);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "APPTB001_Report";
            this.Text = "Appointment Schedule Report ";
            componentTool1.ImageSource = "icon-help";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.APPTB001_Report_ToolClick);
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).EndInit();
            this.pnlHieFil.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CheckBox chkbExcel;
        private Button btnGeneratePdf;
        private Button btnPdfPreview;
        private Button btnSaveParameters;
        private Button btnGetParameters;
        private Panel panel9;
        private RadioButton rbAll;
        private RadioButton rbFuture;
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpEndDate;
        private Panel panel1;
        private Label lblSite;
        private ComboBox CmbYear;
        private TextBox Txt_HieDesc;
        private Panel panel4;
        private RadioButton rbDateRange;
        private ComboBoxEx cmbSite;
        private CheckBox chkbSummary;
        private Spacer spacer2;
        private Spacer spacer1;
        private Panel pnlHieFil;
        private PictureBox Pb_Search_Hie;
        private Label lblEndDate;
        private Panel panel2;
        private Spacer spacer4;
        private Spacer spacer3;
    }
}