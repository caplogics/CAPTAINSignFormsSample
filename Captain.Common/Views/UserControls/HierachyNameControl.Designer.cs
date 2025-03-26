using Wisej.Web;


namespace Captain.Common.Views.UserControls
{
    partial class HierachyNameControl
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

        #region Wisej UserControl Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new Wisej.Web.Panel();
            this.lblHierchy = new Wisej.Web.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblHierchy);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.MinimumSize = new System.Drawing.Size(0, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(750, 27);
            this.panel1.TabIndex = 0;
            this.panel1.TabStop = true;
            // 
            // lblHierchy
            // 
            this.lblHierchy.AllowHtml = true;
            this.lblHierchy.AutoEllipsis = true;
            this.lblHierchy.BackColor = System.Drawing.Color.FromName("@captainBlue");
            this.lblHierchy.Dock = Wisej.Web.DockStyle.Fill;
            this.lblHierchy.Font = new System.Drawing.Font("defaultBold", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblHierchy.ForeColor = System.Drawing.Color.FromArgb(246, 244, 244, 244);
            this.lblHierchy.Location = new System.Drawing.Point(0, 0);
            this.lblHierchy.Margin = new Wisej.Web.Padding(0);
            this.lblHierchy.Name = "lblHierchy";
            this.lblHierchy.Size = new System.Drawing.Size(750, 27);
            this.lblHierchy.TabIndex = 0;
            this.lblHierchy.Text = "Text ";
            this.lblHierchy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HierachyNameControl
            // 
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ForeColor = System.Drawing.Color.FromName("@captainBlue");
            this.Name = "HierachyNameControl";
            this.Size = new System.Drawing.Size(750, 27);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        public Label lblHierchy;





    }
}