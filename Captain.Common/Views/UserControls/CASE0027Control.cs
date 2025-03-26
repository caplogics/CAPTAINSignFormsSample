/************************************************************************
 * Conversion On    :   12/15/2022      * Converted By     :   Kranthi
 * Modified On      :   12/15/2022      * Modified By      :   Kranthi
 * **********************************************************************/
#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Captain.Common.Views.Forms;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Utilities;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Wisej.Web;
using DevExpress.DataProcessing.InMemoryDataProcessor;
using static NPOI.HSSF.Util.HSSFColor;
#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class CASE0027Control : BaseUserControl
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private string strYear = "    ";
        private int strIndex = 0;

        #endregion
        public CASE0027Control(BaseForm baseForm, PrivilegeEntity privileges, string strSearchmode)
            : base(baseForm)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privileges;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 1;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            dtStartDate.Value = DateTime.Now.AddDays(-7);
            //dtEndDate.Value = DateTime.Now;



            caseworkerfilling();
            if (strSearchmode != string.Empty)
            {
                if (BaseForm.BaseClientFolloupFromDate != string.Empty)
                {
                    dtStartDate.Value = Convert.ToDateTime(BaseForm.BaseClientFolloupFromDate);
                }
                if (BaseForm.BaseClientFolloupToDate != string.Empty)
                {
                    dtEndDate.Value = Convert.ToDateTime(BaseForm.BaseClientFolloupToDate);
                }
                CommonFunctions.SetComboBoxValue(cmbCaseworker, BaseForm.BaseClientFolloupWorker.Trim());

                btnSearch_Click(btnSearch, new EventArgs());

                foreach (DataGridViewRow gvrow in gvwDetails.Rows)
                {
                    DataRow dritem = gvrow.Tag as DataRow;
                    if (dritem != null)
                    {
                        string strAppdetails = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear.Trim() + BaseForm.BaseApplicationNo;
                        if (dritem["MST_AGENCY"].ToString() + dritem["MST_DEPT"].ToString() + dritem["MST_PROGRAM"].ToString() + dritem["MST_YEAR"].ToString().Trim() + dritem["SNP_APP"].ToString() == strAppdetails)
                        {
                            gvrow.Selected = true;
                            break;
                        }
                    }
                }

            }

            PopulateToolbar(oToolbarMnustrip);
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public ToolBarButton ToolBarEdit { get; set; }

        // public ToolBarButton ToolBarNew { get; set; }

        // public ToolBarButton ToolBarDel { get; set; }

        public ToolBarButton ToolBarPrint { get; set; }

        public ToolBarButton ToolBarHelp { get; set; }

        public ProgramDefinitionEntity ProgramDefinition { get; set; }

        List<CommonEntity> propfundingsource { get; set; }

        public List<CAMASTEntity> propCAMASTList { get; set; }


        List<CASESP0Entity> propSP0Entity { get; set; }

        // public List<CASEVDDEntity> propCaseVddlist { get; set; }

        public List<CommonEntity> propRejectData { get; set; }

        public string strEMSAccess { get; set; }

        public string propReportPath { get; set; }
        #endregion

        public override void PopulateToolbar(ToolBar toolBar)
        {
            base.PopulateToolbar(toolBar);

            bool toolbarButtonInitialized = ToolBarEdit != null;
            ToolBarButton divider = new ToolBarButton();
            divider.Style = ToolBarButtonStyle.Separator;

            ToolBarHelp = new ToolBarButton();
            ToolBarHelp.Tag = "Help";
            ToolBarHelp.ToolTipText = "Help";
            ToolBarHelp.Enabled = true;
            ToolBarHelp.ImageSource = "icon-help";
            ToolBarHelp.Click -= new EventHandler(OnToolbarButtonClicked);
            ToolBarHelp.Click += new EventHandler(OnToolbarButtonClicked);


            //if (Privileges.AddPriv.Equals("true") && Privileges.ChangePriv.Equals("true") && Privileges.DelPriv.Equals("true"))
            //{
            //    if (ToolBarEdit != null) ToolBarEdit.Enabled = true;
            //}
            //else
            //{
            //    if (ToolBarEdit != null)
            //    {
            //        ToolBarEdit.Enabled = false;
            //    }
            //}

            //if (Privileges.ChangePriv.Equals("false"))
            //{
            //    if (ToolBarEdit != null) ToolBarEdit.Enabled = false;
            //}
            //else
            //{
            //    if (ToolBarEdit != null) ToolBarEdit.Enabled = true;
            //}

            //if (Privileges.DelPriv.Equals("false"))
            //{
            //    if (ToolBarDel != null) ToolBarDel.Enabled = false;
            //}
            //else
            //{
            //    if (ToolBarDel != null) ToolBarDel.Enabled = true;
            //}

            toolBar.Buttons.AddRange(new ToolBarButton[]
            {
                //ToolBarNew,
               // ToolBarEdit,
               // ToolBarDel,
               //ToolBarPrint,
                ToolBarHelp
            });

            //if (gvwData.Rows.Count == 0)
            //{
            //    //if (ToolBarEdit != null)
            //    //    ToolBarEdit.Enabled = false;
            //}
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

                    case Consts.ToolbarActions.Edit:

                        break;
                    //case Consts.ToolbarActions.Delete:
                    //    if (GetSelectedRow() != null)
                    //    {

                    //      MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxHandler, true);

                    //    }
                    //    break;
                    case Consts.ToolbarActions.Print:
                        //On_SaveForm_Closed();
                        break;
                    case Consts.ToolbarActions.Help:
                        //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "CASE0025");
                        //Application.Navigate(CommonFunctions.BuildHelpURLS(Privileges.Program, 1, BaseForm.BusinessModuleID.ToString()), target: "_blank");
                        Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 1, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
                        break;
                }
                executeCode.Append(Consts.Javascript.EndJavascriptCode);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
            }
        }

        private void caseworkerfilling()
        {
            string strNameFormat = string.Empty;
            string strCwFormat = string.Empty;

            strNameFormat = BaseForm.BaseHierarchyCnFormat; //ds.Tables[0].Rows[0]["HIE_CN_FORMAT"].ToString();
            strCwFormat = BaseForm.BaseHierarchyCwFormat;//ds.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();


            cmbCaseworker.Items.Clear();
            cmbCaseworker.ColorMember = "FavoriteColor";
            List<HierarchyEntity> hierarchyEntity = _model.CaseMstData.GetCaseWorker(strCwFormat, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);

            string strCaseworkerDefault = string.Empty;
            foreach (HierarchyEntity caseworker in hierarchyEntity)
            {

                cmbCaseworker.Items.Add(new ListItem(caseworker.HirarchyName.ToString(), caseworker.CaseWorker.ToString(), caseworker.InActiveFlag, caseworker.InActiveFlag.Equals("N") ? Color.Black : Color.Red));
                if (caseworker.UserID.Trim().ToString().ToUpper() == BaseForm.UserID.ToUpper().Trim().ToString())
                {
                    strCaseworkerDefault = caseworker.CaseWorker == null ? "0" : caseworker.CaseWorker.Trim();
                }
            }
            cmbCaseworker.Items.Insert(0, new ListItem("", "0"));
            // cmbCaseworker.Items.Insert(1, new ListItem("No Caseworker Entered", "99"));
            cmbCaseworker.SelectedIndex = 0;
            if (strCaseworkerDefault != string.Empty)
                CommonFunctions.SetComboBoxValue(cmbCaseworker, strCaseworkerDefault.Trim());

            if (BaseForm.UserProfile.Security == "C")
            {
                cmbCaseworker.Enabled = false;
            }
            else
            {
                if (BaseForm.UserProfile.Security == "B")
                {
                    cmbCaseworker.Enabled = true;
                }
            }
        }


        string propScreencode = string.Empty;
        List<CommonEntity> commonData = new List<CommonEntity>();
        private void btnSel_Click(object sender, EventArgs e)
        {

            AlertCodeForm objform = new AlertCodeForm(BaseForm, Privileges, propScreencode, string.Empty, commonData);
            objform.FormClosed += new FormClosedEventHandler(objform_NonCashBenefitFormClosed);
            objform.StartPosition = FormStartPosition.CenterScreen;
            objform.ShowDialog();
        }
        void objform_NonCashBenefitFormClosed(object sender, FormClosedEventArgs e)
        {
            AlertCodeForm form = sender as AlertCodeForm;
            propScreencode = form.propAlertCode;
        }

        List<CaseMstEntity> propMstList = new List<CaseMstEntity>();
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (Validate_Report())
            {
                propMstList.Clear();
                string strScreencode = string.Empty;
                if (rdoAllScreen.Checked)
                {
                    strScreencode = string.Empty;
                    BaseForm.BaseClientFolloupScreentype = "A";
                    BaseForm.BaseClientFolloupScreencode = string.Empty;
                }
                else
                {
                    strScreencode = propScreencode;
                    BaseForm.BaseClientFolloupScreentype = "S";
                    BaseForm.BaseClientFolloupScreencode = propScreencode;
                }
                string strCaseworker = string.Empty;

                if (!((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("0"))
                {
                    strCaseworker = ((ListItem)cmbCaseworker.SelectedItem).Value.ToString();
                }
                BaseForm.BaseClientFolloupFromDate = dtStartDate.Value.ToShortDateString();
                BaseForm.BaseClientFolloupToDate = dtEndDate.Value.ToShortDateString();
                BaseForm.BaseClientFolloupWorker = strCaseworker;

                gvwDetails.SelectionChanged -= new EventHandler(gvwDetails_SelectionChanged);
                gvwDetails.Rows.Clear();
                gvwActionDetails.Rows.Clear();
                //  DataSet dslist = Captain.DatabaseLayer.SPAdminDB.CASE0027_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, dtStartDate.Value.ToShortDateString(), dtEndDate.Value.ToShortDateString(), strCaseworker, strScreencode, string.Empty);
                DataSet dslist = Captain.DatabaseLayer.SPAdminDB.CASE0027_GET(string.Empty, string.Empty, string.Empty, string.Empty, dtStartDate.Value.ToShortDateString(), dtEndDate.Value.ToShortDateString(), strCaseworker, strScreencode, string.Empty);

                if (dslist.Tables.Count > 1)
                {

                    foreach (DataRow drmstitem in dslist.Tables[1].Rows)
                    {
                        propMstList.Add(new Model.Objects.CaseMstEntity(drmstitem, "CASE0027"));
                    }
                }

                if (dslist.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dritem in dslist.Tables[0].Rows)
                    {
                        int i = gvwDetails.Rows.Add(dritem["MST_AGENCY"].ToString() + "-" + dritem["MST_DEPT"].ToString() + "-" + dritem["MST_PROGRAM"].ToString(), dritem["MST_YEAR"].ToString(), dritem["SNP_APP"].ToString(), LookupDataAccess.GetMemberName(dritem["SNP_NAME_IX_FI"].ToString(), dritem["SNP_NAME_IX_MI"].ToString(), dritem["SNP_NAME_IX_LAST"].ToString(), BaseForm.BaseHierarchyCnFormat), Set_Adresslabel(dritem), LookupDataAccess.GetPhoneFormat2(dritem["MST_AREA"].ToString() + dritem["MST_PHONE"].ToString()), "...");
                        gvwDetails.Rows[i].Tag = dritem;
                    }
                    if(gvwDetails.Rows.Count>0)
                        gvwDetails.Rows[0].Selected= true;
                }
                else
                {
                    CommonFunctions.MessageBoxDisplay("No records found");
                }
                gvwDetails.SelectionChanged += new EventHandler(gvwDetails_SelectionChanged);
                gvwDetails_SelectionChanged(sender, e);
            }
        }


        private bool Validate_Report()
        {
            bool Can_Generate = true;
            _errorProvider.SetError(dtStartDate, null);
            _errorProvider.SetError(dtEndDate, null);
            _errorProvider.SetError(cmbCaseworker, null);
            if (((ListItem)cmbCaseworker.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbCaseworker, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCaseworker.Text.Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
            {
                _errorProvider.SetError(cmbCaseworker, null);
            }
            if (!dtStartDate.Checked)
            {
                _errorProvider.SetError(dtStartDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Follow - up On - Start Date"/*lblForm.Text*/.Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(dtStartDate, null);

            if (!dtEndDate.Checked)
            {
                _errorProvider.SetError(dtEndDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Follow - up On - End Date" /*+ lblTo.Text*/.Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
            {
                if (dtStartDate.Checked)
                    _errorProvider.SetError(dtEndDate, null);
            }
            if (dtStartDate.Checked.Equals(true) && dtEndDate.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(dtStartDate.Text))
                {
                    _errorProvider.SetError(dtStartDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Follow - up On - Start Date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(dtStartDate, null);
                }
                if (string.IsNullOrWhiteSpace(dtEndDate.Text))
                {
                    _errorProvider.SetError(dtEndDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Follow - up On - End Date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(dtEndDate, null);
                }
            }

            if (dtStartDate.Checked && dtEndDate.Checked)
            {
                if (!string.IsNullOrEmpty(dtStartDate.Text) && (!string.IsNullOrEmpty(dtEndDate.Text)))
                {
                    if (Convert.ToDateTime(dtStartDate.Text) > Convert.ToDateTime(dtEndDate.Text))
                    {
                        _errorProvider.SetError(dtStartDate, string.Format("'Start Date' should be prior to 'End Date'".Replace(Consts.Common.Colon, string.Empty)));
                        Can_Generate = false;
                    }
                }
            }
            else
            {
                if (dtEndDate.Checked && dtStartDate.Checked)
                    _errorProvider.SetError(dtStartDate, null);
            }

            return Can_Generate;
        }


        private string Set_Adresslabel(DataRow dr)
        {
            string strAddress = string.Empty;
            string Apt = string.Empty; string Floor = string.Empty; string HN = string.Empty; string Suffix = string.Empty; string Street = string.Empty;
            string Zip = string.Empty;
            if (!string.IsNullOrEmpty(dr["MST_HN"].ToString().Trim()))
                strAddress = dr["MST_HN"].ToString() + " ";

            if (!string.IsNullOrEmpty(dr["MST_STREET"].ToString()))
                strAddress = strAddress + dr["MST_STREET"].ToString() + " ";

            if (!string.IsNullOrEmpty(dr["MST_SUFFIX"].ToString()))
                strAddress = strAddress + dr["MST_SUFFIX"].ToString() + ", ";

            if (!string.IsNullOrEmpty(dr["MST_APT"].ToString()))
                strAddress = strAddress + " " + dr["MST_APT"].ToString() + "  ";
            if (!string.IsNullOrEmpty(dr["MST_FLR"].ToString()))
                strAddress = strAddress + dr["MST_FLR"].ToString() + ", ";

            if (!string.IsNullOrEmpty(dr["MST_CITY"].ToString()))
                strAddress = strAddress + dr["MST_CITY"].ToString() + ", ";

            if (!string.IsNullOrEmpty(dr["MST_STATE"].ToString()))
                strAddress = strAddress + dr["MST_STATE"].ToString() + "  ";


            if (!string.IsNullOrEmpty(dr["MST_ZIP"].ToString()) && dr["MST_ZIP"].ToString().Trim() != "0")
                Zip = "00000".Substring(0, 5 - dr["MST_ZIP"].ToString().Trim().Length) + dr["MST_ZIP"].ToString().Trim();



            return strAddress + Zip;

        }

        private void rdoAllScreen_Click(object sender, EventArgs e)
        {
            if (rdoSelect.Checked)
            {
                btnSel.Enabled = true;
            }
            else
            {
                btnSel.Enabled = false;
            }
        }

        private void gvwDetails_SelectionChanged(object sender, EventArgs e)
        {
            int introwindex = 0;
            if (gvwDetails.Rows.Count > 0)
            {
                gvwActionDetails.Rows.Clear();

                DataRow dr = gvwDetails.SelectedRows[0].Tag as DataRow;
                if (dr != null)
                {
                    List<CaseMstEntity> mstdata = propMstList.FindAll(u => u.ApplAgency.Trim() == dr["MST_AGENCY"].ToString().Trim() && u.ApplDept.Trim() == dr["MST_DEPT"].ToString().Trim() && u.ApplProgram.Trim() == dr["MST_PROGRAM"].ToString().Trim() && u.ApplYr.Trim() == dr["MST_YEAR"].ToString().Trim() && u.ApplNo.Trim() == dr["SNP_APP"].ToString().Trim());

                    if (mstdata.FindAll(u => u.Mode == "MST").Count > 0)
                    {
                        introwindex = gvwActionDetails.Rows.Add("Client Intake", "Case Review Date", LookupDataAccess.Getdate(mstdata[0].IntakeDate), LookupDataAccess.Getdate(mstdata[0].CaseReviewDate));

                        caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("CASE2001", dr["MST_AGENCY"].ToString() + dr["MST_DEPT"].ToString() + dr["MST_PROGRAM"].ToString() + dr["MST_YEAR"].ToString() + gvwDetails.SelectedRows[0].Cells["gvtApp"].Value.ToString().Trim());
                        if (caseNotesEntity.Count > 0)
                        {
                           //gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesEdit;
                            gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Style.BackgroundImageSource = "captain-casenotes";
                        }
                        else
                        {
                            //gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesAdd;
                            gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Style.BackgroundImageSource = "captain-casenotesadd";
                        }
                        caseNotesEntity = caseNotesEntity;
                    }
                    if (mstdata.FindAll(u => u.Mode == "VER").Count > 0)
                    {
                        List<CaseVerEntity> caseverdata = _model.CaseMstData.GetCASEVeradpyalst(dr["MST_AGENCY"].ToString().Trim(), dr["MST_DEPT"].ToString().Trim(), dr["MST_PROGRAM"].ToString().Trim(), dr["MST_YEAR"].ToString().Trim(), dr["SNP_APP"].ToString().Trim(), string.Empty, string.Empty);
                        caseverdata = caseverdata.FindAll(u => u.ReverifyDate != string.Empty);
                        caseverdata = caseverdata.FindAll(u => LookupDataAccess.Getdate(u.ReverifyDate) == LookupDataAccess.Getdate(mstdata.FindAll(ui => ui.Mode == "VER")[0].ReverifyDate));

                        if (!((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("0"))
                        {
                            if (((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("99"))
                                caseverdata = caseverdata.FindAll(u => u.Verifier.Trim() == string.Empty);
                            else
                                caseverdata = caseverdata.FindAll(u => u.Verifier.Trim() == ((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Trim());
                        }
                        string strFDate = string.Empty;
                        string strTDate = string.Empty;
                        if (caseverdata.Count > 0)
                        {
                            strFDate = caseverdata[0].VerifyDate;
                            strTDate = caseverdata[0].ReverifyDate;

                        }

                        introwindex = gvwActionDetails.Rows.Add("Income Verification", "Reverification Date", LookupDataAccess.Getdate(strFDate), LookupDataAccess.Getdate(strTDate));
                        gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Style.BackgroundImageSource = "blank"; 

                        //caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + gvwDetails.SelectedRows[0].Cells["gvtApp"].Value.ToString().Trim());
                        //if (caseNotesEntity.Count > 0)
                        //{
                        //    gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesEdit;//"captain-casenotes";
                        //}
                        //else
                        //{
                        //    gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesAdd;//"captain-casenotesadd";
                        //}
                        //caseNotesEntity = caseNotesEntity;
                    }
                    //if (mstdata.FindAll(u => u.Mode == "SER").Count > 0)
                    //{
                    //    CASESPMEntity Search_Entity = new CASESPMEntity(true);

                    //    Search_Entity.agency = dr["MST_AGENCY"].ToString().Trim();
                    //    Search_Entity.dept = dr["MST_DEPT"].ToString();
                    //    Search_Entity.program = dr["MST_PROGRAM"].ToString();
                    //    Search_Entity.year = dr["MST_YEAR"].ToString().Trim();
                    //    Search_Entity.app_no = dr["SNP_APP"].ToString().Trim();


                    //    List<CASESPMEntity> CASESPM_List = _model.SPAdminData.Browse_CASESPM(Search_Entity, "Browse");
                    //    string strFDate = string.Empty;
                    //    string strTDate = string.Empty;
                    //    if (CASESPM_List.Count > 0)
                    //    {
                    //        strFDate = CASESPM_List[0].startdate;
                    //        strTDate = CASESPM_List[0].estdate;
                    //        gvwActionDetails.Rows.Add("Service Plan Master", CASESPM_List[0].Sp0_Desc, LookupDataAccess.Getdate(strFDate), LookupDataAccess.Getdate(strTDate));

                    //    }

                    //}
                    if (mstdata.FindAll(u => u.Mode == "ACT").Count > 0)
                    {
                        CASEACTEntity Search_Enty = new CASEACTEntity(true);
                        Search_Enty.Agency = dr["MST_AGENCY"].ToString().Trim();
                        Search_Enty.Dept = dr["MST_DEPT"].ToString().Trim();
                        Search_Enty.Program = dr["MST_PROGRAM"].ToString().Trim();
                        Search_Enty.Year = dr["MST_YEAR"].ToString().Trim();
                        //Search_Enty.Agency = BaseForm.BaseAgency;
                        //Search_Enty.Dept = BaseForm.BaseDept;
                        //Search_Enty.Program = BaseForm.BaseProg;
                        //Search_Enty.Year = BaseForm.BaseYear;
                        Search_Enty.App_no = dr["SNP_APP"].ToString().Trim();


                        List<CASEACTEntity> Sel_SP_Activity_Details = _model.SPAdminData.Browse_CASEACT(Search_Enty, "Browse");

                        Sel_SP_Activity_Details = Sel_SP_Activity_Details.FindAll(u => u.Followup_On != string.Empty && u.Followup_Comp.Trim() == string.Empty);
                        Sel_SP_Activity_Details = Sel_SP_Activity_Details.FindAll(u => Convert.ToDateTime(u.Followup_On) >= dtStartDate.Value.Date && Convert.ToDateTime(u.Followup_On) <= dtEndDate.Value.Date);
                        if (!((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("0"))
                        {
                            if (((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("99"))
                                Sel_SP_Activity_Details = Sel_SP_Activity_Details.FindAll(u => u.Caseworker.Trim() == string.Empty);
                            else
                                Sel_SP_Activity_Details = Sel_SP_Activity_Details.FindAll(u => u.Caseworker.Trim() == ((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Trim());
                        }
                        string strFDate = string.Empty;
                        string strTDate = string.Empty;
                        string strDesc = string.Empty;
                        if (Sel_SP_Activity_Details.Count > 0)
                        {

                            foreach (CASEACTEntity Actitem in Sel_SP_Activity_Details)
                            {


                                List<CAMASTEntity> CAMASTList = _model.SPAdminData.Browse_CAMAST("Code", Actitem.ACT_Code, null, null);
                                if (CAMASTList.Count > 0)
                                    strDesc = CAMASTList[0].Desc;

                                strFDate = Actitem.ACT_Date;
                                strTDate = Actitem.Followup_On;
                                introwindex = gvwActionDetails.Rows.Add("Service Details", strDesc, LookupDataAccess.Getdate(strFDate), LookupDataAccess.Getdate(strTDate));

                                gvwActionDetails.Rows[introwindex].Cells["gvtScreenName"].Tag = Actitem;

                                Sel_CAMS_Notes_Key = Actitem.Agency + Actitem.Dept + Actitem.Program + Actitem.Year + gvwDetails.SelectedRows[0].Cells["gvtApp"].Value.ToString().Trim() + Actitem.Service_plan.Trim() + Actitem.SPM_Seq + Actitem.Branch.Trim() + Actitem.Group.ToString() + "CA" + Actitem.ACT_Code.Trim() + Actitem.ACT_Seq.Trim() + Actitem.ACT_ID.Trim();

                                caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("CASE00063", Sel_CAMS_Notes_Key);

                                if (caseNotesEntity.Count > 0)
                                {
                                   // gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesEdit;
                                    gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Style.BackgroundImageSource = "captain-casenotes";
                                }
                                else
                                {
                                    //gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesAdd;
                                    gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Style.BackgroundImageSource = "captain-casenotesadd";
                                }
                                caseNotesEntity = caseNotesEntity;
                            }
                        }

                    }
                    if (mstdata.FindAll(u => u.Mode == "MS").Count > 0)
                    {
                        CASEMSEntity Search_Enty = new CASEMSEntity(true);
                        Search_Enty.Agency = dr["MST_AGENCY"].ToString().Trim();
                        Search_Enty.Dept = dr["MST_DEPT"].ToString().Trim();
                        Search_Enty.Program = dr["MST_PROGRAM"].ToString().Trim();
                        Search_Enty.Year = dr["MST_YEAR"].ToString().Trim();
                        //Search_Enty.Agency = BaseForm.BaseAgency;
                        //Search_Enty.Dept = BaseForm.BaseDept;
                        //Search_Enty.Program = BaseForm.BaseProg;
                        //Search_Enty.Year = BaseForm.BaseYear;
                        Search_Enty.App_no = dr["SNP_APP"].ToString().Trim();


                        List<CASEMSEntity> Sel_SP_MS_Details = _model.SPAdminData.Browse_CASEMS(Search_Enty, "Browse");
                        Sel_SP_MS_Details = Sel_SP_MS_Details.FindAll(u => u.MS_FUP_Date != string.Empty && u.MS_Comp_Date.Trim() == string.Empty);
                        Sel_SP_MS_Details = Sel_SP_MS_Details.FindAll(u => Convert.ToDateTime(u.MS_FUP_Date) >= dtStartDate.Value.Date && Convert.ToDateTime(u.MS_FUP_Date) <= dtEndDate.Value.Date);

                        if (!((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("0"))
                        {
                            if (((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("99"))
                                Sel_SP_MS_Details = Sel_SP_MS_Details.FindAll(u => u.CaseWorker.Trim() == string.Empty);
                            else
                                Sel_SP_MS_Details = Sel_SP_MS_Details.FindAll(u => u.CaseWorker.Trim() == ((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Trim());
                        }
                        string strFDate = string.Empty;
                        string strTDate = string.Empty;
                        string strDesc = string.Empty;
                        if (Sel_SP_MS_Details.Count > 0)
                        {
                            foreach (CASEMSEntity Msitem in Sel_SP_MS_Details)
                            {

                                List<MSMASTEntity> CAMASTList = _model.SPAdminData.Browse_MSMAST("Code", Msitem.MS_Code, null, null, null);
                                if (CAMASTList.Count > 0)
                                    strDesc = CAMASTList[0].Desc;

                                strFDate = Msitem.Date;
                                strTDate = Msitem.MS_FUP_Date;
                                introwindex = gvwActionDetails.Rows.Add("Outcome Details", strDesc, LookupDataAccess.Getdate(strFDate), LookupDataAccess.Getdate(strTDate));

                                gvwActionDetails.Rows[introwindex].Cells["gvtScreenName"].Tag = Msitem;

                                Sel_CAMS_Notes_Key = Msitem.Agency + Msitem.Dept + Msitem.Program + Msitem.Year + gvwDetails.SelectedRows[0].Cells["gvtApp"].Value.ToString().Trim() + Msitem.Service_plan.Trim() + Msitem.SPM_Seq + Msitem.Branch.Trim() + Msitem.Group.ToString() + "MS" + Msitem.MS_Code.Trim() + Convert.ToDateTime(Msitem.Date).ToString("MM/dd/yyyy");

                                caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("CASE00064", Sel_CAMS_Notes_Key);
                                if (caseNotesEntity.Count > 0)
                                {
                                    //gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesEdit;
                                    gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Style.BackgroundImageSource = "captain-casenotes";
                                }
                                else
                                {
                                    //gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesAdd;
                                    gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Style.BackgroundImageSource = "captain-casenotesadd";
                                }
                                caseNotesEntity = caseNotesEntity;
                            }
                        }
                    }
                    if (mstdata.FindAll(u => u.Mode == "CON").Count > 0)
                    {
                        string strFDate = string.Empty;
                        string strTDate = string.Empty;
                        string strDesc = string.Empty;
                        CASECONTEntity Cont_Search_Entity = new CASECONTEntity();
                        Cont_Search_Entity.Agency = dr["MST_AGENCY"].ToString().Trim();
                        Cont_Search_Entity.Dept = dr["MST_DEPT"].ToString().Trim();
                        Cont_Search_Entity.Program = dr["MST_PROGRAM"].ToString().Trim();
                        Cont_Search_Entity.Year = dr["MST_YEAR"].ToString().Trim();
                        //Cont_Search_Entity.Agency = BaseForm.BaseAgency;
                        //Cont_Search_Entity.Dept = BaseForm.BaseDept;
                        //Cont_Search_Entity.Program = BaseForm.BaseProg;
                        Cont_Search_Entity.App_no = dr["SNP_APP"].ToString().Trim();

                        //Cont_Search_Entity.Year = "    ";
                        //if (!string.IsNullOrEmpty(BaseForm.BaseYear))
                        //    Cont_Search_Entity.Year = BaseForm.BaseYear;

                        Cont_Search_Entity.Contact_No = Cont_Search_Entity.Contact_Name = Cont_Search_Entity.CaseWorker = Cont_Search_Entity.Cont_Date = null;
                        Cont_Search_Entity.Duration_Type = Cont_Search_Entity.Time = Cont_Search_Entity.Time_Starts = Cont_Search_Entity.Time_Ends = null;
                        Cont_Search_Entity.Duration = Cont_Search_Entity.How_Where = null;
                        Cont_Search_Entity.Language = Cont_Search_Entity.Interpreter = Cont_Search_Entity.Refer_From = Cont_Search_Entity.BillTO = Cont_Search_Entity.BillTo_UOM = null;
                        Cont_Search_Entity.Cust1_Code = Cont_Search_Entity.Cust1_Value = Cont_Search_Entity.Cust2_Code = Cont_Search_Entity.Cust2_Value = Cont_Search_Entity.Cust3_Code = null;

                        Cont_Search_Entity.Cust3_Value = Cont_Search_Entity.Bridge = Cont_Search_Entity.Lsct_Operator = Cont_Search_Entity.Lstc_Date = Cont_Search_Entity.Add_Date = null;
                        Cont_Search_Entity.Add_Operator = null;

                        int TmpCount = 0, Cont_Sel_Index = 0;
                        List<CASECONTEntity> CASECONT_List = _model.SPAdminData.Browse_CASECONT(Cont_Search_Entity, "Browse");
                        CASECONT_List = CASECONT_List.FindAll(u => u.FollowuponDate != string.Empty && u.FollowupCompleteDate == string.Empty);
                        if (!((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("0"))
                        {
                            if (((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("99"))
                                CASECONT_List = CASECONT_List.FindAll(u => u.CaseWorker.Trim() == string.Empty);
                            else
                                CASECONT_List = CASECONT_List.FindAll(u => u.CaseWorker.Trim() == ((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Trim());
                        }
                        CASECONT_List = CASECONT_List.FindAll(u => Convert.ToDateTime(u.FollowuponDate) >= dtStartDate.Value.Date && Convert.ToDateTime(u.FollowuponDate) <= dtEndDate.Value.Date);

                        foreach (CASECONTEntity contactitem in CASECONT_List)
                        {
                            if (CASECONT_List.Count > 0)
                            {
                                strDesc = contactitem.Contact_Name;
                                strFDate = contactitem.Cont_Date;
                                strTDate = contactitem.FollowuponDate;
                            }
                            introwindex = gvwActionDetails.Rows.Add("Contact Details", strDesc, LookupDataAccess.Getdate(strFDate), LookupDataAccess.Getdate(strTDate), "", contactitem.Seq);

                            gvwActionDetails.Rows[introwindex].Cells["gvtScreenName"].Tag = contactitem;

                            string Tmp_Seq = string.Empty;
                            if (!string.IsNullOrEmpty(contactitem.Seq.ToString()))
                                Tmp_Seq = contactitem.Seq.ToString();
                            Sel_Cont_Notes_Key = contactitem.Agency + contactitem.Dept + contactitem.Program + contactitem.Year + gvwDetails.SelectedRows[0].Cells["gvtApp"].Value.ToString().Trim() + "0000".Substring(0, (4 - Tmp_Seq.Length)) + Tmp_Seq;

                            //Get_PROG_Notes_Status();

                            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("CASE00061", Sel_Cont_Notes_Key);
                            if (caseNotesEntity.Count > 0)
                            {
                                //gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesEdit;
                                gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Style.BackgroundImageSource = "captain-casenotes";
                            }
                            else
                            {
                                //gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesAdd;
                                gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Style.BackgroundImageSource = "captain-casenotesadd";
                            }
                            caseNotesEntity = caseNotesEntity;
                        }
                    }
                    if (mstdata.FindAll(u => u.Mode == "MAT").Count > 0)
                    {
                        string strFDate = string.Empty;
                        string strTDate = string.Empty;
                        string strDesc = string.Empty;
                        List<MATAPDTSEntity> matapdtsList = _model.MatrixScalesData.GETMatapdts(dr["MST_AGENCY"].ToString().Trim(), dr["MST_DEPT"].ToString().Trim(), dr["MST_PROGRAM"].ToString().Trim(), dr["MST_YEAR"].ToString().Trim(), dr["SNP_APP"].ToString().Trim(), string.Empty, string.Empty, string.Empty);

                        matapdtsList = matapdtsList.FindAll(u => u.FOllowON != string.Empty && u.FollowCDate == string.Empty);
                        matapdtsList = matapdtsList.FindAll(u => Convert.ToDateTime(u.FOllowON) >= dtStartDate.Value.Date && Convert.ToDateTime(u.FOllowON) <= dtEndDate.Value.Date);
                        if (!((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("0"))
                        {
                            if (((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("99"))
                                matapdtsList = matapdtsList.FindAll(u => u.SSworker.Trim() == string.Empty);
                            else
                                matapdtsList = matapdtsList.FindAll(u => u.SSworker.Trim() == ((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Trim());
                        }

                        if (matapdtsList.Count > 0)
                        {
                            MATDEFEntity Search_Entity = new MATDEFEntity(true);
                            Search_Entity.Mat_Code = matapdtsList[0].MatCode;
                            Search_Entity.Scale_Code = "0";
                            List<MATDEFEntity> propmatdefEntity = _model.MatrixScalesData.Browse_MATDEF(Search_Entity, "Browse");

                            if (propmatdefEntity.Count > 0)
                            {
                                strDesc = propmatdefEntity[0].Desc;

                                //if (matapdtsList.Count > 1)
                                //{
                                //    strFDate = matapdtsList[1].SSDate;
                                //    strTDate = matapdtsList[0].SSDate;
                                //}
                                //else
                                //{
                                strFDate = matapdtsList[0].SSDate;
                                strTDate = matapdtsList[0].FOllowON;
                                //}
                            }
                        }

                        introwindex = gvwActionDetails.Rows.Add("Matrix/Scales Assessments", strDesc, LookupDataAccess.Getdate(strFDate), LookupDataAccess.Getdate(strTDate));

                        //gvwActionDetails.Rows[introwindex].Tag = 

                        caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("MAT00003", dr["MST_AGENCY"].ToString() + dr["MST_DEPT"].ToString() + dr["MST_PROGRAM"].ToString() + dr["MST_YEAR"].ToString() + gvwDetails.SelectedRows[0].Cells["gvtApp"].Value.ToString().Trim());
                        if (caseNotesEntity.Count > 0)
                        {
                            //gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesEdit;
                            gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Style.BackgroundImageSource = "captain-casenotes";
                        }
                        else
                        {
                            //gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesAdd;
                            gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Style.BackgroundImageSource = "captain-casenotesadd";
                        }
                        caseNotesEntity = caseNotesEntity;
                    }

                }
            }
            if (gvwActionDetails.Rows.Count > 0)
                gvwActionDetails.Rows[0].Selected = true;
        }

        private void gvwDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == gvbShow.Index)
                {
                    DataRow dr = gvwDetails.SelectedRows[0].Tag as DataRow;
                    if (dr != null)
                    {
                        string strHierarchy = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
                        BaseForm.BaseAgency = dr["MST_AGENCY"].ToString().Trim();
                        BaseForm.BaseDept = dr["MST_DEPT"].ToString().Trim();
                        BaseForm.BaseProg = dr["MST_PROGRAM"].ToString().Trim();
                        BaseForm.BaseYear = dr["MST_YEAR"].ToString().Trim() == string.Empty ? "    " : dr["MST_YEAR"].ToString();
                        BaseForm.BaseApplicationNo = dr["SNP_APP"].ToString();
                        BaseForm.BasePIPDragSwitch = "Y";
                        BaseForm.BaseTopApplSelect = "Y";
                        ShowHierachyandApplNo(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, strHierarchy);

                        //Case0027ActionItemsForm actionform = new Case0027ActionItemsForm(dr["SNP_APP"].ToString().Trim(), gvwActionDetails);
                        //actionform.ShowDialog();
                    }
                }
            }
        }

        private void gvwDetails_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvwDetails.Rows.Count > 0)
            {
                if (gvwDetails.SelectedRows[0].Selected)
                {
                    if (e.ColumnIndex == gvbShow.Index)
                    { }
                    else
                    {
                        DataRow dr = gvwDetails.SelectedRows[0].Tag as DataRow;
                        if (dr != null)
                        {
                            string strHierarchy = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
                            BaseForm.BaseAgency = dr["MST_AGENCY"].ToString().Trim();
                            BaseForm.BaseDept = dr["MST_DEPT"].ToString().Trim();
                            BaseForm.BaseProg = dr["MST_PROGRAM"].ToString().Trim();
                            BaseForm.BaseYear = dr["MST_YEAR"].ToString().Trim() == string.Empty ? "    " : dr["MST_YEAR"].ToString();
                            BaseForm.BaseApplicationNo = dr["SNP_APP"].ToString();
                            BaseForm.BasePIPDragSwitch = "Y";
                            BaseForm.BaseTopApplSelect = "Y";
                            ShowHierachyandApplNo(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, strHierarchy);
                        }
                    }
                }
            }
        }
        public void ShowHierachyandApplNo(string strAgency, string strDept, string strProg, string strYear1, string strApplicationNo, string strHierchy)
        {


            CaseMstEntity caseMstEntity = null;
            List<CaseSnpEntity> caseSnpEntity = null;
            string strYear = strYear1;
            string strApplNo = strApplicationNo;
            {
                caseMstEntity = _model.CaseMstData.GetCaseMST(strAgency, strDept, strProg, strYear1, strApplNo);
                if (caseMstEntity != null)
                {
                    strApplNo = caseMstEntity.ApplNo;
                    strYear = caseMstEntity.ApplYr;
                }
            }


            if (string.IsNullOrEmpty(strYear1))
                strYear = "    ";

            string strAgencyName = strAgency + " - " + _model.lookupDataAccess.GetHierachyDescription("1", strAgency, strDept, strProg);    // Yeswanth
            string strDeptName = strDept + " - " + _model.lookupDataAccess.GetHierachyDescription("2", strAgency, strDept, strProg);
            string strProgName = strProg + " - " + _model.lookupDataAccess.GetHierachyDescription("3", strAgency, strDept, strProg);

            BaseForm.BaseAgency = strAgency;
            BaseForm.BaseDept = strDept;
            BaseForm.BaseProg = strProg;

            if (string.IsNullOrEmpty(strYear))
                strYear = "    ";

            BaseForm.BaseYear = strYear;

            if (caseMstEntity != null)
            {
                caseSnpEntity = _model.CaseMstData.GetCaseSnpadpyn(strAgency, strDept, strProg, strYear, strApplNo);
                BaseForm.BaseApplicationNo = strApplicationNo;

            }
            else   // Yeswanth
                caseSnpEntity = null;
            if (caseMstEntity == null)
            {
                BaseForm.BaseCaseMstListEntity = null;
                BaseForm.BaseCaseSnpEntity = null;
                BaseForm.BaseApplicationNo = string.Empty; // null; Modified by Yeswanth on 01052013
            }

            //BaseForm.BaseTopApplSelect = "Y";
            if (strHierchy != BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg)
            {
                BaseForm.RefreshNavigationTabs(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg);

                BaseForm.BaseTopApplSelect = !string.IsNullOrEmpty(strApplicationNo.Trim()) ? "Y" : "N";

                BaseForm.GetApplicantDetails(caseMstEntity, caseSnpEntity, strAgencyName, strDeptName, strProgName, strYear.ToString(), string.Empty, !string.IsNullOrEmpty(strApplicationNo.Trim()) ? "Display" : string.Empty);
                BaseForm.AddTabClientIntake("CASE0027");

            }
            else
            {
                BaseForm.BaseTopApplSelect = !string.IsNullOrEmpty(strApplicationNo.Trim()) ? "Y" : "N";
                BaseForm.GetApplicantDetails(caseMstEntity, caseSnpEntity, strAgencyName, strDeptName, strProgName, strYear.ToString(), string.Empty, !string.IsNullOrEmpty(strApplicationNo.Trim()) ? "Display" : string.Empty);

            }
        }

        public List<CaseNotesEntity> caseNotesEntity
        {
            get; set;
        }


        string Sel_CAMS_Notes_Key = string.Empty;
        string Sel_Cont_Notes_Key = string.Empty;
        string strItemDesc = string.Empty;
        //string Img_CaseNotesAdd = Consts.Icons.ico_CaseNotes_New;
        //string Img_CaseNotesEdit = Consts.Icons.ico_CaseNotes_View;
        string strItemHie = string.Empty;
        string strItemApp = string.Empty;
        string strItemYear = string.Empty;
        string strItemName = string.Empty;

        private void gvwActionDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex == gvtNotes.Index)
            {
                strItemDesc = gvwActionDetails.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();
                strItemHie = gvwDetails.SelectedRows[0].Cells["gvtHierachy"].Value.ToString().Replace("-", "");
                strItemApp = gvwDetails.SelectedRows[0].Cells["gvtApp"].Value.ToString().Trim();
                strItemYear = gvwDetails.SelectedRows[0].Cells["gvtYear"].Value.ToString();
                strItemName = gvwDetails.SelectedRows[0].Cells["gvtClientName"].Value.ToString().Trim();

                if (strItemDesc == "Client Intake")
                {
                    caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("CASE2001", strItemHie + strItemYear + strItemApp);

                    CaseNotes caseNotes = new CaseNotes(BaseForm, Privileges, caseNotesEntity, strItemHie + strItemYear + strItemApp, "CASE2001", strItemApp, strItemName);
                    caseNotes.FormClosed += new FormClosedEventHandler(OnCaseNotesFormClosed);
                    caseNotes.StartPosition = FormStartPosition.CenterScreen;
                    caseNotes.ShowDialog();
                }

                if (strItemDesc == "Income Verification")
                {
                    //caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo);

                    //CaseNotes caseNotes = new CaseNotes(BaseForm, Privileges, caseNotesEntity, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo);
                    //caseNotes.FormClosed += new FormClosedEventHandler(OnCaseNotesFormClosed);
                    //caseNotes.StartPosition = FormStartPosition.CenterScreen;
                    //caseNotes.ShowDialog();
                }

                if (strItemDesc == "Service Details")
                {
                    CASEACTEntity Entity = gvwActionDetails.SelectedRows[0].Cells["gvtScreenName"].Tag as CASEACTEntity;

                    Sel_CAMS_Notes_Key = strItemHie + strItemYear + strItemApp + Entity.Service_plan.Trim() + Entity.SPM_Seq + Entity.Branch.Trim() + Entity.Group.ToString() + "CA" + Entity.ACT_Code.Trim() + Entity.ACT_Seq.Trim() + Entity.ACT_ID.Trim();
                    ProgressNotes_Form Prog_Form = new ProgressNotes_Form(BaseForm, "Edit", Privileges, Sel_CAMS_Notes_Key, "CASE0063", strItemApp, strItemName);
                    Prog_Form.FormClosed += new FormClosedEventHandler(On_PROGNOTES_Closed);
                    Prog_Form.StartPosition = FormStartPosition.CenterScreen;
                    Prog_Form.ShowDialog();
                }

                if (strItemDesc == "Outcome Details")
                {
                    CASEMSEntity Entity = gvwActionDetails.SelectedRows[0].Cells["gvtScreenName"].Tag as CASEMSEntity;
                    Sel_CAMS_Notes_Key = strItemHie + strItemYear + strItemApp + Entity.Service_plan.Trim() + Entity.SPM_Seq + Entity.Branch.Trim() + Entity.Group.ToString() + "MS" + Entity.MS_Code.Trim() + Convert.ToDateTime(Entity.Date).ToString("MM/dd/yyyy");
                    ProgressNotes_Form Prog_Form = new ProgressNotes_Form(BaseForm, "Edit", Privileges, Sel_CAMS_Notes_Key, "CASE0064", strItemApp, strItemName);
                    Prog_Form.FormClosed += new FormClosedEventHandler(On_PROGNOTES_Closed);
                    Prog_Form.StartPosition = FormStartPosition.CenterScreen;
                    Prog_Form.ShowDialog();
                }

                if (strItemDesc == "Contact Details")
                {
                    string Tmp_Seq = string.Empty;
                    if (!string.IsNullOrEmpty(gvwActionDetails.CurrentRow.Cells["Cont_Seq"].Value.ToString().Trim()))
                        Tmp_Seq = gvwActionDetails.CurrentRow.Cells["Cont_Seq"].Value.ToString();
                    Sel_Cont_Notes_Key = strItemHie + strItemYear + strItemApp + "0000".Substring(0, (4 - Tmp_Seq.Length)) + Tmp_Seq;

                    Get_PROG_Notes_Status();
                    ProgressNotes_Form Prog_Form = new ProgressNotes_Form(BaseForm, "Edit", Privileges, Sel_Cont_Notes_Key, "CONT", "CASE00061", strItemApp, strItemName);
                    Prog_Form.FormClosed += new FormClosedEventHandler(On_PROGNOTES_Closed);
                    Prog_Form.StartPosition = FormStartPosition.CenterScreen;
                    Prog_Form.ShowDialog();
                }

                if (strItemDesc == "Matrix/Scales Assessments")
                {
                    caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("MAT00003", strItemHie + strItemYear + strItemApp);
                    CaseNotes caseNotes = new CaseNotes(BaseForm, Privileges, caseNotesEntity, strItemHie + strItemYear + strItemApp, "MAT00003", strItemApp, strItemName);
                    caseNotes.FormClosed += new FormClosedEventHandler(OnCaseNotesFormClosed);
                    caseNotes.StartPosition = FormStartPosition.CenterScreen;
                    caseNotes.ShowDialog();
                }
            }

        }

        private void OnCaseNotesFormClosed(object sender, FormClosedEventArgs e)
        {
            CaseNotes form = sender as CaseNotes;

            string strYear = "    ";
            if (!string.IsNullOrEmpty(BaseForm.BaseYear))
            {
                strYear = BaseForm.BaseYear;
            }
            if (strItemDesc == "Matrix/Scales Assessments")
            {
                caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("MAT00003", strItemHie + strItemYear + strItemApp);
            }
            if (strItemDesc == "Client Intake")
            {
                caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("CASE2001", strItemHie + strItemYear + strItemApp);
            }
            if (caseNotesEntity.Count > 0)
            {
                //gvwActionDetails.SelectedRows[0].Cells["gvtNotes"].Value = Img_CaseNotesEdit;//"captain-casenotes";
                gvwActionDetails.SelectedRows[0].Cells["gvtNotes"].Style.BackgroundImageSource = "captain-casenotes";
            }
            else
            {
                //gvwActionDetails.SelectedRows[0].Cells["gvtNotes"].Value = Img_CaseNotesAdd;//"captain-casenotesadd";
                gvwActionDetails.SelectedRows[0].Cells["gvtNotes"].Style.BackgroundImageSource = "captain-casenotesadd";
            }
            caseNotesEntity = caseNotesEntity;
        }

        private void On_PROGNOTES_Closed(object sender, FormClosedEventArgs e)
        {
            ProgressNotes_Form form = sender as ProgressNotes_Form;
            if (form.DialogResult == DialogResult.OK)
            {
                //gvwActionDetails.SelectedRows[0].Cells["gvtNotes"].Value = "Y";
                //gvwActionDetails.CurrentRow.Cells["gvtNotes"].Value = "captain-casenotes";
                gvwActionDetails.SelectedRows[0].Cells["gvtNotes"].Style.BackgroundImageSource = "captain-casenotes";
            }
        }

        private void Get_PROG_Notes_Status()
        {
            List<CaseNotesEntity> caseNotesEntity = new List<CaseNotesEntity>();

            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("CASE00061", Sel_Cont_Notes_Key);
            //gvwActionDetails.CurrentRow.Cells["gvtNotes"].Value = Img_CaseNotesAdd;
            gvwActionDetails.SelectedRows[0].Cells["gvtNotes"].Style.BackgroundImageSource = "captain-casenotesadd";

            if (caseNotesEntity.Count > 0)
            {
                //gvwActionDetails.CurrentRow.Cells["gvtNotes"].Value = Img_CaseNotesEdit;
                gvwActionDetails.SelectedRows[0].Cells["gvtNotes"].Style.BackgroundImageSource = "captain-casenotes";
            }
        }
    }
}