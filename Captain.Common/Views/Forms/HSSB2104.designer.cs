using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class HSSB2104
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSSB2104));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.btnGeneratePdf = new Wisej.Web.Button();
            this.btnPdfPreview = new Wisej.Web.Button();
            this.btnSaveParameters = new Wisej.Web.Button();
            this.btnGetParameters = new Wisej.Web.Button();
            this.pnlButtons = new Wisej.Web.Panel();
            this.spacer5 = new Wisej.Web.Spacer();
            this.chkbExcel = new Wisej.Web.CheckBox();
            this.spacer4 = new Wisej.Web.Spacer();
            this.spacer3 = new Wisej.Web.Spacer();
            this.panel2 = new Wisej.Web.Panel();
            this.pnlDte = new Wisej.Web.Panel();
            this.label1 = new Wisej.Web.Label();
            this.lblAsOfDate = new Wisej.Web.Label();
            this.dtpAsOfDate = new Wisej.Web.DateTimePicker();
            this.pnlFundingSource = new Wisej.Web.Panel();
            this.rbSelected = new Wisej.Web.RadioButton();
            this.rbAll = new Wisej.Web.RadioButton();
            this.lblFundingSource = new Wisej.Web.Label();
            this.pnlReportType = new Wisej.Web.Panel();
            this.rbSiteSummary = new Wisej.Web.RadioButton();
            this.rbRoomDetail = new Wisej.Web.RadioButton();
            this.rbFundDetailByRoom = new Wisej.Web.RadioButton();
            this.lblReportType = new Wisej.Web.Label();
            this.pnlSite = new Wisej.Web.Panel();
            this.lblSite = new Wisej.Web.Label();
            this.cmbSite = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.CmbYear = new Wisej.Web.ComboBox();
            this.Txt_HieDesc = new Wisej.Web.TextBox();
            this.pnlHie = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.Pb_Search_Hie = new Wisej.Web.PictureBox();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlHieFilter = new Wisej.Web.Panel();
            this.pnlFilter = new Wisej.Web.Panel();
            this.spacer2 = new Wisej.Web.Spacer();
            this.pnlButtons.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlDte.SuspendLayout();
            this.pnlFundingSource.SuspendLayout();
            this.pnlReportType.SuspendLayout();
            this.pnlSite.SuspendLayout();
            this.pnlHie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).BeginInit();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlHieFilter.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGeneratePdf
            // 
            this.btnGeneratePdf.AppearanceKey = "button-reports";
            this.btnGeneratePdf.Dock = Wisej.Web.DockStyle.Right;
            this.btnGeneratePdf.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnGeneratePdf.Location = new System.Drawing.Point(534, 5);
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
            this.btnPdfPreview.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnPdfPreview.Location = new System.Drawing.Point(622, 5);
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
            this.btnSaveParameters.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
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
            this.btnGetParameters.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnGetParameters.Location = new System.Drawing.Point(15, 5);
            this.btnGetParameters.Name = "btnGetParameters";
            this.btnGetParameters.Size = new System.Drawing.Size(110, 25);
            this.btnGetParameters.TabIndex = 1;
            this.btnGetParameters.Text = "Get &Parameters";
            this.btnGetParameters.Click += new System.EventHandler(this.btnGetParameters_Click);
            // 
            // pnlButtons
            // 
            this.pnlButtons.AppearanceKey = "panel-grdo";
            this.pnlButtons.Controls.Add(this.btnSaveParameters);
            this.pnlButtons.Controls.Add(this.spacer5);
            this.pnlButtons.Controls.Add(this.chkbExcel);
            this.pnlButtons.Controls.Add(this.spacer4);
            this.pnlButtons.Controls.Add(this.btnGeneratePdf);
            this.pnlButtons.Controls.Add(this.spacer3);
            this.pnlButtons.Controls.Add(this.btnPdfPreview);
            this.pnlButtons.Controls.Add(this.btnGetParameters);
            this.pnlButtons.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 180);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlButtons.Size = new System.Drawing.Size(722, 35);
            this.pnlButtons.TabIndex = 2;
            // 
            // spacer5
            // 
            this.spacer5.Dock = Wisej.Web.DockStyle.Left;
            this.spacer5.Location = new System.Drawing.Point(125, 5);
            this.spacer5.Name = "spacer5";
            this.spacer5.Size = new System.Drawing.Size(3, 25);
            // 
            // chkbExcel
            // 
            this.chkbExcel.Dock = Wisej.Web.DockStyle.Right;
            this.chkbExcel.Location = new System.Drawing.Point(335, 5);
            this.chkbExcel.Name = "chkbExcel";
            this.chkbExcel.Size = new System.Drawing.Size(115, 25);
            this.chkbExcel.TabIndex = 3;
            this.chkbExcel.Text = "Generate Excel";
            // 
            // spacer4
            // 
            this.spacer4.Dock = Wisej.Web.DockStyle.Right;
            this.spacer4.Location = new System.Drawing.Point(450, 5);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(84, 25);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Right;
            this.spacer3.Location = new System.Drawing.Point(619, 5);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(3, 25);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pnlDte);
            this.panel2.Controls.Add(this.pnlFundingSource);
            this.panel2.Controls.Add(this.pnlReportType);
            this.panel2.Controls.Add(this.pnlSite);
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 43);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(722, 137);
            this.panel2.TabIndex = 1;
            // 
            // pnlDte
            // 
            this.pnlDte.Controls.Add(this.label1);
            this.pnlDte.Controls.Add(this.lblAsOfDate);
            this.pnlDte.Controls.Add(this.dtpAsOfDate);
            this.pnlDte.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlDte.Location = new System.Drawing.Point(0, 101);
            this.pnlDte.Name = "pnlDte";
            this.pnlDte.Size = new System.Drawing.Size(722, 36);
            this.pnlDte.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(76, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 11);
            this.label1.TabIndex = 3;
            this.label1.Text = "*";
            // 
            // lblAsOfDate
            // 
            this.lblAsOfDate.Location = new System.Drawing.Point(15, 7);
            this.lblAsOfDate.Name = "lblAsOfDate";
            this.lblAsOfDate.Size = new System.Drawing.Size(62, 16);
            this.lblAsOfDate.TabIndex = 3;
            this.lblAsOfDate.Text = "As Of Date";
            // 
            // dtpAsOfDate
            // 
            this.dtpAsOfDate.AutoSize = false;
            this.dtpAsOfDate.CustomFormat = "MM/dd/yyyy";
            this.dtpAsOfDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpAsOfDate.Location = new System.Drawing.Point(123, 3);
            this.dtpAsOfDate.Name = "dtpAsOfDate";
            this.dtpAsOfDate.ShowCheckBox = true;
            this.dtpAsOfDate.ShowToolTips = false;
            this.dtpAsOfDate.Size = new System.Drawing.Size(116, 25);
            this.dtpAsOfDate.TabIndex = 1;
            // 
            // pnlFundingSource
            // 
            this.pnlFundingSource.Controls.Add(this.rbSelected);
            this.pnlFundingSource.Controls.Add(this.rbAll);
            this.pnlFundingSource.Controls.Add(this.lblFundingSource);
            this.pnlFundingSource.Dock = Wisej.Web.DockStyle.Top;
            this.pnlFundingSource.Location = new System.Drawing.Point(0, 70);
            this.pnlFundingSource.Name = "pnlFundingSource";
            this.pnlFundingSource.Size = new System.Drawing.Size(722, 31);
            this.pnlFundingSource.TabIndex = 3;
            // 
            // rbSelected
            // 
            this.rbSelected.AutoSize = false;
            this.rbSelected.Enabled = false;
            this.rbSelected.Location = new System.Drawing.Point(187, 4);
            this.rbSelected.Name = "rbSelected";
            this.rbSelected.Size = new System.Drawing.Size(76, 21);
            this.rbSelected.TabIndex = 2;
            this.rbSelected.Text = "Selected";
            this.rbSelected.Click += new System.EventHandler(this.rbSelected_Click);
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = false;
            this.rbAll.Checked = true;
            this.rbAll.Enabled = false;
            this.rbAll.Location = new System.Drawing.Point(120, 4);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(42, 21);
            this.rbAll.TabIndex = 1;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "All";
            this.rbAll.CheckedChanged += new System.EventHandler(this.rbAll_CheckedChanged);
            // 
            // lblFundingSource
            // 
            this.lblFundingSource.Location = new System.Drawing.Point(15, 7);
            this.lblFundingSource.Name = "lblFundingSource";
            this.lblFundingSource.Size = new System.Drawing.Size(90, 16);
            this.lblFundingSource.TabIndex = 2;
            this.lblFundingSource.Text = "Funding Source";
            // 
            // pnlReportType
            // 
            this.pnlReportType.Controls.Add(this.rbSiteSummary);
            this.pnlReportType.Controls.Add(this.rbRoomDetail);
            this.pnlReportType.Controls.Add(this.rbFundDetailByRoom);
            this.pnlReportType.Controls.Add(this.lblReportType);
            this.pnlReportType.Dock = Wisej.Web.DockStyle.Top;
            this.pnlReportType.Location = new System.Drawing.Point(0, 39);
            this.pnlReportType.Name = "pnlReportType";
            this.pnlReportType.Size = new System.Drawing.Size(722, 31);
            this.pnlReportType.TabIndex = 2;
            // 
            // rbSiteSummary
            // 
            this.rbSiteSummary.AutoSize = false;
            this.rbSiteSummary.Checked = true;
            this.rbSiteSummary.Location = new System.Drawing.Point(120, 4);
            this.rbSiteSummary.Name = "rbSiteSummary";
            this.rbSiteSummary.Size = new System.Drawing.Size(107, 21);
            this.rbSiteSummary.TabIndex = 1;
            this.rbSiteSummary.TabStop = true;
            this.rbSiteSummary.Text = "Site Summary";
            this.rbSiteSummary.Click += new System.EventHandler(this.rbRoomDetail_Click);
            // 
            // rbRoomDetail
            // 
            this.rbRoomDetail.AutoSize = false;
            this.rbRoomDetail.Location = new System.Drawing.Point(245, 4);
            this.rbRoomDetail.Name = "rbRoomDetail";
            this.rbRoomDetail.Size = new System.Drawing.Size(98, 21);
            this.rbRoomDetail.TabIndex = 2;
            this.rbRoomDetail.Text = "Room Detail";
            this.rbRoomDetail.Click += new System.EventHandler(this.rbRoomDetail_Click);
            // 
            // rbFundDetailByRoom
            // 
            this.rbFundDetailByRoom.AutoSize = false;
            this.rbFundDetailByRoom.Location = new System.Drawing.Point(360, 4);
            this.rbFundDetailByRoom.Name = "rbFundDetailByRoom";
            this.rbFundDetailByRoom.Size = new System.Drawing.Size(146, 21);
            this.rbFundDetailByRoom.TabIndex = 3;
            this.rbFundDetailByRoom.Text = "Fund Detail By Room";
            this.rbFundDetailByRoom.Click += new System.EventHandler(this.rbFundDetailByRoom_Click);
            // 
            // lblReportType
            // 
            this.lblReportType.Location = new System.Drawing.Point(15, 7);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new System.Drawing.Size(70, 16);
            this.lblReportType.TabIndex = 1;
            this.lblReportType.Text = "Report Type";
            // 
            // pnlSite
            // 
            this.pnlSite.Controls.Add(this.lblSite);
            this.pnlSite.Controls.Add(this.cmbSite);
            this.pnlSite.Dock = Wisej.Web.DockStyle.Top;
            this.pnlSite.Location = new System.Drawing.Point(0, 0);
            this.pnlSite.Name = "pnlSite";
            this.pnlSite.Size = new System.Drawing.Size(722, 39);
            this.pnlSite.TabIndex = 1;
            // 
            // lblSite
            // 
            this.lblSite.Location = new System.Drawing.Point(15, 15);
            this.lblSite.Name = "lblSite";
            this.lblSite.Size = new System.Drawing.Size(22, 16);
            this.lblSite.TabIndex = 0;
            this.lblSite.Text = "Site";
            // 
            // cmbSite
            // 
            this.cmbSite.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbSite.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbSite.FormattingEnabled = true;
            this.cmbSite.Location = new System.Drawing.Point(123, 11);
            this.cmbSite.Name = "cmbSite";
            this.cmbSite.Size = new System.Drawing.Size(253, 25);
            this.cmbSite.TabIndex = 1;
            // 
            // CmbYear
            // 
            this.CmbYear.Dock = Wisej.Web.DockStyle.Left;
            this.CmbYear.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbYear.FormattingEnabled = true;
            this.CmbYear.Location = new System.Drawing.Point(570, 0);
            this.CmbYear.Name = "CmbYear";
            this.CmbYear.Size = new System.Drawing.Size(65, 25);
            this.CmbYear.TabIndex = 55;
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
            this.Txt_HieDesc.Size = new System.Drawing.Size(555, 25);
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
            this.pnlHie.Size = new System.Drawing.Size(635, 25);
            this.pnlHie.TabIndex = 88;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(555, 0);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(15, 25);
            // 
            // Pb_Search_Hie
            // 
            this.Pb_Search_Hie.BackColor = System.Drawing.Color.FromArgb(244, 244, 244);
            this.Pb_Search_Hie.CssStyle = "border-radius:25px";
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
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.panel2);
            this.pnlCompleteForm.Controls.Add(this.pnlHieFilter);
            this.pnlCompleteForm.Controls.Add(this.pnlButtons);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(722, 215);
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
            this.pnlHieFilter.Size = new System.Drawing.Size(722, 43);
            this.pnlHieFilter.TabIndex = 99;
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.Pb_Search_Hie);
            this.pnlFilter.Controls.Add(this.spacer2);
            this.pnlFilter.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlFilter.Location = new System.Drawing.Point(650, 9);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(63, 25);
            this.pnlFilter.TabIndex = 44;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(0, 0);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(15, 25);
            // 
            // HSSB2104
            // 
            this.ClientSize = new System.Drawing.Size(722, 215);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSSB2104";
            this.StartPosition = Wisej.Web.FormStartPosition.CenterScreen;
            this.Text = "HSSB2104";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "tlHelp";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.HSSB2104_ToolClick);
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.pnlDte.ResumeLayout(false);
            this.pnlFundingSource.ResumeLayout(false);
            this.pnlReportType.ResumeLayout(false);
            this.pnlSite.ResumeLayout(false);
            this.pnlSite.PerformLayout();
            this.pnlHie.ResumeLayout(false);
            this.pnlHie.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).EndInit();
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlHieFilter.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnGeneratePdf;
        private Button btnPdfPreview;
        private Button btnSaveParameters;
        private Button btnGetParameters;
        private Panel pnlButtons;
        private Panel panel2;
        private Label lblAsOfDate;
        private Label lblFundingSource;
        private Label lblReportType;
        private Label lblSite;
        private Panel pnlFundingSource;
        private RadioButton rbSelected;
        private RadioButton rbAll;
        private Panel pnlReportType;
        private RadioButton rbSiteSummary;
        private RadioButton rbRoomDetail;
        private RadioButton rbFundDetailByRoom;
        private DateTimePicker dtpAsOfDate;
        private ComboBoxEx cmbSite;
        private ComboBox CmbYear;
        private TextBox Txt_HieDesc;
        private Panel pnlHie;
        private PictureBox Pb_Search_Hie;
        private CheckBox chkbExcel;
        private Label label1;
        private Panel pnlCompleteForm;
        private Panel pnlHieFilter;
        private Spacer spacer1;
        private Panel pnlFilter;
        private Spacer spacer2;
        private Spacer spacer3;
        private Spacer spacer4;
        private Spacer spacer5;
        private Panel pnlSite;
        private Panel pnlDte;
    }
}