using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Wisej.Web;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Menus;
using Captain.Common.Views.Forms;
using System.Data.SqlClient;
using Captain.Common.Views.Controls;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;

namespace Captain
{
    public partial class ChangePassword : Form
    {
        CaptainModel model;
        public ChangePassword(BaseForm baseForm)
        {
            InitializeComponent();
            BaseForm = baseForm;
            model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            this.Size = new System.Drawing.Size(434, 226);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            pnlResetPswrd.Visible = false;
        }

        public ChangePassword(BaseForm baseForm, string str)
        {
            InitializeComponent();
            model = new CaptainModel();
            if (str.ToUpper() == "AGCYCNTL")
            {
                //this.pnlResetPswrd.Location = new System.Drawing.Point(0, 0);
                //              this.panel3.Size = new System.Drawing.Size(248, 126);
                //this.Size = new System.Drawing.Size(250, 180);

                this.Height = this.Height - (pnlChangePswrd.Height);
                //this.Width = 270;
                this.Width = pnlResetPswrd.Width;
                this.Text = "Reset Password Setup";
                pnlChangePswrd.Visible = false;
                pnlResetPswrd.Visible = true;
                pnlResetPswrd.BringToFront();
                //  txtpwdDays.Validator = TextBoxValidation.IntegerValidator;
                AgencyControlEntity AgencyControlDetails = model.ZipCodeAndAgency.GetAgencyControlFile("00");
                if (AgencyControlDetails != null)
                {
                    if (AgencyControlDetails.ForcePwd.ToUpper() == "Y")
                        chkForcepwd.Checked = true;
                    else
                        chkForcepwd.Checked = false;
                    txtpwdDays.Text = AgencyControlDetails.ForcePwdDays.ToString();
                }
            }
            else if (str.ToUpper() == "STARTCHNG")
            {
                pnlResetPswrd.Visible = false;
                pnlChangePswrd.Visible = true;
                //this.Size = new System.Drawing.Size(434, 226);
                this.Height = this.Height - (pnlResetPswrd.Height);
                this.Width = pnlChangePswrd.Width;
                //this.FormBorderStyle = Wisej.Web.FormBorderStyle.None;
                this.CloseBox = false;//false;
            }
            else
            {
                pnlResetPswrd.Visible = false;
                pnlChangePswrd.Visible = true;
                //this.Size = new System.Drawing.Size(434, 226);
                this.Height = this.Height - (pnlResetPswrd.Height);
                this.Width = pnlChangePswrd.Width;
                //this.FormBorderStyle = Wisej.Web.FormBorderStyle.None;
                this.CloseBox = true;//false;
            }
            BaseForm = baseForm;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
        }

        public BaseForm BaseForm { get; set; }
        private ErrorProvider _errorProvider = null;
        private void btnChangePassword_Click(object sender, EventArgs e)
        {

            if (!isValidate()) return;
            AgencyControlEntity AgencyControlDetails = model.ZipCodeAndAgency.GetAgencyControlFile("00");
            if (AgencyControlDetails != null)
            {
                bool boolvalid = true;
                if (txtNewPassword.Text.ToUpper().Contains(AgencyControlDetails.AgyShortName.ToUpper()))
                {
                    boolvalid = false;
                }
                if (txtNewPassword.Text.ToUpper().Contains(BaseForm.UserID.ToUpper()))
                {
                    boolvalid = false;

                }
                if (boolvalid)
                {
                    if (txtNewPassword.Text == txtConfirmPassword.Text)
                    {
                        string strMsg = model.UserProfileAccess.UpdatePassword(BaseForm.UserID, txtOldPassword.Text.Trim(), txtNewPassword.Text.Trim());
                        if (strMsg == "success")
                        {
                            CommonFunctions.MessageBoxDisplay("Password successfully updated..  ");
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
        }


        private bool isValidate()
        {
            bool isValid = true;


            if (String.IsNullOrEmpty(txtOldPassword.Text))
            {
                _errorProvider.SetError(txtOldPassword, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblOldPassword.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtOldPassword, null);
            }

            if (String.IsNullOrEmpty(txtNewPassword.Text.Trim()))
            {
                _errorProvider.SetError(txtNewPassword, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblNewPassword.Text));
                isValid = false;
            }
            else
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtNewPassword.Text, Consts.StaticVars.PasswordRegulation))
                {
                    _errorProvider.SetError(txtNewPassword, "Password should have minimum 8 characters and have atleast 1 Alphabet, 1 Number and 1 Special Character");
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
                    //**_errorProvider.SetError(txtNewPassword, "Minimum 8 characters atleast 1 Alphabet, 1 Number and 1 Special Character");
                    _errorProvider.SetError(txtNewPassword, "Password should have minimum 8 characters and have atleast 1 Alphabet, 1 Number and 1 Special Character");
                }
                else
                {
                    _errorProvider.SetError(txtNewPassword, null);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPwdUpdate_Click(object sender, EventArgs e)
        {
            bool boolvalid = true;
            if (chkForcepwd.Checked)
            {
                _errorProvider.SetError(lblDays, null);
                if (txtpwdDays.Text != string.Empty)
                {
                    if (Convert.ToInt32(txtpwdDays.Text) <= 0)
                    {
                        _errorProvider.SetError(lblDays, "please enter greater than zero");
                        boolvalid = false;
                    }
                }
                else
                {
                    _errorProvider.SetError(lblDays, "please enter days");
                    boolvalid = false;
                }
            }
            if (boolvalid)
            {
                AgencyControlEntity agencycontrol = new AgencyControlEntity();
                agencycontrol.AgencyCode = "00";
                agencycontrol.ForcePwd = chkForcepwd.Checked == true ? "Y" : "N";
                agencycontrol.ForcePwdDays = txtpwdDays.Text;
                agencycontrol.Mode = "PASSWORD";
                if (model.ZipCodeAndAgency.UpdateXMLAGCYCNTL(agencycontrol))
                    this.Close();
            }

        }


    }
}