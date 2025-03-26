using DevExpress.XtraRichEdit.Export.WordML;
using System;
using Wisej.Web;

namespace Captain.Common.Views.Controls.Compatibility
{
	public class DataGridViewDateTimeCell : DataGridViewDateTimePickerCell
	{
		public override void InitializeEditingControl(Control editor, DataGridViewCellStyle style)
		{
			var picker = editor as DateTimePicker;
			if (picker != null && this.Value != null)
			{

				var value =
					String.IsNullOrEmpty(this.Value.ToString()) ? DateTime.MinValue : Convert.ToDateTime(this.Value);

				picker.MinDate = this.MinDate;
				picker.MaxDate = this.MaxDate;
				picker.Format = this.Format;
				picker.ShowUpDown = this.ShowUpDown;
				picker.CustomFormat = String.IsNullOrEmpty(style.Format) ? this.CustomFormat : style.Format;
				picker.Value = value;
			}
			
		}
	}
}
