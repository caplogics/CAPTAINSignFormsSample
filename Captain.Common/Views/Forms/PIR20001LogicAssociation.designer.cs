using Wisej.Web;
namespace Captain.Common.Views.Forms
{
    partial class PIR20001LogicAssociation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PIR20001LogicAssociation));
            this.lblIntakReq = new Wisej.Web.Label();
            this.label13 = new Wisej.Web.Label();
            this.label9 = new Wisej.Web.Label();
            this.GenderReq = new Wisej.Web.Label();
            this.dtToDate = new Wisej.Web.DateTimePicker();
            this.dtFromDate = new Wisej.Web.DateTimePicker();
            this.lblToDt = new Wisej.Web.Label();
            this.lblFromDt = new Wisej.Web.Label();
            this.chkResponse = new Wisej.Web.CheckBox();
            this.chkDateRange = new Wisej.Web.CheckBox();
            this.chkValidDate = new Wisej.Web.CheckBox();
            this.label10 = new Wisej.Web.Label();
            this.cmbYear2 = new Wisej.Web.ComboBox();
            this.cmbResponse = new Wisej.Web.ComboBox();
            this.lblCondition = new Wisej.Web.Label();
            this.cmbCondition = new Wisej.Web.ComboBox();
            this.cmbDateKey = new Wisej.Web.ComboBox();
            this.label7 = new Wisej.Web.Label();
            this.lblGroup = new Wisej.Web.Label();
            this.cmbGroup = new Wisej.Web.ComboBox();
            this.cmbConjunction = new Wisej.Web.ComboBox();
            this.label5 = new Wisej.Web.Label();
            this.lblIntakeField = new Wisej.Web.Label();
            this.cmbIntakeField = new Wisej.Web.ComboBox();
            this.cmbType = new Wisej.Web.ComboBox();
            this.lblType = new Wisej.Web.Label();
            this.pnlDetails = new Wisej.Web.Panel();
            this.label1 = new Wisej.Web.Label();
            this.txtResp = new Wisej.Web.TextBox();
            this.pnlSave = new Wisej.Web.Panel();
            this.btnSubmit = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.panel1 = new Wisej.Web.Panel();
            this.pnlDetails.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblIntakReq
            // 
            this.lblIntakReq.AutoSize = true;
            this.lblIntakReq.ForeColor = System.Drawing.Color.Red;
            this.lblIntakReq.Location = new System.Drawing.Point(327, 11);
            this.lblIntakReq.Name = "lblIntakReq";
            this.lblIntakReq.Size = new System.Drawing.Size(9, 14);
            this.lblIntakReq.TabIndex = 28;
            this.lblIntakReq.Text = "*";
            this.lblIntakReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(69, 76);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(9, 14);
            this.label13.TabIndex = 28;
            this.label13.Text = "*";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(49, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(9, 14);
            this.label9.TabIndex = 28;
            this.label9.Text = "*";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // GenderReq
            // 
            this.GenderReq.AutoSize = true;
            this.GenderReq.ForeColor = System.Drawing.Color.Red;
            this.GenderReq.Location = new System.Drawing.Point(41, 13);
            this.GenderReq.Name = "GenderReq";
            this.GenderReq.Size = new System.Drawing.Size(9, 14);
            this.GenderReq.TabIndex = 28;
            this.GenderReq.Text = "*";
            this.GenderReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // dtToDate
            // 
            this.dtToDate.AutoSize = false;
            this.dtToDate.Checked = false;
            this.dtToDate.CustomFormat = "MM/dd/yyyy";
            this.dtToDate.Enabled = false;
            this.dtToDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtToDate.Location = new System.Drawing.Point(606, 107);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.ShowCheckBox = true;
            this.dtToDate.ShowToolTips = false;
            this.dtToDate.Size = new System.Drawing.Size(116, 25);
            this.dtToDate.TabIndex = 13;
            // 
            // dtFromDate
            // 
            this.dtFromDate.AutoSize = false;
            this.dtFromDate.Checked = false;
            this.dtFromDate.CustomFormat = "MM/dd/yyyy";
            this.dtFromDate.Enabled = false;
            this.dtFromDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtFromDate.Location = new System.Drawing.Point(606, 75);
            this.dtFromDate.Name = "dtFromDate";
            this.dtFromDate.ShowCheckBox = true;
            this.dtFromDate.ShowToolTips = false;
            this.dtFromDate.Size = new System.Drawing.Size(116, 25);
            this.dtFromDate.TabIndex = 12;
            // 
            // lblToDt
            // 
            this.lblToDt.AutoSize = true;
            this.lblToDt.Location = new System.Drawing.Point(504, 111);
            this.lblToDt.Name = "lblToDt";
            this.lblToDt.Size = new System.Drawing.Size(47, 14);
            this.lblToDt.TabIndex = 0;
            this.lblToDt.Text = "To Date";
            // 
            // lblFromDt
            // 
            this.lblFromDt.AutoSize = true;
            this.lblFromDt.Location = new System.Drawing.Point(504, 79);
            this.lblFromDt.Name = "lblFromDt";
            this.lblFromDt.Size = new System.Drawing.Size(61, 14);
            this.lblFromDt.TabIndex = 0;
            this.lblFromDt.Text = "From Date";
            // 
            // chkResponse
            // 
            this.chkResponse.AutoSize = false;
            this.chkResponse.Location = new System.Drawing.Point(256, 108);
            this.chkResponse.Name = "chkResponse";
            this.chkResponse.Size = new System.Drawing.Size(81, 21);
            this.chkResponse.TabIndex = 7;
            this.chkResponse.Text = "Response";
            this.chkResponse.CheckedChanged += new System.EventHandler(this.chkResponse_CheckedChanged);
            // 
            // chkDateRange
            // 
            this.chkDateRange.AutoSize = false;
            this.chkDateRange.Enabled = false;
            this.chkDateRange.Location = new System.Drawing.Point(608, 44);
            this.chkDateRange.Name = "chkDateRange";
            this.chkDateRange.Size = new System.Drawing.Size(92, 21);
            this.chkDateRange.TabIndex = 11;
            this.chkDateRange.Text = "Date Range";
            this.chkDateRange.CheckedChanged += new System.EventHandler(this.chkDateRange_CheckedChanged);
            this.chkDateRange.Click += new System.EventHandler(this.chkDateRange_Click);
            // 
            // chkValidDate
            // 
            this.chkValidDate.AutoSize = false;
            this.chkValidDate.Enabled = false;
            this.chkValidDate.Location = new System.Drawing.Point(501, 44);
            this.chkValidDate.Name = "chkValidDate";
            this.chkValidDate.Size = new System.Drawing.Size(84, 21);
            this.chkValidDate.TabIndex = 10;
            this.chkValidDate.Text = "Valid Date";
            this.chkValidDate.Click += new System.EventHandler(this.chkValidDate_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 111);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 14);
            this.label10.TabIndex = 0;
            this.label10.Text = "Year";
            // 
            // cmbYear2
            // 
            this.cmbYear2.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbYear2.FormattingEnabled = true;
            this.cmbYear2.Location = new System.Drawing.Point(90, 107);
            this.cmbYear2.Name = "cmbYear2";
            this.cmbYear2.Size = new System.Drawing.Size(121, 25);
            this.cmbYear2.TabIndex = 6;
            // 
            // cmbResponse
            // 
            this.cmbResponse.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbResponse.FormattingEnabled = true;
            this.cmbResponse.Location = new System.Drawing.Point(350, 107);
            this.cmbResponse.Name = "cmbResponse";
            this.cmbResponse.Size = new System.Drawing.Size(121, 25);
            this.cmbResponse.TabIndex = 8;
            // 
            // lblCondition
            // 
            this.lblCondition.Location = new System.Drawing.Point(15, 79);
            this.lblCondition.Name = "lblCondition";
            this.lblCondition.Size = new System.Drawing.Size(56, 16);
            this.lblCondition.TabIndex = 0;
            this.lblCondition.Text = "Condition";
            // 
            // cmbCondition
            // 
            this.cmbCondition.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbCondition.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbCondition.FormattingEnabled = true;
            this.cmbCondition.Location = new System.Drawing.Point(90, 75);
            this.cmbCondition.Name = "cmbCondition";
            this.cmbCondition.Size = new System.Drawing.Size(381, 25);
            this.cmbCondition.TabIndex = 5;
            this.cmbCondition.SelectedIndexChanged += new System.EventHandler(this.cmbCondition_SelectedIndexChanged);
            // 
            // cmbDateKey
            // 
            this.cmbDateKey.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbDateKey.FormattingEnabled = true;
            this.cmbDateKey.Location = new System.Drawing.Point(606, 11);
            this.cmbDateKey.Name = "cmbDateKey";
            this.cmbDateKey.Size = new System.Drawing.Size(141, 25);
            this.cmbDateKey.TabIndex = 9;
            this.cmbDateKey.SelectedIndexChanged += new System.EventHandler(this.cmbDateKey_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(504, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 16);
            this.label7.TabIndex = 0;
            this.label7.Text = "Select Date Key";
            // 
            // lblGroup
            // 
            this.lblGroup.Location = new System.Drawing.Point(15, 47);
            this.lblGroup.Name = "lblGroup";
            this.lblGroup.Size = new System.Drawing.Size(36, 16);
            this.lblGroup.TabIndex = 0;
            this.lblGroup.Text = "Group";
            // 
            // cmbGroup
            // 
            this.cmbGroup.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbGroup.FormattingEnabled = true;
            this.cmbGroup.Location = new System.Drawing.Point(90, 43);
            this.cmbGroup.Name = "cmbGroup";
            this.cmbGroup.Size = new System.Drawing.Size(121, 25);
            this.cmbGroup.TabIndex = 3;
            // 
            // cmbConjunction
            // 
            this.cmbConjunction.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbConjunction.FormattingEnabled = true;
            this.cmbConjunction.Location = new System.Drawing.Point(350, 43);
            this.cmbConjunction.Name = "cmbConjunction";
            this.cmbConjunction.Size = new System.Drawing.Size(121, 25);
            this.cmbConjunction.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(262, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 16);
            this.label5.TabIndex = 0;
            this.label5.Text = "Conjunction";
            // 
            // lblIntakeField
            // 
            this.lblIntakeField.Location = new System.Drawing.Point(262, 15);
            this.lblIntakeField.Name = "lblIntakeField";
            this.lblIntakeField.Size = new System.Drawing.Size(66, 16);
            this.lblIntakeField.TabIndex = 0;
            this.lblIntakeField.Text = "Intake Field";
            // 
            // cmbIntakeField
            // 
            this.cmbIntakeField.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbIntakeField.Enabled = false;
            this.cmbIntakeField.FormattingEnabled = true;
            this.cmbIntakeField.Location = new System.Drawing.Point(350, 11);
            this.cmbIntakeField.Name = "cmbIntakeField";
            this.cmbIntakeField.Size = new System.Drawing.Size(121, 25);
            this.cmbIntakeField.TabIndex = 2;
            this.cmbIntakeField.SelectedIndexChanged += new System.EventHandler(this.cmbIntakeField_SelectedIndexChanged);
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbType.Enabled = false;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(90, 11);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(121, 25);
            this.cmbType.TabIndex = 1;
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(15, 15);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(28, 16);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "Type";
            // 
            // pnlDetails
            // 
            this.pnlDetails.Controls.Add(this.label1);
            this.pnlDetails.Controls.Add(this.txtResp);
            this.pnlDetails.Controls.Add(this.dtToDate);
            this.pnlDetails.Controls.Add(this.dtFromDate);
            this.pnlDetails.Controls.Add(this.lblToDt);
            this.pnlDetails.Controls.Add(this.lblFromDt);
            this.pnlDetails.Controls.Add(this.chkResponse);
            this.pnlDetails.Controls.Add(this.chkDateRange);
            this.pnlDetails.Controls.Add(this.chkValidDate);
            this.pnlDetails.Controls.Add(this.label10);
            this.pnlDetails.Controls.Add(this.cmbYear2);
            this.pnlDetails.Controls.Add(this.cmbResponse);
            this.pnlDetails.Controls.Add(this.lblCondition);
            this.pnlDetails.Controls.Add(this.cmbCondition);
            this.pnlDetails.Controls.Add(this.cmbDateKey);
            this.pnlDetails.Controls.Add(this.label7);
            this.pnlDetails.Controls.Add(this.lblGroup);
            this.pnlDetails.Controls.Add(this.cmbGroup);
            this.pnlDetails.Controls.Add(this.cmbConjunction);
            this.pnlDetails.Controls.Add(this.label5);
            this.pnlDetails.Controls.Add(this.lblIntakeField);
            this.pnlDetails.Controls.Add(this.cmbIntakeField);
            this.pnlDetails.Controls.Add(this.cmbType);
            this.pnlDetails.Controls.Add(this.lblType);
            this.pnlDetails.Controls.Add(this.GenderReq);
            this.pnlDetails.Controls.Add(this.label9);
            this.pnlDetails.Controls.Add(this.label13);
            this.pnlDetails.Controls.Add(this.lblIntakReq);
            this.pnlDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlDetails.Location = new System.Drawing.Point(0, 0);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Size = new System.Drawing.Size(773, 172);
            this.pnlDetails.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(331, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 10);
            this.label1.TabIndex = 28;
            this.label1.Text = "*";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.Visible = false;
            // 
            // txtResp
            // 
            this.txtResp.Location = new System.Drawing.Point(350, 139);
            this.txtResp.MaxLength = 30;
            this.txtResp.Name = "txtResp";
            this.txtResp.Size = new System.Drawing.Size(121, 25);
            this.txtResp.TabIndex = 8;
            this.txtResp.Visible = false;
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnSubmit);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 172);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(773, 35);
            this.pnlSave.TabIndex = 2;
            // 
            // btnSubmit
            // 
            this.btnSubmit.AppearanceKey = "button-ok";
            this.btnSubmit.Dock = Wisej.Web.DockStyle.Right;
            this.btnSubmit.Location = new System.Drawing.Point(605, 5);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 25);
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Text = "&Save";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(680, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(683, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlDetails);
            this.panel1.Controls.Add(this.pnlSave);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(773, 207);
            this.panel1.TabIndex = 3;
            // 
            // PIR20001LogicAssociation
            // 
            this.ClientSize = new System.Drawing.Size(773, 207);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PIR20001LogicAssociation";
            this.Text = "PIR20001LogicAssociation";
            this.pnlDetails.ResumeLayout(false);
            this.pnlDetails.PerformLayout();
            this.pnlSave.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Label lblIntakReq;
        private Label label13;
        private Label label9;
        private Label GenderReq;
        private DateTimePicker dtToDate;
        private DateTimePicker dtFromDate;
        private Label lblToDt;
        private Label lblFromDt;
        private CheckBox chkResponse;
        private CheckBox chkDateRange;
        private CheckBox chkValidDate;
        private Label label10;
        private ComboBox cmbYear2;
        private ComboBox cmbResponse;
        private Label lblCondition;
        private ComboBox cmbCondition;
        private ComboBox cmbDateKey;
        private Label label7;
        private Label lblGroup;
        private ComboBox cmbGroup;
        private ComboBox cmbConjunction;
        private Label label5;
        private Label lblIntakeField;
        private ComboBox cmbIntakeField;
        private ComboBox cmbType;
        private Label lblType;
        private Panel pnlDetails;
        private Panel pnlSave;
        private Button btnCancel;
        private Button btnSubmit;
        private TextBox txtResp;
        private Label label1;
        private Spacer spacer1;
        private Panel panel1;
    }
}