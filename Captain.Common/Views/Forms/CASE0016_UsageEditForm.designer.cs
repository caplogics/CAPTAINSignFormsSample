using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class CASE0016_UsageEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CASE0016_UsageEditForm));
            this.Act_Date = new Wisej.Web.DateTimePicker();
            this.lblActivityDate = new Wisej.Web.Label();
            this.lblCost = new Wisej.Web.Label();
            this.Btn_CACancel = new Wisej.Web.Button();
            this.Btn_CASave = new Wisej.Web.Button();
            this.pnlSave = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.chkbPdOut = new Wisej.Web.CheckBox();
            this.lblFundSource = new Wisej.Web.Label();
            this.lblBudget = new Wisej.Web.Label();
            this.pnlParameters = new Wisej.Web.Panel();
            this.panel1 = new Wisej.Web.Panel();
            this.txtBalance1 = new Wisej.Web.TextBox();
            this.label5 = new Wisej.Web.Label();
            this.txtBudget = new Wisej.Web.TextBox();
            this.label4 = new Wisej.Web.Label();
            this.txtEnd = new Wisej.Web.TextBox();
            this.label2 = new Wisej.Web.Label();
            this.txtstart1 = new Wisej.Web.TextBox();
            this.label1 = new Wisej.Web.Label();
            this.spacer2 = new Wisej.Web.Spacer();
            this.panel2 = new Wisej.Web.Panel();
            this.lblAccount = new Wisej.Web.Label();
            this.txtAccountNo = new Wisej.Web.TextBox();
            this.cmbBilling = new Wisej.Web.ComboBox();
            this.lblBillingName = new Wisej.Web.Label();
            this.txtLast = new Wisej.Web.TextBox();
            this.txtFirst = new Wisej.Web.TextBox();
            this.lblLast = new Wisej.Web.Label();
            this.lblFirst = new Wisej.Web.Label();
            this.Txt_Cost = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.cmbBudget = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.cmbFundsource = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.pnlSave.SuspendLayout();
            this.pnlParameters.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Act_Date
            // 
            this.Act_Date.CustomFormat = "MM/dd/yyyy";
            this.Act_Date.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.Act_Date.Location = new System.Drawing.Point(114, 11);
            this.Act_Date.MinimumSize = new System.Drawing.Size(0, 25);
            this.Act_Date.Name = "Act_Date";
            this.Act_Date.ShowCheckBox = true;
            this.Act_Date.ShowToolTips = false;
            this.Act_Date.Size = new System.Drawing.Size(120, 25);
            this.Act_Date.TabIndex = 1;
            this.Act_Date.ValueChanged += new System.EventHandler(this.Act_Date_ValueChanged);
            // 
            // lblActivityDate
            // 
            this.lblActivityDate.AutoSize = true;
            this.lblActivityDate.Location = new System.Drawing.Point(10, 15);
            this.lblActivityDate.Name = "lblActivityDate";
            this.lblActivityDate.Size = new System.Drawing.Size(73, 14);
            this.lblActivityDate.TabIndex = 1;
            this.lblActivityDate.Text = "Service Date";
            // 
            // lblCost
            // 
            this.lblCost.Location = new System.Drawing.Point(10, 45);
            this.lblCost.Name = "lblCost";
            this.lblCost.RightToLeft = Wisej.Web.RightToLeft.No;
            this.lblCost.Size = new System.Drawing.Size(83, 17);
            this.lblCost.TabIndex = 1;
            this.lblCost.Text = "Amount Paid";
            // 
            // Btn_CACancel
            // 
            this.Btn_CACancel.AppearanceKey = "button-cancel";
            this.Btn_CACancel.Dock = Wisej.Web.DockStyle.Right;
            this.Btn_CACancel.Location = new System.Drawing.Point(350, 5);
            this.Btn_CACancel.Name = "Btn_CACancel";
            this.Btn_CACancel.Size = new System.Drawing.Size(60, 25);
            this.Btn_CACancel.TabIndex = 2;
            this.Btn_CACancel.Text = "&Cancel";
            this.Btn_CACancel.Click += new System.EventHandler(this.Btn_CACancel_Click);
            // 
            // Btn_CASave
            // 
            this.Btn_CASave.AppearanceKey = "button-ok";
            this.Btn_CASave.Dock = Wisej.Web.DockStyle.Right;
            this.Btn_CASave.Location = new System.Drawing.Point(287, 5);
            this.Btn_CASave.Name = "Btn_CASave";
            this.Btn_CASave.Size = new System.Drawing.Size(60, 25);
            this.Btn_CASave.TabIndex = 1;
            this.Btn_CASave.Text = "&Save";
            this.Btn_CASave.Click += new System.EventHandler(this.Btn_CASave_Click);
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.Btn_CASave);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.Btn_CACancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 359);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(425, 35);
            this.pnlSave.TabIndex = 2;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(347, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // chkbPdOut
            // 
            this.chkbPdOut.AutoSize = false;
            this.chkbPdOut.Location = new System.Drawing.Point(111, 134);
            this.chkbPdOut.Name = "chkbPdOut";
            this.chkbPdOut.Size = new System.Drawing.Size(166, 21);
            this.chkbPdOut.TabIndex = 5;
            this.chkbPdOut.Text = "Pd outside of system";
            // 
            // lblFundSource
            // 
            this.lblFundSource.AutoSize = true;
            this.lblFundSource.Location = new System.Drawing.Point(10, 75);
            this.lblFundSource.MinimumSize = new System.Drawing.Size(0, 16);
            this.lblFundSource.Name = "lblFundSource";
            this.lblFundSource.Size = new System.Drawing.Size(89, 16);
            this.lblFundSource.TabIndex = 43;
            this.lblFundSource.Text = "Funding Source";
            // 
            // lblBudget
            // 
            this.lblBudget.AutoSize = true;
            this.lblBudget.Location = new System.Drawing.Point(10, 105);
            this.lblBudget.MinimumSize = new System.Drawing.Size(0, 16);
            this.lblBudget.Name = "lblBudget";
            this.lblBudget.Size = new System.Drawing.Size(43, 16);
            this.lblBudget.TabIndex = 45;
            this.lblBudget.Text = "Budget";
            // 
            // pnlParameters
            // 
            this.pnlParameters.Controls.Add(this.panel1);
            this.pnlParameters.Controls.Add(this.spacer2);
            this.pnlParameters.Controls.Add(this.panel2);
            this.pnlParameters.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlParameters.Location = new System.Drawing.Point(0, 0);
            this.pnlParameters.Name = "pnlParameters";
            this.pnlParameters.Padding = new Wisej.Web.Padding(5, 0, 5, 3);
            this.pnlParameters.Size = new System.Drawing.Size(425, 359);
            this.pnlParameters.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.txtBalance1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtBudget);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtEnd);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtstart1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(5, 288);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(415, 68);
            this.panel1.TabIndex = 2;
            // 
            // txtBalance1
            // 
            this.txtBalance1.BackColor = System.Drawing.Color.Transparent;
            this.txtBalance1.BorderStyle = Wisej.Web.BorderStyle.None;
            this.txtBalance1.Enabled = false;
            this.txtBalance1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtBalance1.Location = new System.Drawing.Point(300, 35);
            this.txtBalance1.Name = "txtBalance1";
            this.txtBalance1.Size = new System.Drawing.Size(84, 25);
            this.txtBalance1.TabIndex = 36;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label5.Location = new System.Drawing.Point(238, 39);
            this.label5.MinimumSize = new System.Drawing.Size(0, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 16);
            this.label5.TabIndex = 35;
            this.label5.Text = "Balance :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBudget
            // 
            this.txtBudget.BackColor = System.Drawing.Color.Transparent;
            this.txtBudget.BorderStyle = Wisej.Web.BorderStyle.None;
            this.txtBudget.Enabled = false;
            this.txtBudget.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtBudget.Location = new System.Drawing.Point(120, 34);
            this.txtBudget.Name = "txtBudget";
            this.txtBudget.Size = new System.Drawing.Size(85, 25);
            this.txtBudget.TabIndex = 34;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label4.Location = new System.Drawing.Point(10, 39);
            this.label4.MinimumSize = new System.Drawing.Size(0, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 16);
            this.label4.TabIndex = 33;
            this.label4.Text = "Budget :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtEnd
            // 
            this.txtEnd.BackColor = System.Drawing.Color.Transparent;
            this.txtEnd.BorderStyle = Wisej.Web.BorderStyle.None;
            this.txtEnd.Enabled = false;
            this.txtEnd.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtEnd.Location = new System.Drawing.Point(300, 5);
            this.txtEnd.Name = "txtEnd";
            this.txtEnd.Size = new System.Drawing.Size(84, 25);
            this.txtEnd.TabIndex = 32;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(238, 9);
            this.label2.MinimumSize = new System.Drawing.Size(0, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 16);
            this.label2.TabIndex = 31;
            this.label2.Text = "End Date :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtstart1
            // 
            this.txtstart1.BackColor = System.Drawing.Color.Transparent;
            this.txtstart1.BorderStyle = Wisej.Web.BorderStyle.None;
            this.txtstart1.Enabled = false;
            this.txtstart1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtstart1.Location = new System.Drawing.Point(120, 5);
            this.txtstart1.Name = "txtstart1";
            this.txtstart1.Size = new System.Drawing.Size(85, 25);
            this.txtstart1.TabIndex = 30;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.MinimumSize = new System.Drawing.Size(105, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Budget Start Date:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Top;
            this.spacer2.Location = new System.Drawing.Point(5, 285);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(415, 3);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cmbBilling);
            this.panel2.Controls.Add(this.lblBillingName);
            this.panel2.Controls.Add(this.txtLast);
            this.panel2.Controls.Add(this.txtFirst);
            this.panel2.Controls.Add(this.lblLast);
            this.panel2.Controls.Add(this.lblFirst);
            this.panel2.Controls.Add(this.lblAccount);
            this.panel2.Controls.Add(this.txtAccountNo);
            this.panel2.Controls.Add(this.lblFundSource);
            this.panel2.Controls.Add(this.chkbPdOut);
            this.panel2.Controls.Add(this.lblCost);
            this.panel2.Controls.Add(this.Txt_Cost);
            this.panel2.Controls.Add(this.cmbBudget);
            this.panel2.Controls.Add(this.cmbFundsource);
            this.panel2.Controls.Add(this.Act_Date);
            this.panel2.Controls.Add(this.lblActivityDate);
            this.panel2.Controls.Add(this.lblBudget);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(5, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(415, 285);
            this.panel2.TabIndex = 1;
            // 
            // lblAccount
            // 
            this.lblAccount.AutoSize = true;
            this.lblAccount.Location = new System.Drawing.Point(10, 166);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(55, 14);
            this.lblAccount.TabIndex = 47;
            this.lblAccount.Text = "Account#";
            // 
            // txtAccountNo
            // 
            this.txtAccountNo.Location = new System.Drawing.Point(114, 162);
            this.txtAccountNo.MaxLength = 40;
            this.txtAccountNo.Name = "txtAccountNo";
            this.txtAccountNo.RightToLeft = Wisej.Web.RightToLeft.No;
            this.txtAccountNo.Size = new System.Drawing.Size(270, 25);
            this.txtAccountNo.TabIndex = 46;
            this.txtAccountNo.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // cmbBilling
            // 
            this.cmbBilling.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbBilling.FormattingEnabled = true;
            this.cmbBilling.Location = new System.Drawing.Point(114, 192);
            this.cmbBilling.Name = "cmbBilling";
            this.cmbBilling.Size = new System.Drawing.Size(270, 25);
            this.cmbBilling.TabIndex = 48;
            this.cmbBilling.SelectedIndexChanged += new System.EventHandler(this.cmbBilling_SelectedIndexChanged);
            // 
            // lblBillingName
            // 
            this.lblBillingName.AutoSize = true;
            this.lblBillingName.Location = new System.Drawing.Point(10, 198);
            this.lblBillingName.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblBillingName.Name = "lblBillingName";
            this.lblBillingName.Size = new System.Drawing.Size(72, 18);
            this.lblBillingName.TabIndex = 49;
            this.lblBillingName.Text = "Billing Name";
            // 
            // txtLast
            // 
            this.txtLast.Enabled = false;
            this.txtLast.Location = new System.Drawing.Point(114, 252);
            this.txtLast.MaxLength = 20;
            this.txtLast.Name = "txtLast";
            this.txtLast.Size = new System.Drawing.Size(270, 25);
            this.txtLast.TabIndex = 53;
            // 
            // txtFirst
            // 
            this.txtFirst.Enabled = false;
            this.txtFirst.Location = new System.Drawing.Point(114, 222);
            this.txtFirst.MaxLength = 20;
            this.txtFirst.Name = "txtFirst";
            this.txtFirst.Size = new System.Drawing.Size(270, 25);
            this.txtFirst.TabIndex = 52;
            // 
            // lblLast
            // 
            this.lblLast.AutoSize = true;
            this.lblLast.Location = new System.Drawing.Point(10, 258);
            this.lblLast.Name = "lblLast";
            this.lblLast.Size = new System.Drawing.Size(62, 14);
            this.lblLast.TabIndex = 50;
            this.lblLast.Text = "Last Name";
            // 
            // lblFirst
            // 
            this.lblFirst.AutoSize = true;
            this.lblFirst.Location = new System.Drawing.Point(10, 228);
            this.lblFirst.Name = "lblFirst";
            this.lblFirst.Size = new System.Drawing.Size(63, 14);
            this.lblFirst.TabIndex = 51;
            this.lblFirst.Text = "First Name";
            // 
            // Txt_Cost
            // 
            this.Txt_Cost.Location = new System.Drawing.Point(114, 41);
            this.Txt_Cost.MaxLength = 9;
            this.Txt_Cost.Name = "Txt_Cost";
            this.Txt_Cost.Size = new System.Drawing.Size(120, 25);
            this.Txt_Cost.TabIndex = 2;
            this.Txt_Cost.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // cmbBudget
            // 
            this.cmbBudget.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbBudget.Location = new System.Drawing.Point(114, 101);
            this.cmbBudget.Name = "cmbBudget";
            this.cmbBudget.Size = new System.Drawing.Size(270, 25);
            this.cmbBudget.TabIndex = 4;
            this.cmbBudget.SelectedIndexChanged += new System.EventHandler(this.cmbBudget_SelectedIndexChanged);
            // 
            // cmbFundsource
            // 
            this.cmbFundsource.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbFundsource.Location = new System.Drawing.Point(114, 71);
            this.cmbFundsource.Name = "cmbFundsource";
            this.cmbFundsource.Size = new System.Drawing.Size(270, 25);
            this.cmbFundsource.TabIndex = 3;
            this.cmbFundsource.SelectedIndexChanged += new System.EventHandler(this.cmbFundsource_SelectedIndexChanged);
            // 
            // CASE0016_UsageEditForm
            // 
            this.ClientSize = new System.Drawing.Size(425, 394);
            this.Controls.Add(this.pnlParameters);
            this.Controls.Add(this.pnlSave);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CASE0016_UsageEditForm";
            this.Text = "CASE0016_UsageEditForm";
            this.pnlSave.ResumeLayout(false);
            this.pnlParameters.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion

        private DateTimePicker Act_Date;
        private Label lblActivityDate;
        private TextBoxWithValidation Txt_Cost;
        private Label lblCost;
        private Button Btn_CACancel;
        private Button Btn_CASave;
        private Panel pnlSave;
        private Spacer spacer1;
        private CheckBox chkbPdOut;
        private Label lblFundSource;
        private ComboBoxEx cmbFundsource;
        private ComboBoxEx cmbBudget;
        private Label lblBudget;
        private Panel pnlParameters;
        private Panel panel1;
        private Label label1;
        private Panel panel2;
        private TextBox txtstart1;
        private Label label2;
        private TextBox txtEnd;
        private Label label4;
        private TextBox txtBudget;
        private Label label5;
        private TextBox txtBalance1;
        private Spacer spacer2;
        private Label lblAccount;
        private TextBox txtAccountNo;
        private ComboBox cmbBilling;
        private Label lblBillingName;
        private TextBox txtLast;
        private TextBox txtFirst;
        private Label lblLast;
        private Label lblFirst;
    }
}