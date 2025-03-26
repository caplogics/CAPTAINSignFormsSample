namespace Captain.Common.Views.Forms
{
    partial class FrmViewer
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmViewer));
            this.SuspendLayout();
            // 
            // FrmViewer
            // 
            this.ClientSize = new System.Drawing.Size(729, 690);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.Name = "FrmViewer";
            this.Text = "Viewer";
            this.Load += new System.EventHandler(this.frmViewer_Load);
            this.FormClosed += new Wisej.Web.FormClosedEventHandler(this.FrmViewer_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion
    }
}