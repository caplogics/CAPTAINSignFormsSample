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
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Views.UserControls;
using System.Text.RegularExpressions;
using Captain.Common.Views.Forms.Base;


using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.IO;
using XLSExportFile;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CaseIncomeVerification : _iForm
    {
        private CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;
        private string strCaseWorkerDefaultCode = "0";
        private string strCaseWorkerDefaultStartCode = "0";
        private int intTotalinProgress = 0;
        List<FldcntlHieEntity> _fldCntlHieEntity = new List<FldcntlHieEntity>();
        List<CaseQuestionResult> caseQuestionResult = null;
        List<CaseGroupResult> caseGroupResult = null;
        List<CommonEntity> caseEligStatusList = null;


        #region Properties

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity privilege { get; set; }
        public string MainMenuAgency { get; set; }
        public string MainMenuDept { get; set; }
        public string MainMenuProgram { get; set; }
        public string MainMenuYear { get; set; }
        public string ApplicationNo { get; set; }
        public string VerifyCheckDate { get; set; }
        public string propReportPath { get; set; }
        public List<FldcntlHieEntity> FLDCNTLHieEntity
        {
            get
            {
                return _fldCntlHieEntity;
            }
            set
            {
                _fldCntlHieEntity = value;
            }
        }
        public ProgramDefinitionEntity ProgramDefination { get; set; }
        public CaseMstEntity CaseMSTEntity { get; set; }
        public ProgramDefinitionEntity programDefination { get; set; }
        public List<HierarchyEntity> hierarchyEntity { get; set; }
        public List<CommonEntity> MealEntity { get; set; }
        public string propProgramheadswitch { get; set; }
        #endregion

        private string strNameFormat = string.Empty;
        private string strVerfierFormat = string.Empty;
        private int strIndex = 0;
        private string strMode = string.Empty;

        public CaseIncomeVerification(BaseForm baseForm, string strAgency, string strDept, string strProgram, string strYear, string strApplicationNo, PrivilegeEntity privilegeEntity, string strChangeType)
        {

            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.IconPadding = 2;
            //txtIncome.Validator = TextBoxValidation.FloatValidator;
            txtInprogram.Validator = TextBoxValidation.IntegerValidator;
            _model = new CaptainModel();
            BaseForm = baseForm;
            privilege = privilegeEntity;
            this.Text = privilege.PrivilegeName.Trim();
            MainMenuAgency = BaseForm.BaseAgency;
            MainMenuDept = BaseForm.BaseDept;
            MainMenuProgram = BaseForm.BaseProg;
            MainMenuYear = BaseForm.BaseYear;
            ApplicationNo = BaseForm.BaseApplicationNo;
            StrHieCode = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile(BaseForm.BaseAgency);
            AgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
            CalculateStatus = false;
            string HIE = MainMenuAgency + MainMenuDept + MainMenuProgram;
            List<FldcntlHieEntity> CntlEntity = _model.FieldControls.GetFLDCNTLHIE("CASE2003", HIE, "FLDCNTL");
            FLDCNTLHieEntity = CntlEntity;
            // CaseMstEntity caseMSTEntity = _model.CaseMstData.GetCaseMST(MainMenuAgency, MainMenuDept, MainMenuProgram, MainMenuYear, ApplicationNo);
            BaseForm.BaseCaseMstListEntity = _model.CaseMstData.GetCaseMstadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
            BaseForm.BaseCaseSnpEntity = _model.CaseMstData.GetCaseSnpadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
            caseMstList = BaseForm.BaseCaseMstListEntity;
            caseSnpList = BaseForm.BaseCaseSnpEntity;            
            casemst2001data = CaseMSTEntity = caseMstList[0];
            propProgramheadswitch = string.Empty;

            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(PbPdf, "Income Details");
            tooltip.SetToolTip(PbIncome, "Income Verification");

            if (BaseForm.BaseCaseSnpEntity.Count > 0)
            {
                casesnpdata = caseSnpList.Find(u => u.FamilySeq == casemst2001data.FamilySeq);
                List<CaseSnpEntity> listCasesnp = caseSnpList.FindAll(u => u.ProgIncome != string.Empty);
                decimal snppronginccome = listCasesnp.Sum(u => Convert.ToDecimal(u.ProgIncome));
                txtIncome.Text = snppronginccome.ToString();//CaseMSTEntity.ProgIncome;
                txtInprogram.Text = caseSnpList.Count(u => u.Exclude == "N" && u.Status.Trim().ToUpper() != "I" && u.SsnReason != "Q").ToString(); //CaseMSTEntity.NoInProg;

            }
            //HierarchyEntity hierachyEntity = _model.HierarchyAndPrograms.GetCaseHierarchyName(strAgency, "**", "**");
            //if (hierachyEntity != null)
            //{
            strNameFormat = BaseForm.BaseHierarchyCnFormat.ToString();
            strVerfierFormat = BaseForm.BaseHierarchyCwFormat.ToString();
            // }
            // ChangeStatus();
            programDefination = _model.HierarchyAndPrograms.GetCaseDepadp(strAgency, strDept, strProgram);
            if (programDefination != null)
            {
                if (programDefination.StartMonth.Trim() != string.Empty)
                {
                    if (Convert.ToInt32(programDefination.StartMonth) >= 1 && Convert.ToInt32(programDefination.StartMonth) <= 12)
                    {
                        propProgramheadswitch = "Y";
                    }
                }
            }
            EligQuesData();
            fillcombo();
            fillGridData();
            ButtonsPrevilegs();
            EnableAllcontrolsPrivileges(false);
            dataGridCaseIncomeVer.AllowUserToAddRows = false;

            dataGridCaseIncomeVer.Columns[dataGridCaseIncomeVer.ColumnCount - 1].Visible = false;
            dataGridCaseIncomeVer.Columns[dataGridCaseIncomeVer.ColumnCount - 2].Width = 120;

            propReportPath = _model.lookupDataAccess.GetReportPath();

        }

        public List<CommonEntity> lookUpCategrcalEligiblity { get; set; }
        public AgencyControlEntity propAgencyControlDetails { get; set; }
        public AgencyControlEntity AgencyControlDetails { get; set; }

        #region EligibilityProperties

        public List<CaseELIGQUESEntity> EligQueslist { get; set; }
        public List<CaseELIGQUESEntity> EligQuestionsData { get; set; }
        public List<CaseELIGEntity> caseEligEntityAll { get; set; }
        public List<CaseELIGEntity> caseEligEntityOnlyGroups { get; set; }
        public List<CaseQuestionResult> caseQuestionResultProp { get; set; }
        public List<CustomQuestionsEntity> custQuestionActCust { get; set; }
        public List<CaseIncomeEntity> caseIncomeList { get; set; }
        public List<CaseMstEntity> caseMstList { get; set; }
        public List<CaseSnpEntity> caseSnpList { get; set; }
        public CaseMstEntity casemst2001data { get; set; }
        public CaseSnpEntity casesnpdata { get; set; }
        public List<CustomQuestionsEntity> custPrerespQuestion { get; set; }
        #endregion

        #region EligibilityMethods

        private void EligQuesData()
        {
            EligQuestionsData = _model.CaseSumData.GetELIGQUES();
            caseEligEntityOnlyGroups = _model.CaseSumData.GetCASEELIGadpgs(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, "OnlyGroup");
            caseEligEntityAll = _model.CaseSumData.GetCASEELIGadpgs(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, string.Empty);
            caseIncomeList = _model.CaseMstData.GetCaseIncomeadpynf(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);
            CaseSnpEntity caseSnpCustQuesstin = new CaseSnpEntity();
            caseSnpCustQuesstin.Agency = BaseForm.BaseAgency;
            caseSnpCustQuesstin.Dept = BaseForm.BaseDept;
            caseSnpCustQuesstin.Program = BaseForm.BaseProg;
            caseSnpCustQuesstin.Year = BaseForm.BaseYear;
            caseSnpCustQuesstin.App = BaseForm.BaseApplicationNo;
            caseSnpCustQuesstin.FamilySeq = string.Empty;
            custQuestionActCust = _model.CaseMstData.GetCustomQuestionAnswers(caseSnpCustQuesstin);
            custPrerespQuestion = _model.CaseMstData.GetPreassesQuestionAnswers(caseSnpCustQuesstin, "PRESRESP");
        }
        int intQuestionCount = 0;
        private string GetEligibilityStatus()
        {

            string strStatus = "00";
            List<CaseELIGEntity> caseEligGroups = caseEligEntityOnlyGroups.FindAll(u => ((u.EligAgency + u.EligDept + u.EligProgram).Equals(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg)) && (u.EligGroupSeq.Equals("0")));
            if (caseEligGroups.Count > 0)
            {

                caseGroupResult = new List<CaseGroupResult>();
                string strGroupConjunction = string.Empty;
                string strGroupPreviousResult = string.Empty;
                string strGroupResult = string.Empty;
                foreach (CaseELIGEntity ELigitemGroup in caseEligGroups)
                {
                    caseQuestionResult = new List<CaseQuestionResult>();
                    List<CaseELIGEntity> caseEligQuestions = caseEligEntityAll.FindAll(u => ((u.EligAgency + u.EligDept + u.EligProgram).Equals(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg)) && (!u.EligGroupSeq.Equals("0")) && (u.EligGroupCode.Equals(ELigitemGroup.EligGroupCode)));

                    foreach (CaseELIGEntity EligitemQuestion in caseEligQuestions)
                    {
                        strStatus = "98";
                        string strEqual = string.Empty;
                        string strLessthan = string.Empty;
                        string strGraterthan = string.Empty;
                        if ((!EligitemQuestion.EligAgyCode.StartsWith("C")) && (!EligitemQuestion.EligAgyCode.StartsWith("P")))
                        {
                            CaseELIGQUESEntity EligQues = EligQuestionsData.Find(u => u.EligQuesCode.Equals(EligitemQuestion.EligQuesCode));
                            List<CaseSnpEntity> casesnpQuestion = null;
                            List<CaseMstEntity> caseMstQuestion = null;

                            switch (EligQues.EligFldName.Trim())
                            {
                                case Consts.EligQues.Age:
                                    QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);
                                    casesnpQuestion = caseSnpList.FindAll(u => u.DobNa.Equals("0"));
                                    if (casesnpQuestion != null)
                                    {
                                        if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                        {
                                            casesnpQuestion = casesnpQuestion.FindAll(u => (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)).Equals(Convert.ToDecimal(strEqual))));
                                        }
                                        else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                        {
                                            casesnpQuestion = casesnpQuestion.FindAll(u => (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) < Convert.ToDecimal(strLessthan)));
                                        }
                                        else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                        {
                                            casesnpQuestion = casesnpQuestion.FindAll(u => (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) > Convert.ToDecimal(strGraterthan)));
                                        }
                                        else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                        {
                                            casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) < Convert.ToDecimal(strLessthan)));
                                        }
                                        else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                        {
                                            casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) > Convert.ToDecimal(strGraterthan)));
                                        }
                                        else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                        {
                                            casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) < Convert.ToDecimal(strLessthan)));
                                        }
                                        else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                        {
                                            casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) > Convert.ToDecimal(strGraterthan))));
                                        }
                                        if (EligitemQuestion.EligMemberAccess == "A")
                                            casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                        if (casesnpQuestion != null)
                                            QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                        else
                                            QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, 0);
                                    }
                                    else
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, 0);
                                    break;
                                case Consts.EligQues.CurrentAge:
                                    QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);
                                    casesnpQuestion = caseSnpList.FindAll(u => u.DobNa.Equals("0"));
                                    if (casesnpQuestion != null)
                                    {
                                        if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                        {
                                            casesnpQuestion = casesnpQuestion.FindAll(u => (Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)).Equals(Convert.ToDecimal(strEqual))));
                                        }
                                        else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                        {
                                            casesnpQuestion = casesnpQuestion.FindAll(u => (Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) < Convert.ToDecimal(strLessthan)));
                                        }
                                        else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                        {
                                            casesnpQuestion = casesnpQuestion.FindAll(u => (Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) > Convert.ToDecimal(strGraterthan)));
                                        }
                                        else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                        {
                                            casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) < Convert.ToDecimal(strLessthan)));
                                        }
                                        else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                        {
                                            casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) > Convert.ToDecimal(strGraterthan)));
                                        }
                                        else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                        {
                                            casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) < Convert.ToDecimal(strLessthan)));
                                        }
                                        else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                        {
                                            casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) > Convert.ToDecimal(strGraterthan))));
                                        }
                                        if (EligitemQuestion.EligMemberAccess == "A")
                                            casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                        if (casesnpQuestion != null)
                                            QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                        else
                                            QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, 0);
                                    }
                                    else
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, 0);
                                    break;
                                case Consts.EligQues.HeadStartAge:
                                    QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);
                                    casesnpQuestion = caseSnpList.FindAll(u => u.DobNa.Equals("0"));
                                    if (casesnpQuestion != null)
                                    {
                                        if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                        {
                                            casesnpQuestion = casesnpQuestion.FindAll(u => (Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)).Equals(Convert.ToDecimal(strEqual))));
                                        }
                                        else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                        {
                                            casesnpQuestion = casesnpQuestion.FindAll(u => (Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) < Convert.ToDecimal(strLessthan)));
                                        }
                                        else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                        {
                                            casesnpQuestion = casesnpQuestion.FindAll(u => (Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) > Convert.ToDecimal(strGraterthan)));
                                        }
                                        else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                        {
                                            casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) < Convert.ToDecimal(strLessthan)));
                                        }
                                        else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                        {
                                            casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) > Convert.ToDecimal(strGraterthan)));
                                        }
                                        else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                        {
                                            casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) < Convert.ToDecimal(strLessthan)));
                                        }
                                        else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                        {
                                            casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) > Convert.ToDecimal(strGraterthan))));
                                        }
                                        if (EligitemQuestion.EligMemberAccess == "A")
                                            casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                        if (casesnpQuestion != null)
                                            QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                        else
                                            QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, 0);                                       
                                    }
                                    else
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, 0);
                                    break;
                                case Consts.EligQues.Education:

                                    casesnpQuestion = caseSnpList.FindAll(u => u.Education.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.Resident:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.Resident.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.HealthIns:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.HealthIns.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.FoodStamps:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.FootStamps.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.WIC:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.Wic.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.IncomeTypes:
                                    List<CaseIncomeEntity> caseIncomeQuestin = null;
                                    if (EligitemQuestion.EligMemberAccess.ToString().Trim() == "*" || EligitemQuestion.EligMemberAccess.ToString().Trim() == "1")
                                    {
                                        caseIncomeQuestin = caseIncomeList.FindAll(u => u.Type.Equals(EligitemQuestion.EligDDTextResp));
                                    }
                                    else
                                    {
                                        caseIncomeQuestin = caseIncomeList.FindAll(u => u.Type.Equals(EligitemQuestion.EligDDTextResp) && u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    }
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        caseIncomeQuestin = caseIncomeQuestin.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, caseIncomeQuestin.Count);
                                    //casesnpQuestion = CaseSnpList.FindAll(u => u.IncomeTypes.Equals(EligitemQuestion.EligDDResponce));
                                    break;
                                case Consts.EligQues.EthnicCodes:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.Ethnic.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.FamilyTypes:
                                    caseMstQuestion = caseMstList.FindAll(u => u.FamilyType.Equals(EligitemQuestion.EligDDTextResp));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, caseMstQuestion.Count);
                                    break;
                                case Consts.EligQues.Housing:
                                    caseMstQuestion = caseMstList.FindAll(u => u.Housing.Equals(EligitemQuestion.EligDDTextResp));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, caseMstQuestion.Count);
                                    break;
                                case Consts.EligQues.Sex:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.Sex.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.Veteran:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.Vet.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.WorkStatus:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.WorkStatus.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.DisconnectedYouth:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.Youth.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.MiltaryStatus:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.MilitaryStatus.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.HealthCodes:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.Health_Codes.Contains(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.Non_CashBen:
                                    caseMstQuestion = caseMstList.FindAll(u => u.MstNCashBen.Contains(EligitemQuestion.EligDDTextResp));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, caseMstQuestion.Count);
                                    break;
                                case Consts.EligQues.Farmer:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.Farmer.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.Disability:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.Disable.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.PrimaryLanguage:
                                    caseMstQuestion = caseMstList.FindAll(u => u.Language.Equals(EligitemQuestion.EligDDTextResp));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, caseMstQuestion.Count);
                                    break;
                                case Consts.EligQues.FederalOMB:
                                    QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);
                                    string strFedomb = txtFedOmb.Text == string.Empty ? "0" : txtFedOmb.Text;
                                    if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                    {
                                        // caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)).Equals(Convert.ToDecimal(strEqual))));
                                        if (Convert.ToDecimal(strFedomb) == Convert.ToDecimal(strEqual))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }

                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                    {
                                        //  caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) < Convert.ToDecimal(strLessthan)));

                                        if (Convert.ToDecimal(strFedomb) < Convert.ToDecimal(strLessthan))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }

                                    }
                                    else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                    {
                                        //caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) > Convert.ToDecimal(strGraterthan)));

                                        if (Convert.ToDecimal(strFedomb) > Convert.ToDecimal(strGraterthan))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }

                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                    {
                                        //caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) < Convert.ToDecimal(strLessthan)));

                                        if ((Convert.ToDecimal(strFedomb) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(strFedomb) < Convert.ToDecimal(strLessthan)))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }

                                    }
                                    else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                    {
                                        //caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) > Convert.ToDecimal(strGraterthan)));

                                        if ((Convert.ToDecimal(strFedomb) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(strFedomb) > Convert.ToDecimal(strGraterthan)))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                    {
                                        //caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) < Convert.ToDecimal(strLessthan)));

                                        if ((Convert.ToDecimal(strFedomb) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(strFedomb) > Convert.ToDecimal(strGraterthan)))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }

                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                    {
                                        // caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) > Convert.ToDecimal(strGraterthan))));

                                        if ((Convert.ToDecimal(strFedomb) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(strFedomb) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(strFedomb) > Convert.ToDecimal(strGraterthan)))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }
                                    }
                                    if (caseMstQuestion != null)
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, caseMstQuestion.Count);
                                    else
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, 0);
                                    break;
                                case Consts.EligQues.SMI:
                                    QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);

                                    strFedomb = txtSmi.Text == string.Empty ? "0" : txtSmi.Text;
                                    if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                    {
                                        if (Convert.ToDecimal(strFedomb) == Convert.ToDecimal(strEqual))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }

                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                    {

                                        if (Convert.ToDecimal(strFedomb) < Convert.ToDecimal(strLessthan))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }

                                    }
                                    else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                    {

                                        if (Convert.ToDecimal(strFedomb) > Convert.ToDecimal(strGraterthan))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }

                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                    {

                                        if ((Convert.ToDecimal(strFedomb) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(strFedomb) < Convert.ToDecimal(strLessthan)))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }

                                    }
                                    else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                    {

                                        if ((Convert.ToDecimal(strFedomb) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(strFedomb) > Convert.ToDecimal(strGraterthan)))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                    {

                                        if ((Convert.ToDecimal(strFedomb) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(strFedomb) > Convert.ToDecimal(strGraterthan)))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }

                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                    {

                                        if ((Convert.ToDecimal(strFedomb) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(strFedomb) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(strFedomb) > Convert.ToDecimal(strGraterthan)))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }
                                    }

                                    //if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                    //{
                                    //    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)).Equals(Convert.ToDecimal(strEqual))));
                                    //}
                                    //else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                    //{
                                    //    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) < Convert.ToDecimal(strLessthan)));
                                    //}
                                    //else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                    //{
                                    //    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) > Convert.ToDecimal(strGraterthan)));
                                    //}
                                    //else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                    //{
                                    //    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) < Convert.ToDecimal(strLessthan)));
                                    //}
                                    //else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                    //{
                                    //    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) > Convert.ToDecimal(strGraterthan)));
                                    //}
                                    //else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                    //{
                                    //    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) < Convert.ToDecimal(strLessthan)));
                                    //}
                                    //else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                    //{
                                    //    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) > Convert.ToDecimal(strGraterthan))));
                                    //}
                                    if (caseMstQuestion != null)
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, caseMstQuestion.Count);
                                    else
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, 0);
                                    break;
                                case Consts.EligQues.CMI:
                                    QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);

                                    strFedomb = txtCmi.Text == string.Empty ? "0" : txtCmi.Text;
                                    if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                    {

                                        if (Convert.ToDecimal(strFedomb) == Convert.ToDecimal(strEqual))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }

                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                    {

                                        if (Convert.ToDecimal(strFedomb) < Convert.ToDecimal(strLessthan))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }

                                    }
                                    else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                    {

                                        if (Convert.ToDecimal(strFedomb) > Convert.ToDecimal(strGraterthan))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }

                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                    {

                                        if ((Convert.ToDecimal(strFedomb) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(strFedomb) < Convert.ToDecimal(strLessthan)))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }

                                    }
                                    else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                    {

                                        if ((Convert.ToDecimal(strFedomb) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(strFedomb) > Convert.ToDecimal(strGraterthan)))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                    {

                                        if ((Convert.ToDecimal(strFedomb) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(strFedomb) > Convert.ToDecimal(strGraterthan)))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }

                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                    {

                                        if ((Convert.ToDecimal(strFedomb) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(strFedomb) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(strFedomb) > Convert.ToDecimal(strGraterthan)))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }
                                    }


                                    //if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                    //{
                                    //    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)).Equals(Convert.ToDecimal(strEqual))));
                                    //}
                                    //else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                    //{
                                    //    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) < Convert.ToDecimal(strLessthan)));
                                    //}
                                    //else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                    //{
                                    //    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) > Convert.ToDecimal(strGraterthan)));
                                    //}
                                    //else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                    //{
                                    //    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) < Convert.ToDecimal(strLessthan)));
                                    //}
                                    //else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                    //{
                                    //    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) > Convert.ToDecimal(strGraterthan)));
                                    //}
                                    //else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                    //{
                                    //    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) < Convert.ToDecimal(strLessthan)));
                                    //}
                                    //else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                    //{
                                    //    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) > Convert.ToDecimal(strGraterthan))));
                                    //}
                                    if (caseMstQuestion != null)
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, caseMstQuestion.Count);
                                    else
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, 0);
                                    break;
                                case Consts.EligQues.HUD:
                                    QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);

                                    strFedomb = txtHud.Text == string.Empty ? "0" : txtHud.Text;
                                    if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                    {
                                        if (Convert.ToDecimal(strFedomb) == Convert.ToDecimal(strEqual))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }

                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                    {

                                        if (Convert.ToDecimal(strFedomb) < Convert.ToDecimal(strLessthan))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }

                                    }
                                    else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                    {

                                        if (Convert.ToDecimal(strFedomb) > Convert.ToDecimal(strGraterthan))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }

                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                    {

                                        if ((Convert.ToDecimal(strFedomb) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(strFedomb) < Convert.ToDecimal(strLessthan)))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }

                                    }
                                    else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                    {

                                        if ((Convert.ToDecimal(strFedomb) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(strFedomb) > Convert.ToDecimal(strGraterthan)))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                    {

                                        if ((Convert.ToDecimal(strFedomb) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(strFedomb) > Convert.ToDecimal(strGraterthan)))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }

                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                    {

                                        if ((Convert.ToDecimal(strFedomb) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(strFedomb) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(strFedomb) > Convert.ToDecimal(strGraterthan)))
                                        {
                                            caseMstQuestion = caseMstList;
                                        }
                                    }


                                    //if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                    //{
                                    //    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)).Equals(Convert.ToDecimal(strEqual))));
                                    //}
                                    //else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                    //{
                                    //    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) < Convert.ToDecimal(strLessthan)));
                                    //}
                                    //else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                    //{
                                    //    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) > Convert.ToDecimal(strGraterthan)));
                                    //}
                                    //else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                    //{
                                    //    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) < Convert.ToDecimal(strLessthan)));
                                    //}
                                    //else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                    //{
                                    //    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) > Convert.ToDecimal(strGraterthan)));
                                    //}
                                    //else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                    //{
                                    //    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) < Convert.ToDecimal(strLessthan)));
                                    //}
                                    //else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                    //{
                                    //    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) > Convert.ToDecimal(strGraterthan))));
                                    //}
                                    if (caseMstQuestion != null)
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, caseMstQuestion.Count);
                                    else
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, 0);
                                    break;
                                case Consts.EligQues.AreYoupregnant:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.Pregnant.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.MaritalStatus:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.MaritalStatus.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.PresentlyEmployed:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.Employed.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.AnyWorkLimitations:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.WorkLimit.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.HourlyWage:
                                    QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);

                                    if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                    {
                                        casesnpQuestion = caseSnpList.FindAll(u => (Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.Age)).Equals(Convert.ToDecimal(strEqual))));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                    {
                                        casesnpQuestion = caseSnpList.FindAll(u => (Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                    {
                                        casesnpQuestion = caseSnpList.FindAll(u => (Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                    {
                                        casesnpQuestion = caseSnpList.FindAll(u => ((Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                    {
                                        casesnpQuestion = caseSnpList.FindAll(u => ((Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                    {
                                        casesnpQuestion = caseSnpList.FindAll(u => ((Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                    {
                                        casesnpQuestion = caseSnpList.FindAll(u => ((Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) > Convert.ToDecimal(strGraterthan))));
                                    }
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    if (casesnpQuestion != null)
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    else
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, 0);
                                    break;
                                case Consts.EligQues.Relationship:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.MemberCode.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.Race:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.Race.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.SecondaryLanguage:
                                    caseMstQuestion = caseMstList.FindAll(u => u.LanguageOt.Equals(EligitemQuestion.EligDDTextResp));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, caseMstQuestion.Count);
                                    break;
                                case Consts.EligQues.Fund:
                                case Consts.EligQues.School:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.SchoolDistrict.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.DriversLicense:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.Race.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.ReliableTransport:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.Relitran.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.LegaltoWork:
                                    casesnpQuestion = caseSnpList.FindAll(u => u.LegalTowork.Equals(EligitemQuestion.EligDDTextResp));
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, casesnpQuestion.Count);
                                    break;
                                case Consts.EligQues.NumberinHousehold:
                                    QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);

                                    if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)).Equals(Convert.ToDecimal(strEqual))));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) > Convert.ToDecimal(strGraterthan))));
                                    }
                                    if (caseMstQuestion != null)
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, caseMstQuestion.Count);
                                    else
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, 0);
                                    break;
                                case Consts.EligQues.FamilyIncome:
                                    QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);

                                    if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)).Equals(Convert.ToDecimal(strEqual))));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) > Convert.ToDecimal(strGraterthan))));
                                    }
                                    if (caseMstQuestion != null)
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, caseMstQuestion.Count);
                                    else
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, 0);
                                    break;
                                case Consts.EligQues.NumberinProgram:
                                    QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);


                                    if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)).Equals(Convert.ToDecimal(strEqual))));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) > Convert.ToDecimal(strGraterthan))));
                                    }
                                    if (caseMstQuestion != null)
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, caseMstQuestion.Count);
                                    else
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, 0);
                                    break;
                                case Consts.EligQues.ProgramIncome:
                                    QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);
                                    if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)).Equals(Convert.ToDecimal(strEqual))));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) > Convert.ToDecimal(strGraterthan))));
                                    }
                                    if (caseMstQuestion != null)
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, caseMstQuestion.Count);
                                    else
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, 0);
                                    break;
                                case Consts.EligQues.ZipCode:
                                    QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);

                                    if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)).Equals(Convert.ToDecimal(strEqual))));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) > Convert.ToDecimal(strGraterthan))));
                                    }
                                    if (caseMstQuestion != null)
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, caseMstQuestion.Count);
                                    else
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, 0);
                                    break;
                                case Consts.EligQues.PreassTotal:
                                    QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);
                                    if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)).Equals(Convert.ToDecimal(strEqual))));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) > Convert.ToDecimal(strGraterthan))));
                                    }
                                    if (caseMstQuestion != null)
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, caseMstQuestion.Count);
                                    else
                                        QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, 0);
                                    break;
                                case Consts.EligQues.County:
                                    caseMstQuestion = caseMstList.FindAll(u => u.County.Equals(EligitemQuestion.EligDDTextResp));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, caseMstQuestion.Count);
                                    break;
                                case Consts.EligQues.City:
                                    caseMstQuestion = caseMstList.FindAll(u => u.City.ToUpper().Equals(EligitemQuestion.EligDDTextResp.ToUpper()));
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, caseMstQuestion.Count);
                                    break;

                            }
                        }// if condition closed
                        else if (EligitemQuestion.EligAgyCode.StartsWith("C"))
                        {
                            List<CustomQuestionsEntity> customEligQuesAll = null;

                            if (EligitemQuestion.EligResponseType.ToString() == "N")
                            {
                                QuestionNumericConditions(EligitemQuestion.EligResponseType.ToString().Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);

                                if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                {
                                    customEligQuesAll = custQuestionActCust.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && (Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)).Equals(Convert.ToDecimal(strEqual))));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                {
                                    customEligQuesAll = custQuestionActCust.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && (Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                {
                                    customEligQuesAll = custQuestionActCust.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && (Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                {
                                    customEligQuesAll = custQuestionActCust.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && ((Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                {
                                    customEligQuesAll = custQuestionActCust.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && ((Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                {
                                    customEligQuesAll = custQuestionActCust.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && ((Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                {
                                    customEligQuesAll = custQuestionActCust.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && ((Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) > Convert.ToDecimal(strGraterthan))));
                                }
                                QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, customEligQuesAll.Count);
                            }
                            else if (EligitemQuestion.EligResponseType.ToString() == "D")
                            {
                                if (EligitemQuestion.EligMemberAccess.ToString().Trim() == "*" || EligitemQuestion.EligMemberAccess.ToString().Trim() == "1")
                                {
                                    customEligQuesAll = custQuestionActCust.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && (Convert.ToString(u.ACTMULTRESP.Trim()).Equals(EligitemQuestion.EligDDTextResp.Trim())));
                                }
                                else
                                {
                                    if (EligitemQuestion.EligMemberAccess.ToString().Trim() == "A")
                                        customEligQuesAll = custQuestionActCust.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && u.ACTSNPFAMILYSEQ.Equals("9999999") && (Convert.ToString(u.ACTMULTRESP.Trim()).Equals(EligitemQuestion.EligDDTextResp.Trim())));
                                    else
                                        customEligQuesAll = custQuestionActCust.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && u.ACTSNPFAMILYSEQ.Equals("8888888") && (Convert.ToString(u.ACTMULTRESP.Trim()).Equals(EligitemQuestion.EligDDTextResp.Trim())));
                                }
                                if (customEligQuesAll != null)
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, customEligQuesAll.Count);
                                else
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, 0);
                            }
                            else if (EligitemQuestion.EligResponseType.ToString() == "A")
                            {
                                if (EligitemQuestion.EligMemberAccess.ToString().Trim() == "*" || EligitemQuestion.EligMemberAccess.ToString().Trim() == "1")
                                {
                                    customEligQuesAll = custQuestionActCust.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && u.ACTALPHARESP.Trim().Equals(EligitemQuestion.EligDDTextResp));
                                }
                                else
                                {
                                    if (EligitemQuestion.EligMemberAccess.ToString().Trim() == "A")
                                        customEligQuesAll = custQuestionActCust.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && u.ACTSNPFAMILYSEQ.Equals("9999999") && (Convert.ToString(u.ACTMULTRESP.Trim()).Equals(EligitemQuestion.EligDDTextResp.Trim())));
                                    else
                                        customEligQuesAll = custQuestionActCust.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && u.ACTSNPFAMILYSEQ.Equals("8888888") && (Convert.ToString(u.ACTMULTRESP.Trim()).Equals(EligitemQuestion.EligDDTextResp.Trim())));
                                }

                                if (customEligQuesAll != null)
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, customEligQuesAll.Count);
                                else
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, 0);
                            }

                        }
                        else
                        {
                            List<CustomQuestionsEntity> customEligQuesAll = null;

                            if (EligitemQuestion.EligResponseType.ToString() == "N")
                            {
                                QuestionNumericConditions(EligitemQuestion.EligResponseType.ToString().Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);

                                if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                {
                                    customEligQuesAll = custPrerespQuestion.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && (Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)).Equals(Convert.ToDecimal(strEqual))));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                {
                                    customEligQuesAll = custPrerespQuestion.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && (Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                {
                                    customEligQuesAll = custPrerespQuestion.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && (Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                {
                                    customEligQuesAll = custPrerespQuestion.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && ((Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                {
                                    customEligQuesAll = custPrerespQuestion.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && ((Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                {
                                    customEligQuesAll = custPrerespQuestion.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && ((Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                {
                                    customEligQuesAll = custPrerespQuestion.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && ((Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) > Convert.ToDecimal(strGraterthan))));
                                }
                                QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, customEligQuesAll.Count);
                            }
                            else if (EligitemQuestion.EligResponseType.ToString() == "D")
                            {
                                if (EligitemQuestion.EligMemberAccess.ToString().Trim() == "*" || EligitemQuestion.EligMemberAccess.ToString().Trim() == "1")
                                {
                                    customEligQuesAll = custPrerespQuestion.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && (Convert.ToString(u.ACTMULTRESP.Trim()).Equals(EligitemQuestion.EligDDTextResp.Trim())));
                                }
                                else
                                {
                                    if (EligitemQuestion.EligMemberAccess.ToString().Trim() == "A")
                                        customEligQuesAll = custPrerespQuestion.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && u.ACTSNPFAMILYSEQ.Equals("7777777") && (Convert.ToString(u.ACTMULTRESP.Trim()).Equals(EligitemQuestion.EligDDTextResp.Trim())));
                                    else
                                        customEligQuesAll = custPrerespQuestion.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && u.ACTSNPFAMILYSEQ.Equals("7777777") && (Convert.ToString(u.ACTMULTRESP.Trim()).Equals(EligitemQuestion.EligDDTextResp.Trim())));
                                }
                                if (customEligQuesAll != null)
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, customEligQuesAll.Count);
                                else
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, 0);
                            }
                            else if (EligitemQuestion.EligResponseType.ToString() == "A")
                            {
                                if (EligitemQuestion.EligMemberAccess.ToString().Trim() == "*" || EligitemQuestion.EligMemberAccess.ToString().Trim() == "1")
                                {
                                    customEligQuesAll = custPrerespQuestion.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && u.ACTALPHARESP.Trim().Equals(EligitemQuestion.EligDDTextResp));
                                }
                                else
                                {
                                    if (EligitemQuestion.EligMemberAccess.ToString().Trim() == "A")
                                        customEligQuesAll = custPrerespQuestion.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && u.ACTSNPFAMILYSEQ.Equals("7777777") && (Convert.ToString(u.ACTMULTRESP.Trim()).Equals(EligitemQuestion.EligDDTextResp.Trim())));
                                    else
                                        customEligQuesAll = custPrerespQuestion.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && u.ACTSNPFAMILYSEQ.Equals("7777777") && (Convert.ToString(u.ACTMULTRESP.Trim()).Equals(EligitemQuestion.EligDDTextResp.Trim())));
                                }

                                if (customEligQuesAll != null)
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, customEligQuesAll.Count);
                                else
                                    QuestionType(EligitemQuestion.EligMemberAccess, EligitemQuestion, 0);
                            }

                        }

                    }//Questions loop
                    string Conjunction = "O";
                    string ConResult = "N";
                    int i = 0;
                    foreach (CaseQuestionResult item in caseQuestionResult)
                    {
                        if (i == 0)
                        {
                            if (item.QuestionResult == "N")
                            {
                                Conjunction = item.QuestionConjuction == "A" ? "A" : "O";
                                ConResult = item.QuestionResult;
                                item.QuestionStatus = "N";
                            }
                            else
                            {
                                Conjunction = item.QuestionConjuction == "A" ? "A" : "O";
                                ConResult = item.QuestionResult;
                                item.QuestionStatus = "Y";

                            }
                            i++;
                            //if (Conjunction == "O" && ConResult == "N" && item.QuestionResult == "Y")
                            //    item.QuestionStatus = "Y";
                        }
                        else
                        {
                            if (Conjunction == "O" && ConResult == "N" && item.QuestionResult == "Y")
                            {
                                caseQuestionResult[i - 1].QuestionStatus = "Y";
                                item.QuestionStatus = "Y";
                                // Conjunction = item.QuestionConjuction == "A" ? "A" : "O";
                                ConResult = item.QuestionResult;
                            }
                            if (Conjunction == "O" && ConResult == "N" && item.QuestionResult == "N")
                            {
                                item.QuestionStatus = "N";
                                // Conjunction = item.QuestionConjuction == "A" ? "A" : "O";
                                ConResult = item.QuestionResult;
                            }
                            if (Conjunction == "O" && ConResult == "Y" && item.QuestionResult == "N")
                            {
                                item.QuestionStatus = "Y";
                                // Conjunction = item.QuestionConjuction == "A" ? "A" : "O";
                                ConResult = item.QuestionResult;
                            }
                            if (Conjunction == "O" && ConResult == "Y" && item.QuestionResult == "Y")
                            {
                                item.QuestionStatus = "Y";
                                //  Conjunction = item.QuestionConjuction == "A" ? "A" : "O";
                                ConResult = item.QuestionResult;
                            }
                            else if (Conjunction == "A" && ConResult == "N" && item.QuestionResult == "Y")
                            {
                                item.QuestionStatus = "N";
                                //  Conjunction = item.QuestionConjuction == "A" ? "A" : "O";
                                ConResult = item.QuestionResult;
                            }
                            else if (Conjunction == "A" && ConResult == "Y" && item.QuestionResult == "Y")
                            {
                                item.QuestionStatus = "Y";
                                //  Conjunction = item.QuestionConjuction == "A" ? "A" : "O";
                                ConResult = item.QuestionResult;
                            }
                            else if (Conjunction == "A" && ConResult == "N" && item.QuestionResult == "N")
                            {
                                item.QuestionStatus = "N";
                                //  Conjunction = item.QuestionConjuction == "A" ? "A" : "O";
                                ConResult = item.QuestionResult;
                            }
                            else if (Conjunction == "A" && ConResult == "Y" && item.QuestionResult == "N")
                            {
                                item.QuestionStatus = "N";
                                //  Conjunction = item.QuestionConjuction == "A" ? "A" : "O";
                                ConResult = item.QuestionResult;
                            }

                        }

                    }

                    if (Conjunction == "A")
                    {

                        if (caseQuestionResult.Count == caseQuestionResult.FindAll(u => u.QuestionStatus.Equals("Y")).Count)
                            caseGroupResult.Add(new CaseGroupResult(ELigitemGroup.EligGroupCode, caseEligQuestions.Count, ELigitemGroup.EligConjunction == "A" ? "A" : "O", "Y", string.Empty, string.Empty));
                        else
                            caseGroupResult.Add(new CaseGroupResult(ELigitemGroup.EligGroupCode, caseEligQuestions.Count, ELigitemGroup.EligConjunction == "A" ? "A" : "O", "N", string.Empty, string.Empty));
                    }
                    else
                    {
                        if (caseQuestionResult.FindAll(u => u.QuestionStatus.Equals("Y")).Count > 0)
                            caseGroupResult.Add(new CaseGroupResult(ELigitemGroup.EligGroupCode, caseEligQuestions.Count, ELigitemGroup.EligConjunction == "A" ? "A" : "O", "Y", string.Empty, string.Empty));
                        else
                            caseGroupResult.Add(new CaseGroupResult(ELigitemGroup.EligGroupCode, caseEligQuestions.Count, ELigitemGroup.EligConjunction == "A" ? "A" : "O", "N", string.Empty, string.Empty));
                    }


                }

                int intGroup = 0;
                foreach (CaseGroupResult item in caseGroupResult)
                {
                    if (intGroup == 0)
                    {
                        if (item.GroupResult == "N")
                        {
                            strGroupConjunction = item.GroupConjunction == "A" ? "A" : "O";
                            strGroupPreviousResult = item.GroupResult;
                            item.GroupStatus = "N";
                        }
                        else
                        {
                            strGroupConjunction = item.GroupConjunction == "A" ? "A" : "O";
                            strGroupPreviousResult = item.GroupResult;
                            item.GroupStatus = "Y";
                        }
                        intGroup++;
                        //if (Conjunction == "O" && ConResult == "N" && item.QuestionResult == "Y")
                        //    item.QuestionStatus = "Y";
                    }
                    else
                    {
                        intGroup++;
                        if (strGroupConjunction == "O" && strGroupPreviousResult == "N" && item.GroupResult == "Y")
                        {
                            caseGroupResult[intGroup - 1].GroupStatus = "Y";
                            item.GroupStatus = "Y";
                            if (caseGroupResult.Count != intGroup)
                                strGroupConjunction = item.GroupConjunction == "A" ? "A" : "O";
                            strGroupPreviousResult = item.GroupResult;
                        }
                        if (strGroupConjunction == "O" && strGroupPreviousResult == "N" && item.GroupResult == "N")
                        {
                            item.GroupStatus = "N";//changed N to Y
                            if (caseGroupResult.Count != intGroup)
                                strGroupConjunction = item.GroupConjunction == "A" ? "A" : "O";
                            strGroupPreviousResult = item.GroupResult;
                        }
                        if (strGroupConjunction == "O" && strGroupPreviousResult == "Y" && item.GroupResult == "N")
                        {
                            item.GroupStatus = "Y";
                            if (caseGroupResult.Count != intGroup)
                                strGroupConjunction = item.GroupConjunction == "A" ? "A" : "O";
                            strGroupPreviousResult = item.GroupResult;
                        }
                        if (strGroupConjunction == "O" && strGroupPreviousResult == "Y" && item.GroupResult == "Y")
                        {
                            item.GroupStatus = "Y";
                            if (caseGroupResult.Count != intGroup)
                                strGroupConjunction = item.GroupConjunction == "A" ? "A" : "O";
                            strGroupPreviousResult = item.GroupResult;
                        }
                        else if (strGroupConjunction == "A" && strGroupPreviousResult == "N" && item.GroupResult == "Y")
                        {
                            item.GroupStatus = "N";
                            if (caseGroupResult.Count != intGroup)
                                strGroupConjunction = item.GroupConjunction == "A" ? "A" : "O";
                            strGroupPreviousResult = item.GroupResult;
                        }
                        else if (strGroupConjunction == "A" && strGroupPreviousResult == "Y" && item.GroupResult == "Y")
                        {
                            item.GroupStatus = "Y";
                            if (caseGroupResult.Count != intGroup)
                                strGroupConjunction = item.GroupConjunction == "A" ? "A" : "O";
                            strGroupPreviousResult = item.GroupResult;
                        }
                        else if (strGroupConjunction == "A" && strGroupPreviousResult == "N" && item.GroupResult == "N")
                        {
                            item.GroupStatus = "N";
                            if (caseGroupResult.Count != intGroup)
                                strGroupConjunction = item.GroupConjunction == "A" ? "A" : "O";
                            strGroupPreviousResult = item.GroupResult;
                        }
                        else if (strGroupConjunction == "A" && strGroupPreviousResult == "Y" && item.GroupResult == "N")
                        {
                            item.GroupStatus = "N";
                            if (caseGroupResult.Count != intGroup)
                                strGroupConjunction = item.GroupConjunction == "A" ? "A" : "O";
                            strGroupPreviousResult = item.GroupResult;
                        }

                    }

                }
                if (caseGroupResult.Count == caseGroupResult.FindAll(u => u.GroupConjunction.Equals("O")).Count)
                {
                    if (caseGroupResult.FindAll(u => u.GroupStatus.Equals("Y")).Count > 0)
                    {
                        strStatus = "99";
                    }
                }
                else
                {
                    if (caseGroupResult.Count == caseGroupResult.FindAll(u => u.GroupStatus.Equals("Y")).Count)
                        strStatus = "99";
                }
            }
            if (propProgramheadswitch == "Y")
            {
                string strcategory = string.Empty;
                if (!((Captain.Common.Utilities.ListItem)cmbCategorical.SelectedItem).Value.ToString().Equals("0"))
                {

                    if (lookUpCategrcalEligiblity.Count > 0)
                    {
                        CommonEntity commoncategory = lookUpCategrcalEligiblity.Find(u => u.Extension.ToUpper() == "Y" && u.Code.ToUpper() == ((Captain.Common.Utilities.ListItem)cmbCategorical.SelectedItem).Value.ToString().ToUpper());
                        if (commoncategory != null)
                        {
                            strcategory = commoncategory.Code;
                        }
                    }
                    if (strcategory != string.Empty)
                    {
                        if (strcategory.ToUpper() != ((Captain.Common.Utilities.ListItem)cmbCategorical.SelectedItem).Value.ToString().ToUpper())
                        {
                            strStatus = "97";
                        }
                        else
                        {
                            if (txtFedOmb.Text != string.Empty)
                            {
                                if (Convert.ToDecimal(txtFedOmb.Text) >= 101 && Convert.ToDecimal(txtFedOmb.Text) <= 130)
                                {
                                    strStatus = "96";
                                }
                                else if (Convert.ToDecimal(txtFedOmb.Text) > 130)
                                {
                                    strStatus = "95";
                                }
                            }
                        }
                    }
                    else
                    {
                        strStatus = "97";
                    }
                }
                else
                {
                    if (txtFedOmb.Text != string.Empty)
                    {
                        if (Convert.ToDecimal(txtFedOmb.Text) >= 101 && Convert.ToDecimal(txtFedOmb.Text) <= 130)
                        {
                            strStatus = "96";
                        }
                        else if (Convert.ToDecimal(txtFedOmb.Text) > 130)
                        {
                            strStatus = "95";
                        }
                    }
                }
            }

            return strStatus;
        }


        private void QuestionNumericConditions(string strResponceType, CaseELIGEntity EligQueslist, out string strEqual, out string strLessthan, out string strGreaterthan)
        {

            string strLEqual = string.Empty;
            string strLLessthan = string.Empty;
            string strLGreaterthan = string.Empty;

            if (strResponceType == "N")
            {
                if (EligQueslist.EligNumEqual != string.Empty)
                {
                    if (Convert.ToDecimal(EligQueslist.EligNumEqual) > 0)
                        strLEqual = EligQueslist.EligNumEqual;
                }
                if (EligQueslist.EligNumLthan != string.Empty)
                {
                    if (Convert.ToDecimal(EligQueslist.EligNumLthan) > 0)
                        strLLessthan = EligQueslist.EligNumLthan;
                }
                if (EligQueslist.EligNumGthan != string.Empty)
                {
                    if (Convert.ToDecimal(EligQueslist.EligNumGthan) > 0)
                        strLGreaterthan = EligQueslist.EligNumGthan;
                }


            }
            strEqual = strLEqual;
            strLessthan = strLLessthan;
            strGreaterthan = strLGreaterthan;


        }

        private void QuestionType(string strQuestionType, CaseELIGEntity caseEligQues, int TotalResultCount)
        {
            string strResult = "N";
            if (strQuestionType == "*")
            {
                if (caseMstList[0].NoInhh.Equals(TotalResultCount))
                {
                    strResult = "Y";
                }
            }
            else if (strQuestionType == "1" || strQuestionType == "H" || strQuestionType == "A")
            {
                if (TotalResultCount > 0)
                {
                    strResult = "Y";
                }
            }
            caseQuestionResult.Add(new CaseQuestionResult(caseEligQues.EligGroupCode, caseEligQues.EligQuesCode, caseEligQues.EligConjunction, strResult, string.Empty, string.Empty));
        }

        #endregion

        #region EligibilityGroupQuestioncondition


        private string GetEligibilityQuestionStatus(string strHierachy, string strGroupcode, string strQuestioncode, string strGroupSeq)
        {
            string strQuestionResult = "N";
            List<CaseELIGEntity> caseEligQuestions = caseEligEntityAll.FindAll(u => ((u.EligAgency + u.EligDept + u.EligProgram).Equals(strHierachy)) && (u.EligGroupSeq.Equals(strGroupSeq)) && (u.EligGroupCode.Equals(strGroupcode)));

            if (caseEligQuestions.Count > 0)
            {
                CaseELIGEntity EligitemQuestion = caseEligQuestions.Find(u => u.EligGroupCode.Equals(strGroupcode) && u.EligQuesCode.Equals(strQuestioncode));
                if (EligitemQuestion != null)
                {
                    string strEqual = string.Empty;
                    string strLessthan = string.Empty;
                    string strGraterthan = string.Empty;
                    if ((!EligitemQuestion.EligAgyCode.StartsWith("C")) && (!EligitemQuestion.EligAgyCode.StartsWith("P")))
                    {
                        CaseELIGQUESEntity EligQues = EligQuestionsData.Find(u => u.EligQuesCode.Equals(EligitemQuestion.EligQuesCode));
                        List<CaseSnpEntity> casesnpQuestion = null;
                        List<CaseMstEntity> caseMstQuestion = null;

                        switch (EligQues.EligFldName.Trim())
                        {
                            case Consts.EligQues.Age:
                                QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);
                                casesnpQuestion = caseSnpList.FindAll(u => u.DobNa.Equals("0"));
                                if (casesnpQuestion != null)
                                {
                                    if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                    {
                                        casesnpQuestion = casesnpQuestion.FindAll(u => (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)).Equals(Convert.ToDecimal(strEqual))));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                    {
                                        casesnpQuestion = casesnpQuestion.FindAll(u => (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                    {
                                        casesnpQuestion = casesnpQuestion.FindAll(u => (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                    {
                                        casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                    {
                                        casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                    {
                                        casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                    {
                                        casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) > Convert.ToDecimal(strGraterthan))));
                                    }
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    if (casesnpQuestion != null)
                                        strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                    else
                                        strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                                }
                                else
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                                break;
                            case Consts.EligQues.CurrentAge:
                                QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);
                                casesnpQuestion = caseSnpList.FindAll(u => u.DobNa.Equals("0"));
                                if (casesnpQuestion != null)
                                {
                                    if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                    {
                                        casesnpQuestion = casesnpQuestion.FindAll(u => (Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)).Equals(Convert.ToDecimal(strEqual))));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                    {
                                        casesnpQuestion = casesnpQuestion.FindAll(u => (Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                    {
                                        casesnpQuestion = casesnpQuestion.FindAll(u => (Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                    {
                                        casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                    {
                                        casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                    {
                                        casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                    {
                                        casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.CurrentAge == string.Empty ? 0 : Convert.ToDecimal(u.CurrentAge)) > Convert.ToDecimal(strGraterthan))));
                                    }
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    if (casesnpQuestion != null)
                                        strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                    else
                                        strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                                }
                                else
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                                break;
                            case Consts.EligQues.HeadStartAge:
                                QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);
                                casesnpQuestion = caseSnpList.FindAll(u => u.DobNa.Equals("0"));
                                if (casesnpQuestion != null)
                                {
                                    if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                    {
                                        casesnpQuestion = casesnpQuestion.FindAll(u => (Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)).Equals(Convert.ToDecimal(strEqual))));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                    {
                                        casesnpQuestion = casesnpQuestion.FindAll(u => (Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                    {
                                        casesnpQuestion = casesnpQuestion.FindAll(u => (Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                    {
                                        casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                    {
                                        casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                    {
                                        casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                    {
                                        casesnpQuestion = casesnpQuestion.FindAll(u => ((Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.KAge == string.Empty ? 0 : Convert.ToDecimal(u.KAge)) > Convert.ToDecimal(strGraterthan))));
                                    }
                                    if (EligitemQuestion.EligMemberAccess == "A")
                                        casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                    if (casesnpQuestion != null)
                                        strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                    else
                                        strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                                }
                                else
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                                break;
                            case Consts.EligQues.Education:

                                casesnpQuestion = caseSnpList.FindAll(u => u.Education.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.Resident:
                                casesnpQuestion = caseSnpList.FindAll(u => u.Resident.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.HealthIns:
                                casesnpQuestion = caseSnpList.FindAll(u => u.HealthIns.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.FoodStamps:
                                casesnpQuestion = caseSnpList.FindAll(u => u.FootStamps.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.WIC:
                                casesnpQuestion = caseSnpList.FindAll(u => u.Wic.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.IncomeTypes:
                                List<CaseIncomeEntity> caseIncomeQuestin = null;
                                if (EligitemQuestion.EligMemberAccess.ToString().Trim() == "*" || EligitemQuestion.EligMemberAccess.ToString().Trim() == "1")
                                {
                                    caseIncomeQuestin = caseIncomeList.FindAll(u => u.Type.Equals(EligitemQuestion.EligDDTextResp));
                                }
                                else
                                {
                                    caseIncomeQuestin = caseIncomeList.FindAll(u => u.Type.Equals(EligitemQuestion.EligDDTextResp) && u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                }
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    caseIncomeQuestin = caseIncomeQuestin.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, caseIncomeQuestin.Count);
                                //casesnpQuestion = CaseSnpList.FindAll(u => u.IncomeTypes.Equals(EligitemQuestion.EligDDResponce));
                                break;
                            case Consts.EligQues.EthnicCodes:
                                casesnpQuestion = caseSnpList.FindAll(u => u.Ethnic.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.FamilyTypes:
                                caseMstQuestion = caseMstList.FindAll(u => u.FamilyType.Equals(EligitemQuestion.EligDDTextResp));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, caseMstQuestion.Count);
                                break;
                            case Consts.EligQues.Housing:
                                caseMstQuestion = caseMstList.FindAll(u => u.Housing.Equals(EligitemQuestion.EligDDTextResp));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, caseMstQuestion.Count);
                                break;
                            case Consts.EligQues.Sex:
                                casesnpQuestion = caseSnpList.FindAll(u => u.Sex.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.Veteran:
                                casesnpQuestion = caseSnpList.FindAll(u => u.Vet.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.WorkStatus:
                                casesnpQuestion = caseSnpList.FindAll(u => u.WorkStatus.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.DisconnectedYouth:
                                casesnpQuestion = caseSnpList.FindAll(u => u.Youth.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.MiltaryStatus:
                                casesnpQuestion = caseSnpList.FindAll(u => u.MilitaryStatus.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.HealthCodes:
                                casesnpQuestion = caseSnpList.FindAll(u => u.Health_Codes.Contains(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.Non_CashBen:
                                caseMstQuestion = caseMstList.FindAll(u => u.MstNCashBen.Contains(EligitemQuestion.EligDDTextResp));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, caseMstQuestion.Count);
                                break;
                            case Consts.EligQues.Farmer:
                                casesnpQuestion = caseSnpList.FindAll(u => u.Farmer.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.Disability:
                                casesnpQuestion = caseSnpList.FindAll(u => u.Disable.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.PrimaryLanguage:
                                caseMstQuestion = caseMstList.FindAll(u => u.Language.Equals(EligitemQuestion.EligDDTextResp));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, caseMstQuestion.Count);
                                break;
                            case Consts.EligQues.FederalOMB:
                                if (caseMstList[0].EligDate != string.Empty)
                                {
                                    QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);
                                    if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)).Equals(Convert.ToDecimal(strEqual))));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) > Convert.ToDecimal(strGraterthan)));
                                    }
                                    else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) < Convert.ToDecimal(strLessthan)));
                                    }
                                    else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                    {
                                        caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.Poverty == string.Empty ? 0 : Convert.ToDecimal(u.Poverty)) > Convert.ToDecimal(strGraterthan))));
                                    }
                                    if (caseMstQuestion != null)
                                        strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, caseMstQuestion.Count);
                                    else
                                        strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                                }
                                else
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                                break;
                            case Consts.EligQues.SMI:
                                QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);

                                if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)).Equals(Convert.ToDecimal(strEqual))));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.Smi == string.Empty ? 0 : Convert.ToDecimal(u.Smi)) > Convert.ToDecimal(strGraterthan))));
                                }
                                if (caseMstQuestion != null)
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, caseMstQuestion.Count);
                                else
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                                break;
                            case Consts.EligQues.CMI:
                                QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);

                                if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)).Equals(Convert.ToDecimal(strEqual))));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.Cmi == string.Empty ? 0 : Convert.ToDecimal(u.Cmi)) > Convert.ToDecimal(strGraterthan))));
                                }
                                if (caseMstQuestion != null)
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, caseMstQuestion.Count);
                                else
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                                break;
                            case Consts.EligQues.HUD:
                                QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);

                                if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)).Equals(Convert.ToDecimal(strEqual))));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.Hud == string.Empty ? 0 : Convert.ToDecimal(u.Hud)) > Convert.ToDecimal(strGraterthan))));
                                }
                                if (caseMstQuestion != null)
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, caseMstQuestion.Count);
                                else
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                                break;
                            case Consts.EligQues.AreYoupregnant:
                                casesnpQuestion = caseSnpList.FindAll(u => u.Pregnant.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.MaritalStatus:
                                casesnpQuestion = caseSnpList.FindAll(u => u.MaritalStatus.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.PresentlyEmployed:
                                casesnpQuestion = caseSnpList.FindAll(u => u.Employed.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.AnyWorkLimitations:
                                casesnpQuestion = caseSnpList.FindAll(u => u.WorkLimit.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.HourlyWage:
                                QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);

                                if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                {
                                    casesnpQuestion = caseSnpList.FindAll(u => (Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.Age)).Equals(Convert.ToDecimal(strEqual))));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                {
                                    casesnpQuestion = caseSnpList.FindAll(u => (Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                {
                                    casesnpQuestion = caseSnpList.FindAll(u => (Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                {
                                    casesnpQuestion = caseSnpList.FindAll(u => ((Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                {
                                    casesnpQuestion = caseSnpList.FindAll(u => ((Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                {
                                    casesnpQuestion = caseSnpList.FindAll(u => ((Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                {
                                    casesnpQuestion = caseSnpList.FindAll(u => ((Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.HourlyWage == string.Empty ? 0 : Convert.ToDecimal(u.HourlyWage)) > Convert.ToDecimal(strGraterthan))));
                                }
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                if (casesnpQuestion != null)
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                else
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                                break;
                            case Consts.EligQues.Relationship:
                                casesnpQuestion = caseSnpList.FindAll(u => u.MemberCode.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.Race:
                                casesnpQuestion = caseSnpList.FindAll(u => u.Race.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.SecondaryLanguage:
                                caseMstQuestion = caseMstList.FindAll(u => u.LanguageOt.Equals(EligitemQuestion.EligDDTextResp));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, caseMstQuestion.Count);
                                break;
                            case Consts.EligQues.Fund:
                            case Consts.EligQues.School:
                                casesnpQuestion = caseSnpList.FindAll(u => u.SchoolDistrict.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.DriversLicense:
                                casesnpQuestion = caseSnpList.FindAll(u => u.Race.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.ReliableTransport:
                                casesnpQuestion = caseSnpList.FindAll(u => u.Relitran.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.LegaltoWork:
                                casesnpQuestion = caseSnpList.FindAll(u => u.LegalTowork.Equals(EligitemQuestion.EligDDTextResp));
                                if (EligitemQuestion.EligMemberAccess == "A")
                                    casesnpQuestion = casesnpQuestion.FindAll(u => u.FamilySeq.Equals(caseMstList[0].FamilySeq));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, casesnpQuestion.Count);
                                break;
                            case Consts.EligQues.NumberinHousehold:
                                QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);

                                if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)).Equals(Convert.ToDecimal(strEqual))));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.NoInhh == string.Empty ? 0 : Convert.ToDecimal(u.NoInhh)) > Convert.ToDecimal(strGraterthan))));
                                }
                                if (caseMstQuestion != null)
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, caseMstQuestion.Count);
                                else
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                                break;
                            case Consts.EligQues.FamilyIncome:
                                QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);

                                if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)).Equals(Convert.ToDecimal(strEqual))));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.FamIncome == string.Empty ? 0 : Convert.ToDecimal(u.FamIncome)) > Convert.ToDecimal(strGraterthan))));
                                }
                                if (caseMstQuestion != null)
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, caseMstQuestion.Count);
                                else
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                                break;
                            case Consts.EligQues.NumberinProgram:
                                QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);


                                if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)).Equals(Convert.ToDecimal(strEqual))));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.NoInProg == string.Empty ? 0 : Convert.ToDecimal(u.NoInProg)) > Convert.ToDecimal(strGraterthan))));
                                }
                                if (caseMstQuestion != null)
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, caseMstQuestion.Count);
                                else
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                                break;
                            case Consts.EligQues.ProgramIncome:
                                QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);
                                if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)).Equals(Convert.ToDecimal(strEqual))));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.ProgIncome == string.Empty ? 0 : Convert.ToDecimal(u.ProgIncome)) > Convert.ToDecimal(strGraterthan))));
                                }
                                if (caseMstQuestion != null)
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, caseMstQuestion.Count);
                                else
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                                break;
                            case Consts.EligQues.ZipCode:
                                QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);

                                if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)).Equals(Convert.ToDecimal(strEqual))));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.Zip == string.Empty ? 0 : Convert.ToDecimal(u.Zip)) > Convert.ToDecimal(strGraterthan))));
                                }
                                if (caseMstQuestion != null)
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, caseMstQuestion.Count);
                                else
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                                break;
                            case Consts.EligQues.PreassTotal:
                                QuestionNumericConditions(EligQues.EligRespType.Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);
                                if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)).Equals(Convert.ToDecimal(strEqual))));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => (Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) > Convert.ToDecimal(strGraterthan)));
                                }
                                else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) < Convert.ToDecimal(strLessthan)));
                                }
                                else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                                {
                                    caseMstQuestion = caseMstList.FindAll(u => ((Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.PressTotal == string.Empty ? 0 : Convert.ToDecimal(u.PressTotal)) > Convert.ToDecimal(strGraterthan))));
                                }
                                if (caseMstQuestion != null)
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, caseMstQuestion.Count);
                                else
                                    strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                                break;
                            case Consts.EligQues.County:
                                caseMstQuestion = caseMstList.FindAll(u => u.County.Equals(EligitemQuestion.EligDDTextResp));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, caseMstQuestion.Count);
                                break;
                            case Consts.EligQues.City:
                                caseMstQuestion = caseMstList.FindAll(u => u.City.ToUpper().Equals(EligitemQuestion.EligDDTextResp.ToUpper()));
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, caseMstQuestion.Count);
                                break;

                        }
                    }// if condition closed
                    else if (EligitemQuestion.EligAgyCode.StartsWith("C"))
                    {
                        List<CustomQuestionsEntity> customEligQuesAll = null;

                        if (EligitemQuestion.EligResponseType.ToString() == "N")
                        {
                            QuestionNumericConditions(EligitemQuestion.EligResponseType.ToString().Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);

                            if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                            {
                                customEligQuesAll = custQuestionActCust.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && (Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)).Equals(Convert.ToDecimal(strEqual))));
                            }
                            else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                            {
                                customEligQuesAll = custQuestionActCust.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && (Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) < Convert.ToDecimal(strLessthan)));
                            }
                            else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                            {
                                customEligQuesAll = custQuestionActCust.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && (Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) > Convert.ToDecimal(strGraterthan)));
                            }
                            else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                            {
                                customEligQuesAll = custQuestionActCust.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && ((Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) < Convert.ToDecimal(strLessthan)));
                            }
                            else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                            {
                                customEligQuesAll = custQuestionActCust.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && ((Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) > Convert.ToDecimal(strGraterthan)));
                            }
                            else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                            {
                                customEligQuesAll = custQuestionActCust.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && ((Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) < Convert.ToDecimal(strLessthan)));
                            }
                            else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                            {
                                customEligQuesAll = custQuestionActCust.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && ((Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) > Convert.ToDecimal(strGraterthan))));
                            }
                            strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, customEligQuesAll.Count);
                        }
                        else if (EligitemQuestion.EligResponseType.ToString() == "D")
                        {
                            if (EligitemQuestion.EligMemberAccess.ToString().Trim() == "*" || EligitemQuestion.EligMemberAccess.ToString().Trim() == "1")
                            {
                                customEligQuesAll = custQuestionActCust.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && (Convert.ToString(u.ACTMULTRESP.Trim()).Equals(EligitemQuestion.EligDDTextResp.Trim())));
                            }
                            else
                            {
                                if (EligitemQuestion.EligMemberAccess.ToString().Trim() == "A")
                                    customEligQuesAll = custQuestionActCust.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && u.ACTSNPFAMILYSEQ.Equals("9999999") && (Convert.ToString(u.ACTMULTRESP.Trim()).Equals(EligitemQuestion.EligDDTextResp.Trim())));
                                else
                                    customEligQuesAll = custQuestionActCust.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && u.ACTSNPFAMILYSEQ.Equals("8888888") && (Convert.ToString(u.ACTMULTRESP.Trim()).Equals(EligitemQuestion.EligDDTextResp.Trim())));
                            }
                            if (customEligQuesAll != null)
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, customEligQuesAll.Count);
                            else
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                        }
                        else if (EligitemQuestion.EligResponseType.ToString() == "A")
                        {
                            if (EligitemQuestion.EligMemberAccess.ToString().Trim() == "*" || EligitemQuestion.EligMemberAccess.ToString().Trim() == "1")
                            {
                                customEligQuesAll = custQuestionActCust.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && u.ACTALPHARESP.Trim().Equals(EligitemQuestion.EligDDTextResp));
                            }
                            else
                            {
                                if (EligitemQuestion.EligMemberAccess.ToString().Trim() == "A")
                                    customEligQuesAll = custQuestionActCust.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && u.ACTSNPFAMILYSEQ.Equals("9999999") && (Convert.ToString(u.ACTMULTRESP.Trim()).Equals(EligitemQuestion.EligDDTextResp.Trim())));
                                else
                                    customEligQuesAll = custQuestionActCust.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && u.ACTSNPFAMILYSEQ.Equals("8888888") && (Convert.ToString(u.ACTMULTRESP.Trim()).Equals(EligitemQuestion.EligDDTextResp.Trim())));
                            }

                            if (customEligQuesAll != null)
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, customEligQuesAll.Count);
                            else
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                        }

                    }
                    else
                    {
                        List<CustomQuestionsEntity> customEligQuesAll = null;

                        if (EligitemQuestion.EligResponseType.ToString() == "N")
                        {
                            QuestionNumericConditions(EligitemQuestion.EligResponseType.ToString().Trim(), EligitemQuestion, out strEqual, out strLessthan, out strGraterthan);

                            if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan == string.Empty)//Equal
                            {
                                customEligQuesAll = custPrerespQuestion.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && (Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)).Equals(Convert.ToDecimal(strEqual))));
                            }
                            else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Lessthan
                            {
                                customEligQuesAll = custPrerespQuestion.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && (Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) < Convert.ToDecimal(strLessthan)));
                            }
                            else if (strEqual == string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Greaterthan
                            {
                                customEligQuesAll = custPrerespQuestion.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && (Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) > Convert.ToDecimal(strGraterthan)));
                            }
                            else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan == string.Empty)//Equal and Lessthan
                            {
                                customEligQuesAll = custPrerespQuestion.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && ((Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) < Convert.ToDecimal(strLessthan)));
                            }
                            else if (strEqual != string.Empty && strLessthan == string.Empty && strGraterthan != string.Empty)//Equal and graterthan
                            {
                                customEligQuesAll = custPrerespQuestion.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && ((Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) == Convert.ToDecimal(strEqual)) && Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) > Convert.ToDecimal(strGraterthan)));
                            }
                            else if (strEqual == string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Leasthan and greaterthan
                            {
                                customEligQuesAll = custPrerespQuestion.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && ((Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) > Convert.ToDecimal(strGraterthan)) && Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) < Convert.ToDecimal(strLessthan)));
                            }
                            else if (strEqual != string.Empty && strLessthan != string.Empty && strGraterthan != string.Empty)//Equal and Leasthan and greaterthan
                            {
                                customEligQuesAll = custPrerespQuestion.FindAll(u => (u.ACTCODE.Equals(EligitemQuestion.EligAgyCode)) && ((Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) == Convert.ToDecimal(strEqual)) && (Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) < Convert.ToDecimal(strLessthan)) && (Convert.ToDecimal(u.ACTNUMRESP == string.Empty ? 0 : Convert.ToDecimal(u.ACTNUMRESP)) > Convert.ToDecimal(strGraterthan))));
                            }
                            strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, customEligQuesAll.Count);
                        }
                        else if (EligitemQuestion.EligResponseType.ToString() == "D")
                        {
                            if (EligitemQuestion.EligMemberAccess.ToString().Trim() == "*" || EligitemQuestion.EligMemberAccess.ToString().Trim() == "1")
                            {
                                customEligQuesAll = custPrerespQuestion.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && (Convert.ToString(u.ACTMULTRESP.Trim()).Equals(EligitemQuestion.EligDDTextResp.Trim())));
                            }
                            else
                            {
                                if (EligitemQuestion.EligMemberAccess.ToString().Trim() == "A")
                                    customEligQuesAll = custPrerespQuestion.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && u.ACTSNPFAMILYSEQ.Equals("7777777") && (Convert.ToString(u.ACTMULTRESP.Trim()).Equals(EligitemQuestion.EligDDTextResp.Trim())));
                                else
                                    customEligQuesAll = custPrerespQuestion.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && u.ACTSNPFAMILYSEQ.Equals("7777777") && (Convert.ToString(u.ACTMULTRESP.Trim()).Equals(EligitemQuestion.EligDDTextResp.Trim())));
                            }
                            if (customEligQuesAll != null)
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, customEligQuesAll.Count);
                            else
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                        }
                        else if (EligitemQuestion.EligResponseType.ToString() == "A")
                        {
                            if (EligitemQuestion.EligMemberAccess.ToString().Trim() == "*" || EligitemQuestion.EligMemberAccess.ToString().Trim() == "1")
                            {
                                customEligQuesAll = custPrerespQuestion.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && u.ACTALPHARESP.Trim().Equals(EligitemQuestion.EligDDTextResp));
                            }
                            else
                            {
                                if (EligitemQuestion.EligMemberAccess.ToString().Trim() == "A")
                                    customEligQuesAll = custPrerespQuestion.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && u.ACTSNPFAMILYSEQ.Equals("7777777") && (Convert.ToString(u.ACTMULTRESP.Trim()).Equals(EligitemQuestion.EligDDTextResp.Trim())));
                                else
                                    customEligQuesAll = custPrerespQuestion.FindAll(u => u.ACTCODE.Equals(EligitemQuestion.EligAgyCode) && u.ACTSNPFAMILYSEQ.Equals("7777777") && (Convert.ToString(u.ACTMULTRESP.Trim()).Equals(EligitemQuestion.EligDDTextResp.Trim())));
                            }

                            if (customEligQuesAll != null)
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, customEligQuesAll.Count);
                            else
                                strQuestionResult = QuestionResultstatus(EligitemQuestion.EligMemberAccess, 0);
                        }

                    }
                }
            }
            return strQuestionResult;
        }


        private string QuestionResultstatus(string strQuestionType, int TotalResultCount)
        {
            string strResult = "N";
            if (strQuestionType == "*")
            {
                if (caseMstList[0].NoInhh != null)
                {
                    if (Convert.ToInt32(caseMstList[0].NoInhh) == TotalResultCount)
                        strResult = "Y";
                }
            }
            else if (strQuestionType == "1" || strQuestionType == "H" || strQuestionType == "A")
            {
                if (TotalResultCount > 0)
                {
                    strResult = "Y";
                }
            }
            return strResult;
        }

        #endregion


        #region RankCategryProperties

        public List<RankCatgEntity> propRankscategory { get; set; }
        public List<CustomQuestionsEntity> propcustResponce { get; set; }
        public List<CustRespEntity> propCustResponceList { get; set; }
        public List<AGYTABSEntity> propAgyTabsList { get; set; }
        public List<HierarchyEntity> prophierarchyEntity { get; set; }
        public List<CustomQuestionsEntity> propPresResponce { get; set; }
        public List<CustRespEntity> propPresResponceList { get; set; }

        #endregion

        #region RankCateogryPointsCalculation

        List<CommonEntity> ListchildRiskPoints = new List<CommonEntity>();

        private void CalculateRankPoints()
        {
            try
            {

                RankCategoryFillData();

                string strAgency = BaseForm.BaseAgency.ToString();
                List<RankCatgEntity> rankCatg = propRankscategory.FindAll(u => u.Agency.Trim() == BaseForm.BaseAgency && u.SubCode.Trim() == string.Empty);
                if (rankCatg.Count == 0)
                {
                    rankCatg = propRankscategory.FindAll(u => u.Agency.Trim() == "**" && u.SubCode.Trim() == string.Empty);
                    strAgency = "**";
                }
                //List<RNKCRIT2Entity> RnkQuesFledsEntity = _model.SPAdminData.GetRanksCrit2Data("RANKQUES", BaseForm.BaseAgency, string.Empty);
                //List<RNKCRIT2Entity> RnkQuesFledsAllDataEntity = _model.SPAdminData.GetRanksCrit2Data(string.Empty, BaseForm.BaseAgency, string.Empty);
                //List<RNKCRIT2Entity> RnkCustFldsAllDataEntity = _model.SPAdminData.GetRanksCrit2Data("CUSTFLDS", BaseForm.BaseAgency, string.Empty);
                ChldMstEntity chldMst = _model.ChldMstData.GetChldMstDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);

                foreach (RankCatgEntity item in rankCatg)
                {

                    List<RNKCRIT2Entity> RnkQuesFledsEntity = _model.SPAdminData.GetRanksCrit2Data("RANKQUES", BaseForm.BaseAgency, item.Code.Trim().ToString());
                    List<RNKCRIT2Entity> RnkQuesFledsAllDataEntity = _model.SPAdminData.GetRanksCrit2Data(string.Empty, BaseForm.BaseAgency, item.Code.Trim().ToString());
                    List<RNKCRIT2Entity> RnkCustFldsAllDataEntity = _model.SPAdminData.GetRanksCrit2Data("CUSTFLDS", BaseForm.BaseAgency, item.Code.Trim().ToString());


                    List<RNKCRIT2Entity> RnkQuesFledsDataCodeEntity = RnkQuesFledsAllDataEntity;//RnkQuesFledsAllDataEntity.FindAll(u => u.RankCategory.Trim().ToString() == item.Code.Trim().ToString());
                    List<RNKCRIT2Entity> RnkCustFldsDataCodeEntity = RnkCustFldsAllDataEntity;//RnkCustFldsAllDataEntity.FindAll(u => u.RankCategory.Trim().ToString() == item.Code.Trim().ToString());

                    ListViewItem objItem = null;
                    DataTable dt = GetRankCategoryDetails(BaseForm.BaseCaseMstListEntity[0], BaseForm.BaseCaseSnpEntity, chldMst, RnkQuesFledsEntity, RnkQuesFledsDataCodeEntity, RnkCustFldsDataCodeEntity);

                    ListchildRiskPoints.Add(new CommonEntity(item.Code, intRankPoint.ToString()));

                }



                string strRank1, strRank2, strRank3, strRank4, strRank5, strRank6;

                strRank1 = "0"; strRank2 = "0"; strRank3 = "0"; strRank4 = "0"; strRank5 = "0"; strRank6 = "0";
                foreach (CommonEntity lstitem in ListchildRiskPoints)
                {
                    switch (lstitem.Code.Trim())
                    {
                        case "01":
                        case "1":
                            strRank1 = lstitem.Desc;
                            break;
                        case "02":
                        case "2":
                            strRank2 = lstitem.Desc;
                            break;
                        case "03":
                        case "3":
                            strRank3 = lstitem.Desc;
                            break;
                        case "04":
                        case "4":
                            strRank4 = lstitem.Desc;
                            break;
                        case "05":
                        case "5":
                            strRank5 = lstitem.Desc;
                            break;
                        case "06":
                        case "6":
                            strRank6 = lstitem.Desc;
                            break;
                    }

                }
                StringBuilder strMstAppl = new StringBuilder();
                strMstAppl.Append("<Applicants>");
                strMstAppl.Append("<Details MSTApplDetails = \"" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (BaseForm.BaseYear.Trim() == string.Empty ? "    " : BaseForm.BaseYear.Trim()) + BaseForm.BaseApplicationNo + "\" MST_RANK1 = \"" + strRank1 + "\" MST_RANK2 = \"" + strRank2 + "\" MST_RANK3 = \"" + strRank3 + "\" MST_RANK4 = \"" + strRank4 + "\" MST_RANK5 = \"" + strRank5 + "\" MST_RANK6 = \"" + strRank6 + "\"   />");
                strMstAppl.Append("</Applicants>");

                if (_model.CaseMstData.UpdateCaseMstRanks(strMstAppl.ToString(), "Single"))
                {

                }




            }
            catch (Exception ex)
            {

                //  MessageBox.Show(ex.Message);
            }

        }



        public void RankCategoryFillData()
        {
            propRankscategory = _model.SPAdminData.Browse_RankCtg();
            propcustResponce = _model.CaseMstData.GetCustomQuestionAnswersRank(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, BaseForm.BaseCaseMstListEntity[0].FamilySeq, string.Empty, string.Empty);
            propPresResponce = _model.CaseMstData.GetPreassesQuestionAnswersRank(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, BaseForm.BaseCaseMstListEntity[0].FamilySeq, string.Empty, "PRESRESP");

            CustRespEntity SearchCustresp = new CustRespEntity(true);
            SearchCustresp.ScrCode = "CASE2001";
            propCustResponceList = _model.FieldControls.Browse_CUSTRESP(SearchCustresp, "Browse");
            SearchCustresp.ScrCode = "PREASSES";
            propPresResponceList = _model.FieldControls.Browse_CUSTRESP(SearchCustresp, "Browse");

            AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
            AdhocData AgyTabs = new AdhocData();
            propAgyTabsList = AgyTabs.Browse_AGYTABS(searchAgytabs);
        }


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
                dtRankSubDetails.Columns.Add("TableCode", typeof(string));
                dtRankSubDetails.Columns.Add("ResponceType", typeof(string));

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

                List<CustomQuestionsEntity> custResponses = propcustResponce;
                List<CustomQuestionsEntity> custpresResponses = propPresResponce;

                List<RNKCRIT2Entity> RnkQuesSearchList;
                propMstRank = caseMst;
                RNKCRIT2Entity RnkQuesSearch = null;
                // List<RNKCRIT2Entity> RnkQuesCaseSnp = null;
                int intRankSnpPoints = 0;
                string strApplicationcode = string.Empty;
                foreach (RNKCRIT2Entity rnkQuesData in RnkQuesFledsEntity)
                {
                    intRankSnpPoints = 0;
                    DataRow dr = dtRankSubDetails.NewRow();
                    RnkQuesSearch = null;
                    dr["FieldCode"] = rnkQuesData.RankFldDesc.ToString();
                    dr["TableCode"] = rnkQuesData.RankFldName.Trim();
                    dr["ResponceType"] = rnkQuesData.RankFldRespType.Trim();

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
                            if (prophierarchyEntity.Count > 0)
                            {
                                HierarchyEntity hierchy = prophierarchyEntity.Find(u => u.CaseWorker == caseMst.Verifier.Trim());
                                if (hierchy != null)
                                {
                                    dr["FieldDesc"] = hierchy.HirarchyName.ToString();
                                }
                                else
                                    dr["FieldDesc"] = caseMst.Verifier.Trim();

                            }
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
                        ////case Consts.RankQues.MPTJOB:
                        ////    agytabsMstDesc = propAgyTabsList.Find(u => u.Tabs_Type == rnkQuesData.RankAgyCode && u.Table_Code == caseMst.PTJob.Trim());
                        ////    if (agytabsMstDesc != null)
                        ////    {
                        ////        dr["FieldDesc"] = agytabsMstDesc.Code_Desc.Trim();
                        ////    }
                        ////    else
                        ////        dr["FieldDesc"] = string.Empty;
                        ////    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.PTJob.Trim());
                        ////    break;
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
                        case Consts.RankQues.CDentalCoverage:
                            if (chldMst != null)
                            {
                                dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, (chldMst.DentalCoverage.Trim() == "True" ? "1" : "0"));
                                // dr["FieldDesc"] = chldMst.DentalCoverage.Trim() == "True" ? "Seleted" : "UnSelected";
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
                                // dr["FieldDesc"] = chldMst.Disability.Trim() == "True" ? "Seleted" : "UnSelected";
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
                                dr["FieldDesc"] = GetSnpAgyTabDesc(rnkQuesData, (chldMst.MedCoverage.Trim() == "True" ? "1" : "0"));
                                //dr["FieldDesc"] = chldMst.MedCoverage.Trim() == "True" ? "Seleted" : "UnSelected";
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
                                        //dr1["FieldDesc"] = propPresResponceList.Find(u => u.ResoCode.Trim() == responceQuestion.ACTCODE && u.DescCode == responceQuestion.ACTMULTRESP).RespDesc.ToString();
                                        CustRespEntity custrespent = propPresResponceList.Find(u => u.ResoCode.Trim() == responceQuestion.ACTCODE && u.DescCode == responceQuestion.ACTMULTRESP);
                                        if (custrespent != null)
                                            dr1["FieldDesc"] = custrespent.RespDesc.ToString();
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
                                    CustRespEntity custrespent = propPresResponceList.Find(u => u.ResoCode.Trim() == responceQuestion.ACTCODE && u.DescCode == item);
                                    if (custrespent != null)
                                        dr2["FieldDesc"] = custrespent.RespDesc.ToString();
                                    else
                                        dr2["FieldDesc"] = string.Empty;

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

                            //custResponcesearch = null;
                            //rnkPoints = null;
                            //strQuestionType = rnkCustFldsFilterCode[0].RankFldRespType.Trim();
                            //dr1["FieldCode"] = rnkCustFldsFilterCode[0].RankFldDesc.Trim();

                            //switch (rnkCustFldsFilterCode[0].RankFldRespType.Trim())
                            //{
                            //    case "D":
                            //    case "L":
                            //        rnkPoints = rnkCustFldsFilterCode.Find(u => u.RespCd.Trim() == responceQuestion.ACTMULTRESP.Trim());
                            //        //custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && u.ACTMULTRESP.Trim() == item.RespCd.Trim());
                            //        dr1["FieldDesc"] = propCustResponceList.Find(u => u.ResoCode.Trim() == responceQuestion.ACTCODE && u.DescCode == responceQuestion.ACTMULTRESP).RespDesc.ToString();
                            //        break;
                            //    case "N":
                            //        rnkPoints = rnkCustFldsFilterCode.Find(u => Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(responceQuestion.ACTNUMRESP) && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(responceQuestion.ACTNUMRESP));
                            //        // custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && Convert.ToDecimal(u.GtNum) >= Convert.ToDecimal(item.ACTNUMRESP) && Convert.ToDecimal(u.LtNum) <= Convert.ToDecimal(item.ACTNUMRESP));
                            //        dr1["FieldDesc"] = responceQuestion.ACTNUMRESP;
                            //        break;
                            //    case "T":
                            //    case "B":
                            //        rnkPoints = rnkCustFldsFilterCode.Find(u => Convert.ToDateTime(u.GtDate) <= Convert.ToDateTime(responceQuestion.ACTDATERESP) && Convert.ToDateTime(u.LtDate) >= Convert.ToDateTime(responceQuestion.ACTDATERESP));
                            //        // custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && Convert.ToDateTime(u.GtDate) >= Convert.ToDateTime(item.ACTDATERESP) && Convert.ToDateTime(u.LtDate) <= Convert.ToDateTime(item.ACTNUMRESP));
                            //        dr1["FieldDesc"] = responceQuestion.ACTDATERESP;
                            //        break;
                            //}
                            //if (rnkPoints != null)
                            //{
                            //    dr1["Points"] = rnkPoints.Points;
                            //    intRankPoint = intRankPoint + Convert.ToInt32(rnkPoints.Points);
                            //}
                            //else
                            //{
                            //    dr1["Points"] = "0";
                            //}
                            //dtRankSubDetails.Rows.Add(dr1)


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
                                        dr1["FieldDesc"] = propCustResponceList.Find(u => u.ResoCode.Trim() == responceQuestion.ACTCODE && u.DescCode == responceQuestion.ACTMULTRESP).RespDesc.ToString();
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



        private int CaseSnpDetailsCalc(List<RNKCRIT2Entity> rnkCaseSnp, List<CaseSnpEntity> caseSnpDetails, string strApplicantCode, string FilterCode, string ResponceType, out string strResponseDesc)
        {
            int intSnpPoints = 0;
            string strResponceCode = strApplicantCode;
            string strResponceData = strApplicantCode;
            foreach (RNKCRIT2Entity item in rnkCaseSnp)
            {
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
                List<ZipCodeEntity> zipCodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(BaseForm.BaseCaseMstListEntity[0].Zip, string.Empty, string.Empty, string.Empty);
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

        string strFundDefaultCode = "0";
        public void fillcombo()
        {
            DataSet ds = null;
            DataTable dt = null;
            cmbVerifier.SelectedIndexChanged -= new EventHandler(cmbVerifier_SelectedIndexChanged);
            cmbFundingsource.Items.Clear();
            List<CommonEntity> lookUpfundingResource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
            foreach (CommonEntity agyEntity in lookUpfundingResource)
            {
                cmbFundingsource.Items.Add(new Captain.Common.Utilities.ListItem(agyEntity.Desc, agyEntity.Code));
                if (agyEntity.Default.Equals("Y"))
                    strFundDefaultCode = agyEntity.Code;
            }
            if (propAgencyControlDetails.AgyShortName.ToUpper() == "SHI")
            {
                cmbFundingsource.Items.Insert(0, new Captain.Common.Utilities.ListItem("", "0"));
            }
            else
            {
                cmbFundingsource.Items.Insert(0, new Captain.Common.Utilities.ListItem("None", "0"));
            }
            cmbFundingsource.SelectedIndex = 0;


            cmbVerifier.Items.Clear();
            cmbVerifier.ColorMember = "FavoriteColor";
            hierarchyEntity = _model.CaseMstData.GetCaseWorker(strVerfierFormat, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            prophierarchyEntity = hierarchyEntity;
            string strCaseworker = string.Empty;
            foreach (HierarchyEntity caseworker in hierarchyEntity)
            {
                if (strCaseworker != caseworker.CaseWorker.ToString())
                {
                    strCaseworker = caseworker.CaseWorker.ToString();
                    cmbVerifier.Items.Add(new Captain.Common.Utilities.ListItem(caseworker.HirarchyName.ToString(), caseworker.CaseWorker.ToString(), caseworker.InActiveFlag, caseworker.InActiveFlag.Equals("N") ? Color.Black : Color.Red));
                }
                if (caseworker.UserID.Trim().ToString().ToUpper() == BaseForm.UserID.ToUpper().Trim().ToString())
                {
                    strCaseWorkerDefaultCode = caseworker.CaseWorker == null ? "0" : caseworker.CaseWorker;
                    strCaseWorkerDefaultStartCode = caseworker.CaseWorker == null ? "0" : caseworker.CaseWorker;
                }
            }
            cmbVerifier.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            CommonFunctions.SetComboBoxValue(cmbVerifier, strCaseWorkerDefaultCode);

            cmbCategorical.Items.Clear();
            lookUpCategrcalEligiblity = _model.lookupDataAccess.GetCategrcalEligiblityNew();
            foreach (CommonEntity agyEntity in lookUpCategrcalEligiblity)
            {
                cmbCategorical.Items.Add(new Captain.Common.Utilities.ListItem(agyEntity.Desc, agyEntity.Code));
            }
            cmbCategorical.Items.Insert(0, new Captain.Common.Utilities.ListItem("  ", "0"));
            cmbCategorical.SelectedIndex = 0;

            cmbMeal.Items.Clear();
            MealEntity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0009", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);
            foreach (CommonEntity item in MealEntity)
            {
                Captain.Common.Utilities.ListItem li = new Captain.Common.Utilities.ListItem(item.Desc, item.Code);
                cmbMeal.Items.Add(li);
                if (item.Default.Equals("Y")) cmbMeal.SelectedItem = li;
            }
            if (MealEntity.FindAll(u => u.Default.Equals("Y")).ToList().Count == 0)
            {
                cmbMeal.Items.Insert(0, new Captain.Common.Utilities.ListItem("  ", "0"));
                cmbMeal.SelectedIndex = 0;
            }



            cmbVerifier.SelectedIndexChanged += new EventHandler(cmbVerifier_SelectedIndexChanged);
            //list<commonentity> commonmeal = _model.lookupdataaccess.getmeal();
            //foreach (commonentity meal in commonmeal)
            //{
            //    cmbmeal.items.add(new listitem(meal.desc.tostring(), meal.code.tostring()));
            //}
            //cmbmeal.items.insert(0, new listitem("select one", "0"));
            //cmbmeal.SelectedIndex = 0;
        }

        private void fillIncomeControl()
        {
            cmbCategorical.SelectedIndex = 0;
            cmbFundingsource.SelectedIndex = 0;
            if (MealEntity.FindAll(u => u.Default.Equals("Y")).ToList().Count > 0)
            {
                CommonFunctions.SetComboBoxValue(cmbMeal, MealEntity.Find(u => u.Default.Equals("Y")).Code.ToString());
            }
            else
            {
                if (cmbMeal.Items.Count > 0)
                    CommonFunctions.SetComboBoxValue(cmbMeal, "0");
            }

            chkCheckStubs.Checked = false;
            chkLetter.Checked = false;
            chkOther.Checked = false;
            chkTaxReturn.Checked = false;
            chk_self_Decl.Checked = false;
            chkw2.Checked = false;
            CommonFunctions.SetComboBoxValue(cmbVerifier, strCaseWorkerDefaultStartCode);
            txtCmi.Text = string.Empty;
            txtFedOmb.Text = string.Empty;
            txtHud.Text = string.Empty;
            txtIncome.Text = string.Empty;
            txtInprogram.Text = string.Empty;
            txtSmi.Text = string.Empty;
            calReverficationdate.Checked = false;
            calVerificationDate.Checked = false;
            calVerificationDate.Enabled = false;
            if (privilege.ChangePriv.Equals("false"))
            {
                btnSave.Visible = false;
                btnCancel.Text = "Close";
            }
            else
            {
                btnCancel.Text = "Close";
            }
            if (privilege.AddPriv.Equals("false"))
            {
                btnSave.Visible = false;
                btnCancel.Text = "Close";
            }
            else
            {
                btnCancel.Text = "Close";
            }

            //if (CaseMSTEntity != null)
            //{
            if (caseSnpList.Count > 0)
            {
                List<CaseSnpEntity> listCasesnp = caseSnpList.FindAll(u => u.ProgIncome != string.Empty);
                decimal snppronginccome = listCasesnp.Sum(u => Convert.ToDecimal(u.ProgIncome));
                txtIncome.Text = snppronginccome.ToString();//CaseMSTEntity.ProgIncome;                           
                txtInprogram.Text = caseSnpList.Count(u => u.Exclude == "N" && u.Status.Trim().ToUpper() != "I" && u.SsnReason != "Q").ToString();//caseSnpList.Count(u => u.Exclude == "N").ToString();

            }
            //}

            //Commented by Sudheer on 11/28/2022

            //if (ProgramDefination != null)
            //{
            //    if (ProgramDefination.DepFEDUsed == "Y")
            //    {
            //        lblFedOmbReq.Visible = true;
            //        btn1Omb.Enabled = true;

            //    }
            //    if (ProgramDefination.DepCMIUsed == "Y")
            //    {
            //        btn2Cmi.Enabled = true;
            //    }
            //    if (ProgramDefination.DepSMIUsed == "Y")
            //    {
            //        btn3Smi.Enabled = true;
            //    }
            //    if (ProgramDefination.DepHUDUsed == "Y")
            //    {
            //        btn4Hud.Enabled = true;
            //    }

            //}
        }

        private void fillGridData()
        {

            //if (privilege.DelPriv.Equals("false"))
            //{
            dataGridCaseIncomeVer.Columns["Delete"].Visible = false;
            //}
            //else
            //{
            //    dataGridCaseIncomeVer.Columns["Delete"].Visible = true;
            //}

            List<CaseVerEntity> caseVerList = _model.CaseMstData.GetCASEVeradpyalst(MainMenuAgency, MainMenuDept, MainMenuProgram, MainMenuYear, ApplicationNo, string.Empty, string.Empty);
            dataGridCaseIncomeVer.Rows.Clear();
            //string photocross = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("cross.ico");
            //string phototick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");

            string photocross = "icon-cross";// "Resources/Images/cross2.png";
            string phototick = "icon-gridtick"; // "Resources/Images/tick2.png";

            foreach (CaseVerEntity caseVer in caseVerList)
            {

                string strAltDate = LookupDataAccess.Getdate(caseVer.VerifyDate);
                VerifyCheckDate = LookupDataAccess.Getdate(caseVerList[0].VerifyDate);
                string VerOMB = string.Empty;
                if (BaseForm.BaseAgencyControlDetails.ACR_POV_WITH_DEC == "Y")
                    VerOMB = caseVer.VerOmb;
                else
                    VerOMB = Math.Abs((int)decimal.Parse(caseVer.VerOmb)).ToString();


                int rowIndex = dataGridCaseIncomeVer.Rows.Add(strAltDate, caseVer.IncomeAmount, caseVer.NoInhh, VerOMB + "%");
                if (caseVer.VerifyW2 == "Y")
                    dataGridCaseIncomeVer.Rows[rowIndex].Cells["W21"].Value = phototick; // System.Drawing.Image.FromFile(Application.MapPath(phototick));
                else
                    dataGridCaseIncomeVer.Rows[rowIndex].Cells["W21"].Value = photocross; // System.Drawing.Image.FromFile(Application.MapPath(photocross));
                if (caseVer.VerifyCheckStub == "Y")
                    dataGridCaseIncomeVer.Rows[rowIndex].Cells["Stubs"].Value = phototick; // System.Drawing.Image.FromFile(Application.MapPath(phototick));
                else
                    dataGridCaseIncomeVer.Rows[rowIndex].Cells["Stubs"].Value = photocross;// System.Drawing.Image.FromFile(Application.MapPath(photocross));
                if (caseVer.VerifyTaxReturn == "Y")
                    dataGridCaseIncomeVer.Rows[rowIndex].Cells["TaxReturn"].Value = phototick; // System.Drawing.Image.FromFile(Application.MapPath(phototick));
                else
                    dataGridCaseIncomeVer.Rows[rowIndex].Cells["TaxReturn"].Value = photocross; // System.Drawing.Image.FromFile(Application.MapPath(photocross));
                if (caseVer.VerifyLetter == "Y")
                    dataGridCaseIncomeVer.Rows[rowIndex].Cells["Letter"].Value = phototick; // System.Drawing.Image.FromFile(Application.MapPath(phototick));
                else
                    dataGridCaseIncomeVer.Rows[rowIndex].Cells["Letter"].Value = photocross; // System.Drawing.Image.FromFile(Application.MapPath(photocross));
                if (caseVer.VerifyOther == "Y")
                    dataGridCaseIncomeVer.Rows[rowIndex].Cells["Other"].Value = phototick; // System.Drawing.Image.FromFile(Application.MapPath(phototick));
                else
                    dataGridCaseIncomeVer.Rows[rowIndex].Cells["Other"].Value = photocross; // System.Drawing.Image.FromFile(Application.MapPath(photocross));
                if (caseVer.ReverifyDate != "")
                    dataGridCaseIncomeVer.Rows[rowIndex].Cells["Reverified"].Value = phototick; // System.Drawing.Image.FromFile(Application.MapPath(phototick));
                else
                    dataGridCaseIncomeVer.Rows[rowIndex].Cells["Reverified"].Value = photocross; // System.Drawing.Image.FromFile(Application.MapPath(photocross));

                if (caseVer.VerifySelfDecl == "Y")
                    dataGridCaseIncomeVer.Rows[rowIndex].Cells["gvtSelfDeclaration"].Value = phototick;// System.Drawing.Image.FromFile(Application.MapPath(phototick));
                else
                    dataGridCaseIncomeVer.Rows[rowIndex].Cells["gvtSelfDeclaration"].Value = photocross; // System.Drawing.Image.FromFile(Application.MapPath(photocross));

                dataGridCaseIncomeVer.Rows[rowIndex].Cells["MealElig"].Value = MealTypeDesc(caseVer.MealElig);
                dataGridCaseIncomeVer.Rows[rowIndex].Tag = caseVer;
                //dataGridCaseIncomeVer.ItemsPerPage = 100;
                CommonFunctions.setTooltip(rowIndex, caseVer.AddOperator, caseVer.DateAdd, caseVer.LstcOperator, caseVer.DateLstc, dataGridCaseIncomeVer);
            }
            if (caseVerList.Count == 0)
            {

            }
            else
            {

            }

        }

        private string MealTypeDesc(string strMealType)
        {
            string strReturnMealType = string.Empty;
            if (MealEntity.Count > 0)
            {
                CommonEntity commmeal = MealEntity.Find(u => u.Code.Trim() == strMealType);
                if (commmeal != null)
                {
                    strReturnMealType = commmeal.Desc;
                }
            }
            //switch (strMealType)
            //{
            //    case "N":
            //        strReturnMealType = "None";
            //        break;
            //    case "F":
            //        strReturnMealType = "Free";
            //        break;
            //    case "R":
            //        strReturnMealType = "Reduced";
            //        break;
            //    case "P":
            //        strReturnMealType = "Paid";
            //        break;
            //    case "":
            //        strReturnMealType = "None";
            //        break;
            //}
            return strReturnMealType;
        }

        private bool ValidateForm()
        {
            bool isValid = true;


            if (lblVerificationDateReq.Visible && calVerificationDate.Checked == false)
            {
                _errorProvider.SetError(calVerificationDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblVerfificationDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                if (strMode != "Edit")
                {
                    string selectedDate = calVerificationDate.Text;
                    bool flag = false;
                    foreach (DataGridViewRow row in dataGridCaseIncomeVer.Rows)
                    {
                        if (row.Tag is CaseVerEntity)
                        {

                            string strVerifyDate = LookupDataAccess.Getdate((row.Tag as CaseVerEntity).VerifyDate).ToString();
                            if (Convert.ToDateTime(strVerifyDate) == calVerificationDate.Value)
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    _errorProvider.SetError(calVerificationDate, null);
                    if (flag)
                    {
                        _errorProvider.SetError(calVerificationDate, "Verification details already exists for this date");
                        isValid = false;
                    }
                    else
                    {
                        if (calVerificationDate.Value > DateTime.Now)
                        {
                            _errorProvider.SetError(calVerificationDate, "Future date not allowed");
                            isValid = false;

                        }
                        else
                        {
                            _errorProvider.SetError(calVerificationDate, null);
                        }
                    }
                }
                else
                {
                    _errorProvider.SetError(calVerificationDate, null);
                }

            }

            if (lblReverificationReq.Visible && calReverficationdate.Checked == false)
            {
                _errorProvider.SetError(calReverficationdate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblReverificationDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                if (calReverficationdate.Checked == true)
                {
                    if (calVerificationDate.Value > calReverficationdate.Value)
                    {
                        _errorProvider.SetError(calReverficationdate, "Reverification Date should not be prior to Verification");
                        isValid = false;

                    }
                    else
                    {
                        _errorProvider.SetError(calReverficationdate, null);
                    }

                }
                else
                {
                    _errorProvider.SetError(calReverficationdate, null);
                }
            }

            if (lblVerifierReq.Visible && ((Captain.Common.Utilities.ListItem)cmbVerifier.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbVerifier, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblVerifier.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbVerifier, null);
            }
            if (propAgencyControlDetails.AgyShortName.ToUpper() == "SHI")
            {
                if (lblFundingSourceReq.Visible && ((Captain.Common.Utilities.ListItem)cmbFundingsource.SelectedItem).Value.ToString() == "0")
                {
                    _errorProvider.SetError(cmbFundingsource, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFundingSource.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbFundingsource, null);
                }
            }

            if ((lblIncomeVerifiedReq.Visible) && ((chkw2.Checked == false && chkTaxReturn.Checked == false && chkOther.Checked == false && chkLetter.Checked == false && chkCheckStubs.Checked == false && chk_self_Decl.Checked==false)))
            {
                _errorProvider.SetError(lblIncomeVerified, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblIncomeVerified.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(lblIncomeVerified, null);
            }

            if (lblFedOmbReq.Visible && String.IsNullOrEmpty(txtFedOmb.Text))
            {
                _errorProvider.SetError(btn1Omb, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFedOmb.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(btn1Omb, null);
            }

            if (lblIncomeReq.Visible && String.IsNullOrEmpty(txtIncome.Text))
            {
                _errorProvider.SetError(txtIncome, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblIncome.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtIncome, null);
            }

            if (lblInProgramReq.Visible && String.IsNullOrEmpty(txtInprogram.Text))
            {
                _errorProvider.SetError(txtInprogram, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblInprogres.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtInprogram, null);
            }

            if (lblMealReq.Visible && ((Captain.Common.Utilities.ListItem)cmbMeal.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbMeal, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMeal.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbMeal, null);
            }

            if (lblCategoricalReq.Visible && ((Captain.Common.Utilities.ListItem)cmbCategorical.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbCategorical, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCategorical.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbCategorical, null);
            }

            return (isValid);
        }

        private bool ValidateCalculation()
        {
            bool isValid = true;


            if (lblVerificationDateReq.Visible && calVerificationDate.Checked == false)
            {
                _errorProvider.SetError(calVerificationDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblVerfificationDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                if (strMode != "Edit")
                {
                    string selectedDate = calVerificationDate.Text;
                    bool flag = false;
                    foreach (DataGridViewRow row in dataGridCaseIncomeVer.Rows)
                    {
                        if (row.Tag is CaseVerEntity)
                        {

                            string strVerifyDate = LookupDataAccess.Getdate((row.Tag as CaseVerEntity).VerifyDate).ToString();
                            if (Convert.ToDateTime(strVerifyDate) == calVerificationDate.Value)
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    _errorProvider.SetError(calVerificationDate, null);
                    if (flag)
                    {
                        _errorProvider.SetError(calVerificationDate, "Verification details already exists for this date");
                        isValid = false;
                    }
                    else
                    {
                        if (calVerificationDate.Value > DateTime.Now)
                        {
                            _errorProvider.SetError(calVerificationDate, "Future date not allowed");
                            isValid = false;

                        }
                        else
                        {
                            _errorProvider.SetError(calVerificationDate, null);
                        }
                    }
                }
                else
                {
                    _errorProvider.SetError(calVerificationDate, null);
                }

            }

            if (((Captain.Common.Utilities.ListItem)cmbVerifier.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbVerifier, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblVerifier.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbVerifier, null);
            }

            return (isValid);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            bool boolIncomeCheck = true;
            if (dataGridCaseIncomeVer.Rows.Count == 1)
            {
                List<CaseIncomeEntity> caseIncomeList = _model.CaseMstData.GetCaseIncomeadpynf(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);
                if (caseIncomeList.Count > 0)
                    boolIncomeCheck = true;
                else
                    boolIncomeCheck = false;
            }
            if (boolIncomeCheck)
            {

                if (ValidateForm())
                {
                    string strSqlMsg = string.Empty;
                    CaseVerEntity caseVerEntity = new CaseVerEntity();
                    caseVerEntity.Agency = MainMenuAgency;
                    caseVerEntity.Dept = MainMenuDept;
                    caseVerEntity.Program = MainMenuProgram;
                    caseVerEntity.Year = MainMenuYear;
                    caseVerEntity.AppNo = ApplicationNo;
                    caseVerEntity.VerifyDate = calVerificationDate.Value.ToString();
                    //if (!((ListItem)cmbVerifier.SelectedItem).Value.ToString().Equals("0"))
                    //{
                    //    caseVerEntity.Verifier = ((ListItem)cmbVerifier.SelectedItem).Value.ToString();
                    //    if (strMode == Consts.Common.New)
                    //    {
                    //        if (CalculateStatus)
                    caseVerEntity.Classification = GetEligibilityStatus();
                    //        else
                    //            caseVerEntity.Classification = "00";
                    //    }

                    //}
                    //else
                    //{
                    //    caseVerEntity.Classification = "00";
                    //}

                    if (!((Captain.Common.Utilities.ListItem)cmbFundingsource.SelectedItem).Value.ToString().Equals("0"))
                    {
                        caseVerEntity.FundSource = ((Captain.Common.Utilities.ListItem)cmbFundingsource.SelectedItem).Value.ToString();
                    }

                    if (!((Captain.Common.Utilities.ListItem)cmbMeal.SelectedItem).Value.ToString().Equals("0"))
                    {
                        caseVerEntity.MealElig = ((Captain.Common.Utilities.ListItem)cmbMeal.SelectedItem).Value.ToString();
                    }
                    if (cmbVerifier.Items.Count > 0)
                        caseVerEntity.Verifier = ((Captain.Common.Utilities.ListItem)cmbVerifier.SelectedItem).Value.ToString();
                    caseVerEntity.VerOmb = txtFedOmb.Text;
                    caseVerEntity.VerHud = txtHud.Text;
                    caseVerEntity.VerSmi = txtSmi.Text;
                    caseVerEntity.VerCmi = txtCmi.Text;
                    caseVerEntity.IncomeAmount = txtIncome.Text;
                    caseVerEntity.NoInhh = txtInprogram.Text;
                    if (!((Captain.Common.Utilities.ListItem)cmbCategorical.SelectedItem).Value.ToString().Equals("0"))
                    {
                        caseVerEntity.CatElig = ((Captain.Common.Utilities.ListItem)cmbCategorical.SelectedItem).Value.ToString();

                    }
                    if (chkw2.Checked == true)
                        caseVerEntity.VerifyW2 = "Y";
                    if (chkCheckStubs.Checked == true)
                        caseVerEntity.VerifyCheckStub = "Y";
                    if (chkLetter.Checked == true)
                        caseVerEntity.VerifyLetter = "Y";
                    if (chkOther.Checked == true)
                        caseVerEntity.VerifyOther = "Y";
                    if (chkTaxReturn.Checked == true)
                        caseVerEntity.VerifyTaxReturn = "Y";
                    if (chk_self_Decl.Checked == true)
                        caseVerEntity.VerifySelfDecl = "Y";

                    if (calReverficationdate.Checked == true)
                        caseVerEntity.ReverifyDate = calReverficationdate.Value.ToString();
                    caseVerEntity.AddOperator = BaseForm.UserID;
                    caseVerEntity.LstcOperator = BaseForm.UserID;

                    caseVerEntity.Mode = strMode;
                    //List<CaseELIGEntity> caseEligQuestions = caseEligEntityAll.FindAll(u => ((u.EligAgency + u.EligDept + u.EligProgram).Equals(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg)));
                    //if (caseEligQuestions.Count > 0)
                    caseVerEntity.MstModify = CalculateStatus.ToString();
                    //txtSub.Text;                
                    if (_model.CaseMstData.InsertUpdateDelCaseVer(caseVerEntity, out strSqlMsg))
                    {
                        BaseForm.BaseCaseMstListEntity = _model.CaseMstData.GetCaseMstadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
                        //if (!((ListItem)cmbVerifier.SelectedItem).Value.ToString().Equals("0"))
                        //{
                        //    if (CalculateStatus)
                        //    {
                        //        if (GetEligibilityStatus() != "00")
                        //        {
                        //            caseVerEntity.Verifier = ((ListItem)cmbVerifier.SelectedItem).Value.ToString();
                        //            caseVerEntity.Classification = GetEligibilityStatus();
                        //            caseVerEntity.Mode = "Classfication";
                        //            _model.CaseMstData.InsertUpdateDelCaseVer(caseVerEntity, out strSqlMsg);
                        //        }
                        //    }
                        //}

                        if (strMode == Consts.Common.New)
                        {
                            CalculateRankPoints();
                        }
                        //if (BaseForm.BaseAgencyControlDetails.RomaSwitch.ToUpper() == "Y")
                        //{
                        //    InsertUpdateRngCounts();
                        //}
                        fillGridData();
                        if (dataGridCaseIncomeVer.Rows.Count != 0)
                        {
                            dataGridCaseIncomeVer.Rows[strIndex].Selected = true;
                            btnCancel.Text = "Close";
                            dataGridCaseIncomeVer.Enabled = true;
                            ButtonsPrevilegs();
                            dataGridCaseIncomeVer_SelectionChanged(sender, e);

                            //DisableControls();
                            //  EnableDisableControls();
                            calVerificationDate.Enabled = false;
                        }
                        //ClientIntakeControl clientIntakeControl = BaseForm.GetBaseUserControl() as ClientIntakeControl;
                        Case3001Control clientIntakeControl = BaseForm.GetBaseUserControl() as Case3001Control;
                        if (clientIntakeControl != null)
                        {
                            clientIntakeControl.RefreshAlertCode();
                        }
                        btnSave.Visible = false;
                        btnCancel.Text = "Close";


                    }
                    else
                    {
                        if (strSqlMsg == Consts.Common.Exists)
                        {
                            CommonFunctions.MessageBoxDisplay(Consts.LiheAllDetails.Datealreadyexist);
                        }
                    }
                    CalculateStatus = false;
                }
            }
            else
            {
                CommonFunctions.MessageBoxDisplay("No Income Detail Records found for this Applicant");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text == "Close")
            {
                this.Close();
            }
            else
            {
                btnCancel.Text = "Close";
                dataGridCaseIncomeVer.Enabled = true;
                btnSave.Visible = false;
                ButtonsPrevilegs();
                dataGridCaseIncomeVer_SelectionChanged(sender, e);
            }
        }

        private void dataGridCaseIncomeVer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridCaseIncomeVer.ColumnCount - 1 && e.RowIndex != -1)
            {
                if (dataGridCaseIncomeVer.SelectedRows.Count > 0)
                {
                    if (dataGridCaseIncomeVer.SelectedRows[0].Tag is CaseVerEntity)
                    {
                        CaseVerEntity row = dataGridCaseIncomeVer.SelectedRows[0].Tag as CaseVerEntity;
                        if (row != null)
                        {
                            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose:OnDeleteMessageBoxClicked); ;

                        }
                    }
                }
            }
        }

        private void OnDeleteMessageBoxClicked(DialogResult dialogResult)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;

            if (dialogResult == DialogResult.Yes)
            {
                string strSqlMsg = string.Empty;
                CaseVerEntity caseVerEntity = new CaseVerEntity();
                caseVerEntity.Agency = MainMenuAgency;
                caseVerEntity.Dept = MainMenuDept;
                caseVerEntity.Program = MainMenuProgram;
                caseVerEntity.Year = MainMenuYear;
                caseVerEntity.AppNo = ApplicationNo;
                caseVerEntity.VerifyDate = calVerificationDate.Value.ToString();
                // caseVerEntity.Verifier = Convert.ToString(dataGridCaseIncomeVer.SelectedRows[0].Cells["VerifyDate"].Value);
                caseVerEntity.Mode = Consts.Common.Delete;
                caseVerEntity.LstcOperator = BaseForm.UserID;
                if (_model.CaseMstData.InsertUpdateDelCaseVer(caseVerEntity, out strSqlMsg))
                {
                    fillGridData();
                    if (dataGridCaseIncomeVer.Rows.Count != 0)
                    {
                        if (dataGridCaseIncomeVer.Rows.Count > 1)
                        {
                            dataGridCaseIncomeVer.Rows[strIndex].Selected = true;
                        }
                        else
                        {
                            dataGridCaseIncomeVer.Rows[0].Selected = true;
                        }
                    }
                    ButtonsPrevilegs();
                    OnDataGridCaseIncomeVerSelectionChanged();


                    //  DisableControls();
                    // EnableDisableControls();
                    // ClientIntakeControl clientIntakeControl = BaseForm.GetBaseUserControl() as ClientIntakeControl;
                    Case3001Control clientIntakeControl = BaseForm.GetBaseUserControl() as Case3001Control;
                    if (clientIntakeControl != null)
                    {
                        clientIntakeControl.RefreshAlertCode();
                    }
                }
            }
        }

        private void dataGridCaseIncomeVer_SelectionChanged(object sender, EventArgs e)
        {
            OnDataGridCaseIncomeVerSelectionChanged(); //Alaa: This one was changed for better compatibility with Wisej, specifically for the OnDeleteMessageBoxClicked Method, line 5149
        }
        
        private void OnDataGridCaseIncomeVerSelectionChanged()
        {
            _errorProvider.SetError(calVerificationDate, null);
            _errorProvider.SetError(calReverficationdate, null);
            _errorProvider.SetError(cmbVerifier, null);
            _errorProvider.SetError(cmbFundingsource, null);
            _errorProvider.SetError(cmbMeal, null);
            _errorProvider.SetError(cmbCategorical, null);
            _errorProvider.SetError(lblIncomeVerified, null);
            _errorProvider.SetError(btn1Omb, null);
            _errorProvider.SetError(txtIncome, null);
            _errorProvider.SetError(txtInprogram, null);
            lblCertified.Text = string.Empty;
            lblStatus.Visible = false;
            btnCalculateEligibility.Enabled = true;
            ////added by Sudheer on 12/02/2022 as per WiseJ 2001 Feedback_Dec2022.docx
            //btnCalculateEligibility.Visible = true;
            //btnCalculateEligibility.Visible = true;


            CalculateStatus = false;
            calVerificationDate.ValueChanged -= new EventHandler(calVerificationDate_ValueChanged);
            txtInprogram.TextChanged -= new EventHandler(txtInprogram_TextChanged);

            calVerificationDate.CheckedChanged -= new EventHandler(calVerificationDate_CheckedChanged);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            calReverficationdate.Value = DateTime.Now.Date;
            calReverficationdate.Checked = false;
            calVerificationDate.Value = DateTime.Now.Date;
            calVerificationDate.Checked = false;
            _errorProvider.Icon = null;
            cmbVerifier.SelectedIndexChanged -= new EventHandler(cmbVerifier_SelectedIndexChanged);
            if (dataGridCaseIncomeVer.Rows.Count > 0)
            {
                strMode = string.Empty;
                if (dataGridCaseIncomeVer.SelectedRows.Count > 0)
                {
                    if (dataGridCaseIncomeVer.SelectedRows[0].Tag is CaseVerEntity)
                    {
                        CaseVerEntity row = dataGridCaseIncomeVer.SelectedRows[0].Tag as CaseVerEntity;
                        if (row != null)
                        {

                            CaseVerEntity caseVerList = _model.CaseMstData.GetCaseveradpynd(MainMenuAgency, MainMenuDept, MainMenuProgram, MainMenuYear, ApplicationNo, row.VerifyDate, string.Empty);
                            if (caseVerList != null)
                            {
                                calVerificationDate.Value = Convert.ToDateTime(caseVerList.VerifyDate);
                                calVerificationDate.Enabled = false;
                                calVerificationDate.Checked = true;
                                if (caseVerList.VerifyW2 == Consts.YesNoVariants.Y)
                                    chkw2.Checked = true;
                                else
                                    chkw2.Checked = false;

                                if (caseVerList.VerifyTaxReturn == Consts.YesNoVariants.Y)
                                    chkTaxReturn.Checked = true;
                                else
                                    chkTaxReturn.Checked = false;

                                if (caseVerList.VerifyLetter == Consts.YesNoVariants.Y)
                                    chkLetter.Checked = true;
                                else
                                    chkLetter.Checked = false;

                                if (caseVerList.VerifyOther == Consts.YesNoVariants.Y)
                                    chkOther.Checked = true;
                                else
                                    chkOther.Checked = false;

                                if (caseVerList.VerifyCheckStub == Consts.YesNoVariants.Y)
                                    chkCheckStubs.Checked = true;
                                else
                                    chkCheckStubs.Checked = false;

                                if (caseVerList.VerifySelfDecl == Consts.YesNoVariants.Y)
                                    chk_self_Decl.Checked = true;
                                else
                                    chk_self_Decl.Checked = false;


                                txtCmi.Text = caseVerList.VerCmi;

                                if (BaseForm.BaseAgencyControlDetails.ACR_POV_WITH_DEC == "Y")
                                    txtFedOmb.Text = caseVerList.VerOmb;
                                else
                                    txtFedOmb.Text = Math.Abs((int)decimal.Parse(caseVerList.VerOmb)).ToString();

                                //txtFedOmb.Text = caseVerList.VerOmb;
                                txtHud.Text = caseVerList.VerHud;
                                txtIncome.Text = caseVerList.IncomeAmount;
                                txtSmi.Text = caseVerList.VerSmi;
                                txtInprogram.Text = caseVerList.NoInhh;
                                if (caseVerList.NoInhh != string.Empty)
                                    intTotalinProgress = Convert.ToInt32(caseVerList.NoInhh);

                                if (caseVerList.ReverifyDate != "")
                                {
                                    calReverficationdate.Value = Convert.ToDateTime(caseVerList.ReverifyDate);
                                    calReverficationdate.Checked = true;
                                }
                                if (caseVerList.FundSource != string.Empty)
                                    CommonFunctions.SetComboBoxValue(cmbFundingsource, caseVerList.FundSource);
                                else
                                    CommonFunctions.SetComboBoxValue(cmbFundingsource, "0");
                                CommonFunctions.SetComboBoxValue(cmbCategorical, caseVerList.CatElig);
                                if (caseVerList.MealElig != string.Empty)
                                    CommonFunctions.SetComboBoxValue(cmbMeal, caseVerList.MealElig);
                                else
                                    CommonFunctions.SetComboBoxValue(cmbMeal, "0");
                                strCaseWorkerDefaultCode = caseVerList.Verifier;
                                if (strCaseWorkerDefaultCode != string.Empty || strCaseWorkerDefaultCode != "0")
                                    CommonFunctions.SetComboBoxValue(cmbVerifier, strCaseWorkerDefaultCode);
                                else
                                    CommonFunctions.SetComboBoxValue(cmbVerifier, strCaseWorkerDefaultStartCode);
                                ChangeStatus(caseVerList.Classification.ToString().Trim(), false);
                                strMode = Consts.Common.Edit;
                                strIndex = dataGridCaseIncomeVer.SelectedRows[0].Index;
                                //if (privilege.ChangePriv.Equals("false"))
                                //{
                                EnableAllcontrolsPrivileges(false);
                                //    btnSave.Visible = false;
                                //    btnCancel.Text = "Close";

                                //}
                                //else
                                //{
                                //    EnableAllcontrolsPrivileges(true);
                                //    DisableControls();
                                //    EnableDisableControls();
                                //    Programdefinationcheck();
                                //    btnSave.Visible = true;
                                //    btnCancel.Text = "Cancel";
                                //    calVerificationDate.Enabled = false;
                                //}

                                DateTime dtverifyDate = Convert.ToDateTime(caseVerList.VerifyDate);

                                if (dtverifyDate.Date == Convert.ToDateTime(VerifyCheckDate).Date)
                                {
                                    if (privilege.ChangePriv.Equals("false"))
                                    {
                                        PbEdit.Visible = false;
                                    }
                                    else
                                    {
                                        PbEdit.Visible = true;
                                    }
                                    //txtInprogram.Enabled = true;   // commented by sudheer on 11/28/2022 as per the document 'WiseJ 2001 Thoughts.docx' on page 6.
                                }
                                else
                                {
                                    PbEdit.Visible = false;
                                    txtInprogram.Enabled = false;
                                }

                            }
                            cmbVerifier.Focus();
                        }

                    }
                    else
                    {
                        EnableAllcontrolsPrivileges(false);
                        //fillIncomeControl();
                        //ChangeStatus("NEW");
                        //strMode = Consts.Common.New;
                        //if (privilege.AddPriv.Equals("false"))
                        //{
                        //    EnableAllcontrolsPrivileges(false);
                        //    btnSave.Visible = false;
                        //    btnCancel.Text = "Close";
                        //}
                        //else
                        //{
                        //    EnableAllcontrolsPrivileges(true);
                        //    DisableControls();
                        //    EnableDisableControls();
                        //    Programdefinationcheck();
                        //    btnSave.Visible = true;
                        //    btnCancel.Text = "Cancel";
                        //}
                        //calVerificationDate.Focus();
                    }
                }
                else
                {
                    EnableAllcontrolsPrivileges(false);
                    //fillIncomeControl();
                    //ChangeStatus("NEW");
                    //strMode = Consts.Common.New;
                    //if (privilege.AddPriv.Equals("false"))
                    //{
                    //    EnableAllcontrolsPrivileges(false);
                    //    btnSave.Visible = false;
                    //    btnCancel.Text = "Close";
                    //}
                    //else
                    //{
                    //    EnableAllcontrolsPrivileges(true);
                    //    DisableControls();
                    //    EnableDisableControls();
                    //    Programdefinationcheck();
                    //    btnSave.Visible = true;
                    //    btnCancel.Text = "Cancel";
                    //}
                    //calVerificationDate.Focus();
                }
            }
            else
            {
                EnableAllcontrolsPrivileges(false);
                fillIncomeControl();
                ChangeStatus("NEW", false);
                //strMode = Consts.Common.New;
                //if (privilege.AddPriv.Equals("false"))
                //{
                //    EnableAllcontrolsPrivileges(false);
                //    btnSave.Visible = false;
                //    btnCancel.Text = "Close";
                //}
                //else
                //{
                //    EnableAllcontrolsPrivileges(true);
                //    DisableControls();
                //    EnableDisableControls();
                //    Programdefinationcheck();
                //    btnSave.Visible = true;
                //    btnCancel.Text = "Cancel";
                //}
                //calVerificationDate.Focus();
            }
            if (!FLDCNTLHieEntity.Exists(u => u.Enab.Equals("Y")))
            {
                lblFedOmbReq.Visible = false;
                btn1Omb.Enabled = false;
                txtFedOmb.Enabled = false;
                btnSave.Enabled = false;
                btn2Cmi.Enabled = false;
                txtCmi.Enabled = false;
                btn3Smi.Enabled = false;
                txtSmi.Enabled = false;
                btn4Hud.Enabled = false;
                txtHud.Enabled = false;
            }
            cmbVerifier.SelectedIndexChanged += new EventHandler(cmbVerifier_SelectedIndexChanged);
            calVerificationDate.ValueChanged += new EventHandler(calVerificationDate_ValueChanged);
            calVerificationDate.CheckedChanged += new EventHandler(calVerificationDate_CheckedChanged);
            txtInprogram.TextChanged += new EventHandler(txtInprogram_TextChanged);
        }

        private void CaseIncomeVerification_Load(object sender, EventArgs e)
        {
            dataGridCaseIncomeVer.SelectionChanged += new EventHandler(dataGridCaseIncomeVer_SelectionChanged);
            dataGridCaseIncomeVer_SelectionChanged(sender, e);
            DisableControls();
            EnableDisableControls();
            if (!FLDCNTLHieEntity.Exists(u => u.Enab.Equals("Y")))
            {
                CommonFunctions.MessageBoxDisplay("Field controls not defined for this program");
                dataGridCaseIncomeVer.CellClick -= new DataGridViewCellEventHandler(dataGridCaseIncomeVer_CellClick);
                // dataGridCaseIncomeVer.Enabled = false;
                lblFedOmbReq.Visible = false;
                btn1Omb.Enabled = false;
                txtFedOmb.Enabled = false;
                btnSave.Enabled = false;
                btn2Cmi.Enabled = false;
                txtCmi.Enabled = false;
                btn3Smi.Enabled = false;
                txtSmi.Enabled = false;
                btn4Hud.Enabled = false;
                txtHud.Enabled = false;
                btnCalculateEligibility.Enabled = false;
            }
            else
            {
                Programdefinationcheck();
                btnCalculateEligibility.Enabled = true;
                if (!privilege.AddPriv.Equals("false"))
                {
                    if (dataGridCaseIncomeVer.Rows.Count > 0)
                    {
                        // int introw = dataGridCaseIncomeVer.Rows.Count - 1;
                        dataGridCaseIncomeVer.Rows[0].Selected = true;
                    }
                }

                dataGridCaseIncomeVer_SelectionChanged(sender, e);
            }

            /*Commented by Vikash on 02/08/2023 as per request in NCCAA todo document Point - Income Verification Missing # in Program 2/6/2023
              if (programDefination.State.ToUpper() == "TX")
             {
                 btnVerEligiblity.Visible = false;
                 PbIncome.Visible = false;
                 cmbCategorical.Visible = false;
                 lblCategorical.Visible = false;
                 lblCategoricalReq.Visible = false;
                 cmbMeal.Visible = false;
                 lblMeal.Visible = false;
                 lblMealReq.Visible = false;
                 calReverficationdate.Visible = false;
                 lblReverificationDate.Visible = false;
                 lblReverificationReq.Visible = false;
                 txtInprogram.Visible = false;
                 lblInProgramReq.Visible = false;
                 lblInprogres.Visible = false;
             }*/

        }
        public bool CalculateStatus = false;
        private void btnCalculateEligibility_Click(object sender, EventArgs e)
        {
            if (ValidateCalculation())
            {
                if (caseSnpList.Count > 0)
                {
                    CalculateStatus = true;
                    List<CaseSnpEntity> listCasesnp = caseSnpList.FindAll(u => u.ProgIncome != string.Empty);
                    decimal snppronginccome = listCasesnp.Sum(u => Convert.ToDecimal(u.ProgIncome));
                    txtIncome.Text = snppronginccome.ToString();//CaseMSTEntity.ProgIncome;
                    txtInprogram.Text = caseSnpList.Count(u => u.Exclude == "N" && u.Status.Trim().ToUpper() != "I" && u.SsnReason != "Q").ToString();
                    CalculateFedOmbEligData();
                }
            }

        }

        private void btn1Omb_Click(object sender, EventArgs e)
        {
            ShowFedDetails("FED", "Federal OMB Chart");
        }

        private void btn2Cmi_Click(object sender, EventArgs e)
        {
            ShowFedDetails("CMI", "CMI Income Chart");
        }

        private void ShowFedDetails(string strType, string strHeader)
        {
            if (calVerificationDate.Checked == true)
            {
                if (calVerificationDate.Value > System.DateTime.Now)
                {
                    CommonFunctions.MessageBoxDisplay("Verification Date should not be in future");
                }
                else
                {
                    List<MasterPovertyEntity> masterPovertyDetails = _model.masterPovertyData.GetFedralOmbChart(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, strType, calVerificationDate.Value.ToShortDateString());
                    if (strType == "CMI" || strType == "HUD")
                    {
                        string county = string.Empty;
                        if (CaseMSTEntity != null)
                        {
                            county = CaseMSTEntity.County;
                        }
                        masterPovertyDetails = masterPovertyDetails.FindAll(u => u.GdlCounty.Equals(county)).ToList();
                    }

                    if (masterPovertyDetails.Count > 0)
                    {
                        FederalOmbChart fedOmbChart = new FederalOmbChart(BaseForm, privilege, masterPovertyDetails, strType, strHeader);
                        fedOmbChart.StartPosition = FormStartPosition.CenterScreen;
                        fedOmbChart.ShowDialog();
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay("No template found for enter date");
                    }

                }
            }
            else
            {
                CommonFunctions.MessageBoxDisplay("Please select verification date");
            }
        }

        private void btn3Smi_Click(object sender, EventArgs e)
        {
            ShowFedDetails("SMI", "State Median Income Chart");
        }

        private void btn4Hud_Click(object sender, EventArgs e)
        {
            ShowFedDetails("HUD", "Hud Chart");
        }

        private void EnableDisableControls()
        {

            foreach (FldcntlHieEntity entity in FLDCNTLHieEntity)
            {
                bool required = entity.Req.Equals("Y") ? true : false;
                bool enabled = entity.Enab.Equals("Y") ? true : false;

                switch (entity.FldCode)
                {
                    case Consts.CASE2003.Verifier:
                        if (enabled) { cmbVerifier.Enabled = lblVerifier.Enabled = true; if (required) lblVerifierReq.Visible = true; } else { cmbVerifier.Enabled = lblVerifier.Enabled = false; lblVerifierReq.Visible = false; }
                        break;
                    case Consts.CASE2003.FundingSource:
                        if (enabled) { cmbFundingsource.Enabled = lblFundingSource.Enabled = true; if (required) lblFundingSourceReq.Visible = true; } else { cmbFundingsource.Enabled = lblFundingSource.Enabled = false; lblFundingSourceReq.Visible = false; }
                        break;
                    case Consts.CASE2003.IncomeVerified:
                        if (enabled) { chkCheckStubs.Enabled = chkLetter.Enabled = chkOther.Enabled = chk_self_Decl.Enabled = chkTaxReturn.Enabled = chkw2.Enabled = lblIncomeVerified.Enabled = true; if (required) lblIncomeVerifiedReq.Visible = true; } else { chkCheckStubs.Enabled = chkLetter.Enabled = chkOther.Enabled = chk_self_Decl.Enabled = chkTaxReturn.Enabled = chkw2.Enabled = lblIncomeVerified.Enabled = false; lblIncomeVerifiedReq.Visible = false; }
                        break;
                    case Consts.CASE2003.inProgram:
                        if (enabled) { txtInprogram.Enabled = lblInprogres.Enabled = true; if (required) lblInProgramReq.Visible = true; } else { txtInprogram.Enabled = lblInprogres.Enabled = false; lblInProgramReq.Visible = false; }
                        break;
                    case Consts.CASE2003.Income:
                        if (enabled) { lblIncome.Enabled = true; if (required) lblIncomeReq.Visible = true; } else { lblIncome.Enabled = false; lblIncomeReq.Visible = false; }
                        break;
                    case Consts.CASE2003.Meal:
                        if (enabled) { cmbMeal.Enabled = lblMeal.Enabled = true; if (required) lblMealReq.Visible = true; } else { cmbMeal.Enabled = lblMeal.Enabled = false; lblMealReq.Visible = false; }
                        break;
                    case Consts.CASE2003.Categorical:
                        if (enabled) { cmbCategorical.Enabled = lblCategorical.Enabled = true; if (required) lblCategoricalReq.Visible = true; } else { cmbCategorical.Enabled = lblCategorical.Enabled = false; lblCategoricalReq.Visible = false; }
                        break;
                    case Consts.CASE2003.ReverificationDate:
                        if (enabled) { calReverficationdate.Enabled = lblReverificationDate.Enabled = true; if (required) lblReverificationReq.Visible = true; } else { calReverficationdate.Enabled = lblReverificationDate.Enabled = false; lblReverificationReq.Visible = false; }
                        break;
                    case Consts.CASE2003.VerificationDate:
                        if (enabled) { calVerificationDate.Enabled = lblVerfificationDate.Enabled = true; if (required) lblVerificationDateReq.Visible = true; } else { calVerificationDate.Enabled = lblVerfificationDate.Enabled = false; lblVerificationDateReq.Visible = false; }
                        break;
                }

            }
            /* Commented by Vikash on 02/08/2023 as per request in NCCAA todo document Point - Income Verification Missing # in Program 2/6/2023
             if (programDefination.State.ToUpper() == "TX")
             {
                 btnVerEligiblity.Visible = false;
                 PbIncome.Visible = false;
                 cmbCategorical.Visible = false;
                 lblCategorical.Visible = false;
                 lblCategoricalReq.Visible = false;
                 cmbMeal.Visible = false;
                 lblMeal.Visible = false;
                 lblMealReq.Visible = false;
                 calReverficationdate.Visible = false;
                 lblReverificationDate.Visible = false;
                 lblReverificationReq.Visible = false;
                 txtInprogram.Visible = false;
                 lblInProgramReq.Visible = false;
                 lblInprogres.Visible = false;
             }*/


        }

        private void calVerificationDate_TextChanged(object sender, EventArgs e)
        {
            //if (calVerificationDate.Checked == true)
            //{
            //    if (strMode != "Edit")
            //    {
            //        foreach (DataGridViewRow row in dataGridCaseIncomeVer.Rows)
            //        {
            //            if (row.Tag is CaseVerEntity)
            //            {

            //                string strVerifyDate = LookupDataAccess.Getdate((row.Tag as CaseVerEntity).VerifyDate).ToString();
            //                if (Convert.ToDateTime(strVerifyDate) == calVerificationDate.Value)
            //                {
            //                    MessageBox.Show("Verification details already exists for this date");
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}
        }

        private void calc_FEDOMB()
        {
            double povertyPercent = 0;
            string type = "FED";
            if (ProgramDefination == null)
            {
                txtFedOmb.Text = povertyPercent.ToString();
                return;
            }
            if (ProgramDefination.DepFEDUsed.Equals("Y"))
            {
                double povertyBase = 0;
                double povertyFactory = 0;
                double povertyA = 0;
                List<MasterPovertyEntity> masterPovertyDetails = _model.masterPovertyData.GetFedralOmbChart(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, type, calVerificationDate.Value.ToShortDateString());
                if (masterPovertyDetails.Count > 0)
                {
                    povertyBase = double.Parse(masterPovertyDetails[0].Gdl1Value.ToString()) - double.Parse(masterPovertyDetails[0].Gdl2Value.ToString());
                    povertyFactory = double.Parse(masterPovertyDetails[0].Gdl2Value);
                    double noOfHouseHolds = txtInprogram.Text.ToString().Equals(string.Empty) ? 0.0 : double.Parse(txtInprogram.Text);
                    PovertyException propIncrementdata = _model.masterPovertyData.GetPovertyException(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, masterPovertyDetails[0].GdlStartDate, masterPovertyDetails[0].GdlEndDate, calVerificationDate.Value.ToShortDateString(), "OMB");
                    int inthouseholds = txtInprogram.Text.ToString().Equals(string.Empty) ? 0 : int.Parse(txtInprogram.Text);
                    if (propIncrementdata != null)
                    {
                        povertyBase = double.Parse(masterPovertyDetails[0].Gdl1Value.ToString());
                        switch (inthouseholds)
                        {
                            case 1:
                                povertyA = 0;
                                break;
                            case 2:
                                povertyA = Convert.ToDouble(propIncrementdata.Exp2Value);
                                break;
                            case 3:
                                povertyA = Convert.ToDouble(propIncrementdata.Exp2Value) + Convert.ToDouble(propIncrementdata.Exp3Value);
                                break;
                            case 4:
                                povertyA = Convert.ToDouble(propIncrementdata.Exp2Value) + Convert.ToDouble(propIncrementdata.Exp3Value) + Convert.ToDouble(propIncrementdata.Exp4Value);
                                break;
                            case 5:
                                povertyA = Convert.ToDouble(propIncrementdata.Exp2Value) + Convert.ToDouble(propIncrementdata.Exp3Value) + Convert.ToDouble(propIncrementdata.Exp4Value) + Convert.ToDouble(propIncrementdata.Exp5Value);
                                break;
                            case 6:
                                povertyA = Convert.ToDouble(propIncrementdata.Exp2Value) + Convert.ToDouble(propIncrementdata.Exp3Value) + Convert.ToDouble(propIncrementdata.Exp4Value) + Convert.ToDouble(propIncrementdata.Exp5Value) + Convert.ToDouble(propIncrementdata.Exp6Value);
                                break;
                            case 7:
                                povertyA = Convert.ToDouble(propIncrementdata.Exp2Value) + Convert.ToDouble(propIncrementdata.Exp3Value) + Convert.ToDouble(propIncrementdata.Exp4Value) + Convert.ToDouble(propIncrementdata.Exp5Value) + Convert.ToDouble(propIncrementdata.Exp6Value) + Convert.ToDouble(propIncrementdata.Exp7Value);
                                break;
                            case 8:
                                povertyA = Convert.ToDouble(propIncrementdata.Exp2Value) + Convert.ToDouble(propIncrementdata.Exp3Value) + Convert.ToDouble(propIncrementdata.Exp4Value) + Convert.ToDouble(propIncrementdata.Exp5Value) + Convert.ToDouble(propIncrementdata.Exp6Value) + Convert.ToDouble(propIncrementdata.Exp7Value) + Convert.ToDouble(propIncrementdata.Exp8Value);
                                break;
                            default:
                                if (inthouseholds > 8)
                                {
                                    povertyA = (Convert.ToDouble(propIncrementdata.Exp2Value) + Convert.ToDouble(propIncrementdata.Exp3Value) + Convert.ToDouble(propIncrementdata.Exp4Value) + Convert.ToDouble(propIncrementdata.Exp5Value) + Convert.ToDouble(propIncrementdata.Exp6Value) + Convert.ToDouble(propIncrementdata.Exp7Value) + Convert.ToDouble(propIncrementdata.Exp8Value)) + ((inthouseholds - 8) * Convert.ToDouble(propIncrementdata.Exp8Value));
                                }
                                break;
                        }

                    }
                    else
                    {
                        povertyA = noOfHouseHolds * povertyFactory;
                    }
                    double povertyB = povertyBase + povertyA;
                    double povertyProgramIncome = txtIncome.Text.ToString().Equals(string.Empty) ? 0.0 : double.Parse(txtIncome.Text);
                    povertyPercent = (povertyProgramIncome / povertyB) * 100;
                    ////povertyPercent = Math.Round(povertyPercent);

                    /// newly added start this logic 05/10/2016
                    string Str_povertyD = povertyPercent.ToString();
                    if (BaseForm.BaseAgencyControlDetails.ACR_POV_DONT_RNDUP == "N" && BaseForm.BaseAgencyControlDetails.ACR_POV_WITH_DEC == "N")
                    {
                        string[] parts;
                        if (Str_povertyD.Contains("."))
                        {
                            parts = (povertyPercent.ToString()).Split('.');
                            povertyPercent = double.Parse(parts[0]);

                            if (double.Parse(parts[1]) > 0)
                                povertyPercent = (povertyPercent + 1);
                            else
                                povertyPercent = (povertyPercent + 0.5);
                        }
                        //// end



                        //povertyPercent= Math.Round(povertyPercent, 0);

                        if (povertyPercent < 1)
                            povertyPercent = 1;
                        else if (povertyPercent > 1000)
                            povertyPercent = 999;
                    }
                    else if (BaseForm.BaseAgencyControlDetails.ACR_POV_DONT_RNDUP == "Y" && BaseForm.BaseAgencyControlDetails.ACR_POV_WITH_DEC == "N")
                    {
                        string[] parts;
                        if (Str_povertyD.Contains("."))
                        {
                            parts = (povertyPercent.ToString()).Split('.');
                            povertyPercent = double.Parse(parts[0]);
                        }
                    }

                    //Added by sudheer on 06/05/23 if the income is 0 then the Poverty must be 0
                    if (povertyProgramIncome==0)
                        povertyPercent = 0;

                    if (BaseForm.BaseAgencyControlDetails.ACR_POV_WITH_DEC == "Y")
                        txtFedOmb.Text = povertyPercent.ToString("0.0000");
                    else
                        txtFedOmb.Text = Math.Abs((int)povertyPercent).ToString();
                }
            }
        }

        private void calc_Poverty(string type)
        {
            int masterpovertycount = 0;
            double povertyPercent = 0;
            if (ProgramDefination != null)
            {
                string county = string.Empty;
                if (CaseMSTEntity != null)
                {
                    county = CaseMSTEntity.County;
                    if (strMode != "Edit")
                    {
                        if (CaseMSTEntity.NoInProg != string.Empty)
                            intTotalinProgress = Convert.ToInt32(CaseMSTEntity.NoInProg);

                    }
                }
                List<MasterPovertyEntity> masterPovertyDetails = _model.masterPovertyData.GetFedralOmbChart(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, type, calVerificationDate.Value.ToShortDateString());
                if (!type.Equals("SMI"))
                {
                    masterPovertyDetails = masterPovertyDetails.FindAll(u => u.GdlCounty.Equals(county)).ToList();
                }
                if (masterPovertyDetails.Count > 0)
                {
                    bool GDLSW = false;
                    masterpovertycount = masterPovertyDetails.Count;
                    MasterPovertyEntity masterPovertyDetail = masterPovertyDetails[0];
                    double povertyProgramIncome = txtIncome.Text.ToString().Equals(string.Empty) ? 0.0 : double.Parse(txtIncome.Text);
                    int HUDIDX = 0;
                    double GDL1Value = masterPovertyDetail.Gdl1Value.ToString().Equals(string.Empty) ? 0.0 : double.Parse(masterPovertyDetail.Gdl1Value);
                    double GDL2Value = masterPovertyDetail.Gdl2Value.ToString().Equals(string.Empty) ? 0.0 : double.Parse(masterPovertyDetail.Gdl2Value);
                    double GDL3Value = masterPovertyDetail.Gdl3Value.ToString().Equals(string.Empty) ? 0.0 : double.Parse(masterPovertyDetail.Gdl3Value);
                    double GDL4Value = masterPovertyDetail.Gdl4Value.ToString().Equals(string.Empty) ? 0.0 : double.Parse(masterPovertyDetail.Gdl4Value);
                    double GDL5Value = masterPovertyDetail.Gdl5Value.ToString().Equals(string.Empty) ? 0.0 : double.Parse(masterPovertyDetail.Gdl5Value);
                    double GDL6Value = masterPovertyDetail.Gdl6Value.ToString().Equals(string.Empty) ? 0.0 : double.Parse(masterPovertyDetail.Gdl6Value);
                    MasterPovertyEntity masterPovertyAmoutDetail = null;
                    if ((txtInprogram.Text != "") && (txtInprogram.Text != "0"))
                    {
                        if (masterpovertycount > Convert.ToInt64(txtInprogram.Text))
                        {
                            GDLSW = true;
                        }
                        int inprogress = txtInprogram.Text.ToString().Equals(string.Empty) ? 0 : int.Parse(txtInprogram.Text);
                        masterPovertyAmoutDetail = masterPovertyDetails.Find(u => u.GdlNoHouseHolds.Equals(inprogress.ToString()));
                        if (masterPovertyAmoutDetail != null)
                        {
                            GDLSW = true;
                        }
                        else
                        {
                            GDLSW = false;
                        }
                    }

                    if (GDLSW)
                    {

                        double GDL1AmountValue = masterPovertyAmoutDetail.Gdl1Value.ToString().Equals(string.Empty) ? 0.0 : double.Parse(masterPovertyAmoutDetail.Gdl1Value);
                        double GDL2AmountValue = masterPovertyAmoutDetail.Gdl2Value.ToString().Equals(string.Empty) ? 0.0 : double.Parse(masterPovertyAmoutDetail.Gdl2Value);
                        double GDL3AmountValue = masterPovertyAmoutDetail.Gdl3Value.ToString().Equals(string.Empty) ? 0.0 : double.Parse(masterPovertyAmoutDetail.Gdl3Value);
                        double GDL4AmountValue = masterPovertyAmoutDetail.Gdl4Value.ToString().Equals(string.Empty) ? 0.0 : double.Parse(masterPovertyAmoutDetail.Gdl4Value);
                        double GDL5AmountValue = masterPovertyAmoutDetail.Gdl5Value.ToString().Equals(string.Empty) ? 0.0 : double.Parse(masterPovertyAmoutDetail.Gdl5Value);
                        double GDL6AmountValue = masterPovertyAmoutDetail.Gdl6Value.ToString().Equals(string.Empty) ? 0.0 : double.Parse(masterPovertyAmoutDetail.Gdl6Value);

                        if (povertyProgramIncome <= GDL1AmountValue)
                        {
                            HUDIDX = 1;
                            povertyPercent = GDL1Value;
                        }
                        else if (povertyProgramIncome > GDL1AmountValue && povertyProgramIncome <= GDL2AmountValue)
                        {
                            HUDIDX = 2;
                            povertyPercent = GDL2Value;
                        }
                        else if (povertyProgramIncome > GDL2AmountValue && povertyProgramIncome <= GDL3AmountValue)
                        {
                            HUDIDX = 3;
                            povertyPercent = GDL3Value;
                        }
                        else if (povertyProgramIncome > GDL3AmountValue && povertyProgramIncome <= GDL4AmountValue)
                        {
                            HUDIDX = 4;
                            povertyPercent = GDL4Value;
                        }
                        else if (povertyProgramIncome > GDL4AmountValue && povertyProgramIncome <= GDL5AmountValue)
                        {
                            HUDIDX = 5;
                            povertyPercent = GDL5Value;
                        }
                        else if (povertyProgramIncome > GDL5AmountValue && povertyProgramIncome <= GDL6AmountValue)
                        {
                            HUDIDX = 6;
                            povertyPercent = GDL6Value;
                        }
                        //else if (povertyProgramIncome < GDL6AmountValue)
                        //{
                        //    HUDIDX = 6;
                        //    povertyPercent = GDL6Value;
                        //}
                        if (HUDIDX > 0)
                        {
                            switch (type)
                            {
                                case "HUD":
                                    txtHud.Text = povertyPercent.ToString();
                                    break;
                                case "SMI":
                                    txtSmi.Text = povertyPercent.ToString();
                                    break;
                                case "CMI":
                                    txtCmi.Text = povertyPercent.ToString();
                                    break;
                            }
                        }

                    }
                }
            }
        }

        private void txtInprogram_TextChanged(object sender, EventArgs e)
        {
            txtFedOmb.Text = "";
            txtHud.Text = "";
            txtCmi.Text = "";
            txtSmi.Text = "";
            txtInprogram.Text = txtInprogram.Text.Replace('-', ' ');
            txtInprogram.Text = txtInprogram.Text.Trim();
            //txtInprogram.Text.StartsWith("0");
            if (txtInprogram.Text != "")
            {
                if (calVerificationDate.Checked == true)
                {
                    CalculateFedOmbEligData();
                }
            }
            else
                txtInprogram.Text = "0";
        }

        private void cmbMeal_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbVerifier_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Captain.Common.Utilities.ListItem)cmbVerifier.SelectedItem).Value.ToString() != "0")
                if (((Captain.Common.Utilities.ListItem)cmbVerifier.SelectedItem).ID.ToString() != "N")
                    MessageBox.Show("Verfier Inactive", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        private void EnableAllcontrolsPrivileges(bool status)
        {

            // txtCmi.Enabled = status;           
            // txtHud.Enabled = status;           
            txtInprogram.Enabled = status;
            //  txtSmi.Enabled = status;
            calReverficationdate.Enabled = status;
            calVerificationDate.Enabled = status;
            cmbCategorical.Enabled = status;
            cmbFundingsource.Enabled = status;
            cmbMeal.Enabled = status;
            cmbVerifier.Enabled = status;
            chkCheckStubs.Enabled = status;
            chkLetter.Enabled = status;
            chkOther.Enabled = status;
            chk_self_Decl.Enabled = status;
            chkTaxReturn.Enabled = status;
            chkw2.Enabled = status;
            btnSave.Visible = status;
            btnCancel.Text = "Close";
            btn1Omb.Enabled = status;
            btn2Cmi.Enabled = status;
            btn3Smi.Enabled = status;
            btn4Hud.Enabled = status;

        }

        private void DisableControls()
        {
            lblCategorical.Enabled = false;
            lblCertified.Enabled = true;
            lblCmi.Enabled = true;
            lblFedOmb.Enabled = true; ;
            lblFundingSource.Enabled = false;
            lblIncome.Enabled = false; ;
            lblIncomeVerified.Enabled = false;
            lblInprogres.Enabled = false;
            lblMeal.Enabled = false; ;
            lblReverificationDate.Enabled = false;
            lblSmi.Enabled = true;
            lblStatus.Enabled = true;
            lblVerfificationDate.Enabled = false;
            lblVerifier.Enabled = false;
            //txtCmi.Enabled = false;
            //txtFedOmb.Enabled = false;
            //txtHud.Enabled = false;
            txtIncome.Enabled = false;
            txtInprogram.Enabled = false;
            //txtSmi.Enabled = true;
            cmbCategorical.Enabled = false;
            cmbFundingsource.Enabled = false;
            cmbMeal.Enabled = false;
            cmbVerifier.Enabled = false;
            calReverficationdate.Enabled = false;
            calVerificationDate.Enabled = false;
            chkCheckStubs.Enabled = false;
            chkLetter.Enabled = false;
            chkOther.Enabled = false;
            chk_self_Decl.Enabled = false;
            chkTaxReturn.Enabled = false;
            chkw2.Enabled = false;



        }

        private void Programdefinationcheck()
        {

            if (programDefination != null)
            {
                ProgramDefination = programDefination;
                if (programDefination.DepFEDUsed == "Y")
                {
                    lblFedOmbReq.Visible = true;
                    btn1Omb.Enabled = true;
                    txtFedOmb.Enabled = true;

                }
                if (programDefination.DepCMIUsed == "Y")
                {
                    btn2Cmi.Enabled = true;
                    txtCmi.Enabled = true;
                }
                else
                {
                    btn2Cmi.Enabled = false;
                    txtCmi.Enabled = false;
                }
                if (programDefination.DepSMIUsed == "Y")
                {
                    btn3Smi.Enabled = true;
                    txtSmi.Enabled = true;
                }
                else
                {
                    btn3Smi.Enabled = false;
                    txtSmi.Enabled = false;
                }
                if (programDefination.DepHUDUsed == "Y")
                {
                    btn4Hud.Enabled = true;
                    txtHud.Enabled = true;
                }
                else
                {
                    btn4Hud.Enabled = false;
                    txtHud.Enabled = false;
                }

            }
        }

        private string ChangeStatus(string strEligStatus, bool boolViewStatus)
        {
            string strEligStatusnew = string.Empty;
            //string strQueCount = string.Empty, strErrCode = string.Empty, strErrDesc = string.Empty;
            //_model.CaseSumData.CheckCaseSumProgramDetails(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, out strEligStatus, out strQueCount, out strErrCode, out strErrDesc);

            lblStatus.Visible = true;
            btnCalculateEligibility.Enabled = true;
            if (boolViewStatus)
            {
                if (propProgramheadswitch == "Y")
                {
                    string strcategory = string.Empty;
                    if (strEligStatus != "97" || strEligStatus != "96" || strEligStatus != "95")
                    {
                        if (!((Captain.Common.Utilities.ListItem)cmbCategorical.SelectedItem).Value.ToString().Equals("0"))
                        {

                            if (lookUpCategrcalEligiblity.Count > 0)
                            {
                                CommonEntity commoncategory = lookUpCategrcalEligiblity.Find(u => u.Extension.ToUpper() == "Y" && u.Code == ((Captain.Common.Utilities.ListItem)cmbCategorical.SelectedItem).Value.ToString());
                                if (commoncategory != null)
                                {
                                    strcategory = commoncategory.Code;
                                }
                            }
                            if (strcategory != string.Empty)
                            {
                                if (strcategory.ToUpper() != ((Captain.Common.Utilities.ListItem)cmbCategorical.SelectedItem).Value.ToString().ToUpper())
                                {
                                    strEligStatus = "97";
                                }
                                else
                                {
                                    if (txtFedOmb.Text != string.Empty)
                                    {
                                        if (Convert.ToDecimal(txtFedOmb.Text) >= 101 && Convert.ToDecimal(txtFedOmb.Text) <= 130)
                                        {
                                            strEligStatus = "96";
                                        }
                                        else if (Convert.ToDecimal(txtFedOmb.Text) > 130)
                                        {
                                            strEligStatus = "95";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!((Captain.Common.Utilities.ListItem)cmbCategorical.SelectedItem).Value.ToString().Equals("0"))
                                {
                                    strEligStatus = "97";
                                }
                            }
                        }
                        else
                        {
                            if (txtFedOmb.Text != string.Empty)
                            {
                                if (Convert.ToDecimal(txtFedOmb.Text) >= 101 && Convert.ToDecimal(txtFedOmb.Text) <= 130)
                                {
                                    strEligStatus = "96";
                                }
                                else if (Convert.ToDecimal(txtFedOmb.Text) > 130)
                                {
                                    strEligStatus = "95";
                                }
                            }
                        }
                    }
                }
            }
            if (strEligStatus == "99")
                lblCertified.Text = "Certified";
            else if (strEligStatus == "98")
                lblCertified.Text = "Denied";
            else if (strEligStatus == "97")
                lblCertified.Text = "Categorical";//((Captain.Common.Utilities.ListItem)cmbCategorical.SelectedItem).Text.ToString();
            else if (strEligStatus == "96")
                lblCertified.Text = "101% - 130%";
            else if (strEligStatus == "95")
                lblCertified.Text = "Over Income";
            else
            {
                List<CaseELIGEntity> caseEligQuestions = caseEligEntityAll.FindAll(u => ((u.EligAgency + u.EligDept + u.EligProgram).Equals(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg)));
                if (caseEligQuestions.Count > 0)
                {
                    lblCertified.Text = "In Certify";
                }
                else
                {
                    lblCertified.Text = "";
                    lblStatus.Visible = false;
                    //btnCalculateEligibility.Enabled = false;

                    //Added by Sudheer on 12/02/2022 as per the WiseJ 2001 Feedback_Dec2022.docx
                    btnCalculateEligibility.Visible = false;
                    btnVerEligiblity.Visible = false;

                }
            }
            strEligStatusnew = strEligStatus;
            return strEligStatusnew;

        }

        private void calVerificationDate_ValueChanged(object sender, EventArgs e)
        {
            bool boolchkstatus = true;
            if (calVerificationDate.Checked == true)
            {
                if (calVerificationDate.Value > DateTime.Now)
                {
                    CommonFunctions.MessageBoxDisplay("Future date not allowed");
                    calVerificationDate.ValueChanged -= new EventHandler(calVerificationDate_ValueChanged);
                    calVerificationDate.Text = DateTime.Now.ToShortDateString();
                    calVerificationDate.Checked = false;
                    // calVerificationDate.ValueChanged += new EventHandler(calVerificationDate_ValueChanged);
                    boolchkstatus = false;
                }

                if (boolchkstatus)
                {
                    if (strMode != "Edit")
                    {
                        foreach (DataGridViewRow row in dataGridCaseIncomeVer.Rows)
                        {
                            if (row.Tag is CaseVerEntity)
                            {

                                string strVerifyDate = LookupDataAccess.Getdate((row.Tag as CaseVerEntity).VerifyDate).ToString();
                                if (Convert.ToDateTime(strVerifyDate) == calVerificationDate.Value)
                                {
                                    MessageBox.Show("Verification details already exists for this date");
                                    calVerificationDate.ValueChanged -= new EventHandler(calVerificationDate_ValueChanged);
                                    calVerificationDate.Checked = false;
                                    //  calVerificationDate.ValueChanged += new EventHandler(calVerificationDate_ValueChanged);
                                    break;
                                }
                            }
                        }
                    }
                }
                txtFedOmb.Text = "";
                txtHud.Text = "";
                txtCmi.Text = "";
                txtSmi.Text = "";
                // calVerificationDate.ValueChanged -= new EventHandler(calVerificationDate_ValueChanged);
                if (calVerificationDate.Checked == true)
                {
                    CalculateFedOmbEligData();
                }
                //calVerificationDate.ValueChanged += new EventHandler(calVerificationDate_ValueChanged);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Application.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "CASE2001_IncVerify");
            Application.Navigate(CommonFunctions.CreateZenHelps(privilege.Program, 2, BaseForm.BusinessModuleID.ToString(),"",""), target: "_blank");
        }

        private void calVerificationDate_CheckedChanged(object sender, EventArgs e)
        {
            if (calVerificationDate.Checked == false)
            {
                txtFedOmb.Text = "";
                txtHud.Text = "";
                txtCmi.Text = "";
                txtSmi.Text = "";
            }
            else
            {
                txtFedOmb.Text = "";
                txtHud.Text = "";
                txtCmi.Text = "";
                txtSmi.Text = "";
                // calVerificationDate.ValueChanged -= new EventHandler(calVerificationDate_ValueChanged);
                if (ProgramDefination != null)
                {
                    if (ProgramDefination.DepFEDUsed == "Y")
                    {
                        // txtFedOmb.Text = "0";
                        calc_FEDOMB();
                    }
                    if (ProgramDefination.DepCMIUsed == "Y")
                    {
                        // txtCmi.Text = "0";
                        calc_Poverty("CMI");
                    }
                    if (ProgramDefination.DepSMIUsed == "Y")
                    {
                        // txtSmi.Text = "0";
                        calc_Poverty("SMI");
                    }
                    if (ProgramDefination.DepHUDUsed == "Y")
                    {
                        // txtHud.Text = "0";
                        calc_Poverty("HUD");
                    }
                }
            }
        }

        private void btnRankProcess_Click(object sender, EventArgs e)
        {
            CASB2530View obj = new CASB2530View(BaseForm, privilege, hierarchyEntity);
            obj.StartPosition = FormStartPosition.CenterScreen;
            obj.ShowDialog();
        }


        private void CalculateFedOmbEligData()
        {
            if (ProgramDefination != null)
            {
                if (ProgramDefination.DepFEDUsed == "Y")
                {
                    // txtFedOmb.Text = "0";
                    calc_FEDOMB();
                }
                if (ProgramDefination.DepCMIUsed == "Y")
                {
                    // txtCmi.Text = "0";
                    calc_Poverty("CMI");
                }
                if (ProgramDefination.DepSMIUsed == "Y")
                {
                    // txtSmi.Text = "0";
                    calc_Poverty("SMI");
                }
                if (ProgramDefination.DepHUDUsed == "Y")
                {
                    // txtHud.Text = "0";
                    calc_Poverty("HUD");
                }
            }
            //if (ValidateCalculation())
            //{
            ChangeStatus(GetEligibilityStatus(), true);
            //}
        }

        private void ButtonsPrevilegs()
        {

            if (privilege.AddPriv.Equals("false"))
            {
                PbAdd.Visible = false;
            }
            else
            {
                PbAdd.Visible = true;
            }
            PbEdit.Visible = false;
            //if (privilege.ChangePriv.Equals("false"))
            //{
            //    PbEdit.Visible = false;
            //}
            //else
            //{
            //    PbEdit.Visible = true;
            //}


            if (privilege.DelPriv.Equals("false"))
            {
                PbDelete.Visible = false;
            }
            else
            {
                if (dataGridCaseIncomeVer.Rows.Count > 0)
                {
                    PbDelete.Visible = true;
                }
            }
            if (dataGridCaseIncomeVer.Rows.Count == 0)
            {
                PbEdit.Visible = false;
                PbDelete.Visible = false;
            }
        }

        private void btnVerIncomeForm_Click(object sender, EventArgs e)
        {
            On_ViewEligibility();
        }

        /// <summary>
        /// View Eligblity criteria report
        /// </summary>

        // Begin Report Section.......................
        string PDFModule = null;
        string Agency = null; string PdfName = "Pdf File";
        string strFolderPath = string.Empty;
        string StrHieCode = string.Empty;
        string Random_Filename = null;
        private void On_ViewEligibility()//object sender, FormClosedEventArgs e)
        {
            Random_Filename = null;
            PdfName = "Elig_Crit_" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo + "_" + StrHieCode;
            //PdfName = strFolderPath + PdfName;

            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
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

            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            //document.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 12, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bfTimes, 12, 2, BaseColor.BLUE);
            iTextSharp.text.Font fcGreen = new iTextSharp.text.Font(bfTimes, 12, 2, BaseColor.GREEN.Darker().Darker());
            iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bfTimes, 12, 2, BaseColor.RED.Darker().Brighter());
            PdfPTable table = new PdfPTable(2);
            table.TotalWidth = 500f;
            table.WidthPercentage = 100;
            table.LockedWidth = true;
            //table.DefaultCell.Border = PdfPCell.BOX;
            float[] widths = new float[] { 10f, 50f };
            table.SetWidths(widths);
            table.HorizontalAlignment = Element.ALIGN_CENTER;

            table.SpacingAfter = 05f;


            PdfPTable Groups = new PdfPTable(4);
            Groups.TotalWidth = 500f;
            Groups.WidthPercentage = 100;
            Groups.LockedWidth = true;
            //Groups.DefaultCell.Border = PdfPCell.BOX;
            float[] widths2 = new float[] { 80f, 40f, 40f, 10f };
            Groups.SetWidths(widths2);
            Groups.HorizontalAlignment = Element.ALIGN_CENTER;
            //Groups.SpacingBefore = 10f;
            //Groups.SpacingAfter = 10f;
            string PrivAgency = null;
            DataSet dsCaseElig = new DataSet();
            DataTable dtCaseElig = new DataTable();
            StrHieCode = StrHieCode.Trim();
            if (!string.IsNullOrEmpty(StrHieCode))
            {
                dsCaseElig = Captain.DatabaseLayer.CaseSum.Browse_CASEELIG(StrHieCode.Substring(0, 2).ToString(), StrHieCode.Substring(2, 2).ToString(), StrHieCode.Substring(4, 2).ToString());
                dtCaseElig = dsCaseElig.Tables[0];
                DataView dv = new DataView(dtCaseElig);
                dv.Sort = "CASEELIG_AGENCY,CASEELIG_DEPT,CASEELIG_PROGRAM,CASEELIG_GROUP_CODE,CASEELIG_GROUP_SEQ";
                dtCaseElig = dv.ToTable();

                //DataTable datMst=new DataTable();
                DataSet dsMst = Captain.DatabaseLayer.CaseMst.GetCASEMSTadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
                DataTable datMst = dsMst.Tables[0];

                bool First; string MemAccess = null; string Response = null; string Conjuction = null; string GrpConj = null;
                string Code = null; //bool First_Grp;
                List<CaseELIGQUESEntity> EligQueslist;
                EligQueslist = _model.CaseSumData.Browse_ELIGQUES();

                DataSet dsCust = Captain.DatabaseLayer.SPAdminDB.BrowseCustFlds();
                DataTable dtCust = dsCust.Tables[0];
                if (dtCaseElig.Rows.Count > 0)
                {
                    First = true; string strHier = string.Empty; string GroupCd = string.Empty; string Result = string.Empty;
                    foreach (DataRow drEligList in dtCaseElig.Rows)
                    {
                        strHier = drEligList["CASEELIG_AGENCY"].ToString() + drEligList["CASEELIG_DEPT"].ToString() + drEligList["CASEELIG_PROGRAM"].ToString();
                        DataSet dsProg = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(drEligList["CASEELIG_AGENCY"].ToString(), drEligList["CASEELIG_DEPT"].ToString(), drEligList["CASEELIG_PROGRAM"].ToString());
                        Agency = (dsProg.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();

                        if (Agency != PrivAgency)
                        {
                            PdfPCell Program = new PdfPCell(new Phrase("Program", fc));
                            Program.HorizontalAlignment = Element.ALIGN_LEFT;
                            Program.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(Program);

                            PdfPCell ProgDesc = new PdfPCell(new Phrase(Agency + "(" + drEligList["CASEELIG_AGENCY"].ToString() + "-" + drEligList["CASEELIG_DEPT"].ToString() + "-" + drEligList["CASEELIG_PROGRAM"].ToString() + ")", fc));
                            ProgDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProgDesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(ProgDesc);
                            PrivAgency = Agency;

                            PdfPCell Appli = new PdfPCell(new Phrase("Applicant Name", fc));
                            Appli.HorizontalAlignment = Element.ALIGN_LEFT;
                            Appli.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(Appli);

                            PdfPCell AppName = new PdfPCell(new Phrase(BaseForm.BaseApplicationName, fc));
                            AppName.HorizontalAlignment = Element.ALIGN_LEFT;
                            AppName.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(AppName);

                            table.SpacingBefore = 30f;
                            document.Add(table);                       //Header row
                            table.DeleteBodyRows();
                            //table.DeleteLastRow(); 
                        }

                        if (drEligList["CASEELIG_GROUP_SEQ"].ToString() == "0")
                        {
                            GroupCd = drEligList["CASEELIG_GROUP_CODE"].ToString().Trim();

                            if (drEligList["CASEELIG_CONJN"].ToString() == "A")
                                GrpConj = "And";
                            else if (drEligList["CASEELIG_CONJN"].ToString() == "O")
                                GrpConj = "Or";
                            else
                                GrpConj = " ";
                            if (!First)
                            {
                                PdfPCell GrpSpace = new PdfPCell(new Phrase(" ", fc1));
                                GrpSpace.HorizontalAlignment = Element.ALIGN_LEFT;
                                GrpSpace.Colspan = 4;
                                GrpSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Groups.AddCell(GrpSpace);
                            }

                            if (string.IsNullOrEmpty(drEligList["CASEELIG_CONJN"].ToString()))
                            {
                                PdfPCell GrpDesc = new PdfPCell(new Phrase(drEligList["CASEELIG_GROUP_DESC"].ToString(), fc1));
                                GrpDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                GrpDesc.Colspan = 4;
                                GrpDesc.Border = iTextSharp.text.Rectangle.BOX;
                                Groups.AddCell(GrpDesc);
                            }
                            else
                            {
                                PdfPCell GrpDesc = new PdfPCell(new Phrase(drEligList["CASEELIG_GROUP_DESC"].ToString(), fc1));
                                GrpDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                GrpDesc.Colspan = 3;
                                GrpDesc.Border = iTextSharp.text.Rectangle.BOX;
                                Groups.AddCell(GrpDesc);
                            }

                            //PdfPCell Space = new PdfPCell(new Phrase(" ", fc1));
                            //Space.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Space.Border = iTextSharp.text.Rectangle.BOX;
                            //Groups.AddCell(Space);

                            //PdfPCell Space1 = new PdfPCell(new Phrase(" ", fc1));
                            //Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Space1.Border = iTextSharp.text.Rectangle.BOX;
                            //Groups.AddCell(Space1);
                            if (!string.IsNullOrEmpty(drEligList["CASEELIG_CONJN"].ToString()))
                            {
                                PdfPCell Space2 = new PdfPCell(new Phrase(GrpConj, fc1));
                                Space2.HorizontalAlignment = Element.ALIGN_LEFT;
                                Space2.Border = iTextSharp.text.Rectangle.BOX;
                                Groups.AddCell(Space2);
                            }

                            First = false;
                            document.Add(Groups);
                            Groups.DeleteBodyRows();
                        }
                        else
                        {
                            Code = drEligList["CASEELIG_QUES_CODE"].ToString();
                            string GrpSeq = drEligList["CASEELIG_GROUP_SEQ"].ToString();

                            Result = GetEligibilityQuestionStatus(strHier, GroupCd, Code, GrpSeq);
                            if (Code.Substring(0, 1).ToString() == "C")
                            {
                                foreach (DataRow drCust in dtCust.Rows)
                                {
                                    if (Code.ToString().Trim() == drCust["CUST_CODE"].ToString())
                                    {
                                        string Desc = drCust["CUST_DESC"].ToString();
                                        string sequence = drCust["CUST_SEQ"].ToString();

                                        if (Result == "Y")
                                        {
                                            PdfPCell Group = new PdfPCell(new Phrase(Desc, fcGreen));
                                            Group.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Group.Border = iTextSharp.text.Rectangle.BOX;
                                            //Group.BackColor = new BaseColor(224, 224, 224);
                                            Groups.AddCell(Group);
                                        }
                                        else
                                        {
                                            PdfPCell Group = new PdfPCell(new Phrase(Desc, fcRed));
                                            Group.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Group.Border = iTextSharp.text.Rectangle.BOX;
                                            //Group.BackColor = new BaseColor(224, 224, 224);
                                            Groups.AddCell(Group);
                                        }
                                        if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "*")
                                            MemAccess = "All Members";
                                        else if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "A")
                                            MemAccess = "Applicant Only";
                                        else if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "1")
                                            MemAccess = "Any One Member";
                                        else if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "H")
                                            MemAccess = "Household";
                                        else
                                            MemAccess = " ";

                                        PdfPCell MemberAccess = new PdfPCell(new Phrase(MemAccess, fc));
                                        MemberAccess.HorizontalAlignment = Element.ALIGN_LEFT;
                                        MemberAccess.Border = iTextSharp.text.Rectangle.BOX;
                                        //MemberAccess.BackColor = new BaseColor(224, 224, 224);
                                        Groups.AddCell(MemberAccess);

                                        if (drEligList["CASEELIG_RESP_TYPE"].ToString() == "D")
                                        {
                                            DataSet dsResp = DatabaseLayer.SPAdminDB.BrowseCustResp(drCust["CUST_CODE"].ToString());
                                            DataTable dtResp = dsResp.Tables[0];
                                            foreach (DataRow dr in dtResp.Rows)
                                            {
                                                if (drEligList["CASEELIG_DD_TEXT_RESP"].ToString().Trim() == dr["RSP_RESP_CODE"].ToString().Trim())
                                                {
                                                    Response = dr["RSP_DESC"].ToString().Trim(); break;
                                                }
                                            }
                                        }
                                        else if (drEligList["CASEELIG_DD_TEXT_RESP"].ToString().ToString() == "N")
                                        {
                                            if (drEligList["CASEELIG_NUM_EQUAL"].ToString() != string.Empty && drEligList["CASEELIG_NUM_EQUAL"].ToString() != "0.00")
                                                Response = "= " + drEligList["CASEELIG_NUM_EQUAL"].ToString() + ",";
                                            if (drEligList["CASEELIG_NUM_LTHAN"].ToString() != string.Empty && drEligList["CASEELIG_NUM_LTHAN"].ToString() != "0.00")
                                                Response = Response + "<" + drEligList["CASEELIG_NUM_LTHAN"].ToString() + ",";
                                            if (drEligList["CASEELIG_NUM_GTHAN"].ToString() != string.Empty && drEligList["CASEELIG_NUM_GTHAN"].ToString() != "0.00")
                                                Response = Response + "> " + drEligList["CASEELIG_NUM_GTHAN"].ToString() + ",";
                                            Response = Response.Remove(Response.Length - 1);
                                        }
                                        else
                                            Response = " ";

                                        if (drEligList["CASEELIG_CONJN"].ToString() == "A")
                                            Conjuction = "And";
                                        else if (drEligList["CASEELIG_CONJN"].ToString() == "O")
                                            Conjuction = "Or";
                                        else
                                            Conjuction = "";

                                        PdfPCell Resp = new PdfPCell(new Phrase(Response, fc));
                                        Resp.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Resp.Border = iTextSharp.text.Rectangle.BOX;
                                        //Resp.BackColor = new BaseColor(224, 224, 224);
                                        Groups.AddCell(Resp);

                                        PdfPCell Conjuc = new PdfPCell(new Phrase(Conjuction, fc));
                                        Conjuc.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        Conjuc.Border = iTextSharp.text.Rectangle.BOX;
                                        //Conjuc.BackColor = new BaseColor(224, 224, 224);
                                        Groups.AddCell(Conjuc);

                                        DataSet dsAddCust = DatabaseLayer.CaseSum.Browse_AddCust(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
                                        DataTable dtAddCust = dsAddCust.Tables[0];
                                        DataRow[] FoundRows;
                                        FoundRows = dsAddCust.Tables[0].Select("ACT_CODE='" + Code.ToString() + "'");

                                        PdfPCell Name = new PdfPCell();
                                        Name.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Name.Rowspan = dtAddCust.Rows.Count;
                                        Name.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        string Priv_Responce = null;
                                        PdfPCell Field = new PdfPCell();
                                        Field.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Field.Rowspan = dtAddCust.Rows.Count;
                                        //Field.Colspan = 3;
                                        Field.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        if (FoundRows.Length != 0)
                                        {
                                            if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "A")//if (drCust["CUST_MEM_ACCESS"].ToString().Trim() == "A")
                                            {

                                                Name = new PdfPCell(new Phrase(BaseForm.BaseApplicationName, fc));
                                                Name.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                Groups.AddCell(Name);
                                                foreach (DataRow drAddCust in FoundRows)
                                                {
                                                    if (drAddCust["ACT_CODE"].ToString().Trim() == Code.ToString().Trim() && drAddCust["ACT_SNP_FAMILY_SEQ"].ToString() == "9999999")
                                                    {
                                                        string FliedValue = drAddCust["ACT_MULT_RESP"].ToString().Trim();
                                                        DataSet dsCustResp = DatabaseLayer.SPAdminDB.BrowseCustResp(Code);
                                                        DataTable dtCustResp = dsCustResp.Tables[0];
                                                        foreach (DataRow drCustResp in dtCustResp.Rows)
                                                        {
                                                            if (drCustResp["RSP_RESP_CODE"].ToString().Trim() == FliedValue.ToString().Trim())
                                                            {
                                                                string CustResp = drCustResp["RSP_DESC"].ToString();
                                                                Field = new PdfPCell(new Phrase(CustResp, fc));
                                                                //Field.HorizontalAlignment = Element.ALIGN_CENTER;
                                                                Field.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                                Field.Colspan = 3;
                                                                Groups.AddCell(Field);
                                                            }
                                                        }
                                                    }
                                                    //else if (drAddCust["ACT_CODE"].ToString().Trim() == Code.ToString().Trim())
                                                    //{
                                                    //    string Responce = "   ";
                                                    //    if (Responce != Priv_Responce)
                                                    //    {
                                                    //        Field = new PdfPCell(new Phrase("UnAnswered", fc));
                                                    //        Field.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                    //        Field.Colspan = 3;
                                                    //        Groups.AddCell(Field);
                                                    //        Priv_Responce = Responce;
                                                    //    }
                                                    //}
                                                }
                                            }
                                            else if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "H")//if (drCust["CUST_MEM_ACCESS"].ToString().Trim() == "H")
                                            {
                                                //string Priv_Responce = null;
                                                Name = new PdfPCell(new Phrase("Household", fc));
                                                Name.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                Groups.AddCell(Name);
                                                foreach (DataRow drAddCust in FoundRows)
                                                {
                                                    if (drAddCust["ACT_CODE"].ToString().Trim() == Code.ToString().Trim() && drAddCust["ACT_SNP_FAMILY_SEQ"].ToString() == "8888888")
                                                    {
                                                        string FliedValue = drAddCust["ACT_MULT_RESP"].ToString().Trim();
                                                        DataSet dsCustResp = DatabaseLayer.SPAdminDB.BrowseCustResp(Code);
                                                        DataTable dtCustResp = dsCustResp.Tables[0];
                                                        foreach (DataRow drCustResp in dtCustResp.Rows)
                                                        {
                                                            if (drCustResp["RSP_RESP_CODE"].ToString().Trim() == FliedValue.ToString().Trim())
                                                            {
                                                                string CustResp = drCustResp["RSP_DESC"].ToString();
                                                                Field = new PdfPCell(new Phrase(CustResp, fc));
                                                                //Field.HorizontalAlignment = Element.ALIGN_CENTER;
                                                                Field.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                                Field.Colspan = 3;
                                                                Groups.AddCell(Field);
                                                            }
                                                        }
                                                    }
                                                    //else
                                                    //{
                                                    //    string Responce = "   ";
                                                    //    if (Responce != Priv_Responce)
                                                    //    {
                                                    //        Field = new PdfPCell(new Phrase("UnAnswered", fc));
                                                    //        Field.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                    //        Field.Colspan = 3;
                                                    //        Groups.AddCell(Field);
                                                    //        Priv_Responce = Responce;
                                                    //    }
                                                    //}
                                                }
                                            }
                                            else if (drEligList["CASEELIG_MEM_ACCESS"].ToString().Trim() == "1")
                                            {
                                                DataSet dsSnp = DatabaseLayer.CaseSnpData.GetCaseSnpDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, null);
                                                DataTable dtSnp = dsSnp.Tables[0];

                                                foreach (DataRow drSnp in dtSnp.Rows)
                                                {
                                                    string ApplicantName = LookupDataAccess.GetMemberName(drSnp["SNP_NAME_IX_FI"].ToString(), drSnp["SNP_NAME_IX_MI"].ToString(), drSnp["SNP_NAME_IX_LAST"].ToString(), strNameFormat);
                                                    Name = new PdfPCell(new Phrase(ApplicantName, fc));
                                                    Name.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                    Groups.AddCell(Name); string CustResp = null;
                                                    foreach (DataRow drAddCust in FoundRows)
                                                    {
                                                        CustResp = null;
                                                        if (drAddCust["ACT_CODE"].ToString().Trim() == Code.ToString().Trim() && drSnp["SNP_FAMILY_SEQ"].ToString() == drAddCust["ACT_SNP_FAMILY_SEQ"].ToString())
                                                        {
                                                            string FliedValue = drAddCust["ACT_MULT_RESP"].ToString().Trim();
                                                            DataSet dsCustResp = DatabaseLayer.SPAdminDB.BrowseCustResp(Code);
                                                            DataTable dtCustResp = dsCustResp.Tables[0];
                                                            foreach (DataRow drCustResp in dtCustResp.Rows)
                                                            {
                                                                if (drCustResp["RSP_RESP_CODE"].ToString().Trim() == FliedValue.ToString().Trim())
                                                                {
                                                                    CustResp = drCustResp["RSP_DESC"].ToString();
                                                                    Field = new PdfPCell(new Phrase(CustResp, fc));
                                                                    //Field.HorizontalAlignment = Element.ALIGN_CENTER;
                                                                    Field.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                                    Field.Colspan = 3;
                                                                    Groups.AddCell(Field);
                                                                }
                                                            }

                                                        }
                                                        else if (drAddCust["ACT_SNP_FAMILY_SEQ"].ToString().Trim() == "9999999" && drSnp["SNP_FAMILY_SEQ"].ToString().Trim() == BaseForm.BaseCaseMstListEntity[0].FamilySeq.Trim())
                                                        {
                                                            string FliedValue = drAddCust["ACT_MULT_RESP"].ToString().Trim();
                                                            DataSet dsCustResp = DatabaseLayer.SPAdminDB.BrowseCustResp(Code);
                                                            DataTable dtCustResp = dsCustResp.Tables[0];
                                                            foreach (DataRow drCustResp in dtCustResp.Rows)
                                                            {
                                                                if (drCustResp["RSP_RESP_CODE"].ToString().Trim() == FliedValue.ToString().Trim())
                                                                {
                                                                    CustResp = drCustResp["RSP_DESC"].ToString();
                                                                    Field = new PdfPCell(new Phrase(CustResp, fc));
                                                                    //Field.HorizontalAlignment = Element.ALIGN_CENTER;
                                                                    Field.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                                    Field.Colspan = 3;
                                                                    Groups.AddCell(Field);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //Responce = "   ";
                                                            if (ApplicantName.Trim() != Priv_Responce)
                                                            {
                                                                Field = new PdfPCell(new Phrase(" ", fc)); //UnAnswered
                                                                Field.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                                Field.Colspan = 3;
                                                                Groups.AddCell(Field);
                                                                Priv_Responce = ApplicantName.Trim();
                                                            }
                                                        }
                                                    }

                                                }
                                            }

                                            else
                                            {
                                                DataSet dsSnp = DatabaseLayer.CaseSnpData.GetCaseSnpDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, null);
                                                DataTable dtSnp = dsSnp.Tables[0];

                                                foreach (DataRow drSnp in dtSnp.Rows)
                                                {
                                                    string ApplicantName = LookupDataAccess.GetMemberName(drSnp["SNP_NAME_IX_FI"].ToString(), drSnp["SNP_NAME_IX_MI"].ToString(), drSnp["SNP_NAME_IX_LAST"].ToString(), strNameFormat);
                                                    Name = new PdfPCell(new Phrase(ApplicantName, fc));
                                                    Name.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                    Groups.AddCell(Name); string Responce = null;
                                                    foreach (DataRow drAddCust in FoundRows)
                                                    {
                                                        if (drAddCust["ACT_CODE"].ToString().Trim() == Code.ToString().Trim() && drSnp["SNP_FAMILY_SEQ"].ToString() == drAddCust["ACT_SNP_FAMILY_SEQ"].ToString())
                                                        {
                                                            string FliedValue = drAddCust["ACT_MULT_RESP"].ToString().Trim();
                                                            DataSet dsCustResp = DatabaseLayer.SPAdminDB.BrowseCustResp(Code);
                                                            DataTable dtCustResp = dsCustResp.Tables[0];
                                                            foreach (DataRow drCustResp in dtCustResp.Rows)
                                                            {
                                                                if (drCustResp["RSP_RESP_CODE"].ToString().Trim() == FliedValue.ToString().Trim())
                                                                {
                                                                    string CustResp = drCustResp["RSP_DESC"].ToString();
                                                                    Field = new PdfPCell(new Phrase(CustResp, fc));
                                                                    //Field.HorizontalAlignment = Element.ALIGN_CENTER;
                                                                    Field.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                                    Field.Colspan = 3;
                                                                    Groups.AddCell(Field);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //Responce = "   ";
                                                            if (ApplicantName.Trim() != Priv_Responce)
                                                            {
                                                                Field = new PdfPCell(new Phrase(" ", fc)); //UnAnswered
                                                                Field.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                                Field.Colspan = 3;
                                                                Groups.AddCell(Field);
                                                                Priv_Responce = ApplicantName.Trim();
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                        else
                                        {
                                            Name = new PdfPCell(new Phrase(BaseForm.BaseApplicationName, fc));
                                            Name.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                            Groups.AddCell(Name);
                                            string Responce = "   ";
                                            if (Responce != Priv_Responce)
                                            {
                                                Field = new PdfPCell(new Phrase(" ", fc)); //UnAnswered
                                                Field.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                Field.Colspan = 3;
                                                Groups.AddCell(Field);
                                                Priv_Responce = Responce;
                                            }
                                        }


                                        //if (First)
                                        //{
                                        //    table.SpacingBefore = 30f;
                                        //    document.Add(table);                       //Header row
                                        //    table.DeleteBodyRows();
                                        //    First = false;
                                        //}

                                        document.Add(Groups);
                                        Groups.DeleteBodyRows();
                                        //break;
                                    }
                                }

                            }
                            else if (Code.Substring(0, 1).ToString() == "P")
                            {
                                foreach (DataRow drCust in dtCust.Rows)
                                {
                                    if (Code.ToString().Trim() == drCust["CUST_CODE"].ToString())
                                    {
                                        string Desc = drCust["CUST_DESC"].ToString();
                                        string sequence = drCust["CUST_SEQ"].ToString();

                                        if (Result == "Y")
                                        {
                                            PdfPCell Group = new PdfPCell(new Phrase(Desc, fcGreen));
                                            Group.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Group.Border = iTextSharp.text.Rectangle.BOX;
                                            Group.BackgroundColor = new BaseColor(224, 224, 224);
                                            Groups.AddCell(Group);
                                        }
                                        else
                                        {
                                            PdfPCell Group = new PdfPCell(new Phrase(Desc, fcRed));
                                            Group.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Group.Border = iTextSharp.text.Rectangle.BOX;
                                            Group.BackgroundColor = new BaseColor(224, 224, 224);
                                            Groups.AddCell(Group);
                                        }
                                        if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "*")
                                            MemAccess = "All Members";
                                        else if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "A")
                                            MemAccess = "Applicant Only";
                                        else if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "1")
                                            MemAccess = "Any One Member";
                                        else if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "H")
                                            MemAccess = "Household";
                                        else
                                            MemAccess = " ";

                                        PdfPCell MemberAccess = new PdfPCell(new Phrase(MemAccess, fc));
                                        MemberAccess.HorizontalAlignment = Element.ALIGN_LEFT;
                                        MemberAccess.Border = iTextSharp.text.Rectangle.BOX;
                                        MemberAccess.BackgroundColor = new BaseColor(224, 224, 224);
                                        Groups.AddCell(MemberAccess);

                                        if (drEligList["CASEELIG_RESP_TYPE"].ToString() == "D")
                                        {
                                            DataSet dsResp = DatabaseLayer.SPAdminDB.BrowseCustResp(drCust["CUST_CODE"].ToString());
                                            DataTable dtResp = dsResp.Tables[0];
                                            foreach (DataRow dr in dtResp.Rows)
                                            {
                                                if (drEligList["CASEELIG_DD_TEXT_RESP"].ToString().Trim() == dr["RSP_RESP_CODE"].ToString().Trim())
                                                {
                                                    Response = dr["RSP_DESC"].ToString().Trim(); break;
                                                }
                                            }
                                        }
                                        else if (drEligList["CASEELIG_DD_TEXT_RESP"].ToString().ToString() == "N")
                                        {
                                            if (drEligList["CASEELIG_NUM_EQUAL"].ToString() != string.Empty && drEligList["CASEELIG_NUM_EQUAL"].ToString() != "0.00")
                                                Response = "= " + drEligList["CASEELIG_NUM_EQUAL"].ToString() + ",";
                                            if (drEligList["CASEELIG_NUM_LTHAN"].ToString() != string.Empty && drEligList["CASEELIG_NUM_LTHAN"].ToString() != "0.00")
                                                Response = Response + "<" + drEligList["CASEELIG_NUM_LTHAN"].ToString() + ",";
                                            if (drEligList["CASEELIG_NUM_GTHAN"].ToString() != string.Empty && drEligList["CASEELIG_NUM_GTHAN"].ToString() != "0.00")
                                                Response = Response + "> " + drEligList["CASEELIG_NUM_GTHAN"].ToString() + ",";
                                            Response = Response.Remove(Response.Length - 1);
                                        }
                                        else
                                            Response = " ";

                                        if (drEligList["CASEELIG_CONJN"].ToString() == "A")
                                            Conjuction = "And";
                                        else if (drEligList["CASEELIG_CONJN"].ToString() == "O")
                                            Conjuction = "Or";
                                        else
                                            Conjuction = "";

                                        PdfPCell Resp = new PdfPCell(new Phrase(Response, fc));
                                        Resp.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Resp.Border = iTextSharp.text.Rectangle.BOX;
                                        Resp.BackgroundColor = new BaseColor(224, 224, 224);
                                        Groups.AddCell(Resp);

                                        PdfPCell Conjuc = new PdfPCell(new Phrase(Conjuction, fc));
                                        Conjuc.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        Conjuc.Border = iTextSharp.text.Rectangle.BOX;
                                        Conjuc.BackgroundColor = new BaseColor(224, 224, 224);
                                        Groups.AddCell(Conjuc);

                                        DataSet dsAddCust = DatabaseLayer.CaseSnpData.GetCustomQuestionAnswers(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty, "PRESRESP");
                                        DataTable dtAddCust = dsAddCust.Tables[0];
                                        DataRow[] FoundRows;
                                        FoundRows = dsAddCust.Tables[0].Select("PRES_CODE='" + Code.ToString() + "'");

                                        PdfPCell Name = new PdfPCell();
                                        Name.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Name.Rowspan = dtAddCust.Rows.Count;
                                        Name.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        string Priv_Responce = null;
                                        PdfPCell Field = new PdfPCell();
                                        Field.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Field.Rowspan = dtAddCust.Rows.Count;
                                        //Field.Colspan = 3;
                                        Field.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        if (FoundRows.Length != 0)
                                        {
                                            if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "A")//if (drCust["CUST_MEM_ACCESS"].ToString().Trim() == "A")
                                            {

                                                Name = new PdfPCell(new Phrase(BaseForm.BaseApplicationName, fc));
                                                Name.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                Groups.AddCell(Name);
                                                foreach (DataRow drAddCust in FoundRows)
                                                {
                                                    if (drAddCust["PRES_CODE"].ToString().Trim() == Code.ToString().Trim() && drAddCust["PRES_SNP_FAMILY_SEQ"].ToString() == "7777777")
                                                    {
                                                        string FliedValue = drAddCust["PRES_MULT_RESP"].ToString().Trim();
                                                        DataSet dsCustResp = DatabaseLayer.SPAdminDB.BrowseCustResp(Code);
                                                        DataTable dtCustResp = dsCustResp.Tables[0];
                                                        foreach (DataRow drCustResp in dtCustResp.Rows)
                                                        {
                                                            if (drCustResp["RSP_RESP_CODE"].ToString().Trim() == FliedValue.ToString().Trim())
                                                            {
                                                                string CustResp = drCustResp["RSP_DESC"].ToString();
                                                                Field = new PdfPCell(new Phrase(CustResp, fc));
                                                                //Field.HorizontalAlignment = Element.ALIGN_CENTER;
                                                                Field.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                                Field.Colspan = 3;
                                                                Groups.AddCell(Field);
                                                            }
                                                        }
                                                    }
                                                    //else if (drAddCust["ACT_CODE"].ToString().Trim() == Code.ToString().Trim())
                                                    //{
                                                    //    string Responce = "   ";
                                                    //    if (Responce != Priv_Responce)
                                                    //    {
                                                    //        Field = new PdfPCell(new Phrase("UnAnswered", fc));
                                                    //        Field.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                    //        Field.Colspan = 3;
                                                    //        Groups.AddCell(Field);
                                                    //        Priv_Responce = Responce;
                                                    //    }
                                                    //}
                                                }
                                            }
                                            else if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "H")//if (drCust["CUST_MEM_ACCESS"].ToString().Trim() == "H")
                                            {
                                                //string Priv_Responce = null;
                                                Name = new PdfPCell(new Phrase("Household", fc));
                                                Name.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                Groups.AddCell(Name);
                                                foreach (DataRow drAddCust in FoundRows)
                                                {
                                                    if (drAddCust["PRES_CODE"].ToString().Trim() == Code.ToString().Trim() && drAddCust["PRES_SNP_FAMILY_SEQ"].ToString() == "7777777")
                                                    {
                                                        string FliedValue = drAddCust["PRES_MULT_RESP"].ToString().Trim();
                                                        DataSet dsCustResp = DatabaseLayer.SPAdminDB.BrowseCustResp(Code);
                                                        DataTable dtCustResp = dsCustResp.Tables[0];
                                                        foreach (DataRow drCustResp in dtCustResp.Rows)
                                                        {
                                                            if (drCustResp["RSP_RESP_CODE"].ToString().Trim() == FliedValue.ToString().Trim())
                                                            {
                                                                string CustResp = drCustResp["RSP_DESC"].ToString();
                                                                Field = new PdfPCell(new Phrase(CustResp, fc));
                                                                //Field.HorizontalAlignment = Element.ALIGN_CENTER;
                                                                Field.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                                Field.Colspan = 3;
                                                                Groups.AddCell(Field);
                                                            }
                                                        }
                                                    }
                                                    //else
                                                    //{
                                                    //    string Responce = "   ";
                                                    //    if (Responce != Priv_Responce)
                                                    //    {
                                                    //        Field = new PdfPCell(new Phrase("UnAnswered", fc));
                                                    //        Field.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                    //        Field.Colspan = 3;
                                                    //        Groups.AddCell(Field);
                                                    //        Priv_Responce = Responce;
                                                    //    }
                                                    //}
                                                }
                                            }
                                            else if (drEligList["CASEELIG_MEM_ACCESS"].ToString().Trim() == "1")
                                            {
                                                DataSet dsSnp = DatabaseLayer.CaseSnpData.GetCaseSnpDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, null);
                                                DataTable dtSnp = dsSnp.Tables[0];

                                                foreach (DataRow drSnp in dtSnp.Rows)
                                                {
                                                    string ApplicantName = LookupDataAccess.GetMemberName(drSnp["SNP_NAME_IX_FI"].ToString(), drSnp["SNP_NAME_IX_MI"].ToString(), drSnp["SNP_NAME_IX_LAST"].ToString(), strNameFormat);
                                                    Name = new PdfPCell(new Phrase(ApplicantName, fc));
                                                    Name.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                    Groups.AddCell(Name); string CustResp = null;
                                                    foreach (DataRow drAddCust in FoundRows)
                                                    {
                                                        CustResp = null;
                                                        if (drAddCust["PRES_CODE"].ToString().Trim() == Code.ToString().Trim() && drSnp["SNP_FAMILY_SEQ"].ToString() == drAddCust["PRES_SNP_FAMILY_SEQ"].ToString())
                                                        {
                                                            string FliedValue = drAddCust["PRES_MULT_RESP"].ToString().Trim();
                                                            DataSet dsCustResp = DatabaseLayer.SPAdminDB.BrowseCustResp(Code);
                                                            DataTable dtCustResp = dsCustResp.Tables[0];
                                                            foreach (DataRow drCustResp in dtCustResp.Rows)
                                                            {
                                                                if (drCustResp["RSP_RESP_CODE"].ToString().Trim() == FliedValue.ToString().Trim())
                                                                {
                                                                    CustResp = drCustResp["RSP_DESC"].ToString();
                                                                    Field = new PdfPCell(new Phrase(CustResp, fc));
                                                                    //Field.HorizontalAlignment = Element.ALIGN_CENTER;
                                                                    Field.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                                    Field.Colspan = 3;
                                                                    Groups.AddCell(Field);
                                                                }
                                                            }

                                                        }
                                                        else if (drAddCust["PRES_SNP_FAMILY_SEQ"].ToString().Trim() == "7777777" && drSnp["SNP_FAMILY_SEQ"].ToString().Trim() == BaseForm.BaseCaseMstListEntity[0].FamilySeq.Trim())
                                                        {
                                                            string FliedValue = drAddCust["PRES_MULT_RESP"].ToString().Trim();
                                                            DataSet dsCustResp = DatabaseLayer.SPAdminDB.BrowseCustResp(Code);
                                                            DataTable dtCustResp = dsCustResp.Tables[0];
                                                            foreach (DataRow drCustResp in dtCustResp.Rows)
                                                            {
                                                                if (drCustResp["RSP_RESP_CODE"].ToString().Trim() == FliedValue.ToString().Trim())
                                                                {
                                                                    CustResp = drCustResp["RSP_DESC"].ToString();
                                                                    Field = new PdfPCell(new Phrase(CustResp, fc));
                                                                    //Field.HorizontalAlignment = Element.ALIGN_CENTER;
                                                                    Field.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                                    Field.Colspan = 3;
                                                                    Groups.AddCell(Field);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //Responce = "   ";
                                                            if (ApplicantName.Trim() != Priv_Responce)
                                                            {
                                                                Field = new PdfPCell(new Phrase(" ", fc)); //UnAnswered
                                                                Field.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                                Field.Colspan = 3;
                                                                Groups.AddCell(Field);
                                                                Priv_Responce = ApplicantName.Trim();
                                                            }
                                                        }
                                                    }

                                                }
                                            }

                                            else
                                            {
                                                DataSet dsSnp = DatabaseLayer.CaseSnpData.GetCaseSnpDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, null);
                                                DataTable dtSnp = dsSnp.Tables[0];

                                                foreach (DataRow drSnp in dtSnp.Rows)
                                                {
                                                    string ApplicantName = LookupDataAccess.GetMemberName(drSnp["SNP_NAME_IX_FI"].ToString(), drSnp["SNP_NAME_IX_MI"].ToString(), drSnp["SNP_NAME_IX_LAST"].ToString(), strNameFormat);
                                                    Name = new PdfPCell(new Phrase(ApplicantName, fc));
                                                    Name.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                    Groups.AddCell(Name); string Responce = null;
                                                    foreach (DataRow drAddCust in FoundRows)
                                                    {
                                                        if (drAddCust["PRES_CODE"].ToString().Trim() == Code.ToString().Trim() && drSnp["SNP_FAMILY_SEQ"].ToString() == drAddCust["PRES_SNP_FAMILY_SEQ"].ToString())
                                                        {
                                                            string FliedValue = drAddCust["PRES_MULT_RESP"].ToString().Trim();
                                                            DataSet dsCustResp = DatabaseLayer.SPAdminDB.BrowseCustResp(Code);
                                                            DataTable dtCustResp = dsCustResp.Tables[0];
                                                            foreach (DataRow drCustResp in dtCustResp.Rows)
                                                            {
                                                                if (drCustResp["RSP_RESP_CODE"].ToString().Trim() == FliedValue.ToString().Trim())
                                                                {
                                                                    string CustResp = drCustResp["RSP_DESC"].ToString();
                                                                    Field = new PdfPCell(new Phrase(CustResp, fc));
                                                                    //Field.HorizontalAlignment = Element.ALIGN_CENTER;
                                                                    Field.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                                    Field.Colspan = 3;
                                                                    Groups.AddCell(Field);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //Responce = "   ";
                                                            if (ApplicantName.Trim() != Priv_Responce)
                                                            {
                                                                Field = new PdfPCell(new Phrase(" ", fc)); //UnAnswered
                                                                Field.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                                Field.Colspan = 3;
                                                                Groups.AddCell(Field);
                                                                Priv_Responce = ApplicantName.Trim();
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                        else
                                        {
                                            Name = new PdfPCell(new Phrase(BaseForm.BaseApplicationName, fc));
                                            Name.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                            Groups.AddCell(Name);
                                            string Responce = "   ";
                                            if (Responce != Priv_Responce)
                                            {
                                                Field = new PdfPCell(new Phrase(" ", fc)); //UnAnswered
                                                Field.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                Field.Colspan = 3;
                                                Groups.AddCell(Field);
                                                Priv_Responce = Responce;
                                            }
                                        }


                                        //if (First)
                                        //{
                                        //    table.SpacingBefore = 30f;
                                        //    document.Add(table);                       //Header row
                                        //    table.DeleteBodyRows();
                                        //    First = false;
                                        //}

                                        document.Add(Groups);
                                        Groups.DeleteBodyRows();
                                        //break;
                                    }
                                }

                            }
                            else
                            {
                                foreach (CaseELIGQUESEntity EligQuees in EligQueslist)
                                {
                                    if (Code.ToString().Trim() == EligQuees.EligQuesCode.ToString())
                                    {
                                        string Desc = EligQuees.EligQuesDesc.ToString();

                                        if (Result == "Y")
                                        {
                                            PdfPCell Group = new PdfPCell(new Phrase(Desc, fcGreen));
                                            Group.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Group.Border = iTextSharp.text.Rectangle.BOX;
                                            Group.BackgroundColor = new BaseColor(224, 224, 224);
                                            Groups.AddCell(Group);
                                        }
                                        else
                                        {
                                            PdfPCell Group = new PdfPCell(new Phrase(Desc, fcRed));
                                            Group.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Group.Border = iTextSharp.text.Rectangle.BOX;
                                            Group.BackgroundColor = new BaseColor(224, 224, 224);
                                            Groups.AddCell(Group);
                                        }

                                        if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "*")
                                            MemAccess = "All Members";
                                        else if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "A")
                                            MemAccess = "Applicant Only";
                                        else if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "1")
                                            MemAccess = "Any One Member";
                                        else if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "H")
                                            MemAccess = "Household";
                                        else
                                            MemAccess = " ";
                                        PdfPCell MemberAccess = new PdfPCell(new Phrase(MemAccess, fc));
                                        MemberAccess.HorizontalAlignment = Element.ALIGN_LEFT;
                                        MemberAccess.Border = iTextSharp.text.Rectangle.BOX;
                                        MemberAccess.BackgroundColor = new BaseColor(224, 224, 224);
                                        Groups.AddCell(MemberAccess);
                                        if (drEligList["CASEELIG_RESP_TYPE"].ToString() == "D")
                                        {
                                            Response = null;
                                            DataSet dsResp = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(EligQuees.EligAgyCode);
                                            DataTable dtResp = dsResp.Tables[0];
                                            foreach (DataRow dr in dtResp.Rows)
                                            {
                                                if (drEligList["CASEELIG_DD_TEXT_RESP"].ToString().Trim() == dr["Code"].ToString().Trim())
                                                {
                                                    Response = dr["LookUpDesc"].ToString(); break;
                                                }
                                            }
                                        }
                                        else if (drEligList["CASEELIG_RESP_TYPE"].ToString() == "L")
                                        {
                                            Response = null;
                                            DataSet dsResp = DatabaseLayer.CaseSum.GetAGYTABS(EligQuees.EligAgyCode);
                                            DataTable dtResp = dsResp.Tables[0];
                                            foreach (DataRow dr in dtResp.Rows)
                                            {
                                                if (drEligList["CASEELIG_DD_TEXT_RESP"].ToString().Trim() == dr["AGYS_CODE"].ToString().Trim())
                                                {
                                                    Response = dr["AGYS_DESC"].ToString(); break;
                                                }
                                            }
                                            if (string.IsNullOrEmpty(Response))
                                                Response = " ";
                                        }
                                        else if (drEligList["CASEELIG_RESP_TYPE"].ToString() == "N")
                                        {
                                            Response = null;
                                            string Equal = drEligList["CASEELIG_NUM_EQUAL"].ToString();
                                            string Minimum = drEligList["CASEELIG_NUM_LTHAN"].ToString();
                                            string Maximum = drEligList["CASEELIG_NUM_GTHAN"].ToString();
                                            if (drEligList["CASEELIG_NUM_EQUAL"].ToString() != string.Empty && Equal != "0.00")
                                                Response = "= " + drEligList["CASEELIG_NUM_EQUAL"].ToString() + ",";
                                            if (drEligList["CASEELIG_NUM_LTHAN"].ToString() != string.Empty && Minimum != "0.00")
                                                Response = Response + "<" + drEligList["CASEELIG_NUM_LTHAN"].ToString() + ",";
                                            if (drEligList["CASEELIG_NUM_GTHAN"].ToString() != string.Empty && Maximum != "0.00")
                                                Response = Response + "> " + drEligList["CASEELIG_NUM_GTHAN"].ToString() + ",";
                                            Response = Response.Remove(Response.Length - 1);
                                        }
                                        else
                                            Response = " ";

                                        if (drEligList["CASEELIG_CONJN"].ToString() == "A")
                                            Conjuction = "And";
                                        else if (drEligList["CASEELIG_CONJN"].ToString() == "O")
                                            Conjuction = "Or";
                                        else
                                            Conjuction = "";
                                        PdfPCell Resp = new PdfPCell(new Phrase(Response, fc));
                                        Resp.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Resp.Border = iTextSharp.text.Rectangle.BOX;
                                        Resp.BackgroundColor = new BaseColor(224, 224, 224);
                                        Groups.AddCell(Resp);

                                        PdfPCell Conjuc = new PdfPCell(new Phrase(Conjuction, fc));
                                        Conjuc.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        Conjuc.Border = iTextSharp.text.Rectangle.BOX;
                                        Conjuc.BackgroundColor = new BaseColor(224, 224, 224);
                                        Groups.AddCell(Conjuc);
                                        if (EligQuees.EligFileName.Trim() == "CASESNP")
                                        {
                                            DataSet dsSnp = DatabaseLayer.CaseSnpData.GetCaseSnpDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, null);
                                            DataTable dtSnp = dsSnp.Tables[0];


                                            PdfPCell Name = new PdfPCell();
                                            Name.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Name.Rowspan = dtSnp.Rows.Count;
                                            Name.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                            string TempField = null;
                                            PdfPCell Field = new PdfPCell();
                                            Field.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Field.Rowspan = dtSnp.Rows.Count;
                                            //Field.Colspan = 3;
                                            Field.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                            foreach (DataRow drSnp in dtSnp.Rows)
                                            {
                                                if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "A")
                                                {
                                                    foreach (DataRow drCaseMSt in datMst.Rows)
                                                    {
                                                        if (drSnp["SNP_FAMILY_SEQ"].ToString().Trim() == drCaseMSt["MST_FAMILY_SEQ"].ToString().Trim())
                                                        {
                                                            string ApplicantName = LookupDataAccess.GetMemberName(drSnp["SNP_NAME_IX_FI"].ToString(), drSnp["SNP_NAME_IX_MI"].ToString(), drSnp["SNP_NAME_IX_LAST"].ToString(), strNameFormat);
                                                            string FliedValue = drSnp[EligQuees.EligFldName.Trim()].ToString();
                                                            Name = new PdfPCell(new Phrase(ApplicantName, fc));
                                                            Name.Border = iTextSharp.text.Rectangle.BOX;
                                                            Groups.AddCell(Name);
                                                            if (!string.IsNullOrEmpty(EligQuees.EligAgyCode.ToString()))
                                                            {
                                                                //DataSet dsAgyValues = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(EligQuees.EligAgyCode.ToString());
                                                                DataSet dsAgyValues = DatabaseLayer.CaseSum.GetAGYTABS(EligQuees.EligAgyCode.ToString());
                                                                DataTable dtAgyValues = dsAgyValues.Tables[0];
                                                                string Priv_tempField = null; string AgyDesc = null;
                                                                foreach (DataRow drAgyValues in dtAgyValues.Rows)
                                                                {
                                                                    if (!string.IsNullOrWhiteSpace(FliedValue.ToString()))
                                                                    {
                                                                        if (drAgyValues["AGYS_CODE"].ToString().Trim().ToUpper() == FliedValue.ToString().Trim().ToUpper())//drAgyValues["Code"].ToString().Trim()
                                                                        {
                                                                            AgyDesc = " ";
                                                                            if (EligQuees.EligQuesCode == "I00002" && drSnp["SNP_DOB_NA"].ToString() == "1")
                                                                                AgyDesc = "N/A";
                                                                            else
                                                                                AgyDesc = drAgyValues["AGYS_DESC"].ToString();//drAgyValues["LookUpDesc"].ToString()
                                                                            Field = new PdfPCell(new Phrase(AgyDesc, fc));
                                                                            Field.Colspan = 3;
                                                                            Field.Border = PdfPCell.BOX;
                                                                            Groups.AddCell(Field);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        //Priv_tempField = null;
                                                                        if (EligQuees.EligQuesCode == "I00002" && drSnp["SNP_DOB_NA"].ToString() == "1")
                                                                            AgyDesc = "N/A";
                                                                        else if (EligQuees.EligQuesCode == "I00002")
                                                                            AgyDesc = "0";
                                                                        else
                                                                            AgyDesc = "  ";
                                                                        if (Priv_tempField != AgyDesc)
                                                                        {
                                                                            Field = new PdfPCell(new Phrase(AgyDesc, fc));
                                                                            Field.Colspan = 3;
                                                                            Field.Border = PdfPCell.BOX;
                                                                            Groups.AddCell(Field);
                                                                            Priv_tempField = AgyDesc;
                                                                        }
                                                                    }
                                                                }
                                                                if (string.IsNullOrEmpty(AgyDesc))
                                                                {
                                                                    AgyDesc = " ";
                                                                    Field = new PdfPCell(new Phrase(AgyDesc, fc));
                                                                    Field.Colspan = 3;
                                                                    Field.Border = PdfPCell.BOX;
                                                                    Groups.AddCell(Field);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (string.IsNullOrWhiteSpace(FliedValue.ToString()))
                                                                {
                                                                    //Priv_tempField = null;
                                                                    if (EligQuees.EligQuesCode == "I00002" && drSnp["SNP_DOB_NA"].ToString() == "1")
                                                                        TempField = "N/A";
                                                                    else if (EligQuees.EligQuesCode == "I00002")
                                                                        FliedValue = "0";
                                                                }
                                                                else
                                                                {
                                                                    if (EligQuees.EligQuesCode == "I00002" && drSnp["SNP_DOB_NA"].ToString() == "1")
                                                                        FliedValue = "N/A";
                                                                }
                                                                Field = new PdfPCell(new Phrase(FliedValue, fc));
                                                                Field.Colspan = 3;
                                                                Field.Border = iTextSharp.text.Rectangle.BOX;
                                                                Groups.AddCell(Field);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    string ApplicantName = LookupDataAccess.GetMemberName(drSnp["SNP_NAME_IX_FI"].ToString(), drSnp["SNP_NAME_IX_MI"].ToString(), drSnp["SNP_NAME_IX_LAST"].ToString(), strNameFormat);
                                                    string FliedValue = drSnp[EligQuees.EligFldName.Trim()].ToString();
                                                    Name = new PdfPCell(new Phrase(ApplicantName, fc));
                                                    Name.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                    Groups.AddCell(Name);

                                                    if (!string.IsNullOrEmpty(EligQuees.EligAgyCode.ToString()))
                                                    {
                                                        //DataSet dsAgyValues = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(EligQuees.EligAgyCode.ToString());
                                                        DataSet dsAgyValues = DatabaseLayer.CaseSum.GetAGYTABS(EligQuees.EligAgyCode.ToString());
                                                        DataTable dtAgyValues = dsAgyValues.Tables[0];
                                                        string Priv_tempField = null; string AgyDesc = null;
                                                        foreach (DataRow drAgyValues in dtAgyValues.Rows)
                                                        {
                                                            if (!string.IsNullOrWhiteSpace(FliedValue.ToString()))
                                                            {
                                                                if (drAgyValues["AGYS_CODE"].ToString().Trim().ToUpper() == FliedValue.ToString().Trim().ToUpper())//drAgyValues["Code"].ToString().Trim()
                                                                {
                                                                    AgyDesc = drAgyValues["AGYS_DESC"].ToString();//drAgyValues["LookUpDesc"].ToString()
                                                                    Field = new PdfPCell(new Phrase(AgyDesc, fc));
                                                                    Field.Colspan = 3;
                                                                    Field.Border = PdfPCell.PARAGRAPH;
                                                                    Groups.AddCell(Field);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //Priv_tempField = null;
                                                                if (EligQuees.EligQuesCode == "I00002" && drSnp["SNP_DOB_NA"].ToString() == "1")
                                                                    AgyDesc = "N/A";
                                                                else if (EligQuees.EligQuesCode == "I00002")
                                                                    AgyDesc = "0";
                                                                else
                                                                    AgyDesc = "  ";
                                                                if (Priv_tempField != AgyDesc)
                                                                {
                                                                    Field = new PdfPCell(new Phrase(AgyDesc, fc));
                                                                    Field.Colspan = 3;
                                                                    Field.Border = PdfPCell.PARAGRAPH;
                                                                    Groups.AddCell(Field);
                                                                    Priv_tempField = AgyDesc;
                                                                }
                                                            }
                                                        }
                                                        if (string.IsNullOrEmpty(AgyDesc))
                                                        {
                                                            Field = new PdfPCell(new Phrase(AgyDesc, fc));
                                                            Field.Colspan = 3;
                                                            Field.Border = PdfPCell.PARAGRAPH;
                                                            Groups.AddCell(Field);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (string.IsNullOrWhiteSpace(FliedValue.ToString()))
                                                        {
                                                            if (EligQuees.EligQuesCode == "I00002" && drSnp["SNP_DOB_NA"].ToString() == "1")
                                                                FliedValue = "N/A";
                                                            else if (EligQuees.EligQuesCode == "I00002")
                                                                FliedValue = "0";
                                                        }
                                                        else
                                                        {
                                                            if (EligQuees.EligQuesCode == "I00002" && drSnp["SNP_DOB_NA"].ToString() == "1")
                                                                FliedValue = "N/A";
                                                        }
                                                        Field = new PdfPCell(new Phrase(FliedValue, fc));
                                                        Field.Colspan = 3;
                                                        Field.Border = iTextSharp.text.Rectangle.PARAGRAPH;
                                                        Groups.AddCell(Field);
                                                    }
                                                }
                                            }
                                        }
                                        else if (EligQuees.EligFileName.Trim() == "CASEMST")
                                        {
                                            //DataSet dsMst = DatabaseLayer.CaseMst.GetCASEMSTadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
                                            //DataTable dtMst = dsMst.Tables[0];
                                            foreach (DataRow datarowMst in datMst.Rows)
                                            {
                                                string FliedValue = datarowMst[EligQuees.EligFldName.Trim()].ToString();

                                                if(Desc== "Federal OMB" && !string.IsNullOrEmpty(FliedValue.Trim()))
                                                {
                                                    if (BaseForm.BaseAgencyControlDetails.ACR_POV_WITH_DEC == "N")
                                                        FliedValue = Math.Abs((int)Convert.ToDecimal(FliedValue.ToString())).ToString();
                                                }

                                                //if (!string.IsNullOrEmpty(FliedValue.ToString()))
                                                //{
                                                if (!string.IsNullOrEmpty(EligQuees.EligAgyCode.ToString()))
                                                {
                                                    //DataSet dsAgyValues = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(EligQuees.EligAgyCode.ToString());
                                                    DataSet dsAgyValues = DatabaseLayer.CaseSum.GetAGYTABS(EligQuees.EligAgyCode.ToString());
                                                    DataTable dtAgyValues = dsAgyValues.Tables[0];
                                                    string Priv_tempField = null;
                                                    foreach (DataRow drAgyValues in dtAgyValues.Rows)
                                                    {
                                                        if (!string.IsNullOrWhiteSpace(FliedValue.ToString()))
                                                        {
                                                            if (drAgyValues["AGYS_CODE"].ToString().Trim().ToUpper() == FliedValue.ToString().Trim().ToUpper())//drAgyValues["Code"].ToString().Trim()
                                                            {
                                                                string AgyDesc = drAgyValues["AGYS_DESC"].ToString();//drAgyValues["LookUpDesc"].ToString()
                                                                PdfPCell Field = new PdfPCell(new Phrase(AgyDesc, fc));
                                                                Field.HorizontalAlignment = Element.ALIGN_CENTER;
                                                                Field.Rowspan = datMst.Rows.Count;
                                                                Field.Colspan = 4;
                                                                Field.Border = iTextSharp.text.Rectangle.BOX;
                                                                Groups.AddCell(Field);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //Priv_tempField = null;
                                                            //if (EligQuees.EligQuesCode == "I00002" && drSnp["SNP_DOB_NA"].ToString() == "1")
                                                            //    TempField = "N/A";
                                                            //else if (EligQuees.EligQuesCode == "I00002")
                                                            //    TempField = "0";
                                                            //else
                                                            //    TempField = "  ";
                                                            //if (Priv_tempField != TempField)
                                                            //{

                                                            //    FliedValue = "0";
                                                            //}
                                                            //else
                                                            //    FliedValue = "  ";
                                                            if (string.IsNullOrEmpty(Priv_tempField))
                                                            {
                                                                PdfPCell Field = new PdfPCell(new Phrase(" ", fc));//UnAnswered
                                                                Field.HorizontalAlignment = Element.ALIGN_CENTER;
                                                                Field.Rowspan = datMst.Rows.Count;
                                                                Field.Colspan = 4;
                                                                Field.Border = iTextSharp.text.Rectangle.BOX;
                                                                Groups.AddCell(Field);
                                                                Priv_tempField = "NULL";
                                                            }
                                                            //string TempField = "  ";
                                                            //if (TempField != Priv_tempField)
                                                            //{
                                                            //    Field = new PdfPCell(new Phrase(TempField, fc));
                                                            //    Field.Colspan = 3;
                                                            //    Field.Border = PdfPCell.BOX;
                                                            //    Groups.AddCell(Field);
                                                            //    Priv_tempField = TempField;
                                                            //}

                                                        }
                                                    }
                                                }

                                                //}
                                                else
                                                {
                                                    if (!string.IsNullOrEmpty(FliedValue.ToString()))
                                                    {
                                                        PdfPCell Field = new PdfPCell(new Phrase(FliedValue, fc));
                                                        Field.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        Field.Rowspan = datMst.Rows.Count;
                                                        Field.Colspan = 4;
                                                        Field.Border = iTextSharp.text.Rectangle.BOX;
                                                        Groups.AddCell(Field);
                                                    }
                                                    else
                                                    {
                                                        if (EligQuees.EligRespType.ToString() == "N")
                                                        {

                                                            FliedValue = "0";
                                                        }
                                                        else
                                                            FliedValue = "  ";
                                                        PdfPCell Field = new PdfPCell(new Phrase(FliedValue, fc));//UnAnswered
                                                        Field.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        Field.Rowspan = datMst.Rows.Count;
                                                        Field.Colspan = 4;
                                                        Field.Border = iTextSharp.text.Rectangle.BOX;
                                                        Groups.AddCell(Field);
                                                    }
                                                }
                                            }
                                        }
                                        //if (First)
                                        //{
                                        //    table.SpacingBefore = 30f;
                                        //    document.Add(table);                       //Header row
                                        //    table.DeleteBodyRows();
                                        //    First = false;
                                        //}

                                        document.Add(Groups);
                                        Groups.DeleteBodyRows();
                                        //break;
                                    }
                                }
                            }
                        }
                        //document.Add(table);


                    }
                    PdfPCell blank = new PdfPCell(new Phrase("", fc1));
                    blank.HorizontalAlignment = Element.ALIGN_LEFT;
                    blank.Border = PdfPCell.TOP_BORDER;
                    Groups.AddCell(blank);

                    PdfPCell Space4 = new PdfPCell(new Phrase(" ", fc1));
                    Space4.HorizontalAlignment = Element.ALIGN_LEFT;
                    Space4.Border = PdfPCell.TOP_BORDER;
                    Groups.AddCell(Space4);

                    PdfPCell Space5 = new PdfPCell(new Phrase(" ", fc1));
                    Space5.HorizontalAlignment = Element.ALIGN_LEFT;
                    Space5.Border = PdfPCell.TOP_BORDER;
                    Groups.AddCell(Space5);

                    PdfPCell Space6 = new PdfPCell(new Phrase(" ", fc1));
                    Space6.HorizontalAlignment = Element.ALIGN_LEFT;
                    Space6.Border = PdfPCell.TOP_BORDER;
                    Groups.AddCell(Space6);

                    document.Add(Groups);
                    Groups.DeleteBodyRows();
                }
                else
                {
                    DataSet dsProg = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(StrHieCode.Substring(0, 2).ToString(), StrHieCode.Substring(2, 2).ToString(), StrHieCode.Substring(4, 2).ToString());
                    Agency = (dsProg.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();

                    PdfPCell Program = new PdfPCell(new Phrase("Program", fc));
                    Program.HorizontalAlignment = Element.ALIGN_LEFT;
                    Program.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(Program);

                    PdfPCell ProgDesc = new PdfPCell(new Phrase(Agency + "(" + StrHieCode.Substring(0, 2).ToString() + "-" + StrHieCode.Substring(2, 2).ToString() + "-" + StrHieCode.Substring(4, 2).ToString() + ")", fc));
                    ProgDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                    ProgDesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(ProgDesc);
                    PrivAgency = Agency;

                    PdfPCell Appli = new PdfPCell(new Phrase("Applicant Name", fc));
                    Appli.HorizontalAlignment = Element.ALIGN_LEFT;
                    Appli.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(Appli);


                    PdfPCell AppName = new PdfPCell(new Phrase(BaseForm.BaseApplicationName, fc));
                    AppName.HorizontalAlignment = Element.ALIGN_LEFT;
                    AppName.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(AppName);
                    table.SpacingBefore = 30f;
                    document.Add(table);
                    //PrivAgency = Agency;
                    PdfContentByte cb = writer.DirectContent;
                    cb.BeginText();
                    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 18);
                    cb.SetColorFill(BaseColor.RED);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Eligibility Criteria not defined", 300, 600, 0);
                    cb.EndText();



                }
            }
            else
            {
                PdfContentByte cb = writer.DirectContent;
                cb.BeginText();
                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 15);
                cb.SetColorFill(BaseColor.RED.Darker());
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Insufficient Arguments", 300, 700, 0);
                cb.EndText();
            }

            document.Close();
            fs.Close();
            fs.Dispose();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }
            //}
        }



        // End Report Section.................


        /// <summary>
        /// Case Income Report
        /// </summary>

        PdfContentByte cb;
        int X_Pos, Y_Pos;

        int pageNumber = 1;
        //private void On_SaveForm_Closed()
        //{
        //    Random_Filename = null;

        //    PdfName = "Income_Verification_Report";//form.GetFileName();
        //    //PdfName = strFolderPath + PdfName;
        //    PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //    try
        //    {
        //        if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //        { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFunctions.MessageBoxDisplay("Error");
        //    }

        //    try
        //    {
        //        string Tmpstr = PdfName + ".pdf";
        //        if (File.Exists(Tmpstr))
        //            File.Delete(Tmpstr);
        //    }
        //    catch (Exception ex)
        //    {
        //        int length = 8;
        //        string newFileName = System.Guid.NewGuid().ToString();
        //        newFileName = newFileName.Replace("-", string.Empty);

        //        Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
        //    }

        //    if (!string.IsNullOrEmpty(Random_Filename))
        //        PdfName = Random_Filename;
        //    else
        //        PdfName += ".pdf";

        //    FileStream fs = new FileStream(PdfName, FileMode.Create);

        //    Document document = new Document();
        //    document.SetPageSize(iTextSharp.text.PageSize.A4);
        //    PdfWriter writer = PdfWriter.GetInstance(document, fs);
        //    document.Open();
        //    cb = writer.DirectContent;
        //    //string Priv_Scr = null;
        //    //document = new Document(iTextSharp.text.PageSize.A4.Rotate());




        //    BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        //    iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
        //    BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
        //    iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(1, 9, 4);
        //    BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

        //    iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 11);
        //    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 10);
        //    iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
        //    iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 10, 1);
        //    iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
        //    iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
        //    iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());
        //    try
        //    {
        //        X_Pos = 30; Y_Pos = 760;
        //        cb.BeginText();
        //        cb.SetFontAndSize(bf_helv, 16);
        //        //cb.SetColorFill(BaseColor.BLUE.Darker());
        //        //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Head Start Eligibility Verification", 300, 800, 0);
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_CENTER, new Phrase("Head Start Eligibility Verification", TblFontBoldColor), 300, Y_Pos, 0);
        //        //cb.SetColorFill(BaseColor.BLACK.Brighter());
        //        cb.EndText();

        //        X_Pos = 30; Y_Pos -= 80;
        //        //cb.BeginText();
        //        //cb.SetFontAndSize(bf_helv, 12);
        //        //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "1. Child Name: ", X_Pos, Y_Pos, 0);
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("1. Child Name: ", Times), X_Pos, Y_Pos, 0);
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, TableFontBoldItalicUnderline), X_Pos + 75, Y_Pos, 0);


        //        int FSeq = int.Parse(BaseForm.BaseCaseMstListEntity[0].FamilySeq.Trim());
        //        CaseSnpEntity SnpApp = caseSnpList.Find(u => u.FamilySeq.Trim().Equals(FSeq.ToString()) && u.App.Equals(BaseForm.BaseApplicationNo.Trim()));
        //        Y_Pos -= 20;
        //        //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "2. Child Date of Birth: ", X_Pos, Y_Pos, 0);
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("2. Child Date of Birth: ", Times), X_Pos, Y_Pos, 0);
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(SnpApp.AltBdate.Trim()), TableFontBoldItalicUnderline), X_Pos + 105, Y_Pos, 0);



        //        Y_Pos -= 20;
        //        //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "3. Child date of entry into Program: ", X_Pos, Y_Pos, 0);
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("3. Child date of entry into Program: ", Times), X_Pos, Y_Pos, 0);
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(BaseForm.BaseCaseMstListEntity[0].IntakeDate.Trim()), TableFontBoldItalicUnderline), X_Pos + 163, Y_Pos, 0);

        //        Y_Pos -= 20;
        //        //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "4. Verify Eligibility, Check which category of eligibility does this child fall into", X_Pos, Y_Pos, 0);
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("4. Verify Eligibility, Check which category of eligibility does this child fall into", Times), X_Pos, Y_Pos, 0);

        //        //cb.EndText();
        //        Y_Pos -= 20;
        //        /************************************CheckBoxes****************************/
        //        iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(806, 40, 788, 60);
        //        PdfAppearance[] onOff = new PdfAppearance[2];
        //        onOff[0] = cb.CreateAppearance(20, 20);
        //        onOff[0].Rectangle(1, 20, 1, 20);
        //        onOff[0].Rectangle(18, 18, 1, 1);
        //        onOff[0].Stroke();
        //        onOff[1] = cb.CreateAppearance(20, 20);
        //        onOff[1].SetRGBColorFill(255, 128, 128);
        //        onOff[1].Rectangle(18, 18, 1, 1);
        //        onOff[1].FillStroke();
        //        onOff[1].MoveTo(1, 1);
        //        onOff[1].LineTo(19, 19);
        //        onOff[1].MoveTo(1, 19);
        //        onOff[1].LineTo(19, 1);

        //        RadioCheckField checkbox;
        //        PdfFormField SField;

        //        X_Pos = 45;
        //        rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
        //        //rect.Rotate();
        //        checkbox = new RadioCheckField(writer, rect, "Income", "On");
        //        //checkbox.Rotation = 90;
        //        checkbox.BorderColor = BaseColor.BLACK;
        //        SField = checkbox.CheckField;
        //        writer.AddAnnotation(SField);
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Income", Times), X_Pos + 14, Y_Pos, 0);

        //        X_Pos = 60;
        //        Y_Pos -= 20;
        //        rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
        //        //rect.Rotate();
        //        checkbox = new RadioCheckField(writer, rect, "41", "On");
        //        //checkbox.Rotation = 90;
        //        checkbox.BorderColor = BaseColor.BLACK;
        //        SField = checkbox.CheckField;
        //        writer.AddAnnotation(SField);
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Below federal poverty guidelines", Times), X_Pos + 14, Y_Pos, 0);

        //        Y_Pos -= 20;
        //        rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
        //        //rect.Rotate();
        //        checkbox = new RadioCheckField(writer, rect, "42", "On");
        //        //checkbox.Rotation = 90;
        //        checkbox.BorderColor = BaseColor.BLACK;
        //        SField = checkbox.CheckField;
        //        writer.AddAnnotation(SField);
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Between 100-130% federal poverty guidelines", Times), X_Pos + 14, Y_Pos, 0);
        //        Y_Pos -= 10;
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("(no more than 35% of enrolled children may fall into this category)", Times), X_Pos + 14, Y_Pos, 0);

        //        Y_Pos -= 20;
        //        rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
        //        //rect.Rotate();
        //        checkbox = new RadioCheckField(writer, rect, "43", "On");
        //        //checkbox.Rotation = 90;
        //        checkbox.BorderColor = BaseColor.BLACK;
        //        SField = checkbox.CheckField;
        //        writer.AddAnnotation(SField);
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Over Income (counted as part of 10% maximum for non-AI/AN programs)", Times), X_Pos + 14, Y_Pos, 0);

        //        Y_Pos -= 20;
        //        rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
        //        //rect.Rotate();
        //        checkbox = new RadioCheckField(writer, rect, "44", "On");
        //        //checkbox.Rotation = 90;
        //        checkbox.BorderColor = BaseColor.BLACK;
        //        SField = checkbox.CheckField;
        //        writer.AddAnnotation(SField);
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("AIAN Over Income (counted as part of 49% maximum for AI/AN programs)", Times), X_Pos + 14, Y_Pos, 0);

        //        X_Pos = 45;
        //        Y_Pos -= 20;
        //        rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
        //        //rect.Rotate();
        //        checkbox = new RadioCheckField(writer, rect, "Public Assistance", "On");
        //        //checkbox.Rotation = 90;
        //        checkbox.BorderColor = BaseColor.BLACK;
        //        SField = checkbox.CheckField;
        //        writer.AddAnnotation(SField);
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Public Assistance", Times), X_Pos + 14, Y_Pos, 0);


        //        Y_Pos -= 20;
        //        rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
        //        //rect.Rotate();
        //        checkbox = new RadioCheckField(writer, rect, "Homeless", "On");
        //        //checkbox.Rotation = 90;
        //        checkbox.BorderColor = BaseColor.BLACK;
        //        SField = checkbox.CheckField;
        //        writer.AddAnnotation(SField);
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Homeless", Times), X_Pos + 14, Y_Pos, 0);

        //        Y_Pos -= 20;
        //        rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
        //        //rect.Rotate();
        //        checkbox = new RadioCheckField(writer, rect, "Foster Care", "On");
        //        //checkbox.Rotation = 90;
        //        checkbox.BorderColor = BaseColor.BLACK;
        //        SField = checkbox.CheckField;
        //        writer.AddAnnotation(SField);
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Foster Care", Times), X_Pos + 14, Y_Pos, 0);

        //        X_Pos = 30; Y_Pos -= 25;
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("5. What documentation was used determine eligibility?", Times), X_Pos, Y_Pos, 0);
        //        Y_Pos -= 20; X_Pos = 60;
        //        rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
        //        //rect.Rotate();
        //        checkbox = new RadioCheckField(writer, rect, "51", "On");
        //        //checkbox.Rotation = 90;
        //        checkbox.BorderColor = BaseColor.BLACK;
        //        SField = checkbox.CheckField;
        //        writer.AddAnnotation(SField);
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Income Tax Form 1040", Times), X_Pos + 14, Y_Pos, 0);
        //        X_Pos = 285;
        //        rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
        //        //rect.Rotate();
        //        checkbox = new RadioCheckField(writer, rect, "52", "On");
        //        //checkbox.Rotation = 90;
        //        checkbox.BorderColor = BaseColor.BLACK;
        //        SField = checkbox.CheckField;
        //        writer.AddAnnotation(SField);
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Written statements from employers", Times), X_Pos + 14, Y_Pos, 0);

        //        Y_Pos -= 20; X_Pos = 60;
        //        rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
        //        //rect.Rotate();
        //        checkbox = new RadioCheckField(writer, rect, "53", "On");
        //        //checkbox.Rotation = 90;
        //        checkbox.BorderColor = BaseColor.BLACK;
        //        SField = checkbox.CheckField;
        //        writer.AddAnnotation(SField);
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("W-2", Times), X_Pos + 14, Y_Pos, 0);
        //        X_Pos = 285;
        //        rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
        //        //rect.Rotate();
        //        checkbox = new RadioCheckField(writer, rect, "54", "On");
        //        //checkbox.Rotation = 90;
        //        checkbox.BorderColor = BaseColor.BLACK;
        //        SField = checkbox.CheckField;
        //        writer.AddAnnotation(SField);
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Foster care reimbursement", Times), X_Pos + 14, Y_Pos, 0);

        //        Y_Pos -= 20; X_Pos = 60;
        //        rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
        //        //rect.Rotate();
        //        checkbox = new RadioCheckField(writer, rect, "55", "On");
        //        //checkbox.Rotation = 90;
        //        checkbox.BorderColor = BaseColor.BLACK;
        //        SField = checkbox.CheckField;
        //        writer.AddAnnotation(SField);
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("TANF Documentation", Times), X_Pos + 14, Y_Pos, 0);
        //        X_Pos = 285;
        //        rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
        //        //rect.Rotate();
        //        checkbox = new RadioCheckField(writer, rect, "56", "On");
        //        //checkbox.Rotation = 90;
        //        checkbox.BorderColor = BaseColor.BLACK;
        //        SField = checkbox.CheckField;
        //        writer.AddAnnotation(SField);
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("SSI Documentation", Times), X_Pos + 14, Y_Pos, 0);

        //        Y_Pos -= 20; X_Pos = 60;
        //        rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
        //        //rect.Rotate();
        //        checkbox = new RadioCheckField(writer, rect, "57", "On");
        //        //checkbox.Rotation = 90;
        //        checkbox.BorderColor = BaseColor.BLACK;
        //        SField = checkbox.CheckField;
        //        writer.AddAnnotation(SField);
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Pay stub or pay envelopes", Times), X_Pos + 14, Y_Pos, 0);
        //        X_Pos = 285;
        //        rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
        //        //rect.Rotate();
        //        checkbox = new RadioCheckField(writer, rect, "58", "On");
        //        //checkbox.Rotation = 90;
        //        checkbox.BorderColor = BaseColor.BLACK;
        //        SField = checkbox.CheckField;
        //        writer.AddAnnotation(SField);
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Other", Times), X_Pos + 14, Y_Pos, 0);

        //        Y_Pos -= 20; X_Pos = 60;
        //        rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
        //        //rect.Rotate();
        //        checkbox = new RadioCheckField(writer, rect, "59", "On");
        //        //checkbox.Rotation = 90;
        //        checkbox.BorderColor = BaseColor.BLACK;
        //        SField = checkbox.CheckField;
        //        writer.AddAnnotation(SField);
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Unemployment", Times), X_Pos + 14, Y_Pos, 0);

        //        X_Pos = 285;
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("If Other, please explain ____________________________", Times), X_Pos + 14, Y_Pos, 0);

        //        Y_Pos -= 20; X_Pos = 60;
        //        rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
        //        //rect.Rotate();
        //        checkbox = new RadioCheckField(writer, rect, "510", "On");
        //        //checkbox.Rotation = 90;
        //        checkbox.BorderColor = BaseColor.BLACK;
        //        SField = checkbox.CheckField;
        //        writer.AddAnnotation(SField);
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Documentation of no income: ________________________________________________________________", Times), X_Pos + 14, Y_Pos, 0);

        //        X_Pos = 30; Y_Pos -= 40;
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("6. Staff Signature _________________________________", Times), X_Pos, Y_Pos, 0);
        //        X_Pos = 285;
        //        ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Date: ___/___/______", Times), X_Pos + 14, Y_Pos, 0);

        //        string Worker = string.Empty;
        //        if (hierarchyEntity.Count > 0)
        //        {
        //            foreach (HierarchyEntity caseworker in hierarchyEntity)
        //            {
        //                if (BaseForm.BaseCaseMstListEntity[0].Verifier.ToString().Trim() == caseworker.CaseWorker.Trim())
        //                {
        //                    Worker = caseworker.HirarchyName.Trim(); break;
        //                }
        //            }

        //            X_Pos = 30; Y_Pos -= 40;
        //            ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("7. Staff Name : " + Worker, Times), X_Pos, Y_Pos, 0);
        //            X_Pos = 285;
        //            ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Title: ___________________________________________", Times), X_Pos + 14, Y_Pos, 0);
        //        }

        //    }
        //    catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
        //    document.Close();
        //    fs.Close();
        //    fs.Dispose();

        //    if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
        //    {
        //        PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
        //        objfrm.StartPosition = FormStartPosition.CenterScreen;
        //        objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }
        //    else
        //    {
        //        FrmViewer objfrm = new FrmViewer(PdfName);
        //        objfrm.StartPosition = FormStartPosition.CenterScreen;
        //        objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }

        //}

        private void On_SaveForm_Closed()
        {
            Random_Filename = null;

            PdfName = "Income_Verification_Report";//form.GetFileName();
            //PdfName = strFolderPath + PdfName;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
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

            Document document = new Document();
            document.SetPageSize(iTextSharp.text.PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            cb = writer.DirectContent;
            //string Priv_Scr = null;
            //document = new Document(iTextSharp.text.PageSize.A4.Rotate());




            //BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont bf_times = BaseFont.CreateFont(Application.MapPath("~\\Fonts\\OpenSans-Regular.TTF"), BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TimesSmall = new iTextSharp.text.Font(bf_times, 9);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 10, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());
            try
            {
                X_Pos = 30; Y_Pos = 760;
                //cb.BeginText();
                cb.SetFontAndSize(bf_helv, 16);
                //cb.SetColorFill(BaseColor.BLUE.Darker());
                //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Head Start Eligibility Verification", 300, 800, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_CENTER, new Phrase("Head Start Eligibility Verification", TblFontBoldColor), 300, Y_Pos, 0);
                //cb.SetColorFill(BaseColor.BLACK.Brighter());
                //cb.EndText();

                X_Pos = 30; Y_Pos -= 80;
                //cb.BeginText();
                //cb.SetFontAndSize(bf_helv, 12);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "1. Child Name: ", X_Pos, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("1. Child Name: ", Times), X_Pos, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, TableFontBoldItalicUnderline), X_Pos + 75, Y_Pos, 0);


                int FSeq = int.Parse(BaseForm.BaseCaseMstListEntity[0].FamilySeq.Trim());
                CaseSnpEntity SnpApp = caseSnpList.Find(u => u.FamilySeq.Trim().Equals(FSeq.ToString()) && u.App.Equals(BaseForm.BaseApplicationNo.Trim()));
                Y_Pos -= 20;
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "2. Child Date of Birth: ", X_Pos, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("2. Child Date of Birth: ", Times), X_Pos, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(SnpApp.AltBdate.Trim()), TableFontBoldItalicUnderline), X_Pos + 105, Y_Pos, 0);



                Y_Pos -= 20;
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "3. Child date of entry into Program: ", X_Pos, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("3. Child date of entry into Program: ", Times), X_Pos, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(BaseForm.BaseCaseMstListEntity[0].IntakeDate.Trim()), TableFontBoldItalicUnderline), X_Pos + 165, Y_Pos, 0);

                Y_Pos -= 20;
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "4. Verify Eligibility, Check which category of eligibility does this child fall into", X_Pos, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("4. Verify Eligibility, Check which category of eligibility does this child fall into", Times), X_Pos, Y_Pos, 0);

                //cb.EndText();
                Y_Pos -= 20;
                /************************************CheckBoxes****************************/
                iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(806, 40, 788, 60);
                PdfAppearance[] onOff = new PdfAppearance[2];
                onOff[0] = cb.CreateAppearance(20, 20);
                onOff[0].Rectangle(1, 20, 1, 20);
                onOff[0].Rectangle(18, 18, 1, 1);
                onOff[0].Stroke();
                onOff[1] = cb.CreateAppearance(20, 20);
                onOff[1].SetRGBColorFill(255, 128, 128);
                onOff[1].Rectangle(18, 18, 1, 1);
                onOff[1].FillStroke();
                onOff[1].MoveTo(1, 1);
                onOff[1].LineTo(19, 19);
                onOff[1].MoveTo(1, 19);
                onOff[1].LineTo(19, 1);

                RadioCheckField checkbox;
                PdfFormField SField;

                X_Pos = 45;
                rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //rect.Rotate();
                checkbox = new RadioCheckField(writer, rect, "Income", "Yes");
                //checkbox.Rotation = 90;
                checkbox.BorderColor = BaseColor.BLACK;
                //checkbox.Checked = true;
                checkbox.CheckType = RadioCheckField.TYPE_CHECK;
                SField = checkbox.CheckField;
                writer.AddAnnotation(SField);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Income", Times), X_Pos + 14, Y_Pos, 0);

                X_Pos = 80;
                Y_Pos -= 20;
                rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //rect.Rotate();
                checkbox = new RadioCheckField(writer, rect, "41", "Yes");
                //checkbox.Rotation = 90;
                checkbox.BorderColor = BaseColor.BLACK;
                checkbox.CheckType = RadioCheckField.TYPE_CHECK;
                SField = checkbox.CheckField;
                writer.AddAnnotation(SField);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Below federal poverty guidelines", Times), X_Pos + 14, Y_Pos, 0);

                Y_Pos -= 20;
                rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //rect.Rotate();
                checkbox = new RadioCheckField(writer, rect, "42", "Yes");
                //checkbox.Rotation = 90;
                checkbox.BorderColor = BaseColor.BLACK;
                checkbox.CheckType = RadioCheckField.TYPE_CHECK;
                SField = checkbox.CheckField;
                writer.AddAnnotation(SField);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Between 100-130% federal poverty guidelines", Times), X_Pos + 14, Y_Pos, 0);
                Y_Pos -= 13;
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("(no more than 35% of enrolled children may fall into this category)", TimesSmall), X_Pos + 14, Y_Pos, 0);

                Y_Pos -= 20;
                rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //rect.Rotate();
                checkbox = new RadioCheckField(writer, rect, "43", "Yes");
                //checkbox.Rotation = 90;
                checkbox.BorderColor = BaseColor.BLACK;
                checkbox.CheckType = RadioCheckField.TYPE_CHECK;
                SField = checkbox.CheckField;
                writer.AddAnnotation(SField);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Over Income (counted as part of 10% maximum for non-AI/AN programs)", Times), X_Pos + 14, Y_Pos, 0);

                Y_Pos -= 20;
                rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //rect.Rotate();
                checkbox = new RadioCheckField(writer, rect, "44", "Yes");
                //checkbox.Rotation = 90;
                checkbox.BorderColor = BaseColor.BLACK;
                checkbox.CheckType = RadioCheckField.TYPE_CHECK;
                SField = checkbox.CheckField;
                writer.AddAnnotation(SField);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("AIAN Over Income (counted as part of 49% maximum for AI/AN programs)", Times), X_Pos + 14, Y_Pos, 0);

                X_Pos = 45;
                Y_Pos -= 20;
                rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //rect.Rotate();
                checkbox = new RadioCheckField(writer, rect, "Public Assistance", "Yes");
                //checkbox.Rotation = 90;
                checkbox.BorderColor = BaseColor.BLACK;
                checkbox.CheckType = RadioCheckField.TYPE_CHECK;
                SField = checkbox.CheckField;
                writer.AddAnnotation(SField);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Public Assistance", Times), X_Pos + 14, Y_Pos, 0);


                Y_Pos -= 20;
                rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //rect.Rotate();
                checkbox = new RadioCheckField(writer, rect, "Homeless", "Yes");
                //checkbox.Rotation = 90;
                checkbox.BorderColor = BaseColor.BLACK;
                checkbox.CheckType = RadioCheckField.TYPE_CHECK;
                SField = checkbox.CheckField;
                writer.AddAnnotation(SField);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Homeless", Times), X_Pos + 14, Y_Pos, 0);

                Y_Pos -= 20;
                rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //rect.Rotate();
                checkbox = new RadioCheckField(writer, rect, "Foster Care", "Yes");
                //checkbox.Rotation = 90;
                checkbox.BorderColor = BaseColor.BLACK;
                checkbox.CheckType = RadioCheckField.TYPE_CHECK;
                SField = checkbox.CheckField;
                writer.AddAnnotation(SField);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Foster Care", Times), X_Pos + 14, Y_Pos, 0);

                X_Pos = 30; Y_Pos -= 25;
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("5. What documentation was used determine eligibility?", Times), X_Pos, Y_Pos, 0);
                Y_Pos -= 20; X_Pos = 60;
                rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //rect.Rotate();
                checkbox = new RadioCheckField(writer, rect, "51", "Yes");
                //checkbox.Rotation = 90;
                checkbox.BorderColor = BaseColor.BLACK;
                checkbox.CheckType = RadioCheckField.TYPE_CHECK;
                SField = checkbox.CheckField;
                writer.AddAnnotation(SField);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Income Tax Form 1040", Times), X_Pos + 14, Y_Pos, 0);
                X_Pos = 285;
                rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //rect.Rotate();
                checkbox = new RadioCheckField(writer, rect, "52", "Yes");
                //checkbox.Rotation = 90;
                checkbox.BorderColor = BaseColor.BLACK;
                checkbox.CheckType = RadioCheckField.TYPE_CHECK;
                SField = checkbox.CheckField;
                writer.AddAnnotation(SField);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Written statements from employers", Times), X_Pos + 14, Y_Pos, 0);

                Y_Pos -= 20; X_Pos = 60;
                rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //rect.Rotate();
                checkbox = new RadioCheckField(writer, rect, "53", "Yes");
                //checkbox.Rotation = 90;
                checkbox.BorderColor = BaseColor.BLACK;
                checkbox.CheckType = RadioCheckField.TYPE_CHECK;
                SField = checkbox.CheckField;
                writer.AddAnnotation(SField);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("W-2", Times), X_Pos + 14, Y_Pos, 0);
                X_Pos = 285;
                rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //rect.Rotate();
                checkbox = new RadioCheckField(writer, rect, "54", "Yes");
                //checkbox.Rotation = 90;
                checkbox.BorderColor = BaseColor.BLACK;
                checkbox.CheckType = RadioCheckField.TYPE_CHECK;
                SField = checkbox.CheckField;
                writer.AddAnnotation(SField);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Foster care reimbursement", Times), X_Pos + 14, Y_Pos, 0);

                Y_Pos -= 20; X_Pos = 60;
                rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //rect.Rotate();
                checkbox = new RadioCheckField(writer, rect, "55", "Yes");
                //checkbox.Rotation = 90;
                checkbox.BorderColor = BaseColor.BLACK;
                checkbox.CheckType = RadioCheckField.TYPE_CHECK;
                SField = checkbox.CheckField;
                writer.AddAnnotation(SField);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("TANF Documentation", Times), X_Pos + 14, Y_Pos, 0);
                X_Pos = 285;
                rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //rect.Rotate();
                checkbox = new RadioCheckField(writer, rect, "56", "Yes");
                //checkbox.Rotation = 90;
                checkbox.BorderColor = BaseColor.BLACK;
                checkbox.CheckType = RadioCheckField.TYPE_CHECK;
                SField = checkbox.CheckField;
                writer.AddAnnotation(SField);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("SSI Documentation", Times), X_Pos + 14, Y_Pos, 0);

                Y_Pos -= 20; X_Pos = 60;
                rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //rect.Rotate();
                checkbox = new RadioCheckField(writer, rect, "57", "Yes");
                //checkbox.Rotation = 90;
                checkbox.BorderColor = BaseColor.BLACK;
                checkbox.CheckType = RadioCheckField.TYPE_CHECK;
                SField = checkbox.CheckField;
                writer.AddAnnotation(SField);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Pay stub or pay envelopes", Times), X_Pos + 14, Y_Pos, 0);
                X_Pos = 285;
                rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //rect.Rotate();
                checkbox = new RadioCheckField(writer, rect, "58", "Yes");
                //checkbox.Rotation = 90;
                checkbox.BorderColor = BaseColor.BLACK;
                checkbox.CheckType = RadioCheckField.TYPE_CHECK;
                SField = checkbox.CheckField;
                writer.AddAnnotation(SField);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Unemployment", Times), X_Pos + 14, Y_Pos, 0);

                Y_Pos -= 20; X_Pos = 60;
                rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //rect.Rotate();
                checkbox = new RadioCheckField(writer, rect, "59", "Yes");
                //checkbox.Rotation = 90;
                checkbox.BorderColor = BaseColor.BLACK;
                checkbox.CheckType = RadioCheckField.TYPE_CHECK;
                SField = checkbox.CheckField;
                writer.AddAnnotation(SField);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Other, please explain _________________________________________________________________________", Times), X_Pos + 14, Y_Pos, 0);

                //X_Pos = 285;
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("If Other, please explain ____________________________", Times), X_Pos + 14, Y_Pos, 0);

                Y_Pos -= 20; X_Pos = 60;
                rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //rect.Rotate();
                checkbox = new RadioCheckField(writer, rect, "510", "Yes");
                //checkbox.Rotation = 90;
                checkbox.BorderColor = BaseColor.BLACK;
                checkbox.CheckType = RadioCheckField.TYPE_CHECK;
                SField = checkbox.CheckField;
                writer.AddAnnotation(SField);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Documentation of no income: ________________________________________________________________", Times), X_Pos + 14, Y_Pos, 0);

                X_Pos = 30; Y_Pos -= 70;
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("6. Staff Signature _________________________________", Times), X_Pos, Y_Pos, 0);
                X_Pos = 285;
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Date: ___/___/______", Times), X_Pos + 14, Y_Pos, 0);

                string Worker = string.Empty;
                if (hierarchyEntity.Count > 0)
                {
                    foreach (HierarchyEntity caseworker in hierarchyEntity)
                    {
                        if (BaseForm.BaseCaseMstListEntity[0].Verifier.ToString().Trim() == caseworker.CaseWorker.Trim())
                        {
                            Worker = caseworker.HirarchyName.Trim(); break;
                        }
                    }

                    X_Pos = 30; Y_Pos -= 40;
                    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("7. Staff Name : " + Worker, Times), X_Pos, Y_Pos, 0);
                    X_Pos = 285;
                    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Title: ___________________________________________", Times), X_Pos + 14, Y_Pos, 0);
                }

            }
            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
            document.Close();
            fs.Close();
            fs.Dispose();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }

        }


        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(PdfName);
        }

        private void PbEdit_Click(object sender, EventArgs e)
        {
            if (AdminControlValidation())
            {
                calVerificationDate.ValueChanged -= new EventHandler(calVerificationDate_ValueChanged);
                txtInprogram.TextChanged -= new EventHandler(txtInprogram_TextChanged);
                calVerificationDate.CheckedChanged -= new EventHandler(calVerificationDate_CheckedChanged);
                cmbVerifier.SelectedIndexChanged -= new EventHandler(cmbVerifier_SelectedIndexChanged);
                if (dataGridCaseIncomeVer.Rows.Count > 0)
                {
                    PbEdit.Visible = false;
                    PbDelete.Visible = false;
                    PbAdd.Visible = false;
                    dataGridCaseIncomeVer.Enabled = false;
                    CaseVerEntity row = dataGridCaseIncomeVer.SelectedRows[0].Tag as CaseVerEntity;
                    if (row != null)
                    {

                        CaseVerEntity caseVerList = _model.CaseMstData.GetCaseveradpynd(MainMenuAgency, MainMenuDept, MainMenuProgram, MainMenuYear, ApplicationNo, row.VerifyDate, string.Empty);
                        if (caseVerList != null)
                        {
                            calVerificationDate.Value = Convert.ToDateTime(caseVerList.VerifyDate);
                            calVerificationDate.Enabled = false;
                            calVerificationDate.Checked = true;
                            if (caseVerList.VerifyW2 == Consts.YesNoVariants.Y)
                                chkw2.Checked = true;
                            else
                                chkw2.Checked = false;

                            if (caseVerList.VerifyTaxReturn == Consts.YesNoVariants.Y)
                                chkTaxReturn.Checked = true;
                            else
                                chkTaxReturn.Checked = false;

                            if (caseVerList.VerifyLetter == Consts.YesNoVariants.Y)
                                chkLetter.Checked = true;
                            else
                                chkLetter.Checked = false;

                            if (caseVerList.VerifyOther == Consts.YesNoVariants.Y)
                                chkOther.Checked = true;
                            else
                                chkOther.Checked = false;

                            if (caseVerList.VerifyCheckStub == Consts.YesNoVariants.Y)
                                chkCheckStubs.Checked = true;
                            else
                                chkCheckStubs.Checked = false;

                            if (caseVerList.VerifySelfDecl == Consts.YesNoVariants.Y)
                                chk_self_Decl.Checked = true;
                            else
                                chk_self_Decl.Checked = false;

                            txtCmi.Text = caseVerList.VerCmi;
                            if (BaseForm.BaseAgencyControlDetails.ACR_POV_WITH_DEC == "Y")
                                txtFedOmb.Text = caseVerList.VerOmb;
                            else
                                txtFedOmb.Text = Math.Abs((int)decimal.Parse(caseVerList.VerOmb)).ToString();
                            txtHud.Text = caseVerList.VerHud;
                            txtIncome.Text = caseVerList.IncomeAmount;
                            txtSmi.Text = caseVerList.VerSmi;
                            txtInprogram.Text = caseVerList.NoInhh;
                            if (caseVerList.NoInhh != string.Empty)
                                intTotalinProgress = Convert.ToInt32(caseVerList.NoInhh);

                            if (caseVerList.ReverifyDate != "")
                            {
                                calReverficationdate.Value = Convert.ToDateTime(caseVerList.ReverifyDate);
                                calReverficationdate.Checked = true;
                            }
                            if (caseVerList.FundSource != string.Empty)
                                CommonFunctions.SetComboBoxValue(cmbFundingsource, caseVerList.FundSource);
                            else
                                CommonFunctions.SetComboBoxValue(cmbFundingsource, "0");
                            CommonFunctions.SetComboBoxValue(cmbCategorical, caseVerList.CatElig);
                            if (caseVerList.MealElig != string.Empty)
                                CommonFunctions.SetComboBoxValue(cmbMeal, caseVerList.MealElig);
                            else
                                CommonFunctions.SetComboBoxValue(cmbMeal, "0");
                            strCaseWorkerDefaultCode = caseVerList.Verifier;
                            if (strCaseWorkerDefaultCode != string.Empty || strCaseWorkerDefaultCode != "0")
                                CommonFunctions.SetComboBoxValue(cmbVerifier, strCaseWorkerDefaultCode);
                            else
                                CommonFunctions.SetComboBoxValue(cmbVerifier, strCaseWorkerDefaultStartCode);
                            ChangeStatus(caseVerList.Classification.ToString().Trim(), true);
                            strMode = Consts.Common.Edit;
                            strIndex = dataGridCaseIncomeVer.SelectedRows[0].Index;
                            if (privilege.ChangePriv.Equals("false"))
                            {
                                EnableAllcontrolsPrivileges(false);
                                btnSave.Visible = false;
                                btnCancel.Text = "Close";

                            }
                            else
                            {
                                EnableAllcontrolsPrivileges(true);
                                DisableControls();
                                EnableDisableControls();
                                Programdefinationcheck();
                                btnSave.Visible = true;
                                btnCancel.Text = "Cancel";
                                calVerificationDate.Enabled = false;
                            }

                            DateTime dtverifyDate = Convert.ToDateTime(caseVerList.VerifyDate);

                            if (dtverifyDate.Date == Convert.ToDateTime(VerifyCheckDate).Date)
                            {
                                if (privilege.ChangePriv.Equals("false"))
                                {
                                    btnSave.Visible = false;
                                }
                                else
                                {
                                    btnSave.Visible = true;
                                    //List<CaseSnpEntity> listCasesnp = caseSnpList.FindAll(u => u.ProgIncome != string.Empty);
                                    //decimal snppronginccome = listCasesnp.Sum(u => Convert.ToDecimal(u.ProgIncome));
                                    //txtIncome.Text = snppronginccome.ToString();//CaseMSTEntity.ProgIncome;
                                    //txtInprogram.Text = caseSnpList.Count(u => u.Exclude == "N").ToString();

                                }
                                txtInprogram.Enabled = true;
                            }
                            else
                            {
                                btnSave.Visible = false;
                                txtInprogram.Enabled = false;
                            }
                            if (Convert.ToDateTime(calVerificationDate.Value.Date) == DateTime.Now.Date)
                            {
                                if (caseSnpList.Count > 0)
                                {
                                    CalculateStatus = true;
                                    List<CaseSnpEntity> listCasesnp = caseSnpList.FindAll(u => u.ProgIncome != string.Empty);
                                    decimal snppronginccome = listCasesnp.Sum(u => Convert.ToDecimal(u.ProgIncome));
                                    txtIncome.Text = snppronginccome.ToString();//CaseMSTEntity.ProgIncome;
                                    txtInprogram.Text = caseSnpList.Count(u => u.Exclude == "N" && u.Status.Trim().ToUpper() != "I" && u.SsnReason != "Q").ToString();
                                    CalculateFedOmbEligData();
                                }
                            }


                        }
                        cmbVerifier.Focus();
                    }
                }
                cmbVerifier.SelectedIndexChanged += new EventHandler(cmbVerifier_SelectedIndexChanged);
                calVerificationDate.ValueChanged += new EventHandler(calVerificationDate_ValueChanged);
                calVerificationDate.CheckedChanged += new EventHandler(calVerificationDate_CheckedChanged);
                txtInprogram.TextChanged += new EventHandler(txtInprogram_TextChanged);
            }
        }

        private void PbAdd_Click(object sender, EventArgs e)
        {
            if (AdminControlValidation())
            {
                calVerificationDate.ValueChanged -= new EventHandler(calVerificationDate_ValueChanged);
                txtInprogram.TextChanged -= new EventHandler(txtInprogram_TextChanged);
                calVerificationDate.CheckedChanged -= new EventHandler(calVerificationDate_CheckedChanged);
                cmbVerifier.SelectedIndexChanged -= new EventHandler(cmbVerifier_SelectedIndexChanged);
                fillIncomeControl();
                ChangeStatus("NEW", false);
                strMode = Consts.Common.New;
                EnableAllcontrolsPrivileges(true);
                DisableControls();
                EnableDisableControls();
                Programdefinationcheck();
                CommonFunctions.SetComboBoxValue(cmbFundingsource, strFundDefaultCode);
                btnSave.Visible = true;
                btnCancel.Text = "Cancel";
                calVerificationDate.Value = DateTime.Now.Date;
                calReverficationdate.Value = DateTime.Now.Date;
                calVerificationDate.Checked = false;
                calVerificationDate.Focus();
                PbEdit.Visible = false;
                PbDelete.Visible = false;
                PbAdd.Visible = false;
                dataGridCaseIncomeVer.Enabled = false;
                cmbVerifier.SelectedIndexChanged += new EventHandler(cmbVerifier_SelectedIndexChanged);
                calVerificationDate.ValueChanged += new EventHandler(calVerificationDate_ValueChanged);
                calVerificationDate.CheckedChanged += new EventHandler(calVerificationDate_CheckedChanged);
                txtInprogram.TextChanged += new EventHandler(txtInprogram_TextChanged);
            }
        }

        private void PbDelete_Click(object sender, EventArgs e)
        {
            if (dataGridCaseIncomeVer.SelectedRows.Count > 0)
            {
                if (dataGridCaseIncomeVer.SelectedRows[0].Tag is CaseVerEntity)
                {
                    CaseVerEntity row = dataGridCaseIncomeVer.SelectedRows[0].Tag as CaseVerEntity;
                    if (row != null)
                    {
                        strIndex = dataGridCaseIncomeVer.SelectedRows[0].Index;
                        MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose:OnDeleteMessageBoxClicked);

                    }
                }
            }
        }

        private void PbIncome_Click(object sender, EventArgs e)
        {
            string ReaderName = string.Empty;

            if(AgencyControlDetails.AgyShortName=="ICAP" || AgencyControlDetails.AgyShortName == "FSCAA")
                ReaderName = propReportPath + "\\" + "Incver.pdf";
            else ReaderName = propReportPath + "\\" + "HS_Eligibility_Verification_Form.pdf";

            //if (privilege.ModuleCode == "02")
            //{
            //ReaderName = propReportPath + "\\" + "Incver.pdf";

            //if (File.Exists(ReaderName))
            //{ }
            //else
            //{
            //ReaderName = propReportPath + "\\" + "HS_Eligibility_Verification_Form.pdf";
            
            //    }
            //}


            if (File.Exists(ReaderName))
            {
                if (privilege.ModuleCode == "02" && ReaderName.Contains("HS_Eligibility_Verification_Form.pdf"))
                    On_SaveForm_Closed2();
                else if (ReaderName.Contains("Incver.pdf"))
                    On_SaveForm_Closed1();
                else
                    On_SaveForm_Closed();
            }
            else
                On_SaveForm_Closed();

        }

        # region  ValidationData

        public bool AdminControlValidation()
        {
            bool boolvalidation = true;
            bool boolAllowClientIntake = true;
            bool boolAllowCustom = true;
            bool boolAllowIncome = true;
            bool boolDisplayClientIntake = true;
            bool boolDisplayCustom = true;
            bool boolDisplayIncomeVer = true;
            bool boolDisplayCaseIncome = true;
            string strclientIntakeRequired = string.Empty;
            string strVerRequired = string.Empty;
            string strclientDisMsg = string.Empty;
            string strCustomDisMsg = string.Empty;
            string strVerDisMsg = string.Empty;
            string strIncomeDisMsg = string.Empty;
            string strMsg = string.Empty;
            if (BaseForm.BaseAgencyControlDetails.RomaSwitch.ToUpper() == "Y")
            {
                List<ScaFldsHieEntity> ScaFldsHiedata = _model.FieldControls.GETSCAFLDSHIEDATA("CASE2003", string.Empty, string.Empty);
                if (ScaFldsHiedata.Count > 0)
                {
                    List<ScaFldsHieEntity> ScaFldsHie = ScaFldsHiedata.FindAll(u => u.ScrHie == BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg);
                    if (ScaFldsHie.Count == 0)
                    {
                        ScaFldsHie = ScaFldsHiedata.FindAll(u => u.ScrHie == BaseForm.BaseAgency + BaseForm.BaseDept + "**");
                        if (ScaFldsHie.Count == 0)
                        {
                            ScaFldsHie = ScaFldsHiedata.FindAll(u => u.ScrHie == BaseForm.BaseAgency + "****");
                        }
                        if (ScaFldsHie.Count == 0)
                        {
                            ScaFldsHie = ScaFldsHiedata.FindAll(u => u.ScrHie == "******");
                        }
                    }
                    if (ScaFldsHie.Count > 0)
                    {
                        int intvalidcount = 0;
                        ScaFldsHieEntity ScaFldscase2001data = ScaFldsHie.Find(u => u.ScahCode == "S0001");
                        ScaFldsHieEntity ScaFldscustomdata = ScaFldsHie.Find(u => u.ScahCode == "S0002");
                        ScaFldsHieEntity ScaFldsIncomedata = ScaFldsHie.Find(u => u.ScahCode == "S0003");
                        CaseSnpEntity snpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq);
                        if (ScaFldscase2001data.Sel.ToUpper() == "Y")
                        {

                            boolAllowClientIntake = _model.CaseMstData.CheckRequiredCase2001ControlsData(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, BaseForm.BaseCaseSnpEntity, BaseForm.BaseCaseMstListEntity[0], out strclientIntakeRequired);
                            if (!boolAllowClientIntake)
                            {
                                boolDisplayClientIntake = false;
                                intvalidcount = +1;
                                strMsg = ScaFldscase2001data.Msg;
                                strclientDisMsg = ScaFldscase2001data.Msg;
                                if (ScaFldscase2001data.Active.ToUpper() == "Y")
                                    boolAllowClientIntake = true;

                            }
                        }
                        if (ScaFldscustomdata.Sel.ToUpper() == "Y")
                        {
                            boolAllowCustom = _model.CaseMstData.CheckRequiredCustomQuestionsData(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, snpdata);
                            if (!boolAllowCustom)
                            {
                                boolDisplayCustom = false;
                                intvalidcount = intvalidcount + 1;
                                strMsg = strMsg + "\n" + ScaFldscustomdata.Msg;
                                strCustomDisMsg = ScaFldscustomdata.Msg;
                                if (ScaFldscustomdata.Active.ToUpper() == "Y")
                                    boolAllowCustom = true;
                            }
                        }
                        if (ScaFldsIncomedata != null)
                        {
                            if (ScaFldsIncomedata.Sel.ToUpper() == "Y")
                            {
                                List<CaseIncomeEntity> caseincomelist = _model.CaseMstData.GetCaseIncomeadpynf(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);
                                if (caseincomelist.Count == 0)
                                {
                                    boolAllowIncome = false;
                                    boolDisplayCaseIncome = false;
                                    intvalidcount = intvalidcount + 1;
                                    strMsg = strMsg + "\n" + ScaFldsIncomedata.Msg;
                                    strIncomeDisMsg = ScaFldsIncomedata.Msg;
                                    if (ScaFldsIncomedata.Active.ToUpper() == "Y")
                                        boolAllowIncome = true;
                                }
                            }
                        }
                        if (boolAllowClientIntake == true && boolAllowCustom == true && boolAllowIncome == true)
                            boolvalidation = true;
                        else
                            boolvalidation = false;
                        if (intvalidcount > 0)
                        {
                            ScaFldsHieEntity ScaFldscaseGerdata = ScaFldsHie.Find(u => u.ScahCode == "S0004");
                            if (boolvalidation == false)
                            {
                                //if (ScaFldscaseGerdata != null)
                                //    strMsg = ScaFldscaseGerdata.Msg;
                                if (ScaFldscaseGerdata != null)
                                {
                                    if (ScaFldscaseGerdata.Sel.ToUpper() == "Y")
                                        strMsg = ScaFldscaseGerdata.Msg;
                                }
                            }
                            AdminControlMessageForm objForm = new AdminControlMessageForm(BaseForm, strMsg, strclientIntakeRequired, strVerRequired, boolDisplayClientIntake, boolDisplayCustom, boolDisplayIncomeVer, strclientDisMsg, strCustomDisMsg, strVerDisMsg, string.Empty, boolDisplayCaseIncome, strIncomeDisMsg);
                            objForm.StartPosition = FormStartPosition.CenterScreen;
                            objForm.ShowDialog();
                            // CommonFunctions.MessageBoxDisplay(strMsg);
                        }

                    }

                }

            }

            return boolvalidation;
        }

        #endregion


        public void InsertUpdateRngCounts()
        {
            RNGCOUNTSEnitity rngCountsData;
            foreach (CaseSnpEntity snpdataitem in BaseForm.BaseCaseSnpEntity)
            {
                rngCountsData = new RNGCOUNTSEnitity();
                rngCountsData.RNGC_AGENCY = snpdataitem.Agency;
                rngCountsData.RNGC_DEPT = snpdataitem.Dept;
                rngCountsData.RNGC_PROGRAM = snpdataitem.Program;
                rngCountsData.RNGC_YEAR = snpdataitem.Year;
                rngCountsData.RNGC_APP = snpdataitem.App;
                rngCountsData.RNGC_ELIG_DATE = calVerificationDate.Value.ToShortDateString();
                rngCountsData.RNGC_MST_FAMILY_SEQ = BaseForm.BaseCaseMstListEntity[0].FamilySeq;
                rngCountsData.RNGC_SNP_FAMILY_SEQ = snpdataitem.FamilySeq;
                rngCountsData.RNGC_FAMILY_ID = BaseForm.BaseCaseMstListEntity[0].FamilyId;
                rngCountsData.RNGC_MEMBER_CODE = snpdataitem.MemberCode;
                rngCountsData.RNGC_CLIENT_ID = snpdataitem.ClientId;
                rngCountsData.RNGC_NAME_IX_FI = snpdataitem.NameixFi;
                rngCountsData.RNGC_NAME_IX_LAST = snpdataitem.NameixLast;
                rngCountsData.RNGC_NAME_IX_MI = snpdataitem.NameixMi;
                rngCountsData.RNGC_FAMILY_TYPE = BaseForm.BaseCaseMstListEntity[0].FamilyType;
                rngCountsData.RNGC_NO_INPROG = BaseForm.BaseCaseMstListEntity[0].NoInProg;
                rngCountsData.RNGC_HOUSING = BaseForm.BaseCaseMstListEntity[0].Housing;
                rngCountsData.RNGC_POVERTY = BaseForm.BaseCaseMstListEntity[0].Poverty;
                rngCountsData.RNCG_COUNTY = BaseForm.BaseCaseMstListEntity[0].County;
                rngCountsData.RNGC_CASE_TYPE = BaseForm.BaseCaseMstListEntity[0].CaseType;
                rngCountsData.RNGC_ACTIVE_STATUS = BaseForm.BaseCaseMstListEntity[0].ActiveStatus;
                rngCountsData.RNGC_SITE = BaseForm.BaseCaseMstListEntity[0].Site;
                rngCountsData.RNGC_ZIP = BaseForm.BaseCaseMstListEntity[0].Zip;
                rngCountsData.RNGC_ZIPPLUS = BaseForm.BaseCaseMstListEntity[0].Zipplus;
                rngCountsData.RNGC_INTAKE_WORKER = BaseForm.BaseCaseMstListEntity[0].IntakeWorker;
                rngCountsData.RNGC_SECRET = BaseForm.BaseCaseMstListEntity[0].Secret;
                rngCountsData.RNGC_INCOME_TYPES = BaseForm.BaseCaseMstListEntity[0].IncomeTypes;
                rngCountsData.RNGC_NCASHBEN = BaseForm.BaseCaseMstListEntity[0].MstNCashBen;
                rngCountsData.RNGC_ALT_BDATE = snpdataitem.AltBdate;
                rngCountsData.RNGC_SEX = snpdataitem.Sex;
                rngCountsData.RNGC_AGE = snpdataitem.Age;
                rngCountsData.RNGC_ETHNIC = snpdataitem.Ethnic;
                rngCountsData.RNGC_EDUCATION = snpdataitem.Education;
                rngCountsData.RNGC_HEALTH_INS = snpdataitem.HealthIns;
                rngCountsData.RNGC_HEALTH_CODES = snpdataitem.Health_Codes;
                rngCountsData.RNGC_VET = snpdataitem.Vet;
                rngCountsData.RNGC_MILITARY_STATUS = snpdataitem.MilitaryStatus;
                rngCountsData.RNGC_DISABLE = snpdataitem.Disable;
                rngCountsData.RNGC_FOOD_STAMPS = snpdataitem.FootStamps;
                rngCountsData.RNGC_FARMER = snpdataitem.Farmer;
                rngCountsData.RNGC_WORK_STAT = snpdataitem.WorkStatus;
                rngCountsData.RNGC_EMPLOYED = snpdataitem.Employed;
                rngCountsData.RNGC_DOB_NA = snpdataitem.DobNa;
                rngCountsData.RNGC_RACE = snpdataitem.Race;
                rngCountsData.Mode = "Add";
                _model.CaseMstData.InsertUpdateDelRNGCounts(rngCountsData);

            }
        }

        private void pic_Excel_Click(object sender, EventArgs e)
        {
            calc_FEDOMBFix();
        }


        private void calc_FEDOMBFix()
        {

            string PdfName = "ABCDPovertyFile";


            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }


            try
            {
                string Tmpstr = PdfName + ".xls";
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
                PdfName += ".xls";

            try
            {

                ExcelDocument xlWorkSheet = new ExcelDocument();
                xlWorkSheet.ColumnWidth(0, 0);
                xlWorkSheet.ColumnWidth(1, 100);
                xlWorkSheet.ColumnWidth(2, 200);
                xlWorkSheet.ColumnWidth(3, 100);
                xlWorkSheet.ColumnWidth(4, 100);
                xlWorkSheet.ColumnWidth(5, 100);

                int excelcolumn = 0;

                xlWorkSheet[excelcolumn, 1].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                xlWorkSheet[excelcolumn, 1].Alignment = Alignment.Centered;
                xlWorkSheet.WriteCell(excelcolumn, 1, "Applicant No");

                xlWorkSheet[excelcolumn, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                xlWorkSheet[excelcolumn, 2].Alignment = Alignment.Centered;
                xlWorkSheet.WriteCell(excelcolumn, 2, "#In Prog");

                xlWorkSheet[excelcolumn, 3].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                xlWorkSheet[excelcolumn, 3].Alignment = Alignment.Centered;
                xlWorkSheet.WriteCell(excelcolumn, 3, "Program Income");

                xlWorkSheet[excelcolumn, 4].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                xlWorkSheet[excelcolumn, 4].Alignment = Alignment.Centered;
                xlWorkSheet.WriteCell(excelcolumn, 4, "MST Poverty");

                xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Centered;
                xlWorkSheet.WriteCell(excelcolumn, 5, "Calculated New Poverty");



                double povertyPercent = 0;
                string type = "FED";

                LIHEAPBEntity Search_Entity = new LIHEAPBEntity(true);
                Search_Entity.Agency = BaseForm.BaseAgency;
                Search_Entity.Dept = BaseForm.BaseDept;
                Search_Entity.Prog = BaseForm.BaseProg;
                Search_Entity.Year = BaseForm.BaseYear;
                List<LIHEAPBEntity> CaseMstListData = _model.LiheAllData.Browse_LIHEAPB(Search_Entity, "Browse");

                List<CaseMstEntity> casemstapplicantdata = _model.CaseMstData.GetCaseMstadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty);
                foreach (LIHEAPBEntity casemstdataitem in CaseMstListData)
                {

                    double povertyBase = 0;
                    double povertyFactory = 0;
                    double povertyA = 0;
                    List<MasterPovertyEntity> masterPovertyDetails = _model.masterPovertyData.GetFedralOmbChart(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, type, casemstdataitem.Award_Date.ToString());
                    if (masterPovertyDetails.Count > 0)
                    {
                        povertyBase = double.Parse(masterPovertyDetails[0].Gdl1Value.ToString()) - double.Parse(masterPovertyDetails[0].Gdl2Value.ToString());
                        povertyFactory = double.Parse(masterPovertyDetails[0].Gdl2Value);
                        double noOfHouseHolds = casemstdataitem.FAP_No_InHH.ToString().Equals(string.Empty) ? 0.0 : double.Parse(casemstdataitem.FAP_No_InHH);
                        PovertyException propIncrementdata = null; //_model.masterPovertyData.GetPovertyException(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, masterPovertyDetails[0].GdlStartDate, masterPovertyDetails[0].GdlEndDate, calVerificationDate.Value.ToShortDateString(), "OMB");
                        int inthouseholds = casemstdataitem.FAP_No_InHH.ToString().Equals(string.Empty) ? 0 : int.Parse(casemstdataitem.FAP_No_InHH);
                        if (propIncrementdata != null)
                        {
                            povertyBase = double.Parse(masterPovertyDetails[0].Gdl1Value.ToString());
                            switch (inthouseholds)
                            {
                                case 1:
                                    povertyA = 0;
                                    break;
                                case 2:
                                    povertyA = Convert.ToDouble(propIncrementdata.Exp2Value);
                                    break;
                                case 3:
                                    povertyA = Convert.ToDouble(propIncrementdata.Exp2Value) + Convert.ToDouble(propIncrementdata.Exp3Value);
                                    break;
                                case 4:
                                    povertyA = Convert.ToDouble(propIncrementdata.Exp2Value) + Convert.ToDouble(propIncrementdata.Exp3Value) + Convert.ToDouble(propIncrementdata.Exp4Value);
                                    break;
                                case 5:
                                    povertyA = Convert.ToDouble(propIncrementdata.Exp2Value) + Convert.ToDouble(propIncrementdata.Exp3Value) + Convert.ToDouble(propIncrementdata.Exp4Value) + Convert.ToDouble(propIncrementdata.Exp5Value);
                                    break;
                                case 6:
                                    povertyA = Convert.ToDouble(propIncrementdata.Exp2Value) + Convert.ToDouble(propIncrementdata.Exp3Value) + Convert.ToDouble(propIncrementdata.Exp4Value) + Convert.ToDouble(propIncrementdata.Exp5Value) + Convert.ToDouble(propIncrementdata.Exp6Value);
                                    break;
                                case 7:
                                    povertyA = Convert.ToDouble(propIncrementdata.Exp2Value) + Convert.ToDouble(propIncrementdata.Exp3Value) + Convert.ToDouble(propIncrementdata.Exp4Value) + Convert.ToDouble(propIncrementdata.Exp5Value) + Convert.ToDouble(propIncrementdata.Exp6Value) + Convert.ToDouble(propIncrementdata.Exp7Value);
                                    break;
                                case 8:
                                    povertyA = Convert.ToDouble(propIncrementdata.Exp2Value) + Convert.ToDouble(propIncrementdata.Exp3Value) + Convert.ToDouble(propIncrementdata.Exp4Value) + Convert.ToDouble(propIncrementdata.Exp5Value) + Convert.ToDouble(propIncrementdata.Exp6Value) + Convert.ToDouble(propIncrementdata.Exp7Value) + Convert.ToDouble(propIncrementdata.Exp8Value);
                                    break;
                                default:
                                    if (inthouseholds > 8)
                                    {
                                        povertyA = (Convert.ToDouble(propIncrementdata.Exp2Value) + Convert.ToDouble(propIncrementdata.Exp3Value) + Convert.ToDouble(propIncrementdata.Exp4Value) + Convert.ToDouble(propIncrementdata.Exp5Value) + Convert.ToDouble(propIncrementdata.Exp6Value) + Convert.ToDouble(propIncrementdata.Exp7Value) + Convert.ToDouble(propIncrementdata.Exp8Value)) + ((inthouseholds - 8) * Convert.ToDouble(propIncrementdata.Exp8Value));
                                    }
                                    break;
                            }

                        }
                        else
                        {
                            povertyA = noOfHouseHolds * povertyFactory;
                        }
                        double povertyB = povertyBase + povertyA;
                        double povertyProgramIncome = casemstdataitem.FAP_Income.ToString().Equals(string.Empty) ? 0.0 : double.Parse(casemstdataitem.FAP_Income);
                        povertyPercent = (povertyProgramIncome / povertyB) * 100;
                        //povertyPercent = Math.Round(povertyPercent);

                        /// newly added start this logic 05/10/2016
                        string Str_povertyD = povertyPercent.ToString();
                        string[] parts;
                        if (Str_povertyD.Contains("."))
                        {
                            parts = (povertyPercent.ToString()).Split('.');
                            povertyPercent = double.Parse(parts[0]);

                            if (double.Parse(parts[1]) > 0)
                                povertyPercent = (povertyPercent + 1);
                            else
                                povertyPercent = (povertyPercent + 0.5);
                        }
                        /// end

                        if (povertyPercent < 1)
                            povertyPercent = 1;
                        else if (povertyPercent > 1000)
                            povertyPercent = 999;

                        povertyPercent.ToString();
                    }

                    excelcolumn = excelcolumn + 1;

                    xlWorkSheet.WriteCell(excelcolumn, 1, casemstdataitem.AppNo);
                    xlWorkSheet.WriteCell(excelcolumn, 2, casemstdataitem.FAP_No_InHH);
                    xlWorkSheet.WriteCell(excelcolumn, 3, casemstdataitem.FAP_Income);
                    CaseMstEntity caseombdata = casemstapplicantdata.Find(u => u.ApplNo == casemstdataitem.AppNo);
                    if (caseombdata != null)
                    {
                        xlWorkSheet.WriteCell(excelcolumn, 4, caseombdata.Poverty);
                    }
                    else
                    {
                        xlWorkSheet.WriteCell(excelcolumn, 4, caseombdata.Poverty);
                    }
                    xlWorkSheet.WriteCell(excelcolumn, 5, povertyPercent.ToString());

                }

                FileStream stream = new FileStream(PdfName, FileMode.Create);

                xlWorkSheet.Save(stream);
                stream.Close();
            }
            catch (Exception ex)
            {

            }
        }


        private void On_SaveForm_Closed1()
        {
            Random_Filename = null;

            string ReaderName = string.Empty;

            ReaderName = propReportPath + "\\" + "Incver.pdf";



            PdfName = "Income_Verification_Report";//form.GetFileName();
            //PdfName = strFolderPath + PdfName;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
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

            PdfReader Hreader = new PdfReader(ReaderName);

            PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
            Hstamper.Writer.SetPageSize(PageSize.A4);
            PdfContentByte cb = Hstamper.GetOverContent(1);

            //AcroFields form = Hstamper.AcroFields;           
            //FileStream fs = new FileStream(PdfName, FileMode.Create);

            //Document document = new Document();
            //document.SetPageSize(iTextSharp.text.PageSize.A4);
            //PdfWriter writer = PdfWriter.GetInstance(document, fs);
            //document.Open();
            //cb = Writer.DirectContent;
            ////string Priv_Scr = null;
            ////document = new Document(iTextSharp.text.PageSize.A4.Rotate());
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 11);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 11, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

            iTextSharp.text.Image _image_UnChecked = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxUnchecked.JPG"));
            iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

            _image_UnChecked.ScalePercent(60f);
            _image_Checked.ScalePercent(60f);

            try
            {
                X_Pos = 30; Y_Pos = 760;

                //cb.BeginText();
                //cb.SetFontAndSize(bf_helv, 16);
                ////cb.SetColorFill(BaseColor.BLUE.Darker());
                ////cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Head Start Eligibility Verification", 300, 800, 0);
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_CENTER, new Phrase("Head Start Eligibility Verification", TblFontBoldColor), 300, Y_Pos, 0);
                ////cb.SetColorFill(BaseColor.BLACK.Brighter());
                //cb.EndText();


                X_Pos = 110; Y_Pos -= 80;
                //cb.BeginText();
                //cb.SetFontAndSize(bf_helv, 12);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "1. Child Name: ", X_Pos, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("Customer/Primary Name: ", Times), X_Pos, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, TableFontBoldItalicUnderline), X_Pos + 120, Y_Pos, 0);

                //X_Pos = 30; Y_Pos -= 80;
                ////cb.BeginText();
                ////cb.SetFontAndSize(bf_helv, 12);
                ////cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "1. Child Name: ", X_Pos, Y_Pos, 0);
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("1. Child Name: ", Times), X_Pos, Y_Pos, 0);
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, TableFontBoldItalicUnderline), X_Pos + 75, Y_Pos, 0);


                int FSeq = int.Parse(BaseForm.BaseCaseMstListEntity[0].FamilySeq.Trim());
                CaseSnpEntity SnpApp = caseSnpList.Find(u => u.FamilySeq.Trim().Equals(FSeq.ToString()) && u.App.Equals(BaseForm.BaseApplicationNo.Trim()));
                Y_Pos -= 20;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("2. Child Date of Birth: ", Times), X_Pos, Y_Pos, 0);
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(SnpApp.AltBdate.Trim()), TableFontBoldItalicUnderline), X_Pos + 105, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("Customer/Primary Date of Birth: ", Times), X_Pos, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(SnpApp.AltBdate.Trim()), TableFontBoldItalicUnderline), X_Pos + 150, Y_Pos, 0);



                Y_Pos -= 20;
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "3. Child date of entry into Program: ", X_Pos, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("Customer/Primary Intake Date in Program: ", Times), X_Pos, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(BaseForm.BaseCaseMstListEntity[0].IntakeDate.Trim()), TableFontBoldItalicUnderline), X_Pos + 208, Y_Pos, 0);

                Y_Pos -= 20;
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "4. Verify Eligibility, Check which category of eligibility does this child fall into", X_Pos, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("Verify Eligibility Requirements:", TblFontBold), X_Pos, Y_Pos, 0);

                //cb.EndText();
                Y_Pos -= 20;
                /************************************CheckBoxes****************************/
                iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(806, 40, 788, 60);
                PdfAppearance[] onOff = new PdfAppearance[2];
                onOff[0] = cb.CreateAppearance(20, 20);
                onOff[0].Rectangle(1, 20, 1, 20);
                onOff[0].Rectangle(18, 18, 1, 1);
                onOff[0].Stroke();
                onOff[1] = cb.CreateAppearance(20, 20);
                onOff[1].SetRGBColorFill(255, 128, 128);
                onOff[1].Rectangle(18, 18, 1, 1);
                onOff[1].FillStroke();
                onOff[1].MoveTo(1, 1);
                onOff[1].LineTo(19, 19);
                onOff[1].MoveTo(1, 19);
                onOff[1].LineTo(19, 1);

                RadioCheckField checkbox;
                PdfFormField SField;

                X_Pos = 125; //45
                //rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //rect.Rotate();
                //checkbox = new RadioCheckField(writer, rect, "Income", "On");
                ////checkbox.Rotation = 90;
                //SField = checkbox.CheckField;
                //writer.AddAnnotation(SField);
                _image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                cb.AddImage(_image_UnChecked);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Completed CSBG Application", Times), X_Pos + 14, Y_Pos, 0);

                //X_Pos = 60;
                Y_Pos -= 20;
                //rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                ////rect.Rotate();
                //checkbox = new RadioCheckField(writer, rect, "41", "On");
                ////checkbox.Rotation = 90;
                //SField = checkbox.CheckField;
                //writer.AddAnnotation(SField);
                _image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                cb.AddImage(_image_UnChecked);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Income Documentation", Times), X_Pos + 14, Y_Pos, 0);

                X_Pos = 150;
                Y_Pos -= 20;
                //rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                ////rect.Rotate();
                //checkbox = new RadioCheckField(writer, rect, "42", "On");
                ////checkbox.Rotation = 90;
                //SField = checkbox.CheckField;
                //writer.AddAnnotation(SField);
                _image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                cb.AddImage(_image_UnChecked);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Verified income for another program no older than 10/01/2018 entered in Captain/RIAA:", Times), X_Pos + 14, Y_Pos, 0);
                //Y_Pos -= 10;
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("(no more than 35% of enrolled children may fall into this category)", Times), X_Pos + 14, Y_Pos, 0);

                Y_Pos -= 20;
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Date that Income was verified: ______________________", Times), X_Pos + 20, Y_Pos, 0);

                Y_Pos -= 20;
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("FPL% ______________", Times), X_Pos + 20, Y_Pos, 0);//("FPL/OMB% ______________", Times), X_Pos + 20, Y_Pos, 0);


                Y_Pos -= 20;
                //rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                ////rect.Rotate();
                //checkbox = new RadioCheckField(writer, rect, "43", "On");
                ////checkbox.Rotation = 90;
                //SField = checkbox.CheckField;
                //writer.AddAnnotation(SField);
                _image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                cb.AddImage(_image_UnChecked);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Documentation of 12 months of income", Times), X_Pos + 14, Y_Pos, 0);

                Y_Pos -= 20;
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Type of Income used to verify:", Times), X_Pos + 17, Y_Pos, 0);

                Y_Pos -= 20;
                //rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                ////rect.Rotate();
                //checkbox = new RadioCheckField(writer, rect, "44", "On");
                ////checkbox.Rotation = 90;
                //SField = checkbox.CheckField;
                //writer.AddAnnotation(SField);
                _image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                cb.AddImage(_image_UnChecked);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Check stubs", Times), X_Pos + 20, Y_Pos, 0);

                //X_Pos = 45;
                Y_Pos -= 20;
                //rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                ////rect.Rotate();
                //checkbox = new RadioCheckField(writer, rect, "Public Assistance", "On");
                ////checkbox.Rotation = 90;
                //SField = checkbox.CheckField;
                //writer.AddAnnotation(SField);
                _image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                cb.AddImage(_image_UnChecked);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("W-2", Times), X_Pos + 20, Y_Pos, 0);


                Y_Pos -= 20;
                //rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                ////rect.Rotate();
                //checkbox = new RadioCheckField(writer, rect, "Homeless", "On");
                ////checkbox.Rotation = 90;
                //SField = checkbox.CheckField;
                //writer.AddAnnotation(SField);
                _image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                cb.AddImage(_image_UnChecked);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Income Tax Form 1040", Times), X_Pos + 20, Y_Pos, 0);

                Y_Pos -= 20;
                //rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                ////rect.Rotate();
                //checkbox = new RadioCheckField(writer, rect, "Foster Care", "On");
                ////checkbox.Rotation = 90;
                //SField = checkbox.CheckField;
                //writer.AddAnnotation(SField);
                _image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                cb.AddImage(_image_UnChecked);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Wage inquiry", Times), X_Pos + 20, Y_Pos, 0);

                Y_Pos -= 20;
                _image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                cb.AddImage(_image_UnChecked);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("SSI Documentation: Award letter", Times), X_Pos + 20, Y_Pos, 0);

                Y_Pos -= 20;
                _image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                cb.AddImage(_image_UnChecked);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Other: ____________________", Times), X_Pos + 20, Y_Pos, 0);

                Y_Pos -= 20;
                _image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                cb.AddImage(_image_UnChecked);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Documentation of no income: __________________", Times), X_Pos + 20, Y_Pos, 0);

                Y_Pos -= 20;
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Annual Income: " + BaseForm.BaseCaseMstListEntity[0].FamIncome.Trim(), Times), X_Pos + 20, Y_Pos, 0);
                Y_Pos -= 20;
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Household Size: " + caseSnpList.Count.ToString(), Times), X_Pos + 20, Y_Pos, 0);
                Y_Pos -= 20;
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("FPL%: " + BaseForm.BaseCaseMstListEntity[0].Poverty.Trim(), Times), X_Pos + 20, Y_Pos, 0);//("FPG/OMB%: " + BaseForm.BaseCaseMstListEntity[0].Poverty.Trim(), Times), X_Pos + 20, Y_Pos, 0);

                X_Pos = 125; Y_Pos -= 20;
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("5. What documentation was used determine eligibility?", Times), X_Pos, Y_Pos, 0);
                //Y_Pos -= 20; X_Pos = 60;
                _image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                cb.AddImage(_image_UnChecked);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Supporting documentation of need (bills, estimates, etc.)", Times), X_Pos + 14, Y_Pos, 0);

                //X_Pos = 285;

                //_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                //cb.AddImage(_image_UnChecked);
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Written statements from employers", Times), X_Pos + 14, Y_Pos, 0);

                //Y_Pos -= 20; X_Pos = 60;
                ////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //////rect.Rotate();
                ////checkbox = new RadioCheckField(writer, rect, "53", "On");
                //////checkbox.Rotation = 90;
                ////SField = checkbox.CheckField;
                ////writer.AddAnnotation(SField);
                //_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                //cb.AddImage(_image_UnChecked);
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("W-2", Times), X_Pos + 14, Y_Pos, 0);
                //X_Pos = 285;
                ////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //////rect.Rotate();
                ////checkbox = new RadioCheckField(writer, rect, "54", "On");
                //////checkbox.Rotation = 90;
                ////SField = checkbox.CheckField;
                ////writer.AddAnnotation(SField);
                //_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                //cb.AddImage(_image_UnChecked);
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Foster care reimbursement", Times), X_Pos + 14, Y_Pos, 0);

                //Y_Pos -= 20; X_Pos = 60;
                ////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //////rect.Rotate();
                ////checkbox = new RadioCheckField(writer, rect, "55", "On");
                //////checkbox.Rotation = 90;
                ////SField = checkbox.CheckField;
                ////writer.AddAnnotation(SField);
                //_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                //cb.AddImage(_image_UnChecked);
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("TANF Documentation", Times), X_Pos + 14, Y_Pos, 0);
                //X_Pos = 285;
                ////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //////rect.Rotate();
                ////checkbox = new RadioCheckField(writer, rect, "56", "On");
                //////checkbox.Rotation = 90;
                ////SField = checkbox.CheckField;
                ////writer.AddAnnotation(SField);
                //_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                //cb.AddImage(_image_UnChecked);
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("SSI Documentation", Times), X_Pos + 14, Y_Pos, 0);

                //Y_Pos -= 20; X_Pos = 60;
                ////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //////rect.Rotate();
                ////checkbox = new RadioCheckField(writer, rect, "57", "On");
                //////checkbox.Rotation = 90;
                ////SField = checkbox.CheckField;
                ////writer.AddAnnotation(SField);
                //_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                //cb.AddImage(_image_UnChecked);
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Pay stub or pay envelopes", Times), X_Pos + 14, Y_Pos, 0);
                //X_Pos = 285;
                ////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //////rect.Rotate();
                ////checkbox = new RadioCheckField(writer, rect, "58", "On");
                //////checkbox.Rotation = 90;
                ////SField = checkbox.CheckField;
                ////writer.AddAnnotation(SField);
                //_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                //cb.AddImage(_image_UnChecked);
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Other", Times), X_Pos + 14, Y_Pos, 0);

                //Y_Pos -= 20; X_Pos = 60;
                ////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //////rect.Rotate();
                ////checkbox = new RadioCheckField(writer, rect, "59", "On");
                //////checkbox.Rotation = 90;
                ////SField = checkbox.CheckField;
                ////writer.AddAnnotation(SField);
                //_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                //cb.AddImage(_image_UnChecked);
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Unemployment", Times), X_Pos + 14, Y_Pos, 0);

                //X_Pos = 285;
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("If Other, please explain ____________________________", Times), X_Pos + 14, Y_Pos, 0);

                //Y_Pos -= 20; X_Pos = 60;
                ////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //////rect.Rotate();
                ////checkbox = new RadioCheckField(writer, rect, "510", "On");
                //////checkbox.Rotation = 90;
                ////SField = checkbox.CheckField;
                ////writer.AddAnnotation(SField);
                //_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                //cb.AddImage(_image_UnChecked);
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Documentation of no income: ________________________________________________________________", Times), X_Pos + 14, Y_Pos, 0);

                X_Pos = 110; Y_Pos -= 20;
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Staff Signature: __________________________________________________", Times), X_Pos, Y_Pos, 0);

                string Worker = string.Empty;
                if (hierarchyEntity.Count > 0)
                {
                    foreach (HierarchyEntity caseworker in hierarchyEntity)
                    {
                        if (BaseForm.BaseCaseMstListEntity[0].Verifier.ToString().Trim() == caseworker.CaseWorker.Trim())
                        {
                            Worker = caseworker.HirarchyName.Trim(); break;
                        }
                    }
                }

                Y_Pos -= 20;
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Staff Name: " + Worker, Times), X_Pos, Y_Pos, 0);

                Y_Pos -= 20;
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Title: __________________________________________________________", Times), X_Pos, Y_Pos, 0);

                Y_Pos -= 20;
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Date: __________________________________________________________", Times), X_Pos, Y_Pos, 0);

                //string Worker = string.Empty;
                //if (hierarchyEntity.Count > 0)
                //{
                //    foreach (HierarchyEntity caseworker in hierarchyEntity)
                //    {
                //        if (BaseForm.BaseCaseMstListEntity[0].Verifier.ToString().Trim() == caseworker.CaseWorker.Trim())
                //        {
                //            Worker = caseworker.HirarchyName.Trim(); break;
                //        }
                //    }

                //    X_Pos = 30; Y_Pos -= 20;
                //    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("7. Staff Name : " + Worker, Times), X_Pos, Y_Pos, 0);
                //    X_Pos = 285;
                //    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Title: ___________________________________________", Times), X_Pos + 14, Y_Pos, 0);
                //}

            }
            catch (Exception ex) { /*document.Add(new Paragraph("Aborted due to Exception............................................... "));*/ }
            //document.Close();
            //fs.Close();
            //fs.Dispose();
            Hstamper.Close();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }

        }

        private void CaseIncomeVerification_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnSave.Visible)
            {
                DialogResult result = MessageBox.Show("Are you sure want to close? Record not saved", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerFormClosed); ;
                _caseIncomeVerification = (Wisej.Web.Form)sender;
                if (result == DialogResult.Yes)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;

                }
            }
        }

        Wisej.Web.Form _caseIncomeVerification;
        string Refresh_Control = string.Empty;
        private void MessageBoxHandlerFormClosed(DialogResult dialogResult)
        {
            // senderForm.FormClosing -= SampleTestForm_FormClosing;

                // Set DialogResult value of the Form as a text for label
                if (dialogResult == DialogResult.Yes)
                {
                    _caseIncomeVerification.FormClosing -= CaseIncomeVerification_FormClosing;
                    _caseIncomeVerification.Close();
                }
            
        }

        private void On_SaveForm_Closed2()
        {
            Random_Filename = null;

            string ReaderName = string.Empty;

            ReaderName = propReportPath + "\\" + "HS_Eligibility_Verification_Form.pdf";



            PdfName = "Income_Verification_Report";//form.GetFileName();
            //PdfName = strFolderPath + PdfName;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
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

            PdfReader Hreader = new PdfReader(ReaderName);

            PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
            Hstamper.Writer.SetPageSize(PageSize.A4);
            PdfContentByte cb = Hstamper.GetOverContent(1);

            //AcroFields form = Hstamper.AcroFields;           
            //FileStream fs = new FileStream(PdfName, FileMode.Create);

            //Document document = new Document();
            //document.SetPageSize(iTextSharp.text.PageSize.A4);
            //PdfWriter writer = PdfWriter.GetInstance(document, fs);
            //document.Open();
            //cb = Writer.DirectContent;
            ////string Priv_Scr = null;
            ////document = new Document(iTextSharp.text.PageSize.A4.Rotate());
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 11);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 11, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

            iTextSharp.text.Image _image_UnChecked = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxUnchecked.JPG"));
            iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

            _image_UnChecked.ScalePercent(60f);
            _image_Checked.ScalePercent(60f);

            try
            {
                X_Pos = 30; Y_Pos = 760;

                //cb.BeginText();
                //cb.SetFontAndSize(bf_helv, 16);
                ////cb.SetColorFill(BaseColor.BLUE.Darker());
                ////cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Head Start Eligibility Verification", 300, 800, 0);
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_CENTER, new Phrase("Head Start Eligibility Verification", TblFontBoldColor), 300, Y_Pos, 0);
                ////cb.SetColorFill(BaseColor.BLACK.Brighter());
                //cb.EndText();

               
                X_Pos = 55; Y_Pos -= 100;
                //cb.BeginText();
                //cb.SetFontAndSize(bf_helv, 12);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "1. Child Name: ", X_Pos, Y_Pos, 0);
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("Customer/Primary Name: ", Times), X_Pos, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, TableFontBoldItalic), X_Pos + 120, Y_Pos, 0);

                //X_Pos = 30; Y_Pos -= 80;
                ////cb.BeginText();
                ////cb.SetFontAndSize(bf_helv, 12);
                ////cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "1. Child Name: ", X_Pos, Y_Pos, 0);
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("1. Child Name: ", Times), X_Pos, Y_Pos, 0);
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, TableFontBoldItalicUnderline), X_Pos + 75, Y_Pos, 0);


                int FSeq = int.Parse(BaseForm.BaseCaseMstListEntity[0].FamilySeq.Trim());
                CaseSnpEntity SnpApp = caseSnpList.Find(u => u.FamilySeq.Trim().Equals(FSeq.ToString()) && u.App.Equals(BaseForm.BaseApplicationNo.Trim()));
                Y_Pos -= 35;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("2. Child Date of Birth: ", Times), X_Pos, Y_Pos, 0);
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(SnpApp.AltBdate.Trim()), TableFontBoldItalicUnderline), X_Pos + 105, Y_Pos, 0);
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("Customer/Primary Date of Birth: ", Times), X_Pos, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(SnpApp.AltBdate.Trim()), TableFontBoldItalic), X_Pos + 155, Y_Pos, 0);



                //Y_Pos -= 20;
                ////cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "3. Child date of entry into Program: ", X_Pos, Y_Pos, 0);
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("Customer/Primary Intake Date in Program: ", Times), X_Pos, Y_Pos, 0);
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(BaseForm.BaseCaseMstListEntity[0].IntakeDate.Trim()), TableFontBoldItalicUnderline), X_Pos + 208, Y_Pos, 0);

                //Y_Pos -= 20;
                ////cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "4. Verify Eligibility, Check which category of eligibility does this child fall into", X_Pos, Y_Pos, 0);
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("Verify Eligibility Requirements:", TblFontBold), X_Pos, Y_Pos, 0);

                ////cb.EndText();
                //Y_Pos -= 20;
                ///************************************CheckBoxes****************************/
                //iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(806, 40, 788, 60);
                //PdfAppearance[] onOff = new PdfAppearance[2];
                //onOff[0] = cb.CreateAppearance(20, 20);
                //onOff[0].Rectangle(1, 20, 1, 20);
                //onOff[0].Rectangle(18, 18, 1, 1);
                //onOff[0].Stroke();
                //onOff[1] = cb.CreateAppearance(20, 20);
                //onOff[1].SetRGBColorFill(255, 128, 128);
                //onOff[1].Rectangle(18, 18, 1, 1);
                //onOff[1].FillStroke();
                //onOff[1].MoveTo(1, 1);
                //onOff[1].LineTo(19, 19);
                //onOff[1].MoveTo(1, 19);
                //onOff[1].LineTo(19, 1);

                //RadioCheckField checkbox;
                //PdfFormField SField;

                //X_Pos = 125; //45
                ////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                ////rect.Rotate();
                ////checkbox = new RadioCheckField(writer, rect, "Income", "On");
                //////checkbox.Rotation = 90;
                ////SField = checkbox.CheckField;
                ////writer.AddAnnotation(SField);
                //_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                //cb.AddImage(_image_UnChecked);
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Completed CSBG Application", Times), X_Pos + 14, Y_Pos, 0);

                ////X_Pos = 60;
                //Y_Pos -= 20;
                ////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //////rect.Rotate();
                ////checkbox = new RadioCheckField(writer, rect, "41", "On");
                //////checkbox.Rotation = 90;
                ////SField = checkbox.CheckField;
                ////writer.AddAnnotation(SField);
                //_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                //cb.AddImage(_image_UnChecked);
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Income Documentation", Times), X_Pos + 14, Y_Pos, 0);

                //X_Pos = 150;
                //Y_Pos -= 20;
                ////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //////rect.Rotate();
                ////checkbox = new RadioCheckField(writer, rect, "42", "On");
                //////checkbox.Rotation = 90;
                ////SField = checkbox.CheckField;
                ////writer.AddAnnotation(SField);
                //_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                //cb.AddImage(_image_UnChecked);
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Verified income for another program no older than 10/01/2018 entered in Captain/RIAA:", Times), X_Pos + 14, Y_Pos, 0);
                ////Y_Pos -= 10;
                ////ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("(no more than 35% of enrolled children may fall into this category)", Times), X_Pos + 14, Y_Pos, 0);

                //Y_Pos -= 20;
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Date that Income was verified: ______________________", Times), X_Pos + 20, Y_Pos, 0);

                //Y_Pos -= 20;
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("FPL/OMB% ______________", Times), X_Pos + 20, Y_Pos, 0);


                //Y_Pos -= 20;
                ////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //////rect.Rotate();
                ////checkbox = new RadioCheckField(writer, rect, "43", "On");
                //////checkbox.Rotation = 90;
                ////SField = checkbox.CheckField;
                ////writer.AddAnnotation(SField);
                //_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                //cb.AddImage(_image_UnChecked);
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Documentation of 12 months of income", Times), X_Pos + 14, Y_Pos, 0);

                //Y_Pos -= 20;
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Type of Income used to verify:", Times), X_Pos + 17, Y_Pos, 0);

                //Y_Pos -= 20;
                ////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //////rect.Rotate();
                ////checkbox = new RadioCheckField(writer, rect, "44", "On");
                //////checkbox.Rotation = 90;
                ////SField = checkbox.CheckField;
                ////writer.AddAnnotation(SField);
                //_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                //cb.AddImage(_image_UnChecked);
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Check stubs", Times), X_Pos + 20, Y_Pos, 0);

                ////X_Pos = 45;
                //Y_Pos -= 20;
                ////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //////rect.Rotate();
                ////checkbox = new RadioCheckField(writer, rect, "Public Assistance", "On");
                //////checkbox.Rotation = 90;
                ////SField = checkbox.CheckField;
                ////writer.AddAnnotation(SField);
                //_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                //cb.AddImage(_image_UnChecked);
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("W-2", Times), X_Pos + 20, Y_Pos, 0);


                //Y_Pos -= 20;
                ////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //////rect.Rotate();
                ////checkbox = new RadioCheckField(writer, rect, "Homeless", "On");
                //////checkbox.Rotation = 90;
                ////SField = checkbox.CheckField;
                ////writer.AddAnnotation(SField);
                //_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                //cb.AddImage(_image_UnChecked);
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Income Tax Form 1040", Times), X_Pos + 20, Y_Pos, 0);

                //Y_Pos -= 20;
                ////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                //////rect.Rotate();
                ////checkbox = new RadioCheckField(writer, rect, "Foster Care", "On");
                //////checkbox.Rotation = 90;
                ////SField = checkbox.CheckField;
                ////writer.AddAnnotation(SField);
                //_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                //cb.AddImage(_image_UnChecked);
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Wage inquiry", Times), X_Pos + 20, Y_Pos, 0);

                //Y_Pos -= 20;
                //_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                //cb.AddImage(_image_UnChecked);
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("SSI Documentation: Award letter", Times), X_Pos + 20, Y_Pos, 0);

                //Y_Pos -= 20;
                //_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                //cb.AddImage(_image_UnChecked);
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Other: ____________________", Times), X_Pos + 20, Y_Pos, 0);

                //Y_Pos -= 20;
                //_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                //cb.AddImage(_image_UnChecked);
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Documentation of no income: __________________", Times), X_Pos + 20, Y_Pos, 0);

                //Y_Pos -= 20;
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Annual Income: " + BaseForm.BaseCaseMstListEntity[0].FamIncome.Trim(), Times), X_Pos + 20, Y_Pos, 0);
                //Y_Pos -= 20;
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Household Size: " + caseSnpList.Count.ToString(), Times), X_Pos + 20, Y_Pos, 0);
                //Y_Pos -= 20;
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("FPG/OMB%: " + BaseForm.BaseCaseMstListEntity[0].Poverty.Trim(), Times), X_Pos + 20, Y_Pos, 0);

                //X_Pos = 125; Y_Pos -= 20;
                ////ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("5. What documentation was used determine eligibility?", Times), X_Pos, Y_Pos, 0);
                ////Y_Pos -= 20; X_Pos = 60;
                //_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                //cb.AddImage(_image_UnChecked);
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Supporting documentation of need (bills, estimates, etc.)", Times), X_Pos + 14, Y_Pos, 0);

                ////X_Pos = 285;

                ////_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                ////cb.AddImage(_image_UnChecked);
                ////ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Written statements from employers", Times), X_Pos + 14, Y_Pos, 0);

                ////Y_Pos -= 20; X_Pos = 60;
                //////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                ////////rect.Rotate();
                //////checkbox = new RadioCheckField(writer, rect, "53", "On");
                ////////checkbox.Rotation = 90;
                //////SField = checkbox.CheckField;
                //////writer.AddAnnotation(SField);
                ////_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                ////cb.AddImage(_image_UnChecked);
                ////ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("W-2", Times), X_Pos + 14, Y_Pos, 0);
                ////X_Pos = 285;
                //////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                ////////rect.Rotate();
                //////checkbox = new RadioCheckField(writer, rect, "54", "On");
                ////////checkbox.Rotation = 90;
                //////SField = checkbox.CheckField;
                //////writer.AddAnnotation(SField);
                ////_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                ////cb.AddImage(_image_UnChecked);
                ////ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Foster care reimbursement", Times), X_Pos + 14, Y_Pos, 0);

                ////Y_Pos -= 20; X_Pos = 60;
                //////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                ////////rect.Rotate();
                //////checkbox = new RadioCheckField(writer, rect, "55", "On");
                ////////checkbox.Rotation = 90;
                //////SField = checkbox.CheckField;
                //////writer.AddAnnotation(SField);
                ////_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                ////cb.AddImage(_image_UnChecked);
                ////ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("TANF Documentation", Times), X_Pos + 14, Y_Pos, 0);
                ////X_Pos = 285;
                //////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                ////////rect.Rotate();
                //////checkbox = new RadioCheckField(writer, rect, "56", "On");
                ////////checkbox.Rotation = 90;
                //////SField = checkbox.CheckField;
                //////writer.AddAnnotation(SField);
                ////_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                ////cb.AddImage(_image_UnChecked);
                ////ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("SSI Documentation", Times), X_Pos + 14, Y_Pos, 0);

                ////Y_Pos -= 20; X_Pos = 60;
                //////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                ////////rect.Rotate();
                //////checkbox = new RadioCheckField(writer, rect, "57", "On");
                ////////checkbox.Rotation = 90;
                //////SField = checkbox.CheckField;
                //////writer.AddAnnotation(SField);
                ////_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                ////cb.AddImage(_image_UnChecked);
                ////ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Pay stub or pay envelopes", Times), X_Pos + 14, Y_Pos, 0);
                ////X_Pos = 285;
                //////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                ////////rect.Rotate();
                //////checkbox = new RadioCheckField(writer, rect, "58", "On");
                ////////checkbox.Rotation = 90;
                //////SField = checkbox.CheckField;
                //////writer.AddAnnotation(SField);
                ////_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                ////cb.AddImage(_image_UnChecked);
                ////ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Other", Times), X_Pos + 14, Y_Pos, 0);

                ////Y_Pos -= 20; X_Pos = 60;
                //////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                ////////rect.Rotate();
                //////checkbox = new RadioCheckField(writer, rect, "59", "On");
                ////////checkbox.Rotation = 90;
                //////SField = checkbox.CheckField;
                //////writer.AddAnnotation(SField);
                ////_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                ////cb.AddImage(_image_UnChecked);
                ////ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Unemployment", Times), X_Pos + 14, Y_Pos, 0);

                ////X_Pos = 285;
                ////ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("If Other, please explain ____________________________", Times), X_Pos + 14, Y_Pos, 0);

                ////Y_Pos -= 20; X_Pos = 60;
                //////rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 10, X_Pos + 10, Y_Pos);
                ////////rect.Rotate();
                //////checkbox = new RadioCheckField(writer, rect, "510", "On");
                ////////checkbox.Rotation = 90;
                //////SField = checkbox.CheckField;
                //////writer.AddAnnotation(SField);
                ////_image_UnChecked.SetAbsolutePosition(X_Pos, Y_Pos);
                ////cb.AddImage(_image_UnChecked);
                ////ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Documentation of no income: ________________________________________________________________", Times), X_Pos + 14, Y_Pos, 0);

                //X_Pos = 110; Y_Pos -= 20;
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Staff Signature: __________________________________________________", Times), X_Pos, Y_Pos, 0);

                string Worker = string.Empty;
                if (hierarchyEntity.Count > 0)
                {
                    foreach (HierarchyEntity caseworker in hierarchyEntity)
                    {
                        if (BaseForm.BaseCaseMstListEntity[0].Verifier.ToString().Trim() == caseworker.CaseWorker.Trim())
                        {
                            Worker = caseworker.HirarchyName.Trim(); break;
                        }
                    }
                }
                //Y_Pos -= 520;X_Pos = 160;
                Y_Pos -= 476; X_Pos = 165;
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(Worker, TblFontBold), X_Pos, Y_Pos, 0);

                //Y_Pos -= 20;
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Staff Name: " + Worker, Times), X_Pos, Y_Pos, 0);

                //Y_Pos -= 20;
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Title: __________________________________________________________", Times), X_Pos, Y_Pos, 0);

                //Y_Pos -= 20;
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Date: __________________________________________________________", Times), X_Pos, Y_Pos, 0);

                ////string Worker = string.Empty;
                ////if (hierarchyEntity.Count > 0)
                ////{
                ////    foreach (HierarchyEntity caseworker in hierarchyEntity)
                ////    {
                ////        if (BaseForm.BaseCaseMstListEntity[0].Verifier.ToString().Trim() == caseworker.CaseWorker.Trim())
                ////        {
                ////            Worker = caseworker.HirarchyName.Trim(); break;
                ////        }
                ////    }

                ////    X_Pos = 30; Y_Pos -= 20;
                ////    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("7. Staff Name : " + Worker, Times), X_Pos, Y_Pos, 0);
                ////    X_Pos = 285;
                ////    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Title: ___________________________________________", Times), X_Pos + 14, Y_Pos, 0);
                ////}

            }
            catch (Exception ex) { /*document.Add(new Paragraph("Aborted due to Exception............................................... "));*/ }
            //document.Close();
            //fs.Close();
            //fs.Dispose();
            Hstamper.Close();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.StartPosition = FormStartPosition.CenterScreen;

                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.StartPosition = FormStartPosition.CenterScreen;

                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }

        }

        //private void SetCB(AcroFields fields, string F)
        //{
        //    try
        //    {
        //        fields.SetField(F, fields.GetFieldItem(F).GetValue(0).GetAsDict(PdfName.AP).GetAsDict(PdfName.N).Keys.Single().ToString().TrimStart('/'));
        //    }
        //    catch { }
        //}

        IncomeReportForm Pdf_Form;
        private void PbPdf_Click(object sender, EventArgs e)
        {
            string Temp_Year = "    ";
            if (!string.IsNullOrEmpty(BaseForm.BaseYear))
                Temp_Year = BaseForm.BaseYear;
            Pdf_Form = new IncomeReportForm(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + Temp_Year + BaseForm.BaseApplicationNo, BaseForm, string.Empty);
            //Pdf_Form.FormClosed += new FormClosedEventHandler(OnSerachFormClosed);
            //Pdf_Form.ShowDialog();
            //Pdf_Form.Close();
            if (Pdf_Form.DialogResult == DialogResult.OK)
            {
                if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
                {
                    PdfViewerNewForm objfrm = new PdfViewerNewForm(Pdf_Form.Get_Pdf_Path());
                    objfrm.StartPosition = FormStartPosition.CenterScreen;

                    objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File1);

                    objfrm.ShowDialog();
                }
                else
                {
                    FrmViewer objfrm = new FrmViewer(Pdf_Form.Get_Pdf_Path());
                    objfrm.StartPosition = FormStartPosition.CenterScreen;

                    objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File1);
                    objfrm.ShowDialog();
                }
            }

        }

        private void On_Delete_PDF_File1(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(Pdf_Form.Get_Pdf_Path());
        }





    }
}