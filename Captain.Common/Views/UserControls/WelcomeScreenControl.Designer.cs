namespace Captain.Common.Views.UserControls
{
    partial class WelcomeScreenControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeScreenControl));
            this.picWelcomeScreenTopLeft = new Wisej.Web.PictureBox();
            this.pnlWelcomeScreenTop = new Wisej.Web.Panel();
            this.picWelcomeScreenTopMiddle = new Wisej.Web.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picWelcomeScreenTopLeft)).BeginInit();
            this.pnlWelcomeScreenTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWelcomeScreenTopMiddle)).BeginInit();
            this.SuspendLayout();
            // 
            // picWelcomeScreenTopLeft
            // 
            this.picWelcomeScreenTopLeft.Dock = Wisej.Web.DockStyle.Left;
            this.picWelcomeScreenTopLeft.ImageSource = "resource.wx/picWelcomeScreenTopLeft.Image";
            this.picWelcomeScreenTopLeft.Location = new System.Drawing.Point(0, 0);
            this.picWelcomeScreenTopLeft.Name = "picWelcomeScreenTopLeft";
            this.picWelcomeScreenTopLeft.Size = new System.Drawing.Size(782, 480);
            // 
            // pnlWelcomeScreenTop
            // 
            this.pnlWelcomeScreenTop.Controls.Add(this.picWelcomeScreenTopMiddle);
            this.pnlWelcomeScreenTop.Controls.Add(this.picWelcomeScreenTopLeft);
            this.pnlWelcomeScreenTop.Dock = Wisej.Web.DockStyle.Top;
            this.pnlWelcomeScreenTop.Location = new System.Drawing.Point(0, 0);
            this.pnlWelcomeScreenTop.Name = "pnlWelcomeScreenTop";
            this.pnlWelcomeScreenTop.Size = new System.Drawing.Size(1000, 480);
            this.pnlWelcomeScreenTop.TabIndex = 1;
            this.pnlWelcomeScreenTop.TabStop = true;
            // 
            // picWelcomeScreenTopMiddle
            // 
            this.picWelcomeScreenTopMiddle.Dock = Wisej.Web.DockStyle.Fill;
            this.picWelcomeScreenTopMiddle.ImageSource = "resource.wx/picWelcomeScreenTopMiddle.Image";
            this.picWelcomeScreenTopMiddle.Location = new System.Drawing.Point(782, 0);
            this.picWelcomeScreenTopMiddle.Name = "picWelcomeScreenTopMiddle";
            this.picWelcomeScreenTopMiddle.Size = new System.Drawing.Size(218, 480);
            // 
            // WelcomeScreenControl
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = Wisej.Web.ImageLayout.Stretch;
            this.Controls.Add(this.pnlWelcomeScreenTop);
            this.Location = new System.Drawing.Point(15, 15);
            this.Name = "WelcomeScreenControl";
            this.Size = new System.Drawing.Size(1000, 481);
            ((System.ComponentModel.ISupportInitialize)(this.picWelcomeScreenTopLeft)).EndInit();
            this.pnlWelcomeScreenTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picWelcomeScreenTopMiddle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.PictureBox picWelcomeScreenTopLeft;
        private Wisej.Web.Panel pnlWelcomeScreenTop;
        private Wisej.Web.PictureBox picWelcomeScreenTopMiddle;

    }
}
