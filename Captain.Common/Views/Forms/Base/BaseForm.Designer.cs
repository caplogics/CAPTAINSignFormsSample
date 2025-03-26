namespace Captain.Common.Views.Forms.Base
{
    partial class BaseForm
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
            this.SuspendLayout();
            // 
            // BaseForm
            // 
            this.Name = "BaseForm";
            this.Size = new System.Drawing.Size(1831, 812);
            this.ResumeLayout(false);

        }

        #endregion

        //public Captain.Common.Views.Controls.ScriptManagerControl ScriptManager;
        //public Wisej.Web.ToolTip ControlsTooltip;
        //public Captain.Common.Views.Controls.FriendlyMessage FriendlyMessage;
    }
}