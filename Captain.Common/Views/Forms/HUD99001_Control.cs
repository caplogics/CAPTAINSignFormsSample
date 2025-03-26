using Captain.Common.Interfaces;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Controls.Compatibility;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Views.UserControls;
using Captain.DatabaseLayer;
using DevExpress.Data.Extensions;
using DevExpress.DataAccess.Sql;
using DevExpress.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class HUD99001_Control : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion

        public HUD99001_Control(BaseForm baseform, PrivilegeEntity privileges)
        {
            InitializeComponent();

            _baseForm = baseform;
            _privilegeEntity = privileges;
            _model = new CaptainModel();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            pnlControl.Enabled = false;
            pnlSave.Enabled = false;

            txtZIP.Validator = TextBoxValidation.IntegerValidator;
            txtMZIP.Validator = TextBoxValidation.IntegerValidator;

            txtTotBudget.Validator = TextBoxValidation.CustomDecimalValidation15dot2;
            _mode = "View";

            GetHUD_Data();
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

        public DataSet HUD_Data
        {
            get;
            set;
        }

        #endregion

        private void GetHUD_Data()
        {
            btnSave.Visible = btnCancel.Visible = false;

            HUD_Data = Captain.DatabaseLayer.HUDCNTLDB.Get_HUDCNTL(_baseForm.BaseAdminAgency);

            if (HUD_Data.Tables[0].Rows.Count > 0)
            {
                pbAdd.Visible = false;spacerAdd.Visible = false;
                pbEdit.Visible = true;
                spacerEdit.Visible = true;
                pbDelete.Visible = true;
                spacerDelete.Visible = true;

                fillData(HUD_Data);
                btnHUDContacts.Visible = btnHUDCounslers.Visible = btnRepPeriodBudget.Visible = true;
                pnlSave.Enabled = true;
            }
            else
            {
                pbAdd.Visible = true;
                spacerAdd.Visible = true;
                pbEdit.Visible = false;spacerEdit.Visible = false;
                pbDelete.Visible = false;spacerDelete.Visible = false;

                fillCounelingMethods(null);
                fillAgyLang(null);
                btnHUDContacts.Visible = btnHUDCounslers.Visible = btnRepPeriodBudget.Visible = false;
                pnlSave.Enabled = false;
            }
        }

        private void fillData(DataSet hUD_Data)
        {
            DataTable dt = new DataTable(); 
            dt = hUD_Data.Tables[0];

            txtTaxID.Text = dt.Rows[0]["HUDC_TAX_ID"].ToString();
            txtDunBN.Text = dt.Rows[0]["HUDC_DUN_NO"].ToString();

            txtStreet.Text = dt.Rows[0]["HUDC_STREET"].ToString();
            txtCity.Text = dt.Rows[0]["HUDC_CITY"].ToString();
            txtState.Text = dt.Rows[0]["HUDC_STATE"].ToString();
            txtZIP.Text = dt.Rows[0]["HUDC_ZIP"].ToString();

            txtMStreet.Text = dt.Rows[0]["HUDC_MAIL_STREET"].ToString();
            txtMCity.Text = dt.Rows[0]["HUDC_MAIL_CITY"].ToString();
            txtMState.Text = dt.Rows[0]["HUDC_MAIL_STATE"].ToString();
            txtMZIP.Text = dt.Rows[0]["HUDC_MAIL_ZIP"].ToString();

            mstxtPhone.Text = dt.Rows[0]["HUDC_PHONE"].ToString();
            txtEmailID.Text = dt.Rows[0]["HUDC_EMAIL"].ToString();

            txtTAgyID.Text = dt.Rows[0]["HUDC_TRANS_AGY"].ToString();
            txtTUserName.Text = dt.Rows[0]["HUDC_TRANS_USER"].ToString();
            txtTPasswrd.Text = dt.Rows[0]["HUDC_TRANS_PWD"].ToString();
            txtTWebURL.Text = dt.Rows[0]["HUDC_TRANS_URL"].ToString();

            if (dt.Rows[0]["HUDC_MAIL_ADDR"].ToString() == "Y")
                chkbisMailAddress.Checked = true;
            else
                chkbisMailAddress.Checked = false;

            if (dt.Rows[0]["HUDC_FAITH_ORG"].ToString() == "Y")
                chkbFaithOrg.Checked = true;
            else
                chkbFaithOrg.Checked = false;

            if (dt.Rows[0]["HUDC_RURAL_HH"].ToString() == "Y")
                chkbRuralHH.Checked = true;
            else
                chkbRuralHH.Checked = false;

            if (dt.Rows[0]["HUDC_URBAN_HH"].ToString() == "Y")
                chkbUrbanHH.Checked = true;
            else
                chkbUrbanHH.Checked = false;

            if (dt.Rows[0]["HUDC_SER_COLONIAS"].ToString() == "Y")
                chkbServColonias.Checked = true;
            else
                chkbServColonias.Checked = false;

            if (dt.Rows[0]["HUDC_SER_FARMS"].ToString() == "Y")
                chkbServFarm.Checked = true;
            else
                chkbServFarm.Checked = false;

            txtTotBudget.Text = dt.Rows[0]["HUDC_BUDGET"].ToString();

            string[] sel_Coun_Methods = null;
            if (!string.IsNullOrEmpty(dt.Rows[0]["HUDC_COUN_METHODS"].ToString()))
               sel_Coun_Methods = dt.Rows[0]["HUDC_COUN_METHODS"].ToString().Replace(" ", "").Split(',');

            fillCounelingMethods(sel_Coun_Methods);

            string[] sel_Lang = null;
            if (!string.IsNullOrEmpty(dt.Rows[0]["HUDC_LANGUAGE"].ToString()))
                sel_Lang = dt.Rows[0]["HUDC_LANGUAGE"].ToString().Replace(" ", "").Split(',');

            fillAgyLang(sel_Lang);

        }

        private void fillAgyLang(string[] sel_Lang)
        {
            DataSet dsAgyLang = Lookups.GetLookUpFromAGYTAB("00353");
            DataTable dtAgyLang = dsAgyLang.Tables[0];

            DataView dv = new DataView(dtAgyLang);
            dv.Sort = "Code";

            dtAgyLang = dv.ToTable();

            dgvAgyLang.Rows.Clear();

            foreach (DataRow drAgyLang in dtAgyLang.Rows)
            {
                bool isChecked = false;

                if (sel_Lang != null)
                {
                    if (sel_Lang.Contains(drAgyLang["Code"].ToString().Trim()))
                    {
                        isChecked = true;
                    }
                }

                if (_mode == "View" && isChecked)
                    dgvAgyLang.Rows.Add(isChecked, drAgyLang["Code"].ToString().Trim(), drAgyLang["LookUpDesc"].ToString().Trim());
                else if (_mode == "INSERT" || _mode == "UPDATE")
                    dgvAgyLang.Rows.Add(isChecked, drAgyLang["Code"].ToString().Trim(), drAgyLang["LookUpDesc"].ToString().Trim());

                

            }
            if (_mode == "View")
                dgvAgyLang.Columns["Column3"].Visible = false;
            else dgvAgyLang.Columns["Column3"].Visible = true;
        }

        private void fillCounelingMethods(string[] sel_Coun_Methods)
        {
            DataSet dsHUDCouns = Lookups.GetLookUpFromAGYTAB("99021");
            DataTable dtHUDCouns = dsHUDCouns.Tables[0];

            DataView dv = new DataView(dtHUDCouns);
            dv.Sort = "Code";

            dtHUDCouns = dv.ToTable();

            dgvCounselingMethods.Rows.Clear();

            foreach (DataRow drHUDCouns in dtHUDCouns.Rows)
            {
                bool isChecked = false;

                if (sel_Coun_Methods != null)
                {
                    if (sel_Coun_Methods.Contains(drHUDCouns["Code"].ToString().Trim()))
                    {
                        isChecked = true;
                    }
                }


                if (_mode == "View" && isChecked)
                    dgvCounselingMethods.Rows.Add(isChecked, drHUDCouns["Code"].ToString().Trim(), drHUDCouns["LookUpDesc"].ToString().Trim());
                else if(_mode== "INSERT" || _mode== "UPDATE")
                    dgvCounselingMethods.Rows.Add(isChecked, drHUDCouns["Code"].ToString().Trim(), drHUDCouns["LookUpDesc"].ToString().Trim());

                
            }
            if (_mode == "View")
                dgvCounselingMethods.Columns["Column0"].Visible = false;
            else dgvCounselingMethods.Columns["Column0"].Visible = true;

        }

        string _mode = string.Empty;
        private void pbAdd_Click(object sender, EventArgs e)
        {
            _mode = "INSERT";

            pnlControl.Enabled = true;
            pnlSave.Enabled = true;
            this.txtTaxID.Focus();

            pbAdd.Enabled = false;

            pbEdit.Visible = false;
            spacerEdit.Visible = false;
            pbDelete.Visible = false;
            spacerDelete.Visible = false;
            btnSave.Visible = btnCancel.Visible = true;

            fillCounelingMethods(null);

            fillAgyLang(null);

            //this.Column0.Visible = true;
            //this.Column3.Visible = true;
        }

        private void pbEdit_Click(object sender, EventArgs e)
        {
            _mode = "UPDATE";

            pnlControl.Enabled = true;
            pnlSave.Enabled = true;
            this.txtTaxID.Focus();

            pbEdit.Enabled = false;

            btnHUDContacts.Visible = btnHUDCounslers.Visible = btnRepPeriodBudget.Visible = true;
            btnSave.Visible = btnCancel.Visible = true;
            
            GetHUD_Data();

            btnSave.Visible = btnCancel.Visible = true;

            fillData(HUD_Data);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                HUDCNTLEntity hUDCNTLEntity = new HUDCNTLEntity();

                hUDCNTLEntity.Agy = _baseForm.BaseAdminAgency;
                hUDCNTLEntity.Tax_ID = txtTaxID.Text == "" ? "" : txtTaxID.Text.Trim();
                hUDCNTLEntity.Dun_No = txtDunBN.Text == "" ? "" : txtDunBN.Text.Trim();

                hUDCNTLEntity.Street = txtStreet.Text == "" ? "" : txtStreet.Text.Trim();
                hUDCNTLEntity.City = txtCity.Text == "" ? "" : txtCity.Text.Trim();
                hUDCNTLEntity.State = txtState.Text == "" ? "" : txtState.Text.Trim();
                hUDCNTLEntity.ZIP = txtZIP.Text == "" ? "" : txtZIP.Text.Trim();

                hUDCNTLEntity.isMailAdd_AgyAdd = chkbisMailAddress.Checked ? "Y" : "N";

                hUDCNTLEntity.Mail_Street = txtMStreet.Text == "" ? "" : txtMStreet.Text.Trim();
                hUDCNTLEntity.Mail_City = txtMCity.Text == "" ? "" : txtMCity.Text.Trim();
                hUDCNTLEntity.Mail_State = txtMState.Text == "" ? "" : txtMState.Text.Trim();
                hUDCNTLEntity.Mail_ZIP = txtMZIP.Text == "" ? "" : txtMZIP.Text.Trim();

                hUDCNTLEntity.Phone = mstxtPhone.Text == "" ? "" : mstxtPhone.Text.Trim().Replace("-", "");
                hUDCNTLEntity.Email = txtEmailID.Text == "" ? "" : txtEmailID.Text.Trim();

                hUDCNTLEntity.Trans_Agy = txtTAgyID.Text == "" ? "" : txtTAgyID.Text.Trim();
                hUDCNTLEntity.Trans_User = txtTUserName.Text == "" ? "" : txtTUserName.Text.Trim();
                hUDCNTLEntity.Trans_Pswrd = txtTPasswrd.Text == "" ? "" : txtTPasswrd.Text.Trim();
                hUDCNTLEntity.Trans_URL = txtTWebURL.Text == "" ? "" : txtTWebURL.Text.Trim();

                hUDCNTLEntity.Faith_Org = chkbFaithOrg.Checked ? "Y" : "N";
                hUDCNTLEntity.Rural_HH = chkbRuralHH.Checked ? "Y" : "N";
                hUDCNTLEntity.Urban_HH = chkbRuralHH.Checked ? "Y" : "N";

                hUDCNTLEntity.Coun_Methods = Get_Sel_Counsil_Methods();

                hUDCNTLEntity.Ser_Col = chkbServColonias.Checked ? "Y" : "N";
                hUDCNTLEntity.Ser_Farms = chkbServFarm.Checked ? "Y" : "N";

                hUDCNTLEntity.Budget = txtTotBudget.Text == "" ? "" : txtTotBudget.Text.Trim();

                hUDCNTLEntity.Lang = Get_Sel_Lang();

                hUDCNTLEntity.Add_Operator = _baseForm.UserID;
                hUDCNTLEntity.Lstc_Operator = _baseForm.UserID;

                if (HUD_Data.Tables[0].Rows.Count > 0)
                {

                    if (!string.IsNullOrEmpty(HUD_Data.Tables[0].Rows[0]["HUDC_RPB_FISCAL_YEAR"].ToString()))
                        hUDCNTLEntity.RPB_YEAR = HUD_Data.Tables[0].Rows[0]["HUDC_RPB_FISCAL_YEAR"].ToString();

                    if (!string.IsNullOrEmpty(HUD_Data.Tables[0].Rows[0]["HUDC_RPB_QUARTER"].ToString()))
                        hUDCNTLEntity.RPB_QUARTER = HUD_Data.Tables[0].Rows[0]["HUDC_RPB_QUARTER"].ToString();

                    if (!string.IsNullOrEmpty(HUD_Data.Tables[0].Rows[0]["HUDC_RPB_FROM_DATE"].ToString()))
                        hUDCNTLEntity.RPB_FRM_DTE = HUD_Data.Tables[0].Rows[0]["HUDC_RPB_FROM_DATE"].ToString();

                    if (!string.IsNullOrEmpty(HUD_Data.Tables[0].Rows[0]["HUDC_RPB_TO_DATE"].ToString()))
                        hUDCNTLEntity.RPB_TO_DTE = HUD_Data.Tables[0].Rows[0]["HUDC_RPB_TO_DATE"].ToString();

                    if (!string.IsNullOrEmpty(HUD_Data.Tables[0].Rows[0]["HUDC_RPB_SUBM_DATE"].ToString()))
                        hUDCNTLEntity.RPB_SUBM_DTE = HUD_Data.Tables[0].Rows[0]["HUDC_RPB_SUBM_DATE"].ToString();

                    if (!string.IsNullOrEmpty(HUD_Data.Tables[0].Rows[0]["HUDC_RPB_UPDATE_DATE"].ToString()))
                        hUDCNTLEntity.RPB_UPDATE_DTE = HUD_Data.Tables[0].Rows[0]["HUDC_RPB_UPDATE_DATE"].ToString();

                    if (!string.IsNullOrEmpty(HUD_Data.Tables[0].Rows[0]["HUDC_RPB_TOT_BUDGET"].ToString()))
                        hUDCNTLEntity.RPB_TOT_BUDGET = HUD_Data.Tables[0].Rows[0]["HUDC_RPB_TOT_BUDGET"].ToString();

                    if (!string.IsNullOrEmpty(HUD_Data.Tables[0].Rows[0]["HUDC_RPB_TOT_FUND_BUDGET"].ToString()))
                        hUDCNTLEntity.RPB_TOT_FUND_BUDGET = HUD_Data.Tables[0].Rows[0]["HUDC_RPB_TOT_FUND_BUDGET"].ToString();
                }
                if (_model.HUDCNTLData.InsertUpdateHUDCNTL(hUDCNTLEntity, _mode.ToUpper()))
                {
                    if (_mode.ToUpper() == "INSERT")
                        AlertBox.Show("Saved Successfully");
                    else
                        AlertBox.Show("Updated Successfully");

                    _mode = "View";
                    GetHUD_Data();
                    pbEdit.Enabled = true;

                    pnlControl.Enabled = false;
                }

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnSave.Visible = false;
            btnCancel.Visible = false;

            ClearData();

            _mode = "View";

            GetHUD_Data();

            pnlControl.Enabled = false;

            if (pbEdit.Visible == true)
                pbEdit.Enabled = true;

            if (pbAdd.Visible == true)
                pbAdd.Enabled = true;
        }

        private void ClearData()
        {
            txtTaxID.Text = string.Empty;
            txtDunBN.Text = string.Empty;

            txtStreet.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtState.Text = string.Empty;
            txtZIP.Text = string.Empty;

            txtMStreet.Text = string.Empty;
            txtMCity.Text = string.Empty;
            txtMState.Text = string.Empty;
            txtMZIP.Text = string.Empty;

            mstxtPhone.Text = string.Empty;
            txtEmailID.Text = string.Empty;

            txtTAgyID.Text = string.Empty;
            txtTUserName.Text = string.Empty;
            txtTPasswrd.Text = string.Empty;
            txtTWebURL.Text = string.Empty;

            chkbisMailAddress.Checked = false;

            chkbFaithOrg.Checked = false;

            chkbRuralHH.Checked = false;

            chkbUrbanHH.Checked = false;

            chkbServColonias.Checked = false;

            chkbServFarm.Checked = false;

            txtTotBudget.Text = string.Empty;
        }

        private bool ValidateForm()
        {
            bool isValid = true;
            _errorProvider.SetError(mstxtPhone, null);
            _errorProvider.SetError(txtEmailID, null);

            if (txtEmailID.Text.Trim().Length > 0)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmailID.Text, Consts.StaticVars.EmailValidatingString))
                {
                    _errorProvider.SetError(txtEmailID, Consts.Messages.PleaseEnterEmail);
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtEmailID, null);
                }
            }

            if (mstxtPhone.Text != "" && mstxtPhone.Text != "   -   -")
            {
                if (mstxtPhone.Text.Trim().Replace("-", "").Replace(" ", "").Length < 10)
                {
                    _errorProvider.SetError(mstxtPhone, "Please enter a valid Phone#");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(mstxtPhone, null);
                }
            }

            return isValid;
        }

        string sel_Counsil_Methods = string.Empty;
        private string Get_Sel_Counsil_Methods()
        {
            sel_Counsil_Methods = string.Empty;

            foreach (DataGridViewRow dr in dgvCounselingMethods.Rows)
            {
                if (dr["Column0"].Value.ToString() == "True")
                {
                    sel_Counsil_Methods = sel_Counsil_Methods + (dr["Column1"].Value.ToString() + ", ");
                }
            }
            return sel_Counsil_Methods.Trim().TrimEnd(',');
        }

        string sel_Lang = string.Empty;
        private string Get_Sel_Lang()
        {
            sel_Lang = string.Empty;

            foreach (DataGridViewRow dr in dgvAgyLang.Rows)
            {
                if (dr["Column3"].Value.ToString() == "True")
                {
                    sel_Lang = sel_Lang + (dr["Column4"].Value.ToString() + ", ");
                }
            }
            return sel_Lang.Trim().TrimEnd(',');
        }

        private void chkbisMailAddress_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbisMailAddress.Checked)
            {
                txtMStreet.Text = txtStreet.Text.Trim();
                txtMCity.Text = txtCity.Text.Trim();
                txtMState.Text = txtState.Text.Trim();
                txtMZIP.Text = txtZIP.Text.Trim();

                pnlMAddress1.Enabled = pnlMAddress2.Enabled = false;
            }
            else
            {
                txtMStreet.Text = string.Empty;
                txtMCity.Text = string.Empty;
                txtMState.Text = string.Empty;
                txtMZIP.Text = string.Empty;

                pnlMAddress1.Enabled = pnlMAddress2.Enabled = true;

            }
        }

        private void pbDelete_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Record);
        }

        private void Delete_Record(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                HUDCNTLEntity hUDCNTLEntity = new HUDCNTLEntity();

                hUDCNTLEntity.Agy = _baseForm.BaseAdminAgency;

                if (_model.HUDCNTLData.InsertUpdateHUDCNTL(hUDCNTLEntity, ""))
                {
                    AlertBox.Show("Deleted Successfully");
                    this.Close();
                }
            }
        }

        private void pbHelp_Click(object sender, EventArgs e)
        {

        }

        private void btnHUDContacts_Click(object sender, EventArgs e)
        {
            STAFFMSTEntity Search_STAFFMST = new STAFFMSTEntity(true);
            Search_STAFFMST.Agency = _baseForm.BaseAdminAgency;
            List<STAFFMSTEntity> STAFFMST_List = _model.STAFFData.Browse_STAFFMST(Search_STAFFMST, "Browse");
            STAFFMST_List = STAFFMST_List.OrderBy(u => u.Active).ToList();

            if (STAFFMST_List.Count > 0)
            {

                HUD99001_Staff staffForm_Contacts = new HUD99001_Staff("C", _baseForm, _privilegeEntity);
                staffForm_Contacts.StartPosition = FormStartPosition.CenterScreen;
                staffForm_Contacts.ShowDialog();
            }
            else
                AlertBox.Show("No Staff Members found in STAFFMST", MessageBoxIcon.Warning);
        }

        private void btnHUDCounslers_Click(object sender, EventArgs e)
        {
            STAFFMSTEntity Search_STAFFMST = new STAFFMSTEntity(true);
            Search_STAFFMST.Agency = _baseForm.BaseAdminAgency;
            List<STAFFMSTEntity> STAFFMST_List = _model.STAFFData.Browse_STAFFMST(Search_STAFFMST, "Browse");
            STAFFMST_List = STAFFMST_List.OrderBy(u => u.Active).ToList();

            if (STAFFMST_List.Count > 0)
            {
                HUD99001_Staff staffForm_Couns = new HUD99001_Staff("CO", _baseForm, _privilegeEntity);
                staffForm_Couns.StartPosition = FormStartPosition.CenterScreen;
                staffForm_Couns.ShowDialog();
            }
            else
                AlertBox.Show("No Staff Members found in STAFFMST", MessageBoxIcon.Warning);
        }

        private void btnRepPeriodBudget_Click(object sender, EventArgs e)
        {
            if (HUD_Data.Tables[0].Rows.Count > 0)
                _mode = "UPDATE";
            else
                _mode = "INSERT";

            HUD_Data = Captain.DatabaseLayer.HUDCNTLDB.Get_HUDCNTL(_baseForm.BaseAdminAgency);

            HUD99001_RepPeriod_Budget repPeriod_budget = new HUD99001_RepPeriod_Budget(_baseForm, _privilegeEntity, _mode, HUD_Data);
            //repPeriod_budget.FormClosed += new FormClosedEventHandler(On_Form_Closed);
            repPeriod_budget.StartPosition = FormStartPosition.CenterScreen;
            repPeriod_budget.ShowDialog();
        }

        private void On_Form_Closed(object sender, FormClosedEventArgs e)
        {
            HUD99001_RepPeriod_Budget form = sender as HUD99001_RepPeriod_Budget;

            if (form.DialogResult == DialogResult.OK)
            {
                HUD_Data = Captain.DatabaseLayer.HUDCNTLDB.Get_HUDCNTL(_baseForm.BaseAdminAgency);
            }
        }

    }
}
