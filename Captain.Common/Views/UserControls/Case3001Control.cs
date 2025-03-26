#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Web.Configuration;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Menus;
using System.Data.SqlClient;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Captain.Common.Views.Forms;
using System.IO;
using System.Linq;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using Wisej.Web;
using System.Globalization;
using DevExpress.XtraReports.UI;
using Captain.Common.Views.Controls.Compatibility;
using Captain.Common.Interfaces;
using DevExpress.XtraBars.Docking2010.Dragging;
using Microsoft.Ajax.Utilities;
using DevExpress.Web.Internal.XmlProcessor;
using NPOI.OpenXmlFormats.Spreadsheet;
using DevExpress.XtraCharts.Native;
using Microsoft.Practices.ObjectBuilder2;
#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class Case3001Control : BaseUserControl
    {

        #region private variables

        private CaptainModel _model = null;
        private AlertCodes alertCodesUserControl = null;
        private string strNameFormat = string.Empty;
        private string strCwFormat = string.Empty;
        private string strYear = "    ";
        private int strIndex = 0;

        #endregion

        DataTable dtDSSXMLData = new DataTable();
        public Case3001Control(BaseForm baseForm, PrivilegeEntity privileges)
            : base(baseForm)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privileges;
            _model = new CaptainModel();
            if (BaseForm.BaseCaseMstListEntity != null)
            {
                CaseMST = BaseForm.BaseCaseMstListEntity[0];
                CaseSnpEntityProp = BaseForm.BaseCaseSnpEntity;
            }
            MainMenuAppNo = string.Empty;
            ApplicantLastName = string.Empty;
            setControlEnabled(false);
            propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
            string HIE = CaseMST.ApplAgency + CaseMST.ApplDept + CaseMST.ApplProgram;
            preassesCntlEntity = _model.FieldControls.GetFLDCNTLHIE("PREASSES", HIE, "PREASSES");
            proppreassesQuestions = new List<CustomQuestionsEntity>();
            // AbcdControlsVisable();
            if (preassesCntlEntity.Count > 0)
            {
                if (!preassesCntlEntity.Exists(u => u.Enab.Equals("Y")))
                {
                    tabUserDetails.TabPages[5].Hidden = true;
                }
                else
                {
                    preassessMasterEntity = _model.lookupDataAccess.GetDimension();
                    // preassessMasterEntity = _model.FieldControls.GetPreassessData("MASTER");
                    preassessChildEntity = _model.FieldControls.GetPreassessData(string.Empty);
                    proppreassesQuestions = _model.FieldControls.GetPreassesQuestions("PREASSES", "A", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, "Sequence", "ACTIVE", "P");

                    tabUserDetails.TabPages[5].Show();
                    tabUserDetails.TabPages[5].Hidden = false;
                }
            }
            else
            {
                tabUserDetails.TabPages[5].Hidden = true;
            }

            CntlEntity = _model.FieldControls.GetFLDCNTLHIE("CASE2001", HIE, "FLDCNTL");
            if (CntlEntity.Count > 0)
            {
                CntlEntity = CntlEntity.FindAll(u => (u.FldCode == "S00670" && u.Enab == "Y") || (u.FldCode == "S00690" && u.Enab == "Y") || (u.FldCode == "S00700" && u.Enab == "Y") || (u.FldCode == "S00710" && u.Enab == "Y")
                || (u.FldCode == "S00711" && u.Enab == "Y") || (u.FldCode == "S00720" && u.Enab == "Y") || (u.FldCode == "S00730" && u.Enab == "Y") || (u.FldCode == "S00740" && u.Enab == "Y") || (u.FldCode == "S00750" && u.Enab == "Y")
                || (u.FldCode == "S00760" && u.Enab == "Y") || (u.FldCode == "S00765" && u.Enab == "Y") || (u.FldCode == "S00766" && u.Enab == "Y") || (u.FldCode == "S00767" && u.Enab == "Y") || (u.FldCode == "S00768" && u.Enab == "Y"));

                if (CntlEntity.Count > 0) { } else { tabUserDetails.TabPages[2].Hidden = true; }
            }

            CntlEntity = _model.FieldControls.GetFLDCNTLHIE("CASE2001", HIE, "FLDCNTL");
            if (CntlEntity.Count > 0)
            {
                CntlEntity = CntlEntity.FindAll(u => (u.FldCode == "S00920" && u.Enab == "Y") || (u.FldCode == "S00930" && u.Enab == "Y") || (u.FldCode == "S00940" && u.Enab == "Y") || (u.FldCode == "S00950" && u.Enab == "Y") || (u.FldCode == "S00960" && u.Enab == "Y")
                || (u.FldCode == "S00970" && u.Enab == "Y") || (u.FldCode == "S00980" && u.Enab == "Y") || (u.FldCode == "S00990" && u.Enab == "Y") || (u.FldCode == "S01000" && u.Enab == "Y") || (u.FldCode == "S01010" && u.Enab == "Y")
                || (u.FldCode == "S01020" && u.Enab == "Y") || (u.FldCode == "S01030" && u.Enab == "Y") || (u.FldCode == "S01040" && u.Enab == "Y") || (u.FldCode == "S01050" && u.Enab == "Y") || (u.FldCode == "S01060" && u.Enab == "Y")

                || (u.FldCode == "S01070" && u.Enab == "Y") || (u.FldCode == "S01080" && u.Enab == "Y") || (u.FldCode == "S01090" && u.Enab == "Y") || (u.FldCode == "S01100" && u.Enab == "Y") || (u.FldCode == "S01110" && u.Enab == "Y")
                || (u.FldCode == "S01120" && u.Enab == "Y") || (u.FldCode == "S01130" && u.Enab == "Y") || (u.FldCode == "S01140" && u.Enab == "Y")

                );

                if (CntlEntity.Count > 0) { }
                else
                {
                    tabUserDetails.TabPages[6].Hidden = true;

                }
            }


            Enableoutservicecmb();
            GetSelectedProgram();
            fillWaitList();
            fillOutofService();

            //HierarchyEntity HierarchyEntity = CommonFunctions.GetHierachyNameFormat(MainMenuAgency, "**", "**");
            //if (HierarchyEntity != null)
            //{
            strNameFormat = BaseForm.BaseHierarchyCnFormat.ToString();
            strCwFormat = BaseForm.BaseHierarchyCwFormat.ToString();
            //}

            fillDropDowns();
            fillEmployeeCombo();

            //strFolderPath = Consts.Common.ReportFolderLocation + BaseForm.UserID + "\\";
            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            if (programEntity != null)
            {
                ProgramDefinition = programEntity;
            }


            // List<PrivilegeEntity> userPrivilege = _model.UserProfileAccess.GetScreensByUserID(Privileges.ModuleCode, BaseForm.UserID, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg);
            List<PrivilegeEntity> userPrivilege = _model.UserProfileAccess.GetScreensByUserID(BaseForm.BusinessModuleID, BaseForm.UserID, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg);
            CaseVerPrivileges = Privileges;
            CaseIncomePrivileges = Privileges;
            TmsSupplierPrivileges = Privileges;
            ImageUploadPrivileges = Privileges;
            if (userPrivilege.Count > 0)
            {
                bool boolver, boolincome, boolsupp;
                boolver = boolincome = boolsupp = false;
                CaseVerPrivileges = userPrivilege.Find(u => u.Program == "CASE2003");
                CaseIncomePrivileges = userPrivilege.Find(u => u.Program == "CASINCOM");
                TmsSupplierPrivileges = userPrivilege.Find(u => u.Program == "TMS00201");
                ImageUploadPrivileges = userPrivilege.Find(u => u.Program == "IMGUPLOD");
                if (CaseVerPrivileges != null)
                {
                    if (CaseVerPrivileges.ViewPriv.Equals("true"))
                    {
                        btnIncVerfication.Visible = true;
                        boolver = true;
                    }
                    else
                    {
                        btnIncVerfication.Visible = false;
                    }
                }

                if (CaseIncomePrivileges != null)
                {
                    if (CaseIncomePrivileges.ViewPriv.Equals("true"))
                    {
                        btnIncomeDetails.Visible = true;
                        boolincome = true;
                    }
                    else
                    {
                        btnIncomeDetails.Visible = false;
                    }
                }
                if (TmsSupplierPrivileges != null)
                {
                    if (TmsSupplierPrivileges.ViewPriv.Equals("true"))
                    {
                        btnSupplier.Visible = true;
                        boolsupp = true;
                    }
                    else
                    {
                        btnSupplier.Visible = false;
                    }
                }
                if (ImageUploadPrivileges != null)
                {
                    if (ImageUploadPrivileges.ViewPriv.Equals("true"))
                    {
                        if (ToolBarImageTypes != null)
                            ToolBarImageTypes.Visible = true;
                    }
                    else
                    {
                        if (ToolBarImageTypes != null)
                            ToolBarImageTypes.Visible = false;
                    }
                }
                else
                {
                    if (ToolBarImageTypes != null)
                        ToolBarImageTypes.Visible = false;
                }


                if (boolincome == false && boolver == false && boolsupp == true)
                {
                    this.btnSupplier.Location = new System.Drawing.Point(5, 5);
                }
                if (boolincome == false && boolver == true && boolsupp == true)
                {
                    this.btnIncVerfication.Location = new System.Drawing.Point(5, 5);
                    this.btnSupplier.Location = new System.Drawing.Point(112, 5);
                }
                if (boolincome == true && boolver == false && boolsupp == true)
                {
                    this.btnSupplier.Location = new System.Drawing.Point(112, 5);
                }
                if (boolincome == false && boolver == true && boolsupp == false)
                {
                    this.btnIncVerfication.Location = new System.Drawing.Point(5, 5);
                }

            }

            fillClientIntake();
            loadCustomQuestions();
            alertCodesUserControl = new AlertCodes(BaseForm, privileges, ProgramDefinition);
            alertCodesUserControl.Dock = DockStyle.Fill;
            pnlAlertcode.Controls.Add(alertCodesUserControl);
            GetIncompleteIntake();
            DisplayIncomeMsgs();
            ShowFollowIndicator();
            propzipCodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(string.Empty, string.Empty, string.Empty, string.Empty);
            int endCellWidth = gvwCustomer.Width - 300;
            gvwCustomer.Columns["cellEnd"].Width = endCellWidth > 0 ? endCellWidth : 5;
            gvwCustomer.Update();
            //if (Privileges.ModuleCode.Trim() == "08" || Privileges.ModuleCode.Trim() == "09")
            //{
            //    btnSupplier.Visible = true;
            //}

            if (propAgencyControlDetails != null)
            {
                //if (propAgencyControlDetails.AgyShortName.ToUpper() == "UETHDA")
                //{
                //    lblPDss1.Text = "DHS Programs :";
                //    lblPDss2.Text = "Are you currently receiving DHS Services?";
                //}
                if (propAgencyControlDetails.State.ToUpper() == "TX")
                {
                    // lblHousingTX.Visible = true;
                    // cmbHousingTx.Visible = true;
                }
            }


            //if (gvwCustomer.Rows.Count > 0)
            //{
            //    gvwCustomer_SelectionChanged(gvwCustomer, new EventArgs());

            //}

            /**DSS XML DATA**/

            string strfileAGY = baseForm.BaseAgencyControlDetails.AgyShortName; //DSSXMLData.getZIPfileAGY(baseForm.BaseAgencyControlDetails.AgyShortName);
            dtDSSXMLData = DSSXMLData.DSSXMLMID_GET(baseForm.BaseDSSXMLDBConnString, strfileAGY, baseForm.BaseAgency, baseForm.BaseDept, baseForm.BaseProg, baseForm.BaseYear, BaseForm.BaseApplicationNo, "BYAPPNO");

            /***************/

            PopulateToolbar(oToolbarMnustrip);

            CommonFunctions.CnvtCntrl2Copy(txtHomePhone);//home phone number
            CommonFunctions.CnvtCntrl2Copy(txtCell);//cell number
            CommonFunctions.CnvtCntrl2Copy(txtEmail);//email
        }


        public void Refresh()
        {
            gvwCustomer.Rows.Clear();
            gvwCustomer.Update();
            gvwCustomer.ResumeLayout();
            txtInHouse.Text = string.Empty;
            txtTotalIncome.Text = string.Empty;
            txtProgramIncome.Text = string.Empty;
            txtInProg.Text = string.Empty;
            gvwPreassesData.Rows.Clear();
            ClearControls();
            GetSelectedProgram();
            fillEmployeeCombo();
            fillClientIntake();
            loadCustomQuestions();
            GetIncompleteIntake();
            ShowCaseNotesImages();
            RefreshAlertCode();
            ShowFollowIndicator();
            ShowHistoryIcon();
            string strfileAGY = BaseForm.BaseAgencyControlDetails.AgyShortName; //DSSXMLData.getZIPfileAGY(BaseForm.BaseAgencyControlDetails.AgyShortName);
            dtDSSXMLData = DSSXMLData.DSSXMLMID_GET(BaseForm.BaseDSSXMLDBConnString, strfileAGY, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, "BYAPPNO");
            if (dtDSSXMLData.Rows.Count > 0)
                ToolBarDSSXMLPdf.Visible = true;
            else
                ToolBarDSSXMLPdf.Visible = false;


            tabUserDetails.SelectedIndex = 0;
            if (gvwCustomer.Rows.Count != 0)
            {
                if (gvwCustomer.Rows.Count > strIndex)
                {
                    gvwCustomer.Rows[strIndex].Selected = true;
                    gvwCustomer.CurrentCell = gvwCustomer.Rows[strIndex].Cells[1];
                }
                else
                {
                    gvwCustomer.Rows[0].Selected = true;
                    gvwCustomer.CurrentCell = gvwCustomer.Rows[0].Cells[1];
                }
            }
            else
            {
                btnIncomeDetails.Enabled = false;
                btnIncVerfication.Enabled = false;
            }
            if (gvwCustomer.Rows.Count > 0)
            {
                gvwCustomer_SelectionChanged(gvwCustomer, new EventArgs());

            }
        }

        public void ShowFollowIndicator()
        {
            lblFollowInd.Visible = false;
            lblVerFollowupIndicator.Visible = false;
            if (BaseForm.BaseAgencyControlDetails.WorkerFUP.ToString().ToUpper() == "Y")
            {
                if (BaseForm.BaseCaseMstListEntity != null)
                {
                    if (BaseForm.BaseCaseMstListEntity[0].CompleteDate == string.Empty && BaseForm.BaseCaseMstListEntity[0].CaseReviewDate != string.Empty)
                    {

                        lblFollowInd.Visible = true;
                        string strType = CommonFunctions.FollowupIndicatior(BaseForm.BaseCaseMstListEntity[0].CaseReviewDate);
                        if (strType == "R")
                        {
                            lblFollowInd.ForeColor = Color.Red;
                            lblFollowInd.BackColor = Color.Transparent;
                        }
                        else if (strType == "Y")
                        {
                            lblFollowInd.ForeColor = Color.Black;
                            lblFollowInd.BackColor = Color.Yellow;
                        }
                        else
                        {
                            lblFollowInd.ForeColor = Color.Black;
                            lblFollowInd.BackColor = Color.Transparent;
                        }
                    }
                    if (BaseForm.BaseCaseMstListEntity[0].ReverifyDate != string.Empty)
                    {
                        if (btnIncVerfication.Visible == true)
                        {
                            lblVerFollowupIndicator.Visible = true;
                            string strType = CommonFunctions.FollowupIndicatior(BaseForm.BaseCaseMstListEntity[0].ReverifyDate);
                            if (strType == "R")
                            {
                                lblVerFollowupIndicator.ForeColor = Color.Red;
                                lblVerFollowupIndicator.BackColor = Color.Transparent;
                            }
                            else if (strType == "Y")
                            {
                                lblVerFollowupIndicator.ForeColor = Color.Black;
                                lblVerFollowupIndicator.BackColor = Color.Yellow;
                            }
                            else
                            {
                                lblVerFollowupIndicator.ForeColor = Color.Black;
                                lblVerFollowupIndicator.BackColor = Color.Transparent;
                            }
                        }
                    }
                }
            }
        }


        public void RefreshAlertCode()
        {
            pnlAlertcode.Controls.Clear();
            alertCodesUserControl = new AlertCodes(BaseForm, Privileges, ProgramDefinition);
            alertCodesUserControl.Dock = DockStyle.Fill;
            pnlAlertcode.Controls.Add(alertCodesUserControl);
            DisplayIncomeMsgs();
        }

        private void ShowCaseNotesImages()
        {
            strYear = "    ";
            if (!string.IsNullOrEmpty(BaseForm.BaseYear))
            {
                strYear = BaseForm.BaseYear;
            }
            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo);
            if (caseNotesEntity.Count > 0)
            {
                //ToolBarCaseNotes.ImageSource = Consts.Icons.ico_CaseNotes_View;
                ToolBarCaseNotes.ImageSource = "captain-casenotes";
            }
            else
            {
                //ToolBarCaseNotes.ImageSource = Consts.Icons.ico_CaseNotes_New;
                ToolBarCaseNotes.ImageSource = "captain-casenotesadd";
            }
            if (!(gvwCustomer.Rows.Count > 0))
            {
                ToolBarCaseNotes.Enabled = false;
                ToolBarImageTypes.Enabled = false;
                ToolBarHistory.Enabled = false;
                ToolBarEdit.Enabled = false;
                ToolBarNewMember.Enabled = false;
                ToolBarPrint.Enabled = false;
            }
            else
            {
                ToolBarCaseNotes.Enabled = true;
                ToolBarImageTypes.Enabled = true;
                ToolBarHistory.Enabled = true;
                if (Privileges.AddPriv.Equals("false"))
                {
                    if (ToolBarNewMember != null) ToolBarNewMember.Enabled = false;
                }
                else
                {
                    if (ToolBarNewMember != null) ToolBarNewMember.Enabled = true;
                }

                if (Privileges.ChangePriv.Equals("false"))
                {
                    if (ToolBarEdit != null) ToolBarEdit.Enabled = false;
                }
                else
                {
                    if (ToolBarEdit != null) ToolBarEdit.Enabled = true;
                }
                if (Privileges.DelPriv.Equals("false"))
                {
                    if (ToolBarDel != null) ToolBarDel.Enabled = false;
                }
                else
                {
                    if (ToolBarDel != null) ToolBarDel.Enabled = true;
                }

                //if (ProgramDefinition.DepYear.Trim() != BaseForm.BaseYear.Trim())
                //{
                //    if (ToolBarNewMember != null) ToolBarNewMember.Enabled = false;
                //    if (ToolBarEdit != null) ToolBarEdit.Enabled = false;
                //    if (ToolBarDel != null) ToolBarDel.Enabled = false;
                //}

                ToolBarPrint.Enabled = true;
                if (gvwCustomer.Rows.Count > 0)
                {
                    gvwCustomer_SelectionChanged(gvwCustomer, new EventArgs());

                }
            }
            if (ImageUploadPrivileges != null)
            {
                if (ImageUploadPrivileges.ViewPriv.Equals("true"))
                {
                    if (ToolBarImageTypes != null)
                        ToolBarImageTypes.Visible = true;
                }
                else
                {
                    if (ToolBarImageTypes != null)
                        ToolBarImageTypes.Visible = false;
                }
            }
            else
            {

                if (ToolBarImageTypes != null)
                    ToolBarImageTypes.Visible = false;
            }
        }

        private void setTooltip(int rowIndex, CaseSnpEntity entity)
        {
            string toolTipText = "Added By    : " + entity.AddOperator.ToString().Trim() + " on " + entity.DateAdd.ToString() + "\n";
            string modifiedBy = string.Empty;
            if (!entity.LstcOperator.ToString().Trim().Equals(string.Empty))
                modifiedBy = entity.LstcOperator.ToString().Trim() + " on " + entity.DateLstc.ToString();
            toolTipText += "Modified By : " + modifiedBy;
            foreach (DataGridViewCell cell in gvwCustomer.Rows[rowIndex].Cells)
            {
                cell.ToolTipText = toolTipText;
            }
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public PrivilegeEntity CaseVerPrivileges { get; set; }

        public PrivilegeEntity CaseIncomePrivileges { get; set; }

        public PrivilegeEntity TmsSupplierPrivileges { get; set; }

        public PrivilegeEntity ImageUploadPrivileges { get; set; }

        public ToolBarButton ToolBarEdit { get; set; }

        public ToolBarButton ToolBarDel { get; set; }//ToolBarPrint

        //public ToolBarButton ToolBarNew { get; set; }

        public ToolBarButton ToolBarPrint { get; set; }

        public ToolBarButton ToolBarNewMember { get; set; }

        public ToolBarButton ToolBarImageTypes { get; set; }

        public ToolBarButton ToolBarHistory { get; set; }

        public ToolBarButton ToolBarHelp { get; set; }
        public ToolBarButton ToolBarDSSXMLPdf { get; set; }
        public ToolBarButton ToolBarPrintLettersPdf { get; set; }

        public ToolBarButton ToolBarCaseNotes { get; set; }

        public ToolBarButton ToolBarSignature { get; set; }

        public ToolBarButton ToolBarRecentSearch { get; set; }

        public List<FldcntlHieEntity> CntlEntity { get; set; }

        public string MainMenuAgency { get; set; }

        public string MainMenuDept { get; set; }

        public string MainMenuProgram { get; set; }

        public string MainMenuYear { get; set; }

        public string MainMenuAppNo { get; set; }

        public string MainMenuHIE { get; set; }

        public CaseMstEntity CaseMST { get; set; }

        public List<CaseSnpEntity> CaseSnpEntityProp { get; set; }

        public bool IsAddApplicant { get; set; }

        public List<CaseNotesEntity> caseNotesEntity { get; set; }

        public ProgramDefinitionEntity ProgramDefinition { get; set; }

        public string ApplicantLastName { get; set; }
        public bool IsDeleteApplicant { get; set; }
        public List<ZipCodeEntity> propzipCodeEntity { get; set; }
        public List<FldcntlHieEntity> preassesCntlEntity { get; set; }
        public AgencyControlEntity propAgencyControlDetails { get; set; }
        public List<CommonEntity> preassessMasterEntity { get; set; }
        public List<PreassessQuesEntity> preassessChildEntity { get; set; }
        List<CustomQuestionsEntity> proppreassesQuestions { get; set; }
        public string propPreassStatus { get; set; }
        #endregion

        private void ClearControls()
        {
            // txtAlertCodes.Text = string.Empty;

            txtHN.Text = string.Empty;
            txtDirection.Text = string.Empty;
            txtFamilyID.Text = string.Empty;
            txtSuffix.Text = string.Empty;
            txtStreet.Text = string.Empty;
            txtFloor.Text = string.Empty;
            txtPrecinct.Text = string.Empty;
            txtApartment.Text = string.Empty;
            cbActive.Checked = false;
            cbJuvenile.Checked = false;
            cbSecret.Checked = false;
            cbSenior.Checked = false;
            txtZipCode.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtState.Text = string.Empty;
            txtSite.Text = string.Empty;
            CaseReviewDate.Text = string.Empty;


            SetComboBoxValue(cmbAboutUs, Consts.Common.SelectOne);
            SetComboBoxValue(cmbCaseType, Consts.Common.SelectOne);
            SetComboBoxValue(cmbContact, Consts.Common.SelectOne);
            SetComboBoxValue(cmbCounty, Consts.Common.SelectOne);
            SetComboBoxValue(cmbFamilyType, Consts.Common.SelectOne);
            SetComboBoxValue(cmbHousingSituation, Consts.Common.SelectOne);
            // SetComboBoxValue(cmbHousingTx, Consts.Common.SelectOne);
            SetComboBoxValue(cmbPrimaryLang, Consts.Common.SelectOne);
            SetComboBoxValue(cmbSecondLang, Consts.Common.SelectOne);
            SetComboBoxValue(cmbStaff, Consts.Common.SelectOne);
            SetComboBoxValue(cmbTownship, Consts.Common.SelectOne);
            SetComboBoxValue(cmbWaitingList, Consts.Common.SelectOne);
            SetComboBoxValue(cmbOutService, "");
            //Kranthi:: Added on 11/22/2022
            if (BaseForm.BusinessModuleID == "08")
            {
                SetComboBoxValue(cmbEnergydata, Consts.Common.SelectOne);

            }


            txtHomePhone.Text = string.Empty;
            txtCell.Text = string.Empty;
            txtMessage.Text = string.Empty;
            txtTTY.Text = string.Empty;
            txtFax.Text = string.Empty;
            txtEmail.Text = string.Empty;
            dtpInitialDate.Text = string.Empty;
            dtpIntakeDate.Text = string.Empty;
            dtpIntakeDate.Text = string.Empty;

            txtRent.Text = txtRent2.Text = string.Empty;
            txtHeating.Text = string.Empty;
            txtWater.Text = string.Empty;
            txtElectric.Text = string.Empty;
            txtExpand1.Text = string.Empty;
            txtExpand2.Text = string.Empty;
            txtExpand3.Text = string.Empty;
            txtExpand4.Text = string.Empty;
            txtTotalLiving.Text = string.Empty;
            txtTotalHouseHold.Text = string.Empty;

            txtFirst.Text = string.Empty;
            txtLast.Text = string.Empty;
            txtHouseNo.Text = string.Empty;
            txtCityName.Text = string.Empty;
            txtMailStreet.Text = string.Empty;
            txtMailZipCode.Text = string.Empty;
            txtMailZipPlus.Text = string.Empty;
            txtMailState.Text = string.Empty;
            txtMailSuffix.Text = string.Empty;
            txtMailApartment.Text = string.Empty;
            SetComboBoxValue(cmbMailCounty, Consts.Common.SelectOne);
            txtMailFloor.Text = string.Empty;
            txtMailPhone.Text = string.Empty;
        }

        private void GetSelectedProgram()
        {
            if (BaseForm.ContentTabs.TabPages[0].Controls[0] is MainMenuControl)
            {
                MainMenuControl mainMenuControl = (BaseForm.ContentTabs.TabPages[0].Controls[0] as MainMenuControl);
                //MainMenuAgency = mainMenuControl.Agency;
                //MainMenuDept = mainMenuControl.Dept;
                //MainMenuProgram = mainMenuControl.Program;
                //MainMenuYear = mainMenuControl.ProgramYear;
                //MainMenuAppNo = mainMenuControl.ApplicationNo;

                MainMenuAgency = BaseForm.BaseAgency;
                MainMenuDept = BaseForm.BaseDept;
                MainMenuProgram = BaseForm.BaseProg;
                MainMenuYear = BaseForm.BaseYear;
                MainMenuAppNo = BaseForm.BaseApplicationNo;

            }
        }

        private void OnSearchClick(object sender, EventArgs e)
        {
            gvwCustomer.Rows.Clear();

            if (gvwCustomer.Rows.Count > 0)
            {
                gvwCustomer_SelectionChanged(gvwCustomer, new EventArgs());
            }

        }

        private void gvwCustomer_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwCustomer.SelectedRows.Count > 0)
            {
                CaseSnpEntity caseSnpEntity = GetSelectedRow();
                CaseMstEntity caseMST = CaseMST;
                if (caseSnpEntity != null)
                {
                    if (ToolBarDel != null) ToolBarDel.Visible = true;
                    if (caseMST.FamilySeq.Equals(caseSnpEntity.FamilySeq))
                    {

                        if (gvwCustomer.Rows.Count == 1)
                        {
                            if (BaseForm.BaseAgencyControlDetails.DelAppSwitch == "Y")
                            {
                                if ((CaseMST.AddOperator1.Trim() == BaseForm.UserID.Trim()) || (BaseForm.UserProfile.Security.Trim() == "B" || BaseForm.UserProfile.Security.Trim() == "P"))
                                {
                                    if (ToolBarDel != null) ToolBarDel.Visible = true;
                                }
                                else
                                {
                                    if (ToolBarDel != null) ToolBarDel.Visible = false;
                                }
                            }
                            else
                            {
                                if (BaseForm.UserProfile.Security.Trim() == "B" || BaseForm.UserProfile.Security.Trim() == "P")
                                {
                                    if (ToolBarDel != null) ToolBarDel.Visible = true;
                                }
                                else
                                {
                                    if (ToolBarDel != null) ToolBarDel.Visible = false;
                                }
                            }
                        }
                        else
                        {
                            if (ToolBarDel != null) ToolBarDel.Visible = false;
                        }
                    }
                    else
                    {
                        fillEmployeeCombo();
                        getEmployeeIntake(caseSnpEntity);
                        ShowHistoryIcon();
                    }
                }
                setControlEnabled(false);
            }
        }

        public override void PopulateToolbar(ToolBar toolBar)
        {
            base.PopulateToolbar(toolBar);

            bool toolbarButtonInitialized = ToolBarNewMember != null;
            ToolBarButton divider = new ToolBarButton();
            divider.Style = ToolBarButtonStyle.Separator;

            if (toolBar.Controls.Count == 0)
            {
                //ToolBarNew = new ToolBarButton();
                //ToolBarNew.Tag = "NewApp";
                //ToolBarNew.ToolTipText = "New Applicant";
                //ToolBarNew.Enabled = true;
                //ToolBarNew.Image = new IconResourceHandle(Consts.Icons16x16.AddItem);
                //ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                //ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);




                ToolBarNewMember = new ToolBarButton();
                ToolBarNewMember.Tag = "NewMem";
                ToolBarNewMember.ToolTipText = "Add Household Member";
                ToolBarNewMember.Enabled = true;
                ToolBarNewMember.ImageSource = "captain-add";
                ToolBarNewMember.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNewMember.Click += new EventHandler(OnToolbarButtonClicked);


                ToolBarEdit = new ToolBarButton();
                ToolBarEdit.Tag = "Edit";
                ToolBarEdit.ToolTipText = "Edit Member Details";
                ToolBarEdit.Enabled = true;
                ToolBarEdit.ImageSource = "captain-edit";
                ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);
                if (Privileges.ChangePriv.Equals("false"))
                {
                    ToolBarEdit.Enabled = false;
                }

                ToolBarDel = new ToolBarButton();
                ToolBarDel.Tag = "Delete";
                ToolBarDel.ToolTipText = "Delete Member";
                ToolBarDel.Enabled = true;
                ToolBarDel.ImageSource = "captain-delete";
                ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);

                //ToolBarDel = new ToolBarButton();
                //ToolBarDel.Tag = "Delete";
                //ToolBarDel.ToolTipText = "Delete Member";
                //ToolBarDel.Enabled = true;
                //ToolBarDel.ImageSource = "captain-delete";
                //ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
                //ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarImageTypes = new ToolBarButton();
                ToolBarImageTypes.Tag = "ImageTypes";
                ToolBarImageTypes.ToolTipText = "Image Upload";
                ToolBarImageTypes.Enabled = true;
                ToolBarImageTypes.ImageSource = "captain-imagetypeupload";
                //ToolBarImageTypes.Image = new IconResourceHandle(Consts.Icons16x16.ImageType);
                ToolBarImageTypes.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarImageTypes.Click += new EventHandler(OnToolbarButtonClicked);


                ToolBarHistory = new ToolBarButton();
                ToolBarHistory.Tag = "History";
                ToolBarHistory.ToolTipText = "Case History";
                ToolBarHistory.Enabled = true;
                ToolBarHistory.ImageSource = "captain-caseHistory";
                //ToolBarHistory.Image = new IconResourceHandle(Consts.Icons16x16.HistoryImage);
                ToolBarHistory.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarHistory.Click += new EventHandler(OnToolbarButtonClicked);



                ToolBarCaseNotes = new ToolBarButton();
                ToolBarCaseNotes.Tag = "CaseNotes";
                ToolBarCaseNotes.ToolTipText = "Case Notes";
                ToolBarCaseNotes.Enabled = true;
                ToolBarCaseNotes.ImageSource = "captain-casenotes";
                ToolBarCaseNotes.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarCaseNotes.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarPrint = new ToolBarButton();
                ToolBarPrint.Tag = "Print";
                ToolBarPrint.ToolTipText = "Print Form";
                ToolBarPrint.Enabled = true;
                ToolBarPrint.ImageSource = "captain-print";
                //ToolBarPrint.Image = new IconResourceHandle(Consts.Icons16x16.Print);
                ToolBarPrint.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarPrint.Click += new EventHandler(OnToolbarButtonClicked);

                //ToolBarSignature = new ToolBarButton();
                //ToolBarSignature.Tag = "Signature";
                //ToolBarSignature.ToolTipText = "Signature Form";
                //ToolBarSignature.Enabled = true;
                //ToolBarSignature.Image = new IconResourceHandle(Consts.Icons16x16.Signature);
                //ToolBarSignature.Click -= new EventHandler(OnToolbarButtonClicked);
                //ToolBarSignature.Click += new EventHandler(OnToolbarButtonClicked);

                //ToolBarSignature = new ToolBarButton();
                //ToolBarSignature.Tag = "RecentSearch";
                //ToolBarSignature.ToolTipText = "Recent Applicant";
                //ToolBarSignature.Enabled = true;
                //ToolBarSignature.Image = new IconResourceHandle(Consts.Icons16x16.RecentSearch);
                //ToolBarSignature.Click -= new EventHandler(OnToolbarButtonClicked);
                //ToolBarSignature.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarPrintLettersPdf = new ToolBarButton();
                ToolBarPrintLettersPdf.Tag = "PrintLetters";
                ToolBarPrintLettersPdf.ToolTipText = "Print Letters";
                ToolBarPrintLettersPdf.Enabled = true;
                //ToolBarHelp.Image = new IconResourceHandle(Consts.Icons16x16.Help);
                ToolBarPrintLettersPdf.ImageSource = "captain-pdf";
                ToolBarPrintLettersPdf.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarPrintLettersPdf.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarDSSXMLPdf = new ToolBarButton();
                ToolBarDSSXMLPdf.Tag = "DSSXMLPdf";
                ToolBarDSSXMLPdf.ToolTipText = "DSS Intake";
                ToolBarDSSXMLPdf.Enabled = true;
                //ToolBarHelp.Image = new IconResourceHandle(Consts.Icons16x16.Help);
                ToolBarDSSXMLPdf.ImageSource = "captain-pdf";
                ToolBarDSSXMLPdf.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarDSSXMLPdf.Click += new EventHandler(OnToolbarButtonClicked);


                ToolBarHelp = new ToolBarButton();
                ToolBarHelp.Tag = "Help";
                ToolBarHelp.ToolTipText = "Help";
                ToolBarHelp.Enabled = true;
                //ToolBarHelp.Image = new IconResourceHandle(Consts.Icons16x16.Help);
                ToolBarHelp.ImageSource = "icon-help";
                ToolBarHelp.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarHelp.Click += new EventHandler(OnToolbarButtonClicked);
            }
            if (Privileges.AddPriv.Equals("false"))
            {
                //if (ToolBarNew != null) ToolBarNew.Enabled = false;
                if (ToolBarNewMember != null) ToolBarNewMember.Enabled = false;
            }
            else
            {
                // if (ToolBarNew != null) ToolBarNew.Enabled = true;
                if (ToolBarNewMember != null) ToolBarNewMember.Enabled = true;
            }

            if (Privileges.ChangePriv.Equals("false"))
            {
                if (ToolBarEdit != null) ToolBarEdit.Enabled = false;
            }
            else
            {
                if (ToolBarEdit != null) ToolBarEdit.Enabled = true;
            }


            if (Privileges.DelPriv.Equals("false"))
            {
                if (ToolBarDel != null) ToolBarDel.Enabled = false;
            }
            else
            {
                if (ToolBarDel != null) ToolBarDel.Enabled = true;
            }

            //if (ProgramDefinition.DepYear.Trim() != BaseForm.BaseYear.Trim())
            //{
            //    if (ToolBarNewMember != null) ToolBarNewMember.Enabled = false;
            //    if (ToolBarEdit != null) ToolBarEdit.Enabled = false;
            //    if (ToolBarDel != null) ToolBarDel.Enabled = false;
            //}


            if (dtDSSXMLData.Rows.Count > 0)
                ToolBarDSSXMLPdf.Visible = true;
            else
                ToolBarDSSXMLPdf.Visible = false;


            /************************************************/
            if (propAgencyControlDetails.State.ToUpper() == "TX" || propAgencyControlDetails.State.ToUpper() == "IN")
                ToolBarPrintLettersPdf.Visible = true;
            else
                ToolBarPrintLettersPdf.Visible = false;


            /**********************************************/
            ShowCaseNotesImages();
            ShowHistoryIcon();
            toolBar.Buttons.AddRange(new ToolBarButton[]
            {
               // ToolBarNew,
                ToolBarNewMember,
                ToolBarEdit,
                ToolBarDel,
                ToolBarImageTypes,
                ToolBarHistory,
                ToolBarCaseNotes,
                ToolBarPrint,
                //ToolBarSignature,
                ToolBarDSSXMLPdf,
                ToolBarPrintLettersPdf,
                ToolBarHelp
            });
        }

        /// <summary>
        /// Handles the toolbar button clicked event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnToolbarButtonClicked(object sender, EventArgs e)
        {
            ToolBarButton btn = (ToolBarButton)sender;
            StringBuilder executeCode = new StringBuilder();

            executeCode.Append(Consts.Javascript.BeginJavascriptCode);
            if (btn.Tag == null) { return; }
            try
            {
                switch (btn.Tag.ToString())
                {
                    case "NewApp":
                        if (CheckCasedepprivileage())
                        {
                            CaseMstEntity caseMSTEntity = new CaseMstEntity();
                            caseMSTEntity.ApplAgency = MainMenuAgency;
                            caseMSTEntity.ApplDept = MainMenuDept;
                            caseMSTEntity.ApplProgram = MainMenuProgram;
                            caseMSTEntity.ApplYr = MainMenuYear;
                            BaseForm.RemoveTabPages("Client");
                            if (Privileges.Program.ToString().ToUpper() == "CASE2002")
                            {
                                Case3001Form clientSNPForm = new Case3001Form(BaseForm, true, caseMSTEntity, null, Consts.Common.Add, Privileges, CaseSnpEntityProp, ApplicantLastName);
                                clientSNPForm.StartPosition = FormStartPosition.CenterScreen;
                                clientSNPForm.ShowDialog();
                            }
                            else
                            {
                                if (BaseForm.BaseAgencyControlDetails.RomaSwitch.ToUpper() == "Y")
                                {
                                    Case4001Form clientSNPForm = new Case4001Form(BaseForm, true, caseMSTEntity, null, Consts.Common.Add, Privileges, CaseSnpEntityProp, ApplicantLastName);
                                    clientSNPForm.StartPosition = FormStartPosition.CenterScreen;
                                    clientSNPForm.ShowDialog();
                                }
                                else
                                {
                                    Case3001Form clientSNPForm = new Case3001Form(BaseForm, true, caseMSTEntity, null, Consts.Common.Add, Privileges, CaseSnpEntityProp, ApplicantLastName);
                                    clientSNPForm.StartPosition = FormStartPosition.CenterScreen;
                                    clientSNPForm.ShowDialog();
                                }
                            }
                        }
                        else
                        {
                            AlertBox.Show("Cannot add to prior program year", MessageBoxIcon.Warning);
                        }
                        break;
                    case "NewMem":
                        CaseMstEntity caseMST = CaseMST;
                        if (CheckCasedepprivileage())
                        {
                            if (Privileges.Program.ToString().ToUpper() == "CASE2002")
                            {
                                Case3001Form clientSNPMemForm = new Case3001Form(BaseForm, false, caseMST, null, Consts.Common.Add, Privileges, CaseSnpEntityProp, ApplicantLastName);
                                clientSNPMemForm.StartPosition = FormStartPosition.CenterScreen;
                                clientSNPMemForm.ShowDialog();
                            }
                            else
                            {
                                if (BaseForm.BaseAgencyControlDetails.RomaSwitch.ToUpper() == "Y")//(BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "COI" || BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "OK" || BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "SCCAP")
                                {
                                    Case4001Form clientSNPMemForm = new Case4001Form(BaseForm, false, caseMST, null, Consts.Common.Add, Privileges, CaseSnpEntityProp, ApplicantLastName);
                                    clientSNPMemForm.StartPosition = FormStartPosition.CenterScreen;
                                    clientSNPMemForm.Show();

                                    if (Consts.Common.Add == "Add")
                                    {
                                        //if (!IsApplicant && mskSSN.Enabled == true)
                                        //{
                                        //CommonFunctions.MessageBoxDisplay("Field controls not defined for this program");
                                        SSNSearchForm SSNSearchForm = new SSNSearchForm(BaseForm, "Case2001", null, ProgramDefinition, CaseMST, "A", "ALL", Privileges);
                                        SSNSearchForm.FormClosed += new FormClosedEventHandler(clientSNPMemForm.OnSearchFormClosed);
                                        SSNSearchForm.StartPosition = FormStartPosition.CenterScreen;
                                        SSNSearchForm.TopMost = true;
                                        SSNSearchForm.ShowDialog();
                                        //}
                                    }
                                }
                                else
                                {
                                    Case3001Form clientSNPMemForm = new Case3001Form(BaseForm, false, caseMST, null, Consts.Common.Add, Privileges, CaseSnpEntityProp, ApplicantLastName);
                                    clientSNPMemForm.StartPosition = FormStartPosition.CenterScreen;
                                    clientSNPMemForm.ShowDialog();
                                }
                            }
                        }
                        else
                        {
                            AlertBox.Show("Cannot add to prior program year", MessageBoxIcon.Warning);
                        }
                        break;
                    case Consts.ToolbarActions.Edit:
                        CaseSnpEntity caseSnp = GetSelectedRow();
                        caseMST = CaseMST;
                        caseMST = _model.CaseMstData.GetCaseMST(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
                        bool isApplicant = false;
                        if (caseMST.FamilySeq.Equals(caseSnp.FamilySeq)) isApplicant = true;
                        if (CheckCasedepprivileage())
                        {
                            if (Privileges.Program.ToString().ToUpper() == "CASE2002")
                            {
                                Case3001Form clientSNPEditForm = new Case3001Form(BaseForm, isApplicant, caseMST, caseSnp, Consts.Common.Edit, Privileges, CaseSnpEntityProp, ApplicantLastName);
                                clientSNPEditForm.StartPosition = FormStartPosition.CenterScreen;
                                clientSNPEditForm.ShowDialog();
                            }
                            else
                            {
                                if (BaseForm.BaseAgencyControlDetails.RomaSwitch.ToUpper() == "Y")
                                {
                                    Case4001Form clientSNPEditForm = new Case4001Form(BaseForm, isApplicant, caseMST, caseSnp, Consts.Common.Edit, Privileges, CaseSnpEntityProp, ApplicantLastName);
                                    clientSNPEditForm.StartPosition = FormStartPosition.CenterScreen;
                                    clientSNPEditForm.ShowDialog();
                                }
                                else
                                {
                                    Case3001Form clientSNPEditForm = new Case3001Form(BaseForm, isApplicant, caseMST, caseSnp, Consts.Common.Edit, Privileges, CaseSnpEntityProp, ApplicantLastName);
                                    clientSNPEditForm.StartPosition = FormStartPosition.CenterScreen;
                                    clientSNPEditForm.ShowDialog();
                                }
                            }
                        }
                        else
                        {
                            AlertBox.Show("Cannot edit to prior program year", MessageBoxIcon.Warning);
                        }
                        break;
                    case Consts.ToolbarActions.Delete:
                        caseSnp = GetSelectedRow();
                        caseMST = CaseMST;
                        if (CheckCasedepprivileage())
                        {
                            if (caseSnp != null)
                            {
                                IsDeleteApplicant = false;
                                isApplicant = false;
                                if (caseMST.FamilySeq.Equals(caseSnp.FamilySeq)) isApplicant = true;
                                if (isApplicant)
                                {
                                    IsDeleteApplicant = true;
                                    if (gvwCustomer.Rows.Count == 1)
                                    {
                                        if (BaseForm.UserProfile.Security.ToUpper() == "P" || BaseForm.UserProfile.Security.ToUpper() == "B")
                                        {
                                            string snpName = caseSnp.NameixFi.Trim() + " " + caseSnp.NameixLast.Trim();
                                            MessageBox.Show("Are you sure you want to delete " + snpName + " record?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
                                        }
                                        else
                                        {
                                            if (BaseForm.BaseAgencyControlDetails.DelAppSwitch == "Y")
                                            {
                                                if ((CaseMST.AddOperator1.Trim() == BaseForm.UserID.Trim()))
                                                {
                                                    string snpName = caseSnp.NameixFi.Trim() + " " + caseSnp.NameixLast.Trim();
                                                    MessageBox.Show("Are you sure you want to delete " + snpName + " record?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
                                                }
                                            }
                                            else
                                            {
                                                AlertBox.Show("Program Administrator can only delete an applicant", MessageBoxIcon.Warning);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    string snpName = caseSnp.NameixFi.Trim() + " " + caseSnp.NameixLast.Trim();
                                    MessageBox.Show("Are you sure you want to delete " + snpName + " record?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
                                }
                            }
                        }
                        else
                        {
                            CommonFunctions.MessageBoxDisplay("Cannot delete to prior program year");
                        }
                        break;
                    case Consts.ToolbarActions.ImageTypes:
                        // caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo);
                        ImageUpload imageupload = new ImageUpload(BaseForm, ImageUploadPrivileges, ProgramDefinition, "CASE2001");
                        //caseNotes.FormClosed += new Form.FormClosedEventHandler(OnCaseNotesFormClosed);
                        imageupload.StartPosition = FormStartPosition.CenterScreen;
                        imageupload.ShowDialog();
                        break;

                    case Consts.ToolbarActions.History:
                        CaseSnpEntity caseHistSnp = GetSelectedRow();
                        if (caseHistSnp != null)
                        {
                            HistoryForm historyForm = new HistoryForm(BaseForm, Privileges, caseHistSnp);
                            historyForm.StartPosition = FormStartPosition.CenterScreen;
                            historyForm.ShowDialog();
                        }
                        break;

                    case Consts.ToolbarActions.CaseNotes:
                        if (BaseForm.BusinessModuleID == "05")
                        {
                            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo);
                            ProgressNotes_Form Prog_Form = new ProgressNotes_Form(BaseForm, "View", Privileges, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo); //+ "0000".Substring(0, (4 - Pass_Entity.Seq.Length)) + Pass_Entity.Seq);
                            Prog_Form.FormClosed += new FormClosedEventHandler(OnCaseNotesFormClosed);
                            Prog_Form.StartPosition = FormStartPosition.CenterScreen;
                            Prog_Form.ShowDialog();
                        }
                        else
                        {
                            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo);
                            CaseNotes caseNotes = new CaseNotes(BaseForm, Privileges, caseNotesEntity, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo);
                            caseNotes.FormClosed += new FormClosedEventHandler(OnCaseNotesFormClosed);
                            caseNotes.StartPosition = FormStartPosition.CenterScreen;
                            caseNotes.ShowDialog();
                        }
                        break;
                    case Consts.ToolbarActions.Print:
                        PrintApplicants PrintAppl = new PrintApplicants(BaseForm, Privileges, "Case2001");
                        PrintAppl.StartPosition = FormStartPosition.CenterScreen;
                        PrintAppl.ShowDialog();
                        //On_SaveFormClosed();
                        break;
                    case Consts.ToolbarActions.RecentSearch:

                        CaseSnpEntity snpApplicant = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq);

                        DataSet ds = Captain.DatabaseLayer.MainMenu.MainMenuSearchEMS("APP", "ALL", null, null, null, snpApplicant.NameixLast, snpApplicant.NameixFi, null, null, null, null, null, null, null, null, null, snpApplicant.AltBdate, BaseForm.UserID, "Single", string.Empty, string.Empty);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0]["AGENCY"].ToString() == BaseForm.BaseAgency && ds.Tables[0].Rows[0]["DEPT"].ToString() == BaseForm.BaseDept && ds.Tables[0].Rows[0]["PROG"].ToString() == BaseForm.BaseProg && ds.Tables[0].Rows[0]["SnpYear"].ToString().Trim() == BaseForm.BaseYear.Trim() && ds.Tables[0].Rows[0]["APPLICANTNO"].ToString() == BaseForm.BaseApplicationNo)
                            {
                                AlertBox.Show("This Applicant is recent record", MessageBoxIcon.Warning);
                            }
                            else
                            {
                                //PIPUpdateApplicantForm pipupdateForm = new Forms.PIPUpdateApplicantForm(BaseForm, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
                                ////pipupdateForm.FormClosed += new FormClosedEventHandler(PipupdateForm_FormClosed);
                                //pipupdateForm.ShowDialog();
                            }
                        }
                        break;

                    case "DSSXMLPdf":
                        //DSSINTAKE_Form dssIntake = new DSSINTAKE_Form(BaseForm, Privileges);
                        //dssIntake.StartPosition = FormStartPosition.CenterScreen;
                        //dssIntake.ShowDialog();
                        ////PrintDSSXMLPdf();
                        break;

                    case "PrintLetters":
                        Fill_Applicant_SPs();

                        if (CASESPM_SP_List.Count > 0)
                            CASESPM_SP_List = CASESPM_SP_List.FindAll(u => u.SPM_EligStatus != string.Empty);
                        string strSPMSEQ = string.Empty;
                        string strEligStatus = string.Empty; string strSpcCode = string.Empty;
                        if (CASESPM_SP_List.Count > 0)
                        {
                          

                            strSpcCode = CASESPM_SP_List[0].service_plan;// gvwBenefit.CurrentRow.Cells["gvSPCode"].Value.ToString();
                            strSPMSEQ = CASESPM_SP_List[0].Seq; // gvwBenefit.CurrentRow.Cells["gvSPMSeq"].Value.ToString();
                           

                            string Status = string.Empty;
                            if (CASESPM_SP_List[0].SPM_EligStatus.Trim() == "P") Status = "Pending";
                            else if (CASESPM_SP_List[0].SPM_EligStatus.Trim() == "E") Status = "Eligible";
                            else if (CASESPM_SP_List[0].SPM_EligStatus.Trim() == "D") Status = "Denied";
                            else if (CASESPM_SP_List[0].SPM_EligStatus.Trim() == "S") Status = "Eligible";
                            else if (CASESPM_SP_List[0].SPM_EligStatus.Trim() == "M") Status = "Eligible";
                            else if (CASESPM_SP_List[0].SPM_EligStatus.Trim() == "N") Status = "Eligible";

                            strEligStatus = Status;// gvwBenefit.CurrentRow.Cells["gvStatus"].Value.ToString();
                        }
                        //string SecSource = string.Empty;
                        //if (((Utilities.ListItem)cmbSource.SelectedItem).Value.ToString() != "0")
                        //    SecSource = ((Utilities.ListItem)cmbSource.SelectedItem).Text.ToString();

                        PrintLetters PrintApplL = new PrintLetters(BaseForm, Privileges, "CASE3001", strEligStatus, CASESPM_SP_List, strSpcCode, strSPMSEQ, "",string.Empty,string.Empty); 
                        //PrintLetters PrintApplL = new PrintLetters(BaseForm, Privileges, "CASE3001");
                        PrintApplL.StartPosition = FormStartPosition.CenterScreen;
                        PrintApplL.ShowDialog();
                        break;
                    //case Consts.ToolbarActions.Signature:
                    //    string strSignYear = "YYYY";
                    //    if (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
                    //    {
                    //        strSignYear = BaseForm.BaseYear;
                    //    }
                    //    PdfViewerNewForm Signature = new PdfViewerNewForm(string.Empty, BaseForm.BaseAgency.ToString() + BaseForm.BaseDept.ToString() + BaseForm.BaseProg.ToString() + strSignYear + BaseForm.BaseApplicationNo.ToString());
                    //    Signature.ShowDialog();

                    //    break;
                    case Consts.ToolbarActions.Help:
                        
                        //Application.Navigate(CommonFunctions.BuildHelpURLS(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString()), target: "_blank");
                        Application.Navigate(CommonFunctions.CreateZenHelps("CASE2001",0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");

                        break;

                }
                executeCode.Append(Consts.Javascript.EndJavascriptCode);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
            }
        }

        List<CASESPMEntity> CASESPM_SP_List;
        CASESPMEntity Search_Entity = new CASESPMEntity();
        private void Fill_Applicant_SPs()
        {
            Search_Entity.agency = BaseForm.BaseAgency;
            Search_Entity.dept = BaseForm.BaseDept;
            Search_Entity.program = BaseForm.BaseProg;
            //Search_Entity.year = BaseYear;
            Search_Entity.year = null;                          // Year will be always Four-Spaces in CASESPM
            Search_Entity.app_no = BaseForm.BaseApplicationNo;

            Search_Entity.service_plan = Search_Entity.caseworker = Search_Entity.site = null;
            Search_Entity.startdate = Search_Entity.estdate = Search_Entity.compdate = null;
            Search_Entity.sel_branches = Search_Entity.have_addlbr = Search_Entity.date_lstc = null;
            Search_Entity.lstc_operator = Search_Entity.date_add = Search_Entity.add_operator = null;
            Search_Entity.Sp0_Desc = Search_Entity.Sp0_Validatetd = Search_Entity.Def_Program = //Search_Entity.SPM_MassClose =
            Search_Entity.SPM_MassClose = Search_Entity.Seq = Search_Entity.Bulk_Post = null;

            CASESPM_SP_List = _model.SPAdminData.Browse_CASESPM(Search_Entity, "Browse");
        }
        private bool CheckCasedepprivileage()
        {
            bool boolprivilege = true;
            if (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
            {
                switch (BaseForm.UserProfile.Security)
                {
                    case "R":
                    case "C":
                        if (ProgramDefinition.DepYear != BaseForm.BaseYear.Trim())
                        {
                            boolprivilege = false;
                        }
                        break;
                }
            }
            return boolprivilege;
        }

        private void MessageBoxHandler(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                bool boolDeleteObo = true;
                bool boolDeleteCAObo = true;
                CaseSnpEntity caseSnp = GetSelectedRow();
                CASEACTEntity search_Act_Details = new CASEACTEntity(true);
                CASEMSEntity Search_MS_Details = new CASEMSEntity(true);
                Search_MS_Details.Agency = caseSnp.Agency;
                Search_MS_Details.Dept = caseSnp.Dept;
                Search_MS_Details.Program = caseSnp.Program;
                Search_MS_Details.Year = caseSnp.Year;                              // Year will be always Four-Spaces in CASEMS
                Search_MS_Details.App_no = caseSnp.App;

                search_Act_Details.Agency = caseSnp.Agency;
                search_Act_Details.Dept = caseSnp.Dept;
                search_Act_Details.Program = caseSnp.Program;
                search_Act_Details.Year = caseSnp.Year;                              // Year will be always Four-Spaces in CASEMS
                search_Act_Details.App_no = caseSnp.App;

                List<CASEACTEntity> Tmp_SP_ACT_Details = _model.SPAdminData.Browse_CASEACT(search_Act_Details, "Browse");
                if (Tmp_SP_ACT_Details.Count > 0)
                {
                    foreach (CASEACTEntity Actitem in Tmp_SP_ACT_Details)
                    {
                        CAOBOEntity Search_CAOBO_Entity = new CAOBOEntity();
                        Search_CAOBO_Entity.ID = Actitem.ACT_ID;
                        Search_CAOBO_Entity.Fam_Seq = caseSnp.FamilySeq;
                        Search_CAOBO_Entity.Seq = Search_CAOBO_Entity.CLID = string.Empty;

                        List<CAOBOEntity> CAOBO_List = _model.SPAdminData.Browse_CAOBO(Search_CAOBO_Entity, "Browse");
                        if (CAOBO_List.Count > 0)
                            boolDeleteCAObo = false;
                    }

                }

                List<CASEMSEntity> Tmp_SP_MS_Details = _model.SPAdminData.Browse_CASEMS(Search_MS_Details, "Browse");
                if (Tmp_SP_MS_Details.Count > 0)
                {
                    foreach (CASEMSEntity Msitem in Tmp_SP_MS_Details)
                    {
                        CASEMSOBOEntity Search_OBO_Entity = new CASEMSOBOEntity();
                        Search_OBO_Entity.ID = Msitem.ID;
                        Search_OBO_Entity.Fam_Seq = caseSnp.FamilySeq;
                        Search_OBO_Entity.Seq = Search_OBO_Entity.CLID = string.Empty;

                        List<CASEMSOBOEntity> CASEMSOBO_List = _model.SPAdminData.Browse_CASEMSOBO(Search_OBO_Entity, "Browse");
                        if (CASEMSOBO_List.Count > 0)
                            boolDeleteObo = false;
                    }

                }
                if (boolDeleteCAObo)
                {
                    if (boolDeleteObo)
                    {
                        string strMsg = _model.CaseMstData.DeleteCaseSNP(caseSnp, BaseForm.UserID);
                        if (strMsg == "Success")
                        {
                            if (strIndex != 0)
                                strIndex = strIndex - 1;
                            //Refresh();   

                            CaseMstEntity caseMstEntity = null;
                            List<CaseSnpEntity> caseSnpEntity = null;
                            string strYear1 = BaseForm.BaseYear;

                            if (string.IsNullOrEmpty(BaseForm.BaseYear))
                                strYear1 = "    ";


                            string strAgencyName = BaseForm.BaseAgency + " - " + _model.lookupDataAccess.GetHierachyDescription("1", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
                            string strDeptName = BaseForm.BaseDept + " - " + _model.lookupDataAccess.GetHierachyDescription("2", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
                            string strProgName = BaseForm.BaseProg + " - " + _model.lookupDataAccess.GetHierachyDescription("3", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
                            if (gvwCustomer.Rows.Count == 0)
                            {
                                caseMstEntity = _model.CaseMstData.GetCaseMST(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, strYear1, string.Empty);
                                List<CaseMstEntity> caseMstList = new List<CaseMstEntity>();
                                caseMstList.Add(caseMstEntity);
                                BaseForm.BaseCaseMstListEntity = caseMstList;
                            }
                            else
                            {
                                caseMstEntity = _model.CaseMstData.GetCaseMST(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
                            }
                            if (caseMstEntity != null)
                            {

                                BaseForm.BaseApplicationNo = caseMstEntity.ApplNo; ;
                                caseSnpEntity = _model.CaseMstData.GetCaseSnpadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, strYear1, BaseForm.BaseApplicationNo);

                            }
                            else
                            {
                                caseMstEntity = _model.CaseMstData.GetCaseMST(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, strYear1, string.Empty);
                                List<CaseMstEntity> caseMstList = new List<CaseMstEntity>();
                                caseMstList.Add(caseMstEntity);
                                BaseForm.BaseCaseMstListEntity = caseMstList;
                                if (caseMstEntity != null)
                                {

                                    BaseForm.BaseApplicationNo = caseMstEntity.ApplNo; ;
                                    caseSnpEntity = _model.CaseMstData.GetCaseSnpadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, strYear1, BaseForm.BaseApplicationNo);
                                }
                                else
                                {
                                    BaseForm.BaseApplicationNo = null;
                                }
                            }
                            BaseForm.GetApplicantDetails(caseMstEntity, caseSnpEntity, strAgencyName, strDeptName, strProgName, strYear.ToString(), string.Empty, "Display");
                            BaseForm.BaseTopApplSelect = "Y";
                            Refresh();
                            if (IsDeleteApplicant)
                            {
                                MainMenuControl mainMenuControl = BaseForm.GetBaseUserControlMainMenu() as MainMenuControl;
                                if (mainMenuControl != null)
                                {
                                    mainMenuControl.RefreshMainMenuClientIntake();
                                }
                            }
                            AlertBox.Show("Deleted Successfully");
                        }
                        else if (strMsg == "Dependency")
                        {
                            MessageBox.Show("You cant delete this member, as there are Dependents", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (strMsg == "CASEACT")
                        {
                            MessageBox.Show("'You can't delete this member, as there are CA Postings", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (strMsg == "CASEMS")
                        {
                            MessageBox.Show("'You can't delete this member, as there are MS Postings", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (strMsg == "CASESPM")
                        {
                            MessageBox.Show("'You can't delete this member, as there are Service Plan Master", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (strMsg == "CASECONT")
                        {
                            MessageBox.Show("'You can't delete this member, as there are Postings in CASECONT", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (strMsg == "LIHEAPV")
                        {
                            MessageBox.Show("'You can't delete this member, as there are vendors in the Supplier Screen", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (strMsg == "LIHEAPB")
                        {
                            MessageBox.Show("You cant delete this member, as there are records in Control Card - Benefit Maintenance Screen", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (strMsg == "CHLDMST")
                        {
                            MessageBox.Show("You can't delete this member, as there are records in Medical/Emergency Screen", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (strMsg == "CASEENRL")
                        {
                            MessageBox.Show("You cant delete this member, as there are records in Enrollment/Withdrawals Screen", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (strMsg == "MATASMT")
                        {
                            MessageBox.Show("You cant delete this member, as there are records in Matrix Assessment Screen", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (strMsg == "EMSRES")
                        {
                            MessageBox.Show("You cant delete this member, as there are records in Budget Resource Screen", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("You cant delete this member, as there are MS Postings and OBO", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("You cant delete this member, as there are CA Postings and OBO", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void fillClientIntake()
        {
            try
            {
                dtApplicationType.Value = DateTime.Now;
                dtApplicationType.Checked = false;

                if (BaseForm.BaseCaseMstListEntity != null)
                {
                    cmbAboutUs.SelectedIndex = 0;
                    cmbCaseType.SelectedIndex = 0;
                    cmbContact.SelectedIndex = 0;
                    cmbCounty.SelectedIndex = 0;
                    cmbDwelling.SelectedIndex = 0;
                    cmbFamilyType.SelectedIndex = 0;
                    cmbHousingSituation.SelectedIndex = 0;
                    //cmbHousingTx.SelectedIndex = 0;
                    cmbLCounty.SelectedIndex = 0;
                    cmbMailCounty.SelectedIndex = 0;
                    cmbOutService.SelectedIndex = 0;
                    cmbPMOPfHeat.SelectedIndex = 0;
                    cmbPrimaryLang.SelectedIndex = 0;
                    cmbPrimarySourceoHeat.SelectedIndex = 0;
                    cmbSecondLang.SelectedIndex = 0;
                    //Kranthi:: Added on 11/22/2022
                    if (BaseForm.BusinessModuleID == "08")
                    {
                        cmbEnergydata.SelectedIndex = 0;
                    }
                    cmbStaff.SelectedIndex = 0;
                    cmbSubsidized.SelectedIndex = 0;
                    cmbTownship.SelectedIndex = 0;
                    cmbTownship.SelectedIndex = 0;
                    cmbVerifiedStaff.SelectedIndex = 0;
                    cmbWaitingList.SelectedIndex = 0;


                    CaseMstEntity caseMSTEntity = BaseForm.BaseCaseMstListEntity[0];
                    if (caseMSTEntity != null)
                    {
                        // btnAlertCodes.Enabled = true;
                        btnIncomeDetails.Enabled = true;
                        btnIncVerfication.Enabled = true;
                        CaseMST = caseMSTEntity;
                        decimal totIncome = 0;
                        decimal programIncome = 0;
                        MainMenuAppNo = caseMSTEntity.ApplNo;
                        CaseSnpEntityProp = BaseForm.BaseCaseSnpEntity;
                        if (CaseSnpEntityProp != null)
                        {
                            List<CommonEntity> Relation = _model.lookupDataAccess.GetRelationship();
                            foreach (CaseSnpEntity snpEntity in CaseSnpEntityProp)
                            {
                                if (caseMSTEntity.FamilySeq.Equals(snpEntity.FamilySeq))
                                {
                                    ApplicantLastName = snpEntity.NameixLast;
                                    string ApplicantName = LookupDataAccess.GetMemberName(snpEntity.NameixFi, snpEntity.NameixMi, snpEntity.NameixLast, strNameFormat);//snpEntity.NameixFi.Trim() + " " + snpEntity.NameixLast.Trim();
                                    string DOB = string.Empty;
                                    string SSNum = LookupDataAccess.GetCardNo(snpEntity.Ssno, "1", ProgramDefinition.SSNReasonFlag.Trim(), snpEntity.SsnReason);
                                    if (!snpEntity.AltBdate.Equals(string.Empty))
                                    {
                                        DOB = CommonFunctions.ChangeDateFormat(snpEntity.AltBdate, Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                                    }
                                    // if (!snpEntity.Ssno.Equals(string.Empty))
                                    //     SSNum = "nn-nnnn-" + snpEntity.Ssno.Substring(5, 4);

                                    string memberCode = string.Empty;
                                    CommonEntity rel = Relation.Find(u => u.Code.Equals(snpEntity.MemberCode));
                                    if (rel != null) memberCode = rel.Desc;
                                    string cellTotIncome = !snpEntity.TotIncome.Equals(string.Empty) ? snpEntity.TotIncome : "0.0";
                                    string cellProgIncome = !snpEntity.ProgIncome.Equals(string.Empty) ? snpEntity.ProgIncome : "0.0";

                                    cellProgIncome = Decimal.Parse(cellProgIncome).ToString("N", new CultureInfo("en-US"));
                                    cellTotIncome = Decimal.Parse(cellTotIncome).ToString("N", new CultureInfo("en-US"));

                                    //Kranthi:: Adding ClientID column in Grid on 11/22/2022
                                    string strClientID = snpEntity.ClientId;

                                    int rowIndex = gvwCustomer.Rows.Add(ApplicantName, SSNum, DOB, cellTotIncome, cellProgIncome, memberCode, strClientID, string.Empty);
                                    gvwCustomer.Rows[rowIndex].Tag = snpEntity;
                                    if (snpEntity.Status.Trim() != "A")
                                        gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                    if (caseMSTEntity.FamilySeq.Equals(snpEntity.FamilySeq) && snpEntity.Status.Trim() == "A")
                                        gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                                    if (caseMSTEntity.FamilySeq.Equals(snpEntity.FamilySeq) && snpEntity.Status.Trim() != "A")
                                        gvwCustomer.Rows[rowIndex].Cells["AppName"].Style.ForeColor = Color.Blue;
                                    if (snpEntity.Exclude == "Y")
                                        gvwCustomer.Rows[rowIndex].Cells["Relation"].Style.ForeColor = Color.Red;

                                    if (caseMSTEntity.FamilySeq.Equals(snpEntity.FamilySeq) && caseMSTEntity.ActiveStatus.Trim() != "Y")
                                        gvwCustomer.Rows[rowIndex].Cells["AppName"].Style.ForeColor = Color.Red;
                                    if (!snpEntity.TotIncome.Equals(string.Empty))
                                        totIncome += decimal.Parse(snpEntity.TotIncome);
                                    if (!snpEntity.ProgIncome.Equals(string.Empty))
                                        programIncome += decimal.Parse(snpEntity.ProgIncome);
                                    //if (caseMSTEntity.FamilySeq.Equals(snpEntity.FamilySeq))
                                    //{
                                    //    gvwCustomer.Rows[rowIndex].Cells["cellEnd"].Value = "casemst";
                                    //}
                                    //else
                                    //{
                                    //    gvwCustomer.Rows[rowIndex].Cells["cellEnd"].Value = "casesnp";
                                    //}
                                    //if (snpEntity.FamilySeq.Equals("1"))
                                    setTooltip(rowIndex, snpEntity);


                                    /****************************************************************************/
                                    /***************************** EMPLOYEE DETAILS ****************************/
                                    /**************************************************************************/
                                    getEmployeeIntake(snpEntity);
                                    /**************************************************************************/
                                }
                            }


                            foreach (CaseSnpEntity snpEntity in CaseSnpEntityProp)
                            {
                                if (!caseMSTEntity.FamilySeq.Equals(snpEntity.FamilySeq))
                                {
                                    string ApplicantName = LookupDataAccess.GetMemberName(snpEntity.NameixFi, snpEntity.NameixMi, snpEntity.NameixLast, strNameFormat);//snpEntity.NameixFi.Trim() + " " + snpEntity.NameixLast.Trim();
                                    string DOB = string.Empty;
                                    string SSNum = LookupDataAccess.GetCardNo(snpEntity.Ssno, "1", ProgramDefinition.SSNReasonFlag.Trim(), snpEntity.SsnReason);

                                    if (!snpEntity.AltBdate.Equals(string.Empty))
                                    {
                                        DOB = CommonFunctions.ChangeDateFormat(snpEntity.AltBdate, Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                                    }
                                    //if (!snpEntity.Ssno.Equals(string.Empty))
                                    //    SSNum = "nn-nnnn-" + snpEntity.Ssno.Substring(5, 4);

                                    string memberCode = string.Empty;
                                    CommonEntity rel = Relation.Find(u => u.Code.Equals(snpEntity.MemberCode));
                                    if (rel != null) memberCode = rel.Desc;
                                    string cellTotIncome = !snpEntity.TotIncome.Equals(string.Empty) ? snpEntity.TotIncome : "0.0";
                                    string cellProgIncome = !snpEntity.ProgIncome.Equals(string.Empty) ? snpEntity.ProgIncome : "0.0";

                                    cellProgIncome = Decimal.Parse(cellProgIncome).ToString("N", new CultureInfo("en-US"));
                                    cellTotIncome = Decimal.Parse(cellTotIncome).ToString("N", new CultureInfo("en-US"));

                                    //Kranthi:: Adding ClientID column in Grid on 11/22/2022
                                    string strClientID = snpEntity.ClientId;

                                    int rowIndex = gvwCustomer.Rows.Add(ApplicantName, SSNum, DOB, cellTotIncome, cellProgIncome, memberCode, strClientID, string.Empty);
                                    gvwCustomer.Rows[rowIndex].Tag = snpEntity;
                                    if (snpEntity.Status.Trim() != "A")
                                        gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                    if (caseMSTEntity.FamilySeq.Equals(snpEntity.FamilySeq) && snpEntity.Status.Trim() == "A")
                                        gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                                    if (caseMSTEntity.FamilySeq.Equals(snpEntity.FamilySeq) && snpEntity.Status.Trim() != "A")
                                        gvwCustomer.Rows[rowIndex].Cells["AppName"].Style.ForeColor = Color.Blue;
                                    if (snpEntity.Exclude == "Y")
                                        gvwCustomer.Rows[rowIndex].Cells["Relation"].Style.ForeColor = Color.Red;

                                    if (caseMSTEntity.FamilySeq.Equals(snpEntity.FamilySeq) && caseMSTEntity.ActiveStatus.Trim() != "Y")
                                        gvwCustomer.Rows[rowIndex].Cells["AppName"].Style.ForeColor = Color.Red;

                                    if (!snpEntity.TotIncome.Equals(string.Empty))
                                        totIncome += decimal.Parse(snpEntity.TotIncome);
                                    if (!snpEntity.ProgIncome.Equals(string.Empty))
                                        programIncome += decimal.Parse(snpEntity.ProgIncome);
                                    //if (snpEntity.FamilySeq.Equals("1"))
                                    setTooltip(rowIndex, snpEntity);
                                }
                            }
                            txtTotalIncome.Text = caseMSTEntity.FamIncome.Equals("0") ? "0.0" : Decimal.Parse(caseMSTEntity.FamIncome).ToString("N", new CultureInfo("en-US"));//totIncome.ToString();
                            txtProgramIncome.Text = caseMSTEntity.ProgIncome.ToString().Equals("0") ? "0.0" : Decimal.Parse(caseMSTEntity.ProgIncome).ToString("N", new CultureInfo("en-US"));
                            txtInProg.Text = caseMSTEntity.NoInProg.ToString();
                            txtInHouse.Text = caseMSTEntity.NoInhh.ToString();
                            CaseSnpEntity casesnp = CaseSnpEntityProp.Find(u => u.FamilySeq == CaseMST.FamilySeq);

                            if (gvwCustomer.Rows.Count > 0)
                            {
                                gvwCustomer.CurrentCell = gvwCustomer.Rows[0].Cells[1];
                                gvwCustomer.Rows[0].Selected = true;
                            }

                            propPreassStatus = "A";
                            fillPreassCustomQuestions(casesnp, string.Empty);
                            bool boolinactiveQuestions = false;
                            foreach (DataGridViewRow gvrows in gvwPreassesData.Rows)
                            {
                                if ((gvrows.Cells["gvtPQuestions"].Tag == null ? string.Empty : gvrows.Cells["gvtPQuestions"].Tag.ToString()) == "Y")
                                {
                                    boolinactiveQuestions = true;
                                    break;
                                }
                            }
                            if (boolinactiveQuestions)
                            {

                                fillPreassCustomQuestions(casesnp, "I");
                                propPreassStatus = "I";
                            }
                            else
                            {

                                fillPreassCustomQuestions(casesnp, "A");
                            }

                        }
                        ClearControls();
                        // txtAlertCodes.Text = caseMSTEntity.AlertCodes;
                        txtHN.Text = caseMSTEntity.Hn.Trim();
                        txtDirection.Text = caseMSTEntity.Direction.Trim();
                        txtFamilyID.Text = caseMSTEntity.FamilyId.Trim();
                        txtSuffix.Text = caseMSTEntity.Suffix.Trim();
                        txtStreet.Text = caseMSTEntity.Street.Trim();
                        txtFloor.Text = caseMSTEntity.Flr;
                        if (caseMSTEntity.ProgIncome.ToString() != string.Empty)
                        {
                            double MonthlyIncome = double.Parse(CaseMST.ProgIncome);
                            MonthlyIncome = MonthlyIncome / 12;
                            MonthlyIncome = Math.Round(MonthlyIncome, 2);
                            txtMonthlyIncome.Text = MonthlyIncome.ToString();
                        }
                        else
                            txtMonthlyIncome.Text = "0";
                        txtPrecinct.Text = caseMSTEntity.Precinct;
                        txtApartment.Text = caseMSTEntity.Apt;
                        if (caseMSTEntity.ActiveStatus.Equals("Y")) cbActive.Checked = true; else cbActive.Checked = false;
                        if (caseMSTEntity.Juvenile.Equals("Y")) cbJuvenile.Checked = true; else cbJuvenile.Checked = false;
                        if (caseMSTEntity.Secret.Equals("Y")) cbSecret.Checked = true; else cbSecret.Checked = false;
                        if (caseMSTEntity.Senior.Equals("Y")) cbSenior.Checked = true; else cbSenior.Checked = false;
                        string strZip = "00000".Substring(0, 5 - caseMSTEntity.Zip.Length) + caseMSTEntity.Zip;
                        string strZipPlus = "0000".Substring(0, 4 - caseMSTEntity.Zipplus.Length) + caseMSTEntity.Zipplus;
                        txtZipCode.Text = strZip + "-" + strZipPlus;
                        txtCity.Text = caseMSTEntity.City.Trim();
                        txtState.Text = caseMSTEntity.State;
                        txtSite.Text = caseMSTEntity.Site.Trim();

                        SetComboBoxValue(cmbAboutUs, caseMSTEntity.AboutUs);
                        SetComboBoxValue(cmbCaseType, caseMSTEntity.CaseType);
                        SetComboBoxValue(cmbContact, caseMSTEntity.BestContact);
                        SetComboBoxValue(cmbCounty, caseMSTEntity.County);
                        SetComboBoxValue(cmbFamilyType, caseMSTEntity.FamilyType);
                        FamilyTypeWaringMsg(caseMSTEntity.FamilyType);
                        SetComboBoxValue(cmbHousingSituation, caseMSTEntity.Housing);
                        //SetComboBoxValue(cmbHousingTx, caseMSTEntity.Housing);
                        SetComboBoxValue(cmbPrimaryLang, caseMSTEntity.Language);
                        SetComboBoxValue(cmbSecondLang, caseMSTEntity.LanguageOt);
                        SetComboBoxValue(cmbStaff, caseMSTEntity.IntakeWorker);
                        SetComboBoxValue(cmbTownship, caseMSTEntity.TownShip);
                        SetComboBoxValue(cmbWaitingList, caseMSTEntity.WaitList);
                        if (propAgencyControlDetails.State.ToUpper() == "TX")
                        {
                            SetComboBoxValue(cmbOutService, caseMSTEntity.OutOfService);
                        }
                        SetComboBoxValue(cmbVerifiedStaff, caseMSTEntity.ExpCaseWorker);

                        //Kranthi:: Added on 11/22/2022
                        if (cmbEnergydata.Items.Count > 0)
                        {
                            SetComboBoxValue(cmbEnergydata, caseMSTEntity.ApplictionType);
                            //if (caseMSTEntity.ApplictionType != string.Empty)
                            //{
                            //    CaseMST.ApplictionTypeDesc = ((ListItem)cmbEnergydata.SelectedItem).Text.ToString();
                            //}

                            if (caseMSTEntity.ApplictionDate.Trim() != string.Empty)
                            {
                                dtApplicationType.Value = Convert.ToDateTime(caseMSTEntity.ApplictionDate);
                                dtApplicationType.Checked = true;
                            }
                        }

                        dtCompleted.Text = string.Empty;
                        if (!caseMSTEntity.CompleteDate.Equals(string.Empty))
                        {
                            dtCompleted.Text = caseMSTEntity.CompleteDate;
                        }

                        // string phone = ("0000000000" + caseMSTEntity.Phone);
                        txtHomePhone.Text = caseMSTEntity.Area + caseMSTEntity.Phone;
                        txtCell.Text = caseMSTEntity.CellPhone;
                        txtMessage.Text = caseMSTEntity.MessagePhone;
                        txtTTY.Text = caseMSTEntity.TtyNumber;
                        txtFax.Text = caseMSTEntity.FaxNumber;
                        txtEmail.Text = caseMSTEntity.Email.Trim();

                        if (!caseMSTEntity.InitialDate.Equals(string.Empty))
                            dtpInitialDate.Text = CommonFunctions.ChangeDateFormat(caseMSTEntity.InitialDate, Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                        if (!caseMSTEntity.IntakeDate.Equals(string.Empty))
                            dtpIntakeDate.Text = CommonFunctions.ChangeDateFormat(caseMSTEntity.IntakeDate, Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                        if (!caseMSTEntity.CaseReviewDate.Equals(string.Empty))
                        {
                            CaseReviewDate.Text = CommonFunctions.ChangeDateFormat(caseMSTEntity.CaseReviewDate, Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                        }
                        else
                            CaseReviewDate.Text = string.Empty;
                        txtRent2.Text = txtRent.Text = caseMSTEntity.ExpRent;
                        txtHeating.Text = caseMSTEntity.ExpHeat;
                        txtWater.Text = caseMSTEntity.ExpWater;
                        txtElectric.Text = caseMSTEntity.ExpElectric;
                        txtExpand1.Text = caseMSTEntity.Debtcc;
                        txtExpand2.Text = caseMSTEntity.DebtLoans;
                        txtExpand3.Text = caseMSTEntity.DebtMed;
                        txtExpand4.Text = caseMSTEntity.DebtOther;
                        txtTotalLiving.Text = caseMSTEntity.ExpLivexpense;
                        txtTotalHouseHold.Text = caseMSTEntity.ExpTotal;//ExpCaseWorker
                        txtMiscExpenses.Text = caseMSTEntity.ExpMisc;
                        txtMiscDebt.Text = caseMSTEntity.DebtMisc;
                        txtMiscAssets.Text = caseMSTEntity.AsetMisc;
                        txtPhysicalAssets.Text = caseMSTEntity.AsetPhy;
                        txtOtherAssets.Text = caseMSTEntity.AsetOth;
                        txtLiquidAssets.Text = caseMSTEntity.AsetLiq;
                        txtTotalAssets.Text = caseMSTEntity.AsetTotal;
                        txtDebtAssetRatio.Text = caseMSTEntity.AsetRatio;
                        txtTotalHHDebt.Text = caseMSTEntity.DebtTotal;
                        txtDebtIncomeRatio.Text = caseMSTEntity.DebIncmRation;
                        // txtTotalHouseholdIncome.Text = caseMSTEntity.; 

                        CalclationEmployee();
                        txtNoOfYears.Text = caseMSTEntity.AddressYears;

                        SetComboBoxValue(cmbDwelling, caseMSTEntity.Dwelling);
                        SetComboBoxValue(cmbPMOPfHeat, caseMSTEntity.HeatIncRent);
                        SetComboBoxValue(cmbPrimarySourceoHeat, caseMSTEntity.Source);
                        chkSubsidized.Checked = caseMSTEntity.SubShouse == "Y" ? true : false;
                        SetComboBoxValue(cmbSubsidized, caseMSTEntity.SubStype);


                        CaseDiffEntity caseDiffDetails = _model.CaseMstData.GetCaseDiffadpya(BaseForm.BaseAgency.ToString(), BaseForm.BaseDept.ToString(), BaseForm.BaseProg.ToString(), caseMSTEntity.ApplYr, caseMSTEntity.ApplNo, string.Empty);
                        if (caseDiffDetails != null)
                        {
                            tabPageService.Show();
                            tabPageService.Hidden = false;
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
                            txtMailZipCode.Text = "00000".Substring(0, 5 - caseDiffDetails.Zip.Length) + caseDiffDetails.Zip;
                            txtMailZipPlus.Text = "0000".Substring(0, 4 - caseDiffDetails.ZipPlus.Length) + caseDiffDetails.ZipPlus;
                            CommonFunctions.SetComboBoxValue(cmbMailCounty, caseDiffDetails.County);
                        }
                        else
                        {
                            tabPageService.Hidden = true;
                        }

                        CaseDiffEntity caseLandlordDetails = _model.CaseMstData.GetLandlordadpya(BaseForm.BaseAgency.ToString(), BaseForm.BaseDept.ToString(), BaseForm.BaseProg.ToString(), caseMSTEntity.ApplYr, caseMSTEntity.ApplNo, "Landlord");
                        if (caseLandlordDetails != null)
                        {
                            tabPageLandlord.Show();
                            tabPageLandlord.Hidden = false;
                            txtLCityName.Text = caseLandlordDetails.City;
                            txtLHno.Text = caseLandlordDetails.Hn;
                            txtLLast.Text = caseLandlordDetails.IncareLast;
                            txtLFirst.Text = caseLandlordDetails.IncareFirst;
                            txtLApt.Text = caseLandlordDetails.Apt;
                            txtLFlr.Text = caseLandlordDetails.Flr;
                            txtLPhone.Text = caseLandlordDetails.Phone;
                            txtLState.Text = caseLandlordDetails.State;
                            txtLStreet.Text = caseLandlordDetails.Street;
                            txtLSuffix.Text = caseLandlordDetails.Suffix;
                            txtLZip.Text = "00000".Substring(0, 5 - caseLandlordDetails.Zip.Length) + caseLandlordDetails.Zip;
                            txtLzipplus.Text = "0000".Substring(0, 4 - caseLandlordDetails.ZipPlus.Length) + caseLandlordDetails.ZipPlus;
                            CommonFunctions.SetComboBoxValue(cmbLCounty, caseLandlordDetails.County);
                        }
                        else
                        {
                            tabPageLandlord.Hidden = true;
                        }

                        //DataSet serviceDS = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE(string.Empty, caseMSTEntity.ApplAgency, caseMSTEntity.ApplDept, caseMSTEntity.ApplProgram, caseMSTEntity.ApplYr, caseMSTEntity.ApplNo);
                        //DataTable serviceDT = serviceDS.Tables[0];
                        //List<string> serviceList = new List<string>();
                        //gvwServices.Rows.Clear();
                        //foreach (DataRow dr in serviceDT.Rows)
                        //{
                        //    gvwServices.Rows.Add(false, dr["INQ_DESC"].ToString(), dr["INQ_CODE"].ToString());
                        //    // listBoxSelectionControl1.ListBoxSelected.Items.Add(new ListItem(dr["INQ_DESC"].ToString(), dr["INQ_CODE"].ToString()));
                        //    serviceList.Add(dr["INQ_CODE"].ToString());
                        //}

                        gvwServices.Rows.Clear();
                        DataSet serviceSaveDS = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE("SAVE", caseMSTEntity.ApplAgency, caseMSTEntity.ApplDept, caseMSTEntity.ApplProgram, caseMSTEntity.ApplYr, caseMSTEntity.ApplNo);
                        DataTable serviceSaveDT = serviceSaveDS.Tables[0];
                        List<string> serviceSaveList = new List<string>();
                        if (serviceSaveDT.Rows.Count > 0)
                        {
                            foreach (DataRow row in serviceSaveDT.Rows)
                            {
                                gvwServices.Rows.Add(true, row["INQ_DESC"].ToString(), row["INQ_CODE"].ToString());

                                //if (row.Cells["ServiceCode"].Value != null)
                                //{
                                //    for (int i = 0; i < serviceSaveDT.Rows.Count; i++)
                                //    {
                                //        if (Convert.ToString(row.Cells["ServiceCode"].Value.ToString().Trim()) == serviceSaveDT.Rows[i]["INQ_CODE"].ToString().Trim())
                                //        {
                                //            row.Cells["Servicechk"].Value = true; break;
                                //        }
                                //    }

                                //}
                            }
                        }

                        gvwServices.Update();
                        gvwServices.ResumeLayout();

                        //Pre asses form tab loading
                        if (preassesCntlEntity.Exists(u => u.Enab.Equals("Y")))
                        {




                        }
                        if (gvwCustomer.Rows.Count > 0)
                        {
                            gvwCustomer_SelectionChanged(gvwCustomer, new EventArgs());

                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        void getEmployeeIntake(CaseSnpEntity snpEntity)
        {



            SetComboBoxValue(cmbEpresenteEmploy, snpEntity.Employed);
            SetComboBoxValue(cmbEAnywork, snpEntity.WorkLimit);
            if (!snpEntity.LastWorkDate.Equals(string.Empty))
            {
                dtElastDateWorked.Text = snpEntity.LastWorkDate;
                dtElastDateWorked.Checked = true;
            }
            else
            {
                dtElastDateWorked.Value = DateTime.Now.Date;
                dtElastDateWorked.Checked = false;
            }
            txtEifyesexplain.Text = snpEntity.ExplainWorkLimit;
            txtEcurrentHave.Text = snpEntity.NumberOfcjobs;
            txtElastvisit.Text = snpEntity.NumberofLvjobs;
            txtEFullTime.Text = snpEntity.FullTimeHours;
            txtEPartTime.Text = snpEntity.PartTimeHours;
            SetComboBoxValue(cmbEseasonalEmployee, snpEntity.SeasonalEmploy);
            if (snpEntity.IstShift.Trim().Equals("Y"))
                chkE1st.Checked = true;
            if (snpEntity.IIndShift.Trim().Equals("Y"))
                chkE2nd.Checked = true;
            if (snpEntity.IIIrdShift.Trim().Equals("Y"))
                chkE3rd.Checked = true;
            if (snpEntity.RShift.Trim().Equals("Y"))
                chkErotaing.Checked = true;

            txtEEmployer.Text = snpEntity.EmployerName;
            txtEstreet.Text = snpEntity.EmployerStreet;
            txtEcityState.Text = snpEntity.EmployerCity;
            mskEPhone.Text = snpEntity.EmplPhone;
            txtEExt.Text = snpEntity.EmplExt;

            SetComboBoxValue(cmbEJobTitle, snpEntity.JobTitle);
            SetComboBoxValue(cmbEJobCategory, snpEntity.JobCategory);
            txtEhourlywage.Text = snpEntity.HourlyWage;
            SetComboBoxValue(cmbEpayFrequency, snpEntity.PayFrequency);

            if (!snpEntity.HireDate.Equals(string.Empty))
            {
                dtEHireDate.Text = snpEntity.HireDate;
                dtEHireDate.Checked = true;
            }
            else
            {
                dtEHireDate.Value = DateTime.Now.Date;
                dtEHireDate.Checked = false;
            }
            if (dtEHireDate.Checked == true)
            {
                CalculateLengthoftimeEmployed();
            }

        }


        private void CalculateLengthoftimeEmployed()
        {
            //int intpresentYear = DateTime.Now.Year;
            //int intpresentMonth = DateTime.Now.Month;
            //int intpresentDaY = DateTime.Now.Day;
            int intpresentdays = ((Convert.ToInt32(DateTime.Now.Year) * 365) + (Convert.ToInt32(DateTime.Now.Month) * 30) + Convert.ToInt32(DateTime.Now.Day));
            int inthierdays = ((Convert.ToInt32(dtEHireDate.Value.Year) * 365) + (Convert.ToInt32(dtEHireDate.Value.Month) * 30) + Convert.ToInt32(dtEHireDate.Value.Day));
            int intTotaldays = intpresentdays - inthierdays;
            txtELengthofTime.Text = string.Empty;
            if ((intTotaldays < 30) && (intTotaldays > 0))
            {
                txtELengthofTime.Text = intTotaldays + " days";
            }
            else if ((intTotaldays < 60) && (intTotaldays > 0))
            {
                txtELengthofTime.Text = "30 days";
            }
            else if ((intTotaldays < 90) && (intTotaldays > 0))
            {
                txtELengthofTime.Text = "60 days";
            }
            else if ((intTotaldays < 120) && (intTotaldays > 0))
            {
                txtELengthofTime.Text = "90 days";
            }
            else if ((intTotaldays < 365) && (intTotaldays > 0))
            {
                txtELengthofTime.Text = "120 days";
            }
            else if (intTotaldays > 364)
            {
                int intyear = intTotaldays / 365;
                if (intyear > 0 && intyear < 3)
                {
                    txtELengthofTime.Text = "1 Year";
                }
                else if ((!(intyear < 3)) && intyear < 5)
                {
                    txtELengthofTime.Text = "3 Years";
                }
                else if (!(intyear < 5))
                {
                    txtELengthofTime.Text = intyear + " Year";
                }
            }


        }
        public void GetSelectService()
        {

            //DataSet ds = Captain.DatabaseLayer.CaseMst.GetSelectServices();
            //DataTable dt = ds.Tables[0];

            //foreach (DataRow dr in dt.Rows)
            //{
            //    lstAvailable.Items.Add(new ListItem(dr["INQ_DESC"].ToString(), dr["INQ_CODE"].ToString()));

            //}
            //lstAvailable.SelectedIndex = 0;

        }

        private void fillOutofService()
        {

            cmbOutService.Items.Clear();
            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Add(new Captain.Common.Utilities.ListItem("", "0"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Out of Service Area", "O"));
            listItem.Add(new Captain.Common.Utilities.ListItem("In Service Area (in File)", "I"));
            listItem.Add(new Captain.Common.Utilities.ListItem("In Service Area (Not in File)", "X"));
            cmbOutService.Items.AddRange(listItem.ToArray());
            cmbOutService.SelectedIndex = 0;

        }

        private void fillWaitList()
        {
            //cmbWaitingList.Items.Clear();
            //List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            //listItem.Add(new Captain.Common.Utilities.ListItem("Select One", ""));
            //listItem.Add(new Captain.Common.Utilities.ListItem("None", "O"));
            //listItem.Add(new Captain.Common.Utilities.ListItem("Yes", "Y"));
            //listItem.Add(new Captain.Common.Utilities.ListItem("No", "N"));
            //cmbWaitingList.Items.AddRange(listItem.ToArray());
            //cmbWaitingList.SelectedIndex = 0;
            List<CommonEntity> commonwailtinglist = new List<CommonEntity>();

            if (propAgencyControlDetails != null)
            {
                if (propAgencyControlDetails.State.ToUpper() == "TX")
                {
                    commonwailtinglist = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0067", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); ////_model.lookupDataAccess.GetCaseType();
                }
                else
                {
                    commonwailtinglist = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0002", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); ////_model.lookupDataAccess.GetCaseType();
                }
            }
            else
            {
                commonwailtinglist = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0002", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);
            }
            // CaseType = filterByHIE(CaseType);
            cmbWaitingList.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            //cmbWaitingList.ColorMember = "FavoriteColor";
            cmbWaitingList.SelectedIndex = 0;
            foreach (CommonEntity wailtlist in commonwailtinglist)
            {
                Captain.Common.Utilities.ListItem li = new Captain.Common.Utilities.ListItem(wailtlist.Desc, wailtlist.Code);
                cmbWaitingList.Items.Add(li);
                //if (Mode.Equals(Consts.Common.Add) && wailtlist.Default.Equals("Y")) cmbCaseType.SelectedItem = li;
            }
        }

        private void fillDropDowns()
        {
            List<CommonEntity> CaseType = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.CASETYPES, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); //_model.lookupDataAccess.GetCaseType();
            foreach (CommonEntity casetype in CaseType)
            {
                cmbCaseType.Items.Add(new Captain.Common.Utilities.ListItem(casetype.Desc, casetype.Code));
            }
            cmbCaseType.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            cmbCaseType.SelectedIndex = 0;

            List<CommonEntity> Township = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.CITYTOWNTABLE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); // _model.lookupDataAccess.GetTownship();
            foreach (CommonEntity township in Township)
            {
                cmbTownship.Items.Add(new Captain.Common.Utilities.ListItem(township.Desc, township.Code));
            }
            cmbTownship.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            cmbTownship.SelectedIndex = 0;

            List<CommonEntity> Country = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.COUNTY, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); //_model.lookupDataAccess.GetCountry();
            foreach (CommonEntity country in Country)
            {
                cmbCounty.Items.Add(new Captain.Common.Utilities.ListItem(country.Desc, country.Code));
                cmbMailCounty.Items.Add(new Captain.Common.Utilities.ListItem(country.Desc, country.Code));
                cmbLCounty.Items.Add(new Captain.Common.Utilities.ListItem(country.Desc, country.Code));
            }
            cmbCounty.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            cmbCounty.SelectedIndex = 0;
            cmbMailCounty.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            cmbMailCounty.SelectedIndex = 0;
            cmbLCounty.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            cmbLCounty.SelectedIndex = 0;

            List<CommonEntity> Housing = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HOUSINGTYPES, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); //_model.lookupDataAccess.GetHousing();
            foreach (CommonEntity housing in Housing)
            {
                cmbHousingSituation.Items.Add(new Captain.Common.Utilities.ListItem(housing.Desc, housing.Code));
                //cmbHousingTx.Items.Add(new Captain.Common.Utilities.ListItem(housing.Desc, housing.Code));
            }
            cmbHousingSituation.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            cmbHousingSituation.SelectedIndex = 0;
            //cmbHousingTx.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            // cmbHousingTx.SelectedIndex = 0;

            List<CommonEntity> PrimaryLanguage = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.LANGUAGECODES, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); //_model.lookupDataAccess.GetPrimaryLanguage();
            foreach (CommonEntity primarylanguage in PrimaryLanguage)
            {
                cmbPrimaryLang.Items.Add(new Captain.Common.Utilities.ListItem(primarylanguage.Desc, primarylanguage.Code));
            }
            cmbPrimaryLang.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            cmbPrimaryLang.SelectedIndex = 0;

            List<CommonEntity> Secondarylanguage = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.LANGUAGECODES, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); //_model.lookupDataAccess.GetSecondaryLanguage();
            foreach (CommonEntity secondlanguage in Secondarylanguage)
            {
                cmbSecondLang.Items.Add(new Captain.Common.Utilities.ListItem(secondlanguage.Desc, secondlanguage.Code));
            }
            cmbSecondLang.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            cmbSecondLang.SelectedIndex = 0;


            //Kranthi:: Added on 11/22/2022
            if (BaseForm.BusinessModuleID == "08")
            {
                pnlApplicantType.Visible = true;
                cmbEnergydata.Visible = true;
                lblApplicationtype.Visible = true;
                dtApplicationType.Visible = true;
                cmbEnergydata.Items.Clear();
                List<CommonEntity> AgyTabs_List = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0080", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); //_model.AdhocData.Browse_AGYTABS(searchAgytabs);
                foreach (CommonEntity item in AgyTabs_List)
                {
                    cmbEnergydata.Items.Add(new Captain.Common.Utilities.ListItem(item.Desc, item.Code));
                }
                cmbEnergydata.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
                cmbEnergydata.SelectedIndex = 0;


            }

            List<CommonEntity> FamilyType = _model.lookupDataAccess.GetAgyFamilyTypes();

            FamilyType = filterByHIE(FamilyType);
            cmbFamilyType.Items.Clear();
            //cmbFamilyType.ColorMember = "FavoriteColor";

            foreach (CommonEntity familyType in FamilyType)
            {
                Utilities.ListItem li = new Utilities.ListItem(familyType.Desc, familyType.Code, familyType.Active, familyType.Default.ToString(), familyType.Extension.ToString(), familyType.AgyCode.ToString(), familyType.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbFamilyType.Items.Add(li);

            }
            cmbFamilyType.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            cmbFamilyType.SelectedIndex = 0;

            //List<CommonEntity> FamilyType = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.FAMILYTYPE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); // _model.lookupDataAccess.GetFamilyType();
            //foreach (CommonEntity familyType in FamilyType)
            //{
            //    cmbFamilyType.Items.Add(new Captain.Common.Utilities.ListItem(familyType.Desc, familyType.Code));
            //}
            //cmbFamilyType.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            //cmbFamilyType.SelectedIndex = 0;


            List<CommonEntity> contactyou = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.BESTWAYTOCONTACT, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); //_model.lookupDataAccess.Getcontactyou();
            foreach (CommonEntity Contactyou in contactyou)
            {
                cmbContact.Items.Add(new Captain.Common.Utilities.ListItem(Contactyou.Desc, Contactyou.Code));
            }
            cmbContact.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            cmbContact.SelectedIndex = 0;

            List<CommonEntity> AboutUs = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HOWDIDYOUHEARABOUTTHEPROGRAM, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); //_model.lookupDataAccess.GetaboutUs();
            foreach (CommonEntity aboutUs in AboutUs)
            {
                cmbAboutUs.Items.Add(new Captain.Common.Utilities.ListItem(aboutUs.Desc, aboutUs.Code));
            }
            cmbAboutUs.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "-1"));
            cmbAboutUs.SelectedIndex = 0;

            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            DataSet cwDataSet = Captain.DatabaseLayer.CaseMst.GetCaseWorker(strCwFormat, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            DataTable dt = cwDataSet.Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                    listItem.Add(new Captain.Common.Utilities.ListItem(dr["NAME"].ToString().Trim(), dr["PWH_CASEWORKER"].ToString().Trim()));
            }
            cmbStaff.Items.AddRange(listItem.ToArray());
            cmbStaff.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            cmbStaff.SelectedIndex = 0;
            cmbVerifiedStaff.Items.AddRange(listItem.ToArray());
            cmbVerifiedStaff.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            cmbVerifiedStaff.SelectedIndex = 0;


            //AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
            //searchAgytabs.Tabs_Type = "S0030";
            //AdhocData AgyTabs = new AdhocData();
            //List<AGYTABSEntity> SubsizedHouse = AgyTabs.Browse_AGYTABS(searchAgytabs);

            List<CommonEntity> SubsizedHouse = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0030", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); //_model.lookupDataAccess.GetAgyTabRecordsByCode(Consts.AgyTab.Subsidized_Housing_Type);
            //SubsizedHouse = filterByHIE(SubsizedHouse);
            cmbSubsidized.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            //cmbSubsidized.ColorMember = "FavoriteColor";
            cmbSubsidized.SelectedIndex = 0;
            foreach (CommonEntity SubsizedH in SubsizedHouse)
            {
                Captain.Common.Utilities.ListItem li = new Captain.Common.Utilities.ListItem(SubsizedH.Desc, SubsizedH.Code);
                cmbSubsidized.Items.Add(li);

            }

            List<CommonEntity> DwellingType = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DWELLINGTYPE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); //_model.lookupDataAccess.GetAgyTabRecordsByCode(Consts.AgyTab.DWELLINGTYPE);
            //DwellingType = filterByHIE(DwellingType);
            cmbDwelling.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            //cmbDwelling.ColorMember = "FavoriteColor";
            cmbDwelling.SelectedIndex = 0;
            foreach (CommonEntity Dwellingitems in DwellingType)
            {
                Captain.Common.Utilities.ListItem li = new Captain.Common.Utilities.ListItem(Dwellingitems.Desc, Dwellingitems.Code, Dwellingitems.Active, Dwellingitems.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbDwelling.Items.Add(li);
            }

            List<CommonEntity> PrimarySourceHeat = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HEATSOURCE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); //_model.lookupDataAccess.GetAgyTabRecordsByCode(Consts.AgyTab.HEATSOURCE);
            // PrimarySourceHeat = filterByHIE(PrimarySourceHeat);
            cmbPrimarySourceoHeat.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            //cmbPrimarySourceoHeat.ColorMember = "FavoriteColor";
            cmbPrimarySourceoHeat.SelectedIndex = 0;
            foreach (CommonEntity PrimarySourceHeatItems in PrimarySourceHeat)
            {
                Captain.Common.Utilities.ListItem li = new Captain.Common.Utilities.ListItem(PrimarySourceHeatItems.Desc, PrimarySourceHeatItems.Code, PrimarySourceHeatItems.Active, PrimarySourceHeatItems.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbPrimarySourceoHeat.Items.Add(li);
            }

            List<CommonEntity> PrimaryMethodofHeat = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.METHODOFPAYINGFORHEAT, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); //_model.lookupDataAccess.GetAgyTabRecordsByCode(Consts.AgyTab.METHODOFPAYINGFORHEAT);
            //PrimaryMethodofHeat = filterByHIE(PrimaryMethodofHeat);
            cmbPMOPfHeat.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            //cmbPMOPfHeat.ColorMember = "FavoriteColor";
            cmbPMOPfHeat.SelectedIndex = 0;
            foreach (CommonEntity PrimaryMethodofHeatItems in PrimaryMethodofHeat)
            {
                Captain.Common.Utilities.ListItem li = new Captain.Common.Utilities.ListItem(PrimaryMethodofHeatItems.Desc, PrimaryMethodofHeatItems.Code, PrimaryMethodofHeatItems.Active, PrimaryMethodofHeatItems.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbPMOPfHeat.Items.Add(li);

            }


            GetSelectService();
        }

        public string Mode { get; set; }
        private void fillEmployeeCombo()
        {
            chkE1st.Checked = false;
            chkE2nd.Checked = false;
            chkE3rd.Checked = false;
            chkErotaing.Checked = false;
            txtELengthofTime.Text = "";


            txtEcurrentHave.Validator = TextBoxValidation.IntegerValidator;
            txtElastvisit.Validator = TextBoxValidation.IntegerValidator;
            txtEFullTime.Validator = TextBoxValidation.IntegerValidator;
            txtEPartTime.Validator = TextBoxValidation.IntegerValidator;
            //** txtEhourlywage.Validator = TextBoxValidation.FloatValidator;
            cmbEseasonalEmployee.Items.Clear();
            cmbEpresenteEmploy.Items.Clear();
            cmbEpayFrequency.Items.Clear();
            cmbEAnywork.Items.Clear();

            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Add(new Captain.Common.Utilities.ListItem("None", ""));
            listItem.Add(new Captain.Common.Utilities.ListItem("Yes", "Y"));
            listItem.Add(new Captain.Common.Utilities.ListItem("No", "N"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Unavailable", "U"));
            cmbEseasonalEmployee.Items.AddRange(listItem.ToArray());
            cmbEseasonalEmployee.SelectedIndex = 0;
            cmbEAnywork.Items.AddRange(listItem.ToArray());
            cmbEAnywork.SelectedIndex = 0;

            Mode = "EDIT";

            List<CommonEntity> commonpresentemp = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0091", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetJobCategory();

            foreach (CommonEntity JobCategory in commonpresentemp)
            {
                Captain.Common.Utilities.ListItem li = new Captain.Common.Utilities.ListItem(JobCategory.Desc, JobCategory.Code);
                cmbEpresenteEmploy.Items.Add(li);
                if (JobCategory.Default.Equals("Y")) cmbEpresenteEmploy.SelectedItem = li;
            }



            List<Captain.Common.Utilities.ListItem> listFrequency = new List<Captain.Common.Utilities.ListItem>();
            listFrequency.Add(new Captain.Common.Utilities.ListItem("None", ""));
            listFrequency.Add(new Captain.Common.Utilities.ListItem("Paid Weekly", "PW1"));
            listFrequency.Add(new Captain.Common.Utilities.ListItem("Paid Every 2 Weeks", "PW2"));
            listFrequency.Add(new Captain.Common.Utilities.ListItem("Other", "OTH"));
            cmbEpayFrequency.Items.AddRange(listFrequency.ToArray());
            cmbEpayFrequency.SelectedIndex = 0;



            //List<CommonEntity> commonworkEntity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.WorkStatus, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetJobTitle();
            //commonworkEntity = filterByHIE(commonworkEntity);
            //cmbEpresenteEmploy.Items.Insert(0, new ListItem("Select One", "0"));
            //cmbEpresenteEmploy.SelectedIndex = 0;
            //foreach (CommonEntity JobTitle in commonworkEntity)
            //{
            //    ListItem li = new ListItem(JobTitle.Desc, JobTitle.Code, JobTitle.Active, JobTitle.Active.Equals("Y") ? Color.Green : Color.Red);
            //    cmbEpresenteEmploy.Items.Add(li);
            //    if (Mode.Equals(Consts.Common.Add) && JobTitle.Default.Equals("Y")) cmbEpresenteEmploy.SelectedItem = li;
            //}


            List<CommonEntity> commonEntity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.JOBTITLE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetJobTitle();
            commonEntity = filterByHIE(commonEntity);
            cmbEJobTitle.ColorMember = "FavoriteColor";
            cmbEJobTitle.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            cmbEJobTitle.SelectedIndex = 0;
            foreach (CommonEntity JobTitle in commonEntity)
            {
                Captain.Common.Utilities.ListItem li = new Captain.Common.Utilities.ListItem(JobTitle.Desc, JobTitle.Code, JobTitle.Active, JobTitle.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbEJobTitle.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && JobTitle.Default.Equals("Y")) cmbEJobTitle.SelectedItem = li;
            }


            List<CommonEntity> commonjobCategory = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.JOBCATEGORY, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetJobCategory();
            commonjobCategory = filterByHIE(commonjobCategory);
            cmbEJobCategory.ColorMember = "FavoriteColor";
            cmbEJobCategory.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            cmbEJobCategory.SelectedIndex = 0;
            foreach (CommonEntity JobCategory in commonjobCategory)
            {
                Captain.Common.Utilities.ListItem li = new Captain.Common.Utilities.ListItem(JobCategory.Desc, JobCategory.Code, JobCategory.Active, JobCategory.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbEJobCategory.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && JobCategory.Default.Equals("Y")) cmbEJobCategory.SelectedItem = li;
            }


        }

        private void gvtemp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void loadCustomQuestions()
        {
            pnlAppCQ.Controls.Clear(); pnlHHCQ.Controls.Clear();

            ProgramDefinitionEntity programEntity1 = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            //if (programEntity != null)
            //{
            //    ProgramDefinition = programEntity;
            //}

            CaseSnpEntity CaseSNP = GetSelectedRow();
            CustQuesMixedControl customFieldsIntakeControl = new CustQuesMixedControl(BaseForm, CaseSNP, "CASE2001", true, Mode, programEntity1);

            if (customFieldsIntakeControl.GridViewControl.Rows.Count > 0)
            {
                tabUserDetails.TabPages[1].Hidden = false;
                tabUserDetails.TabPages[1].Show();
                customFieldsIntakeControl.Dock = DockStyle.Fill;
                customFieldsIntakeControl.IsMax = false;
                customFieldsIntakeControl.IsApplicant = true;
                customFieldsIntakeControl.AccessLevel = "A";
                customFieldsIntakeControl.filterCustomQuestions("A");

                customFieldsIntakeControl.GridViewControl.Columns["imgsave"].Visible = false;
                customFieldsIntakeControl.GridViewControl.Columns["ResponceDelete"].Visible = false;


                customFieldsIntakeControl.GridViewControl.Enabled = true;
                customFieldsIntakeControl.GridViewControl.ReadOnly = true;

                foreach (Wisej.Web.DataGridViewRow row in customFieldsIntakeControl.GridViewControl.Rows)
                {
                    // Set Enabled property of the fourth column in the DGV.
                    ///row.Cells["Response"].ReadOnly = true;
                    if (row.Cells["Response"].CellRenderer == "ButtonCell")
                    {
                        DataGridViewButtonCell dgcombocell = ((DataGridViewButtonCell)row.Cells["Response"]);
                        dgcombocell.ReadOnly = true;
                        //dgcombocell.Dispose();
                        //dgcombocell.Style.CssStyle = "Opacity:0.7; cursor: not-allowed !important;";
                        //dgcombocell.Style.CssClass = "btngridDeactive";
                        dgcombocell.Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#F2F2F2");
                        dgcombocell.Style.ForeColor = System.Drawing.Color.Black;

                    }
                }
                customFieldsIntakeControl.GridViewControl.Refresh();
                customFieldsIntakeControl.GridViewControl.Update();

                pnlAppCQ.Controls.Add(customFieldsIntakeControl);
                customFieldsIntakeControl.GridViewControl.CellClick += new DataGridViewCellEventHandler(gvtemp_CellClick);






            }
            else
            {
                tabUserDetails.TabPages[1].Hidden = true;
            }


            /************************************** Household Members Questions ************************************************/

            List<CustomQuestionsEntity> ChkHHQues = _model.FieldControls.GetCustomQuestions("CASE2001", "*", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg,
                "Sequence", "All", "P");
            if (ChkHHQues.Count > 0)
            {
                ChkHHQues = ChkHHQues.FindAll(u => u.CUSTMEMACCESS == "H");
            }
            if (ChkHHQues.Count > 0)
            {
                pnlHHCQHeader.Visible = true;
                lblHHHeader.Visible = true;
                pnlHHCQ.Visible = true;
                lblHHHeader.Text = "Household Custom Questions";
                // label8.Text = "Applicant Custom Questions";

                CustQuesMixedControl customFieldsHHIntakeControl = new CustQuesMixedControl(BaseForm, CaseSNP, "CASE2001", false, Mode, programEntity1, "H");

                if (customFieldsHHIntakeControl.GridViewControl.Rows.Count > 0)
                {
                    tabUserDetails.TabPages[1].Hidden = false;
                    tabUserDetails.TabPages[1].Show();
                    customFieldsHHIntakeControl.Dock = DockStyle.Fill;
                    //customFieldsHHIntakeControl.MaxButtonControl.Click += new EventHandler(OnIntakeMaxMinClick);
                    customFieldsHHIntakeControl.IsMax = false;
                    customFieldsHHIntakeControl.IsApplicant = false;
                    customFieldsHHIntakeControl.AccessLevel = "*";
                    //customFieldsHHIntakeControl.filterCustomQuestions("*");
                    customFieldsHHIntakeControl.GridViewControl.Columns["imgsave"].Visible = false;
                    customFieldsHHIntakeControl.GridViewControl.Columns["ResponceDelete"].Visible = false;
                    customFieldsHHIntakeControl.GridViewControl.Enabled = true;
                    customFieldsHHIntakeControl.GridViewControl.ReadOnly = true;

                    foreach (Wisej.Web.DataGridViewRow row in customFieldsHHIntakeControl.GridViewControl.Rows)
                    {
                        // Set Enabled property of the fourth column in the DGV.
                        ///row.Cells["Response"].ReadOnly = true;
                        if (row.Cells["Response"].CellRenderer == "ButtonCell")
                        {
                            DataGridViewButtonCell dgcombocell = ((DataGridViewButtonCell)row.Cells["Response"]);
                            dgcombocell.ReadOnly = true;
                            //dgcombocell.Dispose();
                            //dgcombocell.Style.CssStyle = "Opacity:0.7; cursor: not-allowed !important;";
                            //dgcombocell.Style.CssClass = "btngridDeactive";
                            dgcombocell.Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#F2F2F2");
                            dgcombocell.Style.ForeColor = System.Drawing.Color.Black;

                        }
                    }

                    customFieldsHHIntakeControl.GridViewControl.Refresh();
                    //splitApplicant.Panel2.Controls.Add(customFieldsControl);
                    pnlHHCQ.Controls.Add(customFieldsHHIntakeControl);
                }
                else
                {
                    if (customFieldsHHIntakeControl.GridViewControl.Rows.Count == 0 && customFieldsIntakeControl.GridViewControl.Rows.Count == 0)
                        tabUserDetails.TabPages[1].Hidden = true;
                }


            }
            else
            {
                pnlHHCQHeader.Visible = false;
                pnlHHCQ.Visible = false;
                pnlAppCQ.Dock = DockStyle.Fill;
            }
            /**************************************************************************************/
            /**************************************************************************************/
        }
        private void SetAlertTypes(string AlertTypes)
        {
            List<string> alertTypeList = new List<string>();
            if (AlertTypes != null)
            {
                string[] imageTypes = AlertTypes.Split(' ');
                for (int i = 0; i < imageTypes.Length; i++)
                {
                    alertTypeList.Add(imageTypes.GetValue(i).ToString());
                }
            }
        }

        private void fillEMS()
        {
            cmbTownship.Items.Clear();
            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Add(new Captain.Common.Utilities.ListItem("Select One", ""));
            listItem.Add(new Captain.Common.Utilities.ListItem("Authorization (Level 0)", "A"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Supervisory (Level 1)", "S"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Departmental (Level 2)", "D"));
            cmbTownship.Items.AddRange(listItem.ToArray());
            cmbTownship.SelectedIndex = 0;
        }

        private void fillSecurity()
        {
            cmbCaseType.Items.Clear();
            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Add(new Captain.Common.Utilities.ListItem("Select One", ""));
            listItem.Add(new Captain.Common.Utilities.ListItem("Reception/Clerical", "R"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Case Manager", "C"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Program Administrator", "P"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Both PA and CM", "B"));
            cmbCaseType.Items.AddRange(listItem.ToArray());
            cmbCaseType.SelectedIndex = 0;
        }

        /// <summary>
        /// Get Selected Rows Tag Clas.
        /// </summary>
        /// <returns></returns>
        public CaseSnpEntity GetSelectedRow()
        {
            CaseSnpEntity caseSnpEntity = null;
            if (gvwCustomer != null)
            {
                foreach (DataGridViewRow dr in gvwCustomer.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        strIndex = dr.Index;
                        caseSnpEntity = dr.Tag as CaseSnpEntity;
                        break;
                    }
                }
            }
            return caseSnpEntity;
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
                foreach (Captain.Common.Utilities.ListItem li in comboBox.Items)
                {
                    if (li.Value.Equals(value) || li.Text.Equals(value))
                    {
                        comboBox.SelectedItem = li;
                        break;
                    }
                }
            }
        }

        private void setControlEnabled(bool flag)
        {
            txtHN.Enabled = flag;
            txtStreet.Enabled = flag;
        }

        private void OnSnpDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvwCustomer.Rows[e.RowIndex].Tag is CaseSnpEntity)
            {
                CaseSnpEntity caseSnpEntity = gvwCustomer.Rows[e.RowIndex].Tag as CaseSnpEntity;
                CaseMstEntity caseMST = CaseMST;
                bool isApplicant = false;
                if (caseMST.FamilySeq.Equals(caseSnpEntity.FamilySeq)) isApplicant = true;
                if (Privileges.Program.ToString().ToUpper() == "CASE2002")
                {
                    Case3001Form clientSNPEditForm = new Case3001Form(BaseForm, isApplicant, caseMST, caseSnpEntity, Consts.Common.View, Privileges, CaseSnpEntityProp, ApplicantLastName);
                    clientSNPEditForm.StartPosition = FormStartPosition.CenterScreen;
                    clientSNPEditForm.ShowDialog();
                }
                else
                {
                    if (BaseForm.BaseAgencyControlDetails.RomaSwitch.ToUpper() == "Y")
                    {
                        Case4001Form clientSNPEditForm = new Case4001Form(BaseForm, isApplicant, caseMST, caseSnpEntity, Consts.Common.View, Privileges, CaseSnpEntityProp, ApplicantLastName);
                        clientSNPEditForm.StartPosition = FormStartPosition.CenterScreen;
                        clientSNPEditForm.ShowDialog();
                    }
                    else
                    {
                        Case3001Form clientSNPEditForm = new Case3001Form(BaseForm, isApplicant, caseMST, caseSnpEntity, Consts.Common.View, Privileges, CaseSnpEntityProp, ApplicantLastName);
                        clientSNPEditForm.StartPosition = FormStartPosition.CenterScreen;
                        clientSNPEditForm.ShowDialog();
                    }
                }
            }
        }

        private void btnIncome_Click(object sender, EventArgs e)
        {
            if (BaseForm.ContentTabs.TabPages[0].Controls[0] is MainMenuControl)
            {
                MainMenuControl mainMenuControl = (BaseForm.ContentTabs.TabPages[0].Controls[0] as MainMenuControl);
                //CaseIncome caseIncomeForm = new CaseIncome("V", BaseForm, Privileges);
                //caseIncomeForm.FormClosed += new Form.FormClosedEventHandler(OnCaseIncomeFormClosed);
                //caseIncomeForm.ShowDialog();

                CaseIncomeForm2 objIncome = new CaseIncomeForm2(string.Empty, BaseForm, CaseIncomePrivileges, ProgramDefinition, ProgramDefinition.IncomeTypeOnly.ToString(), Privileges.Program);
                objIncome.FormClosed += new FormClosedEventHandler(OnCaseIncomeFormClosed);
                objIncome.StartPosition = FormStartPosition.CenterScreen;
                objIncome.ShowDialog();


            }
        }


        private List<CommonEntity> filterByHIE(List<CommonEntity> LookupValues)
        {
            string HIE = MainMenuAgency + MainMenuDept + MainMenuProgram;
            LookupValues = LookupValues.FindAll(u => u.ListHierarchy.Contains(HIE) || u.ListHierarchy.Contains(MainMenuAgency + MainMenuDept + "**") || u.ListHierarchy.Contains(MainMenuAgency + "****") || u.ListHierarchy.Contains("******"));
            return LookupValues;
        }



        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void btnIncVerfication_Click(object sender, EventArgs e)
        {
            // GetSelectedProgram();
            CaseIncomeVerification caseIncomeverfication = new CaseIncomeVerification(BaseForm, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, CaseVerPrivileges, "V");
            caseIncomeverfication.FormClosed += new FormClosedEventHandler(OnCaseIncomeVerFormClosed);
            caseIncomeverfication.StartPosition = FormStartPosition.CenterScreen;
            caseIncomeverfication.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCaseNotesFormClosed(object sender, FormClosedEventArgs e)
        {
            CaseNotes form = sender as CaseNotes;

            //if (form.DialogResult == DialogResult.OK)
            //{
            string strYear = "    ";
            if (!string.IsNullOrEmpty(BaseForm.BaseYear))
            {
                strYear = BaseForm.BaseYear;
            }
            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo);
            if (caseNotesEntity.Count > 0)
            {
                //ToolBarCaseNotes.ImageSource = Consts.Icons.ico_CaseNotes_View;
                ToolBarCaseNotes.ImageSource = "captain-casenotes";
            }
            else
            {
                // ToolBarCaseNotes.ImageSource = Consts.Icons.ico_CaseNotes_New;
                ToolBarCaseNotes.ImageSource = "captain-casenotesadd";
            }
            caseNotesEntity = caseNotesEntity;

            //}
        }

        private void OnCaseIncomeVerFormClosed(object sender, FormClosedEventArgs e)
        {
            ShowFollowIndicator();
        }

        private void OnCaseIncomeFormClosed(object sender, FormClosedEventArgs e)
        {
            Refresh();
        }

        private void contextMenu2_Popup(object sender, EventArgs e)
        {

            if (gvwCustomer.Rows.Count > 0)
            {
                if (gvwCustomer.Rows[0].Tag is CaseSnpEntity)
                {

                    if (BaseForm.BaseAgencyControlDetails.LnkAppSwitch.ToString().ToUpper() == "Y")
                    {
                        contextMenu2.MenuItems.Clear();
                        if (BaseForm.UserProfile.Security.Trim() == "B" || BaseForm.UserProfile.Security.Trim() == "P")
                        {
                            CaseSnpEntity drow = gvwCustomer.SelectedRows[0].Tag as CaseSnpEntity;
                            if (!(CaseMST.FamilySeq.Equals(drow.FamilySeq)))
                            {
                                contextMenu2.MenuItems.Clear();
                                MenuItem menuLst = new MenuItem();
                                menuLst.Text = "Link To Applicant";
                                contextMenu2.MenuItems.Add(menuLst);
                            }
                        }
                    }
                    else
                    {
                        CaseSnpEntity drow = gvwCustomer.SelectedRows[0].Tag as CaseSnpEntity;
                        if (!(CaseMST.FamilySeq.Equals(drow.FamilySeq)))
                        {
                            contextMenu2.MenuItems.Clear();
                            MenuItem menuLst = new MenuItem();
                            menuLst.Text = "Link To Applicant";
                            contextMenu2.MenuItems.Add(menuLst);
                        }
                        else
                            contextMenu2.MenuItems.Clear();
                    }

                }
            }
        }

        private void gvwCustomer_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            if (gvwCustomer.Rows.Count > 0)
            {
                CaseSnpEntity caseLinkApplicant = GetSelectedRow();
                if (caseLinkApplicant != null)
                {
                    if (ProgramDefinition.PRODUPSSN.Equals("Y"))
                    {
                        bool boolupdate = true;
                        List<CaseMstEntity> CaseMstAllList = _model.CaseMstData.GetCaseMstSSno(string.Empty, string.Empty, string.Empty, string.Empty);
                        CaseMstEntity caseMstAlllistEntity = CaseMstAllList.Find(u => u.ApplAgency.Equals(CaseMST.ApplAgency) && u.ApplDept.Equals(CaseMST.ApplDept) && u.ApplProgram.Equals(CaseMST.ApplProgram) && u.ApplYr.Trim().Equals(CaseMST.ApplYr.Trim()) && u.Ssn.Equals(caseLinkApplicant.Ssno));
                        if (caseMstAlllistEntity != null)
                        {
                            if (caseLinkApplicant.Ssno == "000000000")
                            {
                                boolupdate = true;
                            }
                            else
                            {
                                CommonFunctions.MessageBoxDisplay("Applicant already exists with this SSN in   Hierarchy: " + caseMstAlllistEntity.ApplAgency + caseMstAlllistEntity.ApplDept + caseMstAlllistEntity.ApplProgram + caseMstAlllistEntity.ApplYr + " with App No: " + caseMstAlllistEntity.ApplNo);
                                boolupdate = false;
                            }
                        }
                        if (boolupdate)
                        {
                            CaseMstEntity CaseMstEntity = new CaseMstEntity();

                            CaseMstEntity.ApplAgency = CaseMST.ApplAgency;
                            CaseMstEntity.ApplDept = CaseMST.ApplDept;
                            CaseMstEntity.ApplProgram = CaseMST.ApplProgram;
                            CaseMstEntity.ApplYr = CaseMST.ApplYr;
                            CaseMstEntity.ApplNo = CaseMST.ApplNo;
                            CaseMstEntity.ClientId = caseLinkApplicant.ClientId;
                            CaseMstEntity.FamilySeq = caseLinkApplicant.FamilySeq;
                            CaseMstEntity.Ssn = caseLinkApplicant.Ssno;
                            CaseMstEntity.LstcOperator4 = BaseForm.UserID;
                            CaseMstEntity.Mode = "LinkApplicant";
                            string strApplNo = string.Empty;
                            string strClientId = string.Empty;
                            string strFamilyId = string.Empty;
                            string strNewSSNNO = string.Empty;
                            string strErrorMsg = string.Empty;
                            if (_model.CaseMstData.InsertUpdateCaseMst(CaseMstEntity, out strApplNo, out strClientId, out strFamilyId, out strNewSSNNO, out strErrorMsg))
                            {
                                CaseMstEntity caseMstEntity = _model.CaseMstData.GetCaseMST(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);

                                if (caseMstEntity != null)
                                {
                                    List<CaseSnpEntity> caseSnpEntity = _model.CaseMstData.GetCaseSnpadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
                                    BaseForm.BaseTopApplSelect = "Y";
                                    BaseForm.GetApplicantDetails(caseMstEntity, caseSnpEntity, CaseMstEntity.ApplAgency, CaseMstEntity.ApplDept, CaseMstEntity.ApplProgram, CaseMstEntity.ApplYr, "LinkApplicant", "Display");
                                    Refresh();
                                }
                            }
                            else
                            {
                                CommonFunctions.MessageBoxDisplay(strErrorMsg);
                            }
                        }
                    }
                    else
                    {
                        CaseMstEntity CaseMstEntity = new CaseMstEntity();

                        CaseMstEntity.ApplAgency = CaseMST.ApplAgency;
                        CaseMstEntity.ApplDept = CaseMST.ApplDept;
                        CaseMstEntity.ApplProgram = CaseMST.ApplProgram;
                        CaseMstEntity.ApplYr = CaseMST.ApplYr;
                        CaseMstEntity.ApplNo = CaseMST.ApplNo;
                        CaseMstEntity.ClientId = caseLinkApplicant.ClientId;
                        CaseMstEntity.FamilySeq = caseLinkApplicant.FamilySeq;
                        CaseMstEntity.Ssn = caseLinkApplicant.Ssno;
                        CaseMstEntity.LstcOperator4 = BaseForm.UserID;
                        CaseMstEntity.Mode = "LinkApplicant";
                        string strApplNo = string.Empty;
                        string strClientId = string.Empty;
                        string strFamilyId = string.Empty;
                        string strSsnNo = string.Empty;
                        string strErrorMsg = string.Empty;
                        if (_model.CaseMstData.InsertUpdateCaseMst(CaseMstEntity, out strApplNo, out strClientId, out strFamilyId, out strSsnNo, out strErrorMsg))
                        {
                            CaseMstEntity caseMstEntity = _model.CaseMstData.GetCaseMST(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);

                            if (caseMstEntity != null)
                            {
                                List<CaseSnpEntity> caseSnpEntity = _model.CaseMstData.GetCaseSnpadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
                                BaseForm.BaseTopApplSelect = "Y";
                                BaseForm.GetApplicantDetails(caseMstEntity, caseSnpEntity, CaseMstEntity.ApplAgency, CaseMstEntity.ApplDept, CaseMstEntity.ApplProgram, CaseMstEntity.ApplYr, "LinkApplicant", "Display");
                                Refresh();
                            }
                        }
                        else
                        {
                            CommonFunctions.MessageBoxDisplay(strErrorMsg);
                        }

                    }


                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //EligibilityConditionForm obj = new EligibilityConditionForm(BaseForm);
            //obj.Show();

        }

        public void CalclationEmployee()
        {
            txtTotalHouseholdIncome.Text = "0";
            txtTotalHHDebt.Text = "0";
            txtTotalAssets.Text = "0";
            double rent = txtRent.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtRent.Text);
            double water = txtWater.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtWater.Text);
            double electric = txtElectric.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtElectric.Text);
            double heating = txtHeating.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtHeating.Text);
            double expand1 = txtExpand1.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtExpand1.Text);
            double expand2 = txtExpand2.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtExpand2.Text);
            double expand3 = txtExpand3.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtExpand3.Text);
            double expand4 = txtExpand4.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtExpand4.Text);
            double miscExpenses = txtMiscExpenses.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtMiscExpenses.Text);
            double miscDebt = txtMiscDebt.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtMiscDebt.Text);
            double miscAsset = txtMiscAssets.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtMiscAssets.Text);
            double physcialAsset = txtPhysicalAssets.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtPhysicalAssets.Text);
            double OtherAsset = txtOtherAssets.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtOtherAssets.Text);
            double LiquidAsset = txtLiquidAssets.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtLiquidAssets.Text);




            //txtTotalAssets.Text = caseMSTEntity.AsetTotal;
            //txtDebtAssetRatio.Text = caseMSTEntity.AsetRatio;           
            //txtDebtIncomeRatio.Text = string.Empty;
            //txtTotalHouseholdIncome.Text = string.Empty;

            double TotalDebt = expand1 + expand2 + expand3 + expand4 + miscDebt;
            txtTotalHHDebt.Text = TotalDebt.ToString();

            double TotalAssets = physcialAsset + OtherAsset + LiquidAsset;//miscAsset
            txtTotalAssets.Text = TotalAssets.ToString();

            double total = rent + water + electric + heating + miscExpenses;
            txtTotalHouseHold.Text = total.ToString();
            double MonthlyIncome = 0;
            double TotalAllIncome = 0;
            txtMonthlyIncome.Text = "0";
            txtTotalLiving.Text = "0";

            if (TotalAssets > 0)
            {
                txtDebtAssetRatio.Text = Math.Round((TotalDebt / TotalAssets), 2).ToString();
            }
            else
                txtDebtAssetRatio.Text = "0.00";

            if (CaseMST != null)
            {

                if (CaseMST.ProgIncome.ToString() != string.Empty)
                {
                    MonthlyIncome = double.Parse(CaseMST.ProgIncome);
                }


                if (MonthlyIncome > 0)
                {
                    TotalAllIncome = MonthlyIncome;
                    txtTotalHouseholdIncome.Text = MonthlyIncome.ToString();
                    txtDebtIncomeRatio.Text = Math.Round((TotalDebt / TotalAllIncome), 2).ToString();
                    MonthlyIncome = MonthlyIncome / 12;
                    MonthlyIncome = Math.Round(MonthlyIncome, 2);
                    txtMonthlyIncome.Text = MonthlyIncome.ToString();
                    if (!((Common.Utilities.ListItem)cmbVerifiedStaff.SelectedItem).Value.ToString().Equals("0"))
                    {
                        double totLiveExp = 0;
                        if (total > 0)
                        {
                            totLiveExp = ((total / MonthlyIncome) * 1000) + 0.5;
                            totLiveExp = Math.Round(totLiveExp);
                            totLiveExp = totLiveExp / 10;
                        }
                        txtTotalLiving.Text = totLiveExp.ToString();
                    }
                }
                else
                    txtDebtIncomeRatio.Text = "0.00";
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            //SampleTestForm sample = new SampleTestForm(BaseForm, Privileges);
            //sample.ShowDialog();



            //PrintRdlcForm objForm = new PrintRdlcForm(BaseForm, Privileges);
            //objForm.ShowDialog();
            //ChldMstEntity chldMst = _model.ChldMstData.GetChldMstDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);
            //GetRankCategoryDetails(BaseForm.BaseCaseMstListEntity[0], BaseForm.BaseCaseSnpEntity, chldMst);

        }
        #region RankCateogryPointsCalculation
        public CaseMstEntity propMstRank { get; set; }
        private void GetRankCategoryDetails(CaseMstEntity caseMst, List<CaseSnpEntity> caseSnp, ChldMstEntity chldMst)
        {
            List<RNKCRIT2Entity> RnkQuesFledsEntity = _model.SPAdminData.GetRanksCrit2Data("RANKQUES", string.Empty, string.Empty);
            List<RNKCRIT2Entity> RnkQuesFledsAllDataEntity = _model.SPAdminData.GetRanksCrit2Data(string.Empty, BaseForm.BaseAgency, string.Empty);
            List<RNKCRIT2Entity> RnkCustFldsAllDataEntity = _model.SPAdminData.GetRanksCrit2Data("CUSTFLDS", BaseForm.BaseAgency, string.Empty);
            List<CustomQuestionsEntity> custResponses = _model.CaseMstData.GetCustomQuestionAnswersRank(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty, string.Empty, string.Empty);
            List<CommonEntity> ListRankPoints = new List<CommonEntity>();
            for (int intRankCtg = 1; intRankCtg <= 6; intRankCtg++)
            {
                List<RNKCRIT2Entity> RnkQuesFledsDataEntity = RnkQuesFledsAllDataEntity.FindAll(u => u.RankCategory.Trim().ToString() == intRankCtg.ToString());
                List<RNKCRIT2Entity> RnkCustFldsDataEntity = RnkCustFldsAllDataEntity.FindAll(u => u.RankCategory.Trim().ToString() == intRankCtg.ToString());

                List<RNKCRIT2Entity> RnkQuesSearchList;
                propMstRank = caseMst;
                RNKCRIT2Entity RnkQuesSearch = null;
                // List<RNKCRIT2Entity> RnkQuesCaseSnp = null;
                int intRankPoint = 0;
                string strApplicationcode = string.Empty;
                foreach (RNKCRIT2Entity rnkQuesData in RnkQuesFledsEntity)
                {
                    RnkQuesSearch = null;
                    switch (rnkQuesData.RankFldName.Trim())
                    {
                        case Consts.RankQues.MZip:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Zip.Trim());
                            break;
                        case Consts.RankQues.MCounty:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.County.Trim());
                            break;
                        case Consts.RankQues.MLanguage:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Language.Trim());
                            break;
                        case Consts.RankQues.MAlertCode:
                            intRankPoint = intRankPoint + fillAlertIncomeCodes(caseMst.AlertCodes, RnkQuesFledsDataEntity, rnkQuesData.RankFldName.Trim());
                            break;
                        case Consts.RankQues.MAboutUs:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.AboutUs.Trim());
                            break;
                        case Consts.RankQues.MAddressYear:
                            if (caseMst.AddressYears != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.AddressYears) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.AddressYears));
                            break;
                        case Consts.RankQues.MBestContact:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.BestContact.Trim());
                            break;
                        case Consts.RankQues.MCaseReviewDate:
                            if (caseMst.CaseReviewDate != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.CaseReviewDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.CaseReviewDate).Date);
                            break;
                        case Consts.RankQues.MCaseType:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.CaseType.Trim());
                            break;
                        case Consts.RankQues.MCmi:
                            if (caseMst.Cmi != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Cmi) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Cmi));
                            break;
                        case Consts.RankQues.MEElectric:
                            if (caseMst.ExpElectric != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpElectric) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpElectric));
                            break;
                        case Consts.RankQues.MEDEBTCC:
                            if (caseMst.Debtcc != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Debtcc) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Debtcc));
                            break;
                        case Consts.RankQues.MEDEBTLoans:
                            if (caseMst.DebtLoans != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.DebtLoans) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.DebtLoans));
                            break;
                        case Consts.RankQues.MEDEBTMed:
                            if (caseMst.DebtMed != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.DebtMed) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.DebtMed));
                            break;
                        case Consts.RankQues.MEHeat:
                            if (caseMst.ExpHeat != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpHeat) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpHeat));
                            break;
                        case Consts.RankQues.MEligDate:
                            if (caseMst.EligDate != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.EligDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.EligDate).Date);
                            break;
                        case Consts.RankQues.MELiveExpenses:
                            if (caseMst.ExpLivexpense != string.Empty)
                            {
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpLivexpense) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpLivexpense));
                            }
                            // RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.ExpLivexpense.Trim());
                            break;
                        case Consts.RankQues.MERent:
                            if (caseMst.ExpRent != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpRent) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpRent));
                            break;
                        case Consts.RankQues.METotal:
                            if (caseMst.ExpTotal != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpTotal) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpTotal));
                            break;
                        case Consts.RankQues.MEWater:
                            if (caseMst.ExpWater != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpWater) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpWater));
                            break;

                        case Consts.RankQues.MExpCaseworker:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.ExpCaseWorker.Trim());
                            break;
                        case Consts.RankQues.MFamilyType:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.FamilyType.Trim());
                            break;
                        case Consts.RankQues.MFamIncome:
                            if (caseMst.FamIncome != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.FamIncome) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.FamIncome));
                            break;
                        case Consts.RankQues.MHousing:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Housing.Trim());
                            break;
                        case Consts.RankQues.MHud:
                            if (caseMst.Hud != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Hud) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Hud));
                            break;

                        case Consts.RankQues.MIncomeTypes:
                            intRankPoint = intRankPoint + fillAlertIncomeCodes(caseMst.AlertCodes, RnkQuesFledsDataEntity, rnkQuesData.RankFldName.Trim());
                            //RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.IncomeTypes.Trim());
                            break;
                        case Consts.RankQues.MInitialDate:
                            if (caseMst.InitialDate != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.InitialDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.InitialDate).Date);
                            break;
                        case Consts.RankQues.MIntakeDate:
                            if (caseMst.IntakeDate != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.IntakeDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.IntakeDate).Date);
                            break;
                        case Consts.RankQues.MIntakeWorker:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.IntakeWorker.Trim());
                            break;
                        case Consts.RankQues.MJuvenile:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Juvenile.Trim());
                            break;
                        case Consts.RankQues.MLanguageOt:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.LanguageOt.Trim());
                            break;
                        case Consts.RankQues.MNoInprog:
                            if (caseMst.NoInProg != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.NoInProg) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.NoInProg));
                            break;
                        case Consts.RankQues.Mpoverty:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Poverty.Trim());
                            break;
                        case Consts.RankQues.MProgIncome:
                            if (caseMst.ProgIncome != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ProgIncome) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ProgIncome));
                            break;
                        case Consts.RankQues.MReverifyDate:
                            if (caseMst.ReverifyDate != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(caseMst.ReverifyDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.ReverifyDate).Date);
                            break;
                        case Consts.RankQues.MSECRET:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Secret.Trim());
                            break;
                        case Consts.RankQues.MSenior:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Senior.Trim());
                            break;
                        case Consts.RankQues.MSite:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.Site.Trim());
                            break;
                        case Consts.RankQues.MSMi:
                            if (caseMst.Smi != string.Empty)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Smi) && Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Smi));
                            break;
                        case Consts.RankQues.MVefiryCheckstub:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyCheckStub.Trim());
                            break;
                        case Consts.RankQues.MVerifyW2:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyW2.Trim());
                            break;
                        case Consts.RankQues.MVeriTaxReturn:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyTaxReturn.Trim());
                            break;
                        case Consts.RankQues.MVerLetter:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyLetter.Trim());
                            break;
                        case Consts.RankQues.MVerOther:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.VerifyOther.Trim());
                            break;
                        case Consts.RankQues.MWaitList:
                            RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.WaitList.Trim());
                            break;
                        case Consts.RankQues.SEducation:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Education.ToString();
                            List<string> SnpFieldsCodesList = new List<string>();
                            List<string> SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Education);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.S1shift:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).IstShift.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].IstShift);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.S2ndshift:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).IIndShift.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].IIndShift);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.S3rdShift:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).IIIrdShift.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].IIIrdShift);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SAge:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Age.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Age);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        //case Consts.RankQues.SAltBdate:
                        //    RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                        //    strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).AltBdate.ToString();
                        //    SnpFieldsCodesList = new List<string>();
                        //    SnpFieldsRelationList = new List<string>();
                        //    for (int i = 0; i < caseSnp.Count; i++)
                        //    {
                        //        SnpFieldsCodesList.Add(caseSnp[i].AltBdate);
                        //        SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                        //    }
                        //    intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                        //    break;
                        case Consts.RankQues.SDisable:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Disable.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Disable);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SDrvlic:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Drvlic.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Drvlic);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SEmployed:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Employed.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Employed);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SEthinic:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Ethnic.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Ethnic);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        //case Consts.RankQues.SExpireWorkDate:
                        //    RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                        //    strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).ExpireWorkDate.ToString();
                        //    SnpFieldsCodesList = new List<string>();
                        //    SnpFieldsRelationList = new List<string>();
                        //    for (int i = 0; i < caseSnp.Count; i++)
                        //    {
                        //        SnpFieldsCodesList.Add(caseSnp[i].ExpireWorkDate);
                        //        SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                        //    }
                        //    intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                        //    break;
                        case Consts.RankQues.SFarmer:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Farmer.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Farmer);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SFoodStamps:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).FootStamps.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].FootStamps);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SFThours:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).FullTimeHours.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].FullTimeHours);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SHealthIns:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).HealthIns.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].HealthIns);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        //case Consts.RankQues.SHireDate:
                        //    RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                        //    strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).HireDate.ToString();
                        //    SnpFieldsCodesList = new List<string>();
                        //    SnpFieldsRelationList = new List<string>();
                        //    for (int i = 0; i < caseSnp.Count; i++)
                        //    {
                        //        SnpFieldsCodesList.Add(caseSnp[i].HireDate);
                        //        SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                        //    }
                        //    intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                        //    break;
                        case Consts.RankQues.SHourlyWage:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).HourlyWage.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Education);
                                SnpFieldsRelationList.Add(caseSnp[i].HourlyWage);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SjobCategory:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).JobCategory.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].JobCategory);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SjobTitle:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).JobTitle.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].JobTitle);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        //case Consts.RankQues.SLastWorkDate:
                        //    RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                        //    strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).LastWorkDate.ToString();
                        //    SnpFieldsCodesList = new List<string>();
                        //    SnpFieldsRelationList = new List<string>();
                        //    for (int i = 0; i < caseSnp.Count; i++)
                        //    {
                        //        SnpFieldsCodesList.Add(caseSnp[i].LastWorkDate);
                        //        SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                        //    }
                        //    intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                        //    break;
                        case Consts.RankQues.SLegalTowork:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).LegalTowork.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].LegalTowork);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SMartialStatus:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).MaritalStatus.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].MaritalStatus);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SMemberCode:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).MemberCode.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].MemberCode);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SNofcjob:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).NumberOfcjobs.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].NumberOfcjobs);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SNofljobs:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).NumberofLvjobs.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].NumberofLvjobs);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SPFrequency:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).PayFrequency.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].PayFrequency);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SPregnant:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Pregnant.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Pregnant);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SPThours:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).PartTimeHours.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].PartTimeHours);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SRace:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Race.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Race);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SRelitran:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Relitran.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Relitran);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SResident:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Resident.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Resident);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SRshift:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).RShift.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].RShift);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SSchoolDistrict:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).SchoolDistrict.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].SchoolDistrict);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SSEmploy:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Employed.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Employed);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SSex:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Sex.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Sex);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SSnpVet:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Vet.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Vet);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SStatus:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Status.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Status);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.STranserv:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Transerv.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Transerv);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SWic:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Wic.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].Wic);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.SworkLimit:
                            RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
                            strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).WorkLimit.ToString();
                            SnpFieldsCodesList = new List<string>();
                            SnpFieldsRelationList = new List<string>();
                            for (int i = 0; i < caseSnp.Count; i++)
                            {
                                SnpFieldsCodesList.Add(caseSnp[i].WorkLimit);
                                SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
                            }
                            intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
                            break;
                        case Consts.RankQues.CDentalCoverage:
                            if (chldMst != null)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == chldMst.DentalCoverage.Trim());
                            break;
                        case Consts.RankQues.CDiagNosisDate:
                            if (chldMst != null)
                                if (chldMst.DiagnosisDate != string.Empty)
                                    RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && Convert.ToDateTime(u.LtDate).Date >= Convert.ToDateTime(chldMst.DiagnosisDate).Date && Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(chldMst.DiagnosisDate).Date);
                            break;
                        case Consts.RankQues.CDisability:
                            if (chldMst != null)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == chldMst.Disability.Trim());
                            break;
                        case Consts.RankQues.CInsCat:
                            if (chldMst != null)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == chldMst.InsCat.Trim());
                            break;
                        case Consts.RankQues.CMedCoverage:
                            if (chldMst != null)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == chldMst.MedCoverage.Trim());
                            break;
                        case Consts.RankQues.CMedicalCoverageType:
                            if (chldMst != null)
                                RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == chldMst.MedCoverType.Trim());
                            break;


                    }

                    if (RnkQuesSearch != null)
                        intRankPoint = intRankPoint + Convert.ToInt32(RnkQuesSearch.Points);
                }
                CustomQuestionsEntity custResponcesearch = null;
                foreach (RNKCRIT2Entity item in RnkCustFldsDataEntity)
                {
                    custResponcesearch = null;
                    if (item.RankFldRespType.Trim().Equals("D"))
                    {
                        custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && u.ACTMULTRESP.Trim() == item.RespCd.Trim());
                    }
                    else if (item.RankFldRespType.Trim().Equals("N"))
                    {
                        custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && Convert.ToDecimal(u.ACTNUMRESP) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(u.ACTNUMRESP) <= Convert.ToDecimal(item.LtNum));
                    }
                    else if (item.RankFldRespType.Trim().Equals("T"))
                    {
                        // custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && Convert.ToDateTime(u.ACTDATERESP) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(u.ACTNUMRESP) <= Convert.ToDecimal(item.LtNum));           
                    }
                    if (custResponcesearch != null)
                        intRankPoint = intRankPoint + Convert.ToInt32(item.Points);
                }

                ListRankPoints.Add(new CommonEntity(intRankCtg.ToString(), intRankPoint.ToString()));
            }
            foreach (CommonEntity item in ListRankPoints)
            {
                txtProcess.Text = txtProcess.Text + item.Code + ":" + item.Desc + ",";
            }
        }

        private int fillAlertIncomeCodes(string alertCodes, List<RNKCRIT2Entity> rnkSearchEntity, string FieldName)
        {
            int intAlertcode = 0;
            List<string> AlertList = new List<string>();
            if (alertCodes != null)
            {
                string[] incomeTypes = alertCodes.Split(' ');
                for (int i = 0; i < incomeTypes.Length; i++)
                {
                    AlertList.Add(incomeTypes.GetValue(i).ToString());
                }
            }
            List<RNKCRIT2Entity> RnkAlertCode = rnkSearchEntity.FindAll(u => u.RankFldName.Trim() == FieldName);

            foreach (RNKCRIT2Entity rnkEntity in RnkAlertCode)
            {
                if (alertCodes != null && AlertList.Contains(rnkEntity.RespCd))
                {
                    intAlertcode = intAlertcode + Convert.ToInt32(rnkEntity.Points);
                }
            }
            return intAlertcode;
        }


        private int CaseSnpDetailsCalc(List<RNKCRIT2Entity> rnkCaseSnp, List<CaseSnpEntity> caseSnpDetails, string strApplicantCode, List<string> listCodestring, List<string> listRelationstring, string FilterCode, string ResponceType)
        {
            int intSnpPoints = 0;
            foreach (RNKCRIT2Entity item in rnkCaseSnp)
            {
                if (item.CountInd.Trim() == "A")
                {
                    if (item.RespCd == strApplicantCode)
                    {
                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                    }

                }
                else if (item.CountInd.Trim() == "M")
                {
                    if (item.Relation == "*")
                    {
                        int count = 0;
                        switch (FilterCode)
                        {
                            case Consts.RankQues.S1shift:
                                count = caseSnpDetails.FindAll(u => u.IstShift.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.S2ndshift:
                                count = caseSnpDetails.FindAll(u => u.IIndShift.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.S3rdShift:
                                count = caseSnpDetails.FindAll(u => u.IIIrdShift.Trim().Equals(item.RespCd)).Count;
                                break;
                            //case Consts.RankQues.SAge:
                            //    foreach (CaseSnpEntity snpDate in caseSnpDetails)
                            //    {
                            //        if (snpDate.AltBdate != string.Empty)
                            //        {
                            //            DateTime EndDate = GetEndDateAgeCalculation(item.AgeClcInd.Trim(), propMstRank);
                            //            int AgeMonth = _model.lookupDataAccess.GetAgeCalculationMonths(Convert.ToDateTime(snpDate.AltBdate), EndDate);
                            //            if (AgeMonth >= Convert.ToDecimal(item.GtNum) && AgeMonth <= Convert.ToDecimal(item.LtNum))
                            //            {
                            //                count = count + 1;
                            //            }
                            //        }
                            //    }
                            //    break;
                            //case Consts.RankQues.SAltBdate:
                            //    foreach (CaseSnpEntity snpDate in caseSnpDetails)
                            //    {
                            //        if (snpDate.AltBdate != string.Empty)
                            //            if (Convert.ToDateTime(snpDate.AltBdate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.AltBdate).Date <= Convert.ToDateTime(item.LtDate).Date)
                            //            {
                            //                count = count + 1;
                            //            }
                            //    }

                            //    break;
                            case Consts.RankQues.SSchoolDistrict:
                                count = caseSnpDetails.FindAll(u => u.SchoolDistrict.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SEducation:
                                count = caseSnpDetails.FindAll(u => u.Education.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SWic:
                                count = caseSnpDetails.FindAll(u => u.Wic.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SDisable:
                                count = caseSnpDetails.FindAll(u => u.Disable.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SDrvlic:
                                count = caseSnpDetails.FindAll(u => u.Drvlic.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SEmployed:
                                count = caseSnpDetails.FindAll(u => u.Employed.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SEthinic:
                                count = caseSnpDetails.FindAll(u => u.Ethnic.Trim().Equals(item.RespCd)).Count;
                                break;
                            //case Consts.RankQues.SExpireWorkDate:
                            //    foreach (CaseSnpEntity snpDate in caseSnpDetails)
                            //    {
                            //        if (snpDate.ExpireWorkDate != string.Empty)
                            //            if (Convert.ToDateTime(snpDate.ExpireWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.ExpireWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                            //            {
                            //                count = count + 1;
                            //            }
                            //    }
                            //    break;
                            case Consts.RankQues.SFarmer:
                                count = caseSnpDetails.FindAll(u => u.Farmer.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SFoodStamps:
                                count = caseSnpDetails.FindAll(u => u.FoodStampsDesc.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SFThours:
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {
                                    if (snpNumeric.FullTimeHours != string.Empty)
                                        if (Convert.ToDecimal(snpNumeric.FullTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.FullTimeHours) <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SHealthIns:
                                count = caseSnpDetails.FindAll(u => u.HealthIns.Trim().Equals(item.RespCd)).Count;
                                break;
                            //case Consts.RankQues.SHireDate:
                            //    foreach (CaseSnpEntity snpDate in caseSnpDetails)
                            //    {
                            //        if (snpDate.HireDate != string.Empty)
                            //            if (Convert.ToDateTime(snpDate.HireDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.HireDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                            //            {
                            //                count = count + 1;
                            //            }
                            //    }
                            //    break;
                            case Consts.RankQues.SHourlyWage:
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {
                                    if (snpNumeric.HourlyWage != string.Empty)
                                        if (Convert.ToDecimal(snpNumeric.HourlyWage) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.HourlyWage) <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SjobCategory:
                                count = caseSnpDetails.FindAll(u => u.JobCategory.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SjobTitle:
                                count = caseSnpDetails.FindAll(u => u.JobTitle.Trim().Equals(item.RespCd)).Count;
                                break;
                            //case Consts.RankQues.SLastWorkDate:
                            //    foreach (CaseSnpEntity snpDate in caseSnpDetails)
                            //    {
                            //        if (snpDate.LastWorkDate != string.Empty)
                            //            if (Convert.ToDateTime(snpDate.LastWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.LastWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                            //            {
                            //                count = count + 1;
                            //            }
                            //    }
                            //    break;
                            case Consts.RankQues.SLegalTowork:
                                count = caseSnpDetails.FindAll(u => u.LegalTowork.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SMartialStatus:
                                count = caseSnpDetails.FindAll(u => u.MaritalStatus.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SMemberCode:
                                count = caseSnpDetails.FindAll(u => u.MemberCode.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SNofcjob:
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {
                                    if (snpNumeric.NumberOfcjobs != string.Empty)
                                        if (Convert.ToDecimal(snpNumeric.NumberOfcjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberOfcjobs) <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SNofljobs:
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {
                                    if (snpNumeric.NumberofLvjobs != string.Empty)
                                        if (Convert.ToDecimal(snpNumeric.NumberofLvjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberofLvjobs) <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SPFrequency:
                                count = caseSnpDetails.FindAll(u => u.PayFrequency.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SPregnant:
                                count = caseSnpDetails.FindAll(u => u.Pregnant.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SPThours:
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {
                                    if (snpNumeric.PartTimeHours != string.Empty)
                                        if (Convert.ToDecimal(snpNumeric.PartTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.PartTimeHours) <= Convert.ToDecimal(item.LtNum))
                                        {
                                            count = count + 1;
                                        }
                                }
                                break;
                            case Consts.RankQues.SRace:
                                count = caseSnpDetails.FindAll(u => u.Race.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SRelitran:
                                count = caseSnpDetails.FindAll(u => u.Relitran.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SResident:
                                count = caseSnpDetails.FindAll(u => u.Resident.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SRshift:
                                count = caseSnpDetails.FindAll(u => u.RShift.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SSEmploy:
                                count = caseSnpDetails.FindAll(u => u.SeasonalEmploy.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SSex:
                                count = caseSnpDetails.FindAll(u => u.Sex.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SSnpVet:
                                count = caseSnpDetails.FindAll(u => u.Vet.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SStatus:
                                count = caseSnpDetails.FindAll(u => u.Status.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.STranserv:
                                count = caseSnpDetails.FindAll(u => u.Transerv.Trim().Equals(item.RespCd)).Count;
                                break;
                            case Consts.RankQues.SworkLimit:
                                count = caseSnpDetails.FindAll(u => u.WorkLimit.Trim().Equals(item.RespCd)).Count;
                                break;

                        }

                        if (caseSnpDetails.Count == count)
                            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                    }
                    else
                    {
                        switch (ResponceType)
                        {
                            case "D":
                                if (listRelationstring.Contains(item.Relation))
                                {
                                    if (listCodestring.Contains(item.RespCd))
                                    {
                                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                    }
                                }
                                break;
                            case "N":
                                foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                {

                                    switch (FilterCode)
                                    {
                                        //case Consts.RankQues.SAge:
                                        //    if (snpNumeric.AltBdate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                        //    {
                                        //        DateTime EndDate = GetEndDateAgeCalculation(item.AgeClcInd.Trim(), propMstRank);
                                        //        int AgeMonth = _model.lookupDataAccess.GetAgeCalculationMonths(Convert.ToDateTime(snpNumeric.AltBdate), EndDate);
                                        //        if (AgeMonth >= Convert.ToDecimal(item.GtNum) && AgeMonth <= Convert.ToDecimal(item.LtNum))
                                        //        {
                                        //            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                        //        }
                                        //    }
                                        //    break;

                                        case Consts.RankQues.SNofcjob:
                                            if (snpNumeric.NumberOfcjobs != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDecimal(snpNumeric.NumberOfcjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberOfcjobs) <= Convert.ToDecimal(item.LtNum))
                                                {
                                                    intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SNofljobs:
                                            if (snpNumeric.NumberofLvjobs != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDecimal(snpNumeric.NumberofLvjobs) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.NumberofLvjobs) <= Convert.ToDecimal(item.LtNum))
                                                {
                                                    intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SFThours:
                                            if (snpNumeric.FullTimeHours != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDecimal(snpNumeric.FullTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.FullTimeHours) <= Convert.ToDecimal(item.LtNum))
                                                {
                                                    intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SPThours:
                                            if (snpNumeric.PartTimeHours != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDecimal(snpNumeric.PartTimeHours) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.PartTimeHours) <= Convert.ToDecimal(item.LtNum))
                                                {
                                                    intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;
                                        case Consts.RankQues.SHourlyWage:
                                            if (snpNumeric.HourlyWage != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                                if (Convert.ToDecimal(snpNumeric.HourlyWage) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(snpNumeric.HourlyWage) <= Convert.ToDecimal(item.LtNum))
                                                {
                                                    intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                                }
                                            break;

                                    }
                                }
                                break;
                                //case "B":
                                //case "T":
                                //    foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
                                //    {

                                //        switch (FilterCode)
                                //        {
                                //            case Consts.RankQues.SAltBdate:
                                //                if (snpNumeric.AltBdate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                //                    if (Convert.ToDateTime(snpNumeric.AltBdate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.AltBdate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                //                    {
                                //                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                //                    }
                                //                break;
                                //            case Consts.RankQues.SExpireWorkDate:
                                //                if (snpNumeric.ExpireWorkDate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                //                    if (Convert.ToDateTime(snpNumeric.ExpireWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.ExpireWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                //                    {
                                //                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                //                    }
                                //                break;
                                //            case Consts.RankQues.SLastWorkDate:
                                //                if (snpNumeric.AltBdate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                //                    if (Convert.ToDateTime(snpNumeric.LastWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.LastWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                //                    {
                                //                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                //                    }
                                //                break;
                                //            case Consts.RankQues.SHireDate:
                                //                if (snpNumeric.HireDate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
                                //                    if (Convert.ToDateTime(snpNumeric.HireDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.HireDate).Date <= Convert.ToDateTime(item.LtDate).Date)
                                //                    {
                                //                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
                                //                    }
                                //                break;


                                //        }
                                //    }
                                //    break;

                        }


                    }

                }

            }
            return intSnpPoints;
        }

        public DateTime GetEndDateAgeCalculation(string Type, CaseMstEntity caseMst)
        {
            DateTime EndDate = DateTime.Now.Date;
            if (Type == "T")
            {
                EndDate = DateTime.Now.Date;
            }
            else if (Type == "I")
            {
                EndDate = Convert.ToDateTime(caseMst.IntakeDate);
            }
            else if (Type == "K")
            {
                string strDate = DateTime.Now.Date.ToShortDateString();
                string strYear;
                ZipCodeEntity zipentity = propzipCodeEntity.Find(u => u.Zcrzip.Trim().Equals(caseMst.Zip.Trim()));
                if (zipentity != null)
                {
                    if (zipentity.Zcrhssyear.Trim() == "2")
                    {
                        strYear = DateTime.Now.AddYears(1).Year.ToString();
                    }
                    else
                    {
                        strYear = DateTime.Now.Year.ToString();
                    }
                    strDate = zipentity.Zcrhssmo + "/" + zipentity.Zcrhssday + "/" + strYear;
                }
                EndDate = Convert.ToDateTime(strDate);
            }
            return EndDate;
        }


        #endregion



        private void button1_Click_1(object sender, EventArgs e)
        {
            //HSS20001ClientSupplierForm objClientFrom = new HSS20001ClientSupplierForm(BaseForm, TmsSupplierPrivileges);
            //objClientFrom.FormClosed += new Wisej.Web.Form.FormClosedEventHandler(objClientFrom_FormClosed);
            //objClientFrom.ShowDialog();

            HSS20001ClientSupplierForm objClientFrom = new HSS20001ClientSupplierForm(BaseForm, TmsSupplierPrivileges);
            objClientFrom.FormClosed += new FormClosedEventHandler(objClientFrom_FormClosed);
            objClientFrom.StartPosition = FormStartPosition.CenterScreen;
            objClientFrom.ShowDialog();
        }

        void objClientFrom_FormClosed(object sender, FormClosedEventArgs e)
        {
            PrimarySourceSetting();
        }

        private void DisplayIncomeMsgs()
        {
            lblFraudalert.Visible = false;
            if (BaseForm.BaseCaseMstListEntity != null)
            {
                CaseMST = BaseForm.BaseCaseMstListEntity[0];//_model.CaseMstData.GetCaseMST(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
                if (CaseMST != null)
                {
                    lblIncomeVerified.Text = "";
                    if (ProgramDefinition.IncomeVerMsg.Equals("Y"))
                    {

                        if (string.IsNullOrEmpty(CaseMST.EligDate))
                        {
                            lblIncomeVerified.Text = "Income Not Verified";
                        }
                        else
                        {
                            List<CaseVerEntity> caseVerList = _model.CaseMstData.GetCASEVeradpyalst(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty, string.Empty);
                            if (caseVerList.Count > 0)
                            {

                                if (!(Convert.ToDecimal(CaseMST.ProgIncome == string.Empty ? "0" : CaseMST.ProgIncome) == Convert.ToDecimal(caseVerList[0].IncomeAmount == string.Empty ? "0" : caseVerList[0].IncomeAmount)))
                                {
                                    lblIncomeVerified.Text = "Household income needs to be reverified as the income was changed";
                                }
                            }
                            else
                            {
                                lblIncomeVerified.Text = "Income Not Verified";
                            }
                        }
                    }
                    else
                        lblIncomeVerified.Text = "";

                    if (Privileges != null)
                    {
                        if (Privileges.ModuleCode == "08")
                        {
                            if (CaseMST.CbFraud == "1" && CaseMST.FraudDate != string.Empty)
                            {
                                lblFraudalert.Visible = true;
                            }
                        }
                    }

                }
            }
        }

        private void PrimarySourceSetting()
        {
            if (BaseForm.BaseCaseMstListEntity != null)
                CommonFunctions.SetComboBoxValue(cmbPrimarySourceoHeat, BaseForm.BaseCaseMstListEntity[0].Source);
        }



        #region PreassesDatafilling

        private void fillPreassCustomQuestions(CaseSnpEntity casesnpdata, string strType)
        {



            List<CustomQuestionsEntity> custQuestions = proppreassesQuestions;
            if (strType == "A")
                custQuestions = custQuestions.FindAll(u => u.CUSTACTIVECUST.ToUpper() == "A");
            if (strType == "I")
                custQuestions = custQuestions.FindAll(u => u.CUSTACTIVECUST.ToUpper() == "I");



            List<CustomQuestionsEntity> custResponses = _model.CaseMstData.GetPreassesQuestionAnswers(casesnpdata, "PRESRESP");
            bool isResponse = false;

            gvwPreassesData.Rows.Clear();
            if (custQuestions.Count > 0)
            {


                //foreach (PreassessQuesEntity preassesdata in preassessMasterEntity)
                //{

                foreach (CommonEntity preassesdata in preassessMasterEntity)
                {
                    List<PreassessQuesEntity> preassessChildList = preassessChildEntity.FindAll(u => u.PRECHILD_DID == preassesdata.Code);


                    bool boolQuestions = false;
                    foreach (PreassessQuesEntity preasschilddata in preassessChildList)
                    {
                        CustomQuestionsEntity dr = custQuestions.Find(u => u.CUSTCODE == preasschilddata.PRECHILD_QID);
                        if (dr != null)
                        {
                            boolQuestions = true;
                        }
                    }
                    if (boolQuestions)
                    {
                        int rowIndex = gvwPreassesData.Rows.Add(preassesdata.Desc, string.Empty, string.Empty);
                        gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                        preassessChildList = preassessChildList.OrderBy(u => Convert.ToInt32(u.PRECHILD_SEQ)).ToList();
                        foreach (PreassessQuesEntity preasschilddata in preassessChildList)
                        {
                            CustomQuestionsEntity dr = custQuestions.Find(u => u.CUSTCODE == preasschilddata.PRECHILD_QID);
                            if (dr != null)
                            {
                                string custCode = dr.CUSTCODE.ToString();
                                List<CustomQuestionsEntity> response = custResponses.FindAll(u => u.ACTCODE.Equals(custCode)).ToList();


                                rowIndex = gvwPreassesData.Rows.Add(dr.CUSTDESC, string.Empty, string.Empty);
                                gvwPreassesData.Rows[rowIndex].Cells["gvtPQuestions"].Tag = "N";

                                gvwPreassesData.Rows[rowIndex].Tag = dr;

                                string fieldType = dr.CUSTRESPTYPE.ToString();

                                string custQuestionResp = string.Empty;
                                string custQuestionCode = string.Empty;
                                if (true)   //!Mode.Equals(Consts.Common.Add))
                                {

                                    if (response != null && response.Count > 0)
                                    {
                                        if (!dr.CUSTACTIVECUST.Equals("A"))
                                        {
                                            gvwPreassesData.Rows[rowIndex].Cells["gvtPQuestions"].Tag = "Y";
                                        }

                                        isResponse = true;
                                        if (fieldType.Equals("D"))
                                        {
                                            List<CustRespEntity> custReponseEntity = _model.FieldControls.GetCustomResponses("PREASSES", response[0].ACTCODE);

                                            foreach (CustomQuestionsEntity custResp in response)
                                            {
                                                string code = custResp.ACTMULTRESP.Trim();
                                                CustRespEntity custRespEntity = custReponseEntity.Find(u => u.DescCode.Trim().Equals(code));
                                                if (custRespEntity != null)
                                                {
                                                    custQuestionResp += custRespEntity.RespDesc;
                                                    custQuestionCode += custResp.ACTMULTRESP.ToString() + " ";
                                                }
                                            }

                                            gvwPreassesData.Rows[rowIndex].Cells[1].Tag = custQuestionCode;
                                            gvwPreassesData.Rows[rowIndex].Cells[1].Value = custQuestionResp;

                                        }
                                        else if (fieldType.Equals("C"))
                                        {
                                            custQuestionResp = response[0].ACTALPHARESP;
                                            gvwPreassesData.Rows[rowIndex].Cells[1].Tag = response[0].ACTALPHARESP;
                                            gvwPreassesData.Rows[rowIndex].Cells[1].Value = response[0].ACTALPHARESP;

                                        }
                                        else if (fieldType.Equals("N"))
                                        {
                                            custQuestionResp = response[0].ACTNUMRESP.ToString();
                                        }
                                        else if (fieldType.Equals("T"))
                                        {
                                            custQuestionResp = LookupDataAccess.Getdate(response[0].ACTDATERESP.ToString());
                                        }
                                        else
                                        {
                                            custQuestionResp = response[0].ACTALPHARESP.ToString();
                                        }

                                    }

                                }

                                if (!dr.CUSTACTIVECUST.Equals("A"))
                                    gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;

                            }
                        }
                    }
                }
                gvwPreassesData.Update();

            }
        }
        #endregion

        private void btnDPoints_Click(object sender, EventArgs e)
        {
            PreassesDimentionForm dimensionform = new PreassesDimentionForm(BaseForm, string.Empty, string.Empty, null);
            dimensionform.StartPosition = FormStartPosition.CenterScreen;
            dimensionform.ShowDialog();
        }

        void Enableoutservicecmb()
        {

            if (propAgencyControlDetails != null) //Added by Sudheer on 12/13/2016
            {
                if (propAgencyControlDetails.State.ToUpper() == "TX")
                {
                    cmbOutService.Visible = true;
                    panel12.Size = new Size(1088, this.panel12.Height);
                }
                else
                {
                    cmbOutService.Visible = false;
                    panel12.Size = new Size(927, this.panel12.Height);
                }
            }
            else //Added by Sudheer on 12/13/2016
            {
                cmbOutService.Visible = false;
                panel12.Size = new Size(927, this.panel12.Height);
            }

        }

        void FamilyTypeWaringMsg(string strFamilyType)
        {
            try
            {
                if (BaseForm.BaseAgencyControlDetails.FTypeSwitch == "Y")
                {
                    if (cmbFamilyType.Items.Count > 0)
                    {
                        if (strFamilyType != string.Empty)
                        {
                            int inthousehold = CaseMST.NoInProg.ToString() == string.Empty ? 0 : Convert.ToInt32(CaseMST.NoInProg);
                            decimal decimalfamilytype = ((Utilities.ListItem)cmbFamilyType.SelectedItem).ScreenCode == null ? 0 : Convert.ToDecimal(((Utilities.ListItem)cmbFamilyType.SelectedItem).ScreenCode);
                            if (decimalfamilytype > 0)
                            {
                                string ScreenType = ((Utilities.ListItem)cmbFamilyType.SelectedItem).ScreenType == null ? "" : (((Utilities.ListItem)cmbFamilyType.SelectedItem).ScreenType);
                                lblFamilyTypeWarning.Visible = false;
                                switch (ScreenType)
                                {

                                    case ">": if (inthousehold > decimalfamilytype) lblFamilyTypeWarning.Visible = false; else lblFamilyTypeWarning.Visible = true; break;
                                    case "=": if (decimalfamilytype == inthousehold) lblFamilyTypeWarning.Visible = false; else lblFamilyTypeWarning.Visible = true; break;
                                    case "<": if (inthousehold < decimalfamilytype) lblFamilyTypeWarning.Visible = false; else lblFamilyTypeWarning.Visible = true; break;
                                    default: lblFamilyTypeWarning.Visible = false; break;
                                }

                                //if (decimalfamilytype != inthousehold)
                                //{
                                //    lblFamilyTypeWarning.Visible = true;
                                //}
                                //else
                                //{
                                //    lblFamilyTypeWarning.Visible = false;
                                //}
                            }
                        }

                    }
                }


            }
            catch (Exception ex)
            {


            }

        }

        private void btnIncompletIntake_Click(object sender, EventArgs e)
        {

            if (BaseForm.BaseCaseSnpEntity != null)
            {
                //IncompleteIntakeForm objform = new IncompleteIntakeForm(BaseForm, Privileges);
                ////objform.FormClosed += new Wisej.Web.Form.FormClosedEventHandler(objform_FormClosed);
                //objform.StartPosition = FormStartPosition.CenterScreen;
                //objform.ShowDialog();
            }
        }

        void objform_FormClosed(object sender, FormClosedEventArgs e)
        {
            // IncompleteIntakeForm form = sender as IncompleteIntakeForm;
            DataSet ds = Captain.DatabaseLayer.CaseSnpData.CAPS_INTKINCOMP_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
            if (ds.Tables[0].Rows.Count > 0)
            {

                btnIncompletIntake.Text = "Incomplete Intake * ";
                btnIncompletIntake.ForeColor = Color.LimeGreen;
            }
            else
            {
                btnIncompletIntake.Text = "Incomplete Intake";
                btnIncompletIntake.ForeColor = Color.White;
            }

        }


        public void GetIncompleteIntake()
        {

            List<CommonEntity> commonIntakeIncompletdata = _model.lookupDataAccess.GetAgyTabs("09997", string.Empty, string.Empty);
            List<CommonEntity> commonEntity = CommonFunctions.AgyTabsFilterCode(commonIntakeIncompletdata, "09997", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);
            if (commonEntity.Count > 0)
            {

                btnIncompletIntake.Visible = false;
                DataSet ds = Captain.DatabaseLayer.CaseSnpData.CAPS_INTKINCOMP_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    btnIncompletIntake.Visible = true;

                    btnIncompletIntake.Text = "Incomplete Intake";
                    btnIncompletIntake.ForeColor = Color.White;
                    //btnIncompletIntake.Text = "Incomplete Intake *";
                    //btnIncompletIntake.ForeColor = Color.Green;
                }
            }
            //else
            //{
            //    btnIncompletIntake.Text = "Incomplete Intake";
            //    btnIncompletIntake.ForeColor = Color.Black;
            //}
            //}
            //else
            //{
            //    btnIncompletIntake.Visible = false;
            //}

        }

        private void gvwCustomer_ShowColumnVisibilityMenuChanged(object sender, EventArgs e)
        {

        }

        private void gvwCustomer_ColumnStateChanged(object sender, DataGridViewColumnStateChangedEventArgs e)
        {
            if (gvwCustomer.Columns["ClientID"].Visible == true)
            {
                pnlFamilyID.Visible = true;
            }
            else
            {
                pnlFamilyID.Visible = false;
            }
        }


        //Added by Sudheer on 11/28/2022
        private void ShowHistoryIcon()
        {
            if (BaseForm.BaseApplicationNo != null)
            {
                CaseSnpEntity caseSnp = GetSelectedRow();
                string fmSeq = "";
                if (caseSnp != null)
                    fmSeq = caseSnp.FamilySeq;
                string StrKey = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo + fmSeq;

                List<CaseHistEntity> caseHistList = _model.CaseMstData.GetCaseHistDetails("CASEMST", StrKey, "CASE2001");
                if (caseHistList.Count > 0)
                {
                    if (ToolBarHistory != null)
                        ToolBarHistory.Visible = true;
                }
                else
                {
                    if (ToolBarHistory != null)
                        ToolBarHistory.Visible = false;
                }
            }
        }

        string PdfName = "pdfName";
        void PrintDSSXMLPdf()
        {
            if (dtDSSXMLData.Rows.Count > 0)
            {
                string _strDSSXMLAppno = dtDSSXMLData.Rows[0]["DXM_APPID"].ToString();
                //PdfListForm form = sender as PdfListForm;
                StringBuilder strMstApplUpdate = new StringBuilder();
                string PdfName = dtDSSXMLData.Rows[0]["DXM_APPID"].ToString(); //dgvUnzipRec.SelectedRows[0].Cells["colAppID"].Value.ToString();
                                                                               //** PdfName = form.GetFileName();

                string propReportPath = _model.lookupDataAccess.GetReportPath();
                PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
                string Random_Filename = null;
                try
                {
                    if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim());
                    }
                }
                catch (Exception ex)
                {
                    AlertBox.Show("Error", MessageBoxIcon.Error);
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

                Document document = new Document(PageSize.A4, 30f, 30f, 30f, 30f);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();
                BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font Calibri = new iTextSharp.text.Font(bf_Calibri, 10);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8);
                iTextSharp.text.Font TblFontR = new iTextSharp.text.Font(bf_Calibri, 9, iTextSharp.text.Font.BOLD, BaseColor.RED);
                iTextSharp.text.Font TblFontG = new iTextSharp.text.Font(bf_Calibri, 9, iTextSharp.text.Font.BOLD, BaseColor.GREEN);
                iTextSharp.text.Font SubHeadFont = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_Calibri, 9, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Calibri, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font TblHFontBold = new iTextSharp.text.Font(bf_Calibri, 11, iTextSharp.text.Font.BOLD, BaseColor.BLUE);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_Calibri, 9, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_Calibri, 9, 4);


                PdfContentByte cb = writer.DirectContent;
                try
                {
                    string strCAPAGY = BaseForm.BaseAgencyControlDetails.AgyShortName;
                    string strZIPfileAGY = strCAPAGY;// DSSXMLData.getZIPfileAGY(strCAPAGY);

                    string AppNo = _strDSSXMLAppno;// dgvUnzipRec.SelectedRows[0].Cells["colAppID"].Value.ToString();
                    System.Data.DataTable dtZipFiles = DSSXMLData.getZippedFiles(BaseForm.BaseDSSXMLDBConnString, strZIPfileAGY, "", "", "C", AppNo, string.Empty, "BYAPPNO");

                    if (dtZipFiles.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtZipFiles.Rows)
                        {
                            if (!string.IsNullOrEmpty(dr["CTZ_XML_FILE"].ToString().Trim()))
                            {
                                string XMLData = dr["CTZ_XML_FILE"].ToString().Trim();
                                StringReader stringReader = new StringReader(XMLData);

                                DataSet dsXMLData = new DataSet();
                                if (dr["CTZ_XML_FILE"].ToString() != "")
                                    dsXMLData.ReadXml(stringReader);

                                DataSet dsTempXML = new DataSet();

                                #region Import all Wanted Tables into dsTempXML table
                                /**/
                                List<string> lstdtblelist = new List<string>();
                                // Get all System.Data.DataTable names in the DataSet
                                foreach (System.Data.DataTable dataTable in dsXMLData.Tables)
                                {
                                    string tableName = dataTable.TableName;
                                    lstdtblelist.Add(tableName);

                                    if (dataTable.TableName == "ApplicationSection")
                                    {
                                        dsTempXML.Tables.Add(dataTable.Copy());
                                    }

                                    if (dataTable.TableName == "LandlordSection")
                                    {
                                        dsTempXML.Tables.Add(dataTable.Copy());
                                    }

                                    if (dataTable.TableName == "SERVICEDETAILS")
                                    {
                                        dsTempXML.Tables.Add(dataTable.Copy());
                                    }

                                    if (dataTable.TableName == "MAILINGDETAILS")
                                    {
                                        dsTempXML.Tables.Add(dataTable.Copy());
                                    }

                                    if (dataTable.TableName == "PROGRAMDETAILSSECTION")
                                    {
                                        dsTempXML.Tables.Add(dataTable.Copy());
                                    }

                                    if (dataTable.TableName == "ADDITIONALQUESTIONSSECTION")
                                    {
                                        dsTempXML.Tables.Add(dataTable.Copy());
                                    }
                                }

                                List<string> lstMembers = lstdtblelist.Where(name => name.Contains("HOUSEHOLDMEMBERS")).ToList();
                                if (lstMembers.Count > 0)
                                {
                                    System.Data.DataTable dt = new System.Data.DataTable();
                                    int x = 0;
                                    foreach (string member in lstMembers)
                                    {
                                        if (x == 0)
                                        {
                                            dt = dsXMLData.Tables[member].Clone();
                                            dt.TableName = "HOUSEHOLDMEMBERS";
                                            x = 1;
                                        }
                                        foreach (DataRow drM in dsXMLData.Tables[member].Rows)
                                        {
                                            dt.ImportRow(drM);
                                        }
                                    }
                                    dsTempXML.Tables.Add(dt);
                                }

                                List<string> lstIncomeDets = lstdtblelist.Where(name => name.Contains("INCOMEDETAILS")).ToList();
                                if (lstIncomeDets.Count > 0)
                                {
                                    System.Data.DataTable dt = new System.Data.DataTable();
                                    int x = 0;
                                    foreach (string member in lstIncomeDets)
                                    {
                                        if (x == 0)
                                        {
                                            dt = dsXMLData.Tables[member].Clone();
                                            dt.TableName = "INCOMEDETAILS";
                                            x = 1;
                                        }
                                        foreach (DataRow drM in dsXMLData.Tables[member].Rows)
                                        {
                                            dt.ImportRow(drM);
                                            // if (_isFirstCols > 0)
                                            // dsFinXML.Tables["INCOMEDETAILS"].ImportRow(drM);
                                        }
                                    }
                                    dsTempXML.Tables.Add(dt);
                                }

                                #endregion

                                PdfPTable tbl = new PdfPTable(1);
                                tbl.WidthPercentage = 100;
                                float[] widths = new float[] { 100f };
                                tbl.SetWidths(widths);
                                tbl.HorizontalAlignment = Element.ALIGN_CENTER;

                                PdfPCell headerddsxml = new PdfPCell(new Phrase("LIHEAP Application Information", TblHFontBold));
                                //headerddsxml.Colspan = 5;
                                headerddsxml.HorizontalAlignment = Element.ALIGN_LEFT;
                                headerddsxml.VerticalAlignment = Element.ALIGN_CENTER;
                                headerddsxml.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(headerddsxml);

                                PdfPCell S1 = new PdfPCell(new Phrase("", TblFontBold));
                                S1.HorizontalAlignment = Element.ALIGN_LEFT;

                                S1.VerticalAlignment = Element.ALIGN_CENTER;
                                S1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(S1);

                                #region Application Section

                                PdfPCell CatElig = new PdfPCell(new Phrase("Categorically Eligbility: " + dsXMLData.Tables["ApplicationSection"].Rows[0]["isImpactEnrolled__c"].ToString(), TblFontBold));
                                CatElig.HorizontalAlignment = Element.ALIGN_LEFT;
                                CatElig.VerticalAlignment = Element.ALIGN_CENTER;
                                CatElig.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(CatElig);

                                S1 = new PdfPCell(new Phrase("", TblFontBold));
                                S1.HorizontalAlignment = Element.ALIGN_LEFT;
                                S1.VerticalAlignment = Element.ALIGN_CENTER;
                                S1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(S1);

                                PdfPCell headerCaptain = new PdfPCell(new Phrase("Basic Details", TblFontBold));
                                headerCaptain.HorizontalAlignment = Element.ALIGN_LEFT;
                                headerCaptain.VerticalAlignment = Element.ALIGN_CENTER;
                                headerCaptain.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(headerCaptain);

                                PdfPCell A1 = new PdfPCell(new Phrase("ID: " + dsXMLData.Tables["ApplicationSection"].Rows[0]["ApplicationId"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                string HousingSituation = string.Empty;
                                if (!string.IsNullOrEmpty(dsXMLData.Tables["ApplicationSection"].Rows[0]["Housing_Situation__c"].ToString().Trim()))
                                    HousingSituation = dsXMLData.Tables["ApplicationSection"].Rows[0]["Housing_Situation__c"].ToString();

                                A1 = new PdfPCell(new Phrase("What is your housing Situation? " + HousingSituation, TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Which Program(s) do you want to apply for today: " + dsXMLData.Tables["ApplicationSection"].Rows[0]["Program_Select__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                DataTable dtMSTRec = new DataTable();
                                DataRow[] drMst = dsTempXML.Tables["HOUSEHOLDMEMBERS"].Select("Is_Primary_Applicant__c='true'");
                                if (drMst.Length > 0)
                                    dtMSTRec = drMst.CopyToDataTable();

                                DataTable dtsnplist = dsTempXML.Tables["HOUSEHOLDMEMBERS"];



                                if (dtMSTRec.Rows.Count > 0)
                                {
                                    //foreach (DataRow dataRow in dtsnplist.Rows)
                                    //{
                                    //if (dataRow["Is_Primary_Applicant__c"].ToString() == "true")
                                    //{
                                    A1 = new PdfPCell(new Phrase("First Name: " + dtMSTRec.Rows[0]["First_Name__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Middle Initial: " + dtMSTRec.Rows[0]["Middle_Name__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Last Name: " + dtMSTRec.Rows[0]["Last_Name__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Gender: " + dtMSTRec.Rows[0]["Sex__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Date of Birth (yyyy-mm-dd): " + dtMSTRec.Rows[0]["Date_of_Birth__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Primary Language: " + dtMSTRec.Rows[0]["Primary_Language__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Social Security Number: " + dtMSTRec.Rows[0]["SSN__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Do you identify as Hispanic, Latinx, or Spanish Origins? " + dtMSTRec.Rows[0]["Ethnicity__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Race(s): " + dtMSTRec.Rows[0]["Additional_Race__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    #region Contact Information

                                    A1 = new PdfPCell(new Phrase("", TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Contact Information", TblFontBold));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Email Address: " + dtMSTRec.Rows[0]["Email__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);


                                    A1 = new PdfPCell(new Phrase("Phone Number: " + dtMSTRec.Rows[0]["Primary_Phone__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);


                                    A1 = new PdfPCell(new Phrase("Phone Type: " + dtMSTRec.Rows[0]["Primary_Phone_Type__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Alternate Phone Number: " + dtMSTRec.Rows[0]["Alternate_Phone__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Alternate Phone Type: " + dtMSTRec.Rows[0]["Alternate_Phone_Type__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Home Address (Street Address, Apt #): " + dsXMLData.Tables["SERVICEDETAILS"].Rows[0]["Address_Line_1__c"].ToString() + " " + dsXMLData.Tables["SERVICEDETAILS"].Rows[0]["Address_Line_2__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("City/Town: " + dsXMLData.Tables["SERVICEDETAILS"].Rows[0]["City__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("State: " + dsXMLData.Tables["SERVICEDETAILS"].Rows[0]["State__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Zip Code: " + dsXMLData.Tables["SERVICEDETAILS"].Rows[0]["AddressZip_Code__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Is Mailing Adress different from the address above? " + dsXMLData.Tables["ApplicationSection"].Rows[0]["Mailing_Address_different_than_Home_Add__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Mailing Address (Street Address, Apt #): " + dsXMLData.Tables["MAILINGDETAILS"].Rows[0]["Address_Line_1__c"].ToString() + " " + dsXMLData.Tables["MAILINGDETAILS"].Rows[0]["Address_Line_2__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);


                                    A1 = new PdfPCell(new Phrase("Mailing City/Town: " + dsXMLData.Tables["MAILINGDETAILS"].Rows[0]["City__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Mailing State: " + dsXMLData.Tables["MAILINGDETAILS"].Rows[0]["State__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Mailing Zip Code: " + dsXMLData.Tables["MAILINGDETAILS"].Rows[0]["AddressZip_Code__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    #endregion

                                    #region Employment Section

                                    A1 = new PdfPCell(new Phrase("", TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Employment Section", TblFontBold));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("What is your employment status? " + dtMSTRec.Rows[0]["Employment_Status__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Are you currently a Student? " + dtMSTRec.Rows[0]["Student_Status__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Have you served in the military " + dtMSTRec.Rows[0]["Have_you_served_in_military__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    DataTable dtDisabled = new DataTable();
                                    DataRow[] drDisabled = dsTempXML.Tables["HOUSEHOLDMEMBERS"].Select("Are_you_with_disability__c='YES'");
                                    if (drDisabled.Length > 0)
                                        dtDisabled = drDisabled.CopyToDataTable();

                                    A1 = new PdfPCell(new Phrase("How many people who are disabled live in your household? " + dtDisabled.Rows.Count.ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("What was your last grade or education level completed, including vocational school?: " + dtMSTRec.Rows[0]["Highest_Education_Level__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Are you a person with a disability? " + dtMSTRec.Rows[0]["Are_you_with_disability__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Which DSS benefit(s) do you receive? " + "", TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    #endregion

                                }

                                #region HOUSEHOLDMEMBERS


                                if (dtsnplist.Rows.Count > 0)
                                {
                                    int Count = 1;
                                    DataRow[] drApp = dtsnplist.Select("Is_Primary_Applicant__c='true'");
                                    if (drApp.Length > 0)
                                    {
                                        A1 = new PdfPCell(new Phrase("", TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);

                                        A1 = new PdfPCell(new Phrase("Household Member " + Count.ToString(), TblFontBold));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);

                                        A1 = new PdfPCell(new Phrase("First Name: " + drApp[0]["First_Name__c"].ToString(), TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);

                                        A1 = new PdfPCell(new Phrase("Middle Initial: " + drApp[0]["Middle_Name__c"].ToString(), TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);

                                        A1 = new PdfPCell(new Phrase("Last Name: " + drApp[0]["Last_Name__c"].ToString(), TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);

                                        A1 = new PdfPCell(new Phrase("Gender: " + drApp[0]["Sex__c"].ToString(), TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);

                                        A1 = new PdfPCell(new Phrase("Date of Birth (yyyy-mm-dd): " + drApp[0]["Date_of_Birth__c"].ToString(), TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);

                                        A1 = new PdfPCell(new Phrase("Primary Language: " + drApp[0]["Primary_Language__c"].ToString(), TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);

                                        A1 = new PdfPCell(new Phrase("Social Security Number: " + drApp[0]["SSN__c"].ToString(), TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);



                                        A1 = new PdfPCell(new Phrase("Race(s): " + drApp[0]["Additional_Race__c"].ToString(), TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);

                                        A1 = new PdfPCell(new Phrase("Do you identify as Hispanic, Latinx, or Spanish Origins? " + drApp[0]["Ethnicity__c"].ToString(), TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);

                                        A1 = new PdfPCell(new Phrase("Relationship to Applicant? " + drApp[0]["CEAP_Relationships__c"].ToString(), TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);



                                        A1 = new PdfPCell(new Phrase("What is your employment status? " + drApp[0]["Employment_Status__c"].ToString(), TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);

                                        A1 = new PdfPCell(new Phrase("Are you currently a Student? " + drApp[0]["Student_Status__c"].ToString(), TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);

                                        A1 = new PdfPCell(new Phrase("Have you served in the military " + drApp[0]["Have_you_served_in_military__c"].ToString(), TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);

                                        A1 = new PdfPCell(new Phrase("Are you a person with a disability? " + drApp[0]["Are_you_with_disability__c"].ToString(), TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);


                                        A1 = new PdfPCell(new Phrase("What was your last grade or education level completed, including vocational school?: " + drApp[0]["Highest_Education_Level__c"].ToString(), TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);

                                        A1 = new PdfPCell(new Phrase("What benefits you receive? " + "", TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);


                                        A1 = new PdfPCell(new Phrase("You’ve indicated that you are a person with a disability who does NOT receive disability benefits like\r\nSupplemental Security Income or the State Supplement for Aged, Blind, or Disabled. Is that right?", TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);

                                        Count++;
                                    }

                                    foreach (DataRow dataRow in dtsnplist.Rows)
                                    {
                                        if (dataRow["Is_Primary_Applicant__c"].ToString() == "false")
                                        {
                                            A1 = new PdfPCell(new Phrase("", TableFont));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);

                                            A1 = new PdfPCell(new Phrase("Household Member " + Count.ToString(), TblFontBold));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);

                                            A1 = new PdfPCell(new Phrase("First Name: " + dataRow["First_Name__c"].ToString(), TableFont));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);

                                            A1 = new PdfPCell(new Phrase("Middle Initial: " + dataRow["Middle_Name__c"].ToString(), TableFont));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);

                                            A1 = new PdfPCell(new Phrase("Last Name: " + dataRow["Last_Name__c"].ToString(), TableFont));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);

                                            A1 = new PdfPCell(new Phrase("Gender: " + dataRow["Sex__c"].ToString(), TableFont));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);

                                            A1 = new PdfPCell(new Phrase("Date of Birth (yyyy-mm-dd): " + dataRow["Date_of_Birth__c"].ToString(), TableFont));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);

                                            A1 = new PdfPCell(new Phrase("Primary Language: " + dataRow["Primary_Language__c"].ToString(), TableFont));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);

                                            A1 = new PdfPCell(new Phrase("Social Security Number: " + dataRow["SSN__c"].ToString(), TableFont));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);



                                            A1 = new PdfPCell(new Phrase("Race(s): " + dataRow["Additional_Race__c"].ToString(), TableFont));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);

                                            A1 = new PdfPCell(new Phrase("Do you identify as Hispanic, Latinx, or Spanish Origins? " + dataRow["Ethnicity__c"].ToString(), TableFont));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);

                                            A1 = new PdfPCell(new Phrase("Relationship to Applicant? " + dataRow["CEAP_Relationships__c"].ToString(), TableFont));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);



                                            A1 = new PdfPCell(new Phrase("What is your employment status? " + dataRow["Employment_Status__c"].ToString(), TableFont));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);

                                            A1 = new PdfPCell(new Phrase("Are you currently a Student? " + dataRow["Student_Status__c"].ToString(), TableFont));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);

                                            A1 = new PdfPCell(new Phrase("Have you served in the military " + dataRow["Have_you_served_in_military__c"].ToString(), TableFont));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);

                                            A1 = new PdfPCell(new Phrase("Are you a person with a disability? " + dataRow["Are_you_with_disability__c"].ToString(), TableFont));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);


                                            A1 = new PdfPCell(new Phrase("What was your last grade or education level completed, including vocational school?: " + dataRow["Highest_Education_Level__c"].ToString(), TableFont));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);

                                            A1 = new PdfPCell(new Phrase("What benefits you receive? " + "", TableFont));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);


                                            A1 = new PdfPCell(new Phrase("You’ve indicated that you are a person with a disability who does NOT receive disability benefits like\r\nSupplemental Security Income or the State Supplement for Aged, Blind, or Disabled. Is that right? " + dataRow["Recieve_Disability_benefits__c"].ToString(), TableFont));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);

                                            Count++;
                                        }

                                    }

                                }

                                #endregion

                                #region Income Details

                                List<string> IncomeDets = lstdtblelist.Where(name => name.Contains("INCOMEDETAILS")).ToList();
                                if (IncomeDets.Count > 0)
                                {

                                    A1 = new PdfPCell(new Phrase("", TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Income Details", TblFontBold));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    DataTable dtinc = new DataTable();
                                    dtinc = dsTempXML.Tables["INCOMEDETAILS"];

                                    if (dtinc.Rows.Count > 0)
                                    {
                                        DataTable dtIncEmployed = new DataTable();
                                        DataRow[] drincEmp = dtinc.Select("Income_Type__c='Employment'");
                                        if (drincEmp.Length > 0) dtIncEmployed = drincEmp.CopyToDataTable();

                                        DataTable dtIncSelfEmployed = new DataTable();
                                        DataRow[] drincSelfEmp = dtinc.Select("Income_Type__c='Self-Employment'");
                                        if (drincSelfEmp.Length > 0) dtIncSelfEmployed = drincSelfEmp.CopyToDataTable();

                                        DataTable dtIncAddtInc = new DataTable();
                                        DataRow[] drincAddtInc = dtinc.Select("Income_Type__c='Additional Income'");
                                        if (drincAddtInc.Length > 0) dtIncAddtInc = drincAddtInc.CopyToDataTable();

                                        string EmployedInc = string.Empty, SelfEmp = string.Empty, AddtionalInc = string.Empty;
                                        if (dtIncEmployed.Rows.Count > 0) EmployedInc = "Yes"; else EmployedInc = "No";
                                        if (dtIncSelfEmployed.Rows.Count > 0) SelfEmp = "Yes"; else SelfEmp = "No";
                                        if (dtIncAddtInc.Rows.Count > 0) AddtionalInc = "Yes"; else AddtionalInc = "No";

                                        A1 = new PdfPCell(new Phrase("Is anyone in your household employed? " + EmployedInc, TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);

                                        A1 = new PdfPCell(new Phrase("Is anyone in your household self-employed? " + SelfEmp, TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);

                                        A1 = new PdfPCell(new Phrase("Does anyone in your household receive additional income? " + AddtionalInc, TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);

                                        if (EmployedInc == "Yes")
                                        {
                                            A1 = new PdfPCell(new Phrase("Employment:", TblFontBold));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);

                                            if (dtIncEmployed.Rows.Count > 0)
                                            {
                                                foreach (DataRow drinc in dtIncEmployed.Rows)
                                                {
                                                    string Name = string.Empty;

                                                    DataTable dtSNPRec = new DataTable();
                                                    DataRow[] drsnp = dsTempXML.Tables["HOUSEHOLDMEMBERS"].Select("HouseholdMemberSystemID_c='" + drinc["HHMember_ID_c"].ToString() + "'");
                                                    if (drsnp.Length > 0)
                                                        dtSNPRec = drsnp.CopyToDataTable();


                                                    if (dtSNPRec.Rows.Count > 0)
                                                    {
                                                        A1 = new PdfPCell(new Phrase("Household Member :" + dtSNPRec.Rows[0]["First_Name__c"].ToString() + " " + dtSNPRec.Rows[0]["Last_Name__c"].ToString(), TableFont));
                                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                                        A1.BorderColor = BaseColor.WHITE;
                                                        tbl.AddCell(A1);

                                                        A1 = new PdfPCell(new Phrase("Income Frequency :" + drinc["Wages_Tips_Frequency__c"].ToString(), TableFont));
                                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                                        A1.BorderColor = BaseColor.WHITE;
                                                        tbl.AddCell(A1);

                                                        A1 = new PdfPCell(new Phrase("Income Amount :" + drinc["Gross_Monthly_Income__c"].ToString(), TableFont));
                                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                                        A1.BorderColor = BaseColor.WHITE;
                                                        tbl.AddCell(A1);

                                                        A1 = new PdfPCell(new Phrase("Does anyone else in your household have employment income?" + "", TableFont));
                                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                                        A1.BorderColor = BaseColor.WHITE;
                                                        tbl.AddCell(A1);

                                                    }

                                                }

                                            }
                                        }
                                        if (SelfEmp == "Yes")
                                        {
                                            A1 = new PdfPCell(new Phrase("Self Employment:", TblFontBold));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);

                                            if (dtIncSelfEmployed.Rows.Count > 0)
                                            {
                                                foreach (DataRow drinc in dtIncSelfEmployed.Rows)
                                                {
                                                    string Name = string.Empty;

                                                    DataTable dtSNPRec = new DataTable();
                                                    DataRow[] drsnp = dsTempXML.Tables["HOUSEHOLDMEMBERS"].Select("HouseholdMemberSystemID_c='" + drinc["HHMember_ID_c"].ToString() + "'");
                                                    if (drsnp.Length > 0)
                                                        dtSNPRec = drsnp.CopyToDataTable();


                                                    if (dtSNPRec.Rows.Count > 0)
                                                    {
                                                        A1 = new PdfPCell(new Phrase("Household Member :" + dtSNPRec.Rows[0]["First_Name__c"].ToString() + " " + dtSNPRec.Rows[0]["Last_Name__c"].ToString(), TableFont));
                                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                                        A1.BorderColor = BaseColor.WHITE;
                                                        tbl.AddCell(A1);

                                                        A1 = new PdfPCell(new Phrase("Income Frequency :" + drinc["Wages_Tips_Frequency__c"].ToString(), TableFont));
                                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                                        A1.BorderColor = BaseColor.WHITE;
                                                        tbl.AddCell(A1);

                                                        A1 = new PdfPCell(new Phrase("Income Amount :" + drinc["Gross_Monthly_Income__c"].ToString(), TableFont));
                                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                                        A1.BorderColor = BaseColor.WHITE;
                                                        tbl.AddCell(A1);

                                                        A1 = new PdfPCell(new Phrase("Does anyone else in your household have employment income?" + "", TableFont));
                                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                                        A1.BorderColor = BaseColor.WHITE;
                                                        tbl.AddCell(A1);

                                                    }

                                                }

                                            }
                                        }
                                        if (AddtionalInc == "Yes")
                                        {
                                            A1 = new PdfPCell(new Phrase("Additional Income:", TblFontBold));
                                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            A1.VerticalAlignment = Element.ALIGN_CENTER;
                                            A1.BorderColor = BaseColor.WHITE;
                                            tbl.AddCell(A1);

                                            if (dtIncAddtInc.Rows.Count > 0)
                                            {
                                                foreach (DataRow drinc in dtIncAddtInc.Rows)
                                                {
                                                    string Name = string.Empty;

                                                    DataTable dtSNPRec = new DataTable();
                                                    DataRow[] drsnp = dsTempXML.Tables["HOUSEHOLDMEMBERS"].Select("HouseholdMemberSystemID_c='" + drinc["HHMember_ID_c"].ToString() + "'");
                                                    if (drsnp.Length > 0)
                                                        dtSNPRec = drsnp.CopyToDataTable();


                                                    if (dtSNPRec.Rows.Count > 0)
                                                    {
                                                        A1 = new PdfPCell(new Phrase("Household Member :" + dtSNPRec.Rows[0]["First_Name__c"].ToString() + " " + dtSNPRec.Rows[0]["Last_Name__c"].ToString(), TableFont));
                                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                                        A1.BorderColor = BaseColor.WHITE;
                                                        tbl.AddCell(A1);

                                                        A1 = new PdfPCell(new Phrase("Income Frequency :" + drinc["Wages_Tips_Frequency__c"].ToString(), TableFont));
                                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                                        A1.BorderColor = BaseColor.WHITE;
                                                        tbl.AddCell(A1);

                                                        A1 = new PdfPCell(new Phrase("Income Amount :" + drinc["Gross_Monthly_Income__c"].ToString(), TableFont));
                                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                                        A1.BorderColor = BaseColor.WHITE;
                                                        tbl.AddCell(A1);

                                                        A1 = new PdfPCell(new Phrase("Does anyone else in your household have employment income?" + "", TableFont));
                                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                                        A1.BorderColor = BaseColor.WHITE;
                                                        tbl.AddCell(A1);

                                                    }

                                                }

                                            }
                                        }


                                    }
                                }




                                #endregion

                                if (dtMSTRec.Rows.Count > 0)
                                {
                                    A1 = new PdfPCell(new Phrase("", TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Child Support ", TblFontBold));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Does your household include children who have one or more non-custodial parents? " + dtMSTRec.Rows[0]["ChildHavingNonCustodialParent"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);

                                    A1 = new PdfPCell(new Phrase("Are you receiving child support? " + dtMSTRec.Rows[0]["RecieveingChildSupport__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);
                                }



                                #region Energy Information

                                //if (dsXMLData.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["IsElectricStr__c"].ToString() == "NO")
                                //{
                                A1 = new PdfPCell(new Phrase("", TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Energy Information: " + "", TblFontBold));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("What type of home do you live in? " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Type_Of_Home_To_Live__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("What is your method for paying for heat? " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Payment_Method_for_Heat__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("What is your primary source of heat? " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Primary_Source_of_Heat__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                if (!string.IsNullOrEmpty(dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Primary_Source_of_Heat__c"].ToString().Trim()))
                                {
                                    if (dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Primary_Source_of_Heat__c"].ToString().Trim().ToUpper() == "OIL" || dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Primary_Source_of_Heat__c"].ToString().Trim().ToUpper() == "PROPANE" || dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Primary_Source_of_Heat__c"].ToString().Trim().ToUpper() == "KEROSENE")
                                    {
                                        A1 = new PdfPCell(new Phrase("What is the name of your primary heat source fuel dealer or utility company? " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Name_of_primary_heat_source_fuel_dealer__c"].ToString(), TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);
                                    }
                                    else
                                    {
                                        A1 = new PdfPCell(new Phrase("What is the name of your primary heat source fuel dealer or utility company? " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Name_of_your_primary_heat_source_company__c"].ToString(), TableFont));
                                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        A1.VerticalAlignment = Element.ALIGN_CENTER;
                                        A1.BorderColor = BaseColor.WHITE;
                                        tbl.AddCell(A1);
                                    }

                                }
                                else
                                {
                                    A1 = new PdfPCell(new Phrase("What is the name of your primary heat source fuel dealer or utility company? " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Name_of_your_primary_heat_source_company__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);
                                }

                                A1 = new PdfPCell(new Phrase("Name on primary heat account: " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Name_on_primary_heat_account__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Heating account number: " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Heating_account_number__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Is your fuel tank shared with another household? " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Fuel_Tank_Shared_with_someone__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Do you have a disconnect notice? " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Electric_Account_Disconnect_Notice__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Are you currently disconnected? " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Electric_Account_Currently_Disconnected__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Disconnection Date (yyyy-mm-dd): " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Electric_Account_Disconnnection_Date__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);
                                #endregion
                                //}
                                //else
                                //{
                                #region Electricity Information

                                A1 = new PdfPCell(new Phrase("", TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Electricity Information: " + "", TblFontBold));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Electricity Payment Method: " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Payment_Method_for_Electricity__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Electricity Provider Company: " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Name_of_Electric_Company__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Electricity Bill Name: " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Name_on_Electric_Account__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Electricity Account Number: " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Electric_Account_Number__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Do you have a disconnect notice? " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Electric_Account_Disconnect_Notice__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Are you currently disconnected? " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Electric_Account_Currently_Disconnected__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Disconnection Date (yyyy-mm-dd): " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Electric_Account_Disconnnection_Date__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);
                                //}
                                #endregion

                                #region Weatherization and CTT (Optional)

                                A1 = new PdfPCell(new Phrase("", TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Weatherization and CTT (Optional): " + "", TblFontBold));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Are you interested in free weatherization services? " + dsXMLData.Tables["ApplicationSection"].Rows[0]["Free_Weatherization_Services__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Are you interested in free Clean, Test, and Tune Services? " + dsXMLData.Tables["ApplicationSection"].Rows[0]["Interested_in_Free_Services__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Landlord or Agent or Company Name: " + dsXMLData.Tables["LandlordSection"].Rows[0]["Landlord_Company_Name__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Landlord/Agent/Company Telephone: " + dsXMLData.Tables["LandlordSection"].Rows[0]["Landlord_Company_Phone__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Landlord Address: " + dsXMLData.Tables["LandlordSection"].Rows[0]["Landlord_Company_Address__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Landlord City/Town: " + dsXMLData.Tables["LandlordSection"].Rows[0]["Landlord_City__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Landlord State: " + dsXMLData.Tables["LandlordSection"].Rows[0]["Landlord_State__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Landlord Zip Code: " + dsXMLData.Tables["LandlordSection"].Rows[0]["Landlord_AddressZip_Code__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                #endregion

                                #region Water Information

                                A1 = new PdfPCell(new Phrase("", TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Water Information " + "", TblFontBold));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("What is your method for paying for water? " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Payment_method_for_water__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Is your water meter shared with multiple households? " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Water_shared_with_multiple_houses__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("What is the name of your water provider? " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Name_of_Water_Provider__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Name on Water Account: " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Name_on_Water_Account__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Water account number: " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Water_Account_Number__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Do you have a disconnect notice? " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Water_Disconnect_Notice__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Are you currently disconnected? " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Water_service_currently_disconnected__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Disconnection Date (yyyy-mm-dd): " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Water_Disconnection_Date__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                #endregion

                                #region Wastewater Information

                                A1 = new PdfPCell(new Phrase("", TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Wastewater Information " + "", TblFontBold));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("What is your method for paying for wastewater/sewer? Wastewater and sewer included in rent" + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Payment_method_for_Wastewater__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Are your wastewater/sewer services shared with multiple households? " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Wastewater_shared_with_multiple_houses__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("What is the name of your wastewater/sewer service provider? " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Name_of_Wastewater_Provider__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Name on wastewater/sewer account: " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Wastewater_account_name__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Wastewater/sewer account number: " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Wastewater_Account_Number__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Do you owe a past-due amount on your wastewater/sewer bill? " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Past_due_Amount_on_Wastewater_Bill__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Do you have a disconnect notice? " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Wastewater_disconnected_notice__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Are you currently disconnected? " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Wastewater_currently_disconnected__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Disconnection Date (yyyy-mm-dd): " + dsXMLData.Tables["PROGRAMDETAILSSECTION"].Rows[0]["Wastewater_Disconnection_Date__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                #endregion

                                #region Additional Questions

                                A1 = new PdfPCell(new Phrase("", TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Additional Questions " + "", TblFontBold));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Have you lived in your current residence for at least a year? " + dsXMLData.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["LivedInCurrentHomeForAtleastAYear__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Have you used the same utility company for a year or longer? " + dsXMLData.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Used_Same_Utility_Company_1_Year__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Is your heating system currently operable?: " + dsXMLData.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Heating_System_currently_operable__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Can you afford to have your heating system repaired or replaced? " + dsXMLData.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Heating_system_repaired_or_replaced__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Is your service currently disconnected? " + "", TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Can you afford to pay your utility company to restore services? " + dsXMLData.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Utility_Company_To_Restore_Services__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Have you received a shut-off notice within the last 30 days? " + dsXMLData.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["RecievedShutOffNoticeInLast30Days__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Can you afford to pay the utility company what you owe so that you can avoid disconnection? " + dsXMLData.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Utility_Company_To_Avoid_Disconnection__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Is your household currently protected from service disconnection through medical protection: " + dsXMLData.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Service_Discon_Protected_By_Medical_Prot__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Do you have a water bill in your name? " + dsXMLData.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Water_Bill_in_your_name__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Do you owe a past-due amount on your water bill? " + dsXMLData.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Owe_past_due_amount_on_water_bill__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Do you have a sewer bill in your name? " + dsXMLData.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Sewer_Bill_in_your_name__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Do you owe a past-due amount on your sewer bill? " + dsXMLData.Tables["ADDITIONALQUESTIONSSECTION"].Rows[0]["Owe_past_due_amount_on_sewer_bill__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                #endregion

                                #region Authorized Representative

                                A1 = new PdfPCell(new Phrase("", TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Authorized Representative and Accommodation Information (Optional)" + "", TblFontBold));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                if (dsXMLData.Tables["ApplicationSection"].Columns.Contains("Want_to_designate_authorized_rep__c"))
                                {
                                    A1 = new PdfPCell(new Phrase("Do you want to designate an authorized representative? " + dsXMLData.Tables["ApplicationSection"].Rows[0]["Want_to_designate_authorized_rep__c"].ToString(), TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);
                                }
                                else
                                {
                                    A1 = new PdfPCell(new Phrase("Do you want to designate an authorized representative? ", TableFont));
                                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A1.VerticalAlignment = Element.ALIGN_CENTER;
                                    A1.BorderColor = BaseColor.WHITE;
                                    tbl.AddCell(A1);
                                }

                                A1 = new PdfPCell(new Phrase("Do you need reasonable accommodation or extra help getting benefits because of a disability or impairment? " + dsXMLData.Tables["ApplicationSection"].Rows[0]["Needs_Impairment_Assistance__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Designated Authorized Representative (First Name): " + dsXMLData.Tables["ApplicationSection"].Rows[0]["Designated_Auth_Rep_First_Name__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Designated Authorized Representative (Last Name): " + dsXMLData.Tables["ApplicationSection"].Rows[0]["Designated_Auth_Rep_Last_Name__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Designated Authorized Representative (Phone #): " + dsXMLData.Tables["ApplicationSection"].Rows[0]["Designated_Auth_Rep_Phone_Number__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Designated Authorized Representative (City/Town): " + dsXMLData.Tables["ApplicationSection"].Rows[0]["Designated_Auth_Rep_City__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Designated Authorized Representative (State): " + dsXMLData.Tables["ApplicationSection"].Rows[0]["Designated_Auth_Rep_State__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Designated Authorized Representative (Zip Code): " + dsXMLData.Tables["ApplicationSection"].Rows[0]["Designated_Auth_Rep_Zip__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Applicant Signature (For Authorized Representative): " + dsXMLData.Tables["ApplicationSection"].Rows[0]["Applicant_Sign__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Date (yyyy-mm-dd): " + dsXMLData.Tables["ApplicationSection"].Rows[0]["Applicant_Sign_Date__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Authorized Representative Signature: " + "", TableFont));//dsXMLData.Tables["ApplicationSection"].Rows[0]["Designated_Auth_Rep_Last_Name__c"].ToString()
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Date (yyyy-mm-dd): " + dsXMLData.Tables["ApplicationSection"].Rows[0]["Designated_Auth_Rep_Sign_Date__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("", TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Sign and Submit" + "", TblFontBold));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Applicant Signature: " + dsXMLData.Tables["ApplicationSection"].Rows[0]["Applicant_Sign__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                A1 = new PdfPCell(new Phrase("Date (yyyy-mm-dd): " + dsXMLData.Tables["ApplicationSection"].Rows[0]["Applicant_Sign_Date__c"].ToString(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.VerticalAlignment = Element.ALIGN_CENTER;
                                A1.BorderColor = BaseColor.WHITE;
                                tbl.AddCell(A1);

                                #endregion


                                #endregion
                                document.Add(tbl);
                            }
                        }


                        document.Close();
                        fs.Close();
                        fs.Dispose();

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


                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }


            }


        }
        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(PdfName);
        }



    }
}