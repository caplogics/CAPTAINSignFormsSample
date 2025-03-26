using System.ComponentModel;
using Wisej.Web;

namespace Captain.Common.Utilities.MixedDGVUtilities
{
	/// <summary>
	/// Represents a custom DataGridView that supports multiple cell types.
	/// </summary>
	/// <remarks>
	/// This class extends the Wisej.Web.DataGridView class to provide support for multiple cell types.
	/// It uses the MultiTypeDataGridViewColumn class to support multiple cell types for each column.
	/// The cell type for each row in the grid is determined by the value of the "CellType" property in the data source.
	/// The cell type is specified as a string value and must match one of the registered cell types.
	/// The SelectCellType event is raised when the cell type for a cell is determined.
	/// </remarks>
	public class MultiTypeDataGridView : Wisej.Web.DataGridView
	{
		/// <summary>
		/// Occurs when the cell type for a cell is determined.
		/// </summary>
		public event DataGridViewCellEventHandler SelectCellType;

		/// <summary>
		/// Initializes the control and raises the SelectCellType event for each cell in the grid.
		/// </summary>
		/// <param name="e">The DataGridViewBindingCompleteEventArgs event arguments.</param>
		protected override void OnDataBindingComplete(DataGridViewBindingCompleteEventArgs e)
		{
			switch (e.ListChangedType)
			{
				case ListChangedType.Reset:
				case ListChangedType.ItemAdded:

					if (e.ListChangedType == ListChangedType.Reset)
					{
						foreach (DataGridViewColumn column in this.Columns)
						{
							if (column is MultiTypeDataGridViewColumn)
							{
								foreach (DataGridViewRow row in this.Rows)
								{
									OnSelectCellType(new DataGridViewCellEventArgs(column.Index, row.Index));
								}
							}
						}
					}
					break;
			}

			base.OnDataBindingComplete(e);
		}

		/// <summary>
		/// Raises the SelectCellType event.
		/// </summary>
		/// <param name="e">The DataGridViewCellEventArgs event arguments.</param>
		protected virtual void OnSelectCellType(DataGridViewCellEventArgs e)
		{
			this.SelectCellType?.Invoke(this, e);
		}
	}
}
