using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class PIPB0004
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

        #region Wisej  Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PIPB0004));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.panel1 = new Wisej.Web.Panel();
            this.rbSubmit = new Wisej.Web.RadioButton();
            this.rbReg = new Wisej.Web.RadioButton();
            this.label1 = new Wisej.Web.Label();
            this.lblIntakeStatus = new Wisej.Web.Label();
            this.cmbIntakeStatus = new Wisej.Web.ComboBox();
            this.CmbAgency = new Wisej.Web.ComboBox();
            this.lblAgency = new Wisej.Web.Label();
            this.cmbDrag = new Wisej.Web.ComboBox();
            this.lblDrag = new Wisej.Web.Label();
            this.lblFrmDt = new Wisej.Web.Label();
            this.dtpFrmDate = new Wisej.Web.DateTimePicker();
            this.dtpToDt = new Wisej.Web.DateTimePicker();
            this.lblToDt = new Wisej.Web.Label();
            this.btnPdfPrev = new Wisej.Web.Button();
            this.btnGenPdf = new Wisej.Web.Button();
            this.panel2 = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.chkbExcel = new Wisej.Web.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbSubmit);
            this.panel1.Controls.Add(this.rbReg);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblIntakeStatus);
            this.panel1.Controls.Add(this.cmbIntakeStatus);
            this.panel1.Controls.Add(this.CmbAgency);
            this.panel1.Controls.Add(this.lblAgency);
            this.panel1.Controls.Add(this.cmbDrag);
            this.panel1.Controls.Add(this.lblDrag);
            this.panel1.Controls.Add(this.lblFrmDt);
            this.panel1.Controls.Add(this.dtpFrmDate);
            this.panel1.Controls.Add(this.dtpToDt);
            this.panel1.Controls.Add(this.lblToDt);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(395, 162);
            this.panel1.TabIndex = 1;
            // 
            // rbSubmit
            // 
            this.rbSubmit.AutoSize = false;
            this.rbSubmit.Location = new System.Drawing.Point(239, 42);
            this.rbSubmit.Name = "rbSubmit";
            this.rbSubmit.Size = new System.Drawing.Size(91, 21);
            this.rbSubmit.TabIndex = 3;
            this.rbSubmit.Text = "Submitted";
            this.rbSubmit.CheckedChanged += new System.EventHandler(this.rbSubmit_CheckedChanged);
            // 
            // rbReg
            // 
            this.rbReg.Checked = true;
            this.rbReg.Location = new System.Drawing.Point(106, 42);
            this.rbReg.Name = "rbReg";
            this.rbReg.Size = new System.Drawing.Size(91, 21);
            this.rbReg.TabIndex = 2;
            this.rbReg.TabStop = true;
            this.rbReg.Text = "Registered";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 46);
            this.label1.MinimumSize = new System.Drawing.Size(0, 18);
            this.label1.Name = "label1";
            this.label1.RightToLeft = Wisej.Web.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(70, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "Report Type";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblIntakeStatus
            // 
            this.lblIntakeStatus.Location = new System.Drawing.Point(19, 133);
            this.lblIntakeStatus.Name = "lblIntakeStatus";
            this.lblIntakeStatus.Size = new System.Drawing.Size(86, 16);
            this.lblIntakeStatus.TabIndex = 4;
            this.lblIntakeStatus.Text = "Intake Status";
            this.lblIntakeStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblIntakeStatus.Visible = false;
            // 
            // cmbIntakeStatus
            // 
            this.cmbIntakeStatus.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbIntakeStatus.FormattingEnabled = true;
            this.cmbIntakeStatus.Location = new System.Drawing.Point(110, 129);
            this.cmbIntakeStatus.Name = "cmbIntakeStatus";
            this.cmbIntakeStatus.Size = new System.Drawing.Size(252, 25);
            this.cmbIntakeStatus.TabIndex = 7;
            this.cmbIntakeStatus.Visible = false;
            // 
            // CmbAgency
            // 
            this.CmbAgency.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbAgency.FormattingEnabled = true;
            this.CmbAgency.Location = new System.Drawing.Point(110, 10);
            this.CmbAgency.Name = "CmbAgency";
            this.CmbAgency.Size = new System.Drawing.Size(252, 25);
            this.CmbAgency.TabIndex = 1;
            this.CmbAgency.Visible = false;
            // 
            // lblAgency
            // 
            this.lblAgency.AutoSize = true;
            this.lblAgency.Location = new System.Drawing.Point(19, 17);
            this.lblAgency.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblAgency.Name = "lblAgency";
            this.lblAgency.Size = new System.Drawing.Size(45, 18);
            this.lblAgency.TabIndex = 4;
            this.lblAgency.Text = "Agency";
            this.lblAgency.Visible = false;
            // 
            // cmbDrag
            // 
            this.cmbDrag.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbDrag.FormattingEnabled = true;
            this.cmbDrag.Location = new System.Drawing.Point(110, 99);
            this.cmbDrag.Name = "cmbDrag";
            this.cmbDrag.Size = new System.Drawing.Size(252, 25);
            this.cmbDrag.TabIndex = 6;
            // 
            // lblDrag
            // 
            this.lblDrag.AutoSize = true;
            this.lblDrag.Location = new System.Drawing.Point(19, 105);
            this.lblDrag.MinimumSize = new System.Drawing.Size(70, 18);
            this.lblDrag.Name = "lblDrag";
            this.lblDrag.Size = new System.Drawing.Size(70, 18);
            this.lblDrag.TabIndex = 4;
            this.lblDrag.Text = "Report for";
            this.lblDrag.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFrmDt
            // 
            this.lblFrmDt.AutoSize = true;
            this.lblFrmDt.Location = new System.Drawing.Point(17, 74);
            this.lblFrmDt.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblFrmDt.Name = "lblFrmDt";
            this.lblFrmDt.RightToLeft = Wisej.Web.RightToLeft.No;
            this.lblFrmDt.Size = new System.Drawing.Size(61, 18);
            this.lblFrmDt.TabIndex = 0;
            this.lblFrmDt.Text = "Date From";
            this.lblFrmDt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dtpFrmDate
            // 
            this.dtpFrmDate.Checked = false;
            this.dtpFrmDate.CustomFormat = "MM/dd/yyyy";
            this.dtpFrmDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpFrmDate.Location = new System.Drawing.Point(110, 69);
            this.dtpFrmDate.MinimumSize = new System.Drawing.Size(0, 25);
            this.dtpFrmDate.Name = "dtpFrmDate";
            this.dtpFrmDate.ShowToolTips = false;
            this.dtpFrmDate.Size = new System.Drawing.Size(96, 25);
            this.dtpFrmDate.TabIndex = 4;
            // 
            // dtpToDt
            // 
            this.dtpToDt.Checked = false;
            this.dtpToDt.CustomFormat = "MM/dd/yyyy";
            this.dtpToDt.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpToDt.Location = new System.Drawing.Point(266, 69);
            this.dtpToDt.MinimumSize = new System.Drawing.Size(0, 25);
            this.dtpToDt.Name = "dtpToDt";
            this.dtpToDt.ShowToolTips = false;
            this.dtpToDt.Size = new System.Drawing.Size(96, 25);
            this.dtpToDt.TabIndex = 5;
            // 
            // lblToDt
            // 
            this.lblToDt.AutoSize = true;
            this.lblToDt.Location = new System.Drawing.Point(243, 72);
            this.lblToDt.Name = "lblToDt";
            this.lblToDt.RightToLeft = Wisej.Web.RightToLeft.No;
            this.lblToDt.Size = new System.Drawing.Size(19, 14);
            this.lblToDt.TabIndex = 0;
            this.lblToDt.Text = "To";
            // 
            // btnPdfPrev
            // 
            this.btnPdfPrev.AppearanceKey = "button-reports";
            this.btnPdfPrev.Dock = Wisej.Web.DockStyle.Right;
            this.btnPdfPrev.Location = new System.Drawing.Point(300, 5);
            this.btnPdfPrev.Name = "btnPdfPrev";
            this.btnPdfPrev.Size = new System.Drawing.Size(80, 25);
            this.btnPdfPrev.TabIndex = 3;
            this.btnPdfPrev.Text = "Pre&view";
            this.btnPdfPrev.Click += new System.EventHandler(this.BtnPdfPrev_Click);
            // 
            // btnGenPdf
            // 
            this.btnGenPdf.AppearanceKey = "button-reports";
            this.btnGenPdf.Dock = Wisej.Web.DockStyle.Right;
            this.btnGenPdf.Location = new System.Drawing.Point(215, 5);
            this.btnGenPdf.Name = "btnGenPdf";
            this.btnGenPdf.Size = new System.Drawing.Size(80, 25);
            this.btnGenPdf.TabIndex = 2;
            this.btnGenPdf.Text = "G&enerate";
            this.btnGenPdf.Click += new System.EventHandler(this.BtnGenFile_Click);
            // 
            // panel2
            // 
            this.panel2.AppearanceKey = "panel-grdo";
            this.panel2.Controls.Add(this.btnGenPdf);
            this.panel2.Controls.Add(this.spacer1);
            this.panel2.Controls.Add(this.chkbExcel);
            this.panel2.Controls.Add(this.btnPdfPrev);
            this.panel2.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 162);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.panel2.Size = new System.Drawing.Size(395, 35);
            this.panel2.TabIndex = 2;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(295, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 25);
            // 
            // chkbExcel
            // 
            this.chkbExcel.Location = new System.Drawing.Point(18, 5);
            this.chkbExcel.Name = "chkbExcel";
            this.chkbExcel.Size = new System.Drawing.Size(115, 21);
            this.chkbExcel.TabIndex = 1;
            this.chkbExcel.Text = "Generate Excel";
            // 
            // PIPB0004
            // 
            this.ClientSize = new System.Drawing.Size(395, 197);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PIPB0004";
            this.Text = "PIPB0002";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "tlHelp";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.PIPB0004_ToolClick);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Label lblFrmDt;
        private DateTimePicker dtpFrmDate;
        private DateTimePicker dtpToDt;
        private Label lblToDt;
        private Button btnPdfPrev;
        private Button btnGenPdf;
        private Panel panel2;
        private CheckBox chkbExcel;
        private ComboBox cmbDrag;
        private Label lblDrag;
        private ComboBox CmbAgency;
        private Label lblAgency;
        private Label lblIntakeStatus;
        private ComboBox cmbIntakeStatus;
        private Spacer spacer1;
        private RadioButton rbSubmit;
        private RadioButton rbReg;
        private Label label1;
    }
}