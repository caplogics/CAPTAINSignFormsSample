using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class MAT00003DateChangeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MAT00003DateChangeForm));
            this.btnSave = new Wisej.Web.Button();
            this.btnCancel = new Wisej.Web.Button();
            this.dtAssessmentDate = new Wisej.Web.DateTimePicker();
            this.lblAssessmentDate = new Wisej.Web.Label();
            this.label2 = new Wisej.Web.Label();
            this.lblAssessmentdt1 = new Wisej.Web.Label();
            this.pnlSave = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(236, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-cancel";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(316, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dtAssessmentDate
            // 
            this.dtAssessmentDate.Checked = false;
            this.dtAssessmentDate.CustomFormat = "MM/dd/yyyy";
            this.dtAssessmentDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtAssessmentDate.Location = new System.Drawing.Point(180, 51);
            this.dtAssessmentDate.MinimumSize = new System.Drawing.Size(0, 25);
            this.dtAssessmentDate.Name = "dtAssessmentDate";
            this.dtAssessmentDate.ShowToolTips = false;
            this.dtAssessmentDate.Size = new System.Drawing.Size(116, 25);
            this.dtAssessmentDate.TabIndex = 3;
            // 
            // lblAssessmentDate
            // 
            this.lblAssessmentDate.Location = new System.Drawing.Point(20, 55);
            this.lblAssessmentDate.Name = "lblAssessmentDate";
            this.lblAssessmentDate.Size = new System.Drawing.Size(144, 16);
            this.lblAssessmentDate.TabIndex = 4;
            this.lblAssessmentDate.Text = "Change Assessment Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 14);
            this.label2.TabIndex = 7;
            this.label2.Text = "Assessment Date";
            // 
            // lblAssessmentdt1
            // 
            this.lblAssessmentdt1.AutoSize = true;
            this.lblAssessmentdt1.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblAssessmentdt1.Location = new System.Drawing.Point(180, 23);
            this.lblAssessmentdt1.Name = "lblAssessmentdt1";
            this.lblAssessmentdt1.Size = new System.Drawing.Size(9, 14);
            this.lblAssessmentdt1.TabIndex = 8;
            this.lblAssessmentdt1.Text = "-";
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnSave);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 95);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(406, 35);
            this.pnlSave.TabIndex = 9;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(311, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 25);
            // 
            // MAT00003DateChangeForm
            // 
            this.ClientSize = new System.Drawing.Size(406, 130);
            this.Controls.Add(this.lblAssessmentdt1);
            this.Controls.Add(this.dtAssessmentDate);
            this.Controls.Add(this.pnlSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblAssessmentDate);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MAT00003DateChangeForm";
            this.Text = "MAT00003DateChangeForm";
            this.pnlSave.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnSave;
        private Button btnCancel;
        private DateTimePicker dtAssessmentDate;
        private Label lblAssessmentDate;
        private Label label2;
        private Label lblAssessmentdt1;
        private Panel pnlSave;
        private Spacer spacer1;
    }
}