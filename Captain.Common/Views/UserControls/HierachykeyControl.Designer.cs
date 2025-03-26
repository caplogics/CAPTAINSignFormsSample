using Wisej.Web;

namespace Captain.Common.Views.UserControls
{
    partial class HierachykeyControl
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

        #region Visual WebGui UserControl Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtMainHierchy = new Wisej.Web.TextBox();
            this.SuspendLayout();
            // 
            // txtMainHierchy
            // 
            this.txtMainHierchy.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtMainHierchy.Location = new System.Drawing.Point(1, 0);
            this.txtMainHierchy.Name = "txtMainHierchy";
            this.txtMainHierchy.ReadOnly = true;
            this.txtMainHierchy.Size = new System.Drawing.Size(474, 20);
            this.txtMainHierchy.TabIndex = 0;
            this.txtMainHierchy.TextAlign = Wisej.Web.HorizontalAlignment.Center;
            // 
            // HierachykeyControl
            // 
            this.Controls.Add(this.txtMainHierchy);
            this.Size = new System.Drawing.Size(475, 20);
            this.Text = "HierachykeyControl";
            this.ResumeLayout(false);

        }

        #endregion

        public TextBox txtMainHierchy;




    }
}