using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class HSSB0118_ReportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSSB0118_ReportForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.pnlParams = new Wisej.Web.Panel();
            this.pnlSkip = new Wisej.Web.Panel();
            this.chkbSkip = new Wisej.Web.CheckBox();
            this.pnlSortBy = new Wisej.Web.Panel();
            this.lblSort = new Wisej.Web.Label();
            this.cmbSort = new Wisej.Web.ComboBox();
            this.pnlRouteNo = new Wisej.Web.Panel();
            this.lblRouteNo = new Wisej.Web.Label();
            this.cmbRoute = new Wisej.Web.ComboBox();
            this.pnlBusNo = new Wisej.Web.Panel();
            this.lblBusNo = new Wisej.Web.Label();
            this.cmbBus = new Wisej.Web.ComboBox();
            this.pnlRepType = new Wisej.Web.Panel();
            this.lblRepType = new Wisej.Web.Label();
            this.rbMaster = new Wisej.Web.RadioButton();
            this.rbChild = new Wisej.Web.RadioButton();
            this.Pb_Search_Hie = new Wisej.Web.PictureBox();
            this.CmbYear = new Wisej.Web.ComboBox();
            this.Txt_HieDesc = new Wisej.Web.TextBox();
            this.pnlHie = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnSaveParameters = new Wisej.Web.Button();
            this.btnGetParameters = new Wisej.Web.Button();
            this.BtnGenPdf = new Wisej.Web.Button();
            this.BtnPdfPrev = new Wisej.Web.Button();
            this.pnlGenerate = new Wisej.Web.Panel();
            this.spacer4 = new Wisej.Web.Spacer();
            this.spacer3 = new Wisej.Web.Spacer();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlHieFilter = new Wisej.Web.Panel();
            this.pnlFilter = new Wisej.Web.Panel();
            this.spacer2 = new Wisej.Web.Spacer();
            this.pnlParams.SuspendLayout();
            this.pnlSkip.SuspendLayout();
            this.pnlSortBy.SuspendLayout();
            this.pnlRouteNo.SuspendLayout();
            this.pnlBusNo.SuspendLayout();
            this.pnlRepType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).BeginInit();
            this.pnlHie.SuspendLayout();
            this.pnlGenerate.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlHieFilter.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlParams
            // 
            this.pnlParams.Controls.Add(this.pnlSkip);
            this.pnlParams.Controls.Add(this.pnlSortBy);
            this.pnlParams.Controls.Add(this.pnlRouteNo);
            this.pnlParams.Controls.Add(this.pnlBusNo);
            this.pnlParams.Controls.Add(this.pnlRepType);
            this.pnlParams.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlParams.Location = new System.Drawing.Point(0, 43);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new System.Drawing.Size(750, 159);
            this.pnlParams.TabIndex = 1;
            // 
            // pnlSkip
            // 
            this.pnlSkip.Controls.Add(this.chkbSkip);
            this.pnlSkip.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlSkip.Location = new System.Drawing.Point(0, 128);
            this.pnlSkip.Name = "pnlSkip";
            this.pnlSkip.Size = new System.Drawing.Size(750, 31);
            this.pnlSkip.TabIndex = 5;
            this.pnlSkip.Visible = false;
            // 
            // chkbSkip
            // 
            this.chkbSkip.AutoSize = false;
            this.chkbSkip.Enabled = false;
            this.chkbSkip.Location = new System.Drawing.Point(112, 2);
            this.chkbSkip.Name = "chkbSkip";
            this.chkbSkip.Size = new System.Drawing.Size(78, 21);
            this.chkbSkip.TabIndex = 1;
            this.chkbSkip.Text = "Skip Line";
            // 
            // pnlSortBy
            // 
            this.pnlSortBy.Controls.Add(this.lblSort);
            this.pnlSortBy.Controls.Add(this.cmbSort);
            this.pnlSortBy.Dock = Wisej.Web.DockStyle.Top;
            this.pnlSortBy.Location = new System.Drawing.Point(0, 97);
            this.pnlSortBy.Name = "pnlSortBy";
            this.pnlSortBy.Size = new System.Drawing.Size(750, 31);
            this.pnlSortBy.TabIndex = 4;
            this.pnlSortBy.Visible = false;
            // 
            // lblSort
            // 
            this.lblSort.Location = new System.Drawing.Point(15, 7);
            this.lblSort.Name = "lblSort";
            this.lblSort.Size = new System.Drawing.Size(41, 16);
            this.lblSort.TabIndex = 0;
            this.lblSort.Text = "Sort by";
            // 
            // cmbSort
            // 
            this.cmbSort.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbSort.Enabled = false;
            this.cmbSort.FormattingEnabled = true;
            this.cmbSort.Location = new System.Drawing.Point(115, 3);
            this.cmbSort.Name = "cmbSort";
            this.cmbSort.Size = new System.Drawing.Size(149, 25);
            this.cmbSort.TabIndex = 1;
            // 
            // pnlRouteNo
            // 
            this.pnlRouteNo.Controls.Add(this.lblRouteNo);
            this.pnlRouteNo.Controls.Add(this.cmbRoute);
            this.pnlRouteNo.Dock = Wisej.Web.DockStyle.Top;
            this.pnlRouteNo.Location = new System.Drawing.Point(0, 66);
            this.pnlRouteNo.Name = "pnlRouteNo";
            this.pnlRouteNo.Size = new System.Drawing.Size(750, 31);
            this.pnlRouteNo.TabIndex = 3;
            // 
            // lblRouteNo
            // 
            this.lblRouteNo.Location = new System.Drawing.Point(15, 7);
            this.lblRouteNo.Name = "lblRouteNo";
            this.lblRouteNo.Size = new System.Drawing.Size(85, 16);
            this.lblRouteNo.TabIndex = 0;
            this.lblRouteNo.Text = "Route Number";
            // 
            // cmbRoute
            // 
            this.cmbRoute.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbRoute.FormattingEnabled = true;
            this.cmbRoute.Location = new System.Drawing.Point(115, 3);
            this.cmbRoute.Name = "cmbRoute";
            this.cmbRoute.Size = new System.Drawing.Size(365, 25);
            this.cmbRoute.TabIndex = 1;
            // 
            // pnlBusNo
            // 
            this.pnlBusNo.Controls.Add(this.lblBusNo);
            this.pnlBusNo.Controls.Add(this.cmbBus);
            this.pnlBusNo.Dock = Wisej.Web.DockStyle.Top;
            this.pnlBusNo.Location = new System.Drawing.Point(0, 35);
            this.pnlBusNo.Name = "pnlBusNo";
            this.pnlBusNo.Size = new System.Drawing.Size(750, 31);
            this.pnlBusNo.TabIndex = 2;
            // 
            // lblBusNo
            // 
            this.lblBusNo.Location = new System.Drawing.Point(15, 7);
            this.lblBusNo.Name = "lblBusNo";
            this.lblBusNo.Size = new System.Drawing.Size(72, 16);
            this.lblBusNo.TabIndex = 0;
            this.lblBusNo.Text = "Bus Number";
            // 
            // cmbBus
            // 
            this.cmbBus.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbBus.FormattingEnabled = true;
            this.cmbBus.Location = new System.Drawing.Point(115, 3);
            this.cmbBus.Name = "cmbBus";
            this.cmbBus.Size = new System.Drawing.Size(365, 25);
            this.cmbBus.TabIndex = 1;
            this.cmbBus.SelectedIndexChanged += new System.EventHandler(this.cmbBus_SelectedIndexChanged);
            // 
            // pnlRepType
            // 
            this.pnlRepType.Controls.Add(this.lblRepType);
            this.pnlRepType.Controls.Add(this.rbMaster);
            this.pnlRepType.Controls.Add(this.rbChild);
            this.pnlRepType.Dock = Wisej.Web.DockStyle.Top;
            this.pnlRepType.Location = new System.Drawing.Point(0, 0);
            this.pnlRepType.Name = "pnlRepType";
            this.pnlRepType.Size = new System.Drawing.Size(750, 35);
            this.pnlRepType.TabIndex = 1;
            // 
            // lblRepType
            // 
            this.lblRepType.Location = new System.Drawing.Point(15, 15);
            this.lblRepType.Name = "lblRepType";
            this.lblRepType.Size = new System.Drawing.Size(70, 16);
            this.lblRepType.TabIndex = 0;
            this.lblRepType.Text = "Report Type";
            // 
            // rbMaster
            // 
            this.rbMaster.AutoSize = false;
            this.rbMaster.Checked = true;
            this.rbMaster.Location = new System.Drawing.Point(112, 12);
            this.rbMaster.Name = "rbMaster";
            this.rbMaster.Size = new System.Drawing.Size(69, 21);
            this.rbMaster.TabIndex = 1;
            this.rbMaster.TabStop = true;
            this.rbMaster.Text = "Master";
            // 
            // rbChild
            // 
            this.rbChild.AutoSize = false;
            this.rbChild.Location = new System.Drawing.Point(193, 12);
            this.rbChild.Name = "rbChild";
            this.rbChild.Size = new System.Drawing.Size(171, 21);
            this.rbChild.TabIndex = 2;
            this.rbChild.Text = "Condensed with Children";
            this.rbChild.CheckedChanged += new System.EventHandler(this.rbChild_CheckedChanged);
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
            // CmbYear
            // 
            this.CmbYear.Dock = Wisej.Web.DockStyle.Left;
            this.CmbYear.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbYear.FormattingEnabled = true;
            this.CmbYear.Location = new System.Drawing.Point(610, 0);
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
            this.Txt_HieDesc.Size = new System.Drawing.Size(595, 25);
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
            this.pnlHie.Size = new System.Drawing.Size(675, 25);
            this.pnlHie.TabIndex = 88;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(595, 0);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(15, 25);
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
            // BtnGenPdf
            // 
            this.BtnGenPdf.AppearanceKey = "button-reports";
            this.BtnGenPdf.Dock = Wisej.Web.DockStyle.Right;
            this.BtnGenPdf.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.BtnGenPdf.Location = new System.Drawing.Point(562, 5);
            this.BtnGenPdf.Name = "BtnGenPdf";
            this.BtnGenPdf.Size = new System.Drawing.Size(85, 25);
            this.BtnGenPdf.TabIndex = 3;
            this.BtnGenPdf.Text = "&Generate";
            this.BtnGenPdf.Click += new System.EventHandler(this.BtnGenPdf_Click);
            // 
            // BtnPdfPrev
            // 
            this.BtnPdfPrev.AppearanceKey = "button-reports";
            this.BtnPdfPrev.Dock = Wisej.Web.DockStyle.Right;
            this.BtnPdfPrev.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.BtnPdfPrev.Location = new System.Drawing.Point(650, 5);
            this.BtnPdfPrev.Name = "BtnPdfPrev";
            this.BtnPdfPrev.Size = new System.Drawing.Size(85, 25);
            this.BtnPdfPrev.TabIndex = 4;
            this.BtnPdfPrev.Text = "Pre&view";
            this.BtnPdfPrev.Click += new System.EventHandler(this.BtnPdfPrev_Click);
            // 
            // pnlGenerate
            // 
            this.pnlGenerate.AppearanceKey = "panel-grdo";
            this.pnlGenerate.Controls.Add(this.btnSaveParameters);
            this.pnlGenerate.Controls.Add(this.spacer4);
            this.pnlGenerate.Controls.Add(this.BtnGenPdf);
            this.pnlGenerate.Controls.Add(this.spacer3);
            this.pnlGenerate.Controls.Add(this.btnGetParameters);
            this.pnlGenerate.Controls.Add(this.BtnPdfPrev);
            this.pnlGenerate.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlGenerate.Location = new System.Drawing.Point(0, 202);
            this.pnlGenerate.Name = "pnlGenerate";
            this.pnlGenerate.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlGenerate.Size = new System.Drawing.Size(750, 35);
            this.pnlGenerate.TabIndex = 2;
            // 
            // spacer4
            // 
            this.spacer4.Dock = Wisej.Web.DockStyle.Left;
            this.spacer4.Location = new System.Drawing.Point(125, 5);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(3, 25);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Right;
            this.spacer3.Location = new System.Drawing.Point(647, 5);
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
            this.pnlCompleteForm.Size = new System.Drawing.Size(750, 237);
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
            this.pnlHieFilter.Size = new System.Drawing.Size(750, 43);
            this.pnlHieFilter.TabIndex = 99;
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.Pb_Search_Hie);
            this.pnlFilter.Controls.Add(this.spacer2);
            this.pnlFilter.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlFilter.Location = new System.Drawing.Point(690, 9);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(51, 25);
            this.pnlFilter.TabIndex = 55;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(0, 0);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(15, 25);
            // 
            // HSSB0118_ReportForm
            // 
            this.ClientSize = new System.Drawing.Size(750, 237);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSSB0118_ReportForm";
            this.Text = "HSSB0118_ReportForm";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "tlHelp";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.HSSB0118_ReportForm_ToolClick);
            this.pnlParams.ResumeLayout(false);
            this.pnlSkip.ResumeLayout(false);
            this.pnlSortBy.ResumeLayout(false);
            this.pnlSortBy.PerformLayout();
            this.pnlRouteNo.ResumeLayout(false);
            this.pnlRouteNo.PerformLayout();
            this.pnlBusNo.ResumeLayout(false);
            this.pnlBusNo.PerformLayout();
            this.pnlRepType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).EndInit();
            this.pnlHie.ResumeLayout(false);
            this.pnlHie.PerformLayout();
            this.pnlGenerate.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlHieFilter.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        //private TextBox Txt_HieDesc;
        private Panel pnlParams;
        private Label lblRouteNo;
        private ComboBox cmbRoute;
        private ComboBox cmbBus;
        private Label lblBusNo;
        private RadioButton rbChild;
        private RadioButton rbMaster;
        private Label lblRepType;
        private ComboBox cmbSort;
        private Label lblSort;
        private CheckBox chkbSkip;
        private PictureBox Pb_Search_Hie;
        private ComboBox CmbYear;
        private TextBox Txt_HieDesc;
        private Panel pnlHie;
        private Button btnSaveParameters;
        private Button btnGetParameters;
        private Button BtnGenPdf;
        private Button BtnPdfPrev;
        private Panel pnlGenerate;
        private Panel pnlCompleteForm;
        private Panel pnlHieFilter;
        private Spacer spacer1;
        private Panel pnlFilter;
        private Spacer spacer2;
        private Spacer spacer3;
        private Spacer spacer4;
        private Panel pnlRepType;
        private Panel pnlBusNo;
        private Panel pnlSkip;
        private Panel pnlSortBy;
        private Panel pnlRouteNo;
    }
}