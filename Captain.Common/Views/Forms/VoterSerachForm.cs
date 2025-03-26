#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Wisej.Web;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class VoterSerachForm : Form
    {
        public VoterSerachForm(string strstreet,List<CASEVOTEntity> casevotentity)
        {
            InitializeComponent();
            txtCity.Text = strstreet;
            lblTotal.Text = string.Empty;
            cmbstreet.Items.Add(new ListItem("STREET", "STREET"));
            cmbstreet.Items.Add(new ListItem("CITY", "CITY"));
            CommonFunctions.SetComboBoxValue(cmbstreet, "STREET");
            propcasevotentity = casevotentity;
        }

        List<CASEVOTEntity> propcasevotentity { get; set; }
        private void onSearch_Click(object sender, EventArgs e)
        {
            List<CASEVOTEntity> casevotelist = new List<CASEVOTEntity>();
            if (((ListItem)cmbstreet.SelectedItem).Value.ToString().Equals("STREET"))
            {
                casevotelist = propcasevotentity.FindAll(u => u.Street.ToUpper().StartsWith(txtCity.Text.ToUpper()));
            }
            else
            {
                casevotelist = propcasevotentity.FindAll(u => u.City.ToUpper().StartsWith(txtCity.Text.ToUpper()));
            }
            gvwvoters.Rows.Clear();
            foreach (CASEVOTEntity item in casevotelist)
            {
               int index =  gvwvoters.Rows.Add(item.Block, item.EO, item.City, item.Direction, item.Street, item.Suffix, item.Precinct);
               gvwvoters.Rows[index].Tag = item;
            }
            if (gvwvoters.Rows.Count > 0)
                lblTotal.Text = "Total Records :  " + gvwvoters.Rows.Count;
            else
                lblTotal.Text = "Total Records : 0 ";
        }

        public CASEVOTEntity GetSelectedVoterdetails()
        {
            CASEVOTEntity voterdetails = null;
            if (gvwvoters != null)
            {
                foreach (DataGridViewRow dr in gvwvoters.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        voterdetails = dr.Tag as CASEVOTEntity;
                    }
                }
            }
            return voterdetails;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (gvwvoters.Rows.Count > 0)
            {
                if (gvwvoters.SelectedRows[0].Selected)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }
    }
}