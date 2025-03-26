namespace Captain.Common.Views.UserControls.Base
{
    partial class ToolbarMenu
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

        #region Wisej Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolBar1 = new Wisej.Web.ToolBar();
            this.SuspendLayout();
            // 
            // toolBar1
            // 
            this.toolBar1.BackColor = System.Drawing.Color.FromArgb(240, 245, 247);
            this.toolBar1.Dock = Wisej.Web.DockStyle.Fill;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.Size = new System.Drawing.Size(738, 26);
            this.toolBar1.TabIndex = 0;
            this.toolBar1.TabStop = false;
            // 
            // ToolbarMenu
            // 
            this.Controls.Add(this.toolBar1);
            this.Name = "ToolbarMenu";
            this.Size = new System.Drawing.Size(738, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Wisej.Web.ToolBar toolBar1;
    }
}
