#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;


using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Menus;
using Captain.Common.Views.Forms;
using System.Data.SqlClient;
using Captain.Common.Views.Controls;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using System.Text.RegularExpressions;
using Captain.Common.Views.UserControls;

using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class Case2011Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private bool boolChangeStatus = false;

        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;

        #endregion

        public Case2011Form(BaseForm baseform,string mode,string code, PrivilegeEntity priviliges)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();

            BaseForm = baseform;
            Mode = mode;
            Code = code;
            Privileges = priviliges;
            FillCountyCombo();
            if (Mode == "Add")
            {
                //LblHeader.Text=
                txtCode.Text = "New";
                txtCode.Enabled = false;
            }
            else if(Mode=="Edit")
            {
                txtCode.Enabled = false;
                FillControls();
                Get_Serviecesfor_Referal();
            }
            else if (Mode == "View")
            {
                txtCode.Enabled = false;
                FillControls();
                Get_Serviecesfor_Referal();
                FillEnabledDisabledFields();
            }
            //this.Text = priviliges.Program + " -" + Mode;
            this.Text ="Agency Referral Database" + " - " + Mode;
           //lblHeader.Text = "Agency Referral Database Form";
            txtZip.Validator = Views.Controls.Compatibility.TextBoxValidation.IntegerValidator;
            txtZipPlus.Validator = TextBoxValidation.IntegerValidator;
            //txtLongDist.Validator = TextBoxValidation.IntegerValidator;
            //  LblHeader.Text = Privileges.PrivilegeName;

        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string Code { get; set; }

        //public string CaMs_Code { get; set; }

        //public string button_Mode { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public bool IsSaveValid { get; set; }

        #endregion

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Application.Navigate("https://app.gitbook.com/s/XYNrNPcoD8nZpAG9fUkf/system-administration-module/screens/agency-referral-database", target: "_blank");
            // Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "CASE2011");
        }

        private void FillCountyCombo()
        {
            List<CommonEntity> County = _model.ZipCodeAndAgency.GetCounty();
            foreach (CommonEntity county in County)
            {
                cmbCounty.Items.Add(new ListItem(county.Desc, county.Code));
            }
            cmbCounty.Items.Insert(0, new ListItem("Select One", "0"));
            cmbCounty.SelectedIndex = 0;
        }

        private void FillControls()
        {
            List<CASEREFEntity> Reflist;
            CASEREFEntity Search_Entity = new CASEREFEntity(true);
            Search_Entity.Code = Code.Trim();
            Reflist = _model.SPAdminData.Browse_CASEREF(Search_Entity, "Browse");

            if (Reflist.Count > 0)
            {
                CASEREFEntity drRef = Reflist[0];
                txtCode.Text = drRef.Code;
                if (drRef.Active.Equals("Y")) chkbActive.Checked = true;
                else chkbActive.Checked = false;
                txtAgyName.Text = drRef.Name1.Trim();
                txtAgyName2.Text = drRef.Name2.Trim();
                txtIndexby.Text = drRef.IndexBy.Trim();
                txtStreet.Text = drRef.Street.Trim();
                txtCity.Text = drRef.City.Trim();
                txtState.Text = drRef.State.Trim();
                txtZip.Text = SetLeadingZeros(drRef.Zip.Trim());
                txtZipPlus.Text = "0000".Substring(0, 4 - drRef.Zip_Plus.Length)+ drRef.Zip_Plus.Trim();
                txtLongDist.Text = drRef.Long_Distance.Trim();
                //maskPhone.Text = drRef.Area.Trim() + "" + drRef.Excgange.Trim() + "" + drRef.Telno.Trim();
                maskPhone.Text = drRef.Telno.Trim();
                //maskFax.Text = drRef.Fax_Area.Trim() + "" + drRef.Fax_Exchange.Trim() + "" + drRef.Fax_Telno.Trim();
                maskFax.Text = drRef.Fax_Telno.Trim();
                txtFirstName.Text = drRef.Cont_Fname.Trim();
                txtLastName.Text = drRef.Cont_Lname.Trim();
                txtNameIndex.Text = drRef.NameIndex.Trim();
                //maskContPhone.Text = drRef.Cont_Area.Trim() + "" + drRef.Cont_Exchange.Trim() + "" + drRef.Cont_TelNO.Trim();
                maskContPhone.Text = drRef.Cont_TelNO.Trim();
                SetComboBoxValue(cmbCounty, drRef.County.Trim());
                if (drRef.From_Hrs != string.Empty)
                {
                    dateTimePickerfrom.Text = drRef.From_Hrs.ToString();
                    dateTimePickerfrom.Checked = true;
                }
                if (drRef.To_Hrs != string.Empty)
                {
                    dateTimePickerTo.Text = drRef.To_Hrs.ToString();
                    dateTimePickerTo.Checked = true;
                }
                txtSerElig.Text = drRef.Sec.Trim();
                txtEmail.Text = drRef.Email.Trim();
                txtwebAddress.Text = drRef.WebAddress.Trim();

            }
        }

        private void FillEnabledDisabledFields()
        {
            txtCode.Enabled = false;
            chkbActive.Enabled = false;
            chkbActive.Enabled = false;
            txtAgyName.Enabled = false;
            txtAgyName2.Enabled = false;
            txtIndexby.Enabled = false;
            txtStreet.Enabled = false;
            txtCity.Enabled = false;
            txtState.Enabled = false;
            txtZip.Enabled = false;
            txtZipPlus.Enabled = false;
            txtLongDist.Enabled = false;
            maskPhone.Enabled = false;
            maskFax.Enabled = false;
            txtFirstName.Enabled = false;
            txtLastName.Enabled = false;
            txtNameIndex.Enabled = false;
            txtwebAddress.Enabled = false;
            txtEmail.Enabled = false;
            maskContPhone.Enabled = false;
            cmbCounty.Enabled = false;
            dateTimePickerfrom.Enabled = false;
            dateTimePickerfrom.Enabled = false;
            dateTimePickerTo.Enabled = false;
            dateTimePickerTo.Enabled = false;
            txtSerElig.Enabled = false;
            btnSave.Visible = false; btnCancel.Visible = false;
            btnServices.Visible = false;
            pnlServices.Visible = false;
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


        List<CASEREFSEntity> Sel_REFS_List = new List<CASEREFSEntity>();
        List<CASEREFSEntity> Add_Refs_List = new List<CASEREFSEntity>();
        
        string strmsgGrp = string.Empty; string SqlMsg = string.Empty;
        private void btnSave_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(dateTimePickerfrom, null);
            _errorProvider.SetError(dateTimePickerTo, null);
            try
            {
                if (ValidateForm())
                {
                    CASEREFEntity RefEntity = new CASEREFEntity();
                    //RefEntity.Code=
                    CaptainModel model = new CaptainModel();

                    if (Mode == "Edit")
                        RefEntity.Rec_Type = "U";
                    RefEntity.Code = txtCode.Text;
                    if (txtCode.Text == "New")
                    {
                        RefEntity.Rec_Type = "I";
                        RefEntity.Code = "1";
                    }
                    RefEntity.Active = chkbActive.Checked ? "Y" : "N";
                    RefEntity.Name1 = txtAgyName.Text;
                    RefEntity.Name2 = txtAgyName2.Text;
                    RefEntity.IndexBy = txtIndexby.Text;
                    RefEntity.Street = txtStreet.Text;
                    RefEntity.City = txtCity.Text;
                    RefEntity.State = txtState.Text;
                    //string[] lines = Regex.Split(MaskZip.Text, "-");
                    RefEntity.Zip = txtZip.Text;
                    RefEntity.Zip_Plus = txtZipPlus.Text;
                    //string[] phonebreak = Regex.Split(maskPhone.Text, "-");
                    string phonebreak = maskPhone.Text.Replace("-","");
                    phonebreak = phonebreak.Replace(" ", "");
                    //RefEntity.Area = phonebreak[0].Trim();
                    //RefEntity.Excgange = phonebreak[1].Trim();
                    //RefEntity.Telno = phonebreak[2].Trim();
                    RefEntity.Telno = phonebreak.Trim();
                    RefEntity.Long_Distance = txtLongDist.Text;
                    //string[] FaxBreak = Regex.Split(maskFax.Text, "-");
                    string FaxBreak = maskFax.Text.Replace("-","");
                    FaxBreak = FaxBreak.Replace(" ", "");
                    //RefEntity.Fax_Area = FaxBreak[0].Trim();
                    //RefEntity.Fax_Exchange = FaxBreak[1].Trim();
                    //RefEntity.Fax_Telno = FaxBreak[2].Trim();
                    RefEntity.Fax_Telno = FaxBreak.Trim();
                    RefEntity.Cont_Fname = txtFirstName.Text;
                    RefEntity.Cont_Lname = txtLastName.Text;
                    RefEntity.NameIndex = txtNameIndex.Text;
                    //string[] ContTelbreak = Regex.Split(maskContPhone.Text, "-");
                    string ContTelbreak = maskContPhone.Text.Replace("-", "");
                    ContTelbreak = ContTelbreak.Replace(" ", "");
                    //RefEntity.Cont_Area = ContTelbreak[0].Trim();
                    //RefEntity.Cont_Exchange = ContTelbreak[1].Trim();
                    //RefEntity.Cont_TelNO = ContTelbreak[2].Trim();
                    RefEntity.Cont_TelNO = ContTelbreak.Trim();
                    RefEntity.Category = string.Empty;
                    RefEntity.Outside = string.Empty;
                    if (!((ListItem)cmbCounty.SelectedItem).Value.ToString().Equals("0"))
                    {
                        RefEntity.County = ((ListItem)cmbCounty.SelectedItem).Value.ToString();
                    }
                    if (dateTimePickerfrom.Checked)
                        RefEntity.From_Hrs = dateTimePickerfrom.Value.ToString("HH:mm:ss");
                    //RefEntity.From_Hrs = dateTimePickerfrom.Text;
                    if (dateTimePickerTo.Checked)
                        RefEntity.To_Hrs = dateTimePickerTo.Value.ToString("HH:mm:ss"); ;
                    //RefEntity.To_Hrs = dateTimePickerTo.Text;
                    RefEntity.Sec = txtSerElig.Text;
                    RefEntity.Email = txtEmail.Text.Trim();
                    RefEntity.WebAddress = txtwebAddress.Text.Trim();
                    RefEntity.Add_Operator = BaseForm.UserID;
                    RefEntity.Lsct_Operator = BaseForm.UserID;

                    if(_model.SPAdminData.UpdateCASEREF(RefEntity,"UPDATE", out strmsgGrp,out SqlMsg))
                    {
                        if (Mode == "Add")
                        {
                            foreach (CASEREFSEntity entity in Add_Refs_List)
                            {
                                entity.Code = strmsgGrp.Trim();
                                _model.SPAdminData.UpdateCASEREFS(entity, "Update",out SqlMsg);
                            }
                            AlertBox.Show("Saved Successfully");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            foreach (CASEREFSEntity entity in Add_Refs_List)
                            {
                                entity.Code = Code.Trim();
                                _model.SPAdminData.UpdateCASEREFS(entity, "Update", out SqlMsg);
                            }

                            //Toast t = new Toast();
                            //t.Text = "<table><tr> <td><p style='font-size:12px;'> Saved successfully</p></td></tr></table>  ";
                            //t.BackColor = ColorTranslator.FromHtml("#01a601");
                            //t.Alignment = ContentAlignment.MiddleCenter;
                            //t.AllowHtml = true;
                            //t.IconSource = "../Resources/images/rightwhite.png";
                            //t.SetPropertyValue("Width", "250");
                            //t.Show();


                            int _delaycount = 5000;
                            AlertBox.Show("Updated Successfully", autoCloseDelay:_delaycount);
                            
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(txtAgyName.Text) || string.IsNullOrWhiteSpace(txtAgyName.Text))
            {
                _errorProvider.SetError(txtAgyName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAgyName.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtAgyName, null);

            //if (string.IsNullOrEmpty(txtStreet.Text) || string.IsNullOrWhiteSpace(txtStreet.Text))
            //{
            //    _errorProvider.SetError(txtStreet, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblStreet.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //    _errorProvider.SetError(txtStreet, null);

            //if (string.IsNullOrEmpty(txtCity.Text) || string.IsNullOrWhiteSpace(txtCity.Text))
            //{
            //    _errorProvider.SetError(txtCity, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCity.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //    _errorProvider.SetError(txtCity, null);

            //if (string.IsNullOrEmpty(txtState.Text) || string.IsNullOrWhiteSpace(txtState.Text))
            //{
            //    _errorProvider.SetError(txtState, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblState.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //    _errorProvider.SetError(txtState, null);

            //if (string.IsNullOrEmpty(txtZip.Text) || string.IsNullOrWhiteSpace(txtZip.Text))
            //{
            //    _errorProvider.SetError(txtZipPlus, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblZip.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //    _errorProvider.SetError(txtZip, null);

            if (dateTimePickerTo.Checked && !dateTimePickerfrom.Checked)
            {
                    _errorProvider.SetError(dateTimePickerfrom, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Agency Hours From Time".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
            }
            else
            {
                _errorProvider.SetError(dateTimePickerfrom, null);
            }

            if (!dateTimePickerTo.Checked && dateTimePickerfrom.Checked)
            {
                _errorProvider.SetError(dateTimePickerTo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Agency Hours To Time".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dateTimePickerTo, null);
            }

            if (dateTimePickerfrom.Checked.Equals(true) && dateTimePickerTo.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(dateTimePickerfrom.Text))
                {
                    _errorProvider.SetError(dateTimePickerfrom, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Agency Hours From Time".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dateTimePickerfrom, null);
                }
                if (string.IsNullOrWhiteSpace(dateTimePickerTo.Text))
                {
                    _errorProvider.SetError(dateTimePickerTo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Agency Hours To Time".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dateTimePickerTo, null);
                }
            }
            if (dateTimePickerfrom.Checked.Equals(true) && dateTimePickerTo.Checked.Equals(true))
            {
                if (Convert.ToDateTime(dateTimePickerfrom.Text) > Convert.ToDateTime(dateTimePickerTo.Text))
                {
                    _errorProvider.SetError(dateTimePickerfrom, string.Format("'Hours-From' should be Equal or Prior to 'Hours-To'". /*lblFrom.Text.*/Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(dateTimePickerfrom, null);
            }

            System.Text.RegularExpressions.Regex rEMail = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z][\w\.-]{2,28}[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
            if (!string.IsNullOrEmpty(txtEmail.Text.Trim()))
            {
                if (!rEMail.IsMatch(txtEmail.Text.Trim()))
                {
                    _errorProvider.SetError(txtEmail, string.Format("E - Mail expected", lblEmail.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
            }

            if (maskPhone.Text != "" && maskPhone.Text != "   -   -")
            {
                if (maskPhone.Text.Trim().Replace("-", "").Replace(" ", "").Length < 10)
                {
                    _errorProvider.SetError(maskPhone, "Please enter valid Telephone Number");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(maskPhone, null);
                }
            }
            if (maskFax.Text != "" && maskFax.Text != "   -   -")
            {
                if (maskFax.Text.Trim().Replace("-", "").Replace(" ", "").Length < 10)
                {
                    _errorProvider.SetError(maskFax, "Please enter valid Fax Number");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(maskFax, null);
                }
            }
            if (maskContPhone.Text != "" && maskContPhone.Text != "   -   -")
            {
                if (maskContPhone.Text.Trim().Replace("-", "").Replace(" ", "").Length < 10)
                {
                    _errorProvider.SetError(maskContPhone, "Please enter valid Phone Number");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(maskContPhone, null);
                }
            }
            IsSaveValid = isValid;
            return (isValid);
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
            }

            return (TmpCode);
        }


        public string[] GetSelected_Agency_Code()
        {
            string[] Added_Edited_RefralCode = new string[2];

            Added_Edited_RefralCode[0] = strmsgGrp;
            Added_Edited_RefralCode[1] = Mode;

            return Added_Edited_RefralCode;
        }

        private void Get_Serviecesfor_Referal()
        {
            if (Mode == "Edit")
            {
                CASEREFSEntity Search_REFS_Entity = new CASEREFSEntity(true);
                Search_REFS_Entity.Code = Code.Trim();
                Sel_REFS_List = _model.SPAdminData.Browse_CASEREFS(Search_REFS_Entity, "Browse");
            }
        }

        private void btnServices_Click(object sender, EventArgs e)
        {
            List<CASEREFSEntity> sel_CASEREFS_entity = new List<CASEREFSEntity>();
            AgencyReferral_SubForm Ref_Form = new AgencyReferral_SubForm("AgyRef", Sel_REFS_List, string.Empty);
            Ref_Form.FormClosed += new FormClosedEventHandler(On_Referral_Select_Closed);
            Ref_Form.StartPosition = FormStartPosition.CenterScreen;
            Ref_Form.ShowDialog();
            //Vendor_Browser vendor_Form = new Vendor_Browser(BaseForm, Privileges);
            //vendor_Form.ShowDialog();

        }
        
        
        private void On_Referral_Select_Closed(object sender, FormClosedEventArgs e)
        {
            string SelRef_Name = null;
            string Sql_MSg=string.Empty;
            AgencyReferral_SubForm form = sender as AgencyReferral_SubForm;
            if (form.DialogResult == DialogResult.OK)
            {
                //Sel_REFS_List = form.GetSelected_Referral_Entity();
                Sel_REFS_List = form.GetSelected_Services();
                Add_Refs_List = form.GetAdd_Del_Selected_Services();
                //if (Sel_REFS_List.Count > 30)
                //{
                //    MessageBox.Show("You may not select more than 30 Seevices", "CAPTAIN");
                //}
                
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtZip_Leave(object sender, EventArgs e)
        {
            string strZipCode = txtZip.Text;
            strZipCode = strZipCode.TrimStart('0');
            txtZip.Text = SetLeadingZeros(txtZip.Text);
            //txtZipPlus.Text = "";
            //txtZipPlus.Focus();
        }

        private void txtZipPlus_Leave(object sender, EventArgs e)
        {
            string zipPlus = txtZipPlus.Text;
            txtZipPlus.Text = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
            //maskContPhone.Focus();
            //zipPlus = zipPlus.TrimStart('0');
            //txtZipPlus.Text = SetLeadingZeros(txtZipPlus.Text);
        }

        private void maskPhone_Leave(object sender, EventArgs e)
        {
            //string Phone = maskPhone.Text.Trim();
            //maskPhone.Text = "000000000".Substring(0, 10 - Phone.Length) + Phone;
            //maskContPhone.Text = maskContPhone.Text.Replace(' ', '0');
        }

        private void maskFax_Leave(object sender, EventArgs e)
        {
            //string Phone = maskFax.Text.Trim();
            //maskFax.Text = "000000000".Substring(0, 10 - Phone.Length) + Phone;
            //maskContPhone.Text = maskContPhone.Text.Replace(' ', '0');
        }

        private void maskContPhone_Leave(object sender, EventArgs e)
        {
            //string[] phonebreak = Regex.Split(maskPhone.Text, "-");
            //phonebreak[0] = "000".Substring(0, 3 - phonebreak[0].Length) + phonebreak[0];
            //phonebreak[1] = "000".Substring(0, 3 - phonebreak[1].Length) + phonebreak[1];
            //phonebreak[2] = "000".Substring(0, 4 - phonebreak[3].Length) + phonebreak[2];
            //maskContPhone.Text = phonebreak[0] + "" + phonebreak[1] + "" + phonebreak[2];
           
        }

        private void Case2011Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "TL_HELP")
            {
                Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
            }
        }
    }
}