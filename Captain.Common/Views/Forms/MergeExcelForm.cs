#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Utilities;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class MergeExcelForm : Form
    {
        public MergeExcelForm(List<ListItem> listdetails)
        {
            InitializeComponent();
            listNames = new List<ListItem>();
            foreach (ListItem item in listdetails)
            {
                gvExcelMerge.Rows.Add(item.Text, item.Value.ToString(), item.ValueDisplayCode.ToString());
            }
        }

        /*public MergeExcelForm(DataGridView dataGridView)
        {
            InitializeComponent();
            //listNames = new List<ListItem>();
            
            foreach (DataGridViewRow item in gvExcelMerge.Rows)
            {
                gvExcelMerge.Rows.Add(item.Text, item.Value.ToString(), item.ValueDisplayCode.ToString());
            }
        }*/

        public List<ListItem> listNames { get; set; }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            listNames.Clear();
            foreach (DataGridViewRow  item in gvExcelMerge.Rows )
            {
                listNames.Add(new ListItem(item.Cells["gvtName"].Value.ToString(), item.Cells["gvtSeq"].Value.ToString(), string.Empty, item.Cells["gvtFileName"].Value.ToString()));
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}