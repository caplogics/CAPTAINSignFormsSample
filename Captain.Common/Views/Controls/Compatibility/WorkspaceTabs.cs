using System.Drawing;

namespace Captain.Common.Views.Controls.Compatibility
{
	public class WorkspaceTabs : Wisej.Web.TabControl
    {
		public override Color BackColor 
		{
			get => Color.FromName("@background"); 
		}
	}
}
