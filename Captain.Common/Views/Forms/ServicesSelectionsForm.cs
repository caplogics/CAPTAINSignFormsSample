#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Wisej.Web;
//using Gizmox.WebGUI.Common;
//using Wisej.Web;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class ServicesSelectionsForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion
        public ServicesSelectionsForm(BaseForm baseForm, PrivilegeEntity privileges, string strMode, string strType, string strAgency, string strDept, string strProgram, string strserviceplanlist, string strservicelist)
        {
            InitializeComponent();
            _model = new CaptainModel();

            BaseForm = baseForm;
            Privileges = privileges;
            PropAgency = strAgency; propDept = strDept; propProgram = strProgram;
            propYear = string.Empty; ;
            propMode = strMode;
            propType = strType;
            propserviceplanlist = strserviceplanlist;
            propserviceslist = strservicelist;

            if (strType == "SERVICEPLAN")
            {
                this.Text = "Select Service Plan";
                Fill_SP_Grid();

            }
            else
            {
                this.Text = "Select Services";
                propCaseSp2List = _model.SPAdminData.Get_CASESP2(null, null, null, null, "SP2");
                Service_Plan.HeaderText = "Service(s)";
                Fill_SP2_Grid();
            }
        }

        string PropAgency = string.Empty, propDept = string.Empty, propProgram = string.Empty, propYear = string.Empty;

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string propMode { get; set; }

        public string propType { get; set; }
        public List<CASESP1Entity> propcasesplist { get; set; }

        //public List<CAMASTEntity> propCAList { get; set; }

        //public List<MSMASTEntity> propMSList { get; set; }

        //public string propReportPath { get; set; }

        //public bool IsSaveValid { get; set; }

        //public List<SPCommonEntity> propfundingSource { get; set; }

        public List<CASESP2Entity> propCaseSp2List { get; set; }
        public List<CASESP2Entity> propCaseSp2selList { get; set; }
        public string propserviceplanlist { get; set; }

        public string propserviceslist { get; set; }

        //public List<HierarchyEntity> propCaseworkerList = new List<HierarchyEntity>();

        //List<CaseHierarchyEntity> propCaseHieNameEntity { get; set; }

        //public List<CaseSiteEntity> propCaseAllSiteEntity { get; set; }

        #endregion

        //string Img_Blank = Consts.Icons.ico_Blank;
        string Img_Blank = "blank";
        private void btnOk_Click(object sender, EventArgs e)
        {
            //int count = 0;
            //foreach (DataGridViewRow dr in gvSps.Rows)
            //{
            //    if (dr.Cells["Selected"].Value.ToString() == "Y")
            //    {
            //        count = 1;
            //        break;

            //    }
            //}
            //if (count > 0)
            //{
                this.DialogResult = DialogResult.OK;
                this.Close();
            //}
            //else
            //{
            //    if (propType == "SERVICEPLAN")
            //        CommonFunctions.MessageBoxDisplay("At least one service plan(s) selected");
            //    else
            //        CommonFunctions.MessageBoxDisplay("At least one service(s) selected");
            //}
        }

        //string Img_Tick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick-icon10.png");
        string Img_Tick = "icon-gridtick";

        private void gvSps_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (gvSps.Rows.Count > 0)
            {
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 0)
                    {
                        if (gvSps.CurrentRow.Cells["Selected"].Value.ToString() == "Y")
                        {
                            gvSps.CurrentRow.Cells["SP_Sel"].Value = Img_Blank;
                            gvSps.CurrentRow.Cells["Selected"].Value = "N";
                        }
                        else
                        {
                            gvSps.CurrentRow.Cells["SP_Sel"].Value = Img_Tick;
                            gvSps.CurrentRow.Cells["Selected"].Value = "Y";
                        }
                    }
                }
            }
        }

        private void Fill_SP_Grid()
        {
            gvSps.Rows.Clear();
            List<CASESP1Entity> SP_Hierarchies = new List<CASESP1Entity>();
            string[] strsplistsarry = propserviceplanlist.Split(',');
            string SPAgy = string.Empty; string SPDept = string.Empty, SPProg = string.Empty;
            if (PropAgency == "**") SPAgy = null; else SPAgy = PropAgency; if (propDept == "**") SPDept = null; else SPDept = propDept;
            if (propProgram == "**") SPProg = null; else SPProg = propProgram;
            SP_Hierarchies = _model.SPAdminData.Browse_CASESP1(null, SPAgy, SPDept, SPProg,"");
            string Sp_Plan = string.Empty, Priv_Sp_Plan = string.Empty;
            if (SP_Hierarchies.Count > 0)
            {
                int index = 0;
                string SP_Valid = null;
                foreach (CASESP1Entity Entity in SP_Hierarchies)  // 08122012
                {
                    Sp_Plan = Entity.Code.Trim();
                    if (Sp_Plan != Priv_Sp_Plan)
                    {
                        SP_Valid = Entity.SP_validated;
                        string Sel_code = "N";
                        if (strsplistsarry.Length > 0)
                        {
                            foreach (string Entity1 in strsplistsarry)
                            {
                                if (Entity1.Trim() == Entity.Code.Trim())
                                {
                                    Sel_code = "Y"; break;
                                }
                            }
                        }

                        if (Sel_code == "Y")
                            index = gvSps.Rows.Add(Img_Tick, Entity.SP_Desc.Trim(), Entity.Code.ToString(), "Y");
                        else
                            index = gvSps.Rows.Add(Img_Blank, Entity.SP_Desc.Trim(), Entity.Code.ToString(), "N");
                        gvSps.Rows[index].Tag = Entity;
                        Priv_Sp_Plan = Sp_Plan;
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Fill_SP2_Grid()
        {
            gvSps.Rows.Clear();

            int index = 0;
            string SP_Valid = null;
            string[] strsplistsarry = propserviceplanlist.Split(',');
            string[] strservicearry = propserviceslist.Split(',');
            foreach (string Entity in strsplistsarry)  // 08122012
            {
                List<CASESP2Entity> spscodewiselist = propCaseSp2List.FindAll(u => u.ServPlan.Trim() == Entity.Trim() && u.Type1 == "CA");
                string Sel_code = "N";
                foreach (CASESP2Entity sp2item in spscodewiselist)
                {
                    Sel_code = "N";
                    if (strservicearry.Length > 0)
                    {
                        foreach (string Entity1 in strservicearry)
                        {
                            if (Entity1.Trim() == sp2item.CamCd.Trim())
                            {
                                Sel_code = "Y"; break;
                            }
                        }
                    }
                    if (Sel_code == "Y")
                        index = gvSps.Rows.Add(Img_Tick, sp2item.CAMS_Desc.Trim(), sp2item.CamCd.ToString(), "Y");
                    else
                        index = gvSps.Rows.Add(Img_Blank, sp2item.CAMS_Desc.Trim(), sp2item.CamCd.ToString(), "");
                    gvSps.Rows[index].Tag = sp2item;
                }
            }

        }


        public List<CASESP1Entity> GetSelected_spslist_Entity()
        {
            List<CASESP1Entity> Select_sps_List = new List<CASESP1Entity>();

            foreach (DataGridViewRow dr in gvSps.Rows)
            {
                if (dr.Cells["Selected"].Value.ToString() == "Y")
                {
                    CASESP1Entity Add_Entity = dr.Tag as CASESP1Entity;
                    if (Add_Entity != null)
                        Select_sps_List.Add(Add_Entity);
                }
            }

            return Select_sps_List;
        }

        public List<CASESP2Entity> GetSelected_sp2list_Entity()
        {
            List<CASESP2Entity> Select_sp2_List = new List<CASESP2Entity>();

            foreach (DataGridViewRow dr in gvSps.Rows)
            {
                if (dr.Cells["Selected"].Value.ToString() == "Y")
                {
                    CASESP2Entity Add_Entity = dr.Tag as CASESP2Entity;
                    if (Add_Entity != null)
                        Select_sp2_List.Add(Add_Entity);
                }
            }

            return Select_sp2_List;
        }
    }
}