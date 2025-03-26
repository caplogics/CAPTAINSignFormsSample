using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wisej.Web;
using Captain.Common.Model.Data;
using System.Globalization;

namespace Captain.Common.Views.Controls.Compatibility
{
	/// <summary>
	/// Custom sorting of DataGridViewDateTimeColumn cells as dates.
	/// </summary>
	public class DataGridViewEx : DataGridView
	{

		protected override void OnSortCompare(DataGridViewSortCompareEventArgs e)
		{
			if (e.Column is DataGridViewDateTimeColumn)
			{

				//Convert.ToDateTime(LookupDataAccess.Getdate2(e.CellValue1.ToString()));
				//var value1 = String.IsNullOrEmpty((string)e.CellValue1) ? DateTime.MinValue : Convert.ToDateTime(e.CellValue1);
				//var value2 = String.IsNullOrEmpty((string)e.CellValue2) ? DateTime.MinValue : Convert.ToDateTime(e.CellValue2);
				if (e.CellValue1.ToString().Trim().Length > 10 && e.CellValue2.ToString().Trim().Length > 0)
				{
					var value1 = String.IsNullOrEmpty((string)e.CellValue1.ToString().Trim()) ? DateTime.MinValue : Convert.ToDateTime(DateTime.ParseExact(e.CellValue1.ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture));
					var value2 = String.IsNullOrEmpty((string)e.CellValue2.ToString().Trim()) ? DateTime.MinValue : Convert.ToDateTime(DateTime.ParseExact(e.CellValue2.ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture));
					e.SortResult = DateTime.Compare(value1, value2);
				}
				else
				{
                    //var value1 = String.IsNullOrEmpty((string)e.CellValue1.ToString().Trim()) ? DateTime.MinValue : Convert.ToDateTime(DateTime.ParseExact(e.CellValue1.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture));
                    //var value2 = String.IsNullOrEmpty((string)e.CellValue2.ToString().Trim()) ? DateTime.MinValue : Convert.ToDateTime(DateTime.ParseExact(e.CellValue2.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture));

                    var value1 = String.IsNullOrEmpty((string)e.CellValue1.ToString().Trim()) ? DateTime.MinValue.ToString("MM/dd/yyyy") : Convert.ToDateTime(e.CellValue1.ToString()).ToString("MM/dd/yyyy");
                    var value2 = String.IsNullOrEmpty((string)e.CellValue2.ToString().Trim()) ? DateTime.MinValue.ToString("MM/dd/yyyy") : Convert.ToDateTime(e.CellValue2.ToString()).ToString("MM/dd/yyyy");


                    e.SortResult = DateTime.Compare(Convert.ToDateTime(value1), Convert.ToDateTime(value2));
                }

				e.Handled = true;
			}
			else if (e.Column is DataGridViewNumberColumn)
			{
				var value1 = String.IsNullOrEmpty((string)e.CellValue1) ? Double.MinValue : Convert.ToDouble(e.CellValue1);
				var value2 = String.IsNullOrEmpty((string)e.CellValue2) ? Double.MinValue : Convert.ToDouble(e.CellValue2);
				e.SortResult = value1 < value2 ? -1 : value1 > value2 ? 1 : 0;

				e.Handled = true;
			}

			base.OnSortCompare(e);
		}
	}
}
