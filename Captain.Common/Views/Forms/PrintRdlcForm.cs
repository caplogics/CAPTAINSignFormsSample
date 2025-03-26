#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;


using Wisej.Web;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Microsoft.Reporting.WebForms;
using System.Data.SqlClient;
using Captain.DatabaseLayer;
using Captain.Common.Utilities;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class PrintRdlcForm : Form
    {
        public bool blnUseHostedpageLoad = true; string Rep = null;
        private CaptainModel _model = null;
        public PrintRdlcForm(BaseForm baseform, PrivilegeEntity privilages)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseform;
            Privileage = privilages;
            propzipCodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(string.Empty, string.Empty, string.Empty, string.Empty);

            propCaseMstList = _model.CaseMstData.GetCaseMstadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty);
            propCaseSnpList = _model.CaseMstData.GetCaseSnpadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty);
            // _model.ChldMstData.GetChldMstDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear,);
           
        }

        public PrintRdlcForm(DataSet ds,string strReportName)
        {
            InitializeComponent();
            _model = new CaptainModel();           
            propReportName = strReportName;
            dsdata = ds;            
            //propstrDate = strstrDate;
            //propEndDate = strEndDate;
            //propUserNames = strUserName;
            //propProgrames = strProgramNames;
          
            //proptabledata = strTableData;
           

        }

        public BaseForm BaseForm { get; set; }

        public List<CaseMstEntity> propCaseMstList { get; set; }
        public List<CaseSnpEntity> propCaseSnpList { get; set; }
        public List<ChldMstEntity> propChldMstEntity { get; set; }
        public DataSet propDataset { get; set; }
        public PrivilegeEntity Privileage { get; set; }
        bool Main_Report_Processing_Completed = false;
        public string propReportName { get; set; }
        public string propUserNames { get; set; }
        public string propProgrames { get; set; }
        public string propstrDate { get; set; }
        public string propEndDate { get; set; }
        public string proptabledata { get; set; }
        public DataSet dsdata { get; set; }

        void case2530_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            if (propDataset.Tables[0].Rows.Count > 0)
            {
                //for (int i = 0; i < 20; i++)
                //{                    

                   // string strApplNo = e.Parameters["MST_APP_NO"].Values[0].ToString();
                    ChldMstEntity chldMst = _model.ChldMstData.GetChldMstDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "2012", "00001898", string.Empty);
                    List<CaseMstEntity> casemst = propCaseMstList.FindAll(u => u.ApplNo.Trim() == "00001898");
                    List<CaseSnpEntity> casesnp = propCaseSnpList.FindAll(u => u.App.Trim() == "00001898");
                    DataTable dt = GetRankCategoryDetails(casemst[0], casesnp, chldMst);
                    ReportDataSource datasource = new ReportDataSource("DataSet1", dt);
                    e.DataSources.Clear();
                    e.DataSources.Add(new ReportDataSource("DataSet1", dt));
               // }

            }


        }


        #region RankCateogryPointsCalculation
        public CaseMstEntity propMstRank { get; set; }

        DataTable dtRankSubDetails;
        private DataTable GetRankCategoryDetails(CaseMstEntity caseMst, List<CaseSnpEntity> caseSnp, ChldMstEntity chldMst)
        {
            dtRankSubDetails = new DataTable();
            dtRankSubDetails.Columns.Add("RNKCRIT1_AGENCY", typeof(string));
            dtRankSubDetails.Columns.Add("RNKCRIT1_RANK_CTG", typeof(string));
            dtRankSubDetails.Columns.Add("RNKCRIT1_FIELD_CODE", typeof(string));

            //
            // Here we add five DataRows.
            //


            List<RNKCRIT2Entity> RnkQuesFledsEntity = _model.SPAdminData.GetRanksCrit2Data("RANKQUES", string.Empty, string.Empty);
            List<RNKCRIT2Entity> RnkQuesFledsAllDataEntity = _model.SPAdminData.GetRanksCrit2Data(string.Empty, BaseForm.BaseAgency, string.Empty);
            List<RNKCRIT2Entity> RnkCustFldsAllDataEntity = _model.SPAdminData.GetRanksCrit2Data("CUSTFLDS", BaseForm.BaseAgency, string.Empty);
            List<CustomQuestionsEntity> custResponses = _model.CaseMstData.GetCustomQuestionAnswersRank(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty,string.Empty,string.Empty);
            List<CommonEntity> ListRankPoints = new List<CommonEntity>();
            for (int intRankCtg = 1; intRankCtg <= 6; intRankCtg++)
            {
                List<RNKCRIT2Entity> RnkQuesFledsDataEntity = RnkQuesFledsAllDataEntity.FindAll(u => u.RankCategory.Trim().ToString() == intRankCtg.ToString());
                List<RNKCRIT2Entity> RnkCustFldsDataEntity = RnkCustFldsAllDataEntity.FindAll(u => u.RankCategory.Trim().ToString() == intRankCtg.ToString());

                List<RNKCRIT2Entity> RnkQuesSearchList;
                propMstRank = caseMst;
                RNKCRIT2Entity RnkQuesSearch = null;
                // List<RNKCRIT2Entity> RnkQuesCaseSnp = null;
                int intRankPoint = 0;
                string strApplicationcode = string.Empty;
                foreach (RNKCRIT2Entity rnkQuesData in RnkQuesFledsEntity)
                {
                    DataRow dr = dtRankSubDetails.NewRow();
                    RnkQuesSearch = null;
                    dr["RNKCRIT1_AGENCY"] = rnkQuesData.RankFldDesc.ToString();
                    switch (rnkQuesData.RankFldName.Trim())
                    {

                        case Consts.RankQues.MZip:
                            dr["RNKCRIT1_RANK_CTG"] = caseMst.Zip.Trim();
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Zip.Trim());
                            break;
                        case Consts.RankQues.MCounty:
                            dr["RNKCRIT1_RANK_CTG"] = caseMst.County.Trim();
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.County.Trim());
                            break;
                        case Consts.RankQues.MLanguage:
                            dr["RNKCRIT1_RANK_CTG"] = caseMst.Language.Trim();
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Language.Trim());
                            break;
                        case Consts.RankQues.MAlertCode:
                            dr["RNKCRIT1_RANK_CTG"] = caseMst.AlertCodes.Trim();
                            intRankPoint = intRankPoint + fillAlertIncomeCodes(caseMst.AlertCodes, RnkQuesFledsDataEntity, rnkQuesData.RankFldName.Trim());
                            break;
                        case Consts.RankQues.MAboutUs:
                            dr["RNKCRIT1_RANK_CTG"] = caseMst.AboutUs.Trim();
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.AboutUs.Trim());
                            break;
                        case Consts.RankQues.MAddressYear:
                            if (caseMst.AddressYears != string.Empty)
                                dr["RNKCRIT1_RANK_CTG"] = caseMst.AddressYears.Trim();
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.AddressYears) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.AddressYears));
                            break;
                        case Consts.RankQues.MBestContact:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.BestContact.Trim());
                            break;
                        case Consts.RankQues.MCaseReviewDate:
                            if (caseMst.CaseReviewDate != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.CaseReviewDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.CaseReviewDate).Date);
                            break;
                        case Consts.RankQues.MCaseType:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.CaseType.Trim());
                            break;
                        case Consts.RankQues.MCmi:
                            if (caseMst.Cmi != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Cmi) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Cmi));
                            break;
                        case Consts.RankQues.MEElectric:
                            if (caseMst.ExpElectric != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpElectric) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpElectric));
                            break;
                        case Consts.RankQues.MEDEBTCC:
                            if (caseMst.Debtcc != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Debtcc) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Debtcc));
                            break;
                        case Consts.RankQues.MEDEBTLoans:
                            if (caseMst.DebtLoans != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.DebtLoans) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.DebtLoans));
                            break;
                        case Consts.RankQues.MEDEBTMed:
                            if (caseMst.DebtMed != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.DebtMed) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.DebtMed));
                            break;
                        case Consts.RankQues.MEHeat:
                            if (caseMst.ExpHeat != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpHeat) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpHeat));
                            break;
                        case Consts.RankQues.MEligDate:
                            if (caseMst.EligDate != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.EligDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.EligDate).Date);
                            break;
                        case Consts.RankQues.MELiveExpenses:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.ExpLivexpense.Trim());
                            break;
                        case Consts.RankQues.MERent:
                            if (caseMst.ExpRent != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpRent) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpRent));
                            break;
                        case Consts.RankQues.METotal:
                            if (caseMst.ExpTotal != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpTotal) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpTotal));
                            break;
                        case Consts.RankQues.MEWater:
                            if (caseMst.ExpWater != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpWater) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpWater));
                            break;

                        case Consts.RankQues.MExpCaseworker:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.ExpCaseWorker.Trim());
                            break;
                        case Consts.RankQues.MFamilyType:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.FamilyType.Trim());
                            break;
                        case Consts.RankQues.MFamIncome:
                            if (caseMst.FamIncome != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.FamIncome) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.FamIncome));
                            break;
                        case Consts.RankQues.MHousing:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Housing.Trim());
                            break;
                        case Consts.RankQues.MHud:
                            if (caseMst.Hud != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Hud) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Hud));
                            break;

                        case Consts.RankQues.MIncomeTypes:
                            intRankPoint = intRankPoint + fillAlertIncomeCodes(caseMst.AlertCodes, RnkQuesFledsDataEntity, rnkQuesData.RankFldName.Trim());
                            //RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.IncomeTypes.Trim());
                            break;
                        case Consts.RankQues.MInitialDate:
                            if (caseMst.InitialDate != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.InitialDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.InitialDate).Date);
                            break;
                        case Consts.RankQues.MIntakeDate:
                            if (caseMst.IntakeDate != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.IntakeDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.IntakeDate).Date);
                            break;
                        case Consts.RankQues.MIntakeWorker:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.IntakeWorker.Trim());
                            break;
                        case Consts.RankQues.MJuvenile:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Juvenile.Trim());
                            break;
                        case Consts.RankQues.MLanguageOt:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.LanguageOt.Trim());
                            break;
                        case Consts.RankQues.MNoInprog:
                            if (caseMst.NoInProg != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.NoInProg) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.NoInProg));
                            break;
                        case Consts.RankQues.Mpoverty:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Poverty.Trim());
                            break;
                        case Consts.RankQues.MProgIncome:
                            if (caseMst.ProgIncome != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ProgIncome) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ProgIncome));
                            break;
                        case Consts.RankQues.MReverifyDate:
                            if (caseMst.ReverifyDate != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.ReverifyDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.ReverifyDate).Date);
                            break;
                        case Consts.RankQues.MSECRET:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Secret.Trim());
                            break;
                        case Consts.RankQues.MSenior:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Senior.Trim());
                            break;
                        case Consts.RankQues.MSite:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Site.Trim());
                            break;
                        case Consts.RankQues.MSMi:
                            if (caseMst.Smi != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Smi) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Smi));
                            break;
                        case Consts.RankQues.MVefiryCheckstub:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyCheckStub.Trim());
                            break;
                        case Consts.RankQues.MVerifyW2:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyW2.Trim());
                            break;
                        case Consts.RankQues.MVeriTaxReturn:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyTaxReturn.Trim());
                            break;
                        case Consts.RankQues.MVerLetter:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyLetter.Trim());
                            break;
                        case Consts.RankQues.MVerOther:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyOther.Trim());
                            break;
                        case Consts.RankQues.MWaitList:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.WaitList.Trim());
                            break;
                        case Consts.RankQues.SEducation:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Education.ToString();
                            List<string> SnpFieldsCodesList = new List<string>();
                            List<string> SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Education);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.S1shift:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).IstShift.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].IstShift);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.S2ndshift:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).IIndShift.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].IIndShift);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.S3rdShift:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).IIIrdShift.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].IIIrdShift);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SAge:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Age.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Age);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        //case Consts.RankQues.SAltBdate:
                        //    RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                        //    strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).AltBdate.ToString();
                        //    SnpFieldsCodesList = new List<string>();
                        //    SnpFieldsRelationList = new List<string>();
                        //    for (int i = 0; i < caseSnp.Count; i++)
                        //    {
                        //        SnpFieldsCodesList.Add(caseSnp[i].AltBdate);
                        //        SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                        //    }
                        //    intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                        //    break;
                        case Consts.RankQues.SDisable:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Disable.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Disable);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SDrvlic:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Drvlic.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Drvlic);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SEmployed:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Employed.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Employed);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SEthinic:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Ethnic.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Ethnic);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        //case Consts.RankQues.SExpireWorkDate:
                        //    RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                        //    strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).ExpireWorkDate.ToString();
                        //    SnpFieldsCodesList = new List<string>();
                        //    SnpFieldsRelationList = new List<string>();
                        //    for (int i = 0; i < caseSnp.Count; i++)
                        //    {
                        //        SnpFieldsCodesList.Add(caseSnp[i].ExpireWorkDate);
                        //        SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                        //    }
                        //    intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                        //    break;
                        case Consts.RankQues.SFarmer:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Farmer.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Farmer);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SFoodStamps:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).FootStamps.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].FootStamps);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SFThours:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).FullTimeHours.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].FullTimeHours);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SHealthIns:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).HealthIns.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].HealthIns);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        //case Consts.RankQues.SHireDate:
                        //    RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                        //    strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).HireDate.ToString();
                        //    SnpFieldsCodesList = new List<string>();
                        //    SnpFieldsRelationList = new List<string>();
                        //    for (int i = 0; i < caseSnp.Count; i++)
                        //    {
                        //        SnpFieldsCodesList.Add(caseSnp[i].HireDate);
                        //        SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                        //    }
                        //    intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                        //    break;
                        case Consts.RankQues.SHourlyWage:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).HourlyWage.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Education);
                                SnpFieldsRelationList.Add(caseSnp[i].HourlyWage);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SjobCategory:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).JobCategory.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].JobCategory);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SjobTitle:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).JobTitle.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].JobTitle);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        //case Consts.RankQues.SLastWorkDate:
                        //    RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                        //    strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).LastWorkDate.ToString();
                        //    SnpFieldsCodesList = new List<string>();
                        //    SnpFieldsRelationList = new List<string>();
                        //    for (int i = 0; i < caseSnp.Count; i++)
                        //    {
                        //        SnpFieldsCodesList.Add(caseSnp[i].LastWorkDate);
                        //        SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                        //    }
                        //    intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                        //    break;
                        case Consts.RankQues.SLegalTowork:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).LegalTowork.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].LegalTowork);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SMartialStatus:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).MaritalStatus.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].MaritalStatus);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SMemberCode:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).MemberCode.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].MemberCode);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SNofcjob:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).NumberOfcjobs.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].NumberOfcjobs);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SNofljobs:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).NumberofLvjobs.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].NumberofLvjobs);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SPFrequency:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).PayFrequency.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].PayFrequency);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SPregnant:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Pregnant.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Pregnant);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SPThours:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).PartTimeHours.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].PartTimeHours);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SRace:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Race.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Race);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SRelitran:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Relitran.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Relitran);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SResident:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Resident.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Resident);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SRshift:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).RShift.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].RShift);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SSchoolDistrict:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).SchoolDistrict.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].SchoolDistrict);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SSEmploy:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Employed.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Employed);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SSex:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Sex.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Sex);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SSnpVet:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Vet.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Vet);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SStatus:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Status.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Status);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.STranserv:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Transerv.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Transerv);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SWic:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Wic.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Wic);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SworkLimit:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).WorkLimit.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].WorkLimit);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.CDentalCoverage:
                            if (chldMst != null)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == chldMst.DentalCoverage.Trim());
                            break;
                        case Consts.RankQues.CDiagNosisDate:
                            if (chldMst != null)
                                if (chldMst.DiagnosisDate != string.Empty)
                                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(chldMst.DiagnosisDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(chldMst.DiagnosisDate).Date);
                            break;
                        case Consts.RankQues.CDisability:
                            if (chldMst != null)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == chldMst.Disability.Trim());
                            break;
                        case Consts.RankQues.CInsCat:
                            if (chldMst != null)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == chldMst.InsCat.Trim());
                            break;
                        case Consts.RankQues.CMedCoverage:
                            if (chldMst != null)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == chldMst.MedCoverage.Trim());
                            break;
                        case Consts.RankQues.CMedicalCoverageType:
                            if (chldMst != null)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == chldMst.MedCoverType.Trim());
                            break;


                    }

                    if (RnkQuesSearch != null)
                    {
                        dr["RNKCRIT1_FIELD_CODE"] = intRankPoint.ToString();
                        intRankPoint = intRankPoint + Convert.ToInt32(RnkQuesSearch.Points);
                        dtRankSubDetails.Rows.Add(dr);
                    }
                }
                //CustomQuestionsEntity custResponcesearch = null;
                //foreach (RNKCRIT2Entity item in RnkCustFldsDataEntity)
                //{
                //    custResponcesearch = null;
                //    if (item.RankFldRespType.Trim().Equals("D"))
                //    {
                //        custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && u.ACTMULTRESP.Trim() == item.RespCd.Trim());
                //    }
                //    else if (item.RankFldRespType.Trim().Equals("N"))
                //    {
                //        custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && Convert.ToDecimal(u.ACTNUMRESP) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(u.ACTNUMRESP) <= Convert.ToDecimal(item.LtNum));
                //    }
                //    else if (item.RankFldRespType.Trim().Equals("T"))
                //    {
                //        // custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && Convert.ToDateTime(u.ACTDATERESP) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(u.ACTNUMRESP) <= Convert.ToDecimal(item.LtNum));           
                //    }
                //    if (custResponcesearch != null)
                //        intRankPoint = intRankPoint + Convert.ToInt32(item.Points);
                //}

                //ListRankPoints.Add(new CommonEntity(intRankCtg.ToString(), intRankPoint.ToString()));
            }
            //foreach (CommonEntity item in ListRankPoints)
            //{
            //   // txtProcess.Text = txtProcess.Text + item.Code + ":" + item.Desc + ",";
            //}
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


        private int CaseSnpDetailsCalc(List<RNKCRIT2Entity> rnkCaseSnp, List<CaseSnpEntity> caseSnpDetails, string strApplicantCode, List<string> listCodestring, List<string> listRelationstring, string FilterCode, string ResponceType)
        {
            int intSnpPoints = 0;
            foreach (RNKCRIT2Entity item in rnkCaseSnp)
            {
                if (item.CountInd.Trim() == "A")
                {
                    if (item.RespCd == strApplicantCode)
                    {
                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
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
                            //case Consts.RankQues.SAge:
                            //    foreach (CaseSnpEntity snpDate in caseSnpDetails)
                            //    {
                            //        if (snpDate.AltBdate != string.Empty)
                            //        {
                            //            DateTime EndDate = GetEndDateAgeCalculation(item.AgeClcInd.Trim(), propMstRank);
                            //            int AgeMonth = _model.lookupDataAccess.GetAgeCalculationMonths(Convert.ToDateTime(snpDate.AltBdate), EndDate);
                            //            if (AgeMonth >= Convert.ToDecimal(item.GtNum) && AgeMonth <= Convert.ToDecimal(item.LtNum))
                            //            {
                            //                count = count + 1;
                            //            }
                            //        }
                            //    }
                            //    break;
                            //case Consts.RankQues.SAltBdate:
                            //    foreach (CaseSnpEntity snpDate in caseSnpDetails)
                            //    {
                            //        if (snpDate.AltBdate != string.Empty)
                            //            if (Convert.ToDateTime(snpDate.AltBdate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.AltBdate).Date <= Convert.ToDateTime(item.LtDate).Date)
                            //            {
                            //                count = count + 1;
                            //            }
                            //    }

                            //    break;
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
                            //case Consts.RankQues.SExpireWorkDate:
                            //    foreach (CaseSnpEntity snpDate in caseSnpDetails)
                            //    {
                            //        if (snpDate.ExpireWorkDate != string.Empty)
                            //            if (Convert.ToDateTime(snpDate.ExpireWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.ExpireWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                            //            {
                            //                count = count + 1;
                            //            }
                            //    }
                            //    break;
                            case Consts.RankQues.SFarmer:
                                count = caseSnpDetails.FindAll(u => u.Farmer.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SFoodStamps:
                                count = caseSnpDetails.FindAll(u => u.FoodStampsDesc.Trim().Equals(item.RespCd)).Count;
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
                            //case Consts.RankQues.SHireDate:
                            //    foreach (CaseSnpEntity snpDate in caseSnpDetails)
                            //    {
                            //        if (snpDate.HireDate != string.Empty)
                            //            if (Convert.ToDateTime(snpDate.HireDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.HireDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                            //            {
                            //                count = count + 1;
                            //            }
                            //    }
                            //    break;
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
                            //case Consts.RankQues.SLastWorkDate:
                            //    foreach (CaseSnpEntity snpDate in caseSnpDetails)
                            //    {
                            //        if (snpDate.LastWorkDate != string.Empty)
                            //            if (Convert.ToDateTime(snpDate.LastWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.LastWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                            //            {
                            //                count = count + 1;
                            //            }
                            //    }
                            //    break;
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
                                if (listRelationstring.Contains(item.Relation))
                                {
                                    if (listCodestring.Contains(item.RespCd))
                                    {
                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                    }
                                }
                                break;
                            case "N":
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {

                                    switch (FilterCode)
                                    {
                                        //case Consts.RankQues.SAge:
                                        //    if (snpNumeric.AltBdate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                        //    {
                                        //        DateTime EndDate = GetEndDateAgeCalculation(item.AgeClcInd.Trim(), propMstRank);
                                        //        int AgeMonth = _model.lookupDataAccess.GetAgeCalculationMonths(Convert.ToDateTime(snpNumeric.AltBdate), EndDate);
                                        //        if (AgeMonth >= Convert.ToDecimal(item.GtNum) && AgeMonth <= Convert.ToDecimal(item.LtNum))
                                        //        {
                                        //            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                        //        }
                                        //    }
                                        //    break;

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
                            //case "B":
                            //case "T":
                            //    foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                            //    {

                            //        switch (FilterCode)
                            //        {
                            //            case Consts.RankQues.SAltBdate:
                            //                if (snpNumeric.AltBdate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                            //                    if (Convert.ToDateTime(snpNumeric.AltBdate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.AltBdate).Date <= Convert.ToDateTime(item.LtDate).Date)
                            //                    {
                            //                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                            //                    }
                            //                break;
                            //            case Consts.RankQues.SExpireWorkDate:
                            //                if (snpNumeric.ExpireWorkDate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                            //                    if (Convert.ToDateTime(snpNumeric.ExpireWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.ExpireWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                            //                    {
                            //                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                            //                    }
                            //                break;
                            //            case Consts.RankQues.SLastWorkDate:
                            //                if (snpNumeric.AltBdate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                            //                    if (Convert.ToDateTime(snpNumeric.LastWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.LastWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                            //                    {
                            //                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                            //                    }
                            //                break;
                            //            case Consts.RankQues.SHireDate:
                            //                if (snpNumeric.HireDate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                            //                    if (Convert.ToDateTime(snpNumeric.HireDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.HireDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                            //                    {
                            //                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                            //                    }
                            //                break;


                            //        }
                            //    }
                            //    break;

                        }


                    }

                }

            }
            return intSnpPoints;
        }
        public List<ZipCodeEntity> propzipCodeEntity { get; set; }
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
                ZipCodeEntity zipentity = propzipCodeEntity.Find(u => u.Zcrzip.Trim().Equals(caseMst.Zip.Trim()));
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


        #endregion

        int inthosteddata;
        //private void rv_HostedPageLoad(object sender, Wisej.Web.Hosts.AspPageEventArgs e)
        //{
        //    if (blnUseHostedpageLoad)
        //    {
        //        if (propReportName != string.Empty)
        //        {
        //            rv.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //            //if (rdox.Checked)
        //            //    reportViewer1.LocalReport.ReportPath = @"SSRS\Reports\ProgramandUsersrpt.rdlc";//UserWiseData.rdl";
        //            //else
        //            if (propReportName == "Report")
        //                rv.LocalReport.ReportPath = @"SSRS\Reports\ProgramWisereport.rdlc";//UserWiseData.rdl";
        //            if (propReportName == "Column")
        //                rv.LocalReport.ReportPath = @"SSRS\Reports\ProgramWisecolumrpt.rdlc";
        //            if (propReportName == "Shape")
        //                rv.LocalReport.ReportPath = @"SSRS\Reports\ProgramWiseshaperpt.rdlc";
        //            if (propReportName == "Bar")
        //                rv.LocalReport.ReportPath = @"SSRS\Reports\ProgramWisebarrpt.rdlc";


        //            //string strUserId = string.Empty;
        //            //string strStartDate = string.Empty;
        //            //string strEndDate = string.Empty;
        //            //string strType = string.Empty;

        //            //if (propstrDate!=string.Empty)
        //            //{
        //            //    strStartDate = Convert.ToDateTime(propstrDate).ToShortDateString();
        //            //    strEndDate = DateTime.Now.ToShortDateString();
        //            //    if (propEndDate!=string.Empty)
        //            //        strEndDate = Convert.ToDateTime(propEndDate).ToShortDateString();
        //            //}




        //            //DataSet thisDataSetDetails = DashBoardDB.GETRDLCALLDATANew123(strUserId, strStartDate, strEndDate, proptabledata.ToString(), string.Empty, propUserNames, propProgrames);



        //            ReportDataSource datasourcedetails = new ReportDataSource("DataSet1", dsdata.Tables[0]);
        //            rv.LocalReport.DataSources.Clear();
        //            rv.LocalReport.DataSources.Add(datasourcedetails);
        //            // rv.LocalReport.Refresh();
        //            // rv.Update();


        //            inthosteddata = inthosteddata + 1;

        //            if (inthosteddata > 3)
        //                blnUseHostedpageLoad = false;
        //            //blnUseHostedpageLoad = false;

        //        }
        //        else
        //        {
        //            rv.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

        //            rv.LocalReport.ReportPath = @"Reports\Case2530Main.rdlc";
        //        }
        //        //SqlConnection con = new SqlConnection(BaseForm.DataBaseConnectionString);
        //        //SqlDataAdapter da = new SqlDataAdapter("Select Top(10) * from CaseMST WHERE MST_AGENCY='02' AND MST_YEAR='2012' ", con);
        //        //DataSet thisDataSet = CaseMst.GetMstSnpCase2530Report(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty);

        //        //propDataset = thisDataSet;
        //        //ReportDataSource datasource = new ReportDataSource("DataSet1", thisDataSet.Tables[0]);
        //        //try
        //        //{
        //        //    rv.LocalReport.DataSources.Add(datasource);
        //        //    rv.LocalReport.Refresh();
        //        //    blnUseHostedpageLoad = false;
        //        //    Main_Report_Processing_Completed = true;
        //        //}

        //        //catch (Exception ex)
        //        //{

        //        //    Main_Report_Processing_Completed = true;
        //        //}
        //        //if (Main_Report_Processing_Completed)
        //        //    this.rv.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(case2530_SubreportProcessing);
        //    }
        //}

        public void GetReportDynamicRdlcData()
        {
            try
            {
                var rv = new ReportCompatibility();
                blnUseHostedpageLoad = true;
                //rv.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                //if (rdox.Checked)
                //    reportViewer1.LocalReport.ReportPath = @"SSRS\Reports\ProgramandUsersrpt.rdlc";//UserWiseData.rdl";
                //else
                if (propReportName == "Report")
                    rv.ReportPath = @"SSRS\Reports\ProgramWisereport.rdlc";//UserWiseData.rdl";
                if (propReportName == "Column")
                    rv.ReportPath = @"SSRS\Reports\ProgramWisecolumrpt.rdlc";
                if (propReportName == "Shape")
                    rv.ReportPath = @"SSRS\Reports\ProgramWiseshaperpt.rdlc";
                if (propReportName == "Bar")
                    rv.ReportPath = @"SSRS\Reports\ProgramWisebarrpt.rdlc";


                //string strUserId = string.Empty;
                //string strStartDate = string.Empty;
                //string strEndDate = string.Empty;
                //string strType = string.Empty;

                //if (propstrDate!=string.Empty)
                //{
                //    strStartDate = Convert.ToDateTime(propstrDate).ToShortDateString();
                //    strEndDate = DateTime.Now.ToShortDateString();
                //    if (propEndDate!=string.Empty)
                //        strEndDate = Convert.ToDateTime(propEndDate).ToShortDateString();
                //}




                //DataSet thisDataSetDetails = DashBoardDB.GETRDLCALLDATANew123(strUserId, strStartDate, strEndDate, proptabledata.ToString(), string.Empty, propUserNames, propProgrames);



                ReportDataSource datasourcedetails = new ReportDataSource("DataSet1", dsdata.Tables[0]);

                var stream = rv.ExportPDF(datasourcedetails);
                // blnUseHostedpageLoad = false;

                this.pdfViewer.PdfStream = stream;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void PrintRdlcForm_Load(object sender, EventArgs e)
        {
            GetReportDynamicRdlcData();
        }

    }
}