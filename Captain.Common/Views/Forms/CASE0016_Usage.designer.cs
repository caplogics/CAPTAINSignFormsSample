using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class CASE0016_Usage
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

        #region WISEJ Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle10 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle11 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle12 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CASE0016_Usage));
            this.btnCancel = new Wisej.Web.Button();
            this.btnSave = new Wisej.Web.Button();
            this.panel5 = new Wisej.Web.Panel();
            this.spacer2 = new Wisej.Web.Spacer();
            this.btnInvoices = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.lblAward = new Wisej.Web.Label();
            this.label17 = new Wisej.Web.Label();
            this.label14 = new Wisej.Web.Label();
            this.panel3 = new Wisej.Web.Panel();
            this.panel6 = new Wisej.Web.Panel();
            this.panel4 = new Wisej.Web.Panel();
            this.txtScore = new Wisej.Web.TextBox();
            this.label7 = new Wisej.Web.Label();
            this.txtInvBal = new Wisej.Web.TextBox();
            this.label3 = new Wisej.Web.Label();
            this.label2 = new Wisej.Web.Label();
            this.txtPaidInv = new Wisej.Web.TextBox();
            this.txtPaid = new Wisej.Web.TextBox();
            this.lblPaidAmt = new Wisej.Web.Label();
            this.txtRemTrans = new Wisej.Web.TextBox();
            this.txtMaxInvs = new Wisej.Web.TextBox();
            this.lblMaxInv = new Wisej.Web.Label();
            this.txtAccountNo = new Wisej.Web.TextBox();
            this.lblAccount = new Wisej.Web.Label();
            this.Text_VendName = new Wisej.Web.TextBox();
            this.Txt_VendNo = new Wisej.Web.TextBox();
            this.label1 = new Wisej.Web.Label();
            this.lblVendor = new Wisej.Web.Label();
            this.lblSPName = new Wisej.Web.Label();
            this.lblServicePlan = new Wisej.Web.Label();
            this.lblService = new Wisej.Web.Label();
            this.gvwMonthQuestions = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.gvtMonthQuesCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtMonthQues = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvkChk = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvtElecpayment = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtServicedate = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.gvtOverride = new Wisej.Web.DataGridViewImageColumn();
            this.gvtAmtPaid = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtBundle = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtCheck = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtChkDate = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.gvVendor = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvDelete = new Wisej.Web.DataGridViewImageColumn();
            this.gvCaseact = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvPDOut = new Wisej.Web.DataGridViewTextBoxColumn();
            this.CmbSP = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.txtBalance = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtAmount = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.gvtUsageReq = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwMonthQuestions)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-cancel";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(896, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 27);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Dock = Wisej.Web.DockStyle.Left;
            this.btnSave.Location = new System.Drawing.Point(142, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(70, 27);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save";
            this.btnSave.Visible = false;
            // 
            // panel5
            // 
            this.panel5.AppearanceKey = "panel-grdo";
            this.panel5.Controls.Add(this.btnSave);
            this.panel5.Controls.Add(this.spacer2);
            this.panel5.Controls.Add(this.btnInvoices);
            this.panel5.Controls.Add(this.spacer1);
            this.panel5.Controls.Add(this.btnCancel);
            this.panel5.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 408);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new Wisej.Web.Padding(4);
            this.panel5.Size = new System.Drawing.Size(970, 35);
            this.panel5.TabIndex = 3;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(4, 4);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(138, 27);
            // 
            // btnInvoices
            // 
            this.btnInvoices.Dock = Wisej.Web.DockStyle.Right;
            this.btnInvoices.Enabled = false;
            this.btnInvoices.Location = new System.Drawing.Point(718, 4);
            this.btnInvoices.Name = "btnInvoices";
            this.btnInvoices.Size = new System.Drawing.Size(173, 27);
            this.btnInvoices.TabIndex = 1;
            this.btnInvoices.Text = "Create &Invoices from Usage";
            this.btnInvoices.Click += new System.EventHandler(this.btnInvoices_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(891, 4);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 27);
            // 
            // lblAward
            // 
            this.lblAward.AutoSize = true;
            this.lblAward.Location = new System.Drawing.Point(621, 11);
            this.lblAward.Name = "lblAward";
            this.lblAward.Size = new System.Drawing.Size(39, 14);
            this.lblAward.TabIndex = 0;
            this.lblAward.Text = "Award";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(612, 67);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(48, 14);
            this.label17.TabIndex = 2;
            this.label17.Text = "Balance";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(5, 36);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(9, 14);
            this.label14.TabIndex = 28;
            this.label14.Text = "*";
            this.label14.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Dock = Wisej.Web.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(970, 443);
            this.panel3.TabIndex = 1;
            this.panel3.Click += new System.EventHandler(this.panel3_Click);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.gvwMonthQuestions);
            this.panel6.Dock = Wisej.Web.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 125);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(970, 283);
            this.panel6.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.txtScore);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.txtInvBal);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.txtPaidInv);
            this.panel4.Controls.Add(this.txtPaid);
            this.panel4.Controls.Add(this.lblPaidAmt);
            this.panel4.Controls.Add(this.txtRemTrans);
            this.panel4.Controls.Add(this.txtMaxInvs);
            this.panel4.Controls.Add(this.lblMaxInv);
            this.panel4.Controls.Add(this.txtAccountNo);
            this.panel4.Controls.Add(this.lblAccount);
            this.panel4.Controls.Add(this.Text_VendName);
            this.panel4.Controls.Add(this.Txt_VendNo);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.lblVendor);
            this.panel4.Controls.Add(this.lblSPName);
            this.panel4.Controls.Add(this.lblServicePlan);
            this.panel4.Controls.Add(this.label14);
            this.panel4.Controls.Add(this.lblService);
            this.panel4.Controls.Add(this.CmbSP);
            this.panel4.Controls.Add(this.lblAward);
            this.panel4.Controls.Add(this.label17);
            this.panel4.Controls.Add(this.txtBalance);
            this.panel4.Controls.Add(this.txtAmount);
            this.panel4.Dock = Wisej.Web.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(970, 125);
            this.panel4.TabIndex = 1;
            // 
            // txtScore
            // 
            this.txtScore.Enabled = false;
            this.txtScore.Location = new System.Drawing.Point(895, 91);
            this.txtScore.MaxLength = 2;
            this.txtScore.Name = "txtScore";
            this.txtScore.Size = new System.Drawing.Size(25, 25);
            this.txtScore.TabIndex = 30;
            this.txtScore.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label7.Location = new System.Drawing.Point(813, 96);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 16);
            this.label7.TabIndex = 29;
            this.label7.Text = "Priority Score";
            // 
            // txtInvBal
            // 
            this.txtInvBal.Enabled = false;
            this.txtInvBal.Location = new System.Drawing.Point(895, 62);
            this.txtInvBal.MaxLength = 2;
            this.txtInvBal.Name = "txtInvBal";
            this.txtInvBal.Size = new System.Drawing.Size(25, 25);
            this.txtInvBal.TabIndex = 4;
            this.txtInvBal.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(805, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 14);
            this.label3.TabIndex = 0;
            this.label3.Text = "Invoice Balance";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(813, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 14);
            this.label2.TabIndex = 0;
            this.label2.Text = "Total Invoices";
            // 
            // txtPaidInv
            // 
            this.txtPaidInv.Enabled = false;
            this.txtPaidInv.Location = new System.Drawing.Point(895, 34);
            this.txtPaidInv.MaxLength = 2;
            this.txtPaidInv.Name = "txtPaidInv";
            this.txtPaidInv.Size = new System.Drawing.Size(25, 25);
            this.txtPaidInv.TabIndex = 4;
            this.txtPaidInv.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // txtPaid
            // 
            this.txtPaid.Enabled = false;
            this.txtPaid.Location = new System.Drawing.Point(670, 36);
            this.txtPaid.MaxLength = 15;
            this.txtPaid.Name = "txtPaid";
            this.txtPaid.Size = new System.Drawing.Size(57, 25);
            this.txtPaid.TabIndex = 5;
            this.txtPaid.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // lblPaidAmt
            // 
            this.lblPaidAmt.AutoSize = true;
            this.lblPaidAmt.Location = new System.Drawing.Point(601, 39);
            this.lblPaidAmt.Name = "lblPaidAmt";
            this.lblPaidAmt.Size = new System.Drawing.Size(59, 14);
            this.lblPaidAmt.TabIndex = 2;
            this.lblPaidAmt.Text = "Total Paid";
            // 
            // txtRemTrans
            // 
            this.txtRemTrans.Enabled = false;
            this.txtRemTrans.Location = new System.Drawing.Point(482, 7);
            this.txtRemTrans.MaxLength = 2;
            this.txtRemTrans.Name = "txtRemTrans";
            this.txtRemTrans.Size = new System.Drawing.Size(45, 25);
            this.txtRemTrans.TabIndex = 4;
            this.txtRemTrans.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtRemTrans.Visible = false;
            // 
            // txtMaxInvs
            // 
            this.txtMaxInvs.Enabled = false;
            this.txtMaxInvs.Location = new System.Drawing.Point(895, 6);
            this.txtMaxInvs.MaxLength = 2;
            this.txtMaxInvs.Name = "txtMaxInvs";
            this.txtMaxInvs.Size = new System.Drawing.Size(25, 25);
            this.txtMaxInvs.TabIndex = 4;
            this.txtMaxInvs.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // lblMaxInv
            // 
            this.lblMaxInv.AutoSize = true;
            this.lblMaxInv.Location = new System.Drawing.Point(817, 10);
            this.lblMaxInv.Name = "lblMaxInv";
            this.lblMaxInv.Size = new System.Drawing.Size(74, 14);
            this.lblMaxInv.TabIndex = 0;
            this.lblMaxInv.Text = "Max Invoices";
            // 
            // txtAccountNo
            // 
            this.txtAccountNo.Enabled = false;
            this.txtAccountNo.Location = new System.Drawing.Point(71, 93);
            this.txtAccountNo.MaxLength = 20;
            this.txtAccountNo.Name = "txtAccountNo";
            this.txtAccountNo.RightToLeft = Wisej.Web.RightToLeft.No;
            this.txtAccountNo.Size = new System.Drawing.Size(136, 25);
            this.txtAccountNo.TabIndex = 4;
            this.txtAccountNo.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // lblAccount
            // 
            this.lblAccount.AutoSize = true;
            this.lblAccount.Location = new System.Drawing.Point(14, 98);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(55, 14);
            this.lblAccount.TabIndex = 11;
            this.lblAccount.Text = "Account#";
            // 
            // Text_VendName
            // 
            this.Text_VendName.Enabled = false;
            this.Text_VendName.Location = new System.Drawing.Point(159, 64);
            this.Text_VendName.Name = "Text_VendName";
            this.Text_VendName.Size = new System.Drawing.Size(368, 25);
            this.Text_VendName.TabIndex = 3;
            // 
            // Txt_VendNo
            // 
            this.Txt_VendNo.Enabled = false;
            this.Txt_VendNo.Location = new System.Drawing.Point(71, 64);
            this.Txt_VendNo.MaxLength = 10;
            this.Txt_VendNo.Name = "Txt_VendNo";
            this.Txt_VendNo.Size = new System.Drawing.Size(84, 25);
            this.Txt_VendNo.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "Vendor #";
            // 
            // lblVendor
            // 
            this.lblVendor.AutoSize = true;
            this.lblVendor.Location = new System.Drawing.Point(-84, 35);
            this.lblVendor.Name = "lblVendor";
            this.lblVendor.Size = new System.Drawing.Size(53, 14);
            this.lblVendor.TabIndex = 1;
            this.lblVendor.Text = "Vendor #";
            // 
            // lblSPName
            // 
            this.lblSPName.AutoSize = true;
            this.lblSPName.Font = new System.Drawing.Font("default", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblSPName.Location = new System.Drawing.Point(90, 13);
            this.lblSPName.Name = "lblSPName";
            this.lblSPName.Size = new System.Drawing.Size(69, 13);
            this.lblSPName.TabIndex = 0;
            this.lblSPName.Text = "Service Plan";
            // 
            // lblServicePlan
            // 
            this.lblServicePlan.AutoSize = true;
            this.lblServicePlan.Location = new System.Drawing.Point(14, 11);
            this.lblServicePlan.Name = "lblServicePlan";
            this.lblServicePlan.Size = new System.Drawing.Size(72, 14);
            this.lblServicePlan.TabIndex = 0;
            this.lblServicePlan.Text = "Service Plan";
            // 
            // lblService
            // 
            this.lblService.AutoSize = true;
            this.lblService.Location = new System.Drawing.Point(14, 40);
            this.lblService.Name = "lblService";
            this.lblService.Size = new System.Drawing.Size(45, 14);
            this.lblService.TabIndex = 0;
            this.lblService.Text = "Service";
            // 
            // gvwMonthQuestions
            // 
            this.gvwMonthQuestions.AllowUserToResizeColumns = false;
            this.gvwMonthQuestions.AllowUserToResizeRows = false;
            this.gvwMonthQuestions.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(183, 183, 183);
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwMonthQuestions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwMonthQuestions.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtMonthQuesCode,
            this.gvtMonthQues,
            this.gvkChk,
            this.gvtElecpayment,
            this.gvtUsageReq,
            this.gvtServicedate,
            this.gvtOverride,
            this.gvtAmtPaid,
            this.gvtBundle,
            this.gvtCheck,
            this.gvtChkDate,
            this.gvVendor,
            this.gvDelete,
            this.gvCaseact,
            this.gvPDOut});
            this.gvwMonthQuestions.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwMonthQuestions.MultiSelect = false;
            this.gvwMonthQuestions.Name = "gvwMonthQuestions";
            this.gvwMonthQuestions.RowHeadersWidth = 14;
            this.gvwMonthQuestions.RowTemplate.Height = 20;
            this.gvwMonthQuestions.RowTemplate.Resizable = Wisej.Web.DataGridViewTriState.True;
            this.gvwMonthQuestions.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwMonthQuestions.Size = new System.Drawing.Size(970, 283);
            this.gvwMonthQuestions.TabIndex = 5;
            this.gvwMonthQuestions.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvwMonthQuestions_CellValueChanged);
            this.gvwMonthQuestions.CellValidating += new Wisej.Web.DataGridViewCellValidatingEventHandler(this.gvwMonthQuestions_CellValidating);
            this.gvwMonthQuestions.SelectionChanged += new System.EventHandler(this.gvwMonthQuestions_SelectionChanged);
            this.gvwMonthQuestions.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwMonthQuestions_CellClick);
            this.gvwMonthQuestions.DataError += new Wisej.Web.DataGridViewDataErrorEventHandler(this.gvwMonthQuestions_DataError);
            // 
            // gvtMonthQuesCode
            // 
            this.gvtMonthQuesCode.HeaderText = "gvtMonthQuesCode";
            this.gvtMonthQuesCode.Name = "gvtMonthQuesCode";
            this.gvtMonthQuesCode.ShowInVisibilityMenu = false;
            this.gvtMonthQuesCode.Visible = false;
            this.gvtMonthQuesCode.Width = 10;
            // 
            // gvtMonthQues
            // 
            this.gvtMonthQues.HeaderText = "Month";
            this.gvtMonthQues.Name = "gvtMonthQues";
            this.gvtMonthQues.ReadOnly = true;
            this.gvtMonthQues.Width = 100;
            // 
            // gvkChk
            // 
            this.gvkChk.AllowHtml = true;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.CssStyle = "background-color:transparent;";
            dataGridViewCellStyle2.NullValue = false;
            this.gvkChk.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvkChk.HeaderText = " ";
            this.gvkChk.Name = "gvkChk";
            this.gvkChk.ShowInVisibilityMenu = false;
            this.gvkChk.Width = 35;
            // 
            // gvtElecpayment
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            this.gvtElecpayment.DefaultCellStyle = dataGridViewCellStyle3;
            this.gvtElecpayment.HeaderText = "Usage";
            this.gvtElecpayment.MaxInputLength = 11;
            this.gvtElecpayment.Name = "gvtElecpayment";
            this.gvtElecpayment.ReadOnly = true;
            this.gvtElecpayment.Width = 65;
            // 
            // gvtServicedate
            // 
            this.gvtServicedate.DefaultCellStyle = dataGridViewCellStyle5;
            this.gvtServicedate.HeaderText = "Service Date";
            this.gvtServicedate.Name = "gvtServicedate";
            this.gvtServicedate.ReadOnly = true;
            this.gvtServicedate.Width = 93;
            // 
            // gvtOverride
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.NullValue = null;
            this.gvtOverride.DefaultCellStyle = dataGridViewCellStyle6;
            this.gvtOverride.HeaderText = " ";
            this.gvtOverride.Name = "gvtOverride";
            this.gvtOverride.ReadOnly = true;
            this.gvtOverride.ShowInVisibilityMenu = false;
            this.gvtOverride.Width = 32;
            // 
            // gvtAmtPaid
            // 
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Padding = new Wisej.Web.Padding(0, 0, 17, 0);
            this.gvtAmtPaid.DefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Padding = new Wisej.Web.Padding(0, 0, 17, 0);
            this.gvtAmtPaid.HeaderStyle = dataGridViewCellStyle8;
            this.gvtAmtPaid.HeaderText = "Amount Paid";
            this.gvtAmtPaid.MaxInputLength = 11;
            this.gvtAmtPaid.Name = "gvtAmtPaid";
            this.gvtAmtPaid.ReadOnly = true;
            this.gvtAmtPaid.Width = 110;
            // 
            // gvtBundle
            // 
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Padding = new Wisej.Web.Padding(0, 0, 17, 0);
            this.gvtBundle.DefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.Padding = new Wisej.Web.Padding(0, 0, 17, 0);
            this.gvtBundle.HeaderStyle = dataGridViewCellStyle10;
            this.gvtBundle.HeaderText = "Bundle #";
            this.gvtBundle.Name = "gvtBundle";
            this.gvtBundle.ReadOnly = true;
            this.gvtBundle.Width = 74;
            // 
            // gvtCheck
            // 
            this.gvtCheck.HeaderText = "Check #";
            this.gvtCheck.Name = "gvtCheck";
            this.gvtCheck.ReadOnly = true;
            this.gvtCheck.Width = 90;
            // 
            // gvtChkDate
            // 
            this.gvtChkDate.DefaultCellStyle = dataGridViewCellStyle11;
            this.gvtChkDate.HeaderText = "Check Date";
            this.gvtChkDate.Name = "gvtChkDate";
            this.gvtChkDate.ReadOnly = true;
            this.gvtChkDate.Width = 88;
            // 
            // gvVendor
            // 
            this.gvVendor.HeaderText = "Vendor";
            this.gvVendor.Name = "gvVendor";
            this.gvVendor.Width = 140;
            // 
            // gvDelete
            // 
            dataGridViewCellStyle12.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.NullValue = null;
            this.gvDelete.DefaultCellStyle = dataGridViewCellStyle12;
            this.gvDelete.HeaderText = " ";
            this.gvDelete.Name = "gvDelete";
            this.gvDelete.ReadOnly = true;
            this.gvDelete.ShowInVisibilityMenu = false;
            this.gvDelete.Width = 25;
            // 
            // gvCaseact
            // 
            this.gvCaseact.HeaderText = "gvCaseact";
            this.gvCaseact.Name = "gvCaseact";
            this.gvCaseact.ShowInVisibilityMenu = false;
            this.gvCaseact.Visible = false;
            this.gvCaseact.Width = 10;
            // 
            // gvPDOut
            // 
            this.gvPDOut.HeaderText = "gvPDOut";
            this.gvPDOut.Name = "gvPDOut";
            this.gvPDOut.Visible = false;
            this.gvPDOut.Width = 40;
            // 
            // CmbSP
            // 
            this.CmbSP.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbSP.FormattingEnabled = true;
            this.CmbSP.Location = new System.Drawing.Point(71, 36);
            this.CmbSP.Name = "CmbSP";
            this.CmbSP.Size = new System.Drawing.Size(457, 25);
            this.CmbSP.TabIndex = 1;
            this.CmbSP.SelectedIndexChanged += new System.EventHandler(this.CmbSP_SelectedIndexChanged);
            // 
            // txtBalance
            // 
            this.txtBalance.Enabled = false;
            this.txtBalance.Location = new System.Drawing.Point(670, 64);
            this.txtBalance.MaxLength = 15;
            this.txtBalance.Name = "txtBalance";
            this.txtBalance.Size = new System.Drawing.Size(57, 25);
            this.txtBalance.TabIndex = 5;
            this.txtBalance.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // txtAmount
            // 
            this.txtAmount.Enabled = false;
            this.txtAmount.Location = new System.Drawing.Point(670, 8);
            this.txtAmount.MaxLength = 10;
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(57, 25);
            this.txtAmount.TabIndex = 4;
            this.txtAmount.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // gvtUsageReq
            // 
            this.gvtUsageReq.DefaultCellStyle = dataGridViewCellStyle4;
            this.gvtUsageReq.HeaderText = "Usage/Req.";
            this.gvtUsageReq.Name = "gvtUsageReq";
            this.gvtUsageReq.ReadOnly = true;
            this.gvtUsageReq.Width = 93;
            // 
            // CASE0016_Usage
            // 
            this.ClientSize = new System.Drawing.Size(970, 443);
            this.Controls.Add(this.panel3);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CASE0016_Usage";
            this.Text = "CASE0016_Form";
            this.panel5.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwMonthQuestions)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion
        private Button btnCancel;
        private Button btnSave;
        private Panel panel5;
        private TextBoxWithValidation txtAmount;
        private Label lblAward;
        private TextBoxWithValidation txtBalance;
        private Label label17;
        private Label label14;
        private Panel panel3;
        private ComboBoxEx CmbSP;
        private Label lblService;
        private Panel panel4;
        private Panel panel6;
        private Label lblSPName;
        private Label lblServicePlan;
        private DataGridViewEx gvwMonthQuestions;
        private DataGridViewTextBoxColumn gvtMonthQuesCode;
        private DataGridViewTextBoxColumn gvtMonthQues;
        private DataGridViewTextBoxColumn gvtElecpayment;
        private DataGridViewTextBoxColumn gvtAmtPaid;
        private DataGridViewTextBoxColumn gvtCheck;
        private Button btnInvoices;
        private DataGridViewCheckBoxColumn gvkChk;
        private DataGridViewImageColumn gvtOverride;
        private Label lblVendor;
        private DataGridViewTextBoxColumn gvCaseact;
        private TextBox Text_VendName;
        private TextBox Txt_VendNo;
        private Label label1;
        private TextBox txtAccountNo;
        private Label lblAccount;
        private TextBox txtMaxInvs;
        private Label lblMaxInv;
        private TextBox txtRemTrans;
        private DataGridViewImageColumn gvDelete;
        private TextBox txtPaid;
        private Label lblPaidAmt;
        private TextBox txtInvBal;
        private Label label3;
        private Label label2;
        private TextBox txtPaidInv;
        private DataGridViewTextBoxColumn gvtBundle;
        private Spacer spacer2;
        private Spacer spacer1;
        private DataGridViewTextBoxColumn gvPDOut;
        private TextBox txtScore;
        private Label label7;
        private DataGridViewTextBoxColumn gvVendor;
        private DataGridViewDateTimeColumn gvtServicedate;
        private DataGridViewDateTimeColumn gvtChkDate;
        private DataGridViewDateTimeColumn gvtUsageReq;
    }
}