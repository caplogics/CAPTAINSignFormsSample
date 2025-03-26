extern alias swf;

using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using Wisej.Web;

namespace Captain
{
    public partial class ForgotPassword : Form
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }
        CaptainModel _captainModel = new CaptainModel();
        AgencyControlEntity AgencyControlDetails = null;
        string _StepTyp = ""; int _formDefaultHieght = 132;
        public ForgotPassword(string StepTyp)
        {
            InitializeComponent();
            this.Size = new System.Drawing.Size(650, _formDefaultHieght);
            _errorProvider = new ErrorProvider(this);
            _StepTyp = StepTyp;
            hidepanels(_StepTyp);
            _captainModel = new CaptainModel();
            AgencyControlDetails = _captainModel.ZipCodeAndAgency.GetAgencyControlFile("00");
            timer1.Stop();
            timer1.Enabled = false;
            pnlSt1Tryanother.Visible = false;
            txtCAPemail.Text = "";
            txtCAPMobile.Text = "";
            lblCapemailmoblmsg.Text = "";
        }
        void hidepanels(string stepTyp)
        {
            if (stepTyp == "STP1")
            {
                pnlStep1.Visible = true;
                pnlStep2.Visible = false;
                pnlStep3.Visible = false;
                this.Size = new System.Drawing.Size(650, (_formDefaultHieght + pnlStep1.Height)-102);
            }
            else if (stepTyp == "STP2")
            {
                pnlStep2.Visible = true;
                pnlStep1.Visible = false;
                pnlStep3.Visible = false;
                this.Size = new System.Drawing.Size(650, (_formDefaultHieght + pnlStep2.Height));
            }
            else if (stepTyp == "STP3")
            {
                pnlStep3.Visible = true;
                pnlStep2.Visible = false;
                pnlStep1.Visible = false;
                this.Size = new System.Drawing.Size(650, (_formDefaultHieght + pnlStep3.Height));
            }

        }


        private void btnSetpBack1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Close();
            //LoginForm obj = new LoginForm();
            //obj.ShowDialog();
        }
        private int inttimelife = 0;
        public string _currEmailID = "";
        public string _currPhonenumber = "";
        public string _currPassword = "";
        public string _currUserID = "";
        public string _currAgencyName = "";
        public string _currofficeAddress = "";
        private void btnSendVerifCode_Click(object sender, EventArgs e)
        {
            if (txtCAPUserName.Text != "" && txtCAPemail.Text == "" && txtCAPMobile.Text == "")
            {
                bool _chkflag = _captainModel.UserProfileAccess.checkUseremailMobile(txtCAPUserName.Text, "CHKEMAILMOBILE");
                if (_chkflag == false)
                {
                    AlertBox.Show("System Access is denied, please provide your email and cell details to the System Administrator", Wisej.Web.MessageBoxIcon.Warning);
                    return;
                }
            }

            DataTable dt = _captainModel.UserProfileAccess.FetchLoginUser(txtCAPUserName.Text, txtCAPemail.Text, txtCAPMobile.Text, "FRGTPASS");
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    _currEmailID = dt.Rows[0]["PWR_EMAIL"].ToString();
                    _currPhonenumber = dt.Rows[0]["PWR_MOBILE"].ToString();
                    _currPassword = dt.Rows[0]["PWR_PASSWORD"].ToString();
                    _currUserID = dt.Rows[0]["PWR_EMPLOYEE_NO"].ToString();
                    proptext = CommonFunctions.RandomTokenNumber(6);


                    _currAgencyName = AgencyControlDetails.AgyName.Trim();

                    var AgyCntrlDets = _captainModel.ZipCodeAndAgency.GetAgencyControlFile(dt.Rows[0]["PWR_DEF_AGENCY"].ToString());//userProfile.Agency
                    string strStreet = "", strCity = "", strState = "", strZIP = "";
                    if (AgyCntrlDets != null)
                    {
                        if (AgyCntrlDets.Street != "")
                            strStreet = AgyCntrlDets.Street + ", ";
                        if (AgyCntrlDets.City != "")
                            strCity = AgyCntrlDets.City + ", ";
                        if (AgyCntrlDets.State != "")
                            strState = AgyCntrlDets.State + ", ";
                        if (AgyCntrlDets.Zip1 != "")
                            strZIP = AgyCntrlDets.Zip1 + ".";
                    }

                    _currofficeAddress = strStreet + strCity + (strCity != "" ? ("<br/>" + strState) : strState) + strZIP;

                    string _isEmailSent = CommonFunctions.SendOTPEmailService(_currEmailID, proptext, _currAgencyName, _currofficeAddress);
                    string _isMobileSent = CommonFunctions.SendTextGridSMSService(proptext, _currPhonenumber);

                    if (_isEmailSent == "Y" || _isMobileSent == "Y")
                    {
                        hidepanels("STP2");
                        txtverifytext.Visible = true; lblEntertext.Visible = true; btnValidCaptcher.Visible = true; lblTimerLeft.Visible = true;
                        inttimelife = 120;
                        lblOnetime2.Text = "Please enter the verification code sent to User : <b style='background-color:#fcfccc; padding:4px;'>" + _currUserID + "</b>,  Email: <u>" + CommonFunctions.MaskEmailfunction(_currEmailID) + "</u> or Mobile: " + CommonFunctions.MaskMobilefunction(_currPhonenumber);
                        timer1.Start();
                        timer1.Enabled = true;
                    }
                    else
                    {
                        txtverifytext.Visible = false; lblEntertext.Visible = false; btnValidCaptcher.Visible = false; lblTimerLeft.Visible = false;
                        inttimelife = 120;
                        lblOnetime2.Text = "";
                        timer1.Start();
                        timer1.Enabled = true;
                    }



                }
                else
                {
                    AlertBox.Show("User doesn't exist in CAPTAIN Applicantion. Please contact adminisatrator", Wisej.Web.MessageBoxIcon.Warning);
                }
            }
            else
            {
                // AlertBox.Show("Failed to Verify the User. Please contact adminisatrator", MessageBoxIcon.Warning);
                AlertBox.Show("User doesn't exist in CAPTAIN Applicantion. Please contact adminisatrator", Wisej.Web.MessageBoxIcon.Warning);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // if (userProfile != null)
            //{
            if (inttimelife > 0)
            {
                // Display the new time left
                // by updating the Time Left label.
                inttimelife = inttimelife - 1;
                //txtverifytext.Watermark = inttimelife + " seconds left";
                lblTimerLeft.Text = inttimelife.ToString() + " seconds left";
                linkresend.Visible = false;
            }
            else
            {
                // If the user ran out of time, stop the timer, show
                // a MessageBox, and fill in the answers.
                timer1.Stop();
                linkresend.Visible = true;
                btnValidCaptcher.Visible = false;
                lblEntertext.Visible = txtverifytext.Visible = lblOnetime2.Visible = false; lblTimerLeft.Visible = false;
                if (proptext != string.Empty)
                {
                    propexptext = proptext;
                    proptext = string.Empty;
                }
            }
            //}
        }
        private ErrorProvider _errorProvider = null;
        private void linkresend_LinkClicked(object sender, Wisej.Web.LinkLabelLinkClickedEventArgs e)
        {
            //_errorProvider.SetError(btnValidCaptcher, null);
            txtverifytext.Text = "";
            txtverifytext.Visible = true; lblTimerLeft.Visible = true;
            lblEntertext.Visible = true;
            btnValidCaptcher.Visible = true;
            lblOnetime2.Visible = true;
            //btnLogin.Enabled = false;
            inttimelife = 120;
            linkresend.Visible = false;
            lblEntertext.Visible = true;
            // txtverifyText.Watermark = "60 seconds left";

            proptext = CommonFunctions.RandomTokenNumber(6);
            string _IsMailSent = CommonFunctions.SendOTPEmailService(_currEmailID, proptext, _currAgencyName, _currofficeAddress);
            string _IsMobileSent = CommonFunctions.SendTextGridSMSService(proptext, _currPhonenumber);

            if (_IsMailSent == "Y" || _IsMobileSent == "Y")
            {
                lblOnetime2.Text = "Please enter the verification code sent to User : <b style='background-color:#fcfccc; padding:4px;'>" + _currUserID + "</b>,  Email: <u>" + CommonFunctions.MaskEmailfunction(_currEmailID) + "</u> or Mobile: " + CommonFunctions.MaskMobilefunction(_currPhonenumber);
                timer1.Start();
            }


        }
        private void btnValidCaptcher_Click(object sender, EventArgs e)
        {
            //_errorProvider.SetError(btnValidCaptcher, null);
            if (string.IsNullOrEmpty(txtverifytext.Text))
            {
                _errorProvider.SetError(btnValidCaptcher, "Please Enter Text");
            }
            else
            {
                var boolpagelogin = true;
                if (proptext == string.Empty)
                {
                    boolpagelogin = false;
                    if (propexptext == txtverifytext.Text)
                    {
                        CommonFunctions.MessageBoxDisplay("Text is expired, Please Click on Resend Text link");
                        boolpagelogin = false;
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay("Wrong Text is entered");
                        boolpagelogin = false;
                    }
                }

                if (boolpagelogin)
                {
                    if (proptext == txtverifytext.Text)
                    {
                        timer1.Stop();
                        hidepanels("STP3");

                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay("Invalid OTP");
                    }
                }
            }
        }


        private void btnSavePassword_Click(object sender, EventArgs e)
        {
            if (!isValidate()) return;
            AgencyControlEntity AgencyControlDetails = _captainModel.ZipCodeAndAgency.GetAgencyControlFile("00");
            if (AgencyControlDetails != null)
            {
                bool boolvalid = true;
                if (txtNewPassword.Text.ToUpper().Contains(AgencyControlDetails.AgyShortName.ToUpper()))
                {
                    boolvalid = false;
                }
                if (txtNewPassword.Text.ToUpper().Contains(_currUserID.ToUpper()))
                {
                    boolvalid = false;

                }
                if (boolvalid)
                {
                    if (txtNewPassword.Text == txtConfirmPassword.Text)
                    {
                        string strMsg = _captainModel.UserProfileAccess.UpdatePassword(_currUserID, _currPassword.Trim(), txtNewPassword.Text.Trim());
                        if (strMsg == "success")
                        {
                            AlertBox.Show("Password successfully updated..  ",MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            CommonFunctions.MessageBoxDisplay(strMsg);
                        }
                    }
                    else
                        CommonFunctions.MessageBoxDisplay("New Password and Confirm Passwords are unmatched");
                }
                else
                {
                    CommonFunctions.MessageBoxDisplay("New Password Not allowed  User Name or Company Name.");
                }

            }
            //this.Close();
            //   LoginForm obj = new LoginForm();
            // obj.ShowDialog();
        }
        private bool isValidate()
        {
            bool isValid = true;


            //if (String.IsNullOrEmpty(txtOldPassword.Text))
            //{
            //    _errorProvider.SetError(txtOldPassword, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblOldPassword.Text));
            //    isValid = false;
            //}
            //else
            //{
            //    _errorProvider.SetError(txtOldPassword, null);
            //}

            if (String.IsNullOrEmpty(txtNewPassword.Text.Trim()))
            {
                _errorProvider.SetError(txtNewPassword, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblNewPassword.Text));
                isValid = false;
            }
            else
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtNewPassword.Text, Consts.StaticVars.PasswordRegulation))
                {
                    _errorProvider.SetError(txtNewPassword, "Minimum 8 characters atleast 1 Alphabet, 1 Number and 1 Special Character");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtNewPassword, null);
                }
            }
            if (String.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
            {
                _errorProvider.SetError(txtConfirmPassword, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblConfirmPassword.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtConfirmPassword, null);
            }


            return isValid;
        }
        private void txtNewPassword_Leave(object sender, EventArgs e)
        {
            if (txtNewPassword.Text.Trim() != string.Empty)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtNewPassword.Text, Consts.StaticVars.PasswordRegulation))
                {
                    _errorProvider.SetError(txtNewPassword, "Minimum 8 characters atleast 1 Alphabet, 1 Number and 1 Special Character");
                }
                else
                {
                    _errorProvider.SetError(txtNewPassword, null);
                }
            }
        }

        private string proptext = string.Empty;
        private string propexptext = string.Empty;

        private void lnkST1Try_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pnlSt1Tryanother.Visible = true;
            txtCAPemail.Text = "";
            txtCAPMobile.Text = "";
            this.Size = new System.Drawing.Size(650, (_formDefaultHieght + pnlStep1.Height));
        }

        private void txtCAPUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            lblCapemailmoblmsg.Text = "";
            if (txtCAPUserName.Text != "")
            {

                DataTable dt = _captainModel.UserProfileAccess.FetchLoginUser(txtCAPUserName.Text, "", "", "FRGTPASS");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        string EmailIDMsg = ""; string PhonenumberMsg = "";
                        string EmailID = dt.Rows[0]["PWR_EMAIL"].ToString();
                        string Phonenumber = dt.Rows[0]["PWR_MOBILE"].ToString();
                        if (EmailID != "")
                            EmailIDMsg = "Email - " + CommonFunctions.MaskEmailfunction(EmailID);
                        if (Phonenumber != "")
                        {
                            if (EmailIDMsg != "")
                                PhonenumberMsg = " & Mobile - " + CommonFunctions.MaskMobilefunction(Phonenumber);
                            else
                                PhonenumberMsg = "Mobile - " + CommonFunctions.MaskMobilefunction(Phonenumber);
                        }

                        if (PhonenumberMsg == "" && EmailIDMsg == "")
                            lblCapemailmoblmsg.Text = "";
                        else
                            lblCapemailmoblmsg.Text = "Your " + EmailIDMsg + " " + PhonenumberMsg;
                    }
                }
            }
        }

        private void lblCapclose_Click(object sender, EventArgs e)
        {
            pnlSt1Tryanother.Visible = false;
            txtCAPemail.Text = "";
            txtCAPMobile.Text = "";
            this.Size = new System.Drawing.Size(650, (_formDefaultHieght + pnlStep1.Height)-102);
        }
    }
}
