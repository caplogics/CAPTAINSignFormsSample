/************************************************************************
 * Conversion On    :   01/02/2023      * Converted By     :   Kranthi
 * Modified On      :   01/02/2023      * Modified By      :   Kranthi
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
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
using Captain.Common.Exceptions;
using NPOI.SS.Formula.Functions;
using DevExpress.ExpressApp.Validation.AllContextsView;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASE0016_UsageEditForm : Form
    {
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        string strCaseWorkerDefaultCode = string.Empty;
        string strCaseWorkerDefaultStartCode = string.Empty;

        public CASE0016_UsageEditForm(BaseForm baseForm, string mode, CASESPMEntity spmRec, CASEACTEntity pass_CA_Entity,CEAPCNTLEntity Ceapcntl, string spmbalance,  PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            Pass_CA_Entity = pass_CA_Entity;
            CEAPControl = Ceapcntl;
            SPM_Entity = spmRec;
            Mode = mode;
            StrBalance = spmbalance;

            //this.Text = privilegeEntity.PrivilegeName + " - " + "Edit Invoice Details" ;
            this.Text = "Edit Invoice Details";

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            propReportPath = _model.lookupDataAccess.GetReportPath();

            Txt_Cost.Validator = TextBoxValidation.FloatValidator;

            propSearch_Entity = _model.SPAdminData.Browse_CASESP0List(null, null, null, null, null, null, null, null, null);
            propfundingsource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "AGYTABS");
            Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, string.Empty);

            fillBillingNameCombo();

            if (pass_CA_Entity!=null)
            {
                Act_Date.ValueChanged -= new EventHandler(Act_Date_ValueChanged);
                Act_Date.Text = pass_CA_Entity.ACT_Date;
                Txt_Cost.Text = pass_CA_Entity.Cost;
                if(!string.IsNullOrEmpty(pass_CA_Entity.PDOUT.Trim()))
                {
                    if (pass_CA_Entity.PDOUT == "Y") chkbPdOut.Checked = true; else chkbPdOut.Checked = false;
                }

                txtAccountNo.Text = pass_CA_Entity.Account;

                if (cmbBilling.Items.Count > 0)
                {
                    if (pass_CA_Entity.BillngType == "T")
                    {
                        CommonFunctions.SetComboBoxValue(cmbBilling, "T");
                        txtFirst.Text = pass_CA_Entity.BillngFname;
                        txtLast.Text = pass_CA_Entity.BillngLname;
                    }
                    else
                    {
                        CommonFunctions.SetComboBoxValue(cmbBilling, pass_CA_Entity.BillngFname.Trim() + pass_CA_Entity.BillngLname.Trim());
                    }
                }

                Act_Date.ValueChanged += new EventHandler(Act_Date_ValueChanged);
            }

            fillFundCombo();

            if (CEAPControl.CPCT_LUMP_SUM == "Y")
            {
                //Act_Date.Enabled = false;
                //Act_Date.ReadOnly = true;
                Txt_Cost.Enabled = false;
                Txt_Cost.ReadOnly = true;
                //Act_Date.ShowCalendar= false;
            }
            else
            {
                Act_Date.ShowCalendar = true;
                Act_Date.Enabled = true;
                Act_Date.ReadOnly = false;
            }

        }

        #region Properties
        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Mode { get; set; }
        public string Source { get; set; }
        public string StrType { get; set; }
        public string StrElec { get; set; }
        public string StrBalance { get; set; }

        public bool IsSaveValid { get; set; }
        public string SP_Code { get; set; }
        public string Sp_Sequence { get; set; }
        public string propReportPath { get; set; }

        public string Spm_Year { get; set; }
        public string ServiceCode { get; set; }

        public CASESPMEntity SPM_Entity { get; set; }
        public CASEACTEntity Pass_CA_Entity { get; set; }
        public CEAPCNTLEntity CEAPControl { get; set; }

        List<CMBDCEntity> Emsbdc_List
        {
            get; set;
        }
        List<CommonEntity> propfundingsource
        {
            get; set;
        }
        List<CASESP0Entity> propSearch_Entity
        {
            get; set;
        }
        #endregion

        private bool ValidateForm()
        {
            bool isValid=true;

            if (Pass_CA_Entity.Service_plan.Trim() == CEAPControl.CPCT_VUL_SP && Pass_CA_Entity.ACT_Code.Trim() == CEAPControl.CPCT_VUL_PRIM_CA.Trim()) 
            {
                if (string.IsNullOrEmpty(Act_Date.Text.Trim()))
                {
                    _errorProvider.SetError(Act_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblActivityDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    if (!CommonFunctions.CheckDateFormat(Act_Date.Value.ToShortDateString(), "MM/dd/yyyy"))
                    {
                        _errorProvider.SetError(Act_Date, "Invalid Date Format");
                        isValid = false;
                    }
                    else
                    {
                        DateTime value;
                        if (!DateTime.TryParse(Act_Date.Value.ToShortDateString(), out value))
                        {
                            _errorProvider.SetError(Act_Date, string.Format("Invalid Date", lblActivityDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                            isValid = false;
                        }
                        else
                        if (Convert.ToDateTime(Act_Date.Value.ToShortDateString()) >= Convert.ToDateTime(CEAPControl.CPCT_PROG_START.Trim()) && Convert.ToDateTime(Act_Date.Value.ToShortDateString()) <= Convert.ToDateTime(CEAPControl.CPCT_PROG_END.Trim()))
                        {
                            _errorProvider.SetError(Act_Date, null);
                        }
                        else
                        {
                            _errorProvider.SetError(Act_Date, "The Service date must be between " + LookupDataAccess.Getdate(CEAPControl.CPCT_PROG_START.Trim()) + " and " + LookupDataAccess.Getdate(CEAPControl.CPCT_PROG_END.Trim()));
                            isValid = false;

                            //_errorProvider.SetError(txtAmount, "Please Enter above 0");
                            //CommonFunctions.MessageBoxDisplay("The Service date must be between " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) + " and " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_END.Trim()));
                            ////booldatevalid = false;
                        }
                    }
                }
                if(string.IsNullOrEmpty(Txt_Cost.Text.Trim()))
                {
                    _errorProvider.SetError(Txt_Cost, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCost.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    decimal TotAmount = decimal.Parse(SPM_Entity.SPM_Amount);
                    decimal SPMBalance = decimal.Parse(StrBalance) + decimal.Parse(Pass_CA_Entity.Cost.Trim());
                    decimal CalBal = SPMBalance - decimal.Parse(Txt_Cost.Text.Trim());
                    decimal balance = 0;
                    if (CalBal>0)
                        balance = TotAmount - CalBal;

                    if (balance<0 || CalBal<0)
                    {
                        _errorProvider.SetError(Txt_Cost, "we can't allow the “Total Dollars Posted” to exceed the maximum award!!");
                        isValid = false;
                    }
                    else
                        _errorProvider.SetError(Txt_Cost, null);
                }

            }
            else if (Pass_CA_Entity.Service_plan.Trim() == CEAPControl.CPCT_NONVUL_SP && Pass_CA_Entity.ACT_Code.Trim() == CEAPControl.CPCT_NONVUL_PRIM_CA.Trim())
            {
                if (string.IsNullOrEmpty(Act_Date.Text.Trim()))
                {
                    _errorProvider.SetError(Act_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblActivityDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    DateTime value;
                    if (!DateTime.TryParse(Act_Date.Value.ToShortDateString(), out value))
                    {
                        _errorProvider.SetError(Act_Date, string.Format("Invalid Date", lblActivityDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    if (Convert.ToDateTime(Act_Date.Value.ToShortDateString()) >= Convert.ToDateTime(CEAPControl.CPCT_PROG_START.Trim()) && Convert.ToDateTime(Act_Date.Value.ToShortDateString()) <= Convert.ToDateTime(CEAPControl.CPCT_PROG_END.Trim()))
                    {
                        _errorProvider.SetError(Act_Date, null);
                    }
                    else
                    {
                        _errorProvider.SetError(Act_Date, "The Service date must be between " + LookupDataAccess.Getdate(CEAPControl.CPCT_PROG_START.Trim()) + " and " + LookupDataAccess.Getdate(CEAPControl.CPCT_PROG_END.Trim()));
                        isValid = false;

                        //_errorProvider.SetError(txtAmount, "Please Enter above 0");
                        //CommonFunctions.MessageBoxDisplay("The Service date must be between " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_START.Trim()) + " and " + LookupDataAccess.Getdate(CEAPCNTL_List[0].CPCT_PROG_END.Trim()));
                        ////booldatevalid = false;
                    }
                }
                if (string.IsNullOrEmpty(Txt_Cost.Text.Trim()))
                {
                    _errorProvider.SetError(Txt_Cost, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCost.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    decimal TotAmount = decimal.Parse(SPM_Entity.SPM_Amount);
                    decimal SPMBalance = decimal.Parse(StrBalance) + decimal.Parse(Pass_CA_Entity.Cost.Trim());
                    decimal balance = TotAmount - (SPMBalance - decimal.Parse(Txt_Cost.Text.Trim()));

                    if (balance < 0)
                    {
                        _errorProvider.SetError(Txt_Cost, "we can't allow the “Total Dollars Posted” to exceed the maximum award!!");
                        isValid = false;
                    }
                    else
                        _errorProvider.SetError(Txt_Cost, null);
                }

            }

            if(((Utilities.ListItem)cmbBudget.SelectedItem).Value.ToString()=="0")
            {
                _errorProvider.SetError(cmbBudget, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblBudget.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }

            if (!string.IsNullOrEmpty(Txt_Cost.Text.Trim()))
            {
                string strcmbBudget = ((Utilities.ListItem)cmbBudget.SelectedItem).Value == null ? string.Empty : ((Utilities.ListItem)cmbBudget.SelectedItem).Value.ToString();
                decimal TotalAmount = decimal.Parse(Txt_Cost.Text.Trim());
                if (!string.IsNullOrEmpty(strcmbBudget))
                {
                    if (Emsbdc_List.Count > 0)
                    {
                        CMBDCEntity Entity = Emsbdc_List.Find(u => u.BDC_ID == (((Utilities.ListItem)cmbBudget.SelectedItem).Value.ToString()));
                        if (Entity != null)
                        {
                            if ((TotalAmount > Convert.ToDecimal(Entity.BDC_BALANCE)))
                            {
                                CommonFunctions.MessageBoxDisplay("Amt Paid may not exceed " + Entity.BDC_BALANCE);
                                Txt_Cost.Text = string.Empty;
                                isValid = false;
                            }
                        }
                    }
                }
            }

            return isValid;
        }

        private void Btn_CACancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        string IsSave = "N";

        private void Btn_CASave_Click(object sender, EventArgs e)
        {
            if(ValidateForm())
            {
                string PrivFund = Pass_CA_Entity.Fund1;
                if (Mode == "Edit")
                {
                    string BudgetId = Pass_CA_Entity.BDC_ID;

                    CASEACTEntity Entity = new CASEACTEntity();

                    Entity = Pass_CA_Entity;

                    Entity.ACT_Date = Act_Date.Text.ToString();//Act_Date.Value.ToShortDateString();
                    Entity.Cost = Txt_Cost.Text.Trim();
                    if (chkbPdOut.Checked)
                        Entity.PDOUT = "Y"; else Entity.PDOUT = "N";

                    Entity.Rec_Type = "CP";

                    Entity.Fund1 = ((ListItem)cmbFundsource.SelectedItem).Value.ToString();
                    Entity.BDC_ID = ((ListItem)cmbBudget.SelectedItem).Value.ToString();

                    Entity.BillngType=((ListItem)cmbBilling.SelectedItem).Value.ToString();
                    Entity.BillngFname = txtFirst.Text.ToString().Trim();
                    Entity.BillngLname = txtLast.Text.ToString().Trim();
                    Entity.Account = txtAccountNo.Text.Trim();

                    Entity.Lsct_Operator = BaseForm.UserID;

                    int New_CAID = 1, New_CA_Seq = 1; string Sql_SP_Result_Message = string.Empty;

                    if (((ListItem)cmbBudget.SelectedItem).ID.ToString() == "N")
                    {
                        MessageBox.Show("Postings are not allowed to this Budget " + ((ListItem)cmbBudget.SelectedItem).Text.ToString() + "\n Do you want to save this? ", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Add_SAVE_CAMS);
                    }
                    else
                        IsSave = "Y";

                    if (IsSave == "Y")
                    {
                        if (_model.SPAdminData.UpdateCASEACT3(Entity, "Update", out New_CAID, out New_CA_Seq, out Sql_SP_Result_Message))
                        {
                            //if (PrivFund != Pass_CA_Entity.Fund1)
                            //{

                            //}

                            Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, Pass_CA_Entity.Fund1);


                            if (Emsbdc_List.Count > 0)
                                Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == Pass_CA_Entity.BDC_ID);

                            CMBDCEntity emsbdcentity = Emsbdc_List.Find(u => (Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(Pass_CA_Entity.ACT_Date.Trim()) && Convert.ToDateTime(u.BDC_END) >= Convert.ToDateTime(Pass_CA_Entity.ACT_Date.Trim())));
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
                                {
                                }
                            }

                            ////Modified by Sudheer on 09/11/24 as per the NCCAA.docx on 'POST INVOICE ADJSUTMENT 9/10/2024'
                            if (PrivFund != Pass_CA_Entity.Fund1)
                                Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, PrivFund);
                            else
                                Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, Pass_CA_Entity.Fund1);

                            if (BudgetId != Pass_CA_Entity.BDC_ID.Trim())
                            {

                                if (Emsbdc_List.Count > 0)
                                    Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == BudgetId);
                                emsbdcentity = Emsbdc_List.Find(u => (Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(Pass_CA_Entity.ACT_Date.Trim()) && Convert.ToDateTime(u.BDC_END) >= Convert.ToDateTime(Pass_CA_Entity.ACT_Date.Trim())));
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
                                    {
                                    }
                                }
                            }




                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }

            }
        }

        private void Add_SAVE_CAMS(DialogResult dialogResult)
        {
            int New_CAID = 1, New_CA_Seq = 1;
            string Sql_SP_Result_Message = string.Empty;
            string BudgetId = Pass_CA_Entity.BDC_ID;
            CASEACTEntity Entity = new CASEACTEntity();

            Entity = Pass_CA_Entity;

            Entity.ACT_Date = Act_Date.Value.ToShortDateString();
            Entity.Cost = Txt_Cost.Text.Trim();
            if (chkbPdOut.Checked)
                Entity.PDOUT = "Y";
            else
                Entity.PDOUT = "N";

            Entity.Rec_Type = "CP";

            Pass_CA_Entity.Fund1 = ((ListItem)cmbFundsource.SelectedItem).Value.ToString();
            Pass_CA_Entity.BDC_ID = ((ListItem)cmbBudget.SelectedItem).Value.ToString();

            Entity.Lsct_Operator = BaseForm.UserID;

            if (dialogResult == DialogResult.Yes)
            {
                if (_model.SPAdminData.UpdateCASEACT3(Entity, "Update", out New_CAID, out New_CA_Seq, out Sql_SP_Result_Message))
                {
                    Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, Pass_CA_Entity.Fund1);


                    if (Emsbdc_List.Count > 0)
                        Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == Pass_CA_Entity.BDC_ID);

                    CMBDCEntity emsbdcentity = Emsbdc_List.Find(u => (Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(Pass_CA_Entity.ACT_Date.Trim()) && Convert.ToDateTime(u.BDC_END) >= Convert.ToDateTime(Pass_CA_Entity.ACT_Date.Trim())));
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
                        {
                        }
                    }

                    Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, Pass_CA_Entity.Fund1);

                    if (BudgetId != Pass_CA_Entity.BDC_ID.Trim())
                    {

                        if (Emsbdc_List.Count > 0)
                            Emsbdc_List = Emsbdc_List.FindAll(u => u.BDC_ID == BudgetId);
                        emsbdcentity = Emsbdc_List.Find(u => (Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(Pass_CA_Entity.ACT_Date.Trim()) && Convert.ToDateTime(u.BDC_END) >= Convert.ToDateTime(Pass_CA_Entity.ACT_Date.Trim())));
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
                            {
                            }
                        }
                    }




                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                IsSave = "N";
            }
        }

        public string[] Get_Selected_Row()
        {
            string[] SelectVendor = new string[5];
            //if (gvwVendor.Rows.Count > 0)
            //{

                SelectVendor[0] = Mode;
                SelectVendor[1] = Act_Date.Value.ToShortDateString();
            SelectVendor[2] = decimal.Parse(Txt_Cost.Text.Trim()).ToString("0.00");
            if (chkbPdOut.Checked)
                SelectVendor[3] = "Y";
            else SelectVendor[3] = "N";

            SelectVendor[4]=((ListItem)cmbBudget.SelectedItem).Value.ToString();

            //}
            return SelectVendor;
        }

        //Added by Vikash on 02/21/2025 as per CCSCT small Chnages document

        private void fillBillingNameCombo()
        {
            cmbBilling.SelectedIndexChanged -= new EventHandler(cmbBilling_SelectedIndexChanged);
            cmbBilling.Items.Add(new Utilities.ListItem("   ", "0"));
            cmbBilling.SelectedIndex = 0;
            int rowIndex = 0;
            foreach (CaseSnpEntity item in BaseForm.BaseCaseSnpEntity)
            {
                rowIndex++;
                if (item.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq)
                    cmbBilling.Items.Add(new Utilities.ListItem(LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, BaseForm.BaseHierarchyCnFormat), item.NameixFi.Trim() + item.NameixLast.Trim(), item.FamilySeq, "A"));
                else
                    cmbBilling.Items.Add(new Utilities.ListItem(LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, BaseForm.BaseHierarchyCnFormat), item.NameixFi.Trim() + item.NameixLast.Trim(), item.FamilySeq, "M"));
            }
            cmbBilling.Items.Add(new Utilities.ListItem("3rd Party Billing", "T", "T", "T"));
            cmbBilling.SelectedIndexChanged += new EventHandler(cmbBilling_SelectedIndexChanged);
        }
            
        private void cmbBilling_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Utilities.ListItem)cmbBilling.SelectedItem).Value.ToString() != "0")
            {
                if (((Utilities.ListItem)cmbBilling.SelectedItem).Value.ToString() == "T")
                {

                    if (Mode == "Add")
                    {
                        txtFirst.Text = string.Empty;
                        txtLast.Text = string.Empty;

                        txtFirst.Enabled = true;
                        txtLast.Enabled = true;
                    }
                    if (Mode == "Edit")
                    {
                        txtFirst.Enabled = true;
                        txtLast.Enabled = true;
                    }
                }
                else
                {
                    CaseSnpEntity casesnp = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == ((Utilities.ListItem)cmbBilling.SelectedItem).ID.ToString());
                    if (casesnp != null)
                    {
                        txtFirst.Enabled = false;
                        txtLast.Enabled = false;
                        txtFirst.Text = casesnp.NameixFi;
                        txtLast.Text = casesnp.NameixLast;
                    }
                }
            }
            else
            {
                txtFirst.Text = string.Empty;
                txtLast.Text = string.Empty;

                txtFirst.Enabled = true;
                txtLast.Enabled = true;
            }
        }

        // Added by Vikash on 12/07/2023 as per WiseJ Enhancement Requests Nov_2023 document 23rd point
        private void fillFundCombo()
        {
            //cmbFundsource.SelectedIndexChanged -= new EventHandler(cmbFundsource_SelectedIndexChanged);
            List<CommonEntity> commonfundingsource = propfundingsource;

            commonfundingsource = filterByHIE(commonfundingsource, Mode);
            cmbFundsource.Items.Clear();
            cmbFundsource.ColorMember = "FavoriteColor";

            CASESP0Entity casesp0data = new CASESP0Entity();

            //if (CmbSP.Items.Count > 0)
            //    casesp0data = propSearch_Entity.Find(u => u.Code == ((Captain.Common.Utilities.ListItem)CmbSP.SelectedItem).Value.ToString() && u.Funds != string.Empty && u.Allow_Add_Branch == "N" && u.B2Code.Trim() == string.Empty);

            casesp0data = propSearch_Entity.Find(u => u.Code == (Pass_CA_Entity.Service_plan.ToString().Trim()) && u.Funds != string.Empty && u.Allow_Add_Branch == "N" && u.B2Code.Trim() == string.Empty);

            bool Istrue = false;int rowIndex = 0;int SelInd = 0;
            foreach (CommonEntity item in commonfundingsource)
            {
                Istrue = false;
                if (casesp0data != null)
                {
                    if (casesp0data.Funds.Contains(item.Code))
                        Istrue = true;
                }
                if (Istrue)
                {
                    cmbFundsource.Items.Add(new Utilities.ListItem(item.Desc.ToString(), item.Code.ToString(), item.Active.ToString(), (item.Active.Equals("Y") ? Color.Black : Color.Red), item.Default.ToString(), string.Empty));
                    if(!string.IsNullOrEmpty(Pass_CA_Entity.Fund1.Trim()))
                    {
                        if (Pass_CA_Entity.Fund1.Trim() == item.Code.Trim())
                            SelInd = rowIndex;
                    }
                    else if (SPM_Entity.SPM_Fund.Trim() == item.Code.Trim())
                        SelInd = rowIndex;

                        rowIndex++;
                }

                

            }
            //cmbFundsource.Items.Insert(0, new Utilities.ListItem("  ", "0"));
            if (cmbFundsource.Items.Count > 0)
                cmbFundsource.SelectedIndex = SelInd;
            //cmbFundsource.SelectedIndexChanged += new EventHandler(cmbFundsource_SelectedIndexChanged);
        }

        private void cmbFundsource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFundsource.Items.Count > 0)
            {
                string strcmbFundsource = ((Utilities.ListItem)cmbFundsource.SelectedItem).Value == null ? string.Empty : ((Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString();
                if (!string.IsNullOrEmpty(strcmbFundsource))
                {
                    if (((Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString() != "0")
                    {
                        if (Emsbdc_List.Count > 0)
                        {
                            List<CMBDCEntity> Entity = Emsbdc_List.FindAll(u => u.BDC_FUND == strcmbFundsource);//&& u.BDC_ALLOW_POSTING == "Y");
                            if (Entity.Count > 0)
                            {
                                Entity = Entity.FindAll(u => Convert.ToDateTime(u.BDC_START.Trim()) <= Convert.ToDateTime(Act_Date.Text.Trim()) && Convert.ToDateTime(u.BDC_END.Trim()) >= Convert.ToDateTime(Act_Date.Text.Trim()));



                                if (Entity.Count > 0)
                                {
                                    fillBudgets(Entity);
                                    if (Entity.Count == 1)
                                    {
                                        cmbBudget.Enabled = false;
                                        CommonFunctions.SetComboBoxValue(cmbBudget, Entity[0].BDC_ID);
                                        txtstart1.Text = LookupDataAccess.Getdate(Entity[0].BDC_START);
                                        txtEnd.Text = LookupDataAccess.Getdate(Entity[0].BDC_END);
                                        txtBudget.Text = Entity[0].BDC_BUDGET;
                                        txtBalance1.Text = Entity[0].BDC_BALANCE;
                                        //if (string.IsNullOrEmpty(txtAmount.Text.Trim()))
                                        //    GetMaxBenfit();
                                    }
                                    else
                                    {
                                        cmbBudget.Enabled = true;
                                    }
                                }
                                else
                                {
                                    
                                    CommonFunctions.MessageBoxDisplay("Budget not set up for this fund given Benefit Date");
                                    txtBalance1.Text = string.Empty;
                                    txtstart1.Text = string.Empty;
                                    txtEnd.Text = string.Empty;
                                    txtBudget.Text = string.Empty;
                                    CommonFunctions.SetComboBoxValue(cmbFundsource, "0");
                                    cmbBudget.Items.Add(new Utilities.ListItem("", "0", "", string.Empty));
                                    cmbBudget.SelectedIndex = 0;
                                }

                            }
                            else
                            {
                                
                                CommonFunctions.MessageBoxDisplay("Budget not set up for this fund...");
                                txtBalance1.Text = string.Empty;
                                txtstart1.Text = string.Empty;
                                txtEnd.Text = string.Empty;
                                txtBudget.Text = string.Empty;
                                CommonFunctions.SetComboBoxValue(cmbFundsource, "0");
                                cmbBudget.Items.Add(new Utilities.ListItem("", "0", "", string.Empty));
                                cmbBudget.SelectedIndex = 0;
                            }
                        }
                        else
                        {
                            
                            CommonFunctions.MessageBoxDisplay("Budget not set up for this fund...");
                            //txtAmount.Text = string.Empty;
                            txtBalance1.Text = string.Empty;
                            txtstart1.Text = string.Empty;
                            txtEnd.Text = string.Empty;
                            txtBudget.Text = string.Empty;
                            CommonFunctions.SetComboBoxValue(cmbFundsource, "0");
                            cmbBudget.Items.Add(new Utilities.ListItem("", "0", "", string.Empty));
                            cmbBudget.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        private void fillBudgets(List<CMBDCEntity> BudgetList)
        {
            //cmbBudget.SelectedIndexChanged -= new EventHandler(cmbBudget_SelectedIndexChanged);
            cmbBudget.Items.Clear();
            int budgetIndex = 1; int budgetSelIndex = 0;
            if (BudgetList.Count > 0)
            {
                if (string.IsNullOrEmpty(Pass_CA_Entity.BDC_ID.Trim()))
                    BudgetList = BudgetList.FindAll(u => u.BDC_ALLOW_POSTING == "Y");

                foreach (CMBDCEntity entity in BudgetList)
                {
                    cmbBudget.Items.Add(new Utilities.ListItem(entity.BDC_DESCRIPTION.ToString(), entity.BDC_ID.ToString(), entity.BDC_ALLOW_POSTING.Trim(), string.Empty));

                    if (Mode == "Add")
                    {
                        if (SPM_Entity.SPM_BDC_ID == entity.BDC_ID)
                            budgetSelIndex = budgetIndex;
                    }
                    else
                    {
                        if (Pass_CA_Entity.BDC_ID == entity.BDC_ID)
                            budgetSelIndex = budgetIndex;
                    }


                    budgetIndex++;
                }

                if (BudgetList.Count > 1)
                {
                    cmbBudget.Items.Insert(0, new Utilities.ListItem("  ", "0"));
                }
                else
                    cmbBudget.Items.Insert(0, new Utilities.ListItem("  ", "0"));

                if (Mode == "Add")
                {
                    if (!string.IsNullOrEmpty(SPM_Entity.SPM_BDC_ID))
                        cmbBudget.SelectedIndex = budgetSelIndex;
                }
                else if (!string.IsNullOrEmpty(Pass_CA_Entity.BDC_ID))
                    cmbBudget.SelectedIndex = budgetSelIndex;
                else
                    cmbBudget.SelectedIndex = 0;

            }
            //cmbBudget.SelectedIndexChanged += new EventHandler(cmbBudget_SelectedIndexChanged);
        }

        private void cmbBudget_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBudget.Items.Count > 0)
            {
                string strcmbBudget = ((Utilities.ListItem)cmbBudget.SelectedItem).Value == null ? string.Empty : ((Utilities.ListItem)cmbBudget.SelectedItem).Value.ToString();
                string strcmbFundsource = ((Utilities.ListItem)cmbFundsource.SelectedItem).Value == null ? string.Empty : ((Utilities.ListItem)cmbFundsource.SelectedItem).Value.ToString();
                if (!string.IsNullOrEmpty(strcmbBudget))
                {
                    if (((Utilities.ListItem)cmbBudget.SelectedItem).Value.ToString() != "0")
                    {
                        if (Emsbdc_List.Count > 0)
                        {
                            CMBDCEntity Entity = Emsbdc_List.Find(u => u.BDC_FUND == strcmbFundsource && u.BDC_ID.Trim() == strcmbBudget.Trim());
                            if (Entity != null)
                            {
                                txtstart1.Text = LookupDataAccess.Getdate(Entity.BDC_START);
                                txtEnd.Text = LookupDataAccess.Getdate(Entity.BDC_END);
                                txtBudget.Text = Entity.BDC_BUDGET;
                                txtBalance1.Text = Entity.BDC_BALANCE;
                            }
                        }
                    }
                    else
                    {
                        txtstart1.Text = string.Empty;
                        txtEnd.Text = string.Empty;
                        txtBudget.Text = string.Empty;
                        txtBalance1.Text = string.Empty;
                    }
                }
            }
        }

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

        private void Act_Date_ValueChanged(object sender, EventArgs e)
        {
            if(Act_Date.Checked)
            {

                if (!CommonFunctions.CheckDateFormat(Act_Date.Value.ToShortDateString(), "MM/dd/yyyy"))
                {

                }
                else
                    fillFundCombo();

                //DateTime value;
                //if (DateTime.TryParse(Act_Date.Value.ToShortDateString(), out value))
                //{
                //    _errorProvider.SetError(Act_Date, string.Format("Invalid Date", lblActivityDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                //    //isValid = false;
                //}
                //else
                //    fillFundCombo();
            }
        }
    }
}