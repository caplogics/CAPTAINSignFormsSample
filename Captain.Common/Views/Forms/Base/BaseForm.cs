/*********************************************************************************************************
 * Class Name    : BaseForm
 * Author        : Kranthi
 * Created Date  : 
 * Version       : 
 * Description   : This is base class for child and main forms,this class contains the major functionality 
 *                 which is common for both the forms. 
 *********************************************************************************************************/

#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using Wisej.Web;
using Captain.Common.Controllers;
using Captain.Common.Exceptions;
using Captain.Common.Handlers;
using Captain.Common.Menus;
using Captain.Common.Model.Parameters;
//using Captain.Common.Model.QuantumFaults;
//using Captain.Common.Model.ServiceModel;
using Captain.Common.Resources;
using Captain.Common.Utilities;
using Captain.Common.Views.Controls;
using Captain.Common.Interfaces;
using Captain.Common.Views.UserControls;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web.Ext.NavigationBar;



#endregion

namespace Captain.Common.Views.Forms.Base
{

    public partial class BaseForm : Page, IBaseForm
    {
        #region Variables

        private string _objectType = string.Empty;
        private TagClass _sourceNode = null;
        private TagClass _targetNode = null;
        private string _previousNodeID = string.Empty;
        private bool _isFirst = false;
        private bool _isKeepValues = false;
        private bool _isCopyFiles = false;


        public SubNavigationTabs NavigationTreeView = new SubNavigationTabs();
        public WorkspaceTabs ContentTabs = new WorkspaceTabs();
        public NavigationTabs NavigationTabs = new NavigationTabs();
        public NavigationBarItem NavigationBarItem = new NavigationBarItem();
        public ToolBar MainToolbar = new ToolBar();
        public TabPage CurrentTabPage = null;
        public MenuManager MenuManager = null;
        public TabPage LinkCreateWorkspaceTab = null;
        public Panel NoTabs = new Panel();
        public TagClass TagClass = null;
        public List<TagClass> OperationTypeTagClassList = new List<TagClass>();
        public List<string> Privileges = new List<string>();
        public List<TagClass> OpenedDocumentTagClassList = new List<TagClass>();
        public List<SpinnerIconItem> SpinnerItemList = new List<SpinnerIconItem>();

        public TagClass GridPasteOperationNode = null;
        public string CopyPasteOperationNodeGridName = string.Empty;
        public TreeNode ClipboardNode = null;
        public string ClipboardNodeName = string.Empty;
        public List<string[]> RequestedUploadGUIDList = new List<string[]>();
        public bool IsContextCutMode = false;
        public bool IsGridCutMode = false;
        public string LastMenuActionGuid = string.Empty;
        public string NewSubmissionPath = string.Empty;
        public string CurrentView = string.Empty;
        public bool ViewChanged = false;
        public string TargetPage = Consts.Common.One;

        public string isPrivSaveFlag = "N";
        public string strcurrentModuleID = "";
        public string frmHiEScren = "N";
        public List<NavigationBarItem> privlgNavAdmnItems = new List<NavigationBarItem>();
        #endregion

        #region Events


        #endregion

        #region Constructors & Initialization

        public BaseForm()
        {
            InitializeComponent();

            CaptainResourceManager.Culture = Application.CurrentCulture;

            ImageList imageList = new ImageList();
            imageList.Images.Add(new ImageListEntry() { ImageSource = "resource.wx/" + Consts.Icons16x16.AddItem, Name = "AddItem" });
            imageList.Images.Add(new ImageListEntry() { ImageSource = "resource.wx/" + Consts.Icons16x16.Entry, Name = "Entry" });

            NavigationTreeView.ImageList = imageList;
            NavigationTreeView.BorderStyle = BorderStyle.None;

            CommonFunctions.SetCacheExpiration();

            this.Size = Application.Browser.Size;
            Application.BrowserSizeChanged += Application_BrowserSizeChanged;
        }

        private void Application_BrowserSizeChanged(object sender, EventArgs e)
        {
            this.Size = Application.Browser.Size;
        }
        #endregion

        #region Properties

        public TreeViewController TreeViewController { get; set; }

        public string UserID
        {
            get
            {
                return Captain<string>.Session[Consts.SessionVariables.UserID, Consts.Common.DefaultUserID];
            }
        }

        public string UserName
        {
            get
            {
                return Captain<string>.Session[Consts.SessionVariables.UserName, string.Empty];
            }
        }

        public string Password
        {
            get
            {
                return Captain<string>.Session[Consts.SessionVariables.Password, string.Empty];
            }
        }

        public string BaseAgency { get; set; }
        public string BaseDept { get; set; }
        public string BaseProg { get; set; }
        public string BaseYear { get; set; }
        public string BaseDefaultHIE { get; set; }
        public string BaseApplicationNo { get; set; }
        public string BaseApplicationName { get; set; }
        public string DataBaseConnectionString { get; set; }
        public string BaseHierarchyCwFormat { get; set; }
        public string BaseHierarchyCnFormat { get; set; }
        public string BaseTopApplSelect { get; set; }
        public string BaseComponent { get; set; }
        public string BaseFilterYear { get; set; }
        public string BaseLeanDataBaseConnectionString { get; set; }
        public string BaseDSSXMLDBConnString { get; set; }
        public string BasePIPDragSwitch { get; set; }

        public string BaseAdminAgency { get; set; }
        public string BaseAccessAll { get; set; }

        public List<CaseMstEntity> BaseCaseMstListEntity { get; set; }
        public List<CaseSnpEntity> BaseCaseSnpEntity { get; set; }
        public List<ChldTrckEntity> BaseTaskEntity { get; set; }
        public List<HlsTrckEntity> BaseHlsTaskEntity { get; set; }
        public List<CommonEntity> BaseAgyTabsEntity { get; set; }
        public AgencyControlEntity BaseAgencyControlDetails { get; set; }
        public List<HierarchyEntity> BaseAgencyuserHierarchys { get; set; }
        public List<HierarchyEntity> BaseCaseHierachyListEntity { get; set; }
        public string BaseAgencyNo { get; set; }
        public string BaseAgencyName { get; set; }

        public string BaseClientFolloupFromDate { get; set; }
        public string BaseClientFolloupToDate { get; set; }
        public string BaseClientFolloupWorker { get; set; }
        public string BaseClientFolloupScreentype { get; set; }
        public string BaseClientFolloupScreencode { get; set; }
        public string Gbl_Rent_Rec { get; set; }


        public UserEntity UserProfile
        {
            get
            {
                return Captain<UserEntity>.Session[Consts.SessionVariables.UserProfile, null];
            }
        }

        public string Locale
        {
            get
            {
                return Captain<string>.Session[Consts.SessionVariables.Language, Consts.Common.DefaultLanguage];
            }
        }

        public string ObjectType { get; set; }

        public bool IsCaptainBannerDisplayed { get; set; }

        public string BusinessModuleID
        {
            get; 
            set;
            //get
            //{
            //    if (NavigationTabs.SelectedItem != null)
            //        return ((NavigationBarItem)NavigationTabs.SelectedItem).Tag as string;
            //    else
            //        return "Administration";
            //}

            //get
            //{
            //    if (NavigationBarItem.Selected != null)
            //        return ((NavigationBarItem)NavigationTabs.SelectedItem).Tag as string;
            //    else
            //        return "Administration";
            //}
        }
        public virtual void BuildMenuTree()
        {

        }
        public virtual void RefreshSCREENSfromuserprivlg() { }
        #endregion

        #region Public / Virtual Methods

        /// <summary>
        /// Used to print information on the scree\n.
        /// </summary>
        /// <param name="htmlCode"></param>
        public void PrintControl(string htmlCode)
        {
            // TODO: Wisej ... 
            //InvokeMethodWithId(Consts.Javascript.Print, htmlCode);
        }

        /// <summary>
        /// SetLinkParameters
        /// </summary>
        /// <param name="linkParameters"></param>

        public void SetLinkParameters(LinkParameters linkParameters)
        {
            try
            {
                SetLinkParameters(linkParameters, 1024, 768);
            }
            catch (Exception ex)
            {
                StackFrame stackframe = new StackFrame(1, true);
                //ExceptionLogger.LogException(stackframe, ex, ExceptionSeverityLevel.High);
            }
        }

        /// <summary>
        /// SetLinkParameters
        /// </summary>
        /// <param name="linkParameters"></param>

        // TODO: Wisej ...
        public void SetLinkParameters(LinkParameters linkParameters, int width, int height)
        {
            try
            {
                linkParameters.WindowStyle = LinkWindowStyle.Normal;
                linkParameters.Size = new System.Drawing.Size(width, height);
                linkParameters.ShowLocationBar = CommonFunctions.CheckDebugMode();
                linkParameters.ShowMenuBar = CommonFunctions.CheckDebugMode();
                linkParameters.ShowToolBar = CommonFunctions.CheckDebugMode();
                linkParameters.ShowStatusBar = CommonFunctions.CheckDebugMode();
                linkParameters.ShowTitleBar = CommonFunctions.CheckDebugMode();
            }
            catch (Exception ex)
            {
                StackFrame stackframe = new StackFrame(1, true);
                ExceptionLogger.LogException(stackframe, ex, ExceptionSeverityLevel.High);
            }
        }

        /// <summary>
        /// Adds the welcome screen when not content tabs are visible.
        /// </summary>
        /// <param name="hostPanel"></param>
        public void AddWelcomeScreen(Panel hostPanel)
        {
            WelcomeScreenControl welcomeScreenControl = new WelcomeScreenControl();
            // 
            // NoTabs
            // 
            NoTabs.Anchor = Wisej.Web.AnchorStyles.None;
            NoTabs.BackColor = System.Drawing.Color.White;
            NoTabs.Controls.Clear();
            NoTabs.Controls.Add(welcomeScreenControl);
            NoTabs.Dock = Wisej.Web.DockStyle.Fill;
            //NoTabs.DragTargets = new Wisej.Web.Component[0]; 
            //NoTabs.Size = new System.Drawing.Size(704, 673);
            NoTabs.TabIndex = 0;
            //
            // welcomeScreenControl
            //
            welcomeScreenControl.Anchor = Wisej.Web.AnchorStyles.None;
            welcomeScreenControl.Dock = Wisej.Web.DockStyle.Fill;
            //welcomeScreenControl.DragTargets = new Wisej.Web.Component[0]; 
            welcomeScreenControl.Name = System.Guid.NewGuid().ToString();
            welcomeScreenControl.TabIndex = 1;
            hostPanel.Controls.Add(NoTabs);
            NoTabs.BringToFront();
        }

        /// <summary>
        /// Adds the welcome screen when not content tabs are visible.
        /// </summary>
        /// <param name="hostPanel"></param>
        public void AddSearchScreen(Panel hostPanel)
        {
            MainMenuControl mainMenuControl = new MainMenuControl(this);
            // 
            // NoTabs
            // 
            NoTabs.Anchor = Wisej.Web.AnchorStyles.None;
            NoTabs.BackColor = System.Drawing.Color.White;
            NoTabs.Controls.Clear();
            NoTabs.Controls.Add(mainMenuControl);
            NoTabs.Dock = Wisej.Web.DockStyle.Fill;
            //NoTabs.DragTargets = new Wisej.Web.Component[0];  
            //NoTabs.Size = new System.Drawing.Size(704, 673);
            NoTabs.TabIndex = 0;
            //
            // welcomeScreenControl
            //
            mainMenuControl.Anchor = Wisej.Web.AnchorStyles.None;
            mainMenuControl.Dock = Wisej.Web.DockStyle.Fill;
            //mainMenuControl.DragTargets = new Wisej.Web.Component[0]; 
            mainMenuControl.Name = System.Guid.NewGuid().ToString();
            mainMenuControl.TabIndex = 1;
            hostPanel.Controls.Add(NoTabs);
            NoTabs.BringToFront();
        }

        /// <summary>
        /// AddContentTab
        /// </summary>
        /// <param name="title"></param>
        /// <param name="name"></param>
        /// <param name="tagClass"></param>
        /// <param name="baseUserControl"></param>
        /// <param name="saveChanges"></param>
        public void AddContentTab(string title, string name, TagClass tagClass, BaseUserControl baseUserControl)
        {
            CurrentTabPage = null;
            try
            {
                for (int iContentTabs = 0; iContentTabs < ContentTabs.TabPages.Count; iContentTabs++)
                {
                    TagClass wsTagClass = (TagClass)ContentTabs.TabPages[iContentTabs].Tag;
                    if (wsTagClass == null) { continue; }
                    if (wsTagClass.ObjectID == tagClass.ObjectID)
                    {
                        CurrentTabPage = ContentTabs.TabPages[iContentTabs];
                        if (CurrentTabPage.Text == title)
                        {
                            ContentTabs.SelectedTab = CurrentTabPage;
                            break;
                        }
                    }
                }

                if (CurrentTabPage == null || name == Consts.Common.AdvancedSearchObjectName || name == Consts.Common.SearchResultsObjectName)
                {
                    CurrentTabPage = new TabPage();
                    this.ContentTabs.Controls.Add(CurrentTabPage);
                    CurrentTabPage.Controls.Add(baseUserControl);
                    baseUserControl.Dock = DockStyle.Fill;


                    //CurrentTabPage.DragTargets = new Wisej.Web.Component[0]; 
                    CurrentTabPage.Location = new System.Drawing.Point(0, 0);
                    CurrentTabPage.Name = name + ContentTabs.TabPages.Count.ToString();
                    CurrentTabPage.Tag = tagClass;
                    //CurrentTabPage.Size = new System.Drawing.Size(828, 589);
                    CurrentTabPage.TabIndex = 0;
                    CurrentTabPage.Text = title;
                    CurrentTabPage.BackColor = System.Drawing.Color.White;
                    ContentTabs.SelectedTab = CurrentTabPage;
                }

                if (this._objectType.Equals(Consts.Common.MainForm))
                {
                    if (NavigationTabs.SelectedItem != null)
                    {
                        if (BusinessModuleID == "")
                        {
                            MenuManager.FileClose.Enabled = true && NavigationTreeView.SelectedItem != null;
                            MenuManager.FileOpen.Enabled = !MenuManager.FileClose.Enabled && NavigationTreeView.SelectedItem != null;
                        }
                        else
                        {
                            MenuManager.FileClose.Enabled = true;
                        }

                    }
                }
                else
                {
                    EnableDisableCloseMenuItem();
                }

                ClearControl();
            }
            catch (Exception ex)
            {
                //ExceptionLogger.LogException(new StackFrame(1, true), ex, ExceptionSeverityLevel.High);
            }
        }

        /// <summary>
        /// Enables or disables the close menu item.
        /// </summary>
        public void EnableDisableCloseMenuItem()
        {
            if (ContentTabs.TabPages.Count > 0)
            {
                MenuManager.FileClose.Enabled = true;
            }
            else
            {
                MenuManager.FileClose.Enabled = false;
            }
        }

        /// <summary>
        /// Closes the selected content tab.
        /// </summary>
        public void CloseSelectedContentTab()
        {
            TagClass tagClass = null;
            //TreeNode treeNode = null;            
            string objectID = "0";

            try
            {
                if (ContentTabs.SelectedTab.Tag is TagClass)
                {
                    tagClass = (TagClass)ContentTabs.SelectedTab.Tag;
                    objectID = tagClass.ObjectID;
                }

                ContentTabs.TabPages.Remove(ContentTabs.SelectedTab);

                if (ContentTabs.TabPages.Count > 0)
                {
                    CurrentTabPage = ContentTabs.TabPages[0];

                    if (CurrentTabPage.Tag != null)
                    {
                        BaseUserControl baseUserControl = GetBaseUserControl();
                        if (baseUserControl != null)
                        {
                            baseUserControl.PopulateToolbar(MainToolbar);
                        }
                    }
                }
                else
                {
                    if (NavigationTabs.SelectedItem != null)
                    {

                    }
                }

                if (NavigationTabs.SelectedItem != null)
                {
                    if (NavigationTreeView.SelectedItem != null)
                    {
                        switch (CurrentTabPage.Text)
                        {

                            case Consts.Common.Inbox:
                                break;


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StackFrame stackframe = new StackFrame(1, true);
                //ExceptionLogger.LogException(stackframe, ex, ExceptionSeverityLevel.High);
            }

        }

        /// <summary>
        /// Sets the startpage state preference.
        /// </summary>
        public void SetStartPagePreference()
        {

        }

        /// <summary>
        /// Closes a content tab.
        /// </summary>
        /// <param name="selectedTab"></param>
        public void CloseContentTab(TabPage selectedTab)
        {
            try
            {
                TreeNode treeNode = null;
                TagClass tagClass = (TagClass)selectedTab.Tag;



                if (selectedTab != null)
                {
                    bool isDocumentTab = false;

                    MainToolbar.Buttons.Clear();
                    ContentTabs.TabPages.Remove(selectedTab);
                    if (ContentTabs.TabPages.Count <= 0)
                    {
                        //if (NavigationTabs.SelectedItem != null)
                        //{
                        //    if (BusinessModuleID == "")
                        //    {
                        //        MenuManager.FileClose.Enabled = true && NavigationTreeView.SelectedItem != null;
                        //        MenuManager.FileOpen.Enabled = !MenuManager.FileClose.Enabled && NavigationTreeView.SelectedItem != null;
                        //    }
                        //    else
                        //    {
                        //        MenuManager.FileClose.Enabled = false;
                        //        MenuManager.FileOpen.Enabled = false;
                        //    }

                        //}
                    }

                    if (ContentTabs.TabPages.Count > 0)
                    {
                        CurrentTabPage = ContentTabs.TabPages[0];
                        ContentTabs.SelectedTab = CurrentTabPage;
                        tagClass = (TagClass)CurrentTabPage.Tag;
                    }

                }
            }
            catch (Exception ex)
            {
                StackFrame stackframe = new StackFrame(1, true);
                //ExceptionLogger.LogException(stackframe, ex, ExceptionSeverityLevel.High);
            }
        }

        public virtual void RefreshNavigationTabs(string HIE)
        {
            //
        }

        /// <summary>
        /// Open a pdf document tab using a tagclass and a baseusercontrol.
        /// </summary>
        /// <param name="tagClass"></param>
        /// <param name="baseUserControl"></param>
        public void OpenContentTab(TagClass tagClass, BaseUserControl baseUserControl)
        {
            CurrentTabPage = null;

            try
            {
                CurrentTabPage = GetContentTabByTagClass(tagClass);

                if (CurrentTabPage == null)
                {
                    CurrentTabPage = new TabPage();
                    this.ContentTabs.TabPages.Add(CurrentTabPage);
                    CurrentTabPage.Controls.Add(baseUserControl);
                    baseUserControl.Dock = DockStyle.Fill;
                }

                //CurrentTabPage.Name = tagClass.TabObjectName;
                CurrentTabPage.Tag = tagClass;
                ContentTabs.SelectedTab = CurrentTabPage;
                ContentTabs.Update();
            }
            catch (Exception ex)
            {
                StackFrame stackFrame = new StackFrame();
                //ExceptionLogger.LogAndDisplayMessageToUser(stackFrame, ex, QuantumFaults.None, ExceptionSeverityLevel.High);
            }
        }

        /// <summary>
        /// Gets the base user control from the selected tab.
        /// </summary>
        /// <returns></returns>
        public BaseUserControl GetBaseUserControl()
        {
            BaseUserControl baseUserControl = null;
            if (ContentTabs.SelectedTab == null) { return baseUserControl; }

            try
            {
                baseUserControl = (BaseUserControl)(ContentTabs.TabPages[ContentTabs.SelectedIndex] as TabPage).Controls[0];

                
            }
            catch (Exception ex)
            {
                //ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
            }
            return baseUserControl;
        }


        /// <summary>
        /// Gets the base user control from the selected tab.
        /// </summary>
        /// <returns></returns>
        public BaseUserControl GetBaseUserControlMainMenu()
        {
            BaseUserControl baseUserControl = null;
            if (ContentTabs.SelectedTab == null) { return baseUserControl; }

            try
            {
                if (ContentTabs.TabPages.Count > 0)
                {
                    baseUserControl = (BaseUserControl)(ContentTabs.TabPages[0] as TabPage).Controls[0];
                }
            }
            catch (Exception ex)
            {
                //ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
            }
            return baseUserControl;
        }

        /// <summary>
        /// Gets the base user control from the selected tab.
        /// </summary>
        /// <returns></returns>
        public T GetBaseUserControl<T>() where T : BaseUserControl
        {
            T control = default(T);

            control = (T)GetBaseUserControl();

            return control;
        }

        /// <summary>
        /// Gets the base user control from the tab that matches the tagClass
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tagClass"></param>
        /// <returns></returns>
        public T GetBaseUserControl<T>(TagClass tagClass) where T : BaseUserControl
        {
            T control = default(T);

            if (tagClass != null)
            {
                TabPage workspaceTab = GetContentTabByTagClass(tagClass);
                if (workspaceTab != null)
                {
                    control = (T)GetBaseUserControl(workspaceTab);
                }
            }

            return control;
        }

        /// <summary>
        /// Gets the base user control from a workspace tab.
        /// </summary>
        /// <param name="workspaceTab"></param>
        /// <returns></returns>
        public BaseUserControl GetBaseUserControl(TabPage workspaceTab)
        {
            BaseUserControl baseUserControl = null;
            if (workspaceTab == null) { return baseUserControl; }

            try
            {
                baseUserControl = (BaseUserControl)workspaceTab.Controls[0];
            }
            catch (Exception ex)
            {
                StackFrame stackFrame = new StackFrame();
                //ExceptionLogger.LogAndDisplayMessageToUser(stackFrame, ex, QuantumFaults.None, ExceptionSeverityLevel.High);
            }
            return baseUserControl;
        }

        /// <summary>
        /// Gets a list of base user controls from the content tabs.
        /// </summary>
        /// <returns></returns>
        public List<BaseUserControl> GetBaseUserControlList()
        {
            List<BaseUserControl> BaseUserControlList = new List<BaseUserControl>();
            foreach (TabPage tabPage in ContentTabs.TabPages)
            {
                try
                {
                    BaseUserControl baseUserControl = GetBaseUserControl(tabPage);
                    baseUserControl.Tag = tabPage.Tag;
                    BaseUserControlList.Add(baseUserControl);
                }
                catch (Exception ex)
                {
                    StackFrame stackFrame = new StackFrame();
                    //ExceptionLogger.LogAndDisplayMessageToUser(stackFrame, ex, QuantumFaults.None, ExceptionSeverityLevel.High);
                }
            }
            return BaseUserControlList;
        }

        /// <summary>
        /// Gets Content Tab for Document
        /// </summary>
        /// <param name="tagClass"></param>
        /// <returns></returns>
        public TabPage GetContentTabByTagClass(TagClass tagClass)
        {
            TabPage currentTabPage = null;
            TagClass tabTagClass = null;

            for (int contentTabsIndex = 0; contentTabsIndex < ContentTabs.TabPages.Count; contentTabsIndex++)
            {
                if (ContentTabs.TabPages[contentTabsIndex].Tag is TagClass)
                {
                    tabTagClass = (TagClass)ContentTabs.TabPages[contentTabsIndex].Tag;

                    if (tabTagClass.ObjectID.Equals(tagClass.ObjectID) && tagClass.ObjectName.Equals(tabTagClass.ObjectName))
                    {
                        currentTabPage = ContentTabs.TabPages[contentTabsIndex];
                        break;
                    }
                    else
                    {
                        currentTabPage = null;
                    }
                }
            }

            return currentTabPage;
        }


        /// <summary>
        /// Used to refresh the selected node and its children, if any.
        /// </summary>
        /// <param name="treeNode"></param>
        public void RefreshNode(TreeNode node)
        {
            if (node == null) { return; }

            //TagClass nodeTagClass = node.GetTagClass();
            bool isExpanded = node.IsExpanded;

            node.Nodes.Clear();

            if (node.Parent == null) //Refreshing top level nodes
            {
                //TreeViewController.RefreshTopTreeNode(node);
            }
            else    //Refreshing child level nodes.
            {
                List<string> attributes = new List<string>();
                //                switch ((int)this.NavigationTabs.SelectedIndex)
                //                {
                //                    case 1:
                //                    case 2:
                ////
                //                        break;
                //                    default:
                //                       //
                //                        break;
                //                }

                //node.Text = nodeName;
                //node.Image = new IconResourceHandle(nodeTagClass.Icon);
            }

            //if (nodeTagClass.HasChildren)
            //{
            //    TreeViewController.AddDummyNode(node, nodeTagClass, true, false);
            //    ProcessBeforeExpandNode(node, false);
            //}
            if (isExpanded) { node.Expand(); }

            OnTreeViewClick(NavigationTreeView, new TreeNodeMouseClickEventArgs(node, MouseButtons.Left, 1, 0, 0));
        }

        /// <summary>
        /// Used to process the before expand node event.
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name="useCachedValues"></param>
        protected void ProcessBeforeExpandNode(TreeNode treeNode, bool useCachedValues)
        {
            try
            {
                TagClass tagClass = (TagClass)treeNode.Tag;
                TreeNode lastNode = treeNode.LastNode;

                switch (BusinessModuleID)
                {
                    case "Welcome":
                        //
                        break;

                }


                //Alaa: 
                //if (!treeNode.Loaded)
                //{
                //    treeNode.Loaded = true; 
                //}

            }
            catch (Exception ex)
            {
                //ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
            }
        }

        /// <summary>
        /// Open a document link.
        /// </summary>
        /// <param name="objectID">The object to open.</param>
        public void OpenContentTab(string objectID)
        {
            OpenContentTab(objectID, 1, string.Empty);
        }

        /// <summary>
        /// Open a document link.
        /// </summary>
        /// <param name="objectID">The object to open.</param>
        /// <param name="pageNumber">The page number to open the document. Applies to pdfs only.</param>
        /// <param name="initialNameDestination">The initial destination name in a document. Applies to pdfs only.</param>
        public void OpenContentTab(string objectID, int pageNumber, string initialNameDestination)
        {
            TagClass tagClass = null;
            if (tagClass == null)
            {
                //MessageBox.Show(Consts.Messages.ObjectNameNotFound.GetMessage(objectID), Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            tagClass.InitialPageNumber = pageNumber;
            tagClass.InitialNamedDestination = initialNameDestination;
            OpenContentTab(tagClass);

        }

        /// <summary>
        /// Hanldels the menuitem click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnProcessMenuItemEvents(object sender, MenuManagerEventArgs e) { }

        /// <summary>
        /// Shows or hides the ViewPoint Logo Banner based on the user-selected preferences.
        /// </summary>
        public virtual void SetLogoBanner() { }

        /// <summary>
        /// Saves the preference.
        /// </summary>
        /// <param name="preferenceNameDisplayCode"></param>
        /// <param name="moduleID"></param>
        /// <param name="value"></param>
        public virtual void SavePreference(string preferenceNameDisplayCode, string moduleID, string value) { }

        /// <summary>
        /// Used to get the specific preference value
        /// </summary>
        /// <param name="preferenceNameDisplayCode"></param>
        /// <param name="moduleID"></param>
        /// <returns></returns>
        public virtual string GetPreferenceValue(string preferenceNameDisplayCode, string moduleID) { return string.Empty; }

        /// <summary>
        /// Virtual method used to handle the navigation TreeView click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnTreeViewClick(object sender, TreeNodeMouseClickEventArgs e) { }

        /// <summary>
        /// Used to handle the treeview key press event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnTreeViewKeyPress(object sender, Wisej.Web.KeyPressEventArgs e) { }

        /// <summary>
        /// Virtual method used to handle the navigation TreeView double click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnTreeViewDoubleClick(object sender, TreeNodeMouseClickEventArgs e) { }

        /// <summary>
        /// Virtual method used to handle the navigation TreeView before expand event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnTreeViewBeforeExpand(object sender, TreeViewCancelEventArgs e) { }

        /// <summary>
        /// Virtual method used to handle the navigation TreeView before collapse event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnTreeViewBeforeCollapse(object sender, TreeViewCancelEventArgs e) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isChecked"></param>
        public virtual void SwitchingCode(bool isChecked) { }

        /// <summary>
        /// Virtual method used to handle the navigation TreeView label edit event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnTreeViewAfterLabelEdit(object sender, NodeLabelEditEventArgs e) { }

        /// <summary>
        /// Virtual method used to handle the Hierarchy Controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void GetApplicantDetails(CaseMstEntity caseMstEntity, List<CaseSnpEntity> caseSNPEntity, string strAgency, string strDept, string strProgram, string strYear, string strType, string strAppDisplay) { }

        /// <summary>
        /// Virtual method used to handle the Hierarchy Controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void GetAgencyDetails(DataRow dr, string strAgency, string strDept, string strProgram, string strYear, string strType, string strAppDisplay) { }


        /// <summary>
        /// Virtual method used to handle the Remove tab pages.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void RemoveTabPages(string strModuleType) { }

        /// <summary>
        /// Virtual method used to handle the addtab clientintake controls
        /// </summary>
        /// <param name="strFormCode"></param>
        public virtual void AddTabClientIntake(string strFormCode) { }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainMenu"></param>
        /// <param name="Count"></param>
        public virtual void GenerateMenus(ref MainMenu mainMenu, string Count)
        {
            try
            {
                mainMenu.MenuItems.Clear();
                MenuManager.InitializeMenus(Count, UserID);
                mainMenu = MenuManager.MainMenu;

                //mainMenu.Location = new System.Drawing.Point(0, 0);
                //mainMenu.Size = new System.Drawing.Size(420, 22);

            }
            catch (Exception ex)
            {
                StackFrame stackFrame = new StackFrame();
                //ExceptionLogger.LogAndDisplayMessageToUser(stackFrame, ex, QuantumFaults.None, ExceptionSeverityLevel.High);
            }
        }

        public virtual void GetTreeView(TreeViewControllerParameter treeViewParam)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void ClearControl() { }

        /// <summary>
        /// Get the maximum favorites/recents count
        /// </summary>
        /// <param name="preferenceNameDisplayCode"></param>
        /// <param name="moduleID"></param>
        /// <returns></returns>
        public virtual int GetPreferenceMaximumItems(string preferenceNameDisplayCode, string moduleID) { return int.Parse(Consts.Common.NullIntegerValue); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listOne"></param>
        /// <param name="secondList"></param>
        /// <returns></returns>
        public virtual bool MoveAllItemsFromOneListToAnotherList(ListBox listOne, ListBox secondList) { return false; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listOne"></param>
        /// <param name="secondList"></param>
        /// <returns></returns>
        public virtual bool MoveSelectedItemsOneListToAnotherList(ListBox listOne, ListBox secondList) { return false; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listBox"></param>
        /// <returns></returns>
        public virtual bool MoveUpListItem(ListBox listBox) { return false; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listBox"></param>
        /// <returns></returns>
        public virtual bool MoveDownListItem(ListBox listBox) { return false; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectID"></param>
        public virtual void CreateSpecificNodesFromCompleted(string objectID) { }

        /// <summary>
        /// Used to open an object in a content tab..
        /// </summary>
        /// <param name="tagClass"></param>
        public virtual void OpenContentTab(TagClass tagClass) { }

        /// <summary>
        /// Open content tab for lifecycle
        /// </summary>
        /// <param name="submissionTagClass"></param>
        /// <param name="moduleName"></param>
        /// <param name="targetObjectIDList"></param>
        public virtual void OpenContentTab(TagClass submissionTagClass, string moduleName, List<string> targetObjectIDList, bool retainSelectedTab) { }

        /// <summary>
        /// Handles the close event of the Generate Builds screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnGenerateBuildsFormClosed(object sender, EventArgs e) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectID"></param>
        public virtual void NavigateToNode(string objectID)
        {
        }

        /// <summary>
        /// Closing the opened ContentTab after deleting the object.
        /// </summary>
        public virtual void DeleteLeafNodeTabs()
        {
        }

        /// <summary>
        /// Used to find a node in a node collection by nodeID.
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        public TreeNode FindNode(TreeNodeCollection nodes, string nodeID)
        {
            foreach (TreeNode treeNode in nodes)
            {
                TagClass tagClass = treeNode.Tag as TagClass;
                if (tagClass.NodeID.Equals(nodeID))
                {
                    return treeNode;
                }
            }
            return null;
        }

        /// <summary>
        /// Process DeAssociate Menu Click
        /// </summary>
        /// <param name="tagClass"></param>
        public virtual void ProcessDeAssociateMenuClick(TagClass tagClass) { }

        public virtual void OnContentTabsCloseClicked(object sender, EventArgs e) { }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an application node.
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        private TreeNode GetApplicationNode(TreeNodeCollection nodes, string nodeID)
        {
            TreeNode applicationNode = null;
            //foreach (TreeNode treeNode in nodes)
            //{
            //    TagClass tagClass = treeNode.Tag as TagClass;
            //    if (tagClass.IsApplication && tagClass.NodeID.Equals(nodeID))
            //    {
            //        return treeNode;
            //    }
            //    if (treeNode.HasNodes && !CommonFunctions.CheckDummyNode(treeNode.Nodes[0]))
            //    {
            //        applicationNode = GetApplicationNode(treeNode.Nodes, nodeID);
            //        if (applicationNode != null) { break; }
            //    }
            //}

            return applicationNode;
        }

        #endregion

        #region Handled Events

        #endregion
    }
}
