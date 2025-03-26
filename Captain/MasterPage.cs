/****************************************************************************************
 * Class Name   : MainForm
 * Author       : Kranthi
 * Created Date : 06/03/2022
 * Version      : 
 * Description  : The applications main presentation layer.
 ****************************************************************************************/
#region Using
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Xml;
using Wisej.Web;
using Captain.Common.Controllers;
using Captain.Common.Exceptions;
using Captain.Common.Handlers;
using Captain.Common.Menus;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Parameters;
using Captain.Common.Model.CaptainFaults;
using Captain.Common.Resources;
using Captain.Common.Utilities;
using Captain.Common.Views.Controls;
using Captain.Common.Views.Forms;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Views.UserControls;
using Captain.Common.Views.UserControls.Base;
using System.ComponentModel;
using System.Collections.Specialized;
using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web.Ext.NavigationBar;
using System.Runtime.Remoting.Lifetime;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;

#endregion

namespace Captain
{
    public partial class MasterPage : BaseForm
    {
        #region Variables

        // Private variables
        private Panel _fullScreenPanel = null;
        private XmlDocument _xmlDocument = null;
        private TreeNode _selectedTreeNode;
        private string _simpleFooterDisplay = string.Empty;
        private bool _isPDFEditorInitialized = false;
        private string _uploadedFileType = string.Empty;
        private PrivilegeEntity _privilegeEntity = null;
        private string _lastGuid = string.Empty;
        private CaptainModel _model = null;
        public HierachyNameControl hierachyNamecontrol = new HierachyNameControl();
        public ApplicationNameControl applicationNameControl;
        public AgencyNameControl agencyNameControl;
        //public ApplicationDetailsControl applicationDetailsControl ;
        public List<TagClass> RequestedDownloadTagClassList = new List<TagClass>();
        private bool _defaultHierchyform = false;
        public List<PrivilegeEntity> AdminSRCNPrivilege = null;


        public string _statAdminAgency = "";
        #endregion

        public MasterPage()
        {
            InitializeComponent();
            try
            {

                //LifetimeServices.LeaseTime = TimeSpan.FromSeconds(5);
                //LifetimeServices.LeaseManagerPollTime = TimeSpan.FromSeconds(5);
                //LifetimeServices.RenewOnCallTime = TimeSpan.FromSeconds(1);
                //LifetimeServices.SponsorshipTimeout = TimeSpan.FromSeconds(5);


                //AddWelcomeScreen(pnlTabs);

                //LoginZendesk(); //Zendesk Login

                DataBaseConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CMMSo"].ConnectionString.ToString();
                _model = new CaptainModel();

                MenuManager = new MenuManager(this);
                applicationNameControl = new ApplicationNameControl(this);
                agencyNameControl = new AgencyNameControl(this);
                BaseAgency = UserProfile.Agency;
                BaseDept = UserProfile.Dept;
                BaseProg = UserProfile.Prog;

                /***************************************/
                BaseDefaultHIE = UserProfile.Agency + "-" + UserProfile.Dept + "-" + UserProfile.Prog;
                /*************************************/

                BaseAdminAgency = BaseAgency;
                _statAdminAgency = BaseAgency;
                BaseAccessAll = UserProfile.AccessAll;

                PrivAdmnAgency = BaseAdminAgency;

                if (BaseAccessAll == "Y")
                {
                    pSwitchAccount.Visible = true;
                    pnlSwitchAccount.Visible = true;
                }
                else
                {
                    pSwitchAccount.Visible = false;
                    pnlSwitchAccount.Visible = false;
                }

                List<ChldTrckEntity> chldtrackList = new List<ChldTrckEntity>();
                BaseTaskEntity = chldtrackList;
                List<HlsTrckEntity> hlstrackList = new List<HlsTrckEntity>();
                BaseHlsTaskEntity = hlstrackList;
                BaseFilterYear = "3";
                BaseYear = "    ";
                BaseComponent = string.Empty;


                ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseAgency, BaseDept, BaseProg);
                if (programEntity != null)
                {
                    BaseYear = programEntity.DepYear.Trim() == string.Empty ? "    " : programEntity.DepYear;
                }
                BaseAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
                BaseAgyTabsEntity = _model.lookupDataAccess.GetAgyTabs(string.Empty, string.Empty, string.Empty);
                BaseAgencyuserHierarchys = _model.UserProfileAccess.GetUserHierarchyByID(UserID);
                BaseCaseHierachyListEntity = _model.HierarchyAndPrograms.GetCaseHierachyAllData("**", "**", "**");
                if (BaseAgencyuserHierarchys.Count > 0)
                {
                    if (BaseAgencyControlDetails.SiteSecurity == "1")
                    {
                        List<HierarchyEntity> userHierarchy = BaseAgencyuserHierarchys.FindAll(u => u.Sites != string.Empty);
                        if (userHierarchy.Count > 0)
                        {
                            BaseAgencyControlDetails.SitesData = "1";
                        }

                    }
                }
                Gbl_Rent_Rec = string.Empty;
                BasePIPDragSwitch = "N";
                DataBaseConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CMMSo"].ConnectionString.ToString();
                if (System.Configuration.ConfigurationManager.ConnectionStrings["LEANINTAKEConnection"] != null)
                    BaseLeanDataBaseConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["LEANINTAKEConnection"].ConnectionString.ToString();


                if (System.Configuration.ConfigurationManager.ConnectionStrings["BaseDSSXMLDBConnection"] != null)
                    BaseDSSXMLDBConnString = System.Configuration.ConfigurationManager.ConnectionStrings["BaseDSSXMLDBConnection"].ConnectionString.ToString();


                BaseClientFolloupFromDate = DateTime.Now.AddDays(-7).ToShortDateString();
                BaseClientFolloupToDate = DateTime.Now.ToShortDateString();
                BaseClientFolloupWorker = string.Empty;
                BaseClientFolloupScreentype = "A";
                BaseClientFolloupScreencode = string.Empty;

                List<PrivilegeEntity> NavigationPrivileges = _model.UserProfileAccess.GetApplicationsByUserID(UserID, string.Empty);


                //Application.SetSessionTimeout(1800);
                Application.SessionTimeout += Application_SessionTimeout;

            }
            catch (Exception ex)
            {

            }
        }
        private static void Application_SessionTimeout(object sender, System.ComponentModel.HandledEventArgs e)
        {
            // suppress the built-in timeout window.
            e.Handled = true;
        }
        private void Application_BrowserSizeChanged(object sender, EventArgs e)
        {
            this.Size = Application.Browser.Size;
        }
        private void MasterPage_Load(object sender, EventArgs e)
        {

            // Application.BrowserSizeChanged += Application_BrowserSizeChanged;

            UserEntity userInfo = Captain<UserEntity>.Session[Consts.SessionVariables.UserProfile];
            string fullName = Captain<string>.Session[Consts.SessionVariables.UserID];
            sbpWelcome.Text = string.Concat(Consts.Controls.Welcome.GetControlName(), Consts.Common.Comma, Consts.Common.Space, fullName);
            sbpLoginStatus.Text = string.Concat("Previous Login was " + Captain<string>.Session[Consts.SessionVariables.LostLogin_Status], Consts.Common.Comma, " On  ", Captain<string>.Session[Consts.SessionVariables.LostLogin_Date]);

            BuildMenuTree();



            if (userInfo.Successful.Equals("0"))
            {

                ChangePassword changePassword = new ChangePassword(this, "STARTCHNG");
                changePassword.StartPosition = FormStartPosition.CenterScreen;
                changePassword.ShowDialog();

            }
            else
            {
                if (userInfo.PWDChangeDate != string.Empty)
                {
                    AgencyControlEntity agencycontroldata = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
                    if (agencycontroldata != null)
                    {
                        if (agencycontroldata.ForcePwd.ToUpper() == "Y")
                        {
                            DateTime dt = Convert.ToDateTime(userInfo.PWDChangeDate).Date;
                            DateTime dtnew = DateTime.Now.Date;
                            int intdif = (dtnew - dt).Days;
                            if (intdif > Convert.ToInt32(agencycontroldata.ForcePwdDays))
                            {
                                ChangePassword changePassword = new ChangePassword(this, string.Empty);
                                changePassword.StartPosition = FormStartPosition.CenterScreen;
                                changePassword.ShowDialog();
                            }
                        }
                    }
                }

            }

        }

        public override void BuildMenuTree()
        {

            NavTabs.Items.Clear();
            HIE = BaseAgency + BaseDept + BaseProg;
            strcurrentHIE = BaseAgency + "-" + BaseDept + "-" + BaseProg;
            CaptainModel Capmodel = new CaptainModel();
            List<PrivilegeEntity> NavigationPrivileges = _model.UserProfileAccess.GetApplicationsByUserID(UserID, HIE);

            if (NavigationPrivileges.Count > 0)
            {
                // BusinessModuleID = NavigationPrivileges[0].ModuleCode;
                BusinessModuleID = NavigationPrivileges.Min(x => x.ModuleCode);
            }

            foreach (PrivilegeEntity iModuleItem in NavigationPrivileges)
            {
                string code = iModuleItem.ModuleCode;

                /******************************** MODULE Menu Head Creation  *********************************************/
                NavigationBarItem nBarModule = new NavigationBarItem();
                nBarModule.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
                nBarModule.CssStyle = "border-bottom:1px solid #F4F4F4;";
                nBarModule.ForeColor = System.Drawing.Color.Black;
                nBarModule.Tag = "Head";
                nBarModule.Icon = "Resources\\Images\\MenuIcons\\" + iModuleItem.ModuleCode + ".png";
                nBarModule.InfoTextBackColor = System.Drawing.Color.FromName("@window");
                nBarModule.Name = "nModule_" + iModuleItem.ModuleCode;
                nBarModule.Text = iModuleItem.ModuleName;
                //nBarModule.Controls[0].Controls["open"].BackColor = System.Drawing.Color.Red;
                //nBarModule.Controls[0].Controls["open"].ForeColor = System.Drawing.Color.Red;
                //nBarModule.Controls[0].Controls["open"].Hide();
                //nBarModule.Controls[0].Controls["open"].Visible = false;

                //nBarModule.Height = 35;

                /******************************** SCREENS Menu Head Creation  *********************************************/

                NavigationBarItem nBarModScreenHead = new NavigationBarItem();
                nBarModScreenHead.BackColor = System.Drawing.Color.FromArgb(239, 239, 239);
                nBarModScreenHead.AllowHtml = true;
                nBarModScreenHead.CssStyle = "border-bottom:1px solid #F4F4F4; ";
                nBarModScreenHead.ForeColor = System.Drawing.Color.Black;
                nBarModScreenHead.Name = "nModule_" + iModuleItem.ModuleCode + "_SCRHead";
                nBarModScreenHead.Text = "Screens";
                nBarModScreenHead.ToolTipText = "Screens";
                nBarModScreenHead.Icon = "Resources/Images/MenuIcons/Screens.png";
                nBarModScreenHead.Controls[0].Controls[0].CssStyle = "margin-left:8px; font-weight:bold;";
                nBarModScreenHead.Controls[0].Controls[1].CssStyle = "margin-left:0px; font-weight:bold;";
                nBarModScreenHead.Expanded = true;
                nBarModule.Items.Add(nBarModScreenHead);    // Add Screen Menu Header to Module

                /******************************** REPORT Menu Head Creation  *********************************************/
                NavigationBarItem nBarModReportHead = new NavigationBarItem();
                nBarModReportHead.BackColor = System.Drawing.Color.FromArgb(239, 239, 239);
                nBarModReportHead.AllowHtml = true;
                nBarModReportHead.CssStyle = "border-bottom:1px solid #F4F4F4;";
                nBarModReportHead.ForeColor = System.Drawing.Color.Black;
                nBarModReportHead.Name = "nModule_" + iModuleItem.ModuleCode + "_RPTHead";
                nBarModReportHead.Text = "Reports";
                nBarModReportHead.ToolTipText = "Reports";
                nBarModReportHead.Icon = "Resources/Images/MenuIcons/Reports.png";
                nBarModReportHead.Controls[0].Controls[0].CssStyle = "margin-left:8px;";
                nBarModReportHead.Controls[0].Controls[1].CssStyle = "margin-left:0px;";
                nBarModule.Items.Add(nBarModReportHead);        // Add Report Menu Header to Module


                if (code == "99")
                {
                    nBarModScreenHead.Visible = false;
                }


                List<PrivilegeEntity> userPrivilege = null;
                if (code == "01")
                {
                    pnlCenterbar.Visible = true;
                    pnlSearchbox.Visible = false;
                    pHIEFilter.Visible = true;


                    userPrivilege = _model.UserProfileAccess.GetScreensByUserID(code, UserID, string.Empty);
                    AdminSRCNPrivilege = userPrivilege;
                }
                else
                {
                    userPrivilege = _model.UserProfileAccess.GetScreensByUserID(code, UserID, HIE);
                }

                List<MenuBranchEntity> menubranhlist = _model.UserProfileAccess.GetMenuBranches();

                if (menubranhlist.Count > 0)
                {
                    foreach (MenuBranchEntity menuitem in menubranhlist)
                    {
                        if (code == "01" && UserID.ToUpper() == "JAKE")
                        {
                            PrivilegeEntity privilegeAdmn12 = new PrivilegeEntity();
                            privilegeAdmn12.UserID = UserID;
                            privilegeAdmn12.ModuleCode = "01";
                            privilegeAdmn12.Program = "ADMN0012";
                            privilegeAdmn12.Hierarchy = "******";
                            privilegeAdmn12.AddPriv = "true";
                            privilegeAdmn12.ChangePriv = "true";
                            privilegeAdmn12.DelPriv = "true";
                            privilegeAdmn12.ViewPriv = "true";
                            privilegeAdmn12.PrivilegeName = "Agency Control File Maintenance";
                            privilegeAdmn12.DateLSTC = string.Empty;
                            privilegeAdmn12.LSTCOperator = UserID;
                            privilegeAdmn12.DateAdd = string.Empty;
                            privilegeAdmn12.AddOperator = UserID;
                            privilegeAdmn12.ModuleName = string.Empty;
                            privilegeAdmn12.showMenu = "Y";
                            privilegeAdmn12.screenType = "SCREEN";
                            userPrivilege.Insert(0, privilegeAdmn12);

                        }
                        if (code == "01" && UserID.ToUpper() == "CAPLOGICS")
                        {
                        }
                        List<PrivilegeEntity> screentypewiselist = userPrivilege.FindAll(u => u.screenType.Trim() == menuitem.MemberCode.Trim());

                        if (screentypewiselist != null && screentypewiselist.Count > 0)
                        {
                            screentypewiselist = screentypewiselist.FindAll(u => u.showMenu.ToString().ToUpper() == "Y");
                            screentypewiselist = screentypewiselist.OrderBy(u => u.PrivilegeName).ToList();

                            if (menuitem.MemberCode.ToString() == "SCREEN")
                            {
                                foreach (PrivilegeEntity privilegeEntity in screentypewiselist)
                                {
                                    if (BaseAgencyControlDetails.AgyShortName == "PROACTION" || BaseAgencyControlDetails.AgyShortName == "PROACT")
                                    {
                                        if (privilegeEntity.Program.Trim() != "HSS00134")
                                        {
                                            NavigationBarItem childNavigationBarItem = new NavigationBarItem();
                                            childNavigationBarItem.Text = privilegeEntity.PrivilegeName;
                                            childNavigationBarItem.ToolTipText = privilegeEntity.PrivilegeName;
                                            childNavigationBarItem.Height = 16;
                                            //childNavigationBarItem.IconVisible = false;
                                            //childNavigationBarItem.ImageIndex = 1;
                                            childNavigationBarItem.Icon = "Resources/Images/MenuIcons/formicon.png";
                                            childNavigationBarItem.AllowHtml = true;
                                            childNavigationBarItem.CssStyle = "height:10px; border-bottom:1px solid #F2F2F2; ";
                                            this.styleSheet1.SetCssClass(childNavigationBarItem, "NavbarItem");
                                            childNavigationBarItem.Tag = privilegeEntity;
                                            childNavigationBarItem.Controls[0].Controls[0].CssStyle = "margin-left:12px;";
                                            childNavigationBarItem.Controls[0].Controls[1].CssStyle = "margin-left:2px;";
                                            nBarModScreenHead.Items.Add(childNavigationBarItem);
                                        }
                                    }
                                    else
                                    {
                                        NavigationBarItem childNavigationBarItem = new NavigationBarItem();
                                        childNavigationBarItem.Text = privilegeEntity.PrivilegeName;
                                        childNavigationBarItem.ToolTipText = privilegeEntity.PrivilegeName;
                                        childNavigationBarItem.Height = 16;
                                        //childNavigationBarItem.IconVisible = false;
                                        //childNavigationBarItem.ImageIndex = 1;
                                        childNavigationBarItem.Icon = "Resources/Images/MenuIcons/formicon.png";
                                        childNavigationBarItem.AllowHtml = true;
                                        childNavigationBarItem.CssStyle = "height:10px; border-bottom:1px solid #F2F2F2; ";
                                        this.styleSheet1.SetCssClass(childNavigationBarItem, "NavbarItem");
                                        childNavigationBarItem.Tag = privilegeEntity;
                                        childNavigationBarItem.Controls[0].Controls[0].CssStyle = "margin-left:12px;";
                                        childNavigationBarItem.Controls[0].Controls[1].CssStyle = "margin-left:2px;";
                                        nBarModScreenHead.Items.Add(childNavigationBarItem);
                                    }
                                }
                            }
                        }
                    }
                }

                userPrivilege = Capmodel.UserProfileAccess.GetReportsByUserID(code, UserID);
                if (userPrivilege != null && userPrivilege.Count > 0)
                {
                    userPrivilege = userPrivilege.OrderBy(u => u.PrivilegeName).ToList();
                    foreach (PrivilegeEntity privilegeEntity in userPrivilege)
                    {
                        NavigationBarItem childNavigationBarItem = new NavigationBarItem();
                        childNavigationBarItem.Text = privilegeEntity.PrivilegeName;
                        childNavigationBarItem.ToolTipText = privilegeEntity.PrivilegeName;
                        childNavigationBarItem.Height = 16;
                        childNavigationBarItem.Icon = "Resources/Images/MenuIcons/formicon.png";
                        childNavigationBarItem.AllowHtml = true;
                        childNavigationBarItem.CssStyle = "height:10px; border-bottom:1px solid #F2F2F2;";
                        childNavigationBarItem.Tag = privilegeEntity;
                        childNavigationBarItem.Controls[0].Controls[0].CssStyle = "margin-left:12px;";
                        childNavigationBarItem.Controls[0].Controls[1].CssStyle = "margin-left:2px;";
                        nBarModReportHead.Items.Add(childNavigationBarItem);
                    }
                }

                userPrivilege = Capmodel.UserProfileAccess.GetUserReportMaintenanceByserID(code, UserID);
                if (userPrivilege != null && userPrivilege.Count > 0)
                {

                    /******************************** User Report Maintenance Menu Head Creation  *********************************************/
                    NavigationBarItem nBarModUserReportMgntHead = new NavigationBarItem();
                    nBarModUserReportMgntHead.BackColor = System.Drawing.Color.FromArgb(239, 239, 239);
                    nBarModUserReportMgntHead.AllowHtml = true;
                    nBarModUserReportMgntHead.CssStyle = "border-bottom:1px solid #F4F4F4;";
                    nBarModUserReportMgntHead.ForeColor = System.Drawing.Color.Black;
                    nBarModUserReportMgntHead.Name = "nModule_" + iModuleItem.ModuleCode + "_USRRPTMGNTHead";
                    nBarModUserReportMgntHead.Text = "User Report Maintenance";
                    nBarModUserReportMgntHead.ToolTipText = "User Report Maintenance";
                    nBarModUserReportMgntHead.Icon = "Resources/Images/MenuIcons/Reports.png";
                    nBarModUserReportMgntHead.Controls[0].Controls[0].CssStyle = "margin-left:8px;";
                    nBarModUserReportMgntHead.Controls[0].Controls[1].CssStyle = "margin-left:0px;";
                    nBarModule.Items.Add(nBarModUserReportMgntHead);        // Add Report Menu Header to Module

                    userPrivilege = userPrivilege.OrderBy(u => u.PrivilegeName).ToList();
                    foreach (PrivilegeEntity privilegeEntity in userPrivilege)
                    {
                        NavigationBarItem childNavigationBarItem = new NavigationBarItem();
                        childNavigationBarItem.Text = privilegeEntity.PrivilegeName;
                        childNavigationBarItem.ToolTipText = privilegeEntity.PrivilegeName;
                        childNavigationBarItem.Height = 16;
                        childNavigationBarItem.Icon = "Resources/Images/MenuIcons/formicon.png";
                        childNavigationBarItem.AllowHtml = true;
                        childNavigationBarItem.CssStyle = "height:10px; border-bottom:1px solid #F2F2F2;";
                        childNavigationBarItem.Tag = privilegeEntity;
                        childNavigationBarItem.Controls[0].Controls[0].CssStyle = "margin-left:12px;";
                        childNavigationBarItem.Controls[0].Controls[1].CssStyle = "margin-left:2px;";
                        nBarModUserReportMgntHead.Items.Add(childNavigationBarItem);
                    }
                }


                //***************** COMPONENTS MENU BUILD ********************************//
                if (UserProfile.Components.ToString() != "" && code == "02")
                {
                    userPrivilege = _model.UserProfileAccess.GetScreensByUserID(code, UserID, HIE);
                    List<PrivilegeEntity> screentypewiselist = userPrivilege.FindAll(u => u.screenType.Trim() == "SCREEN" && u.showMenu.ToString().ToUpper() == "Y" && u.Program.ToString().ToUpper() == "HSS00134");
                    if (screentypewiselist != null && screentypewiselist.Count > 0)
                    {
                        /******************************** COMPONENT Menu Head Creation  *********************************************/
                        NavigationBarItem nBarModComponentHead = new NavigationBarItem();
                        nBarModComponentHead.BackColor = System.Drawing.Color.FromArgb(239, 239, 239);
                        nBarModComponentHead.AllowHtml = true;
                        nBarModComponentHead.CssStyle = "border-bottom:1px solid #F4F4F4;";
                        nBarModComponentHead.ForeColor = System.Drawing.Color.Black;
                        nBarModComponentHead.Name = "nModule_" + iModuleItem.ModuleCode + "_COMPONENTHead";
                        nBarModComponentHead.Text = "Components";
                        nBarModComponentHead.ToolTipText = "Components";
                        nBarModComponentHead.Icon = "Resources/Images/MenuIcons/components.png";
                        nBarModComponentHead.Controls[0].Controls[0].CssStyle = "margin-left:8px;";
                        nBarModComponentHead.Controls[0].Controls[1].CssStyle = "margin-left:0px;";
                        nBarModule.Items.Add(nBarModComponentHead);        // Add Report Menu Header to Module


                        List<SPCommonEntity> AllComponents_List = new List<SPCommonEntity>();
                        AllComponents_List = _model.SPAdminData.Get_AgyRecs("COMPONENTS");
                        if (AllComponents_List.Count > 0) AllComponents_List = AllComponents_List.FindAll(u => u.Active == "Y");
                        if (UserProfile.Components.ToString() != "****")
                        {
                            string[] filterParts = UserProfile.Components.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            AllComponents_List = AllComponents_List.Where(x => filterParts.Any(fp => x.Code.Contains(fp))).ToList();
                        }
                        foreach (SPCommonEntity ComponentEnty in AllComponents_List)
                        {
                            NavigationBarItem childNavigationBarItem = new NavigationBarItem();
                            childNavigationBarItem.Text = ComponentEnty.Desc; //privilegeEntity.PrivilegeName;
                            childNavigationBarItem.ToolTipText = ComponentEnty.Desc; //privilegeEntity.PrivilegeName;
                            childNavigationBarItem.Height = 16;
                            childNavigationBarItem.Icon = "Resources/Images/MenuIcons/formicon.png";
                            childNavigationBarItem.AllowHtml = true;
                            childNavigationBarItem.CssStyle = "height:10px; border-bottom:1px solid #F2F2F2;";

                            OtherApps _OthApps = new OtherApps();
                            _OthApps._Code = "CMPNT_" + ComponentEnty.Code;
                            _OthApps._PrivilegeEntity = screentypewiselist[0];
                            _OthApps._Desc = ComponentEnty.Desc;

                            childNavigationBarItem.Tag = _OthApps; // "CMPNT_" + ComponentEnty.Code;
                            childNavigationBarItem.Controls[0].Controls[0].CssStyle = "margin-left:12px;";
                            childNavigationBarItem.Controls[0].Controls[1].CssStyle = "margin-left:2px;";
                            nBarModComponentHead.Items.Add(childNavigationBarItem);
                        }
                    }
                }

                NavTabs.Items.Add(nBarModule);
                NavTabs.Items[0].Height = 50;
            }

            int navid = 0; int xx = 0;
            foreach (NavigationBarItem onbar in NavTabs.Items)
            {

                if (onbar.Tag.ToString() == "Head")
                {
                    onbar.Controls[0].Controls["open"].Visible = false;

                    string cnavid = onbar.Name.Split('_')[1].ToString();
                    if (strcurrentModuleID == cnavid)
                    {
                        navid = xx;
                    }
                }
                xx++;
            }

            if (NavigationPrivileges.Count > 0)
            {
                // Kranthi :: 03/06/2023 :: PCS.docx :: Wants to close all the Menu Module items when user logged in. They want to open on their own.
                NavTabs_ItemClick(NavTabs.Items[navid], new NavigationBarItemClickEventArgs(NavTabs.Items[navid]));
                if (isFirstLoad == 0)
                {
                    for (int x = 0; x < NavTabs.Items.Count; x++)
                    {
                        NavTabs.Items[x].Expanded = false;
                    }
                    isFirstLoad = 1;
                }
            }
            else
            {
                // AlertBox.Show("No previlages assigned for this user", MessageBoxIcon.Warning);

            }
        }
        int isFirstLoad = 0;

        private void NBarModule_Expand(object sender, EventArgs e)
        {

            //string args = e.GetListItem().ToString();
            //NavTabs_ItemClick(sender, new NavigationBarItemClickEventArgs());
        }

        private void OnLogoutClick(object sender, EventArgs e)
        {
            if (Application.Session["userlogid"] != null)
            {

                _model.UserProfileAccess.InsertUpdateLogUsers(string.Empty, string.Empty, string.Empty, "Edit", Application.Session["userlogid"].ToString());
            }
            LogoutfromZendesk();
            Application.Session.IsLoggedOn = false;

            Application.Exit();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }

        private void buttonCollapse_Click(object sender, EventArgs e)
        {
            var setCompact = !NavTabs.CompactView;
            NavTabs.CompactView = setCompact;
            if (setCompact == true)
            {
                //this.splitContainer1.SplitterDistance = 70;
                this.pnlMenuBar.Width = 70;
                this.pnlMenuBar.CssStyle = "transition:left 300ms linear;";
                //for (int x = 0; x < NavTabs.Items.Count; x++)
                //{
                //    NavTabs.Items[x].Expanded = false;
                //    NavTabs.Items[x].Items[0].Expanded = false; // Screens
                //    NavTabs.Items[x].Items[1].Expanded = false; // Reports
                //}

                btnCollapse.ToolTipText = "Show Menu Tree";
            }
            else
            {
                //this.splitContainer1.SplitterDistance = 300;
                this.pnlMenuBar.Width = 300;
                this.pnlMenuBar.CssStyle = "transition:left 300ms linear;";
                btnCollapse.ToolTipText = "Hide Menu Tree";
                // BuildMenuTree();

                foreach (NavigationBarItem onbar in NavTabs.Items)
                {
                    if (onbar.Tag.ToString() == "Head")
                    {
                        onbar.Controls[0].Controls["open"].Visible = false;
                    }
                }
            }

        }
        private void pnlContainer_Resize(object sender, EventArgs e)
        {
            var minFormWidth =
                Math.Max(780, Math.Min(1028, this.pnlContainer.Width - 100));

            //this.pnlTabs.MinimumSize = new Size(minFormWidth, 0);
            //this.pnlButtonBar.MinimumSize = new Size(minFormWidth, 0);
        }

        private void NavTabs_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < NavTabs.Items.Count; x++)
            {
                NavTabs.Items[x].Expanded = false;
            }
            NavigationBar onavigationBar = sender as NavigationBar;
            if (onavigationBar.Tag.ToString() == "Head")
            {
                NavTabs.Items[onavigationBar.Name].Expanded = true;


            }

        }

        string selModule = "";
        string HIE = string.Empty; //string frmHiEScren = "N";
        private void NavTabs_ItemClick(object sender, NavigationBarItemClickEventArgs e)
        {
            setDefaultIcons();

            NavigationBar onavigationBar = e.Item.Tag as NavigationBar;
            if (e.Item.Tag != null)
            {
                if (e.Item.Tag.ToString() == "Head")
                {
                    btncloseAll.Visible = false;
                    string strTabName = e.Item.Name.ToString();
                    string ModID = strTabName.Substring(8, strTabName.Length - 8).ToString();
                    BusinessModuleID = ModID;
                    colNavItems = new List<NavigationBarItem>();
                    applicationNameControl.Controls[0].Controls["panelNavButtons"].Visible = true; //Kranthi 02/01/2023 :: Brian wants to enable the navbar buttons at top main screen also, but Ram suggest to hide this at master page only show in Screens

                    if (selModule != e.Item.Text.ToString())
                    {
                        pnlTabs.Controls.Clear();
                        pnlTabs.Controls.Add(ContentTabs);
                        ContentTabs.Dock = DockStyle.Fill;
                        ContentTabs.TabPages.Clear();
                        selModule = e.Item.Text.ToString();
                    }
                    else
                    {
                        if (frmHiEScren == "N")
                        {
                            int intTabPagesTotal = ContentTabs.TabPages.Count;
                            if (e.Item.Text.Contains("Admin"))
                            {
                                if (intTabPagesTotal > 0)
                                    btncloseAll.Visible = true;
                            }
                            else
                            {
                                if (intTabPagesTotal > 1)
                                    btncloseAll.Visible = true;
                            }
                            NavTabs.Items[e.Item.Name].Expanded = true;
                            return;
                        }
                    }
                    frmHiEScren = "N";
                    if (e.Item.Text.Contains("Admin"))
                    {
                        int intTabPagesTotal = ContentTabs.TabPages.Count;
                        if (intTabPagesTotal == 0)
                        {
                            this.pnlContainer.BackgroundImageSource = "Resources\\Images\\11-01-01.jpg";
                            this.pnlContainer.BackgroundImageLayout = Wisej.Web.ImageLayout.Stretch;
                        }
                        BaseAdminAgency = _statAdminAgency;

                        pnlCenterbar.Visible = true;
                        pnlSearchbox.Visible = false;
                        pHIEFilter.Visible = true;
                        string strAgencyName = "** - All Agencies";
                        if (BaseAdminAgency != "**")
                            strAgencyName = BaseAdminAgency + " - " + _model.lookupDataAccess.GetHierachyDescription("1", BaseAdminAgency, BaseDept, BaseProg);

                        hierachyNamecontrol.lblHierchy.Text = strAgencyName;//CommonFunctions.GetHTMLHierachyFormat(strAgencyName, string.Empty, string.Empty, string.Empty, "01");

                        pnlCenterbar.Controls.Clear();
                        hierachyNamecontrol.Dock = Wisej.Web.DockStyle.Fill;
                        pnlCenterbar.Controls.Add(hierachyNamecontrol);
                    }
                    else
                    {
                        pnlContainer.BackgroundImageSource = "Resources\\Images\\blank.png";
                        pnlContainer.BackColor = Color.White;

                        pnlCenterbar.Visible = true;
                        pnlSearchbox.Visible = true;
                        pHIEFilter.Visible = true;
                        //applicationNameControl.Controls[0].Controls["panelNavButtons"].Visible = true;

                        if (_model.lookupDataAccess.CheckDefaultHierachy(BaseAgency, BaseDept, BaseProg, UserProfile.UserID) == Consts.Common.Exists)
                        {

                            HIE = BaseAgency + BaseDept + BaseProg;
                            strcurrentHIE = BaseAgency + "-" + BaseDept + "-" + BaseProg;
                            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseAgency, BaseDept, BaseProg);
                            if (string.IsNullOrEmpty(BaseYear.Trim()))
                            {

                                if (programEntity != null)
                                {
                                    BaseYear = programEntity.DepYear.Trim() == string.Empty ? "    " : programEntity.DepYear;
                                }
                            }

                            if (BaseAgency != BaseAdminAgency)
                                BaseAdminAgency = BaseAgency;

                            BaseTopApplSelect = "N";
                            ShowHierachyandApplNo(BaseAgency, BaseDept, BaseProg, BaseYear, BaseApplicationNo, string.Empty);
                            pnlContainer.BackgroundImage = null;

                            if (!e.Item.Text.Contains("Dash Board"))
                                MainmenuAddTab();

                        }
                        else
                        {
                            _defaultHierchyform = true;
                            if (!e.Item.Text.Contains("Dash Board"))
                                MainmenuAddTab();
                            //AdvancedMainMenuSearch advancedMainMenuSearch = new AdvancedMainMenuSearch(this, false, true);
                            //advancedMainMenuSearch.FormClosed += new Form.FormClosedEventHandler(On_ADV_SerachFormClosed);
                            // advancedMainMenuSearch.ShowDialog();
                        }


                    }

                    for (int x = 0; x < NavTabs.Items.Count; x++)
                    {
                        NavTabs.Items[x].Expanded = false;
                        NavTabs.Items[x].Items[0].Expanded = false; // Screens
                        NavTabs.Items[x].Items[1].Expanded = false; // Reports

                        if (NavTabs.Items[x].Name == "nModule_02")
                        {
                            for (int zx = 0; zx < NavTabs.Items[x].Items.Count; zx++)
                            {
                                NavTabs.Items[x].Items[zx].Expanded = false;
                            }
                        }
                    }
                    NavTabs.Items[e.Item.Name].Expanded = true;

                    // Kranthi :: 03/06/2023 :: PCS.docx :: Wants to close all the Menu Module items when user logged in. They want to open on their own.
                    //NavTabs.Items[e.Item.Name].Items[0].Expanded = true; //Screens

                }
                else
                {
                    //TagClass tagClass = null;
                    //tagClass = (TagClass)e.Node.Tag;
                    //OpenContentTab(tagClass);
                    NavTabs.SelectedItem = e.Item;
                    colNavItems.Add(e.Item);
                    if (BusinessModuleID == "01")
                        privlgNavAdmnItems = colNavItems;

                    NavTabs.SelectedItem.Icon = "Resources/Images/MenuIcons/formicon_white.png";
                    this.styleSheet1.SetCssClass(NavTabs.SelectedItem, "NavbarItemSelected");

                    if (!string.IsNullOrEmpty(BaseApplicationNo))
                    {
                        if (BaseTopApplSelect == "Y")
                        {
                            applicationNameControl.Controls[0].Controls["panelNavButtons"].Visible = true;
                        }
                    }
                    pnlContainer.BackgroundImageSource = "Resources\\Images\\blank.png";
                    pnlContainer.BackColor = Color.White;
                    if (e.Item.Tag as PrivilegeEntity != null)
                        ShowForm(e.Item.Tag as PrivilegeEntity);
                    else if (e.Item.Tag.ToString() != "" || e.Item.Tag != null)
                    {
                        //string strOtherType = e.Item.Tag.ToString().Split('_')[0];
                        //if (strOtherType == "CMPNT")
                        //    ShowComponentForms(e.Item.Tag.ToString().Split('_')[1]);
                        ShowComponentForms(e.Item.Tag as OtherApps);

                    }

                    int intTabPagesTotal = ContentTabs.TabPages.Count;
                    if (intTabPagesTotal > 1)
                        btncloseAll.Visible = true;
                }
            }


            /************************/
            if (e.Item.Name.Contains("SCRHead") || e.Item.Name.Contains("RPTHead") || e.Item.Name.Contains("USRRPTMGNTHead") || e.Item.Name.Contains("COMPONENTHead"))
            {

                string[] Items = e.Item.Name.Split('_');
                string setName = Items[0] + "_" + Items[1];

                if (Items[2].ToString() == "SCRHead")
                {
                    e.Item.Expanded = true;
                    NavTabs.Items[setName].Items[setName + "_RPTHead"].Expanded = false;
                    if (NavTabs.Items[setName].Items[setName + "_USRRPTMGNTHead"] != null)
                        NavTabs.Items[setName].Items[setName + "_USRRPTMGNTHead"].Expanded = false;
                    if (NavTabs.Items[setName].Items[setName + "_COMPONENTHead"] != null)
                        NavTabs.Items[setName].Items[setName + "_COMPONENTHead"].Expanded = false;
                }
                if (Items[2].ToString() == "RPTHead")
                {
                    e.Item.Expanded = true;
                    NavTabs.Items[setName].Items[setName + "_SCRHead"].Expanded = false;
                    if (NavTabs.Items[setName].Items[setName + "_USRRPTMGNTHead"] != null)
                        NavTabs.Items[setName].Items[setName + "_USRRPTMGNTHead"].Expanded = false;
                    if (NavTabs.Items[setName].Items[setName + "_COMPONENTHead"] != null)
                        NavTabs.Items[setName].Items[setName + "_COMPONENTHead"].Expanded = false;
                }
                if (Items[2].ToString() == "USRRPTMGNTHead")
                {
                    e.Item.Expanded = true;
                    NavTabs.Items[setName].Items[setName + "_SCRHead"].Expanded = false;
                    NavTabs.Items[setName].Items[setName + "_RPTHead"].Expanded = false;
                    if (NavTabs.Items[setName].Items[setName + "_COMPONENTHead"] != null)
                        NavTabs.Items[setName].Items[setName + "_COMPONENTHead"].Expanded = false;
                }
                if (Items[2].ToString() == "COMPONENTHead")
                {
                    e.Item.Expanded = true;
                    NavTabs.Items[setName].Items[setName + "_SCRHead"].Expanded = false;
                    NavTabs.Items[setName].Items[setName + "_RPTHead"].Expanded = false;
                    if (NavTabs.Items[setName].Items[setName + "_USRRPTMGNTHead"] != null)
                        NavTabs.Items[setName].Items[setName + "_USRRPTMGNTHead"].Expanded = false;
                }

            }
            /************************/
        }
        List<NavigationBarItem> colNavItems = new List<NavigationBarItem>();

        public override void OpenContentTab(TagClass tagClass)
        {
            pnlTabs.Controls.Clear();
            pnlTabs.Controls.Add(ContentTabs);
        }

        public void ShowHierachyandApplNo(string strAgency, string strDept, string strProg, string strYear1, string strApplicationNo, string strAppDisplay)
        {

            CaseMstEntity caseMstEntity = null;
            List<CaseSnpEntity> caseSnpEntity = null;
            string strYear = strYear1;
            string strApplNo = strApplicationNo;

            if (string.IsNullOrEmpty(strYear))
                strYear = "    ";

            string strAgencyName = strAgency + " - " + _model.lookupDataAccess.GetHierachyDescription("1", strAgency, strDept, strProg);
            string strDeptName = strDept + " - " + _model.lookupDataAccess.GetHierachyDescription("2", strAgency, strDept, strProg);
            string strProgName = strProg + " - " + _model.lookupDataAccess.GetHierachyDescription("3", strAgency, strDept, strProg);

            caseMstEntity = _model.CaseMstData.GetCaseMST(strAgency, strDept, strProg, strYear1, strApplNo);
            if (caseMstEntity != null)
            {
                strApplNo = caseMstEntity.ApplNo;
                strYear = caseMstEntity.ApplYr;
                BaseApplicationNo = strApplNo;
                caseSnpEntity = _model.CaseMstData.GetCaseSnpadpyn(strAgency, strDept, strProg, strYear, strApplNo);
            }
            else
            {
                BaseApplicationNo = string.Empty; // null; Modified by Yeswanth on 01052013
            }
            GetApplicantDetails(caseMstEntity, caseSnpEntity, strAgencyName, strDeptName, strProgName, strYear.ToString(), string.Empty, strAppDisplay);
        }
        public override void GetApplicantDetails(CaseMstEntity caseMstEntity, List<CaseSnpEntity> caseSNPEntity, string strAgency, string strDept, string strProgram, string strYear, string strType, string strAppDisplay)
        {
            //this.pnlApplicationHeaderImage.Controls.Clear();            
            // applicationDetailsControl = new ApplicationDetailsControl(this);
            //this.pnlApplicationHeaderImage.Controls.Add(applicationDetailsControl);
            //this.pnlApplicationHeaderImage.Size = new System.Drawing.Size(315, 90);
            if (strType != "LinkApplicant")
            {

                HierarchyEntity hierachyEntity = _model.HierarchyAndPrograms.GetCaseHierarchyName(strAgency.Substring(0, 2), "**", "**");
                if (hierachyEntity != null)
                {
                    hierachyNamecontrol.lblHierchy.Text = CommonFunctions.GetHTMLHierachyFormat(strAgency, strDept, strProgram, strYear, hierachyEntity.HIERepresentation);
                    BaseHierarchyCnFormat = hierachyEntity.CNFormat.ToString();
                    BaseHierarchyCwFormat = hierachyEntity.CWFormat.ToString();
                }
            }
            // hierachyNamecontrol.lblHierchy.Text = strAgency + "        " + strDept + "        " + strProgram + "        " + strYear;


            BaseApplicationName = string.Empty;                                         // Yeswanth To ClearCommon Text in Header
            applicationNameControl.lblApplicationName.Text = string.Empty;
            applicationNameControl.labelAddress.Text = string.Empty;
            applicationNameControl.txtAppNo.Text = string.Empty;

            // applicationDetailsControl.ClearGridData();

            BaseCaseMstListEntity = null;
            BaseCaseSnpEntity = null;

            if (caseMstEntity != null)  // Yeswanth To Bypass Hierarchies With No Applicants
            {
                List<CaseMstEntity> casmsttemp = new List<CaseMstEntity>();
                casmsttemp.Add(caseMstEntity);
                BaseCaseMstListEntity = casmsttemp;
                BaseCaseSnpEntity = caseSNPEntity;
                if (BaseCaseSnpEntity.Count > 0)
                    BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseCaseMstListEntity[0].FamilySeq)).M_Code = "A";
                // string strFormat = applicationDetailsControl.FillGridData(caseMstEntity.ApplAgency.ToString(), caseMstEntity.ApplDept.ToString(), caseMstEntity.ApplProgram.ToString(), caseMstEntity.ApplYr.ToString(), caseMstEntity.ApplNo.ToString());
                foreach (CaseSnpEntity caseSnp in caseSNPEntity)
                {
                    if (caseSnp.FamilySeq == caseMstEntity.FamilySeq)
                    {
                        BaseApplicationName = LookupDataAccess.GetMemberName(caseSnp.NameixFi, caseSnp.NameixMi, caseSnp.NameixLast, BaseHierarchyCnFormat);
                        string strZipcode = "00000".Substring(0, 5 - caseMstEntity.Zip.Length) + caseMstEntity.Zip;

                        string strPhonedis = string.Empty;
                        string strtelephone = string.Empty;
                        if (caseMstEntity.Area != string.Empty && caseMstEntity.Phone != string.Empty && caseMstEntity.Phone.Length == 7)
                        {
                            strPhonedis = "  Home (" + caseMstEntity.Area + ")" + caseMstEntity.Phone.Substring(0, 3) + "-" + caseMstEntity.Phone.Substring(3, 4);
                        }
                        if (caseMstEntity.CellPhone != string.Empty && caseMstEntity.CellPhone.Trim().Length == 10)
                        {
                            strtelephone = " Cell (" + caseMstEntity.CellPhone.Substring(0, 3) + ")" + caseMstEntity.CellPhone.Substring(3, 3) + "-" + caseMstEntity.CellPhone.Substring(6, 4);
                        }
                        if (strZipcode == "00000")
                            strZipcode = "";
                        //if (strAppDisplay == "Display")
                        //{
                        //    applicationNameControl.lblApplicationName.Text = BaseApplicationName + "     " + caseMstEntity.Hn.Trim() + ' ' + caseMstEntity.Street.Trim() + ' ' + caseMstEntity.Suffix.Trim() + ' ' + caseMstEntity.City.Trim() + ' ' + caseMstEntity.State.Trim() + ' ' + strZipcode + strPhonedis;
                        //    applicationNameControl.txtAppNo.Text = caseMstEntity.ApplNo;
                        //}
                        //else
                        //{
                        //    applicationNameControl.lblApplicationName.Text = "";
                        //    applicationNameControl.txtAppNo.Text = string.Empty;
                        //}

                        if (strAppDisplay == "Display")
                        {
                            if (BaseAgencyControlDetails.SitesData == "1")
                            {
                                string strSite = string.Empty;
                                if (caseMstEntity.Site.Trim() != string.Empty)
                                    strSite = " (" + caseMstEntity.Site.Trim() + ")";
                                applicationNameControl.lblApplicationName.Text = BaseApplicationName + strSite;
                            }
                            else
                            {
                                applicationNameControl.lblApplicationName.Text = BaseApplicationName;
                            }
                            // TODO: Levie remove.
                            //ToolTip toolnewtip = new ToolTip();
                            //toolnewtip.SetToolTip(applicationNameControl.lblApplicationName, caseMstEntity.Hn.Trim() + ' ' + caseMstEntity.Street.Trim() + ' ' + caseMstEntity.Suffix.Trim() + ' ' + caseMstEntity.City.Trim() + ' ' + caseMstEntity.State.Trim() + ' ' + strZipcode + strPhonedis + strtelephone);
                            applicationNameControl.labelAddress.Text = caseMstEntity.Hn.Trim() + ' ' + caseMstEntity.Street.Trim() + ' ' + caseMstEntity.Suffix.Trim() + ' ' + caseMstEntity.City.Trim() + ' ' + caseMstEntity.State.Trim() + ' ' + strZipcode;
                            applicationNameControl.labelPhone.Text = $"{strPhonedis}{strtelephone}";
                            applicationNameControl.txtAppNo.Text = caseMstEntity.ApplNo;
                        }
                        else
                        {
                            applicationNameControl.lblApplicationName.Text = "";
                            ToolTip toolnewtip = new ToolTip();
                            toolnewtip.SetToolTip(applicationNameControl.lblApplicationName, "");
                            applicationNameControl.txtAppNo.Text = string.Empty;
                        }
                        if (BaseTopApplSelect == "Y" || !string.IsNullOrEmpty(BaseApplicationNo.Trim()))
                        {

                            applicationNameControl.Btn_First.Visible = applicationNameControl.BtnP10.Visible = applicationNameControl.BtnPrev.Visible =
                            applicationNameControl.BtnNxt.Visible = applicationNameControl.BtnN10.Visible = applicationNameControl.BtnLast.Visible = true;

                            ProgramDefinitionEntity ProgramDefinition = _model.HierarchyAndPrograms.GetCaseDepadp(BaseAgency, BaseDept, BaseProg);
                            if (ProgramDefinition != null)
                            {
                                if (BaseYear.Trim() != ProgramDefinition.DepYear.Trim())
                                {
                                    applicationNameControl.lblMsg.Text = "** Current Program Year is " + ProgramDefinition.DepYear.Trim() + "... you are not in the Current Program Year **";
                                    applicationNameControl.lblMsg.Visible = true;
                                }
                                else
                                {
                                    applicationNameControl.lblMsg.Visible = false;
                                }
                            }
                        }
                        else
                        {
                            applicationNameControl.Btn_First.Visible = applicationNameControl.BtnP10.Visible = applicationNameControl.BtnPrev.Visible =
                              applicationNameControl.BtnNxt.Visible = applicationNameControl.BtnN10.Visible = applicationNameControl.BtnLast.Visible = false;

                            ProgramDefinitionEntity ProgramDefinition = _model.HierarchyAndPrograms.GetCaseDepadp(BaseAgency, BaseDept, BaseProg);
                            if (ProgramDefinition != null)
                            {
                                if (BaseYear.Trim() != ProgramDefinition.DepYear.Trim())
                                {
                                    applicationNameControl.lblMsg.Text = "** Current Program Year is " + ProgramDefinition.DepYear.Trim() + "... you are not in the Current Program Year **";
                                    applicationNameControl.lblMsg.Visible = true;
                                }
                                else
                                {
                                    applicationNameControl.lblMsg.Visible = false;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                applicationNameControl.Btn_First.Visible = applicationNameControl.BtnP10.Visible = applicationNameControl.BtnPrev.Visible =
                             applicationNameControl.BtnNxt.Visible = applicationNameControl.BtnN10.Visible = applicationNameControl.BtnLast.Visible = false;
            }

        }
        private void MainmenuAddTab()
        {
            MainMenuControl mainMenuControl = new MainMenuControl(this);
            AddContentTab("MainMenu Search", "MainMenu", mainMenuControl);

            //pnlSearchbox.Controls.Add(hierachyNamecontrol);
            pnlCenterbar.Controls.Clear();
            pnlCenterbar.Controls.Add(hierachyNamecontrol);

            hierachyNamecontrol.Dock = Wisej.Web.DockStyle.Fill;
            // hierachyNamecontrol.Location = new System.Drawing.Point(200, 6);
            hierachyNamecontrol.Name = "hierachykeycontrol";
            //hierachyNamecontrol.Size = new System.Drawing.Size(720, 20);

            pnlSearchbox.Controls.Clear();
            //pnlSearchbox.Padding = new  System.Windows.Forms.Padding(25, 0, 135, 0);
            pnlSearchbox.Controls.Add(applicationNameControl);
            applicationNameControl.BringToFront();

            applicationNameControl.Name = "applicationNameControl";
            applicationNameControl.lblApplicationName.Visible = true;
            applicationNameControl.Dock = Wisej.Web.DockStyle.Fill;


            applicationNameControl.Controls[0].Controls["panelNavButtons"].Visible = true;
            //foreach (Control c in applicationNameControl.Controls)
            //{
            //    if (c.Name == "panelNavButtons")
            //        c.Visible = false;
            //}

            // ((Panel)fc.Ctrl(fc.TheForm("ApplicationNameControl"), "panelNavButtons")).Visible = false;
        }
        public void AddContentTab(string title, string name, UserControl baseUserControl)
        {
            isForm = "N";
            if (ContentTabs.TabPages.Count == 0)
            {
                pnlTabs.Controls.Clear();
                pnlTabs.Controls.Add(ContentTabs);
                ContentTabs.Dock = DockStyle.Fill;
            }
            CurrentTabPage = null;
            try
            {
                for (int iContentTabs = 0; iContentTabs < ContentTabs.TabPages.Count; iContentTabs++)
                {
                    string tagItem = ContentTabs.TabPages[iContentTabs].Tag.ToString();
                    if (tagItem == null) { continue; }
                    if (tagItem == name && !string.IsNullOrEmpty(name))
                    {
                        CurrentTabPage = ContentTabs.TabPages[iContentTabs];
                        if (CurrentTabPage.Text == title)
                        {
                            ContentTabs.SelectedTab = CurrentTabPage;
                            break;
                        }
                    }
                }

                if (CurrentTabPage == null)
                {
                    CurrentTabPage = new TabPage();
                    CurrentTabPage.ShowCloseButton = name != "MainMenu";
                    this.ContentTabs.Controls.Add(CurrentTabPage);
                    CurrentTabPage.Controls.Add(baseUserControl);
                    baseUserControl.Dock = DockStyle.Fill;
                    CurrentTabPage.Name = name + ContentTabs.TabPages.Count.ToString();
                    CurrentTabPage.Tag = name;
                    CurrentTabPage.TabIndex = 0;
                    ContentTabs.SelectedIndexChanged -= OnContentTabsSelectedIndexChanged;
                    CurrentTabPage.Text = title;
                    CurrentTabPage.BackColor = System.Drawing.Color.White;
                    ContentTabs.SelectedTab = CurrentTabPage;


                    ContentTabs.SelectedIndexChanged += OnContentTabsSelectedIndexChanged;
                    ContentTabs.TabClosed += ContentTabs_TabClosed;
                }


                //BaseUserControl baseUserControl1 = GetBaseUserControl();
                //if (baseUserControl1 != null)
                //{
                //    MainToolbar = baseUserControl1.ToolbarMnustrip();
                //    baseUserControl1.PopulateToolbar(MainToolbar);
                //}
            }
            catch (Exception ex)
            {
                //
            }
        }


        private void ContentTabs_TabClosed(object sender, TabControlEventArgs e)
        {
            //string strMsg = "" + e.TabPage.Name + " form closed";
            //AlertBox.Show(strMsg);
            if (BusinessModuleID == "01")
            {
                this.pnlContainer.BackgroundImageSource = "Resources\\Images\\11-01-01.jpg";
                this.pnlContainer.BackgroundImageLayout = Wisej.Web.ImageLayout.Stretch;
                int intTabPagesTotal = ContentTabs.TabPages.Count;
                if (intTabPagesTotal == 0)
                {
                    btncloseAll.Visible = false;
                    NavTabs.SelectedItem = null;
                }
            }
            else
            {
                pnlContainer.BackgroundImageSource = "Resources\\Images\\blank.png";
                pnlContainer.BackColor = Color.White;
                int intTabPagesTotal = ContentTabs.TabPages.Count;
                if (intTabPagesTotal == 1)
                {
                    btncloseAll.Visible = false;
                    NavTabs.SelectedItem = null;
                    applicationNameControl.Controls[0].Controls["panelNavButtons"].Visible = true;
                }
            }

        }

        void setDefaultIcons()
        {
            foreach (NavigationBarItem nvItem in NavTabs.Items)
            {

                foreach (NavigationBarItem nvItem_src in nvItem.Items[0].Items)
                {
                    nvItem_src.Icon = "Resources/Images/MenuIcons/formicon.png";
                    this.styleSheet1.SetCssClass(nvItem_src, "NavbarItem");
                }
                foreach (NavigationBarItem nvItem_rpt in nvItem.Items[1].Items)
                {
                    nvItem_rpt.Icon = "Resources/Images/MenuIcons/formicon.png";
                    this.styleSheet1.SetCssClass(nvItem_rpt, "NavbarItem");
                }

                //Kranthi :: 03/01/2023:: User report maintenance 
                if (nvItem.Items.Count > 2)
                {
                    foreach (NavigationBarItem nvItem_userrpt in nvItem.Items[2].Items)
                    {
                        nvItem_userrpt.Icon = "Resources/Images/MenuIcons/formicon.png";
                        this.styleSheet1.SetCssClass(nvItem_userrpt, "NavbarItem");
                    }
                }

                //Kranthi :: 07/10/2024:: Components 
                if (nvItem.Items.Count == 4)
                {
                    foreach (NavigationBarItem nvItem_compnt in nvItem.Items[3].Items)
                    {
                        nvItem_compnt.Icon = "Resources/Images/MenuIcons/formicon.png";
                        this.styleSheet1.SetCssClass(nvItem_compnt, "NavbarItem");
                    }
                }
            }
        }
        private void OnContentTabsSelectedIndexChanged(object sender, EventArgs e)
        {
            setDefaultIcons();

            UserEntity userInfo = Captain<UserEntity>.Session[Consts.SessionVariables.UserProfile];
            try
            {
                if (BusinessModuleID != "01")
                {
                    MainToolbar.Buttons.Clear();  // Rao
                }

                int intSelect = ContentTabs.SelectedIndex;
                if (intSelect >= 0 || BusinessModuleID == "01")  // (intSelect > 0 )  No need to check the condition - Changed by Yeswanth on 02/04/2013
                {
                    List<NavigationBarItem> SeltempNavbar = new List<NavigationBarItem>();
                    CurrentTabPage = (TabPage)ContentTabs.TabPages[intSelect];
                    if (CurrentTabPage.Tag.ToString().Contains("CMPNT_"))
                    {
                        SeltempNavbar = colNavItems.FindAll(x => x.Tag.ToString() == "OtherApps");
                        if (SeltempNavbar.Count > 0)
                        {
                            SeltempNavbar = SeltempNavbar.FindAll(x => ((OtherApps)x.Tag)._Code.ToString() == CurrentTabPage.Tag.ToString());
                        }
                    }
                    else
                    {
                        SeltempNavbar = colNavItems.FindAll(x => x.Tag.ToString() != "OtherApps");
                        SeltempNavbar = SeltempNavbar.FindAll(x => ((PrivilegeEntity)x.Tag).Program.ToString() == CurrentTabPage.Tag.ToString());
                    }

                    // List<NavigationBarItem> SeltempNavbar = colNavItems.FindAll(x => ((PrivilegeEntity)x.Tag).Program.ToString() == CurrentTabPage.Tag.ToString());
                    if (SeltempNavbar.Count > 0)
                    {
                        NavTabs.SelectedItem = SeltempNavbar[0];
                        NavTabs.SelectedItem.Icon = "Resources/Images/MenuIcons/formicon_white.png";
                        this.styleSheet1.SetCssClass(NavTabs.SelectedItem, "NavbarItemSelected");
                    }
                    else
                        NavTabs.SelectedItem = null;


                    if (CurrentTabPage.Tag == null)
                    {
                        MainToolbar.Buttons.Clear();
                        return;
                    }


                    string tagClass = null;
                    if (CurrentTabPage.Tag is string)
                    {
                        tagClass = (string)CurrentTabPage.Tag;
                    }

                    if (tagClass != null)
                    {
                        // MainToolbar.Buttons.Clear();


                        //BaseUserControl baseUserControl = GetBaseUserControl();

                        //if (baseUserControl != null)
                        //{
                        //    MainToolbar = baseUserControl.ToolbarMnustrip();
                        //    baseUserControl.PopulateToolbar(MainToolbar);
                        //}

                        if (BusinessModuleID != "01")
                        {
                            if (CurrentTabPage.Tag.ToString() == "MainMenu")
                                applicationNameControl.Controls[0].Controls["panelNavButtons"].Visible = true;
                            else
                                applicationNameControl.Controls[0].Controls["panelNavButtons"].Visible = true;
                        }
                        RefreshOpenedAdminPages();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void ShowComponentForms(OtherApps _otherApps)
        {
            string _type = _otherApps._Code.Split('_')[0].ToString();
            string _Code = _otherApps._Code.Split('_')[1].ToString();
            string _Desc = _otherApps._Desc;
            PrivilegeEntity _prvEntity = _otherApps._PrivilegeEntity;

            // client tracking
            if (!string.IsNullOrEmpty(BaseApplicationNo))
            {
                if (BaseTopApplSelect == "Y")
                {
                    if (!UserProfile.Components.Contains("None"))
                    {

                        //HSS00134Components oHSS00134Components = new HSS00134Components(this, _Code, _Desc, _prvEntity);
                        //oHSS00134Components.StartPosition = FormStartPosition.CenterScreen;
                        //oHSS00134Components.ShowDialog();

                        //HSS00134CompControl HSS00134Comp = new HSS00134CompControl(this, _Code, _Desc, _prvEntity);
                        //AddContentTab(_Desc, "CMPNT_" + _Code, HSS00134Comp);
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay("You can't Access any Component \n Please Contact Your System Administrator");
                    }
                }
                else
                    CommonFunctions.MessageBoxDisplay(Consts.Messages.AplicantSelectionMsg);
            }
            else
            {
                CommonFunctions.MessageBoxDisplay(Consts.Messages.Applicantdoesntexist);
            }
        }

        string isForm = "Y";
        private void ShowForm(PrivilegeEntity privilegeEntity)
        {
            string Ishelp = "N"; isForm = "Y";

            string nodeLabel = string.Empty;
            if (privilegeEntity == null) return;
            switch (privilegeEntity.Program.Trim())
            {
                #region ADMINISTRATION

                #region SCREENS

                #region Assign User Privilages
                case "ADMN0005": //Assign User Privilages
                    UserListControl ulc = new UserListControl(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, ulc);
                    break;
                #endregion

                #region Agency Table Codes
                case "ADMN0006": //Agency Table Codes
                    AgencyTableControl agencyTabelControl = new AgencyTableControl(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, agencyTabelControl);
                    break;
                #endregion

                #region Hierarchy Definition
                case "ADMN0009": //Hierarchy Definition
                    HierarchyDefinitionControl hierarchyDefinitionControl = new HierarchyDefinitionControl(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, hierarchyDefinitionControl);
                    break;
                #endregion

                #region Master Poverty Guidelines 
                case "ADMN0010": //Master Poverty Guidelines 
                    MasterPoverityGuidelineControl masterPovertyControl = new MasterPoverityGuidelineControl(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, masterPovertyControl);
                    break;
                #endregion

                #region Agency Control file Maintanence
                case "ADMN0012": //Agency Control file Maintanence
                    AgencyHierarchyForm agencyForm = new AgencyHierarchyForm(this, "ADMN0012", "AGENCY", "00", privilegeEntity);
                    agencyForm.StartPosition = FormStartPosition.CenterScreen;
                    agencyForm.ShowDialog();
                    break;
                #endregion

                #region Zipcode
                case "ADMN0013": //Zipcode
                    ADMN0013 ADMN0013 = new ADMN0013(this, privilegeEntity);
                    ////AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, zipCodeControl);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, ADMN0013);
                    break;
                #endregion

                #region Incomplete Intake Control
                case "ADMNCONT":    //Incomplete Intake Control
                    AdminScreenControls ADMNCONT = new AdminScreenControls(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, ADMNCONT);
                    break;
                #endregion

                #region Field Control Maintenance
                case "CASE0008":    //Field Control Maintenance
                    FLDCNTLMaintenanceControl FLDControl = new FLDCNTLMaintenanceControl(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, FLDControl);
                    break;
                #endregion

                #region Eligibilty Criteria
                case "CASE0009": //Eligibilty Criteria
                    Case0009Control Case0009 = new Case0009Control(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, Case0009);
                    break;
                #endregion

                #region Agency Referral Database Screen
                case "CASE0011": // Agency Referral Database Screen
                    Case2011Control AgencyReferal = new Case2011Control(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, AgencyReferal);
                    break;
                #endregion

                #region Site and room maintenance
                case "ADMN0015":    //Site and room maintenance
                    ADMN0015Control CaseSite = new ADMN0015Control(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, CaseSite);
                    break;
                #endregion

                #region SP Admin Screen
                case "ADMN0020":    //SP Admin Screen
                    ADMN0020control ADMN0020 = new ADMN0020control(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, ADMN0020);
                    break;
                #endregion

                #region Contact and Service Activity Custom Questions
                case "ADMN0022": //Contact and Service Activity Custom Questions
                    ADMN0022Control ADMN0022 = new ADMN0022Control(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, ADMN0022);
                    break;
                #endregion

                //#region Service Posting Date Control
                //case "ADMN0024":
                //    ADMN0024Control Admn0024control = new ADMN0024Control(this, privilegeEntity);
                //    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, Admn0024control);
                //    break;
                //#endregion

                //#region Program Report Control Table (SSBG)
                //case "ADMN0025":    //Program Report Control Table (SSBG)
                //    SSBGParams_Control SSBGParamscontrol = new SSBGParams_Control(this, privilegeEntity);
                //    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, SSBGParamscontrol);
                //    break;
                //#endregion

                #region Vendor Maintenance
                case "TMS00009":   //Vendor Maintenance
                    Vendor_Maintainance Vendor = new Vendor_Maintainance(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, Vendor);
                    break;
                #endregion

                //#region Master Table Maintenance
                //case "ADMN0016":    //Master Table Maintenance
                //    CriticalActivity ADMN0016 = new CriticalActivity(this, privilegeEntity);
                //    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, ADMN0016);
                //    break;
                //#endregion

                #region Program Definition
                case "CASE0007":    //Program Definition
                    ProgramDefinition programdefinitioncontrol = new ProgramDefinition(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, programdefinitioncontrol);
                    break;
                #endregion

                #region HouseholdID Audit and Review
                case "FIXFAMID":    //HouseholdID Audit and Review
                    FIXFAMILYIDForm fixfamidForm = new FIXFAMILYIDForm(this, privilegeEntity, string.Empty);
                    fixfamidForm.StartPosition = FormStartPosition.CenterScreen;
                    fixfamidForm.ShowDialog();
                    break;
                #endregion

                #region ClientID Audit and Review
                case "FIXSSNUM":    //ClientID Audit and Review
                    FIXSSNFORM fixSSNForm = new FIXSSNFORM(this, privilegeEntity);
                    fixSSNForm.StartPosition = FormStartPosition.CenterScreen;
                    fixSSNForm.ShowDialog();
                    break;
                #endregion


                //#region PIP Control
                //case "PIPADMIN":
                //    try
                //    {

                //        SqlConnection connect = new SqlConnection();
                //        connect.ConnectionString = this.BaseLeanDataBaseConnectionString;
                //        connect.Open();
                //        if (connect.State == ConnectionState.Open)
                //        {
                //            connect.Close();
                //            PIPAdmin pipAdminForm = new PIPAdmin(this, privilegeEntity, string.Empty, "CUSTOM");
                //            pipAdminForm.StartPosition = FormStartPosition.CenterScreen;
                //            pipAdminForm.ShowDialog();
                //        }

                //        else
                //        {
                //            MessageBox.Show("Connection issue with Server \n Please contact CAPSYSTEMS INC", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        }
                //    }
                //    catch (Exception)
                //    {

                //        MessageBox.Show("Connection issue with Server \n Please contact CAPSYSTEMS INC", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    }

                //    break;
                //#endregion

                #region Dashboard
                case "RPREPOR3":
                    ReportGridControl1 RPREPOR3 = new ReportGridControl1(this, privilegeEntity, "RPREPOR3");
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, RPREPOR3);

                    //DashboardControl RPREPOR3 = new DashboardControl(this, privilegeEntity, "RPREPOR3");
                    //AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, RPREPOR3);
                    break;

                #endregion

                #region CEAP Control
                case "ADMN0027":            // CEAP Control
                    ADMN0027Control Admn0027control = new ADMN0027Control(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, Admn0027control);
                    break;
                #endregion

                #region Print Application Control
                case "ADMN0031":    //Print Application Control
                    PrintApplicationControl ADMN0031 = new PrintApplicationControl(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, ADMN0031);
                    break;
                #endregion



                #endregion

                #region REPORTS

                #region Master Tables List
                case "ADMNB001": // Report :: Master Tables List
                    ADMNB001 ADMNB001Form = new ADMNB001(this, privilegeEntity);
                    ADMNB001Form.StartPosition = FormStartPosition.CenterScreen;
                    ADMNB001Form.ShowDialog();
                    break;
                #endregion

                #region User tree structure
                case "ADMNB002":    // Report:: User tree structure
                    ADMNB002 ADMNB002Form = new ADMNB002(this, privilegeEntity);
                    ADMNB002Form.StartPosition = FormStartPosition.CenterScreen;
                    ADMNB002Form.ShowDialog();
                    break;
                #endregion

                #region Image Types Report
                case "ADMNB005":    // Report:: Image Types Report
                    try
                    {

                        if ((BusinessModuleID == "08" || BusinessModuleID=="03") && BaseAgencyControlDetails.State=="CT")
                        {
                            //connect.Close();
                            ADMNB005 ADMNB005_Report = new ADMNB005(this, privilegeEntity);
                            ADMNB005_Report.StartPosition = FormStartPosition.CenterScreen;
                            ADMNB005_Report.ShowDialog();
                        }
                        else if (BusinessModuleID == "01")
                        {
                            SqlConnection connect = new SqlConnection();
                            connect.ConnectionString = this.BaseLeanDataBaseConnectionString;
                            connect.Open();
                            if (connect.State == ConnectionState.Open)
                            {
                                connect.Close();
                                ADMNB005 ADMNB005_Report = new ADMNB005(this, privilegeEntity);
                                ADMNB005_Report.StartPosition = FormStartPosition.CenterScreen;
                                ADMNB005_Report.ShowDialog();
                            }
                        }
                        else if (BaseAgencyControlDetails.PIPSwitch != "N" && BaseAgencyControlDetails.PIPSwitch != string.Empty)
                        {


                            SqlConnection connect = new SqlConnection();
                            connect.ConnectionString = this.BaseLeanDataBaseConnectionString;
                            connect.Open();
                            if (connect.State == ConnectionState.Open)
                            {
                                connect.Close();
                                ADMNB005 ADMNB005_Report = new ADMNB005(this, privilegeEntity);
                                ADMNB005_Report.StartPosition = FormStartPosition.CenterScreen;
                                ADMNB005_Report.ShowDialog();
                            }

                            else
                            {
                                MessageBox.Show("Connection issue with Server \n Please contact CAPSYSTEMS INC", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }

                        else
                        {
                            AlertBox.Show("Connection issue with Server \n Please contact CAPSYSTEMS INC", MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception)
                    {

                        AlertBox.Show("Connection issue with Server \n Please contact CAPSYSTEMS INC", MessageBoxIcon.Warning);
                    }
                    break;
                #endregion

                
                #endregion

                #endregion

                #region CASEMANAGEMENT

                #region SCREENS


                #region Client Intake
                case "CASE2001":
                    if (!string.IsNullOrEmpty(BaseApplicationNo))
                    {
                        _privilegeEntity = privilegeEntity;
                        if (BaseTopApplSelect == "Y")
                        {
                            Case3001Control clientIntakeControl = new Case3001Control(this, privilegeEntity);
                            // ClientIntakeControl clientIntakeControl = new ClientIntakeControl(this, privilegeEntity);
                            AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, clientIntakeControl);
                        }
                        else
                            CommonFunctions.MessageBoxDisplay(Consts.Messages.AplicantSelectionMsg);
                    }
                    else

                        CommonFunctions.MessageBoxDisplay(Consts.Messages.AplicantSelectionMsg);
                    break;
                #endregion

                #region Client Follow-up On
                case "CASE0028":        // Client Follow-up On
                    if (!string.IsNullOrEmpty(BaseApplicationNo))
                    {
                        if (BaseTopApplSelect == "Y")
                        {
                            CASE0028Control case0028 = new CASE0028Control(this, privilegeEntity);
                            AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, case0028);
                        }
                        else
                            CommonFunctions.MessageBoxDisplay(Consts.Messages.AplicantSelectionMsg);
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay(Consts.Messages.Applicantdoesntexist);
                    }
                    break;
                #endregion

                #region Client Follw-up On Search Tool
                case "CASE0027":    //Client Follw-up On Search Tool
                    CASE0027Control case0027 = new CASE0027Control(this, privilegeEntity, string.Empty);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, case0027);
                    break;
                #endregion

                

                #endregion

                //#region REPORTS

                //#region ROMA Individual/Household Characteristics
                //case "RNGB0004": //ROMA Individual/Household Characteristics
                //    RNGB0004Form RNGb0004Form = new RNGB0004Form(this, privilegeEntity);
                //    RNGb0004Form.StartPosition = FormStartPosition.CenterScreen;
                //    RNGb0004Form.ShowDialog();
                //    break;
                //#endregion

                //#region ROMA Outcome Indicators
                //case "RNGB0014":    //ROMA Outcome Indicators
                //    RNGB0014Form RNGb0014Form = new RNGB0014Form(this, privilegeEntity);
                //    RNGb0014Form.StartPosition = FormStartPosition.CenterScreen;
                //    RNGb0014Form.ShowDialog();
                //    break;
                //#endregion

                //#region Roma Services
                //case "RNGS0014":        // Report:: Roma Services
                //    RNGS0014 RNGS0014 = new RNGS0014(this, privilegeEntity);
                //    RNGS0014.StartPosition = FormStartPosition.CenterScreen;
                //    RNGS0014.ShowDialog();
                //    break;
                //#endregion

                //#region Program Service and Outcomes Report
                //case "RNGB0005": // Program Service and Outcomes Report
                //    RNGB0005Form RNGb0005Form = new RNGB0005Form(this, privilegeEntity);
                //    RNGb0005Form.StartPosition = FormStartPosition.CenterScreen;
                //    RNGb0005Form.ShowDialog();
                //    break;
                //#endregion

                //#region Appointment Schedule Report
                //case "APPTB001": //Appointment Schedule Report
                //    APPTB001_Report APPTB001Form = new APPTB001_Report(this, privilegeEntity);
                //    APPTB001Form.StartPosition = FormStartPosition.CenterScreen;
                //    APPTB001Form.ShowDialog();
                //    break;
                //#endregion

                //#region Funnel Report
                //case "CASB0007":    //Funnel Report
                //    CASB4007_FunnelReport Funnel_Form = new CASB4007_FunnelReport(this, privilegeEntity);
                //    Funnel_Form.StartPosition = FormStartPosition.CenterScreen;
                //    Funnel_Form.ShowDialog();
                //    break;
                //#endregion

                //#region Customer Intake Quality Control Report
                //case "CASB0008":    // Customer Intake Quality Control Report
                //    CASB0008 CASB0008_Report = new CASB0008(this, privilegeEntity);
                //    CASB0008_Report.StartPosition = FormStartPosition.CenterScreen;
                //    CASB0008_Report.ShowDialog();
                //    break;
                //#endregion

                //#region SAL/CAL Report
                //case "CASB0009":    //SAL/CAL Report
                //    CASB0009_Report CASB0009_Report = new CASB0009_Report(this, privilegeEntity);
                //    CASB0009_Report.StartPosition = FormStartPosition.CenterScreen;
                //    CASB0009_Report.ShowDialog();
                //    break;
                //#endregion

                //#region Agency Width Activity Report
                //case "CASB0013":    // Agency Width Activity Report
                //    CASB0013Form caseb0013form = new CASB0013Form(this, privilegeEntity);
                //    caseb0013form.StartPosition = FormStartPosition.CenterScreen;
                //    caseb0013form.ShowDialog();
                //    break;
                //#endregion

                //#region Program Statistical Report (SSBG)
                //case "CASB0014":    //Program Statistical Report (SSBG)
                //    SSBG_Report SSBGRep = new SSBG_Report(this, privilegeEntity);
                //    SSBGRep.StartPosition = FormStartPosition.CenterScreen;
                //    SSBGRep.ShowDialog();
                //    break;
                //#endregion

                //#region Ranking/Risk Assessment Report
                //case "CASB0530": //Ranking/Risk Assessment Report
                //    Casb2530Form casb2530form = new Casb2530Form(this, privilegeEntity);
                //    casb2530form.StartPosition = FormStartPosition.CenterScreen;
                //    casb2530form.ShowDialog();
                //    break;
                //#endregion

                //#region Client Follow-up Report
                //case "CASB0028": //Ranking/Risk Assessment Report
                //    CASB0028_Report oCASB0028_Report = new CASB0028_Report(this, privilegeEntity);
                //    oCASB0028_Report.StartPosition = FormStartPosition.CenterScreen;
                //    oCASB0028_Report.ShowDialog();
                //    break;
                //#endregion

                //#region Invoice Upload Report
                //case "CASB0030": //Invoice Upload Report
                //    CASB0030_Report oCASB0030_Report = new CASB0030_Report(this, privilegeEntity);
                //    oCASB0030_Report.StartPosition = FormStartPosition.CenterScreen;
                //    oCASB0030_Report.ShowDialog();
                //    break;
                //#endregion

                //#region APC XML Feed
                //case "CASB0023":    //APC XML Feed
                //    CASB0023 CASB0023ReportForm = new CASB0023(this, privilegeEntity);
                //    CASB0023ReportForm.StartPosition = FormStartPosition.CenterScreen;
                //    CASB0023ReportForm.ShowDialog();
                //    break;
                //#endregion

                //#region Dimension Score Report
                //case "DIMSCORE":    // Dimension Score Report
                //    DIMSCOREREPORT DIMSCOREREPORTForm = new DIMSCOREREPORT(this, privilegeEntity);
                //    DIMSCOREREPORTForm.StartPosition = FormStartPosition.CenterScreen;
                //    DIMSCOREREPORTForm.ShowDialog();
                //    break;
                //#endregion

                //#region Matrix Scales Measures Report
                //case "MATB1002":
                //    MATB1002_Form MATB1002Form = new MATB1002_Form(this, privilegeEntity);
                //    MATB1002Form.StartPosition = FormStartPosition.CenterScreen;
                //    MATB1002Form.ShowDialog();
                //    break;
                //#endregion

                //#region PPR Report
                //case "RNGB0006": // PPR Report
                //    RNGB0006Form RNGb0006Form = new RNGB0006Form(this, privilegeEntity);
                //    RNGb0006Form.StartPosition = FormStartPosition.CenterScreen;
                //    RNGb0006Form.ShowDialog();
                //    break;
                //#endregion

                //#region Smart Choice Feed
                //case "CASB0016":    // Smart Choice Feed
                //    CASB0016_Report casb0016form = new CASB0016_Report(this, privilegeEntity);
                //    casb0016form.StartPosition = FormStartPosition.CenterScreen;
                //    casb0016form.ShowDialog();
                //    break;
                //#endregion

                #endregion

                #region CEAP PROGRAM

                #region SCREENS
                #region Budget Maintenance
                case "CASE0026":
                    CASE0026Control ADMN0026 = new CASE0026Control(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, ADMN0026);
                    break;
                #endregion

                #region Benifit Maintenance & Usage Posting
                case "CASE0016":
                    if (!string.IsNullOrEmpty(BaseApplicationNo))
                    {
                        if (BaseTopApplSelect == "Y")
                        {
                            CASE0016_Control CASE4006_Usage = new CASE0016_Control(this, privilegeEntity);
                            AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, CASE4006_Usage);

                        }
                        else
                            CommonFunctions.MessageBoxDisplay(Consts.Messages.AplicantSelectionMsg);
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay(Consts.Messages.Applicantdoesntexist);
                    }
                    break;
                #endregion

                #region Service Payment Adjustment

                case "CASE0021":
                    if (!string.IsNullOrEmpty(BaseApplicationNo))
                    {
                        if (BaseTopApplSelect == "Y")
                        {
                            CASE0021Control CASE0021cont = new CASE0021Control(this, privilegeEntity);
                            AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, CASE0021cont);
                        }
                        else
                            CommonFunctions.MessageBoxDisplay(Consts.Messages.AplicantSelectionMsg);
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay(Consts.Messages.Applicantdoesntexist);
                    }
                    break;
                #endregion

                #region Signature Documents HUB
                //case "CEAP0010":
                //    CEAP0010_Control CEAP0010 = new CEAP0010_Control(this, privilegeEntity);
                //    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, CEAP0010);
                //    break;
                //#endregion

                #endregion

                //#region REPORTS

                //#region Bundling
                //case "CASB0019":    //Pledge Sheet\Bundling
                //    CASB0019_Bundling casb0019form = new CASB0019_Bundling(this, privilegeEntity);
                //    casb0019form.StartPosition = FormStartPosition.CenterScreen;
                //    casb0019form.ShowDialog();
                //    break;
                //#endregion

                //#region Usage Report
                //case "CASB0017":    //Usage Report
                //    CASB0017_Report casb0017form = new CASB0017_Report(this, privilegeEntity);
                //    casb0017form.StartPosition = FormStartPosition.CenterScreen;
                //    casb0017form.ShowDialog();
                //    break;
                //#endregion

                //#region Request for Payment Process
                //case "CASB0020":    //Request for Payment Process
                //    CASB0020_CheckProcessing casb0020form = new CASB0020_CheckProcessing(this, privilegeEntity);
                //    casb0020form.StartPosition = FormStartPosition.CenterScreen;
                //    casb0020form.ShowDialog();
                //    break;
                //#endregion

                //#region Performance Measures Data
                //case "CEAPB002": //Performance Measures Data
                //    CEAPB002_Report CEAPB002Form = new CEAPB002_Report(this, privilegeEntity);
                //    CEAPB002Form.StartPosition = FormStartPosition.CenterScreen;
                //    CEAPB002Form.ShowDialog();
                //    break;
                //#endregion

                //#region Invoice Commitment Report
                //case "CEAPB003": //Invoice Commitment Report
                //    CEAPB003_Invoice_Commitment CEAPB003Form = new CEAPB003_Invoice_Commitment(this, privilegeEntity);
                //    CEAPB003Form.StartPosition = FormStartPosition.CenterScreen;
                //    CEAPB003Form.ShowDialog();
                //    break;
                //#endregion

                //#region Ser/Pay Adjustment Report
                //case "CASB0021":    //Ser/Pay Adjustment Report
                //    CASB0021_ReportForm CASB0021Form = new CASB0021_ReportForm(this, privilegeEntity);
                //    CASB0021Form.StartPosition = FormStartPosition.CenterScreen;
                //    CASB0021Form.ShowDialog();
                //    break;
                //#endregion

                //#region Funding Source Report
                //case "TMSB0034":    //Funding Source Report
                //    TMSB0034_ReportForm tMSB0034_ReportForm = new TMSB0034_ReportForm(this, privilegeEntity);
                //    tMSB0034_ReportForm.StartPosition = FormStartPosition.CenterScreen;
                //    tMSB0034_ReportForm.ShowDialog();
                //    break;
                //#endregion

                //#region CEAP Annual and Quarterly Report
                //case "TMSB0038":
                //    TMSB0038_Report tmsb0038_report = new TMSB0038_Report(this, privilegeEntity);
                //    tmsb0038_report.StartPosition = FormStartPosition.CenterScreen;
                //    tmsb0038_report.ShowDialog();
                //    break;
                //#endregion

                //#region Vendor List
                //case "TMSB6B01":    //Vendor List
                //    TMSB6B01_Report Vendor_list = new TMSB6B01_Report(this, privilegeEntity);
                //    Vendor_list.StartPosition = FormStartPosition.CenterScreen;
                //    Vendor_list.ShowDialog();
                //    break;
                //#endregion

                //#region Adhoch Report
                //case "CASB0012":
                //    CASB2012_AdhocForm AdhocForm = new CASB2012_AdhocForm(this, privilegeEntity);
                //    AdhocForm.StartPosition = FormStartPosition.CenterScreen;
                //    AdhocForm.ShowDialog();
                //    break;
                //#endregion

                //#endregion

                #endregion

                #endregion

                #region Housing

                #region Screens


                #endregion

                #region Reports


                #endregion

                #endregion


                default:
                    HelpForm2 helpForm = new HelpForm2();
                    helpForm.Text = privilegeEntity.PrivilegeName;
                    helpForm.StartPosition = FormStartPosition.CenterScreen;
                    //FrmUploadFtp helpForm = new FrmUploadFtp();
                    helpForm.ShowDialog();
                    Consts.Messages.UserCreatedSuccesssfully.DisplayFirendlyMessage(Captain.Common.Exceptions.ExceptionSeverityLevel.Information);
                    Ishelp = "Y";
                    break;
            }

            if (privilegeEntity.screenType == "SCREEN" && privilegeEntity.Program != "ADMN0012")
            {
                if (Ishelp == "N" && isForm == "N")
                    btncloseAll.Visible = true;
            }



            #region commented menu code
            //  PrivilegeEntity privilegeEntity = _Code as PrivilegeEntity;
            //if (privilegeEntity == null) return;
            /*
            switch (privilegeEntity.Program.Trim())
            {
                case "ADMN0005": //Users
                    UserListControl ulc = new UserListControl(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, ulc);
                    break;
                case "ADMN0006": //Agency Table Codes
                    AgencyTableControl agencyTabelControl = new AgencyTableControl(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, agencyTabelControl);
                    break;
                case "ADMN0010": //Master Poverty Guidelines 
                    //MasterPovertyGuidelinesForm masterPovertyForm = new MasterPovertyGuidelinesForm(this, privilegeEntity);
                    //masterPovertyForm.ShowDialog();                      
                    MasterPoverityGuidelineControl masterPovertyControl = new MasterPoverityGuidelineControl(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, masterPovertyControl);
                    break;
                case "ADMN0013": //Zipcode
                    ADMN0013 ADMN0013 = new ADMN0013(this, privilegeEntity);
                    ////AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, zipCodeControl);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, ADMN0013);
                    break;
                case "CASE2002":
                    if (!string.IsNullOrEmpty(BaseApplicationNo))
                    {
                        _privilegeEntity = privilegeEntity;
                        if (BaseTopApplSelect == "Y")
                        {
                            Case3001Control clientIntakeControl = new Case3001Control(this, privilegeEntity);
                            // ClientIntakeControl clientIntakeControl = new ClientIntakeControl(this, privilegeEntity);
                            AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, clientIntakeControl);
                        }
                        else
                            CommonFunctions.MessageBoxDisplay(Consts.Messages.AplicantSelectionMsg);
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay(Consts.Messages.Applicantdoesntexist);
                    }
                    break;
                case "TMSB4016":
                    TMSB4601 TMSBForm = new TMSB4601();
                    TMSBForm.ShowDialog();
                    break;
                case "CASE0008":
                    FLDCNTLMaintenanceControl FLDControl = new FLDCNTLMaintenanceControl(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, FLDControl);
                    break;
                case "REPMNT20":
                    PdfListForm pdfListForm = new PdfListForm(this, privilegeEntity, true);
                    pdfListForm.ShowDialog();
                    break;
                case "TMS00300":
                    TMS00300Control TMS00300 = new TMS00300Control(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, TMS00300);
                    break;
               
                case "ADMNB001":
                    ADMNB001 ADMNB001Form = new ADMNB001(this, privilegeEntity);
                    ADMNB001Form.ShowDialog();
                    break;
                case "TMS00310":
                    TMS00310Control TMS00310 = new TMS00310Control(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, TMS00310);
                    break;
                case "CASB2012":
                    CASB2012Control CASB2012 = new CASB2012Control(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, CASB2012);
                    break;
               
               
                
                case "CASE0009":
                    Case0009Control Case0009 = new Case0009Control(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, Case0009);
                    break;
                case "MAT00001":
                    MAT00001Control MAT00001 = new MAT00001Control(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, MAT00001);
                    break;
                case "MAT00002":
                    MAT00002Control MAT00002 = new MAT00002Control(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, MAT00002);
                    break;
   
                case "CASB0004":
                    CASB0004Form Casb0004Form = new CASB0004Form(this, privilegeEntity); ;
                    Casb0004Form.ShowDialog();
                    break;
                
                
               
              


               
                case "STFBLK10":
                    STFBLK10Control staffBulkControl = new STFBLK10Control(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, staffBulkControl);
                    break;          
                case "MATB0002":
                    MATB0002_Form MATB0002Form = new MATB0002_Form(this, privilegeEntity); ;
                    MATB0002Form.ShowDialog();
                    break;
              

                case "HSSB0026":
                    HSSB0026_PIRCounting_From PIR_Counting_Form = new HSSB0026_PIRCounting_From(this, privilegeEntity); ;
                    PIR_Counting_Form.ShowDialog();

                    break;
                case "HSSB0123":
                    HSSB0123 GrowthCharts = new HSSB0123(this, privilegeEntity);
                    GrowthCharts.ShowDialog();
                    break;
                case "ARS00120":
                    if (!string.IsNullOrEmpty(BaseApplicationNo))
                    {
                        _privilegeEntity = privilegeEntity;
                        if (BaseTopApplSelect == "Y")
                        {
                            ARS20120 ARS20120 = new ARS20120(this, privilegeEntity);
                            AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, ARS20120);
                        }
                        else
                            CommonFunctions.MessageBoxDisplay(Consts.Messages.AplicantSelectionMsg);
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay(Consts.Messages.Applicantdoesntexist);
                    }
                    break;
                case "ARS00115":
                    Ars20115Control ARS20115control = new Ars20115Control(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, ARS20115control);
                    break;
                case "ARSB0150":
                    ARSB2150_ReportForm CustStatement = new ARSB2150_ReportForm(this, privilegeEntity);
                    CustStatement.ShowDialog();
                    break;
                case "ARSB0140":
                    ARSB2140_ReportForm custReport = new ARSB2140_ReportForm(this, privilegeEntity);
                    custReport.ShowDialog();
                    break;
                case "ARSB0160":
                    ARSB2160_Report invoice = new ARSB2160_Report(this, privilegeEntity);
                    invoice.ShowDialog();
                    break;
                case "ARSB0120":
                    ARSB2120_ReportForm Billing = new ARSB2120_ReportForm(this, privilegeEntity);
                    Billing.ShowDialog();
                    break;
                case "TMSBLTRB":
                    TMSBLTRB tMSBLTRBReport = new TMSBLTRB(this, privilegeEntity);
                    tMSBLTRBReport.ShowDialog();
                    break;         
                case "ARS00130":
                    if (!string.IsNullOrEmpty(BaseApplicationNo))
                    {
                        _privilegeEntity = privilegeEntity;
                        if (BaseTopApplSelect == "Y")
                        {
                            Ars20130Control ARS20130 = new Ars20130Control(this, privilegeEntity);
                            AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, ARS20130);
                        }
                        else
                            CommonFunctions.MessageBoxDisplay(Consts.Messages.AplicantSelectionMsg);
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay(Consts.Messages.Applicantdoesntexist);
                    }
                    break;
                case "EMS00010":
                    EMS00010Control Ems30010control = new EMS00010Control(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, Ems30010control);
                    break;

                case "EMS00020":
                    if (!string.IsNullOrEmpty(BaseApplicationNo))
                    {
                        _privilegeEntity = privilegeEntity;
                        if (BaseTopApplSelect == "Y")
                        {
                            EMS00020Control ems00020Control = new EMS00020Control(this, privilegeEntity);
                            AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, ems00020Control);
                        }
                        else
                            CommonFunctions.MessageBoxDisplay(Consts.Messages.AplicantSelectionMsg);
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay(Consts.Messages.Applicantdoesntexist);
                    }
                    break;
                case "ADMN0021":
                    Admn0021Form admn0021form = new Admn0021Form(this, privilegeEntity);
                    admn0021form.ShowDialog();
                    break;
                case "EMSB0014":
                    EMSB0014_BudgetReport Budget_report = new EMSB0014_BudgetReport(this, privilegeEntity);
                    Budget_report.ShowDialog();
                    break;
                case "EMSB0003":
                    EMSB0003_PendingCases Pending_report = new EMSB0003_PendingCases(this, privilegeEntity);
                    Pending_report.ShowDialog();
                    break;
                case "EMSB0007":
                    EMSB0007_NoInvoiceReport NoInvoice_report = new EMSB0007_NoInvoiceReport(this, privilegeEntity);
                    NoInvoice_report.ShowDialog();
                    break;
                case "EMSB0010":
                    EMSB0010_FollowUp FolowUp_report = new EMSB0010_FollowUp(this, privilegeEntity);
                    FolowUp_report.ShowDialog();
                    break;
                case "EMSB0012":
                    EMSB0012_Report DeptRej_report = new EMSB0012_Report(this, privilegeEntity);
                    DeptRej_report.ShowDialog();
                    break;
                case "EMSB0018":
                    EMS0018_Report ReFHs_report = new EMS0018_Report(this, privilegeEntity);
                    ReFHs_report.ShowDialog();
                    break;
                case "EMSB0011":
                    EMSB0011_PaidInvoices Paidinvoices_report = new EMSB0011_PaidInvoices(this, privilegeEntity);
                    Paidinvoices_report.ShowDialog();
                    break;
                case "EMSB0017":
                    EMSB3017_Zipcode ZipCode_report = new EMSB3017_Zipcode(this, privilegeEntity);
                    ZipCode_report.ShowDialog();
                    break;
                case "EMS00040":
                    if (!string.IsNullOrEmpty(BaseApplicationNo))
                    {
                        _privilegeEntity = privilegeEntity;
                        if (BaseTopApplSelect == "Y")
                        {
                            EMS00040Control ems00040Control = new EMS00040Control(this, privilegeEntity);
                            AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, ems00040Control);
                        }
                        else
                            CommonFunctions.MessageBoxDisplay(Consts.Messages.AplicantSelectionMsg);
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay(Consts.Messages.Applicantdoesntexist);
                    }
                    break;

              

                case "EMS00030":
                    EMS00030Control Ems30030control = new EMS00030Control(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, Ems30030control);
                    break;

                case "EMSB0008":
                    EMSB0008_Presets Presets_report = new EMSB0008_Presets(this, privilegeEntity);
                    Presets_report.ShowDialog();
                    break;

                case "EMSB0021":
                    EMSB0021_ActivityReport EMSB0021Form = new EMSB0021_ActivityReport(this, privilegeEntity);
                    EMSB0021Form.ShowDialog();
                    break;

                case "ARSB0130":
                    ARSB2130_ReportForm Daycareexclusion = new ARSB2130_ReportForm(this, privilegeEntity);
                    Daycareexclusion.ShowDialog();
                    break;
                case "FIXCLINT":
                    FIXCLIENTForm fixClientForm = new FIXCLIENTForm(this, privilegeEntity);
                    fixClientForm.ShowDialog();
                    break;
                    break;
                //case "FIXFAMID":
                //    FIXFAMILYIDForm2 fixfamidForm2 = new FIXFAMILYIDForm2(this, privilegeEntity, string.Empty);
                //    fixfamidForm2.ShowDialog();
                //    break;
                case "EMSB0024":
                    EMSB0024_ReportForm EMSB0024Form = new EMSB0024_ReportForm(this, privilegeEntity);
                    EMSB0024Form.ShowDialog();
                    break;
                case "EMSB0004":
                    EMSB0004_ReportForm EMSB0004Form = new EMSB0004_ReportForm(this, privilegeEntity);
                    EMSB0004Form.ShowDialog();
                    break;
                case "EMSB0026":
                    EMSB0026_SweepResources EMSB3026Form = new EMSB0026_SweepResources(this, privilegeEntity);
                    EMSB3026Form.ShowDialog();
                    break;
                case "EMSB0009":
                    EMSB0009_Report EMSB3009Form = new EMSB0009_Report(this, privilegeEntity);
                    EMSB3009Form.ShowDialog();
                    break;
                case "EMSB0015":
                    EMSB0015_ReportForm EMSB3015Form = new EMSB0015_ReportForm(this, privilegeEntity);
                    EMSB3015Form.ShowDialog();
                    break;
                case "EMSB0002":
                    EMSB0002_Report EMSB3002Form = new EMSB0002_Report(this, privilegeEntity);
                    EMSB3002Form.ShowDialog();
                    break;
                case "EMSB0001":
                    EMSB0001_Report EMSB3001Form = new EMSB0001_Report(this, privilegeEntity);
                    EMSB3001Form.ShowDialog();
                    break;
                case "EMSB0025":
                    EMSB0025_Report EMSB3025Form = new EMSB0025_Report(this, privilegeEntity);
                    EMSB3025Form.ShowDialog();
                    break;
                case "TOOL0001":
                    TOOL0001_ChangeReport SITEForm = new TOOL0001_ChangeReport(this, privilegeEntity);
                    SITEForm.ShowDialog();
                    break;
                case "EMSB0023":
                    EMSB0023_Report EMSB3023Form = new EMSB0023_Report(this, privilegeEntity);
                    EMSB3023Form.ShowDialog();
                    break;
                case "EMSB0006":
                    EMSB0006_Report EMSB3006Form = new EMSB0006_Report(this, privilegeEntity);
                    EMSB3006Form.ShowDialog();
                    break;

                case "ARSB0170":
                    ARSB2170_ReportForm ARSb0170Form = new ARSB2170_ReportForm(this, privilegeEntity);
                    ARSb0170Form.ShowDialog();
                    break;
                case "EMSB0027":
                    EMSB0027_Report EMSB3027Form = new EMSB0027_Report(this, privilegeEntity);
                    EMSB3027Form.ShowDialog();
                    break;

                case "ARS00125":
                    if (!string.IsNullOrEmpty(BaseApplicationNo))
                    {
                        if (BaseTopApplSelect == "Y")
                        {
                            List<ARSCUSTEntity> CustDetails = _model.ArsData.Browse_ARSCUST(BaseAgency, BaseDept, BaseProg, BaseApplicationNo.Trim(), string.Empty);
                            if (CustDetails.Count > 0)
                            {
                                ARS00125Form ars0012form = new ARS00125Form(this, privilegeEntity);
                                ars0012form.ShowDialog();
                            }
                            else MessageBox.Show("Customer not found in ARSCUST", "ARS00125");
                        }
                        else
                            CommonFunctions.MessageBoxDisplay(Consts.Messages.AplicantSelectionMsg);
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay(Consts.Messages.Applicantdoesntexist);
                    }
                    break;
            
                case "RPMEMBER":
                    ReportControl reportControl = new ReportControl(this, privilegeEntity, string.Empty);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, reportControl);
                    break;
                case "RPINTAKE":
                    ReportControl RPINTAKE = new ReportControl(this, privilegeEntity, "RPINTAKE");
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, RPINTAKE);
                    break;
                case "RPREPORT":
                    ReportControl RPREPORT = new ReportControl(this, privilegeEntity, "RPREPORT");
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, RPREPORT);
                    break;
                case "RPREPOR1":
                    ReportControl RPREPORT1 = new ReportControl(this, privilegeEntity, "RPREPORT1");
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, RPREPORT1);
                    break;
                case "RPREPOR2":
                    ReportGridControl RPREPORT2 = new ReportGridControl(this, privilegeEntity, "RPREPORT2");
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, RPREPORT2);
                    break;
                
                case "AGYXML":
                    TMSB00AGYTABFORM AGYXML = new TMSB00AGYTABFORM(this, privilegeEntity);
                    AGYXML.ShowDialog();
                    break;
                case "EMSB0028":
                    EMSB0028_Report EMS0028Form = new EMSB0028_Report(this, privilegeEntity);
                    EMS0028Form.ShowDialog();
                    break;
                case "EMSB2028":
                    EMSB2028_Report EMS2028Form = new EMSB2028_Report(this, privilegeEntity);
                    EMS2028Form.ShowDialog();
                    break;
                case "EMSB0029":
                    EMSB0029_Report EMS0029Form = new EMSB0029_Report(this, privilegeEntity);
                    EMS0029Form.ShowDialog();
                    break;
                case "ADMNB003":
                    ADMNB003 ADMNB003Form = new ADMNB003(this, privilegeEntity);
                    ADMNB003Form.ShowDialog();
                    break;
                case "HSSB0150":
                    HSSB0150_Report HSSB00150Form = new HSSB0150_Report(this, privilegeEntity);
                    HSSB00150Form.ShowDialog();
                    break;
                case "EMSUNLOK":
                    EmsUnlockScreen EmsUnlockScreenform = new EmsUnlockScreen(this, privilegeEntity);
                    EmsUnlockScreenform.ShowDialog();
                    break;
                case "HLS00133":
                    if (!UserProfile.Components.Contains("None"))
                    {
                        HLS00133_Control HLS00133 = new HLS00133_Control(this, privilegeEntity);
                        AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, HLS00133);
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay("You can't Access any Component \n Please Contact Your System Administrator");
                    }
                    break;
                case "RNG00001":
                    RNG00001 RNG00001 = new RNG00001(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, RNG00001);
                    break;
                case "HLS00134":
                    if (!string.IsNullOrEmpty(BaseApplicationNo))
                    {
                        if (BaseTopApplSelect == "Y")
                        {
                            if (!UserProfile.Components.Contains("None"))
                            {
                                HLS00134Control HSS00134 = new HLS00134Control(this, privilegeEntity);
                                AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, HSS00134);
                            }
                            else
                            {
                                CommonFunctions.MessageBoxDisplay("You can't Access any Component \n Please Contact Your System Administrator");
                            }
                        }
                        else
                            CommonFunctions.MessageBoxDisplay(Consts.Messages.AplicantSelectionMsg);
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay(Consts.Messages.Applicantdoesntexist);
                    }
                    break;
                case "MERGFILE":
                    PdfListForm pdfMergeListForm = new PdfListForm(this);
                    pdfMergeListForm.ShowDialog();
                    break;
                case "ADMNB004":
                    ADMNB004 ADMNB004Form = new ADMNB004(this, privilegeEntity);
                    ADMNB004Form.ShowDialog();
                    break;

                case "HLSB0001":
                    HLSB0001_Report HLSB0001Form = new HLSB0001_Report(this, privilegeEntity);
                    HLSB0001Form.ShowDialog();
                    break;
                case "AGYXMLRG":
                    TMSB00AGYTABFORM AGRNGXML = new TMSB00AGYTABFORM(this, privilegeEntity);
                    AGRNGXML.ShowDialog();
                    break;
                case "CASE0005":
                    CASE0006CLOSESCREEN CASE0006CLOSESCREENFrom = new CASE0006CLOSESCREEN(this, privilegeEntity);
                    CASE0006CLOSESCREENFrom.ShowDialog();
                    break;

                
                case "CUST0001":
                    if (BaseAgencyControlDetails.SpanishSwitch == "Y")
                    {
                        SpanishCustomQuestions spanishForm = new SpanishCustomQuestions(this, privilegeEntity, string.Empty, "CUSTOM");
                        spanishForm.ShowDialog();
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay("AGENCY CONTROL SPANISH SWITCH NOT DEFINED");
                    }
                    break;
                
                case "LAGY0001":
                    if (BaseAgencyControlDetails.SpanishSwitch == "Y")
                    {
                        SpanishCustomQuestions spanishForm = new SpanishCustomQuestions(this, privilegeEntity, string.Empty, "LEANAGYTABS");
                        spanishForm.ShowDialog();
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay("AGENCY CONTROL SPANISH SWITCH NOT DEFINED");
                    }
                    break;
                case "CASEHIE1":
                    SpanishCustomQuestions spanishhieForm = new SpanishCustomQuestions(this, privilegeEntity, string.Empty, "CASEHIE");
                    spanishhieForm.ShowDialog();

                    break;
                case "CAMAST01":
                    SpanishCustomQuestions spanishCAMASTForm = new SpanishCustomQuestions(this, privilegeEntity, string.Empty, "CAMAST");
                    spanishCAMASTForm.ShowDialog();
                    break;
                case "LEANMESS":
                    PIPEmailForm pipEmailForm = new PIPEmailForm(this, privilegeEntity);
                    pipEmailForm.ShowDialog();
                    break;
             
                case "PIPB0005":
                    PIPB0005 PIPB005Form = new PIPB0005(this, privilegeEntity);
                    PIPB005Form.ShowDialog();
                    break;
                //case "CASE0023":
                //    VouchGen_Control VouchControl = new VouchGen_Control(this, privilegeEntity);
                //    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, VouchControl);
                //    break;
                case "ADMN0023":
                    VoucherDefinitionControl VouchDefControl = new VoucherDefinitionControl(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, VouchDefControl);
                    break;
                case "ADMN0022":
                    ADMN0022Control ADMN0022 = new ADMN0022Control(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, ADMN0022);
                    break;
                case "TMS10100":
                    TMS10100 tms10100 = new TMS10100(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, tms10100);
                    break;
                
                case "TMS10110":
                    TMS10110Control TMS10110 = new TMS10110Control(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, TMS10110);
                    break;
                case "TMS10120":
                    TMS00120Control TMS10120 = new TMS00120Control(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, TMS10120);
                    break;
              
                case "EMS00050":
                    EMS00050Control Ems30050control = new EMS00050Control(this, privilegeEntity);
                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, Ems30050control);
                    break;
                
                case "PIP00001":
                    if (!string.IsNullOrEmpty(BaseApplicationNo))
                    {
                        if (BaseTopApplSelect == "Y")
                        {
                            PIP00001Form _PIP00001Form = new PIP00001Form(this, privilegeEntity, this.BaseAgency, this.BaseDept, this.BaseProg, this.BaseYear, this.BaseApplicationNo, string.Empty, string.Empty);
                            _PIP00001Form.ShowDialog();
                        }
                        else
                            CommonFunctions.MessageBoxDisplay(Consts.Messages.AplicantSelectionMsg);
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay(Consts.Messages.Applicantdoesntexist);
                    }
                    break;
                case "CASE0025":
                    if (UserProfile.EMS_Access == "S" || UserProfile.EMS_Access == "D")
                    {
                        if (BaseAgencyControlDetails.PaymentCategorieService == "Y")
                        {
                            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseAgency, BaseDept, BaseProg);

                            if (programEntity != null)
                            {
                                if (programEntity.DepSerpostPAYCAT != string.Empty)
                                {
                                    CASE0025Control _case0025control = new CASE0025Control(this, privilegeEntity);
                                    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, _case0025control);
                                }
                                else
                                {
                                    CommonFunctions.MessageBoxDisplay("This Program is not set for Payment Category");
                                }
                            }
                        }
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay("You are not Authorized to Operate this Screen. Contact Your Administrator");
                    }
                    break;
                case "CASB0015":

                    if (UserProfile.EMS_Access == "S" || UserProfile.EMS_Access == "D")
                    {
                        CASB0015_Form CASB0015_Report = new CASB0015_Form(this, privilegeEntity);
                        CASB0015_Report.ShowDialog();
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay("You are not Authorized to Operate this Report. Contact Your Administrator");
                    }
                    break;
                case "CASBLTRB":

                    if (UserProfile.EMS_Access == "S" || UserProfile.EMS_Access == "D")
                    {
                        CASBLTRB CASBLTRB_Report = new CASBLTRB(this, privilegeEntity);
                        CASBLTRB_Report.ShowDialog();
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay("You are not Authorized to Operate this Report. Contact Your Administrator");
                    }
                    break;
                case "EMSB1009":
                    EMSB1009_Report EMSB1009Form = new EMSB1009_Report(this, privilegeEntity);
                    EMSB1009Form.ShowDialog();
                    break;

                case "APGT0001":
                    if (!string.IsNullOrEmpty(BaseAgencyNo))
                    {
                        if (BaseTopApplSelect == "Y")
                        {
                            APGT0001_Control CASE4006 = new APGT0001_Control(this, privilegeEntity);
                            AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, CASE4006);
                        }
                        else
                            CommonFunctions.MessageBoxDisplay(Consts.Messages.AgencyCodeSelectionMsg);
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay(Consts.Messages.Applicantdoesntexist);
                    }
                    break;
                case "RNGS1014":
                    RNGS1014 RNGS1014 = new RNGS1014(this, privilegeEntity); ;
                    RNGS1014.ShowDialog();
                    break;
                case "CASB0018":
                    CASB0018_Report casb0018form = new CASB0018_Report(this, privilegeEntity);
                    casb0018form.ShowDialog();
                    break;
                case "RNGS2014":
                    RNGS2014 RNGS2014 = new RNGS2014(this, privilegeEntity); ;
                    RNGS2014.ShowDialog();
                    break;
                default:
                    HelpForm2 helpForm = new HelpForm2();
                    //FrmUploadFtp helpForm = new FrmUploadFtp();
                    helpForm.ShowDialog();
                    Consts.Messages.UserCreatedSuccesssfully.DisplayFirendlyMessage(Captain.Common.Exceptions.ExceptionSeverityLevel.Information);
                    break;
            }

            */
            #endregion
        }

        private void pbtnChangePassword_Click(object sender, EventArgs e)
        {
            ChangePassword changePassword = new ChangePassword(this);
            changePassword.StartPosition = FormStartPosition.CenterScreen;
            changePassword.ShowDialog();
        }

        private void pHIEFilter_Click(object sender, EventArgs e)
        {
            if (BusinessModuleID == "01")
            {

                AdminHierarchySelection hierarchieSelectionForm = new AdminHierarchySelection(this);
                hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
                hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnDefaultAgencyFormClosed);
                hierarchieSelectionForm.ShowDialog();
            }
            else
            {
                strcurrentHIE = BaseAgency + "-" + BaseDept + "-" + BaseProg; strYear = BaseYear;
                HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(this, strcurrentHIE, "Master", string.Empty, "A", "I", UserID, "Master");
                hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
                hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnDefaultHierarchieFormClosed);
                hierarchieSelectionForm.ShowDialog();
            }
        }

        string strcurrentHIE = ""; string strYear = ""; string PrivAdmnAgency = string.Empty;
        private void OnDefaultAgencyFormClosed(object sender, FormClosedEventArgs e)
        {
            AdminHierarchySelection form = sender as AdminHierarchySelection;

            if (form.DialogResult == DialogResult.OK)
            {
                string Agency = string.Empty;
                Agency = form.SelectedAgency();

                if (BusinessModuleID == "01")
                {
                    _statAdminAgency = Agency;
                    BaseAdminAgency = _statAdminAgency;
                }
                else
                {
                    if (!string.IsNullOrEmpty(Agency.Trim()))
                        BaseAdminAgency = Agency;
                }
                pnlCenterbar.Visible = true;
                pnlSearchbox.Visible = false;
                pHIEFilter.Visible = true;
                string strAgencyName = BaseAdminAgency + " - " + _model.lookupDataAccess.GetHierachyDescription("1", BaseAdminAgency, BaseDept, BaseProg);
                if (BaseAdminAgency == "**") strAgencyName = BaseAdminAgency + " - " + "All Agencies";

                hierachyNamecontrol.lblHierchy.Text = strAgencyName;//CommonFunctions.GetHTMLHierachyFormat(strAgencyName, string.Empty, string.Empty, string.Empty, "01");

                pnlCenterbar.Controls.Clear();
                hierachyNamecontrol.Dock = Wisej.Web.DockStyle.Fill;
                pnlCenterbar.Controls.Add(hierachyNamecontrol);

                
                OnContentTabsSelectedIndexChanged(ContentTabs, new EventArgs());
            }
        }

        private void OnDefaultHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelection form = sender as HierarchieSelection;
            string strPublicCode = string.Empty;
            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchiesWithYear;
                string StrYear = form.SelectedHierarchyYear();
                if (selectedHierarchies.Count > 0)
                {
                    string hierarchy = string.Empty;
                    foreach (HierarchyEntity row in selectedHierarchies)
                    {
                        strPublicCode = row.Code;
                        hierarchy += row.Agency + row.Dept + row.Prog;
                    }
                    //txtDefaultHierachy.Text = strPublicCode;
                    BaseAgency = selectedHierarchies[0].Agency;
                    BaseDept = selectedHierarchies[0].Dept;
                    BaseProg = selectedHierarchies[0].Prog;

                    if (!string.IsNullOrEmpty(StrYear.Trim()))
                    {
                        BaseYear = StrYear;
                    }
                    else
                    {
                        ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseAgency, BaseDept, BaseProg);
                        if (programEntity != null)
                        {
                            BaseYear = programEntity.DepYear.Trim() == string.Empty ? "    " : programEntity.DepYear;
                        }
                    }

                    BaseAdminAgency = selectedHierarchies[0].Agency;

                    MainMenuControl MainMenu_Control = this.GetBaseUserControlMainMenu() as MainMenuControl;
                    if (MainMenu_Control != null)
                    {
                        applicationNameControl.Btn_First.Visible = applicationNameControl.BtnP10.Visible = applicationNameControl.BtnPrev.Visible =
                               applicationNameControl.BtnNxt.Visible = applicationNameControl.BtnN10.Visible = applicationNameControl.BtnLast.Visible = true;

                        MainMenu_Control.Set_DefHie_as_BaseHie(BaseAgency, BaseDept, BaseProg, BaseYear);

                        ProgramDefinitionEntity SprogramEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseAgency, BaseDept, BaseProg);
                        if (SprogramEntity != null)
                        {
                            if (BaseYear.Trim() != SprogramEntity.DepYear.Trim())
                            {
                                applicationNameControl.lblMsg.Text = "** Current Program Year is " + SprogramEntity.DepYear.Trim() + "... you are not in the Current Program Year **";
                                applicationNameControl.lblMsg.Visible = true;
                            }
                            else
                            {
                                applicationNameControl.lblMsg.Visible = false;
                            }
                        }

                    }

                    

                    /*** Refresh the Opened Tabs in Other modules ***/
                    //RefreshOpenedAdminPages();
                    /************************************************/

                    if (strcurrentHIE + strYear != BaseAgency + "-" + BaseDept + "-" + BaseProg + BaseYear)
                    {
                        ContentTabs.TabPages.Clear();
                        strcurrentModuleID = BusinessModuleID;
                        /*** Refresh the Menu Tree ***/
                        frmHiEScren = "Y";
                        BuildMenuTree();
                        /************************************************/

                        /*** Close All Tabs ***/
                        btncloseAll_Click(sender, e);

                        /************************************************/
                    }
                    else
                    {
                        if (strcurrentHIE + strYear == BaseAgency + "-" + BaseDept + "-" + BaseProg + BaseYear)
                        {
                            if (BaseDefaultHIE != strcurrentHIE)
                            {
                                ContentTabs.TabPages.Clear();
                                strcurrentModuleID = BusinessModuleID;
                                /*** Refresh the Menu Tree ***/
                                frmHiEScren = "Y";
                                BuildMenuTree();
                                /************************************************/

                                /*** Close All Tabs ***/
                                btncloseAll_Click(sender, e);

                            }


                        }
                    }



                }

                //for (int x = 0; x < NavTabs.Items.Count; x++)
                //{
                //    NavTabs.Items[x].Expanded = false;
                //}
            }
        }

        //public string strcurrentModuleID = "";
        private void RefreshOpenedAdminPages()
        {
            BaseUserControl MainMenu_Sub_Open_Control = GetBaseUserControl();
            if (MainMenu_Sub_Open_Control != null)
            {
                /******************************** Case Managament Module Screens Refresh Functions ***********************************************/

                
                /*******************************************************************************************************************************/
                if (MainMenu_Sub_Open_Control is MainMenuControl)
                {
                    (MainMenu_Sub_Open_Control as MainMenuControl).RefreshMainMenu();
                }

                if (MainMenu_Sub_Open_Control is Case3001Control)
                {
                    (MainMenu_Sub_Open_Control as Case3001Control).Refresh();
                }

                if (MainMenu_Sub_Open_Control is ADMN0022Control)
                {
                    (MainMenu_Sub_Open_Control as ADMN0022Control).Getsaldefdata(string.Empty);
                }

                if (MainMenu_Sub_Open_Control is Case0009Control)
                {
                    (MainMenu_Sub_Open_Control as Case0009Control).fillGrid();
                }

                if (MainMenu_Sub_Open_Control is ADMN0015Control)
                {
                    (MainMenu_Sub_Open_Control as ADMN0015Control).fillHieGrid(string.Empty);
                }

                //if (PrivAdmnAgency != BaseAdminAgency)
                //{

                //if (MainMenu_Sub_Open_Control is ADMN0020control)
                //{
                //    (MainMenu_Sub_Open_Control as ADMN0020control).Refresh();
                //}
                //}

                if (PrivAdmnAgency != BaseAdminAgency)
                {
                    PrivAdmnAgency = BaseAdminAgency;
                    //if (MainMenu_Sub_Open_Control is ADMN0030_Control)
                    //{
                    //    (MainMenu_Sub_Open_Control as ADMN0030_Control).Refresh();
                    //    var yourtabs = ContentTabs.Controls.OfType<TabPage>().Where(tab =>
                    //        tab.Name.Contains("ADMN0020")).ToList();
                    //    var index = ContentTabs.TabPages.IndexOf(yourtabs[0]);

                    //    BaseUserControl mainmentsubcontrol = (BaseUserControl)(ContentTabs.TabPages[index] as TabPage).Controls[0];
                    //    (mainmentsubcontrol as ADMN0020control).Refresh();
                    //    //ADMN0020control.refres
                    //}

                    if (MainMenu_Sub_Open_Control is ADMN0020control)
                    {
                        (MainMenu_Sub_Open_Control as ADMN0020control).Refresh();
                        var yourtabs = ContentTabs.Controls.OfType<TabPage>().Where(tab =>
                            tab.Name.Contains("ADMN0030")).ToList();
                        var index = ContentTabs.TabPages.IndexOf(yourtabs[0]);

                        //BaseUserControl mainmentsubcontrol = (BaseUserControl)(ContentTabs.TabPages[index] as TabPage).Controls[0];
                        //(mainmentsubcontrol as ADMN0030_Control).Refresh();
                        //(MainMenu_Sub_Open_Control as ADMN0030_Control).Refresh();
                    }

                }

                if (MainMenu_Sub_Open_Control is HierarchyDefinitionControl)
                {
                    (MainMenu_Sub_Open_Control as HierarchyDefinitionControl).populateGridData(BaseAdminAgency);
                }

                if (MainMenu_Sub_Open_Control is ProgramDefinition)
                {
                    ProgramDefinitionEntity programEntity = new ProgramDefinitionEntity();
                    (MainMenu_Sub_Open_Control as ProgramDefinition).RefreshGrid(programEntity);
                }

                if (MainMenu_Sub_Open_Control is UserListControl)
                {
                    (MainMenu_Sub_Open_Control as UserListControl).RefreshGrid(string.Empty);
                }

                
                if (MainMenu_Sub_Open_Control is FLDCNTLMaintenanceControl)
                {
                    (MainMenu_Sub_Open_Control as FLDCNTLMaintenanceControl).Refreshdata();
                }

                if (MainMenu_Sub_Open_Control is AdminScreenControls)
                {
                    (MainMenu_Sub_Open_Control as AdminScreenControls).Refreshdata();
                }

                
                if (MainMenu_Sub_Open_Control is CASE0016_Control)
                    (MainMenu_Sub_Open_Control as CASE0016_Control).Refresh();

                if (MainMenu_Sub_Open_Control is CASE0026Control)
                    (MainMenu_Sub_Open_Control as CASE0026Control).Refresh();

                if (MainMenu_Sub_Open_Control is CASE0021Control)
                    (MainMenu_Sub_Open_Control as CASE0021Control).Refresh();

                if (MainMenu_Sub_Open_Control is CASE0027Control)
                    (MainMenu_Sub_Open_Control as CASE0027Control).Refresh();

                if (MainMenu_Sub_Open_Control is CASE0028Control)
                    (MainMenu_Sub_Open_Control as CASE0028Control).Refresh();

                
                if (MainMenu_Sub_Open_Control is Vendor_Maintainance)
                    (MainMenu_Sub_Open_Control as Vendor_Maintainance).FillVendorGrid(string.Empty);

                
                if (MainMenu_Sub_Open_Control is Case2011Control)
                    (MainMenu_Sub_Open_Control as Case2011Control).Refresh();



                if (MainMenu_Sub_Open_Control is PrintApplicationControl)
                    (MainMenu_Sub_Open_Control as PrintApplicationControl).RefreshGrid();

                
            }
        }

        private void btncloseAll_Click(object sender, EventArgs e)
        {
            string strModuleType = "";
            if (BusinessModuleID == "01")
            {
                this.pnlContainer.BackgroundImageSource = "Resources\\Images\\11-01-01.jpg";
                this.pnlContainer.BackgroundImageLayout = Wisej.Web.ImageLayout.Stretch;

                int intTabPagesTotal = ContentTabs.TabPages.Count;
                ContentTabs.TabPages.Clear();
            }
            else
            {
                pnlContainer.BackgroundImageSource = "Resources\\Images\\blank.png";
                pnlContainer.BackColor = Color.White;
                int intTabPagesTotal = ContentTabs.TabPages.Count;
                int intTabPages = ContentTabs.TabPages.Count - 1;
                for (int i = 0; i < intTabPagesTotal; i++)
                {
                    if (!ContentTabs.TabPages[intTabPages].Name.Equals("MainMenu1"))
                    {
                        if (strModuleType == "Client")
                        {
                            if (!ContentTabs.TabPages[intTabPages].Name.ToUpper().Contains("CASE2001"))
                            {
                                ContentTabs.TabPages.RemoveAt(intTabPages);
                                intTabPages = intTabPages - 1;
                            }
                            else
                            {
                                intTabPages = intTabPages - 1;
                            }
                        }
                        else
                        {
                            ContentTabs.TabPages.RemoveAt(intTabPages);
                            intTabPages = intTabPages - 1;
                        }
                    }
                }
                pnlTabs.Controls.Add(ContentTabs);
                applicationNameControl.Controls[0].Controls["panelNavButtons"].Visible = true;
                //if (strModuleType != "Client")
                //{
                //    MainToolbar.Buttons.Clear();
                //}

            }
            NavTabs.SelectedItem = null;
            btncloseAll.Visible = false;
        }

        public override void RefreshSCREENSfromuserprivlg()
        {
            for (int iContentTabs = 0; iContentTabs < ContentTabs.TabPages.Count; iContentTabs++)
            {
                string tagItem = ContentTabs.TabPages[iContentTabs].Tag.ToString();
                var i = AdminSRCNPrivilege.FindAll(x => x.Program == tagItem);
                if (i.Count == 0)
                {
                    ContentTabs.TabPages.Remove(ContentTabs.TabPages[iContentTabs]);
                }
            }

            colNavItems = privlgNavAdmnItems;
            OnContentTabsSelectedIndexChanged(ContentTabs, new EventArgs());
        }

        private void pSwitchAccount_Click(object sender, EventArgs e)
        {
            Admn0004UserForm objAdmn0004 = new Admn0004UserForm(UserID);
            objAdmn0004.StartPosition = FormStartPosition.CenterScreen;
            objAdmn0004.FormClosed += ObjAdmn0004_FormClosed;
            objAdmn0004.ShowDialog();
        }

        private void ObjAdmn0004_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.Close();
            if (Admn0004UserForm.closelogin == "Y")
            {
                MasterPage main = new MasterPage();
                main.Show();
                main.Update();

                Admn0004UserForm.closelogin = "N";
            }
        }

        public override void AddTabClientIntake(string strFormCode)
        {
            if (strFormCode == "PIP00000")
            {
                List<PrivilegeEntity> userPrivilege = _model.UserProfileAccess.GetScreensByUserID(BusinessModuleID, UserID, BaseAgency + BaseDept + BaseProg);
                PrivilegeEntity privilegeEntity = userPrivilege.Find(u => u.Program.ToString() == "PIP00000");
                //PIP00000Control pIP00000Control = new PIP00000Control(this, privilegeEntity);
                //AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, pIP00000Control);
            }
            else if (strFormCode == "CASE0027")
            {
                List<PrivilegeEntity> userPrivilege = _model.UserProfileAccess.GetScreensByUserID(BusinessModuleID, UserID, BaseAgency + BaseDept + BaseProg);
                PrivilegeEntity privilegeEntity = userPrivilege.Find(u => u.Program.ToString() == "CASE0027");
                CASE0027Control case0027 = new CASE0027Control(this, privilegeEntity, "Search");
                AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, case0027);
            }
            //else if (strFormCode == "TMS00081")
            //{
            //    List<PrivilegeEntity> userPrivilege = _model.UserProfileAccess.GetScreensByUserID(BusinessModuleID, UserID, BaseAgency + BaseDept + BaseProg);
            //    PrivilegeEntity privilegeEntity = userPrivilege.Find(u => u.Program.ToString() == "TMS00081");
            //    TMS00081_Control TMS00081 = new TMS00081_Control(this, privilegeEntity);
            //    AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, TMS00081);
            //}
            else if (strFormCode == "CASE2005")
            {
                List<PrivilegeEntity> userPrivilege = _model.UserProfileAccess.GetScreensByUserID(BusinessModuleID, UserID, BaseAgency + BaseDept + BaseProg);
                PrivilegeEntity privilegeEntity = userPrivilege.Find(u => u.Program.ToString() == "CASE2005");
                //IncomepleteIntakeControl TMS00081 = new IncomepleteIntakeControl(this, privilegeEntity);
                //AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, TMS00081);
            }
            else
            {


                if (!string.IsNullOrEmpty(BaseApplicationNo))
                {
                    List<PrivilegeEntity> userPrivilege = _model.UserProfileAccess.GetScreensByUserID(BusinessModuleID, UserID, BaseAgency + BaseDept + BaseProg);
                    PrivilegeEntity privilegeEntity = userPrivilege.Find(u => u.Program.ToString() == "CASE2001");
                    if (BaseTopApplSelect == "Y")
                    {
                        //ClientIntakeControl clientIntakeControl = new ClientIntakeControl(this, privilegeEntity);
                        Case3001Control clientIntakeControl = new Case3001Control(this, privilegeEntity);
                        AddContentTab(privilegeEntity.PrivilegeName.Trim(), privilegeEntity.Program, clientIntakeControl);
                    }
                    else
                        CommonFunctions.MessageBoxDisplay(Consts.Messages.AplicantSelectionMsg);
                }
            }
        }


        public void LogoutfromZendesk()
        {


            try
            {
                logoutZendesk obj = new logoutZendesk("LOGOUT");
                obj.StartPosition = FormStartPosition.Manual;
                obj.Location = new System.Drawing.Point(this.Location.X, this.Location.Y + this.Height - obj.Height);

                obj.ShowDialog();
            }
            catch (Exception ex)
            {

            }

        }

        private void pbtnHelp_Click(object sender, EventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps("UNIV", 0, "UNIV", "", ""), target: "_blank");
        }
    }
}
public class OtherApps
{
    public string _Code { get; set; }
    public string _Desc { get; set; }
    public PrivilegeEntity _PrivilegeEntity { get; set; }
}