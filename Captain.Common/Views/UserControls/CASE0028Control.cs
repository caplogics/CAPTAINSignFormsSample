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
using System.Linq;
using Captain.Common.Views.Forms;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Utilities;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Wisej.Web;
using log4net.Repository.Hierarchy;
using static Captain.Common.Utilities.Consts;
using DevExpress.Utils;
#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class CASE0028Control : BaseUserControl
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private string strYear = "    ";
        private int strIndex = 0;

        #endregion
        public CASE0028Control(BaseForm baseForm, PrivilegeEntity privileges)
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

            ResultsList = _model.SPAdminData.Get_AgyRecs("Results");
            caseworkerfilling();
            lblApplicantNumber.Text = BaseForm.BaseApplicationNo;
            Refresh();
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

        List<SPCommonEntity> ResultsList = new List<SPCommonEntity>();
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
                       // Application.Navigate(CommonFunctions.BuildHelpURLS(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString()), target: "_blank");
                        Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(),"",""), target: "_blank");
                        break;
                }
                executeCode.Append(Consts.Javascript.EndJavascriptCode);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
            }
        }
        string propCasworker = string.Empty;
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
                    propCasworker = strCaseworkerDefault;
                }
            }
            cmbCaseworker.Items.Insert(0, new ListItem("", "0"));
            //cmbCaseworker.Items.Insert(1, new ListItem("No Caseworker Entered", "99"));
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
                DefaultGridLoad();
            }
        }

        string Img_Cross = Consts.Icons.ico_Cross;  //new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("cross.ico");
        string Img_Tick = Consts.Icons.ico_Tick; //new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");
        string Img_New = Application.MapPath("~\\Resources\\images\\Next1.ico");  //Consts.Icons.ico_CaseNotes_New;  //new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("Next1.ico");
        string Img_CaseNotesAdd = Consts.Icons.ico_CaseNotes_New;
        string Img_CaseNotesEdit = Consts.Icons.ico_CaseNotes_View;

        public void fillGriddata(bool boolStatus)
        {

            propMstList.Clear();


            if (gvwActionDetails.Rows.Count == 0)
            {
                DefaultGridLoad();
            }
            else
            {
                if (lblApplicantNumber.Text != string.Empty && lblApplicantNumber.Text != BaseForm.BaseApplicationNo)
                {
                    lblApplicantNumber.Text = BaseForm.BaseApplicationNo;
                    DefaultGridLoad();
                }
                else
                {

                    foreach (DataGridViewRow gvrowitem in gvwActionDetails.Rows)
                    {

                        string strItemDesc = gvrowitem.Cells["gvtScreenName"].Value.ToString();
                        string strFollowFromDate = gvrowitem.Cells["gvtFDate"].Value.ToString();
                        string strFollowToDate = gvrowitem.Cells["gvtTDate"].Value.ToString();
                        if (strItemDesc == "Client Intake")
                        {

                            gvrowitem.Cells["gvIStatus"].Value = Img_Cross;
                            if (BaseForm.BaseCaseMstListEntity[0].CompleteDate != string.Empty)
                            {
                                gvrowitem.Cells["gvIStatus"].Value = Img_Tick;
                            }
                            else
                            {
                                if (BaseForm.BaseCaseMstListEntity[0].CaseReviewDate != string.Empty)
                                {
                                    if (DateTime.Now.Date < Convert.ToDateTime(BaseForm.BaseCaseMstListEntity[0].CaseReviewDate))
                                    {
                                        if (Convert.ToDateTime(strFollowToDate) < Convert.ToDateTime(BaseForm.BaseCaseMstListEntity[0].CaseReviewDate))
                                            gvrowitem.Cells["gvIStatus"].Value = Img_New;
                                    }
                                }
                            }

                            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("CASE2001", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo);
                            if (caseNotesEntity.Count > 0)
                            {
                                gvrowitem.Cells["gvtNotes"].Value = Img_CaseNotesEdit;//"captain-casenotes";
                            }
                            else
                            {
                                gvrowitem.Cells["gvtNotes"].Value = Img_CaseNotesAdd;//"captain-casenotesadd";
                            }
                            caseNotesEntity = caseNotesEntity;
                        }
                        if (strItemDesc == "Income Verification")
                        {
                            List<CaseVerEntity> caseverdata = _model.CaseMstData.GetCASEVeradpyalst(BaseForm.BaseAgency.ToString().Trim(), BaseForm.BaseDept.ToString().Trim(), BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty, string.Empty);
                            //if (!((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("0"))
                            //{
                            //    if (((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("99"))
                            //        caseverdata = caseverdata.FindAll(u => u.Verifier.Trim() == string.Empty);
                            //    else
                            //        caseverdata = caseverdata.FindAll(u => u.Verifier.Trim() == ((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Trim());
                            //}
                            if (caseverdata.Count > 0)
                            {
                                //CaseVerEntity caseversingledate;
                                bool boolincomeverstatus = false;
                                bool boolincomeFutureverstatus = false;
                                if (caseverdata[0].ReverifyDate != string.Empty)
                                {
                                    if (Convert.ToDateTime(caseverdata[0].ReverifyDate) == Convert.ToDateTime(strFollowToDate))
                                    {
                                        boolincomeverstatus = false;
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(caseverdata[0].ReverifyDate) > Convert.ToDateTime(strFollowToDate))
                                        {
                                            if (Convert.ToDateTime(caseverdata[0].ReverifyDate) > DateTime.Now)
                                            {
                                                boolincomeverstatus = true;
                                                boolincomeFutureverstatus = true;
                                            }
                                            else
                                            {
                                                boolincomeverstatus = true;
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    if (Convert.ToDateTime(caseverdata[0].VerifyDate) > Convert.ToDateTime(strFollowFromDate))
                                    {
                                        boolincomeverstatus = true;
                                        if (Convert.ToDateTime(caseverdata[0].VerifyDate) > DateTime.Now)
                                        {

                                            boolincomeFutureverstatus = true;
                                        }
                                    }
                                }



                                //string strFDate = string.Empty;
                                //string strTDate = string.Empty;
                                //if (caseveralldata.Count > 0)
                                //{
                                //    strFDate = caseveralldata[0].VerifyDate;
                                //    strTDate = caseveralldata[0].ReverifyDate;
                                //    if (strTDate != string.Empty)
                                //    {
                                //        if (Convert.ToDateTime(caseverdata[0].ReverifyDate) > DateTime.Now.Date)
                                //        {
                                //            boolincomeverstatus = true;
                                //        }
                                //    }
                                //}
                                //else
                                //{
                                //    if (caseverdata.Count > 0)
                                //    {
                                //        CaseVerEntity casevereligdate = caseveralldata.Find(u => LookupDataAccess.Getdate(u.VerifyDate) == LookupDataAccess.Getdate(BaseForm.BaseCaseMstListEntity[0].EligDate));
                                //        if (casevereligdate != null)
                                //        {
                                //            strFDate = caseverdata[0].VerifyDate;
                                //            strTDate = caseverdata[0].ReverifyDate;
                                //            if (strTDate != string.Empty)
                                //            {
                                //                if (Convert.ToDateTime(caseverdata[0].ReverifyDate) > DateTime.Now.Date)
                                //                {
                                //                    boolincomeverstatus = true;
                                //                }
                                //            }
                                //        }
                                //    }
                                //}

                                gvrowitem.Cells["gvIStatus"].Value = Img_Cross;
                                if (boolincomeverstatus)
                                {
                                    if (boolincomeFutureverstatus)
                                    {
                                        gvrowitem.Cells["gvIStatus"].Value = Img_New;
                                    }
                                    else
                                    {
                                        gvrowitem.Cells["gvIStatus"].Value = Img_Tick;
                                    }
                                }

                            }
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
                        if (strItemDesc == "Service Details")
                        {
                            CASEACTEntity Search_Enty1 = gvrowitem.Cells["gvtScreenName"].Tag as CASEACTEntity;

                            CASEACTEntity Search_Enty = new CASEACTEntity(true);
                            Search_Enty.Agency = BaseForm.BaseAgency;
                            Search_Enty.Dept = BaseForm.BaseDept;
                            Search_Enty.Program = BaseForm.BaseProg;
                            Search_Enty.Year = BaseForm.BaseYear;
                            Search_Enty.App_no = lblApplicantNumber.Text.Trim();
                            Search_Enty.Service_plan = Search_Enty1.Service_plan;
                            Search_Enty.SPM_Seq = Search_Enty1.SPM_Seq;
                            Search_Enty.Branch = Search_Enty1.Branch;
                            Search_Enty.Group = Search_Enty1.Group;
                            Search_Enty.ACT_Code = Search_Enty1.ACT_Code;

                            List<CASEACTEntity> Sel_SP_Activity_Details = _model.SPAdminData.Browse_CASEACT(Search_Enty, "Browse");

                            Sel_SP_Activity_Details = Sel_SP_Activity_Details.FindAll(u => u.Followup_On != string.Empty);
                            Sel_SP_Activity_Details = Sel_SP_Activity_Details.FindAll(u => Convert.ToDateTime(u.Followup_On) >= dtStartDate.Value.Date && Convert.ToDateTime(u.Followup_On) <= dtEndDate.Value.Date);

                            if (!((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("0"))
                            {
                                if (((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("99"))
                                    Sel_SP_Activity_Details = Sel_SP_Activity_Details.FindAll(u => u.Caseworker.Trim() == string.Empty);
                                else
                                    Sel_SP_Activity_Details = Sel_SP_Activity_Details.FindAll(u => u.Caseworker.Trim() == ((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Trim());
                            }
                            Sel_SP_Activity_Details = Sel_SP_Activity_Details.OrderByDescending(u => Convert.ToDateTime(u.Followup_On)).ToList();
                            string strFDate = string.Empty;
                            string strTDate = string.Empty;
                            string strDesc = string.Empty;
                            if (Sel_SP_Activity_Details.Count > 0)
                            {

                                List<CAMASTEntity> CAMASTList = _model.SPAdminData.Browse_CAMAST("Code", Sel_SP_Activity_Details[0].ACT_Code, null, null);
                                if (CAMASTList.Count > 0)
                                    strDesc = CAMASTList[0].Desc;

                                strFDate = Sel_SP_Activity_Details[0].ACT_Date;
                                strTDate = Sel_SP_Activity_Details[0].Followup_On;

                                gvrowitem.Cells["gvIStatus"].Value = Img_Cross;
                                if (Sel_SP_Activity_Details[0].Followup_Comp != string.Empty)
                                {
                                    gvrowitem.Cells["gvIStatus"].Value = Img_Tick;
                                }
                                else
                                {
                                    if (Sel_SP_Activity_Details[0].Followup_On != string.Empty)
                                    {
                                        if (DateTime.Now.Date < Convert.ToDateTime(Sel_SP_Activity_Details[0].Followup_On))
                                        {
                                            if (Convert.ToDateTime(strFollowToDate) < Convert.ToDateTime(Sel_SP_Activity_Details[0].Followup_On))
                                                 gvrowitem.Cells["gvIStatus"].Value = Img_New;
                                        }
                                    }
                                }

                                Sel_CAMS_Notes_Key = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo + Sel_SP_Activity_Details[0].Service_plan.Trim() + Sel_SP_Activity_Details[0].SPM_Seq + Sel_SP_Activity_Details[0].Branch.Trim() + Sel_SP_Activity_Details[0].Group.ToString() + "CA" + Sel_SP_Activity_Details[0].ACT_Code.Trim() + Sel_SP_Activity_Details[0].ACT_Seq.Trim() + Sel_SP_Activity_Details[0].ACT_ID.Trim();

                                caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("CASE00063", Sel_CAMS_Notes_Key);

                                if (caseNotesEntity.Count > 0)
                                {
                                    gvrowitem.Cells["gvtNotes"].Value = Img_CaseNotesEdit;
                                }
                                else
                                {
                                    gvrowitem.Cells["gvtNotes"].Value = Img_CaseNotesAdd;
                                }
                                caseNotesEntity = caseNotesEntity;
                            }

                        }
                        if (strItemDesc == "Outcome Details")
                        {
                            CASEMSEntity Search_Enty1 = gvrowitem.Cells["gvtScreenName"].Tag as CASEMSEntity;
                            CASEMSEntity Search_Enty = new CASEMSEntity(true);
                            Search_Enty.Agency = BaseForm.BaseAgency;
                            Search_Enty.Dept = BaseForm.BaseDept;
                            Search_Enty.Program = BaseForm.BaseProg;
                            Search_Enty.Year = BaseForm.BaseYear;
                            Search_Enty.App_no = lblApplicantNumber.Text;                            
                            Search_Enty.Service_plan = Search_Enty1.Service_plan;
                            Search_Enty.SPM_Seq = Search_Enty1.SPM_Seq;
                            Search_Enty.Branch = Search_Enty1.Branch;
                            Search_Enty.Group = Search_Enty1.Group;
                            Search_Enty.MS_Code = Search_Enty1.MS_Code;

                            List<CASEMSEntity> Sel_SP_MS_Details = _model.SPAdminData.Browse_CASEMS(Search_Enty, "Browse");
                            Sel_SP_MS_Details = Sel_SP_MS_Details.FindAll(u => u.MS_FUP_Date != string.Empty);
                            Sel_SP_MS_Details = Sel_SP_MS_Details.FindAll(u => Convert.ToDateTime(u.MS_FUP_Date) >= dtStartDate.Value.Date && Convert.ToDateTime(u.MS_FUP_Date) <= dtEndDate.Value.Date);

                            if (!((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("0"))
                            {
                                if (((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("99"))
                                    Sel_SP_MS_Details = Sel_SP_MS_Details.FindAll(u => u.CaseWorker.Trim() == string.Empty);
                                else
                                    Sel_SP_MS_Details = Sel_SP_MS_Details.FindAll(u => u.CaseWorker.Trim() == ((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Trim());
                            }

                            Sel_SP_MS_Details = Sel_SP_MS_Details.OrderByDescending(u => Convert.ToDateTime(u.MS_FUP_Date)).ToList();

                            string strFDate = string.Empty;
                            string strTDate = string.Empty;
                            string strDesc = string.Empty;
                            if (Sel_SP_MS_Details.Count > 0)
                            {

                                List<MSMASTEntity> CAMASTList = _model.SPAdminData.Browse_MSMAST("Code", Sel_SP_MS_Details[0].MS_Code, null, null, null);
                                if (CAMASTList.Count > 0)
                                    strDesc = CAMASTList[0].Desc;

                                strFDate = Sel_SP_MS_Details[0].Date;
                                strTDate = Sel_SP_MS_Details[0].MS_FUP_Date;

                                gvrowitem.Cells["gvIStatus"].Value = Img_Cross;
                                if (Sel_SP_MS_Details[0].MS_Comp_Date != string.Empty)
                                {
                                    gvrowitem.Cells["gvIStatus"].Value = Img_Tick;
                                }
                                else
                                {
                                    if (Sel_SP_MS_Details[0].MS_FUP_Date != string.Empty)
                                    {
                                        if (DateTime.Now.Date < Convert.ToDateTime(Sel_SP_MS_Details[0].MS_FUP_Date))
                                        {
                                            if (Convert.ToDateTime(strFollowToDate) < Convert.ToDateTime(Sel_SP_MS_Details[0].MS_FUP_Date))
                                                gvrowitem.Cells["gvIStatus"].Value = Img_New;
                                        }
                                    }
                                }

                                Sel_CAMS_Notes_Key = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo + Sel_SP_MS_Details[0].Service_plan.Trim() + Sel_SP_MS_Details[0].SPM_Seq + Sel_SP_MS_Details[0].Branch.Trim() + Sel_SP_MS_Details[0].Group.ToString() + "MS" + Sel_SP_MS_Details[0].MS_Code.Trim() + Convert.ToDateTime(Sel_SP_MS_Details[0].Date).ToString("MM/dd/yyyy");

                                caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("CASE00064", Sel_CAMS_Notes_Key);
                                if (caseNotesEntity.Count > 0)
                                {
                                    gvrowitem.Cells["gvtNotes"].Value = Img_CaseNotesEdit;
                                }
                                else
                                {
                                    gvrowitem.Cells["gvtNotes"].Value = Img_CaseNotesAdd;
                                }
                                caseNotesEntity = caseNotesEntity;
                            }
                        }
                        if (strItemDesc == "Contact Details")
                        {
                            string strFDate = string.Empty;
                            string strTDate = string.Empty;
                            string strDesc = string.Empty;
                            CASECONTEntity Search_Enty1 = gvrowitem.Cells["gvtScreenName"].Tag as CASECONTEntity;
                            CASECONTEntity Cont_Search_Entity = new CASECONTEntity();
                            Cont_Search_Entity.Agency = BaseForm.BaseAgency;
                            Cont_Search_Entity.Dept = BaseForm.BaseDept;
                            Cont_Search_Entity.Program = BaseForm.BaseProg;
                            Cont_Search_Entity.App_no = lblApplicantNumber.Text;
                            Cont_Search_Entity.Contact_Name = Search_Enty1.Contact_Name;
                            Cont_Search_Entity.Year = "    ";
                            if (!string.IsNullOrEmpty(BaseForm.BaseYear))
                                Cont_Search_Entity.Year = BaseForm.BaseYear;

                            Cont_Search_Entity.Contact_No =  Cont_Search_Entity.CaseWorker = Cont_Search_Entity.Cont_Date = null;
                            Cont_Search_Entity.Duration_Type = Cont_Search_Entity.Time = Cont_Search_Entity.Time_Starts = Cont_Search_Entity.Time_Ends = null;
                            Cont_Search_Entity.Duration = Cont_Search_Entity.How_Where = null;
                            Cont_Search_Entity.Language = Cont_Search_Entity.Interpreter = Cont_Search_Entity.Refer_From = Cont_Search_Entity.BillTO = Cont_Search_Entity.BillTo_UOM = null;
                            Cont_Search_Entity.Cust1_Code = Cont_Search_Entity.Cust1_Value = Cont_Search_Entity.Cust2_Code = Cont_Search_Entity.Cust2_Value = Cont_Search_Entity.Cust3_Code = null;

                            Cont_Search_Entity.Cust3_Value = Cont_Search_Entity.Bridge = Cont_Search_Entity.Lsct_Operator = Cont_Search_Entity.Lstc_Date = Cont_Search_Entity.Add_Date = null;
                            Cont_Search_Entity.Add_Operator = null;

                            int TmpCount = 0, Cont_Sel_Index = 0;
                            List<CASECONTEntity> CASECONT_List = _model.SPAdminData.Browse_CASECONT(Cont_Search_Entity, "Browse");
                            CASECONT_List = CASECONT_List.FindAll(u => u.FollowuponDate != string.Empty);
                            if (!((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("0"))
                            {
                                if (((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("99"))
                                    CASECONT_List = CASECONT_List.FindAll(u => u.CaseWorker.Trim() == string.Empty);
                                else
                                    CASECONT_List = CASECONT_List.FindAll(u => u.CaseWorker.Trim() == ((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Trim());
                            }
                            CASECONT_List = CASECONT_List.FindAll(u => Convert.ToDateTime(u.FollowuponDate) >= dtStartDate.Value.Date && Convert.ToDateTime(u.FollowuponDate) <= dtEndDate.Value.Date);

                            CASECONT_List = CASECONT_List.OrderByDescending(u => Convert.ToDateTime(u.FollowuponDate)).ToList();
                            if (CASECONT_List.Count > 0)
                            {
                                strDesc = CASECONT_List[0].Contact_Name;
                                strFDate = CASECONT_List[0].Cont_Date;
                                strTDate = CASECONT_List[0].FollowuponDate;


                                gvrowitem.Cells["gvIStatus"].Value = Img_Cross;
                                if (CASECONT_List[0].FollowupCompleteDate != string.Empty)
                                {
                                    gvrowitem.Cells["gvIStatus"].Value = Img_Tick;
                                }
                                else
                                {
                                    if (CASECONT_List[0].FollowuponDate != string.Empty)
                                    {
                                        if (DateTime.Now.Date < Convert.ToDateTime(CASECONT_List[0].FollowuponDate))
                                        {
                                            if (Convert.ToDateTime(strFollowToDate) < Convert.ToDateTime(CASECONT_List[0].FollowuponDate))
                                                gvrowitem.Cells["gvIStatus"].Value = Img_New;
                                        }
                                    }
                                }

                                string Tmp_Seq = string.Empty;
                                if (!string.IsNullOrEmpty(CASECONT_List[0].Seq.ToString()))
                                    Tmp_Seq = CASECONT_List[0].Seq.ToString();
                                Sel_Cont_Notes_Key = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo + "0000".Substring(0, (4 - Tmp_Seq.Length)) + Tmp_Seq;

                                //Get_PROG_Notes_Status();

                                caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("CASE00061", Sel_Cont_Notes_Key);
                                if (caseNotesEntity.Count > 0)
                                {
                                    gvrowitem.Cells["gvtNotes"].Value = Img_CaseNotesEdit;
                                }
                                else
                                {
                                    gvrowitem.Cells["gvtNotes"].Value = Img_CaseNotesAdd;
                                }
                                caseNotesEntity = caseNotesEntity;
                            }

                        }
                        if (strItemDesc == "Matrix/Scales Assessments")
                        {
                            string strFDate = string.Empty;
                            string strTDate = string.Empty;
                            string strDesc = string.Empty;
                            List<MATAPDTSEntity> matapdtsList = _model.MatrixScalesData.GETMatapdts(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty, string.Empty, string.Empty);

                            matapdtsList = matapdtsList.FindAll(u => u.FOllowON != string.Empty);
                            matapdtsList = matapdtsList.FindAll(u => Convert.ToDateTime(u.FOllowON) >= dtStartDate.Value.Date && Convert.ToDateTime(u.FOllowON) <= dtEndDate.Value.Date);
                            if (!((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("0"))
                            {
                                if (((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("99"))
                                    matapdtsList = matapdtsList.FindAll(u => u.SSworker.Trim() == string.Empty);
                                else
                                    matapdtsList = matapdtsList.FindAll(u => u.SSworker.Trim() == ((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Trim());
                            }
                            gvrowitem.Cells["gvIStatus"].Value = Img_Cross;
                            if (matapdtsList[0].FollowCDate != string.Empty)
                            {
                                gvrowitem.Cells["gvIStatus"].Value = Img_Tick;
                            }
                            else
                            {
                                if (matapdtsList[0].FOllowON != string.Empty)
                                {
                                    if (DateTime.Now.Date < Convert.ToDateTime(matapdtsList[0].FOllowON))
                                    {
                                        if (Convert.ToDateTime(strFollowToDate) < Convert.ToDateTime(matapdtsList[0].FOllowON))
                                            gvrowitem.Cells["gvIStatus"].Value = Img_New;
                                    }
                                }
                            }

                            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("MAT00003", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo);
                            if (caseNotesEntity.Count > 0)
                            {
                                gvrowitem.Cells["gvtNotes"].Value = Img_CaseNotesEdit;
                            }
                            else
                            {
                                gvrowitem.Cells["gvtNotes"].Value = Img_CaseNotesAdd;
                            }
                            caseNotesEntity = caseNotesEntity;

                        }


                    }
                }
            }
        }

        public void DefaultGridLoad()
        {
            string strScreencode = string.Empty;
            if (rdoAllScreen.Checked)
                strScreencode = string.Empty;
            else
                strScreencode = propScreencode;
            string strCaseworker = string.Empty;

            if (!((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("0"))
            {
                strCaseworker = ((ListItem)cmbCaseworker.SelectedItem).Value.ToString();
            }

            if (!((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("0"))
            {
                strCaseworker = ((ListItem)cmbCaseworker.SelectedItem).Value.ToString();
            }
            gvwActionDetails.SelectionChanged -= new EventHandler(gvwActionDetails_SelectionChanged);
            gvwActionDetails.Rows.Clear();
            DataSet dslist = Captain.DatabaseLayer.SPAdminDB.CASE0027_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, dtStartDate.Value.ToShortDateString(), dtEndDate.Value.ToShortDateString(), strCaseworker, strScreencode, BaseForm.BaseApplicationNo);
            if (dslist.Tables.Count > 1)
            {

                foreach (DataRow drmstitem in dslist.Tables[1].Rows)
                {
                    propMstList.Add(new Model.Objects.CaseMstEntity(drmstitem, "CASE0027"));

                }
            }

            if (dslist.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drmstitem in dslist.Tables[0].Rows)
                {
                    ShowFollowDetails(drmstitem, false);
                }
            }
            else
            {
                CommonFunctions.MessageBoxDisplay("No records found");
            }
            gvwActionDetails.SelectionChanged += new EventHandler(gvwActionDetails_SelectionChanged);
            gvwActionDetails_SelectionChanged(gvwActionDetails, new EventArgs());
        }

        public void Refresh()
        {
            if (BaseForm.BaseClientFolloupFromDate != string.Empty)
            {
                dtStartDate.Value = Convert.ToDateTime(BaseForm.BaseClientFolloupFromDate);
                dtEndDate.Value = Convert.ToDateTime(BaseForm.BaseClientFolloupToDate);

                if (BaseForm.BaseClientFolloupWorker != string.Empty)
                    CommonFunctions.SetComboBoxValue(cmbCaseworker, BaseForm.BaseClientFolloupWorker);
                else
                {
                    if (propCasworker != string.Empty)
                        CommonFunctions.SetComboBoxValue(cmbCaseworker, propCasworker);
                    else
                        cmbCaseworker.SelectedIndex = 0;
                }
                if (BaseForm.BaseClientFolloupScreentype == "A")
                {
                    rdoAllScreen.Checked = true;
                    propScreencode = string.Empty;
                    btnSel.Enabled = false;
                }
                else
                {
                    btnSel.Enabled = true;
                    rdoSelect.Checked = true;
                    propScreencode = BaseForm.BaseClientFolloupScreencode;
                }
            }
            else
            {
                dtStartDate.Value = DateTime.Now.AddDays(-7);
               // dtEndDate.Value = DateTime.Now;
            }

            fillGriddata(false);
        }

        private bool Validate_Report()
        {
            bool Can_Generate = true;
            _errorProvider.SetError(cmbCaseworker, null);
            _errorProvider.SetError(dtStartDate, null);
            _errorProvider.SetError(dtEndDate, null);

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
                _errorProvider.SetError(dtStartDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblForm.Text.Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(dtStartDate, null);

            if (!dtEndDate.Checked)
            {
                _errorProvider.SetError(dtEndDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Follow - up On - " + lblTo.Text.Replace(Consts.Common.Colon, string.Empty)));
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
                    _errorProvider.SetError(dtStartDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Start Date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(dtStartDate, null);
                }
                if (string.IsNullOrWhiteSpace(dtEndDate.Text))
                {
                    _errorProvider.SetError(dtEndDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "End Date".Replace(Consts.Common.Colon, string.Empty)));
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
                    else
                    {
                        if (dtEndDate.Checked && dtStartDate.Checked)
                            _errorProvider.SetError(dtStartDate, null);
                    }
                }
            }

            return Can_Generate;
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

        private void ShowFollowDetails(DataRow dr, bool boolStatus)
        {

            int introwindex = 0;
            bool boolNewStatus = false;
            if (dr != null)
            {
                List<CaseMstEntity> mstdata = propMstList.FindAll(u => u.ApplAgency.Trim() == dr["MST_AGENCY"].ToString().Trim() && u.ApplDept.Trim() == dr["MST_DEPT"].ToString().Trim() && u.ApplProgram.Trim() == dr["MST_PROGRAM"].ToString().Trim() && u.ApplYr.Trim() == dr["MST_YEAR"].ToString().Trim() && u.ApplNo.Trim() == dr["SNP_APP"].ToString().Trim());

                if (mstdata.FindAll(u => u.Mode == "MST").Count > 0)
                {
                    introwindex = gvwActionDetails.Rows.Add("Client Intake", "Case Review Date", LookupDataAccess.Getdate(mstdata[0].IntakeDate), LookupDataAccess.Getdate(mstdata[0].CaseReviewDate), Img_Cross, string.Empty, Img_CaseNotesAdd);
                    gvwActionDetails.Rows[introwindex].Tag = "MST";
                    gvwActionDetails.Rows[introwindex].Cells["gvtDesc"].Tag = string.Empty;

                    if (boolStatus)
                    {
                        if (BaseForm.BaseCaseMstListEntity[0].CompleteDate != string.Empty)
                        {
                            gvwActionDetails.Rows[introwindex].Cells["gvIStatus"].Value = Img_Tick;
                        }
                        else
                        {

                        }
                    }

                    caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("CASE2001", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo);
                    if (caseNotesEntity.Count > 0)
                    {
                        gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesEdit;//"captain-casenotes";
                    }
                    else
                    {
                        gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesAdd;//"captain-casenotesadd";
                    }
                    caseNotesEntity = caseNotesEntity;


                }
                if (mstdata.FindAll(u => u.Mode == "VER").Count > 0)
                {
                    List<CaseVerEntity> caseverdata = _model.CaseMstData.GetCASEVeradpyalst(dr["MST_AGENCY"].ToString().Trim(), dr["MST_DEPT"].ToString().Trim(), dr["MST_PROGRAM"].ToString().Trim(), dr["MST_YEAR"].ToString().Trim(), dr["SNP_APP"].ToString().Trim(), string.Empty, string.Empty);

                    if (caseverdata.Count > 0)
                    {
                        //CaseVerEntity caseversingledate;
                        bool boolincomeverstatus = false;
                        //if (caseverdata[0].ReverifyDate != string.Empty)
                        //{
                        //    if (Convert.ToDateTime(caseverdata[0].ReverifyDate) >= DateTime.Now.Date)
                        //    {
                        //        boolincomeverstatus = true;
                        //    }

                        //}
                        //else
                        //{
                        //    if (Convert.ToDateTime(caseverdata[0].VerifyDate) >= DateTime.Now.Date)
                        //    {
                        //        boolincomeverstatus = true;
                        //    }
                        //}

                        List<CaseVerEntity> caseveralldata = caseverdata.FindAll(u => u.ReverifyDate != string.Empty);
                        caseveralldata = caseveralldata.FindAll(u => LookupDataAccess.Getdate(u.ReverifyDate) == LookupDataAccess.Getdate(mstdata.FindAll(ui => ui.Mode == "VER")[0].ReverifyDate));

                        if (!((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("0"))
                        {
                            if (((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("99"))
                                caseveralldata = caseveralldata.FindAll(u => u.Verifier.Trim() == string.Empty);
                            else
                                caseveralldata = caseveralldata.FindAll(u => u.Verifier.Trim() == ((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Trim());
                        }
                        string strFDate = string.Empty;
                        string strTDate = string.Empty;
                        if (caseveralldata.Count > 0)
                        {
                            strFDate = caseveralldata[0].VerifyDate;
                            strTDate = caseveralldata[0].ReverifyDate;
                            if (strTDate != string.Empty)
                            {
                                if (Convert.ToDateTime(caseverdata[0].ReverifyDate) > DateTime.Now.Date)
                                {
                                    boolincomeverstatus = true;
                                }
                            }
                        }
                        else
                        {
                            if (caseverdata.Count > 0)
                            {
                                CaseVerEntity casevereligdate = caseveralldata.Find(u => LookupDataAccess.Getdate(u.VerifyDate) == LookupDataAccess.Getdate(BaseForm.BaseCaseMstListEntity[0].EligDate));
                                if (casevereligdate != null)
                                {
                                    strFDate = caseverdata[0].VerifyDate;
                                    strTDate = caseverdata[0].ReverifyDate;
                                    if (strTDate != string.Empty)
                                    {
                                        if (Convert.ToDateTime(caseverdata[0].ReverifyDate) > DateTime.Now.Date)
                                        {
                                            boolincomeverstatus = true;
                                        }
                                    }
                                }
                            }
                        }
                        introwindex = gvwActionDetails.Rows.Add("Income Verification", "Reverification Date", LookupDataAccess.Getdate(strFDate), LookupDataAccess.Getdate(strTDate), Img_Cross, string.Empty, "");
                        gvwActionDetails.Rows[introwindex].Tag = "VER";
                        gvwActionDetails.Rows[introwindex].Cells["gvtDesc"].Tag = string.Empty;
                        if (boolStatus)
                        {
                            if (boolincomeverstatus)
                            {
                                gvwActionDetails.Rows[introwindex].Cells["gvIStatus"].Value = Img_Tick;
                            }
                        }

                        //caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo);
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
                    Search_Enty.Agency = BaseForm.BaseAgency;
                    Search_Enty.Dept = BaseForm.BaseDept;
                    Search_Enty.Program = BaseForm.BaseProg;
                    Search_Enty.Year = BaseForm.BaseYear;
                    Search_Enty.App_no = dr["SNP_APP"].ToString().Trim();


                    List<CASEACTEntity> Sel_SP_Activity_Details = _model.SPAdminData.Browse_CASEACT(Search_Enty, "Browse");

                    Sel_SP_Activity_Details = Sel_SP_Activity_Details.FindAll(u => u.Followup_On != string.Empty);
                    Sel_SP_Activity_Details = Sel_SP_Activity_Details.FindAll(u => Convert.ToDateTime(u.Followup_On) >= dtStartDate.Value.Date && Convert.ToDateTime(u.Followup_On) <= dtEndDate.Value.Date);

                    if (!((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("0"))
                    {
                        if (((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("99"))
                            Sel_SP_Activity_Details = Sel_SP_Activity_Details.FindAll(u => u.Caseworker.Trim() == string.Empty);
                        else
                            Sel_SP_Activity_Details = Sel_SP_Activity_Details.FindAll(u => u.Caseworker.Trim() == ((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Trim());
                    }
                    Sel_SP_Activity_Details = Sel_SP_Activity_Details.OrderByDescending(u => Convert.ToDateTime(u.Followup_On)).ToList();
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
                            if (Actitem.Followup_Comp == string.Empty)
                            {
                                introwindex = gvwActionDetails.Rows.Add("Service Details", strDesc, LookupDataAccess.Getdate(strFDate), LookupDataAccess.Getdate(strTDate), Img_Cross, string.Empty, Img_CaseNotesAdd);
                                gvwActionDetails.Rows[introwindex].Tag = "ACT";
                                gvwActionDetails.Rows[introwindex].Cells["gvtDesc"].Tag = Actitem.Service_plan;
                                gvwActionDetails.Rows[introwindex].Cells["gvtScreenName"].Tag = Actitem;
                                if (boolStatus)
                                {
                                    if (Sel_SP_Activity_Details[0].Followup_Comp != string.Empty)
                                    {
                                        gvwActionDetails.Rows[introwindex].Cells["gvIStatus"].Value = Img_Tick;
                                    }
                                }

                                // CASEACTEntity Entity = gvwActionDetails.Rows[0].Cells["gvtScreenName"].Tag as CASEACTEntity;
                                
                                Sel_CAMS_Notes_Key = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo + Actitem.Service_plan.Trim() + Actitem.SPM_Seq + Actitem.Branch.Trim() + Actitem.Group.ToString() + "CA" + Actitem.ACT_Code.Trim() + Actitem.ACT_Seq.Trim() + Actitem.ACT_ID.Trim();

                                caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("CASE00063", Sel_CAMS_Notes_Key);

                                if (caseNotesEntity.Count > 0)
                                {
                                    gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesEdit;//"captain-casenotes";
                                }
                                else
                                {
                                    gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesAdd;//"captain-casenotesadd";
                                }
                                caseNotesEntity = caseNotesEntity;

                            }
                        }
                    }

                }
                if (mstdata.FindAll(u => u.Mode == "MS").Count > 0)
                {
                    CASEMSEntity Search_Enty = new CASEMSEntity(true);
                    Search_Enty.Agency = BaseForm.BaseAgency;
                    Search_Enty.Dept = BaseForm.BaseDept;
                    Search_Enty.Program = BaseForm.BaseProg;
                    Search_Enty.Year = BaseForm.BaseYear;
                    Search_Enty.App_no = dr["SNP_APP"].ToString().Trim();


                    List<CASEMSEntity> Sel_SP_MS_Details = _model.SPAdminData.Browse_CASEMS(Search_Enty, "Browse");
                    Sel_SP_MS_Details = Sel_SP_MS_Details.FindAll(u => u.MS_FUP_Date != string.Empty);
                    Sel_SP_MS_Details = Sel_SP_MS_Details.FindAll(u => Convert.ToDateTime(u.MS_FUP_Date) >= dtStartDate.Value.Date && Convert.ToDateTime(u.MS_FUP_Date) <= dtEndDate.Value.Date);

                    if (!((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("0"))
                    {
                        if (((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("99"))
                            Sel_SP_MS_Details = Sel_SP_MS_Details.FindAll(u => u.CaseWorker.Trim() == string.Empty);
                        else
                            Sel_SP_MS_Details = Sel_SP_MS_Details.FindAll(u => u.CaseWorker.Trim() == ((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Trim());
                    }

                    Sel_SP_MS_Details = Sel_SP_MS_Details.OrderByDescending(u => Convert.ToDateTime(u.MS_FUP_Date)).ToList();

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

                            if (Msitem.MS_Comp_Date == string.Empty)
                            {
                                introwindex = gvwActionDetails.Rows.Add("Outcome Details", strDesc, LookupDataAccess.Getdate(strFDate), LookupDataAccess.Getdate(strTDate), Img_Cross, string.Empty, Img_CaseNotesAdd);
                                gvwActionDetails.Rows[introwindex].Tag = "MS";
                                gvwActionDetails.Rows[introwindex].Cells["gvtDesc"].Tag = Msitem.Service_plan;
                                gvwActionDetails.Rows[introwindex].Cells["gvtResult"].Value = Msitem.Result;
                                gvwActionDetails.Rows[introwindex].Cells["gvtScreenName"].Tag = Msitem;
                                if (boolStatus)
                                {
                                    if (Msitem.MS_Comp_Date != string.Empty)
                                    {
                                        gvwActionDetails.Rows[introwindex].Cells["gvIStatus"].Value = Img_Tick;
                                    }
                                }


                                Sel_CAMS_Notes_Key = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo + Msitem.Service_plan.Trim() + Msitem.SPM_Seq + Msitem.Branch.Trim() + Msitem.Group.ToString() + "MS" + Msitem.MS_Code.Trim() + Convert.ToDateTime(Msitem.Date).ToString("MM/dd/yyyy");

                                caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("CASE00064", Sel_CAMS_Notes_Key);
                                if (caseNotesEntity.Count > 0)
                                {
                                    gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesEdit;//"captain-casenotes";
                                }
                                else
                                {
                                    gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesAdd;//"captain-casenotesadd";
                                }
                                caseNotesEntity = caseNotesEntity;

                            }
                        }
                    }
                }
                if (mstdata.FindAll(u => u.Mode == "CON").Count > 0)
                {
                    string strFDate = string.Empty;
                    string strTDate = string.Empty;
                    string strDesc = string.Empty;
                    CASECONTEntity Cont_Search_Entity = new CASECONTEntity();
                    Cont_Search_Entity.Agency = BaseForm.BaseAgency;
                    Cont_Search_Entity.Dept = BaseForm.BaseDept;
                    Cont_Search_Entity.Program = BaseForm.BaseProg;
                    Cont_Search_Entity.App_no = dr["SNP_APP"].ToString().Trim();

                    Cont_Search_Entity.Year = "    ";
                    if (!string.IsNullOrEmpty(BaseForm.BaseYear))
                        Cont_Search_Entity.Year = BaseForm.BaseYear;

                    Cont_Search_Entity.Contact_No = Cont_Search_Entity.Contact_Name = Cont_Search_Entity.CaseWorker = Cont_Search_Entity.Cont_Date = null;
                    Cont_Search_Entity.Duration_Type = Cont_Search_Entity.Time = Cont_Search_Entity.Time_Starts = Cont_Search_Entity.Time_Ends = null;
                    Cont_Search_Entity.Duration = Cont_Search_Entity.How_Where = null;
                    Cont_Search_Entity.Language = Cont_Search_Entity.Interpreter = Cont_Search_Entity.Refer_From = Cont_Search_Entity.BillTO = Cont_Search_Entity.BillTo_UOM = null;
                    Cont_Search_Entity.Cust1_Code = Cont_Search_Entity.Cust1_Value = Cont_Search_Entity.Cust2_Code = Cont_Search_Entity.Cust2_Value = Cont_Search_Entity.Cust3_Code = null;

                    Cont_Search_Entity.Cust3_Value = Cont_Search_Entity.Bridge = Cont_Search_Entity.Lsct_Operator = Cont_Search_Entity.Lstc_Date = Cont_Search_Entity.Add_Date = null;
                    Cont_Search_Entity.Add_Operator = null;

                    int TmpCount = 0, Cont_Sel_Index = 0;
                    List<CASECONTEntity> CASECONT_List = _model.SPAdminData.Browse_CASECONT(Cont_Search_Entity, "Browse");
                    CASECONT_List = CASECONT_List.FindAll(u => u.FollowuponDate != string.Empty);
                    if (!((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("0"))
                    {
                        if (((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Equals("99"))
                            CASECONT_List = CASECONT_List.FindAll(u => u.CaseWorker.Trim() == string.Empty);
                        else
                            CASECONT_List = CASECONT_List.FindAll(u => u.CaseWorker.Trim() == ((ListItem)cmbCaseworker.SelectedItem).Value.ToString().Trim());
                    }
                    CASECONT_List = CASECONT_List.FindAll(u => Convert.ToDateTime(u.FollowuponDate) >= dtStartDate.Value.Date  && Convert.ToDateTime(u.FollowuponDate) <= dtEndDate.Value.Date);

                    CASECONT_List = CASECONT_List.OrderByDescending(u => Convert.ToDateTime(u.FollowuponDate)).ToList();
                    foreach (CASECONTEntity contactitem in CASECONT_List)
                    {
                        if (contactitem.FollowupCompleteDate == string.Empty)
                        {
                            if (CASECONT_List.Count > 0)
                            {
                                strDesc = contactitem.Contact_Name;
                                strFDate = contactitem.Cont_Date;
                                strTDate = contactitem.FollowuponDate;
                            }
                            introwindex = gvwActionDetails.Rows.Add("Contact Details", strDesc, LookupDataAccess.Getdate(strFDate), LookupDataAccess.Getdate(strTDate), Img_Cross, string.Empty, Img_CaseNotesAdd, contactitem.Seq);

                            gvwActionDetails.Rows[introwindex].Tag = "CON";
                            gvwActionDetails.Rows[introwindex].Cells["gvtDesc"].Tag = string.Empty;
                            gvwActionDetails.Rows[introwindex].Cells["gvtScreenName"].Tag = contactitem;
                            if (boolStatus)
                            {
                                if (contactitem.FollowupCompleteDate != string.Empty)
                                {
                                    gvwActionDetails.Rows[introwindex].Cells["gvIStatus"].Value = Img_Tick;
                                }
                            }

                            string Tmp_Seq = string.Empty;
                            if (!string.IsNullOrEmpty(contactitem.Seq.ToString()))
                                Tmp_Seq = contactitem.Seq.ToString();
                            Sel_Cont_Notes_Key = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo + "0000".Substring(0, (4 - Tmp_Seq.Length)) + Tmp_Seq;

                            //Get_PROG_Notes_Status();

                            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("CASE00061", Sel_Cont_Notes_Key);
                            if (caseNotesEntity.Count > 0)
                            {
                                gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesEdit;//"captain-casenotes";
                            }
                            else
                            {
                                gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesAdd;//"captain-casenotesadd";
                            }
                            caseNotesEntity = caseNotesEntity;

                        }
                    }

                }
                if (mstdata.FindAll(u => u.Mode == "MAT").Count > 0)
                {
                    string strFDate = string.Empty;
                    string strTDate = string.Empty;
                    string strDesc = string.Empty;
                    List<MATAPDTSEntity> matapdtsList = _model.MatrixScalesData.GETMatapdts(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, dr["SNP_APP"].ToString().Trim(), string.Empty, string.Empty, string.Empty);

                    matapdtsList = matapdtsList.FindAll(u => u.FOllowON != string.Empty);
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

                    introwindex = gvwActionDetails.Rows.Add("Matrix/Scales Assessments", strDesc, LookupDataAccess.Getdate(strFDate), LookupDataAccess.Getdate(strTDate), Img_Cross, string.Empty, Img_CaseNotesAdd);
                    gvwActionDetails.Rows[introwindex].Tag = "MAT";
                    gvwActionDetails.Rows[introwindex].Cells["gvtDesc"].Tag = string.Empty;
                    if (boolStatus)
                    {
                        if (matapdtsList[0].FollowCDate != string.Empty)
                        {
                            gvwActionDetails.Rows[introwindex].Cells["gvIStatus"].Value = Img_Tick;
                        }
                    }

                    caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("MAT00003", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo);
                    if (caseNotesEntity.Count > 0)
                    {
                        gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesEdit;//"captain-casenotes";
                    }
                    else
                    {
                        gvwActionDetails.Rows[introwindex].Cells["gvtNotes"].Value = Img_CaseNotesAdd;//"captain-casenotesadd";
                    }
                    caseNotesEntity = caseNotesEntity;

                }

            }
            if (gvwActionDetails.Rows.Count>0)
                gvwActionDetails.Rows[0].Selected= true;
        } 

        private void gvwActionDetails_SelectionChanged(object sender, EventArgs e)
        {
            lblDate.Text = lblMs1.Text = lblMS2.Text = lblMs3.Text = lblMs4.Text = lblMs5.Text = lblMs6.Text = lblServicedate.Text = lblServiceDate1.Text = lblName.Text = string.Empty;
            lblDate.Visible = lblMs1.Visible = lblMS2.Visible = lblMs3.Visible = lblMs4.Visible = lblMs5.Visible = lblMs6.Visible = lblServicedate.Visible = lblServiceDate1.Visible = lblName.Visible = false;
            if (gvwActionDetails.Rows.Count > 0)
            {
                CASESPMEntity Search_Entity = new CASESPMEntity(true);
                string strTabletype = gvwActionDetails.SelectedRows[0].Tag as string;
                switch (strTabletype)
                {
                    case "MST":
                        lblName.Visible= lblDate.Visible =true;
                        lblName.Text = "Intake Date";
                        lblDate.Text = gvwActionDetails.SelectedRows[0].Cells["gvtFDate"].Value.ToString();
                        break;
                    case "VER":
                        lblName.Visible = lblDate.Visible = true;
                        lblName.Text = "Verification Date";
                        lblDate.Text = gvwActionDetails.SelectedRows[0].Cells["gvtFDate"].Value.ToString();
                        break;
                    case "ACT":


                        Search_Entity.agency = BaseForm.BaseAgency.ToString().Trim();
                        Search_Entity.dept = BaseForm.BaseDept.ToString();
                        Search_Entity.program = BaseForm.BaseProg.ToString();
                        Search_Entity.year = BaseForm.BaseYear.ToString().Trim();
                        Search_Entity.app_no = BaseForm.BaseApplicationNo.ToString().Trim();
                        Search_Entity.service_plan = gvwActionDetails.SelectedRows[0].Cells["gvtDesc"].Tag as string;


                        List<CASESPMEntity> CASESPM_List = _model.SPAdminData.Browse_CASESPM(Search_Entity, "Browse");
                        string strFDate = string.Empty;
                        string strspmdesc = string.Empty;
                        if (CASESPM_List.Count > 0)
                        {
                            strFDate = CASESPM_List[0].startdate;
                            strspmdesc = CASESPM_List[0].Sp0_Desc;
                        }
                        lblName.Visible = lblDate.Visible = lblServicedate.Visible = lblServiceDate1.Visible = lblMs1.Visible = lblMS2.Visible = lblMs3.Visible = lblMs4.Visible = true;
                        lblName.Text = "Service Plan Description";
                        lblDate.Text = strspmdesc;
                        lblServicedate.Text = "Service Plan Start Date";
                        lblServiceDate1.Text = LookupDataAccess.Getdate(strFDate);
                        lblMs1.Text = "Service Description";
                        lblMS2.Text = gvwActionDetails.SelectedRows[0].Cells["gvtDesc"].Value.ToString();
                        lblMs3.Text = "Service Date";
                        lblMs4.Text = gvwActionDetails.SelectedRows[0].Cells["gvtFDate"].Value.ToString();
                        break;
                    case "MS":


                        Search_Entity.agency = BaseForm.BaseAgency.ToString().Trim();
                        Search_Entity.dept = BaseForm.BaseDept.ToString();
                        Search_Entity.program = BaseForm.BaseProg.ToString();
                        Search_Entity.year = BaseForm.BaseYear.ToString().Trim();
                        Search_Entity.app_no = BaseForm.BaseApplicationNo.ToString().Trim();
                        Search_Entity.service_plan = gvwActionDetails.SelectedRows[0].Cells["gvtDesc"].Tag as string;


                        List<CASESPMEntity> CASESPM_List1 = _model.SPAdminData.Browse_CASESPM(Search_Entity, "Browse");
                        strFDate = string.Empty;
                        strspmdesc = string.Empty;
                        if (CASESPM_List1.Count > 0)
                        {
                            strFDate = CASESPM_List1[0].startdate;
                            strspmdesc = CASESPM_List1[0].Sp0_Desc;
                        }
                        lblName.Visible = lblDate.Visible = lblServicedate.Visible = lblServiceDate1.Visible = lblMs1.Visible = lblMS2.Visible = lblMs3.Visible = lblMs4.Visible = true;
                        lblName.Text = "Service Plan Description";
                        lblDate.Text = strspmdesc;
                        lblServicedate.Text = "Service Plan Start Date";
                        lblServiceDate1.Text = LookupDataAccess.Getdate(strFDate);
                        lblMs1.Text = "Outcome Description";
                        lblMS2.Text = gvwActionDetails.SelectedRows[0].Cells["gvtDesc"].Value.ToString();
                        lblMs3.Text = "Outcome Date";
                        lblMs4.Text = gvwActionDetails.SelectedRows[0].Cells["gvtFDate"].Value.ToString();
                        if (ResultsList.Count > 0)
                        {
                            string strresult = gvwActionDetails.SelectedRows[0].Cells["gvtResult"].Value == null ? string.Empty : gvwActionDetails.SelectedRows[0].Cells["gvtResult"].Value.ToString();
                            lblMs5.Visible = true;
                            lblMs5.Text = "Result";
                            SPCommonEntity commonresult = ResultsList.Find(u => u.Code.Trim() == strresult);
                            if (commonresult != null)
                            {
                                lblMs6.Visible = true;
                                lblMs6.Text = commonresult.Desc;
                            }
                        }
                        break;
                    case "CON":
                        lblName.Visible = lblDate.Visible = lblServicedate.Visible = lblServiceDate1.Visible =  true;
                        lblName.Text = "Contact Name";
                        lblDate.Text = gvwActionDetails.SelectedRows[0].Cells["gvtDesc"].Value.ToString();
                        lblServicedate.Text = "Contact Date";
                        lblServiceDate1.Text = gvwActionDetails.SelectedRows[0].Cells["gvtFDate"].Value.ToString();
                        break;
                    case "MAT":
                        lblName.Visible = lblDate.Visible = lblServicedate.Visible = lblServiceDate1.Visible = true;
                        lblName.Text = "Matrix";
                        lblDate.Text = gvwActionDetails.SelectedRows[0].Cells["gvtDesc"].Value.ToString();
                        lblServicedate.Text = "Assessment Date";
                        lblServiceDate1.Text = gvwActionDetails.SelectedRows[0].Cells["gvtFDate"].Value.ToString();
                        break;

                }

            }
        }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (Validate_Report())
            {
                fillGriddata(true);
            }
        }
        public List<CaseNotesEntity> caseNotesEntity
        {
            get; set;
        }

        string Sel_SPM2_Notes_Key = string.Empty;
        string Sel_CAMS_Notes_Key = string.Empty;
        string Sel_Cont_Notes_Key = string.Empty;
        string strItemDesc = string.Empty;
        private void gvwActionDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex == gvtNotes.Index)
            {
                strItemDesc = gvwActionDetails.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();

                if (strItemDesc == "Client Intake")
                {
                    caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("CASE2001", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo);
                    CaseNotes caseNotes = new CaseNotes(BaseForm, Privileges, caseNotesEntity, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo, "CASE2001", BaseForm.BaseApplicationNo, BaseForm.BaseApplicationName);
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

                    Sel_CAMS_Notes_Key = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo + Entity.Service_plan.Trim() + Entity.SPM_Seq + Entity.Branch.Trim() + Entity.Group.ToString() + "CA" + Entity.ACT_Code.Trim() + Entity.ACT_Seq.Trim() + Entity.ACT_ID.Trim();
                    ProgressNotes_Form Prog_Form = new ProgressNotes_Form(BaseForm, "Edit", Privileges, Sel_CAMS_Notes_Key, "CASE0063", BaseForm.BaseApplicationNo,BaseForm.BaseApplicationName);
                    Prog_Form.FormClosed += new FormClosedEventHandler(On_PROGNOTES_Closed);
                    Prog_Form.StartPosition = FormStartPosition.CenterScreen;
                    Prog_Form.ShowDialog();
                }

                if (strItemDesc == "Outcome Details")
                {
                    CASEMSEntity Entity = gvwActionDetails.SelectedRows[0].Cells["gvtScreenName"].Tag as CASEMSEntity;

                    Sel_CAMS_Notes_Key = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo + Entity.Service_plan.Trim() + Entity.SPM_Seq + Entity.Branch.Trim() + Entity.Group.ToString() + "MS" + Entity.MS_Code.Trim() + Convert.ToDateTime(Entity.Date).ToString("MM/dd/yyyy");
                    ProgressNotes_Form Prog_Form = new ProgressNotes_Form(BaseForm, "Edit", Privileges, Sel_CAMS_Notes_Key, "CASE0064", BaseForm.BaseApplicationNo, BaseForm.BaseApplicationName);
                    Prog_Form.FormClosed += new FormClosedEventHandler(On_PROGNOTES_Closed);
                    Prog_Form.StartPosition = FormStartPosition.CenterScreen;
                    Prog_Form.ShowDialog();
                }

                if (strItemDesc == "Contact Details")
                {
                    string Tmp_Seq = string.Empty;
                    if (!string.IsNullOrEmpty(gvwActionDetails.CurrentRow.Cells["Cont_Seq"].Value.ToString().Trim()))
                        Tmp_Seq = gvwActionDetails.CurrentRow.Cells["Cont_Seq"].Value.ToString();
                    Sel_Cont_Notes_Key = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo + "0000".Substring(0, (4 - Tmp_Seq.Length)) + Tmp_Seq;

                    Get_PROG_Notes_Status();
                    ProgressNotes_Form Prog_Form = new ProgressNotes_Form(BaseForm, "Edit", Privileges, Sel_Cont_Notes_Key, "CONT", "CASE00061", BaseForm.BaseApplicationNo, BaseForm.BaseApplicationName);
                    Prog_Form.FormClosed += new FormClosedEventHandler(On_PROGNOTES_Closed);
                    Prog_Form.StartPosition = FormStartPosition.CenterScreen;
                    Prog_Form.ShowDialog();
                }

                if (strItemDesc == "Matrix/Scales Assessments")
                {
                    caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("MAT00003", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo);
                    CaseNotes caseNotes = new CaseNotes(BaseForm, Privileges, caseNotesEntity, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo, "MAT00003", BaseForm.BaseApplicationNo, BaseForm.BaseApplicationName);
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
                caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("MAT00003", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo);
            }
            if (strItemDesc == "Client Intake")
            {
                caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("CASE2001", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo);
            }
            if (caseNotesEntity.Count > 0)
            {
                gvwActionDetails.CurrentRow.Cells["gvtNotes"].Value = "captain-casenotes";
            }
            else
            {
                gvwActionDetails.CurrentRow.Cells["gvtNotes"].Value = "captain-casenotesadd";
            }
            caseNotesEntity = caseNotesEntity;
        }

        private void On_PROGNOTES_Closed(object sender, FormClosedEventArgs e)
        {
            ProgressNotes_Form form = sender as ProgressNotes_Form;
            if (form.DialogResult == DialogResult.OK)
            {
                gvwActionDetails.SelectedRows[0].Cells["gvtNotes"].Value = "Y";
                gvwActionDetails.CurrentRow.Cells["gvtNotes"].Value = "captain-casenotes";
            }
        }

        private void Get_PROG_Notes_Status()
        {
            List<CaseNotesEntity> caseNotesEntity = new List<CaseNotesEntity>();

            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("CASE00061", Sel_Cont_Notes_Key);
            gvwActionDetails.CurrentRow.Cells["gvtNotes"].Value = Img_CaseNotesAdd;

            if (caseNotesEntity.Count > 0)
                gvwActionDetails.CurrentRow.Cells["gvtNotes"].Value = Img_CaseNotesEdit;
        }
    }


}
