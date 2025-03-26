using Wisej.Web;
using Wisej.Design;

namespace Captain.Common.Views.Forms
{
    partial class AgencySwithcesHierchy
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

        #region WiseJ Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgencySwithcesHierchy));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlOk = new Wisej.Web.Panel();
            this.btnAdd = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.pnlAgencySwitches = new Wisej.Web.Panel();
            this.pnlCheckRecent = new Wisej.Web.Panel();
            this.chkShowCaseManager = new Wisej.Web.CheckBox();
            this.chkmostrecent = new Wisej.Web.CheckBox();
            this.chkClearReport = new Wisej.Web.CheckBox();
            this.chkServiceInquiring = new Wisej.Web.CheckBox();
            this.pnlPayVouch = new Wisej.Web.Panel();
            this.txtExemption = new Wisej.Web.TextBox();
            this.lblTaxexp = new Wisej.Web.Label();
            this.txtPaymentVouch = new Wisej.Web.TextBox();
            this.lblPaymentVouch = new Wisej.Web.Label();
            this.pnlZIPVoucher = new Wisej.Web.Panel();
            this.chkcapvoucher = new Wisej.Web.CheckBox();
            this.chkEditZipandCity = new Wisej.Web.CheckBox();
            this.lblTmsb = new Wisej.Web.Label();
            this.cmbTMSB = new Wisej.Web.ComboBox();
            this.pnlTMSB0020 = new Wisej.Web.Panel();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlOk.SuspendLayout();
            this.pnlAgencySwitches.SuspendLayout();
            this.pnlCheckRecent.SuspendLayout();
            this.pnlPayVouch.SuspendLayout();
            this.pnlZIPVoucher.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlOk);
            this.pnlCompleteForm.Controls.Add(this.pnlAgencySwitches);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(371, 312);
            this.pnlCompleteForm.TabIndex = 3;
            // 
            // pnlOk
            // 
            this.pnlOk.AppearanceKey = "panel-grdo";
            this.pnlOk.Controls.Add(this.btnAdd);
            this.pnlOk.Controls.Add(this.spacer1);
            this.pnlOk.Controls.Add(this.btnCancel);
            this.pnlOk.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlOk.Location = new System.Drawing.Point(0, 277);
            this.pnlOk.Name = "pnlOk";
            this.pnlOk.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlOk.Size = new System.Drawing.Size(371, 35);
            this.pnlOk.TabIndex = 1;
            // 
            // btnAdd
            // 
            this.btnAdd.AppearanceKey = "button-ok";
            this.btnAdd.Dock = Wisej.Web.DockStyle.Right;
            this.btnAdd.Location = new System.Drawing.Point(218, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(60, 25);
            this.btnAdd.TabIndex = 12;
            this.btnAdd.Text = "&OK";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(278, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(281, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlAgencySwitches
            // 
            this.pnlAgencySwitches.Controls.Add(this.pnlCheckRecent);
            this.pnlAgencySwitches.Controls.Add(this.pnlPayVouch);
            this.pnlAgencySwitches.Controls.Add(this.pnlZIPVoucher);
            this.pnlAgencySwitches.Controls.Add(this.lblTmsb);
            this.pnlAgencySwitches.Controls.Add(this.cmbTMSB);
            this.pnlAgencySwitches.Controls.Add(this.pnlTMSB0020);
            this.pnlAgencySwitches.Dock = Wisej.Web.DockStyle.Top;
            this.pnlAgencySwitches.Location = new System.Drawing.Point(0, 0);
            this.pnlAgencySwitches.Name = "pnlAgencySwitches";
            this.pnlAgencySwitches.Padding = new Wisej.Web.Padding(5);
            this.pnlAgencySwitches.Size = new System.Drawing.Size(371, 277);
            this.pnlAgencySwitches.TabIndex = 0;
            // 
            // pnlCheckRecent
            // 
            this.pnlCheckRecent.Controls.Add(this.chkShowCaseManager);
            this.pnlCheckRecent.Controls.Add(this.chkmostrecent);
            this.pnlCheckRecent.Controls.Add(this.chkClearReport);
            this.pnlCheckRecent.Controls.Add(this.chkServiceInquiring);
            this.pnlCheckRecent.Dock = Wisej.Web.DockStyle.Top;
            this.pnlCheckRecent.Location = new System.Drawing.Point(5, 158);
            this.pnlCheckRecent.Name = "pnlCheckRecent";
            this.pnlCheckRecent.Size = new System.Drawing.Size(361, 119);
            this.pnlCheckRecent.TabIndex = 8;
            this.pnlCheckRecent.Visible = false;
            // 
            // chkShowCaseManager
            // 
            this.chkShowCaseManager.AutoSize = false;
            this.chkShowCaseManager.Location = new System.Drawing.Point(6, 36);
            this.chkShowCaseManager.Name = "chkShowCaseManager";
            this.chkShowCaseManager.Size = new System.Drawing.Size(257, 20);
            this.chkShowCaseManager.TabIndex = 9;
            this.chkShowCaseManager.Text = "Show Case Manager Combo in Case2001";
            this.chkShowCaseManager.Visible = false;
            // 
            // chkmostrecent
            // 
            this.chkmostrecent.AutoSize = false;
            this.chkmostrecent.Location = new System.Drawing.Point(6, 9);
            this.chkmostrecent.Name = "chkmostrecent";
            this.chkmostrecent.Size = new System.Drawing.Size(175, 20);
            this.chkmostrecent.TabIndex = 8;
            this.chkmostrecent.Text = "Check most recent Intake";
            this.chkmostrecent.Visible = false;
            // 
            // chkClearReport
            // 
            this.chkClearReport.AutoSize = false;
            this.chkClearReport.Location = new System.Drawing.Point(6, 89);
            this.chkClearReport.Name = "chkClearReport";
            this.chkClearReport.Size = new System.Drawing.Size(221, 20);
            this.chkClearReport.TabIndex = 11;
            this.chkClearReport.Text = "Clear Reports from Reports folder";
            this.chkClearReport.Visible = false;
            // 
            // chkServiceInquiring
            // 
            this.chkServiceInquiring.AutoSize = false;
            this.chkServiceInquiring.Location = new System.Drawing.Point(6, 63);
            this.chkServiceInquiring.Name = "chkServiceInquiring";
            this.chkServiceInquiring.Size = new System.Drawing.Size(222, 20);
            this.chkServiceInquiring.TabIndex = 10;
            this.chkServiceInquiring.Text = "Services Inquiring From Programs";
            this.chkServiceInquiring.Visible = false;
            // 
            // pnlPayVouch
            // 
            this.pnlPayVouch.Controls.Add(this.txtExemption);
            this.pnlPayVouch.Controls.Add(this.lblTaxexp);
            this.pnlPayVouch.Controls.Add(this.txtPaymentVouch);
            this.pnlPayVouch.Controls.Add(this.lblPaymentVouch);
            this.pnlPayVouch.Dock = Wisej.Web.DockStyle.Top;
            this.pnlPayVouch.Location = new System.Drawing.Point(5, 95);
            this.pnlPayVouch.Name = "pnlPayVouch";
            this.pnlPayVouch.Size = new System.Drawing.Size(361, 63);
            this.pnlPayVouch.TabIndex = 7;
            this.pnlPayVouch.Visible = false;
            // 
            // txtExemption
            // 
            this.txtExemption.Location = new System.Drawing.Point(126, 2);
            this.txtExemption.MaxLength = 8;
            this.txtExemption.Name = "txtExemption";
            this.txtExemption.Size = new System.Drawing.Size(77, 25);
            this.txtExemption.TabIndex = 6;
            this.txtExemption.Visible = false;
            // 
            // lblTaxexp
            // 
            this.lblTaxexp.Location = new System.Drawing.Point(10, 7);
            this.lblTaxexp.Name = "lblTaxexp";
            this.lblTaxexp.Size = new System.Drawing.Size(106, 16);
            this.lblTaxexp.TabIndex = 27;
            this.lblTaxexp.Text = "Tax Exemption#";
            this.lblTaxexp.Visible = false;
            // 
            // txtPaymentVouch
            // 
            this.txtPaymentVouch.Location = new System.Drawing.Point(126, 34);
            this.txtPaymentVouch.MaxLength = 8;
            this.txtPaymentVouch.Name = "txtPaymentVouch";
            this.txtPaymentVouch.Size = new System.Drawing.Size(77, 25);
            this.txtPaymentVouch.TabIndex = 7;
            // 
            // lblPaymentVouch
            // 
            this.lblPaymentVouch.Location = new System.Drawing.Point(9, 39);
            this.lblPaymentVouch.Name = "lblPaymentVouch";
            this.lblPaymentVouch.Size = new System.Drawing.Size(121, 16);
            this.lblPaymentVouch.TabIndex = 27;
            this.lblPaymentVouch.Text = "Payment Voucher#";
            // 
            // pnlZIPVoucher
            // 
            this.pnlZIPVoucher.Controls.Add(this.chkcapvoucher);
            this.pnlZIPVoucher.Controls.Add(this.chkEditZipandCity);
            this.pnlZIPVoucher.Dock = Wisej.Web.DockStyle.Top;
            this.pnlZIPVoucher.Location = new System.Drawing.Point(5, 40);
            this.pnlZIPVoucher.Name = "pnlZIPVoucher";
            this.pnlZIPVoucher.Size = new System.Drawing.Size(361, 55);
            this.pnlZIPVoucher.TabIndex = 7;
            // 
            // chkcapvoucher
            // 
            this.chkcapvoucher.AutoSize = false;
            this.chkcapvoucher.Location = new System.Drawing.Point(6, 31);
            this.chkcapvoucher.Name = "chkcapvoucher";
            this.chkcapvoucher.Size = new System.Drawing.Size(232, 20);
            this.chkcapvoucher.TabIndex = 4;
            this.chkcapvoucher.Text = "Payment Voucher in Service Posting";
            this.chkcapvoucher.CheckedChanged += new System.EventHandler(this.chkcapvoucher_CheckedChanged);
            // 
            // chkEditZipandCity
            // 
            this.chkEditZipandCity.AutoSize = false;
            this.chkEditZipandCity.Location = new System.Drawing.Point(6, 6);
            this.chkEditZipandCity.Name = "chkEditZipandCity";
            this.chkEditZipandCity.Size = new System.Drawing.Size(190, 20);
            this.chkEditZipandCity.TabIndex = 3;
            this.chkEditZipandCity.Text = "Edit ZIP and City in Case2001";
            // 
            // lblTmsb
            // 
            this.lblTmsb.AutoSize = true;
            this.lblTmsb.Location = new System.Drawing.Point(14, 15);
            this.lblTmsb.Name = "lblTmsb";
            this.lblTmsb.Size = new System.Drawing.Size(107, 14);
            this.lblTmsb.TabIndex = 5;
            this.lblTmsb.Text = "Launch TMSB0020";
            this.lblTmsb.Visible = false;
            // 
            // cmbTMSB
            // 
            this.cmbTMSB.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbTMSB.FormattingEnabled = true;
            this.cmbTMSB.Location = new System.Drawing.Point(138, 10);
            this.cmbTMSB.Name = "cmbTMSB";
            this.cmbTMSB.Size = new System.Drawing.Size(150, 25);
            this.cmbTMSB.TabIndex = 2;
            // 
            // pnlTMSB0020
            // 
            this.pnlTMSB0020.Dock = Wisej.Web.DockStyle.Top;
            this.pnlTMSB0020.Location = new System.Drawing.Point(5, 5);
            this.pnlTMSB0020.Name = "pnlTMSB0020";
            this.pnlTMSB0020.Size = new System.Drawing.Size(361, 35);
            this.pnlTMSB0020.TabIndex = 6;
            this.pnlTMSB0020.Visible = false;
            // 
            // AgencySwithcesHierchy
            // 
            this.ClientSize = new System.Drawing.Size(371, 312);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AgencySwithcesHierchy";
            this.Text = "Agency Swithces";
            componentTool1.ImageSource = "icon-help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlOk.ResumeLayout(false);
            this.pnlAgencySwitches.ResumeLayout(false);
            this.pnlAgencySwitches.PerformLayout();
            this.pnlCheckRecent.ResumeLayout(false);
            this.pnlPayVouch.ResumeLayout(false);
            this.pnlPayVouch.PerformLayout();
            this.pnlZIPVoucher.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion
        
        private Panel panel4;
        private Panel pnlCompleteForm;
        private Panel pnlAgencySwitches;
        private Button btnCancel;
        private Button btnAdd;
        private CheckBox chkEditZipandCity;
        private Label lblTmsb;
        private ComboBox cmbTMSB;
        private CheckBox chkcapvoucher;
        private TextBox txtExemption;
        private Label lblTaxexp;
        private Panel pnlPayVouch;
        private TextBox txtPaymentVouch;
        private Label lblPaymentVouch;
        private CheckBox chkServiceInquiring;
        private CheckBox chkShowCaseManager;
        private CheckBox chkClearReport;
        private CheckBox chkmostrecent;
        private Panel pnlOk;
        private Spacer spacer1;
        private Panel pnlCheckRecent;
        private Panel pnlZIPVoucher;
        private Panel pnlTMSB0020;
    }
}