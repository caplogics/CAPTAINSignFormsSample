using Wisej.Web;
using Wisej.Design;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class CAMSForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CAMSForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.pnlCompleteform = new Wisej.Web.Panel();
            this.pnlUnit = new Wisej.Web.Panel();
            this.btnUnitPrice = new Wisej.Web.Button();
            this.chkbAutoPost = new Wisej.Web.CheckBox();
            this.txtTrans = new Wisej.Web.TextBox();
            this.lblTrans = new Wisej.Web.Label();
            this.label2 = new Wisej.Web.Label();
            this.lblVendorpay = new Wisej.Web.Label();
            this.cmbVendor = new Wisej.Web.ComboBox();
            this.Lbl_UOM = new Wisej.Web.Label();
            this.lblType = new Wisej.Web.Label();
            this.pnlSave = new Wisej.Web.Panel();
            this.btnSave = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.chkbActive = new Wisej.Web.CheckBox();
            this.lblDesc = new Wisej.Web.Label();
            this.txtDesc = new Wisej.Web.TextBox();
            this.txtService = new Wisej.Web.TextBox();
            this.lblSeviceCode = new Wisej.Web.Label();
            this.rbOutcm = new Wisej.Web.RadioButton();
            this.rbMS = new Wisej.Web.RadioButton();
            this.label3 = new Wisej.Web.Label();
            this.pnlServiceCode = new Wisej.Web.Panel();
            this.Cmb_UOM = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.pnlCompleteform.SuspendLayout();
            this.pnlUnit.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCompleteform
            // 
            this.pnlCompleteform.Controls.Add(this.pnlUnit);
            this.pnlCompleteform.Controls.Add(this.label2);
            this.pnlCompleteform.Controls.Add(this.lblVendorpay);
            this.pnlCompleteform.Controls.Add(this.cmbVendor);
            this.pnlCompleteform.Controls.Add(this.Lbl_UOM);
            this.pnlCompleteform.Controls.Add(this.Cmb_UOM);
            this.pnlCompleteform.Controls.Add(this.lblType);
            this.pnlCompleteform.Controls.Add(this.pnlSave);
            this.pnlCompleteform.Controls.Add(this.chkbActive);
            this.pnlCompleteform.Controls.Add(this.lblDesc);
            this.pnlCompleteform.Controls.Add(this.txtDesc);
            this.pnlCompleteform.Controls.Add(this.txtService);
            this.pnlCompleteform.Controls.Add(this.lblSeviceCode);
            this.pnlCompleteform.Controls.Add(this.rbOutcm);
            this.pnlCompleteform.Controls.Add(this.rbMS);
            this.pnlCompleteform.Controls.Add(this.label3);
            this.pnlCompleteform.Controls.Add(this.pnlServiceCode);
            this.pnlCompleteform.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteform.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteform.Name = "pnlCompleteform";
            this.pnlCompleteform.Size = new System.Drawing.Size(903, 177);
            this.pnlCompleteform.TabIndex = 0;
            // 
            // pnlUnit
            // 
            this.pnlUnit.Controls.Add(this.btnUnitPrice);
            this.pnlUnit.Controls.Add(this.chkbAutoPost);
            this.pnlUnit.Controls.Add(this.txtTrans);
            this.pnlUnit.Controls.Add(this.lblTrans);
            this.pnlUnit.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlUnit.Location = new System.Drawing.Point(0, 102);
            this.pnlUnit.Name = "pnlUnit";
            this.pnlUnit.Size = new System.Drawing.Size(903, 40);
            this.pnlUnit.TabIndex = 6;
            // 
            // btnUnitPrice
            // 
            this.btnUnitPrice.Location = new System.Drawing.Point(539, 5);
            this.btnUnitPrice.Name = "btnUnitPrice";
            this.btnUnitPrice.Size = new System.Drawing.Size(86, 25);
            this.btnUnitPrice.TabIndex = 9;
            this.btnUnitPrice.Text = "&Unit Price";
            this.btnUnitPrice.Visible = false;
            this.btnUnitPrice.Click += new System.EventHandler(this.btnUnitPrice_Click);
            // 
            // chkbAutoPost
            // 
            this.chkbAutoPost.AutoSize = false;
            this.chkbAutoPost.Location = new System.Drawing.Point(334, 6);
            this.chkbAutoPost.Name = "chkbAutoPost";
            this.chkbAutoPost.Size = new System.Drawing.Size(82, 20);
            this.chkbAutoPost.TabIndex = 8;
            this.chkbAutoPost.Text = "Auto Post";
            this.chkbAutoPost.Visible = false;
            // 
            // txtTrans
            // 
            this.txtTrans.Location = new System.Drawing.Point(127, 5);
            this.txtTrans.MaxLength = 20;
            this.txtTrans.Name = "txtTrans";
            this.txtTrans.Size = new System.Drawing.Size(170, 25);
            this.txtTrans.TabIndex = 7;
            this.txtTrans.Visible = false;
            // 
            // lblTrans
            // 
            this.lblTrans.Location = new System.Drawing.Point(15, 9);
            this.lblTrans.Name = "lblTrans";
            this.lblTrans.Size = new System.Drawing.Size(102, 16);
            this.lblTrans.TabIndex = 1;
            this.lblTrans.Text = "Unit limit warning";
            this.lblTrans.Visible = false;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(8, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(6, 11);
            this.label2.TabIndex = 1;
            this.label2.Text = "*";
            // 
            // lblVendorpay
            // 
            this.lblVendorpay.Location = new System.Drawing.Point(536, 15);
            this.lblVendorpay.Name = "lblVendorpay";
            this.lblVendorpay.Size = new System.Drawing.Size(140, 16);
            this.lblVendorpay.TabIndex = 4;
            this.lblVendorpay.Text = "Vendor Pay for Category";
            this.lblVendorpay.Visible = false;
            // 
            // cmbVendor
            // 
            this.cmbVendor.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbVendor.FormattingEnabled = true;
            this.cmbVendor.Location = new System.Drawing.Point(688, 11);
            this.cmbVendor.Name = "cmbVendor";
            this.cmbVendor.Size = new System.Drawing.Size(175, 25);
            this.cmbVendor.TabIndex = 4;
            this.cmbVendor.Visible = false;
            // 
            // Lbl_UOM
            // 
            this.Lbl_UOM.Location = new System.Drawing.Point(686, 51);
            this.Lbl_UOM.Name = "Lbl_UOM";
            this.Lbl_UOM.Size = new System.Drawing.Size(31, 14);
            this.Lbl_UOM.TabIndex = 1;
            this.Lbl_UOM.Text = "UOM";
            this.Lbl_UOM.Visible = false;
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(310, 15);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(28, 16);
            this.lblType.TabIndex = 1;
            this.lblType.Text = "Type";
            this.lblType.Visible = false;
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnSave);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 142);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(903, 35);
            this.pnlSave.TabIndex = 10;
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(735, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(810, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(813, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkbActive
            // 
            this.chkbActive.Appearance = Wisej.Web.Appearance.Switch;
            this.chkbActive.AutoSize = false;
            this.chkbActive.Checked = true;
            this.chkbActive.Location = new System.Drawing.Point(219, 13);
            this.chkbActive.Name = "chkbActive";
            this.chkbActive.Size = new System.Drawing.Size(69, 20);
            this.chkbActive.TabIndex = 3;
            this.chkbActive.Text = "Active";
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(15, 51);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(66, 16);
            this.lblDesc.TabIndex = 1;
            this.lblDesc.Text = "Description";
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(110, 43);
            this.txtDesc.MaxLength = 100;
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(515, 57);
            this.txtDesc.TabIndex = 5;
            // 
            // txtService
            // 
            this.txtService.Location = new System.Drawing.Point(110, 11);
            this.txtService.MaxLength = 5;
            this.txtService.Name = "txtService";
            this.txtService.Size = new System.Drawing.Size(77, 25);
            this.txtService.TabIndex = 2;
            this.txtService.LostFocus += new System.EventHandler(this.txtService_LostFocus);
            this.txtService.Leave += new System.EventHandler(this.txtService_Leave);
            // 
            // lblSeviceCode
            // 
            this.lblSeviceCode.AutoSize = true;
            this.lblSeviceCode.Location = new System.Drawing.Point(15, 15);
            this.lblSeviceCode.Name = "lblSeviceCode";
            this.lblSeviceCode.Size = new System.Drawing.Size(76, 14);
            this.lblSeviceCode.TabIndex = 1;
            this.lblSeviceCode.Text = "Service Code";
            // 
            // rbOutcm
            // 
            this.rbOutcm.AutoSize = false;
            this.rbOutcm.Checked = true;
            this.rbOutcm.Location = new System.Drawing.Point(446, 12);
            this.rbOutcm.Name = "rbOutcm";
            this.rbOutcm.Size = new System.Drawing.Size(79, 20);
            this.rbOutcm.TabIndex = 44;
            this.rbOutcm.Text = "Outcome";
            this.rbOutcm.Visible = false;
            // 
            // rbMS
            // 
            this.rbMS.AutoSize = false;
            this.rbMS.Location = new System.Drawing.Point(349, 12);
            this.rbMS.Name = "rbMS";
            this.rbMS.Size = new System.Drawing.Size(82, 20);
            this.rbMS.TabIndex = 55;
            this.rbMS.Text = "Milestone";
            this.rbMS.Visible = false;
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(8, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(7, 10);
            this.label3.TabIndex = 1;
            this.label3.Text = "*";
            // 
            // pnlServiceCode
            // 
            this.pnlServiceCode.Dock = Wisej.Web.DockStyle.Top;
            this.pnlServiceCode.Location = new System.Drawing.Point(0, 0);
            this.pnlServiceCode.Name = "pnlServiceCode";
            this.pnlServiceCode.Size = new System.Drawing.Size(903, 101);
            this.pnlServiceCode.TabIndex = 1;
            // 
            // Cmb_UOM
            // 
            this.Cmb_UOM.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.Cmb_UOM.FormattingEnabled = true;
            this.Cmb_UOM.Location = new System.Drawing.Point(740, 46);
            this.Cmb_UOM.Name = "Cmb_UOM";
            this.Cmb_UOM.Size = new System.Drawing.Size(83, 25);
            this.Cmb_UOM.TabIndex = 4;
            this.Cmb_UOM.Visible = false;
            // 
            // CAMSForm
            // 
            this.ClientSize = new System.Drawing.Size(903, 177);
            this.Controls.Add(this.pnlCompleteform);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CAMSForm";
            this.Text = "CAMSForm";
            componentTool1.ImageSource = "icon-help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.pnlCompleteform.ResumeLayout(false);
            this.pnlCompleteform.PerformLayout();
            this.pnlUnit.ResumeLayout(false);
            this.pnlUnit.PerformLayout();
            this.pnlSave.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Panel pnlCompleteform;
        private Panel pnlSave;
        private Button btnCancel;
        private Button btnSave;
        private CheckBox chkbActive;
        private Label lblDesc;
        private TextBox txtDesc;
        private TextBox txtService;
        private Label lblSeviceCode;
        private RadioButton rbOutcm;
        private RadioButton rbMS;
        private Label label2;
        private Label label3;
        private Label lblType;
        private CheckBox chkbAutoPost;
        private Label Lbl_UOM;
        private ComboBoxEx Cmb_UOM;
        private Label lblTrans;
        private TextBox txtTrans;
        private Label lblVendorpay;
        private ComboBox cmbVendor;
        private Button btnUnitPrice;
        private Spacer spacer1;
        private Panel pnlUnit;
        private Panel pnlServiceCode;
    }
}