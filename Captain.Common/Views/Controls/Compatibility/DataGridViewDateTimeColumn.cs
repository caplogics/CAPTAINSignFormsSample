using System;
using Wisej.Web;

namespace Captain.Common.Views.Controls.Compatibility
{
	/// <summary>
	/// Implements a DateTimePicker columns that handles string values as dates.
	/// </summary>
	public class DataGridViewDateTimeColumn : DataGridViewDateTimePickerColumn
	{

		public DataGridViewDateTimeColumn() : base(new DataGridViewDateTimeCell())
		{
			this.CustomFormat = "MM/dd/yyyy";
			this.Format = DateTimePickerFormat.Custom;
		}

		public override Type ValueType
		{
			get { return typeof(string); }
			set { }
		}
	}
}
