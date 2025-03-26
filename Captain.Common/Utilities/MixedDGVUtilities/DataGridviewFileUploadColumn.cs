
using Wisej.Web;

namespace Captain.Common.Utilities.MixedDGVUtilities
{
    /// <summary>
    /// Represents a custom DataGridView column that supports multiple cell types.
    /// </summary>
    /// <remarks>
    /// This class extends the DataGridViewColumn class to provide support for multiple cell types.
    /// The cell type for each row in the column is determined by the value of the "CellType" property in the data source.
    /// The cell type is specified as a string value and must match one of the registered cell types.
    /// </remarks>
    public class DataGridViewFileUploadColumn : Wisej.Web.DataGridViewButtonColumn
    {
        public DataGridViewFileUploadColumn() : base(new DataGridViewFileUploadCell())
        {
        }
    }
}