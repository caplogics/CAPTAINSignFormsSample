using Wisej.Web.Ext.NavigationBar;

namespace Captain.Common.Views.Controls.Compatibility
{
	public partial class NavigationTab : NavigationBarItem
	{
		public NavigationTab() : base()
		{
			this.AppearanceKey = "navbar-item-main";
			this.header.AppearanceKey = "navbar-item-main/header";
		}
	
		private void InitializeComponent()
		{
			this.header.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.shortcut)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.open)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.icon)).BeginInit();
			this.SuspendLayout();
			// 
			// items
			// 
			this.items.Size = new System.Drawing.Size(0, 0);
			// 
			// header
			// 
			this.header.Size = new System.Drawing.Size(0, 45);
			// 
			// shortcut
			// 
			this.shortcut.Location = new System.Drawing.Point(94, 12);
			// 
			// info
			// 
			this.info.Location = new System.Drawing.Point(124, 12);
			// 
			// open
			// 
			this.open.Location = new System.Drawing.Point(140, 12);
			// 
			// title
			// 
			this.title.ImageSource = "captain-add";
			this.title.InitScript = "debugger;";
			this.title.Location = new System.Drawing.Point(82, 0);
			this.title.Size = new System.Drawing.Size(0, 45);
			// 
			// icon
			// 
			this.icon.Location = new System.Drawing.Point(25, 0);
			this.icon.MaximumSize = new System.Drawing.Size(0, 0);
			this.icon.Size = new System.Drawing.Size(45, 45);
			// 
			// NavigationTab
			// 
			this.BackColor = System.Drawing.Color.FromName("transparent");
			this.Name = "NavigationTab";
			this.header.ResumeLayout(false);
			this.header.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.shortcut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.open)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
