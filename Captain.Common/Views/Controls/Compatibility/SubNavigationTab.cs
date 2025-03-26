using System;
using Wisej.Web;

namespace Captain.Common.Views.Controls.Compatibility
{
	public partial class SubNavigationTab : Wisej.Web.Ext.NavigationBar.NavigationBarItem
	{
		public SubNavigationTab()
		{
			InitializeComponent();

			this.Icon = "Resources/Images/bullet-grey.png";
		}
	}
}
