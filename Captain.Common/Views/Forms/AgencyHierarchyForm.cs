using System;
using System.IO;
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
using System.Globalization;
//using System.Windows.Forms;

namespace Captain.Common.Views.Forms
{
    public partial class AgencyHierarchyForm : Form
    {
        #region private variables
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private FolderBrowserDialog _repBrowser = new FolderBrowserDialog();
        private FolderBrowserDialog _impBrowser = new FolderBrowserDialog();
        private FolderBrowserDialog _expBrowser = new FolderBrowserDialog();

        #endregion

        public AgencyHierarchyForm(BaseForm baseForm, string mode, string hierarchyType, string agency, PrivilegeEntity privilege)
        {
            InitializeComponent();

            BaseForm = baseForm;
            PrivilegEntity = privilege;
            Mode = mode;
            Agency = agency;
            HierarchyType = hierarchyType;
            _model = new CaptainModel();



            if (BaseForm.BaseAgencyControlDetails.AgyShortName == "CABA")
            {
                txtHexNo.Visible = true;
                lblhexno.Visible = true;
                txtExpPath.Enabled = true;
                txtImpPath.Enabled = true;
            }
            txtCode.Validator = TextBoxValidation.IntegerValidator;
            txtHexNo.Validator = TextBoxValidation.FloatValidator;
            if (BaseForm.UserID.ToUpper() == "JAKE")
            {
                lblxmlHierchy.Visible = true;
                lblxmlpath.Visible = true;
                txtHierchy.Visible = true;
                txtXmlPath.Visible = true;
                lblreportspath.Visible = true;
                txtRepPath.Visible = true;
                lblimportpath.Visible = true;
                lblexportpath.Visible = true;
                txtExpPath.Visible = true;
                txtImpPath.Visible = true;

                lblCentHie.Visible = true;
                txtCentHie.Visible = true;

            }
            if (!Mode.Equals("ADMN0012"))
            {

                // lblHeader.Text = "Agency Definition";
                if (Mode.Equals("Add"))
                {
                    this.Text = "Agency Definition - Add";//"ADMN0009 - Add";
                    ServiceEnquiry = "0";
                    ClearReport = "0";
                    EditZip = "0";
                    ShowCaseManager = "0";
                    Tmsb20 = "C";
                    CAPVoucher = "N";
                    IncVerfication = "N";
                    Inccntl18noinc = "N";
                    VerSwitch = "N";
                    IncMethods = "1";
                    CaseNotesstamp = "N";
                    TaxExemption = string.Empty;
                    MatAssesment = "N";
                    DOB = SSN = FirstName = LastName = ClientRules = "N";
                    RefConn = "N";
                    PaymentVoucherNumber = string.Empty;
                    MostRecentIntake = "N";
                }
                else
                {
                    this.Text = "Agency Definition - Edit";//"ADMN0009 - Edit";
                }
            }
            else
            {
                // lblHeader.Text = privilege.PrivilegeName;
                this.Text = privilege.Program + " - " + Consts.Common.Add;
            }
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            Fillcmbsearchfor();
            fillcmbsearchby();
            FillCmbCaseType();

            if (Mode.Equals("ADMN0012"))
            {
                if (BaseForm.UserID != "JAKE")
                {
                    btnSwitches.Visible = false;
                    pnlreportpath.Visible = false;
                    label5.Visible = false;
                    label7.Visible = false;
                    label8.Visible = false;

                    txtName.Location = new Point(67, 4); txtStreet.Location = new Point(67, 5); txtCity.Location = new Point(67, 37); txtState.Location = new Point(67, 69); msktxtZip.Location = new Point(67, 101);

                    this.lblAgyShortName.Location = new Point(331, 7); this.lblReqAgyName.Location = new Point(399, 3); this.txtAgyShortName.Location = new Point(419, 4);

                    this.lbltelephone.Location = new Point(331, 11); this.msktxtPhone.Location = new Point(419, 5); this.lblTelephoneExt.Location = new Point(540, 11); this.txtExt.Location = new Point(572, 5);

                    this.lblfax.Location = new Point(331, 42); this.msktxtFax.Location = new Point(419, 37);

                    this.lblhoursfrom.Location = new Point(331, 74); this.dateFrom.Location = new Point(419, 69); this.lblhoursto.Location = new Point(540, 74); this.dateTo.Location = new Point(572, 69);

                    this.lblhexno.Location = new Point(331, 107); this.txtHexNo.Location = new Point(419, 101);

                    this.Size = new Size(698, 261); this.pnlCompleteForm.Size = new Size(698, 261);


                }
                else
                {
                    this.Size = new Size(854, 395); this.pnlCompleteForm.Size = new Size(854, 395);

                    /***********************************************/
                    this.lblAgyShortName.Location = new Point(520, 7); this.lblReqAgyName.Location = new Point(590, 3); this.txtAgyShortName.Location = new Point(600, 4);

                    /***********************************************/
                    this.lbltelephone.Location = new Point(520, 11); this.msktxtPhone.Location = new Point(600, 5); this.lblTelephoneExt.Location = new Point(710, 11); this.txtExt.Location = new Point(733, 5);

                    this.lblfax.Location = new Point(520, 42); this.msktxtFax.Location = new Point(600, 37);

                    this.lblhoursfrom.Location = new Point(520, 74); this.dateFrom.Location = new Point(600, 69); this.lblhoursto.Location = new Point(710, 74); this.dateTo.Location = new Point(733, 69);

                    this.lblhexno.Location = new Point(520, 107); this.txtHexNo.Location = new Point(600, 101);


                    /***********************************************/

                    this.lblxmlHierchy.Location = new Point(520, 35); this.label8.Location = new Point(580, 35); this.txtHierchy.Location = new Point(600, 35);
                   
                    this.pnlCntrlHIE.Location = new Point(660, 32);

                    

                }
                this.pnlStreetNew.BorderStyle = Wisej.Web.BorderStyle.None;
                pnlDesktop.Visible = false;
                pnlCode.Visible = false;
                pnlreportpath.Visible = true;
                txtCode.Text = Agency;
                txtCode.ReadOnly = true;
                lblCode.Text = "Name";
                txtShortName.Visible = false;
                //            pnlAgencyDetails.Location = new Point(0, 1);

                //            this.pnlreportpath.Location = new Point(0,225);
                //            this.pnlSave.Location = new Point(0, 463);
                fillAgencyControlForm(Agency);
                // btnAdd.Location = new Point(632,5);
                // btnCancel.Location = new Point(710,5);
                // btnSwitches.Location = new Point(11, 5);
                txtAgyShortName.Visible = true;
                lblAgyShortName.Visible = true;
                lblReqAgyName.Visible = true;
                txtName.Visible = true;
                lblname.Visible = true;
                lblReqName.Visible = true;

            }
            else
            {
                pnlName.Visible = false;
                pnlreportpath.Visible = false;
                pnlAgyLogo.Visible = true;
                //  this.pnlAgyLogo.Location = new Point(0, 246); 
                //   pnlDesktop.Location = new Point(0, 150);
                fillDropdowns();
                //if (BaseForm.UserID.ToUpper() != "JAKE")
                //{
                this.Size = new Size(805, 541);
                this.pnlCompleteForm.Size = new Size(805, 541);
                //  this.pnlSave.Location = new Point(0, 465);
                // this.panel2.Size = new System.Drawing.Size(611, 369);
                //  fillAgencyControlForm(Agency);
                //  btnAdd.Location = new Point(623, 5);  
                //  btnCancel.Location = new Point(701, 5);
                // btnSwitches.Location = new Point(11, 5);
                // }
            }

            if (Mode.Equals("Edit"))
            {
                fillAgencyHierarchyForm(Agency);
            }
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
        }

        #region Properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity PrivilegEntity { get; set; }

        public string Mode { get; set; }

        public string HierarchyType { get; set; }

        public string Agency { get; set; }

        public string selectedAgencyControl { get; set; }

        public string ServiceEnquiry { get; set; }
        public string ShowCaseManager { get; set; }
        public string EditZip { get; set; }
        public string ClearReport { get; set; }
        public string Tmsb20 { get; set; }
        public string CAPVoucher { get; set; }
        public string TaxExemption { get; set; }
        public string IncVerfication { get; set; }
        public string Inccntl18noinc { get; set; }
        public string IncMethods { get; set; }
        public string CaseNotesstamp { get; set; }
        public string ClientInquiry { get; set; }
        public string MatAssesment { get; set; }
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
        public string CAOBO { get; set; }
        public string ProgressNotesSwitch { get; set; }
        public string DeepSearchSwitch { get; set; }
        public string MailAddressSwitch { get; set; }
        public string FTypeSwitch { get; set; }
        public string HealthInsSingleSel { get; set; }

        public string PostUsage { get; set; }
        //public string CEAPStrtChk { get; set; }

        public string QuickPostServ { get; set; }
        public string WipeBMALSP { get; set; }
        public string BenefitFrom { get; set; }
        public string WorkerFUPswitch { get; set; }    //Added by Vikash 12/30/2022 for Followup issue
        public string PaymentVoucherNumber { get; set; }
        public string MostRecentIntake { get; set; }

        //added by Sudheer on 02/21/2018
        public string DOBFirstNamePoint { get; set; }

        public string VerSwitch { get; set; }

        //Murali added on 04/22/2020
        public AgencyControlEntity propAgencyControlEntity { get; set; }

        public string Search_Cntrls //Added by Vikash on 02/19/2025 as per Pre-priority document
        {
            get;
            set;
        }

        public string POV_With_Desc //Added by Vikash on 03/05/2025 as per NCCAA Poverty Calculations Doc
        {
            get;
            set;
        }

        public string POV_Dont_Roundup //Added by Vikash on 03/05/2025 as per NCCAA Poverty Calculations Doc
        {
            get;
            set;
        }

        #endregion

        private void fillDropdowns()
        {
            List<HierarchyEntity> hierarchyAgencyRep = _model.lookupDataAccess.GetAgencyRepresentation();
            foreach (HierarchyEntity hierarchyEntity in hierarchyAgencyRep)
            {
                cmbAgencyRep.Items.Add(new ListItem(hierarchyEntity.ShortName, hierarchyEntity.Code));
            }
            cmbAgencyRep.SelectedIndex = 0;

            List<HierarchyEntity> hierarchyClientName = _model.lookupDataAccess.GetClientNameFormat();
            foreach (HierarchyEntity hierarchyEntity in hierarchyClientName)
            {
                cmbClientName.Items.Add(new ListItem(hierarchyEntity.ShortName, hierarchyEntity.Code));
            }
            cmbClientName.SelectedIndex = 0;

            List<HierarchyEntity> hierarchyCaseWorkerName = _model.lookupDataAccess.GetCaseWorkerFormat();
            foreach (HierarchyEntity hierarchyEntity in hierarchyCaseWorkerName)
            {
                cmbCaseWorkName.Items.Add(new ListItem(hierarchyEntity.ShortName, hierarchyEntity.Code));
            }
            cmbCaseWorkName.SelectedIndex = 0;
        }

        public bool IsSaveValid { get; set; }

        private bool isCodeExists(string code)
        {
            bool isExists = false;
            if (Mode.Equals("Add"))
            {
                HierarchyEntity hierarchyEntity = _model.lookupDataAccess.GetCaseHierarchy(HierarchyType + "CheckDup", Agency, string.Empty, string.Empty, BaseForm.UserID, BaseForm.BaseAdminAgency);
                if (hierarchyEntity != null)
                {
                    isExists = true;
                }
            }
            return isExists;
        }


        private bool ValidateForm()
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                _errorProvider.SetError(txtCode, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCode.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {

                if (isCodeExists(txtCode.Text))
                {
                    _errorProvider.SetError(txtCode, string.Format(Consts.Messages.AlreadyExists.GetMessage(), lblCode.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    if (Mode.Equals("Add"))
                    {
                        if (txtCode.Text == "00")
                        {
                            _errorProvider.SetError(txtCode, "Invalid Agency code");
                            isValid = false;
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(txtCode, null);
                    }
                }
            }
            if (!Mode.Equals("ADMN0012"))
            {

                if (String.IsNullOrEmpty(txtAgencyName.Text.Trim()))
                {
                    _errorProvider.SetError(txtAgencyName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAgencyName.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtAgencyName, null);
                }

                if (String.IsNullOrEmpty(txtShortAgencyName.Text.Trim()))
                {
                    _errorProvider.SetError(txtShortAgencyName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAgencyShortName.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtShortAgencyName, null);
                }
            }

            if (Mode.Equals("ADMN0012"))
            {
                if (String.IsNullOrEmpty(txtName.Text))
                {
                    _errorProvider.SetError(txtName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblname.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtName, null);
                }

                if (String.IsNullOrEmpty(txtAgyShortName.Text.Trim()))
                {
                    _errorProvider.SetError(txtAgyShortName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAgyShortName.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtAgyShortName, null);
                }
            }

            if (!(String.IsNullOrEmpty(txtState.Text.Trim())) && txtState.Text.Length < 2)
            {
                _errorProvider.SetError(txtState, string.Format("State should be of '2' Characters", lblstate.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtState, null);
            }

            if (BaseForm.UserID.ToUpper() == "JAKE")
            {
                if (pnlreportpath.Visible == true)
                {
                    if (txtRepPath.Text.Trim()== string.Empty)
                    {
                        _errorProvider.SetError(txtRepPath, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblreportspath.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtRepPath, null);
                    }

                    if (txtRepPath.Text.Trim() != string.Empty)
                    {
                        if (!Directory.Exists(_model.lookupDataAccess.checkPathLrS(Consts.Common.ServerLocation) + txtRepPath.Text))
                        {
                            _errorProvider.SetError(txtRepPath, lblreportspath.Text + " Does not Exist");
                            isValid = false;
                        }
                        else
                        {
                            _errorProvider.SetError(txtRepPath, null);
                        }
                    }
                    if (txtXmlPath.Text.Trim() == string.Empty)
                    {
                        _errorProvider.SetError(txtXmlPath, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblxmlpath.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtXmlPath, null);
                    }
                    if (txtXmlPath.Text.Trim() != string.Empty)
                    {
                        if (!Directory.Exists(_model.lookupDataAccess.checkPathLrS(Consts.Common.ServerLocation) + txtXmlPath.Text))
                        {
                            _errorProvider.SetError(txtXmlPath, lblxmlpath.Text + " Does not Exist");
                            isValid = false;
                        }
                        else
                        {
                            _errorProvider.SetError(txtXmlPath, null);
                        }
                    }

                    if (String.IsNullOrEmpty(txtHierchy.Text.Trim()))
                    {
                        _errorProvider.SetError(txtHierchy, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblxmlHierchy.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtHierchy, null);
                    }

                    if (txtExpPath.Text.Trim() != string.Empty)
                    {
                        if (!Directory.Exists(Consts.Common.ServerLocation + txtExpPath.Text))
                        {
                            _errorProvider.SetError(txtExpPath, lblexportpath.Text + " Does not Exist");
                            isValid = false;
                        }
                        else
                        {
                            _errorProvider.SetError(txtExpPath, null);
                        }
                    }

                    if (txtImpPath.Text.Trim() != string.Empty)
                    {
                        if (!Directory.Exists(Consts.Common.ServerLocation + txtImpPath.Text))
                        {
                            _errorProvider.SetError(txtImpPath, lblimportpath.Text + " Does not Exist");
                            isValid = false;
                        }
                        else
                        {
                            _errorProvider.SetError(txtImpPath, null);
                        }
                    }
                }
            }
            //if (txtHierchy.Text.Trim() != string.Empty)
            //{
            //    if (!Directory.Exists(Consts.Common.ServerLocation + txtHierchy.Text))
            //    {
            //        _errorProvider.SetError(txtHierchy, lblxmlHierchy.Text + " Does not Exist");
            //        isValid = false;
            //    }
            //    else
            //    {
            //        _errorProvider.SetError(txtHierchy, null);
            //    }
            //}
            //else
            //{
            //    _errorProvider.SetError(txtHierchy, null);
            //}



            //if (String.IsNullOrEmpty(txtName.Text))
            //{
            //    _errorProvider.SetError(txtName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblname.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //{
            //    _errorProvider.SetError(txtName, null);
            //}

            //if (String.IsNullOrEmpty(txtShortName.Text))
            //{
            //    _errorProvider.SetError(txtShortName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblshortname.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //{
            //    _errorProvider.SetError(txtShortName, null);
            //}

            if (String.IsNullOrEmpty(txtCity.Text.Trim()))
            {
                _errorProvider.SetError(txtCity, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblcity.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtCity, null);
            }

                if (string.IsNullOrWhiteSpace(dateFrom.Text))
                {
                    _errorProvider.SetError(dateFrom, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "'Hours From'".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dateFrom, null);
                }
                if (string.IsNullOrWhiteSpace(dateTo.Text))
                {
                    _errorProvider.SetError(dateTo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "'Hours To'".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dateTo, null);
                }

            //DateTime time1 = /*dateFrom.Value;*/ Convert.ToDateTime(dateFrom.Text);
            //DateTime time2 = /*dateTo.Value; */Convert.ToDateTime(dateTo.Text);
            if (!string.IsNullOrEmpty(dateFrom.Text) && (!string.IsNullOrEmpty(dateTo.Text)))
            {
                if (Convert.ToDateTime(dateFrom.Text) > Convert.ToDateTime(dateTo.Text))
                {
                    _errorProvider.SetError(dateFrom, string.Format("'Hours-From' Should be Equal or Prior to 'Hours-To'", lblhoursfrom.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dateFrom, null);
                }
            }


            if (msktxtPhone.Text != "" && msktxtPhone.Text != "   -   -")
            {
                if (msktxtPhone.Text.Trim().Replace("-", "").Replace(" ", "").Length < 10)
                {
                    _errorProvider.SetError(msktxtPhone, "Please enter valid Telephone Number");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(msktxtPhone, null);
                }
            }
            if (msktxtFax.Text != "" && msktxtFax.Text != "   -   -")
            {
                if (msktxtFax.Text.Trim().Replace("-", "").Replace(" ", "").Length < 10)
                {
                    _errorProvider.SetError(msktxtFax, "Please enter valid Fax Number");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(msktxtFax, null);
                }
            }
            AgencyControlEntity AgencyControlDetails = new AgencyControlEntity();
            string output = string.Empty;
            string[] lines = Regex.Split(msktxtZip.Text, "-");
            AgencyControlDetails.Zip1 = lines[0];
            AgencyControlDetails.Zip2 = lines[1];
            if (/*string.IsNullOrEmpty(AgencyControlDetails.Zip2) &&*/ AgencyControlDetails.Zip1 != "     ")
            {
                AgencyControlDetails.Zip2 = "0000";
                output = AgencyControlDetails.Zip1 + "-" + AgencyControlDetails.Zip2;
            }
            else
            {
                output = AgencyControlDetails.Zip1 + "-" + AgencyControlDetails.Zip2;
            }
            msktxtZip.Text = output;

            if (msktxtZip.Text != "" && msktxtZip.Text != "     -")
            {
                if (msktxtZip.Text.Trim().Replace("-", "").Replace(" ", "").Length < 9)
                {
                    _errorProvider.SetError(msktxtZip, "Please enter valid Zip Code");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(msktxtZip, null);
                }
            }



            IsSaveValid = isValid;
            return (isValid);
        }

        private void fillcmbsearchby()
        {
            cmbSearchBy.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("Select One", "0"));
            listItem.Add(new ListItem("Address", "3"));
            listItem.Add(new ListItem("Alias", "5"));
            listItem.Add(new ListItem("App#", "7"));
            //listItem.Add(new ListItem("DOB + First Name", "8"));
            listItem.Add(new ListItem("Name", "1"));
            listItem.Add(new ListItem("Scanned App#", "6"));
            listItem.Add(new ListItem("SS#", "2"));
            listItem.Add(new ListItem("Telephone", "4"));
            //listItem.Add(new ListItem("Name", "1"));
            //listItem.Add(new ListItem("SS#", "2"));
            //listItem.Add(new ListItem("Address", "3"));
            //listItem.Add(new ListItem("Telephone", "4"));
            //listItem.Add(new ListItem("Alias", "5"));
            //listItem.Add(new ListItem("Scanned App#", "6"));
            //listItem.Add(new ListItem("App#", "7"));
            cmbSearchBy.Items.AddRange(listItem.ToArray());
            cmbSearchBy.SelectedIndex = 0;
        }

        private void Fillcmbsearchfor()
        {
            cmbSearchFor.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("Select One", "0"));
            listItem.Add(new ListItem("All", "1"));
            listItem.Add(new ListItem("Applicants", "2"));
            listItem.Add(new ListItem("Members", "3"));
            cmbSearchFor.Items.AddRange(listItem.ToArray());
            cmbSearchFor.SelectedIndex = 0;

        }



        public void FillCmbCaseType()
        {
            DataSet ds = Captain.DatabaseLayer.ZipCodePlusAgency.GetCaseType();

            DataTable dt = ds.Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                cmbCaseType.Items.Add(new ListItem(dr["Agy_8"].ToString(), dr["Agy_2"].ToString()));

            }
            cmbCaseType.Items.Insert(0, new ListItem("Select One", "0"));
            cmbCaseType.SelectedIndex = 0;
        }

        private void fillAgencyHierarchyForm(string agencyControl)
        {
            HierarchyEntity hierarchyDetails = _model.HierarchyAndPrograms.GetCaseHierarchy(HierarchyType, Agency, string.Empty, string.Empty, string.Empty, string.Empty);
            if (hierarchyDetails != null)
            {
                if (HierarchyType.Equals("AGENCY"))
                {
                    txtCode.Text = hierarchyDetails.Agency;
                    SetComboBoxValue(cmbAgencyRep, hierarchyDetails.HIERepresentation);
                    SetComboBoxValue(cmbCaseWorkName, hierarchyDetails.CWFormat);
                    SetComboBoxValue(cmbClientName, hierarchyDetails.CNFormat);
                }
                txtCode.Enabled = false;
                txtAgencyName.Text = hierarchyDetails.HirarchyName.Trim();
                txtShortAgencyName.Text = hierarchyDetails.ShortName.Trim();
                if (hierarchyDetails.Intake.Equals("Y")) cbIntake.Checked = true;
                fillAgencyControlForm(agencyControl);

                setUploadImage(hierarchyDetails.Logo.ToString());
            }
        }

        private void fillAgencyControlForm(string agencyControl)
        {
            AgencyControlEntity AgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile(agencyControl);
            if (AgencyControlDetails != null)
            {
                if (Mode.Equals("ADMN0012"))
                {

                    this.Text = "Agency Control File Maintenance - " + Consts.Common.Edit; // this.Text = PrivilegEntity.Program + " - " + Consts.Common.Edit;
                }

                txtAgyShortName.Text = AgencyControlDetails.AgyShortName.Trim();
                txtName.Text = AgencyControlDetails.AgyName.Trim();
                txtStreet.Text = AgencyControlDetails.Street.Trim();
                txtCity.Text = AgencyControlDetails.City.Trim();
                if(AgencyControlDetails.Zip2.Trim() == "" && !string.IsNullOrEmpty(AgencyControlDetails.Zip1))
                    msktxtZip.Text = AgencyControlDetails.Zip1.Trim() + "" + "0000";
                else
                    msktxtZip.Text = AgencyControlDetails.Zip1.Trim() + "" + AgencyControlDetails.Zip2.Trim();
                txtState.Text = AgencyControlDetails.State.Trim();
                msktxtPhone.Text = AgencyControlDetails.MainPhone.Trim();
                msktxtFax.Text = AgencyControlDetails.FaxNumbe.Trim();
                if (AgencyControlDetails.HoursFrom != string.Empty)
                {
                    dateFrom.Text = AgencyControlDetails.HoursFrom.ToString();
                }
                if (AgencyControlDetails.HoursTo != string.Empty)
                {
                    dateTo.Text = AgencyControlDetails.HoursTo.ToString();
                }
                txtHexNo.Text = AgencyControlDetails.NextService;

                //if (AgencyControlDetails.EditZip.Equals("1"))
                //{
                //    cbEditZip.Checked = true;
                //}
                //if (AgencyControlDetails.ClearCaprep.Equals("1"))
                //{
                //    cbClear.Checked = true;
                //}
                if (AgencyControlDetails.SearchDataBase.Equals("1"))
                {
                    chkEDataBase.Checked = true;
                }
                else
                {
                    chkEDataBase.Checked = false;

                }
                if (AgencyControlDetails.SearchCurAgySwitch.Equals("Y"))
                {
                    chkCurrentAgy.Checked = true;
                }
                else
                {
                    chkCurrentAgy.Checked = false;
                }

                SetComboBoxValue(cmbSearchBy, AgencyControlDetails.SearchBy);
                SetComboBoxValue(cmbSearchFor, AgencyControlDetails.SearchFor);
                SetComboBoxValue(cmbCaseType, AgencyControlDetails.SearchCaseType);
                txtRepPath.Text = AgencyControlDetails.Path;
                txtXmlPath.Text = AgencyControlDetails.XMLPath;
                txtHierchy.Text = AgencyControlDetails.XMLHierarchy;
                txtImpPath.Text = AgencyControlDetails.ImportPath;
                txtExpPath.Text = AgencyControlDetails.ExportPath;
                ServiceEnquiry = AgencyControlDetails.ServinqCaseHie;
                ShowCaseManager = AgencyControlDetails.CasemngrCombo;
                EditZip = AgencyControlDetails.EditZip;
                ClearReport = AgencyControlDetails.ClearCaprep;
                Tmsb20 = AgencyControlDetails.Tmsb20;
                CAPVoucher = AgencyControlDetails.CAPVoucher;
                TaxExemption = AgencyControlDetails.TaxExemption;
                IncVerfication = AgencyControlDetails.IncVerfication;
                Inccntl18noinc = AgencyControlDetails.Inccntl18noinc;
                IncMethods = AgencyControlDetails.IncMethods;
                txtExt.Text = AgencyControlDetails.MainEXT;
                CaseNotesstamp = AgencyControlDetails.CaseNotesstamp;
                ClientInquiry = AgencyControlDetails.AllowClientINQ;
                MatAssesment = AgencyControlDetails.MatAssesment;
                SSN = AgencyControlDetails.SSN;
                DOB = AgencyControlDetails.DOB;
                FirstName = AgencyControlDetails.FirstName;
                LastName = AgencyControlDetails.LastName;
                ClientRules = AgencyControlDetails.ClientRules;
                RefConn = AgencyControlDetails.RefConn;
                SSNPoint = AgencyControlDetails.SSNPoint;
                DOBPoint = AgencyControlDetails.DOBPoint;
                FirstNamePoint = AgencyControlDetails.FirstNamePoint;
                LastNamePoint = AgencyControlDetails.LastNamePoint;
                DOBLastNamePoint = AgencyControlDetails.DOBLastNamePoint;
                SSNLastNamePoint = AgencyControlDetails.SSNLastNamePoint;
                SearchHit = AgencyControlDetails.SearchHit;
                SearchRating = AgencyControlDetails.SearchRating;
                SiteSecurity = AgencyControlDetails.SiteSecurity;
                CAOBO = AgencyControlDetails.CAOBO;
                ProgressNotesSwitch = AgencyControlDetails.ProgressNotesSwitch;
                DeepSearchSwitch = AgencyControlDetails.DeepSearchSwitch;
                DelAppSwitch = AgencyControlDetails.DelAppSwitch;
                DefIntakeDtSwitch = AgencyControlDetails.DefIntakeDtSwitch;
                //PrintBatchNo = AgencyControlDetails.PrintBatchNo;
                //added by sudheer on 02/21/2018
                DOBFirstNamePoint = AgencyControlDetails.DOBFirstNamePoint;
                VerSwitch = AgencyControlDetails.VerSwitch;

                txtCentHie.Text = AgencyControlDetails.CentralHierarchy;
                MailAddressSwitch = AgencyControlDetails.MailAddressSwitch;
                FTypeSwitch = AgencyControlDetails.FTypeSwitch;
                HealthInsSingleSel = AgencyControlDetails.HealthInsSingleSel;
                WorkerFUPswitch = AgencyControlDetails.WorkerFUP;   // Added by Vikash 12/30/2022 for Followup issue
                QuickPostServ = AgencyControlDetails.QuickPostServices;
                PostUsage = AgencyControlDetails.CEAPPostUsage;
                //CEAPStrtChk=AgencyControlDetails.CEAPStrtChk;

                PaymentVoucherNumber = AgencyControlDetails.PaymentVoucherNumber;
                MostRecentIntake = AgencyControlDetails.MostRecentintake;
                WipeBMALSP = AgencyControlDetails.WipeBMALSP;
                BenefitFrom = AgencyControlDetails.BenefitFrom;
                propAgencyControlEntity = AgencyControlDetails;

                Search_Cntrls = AgencyControlDetails.Search_Controls;

                POV_With_Desc = AgencyControlDetails.ACR_POV_WITH_DEC;
                POV_Dont_Roundup = AgencyControlDetails.ACR_POV_DONT_RNDUP;
            }
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

        private void CommonBrowse_Click(object sender, EventArgs e)
        {
            //if (sender == btnRepBrowse)
            //{
            //    _repBrowser.Title = "Report Path Selection Window";
            //    _repBrowser.ShowNewFolderButton = false;
            //    _repBrowser.ShowDialog();
            //    _repBrowser.Closed += new EventHandler(OnFolderCommonClose);
            //}
            //else if (sender == btnImpBrowse)
            //{
            //    _impBrowser.Title = "Import Path Selection Window";
            //    _impBrowser.ShowNewFolderButton = false;
            //    _impBrowser.ShowDialog();
            //    _impBrowser.Closed += new EventHandler(OnFolderCommonClose);
            //}
            //else if (sender == btnExpBrowse)
            //{
            //    _expBrowser.Title = "Export Path Selection Window";
            //    _expBrowser.ShowNewFolderButton = false;
            //    _expBrowser.ShowDialog();
            //    _expBrowser.Closed += new EventHandler(OnFolderCommonClose);
            //}
        }

        private void OnFolderCommonClose(object sender, EventArgs e)
        {
            if (sender == _impBrowser)
            {
                if (!(string.IsNullOrEmpty(_impBrowser.SelectedPath)))
                    txtXmlPath.Text = _impBrowser.SelectedPath;
            }
            else if (sender == _repBrowser)
            {
                if (!(string.IsNullOrEmpty(_repBrowser.SelectedPath)))
                    txtRepPath.Text = _repBrowser.SelectedPath;
            }
            else if (sender == _expBrowser)
            {
                if (!(string.IsNullOrEmpty(_expBrowser.SelectedPath)))
                    txtHierchy.Text = _expBrowser.SelectedPath;
            }
        }


        private void OnSaveClick(object sender, EventArgs e)
        {

            if (Mode.Equals("ADMN0012"))
            {
                if (ValidateForm())
                {
                    if (AgencyControlInsertUpdate())
                    {
                        BaseForm.BaseAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
                        AlertBox.Show("Updated Successfully");
                        this.Close();
                    }
                }
            }
            else
            {
                if (HierarchyType.Equals("AGENCY"))
                {
                    Agency = txtCode.Text;
                }

                if (ValidateForm())
                {
                    //if (Directory.Exists(Consts.Common.ServerLocation + txtRepPath.Text))
                    //{
                    HierarchyEntity hierarchyEntity = new HierarchyEntity();
                    hierarchyEntity.Agency = Agency;
                    hierarchyEntity.Dept = string.Empty;
                    hierarchyEntity.Prog = string.Empty;
                    hierarchyEntity.HirarchyName = txtAgencyName.Text;
                    hierarchyEntity.ShortName = txtShortAgencyName.Text;
                    hierarchyEntity.Intake = cbIntake.Checked ? "Y" : "N";
                    if (HierarchyType.Equals("AGENCY"))
                    {
                        hierarchyEntity.Agency = txtCode.Text;
                        hierarchyEntity.HIERepresentation = ((ListItem)cmbAgencyRep.SelectedItem).Value.ToString();
                        hierarchyEntity.CWFormat = ((ListItem)cmbCaseWorkName.SelectedItem).Value.ToString();
                        hierarchyEntity.CNFormat = ((ListItem)cmbClientName.SelectedItem).Value.ToString();
                    }
                    hierarchyEntity.Mode = Mode;
                    hierarchyEntity.AddOperator = BaseForm.UserID;
                    hierarchyEntity.LSTCOperator = BaseForm.UserID;

                    string logoName = "";
                    if(_strImageFolderPath!="")
                        logoName = Agency + ".jpg";

                    hierarchyEntity.Logo = logoName;

                    if (_model.HierarchyAndPrograms.InsertUpdateHierarchy(hierarchyEntity))
                    {
                        if (_strImageFolderPath != "")
                            SaveStreamAsFile(_strImageFolderPath, (_ostreamLogo), logoName);

                        AgencyControlInsertUpdate();
                        HierarchyDefinitionControl hierarchyControl = BaseForm.GetBaseUserControl() as HierarchyDefinitionControl;
                        if (Mode == "Add") AlertBox.Show("Saved Successfully"); else AlertBox.Show("Updated Successfully");
                        if (hierarchyControl != null)
                        {
                            hierarchyControl.RefreshGrid(HierarchyType, Agency);
                        }
                        this.Close();
                    }
                    //}
                    //else
                    //{
                    //    CommonFunctions.MessageBoxDisplay("Report Path Not Exist");
                    //}
                }
            }
        }


       // string output = string.Empty;
        private bool AgencyControlInsertUpdate()
        {
            AgencyControlEntity AgencyControlDetails = new AgencyControlEntity();
            AgencyControlDetails.Street = txtStreet.Text;
            AgencyControlDetails.City = txtCity.Text;
            AgencyControlDetails.State = txtState.Text;
            AgencyControlDetails.FaxNumbe = msktxtFax.Text;
            string[] lines = Regex.Split(msktxtZip.Text, "-");
            AgencyControlDetails.Zip1 = lines[0];
            AgencyControlDetails.Zip2 = lines[1];
            //if (string.IsNullOrEmpty(AgencyControlDetails.Zip2))
            //{
            //    AgencyControlDetails.Zip2 = "0000";
            //    output = AgencyControlDetails.Zip1 + "-" + AgencyControlDetails.Zip2;
            //}
            //else
            //{ 
            //    output = AgencyControlDetails.Zip1 + "-" + AgencyControlDetails.Zip2; 
            //}
            msktxtZip.Text = AgencyControlDetails.Zip1 + "-" + AgencyControlDetails.Zip2;
            AgencyControlDetails.MainPhone = msktxtPhone.Text;
            //AgencyControlDetails.HoursFrom = dateFrom.Text.ToString();
            //AgencyControlDetails.HoursTo = dateTo.Text.ToString();
            if (dateFrom.Checked)
                AgencyControlDetails.HoursFrom = dateFrom.Value.ToString("HH:mm:ss");
            if (dateTo.Checked)
                AgencyControlDetails.HoursTo = dateTo.Value.ToString("HH:mm:ss");
            if (string.IsNullOrEmpty(txtHexNo.Text))
                AgencyControlDetails.NextService = "0";
            else
                AgencyControlDetails.NextService = txtHexNo.Text;
            AgencyControlDetails.AddOperator = BaseForm.UserID;
            AgencyControlDetails.LastOperator = BaseForm.UserID;
            //if (cbEditZip.Checked == true)
            //{
            //    AgencyControlDetails.EditZip = "1";
            //}
            //else
            //{
            //    AgencyControlDetails.EditZip = "0";
            //}

            //if (cbClear.Checked == true)
            //{
            //    AgencyControlDetails.ClearCaprep = "1";
            //}
            //else
            //{
            //    AgencyControlDetails.ClearCaprep = "0";
            //}

            AgencyControlDetails.EditZip = EditZip;
            AgencyControlDetails.ClearCaprep = ClearReport;
            AgencyControlDetails.ServinqCaseHie = ServiceEnquiry;
            AgencyControlDetails.CasemngrCombo = ShowCaseManager;
            AgencyControlDetails.Tmsb20 = Tmsb20;
            AgencyControlDetails.CAPVoucher = CAPVoucher;
            AgencyControlDetails.TaxExemption = TaxExemption;
            AgencyControlDetails.IncVerfication = IncVerfication;
            AgencyControlDetails.Inccntl18noinc = Inccntl18noinc;
            AgencyControlDetails.VerSwitch = VerSwitch;
            AgencyControlDetails.IncMethods = IncMethods;
            AgencyControlDetails.MainEXT = txtExt.Text;
            AgencyControlDetails.CaseNotesstamp = CaseNotesstamp;
            AgencyControlDetails.MatAssesment = MatAssesment;
            AgencyControlDetails.SSN = SSN;
            AgencyControlDetails.DOB = DOB;
            AgencyControlDetails.FirstName = FirstName;
            AgencyControlDetails.LastName = LastName;
            AgencyControlDetails.SSNPoint = SSNPoint;
            AgencyControlDetails.DOBPoint = DOBPoint;
            AgencyControlDetails.FirstNamePoint = FirstNamePoint;
            AgencyControlDetails.LastNamePoint = LastNamePoint;
            AgencyControlDetails.DOBLastNamePoint = DOBLastNamePoint;
            //added by sudheer on 02/212018
            AgencyControlDetails.DOBFirstNamePoint = DOBFirstNamePoint;
            AgencyControlDetails.SSNLastNamePoint = SSNLastNamePoint;
            AgencyControlDetails.ClientRules = ClientRules;
            AgencyControlDetails.SearchHit = SearchHit;
            AgencyControlDetails.SearchRating = SearchRating;
            AgencyControlDetails.RefConn = RefConn;
            AgencyControlDetails.MailAddressSwitch = MailAddressSwitch;
            AgencyControlDetails.FTypeSwitch = FTypeSwitch;
            AgencyControlDetails.HealthInsSingleSel = HealthInsSingleSel;
            AgencyControlDetails.CEAPPostUsage = PostUsage;
            //AgencyControlDetails.CEAPStrtChk = CEAPStrtChk;

            AgencyControlDetails.QuickPostServices = QuickPostServ;
            AgencyControlDetails.WipeBMALSP = WipeBMALSP;


            if (chkEDataBase.Checked == true)
                AgencyControlDetails.SearchDataBase = "1";
            else
                AgencyControlDetails.SearchDataBase = "0";

            if (chkCurrentAgy.Checked == true)
                AgencyControlDetails.SearchCurAgySwitch = "Y";
            else
                AgencyControlDetails.SearchCurAgySwitch = "N";

            AgencyControlDetails.AllowClientINQ = ClientInquiry;

            AgencyControlDetails.PaymentVoucherNumber = PaymentVoucherNumber;

            AgencyControlDetails.SiteSecurity = SiteSecurity;
            AgencyControlDetails.CAOBO = CAOBO;
            AgencyControlDetails.BenefitFrom = BenefitFrom;
            AgencyControlDetails.ProgressNotesSwitch = ProgressNotesSwitch;
            AgencyControlDetails.DeepSearchSwitch = DeepSearchSwitch;
            AgencyControlDetails.DelAppSwitch = DelAppSwitch;
            AgencyControlDetails.DefIntakeDtSwitch = DefIntakeDtSwitch;
            //AgencyControlDetails.PrintBatchNo = PrintBatchNo;
            AgencyControlDetails.Path = txtRepPath.Text;
            AgencyControlDetails.XMLPath = txtXmlPath.Text;
            AgencyControlDetails.ImportPath = txtImpPath.Text;
            AgencyControlDetails.ExportPath = txtExpPath.Text;
            AgencyControlDetails.XMLHierarchy = txtHierchy.Text;
            
            AgencyControlDetails.CentralHierarchy = txtCentHie.Text;

            AgencyControlDetails.AgencyCode = txtCode.Text;
            AgencyControlDetails.Mode = Mode;
            if (!((ListItem)cmbSearchBy.SelectedItem).Value.ToString().Equals("0"))
            {
                AgencyControlDetails.SearchBy = ((ListItem)cmbSearchBy.SelectedItem).Value.ToString();
            }

            if (!((ListItem)cmbSearchFor.SelectedItem).Value.ToString().Equals("0"))
            {
                AgencyControlDetails.SearchFor = ((ListItem)cmbSearchFor.SelectedItem).Value.ToString();

            }

            if (!((ListItem)cmbCaseType.SelectedItem).Value.ToString().Equals("0"))
            {
                AgencyControlDetails.SearchCaseType = ((ListItem)cmbCaseType.SelectedItem).Value.ToString();

            }

            if (Mode.Equals("ADMN0012"))
            {
                AgencyControlDetails.AgyShortName = txtAgyShortName.Text;
                AgencyControlDetails.AgyName = txtName.Text;
            }
            else
            {
                AgencyControlDetails.AgyShortName = txtShortAgencyName.Text;
                AgencyControlDetails.AgyName = txtAgencyName.Text;
            }

            if (propAgencyControlEntity != null)
            {
                AgencyControlDetails.ClidSmash = propAgencyControlEntity.ClidSmash;
                AgencyControlDetails.ClidYear = propAgencyControlEntity.ClidYear;
                AgencyControlDetails.ClidFrom = propAgencyControlEntity.ClidFrom;
                AgencyControlDetails.ClidTo = propAgencyControlEntity.ClidTo;
                AgencyControlDetails.ClidSSN = propAgencyControlEntity.ClidSSN;
                AgencyControlDetails.ClidClid = propAgencyControlEntity.ClidClid;
                AgencyControlDetails.ClidDateStamp = propAgencyControlEntity.ClidDateStamp;
                AgencyControlDetails.FamilyIdHie = propAgencyControlEntity.FamilyIdHie;
                AgencyControlDetails.FamilyIdDuplvl = propAgencyControlEntity.FamilyIdDuplvl;
                AgencyControlDetails.MemberActivity = propAgencyControlEntity.MemberActivity;
                AgencyControlDetails.TMS201SoftEdit = propAgencyControlEntity.TMS201SoftEdit;
                AgencyControlDetails.ShowIntakeSwitch = propAgencyControlEntity.ShowIntakeSwitch;
                AgencyControlDetails.MostRecentintake = propAgencyControlEntity.MostRecentintake;
                AgencyControlDetails.ServicePlanHiecontrol = propAgencyControlEntity.ServicePlanHiecontrol;
                AgencyControlDetails.LoginMFA = propAgencyControlEntity.LoginMFA;
                AgencyControlDetails.SerPlanAllow = propAgencyControlEntity.SerPlanAllow;
                AgencyControlDetails.SsnDobMMenu = propAgencyControlEntity.SsnDobMMenu;
                AgencyControlDetails.BulkpostTemp = propAgencyControlEntity.BulkpostTemp;
                AgencyControlDetails.WorkerFUP = propAgencyControlEntity.WorkerFUP;
                AgencyControlDetails.LnkAppSwitch = propAgencyControlEntity.LnkAppSwitch;
                AgencyControlDetails.AgyVendor = propAgencyControlEntity.AgyVendor;
                AgencyControlDetails.ReverseFeed = propAgencyControlEntity.ReverseFeed;

                AgencyControlDetails.TXAlienSwitch = propAgencyControlEntity.TXAlienSwitch;
                AgencyControlDetails.CEAPPostUsage = propAgencyControlEntity.CEAPPostUsage;
                //AgencyControlDetails.CEAPStrtChk = propAgencyControlEntity.CEAPStrtChk;

                // AgencyControlDetails.CalQuescontprogram = propAgencyControlEntity.CalQuescontprogram;

            }
            else
            {
                AgencyControlDetails.ClidSmash = string.Empty;
                AgencyControlDetails.ClidYear = string.Empty;
                AgencyControlDetails.ClidFrom = string.Empty;
                AgencyControlDetails.ClidTo = string.Empty;
                AgencyControlDetails.ClidSSN = string.Empty;
                AgencyControlDetails.ClidClid = string.Empty;

                AgencyControlDetails.ClidDateStamp = string.Empty;
                AgencyControlDetails.FamilyIdHie = string.Empty;
                AgencyControlDetails.FamilyIdDuplvl = string.Empty;

                if (propAgencyControlEntity != null)
                {
                    AgencyControlDetails.ClidDateStamp = propAgencyControlEntity.ClidDateStamp;
                    AgencyControlDetails.MemberActivity = propAgencyControlEntity.MemberActivity;
                    AgencyControlDetails.TMS201SoftEdit = propAgencyControlEntity.TMS201SoftEdit;
                    AgencyControlDetails.ShowIntakeSwitch = propAgencyControlEntity.ShowIntakeSwitch;
                    AgencyControlDetails.MostRecentintake = propAgencyControlEntity.MostRecentintake;
                    AgencyControlDetails.ServicePlanHiecontrol = propAgencyControlEntity.ServicePlanHiecontrol;
                    AgencyControlDetails.LoginMFA = propAgencyControlEntity.LoginMFA;
                    AgencyControlDetails.SerPlanAllow = propAgencyControlEntity.SerPlanAllow;
                    AgencyControlDetails.SsnDobMMenu = propAgencyControlEntity.SsnDobMMenu;
                    AgencyControlDetails.BulkpostTemp = propAgencyControlEntity.BulkpostTemp;
                    AgencyControlDetails.WorkerFUP = propAgencyControlEntity.WorkerFUP;
                    AgencyControlDetails.LnkAppSwitch = propAgencyControlEntity.LnkAppSwitch;
                    AgencyControlDetails.AgyVendor= propAgencyControlEntity.AgyVendor;
                    AgencyControlDetails.ReverseFeed = propAgencyControlEntity.ReverseFeed;
                    AgencyControlDetails.TXAlienSwitch = propAgencyControlEntity.TXAlienSwitch;
                    AgencyControlDetails.CEAPPostUsage = propAgencyControlEntity.CEAPPostUsage;
                    //AgencyControlDetails.CEAPStrtChk = propAgencyControlEntity.CEAPStrtChk;
                    // AgencyControlDetails.CalQuescontprogram = propAgencyControlEntity.CalQuescontprogram;
                }

            }
            if (!Mode.Equals("ADMN0009"))
            {
                if (AgencyControlDetails.AgencyCode != "00")
                {
                    AgencyControlDetails.MostRecentintake = MostRecentIntake;
                }
            }

            AgencyControlDetails.Search_Controls = Search_Cntrls;

            AgencyControlDetails.ACR_POV_WITH_DEC = POV_With_Desc;
            AgencyControlDetails.ACR_POV_DONT_RNDUP = POV_Dont_Roundup;

            return _model.ZipCodeAndAgency.InsertUpdateAGCYCNTL(AgencyControlDetails);

        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            this.Close();
        }



        private void OnHelpClick(object sender, EventArgs e)
        {
            //if (!Mode.Equals("ADMN0012"))
            //    Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "HierAgency");
            //else
            Application.Navigate("https://app.gitbook.com/s/XYNrNPcoD8nZpAG9fUkf/system-administration-module/screens/agency-control-file-maintenance", target: "_blank");
            //    Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "ADMN0012");
        }

        private void groupBox1_Click(object sender, EventArgs e)
        {

        }

        private void CommonTextField_LostFocus(object sender, EventArgs e)
        {
            if (sender == txtCode)
            {
                if (txtCode.Text.Length == 1)
                    txtCode.Text = "0" + txtCode.Text;
                if (!(string.IsNullOrEmpty(txtCode.Text)) && txtCode.Text.Length == 2)
                    _errorProvider.SetError(txtCode, null);
            }

            if (sender == txtState && !(string.IsNullOrEmpty(txtState.Text)) && txtState.Text.Length == 2)
                _errorProvider.SetError(txtState, null);

            if (sender == txtRepPath && !(string.IsNullOrEmpty(txtRepPath.Text)))
                _errorProvider.SetError(txtRepPath, null);

            if (sender == txtCity && !(string.IsNullOrEmpty(txtCity.Text)))
                _errorProvider.SetError(txtCity, null);

            if (sender == txtAgencyName && !(string.IsNullOrEmpty(txtAgencyName.Text)))
                _errorProvider.SetError(txtAgencyName, null);

            if (sender == txtShortAgencyName && !(string.IsNullOrEmpty(txtShortAgencyName.Text)))
                _errorProvider.SetError(txtShortAgencyName, null);
        }

        private void btnSwitches_Click(object sender, EventArgs e)
        {
            if (PrivilegEntity.Program == "ADMN0009")
            {

                AgencySwithcesHierchy agencySwitchesHierchy = new AgencySwithcesHierchy(BaseForm, Mode, PrivilegEntity, EditZip, Tmsb20, CAPVoucher, TaxExemption, PaymentVoucherNumber, MostRecentIntake);
                agencySwitchesHierchy.FormClosed += new FormClosedEventHandler(OnAgencySwitchHierchyFormClosed);
                agencySwitchesHierchy.StartPosition = FormStartPosition.CenterScreen;
                agencySwitchesHierchy.ShowDialog();
            }
            else
            {
                AgencySwitches agencySwitches = new AgencySwitches(BaseForm, Mode, PrivilegEntity, ServiceEnquiry, ShowCaseManager, EditZip, ClearReport, Tmsb20, CAPVoucher, TaxExemption,
                    IncVerfication,Inccntl18noinc,IncMethods, CaseNotesstamp, txtCode.Text, ClientInquiry, MatAssesment, SSN, DOB, LastName, FirstName, ClientRules, RefConn, SSNPoint, DOBPoint, LastNamePoint, 
                    FirstNamePoint, DOBLastNamePoint, SSNLastNamePoint, SearchHit, SearchRating, SiteSecurity, DelAppSwitch, DefIntakeDtSwitch, DOBFirstNamePoint, VerSwitch, CAOBO,
                    ProgressNotesSwitch, DeepSearchSwitch, MailAddressSwitch, FTypeSwitch, HealthInsSingleSel, WorkerFUPswitch, QuickPostServ, 
                    PaymentVoucherNumber, WipeBMALSP, BenefitFrom, PostUsage, propAgencyControlEntity, Search_Cntrls, POV_With_Desc, POV_Dont_Roundup);
                agencySwitches.FormClosed += new FormClosedEventHandler(OnAgencySwitchFormClosed);
                agencySwitches.StartPosition = FormStartPosition.CenterScreen;
                agencySwitches.ShowDialog();
            }
        }
        private void OnAgencySwitchFormClosed(object sender, FormClosedEventArgs e)
        {
            AgencySwitches form = sender as AgencySwitches;
            if (form.DialogResult == DialogResult.OK)
            {
                EditZip = propAgencyControlEntity.EditZip;
                ServiceEnquiry = propAgencyControlEntity.ServinqCaseHie;

                ClearReport = propAgencyControlEntity.ClearCaprep;
                Tmsb20 = propAgencyControlEntity.Tmsb20;
                CAPVoucher = propAgencyControlEntity.CAPVoucher;
                TaxExemption = propAgencyControlEntity.TaxExemption;
                PaymentVoucherNumber = propAgencyControlEntity.PaymentVoucherNumber;
                ShowCaseManager = form.ShowCaseManager;
                IncVerfication = form.IncVerfication;
                Inccntl18noinc = form.Inccntl18noinc;
                VerSwitch = form.VerSwitch;
                IncMethods = form.IncMethods;
                CaseNotesstamp = form.CaseNotesstamp;
                ClientInquiry = form.ClientInquiry;
                MatAssesment = form.Matassesment;
                SSN = form.SSN;
                DOB = form.DOB;
                FirstName = form.FirstName;
                LastName = form.LastName;
                ClientRules = form.ClientRules;
                SSNPoint = form.SSNPoint;
                DOBPoint = form.DOBPoint;
                FirstNamePoint = form.FirstNamePoint;
                LastNamePoint = form.LastNamePoint;
                DOBLastNamePoint = form.DOBLastNamePoint;
                SSNLastNamePoint = form.SSNLastNamePoint;
                RefConn = form.RefConn;
                SearchHit = form.SearchHit;
                SearchRating = form.SearchRating;
                SiteSecurity = form.SiteSecurity;
                DelAppSwitch = form.DelAppSwitch;
                DefIntakeDtSwitch = form.DefIntakeDtSwitch;
                //PrintBatchNo = form.PrintBatchNo;
                DOBFirstNamePoint = form.DOBFirstNamePoint;
                CAOBO = form.CAOBO;
                ProgressNotesSwitch = form.ProgressNotesSwitch;
                DeepSearchSwitch = form.DeepSearchSwitch;
                MailAddressSwitch = form.MailAddressSwitch;
                FTypeSwitch = form.FTypeSwitch;
                HealthInsSingleSel = form.HealthInsSingleSel;
                QuickPostServ = form.QuickPostServ;
                WorkerFUPswitch = form.WorkerFUPSwitch;     //Added by Vikash 12/30/2022 for Followup issue
                PostUsage = form.CEAPPostUsage;
                //CEAPStrtChk= form.CEAPStrtChk;
                WipeBMALSP = form.WipeBMALSP;
                BenefitFrom = form.BenefitFrom;
                propAgencyControlEntity = form.PropAgencyControlEntity;


                Search_Cntrls = form.Search_Cntrl; //Added by Vikash on 02/19/2025 as per Pre-priority document

                POV_With_Desc = form.POV_With_Desc;
                POV_Dont_Roundup = form.POV_Dont_Roundup;
            }
        }

        private void OnAgencySwitchHierchyFormClosed(object sender, FormClosedEventArgs e)
        {
            AgencySwithcesHierchy form = sender as AgencySwithcesHierchy;
            if (form.DialogResult == DialogResult.OK)
            {
                EditZip = form.EditZip;
                Tmsb20 = form.TmsB20;
                CAPVoucher = form.CAPVoucher;
                TaxExemption = form.TaxExemption;
                PaymentVoucherNumber = form.PaymentVoucherNumber;
                MostRecentIntake = form.MostRecent;
                if (propAgencyControlEntity != null)
                {
                    ServiceEnquiry = propAgencyControlEntity.ServinqCaseHie;
                    ShowCaseManager = propAgencyControlEntity.CasemngrCombo;
                    ClearReport = propAgencyControlEntity.ClearCaprep;
                    IncVerfication = propAgencyControlEntity.IncVerfication;
                    Inccntl18noinc = propAgencyControlEntity.Inccntl18noinc;
                    VerSwitch = propAgencyControlEntity.VerSwitch;
                    IncMethods = propAgencyControlEntity.IncMethods;
                    CaseNotesstamp = propAgencyControlEntity.CaseNotesstamp;
                    ClientInquiry = propAgencyControlEntity.AllowClientINQ;
                    MatAssesment = propAgencyControlEntity.MatAssesment;
                    SSN = propAgencyControlEntity.SSN;
                    DOB = propAgencyControlEntity.DOB;
                    FirstName = propAgencyControlEntity.FirstName;
                    LastName = propAgencyControlEntity.LastName;
                    ClientRules = propAgencyControlEntity.ClientRules;
                    SSNPoint = propAgencyControlEntity.SSNPoint;
                    DOBPoint = propAgencyControlEntity.DOBPoint;
                    FirstNamePoint = propAgencyControlEntity.FirstNamePoint;
                    LastNamePoint = propAgencyControlEntity.LastNamePoint;
                    DOBLastNamePoint = propAgencyControlEntity.DOBLastNamePoint;
                    SSNLastNamePoint = propAgencyControlEntity.SSNLastNamePoint;
                    RefConn = propAgencyControlEntity.RefConn;
                    SearchHit = propAgencyControlEntity.SearchHit;
                    SearchRating = propAgencyControlEntity.SearchRating;
                    SiteSecurity = propAgencyControlEntity.SiteSecurity;
                    DelAppSwitch = propAgencyControlEntity.DelAppSwitch;
                    DefIntakeDtSwitch = propAgencyControlEntity.DefIntakeDtSwitch;
                    //PrintBatchNo = form.PrintBatchNo;
                    DOBFirstNamePoint = propAgencyControlEntity.DOBFirstNamePoint;
                    CAOBO = propAgencyControlEntity.CAOBO;
                    ProgressNotesSwitch = propAgencyControlEntity.ProgressNotesSwitch;
                    DeepSearchSwitch = propAgencyControlEntity.DeepSearchSwitch;
                    MailAddressSwitch = propAgencyControlEntity.MailAddressSwitch;
                    FTypeSwitch = propAgencyControlEntity.FTypeSwitch;
                    HealthInsSingleSel = propAgencyControlEntity.HealthInsSingleSel;
                    QuickPostServ = propAgencyControlEntity.QuickPostServices;
                    WorkerFUPswitch = propAgencyControlEntity.WorkerFUP;    //Added by Vikash 12/30/2022 for Followup issue
                    PaymentVoucherNumber = propAgencyControlEntity.PaymentVoucherNumber;
                    WipeBMALSP = propAgencyControlEntity.WipeBMALSP;
                    BenefitFrom = propAgencyControlEntity.BenefitFrom;
                    propAgencyControlEntity.MostRecentintake = MostRecentIntake;
                    //propAgencyControlEntity = form.PropAgencyControlEntity;


                }
                else
                {


                    IncVerfication = string.Empty;
                    Inccntl18noinc=string.Empty;
                    VerSwitch = string.Empty;
                    IncMethods = string.Empty;
                    CaseNotesstamp = string.Empty;
                    ClientInquiry = string.Empty;
                    MatAssesment = string.Empty;
                    //SSN = string.Empty;
                    //DOB = string.Empty;
                    //FirstName = string.Empty;
                    //LastName = propAgencyControlEntity.LastName;
                    //ClientRules = propAgencyControlEntity.ClientRules;
                    //SSNPoint = propAgencyControlEntity.SSNPoint;
                    //DOBPoint = propAgencyControlEntity.DOBPoint;
                    //FirstNamePoint = propAgencyControlEntity.FirstNamePoint;
                    //LastNamePoint = propAgencyControlEntity.LastNamePoint;
                    //DOBLastNamePoint = propAgencyControlEntity.DOBLastNamePoint;
                    //SSNLastNamePoint = propAgencyControlEntity.SSNLastNamePoint;
                    //RefConn = propAgencyControlEntity.RefConn;
                    //SearchHit = propAgencyControlEntity.SearchHit;
                    //SearchRating = propAgencyControlEntity.SearchRating;
                    //SiteSecurity = propAgencyControlEntity.SiteSecurity;
                    //DelAppSwitch = propAgencyControlEntity.DelAppSwitch;
                    //DefIntakeDtSwitch = propAgencyControlEntity.DefIntakeDtSwitch;
                    ////PrintBatchNo = form.PrintBatchNo;
                    //DOBFirstNamePoint = propAgencyControlEntity.DOBFirstNamePoint;
                    //CAOBO = propAgencyControlEntity.CAOBO;
                    //ProgressNotesSwitch = propAgencyControlEntity.ProgressNotesSwitch;
                    //DeepSearchSwitch = propAgencyControlEntity.DeepSearchSwitch;
                    //MailAddressSwitch = propAgencyControlEntity.MailAddressSwitch;
                    //FTypeSwitch = propAgencyControlEntity.FTypeSwitch;
                    //QuickPostServ = propAgencyControlEntity.QuickPostServices;

                    //PaymentVoucherNumber = propAgencyControlEntity.PaymentVoucherNumber;
                    //WipeBMALSP = propAgencyControlEntity.WipeBMALSP;
                    //BenefitFrom = propAgencyControlEntity.BenefitFrom;
                    //propAgencyControlEntity = form.PropAgencyControlEntity;


                }

            }
        }

        Stream _ostreamLogo;
        string _strImageFolderPath = "";
        string _strLogoName = "";
        //CaptainModel _cmodel = new CaptainModel();
        private void updAgencyLogo_Uploaded(object sender, UploadedEventArgs e)
        {
            _ostreamLogo = e.Files[0].InputStream;
           
           

           // byte[] buff = System.IO.File.ReadAllBytes(_strImageFolderPath + _strLogoName);
           // System.IO.MemoryStream ms = new System.IO.MemoryStream(buff);
            Image updImg = Image.FromStream(_ostreamLogo);
            
            if (checkImgValidation(updImg, _ostreamLogo))
            {
                pictureBox.Image = Image.FromStream(_ostreamLogo);
                // LoadFile(e.Files);
                _strLogoName = e.Files[0].FileName.ToString();
                _strImageFolderPath = _model.lookupDataAccess.GetReportPath() + "\\AgencyLogos\\";
            }
            else
                AlertBox.Show("Please upload the logo with above Limitations ",MessageBoxIcon.Warning);
        }
        bool checkImgValidation(Image _oimg, Stream _oimgstream)
        {

            bool chkFlag = true;
            int _width = _oimg.Width;
            int _height = _oimg.Height;
            long _size = _oimgstream.Length;

            if (_width > 200)
                chkFlag = false;
            if (_height > 130)
                chkFlag = false;
            if(_size>70000)
                chkFlag=false;

            return chkFlag;
        }
        private void LoadFile(Wisej.Core.HttpFileCollection files)
        {
            if (files == null)
                return;

            if (files.Count == 0)
            {
                this.pictureBox.Image = null;
            }
            else
            {
                pictureBox.BackColor = Color.White;
                this.pictureBox.Image = GetImageFromStream(files[0].InputStream);
            }
        }
        private Image GetImageFromStream(Stream stream)
        {
            MemoryStream mem = new MemoryStream();
            stream.CopyTo(mem, 1024);
            mem.Position = 0;
            return Image.FromStream(mem);
        }
        void SaveStreamAsFile(string filePath, Stream inputStream, string fileName)
        {
            DirectoryInfo info = new DirectoryInfo(filePath);
            if (!info.Exists)
            {
                info.Create();
            }
            string path = Path.Combine(filePath, fileName);
            inputStream.Position = 0;
            using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
            {
                inputStream.CopyTo(outputFileStream);
            }
        }

        void setUploadImage(string FileName)
        {
            if (FileName != "")
            {

                _strLogoName = FileName.ToString();
                _strImageFolderPath = _model.lookupDataAccess.GetReportPath() + "\\AgencyLogos\\";

                FileInfo info = new FileInfo(_strImageFolderPath + _strLogoName);

                if (info.Exists)
                {
                    byte[] buff = System.IO.File.ReadAllBytes(_strImageFolderPath + _strLogoName);
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(buff);
                    _ostreamLogo = ms;
                    pictureBox.BackColor = Color.White;
                    pictureBox.Image = GetImageFromStream(ms);
                }
                else
                    pictureBox.Image = null;
            }
        }
        private void buttonClear_Click(object sender, EventArgs e)
        {
            //this.flowLayoutPanel.Controls.Clear(true);
            this.pictureBox.Image = null;
        }

        //private void msktxtZip_Leave(object sender, EventArgs e)
        //{
        //    AgencyControlEntity AgencyControlDetails = new AgencyControlEntity();
        //    string output = string.Empty;
        //    string[] lines = Regex.Split(msktxtZip.Text, "-");
        //    AgencyControlDetails.Zip1 = lines[0];
        //    AgencyControlDetails.Zip2 = lines[1];
        //    if (/*string.IsNullOrEmpty(AgencyControlDetails.Zip2) &&*/ AgencyControlDetails.Zip1 != "     ")
        //    {
        //        AgencyControlDetails.Zip2 = "0000";
        //        output = AgencyControlDetails.Zip1 + "-" + AgencyControlDetails.Zip2;
        //    }
        //    else
        //    {
        //        output = AgencyControlDetails.Zip1 + "-" + AgencyControlDetails.Zip2;
        //    }
        //    msktxtZip.Text = output;
        //}
       
    }
}