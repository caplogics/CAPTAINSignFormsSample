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
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using Captain.Common.Views.UserControls;
using Captain.DatabaseLayer;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASE0026Form : Form
    {
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        public CASE0026Form(BaseForm baseForm, string mode, string strCode, PrivilegeEntity privilegeEntity, CMBDCEntity emsbdcEntity)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            Mode = mode;
            this.Text = privilegeEntity.PrivilegeName;
            propEmsbdcEntity = emsbdcEntity;
            this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.Add;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            txtBudget.Validator = TextBoxValidation.CustomDecimalValidation13dot2;
            txtBDetailtype.Validator = TextBoxValidation.IntegerValidator;
            //txtReleasedAmt.Validator = TextBoxValidation.FloatValidator;
            //txtTotcommit.Validator = TextBoxValidation.FloatValidator;
            //txtExpended.Validator = TextBoxValidation.FloatValidator;
            propfundingSource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "AGYTABS");
            FillCombo();
            propResourceexist = false;
            btnCalculateBal.Visible = false;
            BudgetID = "1";

            if (Mode.Equals("Edit"))
            {
                this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.Edit;
                btnCalculateBal.Visible = true;
                FillBudgetDetails(propEmsbdcEntity);
                //GetCMBDCCalAmount();
                List<CMBDCEntity> emsbdcEntitylatest = _model.SPAdminData.GetCMBdcFundCheck(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, ((ListItem)cmbFundsource.SelectedItem).Value.ToString(), BudgetID, dtStardt.Value.ToShortDateString(), dtEndDt.Value.ToShortDateString(), string.Empty, string.Empty, "RECORDEXIST");
                if (emsbdcEntitylatest.Count > 0)
                {
                    cmbFundsource.Enabled = false;
                    propResourceexist = true;
                }
               
            }
            else if (Mode.Equals("View"))
            {
                this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.View;
                pnlDetails.Enabled = false;
                btnCancel.Text = "Close";
                btnSave.Visible = false;
                FillBudgetDetails(propEmsbdcEntity);
            }

        }

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Mode { get; set; }

        public string BudgetID { get; set; }

        public bool IsSaveValid { get; set; }

        public CMBDCEntity propEmsbdcEntity { get; set; }

        public List<STAFFPostEntity> propstaffPostEntity { get; set; }

        public List<CommonEntity> propfundingSource { get; set; }

        public bool propResourceexist { get; set; }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "EMS00010");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (Mode.Equals("Edit"))
            {
                //_model.EMSBDCData.InsertEMSLOCKDATA(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, propEmsbdcEntity.BDC_FUND, string.Empty, string.Empty, string.Empty, propEmsbdcEntity.BDC_COST_CENTER, propEmsbdcEntity.BDC_GL_ACCOUNT, propEmsbdcEntity.BDC_BUDGET_YEAR, propEmsbdcEntity.BDC_INT_ORDER, propEmsbdcEntity.BDC_ACCOUNT_TYPE, propEmsbdcEntity.BDC_START, propEmsbdcEntity.BDC_END, "LOCKOFF", "EMSBDC", BaseForm.UserID, Privileges.Program);
            }
            this.Close();
        }

        private void FillCombo()
        {

            //List<CommonEntity> commonAccountType = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0035", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            //foreach (CommonEntity item in commonAccountType)
            //{
            //    ListItem li = new ListItem(item.Desc, item.Code);
            //    cmbProgram.Items.Add(li);
            //}
            

            //List<CommonEntity> commonfundingsource = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00501", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);

            //foreach (CommonEntity item in commonfundingsource)
            //{
            //    ListItem li = new ListItem(item.Desc, item.Code);
            //    cmbFundsource.Items.Add(li);
            //}
            

           

            cmbFundsource.SelectedIndexChanged -= new EventHandler(cmbFundsource_SelectedIndexChanged);
            List<CommonEntity> commonfundingsource = propfundingSource;

            //List<CommonEntity> _AgytabsFilter = new List<CommonEntity>();
            //_AgytabsFilter = propfundingSource;
            //if (_AgytabsFilter.Count > 0)
            //{
            //    _AgytabsFilter = _AgytabsFilter.FindAll(u => u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.ListHierarchy.Contains(BaseForm.BaseAgency + "****") || u.ListHierarchy.Contains("******")).ToList();
            //    _AgytabsFilter = _AgytabsFilter.OrderByDescending(u => u.Active).ThenBy(u => u.Desc).ToList();
            //}


            commonfundingsource = filterByHIE(commonfundingsource, Mode);
            cmbFundsource.Items.Clear();
            cmbFundsource.ColorMember = "FavoriteColor";

            foreach (CommonEntity item in commonfundingsource)
            {
                cmbFundsource.Items.Add(new ListItem(item.Desc.ToString(), item.Code.ToString(), item.Active.ToString(), (item.Active.Equals("Y") ? Color.Black : Color.Red), item.Default.ToString(),string.Empty));
            }
            cmbFundsource.Items.Insert(0, new ListItem("  ", "0"));
            cmbFundsource.SelectedIndex = 0;
            cmbFundsource.SelectedIndexChanged += new EventHandler(cmbFundsource_SelectedIndexChanged);

            

        }

        private void FillBudgetDetails(CMBDCEntity emsbdcentity)
        {
            if (emsbdcentity != null)
            {

                //cmbProgram.SelectedIndexChanged -= new EventHandler(cmbAccountType_SelectedIndexChanged);
                cmbFundsource.SelectedIndexChanged -= new EventHandler(cmbFundsource_SelectedIndexChanged);
                
                //CommonFunctions.SetComboBoxValue(cmbProgram, emsbdcentity.BDC_ACCOUNT_TYPE);
                
                CommonFunctions.SetComboBoxValue(cmbFundsource, emsbdcentity.BDC_FUND);
                cmbFundsource.Enabled = false;
                if (emsbdcentity.BDC_START != string.Empty)
                {
                    dtStardt.Value = Convert.ToDateTime(emsbdcentity.BDC_START);
                    dtStardt.Checked = true;
                }

                if (emsbdcentity.BDC_END != string.Empty)
                {
                    dtEndDt.Value = Convert.ToDateTime(emsbdcentity.BDC_END);
                    dtEndDt.Checked = true;
                }

                BudgetID = emsbdcentity.BDC_ID;

                txtDescription.Text = emsbdcentity.BDC_DESCRIPTION;
                txtBudget.Text = emsbdcentity.BDC_BUDGET;
                txtRemaining.Text = emsbdcentity.BDC_BALANCE;
                txtBDetailtype.Text = emsbdcentity.BDC_BENEFIT_TYPE;
                txtContractNumber.Text = emsbdcentity.BDC_CONTR_NUM ;

                txtFundCd.Text = emsbdcentity.BDC_FUND_CODE;

                chkAllowPost.Checked = emsbdcentity.BDC_ALLOW_POSTING == "Y" ? true : false;
                

                txtTotcommit.Text = emsbdcentity.BDC_TOT_COMMIT;
                txtExpended.Text = emsbdcentity.BDC_TOT_EXPEND;
                cmbProgram.SelectedIndexChanged += new EventHandler(cmbAccountType_SelectedIndexChanged);
                cmbFundsource.SelectedIndexChanged += new EventHandler(cmbFundsource_SelectedIndexChanged);
                cmbAccountType_SelectedIndexChanged(cmbProgram, new EventArgs());

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


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (isValidate())
            {
                //if (isBudgetCodeExists())
                //{
                //    _errorProvider.SetError(cmbFundsource, "Funding Source already exists..can't add");
                //}
                //else
                {
                    if (isBudgetFundExists())
                    {
                        btnSave.Enabled = false;

                        CMBDCEntity emsbdcentity = new CMBDCEntity();
                        emsbdcentity.BDC_AGENCY = BaseForm.BaseAgency;
                        emsbdcentity.BDC_DEPT = BaseForm.BaseDept;
                        emsbdcentity.BDC_PROGRAM = BaseForm.BaseProg;
                        if(Mode=="Edit")
                            emsbdcentity.BDC_YEAR = propEmsbdcEntity.BDC_YEAR;
                        else
                            emsbdcentity.BDC_YEAR = BaseForm.BaseYear;


                        emsbdcentity.BDC_DESCRIPTION = txtDescription.Text.Trim();
                        emsbdcentity.BDC_FUND = ((ListItem)cmbFundsource.SelectedItem).Value.ToString();
                        emsbdcentity.BDC_START = dtStardt.Value.ToShortDateString();
                        emsbdcentity.BDC_END = dtEndDt.Value.ToShortDateString();
                        emsbdcentity.BDC_BUDGET = txtBudget.Text;
                        emsbdcentity.BDC_ID=BudgetID;

                        emsbdcentity.BDC_FUND_CODE = txtFundCd.Text.Trim();

                        if (Mode.Equals("Add"))
                        {
                            emsbdcentity.BDC_BALANCE = txtBudget.Text;
                            
                        }
                        else
                            emsbdcentity.BDC_BALANCE = txtRemaining.Text;
                        
                        emsbdcentity.BDC_ALLOW_POSTING = chkAllowPost.Checked == true ? "Y" : "N";

                        emsbdcentity.BDC_BENEFIT_TYPE = txtBDetailtype.Text;
                        emsbdcentity.BDC_CONTR_NUM = txtContractNumber.Text;
                        emsbdcentity.BDC_TOT_COMMIT = txtTotcommit.Text;
                        emsbdcentity.BDC_TOT_EXPEND = txtExpended.Text;
                        emsbdcentity.BDC_ADD_OPERATOR = BaseForm.UserID;
                        emsbdcentity.BDC_LSTC_OPERATOR = BaseForm.UserID;
                        emsbdcentity.BDC_DATE_LSTC = propEmsbdcEntity.BDC_DATE_LSTC;                       
                        emsbdcentity.Mode = Mode;

                        string strstatus = string.Empty;
                        bool boolvalid = true;
                        //if (Mode.Equals("Edit"))
                        //{
                        //    //boolvalid = false;
                        //    //string strSucess = EMSBDCDB.GETEMSLOCKVALIDDATA(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,  propEmsbdcEntity.BDC_COST_CENTER, propEmsbdcEntity.BDC_GL_ACCOUNT, propEmsbdcEntity.BDC_BUDGET_YEAR, propEmsbdcEntity.BDC_INT_ORDER, propEmsbdcEntity.BDC_ACCOUNT_TYPE, string.Empty, string.Empty, "EMSBDC");
                        //    //if (strSucess.ToUpper() == "SUCCESS")
                        //    //{
                        //    //    boolvalid = true;
                        //    //}
                        //    //else
                        //    //{
                        //    //    if (strSucess.Contains("Record being"))
                        //    //    {
                        //    //        boolvalid = false;
                        //    //        CommonFunctions.MessageBoxDisplay(strSucess);
                        //    //        EMS00010Control ems00010Control = BaseForm.GetBaseUserControl() as EMS00010Control;
                        //    //        if (ems00010Control != null)
                        //    //        {
                        //    //            //if (Mode.Equals("Edit"))
                        //    //            //{
                        //    //            //    ems00010Control.RefreshGrid();
                        //    //            //}
                        //    //            //else
                        //    //            //{
                        //    //            ems00010Control.RefreshGrid(txtCostCenter.Text.Trim() + txtGlAccount.Text.Trim() + txtYear.Text.Trim());
                        //    //            //}
                        //    //        }
                        //    //    }
                        //    //}
                        //}
                        if (boolvalid)
                        {
                            if (_model.SPAdminData.InsertUpdateDelCMBDC(emsbdcentity, out strstatus))
                            {
                                if (Mode.Equals("Edit"))
                                {
                                    //emsbdcentity.Mode = "BdcAmount";
                                    //if (_model.EMSBDCData.InsertUpdateDelEmsbdc(emsbdcentity, out strstatus))
                                    //{ }

                                    //_model.EMSBDCData.InsertEMSLOCKDATA(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, emsbdcentity.BDC_FUND, string.Empty, string.Empty, string.Empty, emsbdcentity.BDC_COST_CENTER, emsbdcentity.BDC_GL_ACCOUNT, emsbdcentity.BDC_BUDGET_YEAR, emsbdcentity.BDC_INT_ORDER, emsbdcentity.BDC_ACCOUNT_TYPE, emsbdcentity.BDC_START, emsbdcentity.BDC_END, "LOCKOFF", "EMSBDC", BaseForm.UserID, Privileges.Program);
                                    AlertBox.Show("Updated Successfully");
                                }
                                else if(Mode.Equals("Add"))
                                {
                                    AlertBox.Show("Saved Successfully");
                                }

                                CASE0026Control ems00010Control = BaseForm.GetBaseUserControl() as CASE0026Control;
                                if (ems00010Control != null)
                                {
                                    ems00010Control.RefreshGrid(strstatus);

                                }                               
                                this.Close();
                            }
                        }
                    }

                }
            }
        }

        private bool isValidate()
        {
            bool isValid = true;

                       

            //if (((ListItem)cmbProgram.SelectedItem).Value.ToString() == "0")
            //{
            //    _errorProvider.SetError(cmbProgram, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAccountType.Text));
            //    isValid = false;
            //}
            //else
            //{
            //    _errorProvider.SetError(cmbProgram, null);
            //}

            if (((ListItem)cmbFundsource.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbFundsource, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFundSource.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbFundsource, null);
            }

            if (string.IsNullOrEmpty(txtBDetailtype.Text.Trim()))
            {
                _errorProvider.SetError(txtBDetailtype, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblBDetailtype.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtBDetailtype, null);

            if (string.IsNullOrEmpty(txtContractNumber.Text.Trim()))
            {
                _errorProvider.SetError(txtContractNumber, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblContractNumber.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtContractNumber, null);



            if (dtStardt.Checked == false)
            {
                _errorProvider.SetError(dtStardt, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblStartDt.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtStardt, null);
            }

            if (dtEndDt.Checked == false)
            {
                _errorProvider.SetError(dtEndDt, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEndDt.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtEndDt, null);
            }
            if ((dtStardt.Checked == true) && (dtEndDt.Checked == true))
            {
                if (string.IsNullOrWhiteSpace(dtEndDt.Text))
                {
                    _errorProvider.SetError(dtEndDt, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEndDt.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtEndDt, null);
                }
                if (string.IsNullOrWhiteSpace(dtStardt.Text))
                {
                    _errorProvider.SetError(dtStardt, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblStartDt.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtStardt, null);
                }
            }
            if ((dtStardt.Checked == true) && (dtEndDt.Checked == true))
            {

                if (!string.IsNullOrEmpty(dtStardt.Text) && (!string.IsNullOrEmpty(dtEndDt.Text)))
                {
                    if (Convert.ToDateTime(dtStardt.Text) > Convert.ToDateTime(dtEndDt.Text))
                    {
                        _errorProvider.SetError(dtStardt, "Start Date cannot be greater than End Date");//"Start Date Greater than End date");
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtStardt, null);
                    }
                }
            }


            //if (Mode.Equals("Edit"))
            //{
            //    _errorProvider.SetError(txtBudget, null);
            //    decimal decbalnce = Convert.ToDecimal(txtReleasedAmt.Text == string.Empty ? "0.00" : txtReleasedAmt.Text);
            //    if (Convert.ToDecimal(decbalnce) < 0)
            //    {
            //        decimal decbudget = Convert.ToDecimal(txtBudget.Text == string.Empty ? "0.00" : txtBudget.Text);
            //        decimal dectotinv = Convert.ToDecimal(txtTotcommit.Text == string.Empty ? "0.00" : txtTotcommit.Text) + Convert.ToDecimal(txtExpended.Text == string.Empty ? "0.00" : txtExpended.Text);
            //        if (dectotinv > decbudget)
            //        {
            //            _errorProvider.SetError(txtBudget, "Budget value more than " + dectotinv + " value");
            //            isValid = false;
            //        }
            //    }
            //}


            return isValid;
        }

        private bool isBudgetCodeExists()
        {
            bool isExists = false;
            if (Mode.Equals("Add"))
            {
                CMBDCEntity emsbdcentity = new CMBDCEntity();
                emsbdcentity.BDC_AGENCY = BaseForm.BaseAgency;
                emsbdcentity.BDC_DEPT = BaseForm.BaseDept;
                emsbdcentity.BDC_PROGRAM = BaseForm.BaseProg;
                emsbdcentity.BDC_YEAR = BaseForm.BaseYear;
                emsbdcentity.BDC_FUND = ((ListItem)cmbFundsource.SelectedItem).Value.ToString();
                List<CMBDCEntity> EmsBdclist = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, ((ListItem)cmbFundsource.SelectedItem).Value.ToString());
                if (EmsBdclist.Count > 0)
                {
                    //if (txtYear.Text.Trim() == string.Empty || txtIntOrder.Text.Trim() == string.Empty)
                    //{
                    //    if (txtYear.Text.Trim() == string.Empty)
                    //    {
                    //        EMSBDCEntity emsdbcdata = EmsBdclist.Find(u => u.BDC_BUDGET_YEAR == string.Empty);
                    //        if (emsdbcdata != null)
                    //            isExists = true;
                    //    }
                    //    else
                    //    {
                    //        EMSBDCEntity emsdbcdata = EmsBdclist.Find(u => u.BDC_INT_ORDER == string.Empty);
                    //        if (emsdbcdata != null)
                    //            isExists = true;
                    //    }
                    //}
                    //else
                        isExists = true;
                }

            }
            return isExists;
        }

        private bool isBudgetFundExists()
        {
            bool isExists = true;
            if (Mode.Equals("Add"))
            {

                List<CMBDCEntity> emsbdcEntitylatest = _model.SPAdminData.GetCMBdcFundCheck(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, ((ListItem)cmbFundsource.SelectedItem).Value.ToString(),string.Empty, dtStardt.Value.ToShortDateString(), dtEndDt.Value.ToShortDateString(), string.Empty, string.Empty, string.Empty);
                if (emsbdcEntitylatest.Count > 0)
                {
                    //CommonFunctions.MessageBoxDisplay(" Overlapping Start Date " + LookupDataAccess.Getdate(emsbdcEntitylatest[0].BDC_START) + " End " + LookupDataAccess.Getdate(emsbdcEntitylatest[0].BDC_END) + " for \n Fund " + emsbdcEntitylatest[0].BDC_FUND);
                    //isExists = false;
                }
            }
            else
            {
                if (propResourceexist)
                {
                    List<CMBDCEntity> emsbdcEntitylatest = _model.SPAdminData.GetCMBdcFundCheck(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, ((ListItem)cmbFundsource.SelectedItem).Value.ToString(), BudgetID, dtStardt.Value.ToShortDateString(), dtEndDt.Value.ToShortDateString(), string.Empty, string.Empty, string.Empty);
                    if (emsbdcEntitylatest.Count > 0)
                    {
                        if (emsbdcEntitylatest.Count == 1)
                        {
                            if ((LookupDataAccess.Getdate(propEmsbdcEntity.BDC_START.ToString()) != LookupDataAccess.Getdate(dtStardt.Value.ToShortDateString())) || (LookupDataAccess.Getdate(propEmsbdcEntity.BDC_END.ToString()) != LookupDataAccess.Getdate(dtEndDt.Value.ToShortDateString())))
                            {
                                List<CMBDCEntity> emsbdcEntityValidation = _model.SPAdminData.GetCMBdcFundCheck(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, ((ListItem)cmbFundsource.SelectedItem).Value.ToString(), BudgetID, propEmsbdcEntity.BDC_START.ToString(), propEmsbdcEntity.BDC_END.ToString(), dtStardt.Value.ToShortDateString(), dtEndDt.Value.ToShortDateString(), "RECORDVALIDATION");
                                if (emsbdcEntityValidation.Count > 0)
                                {
                                    isExists = false;
                                    AlertBox.Show("Resource Records Exists.", MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            CMBDCEntity emsoverlapdata = emsbdcEntitylatest.Find(u => u.BDC_START != propEmsbdcEntity.BDC_START.ToString() && u.BDC_END != propEmsbdcEntity.BDC_END.ToString());
                            if (emsoverlapdata != null)
                                AlertBox.Show("Overlapping Start Date " + LookupDataAccess.Getdate(emsoverlapdata.BDC_START) + " End " + LookupDataAccess.Getdate(emsoverlapdata.BDC_END) + " for \n Fund " + emsoverlapdata.BDC_FUND, MessageBoxIcon.Error);
                            else
                                AlertBox.Show("Overlapping Start Date " + LookupDataAccess.Getdate(emsbdcEntitylatest[0].BDC_START) + " End " + LookupDataAccess.Getdate(emsbdcEntitylatest[0].BDC_END) + " for \n Fund " + emsbdcEntitylatest[0].BDC_FUND, MessageBoxIcon.Error);
                            isExists = false;
                        }
                    }
                }
                else
                {
                    List<CMBDCEntity> emsbdcEntitylatest = _model.SPAdminData.GetCMBdcFundCheck(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, ((ListItem)cmbFundsource.SelectedItem).Value.ToString(), BudgetID, dtStardt.Value.ToShortDateString(), dtEndDt.Value.ToShortDateString(), string.Empty, string.Empty, string.Empty);
                    if (emsbdcEntitylatest.Count > 1)
                    {
                        CMBDCEntity emsoverlapdata = emsbdcEntitylatest.Find(u => u.BDC_START != propEmsbdcEntity.BDC_START.ToString() && u.BDC_END != propEmsbdcEntity.BDC_END.ToString());
                        if (emsoverlapdata != null)
                           AlertBox.Show("Overlapping Start Date " + LookupDataAccess.Getdate(emsoverlapdata.BDC_START) + " End " + LookupDataAccess.Getdate(emsoverlapdata.BDC_END) + " for \n Fund " + emsoverlapdata.BDC_FUND, MessageBoxIcon.Error);
                        else
                            AlertBox.Show("Overlapping Start Date " + LookupDataAccess.Getdate(emsbdcEntitylatest[0].BDC_START) + " End " + LookupDataAccess.Getdate(emsbdcEntitylatest[0].BDC_END) + " for \n Fund " + emsbdcEntitylatest[0].BDC_FUND, MessageBoxIcon.Error);
                        isExists = false;

                    }
                }
            }
            return isExists;
        }

        private void btnBudgetAdj_Click(object sender, EventArgs e)
        {
            //EMS00010Adjustment EMS00010Adjustment = new EMS00010Adjustment(BaseForm, Privileges, propEmsbdcEntity);
            //EMS00010Adjustment.ShowDialog();
        }

       
        private void txtBudget_Leave(object sender, EventArgs e)
        {
            if (txtBudget.Text.Length > 11)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtBudget.Text, Consts.StaticVars.TwoDecimalRange11String))
                {

                    AlertBox.Show(Consts.Messages.PleaseEnterDecimal11Range, MessageBoxIcon.Error);
                    txtBudget.Text = string.Empty;
                }
            }
            if (txtBudget.Text.Trim() != string.Empty)
            {
                if (Mode == "Edit")
                {
                    decimal decBalance = 0;
                    List<CMBDCEntity> emsbdcEntitylatest = _model.SPAdminData.GetCMBdcFundCheck(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, ((ListItem)cmbFundsource.SelectedItem).Value.ToString(), BudgetID, dtStardt.Value.ToShortDateString(), dtEndDt.Value.ToShortDateString(), string.Empty, string.Empty, "TotBalance");
                    if (emsbdcEntitylatest.Count > 0)
                    {
                        if (emsbdcEntitylatest[0].BDC_BALANCE != string.Empty)
                            decBalance = Convert.ToDecimal(emsbdcEntitylatest[0].BDC_BALANCE);
                    }
                    txtRemaining.Text = (Convert.ToDecimal(txtBudget.Text) - decBalance).ToString();

                }                
            }
        }

        private void cmbAccountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProgram.Items.Count > 0)
            {
              
            }
        }

        private void cmbFundsource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbFundsource.SelectedItem).Value.ToString() != "0")
            {
                if (cmbFundsource.Items.Count > 0)
                {
                    if (((ListItem)cmbFundsource.SelectedItem).ID.ToString() != "Y")
                        AlertBox.Show("Inactive Funding Source is selected.", MessageBoxIcon.Error);
                }
            }
        }

        private void btnCalculateBal_Click(object sender, EventArgs e)
        {
            GetCMBDCCalAmount();
        }


        private void GetCMBDCCalAmount()
        {
            List<CMBDCEntity> emsbdcEntitylatest = _model.SPAdminData.GetCMBDCCalAmount(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, ((ListItem)cmbFundsource.SelectedItem).Value.ToString(),BudgetID, dtStardt.Value.ToShortDateString(), dtEndDt.Value.ToShortDateString(), "CalculateBalance",  txtBudget.Text);//_model.EMSBDCData.GetEMSBDCCalAmount(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, ((ListItem)cmbFundsource.SelectedItem).Value.ToString(), dtStardt.Value.ToShortDateString(), dtEndDt.Value.ToShortDateString(), "CalculateBalance");
            if (emsbdcEntitylatest.Count > 0)
            {
                txtTotcommit.Text = emsbdcEntitylatest[0].BDC_TOT_COMMIT;
                txtExpended.Text = emsbdcEntitylatest[0].BDC_TOT_EXPEND;
                txtRemaining.Text = emsbdcEntitylatest[0].BDC_BALANCE;
                //if (Convert.ToDecimal(emsbdcEntitylatest[0].BDC_BALANCE) < 0)
                //{
                //    txtBudget.Text = (Convert.ToDecimal(emsbdcEntitylatest[0].BDC_TOT_COMMIT) + Convert.ToDecimal(emsbdcEntitylatest[0].BDC_TOT_INV)).ToString();
                //    txtBalance.Text = "0.00";
                //}
            }
        }

        private void ADMN0023Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Mode.Equals("Edit"))
            {
                //_model.EMSBDCData.InsertEMSLOCKDATA(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, propEmsbdcEntity.BDC_FUND, string.Empty, string.Empty, string.Empty, propEmsbdcEntity.BDC_COST_CENTER, propEmsbdcEntity.BDC_GL_ACCOUNT, propEmsbdcEntity.BDC_BUDGET_YEAR, propEmsbdcEntity.BDC_INT_ORDER, propEmsbdcEntity.BDC_ACCOUNT_TYPE, propEmsbdcEntity.BDC_START, propEmsbdcEntity.BDC_END, "LOCKOFF", "EMSBDC", BaseForm.UserID, Privileges.Program);
            }
        }

        private void CASE0026Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "tlHelp")
            {

            }
        }
    }
}