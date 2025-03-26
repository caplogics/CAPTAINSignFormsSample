using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Controls.Compatibility;
using Captain.Common.Views.Forms.Base;
using DevExpress.Diagram.Core.Native;
using DevExpress.ExpressApp.SystemModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class HUD99001_RepPeriod_Budget : Form
    {
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        public HUD99001_RepPeriod_Budget(BaseForm baseForm, PrivilegeEntity privilegeEntity, string mode, DataSet dsHUD)
        {
            InitializeComponent();

            _baseForm = baseForm;
            _privilegeEntity = privilegeEntity;

            _model = new CaptainModel();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            _mode = mode;
            HUD_Data= dsHUD;

            txtFiscalYear.Focus();
            txtFiscalYear.TabStop = true;

            txtTotBudget.Validator = TextBoxValidation.CustomDecimalValidation15dot2;
            txtTotFundBudget.Validator = TextBoxValidation.CustomDecimalValidation15dot2;

            if (HUD_Data.Tables[0].Rows[0]["HUDC_RPB_FISCAL_YEAR"].ToString() != "")
            {
                txtFiscalYear.Text = HUD_Data.Tables[0].Rows[0]["HUDC_RPB_FISCAL_YEAR"].ToString();

                DateinBtw();

                fillControls(HUD_Data.Tables[0]);
            }
            else
            {
                txtFiscalYear.Text = DateTime.Now.Year.ToString();/*((int)Math.Ceiling(DateTime.Now.Month / 3.0 + 2) % 4 + 1).ToString();*/
                fillQuarterCombo();

                DateinBtw();
            }
        }

        private void fillControls(DataTable dt)
        {
            if (dt.Rows[0]["HUDC_RPB_FISCAL_YEAR"].ToString() != "")
            {
                //this.txtFiscalYear.Leave -= new System.EventHandler(this.txtFiscalYear_Leave);

                fillQuarterCombo();

                //this.cmbQuarter.SelectedIndexChanged -= new System.EventHandler(this.cmbQuarter_SelectedIndexChanged);

                //txtFiscalYear.Text = dt.Rows[0]["HUDC_RPB_FISCAL_YEAR"].ToString();

                string quart = string.Empty;

                if (quarter.Count > 0)
                    quart = quarter.Find(u => u.Code == dt.Rows[0]["HUDC_RPB_QUARTER"].ToString()).Desc.ToString();

                CommonFunctions.SetComboBoxValue(cmbQuarter, quart);


                dtpDteFrom.Text = dt.Rows[0]["HUDC_RPB_FROM_DATE"].ToString();
                dtpDteTo.Text = dt.Rows[0]["HUDC_RPB_TO_DATE"].ToString();
                dtpSubmDte.Text = dt.Rows[0]["HUDC_RPB_SUBM_DATE"].ToString();
                dtpUpdateDte.Text = dt.Rows[0]["HUDC_RPB_UPDATE_DATE"].ToString();

                txtTotBudget.Text = dt.Rows[0]["HUDC_RPB_TOT_BUDGET"].ToString();
                txtTotFundBudget.Text = dt.Rows[0]["HUDC_RPB_TOT_FUND_BUDGET"].ToString();

                //this.txtFiscalYear.Leave += new System.EventHandler(this.txtFiscalYear_Leave);
                //this.cmbQuarter.SelectedIndexChanged += new System.EventHandler(this.cmbQuarter_SelectedIndexChanged);
            }

           
        }

        private void DateinBtw()
        {
            if (DateTime.Now.Date >= Convert.ToDateTime("01/01/" + txtFiscalYear.Text) && DateTime.Now.Date <= Convert.ToDateTime("03/31/" + txtFiscalYear.Text))
            {
                CommonFunctions.SetComboBoxValue(cmbQuarter, "1");
            }
            else if (DateTime.Now.Date >= Convert.ToDateTime("04/01/" + txtFiscalYear.Text) && DateTime.Now.Date <= Convert.ToDateTime("06/30/" + txtFiscalYear.Text))
            {
                CommonFunctions.SetComboBoxValue(cmbQuarter, "2");
            }
            else if (DateTime.Now.Date >= Convert.ToDateTime("07/01/" + txtFiscalYear.Text) && DateTime.Now.Date <= Convert.ToDateTime("09/30/" + txtFiscalYear.Text))
            {
                CommonFunctions.SetComboBoxValue(cmbQuarter, "3");
            }
            else if (DateTime.Now.Date >= Convert.ToDateTime("10/01/" + txtFiscalYear.Text) && DateTime.Now.Date <= Convert.ToDateTime("12/31/" + txtFiscalYear.Text))
            {
                CommonFunctions.SetComboBoxValue(cmbQuarter, "4");
            }
        }

        #region Properties

        public BaseForm _baseForm
        {
            get;
            set;
        }

        public PrivilegeEntity _privilegeEntity
        {
            get;
            set;
        }

        public string _mode
        {
            get;
            set;
        }
        public HUDCNTLEntity hudEntity
        {
            get;
            set;
        }

        public DataSet HUD_Data
        {
            get;
            set;
        }

        #endregion


        private void txtFiscalYear_Leave(object sender, EventArgs e)
        {
           divideYearintoQuarters(txtFiscalYear.Text.Trim());
        }

        List<CommonEntity> quarter = new List<CommonEntity>();
        private void fillQuarterCombo()
        {
            cmbQuarter.Items.Clear();

            cmbQuarter.Items.Add(new ListItem("Q1", 1));
            cmbQuarter.Items.Add(new ListItem("Q2", 2));
            cmbQuarter.Items.Add(new ListItem("Q3", 3));
            cmbQuarter.Items.Add(new ListItem("Q4", 4));

            if (cmbQuarter.Items.Count > 0)
                cmbQuarter.SelectedIndex = 0;

            quarter.Add(new CommonEntity { Code = "1", Desc = "Q1" });
            quarter.Add(new CommonEntity { Code = "2", Desc = "Q2" });
            quarter.Add(new CommonEntity { Code = "3", Desc = "Q3" });
            quarter.Add(new CommonEntity { Code = "4", Desc = "Q4" });
        }

        string isQ1 = string.Empty;
        string isQ2 = string.Empty;
        string isQ3 = string.Empty;
        string isQ4 = string.Empty;

        string isQ1T = string.Empty;
        string isQ2T = string.Empty;
        string isQ3T = string.Empty;
        string isQ4T = string.Empty;
        private void divideYearintoQuarters(string Year)
        {
            if (((Captain.Common.Utilities.ListItem)cmbQuarter.SelectedItem).Value.ToString() == "1")
            {
                dtpDteFrom.Text = "10/01/" + (Convert.ToInt32(Year) - 1);

                DateTime Q1 = DateTime.Parse(dtpDteFrom.Text);

                isQ1 = Q1.ToString();

                dtpDteTo.Text = Q1.AddMonths(3).AddDays(-1).ToString();

                isQ1T = dtpDteTo.Text;
            }
            else if (((Captain.Common.Utilities.ListItem)cmbQuarter.SelectedItem).Value.ToString() == "2")
            {
                dtpDteFrom.Text = "01/01/" + Year;

                DateTime Q1 = DateTime.Parse(dtpDteFrom.Text);

                isQ2 = Q1.ToString();

                dtpDteTo.Text = Q1.AddMonths(3).AddDays(-1).ToString();

                isQ2T = dtpDteTo.Text;
            }
            else if (((Captain.Common.Utilities.ListItem)cmbQuarter.SelectedItem).Value.ToString() == "3")
            {
                dtpDteFrom.Text = "04/01/" + Year;

                DateTime Q1 = DateTime.Parse(dtpDteFrom.Text);

                isQ3 = Q1.ToString();

                dtpDteTo.Text = Q1.AddMonths(3).AddDays(-1).ToString();

                isQ3T = dtpDteTo.Text;
            }
            else
            {
                dtpDteFrom.Text = "07/01/" + Year;

                DateTime Q1 = DateTime.Parse(dtpDteFrom.Text);

                isQ4 = Q1.ToString();

                dtpDteTo.Text = Q1.AddMonths(3).AddDays(-1).ToString();

                isQ4T = dtpDteTo.Text;
            }
        }

        private void cmbQuarter_SelectedIndexChanged(object sender, EventArgs e)
        {
            divideYearintoQuarters(txtFiscalYear.Text.Trim());
            /*if (isinBtwQ1)
            {
                dtpDteFrom.Text = "01/01/" + DateTime.Now.Year;

                DateTime Q1 = DateTime.Parse(dtpDteFrom.Text);

                dtpDteTo.Text = Q1.AddMonths(3).AddDays(-1).ToString();
            }

            if (isinBtwQ2)
            {
                dtpDteFrom.Text = "04/01/" + DateTime.Now.Year;

                DateTime Q1 = DateTime.Parse(dtpDteFrom.Text);

                dtpDteTo.Text = Q1.AddMonths(3).AddDays(-1).ToString();
            }


            if (isinBtwQ3)
            {
                dtpDteFrom.Text = "07/01/" + DateTime.Now.Year;

                DateTime Q1 = DateTime.Parse(dtpDteFrom.Text);

                dtpDteTo.Text = Q1.AddMonths(3).AddDays(-1).ToString();
            }

            if (isinBtwQ4)
            {
                dtpDteFrom.Text = "10/01/" + DateTime.Now.Year;

                DateTime Q1 = DateTime.Parse(dtpDteFrom.Text);

                dtpDteTo.Text = Q1.AddMonths(3).AddDays(-1).ToString();
            }*/
        }

        //private int GetFinancialQuarter(this DateTime date)
        //{
        //    return (date.AddMonths(-3).Month + 2) / 3;
        //}



        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (FromDateQuarterCheck() && ToDateQuarterCheck())
            {
                if (isValid == true)
                {
                    if (ValidateForm())
                    {
                        HUDCNTLEntity _hUDCNTLEntity = new HUDCNTLEntity();

                        foreach (DataRow dr in HUD_Data.Tables[0].Rows)
                        {
                            _hUDCNTLEntity.Agy = dr["HUDC_AGENCY"].ToString().Trim();
                            _hUDCNTLEntity.Tax_ID = dr["HUDC_TAX_ID"].ToString().Trim();
                            _hUDCNTLEntity.Dun_No = dr["HUDC_DUN_NO"].ToString().Trim();

                            _hUDCNTLEntity.Street = dr["HUDC_STREET"].ToString().Trim();
                            _hUDCNTLEntity.City = dr["HUDC_CITY"].ToString().Trim();
                            _hUDCNTLEntity.State = dr["HUDC_STATE"].ToString().Trim();
                            _hUDCNTLEntity.ZIP = dr["HUDC_ZIP"].ToString().Trim();

                            _hUDCNTLEntity.isMailAdd_AgyAdd = dr["HUDC_MAIL_ADDR"].ToString().Trim();

                            _hUDCNTLEntity.Mail_Street = dr["HUDC_MAIL_STREET"].ToString().Trim();
                            _hUDCNTLEntity.Mail_City = dr["HUDC_MAIL_CITY"].ToString().Trim();
                            _hUDCNTLEntity.Mail_State = dr["HUDC_MAIL_STATE"].ToString().Trim();
                            _hUDCNTLEntity.Mail_ZIP = dr["HUDC_MAIL_ZIP"].ToString().Trim();

                            _hUDCNTLEntity.Phone = dr["HUDC_PHONE"].ToString().Trim();
                            _hUDCNTLEntity.Email = dr["HUDC_EMAIL"].ToString().Trim();

                            _hUDCNTLEntity.Trans_Agy = dr["HUDC_TRANS_AGY"].ToString().Trim();
                            _hUDCNTLEntity.Trans_User = dr["HUDC_TRANS_USER"].ToString().Trim();
                            _hUDCNTLEntity.Trans_Pswrd = dr["HUDC_TRANS_PWD"].ToString().Trim();
                            _hUDCNTLEntity.Trans_URL = dr["HUDC_TRANS_URL"].ToString().Trim();

                            _hUDCNTLEntity.Faith_Org = dr["HUDC_FAITH_ORG"].ToString().Trim();
                            _hUDCNTLEntity.Rural_HH = dr["HUDC_RURAL_HH"].ToString().Trim();
                            _hUDCNTLEntity.Urban_HH = dr["HUDC_URBAN_HH"].ToString().Trim();
                            _hUDCNTLEntity.Coun_Methods = dr["HUDC_COUN_METHODS"].ToString().Trim();
                            _hUDCNTLEntity.Ser_Col = dr["HUDC_SER_COLONIAS"].ToString().Trim();
                            _hUDCNTLEntity.Ser_Farms = dr["HUDC_SER_FARMS"].ToString().Trim();
                            _hUDCNTLEntity.Budget = dr["HUDC_BUDGET"].ToString().Trim();
                            _hUDCNTLEntity.Lang = dr["HUDC_LANGUAGE"].ToString().Trim();

                            _hUDCNTLEntity.Lstc_Operator = dr["HUDC_LSTC_OPERATOR"].ToString().Trim();
                            _hUDCNTLEntity.Lstc_Date = dr["HUDC_DATE_LSTC"].ToString().Trim();

                            _hUDCNTLEntity.RPB_YEAR = txtFiscalYear.Text == "" ? "" : txtFiscalYear.Text.Trim();
                            _hUDCNTLEntity.RPB_QUARTER = ((Captain.Common.Utilities.ListItem)cmbQuarter.SelectedItem).Value.ToString().Trim();
                            _hUDCNTLEntity.RPB_FRM_DTE = dtpDteFrom.Text.Trim();
                            _hUDCNTLEntity.RPB_TO_DTE = dtpDteTo.Text.Trim();
                            _hUDCNTLEntity.RPB_SUBM_DTE = dtpSubmDte.Text.Trim();
                            _hUDCNTLEntity.RPB_UPDATE_DTE = dtpUpdateDte.Text.Trim();
                            _hUDCNTLEntity.RPB_TOT_BUDGET = txtTotBudget.Text == "" ? "" : txtTotBudget.Text.Trim();

                            _hUDCNTLEntity.RPB_TOT_FUND_BUDGET = txtTotFundBudget.Text == "" ? "" : txtTotFundBudget.Text.Trim();
                        }

                        if (_model.HUDCNTLData.InsertUpdateHUDCNTL(_hUDCNTLEntity, _mode.ToUpper()))
                        {
                            if (_mode.ToUpper() == "INSERT")
                                AlertBox.Show("Saved Successfully");
                            else
                                AlertBox.Show("Updated Successfully");

                            //DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                }
            }
            isValid = true;
        }

        bool isValid = true;
        private bool ValidateForm()
        {
            isValid = true;

            if (string.IsNullOrWhiteSpace(dtpDteFrom.Text))
            {
                _errorProvider.SetError(dtpDteFrom, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "From Date".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpDteFrom, null);
            }
            if (string.IsNullOrWhiteSpace(dtpDteTo.Text))
            {
                _errorProvider.SetError(dtpDteTo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "To Date ".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpDteTo, null);
            }

            if (Convert.ToDateTime(dtpDteFrom.Text) > Convert.ToDateTime(dtpDteTo.Text))
            {
                _errorProvider.SetError(dtpDteFrom, string.Format("'From Date' should be prior or equal to 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }

            return isValid;
        }

        private void dtpDteFrom_Leave(object sender, EventArgs e)
        {
            //if (ValidateForm())
            //{
            //    if (((Captain.Common.Utilities.ListItem)cmbQuarter.SelectedItem).Value.ToString() == "1")
            //    {
            //        if (Convert.ToDateTime(dtpDteFrom.Text) < Convert.ToDateTime(isQ1))
            //        {
            //            AlertBox.Show("The entered Date shouldn't be prior to " + Convert.ToDateTime(isQ1).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
            //            dtpDteFrom.Text = isQ1;

            //            isValid = false;
            //        }
            //    }
            //    if (((Captain.Common.Utilities.ListItem)cmbQuarter.SelectedItem).Value.ToString() == "2")
            //    {
            //        if (Convert.ToDateTime(dtpDteFrom.Text) < Convert.ToDateTime(isQ2))
            //        {
            //            AlertBox.Show("The entered Date shouldn't prior to " + Convert.ToDateTime(isQ2).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
            //            dtpDteFrom.Text = isQ2;

            //            isValid = false;
            //        }
            //    }
            //    if (((Captain.Common.Utilities.ListItem)cmbQuarter.SelectedItem).Value.ToString() == "3")
            //    {
            //        if (Convert.ToDateTime(dtpDteFrom.Text) < Convert.ToDateTime(isQ3))
            //        {
            //            AlertBox.Show("The entered Date shouldn't prior to " + Convert.ToDateTime(isQ3).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
            //            dtpDteFrom.Text = isQ3;

            //            isValid = false;
            //        }
            //    }
            //    if (((Captain.Common.Utilities.ListItem)cmbQuarter.SelectedItem).Value.ToString() == "4")
            //    {
            //        if (Convert.ToDateTime(dtpDteFrom.Text) < Convert.ToDateTime(isQ4))
            //        {
            //            AlertBox.Show("The entered Date shouldn't prior to " + Convert.ToDateTime(isQ4).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
            //            dtpDteFrom.Text = isQ4;
                        
            //            isValid = false;
            //        }
            //    }
            //}
        }

        private void dtpDteTo_Leave(object sender, EventArgs e)
        {
            //if (ValidateForm())
            //{
            //    if (((Captain.Common.Utilities.ListItem)cmbQuarter.SelectedItem).Value.ToString() == "1")
            //    {
            //        if (Convert.ToDateTime(dtpDteTo.Text) > Convert.ToDateTime(isQ1T))
            //        {
            //            AlertBox.Show("The entered Date shouldn't be greater than " + Convert.ToDateTime(isQ1T).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
            //            dtpDteTo.Text = isQ1T;

            //            isValid = false;
            //        }
            //    }
            //    if (((Captain.Common.Utilities.ListItem)cmbQuarter.SelectedItem).Value.ToString() == "2")
            //    {
            //        if (Convert.ToDateTime(dtpDteTo.Text) > Convert.ToDateTime(isQ2T))
            //        {
            //            AlertBox.Show("The entered Date shouldn't be greater than " + Convert.ToDateTime(isQ2T).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
            //            dtpDteTo.Text = isQ2T;

            //            isValid = false;
            //        }
            //    }
            //    if (((Captain.Common.Utilities.ListItem)cmbQuarter.SelectedItem).Value.ToString() == "3")
            //    {
            //        if (Convert.ToDateTime(dtpDteTo.Text) > Convert.ToDateTime(isQ3T))
            //        {
            //            AlertBox.Show("The entered Date shouldn't be greater than " + Convert.ToDateTime(isQ3T).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
            //            dtpDteTo.Text = isQ3T;

            //            isValid = false;
            //        }
            //    }
            //    if (((Captain.Common.Utilities.ListItem)cmbQuarter.SelectedItem).Value.ToString() == "4")
            //    {
            //        if (Convert.ToDateTime(dtpDteTo.Text) > Convert.ToDateTime(isQ4T))
            //        {
            //            AlertBox.Show("The entered Date shouldn't be greater than " + Convert.ToDateTime(isQ4T).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
            //            dtpDteTo.Text = isQ4T;

            //            isValid = false;
            //        }
            //    }
            //}
        }

        private bool FromDateQuarterCheck()
        {
            if (ValidateForm())
            {
                if (((Captain.Common.Utilities.ListItem)cmbQuarter.SelectedItem).Value.ToString() == "1")
                {
                    if (Convert.ToDateTime(dtpDteFrom.Text) < Convert.ToDateTime(isQ1))
                    {
                        AlertBox.Show("The entered Date shouldn't be prior to " + Convert.ToDateTime(isQ1).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
                        dtpDteFrom.Text = isQ1;

                        isValid = false;
                    }
                }
                if (((Captain.Common.Utilities.ListItem)cmbQuarter.SelectedItem).Value.ToString() == "2")
                {
                    if (Convert.ToDateTime(dtpDteFrom.Text) < Convert.ToDateTime(isQ2))
                    {
                        AlertBox.Show("The entered Date shouldn't prior to " + Convert.ToDateTime(isQ2).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
                        dtpDteFrom.Text = isQ2;

                        isValid = false;
                    }
                }
                if (((Captain.Common.Utilities.ListItem)cmbQuarter.SelectedItem).Value.ToString() == "3")
                {
                    if (Convert.ToDateTime(dtpDteFrom.Text) < Convert.ToDateTime(isQ3))
                    {
                        AlertBox.Show("The entered Date shouldn't prior to " + Convert.ToDateTime(isQ3).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
                        dtpDteFrom.Text = isQ3;

                        isValid = false;
                    }
                }
                if (((Captain.Common.Utilities.ListItem)cmbQuarter.SelectedItem).Value.ToString() == "4")
                {
                    if (Convert.ToDateTime(dtpDteFrom.Text) < Convert.ToDateTime(isQ4))
                    {
                        AlertBox.Show("The entered Date shouldn't prior to " + Convert.ToDateTime(isQ4).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
                        dtpDteFrom.Text = isQ4;

                        isValid = false;
                    }
                }
            }

            return isValid;
        }

        private bool ToDateQuarterCheck()
        {
            if (ValidateForm())
            {
                if (((Captain.Common.Utilities.ListItem)cmbQuarter.SelectedItem).Value.ToString() == "1")
                {
                    if (Convert.ToDateTime(dtpDteTo.Text) > Convert.ToDateTime(isQ1T))
                    {
                        AlertBox.Show("The entered Date shouldn't be greater than " + Convert.ToDateTime(isQ1T).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
                        dtpDteTo.Text = isQ1T;

                        isValid = false;
                    }
                }
                if (((Captain.Common.Utilities.ListItem)cmbQuarter.SelectedItem).Value.ToString() == "2")
                {
                    if (Convert.ToDateTime(dtpDteTo.Text) > Convert.ToDateTime(isQ2T))
                    {
                        AlertBox.Show("The entered Date shouldn't be greater than " + Convert.ToDateTime(isQ2T).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
                        dtpDteTo.Text = isQ2T;

                        isValid = false;
                    }
                }
                if (((Captain.Common.Utilities.ListItem)cmbQuarter.SelectedItem).Value.ToString() == "3")
                {
                    if (Convert.ToDateTime(dtpDteTo.Text) > Convert.ToDateTime(isQ3T))
                    {
                        AlertBox.Show("The entered Date shouldn't be greater than " + Convert.ToDateTime(isQ3T).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
                        dtpDteTo.Text = isQ3T;

                        isValid = false;
                    }
                }
                if (((Captain.Common.Utilities.ListItem)cmbQuarter.SelectedItem).Value.ToString() == "4")
                {
                    if (Convert.ToDateTime(dtpDteTo.Text) > Convert.ToDateTime(isQ4T))
                    {
                        AlertBox.Show("The entered Date shouldn't be greater than " + Convert.ToDateTime(isQ4T).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
                        dtpDteTo.Text = isQ4T;

                        isValid = false;
                    }
                }
            }

            return isValid;
        }
    }
}
