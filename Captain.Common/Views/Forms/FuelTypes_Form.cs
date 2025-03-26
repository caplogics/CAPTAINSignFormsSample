#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Wisej.Design;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class FuelTypes_Form : Form
    {
        #region private variables
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion
        public FuelTypes_Form(string form_display_mode, string fueltypes)
        {
            InitializeComponent();
            _model = new CaptainModel();
            Form_Display_Mode = form_display_mode;
            FuelTypes = fueltypes;

            Fill_fueltypes();
            if (FuelTypes.Length > 0)
            {
                int Count = (FuelTypes.Length) / 2;
                Sel_Count = Count;
            }
            
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public string Form_Display_Mode { get; set; }

        public string FuelTypes { get; set; }

        public List<CaseVDD1Entity> Sel_CASEVDD1_List { get; set; }

        public string Mode { get; set; }

        #endregion


        private void Fill_fueltypes()
        {
            DataSet ds = Captain.DatabaseLayer.Lookups.GetLookUpFromAGYTAB("08004");
            DataTable dt = new DataTable();
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    bool Sel_Ref = false;
                    int rowIndex = 0;
                    string Temp_Fuel=string.Empty;
                    for (int i = 0; i < FuelTypes.Length; )
                    {
                        Temp_Fuel = FuelTypes.Substring(i, 2).Trim();
                        if (dr["Code"].ToString().Trim() == Temp_Fuel.Trim())
                        {
                            Sel_Ref = true;
                            rowIndex = gvFuels.Rows.Add(true, dr["Code"].ToString().Trim(),dr["LookUpDesc"].ToString().Trim(), "Y");
                        }
                        i += 2;
                    }
                    if (!Sel_Ref)
                        rowIndex = gvFuels.Rows.Add(false, dr["Code"].ToString().Trim(), dr["LookUpDesc"].ToString().Trim(), "N");

                }
            }
            if (gvFuels.Rows.Count > 0)
                gvFuels.Rows[0].Selected = true;
        }

        public List<CaseVDD1Entity> GetSelected_FuelTypes()
        {
            List<CaseVDD1Entity> Sel_CASEVDD1_List = new List<CaseVDD1Entity>();
            CaseVDD1Entity Add_Entity = new CaseVDD1Entity();
            string FuelList = string.Empty;
            foreach (DataGridViewRow dr in gvFuels.Rows)
            {
                if (dr.Cells["Selected"].Value.ToString() == "Y")
                {
                    Add_Entity.Rec_Type = "I";
                    Add_Entity.FuelType = dr.Cells["Code"].Value.ToString();
                    Sel_CASEVDD1_List.Add(new CaseVDD1Entity(Add_Entity));
                    FuelList += dr.Cells["Code"].Value.ToString();
                }
            }
            return Sel_CASEVDD1_List;
        }

        public string GetSelected_Fuels()
        {
            string FuelList = string.Empty;
            foreach (DataGridViewRow dr in gvFuels.Rows)
            {
                if (dr.Cells["Selected"].Value.ToString() == "Y")
                {
                    FuelList += dr.Cells["Code"].Value.ToString();
                }
            }
            return FuelList;
        }
        int Sel_Count = 0;
        private void gvFuels_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvFuels.Rows.Count > 0)
            {
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 0)
                    {
                        if (gvFuels.CurrentRow.Cells["Selected"].Value.ToString() == "Y")
                        {
                            gvFuels.CurrentRow.Cells["Check"].Value = false;
                            gvFuels.CurrentRow.Cells["Selected"].Value = "N";
                            Sel_Count--;
                        }
                        else
                        {
                            gvFuels.CurrentRow.Cells["Check"].Value = true;
                            gvFuels.CurrentRow.Cells["Selected"].Value = "Y";
                            Sel_Count++;
                            if (Sel_Count > 10 && Form_Display_Mode == "FuelType")
                            {
                                gvFuels.CurrentRow.Cells["Check"].Value = false;
                                gvFuels.CurrentRow.Cells["Selected"].Value = "N";
                                Sel_Count--;
                                AlertBox.Show("You should not select more than 10 Fuel Types", MessageBoxIcon.Warning);
                            }

                        }
                    }
                }
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (gvFuels.Rows.Count>0)
                gvFuels.Rows[0].Selected = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }



    }
}