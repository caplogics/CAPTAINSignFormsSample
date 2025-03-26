using System;
using System.Drawing;
using Wisej.Web;
using Wisej.Web.Ext.NavigationBar;

namespace Captain.Common.Views.Controls.Compatibility
{
	public partial class SubNavigationTabs : NavigationBar
    {
		public SubNavigationTabs() : base()
		{
			this.Controls.Remove(this.header);
			this.items.Margin = new Padding(0);

			InitializeComponent();
		}

		/// <summary>
		/// Apply theme colors.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSelectedItemChanged(EventArgs e)
		{
			base.OnSelectedItemChanged(e);

			if (this._selectedItem != null)
				SetUnselectedColors(this._selectedItem);

			if (this.SelectedItem != null)
				SetSelectedColors(this.SelectedItem);

			this._selectedItem = this.SelectedItem;
		}
		private NavigationBarItem _selectedItem;

		private void SetSelectedColors(NavigationBarItem navigationBarItem)
		{
			if (navigationBarItem.Level == 0)
			{
				navigationBarItem.ForeColor = Color.White;
			}
			else
			{
				navigationBarItem.BackColor = System.Drawing.ColorTranslator.FromHtml("#d8e7f0");
				navigationBarItem.Icon = "Resources/Images/ScrRep.png";
				navigationBarItem.ForeColor = Color.FromName("@captainBlue");

				navigationBarItem.Parent.ForeColor = Color.White;
				navigationBarItem.Parent.BackColor = Color.FromName("@navbar-background-selected");
			}
		}

		private void SetUnselectedColors(NavigationBarItem navigationBarItem)
		{
			if (navigationBarItem.Level == 0)
			{
				navigationBarItem.ForeColor = Color.FromName("@navbar-sub-text");
			}
			else
			{
				navigationBarItem.BackColor = Color.Transparent;
				navigationBarItem.Icon = "Resources/Images/ScrRep-gray.png";
				navigationBarItem.ForeColor = Color.FromName("@navbar-sub-text");

				navigationBarItem.Parent.BackColor = Color.Transparent;
				navigationBarItem.Parent.ForeColor = Color.FromName("@navbar-sub-text");
			}
		}

		private void InitializeComponent()
		{
			this.header.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
			this.SuspendLayout();
			// 
			// SubNavigationTabs
			// 
			this.ItemHeight = 35;
			this.Indentation = 20;
			this.Margin = new Wisej.Web.Padding(0);
			this.Name = "SubNavigationTabs";
			this.header.ResumeLayout(false);
			this.header.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
			this.ResumeLayout(false);

		}
	}
}
