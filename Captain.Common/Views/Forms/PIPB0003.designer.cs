using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class PIPB0003
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PIPB0003));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.panel1 = new Wisej.Web.Panel();
            this.lblAgency = new Wisej.Web.Label();
            this.CmbAgency = new Wisej.Web.ComboBox();
            this.lblFrmDt = new Wisej.Web.Label();
            this.dtpFrmDate = new Wisej.Web.DateTimePicker();
            this.dtpToDt = new Wisej.Web.DateTimePicker();
            this.lblToDt = new Wisej.Web.Label();
            this.btnPdfPrev = new Wisej.Web.Button();
            this.BtnGenPdf = new Wisej.Web.Button();
            this.panel2 = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.chkbExcel = new Wisej.Web.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblAgency);
            this.panel1.Controls.Add(this.CmbAgency);
            this.panel1.Controls.Add(this.lblFrmDt);
            this.panel1.Controls.Add(this.dtpFrmDate);
            this.panel1.Controls.Add(this.dtpToDt);
            this.panel1.Controls.Add(this.lblToDt);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(452, 105);
            this.panel1.TabIndex = 0;
            // 
            // lblAgency
            // 
            this.lblAgency.AutoSize = true;
            this.lblAgency.Location = new System.Drawing.Point(29, 28);
            this.lblAgency.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblAgency.Name = "lblAgency";
            this.lblAgency.Size = new System.Drawing.Size(45, 18);
            this.lblAgency.TabIndex = 4;
            this.lblAgency.Text = "Agency";
            this.lblAgency.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblAgency.Visible = false;
            // 
            // CmbAgency
            // 
            this.CmbAgency.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbAgency.FormattingEnabled = true;
            this.CmbAgency.Location = new System.Drawing.Point(151, 25);
            this.CmbAgency.Name = "CmbAgency";
            this.CmbAgency.Size = new System.Drawing.Size(251, 25);
            this.CmbAgency.TabIndex = 1;
            this.CmbAgency.Visible = false;
            // 
            // lblFrmDt
            // 
            this.lblFrmDt.AutoSize = true;
            this.lblFrmDt.Location = new System.Drawing.Point(29, 58);
            this.lblFrmDt.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblFrmDt.Name = "lblFrmDt";
            this.lblFrmDt.RightToLeft = Wisej.Web.RightToLeft.No;
            this.lblFrmDt.Size = new System.Drawing.Size(94, 18);
            this.lblFrmDt.TabIndex = 0;
            this.lblFrmDt.Text = "Registered From";
            this.lblFrmDt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dtpFrmDate
            // 
            this.dtpFrmDate.Checked = false;
            this.dtpFrmDate.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.dtpFrmDate.Location = new System.Drawing.Point(151, 55);
            this.dtpFrmDate.Name = "dtpFrmDate";
            this.dtpFrmDate.ShowToolTips = false;
            this.dtpFrmDate.Size = new System.Drawing.Size(96, 22);
            this.dtpFrmDate.TabIndex = 2;
            // 
            // dtpToDt
            // 
            this.dtpToDt.Checked = false;
            this.dtpToDt.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.dtpToDt.Location = new System.Drawing.Point(306, 55);
            this.dtpToDt.Name = "dtpToDt";
            this.dtpToDt.ShowToolTips = false;
            this.dtpToDt.Size = new System.Drawing.Size(96, 22);
            this.dtpToDt.TabIndex = 3;
            // 
            // lblToDt
            // 
            this.lblToDt.AutoSize = true;
            this.lblToDt.Location = new System.Drawing.Point(278, 58);
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
            this.btnPdfPrev.Location = new System.Drawing.Point(357, 5);
            this.btnPdfPrev.Name = "btnPdfPrev";
            this.btnPdfPrev.Size = new System.Drawing.Size(80, 25);
            this.btnPdfPrev.TabIndex = 2;
            this.btnPdfPrev.Text = "Pre&view";
            this.btnPdfPrev.Click += new System.EventHandler(this.BtnPdfPrev_Click);
            // 
            // BtnGenPdf
            // 
            this.BtnGenPdf.AppearanceKey = "button-reports";
            this.BtnGenPdf.Dock = Wisej.Web.DockStyle.Right;
            this.BtnGenPdf.Location = new System.Drawing.Point(272, 5);
            this.BtnGenPdf.Name = "BtnGenPdf";
            this.BtnGenPdf.Size = new System.Drawing.Size(80, 25);
            this.BtnGenPdf.TabIndex = 1;
            this.BtnGenPdf.Text = "G&enerate";
            this.BtnGenPdf.Click += new System.EventHandler(this.BtnGenFile_Click);
            // 
            // panel2
            // 
            this.panel2.AppearanceKey = "panel-grdo";
            this.panel2.Controls.Add(this.BtnGenPdf);
            this.panel2.Controls.Add(this.spacer1);
            this.panel2.Controls.Add(this.chkbExcel);
            this.panel2.Controls.Add(this.btnPdfPrev);
            this.panel2.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 105);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.panel2.Size = new System.Drawing.Size(452, 35);
            this.panel2.TabIndex = 2;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(352, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 25);
            // 
            // chkbExcel
            // 
            this.chkbExcel.Location = new System.Drawing.Point(11, 8);
            this.chkbExcel.Name = "chkbExcel";
            this.chkbExcel.Size = new System.Drawing.Size(115, 21);
            this.chkbExcel.TabIndex = 28;
            this.chkbExcel.Text = "Generate Excel";
            // 
            // PIPB0003
            // 
            this.ClientSize = new System.Drawing.Size(452, 140);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PIPB0003";
            this.Text = "PIPB0001";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "tlHelp";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.PIPB0003_ToolClick);
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
        private Button BtnGenPdf;
        private Panel panel2;
        private CheckBox chkbExcel;
        private Label lblAgency;
        private ComboBox CmbAgency;
        private Spacer spacer1;
    }
}