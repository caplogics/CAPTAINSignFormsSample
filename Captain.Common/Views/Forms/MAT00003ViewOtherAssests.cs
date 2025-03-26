/************************************************************************
 * Conversion On    :   12/14/2022      * Converted By     :   Kranthi
 * Modified On      :   12/14/2022      * Modified By      :   Kranthi
 * **********************************************************************/
#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Wisej.Web;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class MAT00003ViewOtherAssests : Form
    {
        private CaptainModel _model = null;
        public MAT00003ViewOtherAssests(BaseForm baseForm, PrivilegeEntity privilieges, string Benchmark, string strMatrixCode, List<MATDEFBMEntity> MATADEFBMentityDetails, string strScaleCode)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privilieges;
            //lblHeader.Text = privilieges.PrivilegeName;
            this.Text = /*privilieges.PrivilegeName + " - */"View Other Assessments";
            CaseMstAllList = _model.CaseMstData.GetCaseMstSSno(string.Empty, string.Empty, string.Empty, string.Empty);
            strBenchMark = Benchmark;
            strMatcode = strMatrixCode;
            MATADEFBMentity = MATADEFBMentityDetails;
            propScaleCode = strScaleCode;
            fillGrid();
        }

        #region properties
        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public List<CaseMstEntity> CaseMstAllList { get; set; }
        public List<MATDEFBMEntity> MATADEFBMentity { get; set; }
        public string strMatcode { get; set; }
        public string strBenchMark { get; set; }
        public string propScaleCode { get; set; }

        //  public string ScrCode { get; set; }
        #endregion

        private void fillGrid()
        {
            //MATDEFBMEntity Search_Entity = new MATDEFBMEntity(true);
            //Search_Entity.MatCode = strMatcode;
            //MATADEFBMentity = _model.MatrixScalesData.Browse_MATDEFBM(Search_Entity, "Browse");

            CaseMstEntity caseMstAlllistEntity = CaseMstAllList.Find(u => u.ApplAgency.Equals(BaseForm.BaseAgency) && u.ApplDept.Equals(BaseForm.BaseDept) && u.ApplProgram.Equals(BaseForm.BaseProg) && u.ApplYr.Trim().Equals(BaseForm.BaseYear.Trim()) && u.ApplNo.Equals(BaseForm.BaseApplicationNo));
            List<CaseMstEntity> CaseMstSsnoList = CaseMstAllList.FindAll(u => u.Ssn.Equals(caseMstAlllistEntity.Ssn));
            int rowIndex = 0;
            foreach (CaseMstEntity caseMstssnoItem in CaseMstSsnoList)
            {
                List<MATASMTEntity> MATASMTEntityList = _model.MatrixScalesData.GETMatasmt(caseMstssnoItem.ApplAgency.ToString(), caseMstssnoItem.ApplDept.ToString(), caseMstssnoItem.ApplProgram, caseMstssnoItem.ApplYr, caseMstssnoItem.ApplNo.ToString(), strMatcode, string.Empty, "S", string.Empty, string.Empty, string.Empty);

                foreach (MATASMTEntity MATASMTEntityItemList in MATASMTEntityList)
                {
                    if (strBenchMark == "Y")
                    {
                        MATDEFBMEntity MatadefrowEntity = MATADEFBMentity.Find(u => Convert.ToInt32(u.Low) <= Convert.ToInt32(MATASMTEntityItemList.Points) && Convert.ToInt32(MATASMTEntityItemList.Points) <= Convert.ToInt32(u.High));

                        rowIndex = gvwAssessts.Rows.Add(caseMstssnoItem.ApplAgency.ToString() + "-" + caseMstssnoItem.ApplDept.ToString() + "-" + caseMstssnoItem.ApplProgram + "   " + caseMstssnoItem.ApplYr + "   " + caseMstssnoItem.ApplNo.ToString(), MATASMTEntityItemList.ScrDesc, string.Empty, string.Empty, LookupDataAccess.Getdate(MATASMTEntityItemList.SSDate));
                        if (MatadefrowEntity != null)
                        {
                            gvwAssessts.Rows[rowIndex].Cells["BenchMark"].Value = MatadefrowEntity.Desc;
                        }
                        if (MATASMTEntityItemList.ByPass.Equals("Y"))
                        {
                            gvwAssessts.Rows[rowIndex].Cells["BenchMark"].Value = "Scale Bypassed";
                            gvwAssessts.Rows[rowIndex].Cells["ColScore"].Value = "";
                        }
                        else
                        {
                            gvwAssessts.Rows[rowIndex].Cells["ColScore"].Value = MATASMTEntityItemList.Points;
                            if (MATASMTEntityItemList.RespExcel.Trim().ToString() == "Y")
                            {
                                gvwAssessts.Rows[rowIndex].Cells["ColScore"].Value = string.Empty;
                                gvwAssessts.Rows[rowIndex].Cells["BenchMark"].Value = "No Score";
                            }
                        }

                    }
                    else
                    {
                        rowIndex = gvwAssessts.Rows.Add(caseMstssnoItem.ApplAgency.ToString() + "-" + caseMstssnoItem.ApplDept.ToString() + "-" + caseMstssnoItem.ApplProgram + "   " + caseMstssnoItem.ApplYr + "   " + caseMstssnoItem.ApplNo.ToString(), MATASMTEntityItemList.ScrDesc, string.Empty, string.Empty, LookupDataAccess.Getdate(MATASMTEntityItemList.SSDate));
                    }
                    if (BaseForm.BaseYear.Trim() != string.Empty)
                    {
                        int intYear = Convert.ToInt32(BaseForm.BaseYear) - 1;
                        if (propScaleCode == MATASMTEntityItemList.SclCode && intYear.ToString()== caseMstssnoItem.ApplYr)
                        {
                            gvwAssessts.Rows[rowIndex].Selected = true;
                        }
                    }
                }
            }
            if (gvwAssessts.Rows.Count > 0)
            {
               //** btnSSNSelect.Enabled = true;
                gvwAssessts.Rows[0].Selected= true;
            }
        }

        private void btnSSNSelect_Click(object sender, EventArgs e)
        {

        }
    }
}