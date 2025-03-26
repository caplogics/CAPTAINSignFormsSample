using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using DevExpress.Pdf;
using DevExpress.XtraRichEdit.Model;
using iTextSharp.text.pdf.security;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class Caseincome18NOINCForm : Form
    {
        List<CaseSnpEntity> _caseSnp18lesMembs = new List<CaseSnpEntity>();
        BaseForm _baseForm = null;
        List<CommonEntity> _propRelation { get; set; }
        public ProgramDefinitionEntity _programDefinitionList { get; set; }
        List<CommonEntity> _Disableditems { get; set; }
        private CaptainModel _model = null;
        public List<CommonEntity> _commonInterval { get; set; }
        public List<CommonEntity> _lookUpIncomeTypes { get; set; }
        string _strCaseWorkerDefaultCode = "";
        public Caseincome18NOINCForm(BaseForm obaseForm, List<CaseSnpEntity> caseSnp18lesMembs, List<CommonEntity> propRelation,
            ProgramDefinitionEntity programDefinitionList, List<CommonEntity> disableditems, List<CommonEntity> lookUpIncomeTypes, List<CommonEntity> commonInterval, string strCaseWorkerDefaultCode)
        {
            InitializeComponent();
            _model = new CaptainModel();
            this.Text = "Income Details Control for < 18 and no income";
            _caseSnp18lesMembs = caseSnp18lesMembs;
            _baseForm = obaseForm;
            _propRelation = propRelation;
            _programDefinitionList = programDefinitionList;
            _Disableditems = disableditems;
            _lookUpIncomeTypes = lookUpIncomeTypes;
            _commonInterval = commonInterval;
            _strCaseWorkerDefaultCode= strCaseWorkerDefaultCode;
            FillGrid();
        }
        void FillGrid()
        {
            int rowIndex = 0;
            foreach (CaseSnpEntity caseSnp in _caseSnp18lesMembs)
            {
                string memberCode = string.Empty;
                CommonEntity rel = _propRelation.Find(u => u.Code.Equals(caseSnp.MemberCode));
                if (rel != null) memberCode = rel.Desc;
                string name = LookupDataAccess.GetMemberName(caseSnp.NameixFi, caseSnp.NameixMi, caseSnp.NameixLast, _baseForm.BaseHierarchyCnFormat);
                string strAltDate = LookupDataAccess.Getdate(caseSnp.AltBdate);
                string strSsno = LookupDataAccess.GetCardNo(caseSnp.Ssno, "1", _programDefinitionList.SSNReasonFlag.Trim(), caseSnp.SsnReason);
                string cellTotIncome = !caseSnp.TotIncome.ToString().Equals(string.Empty) ? caseSnp.TotIncome : "0.0";
                string cellProgIncome = !caseSnp.ProgIncome.Equals(string.Empty) ? caseSnp.ProgIncome : "0.0";

                cellProgIncome = Decimal.Parse(cellProgIncome).ToString("N", new CultureInfo("en-US"));
                cellTotIncome = Decimal.Parse(cellTotIncome).ToString("N", new CultureInfo("en-US"));


                string Age = caseSnp.Age.ToString();
                string disabled = caseSnp.Disable.ToString();
                if (!string.IsNullOrEmpty(caseSnp.Disable.Trim()))
                {
                    if (_Disableditems.Count > 0)
                    {
                        foreach (CommonEntity Ent in _Disableditems)
                        {
                            if (Ent.Code.Trim() == caseSnp.Disable.Trim())
                            {
                                disabled = Ent.Desc.Trim();
                                break;
                            }
                        }
                    }
                }

                rowIndex = dataGridCaseSnp.Rows.Add(caseSnp.FamilySeq, name, cellTotIncome, cellProgIncome, memberCode, Age, caseSnp.CurrentAge.ToString(), disabled);
                dataGridCaseSnp.Rows[rowIndex].Tag = caseSnp;
                if (caseSnp.Status.Trim() != "A")
                    dataGridCaseSnp.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                if (caseSnp.Exclude == "Y")
                    dataGridCaseSnp.Rows[rowIndex].Cells["Relation"].Style.ForeColor = Color.Red;

                CommonFunctions.setTooltip(rowIndex, caseSnp.AddOperator, caseSnp.DateAdd, caseSnp.LstcOperator, caseSnp.DateLstc, dataGridCaseSnp);
            }
        }

        string getNoneIncomeType()
        {
            string _result = "";
            if (_lookUpIncomeTypes.Count > 0)
            {
                List<CommonEntity> _lstnone = _lookUpIncomeTypes.Where(u => u.Desc.ToLower().Contains("none")).ToList();
                if (_lstnone.Count > 0)
                    _result = _lstnone[0].Code;
            }
            return _result;
        }
        string getNoneInterval()
        {
            string _result = "";
            if (_commonInterval.Count > 0)
            {
                List<CommonEntity> _lstnone = _commonInterval.Where(u => u.Desc.ToLower().Contains("none")).ToList();
                if (_lstnone.Count > 0)
                    _result = _lstnone[0].Code;
            }
            return _result;
        }

        public void UpdateCaseIncome(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                string IncomeType = getNoneIncomeType();
                string IncomeInterval = getNoneInterval();
                int seq = 0; bool boolSucess = false;
                foreach (DataGridViewRow _dRow in dataGridCaseSnp.Rows)
                {
                    string _FamSeq = _dRow.Cells["SNP_FAMILY_SEQ"].Value.ToString();
                    seq++;
                    CaseIncomeEntity caseIncomeEntity = new CaseIncomeEntity();
                    caseIncomeEntity.Agency = _baseForm.BaseAgency;
                    caseIncomeEntity.Dept = _baseForm.BaseDept;
                    caseIncomeEntity.Program = _baseForm.BaseProg;
                    caseIncomeEntity.Year = _baseForm.BaseYear;
                    caseIncomeEntity.App = _baseForm.BaseApplicationNo;
                    caseIncomeEntity.FamilySeq = Convert.ToString(_FamSeq);
                    caseIncomeEntity.Seq = seq.ToString(); //Convert.ToString(item.Cells["IncomeSeq"].Value); ;
                    caseIncomeEntity.Type = IncomeType; // Convert.ToString(item.Cells["IncomeTypeCode"].Value);


                    caseIncomeEntity.Interval = IncomeInterval; // Convert.ToString(item.Cells["Interval"].Value);
                    caseIncomeEntity.Verifier = _strCaseWorkerDefaultCode;

                    caseIncomeEntity.Val1 = "0.00";
                    caseIncomeEntity.Val2 = "0.00";
                    caseIncomeEntity.Val3 = "0.00";
                    caseIncomeEntity.Val4 = "0.00";
                    caseIncomeEntity.Val5 = "0.00";
                    caseIncomeEntity.Date1 = string.Empty;
                    caseIncomeEntity.Date2 = string.Empty;
                    caseIncomeEntity.Date3 = string.Empty;
                    caseIncomeEntity.Date4 = string.Empty;
                    caseIncomeEntity.Date5 = string.Empty;

                    caseIncomeEntity.Exclude = "N";
                    caseIncomeEntity.HowVerified = "";// item.Cells["gvtHowverfier"].Value.ToString();
                    caseIncomeEntity.Factor = "1.00"; // item.Cells["Factor"].Value.ToString();
                    caseIncomeEntity.Source = "";// item.Cells["gvtIncomesource"].Value.ToString();
                    caseIncomeEntity.TotIncome = "";// item.Cells["Total"].Value.ToString();
                    caseIncomeEntity.ProgIncome = "";// item.Cells["Total"].Value.ToString();

                    caseIncomeEntity.HrRate1 = string.Empty;
                    caseIncomeEntity.HrRate2 = string.Empty;
                    caseIncomeEntity.HrRate3 = string.Empty;
                    caseIncomeEntity.HrRate4 = string.Empty;
                    caseIncomeEntity.HrRate5 = string.Empty;
                    caseIncomeEntity.Average = string.Empty;
                    
                    caseIncomeEntity.LstcOperator = _baseForm.UserID;
                    caseIncomeEntity.AddOperator = _baseForm.UserID;
                    caseIncomeEntity.Mode = string.Empty; //item.Cells["Status"].Value.ToString();
                                                          //txtSub.Text;                
                    if (_model.CaseMstData.InsertCaseIncome(caseIncomeEntity))
                    {
                        boolSucess = true;
                    }
                }

                if (boolSucess)
                {
                    AlertBox.Show("Saved Successfully");
                    this.Close();
                }
            }
        }
        private void btnYes_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Are you sure want to set Income types to 'None' for the members?", "CAPTAIN", MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: UpdateCaseIncome);
            //foreach (DataGridViewRow _dRow in dataGridCaseSnp.Rows)
            //{
            //    string _FamSeq = _dRow.Cells["SNP_FAMILY_SEQ"].Value.ToString();
            //    CaseIncomeEntity caseIncomeEntity = new CaseIncomeEntity();
            //    caseIncomeEntity.Agency = _baseForm.BaseAgency;
            //    caseIncomeEntity.Dept = _baseForm.BaseDept;
            //    caseIncomeEntity.Program = _baseForm.BaseProg;
            //    caseIncomeEntity.Year = _baseForm.BaseYear;
            //    caseIncomeEntity.App = _baseForm.BaseApplicationNo;
            //    caseIncomeEntity.FamilySeq = Convert.ToString(_FamSeq); 
            //    caseIncomeEntity.Seq = string.Empty;
            //    bool booldeletestatus = true;
            //    //if (Privileges.DelPriv.Equals("false"))
            //    //{
            //    //    if (propIncomeTypeSwitch != "Y")
            //    //    {
            //    //        foreach (DataGridViewRow item in dataGridCaseIncome.Rows)
            //    //        {
            //    //            if (item.Cells["Status"].Value.ToString() == "U")
            //    //            {
            //    //                if ((string.IsNullOrEmpty(item.Cells["Factor"].Value.ToString())))
            //    //                {
            //    //                    booldeletestatus = false;
            //    //                    break;
            //    //                }
            //    //            }
            //    //        }
            //    //    }
            //    //}
            //    if (booldeletestatus)
            //    {

            //        strMode = string.Empty;
            //        MenuPropertie = string.Empty;
            //        btnSave.Visible = false;
            //        btnCancel.Visible = false;
            //        dataGridCaseIncome.ReadOnly = true;
            //        if (Privileges.DelPriv.Equals("false"))
            //        {
            //            dataGridCaseIncome.Columns["Delete"].Visible = false;
            //        }
            //        else
            //            dataGridCaseIncome.Columns["Delete"].Visible = true;

            //        if (_model.CaseMstData.DeleteCaseIncome(caseIncomeEntity))
            //        {
            //            bool boolSucess = false;
            //            int intgridloop = 0;
            //            string strSqlMsg;
            //            foreach (DataGridViewRow item in dataGridCaseIncome.Rows)
            //            {
            //                if (!(string.IsNullOrEmpty(item.Cells["Factor"].Value.ToString())))
            //                {
            //                    intgridloop++;
            //                    caseIncomeEntity.Agency = _baseForm.BaseAgency;
            //                    caseIncomeEntity.Dept = _baseForm.BaseDept;
            //                    caseIncomeEntity.Program = _baseForm.BaseProg;
            //                    caseIncomeEntity.Year = _baseForm.BaseYear;
            //                    caseIncomeEntity.App = _baseForm.BaseApplicationNo;
            //                    caseIncomeEntity.FamilySeq = Convert.ToString(_FamSeq);
            //                    caseIncomeEntity.Seq = Convert.ToString(item.Cells["IncomeSeq"].Value); ;
            //                    caseIncomeEntity.Type = Convert.ToString(item.Cells["IncomeTypeCode"].Value);



            //                    //if (!((ListItem)cmbInterval.SelectedItem).Value.ToString().Equals("0"))
            //                    //{
            //                    caseIncomeEntity.Interval = Convert.ToString(item.Cells["Interval"].Value);

            //                    // }
            //                    if (item.Cells["gvtVerifier"].Value.ToString() != "0")
            //                    {
            //                        caseIncomeEntity.Verifier = item.Cells["gvtVerifier"].Value.ToString();
            //                    }

            //                    caseIncomeEntity.Val1 = string.Empty;
            //                    caseIncomeEntity.Val2 = string.Empty;
            //                    caseIncomeEntity.Val3 = string.Empty;
            //                    caseIncomeEntity.Val4 = string.Empty;
            //                    caseIncomeEntity.Val5 = string.Empty;
            //                    if (item.Cells["Amt1"].Value != null)
            //                        caseIncomeEntity.Val1 = item.Cells["Amt1"].Value.ToString();
            //                    if (item.Cells["Amt2"].Value != null)
            //                        caseIncomeEntity.Val2 = item.Cells["Amt2"].Value.ToString();
            //                    if (item.Cells["Amt3"].Value != null)
            //                        caseIncomeEntity.Val3 = item.Cells["Amt3"].Value.ToString();
            //                    if (item.Cells["Amt4"].Value != null)
            //                        caseIncomeEntity.Val4 = item.Cells["Amt4"].Value.ToString();
            //                    if (item.Cells["Amt5"].Value != null)
            //                        caseIncomeEntity.Val5 = item.Cells["Amt5"].Value.ToString();
            //                    caseIncomeEntity.Date1 = string.Empty;
            //                    caseIncomeEntity.Date2 = string.Empty;
            //                    caseIncomeEntity.Date3 = string.Empty;
            //                    caseIncomeEntity.Date4 = string.Empty;
            //                    caseIncomeEntity.Date5 = string.Empty;
            //                    //if (caldate1.Checked)
            //                    if (item.Cells["Date1"].Value.ToString() != string.Empty && item.Cells["Date1"].Value.ToString().Trim() != "/  /")
            //                        caseIncomeEntity.Date1 = LookupDataAccess.GetFormatdate(item.Cells["Date1"].Value.ToString());
            //                    // if (caldate2.Checked)
            //                    if (item.Cells["Date2"].Value.ToString() != string.Empty && item.Cells["Date2"].Value.ToString().Trim() != "/  /")
            //                        caseIncomeEntity.Date2 = LookupDataAccess.GetFormatdate(item.Cells["Date2"].Value.ToString());
            //                    //  if (caldate3.Checked)
            //                    if (item.Cells["Date3"].Value.ToString() != string.Empty && item.Cells["Date3"].Value.ToString().Trim() != "/  /")
            //                        caseIncomeEntity.Date3 = LookupDataAccess.GetFormatdate(item.Cells["Date3"].Value.ToString());
            //                    //  if (caldate4.Checked)
            //                    if (item.Cells["Date4"].Value.ToString() != string.Empty && item.Cells["Date4"].Value.ToString().Trim() != "/  /")
            //                        caseIncomeEntity.Date4 = LookupDataAccess.GetFormatdate(item.Cells["Date4"].Value.ToString());
            //                    //  if (caldate5.Checked)
            //                    if (item.Cells["Date5"].Value.ToString() != string.Empty && item.Cells["Date5"].Value.ToString().Trim() != "/  /")
            //                        caseIncomeEntity.Date5 = LookupDataAccess.GetFormatdate(item.Cells["Date5"].Value.ToString());
            //                    if (Convert.ToBoolean(item.Cells["Exclude"].Value) == true)
            //                    {
            //                        caseIncomeEntity.Exclude = "Y";
            //                    }
            //                    else
            //                    {
            //                        caseIncomeEntity.Exclude = "N";
            //                    }
            //                    caseIncomeEntity.HowVerified = item.Cells["gvtHowverfier"].Value.ToString();
            //                    caseIncomeEntity.Factor = item.Cells["Factor"].Value.ToString();
            //                    caseIncomeEntity.Source = item.Cells["gvtIncomesource"].Value.ToString();
            //                    caseIncomeEntity.TotIncome = item.Cells["Total"].Value.ToString();
            //                    caseIncomeEntity.ProgIncome = item.Cells["Total"].Value.ToString();

            //                    caseIncomeEntity.HrRate1 = string.Empty;
            //                    caseIncomeEntity.HrRate2 = string.Empty;
            //                    caseIncomeEntity.HrRate3 = string.Empty;
            //                    caseIncomeEntity.HrRate4 = string.Empty;
            //                    caseIncomeEntity.HrRate5 = string.Empty;
            //                    caseIncomeEntity.Average = string.Empty;
            //                    if (propHourlyMode.ToUpper() == "Y")
            //                    {
            //                        caseIncomeEntity.Average = item.Cells["Sub"].Value.ToString();
            //                    }
            //                    if (caseIncomeEntity.Interval == "H")
            //                    {
            //                        caseIncomeEntity.HrRate1 = item.Cells["gvtHrRate1"].Value.ToString();
            //                        caseIncomeEntity.HrRate2 = item.Cells["gvtHrRate2"].Value.ToString();
            //                        caseIncomeEntity.HrRate3 = item.Cells["gvtHrRate3"].Value.ToString();
            //                        caseIncomeEntity.HrRate4 = item.Cells["gvtHrRate4"].Value.ToString();
            //                        caseIncomeEntity.HrRate5 = item.Cells["gvtHrRate5"].Value.ToString();

            //                    }
            //                    caseIncomeEntity.LstcOperator = _baseForm.UserID;
            //                    caseIncomeEntity.AddOperator = _baseForm.UserID;
            //                    caseIncomeEntity.Mode = string.Empty; //item.Cells["Status"].Value.ToString();
            //                                                          //txtSub.Text;                
            //                    if (_model.CaseMstData.InsertCaseIncome(caseIncomeEntity))
            //                    {
            //                        boolSucess = true;
            //                    }
            //                }
            //            }
            //            //  if (boolSucess)
            //            // {

            //            /// New Logic MSTSNP Incomes data update  06/24/2015
            //            //if (dataGridCaseSnp.SelectedRows.Count > 0)
            //            //{
            //            //    if (dataGridCaseIncome.Rows.Count > 0)
            //            //    {
            //            //        CaseSnpEntity casesnprow = dataGridCaseSnp.SelectedRows[0].Tag as CaseSnpEntity;
            //            //        if (casesnprow != null)
            //            //            _model.CaseMstData.UpdateCaseMstSnpIncome(casesnprow);
            //            //    }
            //            //}
            //            /// 

            //            int _strSNPindex = strSnpIndex;


            //            if (propAgencyControlDetails.IncVerfication.ToUpper() == "Y")
            //            {
            //                //if (propAgencyControlDetails.VerSwitch.ToUpper() == "Y")
            //                //{
            //                BaseForm.BaseCaseMstListEntity = _model.CaseMstData.GetCaseMstadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
            //                CaseMSTEntity = BaseForm.BaseCaseMstListEntity[0];
            //                //}
            //                //calVerificationDate.Value = DateTime.Now.Date;
            //                txtFedOmb.Text = "";
            //                txtHud.Text = "";
            //                txtCmi.Text = "";
            //                txtSmi.Text = "";
            //                // calVerificationDate.ValueChanged -= new EventHandler(calVerificationDate_ValueChanged);
            //                if (programDefinitionList != null)
            //                {
            //                    if (programDefinitionList.DepFEDUsed == "Y")
            //                    {
            //                        // txtFedOmb.Text = "0";
            //                        calc_FEDOMB();
            //                    }
            //                    if (programDefinitionList.DepCMIUsed == "Y")
            //                    {
            //                        // txtCmi.Text = "0";
            //                        calc_Poverty("CMI");
            //                    }
            //                    if (programDefinitionList.DepSMIUsed == "Y")
            //                    {
            //                        // txtSmi.Text = "0";
            //                        calc_Poverty("SMI");
            //                    }
            //                    if (programDefinitionList.DepHUDUsed == "Y")
            //                    {
            //                        // txtHud.Text = "0";
            //                        calc_Poverty("HUD");
            //                    }
            //                }


            //                CaseVerEntity caseVerEntity = new CaseVerEntity();
            //                caseVerEntity.Agency = BaseForm.BaseAgency;
            //                caseVerEntity.Dept = BaseForm.BaseDept;
            //                caseVerEntity.Program = BaseForm.BaseProg;
            //                caseVerEntity.Year = BaseForm.BaseYear;
            //                caseVerEntity.AppNo = BaseForm.BaseApplicationNo;
            //                if (propAgencyControlDetails.VerSwitch.ToUpper() == "Y")
            //                {
            //                    caseVerEntity.VerifyDate = LookupDataAccess.Getdate(calVerificationDate.Value.ToString()); //DateTime.Now.ToShortDateString();
            //                }
            //                else
            //                {
            //                    caseVerEntity.VerifyDate = DateTime.Now.ToShortDateString();//
            //                    calVerificationDate.Value = DateTime.Now.Date;
            //                }
            //                caseVerEntity.Classification = "00";

            //                if (cmbVerifier.Items.Count > 0)
            //                    caseVerEntity.Verifier = ((Captain.Common.Utilities.ListItem)cmbVerifier.SelectedItem).Value.ToString();
            //                caseVerEntity.VerOmb = txtFedOmb.Text == string.Empty ? "0" : txtFedOmb.Text;
            //                caseVerEntity.VerHud = txtHud.Text == string.Empty ? "0" : txtHud.Text;
            //                caseVerEntity.VerSmi = txtSmi.Text == string.Empty ? "0" : txtSmi.Text;
            //                caseVerEntity.VerCmi = txtCmi.Text == string.Empty ? "0" : txtCmi.Text;
            //                caseVerEntity.IncomeAmount = CaseMSTEntity.ProgIncome; ;
            //                caseVerEntity.NoInhh = CaseMSTEntity.NoInProg;

            //                caseVerEntity.VerifyOther = "Y";

            //                //Kranthi 07/24/2023:: Added to save the Income Verified details
            //                /*********************************************************/
            //                /*********************************************************/
            //                if (chkw2.Checked == true)
            //                    caseVerEntity.VerifyW2 = "Y";
            //                if (chkCheckStubs.Checked == true)
            //                    caseVerEntity.VerifyCheckStub = "Y";
            //                if (chkLetter.Checked == true)
            //                    caseVerEntity.VerifyLetter = "Y";
            //                if (chkOther.Checked == true)
            //                    caseVerEntity.VerifyOther = "Y";
            //                if (chkTaxReturn.Checked == true)
            //                    caseVerEntity.VerifyTaxReturn = "Y";
            //                if (chk_self_Decl.Checked == true)
            //                    caseVerEntity.VerifySelfDecl = "Y";
            //                /*********************************************************/
            //                /*********************************************************/
            //                /*********************************************************/

            //                caseVerEntity.ReverifyDate = string.Empty;
            //                caseVerEntity.AddOperator = BaseForm.UserID;
            //                caseVerEntity.LstcOperator = BaseForm.UserID;
            //                //caseVerEntity.Mode = "EMSVER";//Consts.Common.New;
            //                if (propAgencyControlDetails.VerSwitch.ToUpper() == "Y")
            //                {
            //                    caseVerEntity.Mode = "EMSVER";
            //                }
            //                else
            //                {
            //                    caseVerEntity.Mode = "DeleteIncome";
            //                    if (_model.CaseMstData.InsertUpdateDelCaseVer(caseVerEntity, out strSqlMsg))
            //                    {
            //                    }
            //                    caseVerEntity.Mode = Consts.Common.New;

            //                }

            //                if (_model.CaseMstData.InsertUpdateDelCaseVer(caseVerEntity, out strSqlMsg))
            //                {
            //                    //if (propAgencyControlDetails.VerSwitch.ToUpper() != "Y")
            //                    //{
            //                    //    
            //                    //}
            //                    BaseForm.BaseCaseMstListEntity = _model.CaseMstData.GetCaseMstadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
            //                    CaseMSTEntity = BaseForm.BaseCaseMstListEntity[0];
            //                    BaseForm.BaseCaseSnpEntity = _model.CaseMstData.GetCaseSnpadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
            //                    BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq)).M_Code = "A";

            //                    fillSnpDetails(false);
            //                    if (dataGridCaseSnp.Rows.Count != 0)
            //                    {
            //                        dataGridCaseSnp.Rows[strSnpIndex].Selected = true;
            //                    }
            //                    dataGridCaseIncome.ReadOnly = true;
            //                }

            //                calVerificationDate.Enabled = false;

            //            }
            //            else
            //            {
            //                BaseForm.BaseCaseMstListEntity = _model.CaseMstData.GetCaseMstadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);

            //                BaseForm.BaseCaseSnpEntity = _model.CaseMstData.GetCaseSnpadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
            //                BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq)).M_Code = "A";
            //                CaseMSTEntity = BaseForm.BaseCaseMstListEntity[0];
            //                fillSnpDetails(false);
            //                if (dataGridCaseSnp.Rows.Count != 0)
            //                {
            //                    dataGridCaseSnp.Rows[strSnpIndex].Selected = true;
            //                }
            //                dataGridCaseIncome.ReadOnly = true;
            //            }




            //            dataGridCaseSnp_SelectionChanged(sender, e);
            //            AlertBox.Show("Income Details Saved Successfully");

            //            if (_strSNPindex != strSnpIndex)
            //            {

            //                dataGridCaseSnp.ClearSelection();
            //                dataGridCaseSnp.Rows[_strSNPindex].Selected = true;
            //            }


            //            //}
            //            //if (intgridloop == 0)
            //            //{
            //            //    CommonFunctions.MessageBoxDisplay("Atleast one Interval selected");
            //            //    btnSave.Enabled = true;
            //            //    btnAdd_Click(sender, e);
            //            //}

            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("We can't delete income records.");
            //    }
            //}
        }

        private void btnNo_Click(object sender, EventArgs e)
        {

        }
    }
}
