/***************************************************************************************
 * Class Name    : MenuManager
 * Author        : 
 * Created Date  : 
 * Version       : 
 * Description   : This class file used to define the menu items
 ****************************************************************************************/

#region Using

using System;
using System.Data;
using System.Diagnostics;

using Wisej.Web;
using Captain.Common.Controllers;
using Captain.Common.Model.Data;
//using Captain.Common.Resources;
using Captain.Common.Utilities;
//using Captain.Common.Views.Controls;
//using Captain.Common.Exceptions;
//using Captain.Common.Security;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using Captain.Common.Model.Objects;
using System.Text;
using System.Drawing;
using System.Xml;
using System.IO;
using System.Linq;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Views.UserControls;
using Captain.Common.Handlers;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Resources;

#endregion

namespace Captain.Common.Menus
{
    public class MenuManager
    {
        #region Variables

        private MainMenu _mainMenu = new MainMenu();

        private bool _isMainMenuEventRegistered = false;
        private string _selectedTab = "";
        private string _selectedNode = string.Empty;
        private string _selectedPath = string.Empty;
        private string _userID = string.Empty;
        private string _locale = string.Empty;
        private string _selectedModulesInSession = string.Empty;

        #region Main Menu Items

        private MenuItem _fileMenu = new MenuItem();
        private MenuItem _editMenu = new MenuItem();
        private MenuItem _viewMenu = new MenuItem();
        private MenuItem _insertMenu = new MenuItem();
        private MenuItem _documentMenu = new MenuItem();
        private MenuItem _favoritesMenu = new MenuItem();
        private MenuItem _toolsMenu = new MenuItem();
        private MenuItem _searchMenu = new MenuItem();
        private MenuItem _reportsMenu = new MenuItem();
        private MenuItem _helpMenu = new MenuItem();
        private ContextMenu _documentPublisherTreeContextMenu = new ContextMenu();
        private ContextMenu _commentsGridContextMenu = new ContextMenu();
        private ContextMenu _issuesGridContextMenu = new ContextMenu();
        private ContextMenu _submissionManagerTreeContextMenu = new ContextMenu();
        private ContextMenu _metadataRepositoryTreeContextMenu = new ContextMenu();
        private ContextMenu _contentPlanContextMenu = new ContextMenu();
        private ContextMenu _advancedSeacrhContextMenu = new ContextMenu();

        #endregion

        #region Menu Items

        //Generic menu items

        //File menu items
        private MenuItem _fileNew = new MenuItem();
        private MenuItem _fileOpen = new MenuItem();
        private MenuItem _fileSave = new MenuItem();
        private MenuItem _fileClose = new MenuItem();
        private MenuItem _fileCloseAll = new MenuItem();
        private MenuItem _fileCheckOut = new MenuItem();
        private MenuItem _fileCheckIn = new MenuItem();
        private MenuItem _fileCancelCheckOut = new MenuItem();
        private MenuItem _fileCancelCheckOutDivider = new MenuItem(Consts.Common.Dash);
        private MenuItem _fileDividerBetweenImportAndCheckOut = new MenuItem(Consts.Common.Dash);
        private MenuItem _fileDownload = new MenuItem();
        private List<MenuItem> _workflows = new List<MenuItem>();
        private MenuItem _fileWorkflow = new MenuItem();
        private MenuItem _fileSendTo = new MenuItem();
        private MenuItem _filePrint = new MenuItem();
        private MenuItem _fileExit = new MenuItem();
        private MenuItem _fileSenToExport = new MenuItem(); //_sendTo sub menu item
        private MenuItem _fileSenToArchive = new MenuItem(); //_sendTo sub menu item
        private MenuItem _fileSenToGateway = new MenuItem(); //_sendTo sub menu item
        private List<MenuItem> _recentlyOpened = new List<MenuItem>();
        private MenuItem _filePrintSelectedSection = new MenuItem();
        private MenuItem _filePageSetup = new MenuItem();
        private List<MenuItem> _recentlyEdited = new List<MenuItem>();
        private MenuItem _fileRecentlyEdited = new MenuItem();
        private MenuItem _fileCreatePDF = new MenuItem();

        //Edit menu items
        private MenuItem _changePassword = new MenuItem();
        private MenuItem _help = new MenuItem();
        private MenuItem _aboutus = new MenuItem();
        private MenuItem _editCopy = new MenuItem();
        private MenuItem _editPaste = new MenuItem();
        private MenuItem _editPasteSpecial = new MenuItem();
        private MenuItem _editDelete = new MenuItem();
        private MenuItem _editFind = new MenuItem();
        private MenuItem _editGoToPage = new MenuItem();
        private MenuItem _editSelectAll = new MenuItem();
        private MenuItem _editRename = new MenuItem();
        private MenuItem _editChangeConceptReference = new MenuItem();

  
        #endregion

        #endregion

        #region Events

        public event EventHandler<MenuManagerEventArgs> MenuItemClick;

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="baseForm"></param>
        public MenuManager(BaseForm baseForm)
        {
            BaseForm = baseForm;
            InitializeConstructor();
        }

        #endregion

        #region Properties

        public BaseForm BaseForm { get; set; }

        public bool IsChildForm { get; set; }

        public MainMenu MainMenu
        {
            get { return _mainMenu; }
            set
            {
                _mainMenu = value;
                if (!_isMainMenuEventRegistered) { Init(); }
            }
        }

        public MenuItem FileMenu
        {
            get { return _fileMenu; }
        }

        public MenuItem EditMenu
        {
            get { return _editMenu; }
        }

        public MenuItem ViewMenu
        {
            get { return _viewMenu; }
        }

        public MenuItem InsertMenu
        {
            get { return _insertMenu; }
        }

        public MenuItem DocumentMenu
        {
            get { return _documentMenu; }
        }

        public MenuItem FavoritesMenu
        {
            get { return _favoritesMenu; }
        }

        public MenuItem ToolsMenu
        {
            get { return _toolsMenu; }
        }

        public MenuItem SearchMenu
        {
            get { return _searchMenu; }
        }

        public MenuItem ReportsMenu
        {
            get { return _reportsMenu; }
        }

        public MenuItem HelpMenu
        {
            get { return _helpMenu; }
        }

        public MenuItem FileCreatePDF
        {
            get { return _fileCreatePDF; }
        }

        public MenuItem FileOpen
        {
            get { return _fileOpen; }
        }

        public MenuItem FileClose
        {
            get { return _fileClose; }
        }

        public MenuItem FileCloseAll
        {
            get { return _fileCloseAll; }
        }

        public MenuItem FileSave
        {
            get { return _fileSave; }
        }

        public MenuItem EditDelete
        {
            get { return _editDelete; }
        }

        public MenuItem EditRename
        {
            get { return _editRename; }
        }

        public MenuItem FilePrint
        {
            get { return _filePrint; }
        }

        public MenuItem FileSendToExport
        {
            get { return _fileSenToExport; }
        }

        public MenuItem EditFind
        {
            get { return _editFind; }
        }

        public MenuItem EditGoToPage
        {
            get { return _editGoToPage; }
        }

        public MenuItem Divider
        {
            get
            {
                MenuItem menuItem = new MenuItem(Consts.Common.Dash);
               //SetMenuHandle(menuItem, Consts.MenuAction.NoAction, Consts.MenuItems.Divider, false, string.Empty, false, false, true, true, string.Empty, Consts.PrivilegeNames.NoPrivilegeAssigned, Consts.PrivilegeNames.NoPrivilegeAssigned);
                return menuItem;
            }
        }

        public MenuItem ThreeDots
        {
            get
            {
                MenuItem menuItem = new MenuItem(Consts.Common.ThreeDots);
                //SetMenuHandle(menuItem, Consts.MenuAction.NoAction, Consts.MenuItems.ThreeDots, false, string.Empty, false, false, true, true, string.Empty, Consts.PrivilegeNames.NoPrivilegeAssigned, Consts.PrivilegeNames.NoPrivilegeAssigned);

                return menuItem;
            }
        }

        public MenuItem HierarchicalEditSelectedAllSiblings { get; set; }

        public MenuItem HierarchicalViewExpandAll { get; set; }

        public MenuItem HierarchicalViewCollapseAll { get; set; }

        public MenuItem HierarchicalTaxonomyContentPlanDivider { get; set; }

        public MenuItem HierarchicalEditDelete { get; set; }

        public MenuItem HierarchicalEditRename { get; set; }

        public string SelectedTab
        {
            get { return _selectedTab; }
        }

        public string SelectedNode
        {
            get { return _selectedNode; }
        }

        public string SelectedPath
        {
            get { return _selectedPath; }
        }

        #endregion

        #region Public Methods


        /// <summary>
        /// Initializes all menus
        /// </summary>
        /// <param name="selectedTab"></param>
        /// <param name="userID"></param>
        public void InitializeMenus(string selectedTab, string userID)
        {
            try
            {
                _userID = userID;
                _selectedTab = selectedTab;

                _fileMenu.MenuItems.Clear();
                _toolsMenu.MenuItems.Clear();
                _helpMenu.MenuItems.Clear();

                //Add Main Menu Items
                _mainMenu.MenuItems.AddRange(new Wisej.Web.MenuItem[] { _fileMenu, _toolsMenu, _helpMenu });

                _fileMenu.MenuItems.AddRange(new MenuItem[] { _fileOpen });
                _toolsMenu.MenuItems.AddRange(new MenuItem[] { _changePassword });
                _helpMenu.MenuItems.AddRange(new MenuItem[] { _help });                
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// To bind the click event of the menu item
        /// </summary>
        /// <param name="menuItem"></param>
        /// <param name="eventHandler"></param>
        public void AddMenuItemEventHandler(MenuItem menuItem, EventHandler eventHandler)
        {
            if (menuItem == null) { return; }
            menuItem.Click -= eventHandler;
            menuItem.Click += eventHandler;
        }

        /// <summary>
        /// To unbind the click event of the menu item
        /// </summary>
        /// <param name="menuItem"></param>
        /// <param name="eventHandler"></param>
        public void RemoveMenuItemEventHandler(MenuItem menuItem, EventHandler eventHandler)
        {
            if (menuItem == null) { return; }
            menuItem.Click -= eventHandler;
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes all menu items.
        /// </summary>
        private void Init()
        {
            _isMainMenuEventRegistered = true;

            #region Menu Items initialization

            //Main menu items
            SetMenuHandle(_fileMenu, Consts.MenuAction.NoAction, "File", false, string.Empty, false, false, true, false, string.Empty);
            SetMenuHandle(_fileOpen, Consts.MenuAction.NoAction, "Open", true, string.Empty, false, false, true, false, string.Empty);
            SetMenuHandle(_toolsMenu, Consts.MenuAction.NoAction, "Tools", false, string.Empty, false, false, true, false, string.Empty);
            SetMenuHandle(_changePassword, Consts.MenuAction.ChangeColor, "Change Password", true, string.Empty, false, false, true, false, string.Empty);
            SetMenuHandle(_helpMenu, Consts.MenuAction.NoAction, "Help", false, string.Empty, false, false, true, false, string.Empty);
            SetMenuHandle(_help, Consts.MenuAction.CaptainHelp, "Captain Help", true, string.Empty, false, false, true, false, string.Empty);


            #endregion
        }

        /// <summary>
        /// Sets the setting for a menu item.
        /// </summary>
        /// <param name="menuItem"></param>
        /// <param name="menuAction"></param>
        /// <param name="menuName"></param>
        /// <param name="isEventDriven"></param>
        /// <param name="menuIconText"></param>
        /// <param name="isEnabled"></param>
        /// <param name="isChecked"></param>
        /// <param name="isVisible"></param>
        /// <param name="isMenuNameResourced"></param>
        /// <param name="toolTip"></param>
        /// <param name="visiblePrivilegeName"></param>
        /// <param name="enabledPrivilegeName"></param>
        public void SetMenuHandle(MenuItem menuItem, string menuAction, string menuName, bool isEventDriven, string menuIconText, bool isEnabled, bool isChecked, bool isVisible, bool isMenuNameResourced, string toolTip)
        {
            try
            {
 
                if (menuName != string.Empty)
                {
                    string shortCut = menuItem.Shortcut.ToString();
                    if (!string.IsNullOrEmpty(shortCut))
                    {
                        shortCut = Consts.Common.OpenParentesis + shortCut + Consts.Common.CloseParentesis;
                    }
                    else
                    {
                        shortCut = string.Empty;
                    }

                    if (isMenuNameResourced)
                    {
                        menuItem.Text = CaptainResourceManager.GetControlString(menuName) + shortCut;
                    }
                    else
                    {
                        menuItem.Text = menuName;
                    }
                }

                //menuItem.ToolTip = toolTip;

                if (isEventDriven)
                {
                    //AddMenuItemEventHandler(menuItem, MainMenuMenuClick); 
                    //menuItem.Click += MenuEventHandler(MainMenuMenuClick); 
                }

            }
            catch (Exception ex)
            {
                StackFrame stackframe = new StackFrame(1, true);
                //ExceptionLogger.LogException(stackframe, ex, ExceptionSeverityLevel.Low); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MenuManagerMenuClick(object sender, MenuItemEventArgs e)
        {
            //MainMenuMenuClick(sender, e);
        }
 
        /// <summary>
        /// To intialize the class
        /// </summary>
        private void InitializeConstructor()
        {
            _locale = Captain<string>.Session[Consts.SessionVariables.Language, Consts.Common.DefaultLanguage];
            _userID = Captain<string>.Session[Consts.SessionVariables.UserID, Consts.Common.DefaultUserID];        

            Init();
        }

        #endregion

        #region Handled Events

        /// <summary>
        /// Event Handler For MainMenu click event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Menu Item Event Args</param>
        public void MainMenuMenuClick(object sender, EventArgs e)
        {
            string url = string.Empty;
            TagClass tag = null;

            try
            {
                url = Application.Uri.AbsoluteUri;
                //IsChildForm = url.Contains(StaticVar.FromsChildForm);

                MenuItem menuItem = (MenuItem)sender;
                tag = (TagClass)menuItem.Tag;

                if (MenuItemClick != null)
                {
                    MenuItemClick(sender, new MenuManagerEventArgs(menuItem.GetTagClass(_userID, _locale).ItemClicked));
                }
            }
            catch (Exception ex)
            {
                StackFrame stackframe = new StackFrame(true);
                //ExceptionLogger.LogException(stackframe, ex, ExceptionSeverityLevel.High);
            }
        }

        #endregion
    }
}

