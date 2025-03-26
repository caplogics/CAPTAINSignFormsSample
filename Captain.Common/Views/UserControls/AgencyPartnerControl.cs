#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Web.Configuration;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Menus;
using System.Data.SqlClient;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Views.UserControls;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Captain.Common.Views.Forms;
using System.Text.RegularExpressions;
using Captain.Common.Model.Parameters;
using Captain.DatabaseLayer;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class AgencyPartnerControl : BaseUserControl
    {
        public AgencyPartnerControl(BaseForm baseForm)
            : base(baseForm)
        {
            InitializeComponent();
            _model = new CaptainModel();

            BaseForm = baseForm;
            Agency = BaseForm.BaseAgency;
            Dept = BaseForm.BaseDept;
            Program = BaseForm.BaseProg;
            ProgramYear = BaseForm.BaseYear;
            Old_AGencyNo = AgencyNo = BaseForm.BaseAgencyNo;
            btnAgencyEnquiry.Visible = false;
            propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
            //if (propAgencyControlDetails != null)
            //{
            //    if (propAgencyControlDetails.AllowClientINQ == "1")
            //        btnAgencyEnquiry.Visible = true;
            //    else btnAgencyEnquiry.Visible = false;

            //}
            //else btnAgencyEnquiry.Visible = false;

        }


        bool Loading_Complete = false;
        string PrvPanelCode = null;
        string SearcgCategory = null;
        string SearchFor = null;
        string SearchCaseType = null;
        string SearchCaseWRK = null;
        string Hierarchy = null;
        string SearchHie = null;
        string SelAgency = null;
        string SelDept = null;
        string SelProg = null;
        string SelYear = null;
        string DefAgy = null;
        string DefDept = null;
        string DefProg = null;
        string DefYear = null;
        string DepYear = null;
        bool DefHieExist = false;
        string strAgencyName = null;
        string strDeptName = null;
        string strProgName = null;
        string strNameFormat = null, strCwFormat = null;
        string Old_AGencyNo = null;
        bool Adv_search = true;

        private CaptainModel _model = null;

        #region properties

        public string Agency { get; set; }

        public string Dept { get; set; }


        public string Program { get; set; }


        public string ProgramYear { get; set; }


        public string AgencyName { get; set; }


        public string DeptName { get; set; }


        public string ProgramName { get; set; }


        public string AgencyNo { get; set; }

        public char AddPriv { get; set; }

        public ProgramDefinitionEntity ProgramDefinition { get; set; }

        public AgencyControlEntity propAgencyControlDetails { get; set; }

        #endregion


        private void GetDefaultHierarchy()
        {
            DefAgy = DefDept = DefProg = DefYear = null;
            DataSet ds = Captain.DatabaseLayer.MainMenu.GetUserDefHierarchy(BaseForm.UserID);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DefAgy = dr["Agy"].ToString();
                    DefDept = dr["Dept"].ToString();
                    DefProg = dr["Prog"].ToString();
                    DefYear = dr["DefYear"].ToString();
                    Agency = DefAgy;
                    Dept = DefDept;
                    Program = DefProg;
                    if (!(String.IsNullOrEmpty(DefAgy)) && !(String.IsNullOrEmpty(DefDept)) && !(String.IsNullOrEmpty(DefProg)))
                        DefHieExist = true;
                }
            }
        }



        public void ShowAgencyandName(string strAgency, string strDept, string strProgram, string strYear, string strAgencycode)
        {

            Agency = strAgency;
            
            string strAgypcode = strAgencycode;
            {               
                DataSet ds = Captain.DatabaseLayer.MainMenu.AgencyPartner_Navigate(string.Empty,
                                                         strAgencycode, string.Empty);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        strAgencyName = strAgency + " - " + _model.lookupDataAccess.GetHierachyDescription("1", strAgency, strDept, strProgram);
                        strDeptName = strDept + " - " + _model.lookupDataAccess.GetHierachyDescription("2", strAgency, strDept, strProgram);
                        strProgName = strProgram + " - " + _model.lookupDataAccess.GetHierachyDescription("3", strAgency, strDept, strProgram);
                        AgencyNo = strAgypcode;
                        BaseForm.BaseAgencyNo = strAgypcode;
                        BaseForm.BaseAgencyName = ds.Tables[0].Rows[0]["AGYP_NAME"].ToString();
                        BaseForm.GetAgencyDetails(ds.Tables[0].Rows[0], strAgencyName, strDeptName, strProgName, ProgramYear.ToString(), string.Empty, !string.IsNullOrEmpty(strAgencycode.Trim()) ? "Display" : string.Empty);
                        BaseForm.BaseTopApplSelect = "Y";
                    }
                    else
                        BaseForm.BaseAgencyNo = string.Empty;
                }



            }
        }

        
        private void On_ADV_SerachFormClosed(object sender, FormClosedEventArgs e)
        {
            AdvancedAgencypartnerSearch form = sender as AdvancedAgencypartnerSearch;
            if (form.DialogResult == DialogResult.OK)
            {
                
                if (!string.IsNullOrEmpty(BaseForm.BaseAgencyNo))
                {
                    BaseForm.BaseTopApplSelect = "Y";
                    TxtAgencyCode.Text = BaseForm.BaseAgencyNo;

                    TxtAgencyNo_LostFocus(sender, e);

                }
                
            }
        }


    

        public void TxtAgencyNo_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtAgencyCode.Text))
            {
                Process_From_Scr = "MainMenu";
                BtnSearchApp_Click(BtnSearchApp, EventArgs.Empty);
                if (!string.IsNullOrEmpty(Old_AGencyNo))
                {
                    Adv_search = false;
                    ShowAgencyandName(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, TxtAgencyCode.Text);
                }
            }
        }

        public string Process_From_Scr = "MainMenu";
        public void BtnSearchApp_Click(object sender, EventArgs e)
        {


            string Gbl_Hierarchy = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear;
            Old_AGencyNo = BaseForm.BaseAgencyNo;

            DataSet ds = Captain.DatabaseLayer.MainMenu.AgencyPartner_Navigate(string.Empty,
                                                          TxtAgencyCode.Text, string.Empty);
            DataTable dt = ds.Tables[0];
            string RecentHierChanged = string.Empty;
            
            if (dt.Rows.Count > 0)
            {
                DataRow dr_Mst = dt.Rows[0];


                try
                {
                    int TmpRows = 0;
                    gvwAgencyData.Rows.Clear();
                    foreach (DataRow dr in dt.Rows)
                    {
                        int rowIndex = 0;

                        rowIndex = gvwAgencyData.Rows.Add(dr["AGYP_CODE"].ToString(), dr["AGYP_NAME"].ToString().Trim(), dr["AGYP_Street"].ToString().Trim(), dr["AGYP_City"].ToString().Trim(), dr["AGYP_State"].ToString().Trim()); gvwAgencyData.Rows[rowIndex].Tag = dr;
                                               

                        TmpRows++;
                    }

                    if (TmpRows > 0)
                    {
                        gvwAgencyData.Rows[0].Tag = 0; Old_AGencyNo = TxtAgencyCode.Text;

                    }


                }
                catch (Exception ex) { }

            }
            else
            {
               
                if (!string.IsNullOrEmpty(TxtAgencyCode.Text.Trim()))
                {
                    MessageBox.Show("Agency Code# : " + TxtAgencyCode.Text + " Does Not Exist", "CAPSystems", MessageBoxButtons.OK);
                    TxtAgencyCode.Clear();
                }
                if (BaseForm.BaseTopApplSelect == "Y")
                    TxtAgencyCode.Text = Old_AGencyNo;
               
            }
        }

        private void BtnAdv_Search_Click(object sender, EventArgs e)
        {
            Adv_search = true;
            BaseForm.BasePIPDragSwitch = "N";
            AdvancedAgencypartnerSearch advancedMainMenuSearch = new AdvancedAgencypartnerSearch(BaseForm, true, true);
            advancedMainMenuSearch.FormClosed += new FormClosedEventHandler(On_ADV_SerachFormClosed);
            advancedMainMenuSearch.ShowDialog();
        }

        public void ShowHierachyandApplNo(string strAgency, string strDept, string strProg, string strYear1, string strApplicationNo)
        {

            Agency = strAgency;
            Dept = strDept;
            Program = strProg;
            ProgramYear = strYear1;


            string strYear = strYear1;



            if (string.IsNullOrEmpty(ProgramYear))
                ProgramYear = "    ";

            strAgencyName = strAgency + " - " + _model.lookupDataAccess.GetHierachyDescription("1", strAgency, strDept, strProg);
            strDeptName = strDept + " - " + _model.lookupDataAccess.GetHierachyDescription("2", strAgency, strDept, strProg);
            strProgName = strProg + " - " + _model.lookupDataAccess.GetHierachyDescription("3", strAgency, strDept, strProg);
            AgencyName = strAgencyName;
            DeptName = strDeptName;
            ProgramName = strProgName;
            BaseForm.BaseAgency = strAgency;
            BaseForm.BaseDept = strDept;
            BaseForm.BaseProg = strProg;

            if (string.IsNullOrEmpty(strYear))
                strYear = "    ";

            BaseForm.BaseYear = strYear;

            BaseForm.GetApplicantDetails(null, null, strAgencyName, strDeptName, strProgName, ProgramYear.ToString(), string.Empty, !string.IsNullOrEmpty(strApplicationNo.Trim()) ? "Display" : string.Empty);
        }

              

        private void TxtAgencyNo_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        {
            if (!string.IsNullOrEmpty(TxtAgencyCode.Text))
                TxtAgencyNo_LostFocus(TxtAgencyCode, EventArgs.Empty);
        }

        private void BtnAddApp_Click(object sender, EventArgs e)
        {
            //MainMenuAddApplicantForm AddAppForm = new MainMenuAddApplicantForm(BaseForm, 'N', null, null, null, null);
            //MainMenuAddApplicantForm AddAppForm = new MainMenuAddApplicantForm(BaseForm, 'N', string.Empty, string.Empty, string.Empty, string.Empty);
            //AddAppForm.FormClosed += new Form.FormClosedEventHandler(On_First_Add_App);
            //AddAppForm.ShowDialog();
        }

      

        public void Set_DefHie_as_BaseHie(string Agency, string Dept, string Prog, string Year)
        {
            ShowAgencyandName(Agency, Dept, Prog, Year, string.Empty);
            BaseForm.RefreshNavigationTabs(Agency + Dept + Prog);
        }

        

        private void Navigation_Click(object sender, EventArgs e)
        {
            string Navigation_Option = string.Empty;
            bool Rec_Exists = false;
            if (sender == Btn_First)
                Navigation_Option = "First";
            else if (sender == BtnP10)
            {
                if (string.IsNullOrEmpty(BaseForm.BaseAgencyNo))
                    Navigation_Option = "First";
                else
                    Navigation_Option = "RR";
            }
            else if (sender == BtnNxt)
            {
                if (string.IsNullOrEmpty(BaseForm.BaseAgencyNo))
                    Navigation_Option = "First";
                else
                    Navigation_Option = "Next";
            }
            else if (sender == BtnPrev)
            {
                if (string.IsNullOrEmpty(BaseForm.BaseAgencyNo))
                    Navigation_Option = "First";
                else
                    Navigation_Option = "Previous";
            }
            else if (sender == BtnN10)
            {
                if (string.IsNullOrEmpty(BaseForm.BaseAgencyNo))
                    Navigation_Option = "First";
                else
                    Navigation_Option = "FF";
            }
            else if (sender == BtnLast)
                Navigation_Option = "Last";


            DataSet ds = Captain.DatabaseLayer.MainMenu.AgencyPartner_Navigate(Navigation_Option,
                                                         BaseForm.BaseAgencyNo, "NAVIGATE");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Rec_Exists = true;
                    TxtAgencyCode.Text = ds.Tables[0].Rows[0]["AGYP_CODE"].ToString();
                    BaseForm.BaseAgencyNo = ds.Tables[0].Rows[0]["AGYP_CODE"].ToString();
                    TxtAgencyNo_LostFocus(TxtAgencyCode, EventArgs.Empty);
                }
            }

            if (!Rec_Exists)
            {
                string Error_Msg = string.Empty;
                switch (Navigation_Option)
                {
                    case "First":
                    case "RR":
                    case "Previous": Error_Msg = "You are Already at the First Record"; break;

                    case "Next":
                    case "FF":
                    case "Last": Error_Msg = "You are Already at the Last Record"; break;
                }
                MessageBox.Show(Error_Msg, "CAPTAIN");
            }
        }


        
        public void RefreshClearControl()
        {
            TxtAgencyCode.Clear();
            gvwAgencyData.Rows.Clear();
        }

      

        private void AgencyPartnerControl_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Agency))
            {              
                TxtAgencyCode.Validator = TextBoxValidation.IntegerValidator;
            }
            else
                this.Visible = false;


           
        }

        public void Refresh_NavigationVisibility()
        {
            if (!string.IsNullOrEmpty(Agency))
            {                
                TxtAgencyCode.Validator = TextBoxValidation.IntegerValidator;
            }
            else
                this.Visible = false;
        }

        private void btnAgencyEnquiry_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(BaseForm.BaseApplicationNo))
            {
                //_privilegeEntity = privilegeEntity;
                if (BaseForm.BaseTopApplSelect == "Y")
                {
                    MMDeeperSearchForm MainDeeper = new MMDeeperSearchForm(BaseForm);
                    MainDeeper.ShowDialog();
                }
                else
                    CommonFunctions.MessageBoxDisplay(Consts.Messages.AplicantSelectionMsg);
            }
            else
                CommonFunctions.MessageBoxDisplay(Consts.Messages.Applicantdoesntexist);

        }



        //  End of Latest Code By Yeswanth 

        private void setTreeviewhierachy(string BusinessModuleID)
        {
            switch (BusinessModuleID)
            {
                case Consts.Applications.Code.Administration:
                    InitializeModule("MiddleBanner.gif", TreeType.Administration);
                    break;
                case Consts.Applications.Code.HeadStart:
                    InitializeModule("MiddleBanner.gif", TreeType.HeadStart);
                    break;
                case Consts.Applications.Code.CaseManagement:
                    InitializeModule("MiddleBanner.gif", TreeType.CaseManagement);
                    break;
                case Consts.Applications.Code.EnergyRI:
                    InitializeModule("MiddleBanner.gif", TreeType.EnergyRI);
                    break;
                case Consts.Applications.Code.EnergyCT:
                    InitializeModule("MiddleBanner.gif", TreeType.EnergyCT);
                    break;
                case Consts.Applications.Code.EmergencyAssistance:
                    InitializeModule("MiddleBanner.gif", TreeType.EmergencyAssistance);
                    break;
                case Consts.Applications.Code.AccountsReceivable:
                    InitializeModule("MiddleBanner.gif", TreeType.AccountsReceivable);
                    break;
                case Consts.Applications.Code.HousingWeatherization:
                    InitializeModule("MiddleBanner.gif", TreeType.HousingWeatherization);
                    break;
                case Consts.Applications.Code.DashBoard:
                    InitializeModule("MiddleBanner.gif", TreeType.DashBoard);
                    break;
                case Consts.Applications.Code.HealthyStart:
                    InitializeModule("MiddleBanner.gif", TreeType.HealthyStart);
                    break;
                //case Consts.Applications.Code.CEAPAssistance:
                //    InitializeModule("MiddleBanner.gif", TreeType.CEAPAssistance);
                //    break;
            }
        }

        private void GvwAppHou_Click(object sender, EventArgs e)
        {

        }
               

        private void InitializeModule(string headerTitleImage, TreeType treeViewType)
        {
            //TreeViewControllerParameter treeViewControllerParameter = null;

            //// pnlApplicationHeaderImage.BackgroundImage = new ImageResourceHandle(headerTitleImage);
            ////  pnlApplicationHeaderImage.Width = 30;
            //BaseForm.NavigationTreeView.Nodes.Clear();
            //string HIE = HIE = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg; ;


            //treeViewControllerParameter = new TreeViewControllerParameter()
            //{
            //    TreeType = treeViewType,
            //    TreeView = BaseForm.NavigationTreeView,
            //    Hierarchy = HIE
            //};

            //BaseForm.TreeViewController.Initialize(treeViewControllerParameter);
        }




    }
}