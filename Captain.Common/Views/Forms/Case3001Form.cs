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
using Captain.Common.Views.Forms.Base;
using Captain.Common.Menus;
using System.Data.SqlClient;
using Captain.Common.Views.Controls;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.DatabaseLayer;
using Captain.Common.Views.UserControls;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class Case3001Form : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private CustomQuestionsControl customFieldsControl = null;
        private CustomQuestionsControl customFieldsIntakeControl = null;
        List<FldcntlHieEntity> _fldCntlHieEntity = new List<FldcntlHieEntity>();
        private string strYear;
        private string strApp;
        private string strApplNo;
        private string strClientIdOut;
        private string strFamilyIdOut;
        private string strSSNNOOut;
        private string strErrorMsg;
        private string strProgIncome = "0";
        private string strCaseWorkerDefaultCode = "0";
        private string strCaseWorkerDefaultStartCode = "0";
        private string strCaseworkerDesc = string.Empty;
        private string strRelationCode = string.Empty;
        private string strSsnNumber = string.Empty;
        #endregion

        public Case3001Form(BaseForm baseForm, bool isApplicant, CaseMstEntity caseMST, CaseSnpEntity caseSNP, string mode, PrivilegeEntity privilegeEntity, List<CaseSnpEntity> CaseSnpEntityProp, string strApplicantLastName)
        {
            InitializeComponent();
            _model = new CaptainModel();
            Refresh_Control = string.Empty;
            IsApplicant = isApplicant;
            CaseMST = caseMST;
            CaseSNP = caseSNP;
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            Mode = mode;
            Relationshipdefaultcode = string.Empty;
            IsExclude = false;
            SearchSnpType = string.Empty;
            SearchSnpType1 = string.Empty;
            SearchMstType = Consts.CASE2001.ManualType;
            AltAgency = string.Empty;
            AltDept = string.Empty;
            AltProgram = string.Empty;
            AltYear = string.Empty;
            AltApp = string.Empty;
            AltFamilySeq = string.Empty;
            ApplicantLastName = strApplicantLastName;
            boolHouseingMsg = false;
            this.Text = privilegeEntity.Program + " - " + mode;
            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(CaseMST.ApplAgency, CaseMST.ApplDept, CaseMST.ApplProgram);
            propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
            if (propAgencyControlDetails != null)
            {
                if (propAgencyControlDetails.AgyShortName.ToUpper() == "CABA")
                {
                    txtCurrentAge.Visible = false;
                }

            }

            propBaseAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile(CaseMST.ApplAgency);
            if (programEntity != null)
            {
                ProgramDefinition = programEntity;
            }
            dtpInitialDate.Checked = false;
            dtpCaseReview.Checked = false;
            if (Mode.Equals(Consts.Common.Add))
            {
                //if (ProgramDefinition.ProDupMEM.Equals("Y"))
                //{
                //  CaseSnpAllList = _model.CaseMstData.GetCaseSnpSSno(string.Empty);
                //}
                //else
                //{
                //    CaseSnpAllList = CaseSnpEntityProp;
                //}
                //if (IsApplicant)
                //{
                //    if (ProgramDefinition.PRODUPSSN.Equals("Y"))
                //    {
                // CaseMstAllList = _model.CaseMstData.GetCaseMstSSno(string.Empty);
                //    }

                //}

                ckActive.Checked = true;
                ckActive_Click(ckActive, new EventArgs());
                ckExcludeMember.Enabled = true;
                mskSSN.Focus();
                if (IsApplicant)
                {
                    //dtpIntakeDate.Value = DateTime.Now.Date;
                    //dtpCaseReview.Value = DateTime.Now.Date;
                    //dtpInitialDate.Value = DateTime.Now.Date;

                    dtpCaseReview.Checked = false;
                    dtpInitialDate.Checked = false;

                }
                //dtBirth.Value = DateTime.Now.Date;
                dtBirth.Checked = false;
                //dtExpirationdate.Value = DateTime.Now.Date;
                dtExpirationdate.Checked = false;
                //dtElastDateWorked.Value = DateTime.Now.Date;
                //dtEHireDate.Value = DateTime.Now.Date;
                dtElastDateWorked.Checked = false;
                dtEHireDate.Checked = false;

            }
            fillLegaltowork();
            fillWaitList();
            fillEmployeeCombo();
            RemoveEventHandler();
            fillDropDowns();

            fillSNPDetails();
            if (IsApplicant)
            {

                string HIE = CaseMST.ApplAgency + CaseMST.ApplDept + CaseMST.ApplProgram;
                preassesCntlEntity = _model.FieldControls.GetFLDCNTLHIE("PREASSES", HIE, "PREASSES");
                btnSearch.Visible = false;
                RemoveMstEventHandler();
                fillMSTDropDowns();
                if (preassesCntlEntity.Count > 0)
                {
                    if (!preassesCntlEntity.Exists(u => u.Enab.Equals("Y")))
                    {
                        tabControl1.TabPages[4].Hidden = true;
                    }
                    else
                    {
                        preassessMasterEntity = _model.lookupDataAccess.GetDimension(); // CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "07001", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);
                        // preassessMasterEntity = _model.FieldControls.GetPreassessData("MASTER");
                        preassessChildEntity = _model.FieldControls.GetPreassessData(string.Empty);
                        proppreassesQuestions = _model.FieldControls.GetPreassesQuestions("PREASSES", "A", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, "Sequence", "ACTIVE", "P");
                        propcustRespAllEntity = _model.FieldControls.GetCustomResponses("PREASSES", string.Empty);

                        cmbQuestionType.Enabled = false;
                        lblPQuestionType.Enabled = false;
                        if (Mode.Equals(Consts.Common.Add))
                        {
                            //cmbQuestionType.Visible = false;
                            //lblPQuestionType.Visible = false;
                            cmbQuestionType.SelectedIndex = 1;
                            fillPreassCustomQuestions();
                        }

                        if (Mode.Equals(Consts.Common.Edit) || Mode.Equals(Consts.Common.View))
                        {
                            cmbQuestionType.SelectedIndex = 0;
                            fillPreassCustomQuestions();
                            bool boolinactiveQuestions = false;
                            foreach (DataGridViewRow gvrows in gvwPreassesData.Rows)
                            {
                                if ((gvrows.Cells["gvtPSave"].Tag == null ? string.Empty : gvrows.Cells["gvtPSave"].Tag.ToString()) == "Y")
                                {
                                    boolinactiveQuestions = true;
                                    break;
                                }
                            }
                            if (boolinactiveQuestions)
                            {
                                cmbQuestionType.SelectedIndex = 2;
                                btnInactiveQues.Visible = true;
                                fillPreassCustomQuestions();
                            }
                            else
                            {
                                cmbQuestionType.SelectedIndex = 1;
                                fillPreassCustomQuestions();
                            }
                            if (Mode.Equals(Consts.Common.View))
                            {
                                gvwPreassesData.Enabled = false;
                            }

                        }

                        tabControl1.TabPages[4].Hidden = false;
                    }
                }
                else
                {
                    tabControl1.TabPages[4].Hidden = true;
                }

                fillOutofService();
                fillClientIntake();

                if (Mode.Equals(Consts.Common.Add))
                {
                    propCaseDiffLLDetails = null;
                    propCaseDiffMailDetails = null;

                    if (propAgencyControlDetails.State.ToUpper() == "TX")
                    {
                        txtState.Text = propAgencyControlDetails.State.ToString();
                    }

                    //    tabControl1.TabPages[2].Hidden = true;
                }
            }
            else
            {
                tabControl1.TabPages[0].Text = "Member Details";
                tabControl1.TabPages[1].Hidden = true;
                tabControl1.TabPages[2].Hidden = true;
                tabControl1.TabPages[4].Hidden = true;
                btnSearch.Visible = true;
            }

            customFieldsControl = new CustomQuestionsControl(BaseForm, CaseSNP, "CASE2001", isApplicant, Mode, programEntity);
            customFieldsControl.Dock = DockStyle.Fill;
            customFieldsControl.MaxButtonControl.Click += new EventHandler(OnMaxMinClick);
            customFieldsControl.IsMax = false;
            customFieldsControl.IsApplicant = IsApplicant;
            customFieldsControl.AccessLevel = IsApplicant ? "A" : "*";
            splitApplicant.Panel2.Controls.Add(customFieldsControl);

            customFieldsIntakeControl = new CustomQuestionsControl(BaseForm, CaseSNP, "CASE2001", true, Mode, programEntity);
            customFieldsIntakeControl.Dock = DockStyle.Fill;
            customFieldsIntakeControl.MaxButtonControl.Click += new EventHandler(OnIntakeMaxMinClick);
            customFieldsIntakeControl.IsMax = false;
            customFieldsIntakeControl.IsApplicant = true;
            customFieldsIntakeControl.AccessLevel = "H";
            customFieldsIntakeControl.filterCustomQuestions("H");
            splitIntake.Panel2.Controls.Add(customFieldsIntakeControl);

            if (mode.Equals(Consts.Common.Edit))
            {
                if (CaseSNP != null)
                {
                    mskSSN.Text = CaseSNP.Ssno;
                    //string Agy, Dept, Prog, Year, App, MemCode = null;
                    //Agy = CaseSNP.Agency;
                    //Dept = CaseSNP.Dept;
                    //Prog = CaseSNP.Program;
                    //Year = CaseSNP.Year;
                    //App = CaseSNP.App;
                    //MemCode = CaseSNP.FamilySeq;
                    //fillCaseSnpDetails(Agy, Dept, Prog, Year, App, MemCode);
                }
                CaseDiffEntity caseDiffDetails = _model.CaseMstData.GetCaseDiffadpya(BaseForm.BaseAgency.ToString(), BaseForm.BaseDept.ToString(), BaseForm.BaseProg.ToString(), BaseForm.BaseYear, BaseForm.BaseCaseMstListEntity[0].ApplNo, string.Empty);
                if (caseDiffDetails != null)
                {
                    btnMailingAddress.Text = " *  Mailing Address";
                    btnMailingAddress.ForeColor = System.Drawing.Color.Green;
                    btnMailingAddress2.Text = " *  Mailing Address";
                    btnMailingAddress2.ForeColor = System.Drawing.Color.Green;

                }
                else
                {
                    btnMailingAddress.Text = "Mailing Address";
                    btnMailingAddress2.Text = "Mailing Address";
                }
                if (BaseForm.BaseAgencyControlDetails.MailAddressSwitch.ToUpper() == "Y")
                {
                    btnMailingAddress.Visible = false;
                    btnMailingAddress2.Visible = true;
                }
                else
                {
                    btnMailingAddress.Visible = true;
                    btnMailingAddress2.Visible = false;
                }
                // btnLandlordInfo.Visible = true;

            }
            else
            {

                if (BaseForm.BaseAgencyControlDetails.MailAddressSwitch.ToUpper() == "Y")
                {
                    btnMailingAddress.Visible = false;
                    btnMailingAddress2.Visible = true;
                }
                else
                {
                    btnMailingAddress.Visible = true;
                    btnMailingAddress2.Visible = false;
                }
                //btnLandlordInfo.Visible = false;


            }
            if (!mode.Equals(Consts.Common.View))
            {

                if (propAgencyControlDetails.AgyShortName.ToUpper() == "CABA")
                {
                    ckExcludeMember.Visible = false;
                }
                EnableDisableControls();
                OnGenderSelectionChanged(cmbGender, new EventArgs());
                checkNA_CheckedChanged_1(checkNA, new EventArgs());
                ChkHome_Na_CheckedChanged(ChkHome_Na, new EventArgs());
                chkMessage_Na_CheckedChanged(chkMessage_Na, new EventArgs());
                chk_CellNa_CheckedChanged(chk_CellNa, new EventArgs());
                chkEmail_Na_CheckedChanged(chkEmail_Na, new EventArgs());

            }
            else
            {
                btnCancel.Text = "Close";
                btnSave.Visible = false;
                btnSearch.Visible = false;
                mskSSN.Enabled = false;
                btnSite.Visible = false;
                groupBox2.Enabled = false;
                groupBox3.Enabled = false;
                pnlEmployee.Enabled = false;
                pnlHire.Enabled = false;
                pnlSeasonalEmploy.Enabled = false;
                ckExcludeMember.Enabled = false;
                chkExcludeHHMem.Enabled = false;
                if (BaseForm.BaseAgencyControlDetails.MailAddressSwitch.ToUpper() == "Y")
                {
                    btnMailingAddress.Visible = false;
                    btnMailingAddress2.Visible = true;
                }
                else
                {
                    btnMailingAddress.Visible = true;
                    btnMailingAddress2.Visible = false;
                }
                gvwPreassesData.Enabled = false;
                // btnLandlordInfo.Visible = true;
                string HIE = CaseMST.ApplAgency + CaseMST.ApplDept + CaseMST.ApplProgram;
                CntlEntity = _model.FieldControls.GetFLDCNTLHIE("CASE2001", HIE, "FLDCNTL");
                FLDCNTLHieEntity = CntlEntity;
                FldcntlHieEntity FldcntlHieEntitySuffix = CntlEntity.Find(u => u.FldCode == Consts.CASE2001.AppSuffix);
                if (FldcntlHieEntitySuffix != null)
                {
                    bool enabled = FldcntlHieEntitySuffix.Enab.Equals("Y") ? true : false;
                    if (enabled) { txtAppSuffix.Visible = lblAppSuffix.Visible = true; txtAppSuffix.Enabled = lblAppSuffix.Enabled = false; } else { txtAppSuffix.Visible = lblAppSuffix.Visible = txtAppSuffix.Enabled = lblAppSuffix.Enabled = false; }
                }

                FldcntlHieEntity FldcntlHieEntityExcludeMember2 = CntlEntity.Find(u => u.FldCode == Consts.CASE2001.ExcludeMember2);
                if (FldcntlHieEntityExcludeMember2 != null)
                {
                    if (propAgencyControlDetails.AgyShortName.ToUpper() == "CABA")
                    {
                        ckExcludeMember.Visible = false; lblSnpExcludeMemberReq.Visible = false;

                        bool enabled = FldcntlHieEntityExcludeMember2.Enab.Equals("Y") ? true : false;
                        if (enabled) { chkExcludeHHMem.Visible = true; chkExcludeHHMem.Enabled = false; } else { chkExcludeHHMem.Visible = chkExcludeHHMem.Enabled = false; }
                    }
                }

            }
            if (mode.Equals(Consts.Common.Edit))
            {
                mskSSN.Enabled = true;
                btnSearch.Visible = false;
                txtFirstName.Focus();

            }

            if (IsApplicant)
            {
                //cmbRelation.Enabled = false; 
                //AbcdControlsVisable();
                DisableZipcodeDetails();
                AddMstEventHandler();
                CaseSiteEntityList = _model.CaseMstData.GetCaseSite(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "SiteHie");
                if (BaseForm.BaseAgencyControlDetails.SiteSecurity.ToString() == "1")
                {
                    HierarchyEntity hierarchyEntity = new HierarchyEntity();
                    List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
                    foreach (HierarchyEntity Entity in userHierarchy)
                    {
                        if (Entity.InActiveFlag == "N")
                        {
                            if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == BaseForm.BaseDept && Entity.Prog == BaseForm.BaseProg)
                                hierarchyEntity = Entity;
                            else if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == BaseForm.BaseDept && Entity.Prog == "**")
                                hierarchyEntity = Entity;
                            else if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == "**" && Entity.Prog == "**")
                                hierarchyEntity = Entity;
                            else if (Entity.Agency == "**" && Entity.Dept == "**" && Entity.Prog == "**")
                            {
                                //Entity.Agency = Agency;
                                hierarchyEntity = Entity;
                            }
                        }
                    }

                    List<CaseSiteEntity> ListcaseSiteEntity = new List<CaseSiteEntity>();

                    if (hierarchyEntity.Sites.Length > 0)
                    {
                        string[] Sites = hierarchyEntity.Sites.Split(',');

                        for (int i = 0; i < Sites.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(Sites[i].ToString().Trim()))
                            {
                                foreach (CaseSiteEntity casesite in CaseSiteEntityList) //Site_List)//ListcaseSiteEntity)
                                {
                                    if (Sites[i].ToString() == casesite.SiteNUMBER)
                                    {
                                        ListcaseSiteEntity.Add(casesite);
                                        //break;
                                    }
                                    // Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
                                }
                            }
                        }
                    }
                    //else if (hierarchyEntity.Agency + hierarchyEntity.Dept + hierarchyEntity.Prog != "******") CaseSiteEntityList = ListcaseSiteEntity;

                    if (ListcaseSiteEntity.Count > 0) CaseSiteEntityList = ListcaseSiteEntity;
                }
            }
            AddEventHandler();
            SetNumeric();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.IconPadding = 2;
        }

        #region Properties

        public BaseForm BaseForm { get; set; }

        public string Agency { get; set; }

        public string Dept { get; set; }

        public string Program { get; set; }

        public string Year { get; set; }

        public string App { get; set; }

        public string FamilySeq { get; set; }

        public bool IsExclude { get; set; }

        public bool IsApplicant { get; set; }

        public string ApplicantLastName { get; set; }

        public CaseMstEntity CaseMST { get; set; }

        public List<CaseSnpEntity> CaseSnpAllList { get; set; }

        public List<CaseMstEntity> CaseMstAllList { get; set; }

        public List<CaseSiteEntity> CaseSiteEntityList { get; set; }

        List<CASEVOTEntity> propcaseVot { get; set; }

        public CaseSnpEntity CaseSNP { get; set; }

        public string SearchSnpType { get; set; }
        public string SearchSnpType1 { get; set; }

        public string SearchMstType { get; set; }

        public CaseDiffEntity propCaseDiffLLDetails { get; set; }

        public CaseDiffEntity propCaseDiffMailDetails { get; set; }

        public AgencyControlEntity propAgencyControlDetails { get; set; }
        public AgencyControlEntity propBaseAgencyControlDetails { get; set; }
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

        public string Mode { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public ToolBarButton ToolBarEdit { get; set; }

        public ToolBarButton ToolBarNew { get; set; }

        public ToolBarButton ToolBarNewMember { get; set; }

        public ToolBarButton ToolBarHelp { get; set; }

        public string MainMenuAgency { get; set; }

        public string MainMenuDept { get; set; }

        public string MainMenuProgram { get; set; }

        public string MainMenuHIE { get; set; }

        public string AltAgency { get; set; }
        public string AltDept { get; set; }
        public string AltProgram { get; set; }
        public string AltYear { get; set; }
        public string AltApp { get; set; }
        public string AltFamilySeq { get; set; }
        public string Relationshipdefaultcode { get; set; }

        public ProgramDefinitionEntity ProgramDefinition { get; set; }
        public List<FldcntlHieEntity> CntlEntity { get; set; }
        public List<FldcntlHieEntity> preassesCntlEntity { get; set; }
        public List<CommonEntity> preassessMasterEntity { get; set; }
        public List<PreassessQuesEntity> preassessChildEntity { get; set; }
        List<CustRespEntity> propcustRespAllEntity { get; set; }
        List<CustomQuestionsEntity> proppreassesQuestions { get; set; }


        public bool boolHouseingMsg { get; set; }

        #endregion

        private void EnableDisableControls()
        {
            string HIE = CaseMST.ApplAgency + CaseMST.ApplDept + CaseMST.ApplProgram;
            CntlEntity = _model.FieldControls.GetFLDCNTLHIE("CASE2001", HIE, "FLDCNTL");
            FLDCNTLHieEntity = CntlEntity;
            if (!CntlEntity.Exists(u => u.Enab.Equals("Y")))
            {
                //MessageBox.Show("Field controls not defined for this program");
                btnSave.Enabled = false;
                btnMailingAddress.Visible = false;
                btnMailingAddress2.Visible = false;
                btnLandlordInfo.Visible = false;
            }
            if (Mode.Equals(Consts.Common.Add)) cbActive.Checked = cbActive.Checked = true;
            foreach (FldcntlHieEntity entity in CntlEntity)
            {
                bool required = entity.Req.Equals("Y") ? true : false;
                bool enabled = entity.Enab.Equals("Y") ? true : false;

                switch (entity.FldCode)
                {
                    case Consts.CASE2001.SSN:
                        if (Mode.Equals(Consts.Common.Add))
                        {
                            if (ProgramDefinition.DepIntakeProg.Equals("Y") && ProgramDefinition.DepSsnAutoAssign != "0")
                            {
                                if (ProgramDefinition.DepSsnAutoAssign == "1")
                                {
                                    mskSSN.Enabled = lblSSN.Enabled = txtBIC.Enabled = ssnReq.Visible = false; btnSearch.Enabled = true;// Jen's Questions 2_12_2021 asked Murali changed btnsearch enable false to true on 02/17/2021.

                                }
                                if (ProgramDefinition.DepSsnAutoAssign == "2")
                                {
                                    mskSSN.Enabled = lblSSN.Enabled = txtBIC.Enabled = btnSearch.Enabled = true; ssnReq.Visible = false;
                                }
                            }
                            else
                            {
                                if (enabled) { mskSSN.Enabled = lblSSN.Enabled = btnSearch.Enabled = true; if (required) ssnReq.Visible = true; } else { mskSSN.Enabled = lblSSN.Enabled = btnSearch.Enabled = false; ssnReq.Visible = false; }
                            }
                        }
                        else
                        {
                            if (enabled) { mskSSN.Enabled = lblSSN.Enabled = btnSearch.Enabled = true; if (required) ssnReq.Visible = true; } else { mskSSN.Enabled = lblSSN.Enabled = btnSearch.Enabled = false; ssnReq.Visible = false; }

                        }
                        break;
                    case Consts.CASE2001.BIC:
                        if (Mode.Equals(Consts.Common.Add))
                        {
                            if (ProgramDefinition.DepIntakeProg.Equals("Y") && ProgramDefinition.DepSsnAutoAssign != "0")
                            { }
                            else
                            {
                                if (enabled) { txtBIC.Enabled = true; } else { txtBIC.Enabled = false; }
                            }
                        }
                        else
                        {
                            if (enabled) { txtBIC.Enabled = true; } else { txtBIC.Enabled = false; }
                        }
                        break;
                    case Consts.CASE2001.FirstName:
                        if (enabled) { txtFirstName.Enabled = lblFirstName.Enabled = true; if (required) firstnameReq.Visible = true; } else { txtFirstName.Enabled = lblFirstName.Enabled = false; firstnameReq.Visible = false; }
                        break;
                    case Consts.CASE2001.LastName:
                        if (enabled) { txtLastName.Enabled = lblLastName.Enabled = true; if (required) LastNameReq.Visible = true; } else { txtLastName.Enabled = lblLastName.Enabled = false; LastNameReq.Visible = false; }
                        break;
                    case Consts.CASE2001.MI:
                        if (enabled) { txtMI.Enabled = lblMI.Enabled = true; if (required) lblMIReq.Visible = true; } else { txtMI.Enabled = lblMI.Enabled = false; lblMIReq.Visible = false; }
                        break;
                    case Consts.CASE2001.AppSuffix:
                        if (enabled) { txtAppSuffix.Visible = txtAppSuffix.Enabled = lblAppSuffix.Visible = lblAppSuffix.Enabled = true; if (required) lblReqAppSuffix.Visible = true; } else { txtAppSuffix.Visible = lblAppSuffix.Visible = txtAppSuffix.Enabled = lblAppSuffix.Enabled = false; lblReqAppSuffix.Visible = false; }
                        break;
                    case Consts.CASE2001.DateOfBirth:
                        if (enabled) { dtBirth.Enabled = lblDOB.Enabled = checkNA.Enabled = true; if (required) DOBReq.Visible = true; } else { dtBirth.Enabled = lblDOB.Enabled = checkNA.Enabled = false; DOBReq.Visible = false; }
                        break;
                    case Consts.CASE2001.Alias:
                        if (enabled) { txtAlias.Enabled = lblAlias.Enabled = true; if (required) AliasReq.Visible = true; } else { txtAlias.Enabled = lblAlias.Enabled = false; AliasReq.Visible = false; }
                        break;
                    case Consts.CASE2001.Relation:
                        if (enabled) { cmbRelation.Enabled = lblRelation.Enabled = true; if (required) RelationReq.Visible = true; } else { cmbRelation.Enabled = lblRelation.Enabled = false; RelationReq.Visible = false; }
                        break;
                    case Consts.CASE2001.Ethnicity:
                        if (enabled) { cmbEthnicity.Enabled = lblEthnicity.Enabled = true; if (required) EthnicityReq.Visible = true; } else { cmbEthnicity.Enabled = lblEthnicity.Enabled = false; EthnicityReq.Visible = false; }
                        break;
                    case Consts.CASE2001.Race:
                        if (enabled) { cmbRace.Enabled = lblRace.Enabled = true; if (required) RaceReq.Visible = true; } else { cmbRace.Enabled = lblRace.Enabled = false; RaceReq.Visible = false; }
                        break;
                    case Consts.CASE2001.Education:
                        if (enabled) { cmbEducation.Enabled = lblEducation.Enabled = true; if (required) EducationReq.Visible = true; } else { cmbEducation.Enabled = lblEducation.Enabled = false; EducationReq.Visible = false; }
                        break;
                    case Consts.CASE2001.School:
                        if (enabled) { cmbSchool.Enabled = lblSchool.Enabled = true; if (required) SchoolReq.Visible = true; } else { cmbSchool.Enabled = lblSchool.Enabled = false; SchoolReq.Visible = false; }
                        break;
                    case Consts.CASE2001.Gender:
                        if (enabled) { cmbGender.Enabled = lblGender.Enabled = true; if (required) GenderReq.Visible = true; } else { cmbGender.Enabled = lblGender.Enabled = false; GenderReq.Visible = false; }
                        break;
                    case Consts.CASE2001.AreYouPregnant:
                        if (enabled) { cmbAreyoupregnant.Enabled = lblAreYouPregnant.Enabled = true; if (required) PregnantReq.Visible = true; } else { cmbAreyoupregnant.Enabled = lblAreYouPregnant.Enabled = false; PregnantReq.Visible = false; }
                        break;
                    case Consts.CASE2001.MaritalStatus:
                        if (enabled) { cmbMaritalStatus.Enabled = lblMaritalStatus.Enabled = true; if (required) MaritalReq.Visible = true; } else { cmbMaritalStatus.Enabled = lblMaritalStatus.Enabled = false; MaritalReq.Visible = false; }
                        break;
                    case Consts.CASE2001.HealthInsurance:
                        if (enabled) { cmbHealthInsurance.Enabled = lblHealthInsurance.Enabled = true; if (required) HealthReq.Visible = true; } else { cmbHealthInsurance.Enabled = lblHealthInsurance.Enabled = false; HealthReq.Visible = false; }
                        break;
                    case Consts.CASE2001.VeteranCode:
                        if (enabled) { cmbVeteranCode.Enabled = lblVeteranCode.Enabled = true; if (required) VeteranReq.Visible = true; } else { cmbVeteranCode.Enabled = lblVeteranCode.Enabled = false; VeteranReq.Visible = false; }
                        break;
                    case Consts.CASE2001.FoodStamps:
                        if (enabled) { cmbFoodStamps.Enabled = lblFoodStamps.Enabled = true; if (required) FoodStampsReq.Visible = true; } else { cmbFoodStamps.Enabled = lblFoodStamps.Enabled = false; FoodStampsReq.Visible = false; }
                        break;
                    case Consts.CASE2001.Disabled:
                        if (enabled) { cmbDisabled.Enabled = lblDisabled.Enabled = true; if (required) DisabledReq.Visible = true; } else { cmbDisabled.Enabled = lblDisabled.Enabled = false; DisabledReq.Visible = false; }
                        break;
                    case Consts.CASE2001.Farmer:
                        if (enabled) { cmbFarmer.Enabled = lblFarmer.Enabled = true; if (required) FarmerReq.Visible = true; } else { cmbFarmer.Enabled = lblFarmer.Enabled = false; FarmerReq.Visible = false; }
                        break;
                    case Consts.CASE2001.WIC:
                        if (enabled) { cmbWIC.Enabled = lblWIC.Enabled = true; if (required) WICReq.Visible = true; } else { cmbWIC.Enabled = lblWIC.Enabled = false; WICReq.Visible = false; }
                        break;
                    case Consts.CASE2001.ReliableTransport:
                        if (enabled) { cmbReliableTrans.Enabled = lblReliableTransport.Enabled = true; if (required) ReliableReq.Visible = true; } else { cmbReliableTrans.Enabled = lblReliableTransport.Enabled = false; ReliableReq.Visible = false; }
                        break;
                    case Consts.CASE2001.DriversLicense:
                        if (enabled) { cmbDriving.Enabled = lblDriverLicense.Enabled = true; if (required) DriverReq.Visible = true; } else { cmbDriving.Enabled = lblDriverLicense.Enabled = false; DriverReq.Visible = false; }
                        break;
                    case Consts.CASE2001.Resident:
                        if (enabled) { cmbResident.Enabled = lblResident.Enabled = true; if (required) ResidentReq.Visible = true; } else { cmbResident.Enabled = lblResident.Enabled = false; ResidentReq.Visible = false; }
                        break;
                    //case Consts.CASE2001.AlienRegNo:
                    //    if (enabled) { txtAlienRegNo.Enabled = lblAlienRegNo.Enabled = true; if (required) AlienRegNo.Visible = true; } else { txtAlienRegNo.Enabled = lblAlienRegNo.Enabled = false; AlienRegNo.Visible = false; }
                    //    break;
                    //case Consts.CASE2001.LegalToWork:
                    //    if (enabled) { cmbLegaltowork.Enabled = lblLegalToWork.Enabled = true; if (required) LegalToWork.Visible = true; } else { cmbLegaltowork.Enabled = lblLegalToWork.Enabled = false; LegalToWork.Visible = false; }
                    //    break;
                    //case Consts.CASE2001.ExpirationDate:
                    //    if (enabled) { dtExpirationdate.Enabled = lblExpirationDate.Enabled = true; if (required) ExpirationDateReq.Visible = true; } else { dtExpirationdate.Enabled = lblExpirationDate.Enabled = false; ExpirationDateReq.Visible = false; }
                    //    break;
                    case Consts.CASE2001.SnpActive:
                        if (enabled) { ckActive.Enabled = true; if (required) lblSnpActiveReq.Visible = true; } else { ckActive.Enabled = false; lblSnpActiveReq.Visible = true; }
                        break;
                    case Consts.CASE2001.ExcludeMember:
                        if (propAgencyControlDetails.AgyShortName.ToUpper() == "CABA")
                        {
                            if (enabled) { ckExcludeMember.Visible = ckExcludeMember.Enabled = true; if (required) lblSnpExcludeMemberReq.Visible = true; } else { ckExcludeMember.Visible = ckExcludeMember.Enabled = false; lblSnpExcludeMemberReq.Visible = false; }
                        }
                        else
                        {
                            if (enabled) { ckExcludeMember.Enabled = true; if (required) lblSnpExcludeMemberReq.Visible = true; } else { ckExcludeMember.Enabled = false; lblSnpExcludeMemberReq.Visible = false; }
                        }
                        break;
                    case Consts.CASE2001.ExcludeMember2:
                        if (enabled) { chkExcludeHHMem.Visible = chkExcludeHHMem.Enabled = true; if (required) lblSnpExcludeMember2Req.Visible = true; } else { chkExcludeHHMem.Visible = false; lblSnpExcludeMember2Req.Visible = false; }
                        break;
                    //case Consts.CASE2001.SSNReason:
                    //    if (enabled) { cmbSSNReason.Enabled = lblSSNReason.Enabled = true; if (required) lblSSNReasonReq.Visible = true; } else { cmbSSNReason.Enabled = false; }
                    //    break;

                    // ******************************************************* Employee Details ********************************************** //
                    case Consts.CASE2001.EEmployed:
                        if (enabled) { cmbEpresenteEmploy.Enabled = lblPresentEmployed.Enabled = true; if (required) lblPresentEmployedReq.Visible = true; } else { cmbEpresenteEmploy.Enabled = false; }
                        break;
                    //case Consts.CASE2001.ELastWorkDate:
                    //    if (enabled) { dtElastDateWorked.Enabled = lblDOB.Enabled = true; if (required) DOBReq.Visible = true; } else { dtBirth.Enabled = lblDOB.Enabled = checkNA.Enabled = false; DOBReq.Visible = false; }
                    //    break;
                    case Consts.CASE2001.EWorkLimit:
                        if (enabled) { cmbEAnywork.Enabled = lblAnimationwork.Enabled = true; if (required) lblAnimationworkReq.Visible = true; } else { cmbEAnywork.Enabled = false; }
                        break;
                    //case Consts.CASE2001.EExplainworkLimit:
                    //    if (enabled) { txtEExp.Enabled = lblAlias.Enabled = true; if (required) AliasReq.Visible = true; } else { txtAlias.Enabled = lblAlias.Enabled = false; AliasReq.Visible = false; }
                    //    break;
                    case Consts.CASE2001.ENumberofcjobs:
                        if (enabled) { txtEcurrentHave.Enabled = lblCurrentlyhave.Enabled = true; if (required) lblCurrentlyhaveReq.Visible = true; } else { txtEcurrentHave.Enabled = lblCurrentlyhave.Enabled = false; lblCurrentlyhaveReq.Visible = false; }
                        break;
                    case Consts.CASE2001.ENumberofLvjobs:
                        if (enabled) { txtElastvisit.Enabled = lbllastvisit.Enabled = true; if (required) lbllastvisitReq.Visible = true; } else { txtElastvisit.Enabled = lbllastvisit.Enabled = false; lbllastvisitReq.Visible = false; }
                        break;
                    case Consts.CASE2001.EFullTimeHours:
                        if (enabled) { txtEFullTime.Enabled = lblFullTimehours.Enabled = true; if (required) lblFullTimehoursReq.Visible = true; } else { txtEFullTime.Enabled = lblFullTimehours.Enabled = false; lblFullTimehoursReq.Visible = false; }
                        break;
                    case Consts.CASE2001.EPartTimeHours:
                        if (enabled) { txtEPartTime.Enabled = lblPartTimeHours.Enabled = true; if (required) lblPartTimeHoursReq.Visible = true; } else { txtEPartTime.Enabled = lblPartTimeHours.Enabled = false; lblPartTimeHoursReq.Visible = false; }
                        break;
                    case Consts.CASE2001.ESeasonalEmploy:
                        if (enabled) { cmbEseasonalEmployee.Enabled = lblSeasonalEmploy.Enabled = true; if (required) lblSeasonalEmployReq.Visible = true; } else { cmbEseasonalEmployee.Enabled = false; }
                        break;
                    // chk
                    case Consts.CASE2001.EShiftWorked1st:
                        if (enabled) { chkE1st.Enabled = true; if (required) lblEShiftsWorked1stReq.Visible = true; } else { chkE1st.Enabled = false; }
                        break;
                    case Consts.CASE2001.EShiftWorked2nd:
                        if (enabled) { chkE2nd.Enabled = true; if (required) lblE2nreq.Visible = true; } else { chkE2nd.Enabled = false; }
                        break;
                    case Consts.CASE2001.EShiftWorked3rd:
                        if (enabled) { chkE3rd.Enabled = true; if (required) lblE3rdReq.Visible = true; } else { chkE3rd.Enabled = false; }
                        break;
                    case Consts.CASE2001.EShiftWorkedRotating:
                        if (enabled) { chkErotaing.Enabled = true; if (required) lblERotatingReq.Visible = true; } else { chkErotaing.Enabled = false; }
                        break;
                    case Consts.CASE2001.EEmployerName:
                        if (enabled) { txtEEmployer.Enabled = lblEmployer.Enabled = true; if (required) lblEmployerReq.Visible = true; } else { txtEEmployer.Enabled = lblEmployer.Enabled = false; lblEmployerReq.Visible = false; }
                        break;
                    case Consts.CASE2001.EEmployerStreet:
                        if (enabled) { txtEstreet.Enabled = lblEStreet.Enabled = true; if (required) lblEStreetReq.Visible = true; } else { txtEstreet.Enabled = lblEStreet.Enabled = false; lblEStreetReq.Visible = false; }
                        break;
                    case Consts.CASE2001.EEmployerCity:
                        if (enabled) { txtEcityState.Enabled = lblECityState.Enabled = true; if (required) lblECityStateReq.Visible = true; } else { txtEcityState.Enabled = lblECityState.Enabled = false; lblECityStateReq.Visible = false; }
                        break;
                    case Consts.CASE2001.EPhone:
                        if (enabled) { mskEPhone.Enabled = lblEPhone.Enabled = true; if (required) lblEPhoneReq.Visible = true; } else { mskEPhone.Enabled = lblEPhone.Enabled = false; lblEPhoneReq.Visible = false; }
                        break;
                    case Consts.CASE2001.EExt:
                        if (enabled) { txtEExt.Enabled = lblEExt.Enabled = true; if (required) lblEExtReq.Visible = true; } else { txtEExt.Enabled = lblEExt.Enabled = false; lblEExtReq.Visible = false; }
                        break;
                    case Consts.CASE2001.EJobTitle:
                        if (enabled) { cmbEJobTitle.Enabled = lblEjobTitle.Enabled = true; if (required) lblEjobTitleReq.Visible = true; } else { cmbEJobTitle.Enabled = false; }
                        break;
                    case Consts.CASE2001.EJobCategory:
                        if (enabled) { cmbEJobCategory.Enabled = lblEJobCategory.Enabled = true; if (required) lblEJobCategoryReq.Visible = true; } else { cmbEJobCategory.Enabled = false; }
                        break;
                    case Consts.CASE2001.EHourlyWage:
                        if (enabled) { txtEhourlywage.Enabled = lblHourlyWage.Enabled = true; if (required) lblHourlyWageReq.Visible = true; } else { txtEhourlywage.Enabled = lblHourlyWage.Enabled = false; lblHourlyWageReq.Visible = false; }
                        break;
                    case Consts.CASE2001.EPayFreQuency:
                        if (enabled) { cmbEpayFrequency.Enabled = lblPayFrequency.Enabled = true; if (required) lblPayFrequencyReq.Visible = true; } else { cmbEpayFrequency.Enabled = false; }
                        break;
                    case Consts.CASE2001.EHireDate:
                        if (enabled) { dtEHireDate.Enabled = lblEhireDate.Enabled = true; if (required) lblEHireDateReq.Visible = true; } else { dtEHireDate.Enabled = lblEhireDate.Enabled = false; lblEHireDateReq.Visible = false; }
                        break;

                }

                if (IsApplicant)
                {
                    switch (entity.FldCode)
                    {
                        case Consts.CASE2001.AssignedWorker:
                            if (enabled) { cmbStaff.Enabled = lblAssignedWorker.Enabled = true; if (required) lblAssignedWorkerReq.Visible = true; } else { cmbStaff.Enabled = false; lblAssignedWorkerReq.Visible = false; }
                            break;
                        case Consts.CASE2001.HN:
                            if (enabled) { txtHN.Enabled = lblHN.Enabled = true; if (required) lblHNReq.Visible = true; } else { txtHN.Enabled = lblHN.Enabled = false; lblHNReq.Visible = false; }
                            break;
                        case Consts.CASE2001.Street:
                            if (enabled) { txtStreet.Enabled = lblStreet.Enabled = true; if (required) lblStreetReq.Visible = true; } else { txtStreet.Enabled = lblStreet.Enabled = false; lblStreetReq.Visible = false; }
                            break;
                        case Consts.CASE2001.Suffix:
                            if (enabled) { txtSuffix.Enabled = lblSuffix.Enabled = true; if (required) lblSuffixReq.Visible = true; } else { txtSuffix.Enabled = lblSuffix.Enabled = false; lblSuffixReq.Visible = false; }
                            break;
                        case Consts.CASE2001.Floor:
                            if (enabled) { txtFloor.Enabled = lblFloor.Enabled = true; if (required) lblFloorReq.Visible = true; } else { txtFloor.Enabled = lblFloor.Enabled = false; lblFloorReq.Visible = false; }
                            break;
                        case Consts.CASE2001.Direction:
                            if (enabled) { txtDirection.Enabled = lblDirection.Enabled = true; if (required) lblDirectionReq.Visible = true; } else { txtDirection.Enabled = lblDirection.Enabled = false; lblDirectionReq.Visible = false; }
                            break;
                        case Consts.CASE2001.Apartment:
                            if (enabled) { txtApartment.Enabled = lblApartment.Enabled = true; if (required) lblApartmentReq.Visible = true; } else { txtApartment.Enabled = lblApartment.Enabled = false; lblApartmentReq.Visible = false; }
                            break;
                        case Consts.CASE2001.Precinct:
                            if (enabled) { txtPrecinct.Enabled = lblPrecinct.Enabled = true; if (required) lblPrecinctReq.Visible = true; } else { txtPrecinct.Enabled = lblPrecinct.Enabled = false; lblPrecinctReq.Visible = false; }
                            if (((ListItem)cmbOutService.SelectedItem).Value.ToString() != "I" && ((ListItem)cmbOutService.SelectedItem).Value.ToString() != "0")
                            {
                                lblPrecinctReq.Visible = false;
                            }
                            break;
                        case Consts.CASE2001.CaseType:
                            if (enabled) { cmbCaseType.Enabled = lblCaseType.Enabled = true; if (required) lblCaseTypeReq.Visible = true; } else { cmbCaseType.Enabled = false; lblCaseTypeReq.Visible = false; }
                            break;
                        case Consts.CASE2001.City:
                            if (enabled) { txtCity.Enabled = lblCityName.Enabled = btnCitySearch.Enabled = true; if (required) lblCityReq.Visible = true; } else { txtCity.Enabled = false; lblCityReq.Visible = false; btnCitySearch.Enabled = false; }
                            break;
                        case Consts.CASE2001.ZipCode:
                            if (enabled) { txtZipCode.Enabled = lblZipCode.Enabled = txtZipPlus.Enabled = btnZipCode.Enabled = true; if (required) lblZipCodeReq.Visible = true; } else { txtZipCode.Enabled = txtZipPlus.Enabled = lblZipCode.Enabled = btnZipCode.Enabled = false; lblZipCodeReq.Visible = false; }
                            break;
                        case Consts.CASE2001.Township:
                            if (enabled) { cmbTownship.Enabled = lblTownship.Enabled = true; if (required) lblTownShipReq.Visible = true; } else { cmbTownship.Enabled = lblTownship.Enabled = false; lblTownShipReq.Visible = false; }
                            break;
                        case Consts.CASE2001.County:
                            if (enabled) { cmbCounty.Enabled = lblCounty.Enabled = true; if (required) lblCountyReq.Visible = true; } else { cmbCounty.Enabled = lblCounty.Enabled = false; lblCountyReq.Visible = false; }
                            break;
                        case Consts.CASE2001.State:
                            if (enabled) { txtState.Enabled = lblState.Enabled = true; if (required) lblStateReq.Visible = true; } else { txtState.Enabled = lblState.Enabled = false; lblStateReq.Visible = false; }
                            break;
                        case Consts.CASE2001.NoOfYearsAtThisAddress:
                            if (enabled) { txtNoOfYearsAtAddress.Enabled = lblNoOfYearAtAddress.Enabled = true; if (required) lblNoOfYearAtAddressReq.Visible = true; } else { txtNoOfYearsAtAddress.Enabled = lblNoOfYearAtAddress.Enabled = false; lblNoOfYearAtAddressReq.Visible = false; }
                            break;
                        case Consts.CASE2001.Active:
                            if (enabled) { cbActive.Enabled = true; if (required) lblMstActiveReq.Visible = true; } else { cbActive.Enabled = false; lblMstActiveReq.Visible = false; }
                            break;
                        case Consts.CASE2001.Secret:
                            if (enabled) { cbSecret.Enabled = true; if (required) lblSecretReq.Visible = true; } else { cbSecret.Enabled = false; lblSecretReq.Visible = false; }
                            break;
                        case Consts.CASE2001.InitialDate:
                            if (enabled) { dtpInitialDate.Enabled = lblInitialDate.Enabled = true; if (required) lblInitialDateReq.Visible = true; } else { dtpInitialDate.Enabled = lblInitialDate.Enabled = false; lblInitialDateReq.Visible = false; }
                            break;
                        case Consts.CASE2001.IntakeDate:
                            if (enabled) { dtpIntakeDate.Enabled = lblIntakeDate.Enabled = true; if (required) lblIntakeDateReq.Visible = true; } else { dtpIntakeDate.Enabled = lblIntakeDate.Enabled = false; lblIntakeDateReq.Visible = false; }
                            break;
                        case Consts.CASE2001.CaseReviewDate:
                            if (enabled) { dtpCaseReview.Enabled = lblCaseReview.Enabled = true; if (required) lblCaseReviewReq.Visible = true; } else { dtpCaseReview.Enabled = lblCaseReview.Enabled = false; lblCaseReviewReq.Visible = false; }
                            break;
                        case Consts.CASE2001.HousingSituation:
                            if (enabled) { cmbHousingSituation.Enabled = lblHousing.Enabled = true; if (required) lblHousingSituationReq.Visible = true; } else { cmbHousingSituation.Enabled = lblHousing.Enabled = false; lblHousingSituationReq.Visible = false; }
                            break;
                        case Consts.CASE2001.PrimaryLanguage:
                            if (enabled) { cmbPrimaryLang.Enabled = lblPrimaryLang.Enabled = true; if (required) lblPrimaryLangReq.Visible = true; } else { cmbPrimaryLang.Enabled = lblPrimaryLang.Enabled = false; lblPrimaryLangReq.Visible = false; }
                            break;
                        case Consts.CASE2001.SecondaryLanguage:
                            if (enabled) { cmbSecondLang.Enabled = lblSecondLang.Enabled = true; if (required) lblSecondLangReq.Visible = true; } else { cmbSecondLang.Enabled = lblSecondLang.Enabled = false; lblSecondLangReq.Visible = false; }
                            break;
                        case Consts.CASE2001.FamilyType:
                            if (enabled) { cmbFamilyType.Enabled = lblFamilyType.Enabled = true; if (required) lblFamilyTypeReq.Visible = true; } else { cmbFamilyType.Enabled = lblFamilyType.Enabled = false; lblFamilyTypeReq.Visible = false; }
                            break;
                        case Consts.CASE2001.WaitingList:
                            if (enabled) { cmbWaitingList.Enabled = lblWaitingList.Enabled = true; if (required) lblWaitingListReq.Visible = true; } else { cmbWaitingList.Enabled = lblWaitingList.Enabled = false; lblWaitingListReq.Visible = false; }
                            break;
                        case Consts.CASE2001.Site:
                            if (enabled) { txtSite.Enabled = lblSite.Enabled = btnSite.Enabled = true; if (required) lblSiteReq.Visible = true; } else { txtSite.Enabled = lblSite.Enabled = btnSite.Enabled = false; lblSiteReq.Visible = false; }
                            break;
                        //case Consts.CASE2001.Phone:
                        //    if (enabled) { txtHomePhone.Enabled = lblHomePhone.Enabled = true; if (required) lblHomePhoneReq.Visible = true; } else { txtHomePhone.Enabled = lblHomePhone.Enabled = false; lblHomePhoneReq.Visible = false; }
                        //    break;
                        //case Consts.CASE2001.Message:
                        //    if (enabled) { txtMessage.Enabled = lblMessage.Enabled = true; if (required) lblMessageReq.Visible = true; } else { txtMessage.Enabled = txtMessage.Enabled = false; lblMessageReq.Visible = false; }
                        //    break;
                        case Consts.CASE2001.Phone:
                            if (enabled) { txtHomePhone.Enabled = lblHomePhone.Enabled = ChkHome_Na.Enabled = true; if (required) lblHomePhoneReq.Visible = true; } else { txtHomePhone.Enabled = ChkHome_Na.Enabled = lblHomePhone.Enabled = false; lblHomePhoneReq.Visible = false; }
                            break;
                        case Consts.CASE2001.Message:
                            if (enabled) { txtMessage.Enabled = lblMessage.Enabled = chkMessage_Na.Enabled = true; if (required) lblMessageReq.Visible = true; } else { txtMessage.Enabled = txtMessage.Enabled = chkMessage_Na.Enabled = false; lblMessageReq.Visible = false; }
                            break;
                        case Consts.CASE2001.TTY:
                            if (enabled) { txtTTY.Enabled = lblTTY.Enabled = true; if (required) lblTTYReq.Visible = true; } else { txtTTY.Enabled = lblTTY.Enabled = false; lblTTYReq.Visible = false; }
                            break;
                        //case Consts.CASE2001.Cell:
                        //    if (enabled) { txtCell.Enabled = lblCell.Enabled = true; if (required) lblCellReq.Visible = true; } else { txtCell.Enabled = lblCell.Enabled = false; lblCellReq.Visible = false; }
                        //    break;
                        case Consts.CASE2001.Cell:
                            if (enabled) { txtCell.Enabled = lblCell.Enabled = chk_CellNa.Enabled = true; if (required) lblCellReq.Visible = true; } else { txtCell.Enabled = lblCell.Enabled = chk_CellNa.Enabled = false; lblCellReq.Visible = false; }
                            break;
                        case Consts.CASE2001.Fax:
                            if (enabled) { txtFax.Enabled = lblFax.Enabled = true; if (required) lblFaxReq.Visible = true; } else { txtFax.Enabled = lblFax.Enabled = false; lblFaxReq.Visible = false; }
                            break;
                        case Consts.CASE2001.Email:
                            if (enabled) { txtEmail.Enabled = lblEmail.Enabled = chkEmail_Na.Enabled = true; if (required) { lblEmailReq.Visible = true; } } else { txtEmail.Enabled = chkEmail_Na.Enabled = lblEmail.Enabled = false; lblEmailReq.Visible = false; }
                            break;
                        case Consts.CASE2001.WhatIsTheBestWayToContact:
                            if (enabled) { cmbContact.Enabled = lblWhatIsTheBestWay.Enabled = true; if (required) lblWhatIsTheBestWayReq.Visible = true; } else { cmbContact.Enabled = false; lblWhatIsTheBestWayReq.Visible = false; }
                            break;
                        case Consts.CASE2001.HowDidYouFindOutAboutUs:
                            if (enabled) { cmbAboutUs.Enabled = lblHowDidYouFindOutAboutUs.Enabled = true; if (required) lblHowDidYouFindOutAboutUsReq.Visible = true; } else { cmbAboutUs.Enabled = lblHowDidYouFindOutAboutUs.Enabled = false; lblHowDidYouFindOutAboutUsReq.Visible = false; }
                            break;
                        case Consts.CASE2001.WhatServicesAreYouInquiringAbout:
                            if (enabled) { gvwServices.Enabled = cmbServicesInquired.Enabled = true; if (required) lblServiceReq.Visible = true; } else { gvwServices.Enabled = false; cmbServicesInquired.Enabled = false; lblServiceReq.Visible = false; }
                            break;
                        case Consts.CASE2001.RecentMortage:
                            if (enabled) { txtRent.Enabled = lblRentMortgage.Enabled = true; if (required) lblRentMortgageReq.Visible = true; } else { txtRent.Enabled = lblRentMortgage.Enabled = false; lblRentMortgageReq.Visible = false; }
                            break;
                        case Consts.CASE2001.WaterSewer:
                            if (enabled) { txtWater.Enabled = lblWaterSewer.Enabled = true; if (required) lblWaterSewerReq.Visible = true; } else { txtWater.Enabled = lblWaterSewer.Enabled = false; lblWaterSewerReq.Visible = false; }
                            break;
                        case Consts.CASE2001.Electric:
                            if (enabled) { txtElectric.Enabled = lblElectric.Enabled = true; if (required) lblElectricReq.Visible = true; } else { txtElectric.Enabled = lblElectric.Enabled = false; lblElectricReq.Visible = false; }
                            break;
                        case Consts.CASE2001.Heating:
                            if (enabled) { txtHeating.Enabled = lblHeating.Enabled = true; if (required) lblHeatingReq.Visible = true; } else { txtHeating.Enabled = lblHeating.Enabled = false; lblHeatingReq.Visible = false; }
                            break;
                        case Consts.CASE2001.CreditCardDebit:
                            if (enabled) { txtExpand1.Enabled = lblExpand1.Enabled = true; if (required) lblExpand1Req.Visible = true; } else { txtExpand1.Enabled = lblExpand1.Enabled = false; lblExpand1Req.Visible = false; }
                            break;
                        case Consts.CASE2001.LoansDebt:
                            if (enabled) { txtExpand2.Enabled = lblExpand2.Enabled = true; if (required) lblExpand2Req.Visible = true; } else { txtExpand2.Enabled = lblExpand2.Enabled = false; lblExpand2Req.Visible = false; }
                            break;
                        case Consts.CASE2001.MedicalDebit:
                            if (enabled) { txtExpand3.Enabled = lblExpand3.Enabled = true; if (required) lblExpand3Req.Visible = true; } else { txtExpand3.Enabled = lblExpand3.Enabled = false; lblExpand3Req.Visible = false; }
                            break;
                        case Consts.CASE2001.OtherDebit:
                            if (enabled) { txtExpand4.Enabled = lblExpand4.Enabled = true; if (required) lblExpand4Req.Visible = true; } else { txtExpand4.Enabled = lblExpand4.Enabled = false; lblExpand4Req.Visible = false; }
                            break;
                        case Consts.CASE2001.VerifiedthatallHouseholdExpensesentered:
                            if (enabled) { cmbVerifiedStaff.Enabled = lblverifiedallhousldReq.Enabled = true; if (required) lblverifiedallhousldReq.Visible = true; } else { cmbVerifiedStaff.Enabled = false; lblverifiedallhousldReq.Visible = false; }
                            break;
                        case Consts.CASE2001.Dwelling:
                            if (enabled) { cmbDwelling.Enabled = lblDwellingType.Enabled = true; if (required) lblDwellingReq.Visible = true; } else { cmbDwelling.Enabled = false; lblDwellingType.Enabled = false; lblDwellingReq.Visible = false; }
                            break;
                        case Consts.CASE2001.Primaryopfh:
                            if (enabled) { cmbPMOPfHeat.Enabled = lblPMOPforHeat.Enabled = true; if (required) lblPMopfHeatReq.Visible = true; } else { cmbPMOPfHeat.Enabled = false; lblPMOPforHeat.Enabled = false; lblPMopfHeatReq.Visible = false; }
                            break;
                        case Consts.CASE2001.PrimarySourceHeat:
                            if (enabled) { cmbPrimarySourceoHeat.Enabled = lblPrimaySource.Enabled = true; if (required) lblPrimarySourcReq.Visible = true; } else { cmbPrimarySourceoHeat.Enabled = false; lblPrimaySource.Enabled = false; lblPrimarySourcReq.Visible = false; }
                            break;
                        case Consts.CASE2001.MiscExpenses:
                            if (enabled) { txtMiscExpenses.Enabled = lblMiscExpenses.Enabled = true; if (required) lblMiscExpensesReq.Visible = true; } else { txtMiscExpenses.Enabled = false; lblMiscExpenses.Enabled = false; lblMiscExpensesReq.Visible = false; }
                            break;
                        case Consts.CASE2001.MiscDebit:
                            if (enabled) { txtMiscDebt.Enabled = lblMiscDebt.Enabled = true; if (required) lblMiscDebtReq.Visible = true; } else { txtMiscDebt.Enabled = false; lblMiscDebt.Enabled = false; lblMiscDebtReq.Visible = false; }
                            break;
                        case Consts.CASE2001.PhysicalAssets:
                            if (enabled) { txtPhysicalAssets.Enabled = lblPhysicalAssets.Enabled = true; if (required) lblPhysicalAssetsReq.Visible = true; } else { txtPhysicalAssets.Enabled = false; lblPhysicalAssets.Enabled = false; lblPhysicalAssetsReq.Visible = false; }
                            break;
                        case Consts.CASE2001.LiquidAssets:
                            if (enabled) { txtLiquidAssets.Enabled = lblLiquidAssets.Enabled = true; if (required) lblLiquidAssetsReq.Visible = true; } else { txtLiquidAssets.Enabled = false; lblLiquidAssets.Enabled = false; lblLiquidAssetsReq.Visible = false; }
                            break;
                        case Consts.CASE2001.OtherAssets:
                            if (enabled) { txtOtherAssets.Enabled = lblOtherAssets.Enabled = true; if (required) lblOtherAssetsReq.Visible = true; } else { txtOtherAssets.Enabled = false; lblOtherAssets.Enabled = false; lblOtherAssetsReq.Visible = false; }
                            break;
                        case Consts.CASE2001.MiscAssets:
                            if (enabled) { txtMiscAssets.Enabled = lblMiscAssets.Enabled = true; if (required) lblMiscAssetsReq.Visible = true; } else { txtMiscAssets.Enabled = false; lblMiscAssets.Enabled = false; lblMiscAssetsReq.Visible = false; }
                            break;
                    }
                }
            }

            if (IsApplicant)
            {

                if (CntlEntity.Exists(u => u.Enab.Equals("Y")))
                {
                    foreach (FldcntlHieEntity entity in preassesCntlEntity)
                    { }
                }

            }
        }

        private bool isValidate()
        {
            bool isValid = true;
            bool isFirstTab = false;
            bool isSecondTab = false;
            bool isThirdTab = false;
            bool isFourthTab = false;
            bool isFifthTab = false;
            bool isCustomQues = true;

            if (ProgramDefinition.DepIntakeProg.Equals("Y") && ProgramDefinition.DepSsnAutoAssign != "0")
            { }
            else
            {
                if (ssnReq.Visible && String.IsNullOrEmpty(mskSSN.Text))
                {
                    _errorProvider.SetError(mskSSN, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSSN.Text));
                    isValid = false;
                }
                else
                {
                    string strssn = mskSSN.Text.Replace(" ", string.Empty);
                    if (strssn.Length < 9 && strssn.Length > 0)
                    {
                        _errorProvider.SetError(mskSSN, "Please enter ssno in correct format");
                        isValid = false;

                    }
                    else
                    {
                        _errorProvider.SetError(mskSSN, null);
                    }
                }
            }
            if (firstnameReq.Visible && String.IsNullOrEmpty(txtFirstName.Text.Trim()))
            {
                _errorProvider.SetError(txtFirstName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFirstName.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtFirstName, null);
            }

            if (LastNameReq.Visible && String.IsNullOrEmpty(txtLastName.Text.Trim()))
            {
                _errorProvider.SetError(txtLastName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblLastName.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtLastName, null);
            }

            if (lblMIReq.Visible && String.IsNullOrEmpty(txtMI.Text.Trim()))
            {
                _errorProvider.SetError(txtMI, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMI.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtMI, null);
            }
            if (lblReqAppSuffix.Visible && string.IsNullOrEmpty(txtAppSuffix.Text.Trim()))
            {
                _errorProvider.SetError(txtAppSuffix, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAppSuffix.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtAppSuffix, null);
            }

            if (lblSSNReasonReq.Visible && (((ListItem)cmbSSNReason.SelectedItem).Value.ToString().Equals("0")))
            {
                _errorProvider.SetError(cmbSSNReason, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSSNReason.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbSSNReason, null);
            }

            if (AliasReq.Visible && String.IsNullOrEmpty(txtAlias.Text))
            {
                _errorProvider.SetError(txtAlias, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAlias.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtAlias, null);
            }
            if (DOBReq.Visible && dtBirth.Checked == false)
            {
                _errorProvider.SetError(dtBirth, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDOB.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtBirth, null);

                if (dtBirth.Checked == true)
                {
                    if (dtBirth.Value > DateTime.Now)
                    {
                        _errorProvider.SetError(dtBirth, "Future date not allowed");
                        isValid = false;
                    }
                    if (dtBirth.Value.ToShortDateString() == DateTime.Now.ToShortDateString())
                    {
                        _errorProvider.SetError(dtBirth, "DOB should not be current date");
                        isValid = false;
                    }

                }

            }


            if (lblSnpActiveReq.Visible && ckActive.Checked == false)
            {
                _errorProvider.SetError(lblSnpActiveReq, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), ckActive.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(lblSnpActiveReq, null);
            }
            if (lblSnpExcludeMemberReq.Visible && ckExcludeMember.Checked == false)
            {
                _errorProvider.SetError(lblSnpExcludeMemberReq, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), ckExcludeMember.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(lblSnpExcludeMemberReq, null);
            }

            if (lblSnpExcludeMember2Req.Visible && chkExcludeHHMem.Checked == false)
            {
                _errorProvider.SetError(lblSnpExcludeMember2Req, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), chkExcludeHHMem.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(lblSnpExcludeMember2Req, null);
            }

            if (RelationReq.Visible && (cmbRelation.SelectedItem == null || ((ListItem)cmbRelation.SelectedItem).Text.Trim() == string.Empty))
            {
                _errorProvider.SetError(cmbRelation, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblRelation.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbRelation, null);
            }
            if (EthnicityReq.Visible && (cmbEthnicity.SelectedItem == null || ((ListItem)cmbEthnicity.SelectedItem).Text == Consts.Common.SelectOne))
            {
                _errorProvider.SetError(cmbEthnicity, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEthnicity.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbEthnicity, null);
            }
            if (RaceReq.Visible && (cmbRace.SelectedItem == null || ((ListItem)cmbRace.SelectedItem).Text == Consts.Common.SelectOne))
            {
                _errorProvider.SetError(cmbRace, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblRace.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbRace, null);
            }
            if (SchoolReq.Visible && (cmbSchool.SelectedItem == null || ((ListItem)cmbSchool.SelectedItem).Text == Consts.Common.SelectOne))
            {
                _errorProvider.SetError(cmbSchool, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSchool.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbSchool, null);
            }
            if (GenderReq.Visible && (cmbGender.SelectedItem == null || ((ListItem)cmbGender.SelectedItem).Text == Consts.Common.SelectOne))
            {
                _errorProvider.SetError(cmbGender, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblGender.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbGender, null);
            }
            if (PregnantReq.Visible && (cmbAreyoupregnant.SelectedItem == null || ((ListItem)cmbAreyoupregnant.SelectedItem).Text == Consts.Common.SelectOne || ((ListItem)cmbAreyoupregnant.SelectedItem).Text.ToString().Trim() == string.Empty))
            {
                _errorProvider.SetError(cmbAreyoupregnant, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAreYouPregnant.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbAreyoupregnant, null);
            }
            if (EducationReq.Visible && (cmbEducation.SelectedItem == null || ((ListItem)cmbEducation.SelectedItem).Text == Consts.Common.SelectOne || ((ListItem)cmbEducation.SelectedItem).Text.ToString().Trim() == string.Empty))
            {
                _errorProvider.SetError(cmbEducation, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEducation.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbEducation, null);
            }
            if (MaritalReq.Visible && (cmbMaritalStatus.SelectedItem == null || ((ListItem)cmbMaritalStatus.SelectedItem).Text == Consts.Common.SelectOne))
            {
                _errorProvider.SetError(cmbMaritalStatus, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMaritalStatus.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbMaritalStatus, null);
            }
            if (HealthReq.Visible && (cmbHealthInsurance.SelectedItem == null || ((ListItem)cmbHealthInsurance.SelectedItem).Text == Consts.Common.SelectOne))
            {
                _errorProvider.SetError(cmbHealthInsurance, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHealthInsurance.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbHealthInsurance, null);
            }
            if (VeteranReq.Visible && (cmbVeteranCode.SelectedItem == null || ((ListItem)cmbVeteranCode.SelectedItem).Text == Consts.Common.SelectOne))
            {
                _errorProvider.SetError(cmbVeteranCode, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblVeteranCode.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbVeteranCode, null);
            }
            if (FoodStampsReq.Visible && (cmbFoodStamps.SelectedItem == null || ((ListItem)cmbFoodStamps.SelectedItem).Text == Consts.Common.SelectOne))
            {
                _errorProvider.SetError(cmbFoodStamps, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFoodStamps.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbFoodStamps, null);
            }
            if (DisabledReq.Visible && (cmbDisabled.SelectedItem == null || ((ListItem)cmbDisabled.SelectedItem).Text == Consts.Common.SelectOne))
            {
                _errorProvider.SetError(cmbDisabled, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDisabled.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbDisabled, null);
            }
            if (FarmerReq.Visible && (cmbFarmer.SelectedItem == null || ((ListItem)cmbFarmer.SelectedItem).Text == Consts.Common.SelectOne))
            {
                _errorProvider.SetError(cmbFarmer, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFarmer.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbFarmer, null);
            }
            if (WICReq.Visible && (cmbWIC.SelectedItem == null || ((ListItem)cmbWIC.SelectedItem).Text == Consts.Common.SelectOne))
            {
                _errorProvider.SetError(cmbWIC, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblWIC.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbWIC, null);
            }
            if (ReliableReq.Visible && (cmbReliableTrans.SelectedItem == null || ((ListItem)cmbReliableTrans.SelectedItem).Text == Consts.Common.SelectOne))
            {
                _errorProvider.SetError(cmbReliableTrans, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblReliableTransport.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbReliableTrans, null);
            }
            if (DriverReq.Visible && (cmbDriving.SelectedItem == null || ((ListItem)cmbDriving.SelectedItem).Text == Consts.Common.SelectOne))
            {
                _errorProvider.SetError(cmbDriving, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDriverLicense.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbDriving, null);
            }
            if (ResidentReq.Visible && (cmbResident.SelectedItem == null || ((ListItem)cmbResident.SelectedItem).Text == Consts.Common.SelectOne))
            {
                _errorProvider.SetError(cmbResident, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResident.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbResident, null);
            }
            if (AlienRegNo.Visible && string.IsNullOrEmpty(txtAlienRegNo.Text.Trim()))
            {
                _errorProvider.SetError(txtAlienRegNo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAlienRegNo.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtAlienRegNo, null);
            }
            if (LegalToWork.Visible && (cmbLegaltowork.SelectedItem == null || ((ListItem)cmbLegaltowork.SelectedItem).Text == Consts.Common.SelectOne))
            {
                _errorProvider.SetError(cmbLegaltowork, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblLegalToWork.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbLegaltowork, null);
            }
            if (ExpirationDateReq.Visible && dtExpirationdate.Checked == false)
            {
                _errorProvider.SetError(dtExpirationdate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblExpirationDate.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtExpirationdate, null);
            }
            foreach (DataGridViewRow dataGridViewRow in customFieldsControl.GridViewControl.Rows)
            {

                if (dataGridViewRow.Cells["gvtRequire"].Value.ToString() == "*")
                {
                    string inputValue = string.Empty;
                    inputValue = dataGridViewRow.Cells["Response"].Value != null ? dataGridViewRow.Cells["Response"].Value.ToString() : string.Empty;
                    if (inputValue == string.Empty)
                    {
                        CommonFunctions.MessageBoxDisplay("Please answer the required Questions’");
                        isValid = false;
                        isCustomQues = false;
                        break;
                    }
                }
            }



            if (!isValid) isFirstTab = true;
            if (IsApplicant)
            {
                _errorProvider.SetError(btnLiheapques, null);
                if (lblIntakeDateReq.Visible && dtpIntakeDate.Checked == false)
                {
                    _errorProvider.SetError(dtpIntakeDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblIntakeDate.Text));
                    isValid = false;
                }
                else if (ProgramDefinition != null)
                {
                    if (ProgramDefinition.IntakeEdit.Equals("1"))
                    {
                        if (dtpIntakeDate.Checked == true)
                        {
                            if (DateTime.Parse(dtpIntakeDate.Text.ToString()) >= DateTime.Parse(ProgramDefinition.IntakeFDate) && DateTime.Parse(dtpIntakeDate.Text.ToString()) <= DateTime.Parse(ProgramDefinition.IntakeTDate))
                            {
                                _errorProvider.SetError(dtpIntakeDate, null);
                            }
                            else
                            {
                                _errorProvider.SetError(dtpIntakeDate, "should be between " + LookupDataAccess.Getdate(ProgramDefinition.IntakeFDate) + " to " + LookupDataAccess.Getdate(ProgramDefinition.IntakeTDate) + " dates");
                                isValid = false;
                            }
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(dtpIntakeDate, null);
                    }
                }
                else
                {
                    _errorProvider.SetError(dtpIntakeDate, null);
                }

                if (lblAssignedWorkerReq.Visible && (cmbStaff.SelectedItem == null || ((ListItem)cmbStaff.SelectedItem).Text == Consts.Common.SelectOne || ((ListItem)cmbStaff.SelectedItem).Text == string.Empty))
                {
                    _errorProvider.SetError(cmbStaff, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAssignedWorker.Text));
                    isValid = false;
                }
                //Added by Sudheer on 12/05/2020
                else if (((ListItem)cmbStaff.SelectedItem).ID.ToString() == "Y")
                {
                    _errorProvider.SetError(cmbStaff, string.Format("Inactive ", lblAssignedWorker.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbStaff, null);
                }
                if (lblHNReq.Visible && string.IsNullOrEmpty(txtHN.Text.Trim()))
                {
                    _errorProvider.SetError(txtHN, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHN.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtHN, null);
                }
                if (lblStreetReq.Visible && string.IsNullOrEmpty(txtStreet.Text.Trim()))
                {
                    _errorProvider.SetError(txtStreet, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblStreet.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtStreet, null);
                }
                if (lblSuffixReq.Visible && string.IsNullOrEmpty(txtSuffix.Text.Trim()))
                {
                    _errorProvider.SetError(txtSuffix, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSuffix.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtSuffix, null);
                }
                if (lblFloorReq.Visible && string.IsNullOrEmpty(txtFloor.Text.Trim()))
                {
                    _errorProvider.SetError(txtFloor, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFloor.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtFloor, null);
                }
                if (lblDirectionReq.Visible && string.IsNullOrEmpty(txtDirection.Text.Trim()))
                {
                    _errorProvider.SetError(txtDirection, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDirection.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtDirection, null);
                }
                if (lblApartmentReq.Visible && string.IsNullOrEmpty(txtApartment.Text.Trim()))
                {
                    _errorProvider.SetError(txtApartment, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblApartment.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtApartment, null);
                }
                if (lblPrecinctReq.Visible && string.IsNullOrEmpty(txtPrecinct.Text.Trim()))
                {
                    if (((ListItem)cmbOutService.SelectedItem).Value.ToString() != "I" && cmbOutService.Enabled == true)
                    {
                        if (((ListItem)cmbOutService.SelectedItem).Value.ToString() == "0")
                        {
                            _errorProvider.SetError(txtPrecinct, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPrecinct.Text));
                            isValid = false;
                        }
                        else
                        {
                            //lblPrecinctReq.Visible = false;
                            _errorProvider.SetError(txtPrecinct, null);
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(txtPrecinct, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPrecinct.Text));
                        isValid = false;
                    }
                }
                else
                {
                    _errorProvider.SetError(txtPrecinct, null);
                }
                if (lblCaseTypeReq.Visible && (cmbCaseType.SelectedItem == null || ((ListItem)cmbCaseType.SelectedItem).Text == Consts.Common.SelectOne || ((ListItem)cmbCaseType.SelectedItem).Text == string.Empty))
                {
                    _errorProvider.SetError(cmbCaseType, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCaseType.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbCaseType, null);
                }
                if (lblTownShipReq.Visible && (cmbTownship.SelectedItem == null || ((ListItem)cmbTownship.SelectedItem).Text == Consts.Common.SelectOne || ((ListItem)cmbTownship.SelectedItem).Text == string.Empty))
                {
                    _errorProvider.SetError(cmbTownship, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblTownship.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbTownship, null);
                }
                if (lblCountyReq.Visible && (cmbCounty.SelectedItem == null || ((ListItem)cmbCounty.SelectedItem).Text == Consts.Common.SelectOne))
                {
                    _errorProvider.SetError(cmbCounty, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCounty.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbCounty, null);
                }
                if (lblZipCodeReq.Visible && string.IsNullOrEmpty(txtZipCode.Text))
                {
                    _errorProvider.SetError(txtZipCode, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblZipCode.Text));
                    isValid = false;
                }
                else if (lblZipCodeReq.Visible && Convert.ToDouble(txtZipCode.Text) <= 0)
                {
                    _errorProvider.SetError(txtZipCode, Consts.Messages.Greaterthanzzero);
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtZipCode, null);
                }
                if (lblStateReq.Visible && string.IsNullOrEmpty(txtState.Text.Trim()))
                {
                    _errorProvider.SetError(txtState, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblState.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtState, null);
                }
                if (lblNoOfYearAtAddressReq.Visible && string.IsNullOrEmpty(txtNoOfYearsAtAddress.Text.Trim()))
                {
                    _errorProvider.SetError(txtNoOfYearsAtAddress, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblNoOfYearAtAddress.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtNoOfYearsAtAddress, null);
                }
                if (lblInitialDateReq.Visible && dtpInitialDate.Checked == false)
                {
                    _errorProvider.SetError(dtpInitialDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblInitialDate.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpInitialDate, null);
                }

                if (lblCaseReviewReq.Visible && dtpCaseReview.Checked == false)
                {
                    _errorProvider.SetError(dtpCaseReview, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCaseReview.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpCaseReview, null);
                }
                if (lblSiteReq.Visible && string.IsNullOrEmpty(txtSite.Text.Trim()))
                {
                    _errorProvider.SetError(btnSite, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSite.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(btnSite, null);
                }
                //if (lblHousingSituationReq.Visible && (cmbHousingSituation.SelectedItem == null || ((ListItem)cmbHousingSituation.SelectedItem).Text == Consts.Common.SelectOne))
                //{
                //    _errorProvider.SetError(cmbHousingSituation, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHousing.Text));
                //    isValid = false;
                //}
                //else
                //{
                //    _errorProvider.SetError(cmbHousingSituation, null);
                //}
                if (lblPrimaryLangReq.Visible && (cmbPrimaryLang.SelectedItem == null || ((ListItem)cmbPrimaryLang.SelectedItem).Text == Consts.Common.SelectOne))
                {
                    _errorProvider.SetError(cmbPrimaryLang, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPrimaryLang.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbPrimaryLang, null);
                }
                if (lblSecondLangReq.Visible && (cmbSecondLang.SelectedItem == null || ((ListItem)cmbSecondLang.SelectedItem).Text == Consts.Common.SelectOne))
                {
                    _errorProvider.SetError(cmbSecondLang, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSecondLang.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbSecondLang, null);
                }
                if (lblFamilyTypeReq.Visible && (cmbFamilyType.SelectedItem == null || ((ListItem)cmbFamilyType.SelectedItem).Text == Consts.Common.SelectOne))
                {
                    _errorProvider.SetError(cmbFamilyType, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFamilyType.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbFamilyType, null);
                }
                if (lblWaitingListReq.Visible && (cmbWaitingList.SelectedItem == null || ((ListItem)cmbWaitingList.SelectedItem).Text == Consts.Common.SelectOne))
                {
                    _errorProvider.SetError(cmbWaitingList, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblWaitingList.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbWaitingList, null);
                }
                if (lblWhatIsTheBestWayReq.Visible && (cmbContact.SelectedItem == null || ((ListItem)cmbContact.SelectedItem).Text == Consts.Common.SelectOne))
                {
                    _errorProvider.SetError(cmbContact, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblWhatIsTheBestWay.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbContact, null);
                }
                if (lblHowDidYouFindOutAboutUsReq.Visible && (cmbAboutUs.SelectedItem == null || ((ListItem)cmbAboutUs.SelectedItem).Text == Consts.Common.SelectOne))
                {
                    _errorProvider.SetError(cmbAboutUs, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHowDidYouFindOutAboutUs.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbAboutUs, null);
                }
                if (lblHomePhoneReq.Visible && string.IsNullOrEmpty(txtHomePhone.Text.Trim()))
                {
                    _errorProvider.SetError(txtHomePhone, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHomePhone.Text));
                    isValid = false;
                }
                else
                {
                    string strhome = txtHomePhone.Text.Replace(" ", string.Empty);
                    if (strhome.Length < 10 && strhome.Length > 0)
                    {
                        _errorProvider.SetError(txtHomePhone, "Please enter Home phone in correct format");
                        isValid = false;

                    }
                    else
                    {
                        _errorProvider.SetError(txtHomePhone, null);
                    }

                }
                if (lblMessageReq.Visible && string.IsNullOrEmpty(txtMessage.Text.Trim()))
                {
                    _errorProvider.SetError(txtMessage, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMessage.Text));
                    isValid = false;
                }
                else
                {
                    string strhome = txtMessage.Text.Replace(" ", string.Empty);
                    if (strhome.Length < 10 && strhome.Length > 0)
                    {
                        _errorProvider.SetError(txtMessage, "Please enter Message in correct format");
                        isValid = false;

                    }
                    else
                    {
                        _errorProvider.SetError(txtMessage, null);
                    }
                }
                if (lblTTYReq.Visible && string.IsNullOrEmpty(txtTTY.Text.Trim()))
                {
                    _errorProvider.SetError(txtTTY, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblTTY.Text));
                    isValid = false;
                }
                else
                {
                    string strhome = txtTTY.Text.Replace(" ", string.Empty);
                    if (strhome.Length < 10 && strhome.Length > 0)
                    {
                        _errorProvider.SetError(txtTTY, "Please enter TTY in correct format");
                        isValid = false;

                    }
                    else
                    {
                        _errorProvider.SetError(txtTTY, null);
                    }

                }
                if (lblCellReq.Visible && string.IsNullOrEmpty(txtCell.Text.Trim()))
                {
                    _errorProvider.SetError(txtCell, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCell.Text));
                    isValid = false;
                }
                else
                {
                    string strhome = txtCell.Text.Replace(" ", string.Empty);
                    if (strhome.Length < 10 && strhome.Length > 0)
                    {
                        _errorProvider.SetError(txtCell, "Please enter Cell in correct format");
                        isValid = false;

                    }
                    else
                    {
                        _errorProvider.SetError(txtCell, null);
                    }

                }
                if (lblFaxReq.Visible && string.IsNullOrEmpty(txtFax.Text.Trim()))
                {
                    _errorProvider.SetError(txtFax, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFax.Text));
                    isValid = false;
                }
                else
                {
                    string strhome = txtFax.Text.Replace(" ", string.Empty);
                    if (strhome.Length < 10 && strhome.Length > 0)
                    {
                        _errorProvider.SetError(txtFax, "Please enter Fax in correct format");
                        isValid = false;

                    }
                    else
                    {
                        _errorProvider.SetError(txtFax, null);
                    }
                }
                if (lblCityReq.Visible && string.IsNullOrEmpty(txtCity.Text.Trim()))
                {
                    _errorProvider.SetError(txtCity, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCityName.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtCity, null);
                }
                if (lblEmailReq.Visible && string.IsNullOrEmpty(txtEmail.Text.Trim()))
                {
                    _errorProvider.SetError(txtEmail, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEmail.Text));
                    isValid = false;
                }
                else
                {
                    if (txtEmail.Text.Trim().Length > 0)
                    {
                        if (txtEmail.Text.Split('@').Length == 2)
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, Consts.StaticVars.EmailValidatingString))
                            {
                                _errorProvider.SetError(txtEmail, Consts.Messages.PleaseEnterEmail);
                                isValid = false;
                            }
                            else
                            {
                                _errorProvider.SetError(txtEmail, null);
                            }
                        }
                        else
                        {
                            _errorProvider.SetError(txtEmail, Consts.Messages.PleaseEnterEmail);
                            isValid = false;
                        }
                    }

                }

                if (Mode.Equals(Consts.Common.Edit))
                {
                    CaseDiffEntity caseDiffDetails = _model.CaseMstData.GetCaseDiffadpya(BaseForm.BaseAgency.ToString(), BaseForm.BaseDept.ToString(), BaseForm.BaseProg.ToString(), BaseForm.BaseYear.ToString(), BaseForm.BaseApplicationNo.ToString(), string.Empty);
                    propCaseDiffMailDetails = caseDiffDetails;
                    if (!MailAddressRequirevalidation())
                    {
                        if (BaseForm.BaseAgencyControlDetails.MailAddressSwitch.ToUpper() == "Y")
                        {
                            _errorProvider.SetError(btnMailingAddress2, "Mailing Address Information is not complete");
                            isValid = false;
                        }
                        else
                        {
                            _errorProvider.SetError(btnMailingAddress, "Mailing Address Information is not complete");
                            isValid = false;
                        }
                    }
                }
                else
                {

                    if (!MailAddressRequirevalidation())
                    {
                        if (BaseForm.BaseAgencyControlDetails.MailAddressSwitch.ToUpper() == "Y")
                        {
                            _errorProvider.SetError(btnMailingAddress2, "Mailing Address Information is not complete");
                            isValid = false;
                        }
                        else
                        {
                            _errorProvider.SetError(btnMailingAddress, "Mailing Address Information is not complete");
                            isValid = false;
                        }
                    }
                }


                if (lblMstActiveReq.Visible && cbActive.Checked == false)
                {
                    _errorProvider.SetError(lblMstActiveReq, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), cbActive.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(lblMstActiveReq, null);
                }

                if (lblSecretReq.Visible && cbSecret.Checked == false)
                {
                    _errorProvider.SetError(lblSecretReq, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), cbSecret.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(lblSecretReq, null);
                }
                if (BaseForm.BusinessModuleID == "08")
                {
                    if (lblreqApplicationType.Visible && (cmbEnergydata.SelectedItem == null || ((ListItem)cmbEnergydata.SelectedItem).Text == Consts.Common.SelectOne))
                    {
                        _errorProvider.SetError(cmbEnergydata, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblApplicationtype.Text));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(cmbEnergydata, null);
                    }
                    if (lblreqApplicationType.Visible && dtApplicationType.Checked == false)
                    {
                        _errorProvider.SetError(dtApplicationType, "Please select Application Type Date");
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtApplicationType, null);
                    }

                }
                if (isCustomQues)
                {
                    foreach (DataGridViewRow dataGridViewRow in customFieldsIntakeControl.GridViewControl.Rows)
                    {

                        if (dataGridViewRow.Cells["gvtRequire"].Value.ToString() == "*")
                        {
                            string inputValue = string.Empty;
                            inputValue = dataGridViewRow.Cells["Response"].Value != null ? dataGridViewRow.Cells["Response"].Value.ToString() : string.Empty;
                            if (inputValue == string.Empty)
                            {
                                CommonFunctions.MessageBoxDisplay("Please complete required custom questions.");
                                isValid = false;
                                break;
                            }
                        }
                    }
                }


                if (!isValid) isSecondTab = true;

                //if (Mode.Equals("Edit"))
                //{
                if (lblServiceReq.Visible)
                {
                    if (gvwServices.Rows.Count > 0)
                    {
                        bool boolservice = true;
                        foreach (DataGridViewRow drrows in gvwServices.Rows)
                        {
                            if (drrows.Cells["Servicechk"].Value.ToString().ToUpper() == "TRUE")
                            {
                                boolservice = false;
                                break;
                            }
                        }
                        if (boolservice)
                        {
                            _errorProvider.SetError(gvwServices, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Services"));
                            isValid = false;

                        }
                        else
                        {
                            _errorProvider.SetError(gvwServices, null);
                        }
                    }
                }
                else
                {
                    _errorProvider.SetError(gvwServices, null);
                }

                if (lblverifiedallhousldReq.Visible && (cmbVerifiedStaff.SelectedItem == null || ((ListItem)cmbVerifiedStaff.SelectedItem).Text == Consts.Common.SelectOne || ((ListItem)cmbVerifiedStaff.SelectedItem).Text == string.Empty))
                {
                    _errorProvider.SetError(cmbVerifiedStaff, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblVerifiedthatallhousehold.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbVerifiedStaff, null);
                }

                if (lblRentMortgageReq.Visible && string.IsNullOrEmpty(txtRent.Text))
                {
                    _errorProvider.SetError(txtRent, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblRentMortgage.Text));
                    isValid = false;
                }

                else
                {
                    //if (lblRentMortgageReq.Visible)
                    //{
                    //    if (Convert.ToDouble(txtRent.Text) > 0.00)
                    //    {
                    //        _errorProvider.SetError(txtRent, null);
                    //    }
                    //    else
                    //    {
                    //        _errorProvider.SetError(txtRent, Consts.Messages.Greaterthanzzero);
                    //        isValid = false;
                    //    }
                    //}
                    //else
                    //{
                    _errorProvider.SetError(txtRent, null);
                    //}
                }
                if (lblWaterSewerReq.Visible && string.IsNullOrEmpty(txtWater.Text))
                {
                    _errorProvider.SetError(txtWater, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblWaterSewer.Text));
                    isValid = false;
                }
                else
                {
                    if (lblWaterSewerReq.Visible)
                    {
                        if (Convert.ToDouble(txtWater.Text) > 0.00)
                        {
                            _errorProvider.SetError(txtWater, null);
                        }
                        else
                        {
                            _errorProvider.SetError(txtWater, Consts.Messages.Greaterthanzzero);
                            isValid = false;
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(txtWater, null);
                    }
                }
                if (lblElectricReq.Visible && string.IsNullOrEmpty(txtElectric.Text))
                {
                    _errorProvider.SetError(txtElectric, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblElectric.Text));
                    isValid = false;
                }
                else
                {
                    if (lblElectricReq.Visible)
                    {
                        if (Convert.ToDouble(txtElectric.Text) > 0.00)
                        {
                            _errorProvider.SetError(txtElectric, null);
                        }
                        else
                        {
                            _errorProvider.SetError(txtElectric, Consts.Messages.Greaterthanzzero);
                            isValid = false;
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(txtElectric, null);
                    }
                }
                if (lblHeatingReq.Visible && string.IsNullOrEmpty(txtHeating.Text))
                {
                    _errorProvider.SetError(txtHeating, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHeating.Text));
                    isValid = false;
                }
                else
                {
                    if (lblHeatingReq.Visible)
                    {
                        if (Convert.ToDouble(txtHeating.Text) > 0.00)
                        {
                            _errorProvider.SetError(txtHeating, null);
                        }
                        else
                        {
                            _errorProvider.SetError(txtHeating, Consts.Messages.Greaterthanzzero);
                            isValid = false;
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(txtHeating, null);
                    }
                }
                if (lblExpand1Req.Visible && string.IsNullOrEmpty(txtExpand1.Text))
                {
                    _errorProvider.SetError(txtExpand1, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblExpand1.Text));
                    isValid = false;
                }
                else
                {
                    if (lblExpand1Req.Visible)
                    {
                        if (Convert.ToDouble(txtExpand1.Text) > 0.00)
                        {
                            _errorProvider.SetError(txtExpand1, null);
                        }
                        else
                        {
                            _errorProvider.SetError(txtExpand1, Consts.Messages.Greaterthanzzero);
                            isValid = false;
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(txtExpand1, null);
                    }
                }
                if (lblExpand2Req.Visible && string.IsNullOrEmpty(txtExpand2.Text))
                {
                    _errorProvider.SetError(txtExpand2, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblExpand2.Text));
                    isValid = false;
                }
                else
                {
                    if (lblExpand2Req.Visible)
                    {
                        if (Convert.ToDouble(txtExpand2.Text) > 0.00)
                        {
                            _errorProvider.SetError(txtExpand2, null);
                        }
                        else
                        {
                            _errorProvider.SetError(txtExpand2, Consts.Messages.Greaterthanzzero);
                            isValid = false;
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(txtExpand2, null);
                    }
                }
                if (lblExpand3Req.Visible && string.IsNullOrEmpty(txtExpand3.Text))
                {
                    _errorProvider.SetError(txtExpand3, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblExpand3.Text));
                    isValid = false;
                }
                else
                {
                    if (lblExpand3Req.Visible)
                    {
                        if (Convert.ToDouble(txtExpand3.Text) > 0.00)
                        {
                            _errorProvider.SetError(txtExpand3, null);
                        }
                        else
                        {
                            _errorProvider.SetError(txtExpand3, Consts.Messages.Greaterthanzzero);
                            isValid = false;
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(txtExpand3, null);
                    }
                }
                if (lblExpand4Req.Visible && string.IsNullOrEmpty(txtExpand4.Text))
                {
                    _errorProvider.SetError(txtExpand4, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblExpand4.Text));
                    isValid = false;
                }
                else
                {
                    if (lblExpand4Req.Visible)
                    {
                        if (Convert.ToDouble(txtExpand4.Text) > 0.00)
                        {
                            _errorProvider.SetError(txtExpand4, null);
                        }
                        else
                        {
                            _errorProvider.SetError(txtExpand4, Consts.Messages.Greaterthanzzero);
                            isValid = false;
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(txtExpand4, null);
                    }
                }

                if (lblMiscDebtReq.Visible && string.IsNullOrEmpty(txtMiscDebt.Text))
                {
                    _errorProvider.SetError(txtMiscDebt, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMiscDebt.Text));
                    isValid = false;
                }
                else
                {
                    if (lblMiscDebtReq.Visible)
                    {
                        if (Convert.ToDouble(txtMiscDebt.Text) > 0.00)
                        {
                            _errorProvider.SetError(txtMiscDebt, null);
                        }
                        else
                        {
                            _errorProvider.SetError(txtMiscDebt, Consts.Messages.Greaterthanzzero);
                            isValid = false;
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(txtMiscDebt, null);
                    }
                }

                if (lblMiscExpensesReq.Visible && string.IsNullOrEmpty(txtMiscExpenses.Text))
                {
                    _errorProvider.SetError(txtMiscExpenses, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMiscExpenses.Text));
                    isValid = false;
                }
                else
                {
                    if (lblMiscExpensesReq.Visible)
                    {
                        if (Convert.ToDouble(txtMiscExpenses.Text) > 0.00)
                        {
                            _errorProvider.SetError(txtMiscExpenses, null);
                        }
                        else
                        {
                            _errorProvider.SetError(txtMiscExpenses, Consts.Messages.Greaterthanzzero);
                            isValid = false;
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(txtMiscExpenses, null);
                    }
                }


                if (lblPhysicalAssetsReq.Visible && string.IsNullOrEmpty(txtPhysicalAssets.Text))
                {
                    _errorProvider.SetError(txtPhysicalAssets, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPhysicalAssets.Text));
                    isValid = false;
                }
                else
                {
                    if (lblPhysicalAssetsReq.Visible)
                    {
                        if (Convert.ToDouble(txtPhysicalAssets.Text) > 0.00)
                        {
                            _errorProvider.SetError(txtPhysicalAssets, null);
                        }
                        else
                        {
                            _errorProvider.SetError(txtPhysicalAssets, Consts.Messages.Greaterthanzzero);
                            isValid = false;
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(txtPhysicalAssets, null);
                    }
                }

                if (lblLiquidAssetsReq.Visible && string.IsNullOrEmpty(txtLiquidAssets.Text))
                {
                    _errorProvider.SetError(txtLiquidAssets, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblLiquidAssets.Text));
                    isValid = false;
                }
                else
                {
                    if (lblLiquidAssetsReq.Visible)
                    {
                        if (Convert.ToDouble(txtLiquidAssets.Text) > 0.00)
                        {
                            _errorProvider.SetError(txtLiquidAssets, null);
                        }
                        else
                        {
                            _errorProvider.SetError(txtLiquidAssets, Consts.Messages.Greaterthanzzero);
                            isValid = false;
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(txtLiquidAssets, null);
                    }
                }

                if (lblOtherAssetsReq.Visible && string.IsNullOrEmpty(txtOtherAssets.Text))
                {
                    _errorProvider.SetError(txtOtherAssets, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblOtherAssets.Text));
                    isValid = false;
                }
                else
                {
                    if (lblOtherAssetsReq.Visible)
                    {
                        if (Convert.ToDouble(txtOtherAssets.Text) > 0.00)
                        {
                            _errorProvider.SetError(txtOtherAssets, null);
                        }
                        else
                        {
                            _errorProvider.SetError(txtOtherAssets, Consts.Messages.Greaterthanzzero);
                            isValid = false;
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(txtOtherAssets, null);
                    }
                }

                if (lblTotalAssetsReq.Visible && string.IsNullOrEmpty(txtTotalAssets.Text))
                {
                    _errorProvider.SetError(txtTotalAssets, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblTotalAssets.Text));
                    isValid = false;
                }
                else
                {
                    if (lblTotalAssetsReq.Visible)
                    {
                        if (Convert.ToDouble(txtTotalAssets.Text) > 0.00)
                        {
                            _errorProvider.SetError(txtTotalAssets, null);
                        }
                        else
                        {
                            _errorProvider.SetError(txtTotalAssets, Consts.Messages.Greaterthanzzero);
                            isValid = false;
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(txtTotalAssets, null);
                    }
                }

                if (lblMiscAssetsReq.Visible && string.IsNullOrEmpty(txtMiscAssets.Text))
                {
                    _errorProvider.SetError(txtMiscAssets, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMiscAssets.Text));
                    isValid = false;
                }
                else
                {
                    if (lblMiscAssetsReq.Visible)
                    {
                        if (Convert.ToDouble(txtMiscAssets.Text) > 0.00)
                        {
                            _errorProvider.SetError(txtMiscAssets, null);
                        }
                        else
                        {
                            _errorProvider.SetError(txtMiscAssets, Consts.Messages.Greaterthanzzero);
                            isValid = false;
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(txtMiscAssets, null);
                    }
                }



                if (lblTotalHouseholdExpenReq.Visible && string.IsNullOrEmpty(txtTotalHouseHold.Text))
                {
                    _errorProvider.SetError(txtTotalHouseHold, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblTotalHouseHoldExpenses.Text));
                    isValid = false;
                }
                else
                {
                    if (lblTotalHouseholdExpenReq.Visible)
                    {
                        if (Convert.ToDouble(txtTotalHouseHold.Text) > 0.00)
                        {
                            _errorProvider.SetError(txtTotalHouseHold, null);
                        }
                        else
                        {
                            _errorProvider.SetError(txtTotalHouseHold, Consts.Messages.Greaterthanzzero);
                            isValid = false;
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(txtTotalHouseHold, null);
                    }
                }
                if (lblMonthlyProgramIncomeReq.Visible && string.IsNullOrEmpty(txtMonthlyIncome.Text))
                {
                    _errorProvider.SetError(txtMonthlyIncome, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMOntlyProgramIncome.Text));
                    isValid = false;
                }
                else
                {
                    if (lblMonthlyProgramIncomeReq.Visible)
                    {
                        if (Convert.ToDouble(txtMonthlyIncome.Text) > 0.00)
                        {
                            _errorProvider.SetError(txtMonthlyIncome, null);
                        }
                        else
                        {
                            _errorProvider.SetError(txtMonthlyIncome, Consts.Messages.Greaterthanzzero);
                            isValid = false;
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(txtMonthlyIncome, null);
                    }
                }
                if (lblTotalLivingExpensesReq.Visible && string.IsNullOrEmpty(txtTotalLiving.Text))
                {
                    _errorProvider.SetError(txtTotalLiving, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblTotalLivingExpenses.Text));
                    isValid = false;
                }
                else
                {
                    if (lblTotalLivingExpensesReq.Visible)
                    {
                        if (Convert.ToDouble(txtTotalLiving.Text) > 0.00)
                        {
                            _errorProvider.SetError(txtTotalLiving, null);
                        }
                        else
                        {
                            _errorProvider.SetError(txtTotalLiving, Consts.Messages.Greaterthanzzero);
                            isValid = false;
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(txtTotalLiving, null);
                    }
                }
                if (chkSubsidized.Checked)
                {
                    if (lblSubsidizedReq.Visible && (cmbSubsidized.SelectedItem == null || ((ListItem)cmbSubsidized.SelectedItem).Text == Consts.Common.SelectOne))
                    {
                        _errorProvider.SetError(cmbSubsidized, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSubsidizedH.Text));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(cmbSubsidized, null);
                    }
                }

                if (lblHousingSituationReq.Visible && (cmbHousingSituation.SelectedItem == null || ((ListItem)cmbHousingSituation.SelectedItem).Text == Consts.Common.SelectOne))
                {
                    _errorProvider.SetError(cmbHousingSituation, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHousing.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbHousingSituation, null);
                    _errorProvider.SetError(btnLandlordInfo, null);

                    if ((((ListItem)cmbHousingSituation.SelectedItem).Value.ToString() == "B"))
                    {
                        if (Mode.Equals(Consts.Common.Edit))
                        {
                            CaseDiffEntity caseDiffDetails = _model.CaseMstData.GetLandlordadpya(BaseForm.BaseAgency.ToString(), BaseForm.BaseDept.ToString(), BaseForm.BaseProg.ToString(), BaseForm.BaseYear.ToString(), BaseForm.BaseApplicationNo.ToString(), "Landlord");
                            propCaseDiffLLDetails = caseDiffDetails;
                            if (!LandlordRequirevalidation())
                            {
                                _errorProvider.SetError(btnLandlordInfo, "Landlord Information is not complete");
                                isValid = false;
                            }
                        }
                        else
                        {

                            if (!LandlordRequirevalidation())
                            {
                                _errorProvider.SetError(btnLandlordInfo, "Landlord Information is not complete");
                                isValid = false;
                            }
                        }
                    }
                }

                if (lblDwellingReq.Visible && (cmbDwelling.SelectedItem == null || ((ListItem)cmbDwelling.SelectedItem).Text == Consts.Common.SelectOne))
                {
                    _errorProvider.SetError(cmbDwelling, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDwellingType.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbDwelling, null);
                }

                if (lblPMopfHeatReq.Visible && (cmbPMOPfHeat.SelectedItem == null || ((ListItem)cmbPMOPfHeat.SelectedItem).Text == Consts.Common.SelectOne))
                {
                    _errorProvider.SetError(cmbPMOPfHeat, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPMOPforHeat.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbPMOPfHeat, null);
                }

                if (lblPrimarySourcReq.Visible && (cmbPrimarySourceoHeat.SelectedItem == null || ((ListItem)cmbPrimarySourceoHeat.SelectedItem).Text == Consts.Common.SelectOne))
                {
                    _errorProvider.SetError(cmbPrimarySourceoHeat, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPrimaySource.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbPrimarySourceoHeat, null);


                }
                if (cmbPMOPfHeat.Items.Count > 0)
                {
                    if (((ListItem)cmbPMOPfHeat.SelectedItem).Value.ToString() == "1")
                    {
                        if (propAgencyControlDetails.State.ToUpper() == "CT" && BaseForm.BusinessModuleID.ToString() == "08")
                        {
                            if (!RequireLihpQues(((ListItem)cmbPrimarySourceoHeat.SelectedItem).Value.ToString()))
                            {
                                _errorProvider.SetError(btnLiheapques, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), btnLiheapques.Text));
                                isValid = false;
                            }
                        }
                    }
                }

            }
            //  }


            if (!isValid) isThirdTab = true;

            if (lblPresentEmployedReq.Visible && (cmbEpresenteEmploy.SelectedItem == null || ((ListItem)cmbEpresenteEmploy.SelectedItem).Text == Consts.Common.None1))
            {
                _errorProvider.SetError(cmbEpresenteEmploy, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPresentEmployed.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbEpresenteEmploy, null);
            }
            if (lblAnimationworkReq.Visible && (cmbEAnywork.SelectedItem == null || ((ListItem)cmbEAnywork.SelectedItem).Text == Consts.Common.None1))
            {
                _errorProvider.SetError(cmbEAnywork, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAnimationwork.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbEAnywork, null);
            }

            if (lblCurrentlyhaveReq.Visible && string.IsNullOrEmpty(txtEcurrentHave.Text.Trim()))
            {
                _errorProvider.SetError(txtEcurrentHave, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCurrentlyhave.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtEcurrentHave, null);

            }



            if (lbllastvisitReq.Visible && string.IsNullOrEmpty(txtElastvisit.Text.Trim()))
            {
                _errorProvider.SetError(txtElastvisit, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lbllastvisit.Text));
                isValid = false;
            }
            else
            {

                _errorProvider.SetError(txtElastvisit, null);

            }

            if (lblFullTimehoursReq.Visible && string.IsNullOrEmpty(txtEFullTime.Text.Trim()))
            {
                _errorProvider.SetError(txtEFullTime, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFullTimehours.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtEFullTime, null);

            }

            if (lblPartTimeHoursReq.Visible && string.IsNullOrEmpty(txtEPartTime.Text.Trim()))
            {
                _errorProvider.SetError(txtEPartTime, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPartTimeHours.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtEPartTime, null);

            }

            if (lblEmployerReq.Visible && string.IsNullOrEmpty(txtEEmployer.Text.Trim()))
            {
                _errorProvider.SetError(txtEEmployer, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEmployer.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtEEmployer, null);
            }

            if (lblEStreetReq.Visible && string.IsNullOrEmpty(txtEstreet.Text.Trim()))
            {
                _errorProvider.SetError(txtEstreet, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEStreet.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtEstreet, null);
            }

            if (lblECityStateReq.Visible && string.IsNullOrEmpty(txtEcityState.Text.Trim()))
            {
                _errorProvider.SetError(txtEcityState, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblECityState.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtEcityState, null);
            }

            if (lblEPhoneReq.Visible && string.IsNullOrEmpty(mskEPhone.Text.Trim()))
            {
                _errorProvider.SetError(mskEPhone, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEPhone.Text));
                isValid = false;
            }
            else
            {
                string strssn = mskEPhone.Text.Replace(" ", string.Empty);
                if (strssn.Length < 10 && strssn.Length > 0)
                {
                    _errorProvider.SetError(mskEPhone, "Please enter phone in correct format");
                    isValid = false;

                }
                else
                {
                    _errorProvider.SetError(mskEPhone, null);
                }

            }

            if (lblEExtReq.Visible && string.IsNullOrEmpty(txtEExt.Text.Trim()))
            {
                _errorProvider.SetError(txtEExt, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEExt.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtEExt, null);
            }

            if (lblSeasonalEmployReq.Visible && (cmbEseasonalEmployee.SelectedItem == null || ((ListItem)cmbEseasonalEmployee.SelectedItem).Text == Consts.Common.None1))
            {
                _errorProvider.SetError(cmbEseasonalEmployee, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSeasonalEmploy.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbEseasonalEmployee, null);
            }

            if (lblEjobTitleReq.Visible && (cmbEJobTitle.SelectedItem == null || ((ListItem)cmbEJobTitle.SelectedItem).Text == Consts.Common.SelectOne))
            {
                _errorProvider.SetError(cmbEJobTitle, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEjobTitle.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbEJobTitle, null);
            }

            if (lblEJobCategoryReq.Visible && (cmbEJobCategory.SelectedItem == null || ((ListItem)cmbEJobCategory.SelectedItem).Text == Consts.Common.SelectOne))
            {
                _errorProvider.SetError(cmbEJobCategory, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEJobCategory.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbEJobCategory, null);
            }

            if (lblHourlyWageReq.Visible && string.IsNullOrEmpty(txtEhourlywage.Text.Trim()))
            {
                _errorProvider.SetError(txtEhourlywage, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHourlyWage.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtEhourlywage, null);
            }

            if (txtEhourlywage.Text.Length > 3)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtEhourlywage.Text, Consts.StaticVars.TwoDecimalRange3String))
                {
                    _errorProvider.SetError(txtEhourlywage, Consts.Messages.PleaseEnterDecimals3Range);
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtEhourlywage, null);
                }
            }

            if (lblPayFrequencyReq.Visible && (cmbEpayFrequency.SelectedItem == null || ((ListItem)cmbEpayFrequency.SelectedItem).Text == Consts.Common.None1))
            {
                _errorProvider.SetError(cmbEpayFrequency, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPayFrequency.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbEpayFrequency, null);
            }

            if (lblLastDateWorkedReq.Visible && dtElastDateWorked.Checked == false)
            {
                _errorProvider.SetError(dtElastDateWorked, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblLastDateWorked.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtElastDateWorked, null);
            }

            if (lblEHireDateReq.Visible && dtEHireDate.Checked == false)
            {
                _errorProvider.SetError(dtEHireDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEhireDate.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtEHireDate, null);
            }

            if (lblEShiftsWorked1stReq.Visible && chkE1st.Checked == false)
            {
                _errorProvider.SetError(lblEShiftsWorked1stReq, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEShiftsWorked.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(lblEShiftsWorked1stReq, null);
            }

            if (lblE2nreq.Visible && chkE2nd.Checked == false)
            {
                _errorProvider.SetError(lblE2nreq, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEShiftsWorked.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(lblE2nreq, null);
            }

            if (lblE3rdReq.Visible && chkE3rd.Checked == false)
            {
                _errorProvider.SetError(lblE3rdReq, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEShiftsWorked.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(lblE3rdReq, null);
            }

            if (lblERotatingReq.Visible && chkErotaing.Checked == false)
            {
                _errorProvider.SetError(lblERotatingReq, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEShiftsWorked.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(lblERotatingReq, null);
            }

            if (!isValid) isFourthTab = true;

            // Preasses Tab 

            foreach (DataGridViewRow dataGridViewRow in gvwPreassesData.Rows)
            {
                if (dataGridViewRow.Tag is CustomQuestionsEntity)
                {
                    CustomQuestionsEntity custques = dataGridViewRow.Tag as CustomQuestionsEntity;
                    if (custques != null)
                    {
                        if (custques.CUSTACTIVECUST.ToUpper() == "A")
                        {
                            string strQuestiondi = dataGridViewRow.Cells["gvtPQId"].Value.ToString();
                            string strFieldEnable = dataGridViewRow.Cells["gvtPDEnable"].Tag != null ? dataGridViewRow.Cells["gvtPDEnable"].Tag.ToString() : string.Empty;
                            string strRequire = dataGridViewRow.Cells["gvtPreq"].Tag != null ? dataGridViewRow.Cells["gvtPreq"].Tag.ToString() : string.Empty;
                            if (dataGridViewRow.Cells["gvtPreq"].Value.ToString() == "*")
                            {
                                PreassessQuesEntity preassesdimentdata = preassessChildEntity.Find(u => u.PRECHILD_DQID == strQuestiondi);
                                if (preassesdimentdata != null)
                                {
                                    if (strFieldEnable != "N" && strRequire == "Y")
                                    {
                                        string inputValue = string.Empty;
                                        inputValue = dataGridViewRow.Cells["gvtPResponse"].Value != null ? dataGridViewRow.Cells["gvtPResponse"].Value.ToString() : string.Empty;
                                        if (inputValue == string.Empty)
                                        {
                                            CommonFunctions.MessageBoxDisplay("Please enter require preasses questions answers");
                                            isValid = false;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (strFieldEnable != "N")
                                    {
                                        string inputValue = string.Empty;
                                        inputValue = dataGridViewRow.Cells["gvtPResponse"].Value != null ? dataGridViewRow.Cells["gvtPResponse"].Value.ToString() : string.Empty;
                                        if (inputValue == string.Empty)
                                        {
                                            CommonFunctions.MessageBoxDisplay("Please enter require preasses questions answers");
                                            isValid = false;
                                            break;
                                        }
                                    }

                                }


                            }
                            else
                            {
                                if (strFieldEnable != "N" && strRequire == "Y")
                                {
                                    string inputValue = string.Empty;
                                    inputValue = dataGridViewRow.Cells["gvtPResponse"].Value != null ? dataGridViewRow.Cells["gvtPResponse"].Value.ToString() : string.Empty;
                                    string strQuestion = dataGridViewRow.Cells["gvtPQuestions"].Value != null ? dataGridViewRow.Cells["gvtPQuestions"].Value.ToString() : string.Empty;
                                    if (inputValue == string.Empty)
                                    {
                                        CommonFunctions.MessageBoxDisplay("Please enter " + strQuestion + " question answer");
                                        isValid = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (!isValid) isFifthTab = true;


            if (isFirstTab) tabControl1.SelectedIndex = 0;
            else if (isSecondTab) tabControl1.SelectedIndex = 1;
            else if (isThirdTab) tabControl1.SelectedIndex = 2;
            else if (isFourthTab) tabControl1.SelectedIndex = 3;
            else if (isFifthTab) tabControl1.SelectedIndex = 4;
            return isValid;
        }



        private void SetNumeric()
        {
            txtBIC.Validator = TextBoxValidation.IntegerValidator;
            txtYear.Validator = TextBoxValidation.IntegerValidator;
            txtNoOfYearsAtAddress.Validator = TextBoxValidation.IntegerValidator;
            //// txtEmail.Validator = TextBoxValidation.EmailValidator;
            txtExpand1.Validator = CustomDecimalValidation9dot2;
            txtExpand2.Validator = CustomDecimalValidation9dot2;
            txtExpand3.Validator = CustomDecimalValidation9dot2;
            txtExpand4.Validator = CustomDecimalValidation9dot2;
            txtRent.Validator = CustomDecimalValidation8dot2;
            txtWater.Validator = CustomDecimalValidation8dot2;
            txtHeating.Validator = CustomDecimalValidation8dot2;
            txtElectric.Validator = CustomDecimalValidation8dot2;
            txtMiscExpenses.Validator = CustomDecimalValidation8dot2;
            txtMiscDebt.Validator = CustomDecimalValidation9dot2;
            txtMiscAssets.Validator = CustomDecimalValidation9dot2;
            txtPhysicalAssets.Validator = CustomDecimalValidation9dot2;
            txtLiquidAssets.Validator = CustomDecimalValidation9dot2;
            txtOtherAssets.Validator = CustomDecimalValidation9dot2;
            txtTotalAssets.Validator = CustomDecimalValidation8dot2;

        }

        public static TextBoxValidation CustomDecimalValidation8dot2
        {
            get
            {
                return new TextBoxValidation(@"^[0-9]\d{0,5}(\.\d{1,2})*(,\d+)?$", "Value must be between 0 - 999999.99", "0-9\\.");
            }
        }

        public static TextBoxValidation CustomDecimalValidation9dot2
        {
            get
            {
                return new TextBoxValidation(@"^[0-9]\d{0,6}(\.\d{1,2})*(,\d+)?$", "Value must be between 0 - 9999999.99", "0-9\\.");
            }
        }

        private void fillClientIntake()
        {
            CaseMstEntity caseMSTEntity = CaseMST;
            if (caseMSTEntity != null)
            {
                txtBIC.Text = caseMSTEntity.Bic.Trim();
                txtHN.Text = caseMSTEntity.Hn.Trim();
                txtDirection.Text = caseMSTEntity.Direction.Trim();
                txtSuffix.Text = caseMSTEntity.Suffix.Trim();
                txtStreet.Text = caseMSTEntity.Street.Trim();
                txtFloor.Text = caseMSTEntity.Flr;
                txtPrecinct.Text = caseMSTEntity.Precinct;
                txtApartment.Text = caseMSTEntity.Apt;
                if (caseMSTEntity.ActiveStatus.Equals("Y")) cbActive.Checked = true; else cbActive.Checked = false;
                if (caseMSTEntity.Secret.Equals("Y")) cbSecret.Checked = true; else cbSecret.Checked = false;
                //txtZipCode.Text = caseMSTEntity.Zip + caseMSTEntity.Zipplus;
                if (caseMSTEntity.Zip != string.Empty)
                    txtZipCode.Text = "00000".Substring(0, 5 - caseMSTEntity.Zip.Length) + caseMSTEntity.Zip;
                if (caseMSTEntity.Zipplus != string.Empty)
                    txtZipPlus.Text = "0000".Substring(0, 4 - caseMSTEntity.Zipplus.Length) + caseMSTEntity.Zipplus;
                txtCity.Text = caseMSTEntity.City.Trim();
                txtState.Text = caseMSTEntity.State;
                txtSite.Text = caseMSTEntity.Site.Trim();
                if (Mode.Equals(Consts.Common.Add))
                {
                    if (BaseForm.UserProfile.Site.ToString().Trim() != "****" && BaseForm.UserProfile.Site.ToString().Trim() != string.Empty)
                    {
                        if (BaseForm.UserProfile.Site.ToString().Length > 2)
                            txtSite.Text = BaseForm.UserProfile.Site.Trim().Substring(2, BaseForm.UserProfile.Site.ToString().Length - 2);
                    }
                }
                if (!caseMSTEntity.IntakeDate.Equals(string.Empty)) { dtpIntakeDate.Checked = true; dtpIntakeDate.Text = caseMSTEntity.IntakeDate; }
                else
                {
                    dtpIntakeDate.Value = DateTime.Now.Date;
                    dtpIntakeDate.Checked = false;
                }
                if (!caseMSTEntity.CaseReviewDate.Equals(string.Empty)) { dtpCaseReview.Checked = true; dtpCaseReview.Text = caseMSTEntity.CaseReviewDate; }
                else
                {
                    dtpCaseReview.Value = DateTime.Now.Date;
                    dtpCaseReview.Checked = false;
                }
                if (!caseMSTEntity.InitialDate.Equals(string.Empty)) { dtpInitialDate.Checked = true; dtpInitialDate.Text = caseMSTEntity.InitialDate; }
                else
                {
                    dtpInitialDate.Value = DateTime.Now.Date;
                    dtpInitialDate.Checked = false;
                }

                SetComboBoxValue(cmbAboutUs, caseMSTEntity.AboutUs);
                if (caseMSTEntity.AboutUs != string.Empty)
                    CaseMST.AboutUsDesc = ((ListItem)cmbAboutUs.SelectedItem).Text.ToString();

                SetComboBoxValue(cmbCaseType, caseMSTEntity.CaseType);

                if (caseMSTEntity.CaseType != string.Empty)
                    CaseMST.CaseTypeDesc = ((ListItem)cmbCaseType.SelectedItem).Text.ToString();

                SetComboBoxValue(cmbContact, caseMSTEntity.BestContact);
                if (caseMSTEntity.BestContact != string.Empty)
                    CaseMST.ContactusDesc = ((ListItem)cmbContact.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbCounty, caseMSTEntity.County);
                if (caseMSTEntity.County != string.Empty)
                    CaseMST.CountyDesc = ((ListItem)cmbCounty.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbFamilyType, caseMSTEntity.FamilyType);
                cmbFamilyType_SelectedIndexChanged(cmbFamilyType, new EventArgs());
                if (caseMSTEntity.FamilyType != string.Empty)
                    CaseMST.FamilyTypeDesc = ((ListItem)cmbFamilyType.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbHousingSituation, caseMSTEntity.Housing);
                if (caseMSTEntity.Housing != string.Empty)
                    CaseMST.HousingSituvationDesc = ((ListItem)cmbHousingSituation.SelectedItem).Text.ToString();

                if (propAgencyControlDetails.State.ToUpper() == "TX")
                {
                    SetComboBoxValue(cmbOutService, caseMSTEntity.OutOfService);
                    if (caseMSTEntity.OutOfService != string.Empty)
                        CaseMST.OutofserviceDesc = ((ListItem)cmbOutService.SelectedItem).Text.ToString();
                    //if (((ListItem)cmbOutService.SelectedItem).Value.ToString() != "I" && ((ListItem)cmbOutService.SelectedItem).Value.ToString() != "0")
                    //{
                    //    lblPrecinctReq.Visible = false;
                    //}
                }

                SetComboBoxValue(cmbPrimaryLang, caseMSTEntity.Language);
                if (caseMSTEntity.Language != string.Empty)
                    CaseMST.PrimaryLanDesc = ((ListItem)cmbPrimaryLang.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbSecondLang, caseMSTEntity.LanguageOt);
                if (caseMSTEntity.LanguageOt != string.Empty)
                    CaseMST.SecondaryLanDesc = ((ListItem)cmbSecondLang.SelectedItem).Text.ToString();
                if (Mode.Equals("Add") && caseMSTEntity.IntakeWorker == string.Empty)
                {
                    SetComboBoxValue(cmbStaff, strCaseWorkerDefaultCode);
                }
                else
                    SetComboBoxValue(cmbStaff, caseMSTEntity.IntakeWorker);
                if (caseMSTEntity.IntakeWorker != string.Empty)
                    CaseMST.AssignWorkerDesc = ((ListItem)cmbStaff.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbTownship, caseMSTEntity.TownShip);
                if (caseMSTEntity.TownShip != string.Empty)
                    CaseMST.TownShipDesc = ((ListItem)cmbTownship.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbWaitingList, caseMSTEntity.WaitList);
                if (caseMSTEntity.WaitList != string.Empty)
                    CaseMST.WaitingListDesc = ((ListItem)cmbWaitingList.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbVerifiedStaff, caseMSTEntity.ExpCaseWorker);
                if (caseMSTEntity.ExpCaseWorker != string.Empty)
                {
                    CaseMST.ExpCaseworkerDesc = ((ListItem)cmbVerifiedStaff.SelectedItem).Text.ToString();
                    CaseMST.CasworkerHousDesc = ((ListItem)cmbVerifiedStaff.SelectedItem).Text.ToString();
                }
                else
                {
                    CaseMST.ExpCaseWorker = string.Empty;
                }
                SetComboBoxValue(cmbDwelling, caseMSTEntity.Dwelling);
                if (caseMSTEntity.Dwelling != string.Empty)
                    CaseMST.DwellingDesc = ((ListItem)cmbDwelling.SelectedItem).Text.ToString();

                SetComboBoxValue(cmbPMOPfHeat, caseMSTEntity.HeatIncRent);
                cmbPMOPfHeat_SelectedIndexChanged(cmbPMOPfHeat, new EventArgs());
                if (caseMSTEntity.HeatIncRent != string.Empty)
                    CaseMST.HeatIncRentDesc = ((ListItem)cmbPMOPfHeat.SelectedItem).Text.ToString();

                SetComboBoxValue(cmbPrimarySourceoHeat, caseMSTEntity.Source);
                if (caseMSTEntity.Source != string.Empty)
                    CaseMST.SourceDesc = ((ListItem)cmbPrimarySourceoHeat.SelectedItem).Text.ToString();

                chkSubsidized.Checked = caseMSTEntity.SubShouse == "Y" ? true : false;
                //chkSubsidized_CheckedChanged(chkSubsidized, new EventArgs());
                cmbHousingSituation_SelectedIndexChanged(cmbHousingSituation, new EventArgs());

                SetComboBoxValue(cmbSubsidized, caseMSTEntity.SubStype);
                if (caseMSTEntity.SubStype != string.Empty)
                    CaseMST.SubsTypeDesc = ((ListItem)cmbSubsidized.SelectedItem).Text.ToString();

                //txtHomePhone.Text = caseMSTEntity.Area + caseMSTEntity.Phone;
                //txtCell.Text = caseMSTEntity.CellPhone;
                //txtMessage.Text = caseMSTEntity.MessagePhone;
                if (caseMSTEntity.HomePhone_NA == "Y")
                {
                    txtHomePhone.Text = string.Empty;
                    ChkHome_Na.Checked = true;
                }
                else
                    txtHomePhone.Text = caseMSTEntity.Area + caseMSTEntity.Phone;


                if (caseMSTEntity.CellPhone_NA == "Y")
                {
                    txtCell.Text = string.Empty;
                    chk_CellNa.Checked = true;
                }
                else
                    txtCell.Text = caseMSTEntity.CellPhone;


                if (caseMSTEntity.MessagePhone_NA == "Y")
                {
                    txtMessage.Text = string.Empty;
                    chkMessage_Na.Checked = true;
                }
                else
                    txtMessage.Text = caseMSTEntity.MessagePhone;

                txtTTY.Text = caseMSTEntity.TtyNumber;
                txtFax.Text = caseMSTEntity.FaxNumber;

                if (caseMSTEntity.Email_NA == "Y")
                {
                    txtEmail.Text = string.Empty;
                    chkEmail_Na.Checked = true;
                }
                else
                {
                    txtEmail.Text = caseMSTEntity.Email.Trim();
                }


                txtRent.Text = caseMSTEntity.ExpRent == string.Empty ? "0.00" : caseMSTEntity.ExpRent;
                txtHeating.Text = caseMSTEntity.ExpHeat == string.Empty ? "0.00" : caseMSTEntity.ExpHeat;
                txtWater.Text = caseMSTEntity.ExpWater == string.Empty ? "0.00" : caseMSTEntity.ExpWater;
                txtElectric.Text = caseMSTEntity.ExpElectric == string.Empty ? "0.00" : caseMSTEntity.ExpElectric;
                txtExpand1.Text = caseMSTEntity.Debtcc == string.Empty ? "0.00" : caseMSTEntity.Debtcc;
                txtExpand2.Text = caseMSTEntity.DebtLoans == string.Empty ? "0.00" : caseMSTEntity.DebtLoans;
                txtExpand3.Text = caseMSTEntity.DebtMed == string.Empty ? "0.00" : caseMSTEntity.DebtMed;
                txtExpand4.Text = caseMSTEntity.DebtOther == string.Empty ? "0.00" : caseMSTEntity.DebtOther;
                txtTotalLiving.Text = caseMSTEntity.ExpLivexpense == string.Empty ? "0.00" : caseMSTEntity.ExpLivexpense;
                txtMiscExpenses.Text = caseMSTEntity.ExpMisc == string.Empty ? "0.00" : caseMSTEntity.ExpMisc;
                txtMiscDebt.Text = caseMSTEntity.DebtMisc == string.Empty ? "0.00" : caseMSTEntity.DebtMisc;
                txtMiscAssets.Text = caseMSTEntity.AsetMisc == string.Empty ? "0.00" : caseMSTEntity.AsetMisc;
                txtPhysicalAssets.Text = caseMSTEntity.AsetPhy == string.Empty ? "0.00" : caseMSTEntity.AsetPhy;
                txtOtherAssets.Text = caseMSTEntity.AsetOth == string.Empty ? "0.00" : caseMSTEntity.AsetOth;
                txtLiquidAssets.Text = caseMSTEntity.AsetLiq == string.Empty ? "0.00" : caseMSTEntity.AsetLiq;
                txtTotalAssets.Text = caseMSTEntity.AsetTotal == string.Empty ? "0.00" : caseMSTEntity.AsetTotal;
                txtDebtAssetRatio.Text = caseMSTEntity.AsetRatio == string.Empty ? "0.00" : caseMSTEntity.AsetRatio;
                txtTotalHHDebt.Text = caseMSTEntity.DebtTotal == string.Empty ? "0.00" : caseMSTEntity.DebtTotal;
                txtDebtIncomeRatio.Text = caseMSTEntity.DebIncmRation == string.Empty ? "0.00" : caseMSTEntity.DebIncmRation;
                // txtTotalHouseholdIncome.Text = caseMSTEntity.; 
                if (caseMSTEntity.ProgIncome.ToString() != string.Empty)
                {
                    double MonthlyIncome = double.Parse(CaseMST.ProgIncome);
                    txtTotalHouseholdIncome.Text = MonthlyIncome.ToString();
                    if (MonthlyIncome > 0)
                    {
                        MonthlyIncome = MonthlyIncome / 12;
                        MonthlyIncome = Math.Round(MonthlyIncome, 2);
                        txtMonthlyIncome.Text = MonthlyIncome.ToString();
                    }
                    else
                    {
                        txtMonthlyIncome.Text = "0";
                    }
                }
                else
                    txtMonthlyIncome.Text = "0";
                txtTotalHouseHold.Text = caseMSTEntity.ExpTotal;
                txtNoOfYearsAtAddress.Text = caseMSTEntity.AddressYears;

                DataSet serviceDS = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE(string.Empty, caseMSTEntity.ApplAgency, caseMSTEntity.ApplDept, caseMSTEntity.ApplProgram, caseMSTEntity.ApplYr, caseMSTEntity.ApplNo);
                DataTable serviceDT = serviceDS.Tables[0];
                List<string> serviceList = new List<string>();
                gvwServices.Rows.Clear();
                cmbServicesInquired.Items.Clear();
                foreach (DataRow dr in serviceDT.Rows)
                {
                    gvwServices.Rows.Add(false, dr["INQ_DESC"].ToString(), dr["INQ_CODE"].ToString());
                    // listBoxSelectionControl1.ListBoxSelected.Items.Add(new ListItem(dr["INQ_DESC"].ToString(), dr["INQ_CODE"].ToString()));
                    serviceList.Add(dr["INQ_CODE"].ToString());
                    ListItem li = new ListItem(dr["INQ_DESC"].ToString(), dr["INQ_CODE"].ToString());
                    cmbServicesInquired.Items.Add(li);

                }
                cmbServicesInquired.Items.Insert(0, new ListItem("Select One", "0"));
                cmbServicesInquired.SelectedIndex = 0;

                DataSet serviceSaveDS = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE("SAVE", caseMSTEntity.ApplAgency, caseMSTEntity.ApplDept, caseMSTEntity.ApplProgram, caseMSTEntity.ApplYr, caseMSTEntity.ApplNo);
                DataTable serviceSaveDT = serviceSaveDS.Tables[0];
                List<string> serviceSaveList = new List<string>();
                if (serviceSaveDT.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in gvwServices.Rows)
                    {
                        if (row.Cells["ServiceCode"].Value != null)
                        {
                            for (int i = 0; i < serviceSaveDT.Rows.Count; i++)
                            {
                                if (Convert.ToString(row.Cells["ServiceCode"].Value.ToString().Trim()) == serviceSaveDT.Rows[i]["INQ_CODE"].ToString().Trim())
                                {
                                    row.Cells["Servicechk"].Value = true; break;
                                }
                            }

                        }
                    }
                }
                if (cmbEnergydata.Items.Count > 0)
                {
                    SetComboBoxValue(cmbEnergydata, caseMSTEntity.ApplictionType);
                    if (caseMSTEntity.ApplictionDate.Trim() != string.Empty)
                    {
                        dtApplicationType.Value = Convert.ToDateTime(caseMSTEntity.ApplictionDate);
                        dtApplicationType.Checked = true;
                    }
                }


            }
        }

        private void fillWaitList()
        {
            //cmbWaitingList.Items.Clear();
            //List<ListItem> listItem = new List<ListItem>();
            //listItem.Add(new ListItem("Select One", ""));
            //listItem.Add(new ListItem("None", "O"));
            //listItem.Add(new ListItem("Yes", "Y"));
            //listItem.Add(new ListItem("No", "N"));
            //cmbWaitingList.Items.AddRange(listItem.ToArray());
            //cmbWaitingList.SelectedIndex = 0;

            List<CommonEntity> commonwailtinglist = new List<CommonEntity>();

            if (propAgencyControlDetails != null)
            {
                if (propAgencyControlDetails.State.ToUpper() == "TX")
                {
                    commonwailtinglist = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0067", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); ////_model.lookupDataAccess.GetCaseType();
                }
                else
                {
                    commonwailtinglist = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0002", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); ////_model.lookupDataAccess.GetCaseType();
                }
            }
            else
            {
                commonwailtinglist = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0002", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
            }
            // CaseType = filterByHIE(CaseType);
            cmbWaitingList.Items.Insert(0, new ListItem("Select One", "0"));
            cmbWaitingList.ColorMember = "FavoriteColor";
            cmbWaitingList.SelectedIndex = 0;
            foreach (CommonEntity wailtlist in commonwailtinglist)
            {
                ListItem li = new ListItem(wailtlist.Desc, wailtlist.Code);
                cmbWaitingList.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && wailtlist.Default.Equals("Y")) cmbCaseType.SelectedItem = li;
            }

        }

        private void fillOutofService()
        {
            cmbOutService.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("", "0"));
            listItem.Add(new ListItem("Out of Service Area", "O"));
            listItem.Add(new ListItem("In Service Area (in File)", "I"));
            listItem.Add(new ListItem("In Service Area (Not in File)", "X"));
            cmbOutService.Items.AddRange(listItem.ToArray());
            cmbOutService.SelectedIndex = 0;
        }

        private void fillLegaltowork()
        {
            cmbLegaltowork.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("Select One", ""));
            listItem.Add(new ListItem("Yes", "Y"));
            listItem.Add(new ListItem("No", "N"));
            listItem.Add(new ListItem("Unavailable", "U"));
            cmbLegaltowork.Items.AddRange(listItem.ToArray());
            cmbLegaltowork.SelectedIndex = 0;
        }

        private void fillEmployeeCombo()
        {

            txtEcurrentHave.Validator = TextBoxValidation.IntegerValidator;
            txtElastvisit.Validator = TextBoxValidation.IntegerValidator;
            txtEFullTime.Validator = TextBoxValidation.IntegerValidator;
            txtEPartTime.Validator = TextBoxValidation.IntegerValidator;
            txtEhourlywage.Validator = TextBoxValidation.FloatValidator;

            cmbEseasonalEmployee.Items.Clear();
            cmbEpresenteEmploy.Items.Clear();
            cmbEpayFrequency.Items.Clear();
            cmbEAnywork.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("None", ""));
            listItem.Add(new ListItem("Yes", "Y"));
            listItem.Add(new ListItem("No", "N"));
            listItem.Add(new ListItem("Unavailable", "U"));
            cmbEseasonalEmployee.Items.AddRange(listItem.ToArray());
            cmbEseasonalEmployee.SelectedIndex = 0;
            cmbEAnywork.Items.AddRange(listItem.ToArray());
            cmbEAnywork.SelectedIndex = 0;

            List<ListItem> listItemEmployee = new List<ListItem>();
            listItemEmployee.Add(new ListItem("None", ""));
            listItemEmployee.Add(new ListItem("Yes", "Y"));
            listItemEmployee.Add(new ListItem("No", "N"));
            listItemEmployee.Add(new ListItem("Unavailable", "U"));
            listItemEmployee.Add(new ListItem("No Response", "R"));
            listItemEmployee.Add(new ListItem("N/A", "A"));
            cmbEpresenteEmploy.Items.AddRange(listItemEmployee.ToArray());
            cmbEpresenteEmploy.SelectedIndex = 0;


            List<ListItem> listFrequency = new List<ListItem>();
            listFrequency.Add(new ListItem("None", ""));
            listFrequency.Add(new ListItem("Paid Weekly", "PW1"));
            listFrequency.Add(new ListItem("Paid Every 2 Weeks", "PW2"));
            listFrequency.Add(new ListItem("Other", "OTH"));
            cmbEpayFrequency.Items.AddRange(listFrequency.ToArray());
            cmbEpayFrequency.SelectedIndex = 0;


            List<CommonEntity> commonEntity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.JOBTITLE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetJobTitle();
            commonEntity = filterByHIE(commonEntity);
            cmbEJobTitle.Items.Insert(0, new ListItem("Select One", "0"));
            cmbEJobTitle.ColorMember = "FavoriteColor";
            cmbEJobTitle.SelectedIndex = 0;
            foreach (CommonEntity JobTitle in commonEntity)
            {
                ListItem li = new ListItem(JobTitle.Desc, JobTitle.Code, JobTitle.Active, JobTitle.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbEJobTitle.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && JobTitle.Default.Equals("Y")) cmbEJobTitle.SelectedItem = li;
            }


            List<CommonEntity> commonjobCategory = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.JOBCATEGORY, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetJobCategory();
            commonjobCategory = filterByHIE(commonjobCategory);
            cmbEJobCategory.Items.Insert(0, new ListItem("Select One", "0"));
            cmbEJobCategory.ColorMember = "FavoriteColor";
            cmbEJobCategory.SelectedIndex = 0;
            foreach (CommonEntity JobCategory in commonjobCategory)
            {
                ListItem li = new ListItem(JobCategory.Desc, JobCategory.Code, JobCategory.Active, JobCategory.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbEJobCategory.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && JobCategory.Default.Equals("Y")) cmbEJobCategory.SelectedItem = li;
            }


        }

        private void fillDropDowns()
        {

            //AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
            //searchAgytabs.Tabs_Type = "S0010";
            List<CommonEntity> AgyTabs_List = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0010", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.AdhocData.Browse_AGYTABS(searchAgytabs);
            foreach (CommonEntity item in AgyTabs_List)
            {
                cmbSSNReason.Items.Add(new ListItem(item.Desc, item.Code));
            }
            cmbSSNReason.Items.Insert(0, new ListItem("", "0"));
            cmbSSNReason.SelectedIndex = 0;

            List<CommonEntity> commonEntity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RESIDENTCODES, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetResident();
            //commonEntity = filterByHIE(commonEntity);
            cmbResident.Items.Insert(0, new ListItem("Select One", "0"));
            cmbResident.ColorMember = "FavoriteColor";
            cmbResident.SelectedIndex = 0;
            foreach (CommonEntity Resident in commonEntity)
            {
                ListItem li = new ListItem(Resident.Desc, Resident.Code, Resident.Active, Resident.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbResident.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && Resident.Default.Equals("Y")) cmbResident.SelectedItem = li;
            }
            //_model.lookupDataAccess.GetRelationship();
            List<CommonEntity> Relation = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RELATIONSHIP, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
            //if (Relation.Count > 0)
            //{
            //    string strRelationDefaultValue = Relation.Find(u => u.Default == "Y").Code.ToString();
            //}
            //Relation = filterByHIE(Relation);
            cmbRelation.Items.Insert(0, new ListItem("     ", "0"));
            cmbRelation.ColorMember = "FavoriteColor";
            string[] strRelationTypes = ProgramDefinition.DepReleataionTypes.Split(' ');
            foreach (CommonEntity Relationship in Relation)
            {
                bool boolRelationType = false;
                foreach (string RelationType in strRelationTypes)
                {
                    if (Relationship.Code == RelationType)
                    {
                        boolRelationType = true;
                    }
                }
                ListItem li;
                if (boolRelationType)
                    li = new ListItem(Relationship.Desc, Relationship.Code, Relationship.Active, Relationship.Active.Equals("Y") ? Color.Green : Color.Red, Relationship.Default.Equals("Y") ? "Y" : "N", "Y");
                else
                    li = new ListItem(Relationship.Desc, Relationship.Code, Relationship.Active, Relationship.Active.Equals("Y") ? Color.Green : Color.Red, Relationship.Default.Equals("Y") ? "Y" : "N", "N");

                cmbRelation.Items.Add(li);
                if (Relationship.Default.Equals("Y"))
                    Relationshipdefaultcode = Relationship.Code.Trim();
                if (Mode.Equals(Consts.Common.Add) && IsApplicant && Relationship.Default.Equals("Y"))
                {
                    CaseSNP.MemberCode = Relationship.Code;
                    cmbRelation.SelectedItem = li;
                    cmbRelation.Enabled = false;
                }
            }
            if (IsApplicant && Relationshipdefaultcode == string.Empty)
                cmbRelation.SelectedIndex = 0;
            if (!IsApplicant) cmbRelation.SelectedIndex = 0;

            List<CommonEntity> Ethnicity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.ETHNICODES, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);  // _model.lookupDataAccess.GetEthnicity();
            //Ethnicity = filterByHIE(Ethnicity);
            cmbEthnicity.Items.Insert(0, new ListItem("Select One", "0"));
            cmbEthnicity.ColorMember = "FavoriteColor";
            cmbEthnicity.SelectedIndex = 0;
            foreach (CommonEntity Etncity in Ethnicity)
            {
                ListItem li = new ListItem(Etncity.Desc, Etncity.Code, Etncity.Active, Etncity.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbEthnicity.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && Etncity.Default.Equals("Y")) cmbEthnicity.SelectedItem = li;
            }

            List<CommonEntity> Race = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RACE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetRace();
            //Race = filterByHIE(Race);
            cmbRace.Items.Insert(0, new ListItem("Select One", "0"));
            cmbRace.ColorMember = "FavoriteColor";
            cmbRace.SelectedIndex = 0;
            foreach (CommonEntity race in Race)
            {
                ListItem li = new ListItem(race.Desc, race.Code, race.Active, race.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbRace.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && race.Default.Equals("Y")) cmbRace.SelectedItem = li;
            }

            List<CommonEntity> Education = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.EDUCATIONCODES, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);  //_model.lookupDataAccess.GetEducation();
            // Education = filterByHIE(Education);
            cmbEducation.Items.Insert(0, new ListItem("Select One", "0"));
            cmbEducation.ColorMember = "FavoriteColor";
            cmbEducation.SelectedIndex = 0;
            foreach (CommonEntity education in Education)
            {
                ListItem li = new ListItem(education.Desc, education.Code, education.Active, education.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbEducation.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && education.Default.Equals("Y")) cmbEducation.SelectedItem = li;
            }

            List<CommonEntity> School = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.SCHOOLDISTRICTS, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); // _model.lookupDataAccess.GetSchool();
            //School = filterByHIE(School);
            cmbSchool.Items.Insert(0, new ListItem("Select One", "0"));
            cmbSchool.ColorMember = "FavoriteColor";
            cmbSchool.SelectedIndex = 0;
            foreach (CommonEntity school in School)
            {
                ListItem li = new ListItem(school.Desc, school.Code, school.Active, school.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbSchool.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && school.Default.Equals("Y")) cmbSchool.SelectedItem = li;
            }

            List<CommonEntity> Gender = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.GENDER, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); // _model.lookupDataAccess.GetGender();
            // Gender = filterByHIE(Gender);
            cmbGender.Items.Insert(0, new ListItem("Select One", "0"));
            cmbGender.ColorMember = "FavoriteColor";
            cmbGender.SelectedIndex = 0;
            foreach (CommonEntity gender in Gender)
            {
                ListItem li = new ListItem(gender.Desc, gender.Code, gender.Active, gender.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbGender.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && gender.Default.Equals("Y")) cmbGender.SelectedItem = li;
            }

            List<CommonEntity> Areyoupregnant = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.PREGNANT, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetAreyoupregnant();
            // Areyoupregnant = filterByHIE(Areyoupregnant);
            cmbAreyoupregnant.Items.Insert(0, new ListItem("     ", "0"));
            cmbAreyoupregnant.ColorMember = "FavoriteColor";
            cmbAreyoupregnant.SelectedIndex = 0;
            foreach (CommonEntity areyoupregnant in Areyoupregnant)
            {
                ListItem li = new ListItem(areyoupregnant.Desc, areyoupregnant.Code, areyoupregnant.Active, areyoupregnant.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbAreyoupregnant.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && areyoupregnant.Default.Equals("Y")) cmbAreyoupregnant.SelectedItem = li;
            }

            List<CommonEntity> MaritalStatus = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.MARITALSTATUS, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetMaritalStatus();
            // MaritalStatus = filterByHIE(MaritalStatus);
            cmbMaritalStatus.Items.Insert(0, new ListItem("Select One", "0"));
            cmbMaritalStatus.ColorMember = "FavoriteColor";
            cmbMaritalStatus.SelectedIndex = 0;
            foreach (CommonEntity maritalStatus in MaritalStatus)
            {
                ListItem li = new ListItem(maritalStatus.Desc, maritalStatus.Code, maritalStatus.Active, maritalStatus.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbMaritalStatus.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && maritalStatus.Default.Equals("Y")) cmbMaritalStatus.SelectedItem = li;
            }

            List<CommonEntity> HealthInsurance = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HEALTHINSURANCE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetHealthInsurance();
            // HealthInsurance = filterByHIE(HealthInsurance);
            cmbHealthInsurance.Items.Insert(0, new ListItem("Select One", "0"));
            cmbHealthInsurance.ColorMember = "FavoriteColor";
            cmbHealthInsurance.SelectedIndex = 0;
            foreach (CommonEntity healthInsurance in HealthInsurance)
            {
                ListItem li = new ListItem(healthInsurance.Desc, healthInsurance.Code, healthInsurance.Active, healthInsurance.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbHealthInsurance.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && healthInsurance.Default.Equals("Y")) cmbHealthInsurance.SelectedItem = li;
            }
            List<CommonEntity> VeteranCode = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.VETERAN, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetVeteranCode();
            //VeteranCode = filterByHIE(VeteranCode);
            cmbVeteranCode.Items.Insert(0, new ListItem("Select One", "0"));
            cmbVeteranCode.ColorMember = "FavoriteColor";
            cmbVeteranCode.SelectedIndex = 0;
            foreach (CommonEntity veteranCode in VeteranCode)
            {
                ListItem li = new ListItem(veteranCode.Desc, veteranCode.Code, veteranCode.Active, veteranCode.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbVeteranCode.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && veteranCode.Default.Equals("Y")) cmbVeteranCode.SelectedItem = li;
            }

            List<CommonEntity> FoodStamps = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.FOODSTAMPS, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetFoodStamps();
            //FoodStamps = filterByHIE(FoodStamps);
            cmbFoodStamps.Items.Insert(0, new ListItem("Select One", "0"));
            cmbFoodStamps.ColorMember = "FavoriteColor";
            cmbFoodStamps.SelectedIndex = 0;
            foreach (CommonEntity foodStamps in FoodStamps)
            {
                ListItem li = new ListItem(foodStamps.Desc, foodStamps.Code, foodStamps.Active, foodStamps.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbFoodStamps.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && foodStamps.Default.Equals("Y")) cmbFoodStamps.SelectedItem = li;
            }

            List<CommonEntity> WIC = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.WIC, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetWIC();
            //WIC = filterByHIE(WIC);
            cmbWIC.Items.Insert(0, new ListItem("Select One", "0"));
            cmbWIC.ColorMember = "FavoriteColor";
            cmbWIC.SelectedIndex = 0;
            foreach (CommonEntity wic in WIC)
            {
                ListItem li = new ListItem(wic.Desc, wic.Code, wic.Active, wic.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbWIC.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && wic.Default.Equals("Y")) cmbWIC.SelectedItem = li;
            }

            List<CommonEntity> Farmer = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.FARMER, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); ////_model.lookupDataAccess.GetFarmer();
            // Farmer = filterByHIE(Farmer);
            cmbFarmer.Items.Insert(0, new ListItem("Select One", "0"));
            cmbFarmer.ColorMember = "FavoriteColor";
            cmbFarmer.SelectedIndex = 0;
            foreach (CommonEntity farmer in Farmer)
            {
                ListItem li = new ListItem(farmer.Desc, farmer.Code, farmer.Active, farmer.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbFarmer.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && farmer.Default.Equals("Y")) cmbFarmer.SelectedItem = li;
            }

            List<CommonEntity> Disabled = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DISABLED, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); // _model.lookupDataAccess.GetDisabled();
            // Disabled = filterByHIE(Disabled);
            cmbDisabled.Items.Insert(0, new ListItem("Select One", "0"));
            cmbDisabled.ColorMember = "FavoriteColor";
            cmbDisabled.SelectedIndex = 0;
            foreach (CommonEntity disabled in Disabled)
            {
                ListItem li = new ListItem(disabled.Desc, disabled.Code, disabled.Active, disabled.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbDisabled.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && disabled.Default.Equals("Y")) cmbDisabled.SelectedItem = li;
            }

            List<CommonEntity> Driving = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DRIVERLICENSE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetDriving();
            // Driving = filterByHIE(Driving);
            cmbDriving.Items.Insert(0, new ListItem("Select One", "0"));
            cmbDriving.ColorMember = "FavoriteColor";
            cmbDriving.SelectedIndex = 0;
            foreach (CommonEntity driving in Driving)
            {
                ListItem li = new ListItem(driving.Desc, driving.Code, driving.Active, driving.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbDriving.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && driving.Default.Equals("Y")) cmbDriving.SelectedItem = li;
            }

            List<CommonEntity> ReliableTrans = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RELIABLETRANSPORTATION, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetReliableTrans();
            // ReliableTrans = filterByHIE(ReliableTrans);
            cmbReliableTrans.Items.Insert(0, new ListItem("Select One", "0"));
            cmbReliableTrans.ColorMember = "FavoriteColor";
            cmbReliableTrans.SelectedIndex = 0;
            foreach (CommonEntity reliableTrans in ReliableTrans)
            {
                ListItem li = new ListItem(reliableTrans.Desc, reliableTrans.Code, reliableTrans.Active, reliableTrans.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbReliableTrans.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && reliableTrans.Default.Equals("Y")) cmbReliableTrans.SelectedItem = li;
            }

        }

        private List<CommonEntity> filterByHIE(List<CommonEntity> LookupValues)
        {
            List<CommonEntity> _LookupValues = new List<CommonEntity>();
            _LookupValues = LookupValues;
            if (_LookupValues.Count > 0)
            {

                if (Mode.ToUpper() == "ADD")
                {
                    _LookupValues = _LookupValues.FindAll(u => (u.ListHierarchy.Contains(CaseMST.ApplAgency + CaseMST.ApplDept + CaseMST.ApplProgram) || u.ListHierarchy.Contains(CaseMST.ApplAgency + CaseMST.ApplDept + "**") || u.ListHierarchy.Contains(CaseMST.ApplAgency + "****") || u.ListHierarchy.Contains("******")) && u.Active.ToString().ToUpper() == "Y").ToList();
                }
                else
                {
                    _LookupValues = _LookupValues.FindAll(u => u.ListHierarchy.Contains(CaseMST.ApplAgency + CaseMST.ApplDept + CaseMST.ApplProgram) || u.ListHierarchy.Contains(CaseMST.ApplAgency + CaseMST.ApplDept + "**") || u.ListHierarchy.Contains(CaseMST.ApplAgency + "****") || u.ListHierarchy.Contains("******")).ToList();
                }

                _LookupValues = _LookupValues.OrderByDescending(u => u.Active).ThenBy(u => u.Desc).ToList();
            }
            return _LookupValues;

            //string HIE = CaseMST.ApplAgency + CaseMST.ApplDept + CaseMST.ApplProgram;
            ////if (LookupValues.Exists(u => u.Hierarchy.Equals(HIE)))
            ////    LookupValues = LookupValues.FindAll(u => u.Hierarchy.Equals(HIE)).ToList();
            ////else if (LookupValues.Exists(u => u.Hierarchy.Equals(CaseMST.ApplAgency + CaseMST.ApplDept + "**")))
            ////    LookupValues = LookupValues.FindAll(u => u.Hierarchy.Equals(CaseMST.ApplAgency + CaseMST.ApplDept + "**")).ToList();
            ////else if (LookupValues.Exists(u => u.Hierarchy.Equals(CaseMST.ApplAgency + "****")))
            ////    LookupValues = LookupValues.FindAll(u => u.Hierarchy.Equals(CaseMST.ApplAgency + "****")).ToList();
            ////else
            //LookupValues = LookupValues.FindAll(u => u.ListHierarchy.Contains(HIE) || u.ListHierarchy.Contains(CaseMST.ApplAgency + CaseMST.ApplDept + "**") || u.ListHierarchy.Contains(CaseMST.ApplAgency + "****") || u.ListHierarchy.Contains("******")).ToList();

            //return LookupValues;
        }

        private void fillMSTDropDowns()
        {
            List<CommonEntity> CaseType = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.CASETYPES, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); ////_model.lookupDataAccess.GetCaseType();
            // CaseType = filterByHIE(CaseType);
            cmbCaseType.Items.Insert(0, new ListItem("Select One", "0"));
            cmbCaseType.ColorMember = "FavoriteColor";
            cmbCaseType.SelectedIndex = 0;
            foreach (CommonEntity casetype in CaseType)
            {
                ListItem li = new ListItem(casetype.Desc, casetype.Code, casetype.Active, casetype.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbCaseType.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && casetype.Default.Equals("Y")) cmbCaseType.SelectedItem = li;
            }

            List<CommonEntity> Township = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.CITYTOWNTABLE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetTownship();
            //CaseType = filterByHIE(Township);
            cmbTownship.Items.Insert(0, new ListItem("Select One", "0"));
            cmbTownship.SelectedIndex = 0;
            cmbTownship.ColorMember = "FavoriteColor";
            foreach (CommonEntity township in Township)
            {
                ListItem li = new ListItem(township.Desc, township.Code, township.Active, township.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbTownship.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && township.Default.Equals("Y")) cmbTownship.SelectedItem = li;
            }


            List<CommonEntity> Country = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.COUNTY, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetCountry();
            //CaseType = filterByHIE(Country);
            cmbCounty.Items.Insert(0, new ListItem("Select One", "0"));
            cmbCounty.ColorMember = "FavoriteColor";
            cmbCounty.SelectedIndex = 0;
            foreach (CommonEntity country in Country)
            {
                ListItem li = new ListItem(country.Desc, country.Code, country.Active, country.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbCounty.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && country.Default.Equals("Y")) cmbCounty.SelectedItem = li;
            }

            List<CommonEntity> Housing = _model.lookupDataAccess.GetHousing(); //CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HOUSINGTYPES, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //
            Housing = filterByHIE(Housing);
            cmbHousingSituation.Items.Insert(0, new ListItem("Select One", "0"));
            cmbHousingSituation.ColorMember = "FavoriteColor";
            cmbHousingSituation.SelectedIndex = 0;
            foreach (CommonEntity housing in Housing)
            {
                ListItem li = new ListItem(housing.Desc, housing.Code, housing.Extension, housing.Active.Equals("Y") ? Color.Green : Color.Red, housing.Default, housing.Active);
                cmbHousingSituation.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && housing.Default.Equals("Y")) cmbHousingSituation.SelectedItem = li;
            }


            List<CommonEntity> PrimaryLanguage = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.LANGUAGECODES, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetPrimaryLanguage();
            //CaseType = filterByHIE(PrimaryLanguage);
            cmbPrimaryLang.Items.Insert(0, new ListItem("Select One", "0"));
            cmbPrimaryLang.ColorMember = "FavoriteColor";
            cmbPrimaryLang.SelectedIndex = 0;
            foreach (CommonEntity primarylanguage in PrimaryLanguage)
            {
                ListItem li = new ListItem(primarylanguage.Desc, primarylanguage.Code, primarylanguage.Active, primarylanguage.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbPrimaryLang.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && primarylanguage.Default.Equals("Y")) cmbPrimaryLang.SelectedItem = li;
            }

            List<CommonEntity> Secondarylanguage = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.LANGUAGECODES, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetSecondaryLanguage();
            // CaseType = filterByHIE(Secondarylanguage);
            cmbSecondLang.Items.Insert(0, new ListItem("Select One", "0"));
            cmbSecondLang.ColorMember = "FavoriteColor";
            cmbSecondLang.SelectedIndex = 0;
            foreach (CommonEntity secondlanguage in Secondarylanguage)
            {
                ListItem li = new ListItem(secondlanguage.Desc, secondlanguage.Code, secondlanguage.Active, secondlanguage.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbSecondLang.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && secondlanguage.Default.Equals("Y")) cmbSecondLang.SelectedItem = li;
            }

            //List<CommonEntity> FamilyType = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.FAMILYTYPE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetFamilyType();
            ////CaseType = filterByHIE(FamilyType);
            //cmbFamilyType.Items.Insert(0, new ListItem("Select One", "0"));
            cmbFamilyType.ColorMember = "FavoriteColor";
            //cmbFamilyType.SelectedIndex = 0;
            //foreach (CommonEntity familyType in FamilyType)
            //{
            //    ListItem li = new ListItem(familyType.Desc, familyType.Code, familyType.Active, familyType.Active.Equals("Y") ? Color.Green : Color.Red);
            //    cmbFamilyType.Items.Add(li);
            //    if (Mode.Equals(Consts.Common.Add) && familyType.Default.Equals("Y")) cmbFamilyType.SelectedItem = li;
            //}

            List<CommonEntity> FamilyType = _model.lookupDataAccess.GetAgyFamilyTypes();

            FamilyType = filterByHIE(FamilyType);
            cmbFamilyType.Items.Clear();
            cmbFamilyType.ColorMember = "FavoriteColor";
            cmbFamilyType.Items.Insert(0, new ListItem("Select One", "0"));
            cmbFamilyType.SelectedIndex = 0;
            foreach (CommonEntity familyType in FamilyType)
            {
                ListItem li = new ListItem(familyType.Desc, familyType.Code, familyType.Active, familyType.Default.ToString(), familyType.Extension.ToString(),familyType.AgyCode.ToString(), familyType.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbFamilyType.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && familyType.Default.Equals("Y")) cmbFamilyType.SelectedItem = li;

            }


            List<CommonEntity> contactyou = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.BESTWAYTOCONTACT, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.Getcontactyou();
            //CaseType = filterByHIE(contactyou);
            cmbContact.Items.Insert(0, new ListItem("Select One", "0"));
            cmbContact.ColorMember = "FavoriteColor";
            cmbContact.SelectedIndex = 0;
            foreach (CommonEntity Contactyou in contactyou)
            {
                ListItem li = new ListItem(Contactyou.Desc, Contactyou.Code, Contactyou.Active, Contactyou.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbContact.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && Contactyou.Default.Equals("Y")) cmbContact.SelectedItem = li;
            }

            List<CommonEntity> AboutUs = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HOWDIDYOUHEARABOUTTHEPROGRAM, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetaboutUs();
            //CaseType = filterByHIE(AboutUs);
            cmbAboutUs.Items.Insert(0, new ListItem("Select One", "0"));
            cmbAboutUs.ColorMember = "FavoriteColor";
            cmbAboutUs.SelectedIndex = 0;
            foreach (CommonEntity aboutUs in AboutUs)
            {
                ListItem li = new ListItem(aboutUs.Desc, aboutUs.Code, aboutUs.Active, aboutUs.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbAboutUs.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && aboutUs.Default.Equals("Y")) cmbAboutUs.SelectedItem = li;
            }

            string strNameFormat = string.Empty;
            string strCwFormat = string.Empty;
            //DataSet ds = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(CaseMST.ApplAgency, "**", "**");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            strNameFormat = BaseForm.BaseHierarchyCnFormat; //ds.Tables[0].Rows[0]["HIE_CN_FORMAT"].ToString();
            strCwFormat = BaseForm.BaseHierarchyCwFormat;//ds.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();
            // }

            cmbStaff.Items.Clear();
            cmbStaff.ColorMember = "FavoriteColor";
            List<HierarchyEntity> hierarchyEntity = _model.CaseMstData.GetCaseWorker(strCwFormat, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            if (Mode.ToUpper() == "ADD")
            {
                hierarchyEntity = hierarchyEntity.FindAll(u => u.InActiveFlag == "N").ToList();
            }
            string strCaseworker = string.Empty;
            foreach (HierarchyEntity caseworker in hierarchyEntity)
            {
                if (strCaseworker != caseworker.CaseWorker.ToString())
                {
                    strCaseworker = caseworker.CaseWorker.ToString();
                    cmbStaff.Items.Add(new ListItem(caseworker.HirarchyName.ToString(), caseworker.CaseWorker.ToString(), caseworker.InActiveFlag, caseworker.InActiveFlag.Equals("N") ? Color.Green : Color.Red));
                }
                if (caseworker.UserID.Trim().ToString().ToUpper() == BaseForm.UserID.ToUpper().Trim().ToString())
                {
                    strCaseWorkerDefaultCode = caseworker.CaseWorker == null ? "0" : caseworker.CaseWorker;
                    strCaseWorkerDefaultStartCode = caseworker.CaseWorker == null ? "0" : caseworker.CaseWorker;
                    strCaseworkerDesc = caseworker.HirarchyName.ToString();
                }

            }
            cmbStaff.Items.Insert(0, new ListItem(" ", "0"));
            if (Mode.Equals(Consts.Common.Add))
                SetComboBoxValue(cmbStaff, strCaseWorkerDefaultCode);
            else
                SetComboBoxValue(cmbStaff, "0");


            cmbVerifiedStaff.Items.Clear();
            cmbVerifiedStaff.ColorMember = "FavoriteColor";
            strCaseworker = string.Empty;
            foreach (HierarchyEntity caseworker in hierarchyEntity)
            {
                if (strCaseworker != caseworker.CaseWorker.ToString())
                {
                    strCaseworker = caseworker.CaseWorker.ToString();
                    cmbVerifiedStaff.Items.Add(new ListItem(caseworker.HirarchyName.ToString(), caseworker.CaseWorker.ToString(), caseworker.InActiveFlag, caseworker.InActiveFlag.Equals("N") ? Color.Green : Color.Red));
                }
                if (caseworker.UserID.Trim().ToString().ToUpper() == BaseForm.UserID.ToUpper().Trim().ToString())
                {
                    strCaseWorkerDefaultCode = caseworker.CaseWorker == null ? "0" : caseworker.CaseWorker;
                    strCaseWorkerDefaultStartCode = caseworker.CaseWorker == null ? "0" : caseworker.CaseWorker;
                    strCaseworkerDesc = caseworker.HirarchyName.ToString();
                }

            }
            cmbVerifiedStaff.Items.Insert(0, new ListItem("Select One", "0"));
            if (Mode.Equals(Consts.Common.Add))
                SetComboBoxValue(cmbVerifiedStaff, strCaseWorkerDefaultCode);
            else
                SetComboBoxValue(cmbVerifiedStaff, "0");

            //AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
            //searchAgytabs.Tabs_Type = "S0030";
            //AdhocData AgyTabs = new AdhocData();
            //List<AGYTABSEntity> SubsizedHouse = AgyTabs.Browse_AGYTABS(searchAgytabs);

            List<CommonEntity> SubsizedHouse = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0030", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetAgyTabRecordsByCode(Consts.AgyTab.Subsidized_Housing_Type);
            //SubsizedHouse = filterByHIE(SubsizedHouse);
            cmbSubsidized.Items.Insert(0, new ListItem("Select One", "0"));
            cmbSubsidized.ColorMember = "FavoriteColor";
            cmbSubsidized.SelectedIndex = 0;
            foreach (CommonEntity SubsizedH in SubsizedHouse)
            {
                ListItem li = new ListItem(SubsizedH.Desc, SubsizedH.Code);
                cmbSubsidized.Items.Add(li);

            }

            List<CommonEntity> DwellingType = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DWELLINGTYPE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetAgyTabRecordsByCode(Consts.AgyTab.DWELLINGTYPE);
            //DwellingType = filterByHIE(DwellingType);
            cmbDwelling.Items.Insert(0, new ListItem("Select One", "0"));
            cmbDwelling.ColorMember = "FavoriteColor";
            cmbDwelling.SelectedIndex = 0;
            foreach (CommonEntity Dwellingitems in DwellingType)
            {
                ListItem li = new ListItem(Dwellingitems.Desc, Dwellingitems.Code, Dwellingitems.Active, Dwellingitems.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbDwelling.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && Dwellingitems.Default.Equals("Y")) cmbDwelling.SelectedItem = li;
            }

            List<CommonEntity> PrimarySourceHeat = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HEATSOURCE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetAgyTabRecordsByCode(Consts.AgyTab.HEATSOURCE);
            // PrimarySourceHeat = filterByHIE(PrimarySourceHeat);
            cmbPrimarySourceoHeat.Items.Insert(0, new ListItem("Select One", "0"));
            cmbPrimarySourceoHeat.ColorMember = "FavoriteColor";
            cmbPrimarySourceoHeat.SelectedIndex = 0;
            foreach (CommonEntity PrimarySourceHeatItems in PrimarySourceHeat)
            {
                ListItem li = new ListItem(PrimarySourceHeatItems.Desc, PrimarySourceHeatItems.Code, PrimarySourceHeatItems.Active, PrimarySourceHeatItems.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbPrimarySourceoHeat.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && PrimarySourceHeatItems.Default.Equals("Y")) cmbPrimarySourceoHeat.SelectedItem = li;
            }

            List<CommonEntity> PrimaryMethodofHeat = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.METHODOFPAYINGFORHEAT, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetAgyTabRecordsByCode(Consts.AgyTab.METHODOFPAYINGFORHEAT);
            // PrimaryMethodofHeat = filterByHIE(PrimaryMethodofHeat);
            cmbPMOPfHeat.Items.Insert(0, new ListItem("Select One", "0"));
            cmbPMOPfHeat.ColorMember = "FavoriteColor";
            cmbPMOPfHeat.SelectedIndex = 0;
            foreach (CommonEntity PrimaryMethodofHeatItems in PrimaryMethodofHeat)
            {
                ListItem li = new ListItem(PrimaryMethodofHeatItems.Desc, PrimaryMethodofHeatItems.Code, PrimaryMethodofHeatItems.Active, PrimaryMethodofHeatItems.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbPMOPfHeat.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && PrimaryMethodofHeatItems.Default.Equals("Y")) cmbPMOPfHeat.SelectedItem = li;
            }

            List<ListItem> listItem = new List<ListItem>();
            cmbQuestionType.Items.Clear();
            listItem = new List<ListItem>();
            listItem.Add(new ListItem("All", "ALL"));
            listItem.Add(new ListItem("Active", "A"));
            listItem.Add(new ListItem("Inactive", "I"));
            cmbQuestionType.Items.AddRange(listItem.ToArray());
            cmbQuestionType.SelectedIndex = 1;

            if (BaseForm.BusinessModuleID == "08")
            {

                cmbEnergydata.Visible = true;
                dtApplicationType.Visible = true;
                lblApplicationtype.Visible = true;
                lblreqApplicationType.Visible = true;
                lblReqAppdt.Visible = true;
                listItem = new List<ListItem>();
                cmbEnergydata.Items.Clear();
                List<CommonEntity> AgyTabs_List = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0080", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.AdhocData.Browse_AGYTABS(searchAgytabs);
                foreach (CommonEntity item in AgyTabs_List)
                {
                    cmbEnergydata.Items.Add(new ListItem(item.Desc, item.Code));
                }
                cmbEnergydata.Items.Insert(0, new ListItem("Select One", "0"));
                cmbEnergydata.SelectedIndex = 0;


            }





            //List<ListItem> listItem = new List<ListItem>();
            //DataSet cwDataSet = Captain.DatabaseLayer.CaseMst.GetCaseWorker(strCwFormat, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            //DataTable dt = cwDataSet.Tables[0];
            //if (dt.Rows.Count > 0)
            //{
            //    string strcaseworkerId
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        if(dr[""].ToString()!=
            //        listItem.Add(new ListItem(dr["NAME"].ToString(), dr["PWH_CASEWORKER"].ToString()));
            //    }
            //}
            //cmbStaff.Items.AddRange(listItem.ToArray());
            //cmbStaff.Items.Insert(0, new ListItem("Select One", "0"));
            //cmbStaff.SelectedIndex = 0;
            // cmbVerifiedStaff.Items.AddRange(listItem.ToArray());
            // cmbVerifiedStaff.Items.Insert(0, new ListItem("Select One", "0"));
            //cmbVerifiedStaff.SelectedIndexChanged -= calcTotalExpenses;
            //cmbVerifiedStaff.SelectedIndex = 0;
            //cmbVerifiedStaff.SelectedIndexChanged += calcTotalExpenses;

            #region Preassestabcombofilling

            List<CommonEntity> CRECVDSSEntity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTabs.RECVDSS, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "View"); //_model.lookupDataAccess.GetAgyTabRecordsByCode(Consts.AgyTab.METHODOFPAYINGFORHEAT);

            //cmbPDSS2.Items.Insert(0, new ListItem("Select One", "0"));
            //cmbPDSS2.ColorMember = "FavoriteColor";
            //cmbPDSS2.SelectedIndex = 0;
            //foreach (CommonEntity CRECVdssItems in CRECVDSSEntity)
            //{
            //    ListItem li = new ListItem(CRECVdssItems.Desc, CRECVdssItems.Code, CRECVdssItems.Active, CRECVdssItems.Active.Equals("Y") ? Color.Green : Color.Red);
            //    cmbPDSS2.Items.Add(li);
            //    if (Mode.Equals(Consts.Common.Add) && CRECVdssItems.Default.Equals("Y")) cmbPDSS2.SelectedItem = li;
            //}

            #endregion


        }

        private void fillCaseSnpDetails(string agency, string dep, string program, string year, string app, string seq)
        {
            CaseSnpEntity CaseSnpDetails = _model.CaseMstData.GetCaseSnpDetails(agency, dep, program, year, app, seq);
            CaseSNP = CaseSnpDetails;
            if (CaseSnpDetails != null)
            {
                mskSSN.Text = CaseSnpDetails.Ssno;
                txtFirstName.Text = CaseSnpDetails.NameixFi.Trim();
                txtLastName.Text = CaseSnpDetails.NameixLast.Trim();
                txtMI.Text = CaseSnpDetails.NameixMi.Trim();
                txtAppSuffix.Text = CaseSnpDetails.SnpSuffix.Trim();
                txtBIC.Text = CaseSnpDetails.SsBic.Trim();
                txtAlias.Text = CaseSnpDetails.Alias.Trim();
                if (!CaseSnpDetails.AltBdate.Equals(string.Empty))
                {
                    dtBirth.Text = CaseSnpDetails.AltBdate;
                    dtBirth.Checked = true;
                    txtCurrentAge.Text = CommonFunctions.CalculationYear(Convert.ToDateTime(dtBirth.Text.Trim()), Convert.ToDateTime(DateTime.Now.Date.ToShortDateString()));
                }
                else
                {
                    dtBirth.Value = DateTime.Now.Date;
                    dtBirth.Checked = false;
                }
                txtYear.Text = CaseSnpDetails.Age.Trim();

                txtAlienRegNo.Text = CaseSnpDetails.AlienRegNo.Trim();

                if (!CaseSnpDetails.ExpireWorkDate.Equals(string.Empty))
                {
                    dtExpirationdate.Text = CaseSnpDetails.ExpireWorkDate;
                }
                else
                {
                    dtExpirationdate.Value = DateTime.Now.Date;
                    dtExpirationdate.Checked = false;
                }

                if (CaseSnpDetails.DobNa.Equals("1"))
                {
                    checkNA.Checked = true;
                }
                if (!Mode.Equals("Add"))
                {
                    if (CaseSnpDetails.Status.Trim().Equals("A"))
                    {
                        ckActive.Checked = true;
                        ckExcludeMember.Enabled = true;
                    }
                    else
                    {
                        ckExcludeMember.Enabled = false;
                    }
                }
                else
                {
                    ckActive.Checked = true;
                    ckExcludeMember.Enabled = true;
                }
                ckActive_Click(ckActive, new EventArgs());
                if (CaseSnpDetails.Exclude.Trim().Equals("Y"))
                {
                    ckExcludeMember.Checked = true;
                }
                strRelationCode = "Primary";
                SetComboBoxValue(cmbRelation, CaseSnpDetails.MemberCode);
                strRelationCode = string.Empty;
                SetComboBoxValue(cmbEthnicity, CaseSnpDetails.Ethnic);
                SetComboBoxValue(cmbRace, CaseSnpDetails.Race);
                SetComboBoxValue(cmbEducation, CaseSnpDetails.Education);

                SetComboBoxValue(cmbSchool, CaseSnpDetails.SchoolDistrict);
                SetComboBoxValue(cmbGender, CaseSnpDetails.Sex);
                SetComboBoxValue(cmbAreyoupregnant, CaseSnpDetails.Pregnant);
                SetComboBoxValue(cmbMaritalStatus, CaseSnpDetails.MaritalStatus);

                SetComboBoxValue(cmbHealthInsurance, CaseSnpDetails.HealthIns);
                SetComboBoxValue(cmbVeteranCode, CaseSnpDetails.Vet);
                SetComboBoxValue(cmbFoodStamps, CaseSnpDetails.FootStamps);
                SetComboBoxValue(cmbWIC, CaseSnpDetails.Wic);

                SetComboBoxValue(cmbFarmer, CaseSnpDetails.Farmer);
                SetComboBoxValue(cmbDisabled, CaseSnpDetails.Disable);
                SetComboBoxValue(cmbDriving, CaseSnpDetails.Drvlic);
                SetComboBoxValue(cmbReliableTrans, CaseSnpDetails.Relitran);

                SetComboBoxValue(cmbResident, CaseSnpDetails.Resident);
                SetComboBoxValue(cmbLegaltowork, CaseSnpDetails.LegalTowork);

                //************** Employee Details *****************//
                SetComboBoxValue(cmbEpresenteEmploy, CaseSnpDetails.Employed);
                if (!CaseSnpDetails.LastWorkDate.Equals(string.Empty))
                {
                    dtElastDateWorked.Text = CaseSnpDetails.LastWorkDate;
                    dtElastDateWorked.Checked = true;
                }
                else
                {
                    dtElastDateWorked.Value = DateTime.Now.Date;
                    dtElastDateWorked.Checked = false;
                }
                SetComboBoxValue(cmbEAnywork, CaseSnpDetails.WorkLimit);
                txtEifyesexplain.Text = CaseSnpDetails.ExplainWorkLimit;
                txtEcurrentHave.Text = CaseSnpDetails.NumberOfcjobs;
                txtElastvisit.Text = CaseSnpDetails.NumberofLvjobs;
                txtEFullTime.Text = CaseSnpDetails.FullTimeHours;
                txtEPartTime.Text = CaseSnpDetails.PartTimeHours;
                SetComboBoxValue(cmbEseasonalEmployee, CaseSnpDetails.SeasonalEmploy);
                if (CaseSnpDetails.IstShift.Trim().Equals("Y"))
                    chkE1st.Checked = true;
                if (CaseSnpDetails.IIndShift.Trim().Equals("Y"))
                    chkE2nd.Checked = true;
                if (CaseSnpDetails.IIIrdShift.Trim().Equals("Y"))
                    chkE3rd.Checked = true;
                if (CaseSnpDetails.RShift.Trim().Equals("Y"))
                    chkErotaing.Checked = true;
                txtEEmployer.Text = CaseSnpDetails.EmployerName;
                txtEstreet.Text = CaseSnpDetails.EmployerStreet;
                txtEcityState.Text = CaseSnpDetails.EmployerCity;
                mskEPhone.Text = CaseSnpDetails.EmplPhone;
                txtEExt.Text = CaseSnpDetails.EmplExt;
                SetComboBoxValue(cmbEJobTitle, CaseSnpDetails.JobTitle);
                SetComboBoxValue(cmbEJobCategory, CaseSnpDetails.JobCategory);
                txtEhourlywage.Text = CaseSnpDetails.HourlyWage;
                SetComboBoxValue(cmbEpayFrequency, CaseSnpDetails.PayFrequency);
                if (!CaseSnpDetails.HireDate.Equals(string.Empty))
                {
                    dtEHireDate.Text = CaseSnpDetails.HireDate;
                    dtEHireDate.Checked = true;
                }
                else
                {
                    dtEHireDate.Value = DateTime.Now.Date;
                    dtEHireDate.Checked = false;
                }

            }

        }

        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (ListItem li in comboBox.Items)
                {
                    if (li.Value.Equals(value) || li.Text.Equals(value))
                    {
                        comboBox.SelectedItem = li;
                        break;
                    }
                }
            }
        }

        private void calcTotalExpenses(object sender, EventArgs e)
        {
            //if (((ListItem)cmbVerifiedStaff.SelectedItem).Value.ToString() != "0")
            //{
            //    if (((ListItem)cmbVerifiedStaff.SelectedItem).ID.ToString() != "Y")
            //        MessageBox.Show("Inactive CaseWorker", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //}
            CalclationEmployee();

        }

        public void CalclationEmployee()
        {
            double rent = txtRent.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtRent.Text);
            double water = txtWater.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtWater.Text);
            double electric = txtElectric.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtElectric.Text);
            double heating = txtHeating.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtHeating.Text);
            double expand1 = txtExpand1.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtExpand1.Text);
            double expand2 = txtExpand2.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtExpand2.Text);
            double expand3 = txtExpand3.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtExpand3.Text);
            double expand4 = txtExpand4.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtExpand4.Text);
            double miscExpenses = txtMiscExpenses.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtMiscExpenses.Text);
            double miscDebt = txtMiscDebt.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtMiscDebt.Text);
            double miscAsset = txtMiscAssets.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtMiscAssets.Text);
            double physcialAsset = txtPhysicalAssets.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtPhysicalAssets.Text);
            double OtherAsset = txtOtherAssets.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtOtherAssets.Text);
            double LiquidAsset = txtLiquidAssets.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtLiquidAssets.Text);




            //txtTotalAssets.Text = caseMSTEntity.AsetTotal;
            //txtDebtAssetRatio.Text = caseMSTEntity.AsetRatio;           
            //txtDebtIncomeRatio.Text = string.Empty;
            //txtTotalHouseholdIncome.Text = string.Empty;

            double TotalDebt = expand1 + expand2 + expand3 + expand4 + miscDebt;
            txtTotalHHDebt.Text = TotalDebt.ToString();

            double TotalAssets = physcialAsset + OtherAsset + LiquidAsset;//miscAsset
            txtTotalAssets.Text = TotalAssets.ToString();

            double total = rent + water + electric + heating + miscExpenses;
            txtTotalHouseHold.Text = total.ToString();
            double TotalAllIncome = 0;
            double MonthlyIncome = 0;
            txtMonthlyIncome.Text = "0";
            txtTotalLiving.Text = "0";

            if (TotalAssets > 0)
            {
                txtDebtAssetRatio.Text = Math.Round((TotalDebt / TotalAssets), 2).ToString();
            }
            else
                txtDebtAssetRatio.Text = "0.00";

            if (CaseMST != null)
            {
                if (SearchSnpType == "SSNSEARCH")
                {
                    MonthlyIncome = double.Parse(strProgIncome);
                }
                else
                {
                    if (CaseMST.ProgIncome.ToString() != string.Empty)
                    {
                        MonthlyIncome = double.Parse(CaseMST.ProgIncome);
                    }
                }

                if (MonthlyIncome > 0)
                {

                    TotalAllIncome = MonthlyIncome;
                    txtDebtIncomeRatio.Text = Math.Round((TotalDebt / TotalAllIncome), 2).ToString();
                    txtTotalHouseholdIncome.Text = MonthlyIncome.ToString();
                    MonthlyIncome = MonthlyIncome / 12;
                    MonthlyIncome = Math.Round(MonthlyIncome, 2);
                    txtMonthlyIncome.Text = MonthlyIncome.ToString();
                    if (!((ListItem)cmbVerifiedStaff.SelectedItem).Value.ToString().Equals("0"))
                    {
                        double totLiveExp = 0;
                        if (total > 0)
                        {
                            totLiveExp = ((total / MonthlyIncome) * 1000) + 0.5;
                            totLiveExp = Math.Round(totLiveExp);
                            totLiveExp = totLiveExp / 10;
                        }
                        txtTotalLiving.Text = totLiveExp.ToString();
                    }
                }
                else
                    txtDebtIncomeRatio.Text = "0.00";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!isValidate()) return;
            bool boolcheckaddress = true;
            if (IsApplicant)
            {
                if (ProgramDefinition.DepAddressEdit.ToUpper() == "Y")
                {
                    if (((ListItem)cmbOutService.SelectedItem).Value.ToString() == "I")
                    {
                        boolcheckaddress = checkAddress("Save");
                    }
                    else if (((ListItem)cmbOutService.SelectedItem).Value.ToString() == "0")
                    {
                        if (propAgencyControlDetails.State.ToUpper() == "TX")
                        {
                            List<CASEVOTEntity> casevotdata = propcaseVot.FindAll(u => u.Street.ToUpper() == txtStreet.Text.ToUpper());
                            if (casevotdata.Count > 0)
                            {
                                _errorProvider.SetError(cmbOutService, null);
                            }
                            else
                            {
                                _errorProvider.SetError(cmbOutService, "Please Select Service");
                                boolcheckaddress = false;
                                tabControl1.SelectedIndex = 1;
                            }
                        }
                    }
                }
            }
            if (CheckSSNDetails())
            {
                if (boolcheckaddress)
                {
                    if (CheckSiteDetails())
                    {
                        if (CheckZipcodeDetails())
                        {
                            if (CheckRealtions())
                            {
                                if (CheckRelationsMultiple())
                                {
                                    btnSave.Enabled = false;
                                    CaseMstEntity CaseMstHistEntity = new CaseMstEntity();
                                    CaseMstEntity CaseMstDeleteEntity = new CaseMstEntity();
                                    bool boolSnpInsert = true;
                                    if (IsApplicant)
                                    {
                                        if ((ckExcludeMember.Checked && Mode.Equals(Consts.Common.Edit) && !IsExclude) || (chkExcludeHHMem.Checked && Mode.Equals(Consts.Common.Edit) && !IsExclude))
                                        {
                                            MessageBox.Show("Are you sure you want to exclude the primary applicant?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
                                            IsExclude = true;
                                            return;
                                        }
                                        bool boolDepApplicantNocheck = true;
                                        // Check MaxApplicantNo CaseDep table
                                        if (ProgramDefinition.DepGenerateApps.Trim() == "Y")
                                        {

                                            long CaseMstMaxApplNo = _model.CaseMstData.GetMaxApplicantNo(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);

                                            if (!ProgramDefinition.DepAppNo.Equals(string.Empty))
                                            {
                                                boolDepApplicantNocheck = false;
                                                if (CaseMstMaxApplNo < Convert.ToInt64(ProgramDefinition.DepAppNo))
                                                {
                                                    boolDepApplicantNocheck = true;
                                                }
                                                else
                                                {
                                                    CaseMstMaxApplNo = CaseMstMaxApplNo + 1;
                                                    ProgramDefinitionEntity programEntity = new ProgramDefinitionEntity();
                                                    programEntity.Agency = BaseForm.BaseAgency;
                                                    programEntity.Dept = BaseForm.BaseDept;
                                                    programEntity.Prog = BaseForm.BaseProg;
                                                    programEntity.DepAppNo = CaseMstMaxApplNo.ToString();
                                                    programEntity.Mode = "APPNO";
                                                    if (_model.HierarchyAndPrograms.InsertCaseDep(programEntity))
                                                    {
                                                        boolDepApplicantNocheck = true;
                                                    }
                                                }
                                            }

                                        }

                                        CaseMstEntity CaseMstEntity = new CaseMstEntity();
                                        boolSnpInsert = false;

                                        CaseMstEntity.ApplAgency = CaseMST.ApplAgency;
                                        CaseMstEntity.ApplDept = CaseMST.ApplDept;
                                        CaseMstEntity.ApplProgram = CaseMST.ApplProgram;
                                        if (CaseMST.ApplYr == string.Empty)
                                            CaseMstEntity.ApplYr = "    ";
                                        else
                                            CaseMstEntity.ApplYr = CaseMST.ApplYr;
                                        CaseMstEntity.ApplNo = CaseMST.ApplNo;
                                        CaseMstEntity.FamilySeq = "1";
                                        // CaseMstEntity.FamilyId = "1";
                                        // CaseMstEntity.ClientId = "1";
                                        CaseMstEntity.Ssn = mskSSN.Text.ToUpper();
                                        CaseMstEntity.Bic = txtBIC.Text.Trim();
                                        CaseMstEntity.NickName = txtAlias.Text;
                                        CaseMstEntity.EthnicOther = "";
                                        CaseMstEntity.State = txtState.Text;
                                        CaseMstEntity.City = txtCity.Text;
                                        CaseMstEntity.Street = txtStreet.Text;
                                        CaseMstEntity.Suffix = txtSuffix.Text;
                                        CaseMstEntity.Hn = txtHN.Text;
                                        CaseMstEntity.Direction = txtDirection.Text;
                                        CaseMstEntity.Apt = txtApartment.Text;
                                        CaseMstEntity.Flr = txtFloor.Text;
                                        CaseMstEntity.Zip = txtZipCode.Text.Trim().Equals(string.Empty) ? string.Empty : txtZipCode.Text;
                                        CaseMstEntity.Zipplus = txtZipPlus.Text.Trim().Equals(string.Empty) ? string.Empty : txtZipPlus.Text; ;
                                        CaseMstEntity.Precinct = txtPrecinct.Text.Trim();

                                        CaseMstEntity.Area = txtHomePhone.Text.Trim().Length >= 3 ? txtHomePhone.Text.Trim().Substring(0, 3) : string.Empty;
                                        CaseMstEntity.Phone = txtHomePhone.Text.Trim().Length > 3 ? txtHomePhone.Text.Trim().Substring(3) : string.Empty;
                                        CaseMstEntity.NextYear = "N";
                                        if (Mode.Equals(Consts.Common.Add))
                                            CaseMstEntity.Classification = "00";
                                        if (!((ListItem)cmbPrimaryLang.SelectedItem).Value.ToString().Equals("0"))
                                            CaseMstEntity.Language = ((ListItem)cmbPrimaryLang.SelectedItem).Value.ToString();
                                        if (!((ListItem)cmbSecondLang.SelectedItem).Value.ToString().Equals("0"))
                                            CaseMstEntity.LanguageOt = ((ListItem)cmbSecondLang.SelectedItem).Value.ToString();
                                        if (!((ListItem)cmbStaff.SelectedItem).Value.ToString().Equals("0"))
                                            CaseMstEntity.IntakeWorker = ((ListItem)cmbStaff.SelectedItem).Value.ToString();
                                        if (dtpIntakeDate.Checked == true)
                                            CaseMstEntity.IntakeDate = dtpIntakeDate.Value.ToString();
                                        if (dtpInitialDate.Checked == true)
                                            CaseMstEntity.InitialDate = dtpInitialDate.Value.ToString();
                                        if (!((ListItem)cmbCaseType.SelectedItem).Value.ToString().Equals("0"))
                                            CaseMstEntity.CaseType = ((ListItem)cmbCaseType.SelectedItem).Value.ToString();
                                        if (!((ListItem)cmbHousingSituation.SelectedItem).Value.ToString().Equals("0"))
                                            CaseMstEntity.Housing = ((ListItem)cmbHousingSituation.SelectedItem).Value.ToString();
                                        if (!((ListItem)cmbFamilyType.SelectedItem).Value.ToString().Equals("0"))
                                            CaseMstEntity.FamilyType = ((ListItem)cmbFamilyType.SelectedItem).Value.ToString(); ;

                                        if (!((ListItem)cmbDwelling.SelectedItem).Value.ToString().Equals("0"))
                                            CaseMstEntity.Dwelling = ((ListItem)cmbDwelling.SelectedItem).Value.ToString();

                                        if (!((ListItem)cmbPMOPfHeat.SelectedItem).Value.ToString().Equals("0"))
                                            CaseMstEntity.HeatIncRent = ((ListItem)cmbPMOPfHeat.SelectedItem).Value.ToString();

                                        if (!((ListItem)cmbPrimarySourceoHeat.SelectedItem).Value.ToString().Equals("0"))
                                            CaseMstEntity.Source = ((ListItem)cmbPrimarySourceoHeat.SelectedItem).Value.ToString();


                                        if (!((ListItem)cmbSubsidized.SelectedItem).Value.ToString().Equals("0"))
                                            CaseMstEntity.SubStype = ((ListItem)cmbSubsidized.SelectedItem).Value.ToString();

                                        CaseMstEntity.Site = txtSite.Text;

                                        CaseMstEntity.SubShouse = "N";
                                        if (chkSubsidized.Checked)
                                            CaseMstEntity.SubShouse = "Y";

                                        if (cbSecret.Checked == true)
                                            CaseMstEntity.Secret = "Y";
                                        else
                                            CaseMstEntity.Secret = "N";
                                        if (dtpCaseReview.Checked == true)
                                            CaseMstEntity.CaseReviewDate = dtpCaseReview.Value.ToString();

                                        CaseMstEntity.ParentStatus = "";
                                        CaseMstEntity.IntakeHrs = "";
                                        CaseMstEntity.IntakeMns = "";
                                        CaseMstEntity.IntakeScs = "";
                                        if (!((ListItem)cmbTownship.SelectedItem).Value.ToString().Equals("0"))
                                            CaseMstEntity.TownShip = ((ListItem)cmbTownship.SelectedItem).Value.ToString(); ;
                                        CaseMstEntity.SsnFlag = "N";
                                        if (!((ListItem)cmbWaitingList.SelectedItem).Value.ToString().Equals("0"))
                                            CaseMstEntity.WaitList = ((ListItem)cmbWaitingList.SelectedItem).Value.ToString(); ;
                                        CaseMstEntity.IncomeTypes = string.Empty;
                                        CaseMstEntity.Poverty = "0";
                                        CaseMstEntity.TotalRank = "0";
                                        CaseMstEntity.Hud = "0";
                                        CaseMstEntity.Smi = "0";
                                        CaseMstEntity.Cmi = "0";

                                        CaseMstEntity.ActiveStatus = cbActive.Checked ? "Y" : "N";
                                        CaseMstEntity.ProgIncome = txtMonthlyIncome.Text.Trim().Length > 0 ? txtMonthlyIncome.Text.Trim() : string.Empty;
                                        CaseMstEntity.OutOfService = "";
                                        if (propAgencyControlDetails.State.ToUpper() == "TX")
                                        {
                                            if (!((ListItem)cmbOutService.SelectedItem).Value.ToString().Equals("0"))
                                                CaseMstEntity.OutOfService = ((ListItem)cmbOutService.SelectedItem).Value.ToString();
                                        }


                                        CaseMstEntity.AddressYears = txtNoOfYearsAtAddress.Text.Trim().Length > 0 ? txtNoOfYearsAtAddress.Text.Trim() : "0";
                                        if (txtMessage.Text.Length > 0)
                                        {
                                            txtMessage.Text = txtMessage.Text.Replace(' ', '0') + SsnZeros(txtMessage.TextLength, string.Empty);
                                            CaseMstEntity.MessagePhone = txtMessage.Text;
                                        }
                                        if (txtCell.Text.Length > 0)
                                        {
                                            txtCell.Text = txtCell.Text.Replace(' ', '0') + SsnZeros(txtCell.TextLength, string.Empty);
                                            CaseMstEntity.CellPhone = txtCell.Text;
                                        }
                                        if (txtFax.Text.Length > 0)
                                        {
                                            txtFax.Text = txtFax.Text.Replace(' ', '0') + SsnZeros(txtFax.TextLength, string.Empty);
                                            CaseMstEntity.FaxNumber = txtFax.Text;
                                        }
                                        if (txtTTY.Text.Length > 0)
                                        {
                                            txtTTY.Text = txtTTY.Text.Replace(' ', '0') + SsnZeros(txtTTY.TextLength, string.Empty);
                                            CaseMstEntity.TtyNumber = txtTTY.Text;
                                        }
                                        CaseMstEntity.HomePhone_NA = ChkHome_Na.Checked == true ? "Y" : "N";
                                        CaseMstEntity.CellPhone_NA = chk_CellNa.Checked == true ? "Y" : "N";
                                        CaseMstEntity.MessagePhone_NA = chkMessage_Na.Checked == true ? "Y" : "N";
                                        CaseMstEntity.Email_NA = chkEmail_Na.Checked == true ? "Y" : "N";

                                        CaseMstEntity.Email = txtEmail.Text;
                                        if (!((ListItem)cmbContact.SelectedItem).Value.ToString().Equals("0"))
                                            CaseMstEntity.BestContact = ((ListItem)cmbContact.SelectedItem).Value.ToString();
                                        if (!((ListItem)cmbAboutUs.SelectedItem).Value.ToString().Equals("0"))
                                            CaseMstEntity.AboutUs = ((ListItem)cmbAboutUs.SelectedItem).Value.ToString();
                                        CaseMstEntity.ImportDate = string.Empty;
                                        CaseMstEntity.DateAdded = "";
                                        CaseMstEntity.ExpRent = txtRent.Text.Trim().Length > 0 ? txtRent.Text.Trim() : "0";
                                        CaseMstEntity.ExpWater = txtWater.Text.Trim().Length > 0 ? txtWater.Text.Trim() : "0";
                                        CaseMstEntity.ExpElectric = txtElectric.Text.Trim().Length > 0 ? txtElectric.Text.Trim() : "0";
                                        CaseMstEntity.ExpHeat = txtHeating.Text.Trim().Length > 0 ? txtHeating.Text.Trim() : "0";
                                        CaseMstEntity.Debtcc = txtExpand1.Text.Trim().Length > 0 ? txtExpand1.Text.Trim() : "0";
                                        CaseMstEntity.DebtLoans = txtExpand2.Text.Trim().Length > 0 ? txtExpand2.Text.Trim() : "0";
                                        CaseMstEntity.DebtMed = txtExpand3.Text.Trim().Length > 0 ? txtExpand3.Text.Trim() : "0";
                                        CaseMstEntity.DebtOther = txtExpand4.Text.Trim().Length > 0 ? txtExpand4.Text.Trim() : "0";
                                        CaseMstEntity.ExpTotal = txtTotalHouseHold.Text.Length > 0 ? txtTotalHouseHold.Text.Trim() : "0";
                                        CaseMstEntity.ExpLivexpense = txtTotalLiving.Text.Length > 0 ? txtTotalLiving.Text.Trim() : "0";

                                        CaseMstEntity.ExpMisc = txtMiscExpenses.Text.Trim().Length > 0 ? txtMiscExpenses.Text.Trim() : "0";
                                        CaseMstEntity.DebtMisc = txtMiscDebt.Text.Trim().Length > 0 ? txtMiscDebt.Text.Trim() : "0";
                                        CaseMstEntity.AsetMisc = txtMiscAssets.Text.Trim().Length > 0 ? txtMiscAssets.Text.Trim() : "0";
                                        CaseMstEntity.AsetPhy = txtPhysicalAssets.Text.Length > 0 ? txtPhysicalAssets.Text.Trim() : "0";
                                        CaseMstEntity.AsetOth = txtOtherAssets.Text.Trim().Length > 0 ? txtOtherAssets.Text.Trim() : "0";
                                        CaseMstEntity.AsetLiq = txtLiquidAssets.Text.Trim().Length > 0 ? txtLiquidAssets.Text.Trim() : "0";
                                        CaseMstEntity.AsetTotal = txtTotalAssets.Text.Trim().Length > 0 ? txtTotalAssets.Text.Trim() : "0";
                                        CaseMstEntity.DebtTotal = txtTotalHHDebt.Text.Trim().Length > 0 ? txtTotalHHDebt.Text.Trim() : "0";
                                        CaseMstEntity.DebIncmRation = txtDebtIncomeRatio.Text.Trim().Length > 0 ? txtDebtIncomeRatio.Text.Trim() : "0";
                                        CaseMstEntity.AsetRatio = txtDebtAssetRatio.Text.Trim().Length > 0 ? txtDebtAssetRatio.Text.Trim() : "0";

                                        if (BaseForm.BusinessModuleID.ToString() == "08")
                                        {
                                            if (!((ListItem)cmbEnergydata.SelectedItem).Value.ToString().Equals("0"))
                                                CaseMstEntity.ApplictionType = ((ListItem)cmbEnergydata.SelectedItem).Value.ToString();
                                            if (dtApplicationType.Checked)
                                                CaseMstEntity.ApplictionDate = dtApplicationType.Value.ToShortDateString();
                                        }
                                        else
                                        {
                                            CaseMstEntity.ApplictionType = CaseMST.ApplictionType;
                                            CaseMstEntity.ApplictionDate = CaseMST.ApplictionDate;
                                        }


                                        //txtTotalAssets.Text = caseMSTEntity.AsetTotal;
                                        //txtDebtAssetRatio.Text = caseMSTEntity.AsetRatio;           
                                        //txtDebtIncomeRatio.Text = string.Empty;
                                        //txtTotalHouseholdIncome.Text = string.Empty;



                                        if (Mode.Equals("Add"))
                                        {
                                            CaseMstEntity.CaseManager = strCaseWorkerDefaultCode == "0" ? string.Empty : strCaseWorkerDefaultCode;
                                            CaseMstEntity.ExpCaseWorker = strCaseWorkerDefaultCode == "0" ? string.Empty : strCaseWorkerDefaultCode;
                                        }

                                        if (Mode.Equals(Consts.Common.Edit))
                                        {
                                            CaseMstEntity.AlertCodes = CaseMST.AlertCodes;
                                            if (!((ListItem)cmbVerifiedStaff.SelectedItem).Value.ToString().Equals("0"))
                                            {
                                                CaseMstEntity.CaseManager = ((ListItem)cmbVerifiedStaff.SelectedItem).Value.ToString();
                                                CaseMstEntity.ExpCaseWorker = ((ListItem)cmbVerifiedStaff.SelectedItem).Value.ToString();
                                            }
                                            else
                                            {
                                                CaseMstEntity.ExpCaseWorker = string.Empty;// strCaseWorkerDefaultCode;
                                                CaseMstEntity.CaseManager = string.Empty; //strCaseWorkerDefaultCode;
                                            }

                                        }




                                        if (propAgencyControlDetails.State.ToUpper() == "CT" && BaseForm.BusinessModuleID.ToString() == "08")
                                        {
                                            if (((ListItem)cmbPMOPfHeat.SelectedItem).Value.ToString() == "1")
                                            {
                                                CaseMstEntity.LPM0001 = CaseMST.LPM0001;
                                                CaseMstEntity.LPM0002 = CaseMST.LPM0002;
                                                CaseMstEntity.LPM0003 = CaseMST.LPM0003;
                                                CaseMstEntity.LPM0004 = CaseMST.LPM0004;
                                                CaseMstEntity.LPM0005 = CaseMST.LPM0005;
                                                CaseMstEntity.LPM0006 = CaseMST.LPM0006;
                                                CaseMstEntity.LPM0007 = CaseMST.LPM0007;
                                                CaseMstEntity.LPM0008 = CaseMST.LPM0008;
                                                CaseMstEntity.LPM0009 = CaseMST.LPM0009;
                                                CaseMstEntity.LPM0010 = CaseMST.LPM0010;
                                                CaseMstEntity.LPM0011 = CaseMST.LPM0011;
                                                CaseMstEntity.LPM0012 = CaseMST.LPM0012;
                                                CaseMstEntity.LPM0013 = CaseMST.LPM0013;
                                                CaseMstEntity.LPM0014 = CaseMST.LPM0014;
                                                CaseMstEntity.LPM0015 = CaseMST.LPM0015;
                                                CaseMstEntity.LPM0016 = CaseMST.LPM0016;
                                                CaseMstEntity.LPM0017 = CaseMST.LPM0017;
                                            }
                                            else
                                            {
                                                CaseMstEntity.LPM0001 = string.Empty;
                                                CaseMstEntity.LPM0002 = string.Empty;
                                                CaseMstEntity.LPM0003 = string.Empty;
                                                CaseMstEntity.LPM0004 = string.Empty;
                                                CaseMstEntity.LPM0005 = string.Empty;
                                                CaseMstEntity.LPM0006 = string.Empty;
                                                CaseMstEntity.LPM0007 = string.Empty;
                                                CaseMstEntity.LPM0008 = string.Empty;
                                                CaseMstEntity.LPM0009 = string.Empty;
                                                CaseMstEntity.LPM0010 = string.Empty;
                                                CaseMstEntity.LPM0011 = string.Empty;
                                                CaseMstEntity.LPM0012 = string.Empty;
                                                CaseMstEntity.LPM0013 = string.Empty;
                                                CaseMstEntity.LPM0014 = string.Empty;
                                                CaseMstEntity.LPM0015 = string.Empty;
                                                CaseMstEntity.LPM0016 = string.Empty;
                                                CaseMstEntity.LPM0017 = string.Empty;
                                            }
                                        }
                                        else
                                        {
                                            CaseMstEntity.LPM0001 = CaseMST.LPM0001;
                                            CaseMstEntity.LPM0002 = CaseMST.LPM0002;
                                            CaseMstEntity.LPM0003 = CaseMST.LPM0003;
                                            CaseMstEntity.LPM0004 = CaseMST.LPM0004;
                                            CaseMstEntity.LPM0005 = CaseMST.LPM0005;
                                            CaseMstEntity.LPM0006 = CaseMST.LPM0006;
                                            CaseMstEntity.LPM0007 = CaseMST.LPM0007;
                                            CaseMstEntity.LPM0008 = CaseMST.LPM0008;
                                            CaseMstEntity.LPM0009 = CaseMST.LPM0009;
                                            CaseMstEntity.LPM0010 = CaseMST.LPM0010;
                                            CaseMstEntity.LPM0011 = CaseMST.LPM0011;
                                            CaseMstEntity.LPM0012 = CaseMST.LPM0012;
                                            CaseMstEntity.LPM0013 = CaseMST.LPM0013;
                                            CaseMstEntity.LPM0014 = CaseMST.LPM0014;
                                            CaseMstEntity.LPM0015 = CaseMST.LPM0015;
                                            CaseMstEntity.LPM0016 = CaseMST.LPM0016;
                                            CaseMstEntity.LPM0017 = CaseMST.LPM0017;
                                        }




                                        CaseMstEntity.RollOver = "";
                                        CaseMstEntity.RiskValue = "0";
                                        CaseMstEntity.VerFund = string.Empty;
                                        CaseMstEntity.OmbScreen = string.Empty;
                                        CaseMstEntity.CbCaseManager = "0";
                                        CaseMstEntity.VerifyOthCmb = "";
                                        CaseMstEntity.SimPrint = "";
                                        CaseMstEntity.SimPrintDate = "";
                                        CaseMstEntity.CbFraud = "0";
                                        CaseMstEntity.FraudDate = string.Empty;
                                        CaseMstEntity.AddOperator1 = BaseForm.UserID;
                                        CaseMstEntity.TimesUpdated1 = "0";
                                        CaseMstEntity.TimesUpdated2 = "0";
                                        CaseMstEntity.TimesUpdated3 = "0";
                                        CaseMstEntity.TimesUpdated4 = "0";
                                        CaseMstEntity.LstcOperator1 = BaseForm.UserID;
                                        CaseMstEntity.AddOperator2 = BaseForm.UserID;
                                        CaseMstEntity.LstcOperator2 = BaseForm.UserID;
                                        CaseMstEntity.AddOperator3 = BaseForm.UserID;
                                        CaseMstEntity.LstcOperator3 = BaseForm.UserID;
                                        CaseMstEntity.AddOperator4 = BaseForm.UserID;
                                        CaseMstEntity.LstcOperator4 = BaseForm.UserID;
                                        CaseMstEntity.Mode = Mode;
                                        CaseMstEntity.Type = SearchMstType;


                                        if (CaseMstEntity.AboutUs != string.Empty)
                                            CaseMstEntity.AboutUsDesc = ((ListItem)cmbAboutUs.SelectedItem).Text.ToString();

                                        if (CaseMstEntity.CaseType != string.Empty)
                                            CaseMstEntity.CaseTypeDesc = ((ListItem)cmbCaseType.SelectedItem).Text.ToString();

                                        if (CaseMstEntity.BestContact != string.Empty)
                                            CaseMstEntity.ContactusDesc = ((ListItem)cmbContact.SelectedItem).Text.ToString();

                                        if (!((ListItem)cmbCounty.SelectedItem).Value.ToString().Equals("0"))
                                            CaseMstEntity.County = ((ListItem)cmbCounty.SelectedItem).Value.ToString();

                                        if (CaseMstEntity.County != string.Empty)
                                            CaseMstEntity.CountyDesc = ((ListItem)cmbCounty.SelectedItem).Text.ToString();

                                        if (CaseMstEntity.FamilyType != string.Empty)
                                            CaseMstEntity.FamilyTypeDesc = ((ListItem)cmbFamilyType.SelectedItem).Text.ToString();

                                        if (CaseMstEntity.Housing != string.Empty)
                                            CaseMstEntity.HousingSituvationDesc = ((ListItem)cmbHousingSituation.SelectedItem).Text.ToString();

                                        if (CaseMstEntity.Language != string.Empty)
                                            CaseMstEntity.PrimaryLanDesc = ((ListItem)cmbPrimaryLang.SelectedItem).Text.ToString();

                                        if (CaseMstEntity.LanguageOt != string.Empty)
                                            CaseMstEntity.SecondaryLanDesc = ((ListItem)cmbSecondLang.SelectedItem).Text.ToString();

                                        if (CaseMstEntity.IntakeWorker != string.Empty)
                                            CaseMstEntity.AssignWorkerDesc = ((ListItem)cmbStaff.SelectedItem).Text.ToString();

                                        if (CaseMstEntity.TownShip != string.Empty)
                                            CaseMstEntity.TownShipDesc = ((ListItem)cmbTownship.SelectedItem).Text.ToString();

                                        if (CaseMstEntity.WaitList != string.Empty)
                                            CaseMstEntity.WaitingListDesc = ((ListItem)cmbWaitingList.SelectedItem).Text.ToString();

                                        if (CaseMstEntity.ExpCaseWorker != string.Empty)
                                        {
                                            if (CaseMstEntity.ExpCaseWorker.ToUpper() == strCaseWorkerDefaultCode.ToUpper())
                                            {
                                                CaseMstEntity.ExpCaseworkerDesc = strCaseworkerDesc;
                                            }
                                            else
                                                CaseMstEntity.ExpCaseworkerDesc = ((ListItem)cmbVerifiedStaff.SelectedItem).Text.ToString();
                                        }

                                        if (CaseMstEntity.SubStype != string.Empty)
                                            CaseMstEntity.SubsTypeDesc = ((ListItem)cmbSubsidized.SelectedItem).Text.ToString();

                                        if (CaseMstEntity.HeatIncRent != string.Empty)
                                            CaseMstEntity.HeatIncRentDesc = ((ListItem)cmbPMOPfHeat.SelectedItem).Text.ToString();

                                        if (CaseMstEntity.Source != string.Empty)
                                            CaseMstEntity.SourceDesc = ((ListItem)cmbPrimarySourceoHeat.SelectedItem).Text.ToString();

                                        if (CaseMstEntity.Dwelling != string.Empty)
                                            CaseMstEntity.DwellingDesc = ((ListItem)cmbDwelling.SelectedItem).Text.ToString();

                                        //CaseMstEntity.PressTotal = BaseForm.BaseCaseMstListEntity[0].PressTotal.ToString();
                                        //CaseMstEntity.PressCat = BaseForm.BaseCaseMstListEntity[0].PressCat.ToString();

                                        CaseMstHistEntity = CaseMstEntity;
                                        if (Mode.Equals(Consts.Common.Edit))
                                        {
                                            CaseMstHistEntity.PressTotal = BaseForm.BaseCaseMstListEntity[0].PressTotal.ToString();
                                            CaseMstHistEntity.PressCat = BaseForm.BaseCaseMstListEntity[0].PressCat.ToString();
                                        }
                                        CaseMstEntity.ModuleCode = BaseForm.BusinessModuleID;

                                        /// case snp details logic
                                        /// 


                                        CaseSnpEntity SnpEntity = new CaseSnpEntity();
                                        SnpEntity.Agency = CaseMST.ApplAgency;
                                        SnpEntity.Dept = CaseMST.ApplDept;
                                        SnpEntity.Program = CaseMST.ApplProgram;


                                        SnpEntity.Year = CaseMST.ApplYr;

                                        if (IsApplicant == true && Mode.Equals(Consts.Common.Add))
                                        {
                                            SnpEntity.Type = "MST";
                                            SnpEntity.Ssno = mskSSN.Text.ToUpper();
                                        }
                                        else
                                        {
                                            SnpEntity.Ssno = mskSSN.Text.ToUpper();
                                            SnpEntity.App = CaseMST.ApplNo;
                                            SnpEntity.Type = "MST";
                                        }


                                        SnpEntity.SsBic = txtBIC.Text.Trim();
                                        if (Mode.Equals(Consts.Common.Edit)) SnpEntity.FamilySeq = CaseSNP.FamilySeq;
                                        else if (Mode.Equals("Add") && IsApplicant) SnpEntity.FamilySeq = "1";
                                        if (!((ListItem)cmbRelation.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.MemberCode = ((ListItem)cmbRelation.SelectedItem).Value.ToString();
                                        }
                                        if (!((ListItem)cmbSSNReason.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.SsnReason = ((ListItem)cmbSSNReason.SelectedItem).Value.ToString();
                                        }

                                        SnpEntity.NameixLast = txtLastName.Text;
                                        SnpEntity.NameixFi = txtFirstName.Text;
                                        SnpEntity.NameixMi = txtMI.Text;
                                        SnpEntity.SnpSuffix = txtAppSuffix.Text;
                                        if (dtBirth.Checked == true)
                                            SnpEntity.AltBdate = dtBirth.Text.Trim();
                                        SnpEntity.Alias = txtAlias.Text;
                                        SnpEntity.Status = ckActive.Checked ? "A" : "I";

                                        if (!((ListItem)cmbGender.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Sex = ((ListItem)cmbGender.SelectedItem).Value.ToString();
                                        }

                                        if (!txtYear.Text.ToString().Equals(string.Empty) && !txtYear.Text.ToString().Equals("UNK")) SnpEntity.Age = txtYear.Text;

                                        if (!((ListItem)cmbEthnicity.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Ethnic = ((ListItem)cmbEthnicity.SelectedItem).Value.ToString();
                                        }


                                        if (!((ListItem)cmbEducation.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Education = ((ListItem)cmbEducation.SelectedItem).Value.ToString();
                                        }


                                        if (!((ListItem)cmbHealthInsurance.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.HealthIns = ((ListItem)cmbHealthInsurance.SelectedItem).Value.ToString();
                                        }

                                        if (!((ListItem)cmbVeteranCode.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Vet = ((ListItem)cmbVeteranCode.SelectedItem).Value.ToString();
                                        }


                                        if (!((ListItem)cmbDisabled.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Disable = ((ListItem)cmbDisabled.SelectedItem).Value.ToString();
                                        }

                                        if (!((ListItem)cmbFoodStamps.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.FootStamps = ((ListItem)cmbFoodStamps.SelectedItem).Value.ToString();
                                        }

                                        if (!((ListItem)cmbFarmer.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Farmer = ((ListItem)cmbFarmer.SelectedItem).Value.ToString();
                                        }

                                        if (!((ListItem)cmbWIC.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Wic = ((ListItem)cmbWIC.SelectedItem).Value.ToString();
                                        }

                                        if (!((ListItem)cmbResident.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Resident = ((ListItem)cmbResident.SelectedItem).Value.ToString();
                                        }

                                        if (!((ListItem)cmbAreyoupregnant.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Pregnant = ((ListItem)cmbAreyoupregnant.SelectedItem).Value.ToString();
                                        }


                                        if (!((ListItem)cmbMaritalStatus.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.MaritalStatus = ((ListItem)cmbMaritalStatus.SelectedItem).Value.ToString();
                                        }

                                        if (!((ListItem)cmbSchool.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.SchoolDistrict = ((ListItem)cmbSchool.SelectedItem).Value.ToString();
                                        }

                                        SnpEntity.AlienRegNo = txtAlienRegNo.Text;
                                        if (!((ListItem)cmbLegaltowork.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.LegalTowork = ((ListItem)cmbLegaltowork.SelectedItem).Value.ToString();
                                        }
                                        if (dtExpirationdate.Checked == true)
                                            SnpEntity.ExpireWorkDate = dtExpirationdate.Text.Trim();
                                        if (!((ListItem)cmbReliableTrans.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Relitran = ((ListItem)cmbReliableTrans.SelectedItem).Value.ToString();
                                        }

                                        if (!((ListItem)cmbDriving.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Drvlic = ((ListItem)cmbDriving.SelectedItem).Value.ToString();
                                        }

                                        if (!((ListItem)cmbRace.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Race = ((ListItem)cmbRace.SelectedItem).Value.ToString();
                                        }
                                        SnpEntity.DobNa = checkNA.Checked ? "1" : "0";
                                        SnpEntity.Snp_HH_ExcludeMem = "N";
                                        SnpEntity.Exclude = "N";
                                        if (ckActive.Checked == true)
                                        {
                                            if (!((ListItem)cmbRelation.SelectedItem).Value.ToString().Equals("0"))
                                            {
                                                if (((ListItem)cmbRelation.SelectedItem).ValueDisplayCode.ToString().Equals("N"))
                                                {
                                                    if (ckExcludeMember.Visible)
                                                        SnpEntity.Exclude = ckExcludeMember.Checked ? "Y" : "N";
                                                    if (chkExcludeHHMem.Visible)
                                                        SnpEntity.Snp_HH_ExcludeMem = chkExcludeHHMem.Checked ? "Y" : "N";
                                                    if (SearchSnpType == "SSNSEARCH")
                                                    {
                                                        SnpEntity.Exclude = "N";
                                                    }
                                                }
                                                else
                                                {
                                                    SnpEntity.Exclude = "Y";
                                                    if (chkExcludeHHMem.Visible)
                                                        SnpEntity.Snp_HH_ExcludeMem = chkExcludeHHMem.Checked ? "Y" : "N";
                                                }
                                            }
                                            else
                                            {
                                                if (ckExcludeMember.Visible)
                                                    SnpEntity.Exclude = ckExcludeMember.Checked ? "Y" : "N";
                                                if (chkExcludeHHMem.Visible)
                                                    SnpEntity.Snp_HH_ExcludeMem = chkExcludeHHMem.Checked ? "Y" : "N";
                                                if (SearchSnpType == "SSNSEARCH")
                                                {
                                                    SnpEntity.Exclude = "N";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            SnpEntity.Exclude = "N";
                                        }
                                        SnpEntity.AddOperator = BaseForm.UserID;
                                        SnpEntity.LstcOperator = BaseForm.UserID;

                                        // ********************************** Employee Details **********************************************//

                                        SnpEntity.Employed = ((ListItem)cmbEpresenteEmploy.SelectedItem).Value.ToString();
                                        if (dtElastDateWorked.Checked == true)
                                            SnpEntity.LastWorkDate = dtElastDateWorked.Value.ToString();
                                        SnpEntity.WorkLimit = ((ListItem)cmbEAnywork.SelectedItem).Value.ToString();
                                        SnpEntity.ExplainWorkLimit = txtEifyesexplain.Text;
                                        SnpEntity.NumberOfcjobs = txtEcurrentHave.Text;
                                        SnpEntity.NumberofLvjobs = txtElastvisit.Text;
                                        SnpEntity.FullTimeHours = txtEFullTime.Text;
                                        SnpEntity.PartTimeHours = txtEPartTime.Text;
                                        SnpEntity.SeasonalEmploy = ((ListItem)cmbEseasonalEmployee.SelectedItem).Value.ToString();
                                        if (chkE1st.Checked == true)
                                            SnpEntity.IstShift = "Y";
                                        else
                                            SnpEntity.IstShift = "N";
                                        if (chkE2nd.Checked == true)
                                            SnpEntity.IIndShift = "Y";
                                        else
                                            SnpEntity.IIndShift = "N";
                                        if (chkE3rd.Checked == true)
                                            SnpEntity.IIIrdShift = "Y";
                                        else
                                            SnpEntity.IIIrdShift = "N";
                                        if (chkErotaing.Checked == true)
                                            SnpEntity.RShift = "Y";
                                        else
                                            SnpEntity.RShift = "N";
                                        SnpEntity.EmployerName = txtEEmployer.Text;
                                        SnpEntity.EmployerStreet = txtEstreet.Text;
                                        SnpEntity.EmployerCity = txtEcityState.Text;
                                        if (!((ListItem)cmbEJobTitle.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.JobTitle = ((ListItem)cmbEJobTitle.SelectedItem).Value.ToString();
                                        }
                                        if (!((ListItem)cmbEJobCategory.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.JobCategory = ((ListItem)cmbEJobCategory.SelectedItem).Value.ToString();
                                        }
                                        SnpEntity.HourlyWage = txtEhourlywage.Text;
                                        SnpEntity.PayFrequency = ((ListItem)cmbEpayFrequency.SelectedItem).Value.ToString();
                                        if (dtEHireDate.Checked == true)
                                            SnpEntity.HireDate = dtEHireDate.Value.ToString();
                                        //SnpEntity.Transerv = caseSnpSearchType.Transerv;
                                        if (mskEPhone.Text.Length > 0)
                                        {
                                            mskEPhone.Text = mskEPhone.Text.Replace(' ', '0') + SsnZeros(mskEPhone.TextLength, string.Empty);
                                            SnpEntity.EmplPhone = mskEPhone.Text;
                                        }

                                        if (SnpEntity.Snp_HH_ExcludeMem == "Y")
                                        {
                                            SnpEntity.Exclude = "Y";
                                        }
                                        SnpEntity.EmplExt = txtEExt.Text;
                                        SnpEntity.Mode = Mode;
                                        if (Mode.Equals(Consts.Common.Add))
                                        {
                                            if (ProgramDefinition.DepIntakeProg.Equals("Y") && ProgramDefinition.DepSsnAutoAssign == "2")
                                            {
                                                if (CaseMstEntity.Ssn.Trim() == string.Empty)
                                                {
                                                    CaseMstEntity.Ssn = "NewSSNNum";
                                                }
                                            }
                                            if (ProgramDefinition.DepIntakeProg.Equals("Y") && ProgramDefinition.DepSsnAutoAssign == "1")
                                            {
                                                if (CaseMstEntity.Ssn.Trim() == string.Empty)
                                                {
                                                    CaseMstEntity.Ssn = "NewSSNNum";
                                                }
                                            }

                                        }
                                        boolSnpInsert = _model.CaseMstData.InsertUpdateCaseMst(CaseMstEntity, out strApplNo, out strClientIdOut, out strFamilyIdOut, out strSSNNOOut, out strErrorMsg, SnpEntity);
                                        if (IsApplicant == true && Mode.Equals(Consts.Common.Add))
                                        {
                                            CaseMST.ApplNo = strApplNo;
                                        }
                                        CaseMSTSER MSTSEREntity = new CaseMSTSER();
                                        MSTSEREntity.Agency = CaseMST.ApplAgency;
                                        MSTSEREntity.Dept = CaseMST.ApplDept;
                                        MSTSEREntity.Program = CaseMST.ApplProgram;
                                        if (CaseMST.ApplYr == string.Empty)
                                            MSTSEREntity.Year = "    ";
                                        else
                                            MSTSEREntity.Year = CaseMST.ApplYr;
                                        MSTSEREntity.AppNo = CaseMST.ApplNo;
                                        MSTSEREntity.Mode = "DELETE";
                                        _model.CaseMstData.InsertUpdateDelMSTSER(MSTSEREntity);

                                        foreach (DataGridViewRow drrows in gvwServices.Rows)
                                        {
                                            if (drrows.Cells["Servicechk"].Value.ToString().ToUpper() == "TRUE")
                                            {
                                                MSTSEREntity = new CaseMSTSER();
                                                MSTSEREntity.Agency = CaseMST.ApplAgency;
                                                MSTSEREntity.Dept = CaseMST.ApplDept;
                                                MSTSEREntity.Program = CaseMST.ApplProgram;
                                                if (CaseMST.ApplYr == string.Empty)
                                                    MSTSEREntity.Year = "    ";
                                                else
                                                    MSTSEREntity.Year = CaseMST.ApplYr;
                                                if (IsApplicant == true && Mode.Equals(Consts.Common.Add))
                                                {
                                                    MSTSEREntity.AppNo = strApplNo;
                                                }
                                                else
                                                    MSTSEREntity.AppNo = CaseMST.ApplNo;
                                                MSTSEREntity.Service = drrows.Cells["ServiceCode"].Value.ToString();
                                                MSTSEREntity.AddOperator = BaseForm.UserID;
                                                MSTSEREntity.LstcOperator = BaseForm.UserID;
                                                _model.CaseMstData.InsertUpdateDelMSTSER(MSTSEREntity);
                                            }
                                        }

                                        // Landlard information add data
                                        if (Mode.Equals("Add"))
                                        {

                                            if (propCaseDiffMailDetails != null)
                                            {
                                                if (CaseMST.ApplYr == string.Empty)
                                                    propCaseDiffMailDetails.Year = "    ";
                                                else
                                                    propCaseDiffMailDetails.Year = CaseMST.ApplYr;
                                                propCaseDiffMailDetails.AppNo = CaseMST.ApplNo;
                                                _model.CaseMstData.InsertUpdateDelCaseDiff(propCaseDiffMailDetails);
                                            }

                                            if (propCaseDiffLLDetails != null)
                                            {
                                                if (CaseMST.ApplYr == string.Empty)
                                                    propCaseDiffLLDetails.Year = "    ";
                                                else
                                                    propCaseDiffLLDetails.Year = CaseMST.ApplYr;
                                                propCaseDiffLLDetails.AppNo = CaseMST.ApplNo;
                                                _model.CaseMstData.InsertUpdateDelLandlord(propCaseDiffLLDetails);
                                            }

                                        }

                                    }
                                    if (boolSnpInsert)
                                    {
                                        CaseSnpEntity SnpEntity = new CaseSnpEntity();
                                        SnpEntity.Agency = CaseMST.ApplAgency;
                                        SnpEntity.Dept = CaseMST.ApplDept;
                                        SnpEntity.Program = CaseMST.ApplProgram;
                                        if (Mode.Equals(Consts.Common.Edit))
                                        {
                                            SnpEntity.NonCashBenefits = CaseSNP.NonCashBenefits;
                                            SnpEntity.MilitaryStatus = CaseSNP.MilitaryStatus;
                                            SnpEntity.Health_Codes = CaseSNP.Health_Codes;
                                            SnpEntity.WorkStatus = CaseSNP.WorkStatus;
                                            SnpEntity.Youth = CaseSNP.Youth;
                                        }
                                        if (CaseMST.ApplYr == string.Empty)
                                            SnpEntity.Year = "    ";
                                        else
                                            SnpEntity.Year = CaseMST.ApplYr;

                                        if (IsApplicant == true && Mode.Equals(Consts.Common.Add))
                                        {
                                            SnpEntity.App = strApplNo;
                                            SnpEntity.ClientId = strClientIdOut;
                                            SnpEntity.Type = "MST";
                                            SnpEntity.Ssno = strSSNNOOut;
                                        }
                                        else
                                        {
                                            SnpEntity.Ssno = mskSSN.Text.ToUpper();
                                            SnpEntity.App = CaseMST.ApplNo;
                                            SnpEntity.Type = "SNP";
                                            if (Mode.Equals(Consts.Common.Add))
                                            {
                                                if (ProgramDefinition.DepIntakeProg.Equals("Y") && ProgramDefinition.DepSsnAutoAssign == "2")
                                                {
                                                    if (SnpEntity.Ssno.Trim() == string.Empty)
                                                    {
                                                        SnpEntity.Ssno = "NewSSNNum";
                                                    }
                                                }
                                                if (ProgramDefinition.DepIntakeProg.Equals("Y") && ProgramDefinition.DepSsnAutoAssign == "1")
                                                {
                                                    if (SnpEntity.Ssno.Trim() == string.Empty)
                                                    {
                                                        SnpEntity.Ssno = "NewSSNNum";
                                                    }
                                                }
                                            }
                                        }


                                        SnpEntity.SsBic = txtBIC.Text.Trim();
                                        if (Mode.Equals(Consts.Common.Edit)) SnpEntity.FamilySeq = CaseSNP.FamilySeq;
                                        else if (Mode.Equals("Add") && IsApplicant) SnpEntity.FamilySeq = "1";
                                        if (!((ListItem)cmbRelation.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.MemberCode = ((ListItem)cmbRelation.SelectedItem).Value.ToString();
                                        }
                                        if (SnpEntity.MemberCode != string.Empty)
                                            SnpEntity.RelationDesc = ((ListItem)cmbRelation.SelectedItem).Text.ToString();
                                        if (!((ListItem)cmbSSNReason.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.SsnReason = ((ListItem)cmbSSNReason.SelectedItem).Value.ToString();
                                        }
                                        if (SnpEntity.SsnReason != string.Empty)
                                            SnpEntity.SsnReasonDesc = ((ListItem)cmbSSNReason.SelectedItem).Text.ToString();
                                        SnpEntity.NameixLast = txtLastName.Text;
                                        SnpEntity.NameixFi = txtFirstName.Text;
                                        SnpEntity.NameixMi = txtMI.Text;
                                        SnpEntity.SnpSuffix = txtAppSuffix.Text;
                                        if (dtBirth.Checked == true)
                                            SnpEntity.AltBdate = dtBirth.Text.Trim();
                                        SnpEntity.Alias = txtAlias.Text;
                                        SnpEntity.Status = ckActive.Checked ? "A" : "I";

                                        if (!((ListItem)cmbGender.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Sex = ((ListItem)cmbGender.SelectedItem).Value.ToString();
                                        }
                                        if (SnpEntity.Sex != string.Empty)
                                            SnpEntity.GenderDesc = ((ListItem)cmbGender.SelectedItem).Text.ToString();
                                        if (!txtYear.Text.ToString().Equals(string.Empty) && !txtYear.Text.ToString().Equals("UNK")) SnpEntity.Age = txtYear.Text;

                                        if (!((ListItem)cmbEthnicity.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Ethnic = ((ListItem)cmbEthnicity.SelectedItem).Value.ToString();
                                        }
                                        if (SnpEntity.Ethnic != string.Empty)
                                            SnpEntity.EthnicityDesc = ((ListItem)cmbEthnicity.SelectedItem).Text.ToString();

                                        if (!((ListItem)cmbEducation.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Education = ((ListItem)cmbEducation.SelectedItem).Value.ToString();
                                        }
                                        if (SnpEntity.Education != string.Empty)
                                            SnpEntity.EducationDesc = ((ListItem)cmbEducation.SelectedItem).Text.ToString();

                                        if (!((ListItem)cmbHealthInsurance.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.HealthIns = ((ListItem)cmbHealthInsurance.SelectedItem).Value.ToString();
                                        }
                                        if (SnpEntity.HealthIns != string.Empty)
                                            SnpEntity.HealthInsuranceDesc = ((ListItem)cmbHealthInsurance.SelectedItem).Text.ToString();
                                        if (!((ListItem)cmbVeteranCode.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Vet = ((ListItem)cmbVeteranCode.SelectedItem).Value.ToString();
                                        }
                                        if (SnpEntity.Vet != string.Empty)
                                            SnpEntity.VeterncodeDesc = ((ListItem)cmbVeteranCode.SelectedItem).Text.ToString();

                                        if (!((ListItem)cmbDisabled.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Disable = ((ListItem)cmbDisabled.SelectedItem).Value.ToString();
                                        }
                                        if (SnpEntity.Disable != string.Empty)
                                            SnpEntity.DisabledDesc = ((ListItem)cmbDisabled.SelectedItem).Text.ToString();
                                        if (!((ListItem)cmbFoodStamps.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.FootStamps = ((ListItem)cmbFoodStamps.SelectedItem).Value.ToString();
                                        }
                                        if (SnpEntity.FootStamps != string.Empty)
                                            SnpEntity.FoodStampsDesc = ((ListItem)cmbFoodStamps.SelectedItem).Text.ToString();
                                        if (!((ListItem)cmbFarmer.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Farmer = ((ListItem)cmbFarmer.SelectedItem).Value.ToString();
                                        }
                                        if (SnpEntity.Farmer != string.Empty)
                                            SnpEntity.farmerDesc = ((ListItem)cmbFarmer.SelectedItem).Text.ToString();
                                        if (!((ListItem)cmbWIC.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Wic = ((ListItem)cmbWIC.SelectedItem).Value.ToString();
                                        }
                                        if (SnpEntity.Wic != string.Empty)
                                            SnpEntity.WicDesc = ((ListItem)cmbWIC.SelectedItem).Text.ToString();
                                        if (!((ListItem)cmbResident.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Resident = ((ListItem)cmbResident.SelectedItem).Value.ToString();
                                        }
                                        if (SnpEntity.Resident != string.Empty)
                                            SnpEntity.ResidentDesc = ((ListItem)cmbResident.SelectedItem).Text.ToString();
                                        if (!((ListItem)cmbAreyoupregnant.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Pregnant = ((ListItem)cmbAreyoupregnant.SelectedItem).Value.ToString();
                                        }
                                        if (SnpEntity.Pregnant != string.Empty)
                                            SnpEntity.AreYouPregantDesc = ((ListItem)cmbAreyoupregnant.SelectedItem).Text.ToString();

                                        if (!((ListItem)cmbMaritalStatus.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.MaritalStatus = ((ListItem)cmbMaritalStatus.SelectedItem).Value.ToString();
                                        }
                                        if (SnpEntity.MaritalStatus != string.Empty)
                                            SnpEntity.MartialStatusDesc = ((ListItem)cmbMaritalStatus.SelectedItem).Text.ToString();
                                        if (!((ListItem)cmbSchool.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.SchoolDistrict = ((ListItem)cmbSchool.SelectedItem).Value.ToString();
                                        }
                                        if (SnpEntity.SchoolDistrict != string.Empty)
                                            SnpEntity.SchoolDesc = ((ListItem)cmbSchool.SelectedItem).Text.ToString();
                                        SnpEntity.AlienRegNo = txtAlienRegNo.Text;
                                        if (!((ListItem)cmbLegaltowork.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.LegalTowork = ((ListItem)cmbLegaltowork.SelectedItem).Value.ToString();
                                        }
                                        if (dtExpirationdate.Checked == true)
                                            SnpEntity.ExpireWorkDate = dtExpirationdate.Text.Trim();
                                        if (!((ListItem)cmbReliableTrans.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Relitran = ((ListItem)cmbReliableTrans.SelectedItem).Value.ToString();
                                        }
                                        if (SnpEntity.Relitran != string.Empty)
                                            SnpEntity.ReliableTransportDesc = ((ListItem)cmbReliableTrans.SelectedItem).Text.ToString();
                                        if (!((ListItem)cmbDriving.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Drvlic = ((ListItem)cmbDriving.SelectedItem).Value.ToString();
                                        }
                                        if (SnpEntity.Drvlic != string.Empty)
                                            SnpEntity.DriverLicenceDesc = ((ListItem)cmbDriving.SelectedItem).Text.ToString();
                                        if (!((ListItem)cmbRace.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.Race = ((ListItem)cmbRace.SelectedItem).Value.ToString();
                                        }
                                        if (SnpEntity.Race != string.Empty)
                                            SnpEntity.RaceDesc = ((ListItem)cmbRace.SelectedItem).Text.ToString();
                                        SnpEntity.DobNa = checkNA.Checked ? "1" : "0";
                                        SnpEntity.Snp_HH_ExcludeMem = "N";
                                        SnpEntity.Exclude = "N";
                                        if (ckActive.Checked == true)
                                        {
                                            if (!((ListItem)cmbRelation.SelectedItem).Value.ToString().Equals("0"))
                                            {
                                                if (((ListItem)cmbRelation.SelectedItem).ValueDisplayCode.ToString().Equals("N"))
                                                {
                                                    if (ckExcludeMember.Visible)
                                                        SnpEntity.Exclude = ckExcludeMember.Checked ? "Y" : "N";
                                                    if (chkExcludeHHMem.Visible)
                                                        SnpEntity.Snp_HH_ExcludeMem = chkExcludeHHMem.Checked ? "Y" : "N";
                                                    if (SearchSnpType == "SSNSEARCH")
                                                    {
                                                        SnpEntity.Exclude = "N";
                                                    }
                                                }
                                                else
                                                {
                                                    SnpEntity.Exclude = "Y";
                                                    if (chkExcludeHHMem.Visible)
                                                        SnpEntity.Snp_HH_ExcludeMem = chkExcludeHHMem.Checked ? "Y" : "N";
                                                }
                                            }
                                            else
                                            {
                                                if (ckExcludeMember.Visible)
                                                    SnpEntity.Exclude = ckExcludeMember.Checked ? "Y" : "N";
                                                if (chkExcludeHHMem.Visible)
                                                    SnpEntity.Snp_HH_ExcludeMem = chkExcludeHHMem.Checked ? "Y" : "N";
                                                if (SearchSnpType == "SSNSEARCH")
                                                {
                                                    SnpEntity.Exclude = "N";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            SnpEntity.Exclude = "N";

                                        }
                                        SnpEntity.AddOperator = BaseForm.UserID;
                                        SnpEntity.LstcOperator = BaseForm.UserID;

                                        // ********************************** Employee Details **********************************************//

                                        SnpEntity.Employed = ((ListItem)cmbEpresenteEmploy.SelectedItem).Value.ToString();
                                        if (dtElastDateWorked.Checked == true)
                                            SnpEntity.LastWorkDate = dtElastDateWorked.Value.ToString();
                                        SnpEntity.WorkLimit = ((ListItem)cmbEAnywork.SelectedItem).Value.ToString();
                                        SnpEntity.ExplainWorkLimit = txtEifyesexplain.Text;
                                        SnpEntity.NumberOfcjobs = txtEcurrentHave.Text;
                                        SnpEntity.NumberofLvjobs = txtElastvisit.Text;
                                        SnpEntity.FullTimeHours = txtEFullTime.Text;
                                        SnpEntity.PartTimeHours = txtEPartTime.Text;
                                        SnpEntity.SeasonalEmploy = ((ListItem)cmbEseasonalEmployee.SelectedItem).Value.ToString();
                                        if (chkE1st.Checked == true)
                                            SnpEntity.IstShift = "Y";
                                        else
                                            SnpEntity.IstShift = "N";
                                        if (chkE2nd.Checked == true)
                                            SnpEntity.IIndShift = "Y";
                                        else
                                            SnpEntity.IIndShift = "N";
                                        if (chkE3rd.Checked == true)
                                            SnpEntity.IIIrdShift = "Y";
                                        else
                                            SnpEntity.IIIrdShift = "N";
                                        if (chkErotaing.Checked == true)
                                            SnpEntity.RShift = "Y";
                                        else
                                            SnpEntity.RShift = "N";
                                        SnpEntity.EmployerName = txtEEmployer.Text;
                                        SnpEntity.EmployerStreet = txtEstreet.Text;
                                        SnpEntity.EmployerCity = txtEcityState.Text;
                                        if (!((ListItem)cmbEJobTitle.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.JobTitle = ((ListItem)cmbEJobTitle.SelectedItem).Value.ToString();
                                        }
                                        if (!((ListItem)cmbEJobCategory.SelectedItem).Value.ToString().Equals("0"))
                                        {
                                            SnpEntity.JobCategory = ((ListItem)cmbEJobCategory.SelectedItem).Value.ToString();
                                        }
                                        SnpEntity.HourlyWage = txtEhourlywage.Text;
                                        SnpEntity.PayFrequency = ((ListItem)cmbEpayFrequency.SelectedItem).Value.ToString();
                                        if (dtEHireDate.Checked == true)
                                            SnpEntity.HireDate = dtEHireDate.Value.ToString();
                                        //SnpEntity.Transerv = caseSnpSearchType.Transerv;
                                        if (mskEPhone.Text.Length > 0)
                                        {
                                            mskEPhone.Text = mskEPhone.Text.Replace(' ', '0') + SsnZeros(mskEPhone.TextLength, string.Empty);
                                            SnpEntity.EmplPhone = mskEPhone.Text;
                                        }

                                        //SnpEntity.EmplExch = caseSnpSearchType.EmplExch;
                                        //SnpEntity.EmplTel = caseSnpSearchType.EmplTel;
                                        SnpEntity.EmplExt = txtEExt.Text;

                                        if (SnpEntity.LegalTowork != string.Empty)
                                            SnpEntity.LegaltoworkDesc = ((ListItem)cmbLegaltowork.SelectedItem).Text.ToString();
                                        if (SnpEntity.Employed != string.Empty)
                                            SnpEntity.PresentEmployDesc = ((ListItem)cmbEpresenteEmploy.SelectedItem).Text.ToString();
                                        if (SnpEntity.WorkLimit != string.Empty)
                                            SnpEntity.AnyworklimitationDesc = ((ListItem)cmbEAnywork.SelectedItem).Text.ToString();
                                        if (SnpEntity.SeasonalEmploy != string.Empty)
                                            SnpEntity.SeasonEmployedDesc = ((ListItem)cmbEseasonalEmployee.SelectedItem).Text.ToString();
                                        if (SnpEntity.JobTitle != string.Empty)
                                            SnpEntity.JobTitleDesc = ((ListItem)cmbEJobTitle.SelectedItem).Text.ToString();
                                        if (SnpEntity.JobCategory != string.Empty)
                                            SnpEntity.JobCategoryDesc = ((ListItem)cmbEJobCategory.SelectedItem).Text.ToString();
                                        if (SnpEntity.PayFrequency != string.Empty)
                                            SnpEntity.PayFrequencyDesc = ((ListItem)cmbEpayFrequency.SelectedItem).Text.ToString();




                                        if (SearchSnpType != string.Empty)
                                        {
                                            CaseSnpEntity caseSnpSearchType = CaseSNP;
                                            if (caseSnpSearchType != null)
                                            {
                                                SnpEntity.ClientId = caseSnpSearchType.ClientId;
                                                SnpEntity.SsBic = caseSnpSearchType.SsBic;
                                                SnpEntity.AltLName = caseSnpSearchType.AltLName;
                                                SnpEntity.AltFi = caseSnpSearchType.AltFi;
                                                SnpEntity.IncomeBasis = caseSnpSearchType.IncomeBasis;
                                                SnpEntity.ApplDate = caseSnpSearchType.ApplDate;
                                                SnpEntity.ApplTime = caseSnpSearchType.ApplTime;
                                                SnpEntity.Ampm = caseSnpSearchType.Ampm;
                                                SnpEntity.InitialDate = caseSnpSearchType.InitialDate;
                                                SnpEntity.Site = caseSnpSearchType.Site;
                                                SnpEntity.TotIncome = caseSnpSearchType.TotIncome;
                                                SnpEntity.ProgIncome = caseSnpSearchType.ProgIncome;
                                                SnpEntity.ClaimSsno = caseSnpSearchType.ClaimSsno;
                                                SnpEntity.ClaimSsbic = caseSnpSearchType.ClaimSsbic;
                                                SnpEntity.Wagem = caseSnpSearchType.Wagem;
                                                SnpEntity.Student = caseSnpSearchType.Student;
                                                //SnpEntity.Employed = caseSnpSearchType.Employed;
                                                //SnpEntity.LastWorkDate = caseSnpSearchType.LastWorkDate;
                                                //SnpEntity.WorkLimit = caseSnpSearchType.WorkLimit;
                                                //SnpEntity.ExplainWorkLimit = caseSnpSearchType.ExplainWorkLimit;
                                                //SnpEntity.NumberOfcjobs = caseSnpSearchType.NumberOfcjobs;
                                                //SnpEntity.NumberofLvjobs = caseSnpSearchType.NumberofLvjobs;
                                                //SnpEntity.FullTimeHours = caseSnpSearchType.FullTimeHours;
                                                //SnpEntity.PartTimeHours = caseSnpSearchType.PartTimeHours;
                                                //SnpEntity.SeasonalEmploy = caseSnpSearchType.SeasonalEmploy;
                                                //SnpEntity.IstShift = caseSnpSearchType.IstShift;
                                                //SnpEntity.IIndShift = caseSnpSearchType.IIndShift;
                                                //SnpEntity.IIIrdShift = caseSnpSearchType.IIIrdShift;
                                                //SnpEntity.RShift = caseSnpSearchType.RShift;
                                                //SnpEntity.EmployerName = caseSnpSearchType.EmployerName;
                                                //SnpEntity.EmployerStreet = caseSnpSearchType.EmployerStreet;
                                                //SnpEntity.EmployerCity = caseSnpSearchType.EmployerCity;
                                                //SnpEntity.JobTitle = caseSnpSearchType.JobTitle;
                                                //SnpEntity.JobCategory = caseSnpSearchType.JobCategory;
                                                //SnpEntity.HourlyWage = caseSnpSearchType.HourlyWage;
                                                //SnpEntity.PayFrequency = caseSnpSearchType.PayFrequency;
                                                //SnpEntity.HireDate = caseSnpSearchType.HireDate;
                                                //SnpEntity.Transerv = caseSnpSearchType.Transerv;
                                                //SnpEntity.EmplArea = caseSnpSearchType.EmplArea;
                                                //SnpEntity.EmplExch = caseSnpSearchType.EmplExch;
                                                //SnpEntity.EmplTel = caseSnpSearchType.EmplTel;
                                                //SnpEntity.EmplExt = caseSnpSearchType.EmplExt;
                                                //SnpEntity.SsnReason = caseSnpSearchType.SsnReason;
                                                SnpEntity.Blind = caseSnpSearchType.Blind;
                                                SnpEntity.AbleTowork = caseSnpSearchType.AbleTowork;
                                                SnpEntity.RecMedicare = caseSnpSearchType.RecMedicare;
                                                SnpEntity.PurchaseFood = caseSnpSearchType.PurchaseFood;
                                                SnpEntity.VechicleValue = caseSnpSearchType.VechicleValue;
                                                SnpEntity.OtherVehicleValue = caseSnpSearchType.OtherVehicleValue;
                                                SnpEntity.OtherAssetValue = caseSnpSearchType.OtherAssetValue;
                                                SnpEntity.SsnSearchType = SearchSnpType;
                                                SnpEntity.AltAgency = AltAgency;
                                                SnpEntity.AltDept = AltDept;
                                                SnpEntity.AltProgram = AltProgram;
                                                SnpEntity.AltYear = AltYear;
                                                SnpEntity.AltApp = AltApp;
                                                SnpEntity.AltFamilySeq = AltFamilySeq;
                                            }
                                        }
                                        if (SnpEntity.Snp_HH_ExcludeMem == "Y")
                                        {
                                            SnpEntity.Exclude = "Y";
                                        }

                                        string strOutFamilyseq = SnpEntity.FamilySeq;
                                        if (_model.CaseMstData.InsertSNPDETAILS(SnpEntity, out strOutFamilyseq))
                                        {
                                            string strPreassOldTotal = CaseMST.PressTotal;
                                            if (Mode.Equals("Add") && (!IsApplicant))
                                            {
                                                SnpEntity.FamilySeq = strOutFamilyseq;
                                            }
                                            saveCustQuestions(SnpEntity);
                                            if (IsApplicant)
                                            {
                                                saveCustIntakeQuestions(SnpEntity);
                                                if (preassesCntlEntity.Count > 0)
                                                {
                                                    if (preassesCntlEntity.Exists(u => u.Enab.Equals("Y")))
                                                    {
                                                        savePresrespQuestions(SnpEntity);
                                                    }
                                                }
                                            }
                                            Case3001Control case3001Control = BaseForm.GetBaseUserControl() as Case3001Control;
                                            if (case3001Control != null)
                                            {
                                                if (IsApplicant)
                                                {
                                                    if (Mode.Equals("Add"))
                                                    {
                                                        ShowHierachyandApplNo(strApplNo);
                                                        // Lihe Apv table record insert added by murali 10/14/2015 need to discuss 
                                                        if (propAgencyControlDetails.State.ToUpper() == "CT" && BaseForm.BusinessModuleID.ToString() == "08")
                                                        {
                                                            LiheApvEntity liheapv = new LiheApvEntity();
                                                            liheapv.LPV_AGENCY = BaseForm.BaseAgency;
                                                            liheapv.LPV_DEPT = BaseForm.BaseDept;
                                                            liheapv.LPV_PROGRAM = BaseForm.BaseProg;
                                                            liheapv.LPV_YEAR = BaseForm.BaseYear;
                                                            liheapv.LPV_APP_NO = strApplNo;
                                                            liheapv.LPV_BILL_FNAME = txtFirstName.Text;
                                                            liheapv.LPV_BILL_LNAME = txtLastName.Text;
                                                            liheapv.LPV_PAYMENT_FOR = "09";// Description  Other
                                                            liheapv.LPV_LSTC_OPERATOR = "CASE3001";
                                                            liheapv.LPV_ADD_OPERATOR = "CASE3001";
                                                            liheapv.Mode = "CASE2001";
                                                            liheapv.LPV_REVERIFY = "N";
                                                            if (_model.LiheAllData.InsertUpdateDelLiheApv(liheapv))
                                                            { }

                                                        }
                                                    }
                                                    else
                                                    {
                                                        ShowHierachyandApplNo(CaseMST.ApplNo);
                                                    }


                                                }
                                                else
                                                {
                                                    ShowHierachyandApplNo(CaseMST.ApplNo);
                                                }

                                                if (Mode.Equals(Consts.Common.Edit))
                                                {
                                                    if (IsApplicant)
                                                    {
                                                        CaseMstHistEntity.Juvenile = BaseForm.BaseCaseMstListEntity[0].Juvenile;
                                                        CaseMstHistEntity.Senior = BaseForm.BaseCaseMstListEntity[0].Senior;
                                                        CaseMstHistEntity.FamIncome = BaseForm.BaseCaseMstListEntity[0].FamIncome;
                                                        CaseMstHistEntity.ProgIncome = BaseForm.BaseCaseMstListEntity[0].ProgIncome;
                                                        CaseMstHistEntity.NoInhh = BaseForm.BaseCaseMstListEntity[0].NoInhh;
                                                        CaseMstHistEntity.NoInProg = BaseForm.BaseCaseMstListEntity[0].NoInProg;
                                                        // CaseMstHistEntity.PressTotal = BaseForm.BaseCaseMstListEntity[0].PressTotal;
                                                        //CaseMstHistEntity.PressCat = BaseForm.BaseCaseMstListEntity[0].PressCat;
                                                        if (LookupDataAccess.Getdate(CaseMST.IntakeDate) != LookupDataAccess.Getdate(BaseForm.BaseCaseMstListEntity[0].IntakeDate))
                                                        {
                                                            CaseSnpEntity casesnpHi = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(CaseSNP.FamilySeq));
                                                            CaseSNP.Age = casesnpHi.Age;

                                                            if (dtBirth.Checked == true)
                                                            {
                                                                string strIntakeDate = BaseForm.BaseCaseMstListEntity[0].IntakeDate;
                                                                if (strIntakeDate == string.Empty || strIntakeDate == null)
                                                                    strIntakeDate = DateTime.Now.Date.ToShortDateString();
                                                                SnpEntity.Age = CommonFunctions.CalculationYear(Convert.ToDateTime(dtBirth.Text.Trim()), Convert.ToDateTime(strIntakeDate));
                                                            }

                                                        }
                                                        // Landlord record delete
                                                        if (cmbHousingSituation.Items.Count > 0)
                                                        {
                                                            string strV = ((ListItem)cmbHousingSituation.SelectedItem).Value.ToString().Trim();
                                                            if (strV != "B")
                                                            {
                                                                CaseDiffEntity caseDiffEntity = new CaseDiffEntity();
                                                                caseDiffEntity.Agency = BaseForm.BaseAgency;
                                                                caseDiffEntity.Dept = BaseForm.BaseDept;
                                                                caseDiffEntity.Program = BaseForm.BaseProg;
                                                                caseDiffEntity.Year = BaseForm.BaseYear;
                                                                caseDiffEntity.AppNo = BaseForm.BaseApplicationNo;
                                                                caseDiffEntity.Mode = "Delete";
                                                                _model.CaseMstData.InsertUpdateDelLandlord(caseDiffEntity);
                                                            }
                                                        }

                                                        if (BaseForm.BaseCaseMstListEntity[0].PressTotal != string.Empty)
                                                        {
                                                            if (Convert.ToDecimal(strPreassOldTotal == string.Empty ? "0" : strPreassOldTotal) != Convert.ToDecimal(BaseForm.BaseCaseMstListEntity[0].PressTotal))
                                                            {
                                                                CaseMST.Mode = "PressTotal";
                                                                CaseMST.LstcOperator3 = BaseForm.UserID;
                                                                _model.CaseMstData.InsertUpdateCaseMst(CaseMST, out strApplNo, out strClientIdOut, out strFamilyIdOut, out strSSNNOOut, out strErrorMsg);
                                                            }
                                                        }

                                                    }
                                                    CheckHistoryTableData(CaseMstHistEntity, SnpEntity);
                                                }


                                                case3001Control.Refresh();
                                            }
                                            else
                                            {
                                                if (IsApplicant && Mode.Equals("Add"))
                                                {
                                                    ShowHierachyandApplNo(strApplNo);
                                                    // Lihe Apv table record insert added by murali 10/14/2015 need to discuss 
                                                    if (propAgencyControlDetails.State.ToUpper() == "CT" && BaseForm.BusinessModuleID.ToString() == "08")
                                                    {
                                                        LiheApvEntity liheapv = new LiheApvEntity();
                                                        liheapv.LPV_AGENCY = BaseForm.BaseAgency;
                                                        liheapv.LPV_DEPT = BaseForm.BaseDept;
                                                        liheapv.LPV_PROGRAM = BaseForm.BaseProg;
                                                        liheapv.LPV_YEAR = BaseForm.BaseYear;
                                                        liheapv.LPV_APP_NO = strApplNo;
                                                        liheapv.LPV_BILL_FNAME = txtFirstName.Text;
                                                        liheapv.LPV_BILL_LNAME = txtLastName.Text;
                                                        liheapv.LPV_PAYMENT_FOR = "09";// Description  Other
                                                        liheapv.LPV_LSTC_OPERATOR = "CASE3001";
                                                        liheapv.LPV_ADD_OPERATOR = "CASE3001";
                                                        liheapv.Mode = "CASE2001";
                                                        if (_model.LiheAllData.InsertUpdateDelLiheApv(liheapv))
                                                        { }

                                                    }
                                                    BaseForm.AddTabClientIntake("CASE2001");
                                                }
                                            }
                                            if (Privileges.Program == "Main Menu") // Added By Yeswanth On 01212013
                                                this.DialogResult = DialogResult.OK;
                                            Refresh_Control = "Close";
                                            this.Close();
                                        }
                                        else
                                        {
                                            if (IsApplicant == true && Mode.Equals(Consts.Common.Add))
                                            {
                                                btnSave.Enabled = true;
                                                CaseMstHistEntity.Mode = "DELETE";
                                                CaseMstHistEntity.ApplNo = strApplNo;
                                                CaseMstHistEntity.ClientId = strClientIdOut;
                                                CaseMstHistEntity.Ssn = strSSNNOOut;
                                                boolSnpInsert = _model.CaseMstData.InsertUpdateCaseMst(CaseMstHistEntity, out strApplNo, out strClientIdOut, out strFamilyIdOut, out strSSNNOOut, out strErrorMsg);
                                                CaseMst.InsertErrorLog("Case2001MST", _model.CaseMstData.ErrorLogMst(CaseMstHistEntity, SnpEntity.ClientId, CaseMstHistEntity.FamilyId, SnpEntity.Ssno), "Delete Mst. Snp Record Not exists Error", BaseForm.UserID);
                                                CaseMstHistEntity.Mode = "Add";
                                                CommonFunctions.MessageBoxDisplay("SNP Record Not Properly Inserted, try again ");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        btnSave.Enabled = true;
                                        CommonFunctions.MessageBoxDisplay(strErrorMsg);

                                    }
                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                tabControl1.SelectedIndex = 0;
                                if (Relationshipdefaultcode != string.Empty)
                                    CommonFunctions.MessageBoxDisplay(((ListItem)cmbRelation.SelectedItem).Text.ToString() + "\n Can only be selected by primary Applicant");
                                //else
                                //    CommonFunctions.MessageBoxDisplay("No Default Relation Set in Agy table");


                                return;
                            }
                        }
                        else
                        {
                            tabControl1.SelectedIndex = 1;
                            CommonFunctions.MessageBoxDisplay("Zip code and city/county/Township are not matching");

                        }
                    }
                    else
                    {
                        tabControl1.SelectedIndex = 1;
                    }
                }
                else
                {
                    tabControl1.SelectedIndex = 1;
                }
            }
            else
            {
                tabControl1.SelectedIndex = 0;
            }
        }

        private bool CheckSSNDetails()
        {
            bool boolssndata = true;
            string strssn = mskSSN.Text.Replace(" ", string.Empty);

            if (strssn.Length <= 9 && strssn.Length > 0)
            {
                _errorProvider.SetError(mskSSN, null);
                //mskSSN.Text = mskSSN.Text.Replace(' ', '0') + SsnZeros(mskSSN.TextLength, "SSN");
                if (mskSSN.Text == "000000000" || mskSSN.Text.ToUpper() == "NEWSSNNUM")
                {
                    if (ProgramDefinition.SSNReasonFlag == "Y")
                    {
                        lblSSNReasonReq.Visible = true;
                        lblSSNReason.Visible = true;
                        cmbSSNReason.Visible = true;
                        lblSSNReason.Enabled = true;
                        cmbSSNReason.Enabled = true;
                    }
                    else
                    {
                        lblSSNReasonReq.Visible = false;
                        lblSSNReason.Visible = false;
                        cmbSSNReason.Visible = false;
                    }
                }
                else
                {
                    lblSSNReasonReq.Visible = false;
                    lblSSNReason.Visible = false;
                    cmbSSNReason.Visible = false;
                    cmbSSNReason.SelectedIndex = 0;
                    if (Mode.Equals("Add"))
                    {
                        SearchSnpType = string.Empty;
                        if (mskSSN.TextLength == 9)
                        {
                            if (mskSSN.Text != "000000000" && mskSSN.Text.ToUpper() != "NEWSSNNUM")
                            {
                                List<CaseSnpEntity> CaseSnpApplicantList = _model.CaseMstData.GetCaseSnpSSno(mskSSN.Text,string.Empty, string.Empty, string.Empty);
                                if (IsApplicant)
                                {
                                    if (ProgramDefinition.PRODUPSSN.Equals("Y"))
                                    {
                                        CaseMstAllList = _model.CaseMstData.GetCaseMstSSno(mskSSN.Text,string.Empty, string.Empty, string.Empty);
                                        CaseMstEntity caseMstAlllistEntity = CaseMstAllList.Find(u => u.ApplAgency.Equals(CaseMST.ApplAgency) && u.ApplDept.Equals(CaseMST.ApplDept) && u.ApplProgram.Equals(CaseMST.ApplProgram) && u.ApplYr.Trim().Equals(CaseMST.ApplYr.Trim()) && u.Ssn.Equals(mskSSN.Text));
                                        if (caseMstAlllistEntity != null)
                                        {
                                            _errorProvider.SetError(mskSSN, "Applicant already exists with this SSN in Hierarchy: " + caseMstAlllistEntity.ApplAgency + caseMstAlllistEntity.ApplDept + caseMstAlllistEntity.ApplProgram + caseMstAlllistEntity.ApplYr + " with App No: " + caseMstAlllistEntity.ApplNo + "\n You can add only new Applicant here");
                                            mskSSN.Text = string.Empty;
                                            mskSSN.Focus();
                                            boolssndata = false;
                                        }
                                    }

                                }

                                if (ProgramDefinition.ProDupMEM.Equals("Y"))
                                {

                                    CaseSnpEntity casesnpssnoEntity = CaseSnpApplicantList.Find(u => u.Agency.Equals(CaseMST.ApplAgency) && u.Dept.Equals(CaseMST.ApplDept) && u.Program.Equals(CaseMST.ApplProgram) && u.Year.Trim().Equals(CaseMST.ApplYr.Trim()) && u.App.Equals(CaseMST.ApplNo) && u.Ssno.Equals(mskSSN.Text));
                                    if (casesnpssnoEntity != null)
                                    {
                                        _errorProvider.SetError(mskSSN, "SSN Number already exist in this family");
                                        mskSSN.Text = string.Empty;
                                        mskSSN.Focus();
                                        boolssndata = false;
                                    }

                                    else
                                    {
                                        CaseSnpEntity casesnpAlllistEntity = CaseSnpApplicantList.Find(u => u.Agency.Equals(CaseMST.ApplAgency) && u.Dept.Equals(CaseMST.ApplDept) && u.Program.Equals(CaseMST.ApplProgram) && u.Year.Trim().Equals(CaseMST.ApplYr.Trim()) && u.Ssno.Equals(mskSSN.Text));
                                        if (casesnpAlllistEntity != null)
                                        {
                                            _errorProvider.SetError(mskSSN, "Member already exists in App#: " + casesnpAlllistEntity.App);
                                            mskSSN.Text = string.Empty;
                                            mskSSN.Focus();
                                            boolssndata = false;
                                        }
                                        else
                                        {
                                            List<CaseSnpEntity> casesnpssnosearchEntity = CaseSnpApplicantList.FindAll(u => u.Ssno.Equals(mskSSN.Text));
                                            if (casesnpssnosearchEntity != null)
                                            {
                                                if (casesnpssnosearchEntity.Count != 0)
                                                {
                                                    if (!IsApplicant)
                                                    {
                                                        if (casesnpssnosearchEntity.Count == 1)
                                                        {
                                                            // mskSSN.Text = selectedSsn.Ssno;
                                                            AltAgency = casesnpssnosearchEntity[0].Agency;
                                                            AltDept = casesnpssnosearchEntity[0].Dept;
                                                            AltProgram = casesnpssnosearchEntity[0].Program;
                                                            AltYear = casesnpssnosearchEntity[0].Year;
                                                            AltApp = casesnpssnosearchEntity[0].App.Substring(0, 8);
                                                            AltFamilySeq = casesnpssnosearchEntity[0].FamilySeq;
                                                            SearchSnpType = "SSNSEARCH";
                                                            if (SearchSnpType1 == string.Empty)
                                                                fillCaseSnpDetails(AltAgency.ToString(), AltDept.ToString(), AltProgram.ToString(), AltYear.ToString(), AltApp.ToString(), AltFamilySeq.ToString());
                                                            //if (IsApplicant)
                                                            //{
                                                            //    fillCaseMstDetails(AltAgency.ToString(), AltDept.ToString(), AltProgram.ToString(), AltYear.ToString(), AltApp.ToString());
                                                            //}

                                                        }
                                                        else
                                                        {
                                                            //if (SearchSnpType1 == "SSNSEARCH")
                                                            SearchSnpType = "SSNSEARCH";
                                                            //SsnScanForm SsnScanForm = new SsnScanForm(BaseForm, mskSSN.Text);
                                                            //SsnScanForm.FormClosed += new FormClosedEventHandler(OnSsnScanSearchFormClosed);
                                                            //SsnScanForm.ShowDialog();
                                                            // boolssndata = false;
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }

                                }
                                else
                                {
                                    CaseSnpEntity casesnpssnoEntity = CaseSnpApplicantList.Find(u => u.Agency.Equals(CaseMST.ApplAgency) && u.Dept.Equals(CaseMST.ApplDept) && u.Program.Equals(CaseMST.ApplProgram) && u.Year.Trim().Equals(CaseMST.ApplYr.Trim()) && u.App.Equals(CaseMST.ApplNo) && u.Ssno.Equals(mskSSN.Text));
                                    if (casesnpssnoEntity != null)
                                    {
                                        _errorProvider.SetError(mskSSN, "SSN Number already exist in this family");
                                        mskSSN.Text = string.Empty;
                                        mskSSN.Focus();
                                        boolssndata = false;
                                    }
                                    else
                                    {
                                        List<CaseSnpEntity> casesnpssnosearchEntity = CaseSnpApplicantList.FindAll(u => u.Ssno.Equals(mskSSN.Text)); // _model.CaseMstData.GetCaseSnpSSno(mskSSN.Text);// CaseSnpAllList.FindAll(u => u.Ssno.Equals(mskSSN.Text));
                                        if (casesnpssnosearchEntity != null)
                                        {
                                            if (casesnpssnosearchEntity.Count != 0)
                                            {
                                                if (!IsApplicant)
                                                {
                                                    if (casesnpssnosearchEntity.Count == 1)
                                                    {
                                                        // mskSSN.Text = selectedSsn.Ssno;
                                                        AltAgency = casesnpssnosearchEntity[0].Agency;
                                                        AltDept = casesnpssnosearchEntity[0].Dept;
                                                        AltProgram = casesnpssnosearchEntity[0].Program;
                                                        AltYear = casesnpssnosearchEntity[0].Year;
                                                        AltApp = casesnpssnosearchEntity[0].App.Substring(0, 8);
                                                        AltFamilySeq = casesnpssnosearchEntity[0].FamilySeq;
                                                        SearchSnpType = "SSNSEARCH";
                                                        if (SearchSnpType1 == string.Empty)
                                                            fillCaseSnpDetails(AltAgency.ToString(), AltDept.ToString(), AltProgram.ToString(), AltYear.ToString(), AltApp.ToString(), AltFamilySeq.ToString());
                                                        //if (IsApplicant)
                                                        //{
                                                        //    fillCaseMstDetails(AltAgency.ToString(), AltDept.ToString(), AltProgram.ToString(), AltYear.ToString(), AltApp.ToString());
                                                        //}
                                                    }
                                                    else
                                                    {
                                                        // if (SearchSnpType1 == "SSNSEARCH")
                                                        SearchSnpType = "SSNSEARCH";
                                                        //SsnScanForm SsnScanForm = new SsnScanForm(BaseForm, mskSSN.Text);
                                                        //SsnScanForm.FormClosed += new FormClosedEventHandler(OnSsnScanSearchFormClosed);
                                                        //SsnScanForm.ShowDialog();
                                                        //boolssndata = false;
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                    else if (Mode.Equals(Consts.Common.Edit))
                    {
                        if (mskSSN.Text != strSsnNumber)
                        {
                            if (mskSSN.TextLength == 9)
                            {
                                if (mskSSN.Text != "000000000")
                                {
                                    List<CaseSnpEntity> CaseSnpApplicantList = _model.CaseMstData.GetCaseSnpSSno(mskSSN.Text, string.Empty, string.Empty, string.Empty);
                                    if (IsApplicant)
                                    {
                                        if (ProgramDefinition.PRODUPSSN.Equals("Y"))
                                        {
                                            CaseMstAllList = _model.CaseMstData.GetCaseMstSSno(mskSSN.Text, string.Empty, string.Empty, string.Empty);
                                            CaseMstEntity caseMstAlllistEntity = CaseMstAllList.Find(u => u.ApplAgency.Equals(CaseMST.ApplAgency) && u.ApplDept.Equals(CaseMST.ApplDept) && u.ApplProgram.Equals(CaseMST.ApplProgram) && u.ApplYr.Trim().Equals(CaseMST.ApplYr.Trim()) && u.Ssn.Equals(mskSSN.Text));
                                            if (caseMstAlllistEntity != null)
                                            {
                                                _errorProvider.SetError(mskSSN, "Applicant already exists with this SSN in Hierarchy: " + caseMstAlllistEntity.ApplAgency + caseMstAlllistEntity.ApplDept + caseMstAlllistEntity.ApplProgram + caseMstAlllistEntity.ApplYr + " with App No: " + caseMstAlllistEntity.ApplNo + "");
                                                //  mskSSN.Text = strSsnNumber;
                                                mskSSN.Focus();
                                                boolssndata = false;
                                            }
                                        }

                                    }

                                    if (ProgramDefinition.ProDupMEM.Equals("Y"))
                                    {

                                        CaseSnpEntity casesnpssnoEntity = CaseSnpApplicantList.Find(u => u.Agency.Equals(CaseMST.ApplAgency) && u.Dept.Equals(CaseMST.ApplDept) && u.Program.Equals(CaseMST.ApplProgram) && u.Year.Trim().Equals(CaseMST.ApplYr.Trim()) && u.App.Equals(CaseMST.ApplNo) && u.Ssno.Equals(mskSSN.Text));
                                        if (casesnpssnoEntity != null)
                                        {
                                            _errorProvider.SetError(mskSSN, "SSN Number already exist in this family");
                                            // mskSSN.Text = strSsnNumber;
                                            mskSSN.Focus();
                                            boolssndata = false;
                                        }
                                        else
                                        {
                                            CaseSnpEntity casesnpAlllistEntity = CaseSnpApplicantList.Find(u => u.Agency.Equals(CaseMST.ApplAgency) && u.Dept.Equals(CaseMST.ApplDept) && u.Program.Equals(CaseMST.ApplProgram) && u.Year.Trim().Equals(CaseMST.ApplYr.Trim()) && u.Ssno.Equals(mskSSN.Text));
                                            if (casesnpAlllistEntity != null)
                                            {
                                                _errorProvider.SetError(mskSSN, "Member already exists in App#: " + casesnpAlllistEntity.App);
                                                //  mskSSN.Text = strSsnNumber;
                                                mskSSN.Focus();
                                                boolssndata = false;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        CaseSnpEntity casesnpssnoEntity = CaseSnpApplicantList.Find(u => u.Agency.Equals(CaseMST.ApplAgency) && u.Dept.Equals(CaseMST.ApplDept) && u.Program.Equals(CaseMST.ApplProgram) && u.Year.Trim().Equals(CaseMST.ApplYr.Trim()) && u.App.Equals(CaseMST.ApplNo) && u.Ssno.Equals(mskSSN.Text));
                                        if (casesnpssnoEntity != null)
                                        {
                                            _errorProvider.SetError(mskSSN, "SSN Number already exist in this family");
                                            // mskSSN.Text = strSsnNumber;
                                            mskSSN.Focus();
                                            boolssndata = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return boolssndata;
        }


        private void saveCustQuestions(CaseSnpEntity CaseSnpEntity)
        {
            foreach (DataGridViewRow dataGridViewRow in customFieldsControl.GridViewControl.Rows)
            {
                if (true)   //dataGridViewRow.Cells["Response"].Value != null && !dataGridViewRow.Cells["Response"].Value.ToString().Equals(string.Empty))
                {
                    string inputValue = string.Empty;
                    inputValue = dataGridViewRow.Cells["Response"].Value != null ? dataGridViewRow.Cells["Response"].Value.ToString() : string.Empty;
                    if (dataGridViewRow.Cells[2].Tag == null && (dataGridViewRow.Cells[2].Tag != null && !((string)dataGridViewRow.Cells[2].Tag).Equals("U")))
                    {
                        continue;
                    }
                    CustomQuestionsEntity custEntity = new CustomQuestionsEntity();
                    CustomQuestionsEntity questionEntity = dataGridViewRow.Tag as CustomQuestionsEntity;
                    custEntity.ACTAGENCY = CaseSnpEntity.Agency;
                    custEntity.ACTDEPT = CaseSnpEntity.Dept;
                    custEntity.ACTPROGRAM = CaseSnpEntity.Program;
                    if (CaseSnpEntity.Year == string.Empty)
                        custEntity.ACTYEAR = "    ";
                    else
                        custEntity.ACTYEAR = CaseSnpEntity.Year;
                    custEntity.ACTAPPNO = CaseSnpEntity.App;
                    custEntity.ACTSNPFAMILYSEQ = CaseSnpEntity.FamilySeq;
                    if (IsApplicant && questionEntity.CUSTMEMACCESS.Equals("A"))
                        custEntity.ACTSNPFAMILYSEQ = "9999999";
                    if (IsApplicant && questionEntity.CUSTMEMACCESS.Equals("H"))
                        custEntity.ACTSNPFAMILYSEQ = "8888888";

                    custEntity.ACTCODE = questionEntity.CUSTCODE;
                    if (questionEntity.CUSTRESPTYPE.Equals("D"))
                    {
                        if (dataGridViewRow.Cells["Response"].Tag == null) continue;
                        custEntity.ACTMULTRESP = dataGridViewRow.Cells["Response"].Tag.ToString().Trim();
                    }
                    else if (questionEntity.CUSTRESPTYPE.Equals("N"))
                    {
                        if (inputValue == string.Empty) continue;
                        custEntity.ACTNUMRESP = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                    }
                    else if (questionEntity.CUSTRESPTYPE.Equals("T"))
                    {
                        if (inputValue == string.Empty) continue;
                        custEntity.ACTDATERESP = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                    }
                    else if (questionEntity.CUSTRESPTYPE.Equals("C"))
                    {
                        //if (inputValue == string.Empty)
                        //{ continue; }
                        //else
                        //{
                        //    custEntity.ACTALPHARESP = inputValue;
                        //    custEntity.ACTMULTRESP = inputValue;
                        //}

                        if (inputValue == string.Empty)
                        {
                            continue;
                        }
                        else
                        {
                            if (dataGridViewRow.Cells["Response"].Tag == null) continue;
                            else
                            {
                                custEntity.ACTMULTRESP = dataGridViewRow.Cells["Response"].Tag.ToString().Trim();
                                custEntity.ACTALPHARESP = dataGridViewRow.Cells["Response"].Tag.ToString().Trim();
                            }
                        }
                    }
                    else
                    {
                        if (inputValue == string.Empty) continue;
                        custEntity.ACTALPHARESP = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                    }
                    if (dataGridViewRow.Cells[2].Tag is string)
                    {
                        custEntity.Mode = dataGridViewRow.Cells[2].Tag as string;
                    }
                    custEntity.addoperator = BaseForm.UserID;
                    custEntity.lstcoperator = BaseForm.UserID;
                    _model.CaseMstData.InsertUpdateADDCUST(custEntity);
                }
            }
        }

        private bool CheckRealtions()
        {
            bool boolRelation = true;


            if (((ListItem)cmbRelation.SelectedItem).Value.ToString() != "0")
            {
                if (!IsApplicant)
                {
                    if (((ListItem)cmbRelation.SelectedItem).DefaultValue.ToString() == "Y")
                    {
                        if (strRelationCode == string.Empty)
                        {
                            boolRelation = false;
                        }
                    }
                }
                else
                {
                    if (Relationshipdefaultcode != string.Empty)
                    {
                        if (Relationshipdefaultcode != ((ListItem)cmbRelation.SelectedItem).Value.ToString())
                        {
                            //MessageBox.Show(((ListItem)cmbRelation.SelectedItem).Text.ToString() + "\n Can only be selected by primary Applicant", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            boolRelation = false;
                            cmbRelation.SelectedIndexChanged -= new EventHandler(cmbRelation_SelectedIndexChanged);
                            SetComboBoxValue(cmbRelation, Relationshipdefaultcode);
                            cmbRelation.SelectedIndexChanged += new EventHandler(cmbRelation_SelectedIndexChanged);
                        }
                    }
                    //else
                    //    boolRelation = false;
                }
            }
            else
            {
                if (IsApplicant)
                {
                    if (Relationshipdefaultcode != string.Empty)
                    {
                        if (Relationshipdefaultcode != ((ListItem)cmbRelation.SelectedItem).Value.ToString())
                        {
                            if (Relationshipdefaultcode != string.Empty)
                            {
                                //MessageBox.Show(((ListItem)cmbRelation.SelectedItem).Text.ToString() + "\n Can only be selected by primary Applicant", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                boolRelation = false;
                                cmbRelation.SelectedIndexChanged -= new EventHandler(cmbRelation_SelectedIndexChanged);
                                SetComboBoxValue(cmbRelation, Relationshipdefaultcode);
                                cmbRelation.SelectedIndexChanged += new EventHandler(cmbRelation_SelectedIndexChanged);
                            }
                        }
                    }

                }
            }

            return boolRelation;
        }

        private bool CheckRelationsMultiple()
        {
            bool boolRelation = true;
            if (Mode.Equals("Edit"))
            {
                if (Relationshipdefaultcode != string.Empty)
                {
                    List<CaseSnpEntity> casesnprelation = BaseForm.BaseCaseSnpEntity.FindAll(u => u.MemberCode == Relationshipdefaultcode);
                    if (casesnprelation.Count > 0)
                    {
                        if (casesnprelation.Count == 1)
                        {
                            if (casesnprelation[0].FamilySeq != CaseSNP.FamilySeq)
                            {
                                if (Relationshipdefaultcode == ((ListItem)cmbRelation.SelectedItem).Value.ToString())
                                {
                                    boolRelation = false;
                                    MessageBox.Show("Member Exist Same Relation");
                                }
                            }
                        }
                        else
                        {

                            boolRelation = true;
                            //MessageBox.Show("Member Exist Same Relation");

                        }
                    }
                }
            }
            return boolRelation;
        }

        private void saveCustIntakeQuestions(CaseSnpEntity CaseSnpEntity)
        {
            foreach (DataGridViewRow dataGridViewRow in customFieldsIntakeControl.GridViewControl.Rows)
            {
                if (true)   //dataGridViewRow.Cells["Response"].Value != null && !dataGridViewRow.Cells["Response"].Value.ToString().Equals(string.Empty))
                {
                    string inputValue = string.Empty;
                    inputValue = dataGridViewRow.Cells["Response"].Value != null ? dataGridViewRow.Cells["Response"].Value.ToString() : string.Empty;
                    if (dataGridViewRow.Cells[2].Tag == null && (dataGridViewRow.Cells[2].Tag != null && !((string)dataGridViewRow.Cells[2].Tag).Equals("U")))
                    {
                        continue;
                    }
                    CustomQuestionsEntity custEntity = new CustomQuestionsEntity();
                    CustomQuestionsEntity questionEntity = dataGridViewRow.Tag as CustomQuestionsEntity;
                    custEntity.ACTAGENCY = CaseSnpEntity.Agency;
                    custEntity.ACTDEPT = CaseSnpEntity.Dept;
                    custEntity.ACTPROGRAM = CaseSnpEntity.Program;
                    if (CaseSnpEntity.Year == string.Empty)
                        custEntity.ACTYEAR = "    ";
                    else
                        custEntity.ACTYEAR = CaseSnpEntity.Year;
                    custEntity.ACTAPPNO = CaseSnpEntity.App;
                    //custEntity.ACTSNPFAMILYSEQ = CaseSnpEntity.FamilySeq;
                    //if (IsApplicant && custEntity.CUSTMEMACCESS.Equals("A"))
                    //    custEntity.ACTSNPFAMILYSEQ = "9999999";
                    // if (IsApplicant && custEntity.CUSTMEMACCESS.Equals("H"))
                    custEntity.ACTSNPFAMILYSEQ = "8888888";

                    custEntity.ACTCODE = questionEntity.CUSTCODE;
                    if (questionEntity.CUSTRESPTYPE.Equals("D"))
                    {
                        if (dataGridViewRow.Cells["Response"].Tag == null) continue;
                        custEntity.ACTMULTRESP = dataGridViewRow.Cells["Response"].Tag.ToString().Trim();
                    }
                    else if (questionEntity.CUSTRESPTYPE.Equals("N"))
                    {
                        if (inputValue == string.Empty) continue;
                        custEntity.ACTNUMRESP = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                    }
                    else if (questionEntity.CUSTRESPTYPE.Equals("T"))
                    {
                        if (inputValue == string.Empty) continue;
                        custEntity.ACTDATERESP = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                    }
                    else if (questionEntity.CUSTRESPTYPE.Equals("C"))
                    {
                        //if (inputValue == string.Empty)
                        //{ continue; }
                        //else
                        //{
                        //    custEntity.ACTALPHARESP = inputValue;
                        //    custEntity.ACTMULTRESP = inputValue;
                        //}

                        if (inputValue == string.Empty)
                        {
                            continue;
                        }
                        else
                        {
                            if (dataGridViewRow.Cells["Response"].Tag == null) continue;
                            else
                            {
                                custEntity.ACTMULTRESP = dataGridViewRow.Cells["Response"].Tag.ToString().Trim();
                                custEntity.ACTALPHARESP = dataGridViewRow.Cells["Response"].Tag.ToString().Trim();
                            }
                        }
                    }

                    else
                    {
                        if (inputValue == string.Empty) continue;
                        custEntity.ACTALPHARESP = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                    }
                    if (dataGridViewRow.Cells[2].Tag is string)
                    {
                        custEntity.Mode = dataGridViewRow.Cells[2].Tag as string;
                    }
                    custEntity.addoperator = BaseForm.UserID;
                    custEntity.lstcoperator = BaseForm.UserID;
                    _model.CaseMstData.InsertUpdateADDCUST(custEntity);
                }
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {

            SSNSearchForm SSNSearchForm = new SSNSearchForm(BaseForm, "Case2001", CaseSnpAllList, ProgramDefinition, CaseMST, "A", "ALL");
            SSNSearchForm.StartPosition = FormStartPosition.CenterScreen;
            SSNSearchForm.FormClosed += new FormClosedEventHandler(OnSearchFormClosed);
            SSNSearchForm.ShowDialog();
        }

        private void OnSearchFormClosed(object sender, FormClosedEventArgs e)
        {
            SSNSearchForm form = sender as SSNSearchForm;
            if (form.DialogResult == DialogResult.OK)
            {
                CaseMstSnpEntity selectedSsn = form.GetSelectedRow();
                if (selectedSsn != null)
                {
                    mskSSN.Text = selectedSsn.Ssno;
                    AltAgency = selectedSsn.Agency;
                    AltDept = selectedSsn.Dept;
                    AltProgram = selectedSsn.Program;
                    AltYear = selectedSsn.Year;
                    AltApp = selectedSsn.ApplNo.Substring(0, 8);
                    AltFamilySeq = selectedSsn.FamilySeq;
                    SearchSnpType = "SSNSEARCH";
                    SearchSnpType1 = "SSNSEARCH";
                    fillCaseSnpDetails(AltAgency.ToString(), AltDept.ToString(), AltProgram.ToString(), AltYear.ToString(), AltApp.ToString(), AltFamilySeq.ToString());
                    //if (IsApplicant)
                    //{                        
                    //    fillCaseMstDetails(AltAgency.ToString(), AltDept.ToString(), AltProgram.ToString(), AltYear.ToString(), AltApp.ToString());
                    //}

                }
            }
        }

        private void checkNA_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkNA.Checked)
            {
                dtBirth.Checked = false;
                dtBirth.Enabled = false;
                // dtBirth.Text = string.Empty;
                txtYear.Text = "UNK";
                DOBReq.Visible = false;
            }
            else
            {
                if (dtBirth.Checked == false)
                {
                    txtYear.Text = "0";
                    txtCurrentAge.Text = "0";
                }
                foreach (FldcntlHieEntity entity in CntlEntity)
                {
                    bool required = entity.Req.Equals("Y") ? true : false;
                    bool enabled = entity.Enab.Equals("Y") ? true : false;

                    switch (entity.FldCode)
                    {
                        case Consts.CASE2001.DateOfBirth:
                            if (enabled) { dtBirth.Enabled = true; } else { dtBirth.Enabled = false; }
                            if (required) { DOBReq.Visible = true; } else { DOBReq.Visible = false; }
                            break;
                    }
                }

            }
        }

        private void mskSSN_Leave(object sender, EventArgs e)
        {
            string strssn = mskSSN.Text.Replace(" ", string.Empty);
            SearchSnpType1 = string.Empty;
            if (strssn.Length <= 9 && strssn.Length > 0)
            {
                lblSSNReasonReq.Visible = false;
                lblSSNReason.Visible = false;
                cmbSSNReason.Visible = false;
                cmbSSNReason.SelectedIndex = 0;
                if (mskSSN.Text == "000000000" || mskSSN.Text.ToUpper() == "NEWSSNNUM")
                {
                    if (ProgramDefinition.SSNReasonFlag == "Y")
                    {
                        lblSSNReasonReq.Visible = true;
                        lblSSNReason.Visible = true;
                        cmbSSNReason.Visible = true;
                        lblSSNReason.Enabled = true;
                        cmbSSNReason.Enabled = true;
                    }
                    else
                    {
                        lblSSNReasonReq.Visible = false;
                        lblSSNReason.Visible = false;
                        cmbSSNReason.Visible = false;
                    }
                }
            }

            if (strTabPress == "Tabchanged")
            {
                strTabPress = string.Empty;
                if (strssn.Length <= 9 && strssn.Length > 0)
                {
                    //mskSSN.Text = mskSSN.Text.Replace(' ', '0') + SsnZeros(mskSSN.TextLength, "SSN");
                    if (mskSSN.Text == "000000000" || mskSSN.Text.ToUpper() == "NEWSSNNUM")
                    {
                        //if (ProgramDefinition.SSNReasonFlag == "Y")
                        //{
                        //    lblSSNReasonReq.Visible = true;
                        //    lblSSNReason.Visible = true;
                        //    cmbSSNReason.Visible = true;
                        //    lblSSNReason.Enabled = true;
                        //    cmbSSNReason.Enabled = true;
                        //}
                        //else
                        //{
                        //    lblSSNReasonReq.Visible = false;
                        //    lblSSNReason.Visible = false;
                        //    cmbSSNReason.Visible = false;
                        //}
                    }
                    else
                    {
                        lblSSNReasonReq.Visible = false;
                        lblSSNReason.Visible = false;
                        cmbSSNReason.Visible = false;
                        cmbSSNReason.SelectedIndex = 0;
                        if (Mode.Equals("Add"))
                        {
                            SearchSnpType = string.Empty;
                            SearchSnpType1 = string.Empty;
                            if (mskSSN.TextLength == 9)
                            {
                                if (mskSSN.Text != "000000000" && mskSSN.Text.ToUpper() != "NEWSSNNUM")
                                {
                                    List<CaseSnpEntity> CaseSnpApplicantList = _model.CaseMstData.GetCaseSnpSSno(mskSSN.Text, string.Empty, string.Empty, string.Empty);
                                    if (IsApplicant)
                                    {
                                        if (ProgramDefinition.PRODUPSSN.Equals("Y"))
                                        {
                                            CaseMstAllList = _model.CaseMstData.GetCaseMstSSno(mskSSN.Text, string.Empty, string.Empty, string.Empty);
                                            CaseMstEntity caseMstAlllistEntity = CaseMstAllList.Find(u => u.ApplAgency.Equals(CaseMST.ApplAgency) && u.ApplDept.Equals(CaseMST.ApplDept) && u.ApplProgram.Equals(CaseMST.ApplProgram) && u.ApplYr.Trim().Equals(CaseMST.ApplYr.Trim()) && u.Ssn.Equals(mskSSN.Text));
                                            if (caseMstAlllistEntity != null)
                                            {
                                                CommonFunctions.MessageBoxDisplay("Applicant already exists with this SSN in Hierarchy: " + caseMstAlllistEntity.ApplAgency + caseMstAlllistEntity.ApplDept + caseMstAlllistEntity.ApplProgram + caseMstAlllistEntity.ApplYr + " with App No: " + caseMstAlllistEntity.ApplNo + "\n You can add only new Applicant here");
                                                mskSSN.Text = string.Empty;
                                                mskSSN.Focus();
                                                return;
                                            }
                                        }
                                        //else
                                        //{
                                        //    if (CaseSnpApplicantList.Count > 0)
                                        //    {

                                        //            CommonFunctions.MessageBoxDisplay("Member already exist in Hierarchy: " + CaseSnpApplicantList[0].Agency + CaseSnpApplicantList[0].Dept + CaseSnpApplicantList[0].Program + CaseSnpApplicantList[0].Year + " with App No: " + CaseSnpApplicantList[0].App + "\n You can add only new Applicant here");
                                        //            mskSSN.Text = string.Empty;
                                        //            mskSSN.Focus();
                                        //            return;

                                        //    }
                                        //}

                                    }

                                    if (ProgramDefinition.ProDupMEM.Equals("Y"))
                                    {

                                        CaseSnpEntity casesnpssnoEntity = CaseSnpApplicantList.Find(u => u.Agency.Equals(CaseMST.ApplAgency) && u.Dept.Equals(CaseMST.ApplDept) && u.Program.Equals(CaseMST.ApplProgram) && u.Year.Trim().Equals(CaseMST.ApplYr.Trim()) && u.App.Equals(CaseMST.ApplNo) && u.Ssno.Equals(mskSSN.Text));
                                        if (casesnpssnoEntity != null)
                                        {
                                            CommonFunctions.MessageBoxDisplay("SSN Number already exist in this family");
                                            mskSSN.Text = string.Empty;
                                            mskSSN.Focus();
                                        }

                                        else
                                        {
                                            CaseSnpEntity casesnpAlllistEntity = CaseSnpApplicantList.Find(u => u.Agency.Equals(CaseMST.ApplAgency) && u.Dept.Equals(CaseMST.ApplDept) && u.Program.Equals(CaseMST.ApplProgram) && u.Year.Trim().Equals(CaseMST.ApplYr.Trim()) && u.Ssno.Equals(mskSSN.Text));
                                            if (casesnpAlllistEntity != null)
                                            {
                                                CommonFunctions.MessageBoxDisplay("Member already exists in App#: " + casesnpAlllistEntity.App);
                                                mskSSN.Text = string.Empty;
                                                mskSSN.Focus();
                                            }
                                            else
                                            {
                                                List<CaseSnpEntity> casesnpssnosearchEntity = CaseSnpApplicantList.FindAll(u => u.Ssno.Equals(mskSSN.Text));
                                                if (casesnpssnosearchEntity != null)
                                                {
                                                    if (casesnpssnosearchEntity.Count != 0)
                                                    {
                                                        if (!IsApplicant)
                                                        {
                                                            if (casesnpssnosearchEntity.Count == 1)
                                                            {
                                                                // mskSSN.Text = selectedSsn.Ssno;
                                                                AltAgency = casesnpssnosearchEntity[0].Agency;
                                                                AltDept = casesnpssnosearchEntity[0].Dept;
                                                                AltProgram = casesnpssnosearchEntity[0].Program;
                                                                AltYear = casesnpssnosearchEntity[0].Year;
                                                                AltApp = casesnpssnosearchEntity[0].App.Substring(0, 8);
                                                                AltFamilySeq = casesnpssnosearchEntity[0].FamilySeq;
                                                                SearchSnpType = "SSNSEARCH";
                                                                SearchSnpType1 = "SSNSEARCH";
                                                                fillCaseSnpDetails(AltAgency.ToString(), AltDept.ToString(), AltProgram.ToString(), AltYear.ToString(), AltApp.ToString(), AltFamilySeq.ToString());
                                                                //if (IsApplicant)
                                                                //{
                                                                //    fillCaseMstDetails(AltAgency.ToString(), AltDept.ToString(), AltProgram.ToString(), AltYear.ToString(), AltApp.ToString());
                                                                //}

                                                            }
                                                            else
                                                            {
                                                                SsnScanForm SsnScanForm = new SsnScanForm(BaseForm, mskSSN.Text);
                                                                SsnScanForm.StartPosition = FormStartPosition.CenterScreen;
                                                                SsnScanForm.FormClosed += new FormClosedEventHandler(OnSsnScanSearchFormClosed);
                                                                SsnScanForm.ShowDialog();
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }

                                    }
                                    else
                                    {
                                        CaseSnpEntity casesnpssnoEntity = CaseSnpApplicantList.Find(u => u.Agency.Equals(CaseMST.ApplAgency) && u.Dept.Equals(CaseMST.ApplDept) && u.Program.Equals(CaseMST.ApplProgram) && u.Year.Trim().Equals(CaseMST.ApplYr.Trim()) && u.App.Equals(CaseMST.ApplNo) && u.Ssno.Equals(mskSSN.Text));
                                        if (casesnpssnoEntity != null)
                                        {
                                            CommonFunctions.MessageBoxDisplay("SSN Number already exist in this family");
                                            mskSSN.Text = string.Empty;
                                            mskSSN.Focus();
                                        }
                                        else
                                        {
                                            List<CaseSnpEntity> casesnpssnosearchEntity = CaseSnpApplicantList.FindAll(u => u.Ssno.Equals(mskSSN.Text)); // _model.CaseMstData.GetCaseSnpSSno(mskSSN.Text);// CaseSnpAllList.FindAll(u => u.Ssno.Equals(mskSSN.Text));
                                            if (casesnpssnosearchEntity != null)
                                            {
                                                if (casesnpssnosearchEntity.Count != 0)
                                                {
                                                    if (!IsApplicant)
                                                    {
                                                        if (casesnpssnosearchEntity.Count == 1)
                                                        {
                                                            // mskSSN.Text = selectedSsn.Ssno;
                                                            AltAgency = casesnpssnosearchEntity[0].Agency;
                                                            AltDept = casesnpssnosearchEntity[0].Dept;
                                                            AltProgram = casesnpssnosearchEntity[0].Program;
                                                            AltYear = casesnpssnosearchEntity[0].Year;
                                                            AltApp = casesnpssnosearchEntity[0].App.Substring(0, 8);
                                                            AltFamilySeq = casesnpssnosearchEntity[0].FamilySeq;
                                                            SearchSnpType = "SSNSEARCH";
                                                            SearchSnpType1 = "SSNSEARCH";
                                                            fillCaseSnpDetails(AltAgency.ToString(), AltDept.ToString(), AltProgram.ToString(), AltYear.ToString(), AltApp.ToString(), AltFamilySeq.ToString());
                                                            //if (IsApplicant)
                                                            //{
                                                            //    fillCaseMstDetails(AltAgency.ToString(), AltDept.ToString(), AltProgram.ToString(), AltYear.ToString(), AltApp.ToString());
                                                            //}
                                                        }
                                                        else
                                                        {
                                                            SsnScanForm SsnScanForm = new SsnScanForm(BaseForm, mskSSN.Text);
                                                            SsnScanForm.StartPosition = FormStartPosition.CenterScreen;
                                                            SsnScanForm.FormClosed += new FormClosedEventHandler(OnSsnScanSearchFormClosed);
                                                            SsnScanForm.ShowDialog();
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                        else if (Mode.Equals(Consts.Common.Edit))
                        {
                            if (mskSSN.Text != strSsnNumber)
                            {
                                if (mskSSN.TextLength == 9)
                                {
                                    if (mskSSN.Text != "000000000")
                                    {
                                        List<CaseSnpEntity> CaseSnpApplicantList = _model.CaseMstData.GetCaseSnpSSno(mskSSN.Text, string.Empty, string.Empty, string.Empty);
                                        if (IsApplicant)
                                        {
                                            if (ProgramDefinition.PRODUPSSN.Equals("Y"))
                                            {
                                                CaseMstAllList = _model.CaseMstData.GetCaseMstSSno(mskSSN.Text, string.Empty, string.Empty, string.Empty);
                                                CaseMstEntity caseMstAlllistEntity = CaseMstAllList.Find(u => u.ApplAgency.Equals(CaseMST.ApplAgency) && u.ApplDept.Equals(CaseMST.ApplDept) && u.ApplProgram.Equals(CaseMST.ApplProgram) && u.ApplYr.Trim().Equals(CaseMST.ApplYr.Trim()) && u.Ssn.Equals(mskSSN.Text));
                                                if (caseMstAlllistEntity != null)
                                                {
                                                    CommonFunctions.MessageBoxDisplay("Applicant already exists with this SSN in Hierarchy: " + caseMstAlllistEntity.ApplAgency + caseMstAlllistEntity.ApplDept + caseMstAlllistEntity.ApplProgram + caseMstAlllistEntity.ApplYr + " with App No: " + caseMstAlllistEntity.ApplNo + "");
                                                    //mskSSN.Text = strSsnNumber;
                                                    mskSSN.Focus();
                                                    return;
                                                }
                                            }
                                            //else
                                            //{
                                            //    if (CaseSnpApplicantList.Count > 0)
                                            //    {
                                            //        CommonFunctions.MessageBoxDisplay("Member already exist in Hierarchy: " + CaseSnpApplicantList[0].Agency + CaseSnpApplicantList[0].Dept + CaseSnpApplicantList[0].Program + CaseSnpApplicantList[0].Year + " with App No: " + CaseSnpApplicantList[0].App + "");
                                            //        mskSSN.Text = strSsnNumber;
                                            //        mskSSN.Focus();
                                            //        return;
                                            //    }
                                            //}

                                        }

                                        if (ProgramDefinition.ProDupMEM.Equals("Y"))
                                        {

                                            CaseSnpEntity casesnpssnoEntity = CaseSnpApplicantList.Find(u => u.Agency.Equals(CaseMST.ApplAgency) && u.Dept.Equals(CaseMST.ApplDept) && u.Program.Equals(CaseMST.ApplProgram) && u.Year.Trim().Equals(CaseMST.ApplYr.Trim()) && u.App.Equals(CaseMST.ApplNo) && u.Ssno.Equals(mskSSN.Text));
                                            if (casesnpssnoEntity != null)
                                            {
                                                CommonFunctions.MessageBoxDisplay("SSN Number already exist in this family");
                                                //mskSSN.Text = strSsnNumber;
                                                mskSSN.Focus();
                                            }

                                            else
                                            {
                                                CaseSnpEntity casesnpAlllistEntity = CaseSnpApplicantList.Find(u => u.Agency.Equals(CaseMST.ApplAgency) && u.Dept.Equals(CaseMST.ApplDept) && u.Program.Equals(CaseMST.ApplProgram) && u.Year.Trim().Equals(CaseMST.ApplYr.Trim()) && u.Ssno.Equals(mskSSN.Text));
                                                if (casesnpAlllistEntity != null)
                                                {
                                                    CommonFunctions.MessageBoxDisplay("Member already exists in App#: " + casesnpAlllistEntity.App);
                                                    //mskSSN.Text = strSsnNumber;
                                                    mskSSN.Focus();
                                                }
                                            }



                                        }
                                        else
                                        {
                                            CaseSnpEntity casesnpssnoEntity = CaseSnpApplicantList.Find(u => u.Agency.Equals(CaseMST.ApplAgency) && u.Dept.Equals(CaseMST.ApplDept) && u.Program.Equals(CaseMST.ApplProgram) && u.Year.Trim().Equals(CaseMST.ApplYr.Trim()) && u.App.Equals(CaseMST.ApplNo) && u.Ssno.Equals(mskSSN.Text));
                                            if (casesnpssnoEntity != null)
                                            {
                                                CommonFunctions.MessageBoxDisplay("SSN Number already exist in this family");
                                                //mskSSN.Text = strSsnNumber;
                                                mskSSN.Focus();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void fillCaseMstDetails(string strAgency, string strDept, string strProgram, string strYear, string strAppNo)
        {
            CaseMstEntity CaseMstfillDetails = _model.CaseMstData.GetCaseMST(strAgency, strDept, strProgram, strYear, strAppNo);
            if (CaseMstfillDetails != null)
            {
                if (Mode.Equals("Add"))
                {
                    tabControl1.TabPages[2].Hidden = false;
                }
                txtBIC.Text = CaseMstfillDetails.Bic.Trim();
                txtHN.Text = CaseMstfillDetails.Hn.Trim();
                txtDirection.Text = CaseMstfillDetails.Direction.Trim();
                txtSuffix.Text = CaseMstfillDetails.Suffix.Trim();
                txtStreet.Text = CaseMstfillDetails.Street.Trim();
                txtFloor.Text = CaseMstfillDetails.Flr;
                txtPrecinct.Text = CaseMstfillDetails.Precinct;
                txtApartment.Text = CaseMstfillDetails.Apt;
                if (CaseMstfillDetails.ActiveStatus.Equals("Y")) cbActive.Checked = true; else cbActive.Checked = false;
                if (CaseMstfillDetails.Secret.Equals("Y")) cbSecret.Checked = true; else cbSecret.Checked = false;
                //txtZipCode.Text = CaseMstfillDetails.Zip + CaseMstfillDetails.Zipplus;
                if (CaseMstfillDetails.Zip != string.Empty)
                    txtZipCode.Text = "00000".Substring(0, 5 - CaseMstfillDetails.Zip.Length) + CaseMstfillDetails.Zip;
                if (CaseMstfillDetails.Zipplus != string.Empty)
                    txtZipPlus.Text = "0000".Substring(0, 4 - CaseMstfillDetails.Zipplus.Length) + CaseMstfillDetails.Zipplus;
                txtCity.Text = CaseMstfillDetails.City.Trim();
                txtState.Text = CaseMstfillDetails.State;
                txtSite.Text = CaseMstfillDetails.Site.Trim();
                if (!CaseMstfillDetails.IntakeDate.Equals(string.Empty)) { dtpIntakeDate.Checked = true; dtpIntakeDate.Text = CaseMstfillDetails.IntakeDate; }
                if (!CaseMstfillDetails.CaseReviewDate.Equals(string.Empty)) { dtpCaseReview.Checked = true; dtpCaseReview.Text = CaseMstfillDetails.CaseReviewDate; }
                if (!CaseMstfillDetails.InitialDate.Equals(string.Empty)) { dtpInitialDate.Checked = true; dtpInitialDate.Text = CaseMstfillDetails.InitialDate; }

                SetComboBoxValue(cmbAboutUs, CaseMstfillDetails.AboutUs);
                SetComboBoxValue(cmbCaseType, CaseMstfillDetails.CaseType);
                SetComboBoxValue(cmbContact, CaseMstfillDetails.BestContact);
                SetComboBoxValue(cmbCounty, CaseMstfillDetails.County);
                SetComboBoxValue(cmbFamilyType, CaseMstfillDetails.FamilyType);
                cmbFamilyType_SelectedIndexChanged(cmbFamilyType, new EventArgs());
                SetComboBoxValue(cmbHousingSituation, CaseMstfillDetails.Housing);
                SetComboBoxValue(cmbPrimaryLang, CaseMstfillDetails.Language);
                SetComboBoxValue(cmbSecondLang, CaseMstfillDetails.LanguageOt);
                SetComboBoxValue(cmbStaff, CaseMstfillDetails.IntakeWorker);
                SetComboBoxValue(cmbTownship, CaseMstfillDetails.TownShip);
                SetComboBoxValue(cmbWaitingList, CaseMstfillDetails.WaitList);
                SetComboBoxValue(cmbVerifiedStaff, CaseMstfillDetails.ExpCaseWorker);

                txtHomePhone.Text = CaseMstfillDetails.Area + CaseMstfillDetails.Phone;

                txtCell.Text = CaseMstfillDetails.CellPhone;
                txtMessage.Text = CaseMstfillDetails.MessagePhone;
                txtTTY.Text = CaseMstfillDetails.TtyNumber;
                txtFax.Text = CaseMstfillDetails.FaxNumber;
                txtEmail.Text = CaseMstfillDetails.Email.Trim();

                txtRent.Text = CaseMstfillDetails.ExpRent;
                txtHeating.Text = CaseMstfillDetails.ExpHeat;
                txtWater.Text = CaseMstfillDetails.ExpWater;
                txtElectric.Text = CaseMstfillDetails.ExpElectric;
                txtExpand1.Text = CaseMstfillDetails.Debtcc;
                txtExpand2.Text = CaseMstfillDetails.DebtLoans;
                txtExpand3.Text = CaseMstfillDetails.DebtMed;
                txtExpand4.Text = CaseMstfillDetails.DebtOther;
                txtTotalLiving.Text = CaseMstfillDetails.ExpLivexpense;
                if (CaseMstfillDetails.ProgIncome.ToString() != string.Empty)
                {
                    strProgIncome = CaseMstfillDetails.ProgIncome;
                    double MonthlyIncome = double.Parse(CaseMstfillDetails.ProgIncome);
                    if (MonthlyIncome > 0)
                    {
                        MonthlyIncome = MonthlyIncome / 12;
                        MonthlyIncome = Math.Round(MonthlyIncome, 2);
                        txtMonthlyIncome.Text = MonthlyIncome.ToString();
                    }
                    else
                    {
                        txtMonthlyIncome.Text = "0";
                    }
                }
                else
                    txtMonthlyIncome.Text = "0";
                txtTotalHouseHold.Text = CaseMstfillDetails.ExpTotal;
                txtNoOfYearsAtAddress.Text = CaseMstfillDetails.AddressYears;
                DataSet serviceDS = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE(string.Empty, CaseMstfillDetails.ApplAgency, CaseMstfillDetails.ApplDept, CaseMstfillDetails.ApplProgram, CaseMstfillDetails.ApplYr, CaseMstfillDetails.ApplNo);
                DataTable serviceDT = serviceDS.Tables[0];
                List<string> serviceList = new List<string>();
                gvwServices.Rows.Clear();
                cmbServicesInquired.Items.Clear();
                foreach (DataRow dr in serviceDT.Rows)
                {
                    gvwServices.Rows.Add(false, dr["INQ_DESC"].ToString(), dr["INQ_CODE"].ToString());
                    ListItem li = new ListItem(dr["INQ_DESC"].ToString(), dr["INQ_CODE"].ToString());
                    cmbServicesInquired.Items.Add(li);
                    // listBoxSelectionControl1.ListBoxSelected.Items.Add(new ListItem(dr["INQ_DESC"].ToString(), dr["INQ_CODE"].ToString()));
                    serviceList.Add(dr["INQ_CODE"].ToString());
                }
                cmbServicesInquired.Items.Insert(0, new ListItem("Select One", "0"));
                cmbServicesInquired.SelectedIndex = 0;

                DataSet serviceSaveDS = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE("SAVE", CaseMstfillDetails.ApplAgency, CaseMstfillDetails.ApplDept, CaseMstfillDetails.ApplProgram, CaseMstfillDetails.ApplYr, CaseMstfillDetails.ApplNo);
                DataTable serviceSaveDT = serviceSaveDS.Tables[0];
                List<string> serviceSaveList = new List<string>();
                if (serviceSaveDT.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in gvwServices.Rows)
                    {
                        if (row.Cells["ServiceCode"].Value != null)
                        {
                            for (int i = 0; i < serviceSaveDT.Rows.Count; i++)
                            {
                                if (Convert.ToString(row.Cells["ServiceCode"].Value.ToString().Trim()) == serviceSaveDT.Rows[i]["INQ_CODE"].ToString().Trim())
                                {
                                    row.Cells["Servicechk"].Value = true; break;
                                }
                            }

                        }
                    }
                }

            }
        }


        private void OnSsnScanSearchFormClosed(object sender, FormClosedEventArgs e)
        {

            SsnScanForm form = sender as SsnScanForm;

            if (form.DialogResult == DialogResult.OK)
            {
                CaseMstSnpEntity selectedSsn = form.GetSelectedRow();
                if (selectedSsn != null)
                {
                    mskSSN.Text = selectedSsn.Ssno;
                    AltAgency = selectedSsn.Agency;
                    AltDept = selectedSsn.Dept;
                    AltProgram = selectedSsn.Program;
                    AltYear = selectedSsn.Year;
                    AltApp = selectedSsn.ApplNo.Substring(0, 8);
                    AltFamilySeq = selectedSsn.FamilySeq;
                    SearchSnpType = "SSNSEARCH";
                    SearchSnpType1 = "SSNSEARCH";
                    fillCaseSnpDetails(AltAgency.ToString(), AltDept.ToString(), AltProgram.ToString(), AltYear.ToString(), AltApp.ToString(), AltFamilySeq.ToString());
                    //if (IsApplicant)
                    //{
                    //    fillCaseMstDetails(AltAgency.ToString(), AltDept.ToString(), AltProgram.ToString(), AltYear.ToString(), AltApp.ToString());
                    //}
                }
            }
        }

        private void OnCancleClick(object sender, EventArgs e)
        {
            Case3001Control Case3001Control = BaseForm.GetBaseUserControl() as Case3001Control;
            if (Case3001Control != null)
            {
                Case3001Control.Refresh();
            }
            Refresh_Control = "Close";
            this.Close();
        }

        private void OnMaxMinClick(object sender, EventArgs e)
        {
            if (!customFieldsControl.IsMax)
            {
                splitApplicant.SplitterDistance = 100;
                customFieldsControl.IsMax = true;
                customFieldsControl.MaxButtonControl.ImageSource = "minimize?color=captainBlue";
            }
            else
            {
                splitApplicant.SplitterDistance = 358;
                customFieldsControl.IsMax = false;
                customFieldsControl.MaxButtonControl.ImageSource = "expand?color=captainBlue";
            }
        }

        private void OnIntakeMaxMinClick(object sender, EventArgs e)
        {
            if (!customFieldsIntakeControl.IsMax)
            {
                splitIntake.SplitterDistance = 100;
                customFieldsIntakeControl.IsMax = true;
                customFieldsIntakeControl.MaxButtonControl.ImageSource = "expand?color=captainBlue";
            }
            else
            {
                splitIntake.SplitterDistance = 311;
                customFieldsIntakeControl.IsMax = false;
                customFieldsIntakeControl.MaxButtonControl.ImageSource = "minimize?color=captainBlue";
            }
        }

        private void fillSNPDetails()
        {
            CaseSnpEntity CaseSnpDetails = CaseSNP;
            if (CaseSnpDetails != null)
            {
                strYear = CaseSnpDetails.Year;
                strApp = CaseSnpDetails.App;
                mskSSN.Text = CaseSnpDetails.Ssno;
                strSsnNumber = CaseSnpDetails.Ssno;
                txtBIC.Text = CaseSnpDetails.SsBic.Trim();
                txtFirstName.Text = CaseSnpDetails.NameixFi.Trim();
                txtLastName.Text = CaseSnpDetails.NameixLast.Trim();
                txtAppSuffix.Text = CaseSnpDetails.SnpSuffix.Trim();
                txtMI.Text = CaseSnpDetails.NameixMi.Trim();
                txtAlias.Text = CaseSnpDetails.Alias.Trim();
                txtAlienRegNo.Text = CaseSnpDetails.AlienRegNo.Trim();
                if (!CaseSnpDetails.ExpireWorkDate.Equals(string.Empty))
                {
                    dtExpirationdate.Text = CaseSnpDetails.ExpireWorkDate;
                    dtExpirationdate.Checked = true;
                }
                else
                {
                    dtExpirationdate.Value = DateTime.Now.Date;
                    dtExpirationdate.Checked = false;
                }

                if (CaseSnpDetails.DobNa.Equals("1"))
                {
                    checkNA.Checked = true;
                    dtBirth.Text = string.Empty;
                    dtBirth.Checked = false;
                    dtBirth.Enabled = false;
                    DOBReq.Visible = false;
                    txtYear.Text = "UNK";
                    txtCurrentAge.Text = string.Empty;
                }
                else
                {
                    checkNA.Checked = false;
                    if (!CaseSnpDetails.AltBdate.Equals(string.Empty))
                    {
                        dtBirth.Text = CaseSnpDetails.AltBdate;
                        dtBirth.Checked = true;
                    }
                    else dtBirth.Checked = false;
                    // txtYear.Text = CaseSnpDetails.Age;
                }
                if (CaseSnpDetails.Status.Trim().Equals("A"))
                {
                    ckActive.Checked = true;
                    ckExcludeMember.Enabled = true;
                }
                else
                {
                    ckExcludeMember.Enabled = false;
                }
                if (CaseSnpDetails.Exclude.Trim().Equals("Y"))
                {
                    IsExclude = true;
                    ckExcludeMember.Checked = true;
                }

                if (CaseSnpDetails.Snp_HH_ExcludeMem.Trim().Equals("Y"))
                {
                    chkExcludeHHMem.Checked = true;
                }
                else
                    chkExcludeHHMem.Checked = false;




                SetComboBoxValue(cmbRelation, CaseSnpDetails.MemberCode);
                if (CaseSnpDetails.MemberCode != string.Empty)
                    CaseSNP.RelationDesc = ((ListItem)cmbRelation.SelectedItem).Text.ToString();

                if (IsApplicant && CaseSnpDetails.MemberCode.Trim() == Relationshipdefaultcode.Trim())
                    cmbRelation.Enabled = false;
                SetComboBoxValue(cmbEthnicity, CaseSnpDetails.Ethnic);
                if (CaseSnpDetails.Ethnic != string.Empty)
                    CaseSNP.EthnicityDesc = ((ListItem)cmbEthnicity.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbRace, CaseSnpDetails.Race);
                if (CaseSnpDetails.Race != string.Empty)
                    CaseSNP.RaceDesc = ((ListItem)cmbRace.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbEducation, CaseSnpDetails.Education);
                if (CaseSnpDetails.Education != string.Empty)
                    CaseSNP.EducationDesc = ((ListItem)cmbEducation.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbSchool, CaseSnpDetails.SchoolDistrict);
                if (CaseSnpDetails.SchoolDistrict != string.Empty)
                    CaseSNP.SchoolDesc = ((ListItem)cmbSchool.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbGender, CaseSnpDetails.Sex);
                if (CaseSnpDetails.Sex != string.Empty)
                    CaseSNP.GenderDesc = ((ListItem)cmbGender.SelectedItem).Text.ToString();
                if (!CaseSnpDetails.Sex.Equals("F") && !CaseSnpDetails.Sex.Equals("T"))
                {
                    cmbAreyoupregnant.Enabled = false;
                    lblAreYouPregnant.Enabled = false;
                    PregnantReq.Visible = false;
                    SetComboBoxValue(cmbAreyoupregnant, Consts.Common.SelectOne);
                }
                else
                {
                    SetComboBoxValue(cmbAreyoupregnant, CaseSnpDetails.Pregnant);
                    if (CaseSnpDetails.Pregnant != string.Empty)
                        CaseSNP.AreYouPregantDesc = ((ListItem)cmbAreyoupregnant.SelectedItem).Text.ToString();
                }
                SetComboBoxValue(cmbSSNReason, CaseSnpDetails.SsnReason);
                if (CaseSnpDetails.SsnReason != string.Empty)
                    CaseSNP.SsnReasonDesc = ((ListItem)cmbSSNReason.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbMaritalStatus, CaseSnpDetails.MaritalStatus);
                if (CaseSnpDetails.MaritalStatus != string.Empty)
                    CaseSNP.MartialStatusDesc = ((ListItem)cmbMaritalStatus.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbHealthInsurance, CaseSnpDetails.HealthIns);
                if (CaseSnpDetails.HealthIns != string.Empty)
                    CaseSNP.HealthInsuranceDesc = ((ListItem)cmbHealthInsurance.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbVeteranCode, CaseSnpDetails.Vet);
                if (CaseSnpDetails.Vet != string.Empty)
                    CaseSNP.VeterncodeDesc = ((ListItem)cmbVeteranCode.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbFoodStamps, CaseSnpDetails.FootStamps);
                if (CaseSnpDetails.FootStamps != string.Empty)
                    CaseSNP.FoodStampsDesc = ((ListItem)cmbFoodStamps.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbWIC, CaseSnpDetails.Wic);
                if (CaseSnpDetails.Wic != string.Empty)
                    CaseSNP.WicDesc = ((ListItem)cmbWIC.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbFarmer, CaseSnpDetails.Farmer);
                if (CaseSnpDetails.Farmer != string.Empty)
                    CaseSNP.farmerDesc = ((ListItem)cmbFarmer.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbDisabled, CaseSnpDetails.Disable);
                if (CaseSnpDetails.Disable != string.Empty)
                    CaseSNP.DisabledDesc = ((ListItem)cmbDisabled.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbDriving, CaseSnpDetails.Drvlic);
                if (CaseSnpDetails.Drvlic != string.Empty)
                    CaseSNP.DriverLicenceDesc = ((ListItem)cmbDriving.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbReliableTrans, CaseSnpDetails.Relitran);
                if (CaseSnpDetails.Relitran != string.Empty)
                    CaseSNP.ReliableTransportDesc = ((ListItem)cmbReliableTrans.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbResident, CaseSnpDetails.Resident);
                if (CaseSnpDetails.Resident != string.Empty)
                    CaseSNP.ResidentDesc = ((ListItem)cmbResident.SelectedItem).Text.ToString();
                cmbresidentchangefunction();
                //cmbResident_SelectedIndexChanged(cmbResident, new EventArgs());
                txtAlienRegNo.Text = CaseSnpDetails.AlienRegNo.Trim();
                if (!CaseSnpDetails.ExpireWorkDate.Equals(string.Empty))
                {
                    dtExpirationdate.Text = CaseSnpDetails.ExpireWorkDate;
                    dtExpirationdate.Checked = true;
                }
                else
                {
                    dtExpirationdate.Value = DateTime.Now.Date;
                    dtExpirationdate.Checked = false;
                }
                SetComboBoxValue(cmbLegaltowork, CaseSnpDetails.LegalTowork);
                if (CaseSnpDetails.LegalTowork != string.Empty)
                    CaseSNP.LegaltoworkDesc = ((ListItem)cmbLegaltowork.SelectedItem).Text.ToString();
                //************** Employee Details *****************//
                SetComboBoxValue(cmbEpresenteEmploy, CaseSnpDetails.Employed);
                if (CaseSnpDetails.Employed != string.Empty)
                    CaseSNP.PresentEmployDesc = ((ListItem)cmbEpresenteEmploy.SelectedItem).Text.ToString();
                if (!CaseSnpDetails.LastWorkDate.Equals(string.Empty))
                {
                    dtElastDateWorked.Text = CaseSnpDetails.LastWorkDate;
                    dtElastDateWorked.Checked = true;
                }
                else
                {
                    dtElastDateWorked.Value = DateTime.Now.Date;
                    dtElastDateWorked.Checked = false;
                }
                SetComboBoxValue(cmbEAnywork, CaseSnpDetails.WorkLimit);
                if (CaseSnpDetails.WorkLimit != string.Empty)
                    CaseSNP.AnyworklimitationDesc = ((ListItem)cmbEAnywork.SelectedItem).Text.ToString();
                txtEifyesexplain.Text = CaseSnpDetails.ExplainWorkLimit;
                txtEcurrentHave.Text = CaseSnpDetails.NumberOfcjobs;
                txtElastvisit.Text = CaseSnpDetails.NumberofLvjobs;
                txtEFullTime.Text = CaseSnpDetails.FullTimeHours;
                txtEPartTime.Text = CaseSnpDetails.PartTimeHours;
                SetComboBoxValue(cmbEseasonalEmployee, CaseSnpDetails.SeasonalEmploy);
                if (CaseSnpDetails.SeasonalEmploy != string.Empty)
                    CaseSNP.SeasonEmployedDesc = ((ListItem)cmbEseasonalEmployee.SelectedItem).Text.ToString();
                if (CaseSnpDetails.IstShift.Trim().Equals("Y"))
                    chkE1st.Checked = true;
                if (CaseSnpDetails.IIndShift.Trim().Equals("Y"))
                    chkE2nd.Checked = true;
                if (CaseSnpDetails.IIIrdShift.Trim().Equals("Y"))
                    chkE3rd.Checked = true;
                if (CaseSnpDetails.RShift.Trim().Equals("Y"))
                    chkErotaing.Checked = true;
                txtEEmployer.Text = CaseSnpDetails.EmployerName;
                txtEstreet.Text = CaseSnpDetails.EmployerStreet;
                txtEcityState.Text = CaseSnpDetails.EmployerCity;
                mskEPhone.Text = CaseSnpDetails.EmplPhone;
                txtEExt.Text = CaseSnpDetails.EmplExt;
                SetComboBoxValue(cmbEJobTitle, CaseSnpDetails.JobTitle);
                if (CaseSnpDetails.JobTitle != string.Empty)
                    CaseSNP.JobTitleDesc = ((ListItem)cmbEJobTitle.SelectedItem).Text.ToString();
                SetComboBoxValue(cmbEJobCategory, CaseSnpDetails.JobCategory);
                if (CaseSnpDetails.JobCategory != string.Empty)
                    CaseSNP.JobCategoryDesc = ((ListItem)cmbEJobCategory.SelectedItem).Text.ToString();
                txtEhourlywage.Text = CaseSnpDetails.HourlyWage;
                SetComboBoxValue(cmbEpayFrequency, CaseSnpDetails.PayFrequency);
                if (CaseSnpDetails.PayFrequency != string.Empty)
                    CaseSNP.PayFrequencyDesc = ((ListItem)cmbEpayFrequency.SelectedItem).Text.ToString();
                if (!CaseSnpDetails.HireDate.Equals(string.Empty))
                {
                    dtEHireDate.Text = CaseSnpDetails.HireDate;
                    dtEHireDate.Checked = true;
                }
                else
                {
                    dtEHireDate.Value = DateTime.Now.Date;
                    dtEHireDate.Checked = false;
                }
                // txtELengthofTime.Text = ;


            }
        }

        private void btnIncomeDetails_Click(object sender, EventArgs e)
        {
            CaseSnpEntity CaseSnpDetails = CaseSNP;
            if (CaseSnpDetails != null)
            {
                //CaseIncome caseIncomeForm = new CaseIncome(Mode.Equals(Consts.Common.View) ? "V" : "C", BaseForm, Privileges);
                //caseIncomeForm.ShowDialog();
            }
        }

        private void OnMailingAddressClick(object sender, EventArgs e)
        {
            DiffMailForm diffMailForm = new DiffMailForm(BaseForm, CaseMST.ApplAgency, CaseMST.ApplDept, CaseMST.ApplProgram, CaseMST.ApplYr, CaseMST.ApplNo, Privileges, Mode, string.Empty, propCaseDiffMailDetails);
            if (Mode.Equals(Consts.Common.Add))
            {
                diffMailForm.FormClosed += new FormClosedEventHandler(diffMailForm_MailingFormClosed);
            }

            diffMailForm.StartPosition = FormStartPosition.CenterScreen;
            diffMailForm.ShowDialog();
        }

        private void OnGenderSelectionChanged(object sender, EventArgs e)
        {
            if (cmbGender.SelectedIndex != 0)
            {
                if (!((ListItem)cmbGender.SelectedItem).Value.ToString().Equals("F") && !((ListItem)cmbGender.SelectedItem).Value.ToString().Equals("T"))
                {
                    cmbAreyoupregnant.Enabled = false;
                    lblAreYouPregnant.Enabled = false;
                    PregnantReq.Visible = false;
                    SetComboBoxValue(cmbAreyoupregnant, Consts.Common.SelectOne);
                }
                else
                {
                    FldcntlHieEntity FLDCntl = FLDCNTLHieEntity.Find(u => u.FldCode.Equals(Consts.CASE2001.AreYouPregnant));
                    if (FLDCntl != null && FLDCntl.Enab.Equals("Y"))
                    {
                        cmbAreyoupregnant.Enabled = true;
                        lblAreYouPregnant.Enabled = true;
                        if (FLDCntl.Req.Equals("Y")) PregnantReq.Visible = true;
                    }
                }
            }
            else
            {
                cmbAreyoupregnant.Enabled = false;
                lblAreYouPregnant.Enabled = false;
                PregnantReq.Visible = false;
                SetComboBoxValue(cmbAreyoupregnant, Consts.Common.SelectOne);
            }
        }

        private void btnSite_Click(object sender, EventArgs e)
        {
            SiteSearchForm siteSearchForm = new SiteSearchForm(CaseMST.ApplAgency, CaseMST.ApplDept, CaseMST.ApplProgram,CaseMST.ApplYr, Privileges, "ClientIntake", BaseForm);
            siteSearchForm.StartPosition = FormStartPosition.CenterScreen;
            siteSearchForm.FormClosed += new FormClosedEventHandler(OnSiteFormClosed);
            siteSearchForm.ShowDialog();
        }

        private void OnSiteFormClosed(object sender, FormClosedEventArgs e)
        {
            SiteSearchForm form = sender as SiteSearchForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string selectedSite = form.GetSelectedRowDetails();
                txtSite.Text = selectedSite;
            }
        }

        private void btnZipCode_Click(object sender, EventArgs e)
        {
            ZipCodeSearchForm zipCodeSearchForm = new ZipCodeSearchForm(Privileges, txtZipCode.Text);
            zipCodeSearchForm.StartPosition = FormStartPosition.CenterScreen;
            zipCodeSearchForm.FormClosed += new FormClosedEventHandler(OnZipCodeFormClosed);
            zipCodeSearchForm.ShowDialog();
        }

        private void OnZipCodeFormClosed(object sender, FormClosedEventArgs e)
        {
            btnZipCode.Enabled = true;
            btnCitySearch.Enabled = true;
            ZipCodeSearchForm form = sender as ZipCodeSearchForm;
            if (form.DialogResult == DialogResult.OK)
            {
                ZipCodeEntity zipcodedetais = form.GetSelectedZipCodedetails();
                if (zipcodedetais != null)
                {
                    string zipPlus = zipcodedetais.Zcrplus4;
                    txtZipPlus.Text = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                    txtZipCode.Text = "00000".Substring(0, 5 - zipcodedetais.Zcrzip.Length) + zipcodedetais.Zcrzip;
                    txtState.Text = zipcodedetais.Zcrstate;
                    txtCity.Text = zipcodedetais.Zcrcity;
                    SetComboBoxValue(cmbCounty, zipcodedetais.Zcrcountry);
                    SetComboBoxValue(cmbTownship, zipcodedetais.Zcrcitycode);

                }
            }
            // btnCitySearch.Focus();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (tabControl1.SelectedTab == null) return;
            //string code = tabControl1.SelectedTab.Tag as string;
            //if (!code.Equals("SNP"))
            //{
            //    customFieldsIntakeControl.AccessLevel = "H";
            //    customFieldsIntakeControl.filterCustomQuestions("H");
            //    if (code.Equals("Intake")) cmbStaff.Focus();
            //    else cmbVerifiedStaff.Focus();
            //}
            //else
            //{
            //    customFieldsControl.AccessLevel = "*";
            //    customFieldsControl.filterCustomQuestions("*");
            //    mskSSN.Focus();
            //}
        }

        private void btnIncomeVerification_Click(object sender, EventArgs e)
        {
            CaseIncomeVerification caseIncomeverfication = new CaseIncomeVerification(BaseForm, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, Privileges, Mode.Equals(Consts.Common.View) ? "V" : "C");
            caseIncomeverfication.StartPosition = FormStartPosition.CenterScreen;
            caseIncomeverfication.ShowDialog();
        }

        private void cmbRelation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbRelation.Items.Count > 0)
                {
                    string strcmbRelation = ((ListItem)cmbRelation.SelectedItem).Value == null ? string.Empty : ((ListItem)cmbRelation.SelectedItem).Value.ToString();
                    if (!string.IsNullOrEmpty(strcmbRelation))
                    {

                        if (((ListItem)cmbRelation.SelectedItem).Value.ToString() != "0")
                            if (((ListItem)cmbRelation.SelectedItem).ID.ToString() != "Y")
                                MessageBox.Show(Consts.AgyTab.RELATIONSHIPMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (((ListItem)cmbRelation.SelectedItem).Value.ToString() != "0")
                            if (!IsApplicant)
                            {
                                if (((ListItem)cmbRelation.SelectedItem).DefaultValue.ToString() == "Y")
                                {
                                    if (strRelationCode == string.Empty)
                                    {
                                        MessageBox.Show(((ListItem)cmbRelation.SelectedItem).Text.ToString() + "\n Can only be selected by primary Applicant", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    cmbRelation.SelectedIndex = 0;
                                }
                            }
                            else
                            {
                                if (Relationshipdefaultcode != ((ListItem)cmbRelation.SelectedItem).Value.ToString())
                                {
                                    // CommonFunctions.SetComboBoxValue(
                                }
                            }

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cmbEthnicity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbEthnicity.Items.Count > 0)
                {
                    string strcmbEthnicity = ((ListItem)cmbEthnicity.SelectedItem).Value == null ? string.Empty : ((ListItem)cmbEthnicity.SelectedItem).Value.ToString();
                    if (!string.IsNullOrEmpty(strcmbEthnicity))
                    {
                        if (((ListItem)cmbEthnicity.SelectedItem).Value.ToString() != "0")
                            if (((ListItem)cmbEthnicity.SelectedItem).ID.ToString() != "Y")
                                MessageBox.Show(Consts.AgyTab.ETHNICMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cmbRace_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbRace.Items.Count > 0)
                {
                    string strcmbRace = ((ListItem)cmbRace.SelectedItem).Value == null ? string.Empty : ((ListItem)cmbRace.SelectedItem).Value.ToString();
                    if (!string.IsNullOrEmpty(strcmbRace))
                    {
                        if (((ListItem)cmbRace.SelectedItem).Value.ToString() != "0")
                            if (((ListItem)cmbRace.SelectedItem).ID.ToString() != "Y")
                                MessageBox.Show(Consts.AgyTab.RACEMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cmbEducation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbEducation.Items.Count > 0)
                {
                    string strcmbEducation = ((ListItem)cmbEducation.SelectedItem).Value == null ? string.Empty : ((ListItem)cmbEducation.SelectedItem).Value.ToString();
                    if (string.IsNullOrEmpty(strcmbEducation))
                    {
                        if (((ListItem)cmbEducation.SelectedItem).Value.ToString() != "0")
                            if (((ListItem)cmbEducation.SelectedItem).ID.ToString() != "Y")
                                MessageBox.Show(Consts.AgyTab.EDUCATIONCODESMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cmbSchool_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (((ListItem)cmbSchool.SelectedItem).Value.ToString() != "0")
                    if (((ListItem)cmbSchool.SelectedItem).ID.ToString() != "Y")
                        MessageBox.Show(Consts.AgyTab.SCHOOLDISTRICTSMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {

            }
        }

        private void cmbSSNReason_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbAreyoupregnant_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (((ListItem)cmbAreyoupregnant.SelectedItem).Value.ToString() != "0")
                    if (((ListItem)cmbAreyoupregnant.SelectedItem).ID.ToString() != "Y")
                        MessageBox.Show(Consts.AgyTab.PREGNANTMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {

            }
        }

        private void cmbMaritalStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbMaritalStatus.Items.Count > 0)
                {
                    string strCmbMaritalstatus = ((ListItem)cmbMaritalStatus.SelectedItem).Value == null ? string.Empty : ((ListItem)cmbMaritalStatus.SelectedItem).Value.ToString();
                    if (!string.IsNullOrEmpty(strCmbMaritalstatus))
                    {
                        if (((ListItem)cmbMaritalStatus.SelectedItem).Value.ToString() != "0")
                            if (((ListItem)cmbMaritalStatus.SelectedItem).ID.ToString() != "Y")
                                MessageBox.Show(Consts.AgyTab.MARITALSTATUSMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cmbHealthInsurance_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbHealthInsurance.Items.Count > 0)
                {
                    string strcmbHealthInsurance = ((ListItem)cmbHealthInsurance.SelectedItem).Value == null ? string.Empty : ((ListItem)cmbHealthInsurance.SelectedItem).Value.ToString();
                    if (!string.IsNullOrEmpty(strcmbHealthInsurance))
                    {
                        if (((ListItem)cmbHealthInsurance.SelectedItem).Value.ToString() != "0")
                            if (((ListItem)cmbHealthInsurance.SelectedItem).ID.ToString() != "Y")
                                MessageBox.Show(Consts.AgyTab.HEALTHINSURANCEMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cmbVeteranCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbVeteranCode.Items.Count > 0)
                {
                    string strcmbveterancode = ((ListItem)cmbVeteranCode.SelectedItem).Value == null ? string.Empty : ((ListItem)cmbVeteranCode.SelectedItem).Value.ToString();
                    if (!string.IsNullOrEmpty(strcmbveterancode))
                    {
                        if (((ListItem)cmbVeteranCode.SelectedItem).Value.ToString() != "0")
                            if (((ListItem)cmbVeteranCode.SelectedItem).ID.ToString() != "Y")
                                MessageBox.Show(Consts.AgyTab.VETERANMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void cmbFoodStamps_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbFoodStamps.Items.Count > 0)
                {
                    string strcmbFoodStamps = ((ListItem)cmbFoodStamps.SelectedItem).Value == null ? string.Empty : ((ListItem)cmbFoodStamps.SelectedItem).Value.ToString();
                    if (!string.IsNullOrEmpty(strcmbFoodStamps))
                    {
                        if (((ListItem)cmbFoodStamps.SelectedItem).Value.ToString() != "0")
                            if (((ListItem)cmbFoodStamps.SelectedItem).ID.ToString() != "Y")
                                MessageBox.Show(Consts.AgyTab.FOODSTAMPSMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cmbDisabled_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDisabled.Items.Count > 0)
                {
                    string strcmbDisabled = ((ListItem)cmbDisabled.SelectedItem).Value == null ? string.Empty : ((ListItem)cmbDisabled.SelectedItem).Value.ToString();
                    if (!string.IsNullOrEmpty(strcmbDisabled))
                    {
                        if (((ListItem)cmbDisabled.SelectedItem).Value.ToString() != "0")
                            if (((ListItem)cmbDisabled.SelectedItem).ID.ToString() != "Y")
                                MessageBox.Show(Consts.AgyTab.DISABLEDMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cmbFarmer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (((ListItem)cmbFarmer.SelectedItem).Value.ToString() != "0")
                    if (((ListItem)cmbFarmer.SelectedItem).ID.ToString() != "Y")
                        MessageBox.Show(Consts.AgyTab.FARMERMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {

            }
        }

        private void cmbWIC_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (((ListItem)cmbWIC.SelectedItem).Value.ToString() != "0")
                    if (((ListItem)cmbWIC.SelectedItem).ID.ToString() != "Y")
                        MessageBox.Show(Consts.AgyTab.WICMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {

            }
        }

        private void cmbReliableTrans_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (((ListItem)cmbReliableTrans.SelectedItem).Value.ToString() != "0")
                    if (((ListItem)cmbReliableTrans.SelectedItem).ID.ToString() != "Y")
                        MessageBox.Show(Consts.AgyTab.RELIABLETRANSPORTATIONMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {

            }
        }

        private void cmbDriving_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (((ListItem)cmbDriving.SelectedItem).Value.ToString() != "0")
                    if (((ListItem)cmbDriving.SelectedItem).ID.ToString() != "Y")
                        MessageBox.Show(Consts.AgyTab.DRIVERLICENSEMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {

            }
        }

        private void cmbCaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCaseType.Items.Count > 0)
                {
                    string strcmbCaseType = ((ListItem)cmbCaseType.SelectedItem).Value == null ? string.Empty : ((ListItem)cmbCaseType.SelectedItem).Value.ToString();
                    if (!string.IsNullOrEmpty(strcmbCaseType))
                    {
                        if (((ListItem)cmbCaseType.SelectedItem).Value.ToString() != "0")
                            if (((ListItem)cmbCaseType.SelectedItem).ID.ToString() != "Y")
                                MessageBox.Show(Consts.AgyTab.CASETYPESMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cmbHousingSituation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                string strV = ((ListItem)cmbHousingSituation.SelectedItem).Value == null ? string.Empty : ((ListItem)cmbHousingSituation.SelectedItem).Value.ToString();
                if (strV != "0")
                {
                    //if (Mode.Equals("Edit") || Mode.Equals(Consts.Common.View))
                    //{

                    if (ProgramDefinition.State.ToUpper() == "TX")
                    {
                        if (((ListItem)cmbHousingSituation.SelectedItem).ID.ToString().Trim() == "E")
                        {
                            chkSubsidized.Enabled = lblSHType.Enabled = lblSubsidizedH.Enabled = true;
                            btnLandlordInfo.Visible = true;
                            if (chkSubsidized.Checked == true)
                            {
                                lblSubsidizedReq.Visible = true;
                                cmbSubsidized.Enabled = true;
                            }
                            else
                            {
                                lblSubsidizedReq.Visible = false;
                                cmbSubsidized.Enabled = false;
                                cmbSubsidized.SelectedIndex = 0;
                            }


                        }
                        else
                        {
                            chkSubsidized.Enabled = lblSHType.Enabled = lblSubsidizedH.Enabled = false;
                            lblSubsidizedReq.Visible = false;
                            cmbSubsidized.Enabled = false;
                            cmbSubsidized.SelectedIndex = 0;
                            chkSubsidized.Checked = false;
                        }
                    }
                    else
                    {
                        if (strV == "B")
                        {
                            chkSubsidized.Enabled = lblSHType.Enabled = lblSubsidizedH.Enabled = true;
                            btnLandlordInfo.Visible = true;
                            if (chkSubsidized.Checked == true)
                            {
                                lblSubsidizedReq.Visible = true;
                                cmbSubsidized.Enabled = true;
                            }
                            else
                            {
                                lblSubsidizedReq.Visible = false;
                                cmbSubsidized.Enabled = false;
                                cmbSubsidized.SelectedIndex = 0;
                            }


                        }
                        else
                        {
                            chkSubsidized.Enabled = lblSHType.Enabled = lblSubsidizedH.Enabled = false;
                            lblSubsidizedReq.Visible = false;
                            cmbSubsidized.Enabled = false;
                            cmbSubsidized.SelectedIndex = 0;
                            chkSubsidized.Checked = false;
                        }

                    }

                    switch (strV)
                    {
                        case "B":
                            btnLandlordInfo.Visible = true;
                            break;
                        case "A":
                            btnLandlordInfo.Visible = false;
                            if (Mode.Equals(Consts.Common.Add))
                                propCaseDiffLLDetails = null;
                            break;
                        case "1":
                            btnLandlordInfo.Visible = false;
                            if (Mode.Equals(Consts.Common.Add))
                                propCaseDiffLLDetails = null;
                            break;
                        case "2":
                            btnLandlordInfo.Visible = true;
                            break;
                        case "3":
                            btnLandlordInfo.Visible = true;
                            break;
                        case "4":
                            btnLandlordInfo.Visible = true;
                            break;
                        case "5":
                            btnLandlordInfo.Visible = true;
                            break;
                        default:
                            btnLandlordInfo.Visible = false;
                            if (Mode.Equals(Consts.Common.Add))
                                propCaseDiffLLDetails = null;
                            break;
                    }
                    //}
                    if (((ListItem)cmbHousingSituation.SelectedItem).ValueDisplayCode.ToString() != "Y")
                    {
                        if (boolHouseingMsg)
                            MessageBox.Show(Consts.AgyTab.HOUSINGSITUVATION, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
                else
                {
                    chkSubsidized.Enabled = lblSHType.Enabled = lblSubsidizedH.Enabled = false;
                    lblSubsidizedReq.Visible = false;
                    cmbSubsidized.Enabled = false;
                    cmbSubsidized.SelectedIndex = 0;
                    chkSubsidized.Checked = false;
                    btnLandlordInfo.Visible = false;
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void cmbPrimaryLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbPrimaryLang.Items.Count > 0)
                {
                    string strcmbPrimaryLang = ((ListItem)cmbPrimaryLang.SelectedItem).Value == null ? string.Empty : ((ListItem)cmbPrimaryLang.SelectedItem).Value.ToString();
                    if (!string.IsNullOrEmpty(strcmbPrimaryLang))
                    {
                        if (((ListItem)cmbPrimaryLang.SelectedItem).Value.ToString() != "0")
                            if (((ListItem)cmbPrimaryLang.SelectedItem).ID.ToString() != "Y")
                                MessageBox.Show(Consts.AgyTab.PRIMARYLanguage, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {


            }

        }

        private void cmbSecondLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (((ListItem)cmbSecondLang.SelectedItem).Value.ToString() != "0")
                    if (((ListItem)cmbSecondLang.SelectedItem).ID.ToString() != "Y")
                        MessageBox.Show(Consts.AgyTab.SECONDLANGUAGE, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {


            }

        }

        private void cmbResident_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbResident.Items.Count > 0)
                {
                    string strcmbresident = ((ListItem)cmbResident.SelectedItem).Value == null ? string.Empty : ((ListItem)cmbResident.SelectedItem).Value.ToString();
                    if (!string.IsNullOrEmpty(strcmbresident))
                    {
                        if (((ListItem)cmbResident.SelectedItem).Value.ToString() != "0")
                        {
                            if (((ListItem)cmbResident.SelectedItem).ID.ToString() != "Y")
                                MessageBox.Show(Consts.AgyTab.RESIDENTCODESMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            cmbresidentchangefunction();
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }

        private void cmbresidentchangefunction()
        {
            try
            {
                if (((ListItem)cmbResident.SelectedItem).Value.ToString() != "0")
                {
                    if (((ListItem)cmbResident.SelectedItem).Value.ToString() == "A")
                    {
                        AliasReq.Visible = false;
                        lblAlienRegNo.Enabled = false;
                        lblLegalToWork.Enabled = false;
                        lblExpirationDate.Enabled = false;
                        txtAlienRegNo.Enabled = false;
                        txtAlienRegNo.Text = string.Empty;
                        LegalToWork.Visible = false;
                        cmbLegaltowork.SelectedIndex = 0;
                        cmbLegaltowork.Enabled = false;
                        ExpirationDateReq.Visible = false;
                        dtExpirationdate.Checked = false;
                        dtExpirationdate.Enabled = false;
                    }
                    if (((ListItem)cmbResident.SelectedItem).Value.ToString() == "B")
                    {
                        // AliasReq.Visible = false;
                        txtAlienRegNo.Enabled = true;
                        txtAlienRegNo.Text = string.Empty;
                        lblAlienRegNo.Enabled = true;
                        lblLegalToWork.Enabled = true;
                        lblExpirationDate.Enabled = false;
                        // LegalToWork.Visible = false;
                        cmbLegaltowork.SelectedIndex = 0;
                        cmbLegaltowork.Enabled = true;
                        ExpirationDateReq.Visible = false;
                        dtExpirationdate.Checked = false;
                        dtExpirationdate.Enabled = false;
                    }
                    if (((ListItem)cmbResident.SelectedItem).Value.ToString() == "C")
                    {
                        // AliasReq.Visible = false;
                        txtAlienRegNo.Enabled = false;
                        txtAlienRegNo.Text = string.Empty;
                        lblAlienRegNo.Enabled = false;
                        lblLegalToWork.Enabled = true;
                        lblExpirationDate.Enabled = false;
                        // LegalToWork.Visible = false;
                        cmbLegaltowork.SelectedIndex = 0;
                        cmbLegaltowork.Enabled = true;
                        ExpirationDateReq.Visible = false;
                        dtExpirationdate.Checked = false;
                        dtExpirationdate.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void AddEventHandler()
        {
            //cmbRelation.SelectedIndexChanged += cmbRelation_SelectedIndexChanged;
            //cmbEthnicity.SelectedIndexChanged += cmbEthnicity_SelectedIndexChanged;
            //cmbRace.SelectedIndexChanged += cmbRace_SelectedIndexChanged;           
            //cmbEducation.SelectedIndexChanged += cmbEducation_SelectedIndexChanged;
            //cmbSchool.SelectedIndexChanged += cmbEducation_SelectedIndexChanged;
            //cmbAreyoupregnant.SelectedIndexChanged += cmbAreyoupregnant_SelectedIndexChanged;
            //cmbMaritalStatus.SelectedIndexChanged += cmbMaritalStatus_SelectedIndexChanged;
            //cmbHealthInsurance.SelectedIndexChanged += cmbHealthInsurance_SelectedIndexChanged;
            //cmbVeteranCode.SelectedIndexChanged += cmbVeteranCode_SelectedIndexChanged;
            //cmbFoodStamps.SelectedIndexChanged += cmbFoodStamps_SelectedIndexChanged;
            //cmbWIC.SelectedIndexChanged += cmbWIC_SelectedIndexChanged;
            //cmbFarmer.SelectedIndexChanged += cmbFarmer_SelectedIndexChanged;
            //cmbDisabled.SelectedIndexChanged += cmbDisabled_SelectedIndexChanged;
            //cmbDriving.SelectedIndexChanged += cmbDriving_SelectedIndexChanged;
            //cmbReliableTrans.SelectedIndexChanged += cmbReliableTrans_SelectedIndexChanged;
            //cmbResident.SelectedIndexChanged += cmbResident_SelectedIndexChanged;

            cmbRelation.SelectedIndexChanged += new EventHandler(cmbRelation_SelectedIndexChanged);
            cmbEthnicity.SelectedIndexChanged += new EventHandler(cmbEthnicity_SelectedIndexChanged);
            cmbRace.SelectedIndexChanged += new EventHandler(cmbRace_SelectedIndexChanged);
            cmbEducation.SelectedIndexChanged += new EventHandler(cmbEducation_SelectedIndexChanged);
            cmbSchool.SelectedIndexChanged += new EventHandler(cmbEducation_SelectedIndexChanged);
            cmbAreyoupregnant.SelectedIndexChanged += new EventHandler(cmbAreyoupregnant_SelectedIndexChanged);
            cmbMaritalStatus.SelectedIndexChanged += new EventHandler(cmbMaritalStatus_SelectedIndexChanged);
            cmbHealthInsurance.SelectedIndexChanged += new EventHandler(cmbHealthInsurance_SelectedIndexChanged);
            cmbVeteranCode.SelectedIndexChanged += new EventHandler(cmbVeteranCode_SelectedIndexChanged);
            cmbFoodStamps.SelectedIndexChanged += new EventHandler(cmbFoodStamps_SelectedIndexChanged);
            cmbWIC.SelectedIndexChanged += new EventHandler(cmbWIC_SelectedIndexChanged);
            cmbFarmer.SelectedIndexChanged += new EventHandler(cmbFarmer_SelectedIndexChanged);
            cmbDisabled.SelectedIndexChanged += new EventHandler(cmbDisabled_SelectedIndexChanged);
            cmbDriving.SelectedIndexChanged += new EventHandler(cmbDriving_SelectedIndexChanged);
            cmbReliableTrans.SelectedIndexChanged += new EventHandler(cmbReliableTrans_SelectedIndexChanged);
            cmbResident.SelectedIndexChanged += new EventHandler(cmbResident_SelectedIndexChanged);

        }

        private void RemoveEventHandler()
        {
            //cmbRelation.SelectedIndexChanged -= new EventHandler(cmbRelation_SelectedIndexChanged);
            //cmbEthnicity.SelectedIndexChanged -= new EventHandler(cmbEthnicity_SelectedIndexChanged);
            //cmbRace.SelectedIndexChanged -= new EventHandler(cmbRace_SelectedIndexChanged);
            //cmbEducation.SelectedIndexChanged -= new EventHandler(cmbEducation_SelectedIndexChanged);
            //cmbSchool.SelectedIndexChanged -= new EventHandler(cmbEducation_SelectedIndexChanged);
            //cmbAreyoupregnant.SelectedIndexChanged -= new EventHandler(cmbAreyoupregnant_SelectedIndexChanged);
            //cmbMaritalStatus.SelectedIndexChanged -= new EventHandler(cmbMaritalStatus_SelectedIndexChanged);
            //cmbHealthInsurance.SelectedIndexChanged -= new EventHandler(cmbHealthInsurance_SelectedIndexChanged);
            //cmbVeteranCode.SelectedIndexChanged -= new EventHandler(cmbVeteranCode_SelectedIndexChanged);
            //cmbFoodStamps.SelectedIndexChanged -= new EventHandler(cmbFoodStamps_SelectedIndexChanged);
            //cmbWIC.SelectedIndexChanged -= new EventHandler(cmbWIC_SelectedIndexChanged);
            //cmbFarmer.SelectedIndexChanged -= new EventHandler(cmbFarmer_SelectedIndexChanged);
            //cmbDisabled.SelectedIndexChanged -= new EventHandler(cmbDisabled_SelectedIndexChanged);
            //cmbDriving.SelectedIndexChanged -= new EventHandler(cmbDriving_SelectedIndexChanged);
            //cmbReliableTrans.SelectedIndexChanged -= new EventHandler(cmbReliableTrans_SelectedIndexChanged);
            //cmbResident.SelectedIndexChanged -= new EventHandler(cmbResident_SelectedIndexChanged);

            cmbRelation.SelectedIndexChanged -= new EventHandler(cmbRelation_SelectedIndexChanged);
            cmbEthnicity.SelectedIndexChanged -= new EventHandler(cmbEthnicity_SelectedIndexChanged);
            cmbRace.SelectedIndexChanged -= new EventHandler(cmbRace_SelectedIndexChanged);
            cmbEducation.SelectedIndexChanged -= new EventHandler(cmbEducation_SelectedIndexChanged);
            cmbSchool.SelectedIndexChanged -= new EventHandler(cmbEducation_SelectedIndexChanged);
            cmbAreyoupregnant.SelectedIndexChanged -= new EventHandler(cmbAreyoupregnant_SelectedIndexChanged);
            cmbMaritalStatus.SelectedIndexChanged -= new EventHandler(cmbMaritalStatus_SelectedIndexChanged);
            cmbHealthInsurance.SelectedIndexChanged -= new EventHandler(cmbHealthInsurance_SelectedIndexChanged);
            cmbVeteranCode.SelectedIndexChanged -= new EventHandler(cmbVeteranCode_SelectedIndexChanged);
            cmbFoodStamps.SelectedIndexChanged -= new EventHandler(cmbFoodStamps_SelectedIndexChanged);
            cmbWIC.SelectedIndexChanged -= new EventHandler(cmbWIC_SelectedIndexChanged);
            cmbFarmer.SelectedIndexChanged -= new EventHandler(cmbFarmer_SelectedIndexChanged);
            cmbDisabled.SelectedIndexChanged -= new EventHandler(cmbDisabled_SelectedIndexChanged);
            cmbDriving.SelectedIndexChanged -= new EventHandler(cmbDriving_SelectedIndexChanged);
            cmbReliableTrans.SelectedIndexChanged -= new EventHandler(cmbReliableTrans_SelectedIndexChanged);
            cmbResident.SelectedIndexChanged -= new EventHandler(cmbResident_SelectedIndexChanged);

        }

        public void RemoveMstEventHandler()
        {
            cmbStaff.SelectedIndexChanged -= new EventHandler(cmbStaff_SelectedIndexChanged);
            cmbVerifiedStaff.SelectedIndexChanged -= new EventHandler(cmbVerifiedStaff_SelectedIndexChanged);
            cmbHousingSituation.SelectedIndexChanged -= new EventHandler(cmbHousingSituation_SelectedIndexChanged);
            cmbPrimarySourceoHeat.SelectedIndexChanged -= new EventHandler(cmbPrimarySourceoHeat_SelectedIndexChanged);
            cmbPMOPfHeat.SelectedIndexChanged -= new EventHandler(cmbPMOPfHeat_SelectedIndexChanged);
            cmbDwelling.SelectedIndexChanged -= new EventHandler(cmbDwelling_SelectedIndexChanged);
            cmbPrimaryLang.SelectedIndexChanged -= new EventHandler(cmbPrimaryLang_SelectedIndexChanged);
            cmbSecondLang.SelectedIndexChanged -= new EventHandler(cmbSecondLang_SelectedIndexChanged);
            cmbSubsidized.SelectedIndexChanged -= new EventHandler(cmbSubsidized_SelectedIndexChanged);

            cmbCaseType.SelectedIndexChanged -= new EventHandler(cmbCaseType_SelectedIndexChanged);
            cmbOutService.SelectedIndexChanged -= new EventHandler(cmbOutService_SelectedIndexChanged);
            cmbQuestionType.SelectedIndexChanged -= new EventHandler(cmbQuestionType_SelectedIndexChanged);
            cmbServicesInquired.SelectedIndexChanged -= new EventHandler(cmbServicesInquired_SelectedIndexChanged);
        }

        public void AddMstEventHandler()
        {
            cmbStaff.SelectedIndexChanged += new EventHandler(cmbStaff_SelectedIndexChanged);
            cmbVerifiedStaff.SelectedIndexChanged += new EventHandler(cmbVerifiedStaff_SelectedIndexChanged);
            cmbHousingSituation.SelectedIndexChanged += new EventHandler(cmbHousingSituation_SelectedIndexChanged);
            cmbPrimarySourceoHeat.SelectedIndexChanged += new EventHandler(cmbPrimarySourceoHeat_SelectedIndexChanged);
            cmbPMOPfHeat.SelectedIndexChanged += new EventHandler(cmbPMOPfHeat_SelectedIndexChanged);
            cmbDwelling.SelectedIndexChanged += new EventHandler(cmbDwelling_SelectedIndexChanged);
            cmbPrimaryLang.SelectedIndexChanged += new EventHandler(cmbPrimaryLang_SelectedIndexChanged);
            cmbSecondLang.SelectedIndexChanged += new EventHandler(cmbSecondLang_SelectedIndexChanged);
            cmbSubsidized.SelectedIndexChanged += new EventHandler(cmbSubsidized_SelectedIndexChanged);

            cmbCaseType.SelectedIndexChanged += new EventHandler(cmbCaseType_SelectedIndexChanged);
            cmbOutService.SelectedIndexChanged += new EventHandler(cmbOutService_SelectedIndexChanged);
            cmbQuestionType.SelectedIndexChanged += new EventHandler(cmbQuestionType_SelectedIndexChanged);
            cmbServicesInquired.SelectedIndexChanged += new EventHandler(cmbServicesInquired_SelectedIndexChanged);
        }

        private void ckActive_Click(object sender, EventArgs e)
        {
            if (ckActive.Checked)
            {
                if (FLDCNTLHieEntity.Count > 0)
                {
                    ckExcludeMember.Enabled = false;
                    lblSnpExcludeMemberReq.Visible = false;

                    chkExcludeHHMem.Visible = false;
                    lblSnpExcludeMember2Req.Visible = false;
                    foreach (FldcntlHieEntity entity in FLDCNTLHieEntity)
                    {
                        bool required = entity.Req.Equals("Y") ? true : false;
                        bool enabled = entity.Enab.Equals("Y") ? true : false;

                        switch (entity.FldCode)
                        {
                            case Consts.CASE2001.ExcludeMember:
                                if (propAgencyControlDetails.AgyShortName.ToUpper() == "CABA")
                                {
                                    if (enabled) { ckExcludeMember.Visible = ckExcludeMember.Enabled = true; if (required) lblSnpExcludeMemberReq.Visible = true; } else { ckExcludeMember.Visible = ckExcludeMember.Enabled = false; lblSnpExcludeMemberReq.Visible = false; }
                                }
                                else
                                {
                                    if (enabled) { ckExcludeMember.Enabled = true; if (required) lblSnpExcludeMemberReq.Visible = true; } else { ckExcludeMember.Enabled = false; lblSnpExcludeMemberReq.Visible = false; }
                                }
                                break;
                            case Consts.CASE2001.ExcludeMember2:
                                if (enabled) { chkExcludeHHMem.Visible = chkExcludeHHMem.Enabled = true; if (required) lblSnpExcludeMember2Req.Visible = true; } else { chkExcludeHHMem.Visible = false; lblSnpExcludeMember2Req.Visible = false; }
                                break;

                        }
                    }
                }
                else
                {
                    ckExcludeMember.Enabled = false;
                    chkExcludeHHMem.Visible = false;
                }

            }
            else
            {
                ckExcludeMember.Checked = false;
                ckExcludeMember.Enabled = false;
                chkExcludeHHMem.Checked = false;
                chkExcludeHHMem.Enabled = false;
                lblSnpExcludeMember2Req.Visible = false;
            }
        }

        private void MessageBoxHandler(DialogResult dialogResult)
        {

            if (dialogResult == DialogResult.Yes)
            {
                btnSave_Click(btnSave, new EventArgs());
            }
            else
            {
                IsExclude = false;
                btnSave.Enabled = true;
            }

        }

        private void ClientSNPForm_Load(object sender, EventArgs e)
        {
            try
            {
                boolHouseingMsg = true;
                if (!Mode.Equals(Consts.Common.View))
                {
                    if (!CntlEntity.Exists(u => u.Enab.Equals("Y")))
                    {
                        CommonFunctions.MessageBoxDisplay("Field controls not defined for this program");
                        btnSave.Enabled = false;
                    }
                    ckActive_Click(sender, e);
                }
                if (Mode.Equals(Consts.Common.Edit))
                {
                    if (mskSSN.Text == "000000000")
                    {
                        if (ProgramDefinition.SSNReasonFlag == "Y")
                        {
                            lblSSNReasonReq.Visible = true;
                            lblSSNReason.Visible = true;
                            cmbSSNReason.Visible = true;
                        }
                        else
                        {
                            lblSSNReasonReq.Visible = false;
                            lblSSNReason.Visible = false;
                            cmbSSNReason.Visible = false;
                        }
                    }
                    if (!CntlEntity.Exists(u => u.Enab.Equals("Y")))
                    {
                        mskSSN.Enabled = false;
                        lblSSNReason.Enabled = false;
                        cmbSSNReason.Enabled = false;
                    }
                    else
                    {
                        mskSSN.Enabled = true;
                        lblSSNReason.Enabled = true;
                        cmbSSNReason.Enabled = true;

                    }
                    txtFirstName.Focus();
                    if (IsApplicant)
                    {
                        if (((ListItem)cmbRelation.SelectedItem).Value != null)
                        {
                            string strRelation = ((ListItem)cmbRelation.SelectedItem).Value == null ? string.Empty : ((ListItem)cmbRelation.SelectedItem).Value.ToString();
                            if (strRelation == Relationshipdefaultcode)
                                cmbRelation.Enabled = false;
                        }
                        if (Relationshipdefaultcode == string.Empty)
                        {
                            CommonFunctions.MessageBoxDisplay("No Default Relation Set in Agy table");
                        }
                    }
                }
                if (Mode.Equals(Consts.Common.View))
                {
                    if (mskSSN.Text == "000000000")
                    {
                        if (ProgramDefinition.SSNReasonFlag == "Y")
                        {
                            lblSSNReasonReq.Visible = true;
                            lblSSNReason.Visible = true;
                            cmbSSNReason.Visible = true;
                            cmbSSNReason.Enabled = false;
                            lblSSNReason.Enabled = false;
                        }
                        else
                        {
                            lblSSNReasonReq.Visible = false;
                            lblSSNReason.Visible = false;
                            cmbSSNReason.Visible = false;
                        }
                    }
                }
                if (Mode.Equals(Consts.Common.Add))
                {
                    mskSSN.Focus();

                    if (ProgramDefinition.DepIntakeProg.Equals("Y") && ProgramDefinition.DepSsnAutoAssign == "1")
                    {
                        mskSSN.Mask = "";
                        mskSSN.Text = "NewSSNNum";
                    }
                    if (IsApplicant)
                    {
                        if (((ListItem)cmbRelation.SelectedItem).Value.ToString() == Relationshipdefaultcode)
                            cmbRelation.Enabled = false;
                        if (Relationshipdefaultcode == string.Empty)
                        {
                            CommonFunctions.MessageBoxDisplay("No Default Relation Set in Agy table");
                        }
                    }
                }
                Enableoutservicecmb();
                // 03/05/2015 AREAIV
                //if (ProgramDefinition.CaseTypeEdit.Equals("Y") && Mode.Equals(Consts.Common.Add))
                //{
                //    cmbCaseType.Enabled = false;
                //    lblCaseTypeReq.Visible = false;
                //}
            }
            catch (Exception ex)
            {


            }
        }

        private void mskSSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Mode.Equals(Consts.Common.Add))
            {
                if (e.KeyChar == (char)Keys.PageDown)
                {
                    mskSSN.Text = string.Empty;
                    mskSSN.Mask = "";
                    mskSSN.Text = "NewSSNNum";
                    mskSSN.Enabled = false;
                    this.InvokeFocusCommand(txtBIC);
                    if (ProgramDefinition.SSNReasonFlag == "Y")
                    {
                        lblSSNReasonReq.Visible = true;
                        lblSSNReason.Visible = true;
                        cmbSSNReason.Visible = true;
                        lblSSNReason.Enabled = true;
                        cmbSSNReason.Enabled = true;
                    }
                    else
                    {
                        lblSSNReasonReq.Visible = false;
                        lblSSNReason.Visible = false;
                        cmbSSNReason.Visible = false;
                    }
                }
            }

            //string strssn = mskSSN.Text.Replace(" ",string.Empty);
            //if (strssn.Length >= 8 && strssn.Length > 0)
            //{

            if (e.KeyChar == (char)Keys.Tab || (e.KeyChar == '\t' || e.KeyChar == (char)13))
            {
                strTabPress = "Tabchanged";

                // CheckSSNDetails();
                // mskSSN_Leave(sender, e);
            }

            // }
        }

        string strTabPress = string.Empty;

        private void dtBirth_ValueChanged(object sender, EventArgs e)
        {
            bool boolchkstatus = true;
            if (dtBirth.Checked == true)
            {

                if (boolchkstatus)
                {
                    string strIntakeDate = CaseMST.IntakeDate;
                    if (strIntakeDate == string.Empty || strIntakeDate == null)
                        strIntakeDate = DateTime.Now.Date.ToShortDateString();

                    txtYear.Text = CommonFunctions.CalculationYear(Convert.ToDateTime(dtBirth.Text.Trim()), Convert.ToDateTime(strIntakeDate));
                    txtCurrentAge.Text = CommonFunctions.CalculationYear(Convert.ToDateTime(dtBirth.Text.Trim()), Convert.ToDateTime(DateTime.Now.Date.ToShortDateString()));
                }
            }
            else
            {
                txtYear.Text = string.Empty;
                txtCurrentAge.Text = string.Empty;
            }
        }

        private void cmbLegaltowork_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (((ListItem)cmbLegaltowork.SelectedItem).Value.ToString() == "Y")
                {
                    dtExpirationdate.Enabled = true;
                    lblExpirationDate.Enabled = true;
                }
                else
                {
                    dtExpirationdate.Enabled = false;
                    dtExpirationdate.Checked = false;
                    lblExpirationDate.Enabled = false;
                }
            }
            catch (Exception ex)
            {


            }
        }

        public void ShowHierachyandApplNo(string strApplicationNo)
        {

            CaseMstEntity caseMstEntity = null;
            List<CaseSnpEntity> caseSnpEntity = null;
            string strApplNo = strApplicationNo;
            caseMstEntity = _model.CaseMstData.GetCaseMST(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, strApplNo);
            string strAgencyName = BaseForm.BaseAgency + " - " + _model.lookupDataAccess.GetHierachyDescription("1", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);    // Yeswanth
            string strDeptName = BaseForm.BaseDept + " - " + _model.lookupDataAccess.GetHierachyDescription("2", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            string strProgName = BaseForm.BaseProg + " - " + _model.lookupDataAccess.GetHierachyDescription("3", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            string AgencyName = strAgencyName;
            string DeptName = strDeptName;
            string ProgramName = strProgName;
            string Programyear = string.Empty;
            if (caseMstEntity != null)
            {
                caseSnpEntity = _model.CaseMstData.GetCaseSnpadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, caseMstEntity.ApplYr, strApplNo);
                BaseForm.BaseApplicationNo = strApplNo;
                Programyear = caseMstEntity.ApplYr;
                if (Programyear == string.Empty)
                    Programyear = "    ";

            }
            else   // Yeswanth
                caseSnpEntity = null;
            BaseForm.BaseTopApplSelect = "Y";
            BaseForm.GetApplicantDetails(caseMstEntity, caseSnpEntity, strAgencyName, strDeptName, strProgName, Programyear.ToString(), string.Empty, "Display");
        }

        private void txtZipCode_Leave(object sender, EventArgs e)
        {
            if (propBaseAgencyControlDetails != null)
            {
                if (propBaseAgencyControlDetails.EditZip.ToString() == "1")
                {
                    btnZipCode.Enabled = false;
                    if (txtZipCode.Text.Length == 5)
                    {
                        List<ZipCodeEntity> zipcodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(txtZipCode.Text, string.Empty, string.Empty, string.Empty);
                        if (zipcodeEntity.Count > 0)
                            zipcodeEntity = zipcodeEntity.FindAll(u => u.InActive.Equals("N") || u.InActive.Trim().Equals(""));

                        if (zipcodeEntity.Count > 0)
                        {
                            if (zipcodeEntity.Count == 1)
                            {
                                btnZipCode.Enabled = true;
                            }
                            foreach (ZipCodeEntity zipcodedetais in zipcodeEntity)
                            {
                                if (zipcodedetais != null)
                                {
                                    string zipPlus = zipcodedetais.Zcrplus4;
                                    txtZipPlus.Text = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                                    txtZipCode.Text = "00000".Substring(0, 5 - zipcodedetais.Zcrzip.Length) + zipcodedetais.Zcrzip;
                                    txtState.Text = zipcodedetais.Zcrstate;
                                    txtCity.Text = zipcodedetais.Zcrcity;
                                    SetComboBoxValue(cmbCounty, zipcodedetais.Zcrcountry);
                                    SetComboBoxValue(cmbTownship, zipcodedetais.Zcrcitycode);
                                }

                            }
                        }
                        else
                        {
                            ZipCodeSearchForm zipCodeSearchForm = new ZipCodeSearchForm(Privileges, txtZipCode.Text);
                            zipCodeSearchForm.StartPosition = FormStartPosition.CenterScreen;
                            zipCodeSearchForm.FormClosed += new FormClosedEventHandler(OnZipCodeFormClosed);
                            zipCodeSearchForm.ShowDialog();
                        }
                    }
                    else
                    {
                        if (txtZipCode.Text.Trim().Length > 0)
                        {
                            ZipCodeSearchForm zipCodeSearchForm = new ZipCodeSearchForm(Privileges, txtZipCode.Text);
                            zipCodeSearchForm.StartPosition = FormStartPosition.CenterScreen;
                            zipCodeSearchForm.FormClosed += new FormClosedEventHandler(OnZipCodeFormClosed);
                            zipCodeSearchForm.ShowDialog();
                        }
                        else
                        {
                            CommonFunctions.MessageBoxDisplay("Zip Code Shouldn't be Blank");
                        }
                    }
                }
            }
        }

        private void cmbEpresenteEmploy_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (((ListItem)cmbEpresenteEmploy.SelectedItem).Value.ToString() == "N")
                {
                    dtElastDateWorked.Enabled = true;
                    lblLastDateWorked.Enabled = true;
                    dtElastDateWorked.Checked = false;
                }
                else
                {
                    dtElastDateWorked.Checked = false;
                    dtElastDateWorked.Enabled = false;
                    lblLastDateWorked.Enabled = false;
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void cmbEAnywork_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (((ListItem)cmbEAnywork.SelectedItem).Value.ToString() == "Y")
                {
                    txtEifyesexplain.Text = string.Empty;
                    txtEifyesexplain.Enabled = true;
                    lblEIfyesexplain.Enabled = true;
                }
                else
                {
                    txtEifyesexplain.Text = string.Empty;
                    txtEifyesexplain.Enabled = false;
                    lblEIfyesexplain.Enabled = false;
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void cmbEJobTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (((ListItem)cmbEJobTitle.SelectedItem).Value.ToString() != "0")
                    if (((ListItem)cmbEJobTitle.SelectedItem).ID.ToString() != "Y")
                        MessageBox.Show(Consts.AgyTab.JOBTITLEMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {


            }

        }

        private void cmbEJobCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (((ListItem)cmbEJobCategory.SelectedItem).Value.ToString() != "0")
                    if (((ListItem)cmbEJobCategory.SelectedItem).ID.ToString() != "Y")
                        MessageBox.Show(Consts.AgyTab.JOBCATEGORYMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {


            }

        }

        private void txtEhourlywage_Leave(object sender, EventArgs e)
        {
            if (txtEhourlywage.Text.Length > 3)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtEhourlywage.Text, Consts.StaticVars.TwoDecimalRange3String))
                {

                    CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals3Range);
                    //txtEhourlywage.Text = string.Empty;
                }
            }
        }

        private void btnCitySearch_Click(object sender, EventArgs e)
        {
            ZipCodeSearchForm zipCodeSearchForm = new ZipCodeSearchForm(Privileges, txtZipCode.Text, txtCity.Text);
            zipCodeSearchForm.StartPosition = FormStartPosition.CenterScreen;
            zipCodeSearchForm.FormClosed += new FormClosedEventHandler(OnZipCodeFormClosed);
            zipCodeSearchForm.ShowDialog();
        }

        private void txtCity_Leave(object sender, EventArgs e)
        {
            if (propBaseAgencyControlDetails.EditZip.ToString().Trim() == "1")
            {
                btnCitySearch.Enabled = false;

                List<ZipCodeEntity> zipcodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(string.Empty, txtCity.Text, string.Empty, string.Empty);
                if (zipcodeEntity.Count > 0)
                    zipcodeEntity = zipcodeEntity.FindAll(u => u.InActive.Equals("N") || u.InActive.Trim().Equals(""));
                if (zipcodeEntity.Count > 0)
                {
                    if (zipcodeEntity.Count == 1)
                    {
                        btnCitySearch.Enabled = true;
                        foreach (ZipCodeEntity zipcodedetais in zipcodeEntity)
                        {
                            if (zipcodedetais != null)
                            {
                                string zipPlus = zipcodedetais.Zcrplus4;
                                txtZipPlus.Text = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                                txtZipCode.Text = "00000".Substring(0, 5 - zipcodedetais.Zcrzip.Length) + zipcodedetais.Zcrzip;
                                txtState.Text = zipcodedetais.Zcrstate;
                                txtCity.Text = zipcodedetais.Zcrcity;
                                SetComboBoxValue(cmbCounty, zipcodedetais.Zcrcountry);
                                SetComboBoxValue(cmbTownship, zipcodedetais.Zcrcitycode);
                            }
                        }
                        btnCitySearch.Focus();
                    }

                    if (txtCity.Text.Trim() == string.Empty)
                    {
                        btnCitySearch.Enabled = true;
                        btnCitySearch.Focus();
                    }
                }
                else
                {
                    ZipCodeSearchForm zipCodeSearchForm = new ZipCodeSearchForm(Privileges, txtZipCode.Text, txtCity.Text);
                    zipCodeSearchForm.StartPosition = FormStartPosition.CenterScreen;
                    zipCodeSearchForm.FormClosed += new FormClosedEventHandler(OnZipCodeFormClosed);
                    zipCodeSearchForm.ShowDialog();
                }
                if (btnCitySearch.Enabled == false)
                {
                    txtZipCode.Focus();
                }
            }
        }

        private bool CheckZipcodeDetails()
        {
            bool boolZipSearch = true;
            if (propBaseAgencyControlDetails.EditZip.ToString().Trim() == "1")
            {
                List<ZipCodeEntity> zipcodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(txtZipCode.Text, string.Empty, string.Empty, string.Empty);
                if (zipcodeEntity.Count > 0)
                    zipcodeEntity = zipcodeEntity.FindAll(u => u.InActive.Equals("N") || u.InActive.Trim().Equals(""));
                zipcodeEntity = zipcodeEntity.FindAll(u => u.Zcrzip.Equals(txtZipCode.Text));
                if (zipcodeEntity.Count > 0)
                {
                    foreach (var zipcodesingleEntity in zipcodeEntity)
                    {
                        boolZipSearch = true;
                        if (zipcodesingleEntity.Zcrcitycode.Trim() != string.Empty)
                        {
                            if (!zipcodesingleEntity.Zcrcitycode.Equals(((ListItem)cmbTownship.SelectedItem).Value.ToString()))
                                boolZipSearch = false;
                        }
                        if (zipcodesingleEntity.Zcrcountry.Trim() != string.Empty)
                        {
                            if (!zipcodesingleEntity.Zcrcountry.Equals(((ListItem)cmbCounty.SelectedItem).Value.ToString()))
                                boolZipSearch = false;
                        }
                        if (!zipcodesingleEntity.Zcrcity.Trim().ToUpper().Equals(txtCity.Text.Trim().ToUpper()))
                            boolZipSearch = false;
                        if (boolZipSearch)
                            break;
                    }

                }
            }
            return boolZipSearch;

        }

        private void DisableZipcodeDetails()
        {
            string strEditZip = "0";
            //DataSet ds = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL(BaseForm.BaseAgency, null, null, null, null, null, null);
            //DataRow dr;
            //if (ds.Tables.Count > 0)
            //{
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        dr = ds.Tables[0].Rows[0];
            if (propBaseAgencyControlDetails != null)
            {
                strEditZip = propBaseAgencyControlDetails.EditZip.ToString();//==null?false: ;
                if (strEditZip == "1")
                {
                    txtState.Enabled = false;
                    cmbTownship.Enabled = false;
                    cmbCounty.Enabled = false;
                    btnCitySearch.Enabled = true;
                    btnZipCode.Enabled = true;
                }
                else
                {
                    if (propAgencyControlDetails.State == "TX")
                    {
                        btnCitySearch.Enabled = false;
                        btnZipCode.Enabled = false;

                    }
                    else
                    {
                        btnCitySearch.Enabled = true;
                        btnZipCode.Enabled = true;
                    }
                }
            }
            // booldisableZip = (bool)
            //    }
            //}

        }

        private void cmbStaff_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (cmbStaff.Items.Count > 0)
                {
                    string strcmbstaff = ((ListItem)cmbStaff.SelectedItem).Value == null ? string.Empty : ((ListItem)cmbStaff.SelectedItem).Value.ToString();
                    if (!string.IsNullOrEmpty(strcmbstaff))
                    {

                        if (((ListItem)cmbStaff.SelectedItem).Value.ToString() != "0")
                            if (((ListItem)cmbStaff.SelectedItem).ID.ToString() != "N")
                                MessageBox.Show("Inactive CaseWorker", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
            }
            catch (Exception ex)
            {


            }

        }

        private void cmbVerifiedStaff_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbVerifiedStaff.Items.Count > 0)
                {
                    string strcmbverified = ((ListItem)cmbVerifiedStaff.SelectedItem).Value == null ? string.Empty : ((ListItem)cmbVerifiedStaff.SelectedItem).Value.ToString();
                    if (!string.IsNullOrEmpty(strcmbverified))
                    {
                        if (((ListItem)cmbVerifiedStaff.SelectedItem).Value.ToString() != "0")
                        {
                            if (((ListItem)cmbVerifiedStaff.SelectedItem).ID.ToString() != "N")
                                MessageBox.Show("Inactive CaseWorker", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        }
                    }
                }
                CalclationEmployee();
            }
            catch (Exception ex)
            {


            }
        }

        private bool CheckSiteDetails()
        {
            bool boolsiteDetails = true;
            if (txtSite.Text != string.Empty)
            {
                CaseSiteEntity CaseSiteDetails = CaseSiteEntityList.Find(u => Convert.ToString(u.SiteNUMBER.ToUpper().Trim()).Equals(txtSite.Text.ToUpper().Trim()));
                if (CaseSiteDetails == null)
                {
                    CommonFunctions.MessageBoxDisplay("Site Not found in CASESITE file");
                    boolsiteDetails = false;
                    txtSite.Text = string.Empty;
                }
            }
            return boolsiteDetails;
        }

        private void txtSite_Leave(object sender, EventArgs e)
        {
            CheckSiteDetails();
        }

        private void dtBirth_CheckedChanged(object sender, EventArgs e)
        {
            if (dtBirth.Checked == true)
            {
                string strIntakeDate = CaseMST.IntakeDate;
                if (strIntakeDate == string.Empty || strIntakeDate == null)
                    strIntakeDate = DateTime.Now.Date.ToShortDateString();

                txtYear.Text = CommonFunctions.CalculationYear(Convert.ToDateTime(dtBirth.Text.Trim()), Convert.ToDateTime(strIntakeDate));
                txtCurrentAge.Text = CommonFunctions.CalculationYear(Convert.ToDateTime(dtBirth.Text.Trim()), Convert.ToDateTime(DateTime.Now.Date.ToShortDateString()));
            }
            else
            {
                txtYear.Text = string.Empty;
                txtCurrentAge.Text = string.Empty;
            }
        }

        private void dtEHireDate_ValueChanged(object sender, EventArgs e)
        {
            if (dtEHireDate.Checked == true)
            {
                CalculateLengthoftimeEmployed();
            }
        }

        private void dtEHireDate_Leave(object sender, EventArgs e)
        {
            if (dtEHireDate.Checked == true)
            {
                CalculateLengthoftimeEmployed();
            }
        }

        private void dtEHireDate_Click(object sender, EventArgs e)
        {

        }

        private void CalculateLengthoftimeEmployed()
        {
            //int intpresentYear = DateTime.Now.Year;
            //int intpresentMonth = DateTime.Now.Month;
            //int intpresentDaY = DateTime.Now.Day;
            int intpresentdays = ((Convert.ToInt32(DateTime.Now.Year) * 365) + (Convert.ToInt32(DateTime.Now.Month) * 30) + Convert.ToInt32(DateTime.Now.Day));
            int inthierdays = ((Convert.ToInt32(dtEHireDate.Value.Year) * 365) + (Convert.ToInt32(dtEHireDate.Value.Month) * 30) + Convert.ToInt32(dtEHireDate.Value.Day));
            int intTotaldays = intpresentdays - inthierdays;
            txtELengthofTime.Text = string.Empty;
            if ((intTotaldays < 30) && (intTotaldays > 0))
            {
                txtELengthofTime.Text = intTotaldays + " days";
            }
            else if ((intTotaldays < 60) && (intTotaldays > 0))
            {
                txtELengthofTime.Text = "30 days";
            }
            else if ((intTotaldays < 90) && (intTotaldays > 0))
            {
                txtELengthofTime.Text = "60 days";
            }
            else if ((intTotaldays < 120) && (intTotaldays > 0))
            {
                txtELengthofTime.Text = "90 days";
            }
            else if ((intTotaldays < 365) && (intTotaldays > 0))
            {
                txtELengthofTime.Text = "120 days";
            }
            else if (intTotaldays > 364)
            {
                int intyear = intTotaldays / 365;
                if (intyear > 0 && intyear < 3)
                {
                    txtELengthofTime.Text = "1 Year";
                }
                else if ((!(intyear < 3)) && intyear < 5)
                {
                    txtELengthofTime.Text = "3 Years";
                }
                else if (!(intyear < 5))
                {
                    txtELengthofTime.Text = intyear + " Year";
                }
            }


        }

        private void tabControl1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    btnCancel.TabIndex = 0;
                    txtFirstName.Focus();
                    mskSSN.Focus();
                    break;
                case 1:
                    btnCancel.Focus();
                    btnCancel.TabIndex = 0;
                    cmbStaff.Focus();
                    break;
                case 2:
                    btnCancel.TabIndex = 0;
                    cmbVerifiedStaff.Focus();
                    break;
                case 3:
                    btnCancel.TabIndex = 0;
                    cmbEpresenteEmploy.Focus();
                    break;
                default:
                    break;
            }
        }

        private void btnCancel_Leave(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    //  btnCancel.Focus();
                    txtFirstName.Focus();
                    mskSSN.Focus();
                    break;
                case 1:
                    // btnCancel.Focus();
                    cmbStaff.Focus();
                    break;
                case 2:
                    //  btnCancel.Focus();
                    cmbVerifiedStaff.Focus();
                    break;
                case 3:
                    //  btnCancel.Focus();
                    cmbEpresenteEmploy.Focus();
                    break;
                default:
                    break;
            }
        }



        private void InvokeFocusCommand(Control objControl)
        {
            //IApplicationContext objApplicationContext = this.Context as IApplicationContext;

            //if (objApplicationContext != null)
            //{
            //    objApplicationContext.SetFocused(objControl, true);
            //}

            //Alaa: Please delete if it's the right thing.
            Application.Update(this, () =>
            {
                objControl.Focus();
            });
        }

        private void txtLastName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == (char)Keys.PageDown) && (!IsApplicant) && (Mode.Equals(Consts.Common.Add)))
            {
                txtLastName.Text = string.Empty;
                txtLastName.Text = ApplicantLastName;
                this.InvokeFocusCommand(txtLastName);
            }

        }


        private string SsnZeros(int intLength, string strType)
        {
            string strValue = string.Empty;
            if (strType == "SSN")
            {
                switch (intLength)
                {
                    case 1:
                        strValue = "00000000";
                        break;
                    case 2:
                        strValue = "0000000";
                        break;
                    case 3:
                        strValue = "000000";
                        break;
                    case 4:
                        strValue = "00000";
                        break;
                    case 5:
                        strValue = "0000";
                        break;
                    case 6:
                        strValue = "000";
                        break;
                    case 7:
                        strValue = "00";
                        break;
                    case 8:
                        strValue = "0";
                        break;
                }
            }
            else
            {
                switch (intLength)
                {
                    case 1:
                        strValue = "000000000";
                        break;
                    case 2:
                        strValue = "00000000";
                        break;
                    case 3:
                        strValue = "0000000";
                        break;
                    case 4:
                        strValue = "000000";
                        break;
                    case 5:
                        strValue = "00000";
                        break;
                    case 6:
                        strValue = "0000";
                        break;
                    case 7:
                        strValue = "000";
                        break;
                    case 8:
                        strValue = "00";
                        break;
                    case 9:
                        strValue = "0";
                        break;
                }
            }
            return strValue;
        }

        private void txtHomePhone_Leave(object sender, EventArgs e)
        {
            //if (txtHomePhone.TextLength > 0)
            //{
            //    txtHomePhone.Text = txtHomePhone.Text.Replace(' ', '0') + SsnZeros(txtHomePhone.TextLength, string.Empty);
            //}
        }


        private void CheckHistoryTableData(CaseMstEntity caseHistMst, CaseSnpEntity casesnpHist)
        {
            string strHistoryDetails = "<XmlHistory>";
            bool boolHistory = false;
            bool boolAddressHistory = false;
            if (IsApplicant)
            {

                //if (CaseMST.FamilySeq.Trim() != caseHistMst.FamilySeq.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_FAMILY_SEQ</FieldName><OldValue>" + CaseMST.FamilySeq + "</OldValue><NewValue>" + caseHistMst.FamilySeq + "</NewValue></HistoryFields>";
                //}

                //if (CaseMST.FamilyId.Trim() != caseHistMst.FamilyId.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_FAMILY_ID</FieldName><OldValue>" + CaseMST.FamilyId + "</OldValue><NewValue>" + caseHistMst.FamilyId + "</NewValue></HistoryFields>";
                //}

                //if (CaseMST.ClientId.Trim() != caseHistMst.ClientId.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_CLIENT_ID</FieldName><OldValue>" + CaseMST.ClientId + "</OldValue><NewValue>" + caseHistMst.ClientId + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.Ssn.Trim() != caseHistMst.Ssn.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SSN#</FieldName><OldValue>" + CaseMST.Ssn + "</OldValue><NewValue>" + caseHistMst.Ssn + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.Bic.Trim() != caseHistMst.Bic.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_BIC</FieldName><OldValue>" + CaseMST.Bic + "</OldValue><NewValue>" + caseHistMst.Bic + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.NickName.Trim() != caseHistMst.NickName.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_NICKNAME</FieldName><OldValue>" + CaseMST.NickName + "</OldValue><NewValue>" + caseHistMst.NickName + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.EthnicOther.Trim() != caseHistMst.EthnicOther.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_ETHNIC_OTHER</FieldName><OldValue>" + CaseMST.EthnicOther + "</OldValue><NewValue>" + caseHistMst.EthnicOther + "</NewValue></HistoryFields>";
                // }
                if (CaseMST.State.ToUpper().Trim() != caseHistMst.State.ToUpper().Trim())
                {
                    boolAddressHistory = boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>State</FieldName><OldValue>" + CaseMST.State + "</OldValue><NewValue>" + caseHistMst.State + "</NewValue></HistoryFields>";
                }
                if (CaseMST.City.ToUpper().Trim() != caseHistMst.City.ToUpper().Trim())
                {
                    boolAddressHistory = boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>City Name</FieldName><OldValue>" + CaseMST.City + "</OldValue><NewValue>" + caseHistMst.City + "</NewValue></HistoryFields>";
                }
                if (CaseMST.Street.Trim().ToUpper() != caseHistMst.Street.Trim().ToUpper())
                {
                    boolAddressHistory = boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Street</FieldName><OldValue>" + CaseMST.Street + "</OldValue><NewValue>" + caseHistMst.Street + "</NewValue></HistoryFields>";
                }
                if (CaseMST.Suffix.Trim().ToUpper() != caseHistMst.Suffix.Trim().ToUpper())
                {
                    boolAddressHistory = boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Suffix</FieldName><OldValue>" + CaseMST.Suffix + "</OldValue><NewValue>" + caseHistMst.Suffix + "</NewValue></HistoryFields>";
                }
                if (CaseMST.Hn.Trim() != caseHistMst.Hn.Trim())
                {
                    boolAddressHistory = boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>HN</FieldName><OldValue>" + CaseMST.Hn + "</OldValue><NewValue>" + caseHistMst.Hn + "</NewValue></HistoryFields>";
                }
                if (CaseMST.Direction.Trim() != caseHistMst.Direction.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Direction</FieldName><OldValue>" + CaseMST.Direction + "</OldValue><NewValue>" + caseHistMst.Direction + "</NewValue></HistoryFields>";
                }
                if (CaseMST.Apt.Trim().ToUpper() != caseHistMst.Apt.Trim().ToUpper())
                {
                    boolAddressHistory = boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Apartment</FieldName><OldValue>" + CaseMST.Apt + "</OldValue><NewValue>" + caseHistMst.Apt + "</NewValue></HistoryFields>";
                }
                if (CaseMST.Flr.Trim() != caseHistMst.Flr.Trim())
                {
                    boolAddressHistory = boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Floor</FieldName><OldValue>" + CaseMST.Flr + "</OldValue><NewValue>" + caseHistMst.Flr + "</NewValue></HistoryFields>";
                }
                if ("00000".Substring(0, 5 - CaseMST.Zip.Length) + CaseMST.Zip != "00000".Substring(0, 5 - caseHistMst.Zip.Length) + caseHistMst.Zip.Trim())
                {
                    boolAddressHistory = boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Zip Code</FieldName><OldValue>" + CaseMST.Zip + "</OldValue><NewValue>" + caseHistMst.Zip + "</NewValue></HistoryFields>";
                }
                if ("0000".Substring(0, 4 - CaseMST.Zipplus.Length) + CaseMST.Zipplus != "0000".Substring(0, 4 - caseHistMst.Zipplus.Length) + caseHistMst.Zipplus.Trim())
                {
                    boolAddressHistory = boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Zip Plus</FieldName><OldValue>" + CaseMST.Zipplus + "</OldValue><NewValue>" + caseHistMst.Zipplus + "</NewValue></HistoryFields>";
                }
                if (CaseMST.Precinct.Trim() != caseHistMst.Precinct.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Precinct</FieldName><OldValue>" + CaseMST.Precinct + "</OldValue><NewValue>" + caseHistMst.Precinct + "</NewValue></HistoryFields>";
                }
                // Area Home phone no
                //if (CaseMST.Area.Trim() != caseHistMst.Area.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Area</FieldName><OldValue>" + CaseMST.Area + "</OldValue><NewValue>" + caseHistMst.Area + "</NewValue></HistoryFields>";
                //}
                if ((CaseMST.Area.Trim() + CaseMST.Phone.Trim()) != (caseHistMst.Area.Trim() + caseHistMst.Phone.Trim()))
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Home Phone</FieldName><OldValue>" + LookupDataAccess.GetPhoneSsnNoFormat(CaseMST.Area + CaseMST.Phone) + "</OldValue><NewValue>" + LookupDataAccess.GetPhoneSsnNoFormat(caseHistMst.Area + caseHistMst.Phone) + "</NewValue></HistoryFields>";
                }
                if (CaseMST.HomePhone_NA.Trim() != caseHistMst.HomePhone_NA.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Home NA</FieldName><OldValue>" + (CaseMST.HomePhone_NA == "Y" ? "Active" : "Inactive") + "</OldValue><NewValue>" + (caseHistMst.HomePhone_NA == "Y" ? "Active" : "Inactive") + "</NewValue></HistoryFields>";
                }
                //if (CaseMST.NextYear.Trim() != caseHistMst.NextYear.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Next Year</FieldName><OldValue>" + CaseMST.NextYear + "</OldValue><NewValue>" + caseHistMst.NextYear + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.Classification.Trim() != caseHistMst.Classification.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_CLASSIFICATION</FieldName><OldValue>" + CaseMST.Classification + "</OldValue><NewValue>" + caseHistMst.Classification + "</NewValue></HistoryFields>";
                //}
                if (CaseMST.Language.Trim() != caseHistMst.Language.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Primary Language</FieldName><OldValue>" + CaseMST.PrimaryLanDesc + "</OldValue><NewValue>" + caseHistMst.PrimaryLanDesc + "</NewValue></HistoryFields>";
                }
                if (CaseMST.LanguageOt.Trim() != caseHistMst.LanguageOt.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Second Language</FieldName><OldValue>" + CaseMST.SecondaryLanDesc + "</OldValue><NewValue>" + caseHistMst.SecondaryLanDesc + "</NewValue></HistoryFields>";
                }
                if (CaseMST.IntakeWorker.Trim() != caseHistMst.IntakeWorker.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Assigned Worker</FieldName><OldValue>" + CaseMST.AssignWorkerDesc + "</OldValue><NewValue>" + caseHistMst.AssignWorkerDesc + "</NewValue></HistoryFields>";
                }
                if (CaseMST.IntakeDate.Trim() != caseHistMst.IntakeDate.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Intake Date</FieldName><OldValue>" + CaseMST.IntakeDate + "</OldValue><NewValue>" + caseHistMst.IntakeDate + "</NewValue></HistoryFields>";
                }
                if (CaseMST.InitialDate.Trim() != caseHistMst.InitialDate.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Initial Date</FieldName><OldValue>" + CaseMST.InitialDate + "</OldValue><NewValue>" + caseHistMst.InitialDate + "</NewValue></HistoryFields>";
                }
                if (CaseMST.CaseType.Trim() != caseHistMst.CaseType.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Case Type</FieldName><OldValue>" + CaseMST.CaseTypeDesc + "</OldValue><NewValue>" + caseHistMst.CaseTypeDesc + "</NewValue></HistoryFields>";
                }
                if (CaseMST.Housing.Trim() != caseHistMst.Housing.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Housing Situation</FieldName><OldValue>" + CaseMST.HousingSituvationDesc + "</OldValue><NewValue>" + caseHistMst.HousingSituvationDesc + "</NewValue></HistoryFields>";
                }
                if (CaseMST.FamilyType.Trim() != caseHistMst.FamilyType.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Family Type</FieldName><OldValue>" + CaseMST.FamilyTypeDesc + "</OldValue><NewValue>" + caseHistMst.FamilyTypeDesc + "</NewValue></HistoryFields>";
                }
                if (CaseMST.Site.Trim() != caseHistMst.Site.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Site</FieldName><OldValue>" + CaseMST.Site + "</OldValue><NewValue>" + caseHistMst.Site + "</NewValue></HistoryFields>";
                }
                if (CaseMST.Juvenile.Trim() != caseHistMst.Juvenile.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Juvenile</FieldName><OldValue>" + CaseMST.Juvenile + "</OldValue><NewValue>" + caseHistMst.Juvenile + "</NewValue></HistoryFields>";
                }
                if (CaseMST.Senior.Trim() != caseHistMst.Senior.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Senior</FieldName><OldValue>" + CaseMST.Senior + "</OldValue><NewValue>" + caseHistMst.Senior + "</NewValue></HistoryFields>";
                }
                if (CaseMST.Secret.Trim() != caseHistMst.Secret.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Secret</FieldName><OldValue>" + CaseMST.Secret + "</OldValue><NewValue>" + caseHistMst.Secret + "</NewValue></HistoryFields>";
                }
                if (CaseMST.CaseReviewDate.Trim() != caseHistMst.CaseReviewDate.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Case Review</FieldName><OldValue>" + CaseMST.CaseReviewDate + "</OldValue><NewValue>" + caseHistMst.CaseReviewDate + "</NewValue></HistoryFields>";
                }
                //if (CaseMST.AlertCodes.Trim() != caseHistMst.AlertCodes.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_ALERT_CODES</FieldName><OldValue>" + CaseMST.AlertCodes + "</OldValue><NewValue>" + caseHistMst.AlertCodes + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.ParentStatus.Trim() != caseHistMst.ParentStatus.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_PARENT_STATUS</FieldName><OldValue>" + CaseMST.ParentStatus + "</OldValue><NewValue>" + caseHistMst.ParentStatus + "</NewValue></HistoryFields>";
                //}

                //if (CaseMST.IntakeHrs.Trim() != caseHistMst.IntakeHrs.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_INTAKE_HRS</FieldName><OldValue>" + CaseMST.IntakeHrs + "</OldValue><NewValue>" + caseHistMst.IntakeHrs + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.IntakeMns.Trim() != caseHistMst.IntakeMns.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_INTAKE_MNS</FieldName><OldValue>" + CaseMST.IntakeMns + "</OldValue><NewValue>" + caseHistMst.IntakeMns + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.IntakeScs.Trim() != caseHistMst.IntakeScs.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_INTAKE_SCS</FieldName><OldValue>" + CaseMST.IntakeScs + "</OldValue><NewValue>" + caseHistMst.IntakeScs + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.FinHrs.Trim() != caseHistMst.FinHrs.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_FIN_HRS</FieldName><OldValue>" + CaseMST.FinHrs + "</OldValue><NewValue>" + caseHistMst.FinHrs + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.FinMns.Trim() != caseHistMst.FinMns.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_FIN_MNS</FieldName><OldValue>" + CaseMST.FinMns + "</OldValue><NewValue>" + caseHistMst.FinMns + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.FinScs.Trim() != caseHistMst.FinScs.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_FIN_SCS</FieldName><OldValue>" + CaseMST.FinScs + "</OldValue><NewValue>" + caseHistMst.FinScs + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.SimHrs.Trim() != caseHistMst.SimHrs.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_SIM_HRS</FieldName><OldValue>" + CaseMST.SimHrs + "</OldValue><NewValue>" + caseHistMst.SimHrs + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.SimMns.Trim() != caseHistMst.SimMns.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_SIM_MNS</FieldName><OldValue>" + CaseMST.SimMns + "</OldValue><NewValue>" + caseHistMst.SimMns + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.SimScs.Trim() != caseHistMst.SimScs.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_SIM_SCS</FieldName><OldValue>" + CaseMST.SimScs + "</OldValue><NewValue>" + caseHistMst.SimScs + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.MedHrs.Trim() != caseHistMst.MedHrs.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_MED_HRS</FieldName><OldValue>" + CaseMST.MedHrs + "</OldValue><NewValue>" + caseHistMst.MedHrs + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.MedMns.Trim() != caseHistMst.MedMns.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_MED_MNS</FieldName><OldValue>" + CaseMST.MedMns + "</OldValue><NewValue>" + caseHistMst.MedMns + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.MedScs.Trim() != caseHistMst.MedScs.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_MED_SCS</FieldName><OldValue>" + CaseMST.MedScs + "</OldValue><NewValue>" + caseHistMst.MedScs + "</NewValue></HistoryFields>";
                //}

                if (CaseMST.TownShip.Trim() != caseHistMst.TownShip.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Township</FieldName><OldValue>" + CaseMST.TownShipDesc + "</OldValue><NewValue>" + caseHistMst.TownShipDesc + "</NewValue></HistoryFields>";
                }


                //if (CaseMST.Rank1.Trim() != caseHistMst.Rank1.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_RANK1</FieldName><OldValue>" + CaseMST.Rank1 + "</OldValue><NewValue>" + caseHistMst.Rank1 + "</NewValue></HistoryFields>";
                //}

                //if (CaseMST.Rank2.Trim() != caseHistMst.Rank2.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_RANK2</FieldName><OldValue>" + CaseMST.Rank2 + "</OldValue><NewValue>" + caseHistMst.Rank2 + "</NewValue></HistoryFields>";
                //}

                //if (CaseMST.Rank3.Trim() != caseHistMst.Rank3.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_RANK3</FieldName><OldValue>" + CaseMST.Rank3 + "</OldValue><NewValue>" + caseHistMst.Rank3 + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.Rank4.Trim() != caseHistMst.Rank4.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_RANK4</FieldName><OldValue>" + CaseMST.Rank4 + "</OldValue><NewValue>" + caseHistMst.Rank4 + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.Rank5.Trim() != caseHistMst.Rank5.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_RANK5</FieldName><OldValue>" + CaseMST.Rank5 + "</OldValue><NewValue>" + caseHistMst.Rank5 + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.Rank6.Trim() != caseHistMst.Rank6.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_RANK6</FieldName><OldValue>" + CaseMST.Rank6 + "</OldValue><NewValue>" + caseHistMst.Rank6 + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.Position1.Trim() != caseHistMst.Position1.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_POSITION1</FieldName><OldValue>" + CaseMST.Position1 + "</OldValue><NewValue>" + caseHistMst.Position1 + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.Position2.Trim() != caseHistMst.Position2.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_POSITION2</FieldName><OldValue>" + CaseMST.Position2 + "</OldValue><NewValue>" + caseHistMst.Position2 + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.Position3.Trim() != caseHistMst.Position3.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_POSITION3</FieldName><OldValue>" + CaseMST.Position3 + "</OldValue><NewValue>" + caseHistMst.Position3 + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.IntakeTime1.Trim() != caseHistMst.IntakeTime1.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_INTAKE_TIME1</FieldName><OldValue>" + CaseMST.IntakeTime1 + "</OldValue><NewValue>" + caseHistMst.IntakeTime1 + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.SsnFlag.Trim() != caseHistMst.SsnFlag.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_SSN_FLAG</FieldName><OldValue>" + CaseMST.SsnFlag + "</OldValue><NewValue>" + caseHistMst.SsnFlag + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.StateCase.Trim() != caseHistMst.StateCase.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_STATE_CASE</FieldName><OldValue>" + CaseMST.StateCase + "</OldValue><NewValue>" + caseHistMst.StateCase + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.Verifier.Trim() != caseHistMst.Verifier.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_VERIFIER</FieldName><OldValue>" + CaseMST.Verifier + "</OldValue><NewValue>" + caseHistMst.Verifier + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.EligDate.Trim() != caseHistMst.EligDate.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_ELIG_DATE</FieldName><OldValue>" + CaseMST.EligDate + "</OldValue><NewValue>" + caseHistMst.EligDate + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.CatElig.Trim() != caseHistMst.CatElig.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_CAT_ELIG</FieldName><OldValue>" + CaseMST.CatElig + "</OldValue><NewValue>" + caseHistMst.CatElig + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.MealElig.Trim() != caseHistMst.MealElig.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_MEAL_ELIG</FieldName><OldValue>" + CaseMST.MealElig + "</OldValue><NewValue>" + caseHistMst.MealElig + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.VerifyW2.Trim() != caseHistMst.VerifyW2.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_VERIFY_W2</FieldName><OldValue>" + CaseMST.VerifyW2 + "</OldValue><NewValue>" + caseHistMst.VerifyW2 + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.VerifyCheckStub.Trim() != caseHistMst.VerifyCheckStub.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_VERIFY_CHECK_STUB</FieldName><OldValue>" + CaseMST.VerifyCheckStub + "</OldValue><NewValue>" + caseHistMst.VerifyCheckStub + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.VerifyTaxReturn.Trim() != caseHistMst.VerifyTaxReturn.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_VERIFY_TAX_RETURN</FieldName><OldValue>" + CaseMST.VerifyTaxReturn + "</OldValue><NewValue>" + caseHistMst.VerifyTaxReturn + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.VerifyLetter.Trim() != caseHistMst.VerifyLetter.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_VERIFY_LETTER</FieldName><OldValue>" + CaseMST.VerifyLetter + "</OldValue><NewValue>" + caseHistMst.VerifyLetter + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.VerifyOther.Trim() != caseHistMst.VerifyOther.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_VERIFY_OTHER</FieldName><OldValue>" + CaseMST.VerifyOther + "</OldValue><NewValue>" + caseHistMst.VerifyOther + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.ReverifyDate.Trim() != caseHistMst.ReverifyDate.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_REVERIFY_DATE</FieldName><OldValue>" + CaseMST.ReverifyDate + "</OldValue><NewValue>" + caseHistMst.ReverifyDate + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.IncomeTypes.Trim() != caseHistMst.IncomeTypes.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_INCOME_TYPES</FieldName><OldValue>" + CaseMST.IncomeTypes + "</OldValue><NewValue>" + caseHistMst.IncomeTypes + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.Poverty.Trim() != caseHistMst.Poverty.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_POVERTY</FieldName><OldValue>" + CaseMST.Poverty + "</OldValue><NewValue>" + caseHistMst.Poverty + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.WaitList.Trim() != caseHistMst.WaitList.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_WAIT_LIST</FieldName><OldValue>" + CaseMST.WaitList + "</OldValue><NewValue>" + caseHistMst.WaitList + "</NewValue></HistoryFields>";
                //}
                if (CaseMST.ActiveStatus.Trim() != caseHistMst.ActiveStatus.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Active</FieldName><OldValue>" + CaseMST.ActiveStatus + "</OldValue><NewValue>" + caseHistMst.ActiveStatus + "</NewValue></HistoryFields>";
                }
                //if (CaseMST.TotalRank.Trim() != caseHistMst.TotalRank.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_TOTAL_RANK</FieldName><OldValue>" + CaseMST.TotalRank + "</OldValue><NewValue>" + caseHistMst.TotalRank + "</NewValue></HistoryFields>";
                //}
                if (CaseMST.NoInhh.Trim() != caseHistMst.NoInhh.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName># in House</FieldName><OldValue>" + CaseMST.NoInhh + "</OldValue><NewValue>" + caseHistMst.NoInhh + "</NewValue></HistoryFields>";
                }
                if (CaseMST.FamIncome.Trim() != caseHistMst.FamIncome.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Total Income</FieldName><OldValue>" + CaseMST.FamIncome + "</OldValue><NewValue>" + caseHistMst.FamIncome + "</NewValue></HistoryFields>";
                }
                if (CaseMST.NoInProg.Trim() != caseHistMst.NoInProg.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName># in Prog</FieldName><OldValue>" + CaseMST.NoInProg + "</OldValue><NewValue>" + caseHistMst.NoInProg + "</NewValue></HistoryFields>";
                }
                if (CaseMST.ProgIncome.Trim() != caseHistMst.ProgIncome.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Programe Income</FieldName><OldValue>" + CaseMST.ProgIncome + "</OldValue><NewValue>" + caseHistMst.ProgIncome + "</NewValue></HistoryFields>";
                }

                //if (CaseMST.OutOfService.Trim() != caseHistMst.OutOfService.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_OUT_OF_SERVICE</FieldName><OldValue>" + CaseMST.OutOfService + "</OldValue><NewValue>" + caseHistMst.OutOfService + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.Hud .Trim() != caseHistMst.Hud.Trim())
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_HUD</FieldName><OldValue>"+ + "</OldValue><NewValue>"+ +"</NewValue></HistoryFields>";

                //if (CaseMST.Smi .Trim() != caseHistMst.Smi.Trim())
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_SMI</FieldName><OldValue>"+ + "</OldValue><NewValue>"+ +"</NewValue></HistoryFields>";

                //if (CaseMST.Cmi .Trim() != caseHistMst.Cmi.Trim())
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_CMI</FieldName><OldValue>"+ + "</OldValue><NewValue>"+ +"</NewValue></HistoryFields>";

                if (CaseMST.County.Trim() != caseHistMst.County.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>County</FieldName><OldValue>" + CaseMST.CountyDesc + "</OldValue><NewValue>" + caseHistMst.CountyDesc + "</NewValue></HistoryFields>";
                }
                if (CaseMST.AddressYears.Trim() != caseHistMst.AddressYears.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>No of years at address</FieldName><OldValue>" + CaseMST.AddressYears + "</OldValue><NewValue>" + caseHistMst.AddressYears + "</NewValue></HistoryFields>";
                }
                if (CaseMST.MessagePhone_NA.Trim() != caseHistMst.MessagePhone_NA.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Message NA</FieldName><OldValue>" + (CaseMST.MessagePhone_NA == "Y" ? "Active" : "Inactive") + "</OldValue><NewValue>" + (caseHistMst.MessagePhone_NA == "Y" ? "Active" : "Inactive") + "</NewValue></HistoryFields>";
                }
                if (CaseMST.MessagePhone.Trim() != caseHistMst.MessagePhone.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Message</FieldName><OldValue>" + LookupDataAccess.GetPhoneSsnNoFormat(CaseMST.MessagePhone) + "</OldValue><NewValue>" + LookupDataAccess.GetPhoneSsnNoFormat(caseHistMst.MessagePhone) + "</NewValue></HistoryFields>";
                }
                if (CaseMST.CellPhone_NA.Trim() != caseHistMst.CellPhone_NA.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Cell NA</FieldName><OldValue>" + (CaseMST.CellPhone_NA == "Y" ? "Active" : "Inactive") + "</OldValue><NewValue>" + (caseHistMst.CellPhone_NA == "Y" ? "Active" : "Inactive") + "</NewValue></HistoryFields>";
                }
                if (CaseMST.CellPhone.Trim() != caseHistMst.CellPhone.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Cell</FieldName><OldValue>" + LookupDataAccess.GetPhoneSsnNoFormat(CaseMST.CellPhone) + "</OldValue><NewValue>" + LookupDataAccess.GetPhoneSsnNoFormat(caseHistMst.CellPhone) + "</NewValue></HistoryFields>";
                }
                if (CaseMST.FaxNumber.Trim() != caseHistMst.FaxNumber.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Fax</FieldName><OldValue>" + LookupDataAccess.GetPhoneSsnNoFormat(CaseMST.FaxNumber) + "</OldValue><NewValue>" + LookupDataAccess.GetPhoneSsnNoFormat(caseHistMst.FaxNumber) + "</NewValue></HistoryFields>";
                }
                if (CaseMST.TtyNumber.Trim() != caseHistMst.TtyNumber.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>TTY</FieldName><OldValue>" + LookupDataAccess.GetPhoneSsnNoFormat(CaseMST.TtyNumber) + "</OldValue><NewValue>" + LookupDataAccess.GetPhoneSsnNoFormat(caseHistMst.TtyNumber) + "</NewValue></HistoryFields>";
                }
                if (CaseMST.Email_NA.Trim() != caseHistMst.Email_NA.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Email NA</FieldName><OldValue>" + (CaseMST.Email_NA == "Y" ? "Active" : "Inactive") + "</OldValue><NewValue>" + (caseHistMst.Email_NA == "Y" ? "Active" : "Inactive") + "</NewValue></HistoryFields>";
                }
                if (CaseMST.Email.Trim() != caseHistMst.Email.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Email</FieldName><OldValue>" + CaseMST.Email + "</OldValue><NewValue>" + caseHistMst.Email + "</NewValue></HistoryFields>";
                }
                if (CaseMST.BestContact.Trim() != caseHistMst.BestContact.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>What is the best way to contact you</FieldName><OldValue>" + CaseMST.ContactusDesc + "</OldValue><NewValue>" + caseHistMst.ContactusDesc + "</NewValue></HistoryFields>";
                }
                if (CaseMST.AboutUs.Trim() != caseHistMst.AboutUs.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>How did you find out about us?</FieldName><OldValue>" + CaseMST.AboutUsDesc + "</OldValue><NewValue>" + caseHistMst.AboutUsDesc + "</NewValue></HistoryFields>";
                }
                if (CaseMST.ImportDate.Trim() != caseHistMst.ImportDate.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_IMPORT_DATE</FieldName><OldValue>" + CaseMST.ImportDate + "</OldValue><NewValue>" + caseHistMst.ImportDate + "</NewValue></HistoryFields>";
                }
                //if (CaseMST.DateAdded.Trim() != caseHistMst.)
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_DATE_ADDED</FieldName><OldValue>"+ + "</OldValue><NewValue>"+ +"</NewValue></HistoryFields>" CaseMST.DateAdded));

                if (CaseMST.ExpRent.Trim() != caseHistMst.ExpRent.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Rent/Mortgage</FieldName><OldValue>" + CaseMST.ExpRent + "</OldValue><NewValue>" + caseHistMst.ExpRent + "</NewValue></HistoryFields>";
                }
                if (CaseMST.ExpWater.Trim() != caseHistMst.ExpWater.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Water/Sewer</FieldName><OldValue>" + CaseMST.ExpWater + "</OldValue><NewValue>" + caseHistMst.ExpWater + "</NewValue></HistoryFields>";
                }
                if (CaseMST.ExpElectric.Trim() != caseHistMst.ExpElectric.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Electric</FieldName><OldValue>" + CaseMST.ExpElectric + "</OldValue><NewValue>" + caseHistMst.ExpElectric + "</NewValue></HistoryFields>";
                }
                if (CaseMST.ExpHeat.Trim() != caseHistMst.ExpHeat.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Heating</FieldName><OldValue>" + CaseMST.ExpHeat + "</OldValue><NewValue>" + caseHistMst.ExpHeat + "</NewValue></HistoryFields>";
                }

                if (CaseMST.ExpMisc.Trim() != caseHistMst.ExpMisc.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Misc </FieldName><OldValue>" + CaseMST.ExpMisc + "</OldValue><NewValue>" + caseHistMst.ExpMisc + "</NewValue></HistoryFields>";
                }

                if (CaseMST.Debtcc.Trim() != caseHistMst.Debtcc.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Credit Card Debt</FieldName><OldValue>" + CaseMST.Debtcc + "</OldValue><NewValue>" + caseHistMst.Debtcc + "</NewValue></HistoryFields>";
                }
                if (CaseMST.DebtLoans.Trim() != caseHistMst.DebtLoans.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Loans</FieldName><OldValue>" + CaseMST.DebtLoans + "</OldValue><NewValue>" + caseHistMst.DebtLoans + "</NewValue></HistoryFields>";
                }
                if (CaseMST.DebtMed.Trim() != caseHistMst.DebtMed.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Medical Debt</FieldName><OldValue>" + CaseMST.DebtMed + "</OldValue><NewValue>" + caseHistMst.DebtMed + "</NewValue></HistoryFields>";
                }
                if (CaseMST.DebtOther.Trim() != caseHistMst.DebtOther.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Other Debt</FieldName><OldValue>" + CaseMST.DebtOther + "</OldValue><NewValue>" + caseHistMst.DebtOther + "</NewValue></HistoryFields>";
                }

                if (CaseMST.DebtMisc.Trim() != caseHistMst.DebtMisc.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Misc Debt</FieldName><OldValue>" + CaseMST.DebtMisc + "</OldValue><NewValue>" + caseHistMst.DebtMisc + "</NewValue></HistoryFields>";
                }

                if (CaseMST.AsetPhy.Trim() != caseHistMst.AsetPhy.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Physical Assets</FieldName><OldValue>" + CaseMST.AsetPhy + "</OldValue><NewValue>" + caseHistMst.AsetPhy + "</NewValue></HistoryFields>";
                }

                if (CaseMST.AsetLiq.Trim() != caseHistMst.AsetLiq.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Liquid Assets</FieldName><OldValue>" + CaseMST.AsetLiq + "</OldValue><NewValue>" + caseHistMst.AsetLiq + "</NewValue></HistoryFields>";
                }

                if (CaseMST.AsetOth.Trim() != caseHistMst.AsetOth.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Other Assets</FieldName><OldValue>" + CaseMST.AsetOth + "</OldValue><NewValue>" + caseHistMst.AsetOth + "</NewValue></HistoryFields>";
                }
                double MstTotalAssets = 0.00;
                if (!string.IsNullOrEmpty(CaseMST.AsetTotal))
                {
                    MstTotalAssets = Convert.ToDouble(CaseMST.AsetTotal);
                }
                double histTotalAssets = 0.00;
                if (!string.IsNullOrEmpty(caseHistMst.AsetTotal))
                {
                    histTotalAssets = Convert.ToDouble(caseHistMst.AsetTotal);
                }
                if (MstTotalAssets != histTotalAssets)
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Total Assets</FieldName><OldValue>" + CaseMST.AsetTotal + "</OldValue><NewValue>" + caseHistMst.AsetTotal + "</NewValue></HistoryFields>";
                }

                if (CaseMST.AsetMisc.Trim() != caseHistMst.AsetMisc.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Misc Assets</FieldName><OldValue>" + CaseMST.AsetMisc + "</OldValue><NewValue>" + caseHistMst.AsetMisc + "</NewValue></HistoryFields>";
                }

                double MstAssetration = 0.00;
                if (!string.IsNullOrEmpty(CaseMST.AsetRatio))
                {
                    MstAssetration = Convert.ToDouble(CaseMST.AsetRatio);
                }
                double histAssetration = 0.00;
                if (!string.IsNullOrEmpty(caseHistMst.AsetRatio))
                {
                    histAssetration = Convert.ToDouble(caseHistMst.AsetRatio);
                }
                if (MstAssetration != histAssetration)
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Debt/Asset Ratio</FieldName><OldValue>" + CaseMST.AsetRatio + "</OldValue><NewValue>" + caseHistMst.AsetRatio + "</NewValue></HistoryFields>";
                }

                double MstDebtration = 0.00;
                if (!string.IsNullOrEmpty(CaseMST.DebIncmRation))
                {
                    MstDebtration = Convert.ToDouble(CaseMST.DebIncmRation);
                }
                double histDebtration = 0.00;
                if (!string.IsNullOrEmpty(caseHistMst.DebIncmRation))
                {
                    histDebtration = Convert.ToDouble(caseHistMst.DebIncmRation);
                }
                if (MstDebtration != histDebtration)
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Debt/Income Ratio</FieldName><OldValue>" + CaseMST.DebIncmRation + "</OldValue><NewValue>" + caseHistMst.DebIncmRation + "</NewValue></HistoryFields>";
                }

                double MstHHDebt = 0.00;
                if (!string.IsNullOrEmpty(CaseMST.DebtTotal))
                {
                    MstHHDebt = Convert.ToDouble(CaseMST.DebtTotal);
                }
                double histHHDebt = 0.00;
                if (!string.IsNullOrEmpty(caseHistMst.DebtTotal))
                {
                    histHHDebt = Convert.ToDouble(caseHistMst.DebtTotal);
                }
                if (MstHHDebt != histHHDebt)
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Total HH Debt</FieldName><OldValue>" + CaseMST.DebtTotal + "</OldValue><NewValue>" + caseHistMst.DebtTotal + "</NewValue></HistoryFields>";
                }

                double MstexpTotal = 0.00;
                if (!string.IsNullOrEmpty(CaseMST.ExpTotal))
                {
                    MstexpTotal = Convert.ToDouble(CaseMST.ExpTotal);
                }
                double histexpTotal = 0.00;
                if (!string.IsNullOrEmpty(caseHistMst.ExpTotal))
                {
                    histexpTotal = Convert.ToDouble(caseHistMst.ExpTotal);
                }
                if (MstexpTotal != histexpTotal)
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Total Household Expense</FieldName><OldValue>" + CaseMST.ExpTotal + "</OldValue><NewValue>" + caseHistMst.ExpTotal + "</NewValue></HistoryFields>";
                }

                double MstLiveexpences = 0.00;
                if (!string.IsNullOrEmpty(CaseMST.ExpLivexpense))
                {
                    MstLiveexpences = Convert.ToDouble(CaseMST.ExpLivexpense);
                }
                double histLiveexpences = 0.00;
                if (!string.IsNullOrEmpty(caseHistMst.ExpLivexpense))
                {
                    histLiveexpences = Convert.ToDouble(caseHistMst.ExpLivexpense);
                }

                if (MstLiveexpences != histLiveexpences)
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Total Living Expense %</FieldName><OldValue>" + CaseMST.ExpLivexpense + "</OldValue><NewValue>" + caseHistMst.ExpLivexpense + "</NewValue></HistoryFields>";
                }
                //if (CaseMST.ExpCaseWorker.Trim() != caseHistMst.ExpCaseWorker.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_EXP_CASEWORKER</FieldName><OldValue>" + CaseMST.ExpCaseWorker + "</OldValue><NewValue>" + caseHistMst.ExpCaseWorker + "</NewValue></HistoryFields>";
                //}
                if (CaseMST.Dwelling.Trim() != caseHistMst.Dwelling.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Dwelling Type</FieldName><OldValue>" + CaseMST.DwellingDesc + "</OldValue><NewValue>" + caseHistMst.DwellingDesc + "</NewValue></HistoryFields>";
                }
                if (CaseMST.HeatIncRent.Trim() != caseHistMst.HeatIncRent.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Primary Method of Paying for Heat</FieldName><OldValue>" + CaseMST.HeatIncRentDesc + "</OldValue><NewValue>" + caseHistMst.HeatIncRentDesc + "</NewValue></HistoryFields>";
                }
                if (CaseMST.Source.Trim() != caseHistMst.Source.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Primary Source of Heat </FieldName><OldValue>" + CaseMST.SourceDesc + "</OldValue><NewValue>" + caseHistMst.SourceDesc + "</NewValue></HistoryFields>";
                }
                //if (CaseMST.RollOver.Trim() != caseHistMst.RollOver.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_ROLLOVER</FieldName><OldValue>" + CaseMST.RollOver + "</OldValue><NewValue>" + caseHistMst.RollOver + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.RiskValue.Trim() != caseHistMst.RiskValue.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_RISK_VALUE</FieldName><OldValue>" + CaseMST.RiskValue + "</OldValue><NewValue>" + caseHistMst.RiskValue + "</NewValue></HistoryFields>";
                //}
                if (CaseMST.SubShouse.Trim() != caseHistMst.SubShouse.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Subsidized Housing</FieldName><OldValue>" + CaseMST.SubShouse + "</OldValue><NewValue>" + caseHistMst.SubShouse + "</NewValue></HistoryFields>";
                }
                if (CaseMST.SubStype.Trim() != caseHistMst.SubStype.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Subsidized Housing Type</FieldName><OldValue>" + CaseMST.SubsTypeDesc + "</OldValue><NewValue>" + caseHistMst.SubsTypeDesc + "</NewValue></HistoryFields>";
                }
                //if (CaseMST.VerFund.Trim() != caseHistMst.VerFund.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_VER_FUND</FieldName><OldValue>" + CaseMST.VerFund + "</OldValue><NewValue>" + caseHistMst.VerFund + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.OmbScreen.Trim() != caseHistMst.OmbScreen.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_OMB_SCREEN</FieldName><OldValue>" + CaseMST.OmbScreen + "</OldValue><NewValue>" + caseHistMst.OmbScreen + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.CbCaseManager.Trim() != caseHistMst.CbCaseManager.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_CB_CASE_MANAGER</FieldName><OldValue>" + CaseMST.CbCaseManager + "</OldValue><NewValue>" + caseHistMst.CbCaseManager + "</NewValue></HistoryFields>";
                //}


                if (CaseMST.ExpCaseWorker.Trim() != caseHistMst.ExpCaseWorker.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Verified that all Household Expenses entered</FieldName><OldValue>" + CaseMST.ExpCaseworkerDesc + "</OldValue><NewValue>" + caseHistMst.ExpCaseworkerDesc + "</NewValue></HistoryFields>";
                }


                //if (CaseMST.VerifyOthCmb.Trim() != caseHistMst.VerifyOthCmb.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_VERIFY_OTH_CMB</FieldName><OldValue>" + CaseMST.VerifyOthCmb + "</OldValue><NewValue>" + caseHistMst.VerifyOthCmb + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.SimPrint.Trim() != caseHistMst.SimPrint.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_SIM_PRINT</FieldName><OldValue>" + CaseMST.SimPrint + "</OldValue><NewValue>" + caseHistMst.SimPrint + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.SimPrintDate .Trim() != caseHistMst.SimPrintDate.Trim())
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_SIM_PRINT_DATE</FieldName><OldValue>"+ + "</OldValue><NewValue>"+ +"</NewValue></HistoryFields>";

                //if (CaseMST.CbFraud.Trim() != caseHistMst.CbFraud.Trim())
                //{
                //    boolHistory = true;
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_CB_FRAUD</FieldName><OldValue>" + CaseMST.CbFraud + "</OldValue><NewValue>" + caseHistMst.CbFraud + "</NewValue></HistoryFields>";
                //}
                //if (CaseMST.FraudDate .Trim() != caseHistMst.FraudDate.Trim())
                //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MST_FRAUD_DATE</FieldName><OldValue>"+ + "</OldValue><NewValue>"+ +"</NewValue></HistoryFields>";

                if (BaseForm.BaseCaseMstListEntity[0].PressCat.Trim() != caseHistMst.PressCat.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Press Category</FieldName><OldValue>" + caseHistMst.PressCat + "</OldValue><NewValue>" + BaseForm.BaseCaseMstListEntity[0].PressCat.Trim() + "</NewValue></HistoryFields>";
                }

                decimal MstpressTotal = 0;
                if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].PressTotal))
                {
                    MstpressTotal = Convert.ToDecimal(BaseForm.BaseCaseMstListEntity[0].PressTotal);
                }
                decimal histpresstotal = 0;
                if (!string.IsNullOrEmpty(caseHistMst.PressTotal))
                {
                    histpresstotal = Convert.ToDecimal(caseHistMst.PressTotal);
                }

                if (MstpressTotal != histpresstotal)
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Press Total</FieldName><OldValue>" + caseHistMst.PressTotal + "</OldValue><NewValue>" + BaseForm.BaseCaseMstListEntity[0].PressTotal + "</NewValue></HistoryFields>";
                }

            }


            //if (CaseSNP.FamilySeq.Trim() != casesnpHist.FamilySeq.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_FAMILY_SEQ</FieldName><OldValue>" + CaseSNP + "</OldValue><NewValue>" + casesnpHist + "</NewValue></HistoryFields>";
            //}
            if (CaseSNP.MemberCode.Trim() != casesnpHist.MemberCode.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Relation</FieldName><OldValue>" + CaseSNP.RelationDesc + "</OldValue><NewValue>" + casesnpHist.RelationDesc + "</NewValue></HistoryFields>";
            }
            //if (CaseSNP.ClientId.Trim() != casesnpHist.ClientId.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_CLIENT_ID</FieldName><OldValue>" + CaseSNP + "</OldValue><NewValue>" + casesnpHist + "</NewValue></HistoryFields>";
            //}

            if (CaseSNP.Ssno.Trim() != casesnpHist.Ssno.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SSN#</FieldName><OldValue>" + LookupDataAccess.GetPhoneSsnNoFormat(CaseSNP.Ssno) + "</OldValue><NewValue>" + LookupDataAccess.GetPhoneSsnNoFormat(casesnpHist.Ssno) + "</NewValue></HistoryFields>";
            }

            if (CaseSNP.SsBic.Trim() != casesnpHist.SsBic.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Bic</FieldName><OldValue>" + CaseSNP.SsBic + "</OldValue><NewValue>" + casesnpHist.SsBic + "</NewValue></HistoryFields>";
            }
            if (CaseSNP.NameixLast.Trim().ToUpper() != casesnpHist.NameixLast.Trim().ToUpper())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Last Name</FieldName><OldValue>" + CaseSNP.NameixLast + "</OldValue><NewValue>" + casesnpHist.NameixLast + "</NewValue></HistoryFields>";
            }
            if (CaseSNP.NameixFi.Trim().ToUpper() != casesnpHist.NameixFi.Trim().ToUpper())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>First Name</FieldName><OldValue>" + CaseSNP.NameixFi + "</OldValue><NewValue>" + casesnpHist.NameixFi + "</NewValue></HistoryFields>";
            }
            if (CaseSNP.NameixMi.Trim().ToUpper() != casesnpHist.NameixMi.Trim().ToUpper())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>MI</FieldName><OldValue>" + CaseSNP.NameixMi + "</OldValue><NewValue>" + casesnpHist.NameixMi + "</NewValue></HistoryFields>";
            }

            //if (CaseSNP.AltLName.Trim() != casesnpHist.AltLName.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_ALT_LNAME</FieldName><OldValue>" + CaseSNP.AltLName + "</OldValue><NewValue>" + casesnpHist.AltLName + "</NewValue></HistoryFields>";
            //}
            //if (CaseSNP.AltFi.Trim() != casesnpHist.AltFi.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_ALT_FI</FieldName><OldValue>" + CaseSNP + "</OldValue><NewValue>" + casesnpHist + "</NewValue></HistoryFields>";
            //}
            if (CaseSNP.Alias.Trim() != casesnpHist.Alias.Trim())
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Alias</FieldName><OldValue>" + CaseSNP.Alias + "</OldValue><NewValue>" + casesnpHist.Alias + "</NewValue></HistoryFields>";
            if (CaseSNP.Status.Trim() != casesnpHist.Status.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Active</FieldName><OldValue>" + (CaseSNP.Status == "A" ? "Active" : "Inactive") + "</OldValue><NewValue>" + (casesnpHist.Status == "A" ? "Active" : "Inactive") + "</NewValue></HistoryFields>";
            }
            if (CaseSNP.Sex.Trim() != casesnpHist.Sex.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Gender</FieldName><OldValue>" + CaseSNP.GenderDesc + "</OldValue><NewValue>" + casesnpHist.GenderDesc + "</NewValue></HistoryFields>";
            }


            if (CaseSNP.DobNa.Trim() != casesnpHist.DobNa.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>NA</FieldName><OldValue>" + (CaseSNP.DobNa == "1" ? "Active" : "Inactive") + "</OldValue><NewValue>" + (casesnpHist.DobNa == "1" ? "Active" : "Inactive") + "</NewValue></HistoryFields>";
                if (checkNA.Checked == true)
                {

                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Date of Birth</FieldName><OldValue>" + CaseSNP.AltBdate + "</OldValue><NewValue>" + string.Empty + "</NewValue></HistoryFields>";
                }
                else
                {
                    if (checkNA.Checked == false)
                    {
                        if (LookupDataAccess.Getdate(CaseSNP.AltBdate) != LookupDataAccess.Getdate(casesnpHist.AltBdate))
                        {
                            boolHistory = true;
                            strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Date of Birth</FieldName><OldValue>" + CaseSNP.AltBdate + "</OldValue><NewValue>" + casesnpHist.AltBdate + "</NewValue></HistoryFields>";
                        }
                    }

                    if (checkNA.Checked == false)
                    {
                        if (CaseSNP.Age.Trim() != casesnpHist.Age.Trim())
                        {
                            boolHistory = true; strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Age</FieldName><OldValue>" + CaseSNP.Age + "</OldValue><NewValue>" + casesnpHist.Age + "</NewValue></HistoryFields>";
                        }
                    }
                }

            }
            else
            {

                if (checkNA.Checked == false)
                {
                    //DateTime dtaltDate = DateTime.Now.Date;
                    //if (!string.IsNullOrEmpty(CaseSNP.AltBdate))
                    //{
                    //    dtaltDate = Convert.ToDateTime(CaseSNP.AltBdate).Date;
                    //}
                    //DateTime dtHistaltDate = DateTime.Now.Date;
                    //if (!string.IsNullOrEmpty(casesnpHist.AltBdate))
                    //{
                    //    dtHistaltDate = Convert.ToDateTime(casesnpHist.AltBdate).Date;
                    //}
                    if (LookupDataAccess.Getdate(CaseSNP.AltBdate) != LookupDataAccess.Getdate(casesnpHist.AltBdate))
                    {
                        boolHistory = true;
                        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Date of Birth</FieldName><OldValue>" + CaseSNP.AltBdate + "</OldValue><NewValue>" + casesnpHist.AltBdate + "</NewValue></HistoryFields>";
                    }
                }

                if (checkNA.Checked == false)
                {
                    if (CaseSNP.Age.Trim() != casesnpHist.Age.Trim())
                    {
                        boolHistory = true; strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Age</FieldName><OldValue>" + CaseSNP.Age + "</OldValue><NewValue>" + casesnpHist.Age + "</NewValue></HistoryFields>";
                    }
                }

            }

            if (CaseSNP.Ethnic.Trim() != casesnpHist.Ethnic.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Ethnicity</FieldName><OldValue>" + CaseSNP.EthnicityDesc + "</OldValue><NewValue>" + casesnpHist.EthnicityDesc + "</NewValue></HistoryFields>";
            }
            if (CaseSNP.Education.Trim() != casesnpHist.Education.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Education</FieldName><OldValue>" + CaseSNP.EducationDesc + "</OldValue><NewValue>" + casesnpHist.EducationDesc + "</NewValue></HistoryFields>";
            }

            //if (CaseSNP.IncomeBasis.Trim() != casesnpHist.IncomeBasis.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_INCOME_BASIS</FieldName><OldValue>" + CaseSNP + "</OldValue><NewValue>" + casesnpHist + "</NewValue></HistoryFields>";
            //}
            if ((CaseSNP.HealthIns.Trim() != casesnpHist.HealthIns.Trim()) && (CaseSNP.HealthIns.Trim() != "0"))
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Health Insurance</FieldName><OldValue>" + CaseSNP.HealthInsuranceDesc + "</OldValue><NewValue>" + casesnpHist.HealthInsuranceDesc + "</NewValue></HistoryFields>";
            }

            if (CaseSNP.Vet.Trim() != casesnpHist.Vet.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Veteran Code</FieldName><OldValue>" + CaseSNP.VeterncodeDesc + "</OldValue><NewValue>" + casesnpHist.VeterncodeDesc + "</NewValue></HistoryFields>";
            }

            if (CaseSNP.Disable.Trim() != casesnpHist.Disable.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Disabled</FieldName><OldValue>" + CaseSNP.DisabledDesc + "</OldValue><NewValue>" + casesnpHist.DisabledDesc + "</NewValue></HistoryFields>";
            }

            if (CaseSNP.FootStamps.Trim() != casesnpHist.FootStamps.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Food Stamps</FieldName><OldValue>" + CaseSNP.FoodStampsDesc + "</OldValue><NewValue>" + casesnpHist.FoodStampsDesc + "</NewValue></HistoryFields>";
            }

            if (CaseSNP.Farmer.Trim() != casesnpHist.Farmer.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Farmer</FieldName><OldValue>" + CaseSNP.farmerDesc + "</OldValue><NewValue>" + casesnpHist.farmerDesc + "</NewValue></HistoryFields>";
            }

            //if (CaseSNP.ApplDate.Trim() != casesnpHist.ApplDate.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_APPL_DATE</FieldName><OldValue>" + CaseSNP.ApplDate  + "</OldValue><NewValue>" + casesnpHist.ApplDate + "</NewValue></HistoryFields>";
            //}

            //if (CaseSNP.ApplTime.Trim() != casesnpHist.ApplTime.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_APPL_TIME</FieldName><OldValue>" + CaseSNP + "</OldValue><NewValue>" + casesnpHist + "</NewValue></HistoryFields>";
            //}
            //if (CaseSNP.Ampm.Trim() != casesnpHist.Ampm.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_AMPM</FieldName><OldValue>" + CaseSNP + "</OldValue><NewValue>" + casesnpHist + "</NewValue></HistoryFields>";
            //}
            //if (CaseSNP.InitialDate.Trim() != casesnpHist.InitialDate.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_INTAKE_DATE</FieldName><OldValue>" + CaseSNP.InitialDate + "</OldValue><NewValue>" + casesnpHist.InitialDate  + "</NewValue></HistoryFields>";
            //}
            //if (CaseSNP.Site.Trim() != casesnpHist.Site.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_SITE</FieldName><OldValue>" + CaseSNP.Site + "</OldValue><NewValue>" + casesnpHist.Site + "</NewValue></HistoryFields>";
            //}
            //SNP_TOT_INCOME
            //if (CaseSNP.TotIncome.Trim()!= casesnpHist)
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_TOT_INCOME</FieldName><OldValue>"+ CaseSNP+ "</OldValue><NewValue>"+ casesnpHist +"</NewValue></HistoryFields>";
            //SNP_PROG_INCOME
            //if (CaseSNP.ProgIncome.Trim()!= casesnpHist)
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_PROG_INCOME</FieldName><OldValue>"+ CaseSNP+ "</OldValue><NewValue>"+ casesnpHist +"</NewValue></HistoryFields>";

            //if (CaseSNP.ClaimSsno.Trim() != casesnpHist.ClaimSsno.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_CLAIM_SSNO</FieldName><OldValue>" + CaseSNP.Ssno + "</OldValue><NewValue>" + casesnpHist + "</NewValue></HistoryFields>";
            //}
            //if (CaseSNP.ClaimSsbic.Trim() != casesnpHist.ClaimSsbic.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_CLAIM_SS_BIC</FieldName><OldValue>" + CaseSNP + "</OldValue><NewValue>" + casesnpHist + "</NewValue></HistoryFields>";
            //}
            //if (CaseSNP.Wagem.Trim() != casesnpHist.Wagem.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_WAGEM</FieldName><OldValue>" + CaseSNP + "</OldValue><NewValue>" + casesnpHist + "</NewValue></HistoryFields>";
            //}
            if (CaseSNP.Wic.Trim() != casesnpHist.Wic.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>WIC</FieldName><OldValue>" + CaseSNP.WicDesc + "</OldValue><NewValue>" + casesnpHist.WicDesc + "</NewValue></HistoryFields>";
            }

            //if (CaseSNP.Student.Trim() != casesnpHist.Student.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_STUDENT</FieldName><OldValue>" + CaseSNP.Student + "</OldValue><NewValue>" + casesnpHist + "</NewValue></HistoryFields>";
            //}
            if (CaseSNP.Resident.Trim() != casesnpHist.Resident.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Resident</FieldName><OldValue>" + CaseSNP.ResidentDesc + "</OldValue><NewValue>" + casesnpHist.ResidentDesc + "</NewValue></HistoryFields>";
            }
            if (casesnpHist.Sex.Equals("F"))
            {
                if (CaseSNP.Pregnant.Trim() != casesnpHist.Pregnant.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Are You Pregnant</FieldName><OldValue>" + CaseSNP.AreYouPregantDesc + "</OldValue><NewValue>" + casesnpHist.AreYouPregantDesc + "</NewValue></HistoryFields>";
                }
            }

            if (CaseSNP.MaritalStatus.Trim() != casesnpHist.MaritalStatus.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Marital Status</FieldName><OldValue>" + CaseSNP.MartialStatusDesc + "</OldValue><NewValue>" + casesnpHist.MartialStatusDesc + "</NewValue></HistoryFields>";
            }

            if (CaseSNP.SchoolDistrict.Trim() != casesnpHist.SchoolDistrict.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>School</FieldName><OldValue>" + CaseSNP.SchoolDesc + "</OldValue><NewValue>" + casesnpHist.SchoolDesc + "</NewValue></HistoryFields>";
            }
            if (CaseSNP.AlienRegNo.Trim() != casesnpHist.AlienRegNo.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Alien Reg No</FieldName><OldValue>" + CaseSNP.AlienRegNo + "</OldValue><NewValue>" + casesnpHist.AlienRegNo + "</NewValue></HistoryFields>";
            }
            if (CaseSNP.LegalTowork.Trim() != casesnpHist.LegalTowork.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Legal to Work</FieldName><OldValue>" + CaseSNP.LegaltoworkDesc + "</OldValue><NewValue>" + casesnpHist.LegaltoworkDesc + "</NewValue></HistoryFields>";
            }
            if (LookupDataAccess.Getdate(CaseSNP.ExpireWorkDate.Trim()) != LookupDataAccess.Getdate(casesnpHist.ExpireWorkDate.Trim()))
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Expiration date</FieldName><OldValue>" + CaseSNP.ExpireWorkDate + "</OldValue><NewValue>" + casesnpHist.ExpireWorkDate + "</NewValue></HistoryFields>";
            }

            if (CaseSNP.Employed.Trim() != casesnpHist.Employed.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Presently employed</FieldName><OldValue>" + CaseSNP.PresentEmployDesc + "</OldValue><NewValue>" + casesnpHist.PresentEmployDesc + "</NewValue></HistoryFields>";
            }
            if (CaseSNP.LastWorkDate.Trim() != casesnpHist.LastWorkDate.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Last Date Worked</FieldName><OldValue>" + CaseSNP.LastWorkDate + "</OldValue><NewValue>" + casesnpHist.LastWorkDate + "</NewValue></HistoryFields>";
            }
            if (CaseSNP.WorkLimit.Trim() != casesnpHist.WorkLimit.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Any work limitations</FieldName><OldValue>" + CaseSNP.AnyworklimitationDesc + "</OldValue><NewValue>" + casesnpHist.AnyworklimitationDesc + "</NewValue></HistoryFields>";
            }
            if (CaseSNP.ExplainWorkLimit.Trim() != casesnpHist.ExplainWorkLimit.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>If yes, explain</FieldName><OldValue>" + CaseSNP.ExplainWorkLimit + "</OldValue><NewValue>" + casesnpHist.ExplainWorkLimit + "</NewValue></HistoryFields>";
            }
            if (CaseSNP.NumberOfcjobs.Trim() != casesnpHist.NumberOfcjobs.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName># of jobs that you currently have?</FieldName><OldValue>" + CaseSNP.NumberOfcjobs + "</OldValue><NewValue>" + casesnpHist.NumberOfcjobs + "</NewValue></HistoryFields>";
            }
            if (CaseSNP.NumberofLvjobs.Trim() != casesnpHist.NumberofLvjobs.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName># of jobs since your last visit?</FieldName><OldValue>" + CaseSNP.NumberofLvjobs + "</OldValue><NewValue>" + casesnpHist.NumberofLvjobs + "</NewValue></HistoryFields>";
            }
            if (CaseSNP.FullTimeHours.Trim() != casesnpHist.FullTimeHours.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Full Time Hours(per week)</FieldName><OldValue>" + CaseSNP.FullTimeHours + "</OldValue><NewValue>" + casesnpHist.FullTimeHours + "</NewValue></HistoryFields>";
            }
            if (CaseSNP.PartTimeHours.Trim() != casesnpHist.PartTimeHours.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Part Time Hours(per week)</FieldName><OldValue>" + CaseSNP.PartTimeHours + "</OldValue><NewValue>" + casesnpHist.PartTimeHours + "</NewValue></HistoryFields>";
            }
            if (CaseSNP.SeasonalEmploy.Trim() != casesnpHist.SeasonalEmploy.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Seasonal Employment Only</FieldName><OldValue>" + CaseSNP.SeasonEmployedDesc + "</OldValue><NewValue>" + casesnpHist.SeasonEmployedDesc + "</NewValue></HistoryFields>";
            }
            if ((CaseSNP.IstShift.Trim() == "Y" ? "Y" : "N") != casesnpHist.IstShift.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>1st</FieldName><OldValue>" + CaseSNP.IstShift + "</OldValue><NewValue>" + casesnpHist.IstShift + "</NewValue></HistoryFields>";
            }
            if ((CaseSNP.IIndShift.Trim() == "Y" ? "Y" : "N") != casesnpHist.IIndShift.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>2nd</FieldName><OldValue>" + CaseSNP.IIndShift + "</OldValue><NewValue>" + casesnpHist.IIndShift + "</NewValue></HistoryFields>";
            }

            if ((CaseSNP.IIIrdShift.Trim() == "Y" ? "Y" : "N") != casesnpHist.IIIrdShift.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>3rd</FieldName><OldValue>" + CaseSNP.IIIrdShift + "</OldValue><NewValue>" + casesnpHist.IIIrdShift + "</NewValue></HistoryFields>";
            }
            if ((CaseSNP.RShift.Trim() == "Y" ? "Y" : "N") != casesnpHist.RShift.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Rotating</FieldName><OldValue>" + CaseSNP.RShift + "</OldValue><NewValue>" + casesnpHist.RShift + "</NewValue></HistoryFields>";
            }
            if (CaseSNP.EmployerName.Trim() != casesnpHist.EmployerName.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Employer</FieldName><OldValue>" + CaseSNP.EmployerName + "</OldValue><NewValue>" + casesnpHist.EmployerName + "</NewValue></HistoryFields>";
            }
            if (CaseSNP.EmployerStreet.Trim() != casesnpHist.EmployerStreet.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Street</FieldName><OldValue>" + CaseSNP.EmployerStreet + "</OldValue><NewValue>" + casesnpHist.EmployerStreet + "</NewValue></HistoryFields>";
            }
            if (CaseSNP.EmployerCity.Trim() != casesnpHist.EmployerCity.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>City/State/Zip</FieldName><OldValue>" + CaseSNP.EmployerCity + "</OldValue><NewValue>" + casesnpHist.EmployerCity + "</NewValue></HistoryFields>";
            }
            if ((CaseSNP.JobTitle.Trim() != casesnpHist.JobTitle.Trim()) && (CaseSNP.JobTitle.Trim() != "0"))
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Job Title</FieldName><OldValue>" + CaseSNP.JobTitle + "</OldValue><NewValue>" + casesnpHist.JobTitle + "</NewValue></HistoryFields>";
            }
            if ((CaseSNP.JobCategory.Trim() != casesnpHist.JobCategory.Trim()) && (CaseSNP.JobCategory.Trim() != "0"))
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Job Category</FieldName><OldValue>" + CaseSNP.JobCategory + "</OldValue><NewValue>" + casesnpHist.JobCategory + "</NewValue></HistoryFields>";
            }
            if (CaseSNP.HourlyWage.Trim() != casesnpHist.HourlyWage.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Hourly Wage $</FieldName><OldValue>" + CaseSNP.HourlyWage + "</OldValue><NewValue>" + casesnpHist.HourlyWage + "</NewValue></HistoryFields>";
            }
            if (CaseSNP.PayFrequency.Trim() != casesnpHist.PayFrequency.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Pay Frequency</FieldName><OldValue>" + CaseSNP.PayFrequency + "</OldValue><NewValue>" + casesnpHist.PayFrequency + "</NewValue></HistoryFields>";
            }
            if (CaseSNP.HireDate.Trim() != casesnpHist.HireDate.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Hire Date</FieldName><OldValue>" + CaseSNP.HireDate + "</OldValue><NewValue>" + casesnpHist.HireDate + "</NewValue></HistoryFields>";
            }
            //if (CaseSNP.Transerv.Trim() != casesnpHist.Transerv.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_TRANSERV</FieldName><OldValue>" + CaseSNP.Transerv + "</OldValue><NewValue>" + casesnpHist.Transerv + "</NewValue></HistoryFields>";
            //}
            if (CaseSNP.Relitran.Trim() != casesnpHist.Relitran.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Reliable Transport</FieldName><OldValue>" + CaseSNP.ReliableTransportDesc + "</OldValue><NewValue>" + casesnpHist.ReliableTransportDesc + "</NewValue></HistoryFields>";
            }

            if (CaseSNP.Drvlic.Trim() != casesnpHist.Drvlic.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Drivers License</FieldName><OldValue>" + CaseSNP.DriverLicenceDesc + "</OldValue><NewValue>" + casesnpHist.DriverLicenceDesc + "</NewValue></HistoryFields>";
            }

            if (CaseSNP.Race.Trim() != casesnpHist.Race.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Race</FieldName><OldValue>" + CaseSNP.RaceDesc + "</OldValue><NewValue>" + casesnpHist.RaceDesc + "</NewValue></HistoryFields>";
            }

            if (CaseSNP.EmplPhone.Trim() != casesnpHist.EmplPhone.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Phone</FieldName><OldValue>" + CaseSNP.EmplPhone + "</OldValue><NewValue>" + casesnpHist.EmplPhone + "</NewValue></HistoryFields>";
            }
            //if (CaseSNP.EmplExch.Trim()!= casesnpHist)
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_EMPL_EXCH</FieldName><OldValue>"+ CaseSNP+ "</OldValue><NewValue>"+ casesnpHist +"</NewValue></HistoryFields>"; CaseSNP.EmplExch));

            //if (CaseSNP.EmplTel.Trim()!= casesnpHist)
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_EMPL_TEL</FieldName><OldValue>"+ CaseSNP+ "</OldValue><NewValue>"+ casesnpHist +"</NewValue></HistoryFields>"; CaseSNP.EmplTel));

            if (CaseSNP.EmplExt.Trim() != casesnpHist.EmplExt.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>EXT</FieldName><OldValue>" + CaseSNP.EmplExt + "</OldValue><NewValue>" + casesnpHist.EmplExt + "</NewValue></HistoryFields>";
            }
            if (cmbSSNReason.Visible == true)
            {
                if (CaseSNP.SsnReason.Trim() != casesnpHist.SsnReason.Trim())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SSN # Reason</FieldName><OldValue>" + CaseSNP.SsnReasonDesc + "</OldValue><NewValue>" + casesnpHist.SsnReasonDesc + "</NewValue></HistoryFields>";
                }
            }
            if (CaseSNP.Exclude.Trim() != casesnpHist.Exclude.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Exclude Member</FieldName><OldValue>" + (CaseSNP.Exclude == "Y" ? "Yes" : "NO") + "</OldValue><NewValue>" + (casesnpHist.Exclude == "Y" ? "Yes" : "NO") + "</NewValue></HistoryFields>";
            }
            //if (CaseSNP.Blind.Trim() != casesnpHist.Blind.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_BLIND</FieldName><OldValue>" + CaseSNP.Blind + "</OldValue><NewValue>" + casesnpHist.Blind + "</NewValue></HistoryFields>";
            //}
            //if (CaseSNP.AbleTowork.Trim() != casesnpHist.AbleTowork.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_ABLE_TO_WORK</FieldName><OldValue>" + CaseSNP.AbleTowork + "</OldValue><NewValue>" + casesnpHist.AbleTowork + "</NewValue></HistoryFields>";
            //}
            //if (CaseSNP.RecMedicare.Trim() != casesnpHist.RecMedicare.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_REC_MEDICARE</FieldName><OldValue>" + CaseSNP.RecMedicare + "</OldValue><NewValue>" + casesnpHist + "</NewValue></HistoryFields>";
            //}
            //if (CaseSNP.PurchaseFood.Trim() != casesnpHist.PurchaseFood.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_PURCHASE_FOOD</FieldName><OldValue>" + CaseSNP + "</OldValue><NewValue>" + casesnpHist + "</NewValue></HistoryFields>";
            //}
            //if (CaseSNP.VechicleValue.Trim() != casesnpHist.VechicleValue.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_VEHICLE_VALUE</FieldName><OldValue>" + CaseSNP + "</OldValue><NewValue>" + casesnpHist + "</NewValue></HistoryFields>";
            //}
            //if (CaseSNP.OtherVehicleValue.Trim() != casesnpHist.OtherVehicleValue.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_OTHER_VEHICLE_VALUE</FieldName><OldValue>" + CaseSNP + "</OldValue><NewValue>" + casesnpHist + "</NewValue></HistoryFields>";
            //}
            //if (CaseSNP.OtherAssetValue.Trim() != casesnpHist.OtherAssetValue.Trim())
            //{
            //    boolHistory = true;
            //    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>SNP_OTHER_ASSET_VALUE</FieldName><OldValue>" + CaseSNP + "</OldValue><NewValue>" + casesnpHist + "</NewValue></HistoryFields>";
            //}
            strHistoryDetails = strHistoryDetails + "</XmlHistory>";
            if (boolHistory)
            {
                CaseHistEntity caseHistEntity = new CaseHistEntity();
                caseHistEntity.HistTblName = "CASEMST";
                caseHistEntity.HistScreen = "CASE2001";
                caseHistEntity.HistSubScr = "Intake";
                caseHistEntity.HistTblKey = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo + CaseSNP.FamilySeq;
                caseHistEntity.LstcOperator = BaseForm.UserID;
                caseHistEntity.HistChanges = strHistoryDetails;
                _model.CaseMstData.InsertCaseHist(caseHistEntity);

                if (boolAddressHistory)
                {
                    CaseMST.Mode = "Address";
                    CaseMST.LstcOperator4 = BaseForm.UserID;
                    _model.CaseMstData.InsertUpdateCaseMst(CaseMST, out strApplNo, out strClientIdOut, out strFamilyIdOut, out strSSNNOOut, out strErrorMsg);
                }


            }

        }

        private void dtElastDateWorked_ValueChanged(object sender, EventArgs e)
        {
            if (dtElastDateWorked.Value > DateTime.Now)
            {
                CommonFunctions.MessageBoxDisplay("Last Date Worked should not be in future");
                dtElastDateWorked.ValueChanged -= new EventHandler(dtElastDateWorked_ValueChanged);
                dtElastDateWorked.Text = DateTime.Now.ToShortDateString();
                dtElastDateWorked.Checked = false;
                dtElastDateWorked.ValueChanged += new EventHandler(dtElastDateWorked_ValueChanged);

            }
        }

        //private void btnCancel_LostFocus(object sender, EventArgs e)
        //{
        //    switch (tabControl1.SelectedIndex)
        //    {
        //        case 0:
        //           // btnCancel.Focus();
        //            btnCancel.TabIndex = 0;
        //            txtFirstName.Focus();
        //            mskSSN.Focus();
        //            break;
        //        case 1:
        //          //  btnCancel.Focus();
        //            btnCancel.TabIndex = 0;
        //            cmbStaff.Focus();
        //            break;
        //        case 2:
        //         //   btnCancel.Focus();
        //            btnCancel.TabIndex = 0;
        //            cmbVerifiedStaff.Focus();
        //            break;
        //        case 3:
        //          //  btnCancel.Focus();
        //            btnCancel.TabIndex = 0;
        //            cmbEpresenteEmploy.Focus();
        //            break;
        //        default:
        //            break;
        //    }
        //}

        public string GetNew_App_For_Mainmenu()
        {
            return strApplNo;
        }

        private void cmbDwelling_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDwelling.Items.Count > 0)
                {
                    string strCmbDwellin = ((ListItem)cmbDwelling.SelectedItem).Value == null ? string.Empty : ((ListItem)cmbDwelling.SelectedItem).Value.ToString();
                    if (!string.IsNullOrEmpty(strCmbDwellin))
                    {
                        if (((ListItem)cmbDwelling.SelectedItem).Value.ToString() != "0")
                            if (((ListItem)cmbDwelling.SelectedItem).ID.ToString() != "Y")
                                MessageBox.Show(Consts.AgyTab.DWELLINGTYPEMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {


            }

        }

        private void cmbPrimarySourceoHeat_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbPrimarySourceoHeat.Items.Count > 0)
                {
                    string strcmbprimarysourceHeat = ((ListItem)cmbPrimarySourceoHeat.SelectedItem).Value == null ? string.Empty : ((ListItem)cmbPrimarySourceoHeat.SelectedItem).Value.ToString();
                    if (!string.IsNullOrEmpty(strcmbprimarysourceHeat))
                    {
                        if (((ListItem)cmbPrimarySourceoHeat.SelectedItem).Value.ToString() != "0")
                            if (((ListItem)cmbPrimarySourceoHeat.SelectedItem).ID.ToString() != "Y")
                                MessageBox.Show(Consts.AgyTab.PrimarySohMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
            }
            catch (Exception ex)
            {


            }

        }

        private void cmbPMOPfHeat_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnLiheapques.Visible = false;
                if (((ListItem)cmbPMOPfHeat.SelectedItem).Value.ToString() != "0")
                {
                    if (((ListItem)cmbPMOPfHeat.SelectedItem).ID.ToString() != "Y")
                        MessageBox.Show(Consts.AgyTab.PrimaryMopfhMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (((ListItem)cmbPMOPfHeat.SelectedItem).Value.ToString() == "1")
                    {
                        if (propAgencyControlDetails.State.ToUpper() == "CT" && BaseForm.BusinessModuleID.ToString() == "08")
                        {
                            btnLiheapques.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }

        }

        private void cmbSubsidized_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (((ListItem)cmbSubsidized.SelectedItem).Value.ToString() != "0")
                {

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void chkSubsidized_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSubsidized.Checked == true)
            {
                lblSubsidizedReq.Visible = true;
                cmbSubsidized.Enabled = true;
            }
            else
            {
                lblSubsidizedReq.Visible = false;
                cmbSubsidized.Enabled = false;
                cmbSubsidized.SelectedIndex = 0;
            }
        }

        private void btnLandlordInfo_Click(object sender, EventArgs e)
        {
            DiffMailForm diffMailForm = new DiffMailForm(BaseForm, CaseMST.ApplAgency, CaseMST.ApplDept, CaseMST.ApplProgram, CaseMST.ApplYr, CaseMST.ApplNo, Privileges, Mode, "Landlord", propCaseDiffLLDetails, FLDCNTLHieEntity);
            if (Mode.Equals(Consts.Common.Add))
            {
                diffMailForm.FormClosed += new FormClosedEventHandler(diffMailForm_FormClosed);
            }

            diffMailForm.StartPosition = FormStartPosition.CenterScreen;
            diffMailForm.ShowDialog();
        }

        void diffMailForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Mode.Equals(Consts.Common.Add))
            {
                DiffMailForm diffmailform = sender as DiffMailForm;

                if (diffmailform.DialogResult == DialogResult.OK)
                {
                    propCaseDiffLLDetails = diffmailform.caseDiffLLData;
                }
            }
        }

        void diffMailForm_MailingFormClosed(object sender, FormClosedEventArgs e)
        {
            if (Mode.Equals(Consts.Common.Add))
            {
                DiffMailForm diffmailform = sender as DiffMailForm;

                if (diffmailform.DialogResult == DialogResult.OK)
                {
                    propCaseDiffMailDetails = diffmailform.caseDiffMailAddressData;
                }
            }
        }




        private void btnLiheapques_Click(object sender, EventArgs e)
        {
            LiheapPerfQuestions liheappform = new LiheapPerfQuestions(BaseForm, CaseMST, Mode, Privileges, ((ListItem)cmbPrimarySourceoHeat.SelectedItem).Value.ToString());
            liheappform.FormClosed += new FormClosedEventHandler(liheappform_FormClosed);
            liheappform.StartPosition = FormStartPosition.CenterScreen;
            liheappform.ShowDialog();
        }

        void liheappform_FormClosed(object sender, FormClosedEventArgs e)
        {
            LiheapPerfQuestions liheappform = sender as LiheapPerfQuestions;
            if (liheappform.DialogResult == DialogResult.OK)
            {
                CaseMST = liheappform.propCaseMst;
            }
        }

        private bool RequireLihpQues(string strFilter)
        {
            bool boollihpquesvalidation = true;

            if (CaseMST != null)
            {
                if (BaseForm.BaseYear == "2015")
                {
                    if (strFilter != "04")
                    {
                        if (CaseMST.LPM0001 == string.Empty)
                        {
                            boollihpquesvalidation = false;
                            goto validation;
                        }
                    }
                    else
                    {
                        CaseMST.LPM0001 = string.Empty;
                    }
                    if (strFilter != "04")
                    {
                        if (CaseMST.LPM0002 == string.Empty)
                        {

                            boollihpquesvalidation = false;
                            goto validation;
                        }
                    }
                    else
                    {
                        CaseMST.LPM0002 = string.Empty;
                    }
                    if (strFilter != "04")
                    {
                        if (CaseMST.LPM0003 == string.Empty)
                        {

                            boollihpquesvalidation = false;
                        }
                    }
                    else
                    {
                        CaseMST.LPM0003 = string.Empty;
                    }
                    if (CaseMST.LPM0004 == string.Empty)
                    {
                        boollihpquesvalidation = false;
                        goto validation;
                    }
                    if (strFilter != "02" && strFilter != "04")
                    {
                        if (CaseMST.LPM0005 == string.Empty)
                        {

                            boollihpquesvalidation = false;
                            goto validation;
                        }
                    }
                    else
                    {
                        CaseMST.LPM0005 = string.Empty;
                    }
                    if (strFilter != "02" && strFilter != "04")
                    {
                        if (CaseMST.LPM0006 == string.Empty)
                        {

                            boollihpquesvalidation = false;
                            goto validation;
                        }
                    }
                    else
                    {
                        CaseMST.LPM0006 = string.Empty;
                    }
                    if (strFilter == "02" || strFilter == "04")
                    {
                        if (CaseMST.LPM0007 == string.Empty)
                        {

                            boollihpquesvalidation = false;
                            goto validation;
                        }
                    }
                    else
                    {
                        CaseMST.LPM0007 = string.Empty;
                    }
                    if (strFilter == "02" || strFilter == "04")
                    {
                        if (CaseMST.LPM0008 == string.Empty)
                        {

                            boollihpquesvalidation = false;
                            goto validation;
                        }
                    }
                    else
                    {
                        CaseMST.LPM0008 = string.Empty;
                    }
                    if (strFilter == "02" || strFilter == "04")
                    {
                        if (CaseMST.LPM0009 == string.Empty)
                        {

                            boollihpquesvalidation = false;
                            goto validation;

                        }
                    }
                    else
                    {
                        CaseMST.LPM0009 = string.Empty;
                    }
                    if (strFilter == "02" || strFilter == "04")
                    {
                        if (CaseMST.LPM0010 == string.Empty)
                        {

                            boollihpquesvalidation = false;
                            goto validation;
                        }
                    }
                    else
                    {
                        CaseMST.LPM0010 = string.Empty;
                    }
                    if (strFilter == "02" || strFilter == "04")
                    {
                        if (CaseMST.LPM0011 == string.Empty)
                        {

                            boollihpquesvalidation = false;
                            goto validation;
                        }
                    }
                    else
                    {
                        CaseMST.LPM0011 = string.Empty;
                    }

                }
                //Commented by Sudheer on 08/04/2020
                //else if ((BaseForm.BaseYear == "2016" || BaseForm.BaseYear == "2017" || BaseForm.BaseYear == "2018" || BaseForm.BaseYear == "2019" || BaseForm.BaseYear == "2020") && (strFilter == "02" || strFilter == "04"))
                //Added by Sudheer on 08/04/2020

                if (CaseMST.LPM0012 == string.Empty)
                {
                    boollihpquesvalidation = false;
                    goto validation;
                }
                if (CaseMST.LPM0012 == "Y")
                {
                    if (CaseMST.LPM0013 == string.Empty)
                    {

                        boollihpquesvalidation = false;
                        goto validation;
                    }
                }
                if (CaseMST.LPM0013 == "Y")
                {
                    if (CaseMST.LPM0014 == string.Empty)
                    {

                        boollihpquesvalidation = false;
                        goto validation;
                    }
                }
                if (CaseMST.LPM0015 == string.Empty)
                {
                    boollihpquesvalidation = false;
                    goto validation;

                }
                if (CaseMST.LPM0015 == "Y")
                {
                    if (CaseMST.LPM0016 == string.Empty)
                    {

                        boollihpquesvalidation = false;
                        goto validation;
                    }

                }
                if (CaseMST.LPM0016 == "Y")
                {
                    if (CaseMST.LPM0017 == string.Empty)
                    {
                        boollihpquesvalidation = false;
                        goto validation;
                    }
                }
                else if (strFilter == "02" || strFilter == "04")
                {

                    if (CaseMST.LPM0001 == string.Empty)
                    {
                        boollihpquesvalidation = false;
                        goto validation;
                    }

                    if (CaseMST.LPM0002 == string.Empty)
                    {

                        boollihpquesvalidation = false;
                        goto validation;
                    }

                    if (CaseMST.LPM0002 == "Y")
                    {
                        if (CaseMST.LPM0003 == string.Empty)
                        {

                            boollihpquesvalidation = false;
                        }
                    }
                    if (strFilter == "02")
                    {
                        if (CaseMST.LPM0004 == string.Empty)
                        {
                            boollihpquesvalidation = false;
                            goto validation;
                        }
                    }
                    if (strFilter == "02")
                    {
                        if (CaseMST.LPM0001 == "Y" && CaseMST.LPM0004 == "N")
                        {
                            if (CaseMST.LPM0005 == string.Empty)
                            {

                                boollihpquesvalidation = false;
                                goto validation;
                            }
                        }
                    }
                    if (CaseMST.LPM0006 == string.Empty)
                    {

                        boollihpquesvalidation = false;
                        goto validation;
                    }
                    if (CaseMST.LPM0006 == "Y")
                    {
                        if (CaseMST.LPM0007 == string.Empty)
                        {

                            boollihpquesvalidation = false;
                            goto validation;
                        }
                    }
                    if (CaseMST.LPM0006 == "N")
                    {
                        if (CaseMST.LPM0008 == string.Empty)
                        {
                            boollihpquesvalidation = false;
                            goto validation;
                        }
                    }
                    if (CaseMST.LPM0008 == "Y")
                    {
                        if (CaseMST.LPM0009 == string.Empty)
                        {
                            boollihpquesvalidation = false;
                            goto validation;
                        }
                    }
                    if (CaseMST.LPM0010 == string.Empty)
                    {
                        boollihpquesvalidation = false;
                        goto validation;
                    }
                    if (CaseMST.LPM0011 == string.Empty)
                    {
                        boollihpquesvalidation = false;
                        goto validation;
                    }

                }
                //Commented by Sudheer on 08/04/2020
                //else if ((BaseForm.BaseYear == "2016" || BaseForm.BaseYear == "2017" || BaseForm.BaseYear == "2018" || BaseForm.BaseYear == "2019" || BaseForm.BaseYear == "2020") && (strFilter != "02" || strFilter != "04"))
                //Added by Sudheer on 08/04/2020
                else if (strFilter != "02" || strFilter != "04")
                {

                    if (CaseMST.LPM0001 == string.Empty)
                    {
                        boollihpquesvalidation = false;
                        goto validation;
                    }

                    if (CaseMST.LPM0002 == string.Empty)
                    {

                        boollihpquesvalidation = false;
                        goto validation;
                    }

                    if (CaseMST.LPM0002 == "Y")
                    {
                        if (CaseMST.LPM0003 == string.Empty)
                        {

                            boollihpquesvalidation = false;
                        }
                    }

                    if (CaseMST.LPM0004 == string.Empty)
                    {
                        boollihpquesvalidation = false;
                        goto validation;
                    }
                    if (CaseMST.LPM0001 == "Y" && CaseMST.LPM0004 == "N")
                    {
                        if (CaseMST.LPM0005 == string.Empty)
                        {
                            boollihpquesvalidation = false;
                            goto validation;
                        }
                    }
                    if (CaseMST.LPM0004 == "Y" && (strFilter == "01" || strFilter == "03" || strFilter == "07"))
                    {
                        if (CaseMST.LPM0006 == string.Empty)
                        {

                            boollihpquesvalidation = false;
                            goto validation;
                        }
                    }
                    if (CaseMST.LPM0007 == string.Empty)
                    {

                        boollihpquesvalidation = false;
                        goto validation;
                    }

                    if (CaseMST.LPM0007 == "Y")
                    {
                        if (CaseMST.LPM0008 == string.Empty)
                        {
                            boollihpquesvalidation = false;
                            goto validation;
                        }
                    }

                    if (CaseMST.LPM0007 == "N")
                    {
                        if (CaseMST.LPM0009 == string.Empty)
                        {

                            boollihpquesvalidation = false;
                            goto validation;

                        }
                    }
                    if (CaseMST.LPM0009 == "Y")
                    {
                        if (CaseMST.LPM0010 == string.Empty)
                        {

                            boollihpquesvalidation = false;
                            goto validation;
                        }
                    }
                    if (CaseMST.LPM0011 == string.Empty)
                    {

                        boollihpquesvalidation = false;
                        goto validation;
                    }

                }
            }
        validation:
            return boollihpquesvalidation;

        }


        private bool LandlordRequirevalidation()
        {
            bool boollandlord = true;
            if (FLDCNTLHieEntity.Count > 0)
            {
                foreach (FldcntlHieEntity entity in FLDCNTLHieEntity)
                {
                    bool required = entity.Req.Equals("Y") ? true : false;
                    bool enabled = entity.Enab.Equals("Y") ? true : false;

                    switch (entity.FldCode)
                    {
                        case Consts.CASE2001.LLFirst:
                            if (required)
                            {
                                if (propCaseDiffLLDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffLLDetails.IncareFirst.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                        case Consts.CASE2001.LLLast:
                            if (required)
                            {
                                if (propCaseDiffLLDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffLLDetails.IncareLast.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }

                            }
                            break;
                        case Consts.CASE2001.LLHouseNo:
                            if (required)
                            {
                                if (propCaseDiffLLDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffLLDetails.Hn.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                        case Consts.CASE2001.LLStreet:
                            if (required)
                            {
                                if (propCaseDiffLLDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffLLDetails.Street.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                        case Consts.CASE2001.LLSf:
                            if (required)
                            {
                                if (propCaseDiffLLDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffLLDetails.Suffix.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                        case Consts.CASE2001.LLFlr:
                            if (required)
                            {
                                if (propCaseDiffLLDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffLLDetails.Flr.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                        case Consts.CASE2001.LLCity:
                            if (required)
                            {
                                if (propCaseDiffLLDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffLLDetails.City.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                        case Consts.CASE2001.LLApt:
                            if (required)
                            {
                                if (propCaseDiffLLDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffLLDetails.Apt.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                        case Consts.CASE2001.LLZip:
                            if (required)
                            {
                                if (propCaseDiffLLDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffLLDetails.Zip.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                        case Consts.CASE2001.LLCounty:
                            if (required)
                            {
                                if (propCaseDiffLLDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffLLDetails.County.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                        case Consts.CASE2001.LLState:
                            if (required)
                            {
                                if (propCaseDiffLLDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffLLDetails.State.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                        case Consts.CASE2001.LLPhone:
                            if (required)
                            {
                                if (propCaseDiffLLDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffLLDetails.Phone.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                    }

                }

            }
        lldata:
            return boollandlord;

        }


        private bool MailAddressRequirevalidation()
        {
            bool boollandlord = true;
            if (FLDCNTLHieEntity.Count > 0)
            {
                foreach (FldcntlHieEntity entity in FLDCNTLHieEntity)
                {
                    bool required = entity.Req.Equals("Y") ? true : false;
                    bool enabled = entity.Enab.Equals("Y") ? true : false;

                    switch (entity.FldCode)
                    {
                        case Consts.CASE2001.InCaseOfFirst:
                            if (required)
                            {
                                if (propCaseDiffMailDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffMailDetails.IncareFirst.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                        case Consts.CASE2001.InCaseOfLast:
                            if (required)
                            {
                                if (propCaseDiffMailDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffMailDetails.IncareLast.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }

                            }
                            break;
                        case Consts.CASE2001.MailHouseNo:
                            if (required)
                            {
                                if (propCaseDiffMailDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffMailDetails.Hn.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                        case Consts.CASE2001.MailStreet:
                            if (required)
                            {
                                if (propCaseDiffMailDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffMailDetails.Street.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                        case Consts.CASE2001.MailSf:
                            if (required)
                            {
                                if (propCaseDiffMailDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffMailDetails.Suffix.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                        case Consts.CASE2001.MailFlr:
                            if (required)
                            {
                                if (propCaseDiffMailDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffMailDetails.Flr.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                        case Consts.CASE2001.MailCity:
                            if (required)
                            {
                                if (propCaseDiffMailDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffMailDetails.City.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                        case Consts.CASE2001.MailApt:
                            if (required)
                            {
                                if (propCaseDiffMailDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffMailDetails.Apt.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                        case Consts.CASE2001.MailZip:
                            if (required)
                            {
                                if (propCaseDiffMailDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffMailDetails.Zip.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                        case Consts.CASE2001.MailCounty:
                            if (required)
                            {
                                if (propCaseDiffMailDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffMailDetails.County.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                        case Consts.CASE2001.MailState:
                            if (required)
                            {
                                if (propCaseDiffMailDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffMailDetails.State.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                        case Consts.CASE2001.MailPhone:
                            if (required)
                            {
                                if (propCaseDiffMailDetails != null)
                                {
                                    if (string.IsNullOrEmpty(propCaseDiffMailDetails.Phone.Trim()))
                                    {
                                        boollandlord = false;
                                        goto lldata;
                                    }
                                }
                                else
                                {

                                    boollandlord = false;
                                    goto lldata;
                                }
                            }
                            break;
                    }

                }

            }
        lldata:
            return boollandlord;

        }

        private void txtHN_Leave(object sender, EventArgs e)
        {

            if (txtHN.Text.Trim() == string.Empty)
            {
                txtPrecinct.Text = string.Empty;
                if (ProgramDefinition.DepAddressEdit.ToUpper() == "Y")
                {
                    if (((ListItem)cmbOutService.SelectedItem).Value.ToString() == "I")
                    {
                        SetComboBoxValue(cmbOutService, "0");
                    }
                    cmbOutService.Enabled = true;
                }
                else
                {
                    cmbOutService.Enabled = false;
                }

            }
            else
            {
                if ((propAgencyControlDetails.State.ToUpper() == "TX") && (ProgramDefinition.DepAddressEdit.ToUpper() == "Y"))
                {

                    checkAddress(string.Empty);
                }
            }

        }

        bool checkAddress(string strvalidaddress)
        {
            bool boolcheckaddress = true;
            string strcmbservice = string.Empty;
            string strHousNo = string.Empty; string strEONumber = "O";
            bool boolisgood = false;

            if (ProgramDefinition.DepAddressEdit.ToUpper() == "Y")
            {
                txtPrecinct.Text = string.Empty;
                if (txtStreet.Text == string.Empty || txtCity.Text == string.Empty || txtHN.Text == string.Empty)
                {

                }
                else
                {
                    if (txtStreet.Text.ToUpper() == "HOMELESS")
                    {
                        txtPrecinct.Text = "000";
                        boolisgood = true;
                    }

                    List<CASEVOTEntity> casevotdata = propcaseVot.FindAll(u => u.City.ToUpper() == txtCity.Text.Trim().ToUpper() && u.Street.ToUpper() == txtStreet.Text.ToUpper() && u.Suffix.ToUpper() == txtSuffix.Text.ToUpper() && u.Direction.ToUpper() == txtDirection.Text.ToUpper());
                    if (casevotdata.Count > 0)
                    {
                        foreach (CASEVOTEntity casevotitem in casevotdata)
                        {
                            casevotitem.Block = addextraZeros(casevotitem.Block, casevotitem.Block.Length);
                        }
                        // casevotdata = casevotdata.FindAll(u=>u.Block=addextraZeros(u.Block,u.Block.Length));
                        boolisgood = true;
                        strHousNo = txtHN.Text;
                        strHousNo = strHousNo.Replace(" ", "0");
                        strHousNo = "00000000".Substring(0, 8 - strHousNo.Length) + strHousNo;
                        if (!System.Text.RegularExpressions.Regex.IsMatch(strHousNo, Consts.StaticVars.Numericonly))
                        {
                            int intEO = (Convert.ToInt32(strHousNo) % 2);
                            if (intEO == 0)
                                strEONumber = "E";
                            strHousNo = strHousNo.Substring(2, 6);
                            List<CASEVOTEntity> casevotblockdata = casevotdata.FindAll(u => Convert.ToInt32(u.Block) > Convert.ToInt32(strHousNo) && u.EO == strEONumber);
                            if (casevotblockdata.Count > 0)
                            {
                                txtPrecinct.Text = casevotblockdata[0].Precinct;
                            }
                        }
                    }

                }
            }


            if (txtStreet.Text != string.Empty && txtCity.Text != string.Empty && txtHN.Text != string.Empty)
            {
                if (boolisgood)
                {
                    cmbOutService.Enabled = false;
                    SetComboBoxValue(cmbOutService, "I");
                }
                else
                {
                    if (ProgramDefinition.DepAddressEdit.ToUpper() == "Y")
                    {
                        if (((ListItem)cmbOutService.SelectedItem).Value.ToString() == "I")
                        {
                            SetComboBoxValue(cmbOutService, "0");
                        }
                        if (((ListItem)cmbOutService.SelectedItem).Value.ToString() == "0")
                        {
                            CommonFunctions.MessageBoxDisplay("Invalid Address, Outside Service Area \n If You Want to Continue, Select Out of Service  Area");
                            boolcheckaddress = false;
                        }
                        cmbOutService.Enabled = true;
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay("Invalid Address, Outside Service Area");
                        boolcheckaddress = false;
                        if (((ListItem)cmbOutService.SelectedItem).Value.ToString() == "I")
                        {
                            SetComboBoxValue(cmbOutService, "0");
                        }
                        cmbOutService.Enabled = true;
                    }
                    txtPrecinct.Text = string.Empty;
                }
            }
            if (strvalidaddress == "Save")
            {
                if (lblPrecinctReq.Visible && string.IsNullOrEmpty(txtPrecinct.Text.Trim()))
                {
                    _errorProvider.SetError(txtPrecinct, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPrecinct.Text));
                    boolcheckaddress = false;
                    tabControl1.SelectedIndex = 1;
                }
                else
                {
                    _errorProvider.SetError(txtPrecinct, null);
                }

            }
            return boolcheckaddress;
        }

        string addextraZeros(string strValue, int strLength)
        {
            string strTotalvalue = strValue;
            switch (strLength)
            {
                case 0:
                    strTotalvalue = "000000";
                    break;
                case 1:
                    strTotalvalue = strTotalvalue + "00000";
                    break;
                case 2:
                    strTotalvalue = strTotalvalue + "0000";
                    break;
                case 3:
                    strTotalvalue = strTotalvalue + "000";
                    break;
                case 4:
                    strTotalvalue = strTotalvalue + "00";
                    break;
                case 5:
                    strTotalvalue = strTotalvalue + "0";
                    break;
            }
            return strTotalvalue;
        }

        void Enableoutservicecmb()
        {
            //propcaseVot = _model.EMSBDCData.GETCASEVOT(string.Empty, string.Empty, string.Empty);
            //if (propAgencyControlDetails.State.ToUpper() == "TX")
            //{
            //    cmbOutService.Visible = true;
            //    if (ProgramDefinition.DepAddressEdit.ToUpper() == "Y")
            //    {
            //        if (Mode.ToUpper() == "ADD")
            //        {
            //            cmbOutService.Enabled = true;
            //        }
            //        if (Mode.ToUpper() == "EDIT")
            //        {
            //            if (txtPrecinct.Text == string.Empty)
            //            {
            //                cmbOutService.Enabled = true;
            //            }
            //            else
            //                cmbOutService.Enabled = false;
            //        }
            //    }
            //    else
            //    {
            //        cmbOutService.Enabled = false;
            //    }
            //}
            //else
            //{
            //    cmbOutService.Visible = false;
            //}
            //if (propcaseVot.Count > 0 && txtStreet.Enabled == true)
            //{
            //    btnStreetSearch.Enabled = true;
            //}
            //else
            //{
            //    btnStreetSearch.Enabled = false;
            //}
        }

        private void btnStreetSearch_Click(object sender, EventArgs e)
        {

            VoterSerachForm voterSearchForm = new VoterSerachForm(txtStreet.Text, propcaseVot);
            voterSearchForm.FormClosed += new FormClosedEventHandler(OnvoterSearchFormClosed);
            voterSearchForm.StartPosition = FormStartPosition.CenterScreen;
            voterSearchForm.ShowDialog();
        }

        private void OnvoterSearchFormClosed(object sender, FormClosedEventArgs e)
        {
            VoterSerachForm form = sender as VoterSerachForm;
            if (form.DialogResult == DialogResult.OK)
            {
                CASEVOTEntity voterdetais = form.GetSelectedVoterdetails();
                if (voterdetais != null)
                {
                    txtStreet.Text = voterdetais.Street;
                    txtCity.Text = voterdetais.City;
                    txtPrecinct.Text = voterdetais.Precinct;
                    txtSuffix.Text = voterdetais.Suffix;
                    txtDirection.Text = voterdetais.Direction;
                    if (voterdetais.Zip.Length == 9)
                    {
                        txtZipCode.Text = voterdetais.Zip.Substring(0, 5);
                        txtZipPlus.Text = voterdetais.Zip.Substring(5, 4);

                        if (txtZipCode.Text.Length == 5)
                        {
                            List<ZipCodeEntity> zipcodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(txtZipCode.Text, string.Empty, string.Empty, string.Empty);
                            //if (zipcodeEntity.Count > 0)
                            //    zipcodeEntity = zipcodeEntity.FindAll(u => u.InActive.Equals("N") || u.InActive.Trim().Equals(""));
                            if (zipcodeEntity.Count > 0)
                            {
                                SetComboBoxValue(cmbCounty, zipcodeEntity[0].Zcrcountry);
                            }
                        }


                    }
                    if (txtStreet.Text.ToUpper() == "HOMELESS")
                    {
                        txtPrecinct.Text = "000";
                    }
                    cmbOutService.Enabled = false;
                    SetComboBoxValue(cmbOutService, "I");

                    //else if (voterdetais.Zip.Length < 9 && voterdetais.Zip.Length > 6)
                    //{ }

                }
            }
        }

        private void txtStreet_Leave(object sender, EventArgs e)
        {
            if ((propAgencyControlDetails.State.ToUpper() == "TX") && (ProgramDefinition.DepAddressEdit.ToUpper() == "Y"))
            {
                checkAddress(string.Empty);
            }
        }

        private void cmbOutService_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (propAgencyControlDetails.State.ToUpper() == "TX")
                {
                    if (((ListItem)cmbOutService.SelectedItem).Value.ToString() == "I" && txtPrecinct.Text.Trim() == string.Empty)
                    {
                        CommonFunctions.MessageBoxDisplay("Invalid Access");
                        SetComboBoxValue(cmbOutService, "0");
                    }

                    if (((ListItem)cmbOutService.SelectedItem).Value.ToString() != "I" && ((ListItem)cmbOutService.SelectedItem).Value.ToString() != "0")
                    {
                        lblPrecinctReq.Visible = false;
                    }
                    else
                        lblPrecinctReq.Visible = true;

                }
            }
            catch (Exception ex)
            {


            }
        }

        #region PreassQuestionGridLogic

        //string saveImage = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("save.gif");
        //string DeleteImage = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("DeleteItem.gif");
        //string Img_Blank = Consts.Icons.ico_Blank;

        string saveImage = "Resources/Images/save.gif";
        string DeleteImage = "Resources/Images/DeleteItem.gif";
        string Img_Blank = "Resources/Icons/16X16/BlankIcon.gif";
        

        private void fillPreassCustomQuestions()
        {
            List<CustomQuestionsEntity> custQuestions = proppreassesQuestions;
            if (((ListItem)cmbQuestionType.SelectedItem).Value.ToString().ToUpper() == "A")
                custQuestions = custQuestions.FindAll(u => u.CUSTACTIVECUST.ToUpper() == "A");
            if (((ListItem)cmbQuestionType.SelectedItem).Value.ToString().ToUpper() == "I")
                custQuestions = custQuestions.FindAll(u => u.CUSTACTIVECUST.ToUpper() == "I");

            List<CustomQuestionsEntity> custResponses = _model.CaseMstData.GetPreassesQuestionAnswers(CaseSNP, "PRESRESP");
            bool isResponse = false;

            string strEnabledata = string.Empty;
            string[] arrEnabledata = null;
            string strDisabledata = string.Empty;
            string[] arrDisabledata = null;
            string strRequiredata = string.Empty;
            string[] arrRequiredata = null;

            //bool boolexist = false;
            //foreach (CustRespEntity dr in custReponseEntity)
            //{
            //    boolexist = false;
            //    string resDesc = dr.DescCode.ToString().Trim();


            //    if (arrResponse != null && arrResponse.ToList().Exists(u => u.Equals(resDesc)))
            //    {
            //        boolexist = true;
            //    }


            gvwPreassesData.Rows.Clear();
            if (custQuestions.Count > 0)
            {
                gvwPreassesData.CellValueChanged -= new DataGridViewCellEventHandler(gvwPreassesData_CellValueChanged);

                foreach (CommonEntity preassesdata in preassessMasterEntity)
                {
                    List<PreassessQuesEntity> preassessChildList = preassessChildEntity.FindAll(u => u.PRECHILD_DID == preassesdata.Code);
                    if (preassessChildList.Count > 0)
                    {
                        bool boolQuestions = false;
                        foreach (PreassessQuesEntity preasschilddata in preassessChildList)
                        {
                            CustomQuestionsEntity dr = custQuestions.Find(u => u.CUSTCODE == preasschilddata.PRECHILD_QID);
                            if (dr != null)
                            {
                                boolQuestions = true;
                            }
                        }

                        if (boolQuestions)
                        {
                            int rowIndex = gvwPreassesData.Rows.Add(string.Empty, /*Img_Blank*/"", preassesdata.Desc, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, /*Img_Blank*/"", preassesdata.Code);
                            gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                            gvwPreassesData.Rows[rowIndex].Tag = preassesdata;

                            preassessChildList = preassessChildList.OrderBy(u => Convert.ToInt32(u.PRECHILD_SEQ)).ToList();
                            foreach (PreassessQuesEntity preasschilddata in preassessChildList)
                            {
                                CustomQuestionsEntity dr = custQuestions.Find(u => u.CUSTCODE == preasschilddata.PRECHILD_QID);
                                if (dr != null)
                                {
                                    string custCode = dr.CUSTCODE.ToString();
                                    List<CustomQuestionsEntity> response = custResponses.FindAll(u => u.ACTCODE.Equals(custCode)).ToList();


                                    rowIndex = gvwPreassesData.Rows.Add(string.Empty, System.Drawing.Image.FromFile(Application.MapPath(Img_Blank)), dr.CUSTDESC, string.Empty, string.Empty, preasschilddata.PRECHILD_QID, preasschilddata.PRECHILD_DQID, preasschilddata.PRECHILD_REQ, preasschilddata.PRECHILD_ENABLE, preasschilddata.PRECHILD_DISABLE, System.Drawing.Image.FromFile(Application.MapPath(Img_Blank)), preassesdata.Code);

                                    gvwPreassesData.Rows[rowIndex].Cells["gvtPQuestions"].ToolTipText = dr.CUSTDESC;

                                    if (dr.CUSTREQUIRED == "Y")
                                    {
                                        gvwPreassesData.Rows[rowIndex].Cells["gvtPreq"].Value = "*";
                                    }
                                    gvwPreassesData.Rows[rowIndex].Cells["gvtPSave"].Tag = "N";
                                    if (response.Count > 0)
                                    {
                                        gvwPreassesData.Rows[rowIndex].Cells["gvtPSave"].Value = "icon-save";
                                        gvwPreassesData.Rows[rowIndex].Cells["gvtPDelete"].Value = "delete-item";

                                        
                                        if (!dr.CUSTACTIVECUST.Equals("A"))
                                        {
                                            gvwPreassesData.Rows[rowIndex].Cells["gvtPSave"].Tag = "Y";
                                        }
                                    }
                                    gvwPreassesData.Rows[rowIndex].Tag = dr;

                                    string fieldType = dr.CUSTRESPTYPE.ToString();
                                    if (fieldType.Equals("D"))
                                    {
                                        gvwPreassesData.Rows[rowIndex].ReadOnly = true;
                                        gvwPreassesData.Rows[rowIndex].Cells["gvtPResponse"].ToolTipText = "Question Type: Drop down";

                                        if (Mode.Equals(Consts.Common.Add))
                                        {
                                            List<CustRespEntity> custReponseEntity = propcustRespAllEntity.FindAll(u => u.ResoCode.Trim() == custCode.Trim() && u.RespDefault == "Y");

                                            if (custReponseEntity.Count > 0)
                                            {

                                                gvwPreassesData.Rows[rowIndex].Cells[3].Tag = custReponseEntity[0].DescCode;
                                                gvwPreassesData.Rows[rowIndex].Cells[3].Value = custReponseEntity[0].RespDesc;
                                                gvwPreassesData.Rows[rowIndex].Cells[2].Tag = string.Empty;
                                                PreassessQuesEntity preassesdimentdata = preassessChildEntity.Find(u => u.PRECHILD_DQID == custCode);
                                                if (preassesdimentdata != null)
                                                {
                                                    strDisabledata = preassesdimentdata.PRECHILD_DISABLE;
                                                    strEnabledata = preassesdimentdata.PRECHILD_ENABLE;
                                                    arrDisabledata = null;
                                                    arrEnabledata = null;

                                                    if (strDisabledata.IndexOf(',') > 0)
                                                    {
                                                        arrDisabledata = strDisabledata.Split(',');
                                                    }
                                                    else if (!strDisabledata.Equals(string.Empty))
                                                    {
                                                        arrDisabledata = new string[] { strDisabledata };
                                                    }
                                                    if (strEnabledata.IndexOf(',') > 0)
                                                    {
                                                        arrEnabledata = strEnabledata.Split(',');
                                                    }
                                                    else if (!strEnabledata.Equals(string.Empty))
                                                    {
                                                        arrEnabledata = new string[] { strEnabledata };
                                                    }
                                                    strRequiredata = preassesdimentdata.PRECHILD_REQ;
                                                    arrRequiredata = null;
                                                    if (strRequiredata.IndexOf(',') > 0)
                                                    {
                                                        arrRequiredata = strRequiredata.Split(',');
                                                    }
                                                    else if (!strRequiredata.Equals(string.Empty))
                                                    {
                                                        arrRequiredata = new string[] { strRequiredata };
                                                    }

                                                    //gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                    //gvwPreassesData.Rows[rowIndex].Cells[3].Tag = null;
                                                    //gvwPreassesData.Rows[rowIndex].Cells[3].Value = string.Empty;

                                                    //List<CustomQuestionsEntity> preassesresponse = custResponses.FindAll(u => u.ACTCODE.Equals(preassesdimentdata.PRECHILD_QID)).ToList();
                                                    List<CustRespEntity> preassesresponse = propcustRespAllEntity.FindAll(u => u.ResoCode.Trim() == preassesdimentdata.PRECHILD_QID && u.RespDefault == "Y").ToList();
                                                    if (preassesresponse.Count > 0)
                                                    {
                                                        if (arrDisabledata != null)
                                                        {
                                                            if (arrDisabledata.ToList().Exists(u => u.Equals(preassesresponse[0].DescCode)))
                                                            {
                                                                gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPreq"].Style.ForeColor = System.Drawing.Color.Red;
                                                                gvwPreassesData.Rows[rowIndex].Cells[3].Tag = null;
                                                                gvwPreassesData.Rows[rowIndex].Cells[3].Value = string.Empty;
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPDEnable"].Tag = "N";
                                                            }

                                                        }
                                                        if (arrEnabledata != null)
                                                        {
                                                            if (arrEnabledata.ToList().Exists(u => u.Equals(preassesresponse[0].DescCode)))//preassesresponse[0].ACTMULTRESP)))
                                                            {
                                                                gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPreq"].Style.ForeColor = System.Drawing.Color.Red;
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPDEnable"].Tag = "Y";
                                                            }
                                                            else
                                                            {
                                                                gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPreq"].Style.ForeColor = System.Drawing.Color.Red;
                                                                gvwPreassesData.Rows[rowIndex].Cells[3].Tag = null;
                                                                gvwPreassesData.Rows[rowIndex].Cells[3].Value = string.Empty;
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPDEnable"].Tag = "N";
                                                            }
                                                        }
                                                        if (arrRequiredata != null)
                                                        {
                                                            if (arrRequiredata.ToList().Exists(u => u.Equals(preassesresponse[0].DescCode)))
                                                            {
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPreq"].Tag = "Y";
                                                            }
                                                            else
                                                            {
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPreq"].Tag = "N";
                                                            }
                                                        }

                                                    }
                                                }

                                            }

                                        }
                                    }
                                    else if (fieldType.Equals("C"))
                                    {
                                        gvwPreassesData.Rows[rowIndex].ReadOnly = true;
                                        gvwPreassesData.Rows[rowIndex].Cells["gvtPResponse"].ToolTipText = "Question Type: Check Box";
                                    }
                                    else
                                    {
                                        gvwPreassesData.Rows[rowIndex].Cells[1].ReadOnly = true;
                                        gvwPreassesData.Rows[rowIndex].Cells[2].ReadOnly = true;
                                    }

                                    if (fieldType.Equals("N"))
                                    {
                                        gvwPreassesData.Rows[rowIndex].Cells["gvtPResponse"].ToolTipText = "Question Type: Numeric";
                                    }
                                    else if (fieldType.Equals("T"))
                                    {
                                        gvwPreassesData.Rows[rowIndex].Cells["gvtPResponse"].ToolTipText = "Question Type: Date";
                                    }
                                    else if (fieldType.Equals("X"))
                                    {
                                        gvwPreassesData.Rows[rowIndex].Cells["gvtPResponse"].ToolTipText = "Question Type: Text";
                                    }

                                    string custQuestionResp = string.Empty;
                                    string custQuestionCode = string.Empty;

                                    if (response != null && response.Count > 0)
                                    {
                                        isResponse = true;
                                        if (fieldType.Equals("D"))
                                        {
                                            List<CustRespEntity> custReponseEntity = propcustRespAllEntity.FindAll(u => u.ResoCode.Trim() == response[0].ACTCODE.Trim());

                                            foreach (CustomQuestionsEntity custResp in response)
                                            {
                                                string code = custResp.ACTMULTRESP.Trim();
                                                CustRespEntity custRespEntity = custReponseEntity.Find(u => u.DescCode.Trim().Equals(code));
                                                if (custRespEntity != null)
                                                {
                                                    custQuestionResp += custRespEntity.RespDesc;
                                                    custQuestionCode += custResp.ACTMULTRESP.ToString() + " ";
                                                }
                                            }
                                            if (Mode.Equals(Consts.Common.Edit))
                                            {
                                                PreassessQuesEntity preassesdimentdata = preassessChildEntity.Find(u => u.PRECHILD_DQID == custCode);
                                                if (preassesdimentdata != null)
                                                {
                                                    strDisabledata = preassesdimentdata.PRECHILD_DISABLE;
                                                    strEnabledata = preassesdimentdata.PRECHILD_ENABLE;
                                                    arrDisabledata = null;
                                                    arrEnabledata = null;

                                                    if (strDisabledata.IndexOf(',') > 0)
                                                    {
                                                        arrDisabledata = strDisabledata.Split(',');
                                                    }
                                                    else if (!strDisabledata.Equals(string.Empty))
                                                    {
                                                        arrDisabledata = new string[] { strDisabledata };
                                                    }
                                                    if (strEnabledata.IndexOf(',') > 0)
                                                    {
                                                        arrEnabledata = strEnabledata.Split(',');
                                                    }
                                                    else if (!strEnabledata.Equals(string.Empty))
                                                    {
                                                        arrEnabledata = new string[] { strEnabledata };
                                                    }
                                                    strRequiredata = preassesdimentdata.PRECHILD_REQ;
                                                    arrRequiredata = null;
                                                    if (strRequiredata.IndexOf(',') > 0)
                                                    {
                                                        arrRequiredata = strRequiredata.Split(',');
                                                    }
                                                    else if (!strRequiredata.Equals(string.Empty))
                                                    {
                                                        arrRequiredata = new string[] { strRequiredata };
                                                    }

                                                    //gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                    //gvwPreassesData.Rows[rowIndex].Cells[3].Tag = null;
                                                    //gvwPreassesData.Rows[rowIndex].Cells[3].Value = string.Empty;

                                                    List<CustomQuestionsEntity> preassesresponse = custResponses.FindAll(u => u.ACTCODE.Equals(preassesdimentdata.PRECHILD_QID)).ToList();
                                                    if (preassesresponse.Count > 0)
                                                    {
                                                        if (arrDisabledata != null)
                                                        {
                                                            if (arrDisabledata.ToList().Exists(u => u.Equals(preassesresponse[0].ACTMULTRESP)))
                                                            {
                                                                gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPreq"].Style.ForeColor = System.Drawing.Color.Red;
                                                                gvwPreassesData.Rows[rowIndex].Cells[3].Tag = null;
                                                                gvwPreassesData.Rows[rowIndex].Cells[3].Value = string.Empty;
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPDEnable"].Tag = "N";
                                                            }

                                                        }
                                                        if (arrEnabledata != null)
                                                        {
                                                            if (arrEnabledata.ToList().Exists(u => u.Equals(preassesresponse[0].ACTMULTRESP)))
                                                            {
                                                                gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPreq"].Style.ForeColor = System.Drawing.Color.Red;
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPDEnable"].Tag = "Y";
                                                            }
                                                            else
                                                            {
                                                                gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPreq"].Style.ForeColor = System.Drawing.Color.Red;
                                                                gvwPreassesData.Rows[rowIndex].Cells[3].Tag = null;
                                                                gvwPreassesData.Rows[rowIndex].Cells[3].Value = string.Empty;
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPDEnable"].Tag = "N";
                                                            }
                                                        }
                                                        if (arrRequiredata != null)
                                                        {
                                                            if (arrRequiredata.ToList().Exists(u => u.Equals(preassesresponse[0].ACTMULTRESP)))
                                                            {
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPreq"].Tag = "Y";
                                                            }
                                                            else
                                                            {
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPreq"].Tag = "N";
                                                            }
                                                        }

                                                    }
                                                }
                                            }


                                            gvwPreassesData.Rows[rowIndex].Cells[3].Tag = custQuestionCode;
                                            gvwPreassesData.Rows[rowIndex].Cells[3].Value = custQuestionResp;
                                            gvwPreassesData.Rows[rowIndex].Cells[2].Tag = "U";
                                        }
                                        else if (fieldType.Equals("C"))
                                        {
                                            custQuestionResp = response[0].ACTALPHARESP;
                                            gvwPreassesData.Rows[rowIndex].Cells[3].Tag = response[0].ACTALPHARESP;
                                            gvwPreassesData.Rows[rowIndex].Cells[3].Value = response[0].ACTALPHARESP;
                                            gvwPreassesData.Rows[rowIndex].Cells[2].Tag = "U";

                                            if (Mode.Equals(Consts.Common.Edit))
                                            {
                                                PreassessQuesEntity preassesdimentdata = preassessChildEntity.Find(u => u.PRECHILD_DQID == custCode);
                                                if (preassesdimentdata != null)
                                                {
                                                    strDisabledata = preassesdimentdata.PRECHILD_DISABLE;
                                                    strEnabledata = preassesdimentdata.PRECHILD_ENABLE;
                                                    arrDisabledata = null;
                                                    arrEnabledata = null;
                                                    if (strDisabledata.IndexOf(',') > 0)
                                                    {
                                                        arrDisabledata = strDisabledata.Split(',');
                                                    }
                                                    else if (!strDisabledata.Equals(string.Empty))
                                                    {
                                                        arrDisabledata = new string[] { strDisabledata };
                                                    }
                                                    if (strEnabledata.IndexOf(',') > 0)
                                                    {
                                                        arrEnabledata = strEnabledata.Split(',');
                                                    }
                                                    else if (!strEnabledata.Equals(string.Empty))
                                                    {
                                                        arrEnabledata = new string[] { strEnabledata };
                                                    }
                                                    strRequiredata = preassesdimentdata.PRECHILD_REQ;
                                                    arrRequiredata = null;
                                                    if (strRequiredata.IndexOf(',') > 0)
                                                    {
                                                        arrRequiredata = strRequiredata.Split(',');
                                                    }
                                                    else if (!strRequiredata.Equals(string.Empty))
                                                    {
                                                        arrRequiredata = new string[] { strRequiredata };
                                                    }

                                                    //gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                    //gvwPreassesData.Rows[rowIndex].Cells[3].Tag = null;
                                                    //gvwPreassesData.Rows[rowIndex].Cells[3].Value = string.Empty;

                                                    List<CustomQuestionsEntity> preassesresponse = custResponses.FindAll(u => u.ACTCODE.Equals(preassesdimentdata.PRECHILD_QID)).ToList();
                                                    if (preassesresponse.Count > 0)
                                                    {
                                                        if (arrDisabledata != null)
                                                        {
                                                            if (arrDisabledata.ToList().Exists(u => u.Equals(preassesresponse[0].ACTMULTRESP)))
                                                            {
                                                                gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPreq"].Style.ForeColor = System.Drawing.Color.Red;
                                                                gvwPreassesData.Rows[rowIndex].Cells[3].Tag = null;
                                                                gvwPreassesData.Rows[rowIndex].Cells[3].Value = string.Empty;
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPDEnable"].Tag = "N";
                                                            }

                                                        }
                                                        if (arrEnabledata != null)
                                                        {
                                                            if (arrEnabledata.ToList().Exists(u => u.Equals(preassesresponse[0].ACTMULTRESP)))
                                                            {
                                                                gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPreq"].Style.ForeColor = System.Drawing.Color.Red;
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPDEnable"].Tag = "Y";
                                                            }
                                                            else
                                                            {
                                                                gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPreq"].Style.ForeColor = System.Drawing.Color.Red;
                                                                gvwPreassesData.Rows[rowIndex].Cells[3].Tag = null;
                                                                gvwPreassesData.Rows[rowIndex].Cells[3].Value = string.Empty;
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPDEnable"].Tag = "N";
                                                            }
                                                        }
                                                        if (arrRequiredata != null)
                                                        {
                                                            if (arrRequiredata.ToList().Exists(u => u.Equals(preassesresponse[0].ACTMULTRESP)))
                                                            {
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPreq"].Tag = "Y";
                                                            }
                                                            else
                                                            {
                                                                gvwPreassesData.Rows[rowIndex].Cells["gvtPreq"].Tag = "N";
                                                            }
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                        else if (fieldType.Equals("N"))
                                        {
                                            custQuestionResp = response[0].ACTNUMRESP.ToString();
                                        }
                                        else if (fieldType.Equals("T"))
                                        {
                                            custQuestionResp = LookupDataAccess.Getdate(response[0].ACTDATERESP.ToString());
                                        }
                                        else
                                        {
                                            custQuestionResp = response[0].ACTALPHARESP.ToString();
                                        }
                                        gvwPreassesData.Rows[rowIndex].Cells[3].Value = custQuestionResp;
                                        gvwPreassesData.Rows[rowIndex].Cells[2].Tag = "U";
                                        //gvwPreassesData.Rows[rowIndex].Cells["FamilySeq"].Value = response[0].ACTSNPFAMILYSEQ;
                                        //gvwPreassesData.Rows[rowIndex].Cells["ResponceSeq"].Value = response[0].ACTRESPSEQ;
                                        //gvwPreassesData.Rows[rowIndex].Cells["Code"].Value = response[0].ACTCODE;
                                    }
                                    else
                                    {
                                        if (Mode.Equals(Consts.Common.Edit))
                                        {
                                            PreassessQuesEntity preassesdimentdata = preassessChildEntity.Find(u => u.PRECHILD_DQID == custCode);
                                            if (preassesdimentdata != null)
                                            {
                                                strDisabledata = preassesdimentdata.PRECHILD_DISABLE;
                                                strEnabledata = preassesdimentdata.PRECHILD_ENABLE;
                                                arrDisabledata = null;
                                                arrEnabledata = null;
                                                if (strDisabledata.IndexOf(',') > 0)
                                                {
                                                    arrDisabledata = strDisabledata.Split(',');
                                                }
                                                else if (!strDisabledata.Equals(string.Empty))
                                                {
                                                    arrDisabledata = new string[] { strDisabledata };
                                                }
                                                if (strEnabledata.IndexOf(',') > 0)
                                                {
                                                    arrEnabledata = strEnabledata.Split(',');
                                                }
                                                else if (!strEnabledata.Equals(string.Empty))
                                                {
                                                    arrEnabledata = new string[] { strEnabledata };
                                                }
                                                strRequiredata = preassesdimentdata.PRECHILD_REQ;
                                                arrRequiredata = null;
                                                if (strRequiredata.IndexOf(',') > 0)
                                                {
                                                    arrRequiredata = strRequiredata.Split(',');
                                                }
                                                else if (!strRequiredata.Equals(string.Empty))
                                                {
                                                    arrRequiredata = new string[] { strRequiredata };
                                                }

                                                //gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                //gvwPreassesData.Rows[rowIndex].Cells[3].Tag = null;
                                                //gvwPreassesData.Rows[rowIndex].Cells[3].Value = string.Empty;

                                                List<CustomQuestionsEntity> preassesresponse = custResponses.FindAll(u => u.ACTCODE.Equals(preassesdimentdata.PRECHILD_QID)).ToList();
                                                if (preassesresponse.Count > 0)
                                                {
                                                    if (arrDisabledata != null)
                                                    {
                                                        if (arrDisabledata.ToList().Exists(u => u.Equals(preassesresponse[0].ACTMULTRESP)))
                                                        {
                                                            gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                            gvwPreassesData.Rows[rowIndex].Cells["gvtPreq"].Style.ForeColor = System.Drawing.Color.Red;
                                                            gvwPreassesData.Rows[rowIndex].Cells[3].Tag = null;
                                                            gvwPreassesData.Rows[rowIndex].Cells[3].Value = string.Empty;
                                                            gvwPreassesData.Rows[rowIndex].Cells["gvtPDEnable"].Tag = "N";
                                                        }

                                                    }
                                                    if (arrEnabledata != null)
                                                    {
                                                        if (arrEnabledata.ToList().Exists(u => u.Equals(preassesresponse[0].ACTMULTRESP)))
                                                        {
                                                            gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                                            gvwPreassesData.Rows[rowIndex].Cells["gvtPreq"].Style.ForeColor = System.Drawing.Color.Red;
                                                            gvwPreassesData.Rows[rowIndex].Cells["gvtPDEnable"].Tag = "Y";
                                                        }
                                                        else
                                                        {
                                                            gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                                            gvwPreassesData.Rows[rowIndex].Cells["gvtPreq"].Style.ForeColor = System.Drawing.Color.Red;
                                                            gvwPreassesData.Rows[rowIndex].Cells[3].Tag = null;
                                                            gvwPreassesData.Rows[rowIndex].Cells[3].Value = string.Empty;
                                                            gvwPreassesData.Rows[rowIndex].Cells["gvtPDEnable"].Tag = "N";
                                                        }
                                                    }
                                                    if (arrRequiredata != null)
                                                    {
                                                        if (arrRequiredata.ToList().Exists(u => u.Equals(preassesresponse[0].ACTMULTRESP)))
                                                        {
                                                            gvwPreassesData.Rows[rowIndex].Cells["gvtPreq"].Tag = "Y";
                                                        }
                                                        else
                                                        {
                                                            gvwPreassesData.Rows[rowIndex].Cells["gvtPreq"].Tag = "N";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (!dr.CUSTACTIVECUST.Equals("A"))
                                    {
                                        gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                                    }

                                }
                            }
                        }
                    }
                }
                gvwPreassesData.Update();

                gvwPreassesData.CellValueChanged += new DataGridViewCellEventHandler(gvwPreassesData_CellValueChanged);

            }
        }

        private void gvwPreassesData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int intcolindex = gvwPreassesData.CurrentCell.ColumnIndex;
            int introwindex = gvwPreassesData.CurrentCell.RowIndex;
            string strCurrectValue = Convert.ToString(gvwPreassesData.Rows[introwindex].Cells[intcolindex].Value);
            CustomQuestionsEntity questionEntity = gvwPreassesData.Rows[e.RowIndex].Tag as CustomQuestionsEntity;

            if (gvwPreassesData.Columns[e.ColumnIndex].Name.Equals("gvtPResponse") && questionEntity != null && questionEntity.CUSTRESPTYPE.Equals("N"))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty)
                {
                    gvwPreassesData.CellValueChanged -= new DataGridViewCellEventHandler(gvwPreassesData_CellValueChanged);
                    gvwPreassesData.Rows[introwindex].Cells["gvtPResponse"].Value = string.Empty;
                    gvwPreassesData.CellValueChanged += new DataGridViewCellEventHandler(gvwPreassesData_CellValueChanged);
                    MessageBox.Show(Consts.Messages.PleaseEnterNumbers);
                }
            }
            else if (gvwPreassesData.Columns[e.ColumnIndex].Name.Equals("gvtPResponse") && questionEntity != null && questionEntity.CUSTRESPTYPE.Equals("T"))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.DateFormatMMDDYYYY))
                {
                    gvwPreassesData.CellValueChanged -= new DataGridViewCellEventHandler(gvwPreassesData_CellValueChanged);
                    gvwPreassesData.Rows[introwindex].Cells["gvtPResponse"].Value = string.Empty;
                    gvwPreassesData.CellValueChanged += new DataGridViewCellEventHandler(gvwPreassesData_CellValueChanged);
                    MessageBox.Show(Consts.Messages.PleaseEntermmddyyyyDateFormat);
                }
                else
                {
                    if (questionEntity.CUSTCALLOWFDATE == "N")
                    {
                        if (Convert.ToDateTime(strCurrectValue).Date > DateTime.Now.Date)
                        {
                            gvwPreassesData.CellValueChanged -= new DataGridViewCellEventHandler(gvwPreassesData_CellValueChanged);
                            gvwPreassesData.Rows[introwindex].Cells["gvtPResponse"].Value = string.Empty;
                            gvwPreassesData.CellValueChanged += new DataGridViewCellEventHandler(gvwPreassesData_CellValueChanged);
                            CommonFunctions.MessageBoxDisplay("Future Date is not Allowed...");
                        }
                    }
                }
            }
        }

        private void gvwPreassesData_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            if (objArgs.MenuItem.Tag is CustRespEntity)
            {
                string responseValue = gvwPreassesData.SelectedRows[0].Cells[3].Value.ToString();
                string responseCode = gvwPreassesData.SelectedRows[0].Cells[3].Tag == null ? string.Empty : gvwPreassesData.SelectedRows[0].Cells[3].Tag.ToString();
                CustRespEntity dr = (CustRespEntity)objArgs.MenuItem.Tag as CustRespEntity;

                string selectedValue = objArgs.MenuItem.Text;
                string selectedCode = dr.DescCode.ToString();
                if (objArgs.MenuItem.Checked)
                {
                    responseValue = selectedValue;
                    responseCode = selectedCode;
                }
                else
                {

                    responseValue = selectedValue;
                    responseCode = selectedCode;

                }
                string custCode = ((CustomQuestionsEntity)gvwPreassesData.SelectedRows[0].Tag).CUSTCODE;
                //_customQuestionsandAnswers.FindAll(u => u.CustCode.Equals(custCode)).ForEach(c => c.ResponseValue = responseValue);
                //_customQuestionsandAnswers.FindAll(u => u.CustCode.Equals(custCode)).ForEach(c => c.ResponseCode = responseCode);
                gvwPreassesData.SelectedRows[0].Cells[3].Tag = responseCode;
                gvwPreassesData.SelectedRows[0].Cells[3].Value = responseValue;

                string strDimentionQid = gvwPreassesData.SelectedRows[0].Cells["gvtPDQId"].Value.ToString();
                if (strDimentionQid != string.Empty)
                {
                    string strEnabledata = string.Empty;
                    string[] arrEnabledata = null;
                    string strDisabledata = string.Empty;
                    string[] arrDisabledata = null;
                    string strRequiredata = string.Empty;
                    string[] arrRequiredata = null;
                    foreach (DataGridViewRow item in gvwPreassesData.Rows)
                    {
                        if (item.Cells["gvtPQId"].Value.ToString().Trim() == strDimentionQid)
                        {
                            PreassessQuesEntity preassesdimentdata = preassessChildEntity.Find(u => u.PRECHILD_DQID == strDimentionQid);
                            if (preassesdimentdata != null)
                            {
                                strDisabledata = preassesdimentdata.PRECHILD_DISABLE;
                                strEnabledata = preassesdimentdata.PRECHILD_ENABLE;
                                arrDisabledata = null;
                                arrEnabledata = null;
                                if (strDisabledata.IndexOf(',') > 0)
                                {
                                    arrDisabledata = strDisabledata.Split(',');
                                }
                                else if (!strDisabledata.Equals(string.Empty))
                                {
                                    arrDisabledata = new string[] { strDisabledata };
                                }
                                if (strEnabledata.IndexOf(',') > 0)
                                {
                                    arrEnabledata = strEnabledata.Split(',');
                                }
                                else if (!strEnabledata.Equals(string.Empty))
                                {
                                    arrEnabledata = new string[] { strEnabledata };
                                }
                                strRequiredata = preassesdimentdata.PRECHILD_REQ;
                                arrRequiredata = null;
                                if (strRequiredata.IndexOf(',') > 0)
                                {
                                    arrRequiredata = strRequiredata.Split(',');
                                }
                                else if (!strRequiredata.Equals(string.Empty))
                                {
                                    arrRequiredata = new string[] { strRequiredata };
                                }

                                item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                item.Cells["gvtPreq"].Style.ForeColor = System.Drawing.Color.Red;
                                if (arrDisabledata != null)
                                {
                                    if (arrDisabledata.ToList().Exists(u => u.Equals(responseCode)))
                                    {
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                        item.Cells["gvtPreq"].Style.ForeColor = System.Drawing.Color.Red;
                                        item.Cells[3].Tag = null;
                                        item.Cells[3].Value = string.Empty;
                                        item.Cells["gvtPDEnable"].Tag = "N";
                                    }

                                }
                                if (arrEnabledata != null)
                                {
                                    if (arrEnabledata.ToList().Exists(u => u.Equals(responseCode)))
                                    {
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                        item.Cells["gvtPreq"].Style.ForeColor = System.Drawing.Color.Red;
                                        item.Cells["gvtPDEnable"].Tag = "Y";
                                    }
                                    else
                                    {
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                        item.Cells["gvtPreq"].Style.ForeColor = System.Drawing.Color.Red;
                                        item.Cells[3].Tag = null;
                                        item.Cells[3].Value = string.Empty;
                                        item.Cells["gvtPDEnable"].Tag = "N";
                                    }
                                }
                                if (arrRequiredata != null)
                                {
                                    if (arrRequiredata.ToList().Exists(u => u.Equals(responseCode)))
                                    {
                                        item.Cells["gvtPreq"].Tag = "Y";
                                    }
                                    else
                                    {
                                        item.Cells["gvtPreq"].Tag = "N";
                                    }
                                }

                            }
                            CustomQuestionsEntity customInactive = ((CustomQuestionsEntity)item.Tag);
                            if (customInactive != null)
                            {
                                if (customInactive.CUSTACTIVECUST.ToString().Trim() != "A")
                                {
                                    item.DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                                }
                            }

                            break;
                        }
                    }

                }
            }
            else
            {
                gvwPreassesData.Rows[0].Cells[3].ReadOnly = false;
                gvwPreassesData.Rows[0].Cells[3].Selected = true;
            }
        }

        private void contextPreasses_Popup(object sender, EventArgs e)
        {

            contextPreasses.MenuItems.Clear();
            if (gvwPreassesData.Rows.Count > 0)
            {
                if (gvwPreassesData.SelectedRows[0].Tag is CustomQuestionsEntity)
                {
                    CustomQuestionsEntity drow = gvwPreassesData.SelectedRows[0].Tag as CustomQuestionsEntity;
                    string fieldCode = drow.CUSTCODE.ToString();
                    string fieldType = drow.CUSTRESPTYPE.ToString();

                    if (drow.CUSTACTIVECUST.ToUpper() == "A")
                    {
                        string strFieldEnable = gvwPreassesData.SelectedRows[0].Cells["gvtPDEnable"].Tag != null ? gvwPreassesData.SelectedRows[0].Cells["gvtPDEnable"].Tag.ToString() : string.Empty;

                        if (strFieldEnable != "N")
                        {
                            if (fieldType.Equals("D"))
                            {
                                List<CustRespEntity> custReponseEntity = propcustRespAllEntity.FindAll(u => u.ResoCode.Trim() == fieldCode.Trim());
                                if (custReponseEntity.Count > 0)
                                {
                                    string response = gvwPreassesData.SelectedRows[0].Cells[3].Value != null ? gvwPreassesData.SelectedRows[0].Cells[3].Value.ToString() : string.Empty;
                                    string[] arrResponse = null;
                                    if (response.IndexOf(',') > 0)
                                    {
                                        arrResponse = response.Split(',');
                                    }
                                    else if (!response.Equals(string.Empty))
                                    {
                                        arrResponse = new string[] { response };
                                    }

                                    foreach (CustRespEntity dr in custReponseEntity)
                                    {
                                        string resDesc = dr.RespDesc.ToString().Trim();

                                        MenuItem menuItem = new MenuItem();
                                        menuItem.Text = resDesc;
                                        menuItem.Tag = dr;
                                        if (arrResponse != null && arrResponse.ToList().Exists(u => u.Equals(resDesc)))
                                        {
                                            menuItem.Checked = true;
                                        }
                                        contextPreasses.MenuItems.Add(menuItem);
                                    }
                                }
                            }
                            else if (fieldType.Equals("C"))
                            {
                                string response = gvwPreassesData.SelectedRows[0].Cells[3].Value != null ? gvwPreassesData.SelectedRows[0].Cells[3].Value.ToString() : string.Empty;
                                PrivilegeEntity privileges = new PrivilegeEntity();
                                privileges.AddPriv = "true";
                                AlertCodeForm objform = new AlertCodeForm(BaseForm, privileges, response, fieldCode);
                                objform.StartPosition = FormStartPosition.CenterScreen;
                                objform.FormClosed += new FormClosedEventHandler(objform_FormClosed);
                                objform.ShowDialog();


                            }
                            else if (fieldType.Equals("X"))
                            {
                                MenuItem menuItem = new MenuItem();
                                menuItem.Text = "Please enter text here";
                                menuItem.Tag = "X"; // menuItem.Tag = "A";
                                contextPreasses.MenuItems.Add(menuItem);
                                gvwPreassesData.Rows[0].Cells[3].ReadOnly = false;
                            }
                            else if (fieldType.Equals("T"))
                            {
                                MenuItem menuItem = new MenuItem();
                                menuItem.Text = "Please enter Date here";
                                menuItem.Tag = "T";//menuItem.Tag = "X";
                                contextPreasses.MenuItems.Add(menuItem);
                                gvwPreassesData.Rows[0].Cells[3].ReadOnly = false;
                            }
                            else if (fieldType.Equals("N"))
                            {
                                MenuItem menuItem = new MenuItem();
                                menuItem.Text = "Please enter number here";
                                menuItem.Tag = "N";
                                contextPreasses.MenuItems.Add(menuItem);
                                gvwPreassesData.Rows[0].Cells[3].ReadOnly = false;
                            }
                        }
                    }
                }
            }
        }



        private void gvwPreassesData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gvtPDelete.Index && e.RowIndex != -1)
            {
                if (SelectedRow())
                {
                    if (gvwPreassesData.SelectedRows[0].Cells["gvtPResponse"].Value != string.Empty)
                        MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose:MessageBoxHandlerPreassesQuestions);
                }
            }
        }

        private void MessageBoxHandlerPreassesQuestions(DialogResult dialogResult)
        {
            
                // Set DialogResult value of the Form as a text for label
                if (dialogResult == DialogResult.Yes)
                {
                    CustomQuestionsEntity custEntity = new CustomQuestionsEntity();
                    //CustomQuestionsEntity questionEntity = gvwCustomQuestions.Tag as CustomQuestionsEntity;
                    custEntity.ACTAGENCY = BaseForm.BaseAgency;
                    custEntity.ACTDEPT = BaseForm.BaseDept;
                    custEntity.ACTPROGRAM = BaseForm.BaseProg;
                    custEntity.ACTYEAR = BaseForm.BaseYear;
                    custEntity.ACTAPPNO = BaseForm.BaseApplicationNo;
                    custEntity.ACTSNPFAMILYSEQ = "7777777";



                    custEntity.ACTCODE = gvwPreassesData.SelectedRows[0].Cells["gvtPQId"].Value.ToString();
                    custEntity.ACTRESPSEQ = "1";
                    custEntity.Mode = "Delete";
                    if (_model.CaseMstData.InsertUpdatePRESRESP(custEntity))
                    {
                        gvwPreassesData.CellValueChanged -= new DataGridViewCellEventHandler(gvwPreassesData_CellValueChanged);
                        //gvwPreassesData.SelectedRows[0].Cells[1].Value = Img_Blank;
                        //gvwPreassesData.SelectedRows[0].Cells["gvtPDelete"].Value = Img_Blank;
                        gvwPreassesData.SelectedRows[0].Cells[3].Value = string.Empty;
                        gvwPreassesData.SelectedRows[0].Cells["gvtPResponse"].Value = string.Empty;
                        gvwPreassesData.SelectedRows[0].Cells[2].Tag = string.Empty;
                        gvwPreassesData.SelectedRows[0].Cells["gvtPResponse"].Tag = null;
                        gvwPreassesData.SelectedRows[0].Cells["gvtPSave"].Tag = "N";
                        gvwPreassesData.CellValueChanged += new DataGridViewCellEventHandler(gvwPreassesData_CellValueChanged);
                        DimensionCalculation(BaseForm.BaseCaseSnpEntity[0]);
                    }

                }
           
        }


        private bool SelectedRow()
        {

            bool boolrowselet = false;
            if (gvwPreassesData.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in gvwPreassesData.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        boolrowselet = true;
                        break;
                    }
                }
            }
            return boolrowselet;
        }

        void objform_FormClosed(object sender, FormClosedEventArgs e)
        {
            AlertCodeForm form = sender as AlertCodeForm;
            if (form.DialogResult == DialogResult.OK)
            {
                gvwPreassesData.SelectedRows[0].Cells[3].Tag = form.propAlertCode;

                string custQuestionResp = string.Empty;
                List<CustRespEntity> custReponseEntity = _model.FieldControls.GetCustomResponses("PREASSES", form.propFieldCode);
                if (custReponseEntity.Count > 0)
                {
                    string response1 = form.propAlertCode;
                    string[] arrResponse = null;
                    if (response1.IndexOf(',') > 0)
                    {
                        arrResponse = response1.Split(',');
                    }
                    else if (!response1.Equals(string.Empty))
                    {
                        arrResponse = new string[] { response1 };
                    }
                    foreach (string stringitem in arrResponse)
                    {

                        CustRespEntity custRespEntity = custReponseEntity.Find(u => u.DescCode.Trim().Equals(stringitem));
                        if (custRespEntity != null)
                        {
                            custQuestionResp += custRespEntity.RespDesc + ", ";
                        }
                    }
                }
                if (custQuestionResp.Length > 1)
                {
                    custQuestionResp = custQuestionResp.Trim();
                    if ((custQuestionResp.Substring(custQuestionResp.Length - 1)) == ",")
                    {
                        custQuestionResp = custQuestionResp.Remove(custQuestionResp.Length - 1, 1);
                    }
                }

                gvwPreassesData.SelectedRows[0].Cells[3].Value = custQuestionResp;
            }
            //  txtAlertCodes.Text = form.propAlertCode;

        }

        private void savePresrespQuestions(CaseSnpEntity CaseSnpEntity)
        {
            foreach (DataGridViewRow dataGridViewRow in gvwPreassesData.Rows)
            {
                if (dataGridViewRow.Tag is CustomQuestionsEntity)   //dataGridViewRow.Cells["Response"].Value != null && !dataGridViewRow.Cells["Response"].Value.ToString().Equals(string.Empty))
                {
                    string inputValue = string.Empty;
                    CustomQuestionsEntity questionEntity = dataGridViewRow.Tag as CustomQuestionsEntity;
                    if (questionEntity != null)
                    {
                        inputValue = dataGridViewRow.Cells["gvtPResponse"].Value != null ? dataGridViewRow.Cells["gvtPResponse"].Value.ToString() : string.Empty;
                        string strmode = dataGridViewRow.Cells[2].Tag == null ? string.Empty : dataGridViewRow.Cells[2].Tag.ToString();
                        if (dataGridViewRow.Cells[2].Tag == null && (dataGridViewRow.Cells[2].Tag != null && !((string)dataGridViewRow.Cells[2].Tag).Equals("U")))
                        {
                            continue;
                        }
                        CustomQuestionsEntity custEntity = new CustomQuestionsEntity();

                        custEntity.ACTAGENCY = CaseSnpEntity.Agency;
                        custEntity.ACTDEPT = CaseSnpEntity.Dept;
                        custEntity.ACTPROGRAM = CaseSnpEntity.Program;
                        if (CaseSnpEntity.Year == string.Empty)
                            custEntity.ACTYEAR = "    ";
                        else
                            custEntity.ACTYEAR = CaseSnpEntity.Year;
                        custEntity.ACTAPPNO = CaseSnpEntity.App;
                        custEntity.ACTSNPFAMILYSEQ = "7777777";

                        custEntity.ACTCODE = questionEntity.CUSTCODE;
                        if (questionEntity.CUSTRESPTYPE.Equals("D"))
                        {
                            if (strmode.Equals("U"))
                            {
                                custEntity.ACTMULTRESP = dataGridViewRow.Cells["gvtPResponse"].Tag == null ? string.Empty : dataGridViewRow.Cells["gvtPResponse"].Tag.ToString();
                            }
                            else
                            {
                                if (dataGridViewRow.Cells["gvtPResponse"].Tag == null) continue;
                                custEntity.ACTMULTRESP = dataGridViewRow.Cells["gvtPResponse"].Tag.ToString().Trim();
                            }
                            CustRespEntity custRespEntity = propcustRespAllEntity.Find(u => u.DescCode.Trim().Equals(custEntity.ACTMULTRESP.Trim()) && u.ResoCode.Trim().Equals(custEntity.ACTCODE.Trim()));
                            if (custRespEntity != null)
                            {
                                custEntity.PRESPOINTS = custRespEntity.Points;
                            }
                        }
                        else if (questionEntity.CUSTRESPTYPE.Equals("N"))
                        {
                            if (inputValue == string.Empty) continue;
                            custEntity.ACTNUMRESP = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                        }
                        else if (questionEntity.CUSTRESPTYPE.Equals("T"))
                        {
                            if (inputValue == string.Empty) continue;
                            custEntity.ACTDATERESP = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                        }
                        else if (questionEntity.CUSTRESPTYPE.Equals("C"))
                        {

                            //if (strmode.Equals("U"))
                            //{
                            //    custEntity.ACTALPHARESP = inputValue;
                            //    custEntity.ACTMULTRESP = inputValue;
                            //}
                            //else
                            //{
                            //    if (inputValue == string.Empty) continue;
                            //    custEntity.ACTALPHARESP = inputValue;
                            //    custEntity.ACTMULTRESP = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                            //}


                            if (strmode.Equals("U"))
                            {
                                custEntity.ACTALPHARESP = dataGridViewRow.Cells["gvtPResponse"].Tag == null ? string.Empty : dataGridViewRow.Cells["gvtPResponse"].Tag.ToString(); ;
                                custEntity.ACTMULTRESP = dataGridViewRow.Cells["gvtPResponse"].Tag == null ? string.Empty : dataGridViewRow.Cells["gvtPResponse"].Tag.ToString(); ;
                            }
                            else
                            {
                                if (dataGridViewRow.Cells["gvtPResponse"].Tag == null) continue;
                                else
                                {
                                    custEntity.ACTALPHARESP = dataGridViewRow.Cells["gvtPResponse"].Tag == null ? string.Empty : dataGridViewRow.Cells["gvtPResponse"].Tag.ToString(); ;
                                    custEntity.ACTMULTRESP = dataGridViewRow.Cells["gvtPResponse"].Tag == null ? string.Empty : dataGridViewRow.Cells["gvtPResponse"].Tag.ToString(); ;
                                }

                            }

                            int intPrespoint = 0;
                            var strresponcelist = custEntity.ACTALPHARESP.Split(',');
                            foreach (var item in strresponcelist)
                            {
                                CustRespEntity custRespEntity = propcustRespAllEntity.Find(u => u.DescCode.Trim().Equals(item.Trim()) && u.ResoCode.Trim().Equals(custEntity.ACTCODE.Trim()));
                                if (custRespEntity != null)
                                {
                                    intPrespoint = intPrespoint + Convert.ToInt16(custRespEntity.Points == string.Empty ? "0" : custRespEntity.Points);
                                }
                            }
                            custEntity.PRESPOINTS = intPrespoint.ToString();
                        }
                        else
                        {
                            if (strmode.Equals("U"))
                            {
                                custEntity.ACTALPHARESP = inputValue;
                            }
                            else
                            {
                                if (inputValue == string.Empty) continue;
                                custEntity.ACTALPHARESP = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                            }
                        }
                        if (dataGridViewRow.Cells[2].Tag is string)
                        {
                            custEntity.Mode = dataGridViewRow.Cells[2].Tag as string;
                        }


                        custEntity.addoperator = BaseForm.UserID;
                        custEntity.lstcoperator = BaseForm.UserID;
                        _model.CaseMstData.InsertUpdatePRESRESP(custEntity);
                    }
                }
            }
            DimensionCalculation(CaseSnpEntity);
        }

        private void cmbQuestionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbQuestionType.Items.Count > 0)
                fillPreassCustomQuestions();
        }

        private void btnDPoints_Click(object sender, EventArgs e)
        {
            List<CustomQuestionsEntity> customResponce = GetPreassEntityPoints();
            PreassesDimentionForm dimensionform = new PreassesDimentionForm(BaseForm, "Points", ((ListItem)cmbQuestionType.SelectedItem).Value.ToString(), customResponce);
            dimensionform.StartPosition = FormStartPosition.CenterScreen;
            dimensionform.ShowDialog();
        }


        private void DimensionCalculation(CaseSnpEntity CaseSnpEntity)
        {

            List<CustomQuestionsEntity> custQuestions = proppreassesQuestions;
            if (custQuestions != null)
            {
                if (((ListItem)cmbQuestionType.SelectedItem).Value.ToString().ToUpper() == "A")
                    custQuestions = custQuestions.FindAll(u => u.CUSTACTIVECUST.ToUpper() == "A");
                if (((ListItem)cmbQuestionType.SelectedItem).Value.ToString().ToUpper() == "I")
                    custQuestions = custQuestions.FindAll(u => u.CUSTACTIVECUST.ToUpper() == "I");

                List<CustomQuestionsEntity> custResponses = _model.CaseMstData.GetPreassesQuestionAnswers(CaseSnpEntity, "PRESRESP");
                decimal intDimepoints = 0, intTotalDimePoints = 0;
                CustomQuestionsEntity custEntity = new CustomQuestionsEntity();

                custEntity.ACTAGENCY = CaseSnpEntity.Agency;
                custEntity.ACTDEPT = CaseSnpEntity.Dept;
                custEntity.ACTPROGRAM = CaseSnpEntity.Program;
                if (CaseSnpEntity.Year == string.Empty)
                    custEntity.ACTYEAR = "    ";
                else
                    custEntity.ACTYEAR = CaseSnpEntity.Year;
                custEntity.ACTAPPNO = CaseSnpEntity.App;

                string strGroupCode = string.Empty;

                foreach (CommonEntity preassesdata in preassessMasterEntity)
                {
                    intDimepoints = 0;
                    List<PreassessQuesEntity> preassessChildList = preassessChildEntity.FindAll(u => u.PRECHILD_DID == preassesdata.Code);
                    bool boolQuestions = false;
                    foreach (PreassessQuesEntity preasschilddata in preassessChildList)
                    {
                        CustomQuestionsEntity dr = custQuestions.Find(u => u.CUSTCODE == preasschilddata.PRECHILD_QID);
                        if (dr != null)
                        {
                            boolQuestions = true;
                            CustomQuestionsEntity custResppoint = custResponses.Find(u => u.ACTCODE.Trim() == preasschilddata.PRECHILD_QID.Trim());
                            if (custResppoint != null)
                            {
                                intDimepoints = intDimepoints + Convert.ToDecimal(custResppoint.PRESPOINTS.ToString() == string.Empty ? "0" : custResppoint.PRESPOINTS);
                                if (strGroupCode == string.Empty)
                                    strGroupCode = dr.CUSTOTHER.ToString();
                            }
                        }
                    }
                    if (boolQuestions)
                    {
                        custEntity.ACTCODE = preassesdata.Code;
                        custEntity.PRESPOINTS = intDimepoints.ToString();
                        custEntity.Mode = string.Empty;
                        custEntity.addoperator = BaseForm.UserID;
                        custEntity.lstcoperator = BaseForm.UserID;
                        custEntity.CUSTOTHER = strGroupCode;
                        _model.CaseMstData.InsertUpdateDIMSCORE(custEntity);
                        intTotalDimePoints = intDimepoints + intTotalDimePoints;
                    }
                }

                custEntity.ACTCODE = string.Empty;
                custEntity.PRESPOINTS = intDimepoints.ToString();
                custEntity.Mode = string.Empty;
                custEntity.addoperator = BaseForm.UserID;
                custEntity.lstcoperator = BaseForm.UserID;
                custEntity.CUSTOTHER = strGroupCode;
                custEntity.Mode = "SCORETOTAL";
                _model.CaseMstData.InsertUpdateDIMSCORE(custEntity);
                if (BaseForm.BaseCaseMstListEntity.Count > 0)
                    BaseForm.BaseCaseMstListEntity[0].PressTotal = intTotalDimePoints.ToString();
            }
        }

        private List<CustomQuestionsEntity> GetPreassEntityPoints()
        {
            List<CustomQuestionsEntity> AllCustomQuestion = new List<CustomQuestionsEntity>();
            foreach (DataGridViewRow dataGridViewRow in gvwPreassesData.Rows)
            {
                if (dataGridViewRow.Tag is CustomQuestionsEntity)   //dataGridViewRow.Cells["Response"].Value != null && !dataGridViewRow.Cells["Response"].Value.ToString().Equals(string.Empty))
                {
                    string inputValue = string.Empty;
                    CustomQuestionsEntity questionEntity = dataGridViewRow.Tag as CustomQuestionsEntity;
                    if (questionEntity != null)
                    {
                        inputValue = dataGridViewRow.Cells["gvtPResponse"].Value != null ? dataGridViewRow.Cells["gvtPResponse"].Value.ToString() : string.Empty;
                        string strmode = dataGridViewRow.Cells[2].Tag == null ? string.Empty : dataGridViewRow.Cells[2].Tag.ToString();
                        if (dataGridViewRow.Cells[2].Tag == null && (dataGridViewRow.Cells[2].Tag != null && !((string)dataGridViewRow.Cells[2].Tag).Equals("U")))
                        {
                            continue;
                        }
                        CustomQuestionsEntity custEntity = new CustomQuestionsEntity();



                        custEntity.ACTCODE = questionEntity.CUSTCODE;
                        if (questionEntity.CUSTRESPTYPE.Equals("D"))
                        {
                            if (strmode.Equals("U"))
                            {
                                custEntity.ACTMULTRESP = dataGridViewRow.Cells["gvtPResponse"].Tag == null ? string.Empty : dataGridViewRow.Cells["gvtPResponse"].Tag.ToString();
                            }
                            else
                            {
                                if (dataGridViewRow.Cells["gvtPResponse"].Tag == null) continue;
                                custEntity.ACTMULTRESP = dataGridViewRow.Cells["gvtPResponse"].Tag.ToString().Trim();
                            }
                            CustRespEntity custRespEntity = propcustRespAllEntity.Find(u => u.DescCode.Trim().Equals(custEntity.ACTMULTRESP.Trim()) && u.ResoCode.Trim().Equals(custEntity.ACTCODE.Trim()));
                            if (custRespEntity != null)
                            {
                                custEntity.PRESPOINTS = custRespEntity.Points;
                            }
                        }
                        else if (questionEntity.CUSTRESPTYPE.Equals("N"))
                        {
                            if (inputValue == string.Empty) continue;
                            custEntity.ACTNUMRESP = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                        }
                        else if (questionEntity.CUSTRESPTYPE.Equals("T"))
                        {
                            if (inputValue == string.Empty) continue;
                            custEntity.ACTDATERESP = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                        }
                        else if (questionEntity.CUSTRESPTYPE.Equals("C"))
                        {

                            //if (strmode.Equals("U"))
                            //{
                            //    custEntity.ACTALPHARESP = inputValue;
                            //    custEntity.ACTMULTRESP = inputValue;
                            //}
                            //else
                            //{
                            //    if (inputValue == string.Empty) continue;
                            //    custEntity.ACTALPHARESP = inputValue;
                            //    custEntity.ACTMULTRESP = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                            //}


                            if (strmode.Equals("U"))
                            {
                                custEntity.ACTALPHARESP = dataGridViewRow.Cells["gvtPResponse"].Tag == null ? string.Empty : dataGridViewRow.Cells["gvtPResponse"].Tag.ToString(); ;
                                custEntity.ACTMULTRESP = dataGridViewRow.Cells["gvtPResponse"].Tag == null ? string.Empty : dataGridViewRow.Cells["gvtPResponse"].Tag.ToString(); ;
                            }
                            else
                            {
                                if (dataGridViewRow.Cells["gvtPResponse"].Tag == null) continue;
                                else
                                {
                                    custEntity.ACTALPHARESP = dataGridViewRow.Cells["gvtPResponse"].Tag == null ? string.Empty : dataGridViewRow.Cells["gvtPResponse"].Tag.ToString(); ;
                                    custEntity.ACTMULTRESP = dataGridViewRow.Cells["gvtPResponse"].Tag == null ? string.Empty : dataGridViewRow.Cells["gvtPResponse"].Tag.ToString(); ;
                                }

                            }

                            int intPrespoint = 0;
                            var strresponcelist = custEntity.ACTALPHARESP.Split(',');
                            foreach (var item in strresponcelist)
                            {
                                CustRespEntity custRespEntity = propcustRespAllEntity.Find(u => u.DescCode.Trim().Equals(item.Trim()) && u.ResoCode.Trim().Equals(custEntity.ACTCODE.Trim()));
                                if (custRespEntity != null)
                                {
                                    intPrespoint = intPrespoint + Convert.ToInt16(custRespEntity.Points == string.Empty ? "0" : custRespEntity.Points);
                                }
                            }
                            custEntity.PRESPOINTS = intPrespoint.ToString();
                        }
                        else
                        {
                            if (strmode.Equals("U"))
                            {
                                custEntity.ACTALPHARESP = inputValue;
                            }
                            else
                            {
                                if (inputValue == string.Empty) continue;
                                custEntity.ACTALPHARESP = inputValue; // dataGridViewRow.Cells["Response"].Value.ToString();
                            }
                        }
                        if (dataGridViewRow.Cells[2].Tag is string)
                        {
                            custEntity.Mode = dataGridViewRow.Cells[2].Tag as string;
                        }
                        AllCustomQuestion.Add(custEntity);
                    }
                }
            }
            return AllCustomQuestion;
        }

        #endregion

        private void cmbServicesInquired_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (cmbServicesInquired.Items.Count > 0)
                {
                    if (cmbServicesInquired.SelectedIndex != 0)
                    {
                        string strCode = ((Captain.Common.Utilities.ListItem)cmbServicesInquired.SelectedItem).Value == null ? string.Empty : ((Captain.Common.Utilities.ListItem)cmbServicesInquired.SelectedItem).Value.ToString();
                        foreach (DataGridViewRow item in gvwServices.Rows)
                        {
                            if (strCode.Trim() == Convert.ToString(item.Cells["ServiceCode"].Value).Trim())
                            {
                                int i = item.Index;
                                //gvwServices.FirstDisplayedScrollingRowIndex = i;
                                gvwServices.CurrentCell = gvwServices.Rows[i].Cells[0];
                                gvwServices.Rows[i].Selected = true;
                                break;

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void mskSSN_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnInactiveQues_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Are you sure you want to delete inactive Questions?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxInactiveHandler);
        }

        private void MessageBoxInactiveHandler(DialogResult dialogResult)
        {



            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                CustomQuestionsEntity custEntity = new CustomQuestionsEntity();
                //CustomQuestionsEntity questionEntity = gvwCustomQuestions.Tag as CustomQuestionsEntity;
                custEntity.ACTAGENCY = BaseForm.BaseAgency;
                custEntity.ACTDEPT = BaseForm.BaseDept;
                custEntity.ACTPROGRAM = BaseForm.BaseProg;
                custEntity.ACTYEAR = BaseForm.BaseYear;
                custEntity.ACTAPPNO = BaseForm.BaseApplicationNo;
                custEntity.ACTSNPFAMILYSEQ = "7777777";
                custEntity.ACTCODE = "PREASS";
                custEntity.ACTRESPSEQ = "1";
                custEntity.Mode = "DelAll";
                if (_model.CaseMstData.InsertUpdatePRESRESP(custEntity))
                {
                    cmbQuestionType.SelectedIndex = 1;
                    fillPreassCustomQuestions();
                    btnInactiveQues.Visible = false;
                }
            }
            else
            {
                IsExclude = false;
            }

        }

        private void cmbFamilyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (BaseForm.BaseAgencyControlDetails.FTypeSwitch == "Y")
                {
                    if (cmbFamilyType.Items.Count > 0)
                    {
                        if (((ListItem)cmbFamilyType.SelectedItem).Value.ToString() != "0")
                        {
                            if (!Mode.Equals(Consts.Common.Add))
                            {
                                int inthousehold = CaseMST.NoInProg.ToString() == string.Empty ? 0 : Convert.ToInt32(CaseMST.NoInProg);
                                decimal decimalfamilytype = ((ListItem)cmbFamilyType.SelectedItem).ScreenCode == null ? 0 : Convert.ToDecimal(((ListItem)cmbFamilyType.SelectedItem).ScreenCode);
                                if (decimalfamilytype > 0)
                                {
                                    string ScreenType = ((Utilities.ListItem)cmbFamilyType.SelectedItem).ScreenType == null ? "" : (((Utilities.ListItem)cmbFamilyType.SelectedItem).ScreenType);
                                    lblFamilyTypeWarning.Visible = false;
                                    switch (ScreenType)
                                    {

                                        case ">": if (inthousehold > decimalfamilytype) lblFamilyTypeWarning.Visible = false; else lblFamilyTypeWarning.Visible = true; break;
                                        case "=": if (decimalfamilytype == inthousehold) lblFamilyTypeWarning.Visible = false; else lblFamilyTypeWarning.Visible = true; break;
                                        case "<": if (inthousehold < decimalfamilytype) lblFamilyTypeWarning.Visible = false; else lblFamilyTypeWarning.Visible = true; break;
                                        default: lblFamilyTypeWarning.Visible = false; break;
                                    }

                                    //if (decimalfamilytype != inthousehold)
                                    //{
                                    //    lblFamilyTypeWarning.Visible = true;
                                    //}
                                    //else
                                    //{
                                    //    lblFamilyTypeWarning.Visible = false;
                                    //}
                                }
                            }

                        }
                    }
                }


            }
            catch (Exception ex)
            {


            }

        }


        private void Case3001Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Mode.Equals(Consts.Common.View))
            {
                if (string.IsNullOrEmpty(Refresh_Control))
                {
                    DialogResult result = MessageBox.Show("Are you sure want to close? Record not saved", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerFormClosed); ;
                    _case3001Form = (Wisej.Web.Form)sender;
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
        }

        Wisej.Web.Form _case3001Form;
        string Refresh_Control = string.Empty;
        private void MessageBoxHandlerFormClosed(DialogResult dialogResult)
        {
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                _case3001Form.FormClosing -= Case3001Form_FormClosing;
                _case3001Form.Close();
            }

        }

        private void ChkHome_Na_CheckedChanged(object sender, EventArgs e)
        {

            if (ChkHome_Na.Checked)
            {
                txtHomePhone.Text = string.Empty;
                txtHomePhone.Enabled = false;
                lblHomePhoneReq.Visible = false;
            }
            else
            {

                foreach (FldcntlHieEntity entity in CntlEntity)
                {
                    bool required = entity.Req.Equals("Y") ? true : false;
                    bool enabled = entity.Enab.Equals("Y") ? true : false;

                    switch (entity.FldCode)
                    {
                        case Consts.CASE2001.Phone:
                            if (enabled) { txtHomePhone.Enabled = true; } else { txtHomePhone.Enabled = false; }
                            if (required) { lblHomePhoneReq.Visible = true; } else { lblHomePhoneReq.Visible = false; }
                            break;
                    }
                }

            }
        }

        private void chkMessage_Na_CheckedChanged(object sender, EventArgs e)
        {

            if (chkMessage_Na.Checked)
            {
                txtMessage.Text = string.Empty;
                txtMessage.Enabled = false;
                lblMessageReq.Visible = false;
            }
            else
            {

                foreach (FldcntlHieEntity entity in CntlEntity)
                {
                    bool required = entity.Req.Equals("Y") ? true : false;
                    bool enabled = entity.Enab.Equals("Y") ? true : false;

                    switch (entity.FldCode)
                    {
                        case Consts.CASE2001.Message:
                            if (enabled) { txtMessage.Enabled = true; } else { txtMessage.Enabled = false; }
                            if (required) { lblMessageReq.Visible = true; } else { lblMessageReq.Visible = false; }
                            break;
                    }
                }

            }
        }

        private void chk_CellNa_CheckedChanged(object sender, EventArgs e)
        {

            if (chk_CellNa.Checked)
            {
                txtCell.Text = string.Empty;
                txtCell.Enabled = false;
                lblCellReq.Visible = false;
            }
            else
            {

                foreach (FldcntlHieEntity entity in CntlEntity)
                {
                    bool required = entity.Req.Equals("Y") ? true : false;
                    bool enabled = entity.Enab.Equals("Y") ? true : false;

                    switch (entity.FldCode)
                    {
                        case Consts.CASE2001.Cell:
                            if (enabled) { txtCell.Enabled = true; } else { txtCell.Enabled = false; }
                            if (required) { lblCellReq.Visible = true; } else { lblCellReq.Visible = false; }
                            break;
                    }
                }

            }
        }

        private void chkEmail_Na_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEmail_Na.Checked)
            {
                txtEmail.Text = string.Empty;
                txtEmail.Enabled = false;
                lblEmailReq.Visible = false;
            }
            else
            {

                foreach (FldcntlHieEntity entity in CntlEntity)
                {
                    bool required = entity.Req.Equals("Y") ? true : false;
                    bool enabled = entity.Enab.Equals("Y") ? true : false;

                    switch (entity.FldCode)
                    {
                        case Consts.CASE2001.Email:
                            if (enabled) { txtEmail.Enabled = true; } else { txtEmail.Enabled = false; }
                            if (required) { lblEmailReq.Visible = true; } else { lblEmailReq.Visible = false; }
                            break;
                    }
                }

            }
        }


        //bool familyTypeValidation(string strValue, int inthousehold)
        //{
        //    bool boolvalid = true;
        //    switch (strValue)
        //    {
        //        case "A":
        //        case "B":
        //        case "E":
        //        case "G":
        //        case "H":
        //            if (inthousehold != 2)
        //                boolvalid = false;
        //            break;

        //        case "C":
        //            if (inthousehold != 3)
        //                boolvalid = false;
        //            break;
        //        case "D":
        //        case "F":
        //        case "U":
        //            if (inthousehold != 1)
        //                boolvalid = false;
        //            break;

        //    }
        //    return boolvalid;

        //}

    }
}