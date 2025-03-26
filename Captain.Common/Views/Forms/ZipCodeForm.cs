#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Wisej.Web;
//using Gizmox.WebGUI.Common;
//using Wisej.Web;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Menus;
using Captain.Common.Views.Forms;
using System.Data.SqlClient;
using Captain.Common.Views.Controls;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Views.UserControls;
using Captain.Common.Views.Controls.Compatibility;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class ZipCodeForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;
        PrivilegeEntity _privilegeEntity = null;
        #endregion
        public ZipCodeForm(BaseForm baseForm, string mode, string strZcrZip, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();
            BaseForm = baseForm;
            _privilegeEntity = privilegeEntity;
            Mode = mode;
            SelectZcrZipId = strZcrZip;
            //lblHeader.Text = privilegeEntity.PrivilegeName;
            this.Text = "ZIP Code File Maintenance" + " - " + Consts.Common.Add;
            _model = new CaptainModel();
            fillGridanddropdown();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            txtIntakecode.Validator = TextBoxValidation.IntegerValidator;
            txtZipCode.Validator = TextBoxValidation.IntegerValidator;
            txtZipCode1.Validator = TextBoxValidation.IntegerValidator;
            //txtCity.Validator = new TextBoxValidation("String(value).match(/^[-]{0,1}[0-9]*([.][0-9]+){0,1}$/)", "Invalid value","-0-9.");
            if (Mode.Equals("Edit"))
            {
                fillZipCodeForm();
                this.Text = "ZIP Code File Maintenance" + " - " + Consts.Common.Edit;
            }

        }

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string SelectZcrZipId { get; set; }

        public bool IsSaveValid { get; set; }

        private void ZipCodeForm_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(_privilegeEntity.Program, 1, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        private bool isZipCodeExists(string zipCode, string zipplus)
        {
            bool isExists = false;
            if (Mode.Equals("Add"))
            {
                zipplus = zipplus.TrimStart('0');
                if (zipplus == string.Empty)
                    zipplus = "0";
                ZipCodeEntity zipcodeEntity = _model.ZipCodeAndAgency.GetZipcodeByID(zipCode.TrimStart('0'), zipplus);
                if (zipcodeEntity != null)
                {
                    isExists = true;
                }
            }
            return isExists;
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(txtZipCode.Text))
            {
                _errorProvider.SetError(txtZipCode, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblZipCode.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                if (isZipCodeExists(txtZipCode.Text, txtZipCode1.Text))
                {
                    _errorProvider.SetError(txtZipCode, string.Format(Consts.Messages.AlreadyExists.GetMessage(), lblZipCode.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtZipCode, null);
                }
            }

            if (String.IsNullOrEmpty(txtCity.Text.Trim()))
            {
                _errorProvider.SetError(txtCity, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCity.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtCity, null);
            }

            if (String.IsNullOrEmpty(txtState.Text.Trim()))
            {

                _errorProvider.SetError(txtState, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblState.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;

            }
            else
            {
                if (txtState.TextLength < 2)
                {
                  AlertBox.Show(Consts.Messages.PleaseEnterTwoCharacters, MessageBoxIcon.Warning);
                    //_errorProvider.SetError(txtState, Consts.Messages.PleaseEnterTwoCharacters.GetMessage());
                    isValid = false;
                    //_errorProvider.SetError(txtState, string.Format(Consts.Messages.PleaseEnterTwoCharacters.GetMessage(), lblState.Text.Replace(Consts.Common.Colon, string.Empty)));
                    //isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtState, null);
                }

            }


            IsSaveValid = isValid;
            return (isValid);
        }

        public void fillGridanddropdown()
        {
            fillDropdowns();
            DataSet ds = Captain.DatabaseLayer.Lookups.GetModules();
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                int rowIndex = grdApplications.Rows.Add(false, dr["APPL_CODE"].ToString(), dr["APPL_DESCRIPTION"].ToString());
                grdApplications.Rows[rowIndex].Tag = dr;
            }


            List<CommonEntity> Township = _model.lookupDataAccess.GetTownship();
            foreach (CommonEntity township in Township)
            {
                cmbTownship.Items.Add(new ListItem(township.Desc, township.Code));
            }
            cmbTownship.Items.Insert(0, new ListItem("   None   ", "0"));
            cmbTownship.SelectedIndex = 0;

            List<CommonEntity> Country = _model.ZipCodeAndAgency.GetCounty();
            foreach (CommonEntity country in Country)
            {
                cmbCountry.Items.Add(new ListItem(country.Desc, country.Code));
            }
            cmbCountry.Items.Insert(0, new ListItem("Select One", "0"));
            cmbCountry.SelectedIndex = 0;

        }

        private void fillDropdowns()
        {
            List<CommonEntity> commonEntity = _model.lookupDataAccess.GetMonths();
            foreach (CommonEntity month in commonEntity)
            {

                month.Code = "00".Substring(0, 2 - month.Code.Length) + month.Code;
                cmbMonth.Items.Add(new ListItem(month.Desc, month.Code));
            }
            cmbMonth.Items.Insert(0, new ListItem("    ", "0"));
            cmbMonth.SelectedIndex = 0;

            cmbDay.Items.Insert(0, new ListItem("    ", "0"));
            cmbDay.SelectedIndex = 0;

            cmbYear.Items.Insert(0, new ListItem("    ", "0"));
            cmbYear.Items.Insert(1, new ListItem("This Program Year", "01"));
            cmbYear.Items.Insert(2, new ListItem("Next Year", "02"));
            cmbYear.SelectedIndex = 0;
        }

        private void fillZipCodeForm()
        {
            if (SelectZcrZipId != null)
            {
                string strZipCode = SelectZcrZipId.Substring(0, 5).ToString();
                string strZipPlus = SelectZcrZipId.Substring(6, 4).ToString().TrimStart('0');
                if (strZipPlus == string.Empty)
                    strZipPlus = "0";
                ZipCodeEntity zipcodeList = _model.ZipCodeAndAgency.GetZipcodeByID(strZipCode.TrimStart('0'), strZipPlus);
                if (zipcodeList != null)
                {
                    ZipCodeEntity zipcode = zipcodeList;
                    txtZipCode.Text = SetLeadingZeros(zipcode.Zcrzip);
                    txtZipCode.ReadOnly = true;
                    txtZipCode1.ReadOnly = true;
                    txtZipCode.Enabled = false;
                    txtZipCode1.Enabled = false;
                    string zipPlus = zipcode.Zcrplus4;
                    zipPlus = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                    txtZipCode1.Text = zipPlus;
                    txtState.Text = zipcode.Zcrstate;
                    txtCity.Text = zipcode.Zcrcity.Trim();

                    if (zipcode.InActive.Trim() == "Y")
                        chkbInActive.Checked = true;
                    else chkbInActive.Checked = false;

                    SetComboBoxValue(cmbMonth, zipcode.Zcrhssmo.Trim());
                    SetComboBoxValue(cmbDay, zipcode.Zcrhssday);
                    SetComboBoxValue(cmbYear, zipcode.Zcrhssyear.Trim());
                    txtIntakecode.Text = zipcode.Zcrintakecode.Trim();
                    SetComboBoxValue(cmbTownship, zipcode.Zcrcitycode);
                    SetComboBoxValue(cmbCountry, zipcode.Zcrcountry);
                    applicationCheckEdit(SplitByAppication(zipcode.Zcrapp, 2));

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="value"></param>
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

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //Consts.Messages.UserCreatedSuccesssfully.DisplayFirendlyMessage(Captain.Common.Exceptions.ExceptionSeverityLevel.Information);
            if (ValidateForm())
            {
                //Add ZipCode
                CaptainModel model = new CaptainModel();
                ZipCodeEntity zipCodeDetails = new ZipCodeEntity();
                zipCodeDetails.Zcrzip = txtZipCode.Text;
                if (txtZipCode1.Text != string.Empty)
                {
                    zipCodeDetails.Zcrplus4 = txtZipCode1.Text;
                }
                else
                {
                    zipCodeDetails.Zcrplus4 = "0000";
                }
                zipCodeDetails.Zcrcity = txtCity.Text;
                zipCodeDetails.Zcrstate = txtState.Text;
                zipCodeDetails.Zcrintakecode = txtIntakecode.Text;
                if (!((ListItem)cmbTownship.SelectedItem).Value.ToString().Equals("0"))
                {
                    zipCodeDetails.Zcrcitycode = ((ListItem)cmbTownship.SelectedItem).Value.ToString();
                }
                if (!((ListItem)cmbCountry.SelectedItem).Value.ToString().Equals("0"))
                {
                    zipCodeDetails.Zcrcountry = ((ListItem)cmbCountry.SelectedItem).Value.ToString();
                }

                if (!((ListItem)cmbMonth.SelectedItem).Value.ToString().Equals("0"))
                {
                    zipCodeDetails.Zcrhssmo = ((ListItem)cmbMonth.SelectedItem).Value.ToString();
                }
                if (!((ListItem)cmbDay.SelectedItem).Value.ToString().Equals("0"))
                {
                    zipCodeDetails.Zcrhssday = ((ListItem)cmbDay.SelectedItem).Value.ToString();
                }
                if (!((ListItem)cmbYear.SelectedItem).Value.ToString().Equals("0"))
                {
                    zipCodeDetails.Zcrhssyear = ((ListItem)cmbYear.SelectedItem).Value.ToString();
                }

                if(chkbInActive.Checked)
                {
                    zipCodeDetails.InActive = "Y";
                }
                else
                    zipCodeDetails.InActive = "N";

                // sqlParamList.Add(new SqlParameter("@ZCR_INTAKE_CODE", txtin.Text));
                // zipCodeDetails.Zcrdate = txtHeadStartDt.Text;
                zipCodeDetails.Zcrlstcoperator = BaseForm.UserID; //"SYSTEM";//Captain<string>.Session[Consts.SessionVariables.UserName].ToString();
                zipCodeDetails.Zcraddoperator = BaseForm.UserID;// "SYSTEM";//Captain<string>.Session[Consts.SessionVariables.UserName].ToString();
                zipCodeDetails.Zcrapp = checkgvwApplicationData();
                bool boolsucess = false;
                if (Mode.Equals("Edit"))
                {
                    zipCodeDetails.Mode = "Edit";
                    boolsucess = model.ZipCodeAndAgency.InsertUpdateDelZIPCODE(zipCodeDetails);
                    AlertBox.Show("Updated Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                    //Consts.Messages.UserCreatedSuccesssfully.DisplayFirendlyMessage(Captain.Common.Exceptions.ExceptionSeverityLevel.Information);
                }
                else
                {
                    zipCodeDetails.Mode = "Add";
                    boolsucess = model.ZipCodeAndAgency.InsertUpdateDelZIPCODE(zipCodeDetails);
                    AlertBox.Show("Saved Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                    //Consts.Messages.UserCreatedSuccesssfully.DisplayFirendlyMessage(Captain.Common.Exceptions.ExceptionSeverityLevel.Information);
                }
                if (boolsucess)
                {
                    ADMN0013 zipControl = BaseForm.GetBaseUserControl() as ADMN0013;
                    if (zipControl != null)
                    {
                        if (Mode.Equals("Edit"))
                        {
                            zipControl.RefreshGrid();
                        }
                        else
                        {
                            zipControl.RefreshGrid(txtZipCode.Text);
                        }
                    }
                    this.Close();
                }
                this.Close();

            }
        }

        public void applicationCheckEdit(string[] strApplication)
        {
            foreach (DataGridViewRow row in grdApplications.Rows)
            {
                if (row.Cells["Appcode"].Value != null)
                {
                    for (int i = 0; i <= strApplication.Length - 1; i++)
                    {
                        if (Convert.ToString(row.Cells["Appcode"].Value) == strApplication[i].ToString())
                        {
                            row.Cells["chkAppcode"].Value = true;
                        }
                    }

                }
            }
        }

        public string checkgvwApplicationData()
        {
            string strdata = string.Empty;

            foreach (DataGridViewRow row in grdApplications.Rows)
            {
                if (row.Cells["chkAppcode"].Value != null && Convert.ToBoolean(row.Cells["chkAppcode"].Value) == true)
                {
                    strdata = strdata + row.Cells["Appcode"].Value.ToString();
                }
            }

            return strdata;
        }

        private string[] SplitByAppication(string s, int split)
        {
            //Like using List because I can just add to it 
            List<string> list = new List<string>();
            // Integer Division
            int TimesThroughTheLoop = s.Length / split;
            for (int i = 0; i < TimesThroughTheLoop; i++)
            {
                list.Add(s.Substring(i * split, split));
            }
            // Pickup the end of the string
            if (TimesThroughTheLoop * split != s.Length)
            {
                list.Add(s.Substring(TimesThroughTheLoop * split));
            }
            return list.ToArray();
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            int intDays = 0;
            if (!((ListItem)cmbMonth.SelectedItem).Value.ToString().Equals("0"))
            {
                string strMonth = ((ListItem)cmbMonth.SelectedItem).Value.ToString();
                if (strMonth == "01" || strMonth == "03" || strMonth == "05" || strMonth == "07" || strMonth == "08" || strMonth == "10" || strMonth == "12" || strMonth == "1")
                {
                    intDays = 31;
                }
                else if (strMonth == "02")
                {
                    intDays = 29;
                }
                else
                {
                    intDays = 30;
                }
                cmbDay.Items.Clear();
                for (int i = 1; i <= intDays; i++)
                {
                    string strdays = i.ToString();
                    strdays = "00".Substring(0, 2 - strdays.Length) + strdays;
                    cmbDay.Items.Add(new ListItem(strdays, strdays));
                }
                cmbDay.Items.Insert(0, new ListItem("    ", "0"));
                cmbDay.SelectedIndex = 0;
            }
            else
            {
                cmbDay.Items.Clear();
                cmbDay.Items.Insert(0, new ListItem("    ", "0"));
                cmbDay.SelectedIndex = 0;
            }
        }

        private void txtZipCode_Leave(object sender, EventArgs e)
        {
            string strZipCode = txtZipCode.Text;
            strZipCode = strZipCode.TrimStart('0');
            txtZipCode.Text = SetLeadingZeros(txtZipCode.Text);
            txtZipCode1.Text = "";
            txtZipCode1.Focus();
        }

        private string SetLeadingZeros(string TmpSeq)
        {
            int Seq_len = TmpSeq.Trim().Length;
            string TmpCode = null;
            TmpCode = TmpSeq.ToString().Trim();
            switch (Seq_len)
            {
                case 4: TmpCode = "0" + TmpCode; break;
                case 3: TmpCode = "00" + TmpCode; break;
                case 2: TmpCode = "000" + TmpCode; break;
                case 1: TmpCode = "0000" + TmpCode; break;
                //default: MessageBox.Show("Table Code should not be blank", "CAP Systems", MessageBoxButtons.OK);  TxtCode.Focus();
                //    break;
            }

            return (TmpCode);
        }

        private void txtZipCode1_Leave(object sender, EventArgs e)
        {
            string zipPlus = txtZipCode1.Text;
            txtZipCode1.Text = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
        }

        private void OnHelpClick(object sender, EventArgs e)
        {
           // Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "ADMN00013_Add");
        }

        private void ZipCodeForm_Load(object sender, EventArgs e)
        {
            if (Mode.Equals("Edit"))
            {
                txtIntakecode.Focus();
            }
            else
            {
                txtZipCode.Focus();
            }
        }
    }
}