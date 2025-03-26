namespace Captain.Common.Views.Forms
{
    partial class HUD00003_Form
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

        #region Wisej.NET Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HUD00003_Form));
            this.pnlForm = new Wisej.Web.Panel();
            this.pnlParams = new Wisej.Web.Panel();
            this.pnlDateStatus = new Wisej.Web.Panel();
            this.cmbStatus = new Wisej.Web.ComboBox();
            this.lblStatus = new Wisej.Web.Label();
            this.dtpDate = new Wisej.Web.DateTimePicker();
            this.lblDate = new Wisej.Web.Label();
            this.pnlSave = new Wisej.Web.Panel();
            this.txtMSTSeq = new Wisej.Web.TextBox();
            this.btnSave = new Wisej.Web.Button();
            this.spacer6 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.pnlForm.SuspendLayout();
            this.pnlParams.SuspendLayout();
            this.pnlDateStatus.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlForm
            // 
            this.pnlForm.Controls.Add(this.pnlParams);
            this.pnlForm.Controls.Add(this.pnlSave);
            this.pnlForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlForm.Location = new System.Drawing.Point(0, 0);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(398, 105);
            this.pnlForm.TabIndex = 0;
            // 
            // pnlParams
            // 
            this.pnlParams.Controls.Add(this.pnlDateStatus);
            this.pnlParams.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlParams.Location = new System.Drawing.Point(0, 0);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new System.Drawing.Size(398, 70);
            this.pnlParams.TabIndex = 9;
            // 
            // pnlDateStatus
            // 
            this.pnlDateStatus.Controls.Add(this.cmbStatus);
            this.pnlDateStatus.Controls.Add(this.lblStatus);
            this.pnlDateStatus.Controls.Add(this.dtpDate);
            this.pnlDateStatus.Controls.Add(this.lblDate);
            this.pnlDateStatus.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlDateStatus.Location = new System.Drawing.Point(0, 0);
            this.pnlDateStatus.Name = "pnlDateStatus";
            this.pnlDateStatus.Size = new System.Drawing.Size(398, 70);
            this.pnlDateStatus.TabIndex = 1;
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbStatus.Location = new System.Drawing.Point(255, 25);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(80, 25);
            this.cmbStatus.TabIndex = 2;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(210, 29);
            this.lblStatus.MinimumSize = new System.Drawing.Size(0, 16);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 16);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Status";
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "MM/dd/yyyy";
            this.dtpDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(85, 25);
            this.dtpDate.MaximumSize = new System.Drawing.Size(0, 25);
            this.dtpDate.MinimumSize = new System.Drawing.Size(0, 25);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(96, 25);
            this.dtpDate.TabIndex = 1;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(48, 29);
            this.lblDate.MinimumSize = new System.Drawing.Size(0, 16);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(30, 16);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "Date";
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.txtMSTSeq);
            this.pnlSave.Controls.Add(this.btnSave);
            this.pnlSave.Controls.Add(this.spacer6);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 70);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(398, 35);
            this.pnlSave.TabIndex = 2;
            // 
            // txtMSTSeq
            // 
            this.txtMSTSeq.Dock = Wisej.Web.DockStyle.Left;
            this.txtMSTSeq.Location = new System.Drawing.Point(5, 5);
            this.txtMSTSeq.Name = "txtMSTSeq";
            this.txtMSTSeq.Size = new System.Drawing.Size(70, 25);
            this.txtMSTSeq.TabIndex = 6;
            this.txtMSTSeq.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(230, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // spacer6
            // 
            this.spacer6.Dock = Wisej.Web.DockStyle.Right;
            this.spacer6.Location = new System.Drawing.Point(305, 5);
            this.spacer6.Name = "spacer6";
            this.spacer6.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(308, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // HUD00003_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 105);
            this.Controls.Add(this.pnlForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HUD00003_Form";
            this.Text = "HUD Forms";
            this.FormClosing += new Wisej.Web.FormClosingEventHandler(this.HUD00003_Form_FormClosing);
            this.pnlForm.ResumeLayout(false);
            this.pnlParams.ResumeLayout(false);
            this.pnlDateStatus.ResumeLayout(false);
            this.pnlDateStatus.PerformLayout();
            this.pnlSave.ResumeLayout(false);
            this.pnlSave.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Panel pnlForm;
        private Wisej.Web.Panel pnlParams;
        private Wisej.Web.Panel pnlDateStatus;
        private Wisej.Web.Panel pnlSave;
        private Wisej.Web.Button btnSave;
        private Wisej.Web.Spacer spacer6;
        private Wisej.Web.Button btnCancel;
        private Wisej.Web.Label lblDate;
        private Wisej.Web.Label lblStatus;
        private Wisej.Web.DateTimePicker dtpDate;
        private Wisej.Web.ComboBox cmbStatus;
        private Wisej.Web.TextBox txtMSTSeq;
    }
}