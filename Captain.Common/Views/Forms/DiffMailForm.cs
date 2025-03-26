#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;


using Wisej.Web;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class DiffMailForm : Form
    {
        private CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;


        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity privilege { get; set; }

        public CaseDiffEntity caseDiffMainHist { get; set; }
        public string MainMenuAgency { get; set; }
        public string MainMenuDept { get; set; }
        public string MainMenuProgram { get; set; }
        public string MainMenuYear { get; set; }
        public string ApplicationNo { get; set; }
        public string FormType { get; set; }
        public string Mode { get; set; }
        public CaseDiffEntity caseDiffLLData { get; set; }
        public CaseDiffEntity caseDiffMailAddressData { get; set; }
        public List<FldcntlHieEntity> propFLDCNTLHieEntity { get; set; }
        public DiffMailForm(BaseForm baseForm, string strAgency, string strDept, string strProgram, string strYear, string strApplicationNo, PrivilegeEntity privilegeEntity, string mode, string strFormType, CaseDiffEntity diffmailaddress)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.IconPadding = 2;
            _model = new CaptainModel();
            BaseForm = baseForm;
            privilege = privilegeEntity;
            MainMenuAgency = strAgency;
            MainMenuDept = strDept;
            MainMenuProgram = strProgram;
            MainMenuYear = strYear;
            ApplicationNo = strApplicationNo;
            Mode = mode;
            //txtZip.Validator = TextBoxValidation.IntegerValidator;
            //txtZipPlus.Validator = TextBoxValidation.IntegerValidator;
            fillDropdowns();
            caseDiffMailAddressData = diffmailaddress;
            FormType = string.Empty;
            fillForm();
            this.Text = privilegeEntity.Program + " - Different Mailing Address";
           
            EnableDisableControls();

            if (mode.Equals(Consts.Common.View))
            {
                btnCancel.Text = "Close";
                btnSave.Visible = false;
                btnZipSearch.Visible = false;
                panel6.Enabled = false;

            }
            if (Mode.Equals(Consts.Common.Add))
            {
                btnSave.Text = "OK";
            }
        }


        public DiffMailForm(BaseForm baseForm, string strAgency, string strDept, string strProgram, string strYear, string strApplicationNo, PrivilegeEntity privilegeEntity, string mode, string strFormType, CaseDiffEntity difflandlord, List<FldcntlHieEntity> FLDCNTLHieEntity)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.IconPadding = 2;
            _model = new CaptainModel();
            BaseForm = baseForm;
            privilege = privilegeEntity;
            MainMenuAgency = strAgency;
            MainMenuDept = strDept;
            MainMenuProgram = strProgram;
            MainMenuYear = strYear;
            ApplicationNo = strApplicationNo;
            Mode = mode;
            propFLDCNTLHieEntity = FLDCNTLHieEntity;
            //txtZip.Validator = TextBoxValidation.IntegerValidator;
            //txtZipPlus.Validator = TextBoxValidation.IntegerValidator;
            fillDropdowns();
            caseDiffLLData = difflandlord;
            FormType = "Landlord";
            fillLandlordForm();
            lblFirst.Text = "First Name";
            lblLast.Text = "        Last Name";
            this.Text = privilegeEntity.Program + " - Landlord Information";
            
            EnableDisableControlsLandlord();

            // groupBox1.Text = "Only fill this box if the Landlord Information is differenct from Client Address:";
            lblMessage.Text = "";
            lblMessage.Visible = false;

            if (mode.Equals(Consts.Common.View))
            {
                btnCancel.Text = "Close";
                btnSave.Visible = false;
                btnZipSearch.Visible = false;
                panel6.Enabled = false;

            }
            if (Mode.Equals(Consts.Common.Add))
            {
                btnSave.Text = "OK";
            }
        }

        private void EnableDisableControlsLandlord()
        {

            if (propFLDCNTLHieEntity.Count > 0)
            {
                foreach (FldcntlHieEntity entity in propFLDCNTLHieEntity)
                {
                    bool required = entity.Req.Equals("Y") ? true : false;
                    bool enabled = entity.Enab.Equals("Y") ? true : false;

                    switch (entity.FldCode)
                    {
                        case Consts.CASE2001.LLFirst:
                            if (enabled) { txtFirst.Enabled = lblFirst.Enabled = true; if (required) lblFirstReq.Visible = true; } else { txtFirst.Enabled = lblFirst.Enabled = false; lblFirstReq.Visible = false; }
                            break;
                        case Consts.CASE2001.LLLast:
                            if (enabled) { txtLast.Enabled = lblLast.Enabled = true; if (required) lblLastReq.Visible = true; } else { txtLast.Enabled = lblLast.Enabled = false; lblLastReq.Visible = false; }
                            break;
                        case Consts.CASE2001.LLHouseNo:
                            if (enabled) { txtHouseNo.Enabled = lblHouseNo.Enabled = true; if (required) lblDiffHouseNoReq.Visible = true; } else { txtHouseNo.Enabled = lblHouseNo.Enabled = false; lblDiffHouseNoReq.Visible = false; }
                            break;
                        case Consts.CASE2001.LLStreet:
                            if (enabled) { txtMailStreet.Enabled = lblStreet.Enabled = true; if (required) lblDiffStreetReq.Visible = true; } else { txtMailStreet.Enabled = lblStreet.Enabled = false; lblDiffStreetReq.Visible = false; }
                            break;
                        case Consts.CASE2001.LLSf:
                            if (enabled) { txtMailSuffix.Enabled = lblSuffix.Enabled = true; if (required) lblSuffixReq.Visible = true; } else { txtMailSuffix.Enabled = lblSuffix.Enabled = false; lblSuffixReq.Visible = false; }
                            break;
                        case Consts.CASE2001.LLFlr:
                            if (enabled) { txtMailFloor.Enabled = lblFloor.Enabled = true; if (required) lblFloorReq.Visible = true; } else { txtMailFloor.Enabled = lblFloor.Enabled = false; lblFloorReq.Visible = false; }
                            break;
                        case Consts.CASE2001.LLCity:
                            if (enabled) { txtCityName.Enabled = lblCityName.Enabled = true; if (required) lblDiffCityReq.Visible = true; } else { txtCityName.Enabled = lblCityName.Enabled = false; lblDiffCityReq.Visible = false; }
                            break;
                        case Consts.CASE2001.LLApt:
                            if (enabled) { txtMailApartment.Enabled = lblApartment.Enabled = true; if (required) lblApartmentReq.Visible = true; } else { txtMailApartment.Enabled = lblApartment.Enabled = false; lblApartmentReq.Visible = false; }
                            break;
                        case Consts.CASE2001.LLZip:
                            if (enabled) { txtZip.Enabled = lblZipCode.Enabled = txtZipPlus.Enabled = true; if (required) lblZipCodeReq.Visible = true; } else { txtZip.Enabled = txtZipPlus.Enabled = lblZipCode.Enabled = false; lblZipCodeReq.Visible = false; }
                            break;
                        case Consts.CASE2001.LLCounty:
                            if (enabled) { cmbMailCounty.Enabled = lblCounty.Enabled = true; if (required) lblCountyReq.Visible = true; } else { cmbMailCounty.Enabled = lblCounty.Enabled = false; lblCountyReq.Visible = false; }
                            break;
                        case Consts.CASE2001.LLState:
                            if (enabled) { txtMailState.Enabled = lblState.Enabled = true; if (required) lblDiffStateReq.Visible = true; } else { txtMailState.Enabled = lblState.Enabled = false; lblDiffStateReq.Visible = false; }
                            break;
                        case Consts.CASE2001.LLPhone:
                            if (enabled) { txtMailPhone.Enabled = lblPhone.Enabled = true; if (required) lblPhoneReq.Visible = true; } else { txtMailPhone.Enabled = lblPhone.Enabled = false; lblPhoneReq.Visible = false; }
                            break;
                    }
                }
            }
            else
            {
                btnZipSearch.Enabled = false;
                btnSave.Enabled = false;
            }

        }


     



        private void EnableDisableControls()
        {
            string HIE = MainMenuAgency + MainMenuDept + MainMenuProgram;
            List<FldcntlHieEntity> CntlEntity = _model.FieldControls.GetFLDCNTLHIE("CASE2001", HIE, "FLDCNTL");
            if (CntlEntity.Count > 0)
            {
                foreach (FldcntlHieEntity entity in CntlEntity)
                {
                    bool required = entity.Req.Equals("Y") ? true : false;
                    bool enabled = entity.Enab.Equals("Y") ? true : false;

                    switch (entity.FldCode)
                    {
                        case Consts.CASE2001.InCaseOfFirst:
                            if (enabled) { txtFirst.Enabled = lblFirst.Enabled = true; if (required) lblFirstReq.Visible = true; } else { txtFirst.Enabled = lblFirst.Enabled = false; lblFirstReq.Visible = false; }
                            break;
                        case Consts.CASE2001.InCaseOfLast:
                            if (enabled) { txtLast.Enabled = lblLast.Enabled = true; if (required) lblLastReq.Visible = true; } else { txtLast.Enabled = lblLast.Enabled = false; lblLastReq.Visible = false; }
                            break;
                        case Consts.CASE2001.MailHouseNo:
                            if (enabled) { txtHouseNo.Enabled = lblHouseNo.Enabled = true; if (required) lblDiffHouseNoReq.Visible = true; } else { txtHouseNo.Enabled = lblHouseNo.Enabled = false; lblDiffHouseNoReq.Visible = false; }
                            break;
                        case Consts.CASE2001.MailStreet:
                            if (enabled) { txtMailStreet.Enabled = lblStreet.Enabled = true; if (required) lblDiffStreetReq.Visible = true; } else { txtMailStreet.Enabled = lblStreet.Enabled = false; lblDiffStreetReq.Visible = false; }
                            break;
                        case Consts.CASE2001.MailSf:
                            if (enabled) { txtMailSuffix.Enabled = lblSuffix.Enabled = true; if (required) lblSuffixReq.Visible = true; } else { txtMailSuffix.Enabled = lblSuffix.Enabled = false; lblSuffixReq.Visible = false; }
                            break;
                        case Consts.CASE2001.MailFlr:
                            if (enabled) { txtMailFloor.Enabled = lblFloor.Enabled = true; if (required) lblFloorReq.Visible = true; } else { txtMailFloor.Enabled = lblFloor.Enabled = false; lblFloorReq.Visible = false; }
                            break;
                        case Consts.CASE2001.MailCity:
                            if (enabled) { txtCityName.Enabled = lblCityName.Enabled = true; if (required) lblDiffCityReq.Visible = true; } else { txtCityName.Enabled = lblCityName.Enabled = false; lblDiffCityReq.Visible = false; }
                            break;
                        case Consts.CASE2001.MailApt:
                            if (enabled) { txtMailApartment.Enabled = lblApartment.Enabled = true; if (required) lblApartmentReq.Visible = true; } else { txtMailApartment.Enabled = lblApartment.Enabled = false; lblApartmentReq.Visible = false; }
                            break;
                        case Consts.CASE2001.MailZip:
                            if (enabled) { txtZip.Enabled = lblZipCode.Enabled = txtZipPlus.Enabled = true; if (required) lblZipCodeReq.Visible = true; } else { txtZip.Enabled = txtZipPlus.Enabled = lblZipCode.Enabled = false; lblZipCodeReq.Visible = false; }
                            break;
                        case Consts.CASE2001.MailCounty:
                            if (enabled) { cmbMailCounty.Enabled = lblCounty.Enabled = true; if (required) lblCountyReq.Visible = true; } else { cmbMailCounty.Enabled = lblCounty.Enabled = false; lblCountyReq.Visible = false; }
                            break;
                        case Consts.CASE2001.MailState:
                            if (enabled) { txtMailState.Enabled = lblState.Enabled = true; if (required) lblDiffStateReq.Visible = true; } else { txtMailState.Enabled = lblState.Enabled = false; lblDiffStateReq.Visible = false; }
                            break;
                        case Consts.CASE2001.MailPhone:
                            if (enabled) { txtMailPhone.Enabled = lblPhone.Enabled = true; if (required) lblPhoneReq.Visible = true; } else { txtMailPhone.Enabled = lblPhone.Enabled = false; lblPhoneReq.Visible = false; }
                            break;
                    }
                }
            }
            else
            {
                btnZipSearch.Enabled = false;
                btnSave.Enabled = false;
            }
        }

        private void fillDropdowns()
        {

            List<CommonEntity> Country = _model.lookupDataAccess.GetCountry();
            cmbMailCounty.Items.Insert(0, new ListItem("Select One", "0"));
            cmbMailCounty.ColorMember = "FavoriteColor";
            cmbMailCounty.SelectedIndex = 0;
            foreach (CommonEntity country in Country)
            {
                ListItem li = new ListItem(country.Desc, country.Code, country.Active, country.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbMailCounty.Items.Add(li);
                if (country.Default.Equals("Y")) cmbMailCounty.SelectedItem = li;

            }

        }

        private void fillForm()
        {
            CaseDiffEntity caseDiffDetails = _model.CaseMstData.GetCaseDiffadpya(MainMenuAgency.ToString(), MainMenuDept.ToString(), MainMenuProgram.ToString(), MainMenuYear.ToString(), ApplicationNo.ToString(), string.Empty);
            if (caseDiffDetails != null)
            {

                txtCityName.Text = caseDiffDetails.City;
                txtHouseNo.Text = caseDiffDetails.Hn;
                txtLast.Text = caseDiffDetails.IncareLast;
                txtFirst.Text = caseDiffDetails.IncareFirst;
                txtMailApartment.Text = caseDiffDetails.Apt;
                txtMailFloor.Text = caseDiffDetails.Flr;
                txtMailPhone.Text = caseDiffDetails.Phone;
                txtMailState.Text = caseDiffDetails.State;
                txtMailStreet.Text = caseDiffDetails.Street;
                txtMailSuffix.Text = caseDiffDetails.Suffix;
                txtZip.Text = caseDiffDetails.Zip;
                txtZipPlus.Text = caseDiffDetails.ZipPlus;
                if (caseDiffDetails.Zip != string.Empty)
                    txtZip.Text = "00000".Substring(0, 5 - caseDiffDetails.Zip.Length) + caseDiffDetails.Zip;
                if (caseDiffDetails.ZipPlus != string.Empty)
                    txtZipPlus.Text = "0000".Substring(0, 4 - caseDiffDetails.ZipPlus.Length) + caseDiffDetails.ZipPlus;
                if (caseDiffDetails.County != string.Empty)
                {
                    CommonFunctions.SetComboBoxValue(cmbMailCounty, caseDiffDetails.County);
                    caseDiffDetails.CountyDesc = ((ListItem)cmbMailCounty.SelectedItem).Text.ToString();
                }
                else
                    cmbMailCounty.SelectedIndex = 0;
                caseDiffMainHist = caseDiffDetails;
            }
            else
            {
                if (Mode.Equals(Consts.Common.Add))
                {
                    if (string.IsNullOrEmpty(FormType))
                    {
                        if (caseDiffMailAddressData != null)
                        {
                            txtCityName.Text = caseDiffMailAddressData.City;
                            txtHouseNo.Text = caseDiffMailAddressData.Hn;
                            txtLast.Text = caseDiffMailAddressData.IncareLast;
                            txtFirst.Text = caseDiffMailAddressData.IncareFirst;
                            txtMailApartment.Text = caseDiffMailAddressData.Apt;
                            txtMailFloor.Text = caseDiffMailAddressData.Flr;
                            if (caseDiffMailAddressData.Phone != string.Empty)
                            {
                                txtMailPhone.Text = caseDiffMailAddressData.Phone;
                            }
                            txtMailState.Text = caseDiffMailAddressData.State;
                            txtMailStreet.Text = caseDiffMailAddressData.Street;
                            txtMailSuffix.Text = caseDiffMailAddressData.Suffix;
                            txtZip.Text = caseDiffMailAddressData.Zip;
                            txtZipPlus.Text = caseDiffMailAddressData.ZipPlus;
                            txtZip.Text = "00000".Substring(0, 5 - caseDiffMailAddressData.Zip.Length) + caseDiffMailAddressData.Zip;
                            txtZipPlus.Text = "0000".Substring(0, 4 - caseDiffMailAddressData.ZipPlus.Length) + caseDiffMailAddressData.ZipPlus;
                            if (caseDiffMailAddressData.County != string.Empty)
                            {
                                CommonFunctions.SetComboBoxValue(cmbMailCounty, caseDiffMailAddressData.County);
                                caseDiffDetails.CountyDesc = ((ListItem)cmbMailCounty.SelectedItem).Text.ToString();
                            }
                            else
                                cmbMailCounty.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        private void fillLandlordForm()
        {
            CaseDiffEntity caseDiffDetails = _model.CaseMstData.GetLandlordadpya(MainMenuAgency.ToString(), MainMenuDept.ToString(), MainMenuProgram.ToString(), MainMenuYear.ToString(), ApplicationNo.ToString(), "Landlord");
            if (caseDiffDetails != null)
            {

                txtCityName.Text = caseDiffDetails.City;
                txtHouseNo.Text = caseDiffDetails.Hn;
                txtLast.Text = caseDiffDetails.IncareLast;
                txtFirst.Text = caseDiffDetails.IncareFirst;
                txtMailApartment.Text = caseDiffDetails.Apt;
                txtMailFloor.Text = caseDiffDetails.Flr;
                if (caseDiffDetails.Phone != string.Empty)
                {
                    txtMailPhone.Text = caseDiffDetails.Phone;
                }
                txtMailState.Text = caseDiffDetails.State;
                txtMailStreet.Text = caseDiffDetails.Street;
                txtMailSuffix.Text = caseDiffDetails.Suffix;
                txtZip.Text = caseDiffDetails.Zip;
                txtZipPlus.Text = caseDiffDetails.ZipPlus;
                txtZip.Text = "00000".Substring(0, 5 - caseDiffDetails.Zip.Length) + caseDiffDetails.Zip;
                txtZipPlus.Text = "0000".Substring(0, 4 - caseDiffDetails.ZipPlus.Length) + caseDiffDetails.ZipPlus;
                if (caseDiffDetails.County != string.Empty)
                {
                    CommonFunctions.SetComboBoxValue(cmbMailCounty, caseDiffDetails.County);
                    caseDiffDetails.CountyDesc = ((ListItem)cmbMailCounty.SelectedItem).Text.ToString();
                }
                else
                    cmbMailCounty.SelectedIndex = 0;
                caseDiffMainHist = caseDiffDetails;
            }
            else
            {
                if (Mode.Equals(Consts.Common.Add))
                {
                    if (FormType != string.Empty)
                    {
                        if (caseDiffLLData != null)
                        {
                            txtCityName.Text = caseDiffLLData.City;
                            txtHouseNo.Text = caseDiffLLData.Hn;
                            txtLast.Text = caseDiffLLData.IncareLast;
                            txtFirst.Text = caseDiffLLData.IncareFirst;
                            txtMailApartment.Text = caseDiffLLData.Apt;
                            txtMailFloor.Text = caseDiffLLData.Flr;
                            if (caseDiffLLData.Phone != string.Empty)
                            {
                                txtMailPhone.Text = caseDiffLLData.Phone;
                            }
                            txtMailState.Text = caseDiffLLData.State;
                            txtMailStreet.Text = caseDiffLLData.Street;
                            txtMailSuffix.Text = caseDiffLLData.Suffix;
                            txtZip.Text = caseDiffLLData.Zip;
                            txtZipPlus.Text = caseDiffLLData.ZipPlus;
                            txtZip.Text = "00000".Substring(0, 5 - caseDiffLLData.Zip.Length) + caseDiffLLData.Zip;
                            txtZipPlus.Text = "0000".Substring(0, 4 - caseDiffLLData.ZipPlus.Length) + caseDiffLLData.ZipPlus;
                            if (caseDiffLLData.County != string.Empty)
                            {
                                CommonFunctions.SetComboBoxValue(cmbMailCounty, caseDiffLLData.County);
                                caseDiffDetails.CountyDesc = ((ListItem)cmbMailCounty.SelectedItem).Text.ToString();
                            }
                            else
                                cmbMailCounty.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        private void DiffMailForm_Load(object sender, EventArgs e)
        {
            txtFirst.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                CaseDiffEntity caseDiffEntity = new CaseDiffEntity();
                caseDiffEntity.Agency = MainMenuAgency;
                caseDiffEntity.Dept = MainMenuDept;
                caseDiffEntity.Program = MainMenuProgram;
                caseDiffEntity.Year = MainMenuYear;
                caseDiffEntity.AppNo = ApplicationNo;
                caseDiffEntity.Apt = txtMailApartment.Text;
                caseDiffEntity.City = txtCityName.Text;
                if (!((ListItem)cmbMailCounty.SelectedItem).Value.ToString().Equals("0"))
                    caseDiffEntity.County = ((ListItem)cmbMailCounty.SelectedItem).Value.ToString();
                if (caseDiffEntity.County != string.Empty)
                    caseDiffEntity.CountyDesc = ((ListItem)cmbMailCounty.SelectedItem).Text.ToString();
                // caseDiffEntity.Direction = txt
                caseDiffEntity.Flr = txtMailFloor.Text;
                caseDiffEntity.Hn = txtHouseNo.Text;
                caseDiffEntity.IncareFirst = txtFirst.Text;
                caseDiffEntity.IncareLast = txtLast.Text;
                caseDiffEntity.Phone = txtMailPhone.Text;
                caseDiffEntity.State = txtMailState.Text;
                caseDiffEntity.Street = txtMailStreet.Text;
                caseDiffEntity.Suffix = txtMailSuffix.Text;

                caseDiffEntity.Zip = txtZip.Text;
                caseDiffEntity.ZipPlus = txtZipPlus.Text;
                caseDiffEntity.AddOperator = BaseForm.UserID;
                caseDiffEntity.LstcOperator = BaseForm.UserID;
                if (string.IsNullOrEmpty(FormType))
                {
                    //if (_model.CaseMstData.InsertUpdateDelCaseDiff(caseDiffEntity))
                    //{
                    //    if (caseDiffMainHist != null)
                    //    {
                    //        CheckHistoryTableData(caseDiffEntity, caseDiffMainHist, "Mailing Address");
                    //    }
                    //    this.Close();
                    //}

                    if (Mode.Equals(Consts.Common.Edit))
                    {
                        if (_model.CaseMstData.InsertUpdateDelCaseDiff(caseDiffEntity))
                        {
                            if (caseDiffMainHist != null)
                            {
                                CheckHistoryTableData(caseDiffEntity, caseDiffMainHist, "Mailing Address");
                            }
                            this.Close();
                            this.Close();
                        }
                    }
                    else
                    {
                        caseDiffMailAddressData = caseDiffEntity;
                        this.DialogResult = Wisej.Web.DialogResult.OK;
                        this.Close();
                    }

                }
                else
                {
                    if (Mode.Equals(Consts.Common.Edit))
                    {
                        if (_model.CaseMstData.InsertUpdateDelLandlord(caseDiffEntity))
                        {
                            if (caseDiffMainHist != null)
                            {
                                CheckHistoryTableData(caseDiffEntity, caseDiffMainHist, "Landlord Info");
                            }
                            this.Close();
                            this.Close();
                        }
                    }
                    else
                    {
                        caseDiffLLData = caseDiffEntity;
                        this.DialogResult = Wisej.Web.DialogResult.OK;
                        this.Close();
                    }
                }
            }
        }



        private bool ValidateForm()
        {
            bool isValid = true;

            if (lblFirstReq.Visible && String.IsNullOrEmpty(txtFirst.Text.Trim()))
            {
                _errorProvider.SetError(txtFirst, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFirst.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtFirst, null);
            }
            if (lblLastReq.Visible && String.IsNullOrEmpty(txtLast.Text.Trim()))
            {
                _errorProvider.SetError(txtLast, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblLast.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtLast, null);
            }
            if (lblDiffHouseNoReq.Visible && String.IsNullOrEmpty(txtHouseNo.Text.Trim()))
            {
                _errorProvider.SetError(txtHouseNo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHouseNo.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtHouseNo, null);
            }
            if (lblDiffStreetReq.Visible && String.IsNullOrEmpty(txtMailStreet.Text.Trim()))
            {
                _errorProvider.SetError(txtMailStreet, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblStreet.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtMailStreet, null);
            }
            if (lblSuffixReq.Visible && String.IsNullOrEmpty(txtMailSuffix.Text.Trim()))
            {
                _errorProvider.SetError(txtMailSuffix, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSuffix.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtMailSuffix, null);
            }
            if (lblDiffCityReq.Visible && String.IsNullOrEmpty(txtCityName.Text.Trim()))
            {
                _errorProvider.SetError(txtCityName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCityName.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtCityName, null);
            }
            if (lblDiffStateReq.Visible && String.IsNullOrEmpty(txtMailState.Text.Trim()))
            {
                _errorProvider.SetError(txtMailState, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblState.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtMailState, null);
            }

            if (lblZipCodeReq.Visible && String.IsNullOrEmpty(txtZip.Text.Trim()))
            {
                _errorProvider.SetError(btnZipSearch, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblZipCode.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(btnZipSearch, null);
            }
            if (lblFloorReq.Visible && String.IsNullOrEmpty(txtMailFloor.Text.Trim()))
            {
                _errorProvider.SetError(txtMailFloor, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFloor.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtMailFloor, null);
            }
            if (lblApartmentReq.Visible && String.IsNullOrEmpty(txtMailApartment.Text.Trim()))
            {
                _errorProvider.SetError(txtMailApartment, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblApartment.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtMailApartment, null);
            }
            if (lblPhoneReq.Visible && String.IsNullOrEmpty(txtMailPhone.Text.Trim()))
            {
                _errorProvider.SetError(txtMailPhone, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPhone.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtMailPhone, null);
            }
            if (lblCountyReq.Visible && (cmbMailCounty.SelectedItem == null || ((ListItem)cmbMailCounty.SelectedItem).Text == Consts.Common.SelectOne))
            {
                _errorProvider.SetError(cmbMailCounty, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCounty.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbMailCounty, null);
            }

            return (isValid);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnZipSearch_Click(object sender, EventArgs e)
        {
            ZipCodeSearchForm zipCodeSearchForm = new ZipCodeSearchForm(privilege);
            zipCodeSearchForm.FormClosed += new FormClosedEventHandler(OnZipCodeFormClosed);
            zipCodeSearchForm.StartPosition = FormStartPosition.CenterScreen;
            zipCodeSearchForm.ShowDialog();
        }

        private void OnZipCodeFormClosed(object sender, FormClosedEventArgs e)
        {

            ZipCodeSearchForm form = sender as ZipCodeSearchForm;
            if (form.DialogResult == DialogResult.OK)
            {
                ZipCodeEntity zipcodedetais = form.GetSelectedZipCodedetails();
                if (zipcodedetais != null)
                {
                    string zipPlus = zipcodedetais.Zcrplus4;
                    txtZipPlus.Text = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                    txtZip.Text = "00000".Substring(0, 5 - zipcodedetais.Zcrzip.Length) + zipcodedetais.Zcrzip;
                    txtMailState.Text = zipcodedetais.Zcrstate;
                    txtCityName.Text = zipcodedetais.Zcrcity;
                    if (string.IsNullOrEmpty(zipcodedetais.Zcrcountry))
                        zipcodedetais.Zcrcountry = "0";
                    CommonFunctions.SetComboBoxValue(cmbMailCounty, zipcodedetais.Zcrcountry);

                }
            }
        }

        private void CheckHistoryTableData(CaseDiffEntity caseHistDiff, CaseDiffEntity caseDiffMainHist, string strSubScr)
        {
            string strHistoryDetails = "<XmlHistory>";
            bool boolHistory = false;




            if (caseDiffMainHist.State.Trim() != caseHistDiff.State.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>State</FieldName><OldValue>" + caseDiffMainHist.State + "</OldValue><NewValue>" + caseHistDiff.State + "</NewValue></HistoryFields>";
            }
            if (caseDiffMainHist.City.Trim() != caseHistDiff.City.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>City Name</FieldName><OldValue>" + caseDiffMainHist.City + "</OldValue><NewValue>" + caseHistDiff.City + "</NewValue></HistoryFields>";
            }
            if (caseDiffMainHist.Street.Trim() != caseHistDiff.Street.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Street</FieldName><OldValue>" + caseDiffMainHist.Street + "</OldValue><NewValue>" + caseHistDiff.Street + "</NewValue></HistoryFields>";
            }
            if (caseDiffMainHist.Suffix.Trim() != caseHistDiff.Suffix.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Suffix</FieldName><OldValue>" + caseDiffMainHist.Suffix + "</OldValue><NewValue>" + caseHistDiff.Suffix + "</NewValue></HistoryFields>";
            }
            if (caseDiffMainHist.Hn.Trim() != caseHistDiff.Hn.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>House No</FieldName><OldValue>" + caseDiffMainHist.Hn + "</OldValue><NewValue>" + caseHistDiff.Hn + "</NewValue></HistoryFields>";
            }
            //if (caseDiffMainHist.Direction.Trim() != caseHistDiff.Direction.Trim())
            //    {
            //        boolHistory = true;
            //        strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Direction</FieldName><OldValue>" + caseDiffMainHist.Direction + "</OldValue><NewValue>" + caseHistDiff.Direction + "</NewValue></HistoryFields>";
            //    }
            if (caseDiffMainHist.Apt.Trim() != caseHistDiff.Apt.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Apartment</FieldName><OldValue>" + caseDiffMainHist.Apt + "</OldValue><NewValue>" + caseHistDiff.Apt + "</NewValue></HistoryFields>";
            }
            if (caseDiffMainHist.Flr.Trim() != caseHistDiff.Flr.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Floor</FieldName><OldValue>" + caseDiffMainHist.Flr + "</OldValue><NewValue>" + caseHistDiff.Flr + "</NewValue></HistoryFields>";
            }
            if ("00000".Substring(0, 5 - caseDiffMainHist.Zip.Length) + caseDiffMainHist.Zip != caseHistDiff.Zip.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Zip Code</FieldName><OldValue>" + caseDiffMainHist.Zip + "</OldValue><NewValue>" + caseHistDiff.Zip + "</NewValue></HistoryFields>";
            }
            if ("0000".Substring(0, 4 - caseDiffMainHist.ZipPlus.Length) + caseDiffMainHist.ZipPlus != caseHistDiff.ZipPlus.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Zip Plus</FieldName><OldValue>" + caseDiffMainHist.ZipPlus + "</OldValue><NewValue>" + caseHistDiff.ZipPlus + "</NewValue></HistoryFields>";
            }
            if (caseDiffMainHist.County.Trim() != caseHistDiff.County.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>County</FieldName><OldValue>" + caseDiffMainHist.CountyDesc + "</OldValue><NewValue>" + caseHistDiff.CountyDesc + "</NewValue></HistoryFields>";
            }
            if (caseDiffMainHist.IncareFirst.Trim() != caseHistDiff.IncareFirst.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>In Care of First</FieldName><OldValue>" + caseDiffMainHist.IncareFirst + "</OldValue><NewValue>" + caseHistDiff.IncareFirst + "</NewValue></HistoryFields>";
            }
            if (caseDiffMainHist.IncareLast.Trim() != caseHistDiff.IncareLast.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>In Care of Last</FieldName><OldValue>" + caseDiffMainHist.IncareLast + "</OldValue><NewValue>" + caseHistDiff.IncareLast + "</NewValue></HistoryFields>";
            }
            if (caseDiffMainHist.Phone.Trim() != caseHistDiff.Phone.Trim())
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Phone</FieldName><OldValue>" + caseDiffMainHist.Phone + "</OldValue><NewValue>" + caseHistDiff.Phone + "</NewValue></HistoryFields>";
            }




            strHistoryDetails = strHistoryDetails + "</XmlHistory>";
            if (boolHistory)
            {
                CaseHistEntity caseHistEntity = new CaseHistEntity();
                caseHistEntity.HistTblName = "CASEMST";
                caseHistEntity.HistScreen = "CASE2001";
                caseHistEntity.HistSubScr = strSubScr;
                caseHistEntity.HistTblKey = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo + BaseForm.BaseCaseMstListEntity[0].FamilySeq;
                caseHistEntity.LstcOperator = BaseForm.UserID;
                caseHistEntity.HistChanges = strHistoryDetails;
                _model.CaseMstData.InsertCaseHist(caseHistEntity);
            }


        }
    }
}