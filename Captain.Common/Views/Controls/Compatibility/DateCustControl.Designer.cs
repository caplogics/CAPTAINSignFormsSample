namespace Captain.Common.Views.Controls.Compatibility
{
	partial class DateCustControl
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
            this.checkBox1 = new Wisej.Web.CheckBox();
            this.textBox1 = new Wisej.Web.TextBox();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.Dock = Wisej.Web.DockStyle.Left;
            this.checkBox1.Location = new System.Drawing.Point(0, 0);
            this.checkBox1.MaximumSize = new System.Drawing.Size(23, 0);
            this.checkBox1.MinimumSize = new System.Drawing.Size(23, 0);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(23, 24);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = Wisej.Web.BorderStyle.None;
            this.textBox1.Dock = Wisej.Web.DockStyle.Left;
            this.textBox1.InputType.Type = Wisej.Web.TextBoxType.Date;
            this.textBox1.Location = new System.Drawing.Point(23, 0);
            this.textBox1.Margin = new Wisej.Web.Padding(3, 3, 0, 3);
            this.textBox1.MaxLength = 10;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(98, 24);
            this.textBox1.TabIndex = 3;
            // 
            // DateCustControl
            // 
            this.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.checkBox1);
            this.CssStyle = "border-radius:5px;";
            this.Name = "DateCustControl";
            this.Size = new System.Drawing.Size(133, 26);
            this.Load += new System.EventHandler(this.MyDateTextbox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        #endregion

        public Wisej.Web.CheckBox checkBox1;
        public Wisej.Web.TextBox textBox1;
    }
}
