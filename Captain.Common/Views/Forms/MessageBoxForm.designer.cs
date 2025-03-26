using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class MessageBoxForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageBoxForm));
            this.btnUpdatedt = new Wisej.Web.Button();
            this.btnPreview = new Wisej.Web.Button();
            this.btnEnergyAssis = new Wisej.Web.Button();
            this.SuspendLayout();
            // 
            // btnUpdatedt
            // 
            this.btnUpdatedt.Location = new System.Drawing.Point(70, 13);
            this.btnUpdatedt.Name = "btnUpdatedt";
            this.btnUpdatedt.Size = new System.Drawing.Size(200, 38);
            this.btnUpdatedt.TabIndex = 1;
            this.btnUpdatedt.Text = "&Review Letter Prior to Sending (Letter Date Does Not Fill)";
            this.btnUpdatedt.Click += new System.EventHandler(this.btnUpdatedt_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(70, 64);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(200, 31);
            this.btnPreview.TabIndex = 2;
            this.btnPreview.Text = "&Letter Ready to Send to Client";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnEnergyAssis
            // 
            this.btnEnergyAssis.Location = new System.Drawing.Point(70, 107);
            this.btnEnergyAssis.Name = "btnEnergyAssis";
            this.btnEnergyAssis.Size = new System.Drawing.Size(200, 31);
            this.btnEnergyAssis.TabIndex = 2;
            this.btnEnergyAssis.Text = "&Energy Assistance Application";
            this.btnEnergyAssis.Click += new System.EventHandler(this.btnEnergyAssis_Click);
            // 
            // MessageBoxForm
            // 
            this.ClientSize = new System.Drawing.Size(337, 154);
            this.Controls.Add(this.btnEnergyAssis);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.btnUpdatedt);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageBoxForm";
            this.Text = "MessageBoxForm";
            this.ResumeLayout(false);

        }


        #endregion

        private Button btnUpdatedt;
        private Button btnPreview;
        private Button btnEnergyAssis;
    }
}