using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class AlertCodesForm
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
            this.checkedListBox = new Wisej.Web.CheckedListBox();
            this.flowLayoutPanel1 = new Wisej.Web.FlowLayoutPanel();
            this.btnOk = new Wisej.Web.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkedListBox
            // 
            this.checkedListBox.BorderStyle = Wisej.Web.BorderStyle.None;
            this.checkedListBox.Dock = Wisej.Web.DockStyle.Fill;
            this.checkedListBox.Location = new System.Drawing.Point(0, 0);
            this.checkedListBox.Name = "checkedListBox";
            this.checkedListBox.Size = new System.Drawing.Size(334, 225);
            this.checkedListBox.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromName("@control");
            this.flowLayoutPanel1.Controls.Add(this.btnOk);
            this.flowLayoutPanel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = Wisej.Web.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 225);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new Wisej.Web.Padding(3);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(334, 45);
            this.flowLayoutPanel1.TabIndex = 1;
            this.flowLayoutPanel1.TabStop = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((Wisej.Web.AnchorStyles)((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Right)));
            this.btnOk.Font = new System.Drawing.Font("@buttonText", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btnOk.Location = new System.Drawing.Point(225, 6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 33);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.OnOkClick);
            // 
            // AlertCodesForm
            // 
            this.ClientSize = new System.Drawing.Size(334, 270);
            this.Controls.Add(this.checkedListBox);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlertCodesForm";
            this.Text = "Alert Codes Selection";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CheckedListBox checkedListBox;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnOk;
    }
}