using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class AdminControlMessageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminControlMessageForm));
            this.btnCancel = new Wisej.Web.Button();
            this.btnShow = new Wisej.Web.Button();
            this.pnlComplete = new Wisej.Web.Panel();
            this.pictureBox1 = new Wisej.Web.PictureBox();
            this.txtMsg = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.btnOk = new Wisej.Web.Button();
            this.pnlShow = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlComplete.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlShow.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-cancel";
            this.btnCancel.AutoSize = true;
            this.btnCancel.BackColor = System.Drawing.Color.FromName("@window");
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnCancel.Location = new System.Drawing.Point(515, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(57, 25);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnShow
            // 
            this.btnShow.BackColor = System.Drawing.Color.FromName("@window");
            this.btnShow.Dock = Wisej.Web.DockStyle.Right;
            this.btnShow.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnShow.Location = new System.Drawing.Point(412, 5);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(100, 25);
            this.btnShow.TabIndex = 1;
            this.btnShow.Text = "&Show Details";
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // pnlComplete
            // 
            this.pnlComplete.Controls.Add(this.pnlShow);
            this.pnlComplete.Controls.Add(this.pictureBox1);
            this.pnlComplete.Controls.Add(this.txtMsg);
            this.pnlComplete.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlComplete.Location = new System.Drawing.Point(0, 0);
            this.pnlComplete.Name = "pnlComplete";
            this.pnlComplete.Size = new System.Drawing.Size(587, 110);
            this.pnlComplete.TabIndex = 0;
            this.pnlComplete.TabStop = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.ImageSource = "icon-warning?color=captainBrown";
            this.pictureBox1.Location = new System.Drawing.Point(3, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(80, 60);
            this.pictureBox1.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            // 
            // txtMsg
            // 
            this.txtMsg.BackColor = System.Drawing.Color.White;
            this.txtMsg.BorderStyle = Wisej.Web.BorderStyle.None;
            this.txtMsg.Location = new System.Drawing.Point(89, 5);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ReadOnly = true;
            this.txtMsg.Size = new System.Drawing.Size(493, 60);
            this.txtMsg.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(114, 95);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            // 
            // pnlShow
            // 
            this.pnlShow.AppearanceKey = "panel-grdo";
            this.pnlShow.Controls.Add(this.btnShow);
            this.pnlShow.Controls.Add(this.spacer1);
            this.pnlShow.Controls.Add(this.btnCancel);
            this.pnlShow.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlShow.Location = new System.Drawing.Point(0, 75);
            this.pnlShow.Name = "pnlShow";
            this.pnlShow.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlShow.Size = new System.Drawing.Size(587, 35);
            this.pnlShow.TabIndex = 6;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(512, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            this.spacer1.Click += new System.EventHandler(this.spacer1_Click);
            // 
            // AdminControlMessageForm
            // 
            this.ClientSize = new System.Drawing.Size(587, 110);
            this.Controls.Add(this.pnlComplete);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdminControlMessageForm";
            this.Text = "Admin Control Message Form";
            this.pnlComplete.ResumeLayout(false);
            this.pnlComplete.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlShow.ResumeLayout(false);
            this.pnlShow.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel pnlComplete;
        private Button btnCancel;
        private Button btnOk;
        private Button btnShow;
        private TextBoxWithValidation txtMsg;
        private PictureBox pictureBox1;
        private Panel pnlShow;
        private Spacer spacer1;
    }
}