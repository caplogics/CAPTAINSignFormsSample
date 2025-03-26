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
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.UserControls;
using Captain.DatabaseLayer;
using System.IO;
using System.Linq;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
using Microsoft.IdentityModel.Tokens;
using DevExpress.Utils.Extensions;
using DevExpress.Web.Internal.XmlProcessor;
using DevExpress.DataAccess.Sql;
#endregion


namespace Captain.Common.Views.Forms
{
    public partial class CASE0016_Usage : Form
    {
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        string strCaseWorkerDefaultCode = string.Empty;
        string strCaseWorkerDefaultStartCode = string.Empty;

        public CASE0016_Usage(BaseForm baseForm, string mode, string sp_code, string sp_sequence, string spm_year, string SPName, string strPrimSource, string strType, string strsource, string strelec, CASESPMEntity spmentity, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            Mode = mode;
            SP_Code = sp_code;
            Sp_Sequence = sp_sequence;
            Spm_Year = spm_year;
            Source = strsource; StrType = strType; StrElec = strelec; Prim_Source = strPrimSource;
            propSPM_Entity = spmentity;

            this.Text = "Post Invoice from Usage/Consumption";

            // this.Text = privilegeEntity.Program;// + " - " + Mode;

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            propReportPath = _model.lookupDataAccess.GetReportPath();

            txtAmount.Validator = TextBoxValidation.FloatValidator;
            txtBalance.Validator = TextBoxValidation.FloatValidator;

            PostUsagePriv = new PrivilegeEntity();
            List<PrivilegeEntity> userPrivilege = _model.UserProfileAccess.GetScreensByUserID(Privileges.ModuleCode.Trim(), BaseForm.UserID, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg);
            if (userPrivilege.Count > 0)
            {
                PostUsagePriv = userPrivilege.Find(u => u.Program == "CASE1016");
                if (PostUsagePriv != null)
                {
                    //if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                    //{ btnInvoices.Enabled = true; }
                    //else btnInvoices.Visible = false;
                    //if (PostUsagePriv.ChangePriv.ToUpper() == "TRUE")
                    //{ this.gvwMonthQuestions.Columns["gvtOverride"].ReadOnly=false; }
                    //else this.gvwMonthQuestions.Columns["gvtOverride"].ReadOnly = true;
                    //if (PostUsagePriv.DelPriv.ToUpper() == "TRUE")
                    //{ this.gvwMonthQuestions.Columns["gvDelete"].ReadOnly = false; }
                    //else this.gvwMonthQuestions.Columns["gvDelete"].ReadOnly = true;
                }
            }

            if (!string.IsNullOrEmpty(BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()))
            {
                if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim() == "Y")
                    ACR_SERV_Hies = "S";
            }
            propSearch_Entity = _model.SPAdminData.Browse_CASESP0List(null, null, null, null, null, null, null, null, null);
            propfundingsource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "AGYTABS");
            Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, string.Empty);
            ALLEmsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, string.Empty);
            if (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
                CEAPCNTL_List = _model.SPAdminData.GetCEAPCNTLData(string.Empty, BaseForm.BaseYear, string.Empty, string.Empty);

            SP_Header_Rec = _model.SPAdminData.Browse_CASESP0(SP_Code, null, null, null, null, null, null, null, null);

            Get_Vendor_List();

            Vulner_Flag = Get_SNP_Vulnerable_Status();

            //if (SP_Header_Rec!=null)
            //{
            //    if (!string.IsNullOrEmpty(SP_Header_Rec.Transactions.Trim()))
            //        txtMaxInvs.Text = SP_Header_Rec.Transactions.Trim();
            //}
            txtScore.Text = BaseForm.BaseCaseMstListEntity[0].Rank1;

            if (CEAPCNTL_List.Count > 0)
            {
                if (CEAPCNTL_List[0].CPCT_INV_METHOD == "2")
                {
                    if (Vulner_Flag) { ServiceCode = CEAPCNTL_List[0].CPCT_VUL_PRIM_CA.Trim(); }
                    else if (!Vulner_Flag) { ServiceCode = CEAPCNTL_List[0].CPCT_NONVUL_PRIM_CA.Trim(); }

                    //List<CEAPINVEntity> CEAPINVs= new List<CEAPINVEntity>();
                    List<CEAPINVEntity> CEAPINVs = _model.SPAdminData.GetCEAPINVData(CEAPCNTL_List[0].CPCT_CODE, CEAPCNTL_List[0].CPCT_YEAR, "GET");
                    if (CEAPINVs.Count > 0)
                    {
                        //foreach(CEAPINVEntity Entity in CEAPINVs)
                        //{
                        //    if(decimal.Parse(Entity.CPINV_LOW) >= decimal.Parse(txtScore.Text.Trim()) && decimal.Parse(Entity.CPINV_HIGH) <= decimal.Parse(txtScore.Text.Trim()))
                        //    {
                        //        txtMaxInvs.Text = Entity.CPINV_MAX_INV;
                        //        break;
                        //    }
                        //}

                        CEAPINVEntity CEntity = CEAPINVs.Find(u => int.Parse(u.CPINV_LOW) <= int.Parse(txtScore.Text.Trim()) && int.Parse(u.CPINV_HIGH) >= int.Parse(txtScore.Text.Trim()));
                        if (CEntity != null)
                            txtMaxInvs.Text = CEntity.CPINV_MAX_INV.ToString();
                        else
                            txtMaxInvs.Text = "0";
                    }
                }
                else
                {
                    if (Vulner_Flag) { txtMaxInvs.Text = CEAPCNTL_List[0].CPCT_VUL_MAX_INV; ServiceCode = CEAPCNTL_List[0].CPCT_VUL_PRIM_CA.Trim(); }
                    else if (!Vulner_Flag) { txtMaxInvs.Text = CEAPCNTL_List[0].CPCT_NONVUL_MAX_INV; ServiceCode = CEAPCNTL_List[0].CPCT_NONVUL_PRIM_CA.Trim(); }
                }
            }

            lblSPName.Text = SPName.Trim();


            Fill_Applicant_SPs();

            if (SPM_Entity != null)
            {
                txtAmount.Text = SPM_Entity.SPM_Amount;
                txtBalance.Text = SPM_Entity.SPM_Balance;
                decimal PaidAmt = 0;
                PaidAmt = Convert.ToDecimal(!string.IsNullOrEmpty(SPM_Entity.SPM_Amount.Trim()) ? SPM_Entity.SPM_Amount.Trim() : "0") - Convert.ToDecimal(!string.IsNullOrEmpty(SPM_Entity.SPM_Balance.Trim()) ? SPM_Entity.SPM_Balance.Trim() : "0");
                txtPaid.Text = PaidAmt.ToString();

                if (StrElec == "E")
                {
                    Txt_VendNo.Text = SPM_Entity.SPM_Vendor.Trim();
                    Text_VendName.Text = Get_Vendor_Name(SPM_Entity.SPM_Vendor);
                    txtAccountNo.Text = SPM_Entity.SPM_Account;
                }
                else
                {
                    Txt_VendNo.Text = SPM_Entity.SPM_Gas_Vendor.Trim();
                    Text_VendName.Text = Get_Vendor_Name(SPM_Entity.SPM_Gas_Vendor);
                    txtAccountNo.Text = SPM_Entity.SPM_Gas_Account;
                }


            }

            FillActivityCombo();

            fillMonthlyQuestions();

            //FillCombo();
            //Fill_SP_DropDowns();
            //fillFundCombo();





            if (Mode == "Edit")
            {
                //FillBenefit_Controls();
            }


        }

        CASESPMEntity propSPM_Entity { get; set; }
        List<CASESP0Entity> propSearch_Entity { get; set; }
        List<CommonEntity> propfundingsource { get; set; }
        List<CMBDCEntity> Emsbdc_List { get; set; }
        List<CMBDCEntity> ALLEmsbdc_List { get; set; }
        List<CEAPCNTLEntity> CEAPCNTL_List { get; set; }
        CASESP0Entity SP_Header_Rec;
        string ACR_SERV_Hies = string.Empty;
        #region Properties
        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Mode { get; set; }
        public string Prim_Source { get; set; }
        public string Source { get; set; }
        public string StrType { get; set; }
        public string StrElec { get; set; }

        public bool IsSaveValid { get; set; }
        public string SP_Code { get; set; }
        public string Sp_Sequence { get; set; }
        public string propReportPath { get; set; }

        public string Spm_Year { get; set; }
        public string ServiceCode { get; set; }

        public CASESPMEntity SPM_Entity { get; set; }

        #endregion
        List<HierarchyEntity> hierarchyEntity = new List<HierarchyEntity>();
        List<CASESP2Entity> SP_CAMS_Details = new List<CASESP2Entity>();
        public PrivilegeEntity PostUsagePriv { get; set; }



        private List<CommonEntity> filterByHIE(List<CommonEntity> LookupValues, string Mode)
        {
            string HIE = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            List<CommonEntity> _AgytabsFilter = new List<CommonEntity>();
            _AgytabsFilter = LookupValues;
            if (LookupValues.Count > 0)
            {

                if (Mode.ToUpper() == "ADD")
                {
                    _AgytabsFilter = _AgytabsFilter.FindAll(u => (u.ListHierarchy.Contains(HIE) || u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.ListHierarchy.Contains(BaseForm.BaseAgency + "****") || u.ListHierarchy.Contains("******")) && u.Active.ToString().ToUpper() == "Y").ToList();
                }
                else
                {
                    _AgytabsFilter = _AgytabsFilter.FindAll(u => u.ListHierarchy.Contains(HIE) || u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.ListHierarchy.Contains(BaseForm.BaseAgency + "****") || u.ListHierarchy.Contains("******")).ToList();
                }

                _AgytabsFilter = _AgytabsFilter.OrderByDescending(u => u.Active).ThenBy(u => u.Desc).ToList();
            }

            return _AgytabsFilter;
        }

        private void Fill_SP_DropDowns()
        {
            this.CmbSP.SelectedIndexChanged -= new System.EventHandler(this.CmbSP_SelectedIndexChanged);
            CmbSP.Items.Clear();
            List<CASESP1Entity> SP_Hierarchies = new List<CASESP1Entity>();
            ACR_SERV_Hies = string.Empty;
            if (!string.IsNullOrEmpty(BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()))
            {
                if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim() == "Y")
                    ACR_SERV_Hies = "S";
            }

            if (ACR_SERV_Hies == "Y" || ACR_SERV_Hies == "S")
            {
                if (BaseForm.BaseAgencyControlDetails.SerPlanAllow.Trim() == "D")
                    SP_Hierarchies = _model.SPAdminData.CASESP1_SerPlans(null, BaseForm.BaseAgency, BaseForm.BaseDept, null, BaseForm.UserID);
                else
                    SP_Hierarchies = _model.SPAdminData.CASESP1_SerPlans(null, BaseForm.BaseAgency, null, null, BaseForm.UserID);
            }
            else
                SP_Hierarchies = _model.SPAdminData.Browse_CASESP1(null, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);

            CmbSP.ColorMember = "FavoriteColor";

            //string strcmbFundsource = ((ListItem)cmbFundsource.SelectedItem).Value == null ? string.Empty : ((ListItem)cmbFundsource.SelectedItem).Value.ToString();
            if (SP_Hierarchies.Count > 0)
            {
                bool SP_Exists = false, Allow_Dups = false;
                string Tmp_SP_Desc = null, Tmp_SP_Code = null, SP_Valid = null, SPM_Start_Date = " ", SP_DESC = " ", spm_posting_year = "";
                int Tmp_Sel_Index = 0, Itr_Index = 0;

                if (Mode.Equals("Edit"))
                {
                    foreach (CASESPMEntity Entity1 in CASESPM_SP_List)
                    {
                        SP_DESC = " "; SP_Exists = false;

                        CASESP1Entity casesp1data = SP_Hierarchies.Find(u => u.Code == Entity1.service_plan);
                        if (casesp1data != null)
                        {
                            SP_Exists = true;
                            if (Entity1.Sp0_Validatetd.ToUpper() == "Y" && Entity1.Sp0_Active.ToUpper() == "Y")
                            {
                                SP_Valid = "Y";
                            }
                            else
                            {
                                SP_Valid = "N";
                            }
                            SPM_Start_Date = Entity1.startdate;
                            SP_DESC = casesp1data.SP_Desc;

                            if (casesp1data.SP_Allow_Dups == "Y")
                                Allow_Dups = true;
                        }

                        if ((Mode.Equals("Edit") && SP_Exists))
                        {
                            Tmp_SP_Code = "000000".Substring(0, (6 - Entity1.service_plan.Length)) + Entity1.service_plan;

                            if (SP_Code == Tmp_SP_Code && Entity1.Seq == Sp_Sequence && Spm_Year == Entity1.year)
                                Tmp_Sel_Index = Itr_Index;

                            if (!string.IsNullOrEmpty(SPM_Start_Date.Trim()))
                                SPM_Start_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(SPM_Start_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                            else
                                SPM_Start_Date = " ";

                            spm_posting_year = "";
                            spm_posting_year = (string.IsNullOrEmpty(Entity1.year.ToString().Trim()) ? "" : " - PY" + Entity1.year.ToString().Trim());

                            //if(Mode.Equals("Add"))
                            //    CmbSP.Items.Add(new Captain.Common.Utilities.ListItem(Tmp_SP_Code + " - " + Entity.SP_Desc.Trim(), Entity.Code.ToString(), SP_Valid, (SP_Valid.Equals("Y") ? Color.Green : Color.Red)));
                            //else
                            CASESP0Entity casesp0data = propSearch_Entity.Find(u => u.Code == Entity1.service_plan && u.Funds != string.Empty && u.Allow_Add_Branch == "N" && u.B2Code == string.Empty);
                            if (casesp0data != null)
                            {
                                CmbSP.Items.Add(new Captain.Common.Utilities.ListItem(Tmp_SP_Code + " - " + SPM_Start_Date + " - " + SP_DESC.Trim() + spm_posting_year, Entity1.service_plan.ToString(), SP_Valid, (SP_Valid.Equals("Y") ? Color.Green : Color.Red), Entity1.Seq, Entity1.year));
                                Itr_Index++;
                            }

                        }
                    }

                    if (CmbSP.Items.Count > 0)
                    {


                        CmbSP.SelectedIndex = Tmp_Sel_Index;
                        //SP_Programs_List = _model.lookupDataAccess.Get_SerPlan_Prog_List(BaseForm.UserProfile.UserID, ((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).Value.ToString(), ACR_SERV_Hies);
                    }
                }



                //string Fund = ((ListItem)cmbFundsource.SelectedItem).Value.ToString();

                if (Mode.Equals("Add"))
                {
                    foreach (CASESP1Entity Entity in SP_Hierarchies)  // 08122012
                    {
                        SP_Exists = Allow_Dups = false;
                        Tmp_SP_Desc = null;
                        // SP_Valid = Entity.SP_validated;
                        SPM_Start_Date = " ";
                        if (Entity.SP_validated.ToUpper() == "Y" && Entity.Sp0_Active.ToUpper() == "Y")
                        {
                            SP_Valid = "Y";
                        }
                        else
                        {
                            SP_Valid = "N";
                        }
                        foreach (CASESPMEntity Entity1 in CASESPM_SP_List)
                        {
                            if (Entity1.service_plan == Entity.Code)
                            {
                                SP_Exists = true;
                                if (Entity1.Sp0_Validatetd.ToUpper() == "Y" && Entity1.Sp0_Active.ToUpper() == "Y")
                                {
                                    SP_Valid = "Y";
                                }
                                else
                                {
                                    SP_Valid = "N";
                                }
                                SPM_Start_Date = Entity1.startdate;

                                if (Entity.SP_Allow_Dups == "Y")
                                    Allow_Dups = true;

                                break;
                            }
                        }

                        if (SP_Valid.ToUpper() == "Y")
                        {
                            if ((Mode.Equals("Add") && !SP_Exists) || (Mode.Equals("Add") && Allow_Dups))// || (Mode.Equals("Edit") && SP_Exists))
                            {
                                Tmp_SP_Code = "000000".Substring(0, (6 - Entity.Code.Length)) + Entity.Code;

                                if (propSearch_Entity.Count > 0)
                                {
                                    CASESP0Entity casesp0data = propSearch_Entity.Find(u => u.Code == Entity.Code && u.Funds != string.Empty && u.Allow_Add_Branch == "N" && u.B2Code == string.Empty);
                                    if (casesp0data != null)
                                    {
                                        if (casesp0data.Sp0ReadOnly != "Y" && casesp0data.NoSPM == "Y")
                                        {
                                            CmbSP.Items.Add(new Captain.Common.Utilities.ListItem(Tmp_SP_Code + " - " + Entity.SP_Desc.Trim(), Entity.Code.ToString(), SP_Valid, (SP_Valid.Equals("Y") ? Color.Green : Color.Red)));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            this.CmbSP.SelectedIndexChanged += new System.EventHandler(this.CmbSP_SelectedIndexChanged);
            //if (Mode.Equals("Add") && CmbSP.Items.Count == 1)
            if (CmbSP.Items.Count > 0)
                CmbSP.SelectedIndex = 0;

        }

        private void FillActivityCombo()
        {
            CmbSP.Items.Clear();

            SP_CAMS_Details = _model.SPAdminData.Browse_CASESP2(SPM_Entity.service_plan, null, null, null, "CASE4006");

            if (SP_CAMS_Details.Count > 0)
            {
                List<CASESP2Entity> Sel_Services = SP_CAMS_Details.FindAll(u => u.ServPlan == SPM_Entity.service_plan && u.Type1 == "CA");

                if (Sel_Services.Count > 0)
                {
                    bool IsService = true;
                    foreach (CASESP2Entity Entity in Sel_Services)
                    {
                        //List<PMTFLDCNTLHEntity> propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", "04", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, Entity.ServPlan, Entity.Branch, Entity.Curr_Grp.ToString(), Entity.CamCd.Trim(), "SP");
                        // propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == "04");

                        // if (propPMTFLDCNTLHEntity.Count == 0)
                        // {
                        //     propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", "04", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, "0", " ", "0", "          ", "hie");
                        //     propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == "04");
                        // }

                        // if(propPMTFLDCNTLHEntity.Count>0)
                        // {
                        //     PMTFLDCNTLHEntity PMFLDCnt = propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == "S00205"); 
                        //     if(PMFLDCnt!=null)
                        //     {
                        //         if (PMFLDCnt.PMFLDH_ENABLED == "Y") IsService = true; else IsService = false;
                        //     }
                        // }

                        if (CEAPCNTL_List[0].CPCT_VUL_SP.Trim() == SPM_Entity.service_plan)
                        {
                            if (CEAPCNTL_List[0].CPCT_VUL_PRIM_CA.Trim() == Entity.CamCd.Trim()) IsService = true; else IsService = false;
                        }
                        else if (CEAPCNTL_List[0].CPCT_NONVUL_SP.Trim() == SPM_Entity.service_plan)
                        {
                            if (CEAPCNTL_List[0].CPCT_NONVUL_PRIM_CA.Trim() == Entity.CamCd.Trim()) IsService = true; else IsService = false;
                        }

                        if (IsService)
                            CmbSP.Items.Add(new Captain.Common.Utilities.ListItem(Entity.CAMS_Desc.Trim(), Entity.CamCd.Trim(), Entity.Branch, Entity.Curr_Grp.ToString(), string.Empty, string.Empty, (Entity.CAMS_Active).Equals("Y") ? Color.Green : Color.Red));
                    }
                }
            }

            if (CmbSP.Items.Count > 0)
            {
                CmbSP.SelectedIndex = 0;
            }
        }


        List<CASESPMEntity> CASESPM_SP_List;
        CASESPMEntity Search_Entity = new CASESPMEntity();
        private void Fill_Applicant_SPs()
        {
            Search_Entity.agency = BaseForm.BaseAgency;
            Search_Entity.dept = BaseForm.BaseDept;
            Search_Entity.program = BaseForm.BaseProg;
            //Search_Entity.year = BaseYear;
            Search_Entity.year = null;                          // Year will be always Four-Spaces in CASESPM
            Search_Entity.app_no = BaseForm.BaseApplicationNo;

            Search_Entity.service_plan = SP_Code; Search_Entity.Seq = Sp_Sequence;
            Search_Entity.caseworker = Search_Entity.site = null;
            Search_Entity.startdate = Search_Entity.estdate = Search_Entity.compdate = null;
            Search_Entity.sel_branches = Search_Entity.have_addlbr = Search_Entity.date_lstc = null;
            Search_Entity.lstc_operator = Search_Entity.date_add = Search_Entity.add_operator = null;
            Search_Entity.Sp0_Desc = Search_Entity.Sp0_Validatetd = Search_Entity.Def_Program = //Search_Entity.SPM_MassClose =
            Search_Entity.SPM_MassClose = Search_Entity.Seq = Search_Entity.Bulk_Post = null;

            CASESPM_SP_List = _model.SPAdminData.Browse_CASESPM(Search_Entity, "Browse");

            if (CASESPM_SP_List.Count > 0)
            {
                SPM_Entity = CASESPM_SP_List[0];

                Get_App_CASEACT_List(SPM_Entity);

                txtAmount.Text = SPM_Entity.SPM_Amount;
                txtBalance.Text = SPM_Entity.SPM_Balance;
                decimal PaidAmt = 0;



                PaidAmt = Convert.ToDecimal(!string.IsNullOrEmpty(SPM_Entity.SPM_Amount.Trim()) ? SPM_Entity.SPM_Amount.Trim() : "0") - Convert.ToDecimal(!string.IsNullOrEmpty(SPM_Entity.SPM_Balance.Trim()) ? SPM_Entity.SPM_Balance.Trim() : "0");
                txtPaid.Text = PaidAmt.ToString();

            }

        }

        List<CASEACTEntity> PropCaseactList = new List<CASEACTEntity>();
        List<CASEACTEntity> SP_Activity_Details = new List<CASEACTEntity>();
        private void Get_App_CASEACT_List(CASESPMEntity SPM_Entity)
        {

            CASEACTEntity Search_Enty = new CASEACTEntity(true);
            Search_Enty.Agency = SPM_Entity.agency;
            Search_Enty.Dept = SPM_Entity.dept;
            Search_Enty.Program = SPM_Entity.program;
            Search_Enty.Year = SPM_Entity.year;
            Search_Enty.App_no = SPM_Entity.app_no;
            Search_Enty.SPM_Seq = SPM_Entity.Seq;
            Search_Enty.Service_plan = SPM_Entity.service_plan;


            SP_Activity_Details = _model.SPAdminData.Browse_CASEACT(Search_Enty, "Browse", "PAYMENT");

            if (SP_Activity_Details.Count > 0)
                SP_Activity_Details = SP_Activity_Details.FindAll(u => u.ActSeek_Date.Trim() != string.Empty);

            SP_Activity_Details = SP_Activity_Details.OrderByDescending(u => Convert.ToDateTime(u.ACT_Date.Trim())).ToList();

            Search_Entity.app_no = string.Empty;
            PropCaseactList = _model.SPAdminData.Browse_CASEACT(Search_Enty, "Browse", "PAYMENT");
            if (PropCaseactList.Count > 0) PropCaseactList = PropCaseactList.FindAll(u => u.Elec_Other == "E" || u.Elec_Other == "O");

            if (PropCaseactList.Count > 0)
            {
                txtRemTrans.Text = PropCaseactList.Count.ToString(); txtPaidInv.Text = PropCaseactList.Count.ToString();
                txtInvBal.Text = (Convert.ToDecimal(txtMaxInvs.Text.Trim() == "" ? "0" : txtMaxInvs.Text.Trim()) - Convert.ToDecimal(txtPaidInv.Text.Trim())).ToString();
            }
            else
            {
                txtPaidInv.Text = "0"; txtInvBal.Text = txtMaxInvs.Text.Trim();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void CmbSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim()))
            {
                //fillFundCombo();
                fillMonthlyQuestions();
            }
        }

        List<CASEVDDEntity> CaseVddlist = new List<CASEVDDEntity>();
        private void Get_Vendor_List()
        {
            CASEVDDEntity Search_Entity = new CASEVDDEntity(true);
            CaseVddlist = _model.SPAdminData.Browse_CASEVDD(Search_Entity, "Browse");

            if (BaseForm.BaseAgencyControlDetails.AgyVendor == "Y")
                CaseVddlist = CaseVddlist.FindAll(u => u.VDD_Agency == BaseForm.BaseAgency);
        }

        private string Get_Vendor_Name(string VendorNo)
        {
            string Vend_Name = string.Empty;
            foreach (CASEVDDEntity Entity in CaseVddlist)
            {
                if (Entity.Code == VendorNo)
                {
                    Vend_Name = Entity.Name.Trim(); break;
                }
            }

            return Vend_Name;
        }

        //Gizmox.WebGUI.Common.Resources.IconResourceHandle Override_image = new Gizmox.WebGUI.Common.Resources.IconResourceHandle("24X24.icons-arrow-24.png");
        //Gizmox.WebGUI.Common.Resources.ResourceHandle BlankImg = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("Blank.jpg");
        //Gizmox.WebGUI.Common.Resources.ResourceHandle DeleteImg = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("DeleteItem.gif");
        //Gizmox.WebGUI.Common.Resources.ResourceHandle Img_Edit = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("EditIcon.gif");

        string Override_image = Consts.Icons.ico_Left_up_Corner_Arrow; // new Gizmox.WebGUI.Common.Resources.IconResourceHandle("24X24.icons-arrow-24.png");
        string BlankImg = Consts.Icons.ico_Blank;
        string DeleteImg = Consts.Icons.ico_Delete;
        string Img_Edit = Consts.Icons.ico_Edit;



        List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
        private void fillMonthlyQuestions()
        {
            gvwMonthQuestions.Rows.Clear();
            List<CustomQuestionsEntity> custmonthQuestions = new List<CustomQuestionsEntity>();

            custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);

            string custCode = string.Empty;
            //List<CustomQuestionsEntity> response = custResponses.FindAll(u => u.ACTCODE.Equals(custCode)).ToList();
            RecCount = 0;
            int rowIndex;
            CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
            //if (responsetot != null)
            //{
            //    chkUtility.CheckedChanged -= new EventHandler(chkUtility_CheckedChanged);
            //    chkUtility.Checked = responsetot.USAGE_PRIM_12HIST.ToString() == "Y" ? true : false;
            //    chkGas.Checked = responsetot.USAGE_SEC_12HIST.ToString() == "Y" ? true : false;
            //    chkUtility.CheckedChanged += new EventHandler(chkUtility_CheckedChanged);
            //}

            List<CustomQuestionsEntity> CustRespAll = new List<CustomQuestionsEntity>();

            CustRespAll = custResponses.OrderBy(u => u.USAGE_PRIM_PAYMENT.Trim()).ToList();
            foreach (CustomQuestionsEntity Entity in CustRespAll)
            {
                if (string.IsNullOrEmpty(Entity.USAGE_PRIM_PAYMENT.Trim())) Entity.USAGE_PRIM_PAYMENT = "0.00";
                if (string.IsNullOrEmpty(Entity.USAGE_SEC_PAYMENT.Trim())) Entity.USAGE_SEC_PAYMENT = "0.00";
            }

            if (StrElec == "E")
                CustRespAll = custResponses.OrderByDescending(u => Convert.ToDecimal(u.USAGE_PRIM_PAYMENT.Trim())).ToList();
            else
                CustRespAll = custResponses.OrderByDescending(u => Convert.ToDecimal(u.USAGE_SEC_PAYMENT.Trim())).ToList();

            if (CustRespAll.Count > 0)
            {
                foreach (CustomQuestionsEntity Entity in CustRespAll)
                {
                    string MonthName = string.Empty;
                    MonthName = GetMonthName(Entity.USAGE_MONTH.Trim());
                    if (CmbSP.Items.Count > 0)
                        InvoiceDetails(Entity.USAGE_MONTH.Trim());

                    string UsageAmount = string.Empty;

                    if (Entity.USAGE_MONTH != "TOT")
                    {
                        //if (StrType == "01" || StrType == "02") UsageAmount = Entity.SER_ELEC.Trim();
                        //else

                        if (StrElec == "E")
                            UsageAmount = Entity.USAGE_PRIM_PAYMENT.Trim();
                        else UsageAmount = Entity.USAGE_SEC_PAYMENT.Trim();

                        if (UsageAmount == "0.00")
                        {
                            CustomQuestionsEntity REnt = custResponses.Find(u => u.USAGE_MONTH == Entity.USAGE_MONTH);
                            if (REnt != null)
                            {
                                if (StrElec == "E")
                                    UsageAmount = REnt.USAGE_PRIM_PAYMENT.Trim();
                                else UsageAmount = REnt.USAGE_SEC_PAYMENT.Trim();
                            }
                        }

                        string Iscaseact = "N"; string BundleNo = string.Empty; string PDOut = "N"; string VendName = string.Empty;
                        if (CaseactRec != null) { if (!string.IsNullOrEmpty(CaseactRec.App_no.Trim())) { Iscaseact = "Y"; BundleNo = CaseactRec.BundleNo.Trim(); PDOut = CaseactRec.PDOUT.Trim(); VendName = Get_Vendor_Name(CaseactRec.Vendor_No.Trim()); } }

                        rowIndex = gvwMonthQuestions.Rows.Add(Entity.USAGE_MONTH, MonthName, SeekDate.Trim() == "" ? false : true, UsageAmount, LookupDataAccess.Getdate(SeekDate.Trim()), LookupDataAccess.Getdate(ServsDate.Trim()), BlankImg, ResAmount == 0 ? "" : ResAmount.ToString(), BundleNo, ChkNo, LookupDataAccess.Getdate(ChkDate.Trim()), VendName, BlankImg, Iscaseact, PDOut);

                        if (!string.IsNullOrEmpty(ServsDate.Trim()))
                        {
                            if (PostUsagePriv != null)
                            {
                                if (PostUsagePriv.ChangePriv.ToUpper() == "TRUE")
                                {
                                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtOverride"].ReadOnly = false;
                                    if (!string.IsNullOrEmpty(BundleNo.Trim()))
                                        gvwMonthQuestions.Rows[rowIndex].Cells["gvtOverride"].Value = BlankImg;
                                    else
                                        gvwMonthQuestions.Rows[rowIndex].Cells["gvtOverride"].Value = Img_Edit;
                                }
                                else
                                {
                                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtOverride"].ReadOnly = true;
                                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtOverride"].Value = BlankImg;
                                }
                            }
                            //gvwMonthQuestions.Rows[rowIndex].Cells["gvtOverride"].Value = Img_Edit;

                            gvwMonthQuestions.Rows[rowIndex].Cells["gvkChk"].ReadOnly = true;
                        }
                        gvwMonthQuestions.Rows[rowIndex].Cells["gvtServicedate"].ReadOnly = true;

                        if (!string.IsNullOrEmpty(ServsDate.Trim()) && string.IsNullOrEmpty(ChkDate.Trim()))
                        {
                            if (PostUsagePriv != null)
                            {
                                if (PostUsagePriv.DelPriv.ToUpper() == "TRUE")
                                {
                                    gvwMonthQuestions.Rows[rowIndex].Cells["gvDelete"].ReadOnly = false;
                                    gvwMonthQuestions.Rows[rowIndex].Cells["gvDelete"].Value = DeleteImg;
                                }
                                else
                                {
                                    gvwMonthQuestions.Rows[rowIndex].Cells["gvDelete"].ReadOnly = true;
                                    gvwMonthQuestions.Rows[rowIndex].Cells["gvDelete"].Value = BlankImg;
                                }
                            }

                            //gvwMonthQuestions.Rows[rowIndex].Cells["gvDelete"].Value = DeleteImg;
                        }

                        if (!string.IsNullOrEmpty(BundleNo.Trim()))
                        {
                            gvwMonthQuestions.Rows[rowIndex].Cells["gvDelete"].ReadOnly = true;
                            gvwMonthQuestions.Rows[rowIndex].Cells["gvDelete"].Value = BlankImg;
                        }


                        if (Iscaseact == "Y")
                        {
                            gvwMonthQuestions.Rows[rowIndex].Tag = CaseactRec;
                            set_CAMS_Tooltip(rowIndex, CaseactRec.Add_Date, CaseactRec.Add_Operator, CaseactRec.Lstc_Date, CaseactRec.Lsct_Operator);

                        }
                    }
                }
            }

            if (gvwMonthQuestions.Rows.Count > 0)
            {
                foreach (DataGridViewRow _dRow in gvwMonthQuestions.Rows)
                {
                    string IntakeDate = BaseForm.BaseCaseMstListEntity[0].IntakeDate.Trim();
                    string Month = GetMonth(_dRow.Cells["gvtMonthQuesCode"].Value.ToString().Trim());
                    string StrDate = Convert.ToDateTime(Month + "/" + "1/" + Convert.ToDateTime(IntakeDate).Year).ToShortDateString();
                    int IntakeYear = int.Parse(Convert.ToDateTime(IntakeDate).Year.ToString());

                    if (Convert.ToDateTime(IntakeDate.Trim()) <= Convert.ToDateTime(StrDate.Trim()))
                    {
                        this.gvwMonthQuestions.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvwMonthQuestions_CellValueChanged);
                        string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear - 1)).ToShortDateString();
                        _dRow.Cells["gvtUsageReq"].Value = StrUsageDate;
                        if (string.IsNullOrEmpty(_dRow.Cells["gvtServicedate"].Value.ToString().Trim()))
                            _dRow.Cells["gvtServicedate"].Value = StrDate;

                        this.gvwMonthQuestions.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvwMonthQuestions_CellValueChanged);
                    }
                    else
                    {
                        this.gvwMonthQuestions.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvwMonthQuestions_CellValueChanged);
                        int IntYear = int.Parse(Convert.ToDateTime(IntakeDate).Year.ToString());
                        string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear + 1)).ToShortDateString();

                        _dRow.Cells["gvtUsageReq"].Value = StrDate;

                        if (string.IsNullOrEmpty(_dRow.Cells["gvtServicedate"].Value.ToString().Trim()))
                            _dRow.Cells["gvtServicedate"].Value = StrUsageDate;

                        this.gvwMonthQuestions.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvwMonthQuestions_CellValueChanged);

                    }
                    if (((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() == ServiceCode)
                    {
                        if (Convert.ToDateTime(_dRow.Cells["gvtServicedate"].Value.ToString().Trim()) >= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_START.Trim())
                        && Convert.ToDateTime(_dRow.Cells["gvtServicedate"].Value.ToString().Trim()) <= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_END.Trim()))
                        {
                            //gvwMonthQuestions.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                            this.gvwMonthQuestions.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvwMonthQuestions_CellValueChanged);
                            if (_dRow.Cells["gvkChk"].ReadOnly != true)
                            {
                                _dRow.Cells["gvtUsageReq"].Value = "";
                                _dRow.Cells["gvtServicedate"].Value = "";
                            }
                            this.gvwMonthQuestions.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvwMonthQuestions_CellValueChanged);
                        }
                        else
                        {

                            //_dRow.Cells.ForEach(y => y.ReadOnly = true);
                            //_dRow.Cells.ForEach(y => y.Style.ForeColor = System.Drawing.Color.Silver);
                            //_dRow.Cells[1].Style.ForeColor = Color.OrangeRed;

                            //_dRow.Cells.ForEach(y => y.Style.ForeColor = System.Drawing.Color.Silver);
                            _dRow.Cells[1].Style.ForeColor = Color.OrangeRed;


                            string strMsg = "The Service date must be between " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) + " and " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_END.Trim());
                            _dRow.Cells.ForEach(y => y.ToolTipText = strMsg);
                        }
                    }
                }



            }


            gvwMonthQuestions.Update();

        }

       

        private void set_CAMS_Tooltip(int rowIndex, string Add_Date, string Add_Opr, string Lstc_Date, string Lstc_Opr)
        {
            string toolTipText = "Added By     : " + Add_Opr + " on " + Add_Date + "\n" +
                                 "Modified By  : " + Lstc_Opr + " on " + Lstc_Date;

            foreach (DataGridViewCell cell in gvwMonthQuestions.Rows[rowIndex].Cells)
                cell.ToolTipText = toolTipText;
        }

        private string GetMonthName(string Month)
        {
            string MonName = string.Empty;

            switch (Month)
            {
                case "JAN": MonName = "January"; break;
                case "FEB": MonName = "February"; break;
                case "MAR": MonName = "March"; break;
                case "APR": MonName = "April"; break;
                case "MAY": MonName = "May"; break;
                case "JUN": MonName = "June"; break;
                case "JUL": MonName = "July"; break;
                case "AUG": MonName = "August"; break;
                case "SEP": MonName = "September"; break;
                case "OCT": MonName = "October"; break;
                case "NOV": MonName = "November"; break;
                case "DEC": MonName = "December"; break;
            }

            return MonName;
        }

        private string GetMonth(string Month)
        {
            string MonName = string.Empty;

            switch (Month)
            {
                case "JAN": MonName = "1"; break;
                case "FEB": MonName = "2"; break;
                case "MAR": MonName = "3"; break;
                case "APR": MonName = "4"; break;
                case "MAY": MonName = "5"; break;
                case "JUN": MonName = "6"; break;
                case "JUL": MonName = "7"; break;
                case "AUG": MonName = "8"; break;
                case "SEP": MonName = "9"; break;
                case "OCT": MonName = "10"; break;
                case "NOV": MonName = "11"; break;
                case "DEC": MonName = "12"; break;
            }

            return MonName;
        }

        decimal ResAmount = 0; string ChkNo = string.Empty; string ChkDate = string.Empty; string ServsDate = string.Empty; string SeekDate = string.Empty;
        CASEACTEntity CaseactRec = new CASEACTEntity();
        private void InvoiceDetails(string Month)
        {
            ResAmount = 0; ChkNo = string.Empty; ChkDate = string.Empty; ServsDate = string.Empty; SeekDate = string.Empty; CaseactRec = new CASEACTEntity();
            List<CASEACTEntity> CaseactList = new List<CASEACTEntity>();
            if (((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() == ServiceCode)
            {
                if (SP_Activity_Details.Count > 0)
                {
                    switch (Month)
                    {
                        case "JAN":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "1" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "FEB":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "2" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "MAR":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "3" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "APR":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "4" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "MAY":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "5" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "JUN":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "6" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "JUL":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "7" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "AUG":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "8" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());// CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "SEP":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "9" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "OCT":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "10" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "NOV":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "11" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "DEC":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "12" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                    }
                    //CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "");
                }
            }
            else
            {
                if (SP_Activity_Details.Count > 0)
                {
                    switch (Month)
                    {
                        case "JAN":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "1" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "FEB":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "2" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "MAR":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "3" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "APR":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "4" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "MAY":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "5" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "JUN":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "6" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "JUL":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "7" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "AUG":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "8" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "SEP":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "9" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "OCT":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "10" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "NOV":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "11" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                        case "DEC":
                            CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "" && Convert.ToDateTime(u.ActSeek_Date).Month.ToString() == "12" && u.ACT_Code.Trim() == ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() && u.Elec_Other == StrElec);
                            if (CaseactList.Count > 0)
                            {
                                ResAmount = Convert.ToDecimal(CaseactList[0].Cost.Trim());//CaseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                                ChkNo = CaseactList[0].Check_No; ChkDate = CaseactList[0].Check_Date.Trim(); ServsDate = CaseactList[0].ACT_Date.Trim(); SeekDate = CaseactList[0].ActSeek_Date.Trim();
                                //if (CaseactList.Count == 1) 
                                CaseactRec = CaseactList[0];
                            }
                            break;
                    }
                    //CaseactList = SP_Activity_Details.FindAll(u => u.Cost.Trim() != "");
                }
            }

        }

        private void gvwMonthQuestions_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwMonthQuestions.Rows.Count > 0)
            {
                string strcmbusage = gvwMonthQuestions.CurrentRow.Cells["gvtElecpayment"].Value == null ? string.Empty : gvwMonthQuestions.CurrentRow.Cells["gvtElecpayment"].Value.ToString();
                string strcmbAmount = gvwMonthQuestions.CurrentRow.Cells["gvtAmtPaid"].Value == null ? string.Empty : gvwMonthQuestions.CurrentRow.Cells["gvtAmtPaid"].Value.ToString();

                //if (strcmbAmount != strcmbusage) btnInvoices.Visible = true; else btnInvoices.Visible = false;

                gvwMonthQuestions.CurrentRow.Cells["gvtServicedate"].ReadOnly = true;
                gvwMonthQuestions.CurrentRow.Cells["gvtAmtPaid"].ReadOnly = true;
            }
        }

        private void panel3_Click(object sender, EventArgs e)
        {

        }

        //decimal 
        int RecCount = 0;
        private void gvwMonthQuestions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvwMonthQuestions.Rows.Count > 0)
            {
                gvwMonthQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwMonthQuestions_CellValueChanged);
                if (e.ColumnIndex == gvkChk.Index && e.RowIndex != -1)
                {
                    //if(((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim()==ServiceCode)
                    //{

                    if (gvwMonthQuestions.Rows[e.RowIndex].Cells["gvkChk"].ReadOnly == false)
                    {
                        bool IsFalse = true; IsCal = false;
                        string strdata = gvwMonthQuestions.Rows[e.RowIndex].Cells["gvkChk"].Value.ToString();
                        int introwindex = gvwMonthQuestions.CurrentCell.RowIndex;
                        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtServicedate"].ReadOnly = true;
                        if (strdata.ToUpper() == "TRUE")
                        {
                            RecCount++;
                            string UsageAmount = gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtElecpayment"].Value.ToString();
                            decimal OrigBal = 0;
                            if (!string.IsNullOrEmpty(UsageAmount.Trim()))
                            {
                                this.gvwMonthQuestions.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvwMonthQuestions_CellValueChanged);
                                gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtAmtPaid"].Value = UsageAmount;
                                this.gvwMonthQuestions.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvwMonthQuestions_CellValueChanged);

                                decimal Amount = Convert.ToDecimal(UsageAmount.Trim());
                                if (!string.IsNullOrEmpty(txtBalance.Text.Trim()))
                                {
                                    if (Convert.ToDecimal(txtBalance.Text.Trim()) > 0)
                                        OrigBal = Convert.ToDecimal(txtBalance.Text.Trim());
                                }


                                isAlert = false;
                                if (IsCal == false)
                                    CalculateAmounts(introwindex);

                                decimal Balance = 0;
                                if (!string.IsNullOrEmpty(txtBalance.Text.Trim()))
                                    Balance = Convert.ToDecimal(txtBalance.Text.Trim());// - Amount;

                                if (((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() == ServiceCode)
                                {
                                    if (Amount > 0)
                                    {
                                        int MaxTrans = int.Parse(txtMaxInvs.Text.ToString());
                                        int PaidTrans = 0;
                                        if (!string.IsNullOrEmpty(txtMaxInvs.Text.Trim()))
                                        {
                                            if (!string.IsNullOrEmpty(txtRemTrans.Text.Trim())) PaidTrans = int.Parse(txtRemTrans.Text.Trim());
                                            PaidTrans++;

                                            if (PaidTrans > MaxTrans)
                                            {
                                                gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtAmtPaid"].Value = string.Empty;
                                                gvwMonthQuestions.Rows[e.RowIndex].Cells["gvkChk"].Value = false;
                                                AlertBox.Show("we can't allow the “Transaction  Posted” to exceed the maximum Invoices!!", MessageBoxIcon.Warning);
                                                IsFalse = false;
                                            }
                                            else
                                            {



                                                //txtRemTrans.Text = PaidTrans.ToString();
                                                //txtInvBal.Text = (MaxTrans - (PaidTrans)).ToString();
                                            }

                                        }


                                        if (Balance <= 0)
                                        {
                                            if (Convert.ToDecimal(txtBalance.Text.Trim()) > 0)
                                            {
                                                gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtAmtPaid"].Value = txtBalance.Text;
                                                txtBalance.Text = "0";
                                            }
                                            else
                                            {
                                                if (gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtAmtPaid"].Value.ToString() != OrigBal.ToString())
                                                {
                                                    gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtAmtPaid"].Value = string.Empty;
                                                    gvwMonthQuestions.Rows[e.RowIndex].Cells["gvkChk"].Value = false;

                                                    if (isAlert == false)
                                                        AlertBox.Show("we can't allow the “Total Dollars Posted” to exceed the maximum award!!", MessageBoxIcon.Warning);
                                                    IsFalse = false;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //if (Balance > 0)
                                            //{
                                            //    gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtAmtPaid"].Value = Balance;
                                            //    Balance = 0;
                                            //    CalculateAmounts(introwindex);
                                            //}

                                            //if(IsFalse)
                                            //    txtBalance.Text = Balance.ToString();
                                        }

                                        if (IsFalse)
                                        {
                                            txtRemTrans.Text = PaidTrans.ToString();
                                            txtInvBal.Text = (MaxTrans - (PaidTrans)).ToString();
                                            if (Balance >= 0)
                                                txtBalance.Text = Balance.ToString();
                                        }
                                    }
                                    else
                                    {
                                        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtAmtPaid"].Value = string.Empty;
                                        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvkChk"].Value = false;

                                        AlertBox.Show("You cannot post invoice with Amount Paid of zero", MessageBoxIcon.Warning);
                                        IsFalse = false;
                                    }



                                }

                            }

                            if (IsFalse)
                            {
                                string IntakeDate = BaseForm.BaseCaseMstListEntity[0].IntakeDate.Trim();
                                string Month = GetMonth(gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtMonthQuesCode"].Value.ToString().Trim());
                                string StrDate = Convert.ToDateTime(Month + "/" + "1/" + Convert.ToDateTime(IntakeDate).Year).ToShortDateString();
                                int IntakeYear = int.Parse(Convert.ToDateTime(IntakeDate).Year.ToString());

                                if (Convert.ToDateTime(IntakeDate.Trim()) <= Convert.ToDateTime(StrDate.Trim()))
                                {
                                    string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear - 1)).ToShortDateString();
                                    gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtUsageReq"].Value = StrUsageDate;
                                    if (string.IsNullOrEmpty(gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtServicedate"].Value.ToString().Trim()))
                                        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtServicedate"].Value = StrDate;

                                    if (PostUsagePriv != null)
                                    {
                                        if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                                        {
                                            gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = false;
                                            gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtOverride"].Value = Img_Edit;
                                        }
                                        else
                                        {
                                            gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = true;
                                            gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtOverride"].Value = BlankImg;
                                        }
                                    }
                                    //gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtOverride"].Value = Img_Edit;

                                }
                                else
                                {
                                    int IntYear = int.Parse(Convert.ToDateTime(IntakeDate).Year.ToString());
                                    string StrUsageDate = Convert.ToDateTime(Month + "/" + "1/" + (IntakeYear + 1)).ToShortDateString();
                                    //StrDate = Convert.ToDateTime("01/" + Month + "/" + IntYear.ToString()).ToShortDateString();
                                    gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtUsageReq"].Value = StrDate;

                                    if (string.IsNullOrEmpty(gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtServicedate"].Value.ToString().Trim()))
                                        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtServicedate"].Value = StrUsageDate;

                                    if (PostUsagePriv != null)
                                    {
                                        if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                                        {
                                            gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = false;
                                            gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtOverride"].Value = Img_Edit;
                                        }
                                        else
                                        {
                                            gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtOverride"].ReadOnly = true;
                                            gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtOverride"].Value = BlankImg;
                                        }
                                    }
                                    //gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtOverride"].Value = Img_Edit;

                                }

                                if (((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() == ServiceCode)
                                {
                                    if (Convert.ToDateTime(gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtServicedate"].Value.ToString().Trim()) >= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) && Convert.ToDateTime(gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtServicedate"].Value.ToString().Trim()) <= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_END.Trim()))
                                    {
                                        gvwMonthQuestions.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;

                                        if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y")
                                        {
                                            decimal uAmount = gvwMonthQuestions.Rows.Cast<DataGridViewRow>()
                               .Where(row => !row.IsNewRow && (row.Cells[1].Style.ForeColor != Color.OrangeRed  )) // && !(row.Cells["gvkChk"].Value.ToString().ToUpper() == "TRUE" && row.Cells["gvkChk"].ReadOnly == true)
                                               .Sum(row =>
                                               {
                                                   var cellValue = row.Cells["gvtElecpayment"].Value;
                                                   if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                                                   {
                                                       return 0m;
                                                   }

                                                   if (decimal.TryParse(cellValue.ToString(), out decimal amount))
                                                   {
                                                       return amount;
                                                   }

                                                   return 0m;
                                               });

                                            if (uAmount > Convert.ToDecimal(SPM_Entity.SPM_Balance))
                                            {
                                                uAmount = Convert.ToDecimal(SPM_Entity.SPM_Balance);
                                            }
                                            decimal _balAmt = Convert.ToDecimal(SPM_Entity.SPM_Balance) - Convert.ToDecimal(uAmount);
                                            txtBalance.Text = _balAmt.ToString();
                                            this.gvwMonthQuestions.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvwMonthQuestions_CellValueChanged);
                                            gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtAmtPaid"].Value = uAmount.ToString();
                                            this.gvwMonthQuestions.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvwMonthQuestions_CellValueChanged);


                                            if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y")
                                            {
                                                for (int x = 0; x < gvwMonthQuestions.Rows.Count; x++)
                                                {
                                                    if (gvwMonthQuestions.Rows[x].Cells[1].Style.ForeColor != Color.OrangeRed)
                                                    {
                                                        if (gvwMonthQuestions.Rows[x].Cells["gvkChk"].Value.ToString().ToUpper() == "TRUE")
                                                        {
                                                            //gvwMonthQuestions.Rows[x].Cells["gvtOverride"].Value = BlankImg;
                                                        }
                                                        else
                                                        {
                                                            gvwMonthQuestions.Rows[x].Cells.ForEach(y => y.ReadOnly = true);
                                                            gvwMonthQuestions.Rows[x].Cells.ForEach(y => y.Style.ForeColor = System.Drawing.Color.Silver);
                                                        }
                                                    }
                                                }
                                            }

                                        }

                                    }
                                    else
                                    {
                                        decimal cellAmount = 0;
                                        if (!string.IsNullOrEmpty(UsageAmount.Trim()))
                                            cellAmount = Convert.ToDecimal(UsageAmount.Trim());
                                        //gvwMonthQuestions.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.OrangeRed;

                                        gvwMonthQuestions.Rows[e.RowIndex].Cells.ForEach(x => x.Style.ForeColor = Color.Gray);
                                        gvwMonthQuestions.Rows[e.RowIndex].Cells[1].Style.ForeColor = Color.OrangeRed;


                                        txtBalance.Text = OrigBal.ToString();
                                        AlertBox.Show("The Service date must be between " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) + " and " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_END.Trim()), MessageBoxIcon.Warning);
                                        //booldatevalid = false;
                                    }
                                }



                            }
                            if (RecCount > 0)
                            {
                                if (PostUsagePriv != null)
                                {
                                    if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                                        btnInvoices.Enabled = true;
                                    else
                                        btnInvoices.Enabled = false;

                                }
                                //btnInvoices.Enabled = true;
                            }
                            else btnInvoices.Enabled = false;
                        }
                        else
                        {
                            RecCount--;

                            string UsageAmount = gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtAmtPaid"].Value.ToString();
                            if (!string.IsNullOrEmpty(UsageAmount.Trim()))
                            {
                                //gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtAmtPaid"].Value = UsageAmount;

                                decimal Amount = Convert.ToDecimal(UsageAmount.Trim());
                                decimal Balance = 0;
                                if (!string.IsNullOrEmpty(txtBalance.Text.Trim()))
                                    Balance = Convert.ToDecimal(txtBalance.Text.Trim());
                                if (gvwMonthQuestions.Rows[e.RowIndex].DefaultCellStyle.BackColor != Color.Red)
                                {
                                    if (!string.IsNullOrEmpty(txtBalance.Text.Trim()))
                                        Balance = Convert.ToDecimal(txtBalance.Text.Trim()) + Amount;
                                }
                                //decimal Balance = Convert.ToDecimal(txtBalance.Text.Trim()) + Amount;

                                if (((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() == ServiceCode)
                                {
                                    if (Balance <= 0)
                                    {
                                        AlertBox.Show("we can't allow the “Total Dollars Posted” to exceed the maximum award!!", MessageBoxIcon.Warning);
                                        IsFalse = false;
                                    }
                                    else
                                    {
                                        txtBalance.Text = Balance.ToString();
                                        if (CEAPCNTL_List[0].CPCT_LUMP_SUM.ToString() == "Y")
                                        {
                                            for (int x = 0; x < gvwMonthQuestions.Rows.Count; x++)
                                            {
                                                if (gvwMonthQuestions.Rows[x].Cells[1].Style.ForeColor != Color.OrangeRed)
                                                {
                                                    gvwMonthQuestions.Rows[x].Cells.ForEach(y => y.ReadOnly = false);
                                                    gvwMonthQuestions.Rows[x].Cells.ForEach(y => y.Style.ForeColor = System.Drawing.Color.Black);
                                                }
                                            }
                                        }
                                    }


                                    if (!string.IsNullOrEmpty(txtMaxInvs.Text.Trim()))
                                    {
                                        int MaxTrans = int.Parse(txtMaxInvs.Text.ToString());
                                        int PaidTrans = 0;
                                        if (!string.IsNullOrEmpty(txtRemTrans.Text.Trim())) PaidTrans = int.Parse(txtRemTrans.Text.Trim());
                                        PaidTrans--;

                                        if (PaidTrans > MaxTrans)
                                        {
                                            gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtAmtPaid"].Value = string.Empty;
                                            gvwMonthQuestions.Rows[e.RowIndex].Cells["gvkChk"].Value = false;
                                            AlertBox.Show("we can't allow the “Transaction  Posted” to exceed the maximum Invoices!!", MessageBoxIcon.Warning);
                                            IsFalse = false;
                                        }
                                        else
                                        {
                                            txtRemTrans.Text = PaidTrans.ToString();
                                            txtInvBal.Text = (MaxTrans - (PaidTrans)).ToString();
                                        }

                                    }
                                }

                            }



                            gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtUsageReq"].Value = string.Empty;
                            gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtServicedate"].Value = string.Empty;
                            gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtOverride"].Value = BlankImg;
                            gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtAmtPaid"].Value = string.Empty;
                            gvwMonthQuestions.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;

                            if (RecCount > 0)
                            {
                                if (PostUsagePriv != null)
                                {
                                    if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                                        btnInvoices.Enabled = true;
                                    else
                                        btnInvoices.Enabled = false;
                                }
                                //btnInvoices.Enabled = true;
                            }
                            else btnInvoices.Enabled = false;
                        }
                    }
                    //}

                }
                if (e.ColumnIndex == gvtOverride.Index && e.RowIndex != -1)
                {
                    if (gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtOverride"].Value.ToString() == Img_Edit)
                    {
                        if (gvwMonthQuestions.Rows[e.RowIndex].Cells["gvCaseact"].Value.ToString() == "Y")
                        {
                            CASEACTEntity Entity = gvwMonthQuestions.Rows[e.RowIndex].Tag as CASEACTEntity;

                            if (PostUsagePriv != null)
                            {
                                if (PostUsagePriv.ChangePriv.ToUpper() == "TRUE")
                                {
                                    if (string.IsNullOrEmpty(Entity.BundleNo.Trim()))
                                    {
                                        CASE0016_UsageEditForm EditForm = new CASE0016_UsageEditForm(BaseForm, "Edit", SPM_Entity, Entity, CEAPCNTL_List[0], txtBalance.Text.Trim(), Privileges);
                                        EditForm.FormClosed += new FormClosedEventHandler(On_Edit_Form_Closed);
                                        EditForm.StartPosition = FormStartPosition.CenterScreen;
                                        EditForm.ShowDialog();
                                    }

                                }
                            }

                        }
                        else
                        {
                            CASEACTEntity Entity = new CASEACTEntity();
                            Entity.Service_plan = SP_Code;
                            Entity.ACT_Code = ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString();
                            Entity.ACT_Date = gvwMonthQuestions.CurrentRow.Cells["gvtServicedate"].Value.ToString();
                            Entity.Cost = gvwMonthQuestions.CurrentRow.Cells["gvtAmtPaid"].Value.ToString();
                            Entity.PDOUT = gvwMonthQuestions.CurrentRow.Cells["gvPDOut"].Value.ToString();
                            if (PostUsagePriv != null)
                            {
                                if (PostUsagePriv.AddPriv.ToUpper() == "TRUE")
                                {
                                    CASE0016_UsageEditForm EditForm = new CASE0016_UsageEditForm(BaseForm, "Add", SPM_Entity, Entity, CEAPCNTL_List[0], txtBalance.Text.Trim(), Privileges);
                                    EditForm.FormClosed += new FormClosedEventHandler(On_Edit_Form_Closed);
                                    EditForm.StartPosition = FormStartPosition.CenterScreen;
                                    EditForm.ShowDialog();
                                }
                            }


                            //gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtServicedate"].ReadOnly = false;
                            //gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtAmtPaid"].ReadOnly = false;
                        }
                    }
                }
                if (e.ColumnIndex == gvDelete.Index && e.RowIndex != -1)
                {
                    if (gvwMonthQuestions.Rows[e.RowIndex].Cells["gvCaseact"].Value.ToString() == "Y")
                    {
                        CASEACTEntity Entity = gvwMonthQuestions.Rows[e.RowIndex].Tag as CASEACTEntity;

                        if (string.IsNullOrEmpty(Entity.BundleNo.Trim()))
                        {
                            if (PostUsagePriv != null)
                            {
                                if (PostUsagePriv.DelPriv.ToUpper() == "TRUE")
                                {
                                    MessageBox.Show(/*Consts.Messages.AreYouSureYouWantToDelete.GetMessage() +*/ "Are you sure you want to delete " + gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtMonthQues"].Value.ToString() + " Invoice?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Selected_CAMS);
                                }
                            }
                        }

                    }
                }

                gvwMonthQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwMonthQuestions_CellValueChanged);
            }
        }

        private void Delete_Selected_CAMS(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                bool Delete_Status = true;
                int New_MS_ID = 0, Tmp_CA_Seq = 0; bool VouchMSG = false; int CountCA = 0, CountMS = 0;

                CASEACTEntity Entity = gvwMonthQuestions.CurrentRow.Tag as CASEACTEntity;
                Entity.Rec_Type = "D";

                if (!_model.SPAdminData.UpdateCASEACT2(Entity, "Delete", out New_MS_ID, out Tmp_CA_Seq, out Sql_SP_Result_Message))
                    Delete_Status = false;

                if (Delete_Status)
                {
                    Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, Entity.Fund1);

                    if (Emsbdc_List.Count > 0)
                        Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == Entity.BDC_ID);
                    CMBDCEntity emsbdcentity = Emsbdc_List.Find(u => (Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(SPM_Entity.startdate.Trim()) && Convert.ToDateTime(u.BDC_END) >= Convert.ToDateTime(SPM_Entity.startdate.Trim())));
                    if (emsbdcentity != null)
                    {
                        CMBDCEntity emsbdcdata = new CMBDCEntity();
                        emsbdcdata.BDC_AGENCY = emsbdcentity.BDC_AGENCY;
                        emsbdcdata.BDC_DEPT = emsbdcentity.BDC_DEPT;
                        emsbdcdata.BDC_PROGRAM = emsbdcentity.BDC_PROGRAM;
                        emsbdcdata.BDC_YEAR = emsbdcentity.BDC_YEAR;


                        emsbdcdata.BDC_DESCRIPTION = emsbdcentity.BDC_DESCRIPTION;
                        emsbdcdata.BDC_FUND = emsbdcentity.BDC_FUND;
                        emsbdcdata.BDC_ID = emsbdcentity.BDC_ID;
                        emsbdcdata.BDC_START = emsbdcentity.BDC_START;
                        emsbdcdata.BDC_END = emsbdcentity.BDC_END;
                        emsbdcdata.BDC_BUDGET = emsbdcentity.BDC_BUDGET;
                        emsbdcdata.BDC_LSTC_OPERATOR = BaseForm.UserID;
                        emsbdcdata.Mode = "BdcAmount";
                        string strstatus = string.Empty;
                        if (_model.SPAdminData.InsertUpdateDelCMBDC(emsbdcdata, out strstatus))
                        { }
                    }

                    AlertBox.Show(gvwMonthQuestions.CurrentRow.Cells["gvtMonthQues"].Value.ToString() + " Invoice Deleted Successfully");
                    Fill_Applicant_SPs();
                    Get_App_CASEACT_List(SPM_Entity);
                    fillMonthlyQuestions();
                }


            }
        }

        private void On_Edit_Form_Closed(object sender, FormClosedEventArgs e)
        {
            CASE0016_UsageEditForm form = sender as CASE0016_UsageEditForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] Selected_Details = new string[4];
                Selected_Details = form.Get_Selected_Row();

                //Txt_VendNo.Text = Vendor_Details[0].Trim();
                //Text_VendName.Text = Vendor_Details[1].Trim();
                if (Selected_Details[0].ToString() == "Add")
                {
                    gvwMonthQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwMonthQuestions_CellValueChanged);
                    gvwMonthQuestions.CurrentRow.Cells["gvtServicedate"].Value = Selected_Details[1].ToString();
                    gvwMonthQuestions.CurrentRow.Cells["gvtAmtPaid"].Value = Selected_Details[2].ToString();
                    gvwMonthQuestions.CurrentRow.Cells["gvPDOut"].Value = Selected_Details[3].ToString();
                    gvwMonthQuestions.CurrentRow.Tag = Selected_Details[4].ToString();

                    int introwindex = gvwMonthQuestions.CurrentCell.RowIndex;

                    if (((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() == ServiceCode)
                        CalculateAmounts(introwindex);

                    gvwMonthQuestions.SelectedRows[0].DefaultCellStyle.BackColor = Color.Green;

                    gvwMonthQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwMonthQuestions_CellValueChanged);
                }
                else
                {
                    Fill_Applicant_SPs();
                    Get_App_CASEACT_List(SPM_Entity);
                    fillMonthlyQuestions();
                }
            }
        }


        string Sql_SP_Result_Message = string.Empty;
        private void btnInvoices_Click(object sender, EventArgs e)
        {
            if (CmbSP.Items.Count > 0)
            {
                if (isValidateForm())
                {

                    if (gvwMonthQuestions.Rows.Count > 0)
                    {
                        bool IsPost = false;
                        foreach (DataGridViewRow dr in gvwMonthQuestions.Rows)
                        {
                            if (dr.Cells["gvkChk"].Value.ToString().ToUpper() == "TRUE" && dr.Cells["gvCaseact"].Value.ToString() == "N")
                            {
                                Pass_CA_Entity.Rec_Type = "I";
                                if (dr.Cells["gvCaseact"].Value.ToString() == "Y")
                                    Pass_CA_Entity.Rec_Type = "U";

                                Get_Latest_Activity_data(dr);

                                if (dr.Tag != null)
                                {
                                    Pass_CA_Entity.BDC_ID = dr.Tag.ToString();
                                    CMBDCEntity BDCEntity = ALLEmsbdc_List.Find(u => u.BDC_ID == Pass_CA_Entity.BDC_ID);
                                    if (BDCEntity != null) Pass_CA_Entity.Fund1 = BDCEntity.BDC_FUND;
                                }

                                int New_CAID = 1, New_CA_Seq = 1; string Operatipn_Mode = "Insert";
                                if (Pass_CA_Entity.Rec_Type == "U")
                                    Operatipn_Mode = "Update";
                                //if(!string.IsNullOrEmpty(dr.Cells[""].Value.ToString()))
                                if (Pass_CA_Entity.Rec_Type == "I")
                                { Pass_CA_Entity.ACT_ID = New_CAID.ToString(); Pass_CA_Entity.ACT_Seq = New_CA_Seq.ToString(); }

                                //Addded by Sudheer on 05/25/23
                                //Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, SPM_Entity.SPM_Fund);
                                //if (Emsbdc_List.Count > 0)
                                //    Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == SPM_Entity.SPM_BDC_ID);

                                //Modified by Sudheer on 09/11/24 as per the NCCAA.docx on 'POST INVOICE ADJSUTMENT 9/10/2024'
                                Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, Pass_CA_Entity.Fund1);
                                if (Emsbdc_List.Count > 0)
                                    Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == Pass_CA_Entity.BDC_ID);


                                decimal decamount = 0; bool boolsucess = true;
                                decimal decbal = 0;
                                if (Emsbdc_List[0] != null)
                                {
                                    decbal = Convert.ToDecimal(Emsbdc_List[0].BDC_BALANCE.Trim());
                                    decamount = Convert.ToDecimal(Pass_CA_Entity.Cost.Trim());
                                }

                                if (decamount > decbal)
                                {
                                    CommonFunctions.MessageBoxDisplay("Insufficent funds in budget to post this Benefit \n Amount may not be more than " + decbal);
                                    boolsucess = false;
                                    btnSave.Enabled = true;
                                    break;
                                }

                                if (boolsucess)
                                {
                                    if (_model.SPAdminData.UpdateCASEACT3(Pass_CA_Entity, Operatipn_Mode, out New_CAID, out New_CA_Seq, out Sql_SP_Result_Message))
                                    {
                                        IsPost = true;

                                        Pass_CA_Entity.ACT_ID = New_CAID.ToString();
                                        Pass_CA_Entity.ACT_Seq = New_CA_Seq.ToString();

                                        CAOBOEntity Search_CAOBO_Entity = new CAOBOEntity();
                                        Search_CAOBO_Entity.ID = New_CAID.ToString();
                                        Search_CAOBO_Entity.Rec_Type = "S";
                                        Search_CAOBO_Entity.Seq = "1";

                                        _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Delete", out Sql_SP_Result_Message);
                                        foreach (CaseSnpEntity Entity in BaseForm.BaseCaseSnpEntity)
                                        {
                                            if (Entity.Status == "A" && Entity.Exclude == "N")
                                            {
                                                Search_CAOBO_Entity.CLID = Entity.ClientId.ToString();
                                                Search_CAOBO_Entity.Fam_Seq = Entity.FamilySeq.ToString();
                                                Search_CAOBO_Entity.Seq = "1";
                                                Search_CAOBO_Entity.Rec_Type = "I";

                                                _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Insert", out Sql_SP_Result_Message);
                                            }
                                        }

                                        //Modified by Sudheer on 09/11/24 as per the NCCAA.docx on 'POST INVOICE ADJSUTMENT 9/10/2024'
                                        if (Pass_CA_Entity.Fund1.Trim() != SPM_Entity.SPM_Fund.Trim() || Pass_CA_Entity.BDC_ID != SPM_Entity.SPM_BDC_ID)
                                        {

                                            Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, SPM_Entity.SPM_Fund);

                                            if (Emsbdc_List.Count > 0)
                                                Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == SPM_Entity.SPM_BDC_ID);
                                            CMBDCEntity emsbdcentity = Emsbdc_List.Find(u => (Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(SPM_Entity.startdate.Trim()) && Convert.ToDateTime(u.BDC_END) >= Convert.ToDateTime(SPM_Entity.startdate.Trim())));
                                            if (emsbdcentity != null)
                                            {
                                                CMBDCEntity emsbdcdata = new CMBDCEntity();
                                                emsbdcdata.BDC_AGENCY = emsbdcentity.BDC_AGENCY;
                                                emsbdcdata.BDC_DEPT = emsbdcentity.BDC_DEPT;
                                                emsbdcdata.BDC_PROGRAM = emsbdcentity.BDC_PROGRAM;
                                                emsbdcdata.BDC_YEAR = emsbdcentity.BDC_YEAR;


                                                emsbdcdata.BDC_DESCRIPTION = emsbdcentity.BDC_DESCRIPTION;
                                                emsbdcdata.BDC_FUND = emsbdcentity.BDC_FUND;
                                                emsbdcdata.BDC_ID = emsbdcentity.BDC_ID;
                                                emsbdcdata.BDC_START = emsbdcentity.BDC_START;
                                                emsbdcdata.BDC_END = emsbdcentity.BDC_END;
                                                emsbdcdata.BDC_BUDGET = emsbdcentity.BDC_BUDGET;
                                                emsbdcdata.BDC_LSTC_OPERATOR = BaseForm.UserID;
                                                emsbdcdata.Mode = "BdcAmount";
                                                string strstatus = string.Empty;
                                                if (_model.SPAdminData.InsertUpdateDelCMBDC(emsbdcdata, out strstatus))
                                                { }
                                            }
                                        }

                                        Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, Pass_CA_Entity.Fund1);

                                        if (Emsbdc_List.Count > 0)
                                            Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == Pass_CA_Entity.BDC_ID);
                                        CMBDCEntity bdcentity = Emsbdc_List.Find(u => (Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(SPM_Entity.startdate.Trim()) && Convert.ToDateTime(u.BDC_END) >= Convert.ToDateTime(SPM_Entity.startdate.Trim())));
                                        if (bdcentity != null)
                                        {
                                            CMBDCEntity emsbdcdata = new CMBDCEntity();
                                            emsbdcdata.BDC_AGENCY = bdcentity.BDC_AGENCY;
                                            emsbdcdata.BDC_DEPT = bdcentity.BDC_DEPT;
                                            emsbdcdata.BDC_PROGRAM = bdcentity.BDC_PROGRAM;
                                            emsbdcdata.BDC_YEAR = bdcentity.BDC_YEAR;


                                            emsbdcdata.BDC_DESCRIPTION = bdcentity.BDC_DESCRIPTION;
                                            emsbdcdata.BDC_FUND = bdcentity.BDC_FUND;
                                            emsbdcdata.BDC_ID = bdcentity.BDC_ID;
                                            emsbdcdata.BDC_START = bdcentity.BDC_START;
                                            emsbdcdata.BDC_END = bdcentity.BDC_END;
                                            emsbdcdata.BDC_BUDGET = bdcentity.BDC_BUDGET;
                                            emsbdcdata.BDC_LSTC_OPERATOR = BaseForm.UserID;
                                            emsbdcdata.Mode = "BdcAmount";
                                            string strstatus = string.Empty;
                                            if (_model.SPAdminData.InsertUpdateDelCMBDC(emsbdcdata, out strstatus))
                                            { }
                                        }




                                        //if (Operatipn_Mode == "Add")
                                        //    AlertBox.Show("Saved Successfully");
                                        //else
                                        //    AlertBox.Show("Updated Successfully");
                                    }
                                }




                            }
                        }

                        if (IsPost)
                            AlertBox.Show("Invoices are posted Successfully");

                        btnInvoices.Enabled = false;
                        Fill_Applicant_SPs();
                        Get_App_CASEACT_List(SPM_Entity);
                        fillMonthlyQuestions();
                    }
                }
            }
            else
            {
                AlertBox.Show("There is no services to post", MessageBoxIcon.Warning);
            }
        }

        private bool isValidateForm()
        {
            bool isValid = true;

            if (gvwMonthQuestions.Rows.Count > 0)
            {
                if (((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() == ServiceCode)
                {
                    foreach (DataGridViewRow dr in gvwMonthQuestions.Rows)
                    {
                        if (dr.Cells["gvkChk"].Value.ToString().ToUpper() == "TRUE")
                        {
                            if (Convert.ToDateTime(dr.Cells["gvtServicedate"].Value.ToString().Trim()) >= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) && Convert.ToDateTime(dr.Cells["gvtServicedate"].Value.ToString().Trim()) <= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_END.Trim()))
                            {

                            }
                            else if(Convert.ToDateTime(dr.Cells["gvtServicedate"].Value.ToString().Trim()) <= Convert.ToDateTime(SPM_Entity.startdate.Trim()))
                            {
                                AlertBox.Show("'"+dr.Cells["gvtServicedate"].Value.ToString().Trim() + "' Should not be Prior to the 'Service Plan Master Date "  + " in " + dr.Cells["gvtMonthQues"].Value.ToString(), MessageBoxIcon.Warning);
                                isValid = false;
                                break;
                            }
                            else
                            {
                                AlertBox.Show("The Service date must be between " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) + " and " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_END.Trim()) + " in " + dr.Cells["gvtMonthQues"].Value.ToString(), MessageBoxIcon.Warning);
                                isValid = false;
                                break;
                            }
                        }
                    }
                }
            }


            return isValid;
        }

        CASEACTEntity Pass_CA_Entity = new CASEACTEntity();
        private void Get_Latest_Activity_data(DataGridViewRow dr)
        {
            if (Pass_CA_Entity.Rec_Type == "I")
            {
                Pass_CA_Entity.Agency = BaseForm.BaseAgency; Pass_CA_Entity.Dept = BaseForm.BaseDept; Pass_CA_Entity.Program = BaseForm.BaseProg;
                Pass_CA_Entity.Year = BaseForm.BaseYear; Pass_CA_Entity.App_no = BaseForm.BaseApplicationNo; Pass_CA_Entity.Service_plan = SP_Code;
                Pass_CA_Entity.SPM_Seq = Sp_Sequence;

                Pass_CA_Entity.Check_Date = Pass_CA_Entity.Followup_Comp = Pass_CA_Entity.Followup_On =
                Pass_CA_Entity.Fund2 =
                Pass_CA_Entity.Fund3 = Pass_CA_Entity.Check_No = Pass_CA_Entity.Refer_Data = Pass_CA_Entity.Rate =
                Pass_CA_Entity.Cust_Code1 = Pass_CA_Entity.Cust_Value1 = Pass_CA_Entity.Account = Pass_CA_Entity.Amount = Pass_CA_Entity.Amount2 = Pass_CA_Entity.Amount3 =
                Pass_CA_Entity.Cust_Code2 = Pass_CA_Entity.Cust_Value2 = Pass_CA_Entity.ArrearsAmt = Pass_CA_Entity.BillingPeriod = Pass_CA_Entity.BundleNo = Pass_CA_Entity.CA_VEND_PAY_CAT =
                Pass_CA_Entity.Cust_Code3 = Pass_CA_Entity.Cust_Value3 = Pass_CA_Entity.LVL1Apprval = Pass_CA_Entity.LVL1AprrvalDate = Pass_CA_Entity.LVL2Apprval = Pass_CA_Entity.LVL2ApprvalDate =
                Pass_CA_Entity.Cust_Code4 = Pass_CA_Entity.Cust_Value4 = Pass_CA_Entity.SentPmtDate = Pass_CA_Entity.SentPmtUser = Pass_CA_Entity.UOM2 = Pass_CA_Entity.UOM3 =
                Pass_CA_Entity.Cust_Code5 = Pass_CA_Entity.Cust_Value5 =
                Pass_CA_Entity.UOM = Pass_CA_Entity.Units = null;

                Pass_CA_Entity.Add_Operator = BaseForm.UserID;
            }
            else
            {
                CASEACTEntity Entity = dr.Tag as CASEACTEntity;
                if (Entity != null) Pass_CA_Entity = Entity;
            }

            Pass_CA_Entity.ACT_Date = dr.Cells["gvtServicedate"].Value.ToString(); Pass_CA_Entity.Caseworker = SPM_Entity.caseworker;
            Pass_CA_Entity.Site = SPM_Entity.site; Pass_CA_Entity.Act_PROG = SPM_Entity.Def_Program;
            Pass_CA_Entity.Branch = ((Utilities.ListItem)CmbSP.SelectedItem).ID.ToString();
            Pass_CA_Entity.Group = ((Utilities.ListItem)CmbSP.SelectedItem).ValueDisplayCode.ToString();
            Pass_CA_Entity.ACT_Code = ((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString();
            Pass_CA_Entity.BenefitReason = SPM_Entity.Spm_Benefit_Reasn;

            Pass_CA_Entity.CA_OBF = "2";


            //if (string.IsNullOrEmpty(Pass_CA_Entity.Bulk.Trim()))
            //    Pass_CA_Entity.Bulk = "Q";

            if (!string.IsNullOrEmpty(dr.Cells["gvtServicedate"].Value.ToString()))
                Pass_CA_Entity.ACT_Date = dr.Cells["gvtServicedate"].Value.ToString();

            if (!string.IsNullOrEmpty(dr.Cells["gvtUsageReq"].Value.ToString()))
                Pass_CA_Entity.ActSeek_Date = dr.Cells["gvtUsageReq"].Value.ToString();

            if (!string.IsNullOrEmpty(SPM_Entity.SPM_Fund.Trim()))
                Pass_CA_Entity.Fund1 = SPM_Entity.SPM_Fund;

            if (!string.IsNullOrEmpty(SPM_Entity.SPM_BDC_ID.Trim()))
                Pass_CA_Entity.BDC_ID = SPM_Entity.SPM_BDC_ID;

            if (!string.IsNullOrEmpty(dr.Cells["gvPDOut"].Value.ToString()))
                Pass_CA_Entity.PDOUT = dr.Cells["gvPDOut"].Value.ToString();

            //if (!string.IsNullOrEmpty(SPM_Entity.SPM_Fund.Trim()))
            //    Pass_CA_Entity.Fund1 = SPM_Entity.SPM_Fund;


            if (StrElec == "E")
            {
                Pass_CA_Entity.Vendor_No = SPM_Entity.SPM_Vendor; Pass_CA_Entity.Cost = dr.Cells["gvtAmtPaid"].Value.ToString();
                Pass_CA_Entity.BillngType = SPM_Entity.SPM_BillName_Type; ////Pass_CA_Entity.Fund1 = Pass_CA_Entity.Fund1;
                Pass_CA_Entity.BillngLname = SPM_Entity.SPM_Bill_LName; Pass_CA_Entity.BillngFname = SPM_Entity.SPM_Bill_FName;
                Pass_CA_Entity.Account = SPM_Entity.SPM_Account; Pass_CA_Entity.CA_Source = Prim_Source;
                if (((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() == ServiceCode)
                    Pass_CA_Entity.Elec_Other = "E";
            }
            else
            {
                Pass_CA_Entity.Vendor_No = SPM_Entity.SPM_Gas_Vendor; Pass_CA_Entity.Cost = dr.Cells["gvtAmtPaid"].Value.ToString();
                Pass_CA_Entity.BillngType = SPM_Entity.SPM_Gas_BillName_Type; ////Pass_CA_Entity.Fund1 = Pass_CA_Entity.Fund1;
                Pass_CA_Entity.BillngLname = SPM_Entity.SPM_Gas_Bill_LName; Pass_CA_Entity.BillngFname = SPM_Entity.SPM_Gas_Bill_FName;
                Pass_CA_Entity.Account = SPM_Entity.SPM_Gas_Account; Pass_CA_Entity.CA_Source = Source;
                if (((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() == ServiceCode)
                    Pass_CA_Entity.Elec_Other = "O";
            }

            Pass_CA_Entity.Lsct_Operator = BaseForm.UserID;

            //if (!string.IsNullOrEmpty(Source.Trim()))
            //{
            //    Pass_CA_Entity.Vendor_No = SPM_Entity.SPM_Gas_Vendor; Pass_CA_Entity.Cost = dr.Cells["gvtAmtPaid"].Value.ToString();
            //    Pass_CA_Entity.BillngType = SPM_Entity.SPM_Gas_BillName_Type; ////Pass_CA_Entity.Fund1 = Pass_CA_Entity.Fund1;
            //    Pass_CA_Entity.BillngLname = SPM_Entity.SPM_Gas_Bill_LName; Pass_CA_Entity.BillngFname = SPM_Entity.SPM_Gas_Bill_FName;
            //    Pass_CA_Entity.Account = SPM_Entity.SPM_Gas_Account;
            //}
            //else
            //{
            //    Pass_CA_Entity.Vendor_No = SPM_Entity.SPM_Vendor; Pass_CA_Entity.Cost = dr.Cells["gvtAmtPaid"].Value.ToString();
            //    Pass_CA_Entity.BillngType = SPM_Entity.SPM_BillName_Type; ////Pass_CA_Entity.Fund1 = Pass_CA_Entity.Fund1;
            //    Pass_CA_Entity.BillngLname = SPM_Entity.SPM_Gas_Bill_LName; Pass_CA_Entity.BillngFname = SPM_Entity.SPM_Gas_Bill_FName;
            //    Pass_CA_Entity.Account = SPM_Entity.SPM_Account;
            //}

            //Pass_CA_Entity.PaymentNo = Pass_CA_Entity.PaymentNo; Pass_CA_Entity.Check_No = Pass_CA_Entity.Check_No;
            //Pass_CA_Entity.Check_Date = Pass_CA_Entity.Check_Date;
            //Pass_CA_Entity.BundleNo = Pass_CA_Entity.BundleNo;
        }

        private void gvwMonthQuestions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (gvwMonthQuestions.Rows.Count > 0)
                {
                    //gvwMonthQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwMonthQuestions_CellValueChanged);

                    if (e.ColumnIndex == gvtAmtPaid.Index)
                    {
                        int introwindex = gvwMonthQuestions.CurrentCell.RowIndex;
                        //string strIntervalValue = Convert.ToString(gvwMonthQuestions.Rows[introwindex].Cells["Interval"].Value);
                        int intcolumnindex = gvwMonthQuestions.CurrentCell.ColumnIndex;
                        string strAmtValue = Convert.ToString(gvwMonthQuestions.Rows[introwindex].Cells["gvtAmtPaid"].Value);
                        gvwMonthQuestions.Rows[introwindex].Cells["gvtAmtPaid"].Selected = true;

                        if (!string.IsNullOrEmpty(strAmtValue))
                        {
                            if (CommonFunctions.IsNumeric(strAmtValue.Trim()))
                            {
                                if (Convert.ToDecimal(strAmtValue) < 1 && Convert.ToDecimal(strAmtValue) > 0)
                                {
                                }
                                else
                                {
                                    if (!System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.TwoDecimalString))
                                    {
                                        gvwMonthQuestions.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                        //AlertBox.Show(Consts.Messages.PleaseEnterDecimals);
                                        //InvokeFocusCommand(dataGridCaseIncome);
                                        //dataGridCaseIncome.BeginEdit(true);
                                        //dataGridCaseIncome.Rows[introwindex].Cells["Amt1"].Selected = true;
                                    }
                                    else
                                    {
                                        if (strAmtValue.Length > 6)
                                        {
                                            if (!System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.TwoDecimalRange6String))
                                            {
                                                gvwMonthQuestions.Rows[introwindex].Cells[intcolumnindex].Value = "999999.99";
                                                // CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals6Range);
                                            }
                                        }
                                        else
                                        {
                                            if (System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.NumericString))
                                            {
                                                gvwMonthQuestions.Rows[introwindex].Cells[intcolumnindex].Value = strAmtValue + ".00";
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.TwoDecimalString))
                                {
                                    gvwMonthQuestions.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                }
                            }
                            strAmtValue = Convert.ToString(gvwMonthQuestions.Rows[introwindex].Cells[intcolumnindex].Value);
                            if (!string.IsNullOrEmpty(strAmtValue.Trim()))
                            {
                                if (((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() == ServiceCode)
                                    CalculateAmounts(introwindex);
                            }
                        }

                    }
                    if (e.ColumnIndex == gvtServicedate.Index)
                    {
                        int introwindex = gvwMonthQuestions.CurrentCell.RowIndex;
                        int intcolumnindex = gvwMonthQuestions.CurrentCell.ColumnIndex;
                        string strIntervalValue = Convert.ToString(gvwMonthQuestions.Rows[introwindex].Cells["gvtServicedate"].Value);
                        string strCurrectValue = Convert.ToString(gvwMonthQuestions.Rows[introwindex].Cells["gvtServicedate"].Value);
                        if (strCurrectValue.Length > 10)
                            strCurrectValue = strCurrectValue.Substring(0, 10);

                        strCurrectValue = strCurrectValue.Replace("_", "").Trim();
                        strCurrectValue = strCurrectValue.Replace(" ", "").Trim();

                        if ((!string.IsNullOrEmpty(strCurrectValue)) && strCurrectValue.Trim() != "/  /")
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.DateFormatMMDDYYYY))
                            {
                                try
                                {

                                    if (DateTime.Parse(strCurrectValue) < Convert.ToDateTime("01/01/1800"))
                                    {
                                        AlertBox.Show("01/01/1800 below date not except", MessageBoxIcon.Warning);
                                        gvwMonthQuestions.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;

                                    }
                                    else
                                    {
                                        gvwMonthQuestions.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                        AlertBox.Show(Consts.Messages.PleaseEntermmddyyDateFormat, MessageBoxIcon.Warning);

                                    }

                                }
                                catch (Exception)
                                {
                                    gvwMonthQuestions.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                    AlertBox.Show(Consts.Messages.PleaseEntermmddyyDateFormat, MessageBoxIcon.Warning);

                                }


                            }
                            else
                            {
                                bool booldatevalid = true;
                                string IsLeap = "N";
                                if (DateTime.IsLeapYear(int.Parse(strCurrectValue.Substring(6, 4))))
                                    IsLeap = "Y";

                                if ((strCurrectValue.ToString().Substring(0, 2) == "02") && ((IsLeap == "N" && strCurrectValue.ToString().Substring(3, 2) == "29") || strCurrectValue.ToString().Substring(3, 2) == "30" || strCurrectValue.ToString().Substring(3, 2) == "31"))
                                {
                                    gvwMonthQuestions.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                    AlertBox.Show(Consts.Messages.PleaseEntermmddyyDateFormat, MessageBoxIcon.Warning);
                                    booldatevalid = false;
                                }

                                if (booldatevalid)
                                {
                                    if (((Utilities.ListItem)CmbSP.SelectedItem).Value.ToString().Trim() == ServiceCode)
                                    {
                                        if (Convert.ToDateTime(strCurrectValue.Trim()) >= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) && Convert.ToDateTime(strCurrectValue.Trim()) <= Convert.ToDateTime(CEAPCNTL_List[0].CPCT_PROG_END.Trim()))
                                        {
                                            if (gvwMonthQuestions.Rows[introwindex].Cells["gvCaseact"].Value.ToString() == "Y")
                                                gvwMonthQuestions.Rows[introwindex].DefaultCellStyle.BackColor = Color.White;
                                            else
                                                gvwMonthQuestions.Rows[introwindex].DefaultCellStyle.BackColor = Color.White;
                                        }
                                        else
                                        {
                                            gvwMonthQuestions.Rows[introwindex].DefaultCellStyle.BackColor = Color.Red;
                                            AlertBox.Show("The Service date must be between " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) + " and " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_END.Trim()));
                                            booldatevalid = false;
                                        }
                                    }
                                }
                            }
                        }

                    }
                    //gvwMonthQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwMonthQuestions_CellValueChanged);
                }
            }
            catch (Exception ex) { }
        }

        bool IsCal = false; bool isAlert = false;
        private void CalculateAmounts(int rowcalindex)
        {
            if (gvwMonthQuestions.Rows.Count > 0)
            {
                decimal ResAmount = 0; decimal Total = 0; decimal Bal = 0;
                if (!string.IsNullOrEmpty(txtAmount.Text.Trim())) Total = Convert.ToDecimal(txtAmount.Text.Trim());
                if (PropCaseactList.Count > 0)
                {
                    List<CASEACTEntity> caseactList = PropCaseactList.FindAll(u => u.Elec_Other != StrElec && u.Cost != "");
                    if (caseactList.Count > 0)
                        ResAmount = caseactList.Sum(x => Convert.ToDecimal(x.Cost.Trim()));
                }
                foreach (DataGridViewRow dr in gvwMonthQuestions.Rows)
                {
                    if (!string.IsNullOrEmpty(dr.Cells["gvtAmtPaid"].Value.ToString().Trim()))
                    {
                        ResAmount = ResAmount + Convert.ToDecimal(dr.Cells["gvtAmtPaid"].Value.ToString().Trim());
                    }
                }

                if (ResAmount > 0 && Total > 0)
                {
                    Bal = Total - ResAmount;
                    if (Bal < 0)
                    {
                        if (Convert.ToDecimal(txtBalance.Text.Trim()) > 0)
                        {
                            this.gvwMonthQuestions.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvwMonthQuestions_CellValueChanged);
                            gvwMonthQuestions.Rows[rowcalindex].Cells["gvtAmtPaid"].Value = txtBalance.Text;
                            txtBalance.Text = "0";
                            this.gvwMonthQuestions.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvwMonthQuestions_CellValueChanged);
                        }
                        else
                        {
                            this.gvwMonthQuestions.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvwMonthQuestions_CellValueChanged);
                            gvwMonthQuestions.Rows[rowcalindex].Cells["gvtAmtPaid"].Value = string.Empty;
                            gvwMonthQuestions.Rows[rowcalindex].Cells["gvkChk"].Value = false;
                            this.gvwMonthQuestions.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvwMonthQuestions_CellValueChanged);

                            AlertBox.Show("we can't allow the “Total Dollars Posted” to exceed the maximum award!!", MessageBoxIcon.Warning);
                            //IsFalse = false;
                            isAlert = true;
                        }
                    }
                    else
                    {
                        txtBalance.Text = Bal.ToString();
                        IsCal = true;
                    }
                }

            }
        }

        private void gvwMonthQuestions_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (gvwMonthQuestions.Rows.Count > 0)
            {
                //gvwMonthQuestions_CellValueChanged(sender, e);
            }
        }

        bool Vulner_Flag = false;
        bool Age_Grt_60 = false, Age_Les_6 = false, Disable_Flag = false, FoodStamps_Flag = false;

        private void gvwMonthQuestions_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private bool Get_SNP_Vulnerable_Status()
        {
            //bool Vulner_Flag = false;
            Vulner_Flag = false;
            DateTime MST_Intake_Date = DateTime.Today, SNP_DOB = DateTime.Today;
            DateTime zeroTime = new DateTime(1, 1, 1);
            TimeSpan Time_Span;
            int Age_In_years = 0;
            string Non_Qual_Alien_SW = "N";
            if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].IntakeDate.Trim()))
                MST_Intake_Date = Convert.ToDateTime(BaseForm.BaseCaseMstListEntity[0].IntakeDate);
            
            foreach (CaseSnpEntity Entity in BaseForm.BaseCaseSnpEntity)
            {
                SNP_DOB = MST_Intake_Date;
                if (!string.IsNullOrEmpty(Entity.AltBdate.Trim()))
                    SNP_DOB = Convert.ToDateTime(Entity.AltBdate);

                Age_In_years = 0;

                if (MST_Intake_Date > SNP_DOB)
                {
                    Time_Span = (MST_Intake_Date - SNP_DOB);
                    Age_In_years = (zeroTime + Time_Span).Year - 1;
                }
                //Age_In_years = (Time_Span). - 1;

                if (Age_In_years > 59)
                    Age_Grt_60 = true;

                if (Age_In_years < 6)
                    Age_Les_6 = true;

                if (Entity.Disable == "Y")
                    Disable_Flag = true;

                if (Entity.FootStamps == "Y")
                    FoodStamps_Flag = true;

                if (Entity.SsnReason == "Q" && BaseForm.BaseAgencyControlDetails.State == "TX") Non_Qual_Alien_SW = "Y";
            }

            string Tmp_Age_Dis = string.Empty;
            //if (Sel_Activity == "B")
            //{
            //    if (((ListItem)Cmb_Age_Dis.SelectedItem).Value.ToString() != null)
            //        Tmp_Age_Dis = ((ListItem)Cmb_Age_Dis.SelectedItem).Value.ToString();
            //}
            //else
            //    Tmp_Age_Dis = PassLIHEAPB_List[0].Age_dis;


            if ((Age_Grt_60 || Age_Les_6 || Disable_Flag) && Non_Qual_Alien_SW == "N")
            {
                //if (Tmp_Age_Dis == "1" || Tmp_Age_Dis == "2" || Tmp_Age_Dis == "3")
                Vulner_Flag = true;

                if (Age_Les_6)
                    Vulner_Flag = true;
            }

            return Vulner_Flag;
        }

    }
}