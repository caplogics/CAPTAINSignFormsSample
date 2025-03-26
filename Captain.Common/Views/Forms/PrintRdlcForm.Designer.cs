using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class PrintRdlcForm
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
            this.pdfViewer = new Wisej.Web.PdfViewer();
            this.SuspendLayout();
            // 
            // pdfViewer
            // 
            this.pdfViewer.Dock = Wisej.Web.DockStyle.Fill;
            this.pdfViewer.Location = new System.Drawing.Point(0, 0);
            this.pdfViewer.Name = "pdfViewer";
            this.pdfViewer.Size = new System.Drawing.Size(684, 498);
            this.pdfViewer.TabIndex = 0;
            // 
            // PrintRdlcForm
            // 
            this.ClientSize = new System.Drawing.Size(684, 498);
            this.Controls.Add(this.pdfViewer);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.MinimizeBox = false;
            this.Name = "PrintRdlcForm";
            this.Text = "Report";
            this.Load += new System.EventHandler(this.PrintRdlcForm_Load);
            this.ResumeLayout(false);

        }

        private PdfViewer pdfViewer;

        #endregion


    }
}