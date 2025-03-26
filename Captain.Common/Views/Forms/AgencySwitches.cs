#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Wisej.Design;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class AgencySwitches : Form
    {

        #region private variables
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion
        /*Added by Vikash 12/30/2022 for Followup issue*/
        public AgencySwitches(BaseForm baseForm, string mode, PrivilegeEntity privilege, string strService, string strShowcase, string streditzip, string strclearreport,
            string strTmsB20, string strCApVoucher, string strTaxexemption, string strIncVerification, string strInccntl18noinc, string strIncMethod3, string strCaseNotesstamp,
            string strAgencyCode, string strCilentInq, string strMatAssess, string strSsn, string strDob, string strLastName, string strFirstName, string strClientRules,
            string strRefConn, string strSsnPoint, string strDobPoint, string strLastNamePoint, string strFirstNamePoint, string strDOBLastNamePoint, string strSSNLastNamePoint,
            string strSearchhit, string strSearhRating, string strSiteSec, string strDelAppSwitch, string strDefIntakeDtSwitch, string strDOBFirstNamePoint, string strVerSwitch,
            string strCAOBO, string strProgressNotes, string strdeepSearch, string strMailAddress, string strFamilyTypeswitch, string strHealthInsSingleSel,
            string strWorkerFUPswitch, string strQuickPostSer, string strPaymentVoucher, string strWipeBmalps,
            string strBenefitfrom, string PostUsage, AgencyControlEntity _agencyControlFrom, string Search_Cntl, string POVDesc, string POVdontRoundup)
        {
            InitializeComponent();
            BaseForm = baseForm;
            PrivilegEntity = privilege;
            Mode = mode;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            FillCombo();
            pnlIncomeSwitch.Visible = false;
            chkCaseNotesDate.Visible = false;
            chkbAllowClientInquiry.Visible = false;
            chkMatassessment.Visible = false;
            chkbSiteSec.Visible = false;
            chkDeleteApp.Visible = false;
            chkbCAOBO.Visible = false;
            chkProgressNotes.Visible = false;
            chkDeepSearch.Visible = false;
            chkMailaddress.Checked = false;
            chkMailaddress.Visible = false;
            chkFamilyType.Visible = false;
            cbQkPost.Checked = false;
            cbQkPost.Visible = false;
            chkWipebmps.Checked = false;
            chkWipebmps.Visible = false;
            pnlBenefitfrom.Visible = false;
            chkMemberActivity.Visible = false;
            chkTms201SEdit.Visible = false;
            chkOnlyIntakeHie.Visible = false;
            chkmostrecent.Visible = false;
            chkServicePlancont.Visible = false;
            chkLoginMfa.Visible = false;
            chkReversFeed.Visible = false;
            chkbAgyVend.Visible = false;
            pnlservicesame.Visible = false;
            pnlshowmenugrid.Visible = false;
            chkBulkposttemplate.Visible = false;
            chkCaseworkerFollowup.Visible = false;
            chkLnkApplicant.Visible = false;

            pnlTXAlienControls.Visible = false;
            chkTXAlienSwitch.Checked = false;
            chkHealthInsSingleSel.Checked = false;
            chkbPostUsage.Checked = false;
            //chkbStrtChk.Checked = false;

            if (BaseForm.UserID.ToUpper() == "JAKE")
            {
                if (strAgencyCode == "00")
                {
                    pnlIncomeSwitch.Visible = true;
                    chkCaseNotesDate.Visible = true;
                    chkMatassessment.Visible = true;
                    //  chkServiceInquiring.Visible = true;
                    chkbAllowClientInquiry.Visible = true;
                    chkbSiteSec.Visible = true;
                    chkDeleteApp.Visible = true;
                    grpClientRules.Visible = true;
                    chkClientRules.Visible = true;
                    ChkRefConn.Visible = true;
                    chkDefIntakedtswitch.Visible = true;
                    //  txtintakeDrag.Visible = true;
                    chkbCAOBO.Visible = true;
                    chkProgressNotes.Visible = true;
                    chkDeepSearch.Visible = true;
                    chkMailaddress.Visible = true;
                    chkFamilyType.Visible = true;
                    cbQkPost.Visible = true;
                    chkWipebmps.Visible = true;
                    chkMemberActivity.Visible = true;
                    chkTms201SEdit.Visible = true;
                    chkOnlyIntakeHie.Visible = true;
                    chkmostrecent.Visible = true;
                    chkServicePlancont.Visible = true;
                    chkLoginMfa.Visible = true;
                    chkReversFeed.Visible = true;
                    chkbAgyVend.Visible = true;

                    txtDobPoint.Validator = TextBoxValidation.IntegerValidator;
                    txtSSNPoint.Validator = TextBoxValidation.IntegerValidator;
                    txtFirstNamePoint.Validator = TextBoxValidation.IntegerValidator;
                    txtLastNamePoint.Validator = TextBoxValidation.IntegerValidator;
                    txtDobLastName.Validator = TextBoxValidation.IntegerValidator;
                    txtSSNLastName.Validator = TextBoxValidation.IntegerValidator;
                    txtSearchRating.Validator = TextBoxValidation.IntegerValidator;
                    //added by sudheer on 02/21/2018
                    txtDobFirstName.Validator = TextBoxValidation.IntegerValidator;


                    txtClidYear.Validator = TextBoxValidation.IntegerValidator;
                    chkClidSmash.Visible = true;
                    if (chkClidSmash.Checked)
                    {
                        txtClidYear.Visible = true;
                        pnlIntegrity.Visible = true;
                        pnlClientIdswitch.Visible = true;
                        lblClidYear.Visible = true;
                        this.Size = new Size(1108, 672);
                    }
                    else
                    {
                        txtClidYear.Visible = false;
                        pnlClientIdswitch.Visible = false;
                        pnlIntegrity.Visible = false;
                        lblClidYear.Visible = false;
                        pnlIntegrity.Visible = false;
                        this.Size = new Size(1108, 645);
                    }

                    if (baseForm.BaseAgencyControlDetails.State == "TX" || baseForm.BaseAgencyControlDetails.State == "IN")
                    {
                        label1.Text = baseForm.BaseAgencyControlDetails.State + " " + "Controls";
                        pnlTXAlienControls.Visible = true;
                        chkTXAlienSwitch.Checked = false;
                        chkbPostUsage.Checked = false;
                        //chkbStrtChk.Checked= false;

                    }

                    pnlDateStamp.Visible = true;
                    pnlClientiddis.Visible = true;
                    pnlFamilyidControls.Visible = true;
                    pnlServiceControls.Visible = true;
                    pnlMisellanous.Visible = true;
                    pnlMainmenu.Visible = true;
                    pnlClientintake.Visible = true;
                    pnlshowmenugrid.Visible = true;
                    chkBulkposttemplate.Visible = true;
                    chkCaseworkerFollowup.Visible = true;
                    chkLnkApplicant.Visible = true;
                    //grpbMemberDate.Visible = true;
                    //txtClidYear.Visible = true;
                    //lblClidYear.Visible = true;
                }

            }

            //  lblHeader.Text = "Agency Switches";
            if (Mode.Equals("Add"))
            {
                this.Text = "Agency Switches" + " - Add";
                this.Size = new Size(1108, 635);
            }
            else
            {
                this.Text = "Agency Switches" + " - Edit";
                this.Size = new Size(1108, 645);
                TmsB20 = strTmsB20;
            }
            ServiceEnquiry = strService;
            ClearReport = strclearreport;
            EditZip = streditzip;
            ShowCaseManager = strShowcase;
            TmsB20 = strTmsB20;
            CAPVoucher = strCApVoucher;
            TaxExemption = strTaxexemption;
            IncVerfication = strIncVerification;
            Inccntl18noinc = strInccntl18noinc;
            VerSwitch = strVerSwitch;
            IncMethods = strIncMethod3;
            CaseNotesstamp = strCaseNotesstamp;
            ClientInquiry = strCilentInq;
            SiteSecurity = strSiteSec;
            CAOBO = strCAOBO;
            ProgressNotesSwitch = strProgressNotes;
            DeepSearchSwitch = strdeepSearch;
            DelAppSwitch = strDelAppSwitch;
            DefIntakeDtSwitch = strDefIntakeDtSwitch;
            Matassesment = strMatAssess;
            SSN = strSsn;
            DOB = strDob;
            FirstName = strFirstName;
            LastName = strLastName;
            ClientRules = strClientRules;
            txtSSNPoint.Text = SSNPoint = strSsnPoint;
            txtDobPoint.Text = DOBPoint = strDobPoint;
            txtFirstNamePoint.Text = FirstNamePoint = strFirstNamePoint;
            txtLastNamePoint.Text = LastNamePoint = strLastNamePoint;
            txtDobLastName.Text = DOBLastNamePoint = strDOBLastNamePoint;
            txtSSNLastName.Text = SSNLastNamePoint = strSSNLastNamePoint;//added by sudheer on 02/28/2018
            txtDobFirstName.Text = DOBFirstNamePoint = strDOBFirstNamePoint;
            RefConn = strRefConn;
            SearchHit = strSearchhit;
            SearchRating = strSearhRating;
            MailAddressSwitch = strMailAddress;
            FTypeSwitch = strFamilyTypeswitch;
            HealthInsSingleSel = strHealthInsSingleSel;
            CEAPPostUsage = PostUsage;
            //CEAPStrtChk = StrtChk;
            WorkerFUPSwitch = strWorkerFUPswitch;   // Added by Vikash 12/30/2022 for Followup issue
            QuickPostServ = strQuickPostSer;
            WorkerFUPSwitch =
            PaymentVoucherNumber = strPaymentVoucher;
            WipeBMALSP = strWipeBmalps;
            BenefitFrom = strBenefitfrom;
            PropAgencyControlEntity = _agencyControlFrom;

            Search_Cntrl = Search_Cntl;
            if (Search_Cntrl == "F")
                rdbFNDOBSSN.Checked = true;
            else if(Search_Cntrl == "C")
                rdbClientID.Checked = true;
            else
                rdbFNDOBSSN.Checked = true;

            POV_With_Desc = POVDesc;
            if (POV_With_Desc == "Y")
                chkbPOV_With_Desc.Checked = true;
            else
                chkbPOV_With_Desc.Checked = false;

            POV_Dont_Roundup = POVdontRoundup;
            if (POV_Dont_Roundup == "Y")
                chkbPOV_Dont_Roundup.Checked = true;
            else
                chkbPOV_Dont_Roundup.Checked = false;

            //if (ServiceEnquiry == "1")
            //    chkServiceInquiring.Checked = true;
            //if (ClearReport == "1")
            //    chkClearReport.Checked = true;
            //if (EditZip == "1")
            //    chkEditZipandCity.Checked = true;
            if (ShowCaseManager == "1")
                chkShowCaseManager.Checked = true;
            //if (CAPVoucher.ToUpper() == "Y")
            //    chkcapvoucher.Checked = true;
            if (CaseNotesstamp.ToUpper() == "Y")
                chkCaseNotesDate.Checked = true;
            if (string.IsNullOrEmpty(IncMethods))
                IncMethods = "1";
            if (IncMethods.ToUpper() == "1")
                rdbMethod1.Checked = true;
            if (IncMethods.ToUpper() == "2")
                rdbMethod2.Checked = true;
            if (IncMethods.ToUpper() == "3")
                rdbMethod3.Checked = true;
            if (IncMethods.ToUpper() == "4")
                rdbMethod4.Checked = true;
            if (IncVerfication.ToUpper() == "Y")
            {
                chkIncVerification.Checked = true;
                chkIncVerSwitch.Enabled = true;
            }
            else
            {
                chkIncVerSwitch.Enabled = false;
                chkIncVerSwitch.Checked = false;

            }

            if (Inccntl18noinc.ToUpper() == "Y")
                chkInccntl18noinc.Checked = true;
            else
                chkInccntl18noinc.Checked = false;


            if (ClientInquiry == "1")
                chkbAllowClientInquiry.Checked = true;
            if (SiteSecurity == "1")
                chkbSiteSec.Checked = true;
            if (DelAppSwitch == "Y")
                chkDeleteApp.Checked = true;
            if (DefIntakeDtSwitch == "Y")
                chkDefIntakedtswitch.Checked = true;
            if (VerSwitch.ToUpper() == "Y")
                chkIncVerSwitch.Checked = true;
            if (CAOBO == "Y")
            {
                chkbCAOBO.Checked = true;
                if (BaseForm.UserID.ToUpper() == "JAKE")
                {
                    pnlBenefitfrom.Visible = true;
                    CommonFunctions.SetComboBoxValue(cmbBenefitFrom, BenefitFrom);
                }
            }
            else
            {
                pnlBenefitfrom.Visible = false;
            }
            if (ProgressNotesSwitch == "Y")
                chkProgressNotes.Checked = true;
            if (DeepSearchSwitch == "Y")
                chkDeepSearch.Checked = true;



            if (Matassesment.ToUpper() == "Y")
                chkMatassessment.Checked = true;
            if (SSN.ToUpper() == "Y")
                chkSsn.Checked = true;
            if (DOB.ToUpper() == "Y")
                chkDob.Checked = true;
            if (FirstName.ToUpper() == "Y")
                chkFirstName.Checked = true;
            if (LastName.ToUpper() == "Y")
                chkLastName.Checked = true;
            if (ClientRules.ToUpper() == "Y")
                chkClientRules.Checked = true;
            if (RefConn.ToUpper() == "Y")
                ChkRefConn.Checked = true;
            txtSearchRating.Visible = false;
            CommonFunctions.SetComboBoxValue(cmbNohits, SearchHit);

            if (SearchHit == "3")
            {
                txtSearchRating.Text = SearchRating;
            }

            if (MailAddressSwitch == "Y")
                chkMailaddress.Checked = true;
            if (FTypeSwitch == "Y")
                chkFamilyType.Checked = true;

            if (strQuickPostSer == "Y")
                cbQkPost.Checked = true;

            if (strWipeBmalps == "Y")
                chkWipebmps.Checked = true;

            if (HealthInsSingleSel == "Y")
                chkHealthInsSingleSel.Checked = true;

            if (CEAPPostUsage == "Y")
                chkbPostUsage.Checked = true;

            //if (CEAPStrtChk == "Y")
            //    chkbStrtChk.Checked = true;

            //chkbPostUsage.Checked = _agencyControlFrom.CEAPPostUsage == "Y" ? true : false;

            if (_agencyControlFrom != null)
            {
                if (_agencyControlFrom.ClidSmash == "Y")
                {
                    chkClidSmash.Checked = true;
                    txtClidYear.Text = _agencyControlFrom.ClidYear;
                    if (_agencyControlFrom.ClidFrom != string.Empty)
                    {
                        dtClidFrom.Value = Convert.ToDateTime(_agencyControlFrom.ClidFrom);
                        dtClidFrom.Checked = true;
                    }
                    if (_agencyControlFrom.ClidTo != string.Empty)
                    {
                        dtClidTo.Value = Convert.ToDateTime(_agencyControlFrom.ClidTo);
                        dtClidTo.Checked = true;
                    }
                    chkClidSSN.Checked = _agencyControlFrom.ClidSSN == "Y" ? true : false;
                    chkClidClid.Checked = _agencyControlFrom.ClidClid == "Y" ? true : false;
                    if (_agencyControlFrom.ClidDateStamp == "2")
                        rdoClidAllChanged.Checked = true;
                    else
                        rdoClidMostRecent.Checked = true;
                    txtClidYear.Visible = true;
                    pnlIntegrity.Visible = true;
                    pnlClientIdswitch.Visible = true;
                    lblClidYear.Visible = true;

                }
                else
                {
                    if (_agencyControlFrom.ClidDateStamp == "2")
                        rdoClidAllChanged.Checked = true;
                    else
                        rdoClidMostRecent.Checked = true;

                }
                if (_agencyControlFrom.FamilyIdHie == "Y")
                {
                    chkFamilyIdHie.Checked = _agencyControlFrom.FamilyIdHie == "Y" ? true : false;
                    chkFamilyIdDupblevel.Checked = _agencyControlFrom.FamilyIdDuplvl == "Y" ? true : false;
                }
                else
                {
                    chkFamilyIdDupblevel.Visible = false;
                    chkFamilyIdDupblevel.Checked = false;
                }
                chkMemberActivity.Checked = _agencyControlFrom.MemberActivity == "Y" ? true : false;
                chkTms201SEdit.Checked = _agencyControlFrom.TMS201SoftEdit == "Y" ? true : false;
                chkOnlyIntakeHie.Checked = _agencyControlFrom.ShowIntakeSwitch == "Y" ? true : false;
                chkmostrecent.Checked = _agencyControlFrom.MostRecentintake == "Y" ? true : false;
                chkServicePlancont.Checked = _agencyControlFrom.ServicePlanHiecontrol == "Y" ? true : false;
                //chkCalQues.Checked = _agencyControlFrom.CalQuescontprogram == "Y" ? true : false;
                chkLoginMfa.Checked = _agencyControlFrom.LoginMFA == "Y" ? true : false;

                chkReversFeed.Checked = _agencyControlFrom.ReverseFeed == "Y" ? true : false;

                chkTXAlienSwitch.Checked = _agencyControlFrom.TXAlienSwitch == "Y" ? true : false;
                

                chkbAgyVend.Checked = _agencyControlFrom.AgyVendor == "Y" ? true : false;
                pnlservicesame.Visible = false;
                if (_agencyControlFrom.ServicePlanHiecontrol == "Y")
                {
                    pnlservicesame.Visible = true;
                    if (_agencyControlFrom.SerPlanAllow == "A")
                        rdosameagency.Checked = true;
                    else
                        rdosamedept.Checked = true;
                }
                if (_agencyControlFrom.SsnDobMMenu == "D")
                    rdoDOB.Checked = true;
                else
                    rdoSSN.Checked = true;
                chkBulkposttemplate.Checked = _agencyControlFrom.BulkpostTemp == "Y" ? true : false;
                chkCaseworkerFollowup.Checked = _agencyControlFrom.WorkerFUP == "Y" ? true : false;
                chkLnkApplicant.Checked = _agencyControlFrom.LnkAppSwitch == "Y" ? true : false;


            }
            if (privilege.Program.ToUpper() == "ADMN0009")
            {
                this.Size = new Size(300, 300);
                // pictureBox2.Location = new Point(270, 14);
                btnOk.Location = new Point(137, 268);
                btnCancel.Location = new Point(212, 268);
                pnlCompleteForm.Size = new Size(295, 295);
                pnlAgencySwitches.Size = new Size(287, 210);


            }
        }

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity PrivilegEntity { get; set; }

        public string Mode { get; set; }

        public string ServiceEnquiry { get; set; }
        public string ShowCaseManager { get; set; }
        public string EditZip { get; set; }
        public string ClearReport { get; set; }
        public string TmsB20 { get; set; }
        public string CAPVoucher { get; set; }
        public string TaxExemption { get; set; }
        public string IncVerfication { get; set; }
        public string Inccntl18noinc { get; set; }
        public string IncMethods { get; set; }
        public string CaseNotesstamp { get; set; }
        public string ClientInquiry { get; set; }
        public string Matassesment { get; set; }
        public string SSN { get; set; }
        public string DOB { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SSNPoint { get; set; }
        public string DOBPoint { get; set; }
        public string FirstNamePoint { get; set; }
        public string LastNamePoint { get; set; }
        public string DOBLastNamePoint { get; set; }
        public string SSNLastNamePoint { get; set; }
        public string ClientRules { get; set; }
        public string RefConn { get; set; }
        public string SearchHit { get; set; }
        public string SearchRating { get; set; }
        public string SiteSecurity { get; set; }
        public string DelAppSwitch { get; set; }
        public string DefIntakeDtSwitch { get; set; }
        public string PrintBatchNo { get; set; }
        public string VerSwitch { get; set; }
        public string CAOBO { get; set; }
        public string ProgressNotesSwitch { get; set; }
        public string DeepSearchSwitch { get; set; }
        public string QuickPostServ { get; set; }

        public string PaymentVoucherNumber { get; set; }
        public string WipeBMALSP { get; set; }

        //added by sudheer on 02/21/2018
        public string DOBFirstNamePoint { get; set; }

        public string MailAddressSwitch { get; set; }
        public string WorkerFUPSwitch { get; set; }     // Added by Vikash 12/30/2022 for Followup issue
        public string FTypeSwitch { get; set; }
        //added by murali on 03/17/2020
        public string BenefitFrom { get; set; }
        // added by Murali on 04/22/2020
        public AgencyControlEntity PropAgencyControlEntity { get; set; }

        public string TXAlienSwitch { get; set; }
        
        public string CEAPPostUsage { get; set; }
        public string HealthInsSingleSel { get; set; }

        public string Search_Cntrl
        {
            get; set;
        }
        public string POV_With_Desc
        {
            get; set;
        }
        public string POV_Dont_Roundup
        {
            get; set;
        }

        //public string CEAPStrtChk { get; set; }

        private void FillCombo()
        {

            cmbBenefitFrom.Items.Add(new ListItem("Applicant", "1"));
            cmbBenefitFrom.Items.Add(new ListItem("All HH Members", "2"));
            cmbBenefitFrom.Items.Add(new ListItem("Selected HH Members", "3"));
            cmbBenefitFrom.SelectedIndex = 0;
            cmbNohits.Items.Add(new ListItem("NO hit on ss# and last name", "1"));
            cmbNohits.Items.Add(new ListItem("NO hit on dob last name", "2"));
            cmbNohits.Items.Add(new ListItem("NO hit on dob first name", "4"));
            cmbNohits.Items.Add(new ListItem("Search rating < a value", "3"));
            cmbNohits.SelectedIndex = 0;

        }

        bool chkAlienForm()
        {
            bool _Flag = true;
            DataSet ds = DatabaseLayer.MainMenu.GET_PRINAPPCNTL(string.Empty, string.Empty, string.Empty, "L");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dr = ds.Tables[0].Select("PAC_FORM='10' AND PAC_ENABLE='Y'");
                    if (dr.Length > 0)
                    {
                        TXAlienSwitch = chkTXAlienSwitch.Checked == true ? "Y" : "N";
                        if (TXAlienSwitch == "N")
                        {
                            _Flag = false;
                        }
                    }
                }

            }
            return _Flag;
        }

        private void btnOkClick(object sender, EventArgs e)
        {

            bool boolvalidate = true;
            if (((ListItem)cmbNohits.SelectedItem).Value.ToString() == "3")
            {
                if (String.IsNullOrEmpty(txtSearchRating.Text.Trim()))
                {
                    _errorProvider.SetError(txtSearchRating, "Search Rating value is requied");
                    boolvalidate = false;
                }
                else
                {
                    _errorProvider.SetError(txtSearchRating, null);
                }
            }
            if (chkClidSmash.Checked && chkClidSmash.Visible == true && BaseForm.UserID == "JAKE")
            {
                _errorProvider.SetError(dtClidTo, null);
                _errorProvider.SetError(dtClidFrom, null);
                _errorProvider.SetError(txtClidYear, null);
                _errorProvider.SetError(chkClidClid, null);

                if (String.IsNullOrEmpty(txtClidYear.Text.Trim()) && dtClidFrom.Checked == false && dtClidTo.Checked == false)
                {
                    _errorProvider.SetError(txtClidYear, "Please fill Dates or Year");
                    boolvalidate = false;
                }
                else
                {
                    _errorProvider.SetError(txtClidYear, null);
                }
                if (txtClidYear.Text != string.Empty)
                {
                    if (Convert.ToInt32(txtClidYear.Text) < 2000)
                    {
                        _errorProvider.SetError(txtClidYear, "Please enter Year is greathan 2000 above");
                        boolvalidate = false;
                    }
                }
                if (boolvalidate)
                {
                    if (dtClidFrom.Checked == false && dtClidTo.Checked == true)
                    {
                        _errorProvider.SetError(dtClidFrom, "Clid From Date requied");
                        boolvalidate = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtClidFrom, null);
                    }
                }
                if (boolvalidate)
                {
                    if (dtClidTo.Checked == false && dtClidFrom.Checked == true)
                    {
                        _errorProvider.SetError(dtClidTo, "Clid To Date requied");
                        boolvalidate = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtClidTo, null);
                    }
                }
                if (chkClidClid.Checked == false && chkClidSSN.Checked == false)
                {
                    _errorProvider.SetError(chkClidClid, "Please select atleast One Integrity checkbox option");
                    boolvalidate = false;
                }

                if (dtClidFrom.Checked.Equals(true) && dtClidTo.Checked.Equals(true))
                {
                    if (string.IsNullOrWhiteSpace(dtClidFrom.Text))
                    {
                        _errorProvider.SetError(dtClidFrom, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Clid From Date".Replace(Consts.Common.Colon, string.Empty)));
                        boolvalidate = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtClidFrom, null);
                    }
                    if (string.IsNullOrWhiteSpace(dtClidTo.Text))
                    {
                        _errorProvider.SetError(dtClidTo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Clid To Date".Replace(Consts.Common.Colon, string.Empty)));
                        boolvalidate = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtClidTo, null);
                    }
                }
                if (boolvalidate)
                {
                    if ((dtClidFrom.Checked == true) && (dtClidTo.Checked == true))
                    {

                        if (dtClidFrom.Value.Date > dtClidTo.Value.Date)
                        {
                            _errorProvider.SetError(dtClidTo, "From Date Greater than To date");
                            boolvalidate = false;
                        }
                        else
                        {
                            _errorProvider.SetError(dtClidTo, null);
                        }

                    }
                }
            }

            if (chkAlienForm())
            {
                if (boolvalidate)
                {
                    //if (chkClearReport.Checked == true)
                    //    ClearReport = "1";
                    //else
                    //    ClearReport = "0";
                    //if (chkEditZipandCity.Checked == true)
                    //    EditZip = "1";
                    //else
                    //    EditZip = "0";
                    //if (chkServiceInquiring.Checked == true)
                    //    ServiceEnquiry = "1";
                    //else
                    //    ServiceEnquiry = "0";
                    if (chkShowCaseManager.Checked == true)
                        ShowCaseManager = "1";
                    else
                        ShowCaseManager = "0";

                    //if (chkcapvoucher.Checked == true)
                    //    CAPVoucher = "Y";
                    //else
                    //    CAPVoucher = "N";

                    if (rdbMethod1.Checked == true)
                        IncMethods = "1";
                    if (rdbMethod2.Checked == true)
                        IncMethods = "2";
                    if (rdbMethod3.Checked == true)
                        IncMethods = "3";
                    if (rdbMethod4.Checked == true)
                        IncMethods = "4";


                    if (chkIncVerification.Checked == true)
                        IncVerfication = "Y";
                    else
                        IncVerfication = "N";

                    if (chkInccntl18noinc.Checked == true)
                        Inccntl18noinc = "Y";
                    else
                        Inccntl18noinc = "N";



                    if (chkCaseNotesDate.Checked == true)
                        CaseNotesstamp = "Y";
                    else
                        CaseNotesstamp = "N";

                    if (chkbAllowClientInquiry.Checked == true)
                        ClientInquiry = "1";
                    else
                        ClientInquiry = "0";

                    if (chkbSiteSec.Checked == true)
                        SiteSecurity = "1";
                    else
                        SiteSecurity = "0";

                    if (chkbCAOBO.Checked == true)
                    {
                        CAOBO = "Y";
                        BenefitFrom = ((ListItem)cmbBenefitFrom.SelectedItem).Value.ToString();
                    }
                    else
                    {
                        CAOBO = "N";
                        BenefitFrom = string.Empty;
                    }

                    if (chkProgressNotes.Checked == true)
                        ProgressNotesSwitch = "Y";
                    else
                        ProgressNotesSwitch = "N";

                    if (chkDeepSearch.Checked == true)
                        DeepSearchSwitch = "Y";
                    else
                        DeepSearchSwitch = "N";

                    if (chkDeleteApp.Checked == true)
                        DelAppSwitch = "Y";
                    else
                        DelAppSwitch = "N";

                    if (chkDefIntakedtswitch.Checked == true)
                        DefIntakeDtSwitch = "Y";
                    else
                        DefIntakeDtSwitch = "N";


                    if (chkMatassessment.Checked == true)
                        Matassesment = "Y";
                    else
                        Matassesment = "N";

                    if (chkSsn.Checked == true)
                        SSN = "Y";
                    else
                        SSN = "N";

                    if (chkDob.Checked == true)
                        DOB = "Y";
                    else
                        DOB = "N";

                    if (chkFirstName.Checked == true)
                        FirstName = "Y";
                    else
                        FirstName = "N";

                    if (chkLastName.Checked == true)
                        LastName = "Y";
                    else
                        LastName = "N";

                    if (chkClientRules.Checked == true)
                        ClientRules = "Y";
                    else
                        ClientRules = "N";

                    if (ChkRefConn.Checked == true)
                        RefConn = "Y";
                    else
                        RefConn = "N";

                    if (chkIncVerSwitch.Checked == true)
                        VerSwitch = "Y";
                    else
                        VerSwitch = "N";


                    //TmsB20 = ((ListItem)cmbTMSB.SelectedItem).Value.ToString();
                    //TaxExemption = txtExemption.Text;
                    //PaymentVoucherNumber = txtPaymentVouch.Text;

                    SSNPoint = txtSSNPoint.Text;
                    DOBPoint = txtDobPoint.Text;
                    FirstNamePoint = txtFirstNamePoint.Text;
                    LastNamePoint = txtLastNamePoint.Text;
                    DOBLastNamePoint = txtDobLastName.Text;
                    //added by sudheer on 02/21/2018
                    DOBFirstNamePoint = txtDobFirstName.Text;
                    SSNLastNamePoint = txtSSNLastName.Text;
                    SearchRating = string.Empty;
                    SearchHit = ((ListItem)cmbNohits.SelectedItem).Value.ToString();
                    if (SearchHit == "3")
                    {
                        SearchHit = "3";
                        SearchRating = txtSearchRating.Text;
                    }


                    MailAddressSwitch = chkMailaddress.Checked == true ? "Y" : "N";
                    FTypeSwitch = chkFamilyType.Checked == true ? "Y" : "N";

                    QuickPostServ = cbQkPost.Checked == true ? "Y" : "N";

                    WipeBMALSP = chkWipebmps.Checked == true ? "Y" : "N";

                    TXAlienSwitch = chkTXAlienSwitch.Checked == true ? "Y" : "N";
                    HealthInsSingleSel = chkHealthInsSingleSel.Checked == true ? "Y" : "N";

                    CEAPPostUsage = chkbPostUsage.Checked == true ? "Y" : "N";

                    //CEAPStrtChk=chkbStrtChk.Checked== true ? "Y" : "N";

                    if (rdbFNDOBSSN.Checked)
                        Search_Cntrl = "F";
                    else if (rdbClientID.Checked)
                        Search_Cntrl = "C";
                    else
                        Search_Cntrl = "F";

                    //Added by Vikash on 03/05/2025 as per NCCAA Poverty Calculations Doc

                    POV_With_Desc = chkbPOV_With_Desc.Checked ? "Y" : "N";
                    POV_Dont_Roundup = chkbPOV_Dont_Roundup.Checked ? "Y" : "N";

                    AgencyControlEntity agencycontrol = new Model.Objects.AgencyControlEntity();
                    agencycontrol.ClidSmash = chkClidSmash.Checked == true ? "Y" : "N";
                    if (agencycontrol.ClidSmash == "Y")
                    {
                        agencycontrol.ClidYear = txtClidYear.Text;
                        if (dtClidFrom.Checked)
                            agencycontrol.ClidFrom = dtClidFrom.Value.ToShortDateString();
                        else
                            agencycontrol.ClidFrom = string.Empty;

                        if (dtClidTo.Checked)
                            agencycontrol.ClidTo = dtClidTo.Value.ToShortDateString();
                        else
                            agencycontrol.ClidTo = string.Empty;

                        agencycontrol.ClidSSN = chkClidSSN.Checked == true ? "Y" : "N";
                        agencycontrol.ClidClid = chkClidClid.Checked == true ? "Y" : "N";
                        agencycontrol.ClidDateStamp = rdoClidAllChanged.Checked == true ? "2" : "1";

                    }
                    else
                    {
                        agencycontrol.ClidDateStamp = rdoClidAllChanged.Checked == true ? "2" : "1";
                    }

                    if (chkFamilyIdHie.Checked)
                    {
                        agencycontrol.FamilyIdHie = chkFamilyIdHie.Checked == true ? "Y" : "N";
                        agencycontrol.FamilyIdDuplvl = chkFamilyIdDupblevel.Checked == true ? "Y" : "N";

                    }
                    else
                    {
                        agencycontrol.FamilyIdHie = "N";
                        agencycontrol.FamilyIdDuplvl = "N";
                    }
                    agencycontrol.MemberActivity = chkMemberActivity.Checked == true ? "Y" : "N";
                    agencycontrol.TMS201SoftEdit = chkTms201SEdit.Checked == true ? "Y" : "N";
                    agencycontrol.ShowIntakeSwitch = chkOnlyIntakeHie.Checked == true ? "Y" : "N";
                    agencycontrol.MostRecentintake = chkmostrecent.Checked == true ? "Y" : "N";
                    agencycontrol.ServicePlanHiecontrol = chkServicePlancont.Checked == true ? "Y" : "N";
                    agencycontrol.LoginMFA = chkLoginMfa.Checked == true ? "Y" : "N";
                    agencycontrol.ReverseFeed = chkReversFeed.Checked == true ? "Y" : "N";

                    agencycontrol.TXAlienSwitch = chkTXAlienSwitch.Checked == true ? "Y" : "N";

                    agencycontrol.CEAPPostUsage = chkbPostUsage.Checked == true ? "Y" : "N";

                    //agencycontrol.CEAPStrtChk= chkbStrtChk.Checked == true ? "Y" : "N";

                    agencycontrol.HealthInsSingleSel = chkHealthInsSingleSel.Checked == true ? "Y" : "N";

                    agencycontrol.AgyVendor = chkbAgyVend.Checked == true ? "Y" : "N";
                    // agencycontrol.CalQuescontprogram = chkCalQues.Checked == true ? "Y" : "N";
                    agencycontrol.SerPlanAllow = string.Empty;
                    if (agencycontrol.ServicePlanHiecontrol == "Y")
                    {
                        agencycontrol.SerPlanAllow = rdosamedept.Checked == true ? "D" : "A";
                    }
                    agencycontrol.SsnDobMMenu = rdoSSN.Checked == true ? "S" : "D";
                    agencycontrol.BulkpostTemp = chkBulkposttemplate.Checked == true ? "Y" : "N";
                    agencycontrol.WorkerFUP = chkCaseworkerFollowup.Checked == true ? "Y" : "N";
                    agencycontrol.LnkAppSwitch = chkLnkApplicant.Checked == true ? "Y" : "N";

                    PropAgencyControlEntity = agencycontrol;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else
                AlertBox.Show("Alien Form is associate to an agency, please remove the association first ", MessageBoxIcon.Warning);
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            this.Close();
        }


        private void OnHelpClick(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Agency Switches");
        }





        private void chkIncVerification_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIncVerification.Checked == true)
            {
                chkIncVerSwitch.Enabled = true;
            }
            else
            {
                chkIncVerSwitch.Enabled = false;
                chkIncVerSwitch.Checked = false;
            }
        }

        private void chkbCAOBO_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbCAOBO.Checked)
            {
                if (BaseForm.UserID.ToUpper() == "JAKE")
                {
                    pnlBenefitfrom.Visible = true;
                }
            }
            else
            {
                pnlBenefitfrom.Visible = false;
            }
        }

        private void chkClidSmash_CheckedChanged(object sender, EventArgs e)
        {
            _errorProvider.SetError(dtClidFrom, null);
            _errorProvider.SetError(dtClidTo, null);
            _errorProvider.SetError(txtClidYear, null);
            _errorProvider.SetError(chkClidClid, null);
            if (chkClidSmash.Checked)
            {
                txtClidYear.Visible = true;
                pnlIntegrity.Visible = true;
                pnlClientIdswitch.Visible = true;
                lblClidYear.Visible = true;
                this.Size = new Size(1108, 672);

            }
            else
            {
                txtClidYear.Visible = false;
                pnlClientIdswitch.Visible = false;
                pnlIntegrity.Visible = false;
                lblClidYear.Visible = false;
                pnlIntegrity.Visible = false;
                // this.pnlCompleteForm.Size = new Size(1108,630);
                this.Size = new Size(1108, 645);
            }
        }

        private void chkFamilyIdHie_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFamilyIdHie.Checked)
            {
                chkFamilyIdDupblevel.Visible = true;
            }
            else
            {
                chkFamilyIdDupblevel.Visible = false;
                chkFamilyIdDupblevel.Checked = false;
            }

        }

        private void pnlClientintake_Click(object sender, EventArgs e)
        {

        }

        private void pnlIncomeSwitch_Click(object sender, EventArgs e)
        {

        }

        private void chkServicePlancont_CheckedChanged(object sender, EventArgs e)
        {
            if (chkServicePlancont.Checked)
            {
                pnlservicesame.Visible = true;
                rdosamedept.Checked = true;
            }
            else
                pnlservicesame.Visible = false;
        }

        private void cmbNohits_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNohits.Items.Count > 0)
            {
                if (((ListItem)cmbNohits.SelectedItem).Value.ToString() == "3")
                    txtSearchRating.Visible = true;
                else
                    txtSearchRating.Visible = false;

            }
        }
    }
}