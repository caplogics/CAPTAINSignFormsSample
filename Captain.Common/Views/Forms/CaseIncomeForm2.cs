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
using Captain.Common.Views.Forms.Base;
using Captain.Common.Views.Controls.Compatibility;
using System.Globalization;
using Captain.DatabaseLayer;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DevExpress.CodeParser;
using NPOI.SS.Formula.Functions;
using static Captain.Common.Utilities.Consts;
using DevExpress.Utils.Filtering;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CaseIncomeForm2 : _iForm
    {
        private CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;
        List<FldcntlHieEntity> _fldCntlHieEntity = new List<FldcntlHieEntity>();
        private string strCaseWorkerDefaultCode = "0";
        private string strCaseWorkerDefaultStartCode = "0";
        private int strSnpIndex = 0;
        private bool Amt1Interval = true;
        private bool Amt2Interval = true;
        private bool Amt3Interval = true;
        private bool Amt4Interval = true;
        private bool Amt5Interval = true;
        private bool Date1Interval = true;
        private bool Date2Interval = true;
        private bool Date3Interval = true;
        private bool Date4Interval = true;
        private bool Date5Interval = true;

        string strDefaultValue = "0";
        public CaseIncomeForm2(string changeType, BaseForm baseForm, PrivilegeEntity privilieges, ProgramDefinitionEntity propgramdefination, string strIncomeType,string formType)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            BaseForm = baseForm;
            CaseMSTEntity = BaseForm.BaseCaseMstListEntity[0];

            Privileges = privilieges;
            this.Text = "Income Entry";
            programDefinitionList = propgramdefination;
            propIncomeTypeSwitch = strIncomeType;
            MenuPropertie = string.Empty;
            Amt1Req = false;
            Amt2Req = false;
            Amt3Req = false;
            Amt4Req = false;
            Amt5Req = false;
            Date1Req = false;
            Date2Req = false;
            Date3Req = false;
            Date4Req = false;
            Date5Req = false;
            Amt1Enable = false;
            Amt2Enable = false;
            Amt3Enable = false;
            Amt4Enable = false;
            Amt5Enable = false;
            Date1Enable = false;
            Date2Enable = false;
            Date3Enable = false;
            Date4Enable = false;
            Date5Enable = false;
            IncomeTypeEnable = false;
            ExcludeEnable = false;
            IntervalEnable = false;
            IntervalReq = false;
            IncomeTypeReq = false;
            chkIncomeTypeEnable = false;
            ExcludeReq = false;
            VerifierReq = false;
            StrFormType = formType;
            string HIE = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            List<FldcntlHieEntity> CntlEntity = _model.FieldControls.GetFLDCNTLHIE("CASINCOM", HIE, "FLDCNTL");
            propHourlyMode = "N";
            propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
            if (propAgencyControlDetails != null)
            {
                if (propAgencyControlDetails.IncMethods == "2")
                {
                    propHourlyMode = "Y";
                    this.Factor.Visible = false;
                    this.Sub.HeaderText = "AVG";
                    this.gvtAvgSub.Visible = true;
                    lblMstIntakeDate.Visible = true;
                }
                if (propAgencyControlDetails.IncMethods.ToUpper() == "4")
                {
                    btnIncomeTotal.Visible = true;
                }
            }

            if(BaseForm.BaseAgencyControlDetails.State.Trim()=="CT" && formType=="TMS00081")
            { lblVerMethod.Visible = true;cmbVerfMethod.Visible = true; }

            if(BaseForm.BaseAgencyControlDetails.State.Trim()=="CT")
            {
                dataGridCaseSnp.Columns["Column2"].Visible = true;
            }
            else
                dataGridCaseSnp.Columns["Column2"].Visible = false;

            FLDCNTLHieEntity = CntlEntity;
            fillcombo();
            fillVerMethod_Combo();
            txtHr1.Validator = TextBoxValidation.FloatValidator;
            txtHr2.Validator = TextBoxValidation.FloatValidator;
            txtHr3.Validator = TextBoxValidation.FloatValidator;
            txtHr4.Validator = TextBoxValidation.FloatValidator;
            txtHr5.Validator = TextBoxValidation.FloatValidator;
            lookUpIncomeTypes = _model.lookupDataAccess.GetIncomeTypesDeduction();

            if (lookUpIncomeTypes.Count > 0)
            {
                lookUpIncomeTypes = lookUpIncomeTypes.FindAll(u => u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.ListHierarchy.Contains(BaseForm.BaseAgency + "****") || u.ListHierarchy.Contains("******")).ToList();

                lookUpIncomeTypes = lookUpIncomeTypes.OrderByDescending(u => u.Active).ThenBy(u => u.Desc).ToList();
            }

            DataSet dsNonCash = DatabaseLayer.AgyTab.GetAgyTabDetails(Consts.AgyTab.NonCashBenefits);
            if (dsNonCash.Tables[0].Rows.Count > 0)
            {
                DataTable dt = dsNonCash.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    DataView dv = new DataView(dt);
                    dv.RowFilter = "AGY_9 IS NOT NULL";
                    dtNon_Cash = dv.ToTable();
                }
            }

            DataSet dsFood = DatabaseLayer.AgyTab.GetAgyTabDetails(Consts.AgyTab.FOODSTAMPS);
            if (dsFood.Tables[0].Rows.Count > 0)
            {
                DataTable dt = dsFood.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    DataView dv = new DataView(dt);
                    dv.RowFilter = "AGY_9 IS NOT NULL";
                    dtFood = dv.ToTable();
                }
            }

            propRelation = _model.lookupDataAccess.GetRelationship();
            fillSnpDetails(true);
            Programdefinationcheck();
            if (Privileges.AddPriv.Equals("false"))
            {
                btnAdd.Visible = false;
            }

            if (Privileges.ChangePriv.Equals("false"))
            {
                btnEdit.Visible = false;
            }
            if (Privileges.DelPriv.Equals("false"))
            {
                dataGridCaseIncome.Columns["Delete"].Visible = false;
            }
            if (propIncomeTypeSwitch != "Y")
            {
                dataGridCaseIncome.Columns["gvchkType"].Visible = false;
            }
            else
            {
                dataGridCaseIncome.Columns["gvchkType"].Visible = true;
            }
            dataGridCaseIncome.ReadOnly = true;
            Wisej.Web.ToolTip tooltipadd = new Wisej.Web.ToolTip();
            tooltipadd.SetToolTip(btnAdd, "Add Income");
            Wisej.Web.ToolTip tooltipEdit = new Wisej.Web.ToolTip();
            tooltipEdit.SetToolTip(btnEdit, "Edit Income");

            EnableDisableControls();

        }
        List<CommonEntity> Disableditems { get; set; }
        DataTable dtNon_Cash = new DataTable();
        DataTable dtFood = new DataTable();
        void check_under18NoIncomeMembers()
        {
            string _year = BaseForm.BaseYear.Trim();
            if (_year != "")
            {
                int _oYear = Convert.ToInt32(_year);
                if (_oYear > 2024)
                {
                    if (propAgencyControlDetails.Inccntl18noinc == "Y")
                    {
                        int _hasIncomeval = 0;
                        List<CaseSnpEntity> SNP18FinalList = new List<CaseSnpEntity>();
                        List<CaseSnpEntity> caseSnp18lesMembs = BaseForm.BaseCaseSnpEntity.Where(u => Convert.ToInt32(u.Age) < 18).ToList();
                        if (caseSnp18lesMembs.Count > 0)
                        {
                            foreach (CaseSnpEntity caseSnp in caseSnp18lesMembs)
                            {
                                string _FamSeq = caseSnp.FamilySeq.ToString();
                                List<CaseIncomeEntity> caseIncomeList = _model.CaseMstData.GetCaseIncomeadpynf(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg,
                                    BaseForm.BaseYear, BaseForm.BaseApplicationNo, _FamSeq);
                                if (caseIncomeList.Count > 0)
                                {
                                    _hasIncomeval++;
                                }
                                else
                                {
                                    CaseSnpEntity _casSNP = new CaseSnpEntity();
                                    _casSNP = caseSnp;
                                    SNP18FinalList.Add(_casSNP);
                                }
                            }

                            if (SNP18FinalList.Count > 0)
                            {
                                Caseincome18NOINCForm _oCaseincome18NOINCForm = new Caseincome18NOINCForm(this.BaseForm, SNP18FinalList, propRelation, programDefinitionList, Disableditems,
                                    lookUpIncomeTypes, commonIntervalProp, strCaseWorkerDefaultCode);
                                _oCaseincome18NOINCForm.StartPosition = FormStartPosition.CenterScreen;
                                _oCaseincome18NOINCForm.ShowDialog();
                            }
                        }
                    }
                }
            }
        }


        #region properties
        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public ProgramDefinitionEntity programDefinitionList { get; set; }
        public string MstIntakeDate { get; set; }
        public DateTime MstIntakeStartDate { get; set; }
        public DateTime MstIntakeEndDate { get; set; }
        public AgencyControlEntity propAgencyControlDetails { get; set; }
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
        public List<CommonEntity> commonIntervalProp { get; set; }
        public List<CommonEntity> lookUpIncomeTypes { get; set; }
        public string strFamilySeq { get; set; }
        public bool Amt1Req { get; set; }
        public bool Amt2Req { get; set; }
        public bool Amt3Req { get; set; }
        public bool Amt4Req { get; set; }
        public bool Amt5Req { get; set; }
        public bool Date1Req { get; set; }
        public bool Date2Req { get; set; }
        public bool Date3Req { get; set; }
        public bool Date4Req { get; set; }
        public bool Date5Req { get; set; }
        public bool IntervalReq { get; set; }
        public bool IncomeTypeReq { get; set; }
        public bool Amt1Enable { get; set; }
        public bool Amt2Enable { get; set; }
        public bool Amt3Enable { get; set; }
        public bool Amt4Enable { get; set; }
        public bool Amt5Enable { get; set; }
        public bool Date1Enable { get; set; }
        public bool Date2Enable { get; set; }
        public bool Date3Enable { get; set; }
        public bool Date4Enable { get; set; }
        public bool Date5Enable { get; set; }
        public bool IntervalEnable { get; set; }
        public bool IncomeTypeEnable { get; set; }
        public bool ExcludeEnable { get; set; }
        public bool ExcludeReq { get; set; }
        public bool VerifierReq { get; set; }
        public bool chkIncomeTypeEnable { get; set; }
        public string MenuPropertie { get; set; }
        public string propIncomeTypeSwitch { get; set; }
        public CaseMstEntity CaseMSTEntity { get; set; }
        public string propHourlyMode { get; set; }

        public string StrFormType { get; set; }

        List<CommonEntity> propRelation { get; set; }
        #endregion

        public void fillcombo()
        {
            DataSet ds = null;
            DataTable dt = null;


            //cmbIncomeType.Items.Clear();
            //List<AgyTabEntity> lookUpIncomeTypes = _model.lookupDataAccess.GetIncomeTypes();
            //string[] strIncomeTypes = programDefinitionList.DepIncomeTypes.Split(' ');

            //foreach (AgyTabEntity agyEntity in lookUpIncomeTypes)
            //{
            //    bool boolIncomeType = false;
            //    foreach (string incomeType in strIncomeTypes)
            //    {
            //        if (agyEntity.agycode == incomeType)
            //        {
            //            boolIncomeType = true;
            //        }
            //    }
            //    if (boolIncomeType)
            //        cmbIncomeType.Items.Add(new ListItem(agyEntity.agydesc, agyEntity.agycode, "Y", string.Empty));
            //    else
            //        cmbIncomeType.Items.Add(new ListItem(agyEntity.agydesc, agyEntity.agycode, "N", string.Empty));
            //    if (agyEntity.agydefault.Equals("Y"))
            //        strIncomeDefaultCode = agyEntity.agycode == null ? string.Empty : agyEntity.agycode;

            //}
            //cmbIncomeType.Items.Insert(0, new ListItem("Select One", "0"));
            //SetComboBoxValue(cmbIncomeType, strIncomeDefaultCode);

            if (FLDCNTLHieEntity.Count > 0)
            {
                foreach (FldcntlHieEntity entity in FLDCNTLHieEntity)
                {
                    bool required = entity.Req.Equals("Y") ? true : false;
                    bool enabled = entity.Enab.Equals("Y") ? true : false;

                    switch (entity.FldDesc)
                    {
                        case Consts.CASEINCOME.Verifier:
                            if (enabled) { cmbVerifier.Enabled = lblVerifier.Enabled = true; if (required) lblVerifierReq.Visible = VerifierReq = true; } else { cmbVerifier.Enabled = lblVerifier.Enabled = false; lblVerifierReq.Visible = VerifierReq = false; }
                            break;
                    }
                }
            }

            cmbVerifier.Items.Clear();
            cmbVerifier.ColorMember = "FavoriteColor";
            List<HierarchyEntity> hierarchyEntity = _model.CaseMstData.GetCaseWorker(BaseForm.BaseHierarchyCwFormat, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            string strCaseworker = string.Empty;
            foreach (HierarchyEntity caseworker in hierarchyEntity)
            {
                if (strCaseworker != caseworker.CaseWorker.ToString())
                {
                    strCaseworker = caseworker.CaseWorker.ToString();
                    cmbVerifier.Items.Add(new ListItem(caseworker.HirarchyName.ToString(), caseworker.CaseWorker.ToString(), caseworker.InActiveFlag, caseworker.InActiveFlag.Equals("N") ? Color.Black : Color.Red));
                }
                if (caseworker.UserID.Trim().ToString().ToUpper() == BaseForm.UserID.ToUpper().Trim().ToString())
                {
                    //if (VerifierReq == true)
                    //{
                    strCaseWorkerDefaultCode = caseworker.CaseWorker == null ? "0" : caseworker.CaseWorker;
                    strCaseWorkerDefaultStartCode = caseworker.CaseWorker == null ? "0" : caseworker.CaseWorker;
                    // }
                }

            }
            cmbVerifier.Items.Insert(0, new ListItem("Select One", "0"));
            CommonFunctions.SetComboBoxValue(cmbVerifier, strCaseWorkerDefaultCode);


            //List<CommonEntity> commonEntity = _model.lookupDataAccess.GetIncomeInterval();
            //foreach (CommonEntity interval in commonEntity)
            //{
            //    cmbInterval.Items.Add(new ListItem(interval.Desc, interval.Code));
            //}
            //cmbInterval.Items.Insert(0, new ListItem("    ", "0"));


            //if (programDefinitionList.IncomeDateValidate == "1")
            //{
            //    if (programDefinitionList.DepIncExcIntUsed3 == "1")
            //    {
            //        cmbInterval.Items.Insert(0, new ListItem("90 Days", "9"));
            //        if (programDefinitionList.DepIncExcIntDefault3 == "1")
            //        {
            //            strDefaultValue = "9";
            //        }
            //    }
            //    if (programDefinitionList.DepIncExcIntUsed2 == "1")
            //    {
            //        cmbInterval.Items.Insert(0, new ListItem("60 Days", "6"));
            //        if (programDefinitionList.DepIncExcIntDefault2 == "1")
            //        {
            //            strDefaultValue = "6";
            //        }
            //    }
            //    if (programDefinitionList.DepIncExcIntUsed1 == "1")
            //    {
            //        cmbInterval.Items.Insert(0, new ListItem("30 Days", "3"));
            //        if (programDefinitionList.DepIncExcIntDefault1 == "1")
            //        {
            //            strDefaultValue = "3";
            //        }
            //    }

            //    SetComboBoxValue(cmbInterval, strDefaultValue);
            //}
        }

        List<CommonEntity> VerMethod = new List<CommonEntity>();
        private void fillVerMethod_Combo()
        {
            cmbVerfMethod.Items.Clear();
            cmbVerfMethod.ColorMember = "FavoriteColor";

            VerMethod = _model.lookupDataAccess.GetLookkupFronAGYTAB("00101");
            VerMethod = VerMethod.OrderBy(u => u.Code).ToList();

            cmbVerfMethod.Items.Add(new ListItem("", "00"));

            foreach (CommonEntity entity in VerMethod)
            {
                cmbVerfMethod.Items.Add(new ListItem(entity.Desc, entity.Code, entity.Active, entity.Active.Equals("Y") ? Color.Black : Color.Red));
            }


            if (cmbVerfMethod.Items.Count > 0)
                cmbVerfMethod.SelectedIndex = 0;
        }

        private void fillSnpDetails(bool boolfillcontrols)
        {
            Disableditems = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DISABLED, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);
            if (boolfillcontrols)
            {
                DataGridViewComboBoxColumn cb = (DataGridViewComboBoxColumn)this.dataGridCaseIncome.Columns["Interval"];
                string strHourlymode = "N";
                if (propAgencyControlDetails != null)
                {
                    if (propAgencyControlDetails.IncMethods == "2")
                        strHourlymode = "Y";
                }
                List<CommonEntity> commonInterval = _model.lookupDataAccess.GetIncomeInterval(strHourlymode, propAgencyControlDetails.State.ToString());


                //if (propHourlyMode != "Y")
                //{
                if (programDefinitionList.IncomeDateValidate == "1")
                {
                    //if (programDefinitionList.DepIncExcIntUsed3 == "1")
                    //{
                    //    commonInterval.Insert(0, new CommonEntity("9", "90 Days"));
                    //    if (programDefinitionList.DepIncExcIntDefault3 == "1")
                    //    {
                    //        strDefaultValue = "9";
                    //    }
                    //}
                    //if (programDefinitionList.DepIncExcIntUsed2 == "1")
                    //{
                    //    commonInterval.Insert(0, new CommonEntity("6", "60 Days"));
                    //    if (programDefinitionList.DepIncExcIntDefault2 == "1")
                    //    {
                    //        strDefaultValue = "6";
                    //    }
                    //}
                    if (programDefinitionList.DepIncExcIntUsed1 == "1")
                    {

                        commonInterval.Insert(commonInterval.Count, new CommonEntity("3", "30 Days"));
                        if (programDefinitionList.DepIncExcIntDefault1 == "1")
                        {
                            strDefaultValue = "3";
                        }
                    }
                }

                commonIntervalProp = commonInterval;
                cb.DataSource = commonInterval;
                cb.DisplayMember = "DESC";
                cb.ValueMember = "CODE";
                cb = (DataGridViewComboBoxColumn)this.dataGridCaseIncome.Columns["Interval"];
            }
            calVerificationDate.ValueChanged -= new EventHandler(calVerificationDate_ValueChanged);
            dataGridCaseSnp.SelectionChanged -= new EventHandler(dataGridCaseSnp_SelectionChanged);
            dataGridCaseSnp.Rows.Clear();




            if (BaseForm.BaseCaseSnpEntity != null)
            {
                CaseSnpEntity casesnpApplicant = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq));

                List<CaseSnpEntity> caseSnpMembers = BaseForm.BaseCaseSnpEntity.FindAll(u => u.FamilySeq != BaseForm.BaseCaseMstListEntity[0].FamilySeq);
                int rowIndex = 0;
                if (casesnpApplicant != null)
                {
                    string memberCode = string.Empty;
                    CommonEntity rel = propRelation.Find(u => u.Code.Equals(casesnpApplicant.MemberCode));
                    if (rel != null) memberCode = rel.Desc;
                    string name = LookupDataAccess.GetMemberName(casesnpApplicant.NameixFi, casesnpApplicant.NameixMi, casesnpApplicant.NameixLast, BaseForm.BaseHierarchyCnFormat);
                    string cellTotIncome = !casesnpApplicant.TotIncome.ToString().Equals(string.Empty) ? casesnpApplicant.TotIncome : "0.0";
                    string cellProgIncome = !casesnpApplicant.ProgIncome.Equals(string.Empty) ? casesnpApplicant.ProgIncome : "0.0";

                    cellProgIncome = Decimal.Parse(cellProgIncome).ToString("N", new CultureInfo("en-US"));
                    cellTotIncome = Decimal.Parse(cellTotIncome).ToString("N", new CultureInfo("en-US"));

                    string Age = casesnpApplicant.Age.ToString();
                    string disabled = casesnpApplicant.Disable.ToString();
                    if (!string.IsNullOrEmpty(casesnpApplicant.Disable.Trim()))
                    {
                        if (Disableditems.Count > 0)
                        {
                            foreach (CommonEntity Ent in Disableditems)
                            {
                                if (Ent.Code.Trim() == casesnpApplicant.Disable.Trim())
                                {
                                    disabled = Ent.Desc.Trim();
                                    break;
                                }
                            }
                        }
                    }

                    string SNAP = "No";
                    if (dtFood.Rows.Count > 0 && dtNon_Cash.Rows.Count > 0)
                    {
                        bool IsFood = false; bool Non_Cash = false;
                        foreach (DataRow dr in dtFood.Rows)
                        {
                            if (casesnpApplicant.FootStamps == dr["AGY_1"].ToString().Trim()) { IsFood = true; break; }
                        }
                        foreach (DataRow dr in dtNon_Cash.Rows)
                        {
                            if (casesnpApplicant.NonCashBenefits.Contains(dr["AGY_1"].ToString().Trim())) { Non_Cash = true; break; }
                        }

                        if (IsFood || Non_Cash) SNAP = "Yes";
                    }


                    rowIndex = dataGridCaseSnp.Rows.Add(casesnpApplicant.FamilySeq, name, cellTotIncome, cellProgIncome, memberCode, Age, casesnpApplicant.CurrentAge.ToString(), disabled,SNAP);
                    if (casesnpApplicant.Status.Trim() != "A")
                        dataGridCaseSnp.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    if (casesnpApplicant.Status.Trim() == "A")
                        dataGridCaseSnp.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                    if (casesnpApplicant.Status.Trim() != "A")
                        dataGridCaseSnp.Rows[rowIndex].Cells["MemberName"].Style.ForeColor = Color.Blue;
                    if (casesnpApplicant.Exclude == "Y")
                        dataGridCaseSnp.Rows[rowIndex].Cells["Relation"].Style.ForeColor = Color.Red;
                    dataGridCaseSnp.Rows[rowIndex].Tag = casesnpApplicant;

                    //Kranthi-12/29/2022:: Added tooltip for applicant also // Client intake testing.docx
                    CommonFunctions.setTooltip(rowIndex, casesnpApplicant.AddOperator, casesnpApplicant.DateAdd, casesnpApplicant.LstcOperator, casesnpApplicant.DateLstc, dataGridCaseSnp);
                }
                foreach (CaseSnpEntity caseSnp in caseSnpMembers)
                {
                    string memberCode = string.Empty;
                    CommonEntity rel = propRelation.Find(u => u.Code.Equals(caseSnp.MemberCode));
                    if (rel != null) memberCode = rel.Desc;
                    string name = LookupDataAccess.GetMemberName(caseSnp.NameixFi, caseSnp.NameixMi, caseSnp.NameixLast, BaseForm.BaseHierarchyCnFormat);
                    string strAltDate = LookupDataAccess.Getdate(caseSnp.AltBdate);
                    string strSsno = LookupDataAccess.GetCardNo(caseSnp.Ssno, "1", programDefinitionList.SSNReasonFlag.Trim(), caseSnp.SsnReason);
                    string cellTotIncome = !caseSnp.TotIncome.ToString().Equals(string.Empty) ? caseSnp.TotIncome : "0.0";
                    string cellProgIncome = !caseSnp.ProgIncome.Equals(string.Empty) ? caseSnp.ProgIncome : "0.0";

                    cellProgIncome = Decimal.Parse(cellProgIncome).ToString("N", new CultureInfo("en-US"));
                    cellTotIncome = Decimal.Parse(cellTotIncome).ToString("N", new CultureInfo("en-US"));


                    string Age = caseSnp.Age.ToString();
                    string disabled = caseSnp.Disable.ToString();
                    if (!string.IsNullOrEmpty(caseSnp.Disable.Trim()))
                    {
                        if (Disableditems.Count > 0)
                        {
                            foreach (CommonEntity Ent in Disableditems)
                            {
                                if (Ent.Code.Trim() == caseSnp.Disable.Trim())
                                {
                                    disabled = Ent.Desc.Trim();
                                    break;
                                }
                            }
                        }
                    }

                    string SNAP = "No";
                    if (dtFood.Rows.Count > 0 && dtNon_Cash.Rows.Count > 0)
                    {
                        bool IsFood = false; bool Non_Cash = false;
                        foreach (DataRow dr in dtFood.Rows)
                        {
                            if (caseSnp.FootStamps == dr["AGY_1"].ToString().Trim()) { IsFood = true; break; }
                        }
                        foreach (DataRow dr in dtNon_Cash.Rows)
                        {
                            if (caseSnp.NonCashBenefits.Contains(dr["AGY_1"].ToString().Trim())) { Non_Cash = true; break; }
                        }

                        if (IsFood || Non_Cash) SNAP = "Yes";
                    }

                    rowIndex = dataGridCaseSnp.Rows.Add(caseSnp.FamilySeq, name, cellTotIncome, cellProgIncome, memberCode, Age, caseSnp.CurrentAge.ToString(), disabled,SNAP);
                    dataGridCaseSnp.Rows[rowIndex].Tag = caseSnp;
                    if (caseSnp.Status.Trim() != "A")
                        dataGridCaseSnp.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    if (caseSnp.Exclude == "Y")
                        dataGridCaseSnp.Rows[rowIndex].Cells["Relation"].Style.ForeColor = Color.Red;

                    CommonFunctions.setTooltip(rowIndex, caseSnp.AddOperator, caseSnp.DateAdd, caseSnp.LstcOperator, caseSnp.DateLstc, dataGridCaseSnp);
                }

                dataGridCaseSnp.Rows[0].Selected = true;
            }
            calVerificationDate.ValueChanged -= new EventHandler(calVerificationDate_ValueChanged);
            if (BaseForm.BaseCaseMstListEntity.Count > 0)
            {

                //txtInHouse.Text = BaseForm.BaseCaseMstListEntity[0].NoInhh.ToString();
                lblTotIncome.Text = decimal.Parse(BaseForm.BaseCaseMstListEntity[0].FamIncome.ToString()).ToString("N", new CultureInfo("en-US"));
                //txtInProg.Text = BaseForm.BaseCaseMstListEntity[0].NoInProg.ToString();
                lblProgIncome.Text = decimal.Parse(BaseForm.BaseCaseMstListEntity[0].ProgIncome.ToString()).ToString("N", new CultureInfo("en-US"));
                MstIntakeDate = BaseForm.BaseCaseMstListEntity[0].IntakeDate.ToString();
                txtHud.Text = BaseForm.BaseCaseMstListEntity[0].Hud.ToString();
                txtCmi.Text = BaseForm.BaseCaseMstListEntity[0].Cmi.ToString();
                txtSmi.Text = BaseForm.BaseCaseMstListEntity[0].Smi.ToString();
                if (BaseForm.BaseAgencyControlDetails.ACR_POV_WITH_DEC == "Y")
                    txtFedOmb.Text = BaseForm.BaseCaseMstListEntity[0].Poverty.ToString();
                else if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].Poverty.ToString().Trim()))
                    txtFedOmb.Text = Math.Abs((int)decimal.Parse(BaseForm.BaseCaseMstListEntity[0].Poverty.ToString())).ToString();

                //txtFedOmb.Text = BaseForm.BaseCaseMstListEntity[0].Poverty.ToString();
                if (BaseForm.BaseCaseMstListEntity[0].EligDate != string.Empty)
                {
                    calVerificationDate.Value = Convert.ToDateTime(BaseForm.BaseCaseMstListEntity[0].EligDate);
                    //calVerificationDate.Checked = true;
                }



            }
            calVerificationDate.ValueChanged += new EventHandler(calVerificationDate_ValueChanged);
            dataGridCaseSnp.SelectionChanged += new EventHandler(dataGridCaseSnp_SelectionChanged);

            CaseVerEntity caseVerList = _model.CaseMstData.GetCaseveradpynd(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, calVerificationDate.Value.ToString(), string.Empty);
            if (caseVerList != null)
            {
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
            }
        }

        private void dataGridCaseSnp_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                cmbVerifier.SelectedIndexChanged -= new EventHandler(cmbVerifier_SelectedIndexChanged);
                cmbVerfMethod.SelectedIndexChanged -= new EventHandler(cmbVerfMethod_SelectedIndexChanged);
                if (dataGridCaseSnp.SelectedRows.Count > 0)
                {
                    btnSave.Visible = false;
                    lbl18Years.Visible = false;
                    btnCancel.Visible = false;
                    MenuPropertie = string.Empty;
                    lblMstIntakeDate.Text = string.Empty;
                    dataGridCaseIncome.ReadOnly = true;
                    txtHowVerified.Enabled = lblHowVerified.Enabled = cmbVerifier.Enabled = txtIncSource.Enabled = lblIncomeSource.Enabled = lblVerifier.Enabled = lblVerifierReq.Visible = lblIncomeSourceReq.Visible = lblReqHowverified.Visible = calVerificationDate.Enabled = false;

                    CaseSnpEntity row = dataGridCaseSnp.SelectedRows[0].Tag as CaseSnpEntity;
                    if (row != null)
                    {
                        string strFamilySeq = row.FamilySeq;
                        //strFamilySeq = row.FamilySeq;
                        if (propHourlyMode == "Y")
                        {
                            //if (BaseForm.BaseCaseMstListEntity[0].FamilySeq == strFamilySeq)
                            //{
                            if (row.Age != string.Empty)
                            {
                                if (Convert.ToInt32(row.Age) < 18)
                                {
                                    lbl18Years.Visible = true;
                                }
                            }
                            //}
                        }

                        strSnpIndex = dataGridCaseSnp.SelectedRows[0].Index;
                        List<CaseIncomeEntity> caseIncomeList = _model.CaseMstData.GetCaseIncomeadpynf(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, strFamilySeq);
                        caseIncomeList = caseIncomeList.OrderBy(u => u.Interval).ToList();
                        dataGridCaseIncome.CellValueChanged -= new DataGridViewCellEventHandler(dataGridCaseIncome_CellValueChanged);
                        dataGridCaseIncome.SelectionChanged -= new EventHandler(dataGridCaseIncome_SelectionChanged);
                        dataGridCaseIncome.Rows.Clear();
                        bool boolExclude = false;
                        if (caseIncomeList.Count == 0)
                        {
                            txtHowVerified.Text = string.Empty;
                            txtIncSource.Text = string.Empty;
                            CommonFunctions.SetComboBoxValue(cmbVerifier, strCaseWorkerDefaultCode);
                        }
                        foreach (CaseIncomeEntity caseIncome in caseIncomeList)
                        {
                            boolExclude = false;
                            string strInterval = caseIncome.Interval;
                            string strTotIncome = caseIncome.TotIncome;
                            if (caseIncome.Exclude == "Y")
                                boolExclude = true;
                            //if (propHourlyMode != "Y")
                            //{
                            if (caseIncome.Interval == "3" || caseIncome.Interval == "6" || caseIncome.Interval == "9")
                            {
                                strInterval = string.Empty;
                                if (programDefinitionList.IncomeDateValidate == "1")
                                {
                                    if (programDefinitionList.DepIncExcIntUsed3 == "1" && caseIncome.Interval == "9")
                                    {
                                        strInterval = "9";
                                    }
                                    if (programDefinitionList.DepIncExcIntUsed2 == "1" && caseIncome.Interval == "6")
                                    {
                                        strInterval = "6";
                                    }
                                    if (programDefinitionList.DepIncExcIntUsed1 == "1" && caseIncome.Interval == "3")
                                    {
                                        strInterval = "3";
                                    }
                                }
                            }
                            //}
                            if (strInterval == string.Empty)
                                strTotIncome = string.Empty;
                            int rowIndex = dataGridCaseIncome.Rows.Add(true, caseIncome.FamilySeq, caseIncome.Seq, caseIncome.Type, LookupDataAccess.GetLookUpCode("00004", caseIncome.Type), strInterval, boolExclude, caseIncome.Val1, LookupDataAccess.Getdate(caseIncome.Date1), caseIncome.Val2, LookupDataAccess.Getdate(caseIncome.Date2), caseIncome.Val3, LookupDataAccess.Getdate(caseIncome.Date3), caseIncome.Val4, LookupDataAccess.Getdate(caseIncome.Date4), caseIncome.Val5, LookupDataAccess.Getdate(caseIncome.Date5), caseIncome.Factor, string.Empty, string.Empty, strTotIncome, "U", caseIncome.Verifier, caseIncome.HowVerified, caseIncome.Source, caseIncome.HrRate1, caseIncome.HrRate2, caseIncome.HrRate3, caseIncome.HrRate4, caseIncome.HrRate5);

                            //Added by Sudheer on 08/30/24 as per CRT 2025 document
                            dataGridCaseIncome.Rows[rowIndex].Cells["Column1"].Value = caseIncome.VerMethod;

                            //Kranthi-12/29/2022:: Added tooltip for income records also also // Client intake testing.docx
                            CommonFunctions.setTooltip(rowIndex, caseIncome.AddOperator, caseIncome.DateAdd, caseIncome.LstcOperator, caseIncome.DateLstc, dataGridCaseIncome);

                            CommonEntity agytabsdata = lookUpIncomeTypes.Find(u => u.Code == caseIncome.Type);
                            if (agytabsdata != null)
                            {
                                dataGridCaseIncome.Rows[rowIndex].Cells["gvtDeducation"].Value = agytabsdata.Extension;
                                if (agytabsdata.Active != "Y")
                                    dataGridCaseIncome.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                            }
                            else
                            {
                                dataGridCaseIncome.Rows[rowIndex].Cells["gvtDeducation"].Value = string.Empty;
                            }
                            dataGridCaseIncome.Rows[rowIndex].Tag = caseIncome;
                            //dataGridCaseIncome.ItemsPerPage = 100;
                            //  CommonFunctions.setTooltip(rowIndex, caseIncome.AddOperator, caseIncome.DateAdd, caseIncome.LstcOperator, caseIncome.DateLstc, dataGridCaseIncome);
                            txtHowVerified.Text = caseIncome.HowVerified;
                            txtIncSource.Text = caseIncome.Source;
                            CommonFunctions.SetComboBoxValue(cmbVerifier, caseIncome.Verifier);
                            
                            CommonFunctions.SetComboBoxValue(cmbVerfMethod, caseIncome.VerMethod);
                            ShowFactor(caseIncome.Interval, rowIndex, strMode);
                            if (propHourlyMode.ToUpper() == "Y")
                            {
                                ShowDateLabelDisplay(-29);
                            }
                            else
                            {
                                if (caseIncome.Interval == "3")
                                    ShowDateLabelDisplay(-29);
                                else if (caseIncome.Interval == "6")
                                    ShowDateLabelDisplay(-59);
                                else if (caseIncome.Interval == "9")
                                    ShowDateLabelDisplay(-89);
                                else
                                    lblMstIntakeDate.Text = string.Empty;
                            }
                            CalculationAmount(caseIncome.Interval, rowIndex);
                            if (strInterval == string.Empty)
                                dataGridCaseIncome.Rows[rowIndex].Cells["Total"].Value = string.Empty;
                        }
                        if (dataGridCaseIncome.Rows.Count > 0)
                        {
                            //btnAdd.SendToBack();
                            //btnEdit.BringToFront();
                            btnEdit.Enabled = true;
                            btnAdd.Enabled = true;

                            btnEdit.Visible = true;
                            btnAdd.Visible = true;
                        }

                        else
                        {
                            //btnEdit.SendToBack();
                            //btnAdd.BringToFront();
                            btnEdit.Enabled = false;
                            btnAdd.Enabled = true;

                            btnEdit.Visible = false;
                            btnAdd.Visible = true;
                            CommonFunctions.SetComboBoxValue(cmbVerifier, "0");
                        }
                        RequireAllGridColors();
                        dataGridCaseIncome.CellValueChanged += new DataGridViewCellEventHandler(dataGridCaseIncome_CellValueChanged);
                        dataGridCaseIncome.SelectionChanged += new EventHandler(dataGridCaseIncome_SelectionChanged);
                        if (dataGridCaseIncome.Rows.Count > 0)
                        {
                            dataGridCaseIncome_SelectionChanged(sender, e);
                        }
                    }
                    if (MenuPropertie == string.Empty)
                    {
                        if (Privileges.DelPriv.Equals("false"))
                        {
                            dataGridCaseIncome.Columns["Delete"].Visible = false;
                        }
                        else
                            dataGridCaseIncome.Columns["Delete"].Visible = true;
                    }
                }
                cmbVerifier.SelectedIndexChanged += new EventHandler(cmbVerifier_SelectedIndexChanged);
                cmbVerfMethod.SelectedIndexChanged += new EventHandler(cmbVerfMethod_SelectedIndexChanged);

            }
            catch (Exception ex)
            {

            }
        }

        private void CaseIncomeForm2_Load(object sender, EventArgs e)
        {
            dataGridCaseSnp_SelectionChanged(sender, e);
            if (propIncomeTypeSwitch != "Y")
            {
                if (!FLDCNTLHieEntity.Exists(u => u.Enab.Equals("Y")))
                {
                    CommonFunctions.MessageBoxDisplay("Field controls not defined for this program");
                    btnSave.Visible = false;
                    btnCancel.Text = "Close";
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;

                }
            }
            check_under18NoIncomeMembers();
        }

        private void ShowFactor(string strInterval, int rowindex, string mode)
        {
            // dataGridCaseIncome.CellValueChanged -= new DataGridViewCellEventHandler(dataGridCaseIncome_CellValueChanged);
            if (propHourlyMode.ToUpper() == "Y")
            {
                if (strInterval == "3")
                {
                    dataGridCaseIncome.Rows[rowindex].Cells["Factor"].Value = programDefinitionList.DepIncExcIntFactr1;
                }
                else
                {
                    dataGridCaseIncome.Rows[rowindex].Cells["Factor"].Value = "1.00";
                }
                //else if (strInterval == "6")
                //{
                //    dataGridCaseIncome.Rows[rowindex].Cells["Factor"].Value = programDefinitionList.DepIncExcIntFactr2;
                //    ShowDateLabelDisplay(-60);
                //}
                //else if (strInterval == "9")
                //{
                //    dataGridCaseIncome.Rows[rowindex].Cells["Factor"].Value = programDefinitionList.DepIncExcIntFactr3;
                //    ShowDateLabelDisplay(-90);
                //}

                ShowDateLabelDisplay(-29);
            }
            else
            {
                lblMstIntakeDate.Text = string.Empty;
                if (strInterval == "Q")
                {
                    dataGridCaseIncome.Rows[rowindex].Cells["Factor"].Value = "4.00";
                }
                else if (strInterval == "N")
                {
                    dataGridCaseIncome.Rows[rowindex].Cells["Factor"].Value = "2.00";
                }
                else if (strInterval == "S" || strInterval == "M")
                {
                    dataGridCaseIncome.Rows[rowindex].Cells["Factor"].Value = "12.00";
                }
                else if (strInterval == "A" || strInterval == "O" || strInterval == "E" || strInterval == "H")
                {
                    dataGridCaseIncome.Rows[rowindex].Cells["Factor"].Value = "1.00";
                }
                else if (strInterval == "W" || strInterval == "B")
                {
                    if (programDefinitionList.IncomeWeek == "Y")
                        dataGridCaseIncome.Rows[rowindex].Cells["Factor"].Value = "12.00";
                    else
                        dataGridCaseIncome.Rows[rowindex].Cells["Factor"].Value = "13.00";
                }
                else if (string.IsNullOrEmpty(strInterval))
                {
                    if (strMode != string.Empty)
                        dataGridCaseIncome.Rows[rowindex].Cells["Factor"].Value = string.Empty;
                }
                else if (strInterval == "3")
                {
                    dataGridCaseIncome.Rows[rowindex].Cells["Factor"].Value = programDefinitionList.DepIncExcIntFactr1;
                    ShowDateLabelDisplay(-29);

                }
                else if (strInterval == "6")
                {
                    dataGridCaseIncome.Rows[rowindex].Cells["Factor"].Value = programDefinitionList.DepIncExcIntFactr2;
                    ShowDateLabelDisplay(-59);
                }
                else if (strInterval == "9")
                {
                    dataGridCaseIncome.Rows[rowindex].Cells["Factor"].Value = programDefinitionList.DepIncExcIntFactr3;
                    ShowDateLabelDisplay(-89);
                }
                // dataGridCaseIncome.CellValueChanged += new DataGridViewCellEventHandler(dataGridCaseIncome_CellValueChanged);

            }
        }

        private void ShowDateLabelDisplay(int intdays)
        {
            if (propHourlyMode.ToUpper() == "Y")
            {

                if (string.IsNullOrEmpty(MstIntakeDate))// == "" || MstIntakeDate == null)
                {
                    MstIntakeEndDate = DateTime.Now;
                    MstIntakeStartDate = MstIntakeEndDate.AddDays(-29);
                    lblMstIntakeDate.Text = "Intake Date: " + " | 30 Day Window: ......... - " + LookupDataAccess.Getdate(MstIntakeStartDate.ToShortDateString()) + "";
                }
                else
                {
                    if (BaseForm.BaseAgencyControlDetails.AgyShortName == "CACOST")
                    {
                        MstIntakeEndDate = Convert.ToDateTime(MstIntakeDate).AddDays(-1);
                        MstIntakeStartDate = MstIntakeEndDate.AddDays(-29);
                        lblMstIntakeDate.Text = "Intake Date: " + LookupDataAccess.Getdate(MstIntakeDate.ToString()) + " | 30 Day Window: " + LookupDataAccess.Getdate(Convert.ToDateTime(MstIntakeEndDate).ToShortDateString()) + " - " + LookupDataAccess.Getdate(MstIntakeStartDate.ToShortDateString()) + "";
                    }
                    else
                    {
                        MstIntakeEndDate = Convert.ToDateTime(MstIntakeDate);//.AddDays(-1);
                        MstIntakeStartDate = MstIntakeEndDate.AddDays(-29);
                        lblMstIntakeDate.Text = "Intake Date: " + LookupDataAccess.Getdate(MstIntakeDate.ToString()) + " | 30 Day Window: " + LookupDataAccess.Getdate(Convert.ToDateTime(MstIntakeEndDate).ToShortDateString()) + " - " + LookupDataAccess.Getdate(MstIntakeStartDate.ToShortDateString()) + "";
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(MstIntakeDate))// == "" || MstIntakeDate == null)
                {
                    MstIntakeEndDate = DateTime.Now;
                    MstIntakeStartDate = MstIntakeEndDate.AddDays(intdays);
                    lblMstIntakeDate.Text = "Intake date : ......... window date : " + MstIntakeStartDate.ToShortDateString() + "";
                }
                else
                {
                    MstIntakeEndDate = Convert.ToDateTime(MstIntakeDate);
                    MstIntakeStartDate = MstIntakeEndDate.AddDays(intdays);
                    lblMstIntakeDate.Text = "Intake date : " + Convert.ToDateTime(MstIntakeDate).ToShortDateString() + " window date : " + MstIntakeStartDate.ToShortDateString() + "";
                }
            }
        }



        private void dataGridCaseIncome_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {


                dataGridCaseIncome.CellValueChanged -= new DataGridViewCellEventHandler(dataGridCaseIncome_CellValueChanged);
                if (e.ColumnIndex == Interval.Index)
                {
                    bool boolvalid = true;
                    int introwindex = dataGridCaseIncome.CurrentCell.RowIndex;
                    string strIntervalValue = string.Empty;//Convert.ToString(dataGridCaseIncome.Rows[introwindex].Cells["Interval"].Value);
                    //ADDED BY SUDHEER ON 01/16/2020
                    if (dataGridCaseIncome.Rows[introwindex].Cells["Interval"].Value != null)
                    {
                        if (!string.IsNullOrEmpty(dataGridCaseIncome.Rows[introwindex].Cells["Interval"].Value.ToString().Trim()))
                            strIntervalValue = dataGridCaseIncome.Rows[introwindex].Cells["Interval"].Value.ToString();
                    }

                    if (propHourlyMode.ToUpper() == "Y")
                    {

                        if ((strIntervalValue == "3" || strIntervalValue == "6" || strIntervalValue == "9") && (strMode.ToUpper() == "ADD"))
                        {
                            CommonFunctions.MessageBoxDisplay("30 days interval not allowed.");//or 60 or 90
                            dataGridCaseIncome.Rows[introwindex].Cells["Interval"].Value = string.Empty;
                            strIntervalValue = string.Empty;
                        }
                        else
                        {
                            if ((strIntervalValue == "3" || strIntervalValue == "6" || strIntervalValue == "9") && (strMode.ToUpper() == "EDIT"))
                            {
                                CommonFunctions.MessageBoxDisplay("30 days interval not allowed.");//or 60 or 90
                                dataGridCaseIncome.Rows[introwindex].Cells["Interval"].Value = string.Empty;
                                strIntervalValue = string.Empty;
                                boolvalid = false;
                            }
                        }

                    }
                    if (boolvalid)
                    {
                        changeAllFieldsClear(introwindex);

                        disableenableFields(strIntervalValue, introwindex);

                        ShowFactor(strIntervalValue, introwindex, strMode);


                        //dataGridCaseIncome.RefreshEdit();
                        dataGridCaseIncome.Update();
                        HourlyEnableControl(strIntervalValue);
                    }

                }
                else if (e.ColumnIndex == Amt1.Index || e.ColumnIndex == Amt2.Index || e.ColumnIndex == Amt3.Index || e.ColumnIndex == Amt4.Index || e.ColumnIndex == Amt5.Index)
                {
                    int introwindex = dataGridCaseIncome.CurrentCell.RowIndex;
                    string strIntervalValue = Convert.ToString(dataGridCaseIncome.Rows[introwindex].Cells["Interval"].Value);
                    int intcolumnindex = dataGridCaseIncome.CurrentCell.ColumnIndex;
                    string strAmtValue = Convert.ToString(dataGridCaseIncome.Rows[introwindex].Cells[intcolumnindex].Value);

                    #region Vikash added on 09/01/2023 to check numbers entered ending with '.' and '.0' as per small things document
                    if (strAmtValue.EndsWith("."))
                    {
                        strAmtValue = Convert.ToString(dataGridCaseIncome.Rows[introwindex].Cells[intcolumnindex].Value.ToString().Replace(".", ""));
                    }
                    else if (strAmtValue.EndsWith(".0"))
                    {
                        strAmtValue = Convert.ToString(dataGridCaseIncome.Rows[introwindex].Cells[intcolumnindex].Value.ToString().Replace(".0", ""));
                    }
                    else
                    {
                        strAmtValue = Convert.ToString(dataGridCaseIncome.Rows[introwindex].Cells[intcolumnindex].Value);
                    }
                    #endregion

                    dataGridCaseIncome.Rows[introwindex].Cells["Amt1"].Selected = true;
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
                                    dataGridCaseIncome.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                    CommonFunctions.MessageBoxDisplay("Only two decimal values are allowed");//CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals);
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
                                            dataGridCaseIncome.Rows[introwindex].Cells[intcolumnindex].Value = "999999.99";
                                            // CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals6Range);
                                        }
                                    }
                                    else
                                    {
                                        if (System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.NumericString))
                                        {
                                            dataGridCaseIncome.Rows[introwindex].Cells[intcolumnindex].Value = strAmtValue + ".00";
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.TwoDecimalString))
                            {
                                dataGridCaseIncome.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                /*AlertBox.Show("Please enter value properly");//*/
                                CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals);
                                //InvokeFocusCommand(dataGridCaseIncome);
                                //dataGridCaseIncome.BeginEdit(true);
                                //dataGridCaseIncome.Rows[introwindex].Cells["Amt1"].Selected = true;
                            }
                        }
                    }

                    if (strIntervalValue != string.Empty)
                        CalculationAmount(strIntervalValue, introwindex);

                }
                else if (e.ColumnIndex == Date1.Index || e.ColumnIndex == Date2.Index || e.ColumnIndex == Date3.Index || e.ColumnIndex == Date4.Index || e.ColumnIndex == Date5.Index)
                {
                    int introwindex = dataGridCaseIncome.CurrentCell.RowIndex;
                    int intcolumnindex = dataGridCaseIncome.CurrentCell.ColumnIndex;
                    string strIntervalValue = Convert.ToString(dataGridCaseIncome.Rows[introwindex].Cells["Interval"].Value);
                    string strCurrectValue = Convert.ToString(dataGridCaseIncome.Rows[introwindex].Cells[intcolumnindex].Value);

                    if (strCurrectValue != "" && strCurrectValue != null)
                        strCurrectValue = Convert.ToDateTime(strCurrectValue).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

                    if (strCurrectValue.Length > 10)
                    {
                        strCurrectValue = strCurrectValue.Substring(0, 10);
                    }


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
                                    CommonFunctions.MessageBoxDisplay("01/01/1800 below date not except");
                                    dataGridCaseIncome.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;

                                }
                                else
                                {
                                    dataGridCaseIncome.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                    CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEntermmddyyDateFormat);

                                }

                            }
                            catch (Exception)
                            {
                                dataGridCaseIncome.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEntermmddyyDateFormat);

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
                                dataGridCaseIncome.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEntermmddyyDateFormat);
                                booldatevalid = false;
                            }


                            if (strIntervalValue == "W")
                            {
                                if (booldatevalid)
                                {
                                    if (e.ColumnIndex == Date1.Index)
                                    {
                                        if (propHourlyMode == "Y")
                                        {

                                            //if (dataGridCaseIncome.Rows[introwindex].Cells["Status"].Value.ToString() != "U")
                                            //{
                                            dataGridCaseIncome.Rows[introwindex].Cells["Date1"].Value = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).Date.ToShortDateString());
                                            //string strdate = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).AddDays(7).Date.ToShortDateString());
                                            //this.Date2.TextMaskFormat = MaskFormat.IncludePrompt;
                                            dataGridCaseIncome.Rows[introwindex].Cells["Date2"].Value = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).AddDays(-7).Date.ToShortDateString());
                                            //this.Date2.TextMaskFormat = MaskFormat.IncludePrompt;
                                            dataGridCaseIncome.Rows[introwindex].Cells["Date3"].Value = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).AddDays(-14).Date.ToShortDateString());
                                            dataGridCaseIncome.Rows[introwindex].Cells["Date4"].Value = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).AddDays(-21).Date.ToShortDateString());
                                            if (propHourlyMode != "Y")
                                                dataGridCaseIncome.Rows[introwindex].Cells["Date5"].Value = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).AddDays(-28).Date.ToShortDateString());
                                        }
                                        else
                                        {
                                            dataGridCaseIncome.Rows[introwindex].Cells["Date1"].Value = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).Date.ToShortDateString());
                                            //string strdate = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).AddDays(7).Date.ToShortDateString());
                                            //this.Date2.TextMaskFormat = MaskFormat.IncludePrompt;
                                            dataGridCaseIncome.Rows[introwindex].Cells["Date2"].Value = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).AddDays(7).Date.ToShortDateString());
                                            //this.Date2.TextMaskFormat = MaskFormat.IncludePrompt;
                                            dataGridCaseIncome.Rows[introwindex].Cells["Date3"].Value = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).AddDays(14).Date.ToShortDateString());
                                            dataGridCaseIncome.Rows[introwindex].Cells["Date4"].Value = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).AddDays(21).Date.ToShortDateString());
                                            if (propHourlyMode != "Y")
                                                dataGridCaseIncome.Rows[introwindex].Cells["Date5"].Value = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).AddDays(28).Date.ToShortDateString());
                                        }
                                        // }
                                    }
                                }

                            }

                            if (strIntervalValue == "B" && propHourlyMode=="Y")
                            {
                                if (booldatevalid)
                                {
                                    if (e.ColumnIndex == Date1.Index)
                                    {
                                        if (propHourlyMode == "Y")
                                        {
                                            dataGridCaseIncome.Rows[introwindex].Cells["Date1"].Value = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).Date.ToShortDateString());
                                            //string strdate = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).AddDays(7).Date.ToShortDateString());
                                            //this.Date2.TextMaskFormat = MaskFormat.IncludePrompt;
                                            //we changed dates from 7 to 14 days as per the RPMC.docx on 02/24/25 Sudheer
                                            dataGridCaseIncome.Rows[introwindex].Cells["Date2"].Value = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).AddDays(-14).Date.ToShortDateString());
                                            //this.Date2.TextMaskFormat = MaskFormat.IncludePrompt;
                                            dataGridCaseIncome.Rows[introwindex].Cells["Date3"].Value = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).AddDays(-28).Date.ToShortDateString());
                                            //dataGridCaseIncome.Rows[introwindex].Cells["Date4"].Value = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).AddDays(21).Date.ToShortDateString());
                                            //if (propHourlyMode != "Y")
                                            //    dataGridCaseIncome.Rows[introwindex].Cells["Date5"].Value = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).AddDays(28).Date.ToShortDateString());
                                        }
                                        else
                                        {
                                            dataGridCaseIncome.Rows[introwindex].Cells["Date1"].Value = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).Date.ToShortDateString());
                                            //string strdate = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).AddDays(7).Date.ToShortDateString());
                                            //this.Date2.TextMaskFormat = MaskFormat.IncludePrompt;
                                            dataGridCaseIncome.Rows[introwindex].Cells["Date2"].Value = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).AddDays(7).Date.ToShortDateString());
                                            //this.Date2.TextMaskFormat = MaskFormat.IncludePrompt;
                                            dataGridCaseIncome.Rows[introwindex].Cells["Date3"].Value = LookupDataAccess.Getdate(Convert.ToDateTime(strCurrectValue).AddDays(14).Date.ToShortDateString());

                                        }
                                    }
                                }

                            }



                            //if (strIntervalValue != string.Empty)
                            //    //  dataGridCaseIncome.Rows[introwindex].Cells[intcolumnindex].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                            //    if (strIntervalValue == "3" || strIntervalValue == "6" || strIntervalValue == "9")
                            //    {

                            //        // dataGridCaseIncome.Rows[introwindex].Cells[intcolumnindex].Style.ForeColor = System.Drawing.Color.Black;
                            //        if (!CheckMstIntakeDate(Convert.ToDateTime(strCurrectValue).Date, strIntervalValue))
                            //            //   dataGridCaseIncome.Rows[introwindex].Cells[intcolumnindex].Style.ForeColor = System.Drawing.Color.Red;
                            //            dataGridCaseIncome.Rows[introwindex].Cells[intcolumnindex].Update();
                            //    }
                        }
                    }

                    if (strIntervalValue != string.Empty)
                    {
                        if (propHourlyMode.ToUpper() == "Y")
                        {
                            CalculationAmount(strIntervalValue, introwindex);
                        }
                        else
                        {
                            //  dataGridCaseIncome.Rows[introwindex].Cells[intcolumnindex].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                            if (strIntervalValue == "3" || strIntervalValue == "6" || strIntervalValue == "9")
                            {
                                CalculationAmount(strIntervalValue, introwindex);
                            }
                        }
                    }
                }
                dataGridCaseIncome.CellValueChanged += new DataGridViewCellEventHandler(dataGridCaseIncome_CellValueChanged);
            }
            catch (Exception ex)
            {

            }
        }

        private void CalculationAmount(string strInterval, int rowcalindex)
        {
            int intamtcount = 0;
            int intTotalChecks = 0;
            decimal decAmount1 = 0;
            decimal decAmount2 = 0;
            decimal decAmount3 = 0;
            decimal decAmount4 = 0;
            decimal decAmount5 = 0;
            decimal decTotalHourly = 0;
            decimal decAvgTotal = 0;
            if (propHourlyMode.ToUpper() == "Y")
            {

                if (dataGridCaseIncome.Rows[rowcalindex].Cells["Amt1"].Value != string.Empty)
                {
                    if (dataGridCaseIncome.Rows[rowcalindex].Cells["Date1"].Value != string.Empty && dataGridCaseIncome.Rows[rowcalindex].Cells["Date1"].Value.ToString().Trim() != "/  /")
                    {
                        if (!CheckMstIntakeDate(Convert.ToDateTime(LookupDataAccess.GetFormatdate(dataGridCaseIncome.Rows[rowcalindex].Cells["Date1"].Value.ToString())).Date, strInterval))
                        {
                            decAmount1 = 0;
                            dataGridCaseIncome.Rows[rowcalindex].Cells["Date1"].Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            dataGridCaseIncome.Rows[rowcalindex].Cells["Date1"].Style.ForeColor = Color.Black;
                            decAmount1 = Convert.ToDecimal(dataGridCaseIncome.Rows[rowcalindex].Cells["Amt1"].Value);
                            if (txtHr1.Text != string.Empty)
                                decTotalHourly = decTotalHourly + Convert.ToDecimal(txtHr1.Text);
                            intTotalChecks = intTotalChecks + 1;
                        }
                    }
                }
                if (dataGridCaseIncome.Rows[rowcalindex].Cells["Amt2"].Value != string.Empty)
                {
                    if (dataGridCaseIncome.Rows[rowcalindex].Cells["Date2"].Value != string.Empty && dataGridCaseIncome.Rows[rowcalindex].Cells["Date2"].Value.ToString().Trim() != "/  /")
                    {
                        if (!CheckMstIntakeDate(Convert.ToDateTime(LookupDataAccess.GetFormatdate(dataGridCaseIncome.Rows[rowcalindex].Cells["Date2"].Value.ToString())).Date, strInterval))
                        {
                            decAmount2 = 0;
                            dataGridCaseIncome.Rows[rowcalindex].Cells["Date2"].Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            dataGridCaseIncome.Rows[rowcalindex].Cells["Date2"].Style.ForeColor = Color.Black;
                            decAmount2 = Convert.ToDecimal(dataGridCaseIncome.Rows[rowcalindex].Cells["Amt2"].Value);
                            intTotalChecks = intTotalChecks + 1;
                            if (txtHr2.Text != string.Empty)
                                decTotalHourly = decTotalHourly + Convert.ToDecimal(txtHr2.Text);
                        }
                    }
                }
                if (dataGridCaseIncome.Rows[rowcalindex].Cells["Amt3"].Value != string.Empty)
                {
                    if (dataGridCaseIncome.Rows[rowcalindex].Cells["Date3"].Value != string.Empty && dataGridCaseIncome.Rows[rowcalindex].Cells["Date3"].Value.ToString().Trim() != "/  /")
                    {
                        if (!CheckMstIntakeDate(Convert.ToDateTime(LookupDataAccess.GetFormatdate(dataGridCaseIncome.Rows[rowcalindex].Cells["Date3"].Value.ToString())).Date, strInterval))
                        {
                            decAmount3 = 0;
                            dataGridCaseIncome.Rows[rowcalindex].Cells["Date3"].Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            dataGridCaseIncome.Rows[rowcalindex].Cells["Date3"].Style.ForeColor = Color.Black;
                            decAmount3 = Convert.ToDecimal(dataGridCaseIncome.Rows[rowcalindex].Cells["Amt3"].Value);
                            intTotalChecks = intTotalChecks + 1;
                            if (txtHr3.Text != string.Empty)
                                decTotalHourly = decTotalHourly + Convert.ToDecimal(txtHr3.Text);
                        }
                    }
                }
                if (dataGridCaseIncome.Rows[rowcalindex].Cells["Amt4"].Value != string.Empty)
                {
                    if (dataGridCaseIncome.Rows[rowcalindex].Cells["Date4"].Value != string.Empty && dataGridCaseIncome.Rows[rowcalindex].Cells["Date4"].Value.ToString().Trim() != "/  /")
                    {
                        if (!CheckMstIntakeDate(Convert.ToDateTime(LookupDataAccess.GetFormatdate(dataGridCaseIncome.Rows[rowcalindex].Cells["Date4"].Value.ToString())).Date, strInterval))
                        {
                            decAmount4 = 0;
                            dataGridCaseIncome.Rows[rowcalindex].Cells["Date4"].Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            dataGridCaseIncome.Rows[rowcalindex].Cells["Date4"].Style.ForeColor = Color.Black;
                            decAmount4 = Convert.ToDecimal(dataGridCaseIncome.Rows[rowcalindex].Cells["Amt4"].Value);
                            intTotalChecks = intTotalChecks + 1;
                            if (txtHr4.Text != string.Empty)
                                decTotalHourly = decTotalHourly + Convert.ToDecimal(txtHr4.Text);
                        }
                    }
                }
                if (dataGridCaseIncome.Rows[rowcalindex].Cells["Amt5"].Value != string.Empty)
                {
                    if (dataGridCaseIncome.Rows[rowcalindex].Cells["Date5"].Value != string.Empty && dataGridCaseIncome.Rows[rowcalindex].Cells["Date5"].Value.ToString().Trim() != "/  /")
                    {
                        if (!CheckMstIntakeDate(Convert.ToDateTime(LookupDataAccess.GetFormatdate(dataGridCaseIncome.Rows[rowcalindex].Cells["Date5"].Value.ToString())).Date, strInterval))
                        {
                            decAmount5 = 0;
                            dataGridCaseIncome.Rows[rowcalindex].Cells["Date5"].Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            dataGridCaseIncome.Rows[rowcalindex].Cells["Date5"].Style.ForeColor = Color.Black;
                            decAmount5 = Convert.ToDecimal(dataGridCaseIncome.Rows[rowcalindex].Cells["Amt5"].Value);
                            intTotalChecks = intTotalChecks + 1;
                            if (txtHr5.Text != string.Empty)
                                decTotalHourly = decTotalHourly + Convert.ToDecimal(txtHr5.Text);
                        }
                    }
                }

                if (intTotalChecks == 0)
                    intTotalChecks = 1;
                decimal dechourlyIncTotal = decAmount1 + decAmount2 + decAmount3 + decAmount4 + decAmount5;
                decimal decsubtotal = dechourlyIncTotal;
                if (decTotalHourly > 0)
                    decTotalHourly = decTotalHourly / intTotalChecks;

                switch (strInterval)
                {
                    case "H":
                        if (decTotalHourly == 0)
                            decTotalHourly = 1;
                        decAvgTotal = dechourlyIncTotal / decTotalHourly / intTotalChecks;
                        decAvgTotal = Math.Round(decAvgTotal, 2, MidpointRounding.AwayFromZero);
                        dechourlyIncTotal = (decAvgTotal * 52) * decTotalHourly;
                        break;
                    case "W":
                        decAvgTotal = dechourlyIncTotal / intTotalChecks;
                        decAvgTotal = Math.Round(decAvgTotal, 2,MidpointRounding.AwayFromZero) ;
                        dechourlyIncTotal = (decAvgTotal * 52);
                        break;
                    case "B":
                        decAvgTotal = dechourlyIncTotal / intTotalChecks;
                        decAvgTotal = Math.Round(decAvgTotal, 2, MidpointRounding.AwayFromZero);
                        dechourlyIncTotal = (decAvgTotal * 26);
                        break;
                    case "S":
                        decAvgTotal = dechourlyIncTotal / intTotalChecks;
                        decAvgTotal = Math.Round(decAvgTotal, 2, MidpointRounding.AwayFromZero);
                        dechourlyIncTotal = (decAvgTotal * 24);
                        break;
                    case "M":
                        decAvgTotal = dechourlyIncTotal / intTotalChecks;
                        decAvgTotal = Math.Round(decAvgTotal, 2, MidpointRounding.AwayFromZero);
                        dechourlyIncTotal = (dechourlyIncTotal * 12);
                        //dechourlyIncTotal = (decAvgTotal * 12);
                        break;
                    case "3":
                    case "6":
                    case "9":
                        decAvgTotal = dechourlyIncTotal / intTotalChecks;
                        //decAvgTotal = dechourlyIncTotal;
                        decAvgTotal = Math.Round(decAvgTotal, 2, MidpointRounding.AwayFromZero);
                        dechourlyIncTotal = (dechourlyIncTotal * Convert.ToDecimal(dataGridCaseIncome.Rows[rowcalindex].Cells["Factor"].Value));
                        break;
                    default:
                        decAvgTotal = dechourlyIncTotal / intTotalChecks;
                        decAvgTotal = Math.Round(decAvgTotal, 2, MidpointRounding.AwayFromZero);
                        break;
                }

                // decAvgTotal = Math.Round(decAvgTotal, 2);
                dataGridCaseIncome.Rows[rowcalindex].Cells["Sub"].Value = LookupDataAccess.decimal2value(decAvgTotal.ToString());
                dataGridCaseIncome.Rows[rowcalindex].Cells["gvtAvgSub"].Value = LookupDataAccess.decimal2value(decsubtotal.ToString());
                if (dataGridCaseIncome.Rows[rowcalindex].Cells["gvtDeducation"].Value.ToString() == "N")
                {
                    dataGridCaseIncome.Rows[rowcalindex].Cells["Total"].Value = "-" + LookupDataAccess.decimal2value(dechourlyIncTotal.ToString());//("0.00##");
                }
                else
                {
                    dataGridCaseIncome.Rows[rowcalindex].Cells["Total"].Value = LookupDataAccess.decimal2value(dechourlyIncTotal.ToString());//("0.00##");
                }

            }
            else
            {

                if (dataGridCaseIncome.Rows[rowcalindex].Cells["Factor"].Value != string.Empty)
                {
                    intamtcount = 0;
                    decAmount1 = 0;
                    decAmount2 = 0;
                    decAmount3 = 0;
                    decAmount4 = 0;
                    decAmount5 = 0;
                    if (strInterval == "3" || strInterval == "6" || strInterval == "9")
                    {
                        if (dataGridCaseIncome.Rows[rowcalindex].Cells["Amt1"].Value != string.Empty)
                        {
                            if (dataGridCaseIncome.Rows[rowcalindex].Cells["Date1"].Value != string.Empty && dataGridCaseIncome.Rows[rowcalindex].Cells["Date1"].Value.ToString().Trim() != "/  /")
                            {
                                if (!CheckMstIntakeDate(Convert.ToDateTime(dataGridCaseIncome.Rows[rowcalindex].Cells["Date1"].Value).Date, strInterval))
                                {
                                    decAmount1 = 0;
                                    dataGridCaseIncome.Rows[rowcalindex].Cells["Date1"].Style.ForeColor = Color.Red;
                                }
                                else
                                {
                                    dataGridCaseIncome.Rows[rowcalindex].Cells["Date1"].Style.ForeColor = Color.Black;
                                    decAmount1 = Convert.ToDecimal(dataGridCaseIncome.Rows[rowcalindex].Cells["Amt1"].Value);
                                }
                            }
                        }
                        if (dataGridCaseIncome.Rows[rowcalindex].Cells["Amt2"].Value != string.Empty)
                        {
                            if (dataGridCaseIncome.Rows[rowcalindex].Cells["Date2"].Value != string.Empty && dataGridCaseIncome.Rows[rowcalindex].Cells["Date2"].Value.ToString().Trim() != "/  /")
                            {
                                if (!CheckMstIntakeDate(Convert.ToDateTime(dataGridCaseIncome.Rows[rowcalindex].Cells["Date2"].Value).Date, strInterval))
                                {
                                    decAmount2 = 0;
                                    dataGridCaseIncome.Rows[rowcalindex].Cells["Date2"].Style.ForeColor = Color.Red;
                                }
                                else
                                {
                                    dataGridCaseIncome.Rows[rowcalindex].Cells["Date2"].Style.ForeColor = Color.Black;
                                    decAmount2 = Convert.ToDecimal(dataGridCaseIncome.Rows[rowcalindex].Cells["Amt2"].Value);
                                }
                            }
                        }
                        if (dataGridCaseIncome.Rows[rowcalindex].Cells["Amt3"].Value != string.Empty)
                        {
                            if (dataGridCaseIncome.Rows[rowcalindex].Cells["Date3"].Value != string.Empty && dataGridCaseIncome.Rows[rowcalindex].Cells["Date3"].Value.ToString().Trim() != "/  /")
                            {
                                if (!CheckMstIntakeDate(Convert.ToDateTime(dataGridCaseIncome.Rows[rowcalindex].Cells["Date3"].Value).Date, strInterval))
                                {
                                    decAmount3 = 0;
                                    dataGridCaseIncome.Rows[rowcalindex].Cells["Date3"].Style.ForeColor = Color.Red;
                                }
                                else
                                {
                                    dataGridCaseIncome.Rows[rowcalindex].Cells["Date3"].Style.ForeColor = Color.Black;
                                    decAmount3 = Convert.ToDecimal(dataGridCaseIncome.Rows[rowcalindex].Cells["Amt3"].Value);
                                }
                            }
                        }
                        if (dataGridCaseIncome.Rows[rowcalindex].Cells["Amt4"].Value != string.Empty)
                        {
                            if (dataGridCaseIncome.Rows[rowcalindex].Cells["Date4"].Value != string.Empty && dataGridCaseIncome.Rows[rowcalindex].Cells["Date4"].Value.ToString().Trim() != "/  /")
                            {
                                if (!CheckMstIntakeDate(Convert.ToDateTime(dataGridCaseIncome.Rows[rowcalindex].Cells["Date4"].Value).Date, strInterval))
                                {
                                    decAmount4 = 0;
                                    dataGridCaseIncome.Rows[rowcalindex].Cells["Date4"].Style.ForeColor = Color.Red;
                                }
                                else
                                {
                                    dataGridCaseIncome.Rows[rowcalindex].Cells["Date4"].Style.ForeColor = Color.Black;
                                    decAmount4 = Convert.ToDecimal(dataGridCaseIncome.Rows[rowcalindex].Cells["Amt4"].Value);
                                }
                            }
                        }
                        if (dataGridCaseIncome.Rows[rowcalindex].Cells["Amt5"].Value != string.Empty)
                        {
                            if (dataGridCaseIncome.Rows[rowcalindex].Cells["Date5"].Value != string.Empty && dataGridCaseIncome.Rows[rowcalindex].Cells["Date5"].Value.ToString().Trim() != "/  /")
                            {
                                if (!CheckMstIntakeDate(Convert.ToDateTime(dataGridCaseIncome.Rows[rowcalindex].Cells["Date5"].Value).Date, strInterval))
                                {
                                    decAmount5 = 0;
                                    dataGridCaseIncome.Rows[rowcalindex].Cells["Date5"].Style.ForeColor = Color.Red;
                                }
                                else
                                {
                                    dataGridCaseIncome.Rows[rowcalindex].Cells["Date5"].Style.ForeColor = Color.Black;
                                    decAmount5 = Convert.ToDecimal(dataGridCaseIncome.Rows[rowcalindex].Cells["Amt5"].Value);
                                }
                            }
                        }
                        // dataGridCaseIncome.Rows[rowcalindex].Cells[intcolumnindex].Style.ForeColor = System.Drawing.Color.Black;
                        // if (!CheckMstIntakeDate(Convert.ToDateTime(strCurrectValue).Date, strIntervalValue))
                        // dataGridCaseIncome.Rows[rowcalindex].Cells[intcolumnindex].Style.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        if (dataGridCaseIncome.Rows[rowcalindex].Cells["Amt1"].Value != string.Empty)
                            decAmount1 = Convert.ToDecimal(dataGridCaseIncome.Rows[rowcalindex].Cells["Amt1"].Value);
                        if (dataGridCaseIncome.Rows[rowcalindex].Cells["Amt2"].Value != string.Empty)
                            decAmount2 = Convert.ToDecimal(dataGridCaseIncome.Rows[rowcalindex].Cells["Amt2"].Value);
                        if (dataGridCaseIncome.Rows[rowcalindex].Cells["Amt3"].Value != string.Empty)
                            decAmount3 = Convert.ToDecimal(dataGridCaseIncome.Rows[rowcalindex].Cells["Amt3"].Value);
                        if (dataGridCaseIncome.Rows[rowcalindex].Cells["Amt4"].Value != string.Empty)
                            decAmount4 = Convert.ToDecimal(dataGridCaseIncome.Rows[rowcalindex].Cells["Amt4"].Value);
                        if (dataGridCaseIncome.Rows[rowcalindex].Cells["Amt5"].Value != string.Empty)
                            decAmount5 = Convert.ToDecimal(dataGridCaseIncome.Rows[rowcalindex].Cells["Amt5"].Value);
                    }
                    decimal decTotal = decAmount1 + decAmount2 + decAmount3 + decAmount4 + decAmount5;

                    if (programDefinitionList.IncomeWeek == "Y")
                    {
                        //if (strInterval == "W" || strInterval == "E")
                        if (strInterval == "W") // modifed this logic remove "E" tag 04/02/2019
                        {
                            if (decAmount1 > 0)
                                intamtcount = intamtcount + 1;
                            if (decAmount2 > 0)
                                intamtcount = intamtcount + 1;
                            if (decAmount3 > 0)
                                intamtcount = intamtcount + 1;
                            if (decAmount4 > 0)
                                intamtcount = intamtcount + 1;
                            if (decAmount5 > 0)
                                intamtcount = intamtcount + 1;
                            if (propAgencyControlDetails.IncMethods.ToUpper() == "3")
                            {
                                if (intamtcount > 0)
                                    decTotal = decTotal / intamtcount;
                            }
                            else
                            {
                                decTotal = decTotal / 4;
                            }
                            decTotal = decTotal * Convert.ToDecimal(4.33);
                        }
                        else if (strInterval == "B")
                        {
                            decTotal = decTotal / 2;
                            decTotal = decTotal * Convert.ToDecimal(2.165);
                        }
                    }
                    // decTotal = Math.Round(decTotal, 2);
                    dataGridCaseIncome.Rows[rowcalindex].Cells["Sub"].Value = LookupDataAccess.decimal2value(decTotal.ToString());
                    if (dataGridCaseIncome.Rows[rowcalindex].Cells["gvtDeducation"].Value.ToString() == "N")
                    {
                        dataGridCaseIncome.Rows[rowcalindex].Cells["Total"].Value = "-" + LookupDataAccess.decimal2value((decTotal * Convert.ToDecimal(dataGridCaseIncome.Rows[rowcalindex].Cells["Factor"].Value)).ToString());
                    }
                    else
                    {
                        dataGridCaseIncome.Rows[rowcalindex].Cells["Total"].Value = LookupDataAccess.decimal2value((decTotal * Convert.ToDecimal(dataGridCaseIncome.Rows[rowcalindex].Cells["Factor"].Value)).ToString());
                    }

                }
            }

        }
        string strMode = string.Empty;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            bool boolFedMsg = true;

            try
            {

                if (propAgencyControlDetails.IncVerfication.ToUpper() == "Y")
                {
                    chkCheckStubs.Enabled = true;
                    chkLetter.Enabled = true;
                    chkOther.Enabled = true;
                    chk_self_Decl.Enabled = true;
                    chkTaxReturn.Enabled = true;
                    chkw2.Enabled = true;
                }
                else
                {
                    chkCheckStubs.Enabled = false;
                    chkLetter.Enabled = false;
                    chkOther.Enabled = false;
                    chk_self_Decl.Enabled = false;
                    chkTaxReturn.Enabled = false;
                    chkw2.Enabled = false;
                }


                if (propAgencyControlDetails.VerSwitch.ToUpper() != "Y")
                {
                    boolFedMsg = ShowFedDetailsMessages();
                }

                if (boolFedMsg)
                {
                    if (dataGridCaseIncome.Rows.Count > 0)
                    {
                        strMode = "ADD";
                        _errorProvider.SetError(dataGridCaseIncome, null);
                        dataGridCaseIncome.ReadOnly = false;
                        dataGridCaseSnp_SelectionChanged(sender, e);
                        dataGridCaseIncome.Columns["Delete"].Visible = false;
                        dataGridCaseIncome.ReadOnly = false;
                        MenuPropertie = "Menu";
                        EnableOnlyMaincontrol();
                        EnableDisableGridcontrol();
                        btnAdd.Enabled = false;
                        if (dataGridCaseIncome.Rows.Count > 0)
                        {
                            List<CommonEntity> agyIncomeTypes = lookUpIncomeTypes.FindAll(u => u.Active == "Y");
                            foreach (DataGridViewRow item in dataGridCaseIncome.Rows)
                            {
                                agyIncomeTypes = agyIncomeTypes.FindAll(u => u.Code.ToUpper().Trim() != item.Cells["IncomeTypeCode"].Value.ToString().ToUpper().Trim());
                            }
                            string[] strIncomeTypes = programDefinitionList.DepIncomeTypes.Split(' ');

                            foreach (CommonEntity agyEntity in agyIncomeTypes)
                            {
                                //bool boolIncomeType = false;
                                bool boolAddExclude = false;
                                foreach (string incomeType in strIncomeTypes)
                                {
                                    // agyEntity.agyRowtype = "N";
                                    if (agyEntity.Code == incomeType)
                                    {
                                        boolAddExclude = true;
                                        // agyEntity.agyRowtype = "Y";
                                    }

                                }
                                int index = dataGridCaseIncome.Rows.Add(false, strFamilySeq, string.Empty, agyEntity.Code, agyEntity.Desc, string.Empty, false, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, strCaseWorkerDefaultCode, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                dataGridCaseIncome.Rows[index].Tag = agyEntity;
                                dataGridCaseIncome.Rows[index].Cells["Exclude"].Value = boolAddExclude;
                                dataGridCaseIncome.Rows[index].Cells["Status"].Value = string.Empty;
                                dataGridCaseIncome.Rows[index].Cells["gvtDeducation"].Value = agyEntity.Extension;

                                dataGridCaseIncome.Rows[index].Cells["Column1"].Value = string.Empty;

                                if (agyEntity.Active != "Y")
                                    dataGridCaseIncome.Rows[index].DefaultCellStyle.ForeColor = Color.Red;



                                //if (agyEntity.agydefault.Equals("Y"))
                                //    strIncomeDefaultCode = agyEntity.agycode == null ? string.Empty : agyEntity.agycode;

                            }
                            btnEdit.Enabled = false;
                            btnSave.Visible = true;
                            btnCancel.Visible = true;
                        }
                        else
                        {
                            CommonFunctions.SetComboBoxValue(cmbVerifier, strCaseWorkerDefaultStartCode);
                        }
                        RequireAllGridColors();


                    }
                    else
                    {

                        _errorProvider.SetError(dataGridCaseIncome, null);
                        MenuPropertie = "Menu";
                        btnSave.Visible = true;
                        btnCancel.Visible = true;
                        btnAdd.Enabled = false;
                        btnEdit.Enabled = false;

                        CommonFunctions.SetComboBoxValue(cmbVerifier, strCaseWorkerDefaultStartCode);

                        dataGridCaseIncome.ReadOnly = false;
                        EnableOnlyMaincontrol();
                        EnableDisableGridcontrol();
                        dataGridCaseIncome.Columns["Delete"].Visible = false;
                        DataGridViewComboBoxColumn cb = (DataGridViewComboBoxColumn)this.dataGridCaseIncome.Columns["Interval"];
                        cb.DataSource = commonIntervalProp;
                        cb.DisplayMember = "DESC";
                        cb.ValueMember = "CODE";
                        cb = (DataGridViewComboBoxColumn)this.dataGridCaseIncome.Columns["Interval"];

                        List<CommonEntity> agyIncomeTypes = lookUpIncomeTypes.FindAll(u => u.Active == "Y");
                        //foreach (DataGridViewRow item in dataGridCaseIncome.Rows)
                        //{
                        //    agyIncomeTypes = agyIncomeTypes.FindAll(u => u.Code.ToUpper().Trim() != item.Cells["IncomeTypeCode"].Value.ToString().ToUpper().Trim());
                        //}
                        dataGridCaseIncome.SelectionChanged -= new EventHandler(dataGridCaseIncome_SelectionChanged);
                        dataGridCaseIncome.Rows.Clear();
                        string[] strIncomeTypes = programDefinitionList.DepIncomeTypes.Split(' ');

                        foreach (CommonEntity agyEntity in agyIncomeTypes)
                        {
                            //bool boolIncomeType = false;
                            bool boolAddExclude = false;
                            foreach (string incomeType in strIncomeTypes)
                            {
                                // agyEntity.agyRowtype = "N";
                                if (agyEntity.Code == incomeType)
                                {
                                    boolAddExclude = true;
                                    // agyEntity.agyRowtype = "Y";
                                }

                            }
                            int index = dataGridCaseIncome.Rows.Add(false, strFamilySeq, string.Empty, agyEntity.Code, agyEntity.Desc, string.Empty, false, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, strCaseWorkerDefaultCode, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

                            dataGridCaseIncome.Rows[index].Tag = agyEntity;
                            dataGridCaseIncome.Rows[index].Cells["Exclude"].Value = boolAddExclude;
                            dataGridCaseIncome.Rows[index].Cells["Status"].Value = string.Empty;
                            dataGridCaseIncome.Rows[index].Cells["gvtDeducation"].Value = agyEntity.Extension;
                            dataGridCaseIncome.Rows[index].Cells["Column1"].Value = string.Empty;
                            //if (agyEntity.agydefault.Equals("Y"))
                            //    strIncomeDefaultCode = agyEntity.agycode == null ? string.Empty : agyEntity.agycode;

                            if (agyEntity.Active != "Y")
                                dataGridCaseIncome.Rows[index].DefaultCellStyle.ForeColor = Color.Red;

                        }
                        dataGridCaseIncome.SelectionChanged += new EventHandler(dataGridCaseIncome_SelectionChanged);

                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void cmbVerifier_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (((ListItem)cmbVerifier.SelectedItem).Value.ToString() != "0")
                {
                    if (dataGridCaseIncome.Rows.Count > 0)
                    {
                        if (dataGridCaseIncome.SelectedCells.Count > 0)
                        {
                            dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["gvtVerifier"].Value = ((ListItem)cmbVerifier.SelectedItem).Value.ToString();
                        }
                    }
                    if (((ListItem)cmbVerifier.SelectedItem).ID.ToString() != "N")
                        MessageBox.Show("Inactive CaseWorker", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (propAgencyControlDetails.IncVerfication.ToUpper() == "Y")
                {
                    chkCheckStubs.Enabled = true;
                    chkLetter.Enabled = true;
                    chkOther.Enabled = true;
                    chk_self_Decl.Enabled = true;
                    chkTaxReturn.Enabled = true;
                    chkw2.Enabled = true;
                }
                else
                {
                    chkCheckStubs.Enabled = false;
                    chkLetter.Enabled = false;
                    chkOther.Enabled = false;
                    chk_self_Decl.Enabled = false;
                    chkTaxReturn.Enabled = false;
                    chkw2.Enabled = false;
                }

                bool boolFedMsg = true;

                if (propAgencyControlDetails.VerSwitch.ToUpper() != "Y")
                {
                    boolFedMsg = ShowFedDetailsMessages();
                }

                if (boolFedMsg)
                {
                    _errorProvider.SetError(dataGridCaseIncome, null);
                    dataGridCaseIncome.ReadOnly = false;
                    dataGridCaseSnp_SelectionChanged(sender, e);

                    if (dataGridCaseIncome.Rows.Count > 0)
                    {
                        dataGridCaseIncome.Focus();
                        dataGridCaseIncome.Rows[0].Cells[0].Selected = true;
                    }
                    dataGridCaseIncome.Columns["Delete"].Visible = false;
                    dataGridCaseIncome.ReadOnly = false;
                    MenuPropertie = "Menu";
                    EnableOnlyMaincontrol();
                    EnableDisableGridcontrol();
                    btnAdd.Enabled = false;
                    if (dataGridCaseIncome.Rows.Count > 0)
                    {
                        strMode = "EDIT";
                        //List<AgyTabEntity> agyIncomeTypes = lookUpIncomeTypes;
                        //foreach (DataGridViewRow item in dataGridCaseIncome.Rows)
                        //{
                        //    agyIncomeTypes = agyIncomeTypes.FindAll(u => u.agycode.ToUpper().Trim() != item.Cells["IncomeTypeCode"].Value.ToString().ToUpper().Trim());
                        //}
                        //string[] strIncomeTypes = programDefinitionList.DepIncomeTypes.Split(' ');

                        //foreach (AgyTabEntity agyEntity in agyIncomeTypes)
                        //{
                        //    //bool boolIncomeType = false;
                        //    bool boolAddExclude = false;
                        //    foreach (string incomeType in strIncomeTypes)
                        //    {
                        //        agyEntity.agyRowtype = "N";
                        //        if (agyEntity.agycode == incomeType)
                        //        {
                        //            boolAddExclude = true;
                        //            agyEntity.agyRowtype = "Y";
                        //        }

                        //    }
                        //    int index = dataGridCaseIncome.Rows.Add(false, strFamilySeq, string.Empty, agyEntity.agycode, agyEntity.agydesc, string.Empty, false, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, strCaseWorkerDefaultCode, string.Empty, string.Empty);
                        //    dataGridCaseIncome.Rows[index].Tag = agyEntity;
                        //    dataGridCaseIncome.Rows[index].Cells["Exclude"].Value = boolAddExclude;
                        //    dataGridCaseIncome.Rows[index].Cells["Status"].Value = string.Empty;
                        //    //if (agyEntity.agydefault.Equals("Y"))
                        //    //    strIncomeDefaultCode = agyEntity.agycode == null ? string.Empty : agyEntity.agycode;

                        //}
                        btnEdit.Enabled = false;
                        btnSave.Visible = true;
                        if (propAgencyControlDetails.IncMethods == "2")
                        {
                            string strIntervalValue = Convert.ToString(dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["Interval"].Value);
                            HourlyEnableControl(strIntervalValue);
                        }
                        btnCancel.Visible = true;
                    }
                    RequireAllGridColors();
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {

                string strInterval = string.Empty;
                // RequiredControlsColorChange();
                //  SelectIntervalcolorchange();
                _errorProvider.SetError(dataGridCaseIncome, null);
                if (ValidationGrid())
                {
                    if (ValidationIntervalGrid(out strInterval))
                    {
                        if (ShowFedDetailsMessages())
                        {
                            CaseIncomeEntity caseIncomeEntity = new CaseIncomeEntity();
                            caseIncomeEntity.Agency = BaseForm.BaseAgency;
                            caseIncomeEntity.Dept = BaseForm.BaseDept;
                            caseIncomeEntity.Program = BaseForm.BaseProg;
                            caseIncomeEntity.Year = BaseForm.BaseYear;
                            caseIncomeEntity.App = BaseForm.BaseApplicationNo;
                            caseIncomeEntity.FamilySeq = Convert.ToString(dataGridCaseSnp.SelectedRows[0].Cells["SNP_FAMILY_SEQ"].Value); ;
                            caseIncomeEntity.Seq = string.Empty;
                            bool booldeletestatus = true;
                            if (Privileges.DelPriv.Equals("false"))
                            {
                                if (propIncomeTypeSwitch != "Y")
                                {
                                    foreach (DataGridViewRow item in dataGridCaseIncome.Rows)
                                    {
                                        if (item.Cells["Status"].Value.ToString() == "U")
                                        {
                                            if ((string.IsNullOrEmpty(item.Cells["Factor"].Value.ToString())))
                                            {
                                                booldeletestatus = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if (booldeletestatus)
                            {

                                strMode = string.Empty;
                                MenuPropertie = string.Empty;
                                btnSave.Visible = false;
                                btnCancel.Visible = false;
                                dataGridCaseIncome.ReadOnly = true;
                                if (Privileges.DelPriv.Equals("false"))
                                {
                                    dataGridCaseIncome.Columns["Delete"].Visible = false;
                                }
                                else
                                    dataGridCaseIncome.Columns["Delete"].Visible = true;
                                if (_model.CaseMstData.DeleteCaseIncome(caseIncomeEntity))
                                {
                                    bool boolSucess = false;
                                    int intgridloop = 0;
                                    string strSqlMsg;
                                    foreach (DataGridViewRow item in dataGridCaseIncome.Rows)
                                    {
                                        if (!(string.IsNullOrEmpty(item.Cells["Factor"].Value.ToString())))
                                        {
                                            intgridloop++;
                                            caseIncomeEntity.Agency = BaseForm.BaseAgency;
                                            caseIncomeEntity.Dept = BaseForm.BaseDept;
                                            caseIncomeEntity.Program = BaseForm.BaseProg;
                                            caseIncomeEntity.Year = BaseForm.BaseYear;
                                            caseIncomeEntity.App = BaseForm.BaseApplicationNo;
                                            caseIncomeEntity.FamilySeq = Convert.ToString(dataGridCaseSnp.SelectedRows[0].Cells["SNP_FAMILY_SEQ"].Value);
                                            caseIncomeEntity.Seq = Convert.ToString(item.Cells["IncomeSeq"].Value); ;
                                            caseIncomeEntity.Type = Convert.ToString(item.Cells["IncomeTypeCode"].Value);



                                            //if (!((ListItem)cmbInterval.SelectedItem).Value.ToString().Equals("0"))
                                            //{
                                            caseIncomeEntity.Interval = Convert.ToString(item.Cells["Interval"].Value);

                                            // }
                                            if (item.Cells["gvtVerifier"].Value.ToString() != "0")
                                            {
                                                caseIncomeEntity.Verifier = item.Cells["gvtVerifier"].Value.ToString();
                                            }

                                            if (StrFormType == "TMS00081")
                                            {
                                                if (item.Cells["Column1"].Value.ToString() != "0")
                                                {
                                                    caseIncomeEntity.VerMethod = item.Cells["Column1"].Value.ToString();
                                                }
                                            }

                                            caseIncomeEntity.Val1 = string.Empty;
                                            caseIncomeEntity.Val2 = string.Empty;
                                            caseIncomeEntity.Val3 = string.Empty;
                                            caseIncomeEntity.Val4 = string.Empty;
                                            caseIncomeEntity.Val5 = string.Empty;
                                            if (item.Cells["Amt1"].Value != null)
                                                caseIncomeEntity.Val1 = item.Cells["Amt1"].Value.ToString();
                                            if (item.Cells["Amt2"].Value != null)
                                                caseIncomeEntity.Val2 = item.Cells["Amt2"].Value.ToString();
                                            if (item.Cells["Amt3"].Value != null)
                                                caseIncomeEntity.Val3 = item.Cells["Amt3"].Value.ToString();
                                            if (item.Cells["Amt4"].Value != null)
                                                caseIncomeEntity.Val4 = item.Cells["Amt4"].Value.ToString();
                                            if (item.Cells["Amt5"].Value != null)
                                                caseIncomeEntity.Val5 = item.Cells["Amt5"].Value.ToString();
                                            caseIncomeEntity.Date1 = string.Empty;
                                            caseIncomeEntity.Date2 = string.Empty;
                                            caseIncomeEntity.Date3 = string.Empty;
                                            caseIncomeEntity.Date4 = string.Empty;
                                            caseIncomeEntity.Date5 = string.Empty;
                                            //if (caldate1.Checked)
                                            if (item.Cells["Date1"].Value!=null)
                                            {
                                                if (item.Cells["Date1"].Value.ToString() != string.Empty && item.Cells["Date1"].Value.ToString().Trim() != "/  /")
                                                    caseIncomeEntity.Date1 = LookupDataAccess.GetFormatdate(item.Cells["Date1"].Value.ToString());
                                            }
                                            // if (caldate2.Checked)
                                            if (item.Cells["Date2"].Value != null)
                                            {
                                                if (item.Cells["Date2"].Value.ToString() != string.Empty && item.Cells["Date2"].Value.ToString().Trim() != "/  /")
                                                    caseIncomeEntity.Date2 = LookupDataAccess.GetFormatdate(item.Cells["Date2"].Value.ToString());
                                            }
                                            //  if (caldate3.Checked)
                                            if (item.Cells["Date3"].Value != null)
                                            {
                                                if (item.Cells["Date3"].Value.ToString() != string.Empty && item.Cells["Date3"].Value.ToString().Trim() != "/  /")
                                                    caseIncomeEntity.Date3 = LookupDataAccess.GetFormatdate(item.Cells["Date3"].Value.ToString());
                                            }
                                            //  if (caldate4.Checked)
                                            if (item.Cells["Date4"].Value != null)
                                            {
                                                if (item.Cells["Date4"].Value.ToString() != string.Empty && item.Cells["Date4"].Value.ToString().Trim() != "/  /")
                                                    caseIncomeEntity.Date4 = LookupDataAccess.GetFormatdate(item.Cells["Date4"].Value.ToString());
                                            }
                                            //  if (caldate5.Checked)
                                            if (item.Cells["Date5"].Value != null)
                                            {
                                                if (item.Cells["Date5"].Value.ToString() != string.Empty && item.Cells["Date5"].Value.ToString().Trim() != "/  /")
                                                    caseIncomeEntity.Date5 = LookupDataAccess.GetFormatdate(item.Cells["Date5"].Value.ToString());
                                            }
                                            if (Convert.ToBoolean(item.Cells["Exclude"].Value) == true)
                                            {
                                                caseIncomeEntity.Exclude = "Y";
                                            }
                                            else
                                            {
                                                caseIncomeEntity.Exclude = "N";
                                            }
                                            caseIncomeEntity.HowVerified = item.Cells["gvtHowverfier"].Value.ToString();
                                            caseIncomeEntity.Factor = item.Cells["Factor"].Value.ToString();
                                            caseIncomeEntity.Source = item.Cells["gvtIncomesource"].Value.ToString();
                                            caseIncomeEntity.TotIncome = item.Cells["Total"].Value.ToString();
                                            caseIncomeEntity.ProgIncome = item.Cells["Total"].Value.ToString();

                                            caseIncomeEntity.HrRate1 = string.Empty;
                                            caseIncomeEntity.HrRate2 = string.Empty;
                                            caseIncomeEntity.HrRate3 = string.Empty;
                                            caseIncomeEntity.HrRate4 = string.Empty;
                                            caseIncomeEntity.HrRate5 = string.Empty;
                                            caseIncomeEntity.Average = string.Empty;
                                            if (propHourlyMode.ToUpper() == "Y")
                                            {
                                                caseIncomeEntity.Average = item.Cells["Sub"].Value.ToString();
                                            }
                                            if (caseIncomeEntity.Interval == "H")
                                            {
                                                caseIncomeEntity.HrRate1 = item.Cells["gvtHrRate1"].Value.ToString();
                                                caseIncomeEntity.HrRate2 = item.Cells["gvtHrRate2"].Value.ToString();
                                                caseIncomeEntity.HrRate3 = item.Cells["gvtHrRate3"].Value.ToString();
                                                caseIncomeEntity.HrRate4 = item.Cells["gvtHrRate4"].Value.ToString();
                                                caseIncomeEntity.HrRate5 = item.Cells["gvtHrRate5"].Value.ToString();

                                            }
                                            caseIncomeEntity.LstcOperator = BaseForm.UserID;
                                            caseIncomeEntity.AddOperator = BaseForm.UserID;
                                            caseIncomeEntity.Mode = string.Empty; //item.Cells["Status"].Value.ToString();
                                            //txtSub.Text;                
                                            if (_model.CaseMstData.InsertCaseIncome(caseIncomeEntity))
                                            {
                                                boolSucess = true;
                                            }
                                        }
                                    }
                                    //  if (boolSucess)
                                    // {

                                    /// New Logic MSTSNP Incomes data update  06/24/2015
                                    //if (dataGridCaseSnp.SelectedRows.Count > 0)
                                    //{
                                    //    if (dataGridCaseIncome.Rows.Count > 0)
                                    //    {
                                    //        CaseSnpEntity casesnprow = dataGridCaseSnp.SelectedRows[0].Tag as CaseSnpEntity;
                                    //        if (casesnprow != null)
                                    //            _model.CaseMstData.UpdateCaseMstSnpIncome(casesnprow);
                                    //    }
                                    //}
                                    /// 

                                    int _strSNPindex = strSnpIndex;


                                    if (propAgencyControlDetails.IncVerfication.ToUpper() == "Y")
                                    {
                                        //if (propAgencyControlDetails.VerSwitch.ToUpper() == "Y")
                                        //{
                                        BaseForm.BaseCaseMstListEntity = _model.CaseMstData.GetCaseMstadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
                                        CaseMSTEntity = BaseForm.BaseCaseMstListEntity[0];
                                        //}
                                        //calVerificationDate.Value = DateTime.Now.Date;
                                        txtFedOmb.Text = "";
                                        txtHud.Text = "";
                                        txtCmi.Text = "";
                                        txtSmi.Text = "";
                                        // calVerificationDate.ValueChanged -= new EventHandler(calVerificationDate_ValueChanged);
                                        if (programDefinitionList != null)
                                        {
                                            if (programDefinitionList.DepFEDUsed == "Y")
                                            {
                                                // txtFedOmb.Text = "0";
                                                calc_FEDOMB();
                                            }
                                            if (programDefinitionList.DepCMIUsed == "Y")
                                            {
                                                // txtCmi.Text = "0";
                                                calc_Poverty("CMI");
                                            }
                                            if (programDefinitionList.DepSMIUsed == "Y")
                                            {
                                                // txtSmi.Text = "0";
                                                calc_Poverty("SMI");
                                            }
                                            if (programDefinitionList.DepHUDUsed == "Y")
                                            {
                                                // txtHud.Text = "0";
                                                calc_Poverty("HUD");
                                            }
                                        }


                                        CaseVerEntity caseVerEntity = new CaseVerEntity();
                                        caseVerEntity.Agency = BaseForm.BaseAgency;
                                        caseVerEntity.Dept = BaseForm.BaseDept;
                                        caseVerEntity.Program = BaseForm.BaseProg;
                                        caseVerEntity.Year = BaseForm.BaseYear;
                                        caseVerEntity.AppNo = BaseForm.BaseApplicationNo;
                                        if (propAgencyControlDetails.VerSwitch.ToUpper() == "Y")
                                        {
                                            caseVerEntity.VerifyDate = LookupDataAccess.Getdate(calVerificationDate.Value.ToString()); //DateTime.Now.ToShortDateString();
                                        }
                                        else
                                        {
                                            caseVerEntity.VerifyDate = DateTime.Now.ToShortDateString();//
                                            calVerificationDate.Value = DateTime.Now.Date;
                                        }
                                        caseVerEntity.Classification = "00";

                                        if (cmbVerifier.Items.Count > 0)
                                            caseVerEntity.Verifier = ((Captain.Common.Utilities.ListItem)cmbVerifier.SelectedItem).Value.ToString();
                                        caseVerEntity.VerOmb = txtFedOmb.Text == string.Empty ? "0" : txtFedOmb.Text;
                                        caseVerEntity.VerHud = txtHud.Text == string.Empty ? "0" : txtHud.Text;
                                        caseVerEntity.VerSmi = txtSmi.Text == string.Empty ? "0" : txtSmi.Text;
                                        caseVerEntity.VerCmi = txtCmi.Text == string.Empty ? "0" : txtCmi.Text;
                                        caseVerEntity.IncomeAmount = CaseMSTEntity.ProgIncome; ;
                                        caseVerEntity.NoInhh = CaseMSTEntity.NoInProg;

                                        //cOMMENTED BY SUDHEER ON 02/25/25 BASED ON THE CACOST.docx
                                        //caseVerEntity.VerifyOther = "Y";

                                        //Kranthi 07/24/2023:: Added to save the Income Verified details
                                        /*********************************************************/
                                        /*********************************************************/
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

                                        //ADDED BY SUDHEER ON 02/25/25  BASED ON THE CACOST.docx
                                        if (caseVerEntity.VerifyW2!="Y" && caseVerEntity.VerifyLetter!="Y" && caseVerEntity.VerifyOther!="Y" && caseVerEntity.VerifyTaxReturn != "Y" && caseVerEntity.VerifySelfDecl != "Y")
                                            caseVerEntity.VerifyOther = "Y";


                                        /*********************************************************/
                                        /*********************************************************/
                                        /*********************************************************/

                                        caseVerEntity.ReverifyDate = string.Empty;
                                        caseVerEntity.AddOperator = BaseForm.UserID;
                                        caseVerEntity.LstcOperator = BaseForm.UserID;
                                        //caseVerEntity.Mode = "EMSVER";//Consts.Common.New;
                                        if (propAgencyControlDetails.VerSwitch.ToUpper() == "Y")
                                        {
                                            caseVerEntity.Mode = "EMSVER";
                                        }
                                        else
                                        {
                                            caseVerEntity.Mode = "DeleteIncome";
                                            if (_model.CaseMstData.InsertUpdateDelCaseVer(caseVerEntity, out strSqlMsg))
                                            {
                                            }
                                            caseVerEntity.Mode = Consts.Common.New;

                                        }

                                        if (_model.CaseMstData.InsertUpdateDelCaseVer(caseVerEntity, out strSqlMsg))
                                        {
                                            //if (propAgencyControlDetails.VerSwitch.ToUpper() != "Y")
                                            //{
                                            //    
                                            //}
                                            BaseForm.BaseCaseMstListEntity = _model.CaseMstData.GetCaseMstadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
                                            CaseMSTEntity = BaseForm.BaseCaseMstListEntity[0];
                                            BaseForm.BaseCaseSnpEntity = _model.CaseMstData.GetCaseSnpadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
                                            BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq)).M_Code = "A";

                                            fillSnpDetails(false);
                                            if (dataGridCaseSnp.Rows.Count != 0)
                                            {
                                                dataGridCaseSnp.Rows[strSnpIndex].Selected = true;
                                            }
                                            dataGridCaseIncome.ReadOnly = true;
                                        }

                                        calVerificationDate.Enabled = false;
                                        
                                    }
                                    else
                                    {
                                        BaseForm.BaseCaseMstListEntity = _model.CaseMstData.GetCaseMstadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);

                                        BaseForm.BaseCaseSnpEntity = _model.CaseMstData.GetCaseSnpadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
                                        BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq)).M_Code = "A";
                                        CaseMSTEntity = BaseForm.BaseCaseMstListEntity[0];
                                        fillSnpDetails(false);
                                        if (dataGridCaseSnp.Rows.Count != 0)
                                        {
                                            dataGridCaseSnp.Rows[strSnpIndex].Selected = true;
                                        }
                                        dataGridCaseIncome.ReadOnly = true;
                                    }



                                    cmbVerfMethod.Enabled = false;
                                    dataGridCaseSnp_SelectionChanged(sender, e);
                                    AlertBox.Show("Income Details Saved Successfully");

                                    if (_strSNPindex != strSnpIndex)
                                    {

                                        dataGridCaseSnp.ClearSelection();
                                        dataGridCaseSnp.Rows[_strSNPindex].Selected = true;
                                    }


                                    //}
                                    //if (intgridloop == 0)
                                    //{
                                    //    CommonFunctions.MessageBoxDisplay("Atleast one Interval selected");
                                    //    btnSave.Enabled = true;
                                    //    btnAdd_Click(sender, e);
                                    //}

                                }
                            }
                            else
                            {
                                MessageBox.Show("We can't delete income records.");
                            }
                        }
                    }
                    else
                    {
                        // CommonFunctions.MessageBoxDisplay(strInterval);
                        MessageBox.Show("Are you sure save date range records", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
                    }
                }
                else
                {
                    //   _errorProvider.SetError(dataGridCaseIncome, "Fill Required cells data");
                }
            }
        }

        private void MessageBoxHandler(DialogResult dialogResult)
        {

            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                MenuPropertie = string.Empty;
                btnSave.Visible = false;
                btnCancel.Visible = false;
                dataGridCaseIncome.ReadOnly = true;
                if (Privileges.DelPriv.Equals("false"))
                {
                    dataGridCaseIncome.Columns["Delete"].Visible = false;
                }
                else
                    dataGridCaseIncome.Columns["Delete"].Visible = true;
                CaseIncomeEntity caseIncomeEntity = new CaseIncomeEntity();
                caseIncomeEntity.Agency = BaseForm.BaseAgency;
                caseIncomeEntity.Dept = BaseForm.BaseDept;
                caseIncomeEntity.Program = BaseForm.BaseProg;
                caseIncomeEntity.Year = BaseForm.BaseYear;
                caseIncomeEntity.App = BaseForm.BaseApplicationNo;
                caseIncomeEntity.FamilySeq = Convert.ToString(dataGridCaseSnp.SelectedRows[0].Cells["SNP_FAMILY_SEQ"].Value); ;
                caseIncomeEntity.Seq = string.Empty;
                if (_model.CaseMstData.DeleteCaseIncome(caseIncomeEntity))
                {
                    bool boolSucess = false;
                    int intgridloop = 0;
                    foreach (DataGridViewRow item in dataGridCaseIncome.Rows)
                    {
                        if (!(string.IsNullOrEmpty(item.Cells["Factor"].Value.ToString())))
                        {
                            intgridloop++;
                            caseIncomeEntity.Agency = BaseForm.BaseAgency;
                            caseIncomeEntity.Dept = BaseForm.BaseDept;
                            caseIncomeEntity.Program = BaseForm.BaseProg;
                            caseIncomeEntity.Year = BaseForm.BaseYear;
                            caseIncomeEntity.App = BaseForm.BaseApplicationNo;
                            caseIncomeEntity.FamilySeq = Convert.ToString(dataGridCaseSnp.SelectedRows[0].Cells["SNP_FAMILY_SEQ"].Value);
                            caseIncomeEntity.Seq = Convert.ToString(item.Cells["IncomeSeq"].Value); ;
                            caseIncomeEntity.Type = Convert.ToString(item.Cells["IncomeTypeCode"].Value);



                            //if (!((ListItem)cmbInterval.SelectedItem).Value.ToString().Equals("0"))
                            //{
                            caseIncomeEntity.Interval = Convert.ToString(item.Cells["Interval"].Value);

                            // }
                            if (!((ListItem)cmbVerifier.SelectedItem).Value.ToString().Equals("0"))
                            {
                                caseIncomeEntity.Verifier = ((ListItem)cmbVerifier.SelectedItem).Value.ToString();
                            }
                            caseIncomeEntity.Val1 = item.Cells["Amt1"].Value.ToString();
                            caseIncomeEntity.Val2 = item.Cells["Amt2"].Value.ToString();
                            caseIncomeEntity.Val3 = item.Cells["Amt3"].Value.ToString();
                            caseIncomeEntity.Val4 = item.Cells["Amt4"].Value.ToString();
                            caseIncomeEntity.Val5 = item.Cells["Amt5"].Value.ToString();

                            caseIncomeEntity.Date1 = string.Empty;
                            caseIncomeEntity.Date2 = string.Empty;
                            caseIncomeEntity.Date3 = string.Empty;
                            caseIncomeEntity.Date4 = string.Empty;
                            caseIncomeEntity.Date5 = string.Empty;
                            //if (caldate1.Checked)
                            if (item.Cells["Date1"].Value.ToString() != string.Empty && item.Cells["Date1"].Value.ToString().Trim() != "/  /")
                                caseIncomeEntity.Date1 = item.Cells["Date1"].Value.ToString();
                            // if (caldate2.Checked)
                            if (item.Cells["Date2"].Value.ToString() != string.Empty && item.Cells["Date2"].Value.ToString().Trim() != "/  /")
                                caseIncomeEntity.Date2 = item.Cells["Date2"].Value.ToString();
                            //  if (caldate3.Checked)
                            if (item.Cells["Date3"].Value.ToString() != string.Empty && item.Cells["Date3"].Value.ToString().Trim() != "/  /")
                                caseIncomeEntity.Date3 = item.Cells["Date3"].Value.ToString();
                            //  if (caldate4.Checked)
                            if (item.Cells["Date4"].Value.ToString() != string.Empty && item.Cells["Date4"].Value.ToString().Trim() != "/  /")
                                caseIncomeEntity.Date4 = item.Cells["Date4"].Value.ToString();
                            //  if (caldate5.Checked)
                            if (item.Cells["Date5"].Value.ToString() != string.Empty && item.Cells["Date5"].Value.ToString().Trim() != "/  /")
                                caseIncomeEntity.Date5 = item.Cells["Date5"].Value.ToString();
                            if (Convert.ToBoolean(item.Cells["Exclude"].Value) == true)
                            {
                                caseIncomeEntity.Exclude = "Y";
                            }
                            else
                            {
                                caseIncomeEntity.Exclude = "N";
                            }
                            caseIncomeEntity.HowVerified = txtHowVerified.Text;
                            caseIncomeEntity.Factor = item.Cells["Factor"].Value.ToString();
                            caseIncomeEntity.Source = txtIncSource.Text;
                            caseIncomeEntity.TotIncome = item.Cells["Total"].Value.ToString();
                            caseIncomeEntity.ProgIncome = item.Cells["Total"].Value.ToString();
                            caseIncomeEntity.LstcOperator = BaseForm.UserID;
                            caseIncomeEntity.AddOperator = BaseForm.UserID;
                            caseIncomeEntity.Mode = string.Empty; //item.Cells["Status"].Value.ToString();
                                                                  //txtSub.Text;                
                            if (_model.CaseMstData.InsertCaseIncome(caseIncomeEntity))
                            {
                                boolSucess = true;
                            }
                        }
                    }
                    //  if (boolSucess)
                    // {
                    BaseForm.BaseCaseMstListEntity = _model.CaseMstData.GetCaseMstadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
                    BaseForm.BaseCaseSnpEntity = _model.CaseMstData.GetCaseSnpadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
                    BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq)).M_Code = "A";
                    CaseMSTEntity = BaseForm.BaseCaseMstListEntity[0];
                    fillSnpDetails(false);
                    if (dataGridCaseSnp.Rows.Count != 0)
                    {
                        dataGridCaseSnp.Rows[strSnpIndex].Selected = true;
                    }
                    dataGridCaseIncome.ReadOnly = true;

                    if (propAgencyControlDetails.IncVerfication.ToUpper() == "Y")
                    {
                        // calVerificationDate.Value = DateTime.Now.Date;
                        txtFedOmb.Text = "";
                        txtHud.Text = "";
                        txtCmi.Text = "";
                        txtSmi.Text = "";
                        // calVerificationDate.ValueChanged -= new EventHandler(calVerificationDate_ValueChanged);
                        if (programDefinitionList != null)
                        {
                            if (programDefinitionList.DepFEDUsed == "Y")
                            {
                                // txtFedOmb.Text = "0";
                                calc_FEDOMB();
                            }
                            if (programDefinitionList.DepCMIUsed == "Y")
                            {
                                // txtCmi.Text = "0";
                                calc_Poverty("CMI");
                            }
                            if (programDefinitionList.DepSMIUsed == "Y")
                            {
                                // txtSmi.Text = "0";
                                calc_Poverty("SMI");
                            }
                            if (programDefinitionList.DepHUDUsed == "Y")
                            {
                                // txtHud.Text = "0";
                                calc_Poverty("HUD");
                            }
                        }


                        CaseVerEntity caseVerEntity = new CaseVerEntity();
                        caseVerEntity.Agency = BaseForm.BaseAgency;
                        caseVerEntity.Dept = BaseForm.BaseDept;
                        caseVerEntity.Program = BaseForm.BaseProg;
                        caseVerEntity.Year = BaseForm.BaseYear;
                        caseVerEntity.AppNo = BaseForm.BaseApplicationNo;
                        caseVerEntity.Classification = "00";

                        if (cmbVerifier.Items.Count > 0)
                            caseVerEntity.Verifier = ((Captain.Common.Utilities.ListItem)cmbVerifier.SelectedItem).Value.ToString();

                        caseVerEntity.VerOmb = txtFedOmb.Text == string.Empty ? "0" : txtFedOmb.Text;
                        caseVerEntity.VerHud = txtHud.Text == string.Empty ? "0" : txtHud.Text;
                        caseVerEntity.VerSmi = txtSmi.Text == string.Empty ? "0" : txtSmi.Text;
                        caseVerEntity.VerCmi = txtCmi.Text == string.Empty ? "0" : txtCmi.Text;
                        caseVerEntity.IncomeAmount = CaseMSTEntity.ProgIncome; ;
                        caseVerEntity.NoInhh = CaseMSTEntity.NoInProg;

                        caseVerEntity.VerifyOther = "Y";


                        caseVerEntity.ReverifyDate = string.Empty;
                        caseVerEntity.AddOperator = BaseForm.UserID;
                        caseVerEntity.LstcOperator = BaseForm.UserID;


                        if (propAgencyControlDetails.VerSwitch.ToUpper() == "Y")
                        {
                            caseVerEntity.VerifyDate = calVerificationDate.Value.ToString();//DateTime.Now.ToShortDateString();//
                            caseVerEntity.Mode = "Delete";
                            string strSqlMsg = string.Empty;
                            if (_model.CaseMstData.InsertUpdateDelCaseVer(caseVerEntity, out strSqlMsg))
                            {
                            }
                            caseVerEntity.Mode = Consts.Common.New;
                            if (_model.CaseMstData.InsertUpdateDelCaseVer(caseVerEntity, out strSqlMsg))
                            { }
                        }
                        else
                        {
                            caseVerEntity.VerifyDate = DateTime.Now.ToShortDateString();//
                            calVerificationDate.Value = DateTime.Now.Date;
                            caseVerEntity.Mode = "DeleteIncome";
                            string strSqlMsg = string.Empty;
                            if (_model.CaseMstData.InsertUpdateDelCaseVer(caseVerEntity, out strSqlMsg))
                            {
                            }
                            caseVerEntity.Mode = Consts.Common.New;
                            if (_model.CaseMstData.InsertUpdateDelCaseVer(caseVerEntity, out strSqlMsg))
                            { }

                        }





                    }

                    //dataGridCaseSnp_SelectionChanged(sender, e);
                    //}
                    //if (intgridloop == 0)
                    //{
                    //    CommonFunctions.MessageBoxDisplay("Atleast one Interval selected");
                    //    btnSave.Enabled = true;
                    //    btnAdd_Click(sender, e);
                    //}
                }

            }

        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            chkCheckStubs.Enabled = false;
            chkLetter.Enabled = false;
            chkOther.Enabled = false;
            chk_self_Decl.Enabled = false;
            chkTaxReturn.Enabled = false;
            chkw2.Enabled = false;


            chkCheckStubs.Checked = false;
            chkLetter.Checked = false;
            chkOther.Checked = false;
            chk_self_Decl.Checked = false;
            chkTaxReturn.Checked = false;
            chkw2.Checked = false;

            _errorProvider.SetError(dataGridCaseIncome, null);
            dataGridCaseIncome.ReadOnly = true;
            strMode = string.Empty;
            dataGridCaseSnp_SelectionChanged(sender, e);
            if (Privileges.DelPriv.Equals("false"))
            {
                dataGridCaseIncome.Columns["Delete"].Visible = false;
            }
            else
                dataGridCaseIncome.Columns["Delete"].Visible = true;
            MenuPropertie = string.Empty;

            CaseVerEntity caseVerList = _model.CaseMstData.GetCaseveradpynd(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, calVerificationDate.Value.ToString(), string.Empty);
            if (caseVerList != null)
            {
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
            }
        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            contextMenu1.MenuItems.Clear();
            if (dataGridCaseIncome.Rows.Count > 0)
            {
                if (dataGridCaseIncome.CurrentRow.Index >= 0)
                {
                    //foreach (DataGridViewRow dr in dataGridCaseIncome.Rows)
                    //{

                    //    if (dr.Selected)
                    //    {
                    if (MenuPropertie != string.Empty)
                    {
                        if (Convert.ToString(dataGridCaseIncome.CurrentRow.Cells["IncomeTypeCode"].Value) != string.Empty)
                        {
                            MenuItem menuItem = new MenuItem();
                            menuItem.Text = "Add Additional Income Type Row";
                            menuItem.Tag = dataGridCaseIncome.Rows[dataGridCaseIncome.CurrentRow.Index].Tag;
                            contextMenu1.MenuItems.Add(menuItem);
                        }
                        else
                        {
                            foreach (CommonEntity agyEntity in lookUpIncomeTypes)
                            {
                                MenuItem menuItem = new MenuItem();
                                menuItem.Text = agyEntity.Desc;
                                menuItem.Tag = agyEntity.Code;
                                contextMenu1.MenuItems.Add(menuItem);
                            }
                        }
                    }
                    // }
                    // }
                }
            }
            contextMenu1.Update();
        }

        private void dataGridCaseIncome_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            try
            {

                if (dataGridCaseIncome.Rows.Count > 0)
                {
                    //foreach (DataGridViewRow dr in dataGridCaseIncome.SelectedRows)
                    //{
                    //    if (dr.Selected)
                    //    {
                    // int index = dataGridCaseIncome.Rows[dataGridCaseIncome.CurrentRow.Index].Index;
                    if (dataGridCaseIncome.Rows[dataGridCaseIncome.CurrentRow.Index].Tag is CommonEntity)
                    {
                        // int index1 = dataGridCaseIncome.Rows.Add();
                        //  DataGridViewRow row = dataGridCaseIncome.Rows[index1];
                        CommonEntity drow = dataGridCaseIncome.Rows[dataGridCaseIncome.CurrentRow.Index].Tag as CommonEntity;
                        DataGridViewRow row = (DataGridViewRow)dataGridCaseIncome.Rows[dataGridCaseIncome.CurrentRow.Index].Clone();
                        // DataGridViewRow row = new DataGridViewRow();
                        row.Cells[3].Value = drow.Code;
                        row.Cells[4].Value = drow.Desc;
                        row.Cells[5].Value = string.Empty;
                        row.Cells[31].Value = drow.Extension;
                        
                        for (int i = 7; i < 25; i++)
                        {
                            row.Cells[i].Value = string.Empty;
                            row.Cells[i].ReadOnly = true;
                        }
                        row.Cells[22].Value = strCaseWorkerDefaultCode;
                        if (!string.IsNullOrEmpty(txtIncSource.Text.Trim()))
                        {
                            row.Cells[24].Value = txtIncSource.Text;
                            //dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["gvtIncomesource"].Value = txtIncSource.Text;
                        }
                        string[] strIncomeTypes = programDefinitionList.DepIncomeTypes.Split(' ');
                        bool boolAddExclude = false;
                        foreach (string incomeType in strIncomeTypes)
                        {
                            if (drow.Code == incomeType)
                            {
                                boolAddExclude = true;
                            }
                        }
                        if (boolAddExclude == true)
                            row.Cells[6].Value = true;
                        // dataGridCaseIncome.Rows.Add(row);
                        dataGridCaseIncome.Rows.Insert(dataGridCaseIncome.CurrentRow.Index + 1, row);
                        dataGridCaseIncome.Rows[dataGridCaseIncome.CurrentRow.Index + 1].Tag = drow;
                        changeFieldsNonRequirecolor(dataGridCaseIncome.CurrentRow.Index + 1);
                    }
                    else if (dataGridCaseIncome.Rows[dataGridCaseIncome.CurrentRow.Index].Tag is CaseIncomeEntity)
                    {
                        // int index1 = dataGridCaseIncome.Rows.Add();
                        //  DataGridViewRow row = dataGridCaseIncome.Rows[index1];
                        CaseIncomeEntity drow = dataGridCaseIncome.Rows[dataGridCaseIncome.CurrentRow.Index].Tag as CaseIncomeEntity;
                        if (drow != null)
                        {
                            if (drow.Type == string.Empty)
                            {
                                drow.Type = objArgs.MenuItem.Tag.ToString();
                                dataGridCaseIncome.Rows[dataGridCaseIncome.CurrentRow.Index].Cells["IncomeTypeCode"].Value = objArgs.MenuItem.Tag.ToString();
                                dataGridCaseIncome.Rows[dataGridCaseIncome.CurrentRow.Index].Cells["IncomeType"].Value = objArgs.MenuItem.Text.ToString();
                                dataGridCaseIncome.Rows[dataGridCaseIncome.CurrentRow.Index].Tag = drow;
                            }
                            else
                            {
                                DataGridViewRow row = (DataGridViewRow)dataGridCaseIncome.Rows[dataGridCaseIncome.CurrentRow.Index].Clone();
                                // DataGridViewRow row = new DataGridViewRow();
                                row.Cells[3].Value = drow.Type;
                                row.Cells[4].Value = LookupDataAccess.GetLookUpCode("00004", drow.Type);//, LookupDataAccess.ShowIncomeInterval(caseIncome.Interval)
                                row.Cells[5].Value = string.Empty;
                                for (int i = 7; i < 25; i++)
                                {
                                    row.Cells[i].Value = string.Empty;
                                    row.Cells[i].ReadOnly = true;
                                }
                                row.Cells[22].Value = strCaseWorkerDefaultCode;
                                row.Cells[23].Value = drow.HowVerified;
                                row.Cells[24].Value = drow.Source;
                                CommonEntity agytabsdata = lookUpIncomeTypes.Find(u => u.Code == drow.Type);
                                if (agytabsdata != null)
                                {
                                    row.Cells[31].Value = agytabsdata.Extension;
                                }
                                else
                                {
                                    row.Cells[31].Value = string.Empty;
                                }
                                row.Cells[32].Value = string.Empty;
                                string[] strIncomeTypes = programDefinitionList.DepIncomeTypes.Split(' ');
                                bool boolAddExclude = false;
                                foreach (string incomeType in strIncomeTypes)
                                {
                                    if (drow.Type == incomeType)
                                    {
                                        boolAddExclude = true;
                                    }
                                }
                                if (boolAddExclude == true)
                                    row.Cells[6].Value = true;
                                // dataGridCaseIncome.Rows.Add(row);
                                dataGridCaseIncome.Rows.Insert(dataGridCaseIncome.CurrentRow.Index + 1, row);
                                dataGridCaseIncome.Rows[dataGridCaseIncome.CurrentRow.Index + 1].Tag = drow;
                                changeFieldsNonRequirecolor(dataGridCaseIncome.CurrentRow.Index + 1);
                            }
                        }
                    }
                    //dataGridCaseIncome.RefreshEdit();
                    dataGridCaseIncome.Update();


                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {

            }
        }

        object _sender = null;
        EventArgs _e = null;
        private void dataGridCaseIncome_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == Delete.Index && e.RowIndex != -1)
                {
                    //if (dataGridCaseIncome.SelectedRows.Count > 0)
                    //{
                    if (dataGridCaseIncome.Rows[0].Tag is CaseIncomeEntity)
                    {
                        CaseIncomeEntity row = dataGridCaseIncome.Rows[dataGridCaseIncome.CurrentRow.Index].Tag as CaseIncomeEntity;
                        if (row != null)
                        {
                            _sender = sender; _e = e;
                            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: OnDeleteMessageBoxClicked);

                        }
                    }
                    //}
                }
                else if (e.ColumnIndex == gvchkType.Index && e.RowIndex != -1)
                {
                    if (MenuPropertie != string.Empty)
                    {
                        if (dataGridCaseIncome.Rows[e.RowIndex].Cells["Status"].Value.ToString() == "U")
                        {

                            string Val1 = "0";
                            string Val2 = "0";
                            string Val3 = "0";
                            string Val4 = "0";
                            string Val5 = "0";
                            string Date1 = string.Empty;
                            string Date2 = string.Empty;
                            string Date3 = string.Empty;
                            string Date4 = string.Empty;
                            string Date5 = string.Empty;

                            if (dataGridCaseIncome.Rows[e.RowIndex].Cells["Amt1"].Value.ToString() != string.Empty)
                                Val1 = dataGridCaseIncome.Rows[e.RowIndex].Cells["Amt1"].Value.ToString();

                            if (dataGridCaseIncome.Rows[e.RowIndex].Cells["Amt2"].Value.ToString() != string.Empty)
                                Val2 = dataGridCaseIncome.Rows[e.RowIndex].Cells["Amt2"].Value.ToString();

                            if (dataGridCaseIncome.Rows[e.RowIndex].Cells["Amt3"].Value.ToString() != string.Empty)
                                Val3 = dataGridCaseIncome.Rows[e.RowIndex].Cells["Amt3"].Value.ToString();

                            if (dataGridCaseIncome.Rows[e.RowIndex].Cells["Amt4"].Value.ToString() != string.Empty)
                                Val4 = dataGridCaseIncome.Rows[e.RowIndex].Cells["Amt4"].Value.ToString();

                            if (dataGridCaseIncome.Rows[e.RowIndex].Cells["Amt5"].Value.ToString() != string.Empty)
                                Val5 = dataGridCaseIncome.Rows[e.RowIndex].Cells["Amt5"].Value.ToString();

                            if (dataGridCaseIncome.Rows[e.RowIndex].Cells["Date1"].Value.ToString() != string.Empty && dataGridCaseIncome.Rows[e.RowIndex].Cells["Date1"].Value.ToString().Trim() != "/  /")
                                Date1 = dataGridCaseIncome.Rows[e.RowIndex].Cells["Date1"].Value.ToString();

                            if (dataGridCaseIncome.Rows[e.RowIndex].Cells["Date2"].Value.ToString() != string.Empty && dataGridCaseIncome.Rows[e.RowIndex].Cells["Date2"].Value.ToString().Trim() != "/  /")
                                Date2 = dataGridCaseIncome.Rows[e.RowIndex].Cells["Date2"].Value.ToString();

                            if (dataGridCaseIncome.Rows[e.RowIndex].Cells["Date3"].Value.ToString() != string.Empty && dataGridCaseIncome.Rows[e.RowIndex].Cells["Date3"].Value.ToString().Trim() != "/  /")
                                Date3 = dataGridCaseIncome.Rows[e.RowIndex].Cells["Date3"].Value.ToString();

                            if (dataGridCaseIncome.Rows[e.RowIndex].Cells["Date4"].Value.ToString() != string.Empty && dataGridCaseIncome.Rows[e.RowIndex].Cells["Date4"].Value.ToString().Trim() != "/  /")
                                Date4 = dataGridCaseIncome.Rows[e.RowIndex].Cells["Date4"].Value.ToString();

                            if (dataGridCaseIncome.Rows[e.RowIndex].Cells["Date5"].Value.ToString() != string.Empty && dataGridCaseIncome.Rows[e.RowIndex].Cells["Date5"].Value.ToString().Trim() != "/  /")
                                Date5 = dataGridCaseIncome.Rows[e.RowIndex].Cells["Date5"].Value.ToString();


                            if (((Convert.ToDecimal(Val1) > 0 || Convert.ToDecimal(Val2) > 0 || Convert.ToDecimal(Val3) > 0 || Convert.ToDecimal(Val4) > 0 || Convert.ToDecimal(Val5) > 0) || (Date1 != string.Empty || Date2 != string.Empty || Date3 != string.Empty || Date4 != string.Empty || Date5 != string.Empty) || dataGridCaseIncome.Rows[e.RowIndex].Cells["Interval"].Value.ToString() != "E") && (dataGridCaseIncome.Rows[e.RowIndex].Cells["Interval"].Value.ToString() != string.Empty))
                            {
                                dataGridCaseIncome.Rows[e.RowIndex].Cells["gvchkType"].Value = true;
                            }
                            else
                            {
                                bool booltype = dataGridCaseIncome.Rows[e.RowIndex].Cells["gvchkType"].Value == null ? false : Convert.ToBoolean(dataGridCaseIncome.Rows[e.RowIndex].Cells["gvchkType"].Value);
                                if (booltype == true)
                                {
                                    dataGridCaseIncome.Rows[e.RowIndex].Cells["Interval"].Value = "E";
                                    dataGridCaseIncome.Rows[e.RowIndex].Cells["gvchkType"].Value = true;
                                }
                                else
                                {
                                    dataGridCaseIncome.Rows[e.RowIndex].Cells["Interval"].Value = "";
                                    dataGridCaseIncome.Rows[e.RowIndex].Cells["gvchkType"].Value = false;
                                }

                            }
                        }

                        else
                        {

                            bool booltype = dataGridCaseIncome.Rows[e.RowIndex].Cells["gvchkType"].Value == null ? false : Convert.ToBoolean(dataGridCaseIncome.Rows[e.RowIndex].Cells["gvchkType"].Value);
                            if (booltype == true)
                            {
                                dataGridCaseIncome.Rows[e.RowIndex].Cells["Interval"].Value = "E";
                            }
                            else
                            {
                                dataGridCaseIncome.Rows[e.RowIndex].Cells["Interval"].Value = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnDeleteMessageBoxClicked(DialogResult dialogResult)
        {

            if (dialogResult == DialogResult.Yes)
            {
                CaseIncomeEntity caseIncomeEntity = new CaseIncomeEntity();
                caseIncomeEntity.Agency = BaseForm.BaseAgency;
                caseIncomeEntity.Dept = BaseForm.BaseDept;
                caseIncomeEntity.Program = BaseForm.BaseProg;
                caseIncomeEntity.Year = BaseForm.BaseYear;
                caseIncomeEntity.App = BaseForm.BaseApplicationNo;
                caseIncomeEntity.FamilySeq = Convert.ToString(dataGridCaseSnp.SelectedRows[0].Cells["SNP_FAMILY_SEQ"].Value); ;
                caseIncomeEntity.Seq = Convert.ToString(dataGridCaseIncome.Rows[dataGridCaseIncome.CurrentRow.Index].Cells["INCOMESEQ"].Value); ;
                if (_model.CaseMstData.DeleteCaseIncome(caseIncomeEntity))
                {
                    BaseForm.BaseCaseMstListEntity = _model.CaseMstData.GetCaseMstadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
                    BaseForm.BaseCaseSnpEntity = _model.CaseMstData.GetCaseSnpadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
                    BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq)).M_Code = "A";
                    CaseMSTEntity = BaseForm.BaseCaseMstListEntity[0];
                    fillSnpDetails(false);

                    AlertBox.Show("Income Details Deleted Successfully");

                    int _strSNPindex = strSnpIndex;
                    if (dataGridCaseSnp.Rows.Count != 0)
                    {
                        dataGridCaseSnp.Rows[strSnpIndex].Selected = true;
                    }
                    if (_strSNPindex != strSnpIndex)
                    {

                        dataGridCaseSnp.ClearSelection();
                        dataGridCaseSnp.Rows[_strSNPindex].Selected = true;
                    }

                    if (propAgencyControlDetails.IncVerfication.ToUpper() == "Y")
                    {
                        // calVerificationDate.Value = DateTime.Now.Date;
                        txtFedOmb.Text = "";
                        txtHud.Text = "";
                        txtCmi.Text = "";
                        txtSmi.Text = "";
                        // calVerificationDate.ValueChanged -= new EventHandler(calVerificationDate_ValueChanged);
                        if (programDefinitionList != null)
                        {
                            if (programDefinitionList.DepFEDUsed == "Y")
                            {
                                // txtFedOmb.Text = "0";
                                calc_FEDOMB();
                            }
                            if (programDefinitionList.DepCMIUsed == "Y")
                            {
                                // txtCmi.Text = "0";
                                calc_Poverty("CMI");
                            }
                            if (programDefinitionList.DepSMIUsed == "Y")
                            {
                                // txtSmi.Text = "0";
                                calc_Poverty("SMI");
                            }
                            if (programDefinitionList.DepHUDUsed == "Y")
                            {
                                // txtHud.Text = "0";
                                calc_Poverty("HUD");
                            }
                        }


                        CaseVerEntity caseVerEntity = new CaseVerEntity();
                        caseVerEntity.Agency = BaseForm.BaseAgency;
                        caseVerEntity.Dept = BaseForm.BaseDept;
                        caseVerEntity.Program = BaseForm.BaseProg;
                        caseVerEntity.Year = BaseForm.BaseYear;
                        caseVerEntity.AppNo = BaseForm.BaseApplicationNo;
                        caseVerEntity.Classification = "00";

                        if (cmbVerifier.Items.Count > 0)
                            caseVerEntity.Verifier = ((Captain.Common.Utilities.ListItem)cmbVerifier.SelectedItem).Value.ToString();
                        caseVerEntity.VerOmb = txtFedOmb.Text == string.Empty ? "0" : txtFedOmb.Text;
                        caseVerEntity.VerHud = txtHud.Text == string.Empty ? "0" : txtHud.Text;
                        caseVerEntity.VerSmi = txtSmi.Text == string.Empty ? "0" : txtSmi.Text;
                        caseVerEntity.VerCmi = txtCmi.Text == string.Empty ? "0" : txtCmi.Text;
                        caseVerEntity.IncomeAmount = CaseMSTEntity.ProgIncome; ;
                        caseVerEntity.NoInhh = CaseMSTEntity.NoInProg;

                        caseVerEntity.VerifyOther = "Y";


                        caseVerEntity.ReverifyDate = string.Empty;
                        caseVerEntity.AddOperator = BaseForm.UserID;
                        caseVerEntity.LstcOperator = BaseForm.UserID;


                        if (propAgencyControlDetails.VerSwitch.ToUpper() == "Y")
                        {
                            caseVerEntity.VerifyDate = calVerificationDate.Value.ToString();////DateTime.Now.ToShortDateString();//
                            caseVerEntity.Mode = "Delete";
                            string strSqlMsg = string.Empty;
                            if (_model.CaseMstData.InsertUpdateDelCaseVer(caseVerEntity, out strSqlMsg))
                            {
                            }

                            caseVerEntity.Mode = Consts.Common.New;
                            if (_model.CaseMstData.InsertUpdateDelCaseVer(caseVerEntity, out strSqlMsg))
                            { }
                        }
                        else
                        {
                            caseVerEntity.VerifyDate = DateTime.Now.ToShortDateString();//
                            calVerificationDate.Value = DateTime.Now.Date;
                            caseVerEntity.Mode = "DeleteIncome";
                            string strSqlMsg = string.Empty;
                            if (_model.CaseMstData.InsertUpdateDelCaseVer(caseVerEntity, out strSqlMsg))
                            {
                            }
                            caseVerEntity.Mode = Consts.Common.New;
                            if (_model.CaseMstData.InsertUpdateDelCaseVer(caseVerEntity, out strSqlMsg))
                            { }

                        }





                    }


                    dataGridCaseSnp_SelectionChanged(_sender, _e);
                }
            }
        }

        private void calVerificationDate_ValueChanged(object sender, EventArgs e)
        {
            bool boolchkstatus = true;
            if (propAgencyControlDetails.IncVerfication.ToUpper() == "Y")
            {
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

                    //if (boolchkstatus)
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
                    //                    calVerificationDate.ValueChanged -= new EventHandler(calVerificationDate_ValueChanged);
                    //                    calVerificationDate.Checked = false;
                    //                    //  calVerificationDate.ValueChanged += new EventHandler(calVerificationDate_ValueChanged);
                    //                    break;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    txtFedOmb.Text = "";
                    txtHud.Text = "";
                    txtCmi.Text = "";
                    txtSmi.Text = "";
                    // calVerificationDate.ValueChanged -= new EventHandler(calVerificationDate_ValueChanged);


                    // calVerificationDate.ValueChanged -= new EventHandler(calVerificationDate_ValueChanged);
                    if (programDefinitionList != null)
                    {
                        if (programDefinitionList.DepFEDUsed == "Y")
                        {
                            // txtFedOmb.Text = "0";
                            calc_FEDOMB();
                        }
                        if (programDefinitionList.DepCMIUsed == "Y")
                        {
                            // txtCmi.Text = "0";
                            calc_Poverty("CMI");
                        }
                        if (programDefinitionList.DepSMIUsed == "Y")
                        {
                            // txtSmi.Text = "0";
                            calc_Poverty("SMI");
                        }
                        if (programDefinitionList.DepHUDUsed == "Y")
                        {
                            // txtHud.Text = "0";
                            calc_Poverty("HUD");
                        }

                    }
                    //calVerificationDate.ValueChanged += new EventHandler(calVerificationDate_ValueChanged);
                }
            }
        }


        private void disableenableFields(string strInterval, int rowindex)
        {
            changeAllReadonlyTrue(rowindex);
            // changeFieldsNonRequirecolor(rowindex);
            if (propIncomeTypeSwitch != "Y")
            {
                if (strInterval == "0" || strInterval == "O" || strInterval == "A" || strInterval == "Q" || strInterval == "E") // added "E" logic 04/02/2019
                {
                    if (Amt1Enable)
                        dataGridCaseIncome.Rows[rowindex].Cells["Amt1"].ReadOnly = false;
                    if (Date1Enable)
                        dataGridCaseIncome.Rows[rowindex].Cells["Date1"].ReadOnly = false;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt1"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date1"].Style.BackColor = System.Drawing.Color.AntiqueWhite;

                    dataGridCaseIncome.Rows[rowindex].Cells["Amt2"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt4"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt5"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt2"].Style.BackColor = System.Drawing.Color.Transparent;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].Style.BackColor = System.Drawing.Color.White;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt4"].Style.BackColor = System.Drawing.Color.White;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt5"].Style.BackColor = System.Drawing.Color.White;

                    dataGridCaseIncome.Rows[rowindex].Cells["Date2"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date3"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date4"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date5"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date2"].Style.BackColor = System.Drawing.Color.White;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date3"].Style.BackColor = System.Drawing.Color.White;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date4"].Style.BackColor = System.Drawing.Color.White;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date5"].Style.BackColor = System.Drawing.Color.White;
                }
                else if (strInterval == "B" || strInterval == "S" || strInterval == "M" || strInterval == "N")
                {
                    if (Amt1Enable)
                        dataGridCaseIncome.Rows[rowindex].Cells["Amt1"].ReadOnly = false;
                    if (Date1Enable)
                        dataGridCaseIncome.Rows[rowindex].Cells["Date1"].ReadOnly = false;
                    if (Amt2Enable)
                        dataGridCaseIncome.Rows[rowindex].Cells["Amt2"].ReadOnly = false;
                    if (Date2Enable)
                        dataGridCaseIncome.Rows[rowindex].Cells["Date2"].ReadOnly = false;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt1"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date1"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt2"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date2"].Style.BackColor = System.Drawing.Color.AntiqueWhite;


                    dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt4"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt5"].ReadOnly = true;

                    dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].Style.BackColor = System.Drawing.Color.White;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt4"].Style.BackColor = System.Drawing.Color.White;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt5"].Style.BackColor = System.Drawing.Color.White;

                    dataGridCaseIncome.Rows[rowindex].Cells["Date3"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date4"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date5"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date3"].Style.BackColor = System.Drawing.Color.White;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date4"].Style.BackColor = System.Drawing.Color.White;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date5"].Style.BackColor = System.Drawing.Color.White;

                    if (strInterval == "B" && propHourlyMode == "Y")
                    {
                        if (Amt3Enable)
                            dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].ReadOnly = false;
                        if (Date3Enable)
                            dataGridCaseIncome.Rows[rowindex].Cells["Date3"].ReadOnly = false;
                        dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                        dataGridCaseIncome.Rows[rowindex].Cells["Date3"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                    }
                    if (strInterval == "B" && propAgencyControlDetails.IncMethods == "1")
                    {
                        if (Amt3Enable)
                            dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].ReadOnly = false;
                        if (Date3Enable)
                            dataGridCaseIncome.Rows[rowindex].Cells["Date3"].ReadOnly = false;

                        dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                        dataGridCaseIncome.Rows[rowindex].Cells["Date3"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                    }
                    if (strInterval == "S" && propHourlyMode == "Y")
                    {
                        if (Amt3Enable)
                            dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].ReadOnly = false;
                        if (Date3Enable)
                            dataGridCaseIncome.Rows[rowindex].Cells["Date3"].ReadOnly = false;
                        //dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                        //dataGridCaseIncome.Rows[rowindex].Cells["Date3"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                    }


                }
                //else if (strInterval == "E")
                //{
                //    changeAllReadonlyfalse(rowindex);
                //    changeFieldsReqcolor(rowindex);
                //}
                else if (strInterval == "W")
                {
                    changeAllReadonlyfalse(rowindex);
                    changeFieldsReqcolor(rowindex);
                }
                else if (strInterval == "H")
                {
                    changeAllReadonlyfalse(rowindex);
                    changeFieldsReqcolor(rowindex);
                }
                else if (string.IsNullOrEmpty(strInterval))
                {
                    changeAllReadonlyTrue(rowindex);
                    changeFieldsNonRequirecolor(rowindex);
                }

                else if (strInterval == "3" || strInterval == "6" || strInterval == "9")
                {
                    lblMstIntakeDate.Visible = true;
                    changeAllReadonlyfalse(rowindex);
                    changeFieldsReqcolor(rowindex);
                }
            }
        }

        private void RequiredfieldColorchanges(string strInterval, int rowindex, DataGridViewRow dataInterval)
        {
            if (propIncomeTypeSwitch != "Y")
            {

                if (strInterval == "0" || strInterval == "O" || strInterval == "A" || strInterval == "Q" || strInterval == "E") // added "E" 04/02/2019
                {
                    if (Amt1Enable)
                        dataGridCaseIncome.Rows[rowindex].Cells["Amt1"].ReadOnly = false;
                    if (Date1Enable)
                        dataGridCaseIncome.Rows[rowindex].Cells["Date1"].ReadOnly = false;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt1"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date1"].Style.BackColor = System.Drawing.Color.AntiqueWhite;

                    dataGridCaseIncome.Rows[rowindex].Cells["Amt2"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt4"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt5"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt2"].Style.BackColor = System.Drawing.Color.White;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].Style.BackColor = System.Drawing.Color.White;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt4"].Style.BackColor = System.Drawing.Color.White;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt5"].Style.BackColor = System.Drawing.Color.White;

                    dataGridCaseIncome.Rows[rowindex].Cells["Date2"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date3"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date4"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date5"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date2"].Style.BackColor = System.Drawing.Color.White;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date3"].Style.BackColor = System.Drawing.Color.White;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date4"].Style.BackColor = System.Drawing.Color.White;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date5"].Style.BackColor = System.Drawing.Color.White;
                }
                else if (strInterval == "B" || strInterval == "S" || strInterval == "M" || strInterval == "N")
                {
                    if (Amt1Enable)
                        dataGridCaseIncome.Rows[rowindex].Cells["Amt1"].ReadOnly = false;
                    if (Date1Enable)
                        dataGridCaseIncome.Rows[rowindex].Cells["Date1"].ReadOnly = false;
                    if (Amt2Enable)
                        dataGridCaseIncome.Rows[rowindex].Cells["Amt2"].ReadOnly = false;
                    if (Date2Enable)
                        dataGridCaseIncome.Rows[rowindex].Cells["Date2"].ReadOnly = false;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt1"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date1"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt2"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date2"].Style.BackColor = System.Drawing.Color.AntiqueWhite;

                    dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt4"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt5"].ReadOnly = true;

                    dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].Style.BackColor = System.Drawing.Color.White;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt4"].Style.BackColor = System.Drawing.Color.White;
                    dataGridCaseIncome.Rows[rowindex].Cells["Amt5"].Style.BackColor = System.Drawing.Color.White;

                    dataGridCaseIncome.Rows[rowindex].Cells["Date3"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date4"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date5"].ReadOnly = true;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date3"].Style.BackColor = System.Drawing.Color.White;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date4"].Style.BackColor = System.Drawing.Color.White;
                    dataGridCaseIncome.Rows[rowindex].Cells["Date5"].Style.BackColor = System.Drawing.Color.White;

                    if (strInterval == "B" && propHourlyMode == "Y")
                    {
                        if (Amt3Enable)
                            dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].ReadOnly = false;
                        if (Date3Enable)
                            dataGridCaseIncome.Rows[rowindex].Cells["Date3"].ReadOnly = false;
                        dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                        dataGridCaseIncome.Rows[rowindex].Cells["Date3"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                    }
                    if (strInterval == "B" && propAgencyControlDetails.IncMethods == "1")
                    {
                        if (Amt3Enable)
                            dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].ReadOnly = false;
                        if (Date3Enable)
                            dataGridCaseIncome.Rows[rowindex].Cells["Date3"].ReadOnly = false;
                        dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                        dataGridCaseIncome.Rows[rowindex].Cells["Date3"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                    }

                }
                else if (strInterval == "E")
                {
                    changeAllReadonlyfalse(rowindex);
                    changeFieldsReqcolor(rowindex);
                }
                else if (strInterval == "W")
                {
                    changeAllReadonlyfalse(rowindex);
                    changeFieldsReqcolor(rowindex);
                }
                else if (strInterval == "H")
                {
                    changeAllReadonlyfalse(rowindex);
                    changeFieldsReqcolor(rowindex);
                }
                else if (strInterval == "3" || strInterval == "6" || strInterval == "9")
                {
                    lblMstIntakeDate.Visible = true;
                    changeAllReadonlyfalse(rowindex);
                    changeFieldsReqcolor(rowindex);
                    if ((!string.IsNullOrEmpty(dataInterval.Cells["Date1"].Value.ToString())) && (dataInterval.Cells["Date1"].Value.ToString().Trim() != "/  /"))
                        if (!CheckMstIntakeDate(Convert.ToDateTime(dataInterval.Cells["Date1"].Value).Date, strInterval))
                            dataGridCaseIncome.Rows[rowindex].Cells["Date1"].Style.ForeColor = System.Drawing.Color.Red;

                    if ((!string.IsNullOrEmpty(dataInterval.Cells["Date2"].Value.ToString())) && (dataInterval.Cells["Date2"].Value.ToString().Trim() != "/  /"))
                        if (!CheckMstIntakeDate(Convert.ToDateTime(dataInterval.Cells["Date2"].Value).Date, strInterval))
                            dataGridCaseIncome.Rows[rowindex].Cells["Date2"].Style.ForeColor = System.Drawing.Color.Red;

                    if ((!string.IsNullOrEmpty(dataInterval.Cells["Date3"].Value.ToString())) && (dataInterval.Cells["Date3"].Value.ToString().Trim() != "/  /"))
                        if (!CheckMstIntakeDate(Convert.ToDateTime(dataInterval.Cells["Date3"].Value).Date, strInterval))
                            dataGridCaseIncome.Rows[rowindex].Cells["Date3"].Style.ForeColor = System.Drawing.Color.Red;

                    if ((!string.IsNullOrEmpty(dataInterval.Cells["Date4"].Value.ToString())) && (dataInterval.Cells["Date4"].Value.ToString().Trim() != "/  /"))
                        if (!CheckMstIntakeDate(Convert.ToDateTime(dataInterval.Cells["Date4"].Value).Date, strInterval))
                            dataGridCaseIncome.Rows[rowindex].Cells["Date4"].Style.ForeColor = System.Drawing.Color.Red;

                    if ((!string.IsNullOrEmpty(dataInterval.Cells["Date5"].Value.ToString())) && (dataInterval.Cells["Date5"].Value.ToString().Trim() != "/  /"))
                        if (!CheckMstIntakeDate(Convert.ToDateTime(dataInterval.Cells["Date5"].Value).Date, strInterval))
                            dataGridCaseIncome.Rows[rowindex].Cells["Date5"].Style.ForeColor = System.Drawing.Color.Red;
                }
                if (propHourlyMode.ToString() == "Y")
                {
                    if ((!string.IsNullOrEmpty(dataInterval.Cells["Date1"].Value.ToString())) && (dataInterval.Cells["Date1"].Value.ToString().Trim() != "/  /"))
                        if (!CheckMstIntakeDate(Convert.ToDateTime(dataInterval.Cells["Date1"].Value).Date, strInterval))
                            dataGridCaseIncome.Rows[rowindex].Cells["Date1"].Style.ForeColor = System.Drawing.Color.Red;

                    if ((!string.IsNullOrEmpty(dataInterval.Cells["Date2"].Value.ToString())) && (dataInterval.Cells["Date2"].Value.ToString().Trim() != "/  /"))
                        if (!CheckMstIntakeDate(Convert.ToDateTime(dataInterval.Cells["Date2"].Value).Date, strInterval))
                            dataGridCaseIncome.Rows[rowindex].Cells["Date2"].Style.ForeColor = System.Drawing.Color.Red;

                    if ((!string.IsNullOrEmpty(dataInterval.Cells["Date3"].Value.ToString())) && (dataInterval.Cells["Date3"].Value.ToString().Trim() != "/  /"))
                        if (!CheckMstIntakeDate(Convert.ToDateTime(dataInterval.Cells["Date3"].Value).Date, strInterval))
                            dataGridCaseIncome.Rows[rowindex].Cells["Date3"].Style.ForeColor = System.Drawing.Color.Red;

                    if ((!string.IsNullOrEmpty(dataInterval.Cells["Date4"].Value.ToString())) && (dataInterval.Cells["Date4"].Value.ToString().Trim() != "/  /"))
                        if (!CheckMstIntakeDate(Convert.ToDateTime(dataInterval.Cells["Date4"].Value).Date, strInterval))
                            dataGridCaseIncome.Rows[rowindex].Cells["Date4"].Style.ForeColor = System.Drawing.Color.Red;

                    if ((!string.IsNullOrEmpty(dataInterval.Cells["Date5"].Value.ToString())) && (dataInterval.Cells["Date5"].Value.ToString().Trim() != "/  /"))
                        if (!CheckMstIntakeDate(Convert.ToDateTime(dataInterval.Cells["Date5"].Value).Date, strInterval))
                            dataGridCaseIncome.Rows[rowindex].Cells["Date5"].Style.ForeColor = System.Drawing.Color.Red;
                }

            }
        }

        private void changeAllReadonlyfalse(int rowindex)
        {
            if (Amt1Enable)
                dataGridCaseIncome.Rows[rowindex].Cells["Amt1"].ReadOnly = false;
            if (Amt2Enable)
                dataGridCaseIncome.Rows[rowindex].Cells["Amt2"].ReadOnly = false;
            if (Amt3Enable)
                dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].ReadOnly = false;
            if (Amt4Enable)
                dataGridCaseIncome.Rows[rowindex].Cells["Amt4"].ReadOnly = false;
            if (Amt5Enable)
                dataGridCaseIncome.Rows[rowindex].Cells["Amt5"].ReadOnly = false;
            if (Date1Enable)
                dataGridCaseIncome.Rows[rowindex].Cells["Date1"].ReadOnly = false;
            if (Date2Enable)
                dataGridCaseIncome.Rows[rowindex].Cells["Date2"].ReadOnly = false;
            if (Date3Enable)
                dataGridCaseIncome.Rows[rowindex].Cells["Date3"].ReadOnly = false;
            if (Date4Enable)
                dataGridCaseIncome.Rows[rowindex].Cells["Date4"].ReadOnly = false;
            if (Date5Enable)
                dataGridCaseIncome.Rows[rowindex].Cells["Date5"].ReadOnly = false;


        }

        private void changeAllReadonlyTrue(int rowindex)
        {

            dataGridCaseIncome.Rows[rowindex].Cells["Amt1"].ReadOnly = true;
            dataGridCaseIncome.Rows[rowindex].Cells["Amt2"].ReadOnly = true;
            dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].ReadOnly = true;
            dataGridCaseIncome.Rows[rowindex].Cells["Amt4"].ReadOnly = true;
            dataGridCaseIncome.Rows[rowindex].Cells["Amt5"].ReadOnly = true;
            dataGridCaseIncome.Rows[rowindex].Cells["Date1"].ReadOnly = true;
            dataGridCaseIncome.Rows[rowindex].Cells["Date2"].ReadOnly = true;
            dataGridCaseIncome.Rows[rowindex].Cells["Date3"].ReadOnly = true;
            dataGridCaseIncome.Rows[rowindex].Cells["Date4"].ReadOnly = true;
            dataGridCaseIncome.Rows[rowindex].Cells["Date5"].ReadOnly = true;


        }

        private void changeFieldsReqcolor(int rowindex)
        {
            dataGridCaseIncome.Rows[rowindex].Cells["Amt1"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
            dataGridCaseIncome.Rows[rowindex].Cells["Date1"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
            dataGridCaseIncome.Rows[rowindex].Cells["Amt2"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
            dataGridCaseIncome.Rows[rowindex].Cells["Date2"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
            dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
            dataGridCaseIncome.Rows[rowindex].Cells["Amt4"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
            
            dataGridCaseIncome.Rows[rowindex].Cells["Date3"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
            dataGridCaseIncome.Rows[rowindex].Cells["Date4"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
            //if (propHourlyMode != "Y")
            //{
                dataGridCaseIncome.Rows[rowindex].Cells["Amt5"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                dataGridCaseIncome.Rows[rowindex].Cells["Date5"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
            //}
        }

        private void changeFieldsNonRequirecolor(int rowindex)
        {
            dataGridCaseIncome.Rows[rowindex].Cells["Amt1"].Style.BackColor = System.Drawing.Color.White;
            dataGridCaseIncome.Rows[rowindex].Cells["Date1"].Style.BackColor = System.Drawing.Color.White;
            dataGridCaseIncome.Rows[rowindex].Cells["Amt2"].Style.BackColor = System.Drawing.Color.White;
            dataGridCaseIncome.Rows[rowindex].Cells["Date2"].Style.BackColor = System.Drawing.Color.White;
            dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].Style.BackColor = System.Drawing.Color.White;
            dataGridCaseIncome.Rows[rowindex].Cells["Amt4"].Style.BackColor = System.Drawing.Color.White;
            dataGridCaseIncome.Rows[rowindex].Cells["Amt5"].Style.BackColor = System.Drawing.Color.White;
            dataGridCaseIncome.Rows[rowindex].Cells["Date3"].Style.BackColor = System.Drawing.Color.White;
            dataGridCaseIncome.Rows[rowindex].Cells["Date4"].Style.BackColor = System.Drawing.Color.White;
            dataGridCaseIncome.Rows[rowindex].Cells["Date5"].Style.BackColor = System.Drawing.Color.White;
        }
        private void changeAllFieldsClear(int rowindex)
        {
            // dataGridCaseIncome.CellValueChanged -=new DataGridViewCellEventHandler(dataGridCaseIncome_CellValueChanged);

            dataGridCaseIncome.Rows[rowindex].Cells["Amt1"].Value = string.Empty;
            dataGridCaseIncome.Rows[rowindex].Cells["Amt2"].Value = string.Empty;
            dataGridCaseIncome.Rows[rowindex].Cells["Amt3"].Value = string.Empty;
            dataGridCaseIncome.Rows[rowindex].Cells["Amt4"].Value = string.Empty;
            dataGridCaseIncome.Rows[rowindex].Cells["Amt5"].Value = string.Empty;
            dataGridCaseIncome.Rows[rowindex].Cells["Date1"].Value = string.Empty;
            dataGridCaseIncome.Rows[rowindex].Cells["Date2"].Value = string.Empty;
            dataGridCaseIncome.Rows[rowindex].Cells["Date3"].Value = string.Empty;
            dataGridCaseIncome.Rows[rowindex].Cells["Date4"].Value = string.Empty;
            dataGridCaseIncome.Rows[rowindex].Cells["Date5"].Value = string.Empty;
            dataGridCaseIncome.CellValueChanged -= new DataGridViewCellEventHandler(dataGridCaseIncome_CellValueChanged);
            dataGridCaseIncome.Rows[rowindex].Cells["Factor"].Value = string.Empty;
            dataGridCaseIncome.Rows[rowindex].Cells["Sub"].Value = string.Empty;
            dataGridCaseIncome.Rows[rowindex].Cells["Total"].Value = string.Empty;
            lblMstIntakeDate.Text = string.Empty;
            // dataGridCaseIncome.CellValueChanged += new DataGridViewCellEventHandler(dataGridCaseIncome_CellValueChanged);

        }


        private void EnableDisableControls()
        {

            if (propIncomeTypeSwitch != "Y")
            {

                if (FLDCNTLHieEntity.Count > 0)
                {
                    foreach (FldcntlHieEntity entity in FLDCNTLHieEntity)
                    {
                        bool required = entity.Req.Equals("Y") ? true : false;
                        bool enabled = entity.Enab.Equals("Y") ? true : false;

                        switch (entity.FldDesc)
                        {

                            case Consts.CASEINCOME.IncomeType:
                                if (enabled) { IncomeTypeEnable = true; dataGridCaseIncome.Columns["IncomeType"].ReadOnly = true; if (required) { dataGridCaseIncome.Columns["IncomeType"].HeaderText = "* IncomeType"; IncomeTypeReq = true; } } else { dataGridCaseIncome.Columns["IncomeType"].ReadOnly = true; IncomeTypeReq = false; }
                                break;
                            case Consts.CASEINCOME.Interval:
                                if (enabled) { IntervalEnable = true; dataGridCaseIncome.Columns["Interval"].ReadOnly = false; if (required) { dataGridCaseIncome.Columns["Interval"].HeaderText = "* Interval"; IntervalReq = true; } } else { dataGridCaseIncome.Columns["Interval"].ReadOnly = true; IntervalReq = false; }
                                break;
                            case Consts.CASEINCOME.Value1:
                                if (enabled) { Amt1Enable = true; dataGridCaseIncome.Columns["Amt1"].ReadOnly = false; if (required) { dataGridCaseIncome.Columns["Amt1"].HeaderText = "* Amt1"; Amt1Req = true; } } else { dataGridCaseIncome.Columns["Amt1"].ReadOnly = true; Amt1Req = false; }
                                break;
                            case Consts.CASEINCOME.Value2:
                                if (enabled) { Amt2Enable = true; dataGridCaseIncome.Columns["Amt2"].ReadOnly = false; if (required) { dataGridCaseIncome.Columns["Amt2"].HeaderText = "* Amt2"; Amt2Req = true; } } else { dataGridCaseIncome.Columns["Amt2"].ReadOnly = true; Amt2Req = false; }
                                break;
                            case Consts.CASEINCOME.Value3:
                                if (enabled) { Amt3Enable = true; dataGridCaseIncome.Columns["Amt3"].ReadOnly = false; if (required) { dataGridCaseIncome.Columns["Amt3"].HeaderText = "* Amt3"; Amt3Req = true; } } else { dataGridCaseIncome.Columns["Amt3"].ReadOnly = true; Amt3Req = false; }
                                break;
                            case Consts.CASEINCOME.Value4:
                                if (enabled) { Amt4Enable = true; dataGridCaseIncome.Columns["Amt4"].ReadOnly = false; if (required) { dataGridCaseIncome.Columns["Amt4"].HeaderText = "* Amt4"; Amt4Req = true; } } else { dataGridCaseIncome.Columns["Amt4"].ReadOnly = true; Amt4Req = false; }
                                break;
                            case Consts.CASEINCOME.Value5:
                                if (enabled) { Amt5Enable = true; dataGridCaseIncome.Columns["Amt5"].ReadOnly = false; if (required) { dataGridCaseIncome.Columns["Amt5"].HeaderText = "* Amt5"; Amt5Req = true; } } else { dataGridCaseIncome.Columns["Amt5"].ReadOnly = true; Amt5Req = false; }
                                break;
                            case Consts.CASEINCOME.Date1:
                                if (enabled) { Date1Enable = true; dataGridCaseIncome.Columns["Date1"].ReadOnly = false; if (required) { dataGridCaseIncome.Columns["Date1"].HeaderText = "* Date1"; Date1Req = true; } } else { dataGridCaseIncome.Columns["Date1"].ReadOnly = true; Date1Req = false; }
                                break;
                            case Consts.CASEINCOME.Date2:
                                if (enabled) { Date2Enable = true; dataGridCaseIncome.Columns["Date2"].ReadOnly = false; if (required) { dataGridCaseIncome.Columns["Date2"].HeaderText = "* Date2"; Date2Req = true; } } else { dataGridCaseIncome.Columns["Date2"].ReadOnly = true; Date2Req = false; }
                                break;
                            case Consts.CASEINCOME.Date3:
                                if (enabled) { Date3Enable = true; dataGridCaseIncome.Columns["Date3"].ReadOnly = false; if (required) { dataGridCaseIncome.Columns["Date3"].HeaderText = "* Date3"; Date3Req = true; } } else { dataGridCaseIncome.Columns["Date3"].ReadOnly = true; Date3Req = false; }
                                break;
                            case Consts.CASEINCOME.Date4:
                                if (enabled) { Date4Enable = true; dataGridCaseIncome.Columns["Date4"].ReadOnly = false; if (required) { dataGridCaseIncome.Columns["Date4"].HeaderText = "* Date4"; Date4Req = true; } } else { dataGridCaseIncome.Columns["Date4"].ReadOnly = true; Date4Req = false; }
                                break;
                            case Consts.CASEINCOME.Date5:
                                if (enabled) { Date5Enable = true; dataGridCaseIncome.Columns["Date5"].ReadOnly = false; if (required) { dataGridCaseIncome.Columns["Date5"].HeaderText = "* Date5"; Date5Req = true; } } else { dataGridCaseIncome.Columns["Date5"].ReadOnly = true; Date5Req = false; }
                                break;
                            //case Consts.CASEINCOME.Factor:
                            //    if (enabled) { txtFactor.Enabled = lblFactor.Enabled = true; if (required) lblFactorReq.Visible = true; } else { txtFactor.Enabled = lblFactor.Enabled = false; lblFactorReq.Visible = false; }
                            //    break;
                            //case Consts.CASEINCOME.Sub:
                            //    if (enabled) { txtSub.Enabled = lblSub.Enabled = true; if (required) lblSubReq.Visible = true; } else { txtSub.Enabled = lblSub.Enabled = false; lblSubReq.Visible = false; }
                            //    break;
                            //case Consts.CASEINCOME.Total:
                            //    if (enabled) { txtTotal.Enabled = lblTotal.Enabled = true; if (required) lblTotalReq.Visible = true; } else { txtTotal.Enabled = lblTotal.Enabled = false; lblTotalReq.Visible = false; }
                            //    break;
                            case Consts.CASEINCOME.Exclude:
                                if (enabled) { ExcludeEnable = true; dataGridCaseIncome.Columns["Exclude"].ReadOnly = false; if (required) { dataGridCaseIncome.Columns["Exclude"].HeaderText = "* Exclude"; ExcludeReq = true; } } else { dataGridCaseIncome.Columns["Exclude"].ReadOnly = true; ExcludeReq = false; }
                                break;
                        }
                    }
                }
            }
            else
            {
                IncomeTypeEnable = true;
                chkIncomeTypeEnable = true;
            }
        }

        private void RequiredControlsColorChange()
        {

            if (FLDCNTLHieEntity.Count > 0)
            {
                foreach (FldcntlHieEntity entity in FLDCNTLHieEntity)
                {
                    bool required = entity.Req.Equals("Y") ? true : false;
                    bool enabled = entity.Enab.Equals("Y") ? true : false;

                    switch (entity.FldDesc)
                    {

                        case Consts.CASEINCOME.IncomeType:
                            if (required) { dataGridCaseIncome.Columns["IncomeType"].DefaultCellStyle.BackColor = Color.AntiqueWhite; } else { dataGridCaseIncome.Columns["IncomeType"].DefaultCellStyle.BackColor = Color.White; }
                            break;
                        case Consts.CASEINCOME.Interval:
                            if (required) { dataGridCaseIncome.Columns["Interval"].DefaultCellStyle.BackColor = Color.AntiqueWhite; } else { dataGridCaseIncome.Columns["Interval"].DefaultCellStyle.BackColor = Color.White; }
                            break;
                        case Consts.CASEINCOME.Value1:
                            if (required) { dataGridCaseIncome.Columns["Amt1"].DefaultCellStyle.BackColor = Color.AntiqueWhite; } else { dataGridCaseIncome.Columns["Amt1"].DefaultCellStyle.BackColor = Color.White; }
                            break;
                        case Consts.CASEINCOME.Value2:
                            if (required) { dataGridCaseIncome.Columns["Amt2"].DefaultCellStyle.BackColor = Color.AntiqueWhite; } else { dataGridCaseIncome.Columns["Amt2"].DefaultCellStyle.BackColor = Color.White; }
                            break;
                        case Consts.CASEINCOME.Value3:
                            if (required) { dataGridCaseIncome.Columns["Amt3"].DefaultCellStyle.BackColor = Color.AntiqueWhite; } else { dataGridCaseIncome.Columns["Amt3"].DefaultCellStyle.BackColor = Color.White; }
                            break;
                        case Consts.CASEINCOME.Value4:
                            if (required) { dataGridCaseIncome.Columns["Amt4"].DefaultCellStyle.BackColor = Color.AntiqueWhite; } else { dataGridCaseIncome.Columns["Amt4"].DefaultCellStyle.BackColor = Color.White; }
                            break;
                        case Consts.CASEINCOME.Value5:
                            if (required) { dataGridCaseIncome.Columns["Amt5"].DefaultCellStyle.BackColor = Color.AntiqueWhite; } else { dataGridCaseIncome.Columns["Amt5"].DefaultCellStyle.BackColor = Color.White; }
                            break;
                        case Consts.CASEINCOME.Date1:
                            if (required) { dataGridCaseIncome.Columns["Date1"].DefaultCellStyle.BackColor = Color.AntiqueWhite; } else { dataGridCaseIncome.Columns["Date1"].DefaultCellStyle.BackColor = Color.White; }
                            break;
                        case Consts.CASEINCOME.Date2:
                            if (required) { dataGridCaseIncome.Columns["Date2"].DefaultCellStyle.BackColor = Color.AntiqueWhite; } else { dataGridCaseIncome.Columns["Date2"].DefaultCellStyle.BackColor = Color.White; }
                            break;
                        case Consts.CASEINCOME.Date3:
                            if (required) { dataGridCaseIncome.Columns["Date3"].DefaultCellStyle.BackColor = Color.AntiqueWhite; } else { dataGridCaseIncome.Columns["Date3"].DefaultCellStyle.BackColor = Color.White; }
                            break;
                        case Consts.CASEINCOME.Date4:
                            if (required) { dataGridCaseIncome.Columns["Date4"].DefaultCellStyle.BackColor = Color.AntiqueWhite; } else { dataGridCaseIncome.Columns["Date4"].DefaultCellStyle.BackColor = Color.White; }
                            break;
                        case Consts.CASEINCOME.Date5:
                            if (required) { dataGridCaseIncome.Columns["Date5"].DefaultCellStyle.BackColor = Color.AntiqueWhite; } else { dataGridCaseIncome.Columns["Date5"].DefaultCellStyle.BackColor = Color.White; }
                            break;

                        //case Consts.CASEINCOME.Factor:
                        //    if (enabled) { txtFactor.Enabled = lblFactor.Enabled = true; if (required) lblFactorReq.Visible = true; } else { txtFactor.Enabled = lblFactor.Enabled = false; lblFactorReq.Visible = false; }
                        //    break;
                        //case Consts.CASEINCOME.Sub:
                        //    if (enabled) { txtSub.Enabled = lblSub.Enabled = true; if (required) lblSubReq.Visible = true; } else { txtSub.Enabled = lblSub.Enabled = false; lblSubReq.Visible = false; }
                        //    break;
                        //case Consts.CASEINCOME.Total:
                        //    if (enabled) { txtTotal.Enabled = lblTotal.Enabled = true; if (required) lblTotalReq.Visible = true; } else { txtTotal.Enabled = lblTotal.Enabled = false; lblTotalReq.Visible = false; }
                        //    break;
                        case Consts.CASEINCOME.Exclude:
                            if (enabled) { dataGridCaseIncome.Columns["Exclude"].ReadOnly = false; if (required) { dataGridCaseIncome.Columns["Exclude"].DefaultCellStyle.BackColor = Color.AntiqueWhite; ExcludeReq = true; } } else { dataGridCaseIncome.Columns["Exclude"].ReadOnly = false; ExcludeReq = false; }
                            break;
                    }
                }
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;


            if (lblVerifierReq.Visible && ((ListItem)cmbVerifier.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbVerifier, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblVerifier.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbVerifier, null);
            }



            if (lblReqHowverified.Visible && String.IsNullOrEmpty(txtHowVerified.Text.Trim()))
            {
                _errorProvider.SetError(txtHowVerified, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHowVerified.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtHowVerified, null);
            }


            if (lblIncomeSourceReq.Visible && String.IsNullOrEmpty(txtIncSource.Text.Trim()))
            {
                _errorProvider.SetError(txtIncSource, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblIncomeSource.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtIncSource, null);
            }

            foreach (DataGridViewRow item in dataGridCaseIncome.Rows)
            {
                string strInterval = item.Cells["Interval"].Value == null ? string.Empty : item.Cells["Interval"].Value.ToString();
                if (!(string.IsNullOrEmpty(item.Cells["Factor"].Value.ToString())))
                {

                    if (lblReqHowverified.Visible && String.IsNullOrEmpty(Convert.ToString(item.Cells["gvtHowverfier"].Value)))
                    {
                        CommonFunctions.MessageBoxDisplay("This income type " + item.Cells["IncomeType"].Value.ToString() + " How Verfier value required");
                        isValid = false;
                        break;
                    }

                    if (lblIncomeSourceReq.Visible && String.IsNullOrEmpty(Convert.ToString(item.Cells["gvtIncomesource"].Value)))
                    {
                        CommonFunctions.MessageBoxDisplay("This income type " + item.Cells["IncomeType"].Value.ToString() + " Income Source value required");
                        isValid = false;
                        break;
                    }
                }
            }
            if (propAgencyControlDetails.IncVerfication.ToUpper() == "Y")
            {
                if (calVerificationDate.Checked == false)
                {
                    _errorProvider.SetError(calVerificationDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Verified date is required"));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(calVerificationDate, null);
                }
            }
            //if (txtAmount1.Text.Length > 6)
            //{
            //    if (!System.Text.RegularExpressions.Regex.IsMatch(txtAmount1.Text, Consts.StaticVars.TwoDecimalRange6String))
            //    {
            //        _errorProvider.SetError(txtAmount1, Consts.Messages.PleaseEnterDecimals6Range);
            //        isValid = false;
            //    }
            //    else
            //    {
            //        _errorProvider.SetError(txtAmount1, null);
            //    }
            //}
            //if (txtAmount2.Text.Length > 6)
            //{
            //    if (!System.Text.RegularExpressions.Regex.IsMatch(txtAmount2.Text, Consts.StaticVars.TwoDecimalRange6String))
            //    {
            //        _errorProvider.SetError(txtAmount2, Consts.Messages.PleaseEnterDecimals6Range);
            //        isValid = false;
            //    }
            //    else
            //    {
            //        _errorProvider.SetError(txtAmount2, null);
            //    }
            //}
            //if (txtAmount3.Text.Length > 6)
            //{
            //    if (!System.Text.RegularExpressions.Regex.IsMatch(txtAmount3.Text, Consts.StaticVars.TwoDecimalRange6String))
            //    {
            //        _errorProvider.SetError(txtAmount3, Consts.Messages.PleaseEnterDecimals6Range);
            //        isValid = false;
            //    }
            //    else
            //    {
            //        _errorProvider.SetError(txtAmount3, null);
            //    }
            //}
            //if (txtAmount4.Text.Length > 6)
            //{
            //    if (!System.Text.RegularExpressions.Regex.IsMatch(txtAmount4.Text, Consts.StaticVars.TwoDecimalRange6String))
            //    {
            //        _errorProvider.SetError(txtAmount4, Consts.Messages.PleaseEnterDecimals6Range);
            //        isValid = false;
            //    }
            //    else
            //    {
            //        _errorProvider.SetError(txtAmount4, null);
            //    }
            //}
            //if (txtAmount5.Text.Length > 6)
            //{
            //    if (!System.Text.RegularExpressions.Regex.IsMatch(txtAmount5.Text, Consts.StaticVars.TwoDecimalRange6String))
            //    {
            //        _errorProvider.SetError(txtAmount5, Consts.Messages.PleaseEnterDecimals6Range);
            //        isValid = false;
            //    }
            //    else
            //    {
            //        _errorProvider.SetError(txtAmount5, null);
            //    }
            //}

            return (isValid);
        }

        private bool ValidationGrid()
        {
            bool boolRequired = true;
            foreach (DataGridViewRow item in dataGridCaseIncome.Rows)
            {
                string strInterval = item.Cells["Interval"].Value == null ? string.Empty : item.Cells["Interval"].Value.ToString();
                if (!(string.IsNullOrEmpty(item.Cells["Factor"].Value.ToString())))
                {
                    if (strInterval == "0" || strInterval == "O" || strInterval == "A" || strInterval == "Q" || strInterval == "E") // added "E" logic 04/02/2019
                    {
                        if (Amt1Req == true && Convert.ToString(item.Cells["Amt1"].Value) == string.Empty)
                        {
                            // dataGridCaseIncome.Rows[item.Index].Selected = true;
                            boolRequired = false;
                            item.Cells["Amt1"].Style.BackColor = System.Drawing.Color.Red;
                            //break;
                        }
                        else
                            item.Cells["Amt1"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                        if (Date1Req == true && (item.Cells["Date1"].Value.ToString().Trim() == string.Empty || item.Cells["Date1"].Value.ToString().Trim() == "/  /"))
                        {
                            //  dataGridCaseIncome.Rows[item.Index].Selected = true;
                            item.Cells["Date1"].Style.BackColor = System.Drawing.Color.Red;
                            boolRequired = false;
                            //break;
                        }
                        else
                            item.Cells["Date1"].Style.BackColor = System.Drawing.Color.AntiqueWhite;

                    }
                    else if (strInterval == "B" || strInterval == "S" || strInterval == "M" || strInterval == "N")
                    {
                        if (Amt1Req == true && Convert.ToString(item.Cells["Amt1"].Value) == string.Empty)
                        {
                            //  dataGridCaseIncome.Rows[item.Index].Selected = true;
                            item.Cells["Amt1"].Style.BackColor = System.Drawing.Color.Red;
                            boolRequired = false;
                            //   break;

                        }
                        else
                            item.Cells["Amt1"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                        if (Date1Req == true && (item.Cells["Date1"].Value.ToString().Trim() == string.Empty || item.Cells["Date1"].Value.ToString().Trim() == "/  /"))
                        {
                            //  dataGridCaseIncome.Rows[item.Index].Selected = true;
                            item.Cells["Date1"].Style.BackColor = System.Drawing.Color.Red;
                            boolRequired = false;
                            //  break;
                        }
                        else
                            item.Cells["Date1"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                        if (Amt2Req == true && Convert.ToString(item.Cells["Amt2"].Value) == string.Empty)
                        {
                            // dataGridCaseIncome.Rows[item.Index].Selected = true;
                            item.Cells["Amt2"].Style.BackColor = System.Drawing.Color.Red;
                            boolRequired = false;
                            //break;
                        }
                        else
                            item.Cells["Amt2"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                        if (Date2Req == true && (item.Cells["Date2"].Value.ToString().Trim() == string.Empty || item.Cells["Date2"].Value.ToString().Trim() == "/  /"))
                        {
                            //  dataGridCaseIncome.Rows[item.Index].Selected = true;
                            item.Cells["Date2"].Style.BackColor = System.Drawing.Color.Red;
                            boolRequired = false;
                            //break;
                        }
                        else
                            item.Cells["Date2"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                    }
                    // Modified this logic none base 04/02/2019 else if (strInterval == "E" || strInterval == "W" || strInterval == "3" || strInterval == "6" || strInterval == "9" || strInterval == "H")
                    else if (strInterval == "W" || strInterval == "3" || strInterval == "6" || strInterval == "9" || strInterval == "H")
                    {
                        if (Amt1Req == true && item.Cells["Amt1"].Value.ToString().Trim() == string.Empty)
                        {
                            // dataGridCaseIncome.Rows[item.Index].Selected = true;
                            item.Cells["Amt1"].Style.BackColor = System.Drawing.Color.Red;
                            boolRequired = false;
                            // break;
                        }
                        else
                            item.Cells["Amt1"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                        if (Date1Req == true && (Convert.ToString(item.Cells["Date1"].Value) == string.Empty || item.Cells["Date1"].Value.ToString().Trim() == "/  /"))
                        {
                            //  dataGridCaseIncome.Rows[item.Index].Selected = true;
                            item.Cells["Date1"].Style.BackColor = System.Drawing.Color.Red;
                            boolRequired = false;
                            //  break;
                        }
                        else
                            item.Cells["Date1"].Style.BackColor = System.Drawing.Color.AntiqueWhite;

                        if (Amt2Req == true && Convert.ToString(item.Cells["Amt2"].Value) == string.Empty)
                        {
                            //  dataGridCaseIncome.Rows[item.Index].Selected = true;
                            item.Cells["Amt2"].Style.BackColor = System.Drawing.Color.Red;
                            boolRequired = false;
                            // break;
                        }
                        else
                            item.Cells["Amt2"].Style.BackColor = System.Drawing.Color.AntiqueWhite;


                        if (Date2Req == true && (item.Cells["Date2"].Value.ToString().Trim() == string.Empty || item.Cells["Date2"].Value.ToString().Trim() == "/  /"))
                        {
                            //  dataGridCaseIncome.Rows[item.Index].Selected = true;
                            item.Cells["Date2"].Style.BackColor = System.Drawing.Color.Red;
                            boolRequired = false;
                            // break;
                        }
                        else
                            item.Cells["Date2"].Style.BackColor = System.Drawing.Color.AntiqueWhite;

                        if (Amt3Req == true && Convert.ToString(item.Cells["Amt3"].Value) == string.Empty)
                        {
                            //  dataGridCaseIncome.Rows[item.Index].Selected = true;
                            item.Cells["Amt3"].Style.BackColor = System.Drawing.Color.Red;
                            boolRequired = false;
                            //break;
                        }
                        else
                            item.Cells["Amt3"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                        if (Date3Req == true && (item.Cells["Date3"].Value.ToString().Trim() == string.Empty || item.Cells["Date3"].Value.ToString().Trim() == "/  /"))
                        {
                            // dataGridCaseIncome.Rows[item.Index].Selected = true;
                            item.Cells["Date3"].Style.BackColor = System.Drawing.Color.Red;
                            boolRequired = false;
                            // break;
                        }
                        else
                            item.Cells["Date3"].Style.BackColor = System.Drawing.Color.AntiqueWhite;

                        if (Amt4Req == true && Convert.ToString(item.Cells["Amt4"].Value) == string.Empty)
                        {
                            //  dataGridCaseIncome.Rows[item.Index].Selected = true;
                            item.Cells["Amt4"].Style.BackColor = System.Drawing.Color.Red;
                            boolRequired = false;
                            // break;
                        }
                        else
                            item.Cells["Amt4"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                        if (Date4Req == true && (item.Cells["Date4"].Value.ToString().Trim() == string.Empty || item.Cells["Date4"].Value.ToString().Trim() == "/  /"))
                        {
                            // dataGridCaseIncome.Rows[item.Index].Selected = true;
                            item.Cells["Date4"].Style.BackColor = System.Drawing.Color.Red;
                            boolRequired = false;
                            // break;
                        }
                        else
                            item.Cells["Date4"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                        if (Amt5Req == true && Convert.ToString(item.Cells["Amt5"].Value) == string.Empty)
                        {
                            // dataGridCaseIncome.Rows[item.Index].Selected = true;
                            item.Cells["Amt5"].Style.BackColor = System.Drawing.Color.Red;
                            boolRequired = false;
                            //break;
                        }
                        else
                            item.Cells["Amt5"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                        if (Date5Req == true && (item.Cells["Date5"].Value.ToString().Trim() == string.Empty || item.Cells["Date5"].Value.ToString().Trim() == "/  /"))
                        {
                            // dataGridCaseIncome.Rows[item.Index].Selected = true;
                            item.Cells["Date5"].Style.BackColor = System.Drawing.Color.Red;
                            boolRequired = false;
                            //break;
                        }
                        else
                            item.Cells["Date5"].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                    }

                    if (item.Cells["Amt1"].Value.ToString().Trim() != string.Empty && item.Cells["Amt1"].Value.ToString().Trim() != "0.00" && (Convert.ToString(item.Cells["Date1"].Value) == string.Empty || item.Cells["Date1"].Value.ToString().Trim() == "/  /") && propHourlyMode == "Y")
                    {
                        item.Cells["Date1"].Style.BackColor = System.Drawing.Color.Red;
                        boolRequired = false;
                    }
                    if (item.Cells["Amt2"].Value.ToString().Trim() != string.Empty && item.Cells["Amt2"].Value.ToString().Trim() != "0.00" && (Convert.ToString(item.Cells["Date2"].Value) == string.Empty || item.Cells["Date2"].Value.ToString().Trim() == "/  /") && propHourlyMode == "Y")
                    {
                        item.Cells["Date2"].Style.BackColor = System.Drawing.Color.Red;
                        boolRequired = false;
                    }
                    if (item.Cells["Amt3"].Value.ToString().Trim() != string.Empty && item.Cells["Amt3"].Value.ToString().Trim() != "0.00" && (Convert.ToString(item.Cells["Date3"].Value) == string.Empty || item.Cells["Date3"].Value.ToString().Trim() == "/  /") && propHourlyMode == "Y")
                    {
                        item.Cells["Date3"].Style.BackColor = System.Drawing.Color.Red;
                        boolRequired = false;
                    }
                    if (item.Cells["Amt4"].Value.ToString().Trim() != string.Empty && item.Cells["Amt4"].Value.ToString().Trim() != "0.00" && (Convert.ToString(item.Cells["Date4"].Value) == string.Empty || item.Cells["Date4"].Value.ToString().Trim() == "/  /") && propHourlyMode == "Y")
                    {
                        item.Cells["Date4"].Style.BackColor = System.Drawing.Color.Red;
                        boolRequired = false;
                    }
                    if (item.Cells["Amt5"].Value.ToString().Trim() != string.Empty && item.Cells["Amt5"].Value.ToString().Trim() != "0.00" && (Convert.ToString(item.Cells["Date5"].Value) == string.Empty || item.Cells["Date5"].Value.ToString().Trim() == "/  /") && propHourlyMode == "Y")
                    {
                        item.Cells["Date5"].Style.BackColor = System.Drawing.Color.Red;
                        boolRequired = false;
                    }



                    if (strInterval == "3")
                    {
                        boolRequired = false;
                        CommonFunctions.MessageBoxDisplay("Please change 30 days interval data.");
                        break;
                    }
                }
            }
            //dataGridCaseIncome.RefreshEdit();
            dataGridCaseIncome.Update();
            return boolRequired;

        }

        private bool ValidationIntervalGrid(out string strIncomeTypesMsg)
        {
            bool boolInterval = true;
            string strIncomeTypes = string.Empty;
            foreach (DataGridViewRow item in dataGridCaseIncome.Rows)
            {
                string strInterval = item.Cells["Interval"].Value == null ? string.Empty : item.Cells["Interval"].Value.ToString();
                if (!(string.IsNullOrEmpty(item.Cells["Factor"].Value.ToString())))
                {
                    if (strInterval == "3" || strInterval == "6" || strInterval == "9")
                    {
                        if (item.Cells["Amt1"].Value != string.Empty)
                        {
                            if (item.Cells["Date1"].Value != string.Empty && item.Cells["Date1"].Value.ToString().Trim() != "/  /")
                            {
                                if (!CheckMstIntakeDate(Convert.ToDateTime(item.Cells["Date1"].Value).Date, strInterval))
                                {
                                    item.Cells["Date1"].Style.ForeColor = System.Drawing.Color.Red;
                                    boolInterval = false;
                                    strIncomeTypes = strIncomeTypes + "   " + item.Cells["IncomeType"].ToString();
                                }
                                else
                                    item.Cells["Date1"].Style.ForeColor = System.Drawing.Color.Black;

                            }
                        }
                        if (item.Cells["Amt2"].Value != string.Empty)
                        {
                            if (item.Cells["Date2"].Value != string.Empty && item.Cells["Date2"].Value.ToString().Trim() != "/  /")
                            {
                                if (!CheckMstIntakeDate(Convert.ToDateTime(item.Cells["Date2"].Value).Date, strInterval))
                                {
                                    item.Cells["Date2"].Style.ForeColor = System.Drawing.Color.Red;
                                    boolInterval = false;
                                    strIncomeTypes = strIncomeTypes + "   " + item.Cells["IncomeType"].ToString();
                                }
                                else
                                    item.Cells["Date2"].Style.ForeColor = System.Drawing.Color.Black;
                            }
                        }
                        if (item.Cells["Amt3"].Value != string.Empty)
                        {
                            if (item.Cells["Date3"].Value != string.Empty && item.Cells["Date3"].Value.ToString().Trim() != "/  /")
                            {
                                if (!CheckMstIntakeDate(Convert.ToDateTime(item.Cells["Date3"].Value).Date, strInterval))
                                {
                                    item.Cells["Date3"].Style.ForeColor = System.Drawing.Color.Red;
                                    boolInterval = false;
                                    strIncomeTypes = strIncomeTypes + "   " + item.Cells["IncomeType"].ToString();
                                }
                                else
                                    item.Cells["Date3"].Style.ForeColor = System.Drawing.Color.Black;
                            }
                        }
                        if (item.Cells["Amt4"].Value != string.Empty)
                        {
                            if (item.Cells["Date4"].Value != string.Empty && item.Cells["Date4"].Value.ToString().Trim() != "/  /")
                            {
                                if (!CheckMstIntakeDate(Convert.ToDateTime(item.Cells["Date4"].Value).Date, strInterval))
                                {
                                    item.Cells["Date4"].Style.ForeColor = System.Drawing.Color.Red;
                                    boolInterval = false;
                                    strIncomeTypes = strIncomeTypes + "   " + item.Cells["IncomeType"].ToString();
                                }
                                else
                                    item.Cells["Date4"].Style.ForeColor = System.Drawing.Color.Black;
                            }
                        }
                        if (item.Cells["Amt5"].Value != string.Empty)
                        {
                            if (item.Cells["Date5"].Value != string.Empty && item.Cells["Date5"].Value.ToString().Trim() != "/  /")
                            {
                                if (!CheckMstIntakeDate(Convert.ToDateTime(item.Cells["Date5"].Value).Date, strInterval))
                                {
                                    item.Cells["Date5"].Style.ForeColor = System.Drawing.Color.Red;
                                    boolInterval = false;
                                    strIncomeTypes = strIncomeTypes + "   " + item.Cells["IncomeType"].ToString();
                                }
                                else
                                    item.Cells["Date5"].Style.ForeColor = System.Drawing.Color.Black;

                            }
                        }
                        // dataGridCaseIncome.Rows[rowcalindex].Cells[intcolumnindex].Style.ForeColor = System.Drawing.Color.Black;
                        // if (!CheckMstIntakeDate(Convert.ToDateTime(strCurrectValue).Date, strIntervalValue))
                        // dataGridCaseIncome.Rows[rowcalindex].Cells[intcolumnindex].Style.ForeColor = System.Drawing.Color.Red;
                    }

                }
            }
            strIncomeTypesMsg = strIncomeTypes;
            return boolInterval;

        }


        private void SelectIntervalcolorchange()
        {
            foreach (DataGridViewRow item in dataGridCaseIncome.Rows)
            {
                disableenableFields(item.Cells["Interval"].ToString().Trim(), item.Index);
            }
        }

        private void EnableDisableGridcontrol()
        {
            if (chkIncomeTypeEnable)
                dataGridCaseIncome.Columns["gvchkType"].ReadOnly = false;
            else
                dataGridCaseIncome.Columns["gvchkType"].ReadOnly = true;

            if (IncomeTypeEnable)
                dataGridCaseIncome.Columns["IncomeType"].ReadOnly = true;
            else
                dataGridCaseIncome.Columns["IncomeType"].ReadOnly = true;

            if (IntervalEnable)
                dataGridCaseIncome.Columns["Interval"].ReadOnly = false;
            else
                dataGridCaseIncome.Columns["Interval"].ReadOnly = true;

            if (Amt1Enable)
                dataGridCaseIncome.Columns["Amt1"].ReadOnly = false;
            else
                dataGridCaseIncome.Columns["Amt1"].ReadOnly = true;

            if (Amt2Enable)
                dataGridCaseIncome.Columns["Amt2"].ReadOnly = false;
            else
                dataGridCaseIncome.Columns["Amt2"].ReadOnly = true;

            if (Amt3Enable)
                dataGridCaseIncome.Columns["Amt3"].ReadOnly = false;
            else
                dataGridCaseIncome.Columns["Amt3"].ReadOnly = true;

            if (Amt4Enable)
                dataGridCaseIncome.Columns["Amt4"].ReadOnly = false;
            else
                dataGridCaseIncome.Columns["Amt4"].ReadOnly = true;

            if (Amt5Enable)
                dataGridCaseIncome.Columns["Amt5"].ReadOnly = false;
            else
                dataGridCaseIncome.Columns["Amt5"].ReadOnly = true;

            if (Date1Enable)
                dataGridCaseIncome.Columns["Date1"].ReadOnly = false;
            else
                dataGridCaseIncome.Columns["Date1"].ReadOnly = true;

            if (Date2Enable)
                dataGridCaseIncome.Columns["Date2"].ReadOnly = false;
            else
                dataGridCaseIncome.Columns["Date2"].ReadOnly = true;

            if (Date3Enable)
                dataGridCaseIncome.Columns["Date3"].ReadOnly = false;
            else
                dataGridCaseIncome.Columns["Date3"].ReadOnly = true;

            if (Date4Enable)
                dataGridCaseIncome.Columns["Date4"].ReadOnly = false;
            else
                dataGridCaseIncome.Columns["Date4"].ReadOnly = true;

            if (Date5Enable)
                dataGridCaseIncome.Columns["Date5"].ReadOnly = false;
            else
                dataGridCaseIncome.Columns["Date5"].ReadOnly = true;

            if (ExcludeEnable)
                dataGridCaseIncome.Columns["Exclude"].ReadOnly = false;
            else
                dataGridCaseIncome.Columns["Exclude"].ReadOnly = true;
        }

        private void RequireAllGridColors()
        {
            foreach (DataGridViewRow item in dataGridCaseIncome.Rows)
            {
                if (item.Cells["Interval"].Value != string.Empty)
                {
                    RequiredfieldColorchanges(item.Cells["Interval"].Value.ToString().Trim(), item.Index, item);
                }
            }

        }

        private bool CheckMstIntakeDate(DateTime datPresentDate, string strInterval)
        {
            bool booldate = true;
            DateTime startDate;
            if (propHourlyMode.ToUpper() == "Y")
            {
                strInterval = "-29";
                startDate = MstIntakeEndDate.AddDays(Convert.ToInt32(strInterval));
                if (!((startDate <= datPresentDate) && (datPresentDate <= MstIntakeEndDate)))
                {
                    //MessageBox.Show("Please enter window date and Intake date between only");
                    booldate = false;
                }
            }
            else
            {
                if (strInterval == "3" || strInterval == "6" || strInterval == "9")
                {
                    strInterval = "-" + strInterval + "0";
                    startDate = MstIntakeEndDate.AddDays(Convert.ToInt32(strInterval));
                    if (!((startDate <= datPresentDate) && (datPresentDate <= MstIntakeEndDate)))
                    {
                        //MessageBox.Show("Please enter window date and Intake date between only");
                        booldate = false;
                    }
                }
            }
            return booldate;
        }

        private void dataGridCaseIncome_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridCaseIncome.Rows.Count > 0)
            {
                if (dataGridCaseIncome.SelectedCells.Count > 0)
                {
                    if (dataGridCaseIncome.SelectedCells[0].ColumnIndex == IncomeType.Index)
                    {
                        string strInterval = Convert.ToString(dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["Interval"].Value);
                        if (propHourlyMode == "Y")
                        {
                            ShowDateLabelDisplay(-29);
                        }
                        else
                        {
                            if (strInterval == "3")
                                ShowDateLabelDisplay(-29);
                            else if (strInterval == "6")
                                ShowDateLabelDisplay(-59);
                            else if (strInterval == "9")
                                ShowDateLabelDisplay(-89);
                            else
                                lblMstIntakeDate.Text = string.Empty;
                        }
                    }


                    string strIntervalValue = Convert.ToString(dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["Interval"].Value);

                    HourlyEnableControl(strIntervalValue);

                    if (strIntervalValue == "H")
                    {
                        txtHr1.Text = dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["gvtHrRate1"].Value.ToString();
                        txtHr2.Text = dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["gvtHrRate2"].Value.ToString();
                        txtHr3.Text = dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["gvtHrRate3"].Value.ToString();
                        txtHr4.Text = dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["gvtHrRate4"].Value.ToString();
                        txtHr5.Text = dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["gvtHrRate5"].Value.ToString();
                    }
                    else
                    {
                        txtHr1.Text = string.Empty;
                        txtHr2.Text = string.Empty;
                        txtHr3.Text = string.Empty;
                        txtHr4.Text = string.Empty;
                        txtHr5.Text = string.Empty;
                    }

                    txtIncSource.Text = dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["gvtIncomesource"].Value.ToString();
                    txtHowVerified.Text = dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["gvtHowverfier"].Value.ToString();
                    if (Convert.ToString(dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["gvtVerifier"].Value) != string.Empty && Convert.ToString(dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["gvtVerifier"].Value) != "0")
                        CommonFunctions.SetComboBoxValue(cmbVerifier, dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["gvtVerifier"].Value.ToString());
                    else
                        CommonFunctions.SetComboBoxValue(cmbVerifier, strCaseWorkerDefaultCode);

                    if (Convert.ToString(dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["Column1"].Value) != string.Empty && Convert.ToString(dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["Column1"].Value) != "0")
                        CommonFunctions.SetComboBoxValue(cmbVerfMethod, dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["Column1"].Value.ToString());
                    //else
                    //    CommonFunctions.SetComboBoxValue(cmbVerfMethod, strCaseWorkerDefaultCode);
                    //gvtVerMethod
                }

            }
        }

        private void EnableOnlyMaincontrol()
        {


            if (FLDCNTLHieEntity.Count > 0)
            {
                foreach (FldcntlHieEntity entity in FLDCNTLHieEntity)
                {
                    bool required = entity.Req.Equals("Y") ? true : false;
                    bool enabled = entity.Enab.Equals("Y") ? true : false;

                    switch (entity.FldDesc)
                    {
                        case Consts.CASEINCOME.Verifier:
                            if (enabled) { cmbVerifier.Enabled = lblVerifier.Enabled = true; if (required) lblVerifierReq.Visible = VerifierReq = true; } else { cmbVerifier.Enabled = lblVerifier.Enabled = false; lblVerifierReq.Visible = VerifierReq = false; }
                            break;
                        case Consts.CASEINCOME.HowVerified:
                            if (enabled) { txtHowVerified.Enabled = lblHowVerified.Enabled = true; if (required) lblReqHowverified.Visible = true; } else { txtHowVerified.Enabled = lblHowVerified.Enabled = false; lblReqHowverified.Visible = false; }
                            break;
                        case Consts.CASEINCOME.IncomeSource:
                            if (enabled) { txtIncSource.Enabled = lblIncomeSource.Enabled = true; if (required) lblIncomeSourceReq.Visible = true; } else { txtIncSource.Enabled = lblIncomeSource.Enabled = false; lblIncomeSourceReq.Visible = false; }
                            break;
                    }

                    //if(programDefinitionList.State !="TX")
                    //{
                    //lblHowVerified.Enabled = txtHowVerified.Enabled = true;
                    //}
                }
                cmbVerfMethod.Enabled = true;
            }
            if (propIncomeTypeSwitch == "Y")
            {
                lblVerifierReq.Visible = false;
                lblHowVerified.Enabled = txtHowVerified.Enabled = false;
                cmbVerifier.Enabled = lblVerifier.Enabled = false;
                txtIncSource.Enabled = lblIncomeSource.Enabled = false;
                txtHowVerified.Enabled = lblHowVerified.Enabled = false;
                lblIncomeSourceReq.Visible = lblReqHowverified.Visible = false;
            }
            if (propAgencyControlDetails.IncVerfication.ToUpper() == "Y")
            {
                if (propAgencyControlDetails.VerSwitch.ToUpper() == "Y")
                {
                    calVerificationDate.Enabled = true;
                }
                else
                {
                    calVerificationDate.Enabled = false;
                }
            }
            else
            {
                calVerificationDate.Enabled = false;
            }
        }

        IncomeReportForm Pdf_Form;
        private void PbPdf_Click(object sender, EventArgs e)
        {
            string Temp_Year = "    ";
            if (!string.IsNullOrEmpty(BaseForm.BaseYear))
                Temp_Year = BaseForm.BaseYear;
            Pdf_Form = new IncomeReportForm(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + Temp_Year + BaseForm.BaseApplicationNo, BaseForm, txtHowVerified.Text.Trim());
            //Pdf_Form.FormClosed += new FormClosedEventHandler(OnSerachFormClosed);
            //Pdf_Form.ShowDialog();
            //Pdf_Form.Close();
            if (Pdf_Form.DialogResult == DialogResult.OK)
            {
                if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
                {
                    PdfViewerNewForm objfrm = new PdfViewerNewForm(Pdf_Form.Get_Pdf_Path());
                    objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                    objfrm.StartPosition = FormStartPosition.CenterScreen;
                    objfrm.ShowDialog();
                }
                else
                {
                    FrmViewer objfrm = new FrmViewer(Pdf_Form.Get_Pdf_Path());
                    objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                    objfrm.StartPosition = FormStartPosition.CenterScreen;
                    objfrm.ShowDialog();
                }
            }
        }
        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(Pdf_Form.Get_Pdf_Path());
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Application.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "CASE2001_Income");

            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(),"",""), target: "_blank");
        }

        private void txtHowVerified_Leave(object sender, EventArgs e)
        {
            if (dataGridCaseIncome.Rows.Count > 0)
            {
                if (dataGridCaseIncome.SelectedCells.Count > 0)
                {

                    dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["gvtHowverfier"].Value = txtHowVerified.Text;
                }
            }
        }

        private void txtIncSource_Leave(object sender, EventArgs e)
        {
            if (dataGridCaseIncome.Rows.Count > 0)
            {
                if (dataGridCaseIncome.SelectedCells.Count > 0)
                {
                    dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["gvtIncomesource"].Value = txtIncSource.Text;
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
                    string strDate = string.Empty;
                    if (propAgencyControlDetails.VerSwitch.ToUpper() == "Y")
                    {
                        strDate = calVerificationDate.Value.ToShortDateString();
                    }
                    else
                    {
                        strDate = DateTime.Now.Date.ToShortDateString();
                    }

                    List<MasterPovertyEntity> masterPovertyDetails = _model.masterPovertyData.GetFedralOmbChart(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, strType, strDate);
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
                        FederalOmbChart fedOmbChart = new FederalOmbChart(BaseForm, Privileges, masterPovertyDetails, strType, strHeader);
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


        private bool ShowFedDetailsValid(string strType)
        {
            bool boolFedData = true;
            string strDate = string.Empty;

            if (propAgencyControlDetails.VerSwitch.ToUpper() == "Y")
            {
                strDate = calVerificationDate.Value.ToShortDateString();
            }
            else
            {
                strDate = DateTime.Now.Date.ToShortDateString();
            }

            List<MasterPovertyEntity> masterPovertyDetails = _model.masterPovertyData.GetFedralOmbChart(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, strType, strDate); //DateTime.Now.Date.ToShortDateString());
            if (strType == "CMI" || strType == "HUD")
            {
                string county = string.Empty;
                if (CaseMSTEntity != null)
                {
                    county = CaseMSTEntity.County;
                }
                masterPovertyDetails = masterPovertyDetails.FindAll(u => u.GdlCounty.Equals(county)).ToList();
            }

            if (masterPovertyDetails.Count == 0)
            {
                CommonFunctions.MessageBoxDisplay("No " + strType + " template found for verified date");//current date");
                boolFedData = false;
            }

            return boolFedData;

        }

        private void btn3Smi_Click(object sender, EventArgs e)
        {
            ShowFedDetails("SMI", "State Median Income Chart");
        }

        private void btn4Hud_Click(object sender, EventArgs e)
        {
            ShowFedDetails("HUD", "Hud Chart");
        }

        private void calc_FEDOMB()
        {
            double povertyPercent = 0;
            string type = "FED";
            if (programDefinitionList == null)
            {
                txtFedOmb.Text = povertyPercent.ToString();
                return;
            }
            if (programDefinitionList.DepFEDUsed.Equals("Y"))
            {
                double povertyBase = 0;
                double povertyFactory = 0;
                double povertyA = 0;
                double povertyB = 0;

                string strDate = string.Empty;
                if (propAgencyControlDetails.VerSwitch.ToUpper() == "Y")
                {
                    strDate = calVerificationDate.Value.ToShortDateString();
                }
                else
                {
                    strDate = DateTime.Now.Date.ToShortDateString();
                }

                List<MasterPovertyEntity> masterPovertyDetails = _model.masterPovertyData.GetFedralOmbChart(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, type, strDate);//DateTime.Now.ToShortDateString()
                if (masterPovertyDetails.Count > 0)
                {
                    povertyBase = double.Parse(masterPovertyDetails[0].Gdl1Value.ToString()) - double.Parse(masterPovertyDetails[0].Gdl2Value.ToString());
                    povertyFactory = double.Parse(masterPovertyDetails[0].Gdl2Value);
                    double noOfHouseHolds = CaseMSTEntity.NoInProg.ToString().Equals(string.Empty) ? 0.0 : double.Parse(CaseMSTEntity.NoInProg);
                    PovertyException propIncrementdata = _model.masterPovertyData.GetPovertyException(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, masterPovertyDetails[0].GdlStartDate, masterPovertyDetails[0].GdlEndDate, strDate, "OMB");//DateTime.Now.ToShortDateString()
                    int inthouseholds = CaseMSTEntity.NoInProg.ToString().Equals(string.Empty) ? 0 : int.Parse(CaseMSTEntity.NoInProg);
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
                    povertyB = povertyBase + povertyA;
                    double povertyProgramIncome = CaseMSTEntity.ProgIncome.ToString().Equals(string.Empty) ? 0.0 : double.Parse(CaseMSTEntity.ProgIncome);
                    povertyPercent = (povertyProgramIncome / povertyB) * 100;
                    //povertyPercent = Math.Round(povertyPercent);
                    /// newly added start this logic 05/10/2016
                    string Str_povertyD = povertyPercent.ToString();

                    if (BaseForm.BaseAgencyControlDetails.ACR_POV_DONT_RNDUP=="N" && BaseForm.BaseAgencyControlDetails.ACR_POV_WITH_DEC=="N")
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
                        /// end

                        if (povertyPercent < 1)
                            povertyPercent = 1;
                        else if (povertyPercent > 1000)
                            povertyPercent = 999;
                    }
                    else if(BaseForm.BaseAgencyControlDetails.ACR_POV_DONT_RNDUP == "Y" && BaseForm.BaseAgencyControlDetails.ACR_POV_WITH_DEC == "N")
                    {
                        string[] parts;
                        if (Str_povertyD.Contains("."))
                        {
                            parts = (povertyPercent.ToString()).Split('.');
                            povertyPercent = double.Parse(parts[0]);
                        }
                    }

                    //Added by sudheer on 06/05/23 if the income is 0 then the Poverty must be 0
                    if (povertyProgramIncome == 0)
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
            if (programDefinitionList != null)
            {
                string county = string.Empty;
                if (CaseMSTEntity != null)
                {
                    county = CaseMSTEntity.County;

                }
                string strDate = string.Empty;
                if (propAgencyControlDetails.VerSwitch.ToUpper() == "Y")
                {
                    strDate = calVerificationDate.Value.ToShortDateString();
                }
                else
                {
                    strDate = DateTime.Now.Date.ToShortDateString();
                }
                List<MasterPovertyEntity> masterPovertyDetails = _model.masterPovertyData.GetFedralOmbChart(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, type, strDate);//DateTime.Now.ToShortDateString()
                if (!type.Equals("SMI"))
                {
                    masterPovertyDetails = masterPovertyDetails.FindAll(u => u.GdlCounty.Equals(county)).ToList();
                }
                if (masterPovertyDetails.Count > 0)
                {
                    bool GDLSW = false;
                    masterpovertycount = masterPovertyDetails.Count;
                    MasterPovertyEntity masterPovertyDetail = masterPovertyDetails[0];
                    double povertyProgramIncome = CaseMSTEntity.ProgIncome.ToString().Equals(string.Empty) ? 0.0 : double.Parse(CaseMSTEntity.ProgIncome);
                    int HUDIDX = 0;
                    double GDL1Value = masterPovertyDetail.Gdl1Value.ToString().Equals(string.Empty) ? 0.0 : double.Parse(masterPovertyDetail.Gdl1Value);
                    double GDL2Value = masterPovertyDetail.Gdl2Value.ToString().Equals(string.Empty) ? 0.0 : double.Parse(masterPovertyDetail.Gdl2Value);
                    double GDL3Value = masterPovertyDetail.Gdl3Value.ToString().Equals(string.Empty) ? 0.0 : double.Parse(masterPovertyDetail.Gdl3Value);
                    double GDL4Value = masterPovertyDetail.Gdl4Value.ToString().Equals(string.Empty) ? 0.0 : double.Parse(masterPovertyDetail.Gdl4Value);
                    double GDL5Value = masterPovertyDetail.Gdl5Value.ToString().Equals(string.Empty) ? 0.0 : double.Parse(masterPovertyDetail.Gdl5Value);
                    double GDL6Value = masterPovertyDetail.Gdl6Value.ToString().Equals(string.Empty) ? 0.0 : double.Parse(masterPovertyDetail.Gdl6Value);
                    MasterPovertyEntity masterPovertyAmoutDetail = null;
                    if ((CaseMSTEntity.NoInProg != "") && (CaseMSTEntity.NoInProg != "0"))
                    {
                        if (masterpovertycount > Convert.ToInt64(CaseMSTEntity.NoInProg))
                        {
                            GDLSW = true;
                        }
                        int inprogress = CaseMSTEntity.NoInProg.ToString().Equals(string.Empty) ? 0 : int.Parse(CaseMSTEntity.NoInProg);
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

        private void Programdefinationcheck()
        {
            if (propAgencyControlDetails.IncVerfication.ToUpper() == "Y")
            {
                panel2.Size = new Size(panel2.Width, 107);
                pnlIncomeVerf.Visible = true; //Kranthi :: 07-24-2023 :: July 2023 Enhancements docs:: Want to show Incom verified fields when ACR switch is on



                lblVerfificationDate.Visible = true;
                calVerificationDate.Visible = true;
                txtCmi.Visible = true;
                txtFedOmb.Visible = true;
                lblFedOmbReq.Visible = true;
                txtHud.Visible = true;
                txtSmi.Visible = true;
                lblCmi.Visible = true;
                lblFedOmb.Visible = true;
                lblHud.Visible = true;
                lblSmi.Visible = true;
                btn1Omb.Visible = true;
                btn2Cmi.Visible = true;
                btn3Smi.Visible = true;
                btn4Hud.Visible = true;
                if (propAgencyControlDetails.VerSwitch.ToUpper() == "Y")
                {
                    btnIncomeHistory.Visible = true;
                }
                else
                {
                    btnIncomeHistory.Visible = false;
                }

                //this.dataGridCaseIncome.Size = new System.Drawing.Size(922, 327);
                if (programDefinitionList != null)
                {

                    if (programDefinitionList.DepFEDUsed == "Y")
                    {
                        lblFedOmbReq.Visible = true;
                        btn1Omb.Enabled = true;
                        txtFedOmb.Enabled = true;

                    }
                    if (programDefinitionList.DepCMIUsed == "Y")
                    {
                        btn2Cmi.Enabled = true;
                        txtCmi.Enabled = true;
                    }
                    else
                    {
                        btn2Cmi.Enabled = false;
                        txtCmi.Enabled = false;
                    }
                    if (programDefinitionList.DepSMIUsed == "Y")
                    {
                        btn3Smi.Enabled = true;
                        txtSmi.Enabled = true;
                    }
                    else
                    {
                        btn3Smi.Enabled = false;
                        txtSmi.Enabled = false;
                    }
                    if (programDefinitionList.DepHUDUsed == "Y")
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
            else
            {
                //  this.dataGridCaseIncome.Size = new System.Drawing.Size(922, 354);
                panel2.Size = new Size(panel2.Width, 35);
                pnlIncomeVerf.Visible = false; //Kranthi :: 07-24-2023 :: July 2023 Enhancements docs:: Want to show Incom verified fields when ACR switch is Off

                chkCheckStubs.Enabled = false;
                chkLetter.Enabled = false;
                chkOther.Enabled = false;
                chk_self_Decl.Enabled = false;
                chkTaxReturn.Enabled = false;
                chkw2.Enabled = false;

                lblVerfificationDate.Visible = false;
                calVerificationDate.Visible = false;
                txtCmi.Visible = false;
                txtFedOmb.Visible = false;
                lblFedOmbReq.Visible = false;
                txtHud.Visible = false;
                txtSmi.Visible = false;
                lblCmi.Visible = false;
                lblFedOmb.Visible = false;
                lblHud.Visible = false;
                lblSmi.Visible = false;
                btnIncomeHistory.Visible = false;

            }
        }

        private bool ShowFedDetailsMessages()
        {
            bool boolfeddata = true;
            if (propAgencyControlDetails.IncVerfication.ToUpper() == "Y")
            {
                if (propAgencyControlDetails.VerSwitch.ToUpper() == "Y")
                {
                    if (calVerificationDate.Checked == true)
                    {
                        if (calVerificationDate.Value > DateTime.Now)
                        {
                            CommonFunctions.MessageBoxDisplay("Future date not allowed");
                            calVerificationDate.ValueChanged -= new EventHandler(calVerificationDate_ValueChanged);
                            calVerificationDate.Text = DateTime.Now.ToShortDateString();
                            calVerificationDate.Checked = false;
                            boolfeddata = false;
                            goto kk;
                        }
                    }
                }
                if (programDefinitionList != null)
                {
                    if (programDefinitionList.DepFEDUsed == "Y")
                    {
                        if (!ShowFedDetailsValid("FED"))
                        {
                            boolfeddata = false;
                            goto kk;
                        }
                    }
                    if (programDefinitionList.DepCMIUsed == "Y")
                    {
                        if (!ShowFedDetailsValid("CMI"))
                        {
                            boolfeddata = false;
                            goto kk;
                        }
                    }

                    if (programDefinitionList.DepSMIUsed == "Y")
                    {
                        if (!ShowFedDetailsValid("SMI"))
                        {
                            boolfeddata = false;
                            goto kk;
                        }
                    }
                    if (programDefinitionList.DepHUDUsed == "Y")
                    {
                        if (!ShowFedDetailsValid("HUD"))
                        {
                            boolfeddata = false;
                            goto kk;
                        }
                    }

                }


            }
        kk:
            return boolfeddata;

        }


        private void HourlyEnableControl(string strIntervalValue)
        {
            if (strIntervalValue == "H")
            {
                txtHr1.Visible = true;
                txtHr2.Visible = true;
                txtHr3.Visible = true;
                txtHr4.Visible = true;
                txtHr5.Visible = true;
                lblHr1.Visible = true;
                lblHr2.Visible = true;
                lblHr3.Visible = true;
                lblHr4.Visible = true;
                lblHr5.Visible = true;
                if (btnSave.Visible == true)
                {
                    txtHr1.Enabled = true;
                    txtHr2.Enabled = true;
                    txtHr3.Enabled = true;
                    txtHr4.Enabled = true;
                    txtHr5.Enabled = true;
                }
                else
                {
                    txtHr1.Enabled = false;
                    txtHr2.Enabled = false;
                    txtHr3.Enabled = false;
                    txtHr4.Enabled = false;
                    txtHr5.Enabled = false;
                }

            }
            else
            {
                txtHr1.Visible = false;
                txtHr2.Visible = false;
                txtHr3.Visible = false;
                txtHr4.Visible = false;
                txtHr5.Visible = false;
                lblHr1.Visible = false;
                lblHr2.Visible = false;
                lblHr3.Visible = false;
                lblHr4.Visible = false;
                lblHr5.Visible = false;
                txtHr1.Enabled = false;
                txtHr2.Enabled = false;
                txtHr3.Enabled = false;
                txtHr4.Enabled = false;
                txtHr5.Enabled = false;
            }
        }

        private void txtHr1_Leave(object sender, EventArgs e)
        {
            if (dataGridCaseIncome.Rows.Count > 0)
            {
                if (dataGridCaseIncome.SelectedCells.Count > 0)
                {
                    dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["gvtHrRate1"].Value = txtHr1.Text;
                    if (propHourlyMode == "Y")
                    {
                        int introwindex = dataGridCaseIncome.CurrentCell.RowIndex;
                        string strInterval = dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["Interval"].Value.ToString();
                        CalculationAmount(strInterval, introwindex);
                    }
                }
            }
        }

        private void txtHr2_Leave(object sender, EventArgs e)
        {
            if (dataGridCaseIncome.Rows.Count > 0)
            {
                if (dataGridCaseIncome.SelectedCells.Count > 0)
                {
                    dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["gvtHrRate2"].Value = txtHr2.Text;
                    if (propHourlyMode == "Y")
                    {
                        int introwindex = dataGridCaseIncome.CurrentCell.RowIndex;
                        string strInterval = dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["Interval"].Value.ToString();
                        CalculationAmount(strInterval, introwindex);
                    }
                }
            }
        }

        private void txtHr3_Leave(object sender, EventArgs e)
        {
            if (dataGridCaseIncome.Rows.Count > 0)
            {
                if (dataGridCaseIncome.SelectedCells.Count > 0)
                {
                    dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["gvtHrRate3"].Value = txtHr3.Text;
                    if (propHourlyMode == "Y")
                    {
                        int introwindex = dataGridCaseIncome.CurrentCell.RowIndex;
                        string strInterval = dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["Interval"].Value.ToString();
                        CalculationAmount(strInterval, introwindex);
                    }
                }
            }
        }

        private void txtHr4_Leave(object sender, EventArgs e)
        {
            if (dataGridCaseIncome.Rows.Count > 0)
            {
                if (dataGridCaseIncome.SelectedCells.Count > 0)
                {
                    dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["gvtHrRate4"].Value = txtHr4.Text;
                    if (propHourlyMode == "Y")
                    {
                        int introwindex = dataGridCaseIncome.CurrentCell.RowIndex;
                        string strInterval = dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["Interval"].Value.ToString();
                        CalculationAmount(strInterval, introwindex);
                    }
                }
            }
        }

        private void txtHr5_Leave(object sender, EventArgs e)
        {
            if (dataGridCaseIncome.Rows.Count > 0)
            {
                if (dataGridCaseIncome.SelectedCells.Count > 0)
                {

                    dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["gvtHrRate5"].Value = txtHr5.Text;
                    if (propHourlyMode == "Y")
                    {
                        int introwindex = dataGridCaseIncome.CurrentCell.RowIndex;
                        string strInterval = dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["Interval"].Value.ToString();
                        CalculationAmount(strInterval, introwindex);
                    }
                }
            }
        }

        private void btnIncomeTotal_Click(object sender, EventArgs e)
        {
            if (dataGridCaseIncome.Rows.Count > 0)
            {
                CaseIncomeEntity drow = dataGridCaseIncome.Rows[dataGridCaseIncome.CurrentRow.Index].Tag as CaseIncomeEntity;
                if (drow != null)
                {
                    string strFamilySeq = string.Empty;
                    CaseSnpEntity row = dataGridCaseSnp.SelectedRows[0].Tag as CaseSnpEntity;
                    if (row != null)
                        strFamilySeq = row.FamilySeq;


                    string strFactor = dataGridCaseIncome.Rows[dataGridCaseIncome.CurrentRow.Index].Cells["Factor"].Value.ToString();

                    CaseIncomeTotDetails incomedetails = new CaseIncomeTotDetails(BaseForm, dataGridCaseIncome.Rows[dataGridCaseIncome.CurrentRow.Index].Cells["IncomeTypeCode"].Value.ToString(), dataGridCaseIncome.Rows[dataGridCaseIncome.CurrentRow.Index].Cells["Interval"].Value.ToString(), dataGridCaseIncome.Rows[dataGridCaseIncome.CurrentRow.Index].Cells["IncomeType"].Value.ToString(), string.Empty, strFamilySeq, strFactor, programDefinitionList);
                    incomedetails.StartPosition = FormStartPosition.CenterScreen;
                    incomedetails.ShowDialog();
                }
                else
                {
                    CommonFunctions.MessageBoxDisplay("Please Select Income Type");
                }
            }
        }

        private void btnIncomeHistory_Click(object sender, EventArgs e)
        {
            CASEVERHISTORY verHistory = new CASEVERHISTORY(BaseForm, Privileges);
            verHistory.StartPosition = FormStartPosition.CenterScreen;
            verHistory.ShowDialog();
        }

        private void CaseIncomeForm2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnSave.Visible)
            {
                DialogResult result = MessageBox.Show("Are you sure want to close? Record not saved", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerFormClosed);
                _caseIncomeForm2 = (Wisej.Web.Form)sender;
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

        Wisej.Web.Form _caseIncomeForm2;
        string Refresh_Control = string.Empty;
        private void MessageBoxHandlerFormClosed(DialogResult dialogResult)
        {
            // senderForm.FormClosing -= SampleTestForm_FormClosing;

            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                _caseIncomeForm2.FormClosing -= CaseIncomeForm2_FormClosing;
                _caseIncomeForm2.Close();
            }
        }

        private void cmbVerfMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (((ListItem)cmbVerfMethod.SelectedItem).Value.ToString() != "0")
                {
                    if (dataGridCaseIncome.Rows.Count > 0)
                    {
                        if (dataGridCaseIncome.SelectedCells.Count > 0) 
                        {
                            dataGridCaseIncome.Rows[dataGridCaseIncome.SelectedCells[0].RowIndex].Cells["Column1"].Value = ((ListItem)cmbVerfMethod.SelectedItem).Value.ToString();
                        }
                    }
                    //if (((ListItem)cmbVerfMethod.SelectedItem).ID.ToString() != "N")
                    //    MessageBox.Show("Inactive Verification Method", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}