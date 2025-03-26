namespace Captain.Common.Views.UserControls.Base
{
    partial class BaseUserControl
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
            try
            {
                base.Dispose(disposing);
            }
            catch { }
        }

        #region WiseJ UserControl Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.controlToolTip = new Wisej.Web.ToolTip(this.components);
            this.toolBar1 = new Wisej.Web.ToolBar();
            this.SuspendLayout();
            // 
            // toolBar1
            // 
            this.toolBar1.AutoOverflow = false;
            this.toolBar1.BackColor = System.Drawing.Color.FromArgb(237, 244, 250);
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.MaximumSize = new System.Drawing.Size(0, 25);
            this.toolBar1.MinimumSize = new System.Drawing.Size(0, 25);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.Padding = new Wisej.Web.Padding(2);
            this.toolBar1.Size = new System.Drawing.Size(743, 25);
            this.toolBar1.TabIndex = 1;
            this.toolBar1.TabStop = false;
            // 
            // BaseUserControl
            // 
            this.Controls.Add(this.toolBar1);
            this.Name = "BaseUserControl";
            this.Size = new System.Drawing.Size(743, 41);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected Wisej.Web.ToolTip controlToolTip;
        private Wisej.Web.ToolBar toolBar1;
    }
}