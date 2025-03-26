using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;


namespace Captain.Common.Views.UserControls
{
    partial class AlertCodes
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
            this.components = new System.ComponentModel.Container();
            this.btnAlertCodes = new Wisej.Web.Button();
            this.contextMenu1 = new Wisej.Web.ContextMenu(this.components);
            this.txtAlertCodes = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lnlAlertCode = new Wisej.Web.Label();
            this.spacer1 = new Wisej.Web.Spacer();
            this.spacer2 = new Wisej.Web.Spacer();
            this.SuspendLayout();
            // 
            // btnAlertCodes
            // 
            this.btnAlertCodes.Display = Wisej.Web.Display.Icon;
            this.btnAlertCodes.Dock = Wisej.Web.DockStyle.Left;
            this.btnAlertCodes.Enabled = false;
            this.btnAlertCodes.ImageSource = "captain-more?color=captainGray";
            this.btnAlertCodes.Location = new System.Drawing.Point(226, 0);
            this.btnAlertCodes.Name = "btnAlertCodes";
            this.btnAlertCodes.Size = new System.Drawing.Size(25, 27);
            this.btnAlertCodes.TabIndex = 12;
            this.btnAlertCodes.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAlertCodes.Click += new System.EventHandler(this.btnAlertCodes_Click);
            // 
            // contextMenu1
            // 
            this.contextMenu1.Name = "contextMenu1";
            this.contextMenu1.RightToLeft = Wisej.Web.RightToLeft.No;
            // 
            // txtAlertCodes
            // 
            this.txtAlertCodes.ContextMenu = this.contextMenu1;
            this.txtAlertCodes.Dock = Wisej.Web.DockStyle.Left;
            this.txtAlertCodes.Location = new System.Drawing.Point(106, 0);
            this.txtAlertCodes.MaxLength = 12;
            this.txtAlertCodes.Name = "txtAlertCodes";
            this.txtAlertCodes.ReadOnly = true;
            this.txtAlertCodes.Size = new System.Drawing.Size(97, 27);
            this.txtAlertCodes.TabIndex = 11;
            // 
            // lnlAlertCode
            // 
            this.lnlAlertCode.Dock = Wisej.Web.DockStyle.Left;
            this.lnlAlertCode.Location = new System.Drawing.Point(15, 0);
            this.lnlAlertCode.Name = "lnlAlertCode";
            this.lnlAlertCode.Size = new System.Drawing.Size(71, 27);
            this.lnlAlertCode.TabIndex = 10;
            this.lnlAlertCode.Text = "Alert Codes";
            this.lnlAlertCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(86, 0);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(20, 27);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(203, 0);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(23, 27);
            // 
            // AlertCodes
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnAlertCodes);
            this.Controls.Add(this.spacer2);
            this.Controls.Add(this.txtAlertCodes);
            this.Controls.Add(this.spacer1);
            this.Controls.Add(this.lnlAlertCode);
            this.Name = "AlertCodes";
            this.Padding = new Wisej.Web.Padding(15, 0, 0, 0);
            this.Size = new System.Drawing.Size(625, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnAlertCodes;
        private TextBoxWithValidation txtAlertCodes;
        private Label lnlAlertCode;
        private ContextMenu contextMenu1;
        private Spacer spacer1;
        private Spacer spacer2;
    }
}