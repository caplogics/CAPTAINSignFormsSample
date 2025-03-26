using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class CASE0026Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CASE0026Form));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlDetails = new Wisej.Web.Panel();
            this.lblReqStateNo = new Wisej.Web.Label();
            this.lblReqEndDate = new Wisej.Web.Label();
            this.lblContractNumber = new Wisej.Web.Label();
            this.txtContractNumber = new Wisej.Web.TextBox();
            this.txtBDetailtype = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblBDetailtype = new Wisej.Web.Label();
            this.btnCalculateBal = new Wisej.Web.Button();
            this.lblFndCd = new Wisej.Web.Label();
            this.txtFundCd = new Wisej.Web.TextBox();
            this.txtDescription = new Wisej.Web.TextBox();
            this.lblRemaining = new Wisej.Web.Label();
            this.txtRemaining = new Wisej.Web.TextBox();
            this.lblDesc = new Wisej.Web.Label();
            this.txtExpended = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblExpend = new Wisej.Web.Label();
            this.lblAllowPosting = new Wisej.Web.Label();
            this.chkAllowPost = new Wisej.Web.CheckBox();
            this.txtTotcommit = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblCommit = new Wisej.Web.Label();
            this.lblbudget = new Wisej.Web.Label();
            this.txtBudget = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblReqFundSource = new Wisej.Web.Label();
            this.lblFundSource = new Wisej.Web.Label();
            this.cmbFundsource = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.lblEndDt = new Wisej.Web.Label();
            this.lblReqStartDte = new Wisej.Web.Label();
            this.dtEndDt = new Wisej.Web.DateTimePicker();
            this.lblStartDt = new Wisej.Web.Label();
            this.lblReqBenDeatils = new Wisej.Web.Label();
            this.dtStardt = new Wisej.Web.DateTimePicker();
            this.pnlSave = new Wisej.Web.Panel();
            this.btnSave = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.cmbProgram = new Wisej.Web.ComboBox();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlDetails);
            this.pnlCompleteForm.Controls.Add(this.pnlSave);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(547, 281);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlDetails
            // 
            this.pnlDetails.Controls.Add(this.lblReqStateNo);
            this.pnlDetails.Controls.Add(this.lblReqEndDate);
            this.pnlDetails.Controls.Add(this.lblContractNumber);
            this.pnlDetails.Controls.Add(this.txtContractNumber);
            this.pnlDetails.Controls.Add(this.txtBDetailtype);
            this.pnlDetails.Controls.Add(this.lblBDetailtype);
            this.pnlDetails.Controls.Add(this.btnCalculateBal);
            this.pnlDetails.Controls.Add(this.lblFndCd);
            this.pnlDetails.Controls.Add(this.txtFundCd);
            this.pnlDetails.Controls.Add(this.txtDescription);
            this.pnlDetails.Controls.Add(this.lblRemaining);
            this.pnlDetails.Controls.Add(this.txtRemaining);
            this.pnlDetails.Controls.Add(this.lblDesc);
            this.pnlDetails.Controls.Add(this.txtExpended);
            this.pnlDetails.Controls.Add(this.lblExpend);
            this.pnlDetails.Controls.Add(this.lblAllowPosting);
            this.pnlDetails.Controls.Add(this.chkAllowPost);
            this.pnlDetails.Controls.Add(this.txtTotcommit);
            this.pnlDetails.Controls.Add(this.lblCommit);
            this.pnlDetails.Controls.Add(this.lblbudget);
            this.pnlDetails.Controls.Add(this.txtBudget);
            this.pnlDetails.Controls.Add(this.lblReqFundSource);
            this.pnlDetails.Controls.Add(this.lblFundSource);
            this.pnlDetails.Controls.Add(this.cmbFundsource);
            this.pnlDetails.Controls.Add(this.lblEndDt);
            this.pnlDetails.Controls.Add(this.lblReqStartDte);
            this.pnlDetails.Controls.Add(this.dtEndDt);
            this.pnlDetails.Controls.Add(this.lblStartDt);
            this.pnlDetails.Controls.Add(this.lblReqBenDeatils);
            this.pnlDetails.Controls.Add(this.dtStardt);
            this.pnlDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlDetails.Location = new System.Drawing.Point(0, 0);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Size = new System.Drawing.Size(547, 246);
            this.pnlDetails.TabIndex = 1;
            // 
            // lblReqStateNo
            // 
            this.lblReqStateNo.AutoSize = true;
            this.lblReqStateNo.ForeColor = System.Drawing.Color.Red;
            this.lblReqStateNo.Location = new System.Drawing.Point(264, 98);
            this.lblReqStateNo.Name = "lblReqStateNo";
            this.lblReqStateNo.Size = new System.Drawing.Size(9, 14);
            this.lblReqStateNo.TabIndex = 28;
            this.lblReqStateNo.Text = "*";
            this.lblReqStateNo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblReqEndDate
            // 
            this.lblReqEndDate.AutoSize = true;
            this.lblReqEndDate.ForeColor = System.Drawing.Color.Red;
            this.lblReqEndDate.Location = new System.Drawing.Point(8, 154);
            this.lblReqEndDate.Name = "lblReqEndDate";
            this.lblReqEndDate.Size = new System.Drawing.Size(9, 14);
            this.lblReqEndDate.TabIndex = 28;
            this.lblReqEndDate.Text = "*";
            this.lblReqEndDate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblContractNumber
            // 
            this.lblContractNumber.AutoSize = true;
            this.lblContractNumber.Location = new System.Drawing.Point(273, 102);
            this.lblContractNumber.MinimumSize = new System.Drawing.Size(135, 18);
            this.lblContractNumber.Name = "lblContractNumber";
            this.lblContractNumber.Size = new System.Drawing.Size(135, 18);
            this.lblContractNumber.TabIndex = 0;
            this.lblContractNumber.Text = "State Contract Number";
            // 
            // txtContractNumber
            // 
            this.txtContractNumber.Location = new System.Drawing.Point(414, 98);
            this.txtContractNumber.MaxLength = 11;
            this.txtContractNumber.Name = "txtContractNumber";
            this.txtContractNumber.Size = new System.Drawing.Size(105, 25);
            this.txtContractNumber.TabIndex = 9;
            // 
            // txtBDetailtype
            // 
            this.txtBDetailtype.Location = new System.Drawing.Point(169, 98);
            this.txtBDetailtype.MaxLength = 1;
            this.txtBDetailtype.Name = "txtBDetailtype";
            this.txtBDetailtype.Size = new System.Drawing.Size(26, 25);
            this.txtBDetailtype.TabIndex = 4;
            // 
            // lblBDetailtype
            // 
            this.lblBDetailtype.AutoSize = true;
            this.lblBDetailtype.Location = new System.Drawing.Point(16, 102);
            this.lblBDetailtype.MinimumSize = new System.Drawing.Size(150, 18);
            this.lblBDetailtype.Name = "lblBDetailtype";
            this.lblBDetailtype.Size = new System.Drawing.Size(150, 18);
            this.lblBDetailtype.TabIndex = 0;
            this.lblBDetailtype.Text = "State Benefit Detail Type";
            // 
            // btnCalculateBal
            // 
            this.btnCalculateBal.Location = new System.Drawing.Point(400, 214);
            this.btnCalculateBal.Name = "btnCalculateBal";
            this.btnCalculateBal.Size = new System.Drawing.Size(119, 25);
            this.btnCalculateBal.TabIndex = 13;
            this.btnCalculateBal.Text = "Calculate &Balance";
            this.btnCalculateBal.Visible = false;
            this.btnCalculateBal.Click += new System.EventHandler(this.btnCalculateBal_Click);
            // 
            // lblFndCd
            // 
            this.lblFndCd.AutoSize = true;
            this.lblFndCd.Location = new System.Drawing.Point(17, 73);
            this.lblFndCd.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblFndCd.Name = "lblFndCd";
            this.lblFndCd.Size = new System.Drawing.Size(64, 18);
            this.lblFndCd.TabIndex = 0;
            this.lblFndCd.Text = "Fund Code";
            // 
            // txtFundCd
            // 
            this.txtFundCd.Location = new System.Drawing.Point(113, 69);
            this.txtFundCd.MaxLength = 35;
            this.txtFundCd.Name = "txtFundCd";
            this.txtFundCd.Size = new System.Drawing.Size(406, 25);
            this.txtFundCd.TabIndex = 3;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(113, 39);
            this.txtDescription.MaxLength = 40;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(406, 25);
            this.txtDescription.TabIndex = 2;
            // 
            // lblRemaining
            // 
            this.lblRemaining.AutoSize = true;
            this.lblRemaining.Location = new System.Drawing.Point(346, 186);
            this.lblRemaining.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblRemaining.Name = "lblRemaining";
            this.lblRemaining.Size = new System.Drawing.Size(62, 18);
            this.lblRemaining.TabIndex = 20;
            this.lblRemaining.Text = "Remaining";
            // 
            // txtRemaining
            // 
            this.txtRemaining.Location = new System.Drawing.Point(414, 182);
            this.txtRemaining.MaxLength = 15;
            this.txtRemaining.Name = "txtRemaining";
            this.txtRemaining.ReadOnly = true;
            this.txtRemaining.Size = new System.Drawing.Size(105, 25);
            this.txtRemaining.TabIndex = 12;
            this.txtRemaining.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Location = new System.Drawing.Point(17, 43);
            this.lblDesc.MinimumSize = new System.Drawing.Size(70, 18);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(70, 18);
            this.lblDesc.TabIndex = 0;
            this.lblDesc.Text = "Description";
            // 
            // txtExpended
            // 
            this.txtExpended.Location = new System.Drawing.Point(414, 154);
            this.txtExpended.MaxLength = 15;
            this.txtExpended.Name = "txtExpended";
            this.txtExpended.ReadOnly = true;
            this.txtExpended.Size = new System.Drawing.Size(105, 25);
            this.txtExpended.TabIndex = 11;
            this.txtExpended.Text = "0.00";
            this.txtExpended.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // lblExpend
            // 
            this.lblExpend.AutoSize = true;
            this.lblExpend.Location = new System.Drawing.Point(349, 158);
            this.lblExpend.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblExpend.Name = "lblExpend";
            this.lblExpend.Size = new System.Drawing.Size(59, 18);
            this.lblExpend.TabIndex = 20;
            this.lblExpend.Text = "Expended";
            // 
            // lblAllowPosting
            // 
            this.lblAllowPosting.AutoSize = true;
            this.lblAllowPosting.Location = new System.Drawing.Point(17, 217);
            this.lblAllowPosting.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblAllowPosting.Name = "lblAllowPosting";
            this.lblAllowPosting.Size = new System.Drawing.Size(83, 18);
            this.lblAllowPosting.TabIndex = 2;
            this.lblAllowPosting.Text = "Allow Postings";
            // 
            // chkAllowPost
            // 
            this.chkAllowPost.CheckState = Wisej.Web.CheckState.Checked;
            this.chkAllowPost.Location = new System.Drawing.Point(113, 215);
            this.chkAllowPost.Name = "chkAllowPost";
            this.chkAllowPost.Size = new System.Drawing.Size(32, 21);
            this.chkAllowPost.TabIndex = 8;
            // 
            // txtTotcommit
            // 
            this.txtTotcommit.Location = new System.Drawing.Point(414, 126);
            this.txtTotcommit.MaxLength = 15;
            this.txtTotcommit.Name = "txtTotcommit";
            this.txtTotcommit.ReadOnly = true;
            this.txtTotcommit.Size = new System.Drawing.Size(105, 25);
            this.txtTotcommit.TabIndex = 10;
            this.txtTotcommit.Text = "0.00";
            this.txtTotcommit.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // lblCommit
            // 
            this.lblCommit.AutoSize = true;
            this.lblCommit.Location = new System.Drawing.Point(338, 130);
            this.lblCommit.MinimumSize = new System.Drawing.Size(70, 18);
            this.lblCommit.Name = "lblCommit";
            this.lblCommit.Size = new System.Drawing.Size(70, 18);
            this.lblCommit.TabIndex = 19;
            this.lblCommit.Text = "Committed";
            // 
            // lblbudget
            // 
            this.lblbudget.AutoSize = true;
            this.lblbudget.Location = new System.Drawing.Point(17, 186);
            this.lblbudget.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblbudget.Name = "lblbudget";
            this.lblbudget.Size = new System.Drawing.Size(43, 18);
            this.lblbudget.TabIndex = 2;
            this.lblbudget.Text = "Budget";
            // 
            // txtBudget
            // 
            this.txtBudget.Location = new System.Drawing.Point(113, 182);
            this.txtBudget.MaxLength = 14;
            this.txtBudget.Name = "txtBudget";
            this.txtBudget.Size = new System.Drawing.Size(116, 25);
            this.txtBudget.TabIndex = 7;
            this.txtBudget.Text = "0.00";
            this.txtBudget.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtBudget.Leave += new System.EventHandler(this.txtBudget_Leave);
            // 
            // lblReqFundSource
            // 
            this.lblReqFundSource.AutoSize = true;
            this.lblReqFundSource.ForeColor = System.Drawing.Color.Red;
            this.lblReqFundSource.Location = new System.Drawing.Point(8, 10);
            this.lblReqFundSource.Name = "lblReqFundSource";
            this.lblReqFundSource.Size = new System.Drawing.Size(9, 14);
            this.lblReqFundSource.TabIndex = 28;
            this.lblReqFundSource.Text = "*";
            this.lblReqFundSource.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblFundSource
            // 
            this.lblFundSource.AutoSize = true;
            this.lblFundSource.Location = new System.Drawing.Point(17, 14);
            this.lblFundSource.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblFundSource.Name = "lblFundSource";
            this.lblFundSource.Size = new System.Drawing.Size(89, 18);
            this.lblFundSource.TabIndex = 0;
            this.lblFundSource.Text = "Funding Source";
            // 
            // cmbFundsource
            // 
            this.cmbFundsource.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbFundsource.FormattingEnabled = true;
            this.cmbFundsource.Location = new System.Drawing.Point(113, 10);
            this.cmbFundsource.Name = "cmbFundsource";
            this.cmbFundsource.Size = new System.Drawing.Size(406, 25);
            this.cmbFundsource.TabIndex = 1;
            this.cmbFundsource.SelectedIndexChanged += new System.EventHandler(this.cmbFundsource_SelectedIndexChanged);
            // 
            // lblEndDt
            // 
            this.lblEndDt.AutoSize = true;
            this.lblEndDt.Location = new System.Drawing.Point(17, 158);
            this.lblEndDt.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblEndDt.Name = "lblEndDt";
            this.lblEndDt.Size = new System.Drawing.Size(54, 18);
            this.lblEndDt.TabIndex = 0;
            this.lblEndDt.Text = "End Date";
            // 
            // lblReqStartDte
            // 
            this.lblReqStartDte.AutoSize = true;
            this.lblReqStartDte.ForeColor = System.Drawing.Color.Red;
            this.lblReqStartDte.Location = new System.Drawing.Point(8, 126);
            this.lblReqStartDte.Name = "lblReqStartDte";
            this.lblReqStartDte.Size = new System.Drawing.Size(9, 14);
            this.lblReqStartDte.TabIndex = 28;
            this.lblReqStartDte.Text = "*";
            this.lblReqStartDte.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // dtEndDt
            // 
            this.dtEndDt.CustomFormat = "MM/dd/yyyy";
            this.dtEndDt.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtEndDt.Location = new System.Drawing.Point(113, 154);
            this.dtEndDt.MinimumSize = new System.Drawing.Size(0, 25);
            this.dtEndDt.Name = "dtEndDt";
            this.dtEndDt.ShowCheckBox = true;
            this.dtEndDt.ShowToolTips = false;
            this.dtEndDt.Size = new System.Drawing.Size(116, 25);
            this.dtEndDt.TabIndex = 6;
            // 
            // lblStartDt
            // 
            this.lblStartDt.AutoSize = true;
            this.lblStartDt.Location = new System.Drawing.Point(17, 130);
            this.lblStartDt.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblStartDt.Name = "lblStartDt";
            this.lblStartDt.Size = new System.Drawing.Size(58, 18);
            this.lblStartDt.TabIndex = 0;
            this.lblStartDt.Text = "Start Date";
            // 
            // lblReqBenDeatils
            // 
            this.lblReqBenDeatils.AutoSize = true;
            this.lblReqBenDeatils.ForeColor = System.Drawing.Color.Red;
            this.lblReqBenDeatils.Location = new System.Drawing.Point(8, 96);
            this.lblReqBenDeatils.Name = "lblReqBenDeatils";
            this.lblReqBenDeatils.Size = new System.Drawing.Size(9, 14);
            this.lblReqBenDeatils.TabIndex = 28;
            this.lblReqBenDeatils.Text = "*";
            this.lblReqBenDeatils.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // dtStardt
            // 
            this.dtStardt.CustomFormat = "MM/dd/yyyy";
            this.dtStardt.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtStardt.Location = new System.Drawing.Point(113, 126);
            this.dtStardt.MinimumSize = new System.Drawing.Size(0, 25);
            this.dtStardt.Name = "dtStardt";
            this.dtStardt.ShowCheckBox = true;
            this.dtStardt.ShowToolTips = false;
            this.dtStardt.Size = new System.Drawing.Size(116, 25);
            this.dtStardt.TabIndex = 5;
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnSave);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 246);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(547, 35);
            this.pnlSave.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(379, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(454, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-cancel";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(457, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbProgram
            // 
            this.cmbProgram.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbProgram.FormattingEnabled = true;
            this.cmbProgram.Location = new System.Drawing.Point(100, 44);
            this.cmbProgram.Name = "cmbProgram";
            this.cmbProgram.Size = new System.Drawing.Size(406, 25);
            this.cmbProgram.TabIndex = 2;
            // 
            // CASE0026Form
            // 
            this.ClientSize = new System.Drawing.Size(547, 281);
            this.Controls.Add(this.pnlCompleteForm);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CASE0026Form";
            this.Text = "EMS30010Form";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "tlHelp";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.FormClosed += new Wisej.Web.FormClosedEventHandler(this.ADMN0023Form_FormClosed);
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.CASE0026Form_ToolClick);
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.pnlDetails.PerformLayout();
            this.pnlSave.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Panel pnlCompleteForm;
        private TextBoxWithValidation txtExpended;
        private Label lblExpend;
        private Label lblCommit;
        private TextBoxWithValidation txtTotcommit;
        private Panel pnlDetails;
        private Button btnCancel;
        private Button btnSave;
        private Panel pnlSave;
        private Label lblEndDt;
        private Label lblReqStartDte;
        private DateTimePicker dtEndDt;
        private Label lblStartDt;
        private Label lblReqBenDeatils;
        private DateTimePicker dtStardt;
        private Label lblAllowPosting;
        private CheckBox chkAllowPost;
        private Label lblbudget;
        private TextBoxWithValidation txtBudget;
        private Label lblReqFundSource;
        private Label lblFundSource;
        private ComboBoxEx cmbFundsource;
        private Label lblDesc;
        private Label lblRemaining;
        private TextBox txtRemaining;
        private ComboBox cmbProgram;
        private TextBox txtDescription;
        private Label lblFndCd;
        private TextBox txtFundCd;
        private Button btnCalculateBal;
        private Label lblContractNumber;
        private TextBox txtContractNumber;
        private TextBoxWithValidation txtBDetailtype;
        private Label lblBDetailtype;
        private Label lblReqStateNo;
        private Label lblReqEndDate;
        private Spacer spacer1;
    }
}