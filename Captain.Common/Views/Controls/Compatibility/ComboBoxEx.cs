using Captain.Common.Utilities;
using System;
using System.ComponentModel;
using System.Drawing;
using Wisej.Web;

namespace Captain.Common.Views.Controls.Compatibility
{
	/// <summary>
	/// Extends the <see cref="ComboBox"/> control to render
	/// a color block in each item.
	/// </summary>
	public class ComboBoxEx : ComboBox
	{
		public ComboBoxEx()
		{
		}

		/// <summary>
		/// Property that defines the color to render.
		/// </summary>
		public string ColorMember
		{
			get;
			set;
		}

		protected override dynamic RenderItem(object item)
		{
			var config = base.RenderItem(item);

			if (!String.IsNullOrEmpty(this.ColorMember))
				config.label = $"<div class='combobox-item' style='color:{GetColor(item)}'><div class='combobox-color' style='background-color:{GetColor(item)}'></div>{config.label}</div>";

			return config;
		}

		private string GetColor(object item)
		{
			if (item == null)
				return string.Empty;

			_colorMember = _colorMember ?? (TypeDescriptor.GetProperties(typeof(ListItem))[this.ColorMember]);
			
			var color = _colorMember.GetValue(item);
			
			return 
				color is Color
					? ColorTranslator.ToHtml((Color)color)
					: Convert.ToString(color);
		}
		private static PropertyDescriptor _colorMember;
	}
}
