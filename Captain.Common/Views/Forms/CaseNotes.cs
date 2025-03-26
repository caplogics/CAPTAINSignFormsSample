#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;


using Wisej.Web;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using DevExpress.Pdf.Xmp;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CaseNotes : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        private string strYear = "    ";

        #endregion

        public CaseNotes(BaseForm baseForm, PrivilegeEntity privilegeEntity, List<CaseNotesEntity> caseNotes, string strkeyField)
        {
            InitializeComponent();
            sizecombo();
            BaseForm = baseForm;
            _model = new CaptainModel();
            Privilege = privilegeEntity;
            caseNotesEntity = caseNotes;
            propKeyFiedld = strkeyField;
            propBaseAppNo = BaseForm.BaseApplicationNo;
            propBaseAppName = BaseForm.BaseApplicationName;
            this.Text = /*privilegeEntity.Program + " - " + */"Case Notes";
            strYear = "    ";
            if ((BaseForm.BaseYear.Trim() != string.Empty))
            {
                strYear = BaseForm.BaseYear;
            }
            FillYearCombo(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, strYear);

            propReportPath = _model.lookupDataAccess.GetReportPath();
            propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
            SetApplicationDetails();
            fillCaseNotes(caseNotes);
            strFolderPath = Consts.Common.ReportFolderLocation + baseForm.UserID + "\\";
            if (string.IsNullOrEmpty(txtDesc.Text) || string.IsNullOrWhiteSpace(txtDesc.Text))
                btnPrint.Visible = false;
            else
                btnPrint.Visible = true;
        }

        public CaseNotes(BaseForm baseForm, PrivilegeEntity privilegeEntity, List<CaseNotesEntity> caseNotes, string strkeyField, string ScreenName, string AppNo, string AppName)
        {
            InitializeComponent();
            sizecombo();
            BaseForm = baseForm;
            _model = new CaptainModel();
            Privilege = privilegeEntity;
            caseNotesEntity = caseNotes;
            propKeyFiedld = strkeyField;
            propBaseAppNo = BaseForm.BaseApplicationNo;
            propBaseAppName = BaseForm.BaseApplicationName;
            _AppNo = AppNo;
            _AppName = AppName;

            ScreenCode = ScreenName;
            this.Text = "Case Notes";
            strYear = "    ";
            if ((BaseForm.BaseYear.Trim() != string.Empty))
            {
                strYear = BaseForm.BaseYear;
            }
            FillYearCombo(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, strYear);

            propReportPath = _model.lookupDataAccess.GetReportPath();
            propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
            SetApplicationDetails();
            fillCaseNotes(caseNotes);
            strFolderPath = Consts.Common.ReportFolderLocation + baseForm.UserID + "\\";
            if (string.IsNullOrEmpty(txtDesc.Text) || string.IsNullOrWhiteSpace(txtDesc.Text))
                btnPrint.Visible = false;
            else
                btnPrint.Visible = true;

            if (Privilege.Program == "CASE0028")
            {
                lblApplicationNo.Text = "App#: ";
                lblClientName.Text = "Name: ";
                lblApplicationNon.Text = propBaseAppNo;
                lblClientNameD.Text = propBaseAppName;
            }
            else if(Privilege.Program == "CASE0027")
            {
                lblApplicationNo.Text = "App#: ";
                lblClientName.Text = "Name: ";
                lblApplicationNon.Text = AppNo;
                lblClientNameD.Text = AppName;
            }
        }

        /// <summary>
        /// This constractor using only Hss00137Control 
        /// </summary>
        /// <param name="baseForm"></param>
        /// <param name="privilegeEntity"></param>
        /// <param name="caseNotes"></param>
        /// <param name="strkeyField"></param>
        /// 
        public CaseNotes(BaseForm baseForm, PrivilegeEntity privilegeEntity, List<CaseNotesEntity> caseNotes, string strkeyField, string Screen)
        {
            InitializeComponent();
            sizecombo();
            BaseForm = baseForm;
            _model = new CaptainModel();
            Privilege = privilegeEntity;
            caseNotesEntity = caseNotes;
            propKeyFiedld = strkeyField;
            propBaseAppNo = BaseForm.BaseApplicationNo;
            propBaseAppName = BaseForm.BaseApplicationName;
            this.Text = privilegeEntity.Program + " - " + "Case Notes";
            strYear = "    ";
            if ((BaseForm.BaseYear.Trim() != string.Empty))
            {
                strYear = BaseForm.BaseYear;
            }
            FillYearCombo(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, strYear);

            DataSet dsAgency = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL(BaseForm.BaseAgency, null, null, null, null, null, null);
            if (dsAgency != null && dsAgency.Tables[0].Rows.Count > 0)
            {
                AGYShortName = dsAgency.Tables[0].Rows[0]["ACR_SHORT_NAME"].ToString().Trim();
            }

            propReportPath = _model.lookupDataAccess.GetReportPath();
            propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
            SetApplicationDetails();
            fillCaseNotes(caseNotes);
            strFolderPath = Consts.Common.ReportFolderLocation + baseForm.UserID + "\\";
            if (string.IsNullOrEmpty(txtDesc.Text) || string.IsNullOrWhiteSpace(txtDesc.Text))
                btnPrint.Visible = false;
            else
                btnPrint.Visible = true;

            if ((Screen == "SAL" || Screen == "SPM") && AGYShortName == "YWCAB" && (BaseForm.UserID != "DKING" && BaseForm.UserID != "JSTULL" && BaseForm.UserID != "SYSTEM"))
            {
                btnAdd.Visible = btnDelete.Visible = btnChange.Visible = false;
                //if(BaseForm.UserID== "DKING" || BaseForm.UserID == "JSTULL" || BaseForm.UserID == "SYSTEM")
                //{
                //    btnDelete.Visible == true;
                //}
            }

        }
        public CaseNotes(BaseForm baseForm, PrivilegeEntity privilegeEntity, List<CaseNotesEntity> caseNotes, string strkeyField, string Name, string strApp)
        {
            InitializeComponent();
            BaseForm = baseForm;
            _model = new CaptainModel();
            sizecombo();
            Privilege = privilegeEntity;
            propBaseAppNo = strApp;
            propBaseAppName = Name;
            caseNotesEntity = caseNotes;
            propKeyFiedld = strkeyField;
            this.Text = privilegeEntity.Program + " - " + "Case Notes";
            strYear = "    ";
            if ((BaseForm.BaseYear.Trim() != string.Empty))
            {
                strYear = BaseForm.BaseYear;
            }
            FillYearCombo(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, strYear);

            propReportPath = _model.lookupDataAccess.GetReportPath();
            propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
            lblApplicationNo.Text = "App#: ";
            lblClientName.Text = "Name: ";
            lblApplicationNon.Text = propBaseAppNo;
            lblClientNameD.Text = propBaseAppName;

            if (Privilege.Program.ToUpper() == "ADMN0011")
            {
                propBaseAppNo = strkeyField;
                lblApplicationNon.Text = propBaseAppNo;
                lblApplicationNo.Text = "Code: ";
            }

            fillCaseNotes(caseNotes);
            strFolderPath = Consts.Common.ReportFolderLocation + baseForm.UserID + "\\";
            if (string.IsNullOrEmpty(txtDesc.Text) || string.IsNullOrWhiteSpace(txtDesc.Text))
                btnPrint.Visible = false;
            else
                btnPrint.Visible = true;
        }

        public CaseNotes(BaseForm baseForm, PrivilegeEntity privilegeEntity, string strkeyField, string Opr_mode)
        {
            InitializeComponent();
            BaseForm = baseForm;
            _model = new CaptainModel();
            sizecombo();
            propBaseAppNo = BaseForm.BaseApplicationNo;
            propBaseAppName = BaseForm.BaseApplicationName;
            Privilege = privilegeEntity;
            propKeyFiedld = strkeyField;
            this.Text = privilegeEntity.Program + " - " + "Case Notes";
            strYear = "    ";
            if ((BaseForm.BaseYear.Trim() != string.Empty))
            {
                strYear = BaseForm.BaseYear;
            }
            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privilege.Program, propKeyFiedld);
            propReportPath = _model.lookupDataAccess.GetReportPath();
            propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
            SetApplicationDetails();
            fillCaseNotes();
            strFolderPath = Consts.Common.ReportFolderLocation + baseForm.UserID + "\\";
            if (string.IsNullOrEmpty(txtDesc.Text) || string.IsNullOrWhiteSpace(txtDesc.Text))
                btnPrint.Visible = false;
            else
                btnPrint.Visible = true;

            if (Opr_mode == "view")
            {
                //this.btnPrint.Location = new System.Drawing.Point(2, 315);
                btnAdd.Visible = btnDelete.Visible = btnChange.Visible = false;
            }
        }

        public CaseNotes(BaseForm baseForm, PrivilegeEntity privilegeEntity, string strkeyField, string Opr_mode, string strProgramYear)
        {
            InitializeComponent();
            BaseForm = baseForm;
            _model = new CaptainModel();
            sizecombo();
            propBaseAppNo = BaseForm.BaseApplicationNo;
            propBaseAppName = BaseForm.BaseApplicationName;
            Privilege = privilegeEntity;
            propKeyFiedld = strkeyField;
            this.Text = /*privilegeEntity.Program + " - " + */"Case Notes";
            strYear = "    ";
            if ((BaseForm.BaseYear.Trim() != string.Empty))
            {
                strYear = strProgramYear;
            }
            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privilege.Program, propKeyFiedld);
            propReportPath = _model.lookupDataAccess.GetReportPath();
            propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
            SetApplicationDetails();
            fillCaseNotes();
            strFolderPath = Consts.Common.ReportFolderLocation + baseForm.UserID + "\\";
            if (string.IsNullOrEmpty(txtDesc.Text) || string.IsNullOrWhiteSpace(txtDesc.Text))
                btnPrint.Visible = false;
            else
                btnPrint.Visible = true;

            if (Opr_mode == "view")
            {
                //this.btnPrint.Location = new System.Drawing.Point(2, 315);
                btnAdd.Visible = btnDelete.Visible = btnChange.Visible = false;
            }
        }


        public CaseNotes(BaseForm baseForm, PrivilegeEntity privilegeEntity, string strkeyField, string Opr_mode, string strVendorName, string strVendorCode)
        {
            InitializeComponent();
            BaseForm = baseForm;
            _model = new CaptainModel();
            sizecombo();
            propBaseAppNo = strVendorCode;
            propBaseAppName = strVendorName;
            Privilege = privilegeEntity;
            propKeyFiedld = strkeyField;
            this.Text = privilegeEntity.Program + " - " + "Case Notes";
            strYear = "    ";
            if ((BaseForm.BaseYear.Trim() != string.Empty))
            {
                strYear = BaseForm.BaseYear;
            }
            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privilege.Program, propKeyFiedld);
            propReportPath = _model.lookupDataAccess.GetReportPath();
            propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
            SetApplicationDetails();
            fillCaseNotes();
            strFolderPath = Consts.Common.ReportFolderLocation + baseForm.UserID + "\\";
            if (string.IsNullOrEmpty(txtDesc.Text) || string.IsNullOrWhiteSpace(txtDesc.Text))
                btnPrint.Visible = false;
            else
                btnPrint.Visible = true;

            if (Opr_mode == "view")
            {
                //this.btnPrint.Location = new System.Drawing.Point(2, 315);
                btnAdd.Visible = btnDelete.Visible = btnChange.Visible = false;
            }
        }


        private void SetApplicationDetails()
        {
            if (Privilege.Program.Trim() == "HSS20138")
            {
                lblApplicationNo.Text = "Bus Id";
                lblClientName.Text = "Bus Desc";
            }
            else if (Privilege.Program.Trim() == "HSS20139")
            {
                lblApplicationNo.Text = "Bus Route Id";
                lblClientName.Text = "Route Desc";
            }
            else if (Privilege.Program.Trim() == "HOU00012")
            {
                lblApplicationNo.Text = "Employee#";
                lblClientName.Text = "Emp. Name";
            }
            else if (Privilege.Program.Trim() == "HOU00002")
            {
                lblApplicationNo.Text = "Request#";
                lblClientName.Text = " ";
                lblClientName.Visible = false;
                lblClientNameD.Visible = false;

            }
            else if (Privilege.Program.Trim() == "CASE0012")
            {
                

                lblClientName.Text = "";
                lblClientNameD.Text = "Volunteer/Donor Number: " + propBaseAppNo; 
                //lblClientNameD.Location = new Point(70, 35);
                lblClientNameD.Dock = DockStyle.Top;
                lblClientNameD.Padding = new System.Windows.Forms.Padding(20, 5, 5, 5);
                lblClientNameD.SendToBack();

                lblApplicationNo.Text = "";
                lblApplicationNon.Text = "Volunteer/Donor Name: " + propBaseAppName;
                lblApplicationNon.AutoSize = true;
                //lblApplicationNon.Location = new Point(70, 5);
                lblApplicationNon.Dock = DockStyle.Top;
                lblApplicationNon.Padding = new System.Windows.Forms.Padding(20, 3, 5, 5);
                lblApplicationNon.BringToFront();

              
            }
            else
            {
                lblApplicationNo.Text = "App#: ";
                lblClientName.Text = "Name: ";
                lblApplicationNon.Text = propBaseAppNo;
                lblClientNameD.Text = propBaseAppName;
            }

        }

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privilege { get; set; }

        public List<CaseNotesEntity> caseNotesEntity { get; set; }

        public string propBaseAppNo { get; set; }

        public string propBaseAppName { get; set; }

        public string propKeyFiedld { get; set; }

        public string propReportPath { get; set; }
        public string AGYShortName { get; set; }

        public AgencyControlEntity propAgencyControlDetails { get; set; }

        private void sizecombo()
        {
            cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("10", "10"));
            //cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("11", "11"));
            cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("12", "12"));
            //  cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("13", "13"));
            cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("14", "14"));
            //   cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("15", "15"));
            cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("16", "16"));
            //  cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("17", "17"));
            cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("18", "18"));
            //  cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("19", "19"));
            cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("20", "20"));
            cmbsize.SelectedIndex = 0;
        }

        private void fillCaseNotes()
        {
            ToolTip tooltip = new ToolTip();
            btnClose.Visible = false;
            btnOk.Visible = false;
            txtDesc.Text = string.Empty;
            if (caseNotesEntity.Count > 0)
            {

                txtDesc.Text = caseNotesEntity[0].Data.ToString();
                string toolTipText = "Added By     : " + caseNotesEntity[0].AddOperator.Trim() + " on " + caseNotesEntity[0].DateAdd.ToString() + "\n";
                string modifiedBy = string.Empty;
                if (!caseNotesEntity[0].LstcOperation.ToString().Trim().Equals(string.Empty))
                    modifiedBy = caseNotesEntity[0].LstcOperation.ToString().Trim() + " on " + caseNotesEntity[0].DateLstc.ToString();
                toolTipText += "Modified By : " + modifiedBy;
                tooltip.SetToolTip(txtDesc, toolTipText);
                tooltip.SetToolTip(lblApplicationNon, toolTipText);
                tooltip.SetToolTip(lblClientNameD, toolTipText);
                tooltip.SetToolTip(lblApplicationNo, toolTipText);
                tooltip.SetToolTip(lblClientName, toolTipText);
                txtDesc.ReadOnly = true;
                btnAdd.Visible = false;
                btnPrint.Visible = true;
                if (Privilege.ChangePriv.Equals("false"))
                {
                    btnChange.Visible = false;
                }
                else
                {
                    btnChange.Visible = true;
                }
                if (Privilege.DelPriv.Equals("false"))
                {
                    btnDelete.Visible = false;
                }
                else
                {
                    btnDelete.Visible = true;
                }
                if (strYear != BaseForm.BaseYear)
                {
                    btnChange.Visible = false;
                    btnDelete.Visible = false;
                }

            }
            else
            {
                btnChange.Visible = false;
                btnDelete.Visible = false;
                btnPrint.Visible = false;
                if (Privilege.AddPriv.Equals("false"))
                {
                    btnAdd.Visible = false;

                }
                else
                {
                    btnAdd.Visible = true;
                }
                if (strYear != BaseForm.BaseYear)
                {
                    btnAdd.Visible = false;
                }
            }
        }

        private void fillCaseNotes(List<CaseNotesEntity> caseNotesEntity)
        {
            ToolTip tooltip = new ToolTip();
            btnClose.Visible = false;
            btnOk.Visible = false;
            txtDesc.Text = string.Empty;
            if (caseNotesEntity.Count > 0)
            {

                txtDesc.Text = caseNotesEntity[0].Data.ToString().Trim();
                string toolTipText = "Added By     : " + caseNotesEntity[0].AddOperator.Trim() + " on " + caseNotesEntity[0].DateAdd.ToString() + "\n";
                string modifiedBy = string.Empty;
                if (!caseNotesEntity[0].LstcOperation.ToString().Trim().Equals(string.Empty))
                    modifiedBy = caseNotesEntity[0].LstcOperation.ToString().Trim() + " on " + caseNotesEntity[0].DateLstc.ToString();
                toolTipText += "Modified By : " + modifiedBy;
                tooltip.SetToolTip(txtDesc, toolTipText);
                tooltip.SetToolTip(lblApplicationNon, toolTipText);
                tooltip.SetToolTip(lblClientNameD, toolTipText);
                tooltip.SetToolTip(lblApplicationNo, toolTipText);
                tooltip.SetToolTip(lblClientName, toolTipText);
                txtDesc.ReadOnly = true;
                btnAdd.Visible = false;
                btnPrint.Visible = true;
                if (Privilege.ChangePriv.Equals("false"))
                {
                    btnChange.Visible = false;
                }
                else
                {
                    btnChange.Visible = true;
                }
                if (Privilege.DelPriv.Equals("false"))
                {
                    btnDelete.Visible = false;
                }
                else
                {
                    btnDelete.Visible = true;
                }
                if (strYear != BaseForm.BaseYear)
                {
                    btnChange.Visible = false;
                    btnDelete.Visible = false;
                }

            }
            else
            {
                btnChange.Visible = false;
                btnDelete.Visible = false;
                btnPrint.Visible = false;
                if (Privilege.AddPriv.Equals("false"))
                {
                    btnAdd.Visible = false;

                }
                else
                {
                    btnAdd.Visible = true;
                }
                if (strYear != BaseForm.BaseYear)
                {
                    btnAdd.Visible = false;
                }
            }
        }
        public string ScreenCode = string.Empty;
        public string _AppNo = string.Empty;
        public string _AppName = string.Empty;
        private void btnOk_Click(object sender, EventArgs e)
        {
            CaseNotesEntity caseNotesDetails = new CaseNotesEntity();
            if(!string.IsNullOrEmpty(ScreenCode.Trim()))
                caseNotesDetails.ScreenName = ScreenCode.Trim();
            else
                caseNotesDetails.ScreenName = Privilege.Program;
            caseNotesDetails.FieldName = propKeyFiedld;
            if (Privilege.Program.ToUpper() == "CASE0012")
            {
                caseNotesDetails.AppliCationNo = BaseForm.BaseAgency + "    " + propBaseAppNo;
            }
            else if (Privilege.Program.ToUpper() == "ADMN0011")
            {
                caseNotesDetails.AppliCationNo = propKeyFiedld;
                propBaseAppNo = propKeyFiedld;
            }
            else
            {
                caseNotesDetails.AppliCationNo = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + propBaseAppNo;
            }
            if (propAgencyControlDetails.CaseNotesstamp.ToUpper() == "Y")
            {
                if (caseNotesEntity.Count > 0)
                {
                    if (caseNotesEntity[0].Data.Trim() != txtDesc.Text.Trim())
                    {

                        string test = txtDesc.Text.Trim().Substring(txtDesc.Text.Trim().Length - 2);

                        if(test.Length> 0)
                        {
                            if(test=="AM" || test=="PM")
                            {
                                int index = txtDesc.Text.Trim().LastIndexOf("-");

                                txtDesc.Text = txtDesc.Text.Trim().Remove(index - 1, txtDesc.Text.Trim().Length - (index - 1));
                            }
                        }

                        //**caseNotesDetails.Data = txtDesc.Text.Trim() + "\n __________________Notes Changed by: " + BaseForm.UserID + " on " + DateTime.Today.ToString("MMM") + " " + DateTime.Now.Day + " " + DateTime.Now.Year + " " + DateTime.Now.ToString("hh:mm tt");
                        caseNotesDetails.Data =  txtDesc.Text.Trim()+" - "+ "Changed by: " + BaseForm.UserID + " " + DateTime.Today.ToString("MMM") + " " + DateTime.Now.Day + " " + DateTime.Now.Year + " " + DateTime.Now.ToString("hh:mm tt");

                    }
                    else
                    {
                        if (txtDesc.Text.Contains("Changed by") || txtDesc.Text.Contains("Added by"))
                        {
                            caseNotesDetails.Data = txtDesc.Text.Trim();
                        }
                        else
                        {
                            //**caseNotesDetails.Data = txtDesc.Text.Trim() + "\n __________________Notes Changed by: " + BaseForm.UserID + " on " + DateTime.Today.ToString("MMM") + " " + DateTime.Now.Day + " " + DateTime.Now.Year + " " + DateTime.Now.ToString("hh:mm tt");
                            caseNotesDetails.Data =  txtDesc.Text.Trim() +" - "+ "Changed by: " + BaseForm.UserID + " " + DateTime.Today.ToString("MMM") + " " + DateTime.Now.Day + " " + DateTime.Now.Year + " " + DateTime.Now.ToString("hh:mm tt");
                        }
                    }
                }
                else
                {
                    //**caseNotesDetails.Data = txtDesc.Text.Trim() + "\n __________________Notes Added by: " + BaseForm.UserID + " on " + DateTime.Today.ToString("MMM") + " " + DateTime.Now.Day + " " + DateTime.Now.Year + " " + DateTime.Now.ToString("hh:mm tt");
                    caseNotesDetails.Data =  txtDesc.Text.Trim()+" - "+ "Added by: " + BaseForm.UserID + " " + DateTime.Today.ToString("MMM") + " " + DateTime.Now.Day + " " + DateTime.Now.Year + " " + DateTime.Now.ToString("hh:mm tt");
                }
            }
            else
            {
                //**caseNotesDetails.Data = txtDesc.Text.Trim() + "\n __________________Notes Added by: " + BaseForm.UserID + " on " + DateTime.Today.ToString("MMM") + " " + DateTime.Now.Day + " " + DateTime.Now.Year + " " + DateTime.Now.ToString("hh:mm tt");
                //caseNotesDetails.Data = "Added by: " + BaseForm.UserID + " " + DateTime.Today.ToString("MMM") + " " + DateTime.Now.Day + " " + DateTime.Now.Year + " " + DateTime.Now.ToString("hh:mm tt") + "\n" + txtDesc.Text.Trim();
                caseNotesDetails.Data =txtDesc.Text.Trim();
            }
            caseNotesDetails.AddOperator = BaseForm.UserID;
            caseNotesDetails.LstcOperation = BaseForm.UserID;
            if (_model.TmsApcndata.InsertUpdateCaseNotes(caseNotesDetails))
            {
                strYear = "    ";
                if (!string.IsNullOrEmpty(BaseForm.BaseYear))
                {
                    strYear = BaseForm.BaseYear;
                }
                if (!string.IsNullOrEmpty(ScreenCode.Trim()))
                    caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(ScreenCode.Trim(), propKeyFiedld);
                else
                    caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privilege.Program, propKeyFiedld);

                fillCaseNotes();
            }
            AlertBox.Show("Saved Successfully");
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            txtDesc.ReadOnly = false;
            btnPrint.Visible = false;
            btnClose.Visible = true;
            btnOk.Visible = true;
            btnChange.Visible = false;
            btnDelete.Visible = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: OnDeleteMessageBoxClicked);

        }

        private void OnDeleteMessageBoxClicked(DialogResult dialogResult)
        {


            if (dialogResult == DialogResult.Yes)
            {
                CaseNotesEntity caseNotesDetails = new CaseNotesEntity();
                if(!string.IsNullOrEmpty(ScreenCode.Trim()))
                    caseNotesDetails.ScreenName = ScreenCode.Trim();
                else
                    caseNotesDetails.ScreenName = Privilege.Program;
                caseNotesDetails.FieldName = propKeyFiedld;
                caseNotesDetails.AppliCationNo = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + propBaseAppNo;
                caseNotesDetails.Mode = "Del";
                if (_model.TmsApcndata.InsertUpdateCaseNotes(caseNotesDetails))
                {
                    //this.DialogResult = DialogResult.OK;
                    //this.Close();
                    txtDesc.ReadOnly = true;
                    txtDesc.Clear();
                    //btnOk.Enabled = false;
                    //btnChange.Enabled = true;
                    strYear = "    ";
                    if (!string.IsNullOrEmpty(BaseForm.BaseYear))
                    {
                        strYear = BaseForm.BaseYear;
                    }
                    if (!string.IsNullOrEmpty(ScreenCode))
                        caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(ScreenCode.Trim(), propKeyFiedld);
                    else
                        caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privilege.Program, propKeyFiedld);

                    fillCaseNotes();
                }
                AlertBox.Show("Deleted Successfully");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtDesc.ReadOnly = false;
            btnOk.Visible = true;
            btnClose.Visible = true;
            btnAdd.Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            txtDesc.ReadOnly = true;
            Refresh_Control = "Close";
            fillCaseNotes();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (Privilege.Program == "CASE0027")
            {
                CaseNotesPrintPreview caseNotesPrintPreview = new CaseNotesPrintPreview(BaseForm, Privilege, strYear, _AppNo, _AppName);
                caseNotesPrintPreview.StartPosition = FormStartPosition.CenterScreen;
                caseNotesPrintPreview.ShowDialog();
            }
            else
            {
                CaseNotesPrintPreview caseNotesPrintPreview = new CaseNotesPrintPreview(BaseForm, Privilege, strYear, propBaseAppNo, propBaseAppName);
                caseNotesPrintPreview.StartPosition = FormStartPosition.CenterScreen;
                caseNotesPrintPreview.ShowDialog();
            }
        }

        //Report Of Case Notes Begin Section

        private void btnPrint_Click(object sender, EventArgs e)
        {
            On_SaveForm_Closed(PdfName, EventArgs.Empty);

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.StartPosition = FormStartPosition.CenterScreen;

                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.StartPosition = FormStartPosition.CenterScreen;

                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }
        }

        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(PdfName);
        }

        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string strFolderPath = string.Empty;
        string Random_Filename = null;
        string PdfName = "Pdf File";
        private void On_SaveForm_Closed(object sender, EventArgs e)
        {
            Random_Filename = null;

            PdfName = "CaseNotes_" + propBaseAppNo;
            //PdfName = strFolderPath + PdfName;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }

            try
            {
                string Tmpstr = PdfName + ".pdf";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            FileStream fs = new FileStream(PdfName, FileMode.Create);

            Document document = new Document();
            document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 16, 3, new iTextSharp.text.BaseColor(0, 0, 102));
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            float floatvalue = float.Parse(((Captain.Common.Utilities.ListItem)cmbsize.SelectedItem).Value.ToString());

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, floatvalue);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, floatvalue, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
            cb = writer.DirectContent;

            PdfPTable APP_details = new PdfPTable(2);
            APP_details.TotalWidth = 750f;
            APP_details.WidthPercentage = 100;
            APP_details.LockedWidth = true;
            float[] APP_details_Widths = new float[] { 30f, 70f };
            APP_details.SetWidths(APP_details_Widths);
            APP_details.HorizontalAlignment = Element.ALIGN_CENTER;
            APP_details.SpacingBefore = 9f;

            if (Privilege.Program.Trim() == "CASE0012")
            {
                PdfPCell Appl_No = new PdfPCell(new Phrase("Code:" + propBaseAppNo.Trim(), TblFontBold));
                Appl_No.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                Appl_No.Border = iTextSharp.text.Rectangle.NO_BORDER;
                APP_details.AddCell(Appl_No);

                PdfPCell App_Name = new PdfPCell(new Phrase("Volunteer/Donor Name :" + propBaseAppName.Trim(), TblFontBold));
                App_Name.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                App_Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
                APP_details.AddCell(App_Name);
            }
            else
            {

                PdfPCell Appl_No = new PdfPCell(new Phrase("App # :" + propBaseAppNo.Trim(), TblFontBold));
                Appl_No.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                Appl_No.Border = iTextSharp.text.Rectangle.NO_BORDER;
                APP_details.AddCell(Appl_No);

                PdfPCell App_Name = new PdfPCell(new Phrase("Client Name :" + propBaseAppName.Trim(), TblFontBold));
                App_Name.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                App_Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
                APP_details.AddCell(App_Name);
            }
            PdfPCell Date = new PdfPCell(new Phrase("", TblFontBold));
            Date.Colspan = 4;
            Date.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
            APP_details.AddCell(Date);
            List<CaseNotesEntity> casenotes_LIst = new List<CaseNotesEntity>();
            if (BaseForm.BaseYear.Trim() == string.Empty)
            {
                casenotes_LIst = _model.TmsApcndata.BrowseCaseNotes(Privilege.Program.Trim(), propKeyFiedld);//BaseForm.BaseAgency.Trim() + BaseForm.BaseDept.Trim() + BaseForm.BaseProg.Trim() + strYear + BaseForm.BaseApplicationNo.Trim(),
            }
            else
            {
                if (propKeyFiedld.Length > 18)
                    casenotes_LIst = _model.TmsApcndata.BrowseCaseNotes(Privilege.Program.Trim(), propKeyFiedld);
                else
                    casenotes_LIst = _model.TmsApcndata.BrowseCaseNotes(Privilege.Program.Trim(), BaseForm.BaseAgency.Trim() + BaseForm.BaseDept.Trim() + BaseForm.BaseProg.Trim() + strYear + propBaseAppNo.Trim());
            }
            string Screen_Name = null;
            DataSet dsScreen = DatabaseLayer.Lookups.GetScreens(Privilege.ModuleCode);
            DataTable dtScreen = dsScreen.Tables[0];
            if (casenotes_LIst.Count > 0)
            {
                foreach (CaseNotesEntity drCasenotes in casenotes_LIst)
                {
                    foreach (DataRow drScreen in dtScreen.Rows)
                    {
                        if (drCasenotes.ScreenName.Trim() == drScreen["CFC_PROGNO"].ToString().Trim())
                        {
                            Screen_Name = drScreen["CFC_DESCRIPTION"].ToString().Trim(); break;
                        }
                    }

                    PdfPCell Screen = new PdfPCell(new Phrase("Screen Name", TblFontBold));
                    Screen.Colspan = 2;
                    Screen.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    Screen.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                    APP_details.AddCell(Screen);

                    //PdfPCell Space = new PdfPCell(new Phrase("" , TblFontBold));
                    //Space.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    //Space.Border = iTextSharp.text.Rectangle.BOX;
                    //APP_details.AddCell(Space);

                    string CaseNotes_Desc = drCasenotes.Data.Trim();

                    PdfPCell Screen_desc = new PdfPCell(new Phrase(Screen_Name, Times));
                    Screen_desc.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    Screen_desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    APP_details.AddCell(Screen_desc);

                    PdfPCell Notes = new PdfPCell(new Phrase(CaseNotes_Desc, Times));
                    Notes.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    Notes.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    APP_details.AddCell(Notes);

                    PdfPCell Bottam_Border = new PdfPCell(new Phrase("", Times));
                    Bottam_Border.Colspan = 2;
                    Bottam_Border.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    Bottam_Border.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    APP_details.AddCell(Bottam_Border);

                }
            }

            document.Add(APP_details);
            document.Close();
            fs.Close();
            fs.Dispose();
        }

        // End Of Report Section......................


        string DepYear, Program_Year;
        bool DefHieExist = false;
        private void FillYearCombo(string Agy, string Dept, string Prog, string Year)
        {
            cmbYear.Visible = lblYear.Visible = DefHieExist = false;
            Program_Year = "    ";
            if (!string.IsNullOrEmpty(Year.Trim()))
                DefHieExist = true;

            DataSet ds = Captain.DatabaseLayer.MainMenu.GetCaseDepForHierarchy(Agy, Dept, Prog);
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                int YearIndex = 0;

                if (dt.Rows.Count > 0)
                {
                    Program_Year = DepYear = dt.Rows[0]["DEP_YEAR"].ToString();
                    if (!(String.IsNullOrEmpty(DepYear.Trim())) && DepYear != null && DepYear != "    ")
                    {
                        int TmpYear = int.Parse(DepYear);
                        int TempCompareYear = 0;
                        string TmpYearStr = null;
                        if (!(String.IsNullOrEmpty(Year)) && Year != null && Year != " " && DefHieExist)
                            TempCompareYear = int.Parse(Year);
                        List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
                        for (int i = 0; i < 10; i++)
                        {
                            TmpYearStr = (TmpYear - i).ToString();
                            listItem.Add(new Captain.Common.Utilities.ListItem(TmpYearStr, i));
                            if (TempCompareYear == (TmpYear - i) && TmpYear != 0 && TempCompareYear != 0)
                                YearIndex = i;
                        }

                        cmbYear.Items.AddRange(listItem.ToArray());

                        cmbYear.Visible = lblYear.Visible = true;

                        if (DefHieExist)
                            cmbYear.SelectedIndex = YearIndex;
                        else
                            cmbYear.SelectedIndex = 0;
                    }
                }
            }

        }



        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program_Year = "    ";
            if (!(string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbYear.SelectedItem).Text.ToString())))
            {
                Program_Year = ((Captain.Common.Utilities.ListItem)cmbYear.SelectedItem).Text.ToString();
                string strKeyfield = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + Program_Year + propBaseAppNo;
                if (!string.IsNullOrEmpty(ScreenCode.Trim()))
                    caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(ScreenCode.Trim(), strKeyfield);
                else
                    caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privilege.Program, strKeyfield);
                strYear = Program_Year;
                fillCaseNotes();

            }
            strYear = Program_Year;

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Application.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "casenotes");
        }

        Wisej.Web.Form _caseNotesForm;
        string Refresh_Control = string.Empty;

        private void CaseNotes_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnOk.Visible)
            {
                if (string.IsNullOrEmpty(Refresh_Control))
                {

                    DialogResult result = MessageBox.Show("Are you sure want to close? Record not saved", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerFormClosed);
                    _caseNotesForm = (Wisej.Web.Form)sender;
                    if (result == DialogResult.Yes)
                    {
                        e.Cancel = false;
                    }
                    else
                    {
                        e.Cancel = true;

                    }
                }
            }
        }

        private void MessageBoxHandlerFormClosed(DialogResult dialogResult)
        {
            // senderForm.FormClosing -= SampleTestForm_FormClosing;

            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                _caseNotesForm.FormClosing -= CaseNotes_FormClosing;
                _caseNotesForm.Close();
            }
        }




    }
}