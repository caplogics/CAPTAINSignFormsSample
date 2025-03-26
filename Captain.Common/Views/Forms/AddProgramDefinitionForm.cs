#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Captain.Common.Views.Forms.Base;
using Wisej.Web;
using Wisej.Design;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.UserControls;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class AddProgramDefinitionForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = new List<HierarchyEntity>();
        #endregion
        public AddProgramDefinitionForm(BaseForm baseForm, string mode, ProgramDefinitionEntity ProgramEntity, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();
            BaseForm = baseForm;
            PrivilegeEntity = privilegeEntity;
            _model = new CaptainModel();
            SelectedProgram = ProgramEntity;
            Mode = mode;
            this.Text = privilegeEntity.PrivilegeName + " - " + mode;
            fillDropDowns();
            setNumeric();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            cb30DaysDefault.Click += OnDefaultIncomeClick;
            cb60DaysDefault.Click += OnDefaultIncomeClick;
            cb90DaysDefault.Click += OnDefaultIncomeClick;
            picEditContact.Enabled = false;
            picDeleteContact.Enabled = false;
            ShowHidePrivilegeButtons();


            if (BaseForm.BaseAgencyControlDetails.PaymentCategorieService == "Y")
            {
                List<CommonEntity> PAYMENTService = _model.lookupDataAccess.GetPaymentCategoryServicee(); //CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00201", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);

                cmbPayCatservice.SelectedIndexChanged -= cmbPayCatservice_SelectedIndexChanged;


                cmbPayCatservice.Items.Insert(0, new ListItem("Select One", "0"));
                cmbPayCatservice.SelectedIndex = 0;

                foreach (CommonEntity servicedata in PAYMENTService)
                {
                    if (servicedata.Extension.Trim() == string.Empty)
                        cmbPayCatservice.Items.Add(new ListItem(servicedata.Desc, servicedata.Code));
                }
                btnPushpayment.Visible = true;
                cmbPayCatservice.Visible = true;
                lblPayemntserviceposting.Visible = true;

                cmbPayCatservice.SelectedIndexChanged += cmbPayCatservice_SelectedIndexChanged;
            }

            tabControl1.TabPages[1].Hidden = true;  // To Hide "Poverty GuideLines Tab"   Added By Yeswanth  07142012 

            if (Mode.Equals("Edit"))
            {
                txtProgram.TabStop = false;
                txtProgramName.TabStop = false;
                txtShortName.TabStop = false;
                txtYear.Focus();
                Agency = SelectedProgram.Agency;
                Dept = SelectedProgram.Dept;
                Program = SelectedProgram.Prog;
                fillProgramForm();
                txtProgram.ReadOnly = true;
                picProgram.Visible = false;
                fillMasterPoverty();
            }
            else
            {
                fillIncome(null);
                fillRelation(null);
            }
            CaseMstMaxApplNo = _model.CaseMstData.GetMaxApplicantNo(Agency, Dept, Program, txtYear.Text);

            if (Mode == "Add")
            {
                if (SelectedProgram != null)
                {
                    Agency = SelectedProgram.Agency;
                    Dept = SelectedProgram.Dept;
                    Program = SelectedProgram.Prog;

                    MessageBox.Show("Do you want to Copy Field Controls from " + SelectedProgram.Agency.Trim() + " - " + SelectedProgram.Dept.Trim() + " - " + SelectedProgram.Prog.Trim() + Consts.Common.TabSpace + SelectedProgram.ProgramName.Trim() + " ...?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Get_From_Sel_Hie);
                }
            }

            if (BaseForm.UserID.Trim().ToUpper() == "JAKE")
            {
                txtYear.Enabled = true;
                cbGenerateApp.Enabled = true;
                txtNextApp.Enabled = true;
            }
            else
            {
                txtYear.Enabled = false;
                cbGenerateApp.Enabled = false;
                txtNextApp.Enabled = false;
            }
        }

        
        List<PMTFLDCNTLHEntity> propPMTFLDCNTLHEntity = new List<PMTFLDCNTLHEntity>();
        public static TextBoxValidation CustomDecimalValidation
        {
            get
            {
                return new TextBoxValidation(@"^[0-9]\d{0,2}(\.\d{1,3})*(,\d+)?$", "Value must be between 0 - 99.999", "0-9\\.");
            }
        }

        public static TextBoxValidation CustomDecimalValidation8dot3
        {
            get
            {
                return new TextBoxValidation(@"^[0-9]\d{0,5}(\.\d{1,3})*(,\d+)?$", "Value must be between 0 - 99999.999", "0-9\\.");
            }
        }

        #region Properties
        public TagClass SelectedNodeTagClass { get; set; }

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity PrivilegeEntity { get; set; }

        public ProgramDefinitionEntity SelectedProgram { get; set; }

        public bool IsSaveValid { get; set; }

        public string Agency { get; set; }

        public string Dept { get; set; }

        public string Program { get; set; }

        public string Mode { get; set; }

        public int SelectedRowIndex { get; set; }

        public int SelectEnrollRowIndex { get; set; }
        public bool IsDefinitionTab { get; set; }

        public bool IsMasterTab { get; set; }

        public bool IsSwitchesTab { get; set; }

        public bool IsIncomeTab { get; set; }

        public bool IsARHSTab { get; set; }

        public long CaseMstMaxApplNo { get; set; }
        public string propDepSerpostPAYCAT { get; set; }
        #endregion

        private void setNumeric()
        {
            txtYear.Validator = TextBoxValidation.IntegerValidator;
            txtNextApp.Validator = TextBoxValidation.IntegerValidator;
            txtJuvAgeTo.Validator = TextBoxValidation.IntegerValidator;
            txtJuvAgeFrom.Validator = TextBoxValidation.IntegerValidator;
            txtSAgeTo.Validator = TextBoxValidation.IntegerValidator;
            txtSAgeFrom.Validator = TextBoxValidation.IntegerValidator;
            txtThreshold.Validator = TextBoxValidation.IntegerValidator;
            txt30DaysFactor.Validator = TextBoxValidation.CustomDecimalValidation;//new TextBoxValidation(@"String(value).match(/^[1-9]\d{0,4}(\.\d{1,3})*(,\d+)?$/ )", "Should Allow only this format 12345 or 12343.123", "0-9\\.");//TextBoxValidation.FloatValidator;
            txt60DaysFactor.Validator = TextBoxValidation.CustomDecimalValidation;
            txt90DaysFactor.Validator = TextBoxValidation.CustomDecimalValidation;
            txtFreeBF.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtFreeAMSnack.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtFreeLunch.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtFreePMSnack.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtFreeSupper.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtFreeOthers.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtReducedBF.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtReducedAMSnack.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtReducedLunch.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtReducedPMSnack.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtReducedSupper.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtReducedOthers.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtPaidAMSnack.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtPaidBF.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtPaidLunch.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtPaidPMSnack.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtPaidSupper.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtPaidOthers.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtOthersAMSnack.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtOthersBF.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtOthersLunch.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtOthersSupper.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtPaidAMSnack.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtOthers.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
        }

        private void fillProgramForm()
        {
            if (SelectedProgram != null)
            {
                ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(SelectedProgram.Agency, SelectedProgram.Dept, SelectedProgram.Prog);
                if (programEntity != null)
                {
                    DataSet emsclcpmcds = Captain.DatabaseLayer.SPAdminDB.CASE0025_GET(SelectedProgram.Agency, SelectedProgram.Dept, SelectedProgram.Prog, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "SINGLE", string.Empty, string.Empty);
                    if (emsclcpmcds != null && emsclcpmcds.Tables[0].Rows.Count > 0)
                    {
                        cmbPayCatservice.Enabled = false;
                    }

                    propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLH("CASE0063", programEntity.DepSerpostPAYCAT, programEntity.Agency + programEntity.Dept + programEntity.Prog, "hie");

                    propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CURR_GRP == "0" && u.PMFLDH_SP == "0" && u.PMFLDH_CATG == programEntity.DepSerpostPAYCAT);
                    propDepSerpostPAYCAT = programEntity.DepSerpostPAYCAT;

                    if (Mode == "Add")
                    {

                    }
                    else
                    {
                        txtProgram.Text = programEntity.Agency.Trim() + " - " + programEntity.AgencyName.Trim() + Consts.Common.TabSpace + programEntity.Dept.Trim() + " - " + programEntity.DeptName.Trim() + Consts.Common.TabSpace + programEntity.Prog.Trim() + " - " + programEntity.ProgramName.Trim();
                        txtProgramName.Text = programEntity.ProgramName;
                        txtShortName.Text = programEntity.ShortName;
                        txtYear.Text = programEntity.DepYear;
                    }
                    txtFname.Text = programEntity.DepFirstName;
                    txtLname.Text = programEntity.DepLastName;
                    txtMI.Text = programEntity.DepMI;
                    txtPhone.Text = programEntity.Phone.Trim();
                    txtFax.Text = programEntity.DepFax.Trim();
                    txtAddress1.Text = programEntity.Address1;
                    txtAddress2.Text = programEntity.Address2;
                    txtAddress3.Text = programEntity.Address3;
                    txtCity.Text = programEntity.City;
                    txtState.Text = programEntity.State;

                    txtEmail.Text = programEntity.emailID;

                    if (!programEntity.Zip.Equals(string.Empty))
                        txtZip.Text = "00000".Substring(0, 5 - programEntity.Zip.Length) + programEntity.Zip;
                    if (!programEntity.ZipPlus.Equals(string.Empty))
                        txtZipPlus.Text = "0000".Substring(0, 4 - programEntity.ZipPlus.Length) + programEntity.ZipPlus;

                    if (programEntity.DepGenerateApps.Equals("Y"))
                        cbGenerateApp.Checked = true;
                    else
                        cbGenerateApp.Checked = false;
                    if (!programEntity.DepAppNo.Equals(string.Empty))
                        txtNextApp.Text = "00000000".Substring(0, 8 - programEntity.DepAppNo.Length) + programEntity.DepAppNo.Trim();
                    if (BaseForm.BaseAgencyControlDetails.PaymentCategorieService == "Y")
                    {
                        cmbPayCatservice.SelectedIndexChanged -= cmbPayCatservice_SelectedIndexChanged;
                        CommonFunctions.SetComboBoxValue(cmbPayCatservice, programEntity.DepSerpostPAYCAT.ToString());
                        cmbPayCatservice.SelectedIndexChanged += cmbPayCatservice_SelectedIndexChanged;
                    }
                    cbIncomeVerification.Checked = programEntity.IncomeVerMsg.Equals("Y") ? true : false;
                    cbIncludeOnly.Checked = programEntity.IncomeTypeOnly.Equals("Y") ? true : false;
                    cbWeekly.Checked = programEntity.IncomeWeek.Equals("Y") ? true : false;
                    cbIntakeDate.Checked = programEntity.IntakeEdit.Equals("1") ? true : false;
                    cbPhohibitDuplicateSSN.Checked = programEntity.PRODUPSSN.Equals("Y") ? true : false;
                    if(cbPhohibitDuplicateSSN.Checked)
                    {
                        switch(programEntity.PRODUPAPP_BY)
                        {
                            case "1": rbAppSSN.Checked = true; break;
                            case "2": rbAppFN.Checked = true;break;
                            case "3": rbAppFNI.Checked = true; break;
                        }
                    }
                    cbProhibitDuplicateMember.Checked = programEntity.ProDupMEM.Equals("Y") ? true : false;
                    if (cbProhibitDuplicateMember.Checked)
                    {
                        switch (programEntity.PRODUPMEM_BY)
                        {
                            case "1": rbMemSSN.Checked = true; break;
                            case "2": rbMemFN.Checked = true; break;
                            case "3": rbMemFNI.Checked = true; break;
                        }
                    }
                    cbSMI.Checked = programEntity.DepSMIUsed.Equals("Y") ? true : false;
                    cbCMI.Checked = programEntity.DepCMIUsed.Equals("Y") ? true : false;
                    cbFedOMB.Checked = programEntity.DepFEDUsed.Equals("Y") ? true : false;
                    cbHUD.Checked = programEntity.DepHUDUsed.Equals("Y") ? true : false;
                    cbIncludeIncomeVer.Checked = programEntity.DepIncludeIncVer.Equals("1") ? true : false;
                    cbIntakeDate.Checked = programEntity.IntakeEdit.Equals("1") ? true : false;
                    cbAddress.Checked = programEntity.DepAddressEdit.Equals("Y") ? true : false;
                    cbAutoInactive.Checked = programEntity.AutoInActivation.Equals("1") ? true : false;
                    cbEditCaseTypes.Checked = programEntity.CaseTypeEdit.Equals("Y") ? true : false;
                    cbSecretProgram.Checked = programEntity.SecretProgram.Equals("Y") ? true : false;
                    cbUom.Checked = programEntity.DepUnitCalc.Equals("Y") ? true : false;
                    cbZip.Checked = programEntity.ZipSearch.Equals("Y") ? true : false;
                    cbSS.Checked = programEntity.SSNReasonFlag.Equals("Y") ? true : false;
                    cbEnroll.Checked = programEntity.SelectedClient.Equals("1") ? true : false;
                    chkAttendanceTime.Checked = programEntity.DepAttendanceTimes.Equals("Y") ? true : false;
                    chkIntakeProg.Checked = programEntity.DepIntakeProg.Equals("Y") ? true : false;
                    chkssnMask.Checked = false;
                    chkssnauto.Checked = false;
                    if (programEntity.DepIntakeProg.Equals("Y"))
                    {
                        //chkssnMask.Visible = true;
                        //chkssnauto.Visible = true;
                        //if (programEntity.DepSsnAutoAssign == "1")
                        //    chkssnMask.Checked = true;
                        //if (programEntity.DepSsnAutoAssign == "2")
                        //    chkssnauto.Checked = true;
                    }
                    else
                    {
                        chkssnMask.Visible = false;
                        chkssnauto.Visible = false;
                    }
                    if (cbIntakeDate.Checked)
                    {
                        if (!programEntity.IntakeFDate.Equals(string.Empty))
                            dtpFrom.Text = programEntity.IntakeFDate;
                        if (!programEntity.IntakeTDate.Equals(string.Empty))
                            dtpTo.Text = programEntity.IntakeTDate;
                    }
                    txtJuvAgeFrom.Text = programEntity.DepJUVFromAge.ToString().Trim();
                    txtJuvAgeTo.Text = programEntity.DepJUVToAge.ToString().Trim();
                    txtSAgeFrom.Text = programEntity.DepSENFromAge.ToString().Trim();
                    txtSAgeTo.Text = programEntity.DepSENToAge.ToString().Trim();
                    txtThreshold.Text = programEntity.DepThreshold.ToString().Trim();
                    if (programEntity.DepAutoRefer.Equals("Y"))
                    {
                        rbSystem.Checked = true;
                    }
                    else
                    {
                        rbManual.Checked = true;
                    }
                    cbPoints.Checked = programEntity.SIMPointsMethod.Equals("Y") ? true : false;

                    cbDateValidateIncome.Checked = programEntity.IncomeDateValidate.Equals("1") ? true : false;
                    cb30DaysDefault.Checked = programEntity.DepIncExcIntDefault1.Equals("1") ? true : false;
                    cb60DaysDefault.Checked = programEntity.DepIncExcIntDefault2.Equals("1") ? true : false;
                    cb90DaysDefault.Checked = programEntity.DepIncExcIntDefault3.Equals("1") ? true : false;
                    cb30DaysUsed.Checked = programEntity.DepIncExcIntUsed1.Equals("1") ? true : false;
                    cb60DaysUsed.Checked = programEntity.DepIncExcIntUsed2.Equals("1") ? true : false;
                    cb90DaysUsed.Checked = programEntity.DepIncExcIntUsed3.Equals("1") ? true : false;
                    txt30DaysFactor.Text = programEntity.DepIncExcIntFactr1.ToString().Trim();
                    txt60DaysFactor.Text = programEntity.DepIncExcIntFactr2.ToString().Trim();
                    txt90DaysFactor.Text = programEntity.DepIncExcIntFactr3.ToString().Trim();

                    if (!programEntity.Account.Equals(string.Empty))
                        maskedTextBoxDefAcct.Text = "00000000000000".Substring(0, 14 - programEntity.Account.Trim().Length) + programEntity.Account.Trim();

                    txtFreeBF.Text = programEntity.Free1.ToString();
                    txtFreeAMSnack.Text = programEntity.Free2.ToString();
                    txtFreeLunch.Text = programEntity.Free3.ToString();
                    txtFreePMSnack.Text = programEntity.Free4.ToString();
                    txtFreeSupper.Text = programEntity.Free5.ToString();
                    txtFreeOthers.Text = programEntity.Free6.ToString();

                    txtReducedBF.Text = programEntity.Reduced1.ToString();
                    txtReducedAMSnack.Text = programEntity.Reduced2.ToString();
                    txtReducedLunch.Text = programEntity.Reduced3.ToString();
                    txtReducedPMSnack.Text = programEntity.Reduced4.ToString();
                    txtReducedSupper.Text = programEntity.Reduced5.ToString();
                    txtReducedOthers.Text = programEntity.Reduced6.ToString();

                    txtPaidBF.Text = programEntity.Paid1.ToString();
                    txtPaidAMSnack.Text = programEntity.Paid2.ToString();
                    txtPaidLunch.Text = programEntity.Paid3.ToString();
                    txtPaidPMSnack.Text = programEntity.Paid4.ToString();
                    txtPaidSupper.Text = programEntity.Paid5.ToString();
                    txtPaidOthers.Text = programEntity.Paid6.ToString();
                    if (programEntity.DepHsAgeMethod == "S")
                        rdoSchool.Checked = true;
                    else
                        rdoZip.Checked = true;

                    //if (programEntity.Search_Cntrl == "F")
                    //    rdbFNDOBSSN.Checked = true;
                    //else if (programEntity.Search_Cntrl == "C")
                    //    rdbClientID.Checked = true;

                    fillIncome(programEntity.DepIncomeTypes);
                    fillRelation(programEntity.DepReleataionTypes);

                    SetComboBoxValue(cmbStartMonth, programEntity.StartMonth);
                    SetComboBoxValue(cmbEndMonth, programEntity.EndMonth);
                    SetComboBoxValue(cmbCustomerType, programEntity.Type);
                    SetComboBoxValue(cmbInvoiceSource, programEntity.Source);
                    SetComboBoxValue(cmbDefaultQuestions, programEntity.LoadProgramQuestions);
                    gvwHiearchys.Rows.Clear();
                    fillEnrlHieracheis(programEntity.Agency.ToString(), programEntity.Dept, programEntity.Prog);
                    RefreshContactGrid();
                }
            }
        }

        private void Get_From_Sel_Hie(DialogResult dialogresult)
        {
            if (dialogresult == DialogResult.Yes)
            {
                fillProgramForm();
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

        private void fillIncome(string incomes)
        {
            List<string> incomeList = new List<string>();
            if (incomes != null)
            {
                string[] incomeTypes = incomes.Split(' ');
                for (int i = 0; i < incomeTypes.Length; i++)
                {
                    incomeList.Add(incomeTypes.GetValue(i).ToString());
                }
            }

            List<AgyTabEntity> lookUpIncomeTypes = _model.lookupDataAccess.GetIncomeTypes();
            gvwIncomeTypes.Rows.Clear();
            foreach (AgyTabEntity agyEntity in lookUpIncomeTypes)
            {
                string flag = "false";
                if (incomes != null && incomeList.Contains(agyEntity.agycode))
                {
                    flag = "true";
                }
                int rowIndex = gvwIncomeTypes.Rows.Add(flag, agyEntity.agydesc);
                gvwIncomeTypes.Rows[rowIndex].Tag = agyEntity;
            }
        }

        private void fillRelation(string relations)
        {
            List<string> relationList = new List<string>();
            if (relations != null)
            {
                string[] relationTypes = relations.Split(' ');
                for (int i = 0; i < relationTypes.Length; i++)
                {
                    relationList.Add(relationTypes.GetValue(i).ToString());
                }
            }
            List<CommonEntity> lookUps = _model.lookupDataAccess.GetRelationship();
            gvwRelationShip.Rows.Clear();
            foreach (CommonEntity agyEntity in lookUps)
            {
                string flag = "false";
                if (relations != null && relationList.Contains(agyEntity.Code))
                {
                    flag = "true";
                }
                int rowIndex = gvwRelationShip.Rows.Add(flag, agyEntity.Desc);
                gvwRelationShip.Rows[rowIndex].Tag = agyEntity;
            }

        }

        private void fillEnrlHieracheis(string Agency, string Dept, string Program)
        {
            pbEditEnroll.Enabled = false;
            pbDeleteEnroll.Enabled = false;
            List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetCaseHierarchy("ALL", string.Empty, string.Empty, string.Empty, BaseForm.BaseAdminAgency == "**" ? string.Empty : BaseForm.BaseAdminAgency);
            List<DepEnrollHierachiesEntity> DepEnrollList = _model.HierarchyAndPrograms.GetDepEntollHierachies(Agency, Dept, Program, string.Empty);
            gvwHiearchys.Rows.Clear();
            foreach (HierarchyEntity hieEntity in caseHierarchy)
            {
                foreach (DepEnrollHierachiesEntity item in DepEnrollList)
                {
                    if (item.Hierachies == hieEntity.Agency + hieEntity.Dept + hieEntity.Prog)
                    {
                        int rowIndex = gvwHiearchys.Rows.Add(hieEntity.Code, item.Nofoslots.ToString(), LookupDataAccess.Getdate(item.StartDate), LookupDataAccess.Getdate(item.Enddate), hieEntity.Agency + hieEntity.Dept + hieEntity.Prog);
                        item.DepEnrollCode = hieEntity.Code;
                        item.DepEnrollDesc = hieEntity.HirarchyName.ToString();
                        item.Mode = "Update";
                        gvwHiearchys.Rows[rowIndex].Tag = item;
                    }
                }


            }

            if (gvwHiearchys.Rows.Count > 0)
            {
                gvwHiearchys.CurrentCell = gvwHiearchys.Rows[0].Cells[1];
                gvwHiearchys.Rows[0].Selected = true;

                pbEditEnroll.Enabled = true;
                pbDeleteEnroll.Enabled = true;

                if (PrivilegeEntity.ChangePriv.Equals("false"))
                {
                    pbEditEnroll.Visible = false;
                }
                else
                {
                    pbEditEnroll.Visible = true;
                }

                if (PrivilegeEntity.DelPriv.Equals("false"))
                {
                    pbDeleteEnroll.Visible = false;
                }
                else
                {
                    pbDeleteEnroll.Visible = true;
                }

            }
            else
            {
                pbEditEnroll.Visible = false;
                pbDeleteEnroll.Visible = false;
            }

        }

        private void fillDropDowns()
        {
            List<CommonEntity> commonEntity = _model.lookupDataAccess.GetMonths();
            foreach (CommonEntity month in commonEntity)
            {
                cmbStartMonth.Items.Add(new ListItem(month.Desc, month.Code));
            }
            cmbStartMonth.Items.Insert(0, new ListItem("Select One", "0"));
            cmbStartMonth.SelectedIndex = 0;

            foreach (CommonEntity month in commonEntity)
            {
                cmbEndMonth.Items.Add(new ListItem(month.Desc, month.Code));
            }
            cmbEndMonth.Items.Insert(0, new ListItem("Select One", "0"));
            cmbEndMonth.SelectedIndex = 0;

            commonEntity = _model.lookupDataAccess.GetDefaultQuestions();
            foreach (CommonEntity questions in commonEntity)
            {
                cmbDefaultQuestions.Items.Add(new ListItem(questions.Desc, questions.Code));
            }
            //cmbDefaultQuestions.Items.Insert(0, new ListItem("Select One", "0"));
            cmbDefaultQuestions.SelectedIndex = 0;

            List<AgyTabEntity> lookUpCustomerTypes = _model.lookupDataAccess.GetCustomerTypes();
            foreach (AgyTabEntity customerType in lookUpCustomerTypes)
            {
                cmbCustomerType.Items.Add(new ListItem(customerType.agydesc, customerType.agycode));
            }

            cmbCustomerType.Items.Insert(0, new ListItem("Select One", "0"));
            cmbCustomerType.SelectedIndex = 0;

            List<AgyTabEntity> lookUpInvoiceSource = _model.lookupDataAccess.GetInvoiceSource();
            foreach (AgyTabEntity invoiceSource in lookUpInvoiceSource)
            {
                cmbInvoiceSource.Items.Add(new ListItem(invoiceSource.agydesc, invoiceSource.agycode));
            }
            cmbInvoiceSource.Items.Insert(0, new ListItem("Select One", "0"));
            cmbInvoiceSource.SelectedIndex = 0;
        }

        private void fillMasterPoverty()
        {
            if (Agency != null)
            {
                List<MasterPovertyEntity> modelPovertyList = _model.masterPovertyData.GetCaseGdlByHIE(Agency, Dept, Program);
                gvwMasterPoverty.Rows.Clear();
                foreach (MasterPovertyEntity hierarchy in modelPovertyList)
                {
                    string code = hierarchy.GdlAgency + "-" + hierarchy.GdlDept + "-" + hierarchy.GdlProgram;
                    int rowIndex = gvwMasterPoverty.Rows.Add(hierarchy.GdlType, hierarchy.GdlStartDate.ToString(), hierarchy.GdlEndDate, code);
                    setTooltip(rowIndex, hierarchy.GdlAddOperator, hierarchy.GdlDateAdd, hierarchy.GdlLstcOperator, hierarchy.GdlDateLstc, gvwMasterPoverty);
                }
            }
        }

        public void RefreshContactGrid()
        {
            DepContactEntity caseDepContactEntity = new DepContactEntity();
            caseDepContactEntity.Agency = Agency;
            caseDepContactEntity.Dept = Dept;
            caseDepContactEntity.Program = Program;
            picEditContact.Enabled = false;
            pbEditEnroll.Enabled = false;
            pbDeleteEnroll.Enabled = false;
            picDeleteContact.Enabled = false;
            List<DepContactEntity> contactEntity = _model.HierarchyAndPrograms.GetCASEDEPContacts(caseDepContactEntity);
            gvwContactInfo.Rows.Clear();
            foreach (DepContactEntity contEntity in contactEntity)
            {
                int rowIndex = gvwContactInfo.Rows.Add(contEntity.FirstName.Trim() + " " + contEntity.LastName.Trim(), CommonFunctions.GetPhoneNo(contEntity.Phone1.Trim()));
                contEntity.Mode = "U";
                gvwContactInfo.Rows[rowIndex].Tag = contEntity;
            }
            if (contactEntity.Count > 0)
            {
                picEditContact.Enabled = true;
                picDeleteContact.Enabled = true;
            }
            if (gvwHiearchys.Rows.Count > 0)
            {
                pbEditEnroll.Enabled = true;
                pbDeleteEnroll.Enabled = true;
            }
            if (gvwContactInfo.Rows.Count > 0)
            {
                gvwContactInfo.CurrentCell = gvwContactInfo.Rows[0].Cells[1];
                gvwContactInfo.Rows[0].Selected = true;
                if (PrivilegeEntity.ChangePriv.Equals("false"))
                {
                    picEditContact.Visible = false;

                }
                else
                {
                    picEditContact.Visible = true;
                }

                if (PrivilegeEntity.DelPriv.Equals("false"))
                {
                    picDeleteContact.Visible = false;
                }
                else
                {
                    picDeleteContact.Visible = true;
                }

            }
            else
            {
                picEditContact.Visible = false;
                picDeleteContact.Visible = false;
            }


        }

        private void setTooltip(int rowIndex, string addOperator, string addDate, string updateOperator, string updateDate, DataGridView datagridview)
        {
            string toolTipText = "Added By     : " + addOperator.Trim() + " on " + addDate.ToString() + "\n";
            string modifiedBy = string.Empty;
            if (!updateOperator.ToString().Trim().Equals(string.Empty))
                modifiedBy = updateOperator.ToString().Trim() + " on " + updateDate.ToString();
            toolTipText += "Modified By : " + modifiedBy;

            foreach (DataGridViewCell cell in datagridview.Rows[rowIndex].Cells)
            {
                cell.ToolTipText = toolTipText;
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;
            IsDefinitionTab = false;
            IsMasterTab = false;
            IsSwitchesTab = false;
            IsIncomeTab = false;
            IsARHSTab = false;

           
            if (txtFax.Text != "" && txtFax.Text != "   -   -")
            {
                if (txtFax.Text.Trim().Replace("-", "").Replace(" ", "").Length < 10)
                {
                    _errorProvider.SetError(txtFax, "Please enter valid Fax Number");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtFax, null);
                }
            }

            if (String.IsNullOrEmpty(txtProgram.Text.Trim()))
            {
                _errorProvider.SetError(txtProgram, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblProgramHIE.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
                IsDefinitionTab = true;
            }
            else
            {
                _errorProvider.SetError(txtProgram, null);
            }

            if (BaseForm.UserID.Trim().ToUpper() == "JAKE")
            {
                if (!String.IsNullOrEmpty(txtYear.Text.Trim()))
                {
                    if (!CommonFunctions.IsNumeric(txtYear.Text))
                    {
                        _errorProvider.SetError(txtYear, string.Format(Consts.Messages.NumericOnly.GetMessage(), "Year"));
                        isValid = false;
                        IsDefinitionTab = true;
                    }
                    else
                    {
                        int year = int.Parse(txtYear.Text.ToString());
                        if (!(year >= 2000 && year <= 2100))
                        {
                            _errorProvider.SetError(txtYear, "Year should be between 2000 to 2100");
                            isValid = false;
                            IsDefinitionTab = true;
                        }
                        else
                        {
                            _errorProvider.SetError(txtYear, null);
                        }
                    }
                }
            }
            
            if (String.IsNullOrEmpty(txtPhone.Text.Trim()))
            {
                _errorProvider.SetError(txtPhone, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPhone.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
                IsDefinitionTab = true;

               
            }
            else
            {
               // _errorProvider.SetError(txtPhone, null);
                if (txtPhone.Text != "" && txtPhone.Text != "   -   -")
                {
                    if (txtPhone.Text.Trim().Replace("-", "").Replace(" ", "").Length < 10)
                    {
                        _errorProvider.SetError(txtPhone, "Please enter valid Phone Number");
                        isValid = false;
                        IsDefinitionTab = true;
                    }
                    else
                    {
                        _errorProvider.SetError(txtPhone, null);
                    }
                }
                else
                {
                    _errorProvider.SetError(txtPhone, null);
                }
            }

            if (String.IsNullOrEmpty(txtAddress1.Text.Trim()))
            {
                _errorProvider.SetError(txtAddress1, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAddress.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
                IsDefinitionTab = true;
            }
            else
            {
                _errorProvider.SetError(txtAddress1, null);
            }
            if (String.IsNullOrEmpty(txtCity.Text.Trim()))
            {
                _errorProvider.SetError(txtCity, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCityTown.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
                IsDefinitionTab = true;
            }
            else
            {
                _errorProvider.SetError(txtCity, null);
            }
            if (String.IsNullOrEmpty(txtZip.Text.Trim()))
            {
                _errorProvider.SetError(txtZip, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblZipCode.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
                IsDefinitionTab = true;
            }
            else
            {
                _errorProvider.SetError(txtZip, null);
            }
            if (!String.IsNullOrEmpty(txtState.Text.Trim()))
            {
                if (!CommonFunctions.IsAlpha(txtState.Text))
                {
                    _errorProvider.SetError(txtState, string.Format("Please Enter Alpha letters only.", "State"));
                    isValid = false;
                    IsDefinitionTab = true;
                }
                else if (txtState.Text.Trim().Length != 2)
                {
                    _errorProvider.SetError(txtState, string.Format("State should be two characters.", "State"));
                    isValid = false;
                    IsDefinitionTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtState, null);
                }
            }
            _errorProvider.SetError(btnPushpayment, null);

            if (BaseForm.BaseAgencyControlDetails.PaymentCategorieService == "Y")
            {
                if(cmbPayCatservice.Items.Count>0)
                {
                    if (!((ListItem)cmbPayCatservice.SelectedItem).Value.ToString().Equals("0"))
                    {
                        if (propPMTFLDCNTLHEntity.Count == 0)
                        {

                            _errorProvider.SetError(btnPushpayment, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPayemntserviceposting.Text.Replace(Consts.Common.Colon, string.Empty)));
                            isValid = false;
                            IsDefinitionTab = true;


                        }
                        else
                        {
                            if (propDepSerpostPAYCAT != ((ListItem)cmbPayCatservice.SelectedItem).Value.ToString())
                            {
                                if (propPMTFLDCNTLHEntity.Count > 0)
                                {
                                    List<PMTFLDCNTLHEntity> categoryFLDHENTIY = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG.Trim() == ((ListItem)cmbPayCatservice.SelectedItem).Value.ToString());
                                    if (categoryFLDHENTIY.Count == 0)
                                    {
                                        _errorProvider.SetError(btnPushpayment, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPayemntserviceposting.Text.Replace(Consts.Common.Colon, string.Empty)));
                                        isValid = false;
                                        IsDefinitionTab = true;
                                    }
                                }

                            }

                        }
                    }
                }
            }

            if (BaseForm.UserID.Trim().ToUpper() == "JAKE")
            {
                if (cbGenerateApp.Checked)
                {
                    if (String.IsNullOrEmpty(txtNextApp.Text.Trim()))
                    {
                        _errorProvider.SetError(txtNextApp, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Next App"));
                        isValid = false;
                        IsDefinitionTab = true;
                    }
                    else
                    {
                        if (!CommonFunctions.IsNumeric(txtNextApp.Text))
                        {
                            _errorProvider.SetError(txtNextApp, string.Format(Consts.Messages.NumericOnly.GetMessage(), "Next App"));
                            isValid = false;
                            IsDefinitionTab = true;
                        }
                        else
                        {
                            _errorProvider.SetError(txtNextApp, null);
                        }
                    }
                }
            }

            if (!String.IsNullOrEmpty(txtJuvAgeTo.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtJuvAgeTo.Text))
                {
                    _errorProvider.SetError(txtJuvAgeTo, string.Format(Consts.Messages.NumericOnly.GetMessage(), "JuvAgeTo"));
                    isValid = false;
                    IsSwitchesTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtJuvAgeTo, null);
                }
            }
            if (!String.IsNullOrEmpty(txtJuvAgeFrom.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtJuvAgeFrom.Text))
                {
                    _errorProvider.SetError(txtJuvAgeFrom, string.Format(Consts.Messages.NumericOnly.GetMessage(), "JuvAgeFrom"));
                    isValid = false;
                    IsSwitchesTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtJuvAgeFrom, null);
                }
            }
            if (!String.IsNullOrEmpty(txtSAgeTo.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtSAgeTo.Text))
                {
                    _errorProvider.SetError(txtSAgeTo, string.Format(Consts.Messages.NumericOnly.GetMessage(), "SAgeTo"));
                    isValid = false;
                    IsSwitchesTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtSAgeTo, null);
                }
            }
            if (!String.IsNullOrEmpty(txtSAgeFrom.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtSAgeFrom.Text))
                {
                    _errorProvider.SetError(txtSAgeFrom, string.Format(Consts.Messages.NumericOnly.GetMessage(), "Senior Age From"));
                    isValid = false;
                    IsSwitchesTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtSAgeFrom, null);
                }
            }
            if (!String.IsNullOrEmpty(txtThreshold.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtThreshold.Text))
                {
                    _errorProvider.SetError(txtThreshold, string.Format(Consts.Messages.NumericOnly.GetMessage(), "Threshold"));
                    isValid = false;
                    IsSwitchesTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtThreshold, null);
                }
            }

            if (!String.IsNullOrEmpty(txt30DaysFactor.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txt30DaysFactor.Text))
                {
                    _errorProvider.SetError(txt30DaysFactor, "30 Days Factor Should be Number");
                    isValid = false;
                    IsIncomeTab = true;
                }
                else
                {
                    _errorProvider.SetError(txt30DaysFactor, null);
                }
            }
            if (!String.IsNullOrEmpty(txt60DaysFactor.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txt60DaysFactor.Text))
                {
                    _errorProvider.SetError(txt60DaysFactor, "60 Days Factor Should be Number");
                    isValid = false;
                    IsIncomeTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtFreeBF, null);
                }
            }
            if (!String.IsNullOrEmpty(txt90DaysFactor.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txt90DaysFactor.Text))
                {
                    _errorProvider.SetError(txt90DaysFactor, "90 Days Factor Should be Number");
                    isValid = false;
                    IsIncomeTab = true;
                }
                else
                {
                    _errorProvider.SetError(txt90DaysFactor, null);
                }
            }

            if (!String.IsNullOrEmpty(txtFreeBF.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtFreeBF.Text))
                {
                    _errorProvider.SetError(txtFreeBF, string.Format(Consts.Messages.NumericOnly.GetMessage(), "FreeBF"));
                    isValid = false;
                    IsARHSTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtFreeBF, null);
                }
            }

            if (!String.IsNullOrEmpty(txtFreeAMSnack.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtFreeAMSnack.Text))
                {
                    _errorProvider.SetError(txtFreeAMSnack, string.Format(Consts.Messages.NumericOnly.GetMessage(), "FreeAMSnack"));
                    isValid = false;
                    IsARHSTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtFreeAMSnack, null);
                }
            }

            if (!String.IsNullOrEmpty(txtFreeLunch.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtFreeLunch.Text))
                {
                    _errorProvider.SetError(txtFreeLunch, string.Format(Consts.Messages.NumericOnly.GetMessage(), "FreeLunch"));
                    isValid = false;
                    IsARHSTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtFreeLunch, null);
                }
            }

            if (!String.IsNullOrEmpty(txtFreePMSnack.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtFreePMSnack.Text))
                {
                    _errorProvider.SetError(txtFreePMSnack, string.Format(Consts.Messages.NumericOnly.GetMessage(), "FreePMSnack"));
                    isValid = false;
                    IsARHSTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtFreePMSnack, null);
                }
            }

            if (!String.IsNullOrEmpty(txtFreeSupper.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtFreeSupper.Text))
                {
                    _errorProvider.SetError(txtFreeSupper, string.Format(Consts.Messages.NumericOnly.GetMessage(), "FreeSupper"));
                    isValid = false;
                    IsARHSTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtFreeSupper, null);
                }
            }

            if (!String.IsNullOrEmpty(txtFreeOthers.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtFreeOthers.Text))
                {
                    _errorProvider.SetError(txtFreeOthers, string.Format(Consts.Messages.NumericOnly.GetMessage(), "FreeOthers"));
                    isValid = false;
                    IsARHSTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtFreeOthers, null);
                }
            }

            if (!String.IsNullOrEmpty(txtReducedBF.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtReducedBF.Text))
                {
                    _errorProvider.SetError(txtReducedBF, string.Format(Consts.Messages.NumericOnly.GetMessage(), "ReducedBF"));
                    isValid = false;
                    IsARHSTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtReducedBF, null);
                }
            }

            if (!String.IsNullOrEmpty(txtReducedAMSnack.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtReducedAMSnack.Text))
                {
                    _errorProvider.SetError(txtReducedAMSnack, string.Format(Consts.Messages.NumericOnly.GetMessage(), "ReducedAMSnack"));
                    isValid = false;
                    IsARHSTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtReducedAMSnack, null);
                }
            }

            if (!String.IsNullOrEmpty(txtReducedLunch.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtReducedLunch.Text))
                {
                    _errorProvider.SetError(txtReducedLunch, string.Format(Consts.Messages.NumericOnly.GetMessage(), "ReducedLunch"));
                    isValid = false;
                    IsARHSTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtReducedLunch, null);
                }
            }
            if (!String.IsNullOrEmpty(txtReducedPMSnack.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtReducedPMSnack.Text))
                {
                    _errorProvider.SetError(txtReducedPMSnack, string.Format(Consts.Messages.NumericOnly.GetMessage(), "ReducedPMSnack"));
                    isValid = false;
                    IsARHSTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtReducedPMSnack, null);
                }
            }

            if (!String.IsNullOrEmpty(txtReducedSupper.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtReducedSupper.Text))
                {
                    _errorProvider.SetError(txtReducedSupper, string.Format(Consts.Messages.NumericOnly.GetMessage(), "ReducedSupper"));
                    isValid = false;
                    IsARHSTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtReducedSupper, null);
                }
            }

            if (!String.IsNullOrEmpty(txtReducedOthers.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtReducedOthers.Text))
                {
                    _errorProvider.SetError(txtReducedOthers, string.Format(Consts.Messages.NumericOnly.GetMessage(), "ReducedOthers"));
                    isValid = false;
                    IsARHSTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtReducedOthers, null);
                }
            }

            if (!String.IsNullOrEmpty(txtPaidBF.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtPaidBF.Text))
                {
                    _errorProvider.SetError(txtPaidBF, string.Format(Consts.Messages.NumericOnly.GetMessage(), "PaidBF"));
                    isValid = false;
                    IsARHSTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtPaidBF, null);
                }
            }
            if (!String.IsNullOrEmpty(txtPaidAMSnack.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtPaidAMSnack.Text))
                {
                    _errorProvider.SetError(txtPaidAMSnack, string.Format(Consts.Messages.NumericOnly.GetMessage(), "PaidAMSnack"));
                    isValid = false;
                    IsARHSTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtPaidAMSnack, null);
                }
            }
            if (!String.IsNullOrEmpty(txtPaidLunch.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtPaidLunch.Text))
                {
                    _errorProvider.SetError(txtPaidLunch, string.Format(Consts.Messages.NumericOnly.GetMessage(), "PaidLunch"));
                    isValid = false;
                    IsARHSTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtPaidLunch, null);
                }
            }

            if (!String.IsNullOrEmpty(txtPaidPMSnack.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtPaidPMSnack.Text))
                {
                    _errorProvider.SetError(txtPaidPMSnack, string.Format(Consts.Messages.NumericOnly.GetMessage(), "PaidPMSnack"));
                    isValid = false;
                    IsARHSTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtPaidPMSnack, null);
                }
            }
            if (!String.IsNullOrEmpty(txtPaidSupper.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtPaidSupper.Text))
                {
                    _errorProvider.SetError(txtPaidSupper, string.Format(Consts.Messages.NumericOnly.GetMessage(), "PaidSupper"));
                    isValid = false;
                    IsARHSTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtPaidSupper, null);
                }
            }
            if (!String.IsNullOrEmpty(txtPaidOthers.Text.Trim()))
            {
                if (!CommonFunctions.IsNumeric(txtPaidOthers.Text))
                {
                    _errorProvider.SetError(txtPaidOthers, string.Format(Consts.Messages.NumericOnly.GetMessage(), "PaidOthers"));
                    isValid = false;
                    IsARHSTab = true;
                }
                else
                {
                    _errorProvider.SetError(txtPaidOthers, null);
                }
            }

            if (!string.IsNullOrEmpty(txtJuvAgeFrom.Text.Trim()) && !string.IsNullOrEmpty(txtJuvAgeTo.Text.Trim()))
            {
                if (int.Parse(txtJuvAgeFrom.Text) > int.Parse(txtJuvAgeTo.Text))
                {
                    _errorProvider.SetError(txtJuvAgeTo, "Juvenile Age From Should Be Less Than Juvenile Age To");
                    isValid = false;
                    IsSwitchesTab = true;
                }
            }

            if (!string.IsNullOrEmpty(txtSAgeFrom.Text.Trim()) && !string.IsNullOrEmpty(txtSAgeTo.Text.Trim()))
            {
                if (int.Parse(txtSAgeFrom.Text) > int.Parse(txtSAgeTo.Text))
                {
                    _errorProvider.SetError(txtSAgeTo, "Senior Age From Should Be Less Than Senior Age To");
                    isValid = false;
                    IsSwitchesTab = true;
                }
            }
            if(cbIntakeDate.Checked)
            {
                    if (dtpFrom.Checked == false)
                    {
                        _errorProvider.SetError(dtpFrom, "Please select Intake From Date");
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpFrom, null);
                    }
                    if (dtpTo.Checked == false)
                    {
                        _errorProvider.SetError(dtpTo, "Please select Intake To Date");
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpTo, null);
                    }
            }

            if (cbIntakeDate.Checked == true)
            {
                if (dtpFrom.Checked.Equals(true) && dtpTo.Checked.Equals(true))
                {
                    if (string.IsNullOrWhiteSpace(dtpFrom.Text))
                    {
                        _errorProvider.SetError(dtpFrom, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Intake From Date".Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpFrom, null);
                    }
                    if (string.IsNullOrWhiteSpace(dtpTo.Text))
                    {
                        _errorProvider.SetError(dtpTo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Intake To Date".Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpTo, null);
                    }
                }
            }

            if (cbIntakeDate.Checked && !string.IsNullOrEmpty(dtpFrom.Text.Trim()) && !string.IsNullOrEmpty(dtpTo.Text.Trim()))
            {
                if (dtpFrom.Checked.Equals(true) && dtpTo.Checked.Equals(true))
                {
                    if (Convert.ToDateTime(dtpFrom.Text/*Value.ToShortDateString()*/) > Convert.ToDateTime(dtpTo.Text/*Value.ToShortDateString()*/))
                    {
                        _errorProvider.SetError(dtpFrom, "Intake From Date should be less than or equal to Intake To Date");
                        isValid = false;
                        IsSwitchesTab = true;
                    }
                    else
                        _errorProvider.SetError(dtpFrom, null);
                }
            }

            if (txtEmail.Text.Trim().Length > 0)
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


            if (IsDefinitionTab)
            {
                tabControl1.SelectedIndex = 0;
            }
            else if (IsMasterTab)
            {
                tabControl1.SelectedIndex = 1;
            }
            else if (IsSwitchesTab)
            {
                tabControl1.SelectedIndex = 2;
            }
            else if (IsIncomeTab)
            {
                tabControl1.SelectedIndex = 3;
            }
            else if (IsARHSTab)
            {
                tabControl1.SelectedIndex = 4;
            }
            IsSaveValid = isValid;
            return (isValid);
        }

        private void ShowHidePrivilegeButtons()
        {
            if (PrivilegeEntity.AddPriv.Equals("false"))
            {
                picAddContact.Visible = false;
                pbAddEnroll.Visible = false;
            }
            else
            {
                picAddContact.Visible = true;
                pbAddEnroll.Visible = true;
            }

            if (PrivilegeEntity.ChangePriv.Equals("false"))
            {
                picEditContact.Visible = false;
                pbEditEnroll.Visible = false;
            }
            else
            {
                picEditContact.Visible = true;
                pbEditEnroll.Visible = true;
            }

            if (PrivilegeEntity.DelPriv.Equals("false"))
            {
                picDeleteContact.Visible = false;
                pbDeleteEnroll.Visible = false;
            }
            else
            {
                picDeleteContact.Visible = true;
                pbDeleteEnroll.Visible = true;
            }
            if (Mode.Equals("Add"))
            {
                picEditContact.Visible = false;
                pbEditEnroll.Visible = false;
                picDeleteContact.Visible = false;
                pbDeleteEnroll.Visible = false;
            }


        }

        /// <summary>
        /// Handles the grid DataError event to prevent error messages in the client.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewDataError(object sender, Wisej.Web.DataGridViewDataErrorEventArgs e)
        {
            //DO NOTHING HERE
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Click(object sender, EventArgs e)
        {

        }

        private void OnpicProgramClick(object sender, EventArgs e)
        {
            List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetCaseHierarchy(string.Empty);
            if (caseHierarchy.Count > 0)
            {
                //HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, string.Empty, "Program", string.Empty, "U", string.Empty);
                HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, string.Empty, "Program", string.Empty, "U", string.Empty,BaseForm.UserID);
                hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
                hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
                hierarchieSelectionForm.ShowDialog();
            }
            else
                CommonFunctions.MessageBoxDisplay("All programs are defined");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {

            //HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;
            HierarchieSelection form = sender as HierarchieSelection;

            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                if (selectedHierarchies.Count > 0)
                {
                    txtProgram.Text =  selectedHierarchies[0].Agency + " - " + selectedHierarchies[0].AgencyName.Trim() + Consts.Common.TabSpace + selectedHierarchies[0].Dept + " - " + selectedHierarchies[0].DeptName.Trim() + Consts.Common.TabSpace +  selectedHierarchies[0].Prog + " - " + selectedHierarchies[0].HirarchyName.Trim();
                    Agency = selectedHierarchies[0].Agency;
                    Dept = selectedHierarchies[0].Dept;
                    Program = selectedHierarchies[0].Prog;
                    txtProgramName.Text = selectedHierarchies[0].HirarchyName;
                    txtShortName.Text = selectedHierarchies[0].ShortName;
                    fillMasterPoverty();
                    CaseMstMaxApplNo = _model.CaseMstData.GetMaxApplicantNo(Agency, Dept, Program, txtYear.Text);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnContactFormClosed(object sender, FormClosedEventArgs e)
        {
            AddProgramContactForm form = sender as AddProgramContactForm;
            DepContactEntity contEntity = form.ContactEntity;
            string mode = form.Mode;
            if (form.DialogResult == DialogResult.OK)
            {
                DepContactEntity selectedRowEntity = GetSelectedRow();
                if (selectedRowEntity != null && mode.Equals("Edit"))
                {
                    gvwContactInfo.Rows.Remove(gvwContactInfo.Rows[SelectedRowIndex]);
                }
                int rowIndex = gvwContactInfo.Rows.Add(contEntity.FirstName.Trim() + " " + contEntity.LastName.Trim(), CommonFunctions.GetPhoneNo(contEntity.Phone1.Trim()));
                gvwContactInfo.Rows[rowIndex].Tag = contEntity;
            }
            if (gvwContactInfo.Rows.Count > 0)
            {
                gvwContactInfo.CurrentCell = gvwContactInfo.Rows[0].Cells[1];
                gvwContactInfo.Rows[0].Selected = true;

                picEditContact.Enabled = true;
                picDeleteContact.Enabled = true;
                if (PrivilegeEntity.ChangePriv.Equals("false"))
                {
                    picEditContact.Visible = false;
                }
                else
                {
                    picEditContact.Visible = true;
                }

                if (PrivilegeEntity.DelPriv.Equals("false"))
                {
                    picDeleteContact.Visible = false;
                }
                else
                {
                    picDeleteContact.Visible = true;
                }

            }
            else
            {
                picEditContact.Visible = false;
                picDeleteContact.Visible = false;
            }
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox24_TextChanged(object sender, EventArgs e)
        {

        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            ProgramDefinition programDefinitionControl = BaseForm.GetBaseUserControl() as ProgramDefinition;
            if (programDefinitionControl != null)
            {
                programDefinitionControl.RefreshGrid(SelectedProgram);
            }
            this.Close();
        }

        private void OnSaveClick(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    bool boolInsert = true;
                    if (!txtNextApp.Text.Trim().Equals(string.Empty))
                    {
                        if (CaseMstMaxApplNo < Convert.ToInt64(txtNextApp.Text))
                        {
                            txtNextApp.Text = "00000000".Substring(0, 8 - txtNextApp.Text.Trim().Length) + txtNextApp.Text.Trim();
                            boolInsert = true;
                        }
                        else
                        {
                            if (cbGenerateApp.Checked == true)
                            {
                                CommonFunctions.MessageBoxDisplay("Next App# should be greater than " + CaseMstMaxApplNo + "");
                                boolInsert = false;
                            }
                            else
                            {
                                boolInsert = true;
                            }
                        }
                    }
                    if (boolInsert)
                    {
                        ProgramDefinitionEntity programEntity = new ProgramDefinitionEntity();
                        programEntity.Agency = Agency;
                        programEntity.Dept = Dept;
                        programEntity.Prog = Program;
                        programEntity.DepAGCY = txtProgramName.Text.Trim();
                        programEntity.ShortName = txtShortName.Text.Trim();
                        programEntity.DepYear = txtYear.Text.Trim();
                        programEntity.DepFirstName = txtFname.Text.Trim();
                        programEntity.DepLastName = txtLname.Text.Trim();
                        programEntity.DepMI = txtMI.Text.Trim();
                        programEntity.Phone = txtPhone.Text.Trim();
                        programEntity.DepFax = txtFax.Text.Trim();
                        programEntity.Address1 = txtAddress1.Text.Trim();
                        programEntity.Address2 = txtAddress2.Text.Trim();
                        programEntity.Address3 = txtAddress3.Text.Trim();
                        programEntity.City = txtCity.Text.Trim();
                        if (!txtZip.Text.Trim().Equals(string.Empty))
                        {
                            programEntity.Zip = txtZip.Text.Trim();
                        }
                        if (!txtZipPlus.Text.Trim().Equals(string.Empty))
                        {
                            programEntity.ZipPlus = txtZipPlus.Text.Trim();
                        }
                        programEntity.State = txtState.Text.Trim();

                        programEntity.emailID = txtEmail.Text.Trim();


                        programEntity.DepGenerateApps = cbGenerateApp.Checked ? "Y" : "N";
                        programEntity.DepAppNo = !txtNextApp.Text.Trim().Equals(string.Empty) ? txtNextApp.Text.ToString().Trim() : "0";


                        programEntity.IncomeVerMsg = cbIncomeVerification.Checked ? "Y" : "N";
                        programEntity.IncomeTypeOnly = cbIncludeOnly.Checked ? "Y" : "N";
                        programEntity.IncomeWeek = cbWeekly.Checked ? "Y" : "N";
                        programEntity.IntakeEdit = cbIntakeDate.Checked ? "1" : "0";
                        programEntity.PRODUPSSN = cbPhohibitDuplicateSSN.Checked ? "Y" : "N";
                        if(cbPhohibitDuplicateSSN.Checked)
                        {
                            if (rbAppSSN.Checked) programEntity.PRODUPAPP_BY = "1";
                            else if (rbAppFN.Checked) programEntity.PRODUPAPP_BY = "2";
                            else if (rbAppFNI.Checked) programEntity.PRODUPAPP_BY = "3";
                        }

                        programEntity.ProDupMEM = cbProhibitDuplicateMember.Checked ? "Y" : "N";
                        if (cbProhibitDuplicateMember.Checked)
                        {
                            if (rbMemSSN.Checked) programEntity.PRODUPMEM_BY = "1";
                            else if (rbMemFN.Checked) programEntity.PRODUPMEM_BY = "2";
                            else if (rbMemFNI.Checked) programEntity.PRODUPMEM_BY = "3";
                        }
                        programEntity.DepSMIUsed = cbSMI.Checked ? "Y" : "N";
                        programEntity.DepCMIUsed = cbCMI.Checked ? "Y" : "N";
                        programEntity.DepFEDUsed = cbFedOMB.Checked ? "Y" : "N";
                        programEntity.DepHUDUsed = cbHUD.Checked ? "Y" : "N";
                        programEntity.DepIncludeIncVer = cbIncludeIncomeVer.Checked ? "1" : "0";
                        programEntity.DepAddressEdit = cbAddress.Checked ? "Y" : "N";
                        programEntity.AutoInActivation = cbAutoInactive.Checked ? "1" : "0";
                        programEntity.CaseTypeEdit = cbEditCaseTypes.Checked ? "Y" : "N";
                        programEntity.SecretProgram = cbSecretProgram.Checked ? "Y" : "N";
                        programEntity.DepUnitCalc = cbUom.Checked ? "Y" : "N";
                        programEntity.ZipSearch = cbZip.Checked ? "Y" : "N";
                        programEntity.SSNReasonFlag = cbSS.Checked ? "Y" : "N";
                        programEntity.SelectedClient = cbEnroll.Checked ? "1" : "0";
                        programEntity.DepAttendanceTimes = chkAttendanceTime.Checked ? "Y" : "N";
                        programEntity.DepIntakeProg = chkIntakeProg.Checked ? "Y" : "N";
                        programEntity.IntakeEdit = "0";
                        if (cbIntakeDate.Checked)
                        {
                            programEntity.IntakeEdit = "1";
                            programEntity.IntakeFDate = dtpFrom.Text.Trim();
                            programEntity.IntakeTDate = dtpTo.Text.Trim();
                        }
                        programEntity.DepJUVToAge = !txtJuvAgeTo.Text.Trim().Equals(string.Empty) ? txtJuvAgeTo.Text.ToString().Trim() : "0";
                        programEntity.DepJUVFromAge = !txtJuvAgeFrom.Text.Trim().Equals(string.Empty) ? txtJuvAgeFrom.Text.ToString().Trim() : "0";
                        programEntity.DepSENToAge = !txtSAgeTo.Text.Trim().Equals(string.Empty) ? txtSAgeTo.Text.ToString().Trim() : "0";
                        programEntity.DepSENFromAge = !txtSAgeFrom.Text.Trim().Equals(string.Empty) ? txtSAgeFrom.Text.ToString().Trim() : "0";
                        programEntity.DepThreshold = !txtThreshold.Text.Trim().Equals(string.Empty) ? txtThreshold.Text.ToString().Trim() : "0";
                        if (rbSystem.Checked)
                        {
                            programEntity.DepAutoRefer = "Y";
                        }
                        else
                        {
                            programEntity.DepAutoRefer = "N";
                        }
                        programEntity.SIMPointsMethod = cbPoints.Checked ? "Y" : "N";

                        programEntity.IncomeDateValidate = cbDateValidateIncome.Checked ? "1" : "0";
                        programEntity.DepIncExcIntDefault1 = cb30DaysDefault.Checked ? "1" : "0";
                        programEntity.DepIncExcIntDefault2 = cb60DaysDefault.Checked ? "1" : "0";
                        programEntity.DepIncExcIntDefault3 = cb90DaysDefault.Checked ? "1" : "0";
                        programEntity.DepIncExcIntUsed1 = cb30DaysUsed.Checked ? "1" : "0";
                        programEntity.DepIncExcIntUsed2 = cb60DaysUsed.Checked ? "1" : "0";
                        programEntity.DepIncExcIntUsed3 = cb90DaysUsed.Checked ? "1" : "0";
                        programEntity.DepIncExcIntFactr1 = !txt30DaysFactor.Text.Trim().Equals(string.Empty) ? txt30DaysFactor.Text.Trim() : "0";
                        programEntity.DepIncExcIntFactr2 = !txt60DaysFactor.Text.Trim().Equals(string.Empty) ? txt60DaysFactor.Text.Trim() : "0";
                        programEntity.DepIncExcIntFactr3 = !txt90DaysFactor.Text.Trim().Equals(string.Empty) ? txt90DaysFactor.Text.Trim() : "0";

                        programEntity.Account = maskedTextBoxDefAcct.Text;
                        programEntity.Free1 = !txtFreeBF.Text.Trim().Equals(string.Empty) ? txtFreeBF.Text.Trim() : "0";
                        programEntity.Free2 = !txtFreeAMSnack.Text.Trim().Equals(string.Empty) ? txtFreeAMSnack.Text.Trim() : "0";
                        programEntity.Free3 = !txtFreeLunch.Text.Trim().Equals(string.Empty) ? txtFreeLunch.Text.Trim() : "0";
                        programEntity.Free4 = !txtFreePMSnack.Text.Trim().Equals(string.Empty) ? txtFreePMSnack.Text.Trim() : "0";
                        programEntity.Free5 = !txtFreeSupper.Text.Trim().Equals(string.Empty) ? txtFreeSupper.Text.Trim() : "0";
                        programEntity.Free6 = !txtFreeOthers.Text.Trim().Equals(string.Empty) ? txtFreeOthers.Text.Trim() : "0";
                        programEntity.Reduced1 = !txtReducedBF.Text.Trim().Equals(string.Empty) ? txtReducedBF.Text.Trim() : "0";
                        programEntity.Reduced2 = !txtReducedAMSnack.Text.Trim().Equals(string.Empty) ? txtReducedAMSnack.Text.Trim() : "0";
                        programEntity.Reduced3 = !txtReducedLunch.Text.Trim().Equals(string.Empty) ? txtReducedLunch.Text.Trim() : "0";
                        programEntity.Reduced4 = !txtReducedPMSnack.Text.Trim().Equals(string.Empty) ? txtReducedPMSnack.Text.Trim() : "0";
                        programEntity.Reduced5 = !txtReducedSupper.Text.Trim().Equals(string.Empty) ? txtReducedSupper.Text.Trim() : "0";
                        programEntity.Reduced6 = !txtReducedOthers.Text.Trim().Equals(string.Empty) ? txtReducedOthers.Text.Trim() : "0";

                        programEntity.Paid1 = !txtPaidBF.Text.Trim().Equals(string.Empty) ? txtPaidBF.Text.Trim() : "0";
                        programEntity.Paid2 = !txtPaidAMSnack.Text.Trim().Equals(string.Empty) ? txtPaidAMSnack.Text.Trim() : "0";
                        programEntity.Paid3 = !txtPaidLunch.Text.Trim().Equals(string.Empty) ? txtPaidLunch.Text.Trim() : "0";
                        programEntity.Paid4 = !txtPaidPMSnack.Text.Trim().Equals(string.Empty) ? txtPaidPMSnack.Text.Trim() : "0";
                        programEntity.Paid5 = !txtPaidSupper.Text.Trim().Equals(string.Empty) ? txtPaidSupper.Text.Trim() : "0";
                        programEntity.Paid6 = !txtPaidOthers.Text.Trim().Equals(string.Empty) ? txtPaidOthers.Text.Trim() : "0";

                        string relationTypes = string.Empty;
                        List<CommonEntity> selectedRelations = (from c in gvwRelationShip.Rows.Cast<DataGridViewRow>().ToList()
                                                                where (((DataGridViewCheckBoxCell)c.Cells["cbSelect"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                                select ((DataGridViewRow)c).Tag as CommonEntity).ToList();
                        foreach (CommonEntity relation in selectedRelations)
                        {
                            if (!relationTypes.Equals(string.Empty)) relationTypes += " ";
                            relationTypes += relation.Code;
                        }

                        string incomeTypes = string.Empty;
                        List<AgyTabEntity> selectedIncomes = (from c in gvwIncomeTypes.Rows.Cast<DataGridViewRow>().ToList()
                                                              where (((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                              select ((DataGridViewRow)c).Tag as AgyTabEntity).ToList();
                        foreach (AgyTabEntity income in selectedIncomes)
                        {
                            if (!incomeTypes.Equals(string.Empty)) incomeTypes += " ";
                            incomeTypes += income.agycode;
                        }
                        programEntity.DepIncomeTypes = incomeTypes;
                        programEntity.DepReleataionTypes = relationTypes;

                        if (!((ListItem)cmbStartMonth.SelectedItem).Value.ToString().Equals("0"))
                        {
                            programEntity.StartMonth = ((ListItem)cmbStartMonth.SelectedItem).Value.ToString();
                        }
                        if (!((ListItem)cmbEndMonth.SelectedItem).Value.ToString().Equals("0"))
                        {
                            programEntity.EndMonth = ((ListItem)cmbEndMonth.SelectedItem).Value.ToString();
                        }
                        if (!((ListItem)cmbCustomerType.SelectedItem).Value.ToString().Equals("0"))
                        {
                            programEntity.Type = ((ListItem)cmbCustomerType.SelectedItem).Value.ToString();
                        }
                        if (!((ListItem)cmbInvoiceSource.SelectedItem).Value.ToString().Equals("0"))
                        {
                            programEntity.Source = ((ListItem)cmbInvoiceSource.SelectedItem).Value.ToString();
                        }
                        if (!((ListItem)cmbDefaultQuestions.SelectedItem).Value.ToString().Equals("0"))
                        {
                            programEntity.LoadProgramQuestions = ((ListItem)cmbDefaultQuestions.SelectedItem).Value.ToString();
                        }

                        string strEnrlHie = string.Empty;
                        foreach (DataGridViewRow gvhierachyitem in gvwHiearchys.Rows)
                        {
                            if (!strEnrlHie.Equals(string.Empty)) strEnrlHie += " ";
                            strEnrlHie += gvhierachyitem.Cells["HieCode"].Value.ToString();

                        }

                        programEntity.DepHsAgeMethod = (rdoZip.Checked == true ? "Z" : "S");
                        programEntity.AddOperator = BaseForm.UserID;
                        programEntity.LSTCOperator = BaseForm.UserID;
                        programEntity.DepSsnAutoAssign = "0";
                        if (chkIntakeProg.Checked)
                        {
                            //if (chkssnMask.Checked)
                            //    programEntity.DepSsnAutoAssign = "1";
                            //if (chkssnauto.Checked)
                            //    programEntity.DepSsnAutoAssign = "2";
                        }

                        // Murali added this field on 05/17/2021
                        if (BaseForm.BaseAgencyControlDetails.PaymentCategorieService == "Y")
                        {
                            if (!((ListItem)cmbPayCatservice.SelectedItem).Value.ToString().Equals("0"))
                            {
                                programEntity.DepSerpostPAYCAT = ((ListItem)cmbPayCatservice.SelectedItem).Value.ToString();
                            }
                        }
                        else
                            programEntity.DepSerpostPAYCAT = string.Empty;

                        //Added by Vikash on 02/18/2025 as per CASESNP Search point from Priority
                        //string SearchCntrl = string.Empty;
                        //if (rdbFNDOBSSN.Checked)
                        //    SearchCntrl = "F";
                        //if (rdbClientID.Checked)
                        //    SearchCntrl = "C";
                        //programEntity.Search_Cntrl = SearchCntrl;

                        if (Mode.Equals("Edit")) programEntity.Mode = "U";
                        if (_model.HierarchyAndPrograms.InsertCaseDep(programEntity))
                        {
                            saveContacts();
                            savePMTFLDFLSH();
                            saveEnrollHierachies();
                            ProgramDefinition programDefinitionControl = BaseForm.GetBaseUserControl() as ProgramDefinition;
                            //if (Mode.Equals("Edit"))
                            //{
                            //    MessageBox.Show("Record Updated Successfully", "Program Definition");
                            //}

                            if (programDefinitionControl != null)
                            {
                                programDefinitionControl.RefreshGrid(programEntity);
                            }
                            AlertBox.Show("Saved Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                            this.Close();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AlertBox.Show(ex.Message, MessageBoxIcon.Error);
            }
        }


        private void saveContacts()
        {
            foreach (DataGridViewRow row in gvwContactInfo.Rows)
            {
                if (row.Tag is DepContactEntity)
                {
                    DepContactEntity contactEntity = row.Tag as DepContactEntity;
                    _model.HierarchyAndPrograms.InsertDepContact(contactEntity);
                }
            }
        }

        private void savePMTFLDFLSH()
        {
            if (BaseForm.BaseAgencyControlDetails.PaymentCategorieService == "Y")
            {
                if (!((ListItem)cmbPayCatservice.SelectedItem).Value.ToString().Equals("0"))
                {
                    PMTFLDCNTLHEntity pmtflddata = new Model.Objects.PMTFLDCNTLHEntity();
                    if (propPMTFLDCNTLHEntity.Count > 0)
                    {
                        pmtflddata = propPMTFLDCNTLHEntity[0];
                        pmtflddata.PMFLDH_SCR_HIE = Agency + Dept + Program;
                        pmtflddata.PMFLDH_CATG = ((ListItem)cmbPayCatservice.SelectedItem).Value.ToString();
                        pmtflddata.Mode = "DELETECAT";
                        _model.FieldControls.InsertUpdateDelPMTFLDCNTLH(pmtflddata);
                        pmtflddata.Mode = "DELETE";
                        _model.FieldControls.InsertUpdateDelPMTFLDCNTLH(pmtflddata);

                        foreach (PMTFLDCNTLHEntity pmtfldentity in propPMTFLDCNTLHEntity)
                        {
                            pmtfldentity.PMFLDH_SCR_HIE = Agency + Dept + Program;
                            pmtfldentity.PMFLDH_CATG = ((ListItem)cmbPayCatservice.SelectedItem).Value.ToString();
                            pmtfldentity.Mode = "ADD";
                            _model.FieldControls.InsertUpdateDelPMTFLDCNTLH(pmtfldentity);

                        }
                    }
                }
                else
                {
                    PMTFLDCNTLHEntity pmtflddata = new Model.Objects.PMTFLDCNTLHEntity();
                    pmtflddata.PMFLDH_SCR_CODE = "CASE0063";
                    pmtflddata.PMFLDH_SCR_CODE = "CASE0063";
                    pmtflddata.PMFLDH_SCR_HIE = Agency + Dept + Program;
                    pmtflddata.PMFLDH_CATG = ((ListItem)cmbPayCatservice.SelectedItem).Value.ToString();
                    pmtflddata.Mode = "DELETEALL";
                    _model.FieldControls.InsertUpdateDelPMTFLDCNTLH(pmtflddata);
                }
            }
        }


        private void saveEnrollHierachies()
        {
            foreach (DataGridViewRow row in gvwHiearchys.Rows)
            {
                if (row.Tag is DepEnrollHierachiesEntity)
                {
                    DepEnrollHierachiesEntity DepEnrollEntity = row.Tag as DepEnrollHierachiesEntity;
                    DepEnrollEntity.StartDate = LookupDataAccess.Getdate(DepEnrollEntity.StartDate.Trim());
                    DepEnrollEntity.Enddate = LookupDataAccess.Getdate(DepEnrollEntity.Enddate.Trim());
                    _model.HierarchyAndPrograms.InsertCaseDepEnrollHierachies(DepEnrollEntity);
                }
            }
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void OnAddContactClick(object sender, EventArgs e)
        {
            if (Agency != null)
            {
                DepContactEntity contactEntity = new DepContactEntity();
                contactEntity.Agency = Agency;
                contactEntity.Dept = Dept;
                contactEntity.Program = Program;
                AddProgramContactForm addContactForm = new AddProgramContactForm(BaseForm, "Add", contactEntity);
                addContactForm.FormClosed += new FormClosedEventHandler(OnContactFormClosed);
                addContactForm.StartPosition = FormStartPosition.CenterScreen;
                addContactForm.ShowDialog();
            }
            else
            {
                AlertBox.Show("Please select the Program", MessageBoxIcon.Warning);
            }
        }

        private void OnEditContactClick(object sender, EventArgs e)
        {
            DepContactEntity selectedRow = GetSelectedRow();
            if (selectedRow != null)
            {
                AddProgramContactForm addContactForm = new AddProgramContactForm(BaseForm, "Edit", selectedRow);
                addContactForm.FormClosed += new FormClosedEventHandler(OnContactFormClosed);
                addContactForm.StartPosition = FormStartPosition.CenterScreen;
                addContactForm.ShowDialog();
            }
            else
            {
                AlertBox.Show("Please select the Row", MessageBoxIcon.Warning);
            }
        }

        private void OnDeleteContactClick(object sender, EventArgs e)
        {
            DepContactEntity selectedRow = GetSelectedRow();
            if (selectedRow != null)
            {
                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\nContact: " + selectedRow.FirstName + " " + selectedRow.LastName, Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
            }
            else
            {
                AlertBox.Show("Please select the Row", MessageBoxIcon.Warning);
            }
        }

        private void MessageBoxHandler(DialogResult dialogresult)
        {
            // Get Wisej.Web.Form object that called MessageBox
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
                // Set DialogResult value of the Form as a text for label
                if (dialogresult == DialogResult.Yes)
                {
                    DepContactEntity selectedRowEntity = GetSelectedRow();
                    if (selectedRowEntity.Mode == null)
                    {
                        gvwContactInfo.Rows.Remove(gvwContactInfo.Rows[SelectedRowIndex]);
                    }
                    else if (selectedRowEntity.Mode.Equals("U"))
                    {
                        if (_model.HierarchyAndPrograms.DeleteDepContact(selectedRowEntity))
                        {
                            gvwContactInfo.Rows.Remove(gvwContactInfo.Rows[SelectedRowIndex]);
                        }
                    }
                    gvwContactInfo.Update();
                    gvwContactInfo.ResumeLayout();
                }
                if (gvwContactInfo.Rows.Count == 0)
                {
                    picDeleteContact.Visible = false;
                    picEditContact.Visible = false;
                }
            //}
        }

        /// <summary>
        /// Get Selected Rows Tag Clas.
        /// </summary>
        /// <returns></returns>
        public DepContactEntity GetSelectedRow()
        {
            DepContactEntity contactEntity = null;
            if (gvwContactInfo != null)
            {
                foreach (DataGridViewRow dr in gvwContactInfo.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        contactEntity = dr.Tag as DepContactEntity;
                        SelectedRowIndex = dr.Index;
                        break;
                    }
                }
            }
            return contactEntity;
        }

        private void OnGenerateAppClick(object sender, EventArgs e)
        {
            if (cbGenerateApp.Checked)
            {
                txtNextApp.Enabled = true;
                txtNextApp.Focus();
                lblNextApp.Visible = true;
            }
            else
            {
                txtNextApp.Enabled = false;
                txtNextApp.Text = string.Empty;
                lblNextApp.Visible = false;
            }
        }

        private void gvwRelationShip_Click(object sender, EventArgs e)
        {

        }

        private void OnEditIntakeDateClick(object sender, EventArgs e)
        {
            _errorProvider.SetError(dtpFrom, null);
            _errorProvider.SetError(dtpTo, null);
            if (cbIntakeDate.Checked)
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                dtpFrom.Checked = true;
                dtpTo.Checked = true;
            }
            else
            {
                dtpFrom.Enabled = false;
                dtpFrom.Checked = false;
                dtpTo.Checked = false;
                dtpTo.Enabled = false;
            }
        }

        private void OnDefaultIncomeClick(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Name.Equals("cb30DaysDefault"))
            {
                cb60DaysDefault.Click -= OnDefaultIncomeClick;
                cb90DaysDefault.Click -= OnDefaultIncomeClick;
                cb60DaysDefault.Checked = false;
                cb90DaysDefault.Checked = false;
                cb60DaysDefault.Click += OnDefaultIncomeClick;
                cb90DaysDefault.Click += OnDefaultIncomeClick;
            }
            else if ((sender as CheckBox).Name.Equals("cb60DaysDefault"))
            {
                cb30DaysDefault.Click -= OnDefaultIncomeClick;
                cb90DaysDefault.Click -= OnDefaultIncomeClick;
                cb30DaysDefault.Checked = false;
                cb90DaysDefault.Checked = false;
                cb30DaysDefault.Click += OnDefaultIncomeClick;
                cb90DaysDefault.Click += OnDefaultIncomeClick;
            }
            else if ((sender as CheckBox).Name.Equals("cb90DaysDefault"))
            {
                cb60DaysDefault.Click -= OnDefaultIncomeClick;
                cb30DaysDefault.Click -= OnDefaultIncomeClick;
                cb60DaysDefault.Checked = false;
                cb30DaysDefault.Checked = false;
                cb60DaysDefault.Click += OnDefaultIncomeClick;
                cb30DaysDefault.Click += OnDefaultIncomeClick;
            }
        }

        private void txtZipPlus_Leave(object sender, EventArgs e)
        {
            if (!txtZipPlus.Text.Trim().Equals(string.Empty))
                txtZipPlus.Text = "0000".Substring(0, 4 - txtZipPlus.Text.Trim().Length) + txtZipPlus.Text.Trim();
        }

        private void txtZip_Leave(object sender, EventArgs e)
        {
            if (!txtZip.Text.Trim().Equals(string.Empty))
                txtZip.Text = "00000".Substring(0, 5 - txtZip.Text.Trim().Length) + txtZip.Text.Trim();
        }

        private void txtNextApp_Leave(object sender, EventArgs e)
        {
            if (cbGenerateApp.Checked == true)
            {
                if (!txtNextApp.Text.Trim().Equals(string.Empty))
                {
                    if (CaseMstMaxApplNo < Convert.ToInt64(txtNextApp.Text))
                    {
                        txtNextApp.Text = "00000000".Substring(0, 8 - txtNextApp.Text.Trim().Length) + txtNextApp.Text.Trim();
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay("Please enter greater than this no. " + CaseMstMaxApplNo + "");
                    }
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == null) return;
            string code = tabControl1.SelectedTab.Tag as string;
            if (code.Equals("ProgramSwitches"))
            {
                cbIncomeVerification.Focus();
            }
            else if (code.Equals("ProgramDefinition"))
            {
                txtFname.Focus();
            }
            else if (code.Equals("HS/AR"))
            {
                cbEnroll.Focus();
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //if (tabControl1.SelectedIndex == 0)
            //    Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "CASE2007_Program");
            //if (tabControl1.SelectedIndex == 1)
            //    Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "CASE2007_Poverty");
            //if (tabControl1.SelectedIndex == 2)
            //    Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "CASE2007_Switches");
            //if (tabControl1.SelectedIndex == 3)
            //    Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "CASE2007_Income");
            //if (tabControl1.SelectedIndex == 4)
            //    Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "CASE2007_HS");
        }



        public List<HierarchyEntity> SelectedHierarchies
        {
            get
            {
                _selectedHierarchies = new List<HierarchyEntity>();
                foreach (DataGridViewRow row in gvwHiearchys.Rows)
                {
                    HierarchyEntity hierarchy = row.Tag as HierarchyEntity;
                    if (!hierarchy.UsedFlag.Equals("Y"))
                    {
                        _selectedHierarchies.Add(hierarchy);
                    }
                }
                return _selectedHierarchies;
            }
        }

        public List<string> GetHierachys()
        {

            List<string> HierachiesList = new List<string>();
            foreach (DataGridViewRow item in gvwHiearchys.Rows)
            {
                HierachiesList.Add(item.Cells["HieCode"].Value.ToString());
            }


            return HierachiesList;
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (Agency != null)
            {
                DepEnrollHierachiesEntity EnrollEntity = new DepEnrollHierachiesEntity();
                EnrollEntity.Agency = Agency;
                EnrollEntity.Dept = Dept;
                EnrollEntity.Program = Program;
                EnrollHierarchiesForm addEnrollForm = new EnrollHierarchiesForm(BaseForm, "Add", EnrollEntity, GetHierachys());
                addEnrollForm.FormClosed += new FormClosedEventHandler(OnEnrollFormClosed);
                addEnrollForm.StartPosition = FormStartPosition.CenterScreen;
                addEnrollForm.ShowDialog();
            }
            //else
            //{
            //    MessageBox.Show("Please select the program");
            //}
        }

        private void pbEditEnroll_Click(object sender, EventArgs e)
        {
            DepEnrollHierachiesEntity selectedRow = GetSelectedEnrollRow();
            if (selectedRow != null)
            {
                EnrollHierarchiesForm EditEnrollForm = new EnrollHierarchiesForm(BaseForm, "Edit", selectedRow, GetHierachys());
                EditEnrollForm.FormClosed += new FormClosedEventHandler(OnEnrollFormClosed);
                EditEnrollForm.StartPosition = FormStartPosition.CenterScreen;
                EditEnrollForm.ShowDialog();
            }
            //else
            //{
            //    MessageBox.Show("Please select the row");
            //}
        }

        private void pbDeleteEnroll_Click(object sender, EventArgs e)
        {
            DepEnrollHierachiesEntity selectedRow = GetSelectedEnrollRow();
            if (selectedRow != null)
            {
                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxEnrollHandler);
            }
            //else
            //{
            //    MessageBox.Show("Please select the row");
            //}
        }

        /// <summary>
        /// Get Selected Rows Tag Clas.
        /// </summary>
        /// <returns></returns>
        public DepEnrollHierachiesEntity GetSelectedEnrollRow()
        {
            DepEnrollHierachiesEntity EnrollEntity = null;
            if (gvwContactInfo != null)
            {
                foreach (DataGridViewRow dr in gvwHiearchys.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        EnrollEntity = dr.Tag as DepEnrollHierachiesEntity;
                        SelectEnrollRowIndex = dr.Index;
                        break;
                    }
                }
            }
            return EnrollEntity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEnrollFormClosed(object sender, FormClosedEventArgs e)
        {
            EnrollHierarchiesForm form = sender as EnrollHierarchiesForm;
            DepEnrollHierachiesEntity EnrollEntity = form.EnrollEntity;
            string mode = form.Mode;
            if (form.DialogResult == DialogResult.OK)
            {
                DepEnrollHierachiesEntity selectedRowEntity = GetSelectedEnrollRow();
                if (selectedRowEntity != null && mode.Equals("Edit"))
                {
                    gvwHiearchys.Rows.Remove(gvwHiearchys.Rows[SelectEnrollRowIndex]);
                }
                int rowIndex = gvwHiearchys.Rows.Add(EnrollEntity.DepEnrollCode.Trim(), EnrollEntity.Nofoslots, LookupDataAccess.Getdate(EnrollEntity.StartDate), LookupDataAccess.Getdate(EnrollEntity.Enddate), EnrollEntity.Hierachies);
                gvwHiearchys.Rows[rowIndex].Tag = EnrollEntity;
            }
            if (gvwHiearchys.Rows.Count > 0)
            {
                gvwHiearchys.CurrentCell = gvwHiearchys.Rows[0].Cells[1];
                gvwHiearchys.Rows[0].Selected = true;

                pbEditEnroll.Enabled = true;
                pbDeleteEnroll.Enabled = true;
                if (PrivilegeEntity.ChangePriv.Equals("false"))
                {
                    pbEditEnroll.Visible = false;
                }
                else
                {
                    pbEditEnroll.Visible = true;
                }

                if (PrivilegeEntity.DelPriv.Equals("false"))
                {
                    pbDeleteEnroll.Visible = false;
                }
                else
                {
                    pbDeleteEnroll.Visible = true;
                }

            }
            else
            {
                pbEditEnroll.Visible = false;
                pbDeleteEnroll.Visible = false;
            }
        }

        private void MessageBoxEnrollHandler(DialogResult dialogresult)
        {
            // Get Wisej.Web.Form object that called MessageBox
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
                // Set DialogResult value of the Form as a text for label
                if (dialogresult == DialogResult.Yes)
                {
                    DepEnrollHierachiesEntity selectedRowEntity = GetSelectedEnrollRow();
                    if (selectedRowEntity.Mode == "Insert")
                    {
                        gvwHiearchys.Rows.Remove(gvwHiearchys.Rows[SelectEnrollRowIndex]);
                    }
                    else if (selectedRowEntity.Mode.Equals("Update"))
                    {
                        selectedRowEntity.Mode = "Delete";
                        if (_model.HierarchyAndPrograms.InsertCaseDepEnrollHierachies(selectedRowEntity))
                        {
                            gvwHiearchys.Rows.Remove(gvwHiearchys.Rows[SelectEnrollRowIndex]);
                        }
                    }
                    gvwContactInfo.Update();
                    gvwContactInfo.ResumeLayout();
                }
                if (gvwHiearchys.Rows.Count == 0)
                {
                    pbDeleteEnroll.Visible = false;
                    pbEditEnroll.Visible = false;
                }
            //}
        }

        private void btnSchool_Click(object sender, EventArgs e)
        {
            HsAgeControlForm hsagecontrolform = new HsAgeControlForm(BaseForm, Mode, PrivilegeEntity);
            hsagecontrolform.StartPosition = FormStartPosition.CenterScreen;
            hsagecontrolform.ShowDialog();
        }

        private void chkIntakeProg_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIntakeProg.Checked)
            {
                //chkssnauto.Visible = true;
                //chkssnMask.Visible = true;
                //chkssnauto.Checked = false;
                //chkssnMask.Checked = false;
            }
            else
            {
                chkssnauto.Visible = false;
                chkssnMask.Visible = false;
                chkssnauto.Checked = false;
                chkssnMask.Checked = false;
            }
        }

        private void chkssnMask_CheckedChanged(object sender, EventArgs e)
        {
            if (chkssnMask.Checked)
            {
                chkssnauto.Checked = false;
            }

        }

        private void chkssnauto_Click(object sender, EventArgs e)
        {
            if (chkssnauto.Checked)
            {
                chkssnMask.Checked = false;
            }
        }

        private void btnPushpayment_Click(object sender, EventArgs e)
        {
            if (cmbPayCatservice.Items.Count > 0)
            {
                if (!((ListItem)cmbPayCatservice.SelectedItem).Value.ToString().Equals("0"))
                {
                    ProgramFieldControlForm form = new Forms.ProgramFieldControlForm(BaseForm, Mode, ((ListItem)cmbPayCatservice.SelectedItem).Value.ToString(), Agency + Dept + Program, propPMTFLDCNTLHEntity);
                    form.FormClosed += new FormClosedEventHandler(objform_FormClosed);
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.ShowDialog();
                }
            }
        }
        void objform_FormClosed(object sender, FormClosedEventArgs e)
        {
            ProgramFieldControlForm form = sender as ProgramFieldControlForm;
            if (form.DialogResult == DialogResult.OK)
            {
                propPMTFLDCNTLHEntity = form.propPMTFLDCNTLHEntity;
            }

        }

        private void cmbPayCatservice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BaseForm.BaseAgencyControlDetails.PaymentCategorieService == "Y")
            {
                if (cmbPayCatservice.Items.Count > 0)
                {
                    if (Mode.Equals("Edit"))
                    {
                        if (propDepSerpostPAYCAT != string.Empty)
                        {
                            if (propDepSerpostPAYCAT != ((ListItem)cmbPayCatservice.SelectedItem).Value.ToString())
                            {
                                MessageBox.Show("Do you want to remove all Field control settings for this Program?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxProgramHandler);

                            }
                        }
                    }

                }
            }
        }

        private void MessageBoxProgramHandler(DialogResult dialogresult)
        {
            // Get Wisej.Web.Form object that called MessageBox
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
                // Set DialogResult value of the Form as a text for label
                if (dialogresult == DialogResult.No)
                {
                    cmbPayCatservice.SelectedIndexChanged -= cmbPayCatservice_SelectedIndexChanged;
                    CommonFunctions.SetComboBoxValue(cmbPayCatservice, propDepSerpostPAYCAT);
                    cmbPayCatservice.SelectedIndexChanged += cmbPayCatservice_SelectedIndexChanged;
                }
            //}
        }

        private void cbPhohibitDuplicateSSN_CheckedChanged(object sender, EventArgs e)
        {
            if(cbPhohibitDuplicateSSN.Checked)
            {
                pnlProbApps.Enabled = true;
                if(Mode=="Add")
                    rbAppFN.Checked = true;
            }
            else
            {
                pnlProbApps.Enabled = false;
                rbAppFN.Checked = false;rbAppFNI.Checked = false;rbAppSSN.Checked = false;
            }
        }

        private void cbProhibitDuplicateMember_CheckedChanged(object sender, EventArgs e)
        {
            if (cbProhibitDuplicateMember.Checked)
            {
                pnlProbMems.Enabled = true;
                if (Mode == "Add")
                    rbMemFN.Checked = true;
            }
            else
            {
                pnlProbMems.Enabled = false;
                rbMemFN.Checked = false; rbMemFNI.Checked = false; rbMemSSN.Checked = false;
            }
        }

        private void AddProgramDefinitionForm_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                if (Mode == "Add")
                    Application.Navigate(CommonFunctions.CreateZenHelps(PrivilegeEntity.Program, 1, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
                else if (Mode == "Edit")
                    Application.Navigate(CommonFunctions.CreateZenHelps(PrivilegeEntity.Program, 6, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
            }
            if (tabControl1.SelectedIndex == 1)
                Application.Navigate(CommonFunctions.CreateZenHelps(PrivilegeEntity.Program, 2, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
            if (tabControl1.SelectedIndex == 2)
                Application.Navigate(CommonFunctions.CreateZenHelps(PrivilegeEntity.Program, 3, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
            if (tabControl1.SelectedIndex == 3)
                Application.Navigate(CommonFunctions.CreateZenHelps(PrivilegeEntity.Program, 4, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
            if (tabControl1.SelectedIndex == 4)
                Application.Navigate(CommonFunctions.CreateZenHelps(PrivilegeEntity.Program, 5, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }
    }
}