#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Captain.Common.Utilities;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Views.UserControls;
using System.Text.RegularExpressions;
using Captain.Common.Views.Forms.Base;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CaseIncome : Form
    {
        private CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;
        List<FldcntlHieEntity> _fldCntlHieEntity = new List<FldcntlHieEntity>();
        private string strAgency = string.Empty;
        private string strDept = string.Empty;
        private string strProgram = string.Empty;
        private string strYear = string.Empty;
        private string strApplNo = string.Empty;
        private string strMode = string.Empty;
        private string strNameFormat = string.Empty;
        private string strVerfierFormat = string.Empty;
        private string strIncomeDefaultCode = "0";
        private string strCaseWorkerDefaultCode = "0";
        private string strCaseWorkerDefaultStartCode = "0";
        public int strSnpIndex = 0;
        string strDefaultValue = "0";
        public CaseIncome(string changeType, BaseForm baseForm, PrivilegeEntity privilieges)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privilieges;
            this.Text = Privileges.Program + " - Income Entry";
            strAgency = BaseForm.BaseAgency;
            strDept = BaseForm.BaseDept;
            strProgram = BaseForm.BaseProg;
            strYear = BaseForm.BaseYear;
            strApplNo = BaseForm.BaseApplicationNo;
            HierarchyEntity HierarchyEntity = CommonFunctions.GetHierachyNameFormat(strAgency, "**", "**");
            if (HierarchyEntity != null)
            {
                strNameFormat = HierarchyEntity.CNFormat.ToString();
                strVerfierFormat = HierarchyEntity.CWFormat.ToString();
            }
            programDefinitionList = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            fillcombo();
            string HIE = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            List<FldcntlHieEntity> CntlEntity = _model.FieldControls.GetFLDCNTLHIE("CASINCOM", HIE, "FLDCNTL");
            FLDCNTLHieEntity = CntlEntity;
            txtAmount1.Validator = TextBoxValidation.FloatValidator;
            txtAmount2.Validator = TextBoxValidation.FloatValidator;
            txtAmount3.Validator = TextBoxValidation.FloatValidator;
            txtAmount4.Validator = TextBoxValidation.FloatValidator;
            txtAmount5.Validator = TextBoxValidation.FloatValidator;
            EnableAllcontrols();
            if (Privileges.AddPriv.Equals("false"))
            {
                dataGridCaseIncome.AllowUserToAddRows = false;
            }
            else
            {
                dataGridCaseIncome.AllowUserToAddRows = true;
            }

            if (Privileges.ChangePriv.Equals("false"))
            {

            }
            else
            {

            }
            if (Privileges.DelPriv.Equals("false"))
            {
                dataGridCaseIncome.Columns[dataGridCaseIncome.ColumnCount - 1].Visible = false;
                dataGridCaseIncome.Columns[dataGridCaseIncome.ColumnCount - 2].Width = 200;
            }
            else
            {
                dataGridCaseIncome.Columns[dataGridCaseIncome.ColumnCount - 1].Visible = true;

            }
            if (programDefinitionList.IncomeTypeOnly == "Y" && Privileges.PrivilegeName == "CASE2001")
            {
                if (FLDCNTLHieEntity.Count > 0)
                {
                    foreach (FldcntlHieEntity entity in CntlEntity)
                    {
                        bool required = entity.Req.Equals("Y") ? true : false;
                        bool enabled = entity.Enab.Equals("Y") ? true : false;

                        switch (entity.FldDesc)
                        {
                            case Consts.CASEINCOME.IncomeType:
                                if (enabled) { cmbIncomeType.Enabled = lblIncomeType.Enabled = true; if (required) lblIncomeTypeReq.Visible = true; } else { cmbIncomeType.Enabled = lblIncomeType.Enabled = false; lblIncomeTypeReq.Visible = false; }
                                break;
                        }
                    }
                }
            }
            else
            {
                DisableControls();
                EnableDisableControls();
                RequiredAmountControl();
            }


        }

        #region properties
        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public ProgramDefinitionEntity programDefinitionList { get; set; }
        public string MstIntakeDate { get; set; }
        public DateTime MstIntakeStartDate { get; set; }
        public DateTime MstIntakeEndDate { get; set; }
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
        #endregion

        public void fillcombo()
        {
            DataSet ds = null;
            DataTable dt = null;


            cmbIncomeType.Items.Clear();
            List<AgyTabEntity> lookUpIncomeTypes = _model.lookupDataAccess.GetIncomeTypes();
            string[] strIncomeTypes = programDefinitionList.DepIncomeTypes.Split(' ');

            foreach (AgyTabEntity agyEntity in lookUpIncomeTypes)
            {
                bool boolIncomeType = false;
                foreach (string incomeType in strIncomeTypes)
                {
                    if (agyEntity.agycode == incomeType)
                    {
                        boolIncomeType = true;
                    }
                }
                if (boolIncomeType)
                    cmbIncomeType.Items.Add(new ListItem(agyEntity.agydesc, agyEntity.agycode, "Y", string.Empty));
                else
                    cmbIncomeType.Items.Add(new ListItem(agyEntity.agydesc, agyEntity.agycode, "N", string.Empty));
                if (agyEntity.agydefault.Equals("Y"))
                    strIncomeDefaultCode = agyEntity.agycode == null ? string.Empty : agyEntity.agycode;

            }
            cmbIncomeType.Items.Insert(0, new ListItem("Select One", "0"));
            SetComboBoxValue(cmbIncomeType, strIncomeDefaultCode);

            cmbVerifier.Items.Clear();
            cmbVerifier.ColorMember = "FavoriteColor";
            List<HierarchyEntity> hierarchyEntity = _model.CaseMstData.GetCaseWorker(strVerfierFormat, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
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
                    strCaseWorkerDefaultCode = caseworker.CaseWorker == null ? "0" : caseworker.CaseWorker;
                    strCaseWorkerDefaultStartCode = caseworker.CaseWorker == null ? "0" : caseworker.CaseWorker;
                }

            }
            cmbVerifier.Items.Insert(0, new ListItem("Select One", "0"));
            CommonFunctions.SetComboBoxValue(cmbVerifier, strCaseWorkerDefaultCode);

            string strHourlymode = "N";
            //if (propAgencyControlDetails != null)
            //{
            //    if (propAgencyControlDetails.IncScrMode == "Y")
            //        strHourlymode = "Y";
            //}
            List<CommonEntity> commonEntity = _model.lookupDataAccess.GetIncomeInterval(strHourlymode,string.Empty);
            foreach (CommonEntity interval in commonEntity)
            {
                cmbInterval.Items.Add(new ListItem(interval.Desc, interval.Code));
            }
            cmbInterval.Items.Insert(0, new ListItem("    ", "0"));


            if (programDefinitionList.IncomeDateValidate == "1")
            {
                if (programDefinitionList.DepIncExcIntUsed3 == "1")
                {
                    cmbInterval.Items.Insert(0, new ListItem("90 Days", "9"));
                    if (programDefinitionList.DepIncExcIntDefault3 == "1")
                    {
                        strDefaultValue = "9";
                    }
                }
                if (programDefinitionList.DepIncExcIntUsed2 == "1")
                {
                    cmbInterval.Items.Insert(0, new ListItem("60 Days", "6"));
                    if (programDefinitionList.DepIncExcIntDefault2 == "1")
                    {
                        strDefaultValue = "6";
                    }
                }
                if (programDefinitionList.DepIncExcIntUsed1 == "1")
                {
                    cmbInterval.Items.Insert(0, new ListItem("30 Days", "3"));
                    if (programDefinitionList.DepIncExcIntDefault1 == "1")
                    {
                        strDefaultValue = "3";
                    }
                }

                SetComboBoxValue(cmbInterval, strDefaultValue);
            }
        }

        private void fillInterval()
        {
            cmbInterval.Items.Clear();
            cmbInterval.SelectedIndex = 0;
        }

        private void fillIncomeControl()
        {
            SetComboBoxValue(cmbIncomeType, strIncomeDefaultCode);
            chkExclude.Checked = false;
            cmbVerifier.SelectedIndex = 0;
            txtHowVerified.Text = string.Empty;
            txtFactor.Text = string.Empty;
            txtSub.Text = string.Empty;
            txtTotal.Text = string.Empty;
            txtIncSource.Text = string.Empty;
            txtAmount1.Text = string.Empty;
            txtAmount2.Text = string.Empty;
            txtAmount3.Text = string.Empty;
            txtAmount4.Text = string.Empty;
            txtAmount5.Text = string.Empty;
            SetComboBoxValue(cmbInterval, strDefaultValue);
            CommonFunctions.SetComboBoxValue(cmbVerifier, strCaseWorkerDefaultStartCode);
        }

        private void FillGridData(string Agency, string Dept, string Program, string Year, string AppNo)
        {

            List<CaseMstEntity> caseMstList = _model.CaseMstData.GetCaseMstadpyn(Agency, Dept, Program, Year, AppNo);
            BaseForm.BaseCaseMstListEntity = caseMstList;
            List<CaseSnpEntity> caseSnpList = _model.CaseMstData.GetCaseSnpadpyn(Agency, Dept, Program, Year, AppNo);
            dataGridCaseSnp.Rows.Clear();
            foreach (CaseSnpEntity caseSnp in caseSnpList)
            {
                string name = LookupDataAccess.GetMemberName(caseSnp.NameixFi, caseSnp.NameixMi, caseSnp.NameixLast, strNameFormat);
                string strAltDate = LookupDataAccess.Getdate(caseSnp.AltBdate);
                string strSsno = LookupDataAccess.GetCardNo(caseSnp.Ssno, "1", programDefinitionList.SSNReasonFlag.Trim(), caseSnp.SsnReason);
                int rowIndex = dataGridCaseSnp.Rows.Add(caseSnp.FamilySeq, name, strSsno, strAltDate, caseSnp.TotIncome.ToString(), caseSnp.ProgIncome, LookupDataAccess.GetLookUpCode("03259", caseSnp.MemberCode));
                dataGridCaseSnp.Rows[rowIndex].Tag = caseSnp;
               // dataGridCaseSnp.ItemsPerPage = 100;
                CommonFunctions.setTooltip(rowIndex, caseSnp.AddOperator, caseSnp.DateAdd, caseSnp.LstcOperator, caseSnp.DateLstc, dataGridCaseSnp);
            }
            if (caseMstList.Count > 0)
            {
                txtInHouse.Text = caseMstList[0].NoInhh.ToString();
                txtTotIncome.Text = caseMstList[0].FamIncome.ToString();
                txtInProg.Text = caseMstList[0].NoInProg.ToString();
                txtProgIncome.Text = caseMstList[0].ProgIncome.ToString();
                MstIntakeDate = caseMstList[0].IntakeDate.ToString();
            }
        }


        private void cmbInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableDisableControlsInterval();
            lblMstIntakeDate.Visible = false;
            lblDate1Range.Visible = false;
            lblDate2Range.Visible = false;
            lblDate3Range.Visible = false;
            lblDate4Range.Visible = false;
            lblDate5Range.Visible = false;
            string strInterval = ((ListItem)cmbInterval.SelectedItem).Value.ToString();

            if (strInterval == "0" || strInterval == "O" || strInterval == "A" || strInterval == "Q")
            {
                lblAmount1.Text = "Amount 1";
                lblAMount2Req.Visible = false;
                lblAmount3Req.Visible = false;
                lblAmount4Req.Visible = false;
                lblAmount5Req.Visible = false;
                lblAmount2.Visible = false;
                lblAmount3.Visible = false;
                lblAmount4.Visible = false;
                lblAmount5.Visible = false;
                txtAmount2.Visible = false;
                txtAmount3.Visible = false;
                txtAmount4.Visible = false;
                txtAmount5.Visible = false;
                lblDate2.Visible = false;
                lblDate3.Visible = false;
                lblDate4.Visible = false;
                lblDate5.Visible = false;
                caldate2.Visible = false;
                caldate3.Visible = false;
                caldate4.Visible = false;
                caldate5.Visible = false;
                lblDate2Req.Visible = false;
                lblDate3Req.Visible = false;
                lblDate4Req.Visible = false;
                lblDate5Req.Visible = false;
            }
            else if (strInterval == "B" || strInterval == "S" || strInterval == "M" || strInterval == "N")
            {
                lblAmount1.Text = "Amount 1";
                lblAmount2.Text = "Amount 2";
                lblAmount3Req.Visible = false;
                lblAmount4Req.Visible = false;
                lblAmount5Req.Visible = false;
                lblAmount2.Visible = true;
                lblAmount3.Visible = false;
                lblAmount4.Visible = false;
                lblAmount5.Visible = false;
                txtAmount2.Visible = true;
                txtAmount3.Visible = false;
                txtAmount4.Visible = false;
                txtAmount5.Visible = false;
                lblDate2.Visible = true;
                lblDate3.Visible = false;
                lblDate4.Visible = false;
                lblDate5.Visible = false;
                caldate2.Visible = true;
                caldate3.Visible = false;
                caldate4.Visible = false;
                caldate5.Visible = false;
                lblDate3Req.Visible = false;
                lblDate4Req.Visible = false;
                lblDate5Req.Visible = false;
            }
            else if (strInterval == "E")
            {
                changelblText("Amount");
            }
            else if (strInterval == "W")
            {
                changelblText("Week");
            }
            else if (strInterval == "3" || strInterval == "6" || strInterval == "9")
            {
                lblMstIntakeDate.Visible = true;
                changelblText("Amount");
            }
            caldate1.Checked = false;
            caldate2.Checked = false;
            caldate3.Checked = false;
            caldate4.Checked = false;
            caldate5.Checked = false;
            txtAmount1.Text = string.Empty;
            txtAmount2.Text = string.Empty;
            txtAmount3.Text = string.Empty;
            txtAmount4.Text = string.Empty;
            txtAmount5.Text = string.Empty;
            ShowFactor();


        }

        private void changelblText(string strValue)
        {
            lblAmount1.Text = strValue + " 1";
            lblAmount2.Text = strValue + " 2";
            lblAmount3.Text = strValue + " 3";
            lblAmount4.Text = strValue + " 4";
            lblAmount5.Text = strValue + " 5";
            lblAmount2.Visible = true;
            lblAmount3.Visible = true;
            lblAmount4.Visible = true;
            lblAmount5.Visible = true;
            txtAmount2.Visible = true;
            txtAmount3.Visible = true;
            txtAmount4.Visible = true;
            txtAmount5.Visible = true;
            lblDate2.Visible = true;
            lblDate3.Visible = true;
            lblDate4.Visible = true;
            lblDate5.Visible = true;
            caldate2.Visible = true;
            caldate3.Visible = true;
            caldate4.Visible = true;
            caldate5.Visible = true;
        }

        private void dataGridCaseSnp_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridCaseSnp.SelectedRows.Count > 0)
            {
                CaseSnpEntity row = dataGridCaseSnp.SelectedRows[0].Tag as CaseSnpEntity;
                if (row != null)
                {
                    string strFamilySeq = row.FamilySeq;
                    List<CaseIncomeEntity> caseIncomeList = _model.CaseMstData.GetCaseIncomeadpynf(strAgency, strDept, strProgram, strYear, strApplNo, strFamilySeq);
                    dataGridCaseIncome.SelectionChanged -= new EventHandler(dataGridCaseIncome_SelectionChanged);
                    dataGridCaseIncome.Rows.Clear();
                    foreach (CaseIncomeEntity caseIncome in caseIncomeList)
                    {
                        int rowIndex = dataGridCaseIncome.Rows.Add(caseIncome.FamilySeq, caseIncome.Seq, LookupDataAccess.GetLookUpCode("00004", caseIncome.Type), LookupDataAccess.ShowIncomeInterval(caseIncome.Interval), caseIncome.TotIncome.ToString(), caseIncome.ProgIncome, caseIncome.Name);
                        dataGridCaseIncome.Rows[rowIndex].Tag = caseIncome;
                       // dataGridCaseIncome.ItemsPerPage = 100;
                        CommonFunctions.setTooltip(rowIndex, caseIncome.AddOperator, caseIncome.DateAdd, caseIncome.LstcOperator, caseIncome.DateLstc, dataGridCaseIncome);
                    }
                    dataGridCaseIncome.SelectionChanged += new EventHandler(dataGridCaseIncome_SelectionChanged);
                    dataGridCaseIncome_SelectionChanged(sender, e);
                    DisableControls();
                    EnableDisableControls();
                    RequiredAmountControl();
                    strSnpIndex = dataGridCaseSnp.SelectedRows[0].Index;

                }
            }
        }

        private void dataGridCaseIncome_SelectionChanged(object sender, EventArgs e)
        {
            caldate1.Value = DateTime.Now.Date;
            caldate1.Checked = false;
            caldate2.Value = DateTime.Now.Date;
            caldate2.Checked = false;
            caldate3.Value = DateTime.Now.Date;
            caldate3.Checked = false;
            caldate4.Value = DateTime.Now.Date;
            caldate4.Checked = false;
            caldate5.Value = DateTime.Now.Date;
            caldate5.Checked = false;
            cmbVerifier.SelectedIndexChanged -= new EventHandler(cmbVerifier_SelectedIndexChanged);
            cmbIncomeType.SelectedIndexChanged -= new EventHandler(cmbIncomeType_SelectedIndexChanged);
            if (dataGridCaseIncome.Rows.Count > 0)
            {
                strMode = string.Empty;
                if (dataGridCaseIncome.SelectedRows.Count > 0)
                {
                    if (dataGridCaseIncome.SelectedRows[0].Tag is CaseIncomeEntity)
                    {
                        CaseIncomeEntity row = dataGridCaseIncome.SelectedRows[0].Tag as CaseIncomeEntity;
                        if (row != null)
                        {
                            string strFamilySeq = row.FamilySeq;// dataGridCaseIncome.SelectedRows[0].Cells["INCOME_FAMILY_SEQ"].Value.ToString();//
                            string strSeq = row.Seq;//dataGridCaseIncome.SelectedRows[0].Cells["INCOME_SEQ"].Value.ToString(); //
                            List<CaseIncomeEntity> caseIncomeList = _model.CaseMstData.GetCaseIncomeadpynfs(strAgency, strDept, strProgram, strYear, strApplNo, strFamilySeq, strSeq);
                            if (caseIncomeList.Count > 0)
                            {
                                SetComboBoxValue(cmbIncomeType, "0");
                                if (caseIncomeList[0].Interval != string.Empty)
                                    SetComboBoxValue(cmbInterval, caseIncomeList[0].Interval);
                                else
                                    SetComboBoxValue(cmbInterval, "E");
                                strCaseWorkerDefaultCode = caseIncomeList[0].Verifier;
                                if (strCaseWorkerDefaultCode != string.Empty)
                                    CommonFunctions.SetComboBoxValue(cmbVerifier, strCaseWorkerDefaultCode);
                                else
                                    CommonFunctions.SetComboBoxValue(cmbVerifier, strCaseWorkerDefaultStartCode);
                                txtAmount1.Text = caseIncomeList[0].Val1;
                                txtAmount2.Text = caseIncomeList[0].Val2;
                                txtAmount3.Text = caseIncomeList[0].Val3;
                                txtAmount4.Text = caseIncomeList[0].Val4;
                                txtAmount5.Text = caseIncomeList[0].Val5;
                                if (caseIncomeList[0].Date1 != "")
                                {
                                    caldate1.Value = Convert.ToDateTime(caseIncomeList[0].Date1);
                                    caldate1.Checked = true;
                                }
                                if (caseIncomeList[0].Date2 != "")
                                {
                                    caldate2.Value = Convert.ToDateTime(caseIncomeList[0].Date2);
                                    caldate2.Checked = true;
                                }
                                if (caseIncomeList[0].Date3 != "")
                                {
                                    caldate3.Value = Convert.ToDateTime(caseIncomeList[0].Date3);
                                    caldate3.Checked = true;
                                }
                                if (caseIncomeList[0].Date4 != "")
                                {
                                    caldate4.Value = Convert.ToDateTime(caseIncomeList[0].Date4);
                                    caldate4.Checked = true;
                                }
                                if (caseIncomeList[0].Date5 != "")
                                {
                                    caldate5.Value = Convert.ToDateTime(caseIncomeList[0].Date5);
                                    caldate5.Checked = true;
                                }
                                if (caseIncomeList[0].Exclude == "Y")
                                {
                                    chkExclude.Checked = true;
                                }
                                else
                                {
                                    chkExclude.Checked = false;
                                }
                                txtHowVerified.Text = caseIncomeList[0].HowVerified;
                                txtFactor.Text = caseIncomeList[0].Factor;
                                // txtSub.Text=caseIncomeList[0].st                       
                                txtTotal.Text = caseIncomeList[0].TotIncome;
                                txtAmount1_TextChanged(sender, e);
                                txtIncSource.Text = caseIncomeList[0].Source;
                                strMode = "U";
                                //strIncomeIndex = dataGridCaseIncome.SelectedRows[0].Index;
                                if (Privileges.ChangePriv.Equals("false"))
                                {
                                    EnableAllcontrols();

                                }
                                else
                                {
                                    EnableAllcontrols(true);
                                    DisableControls();
                                    EnableDisableControls();
                                    EnableDisableControlsInterval();

                                }
                            }
                        }
                    }
                    else
                    {
                        fillIncomeControl();
                        if (Privileges.AddPriv.Equals("false"))
                        {
                            EnableAllcontrols();

                        }
                        else
                        {
                            EnableAllcontrols(true);
                            DisableControls();
                            EnableDisableControls();
                            EnableDisableControlsInterval();

                        }
                        if (((ListItem)cmbIncomeType.SelectedItem).ID.ToString() == "Y")
                            chkExclude.Checked = true;
                        else
                            chkExclude.Checked = false;
                    }
                }
                else
                {
                    fillIncomeControl();
                    if (Privileges.AddPriv.Equals("false"))
                    {
                        EnableAllcontrols();

                    }
                    else
                    {
                        EnableAllcontrols(true);
                        DisableControls();
                        EnableDisableControls();
                        EnableDisableControlsInterval();

                    }
                    if (((ListItem)cmbIncomeType.SelectedItem).ID.ToString() == "Y")
                        chkExclude.Checked = true;
                    else
                        chkExclude.Checked = false;
                }
            }
            else
            {
                fillIncomeControl();
                if (Privileges.AddPriv.Equals("false"))
                {
                    EnableAllcontrols();

                }
                else
                {
                    EnableAllcontrols(true);
                    DisableControls();
                    EnableDisableControls();
                    EnableDisableControlsInterval();

                }
                if (((ListItem)cmbIncomeType.SelectedItem).ID.ToString() == "Y")
                    chkExclude.Checked = true;
                else
                    chkExclude.Checked = false;
                strMode = string.Empty;
            }
            RequiredAmountControl();
            cmbVerifier.SelectedIndexChanged += new EventHandler(cmbVerifier_SelectedIndexChanged);
            cmbIncomeType.SelectedIndexChanged += new EventHandler(cmbIncomeType_SelectedIndexChanged);
            cmbIncomeType.Focus();

        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (lblIncomeTypeReq.Visible && ((ListItem)cmbIncomeType.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbIncomeType, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblIncomeType.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbIncomeType, null);
            }

            if (lblVerifierReq.Visible && ((ListItem)cmbVerifier.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbVerifier, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblVerifier.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbVerifier, null);
            }

            if (lblIntervalReq.Visible && ((ListItem)cmbInterval.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbInterval, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblVerifier.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbInterval, null);
            }


            if (lblAmount1Req.Visible && String.IsNullOrEmpty(txtAmount1.Text.Trim()))
            {
                _errorProvider.SetError(txtAmount1, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAmount1.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {

                _errorProvider.SetError(txtAmount1, null);
            }

            if (lblAMount2Req.Visible && String.IsNullOrEmpty(txtAmount2.Text.Trim()))
            {
                _errorProvider.SetError(txtAmount2, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAmount2.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {

                _errorProvider.SetError(txtAmount2, null);
            }


            if (lblAmount3Req.Visible && String.IsNullOrEmpty(txtAmount3.Text.Trim()))
            {
                _errorProvider.SetError(txtAmount3, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAmount3.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {

                _errorProvider.SetError(txtAmount3, null);
            }


            if (lblAmount4Req.Visible && String.IsNullOrEmpty(txtAmount4.Text.Trim()))
            {
                _errorProvider.SetError(txtAmount4, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAmount4.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtAmount4, null);
            }
            if (lblAmount5Req.Visible && String.IsNullOrEmpty(txtAmount5.Text.Trim()))
            {
                _errorProvider.SetError(txtAmount5, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAmount5.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtAmount5, null);
            }
            if (lblDate1Req.Visible && caldate1.Checked == false)
            {
                _errorProvider.SetError(caldate1, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDate1.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(caldate1, null);
            }
            if (lblDate2Req.Visible && caldate2.Checked == false)
            {
                _errorProvider.SetError(caldate2, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDate2.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(caldate2, null);
            }
            if (lblDate3Req.Visible && caldate3.Checked == false)
            {
                _errorProvider.SetError(caldate3, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDate3.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(caldate3, null);
            }
            if (lblDate4Req.Visible && caldate4.Checked == false)
            {
                _errorProvider.SetError(caldate4, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDate4.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(caldate4, null);
            }
            if (lblDate5Req.Visible && caldate5.Checked == false)
            {
                _errorProvider.SetError(caldate5, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDate5.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(caldate5, null);
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

            if (lblFactorReq.Visible && String.IsNullOrEmpty(txtFactor.Text.Trim()))
            {
                _errorProvider.SetError(txtFactor, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFactor.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtFactor, null);
            }

            if (lblSubReq.Visible && String.IsNullOrEmpty(txtSub.Text.Trim()))
            {
                _errorProvider.SetError(txtSub, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSub.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtSub, null);
            }

            if (lblTotalReq.Visible && String.IsNullOrEmpty(txtTotal.Text.Trim()))
            {
                _errorProvider.SetError(txtTotal, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblTotal.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtTotal, null);
            }
            if (txtAmount1.Text.Length > 6)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtAmount1.Text, Consts.StaticVars.TwoDecimalRange6String))
                {
                    _errorProvider.SetError(txtAmount1, Consts.Messages.PleaseEnterDecimals6Range);
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtAmount1, null);
                }
            }
            if (txtAmount2.Text.Length > 6)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtAmount2.Text, Consts.StaticVars.TwoDecimalRange6String))
                {
                    _errorProvider.SetError(txtAmount2, Consts.Messages.PleaseEnterDecimals6Range);
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtAmount2, null);
                }
            }
            if (txtAmount3.Text.Length > 6)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtAmount3.Text, Consts.StaticVars.TwoDecimalRange6String))
                {
                    _errorProvider.SetError(txtAmount3, Consts.Messages.PleaseEnterDecimals6Range);
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtAmount3, null);
                }
            }
            if (txtAmount4.Text.Length > 6)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtAmount4.Text, Consts.StaticVars.TwoDecimalRange6String))
                {
                    _errorProvider.SetError(txtAmount4, Consts.Messages.PleaseEnterDecimals6Range);
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtAmount4, null);
                }
            }
            if (txtAmount5.Text.Length > 6)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtAmount5.Text, Consts.StaticVars.TwoDecimalRange6String))
                {
                    _errorProvider.SetError(txtAmount5, Consts.Messages.PleaseEnterDecimals6Range);
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtAmount5, null);
                }
            }

            return (isValid);
        }

        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (ListItem li in comboBox.Items)
                {
                    if (Convert.ToString(li.Value).Trim().Equals(value.Trim()) || Convert.ToString(li.Text).Trim().Equals(value.Trim()))
                    {
                        comboBox.SelectedItem = li;
                        break;
                    }
                }
            }
        }

        private void SetComboBoxCaseWorkerValue(ComboBox comboBox, string value)
        {
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (ListItem li in comboBox.Items)
                {
                    string strValue = li.Value.ToString();
                    if (strValue.Length > 1)
                    {
                        strValue = strValue.Replace(strValue.Substring(strValue.Length - 1, 1), "");
                    }
                    if (strValue.Trim().Equals(value.Trim()) || Convert.ToString(li.Text).Trim().Equals(value.Trim()))
                    {
                        comboBox.SelectedItem = li;
                        break;
                    }
                }
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                CaseIncomeEntity caseIncomeEntity = new CaseIncomeEntity();
                caseIncomeEntity.Agency = strAgency;
                caseIncomeEntity.Dept = strDept;
                caseIncomeEntity.Program = strProgram;
                caseIncomeEntity.Year = strYear;
                caseIncomeEntity.App = strApplNo;
                caseIncomeEntity.FamilySeq = Convert.ToString(dataGridCaseSnp.SelectedRows[0].Cells["SNP_FAMILY_SEQ"].Value);
                if (Privileges.AddPriv.Equals("false"))
                {
                    if (dataGridCaseIncome.Rows.Count > 0)
                        caseIncomeEntity.Seq = Convert.ToString(dataGridCaseIncome.SelectedRows[0].Cells["INCOME_SEQ"].Value); ;
                }
                else
                {
                    if (dataGridCaseIncome.Rows.Count > 1)
                        caseIncomeEntity.Seq = Convert.ToString(dataGridCaseIncome.SelectedRows[0].Cells["INCOME_SEQ"].Value);
                }

                if (!((ListItem)cmbIncomeType.SelectedItem).Value.ToString().Equals("0"))
                {
                    caseIncomeEntity.Type = ((ListItem)cmbIncomeType.SelectedItem).Value.ToString();

                }

                if (!((ListItem)cmbInterval.SelectedItem).Value.ToString().Equals("0"))
                {
                    caseIncomeEntity.Interval = ((ListItem)cmbInterval.SelectedItem).Value.ToString();

                }
                if (!((ListItem)cmbVerifier.SelectedItem).Value.ToString().Equals("0"))
                {
                    caseIncomeEntity.Verifier = ((ListItem)cmbVerifier.SelectedItem).Value.ToString();
                }
                caseIncomeEntity.Val1 = txtAmount1.Text;
                caseIncomeEntity.Val2 = txtAmount2.Text;
                caseIncomeEntity.Val3 = txtAmount3.Text;
                caseIncomeEntity.Val4 = txtAmount4.Text;
                caseIncomeEntity.Val5 = txtAmount5.Text;
                if (caldate1.Checked)
                    caseIncomeEntity.Date1 = caldate1.Value.ToString();
                if (caldate2.Checked)
                    caseIncomeEntity.Date2 = caldate2.Value.ToString();
                if (caldate3.Checked)
                    caseIncomeEntity.Date3 = caldate3.Value.ToString();
                if (caldate4.Checked)
                    caseIncomeEntity.Date4 = caldate4.Value.ToString();
                if (caldate5.Checked)
                    caseIncomeEntity.Date5 = caldate5.Value.ToString();
                if (chkExclude.Checked)
                {
                    caseIncomeEntity.Exclude = "Y";
                }
                else
                {
                    caseIncomeEntity.Exclude = "N";
                }
                caseIncomeEntity.HowVerified = txtHowVerified.Text;
                caseIncomeEntity.Factor = txtFactor.Text;
                caseIncomeEntity.Source = txtIncSource.Text;
                caseIncomeEntity.TotIncome = txtTotal.Text;
                caseIncomeEntity.ProgIncome = txtTotal.Text;
                caseIncomeEntity.LstcOperator = BaseForm.UserID;
                caseIncomeEntity.AddOperator = BaseForm.UserID;
                caseIncomeEntity.Mode = strMode;
                //txtSub.Text;                
                if (_model.CaseMstData.InsertCaseIncome(caseIncomeEntity))
                {
                   
                    FillGridData(strAgency, strDept, strProgram, strYear, strApplNo);
                    if (dataGridCaseSnp.Rows.Count != 0)
                    {
                        dataGridCaseSnp.Rows[strSnpIndex].Selected = true;
                    }
                    dataGridCaseSnp_SelectionChanged(sender, e);

                    //if (dataGridCaseIncome.Rows.Count > 1)
                    //{
                    //    dataGridCaseIncome.Rows[strIncomeIndex].Selected = true;
                    //}

                }

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtAmount1_TextChanged(object sender, EventArgs e)
        {
            if (txtFactor.Text != string.Empty)
            {
                decimal decAmount1 = 0;
                decimal decAmount2 = 0;
                decimal decAmount3 = 0;
                decimal decAmount4 = 0;
                decimal decAmount5 = 0;
                if (txtAmount1.Text != string.Empty)
                    decAmount1 = Convert.ToDecimal(txtAmount1.Text);
                if (txtAmount2.Text != string.Empty)
                    decAmount2 = Convert.ToDecimal(txtAmount2.Text);
                if (txtAmount3.Text != string.Empty)
                    decAmount3 = Convert.ToDecimal(txtAmount3.Text);
                if (txtAmount4.Text != string.Empty)
                    decAmount4 = Convert.ToDecimal(txtAmount4.Text);
                if (txtAmount5.Text != string.Empty)
                    decAmount5 = Convert.ToDecimal(txtAmount5.Text);
                decimal decTotal = decAmount1 + decAmount2 + decAmount3 + decAmount4 + decAmount5;
                
                if (programDefinitionList.IncomeWeek == "Y")
                {
                    if (((ListItem)cmbInterval.SelectedItem).Value == "W" || ((ListItem)cmbInterval.SelectedItem).Value == "E")
                    {
                        decTotal = decTotal / 4;
                        decTotal = decTotal * Convert.ToDecimal(4.33);
                    }
                    else if (((ListItem)cmbInterval.SelectedItem).Value == "B")
                    {
                        decTotal = decTotal / 2;
                        decTotal = decTotal * Convert.ToDecimal(2.16);
                    }
                }
                decTotal = Math.Round(decTotal, 2);
                txtSub.Text = decTotal.ToString();
                txtTotal.Text = (decTotal * Convert.ToDecimal(txtFactor.Text)).ToString("0.00##");
                //  txtTotal.Text = strTotal.ToString("0.00##"); //decimal strTotal

            }
        }

        private void ShowFactor()
        {

            if (((ListItem)cmbInterval.SelectedItem).Value == "Q")
            {
                txtFactor.Text = "4.00";
            }
            else if (((ListItem)cmbInterval.SelectedItem).Value == "N")
            {
                txtFactor.Text = "2.00";
            }
            else if (((ListItem)cmbInterval.SelectedItem).Value == "S" || ((ListItem)cmbInterval.SelectedItem).Value == "M")
            {
                txtFactor.Text = "12.00";
            }
            else if (((ListItem)cmbInterval.SelectedItem).Value == "A" || ((ListItem)cmbInterval.SelectedItem).Value == "O" || ((ListItem)cmbInterval.SelectedItem).Value == "E")
            {
                txtFactor.Text = "1.00";
            }
            else if (((ListItem)cmbInterval.SelectedItem).Value == "W" || ((ListItem)cmbInterval.SelectedItem).Value == "B")
            {
                ProgramDefinitionEntity programDefinitionList = _model.HierarchyAndPrograms.GetCaseDepadp(strAgency, strDept, strProgram);
                if (programDefinitionList.IncomeWeek == "Y")
                    txtFactor.Text = "12.00";
                else
                    txtFactor.Text = "13.00";
            }
            else if (((ListItem)cmbInterval.SelectedItem).Value == "0")
            {
                txtFactor.Text = string.Empty;
            }
            else if (((ListItem)cmbInterval.SelectedItem).Value == "3")
            {
                txtFactor.Text = programDefinitionList.DepIncExcIntFactr1;
                ShowDateLabelDisplay(-30);

            }
            else if (((ListItem)cmbInterval.SelectedItem).Value == "6")
            {
                txtFactor.Text = programDefinitionList.DepIncExcIntFactr2;
                ShowDateLabelDisplay(-60);
            }
            else if (((ListItem)cmbInterval.SelectedItem).Value == "9")
            {
                txtFactor.Text = programDefinitionList.DepIncExcIntFactr3;
                ShowDateLabelDisplay(-90);
            }

        }

        private void ShowDateLabelDisplay(int intdays)
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

        private void dataGridCaseIncome_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridCaseIncome.ColumnCount - 1 && e.RowIndex != -1)
            {
                if (dataGridCaseIncome.SelectedRows.Count > 0)
                {
                    if (dataGridCaseIncome.SelectedRows[0].Tag is CaseIncomeEntity)
                    {
                        CaseIncomeEntity row = dataGridCaseIncome.SelectedRows[0].Tag as CaseIncomeEntity;
                        if (row != null)
                        {
                            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose:OnDeleteMessageBoxClicked);

                        }
                    }
                }
            }
        }

        private void CaseIncome_Load(object sender, EventArgs e)
        {
            FillGridData(strAgency, strDept, strProgram, strYear, strApplNo);
            dataGridCaseSnp_SelectionChanged(sender, e);
            ShowFactor();

            DisableControls();
            EnableDisableControls();
            RequiredAmountControl();

            if (!FLDCNTLHieEntity.Exists(u => u.Enab.Equals("Y")))
            {
                CommonFunctions.MessageBoxDisplay("Field controls not defined for this program");
                btnAdd.Enabled = false;
            }
        }

        private void OnDeleteMessageBoxClicked(DialogResult dialogResult)
        {
           // MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;

            if (dialogResult == DialogResult.Yes)
            {
                CaseIncomeEntity caseIncomeEntity = new CaseIncomeEntity();
                caseIncomeEntity.Agency = strAgency;
                caseIncomeEntity.Dept = strDept;
                caseIncomeEntity.Program = strProgram;
                caseIncomeEntity.Year = strYear;
                caseIncomeEntity.App = strApplNo;
                caseIncomeEntity.FamilySeq = Convert.ToString(dataGridCaseSnp.SelectedRows[0].Cells["SNP_FAMILY_SEQ"].Value); ;
                caseIncomeEntity.Seq = Convert.ToString(dataGridCaseIncome.SelectedRows[0].Cells["INCOME_SEQ"].Value); ;
                if (_model.CaseMstData.DeleteCaseIncome(caseIncomeEntity))
                {
                    FillGridData(strAgency, strDept, strProgram, strYear, strApplNo);
                    if (dataGridCaseSnp.Rows.Count != 0)
                    {
                        dataGridCaseSnp.Rows[strSnpIndex].Selected = true;
                    }

                    //kranthi commented//
                  //  dataGridCaseSnp_SelectionChanged(sender, e);

                }
            }
        }

        private void lblVerifier_Click(object sender, EventArgs e)
        {
        }
        IncomeReportForm Pdf_Form;
        private void PbPdf_Click(object sender, EventArgs e)
        {
            string Temp_Year = "    ";
            if (!string.IsNullOrEmpty(strYear))
                Temp_Year = strYear;
            Pdf_Form = new IncomeReportForm(strAgency + strDept + strProgram + Temp_Year + strApplNo, BaseForm,txtHowVerified.Text.Trim());
            //Pdf_Form.FormClosed += new Form.FormClosedEventHandler(OnSerachFormClosed);
            //Pdf_Form.ShowDialog();
            //Pdf_Form.Close();
            if (Pdf_Form.DialogResult == DialogResult.OK)
            {
                if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
                {
                    PdfViewerNewForm objfrm = new PdfViewerNewForm(Pdf_Form.Get_Pdf_Path());
                    objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                    objfrm.ShowDialog();
                }
                else
                {
                    FrmViewer objfrm = new FrmViewer(Pdf_Form.Get_Pdf_Path());
                    objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
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

        }

        //private void OnSerachFormClosed(object sender, FormClosedEventArgs e)
        //{
        //    IncomeReportForm form = sender as IncomeReportForm;
        //    if (form.DialogResult == DialogResult.OK)
        //    {
        //        FrmViewer objfrm = new FrmViewer(form.Get_Pdf_Path());
        //        objfrm.ShowDialog();
        //    }
        //}

        private void EnableDisableControls()
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
                            if (enabled) { cmbVerifier.Enabled = lblVerifier.Enabled = true; if (required) lblVerifierReq.Visible = true; } else { cmbVerifier.Enabled = lblVerifier.Enabled = false; lblVerifierReq.Visible = false; }
                            break;
                        case Consts.CASEINCOME.IncomeType:
                            if (enabled) { cmbIncomeType.Enabled = lblIncomeType.Enabled = true; if (required) lblIncomeTypeReq.Visible = true; } else { cmbIncomeType.Enabled = lblIncomeType.Enabled = false; lblIncomeTypeReq.Visible = false; }
                            break;
                        case Consts.CASEINCOME.Interval:
                            if (enabled) { cmbInterval.Enabled = lblInterval.Enabled = true; if (required) lblIntervalReq.Visible = true; } else { cmbInterval.Enabled = lblInterval.Enabled = false; lblIntervalReq.Visible = false; }
                            break;
                        case Consts.CASEINCOME.Value1:
                            if (enabled) { txtAmount1.Enabled = lblAmount1.Enabled = true; if (required) lblAmount1Req.Visible = true; } else { txtAmount1.Enabled = lblAmount1.Enabled = false; lblAmount1Req.Visible = false; }
                            break;
                        case Consts.CASEINCOME.Value2:
                            if (enabled) { txtAmount2.Enabled = lblAmount2.Enabled = true; if (required) lblAMount2Req.Visible = true; } else { txtAmount2.Enabled = lblAmount2.Enabled = false; lblAMount2Req.Visible = false; }
                            break;
                        case Consts.CASEINCOME.Value3:
                            if (enabled) { txtAmount3.Enabled = lblAmount3.Enabled = true; if (required) lblAmount3Req.Visible = true; } else { txtAmount3.Enabled = lblAmount3.Enabled = false; lblAmount3Req.Visible = false; }
                            break;
                        case Consts.CASEINCOME.Value4:
                            if (enabled) { txtAmount4.Enabled = lblAmount4.Enabled = true; if (required) lblAmount4Req.Visible = true; } else { txtAmount4.Enabled = lblAmount4.Enabled = false; lblAmount4Req.Visible = false; }
                            break;
                        case Consts.CASEINCOME.Value5:
                            if (enabled) { txtAmount5.Enabled = lblAmount5.Enabled = true; if (required) lblAmount5Req.Visible = true; } else { txtAmount5.Enabled = lblAmount5.Enabled = false; lblAmount5Req.Visible = false; }
                            break;
                        case Consts.CASEINCOME.Date1:
                            if (enabled) { caldate1.Enabled = lblDate1.Enabled = true; if (required) lblDate1Req.Visible = true; } else { caldate1.Enabled = lblDate1.Enabled = false; lblDate1Req.Visible = false; }
                            break;
                        case Consts.CASEINCOME.Date2:
                            if (enabled) { caldate2.Enabled = lblDate2.Enabled = true; if (required) lblDate2Req.Visible = true; } else { caldate2.Enabled = lblDate2.Enabled = false; lblDate2Req.Visible = false; }
                            break;
                        case Consts.CASEINCOME.Date3:
                            if (enabled) { caldate3.Enabled = lblDate3.Enabled = true; if (required) lblDate3Req.Visible = true; } else { caldate3.Enabled = lblDate3.Enabled = false; lblDate3Req.Visible = false; }
                            break;
                        case Consts.CASEINCOME.Date4:
                            if (enabled) { caldate4.Enabled = lblDate4.Enabled = true; if (required) lblDate4Req.Visible = true; } else { caldate4.Enabled = lblDate4.Enabled = false; lblDate4Req.Visible = false; }
                            break;
                        case Consts.CASEINCOME.Date5:
                            if (enabled) { caldate5.Enabled = lblDate5.Enabled = true; if (required) lblDate5Req.Visible = true; } else { caldate5.Enabled = lblDate5.Enabled = false; lblDate5Req.Visible = false; }
                            break;
                        case Consts.CASEINCOME.IncomeSource:
                            if (enabled) { txtIncSource.Enabled = lblIncomeSource.Enabled = true; if (required) lblIncomeSourceReq.Visible = true; } else { txtIncSource.Enabled = lblIncomeSource.Enabled = false; lblIncomeSourceReq.Visible = false; }
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
                            if (enabled) { chkExclude.Enabled = true; if (required) lblExcludeReq.Visible = true; } else { chkExclude.Enabled = false; lblExcludeReq.Visible = false; }
                            break;
                    }
                }
            }
        }

        private void RequiredAmountControl()
        {
            if (lblAmount1.Visible == false)
                lblAmount1Req.Visible = false;
            if (lblAmount2.Visible == false)
                lblAMount2Req.Visible = false;
            if (lblAmount3.Visible == false)
                lblAmount3Req.Visible = false;
            if (lblAmount4.Visible == false)
                lblAmount4Req.Visible = false;
            if (lblAmount5.Visible == false)
                lblAmount5Req.Visible = false;
            if (lblDate1.Visible == false)
                lblDate1Req.Visible = false;
            if (lblDate2.Visible == false)
                lblDate2Req.Visible = false;
            if (lblDate3.Visible == false)
                lblDate3Req.Visible = false;
            if (lblDate4.Visible == false)
                lblDate4Req.Visible = false;
            if (lblDate5.Visible == false)
                lblDate5Req.Visible = false;
        }

        private void EnableDisableControlsInterval()
        {
            if (!(programDefinitionList.IncomeTypeOnly == "Y" && Privileges.PrivilegeName == "CASE2001"))
            {
                foreach (FldcntlHieEntity entity in FLDCNTLHieEntity)
                {
                    bool required = entity.Req.Equals("Y") ? true : false;
                    bool enabled = entity.Enab.Equals("Y") ? true : false;

                    switch (entity.FldDesc)
                    {

                        case Consts.CASEINCOME.Value1:
                            if (enabled) { txtAmount1.Enabled = lblAmount1.Enabled = true; if (required) lblAmount1Req.Visible = true; } else { txtAmount1.Enabled = lblAmount1.Enabled = false; lblAmount1Req.Visible = false; }
                            break;
                        case Consts.CASEINCOME.Value2:
                            if (enabled) { txtAmount2.Enabled = lblAmount2.Enabled = true; if (required) lblAMount2Req.Visible = true; } else { txtAmount2.Enabled = lblAmount2.Enabled = false; lblAMount2Req.Visible = false; }
                            break;
                        case Consts.CASEINCOME.Value3:
                            if (enabled) { txtAmount3.Enabled = lblAmount3.Enabled = true; if (required) lblAmount3Req.Visible = true; } else { txtAmount3.Enabled = lblAmount3.Enabled = false; lblAmount3Req.Visible = false; }
                            break;
                        case Consts.CASEINCOME.Value4:
                            if (enabled) { txtAmount4.Enabled = lblAmount4.Enabled = true; if (required) lblAmount4Req.Visible = true; } else { txtAmount4.Enabled = lblAmount4.Enabled = false; lblAmount4Req.Visible = false; }
                            break;
                        case Consts.CASEINCOME.Value5:
                            if (enabled) { txtAmount5.Enabled = lblAmount5.Enabled = true; if (required) lblAmount5Req.Visible = true; } else { txtAmount5.Enabled = lblAmount5.Enabled = false; lblAmount5Req.Visible = false; }
                            break;
                        case Consts.CASEINCOME.Date1:
                            if (enabled) { caldate1.Enabled = lblDate1.Enabled = true; if (required) lblDate1Req.Visible = true; } else { caldate1.Enabled = lblDate1.Enabled = false; lblDate1Req.Visible = false; }
                            break;
                        case Consts.CASEINCOME.Date2:
                            if (enabled) { caldate2.Enabled = lblDate2.Enabled = true; if (required) lblDate2Req.Visible = true; } else { caldate2.Enabled = lblDate2.Enabled = false; lblDate2Req.Visible = false; }
                            break;
                        case Consts.CASEINCOME.Date3:
                            if (enabled) { caldate3.Enabled = lblDate3.Enabled = true; if (required) lblDate3Req.Visible = true; } else { caldate3.Enabled = lblDate3.Enabled = false; lblDate3Req.Visible = false; }
                            break;
                        case Consts.CASEINCOME.Date4:
                            if (enabled) { caldate4.Enabled = lblDate4.Enabled = true; if (required) lblDate4Req.Visible = true; } else { caldate4.Enabled = lblDate4.Enabled = false; lblDate4Req.Visible = false; }
                            break;
                        case Consts.CASEINCOME.Date5:
                            if (enabled) { caldate5.Enabled = lblDate5.Enabled = true; if (required) lblDate5Req.Visible = true; } else { caldate5.Enabled = lblDate5.Enabled = false; lblDate5Req.Visible = false; }
                            break;

                    }
                }
            }
        }

        private void DisableControls()
        {
            lblAmount1.Enabled = false;
            lblAmount2.Enabled = false;
            lblAmount3.Enabled = false;
            lblAmount4.Enabled = false;
            lblAmount5.Enabled = false; ;
            lblDate1.Enabled = false; ;
            lblDate2.Enabled = false;
            lblDate3.Enabled = false;
            lblDate4.Enabled = false;
            lblDate5.Enabled = false;
            // lblFactor.Enabled = false;            
            //lblHowVerified.Enabled = false; ;
            // lblIncomeSource.Enabled = false; ;
            lblIncomeType.Enabled = false;
            lblInHouse.Enabled = false; ;
            lblInProg.Enabled = false;
            lblInterval.Enabled = false; ;
            //lblSub.Enabled = false;
            // lblTotal.Enabled = false;
            lblVerifier.Enabled = false;
            txtAmount1.Enabled = false;
            txtAmount2.Enabled = false;
            txtAmount3.Enabled = false;
            txtAmount4.Enabled = false;
            txtAmount5.Enabled = false;
            txtFactor.Enabled = false;
            // txtHowVerified.Enabled = false;
            // txtIncSource.Enabled = false;
            txtInHouse.Enabled = false;
            txtInProg.Enabled = false;
            txtProgIncome.Enabled = false;
            txtSub.Enabled = false;
            txtTotal.Enabled = false;
            txtTotIncome.Enabled = false;
            caldate1.Enabled = false;
            caldate2.Enabled = false;
            caldate3.Enabled = false;
            caldate4.Enabled = false;
            caldate5.Enabled = false;
            cmbIncomeType.Enabled = false;
            cmbInterval.Enabled = false;
            cmbVerifier.Enabled = false;
            chkExclude.Enabled = false;



        }

        private void caldate1_ValueChanged(object sender, EventArgs e)
        {
            lblDate1Range.Visible = false;
            if (caldate1.Checked == true)
                if (!CheckMstIntakeDate(caldate1.Value))
                    lblDate1Range.Visible = true;
        }

        private bool CheckMstIntakeDate(DateTime datPresentDate)
        {
            bool booldate = true;
            string strInterval = ((ListItem)cmbInterval.SelectedItem).Value==null?string.Empty:((ListItem)cmbInterval.SelectedItem).Value.ToString();
            if (strInterval == "3" || strInterval == "6" || strInterval == "9")
            {
                if (!((MstIntakeStartDate <= datPresentDate) && (datPresentDate <= MstIntakeEndDate)))
                {
                    //MessageBox.Show("Please enter window date and Intake date between only");
                    booldate = false;
                }
            }
            return booldate;
        }

        private void caldate2_ValueChanged(object sender, EventArgs e)
        {
            lblDate2Range.Visible = false;
            if (caldate2.Checked == true)
                if (!CheckMstIntakeDate(caldate2.Value))
                    lblDate2Range.Visible = true;

        }

        private void caldate3_ValueChanged(object sender, EventArgs e)
        {
            lblDate3Range.Visible = false;
            if (caldate3.Checked == true)
                if (!CheckMstIntakeDate(caldate3.Value)) lblDate3Range.Visible = true;

        }

        private void caldate4_ValueChanged(object sender, EventArgs e)
        {
            lblDate4Range.Visible = false;
            if (caldate4.Checked == true)
                if (!CheckMstIntakeDate(caldate4.Value))
                    lblDate4Range.Visible = true;
        }

        private void caldate5_ValueChanged(object sender, EventArgs e)
        {
            lblDate5Range.Visible = false;
            if (caldate5.Checked == true)
                if (!CheckMstIntakeDate(caldate5.Value))
                    lblDate5Range.Visible = true;

        }

        private void cmbVerifier_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbVerifier.SelectedItem).Value.ToString() != "0")
                if (((ListItem)cmbVerifier.SelectedItem).ID.ToString() != "N")
                    MessageBox.Show("Inactive CaseWorker", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void EnableAllcontrols()
        {
            txtAmount1.ReadOnly = true;
            txtAmount2.ReadOnly = true;
            txtAmount3.ReadOnly = true;
            txtAmount4.ReadOnly = true;
            txtAmount5.ReadOnly = true;
            caldate1.Enabled = false;
            caldate2.Enabled = false;
            caldate3.Enabled = false;
            caldate4.Enabled = false;
            caldate5.Enabled = false;
            cmbIncomeType.Enabled = false;
            cmbInterval.Enabled = false;
            cmbVerifier.Enabled = false;
            // txtHowVerified.ReadOnly = true;
            //  txtIncSource.ReadOnly = true;
            chkExclude.Enabled = false;
            btnAdd.Visible = false;
            btnCancel.Text = "Close";
        }

        private void EnableAllcontrols(bool booltrue)
        {
            txtAmount1.ReadOnly = false;
            txtAmount2.ReadOnly = false;
            txtAmount3.ReadOnly = false;
            txtAmount4.ReadOnly = false;
            txtAmount5.ReadOnly = false;
            caldate1.Enabled = true;
            caldate2.Enabled = true;
            caldate3.Enabled = true;
            caldate4.Enabled = true;
            caldate5.Enabled = true;
            cmbIncomeType.Enabled = true;
            cmbInterval.Enabled = true;
            cmbVerifier.Enabled = true;
            //  txtHowVerified.ReadOnly = false;
            //  txtIncSource.ReadOnly = false;
            chkExclude.Enabled = true;
            btnAdd.Visible = true;
            btnCancel.Text = "Close";
        }

        private void txtAmount1_Leave(object sender, EventArgs e)
        {
            if (txtAmount1.Text.Length > 6)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtAmount1.Text, Consts.StaticVars.TwoDecimalRange6String))
                {

                    CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals6Range);
                    //txtEhourlywage.Text = string.Empty;
                }
            }
        }

        private void txtAmount2_Leave(object sender, EventArgs e)
        {
            if (txtAmount2.Text.Length > 6)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtAmount2.Text, Consts.StaticVars.TwoDecimalRange6String))
                {

                    CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals6Range);
                    //txtEhourlywage.Text = string.Empty;
                }
            }
        }

        private void txtAmount3_Leave(object sender, EventArgs e)
        {
            if (txtAmount3.Text.Length > 6)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtAmount3.Text, Consts.StaticVars.TwoDecimalRange6String))
                {

                    CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals6Range);
                    //txtEhourlywage.Text = string.Empty;
                }
            }
        }

        private void txtAmount4_Leave(object sender, EventArgs e)
        {
            if (txtAmount4.Text.Length > 6)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtAmount4.Text, Consts.StaticVars.TwoDecimalRange6String))
                {

                    CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals6Range);
                    //txtEhourlywage.Text = string.Empty;
                }
            }

        }

        private void txtAmount5_Leave(object sender, EventArgs e)
        {
            if (txtAmount5.Text.Length > 6)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtAmount5.Text, Consts.StaticVars.TwoDecimalRange6String))
                {

                    CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals6Range);
                    //txtEhourlywage.Text = string.Empty;
                }
            }
        }

        private void cmbIncomeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbIncomeType.SelectedItem).Value.ToString() != "0")
                if (((ListItem)cmbIncomeType.SelectedItem).ID.ToString() == "Y")
                    chkExclude.Checked = true;
                else
                    chkExclude.Checked = false;
        }

    }

}
