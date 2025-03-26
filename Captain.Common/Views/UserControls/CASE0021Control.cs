/************************************************************************
 * Conversion On    :   01/05/2023      * Converted By     :   Kranthi
 * Modified On      :   01/05/2023      * Modified By      :   Kranthi
 * **********************************************************************/

#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Captain.DatabaseLayer;
using Wisej.Web;
#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class CASE0021Control : BaseUserControl
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private AlertCodes alertCodesUserControl = null;
        private string strYear = "    ";
        private int strIndex = 0;

        #endregion

        public CASE0021Control(BaseForm baseForm, PrivilegeEntity privileges)
            : base(baseForm)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privileges;
            _model = new CaptainModel();
            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            if (programEntity != null)
            {
                ProgramDefinition = programEntity;
            }

            propfundingsource = _model.lookupDataAccess.GetAgyFunds(); //CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00501", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg,Mode);

            propfundingsource = filterByHIE(propfundingsource, "View");

            // propCAMASTList = _model.SPAdminData.Browse_CAMAST("Code", null, null, null);

            alertCodesUserControl = new AlertCodes(BaseForm, privileges, ProgramDefinition);
            alertCodesUserControl.Dock = DockStyle.Fill;
            pnlAlertcode.Controls.Add(alertCodesUserControl);
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            boolVulnerable = Get_SNP_Vulnerable_Status();
            propcmbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, string.Empty);

            FillGridCaseActdata();
            PopulateToolbar(oToolbarMnustrip);

        }

        bool boolVulnerable;
        bool Age_Grt_60 = false, Age_Les_6 = false, Disable_Flag = false, FoodStamps_Flag = false;
        private bool Get_SNP_Vulnerable_Status()
        {
            bool Vulner_Flag = false;
            DateTime MST_Intake_Date = DateTime.Today, SNP_DOB = DateTime.Today;
            DateTime zeroTime = new DateTime(1, 1, 1);
            TimeSpan Time_Span;
            int Age_In_years = 0;

            if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].IntakeDate.Trim()))
                MST_Intake_Date = Convert.ToDateTime(BaseForm.BaseCaseMstListEntity[0].IntakeDate);
            string Non_Qual_Alien_SW = "N";
            Age_Grt_60 = false; Age_Les_6 = false; Disable_Flag = false; FoodStamps_Flag = false;
            foreach (CaseSnpEntity Entity in BaseForm.BaseCaseSnpEntity)
            {
                SNP_DOB = MST_Intake_Date;
                if (!string.IsNullOrEmpty(Entity.AltBdate.Trim()))
                    SNP_DOB = Convert.ToDateTime(Entity.AltBdate);

                Age_In_years = 0;

                if (MST_Intake_Date > SNP_DOB)
                {
                    Time_Span = (MST_Intake_Date - SNP_DOB);
                    Age_In_years = (zeroTime + Time_Span).Year - 1;
                }

                if (Age_In_years > 59)
                    Age_Grt_60 = true;

                if (Age_In_years < 6)
                    Age_Les_6 = true;

                if (Entity.Disable == "Y")
                    Disable_Flag = true;

                if (Entity.FootStamps == "Y")
                    FoodStamps_Flag = true;

                if (Entity.SsnReason == "Q" && BaseForm.BaseAgencyControlDetails.State == "TX") Non_Qual_Alien_SW = "Y";

            }



            //string Tmp_Age_Dis = propLiheApbdata.Age_dis;



            if ((Age_Grt_60 || Age_Les_6 || Disable_Flag) && Non_Qual_Alien_SW == "N")
            {
                //if (Tmp_Age_Dis == "1" || Tmp_Age_Dis == "2" || Tmp_Age_Dis == "3")
                //    Vulner_Flag = true;

                //if (Age_Les_6)
                Vulner_Flag = true;
            }



            return Vulner_Flag;
        }

        public void Refresh()
        {

            boolVulnerable = Get_SNP_Vulnerable_Status();
            propcmbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, string.Empty);

            FillGridCaseActdata();

            RefreshAlertCode();
            //gvwRes.SelectionChanged += new EventHandler(gvwRes_SelectionChanged);
            //if (gvwRes.Rows.Count != 0)
            //{
            //    if (gvwRes.Rows.Count > strIndex)
            //    {
            //        gvwRes.Rows[strIndex].Selected = true;
            //        gvwRes.CurrentCell = gvwRes.Rows[strIndex].Cells[1];
            //    }
            //    else
            //    {
            //        gvwRes.Rows[0].Selected = true;
            //        gvwRes.CurrentCell = gvwRes.Rows[0].Cells[1];
            //    }

            //    gvwRes_SelectionChanged(gvwRes, new EventArgs());

            //}
            //else
            //{

            //}
        }

        public void RefreshAlertCode()
        {
            pnlAlertcode.Controls.Clear();
            alertCodesUserControl = new AlertCodes(BaseForm, Privileges, ProgramDefinition);
            alertCodesUserControl.Dock = DockStyle.Fill;
            pnlAlertcode.Controls.Add(alertCodesUserControl);
        }

        private List<CommonEntity> filterByHIE(List<CommonEntity> LookupValues, string Mode)
        {
            string HIE = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            List<CommonEntity> _AgytabsFilter = new List<CommonEntity>();
            _AgytabsFilter = LookupValues;
            if (LookupValues.Count > 0)
            {

                if (Mode.ToUpper() == "ADD")
                {
                    _AgytabsFilter = _AgytabsFilter.FindAll(u => (u.ListHierarchy.Contains(HIE) || u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.ListHierarchy.Contains(BaseForm.BaseAgency + "****") || u.ListHierarchy.Contains("******")) && u.Active.ToString().ToUpper() == "Y").ToList();
                }
                else
                {
                    _AgytabsFilter = _AgytabsFilter.FindAll(u => u.ListHierarchy.Contains(HIE) || u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.ListHierarchy.Contains(BaseForm.BaseAgency + "****") || u.ListHierarchy.Contains("******")).ToList();
                }

                _AgytabsFilter = _AgytabsFilter.OrderByDescending(u => u.Active).ThenBy(u => u.Desc).ToList();
            }

            return _AgytabsFilter;
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public ToolBarButton ToolBarEdit { get; set; }

        // public ToolBarButton ToolBarNew { get; set; }

        // public ToolBarButton ToolBarDel { get; set; }

        public ToolBarButton ToolBarHelp { get; set; }

        public ToolBarButton ToolBarNotes { get; set; }

        public ProgramDefinitionEntity ProgramDefinition { get; set; }

        List<CommonEntity> propfundingsource { get; set; }

        List<CMBDCEntity> propcmbdc_List { get; set; }

        List<EMSCLCPMCEntity> propEMSCLCPMCList { get; set; }

        List<EMSCLAPMAEntity> propEMSCLAPMAList { get; set; }

        public List<CAMASTEntity> propCAMASTList { get; set; }

        public List<CASESPMEntity> propspmEntity { get; set; }

        #endregion

        public override void PopulateToolbar(ToolBar toolBar)
        {
            base.PopulateToolbar(toolBar);

            bool toolbarButtonInitialized = ToolBarEdit != null;
            ToolBarButton divider = new ToolBarButton();
            divider.Style = ToolBarButtonStyle.Separator;

            if (ToolBarEdit == null)
            {
                //ToolBarNew = new ToolBarButton();
                //ToolBarNew.Tag = "New";
                //ToolBarNew.ToolTipText = "New Adjustments";
                //ToolBarNew.Enabled = true;
                //ToolBarNew.Image = new IconResourceHandle(Consts.Icons16x16.AddItem);
                //ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                //ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarEdit = new ToolBarButton();
                ToolBarEdit.Tag = "Edit";
                ToolBarEdit.ToolTipText = "Edit Adjustments";
                ToolBarEdit.Enabled = true;
                ToolBarEdit.ImageSource = "captain-edit";// new IconResourceHandle(Consts.Icons16x16.EditIcon);
                ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);

                //ToolBarDel = new ToolBarButton();
                //ToolBarDel.Tag = "Delete";
                //ToolBarDel.ToolTipText = "Delete Adjustments";
                //ToolBarDel.Enabled = true;
                //ToolBarDel.Image = new IconResourceHandle(Consts.Icons16x16.Delete);
                //ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
                //ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarNotes = new ToolBarButton();
                ToolBarNotes.Tag = "CaseNotes";
                ToolBarNotes.ToolTipText = "Case Notes";
                ToolBarNotes.Enabled = true;
                ToolBarNotes.ImageSource = "captain-casenotesadd";// new IconResourceHandle(Consts.Icons16x16.CaseNotesNew);
                ToolBarNotes.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNotes.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarHelp = new ToolBarButton();
                ToolBarHelp.Tag = "Help";
                ToolBarHelp.ToolTipText = "Help";
                ToolBarHelp.Enabled = true;
                ToolBarHelp.ImageSource = "icon-help"; //new IconResourceHandle(Consts.Icons16x16.Help);
                ToolBarHelp.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarHelp.Click += new EventHandler(OnToolbarButtonClicked);
            }

            if (Privileges.AddPriv.Equals("true") && Privileges.ChangePriv.Equals("true") && Privileges.DelPriv.Equals("true"))
            {
                if (ToolBarEdit != null) ToolBarEdit.Visible = true;
                if (ToolBarNotes != null) ToolBarNotes.Visible = true;
            }
            else
            {
                if (ToolBarEdit != null)
                {
                    ToolBarEdit.Visible = false;
                }

                if (ToolBarNotes != null) ToolBarNotes.Visible = false;
            }

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
                ToolBarEdit,
               // ToolBarDel,
                ToolBarNotes,
                ToolBarHelp
            });

            if (gvwServices.Rows.Count == 0)
            {
                if (ToolBarEdit != null)
                    ToolBarEdit.Visible = false;

                if (ToolBarNotes != null)
                    ToolBarNotes.Visible = false;
            }

            if (Service_Cnt > 0)
                SetCaseNotesImages();
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
                    case Consts.ToolbarActions.New:
                        CASE0021Form adjustmentForm = new CASE0021Form(BaseForm, Privileges, "Add", null, ProgramDefinition);
                        adjustmentForm.StartPosition = FormStartPosition.CenterScreen;
                        adjustmentForm.ShowDialog();
                        break;
                    case Consts.ToolbarActions.Edit:
                        DataRow drrow = GetSelectedRow();
                        if (drrow != null)
                        {
                            if (ProgramDefinition.DepSerpostPAYCAT == "04")
                            {
                                CASE0021Form editadjustmentForm = new CASE0021Form(BaseForm, Privileges, "Edit", drrow, ProgramDefinition);
                                editadjustmentForm.FormClosed += new FormClosedEventHandler(Adjustments_Form_Closed);
                                editadjustmentForm.StartPosition = FormStartPosition.CenterScreen;
                                editadjustmentForm.ShowDialog();
                            }
                            else if (ProgramDefinition.DepSerpostPAYCAT == "02")
                            {
                                CASE0021Form editadjustmentForm = new CASE0021Form(BaseForm, Privileges, "Edit", drrow, ProgramDefinition);
                                editadjustmentForm.FormClosed += new FormClosedEventHandler(Adjustments_Form_Closed);
                                editadjustmentForm.StartPosition = FormStartPosition.CenterScreen;
                                editadjustmentForm.ShowDialog();
                            }
                            else
                            {
                                AlertBox.Show("Payment Category in Service Posting is not defined", MessageBoxIcon.Warning);
                            }

                        }
                        break;
                    case Consts.ToolbarActions.Delete:
                        if (GetSelectedRow() != null)
                        {
                            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
                        }
                        break;
                    case Consts.ToolbarActions.Help:
                            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(),"",""), target: "_blank");
                        break;

                    case Consts.ToolbarActions.CaseNotes:
                        //CaseNotes_List = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo);
                        //CaseNotes caseNotes = new CaseNotes(BaseForm, Privileges, CaseNotes_List, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo);
                        //caseNotes.FormClosed += new Form.FormClosedEventHandler(OnCaseNotesFormClosed);
                        //caseNotes.ShowDialog();


                        EMSCLCPMCEntity Ent = (gvwServices.SelectedRows[0].Tag as EMSCLCPMCEntity);
                        if (Ent != null)
                        {
                            string Notes_Key = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo + Ent.CLC_RES_FUND + Ent.CLC_RES_SEQ + Ent.CLC_SEQ;
                            ProgressNotes_Form Prog_Form = new ProgressNotes_Form(BaseForm, "Edit", Privileges, Notes_Key);
                            Prog_Form.FormClosed += new FormClosedEventHandler(On_PROGNOTES_Closed);
                            Prog_Form.StartPosition = FormStartPosition.CenterScreen;
                            Prog_Form.ShowDialog();
                        }

                        break;

                }
                executeCode.Append(Consts.Javascript.EndJavascriptCode);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
            }
        }

        private void Adjustments_Form_Closed(object sender, FormClosedEventArgs e)
        {
            //CASE0021Form form = sender as CASE0021Form;
            //if (form.DialogResult == DialogResult.OK)
            //{
            //    propEmsbdc_List = _model.EMSBDCData.GetEmsBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            //    GetEMSRes();
            //    FillGridPMCData();
            //}
        }

        private void On_PROGNOTES_Closed(object sender, FormClosedEventArgs e)
        {
            ProgressNotes_Form form = sender as ProgressNotes_Form;
            if (form.DialogResult == DialogResult.OK)
            {
                //PB_SP2_Notes.Image = new IconResourceHandle(Consts.Icons16x16.CaseNotesView);
                //SP_CAMS_Grid.CurrentRow.Cells["SP2_Notes_SW"].Value = "Y";
                //PB_SP2_Notes.Image = new IconResourceHandle(Consts.Icons16x16.CaseNotesView);
                SetCaseNotesImages();
            }
        }



        private void MessageBoxHandler(DialogResult dialogResult)
        {
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                string strSid = string.Empty;
                //EMSCLCPMCEntity emsclcpmcdelete = gvwServices.SelectedRows[0].Tag as EMSCLCPMCEntity;
                //if (emsclcpmcdelete != null)
                //{

                //    EMSCLCPMCEntity emsclcpmc = new EMSCLCPMCEntity();
                //    emsclcpmc.CLC_AGENCY = BaseForm.BaseAgency;
                //    emsclcpmc.CLC_DEPT = BaseForm.BaseDept;
                //    emsclcpmc.CLC_PROGRAM = BaseForm.BaseProg;
                //    emsclcpmc.CLC_YEAR = BaseForm.BaseYear;
                //    emsclcpmc.CLC_APP = BaseForm.BaseApplicationNo;
                //    emsclcpmc.CLC_RES_FUND = emsclcpmcdelete.CLC_RES_FUND;
                //    emsclcpmc.CLC_RES_SEQ = emsclcpmcdelete.CLC_RES_SEQ;
                //    emsclcpmc.CLC_SEQ = emsclcpmcdelete.CLC_SEQ;
                //    emsclcpmc.CLC_RES_DATE = emsclcpmcdelete.CLC_RES_DATE;
                //    emsclcpmc.Mode = "Delete";
                //    if (_model.EMSBDCData.InsertUpdateDelEmsclcpmc(emsclcpmc, out strSid))
                //    {
                //        propEMSCLCPMCList = _model.EMSBDCData.GetEmsclcpmcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear.Trim(), BaseForm.BaseApplicationNo, string.Empty, string.Empty, string.Empty, string.Empty);
                //        propEMSCLAPMAList = _model.EMSBDCData.GetEmsclapmaAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear.Trim(), BaseForm.BaseApplicationNo, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

                //    }
                //    else
                //    {
                //        MessageBox.Show("You can’t delete this record, as there are Dependices", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    }
                //}
            }
        }

        /// <summary>
        /// Get Selected Rows Tag Clas.
        /// </summary>
        /// <returns></returns>
        /// 
        int selRowIndex = 0;
        public DataRow GetSelectedRow()
        {
            DataRow drrow = null;
            if (gvwServices != null)
            {
                foreach (DataGridViewRow dr in gvwServices.SelectedRows)
                {
                    if (dr.Selected)
                    {

                        strIndex = gvwServices.SelectedRows[0].Index;
                        drrow = dr.Tag as DataRow;
                        selRowIndex = strIndex;
                    }
                }
            }
            return drrow;
        }

        List<CaseNotesEntity> CaseNotes_List = new List<CaseNotesEntity>();
        private void SetCaseNotesImages()
        {
            //strYear = "    ";
            //if (!string.IsNullOrEmpty(BaseForm.BaseYear))
            //{
            //    strYear = BaseForm.BaseYear;
            //}
            //EMSCLCPMCEntity Ent = (gvwServices.SelectedRows[0].Tag as EMSCLCPMCEntity);
            //if (Ent != null)
            //{
            //    string Notes_Key = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo + Ent.CLC_RES_FUND + Ent.CLC_RES_SEQ + Ent.CLC_SEQ;
            //    CaseNotes_List = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, Notes_Key);
            //    if (ToolBarNotes != null)
            //    {
            //        if (CaseNotes_List.Count > 0)
            //            ToolBarNotes.Image = new IconResourceHandle(Consts.Icons16x16.CaseNotesView);
            //        else
            //            ToolBarNotes.Image = new IconResourceHandle(Consts.Icons16x16.CaseNotesNew);
            //    }
            //}
        }


        int Service_Cnt = 0;
        private void FillGridCaseActdata()
        {
            gvwServices.SelectionChanged -= new EventHandler(gvwServices_SelectionChanged);
            gvwServices.Rows.Clear();
            Service_Cnt = 0;
            if (ToolBarNotes != null)
                ToolBarNotes.ImageSource = Consts.Icons.ico_CaseNotes_New; //new IconResourceHandle(Consts.Icons16x16.CaseNotesNew);

            DataSet ds = SPAdminDB.CAPS_CASE0021_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear.Trim(), BaseForm.BaseApplicationNo, string.Empty, string.Empty, string.Empty);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {

                    int introwindex = gvwServices.Rows.Add(item["CASEACT_CASEWRKR"].ToString(), item["CA_DESC"].ToString(), item["CASEACT_COST"].ToString(), LookupDataAccess.Getdate(item["CASEACT_ACTY_DATE"].ToString()), string.Empty);
                    gvwServices.Rows[introwindex].Tag = item;
                    CommonFunctions.setTooltip(introwindex, item["CASEACT_ADD_OPERATOR"].ToString(), item["CASEACT_DATE_ADD"].ToString(), item["CASEACT_LSTC_OPERATOR"].ToString(), item["CASEACT_DATE_LSTC"].ToString(), gvwServices);

                }
            }
            if (gvwServices.Rows.Count > 0)
            {
                if (ToolBarEdit != null)
                    ToolBarEdit.Visible = true;

                gvwServices.Rows[selRowIndex].Selected = true;
            }
            else
            {
                if (ToolBarEdit != null)
                    ToolBarEdit.Visible = false;
            }

            gvwServices.SelectionChanged += new EventHandler(gvwServices_SelectionChanged);
            gvwServices_SelectionChanged(gvwServices, new EventArgs());
        }

        private void gvwServices_DoubleClick(object sender, EventArgs e)
        {
            if (gvwServices.Rows.Count > 0)
            {
                if (gvwServices.SelectedRows[0].Selected)
                {
                    DataRow drrow = GetSelectedRow();
                    if (drrow != null)
                    {
                        CASE0021Form editadjustmentForm = new CASE0021Form(BaseForm, Privileges, "View", drrow, ProgramDefinition);
                        editadjustmentForm.StartPosition = FormStartPosition.CenterScreen;
                        editadjustmentForm.ShowDialog();
                    }
                }
            }
        }

        private void gvwServices_SelectionChanged(object sender, EventArgs e)
        {
            txt1.Text = "SP: ";
            txt2.Text = "Case Worker: ";
            txt3.Text = "Fund: ";
            txt4.Text = "Amount: ";
            txt5.Text = "Vendor: ";
            txt7.Text = "Date: ";
            txt8.Text = "Balance:";

            if (gvwServices.Rows.Count > 0)
            {

                try
                {
                    if (gvwServices.Rows.Count < gvwServices.SelectedRows[0].Index)
                        gvwServices.Rows[0].Selected = true;
                }
                catch (Exception ex)
                {
                    gvwServices.Rows[0].Selected = true;

                }
                if (gvwServices.SelectedRows[0].Selected)
                {
                    DataRow drrow = gvwServices.SelectedRows[0].Tag as DataRow;

                    if (drrow != null)
                    {
                        txt1.Text = "SP: " + drrow["SP0_DESCRIPTION"].ToString();
                        txt2.Text = "Case Worker: " + drrow["CASEACT_CASEWRKR"].ToString();
                        txt3.Text = "Fund: " + drrow["CASEACT_FUND1"].ToString();
                        txt4.Text = "Amount: " + drrow["SPM_AMOUNT"].ToString();

                        txt5.Text = "Vendor: " + drrow["CASEVDD_NAME"].ToString();
                        txt6.Text = boolVulnerable == true ? "Vulnerable" : "Non Vulnerable";
                        txt7.Text = "Date: " + LookupDataAccess.Getdate(drrow["SPM_STARTDATE"].ToString());
                        txt8.Text = "Balance: " + drrow["BDC_BALANCE"].ToString();

                        SetCaseNotesImages();
                    }

                }
            }
            else
            {
                txt1.Text = "SP: ";
                txt2.Text = "Case Worker: ";
                txt3.Text = "Fund: ";
                txt4.Text = "Amount: ";
                //if (emsresEntity.EMSRES_NPDATE != string.Empty)
                //    txt5.Text = "No Posting : " + LookupDataAccess.Getdate(emsresEntity.EMSRES_NPDATE);
                //else
                txt5.Text = "Vendor: ";
                //txt6.Text = "Send to Auditor : " + (emsresEntity.EMS_AllowPosting == "Y" ? "Yes" : "No");
                txt7.Text = "Date: ";
                txt8.Text = "Balance:";
            }
        }

    }
}