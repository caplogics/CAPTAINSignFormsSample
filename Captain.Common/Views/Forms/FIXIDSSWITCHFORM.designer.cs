using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class FIXIDSSWITCHFORM
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FIXIDSSWITCHFORM));
            this.pnlReason = new Wisej.Web.Panel();
            this.btnSubmit = new Wisej.Web.Button();
            this.btnCancel = new Wisej.Web.Button();
            this.txtReason = new Wisej.Web.TextBox();
            this.label1 = new Wisej.Web.Label();
            this.pnltxtReason = new Wisej.Web.Panel();
            this.pnlSave = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlReason.SuspendLayout();
            this.pnltxtReason.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlReason
            // 
            this.pnlReason.Controls.Add(this.pnltxtReason);
            this.pnlReason.Controls.Add(this.pnlSave);
            this.pnlReason.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlReason.Location = new System.Drawing.Point(0, 0);
            this.pnlReason.Name = "pnlReason";
            this.pnlReason.Size = new System.Drawing.Size(378, 123);
            this.pnlReason.TabIndex = 0;
            // 
            // btnSubmit
            // 
            this.btnSubmit.AppearanceKey = "button-ok";
            this.btnSubmit.Dock = Wisej.Web.DockStyle.Right;
            this.btnSubmit.Location = new System.Drawing.Point(210, 5);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 25);
            this.btnSubmit.TabIndex = 6;
            this.btnSubmit.Text = "&Save";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(288, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtReason
            // 
            this.txtReason.Location = new System.Drawing.Point(20, 26);
            this.txtReason.MaxLength = 150;
            this.txtReason.Multiline = true;
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(344, 56);
            this.txtReason.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Reason";
            // 
            // pnltxtReason
            // 
            this.pnltxtReason.Controls.Add(this.txtReason);
            this.pnltxtReason.Controls.Add(this.label1);
            this.pnltxtReason.Dock = Wisej.Web.DockStyle.Fill;
            this.pnltxtReason.Location = new System.Drawing.Point(0, 0);
            this.pnltxtReason.Name = "pnltxtReason";
            this.pnltxtReason.Size = new System.Drawing.Size(378, 88);
            this.pnltxtReason.TabIndex = 8;
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnSubmit);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 88);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(378, 35);
            this.pnlSave.TabIndex = 9;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(285, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // FIXIDSSWITCHFORM
            // 
            this.ClientSize = new System.Drawing.Size(378, 123);
            this.Controls.Add(this.pnlReason);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FIXIDSSWITCHFORM";
            this.Text = "Reason Form";
            this.pnlReason.ResumeLayout(false);
            this.pnltxtReason.ResumeLayout(false);
            this.pnltxtReason.PerformLayout();
            this.pnlSave.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private Panel pnlReason;
        private TextBox txtReason;
        private Label label1;
        private Button btnSubmit;
        private Button btnCancel;
        private Panel pnltxtReason;
        private Panel pnlSave;
        private Spacer spacer1;
    }
}