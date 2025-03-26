#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Menus;
using Captain.Common.Views.Forms;
using System.Data.SqlClient;
using Captain.Common.Views.Controls;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using System.Text.RegularExpressions;
using Captain.Common.Views.UserControls;
using Wisej.Web;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class Sub_IncomeSource_Selection : Form
    {

        #region private variables

        private CaptainModel _model = null;

        #endregion

        public Sub_IncomeSource_Selection(string FormName, string sourceList)
        {
            InitializeComponent();
            _model = new CaptainModel();
            SourceList = sourceList;

            Fill_IncomeSource_Grid();


            this.Text = "Income Source Selection";
        }

        string SourceList = null;
        private void Fill_IncomeSource_Grid()
        {

            bool Selected = false;

            int i, j = 2;
            List<AgyTabEntity> HeatingSources = _model.lookupDataAccess.GetIncomeTypes();
            foreach (AgyTabEntity Entity in HeatingSources)
            {
                Selected = false;
                for(i= 0; i < (SourceList.Length ); )
                {
                    j = 2;

                    if (SourceList.Substring(i, (SourceList.Length - i)).Length < 2)
                        j = 1;

                    //if (SourceList.Substring(i, 2).Contains(" "))
                    //    j = 1;

                    if (SourceList.Substring(i, j).Trim() == Entity.agycode)
                    {
                        Selected = true; break;
                    }

                    i +=2;
                }
                IncSourceGrid.Rows.Add(Selected, Entity.agydesc, Entity.agycode.Trim());
            }
        }


        int Total_Count = 0;
        bool Temp = true;
        private void IncSourceGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Total_Count < 15)
            {
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 0)
                    {
                        if (IncSourceGrid.CurrentRow.Cells["Check"].Value.ToString() == Temp.ToString())
                            Total_Count++;
                    }
                }
            }
            else
                AlertBox.Show("You can Select at most '15' sources.",MessageBoxIcon.Warning);
        }


        public string GetSelectedSources()
        {
            string SelSourdesc =  null;
            foreach (DataGridViewRow dr in IncSourceGrid.Rows)
            {
               if(dr.Cells["Check"].Value.ToString() == Temp.ToString())
               {
                   if (dr.Cells["IncCode"].Value.ToString().Length == 1)
                       SelSourdesc += dr.Cells["IncCode"].Value.ToString() + " ";
                   else
                       SelSourdesc += dr.Cells["IncCode"].Value.ToString();
               }
            }
            return SelSourdesc;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}