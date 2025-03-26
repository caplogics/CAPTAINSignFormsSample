using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.DatabaseLayer;
using DevExpress.CodeParser;
using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.XtraGauges.Core.Styles;
//using Microsoft.ReportingServices.Diagnostics.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using Wisej.Web;
using static DevExpress.Data.Helpers.SyncHelper.ZombieContextsDetector;
using static DevExpress.Utils.Filtering.ExcelFilterOptions;

namespace Captain.Common.Views
{
    public partial class SPTargetHierarchyTarget : Form
    {

        #region private variables
        private List<HierarchyEntity> _selectedHierarchies = null;
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        int selIndex = 0;
        #endregion

        string Code = string.Empty, spCOde = string.Empty, Agy = string.Empty;
        string GroupCode=string.Empty;string TblCode = string.Empty;string FormType = string.Empty;
        public BaseForm BaseForm { get; set; }
        public List<HierarchyEntity> Hie_List { get; set; }
        public string GoalCode { get; set; }

        public SPTargetHierarchyTarget(BaseForm baseForm, List<HierarchyEntity> HieEntityData, string rngCode, string spCode, string rngAgy)
        {
            InitializeComponent();
            Code = rngCode;
            spCOde = spCode;
            Agy= rngAgy;
            BaseForm = baseForm;
            _selectedHierarchies = HieEntityData;
            fillHieGrid();
        }

        public SPTargetHierarchyTarget(BaseForm baseForm, List<HierarchyEntity> HieEntityData, string rngCode, string spCode, string rngAgy,string strgoalcode)
        {
            InitializeComponent();
            _model = new CaptainModel();
            Code = rngCode;
            spCOde = spCode;
            Agy = rngAgy;
            GoalCode = strgoalcode;
            BaseForm = baseForm;
            _selectedHierarchies = HieEntityData;
            fillHieGridwithService();
        }

        public SPTargetHierarchyTarget(BaseForm baseForm, List<HierarchyEntity> HieEntityData, string rngCode, string GrpCd,string TblCd, string rngAgy, string strgoalcode,string type)
        {
            InitializeComponent();
            _model = new CaptainModel();
            Code = rngCode;
            GroupCode = GrpCd;
            TblCode = TblCd; FormType = type;
            Agy = rngAgy;
            GoalCode = strgoalcode;
            BaseForm = baseForm;
            _selectedHierarchies = HieEntityData;
            FillProgamGrid();
        }

        private void fillHieGrid()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int rowIndex = 0; string tempProg = string.Empty;
            int HieIndex = 0;

            ds = SPAdminDB.CAPS_ADMN0030_BROWSE(Code, spCOde, Agy);

           /* List<TargetEntity> RNGList = _model.SPAdminData.CAPS_ADMN0030_BROWSE(Code, spCOde, Agy);

            if (RNGList.Count > 0)
            {
                RNGList = (List<TargetEntity>)RNGList.Select(u => u.SP_Agy + u.SP_Dept + u.SP_Prog + u.PROG_Name).Distinct();

                foreach (TargetEntity Entity in RNGList)
                {
                    if (tempProg != (Entity.SP_Agy + "-" + Entity.SP_Dept + "-" + Entity.SP_Prog).Trim())
                    {
                        rowIndex = dgvHie.Rows.Add(true, Entity.SP_Agy + Entity.SP_Dept + Entity.SP_Prog, Entity.PROG_Name);

                        tempProg = (Entity.SP_Agy + "-" + Entity.SP_Dept + "-" + Entity.SP_Prog).Trim();
                    }
                }
            }*/

            dt = ds.Tables[0];


            if (dt.Rows.Count > 0)
            {
                DataView dv = new DataView(dt);
                dt = dv.ToTable(true, "SP1_AGENCY", "SP1_DEPT", "SP1_PROGRAM", "PROG_NAME");
                
                foreach(DataRow dr in dt.Rows)
                {
                    bool isHie = false;
                    if (tempProg != (dr["SP1_AGENCY"] + "-" + dr["SP1_DEPT"] + "-" + dr["SP1_PROGRAM"]).ToString())
                    {
                        if (_selectedHierarchies.Count > 0)
                        {
                            foreach(HierarchyEntity entity in _selectedHierarchies) 
                            {
                                if(entity.Code== dr["SP1_AGENCY"] + "-" + dr["SP1_DEPT"] + "-" + dr["SP1_PROGRAM"])
                                {
                                    isHie = true;
                                    break;
                                }
                            }
                        }

                                                   
                            rowIndex = dgvHie.Rows.Add(isHie, dr["SP1_AGENCY"] + "-" + dr["SP1_DEPT"] + "-" + dr["SP1_PROGRAM"], dr["PROG_NAME"]);
                       
                        tempProg = (dr["SP1_AGENCY"] + "-" + dr["SP1_DEPT"] + "-" + dr["SP1_PROGRAM"]).ToString();

                        //HieIndex = rowIndex;

                        rowIndex++;
                    }
                }
            }
        }

        private void fillHieGridwithService()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int rowIndex = 0; string tempProg = string.Empty;
            int HieIndex = 0;

            //ds = SPAdminDB.CAPS_ADMN0030_BROWSE(Code, spCOde, Agy);

            List<TargetEntity> RNGList = _model.SPAdminData.CAPS_ADMN0030_BROWSE(Code, spCOde, Agy);
            if(RNGList.Count>0)
            {
                RNGList = RNGList.FindAll(u => u.SP_Agy == BaseForm.BaseAdminAgency);

                //List<TargetEntity> TargetList=RNGList.Select(u => u.GOAL_Code.Trim()== GoalCode.Trim() && u.SP_Agy + u.SP_Dept + u.SP_Prog + u.PROG_Name).Distinct();
                List<TargetEntity> distinctProductList = RNGList.GroupBy(d => new { d.GOAL_Code, d.SP_Agy, d.SP_Dept,d.SP_Prog,d.PROG_Name }).Select(group => group.First()).ToList();

                if (distinctProductList.Count > 0)
                    distinctProductList = distinctProductList.FindAll(u => u.GOAL_Code == GoalCode);

                foreach (TargetEntity TEntity in distinctProductList)
                {
                    bool isHie = false;
                    if (_selectedHierarchies.Count > 0)
                    {
                        foreach (HierarchyEntity entity in _selectedHierarchies)
                        {
                            if (entity.Code == TEntity.SP_Agy + "-" + TEntity.SP_Dept + "-" + TEntity.SP_Prog && entity.PIPActive.Trim()==GoalCode.Trim())
                            {
                                isHie = true;
                                break;
                            }
                        }
                    }

                    if(RNGList.Count>0)
                    {
                        TargetEntity SelTarget=RNGList.Find(u=>u.GOAL_Code.Trim()==GoalCode.Trim() && u.SP_Agy.Trim()==TEntity.SP_Agy.Trim() && u.SP_Dept.Trim() == TEntity.SP_Dept.Trim() && u.SP_Prog.Trim() == TEntity.SP_Prog.Trim());
                        if(SelTarget!=null)
                        {
                            if (!string.IsNullOrEmpty(SelTarget.Target.Trim()))
                                isHie = true;
                        }
                    }

                    rowIndex = dgvHie.Rows.Add(isHie, TEntity.SP_Agy + "-" + TEntity.SP_Dept + "-" + TEntity.SP_Prog, TEntity.PROG_Name.Trim());
                    rowIndex++;
                }

            }

            ///* List<TargetEntity> RNGList = _model.SPAdminData.CAPS_ADMN0030_BROWSE(Code, spCOde, Agy);

            // if (RNGList.Count > 0)
            // {
            //     RNGList = (List<TargetEntity>)RNGList.Select(u => u.SP_Agy + u.SP_Dept + u.SP_Prog + u.PROG_Name).Distinct();

            //     foreach (TargetEntity Entity in RNGList)
            //     {
            //         if (tempProg != (Entity.SP_Agy + "-" + Entity.SP_Dept + "-" + Entity.SP_Prog).Trim())
            //         {
            //             rowIndex = dgvHie.Rows.Add(true, Entity.SP_Agy + Entity.SP_Dept + Entity.SP_Prog, Entity.PROG_Name);

            //             tempProg = (Entity.SP_Agy + "-" + Entity.SP_Dept + "-" + Entity.SP_Prog).Trim();
            //         }
            //     }
            // }*/

            //dt = ds.Tables[0];


            //if (dt.Rows.Count > 0)
            //{
            //    DataView dv = new DataView(dt);
            //    dt = dv.ToTable(true, "SP1_AGENCY", "SP1_DEPT", "SP1_PROGRAM", "PROG_NAME");

            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        bool isHie = false;
            //        if (tempProg != (dr["SP1_AGENCY"] + "-" + dr["SP1_DEPT"] + "-" + dr["SP1_PROGRAM"]).ToString())
            //        {
            //            if (_selectedHierarchies.Count > 0)
            //            {
            //                foreach (HierarchyEntity entity in _selectedHierarchies)
            //                {
            //                    if (entity.Code == dr["SP1_AGENCY"] + "-" + dr["SP1_DEPT"] + "-" + dr["SP1_PROGRAM"])
            //                    {
            //                        isHie = true;
            //                        break;
            //                    }
            //                }
            //            }




            //                rowIndex = dgvHie.Rows.Add(isHie, dr["SP1_AGENCY"] + "-" + dr["SP1_DEPT"] + "-" + dr["SP1_PROGRAM"], dr["PROG_NAME"]);

            //            tempProg = (dr["SP1_AGENCY"] + "-" + dr["SP1_DEPT"] + "-" + dr["SP1_PROGRAM"]).ToString();

            //            //HieIndex = rowIndex;

            //            rowIndex++;
            //        }
            //    }
            //}
        }


        private void FillProgamGrid()
        {
            if (FormType == "RPerfMeasures")
            {
                //SelGoalsHies = GoalHieEntity.FindAll(u => u.RNGGAH_GOAL_CODE.Trim().Equals(gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString().Trim()));

                string Agy = string.Empty; string Dept = string.Empty;
                dgvHie.Rows.Clear();

                Agy = BaseForm.BaseAdminAgency == "**" ? "" : BaseForm.BaseAdminAgency;

                List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(BaseForm.UserID, "I", "I");
                if (caseHierarchy.Count > 0)
                    caseHierarchy = caseHierarchy.FindAll(u => !u.Prog.Trim().Equals(""));

                List<RCsb14GroupEntity> grpCntrls = _model.SPAdminData.Browse_RNGGrp(Code, Agy, GroupCode, null, null, BaseForm.UserID, BaseForm.BaseAdminAgency);
                if (grpCntrls.Count > 0)
                {
                    if (!string.IsNullOrEmpty(grpCntrls[0].DomainHie.Trim()))
                    {

                        Agy = grpCntrls[0].DomainHie.Substring(0, 2) == "**" ? "" : grpCntrls[0].DomainHie.Substring(0, 2);
                        Dept = grpCntrls[0].DomainHie.Substring(3, 2) == "**" ? "" : grpCntrls[0].DomainHie.Substring(3, 2);

                        if (!string.IsNullOrEmpty(Dept.Trim()))
                            caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy) && u.Dept == Dept);
                        else
                            caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(grpCntrls[0].Agency.Trim()))
                        {
                            Agy = grpCntrls[0].Agency.ToString() == "**" ? "" : grpCntrls[0].Agency.ToString();

                            if (!string.IsNullOrEmpty(Agy.Trim()))
                                caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy));
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Agy.Trim()))
                        caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy));
                }

                if (caseHierarchy.Count > 0)
                {

                    foreach (HierarchyEntity entity in caseHierarchy)
                    {
                        bool isHie = false;
                        if (_selectedHierarchies.Count > 0)
                        {
                            foreach (HierarchyEntity Hentity in _selectedHierarchies)
                            {
                                if (entity.Code == entity.Agency + "-" + entity.Dept + "-" + entity.Prog && entity.PIPActive == GoalCode.Trim())
                                {
                                    isHie = true;
                                    break;
                                }
                            }
                        }
                        int rowIndex = dgvHie.Rows.Add(isHie, entity.Agency + "-" + entity.Dept + "-" + entity.Prog, entity.HirarchyName.Trim());

                        //string Target = string.Empty;
                        //if (_selectedHierarchies.Count > 0)
                        //{
                        //    List<RNGGoalHEntity> SelProgs = new List<RNGGoalHEntity>();
                        //    SelProgs = SelGoalsHies.FindAll(u => u.RNGGAH_HIE.Equals(entity.Agency + entity.Dept + entity.Prog));
                        //    if (SelProgs.Count > 0) Target = SelProgs[0].RNGGAH_TARGET.Trim();
                        //}
                        //int rowIndex = gvwPrograms.Rows.Add(entity.Code + "     " + entity.HirarchyName.Trim(), Target, txtCode.Text, entity.Agency + entity.Dept + entity.Prog);
                    }

                    //if(gvwPrograms.Rows.Count>0)

                }

            }
            else if (FormType == "RServices")
            {
                //SelSRGoalsHies = SRGoalHieEntity.FindAll(u => u.RNGSRGAH.GOAL_CODE.Trim().Equals(gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString().Trim()));

                string Agy = string.Empty; string Dept = string.Empty;
                dgvHie.Rows.Clear();

                Agy = BaseForm.BaseAdminAgency == "**" ? "" : BaseForm.BaseAdminAgency;

                List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(BaseForm.UserID, "I", "I");
                if (caseHierarchy.Count > 0)
                    caseHierarchy = caseHierarchy.FindAll(u => !u.Prog.Trim().Equals(""));

                List<SRCsb14GroupEntity> grpCntrls = _model.SPAdminData.Browse_RNGSRGrp(Code, Agy, GroupCode, null, null, BaseForm.UserID, BaseForm.BaseAdminAgency);
                if (grpCntrls.Count > 0)
                {
                    if (!string.IsNullOrEmpty(grpCntrls[0].DomainHie.Trim()))
                    {

                        Agy = grpCntrls[0].DomainHie.Substring(0, 2) == "**" ? "" : grpCntrls[0].DomainHie.Substring(0, 2);
                        Dept = grpCntrls[0].DomainHie.Substring(3, 2) == "**" ? "" : grpCntrls[0].DomainHie.Substring(3, 2);

                        if (!string.IsNullOrEmpty(Dept.Trim()))
                            caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy) && u.Dept == Dept);
                        else
                            caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(grpCntrls[0].Agency.Trim()))
                        {
                            Agy = grpCntrls[0].Agency.ToString() == "**" ? "" : grpCntrls[0].Agency.ToString();

                            if (!string.IsNullOrEmpty(Agy.Trim()))
                                caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy));
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Agy.Trim()))
                        caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy));
                }

                if (caseHierarchy.Count > 0)
                {

                    foreach (HierarchyEntity entity in caseHierarchy)
                    {
                        bool isHie = false;
                        if (_selectedHierarchies.Count > 0)
                        {
                            foreach (HierarchyEntity Hentity in _selectedHierarchies)
                            {
                                if (entity.Code == entity.Agency + "-" + entity.Dept + "-" + entity.Prog && entity.PIPActive == GoalCode.Trim())
                                {
                                    isHie = true;
                                    break;
                                }
                            }
                        }
                        int rowIndex = dgvHie.Rows.Add(isHie, entity.Agency + "-" + entity.Dept + "-" + entity.Prog, entity.HirarchyName.Trim());

                        //string Target = string.Empty;
                        //if (SelSRGoalsHies.Count > 0)
                        //{
                        //    List<RNGSRGoalHEntity> SelProgs = new List<RNGSRGoalHEntity>();
                        //    SelProgs = SelSRGoalsHies.FindAll(u => u.RNGSRGAH_HIE.Equals(entity.Agency + entity.Dept + entity.Prog));
                        //    if (SelProgs.Count > 0) Target = SelProgs[0].RNGSRGAH_TARGET.Trim();
                        //}
                        //int rowIndex = gvwPrograms.Rows.Add(entity.Code + "     " + entity.HirarchyName.Trim(), Target, txtCode.Text, entity.Agency + entity.Dept + entity.Prog);
                    }

                    //if(gvwPrograms.Rows.Count>0)

                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public List<HierarchyEntity> SelectedHies()
        {
            List<HierarchyEntity> hierarchyEntities= new List<HierarchyEntity>();
            
            if(dgvHie.Rows.Count > 0) 
            {
                List<TargetEntity> RNGList = _model.SPAdminData.CAPS_ADMN0030_BROWSE(Code, spCOde, Agy);
                if (RNGList.Count > 0)
                    RNGList = RNGList.FindAll(u => u.GOAL_Code.Trim() == GoalCode);

                foreach (DataGridViewRow dr in dgvHie.Rows) 
                {
                    if (dr.Cells["Hie_Select"].Value.ToString()=="True")
                    {
                        HierarchyEntity entity=new HierarchyEntity();
                        entity.Agency = dr.Cells["Hie_Code"].Value.ToString().Substring(0,2);
                        entity.Dept = dr.Cells["Hie_Code"].Value.ToString().Substring(3, 2);
                        entity.Prog = dr.Cells["Hie_Code"].Value.ToString().Substring(6, 2);
                        entity.Code = dr.Cells["Hie_Code"].Value.ToString();
                        entity.HirarchyName = dr.Cells["Hie_Name"].Value.ToString();
                        entity.PIPActive = GoalCode.Trim();

                        hierarchyEntities.Add(entity);
                    }
                    else
                    {
                        if(RNGList.Count>0)
                        {
                            TargetEntity entity = RNGList.Find(u => u.SP_Agy == dr.Cells["Hie_Code"].Value.ToString().Substring(0, 2) && u.SP_Dept == dr.Cells["Hie_Code"].Value.ToString().Substring(3, 2) && u.SP_Prog == dr.Cells["Hie_Code"].Value.ToString().Substring(6, 2));
                            if(entity!=null)
                            {
                                if (entity.CAMS_Type == "CA")
                                {
                                    RNGSRGoalHEntity ServEntity = new RNGSRGoalHEntity();
                                    ServEntity.RNGSRGAH_CODE = entity.RNG_Code;
                                    ServEntity.RNGSRGAH_GRP_CODE = entity.GRP_Code;
                                    ServEntity.RNGSRGAH_TBL_CODE = entity.TBL_Code;
                                    ServEntity.RNGSRGAH_AGENCY = entity.SP_Agy;
                                    ServEntity.RNGSRGAH_HIE = dr.Cells["Hie_Code"].Value.ToString().Substring(0, 2)+ dr.Cells["Hie_Code"].Value.ToString().Substring(3, 2)+ dr.Cells["Hie_Code"].Value.ToString().Substring(6, 2);
                                    ServEntity.RNGSRGAH_GOAL_CODE = GoalCode;
                                    //ServEntity.RNGSRGAH_TARGET = sptTar;
                                    ServEntity.RNGSRGAH_LSTC_OPERATOR = BaseForm.UserID;


                                    ServEntity.Mode = "DelHie";

                                    _model.SPAdminData.InsertUpdateRNGSRGAHIE(ServEntity);
                                }
                                else if (entity.CAMS_Type == "MS")
                                {
                                    RNGGoalHEntity OutcEntity = new RNGGoalHEntity();
                                    OutcEntity.RNGGAH_CODE = entity.RNG_Code;
                                    OutcEntity.RNGGAH_GRP_CODE = entity.GRP_Code;
                                    OutcEntity.RNGGAH_TBL_CODE = entity.TBL_Code;
                                    OutcEntity.RNGGAH_AGENCY = entity.SP_Agy;
                                    OutcEntity.RNGGAH_HIE = dr.Cells["Hie_Code"].Value.ToString().Substring(0, 2) + dr.Cells["Hie_Code"].Value.ToString().Substring(3, 2) + dr.Cells["Hie_Code"].Value.ToString().Substring(6, 2); 
                                    OutcEntity.RNGGAH_GOAL_CODE = GoalCode;
                                    //OutcEntity.RNGGAH_TARGET = sptTar;
                                    OutcEntity.RNGGAH_LSTC_OPERATOR = BaseForm.UserID;
                                    OutcEntity.Mode = "DelHie";

                                    _model.SPAdminData.InsertUpdateRNGGAHIE(OutcEntity);
                                }
                            }
                        }
                    }
                }
            }
            return hierarchyEntities;
        }
    }
}