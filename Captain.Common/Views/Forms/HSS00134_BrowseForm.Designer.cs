namespace Captain.Common.Views.Forms
{
    partial class HSS00134_BrowseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSS00134_BrowseForm));
            this.uploadBrowser = new Wisej.Web.Upload();
            this.SuspendLayout();
            // 
            // uploadBrowser
            // 
            this.uploadBrowser.AllowedFileTypes = "file_extension|image/*";
            this.uploadBrowser.HideValue = true;
            this.uploadBrowser.Location = new System.Drawing.Point(62, 23);
            this.uploadBrowser.Name = "uploadBrowser";
            this.uploadBrowser.Size = new System.Drawing.Size(146, 40);
            this.uploadBrowser.TabIndex = 9;
            this.uploadBrowser.Text = " Browse Document";
            this.uploadBrowser.Uploaded += new Wisej.Web.UploadedEventHandler(this.uploadBrowser_Uploaded);
            // 
            // HSS00134_BrowseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 96);
            this.Controls.Add(this.uploadBrowser);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSS00134_BrowseForm";
            this.Text = "Browse";
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Upload uploadBrowser;
    }
}