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
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using Captain.DatabaseLayer;
using XLSExportFile;
using static DevExpress.Xpo.DB.DataStoreLongrunnersWatch;
using Spire.Pdf.Graphics;
using DevExpress.Pdf;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.CodeParser;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class Casb2530Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion
        public Casb2530Form(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privileges;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            this.Text = /*privileges.Program + " - " +*/ privileges.PrivilegeName;
            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
            Agency = BaseForm.BaseAgency;
            Dept = BaseForm.BaseDept;
            Prog = BaseForm.BaseProg;
            Program_Year = BaseForm.BaseYear;
            propzipCodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(string.Empty, string.Empty, string.Empty, string.Empty);

            AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
            AdhocData AgyTabs = new AdhocData();
            propAgyTabsList = AgyTabs.Browse_AGYTABS(searchAgytabs);

            CustRespEntity SearchCustresp = new CustRespEntity(true);
            //SearchCustresp.ScrCode = "CASE2001";
            propCustResponceList = _model.FieldControls.Browse_CUSTRESP(SearchCustresp, "Browse");
            //SearchCustresp.ScrCode = "PREASSES";
            //propPresResponceList = _model.FieldControls.Browse_CUSTRESP(SearchCustresp, "Browse");

            propRankscategory = _model.SPAdminData.Browse_RankCtg();
            propReportPath = _model.lookupDataAccess.GetReportPath();
            fillDropdown(BaseForm.BaseAgency);
            List<CommonEntity> listSequence = _model.lookupDataAccess.GetSequence2530();
            foreach (CommonEntity item in listSequence)
            {
                cmbSequence.Items.Add(new Captain.Common.Utilities.ListItem(item.Desc, item.Code));
            }
            cmbSequence.SelectedIndex = 0;

            cmbSortOn.Items.Add(new Captain.Common.Utilities.ListItem("Description", "1"));
            cmbSortOn.Items.Add(new Captain.Common.Utilities.ListItem("Points(Ascending)", "2"));
            cmbSortOn.Items.Add(new Captain.Common.Utilities.ListItem("Points(Descending)", "3"));
            cmbSortOn.SelectedIndex = 0;
            if (Agency == "**" || Dept == "**" || Prog == "**")
            {
                rdoAllApplicant.Checked = true;
                rdoSelectApplicant_CheckedChanged(rdoSelectApplicant, new EventArgs());
                rdoSelectApplicant.Enabled = false;
            }
            else
            {
                rdoSelectApplicant.Enabled = true;
            }
        }


        #region properties

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public List<RankCatgEntity> propRankscategory { get; set; }
        public string propReportPath { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public List<CaseMstEntity> propCaseMstList { get; set; }
        public List<CaseSnpEntity> propCaseSnpList { get; set; }
        public List<ChldMstEntity> propChldMstEntity { get; set; }
        public List<ZipCodeEntity> propzipCodeEntity { get; set; }
        public List<CustomQuestionsEntity> propcustResponses { get; set; }
        public List<CustRespEntity> propCustResponceList { get; set; }
        public List<AGYTABSEntity> propAgyTabsList { get; set; }
        // public List<CustRespEntity> propPresResponceList { get; set; }
        #endregion



        #region RankCateogryPointsCalculation
        public CaseMstEntity propMstRank { get; set; }

        DataTable dtRankSubDetails;
        int intRankPoint = 0;
        private DataTable GetRankCategoryDetails(CaseMstEntity caseMst, List<CaseSnpEntity> caseSnp, ChldMstEntity chldMst, List<RNKCRIT2Entity> RnkQuesFledsEntity, List<RNKCRIT2Entity> RnkQuesFledsAllDataEntity, List<RNKCRIT2Entity> RnkCustFldsAllDataEntity)
        {
            try
            {

                string strResponceDesc = string.Empty;
                AGYTABSEntity agytabsMstDesc = null;
                intRankPoint = 0;
                dtRankSubDetails = new DataTable();
                dtRankSubDetails.Columns.Add("FieldCode", typeof(string));
                dtRankSubDetails.Columns.Add("FieldDesc", typeof(string));
                dtRankSubDetails.Columns.Add("Points", typeof(string));

                //
                // Here we add five DataRows.
                //        




                List<CommonEntity> ListRankPoints = new List<CommonEntity>();
                //for (int intRankCtg = 1; intRankCtg <= 6; intRankCtg++)
                //{ 

                //List<RNKCRIT2Entity> RnkQuesFledsDataEntity = RnkQuesFledsAllDataEntity.FindAll(u => u.RankCategory.Trim().ToString() == ((Captain.Common.Utilities.ListItem)cmbRankCategory.SelectedItem).Value.ToString());
                //List<RNKCRIT2Entity> RnkCustFldsDataEntity = RnkCustFldsAllDataEntity.FindAll(u => u.RankCategory.Trim().ToString() == ((Captain.Common.Utilities.ListItem)cmbRankCategory.SelectedItem).Value.ToString());

                List<RNKCRIT2Entity> RnkQuesFledsDataEntity = RnkQuesFledsAllDataEntity;
                List<RNKCRIT2Entity> RnkCustFldsDataEntity = RnkCustFldsAllDataEntity;

                List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
                List<CustomQuestionsEntity> custpresResponses = new List<CustomQuestionsEntity>();
                if (RnkCustFldsAllDataEntity.Count > 0)
                {
                    custResponses = _model.CaseMstData.GetCustomQuestionAnswersRank(caseMst.ApplAgency.ToString(), caseMst.ApplDept, caseMst.ApplProgram, caseMst.ApplYr, caseMst.ApplNo.ToString(), caseMst.FamilySeq.ToString(), string.Empty, string.Empty);
                    custpresResponses = _model.CaseMstData.GetPreassesQuestionAnswersRank(caseMst.ApplAgency.ToString(), caseMst.ApplDept, caseMst.ApplProgram, caseMst.ApplYr, caseMst.ApplNo.ToString(), caseMst.FamilySeq.ToString(), string.Empty, "PRESRESP");
                    //if (propcustResponses.Count > 0)
                    //  custResponses = propcustResponses.FindAll(u => (u.ACTAGENCY == caseMst.ApplAgency.ToString()) && (u.ACTDEPT == caseMst.ApplDept) && (u.ACTPROGRAM == caseMst.ApplProgram) && (u.ACTYEAR == caseMst.ApplYr) && (u.ACTAPPNO == caseMst.ApplNo.ToString()));
                }



                List<RNKCRIT2Entity> RnkQuesSearchList;
                propMstRank = caseMst;
                RNKCRIT2Entity RnkQuesSearch = null;
                // List<RNKCRIT2Entity> RnkQuesCaseSnp = null;
                int intRankSnpPoints = 0;
                string strApplicationcode = string.Empty;
                foreach (RNKCRIT2Entity rnkQuesData in RnkQuesFledsEntity)
                {

                    //Added by Sudheer on 11/04/24
                    if (rnkQuesData.RankFldDesc.Trim() == "Federal OMB")
                    {
                        if (dtRankSubDetails.Rows.Count > 0)
                        {
                            DataRow[] dromb = dtRankSubDetails.Select("FieldCode='Categorical Eligibility' ");
                            if (dromb.Length > 0)
                            {
                                DataTable dtra = dromb.CopyToDataTable();
                                if (dtra.Rows.Count > 0)
                                {
                                    if (dtra.Rows[0]["Points"].ToString() != "" && dtra.Rows[0]["Points"].ToString() != "0")
                                        continue;
                                }

                            }
                        }
                    }

                    intRankSnpPoints = 0;
                    DataRow dr = dtRankSubDetails.NewRow();
                    RnkQuesSearch = null;
                    dr["FieldCode"] = rnkQuesData.RankFldDesc.ToString();
                    switch (rnkQuesData.RankFldName.Trim())
                    {

                        case Consts.RankQues.MZip:
                            dr["FieldDesc"] = caseMst.Zip.Trim();
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Zip.Trim());
                            break;
                        case Consts.RankQues.MCounty:
                            if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            {
                                agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.County.Trim());
                                if (agytabsMstDesc != null)
                                {
                                    dr["FieldDesc"] = agytabsMstDesc.Code_Desc.ToString();
                                }
                                else
                                    dr["FieldDesc"] = string.Empty;
                            }
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.County.Trim());
                            break;
                        case Consts.RankQues.MLanguage:
                            if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            {
                                agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.Language.Trim());
                                if (agytabsMstDesc != null)
                                {
                                    dr["FieldDesc"] = agytabsMstDesc.Code_Desc.ToString();
                                }
                                else
                                    dr["FieldDesc"] = string.Empty;
                            }
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Language.Trim());
                            break;
                        case Consts.RankQues.MAlertCode:
                            dr["FieldDesc"] = caseMst.AlertCodes.Trim();
                            intRankSnpPoints = fillAlertIncomeCodes(caseMst.AlertCodes, RnkQuesFledsDataEntity, rnkQuesData.RankFldName.Trim());
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.MAboutUs:
                            if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            {
                                agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.AboutUs.Trim());
                                if (agytabsMstDesc != null)
                                {
                                    dr["FieldDesc"] = agytabsMstDesc.Code_Desc.ToString();
                                }
                                else
                                    dr["FieldDesc"] = string.Empty;
                            }
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.AboutUs.Trim());
                            break;
                        case Consts.RankQues.MAddressYear:
                            if (caseMst.AddressYears != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.AddressYears.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.AddressYears) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.AddressYears));
                            }
                            break;
                        case Consts.RankQues.MBestContact:
                            if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            {
                                AGYTABSEntity agytabsEntity = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.BestContact.Trim());
                                if (agytabsEntity != null)
                                {
                                    dr["FieldDesc"] = agytabsEntity.Code_Desc.Trim();
                                }
                                else
                                    dr["FieldDesc"] = string.Empty;
                            }
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.BestContact.Trim());
                            break;
                        case Consts.RankQues.MCaseReviewDate:
                            if (caseMst.CaseReviewDate != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.CaseReviewDate.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.CaseReviewDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.CaseReviewDate).Date);
                            }
                            break;
                        case Consts.RankQues.MCaseType:
                            agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.CaseType.Trim());
                            if (agytabsMstDesc != null)
                            {
                                dr["FieldDesc"] = agytabsMstDesc.Code_Desc.ToString();
                            }
                            else
                                dr["FieldDesc"] = string.Empty;
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.CaseType.Trim());
                            break;
                        case Consts.RankQues.MCmi:
                            if (caseMst.Cmi != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.Cmi.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Cmi) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Cmi));
                            }
                            break;
                        case Consts.RankQues.MEElectric:
                            if (caseMst.ExpElectric != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.ExpElectric.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpElectric) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpElectric));
                            }
                            break;
                        case Consts.RankQues.MEDEBTCC:
                            if (caseMst.Debtcc != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.Debtcc.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Debtcc) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Debtcc));
                            }
                            break;
                        case Consts.RankQues.MEDEBTLoans:
                            if (caseMst.DebtLoans != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.DebtLoans.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.DebtLoans) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.DebtLoans));
                            }
                            break;
                        case Consts.RankQues.MEDEBTMed:
                            if (caseMst.DebtMed != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.DebtMed.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.DebtMed) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.DebtMed));
                            }
                            break;
                        case Consts.RankQues.MEHeat:
                            if (caseMst.ExpHeat != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.ExpHeat.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpHeat) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpHeat));
                            }
                            break;
                        case Consts.RankQues.MEligDate:
                            if (caseMst.EligDate != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.EligDate.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.EligDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.EligDate).Date);
                            }
                            break;
                        case Consts.RankQues.MELiveExpenses:
                            if (caseMst.ExpLivexpense != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.ExpLivexpense.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpLivexpense) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpLivexpense));
                            }
                            //dr["FieldDesc"] = caseMst.ExpLivexpense.Trim();
                            //RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.ExpLivexpense.Trim());
                            break;
                        case Consts.RankQues.MERent:
                            if (caseMst.ExpRent != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.ExpRent.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpRent) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpRent));
                            }
                            break;
                        case Consts.RankQues.METotal:
                            if (caseMst.ExpTotal != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.ExpTotal.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpTotal) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpTotal));
                            }
                            break;
                        case Consts.RankQues.MEWater:
                            if (caseMst.ExpWater != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.ExpWater.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpWater) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpWater));
                            }
                            break;

                        case Consts.RankQues.MExpCaseworker:
                            dr["FieldDesc"] = caseMst.ExpCaseWorker.Trim();
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.ExpCaseWorker.Trim());
                            break;
                        case Consts.RankQues.MFamilyType:
                            agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.FamilyType.Trim());
                            if (agytabsMstDesc != null)
                            {
                                dr["FieldDesc"] = agytabsMstDesc.Code_Desc.ToString();
                            }
                            else
                                dr["FieldDesc"] = string.Empty;
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.FamilyType.Trim());
                            break;
                        case Consts.RankQues.MFamIncome:
                            if (caseMst.FamIncome != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.FamIncome.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.FamIncome) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.FamIncome));
                            }
                            break;
                        case Consts.RankQues.MHousing:
                            agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.Housing.Trim());
                            if (agytabsMstDesc != null)
                            {
                                dr["FieldDesc"] = agytabsMstDesc.Code_Desc.ToString();
                            }
                            else
                                dr["FieldDesc"] = string.Empty;
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Housing.Trim());
                            break;
                        case Consts.RankQues.MHud:
                            if (caseMst.Hud != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.Hud.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Hud) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Hud));
                            }
                            break;

                        case Consts.RankQues.MIncomeTypes:
                            dr["FieldDesc"] = caseMst.IncomeTypes.Trim();
                            intRankSnpPoints = fillAlertIncomeCodes(caseMst.IncomeTypes, RnkQuesFledsDataEntity, rnkQuesData.RankFldName.Trim());
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            //RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.IncomeTypes.Trim());
                            break;
                        case Consts.RankQues.NonCashBenefits:
                            dr["FieldDesc"] = caseMst.MstNCashBen.Trim();
                            intRankSnpPoints = fillAlertNonCashBenCodes(caseMst.MstNCashBen, RnkQuesFledsDataEntity, rnkQuesData.RankFldName.Trim());
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            //RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.IncomeTypes.Trim());
                            break;
                        case Consts.RankQues.MInitialDate:
                            if (caseMst.InitialDate != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.InitialDate.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.InitialDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.InitialDate).Date);
                            }
                            break;
                        case Consts.RankQues.MIntakeDate:
                            if (caseMst.IntakeDate != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.IntakeDate.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.IntakeDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.IntakeDate).Date);
                            }
                            break;
                        case Consts.RankQues.MIntakeWorker:
                            dr["FieldDesc"] = caseMst.IntakeWorker.Trim();
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.IntakeWorker.Trim());
                            break;
                        case Consts.RankQues.MJuvenile:
                            agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.Juvenile.Trim());
                            if (agytabsMstDesc != null)
                            {
                                dr["FieldDesc"] = agytabsMstDesc.Code_Desc.ToString();
                            }
                            else
                                dr["FieldDesc"] = string.Empty;
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Juvenile.Trim());
                            break;
                        case Consts.RankQues.MLanguageOt:
                            agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.LanguageOt.Trim());
                            if (agytabsMstDesc != null)
                            {
                                dr["FieldDesc"] = agytabsMstDesc.Code_Desc.ToString();
                            }
                            else
                                dr["FieldDesc"] = string.Empty;
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.LanguageOt.Trim());
                            break;
                        case Consts.RankQues.MNoInprog:
                            if (caseMst.NoInProg != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.NoInProg.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.NoInProg) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.NoInProg));
                            }
                            break;
                        case Consts.RankQues.Mpoverty:
                            if (caseMst.Poverty != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.Poverty.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Poverty) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Poverty));
                            }
                            break;
                        case Consts.RankQues.MProgIncome:
                            if (caseMst.ProgIncome != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.ProgIncome.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ProgIncome) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ProgIncome));
                            }
                            break;
                        case Consts.RankQues.MReverifyDate:
                            if (caseMst.ReverifyDate != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.ReverifyDate.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.ReverifyDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.ReverifyDate).Date);
                            }
                            break;
                        case Consts.RankQues.MSECRET:
                            agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.Secret.Trim());
                            if (agytabsMstDesc != null)
                            {
                                dr["FieldDesc"] = agytabsMstDesc.Code_Desc.ToString();
                            }
                            else
                                dr["FieldDesc"] = string.Empty;
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Secret.Trim());
                            break;
                        case Consts.RankQues.MSenior:
                            agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.Senior.Trim());
                            if (agytabsMstDesc != null)
                            {
                                dr["FieldDesc"] = agytabsMstDesc.Code_Desc.ToString();
                            }
                            else
                                dr["FieldDesc"] = string.Empty;
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Senior.Trim());
                            break;
                        case Consts.RankQues.MSite:
                            dr["FieldDesc"] = caseMst.Site.Trim();
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Site.Trim());
                            break;
                        case Consts.RankQues.MSMi:
                            if (caseMst.Smi != string.Empty)
                            {
                                dr["FieldDesc"] = caseMst.Smi.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Smi) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Smi));
                            }
                            break;
                        case Consts.RankQues.MVefiryCheckstub:
                            // {
                            agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.VerifyCheckStub.Trim());
                            if (agytabsMstDesc != null)
                            {
                                dr["FieldDesc"] = agytabsMstDesc.Code_Desc.ToString();
                            }
                            else
                                dr["FieldDesc"] = string.Empty;
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyCheckStub.Trim());
                            // }
                            break;
                        case Consts.RankQues.MVerifier:
                            //if (prophierarchyEntity.Count > 0)
                            //{
                            //    HierarchyEntity hierchy = prophierarchyEntity.Find(u => u.CaseWorker == caseMst.Verifier.Trim());
                            //    if (hierchy != null)
                            //    {
                            //        dr["FieldDesc"] = hierchy.HirarchyName.ToString();
                            //    }
                            //  else
                            dr["FieldDesc"] = caseMst.Verifier.Trim();

                            //}
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Verifier.Trim());
                            break;
                        case Consts.RankQues.MVerifyW2:
                            AGYTABSEntity agytabw2Entity = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.VerifyW2.Trim());
                            if (agytabw2Entity != null)
                            {
                                dr["FieldDesc"] = agytabw2Entity.Code_Desc.Trim();
                            }
                            else
                                dr["FieldDesc"] = string.Empty;
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyW2.Trim());
                            break;
                        case Consts.RankQues.MVeriTaxReturn:
                            agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.VerifyTaxReturn.Trim());
                            if (agytabsMstDesc != null)
                            {
                                dr["FieldDesc"] = agytabsMstDesc.Code_Desc.ToString();
                            }
                            else
                                dr["FieldDesc"] = string.Empty;
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyTaxReturn.Trim());
                            break;
                        case Consts.RankQues.MVerLetter:
                            agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.VerifyLetter.Trim());
                            if (agytabsMstDesc != null)
                            {
                                dr["FieldDesc"] = agytabsMstDesc.Code_Desc.ToString();
                            }
                            else
                                dr["FieldDesc"] = string.Empty;
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyLetter.Trim());
                            break;
                        case Consts.RankQues.MVerOther:
                            agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.VerifyOther.Trim());
                            if (agytabsMstDesc != null)
                            {
                                dr["FieldDesc"] = agytabsMstDesc.Code_Desc.ToString();
                            }
                            else
                                dr["FieldDesc"] = string.Empty;
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyOther.Trim());
                            break;
                        case Consts.RankQues.MWaitList:
                            dr["FieldDesc"] = caseMst.WaitList.Trim();
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.WaitList.Trim());
                            break;
                        case Consts.RankQues.MverifySelfDecl:
                            agytabw2Entity = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.VERIFY_SELF_DECL.Trim());
                            if (agytabw2Entity != null)
                            {
                                dr["FieldDesc"] = agytabw2Entity.Code_Desc.Trim();
                            }
                            else
                                dr["FieldDesc"] = string.Empty;
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VERIFY_SELF_DECL.Trim());
                            break;

                        case Consts.RankQues.CatElig:
                            if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            {
                                agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.CatElig.Trim());
                                if (agytabsMstDesc != null)
                                {
                                    dr["FieldDesc"] = agytabsMstDesc.Code_Desc.ToString();
                                }
                                else
                                    dr["FieldDesc"] = string.Empty;
                            }

                            //dr["FieldDesc"] = caseMst.CatElig.Trim();
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.CatElig.Trim());
                            break;

                        //Preassses Questuibs

                        //case Consts.RankQues.MPJOB:
                        //    agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.PJob.Trim());
                        //    if (agytabsMstDesc != null)
                        //    {
                        //        dr["FieldDesc"] = agytabsMstDesc.Code_Desc.Trim();
                        //    }
                        //    else
                        //        dr["FieldDesc"] = string.Empty;
                        //    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.PJob.Trim());
                        //    break;                       
                        //case Consts.RankQues.MPHsd:
                        //    agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.PHSD.Trim());
                        //    if (agytabsMstDesc != null)
                        //    {
                        //        dr["FieldDesc"] = agytabsMstDesc.Code_Desc.Trim();
                        //    }
                        //    else
                        //        dr["FieldDesc"] = string.Empty;
                        //    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.PHSD.Trim());
                        //    break;
                        //case Consts.RankQues.MPSkills:
                        //    agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.PSkills.Trim());
                        //    if (agytabsMstDesc != null)
                        //    {
                        //        dr["FieldDesc"] = agytabsMstDesc.Code_Desc.Trim();
                        //    }
                        //    else
                        //        dr["FieldDesc"] = string.Empty;
                        //    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.PSkills.Trim());
                        //    break;
                        //case Consts.RankQues.MPHousing:
                        //    agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.PHousing.Trim());
                        //    if (agytabsMstDesc != null)
                        //    {
                        //        dr["FieldDesc"] = agytabsMstDesc.Code_Desc.Trim();
                        //    }
                        //    else
                        //        dr["FieldDesc"] = string.Empty;
                        //    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.PHousing.Trim());
                        //    break;

                        //case Consts.RankQues.MPTransport:
                        //    agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.PTransport.Trim());
                        //    if (agytabsMstDesc != null)
                        //    {
                        //        dr["FieldDesc"] = agytabsMstDesc.Code_Desc.Trim();
                        //    }
                        //    else
                        //        dr["FieldDesc"] = string.Empty;
                        //    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.PTransport.Trim());
                        //    break;

                        //case Consts.RankQues.MPChildCare:
                        //    agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.PChldCare.Trim());
                        //    if (agytabsMstDesc != null)
                        //    {
                        //        dr["FieldDesc"] = agytabsMstDesc.Code_Desc.Trim();
                        //    }
                        //    else
                        //        dr["FieldDesc"] = string.Empty;
                        //    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.PChldCare.Trim());
                        //    break;
                        //case Consts.RankQues.MPCCEnrl:
                        //    agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.PCCENRL.Trim());
                        //    if (agytabsMstDesc != null)
                        //    {
                        //        dr["FieldDesc"] = agytabsMstDesc.Code_Desc.Trim();
                        //    }
                        //    else
                        //        dr["FieldDesc"] = string.Empty;
                        //    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.PCCENRL.Trim());
                        //    break;
                        //case Consts.RankQues.MPEldrcare:
                        //    agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.PELDCARE.Trim());
                        //    if (agytabsMstDesc != null)
                        //    {
                        //        dr["FieldDesc"] = agytabsMstDesc.Code_Desc.Trim();
                        //    }
                        //    else
                        //        dr["FieldDesc"] = string.Empty;
                        //    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.PELDCARE.Trim());
                        //    break;
                        //case Consts.RankQues.MPEcneed:
                        //    agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.PECNEED.Trim());
                        //    if (agytabsMstDesc != null)
                        //    {
                        //        dr["FieldDesc"] = agytabsMstDesc.Code_Desc.Trim();
                        //    }
                        //    else
                        //        dr["FieldDesc"] = string.Empty;
                        //    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.PECNEED.Trim());
                        //    break;
                        //case Consts.RankQues.MPChins:
                        //    agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.PECHINS.Trim());
                        //    if (agytabsMstDesc != null)
                        //    {
                        //        dr["FieldDesc"] = agytabsMstDesc.Code_Desc.Trim();
                        //    }
                        //    else
                        //        dr["FieldDesc"] = string.Empty;
                        //    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.PECHINS.Trim());
                        //    break;
                        //case Consts.RankQues.MPAhins:
                        //    agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.PAHINS.Trim());
                        //    if (agytabsMstDesc != null)
                        //    {
                        //        dr["FieldDesc"] = agytabsMstDesc.Code_Desc.Trim();
                        //    }
                        //    else
                        //        dr["FieldDesc"] = string.Empty;
                        //    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.PAHINS.Trim());
                        //    break;
                        //case Consts.RankQues.MPRWeng:
                        //    agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.PRWENG.Trim());
                        //    if (agytabsMstDesc != null)
                        //    {
                        //        dr["FieldDesc"] = agytabsMstDesc.Code_Desc.Trim();
                        //    }
                        //    else
                        //        dr["FieldDesc"] = string.Empty;
                        //    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.PRWENG.Trim());
                        //    break;
                        //case Consts.RankQues.MPCurrDss:
                        //    agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.PCURRDSS.Trim());
                        //    if (agytabsMstDesc != null)
                        //    {
                        //        dr["FieldDesc"] = agytabsMstDesc.Code_Desc.Trim();
                        //    }
                        //    else
                        //        dr["FieldDesc"] = string.Empty;
                        //    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.PCURRDSS.Trim());
                        //    break;
                        //case Consts.RankQues.MPRecvDss:
                        //    agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.PRECVDSS.Trim());
                        //    if (agytabsMstDesc != null)
                        //    {
                        //        dr["FieldDesc"] = agytabsMstDesc.Code_Desc.Trim();
                        //    }
                        //    else
                        //        dr["FieldDesc"] = string.Empty;
                        //    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.PRECVDSS.Trim());
                        //    break;



                        case Consts.RankQues.SEducation:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Education.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;
                            //List<string> SnpFieldsCodesList = new List<string>();
                            //List<string> SnpFieldsRelationList = new List<string>();
                            //for (int i = 0; i < caseSnp.Count; i++)
                            //{
                            //    SnpFieldsCodesList.Add(caseSnp[i].Education);
                            //    SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            //}
                            //intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.S1shift:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).IstShift.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;
                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.S2ndshift:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).IIndShift.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;
                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.S3rdShift:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).IIIrdShift.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;
                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SAge:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Age.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;
                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SAltBdate:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).AltBdate.ToString();
                            dr["FieldDesc"] = LookupDataAccess.Getdate(strApplicationcode);

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SDisable:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Disable.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SDrvlic:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Drvlic.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SEmployed:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Employed.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SEthinic:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Ethnic.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SExpireWorkDate:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).ExpireWorkDate.ToString();
                            dr["FieldDesc"] = LookupDataAccess.Getdate(strApplicationcode);

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SFarmer:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Farmer.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SFoodStamps:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).FootStamps.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SFThours:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).FullTimeHours.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SHealthIns:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).HealthIns.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SHireDate:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).HireDate.ToString();
                            dr["FieldDesc"] = LookupDataAccess.Getdate(strApplicationcode);

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SHourlyWage:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).HourlyWage.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SjobCategory:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).JobCategory.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SjobTitle:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).JobTitle.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SLastWorkDate:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).LastWorkDate.ToString();
                            dr["FieldDesc"] = LookupDataAccess.Getdate(strApplicationcode);

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SLegalTowork:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).LegalTowork.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SMartialStatus:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).MaritalStatus.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SMemberCode:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).MemberCode.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SNofcjob:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).NumberOfcjobs.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SNofljobs:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).NumberofLvjobs.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SPFrequency:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).PayFrequency.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SPregnant:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Pregnant.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SPThours:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).PartTimeHours.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SRace:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Race.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SRelitran:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Relitran.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SResident:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Resident.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SRshift:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).RShift.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;
                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SSchoolDistrict:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).SchoolDistrict.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;
                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SSEmploy:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Employed.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;
                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SSex:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Sex.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;
                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SSnpVet:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Vet.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;
                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SStatus:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Status.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;
                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.STranserv:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Transerv.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;
                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SWic:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Wic.ToString();

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.SworkLimit:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).WorkLimit.ToString();
                            //if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
                            //{
                            //    AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                            //    if (agytab != null)
                            //        dr["FieldDesc"] = agytab.Code_Desc.ToString();
                            //    else
                            //        dr["FieldDesc"] = strApplicationcode;
                            //}
                            //else
                            //    dr["FieldDesc"] = strApplicationcode;
                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.WorkStatus:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).WorkStatus.ToString();
                            
                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.DisconectYouth:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Youth.ToString();

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.MiltaryStatus:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).MilitaryStatus.ToString();

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.HealthCodes:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Health_Codes.ToString();

                            intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim(), out strResponceDesc);
                            dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, strResponceDesc);
                            intRankPoint = intRankPoint + intRankSnpPoints;
                            break;
                        case Consts.RankQues.CDentalCoverage:
                            if (chldMst != null)
                            {
                                dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, (chldMst.DentalCoverage.Trim() == "True" ? "1" : "0"));
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == (chldMst.DentalCoverage.Trim() == "True" ? "1" : "0"));
                            }
                            break;
                        case Consts.RankQues.CDiagNosisDate:
                            if (chldMst != null)
                                if (chldMst.DiagnosisDate != string.Empty)
                                {
                                    dr["FieldDesc"] = LookupDataAccess.Getdate(chldMst.DiagnosisDate);
                                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(chldMst.DiagnosisDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(chldMst.DiagnosisDate).Date);
                                }
                            break;
                        case Consts.RankQues.CDisability:
                            if (chldMst != null)
                            {
                                dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, (chldMst.Disability.Trim() == "True" ? "1" : "0"));
                                //dr["FieldDesc"] = chldMst.Disability.Trim() == "True" ? "Seleted" : "UnSelected";
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == (chldMst.Disability.Trim() == "True" ? "1" : "0"));
                            }
                            break;
                        case Consts.RankQues.CInsCat:
                            if (chldMst != null)
                            {
                                dr["FieldDesc"] = chldMst.InsCat.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == chldMst.InsCat.Trim());
                            }
                            break;
                        case Consts.RankQues.CMedCoverage:
                            if (chldMst != null)
                            {
                                // dr["FieldDesc"] = chldMst.MedCoverage.Trim() == "True" ? "Seleted" : "UnSelected";
                                dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, (chldMst.MedCoverage.Trim() == "True" ? "1" : "0"));
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == (chldMst.MedCoverage.Trim() == "True" ? "1" : "0"));
                            }
                            break;
                        case Consts.RankQues.CMedicalCoverageType:
                            if (chldMst != null)
                            {
                                dr["FieldDesc"] = chldMst.MedCoverType.Trim();
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == chldMst.MedCoverType.Trim());
                            }
                            break;


                    }

                    if (RnkQuesSearch != null)
                    {
                        intRankPoint = intRankPoint + Convert.ToInt32(RnkQuesSearch.Points);
                        dr["Points"] = RnkQuesSearch.Points;
                        dtRankSubDetails.Rows.Add(dr);
                    }
                    else
                    {
                        dr["Points"] = intRankSnpPoints;
                        dtRankSubDetails.Rows.Add(dr);
                    }
                    // }


                    //ListRankPoints.Add(new CommonEntity(intRankCtg.ToString(), intRankPoint.ToString()));
                }


                #region Preassess tab calculation
                if (custpresResponses.Count > 0)
                {
                    CustomQuestionsEntity custpresResponcesearch = null;
                    RNKCRIT2Entity rnkPoints = null;
                    string strQuestionType = string.Empty;
                    foreach (CustomQuestionsEntity responceQuestion in custpresResponses)
                    {
                        DataRow dr1 = dtRankSubDetails.NewRow();
                        List<RNKCRIT2Entity> rnkCustFldsFilterCode = RnkCustFldsDataEntity.FindAll(u => u.RankFiledCode.Trim() == responceQuestion.ACTCODE.Trim());

                        if (rnkCustFldsFilterCode.Count > 0)
                        {
                            custpresResponcesearch = null;
                            rnkPoints = null;
                            strQuestionType = rnkCustFldsFilterCode[0].RankFldRespType.Trim();
                            if (strQuestionType.ToString() != "C")
                            {
                                dr1["FieldCode"] = rnkCustFldsFilterCode[0].RankFldDesc.Trim();

                                switch (rnkCustFldsFilterCode[0].RankFldRespType.Trim())
                                {
                                    case "D":
                                    case "L":
                                        rnkPoints = rnkCustFldsFilterCode.Find(u => u.RespCd.Trim() == responceQuestion.ACTMULTRESP.Trim());
                                        //custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && u.ACTMULTRESP.Trim() == item.RespCd.Trim());
                                        CustRespEntity custrespent = propCustResponceList.Find(u => u.ResoCode.Trim() == responceQuestion.ACTCODE && u.DescCode == responceQuestion.ACTMULTRESP);
                                        if (custrespent != null)
                                            dr1["FieldDesc"] = custrespent.RespDesc.ToString();
                                        else
                                            dr1["FieldDesc"] = string.Empty;
                                        //dr1["FieldDesc"] = propCustResponceList.Find(u => u.ResoCode.Trim() == responceQuestion.ACTCODE && u.DescCode == responceQuestion.ACTMULTRESP).RespDesc.ToString();
                                        break;
                                    case "N":
                                        rnkPoints = rnkCustFldsFilterCode.Find(u => Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(responceQuestion.ACTNUMRESP) && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(responceQuestion.ACTNUMRESP));
                                        // custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && Convert.ToDecimal(u.GtNum) >= Convert.ToDecimal(item.ACTNUMRESP) && Convert.ToDecimal(u.LtNum) <= Convert.ToDecimal(item.ACTNUMRESP));
                                        dr1["FieldDesc"] = responceQuestion.ACTNUMRESP;
                                        break;
                                    case "T":
                                    case "B":
                                        rnkPoints = rnkCustFldsFilterCode.Find(u => Convert.ToDateTime(u.GtDate) <= Convert.ToDateTime(responceQuestion.ACTDATERESP) && Convert.ToDateTime(u.LtDate) >= Convert.ToDateTime(responceQuestion.ACTDATERESP));
                                        // custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && Convert.ToDateTime(u.GtDate) >= Convert.ToDateTime(item.ACTDATERESP) && Convert.ToDateTime(u.LtDate) <= Convert.ToDateTime(item.ACTNUMRESP));
                                        dr1["FieldDesc"] = responceQuestion.ACTDATERESP;
                                        break;
                                }
                                if (rnkPoints != null)
                                {
                                    dr1["Points"] = rnkPoints.Points;
                                    intRankPoint = intRankPoint + Convert.ToInt32(rnkPoints.Points);
                                }
                                else
                                {
                                    dr1["Points"] = "0";
                                }
                                dtRankSubDetails.Rows.Add(dr1);
                            }
                            else
                            {

                                var strresponcelist = responceQuestion.ACTALPHARESP.Split(',');
                                foreach (var item in strresponcelist)
                                {
                                    DataRow dr2 = dtRankSubDetails.NewRow();

                                    dr2["FieldCode"] = rnkCustFldsFilterCode[0].RankFldDesc.Trim();
                                    rnkPoints = rnkCustFldsFilterCode.Find(u => u.RespCd.Trim() == item);
                                    CustRespEntity custrespent = propCustResponceList.Find(u => u.ResoCode.Trim() == responceQuestion.ACTCODE && u.DescCode == item);
                                    if (custrespent != null)
                                        dr2["FieldDesc"] = custrespent.RespDesc.ToString();
                                    else
                                        dr2["FieldDesc"] = string.Empty;
                                    //dr2["FieldDesc"] = propCustResponceList.Find(u => u.ResoCode.Trim() == responceQuestion.ACTCODE && u.DescCode == item).RespDesc.ToString();

                                    if (rnkPoints != null)
                                    {
                                        dr2["Points"] = rnkPoints.Points;
                                        intRankPoint = intRankPoint + Convert.ToInt32(rnkPoints.Points);
                                    }
                                    else
                                    {
                                        dr2["Points"] = "0";
                                    }
                                    dtRankSubDetails.Rows.Add(dr2);
                                }

                            }

                        }

                    }
                }

                #endregion



                if (custResponses.Count > 0)
                {
                    CustomQuestionsEntity custResponcesearch = null;
                    RNKCRIT2Entity rnkPoints = null;
                    string strQuestionType = string.Empty;
                    foreach (CustomQuestionsEntity responceQuestion in custResponses)
                    {
                        DataRow dr1 = dtRankSubDetails.NewRow();
                        List<RNKCRIT2Entity> rnkCustFldsFilterCode = RnkCustFldsDataEntity.FindAll(u => u.RankFiledCode.Trim() == responceQuestion.ACTCODE.Trim());
                        if (rnkCustFldsFilterCode.Count > 0)
                        {

                            custResponcesearch = null;
                            rnkPoints = null;
                            strQuestionType = rnkCustFldsFilterCode[0].RankFldRespType.Trim();
                            if (strQuestionType.ToString() != "C")
                            {
                                dr1["FieldCode"] = rnkCustFldsFilterCode[0].RankFldDesc.Trim();

                                switch (rnkCustFldsFilterCode[0].RankFldRespType.Trim())
                                {
                                    case "D":
                                    case "L":
                                        rnkPoints = rnkCustFldsFilterCode.Find(u => u.RespCd.Trim() == responceQuestion.ACTMULTRESP.Trim());
                                        //custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && u.ACTMULTRESP.Trim() == item.RespCd.Trim());
                                        if(!string.IsNullOrEmpty(responceQuestion.ACTMULTRESP.Trim()))
                                            dr1["FieldDesc"] = propCustResponceList.Find(u => u.ResoCode.Trim() == responceQuestion.ACTCODE && u.DescCode == responceQuestion.ACTMULTRESP).RespDesc.ToString();
                                        else
                                            dr1["FieldDesc"] = string.Empty;
                                        break;
                                    case "N":
                                        rnkPoints = rnkCustFldsFilterCode.Find(u => Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(responceQuestion.ACTNUMRESP) && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(responceQuestion.ACTNUMRESP));
                                        // custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && Convert.ToDecimal(u.GtNum) >= Convert.ToDecimal(item.ACTNUMRESP) && Convert.ToDecimal(u.LtNum) <= Convert.ToDecimal(item.ACTNUMRESP));
                                        dr1["FieldDesc"] = responceQuestion.ACTNUMRESP;
                                        break;
                                    case "T":
                                    case "B":
                                        rnkPoints = rnkCustFldsFilterCode.Find(u => Convert.ToDateTime(u.GtDate) <= Convert.ToDateTime(responceQuestion.ACTDATERESP) && Convert.ToDateTime(u.LtDate) >= Convert.ToDateTime(responceQuestion.ACTDATERESP));
                                        // custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && Convert.ToDateTime(u.GtDate) >= Convert.ToDateTime(item.ACTDATERESP) && Convert.ToDateTime(u.LtDate) <= Convert.ToDateTime(item.ACTNUMRESP));
                                        dr1["FieldDesc"] = responceQuestion.ACTDATERESP;
                                        break;
                                }
                                if (rnkPoints != null)
                                {
                                    dr1["Points"] = rnkPoints.Points;
                                    intRankPoint = intRankPoint + Convert.ToInt32(rnkPoints.Points);
                                }
                                else
                                {
                                    dr1["Points"] = "0";
                                }
                                dtRankSubDetails.Rows.Add(dr1);
                            }
                            else
                            {

                                var strresponcelist = responceQuestion.ACTALPHARESP.Split(',');
                                foreach (var item in strresponcelist)
                                {
                                    DataRow dr2 = dtRankSubDetails.NewRow();

                                    dr2["FieldCode"] = rnkCustFldsFilterCode[0].RankFldDesc.Trim();
                                    rnkPoints = rnkCustFldsFilterCode.Find(u => u.RespCd.Trim() == item);
                                    dr2["FieldDesc"] = propCustResponceList.Find(u => u.ResoCode.Trim() == responceQuestion.ACTCODE && u.DescCode == item).RespDesc.ToString();

                                    if (rnkPoints != null)
                                    {
                                        dr2["Points"] = rnkPoints.Points;
                                        intRankPoint = intRankPoint + Convert.ToInt32(rnkPoints.Points);
                                    }
                                    else
                                    {
                                        dr2["Points"] = "0";
                                    }
                                    dtRankSubDetails.Rows.Add(dr2);
                                }

                            }
                        }





                        //foreach (RNKCRIT2Entity item in rnkCustFldsFilterCode)
                        //{

                        //    if (responceQuestion.ACTCODE.Trim() == item.RankFiledCode.Trim())
                        //    {
                        //        custResponcesearch = null;
                        //        strQuestionType = item.RankFldRespType.Trim();
                        //        dr1["FieldCode"] = item.RankFldDesc.Trim();

                        //        switch (item.RankFldRespType.Trim())
                        //        {
                        //            case "D":
                        //            case "L":
                        //                custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && u.ACTMULTRESP.Trim() == item.RespCd.Trim());
                        //                dr1["FieldDesc"] = propCustResponceList.Find(u => u.ResoCode.Trim() == responceQuestion.ACTCODE && u.DescCode == responceQuestion.ACTMULTRESP).RespDesc.ToString();
                        //                break;
                        //            case "N":
                        //                custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && Convert.ToDecimal(u.ACTNUMRESP) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(u.ACTNUMRESP) <= Convert.ToDecimal(item.LtNum));
                        //                dr1["FieldDesc"] = responceQuestion.ACTNUMRESP;
                        //                break;
                        //            case "T":
                        //            case "B":
                        //                custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && Convert.ToDateTime(u.ACTDATERESP) >= Convert.ToDateTime(item.GtDate) && Convert.ToDateTime(u.ACTNUMRESP) <= Convert.ToDateTime(item.LtDate));
                        //                dr1["FieldDesc"] = responceQuestion.ACTDATERESP;
                        //                break;
                        //        }
                        //        if (custResponcesearch != null)
                        //        {
                        //            dr1["Points"] = item.Points;
                        //            intRankPoint = intRankPoint + Convert.ToInt32(item.Points);
                        //        }
                        //        else
                        //        {
                        //            dr1["Points"] = "0";
                        //        }
                        //        dtRankSubDetails.Rows.Add(dr1);                                
                        //    }
                        //}

                    }
                }
            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }

            return dtRankSubDetails;
        }

        private int fillAlertIncomeCodes(string alertCodes, List<RNKCRIT2Entity> rnkSearchEntity, string FieldName)
        {
            int intAlertcode = 0;
            List<string> AlertList = new List<string>();
            if (alertCodes != null)
            {
                string[] incomeTypes = alertCodes.Split(' ');
                for (int i = 0; i < incomeTypes.Length; i++)
                {
                    AlertList.Add(incomeTypes.GetValue(i).ToString());
                }
            }
            List<RNKCRIT2Entity> RnkAlertCode = rnkSearchEntity.FindAll(u => u.RankFldName.Trim() == FieldName);

            foreach (RNKCRIT2Entity rnkEntity in RnkAlertCode)
            {
                if (alertCodes != null && AlertList.Contains(rnkEntity.RespCd))
                {
                    intAlertcode = intAlertcode + Convert.ToInt32(rnkEntity.Points);
                }
            }
            return intAlertcode;
        }

        private int fillAlertNonCashBenCodes(string alertCodes, List<RNKCRIT2Entity> rnkSearchEntity, string FieldName)
        {
            int intAlertcode = 0;
            List<string> AlertList = new List<string>();
            if (alertCodes != null)
            {
                string[] NonCashBen = alertCodes.Split(',');
                for (int i = 0; i < NonCashBen.Length; i++)
                {
                    AlertList.Add(NonCashBen.GetValue(i).ToString());
                }
            }
            List<RNKCRIT2Entity> RnkAlertCode = rnkSearchEntity.FindAll(u => u.RankFldName.Trim() == FieldName);

            foreach (RNKCRIT2Entity rnkEntity in RnkAlertCode)
            {
                if (alertCodes != null && AlertList.Contains(rnkEntity.RespCd))
                {
                    intAlertcode = intAlertcode + Convert.ToInt32(rnkEntity.Points);
                }
            }
            return intAlertcode;
        }

        private int fillAlertHealthCodes(string alertCodes, List<RNKCRIT2Entity> rnkSearchEntity, string FieldName,string CountInd)
        {
            int intAlertcode = 0;
            List<string> AlertList = new List<string>();
            if (alertCodes != null)
            {
                string[] HealthCodes = alertCodes.Split(',');
                for (int i = 0; i < HealthCodes.Length; i++)
                {
                    AlertList.Add(HealthCodes.GetValue(i).ToString());
                }
            }
            List<RNKCRIT2Entity> RnkAlertCode = rnkSearchEntity.FindAll(u => u.RankFldName.Trim() == FieldName && u.CountInd.Trim()==CountInd);

            foreach (RNKCRIT2Entity rnkEntity in RnkAlertCode)
            {
                if (alertCodes != null && AlertList.Contains(rnkEntity.RespCd))
                {
                    intAlertcode = intAlertcode + Convert.ToInt32(rnkEntity.Points);
                }
            }
            return intAlertcode;
        }


        private int CaseSnpDetailsCalc(List<RNKCRIT2Entity> rnkCaseSnp, List<CaseSnpEntity> caseSnpDetails, string strApplicantCode, string FilterCode, string ResponceType, out string strResponseDesc)
        {
            int intSnpPoints = 0; int HealthPoints=0;
            string strResponceCode = strApplicantCode;
            string strResponceData = strApplicantCode;
            List<CommonEntity> commonHighcount = new List<CommonEntity>();
            List<CommonEntity> commonLowcount = new List<CommonEntity>();
            commonHighcount.Clear();
            commonLowcount.Clear();
            foreach (RNKCRIT2Entity item in rnkCaseSnp)
            {
                List<RNKCRIT2Entity> RnkCrit2 = rnkCaseSnp.FindAll(u => u.RespCd.Equals(item.RespCd));
                HealthPoints = 0;
                if (item.CountInd.Trim() == "A")
                {
                    switch (ResponceType)
                    {
                        case "D":
                        case "L":
                            if (item.RespCd.Trim() == strApplicantCode)
                            {
                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                            }
                            if (FilterCode == Consts.RankQues.HealthCodes)
                            {
                                intSnpPoints = fillAlertHealthCodes(strApplicantCode, RnkCrit2, FilterCode.Trim(), item.CountInd.Trim());
                            }
                            break;
                        case "N":
                            if (strApplicantCode != string.Empty)
                                if (Convert.ToDecimal(strApplicantCode) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(strApplicantCode) <= Convert.ToDecimal(item.LtNum))
                                {
                                    intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                }
                            break;
                        case "G":
                            CaseSnpEntity casesnpAge = caseSnpDetails.Find(u => u.FamilySeq == propMstRank.FamilySeq);
                            if (casesnpAge != null)
                            {
                                if (casesnpAge.AltBdate != string.Empty && item.Relation.Trim() == casesnpAge.MemberCode)
                                {
                                    DateTime EndDate = GetEndDateAgeCalculation(item.AgeClcInd.Trim(), propMstRank);
                                    int AgeMonth = _model.lookupDataAccess.GetAgeCalculationMonths(Convert.ToDateTime(casesnpAge.AltBdate), EndDate);
                                    if (AgeMonth >= Convert.ToDecimal(item.GtNum) && AgeMonth <= Convert.ToDecimal(item.LtNum))
                                    {
                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                    }
                                    if (AgeMonth > 12)
                                    {
                                        strResponceData = (AgeMonth / 12).ToString();
                                    }
                                }
                            }
                            break;
                        case "B":
                        case "T":
                            if (strApplicantCode != string.Empty)
                                if (Convert.ToDateTime(strApplicantCode).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(strApplicantCode).Date <= Convert.ToDateTime(item.LtDate).Date)
                                {
                                    intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                }
                            break;

                    }

                }
                else if (item.CountInd.Trim() == "M")
                {
                    if (item.Relation == "*")
                    {
                        int count = 0;
                        switch (FilterCode)
                        {
                            case Consts.RankQues.S1shift:
                                count = caseSnpDetails.FindAll(u => u.IstShift.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.S2ndshift:
                                count = caseSnpDetails.FindAll(u => u.IIndShift.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.S3rdShift:
                                count = caseSnpDetails.FindAll(u => u.IIIrdShift.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SAge:
                                foreach (CaseSnpEntity snpDate in caseSnpDetails)
                                {
                                    if (snpDate.AltBdate != string.Empty)
                                    {
                                        DateTime EndDate = GetEndDateAgeCalculation(item.AgeClcInd.Trim(), propMstRank);
                                        int AgeMonth = _model.lookupDataAccess.GetAgeCalculationMonths(Convert.ToDateTime(snpDate.AltBdate), EndDate);
                                        if (AgeMonth >= Convert.ToDecimal(item.GtNum) && AgeMonth <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                    }
                                }
                                break;
                            case Consts.RankQues.SAltBdate:
                                foreach (CaseSnpEntity snpDate in caseSnpDetails)
                                {
                                    if (snpDate.AltBdate != string.Empty)
                                        if (Convert.ToDateTime(snpDate.AltBdate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.AltBdate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                        {
                                            count = count + 1;
                                        }
                                }

                                break;
                            case Consts.RankQues.SSchoolDistrict:
                                count = caseSnpDetails.FindAll(u => u.SchoolDistrict.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SEducation:
                                count = caseSnpDetails.FindAll(u => u.Education.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SWic:
                                count = caseSnpDetails.FindAll(u => u.Wic.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SDisable:
                                count = caseSnpDetails.FindAll(u => u.Disable.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SDrvlic:
                                count = caseSnpDetails.FindAll(u => u.Drvlic.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SEmployed:
                                count = caseSnpDetails.FindAll(u => u.Employed.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SEthinic:
                                count = caseSnpDetails.FindAll(u => u.Ethnic.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SExpireWorkDate:
                                foreach (CaseSnpEntity snpDate in caseSnpDetails)
                                {
                                    if (snpDate.ExpireWorkDate != string.Empty)
                                        if (Convert.ToDateTime(snpDate.ExpireWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.ExpireWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SFarmer:
                                count = caseSnpDetails.FindAll(u => u.Farmer.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SFoodStamps:
                                count = caseSnpDetails.FindAll(u => u.FootStamps.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.WorkStatus:
                                count = caseSnpDetails.FindAll(u => u.WorkStatus.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.DisconectYouth:
                                count = caseSnpDetails.FindAll(u => u.Youth.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.MiltaryStatus:
                                count = caseSnpDetails.FindAll(u => u.MilitaryStatus.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.HealthCodes:
                                foreach (CaseSnpEntity snpHealth in caseSnpDetails)
                                {
                                    if (!string.IsNullOrEmpty(snpHealth.Health_Codes.Trim()))
                                    {
                                        int intRankSnpPoints = fillAlertHealthCodes(snpHealth.Health_Codes.Trim(), RnkCrit2, FilterCode.Trim(),item.CountInd.Trim());
                                        HealthPoints = HealthPoints + intRankSnpPoints;
                                        //count = intRankSnpPoints + Convert.ToInt32(item.Points);

                                        if(intRankSnpPoints>0)
                                            count = count + 1;
                                    }
                                    
                                }
                                if (count == caseSnpDetails.Count)
                                { intSnpPoints = intSnpPoints + HealthPoints; count++; }
                                break;
                            case Consts.RankQues.SFThours:
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {
                                    if (snpNumeric.FullTimeHours != string.Empty)
                                        if (Convert.ToDecimal(snpNumeric.FullTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.FullTimeHours) <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SHealthIns:
                                count = caseSnpDetails.FindAll(u => u.HealthIns.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SHireDate:
                                foreach (CaseSnpEntity snpDate in caseSnpDetails)
                                {
                                    if (snpDate.HireDate != string.Empty)
                                        if (Convert.ToDateTime(snpDate.HireDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.HireDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SHourlyWage:
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {
                                    if (snpNumeric.HourlyWage != string.Empty)
                                        if (Convert.ToDecimal(snpNumeric.HourlyWage) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.HourlyWage) <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SjobCategory:
                                count = caseSnpDetails.FindAll(u => u.JobCategory.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SjobTitle:
                                count = caseSnpDetails.FindAll(u => u.JobTitle.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SLastWorkDate:
                                foreach (CaseSnpEntity snpDate in caseSnpDetails)
                                {
                                    if (snpDate.LastWorkDate != string.Empty)
                                        if (Convert.ToDateTime(snpDate.LastWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.LastWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SLegalTowork:
                                count = caseSnpDetails.FindAll(u => u.LegalTowork.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SMartialStatus:
                                count = caseSnpDetails.FindAll(u => u.MaritalStatus.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SMemberCode:
                                count = caseSnpDetails.FindAll(u => u.MemberCode.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SNofcjob:
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {
                                    if (snpNumeric.NumberOfcjobs != string.Empty)
                                        if (Convert.ToDecimal(snpNumeric.NumberOfcjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberOfcjobs) <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SNofljobs:
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {
                                    if (snpNumeric.NumberofLvjobs != string.Empty)
                                        if (Convert.ToDecimal(snpNumeric.NumberofLvjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberofLvjobs) <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SPFrequency:
                                count = caseSnpDetails.FindAll(u => u.PayFrequency.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SPregnant:
                                count = caseSnpDetails.FindAll(u => u.Pregnant.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SPThours:
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {
                                    if (snpNumeric.PartTimeHours != string.Empty)
                                        if (Convert.ToDecimal(snpNumeric.PartTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.PartTimeHours) <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SRace:
                                count = caseSnpDetails.FindAll(u => u.Race.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SRelitran:
                                count = caseSnpDetails.FindAll(u => u.Relitran.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SResident:
                                count = caseSnpDetails.FindAll(u => u.Resident.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SRshift:
                                count = caseSnpDetails.FindAll(u => u.RShift.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SSEmploy:
                                count = caseSnpDetails.FindAll(u => u.SeasonalEmploy.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SSex:
                                count = caseSnpDetails.FindAll(u => u.Sex.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SSnpVet:
                                count = caseSnpDetails.FindAll(u => u.Vet.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SStatus:
                                count = caseSnpDetails.FindAll(u => u.Status.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.STranserv:
                                count = caseSnpDetails.FindAll(u => u.Transerv.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SworkLimit:
                                count = caseSnpDetails.FindAll(u => u.WorkLimit.Trim().Equals(item.RespCd)).Count;
                                break;

                        }

                        if (caseSnpDetails.Count == count)
                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                    }
                    else
                    {
                        switch (ResponceType)
                        {
                            case "D":
                            case "L":
                                foreach (CaseSnpEntity snpdropdown in caseSnpDetails)
                                {

                                    switch (FilterCode)
                                    {
                                        case Consts.RankQues.S1shift:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.IstShift.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.S2ndshift:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.IIndShift.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.S3rdShift:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.IIIrdShift.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SSchoolDistrict:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.SchoolDistrict.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SEducation:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Education.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SWic:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Wic.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SDisable:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Disable.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SDrvlic:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Drvlic.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SEmployed:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Employed.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SEthinic:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Ethnic.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SFarmer:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Farmer.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SFoodStamps:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.FootStamps.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.WorkStatus:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.WorkStatus.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.DisconectYouth:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Youth.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.MiltaryStatus:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.MilitaryStatus.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.HealthCodes:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode)
                                            {
                                                int intRankSnpPoints = fillAlertHealthCodes(snpdropdown.Health_Codes.Trim(), RnkCrit2, FilterCode.Trim(), item.CountInd.Trim());
                                                intSnpPoints = intSnpPoints + intRankSnpPoints;
                                                
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SHealthIns:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.HealthIns.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SjobCategory:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.JobCategory.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SjobTitle:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.JobTitle.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SLegalTowork:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.LegalTowork.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SMartialStatus:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.MaritalStatus.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SMemberCode:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.MemberCode.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SPFrequency:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.PayFrequency.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SPregnant:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Pregnant.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SRace:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Race.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SRelitran:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Relitran.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SResident:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Resident.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SRshift:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.RShift.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SSEmploy:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.SeasonalEmploy.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SSex:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Sex.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SSnpVet:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Vet.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SStatus:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Status.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.STranserv:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Transerv.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SworkLimit:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.WorkLimit.Trim())
                                            {
                                                intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                    }
                                }
                                //if (listRelationstring.Contains(item.Relation))
                                //{
                                //    if (listCodestring.Contains(item.RespCd))
                                //    {
                                //        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                //    }
                                //}
                                break;
                            case "N":
                            case "G":
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {

                                    switch (FilterCode)
                                    {
                                        case Consts.RankQues.SAge:
                                            if (snpNumeric.AltBdate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                            {
                                                DateTime EndDate = GetEndDateAgeCalculation(item.AgeClcInd.Trim(), propMstRank);
                                                int AgeMonth = _model.lookupDataAccess.GetAgeCalculationMonths(Convert.ToDateTime(snpNumeric.AltBdate), EndDate);
                                                if (AgeMonth >= Convert.ToDecimal(item.GtNum) && AgeMonth <= Convert.ToDecimal(item.LtNum))
                                                {
                                                    intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                                if (AgeMonth > 12)
                                                {
                                                    strResponceData = (AgeMonth / 12).ToString();
                                                }
                                            }
                                            break;

                                        case Consts.RankQues.SNofcjob:
                                            if (snpNumeric.NumberOfcjobs != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDecimal(snpNumeric.NumberOfcjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberOfcjobs) <= Convert.ToDecimal(item.LtNum))
                                                {
                                                    intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SNofljobs:
                                            if (snpNumeric.NumberofLvjobs != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDecimal(snpNumeric.NumberofLvjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberofLvjobs) <= Convert.ToDecimal(item.LtNum))
                                                {
                                                    intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SFThours:
                                            if (snpNumeric.FullTimeHours != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDecimal(snpNumeric.FullTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.FullTimeHours) <= Convert.ToDecimal(item.LtNum))
                                                {
                                                    intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SPThours:
                                            if (snpNumeric.PartTimeHours != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDecimal(snpNumeric.PartTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.PartTimeHours) <= Convert.ToDecimal(item.LtNum))
                                                {
                                                    intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SHourlyWage:
                                            if (snpNumeric.HourlyWage != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDecimal(snpNumeric.HourlyWage) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.HourlyWage) <= Convert.ToDecimal(item.LtNum))
                                                {
                                                    intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;

                                    }
                                }
                                break;
                            case "B":
                            case "T":
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {

                                    switch (FilterCode)
                                    {
                                        case Consts.RankQues.SAltBdate:
                                            if (snpNumeric.AltBdate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDateTime(snpNumeric.AltBdate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.AltBdate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                                {
                                                    intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SExpireWorkDate:
                                            if (snpNumeric.ExpireWorkDate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDateTime(snpNumeric.ExpireWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.ExpireWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                                {
                                                    intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SLastWorkDate:
                                            if (snpNumeric.LastWorkDate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDateTime(snpNumeric.LastWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.LastWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                                {
                                                    intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SHireDate:
                                            if (snpNumeric.HireDate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDateTime(snpNumeric.HireDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.HireDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                                {
                                                    intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;


                                    }
                                }
                                break;

                        }


                    }

                }
                else if (item.CountInd.Trim() == "H")
                {
                    if (item.Relation == "*")
                    {
                        int count = 0;
                        switch (FilterCode)
                        {
                            case Consts.RankQues.S1shift:
                                count = caseSnpDetails.FindAll(u => u.IstShift.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.S2ndshift:
                                count = caseSnpDetails.FindAll(u => u.IIndShift.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.S3rdShift:
                                count = caseSnpDetails.FindAll(u => u.IIIrdShift.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SAge:
                                foreach (CaseSnpEntity snpDate in caseSnpDetails)
                                {
                                    if (snpDate.AltBdate != string.Empty)
                                    {
                                        DateTime EndDate = GetEndDateAgeCalculation(item.AgeClcInd.Trim(), propMstRank);
                                        int AgeMonth = _model.lookupDataAccess.GetAgeCalculationMonths(Convert.ToDateTime(snpDate.AltBdate), EndDate);
                                        if (AgeMonth >= Convert.ToDecimal(item.GtNum) && AgeMonth <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                    }
                                }
                                break;
                            case Consts.RankQues.SAltBdate:
                                foreach (CaseSnpEntity snpDate in caseSnpDetails)
                                {
                                    if (snpDate.AltBdate != string.Empty)
                                        if (Convert.ToDateTime(snpDate.AltBdate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.AltBdate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                        {
                                            count = count + 1;
                                        }
                                }

                                break;
                            case Consts.RankQues.SSchoolDistrict:
                                count = caseSnpDetails.FindAll(u => u.SchoolDistrict.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SEducation:
                                count = caseSnpDetails.FindAll(u => u.Education.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SWic:
                                count = caseSnpDetails.FindAll(u => u.Wic.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SDisable:
                                count = caseSnpDetails.FindAll(u => u.Disable.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SDrvlic:
                                count = caseSnpDetails.FindAll(u => u.Drvlic.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SEmployed:
                                count = caseSnpDetails.FindAll(u => u.Employed.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SEthinic:
                                count = caseSnpDetails.FindAll(u => u.Ethnic.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SExpireWorkDate:
                                foreach (CaseSnpEntity snpDate in caseSnpDetails)
                                {
                                    if (snpDate.ExpireWorkDate != string.Empty)
                                        if (Convert.ToDateTime(snpDate.ExpireWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.ExpireWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SFarmer:
                                count = caseSnpDetails.FindAll(u => u.Farmer.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SFoodStamps:
                                count = caseSnpDetails.FindAll(u => u.FootStamps.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.WorkStatus:
                                count = caseSnpDetails.FindAll(u => u.WorkStatus.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.DisconectYouth:
                                count = caseSnpDetails.FindAll(u => u.Youth.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.MiltaryStatus:
                                count = caseSnpDetails.FindAll(u => u.MilitaryStatus.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.HealthCodes:
                                foreach (CaseSnpEntity snpHealth in caseSnpDetails)
                                {
                                    if (!string.IsNullOrEmpty(snpHealth.Health_Codes.Trim()))
                                    {
                                        int intRankSnpPoints = fillAlertHealthCodes(snpHealth.Health_Codes.Trim(), RnkCrit2, FilterCode.Trim(), item.CountInd.Trim());
                                        HealthPoints = HealthPoints + intRankSnpPoints;
                                        //count = intRankSnpPoints + Convert.ToInt32(item.Points);
                                    }
                                    count = count + 1;
                                }
                                if (caseSnpDetails.Count == count)
                                {
                                    commonHighcount.Add(new CommonEntity(HealthPoints.ToString(), item.RespCd));
                                    count++;
                                }

                                break;
                            case Consts.RankQues.SFThours:
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {
                                    if (snpNumeric.FullTimeHours != string.Empty)
                                        if (Convert.ToDecimal(snpNumeric.FullTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.FullTimeHours) <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SHealthIns:
                                count = caseSnpDetails.FindAll(u => u.HealthIns.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SHireDate:
                                foreach (CaseSnpEntity snpDate in caseSnpDetails)
                                {
                                    if (snpDate.HireDate != string.Empty)
                                        if (Convert.ToDateTime(snpDate.HireDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.HireDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SHourlyWage:
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {
                                    if (snpNumeric.HourlyWage != string.Empty)
                                        if (Convert.ToDecimal(snpNumeric.HourlyWage) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.HourlyWage) <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SjobCategory:
                                count = caseSnpDetails.FindAll(u => u.JobCategory.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SjobTitle:
                                count = caseSnpDetails.FindAll(u => u.JobTitle.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SLastWorkDate:
                                foreach (CaseSnpEntity snpDate in caseSnpDetails)
                                {
                                    if (snpDate.LastWorkDate != string.Empty)
                                        if (Convert.ToDateTime(snpDate.LastWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.LastWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SLegalTowork:
                                count = caseSnpDetails.FindAll(u => u.LegalTowork.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SMartialStatus:
                                count = caseSnpDetails.FindAll(u => u.MaritalStatus.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SMemberCode:
                                count = caseSnpDetails.FindAll(u => u.MemberCode.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SNofcjob:
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {
                                    if (snpNumeric.NumberOfcjobs != string.Empty)
                                        if (Convert.ToDecimal(snpNumeric.NumberOfcjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberOfcjobs) <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SNofljobs:
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {
                                    if (snpNumeric.NumberofLvjobs != string.Empty)
                                        if (Convert.ToDecimal(snpNumeric.NumberofLvjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberofLvjobs) <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SPFrequency:
                                count = caseSnpDetails.FindAll(u => u.PayFrequency.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SPregnant:
                                count = caseSnpDetails.FindAll(u => u.Pregnant.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SPThours:
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {
                                    if (snpNumeric.PartTimeHours != string.Empty)
                                        if (Convert.ToDecimal(snpNumeric.PartTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.PartTimeHours) <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SRace:
                                count = caseSnpDetails.FindAll(u => u.Race.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SRelitran:
                                count = caseSnpDetails.FindAll(u => u.Relitran.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SResident:
                                count = caseSnpDetails.FindAll(u => u.Resident.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SRshift:
                                count = caseSnpDetails.FindAll(u => u.RShift.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SSEmploy:
                                count = caseSnpDetails.FindAll(u => u.SeasonalEmploy.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SSex:
                                count = caseSnpDetails.FindAll(u => u.Sex.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SSnpVet:
                                count = caseSnpDetails.FindAll(u => u.Vet.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SStatus:
                                count = caseSnpDetails.FindAll(u => u.Status.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.STranserv:
                                count = caseSnpDetails.FindAll(u => u.Transerv.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SworkLimit:
                                count = caseSnpDetails.FindAll(u => u.WorkLimit.Trim().Equals(item.RespCd)).Count;
                                break;

                        }

                        if (caseSnpDetails.Count == count)
                        {
                            commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                            // intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                        }
                    }
                    else
                    {
                        switch (ResponceType)
                        {
                            case "D":
                            case "L":
                                foreach (CaseSnpEntity snpdropdown in caseSnpDetails)
                                {

                                    switch (FilterCode)
                                    {
                                        case Consts.RankQues.S1shift:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.IstShift.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.S2ndshift:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.IIndShift.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.S3rdShift:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.IIIrdShift.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SSchoolDistrict:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.SchoolDistrict.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SEducation:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Education.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SWic:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Wic.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SDisable:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Disable.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SDrvlic:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Drvlic.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SEmployed:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Employed.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SEthinic:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Ethnic.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SFarmer:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Farmer.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SFoodStamps:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.FootStamps.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.WorkStatus:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.WorkStatus.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.DisconectYouth:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Youth.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.MiltaryStatus:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.MilitaryStatus.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.HealthCodes:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode)
                                            {
                                                int intRankSnpPoints = fillAlertHealthCodes(snpdropdown.Health_Codes.Trim(), RnkCrit2, FilterCode.Trim(),item.CountInd.Trim());
                                                //intRankPoint = intRankPoint + intRankSnpPoints;
                                                commonHighcount.Add(new CommonEntity(intRankSnpPoints.ToString(), item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                //strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SHealthIns:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.HealthIns.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SjobCategory:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.JobCategory.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SjobTitle:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.JobTitle.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SLegalTowork:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.LegalTowork.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SMartialStatus:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.MaritalStatus.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SMemberCode:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.MemberCode.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SPFrequency:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.PayFrequency.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SPregnant:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Pregnant.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SRace:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Race.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SRelitran:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Relitran.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SResident:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Resident.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SRshift:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.RShift.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SSEmploy:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.SeasonalEmploy.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SSex:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Sex.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SSnpVet:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Vet.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SStatus:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Status.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.STranserv:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Transerv.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SworkLimit:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.WorkLimit.Trim())
                                            {
                                                commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                    }
                                }
                                //if (listRelationstring.Contains(item.Relation))
                                //{
                                //    if (listCodestring.Contains(item.RespCd))
                                //    {
                                //        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                //    }
                                //}
                                break;
                            case "N":
                            case "G":
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {

                                    switch (FilterCode)
                                    {
                                        case Consts.RankQues.SAge:
                                            if (snpNumeric.AltBdate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                            {
                                                DateTime EndDate = GetEndDateAgeCalculation(item.AgeClcInd.Trim(), propMstRank);
                                                int AgeMonth = _model.lookupDataAccess.GetAgeCalculationMonths(Convert.ToDateTime(snpNumeric.AltBdate), EndDate);
                                                if (AgeMonth >= Convert.ToDecimal(item.GtNum) && AgeMonth <= Convert.ToDecimal(item.LtNum))
                                                {
                                                    // intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                    commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));

                                                }
                                                if (AgeMonth > 12)
                                                {
                                                    strResponceData = (AgeMonth / 12).ToString();
                                                }
                                            }
                                            break;

                                        case Consts.RankQues.SNofcjob:
                                            if (snpNumeric.NumberOfcjobs != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDecimal(snpNumeric.NumberOfcjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberOfcjobs) <= Convert.ToDecimal(item.LtNum))
                                                {
                                                    commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                    //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SNofljobs:
                                            if (snpNumeric.NumberofLvjobs != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDecimal(snpNumeric.NumberofLvjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberofLvjobs) <= Convert.ToDecimal(item.LtNum))
                                                {

                                                    commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                    //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SFThours:
                                            if (snpNumeric.FullTimeHours != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDecimal(snpNumeric.FullTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.FullTimeHours) <= Convert.ToDecimal(item.LtNum))
                                                {

                                                    commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                    //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SPThours:
                                            if (snpNumeric.PartTimeHours != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDecimal(snpNumeric.PartTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.PartTimeHours) <= Convert.ToDecimal(item.LtNum))
                                                {

                                                    commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                    //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SHourlyWage:
                                            if (snpNumeric.HourlyWage != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDecimal(snpNumeric.HourlyWage) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.HourlyWage) <= Convert.ToDecimal(item.LtNum))
                                                {

                                                    commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                    //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;

                                    }
                                }
                                break;
                            case "B":
                            case "T":
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {

                                    switch (FilterCode)
                                    {
                                        case Consts.RankQues.SAltBdate:
                                            if (snpNumeric.AltBdate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDateTime(snpNumeric.AltBdate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.AltBdate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                                {

                                                    commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                    //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SExpireWorkDate:
                                            if (snpNumeric.ExpireWorkDate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDateTime(snpNumeric.ExpireWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.ExpireWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                                {

                                                    commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                    //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SLastWorkDate:
                                            if (snpNumeric.LastWorkDate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDateTime(snpNumeric.LastWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.LastWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                                {

                                                    commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                    //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SHireDate:
                                            if (snpNumeric.HireDate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDateTime(snpNumeric.HireDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.HireDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                                {

                                                    commonHighcount.Add(new CommonEntity(item.Points, item.RespCd));
                                                    //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;


                                    }
                                }
                                break;

                        }


                    }

                }
                //Lowest Points
                else if (item.CountInd.Trim() == "L")
                {
                    if (item.Relation == "*")
                    {
                        int count = 0;
                        switch (FilterCode)
                        {
                            case Consts.RankQues.S1shift:
                                count = caseSnpDetails.FindAll(u => u.IstShift.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.S2ndshift:
                                count = caseSnpDetails.FindAll(u => u.IIndShift.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.S3rdShift:
                                count = caseSnpDetails.FindAll(u => u.IIIrdShift.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SAge:
                                foreach (CaseSnpEntity snpDate in caseSnpDetails)
                                {
                                    if (snpDate.AltBdate != string.Empty)
                                    {
                                        DateTime EndDate = GetEndDateAgeCalculation(item.AgeClcInd.Trim(), propMstRank);
                                        int AgeMonth = _model.lookupDataAccess.GetAgeCalculationMonths(Convert.ToDateTime(snpDate.AltBdate), EndDate);
                                        if (AgeMonth >= Convert.ToDecimal(item.GtNum) && AgeMonth <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                    }
                                }
                                break;
                            case Consts.RankQues.SAltBdate:
                                foreach (CaseSnpEntity snpDate in caseSnpDetails)
                                {
                                    if (snpDate.AltBdate != string.Empty)
                                        if (Convert.ToDateTime(snpDate.AltBdate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.AltBdate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                        {
                                            count = count + 1;
                                        }
                                }

                                break;
                            case Consts.RankQues.SSchoolDistrict:
                                count = caseSnpDetails.FindAll(u => u.SchoolDistrict.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SEducation:
                                count = caseSnpDetails.FindAll(u => u.Education.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SWic:
                                count = caseSnpDetails.FindAll(u => u.Wic.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SDisable:
                                count = caseSnpDetails.FindAll(u => u.Disable.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SDrvlic:
                                count = caseSnpDetails.FindAll(u => u.Drvlic.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SEmployed:
                                count = caseSnpDetails.FindAll(u => u.Employed.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SEthinic:
                                count = caseSnpDetails.FindAll(u => u.Ethnic.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SExpireWorkDate:
                                foreach (CaseSnpEntity snpDate in caseSnpDetails)
                                {
                                    if (snpDate.ExpireWorkDate != string.Empty)
                                        if (Convert.ToDateTime(snpDate.ExpireWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.ExpireWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SFarmer:
                                count = caseSnpDetails.FindAll(u => u.Farmer.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SFoodStamps:
                                count = caseSnpDetails.FindAll(u => u.FootStamps.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.WorkStatus:
                                count = caseSnpDetails.FindAll(u => u.WorkStatus.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.DisconectYouth:
                                count = caseSnpDetails.FindAll(u => u.Youth.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.MiltaryStatus:
                                count = caseSnpDetails.FindAll(u => u.MilitaryStatus.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.HealthCodes:
                                foreach (CaseSnpEntity snpHealth in caseSnpDetails)
                                {
                                    if (!string.IsNullOrEmpty(snpHealth.Health_Codes.Trim()))
                                    {
                                        int intRankSnpPoints = fillAlertHealthCodes(snpHealth.Health_Codes.Trim(), RnkCrit2, FilterCode.Trim(), item.CountInd.Trim());
                                        HealthPoints = HealthPoints + intRankSnpPoints;
                                    }
                                    count = count + 1;
                                }
                                if (caseSnpDetails.Count == count)
                                {
                                    commonLowcount.Add(new CommonEntity(HealthPoints.ToString(), item.RespText));
                                    count++;
                                }
                                break;
                            case Consts.RankQues.SFThours:
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {
                                    if (snpNumeric.FullTimeHours != string.Empty)
                                        if (Convert.ToDecimal(snpNumeric.FullTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.FullTimeHours) <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SHealthIns:
                                count = caseSnpDetails.FindAll(u => u.HealthIns.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SHireDate:
                                foreach (CaseSnpEntity snpDate in caseSnpDetails)
                                {
                                    if (snpDate.HireDate != string.Empty)
                                        if (Convert.ToDateTime(snpDate.HireDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.HireDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SHourlyWage:
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {
                                    if (snpNumeric.HourlyWage != string.Empty)
                                        if (Convert.ToDecimal(snpNumeric.HourlyWage) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.HourlyWage) <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SjobCategory:
                                count = caseSnpDetails.FindAll(u => u.JobCategory.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SjobTitle:
                                count = caseSnpDetails.FindAll(u => u.JobTitle.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SLastWorkDate:
                                foreach (CaseSnpEntity snpDate in caseSnpDetails)
                                {
                                    if (snpDate.LastWorkDate != string.Empty)
                                        if (Convert.ToDateTime(snpDate.LastWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.LastWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SLegalTowork:
                                count = caseSnpDetails.FindAll(u => u.LegalTowork.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SMartialStatus:
                                count = caseSnpDetails.FindAll(u => u.MaritalStatus.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SMemberCode:
                                count = caseSnpDetails.FindAll(u => u.MemberCode.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SNofcjob:
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {
                                    if (snpNumeric.NumberOfcjobs != string.Empty)
                                        if (Convert.ToDecimal(snpNumeric.NumberOfcjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberOfcjobs) <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SNofljobs:
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {
                                    if (snpNumeric.NumberofLvjobs != string.Empty)
                                        if (Convert.ToDecimal(snpNumeric.NumberofLvjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberofLvjobs) <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SPFrequency:
                                count = caseSnpDetails.FindAll(u => u.PayFrequency.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SPregnant:
                                count = caseSnpDetails.FindAll(u => u.Pregnant.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SPThours:
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {
                                    if (snpNumeric.PartTimeHours != string.Empty)
                                        if (Convert.ToDecimal(snpNumeric.PartTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.PartTimeHours) <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SRace:
                                count = caseSnpDetails.FindAll(u => u.Race.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SRelitran:
                                count = caseSnpDetails.FindAll(u => u.Relitran.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SResident:
                                count = caseSnpDetails.FindAll(u => u.Resident.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SRshift:
                                count = caseSnpDetails.FindAll(u => u.RShift.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SSEmploy:
                                count = caseSnpDetails.FindAll(u => u.SeasonalEmploy.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SSex:
                                count = caseSnpDetails.FindAll(u => u.Sex.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SSnpVet:
                                count = caseSnpDetails.FindAll(u => u.Vet.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SStatus:
                                count = caseSnpDetails.FindAll(u => u.Status.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.STranserv:
                                count = caseSnpDetails.FindAll(u => u.Transerv.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SworkLimit:
                                count = caseSnpDetails.FindAll(u => u.WorkLimit.Trim().Equals(item.RespCd)).Count;
                                break;

                        }

                        if (caseSnpDetails.Count == count)
                        {
                            commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                        }
                    }
                    else
                    {
                        switch (ResponceType)
                        {
                            case "D":
                            case "L":
                                foreach (CaseSnpEntity snpdropdown in caseSnpDetails)
                                {

                                    switch (FilterCode)
                                    {
                                        case Consts.RankQues.S1shift:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.IstShift.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.S2ndshift:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.IIndShift.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.S3rdShift:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.IIIrdShift.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SSchoolDistrict:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.SchoolDistrict.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SEducation:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Education.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SWic:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Wic.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SDisable:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Disable.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SDrvlic:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Drvlic.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SEmployed:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Employed.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SEthinic:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Ethnic.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SFarmer:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Farmer.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SFoodStamps:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.FootStamps.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.WorkStatus:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.WorkStatus.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.DisconectYouth:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Youth.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.MiltaryStatus:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.MilitaryStatus.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.HealthCodes:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode)
                                            {
                                                int intRankSnpPoints = fillAlertHealthCodes(snpdropdown.Health_Codes.Trim(), RnkCrit2, FilterCode.Trim(), item.CountInd.Trim());
                                                //intRankPoint = intRankPoint + intRankSnpPoints;
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                commonLowcount.Add(new CommonEntity(intRankSnpPoints.ToString(), item.RespText));
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SHealthIns:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.HealthIns.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SjobCategory:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.JobCategory.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SjobTitle:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.JobTitle.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SLegalTowork:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.LegalTowork.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SMartialStatus:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.MaritalStatus.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SMemberCode:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.MemberCode.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SPFrequency:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.PayFrequency.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SPregnant:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Pregnant.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SRace:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Race.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SRelitran:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Relitran.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SResident:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Resident.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SRshift:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.RShift.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SSEmploy:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.SeasonalEmploy.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SSex:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Sex.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SSnpVet:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Vet.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SStatus:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Status.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.STranserv:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Transerv.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                        case Consts.RankQues.SworkLimit:
                                            if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.WorkLimit.Trim())
                                            {
                                                commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                strResponceData = item.RespCd.Trim();
                                            }
                                            break;
                                    }
                                }
                                //if (listRelationstring.Contains(item.Relation))
                                //{
                                //    if (listCodestring.Contains(item.RespCd))
                                //    {
                                //        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                //    }
                                //}
                                break;
                            case "N":
                            case "G":
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {

                                    switch (FilterCode)
                                    {
                                        case Consts.RankQues.SAge:
                                            if (snpNumeric.AltBdate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                            {
                                                DateTime EndDate = GetEndDateAgeCalculation(item.AgeClcInd.Trim(), propMstRank);
                                                int AgeMonth = _model.lookupDataAccess.GetAgeCalculationMonths(Convert.ToDateTime(snpNumeric.AltBdate), EndDate);
                                                if (AgeMonth >= Convert.ToDecimal(item.GtNum) && AgeMonth <= Convert.ToDecimal(item.LtNum))
                                                {
                                                    // intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                    commonLowcount.Add(new CommonEntity(item.Points, item.RespText));

                                                }
                                                if (AgeMonth > 12)
                                                {
                                                    strResponceData = (AgeMonth / 12).ToString();
                                                }
                                            }
                                            break;

                                        case Consts.RankQues.SNofcjob:
                                            if (snpNumeric.NumberOfcjobs != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDecimal(snpNumeric.NumberOfcjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberOfcjobs) <= Convert.ToDecimal(item.LtNum))
                                                {
                                                    commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                    //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SNofljobs:
                                            if (snpNumeric.NumberofLvjobs != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDecimal(snpNumeric.NumberofLvjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberofLvjobs) <= Convert.ToDecimal(item.LtNum))
                                                {

                                                    commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                    //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SFThours:
                                            if (snpNumeric.FullTimeHours != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDecimal(snpNumeric.FullTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.FullTimeHours) <= Convert.ToDecimal(item.LtNum))
                                                {

                                                    commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                    //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SPThours:
                                            if (snpNumeric.PartTimeHours != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDecimal(snpNumeric.PartTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.PartTimeHours) <= Convert.ToDecimal(item.LtNum))
                                                {

                                                    commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                    //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SHourlyWage:
                                            if (snpNumeric.HourlyWage != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDecimal(snpNumeric.HourlyWage) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.HourlyWage) <= Convert.ToDecimal(item.LtNum))
                                                {

                                                    commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                    //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;

                                    }
                                }
                                break;
                            case "B":
                            case "T":
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {

                                    switch (FilterCode)
                                    {
                                        case Consts.RankQues.SAltBdate:
                                            if (snpNumeric.AltBdate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDateTime(snpNumeric.AltBdate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.AltBdate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                                {

                                                    commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                    //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SExpireWorkDate:
                                            if (snpNumeric.ExpireWorkDate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDateTime(snpNumeric.ExpireWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.ExpireWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                                {

                                                    commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                    //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SLastWorkDate:
                                            if (snpNumeric.LastWorkDate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDateTime(snpNumeric.LastWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.LastWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                                {

                                                    commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                    //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SHireDate:
                                            if (snpNumeric.HireDate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDateTime(snpNumeric.HireDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.HireDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                                {

                                                    commonLowcount.Add(new CommonEntity(item.Points, item.RespText));
                                                    //intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;


                                    }
                                }
                                break;

                        }


                    }

                }

            }
            if (commonHighcount.Count > 0)
            {
                commonHighcount = commonHighcount.FindAll(u => u.Code.Trim() != string.Empty);
                commonHighcount = commonHighcount.OrderByDescending(u => Convert.ToInt16(u.Code)).ToList();
                if (commonHighcount.Count > 0)
                {
                    intSnpPoints = intSnpPoints + Convert.ToInt32(commonHighcount[0].Code);
                }
            }
            if (commonLowcount.Count > 0)
            {
                commonLowcount = commonLowcount.FindAll(u => u.Code.Trim() != string.Empty);
                commonLowcount = commonLowcount.OrderBy(u => Convert.ToInt16(u.Code)).ToList();
                if (commonLowcount.Count > 0)
                {
                    intSnpPoints = intSnpPoints + Convert.ToInt32(commonLowcount[0].Code);
                }
            }

            strResponseDesc = strResponceData;
            return intSnpPoints;
        }



        public DateTime GetEndDateAgeCalculation(string Type, CaseMstEntity caseMst)
        {
            DateTime EndDate = DateTime.Now.Date;
            if (Type == "T")
            {
                EndDate = DateTime.Now.Date;
            }
            else if (Type == "I")
            {
                EndDate = Convert.ToDateTime(caseMst.IntakeDate);
            }
            else if (Type == "K")
            {
                string strDate = DateTime.Now.Date.ToShortDateString();
                string strYear;
                //  List<ZipCodeEntity> zipCodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(caseMst.Zip, string.Empty, string.Empty, string.Empty);
                List<ZipCodeEntity> zipCodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(caseMst.Zip, string.Empty, string.Empty, string.Empty);
                ZipCodeEntity zipentity = zipCodeEntity.Find(u => u.Zcrzip.Trim().Equals(caseMst.Zip.Trim()));
                if (zipentity != null)
                {
                    if (zipentity.Zcrhssyear.Trim() == "2")
                    {
                        strYear = DateTime.Now.AddYears(1).Year.ToString();
                    }
                    else
                    {
                        strYear = DateTime.Now.Year.ToString();
                    }
                    strDate = zipentity.Zcrhssmo + "/" + zipentity.Zcrhssday + "/" + strYear;
                }
                EndDate = Convert.ToDateTime(strDate);
            }
            return EndDate;
        }


        public string GetSnpAgyTabDesc(RNKCRIT2Entity rnkQuesData, string strApplicationcode)
        {
            string strDesc = string.Empty;
            if (rnkQuesData.RankFldRespType == "D" || rnkQuesData.RankFldRespType == "L")
            {
                AGYTABSEntity agytab = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == strApplicationcode);
                if (agytab != null)
                    strDesc = agytab.Code_Desc.ToString();
                else
                    strDesc = strApplicationcode;
            }
            else
                strDesc = strApplicationcode;

            return strDesc;
        }



        #endregion



        #region RankCateogryPointsCalculation
        //public CaseMstEntity propMstRank { get; set; }

        //DataTable dtRankSubDetails;
        //int intRankPoint = 0;
        //private DataTable GetRankCategoryDetails(CaseMstEntity caseMst, List<CaseSnpEntity> caseSnp, ChldMstEntity chldMst, List<RNKCRIT2Entity> RnkQuesFledsEntity, List<RNKCRIT2Entity> RnkQuesFledsAllDataEntity, List<RNKCRIT2Entity> RnkCustFldsAllDataEntity)
        //{
        //    intRankPoint = 0;
        //    dtRankSubDetails = new DataTable();
        //    dtRankSubDetails.Columns.Add("FieldCode", typeof(string));
        //    dtRankSubDetails.Columns.Add("FieldDesc", typeof(string));
        //    dtRankSubDetails.Columns.Add("Points", typeof(string));

        //    //
        //    // Here we add five DataRows.
        //    //      


        //    List<CommonEntity> ListRankPoints = new List<CommonEntity>();
        //    //for (int intRankCtg = 1; intRankCtg <= 6; intRankCtg++)
        //    //{ 

        //    //List<RNKCRIT2Entity> RnkQuesFledsDataEntity = RnkQuesFledsAllDataEntity.FindAll(u => u.RankCategory.Trim().ToString() == ((Captain.Common.Utilities.ListItem)cmbRankCategory.SelectedItem).Value.ToString());
        //    //List<RNKCRIT2Entity> RnkCustFldsDataEntity = RnkCustFldsAllDataEntity.FindAll(u => u.RankCategory.Trim().ToString() == ((Captain.Common.Utilities.ListItem)cmbRankCategory.SelectedItem).Value.ToString());

        //    List<RNKCRIT2Entity> RnkQuesFledsDataEntity = RnkQuesFledsAllDataEntity;
        //    List<RNKCRIT2Entity> RnkCustFldsDataEntity = RnkCustFldsAllDataEntity;
        //    List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
        //    if (RnkCustFldsAllDataEntity.Count > 0)
        //    {
        //        if (propcustResponses.Count > 0)
        //            custResponses = propcustResponses.FindAll(u => (u.ACTAGENCY == caseMst.ApplAgency.ToString()) && (u.ACTDEPT == caseMst.ApplDept) && (u.ACTPROGRAM == caseMst.ApplProgram) && (u.ACTYEAR == caseMst.ApplYr) && (u.ACTAPPNO == caseMst.ApplNo.ToString()));
        //    }


        //    List<RNKCRIT2Entity> RnkQuesSearchList;
        //    propMstRank = caseMst;
        //    RNKCRIT2Entity RnkQuesSearch = null;
        //    // List<RNKCRIT2Entity> RnkQuesCaseSnp = null;
        //    int intRankSnpPoints = 0;
        //    string strApplicationcode = string.Empty;
        //    foreach (RNKCRIT2Entity rnkQuesData in RnkQuesFledsEntity)
        //    {
        //        intRankSnpPoints = 0;
        //        DataRow dr = dtRankSubDetails.NewRow();
        //        RnkQuesSearch = null;
        //        dr["FieldCode"] = rnkQuesData.RankFldDesc.ToString();
        //        switch (rnkQuesData.RankFldName.Trim())
        //        {

        //            case Consts.RankQues.MZip:
        //                dr["FieldDesc"] = caseMst.Zip.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Zip.Trim());
        //                break;
        //            case Consts.RankQues.MCounty:
        //                dr["FieldDesc"] = caseMst.County.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.County.Trim());
        //                break;
        //            case Consts.RankQues.MLanguage:
        //                dr["FieldDesc"] = caseMst.Language.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Language.Trim());
        //                break;
        //            case Consts.RankQues.MAlertCode:
        //                dr["FieldDesc"] = caseMst.AlertCodes.Trim();
        //                intRankSnpPoints = fillAlertIncomeCodes(caseMst.AlertCodes, RnkQuesFledsDataEntity, rnkQuesData.RankFldName.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.MAboutUs:
        //                dr["FieldDesc"] = caseMst.AboutUs.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.AboutUs.Trim());
        //                break;
        //            case Consts.RankQues.MAddressYear:
        //                if (caseMst.AddressYears != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.AddressYears.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.AddressYears) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.AddressYears));
        //                }
        //                break;
        //            case Consts.RankQues.MBestContact:
        //                dr["FieldDesc"] = caseMst.BestContact.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.BestContact.Trim());
        //                break;
        //            case Consts.RankQues.MCaseReviewDate:
        //                if (caseMst.CaseReviewDate != string.Empty)
        //                {
        //                    dr["FieldDesc"] = LookupDataAccess.Getdate(caseMst.CaseReviewDate);
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.CaseReviewDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.CaseReviewDate).Date);
        //                }
        //                break;
        //            case Consts.RankQues.MCaseType:
        //                dr["FieldDesc"] = caseMst.CaseType.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.CaseType.Trim());
        //                break;
        //            case Consts.RankQues.MCmi:
        //                if (caseMst.Cmi != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.Cmi.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Cmi) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Cmi));
        //                }
        //                break;
        //            case Consts.RankQues.MEElectric:
        //                if (caseMst.ExpElectric != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.ExpElectric.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpElectric) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpElectric));
        //                }
        //                break;
        //            case Consts.RankQues.MEDEBTCC:
        //                if (caseMst.Debtcc != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.Debtcc.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Debtcc) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Debtcc));
        //                }
        //                break;
        //            case Consts.RankQues.MEDEBTLoans:
        //                if (caseMst.DebtLoans != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.DebtLoans.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.DebtLoans) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.DebtLoans));
        //                }
        //                break;
        //            case Consts.RankQues.MEDEBTMed:
        //                if (caseMst.DebtMed != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.DebtMed.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.DebtMed) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.DebtMed));
        //                }
        //                break;
        //            case Consts.RankQues.MEHeat:
        //                if (caseMst.ExpHeat != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.ExpHeat.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpHeat) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpHeat));
        //                }
        //                break;
        //            case Consts.RankQues.MEligDate:
        //                if (caseMst.EligDate != string.Empty)
        //                {
        //                    dr["FieldDesc"] = LookupDataAccess.Getdate(caseMst.EligDate);
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.EligDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.EligDate).Date);
        //                }
        //                break;
        //            case Consts.RankQues.MELiveExpenses:
        //                if (caseMst.ExpLivexpense != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.ExpLivexpense.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpLivexpense) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpLivexpense));

        //                }
        //                break;
        //            case Consts.RankQues.MERent:
        //                if (caseMst.ExpRent != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.ExpRent.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpRent) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpRent));
        //                }
        //                break;
        //            case Consts.RankQues.METotal:
        //                if (caseMst.ExpTotal != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.ExpTotal.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpTotal) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpTotal));
        //                }
        //                break;
        //            case Consts.RankQues.MEWater:
        //                if (caseMst.ExpWater != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.ExpWater.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpWater) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpWater));
        //                }
        //                break;

        //            case Consts.RankQues.MExpCaseworker:
        //                dr["FieldDesc"] = caseMst.ExpCaseWorker.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.ExpCaseWorker.Trim());
        //                break;
        //            case Consts.RankQues.MFamilyType:
        //                dr["FieldDesc"] = caseMst.FamilyType.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.FamilyType.Trim());
        //                break;
        //            case Consts.RankQues.MFamIncome:
        //                if (caseMst.FamIncome != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.FamIncome.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.FamIncome) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.FamIncome));
        //                }
        //                break;
        //            case Consts.RankQues.MHousing:
        //                dr["FieldDesc"] = caseMst.Housing.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Housing.Trim());
        //                break;
        //            case Consts.RankQues.MHud:
        //                if (caseMst.Hud != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.Hud.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Hud) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Hud));
        //                }
        //                break;

        //            case Consts.RankQues.MIncomeTypes:
        //                dr["FieldDesc"] = caseMst.IncomeTypes.Trim();
        //                intRankSnpPoints = fillAlertIncomeCodes(caseMst.IncomeTypes, RnkQuesFledsDataEntity, rnkQuesData.RankFldName.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                //RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.IncomeTypes.Trim());
        //                break;
        //            case Consts.RankQues.MInitialDate:
        //                if (caseMst.InitialDate != string.Empty)
        //                {
        //                    dr["FieldDesc"] = LookupDataAccess.Getdate(caseMst.InitialDate);
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.InitialDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.InitialDate).Date);
        //                }
        //                break;
        //            case Consts.RankQues.MIntakeDate:
        //                if (caseMst.IntakeDate != string.Empty)
        //                {
        //                    dr["FieldDesc"] = LookupDataAccess.Getdate(caseMst.IntakeDate);
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.IntakeDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.IntakeDate).Date);
        //                }
        //                break;
        //            case Consts.RankQues.MIntakeWorker:
        //                dr["FieldDesc"] = caseMst.IntakeWorker.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.IntakeWorker.Trim());
        //                break;
        //            case Consts.RankQues.MJuvenile:
        //                dr["FieldDesc"] = caseMst.Juvenile.Trim() == "Y" ? "Selected" : "UnSelected";
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Juvenile.Trim());
        //                break;
        //            case Consts.RankQues.MLanguageOt:
        //                dr["FieldDesc"] = caseMst.LanguageOt.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.LanguageOt.Trim());
        //                break;
        //            case Consts.RankQues.MNoInprog:
        //                if (caseMst.NoInProg != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.NoInProg.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.NoInProg) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.NoInProg));
        //                }
        //                break;
        //            case Consts.RankQues.Mpoverty:
        //                if (caseMst.Poverty != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.Poverty.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Poverty) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Poverty));
        //                    // RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Poverty.Trim());
        //                }
        //                break;
        //            case Consts.RankQues.MProgIncome:
        //                if (caseMst.ProgIncome != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.ProgIncome.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ProgIncome) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ProgIncome));
        //                }
        //                break;
        //            case Consts.RankQues.MReverifyDate:
        //                if (caseMst.ReverifyDate != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.ReverifyDate.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.ReverifyDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.ReverifyDate).Date);
        //                }
        //                break;
        //            case Consts.RankQues.MSECRET:
        //                dr["FieldDesc"] = caseMst.Secret.Trim() == "Y" ? "Selected" : "UnSelected";
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Secret.Trim());
        //                break;
        //            case Consts.RankQues.MSenior:
        //                dr["FieldDesc"] = caseMst.Senior.Trim() == "Y" ? "Selected" : "UnSelected";
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Senior.Trim());
        //                break;
        //            case Consts.RankQues.MSite:
        //                dr["FieldDesc"] = caseMst.Site.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Site.Trim());
        //                break;
        //            case Consts.RankQues.MSMi:
        //                if (caseMst.Smi != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.Smi.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Smi) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Smi));
        //                }
        //                break;
        //            case Consts.RankQues.MVefiryCheckstub:
        //                // {
        //                dr["FieldDesc"] = caseMst.VerifyCheckStub.Trim() == "Y" ? "Selected" : "UnSelected";
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyCheckStub.Trim());
        //                //}
        //                break;
        //            case Consts.RankQues.MVerifier:
        //                dr["FieldDesc"] = caseMst.Verifier.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Verifier.Trim());
        //                break;
        //            case Consts.RankQues.MVerifyW2:
        //                dr["FieldDesc"] = caseMst.VerifyW2.Trim() == "Y" ? "Selected" : "UnSelected";
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyW2.Trim());
        //                break;
        //            case Consts.RankQues.MVeriTaxReturn:
        //                dr["FieldDesc"] = caseMst.VerifyTaxReturn.Trim() == "Y" ? "Selected" : "UnSelected";
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyTaxReturn.Trim());
        //                break;
        //            case Consts.RankQues.MVerLetter:
        //                dr["FieldDesc"] = caseMst.VerifyLetter.Trim() == "Y" ? "Selected" : "UnSelected";
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyLetter.Trim());
        //                break;
        //            case Consts.RankQues.MVerOther:
        //                dr["FieldDesc"] = caseMst.VerifyOther.Trim() == "Y" ? "Selected" : "UnSelected";
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyOther.Trim());
        //                break;
        //            case Consts.RankQues.MWaitList:
        //                dr["FieldDesc"] = caseMst.WaitList.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.WaitList.Trim());
        //                break;
        //            case Consts.RankQues.SEducation:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Education.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                //List<string> SnpFieldsCodesList = new List<string>();
        //                //List<string> SnpFieldsRelationList = new List<string>();
        //                //for (int i = 0; i < caseSnp.Count; i++)
        //                //{
        //                //    SnpFieldsCodesList.Add(caseSnp[i].Education);
        //                //    SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
        //                //}
        //                //intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.S1shift:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).IstShift.ToString();
        //                dr["FieldDesc"] = strApplicationcode == "Y" ? "Selected" : "UnSelected";
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.S2ndshift:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).IIndShift.ToString();
        //                dr["FieldDesc"] = strApplicationcode == "Y" ? "Selected" : "UnSelected";
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.S3rdShift:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).IIIrdShift.ToString();
        //                dr["FieldDesc"] = strApplicationcode == "Y" ? "Selected" : "UnSelected";
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SAge:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Age.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SAltBdate:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).AltBdate.ToString();
        //                dr["FieldDesc"] = LookupDataAccess.Getdate(strApplicationcode);

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SDisable:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Disable.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SDrvlic:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Drvlic.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SEmployed:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Employed.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SEthinic:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Ethnic.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SExpireWorkDate:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).ExpireWorkDate.ToString();
        //                dr["FieldDesc"] = LookupDataAccess.Getdate(strApplicationcode);

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SFarmer:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Farmer.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SFoodStamps:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).FootStamps.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SFThours:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).FullTimeHours.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SHealthIns:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).HealthIns.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SHireDate:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).HireDate.ToString();
        //                dr["FieldDesc"] = LookupDataAccess.Getdate(strApplicationcode);

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SHourlyWage:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).HourlyWage.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SjobCategory:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).JobCategory.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SjobTitle:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).JobTitle.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SLastWorkDate:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).LastWorkDate.ToString();
        //                dr["FieldDesc"] = LookupDataAccess.Getdate(strApplicationcode);

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SLegalTowork:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).LegalTowork.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SMartialStatus:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).MaritalStatus.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SMemberCode:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).MemberCode.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SNofcjob:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).NumberOfcjobs.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SNofljobs:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).NumberofLvjobs.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SPFrequency:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).PayFrequency.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SPregnant:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Pregnant.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SPThours:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).PartTimeHours.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SRace:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Race.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SRelitran:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Relitran.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SResident:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Resident.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SRshift:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).RShift.ToString();
        //                dr["FieldDesc"] = strApplicationcode == "Y" ? "Selected" : "UnSelected";
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SSchoolDistrict:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).SchoolDistrict.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SSEmploy:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Employed.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SSex:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Sex.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SSnpVet:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Vet.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SStatus:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Status.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.STranserv:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Transerv.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SWic:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Wic.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SworkLimit:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).WorkLimit.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.CDentalCoverage:
        //                if (chldMst != null)
        //                {
        //                    dr["FieldDesc"] = chldMst.DentalCoverage.Trim() == "True" ? "Seleted" : "UnSelected";
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == (chldMst.DentalCoverage.Trim() == "True" ? "Y" : "N"));
        //                }
        //                break;
        //            case Consts.RankQues.CDiagNosisDate:
        //                if (chldMst != null)
        //                    if (chldMst.DiagnosisDate != string.Empty)
        //                    {
        //                        dr["FieldDesc"] = LookupDataAccess.Getdate(chldMst.DiagnosisDate);
        //                        RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(chldMst.DiagnosisDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(chldMst.DiagnosisDate).Date);
        //                    }
        //                break;
        //            case Consts.RankQues.CDisability:
        //                if (chldMst != null)
        //                {
        //                    dr["FieldDesc"] = chldMst.Disability.Trim() == "True" ? "Seleted" : "UnSelected";
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == (chldMst.Disability.Trim() == "True" ? "Y" : "N"));
        //                }
        //                break;
        //            case Consts.RankQues.CInsCat:
        //                if (chldMst != null)
        //                {
        //                    dr["FieldDesc"] = chldMst.InsCat.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == chldMst.InsCat.Trim());
        //                }
        //                break;
        //            case Consts.RankQues.CMedCoverage:
        //                if (chldMst != null)
        //                {
        //                    dr["FieldDesc"] = chldMst.MedCoverage.Trim() == "True" ? "Seleted" : "UnSelected";
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == (chldMst.MedCoverage.Trim() == "True" ? "Y" : "N"));
        //                }
        //                break;
        //            case Consts.RankQues.CMedicalCoverageType:
        //                if (chldMst != null)
        //                {
        //                    dr["FieldDesc"] = chldMst.MedCoverType.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == chldMst.MedCoverType.Trim());
        //                }
        //                break;


        //        }

        //        if (RnkQuesSearch != null)
        //        {
        //            intRankPoint = intRankPoint + Convert.ToInt32(RnkQuesSearch.Points);
        //            dr["Points"] = RnkQuesSearch.Points;
        //            dtRankSubDetails.Rows.Add(dr);
        //        }
        //        else
        //        {
        //            dr["Points"] = intRankSnpPoints;
        //            dtRankSubDetails.Rows.Add(dr);
        //        }
        //        // }


        //        //ListRankPoints.Add(new CommonEntity(intRankCtg.ToString(), intRankPoint.ToString()));
        //    }
        //    if (custResponses.Count > 0)
        //    {
        //        try
        //        {


        //            CustomQuestionsEntity custResponcesearch = null;
        //            RNKCRIT2Entity rnkPoints = null;
        //            string strQuestionType = string.Empty;
        //            foreach (CustomQuestionsEntity responceQuestion in custResponses)
        //            {
        //                DataRow dr1 = dtRankSubDetails.NewRow();
        //                List<RNKCRIT2Entity> rnkCustFldsFilterCode = RnkCustFldsDataEntity.FindAll(u => u.RankFiledCode.Trim() == responceQuestion.ACTCODE.Trim());

        //                if (rnkCustFldsFilterCode.Count > 0)
        //                {

        //                    custResponcesearch = null;
        //                    rnkPoints = null;
        //                    strQuestionType = rnkCustFldsFilterCode[0].RankFldRespType.Trim();
        //                    dr1["FieldCode"] = rnkCustFldsFilterCode[0].RankFldDesc.Trim();

        //                    switch (rnkCustFldsFilterCode[0].RankFldRespType.Trim())
        //                    {
        //                        case "D":
        //                        case "L":
        //                            rnkPoints = rnkCustFldsFilterCode.Find(u => u.RespCd.Trim() == responceQuestion.ACTMULTRESP.Trim());
        //                            //custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && u.ACTMULTRESP.Trim() == item.RespCd.Trim());
        //                            dr1["FieldDesc"] = propCustResponceList.Find(u => u.ResoCode.Trim() == responceQuestion.ACTCODE && u.DescCode == responceQuestion.ACTMULTRESP).RespDesc.ToString();
        //                            break;
        //                        case "N":
        //                            rnkPoints = rnkCustFldsFilterCode.Find(u => Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(responceQuestion.ACTNUMRESP) && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(responceQuestion.ACTNUMRESP));
        //                            // custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && Convert.ToDecimal(u.GtNum) >= Convert.ToDecimal(item.ACTNUMRESP) && Convert.ToDecimal(u.LtNum) <= Convert.ToDecimal(item.ACTNUMRESP));
        //                            dr1["FieldDesc"] = responceQuestion.ACTNUMRESP;
        //                            break;
        //                        case "T":
        //                        case "B":
        //                            rnkPoints = rnkCustFldsFilterCode.Find(u => Convert.ToDateTime(u.GtDate) <= Convert.ToDateTime(responceQuestion.ACTDATERESP) && Convert.ToDateTime(u.LtDate) >= Convert.ToDateTime(responceQuestion.ACTDATERESP));
        //                            // custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && Convert.ToDateTime(u.GtDate) >= Convert.ToDateTime(item.ACTDATERESP) && Convert.ToDateTime(u.LtDate) <= Convert.ToDateTime(item.ACTNUMRESP));
        //                            dr1["FieldDesc"] = responceQuestion.ACTDATERESP;
        //                            break;
        //                    }
        //                    if (rnkPoints != null)
        //                    {
        //                        dr1["Points"] = rnkPoints.Points;
        //                        intRankPoint = intRankPoint + Convert.ToInt32(rnkPoints.Points);
        //                    }
        //                    else
        //                    {
        //                        dr1["Points"] = "0";
        //                    }
        //                    dtRankSubDetails.Rows.Add(dr1);

        //                }

        //            }


        //            //********* Old Code  *********


        //            //CustomQuestionsEntity custResponcesearch = null;
        //            //string strQuestionType = string.Empty;
        //            //foreach (CustomQuestionsEntity responceQuestion in custResponses)
        //            //{
        //            //    DataRow dr1 = dtRankSubDetails.NewRow();
        //            //    foreach (RNKCRIT2Entity item in RnkCustFldsDataEntity)
        //            //    {

        //            //        if (responceQuestion.ACTCODE.Trim() == item.RankFiledCode.Trim())
        //            //        {
        //            //            custResponcesearch = null;
        //            //            strQuestionType = item.RankFldRespType.Trim();
        //            //            dr1["FieldCode"] = item.RankFldDesc.Trim();

        //            //            switch (item.RankFldRespType.Trim())
        //            //            {
        //            //                case "D":
        //            //                case "L":
        //            //                    custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && u.ACTMULTRESP.Trim() == item.RespCd.Trim());
        //            //                    CustRespEntity custResponce = propCustResponceList.Find(u => u.ResoCode.Trim() == responceQuestion.ACTCODE && u.DescCode == responceQuestion.ACTMULTRESP);
        //            //                    if (custResponce != null)
        //            //                        dr1["FieldDesc"] = custResponce.RespDesc.ToString();
        //            //                    else
        //            //                        dr1["FieldDesc"] = string.Empty;
        //            //                    break;
        //            //                case "N":
        //            //                    custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && Convert.ToDecimal(u.ACTNUMRESP) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(u.ACTNUMRESP) <= Convert.ToDecimal(item.LtNum));
        //            //                    dr1["FieldDesc"] = responceQuestion.ACTNUMRESP;
        //            //                    break;
        //            //                case "T":
        //            //                case "B":
        //            //                    custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && Convert.ToDateTime(u.ACTDATERESP) >= Convert.ToDateTime(item.GtDate) && Convert.ToDateTime(u.ACTNUMRESP) <= Convert.ToDateTime(item.LtDate));
        //            //                    dr1["FieldDesc"] = responceQuestion.ACTDATERESP;
        //            //                    break;
        //            //            }
        //            //            if (custResponcesearch != null)
        //            //            {
        //            //                dr1["Points"] = item.Points;
        //            //                intRankPoint = intRankPoint + Convert.ToInt32(item.Points);
        //            //            }
        //            //            else
        //            //            {
        //            //                dr1["Points"] = "0";
        //            //            }
        //            //            dtRankSubDetails.Rows.Add(dr1);
        //            //            break;
        //            //        }
        //            //    }

        //            //}

        //        }
        //        catch (Exception ex)
        //        {


        //        }
        //    }
        //    //foreach (CommonEntity item in ListRankPoints)
        //    //{
        //    //   // txtProcess.Text = txtProcess.Text + item.Code + ":" + item.Desc + ",";
        //    //}
        //    return dtRankSubDetails;
        //}

        //private int fillAlertIncomeCodes(string alertCodes, List<RNKCRIT2Entity> rnkSearchEntity, string FieldName)
        //{
        //    int intAlertcode = 0;
        //    List<string> AlertList = new List<string>();
        //    if (alertCodes != null)
        //    {
        //        string[] incomeTypes = alertCodes.Split(' ');
        //        for (int i = 0; i < incomeTypes.Length; i++)
        //        {
        //            AlertList.Add(incomeTypes.GetValue(i).ToString());
        //        }
        //    }
        //    List<RNKCRIT2Entity> RnkAlertCode = rnkSearchEntity.FindAll(u => u.RankFldName.Trim() == FieldName);

        //    foreach (RNKCRIT2Entity rnkEntity in RnkAlertCode)
        //    {
        //        if (alertCodes != null && AlertList.Contains(rnkEntity.RespCd))
        //        {
        //            intAlertcode = intAlertcode + Convert.ToInt32(rnkEntity.Points);
        //        }
        //    }
        //    return intAlertcode;
        //}


        //private int CaseSnpDetailsCalc(List<RNKCRIT2Entity> rnkCaseSnp, List<CaseSnpEntity> caseSnpDetails, string strApplicantCode, string FilterCode, string ResponceType)
        //{
        //    int intSnpPoints = 0;
        //    foreach (RNKCRIT2Entity item in rnkCaseSnp)
        //    {
        //        if (item.CountInd.Trim() == "A")
        //        {
        //            switch (ResponceType)
        //            {
        //                case "D":
        //                case "L":
        //                    if (item.RespCd.Trim() == strApplicantCode)
        //                    {
        //                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                    }
        //                    break;
        //                case "N":
        //                    if (strApplicantCode != string.Empty)
        //                        if (Convert.ToDecimal(strApplicantCode) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(strApplicantCode) <= Convert.ToDecimal(item.LtNum))
        //                        {
        //                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                        }
        //                    break;
        //                case "B":
        //                case "T":
        //                    if (strApplicantCode != string.Empty)
        //                        if (Convert.ToDateTime(strApplicantCode).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(strApplicantCode).Date <= Convert.ToDateTime(item.LtDate).Date)
        //                        {
        //                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                        }
        //                    break;

        //            }

        //        }
        //        else if (item.CountInd.Trim() == "M")
        //        {
        //            if (item.Relation == "*")
        //            {
        //                int count = 0;
        //                switch (FilterCode)
        //                {
        //                    case Consts.RankQues.S1shift:
        //                        count = caseSnpDetails.FindAll(u => u.IstShift.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.S2ndshift:
        //                        count = caseSnpDetails.FindAll(u => u.IIndShift.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.S3rdShift:
        //                        count = caseSnpDetails.FindAll(u => u.IIIrdShift.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SAge:
        //                        foreach (CaseSnpEntity snpDate in caseSnpDetails)
        //                        {
        //                            if (snpDate.AltBdate != string.Empty)
        //                            {
        //                                DateTime EndDate = GetEndDateAgeCalculation(item.AgeClcInd.Trim(), propMstRank);
        //                                int AgeMonth = _model.lookupDataAccess.GetAgeCalculationMonths(Convert.ToDateTime(snpDate.AltBdate), EndDate);
        //                                if (AgeMonth >= Convert.ToDecimal(item.GtNum) && AgeMonth <= Convert.ToDecimal(item.LtNum))
        //                                {
        //                                    count = count + 1;
        //                                }
        //                            }
        //                        }
        //                        break;
        //                    case Consts.RankQues.SAltBdate:
        //                        foreach (CaseSnpEntity snpDate in caseSnpDetails)
        //                        {
        //                            if (snpDate.AltBdate != string.Empty)
        //                                if (Convert.ToDateTime(snpDate.AltBdate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.AltBdate).Date <= Convert.ToDateTime(item.LtDate).Date)
        //                                {
        //                                    count = count + 1;
        //                                }
        //                        }

        //                        break;
        //                    case Consts.RankQues.SSchoolDistrict:
        //                        count = caseSnpDetails.FindAll(u => u.SchoolDistrict.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SEducation:
        //                        count = caseSnpDetails.FindAll(u => u.Education.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SWic:
        //                        count = caseSnpDetails.FindAll(u => u.Wic.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SDisable:
        //                        count = caseSnpDetails.FindAll(u => u.Disable.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SDrvlic:
        //                        count = caseSnpDetails.FindAll(u => u.Drvlic.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SEmployed:
        //                        count = caseSnpDetails.FindAll(u => u.Employed.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SEthinic:
        //                        count = caseSnpDetails.FindAll(u => u.Ethnic.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SExpireWorkDate:
        //                        foreach (CaseSnpEntity snpDate in caseSnpDetails)
        //                        {
        //                            if (snpDate.ExpireWorkDate != string.Empty)
        //                                if (Convert.ToDateTime(snpDate.ExpireWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.ExpireWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
        //                                {
        //                                    count = count + 1;
        //                                }
        //                        }
        //                        break;
        //                    case Consts.RankQues.SFarmer:
        //                        count = caseSnpDetails.FindAll(u => u.Farmer.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SFoodStamps:
        //                        count = caseSnpDetails.FindAll(u => u.FootStamps.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SFThours:
        //                        foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
        //                        {
        //                            if (snpNumeric.FullTimeHours != string.Empty)
        //                                if (Convert.ToDecimal(snpNumeric.FullTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.FullTimeHours) <= Convert.ToDecimal(item.LtNum))
        //                                {
        //                                    count = count + 1;
        //                                }
        //                        }
        //                        break;
        //                    case Consts.RankQues.SHealthIns:
        //                        count = caseSnpDetails.FindAll(u => u.HealthIns.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SHireDate:
        //                        foreach (CaseSnpEntity snpDate in caseSnpDetails)
        //                        {
        //                            if (snpDate.HireDate != string.Empty)
        //                                if (Convert.ToDateTime(snpDate.HireDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.HireDate).Date <= Convert.ToDateTime(item.LtDate).Date)
        //                                {
        //                                    count = count + 1;
        //                                }
        //                        }
        //                        break;
        //                    case Consts.RankQues.SHourlyWage:
        //                        foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
        //                        {
        //                            if (snpNumeric.HourlyWage != string.Empty)
        //                                if (Convert.ToDecimal(snpNumeric.HourlyWage) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.HourlyWage) <= Convert.ToDecimal(item.LtNum))
        //                                {
        //                                    count = count + 1;
        //                                }
        //                        }
        //                        break;
        //                    case Consts.RankQues.SjobCategory:
        //                        count = caseSnpDetails.FindAll(u => u.JobCategory.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SjobTitle:
        //                        count = caseSnpDetails.FindAll(u => u.JobTitle.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SLastWorkDate:
        //                        foreach (CaseSnpEntity snpDate in caseSnpDetails)
        //                        {
        //                            if (snpDate.LastWorkDate != string.Empty)
        //                                if (Convert.ToDateTime(snpDate.LastWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.LastWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
        //                                {
        //                                    count = count + 1;
        //                                }
        //                        }
        //                        break;
        //                    case Consts.RankQues.SLegalTowork:
        //                        count = caseSnpDetails.FindAll(u => u.LegalTowork.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SMartialStatus:
        //                        count = caseSnpDetails.FindAll(u => u.MaritalStatus.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SMemberCode:
        //                        count = caseSnpDetails.FindAll(u => u.MemberCode.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SNofcjob:
        //                        foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
        //                        {
        //                            if (snpNumeric.NumberOfcjobs != string.Empty)
        //                                if (Convert.ToDecimal(snpNumeric.NumberOfcjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberOfcjobs) <= Convert.ToDecimal(item.LtNum))
        //                                {
        //                                    count = count + 1;
        //                                }
        //                        }
        //                        break;
        //                    case Consts.RankQues.SNofljobs:
        //                        foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
        //                        {
        //                            if (snpNumeric.NumberofLvjobs != string.Empty)
        //                                if (Convert.ToDecimal(snpNumeric.NumberofLvjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberofLvjobs) <= Convert.ToDecimal(item.LtNum))
        //                                {
        //                                    count = count + 1;
        //                                }
        //                        }
        //                        break;
        //                    case Consts.RankQues.SPFrequency:
        //                        count = caseSnpDetails.FindAll(u => u.PayFrequency.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SPregnant:
        //                        count = caseSnpDetails.FindAll(u => u.Pregnant.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SPThours:
        //                        foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
        //                        {
        //                            if (snpNumeric.PartTimeHours != string.Empty)
        //                                if (Convert.ToDecimal(snpNumeric.PartTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.PartTimeHours) <= Convert.ToDecimal(item.LtNum))
        //                                {
        //                                    count = count + 1;
        //                                }
        //                        }
        //                        break;
        //                    case Consts.RankQues.SRace:
        //                        count = caseSnpDetails.FindAll(u => u.Race.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SRelitran:
        //                        count = caseSnpDetails.FindAll(u => u.Relitran.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SResident:
        //                        count = caseSnpDetails.FindAll(u => u.Resident.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SRshift:
        //                        count = caseSnpDetails.FindAll(u => u.RShift.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SSEmploy:
        //                        count = caseSnpDetails.FindAll(u => u.SeasonalEmploy.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SSex:
        //                        count = caseSnpDetails.FindAll(u => u.Sex.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SSnpVet:
        //                        count = caseSnpDetails.FindAll(u => u.Vet.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SStatus:
        //                        count = caseSnpDetails.FindAll(u => u.Status.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.STranserv:
        //                        count = caseSnpDetails.FindAll(u => u.Transerv.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SworkLimit:
        //                        count = caseSnpDetails.FindAll(u => u.WorkLimit.Trim().Equals(item.RespCd)).Count;
        //                        break;

        //                }

        //                if (caseSnpDetails.Count == count)
        //                    intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //            }
        //            else
        //            {
        //                switch (ResponceType)
        //                {
        //                    case "D":
        //                    case "L":
        //                        foreach (CaseSnpEntity snpdropdown in caseSnpDetails)
        //                        {

        //                            switch (FilterCode)
        //                            {
        //                                case Consts.RankQues.S1shift:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.IstShift.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.S2ndshift:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.IIndShift.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.S3rdShift:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.IIIrdShift.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SSchoolDistrict:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.SchoolDistrict.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SEducation:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Education.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SWic:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Wic.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SDisable:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Disable.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SDrvlic:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Drvlic.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SEmployed:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Employed.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SEthinic:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Ethnic.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SFarmer:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Farmer.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SFoodStamps:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.FootStamps.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SHealthIns:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.HealthIns.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SjobCategory:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.JobCategory.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SjobTitle:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.JobTitle.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SLegalTowork:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.LegalTowork.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SMartialStatus:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.MaritalStatus.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SMemberCode:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.MemberCode.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SPFrequency:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.PayFrequency.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SPregnant:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Pregnant.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SRace:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Race.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SRelitran:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Relitran.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SResident:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Resident.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SRshift:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.RShift.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SSEmploy:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.SeasonalEmploy.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SSex:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Sex.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SSnpVet:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Vet.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SStatus:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Status.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.STranserv:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Transerv.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SworkLimit:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.WorkLimit.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                            }
        //                        }
        //                        //if (listRelationstring.Contains(item.Relation))
        //                        //{
        //                        //    if (listCodestring.Contains(item.RespCd))
        //                        //    {
        //                        //        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                        //    }
        //                        //}
        //                        break;
        //                    case "N":
        //                        foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
        //                        {

        //                            switch (FilterCode)
        //                            {
        //                                case Consts.RankQues.SAge:
        //                                    if (snpNumeric.AltBdate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
        //                                    {
        //                                        DateTime EndDate = GetEndDateAgeCalculation(item.AgeClcInd.Trim(), propMstRank);
        //                                        int AgeMonth = _model.lookupDataAccess.GetAgeCalculationMonths(Convert.ToDateTime(snpNumeric.AltBdate), EndDate);
        //                                        if (AgeMonth >= Convert.ToDecimal(item.GtNum) && AgeMonth <= Convert.ToDecimal(item.LtNum))
        //                                        {
        //                                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                        }
        //                                    }
        //                                    break;

        //                                case Consts.RankQues.SNofcjob:
        //                                    if (snpNumeric.NumberOfcjobs != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
        //                                        if (Convert.ToDecimal(snpNumeric.NumberOfcjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberOfcjobs) <= Convert.ToDecimal(item.LtNum))
        //                                        {
        //                                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                        }
        //                                    break;
        //                                case Consts.RankQues.SNofljobs:
        //                                    if (snpNumeric.NumberofLvjobs != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
        //                                        if (Convert.ToDecimal(snpNumeric.NumberofLvjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberofLvjobs) <= Convert.ToDecimal(item.LtNum))
        //                                        {
        //                                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                        }
        //                                    break;
        //                                case Consts.RankQues.SFThours:
        //                                    if (snpNumeric.FullTimeHours != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
        //                                        if (Convert.ToDecimal(snpNumeric.FullTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.FullTimeHours) <= Convert.ToDecimal(item.LtNum))
        //                                        {
        //                                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                        }
        //                                    break;
        //                                case Consts.RankQues.SPThours:
        //                                    if (snpNumeric.PartTimeHours != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
        //                                        if (Convert.ToDecimal(snpNumeric.PartTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.PartTimeHours) <= Convert.ToDecimal(item.LtNum))
        //                                        {
        //                                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                        }
        //                                    break;
        //                                case Consts.RankQues.SHourlyWage:
        //                                    if (snpNumeric.HourlyWage != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
        //                                        if (Convert.ToDecimal(snpNumeric.HourlyWage) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.HourlyWage) <= Convert.ToDecimal(item.LtNum))
        //                                        {
        //                                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                        }
        //                                    break;

        //                            }
        //                        }
        //                        break;
        //                    case "B":
        //                    case "T":
        //                        foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
        //                        {

        //                            switch (FilterCode)
        //                            {
        //                                case Consts.RankQues.SAltBdate:
        //                                    if (snpNumeric.AltBdate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
        //                                        if (Convert.ToDateTime(snpNumeric.AltBdate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.AltBdate).Date <= Convert.ToDateTime(item.LtDate).Date)
        //                                        {
        //                                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                        }
        //                                    break;
        //                                case Consts.RankQues.SExpireWorkDate:
        //                                    if (snpNumeric.ExpireWorkDate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
        //                                        if (Convert.ToDateTime(snpNumeric.ExpireWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.ExpireWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
        //                                        {
        //                                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                        }
        //                                    break;
        //                                case Consts.RankQues.SLastWorkDate:
        //                                    if (snpNumeric.LastWorkDate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
        //                                        if (Convert.ToDateTime(snpNumeric.LastWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.LastWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
        //                                        {
        //                                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                        }
        //                                    break;
        //                                case Consts.RankQues.SHireDate:
        //                                    if (snpNumeric.HireDate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
        //                                        if (Convert.ToDateTime(snpNumeric.HireDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.HireDate).Date <= Convert.ToDateTime(item.LtDate).Date)
        //                                        {
        //                                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                        }
        //                                    break;


        //                            }
        //                        }
        //                        break;

        //                }


        //            }

        //        }

        //    }
        //    return intSnpPoints;
        //}

        //public DateTime GetEndDateAgeCalculation(string Type, CaseMstEntity caseMst)
        //{
        //    DateTime EndDate = DateTime.Now.Date;
        //    if (Type == "T")
        //    {
        //        EndDate = DateTime.Now.Date;
        //    }
        //    else if (Type == "I")
        //    {
        //        EndDate = Convert.ToDateTime(caseMst.IntakeDate);
        //    }
        //    else if (Type == "K")
        //    {
        //        string strDate = DateTime.Now.Date.ToShortDateString();
        //        string strYear;
        //        ZipCodeEntity zipentity = propzipCodeEntity.Find(u => u.Zcrzip.Trim().Equals(caseMst.Zip.Trim()));
        //        if (zipentity != null)
        //        {
        //            if (zipentity.Zcrhssyear.Trim() == "2")
        //            {
        //                strYear = DateTime.Now.AddYears(1).Year.ToString();
        //            }
        //            else
        //            {
        //                strYear = DateTime.Now.Year.ToString();
        //            }
        //            strDate = zipentity.Zcrhssmo + "/" + zipentity.Zcrhssday + "/" + strYear;
        //        }
        //        EndDate = Convert.ToDateTime(strDate);
        //    }
        //    return EndDate;
        //}


        //#endregion


        //#region RankCateogryPointsCalculation
        //public CaseMstEntity propMstRank { get; set; }

        //DataTable dtRankSubDetails;
        //int intRankPoint = 0;
        //private DataTable GetRankCategoryDetails(CaseMstEntity caseMst, List<CaseSnpEntity> caseSnp, ChldMstEntity chldMst, List<RNKCRIT2Entity> RnkQuesFledsEntity, List<RNKCRIT2Entity> RnkQuesFledsAllDataEntity, List<RNKCRIT2Entity> RnkCustFldsAllDataEntity)
        //{
        //    intRankPoint = 0;
        //    dtRankSubDetails = new DataTable();
        //    dtRankSubDetails.Columns.Add("FieldCode", typeof(string));
        //    dtRankSubDetails.Columns.Add("FieldDesc", typeof(string));
        //    dtRankSubDetails.Columns.Add("Points", typeof(string));

        //    //
        //    // Here we add five DataRows.
        //    //      


        //    List<CommonEntity> ListRankPoints = new List<CommonEntity>();
        //    //for (int intRankCtg = 1; intRankCtg <= 6; intRankCtg++)
        //    //{ 

        //    //List<RNKCRIT2Entity> RnkQuesFledsDataEntity = RnkQuesFledsAllDataEntity.FindAll(u => u.RankCategory.Trim().ToString() == ((Captain.Common.Utilities.ListItem)cmbRankCategory.SelectedItem).Value.ToString());
        //    //List<RNKCRIT2Entity> RnkCustFldsDataEntity = RnkCustFldsAllDataEntity.FindAll(u => u.RankCategory.Trim().ToString() == ((Captain.Common.Utilities.ListItem)cmbRankCategory.SelectedItem).Value.ToString());

        //    List<RNKCRIT2Entity> RnkQuesFledsDataEntity = RnkQuesFledsAllDataEntity;
        //    List<RNKCRIT2Entity> RnkCustFldsDataEntity = RnkCustFldsAllDataEntity;
        //    List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
        //    if (RnkCustFldsAllDataEntity.Count > 0)
        //    {
        //        if (propcustResponses.Count > 0)
        //            custResponses = propcustResponses.FindAll(u => (u.ACTAGENCY == caseMst.ApplAgency.ToString()) && (u.ACTDEPT == caseMst.ApplDept) && (u.ACTPROGRAM == caseMst.ApplProgram) && (u.ACTYEAR == caseMst.ApplYr) && (u.ACTAPPNO == caseMst.ApplNo.ToString()));
        //    }


        //    List<RNKCRIT2Entity> RnkQuesSearchList;
        //    propMstRank = caseMst;
        //    RNKCRIT2Entity RnkQuesSearch = null;
        //    // List<RNKCRIT2Entity> RnkQuesCaseSnp = null;
        //    int intRankSnpPoints = 0;
        //    string strApplicationcode = string.Empty;
        //    foreach (RNKCRIT2Entity rnkQuesData in RnkQuesFledsEntity)
        //    {
        //        intRankSnpPoints = 0;
        //        DataRow dr = dtRankSubDetails.NewRow();
        //        RnkQuesSearch = null;
        //        dr["FieldCode"] = rnkQuesData.RankFldDesc.ToString();
        //        switch (rnkQuesData.RankFldName.Trim())
        //        {

        //            case Consts.RankQues.MZip:
        //                dr["FieldDesc"] = caseMst.Zip.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Zip.Trim());
        //                break;
        //            case Consts.RankQues.MCounty:
        //                dr["FieldDesc"] = caseMst.County.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.County.Trim());
        //                break;
        //            case Consts.RankQues.MLanguage:
        //                dr["FieldDesc"] = caseMst.Language.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Language.Trim());
        //                break;
        //            case Consts.RankQues.MAlertCode:
        //                dr["FieldDesc"] = caseMst.AlertCodes.Trim();
        //                intRankSnpPoints = fillAlertIncomeCodes(caseMst.AlertCodes, RnkQuesFledsDataEntity, rnkQuesData.RankFldName.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.MAboutUs:
        //                dr["FieldDesc"] = caseMst.AboutUs.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.AboutUs.Trim());
        //                break;
        //            case Consts.RankQues.MAddressYear:
        //                if (caseMst.AddressYears != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.AddressYears.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.AddressYears) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.AddressYears));
        //                }
        //                break;
        //            case Consts.RankQues.MBestContact:
        //                dr["FieldDesc"] = caseMst.BestContact.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.BestContact.Trim());
        //                break;
        //            case Consts.RankQues.MCaseReviewDate:
        //                if (caseMst.CaseReviewDate != string.Empty)
        //                {
        //                    dr["FieldDesc"] = LookupDataAccess.Getdate(caseMst.CaseReviewDate);
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.CaseReviewDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.CaseReviewDate).Date);
        //                }
        //                break;
        //            case Consts.RankQues.MCaseType:
        //                dr["FieldDesc"] = caseMst.CaseType.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.CaseType.Trim());
        //                break;
        //            case Consts.RankQues.MCmi:
        //                if (caseMst.Cmi != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.Cmi.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Cmi) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Cmi));
        //                }
        //                break;
        //            case Consts.RankQues.MEElectric:
        //                if (caseMst.ExpElectric != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.ExpElectric.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpElectric) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpElectric));
        //                }
        //                break;
        //            case Consts.RankQues.MEDEBTCC:
        //                if (caseMst.Debtcc != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.Debtcc.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Debtcc) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Debtcc));
        //                }
        //                break;
        //            case Consts.RankQues.MEDEBTLoans:
        //                if (caseMst.DebtLoans != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.DebtLoans.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.DebtLoans) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.DebtLoans));
        //                }
        //                break;
        //            case Consts.RankQues.MEDEBTMed:
        //                if (caseMst.DebtMed != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.DebtMed.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.DebtMed) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.DebtMed));
        //                }
        //                break;
        //            case Consts.RankQues.MEHeat:
        //                if (caseMst.ExpHeat != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.ExpHeat.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpHeat) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpHeat));
        //                }
        //                break;
        //            case Consts.RankQues.MEligDate:
        //                if (caseMst.EligDate != string.Empty)
        //                {
        //                    dr["FieldDesc"] = LookupDataAccess.Getdate(caseMst.EligDate);
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.EligDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.EligDate).Date);
        //                }
        //                break;
        //            case Consts.RankQues.MELiveExpenses:
        //                if (caseMst.ExpLivexpense != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.ExpLivexpense.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpLivexpense) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpLivexpense));

        //                }
        //                break;
        //            case Consts.RankQues.MERent:
        //                if (caseMst.ExpRent != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.ExpRent.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpRent) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpRent));
        //                }
        //                break;
        //            case Consts.RankQues.METotal:
        //                if (caseMst.ExpTotal != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.ExpTotal.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpTotal) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpTotal));
        //                }
        //                break;
        //            case Consts.RankQues.MEWater:
        //                if (caseMst.ExpWater != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.ExpWater.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpWater) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpWater));
        //                }
        //                break;

        //            case Consts.RankQues.MExpCaseworker:
        //                dr["FieldDesc"] = caseMst.ExpCaseWorker.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.ExpCaseWorker.Trim());
        //                break;
        //            case Consts.RankQues.MFamilyType:
        //                dr["FieldDesc"] = caseMst.FamilyType.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.FamilyType.Trim());
        //                break;
        //            case Consts.RankQues.MFamIncome:
        //                if (caseMst.FamIncome != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.FamIncome.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.FamIncome) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.FamIncome));
        //                }
        //                break;
        //            case Consts.RankQues.MHousing:
        //                dr["FieldDesc"] = caseMst.Housing.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Housing.Trim());
        //                break;
        //            case Consts.RankQues.MHud:
        //                if (caseMst.Hud != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.Hud.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Hud) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Hud));
        //                }
        //                break;

        //            case Consts.RankQues.MIncomeTypes:
        //                dr["FieldDesc"] = caseMst.IncomeTypes.Trim();
        //                intRankSnpPoints = fillAlertIncomeCodes(caseMst.IncomeTypes, RnkQuesFledsDataEntity, rnkQuesData.RankFldName.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                //RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.IncomeTypes.Trim());
        //                break;
        //            case Consts.RankQues.MInitialDate:
        //                if (caseMst.InitialDate != string.Empty)
        //                {
        //                    dr["FieldDesc"] = LookupDataAccess.Getdate(caseMst.InitialDate);
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.InitialDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.InitialDate).Date);
        //                }
        //                break;
        //            case Consts.RankQues.MIntakeDate:
        //                if (caseMst.IntakeDate != string.Empty)
        //                {
        //                    dr["FieldDesc"] = LookupDataAccess.Getdate(caseMst.IntakeDate);
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.IntakeDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.IntakeDate).Date);
        //                }
        //                break;
        //            case Consts.RankQues.MIntakeWorker:
        //                dr["FieldDesc"] = caseMst.IntakeWorker.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.IntakeWorker.Trim());
        //                break;
        //            case Consts.RankQues.MJuvenile:
        //                dr["FieldDesc"] = caseMst.Juvenile.Trim() == "Y" ? "Selected" : "UnSelected";
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Juvenile.Trim());
        //                break;
        //            case Consts.RankQues.MLanguageOt:
        //                dr["FieldDesc"] = caseMst.LanguageOt.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.LanguageOt.Trim());
        //                break;
        //            case Consts.RankQues.MNoInprog:
        //                if (caseMst.NoInProg != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.NoInProg.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.NoInProg) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.NoInProg));
        //                }
        //                break;
        //            case Consts.RankQues.Mpoverty:
        //                if (caseMst.Poverty != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.Poverty.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Poverty) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Poverty));
        //                    // RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Poverty.Trim());
        //                }
        //                break;
        //            case Consts.RankQues.MProgIncome:
        //                if (caseMst.ProgIncome != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.ProgIncome.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ProgIncome) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ProgIncome));
        //                }
        //                break;
        //            case Consts.RankQues.MReverifyDate:
        //                if (caseMst.ReverifyDate != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.ReverifyDate.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.ReverifyDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.ReverifyDate).Date);
        //                }
        //                break;
        //            case Consts.RankQues.MSECRET:
        //                dr["FieldDesc"] = caseMst.Secret.Trim() == "Y" ? "Selected" : "UnSelected";
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Secret.Trim());
        //                break;
        //            case Consts.RankQues.MSenior:
        //                dr["FieldDesc"] = caseMst.Senior.Trim() == "Y" ? "Selected" : "UnSelected";
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Senior.Trim());
        //                break;
        //            case Consts.RankQues.MSite:
        //                dr["FieldDesc"] = caseMst.Site.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Site.Trim());
        //                break;
        //            case Consts.RankQues.MSMi:
        //                if (caseMst.Smi != string.Empty)
        //                {
        //                    dr["FieldDesc"] = caseMst.Smi.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Smi) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Smi));
        //                }
        //                break;
        //            case Consts.RankQues.MVefiryCheckstub:
        //                // {
        //                dr["FieldDesc"] = caseMst.VerifyCheckStub.Trim() == "Y" ? "Selected" : "UnSelected";
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyCheckStub.Trim());
        //                //}
        //                break;
        //            case Consts.RankQues.MVerifier:
        //                dr["FieldDesc"] = caseMst.Verifier.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Verifier.Trim());
        //                break;
        //            case Consts.RankQues.MVerifyW2:
        //                dr["FieldDesc"] = caseMst.VerifyW2.Trim() == "Y" ? "Selected" : "UnSelected";
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyW2.Trim());
        //                break;
        //            case Consts.RankQues.MVeriTaxReturn:
        //                dr["FieldDesc"] = caseMst.VerifyTaxReturn.Trim() == "Y" ? "Selected" : "UnSelected";
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyTaxReturn.Trim());
        //                break;
        //            case Consts.RankQues.MVerLetter:
        //                dr["FieldDesc"] = caseMst.VerifyLetter.Trim() == "Y" ? "Selected" : "UnSelected";
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyLetter.Trim());
        //                break;
        //            case Consts.RankQues.MVerOther:
        //                dr["FieldDesc"] = caseMst.VerifyOther.Trim() == "Y" ? "Selected" : "UnSelected";
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyOther.Trim());
        //                break;
        //            case Consts.RankQues.MWaitList:
        //                dr["FieldDesc"] = caseMst.WaitList.Trim();
        //                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.WaitList.Trim());
        //                break;
        //            case Consts.RankQues.SEducation:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Education.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                //List<string> SnpFieldsCodesList = new List<string>();
        //                //List<string> SnpFieldsRelationList = new List<string>();
        //                //for (int i = 0; i < caseSnp.Count; i++)
        //                //{
        //                //    SnpFieldsCodesList.Add(caseSnp[i].Education);
        //                //    SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
        //                //}
        //                //intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.S1shift:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).IstShift.ToString();
        //                dr["FieldDesc"] = strApplicationcode == "Y" ? "Selected" : "UnSelected";
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.S2ndshift:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).IIndShift.ToString();
        //                dr["FieldDesc"] = strApplicationcode == "Y" ? "Selected" : "UnSelected";
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.S3rdShift:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).IIIrdShift.ToString();
        //                dr["FieldDesc"] = strApplicationcode == "Y" ? "Selected" : "UnSelected";
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SAge:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Age.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SAltBdate:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).AltBdate.ToString();
        //                dr["FieldDesc"] = LookupDataAccess.Getdate(strApplicationcode);

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SDisable:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Disable.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SDrvlic:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Drvlic.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SEmployed:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Employed.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SEthinic:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Ethnic.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SExpireWorkDate:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).ExpireWorkDate.ToString();
        //                dr["FieldDesc"] = LookupDataAccess.Getdate(strApplicationcode);

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SFarmer:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Farmer.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SFoodStamps:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).FootStamps.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SFThours:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).FullTimeHours.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SHealthIns:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).HealthIns.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SHireDate:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).HireDate.ToString();
        //                dr["FieldDesc"] = LookupDataAccess.Getdate(strApplicationcode);

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SHourlyWage:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).HourlyWage.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SjobCategory:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).JobCategory.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SjobTitle:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).JobTitle.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SLastWorkDate:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).LastWorkDate.ToString();
        //                dr["FieldDesc"] = LookupDataAccess.Getdate(strApplicationcode);

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SLegalTowork:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).LegalTowork.ToString();
        //                dr["FieldDesc"] = strApplicationcode;

        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SMartialStatus:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).MaritalStatus.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SMemberCode:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).MemberCode.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SNofcjob:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).NumberOfcjobs.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SNofljobs:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).NumberofLvjobs.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SPFrequency:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).PayFrequency.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SPregnant:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Pregnant.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SPThours:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).PartTimeHours.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SRace:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Race.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SRelitran:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Relitran.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SResident:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Resident.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SRshift:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).RShift.ToString();
        //                dr["FieldDesc"] = strApplicationcode == "Y" ? "Selected" : "UnSelected";
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SSchoolDistrict:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).SchoolDistrict.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SSEmploy:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Employed.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SSex:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Sex.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SSnpVet:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Vet.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SStatus:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Status.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.STranserv:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Transerv.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SWic:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Wic.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.SworkLimit:
        //                RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
        //                strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).WorkLimit.ToString();
        //                dr["FieldDesc"] = strApplicationcode;
        //                intRankSnpPoints = CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
        //                intRankPoint = intRankPoint + intRankSnpPoints;
        //                break;
        //            case Consts.RankQues.CDentalCoverage:
        //                if (chldMst != null)
        //                {
        //                    dr["FieldDesc"] = chldMst.DentalCoverage.Trim() == "True" ? "Seleted" : "UnSelected";
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == (chldMst.DentalCoverage.Trim() == "True" ? "Y" : "N"));
        //                }
        //                break;
        //            case Consts.RankQues.CDiagNosisDate:
        //                if (chldMst != null)
        //                    if (chldMst.DiagnosisDate != string.Empty)
        //                    {
        //                        dr["FieldDesc"] = LookupDataAccess.Getdate(chldMst.DiagnosisDate);
        //                        RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(chldMst.DiagnosisDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(chldMst.DiagnosisDate).Date);
        //                    }
        //                break;
        //            case Consts.RankQues.CDisability:
        //                if (chldMst != null)
        //                {
        //                    dr["FieldDesc"] = chldMst.Disability.Trim() == "True" ? "Seleted" : "UnSelected";
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == (chldMst.Disability.Trim() == "True" ? "Y" : "N"));
        //                }
        //                break;
        //            case Consts.RankQues.CInsCat:
        //                if (chldMst != null)
        //                {
        //                    dr["FieldDesc"] = chldMst.InsCat.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == chldMst.InsCat.Trim());
        //                }
        //                break;
        //            case Consts.RankQues.CMedCoverage:
        //                if (chldMst != null)
        //                {
        //                    dr["FieldDesc"] = chldMst.MedCoverage.Trim() == "True" ? "Seleted" : "UnSelected";
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == (chldMst.MedCoverage.Trim() == "True" ? "Y" : "N"));
        //                }
        //                break;
        //            case Consts.RankQues.CMedicalCoverageType:
        //                if (chldMst != null)
        //                {
        //                    dr["FieldDesc"] = chldMst.MedCoverType.Trim();
        //                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == chldMst.MedCoverType.Trim());
        //                }
        //                break;


        //        }

        //        if (RnkQuesSearch != null)
        //        {
        //            intRankPoint = intRankPoint + Convert.ToInt32(RnkQuesSearch.Points);
        //            dr["Points"] = RnkQuesSearch.Points;
        //            dtRankSubDetails.Rows.Add(dr);
        //        }
        //        else
        //        {
        //            dr["Points"] = intRankSnpPoints;
        //            dtRankSubDetails.Rows.Add(dr);
        //        }
        //        // }


        //        //ListRankPoints.Add(new CommonEntity(intRankCtg.ToString(), intRankPoint.ToString()));
        //    }
        //    if (custResponses.Count > 0)
        //    {
        //        try
        //        {


        //            CustomQuestionsEntity custResponcesearch = null;
        //            RNKCRIT2Entity rnkPoints = null;
        //            string strQuestionType = string.Empty;
        //            foreach (CustomQuestionsEntity responceQuestion in custResponses)
        //            {
        //                DataRow dr1 = dtRankSubDetails.NewRow();
        //                List<RNKCRIT2Entity> rnkCustFldsFilterCode = RnkCustFldsDataEntity.FindAll(u => u.RankFiledCode.Trim() == responceQuestion.ACTCODE.Trim());

        //                if (rnkCustFldsFilterCode.Count > 0)
        //                {

        //                    custResponcesearch = null;
        //                    rnkPoints = null;
        //                    strQuestionType = rnkCustFldsFilterCode[0].RankFldRespType.Trim();
        //                    dr1["FieldCode"] = rnkCustFldsFilterCode[0].RankFldDesc.Trim();

        //                    switch (rnkCustFldsFilterCode[0].RankFldRespType.Trim())
        //                    {
        //                        case "D":
        //                        case "L":
        //                            rnkPoints = rnkCustFldsFilterCode.Find(u => u.RespCd.Trim() == responceQuestion.ACTMULTRESP.Trim());
        //                            //custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && u.ACTMULTRESP.Trim() == item.RespCd.Trim());
        //                            dr1["FieldDesc"] = propCustResponceList.Find(u => u.ResoCode.Trim() == responceQuestion.ACTCODE && u.DescCode == responceQuestion.ACTMULTRESP).RespDesc.ToString();
        //                            break;
        //                        case "N":
        //                            rnkPoints = rnkCustFldsFilterCode.Find(u => Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(responceQuestion.ACTNUMRESP) && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(responceQuestion.ACTNUMRESP));
        //                            // custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && Convert.ToDecimal(u.GtNum) >= Convert.ToDecimal(item.ACTNUMRESP) && Convert.ToDecimal(u.LtNum) <= Convert.ToDecimal(item.ACTNUMRESP));
        //                            dr1["FieldDesc"] = responceQuestion.ACTNUMRESP;
        //                            break;
        //                        case "T":
        //                        case "B":
        //                            rnkPoints = rnkCustFldsFilterCode.Find(u => Convert.ToDateTime(u.GtDate) <= Convert.ToDateTime(responceQuestion.ACTDATERESP) && Convert.ToDateTime(u.LtDate) >= Convert.ToDateTime(responceQuestion.ACTDATERESP));
        //                            // custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && Convert.ToDateTime(u.GtDate) >= Convert.ToDateTime(item.ACTDATERESP) && Convert.ToDateTime(u.LtDate) <= Convert.ToDateTime(item.ACTNUMRESP));
        //                            dr1["FieldDesc"] = responceQuestion.ACTDATERESP;
        //                            break;
        //                    }
        //                    if (rnkPoints != null)
        //                    {
        //                        dr1["Points"] = rnkPoints.Points;
        //                        intRankPoint = intRankPoint + Convert.ToInt32(rnkPoints.Points);
        //                    }
        //                    else
        //                    {
        //                        dr1["Points"] = "0";
        //                    }
        //                    dtRankSubDetails.Rows.Add(dr1);

        //                }

        //            }


        //            //********* Old Code  *********


        //            //CustomQuestionsEntity custResponcesearch = null;
        //            //string strQuestionType = string.Empty;
        //            //foreach (CustomQuestionsEntity responceQuestion in custResponses)
        //            //{
        //            //    DataRow dr1 = dtRankSubDetails.NewRow();
        //            //    foreach (RNKCRIT2Entity item in RnkCustFldsDataEntity)
        //            //    {

        //            //        if (responceQuestion.ACTCODE.Trim() == item.RankFiledCode.Trim())
        //            //        {
        //            //            custResponcesearch = null;
        //            //            strQuestionType = item.RankFldRespType.Trim();
        //            //            dr1["FieldCode"] = item.RankFldDesc.Trim();

        //            //            switch (item.RankFldRespType.Trim())
        //            //            {
        //            //                case "D":
        //            //                case "L":
        //            //                    custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && u.ACTMULTRESP.Trim() == item.RespCd.Trim());
        //            //                    CustRespEntity custResponce = propCustResponceList.Find(u => u.ResoCode.Trim() == responceQuestion.ACTCODE && u.DescCode == responceQuestion.ACTMULTRESP);
        //            //                    if (custResponce != null)
        //            //                        dr1["FieldDesc"] = custResponce.RespDesc.ToString();
        //            //                    else
        //            //                        dr1["FieldDesc"] = string.Empty;
        //            //                    break;
        //            //                case "N":
        //            //                    custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && Convert.ToDecimal(u.ACTNUMRESP) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(u.ACTNUMRESP) <= Convert.ToDecimal(item.LtNum));
        //            //                    dr1["FieldDesc"] = responceQuestion.ACTNUMRESP;
        //            //                    break;
        //            //                case "T":
        //            //                case "B":
        //            //                    custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && Convert.ToDateTime(u.ACTDATERESP) >= Convert.ToDateTime(item.GtDate) && Convert.ToDateTime(u.ACTNUMRESP) <= Convert.ToDateTime(item.LtDate));
        //            //                    dr1["FieldDesc"] = responceQuestion.ACTDATERESP;
        //            //                    break;
        //            //            }
        //            //            if (custResponcesearch != null)
        //            //            {
        //            //                dr1["Points"] = item.Points;
        //            //                intRankPoint = intRankPoint + Convert.ToInt32(item.Points);
        //            //            }
        //            //            else
        //            //            {
        //            //                dr1["Points"] = "0";
        //            //            }
        //            //            dtRankSubDetails.Rows.Add(dr1);
        //            //            break;
        //            //        }
        //            //    }

        //            //}

        //        }
        //        catch (Exception ex)
        //        {


        //        }
        //    }
        //    //foreach (CommonEntity item in ListRankPoints)
        //    //{
        //    //   // txtProcess.Text = txtProcess.Text + item.Code + ":" + item.Desc + ",";
        //    //}
        //    return dtRankSubDetails;
        //}

        //private int fillAlertIncomeCodes(string alertCodes, List<RNKCRIT2Entity> rnkSearchEntity, string FieldName)
        //{
        //    int intAlertcode = 0;
        //    List<string> AlertList = new List<string>();
        //    if (alertCodes != null)
        //    {
        //        string[] incomeTypes = alertCodes.Split(' ');
        //        for (int i = 0; i < incomeTypes.Length; i++)
        //        {
        //            AlertList.Add(incomeTypes.GetValue(i).ToString());
        //        }
        //    }
        //    List<RNKCRIT2Entity> RnkAlertCode = rnkSearchEntity.FindAll(u => u.RankFldName.Trim() == FieldName);

        //    foreach (RNKCRIT2Entity rnkEntity in RnkAlertCode)
        //    {
        //        if (alertCodes != null && AlertList.Contains(rnkEntity.RespCd))
        //        {
        //            intAlertcode = intAlertcode + Convert.ToInt32(rnkEntity.Points);
        //        }
        //    }
        //    return intAlertcode;
        //}


        //private int CaseSnpDetailsCalc(List<RNKCRIT2Entity> rnkCaseSnp, List<CaseSnpEntity> caseSnpDetails, string strApplicantCode, string FilterCode, string ResponceType)
        //{
        //    int intSnpPoints = 0;
        //    foreach (RNKCRIT2Entity item in rnkCaseSnp)
        //    {
        //        if (item.CountInd.Trim() == "A")
        //        {
        //            switch (ResponceType)
        //            {
        //                case "D":
        //                case "L":
        //                    if (item.RespCd.Trim() == strApplicantCode)
        //                    {
        //                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                    }
        //                    break;
        //                case "N":
        //                    if (strApplicantCode != string.Empty)
        //                        if (Convert.ToDecimal(strApplicantCode) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(strApplicantCode) <= Convert.ToDecimal(item.LtNum))
        //                        {
        //                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                        }
        //                    break;
        //                case "B":
        //                case "T":
        //                    if (strApplicantCode != string.Empty)
        //                        if (Convert.ToDateTime(strApplicantCode).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(strApplicantCode).Date <= Convert.ToDateTime(item.LtDate).Date)
        //                        {
        //                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                        }
        //                    break;

        //            }

        //        }
        //        else if (item.CountInd.Trim() == "M")
        //        {
        //            if (item.Relation == "*")
        //            {
        //                int count = 0;
        //                switch (FilterCode)
        //                {
        //                    case Consts.RankQues.S1shift:
        //                        count = caseSnpDetails.FindAll(u => u.IstShift.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.S2ndshift:
        //                        count = caseSnpDetails.FindAll(u => u.IIndShift.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.S3rdShift:
        //                        count = caseSnpDetails.FindAll(u => u.IIIrdShift.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SAge:
        //                        foreach (CaseSnpEntity snpDate in caseSnpDetails)
        //                        {
        //                            if (snpDate.AltBdate != string.Empty)
        //                            {
        //                                DateTime EndDate = GetEndDateAgeCalculation(item.AgeClcInd.Trim(), propMstRank);
        //                                int AgeMonth = _model.lookupDataAccess.GetAgeCalculationMonths(Convert.ToDateTime(snpDate.AltBdate), EndDate);
        //                                if (AgeMonth >= Convert.ToDecimal(item.GtNum) && AgeMonth <= Convert.ToDecimal(item.LtNum))
        //                                {
        //                                    count = count + 1;
        //                                }
        //                            }
        //                        }
        //                        break;
        //                    case Consts.RankQues.SAltBdate:
        //                        foreach (CaseSnpEntity snpDate in caseSnpDetails)
        //                        {
        //                            if (snpDate.AltBdate != string.Empty)
        //                                if (Convert.ToDateTime(snpDate.AltBdate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.AltBdate).Date <= Convert.ToDateTime(item.LtDate).Date)
        //                                {
        //                                    count = count + 1;
        //                                }
        //                        }

        //                        break;
        //                    case Consts.RankQues.SSchoolDistrict:
        //                        count = caseSnpDetails.FindAll(u => u.SchoolDistrict.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SEducation:
        //                        count = caseSnpDetails.FindAll(u => u.Education.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SWic:
        //                        count = caseSnpDetails.FindAll(u => u.Wic.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SDisable:
        //                        count = caseSnpDetails.FindAll(u => u.Disable.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SDrvlic:
        //                        count = caseSnpDetails.FindAll(u => u.Drvlic.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SEmployed:
        //                        count = caseSnpDetails.FindAll(u => u.Employed.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SEthinic:
        //                        count = caseSnpDetails.FindAll(u => u.Ethnic.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SExpireWorkDate:
        //                        foreach (CaseSnpEntity snpDate in caseSnpDetails)
        //                        {
        //                            if (snpDate.ExpireWorkDate != string.Empty)
        //                                if (Convert.ToDateTime(snpDate.ExpireWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.ExpireWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
        //                                {
        //                                    count = count + 1;
        //                                }
        //                        }
        //                        break;
        //                    case Consts.RankQues.SFarmer:
        //                        count = caseSnpDetails.FindAll(u => u.Farmer.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SFoodStamps:
        //                        count = caseSnpDetails.FindAll(u => u.FootStamps.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SFThours:
        //                        foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
        //                        {
        //                            if (snpNumeric.FullTimeHours != string.Empty)
        //                                if (Convert.ToDecimal(snpNumeric.FullTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.FullTimeHours) <= Convert.ToDecimal(item.LtNum))
        //                                {
        //                                    count = count + 1;
        //                                }
        //                        }
        //                        break;
        //                    case Consts.RankQues.SHealthIns:
        //                        count = caseSnpDetails.FindAll(u => u.HealthIns.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SHireDate:
        //                        foreach (CaseSnpEntity snpDate in caseSnpDetails)
        //                        {
        //                            if (snpDate.HireDate != string.Empty)
        //                                if (Convert.ToDateTime(snpDate.HireDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.HireDate).Date <= Convert.ToDateTime(item.LtDate).Date)
        //                                {
        //                                    count = count + 1;
        //                                }
        //                        }
        //                        break;
        //                    case Consts.RankQues.SHourlyWage:
        //                        foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
        //                        {
        //                            if (snpNumeric.HourlyWage != string.Empty)
        //                                if (Convert.ToDecimal(snpNumeric.HourlyWage) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.HourlyWage) <= Convert.ToDecimal(item.LtNum))
        //                                {
        //                                    count = count + 1;
        //                                }
        //                        }
        //                        break;
        //                    case Consts.RankQues.SjobCategory:
        //                        count = caseSnpDetails.FindAll(u => u.JobCategory.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SjobTitle:
        //                        count = caseSnpDetails.FindAll(u => u.JobTitle.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SLastWorkDate:
        //                        foreach (CaseSnpEntity snpDate in caseSnpDetails)
        //                        {
        //                            if (snpDate.LastWorkDate != string.Empty)
        //                                if (Convert.ToDateTime(snpDate.LastWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.LastWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
        //                                {
        //                                    count = count + 1;
        //                                }
        //                        }
        //                        break;
        //                    case Consts.RankQues.SLegalTowork:
        //                        count = caseSnpDetails.FindAll(u => u.LegalTowork.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SMartialStatus:
        //                        count = caseSnpDetails.FindAll(u => u.MaritalStatus.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SMemberCode:
        //                        count = caseSnpDetails.FindAll(u => u.MemberCode.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SNofcjob:
        //                        foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
        //                        {
        //                            if (snpNumeric.NumberOfcjobs != string.Empty)
        //                                if (Convert.ToDecimal(snpNumeric.NumberOfcjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberOfcjobs) <= Convert.ToDecimal(item.LtNum))
        //                                {
        //                                    count = count + 1;
        //                                }
        //                        }
        //                        break;
        //                    case Consts.RankQues.SNofljobs:
        //                        foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
        //                        {
        //                            if (snpNumeric.NumberofLvjobs != string.Empty)
        //                                if (Convert.ToDecimal(snpNumeric.NumberofLvjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberofLvjobs) <= Convert.ToDecimal(item.LtNum))
        //                                {
        //                                    count = count + 1;
        //                                }
        //                        }
        //                        break;
        //                    case Consts.RankQues.SPFrequency:
        //                        count = caseSnpDetails.FindAll(u => u.PayFrequency.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SPregnant:
        //                        count = caseSnpDetails.FindAll(u => u.Pregnant.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SPThours:
        //                        foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
        //                        {
        //                            if (snpNumeric.PartTimeHours != string.Empty)
        //                                if (Convert.ToDecimal(snpNumeric.PartTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.PartTimeHours) <= Convert.ToDecimal(item.LtNum))
        //                                {
        //                                    count = count + 1;
        //                                }
        //                        }
        //                        break;
        //                    case Consts.RankQues.SRace:
        //                        count = caseSnpDetails.FindAll(u => u.Race.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SRelitran:
        //                        count = caseSnpDetails.FindAll(u => u.Relitran.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SResident:
        //                        count = caseSnpDetails.FindAll(u => u.Resident.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SRshift:
        //                        count = caseSnpDetails.FindAll(u => u.RShift.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SSEmploy:
        //                        count = caseSnpDetails.FindAll(u => u.SeasonalEmploy.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SSex:
        //                        count = caseSnpDetails.FindAll(u => u.Sex.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SSnpVet:
        //                        count = caseSnpDetails.FindAll(u => u.Vet.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SStatus:
        //                        count = caseSnpDetails.FindAll(u => u.Status.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.STranserv:
        //                        count = caseSnpDetails.FindAll(u => u.Transerv.Trim().Equals(item.RespCd)).Count;
        //                        break;
        //                    case Consts.RankQues.SworkLimit:
        //                        count = caseSnpDetails.FindAll(u => u.WorkLimit.Trim().Equals(item.RespCd)).Count;
        //                        break;

        //                }

        //                if (caseSnpDetails.Count == count)
        //                    intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //            }
        //            else
        //            {
        //                switch (ResponceType)
        //                {
        //                    case "D":
        //                    case "L":
        //                        foreach (CaseSnpEntity snpdropdown in caseSnpDetails)
        //                        {

        //                            switch (FilterCode)
        //                            {
        //                                case Consts.RankQues.S1shift:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.IstShift.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.S2ndshift:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.IIndShift.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.S3rdShift:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.IIIrdShift.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SSchoolDistrict:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.SchoolDistrict.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SEducation:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Education.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SWic:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Wic.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SDisable:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Disable.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SDrvlic:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Drvlic.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SEmployed:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Employed.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SEthinic:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Ethnic.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SFarmer:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Farmer.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SFoodStamps:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.FootStamps.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SHealthIns:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.HealthIns.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SjobCategory:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.JobCategory.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SjobTitle:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.JobTitle.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SLegalTowork:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.LegalTowork.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SMartialStatus:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.MaritalStatus.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SMemberCode:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.MemberCode.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SPFrequency:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.PayFrequency.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SPregnant:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Pregnant.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SRace:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Race.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SRelitran:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Relitran.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SResident:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Resident.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SRshift:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.RShift.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SSEmploy:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.SeasonalEmploy.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SSex:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Sex.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SSnpVet:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Vet.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SStatus:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Status.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.STranserv:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.Transerv.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                                case Consts.RankQues.SworkLimit:
        //                                    if (item.Relation.Trim() == snpdropdown.MemberCode && item.RespCd.Trim() == snpdropdown.WorkLimit.Trim())
        //                                    {
        //                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                    }
        //                                    break;
        //                            }
        //                        }
        //                        //if (listRelationstring.Contains(item.Relation))
        //                        //{
        //                        //    if (listCodestring.Contains(item.RespCd))
        //                        //    {
        //                        //        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                        //    }
        //                        //}
        //                        break;
        //                    case "N":
        //                        foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
        //                        {

        //                            switch (FilterCode)
        //                            {
        //                                case Consts.RankQues.SAge:
        //                                    if (snpNumeric.AltBdate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
        //                                    {
        //                                        DateTime EndDate = GetEndDateAgeCalculation(item.AgeClcInd.Trim(), propMstRank);
        //                                        int AgeMonth = _model.lookupDataAccess.GetAgeCalculationMonths(Convert.ToDateTime(snpNumeric.AltBdate), EndDate);
        //                                        if (AgeMonth >= Convert.ToDecimal(item.GtNum) && AgeMonth <= Convert.ToDecimal(item.LtNum))
        //                                        {
        //                                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                        }
        //                                    }
        //                                    break;

        //                                case Consts.RankQues.SNofcjob:
        //                                    if (snpNumeric.NumberOfcjobs != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
        //                                        if (Convert.ToDecimal(snpNumeric.NumberOfcjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberOfcjobs) <= Convert.ToDecimal(item.LtNum))
        //                                        {
        //                                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                        }
        //                                    break;
        //                                case Consts.RankQues.SNofljobs:
        //                                    if (snpNumeric.NumberofLvjobs != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
        //                                        if (Convert.ToDecimal(snpNumeric.NumberofLvjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberofLvjobs) <= Convert.ToDecimal(item.LtNum))
        //                                        {
        //                                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                        }
        //                                    break;
        //                                case Consts.RankQues.SFThours:
        //                                    if (snpNumeric.FullTimeHours != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
        //                                        if (Convert.ToDecimal(snpNumeric.FullTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.FullTimeHours) <= Convert.ToDecimal(item.LtNum))
        //                                        {
        //                                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                        }
        //                                    break;
        //                                case Consts.RankQues.SPThours:
        //                                    if (snpNumeric.PartTimeHours != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
        //                                        if (Convert.ToDecimal(snpNumeric.PartTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.PartTimeHours) <= Convert.ToDecimal(item.LtNum))
        //                                        {
        //                                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                        }
        //                                    break;
        //                                case Consts.RankQues.SHourlyWage:
        //                                    if (snpNumeric.HourlyWage != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
        //                                        if (Convert.ToDecimal(snpNumeric.HourlyWage) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.HourlyWage) <= Convert.ToDecimal(item.LtNum))
        //                                        {
        //                                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                        }
        //                                    break;

        //                            }
        //                        }
        //                        break;
        //                    case "B":
        //                    case "T":
        //                        foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
        //                        {

        //                            switch (FilterCode)
        //                            {
        //                                case Consts.RankQues.SAltBdate:
        //                                    if (snpNumeric.AltBdate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
        //                                        if (Convert.ToDateTime(snpNumeric.AltBdate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.AltBdate).Date <= Convert.ToDateTime(item.LtDate).Date)
        //                                        {
        //                                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                        }
        //                                    break;
        //                                case Consts.RankQues.SExpireWorkDate:
        //                                    if (snpNumeric.ExpireWorkDate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
        //                                        if (Convert.ToDateTime(snpNumeric.ExpireWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.ExpireWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
        //                                        {
        //                                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                        }
        //                                    break;
        //                                case Consts.RankQues.SLastWorkDate:
        //                                    if (snpNumeric.LastWorkDate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
        //                                        if (Convert.ToDateTime(snpNumeric.LastWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.LastWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
        //                                        {
        //                                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                        }
        //                                    break;
        //                                case Consts.RankQues.SHireDate:
        //                                    if (snpNumeric.HireDate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
        //                                        if (Convert.ToDateTime(snpNumeric.HireDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.HireDate).Date <= Convert.ToDateTime(item.LtDate).Date)
        //                                        {
        //                                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
        //                                        }
        //                                    break;


        //                            }
        //                        }
        //                        break;

        //                }


        //            }

        //        }

        //    }
        //    return intSnpPoints;
        //}

        //public DateTime GetEndDateAgeCalculation(string Type, CaseMstEntity caseMst)
        //{
        //    DateTime EndDate = DateTime.Now.Date;
        //    if (Type == "T")
        //    {
        //        EndDate = DateTime.Now.Date;
        //    }
        //    else if (Type == "I")
        //    {
        //        EndDate = Convert.ToDateTime(caseMst.IntakeDate);
        //    }
        //    else if (Type == "K")
        //    {
        //        string strDate = DateTime.Now.Date.ToShortDateString();
        //        string strYear;
        //        ZipCodeEntity zipentity = propzipCodeEntity.Find(u => u.Zcrzip.Trim().Equals(caseMst.Zip.Trim()));
        //        if (zipentity != null)
        //        {
        //            if (zipentity.Zcrhssyear.Trim() == "2")
        //            {
        //                strYear = DateTime.Now.AddYears(1).Year.ToString();
        //            }
        //            else
        //            {
        //                strYear = DateTime.Now.Year.ToString();
        //            }
        //            strDate = zipentity.Zcrhssmo + "/" + zipentity.Zcrhssday + "/" + strYear;
        //        }
        //        EndDate = Convert.ToDateTime(strDate);
        //    }
        //    return EndDate;
        //}


        #endregion


        private void fillDropdown(string Agency)
        {
            List<RankCatgEntity> rankCatg = propRankscategory.FindAll(u => u.Agency.Trim() == Agency && u.SubCode.Trim() == string.Empty);
            if (rankCatg.Count == 0)
            {
                rankCatg = propRankscategory.FindAll(u => u.Agency.Trim() == "**" && u.SubCode.Trim() == string.Empty);
            }
            foreach (RankCatgEntity item in rankCatg)
            {
                cmbRankCategory.Items.Add(new Captain.Common.Utilities.ListItem(item.Desc, item.Code));
            }
            cmbRankCategory.Items.Insert(0, new Captain.Common.Utilities.ListItem("  ", "0"));
            cmbRankCategory.SelectedIndex = 0;
            cmbCaseType.ColorMember = "FavoriteColor";
            //Vikash added on 01/24/2023 as part of enhancement
            List<CommonEntity> CaseType = _model.lookupDataAccess.GetCaseType();
            CaseType = CaseType.OrderByDescending(u => u.Active.Trim()).ToList();
            foreach (CommonEntity casetype in CaseType)
            {
                cmbCaseType.Items.Add(new Captain.Common.Utilities.ListItem(casetype.Desc, casetype.Code, casetype.Active, casetype.Active.Equals("Y") ? Color.Black : Color.Red));
            }
            cmbCaseType.Items.Insert(0, new Captain.Common.Utilities.ListItem("All", "**"));
            cmbCaseType.SelectedIndex = 0;
        }

        private void PbPdf_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            //HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", string.Empty, "*", "Reports");
            //hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            //hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            //hierarchieSelectionForm.ShowDialog();

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", string.Empty, "*", "Reports", BaseForm.UserID);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();

        }

        string Current_Hierarchy = "******", Current_Hierarchy_DB = "**-**-**";
        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
           // HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;

            HierarchieSelection form = sender as HierarchieSelection;

            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                string hierarchy = string.Empty;

                if (selectedHierarchies.Count > 0)
                {
                    foreach (HierarchyEntity row in selectedHierarchies)
                    {
                        hierarchy += (string.IsNullOrEmpty(row.Agency) ? "**" : row.Agency) + (string.IsNullOrEmpty(row.Dept) ? "**" : row.Dept) + (string.IsNullOrEmpty(row.Prog) ? "**" : row.Prog);
                    }
                    //Current_Hierarchy = hierarchy.Substring(0, 2) + "-" + hierarchy.Substring(2, 2) + "-" + hierarchy.Substring(4, 2);

                    Set_Report_Hierarchy(hierarchy.Substring(0, 2), hierarchy.Substring(2, 2), hierarchy.Substring(4, 2), string.Empty);
                    Agency = hierarchy.Substring(0, 2);
                    Dept = hierarchy.Substring(2, 2);
                    Prog = hierarchy.Substring(4, 2);

                    cmbRankCategory.Items.Clear();
                    cmbCaseType.Items.Clear();
                    fillDropdown(Agency);
                    if (Agency == "**" || Dept == "**" || Prog == "**")
                    {
                        rdoAllApplicant.Checked = true;
                        rdoSelectApplicant_CheckedChanged(sender, e);
                        rdoSelectApplicant.Enabled = false;
                    }
                    else
                    {
                        rdoSelectApplicant.Enabled = true;
                    }
                    //  propCaseMstList = _model.CaseMstData.GetCaseMstadpyn(Agency == "**" ? string.Empty : Agency, Dept == "**" ? string.Empty : Dept, Prog == "**" ? string.Empty : Prog, Program_Year.Trim(), string.Empty);

                }
            }
        }

        string Program_Year;
        private void Set_Report_Hierarchy(string Agy, string Dept, string Prog, string Year)
        {
            Txt_HieDesc.Clear();
            CmbYear.Visible = false;
            Program_Year = "    ";
            Current_Hierarchy = Agy + Dept + Prog;
            Current_Hierarchy_DB = Agy + "-" + Dept + "-" + Prog;

            if (Agy != "**")
            {
                DataSet ds_AGY = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, "**", "**");
                if (ds_AGY.Tables.Count > 0)
                {
                    if (ds_AGY.Tables[0].Rows.Count > 0)
                        Txt_HieDesc.Text += "AGY : " + Agy + " - " + (ds_AGY.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
                }
            }
            else
                Txt_HieDesc.Text += "AGY : ** - All Agencies      ";

            if (Dept != "**")
            {
                DataSet ds_DEPT = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, "**");
                if (ds_DEPT.Tables.Count > 0)
                {
                    if (ds_DEPT.Tables[0].Rows.Count > 0)
                        Txt_HieDesc.Text += "DEPT : " + Dept + " - " + (ds_DEPT.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
                }
            }
            else
                Txt_HieDesc.Text += "DEPT : ** - All Departments      ";

            if (Prog != "**")
            {
                DataSet ds_PROG = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, Prog);
                if (ds_PROG.Tables.Count > 0)
                {
                    if (ds_PROG.Tables[0].Rows.Count > 0)
                        Txt_HieDesc.Text += "PROG : " + Prog + " - " + (ds_PROG.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                }
            }
            else
                Txt_HieDesc.Text += "PROG : ** - All Programs ";


            if (Agy != "**")
                Get_NameFormat_For_Agencirs(Agy);
            else
                Member_NameFormat = CAseWorkerr_NameFormat = "1";

            if (Agy != "**" && Dept != "**" && Prog != "**")
                FillYearCombo(Agy, Dept, Prog, Year);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(814, 25);
        }

        string DepYear;
        bool DefHieExist = false;
        private void FillYearCombo(string Agy, string Dept, string Prog, string Year)
        {
            CmbYear.SelectedIndexChanged -= new EventHandler(CmbYear_SelectedIndexChanged);
            CmbYear.Items.Clear();
            CmbYear.Visible = DefHieExist = false;
            Program_Year = "    ";
            if (!string.IsNullOrEmpty(Year.Trim()))
                DefHieExist = true;

            DataSet ds = Captain.DatabaseLayer.MainMenu.GetCaseDepForHierarchy(Agy, Dept, Prog);
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                int YearIndex = 0;

                if (dt.Rows.Count > 0)
                {
                    Program_Year = DepYear = dt.Rows[0]["DEP_YEAR"].ToString();
                    if (!(String.IsNullOrEmpty(DepYear.Trim())) && DepYear != null && DepYear != "    ")
                    {
                        int TmpYear = int.Parse(DepYear);
                        int TempCompareYear = 0;
                        string TmpYearStr = null;
                        if (!(String.IsNullOrEmpty(Year)) && Year != null && Year != " " && DefHieExist)
                            TempCompareYear = int.Parse(Year);
                        List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
                        for (int i = 0; i < 10; i++)
                        {
                            TmpYearStr = (TmpYear - i).ToString();
                            listItem.Add(new Captain.Common.Utilities.ListItem(TmpYearStr, i));
                            if (TempCompareYear == (TmpYear - i) && TmpYear != 0 && TempCompareYear != 0)
                                YearIndex = i;
                        }

                        CmbYear.Items.AddRange(listItem.ToArray());

                        CmbYear.Visible = true;

                        if (DefHieExist)
                            CmbYear.SelectedIndex = YearIndex;
                        else
                            CmbYear.SelectedIndex = 0;
                    }
                }
            }

            if (!string.IsNullOrEmpty(Program_Year.Trim()))
                this.Txt_HieDesc.Size = new System.Drawing.Size(744, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(814, 25);

            CmbYear.SelectedIndexChanged += new EventHandler(CmbYear_SelectedIndexChanged);
        }

        string Member_NameFormat = "1", CAseWorkerr_NameFormat = "1";
        private void Get_NameFormat_For_Agencirs(string Agency)
        {
            DataSet ds = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agency, "**", "**");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Member_NameFormat = ds.Tables[0].Rows[0]["HIE_CN_FORMAT"].ToString();
                    CAseWorkerr_NameFormat = ds.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();
                }
            }

        }


        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program_Year = "    ";
            if (!(string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString())))
                Program_Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
        }

        private void btnGeneratePdf_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "PDF");
                pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveFormClosed);
                pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                pdfListForm.ShowDialog();

            }
            else if(ValidateForm())
                AlertBox.Show("No Records Found", MessageBoxIcon.Warning);
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (((Captain.Common.Utilities.ListItem)cmbRankCategory.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbRankCategory, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblRankCategory.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbRankCategory, null);
            }
            if (rdoAllApplicant.Checked == true)
            {
                if (((Captain.Common.Utilities.ListItem)cmbSequence.SelectedItem).Value.ToString() == "0")
                {
                    _errorProvider.SetError(cmbSequence, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSequence.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbSequence, null);
                }
            }
            else
                _errorProvider.SetError(cmbSequence, null);
            if (rdoAllApplicant.Checked == true)
            {
                if (dtFromDate.Checked == false)
                {
                    _errorProvider.SetError(dtFromDate, "Please select 'From Date'");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtFromDate, null);
                }
                if (dtToDate.Checked == false)
                {
                    _errorProvider.SetError(dtToDate, "Please select 'To Date'");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtToDate, null);
                }
            }
            if (rdoAllApplicant.Checked == true)
            {
                if (dtFromDate.Checked.Equals(true) && dtToDate.Checked.Equals(true))
                {
                    if (string.IsNullOrWhiteSpace(dtFromDate.Text))
                    {
                        _errorProvider.SetError(dtFromDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "From Date".Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtFromDate, null);
                    }
                    if (string.IsNullOrWhiteSpace(dtToDate.Text))
                    {
                        _errorProvider.SetError(dtToDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "To Date".Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtToDate, null);
                    }
                }
            }
            if (rdoAllApplicant.Checked == true)
            {
                if (dtFromDate.Checked.Equals(true) && dtToDate.Checked.Equals(true))
                {
                    if (!string.IsNullOrEmpty(dtFromDate.Text) && (!string.IsNullOrEmpty(dtToDate.Text)))
                    {
                        if (Convert.ToDateTime(dtFromDate.Text) > Convert.ToDateTime(dtToDate.Text))
                        {
                            _errorProvider.SetError(dtFromDate, "'Start Date' should be less than or equal to 'End Date'".Replace(Consts.Common.Colon, string.Empty));
                            isValid = false;
                        }
                        else
                        {
                            _errorProvider.SetError(dtToDate, null);
                        }
                    }
                }
            }
            if (rdoSelectApplicant.Checked == true)
            {
                _errorProvider.SetError(dtToDate, null);
                _errorProvider.SetError(dtFromDate, null);
                if (txtApplicant.Text.Trim() == string.Empty)
                {
                    _errorProvider.SetError(btnBrowse, "Please Select Applicant");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(btnBrowse, null);
                }
            }


            return (isValid);
        }

        private void btnPdfPreview_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }




        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string strFolderPath = string.Empty;
        string Random_Filename = null; string PdfName = "Pdf File";
        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
        //int pageNumber = 1;
        //string PdfScreen = null, rnkCd = null, PrivrnkCd = null, Rankdesc = null;
        //string PrintText = null;
        private void On_SaveFormClosed(object sender, FormClosedEventArgs e)
        {
            List<CommonEntity> ListchildRiskPoints = new List<CommonEntity>();
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                StringBuilder strMstApplUpdate = new StringBuilder();
                string PdfName = "Pdf File";
                PdfName = form.GetFileName();

                PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
                try
                {
                    if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                    { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
                }
                catch (Exception ex)
                {
                    AlertBox.Show("Error", MessageBoxIcon.Error);
                }


                try
                {
                    string Tmpstr = PdfName + ".pdf";
                    if (File.Exists(Tmpstr))
                        File.Delete(Tmpstr);
                }
                catch (Exception ex)
                {
                    int length = 8;
                    string newFileName = System.Guid.NewGuid().ToString();
                    newFileName = newFileName.Replace("-", string.Empty);

                    Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
                }


                if (!string.IsNullOrEmpty(Random_Filename))
                    PdfName = Random_Filename;
                else
                    PdfName += ".pdf";

                FileStream fs = new FileStream(PdfName, FileMode.Create);

                //Document document = new Document();
                Document document = new Document(PageSize.A4, 30f, 30f, 30f, 50f);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                //document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
                //PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();
                //BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                //iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
                //BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
                //iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(1, 9, 4);
                //BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

                //iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 8);
                //iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
                //iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
                //iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 9, 1);
                //iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
                //iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
                //cb = writer.DirectContent;

                //New Font Calibri applied to all data
                BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font Calibri = new iTextSharp.text.Font(bf_Calibri, 10);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8);
                iTextSharp.text.Font SubHeadFont = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_Calibri, 9, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_Calibri, 9, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_Calibri, 9, 4);
                cb = writer.DirectContent;

                //Mst Details Table
                //DataSet dsCaseMST = DatabaseLayer.CaseSnpData.GetCaseMST(Agency, Depart, Program, strYear, BaseForm.BaseApplicationNo);
                //DataRow drCaseMST = dsCaseMST.Tables[0].Rows[0];

                //Snp details Table
                //DataSet dsCaseSNP = DatabaseLayer.CaseSnpData.GetCaseSnpDetails(Agency, Depart, Program, strYear, BaseForm.BaseApplicationNo, null);
                try
                {
                    PrintHeaderPage(document, writer);

                    string strType = "I";
                    if (rdoServiceDate.Checked)
                        strType = "S";
                    else if (rdoDateAdd.Checked)
                        strType = "D";
                    if (rdoSelectApplicant.Checked == true)
                        strType = "APPLICANT";

                    string strsecurty = "A";

                    string strSequence = "1";
                    if (rdoAllApplicant.Checked == true)
                        strSequence = ((Captain.Common.Utilities.ListItem)cmbSequence.SelectedItem).Value.ToString();

                    string strCaseType = string.Empty;
                    if (((Captain.Common.Utilities.ListItem)cmbCaseType.SelectedItem).Value.ToString() != "**")
                        strCaseType = ((Captain.Common.Utilities.ListItem)cmbCaseType.SelectedItem).Value.ToString();

                    if (rdoBothNonSecret.Checked == true)
                        strsecurty = "A";
                    else if (rdoSecret.Checked == true)
                        strsecurty = "Y";
                    else if (rdoNonSecreat.Checked == true)
                        strsecurty = "N";
                    List<CaseMstEntity> caseMstEntityReport = _model.CaseMstData.GetCaseMstReportcase2530(Agency == "**" ? string.Empty : Agency, Dept == "**" ? string.Empty : Dept, Prog == "**" ? string.Empty : Prog, Program_Year.Trim(), txtApplicant.Text.Trim(), strsecurty, strSequence, strType, strCaseType, dtFromDate.Value.Date.ToShortDateString(), dtToDate.Value.Date.ToShortDateString());
                    StringBuilder strMstAppl = new StringBuilder();

                    if (rdoDateAdd.Checked)
                    {
                        if (((Captain.Common.Utilities.ListItem)cmbSequence.SelectedItem).Value.ToString() == "5")
                            caseMstEntityReport = caseMstEntityReport.OrderBy(u => (u.ApplNo)).ToList();
                        if (((Captain.Common.Utilities.ListItem)cmbSequence.SelectedItem).Value.ToString() == "6")
                            caseMstEntityReport = caseMstEntityReport.OrderByDescending(u => u.ApplNo).ToList();
                    }
                    else
                    {
                        caseMstEntityReport = caseMstEntityReport.FindAll(u => u.IntakeDate != string.Empty);
                        if (((Captain.Common.Utilities.ListItem)cmbSequence.SelectedItem).Value.ToString() == "5")
                            caseMstEntityReport = caseMstEntityReport.OrderBy(u => Convert.ToDateTime(u.IntakeDate)).ToList();
                        if (((Captain.Common.Utilities.ListItem)cmbSequence.SelectedItem).Value.ToString() == "6")
                            caseMstEntityReport = caseMstEntityReport.OrderByDescending(u => Convert.ToDateTime(u.IntakeDate)).ToList();
                    }


                    strMstAppl.Append("<Applicants>");
                    strMstApplUpdate.Append("<Applicants>");
                    foreach (CaseMstEntity item in caseMstEntityReport)
                    {
                        strMstAppl.Append("<Details MSTApplDetails = \"" + item.ApplAgency + item.ApplDept + item.ApplProgram + (item.ApplYr.Trim() == string.Empty ? "    " : item.ApplYr) + item.ApplNo + "\"/>");
                    }
                    strMstAppl.Append("</Applicants>");







                    document.NewPage();
                    Y_Pos = 795;

                    PdfPTable casb0530_Table = new PdfPTable(8);
                    casb0530_Table.TotalWidth = 550f;
                    casb0530_Table.WidthPercentage = 100;
                    casb0530_Table.LockedWidth = true;
                    float[] widths = new float[] { 37f, 25f, 23f, 50f, 26f, 68f, 24f, 25f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                    casb0530_Table.SetWidths(widths);
                    casb0530_Table.HorizontalAlignment = Element.ALIGN_CENTER;
                    casb0530_Table.HeaderRows = 1;
                    if (caseMstEntityReport.Count > 0)
                    {
                        propCaseSnpList = _model.CaseMstData.GetCaseSnpReportcase2530(Agency == "**" ? string.Empty : Agency, Dept == "**" ? string.Empty : Dept, Prog == "**" ? string.Empty : Prog, Program_Year.Trim(), string.Empty, strMstAppl.ToString());
                        propChldMstEntity = _model.ChldMstData.GetChldMstcase2530Report(Agency == "**" ? string.Empty : Agency, Dept == "**" ? string.Empty : Dept, Prog == "**" ? string.Empty : Prog, Program_Year.Trim(), string.Empty, string.Empty, strMstAppl.ToString());
                        // propcustResponses = _model.CaseMstData.GetCustomQuestionAnswersRank(Agency == "**" ? string.Empty : Agency, Dept == "**" ? string.Empty : Dept, Prog == "**" ? string.Empty : Prog, Program_Year.Trim(), string.Empty, string.Empty, strMstAppl.ToString());
                        List<RankCatgEntity> rankCatgList = propRankscategory.FindAll(u => u.Agency == Agency && u.SubCode.Trim() != string.Empty && u.Code == ((Captain.Common.Utilities.ListItem)cmbRankCategory.SelectedItem).Value.ToString());
                        if (rankCatgList.Count == 0)
                            rankCatgList = propRankscategory.FindAll(u => u.Agency == "**" && u.SubCode.Trim() != string.Empty && u.Code == ((Captain.Common.Utilities.ListItem)cmbRankCategory.SelectedItem).Value.ToString());
                        PdfPCell Header_1 = new PdfPCell(new Phrase("Points Category", TblFontBold));
                        Header_1.HorizontalAlignment = Element.ALIGN_LEFT;
                        //Header.Colspan = 4;
                        Header_1.FixedHeight = 15f;
                        Header_1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        Header_1.BorderColor = BaseColor.WHITE;
                        casb0530_Table.AddCell(Header_1);

                        PdfPCell Header = new PdfPCell(new Phrase("Hierarchy", TblFontBold));
                        Header.HorizontalAlignment = Element.ALIGN_LEFT;
                        //Header.Colspan = 4;
                        Header.FixedHeight = 15f;
                        Header.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));   // Column Header background
                        Header.BorderColor = BaseColor.WHITE;
                        casb0530_Table.AddCell(Header);

                        PdfPCell Header1 = new PdfPCell(new Phrase("App #", TblFontBold));
                        Header1.HorizontalAlignment = Element.ALIGN_LEFT;
                        Header1.FixedHeight = 15f;
                        Header1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        Header1.BorderColor = BaseColor.WHITE;
                        casb0530_Table.AddCell(Header1);


                        PdfPCell Header3 = new PdfPCell(new Phrase("Client Name", TblFontBold));
                        Header3.HorizontalAlignment = Element.ALIGN_LEFT;
                        Header3.FixedHeight = 15f;
                        Header3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        Header3.BorderColor = BaseColor.WHITE;
                        casb0530_Table.AddCell(Header3);

                        PdfPCell Header4 = new PdfPCell(new Phrase("Age", TblFontBold));
                        Header4.HorizontalAlignment = Element.ALIGN_LEFT;
                        Header4.FixedHeight = 15f;
                        Header4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        Header4.BorderColor = BaseColor.WHITE;
                        casb0530_Table.AddCell(Header4);

                        PdfPCell Header5 = new PdfPCell(new Phrase("Address", TblFontBold));
                        Header5.HorizontalAlignment = Element.ALIGN_LEFT;
                        Header5.FixedHeight = 15f;
                        Header5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        Header5.BorderColor = BaseColor.WHITE;
                        casb0530_Table.AddCell(Header5);

                        PdfPCell Header2 = new PdfPCell(new Phrase("Date Add", TblFontBold));
                        Header2.HorizontalAlignment = Element.ALIGN_LEFT;
                        Header2.FixedHeight = 15f;
                        Header2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        Header2.BorderColor = BaseColor.WHITE;
                        casb0530_Table.AddCell(Header2);

                        PdfPCell Header6 = new PdfPCell(new Phrase("Intake Date", TblFontBold));
                        Header6.HorizontalAlignment = Element.ALIGN_LEFT;
                        Header6.FixedHeight = 15f;
                        Header6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        Header6.BorderColor = BaseColor.WHITE;
                        casb0530_Table.AddCell(Header6);

                        List<CommonEntity> commonAllPoints = new List<CommonEntity>();
                        List<RNKCRIT2Entity> RnkQuesFledsEntity = _model.SPAdminData.GetRanksCrit2Data("RANKQUES", Agency, ((Captain.Common.Utilities.ListItem)cmbRankCategory.SelectedItem).Value.ToString());
                        List<RNKCRIT2Entity> RnkQuesFledsAllDataEntity = _model.SPAdminData.GetRanksCrit2Data(string.Empty, Agency, ((Captain.Common.Utilities.ListItem)cmbRankCategory.SelectedItem).Value.ToString());
                        List<RNKCRIT2Entity> RnkCustFldsAllDataEntity = _model.SPAdminData.GetRanksCrit2Data("CUSTFLDS", Agency, ((Captain.Common.Utilities.ListItem)cmbRankCategory.SelectedItem).Value.ToString());
                        bool boolPoints = true;
                        string strCategory = string.Empty;
                        // var v = caseMstEntityReport.Take(5000);

                        foreach (CaseMstEntity itemMainMst in caseMstEntityReport)
                        {
                            ChldMstEntity chldMst = propChldMstEntity.Find(u => (u.ChldMstAgency.ToString() == itemMainMst.ApplAgency.ToString()) && (u.ChldMstDept.ToString() == itemMainMst.ApplDept.ToString()) && (u.ChldMstProgram.ToString() == itemMainMst.ApplProgram.ToString()) && (u.ChldMstYr.Trim().ToString() == itemMainMst.ApplYr.Trim().ToString()) && (u.ApplNo.Trim() == itemMainMst.ApplNo));
                            List<CaseMstEntity> casemst = caseMstEntityReport.FindAll(u => (u.ApplAgency.ToString() == itemMainMst.ApplAgency.ToString()) && (u.ApplDept.ToString() == itemMainMst.ApplDept.ToString()) && (u.ApplProgram.ToString() == itemMainMst.ApplProgram.ToString()) && (u.ApplYr.Trim().ToString() == itemMainMst.ApplYr.Trim().ToString()) && (u.ApplNo.Trim() == itemMainMst.ApplNo));
                            List<CaseSnpEntity> casesnp = propCaseSnpList.FindAll(u => (u.Agency.ToString() == itemMainMst.ApplAgency.ToString()) && (u.Dept.ToString() == itemMainMst.ApplDept.ToString()) && (u.Program.ToString() == itemMainMst.ApplProgram.ToString()) && (u.Year.Trim().ToString() == itemMainMst.ApplYr.Trim().ToString()) && (u.App.Trim() == itemMainMst.ApplNo));
                            intRankPoint = 0;
                            //if (itemMainMst.ApplNo.ToString() == "00006715")
                            //{

                            //}
                            DataTable dt = GetRankCategoryDetails(casemst[0], casesnp, chldMst, RnkQuesFledsEntity, RnkQuesFledsAllDataEntity, RnkCustFldsAllDataEntity);
                            foreach (DataRow drow in dt.Rows)
                            {
                                ListchildRiskPoints.Add(new CommonEntity(itemMainMst.ApplNo.ToString(), drow["FieldCode"].ToString(), drow["FieldDesc"].ToString(), drow["Points"].ToString(), itemMainMst.ApplYr, itemMainMst.ApplAgency, itemMainMst.ApplDept, itemMainMst.ApplProgram));
                            }
                            boolPoints = true;
                            if ((rdoClientSum.Checked == true) || (rdoClientSumDetails.Checked == true && rdoonlyzeroabove.Checked == true))
                            {
                                if (intRankPoint > 0)
                                {
                                    itemMainMst.PointsOnly = intRankPoint;
                                    boolPoints = true;
                                }
                                else
                                    boolPoints = false;
                            }
                            else
                            {
                                itemMainMst.PointsOnly = intRankPoint;
                            }

                            if (chkUpdatemaster.Checked == true)
                            {
                                strMstApplUpdate.Append("<Details MSTApplDetails = \"" + itemMainMst.ApplAgency + itemMainMst.ApplDept + itemMainMst.ApplProgram + (itemMainMst.ApplYr.Trim() == string.Empty ? "    " : itemMainMst.ApplYr.Trim()) + itemMainMst.ApplNo + "\" MST_RANK = \"" + ((Captain.Common.Utilities.ListItem)cmbRankCategory.SelectedItem).Value.ToString().Replace("0", "").TrimStart().TrimEnd() + "\" MST_POINTS = \"" + intRankPoint + "\"   />");
                            }

                            if (boolPoints)
                            {
                                strCategory = string.Empty;
                                commonAllPoints.Add(new CommonEntity(intRankPoint.ToString(), itemMainMst.ApplNo.ToString()));
                                if (rankCatgList.Count > 0)
                                {
                                    RankCatgEntity rnkcatgCategroyDesc = rankCatgList.Find(u => Convert.ToDecimal(u.PointsLow) <= intRankPoint && Convert.ToDecimal(u.PointsHigh) >= intRankPoint);
                                    if (rnkcatgCategroyDesc != null)
                                    {
                                        strCategory = rnkcatgCategroyDesc.Desc.ToString();
                                        itemMainMst.PointsCategory = strCategory;
                                    }

                                }

                            }
                        }


                        // New Rankpoints functionality Filter with Points Assending or desending Order

                        if (((Captain.Common.Utilities.ListItem)cmbSequence.SelectedItem).Value.ToString() == "3")
                        {
                            caseMstEntityReport = caseMstEntityReport.OrderBy(u => Convert.ToInt64(u.PointsOnly)).ToList();
                        }
                        else if (((Captain.Common.Utilities.ListItem)cmbSequence.SelectedItem).Value.ToString() == "4")
                        {
                            caseMstEntityReport = caseMstEntityReport.OrderByDescending(u => Convert.ToInt64(u.PointsOnly)).ToList();
                        }


                        foreach (CaseMstEntity itemMainMstnew in caseMstEntityReport)
                        {
                            //ChldMstEntity chldMst = propChldMstEntity.Find(u => (u.ChldMstAgency.ToString() == itemMainMst.ApplAgency.ToString()) && (u.ChldMstDept.ToString() == itemMainMst.ApplDept.ToString()) && (u.ChldMstProgram.ToString() == itemMainMst.ApplProgram.ToString()) && (u.ChldMstYr.Trim().ToString() == itemMainMst.ApplYr.Trim().ToString()) && (u.ApplNo.Trim() == itemMainMst.ApplNo));
                            //List<CaseMstEntity> casemst = caseMstEntityReport.FindAll(u => (u.ApplAgency.ToString() == itemMainMst.ApplAgency.ToString()) && (u.ApplDept.ToString() == itemMainMst.ApplDept.ToString()) && (u.ApplProgram.ToString() == itemMainMst.ApplProgram.ToString()) && (u.ApplYr.Trim().ToString() == itemMainMst.ApplYr.Trim().ToString()) && (u.ApplNo.Trim() == itemMainMst.ApplNo));
                            //List<CaseSnpEntity> casesnp = propCaseSnpList.FindAll(u => (u.Agency.ToString() == itemMainMst.ApplAgency.ToString()) && (u.Dept.ToString() == itemMainMst.ApplDept.ToString()) && (u.Program.ToString() == itemMainMst.ApplProgram.ToString()) && (u.Year.Trim().ToString() == itemMainMst.ApplYr.Trim().ToString()) && (u.App.Trim() == itemMainMst.ApplNo));
                            intRankPoint = 0;
                            //if (itemMainMstnew.ApplNo.ToString() == "00006020")
                            //{

                            //}
                            //DataTable dt = GetRankCategoryDetails(casemst[0], casesnp, chldMst, RnkQuesFledsEntity, RnkQuesFledsAllDataEntity, RnkCustFldsAllDataEntity);
                            //foreach (DataRow drow in dt.Rows)
                            //{
                            //    ListchildRiskPoints.Add(new CommonEntity(itemMainMst.ApplNo.ToString(), drow["FieldCode"].ToString(), drow["FieldDesc"].ToString(), drow["Points"].ToString()));
                            //}
                            boolPoints = true;
                            if ((rdoClientSum.Checked == true) || (rdoClientSumDetails.Checked == true && rdoonlyzeroabove.Checked == true))
                            {
                                if (Convert.ToInt32(itemMainMstnew.PointsOnly) > 0)
                                {
                                    boolPoints = true;
                                }
                                else
                                    boolPoints = false;
                            }

                            if (boolPoints)
                            {
                                //strCategory = string.Empty;
                                //commonAllPoints.Add(new CommonEntity(intRankPoint.ToString(), itemMainMst.ApplNo.ToString()));
                                //if (rankCatgList.Count > 0)
                                //{
                                //    RankCatgEntity rnkcatgCategroyDesc = rankCatgList.Find(u => Convert.ToInt32(u.PointsLow) <= intRankPoint && Convert.ToInt32(u.PointsHigh) >= intRankPoint);
                                //    if (rnkcatgCategroyDesc != null)
                                //    {
                                //        strCategory = rnkcatgCategroyDesc.Desc.ToString();
                                //        itemMainMst.PointsCategory = strCategory;
                                //    }

                                //}
                                PdfPCell pdfPoints = new PdfPCell(new Phrase(itemMainMstnew.PointsOnly.ToString() + " " + itemMainMstnew.PointsCategory, TableFont));
                                pdfPoints.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                pdfPoints.BorderColor = BaseColor.WHITE;
                                pdfPoints.FixedHeight = 15f;
                                pdfPoints.HorizontalAlignment = Element.ALIGN_LEFT;
                                casb0530_Table.AddCell(pdfPoints);

                                PdfPCell pdfHier = new PdfPCell(new Phrase(itemMainMstnew.ApplAgency + itemMainMstnew.ApplDept + itemMainMstnew.ApplProgram + " " + itemMainMstnew.ApplYr.ToString(), TableFont));
                                pdfHier.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                pdfHier.BorderColor = BaseColor.WHITE;
                                pdfHier.FixedHeight = 15f;
                                pdfHier.HorizontalAlignment = Element.ALIGN_LEFT;
                                casb0530_Table.AddCell(pdfHier);

                                PdfPCell pdfApp = new PdfPCell(new Phrase(itemMainMstnew.ApplNo.ToString(), TableFont));
                                pdfApp.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                pdfApp.BorderColor = BaseColor.WHITE;
                                pdfApp.FixedHeight = 15f;
                                pdfApp.HorizontalAlignment = Element.ALIGN_LEFT;
                                casb0530_Table.AddCell(pdfApp);



                                PdfPCell pdfName = new PdfPCell(new Phrase(itemMainMstnew.NickName.ToString(), TableFont));
                                pdfName.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                pdfName.BorderColor = BaseColor.WHITE;
                                pdfName.FixedHeight = 15f;
                                pdfName.HorizontalAlignment = Element.ALIGN_LEFT;
                                casb0530_Table.AddCell(pdfName);

                                PdfPCell pdfAge = new PdfPCell(new Phrase(itemMainMstnew.Age.ToString(), TableFont));
                                pdfAge.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                pdfAge.BorderColor = BaseColor.WHITE;
                                pdfAge.FixedHeight = 15f;
                                pdfAge.HorizontalAlignment = Element.ALIGN_LEFT;
                                casb0530_Table.AddCell(pdfAge);

                                PdfPCell pdfAddress = new PdfPCell(new Phrase(itemMainMstnew.AddressDetails.ToString(), TableFont));
                                pdfAddress.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                pdfAddress.BorderColor = BaseColor.WHITE;
                                pdfAddress.FixedHeight = 15f;
                                pdfAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                                casb0530_Table.AddCell(pdfAddress);

                                PdfPCell pdfYear = new PdfPCell(new Phrase(LookupDataAccess.Getdate(itemMainMstnew.DateAdd1.ToString()), TableFont));
                                pdfYear.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                pdfYear.BorderColor = BaseColor.WHITE;
                                pdfYear.FixedHeight = 15f;
                                pdfYear.HorizontalAlignment = Element.ALIGN_LEFT;
                                casb0530_Table.AddCell(pdfYear);

                                //string strDate;
                                //if (rdoDateAdd.Checked)
                                //    strDate = LookupDataAccess.Getdate(itemMainMst.DateAdd1.ToString());
                                //else
                                //    strDate = LookupDataAccess.Getdate(itemMainMst.IntakeDate.ToString());

                                PdfPCell pdfIntake = new PdfPCell(new Phrase(LookupDataAccess.Getdate(itemMainMstnew.IntakeDate.ToString()), TableFont));
                                pdfIntake.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                pdfIntake.BorderColor = BaseColor.WHITE;
                                pdfIntake.FixedHeight = 15f;
                                pdfIntake.HorizontalAlignment = Element.ALIGN_LEFT;
                                casb0530_Table.AddCell(pdfIntake);
                                if (rdoClientSumDetails.Checked == true)
                                {
                                    List<CommonEntity> commonSubPoints = new List<CommonEntity>();
                                    if (ListchildRiskPoints.Count > 0)
                                    {
                                        commonSubPoints = ListchildRiskPoints.FindAll(u => u.Code == itemMainMstnew.ApplNo.ToString() && u.Pyear.Trim() == itemMainMstnew.ApplYr.Trim() && u.PAgency == itemMainMstnew.ApplAgency.Trim() && u.PDept == itemMainMstnew.ApplDept.Trim() && u.Pprog == itemMainMstnew.ApplProgram.Trim());
                                        if (rdoClientSumDetails.Checked == true)
                                        {
                                            if (((Captain.Common.Utilities.ListItem)cmbSortOn.SelectedItem).Value.ToString() == "1")
                                            {
                                                commonSubPoints = commonSubPoints.OrderBy(u => u.Desc).ToList();
                                            }
                                            else if (((Captain.Common.Utilities.ListItem)cmbSortOn.SelectedItem).Value.ToString() == "2")
                                            {
                                                commonSubPoints = commonSubPoints.OrderBy(u => Convert.ToInt32(u.Extension)).ToList();
                                            }
                                            else if (((Captain.Common.Utilities.ListItem)cmbSortOn.SelectedItem).Value.ToString() == "3")
                                            {
                                                commonSubPoints = commonSubPoints.OrderByDescending(u => Convert.ToInt32(u.Extension)).ToList();
                                            }
                                        }

                                    }
                                    foreach (CommonEntity item in commonSubPoints)
                                    {
                                        if (rdoonlyzeroabove.Checked == true)
                                        {
                                            if (item.Extension != string.Empty)
                                            {
                                                if (Convert.ToInt32(item.Extension) > 0)
                                                {

                                                    PdfPCell pdfSpace = new PdfPCell(new Phrase("", TableFont));
                                                    pdfSpace.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                    pdfSpace.BorderColor = BaseColor.WHITE;
                                                    pdfSpace.FixedHeight = 15f;
                                                    pdfSpace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    casb0530_Table.AddCell(pdfSpace);

                                                    PdfPCell pdfCode = new PdfPCell(new Phrase(item.Desc.ToString(), TableFont));
                                                    pdfCode.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                    pdfCode.BorderColor = BaseColor.WHITE;
                                                    pdfCode.FixedHeight = 15f;
                                                    pdfCode.Colspan = 4;
                                                    pdfCode.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    casb0530_Table.AddCell(pdfCode);

                                                    PdfPCell pdfDesc = new PdfPCell(new Phrase(item.Hierarchy.ToString(), TableFont));
                                                    pdfDesc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                    pdfDesc.BorderColor = BaseColor.WHITE;
                                                    pdfDesc.FixedHeight = 15f;
                                                    pdfDesc.Colspan = 2;
                                                    pdfDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    casb0530_Table.AddCell(pdfDesc);

                                                    PdfPCell pdfsubPoints = new PdfPCell(new Phrase(item.Extension.ToString(), TableFont));
                                                    pdfsubPoints.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                    pdfsubPoints.BorderColor = BaseColor.WHITE;
                                                    pdfsubPoints.FixedHeight = 15f;
                                                    //pdfsubPoints.Colspan = 2;
                                                    pdfsubPoints.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    casb0530_Table.AddCell(pdfsubPoints);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            PdfPCell pdfSpace = new PdfPCell(new Phrase("", TableFont));
                                            pdfSpace.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                            pdfSpace.BorderColor = BaseColor.WHITE;
                                            pdfSpace.FixedHeight = 15f;
                                            pdfSpace.HorizontalAlignment = Element.ALIGN_LEFT;
                                            casb0530_Table.AddCell(pdfSpace);

                                            PdfPCell pdfCode = new PdfPCell(new Phrase(item.Desc.ToString(), TableFont));
                                            pdfCode.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            pdfCode.BorderColor = BaseColor.WHITE;
                                            pdfCode.FixedHeight = 15f;
                                            pdfCode.Colspan = 4;
                                            pdfCode.HorizontalAlignment = Element.ALIGN_LEFT;
                                            casb0530_Table.AddCell(pdfCode);

                                            PdfPCell pdfDesc = new PdfPCell(new Phrase(item.Hierarchy.ToString(), TableFont));
                                            pdfDesc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            pdfDesc.BorderColor = BaseColor.WHITE;
                                            pdfDesc.FixedHeight = 15f;
                                            pdfDesc.Colspan = 2;
                                            pdfDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                            casb0530_Table.AddCell(pdfDesc);

                                            PdfPCell pdfsubPoints = new PdfPCell(new Phrase(item.Extension.ToString(), TableFont));
                                            pdfsubPoints.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            pdfsubPoints.BorderColor = BaseColor.WHITE;
                                            pdfsubPoints.FixedHeight = 15f;
                                            pdfsubPoints.HorizontalAlignment = Element.ALIGN_LEFT;
                                            casb0530_Table.AddCell(pdfsubPoints);
                                        }

                                    }
                                }
                                PdfPCell pdfSpace1 = new PdfPCell(new Phrase("", TableFont));
                                pdfSpace1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                pdfSpace1.BorderColor = BaseColor.WHITE;
                                pdfSpace1.Colspan = 8;
                                pdfSpace1.FixedHeight = 15f;
                                pdfSpace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                casb0530_Table.AddCell(pdfSpace1);

                                //intRankPoint
                            }
                        }





                        // New RankPoints Ending

                        if (casb0530_Table.Rows.Count > 0)
                            document.Add(casb0530_Table);

                        if (rdoClientSumDetails.Checked == true || rdoClientSum.Checked == true)
                        {
                            document.NewPage();
                            casb0530_Table.Rows.Clear();
                            PdfPCell HeadeRange1 = new PdfPCell(new Phrase("Range", TblFontBold));
                            HeadeRange1.HorizontalAlignment = Element.ALIGN_LEFT;
                            HeadeRange1.Colspan = 2;
                            HeadeRange1.FixedHeight = 15f;
                            HeadeRange1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            HeadeRange1.BorderColor = BaseColor.WHITE;
                            casb0530_Table.AddCell(HeadeRange1);

                            PdfPCell HeaderApps1 = new PdfPCell(new Phrase("# of Apps", TblFontBold));
                            HeaderApps1.HorizontalAlignment = Element.ALIGN_LEFT;
                            // HeaderApps1.Colspan = 2;
                            HeaderApps1.FixedHeight = 15f;
                            HeaderApps1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            HeaderApps1.BorderColor = BaseColor.WHITE;
                            casb0530_Table.AddCell(HeaderApps1);

                            PdfPCell HeadeRange2 = new PdfPCell(new Phrase("Range", TblFontBold));
                            HeadeRange2.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Header.Colspan = 2;
                            HeadeRange2.FixedHeight = 15f;
                            HeadeRange2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            HeadeRange2.BorderColor = BaseColor.WHITE;
                            casb0530_Table.AddCell(HeadeRange2);

                            PdfPCell HeaderApps2 = new PdfPCell(new Phrase("# of Apps", TblFontBold));
                            HeaderApps2.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Header.Colspan = 4;
                            HeaderApps2.FixedHeight = 15f;
                            HeaderApps2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            HeaderApps2.BorderColor = BaseColor.WHITE;
                            casb0530_Table.AddCell(HeaderApps2);

                            PdfPCell HeadeRange3 = new PdfPCell(new Phrase("Range", TblFontBold));
                            HeadeRange3.HorizontalAlignment = Element.ALIGN_LEFT;
                            // Header.Colspan = 2;
                            HeadeRange3.FixedHeight = 15f;
                            HeadeRange3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            HeadeRange3.BorderColor = BaseColor.WHITE;
                            casb0530_Table.AddCell(HeadeRange3);

                            PdfPCell HeaderApps3 = new PdfPCell(new Phrase("# of Apps", TblFontBold));
                            HeaderApps3.HorizontalAlignment = Element.ALIGN_LEFT;
                            HeaderApps3.Colspan = 2;
                            HeaderApps3.FixedHeight = 15f;
                            HeaderApps3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            HeaderApps3.BorderColor = BaseColor.WHITE;
                            casb0530_Table.AddCell(HeaderApps3);

                            PdfPCell pdfRange1 = new PdfPCell(new Phrase(" 0 - 50 ", TableFont));
                            pdfRange1.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfRange1.Colspan = 2;
                            pdfRange1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            pdfRange1.BorderColor = BaseColor.WHITE;
                            pdfRange1.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfRange1);



                            PdfPCell pdfApps1 = new PdfPCell(new Phrase(commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 0 && Convert.ToInt32(u.Code) <= 50).Count.ToString(), TableFont));
                            pdfApps1.HorizontalAlignment = Element.ALIGN_LEFT;
                            // pdfApps1.Colspan = 2;
                            pdfApps1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                            pdfApps1.BorderColor = BaseColor.WHITE;
                            pdfApps1.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfApps1);

                            PdfPCell pdfRange2 = new PdfPCell(new Phrase("51 - 100", TableFont));
                            pdfRange2.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Header.Colspan = 2;
                            pdfRange2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            pdfRange2.BorderColor = BaseColor.WHITE;
                            pdfRange2.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfRange2);

                            PdfPCell pdfApps2 = new PdfPCell(new Phrase(commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 51 && Convert.ToInt32(u.Code) <= 100).Count.ToString(), TableFont));
                            pdfApps2.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Header.Colspan = 4;
                            pdfApps2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                            pdfApps2.BorderColor = BaseColor.WHITE;
                            pdfApps2.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfApps2);

                            PdfPCell pdfRange3 = new PdfPCell(new Phrase("101 - 150", TableFont));
                            pdfRange3.HorizontalAlignment = Element.ALIGN_LEFT;
                            // Header.Colspan = 2;
                            pdfRange3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            pdfRange3.BorderColor = BaseColor.WHITE;
                            pdfRange3.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfRange3);

                            PdfPCell pdfApps3 = new PdfPCell(new Phrase(commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 101 && Convert.ToInt32(u.Code) <= 150).Count.ToString(), TableFont));
                            pdfApps3.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfApps3.Colspan = 2;
                            pdfApps3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                            pdfApps3.BorderColor = BaseColor.WHITE;
                            pdfApps3.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfApps3);


                            PdfPCell pdfRange4 = new PdfPCell(new Phrase("151 - 200 ", TableFont));
                            pdfRange4.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfRange4.Colspan = 2;
                            pdfRange4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            pdfRange4.BorderColor = BaseColor.WHITE;
                            pdfRange4.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfRange4);

                            PdfPCell pdfApps4 = new PdfPCell(new Phrase(commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 151 && Convert.ToInt32(u.Code) <= 200).Count.ToString(), TableFont));
                            pdfApps4.HorizontalAlignment = Element.ALIGN_LEFT;
                            //pdfApps4.Colspan = 2;
                            pdfApps4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                            pdfApps4.BorderColor = BaseColor.WHITE;
                            pdfApps4.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfApps4);

                            PdfPCell pdfRange5 = new PdfPCell(new Phrase("201 - 250", TableFont));
                            pdfRange5.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Header.Colspan = 2;
                            pdfRange5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            pdfRange5.BorderColor = BaseColor.WHITE;
                            pdfRange5.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfRange5);

                            PdfPCell pdfApps5 = new PdfPCell(new Phrase(commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 201 && Convert.ToInt32(u.Code) <= 250).Count.ToString(), TableFont));
                            pdfApps5.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Header.Colspan = 4;
                            pdfApps5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                            pdfApps5.BorderColor = BaseColor.WHITE;
                            pdfApps5.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfApps5);

                            PdfPCell pdfRange6 = new PdfPCell(new Phrase("251 - 300", TableFont));
                            pdfRange6.HorizontalAlignment = Element.ALIGN_LEFT;
                            // Header.Colspan = 2;
                            pdfRange6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            pdfRange6.BorderColor = BaseColor.WHITE;
                            pdfRange6.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfRange6);

                            PdfPCell pdfApps6 = new PdfPCell(new Phrase(commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 251 && Convert.ToInt32(u.Code) <= 300).Count.ToString(), TableFont));
                            pdfApps6.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfApps6.Colspan = 2;
                            pdfApps6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                            pdfApps6.BorderColor = BaseColor.WHITE;
                            pdfApps6.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfApps6);


                            PdfPCell pdfRange7 = new PdfPCell(new Phrase("301 - 400 ", TableFont));
                            pdfRange7.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfRange7.Colspan = 2;
                            pdfRange7.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            pdfRange7.BorderColor = BaseColor.WHITE;
                            pdfRange7.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfRange7);

                            PdfPCell pdfApps7 = new PdfPCell(new Phrase(commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 301 && Convert.ToInt32(u.Code) <= 400).Count.ToString(), TableFont));
                            pdfApps7.HorizontalAlignment = Element.ALIGN_LEFT;
                            // pdfApps7.Colspan = 2;
                            pdfApps7.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                            pdfApps7.BorderColor = BaseColor.WHITE;
                            pdfApps7.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfApps7);

                            PdfPCell pdfRange8 = new PdfPCell(new Phrase("401 - 450", TableFont));
                            pdfRange8.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Header.Colspan = 2;
                            pdfRange8.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            pdfRange8.BorderColor = BaseColor.WHITE;
                            pdfRange8.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfRange8);

                            PdfPCell pdfApps8 = new PdfPCell(new Phrase(commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 401 && Convert.ToInt32(u.Code) <= 450).Count.ToString(), TableFont));
                            pdfApps8.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Header.Colspan = 4;
                            pdfApps8.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                            pdfApps8.BorderColor = BaseColor.WHITE;
                            pdfApps8.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfApps8);

                            PdfPCell pdfRange9 = new PdfPCell(new Phrase("451 - 500", TableFont));
                            pdfRange9.HorizontalAlignment = Element.ALIGN_LEFT;
                            // Header.Colspan = 2;
                            pdfRange9.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            pdfRange9.BorderColor = BaseColor.WHITE;
                            pdfRange9.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfRange9);

                            PdfPCell pdfApps9 = new PdfPCell(new Phrase(commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 451 && Convert.ToInt32(u.Code) <= 500).Count.ToString(), TableFont));
                            pdfApps9.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfApps9.Colspan = 2;
                            pdfApps9.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                            pdfApps9.BorderColor = BaseColor.WHITE;
                            pdfApps9.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfApps9);



                            PdfPCell pdfRange10 = new PdfPCell(new Phrase("Above 500", TableFont));
                            pdfRange10.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfRange10.Colspan = 2;
                            pdfRange10.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            pdfRange10.BorderColor = BaseColor.WHITE;
                            pdfRange10.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfRange10);

                            PdfPCell pdfApps10 = new PdfPCell(new Phrase(commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 500).Count.ToString(), TableFont));
                            pdfApps10.HorizontalAlignment = Element.ALIGN_LEFT;
                            // pdfApps10.Colspan = 2;
                            pdfApps10.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                            pdfApps10.BorderColor = BaseColor.WHITE;
                            pdfApps10.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfApps10);

                            PdfPCell pdfRange11 = new PdfPCell(new Phrase("Others ", TableFont));
                            pdfRange11.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Header.Colspan = 2;
                            pdfRange11.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            pdfRange11.BorderColor = BaseColor.WHITE;
                            pdfRange11.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfRange11);

                            PdfPCell pdfApps11 = new PdfPCell(new Phrase(commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) < 0).Count.ToString(), TableFont));
                            pdfApps11.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Header.Colspan = 4;
                            pdfApps11.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                            pdfApps11.BorderColor = BaseColor.WHITE;
                            pdfApps11.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfApps11);

                            PdfPCell pdfRange12 = new PdfPCell(new Phrase("Total ", TableFont));
                            pdfRange12.HorizontalAlignment = Element.ALIGN_LEFT;
                            // Header.Colspan = 2;
                            pdfRange12.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            pdfRange12.BorderColor = BaseColor.WHITE;
                            pdfRange12.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfRange12);

                            PdfPCell pdfApps12 = new PdfPCell(new Phrase(commonAllPoints.Count.ToString(), TableFont));
                            pdfApps12.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfApps12.Colspan = 2;
                            pdfApps12.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                            pdfApps12.BorderColor = BaseColor.WHITE;
                            pdfApps12.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfApps12);

                            PdfPCell pdfBottom = new PdfPCell(new Phrase("", TableFont));
                            pdfBottom.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfBottom.Colspan = 8;
                            pdfBottom.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                            pdfBottom.BorderColor = BaseColor.WHITE;
                            pdfBottom.FixedHeight = 15f;
                            casb0530_Table.AddCell(pdfBottom);


                            if (rankCatgList.Count > 0)
                            {
                                PdfPCell HeaderRankPoints = new PdfPCell(new Phrase("", TblFontBold));
                                HeaderRankPoints.HorizontalAlignment = Element.ALIGN_LEFT;
                                HeaderRankPoints.Colspan = 3;
                                HeaderRankPoints.FixedHeight = 15f;
                                HeaderRankPoints.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                HeaderRankPoints.BorderColor = BaseColor.WHITE;
                                casb0530_Table.AddCell(HeaderRankPoints);

                                PdfPCell HeaderRankPoints1 = new PdfPCell(new Phrase("Rank Points", TblFontBold));
                                HeaderRankPoints1.HorizontalAlignment = Element.ALIGN_CENTER;
                                HeaderRankPoints1.Colspan = 3;
                                HeaderRankPoints1.FixedHeight = 15f;
                                HeaderRankPoints1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                HeaderRankPoints1.BorderColor = BaseColor.WHITE;
                                casb0530_Table.AddCell(HeaderRankPoints1);

                                PdfPCell HeaderRankPoints2 = new PdfPCell(new Phrase("", TblFontBold));
                                HeaderRankPoints2.HorizontalAlignment = Element.ALIGN_LEFT;
                                HeaderRankPoints2.Colspan = 2;
                                HeaderRankPoints2.FixedHeight = 15f;
                                HeaderRankPoints2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                HeaderRankPoints2.BorderColor = BaseColor.WHITE;
                                casb0530_Table.AddCell(HeaderRankPoints2);

                                PdfPCell HeaderRankPoints3 = new PdfPCell(new Phrase("Rank Category", TblFontBold));
                                HeaderRankPoints3.HorizontalAlignment = Element.ALIGN_LEFT;
                                HeaderRankPoints3.Colspan = 3;
                                HeaderRankPoints3.FixedHeight = 15f;
                                HeaderRankPoints3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                HeaderRankPoints3.BorderColor = BaseColor.WHITE;
                                casb0530_Table.AddCell(HeaderRankPoints3);

                                PdfPCell HeaderRankPoints4 = new PdfPCell(new Phrase("From", TblFontBold));
                                HeaderRankPoints4.HorizontalAlignment = Element.ALIGN_LEFT;
                                HeaderRankPoints4.Colspan = 2;
                                HeaderRankPoints4.FixedHeight = 15f;
                                HeaderRankPoints4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                HeaderRankPoints4.BorderColor = BaseColor.WHITE;
                                casb0530_Table.AddCell(HeaderRankPoints4);



                                PdfPCell HeaderRankPoints5 = new PdfPCell(new Phrase("To", TblFontBold));
                                HeaderRankPoints5.HorizontalAlignment = Element.ALIGN_LEFT;
                                HeaderRankPoints5.Colspan = 1;
                                HeaderRankPoints5.FixedHeight = 15f;
                                HeaderRankPoints5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                HeaderRankPoints5.BorderColor = BaseColor.WHITE;
                                casb0530_Table.AddCell(HeaderRankPoints5);

                                PdfPCell HeaderRankPoints6 = new PdfPCell(new Phrase("# of Apps", TblFontBold));
                                HeaderRankPoints6.HorizontalAlignment = Element.ALIGN_LEFT;
                                HeaderRankPoints6.Colspan = 2;
                                HeaderRankPoints6.FixedHeight = 15f;
                                HeaderRankPoints6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                HeaderRankPoints6.BorderColor = BaseColor.WHITE;
                                casb0530_Table.AddCell(HeaderRankPoints6);
                                int intCounttotal = 0;
                                foreach (RankCatgEntity item in rankCatgList)
                                {
                                    PdfPCell pdfRankpoin1 = new PdfPCell(new Phrase(item.Desc, TableFont));
                                    pdfRankpoin1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfRankpoin1.Colspan = 3;
                                    pdfRankpoin1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                    pdfRankpoin1.BorderColor = BaseColor.WHITE;
                                    pdfRankpoin1.FixedHeight = 15f;
                                    casb0530_Table.AddCell(pdfRankpoin1);

                                    PdfPCell pdfRankpoin2 = new PdfPCell(new Phrase(item.PointsLow, TableFont));
                                    pdfRankpoin2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfRankpoin2.Colspan = 2;
                                    pdfRankpoin2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                    pdfRankpoin2.BorderColor = BaseColor.WHITE;
                                    pdfRankpoin2.FixedHeight = 15f;
                                    casb0530_Table.AddCell(pdfRankpoin2);

                                    PdfPCell pdfRankpoin3 = new PdfPCell(new Phrase(item.PointsHigh, TableFont));
                                    pdfRankpoin3.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfRankpoin3.Colspan = 1;
                                    pdfRankpoin3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                    pdfRankpoin3.BorderColor = BaseColor.WHITE;
                                    pdfRankpoin3.FixedHeight = 15f;
                                    casb0530_Table.AddCell(pdfRankpoin3);

                                    intCounttotal = intCounttotal + commonAllPoints.FindAll(u => Convert.ToDecimal(u.Code) >= Convert.ToDecimal(item.PointsLow) && Convert.ToDecimal(u.Code) <= Convert.ToDecimal(item.PointsHigh)).Count;

                                    PdfPCell pdfRankpoin4 = new PdfPCell(new Phrase(commonAllPoints.FindAll(u => Convert.ToDecimal(u.Code) >= Convert.ToDecimal(item.PointsLow) && Convert.ToDecimal(u.Code) <= Convert.ToDecimal(item.PointsHigh)).Count.ToString(), TableFont));
                                    pdfRankpoin4.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfRankpoin4.Colspan = 2;
                                    pdfRankpoin4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                    pdfRankpoin4.BorderColor = BaseColor.WHITE;
                                    pdfRankpoin4.FixedHeight = 15f;
                                    casb0530_Table.AddCell(pdfRankpoin4);
                                }

                                PdfPCell BottomTotal = new PdfPCell(new Phrase("", TableFont));
                                BottomTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                                BottomTotal.Colspan = 8;
                                BottomTotal.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                BottomTotal.BorderColor = BaseColor.WHITE;
                                BottomTotal.FixedHeight = 15f;
                               // BottomTotal.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                casb0530_Table.AddCell(BottomTotal);

                                PdfPCell BottomTotal1 = new PdfPCell(new Phrase("Total No of Applicants ", TblFontBold));
                                BottomTotal1.HorizontalAlignment = Element.ALIGN_LEFT;
                                BottomTotal1.Colspan = 6;
                                BottomTotal1.FixedHeight = 15f;
                                BottomTotal1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                BottomTotal1.BorderColor = BaseColor.WHITE;
                                casb0530_Table.AddCell(BottomTotal1);

                                PdfPCell BottomTotal2 = new PdfPCell(new Phrase(intCounttotal.ToString(), TblFontBold));
                                BottomTotal2.HorizontalAlignment = Element.ALIGN_LEFT;
                                BottomTotal2.Colspan = 2;
                                BottomTotal2.FixedHeight = 15f;
                                BottomTotal2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                BottomTotal2.BorderColor = BaseColor.WHITE;
                                casb0530_Table.AddCell(BottomTotal2);

                                PdfPCell BottomTotal3 = new PdfPCell(new Phrase("", TblFontBold));
                                BottomTotal3.HorizontalAlignment = Element.ALIGN_LEFT;
                                BottomTotal3.Colspan = 8;
                                // BottomTotal3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                BottomTotal3.FixedHeight = 15f;
                                BottomTotal3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                BottomTotal3.BorderColor = BaseColor.WHITE;
                                casb0530_Table.AddCell(BottomTotal3);


                            }
                            if (casb0530_Table.Rows.Count > 0)
                                document.Add(casb0530_Table);
                        }

                        strMstApplUpdate.Append("</Applicants>");
                        //if (casb0530_Table.Rows.Count > 0)
                        //    document.Add(casb0530_Table);

                        if (chkbExcel.Checked)
                        {

                            string PdfExcelName = form.GetFileName();
                            //PdfName = strFolderPath + PdfName;
                            PdfExcelName = propReportPath + BaseForm.UserID + "\\" + PdfExcelName;
                            try
                            {
                                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
                            }
                            catch (Exception ex)
                            {
                                AlertBox.Show("Error", MessageBoxIcon.Error);
                            }


                            try
                            {
                                string Tmpstr = PdfExcelName + ".xls";
                                if (File.Exists(Tmpstr))
                                    File.Delete(Tmpstr);
                            }
                            catch (Exception ex)
                            {
                                int length = 8;
                                string newFileName = System.Guid.NewGuid().ToString();
                                newFileName = newFileName.Replace("-", string.Empty);

                                Random_Filename = PdfExcelName + newFileName.Substring(0, length) + ".xls";
                            }


                            if (!string.IsNullOrEmpty(Random_Filename))
                                PdfExcelName = Random_Filename;
                            else
                                PdfExcelName += ".xls";

                            string data = null;
                            int excelcolumn = 0;

                            ExcelDocument xlWorkSheet = new ExcelDocument();

                            xlWorkSheet.ColumnWidth(0, 0);
                            xlWorkSheet.ColumnWidth(1, 100);
                            xlWorkSheet.ColumnWidth(2, 100);
                            xlWorkSheet.ColumnWidth(3, 100);
                            xlWorkSheet.ColumnWidth(4, 180);
                            xlWorkSheet.ColumnWidth(5, 100);
                            xlWorkSheet.ColumnWidth(6, 250);
                            xlWorkSheet.ColumnWidth(7, 100);
                            xlWorkSheet.ColumnWidth(8, 100);


                            rankCatgList = propRankscategory.FindAll(u => u.Agency == Agency && u.SubCode.Trim() != string.Empty && u.Code == ((Captain.Common.Utilities.ListItem)cmbRankCategory.SelectedItem).Value.ToString());
                            if (rankCatgList.Count == 0)
                                rankCatgList = propRankscategory.FindAll(u => u.Agency == "**" && u.SubCode.Trim() != string.Empty && u.Code == ((Captain.Common.Utilities.ListItem)cmbRankCategory.SelectedItem).Value.ToString());

                            xlWorkSheet[excelcolumn, 1].Font = new System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Bold);
                            xlWorkSheet[excelcolumn, 1].Alignment = Alignment.Centered;
                            xlWorkSheet.WriteCell(excelcolumn, 1, "Points Catg");


                            xlWorkSheet[excelcolumn, 2].Font = new System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Bold);
                            xlWorkSheet[excelcolumn, 2].Alignment = Alignment.Centered;
                            xlWorkSheet.WriteCell(excelcolumn, 2, "Hierarchy");

                            xlWorkSheet[excelcolumn, 3].Font = new System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Bold);
                            xlWorkSheet[excelcolumn, 3].Alignment = Alignment.Centered;
                            xlWorkSheet.WriteCell(excelcolumn, 3, "App #");



                            xlWorkSheet[excelcolumn, 4].Font = new System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Bold);
                            xlWorkSheet[excelcolumn, 4].Alignment = Alignment.Centered;
                            xlWorkSheet.WriteCell(excelcolumn, 4, "Client Name");

                            xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Bold);
                            xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Centered;
                            xlWorkSheet.WriteCell(excelcolumn, 5, "Age");

                            xlWorkSheet[excelcolumn, 6].Font = new System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Bold);
                            xlWorkSheet[excelcolumn, 6].Alignment = Alignment.Centered;
                            xlWorkSheet.WriteCell(excelcolumn, 6, "Address");

                            xlWorkSheet[excelcolumn, 7].Font = new System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Bold);
                            xlWorkSheet[excelcolumn, 7].Alignment = Alignment.Centered;
                            xlWorkSheet.WriteCell(excelcolumn, 7, "Date Add");

                            xlWorkSheet[excelcolumn, 8].Font = new System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Bold);
                            xlWorkSheet[excelcolumn, 8].Alignment = Alignment.Centered;
                            xlWorkSheet.WriteCell(excelcolumn, 8, "Intake Date");



                            //commonAllPoints = new List<CommonEntity>();
                            //RnkQuesFledsEntity = _model.SPAdminData.GetRanksCrit2Data("RANKQUES", Agency, ((Captain.Common.Utilities.ListItem)cmbRankCategory.SelectedItem).Value.ToString());
                            //RnkQuesFledsAllDataEntity = _model.SPAdminData.GetRanksCrit2Data(string.Empty, Agency, ((Captain.Common.Utilities.ListItem)cmbRankCategory.SelectedItem).Value.ToString());
                            //RnkCustFldsAllDataEntity = _model.SPAdminData.GetRanksCrit2Data("CUSTFLDS", Agency, ((Captain.Common.Utilities.ListItem)cmbRankCategory.SelectedItem).Value.ToString());
                            boolPoints = true;
                            strCategory = string.Empty;
                            // var v = caseMstEntityReport.Take(5000);
                            excelcolumn = excelcolumn + 1;     
                          


                            foreach (CaseMstEntity itemMainMstnew in caseMstEntityReport)
                            {
                                //ChldMstEntity chldMst = propChldMstEntity.Find(u => (u.ChldMstAgency.ToString() == itemMainMst.ApplAgency.ToString()) && (u.ChldMstDept.ToString() == itemMainMst.ApplDept.ToString()) && (u.ChldMstProgram.ToString() == itemMainMst.ApplProgram.ToString()) && (u.ChldMstYr.Trim().ToString() == itemMainMst.ApplYr.Trim().ToString()) && (u.ApplNo.Trim() == itemMainMst.ApplNo));
                                //List<CaseMstEntity> casemst = caseMstEntityReport.FindAll(u => (u.ApplAgency.ToString() == itemMainMst.ApplAgency.ToString()) && (u.ApplDept.ToString() == itemMainMst.ApplDept.ToString()) && (u.ApplProgram.ToString() == itemMainMst.ApplProgram.ToString()) && (u.ApplYr.Trim().ToString() == itemMainMst.ApplYr.Trim().ToString()) && (u.ApplNo.Trim() == itemMainMst.ApplNo));
                                //List<CaseSnpEntity> casesnp = propCaseSnpList.FindAll(u => (u.Agency.ToString() == itemMainMst.ApplAgency.ToString()) && (u.Dept.ToString() == itemMainMst.ApplDept.ToString()) && (u.Program.ToString() == itemMainMst.ApplProgram.ToString()) && (u.Year.Trim().ToString() == itemMainMst.ApplYr.Trim().ToString()) && (u.App.Trim() == itemMainMst.ApplNo));
                                intRankPoint = 0;
                                //if (itemMainMstnew.ApplNo.ToString() == "00006020")
                                //{

                                //}
                                //DataTable dt = GetRankCategoryDetails(casemst[0], casesnp, chldMst, RnkQuesFledsEntity, RnkQuesFledsAllDataEntity, RnkCustFldsAllDataEntity);
                                //foreach (DataRow drow in dt.Rows)
                                //{
                                //    ListchildRiskPoints.Add(new CommonEntity(itemMainMst.ApplNo.ToString(), drow["FieldCode"].ToString(), drow["FieldDesc"].ToString(), drow["Points"].ToString()));
                                //}
                                boolPoints = true;
                                if ((rdoClientSum.Checked == true) || (rdoClientSumDetails.Checked == true && rdoonlyzeroabove.Checked == true))
                                {
                                    if (Convert.ToInt32(itemMainMstnew.PointsOnly) > 0)
                                    {
                                        boolPoints = true;
                                    }
                                    else
                                        boolPoints = false;
                                }

                                if (boolPoints)
                                {

                                    excelcolumn = excelcolumn + 1;
                                    xlWorkSheet.WriteCell(excelcolumn, 1, itemMainMstnew.PointsOnly.ToString() + " " + itemMainMstnew.PointsCategory);

                                    xlWorkSheet.WriteCell(excelcolumn, 2, itemMainMstnew.ApplAgency + itemMainMstnew.ApplDept + itemMainMstnew.ApplProgram + " " + itemMainMstnew.ApplYr.ToString());


                                    xlWorkSheet.WriteCell(excelcolumn, 3, itemMainMstnew.ApplNo.ToString());

                                    xlWorkSheet.WriteCell(excelcolumn, 4, itemMainMstnew.NickName.ToString());

                                    xlWorkSheet.WriteCell(excelcolumn, 5, itemMainMstnew.Age.ToString());

                                    xlWorkSheet.WriteCell(excelcolumn, 6, itemMainMstnew.AddressDetails.ToString());

                                    xlWorkSheet.WriteCell(excelcolumn, 7, LookupDataAccess.Getdate(itemMainMstnew.DateAdd1.ToString()));

                                    xlWorkSheet.WriteCell(excelcolumn, 8, LookupDataAccess.Getdate(itemMainMstnew.IntakeDate.ToString()));

                                    if (rdoClientSumDetails.Checked == true)
                                    {
                                        List<CommonEntity> commonSubPoints = new List<CommonEntity>();
                                        if (ListchildRiskPoints.Count > 0)
                                        {
                                            commonSubPoints = ListchildRiskPoints.FindAll(u => u.Code == itemMainMstnew.ApplNo.ToString() && u.Pyear.Trim() == itemMainMstnew.ApplYr.Trim() && u.PAgency == itemMainMstnew.ApplAgency.Trim() && u.PDept == itemMainMstnew.ApplDept.Trim() && u.Pprog == itemMainMstnew.ApplProgram.Trim());
                                            if (rdoClientSumDetails.Checked == true)
                                            {
                                                if (((Captain.Common.Utilities.ListItem)cmbSortOn.SelectedItem).Value.ToString() == "1")
                                                {
                                                    commonSubPoints = commonSubPoints.OrderBy(u => u.Desc).ToList();
                                                }
                                                else if (((Captain.Common.Utilities.ListItem)cmbSortOn.SelectedItem).Value.ToString() == "2")
                                                {
                                                    commonSubPoints = commonSubPoints.OrderBy(u => Convert.ToInt32(u.Extension)).ToList();
                                                }
                                                else if (((Captain.Common.Utilities.ListItem)cmbSortOn.SelectedItem).Value.ToString() == "3")
                                                {
                                                    commonSubPoints = commonSubPoints.OrderByDescending(u => Convert.ToInt32(u.Extension)).ToList();
                                                }
                                            }

                                        }
                                        foreach (CommonEntity item in commonSubPoints)
                                        {
                                            if (rdoonlyzeroabove.Checked == true)
                                            {
                                                if (item.Extension != string.Empty)
                                                {
                                                    if (Convert.ToInt32(item.Extension) > 0)
                                                    {

                                                        excelcolumn = excelcolumn + 1;
                                                        xlWorkSheet.WriteCell(excelcolumn, 1, "");
                                                        xlWorkSheet.WriteCell(excelcolumn, 2, item.Desc.ToString());
                                                        xlWorkSheet.WriteCell(excelcolumn, 6, item.Hierarchy.ToString());       //item.Hierachy  

                                                        xlWorkSheet[excelcolumn, 8].Font = new System.Drawing.Font("Arial", 10);
                                                        xlWorkSheet[excelcolumn, 8].Alignment = Alignment.Right;
                                                        xlWorkSheet.WriteCell(excelcolumn, 8, item.Extension.ToString());
                                                        
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                excelcolumn = excelcolumn + 1;
                                                xlWorkSheet.WriteCell(excelcolumn, 1, "");
                                                xlWorkSheet.WriteCell(excelcolumn, 2, item.Desc.ToString());
                                                xlWorkSheet.WriteCell(excelcolumn, 6, item.Hierarchy.ToString());
                                                // xlWorkSheet.WriteCell(excelcolumn, 8, item.Extension.ToString());
                                                xlWorkSheet[excelcolumn, 8].Font = new System.Drawing.Font("Arial", 10);
                                                xlWorkSheet[excelcolumn, 8].Alignment = Alignment.Right;
                                                xlWorkSheet.WriteCell(excelcolumn, 8, item.Extension.ToString());

                                            }

                                        }
                                    }
                                    excelcolumn = excelcolumn + 1;

                                    //intRankPoint
                                }
                            }





                            // New RankPoints Ending                           

                            if (rdoClientSumDetails.Checked == true || rdoClientSum.Checked == true)
                            {

                                excelcolumn = excelcolumn + 5;

                                xlWorkSheet[excelcolumn, 1].Font = new System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[excelcolumn, 1].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(excelcolumn, 2, "Range");


                                xlWorkSheet[excelcolumn, 3].Font = new System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[excelcolumn, 3].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(excelcolumn, 3, "# of Apps");

                                xlWorkSheet[excelcolumn, 4].Font = new System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[excelcolumn, 4].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(excelcolumn, 4, "Range");

                                xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(excelcolumn, 5, "# of Apps");


                                xlWorkSheet[excelcolumn, 6].Font = new System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[excelcolumn, 6].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(excelcolumn, 6, "Range");


                                xlWorkSheet[excelcolumn, 7].Font = new System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[excelcolumn, 7].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(excelcolumn, 7, "# of Apps");


                                excelcolumn = excelcolumn + 1;

                                xlWorkSheet.WriteCell(excelcolumn, 2, " 0 - 50 ");


                                //xlWorkSheet.WriteCell(excelcolumn, 3, commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 0 && Convert.ToInt32(u.Code) <= 50).Count.ToString());
                                xlWorkSheet[excelcolumn, 3].Font = new System.Drawing.Font("Arial", 10);
                                xlWorkSheet[excelcolumn, 3].Alignment = Alignment.Right;
                                xlWorkSheet.WriteCell(excelcolumn, 3, commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 0 && Convert.ToInt32(u.Code) <= 50).Count.ToString());

                                xlWorkSheet.WriteCell(excelcolumn, 4, "51 - 100");


                                //xlWorkSheet.WriteCell(excelcolumn, 5, commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 51 && Convert.ToInt32(u.Code) <= 100).Count.ToString());
                                xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Arial", 10);
                                xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Right;
                                xlWorkSheet.WriteCell(excelcolumn, 5, commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 51 && Convert.ToInt32(u.Code) <= 100).Count.ToString());


                                xlWorkSheet.WriteCell(excelcolumn, 6, "101 - 150");


                                //xlWorkSheet.WriteCell(excelcolumn, 7, commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 101 && Convert.ToInt32(u.Code) <= 150).Count.ToString());
                                xlWorkSheet[excelcolumn, 7].Font = new System.Drawing.Font("Arial", 10);
                                xlWorkSheet[excelcolumn, 7].Alignment = Alignment.Right;
                                xlWorkSheet.WriteCell(excelcolumn, 7, commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 101 && Convert.ToInt32(u.Code) <= 150).Count.ToString());



                                excelcolumn = excelcolumn + 1;

                                xlWorkSheet.WriteCell(excelcolumn, 2, "151 - 200 ");


                                // xlWorkSheet.WriteCell(excelcolumn, 3, commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 151 && Convert.ToInt32(u.Code) <= 200).Count.ToString());
                                xlWorkSheet[excelcolumn, 3].Font = new System.Drawing.Font("Arial", 10);
                                xlWorkSheet[excelcolumn, 3].Alignment = Alignment.Right;
                                xlWorkSheet.WriteCell(excelcolumn, 3, commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 151 && Convert.ToInt32(u.Code) <= 200).Count.ToString());



                                xlWorkSheet.WriteCell(excelcolumn, 4, "201 - 250");


                                //  xlWorkSheet.WriteCell(excelcolumn, 5, commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 201 && Convert.ToInt32(u.Code) <= 250).Count.ToString());
                                xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Arial", 10);
                                xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Right;
                                xlWorkSheet.WriteCell(excelcolumn, 5, commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 201 && Convert.ToInt32(u.Code) <= 250).Count.ToString());


                                xlWorkSheet.WriteCell(excelcolumn, 6, "251 - 300");

                                // xlWorkSheet.WriteCell(excelcolumn, 7, commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 251 && Convert.ToInt32(u.Code) <= 300).Count.ToString());
                                xlWorkSheet[excelcolumn, 7].Font = new System.Drawing.Font("Arial", 10);
                                xlWorkSheet[excelcolumn, 7].Alignment = Alignment.Right;
                                xlWorkSheet.WriteCell(excelcolumn, 7, commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 251 && Convert.ToInt32(u.Code) <= 300).Count.ToString());


                                excelcolumn = excelcolumn + 1;

                                xlWorkSheet.WriteCell(excelcolumn, 2, "301 - 400 ");

                                // xlWorkSheet.WriteCell(excelcolumn, 3, commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 301 && Convert.ToInt32(u.Code) <= 400).Count.ToString());

                                xlWorkSheet[excelcolumn, 3].Font = new System.Drawing.Font("Arial", 10);
                                xlWorkSheet[excelcolumn, 3].Alignment = Alignment.Right;
                                xlWorkSheet.WriteCell(excelcolumn, 3, commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 301 && Convert.ToInt32(u.Code) <= 400).Count.ToString());


                                xlWorkSheet.WriteCell(excelcolumn, 4, "401 - 450");

                                // xlWorkSheet.WriteCell(excelcolumn, 5, commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 401 && Convert.ToInt32(u.Code) <= 450).Count.ToString());

                                xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Arial", 10);
                                xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Right;
                                xlWorkSheet.WriteCell(excelcolumn, 5, commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 401 && Convert.ToInt32(u.Code) <= 450).Count.ToString());


                                xlWorkSheet.WriteCell(excelcolumn, 6, "451 - 500");

                                //xlWorkSheet.WriteCell(excelcolumn, 7, commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 451 && Convert.ToInt32(u.Code) <= 500).Count.ToString());
                                xlWorkSheet[excelcolumn, 7].Font = new System.Drawing.Font("Arial", 10);
                                xlWorkSheet[excelcolumn, 7].Alignment = Alignment.Right;
                                xlWorkSheet.WriteCell(excelcolumn, 7, commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 451 && Convert.ToInt32(u.Code) <= 500).Count.ToString());


                                excelcolumn = excelcolumn + 1;

                                xlWorkSheet.WriteCell(excelcolumn, 2, "Above 500");

                                // xlWorkSheet.WriteCell(excelcolumn, 3, commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 500).Count.ToString());
                                xlWorkSheet[excelcolumn, 3].Font = new System.Drawing.Font("Arial", 10);
                                xlWorkSheet[excelcolumn, 3].Alignment = Alignment.Right;
                                xlWorkSheet.WriteCell(excelcolumn, 3, commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) >= 500).Count.ToString());

                                xlWorkSheet.WriteCell(excelcolumn, 4, "Others ");

                                // xlWorkSheet.WriteCell(excelcolumn, 5, commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) < 0).Count.ToString());
                                xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Arial", 10);
                                xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Right;
                                
                                xlWorkSheet.WriteCell(excelcolumn, 5,  commonAllPoints.FindAll(u => Convert.ToInt32(u.Code) < 0).Count.ToString());


                                xlWorkSheet.WriteCell(excelcolumn, 6, "Total ");


                                // xlWorkSheet.WriteCell(excelcolumn, 7, commonAllPoints.Count.ToString());
                                xlWorkSheet[excelcolumn, 7].Font = new System.Drawing.Font("Arial", 10);
                                xlWorkSheet[excelcolumn, 7].Alignment = Alignment.Right;
                                xlWorkSheet.WriteCell(excelcolumn, 7, commonAllPoints.Count.ToString());


                                excelcolumn = excelcolumn + 2;




                                if (rankCatgList.Count > 0)
                                {



                                    xlWorkSheet[excelcolumn, 4].Font = new System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Bold);
                                    xlWorkSheet[excelcolumn, 4].Alignment = Alignment.Centered;
                                    xlWorkSheet.WriteCell(excelcolumn, 4, "Rank Points");

                                    excelcolumn = excelcolumn + 1;
                                    xlWorkSheet[excelcolumn, 1].Font = new System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Bold);
                                    xlWorkSheet[excelcolumn, 1].Alignment = Alignment.Centered;
                                    xlWorkSheet.WriteCell(excelcolumn, 1, "Rank Category");


                                    xlWorkSheet[excelcolumn, 4].Font = new System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Bold);
                                    xlWorkSheet[excelcolumn, 4].Alignment = Alignment.Centered;
                                    xlWorkSheet.WriteCell(excelcolumn, 4, "From");



                                    xlWorkSheet[excelcolumn, 6].Font = new System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Bold);
                                    xlWorkSheet[excelcolumn, 6].Alignment = Alignment.Centered;
                                    xlWorkSheet.WriteCell(excelcolumn, 6, "To");



                                    xlWorkSheet[excelcolumn, 7].Font = new System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Bold);
                                    xlWorkSheet[excelcolumn, 7].Alignment = Alignment.Centered;
                                    xlWorkSheet.WriteCell(excelcolumn, 7, "# of Apps");


                                    int intCounttotal = 0;
                                    foreach (RankCatgEntity item in rankCatgList)
                                    {
                                        excelcolumn = excelcolumn + 1;
                                        xlWorkSheet.WriteCell(excelcolumn, 1, item.Desc);

                                        xlWorkSheet.WriteCell(excelcolumn, 4, item.PointsLow);

                                        xlWorkSheet.WriteCell(excelcolumn, 6, item.PointsHigh);

                                        intCounttotal = intCounttotal + commonAllPoints.FindAll(u => Convert.ToDecimal(u.Code) >= Convert.ToDecimal(item.PointsLow) && Convert.ToDecimal(u.Code) <= Convert.ToDecimal(item.PointsHigh)).Count;

                                         xlWorkSheet.WriteCell(excelcolumn, 7,   commonAllPoints.FindAll(u => Convert.ToDecimal(u.Code) >= Convert.ToDecimal(item.PointsLow) && Convert.ToDecimal(u.Code) <= Convert.ToDecimal(item.PointsHigh)).Count.ToString());
                                        //xlWorkSheet[excelcolumn, 7].Font = new System.Drawing.Font("Arial", 10);
                                        //xlWorkSheet[excelcolumn, 7].Alignment = Alignment.Right;
                                        //xlWorkSheet.WriteCell(excelcolumn, 7, commonAllPoints.FindAll(u => Convert.ToDecimal(u.Code) >= Convert.ToDecimal(item.PointsLow) && Convert.ToDecimal(u.Code) <= Convert.ToDecimal(item.PointsHigh)).Count.ToString());


                                    }

                                    excelcolumn = excelcolumn + 2;

                                    xlWorkSheet.WriteCell(excelcolumn, 1, "Total No of Applicants ");

                                    xlWorkSheet.WriteCell(excelcolumn, 7, intCounttotal.ToString());

                                    excelcolumn = excelcolumn + 1;
                                    xlWorkSheet.WriteCell(excelcolumn, 2, "");


                                }

                            }

                            FileStream stream = new FileStream(PdfExcelName, FileMode.Create);

                            xlWorkSheet.Save(stream);
                            stream.Close();
                        }


                    }



                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
                document.Close();
                fs.Close();
                fs.Dispose();
                AlertBox.Show("Report Generated Successfully");
                if (chkUpdatemaster.Checked == true)
                {
                    _model.CaseMstData.UpdateCaseMstRanks(strMstApplUpdate.ToString(), "Multiple");
                }
            }
        }





        private void PrintHeaderPage(Document document, PdfWriter writer )
        {


            //BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/calibrib.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            //BaseFont bfTimes = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            ////BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            //BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            //iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 10, 2);
            //iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
            //iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 10);
            //iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 13);

            BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont bf_TimesRomanI = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1250, false);
            BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bf_Calibri, 10, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 8);
            iTextSharp.text.Font TblFont = new iTextSharp.text.Font(bf_Times, 8);
            iTextSharp.text.Font TblParamsHeaderFont = new iTextSharp.text.Font(bf_Calibri, 11, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#2e5f71")));
            iTextSharp.text.Font TblHeaderTitleFont = new iTextSharp.text.Font(bf_Calibri, 14, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#806000")));
            iTextSharp.text.Font fnttimesRoman_Italic = new iTextSharp.text.Font(bf_TimesRomanI, 9, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000")));

            HierarchyEntity hierarchyDetails = _model.HierarchyAndPrograms.GetCaseHierarchy("AGENCY", BaseForm.BaseAdminAgency, string.Empty, string.Empty, string.Empty, string.Empty);
            string _strImageFolderPath = "";
            if (hierarchyDetails != null)
            {
                string LogoName = hierarchyDetails.Logo.ToString();
                _strImageFolderPath = _model.lookupDataAccess.GetReportPath() + "\\AgencyLogos\\";
                FileInfo info = new FileInfo(_strImageFolderPath + LogoName);
                if (info.Exists)
                    _strImageFolderPath = _model.lookupDataAccess.GetReportPath() + "\\AgencyLogos\\" + LogoName;
                else
                    _strImageFolderPath = "";

            }

            PdfPTable Headertable = new PdfPTable(2);
            Headertable.TotalWidth = 510f;
            Headertable.WidthPercentage = 100;
            Headertable.LockedWidth = true;
            float[] Headerwidths = new float[] { 25f, 75f };
            Headertable.SetWidths(Headerwidths);
            Headertable.HorizontalAlignment = Element.ALIGN_CENTER;

            //border trails
            PdfContentByte content = writer.DirectContent;
            iTextSharp.text.Rectangle rectangle = new iTextSharp.text.Rectangle(document.PageSize);
            rectangle.Left += document.LeftMargin;
            rectangle.Right -= document.RightMargin;
            rectangle.Top -= document.TopMargin;
            rectangle.Bottom += document.BottomMargin;
            content.SetColorStroke(BaseColor.BLACK);
            content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
            content.Stroke();

            if (_strImageFolderPath != "")
            {
                iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(_strImageFolderPath);
                PdfPCell cellLogo = new PdfPCell(imgLogo);
                cellLogo.HorizontalAlignment = Element.ALIGN_CENTER;
                cellLogo.Colspan = 2;
                cellLogo.Padding = 5;
                cellLogo.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Headertable.AddCell(cellLogo);
            }

            PdfPCell PrivName = new PdfPCell(new Phrase(Privileges.Program + " - " + Privileges.PrivilegeName, TblHeaderTitleFont));
            PrivName.HorizontalAlignment = Element.ALIGN_CENTER;
            PrivName.Colspan = 2;
            PrivName.PaddingBottom = 15;
            PrivName.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(PrivName);

            //PdfPCell row2 = new PdfPCell(new Phrase("Run By :" + LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), TableFont));
            //row2.HorizontalAlignment = Element.ALIGN_LEFT;
            ////row2.Colspan = 2;
            //row2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(row2);

            //PdfPCell row21 = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString(), TableFont));
            //row21.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////row2.Colspan = 2;
            //row21.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(row21);

            PdfPCell SelRepPar = new PdfPCell(new Phrase("Selected Report Parameters", TblParamsHeaderFont));
            SelRepPar.HorizontalAlignment = Element.ALIGN_CENTER;
            SelRepPar.VerticalAlignment = PdfPCell.ALIGN_TOP;
            SelRepPar.PaddingBottom = 5;
            SelRepPar.MinimumHeight = 6;
            SelRepPar.Colspan = 2;
            SelRepPar.Border = iTextSharp.text.Rectangle.NO_BORDER;
            SelRepPar.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b8e9fb"));
            Headertable.AddCell(SelRepPar);

            //row3.HorizontalAlignment = Element.ALIGN_CENTER;
            //row3.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            //row3.MinimumHeight = 8;
            //row3.Colspan = 2;
            //row3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //row3.BackgroundColor = BaseColor.LIGHT_GRAY;
            //Headertable.AddCell(row3);

            //string Agy = "Agency: All"; string Det = "Department: All"; string Prg = "Program: All"; string Header_year = string.Empty;
            //if (Agency != "**") Agy = "Agency: " + Agency;
            //if (Dept != "**") Det = "Department: " + Dept;
            //if (Prog != "**") Prg = "Program: " + Prog;
            //if (CmbYear.Visible == true)
            //    Header_year = "Year: " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();


            //PdfPCell Hierarchy = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim() + "     " + Header_year, TableFont));
            //Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
            //Hierarchy.Colspan = 2;
            //Hierarchy.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(Hierarchy);

            string Agy = /*Agency : */"All"; string Det = /*Dept : */"All"; string Prg = /*Program : */"All"; string Header_year = string.Empty;
            if (Agency != "**") Agy = /*"Agency : " + */Agency;
            if (Dept != "**") Det = /*"Dept : " + */Dept;
            if (Prog != "**") Prg = /*"Program : " + */Prog;
            if (CmbYear.Visible == true)
                Header_year = /*"Year : " + */((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

            if (CmbYear.Visible == true)
            {
                PdfPCell Hierarchy = new PdfPCell(new Phrase("  " + "Hierarchy", TableFont));
                Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
                Hierarchy.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Hierarchy.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hierarchy.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                Hierarchy.PaddingBottom = 5;
                Headertable.AddCell(Hierarchy);

                PdfPCell Hierarchy1 = new PdfPCell(new Phrase("Agency: " + Agy + ", " + "Department: " + Det + ", " + "Program: " + Prg + ", " + "Year: " + Header_year, TableFont));
                Hierarchy1.HorizontalAlignment = Element.ALIGN_LEFT;
                Hierarchy1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                //Hierarchy1.Colspan = 7;
                Hierarchy1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hierarchy1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                Hierarchy1.PaddingBottom = 5;
                Headertable.AddCell(Hierarchy1);
            }
            else
            {
                PdfPCell hierarchy = new PdfPCell(new Phrase("  " + "Hierarchy", TableFont));
                hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
                hierarchy.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                hierarchy.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                hierarchy.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                hierarchy.PaddingBottom = 5;
                Headertable.AddCell(hierarchy);

                PdfPCell hierarchy1 = new PdfPCell(new Phrase("Agency: " + Agy + ", " + "Department: " + Det + ", " + "Program: " + Prg, TableFont));
                hierarchy1.HorizontalAlignment = Element.ALIGN_LEFT;
                hierarchy1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                //Hierarchy1.Colspan = 7;
                hierarchy1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                hierarchy1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                hierarchy1.PaddingBottom = 5;
                Headertable.AddCell(hierarchy1);
            }

                //string Report = Privileges.PrivilegeName;

                //PdfPCell R1 = new PdfPCell(new Phrase("Report Name : " + Report, TableFont));
                //R1.HorizontalAlignment = Element.ALIGN_LEFT;
                //R1.Colspan = 2;
                //R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //Headertable.AddCell(R1);



            PdfPCell RankCat = new PdfPCell(new Phrase("  " + "Ranking Category" /*+ ((Captain.Common.Utilities.ListItem)cmbRankCategory.SelectedItem).Text.ToString().Trim()*/, TableFont));
            RankCat.HorizontalAlignment = Element.ALIGN_LEFT;
            RankCat.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            RankCat.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            RankCat.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            RankCat.PaddingBottom = 5;
            Headertable.AddCell(RankCat);

            PdfPCell RankCatCell = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbRankCategory.SelectedItem).Text.ToString().Trim(), TableFont));
            RankCatCell.HorizontalAlignment = Element.ALIGN_LEFT;
            RankCatCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            RankCatCell.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            RankCatCell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            RankCatCell.PaddingBottom = 5;
            Headertable.AddCell(RankCatCell);

            //Vikash added on 01/24/2023 for new case type field
            
            PdfPCell CaseType = new PdfPCell(new Phrase("  " + "Case Type", TableFont));
            CaseType.HorizontalAlignment = Element.ALIGN_LEFT;
            CaseType.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            CaseType.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            CaseType.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            CaseType.PaddingBottom = 5;
            Headertable.AddCell(CaseType);

            PdfPCell cmbCasetype = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbCaseType.SelectedItem).Text.ToString().Trim(), TableFont));
            cmbCasetype.HorizontalAlignment = Element.ALIGN_LEFT;
            cmbCasetype.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cmbCasetype.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cmbCasetype.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cmbCasetype.PaddingBottom = 5;
            Headertable.AddCell(cmbCasetype);

            string strReporttype = string.Empty;
            if (rdoAllApplicant.Checked == true)
            {
                strReporttype = "  " + "Report Type";
                PdfPCell RepType = new PdfPCell(new Phrase(strReporttype, TableFont));
                RepType.HorizontalAlignment = Element.ALIGN_LEFT;
                RepType.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                RepType.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                RepType.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                RepType.PaddingBottom = 5;
                Headertable.AddCell(RepType);

                PdfPCell RepTypeCell = new PdfPCell(new Phrase("All Applicants", TableFont));
                RepTypeCell.HorizontalAlignment = Element.ALIGN_LEFT;
                RepTypeCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                RepTypeCell.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                RepTypeCell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                RepTypeCell.PaddingBottom = 5;
                Headertable.AddCell(RepTypeCell);
            }
            else
            {
                strReporttype = "  " + "Applicant No";
                //cb.ShowTextAligned(100, strReporttype, 120, 550, 0);

                PdfPCell AppNo = new PdfPCell(new Phrase(strReporttype, TableFont));
                AppNo.HorizontalAlignment = Element.ALIGN_LEFT;
                AppNo.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                AppNo.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                AppNo.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                AppNo.PaddingBottom = 5;
                Headertable.AddCell(AppNo);

                PdfPCell AppNoCell = new PdfPCell(new Phrase(txtApplicant.Text, TableFont));
                AppNoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                AppNoCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                AppNoCell.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                AppNoCell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                AppNoCell.PaddingBottom = 5;
                Headertable.AddCell(AppNoCell);
            }

            string Process = string.Empty; string Applications = string.Empty; string Format = string.Empty;
            if (rdoAllApplicant.Checked == true)
            {

                if (rdoDateAdd.Checked.Equals(true)) Process = rdoDateAdd.Text.Trim();
                else if (rdoServiceDate.Checked.Equals(true))
                    Process = rdoServiceDate.Text.Trim();
                else
                    Process = rdoIntakeDt.Text.Trim();
                // cb.ShowTextAligned(100, "Process By : " + Process + "    From : " + dtForm.Text.Trim() + "  To : " + dtTodate.Text.Trim(), 120, 530, 0);

                PdfPCell ProcesBy = new PdfPCell(new Phrase("  " + "Process By: " + Process /*+ "    From: " + dtForm.Text.Trim() + "  To: " + dtTodate.Text.Trim()*/, TableFont));
                ProcesBy.HorizontalAlignment = Element.ALIGN_LEFT;
                ProcesBy.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                ProcesBy.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                ProcesBy.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                ProcesBy.PaddingBottom = 5;
                Headertable.AddCell(ProcesBy);

                PdfPCell ProcesByCell = new PdfPCell(new Phrase("From: " + dtFromDate.Text.Trim() + "    To: " + dtToDate.Text.Trim(), TableFont));
                ProcesByCell.HorizontalAlignment = Element.ALIGN_LEFT;
                ProcesByCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                ProcesByCell.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                ProcesByCell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                ProcesByCell.PaddingBottom = 5;
                Headertable.AddCell(ProcesByCell);

                //cb.ShowTextAligned(100, "From :" + dtForm.Text.Trim() +"  To : "+dtTodate.Text.Trim(), 120, 530, 0);
                //  cb.ShowTextAligned(100, "Sequence : " + ((Captain.Common.Utilities.ListItem)cmbSequence.SelectedItem).Text.ToString(), 120, 510, 0);

                PdfPCell Seq = new PdfPCell(new Phrase("  " + "Sequence" /*((Captain.Common.Utilities.ListItem)cmbSequence.SelectedItem).Text.ToString()*/, TableFont));
                Seq.HorizontalAlignment = Element.ALIGN_LEFT;
                Seq.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Seq.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Seq.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                Seq.PaddingBottom = 5;
                Headertable.AddCell(Seq);

                PdfPCell SeqCell = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbSequence.SelectedItem).Text.ToString(), TableFont));
                SeqCell.HorizontalAlignment = Element.ALIGN_LEFT;
                SeqCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                SeqCell.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                SeqCell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                SeqCell.PaddingBottom = 5;
                Headertable.AddCell(SeqCell);

                if (rdoNonSecreat.Checked.Equals(true)) Applications = rdoNonSecreat.Text.Trim();
                else if (rdoSecret.Checked.Equals(true)) Applications = rdoSecret.Text.Trim();
                else Applications = rdoBothNonSecret.Text.Trim();
                // cb.ShowTextAligned(100, "Applications : " + Applications, 120, 490, 0);

                PdfPCell App = new PdfPCell(new Phrase("  " + "Applications" /*+ Applications*/, TableFont));
                App.HorizontalAlignment = Element.ALIGN_LEFT;
                App.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                App.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                App.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                App.PaddingBottom = 5;
                Headertable.AddCell(App);

                PdfPCell AppCell = new PdfPCell(new Phrase(Applications, TableFont));
                AppCell.HorizontalAlignment = Element.ALIGN_LEFT;
                AppCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                AppCell.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                AppCell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                AppCell.PaddingBottom = 5;
                Headertable.AddCell(AppCell);


                if (rdoClientSum.Checked.Equals(true))
                {
                    Format = rdoClientSum.Text.Trim();
                }
                else
                {
                    Format = rdoClientSumDetails.Text.Trim() + ", Sort On" + ((Captain.Common.Utilities.ListItem)cmbSortOn.SelectedItem).Text.ToString();
                }
                //cb.ShowTextAligned(100, "Report Format : " + Format, 120, 470, 0);

                PdfPCell Report = new PdfPCell(new Phrase("  " + "Report Format" /*+ Format*/, TableFont));
                Report.HorizontalAlignment = Element.ALIGN_LEFT;
                Report.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Report.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Report.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                Report.PaddingBottom = 5;
                Headertable.AddCell(Report);

                PdfPCell ReportCell = new PdfPCell(new Phrase(Format, TableFont));
                ReportCell.HorizontalAlignment = Element.ALIGN_LEFT;
                ReportCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                ReportCell.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                ReportCell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                ReportCell.PaddingBottom = 5;
                Headertable.AddCell(ReportCell);

                if (rdoClientSumDetails.Checked == true)
                {
                    //  cb.ShowTextAligned(100, "Print Option : " + (rdoPrintAll.Checked == true ? rdoPrintAll.Text : rdoonlyzeroabove.Text), 120, 450, 0);
                    PdfPCell Option = new PdfPCell(new Phrase("  " + "Print Option", TableFont));
                    Option.HorizontalAlignment = Element.ALIGN_LEFT;
                    Option.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    Option.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    Option.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    Option.PaddingBottom = 5;
                    Headertable.AddCell(Option);

                    PdfPCell OptionCell = new PdfPCell(new Phrase((rdoPrintAll.Checked == true ? rdoPrintAll.Text : rdoonlyzeroabove.Text), TableFont));
                    OptionCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    OptionCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    OptionCell.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    OptionCell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    OptionCell.PaddingBottom = 5;
                    Headertable.AddCell(OptionCell);

                    // cb.ShowTextAligned(100, "Update Ranks in Case Master : " + (chkUpdatemaster.Checked == true ? "YES" : "NO"), 120, 430, 0);

                    PdfPCell Ranks = new PdfPCell(new Phrase("  " + "Update Ranks in Case Master", TableFont));
                    Ranks.HorizontalAlignment = Element.ALIGN_LEFT;
                    Ranks.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    Ranks.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    Ranks.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    Ranks.PaddingBottom = 5;
                    Headertable.AddCell(Ranks);


                    PdfPCell RanksCell = new PdfPCell(new Phrase((chkUpdatemaster.Checked == true ? "YES" : "NO"), TableFont));
                    RanksCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    RanksCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    RanksCell.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    RanksCell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    RanksCell.PaddingBottom = 5;
                    Headertable.AddCell(RanksCell);
                }
                else
                {
                    // cb.ShowTextAligned(100, "Update Ranks in Case Master : " + (chkUpdatemaster.Checked == true ? "YES" : "NO"), 120, 450, 0);

                    PdfPCell URanks = new PdfPCell(new Phrase("  " + "Update Ranks in Case Master", TableFont));
                    URanks.HorizontalAlignment = Element.ALIGN_LEFT;
                    URanks.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    URanks.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    URanks.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    URanks.PaddingBottom = 5;
                    Headertable.AddCell(URanks);

                    PdfPCell RanksCell = new PdfPCell(new Phrase((chkUpdatemaster.Checked == true ? "YES" : "NO"), TableFont));
                    RanksCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    RanksCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    RanksCell.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    RanksCell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    RanksCell.PaddingBottom = 5;
                    Headertable.AddCell(RanksCell);
                }

            }
            else
            {
                if (rdoClientSum.Checked.Equals(true)) Format = rdoClientSum.Text.Trim();
                else Format = rdoClientSumDetails.Text.Trim();
                // cb.ShowTextAligned(100, "Report Format : " + Format, 120, 530, 0);

                PdfPCell ReportF = new PdfPCell(new Phrase("  " + "Report Format" /*+ Format*/, TableFont));
                ReportF.HorizontalAlignment = Element.ALIGN_LEFT;
                ReportF.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                ReportF.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                ReportF.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                ReportF.PaddingBottom = 5;
                Headertable.AddCell(ReportF);

                PdfPCell ReportCell = new PdfPCell(new Phrase(Format, TableFont));
                ReportCell.HorizontalAlignment = Element.ALIGN_LEFT;
                ReportCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                ReportCell.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                ReportCell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                ReportCell.PaddingBottom = 5;
                Headertable.AddCell(ReportCell);

                if (rdoClientSumDetails.Checked == true)
                {
                    // cb.ShowTextAligned(100, "Print Option : " + (rdoPrintAll.Checked == true ? rdoPrintAll.Text : rdoonlyzeroabove.Text), 120, 510, 0);

                    PdfPCell POption = new PdfPCell(new Phrase("  " + "Print Option", TableFont));
                    POption.HorizontalAlignment = Element.ALIGN_LEFT;
                    POption.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    POption.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    POption.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    POption.PaddingBottom = 5;
                    Headertable.AddCell(POption);

                    PdfPCell OptionCell = new PdfPCell(new Phrase((rdoPrintAll.Checked == true ? rdoPrintAll.Text : rdoonlyzeroabove.Text), TableFont));
                    OptionCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    OptionCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    OptionCell.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    OptionCell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    OptionCell.PaddingBottom = 5;
                    Headertable.AddCell(OptionCell);

                    //   cb.ShowTextAligned(100, "Update Ranks in Case Master : " + (chkUpdatemaster.Checked == true ? "YES" : "NO"), 120, 490, 0);

                    PdfPCell UpRanks = new PdfPCell(new Phrase("  " + "Update Ranks in Case Master", TableFont));
                    UpRanks.HorizontalAlignment = Element.ALIGN_LEFT;
                    UpRanks.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    UpRanks.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    UpRanks.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    UpRanks.PaddingBottom = 5;
                    Headertable.AddCell(UpRanks);


                    PdfPCell RanksCell = new PdfPCell(new Phrase((chkUpdatemaster.Checked == true ? "YES" : "NO"), TableFont));
                    RanksCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    RanksCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    RanksCell.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    RanksCell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    RanksCell.PaddingBottom = 5;
                    Headertable.AddCell(RanksCell);
                }
                else
                {// cb.ShowTextAligned(100, "Update Ranks in Case Master : " + (chkUpdatemaster.Checked == true ? "YES" : "NO"), 120, 510, 0);
                    PdfPCell UpdRanks = new PdfPCell(new Phrase("  " + "Update Ranks in Case Master: " /*+ (chkUpdatemaster.Checked == true ? "YES" : "NO")*/, TableFont));
                    UpdRanks.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    UpdRanks.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    UpdRanks.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    UpdRanks.PaddingBottom = 5;
                    Headertable.AddCell(UpdRanks);

                    PdfPCell RanksCell = new PdfPCell(new Phrase((chkUpdatemaster.Checked == true ? "YES" : "NO"), TableFont));
                    RanksCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    RanksCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    RanksCell.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    RanksCell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    RanksCell.PaddingBottom = 5;
                    Headertable.AddCell(RanksCell);
                }
            }
            if (Headertable.Rows.Count > 0)
            {
                document.Add(Headertable);
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By : ", fnttimesRoman_Italic), 33, 40, 0);
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On : ", fnttimesRoman_Italic), 410, 40, 0);
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);
                //document.NewPage();
            }

            //document.Add(Headertable);
        }

        private string GetModuleDesc()
        {
            string ModuleDesc = null;
            DataSet ds = Captain.DatabaseLayer.Lookups.GetModules();
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["APPL_CODE"].ToString() == Privileges.ModuleCode)
                {
                    ModuleDesc = dr["APPL_DESCRIPTION"].ToString(); break;
                }
            }
            return ModuleDesc;
        }

        private void rdoClientSum_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoClientSum.Checked == true)
            {
                pnlPrintDetails.Enabled = false;
                rdoPrintAll.Checked = false;
                rdoonlyzeroabove.Checked = false;

            }
            else
            {
                pnlPrintDetails.Enabled = true;
                rdoonlyzeroabove.Checked = true;
            }
        }

        private void btnGetParameters_Click(object sender, EventArgs e)
        {
            ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
            Save_Entity.Scr_Code = Privileges.Program;
            Save_Entity.UserID = BaseForm.UserID;
            Save_Entity.Module = BaseForm.BusinessModuleID;
            Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Get");
            Save_Form.FormClosed += new FormClosedEventHandler(Get_Saved_Parameters);
            Save_Form.StartPosition = FormStartPosition.CenterScreen;
            Save_Form.ShowDialog();
        }



        private void Get_Saved_Parameters(object sender, FormClosedEventArgs e)
        {
            Report_Get_SaveParams_Form form = sender as Report_Get_SaveParams_Form;
            string[] Saved_Parameters = new string[2];
            Saved_Parameters[0] = Saved_Parameters[1] = string.Empty;

            if (form.DialogResult == DialogResult.OK)
            {
                DataTable RepCntl_Table = new DataTable();
                Saved_Parameters = form.Get_Adhoc_Saved_Parameters();

                RepCntl_Table = CommonFunctions.Convert_XMLstring_To_Datatable(Saved_Parameters[0]);
                Set_Report_Controls(RepCntl_Table);


            }
        }

        private string Get_XML_Format_for_Report_Controls()
        {
            string strReportType = string.Empty;
            string Category = ((Captain.Common.Utilities.ListItem)cmbRankCategory.SelectedItem).Value.ToString();
            string SortOrder = ((Captain.Common.Utilities.ListItem)cmbSequence.SelectedItem).Value.ToString();
            string CaseType = ((Captain.Common.Utilities.ListItem)cmbCaseType.SelectedItem).Value.ToString();
            string strProcessBy = "I";
            
            if (rdoServiceDate.Checked)
                strProcessBy = "S";
            else if (rdoDateAdd.Checked)
                strProcessBy = "D";

            string strsecurity = string.Empty;
            if (rdoAllApplicant.Checked == true)
                strReportType = "A";
            else
                strReportType = "S";


            if (rdoBothNonSecret.Checked == true)
                strsecurity = "A";
            else if (rdoSecret.Checked == true)
                strsecurity = "Y";
            else if (rdoNonSecreat.Checked == true)
                strsecurity = "N";

            string strReportFormat = rdoClientSumDetails.Checked ? "Y" : "N";
            string strReportFormatSortOn = string.Empty;
            if (rdoClientSumDetails.Checked == true)
                strReportFormatSortOn = ((Captain.Common.Utilities.ListItem)cmbSortOn.SelectedItem).Value.ToString();

            string strPrintDetails = rdoPrintAll.Checked ? "Y" : "N";

            string updateMaster = chkUpdatemaster.Checked == true ? "Y" : "N";

            string chckexcel = chkbExcel.Checked == true ? "Y" : "N";

            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");

            //   PROG = \"" + Current_Hierarchy_DB.Substring(6, 2) + "\" 
            str.Append("<Row AGENCY = \"" + Agency + "\" DEPT = \"" + Dept + "\" PROG = \"" + Prog +
                            "\" YEAR = \"" + Program_Year + "\" ProcessBy = \"" + strProcessBy + "\" Category = \"" + Category + "\" SORT = \"" + SortOrder +"\" CaseType = \"" + CaseType + "\" Security = \"" + strsecurity + "\" ReportFormat = \"" + strReportFormat + 
                            "\" PrintDetails = \"" + strPrintDetails + "\" UpdateMaster = \"" + updateMaster + "\" FromDate = \"" + dtFromDate.Value.Date + "\" ToDate = \"" + dtToDate.Value.Date + "\" ApplicantNo = \"" +
                            "\" Excel = \"" + chckexcel + txtApplicant.Text + "\" ReportType = \"" + strReportType + "\" FormatSortOn = \"" + strReportFormatSortOn + "\" />");
            str.Append("</Rows>");

            return str.ToString();
        }

        private void btnSaveParameters_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {

                ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
                Save_Entity.Scr_Code = Privileges.Program;
                Save_Entity.UserID = BaseForm.UserID;
                Save_Entity.Card_1 = Get_XML_Format_for_Report_Controls();
                Save_Entity.Card_2 = string.Empty;
                Save_Entity.Card_3 = string.Empty;
                Save_Entity.Module = BaseForm.BusinessModuleID;

                Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Save", BaseForm, Privileges);
                Save_Form.StartPosition = FormStartPosition.CenterScreen;
                Save_Form.ShowDialog();
            }

        }

        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];

                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());

                CommonFunctions.SetComboBoxValue(cmbRankCategory, dr["Category"].ToString());
                CommonFunctions.SetComboBoxValue(cmbSequence, dr["Sort"].ToString());
                CommonFunctions.SetComboBoxValue(cmbCaseType, dr["CaseType"].ToString());
                if (dr["ReportType"].ToString() == "A")
                {
                    rdoAllApplicant.Checked = true;
                }
                else
                {
                    rdoSelectApplicant.Checked = true;
                }
                rdoSelectApplicant_CheckedChanged(rdoSelectApplicant, new EventArgs());
                txtApplicant.Text = dr["ApplicantNo"].ToString();


                // ApplicantNo = \"" + txtApplicant.Text + "\" ReportType = \"" + strReportType + "\" FormatSortOn = \"" + strReportFormatSortOn + "\" />");

                //SetComboBoxValue(Cmb_Group_Sort, dr["SORT"].ToString());
                if (dr["ProcessBy"].ToString() == "D")
                    rdoDateAdd.Checked = true;
                else if (dr["ProcessBy"].ToString() == "S")
                    rdoServiceDate.Checked = true;
                else if (dr["ProcessBy"].ToString() == "I")
                    rdoIntakeDt.Checked = true;

                if (dr["Security"].ToString() == "A")
                    rdoBothNonSecret.Checked = true;

                else if (dr["Security"].ToString() == "Y")
                    rdoSecret.Checked = true;

                else if (dr["Security"].ToString() == "N")
                    rdoNonSecreat.Checked = true;

                if (dr["ReportFormat"].ToString() == "Y")
                {
                    rdoClientSumDetails.Checked = true;
                    CommonFunctions.SetComboBoxValue(cmbSortOn, dr["FormatSortOn"].ToString());
                }
                else
                {
                    rdoClientSum.Checked = true;
                    pnlSortOn.Visible = false;
                }
                if (dr["ReportFormat"].ToString() == "Y")
                {
                    pnlPrintDetails.Enabled = true;
                    if (dr["PrintDetails"].ToString() == "Y")
                        rdoPrintAll.Checked = true;
                    else
                        rdoonlyzeroabove.Checked = true;
                }
                else
                {
                    rdoonlyzeroabove.Checked = false;
                    rdoPrintAll.Checked = false;
                    pnlPrintDetails.Enabled = false;
                }

                chkUpdatemaster.Checked = dr["UpdateMaster"].ToString() == "Y" ? true : false;
                chkbExcel.Checked = dr["Excel"].ToString() == "Y" ? true : false;

                dtFromDate.Value = Convert.ToDateTime(dr["FromDate"]);
                dtToDate.Value = Convert.ToDateTime(dr["ToDate"]);

                //if (dr["CASEDIFF"].ToString() == "Y")
                //    Cb_Use_DIFF.Checked = true;
                //else
                //    Cb_Use_DIFF.Checked = false;

                //if (dr["INCMEM"].ToString() == "Y")
                //    Cb_Inc_Menbers.Checked = true;
                //else
                //    Cb_Inc_Menbers.Checked = false;
            }
        }

        private void rdoClientSumDetails_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoClientSumDetails.Checked == true)
                pnlSortOn.Visible = true;
            else
                pnlSortOn.Visible = false;
        }

        private void Pb_Help_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "CASB2530");
        }

        private void rdoSelectApplicant_CheckedChanged(object sender, EventArgs e)
        {
            _errorProvider.SetError(btnBrowse, null);
            _errorProvider.SetError(dtFromDate, null);
            if (rdoSelectApplicant.Checked == true)
            {
                txtApplicant.Enabled = true;
                btnBrowse.Enabled = true;
                txtApplicant.Visible = true;
                btnBrowse.Visible = true;
                lblApplicantReq.Visible = true;
                rdoBothNonSecret.Checked = true;
                pnlApplications.Enabled = false;
                pnlProceeby.Enabled = false;
                //pnlApplications.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
                //pnlProceeby.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
                cmbSequence.SelectedIndex = 0;
                dtFromDate.Checked = false;
                dtToDate.Checked = false;
            }
            else
            {
                txtApplicant.Clear();
                txtApplicant.Visible = false;
                btnBrowse.Visible = false;
                lblApplicantReq.Visible = false;
                pnlApplications.Enabled = true;
                pnlProceeby.Enabled = true;
                dtFromDate.Value = DateTime.Now.Date;
                dtToDate.Value = DateTime.Now.Date;
                dtFromDate.Checked = true;
                dtToDate.Checked = true;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            BrowseApplicantForm BrowseApplcantForm = new BrowseApplicantForm(BaseForm, string.Empty, Privileges, Agency, Dept, Prog, Program_Year);
            BrowseApplcantForm.FormClosed += new FormClosedEventHandler(BrowseApplcantForm_FormClosed);
            BrowseApplcantForm.StartPosition = FormStartPosition.CenterScreen;
            BrowseApplcantForm.ShowDialog();
        }

        private void Casb2530Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        void BrowseApplcantForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            BrowseApplicantForm BrowseApplication = sender as BrowseApplicantForm;
            if (BrowseApplication.DialogResult == DialogResult.OK)
            {

                CaseMstEntity caseMstData = BrowseApplication.MstData;
                if (caseMstData != null)
                {
                    txtApplicant.Text = caseMstData.ApplNo;
                }
            }
        }

        private void txtApplicant_Leave(object sender, EventArgs e)
        {
            if (txtApplicant.Text.Trim() != string.Empty)
            {
                txtApplicant.Text = SetLeadingZeros(txtApplicant.Text);
                List<CaseMstEntity> casemstlist = _model.CaseMstData.GetCaseMstAll(Agency, Dept, Prog, Program_Year, txtApplicant.Text, string.Empty, string.Empty, string.Empty, string.Empty, "MSTALLSNP");
                if (casemstlist.Count == 0)
                {
                    txtApplicant.Text = string.Empty;
                    AlertBox.Show("Applicant does not exist", MessageBoxIcon.Warning);
                }
            }
        }

        private string SetLeadingZeros(string TmpSeq)
        {
            int Seq_len = TmpSeq.Trim().Length;
            string TmpCode = null;
            TmpCode = TmpSeq.ToString().Trim();
            switch (Seq_len)
            {
                case 7: TmpCode = "0" + TmpCode; break;
                case 6: TmpCode = "00" + TmpCode; break;
                case 5: TmpCode = "000" + TmpCode; break;
                case 4: TmpCode = "0000" + TmpCode; break;
                case 3: TmpCode = "00000" + TmpCode; break;
                case 2: TmpCode = "000000" + TmpCode; break;
                case 1: TmpCode = "0000000" + TmpCode; break;
                //default: MessageBox.Show("Table Code should not be blank", "CAP Systems", MessageBoxButtons.OK);  TxtCode.Focus();
                //    break;
            }
            return (TmpCode);
        }


    }
}