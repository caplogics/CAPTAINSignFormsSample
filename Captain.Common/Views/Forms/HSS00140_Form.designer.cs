using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class HSS00140_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSS00140_Form));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.pnlFields = new Wisej.Web.Panel();
            this.label1 = new Wisej.Web.Label();
            this.dtpDropoff = new Wisej.Web.DateTimePicker();
            this.dtpPickUp = new Wisej.Web.DateTimePicker();
            this.txtComments = new Wisej.Web.TextBox();
            this.btnAppSearch = new Wisej.Web.Button();
            this.lblComments = new Wisej.Web.Label();
            this.label3 = new Wisej.Web.Label();
            this.label2 = new Wisej.Web.Label();
            this.lblPickup = new Wisej.Web.Label();
            this.lblDropoff = new Wisej.Web.Label();
            this.maskedTextBox1 = new Wisej.Web.MaskedTextBox();
            this.lblPhone = new Wisej.Web.Label();
            this.txtLName = new Wisej.Web.TextBox();
            this.lblLName = new Wisej.Web.Label();
            this.lblFName = new Wisej.Web.Label();
            this.txtFName = new Wisej.Web.TextBox();
            this.txtAppNo = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblAppNo = new Wisej.Web.Label();
            this.pnlSave = new Wisej.Web.Panel();
            this.btnSave = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlFields.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFields
            // 
            this.pnlFields.Controls.Add(this.label1);
            this.pnlFields.Controls.Add(this.dtpDropoff);
            this.pnlFields.Controls.Add(this.dtpPickUp);
            this.pnlFields.Controls.Add(this.txtComments);
            this.pnlFields.Controls.Add(this.btnAppSearch);
            this.pnlFields.Controls.Add(this.lblComments);
            this.pnlFields.Controls.Add(this.label3);
            this.pnlFields.Controls.Add(this.label2);
            this.pnlFields.Controls.Add(this.lblPickup);
            this.pnlFields.Controls.Add(this.lblDropoff);
            this.pnlFields.Controls.Add(this.maskedTextBox1);
            this.pnlFields.Controls.Add(this.lblPhone);
            this.pnlFields.Controls.Add(this.txtLName);
            this.pnlFields.Controls.Add(this.lblLName);
            this.pnlFields.Controls.Add(this.lblFName);
            this.pnlFields.Controls.Add(this.txtFName);
            this.pnlFields.Controls.Add(this.txtAppNo);
            this.pnlFields.Controls.Add(this.lblAppNo);
            this.pnlFields.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlFields.Location = new System.Drawing.Point(0, 0);
            this.pnlFields.Name = "pnlFields";
            this.pnlFields.Size = new System.Drawing.Size(495, 235);
            this.pnlFields.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(57, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(7, 10);
            this.label1.TabIndex = 28;
            this.label1.Text = "*";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // dtpDropoff
            // 
            this.dtpDropoff.AutoSize = false;
            this.dtpDropoff.Checked = false;
            this.dtpDropoff.CustomFormat = "hh:mm:ss tt";
            this.dtpDropoff.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpDropoff.Location = new System.Drawing.Point(323, 139);
            this.dtpDropoff.Name = "dtpDropoff";
            this.dtpDropoff.ShowCheckBox = true;
            this.dtpDropoff.ShowToolTips = false;
            this.dtpDropoff.ShowUpDown = true;
            this.dtpDropoff.Size = new System.Drawing.Size(140, 25);
            this.dtpDropoff.TabIndex = 7;
            // 
            // dtpPickUp
            // 
            this.dtpPickUp.AutoSize = false;
            this.dtpPickUp.Checked = false;
            this.dtpPickUp.CustomFormat = "hh:mm:ss tt";
            this.dtpPickUp.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpPickUp.Location = new System.Drawing.Point(94, 139);
            this.dtpPickUp.Name = "dtpPickUp";
            this.dtpPickUp.ShowCheckBox = true;
            this.dtpPickUp.ShowToolTips = false;
            this.dtpPickUp.ShowUpDown = true;
            this.dtpPickUp.Size = new System.Drawing.Size(140, 25);
            this.dtpPickUp.TabIndex = 6;
            // 
            // txtComments
            // 
            this.txtComments.Location = new System.Drawing.Point(94, 171);
            this.txtComments.MaxLength = 80;
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.Size = new System.Drawing.Size(369, 56);
            this.txtComments.TabIndex = 8;
            // 
            // btnAppSearch
            // 
            this.btnAppSearch.Location = new System.Drawing.Point(205, 11);
            this.btnAppSearch.Name = "btnAppSearch";
            this.btnAppSearch.Size = new System.Drawing.Size(75, 25);
            this.btnAppSearch.TabIndex = 2;
            this.btnAppSearch.Text = "&Search";
            this.btnAppSearch.ToolTipText = "Search Applicant Number";
            this.btnAppSearch.Click += new System.EventHandler(this.btnAppSearch_Click);
            // 
            // lblComments
            // 
            this.lblComments.AutoSize = true;
            this.lblComments.Location = new System.Drawing.Point(15, 170);
            this.lblComments.Name = "lblComments";
            this.lblComments.Size = new System.Drawing.Size(63, 14);
            this.lblComments.TabIndex = 0;
            this.lblComments.Text = "Comments";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(310, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(6, 10);
            this.label3.TabIndex = 28;
            this.label3.Text = "*";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(53, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(7, 10);
            this.label2.TabIndex = 28;
            this.label2.Text = "*";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPickup
            // 
            this.lblPickup.Location = new System.Drawing.Point(15, 143);
            this.lblPickup.Name = "lblPickup";
            this.lblPickup.Size = new System.Drawing.Size(40, 16);
            this.lblPickup.TabIndex = 0;
            this.lblPickup.Text = "Pickup";
            // 
            // lblDropoff
            // 
            this.lblDropoff.Location = new System.Drawing.Point(264, 143);
            this.lblDropoff.Name = "lblDropoff";
            this.lblDropoff.Size = new System.Drawing.Size(47, 16);
            this.lblDropoff.TabIndex = 0;
            this.lblDropoff.Text = "Dropoff";
            // 
            // maskedTextBox1
            // 
            this.maskedTextBox1.Enabled = false;
            this.maskedTextBox1.Location = new System.Drawing.Point(94, 107);
            this.maskedTextBox1.Mask = "999-000-0000";
            this.maskedTextBox1.Name = "maskedTextBox1";
            this.maskedTextBox1.Size = new System.Drawing.Size(96, 25);
            this.maskedTextBox1.TabIndex = 5;
            // 
            // lblPhone
            // 
            this.lblPhone.Location = new System.Drawing.Point(15, 112);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(61, 16);
            this.lblPhone.TabIndex = 0;
            this.lblPhone.Text = "Telephone";
            // 
            // txtLName
            // 
            this.txtLName.Enabled = false;
            this.txtLName.Location = new System.Drawing.Point(94, 75);
            this.txtLName.MaxLength = 50;
            this.txtLName.Name = "txtLName";
            this.txtLName.Size = new System.Drawing.Size(369, 25);
            this.txtLName.TabIndex = 4;
            // 
            // lblLName
            // 
            this.lblLName.AutoSize = true;
            this.lblLName.Location = new System.Drawing.Point(15, 79);
            this.lblLName.Name = "lblLName";
            this.lblLName.Size = new System.Drawing.Size(62, 14);
            this.lblLName.TabIndex = 0;
            this.lblLName.Text = "Last Name";
            // 
            // lblFName
            // 
            this.lblFName.AutoSize = true;
            this.lblFName.Location = new System.Drawing.Point(15, 47);
            this.lblFName.Name = "lblFName";
            this.lblFName.Size = new System.Drawing.Size(63, 14);
            this.lblFName.TabIndex = 0;
            this.lblFName.Text = "First Name";
            // 
            // txtFName
            // 
            this.txtFName.Enabled = false;
            this.txtFName.Location = new System.Drawing.Point(94, 43);
            this.txtFName.MaxLength = 50;
            this.txtFName.Name = "txtFName";
            this.txtFName.Size = new System.Drawing.Size(369, 25);
            this.txtFName.TabIndex = 3;
            // 
            // txtAppNo
            // 
            this.txtAppNo.Location = new System.Drawing.Point(94, 11);
            this.txtAppNo.MaxLength = 8;
            this.txtAppNo.Name = "txtAppNo";
            this.txtAppNo.Size = new System.Drawing.Size(80, 25);
            this.txtAppNo.TabIndex = 1;
            this.txtAppNo.Leave += new System.EventHandler(this.txtAppNo_Leave);
            // 
            // lblAppNo
            // 
            this.lblAppNo.Location = new System.Drawing.Point(15, 15);
            this.lblAppNo.Name = "lblAppNo";
            this.lblAppNo.Size = new System.Drawing.Size(46, 16);
            this.lblAppNo.TabIndex = 0;
            this.lblAppNo.Text = "App No";
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnSave);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 235);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(495, 35);
            this.pnlSave.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(327, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(402, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(405, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlFields);
            this.pnlCompleteForm.Controls.Add(this.pnlSave);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(495, 270);
            this.pnlCompleteForm.TabIndex = 3;
            // 
            // HSS00140_Form
            // 
            this.ClientSize = new System.Drawing.Size(495, 270);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSS00140_Form";
            this.Text = "Bus Client Placement";
            componentTool1.ImageSource = "icon-help";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.pnlFields.ResumeLayout(false);
            this.pnlFields.PerformLayout();
            this.pnlSave.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Panel pnlFields;
        private TextBox txtLName;
        private Label lblLName;
        private Label lblFName;
        private TextBox txtFName;
        private TextBoxWithValidation txtAppNo;
        private Label lblAppNo;
        private MaskedTextBox maskedTextBox1;
        private Label lblPhone;
        private Label lblPickup;
        private Label lblDropoff;
        private Label label3;
        private Label label2;
        private Label label1;
        private Label lblComments;
        private Panel pnlSave;
        private Button btnCancel;
        private Button btnSave;
        private Button btnAppSearch;
        private TextBox txtComments;
        private DateTimePicker dtpPickUp;
        private DateTimePicker dtpDropoff;
        private Panel pnlCompleteForm;
        private Spacer spacer1;
    }
}