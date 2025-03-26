#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Wisej.Web;
using Captain.Common.Utilities;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using System.IO;
using Captain.Common.Views.Forms;
using System.Net.Mail;
using System.Configuration;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Globalization;
using static System.Net.WebRequestMethods;
using System.Collections.Specialized;
using System.Net;
using Captain.Common.Interfaces;

#endregion

namespace Captain
{
    public partial class LoginForm : Form /*, ILogonForm*/
    {
        private ErrorProvider _errorProvider = null;
        private string strFolderPath = string.Empty;
        private DirectoryInfo MyDir;
        string AgyShortName = "";
        public string strURL = "";
        public LoginForm()
        {
            InitializeComponent();
            userProfile = null;
            timer1.Stop();
            timer1.Enabled = false;

            pnlAuthentication.Visible = false;
            pnlUserlogin.Visible = true;
            this.Size = new Size(530, Size.Height - pnlAuthentication.Height);




            //string URL = Application.Url;


            // chkLoginMFAControls();

            //var regionInfo = RegionInfo.CurrentRegion;
            //var name = regionInfo.Name;
            //var englishName = regionInfo.EnglishName;
            //var displayName = regionInfo.DisplayName;
            //MessageBox.Show("name:" + name + " eng:" + englishName + " displayname:" + displayName);
        }
        //void chkLoginMFAControls()
        //{
        //    pnlOTPoptions.Visible = false;
        //    var model = new CaptainModel();
        //    var AgencyControlDetails = model.ZipCodeAndAgency.GetAgencyControlFile("00");
        //    if (AgencyControlDetails != null)
        //    {
        //        //if (userProfile.CaseWorker.ToUpper() != "JAKE")
        //        // {
        //        if (AgencyControlDetails.LoginMFA.ToUpper() == "Y")
        //        {
        //            pnlOTPoptions.Visible = true;
        //            this.Size = new Size(530, 300);
        //        }
        //        // }
        //    }
        //}
        private void LoginFormLoad(object sender, EventArgs e)
        {
            try
            {
                strURL = Application.Uri.AbsoluteUri;
                //if (strURL.ToLower().ToString() == "http://localhost:53370/" || strURL.ToLower().ToString() == "https://capsysdev.capsystems.com/captainwisej/")
                //{
                lnkForgetpassword.Visible = true;
                //}
                timer1.Stop();
                if (!string.IsNullOrEmpty(Application.Cookies["UserName"]))
                {
                    txtUserName.Text = Application.Cookies["UserName"];
                    chkRememberUserName.Checked = true;
                    txtPassword.Focus();
                }
                else
                {
                    txtUserName.Focus();
                }
            }
            catch (Exception ex)
            {
                //
            }
        }

        /// <summary>
        /// This method is to validate the form when login button is clicked.
        /// </summary>
        /// <returns>true/false</returns>
        private bool IsFormValid()
        {
            var isValid = true;

            if (_errorProvider == null)
            {
                _errorProvider = new ErrorProvider(this);
                _errorProvider.IconPadding = 10;
            }

            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                _errorProvider.SetError(txtUserName, "Please Enter User ID");
                AlertBox.Show("Please Enter User ID", MessageBoxIcon.Warning);
                txtUserName.Clear();
                txtUserName.Focus();
                return isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtUserName, string.Empty);
                isValid = true;
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                _errorProvider.SetError(txtPassword, "Please Enter Password");
                AlertBox.Show("Please Enter Password", MessageBoxIcon.Warning);
                txtPassword.Clear();
                txtPassword.Focus();
                return isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtPassword, string.Empty);
                isValid = true;
            }

            return isValid;
        }

        private int inttimelife = 0;
        private UserEntity userProfile; string _FullName = "";
        private string AgencyName = ""; string _optinDate = "";
        List<verifyDevice> lstverifyDevices = new List<verifyDevice>();
        private void LoginClick(object sender, EventArgs e)
        {

            lblSendCodeby.Visible = false;
            lblgetcodebyemail.Visible = false;

            VerifyFrom = "frmLogin"; _isemailSent = "N"; _isMobileSent = "N";
            if (IsFormValid())
            {
                _officeAddress = ""; AgencyName = "";
                var userName = txtUserName.Text;
                var password = txtPassword.Text;
                var strErrorMsg = string.Empty;
                lblOnetime2.Text = string.Empty;
                lblOnetime2.Visible = true;
                var model = new CaptainModel();
                try
                {
                    userProfile =
                        model.AuthenticateUser.AuthenticateWithProfile(userName, password, string.Empty,
                                                                        out strErrorMsg);

                    if (userProfile != null)
                    {
                        if (!userProfile.UserID.Equals("@InCorrect@1UID@2PSW") &&
                            !userProfile.UserID.Equals("@InActiveUserId") &&
                            int.Parse(userProfile.UnSuccessful) < 5) //|| userProfile != null) 
                        {
                            var isLoginRegistered = model.AuthenticateUser.RegisterLogin(userProfile.UserID);
                            var boollogin = true;
                            if (txtPassword.Text != "******")
                            {
                                var AgencyControlDetails = model.ZipCodeAndAgency.GetAgencyControlFile("00");
                                if (AgencyControlDetails != null)
                                {
                                    if (userProfile.CaseWorker.ToUpper() != "JAKE")
                                    {
                                        _FullName = userProfile.FirstName + " " + userProfile.LastName;
                                        if (AgencyControlDetails.LoginMFA.ToUpper() == "Y")
                                        {

                                            //**** Check the OPT-IN date is Null or with date
                                            bool chk_optin_date = userProfile.OptInDATE.ToString() != "" ? true : false;
                                            string _AlertMsg = "";
                                            bool chkoptin = false;
                                            if (chk_optin_date)
                                            {
                                                if (userProfile.PWDEmail.Trim() != string.Empty || userProfile.PWRMobile.Trim() != string.Empty)
                                                    chkoptin = true;
                                                else
                                                {
                                                    chkoptin = false;
                                                    _AlertMsg = "System Access is denied, please provide your email and cell details to the System Administrator.";
                                                }
                                            }
                                            else
                                            {
                                                if (userProfile.PWDEmail.Trim() != string.Empty)
                                                    chkoptin = true;
                                                else
                                                {
                                                    chkoptin = false;
                                                    _AlertMsg = "System Access is denied, please provide your email details to the System Administrator.";
                                                }
                                            }

                                            //****************************************************************************************************//


                                            if (chkoptin)
                                            {
                                                /****************************************************************************************************/
                                                /****************************************************************************************************/
                                                string strverTrust = "";
                                                Wisej.Base.Cookie c = Wisej.Web.Application.Cookies.Get("lstverifyDevices");
                                                if (c != null)
                                                {
                                                    lstverifyDevices = JsonConvert.DeserializeObject<List<verifyDevice>>(c.Value);
                                                    if (lstverifyDevices.Count > 0)
                                                    {

                                                        var lstverfdev1 = lstverifyDevices.Where(x => x.username == txtUserName.Text).ToList();
                                                        if (lstverfdev1.Count > 0)
                                                        {
                                                            strverTrust = lstverfdev1[0].trust;
                                                        }

                                                    }
                                                }
                                                /****************************************************************************************************/
                                                /****************************************************************************************************/




                                                if (userProfile.PWDEmail.Trim() != string.Empty && strverTrust != "Y")
                                                {
                                                    HierarchyEntity hierarchyDetails = model.HierarchyAndPrograms.GetCaseHierarchy("AGENCYNAME", userProfile.Agency, string.Empty, string.Empty, string.Empty, string.Empty);
                                                    AgencyName = hierarchyDetails.HirarchyName.Trim();// AgencyControlDetails.AgyName.Trim();

                                                    var AgyCntrlDets = model.ZipCodeAndAgency.GetAgencyControlFile(userProfile.Agency);
                                                    //AgencyControlEntity AgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile(agencyControl);
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

                                                    _officeAddress = strStreet + strCity + (strCity != "" ? ("<br/>" + strState) : strState) + strZIP;
                                                    boollogin = false;
                                                }
                                            }
                                            else
                                            {
                                                //MessageBox.Show("Email Address is not updated. Please contact your Adminsitrator.","CAPTAIN",MessageBoxButtons.OK,MessageBoxIcon.Question);
                                                AlertBox.Show(_AlertMsg, MessageBoxIcon.Warning);
                                                return;
                                            }
                                        }
                                    }
                                }
                                if (boollogin)
                                {

                                    Captain<string>.Session[Consts.SessionVariables.FullName] =
                                        userProfile.FirstName.Trim() + Consts.Common.Space +
                                        (userProfile.MI.Trim().Equals(string.Empty)
                                            ? string.Empty
                                            : userProfile.MI + Consts.Common.Space) + userProfile.LastName.Trim();
                                    Captain<string>.Session[Consts.SessionVariables.UserID] = userProfile.UserID.Trim();
                                    Captain<string>.Session[Consts.SessionVariables.UserName] = userProfile.UserName;

                                    Captain<string>.Session[Consts.SessionVariables.LostLogin_Status] = " Successful";
                                    Captain<string>.Session[Consts.SessionVariables.LostLogin_Date] =
                                        userProfile.LastSuccessful;
                                    if (int.Parse(userProfile.UnSuccessful) > 0)
                                    {
                                        Captain<string>.Session[Consts.SessionVariables.LostLogin_Status] =
                                            " unsuccessful";
                                        Captain<string>.Session[Consts.SessionVariables.LostLogin_Date] =
                                            userProfile.LastUnSuccessful;
                                    }

                                    string strHelpURL = chkZendeskURL();
                                    if (strHelpURL == "")
                                    {
                                        Captain<UserEntity>.Session[Consts.SessionVariables.UserProfile] = userProfile;
                                        Application.Session.IsLoggedOn = true;
                                        DialogResult = DialogResult.OK;
                                        Close();
                                    }
                                    else
                                    {
                                        Application.Navigate(strHelpURL);
                                    }
                                }
                                else
                                {
                                    //var _model = new CaptainModel();
                                    //List<HierarchyEntity> lstHIEentity = _model.HierarchyAndPrograms.GetCaseHierachyAllData(userProfile.Agency, "", "");
                                    //if (lstHIEentity.Count > 0)
                                    //{
                                    //    AgencyName = lstHIEentity[0].HirarchyName;
                                    //}

                                    /****************************************************************************************************/
                                    /****************************************************************************************************/
                                    chktrusted.Visible = true;
                                    Wisej.Base.Cookie c = Wisej.Web.Application.Cookies.Get("lstverifyDevices");
                                    if (c != null)
                                    {
                                        lstverifyDevices = JsonConvert.DeserializeObject<List<verifyDevice>>(c.Value);
                                        if (lstverifyDevices.Count > 0)
                                        {

                                            var lstverfdev1 = lstverifyDevices.Where(x => x.username == txtUserName.Text).ToList();
                                            if (lstverfdev1.Count > 0)
                                            {
                                                chktrusted.Visible = false;
                                            }

                                        }
                                    }
                                    /****************************************************************************************************/
                                    /****************************************************************************************************/
                                    HierarchyEntity hierarchyDetails = model.HierarchyAndPrograms.GetCaseHierarchy("AGENCYNAME", userProfile.Agency, string.Empty, string.Empty, string.Empty, string.Empty);
                                    AgencyName = hierarchyDetails.HirarchyName.Trim();
                                    //AgyShortName = AgencyControlDetails.AgyShortName.Trim();
                                    RandomTokenNumber(6);

                                    if (userProfile.OptInDATE.ToString() != "")
                                    {
                                        if (userProfile.PWRMobile != "")
                                        {
                                            //if (strURL.ToLower().ToString() == "http://localhost:53370/" || strURL.ToLower().ToString() == "https://capsysdev.capsystems.com/captainwisej/")
                                            //{
                                            //    if (userProfile.UserID.ToUpper() == "SYSTEM")
                                            SendMobileSMS(userProfile.PWRMobile, proptext);
                                            if (userProfile.PWDEmail != "")
                                            {
                                                lblSendCodeby.Visible = true;
                                                lblgetcodebyemail.Visible = true;
                                                lblgetcodebyemail.Text = "Get Code via email";
                                            }
                                            //}
                                        }
                                        else if (userProfile.PWDEmail != "")
                                        {
                                            SendEmail(userProfile.PWDEmail, proptext, AgencyName, _officeAddress, userProfile.OptInDATE, userProfile.PWRMobile);
                                        }
                                    }
                                    else
                                    {
                                        if (userProfile.PWDEmail != "")
                                        {
                                            SendEmail(userProfile.PWDEmail, proptext, AgencyName, _officeAddress, userProfile.OptInDATE, userProfile.PWRMobile);
                                        }
                                        if (userProfile.PWRMobile != "")
                                        {
                                            lblgetcodebyemail.Visible = true; lblgetcodebyemail.Text = "Get Code via Phone";
                                        }
                                    }


                                    if (_isemailSent == "Y" || _isMobileSent == "Y")
                                    {
                                        
                                        txtverifytext.Text = "";
                                        label3.Visible = true;
                                        pnlUserlogin.Visible = false;
                                        this.Size = new Size(630, this.Size.Height + pnlAuthentication.Height - pnlUserlogin.Height);
                                        pnlauthblock.Visible = true;
                                        pnlAuthentication.Visible = true;
                                        lblTimerLeft.Visible = true;
                                        txtverifytext.Visible = true; lblEntertext.Visible = true; btnValidCaptcher.Visible = true; chktrusted.Visible = true; lblTimerLeft.Visible = true;
                                        btnLogin.Enabled = false; linkresend.Visible = false; linktryanotheruser.Visible = lblOnetime2.Visible = true;
                                        txtPassword.Enabled = false; txtUserName.Enabled = false;
                                        linktryanotheruser.Location = new Point(345, 164);
                                        inttimelife = 120;
                                        txtverifytext.Focus();

                                        lblOTPUserID.Text = userProfile.UserID;
                                        lblOTPsentdate.Text = DateTime.Now.ToString();

                                        if (_isemailSent == "Y")
                                        {
                                            lblOTPMessage.Text = "Check your email for the code";
                                            lblOTPMessage.Tag = "email";
                                            lblOnetime2.Text = " " + MaskEmailfunction(userProfile.PWDEmail);
                                        }
                                        else if (_isMobileSent == "Y")
                                        {
                                            lblOTPMessage.Text = "Check your phone for the code";
                                            lblOTPMessage.Tag = "phone";
                                            lblOnetime2.Text = " " + CommonFunctions.MaskMobilefunction(userProfile.PWRMobile.ToString());
                                        }

                                        timer1.Start();
                                        timer1.Enabled = true;
                                    }
                                    if (_isemailSent == "N" && _isMobileSent == "N")
                                    {
                                        pnlUserlogin.Visible = true;
                                        this.Size = new Size(630, this.Size.Height + 30);
                                        pnlauthblock.Visible = true;
                                        pnlAuthentication.Visible = true;
                                        txtverifytext.Visible = false; lblEntertext.Visible = false; btnValidCaptcher.Visible = false; chktrusted.Visible = false; lblTimerLeft.Visible = false;
                                        btnLogin.Enabled = false; linkresend.Visible = false; lblOnetime2.Visible = false;
                                        txtPassword.Enabled = false; txtUserName.Enabled = false;
                                        label3.Visible = false;
                                        linktryanotheruser.Visible = true;
                                        linktryanotheruser.Location = new Point(340, 0);
                                        inttimelife = 120;
                                        lblOnetime2.Text = "";
                                        timer1.Start();
                                        timer1.Enabled = true;
                                    }
                                }
                            }
                            else
                            {
                                if (userProfile.AccessAll.Equals("Y"))
                                {
                                    Admn0004UserForm objAdmn0004 = new Admn0004UserForm(userProfile, txtUserName.Text);
                                    objAdmn0004.StartPosition = FormStartPosition.CenterScreen;

                                    objAdmn0004.ShowDialog();

                                    if (Application.Session.IsLoggedOn == true)
                                    {
                                        //DialogResult = DialogResult.OK;
                                        Close();
                                    }
                                }
                                else
                                {
                                    AlertBox.Show("Invalid User ID/Password. Please Contact System administrator", MessageBoxIcon.Warning);
                                }
                            }

                            Application.Session["usersessionid"] = Application.SessionId;
                            var ip = Application.ServerVariables["HTTP_X_FORWARDED_FOR"];
                            if (string.IsNullOrEmpty(ip)) ip = Application.ServerVariables["REMOTE_ADDR"];
                            Application.Session["userlogid"] = model.UserProfileAccess.InsertUpdateLogUsers(
                                userProfile.UserID, Application.Session["usersessionid"].ToString(), ip, "Add",
                                string.Empty);
                        }
                        else if (userProfile.UserID.Equals("@NoHierarchy"))
                        {
                            AlertBox.Show("Sorry! you have access on no hierarchy", MessageBoxIcon.Warning);
                        }
                        else if (userProfile.UserID.Equals("@InActiveUserId"))
                        {
                            AlertBox.Show("Inactivated User...Please Contact Administrator...", MessageBoxIcon.Warning);
                        }
                        else
                        {
                            //AlertBox.Show("Invalid User ID/Password. Please Contact System administrator", MessageBoxIcon.Warning);
                            //return;
                            if (userProfile.UserID.Equals("@InCorrect@1UID@2PSW") &&
                                (userProfile.Password.Equals("PASSWORD") || int.Parse(userProfile.UnSuccessful) > 0))
                            {
                                //if (int.Parse(userProfile.UnSuccessful) < 5)
                                //{
                                //    AlertBox.Show("Invalid Password. You have '" +
                                //                    (5 - int.Parse(userProfile.UnSuccessful)).ToString() +
                                //                    "' attempts Left With", MessageBoxIcon.Warning);
                                //    return;
                                //}
                                //else
                                //{
                                //    AlertBox.Show("Your Account is Blocked. Please Contact System administrator", MessageBoxIcon.Warning);
                                //    return;
                                //}
                                if (int.Parse(userProfile.UnSuccessful) == 5)
                                {
                                    AlertBox.Show("Your Account is Blocked. Please Contact System administrator", MessageBoxIcon.Warning);
                                    return;
                                }
                                else
                                {
                                    if (int.Parse(userProfile.UnSuccessful) < 5)
                                    {
                                        AlertBox.Show("Invalid Password. You have '" +
                                                        (5 - int.Parse(userProfile.UnSuccessful)).ToString() +
                                                        "' attempts Left With", MessageBoxIcon.Warning);
                                        return;
                                    }
                                    else
                                    {
                                        AlertBox.Show("Your Account is Blocked. Please Contact System administrator", MessageBoxIcon.Warning);
                                        return;
                                    }
                                    //AlertBox.Show("Invalid User ID/Password. Please Contact System administrator", MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                AlertBox.Show("Invalid User ID/Password. Please Contact System administrator", MessageBoxIcon.Warning);
                            }


                        }

                        if (chkRememberUserName.Checked)
                            Application.Cookies[Consts.SessionVariables.UserName] = txtUserName.Text;
                        else
                            Application.Cookies[Consts.SessionVariables.UserName] = null;
                    }
                    else
                    {
                        //CommonFunctions.MessageBoxDisplay("Sqlserver is down please contact administrator");
                        CommonFunctions.MessageBoxDisplay(strErrorMsg);
                    }
                }
                catch (Exception ex)
                {
                    CommonFunctions.MessageBoxDisplay(strErrorMsg);
                }
            }
        }

      

        /// <summary>
        /// This event is fired when the user types enter key in password textbox.
        /// </summary>
        /// <param name="objSender"></param>
        /// <param name="objArgs"></param>
        private void PasswordEnterKeyDown(object objSender, KeyEventArgs objArgs)
        {
            if (objArgs.KeyCode == Keys.Enter)
                LoginClick(btnLogin, EventArgs.Empty);
        }


        private void txtPassword_GotFocus(object sender, EventArgs e)
        {
            txtPassword.SelectionLength = txtPassword.Text.Length;
        }

        string VerifyFrom = "";
        private void btnValidCaptcher_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(lblverifyicon, null);
            if (string.IsNullOrEmpty(txtverifytext.Text))
            {
                _errorProvider.SetError(lblverifyicon, "Please Enter Text");
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

                if (VerifyFrom == "frmLogin")
                {
                    if (boolpagelogin)
                    {
                        if (proptext == txtverifytext.Text)
                        {
                            timer1.Stop();
                            var model = new CaptainModel();
                            _errorProvider.SetError(lblverifyicon, null);
                            var isLoginRegistered = model.AuthenticateUser.RegisterLogin(userProfile.UserID);


                            Captain<string>.Session[Consts.SessionVariables.FullName] = userProfile.FirstName.Trim() +
                                Consts.Common.Space +
                                (userProfile.MI.Trim().Equals(string.Empty)
                                    ? string.Empty
                                    : userProfile.MI + Consts.Common.Space) + userProfile.LastName.Trim();
                            Captain<string>.Session[Consts.SessionVariables.UserID] = userProfile.UserID.Trim();
                            Captain<string>.Session[Consts.SessionVariables.UserName] = userProfile.UserName;

                            Captain<string>.Session[Consts.SessionVariables.LostLogin_Status] = " Successful";
                            Captain<string>.Session[Consts.SessionVariables.LostLogin_Date] = userProfile.LastSuccessful;
                            if (int.Parse(userProfile.UnSuccessful) > 0)
                            {
                                Captain<string>.Session[Consts.SessionVariables.LostLogin_Status] = " unsuccessful";
                                Captain<string>.Session[Consts.SessionVariables.LostLogin_Date] =
                                    userProfile.LastUnSuccessful;
                            }

                            Wisej.Base.Cookie c = Wisej.Web.Application.Cookies.Get("lstverifyDevices");
                            string strverTrust = "";
                            if (c != null)
                            {
                                lstverifyDevices = JsonConvert.DeserializeObject<List<verifyDevice>>(c.Value);

                                if (lstverifyDevices.Count > 0)
                                {

                                    var lstverfdev1 = lstverifyDevices.Where(x => x.username == txtUserName.Text).ToList();
                                    if (lstverfdev1.Count > 0)
                                        strverTrust = lstverfdev1[0].trust;

                                }
                            }

                            if (chktrusted.Checked)
                            {

                                string strchkTrust = "Y";
                                // if (chkTrust.Checked)
                                //  strchkTrust = "Y";

                                string username = txtUserName.Text;

                                var lstverfdev = lstverifyDevices.Where(x => x.username == txtUserName.Text).ToList();
                                if (lstverfdev.Count > 0)
                                {
                                    lstverfdev[0].password = txtPassword.Text;
                                    lstverfdev[0].trust = strchkTrust;

                                }
                                else
                                    lstverifyDevices.Add(new verifyDevice { username = username, password = txtPassword.Text, trust = strchkTrust });

                                string myObjectJson = new JavaScriptSerializer().Serialize(lstverifyDevices);
                                Wisej.Web.Application.Cookies.Add("lstverifyDevices", myObjectJson);

                            }



                            Captain<UserEntity>.Session[Consts.SessionVariables.UserProfile] = userProfile;
                            Application.Session.IsLoggedOn = true;
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                        else
                        {
                            CommonFunctions.MessageBoxDisplay("Invalid OTP");
                        }
                    }
                }
                else if (VerifyFrom == "frmForgotpass")
                {
                    if (boolpagelogin)
                    {
                        _isemailSent = "N"; _isMobileSent = "N";
                        if (proptext == txtverifytext.Text)
                        {
                            timer1.Stop();

                            if (lblOTPMessage.Tag.ToString() == "email")
                            {
                                if (_forgotEmail != "")
                                {
                                    var model = new CaptainModel();
                                    var AgencyControlDetails = model.ZipCodeAndAgency.GetAgencyControlFile("00");
                                    AgencyName = AgencyControlDetails.AgyName.Trim();

                                    var AgyCntrlDets = AgencyControlDetails;// model.ZipCodeAndAgency.GetAgencyControlFile(userProfile.Agency);
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

                                    _officeAddress = strStreet + strCity + (strCity != "" ? ("<br/>" + strState) : strState) + strZIP;

                                    SendPasswordEmail(_forgotEmail, _forgotPassword, AgencyName, _officeAddress);
                                }
                            }
                            else if (lblOTPMessage.Tag.ToString() == "phone")
                            {
                                if (_forgotMobileno != "")
                                {
                                    //if (strURL.ToLower().ToString() == "http://localhost:53370/" || strURL.ToLower().ToString() == "https://capsysdev.capsystems.com/captainwisej/")
                                    //{
                                    //    if (_forgotUserID.ToUpper() == "SYSTEM")
                                    //    {
                                    //   SendMobileSMS(_forgotMobileno, proptext);

                                    if (userProfile.OptInDATE.ToString() != "")
                                        SendTextGridSMSService("Your CAPTAIN Login Password is " + _forgotPassword, _forgotMobileno);
                                    //    }
                                    //}
                                }
                            }




                            if (_isemailSent == "Y" || _isMobileSent == "Y")
                            {
                                if (_isMobileSent == "Y")
                                    AlertBox.Show("Password sent to your Mobile no");
                                if (_isemailSent == "Y")
                                    AlertBox.Show("Password sent to your Email");

                                var model = new CaptainModel();
                                var dtMailConfig = model.UserProfileAccess.UpdateForcePassword(_forgotUserID);

                                //Application.Session["Forgotpass"] = "Y";

                                _errorProvider.SetError(lblverifyicon, null);
                                timer1.Stop();
                                lblTimerLeft.Visible = false;
                                txtverifytext.Visible = false; lblEntertext.Visible = false; btnValidCaptcher.Visible = linkresend.Visible = lblOnetime2.Visible = linktryanotheruser.Visible = false; chktrusted.Visible = false; lblTimerLeft.Visible = false;
                                btnLogin.Enabled = true; txtUserName.Enabled = true; txtPassword.Enabled = true; userProfile = null; txtUserName.Text = ""; txtPassword.Clear();

                                pnlAuthentication.Visible = false;
                                pnlUserlogin.Visible = true;
                                this.Size = new System.Drawing.Size(530, 300);
                            }


                        }
                        else
                        {
                            CommonFunctions.MessageBoxDisplay("Invalid OTP");
                        }
                    }
                }
            }
        }

        #region Recapptcha

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string proptext = string.Empty;
        private string propexptext = string.Empty;

        private string RandomTokenNumber(int length)
        {
            var strTokenNumber = RandomString(length);
            proptext = strTokenNumber;
            return strTokenNumber;
        }

        public string _isemailSent = "Y";
        public string _isMobileSent = "Y";
        public string _officeAddress = "";
        private void SendEmail(string emailID, string TokenNumber, string _AgencyName, string _officeAddr, string _optinDate, string _mobileNo)
        {
            try
            {
                var model = new CaptainModel();
                var dtMailConfig = model.UserProfileAccess.GetEMailSetting("LOGINOTP");
                //AlertBox.Show(_AgencyName);
                if (dtMailConfig.Rows.Count > 0)
                {
                    var mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(dtMailConfig.Rows[0]["MAIL_EMAILID"].ToString(), _AgencyName);
                    mailMessage.Subject = dtMailConfig.Rows[0]["MAIL_SUBJECT"].ToString();

                    var body = dtMailConfig.Rows[0]["MAIL_CONTENT"].ToString().Replace("User",_FullName) + TokenNumber;
                    body = body + "<br/>";



                    if (_mobileNo != "")
                    {
                        List<OPTinParams> _optinParams = new List<OPTinParams>();
                        _optinParams.Add(new OPTinParams { username = txtUserName.Text, optStatus = (_optinDate.ToString() == "" ? "A" : "D") });
                        var jsonParms = JsonConvert.SerializeObject(_optinParams);
                        string stroptinMessage = CommonFunctions.OPTinMessage(_optinDate, jsonParms);
                        body += stroptinMessage;
                    }

                    body += "<div style='color:#707070;'>" + _AgencyName + "<br/>" + _officeAddr + "</div>";
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;

                    mailMessage.To.Add(emailID);


                    var smtp = new SmtpClient();
                    smtp.Host = dtMailConfig.Rows[0]["MAIL_HOST"].ToString();
                    smtp.EnableSsl = true;
                    var NetworkCred = new System.Net.NetworkCredential();
                    NetworkCred.UserName = dtMailConfig.Rows[0]["MAIL_EMAILID"].ToString();
                    NetworkCred.Password = dtMailConfig.Rows[0]["MAIL_PASSWORD"].ToString();
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = int.Parse(dtMailConfig.Rows[0]["MAIL_PORT"].ToString());
                    smtp.Send(mailMessage);
                    _isemailSent = "Y";
                }
            }
            catch (Exception ex)
            {
                _isemailSent = "N";
                //AlertBox.Show("Failed to send email OTP. Please contact Administrator", MessageBoxIcon.Warning);
            }
        }

        private void SendPasswordEmail(string emailID, string strPassword, string _AgencyName, string _officeAddr)
        {
            try
            {
                var model = new CaptainModel();
                var dtMailConfig = model.UserProfileAccess.GetEMailSetting("LOGINOTP");

                if (dtMailConfig.Rows.Count > 0)
                {
                    var mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(dtMailConfig.Rows[0]["MAIL_EMAILID"].ToString(), _AgencyName);
                    mailMessage.Subject = "Password for CAPTAIN Login";

                    var body = "Your CAPTAIN Login Password is : <b style='padding:5px; background-color:#fafcc0'>" + strPassword + "</b>";
                    body = body + "<br/><br/><br/><br/><br/><br/>" +
                        _AgencyName + "<br/>" + _officeAddr;
                    //dtMailConfig.Rows[0]["MAIL_SENDER_NAME"].ToString() + "<br/>" + 
                    //dtMailConfig.Rows[0]["MAIL_SENDER_ADDR"].ToString();
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;

                    mailMessage.To.Add(emailID);


                    var smtp = new SmtpClient();
                    smtp.Host = dtMailConfig.Rows[0]["MAIL_HOST"].ToString();
                    smtp.EnableSsl = true;
                    var NetworkCred = new System.Net.NetworkCredential();
                    NetworkCred.UserName = dtMailConfig.Rows[0]["MAIL_EMAILID"].ToString();
                    NetworkCred.Password = dtMailConfig.Rows[0]["MAIL_PASSWORD"].ToString();
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = int.Parse(dtMailConfig.Rows[0]["MAIL_PORT"].ToString());
                    smtp.Send(mailMessage);
                    _isemailSent = "Y";
                }
            }
            catch (Exception ex)
            {
                _isemailSent = "N";
                //AlertBox.Show("Failed to send email OTP. Please contact Administrator", MessageBoxIcon.Warning);
            }
        }
        private void SendMobileSMS(string MobileNo, string OTP)
        {

            SendTextGridSMSService("Your CAPTAIN Login OTP is " + OTP, MobileNo);


        }

        void SendTextGridSMSService(string MsgText, string toMobileNo)
        {
            try
            {
                // Textgrid
                const string accountSid = "aUS5GO7UX98eIwuElE77Qw==";
                const string authToken = "3F378BBC927E4815AB7BAF7CF21272DE";//"d3e258866ab946b69f404f7c04ac4492";
                const string twilioPhoneNumber = "+19782129302";
                string recipientPhoneNumber = toMobileNo; //"+917075380141";

                string messageBody = MsgText;
                var url = $"https://api.textgrid.com/2010-04-01/Accounts/{accountSid}/Messages.json";
                using (var client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(accountSid, authToken);

                    var values = new NameValueCollection
            {
                { "From", twilioPhoneNumber },
                { "To", recipientPhoneNumber },
                { "Body", messageBody }
            };

                    try
                    {
                        byte[] response = client.UploadValues(url, "POST", values);
                        string strJson = System.Text.Encoding.UTF8.GetString(response);
                        dynamic jsonArray = JsonConvert.DeserializeObject(strJson);
                        //MessageBox.Show("SMS Sent Successfully!");
                    }
                    catch (WebException ex)
                    {
                        //MessageBox.Show($"Error sending SMS: {ex.Message}");
                    }
                }
                _isMobileSent = "Y";
            }
            catch (Exception ex)
            {
                _isMobileSent = "N";
            }
        }
        
        
        /*
        void SendTwilloSMSService(string MsgText, string toMobileNo)
        {
            try
            {
                // Find your Account SID and Auth Token at twilio.com/console
                // and set the environment variables. See http://twil.io/secure
                string accountSid = "ACa05625950fa41c32b6e0ff41150915d6";// Environment.GetEnvironmentVariable("ACa05625950fa41c32b6e0ff41150915d6");
                string authToken = "3bfff993ac12e189fb631f2a7652d83f"; // Environment.GetEnvironmentVariable("3bfff993ac12e189fb631f2a7652d83f");

                TwilioClient.Init(accountSid, authToken);

                if (toMobileNo == "7075380141" || toMobileNo == "9849899214" || toMobileNo == "9963481181" || toMobileNo == "7075249766")
                    toMobileNo = "+91" + toMobileNo;
                else
                    toMobileNo = "+1" + toMobileNo;

                var message = MessageResource.Create(
                    body: MsgText,                                                 //"Join Earth's mightiest heroes. Like Kevin Bacon.",
                    from: new Twilio.Types.PhoneNumber("+12057517533"),  // Trail version Twillo Phone number provided by the Twillo Website                   //+15017122661
                    to: new Twilio.Types.PhoneNumber(toMobileNo)                        //+15558675310
                );
                _isemailSent = "Y";
            }
            catch (Exception ex)
            {
                _isemailSent = "N";
            }

        }
        */
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (userProfile != null)
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
                lblEntertext.Visible = txtverifytext.Visible = lblOnetime2.Visible = false; chktrusted.Visible = false; lblTimerLeft.Visible = false;
                if (proptext != string.Empty)
                {
                    propexptext = proptext;
                    proptext = string.Empty;
                }
            }
            //}
        }

        private void linkresend_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _errorProvider.SetError(lblverifyicon, null); txtverifytext.Text = string.Empty;
            txtverifytext.Visible = true; chktrusted.Visible = false; lblTimerLeft.Visible = true;
            lblEntertext.Visible = true;
            btnValidCaptcher.Visible = true;
            lblOnetime2.Visible = true;
            btnLogin.Enabled = false;
            inttimelife = 120;
            linkresend.Visible = false;
            lblEntertext.Visible = true;
            //lblSendCodeby.Visible = false;
            //lblgetcodebyemail.Visible = false;
            // txtverifyText.Watermark = "60 seconds left";
            timer1.Start();
            RandomTokenNumber(6);
            var strErrorMsg = string.Empty;
            var model = new CaptainModel();
            userProfile = model.AuthenticateUser.AuthenticateWithProfile(txtUserName.Text, txtPassword.Text, string.Empty,
                                                          out strErrorMsg);

            //HierarchyEntity hierarchyDetails = model.HierarchyAndPrograms.GetCaseHierarchy("Agency", userProfile.Agency, string.Empty, string.Empty, string.Empty, string.Empty);
            //AgencyName = hierarchyDetails.HirarchyName.Trim();

            if (VerifyFrom == "frmLogin")
            {
                chktrusted.Visible = true; _FullName = userProfile.FirstName + " " + userProfile.LastName;
                if (lblOTPMessage.Tag.ToString() == "email")
                {
                    SendEmail(userProfile.PWDEmail, proptext, AgencyName, _officeAddress, userProfile.OptInDATE.ToString(), userProfile.PWRMobile);
                }
                else if (lblOTPMessage.Tag.ToString() == "phone")
                {
                    //if (strURL.ToLower().ToString() == "http://localhost:53370/" || strURL.ToLower().ToString() == "https://capsysdev.capsystems.com/captainwisej/")
                    //{

                    //  if (userProfile.UserID.ToUpper() == "SYSTEM")
                    if (userProfile.OptInDATE.ToString() != "")
                        SendMobileSMS(userProfile.PWRMobile, proptext);
                    //}
                }
                lblOTPsentdate.Text = DateTime.Now.ToString();
            }
            else if (VerifyFrom == "frmForgotpass")
            {
                _FullName = txtUserName.Text;
                if (lblOTPMessage.Tag.ToString() == "email")
                {
                    SendEmail(_forgotEmail, proptext, AgencyName, _officeAddress, _forgotOptinDate, _forgotMobileno);
                }
                else if (lblOTPMessage.Tag.ToString() == "phone")
                {
                    //if (strURL.ToLower().ToString() == "http://localhost:53370/" || strURL.ToLower().ToString() == "https://capsysdev.capsystems.com/captainwisej/")
                    //{

                    //    if (_forgotUserID.ToUpper() == "SYSTEM")
                    if (userProfile.OptInDATE.ToString() != "")
                        SendMobileSMS(_forgotMobileno, proptext);
                    //}
                }
                lblOTPsentdate.Text = DateTime.Now.ToString();
            }

        }

        private void linktryanotheruser_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            _errorProvider.SetError(lblverifyicon, null);
            timer1.Stop();
            lblTimerLeft.Visible = false;
            txtverifytext.Visible = false; lblEntertext.Visible = false; btnValidCaptcher.Visible = linkresend.Visible = lblOnetime2.Visible = linktryanotheruser.Visible = false; chktrusted.Visible = false; lblTimerLeft.Visible = false;
            btnLogin.Enabled = true; txtUserName.Enabled = true; txtPassword.Enabled = true; userProfile = null; txtUserName.Text = ""; txtPassword.Clear();

            pnlAuthentication.Visible = false;
            pnlUserlogin.Visible = true;
            this.Size = new System.Drawing.Size(530, 300);
        }

        private string MaskEmailfunction(string strEmail)
        {
            var strMainmaskemail = string.Empty;
            try
            {
                var stremailat = strEmail.Split('@');
                if (stremailat.Length > 0)
                {
                    var email = stremailat[0].ToString();
                    var strmaskemail = string.Empty;
                    if (email.Length > 0)
                        for (var i = 0; i < email.Length; i++)
                            if (i == 0)
                                strmaskemail = strmaskemail + email[0].ToString();
                            else if (i == email.Length - 1)
                                strmaskemail = strmaskemail + email[email.Length - 1].ToString();
                            else
                                strmaskemail = strmaskemail + "*";
                    //var maskedEmail = string.Format("{0}****{1}", email[0],
                    //email.Substring(email.Length - 1));
                    var email2 = stremailat[1].ToString();
                    var strmaskemail2 = string.Empty;
                    if (email2.Length > 0)
                    {
                        var strarremail = email2.Split('.');
                        for (var i = 0; i < strarremail[0].Length; i++)
                            if (i == 0)
                                strmaskemail2 = strmaskemail2 + strarremail[0][0].ToString();
                            else if (i == strarremail[0].Length - 1)
                                strmaskemail2 = strmaskemail2 + strarremail[0][strarremail[0].Length - 1].ToString();
                            else
                                strmaskemail2 = strmaskemail2 + "*";
                        strmaskemail2 = strmaskemail2 + "." + strarremail[1];
                    }


                    //var maskedEmail2 = string.Format("{0}****{1}", email2[0],
                    //email2.Substring(email2.IndexOf('.') - 1));
                    strMainmaskemail = strmaskemail + "@" + strmaskemail2;
                }
            }
            catch (Exception ex)
            {
            }

            return strMainmaskemail;
        }

        private void lblOnetime_TextChanged(object sender, EventArgs e)
        {
            pnlAuthentication.Visible = true;
            lblOnetime2.Visible = true;

            pnlUserlogin.Visible = false;
        }
        CaptainModel _captainModel = new CaptainModel();
        string _forgotPassword = ""; string _forgotUserID = "";
        string _forgotMobileno = ""; string _forgotEmail = ""; string _forgotOptinDate = "";
        private void lnkForgetpassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            VerifyFrom = "frmForgotpass"; _isemailSent = "N"; _isMobileSent = "N"; txtverifytext.Text = "";
            lblSendCodeby.Visible = false;
            lblgetcodebyemail.Visible = false;
            if (_errorProvider == null)
            {
                _errorProvider = new ErrorProvider(this);
                //_errorProvider.IconPadding = 10;
            }
            if (txtUserName.Text != "")
            {
                DataTable dt = _captainModel.UserProfileAccess.FetchLoginUser(txtUserName.Text, "", "", "FRGTPASS");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        _forgotMobileno = dt.Rows[0]["PWR_MOBILE"].ToString();
                        _forgotEmail = dt.Rows[0]["PWR_EMAIL"].ToString();
                        _forgotUserID = dt.Rows[0]["PWR_EMPLOYEE_NO"].ToString();
                        _forgotPassword = dt.Rows[0]["PWR_PASSWORD"].ToString();
                        _forgotOptinDate = dt.Rows[0]["PWR_OPT_IN"].ToString();




                        //**** Check the OPT-IN date is Null or with date
                        bool chk_optin_date = dt.Rows[0]["PWR_OPT_IN"].ToString() != "" ? true : false;
                        string _AlertMsg = "";
                        bool chkoptin = false;
                        if (chk_optin_date)
                        {
                            if (_forgotEmail.Trim() != string.Empty || _forgotMobileno.Trim() != string.Empty)
                                chkoptin = true;
                            else
                            {
                                chkoptin = false;
                                _AlertMsg = "System Access is denied, please provide your email and cell details to the System Administrator.";
                            }
                        }
                        else
                        {
                            if (_forgotEmail.Trim() != string.Empty)
                                chkoptin = true;
                            else
                            {
                                chkoptin = false;
                                _AlertMsg = "System Access is denied, please provide your email details to the System Administrator.";
                            }
                        }

                        //****************************************************************************************************//


                        bool _chkflag = _captainModel.UserProfileAccess.checkUseremailMobile(txtUserName.Text, "CHKEMAILMOBILE");
                        if (_chkflag)
                        {
                            //ForgotPassword forgotPassword = new ForgotPassword("STP1");
                            //forgotPassword.StartPosition = FormStartPosition.CenterParent;
                            //forgotPassword.ShowDialog();

                            RandomTokenNumber(6);

                            if (dt.Rows[0]["PWR_OPT_IN"].ToString() != "")
                            {
                                if (_forgotMobileno != "")
                                {
                                    //if (strURL.ToLower().ToString() == "http://localhost:53370/" || strURL.ToLower().ToString() == "https://capsysdev.capsystems.com/captainwisej/")
                                    //{
                                    //    if (_forgotUserID.ToUpper() == "SYSTEM")
                                    SendMobileSMS(_forgotMobileno, proptext);
                                    if (_forgotEmail != "")
                                    {
                                        lblSendCodeby.Visible = true;
                                        lblgetcodebyemail.Visible = true; lblgetcodebyemail.Text = "Get Code via email";
                                    }
                                    //}
                                }
                                else if (_forgotEmail != "")
                                {
                                    var model = new CaptainModel();
                                    var AgencyControlDetails = model.ZipCodeAndAgency.GetAgencyControlFile("00");
                                    //AgencyName = AgencyControlDetails.AgyName.Trim();

                                    HierarchyEntity hierarchyDetails = model.HierarchyAndPrograms.GetCaseHierarchy("AGENCYNAME", userProfile.Agency, string.Empty, string.Empty, string.Empty, string.Empty);
                                    AgencyName = hierarchyDetails.HirarchyName.Trim();

                                    var AgyCntrlDets = model.ZipCodeAndAgency.GetAgencyControlFile(userProfile.Agency);
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

                                    _officeAddress = strStreet + strCity + (strCity != "" ? ("<br/>" + strState) : strState) + strZIP;

                                    SendEmail(_forgotEmail, proptext, AgencyName, _officeAddress, _forgotOptinDate, _forgotMobileno);
                                }
                            }
                            else
                            {
                                if (_forgotEmail != "")
                                {
                                    var model = new CaptainModel();
                                    var AgencyControlDetails = model.ZipCodeAndAgency.GetAgencyControlFile("00");
                                    AgencyName = AgencyControlDetails.AgyName;
                                    if (userProfile != null)
                                    {
                                        HierarchyEntity hierarchyDetails = model.HierarchyAndPrograms.GetCaseHierarchy("AGENCYNAME", userProfile.Agency, string.Empty, string.Empty, string.Empty, string.Empty);
                                        AgencyName = hierarchyDetails.HirarchyName.Trim();
                                    }
                                    var AgyCntrlDets = AgencyControlDetails;
                                    if (userProfile != null)
                                        AgyCntrlDets = model.ZipCodeAndAgency.GetAgencyControlFile(userProfile.Agency);

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

                                    _officeAddress = strStreet + strCity + (strCity != "" ? ("<br/>" + strState) : strState) + strZIP;

                                    SendEmail(_forgotEmail, proptext, AgencyName, _officeAddress, _forgotOptinDate, _forgotMobileno);
                                }
                                if (_forgotMobileno != "")
                                {
                                    lblgetcodebyemail.Visible = true; lblgetcodebyemail.Text = "Get Code via Phone";
                                }
                            }



                            if (_isemailSent == "Y" || _isMobileSent == "Y")
                            {
                                label3.Visible = true;
                                pnlUserlogin.Visible = false;
                                this.Size = new Size(630, this.Size.Height + pnlAuthentication.Height - pnlUserlogin.Height);
                                pnlauthblock.Visible = true;
                                pnlAuthentication.Visible = true;
                                lblTimerLeft.Visible = true;
                                txtverifytext.Visible = true; lblEntertext.Visible = true; btnValidCaptcher.Visible = true; chktrusted.Visible = false; lblTimerLeft.Visible = true;
                                btnLogin.Enabled = false; linkresend.Visible = false; linktryanotheruser.Visible = lblOnetime2.Visible = true;
                                txtPassword.Enabled = false; txtUserName.Enabled = false;
                                linktryanotheruser.Location = new Point(345, 164);
                                inttimelife = 120;

                                lblOTPUserID.Text = _forgotUserID;
                                lblOTPsentdate.Text = DateTime.Now.ToString();

                                if (_isemailSent == "Y")
                                {
                                    lblOTPMessage.Text = "Check your email for the code";
                                    lblOTPMessage.Tag = "email";
                                    lblOnetime2.Text = " " + MaskEmailfunction(_forgotEmail);
                                }
                                else if (_isMobileSent == "Y")
                                {
                                    lblOTPMessage.Text = "Check your phone for the code";
                                    lblOTPMessage.Tag = "phone";
                                    lblOnetime2.Text = " " + CommonFunctions.MaskMobilefunction(_forgotMobileno.ToString());
                                }

                                timer1.Start();
                                timer1.Enabled = true;
                            }
                        }
                        else
                        {
                            AlertBox.Show(_AlertMsg, Wisej.Web.MessageBoxIcon.Warning);
                        }
                    }
                    else
                        AlertBox.Show("User ID not Valid", Wisej.Web.MessageBoxIcon.Warning);

                }
                else
                    AlertBox.Show("User ID not Valid", Wisej.Web.MessageBoxIcon.Warning);

            }
            else
            {
                AlertBox.Show("Please enter User ID", MessageBoxIcon.Warning);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(lblverifyicon, null);
            timer1.Stop();
            lblTimerLeft.Visible = false;
            txtverifytext.Visible = false; lblEntertext.Visible = false; btnValidCaptcher.Visible = linkresend.Visible = lblOnetime2.Visible = linktryanotheruser.Visible = false; chktrusted.Visible = false; lblTimerLeft.Visible = false;
            btnLogin.Enabled = true; txtUserName.Enabled = true; txtPassword.Enabled = true; userProfile = null; txtUserName.Text = ""; txtPassword.Clear();

            pnlAuthentication.Visible = false;
            pnlUserlogin.Visible = true;
            this.Size = new System.Drawing.Size(530, 300);
        }

        private void lblgetcodebyemail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _isemailSent = "N"; _isMobileSent = "N";

            if (VerifyFrom == "frmLogin")
            {
                _forgotEmail = userProfile.PWDEmail;
                _forgotUserID = userProfile.UserID;
                _forgotOptinDate = userProfile.OptInDATE;
                _forgotMobileno = userProfile.PWRMobile;
                chktrusted.Visible = true;
            }

            if (lblgetcodebyemail.Text.ToLower() == "get code via email")
            {
                if (_forgotEmail != "")
                {
                    RandomTokenNumber(6);
                    var model = new CaptainModel();
                    var AgencyControlDetails = model.ZipCodeAndAgency.GetAgencyControlFile("00");
                    //AgencyName = AgencyControlDetails.AgyName.Trim();

                    HierarchyEntity hierarchyDetails = model.HierarchyAndPrograms.GetCaseHierarchy("AGENCYNAME", userProfile.Agency, string.Empty, string.Empty, string.Empty, string.Empty);
                    AgencyName = hierarchyDetails.HirarchyName.Trim();

                    var AgyCntrlDets = model.ZipCodeAndAgency.GetAgencyControlFile(userProfile.Agency);
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

                    _officeAddress = strStreet + strCity + (strCity != "" ? ("<br/>" + strState) : strState) + strZIP;

                    SendEmail(_forgotEmail, proptext, AgencyName, _officeAddress, _forgotOptinDate, _forgotMobileno);

                    if (_isemailSent == "Y")
                    {
                        AlertBox.Show("Verification code send to your emailid");
                        _errorProvider.SetError(lblverifyicon, null);
                        txtverifytext.Text = "";
                        lblOTPMessage.Text = "Check your email for the code";
                        lblOTPMessage.Tag = "email";
                        lblOnetime2.Text = " " + MaskEmailfunction(_forgotEmail);
                        lblOTPsentdate.Text = DateTime.Now.ToString();
                        lblOnetime2.Visible = true; txtverifytext.Visible = true; lblEntertext.Visible = true; lblTimerLeft.Visible = true; chktrusted.Visible = false;
                        btnValidCaptcher.Visible = true;

                        inttimelife = 120;

                        lblOTPUserID.Text = _forgotUserID;
                        lblOTPsentdate.Text = DateTime.Now.ToString();

                        lblOTPMessage.Tag = "email";
                        lblOTPMessage.Text = "Check your email for the code";
                        lblOnetime2.Text = " " + MaskEmailfunction(_forgotEmail);

                        if (_forgotMobileno != "")
                        {
                            lblSendCodeby.Visible = false;
                            lblgetcodebyemail.Visible = true;
                            lblgetcodebyemail.Text = "Get Code via Phone";
                        }
                        else
                        {
                            lblSendCodeby.Visible = false;
                            lblgetcodebyemail.Visible = false;
                        }


                        if (VerifyFrom == "frmLogin")
                            chktrusted.Visible = true;

                        timer1.Start();
                        timer1.Enabled = true;
                    }
                    else
                    {
                        AlertBox.Show("email not sent", Wisej.Web.MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    AlertBox.Show("Emailid not found", Wisej.Web.MessageBoxIcon.Warning);
                }
            }
            else if (lblgetcodebyemail.Text.ToLower() == "get code via phone")
            {
                if (_forgotMobileno != "")
                {
                    RandomTokenNumber(6);
                    if (CommonFunctions.UpdateOPTDate(txtUserName.Text, "A"))
                        SendMobileSMS(_forgotMobileno, proptext);

                    if (_isMobileSent == "Y")
                    {
                        AlertBox.Show("Verification code send to your Mobile no");
                        var strErrorMsg = string.Empty;
                        var model = new CaptainModel();
                        userProfile = model.AuthenticateUser.AuthenticateWithProfile(txtUserName.Text, txtPassword.Text, string.Empty,
                                                                      out strErrorMsg);
                        _errorProvider.SetError(lblverifyicon, null);
                        txtverifytext.Text = "";
                        lblOTPMessage.Text = "Check your phone for the code";
                        lblOTPMessage.Tag = "phone";
                        lblOnetime2.Text = " " + CommonFunctions.MaskMobilefunction(_forgotMobileno.ToString());
                        lblOTPsentdate.Text = DateTime.Now.ToString();
                        lblOnetime2.Visible = true; txtverifytext.Visible = true; lblEntertext.Visible = true; lblTimerLeft.Visible = true; chktrusted.Visible = false;
                        btnValidCaptcher.Visible = true;

                        inttimelife = 120;

                        lblOTPUserID.Text = _forgotUserID;
                        lblOTPsentdate.Text = DateTime.Now.ToString();

                        lblSendCodeby.Visible = true;
                        lblgetcodebyemail.Visible = true;

                        lblgetcodebyemail.Text = "Get code via email";
                        //lblOTPMessage.Tag = "email";
                        //lblOTPMessage.Text = "Check your email for the code";
                        //lblOnetime2.Text = " " + MaskEmailfunction(_forgotEmail);

                        if (VerifyFrom == "frmLogin")
                            chktrusted.Visible = true;

                        timer1.Start();
                        timer1.Enabled = true;
                    }
                    else
                    {
                        AlertBox.Show("email not sent", Wisej.Web.MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    AlertBox.Show("Mobile no not found", Wisej.Web.MessageBoxIcon.Warning);
                }
            }
        }


        private string chkZendeskURL()
        {
            string _Res = "";
            
            if (Application.Uri.Query.Contains("return_to"))
            {
                string zendeskUrl = Application.QueryString["return_to"].ToString();
                _Res = CommonFunctions.ZendeskLogin(zendeskUrl);
            }
            return _Res;
        }

        //private void pnlauthblock_VisibleChanged(object sender, EventArgs e)
        //{
        //	if (pnlauthblock.Visible)
        //	{
        //		SendEmail(userProfile.PWDEmail, proptext);
        //		txtverifytext.Visible = true;
        //		lblEntertext.Visible = true;
        //		btnValidCaptcher.Visible = true;
        //		btnLogin.Enabled = false;
        //		linkresend.Visible = false;
        //		linktryanotheruser.Visible = lblOnetime.Visible = true;
        //		txtPassword.Enabled = false;
        //		txtUserName.Enabled = false;
        //		inttimelife = 1;
        //		//txtverifyText.Watermark = "60 seconds left";
        //		lblOnetime.Text = "One time Text sent to your email id :" +
        //						MaskEmailfunction(userProfile.PWDEmail);
        //		timer1.Start();
        //	}
        //	else
        //	{
        //		_errorProvider.SetError(btnValidCaptcher, null);
        //		txtverifytext.Visible = false;
        //		lblEntertext.Visible = false;
        //		btnValidCaptcher.Visible = linkresend.Visible = lblOnetime.Visible = linktryanotheruser.Visible = false;
        //		btnLogin.Enabled = true;
        //		txtUserName.Enabled = true;
        //		txtPassword.Enabled = true;
        //		userProfile = null;
        //		txtUserName.Text = "";
        //		txtPassword.Clear();
        //	}
        //}
    }
}

class verifyDevice
{

    public string username { get; set; }
    public string password { get; set; }
    public string trust { get; set; }

}
public class OPTinParams
{
    public string username { get; set; }
    public string optStatus { get; set; }
}