using System.Drawing;
using Wisej.Web;
using Wisej.Web.Ext.NavigationBar;

namespace Captain.Common.Views.Controls.Compatibility
{
	public partial class NavigationTabs : NavigationBar
    {
		public NavigationTabs() : base()
		{
			this.Controls.Remove(this.header);
			this.AppearanceKey = "navbar-main";
			this.items.Margin = new Padding(0);
		}

		private void InitializeComponent()
		{
			this.header.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
			this.SuspendLayout();
			// 
			// NavigationTabs
			// 
			this.Margin = new Wisej.Web.Padding(0);
			this.Name = "NavigationTabs";
			this.header.ResumeLayout(false);
			this.header.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
			this.ResumeLayout(false);

		}
	}
}
