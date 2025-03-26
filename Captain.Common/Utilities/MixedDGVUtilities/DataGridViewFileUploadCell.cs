using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Wisej.Web;

namespace Captain.Common.Utilities.MixedDGVUtilities
{
    public class DataGridViewFileUploadCell : DataGridViewButtonCell
    {
        public DataGridViewFileUploadCell()
        {
            this.Control = new Upload()
            {
                Dock = DockStyle.Fill
                //Maximum = 100
            };
        }
    }
}
