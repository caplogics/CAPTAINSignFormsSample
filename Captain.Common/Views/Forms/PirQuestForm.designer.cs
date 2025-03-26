using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class PirQuestForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PirQuestForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.pnlDetails = new Wisej.Web.Panel();
            this.label5 = new Wisej.Web.Label();
            this.txtFundType = new Wisej.Web.TextBox();
            this.btnFundType = new Wisej.Web.Button();
            this.chkBold = new Wisej.Web.CheckBox();
            this.chkAttnday = new Wisej.Web.CheckBox();
            this.lblPosition = new Wisej.Web.Label();
            this.chkStatus = new Wisej.Web.CheckBox();
            this.label8 = new Wisej.Web.Label();
            this.label7 = new Wisej.Web.Label();
            this.label6 = new Wisej.Web.Label();
            this.label4 = new Wisej.Web.Label();
            this.label3 = new Wisej.Web.Label();
            this.label2 = new Wisej.Web.Label();
            this.label1 = new Wisej.Web.Label();
            this.lblreqCity = new Wisej.Web.Label();
            this.lblDesc = new Wisej.Web.Label();
            this.txtDesc = new Wisej.Web.TextBox();
            this.lblQType = new Wisej.Web.Label();
            this.cmbQuesType = new Wisej.Web.ComboBox();
            this.lblFundType = new Wisej.Web.Label();
            this.cmbSection = new Wisej.Web.ComboBox();
            this.lblSection = new Wisej.Web.Label();
            this.lblQSection = new Wisej.Web.Label();
            this.txtQuesSection = new Wisej.Web.TextBox();
            this.lblQSCode = new Wisej.Web.Label();
            this.lblQCode = new Wisej.Web.Label();
            this.lblQNo = new Wisej.Web.Label();
            this.pnlSave = new Wisej.Web.Panel();
            this.btnSave = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.txtPosition = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtSeqCode = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtQuesCode = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtQuesNo = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.pnlDetails.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDetails
            // 
            this.pnlDetails.Controls.Add(this.label5);
            this.pnlDetails.Controls.Add(this.txtFundType);
            this.pnlDetails.Controls.Add(this.btnFundType);
            this.pnlDetails.Controls.Add(this.chkBold);
            this.pnlDetails.Controls.Add(this.chkAttnday);
            this.pnlDetails.Controls.Add(this.lblPosition);
            this.pnlDetails.Controls.Add(this.txtPosition);
            this.pnlDetails.Controls.Add(this.chkStatus);
            this.pnlDetails.Controls.Add(this.label8);
            this.pnlDetails.Controls.Add(this.label7);
            this.pnlDetails.Controls.Add(this.label6);
            this.pnlDetails.Controls.Add(this.label4);
            this.pnlDetails.Controls.Add(this.label3);
            this.pnlDetails.Controls.Add(this.label2);
            this.pnlDetails.Controls.Add(this.label1);
            this.pnlDetails.Controls.Add(this.lblreqCity);
            this.pnlDetails.Controls.Add(this.lblDesc);
            this.pnlDetails.Controls.Add(this.txtDesc);
            this.pnlDetails.Controls.Add(this.lblQType);
            this.pnlDetails.Controls.Add(this.cmbQuesType);
            this.pnlDetails.Controls.Add(this.lblFundType);
            this.pnlDetails.Controls.Add(this.cmbSection);
            this.pnlDetails.Controls.Add(this.lblSection);
            this.pnlDetails.Controls.Add(this.lblQSection);
            this.pnlDetails.Controls.Add(this.txtQuesSection);
            this.pnlDetails.Controls.Add(this.txtSeqCode);
            this.pnlDetails.Controls.Add(this.lblQSCode);
            this.pnlDetails.Controls.Add(this.lblQCode);
            this.pnlDetails.Controls.Add(this.txtQuesCode);
            this.pnlDetails.Controls.Add(this.txtQuesNo);
            this.pnlDetails.Controls.Add(this.lblQNo);
            this.pnlDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlDetails.Location = new System.Drawing.Point(0, 0);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Size = new System.Drawing.Size(558, 229);
            this.pnlDetails.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(338, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(6, 11);
            this.label5.TabIndex = 14;
            this.label5.Text = "*";
            // 
            // txtFundType
            // 
            this.txtFundType.Location = new System.Drawing.Point(369, 75);
            this.txtFundType.MaxLength = 40;
            this.txtFundType.Name = "txtFundType";
            this.txtFundType.ReadOnly = true;
            this.txtFundType.Size = new System.Drawing.Size(105, 25);
            this.txtFundType.TabIndex = 32;
            // 
            // btnFundType
            // 
            this.btnFundType.Location = new System.Drawing.Point(482, 75);
            this.btnFundType.Name = "btnFundType";
            this.btnFundType.Size = new System.Drawing.Size(40, 25);
            this.btnFundType.TabIndex = 33;
            this.btnFundType.Text = "...";
            this.btnFundType.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnFundType.ToolTipText = "Select Fund(s)";
            this.btnFundType.Click += new System.EventHandler(this.btnFundType_Click);
            // 
            // chkBold
            // 
            this.chkBold.Location = new System.Drawing.Point(330, 203);
            this.chkBold.Name = "chkBold";
            this.chkBold.Size = new System.Drawing.Size(57, 21);
            this.chkBold.TabIndex = 12;
            this.chkBold.Text = "Bold ";
            // 
            // chkAttnday
            // 
            this.chkAttnday.Location = new System.Drawing.Point(215, 203);
            this.chkAttnday.Name = "chkAttnday";
            this.chkAttnday.Size = new System.Drawing.Size(88, 21);
            this.chkAttnday.TabIndex = 11;
            this.chkAttnday.Text = "Attn 1 Day";
            // 
            // lblPosition
            // 
            this.lblPosition.AutoSize = true;
            this.lblPosition.Location = new System.Drawing.Point(254, 111);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(47, 14);
            this.lblPosition.TabIndex = 0;
            this.lblPosition.Text = "Position";
            // 
            // chkStatus
            // 
            this.chkStatus.CheckState = Wisej.Web.CheckState.Checked;
            this.chkStatus.Location = new System.Drawing.Point(130, 203);
            this.chkStatus.Name = "chkStatus";
            this.chkStatus.Size = new System.Drawing.Size(67, 21);
            this.chkStatus.TabIndex = 10;
            this.chkStatus.Text = "Status";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(300, 109);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(9, 14);
            this.label8.TabIndex = 14;
            this.label8.Text = "*";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(313, 76);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(9, 14);
            this.label7.TabIndex = 14;
            this.label7.Text = "*";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(351, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(9, 14);
            this.label6.TabIndex = 14;
            this.label6.Text = "*";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(80, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(9, 14);
            this.label4.TabIndex = 14;
            this.label4.Text = "*";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(97, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(9, 14);
            this.label3.TabIndex = 14;
            this.label3.Text = "*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(56, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(9, 14);
            this.label2.TabIndex = 14;
            this.label2.Text = "*";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(111, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(9, 14);
            this.label1.TabIndex = 14;
            this.label1.Text = "*";
            // 
            // lblreqCity
            // 
            this.lblreqCity.AutoSize = true;
            this.lblreqCity.ForeColor = System.Drawing.Color.Red;
            this.lblreqCity.Location = new System.Drawing.Point(86, 12);
            this.lblreqCity.Name = "lblreqCity";
            this.lblreqCity.Size = new System.Drawing.Size(9, 14);
            this.lblreqCity.TabIndex = 14;
            this.lblreqCity.Text = "*";
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(15, 142);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(66, 16);
            this.lblDesc.TabIndex = 0;
            this.lblDesc.Text = "Description";
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(130, 139);
            this.txtDesc.MaxLength = 200;
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(392, 56);
            this.txtDesc.TabIndex = 9;
            // 
            // lblQType
            // 
            this.lblQType.Location = new System.Drawing.Point(15, 111);
            this.lblQType.Name = "lblQType";
            this.lblQType.Size = new System.Drawing.Size(83, 16);
            this.lblQType.TabIndex = 0;
            this.lblQType.Text = "Question Type";
            // 
            // cmbQuesType
            // 
            this.cmbQuesType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbQuesType.FormattingEnabled = true;
            this.cmbQuesType.Location = new System.Drawing.Point(130, 107);
            this.cmbQuesType.Name = "cmbQuesType";
            this.cmbQuesType.Size = new System.Drawing.Size(90, 25);
            this.cmbQuesType.TabIndex = 7;
            // 
            // lblFundType
            // 
            this.lblFundType.Location = new System.Drawing.Point(254, 79);
            this.lblFundType.Name = "lblFundType";
            this.lblFundType.Size = new System.Drawing.Size(59, 16);
            this.lblFundType.TabIndex = 0;
            this.lblFundType.Text = "Fund Type";
            // 
            // cmbSection
            // 
            this.cmbSection.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbSection.FormattingEnabled = true;
            this.cmbSection.Location = new System.Drawing.Point(130, 75);
            this.cmbSection.Name = "cmbSection";
            this.cmbSection.Size = new System.Drawing.Size(90, 25);
            this.cmbSection.TabIndex = 5;
            // 
            // lblSection
            // 
            this.lblSection.AutoSize = true;
            this.lblSection.Location = new System.Drawing.Point(15, 79);
            this.lblSection.Name = "lblSection";
            this.lblSection.Size = new System.Drawing.Size(45, 14);
            this.lblSection.TabIndex = 0;
            this.lblSection.Text = "Section";
            // 
            // lblQSection
            // 
            this.lblQSection.Location = new System.Drawing.Point(254, 46);
            this.lblQSection.Name = "lblQSection";
            this.lblQSection.Size = new System.Drawing.Size(97, 16);
            this.lblQSection.TabIndex = 0;
            this.lblQSection.Text = "Question Section";
            // 
            // txtQuesSection
            // 
            this.txtQuesSection.Location = new System.Drawing.Point(369, 43);
            this.txtQuesSection.MaxLength = 1;
            this.txtQuesSection.Name = "txtQuesSection";
            this.txtQuesSection.Size = new System.Drawing.Size(55, 25);
            this.txtQuesSection.TabIndex = 4;
            // 
            // lblQSCode
            // 
            this.lblQSCode.Location = new System.Drawing.Point(15, 47);
            this.lblQSCode.Name = "lblQSCode";
            this.lblQSCode.Size = new System.Drawing.Size(97, 16);
            this.lblQSCode.TabIndex = 0;
            this.lblQSCode.Text = "Sub Question No";
            // 
            // lblQCode
            // 
            this.lblQCode.Location = new System.Drawing.Point(254, 15);
            this.lblQCode.Name = "lblQCode";
            this.lblQCode.Size = new System.Drawing.Size(85, 16);
            this.lblQCode.TabIndex = 0;
            this.lblQCode.Text = "Question Code";
            // 
            // lblQNo
            // 
            this.lblQNo.Location = new System.Drawing.Point(15, 15);
            this.lblQNo.Name = "lblQNo";
            this.lblQNo.Size = new System.Drawing.Size(72, 16);
            this.lblQNo.TabIndex = 0;
            this.lblQNo.Text = "Question No";
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnSave);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 229);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(558, 35);
            this.pnlSave.TabIndex = 11;
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(390, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(465, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(468, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "&Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlDetails);
            this.pnlCompleteForm.Controls.Add(this.pnlSave);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(558, 264);
            this.pnlCompleteForm.TabIndex = 12;
            // 
            // txtPosition
            // 
            this.txtPosition.Location = new System.Drawing.Point(369, 107);
            this.txtPosition.MaxLength = 3;
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.Size = new System.Drawing.Size(55, 25);
            this.txtPosition.TabIndex = 8;
            // 
            // txtSeqCode
            // 
            this.txtSeqCode.Location = new System.Drawing.Point(130, 43);
            this.txtSeqCode.MaxLength = 3;
            this.txtSeqCode.Name = "txtSeqCode";
            this.txtSeqCode.Size = new System.Drawing.Size(55, 25);
            this.txtSeqCode.TabIndex = 3;
            // 
            // txtQuesCode
            // 
            this.txtQuesCode.Location = new System.Drawing.Point(369, 11);
            this.txtQuesCode.MaxLength = 3;
            this.txtQuesCode.Name = "txtQuesCode";
            this.txtQuesCode.Size = new System.Drawing.Size(55, 25);
            this.txtQuesCode.TabIndex = 2;
            // 
            // txtQuesNo
            // 
            this.txtQuesNo.Location = new System.Drawing.Point(130, 11);
            this.txtQuesNo.MaxLength = 4;
            this.txtQuesNo.Name = "txtQuesNo";
            this.txtQuesNo.Size = new System.Drawing.Size(55, 25);
            this.txtQuesNo.TabIndex = 1;
            // 
            // PirQuestForm
            // 
            this.ClientSize = new System.Drawing.Size(558, 264);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PirQuestForm";
            this.Text = "Pir Questions";
            componentTool1.ImageSource = "icon-help";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.pnlDetails.ResumeLayout(false);
            this.pnlDetails.PerformLayout();
            this.pnlSave.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel pnlDetails;
        private Label lblQNo;
        private Label lblDesc;
        private TextBox txtDesc;
        private CheckBox chkStatus;
        private Label lblQType;
        private ComboBox cmbQuesType;
        private Label lblFundType;
        private ComboBox cmbSection;
        private Label lblSection;
        private Label lblQSection;
        private TextBox txtQuesSection;
        private TextBoxWithValidation txtSeqCode;
        private Label lblQSCode;
        private Label lblQCode;
        private TextBoxWithValidation txtQuesCode;
        private TextBoxWithValidation txtQuesNo;
        private Panel pnlSave;
        private Button btnCancel;
        private Button btnSave;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Label lblreqCity;
        private Panel panel3;
        private Label lblPosition;
        private TextBoxWithValidation txtPosition;
        private Label label8;
        private CheckBox chkAttnday;
        private CheckBox chkBold;
        private TextBox txtFundType;
        private Button btnFundType;
        private Panel pnlCompleteForm;
        private Spacer spacer1;
    }
}