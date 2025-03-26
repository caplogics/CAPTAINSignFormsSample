using System;
using System.ComponentModel;
using Wisej.Web;

namespace Captain.Common.Views.Controls.Compatibility
{
    public partial class DateCustControl : Wisej.Web.UserControl
    {
        public DateCustControl()
        {
            InitializeComponent();
        }
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox1.Checked)
			{
				//enable typing in the textbox
				textBox1.ReadOnly = false;
			}
			else
			{
				textBox1.ReadOnly = true;
			}
		}

		private void MyDateTextbox_Load(object sender, EventArgs e)
		{
			//textBox1.Text = DateTime.Now.ToString();
		}

		[Description("Checkbox checked value"), Category("Data")]
		public bool Checked
		{
			get => checkBox1.Checked;
			set { checkBox1.Checked = value; RaiseCheckedChanged(); }
		}

		[Description("Test text displayed in the textbox"), Category("Data")]
		public string Text
		{
			get => textBox1.Text;
			set => textBox1.Text = value;
		}

		public event EventHandler CheckedChanged;
		private void RaiseCheckedChanged()
		{
			if (CheckedChanged != null)
				CheckedChanged(this, EventArgs.Empty);
		}
	}
}
