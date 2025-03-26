/****************************************************************************************
 * Class Name    : Consts
 * Author        : 
 * Created Date  : 
 * Version       : 1.0.0
 * Description   : This class has constants
 ****************************************************************************************/

using System.ComponentModel;
using System.Collections.Generic;


namespace Captain.Common.Utilities
{
    public static class Consts
    {
        public static class Applications
        {
            public const string Administration = "Administration";
            public const string CaseManagement = "CaseManagement";
            public const string HeadStart = "HeadStart";
            public const string AccountsReceivable = "AccountsReceivable";
            public const string EmergencyAssistance = "EmergencyAssistance";
            public const string HousingWeatherization = "HousingWeatherization";
            public const string EnergyCT = "EnergyCT";
            public const string EnergyRI = "EnergyRI";
            public const string DashBoard = "Executive Dashboard";

            public static class Code
            {
                public const string Administration = "01";
                public const string HeadStart = "02";
                public const string CaseManagement = "03";
                public const string AccountsReceivable = "04";
                public const string EmergencyAssistance = "05";
                public const string EnergyCT = "08";
                public const string EnergyRI = "09";
                public const string HousingWeatherization = "10";
                public const string DashBoard = "99";
                public const string HealthyStart = "06";
                public const string AgencyPartner = "11";
            }
        }

        public static class Buttons
        {
            public const string Delete = "Delete";
            public const string Save = "Save";
            public const string Cancel = "Cancel";
            public const string OK = "OK";
        }

        public static class Operations
        {
            public const string New = "new";
            public const string Append = "append";
            public const string Replace = "replace";
            public const string Delete = "delete";
        }

        public static class CellTypes
        {
            public const string DataGridViewDragDropCell = "DataGridViewDragDropCell";
            public const string DataGridViewHierarchicalCell = "DataGridViewHierarchicalCell";
            public const string DataGridViewComboBoxCell = "DataGridViewComboBoxCell";
            public const string DataGridViewDateTimePickerCell = "DataGridViewDateTimePickerCell";
            public const string DataGridViewTextBoxCell = "DataGridViewTextBoxCell";
            public const string DataGridViewCheckBoxCell = "DataGridViewCheckBoxCell";
            public const string DataGridViewLinkCell = "DataGridViewLinkCell";
        }

        public static class CacheKeys
        {
            public static readonly object Locker = new object();
        }

        public static class DateTimeFormats
        {
            /// <summary>
            /// Used to save Date. Format is: yyyyMMdd.
            /// </summary>
            public const string DateSaveFormat = "yyyyMMdd";

            /// <summary>
            /// Used to Display Date.  Format is: dd-MMM-yyyy.
            /// </summary>
            public const string DateDisplayFormat = "MM/dd/yyyy";

            /// <summary>
            /// Used to save DateTime. Format is: yyyyMMddHHmmss.
            /// </summary>
            public const string DateTimeSaveFormat = "yyyyMMddHHmmss";

            /// <summary>
            /// Used to Display DateTime. Format is: dd-MMM-yyyy HH:mm:ss.
            /// </summary>
            public const string DateTimeDisplayFormat = "dd-MMM-yyyy HH:mm:ss";

            /// <summary>
            /// Used to Display DateTime. Format is: MM-dd-yyyy HH:mm:ss.
            /// </summary>
            public const string DateTimeStandredFormat = "MM-dd-yyyy HH:mm:ss";

            /// <summary>
            /// Used to Display DateTime. Format is: HH:mm:ss.
            /// </summary>
            public const string TimeDisplayFormat = "HH:mm:ss";

            /// <summary>
            /// Used to convert from timestamp to other formats. Format is: yyyy-MM-ddTHH:mm:ss.fffK
            /// </summary>
            public const string TimeStamp = "yyyy-MM-ddTHH:mm:ss.fffK";

            /// <summary>
            /// Used to convert to system date format. Format is: MM/dd/yyyy
            /// </summary>
            public const string SystemDateFormat = "MM/dd/yyyy";

            /// <summary>
            /// Used to Convert system date format.Format is: yyyy-MM-dd
            /// </summary>
            public const string SearchDateFormat = "yyyy-MM-dd";

            /// <summary>
            /// Used to Convert system date format.Format is: yyyy
            /// </summary>
            public const string FourLetterYear = "yyyy";

            /// <summary>
            /// Used to Display DateTime. Format is: MM-dd-yyyy HH:mm.
            /// </summary>
            public const string DateTimeStandredFormatWithNoSeconds = "MM-dd-yyyy HH:mm";
        }

        public static class Security
        {
            public const string EncryptionKey = "0000111122223333444455556666777788889999AAAABBBBCCCCDDDDEEEEFFFF";
            public const string EncryptionIV = "FFFFEEEEDDDDCCCCBBBBAAAA9999888877776666555544443333222211110000";
        }

        public static class PropertyNames
        {
            public const string AccessLevelName = "AccessLevelName";
            public const string GroupsAndUsers = "GroupsAndUsers";
            public const string TeamRoleName = "teamRoleName";
            public const string ObjectName = "ObjectName";
            public const string ItemClicked = "ItemClicked";
            public const string ApplicationNode = "ApplicationNode";
            public const string ErrorMessage = "ErrorMessage";
            public const string GlobalGuid = "GlobalGuid";
            public const string InboxObject = "InboxObject";
            public const string IsSaveValid = "IsSaveValid";
            public const string Locale = "Locale";
            public const string ReadOnly = "ReadOnly";
            public const string ReleaseDate = "ReleaseDate";
            public const string ReleaseLabel = "ReleaseLabel";
            public const string SelectedNodeTagClass = "SelectedNodeTagClass";
            public const string SelectedReportTagClass = "SelectedReportTagClass";
            public const string SubmissionGroupBox = "SubmissionGroupBox";
            public const string SubmissionNode = "SubmissionNode";
            public const string SubmissionType = "SubmissionType";
            public const string TaxonomyNode = "TaxonomyNode";
            public const string TaxonomyNodeList = "TaxonomyNodeList";
            public const string TeamRoles = "TeamRoles";
            public const string Tier = "Tier";
            public const string TitleBarText = "TitleBarText";
            public const string TradeNamesItems = "TradeNamesItems";
            public const string UserID = "UserID";
            public const string Name = "Name";
            public const string ObjectID = "ObjectID";
            public const string ParamNameValueList = "ParamNameValueList";
            public const string CommentTypeDisplay = "CommentTypeDisplay";
            public const string CommentText = "CommentText";
            public const string PageNumber = "PageNumber";
            public const string IsPublicDisplay = "IsPublicDisplay";
            public const string UserFullName = "UserFullName";
            public const string PaperSize = "PaperSize";
            public const string Finishing = "Finishing";
            public const string Copies = "Copies";
            public const string Notes = "Notes";
            public const string Volumes = "Volumes";
            public const string Visible = "Visible";
            public const string SendEmail = "SendEmail";
            public const string PriorityName = "PriorityName";
            public const string SelectedTreeViewNode = "SelectedTreeViewNode";
            public const string SelectedTreeNode = "SelectedTreeNode";
            public const string InitialisedWorkFlowIDs = "InitialisedWorkFlowIDs";
            public const string Attachments = "Attachments";
            public const string MenuAction = "MenuAction";
            public const string CodeListEntity = "CodeListEntity";
            public const string Tag = "Tag";
        }

        public static class Common
        {

            public static string ServerLocation = "\\\\" + System.Environment.MachineName + "\\"; // To Run At Serverl̥

            //TODO: See changes in LookupDataAccess.GetReportPath(); //Alaa: Leave this one here for now
            //if(System.Environment.MachineName=="SYSTEM-1")
            //ServerLocation = "";                                          // To Run At Local


            public const string ICaptainPlugin = "Captain.Common.Interfaces.ICaptainPlugin";
            public const string XmlSerializers = "XmlSerializers";

            public const string SystemRepository = "system=repository";
            public const string SystemRepositoryID = "repositoryid=2";


            public const string ControlResource = "Captain.Common.Resources.Controls.Resource_";
            public const string MessagesResource = "Captain.Common.Resources.Messages.Resource_";
            public const string ReturnedInformation = "Returned Information: ";

            public const string HtmlLineBreak = "<br />";
            public const string HtmlBoldStart = "<b>";
            public const string HtmlBoldEnd = "</b>";
            public const string HtmlUnorderedListStart = "<ul>";
            public const string HtmlUnorderedListEnd = "</ul>";
            public const string HtmlListItemStart = "<li>";
            public const string HtmlListItemEnd = "</li>";

            public const string AllFiles = "*.*";

            public const string ShowControl = "showcontrol";
            public const string LoadInitialize = "loadinitialize";
            public const string LoadSecurityCheck = "loadsecuritycheck";
            public const string ContentType = "Content-Type";
            public const string DownloadContentLength = "Content-Length";
            public const string ApplicationOctetStream = "application/octet-stream";
            public const string ContentDisposition = "Content-Disposition";
            public const string AttachmentFile = "attachment; filename=";
            public const string Size = "size";
            public const string TargetNew = "_new";
            public const string TargetSelf = "_self";

            public const string Csa = "CSA";
            public const string Documentum = "Documentum";
            public const string Resolution = "Resolution";

            public const string ThreeDots = "...";
            public const string TaskNodeTemplate = "<{0} xmlns=\"http://xmlns.oracle.com/bpel/workflow/task\">{1}</{0}>";
            public const string TaskPrefix = "task";
            public const string TaskNamespace = "http://xmlns.oracle.com/bpel/workflow/task";
            public const string Update = "UPDATE";
            public const string Where = "WHERE";
            public const string Set = "SET";
            public const string MsiQuote = "`";
            public const string SingleQuote = "'";
            public const string WordSingleQuote = "’";
            public const string XmlVersion = "1.0";
            public const string XmlEncoding = "utf-8";
            public const string EndOfFile = "\0";

            public const string Nodes = "nodes";
            public const string None = "none";
            public const string SearchResultsObjectName = "SearchResults";
            public const string AdvancedSearchObjectName = "AdvancedSearch";
            public const string AdvancedSearch = "Advanced Search";
            public const string ContentTab = "ContentTab";
            public const string NullIntegerValue = "-1";
            public const string DocsRequested = "150";
            public const string DepthFour = "4";
            public const string DepthFive = "5";
            /// <summary>
            /// Used to retrieve the user cache expiration timeout.
            /// </summary>
            public const string CacheExpiration = "CacheExpiration";

            /// <summary>
            /// Used to retrieve the system cache expiration timeout.
            /// </summary>
            public const string SystemCacheExpiration = "SystemCacheExpiration";
            public const string AllDll = "*.dll";

            /// <summary>
            /// Configuration options constants
            /// </summary>
            public const string FriendlyMessageColors = "FriendlyMessageColors";
            public const string FriendlyMessageDefaultColors = "FFF98F,FFFFCC,000000~FFF98F,FFFFCC,000000~FFF98F,FFFFCC,000000~FFF98F,FFFFCC,000000~477FB9,4AA6D5,FFFFFF~477FB9,4AA6D5,FFFFFF~477FB9,4AA6D5,FFFFFF~477FB9,4AA6D5,FFFFFF";
            public const string IsDebugMode = "IsDebugMode";
            public const string ErrorLogs = "ErrorLogs";
            public const string Log4Net = "Log4Net";
            public const string ExceptionLogExtension = "ExceptionLogExtension";
            public const string DefaultExceptionLogsPath = @"C:\Captain\Logs\";
            public const string ExceptionLogsPath = "ExceptionLogsPath";
            public const string ExceptionLogEnabled = "ExceptionLogEnabled";
            public const string UserNameTemplate = "UserNameTemplate";
            public const string IsEditingEnabled = "IsEditingEnabled";

            /// <summary>
            /// asc
            /// </summary>
            public const string Ascending1 = "asc";

            /// <summary>
            /// ascending
            /// </summary>
            public const string Ascending2 = "ascending";

            /// <summary>
            /// desc
            /// </summary>
            public const string Descending1 = "desc";

            /// <summary>
            /// descending
            /// </summary>
            public const string Descending2 = "descending";

            public const string Reassociate = "reassociate";

            public const string Inbox = "Inbox";
            public const string Tasks = "Tasks";
            public const string TasksPriotiry = "TasksPriotiry";
            public const string TasksRecentlyCompleted = "TasksRecentlyCompleted";
            public const string Active = "Active";
            public const string Priority = "Priority";
            public const string Recently = "Recently";
            public const string Issues = "Issues";
            public const string Issue = "Issue";
            public const string Action = "Action";
            public const string UnRead = "Unread";
            public const string IssuesIdentified = "Issues I Identified";
            public const string Notifications = "Notifications";

            public const string Filter = "filter";
            public const string Clear = "clear";
            public const string Public = "public";
            public const string EmptyValue = "Empty Value";
            public const string ClearValue = "Clear Value";
            public const string SelectOne = "Select One";
            public const string UnDefined = "Undefined";
            public const string UpdateValue = "Update";
            public const string None1 = "None";

            public const string All = "All";
            public const string Text = "Text";
            public const string Value = "Value";
            public const string Name = "Name";
            public const string ID = "ID";
            public const string BadID = "Id";
            public const string Open = "Open";
            public const string Complete = "Complete";
            public const string Success = "success";
            public const string Cancelled = "Cancelled";
            public const string ReturnNewLine = "\r\n";

            public const string Upload = "upload";
            public const string Download = "download";
            public const string File = "file";

            public const string Assembly = "Assembly";

            public const string Commands = "Commands";

            public const string Metadata = "Metadata";

            public const string ExistingStateOfPackagesHasBeenDiscarded = "existing state of packages has been discarded";

            public const string Email = "email";

            public const string Row = "R";

            public const string OfSpace = "of ";

            /// <summary>
            /// Use to get the tag value of a tag property
            /// </summary>
            public const string Tag = "Tag";

            /// <summary>
            /// Regex to change a string to title case.
            /// </summary>
            public const string TitleCaseRegEx = @"\b(\w)";

            /// <summary>
            /// Error message from controls.
            /// </summary>
            public const string ErrorMessage = "errormessage";

            /// <summary>
            /// The height of the Captain banner.
            /// </summary>
            public const int CaptainBannerHeight = 60;

            /// <summary>
            /// Used to get the height of the toolbar.
            /// </summary>
            public const int ToolBarHeight = 28;

            /// <summary>
            /// Used to get the hieght of the minimized details pane.
            /// </summary>
            public const int DetailsPaneMinimizedHeight = 36;

            /// <summary>
            /// 
            /// </summary>
            public const string WinControl = "WinControl";

            /// <summary>
            /// This is used for comparing a BaseUserControl against the inherited NoContentControl.
            /// </summary>
            public const string NoContentControl = "NoContentControl";

            /// <summary>
            /// The caption used in message boxes.
            /// </summary>
            public const string ApplicationCaption = "CAPTAIN";

            /// <summary>
            /// This caption is used for the form title which displays in IE Tab.
            /// </summary>
            public const string ApplicationCaptionNoSpaces = "Captain";

            /// <summary>
            /// Used to identify the object type of the main form.
            /// </summary>
            public const string MainForm = "MainForm";

            /// <summary>
            /// Used to identify the update form.
            /// </summary>
            public const string UpdateForm = "UpdateForm";

            /// <summary>
            /// Used to identify the object type of the child form.
            /// </summary>
            public const string ChildForm = "ChildForm";

            /// <summary>
            /// Used as the default language locale.
            /// </summary>
            public const string DefaultLanguage = "en-US";

            /// <summary>
            /// Used as the default userId .
            /// </summary>
            public const string DefaultUserID = "0";

            /// <summary>
            /// Used to get the client controls path from the web.config.
            /// </summary>
            public const string ClientControlPath = "ClientControlPath";

            /// <summary>
            /// Used to get the server controls path from the web.config.
            /// </summary>
            public const string ServerControlPath = "ServerControlPath";

            /// <summary>
            /// 
            /// </summary>
            public const string CaptainHelpPath = "CaptainHelpPath";

            public const string TutorialsPath = "TutorialsPath";

            /// <summary>
            /// Used to get the language
            /// </summary>
            public const string Language = "Language";

            /// <summary>
            /// Used to get the FromPath
            /// </summary>
            public const string FromPath = "FromPath";

            /// <summary>
            /// Used to get the ToPath
            /// </summary>
            public const string ToPath = "ToPath";

            /// <summary>
            /// 
            /// </summary>
            public const string Controls = @"Controls\";

            /// <summary>
            /// Used as the default client path.
            /// </summary>
            public const string DefaultClientRepositoryPath = @"C:\Captain\Temp\";

            /// <summary>
            /// Used as the default server plugins path.
            /// </summary>
            public const string DefaultPluginsPath = @"C:\Captain\Plugins\";

            /// <summary>
            /// Used as the default server files path.
            /// </summary>
            public const string DefaultUpDownRootPath = @"C:\CDBOutput\";

            /// <summary>
            /// Used as the default client controls path.
            /// </summary>
            public const string DefaultClientControlPath = @"C:\Captain\Controls\";

            /// <summary>
            /// 
            /// </summary>
            public const string NewLine = "\n";

            /// <summary>
            /// Multiple purpose use dash (-) character in string format.
            /// </summary>
            public const string Dash = "-";

            /// <summary>
            /// Multiple purpose use space ( ) character in string format.
            /// </summary>
            /// <remarks>DO NOT USE ALT-255 FOR THIS CONSTANT.</remarks>
            public const string Space = " ";

            /// <summary>
            /// Used to search and replace with the Space constant.
            /// </summary>
            public const string ReplacableSpace = "_space_";

            /// <summary>
            /// Multiple purpose use colon (:) character in string format.
            /// </summary>
            public const string Colon = ":";

            /// <summary>
            /// Multiple purpose use colon (;) character in string format.
            /// </summary>
            public const string SemiColon = ";";

            /// <summary>
            /// Used for concatenating with any form title to hide the IE default dialog title.
            /// </summary>
            public const string TitleSpaces = "";

            /// <summary>
            /// Used as a measurment.
            /// </summary>
            public const string Pixel = "px";

            /// <summary>
            /// Multiple purpose use percent sign (%) character in string format.
            /// </summary>
            public const string PercentSign = "%";

            /// <summary>
            /// Multiple purpose use front slash (/) character in string format.
            /// </summary>
            public const string Slash = "/";

            /// <summary>
            /// Multiple purpose use back slash (\) character in string format.
            /// </summary>
            public const string BackSlash = @"\";

            /// <summary>
            /// Multiple purpose use amper sign (&) character in string format.
            /// </summary>
            public const string AmperSign = "&";

            /// <summary>
            /// Multiple purpose use question mark (?) character in string format.
            /// </summary>
            public const string QuestionMark = "?";

            /// <summary>
            /// Multiple purpose use double back slash (\\) characters in string format.
            /// </summary>
            public const string DoubleBackSlash = @"\\";

            /// <summary>
            /// Multiple purpose use double slash (//) characters in string format.
            /// </summary>
            public const string DoubleSlash = "//";

            /// <summary>
            /// Multiple purpose use pipe (|) character in string format.
            /// </summary>
            public const string Pipe = "|";

            /// <summary>
            /// Used to get a report option value an other.
            /// </summary>
            public const string LessThan = "<";

            /// <summary>
            /// Used to get a report option value an other.
            /// </summary>
            public const string GreaterThan = ">";

            /// <summary>
            /// Multiple purpose use coma (,) character in string format.
            /// </summary>
            public const string Comma = ",";

            /// <summary>
            /// Used to escape a command in a string.
            /// </summary>
            public const string CommaEscape = "[comma]";

            /// <summary>
            /// Multiple purpose use at sign (@) character in string format.
            /// </summary>
            public const string AtSign = "@";

            /// <summary>
            /// Multiple purpose use equal (=) character in string format.
            /// </summary>
            public const string EqualsSign = "=";

            /// <summary>
            /// Multiple purpose use caret (^) character in string format.
            /// </summary>
            public const string Caret = "^";

            /// <summary>
            /// Multiple purpose use period (.) character in string format.
            /// </summary>
            public const string Period = ".";

            /// <summary>
            /// Multiple purpose use double (..) period characters in string format.
            /// </summary>
            public const string DoublePeriods = "..";

            /// <summary>
            /// Multiple purpose use hash/pound (#) sign character in string format.
            /// </summary>
            public const string Sharp = "#";

            /// <summary>
            /// Used to get the upload/download path from the web.config.
            /// </summary>
            public const string UpDownRootPath = "UpDownRootPath";

            /// <summary>
            /// Used in the CommonFunctions.GetNextSequenceName to set as the initial sequence when one is not found.
            /// </summary>
            public const string InitialSequenceNumber = "0000";

            /// <summary>
            /// Used to get the upload/download physical path from the web.config.
            /// </summary>
            public const string UpDownRootDrive = "UpDownRootDrive";

            /// <summary>
            /// Used to compare is an icon name contains "Document" as the part of the name.
            /// </summary>
            public const string Document = "Document";

            /// <summary>
            /// Multiple purpose use tilda (~) character in string format.
            /// </summary>
            public const string Tilda = "~";

            /// <summary>
            /// Multiple purpose use underscore/underline (_) character in string format.
            /// </summary>
            public const string Underscore = "_";

            /// <summary>
            /// Used as the name of a dummy node added in order to simulate sub nodes.
            /// </summary>
            public const string Dummy = "dummy";

            /// <summary>
            /// Multiple purpose use open parentesis [(] character in string format.
            /// </summary>
            public const string OpenParentesis = "(";

            /// <summary>
            /// Multiple purpose use close parentesis [)] character in string format.
            /// </summary>
            public const string CloseParentesis = ")";

            /// <summary>
            /// Multiple purpose use open bracket [[] character in string format.
            /// </summary>
            public const string OpenBracket = "[";

            /// <summary>
            /// Multiple purpose use close bracket []] character in string format.
            /// </summary>
            public const string CloseBracket = "]";

            /// <summary>
            /// Multiple purpose use open curly bracket [{] character in string format.
            /// </summary>
            public const string OpenCurlyBracket = "{";

            /// <summary>
            /// Multiple purpose use close curly bracket [}] character in string format.
            /// </summary>
            public const string CloseCurlyBracket = "}";

            /// <summary>
            /// Used to get the client repository path (local client Captain file path) from the web.config.
            /// </summary>
            public const string ClientRepositoryPath = "ClientRepositoryPath";

            /// <summary>
            /// The default path to the client repository.
            /// </summary>
            public const string ClientRepositoryDefaultPath = @"C:\Captain\Temp\";

            /// <summary>
            /// Used to get the friendly message view delay from the web.config.
            /// </summary>
            public const string FriendlyMessageDelay = "FriendlyMessageDelay";

            /// <summary>
            /// Used to compare an Icon based on the operator for a node.
            /// </summary>
            public const string OperatorOnly = ".{0}.";

            /// <summary>
            /// Used as a parameter for the WinControlParamItem object.
            /// </summary>
            public const string ShellFileOnDownload = "ShellFileOnDownload";

            /// <summary>
            /// Used to set a default value of 0.
            /// </summary>
            public const string Zero = "0";

            /// <summary>
            /// Used to display zero with two decimals.
            /// </summary>
            public const string ZeroTwoDecimal = "0.00";

            /// <summary>
            /// Used to construct the xml
            /// </summary>
            public const string XMLDeclaration = "<?xml version=\'1.0\' encoding=\'utf-8\'?>";

            /// <summary>
            /// 
            /// </summary>
            public const string XMLDeclarationEnd = "?>";

            /// <summary>
            /// Identity for the UserEntity object
            /// </summary>
            public const string User = "User";

            /// <summary>
            /// Identity for the ObjectGroupEntity object
            /// </summary>
            public const string UserGroup = "UserGroup";

            /// <summary>
            /// To indicates the row state
            /// </summary>
            public const string Add = "Add";

            /// <summary>
            /// To indicates the row state
            /// </summary>
            public const string Edit = "Edit";

            /// <summary>
            /// To indicates the row state
            /// </summary>
            public const string Delete = "Delete";

            /// <summary>
            /// To indicates the row state
            /// </summary>
            public const string View = "View";

            /// <summary>
            /// To Use in Advanced searchControl
            /// </summary>
            public const string ColonEqual = ":=";

            /// <summary>
            /// To Use in Advanced searchControl
            /// </summary>
            public const string ColonGreaterThan = ":>";

            /// <summary>
            /// To Use in Advanced searchControl
            /// </summary>
            public const string ColonLessThan = ":<";

            /// <summary>
            /// To Use in Communication searchControl
            /// </summary>
            public const string ColonGreaterOrEqualThan = ":>=";

            /// <summary>
            /// To Use in Communication searchControl
            /// </summary>
            public const string ColonLessOrEqualThan = ":<=";

            /// <summary>
            /// To Use in Advanced searchControl
            /// </summary>
            public const string Star = "*";

            /// <summary>
            /// To Use in Advanced searchControl
            /// </summary>
            public const string AndJoinOperator = " & ";

            /// <summary>
            /// To Use in Advanced searchControl
            /// </summary>
            public const string OrJoinOperator = " | ";

            /// <summary>
            /// To Use in Advanced searchControl
            /// </summary>
            public const string Or = "OR";

            /// <summary>
            /// The number 1
            /// </summary>
            public const string One = "1";

            /// <summary>
            /// The number 2
            /// </summary>
            public const string Two = "2";

            /// <summary>
            /// The number 3
            /// </summary>
            public const string Three = "3";

            /// <summary>
            /// The number 4
            /// </summary>
            public const string Four = "4";

            /// <summary>
            /// New
            /// </summary>
            public const string New = "New";

            /// <summary>
            /// To Use in QueryItem Control
            /// </summary>
            public const string AddRow = "Add Row";

            public const string SelectAll = "Select All";
            public const string UnSelectAll = "Unselect All";

            public const string AddedBy = "Added By     : ";
            public const string On = " on ";
            public const string ModifiedBy = "Modified By : ";

            public const string TabSpace = "    ";

            public const string HTMLTabSpace = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

            public const string Exists = "Exists";

            public const string Tmp_ReportFolderLocation = "\\\\cap-dev\\C-Drive\\CapReports\\"; // Run At Server
            //public const string Tmp_ReportFolderLocation = "C:\\CapReports\\";                  //  Run At Local host
            public const string ReportFolderLocation = "";                  //  Run At Local host
            //public const string ReportFolderLocation = "\\\\cap-dev\\C-Drive\\CapReports\\"; // Run At Server

            //  public const string ServerLocation = "";
            //  public const string ServerLocation = "\\\\CAP-ts\\";         


            public const string Standard = "Standard";

            public const string Custom = "Custom";

            public const string DB_Exception = "DataBase Exception";
        }

        public static class Controls
        {
            public const string Welcome = "lblWelcome";

            /// <summary>
            /// Used to display Page of TotalPages.
            /// </summary>
            public const string BlankOfBlank = "lblBlankOfBlank";
            public const string InformationTab = "tabInformation";
            public const string Information = "Information";
            public const string ButtonOK = "btnOK";
            public const string ButtonDelete = "btnDelete";
            public const string ButtonSave = "btnSave";
            public const string ButtonCancel = "btnCancel";
            public const string LogoutLabel = "lblLogout";
            public const string HelpLabel = "lblHelp";
            /// <summary>
            /// Gets the FieldChooser form text
            /// </summary>
            public const string FieldChooserTitle = "frmFieldChooser";

        }

        public static class Parameters
        {
            public const string Zero = "{0}";
            public const string One = "{1}";
            public const string Two = "{2}";
            public const string Three = "{3}";
            public const string Four = "{4}";
            public const string FirstName = "[firstname]";
            public const string LastName = "[lastname]";
            public const string MiddleName = "[middlename]";
            public const string Title = "[title]";
            public const string Email = "[email]";
            public const string UserName = "[username]";
            public const string UserID = "[userid]";
            public const string CompanyID = "[companyid]";
            public const string ReportName = "{REPORTNAME}";
        }

        public static class Javascript
        {
            /// <summary>
            /// Script manager control.
            /// </summary>
            public const string ScriptManagerControl = "ScriptManagerControl";

            /// <summary>
            /// The common actions control.
            /// </summary>
            public const string CommonActionsControl = "CommonActions";

            /// <summary>
            /// Parameters are: {0} - Grid Guid, {1} - RowNumber, {2} - DataID
            /// </summary>
            public const string DropDownAfterMethod = "mobjApp.DataGridView_DropDownAfter";

            /// <summary>
            /// Executes code inside the iframe. Parameters: controlName, code
            /// </summary>
            public const string ExecuteChildMethod = "mobjApp.WinControl_ExecuteChild";

            /// <summary>
            /// Executes code inside the control iframe and hides the iframe. Parameters: controlName, code
            /// Uses the CloseDocumentExecuteCode as the code.
            /// </summary>
            public const string CloseAndHideControlMethod = "mobjApp.WinControl_CloseAndHideControl";

            /// <summary>
            /// Hides as section of a wincontrol iframe. Parameters: hide
            /// </summary>
            public const string ShowHideFrame = "mobjApp.WinControl_ShowHideFrame";

            /// <summary>
            /// Executes code inside the iframe. Parameters: controlName, code
            /// </summary>
            public const string ExecuteParentMethod = "mobjApp.WinControl_ExecuteParent";

            /// <summary>
            /// Executes code inside the iframe and then outside. Parameters are: controlName, parentCode, childCode
            /// </summary>
            public const string ExecuteChildParentMethod = "mobjApp.WinControl_ExecuteChildParent";

            /// <summary>
            /// Executes code inside the iframe and then outside. Parameters are: controlName, parentCode, childCode
            /// </summary>
            public const string ExecuteParentChildMethod = "mobjApp.WinControl_ExecuteParentChild";

            /// <summary>
            /// Executes code to set IE to full screen or normal screen. Parameter is: true or false.
            /// </summary>
            public const string ExecuteFullScreenModeMethod = "mobjApp.WinControl_FullScreen";

            /// <summary>
            /// Used to print a control.
            /// </summary>
            public const string Print = "mobjApp.WinControl_PrintSection";

            /// <summary>
            /// Closes a window.
            /// </summary>
            public const string CloseWindowMethod = "mobjApp.ScriptManagerControl_CloseWindow";

            /// <summary>
            /// Closes the IE Browser
            /// </summary>
            public const string CloseBrowser = "    document.getElementById('objectControl').CloseBrowser();\r\n";

            /// <summary>
            /// Sets a command to be process by a winform control.
            /// Format parameters are: {0} - Command
            /// </summary>
            public const string ProcessCommandCode = "    document.getElementById('objectControl').ProcessCommand('{0}');\r\n";

            /// <summary>
            /// Begining of a javascript call to a control.
            /// </summary>
            public const string BeginJavascriptCode = "var oControl = document.getElementById('objectControl');\r\nif(oControl)\r\n{\r\n";

            /// <summary>
            /// Ending of a javascript call to a control.
            /// </summary>
            public const string EndJavascriptCode = "}\r\n";

            /// <summary>
            /// Opens the content repository browser. Format parameters are: {0} - UserID, {1} - IsFilesShow, {2} - OperationType, {3} - ObjectID
            /// </summary>
            public const string OpenContentBrowserCode = " document.getElementById('objectControl').UserID = {0};\r\n document.getElementById('objectControl').IsFilesShow = {1};\r\n document.getElementById('objectControl').OperationType = '{2}';\r\n document.getElementById('objectControl').ObjectID = '{3}';\r\n document.getElementById('objectControl').ShowCustomBrowser();\r\n";

            /// <summary>
            /// Sets the goto url of the control
            /// Format parameters are: {0} - GotoUrl
            /// </summary>
            public const string GoToUrlCode = "    document.getElementById('objectControl').GotoUrl = '{0}';\r\n";

            /// <summary>
            /// Export on the client's pc
            /// Format parameters are:  {0} - objectID, {1} - exportType, {2} - exportNumber
            /// </summary>
            public const string Export = "    document.getElementById('objectControl').Export('{0}','{1}','{2}');\r\n";

        }

        public static class ProcessCommands
        {
            /// <summary>
            /// Sets the Document view stae.
            /// Format parameters is: {0} - viewstate string 
            /// </summary>
            public const string SetViewState = "setviewstate,{0}";
        }

        public static class Icons24x24
        {
            public const string Administration = "_24X24.Admin.gif";
        }

        public static class Icons
        {
            public const string ico_Save    = "icon-save";
            public const string ico_Blank   = "blank";
            public const string ico_Tick    = "icon-gridtick";
            public const string ico_Cross   = "icon-cross";
            public const string ico_Add     = "captain-add";
            public const string ico_Edit    = "captain-edit";
            public const string ico_Delete = "captain-delete";
            public const string ico_CaseNotes_New = "captain-casenotesadd";
            public const string ico_CaseNotes_View = "captain-casenotes";
            public const string ico_Help = "icon-help";

            public const string ico_Left_Arrow = "icon-leftArrow";
            public const string ico_Up_Arrow = "icon-uparrow";
            public const string ico_Righ_Arrow = "icon-rightarrow";
            public const string ico_Down_Arrow = "icon-downarrow";
            
            public const string ico_Left_up_Corner_Arrow = "icon-left-up-arrow";

            public const string ico_lg_Left_Arrow = "icon-lg-leftarrow";
            public const string ico_lg_Up_Arrow = "icon-lg-uparrow";
            public const string ico_lg_Righ_Arrow = "icon-lg-rightarrow";
            public const string ico_lg_Down_Arrow = "icon-lg-downarrow";

            public const string ico_checkbox = "checkbox";
            public const string ico_undefined_checkbox = "checkbox-undefined";
            public const string ico_Checked_checkbox = "checkbox-checked";

            public const string ico_SP_btn_plain = "sP_btn_plain";
            public const string ico_SP_btn_dot = "sP_btn_dot";

            public const string ico_Instructions = "icon-instructions";
            public const string ico_History = "captain-caseHistory";

            public const string ico_DocUpload = "captain-Docupload";

        }
        public static class Icons16x16
        {
            public const string SignDocument = "_16X16.SignDocument.gif";
            public const string EnableDragDrop = "_16X16.EnableDragDrop.gif";
            public const string FitWidth = "_16X16.FitWidth.gif";
            public const string FitHeight = "_16X16.FitHeight.gif";
            public const string SinglePage = "_16X16.SinglePage.gif";
            public const string SingleContinuousPages = "_16X16.SingleContinuousPages.gif";
            public const string MultiplePages = "_16X16.MultiplePages.gif";
            public const string MultipleContinuousPages = "_16X16.MultipleContinuousPages.gif";
            public const string PreviousView = "_16X16.PreviousView.gif";
            public const string NextView = "_16X16.NextView.gif";
            public const string FirstPage = "_16X16.FirstPage.gif";
            public const string PreviousPage = "_16X16.PreviousPage.gif";
            public const string NextPage = "_16X16.NextPage.gif";
            public const string LastPage = "_16X16.LastPage.gif";
            public const string ZoomIn = "_16X16.ZoomIn.gif";
            public const string ZoomOut = "_16X16.ZoomOut.gif";
            public const string SetDestination = "_16X16.SetDestination.gif";
            public const string MagnifyingGlass = "_16X16.MagnifyingGlass.gif";
            public const string QueryOr = "_16X16.QueryOr.gif";
            public const string QueryAnd = "_16X16.QueryAnd.gif";
            public const string STFBook = "_16X16.STFBook.gif";
            public const string ErrorX = "_16X16.ErrorX.gif";
            public const string CheckOK = "_16X16.CheckOK.gif";
            public const string CancelDestination = "_16X16.CancelDestination.gif";
            public const string Refresh = "_16X16.Refresh.gif";
            public const string Find = "_16X16.Find.gif";
            public const string Delete = "_16X16.Delete.gif";
            public const string Save = "_16X16.Save.gif";
            public const string Save1 = "_16X16.Save1.gif";
            public const string Export = "_16X16.Export.gif";
            public const string Print = "_16X16.Print.gif";
            public const string MSExcel = "_16X16.MSExcel.gif";
            public const string Download = "_16X16.Download.gif";
            public const string EditIcon = "_16X16.EditIcon.gif";
            public const string Entry = "_16X16.Entry.gif";
            public const string TabSheet = "_16X16.TabSheet.gif";
            public const string SlipSheet = "_16X16.SlipSheet.gif";
            public const string Open = "_16X16.Open.gif";
            public const string ResetChanges = "_16X16.ClearForm.gif";
            public const string MinimizeTabs = "_16X16.MinimizeTabs.gif";
            public const string MaximizeTabs = "_16X16.MaximizeTabs.gif";
            public const string MaximizeTabsDisabled = "_16X16.MaximizeTabsDisabled.gif";
            public const string Message = "_16X16.Messages.gif";
            public const string Task = "_16X16.Task.gif";
            public const string Note = "_16X16.Note.gif";
            public const string DropDownArrow = "_16X16.DropDownArrow.gif";
            public const string DropDownArrowDown = "_16X16.DropDownArrowDown.gif";
            public const string DropDownArrowUp = "_16X16.DropDownArrowUp.gif";
            public const string Spacer = "_16X16.Spacer.gif";
            public const string Document = "_16X16.Document.gif";
            public const string AddItem = "_16X16.AddItem.gif";
            public const string DeleteItem = "_16X16.DeleteItem.gif";
            public const string Link = "_16X16.Link.gif";
            public const string CancelChanges = "_16X16.CancelChanges.gif";
            public const string NoContent = "_16X16.NoContent.gif";
            public const string Hand = "_16X16.Hand.gif";
            public const string TextSelection = "_16X16.TextSelection.gif";
            public const string ArrowDown = "_16X16.ArrowDown.gif";
            public const string ArrowUp = "_16X16.ArrowUp.gif";
            public const string ArrowDownDisabled = "_16X16.ArrowDownDisabled.gif";
            public const string ArrowUpDisabled = "_16X16.ArrowUpDisabled.gif";
            public const string FieldChooser = "_16X16.FieldChooser.gif";
            public const string User = "_16X16.User.gif";
            public const string Users = "_16X16.Users.gif";
            public const string WaitIndicator = "_16X16.WaitIndicator.gif";
            public const string Countries = "_16X16.Countries.gif";
            public const string ExpandAll = "_16X16.Expand.gif";
            public const string CollapseAll = "_16X16.Collapse.gif";
            public const string UnSort = "_16X16.UnSort.gif";
            public const string ViewAll = "_16X16.ViewAll.gif";
            public const string ViewModules = "_16X16.ViewModules.gif";
            public const string Grid = "_16X16.Grid.gif";
            public const string List = "_16X16.List.gif";
            public const string Filter = "_16X16.Filter.gif";
            public const string Help = "_16X16.Help.gif";
            public const string CaseNotesView = "_16X16.Notes2.gif";
            public const string CaseNotesNew = "_16X16.Notes1.gif";
            public const string Pdf = "_16X16.PDF.gif";
            public const string ImageType = "_16X16.ImageTypeIcon.gif";
            public const string HistoryImage = "_16X16.History.gif";

            public const string UploadIcon = "_16X16.upload_icon.png";
            /// <summary>
            /// Generic Icon
            /// Paremeter: {0} = Icon Name
            /// </summary>
            public const string GenericIcon = "_16X16.{0}.gif";
            public const string Signature = "_16X16.Sign1.png";
            public const string RecentSearch = "_16X16.RecentSearch.png";
        }

        public static class MenuItems
        {
            public const string Divider = "mnuDivider";
            public const string More = "mnuMore";
            public const string ThreeDots = "mnuThreeDots";

            public const string File = "mnuFile";
            public const string Edit = "mnuEdit";
            public const string View = "mnuView";
            public const string Tools = "mnuTools";
            public const string Search = "mnuSearch";
            public const string Reports = "mnuReports";
            public const string Refresh = "mnuRefresh";
            public const string Help = "mnuHelp";

            public const string New = "mnuFileNew";
            public const string NewItem = "mnuNewItem";
            public const string NewItemsFrom = "mnuNewItemsFrom";

            public const string Open = "mnuFileOpen";
            public const string Save = "mnuFileSave";
            public const string Close = "mnuFileClose";
            public const string CloseAll = "mnuFileCloseAll";

            public const string Find = "mnuEditFind";
            public const string GotoPage = "mnuEditGotoPage";
            public const string Delete = "mnuEditDelete";
            public const string Rename = "mnuEditRename";
            public const string EditValues = "mnuEditEditValues";
            public const string SelectAll = "mnuEditSelectAll";

            public const string Pages = "mnuPages";
            public const string FirstPage = "mnuFirstPage";
            public const string PreviousPage = "mnuPreviousPage";
            public const string NextPage = "mnuNextPage";
            public const string LastPage = "mnuLastPage";
            public const string FitWidth = "mnuFitWidth";
            public const string FitWindow = "mnuFitWindow";

            public const string Export = "mnuExport";

            public const string CaptainHelp = "mnuCaptainHelp";
            public const string Tutorials = "mnuTutorials";
            public const string AboutCaptain = "mnuAboutCaptain";

            public const string EditUnSelectAll = "mnuEditUnSelectAll";
            public const string ViewFilter = "mnuViewFilter";
            public const string FieldChooser = "mnuFieldChooser";
            public const string Content = "mnuContent";

            public const string AddHierarchie = "mnuAddHierarchy";
            public const string EditHierarchie = "mnuEditHierarchy";
            public const string DeleteHierarchie = "mnuDeleteHierarchy";

            public const string AddRole = "mnuAddRole";
            public const string EditRole = "mnuEditRole";
            public const string DeleteRole = "mnuDeleteRole";

            public const string ZoomIn = "mnuZoomIn";
            public const string ZoomOut = "mnuZoomOut";
            public const string PDF = "mnuPDF";
            public const string ExpandAll = "mnuExpandAll";
            public const string CollapseAll = "mnuCollapseAll";
            public const string FullScreen = "mnuFullScreen";
            public const string PDFProperties = "mnuPDFProperties";
            public const string FontInformation = "mnuFontInformation";

            public const string CustomizeColumns = "mnuCustomize";
            public const string OpenInNewTab = "mnuOpenInNewTab";
            public const string AdvancedSearch = "mnuAdvancedSearch";
        }

        public static class MenuAction
        {
            public const string NoAction = "";
            public const string More = "More";
            public const string Reports = "Reports";
            public const string ChangeColor = "ChangeColor";

            public const string New = "New";
            public const string Open = "Open";
            public const string Save = "Save";
            public const string Close = "Close";
            public const string CloseAll = "Close All";
            public const string Download = "Download";
            public const string Lock = "Lock";
            public const string Unlock = "Unlock";
            public const string Print = "Print";
            public const string Exit = "Exit";
            public const string SendToExport = "SendToExport";

            public const string Find = "Find";
            public const string GotoPage = "GotoPage";
            public const string Delete = "Delete";
            public const string Rename = "Rename";
            public const string EditValues = "EditValues";
            public const string SelectAll = "SelectAll";

            public const string FirstPage = "FirstPage";
            public const string PreviousPage = "PreviousPage";
            public const string NextPage = "NextPage";
            public const string LastPage = "LastPage";
            public const string FitWidth = "FitWidth";
            public const string FitWindow = "FitWindow";
            public const string SinglePage = "SinglePage";
            public const string SingleContinuousPage = "SingleContinuousPage";
            public const string MultiplePages = "MultiplePages";
            public const string MultipleContinuousPages = "MultipleContinuousPages";

            public const string Options = "Options";
            public const string Export = "Export";

            public const string Search = "Search";
            public const string Refresh = "Refresh";

            public const string CaptainHelp = "CaptainHelp";
            public const string Tutorials = "Tutorials";
            public const string LicenseDetails = "LicenseDetails";
            public const string AboutCaptain = "AboutCaptain";

            public const string Edit = "Edit";

            public const string UpdateTeam = "UpdateTeam";
            public const string DeleteRole = "DeleteRole";
            public const string AddRole = "AddRole";
            public const string EditRole = "EditRole";

            public const string UpdateHierarchie = "UpdateTeam";
            public const string DeleteHierarchie = "DeleteRole";
            public const string AddHierarchie = "AddRole";
            public const string EditHierarchie = "EditRole";

            public const string RefreshTab = "RefreshTab";

            public const string ZoomIn = "ZoomIn";
            public const string ZoomOut = "ZoomOut";
            public const string PDF = "PDF";
            public const string ExpandAll = "ExpandAll";
            public const string CollapseAll = "CollapseAll";
            public const string FullScreen = "FullScreen";
            public const string PDFProperties = "PDFProperties";

            public const string HandTool = "HandTool";
        }

        public static class ToolTips
        {
            public const string Save = "btnSave";
            public const string CancelChanges = "btnCancelChanges";
            public const string Refresh = "btnRefresh";
            public const string FieldChooser = "btnFieldChooser";
            public const string ExpandAll = "btnExpandAll";
            public const string CollapseAll = "btnCollapseAll";
            public const string UnSort = "btnUnSort";
            public const string Open = "btnOpen";
            public const string Close = "btnClose";
            public const string Print = "btnPrint";
            public const string Find = "btnFind";
            public const string ViewAll = "btnViewAll";
            public const string ViewModules = "btnViewModules";
            public const string Search = "btnSearch";
            public const string List = "btnList";
            public const string Grid = "btnGrid";
            public const string Delete = "btnDelete";
            public const string UnFilter = "btnUnFilter";
        }

        public static class ToolbarActions
        {
            public const string New = "New";
            public const string Save = "Save";
            public const string CancelChanges = "CancelChanges";
            public const string Refresh = "Refresh";
            public const string FieldChooser = "FieldChooser";
            public const string ExpandAll = "ExpandAll";
            public const string CollapseAll = "CollapseAll";
            public const string UnSort = "Sort";
            public const string Open = "Open";
            public const string Close = "Close";
            public const string Print = "Print";
            public const string Edit = "Edit";
            public const string Find = "Find";
            public const string ViewAll = "ViewAll";
            public const string Search = "Search";
            public const string List = "List";
            public const string Grid = "Grid";
            public const string Delete = "Delete";
            public const string UnFilter = "UnFilter";
            public const string First = "First";
            public const string Previous = "Previous";
            public const string Next = "Next";
            public const string Last = "Last";
            public const string Help = "Help";
            public const string CaseNotes = "CaseNotes";
            public const string ImageTypes = "ImageTypes";
            public const string History = "History";
            public const string PDF = "PDF";
            public const string Excel = "Excel";
            public const string Signature = "Signature";
            public const string RecentSearch = "RecentSearch";
            public const string Upload = "Upload";

        }

        public static class YesNoVariants
        {
            public const string Y = "Y";
            public const string Yes = "Yes";
            public const string N = "N";
            public const string No = "No";
            public const string True = "True";
            public const string False = "False";
            public const string T = "T";
            public const string F = "F";
        }

        public static class SessionVariables
        {
            public const string HTTPReferer = "HTTP_REFERER";
            public const string ApplicationURL = "ApplicationURL";
            public const string IsInitialLoad = "IsInitialLoad";
            public const string IsSelectUserFlag = "IsSelectUserFlag";
            public const string UserPreference = "UserPreference";
            public const string UserID = "UserID";
            public const string FullName = "FullName";
            public const string LoginSCN = "LoginSCN";
            public const string UserProfile = "UserProfile";
            public const string UserName = "UserName";
            public const string Password = "Password";
            public const string Language = "Language";
            public const string ServerName = "ServerName";
            public const string ChildParentName = "ChildParentName";
            public const string SelectedNodeTag = "SelectedNodeTag";
            public const string SelectedNode = "SelectedNode";
            public const string FormBaseUserControl = "FormBaseUserControl";
            public const string FormName = "FormName";
            public const string FormTitle = "FormTitle";
            public const string FormTagClass = "FormTagClass";
            public const string ShowStartPage = "ShowStartPage";
            public const string CaptainLogoBanner = "CaptainLogoBanner";
            public const string SelectedNodeFullPath = "SelectedNodeFullPath";
            public const string ScreenWidth = "ScreenWidth";
            public const string ScreenHeight = "ScreenHeight";
            public const string AutoRefreshTime = "AutoRefreshTime";
            public const string IsSessionAlive = "IsSessionAlive";
            public const string LostLogin_Status = "successful";
            public const string LostLogin_Date = "Lost Login Date";

        }

        public static class Messages
        {
            /// <summary>
            /// Used to display a message when a name already exists.
            /// </summary>
            public const string NameAlreadyExists = "NameAlreadyExists";

            /// <summary>
            /// Used to display a message when a user is not found.
            /// </summary>
            public const string TheUserWasNotFound = "TheUserWasNotFound";

            /// <summary>
            /// Used to display a message when a duplicate entry is chosen.
            /// </summary>
            public const string TheNameYouEnteredAreadyExists = "TheNameYouEnteredAreadyExists";

            /// <summary>
            /// Used to display a message when a specific amount of items can be chosen.
            /// </summary>
            public const string OnlyBlankItemsAreAllowedToBeSelected = "OnlyBlankItemsAreAllowedToBeSelected";

            /// <summary>
            /// Used to display a message when a value already exists.
            /// </summary>
            public const string BlankAlreadyExists = "BlankAlreadyExists";

            /// <summary>
            /// Used to display a generic error message to the user.
            /// </summary>
            public const string ExceptionMessage = "ExceptionMessage";

            /// <summary>
            /// Used to display a generic message to the user when deleting.
            /// </summary>
            public const string AreYouSureYouWantToDelete = "AreYouSureYouWantToDelete";

            /// <summary>
            /// Used to display a message when the role is not available on the issue typ.
            /// </summary>
            public const string BlankIsRequired = "BlankIsRequired";

            /// <summary>
            /// Used to display a message when the role is not available on the issue typ.
            /// </summary>
            public const string NumericOnly = "NumericOnly";

            /// <summary>
            /// Used to display a message when an export is successfully initialized.
            /// </summary>
            public const string ExportCompleted = "ExportCompleted";

            /// <summary>
            /// Used to display a message when the username or password is not valid.
            /// </summary>
            public const string TheUserNameOrPasswordSuppliedIsNotValid = "TheUserNameOrPasswordSuppliedIsNotValid";

            /// <summary>
            /// Ask user to give his/her credentials
            /// </summary>
            public const string PleaseEnterUserName = "PleaseEnterUserName";

            /// <summary>
            /// Ask user to give his/her credentials
            /// </summary>
            public const string PleaseEnterPassword = "PleaseEnterPassword";

            /// <summary>
            /// Creating Concept Request(s)
            /// </summary>
            public const string UserCreatedSuccesssfully = "UserCreatedSuccesssfully";

            /// <summary>
            /// Please select at least one item.
            /// </summary>
            public const string PleaseSelectAtLeastOneItem = "Please select at least one item.";

            /// <summary>
            /// Copied one already Exists
            /// </summary>
            public const string AlreadyExists = "AlreadyExists";

            /// <summary>
            /// Please Enter two characters
            /// </summary>
            public const string PleaseEnterTwoCharacters = "Please enter state two characters";
            /// <summary>
            /// Please Enter decimals
            /// </summary>
            public const string PleaseEnterDecimals = "Please enter decimals";
            /// <summary>
            /// Please Enter decimals
            /// </summary>
            public const string PleaseEnterDecimalsRange = "Please Range between 0 to 9999999.99 ";

            public const string PleaseEnterDecimals5Range = "Please Range between 0 to 99999.99 ";

            public const string PleaseEnterDecimals6Range = "Please Range between 0 to 999999.99 ";

            public const string PleaseEnterDecimals3Range = "Please Range between 0 to 999.99 ";

            public const string PleaseEnterDecimal11Range = "Please Range between 0 to 99999999999.99";
            /// <summary>
            /// Please Enter decimals
            /// </summary>
            public const string PleaseEnterNumbers = "Please enter numbers only";

            /// <summary>
            /// Please Enter decimals
            /// </summary>
            public const string PleaseEnterEmail = "Please enter Email format only";

            /// <summary>
            /// Please enter MM/dd/yyyy date format
            /// </summary>
            public const string PleaseEntermmddyyyyDateFormat = "Please enter MM/DD/YYYY date format";

            /// <summary>
            /// Please enter MM/dd/yy date format
            /// </summary>
            public const string PleaseEntermmddyyDateFormat = "Please enter (MM/DD/YY) or (MM/DD/YYYY) date format";

            /// <summary>
            /// Please enter MM/dd/yy date format
            /// </summary>
            public const string PleaseEnterHHMMTimeFormat = "Please enter (HH:MM) or (00:00 to 24:00 ) format";

            /// <summary>
            /// Required hierarchy
            /// </summary>
            public const string RequiredHierarchy = "Hierachy is required";

            /// <summary>
            /// Required hierarchy
            /// </summary>
            public const string RequiredData = "Field is required";

            /// <summary>
            /// Required proper order
            /// </summary>
            public const string RequiredbelowFields = "% Should be proper order";
            /// <summary>
            /// Complete Family Size column first
            /// </summary>
            public const string RequiredFamSizecolumnfirst = "Complete Family Size column first";
            /// <summary>
            /// Required dateFields
            /// </summary>
            public const string RequireddateFields = "Date is mandatory";
            /// <summary>
            /// Required proper order
            /// </summary>
            public const string RequiredbelowFields1 = "Should be proper order";
            /// <summary>
            /// 
            public const string RequiredbelowMaspoverty = "This row cannot be inserted as there is missing data in one of the cells";
            ///
            /// 01/01/1800 below date not except
            /// </summary>
            public const string Belowdatenotexcept = "01/01/1800 below date not except";

            public const string Greaterthanzzero = "Please enter greater than zero";

            public const string Recordsornotfound = "Record does not exist";

            public const string RecordsorExists = "Record already exist";

            public const string Datealreadyexists = "Date already exist";

            public const string Applicantdoesntexist = "No applicant/client data in your work area.\n Screens can only be launched after the applicant/client is referenced in this screen area.\n Search for the applicant/client you want to work with and then launch your screen";//"No applications exist in this hierarchy.";//"Applicants doesn't exist in this hierarchy.\n Please add New Applicant.";

            public const string AplicantSelectionMsg = "Select an applicant from mainmenu";

        }

        public static class TabPages
        {
            public const string InformationPage = "Information";
        }

        public static class DataTypes
        {
            public const string Boolean = "boolean";
            public const string String = "string";
            public const string Percent = "percent";
            public const string Date = "date";
            public const string Time = "time";
            public const string DateTime = "datetime";
            public const string Number = "number";
            public const string ObjectID = "objectid";
            public const string ObjectProperty = "objectproperty";
            public const string ProjectUserID = "projectuserid";
            public const string CompanyUserID = "companyuserid";
            public const string SystemUserID = "systemuserid";
            public const string ProjectRoleUser = "projectroleuser";
            public const string Sequence = "sequence";
            public const string CaptainSequence = "CaptainSequence";
            public const string CaptainLink = "Captainlink";
            public const string Formula = "formula";
            public const string FixedGridColumn = "fixedgridcolumn";
            public const string Bytes = "bytes";
            public const string Table = "table";
            public const string ClobType = "clobtype";
            public const string UserType = "usertype";
        }

        public static class StaticVars
        {
            public const string AlphaString = @"^[A-Z]+$";
            public const string AlphaNumericString = @"^[a-zA-Z0-9 ]+$";
            public const string NumericString = @"^[+]?\d*$";
            public const string PNNumericString = @"^-?\d*\.{0,1}\d+$";
            public const string Numericonly = @"[^0-9]";
            public const string DecimalString = @"^([0-9]*)(\.[0-9]{3})?$";
            public const string TwoDecimalString = @"^\d{1,10}(\.\d{1,2})?$";
            public const string TwoDecimalRange11String = @"^\d{1,11}(\.\d{1,2})?$";
            public const string TwoDecimalRange7String = @"^\d{1,7}(\.\d{1,2})?$";
            public const string TwoDecimalRange5String = @"^\d{1,5}(\.\d{1,2})?$";
            public const string TwoDecimalRange3String = @"^\d{1,3}(\.\d{1,2})?$";
            public const string TwoDecimalRange6String = @"^\d{1,6}(\.\d{1,2})?$";
            public const string TwoDecimalRange8String = @"^\d{1,8}(\.\d{1,2})?$";
            public const string FiveDecimalRange6String = @"^\d{1,6}(\.\d{1,5})?$";
            public const string EmailValidatingString = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            //"^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$";//
            //  "^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$";
            // @" ^ (([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";//@"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
            public const string UrlValidatingString = @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$";
            public const string AlphaNumericStringExtended = @"^[a-zA-Z0-9 ._-:\?]+$";
            public const string DateFormatMMDDYYYY = @"((^(10|12|0?[13578])([/])(3[01]|[12][0-9]|0?[1-9])([/])((1[8-9]\d{2})|([2-9]\d{3}))$)|(^(11|0?[469])([/])(30|[12][0-9]|0?[1-9])([/])((1[8-9]\d{2})|([2-9]\d{3}))$)|(^(0?2)([/])(2[0-8]|1[0-9]|0?[1-9])([/])((1[8-9]\d{2})|([2-9]\d{3}))$)|(^(0?2)([/])(29)([/])([2468][048]00)$)|(^(0?2)([/])(29)([/])([3579][26]00)$)|(^(0?2)([/])(29)([/])([1][89][0][48])$)|(^(0?2)([/])(29)([/])([2-9][0-9][0][48])$)|(^(0?2)([/])(29)([/])([1][89][2468][048])$)|(^(0?2)([/])(29)([/])([2-9][0-9][2468][048])$)|(^(0?2)([/])(29)([/])([1][89][13579][26])$)|(^(0?2)([/])(29)([/])([2-9][0-9][13579][26])$))";//@"^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$";
            public const string DateFormatMMDDYY = @"^((0?[13578]|10|12)(-|\/)(([1-9])|(0[1-9])|([12])([0-9]?)|(3[01]?))(-|\/)((19)([2-9])(\d{1})|(20)([01])(\d{1})|([8901])(\d{1}))|(0?[2469]|11)(-|\/)(([1-9])|(0[1-9])|([12])([0-9]?)|(3[0]?))(-|\/)((19)([2-9])(\d{1})|(20)([01])(\d{1})|([8901])(\d{1})))$";//@"^(0[1-9]|[12]\d|3[01])/(0[1-9]|1[0-2])/((?:19|20)\d{2})$";
                                                                                                                                                                                                                                                                                                                 // public const string DateFormatMMDDYY = @"^((0?[13578]|10|12)(-|\/)(([1-9])|(0[1-9])|([12])([0-9]?)|(3[01]?))(-|\/)((19)([2-9])(\d{1})|(20)([01])(\d{1})|([8901])(\d{1}))|(0?[2469]|11)(-|\/)(([1-9])|(0[1-9])|([12])([0-9]?)|(3[0]?))(-|\/)((19)([2-9])(\d{1})|(20)([01])(\d{1})|([8901])(\d{1})))$";
                                                                                                                                                                                                                                                                                                                 //public const string DateFormatMMDDYY = @"^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d+$/";
            public const string PasswordRegulation = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&\.])[A-Za-z\d$@$!%*#?&\.]{8,}$";
            public const string TimeFormatHHMM = @"^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$";
            //  @"[a-zA-Z0-9\@\#\$\%\&\*\(\)\-\_\+\]\[\'\;\:\?\.\,\!]+$";




        }

        public static class AgyTab
        {
            public const string IMAGENAMECONVENTION = "00026";
            public const string IMAGETYPES = "00027";
            public const string ETHNIC = "00001";
            public const string DISABLED = "00002";
            public const string RACE = "00003";
            public const string INCOMETYPES = "00004";
            public const string EDUCATIONCODES = "00007";
            public const string FAMILYTYPE = "00008";
            public const string HOUSINGTYPES = "00009";
            public const string DRIVERLICENSE = "00015";
            public const string RELIABLETRANSPORTATION = "00016";
            public const string TRANSPRTSERVS = "00017";
            public const string PREGNANT = "00018";
            public const string GENDER = "00019";
            public const string FOODSTAMPS = "00020";
            public const string VETERAN = "00021";
            public const string FARMER = "00022";
            public const string HEALTHINSURANCE = "00025";
            public const string WIC = "00030";
            public const string HSREASONWITHDRWL = "00092";
            public const string HSRANKVALS = "00096";
            public const string DISABILITYTASKS = "00098";
            public const string CATEGRCALELIGIBLTY = "00099";
            public const string ABSENTEEISM = "00107";
            public const string HSOPTNRANKVALS = "00118";
            public const string DELAWAREFUELBENFTS = "00141";
            public const string DELAWAREBENFTYPES = "00143";
            public const string ETHNICODES = "00352";
            public const string LANGUAGECODES = "00353";
            public const string RESIDENTCODES = "00354";
            public const string FUNDINGSOURCES = "03320";
            public const string COMPONENTS = "00356";
            public const string HSCALENDARDAYSTATS = "00357";
            public const string HSINCOMPLETEAPLICTNS = "00358";
            public const string STATEMEDICALCATEGRS = "00359";
            public const string ARINVOICESOURCES = "00430";
            public const string ARCUSTOMERTYPES = "00432";
            public const string ARTERMS = "00433";
            public const string FUNDCODS = "00501";
            public const string CASETYPES = "00523";
            public const string ALERTCODES = "00524";
            public const string COUNTY = "00525";
            public const string BESTWAYTOCONTACT = "00526";
            public const string HOWDIDYOUHEARABOUTTHEPROGRAM = "00527";
            public const string POSITIONCODS = "00550";
            public const string FUELOPTIONS = "00555";
            public const string NXTHOUSINGJOBNUM = "01009";
            public const string HOUSINGJOBSTATS = "01010";
            public const string WAPJOBSTATS = "01011";
            public const string MISCDATACATEGRS = "03002";
            public const string SCHOOLDISTRICTS = "03003";
            public const string JOBTITLE = "03004";
            public const string JOBCATEGORY = "03005";
            public const string MARITALSTATUS = "03006";
            public const string REFERRALSTATUS = "03007";
            public const string LEGALISSUES = "03240";
            public const string RELATIONSHIP = "03259";
            public const string ACTIVITYCATEGORY = "03310";
            public const string CASEMNGMTFUNDSRC = "03320";
            public const string GOALS = "03330";
            public const string RESULTS = "03335";
            public const string CONTACTHOWHERE = "03340";
            public const string UOMTABLE = "03341";
            public const string RISKCATEGRY = "03342";
            public const string REFERLDENIALREASN = "03600";
            public const string CITYTOWNTABLE = "08001";
            public const string DWELLINGTYPE = "08002";
            public const string METHODOFPAYINGFORHEAT = "08003";
            public const string CELLPHONEPROVIDR = "08005";
            public const string CTFUEL = "08141";
            public const string RIFUELBENEFITS = "08241";
            public const string RIFUELBENEFITSB2 = "08242";
            public const string CASHAWARDSFILEFORMATS = "08550";
            public const string FUELELECTRICBILLING = "08556";
            public const string REASONFORDENIAL = "09001";
            public const string STOPPAYCODS = "09002";
            public const string EMERGENCYCODES = "09003";
            public const string TYPEUSESECONDHOME = "09004";
            public const string REASNFORPAYMNTADJSTENTS = "09005";
            public const string MISCLLANEOUS = "55555";
            public const string HEATSOURCE = "08004";
            public const string Reasonfor = "00560";
            public const string Category = "00551";
            public const string Enroll_Reasons = "03555";
            public const string Subsidized_Housing_Type = "08010";
            public const string DONORTYPE = "05555";
            public const string DONOR_SERVICE_TYPE = "05558";
            public const string DecisionType = "05503";
            public const string Reasoncodes = "05518";
            public const string STD_ALLOWANCE = "09888";
            public const string RejectCodes = "00516";
            public const string VendorType = "08006";

            public const string MilitaryStatus = "00035";
            public const string WorkStatus = "00036";
            public const string NonCashBenefits = "00037";
            public const string HealthCodes = "00038";
            public const string DisconnectedYouth = "00039";

            public const string SALStatus = "03012";
            public const string SALLocation = "03014";
            public const string SALRecipient = "03016";
            public const string SALAttendence = "03018";



            public const string IMAGETYPESMsg = "Inactive IMAGETYPES";
            public const string ETHNICMsg = "Inactive ETHNIC";
            public const string DISABLEDMsg = "Inactive DISABLED";
            public const string RACEMsg = "Inactive RACE";
            public const string INCOMETYPESMsg = "Inactive INCOMETYPES";
            public const string EDUCATIONCODESMsg = "Inactive EDUCATIONCODES";
            public const string FAMILYTYPEMsg = "Inactive FAMILYTYPE";
            public const string HOUSINGTYPESMsg = "Inactive HOUSINGTYPES";
            public const string DRIVERLICENSEMsg = "Inactive DRIVERLICENSE";
            public const string RELIABLETRANSPORTATIONMsg = "Inactive RELIABLETRANSPORTATION";
            public const string TRANSPRTSERVSMsg = "Inactive TRANSPRTSERVS";
            public const string PREGNANTMsg = "Inactive PREGNANT";
            public const string GENDERMsg = "Inactive GENDER";
            public const string FOODSTAMPSMsg = "Inactive FOODSTAMPS";
            public const string VETERANMsg = "Inactive VETERAN";
            public const string FARMERMsg = "Inactive FARMER";
            public const string HEALTHINSURANCEMsg = "Inactive HEALTHINSURANCE";
            public const string WICMsg = "Inactive WIC";
            public const string HSREASONWITHDRWLMsg = "Inactive HSREASONWITHDRWL";
            public const string HSRANKVALSMsg = "Inactive HSRANKVALS";
            public const string DISABILITYTASKSMsg = "Inactive DISABILITYTASKS";
            public const string CATEGRCALELIGIBLTYMsg = "Inactive CATEGRCALELIGIBLTY";
            public const string ABSENTEEISMMsg = "Inactive ABSENTEEISM";
            public const string HSOPTNRANKVALSMsg = "Inactive HSOPTNRANKVALS";
            public const string DELAWAREFUELBENFTSMsg = "Inactive DELAWAREFUELBENFTS";
            public const string DELAWAREBENFTYPESMsg = "Inactive DELAWAREBENFTYPES";
            public const string ETHNICODESMsg = "Inactive ETHNICODES";
            public const string LANGUAGECODESMsg = "Inactive LANGUAGECODES";
            public const string RESIDENTCODESMsg = "Inactive RESIDENTCODES";
            public const string NONCASHBENIFITSCODESMsg = "Inactive Non-Cash Benefits";
            public const string FUNDINGSOURCESMsg = "Inactive FUNDINGSOURCES";
            public const string COMPONENTSMsg = "Inactive COMPONENTS";
            public const string HSCALENDARDAYSTATSMsg = "Inactive HSCALENDARDAYSTATS";
            public const string HSINCOMPLETEAPLICTNSMsg = "Inactive HSINCOMPLETEAPLICTNS";
            public const string STATEMEDICALCATEGRSMsg = "Inactive STATEMEDICALCATEGRS";
            public const string ARINVOICESOURCESMsg = "Inactive ARINVOICESOURCES";
            public const string ARCUSTOMERTYPESMsg = "Inactive ARCUSTOMERTYPES";
            public const string ARTERMSMsg = "Inactive ARTERMS";
            public const string FUNDCODSMsg = "Inactive FUNDCODS";
            public const string CASETYPESMsg = "Inactive CASETYPES";
            public const string ALERTCODESMsg = "Inactive ALERTCODES";
            public const string COUNTYMsg = "Inactive COUNTY";
            public const string BESTWAYTOCONTACTMsg = "Inactive BESTWAYTOCONTACT";
            public const string HOWDIDYOUHEARABOUTTHEPROGRAMMsg = "Inactive HOWDIDYOUHEARABOUTTHEPROGRAM";
            public const string POSITIONCODSMsg = "Inactive POSITIONCODS";
            public const string FUELOPTIONSMsg = "Inactive FUELOPTIONS";
            public const string NXTHOUSINGJOBNUMMsg = "Inactive NXTHOUSINGJOBNUM";
            public const string HOUSINGJOBSTATSMsg = "Inactive HOUSINGJOBSTATS";
            public const string WAPJOBSTATSMsg = "Inactive WAPJOBSTATS";
            public const string MISCDATACATEGRSMsg = "Inactive MISCDATACATEGRS";
            public const string SCHOOLDISTRICTSMsg = "Inactive SCHOOLDISTRICTS";
            public const string JOBTITLEMsg = "Inactive JOBTITLE";
            public const string JOBCATEGORYMsg = "Inactive JOBCATEGORY";
            public const string MARITALSTATUSMsg = "Inactive MARITALSTATUS";
            public const string REFERRALSTATUSMsg = "Inactive REFERRALSTATUS";
            public const string LEGALISSUESMsg = "Inactive LEGALISSUES";
            public const string RELATIONSHIPMsg = "Inactive RELATIONSHIP";
            public const string ACTIVITYCATEGORYMsg = "Inactive ACTIVITYCATEGORY";
            public const string CASEMNGMTFUNDSRCMsg = "Inactive CASEMNGMTFUNDSRC";
            public const string GOALSMsg = "Inactive GOALS";
            public const string RESULTSMsg = "Inactive RESULTS";
            public const string CONTACTHOWHEREMsg = "Inactive CONTACTHOWHERE";
            public const string UOMTABLEMsg = "Inactive UOMTABLE";
            public const string RISKCATEGRYMsg = "Inactive RISKCATEGRY";
            public const string REFERLDENIALREASNMsg = "Inactive REFERLDENIALREASN";
            public const string CITYTOWNTABLEMsg = "Inactive CITYTOWNTABLE";
            public const string DWELLINGTYPEMsg = "Inactive DWELLINGTYPE";
            public const string METHODOFPAYINGFORHEATMsg = "Inactive METHODOFPAYINGFORHEAT";
            public const string CELLPHONEPROVIDRMsg = "Inactive CELLPHONEPROVIDR";
            public const string CTFUELMsg = "Inactive CTFUEL";
            public const string RIFUELBENEFITSMsg = "Inactive RIFUELBENEFITS";
            public const string RIFUELBENEFITSB2Msg = "Inactive RIFUELBENEFITSB2";
            public const string CASHAWARDSFILEFORMATSMsg = "Inactive CASHAWARDSFILEFORMATS";
            public const string FUELELECTRICBILLINGMsg = "Inactive FUELELECTRICBILLING";
            public const string REASONFORDENIALMsg = "Inactive REASONFORDENIAL";
            public const string STOPPAYCODSMsg = "Inactive STOPPAYCODS";
            public const string EMERGENCYCODESMsg = "Inactive EMERGENCYCODE";
            public const string TYPEUSESECONDHOMEMsg = "Inactive TYPEUSESECONDHOME";
            public const string REASNFORPAYMNTADJSTENTSMsg = "Inactive REASNFORPAYMNTADJSTENTS";
            public const string MISCLLANEOUSMsg = "Inactive MISCLLANEOUS";
            public const string HEATSOURCEMsg = "Inactive HEATSOURCE";
            public const string LEGALTOWORKMSG = "Inactive Legal To Work";
            public const string SECONDLANGUAGE = "Inactive Second Language";
            public const string PRIMARYLanguage = "Inactive Primary Language";
            public const string HOUSINGSITUVATION = "Inactive Housing Situvation";
            public const string ReasonforMsg = "Inactive Reason";
            public const string PrimaryMopfhMsg = "Inactive Primary Method of Paying for Heat";
            public const string PrimarySohMsg = "Inactive Primary Source of Heat";
            public const string SubsidizedMsg = "Inactive Subsidized Housing Type";
            public const string WorkStatusRMsg = "Inactive WorkStatus";
        }

        public static class TMS00100
        {
            public const string StatusOpen = "Open";
            public const string StatusClose = "Close";
            public const string OpenValue = "1";
            public const string CloseValue = "0";
            public const string ChangeMessage = "Template is already used,You can't change";
            public const string DeleteMessage = "Template is already used,You can't delete";
            public const string DataExists = "Exists";
            public const string MessageMonth = "Template already defined for this Month";
            public const string MessageDate = "Template already defined for this Date";//"This Date already exists";
            public const string TypeMaster = "Master";
            public const string TypeMonth = "Month";
            public const string TypeDate = "Date";
            public const string MessageScheduleMonth = "Warning: There is already appointments booked for this month as per Master schedule, they may loose";
            public const string MessageScheduleDate = "Warning: There is already appointments booked for this date, they may loose";
        }

        public static class LiheAllDetails
        {
            public const string LdSeq = "0";
            public const string LdType1 = "Letter Categories and Boiler plate text";
            public const string LdType2 = "Boiler plate text only";
            public const string ChangeMessage = "Template is already used,You can't change";
            public const string DeleteMessage = "Template is already used,You can't delete";
            public const string strMainType = "Main";
            public const string strSubType = "Sub";
            public const string LdType0 = "RETURN THIS FORM, YOUR APPLICATION AND ALL DOCUMENTS WITHIN 5 DAYS.";
            public const string LdCategory = "NOTICE OF INCOMPLETE APPLICATION";
            public const string DataExists = "Exists";
            public const string CategoryExists = "Category already exists";
            public const string RecordsNotfound = "No Categories found";
            public const string View = "View";
            public const string Datealreadyexist = "Date already exist";
            public const string Atleastonecategoryrequired = "At least one category required";
            public const string DateShouldnotbeLessthanLetterDate = "Date should not be less than letter date";
        }

        public static class CASE2001
        {
            public const string AlertCodes = "S00010";
            public const string AssignedWorker = "S00030";
            public const string CaseType = "S00040";
            public const string Active = "S00120";
            public const string ExcludeMember = "S00460";
            public const string ExcludeMember2 = "S00461";
            public const string Secret = "S00130";
            public const string HN = "S00050";
            public const string Direction = "S00060";
            public const string Suffix = "S00080";
            public const string Apartment = "S00090";
            public const string Floor = "S00100";
            public const string City = "S00150";
            public const string County = "S00170";
            public const string Street = "S00070";
            public const string ZipCode = "S00140";
            public const string Township = "S00160";
            public const string Precinct = "S00110";
            public const string State = "S00180";
            public const string NoOfYearsAtThisAddress = "S00190";
            public const string InitialDate = "S00200";
            public const string IntakeDate = "SA0210";
            public const string CaseReviewDate = "S00220";
            public const string HousingSituation = "S00230";
            public const string PrimaryLanguage = "S00240";
            public const string SecondaryLanguage = "S00250";
            public const string FamilyType = "S00260";
            public const string WaitingList = "S00270";
            public const string Site = "S00280";
            public const string Phone = "S00290";
            public const string Message = "S00300";
            public const string Cell = "S00310";
            public const string Fax = "S00320";
            public const string TTY = "S00330";
            public const string Email = "S00340";
            public const string WhatIsTheBestWayToContact = "S00350";
            public const string HowDidYouFindOutAboutUs = "S00360";
            public const string WhatServicesAreYouInquiringAbout = "S00790";
            public const string VerifiedthatallHouseholdExpensesentered = "S00670";
            //public const string SSN = "SA0370";
            public const string SSN = "S00370";
            public const string BIC = "S00380";
            public const string FirstName = "SA0400";
            public const string MI = "S00410";
            public const string LastName = "SA0420";
            public const string Alias = "S00430";
            public const string DateOfBirth = "SA0440";
            public const string Relation = "S00500";
            public const string Ethnicity = "S00510";
            public const string Race = "S00520";
            public const string Education = "S00530";
            public const string School = "S00540";
            public const string SnpActive = "S00450";
            public const string SSNReason = "S00390";
            public const string Gender = "S00470";
            public const string AreYouPregnant = "S00480";
            public const string MaritalStatus = "S00490";
            public const string HealthInsurance = "S00550";
            public const string VeteranCode = "S00560";
            public const string FoodStamps = "S00570";
            public const string WIC = "S00600";
            public const string Farmer = "S00590";
            public const string Disabled = "S00580";
            public const string DriversLicense = "S00620";
            public const string ReliableTransport = "S00610";
            public const string Resident = "S00630";
            public const string AlienRegNo = "S00640";
            public const string LegalToWork = "S00650";
            public const string ExpirationDate = "S00660";
            public const string InCaseOfFirst = "S00800";
            public const string InCaseOfLast = "S00810";
            public const string MailHouseNo = "S00820";
            public const string MailStreet = "S00830";
            public const string MailSf = "S00840";
            public const string MailApt = "S00850";
            public const string MailFlr = "S00860";
            public const string MailCity = "S00870";
            public const string MailState = "S00880";
            public const string MailZip = "S00890";
            public const string MailCounty = "S00900";
            public const string MailPhone = "S00910";
            public const string RecentMortage = "S00680";
            public const string WaterSewer = "S00690";
            public const string Electric = "S00700";
            public const string Heating = "S00710";
            public const string MiscExpenses = "S00711";
            public const string CreditCardDebit = "S00720";
            public const string LoansDebt = "S00730";
            public const string MedicalDebit = "S00740";
            public const string OtherDebit = "S00750";
            public const string MiscDebit = "S00760";
            public const string MonthlyProgramIncome = "S00770";
            public const string TotalLivingExpense = "S00780";
            public const string ManualType = "Manual";
            public const string DragType = "Drag";
            public const string EEmployed = "S00920";
            public const string ELastWorkDate = "S00930";
            public const string EWorkLimit = "S00940";
            public const string EExplainworkLimit = "S00950";
            public const string ENumberofcjobs = "S00960";
            public const string ENumberofLvjobs = "S00970";
            public const string EFullTimeHours = "S00980";
            public const string EPartTimeHours = "S00990";
            public const string ESeasonalEmploy = "S01000";
            public const string EShiftWorked1st = "S01010";
            public const string EShiftWorked2nd = "S01020";
            public const string EShiftWorked3rd = "S01030";
            public const string EShiftWorkedRotating = "S01040";
            public const string EEmployerName = "S01050";
            public const string EEmployerStreet = "S01060";
            public const string EEmployerCity = "S01070";
            public const string EPhone = "S01080";
            public const string EExt = "S01090";
            public const string EJobTitle = "S01100";
            public const string EJobCategory = "S01110";
            public const string EHourlyWage = "S01120";
            public const string EPayFreQuency = "S01130";
            public const string EHireDate = "S01140";

            public const string Dwelling = "S00781";
            public const string Primaryopfh = "S00783";
            public const string PrimarySourceHeat = "S00782";
            public const string PhysicalAssets = "S00765";
            public const string LiquidAssets = "S00766";
            public const string OtherAssets = "S00767";
            public const string MiscAssets = "S00768";

            public const string LLFirst = "S01170";
            public const string LLLast = "S01180";
            public const string LLHouseNo = "S01190";
            public const string LLStreet = "S01200";
            public const string LLSf = "S01210";
            public const string LLApt = "S01220";
            public const string LLFlr = "S01230";
            public const string LLCity = "S01240";
            public const string LLState = "S01250";
            public const string LLZip = "S01260";
            public const string LLCounty = "S01270";
            public const string LLPhone = "S01280";

            public const string MilitaryStatus = "S01281";
            public const string WorkStatus = "S01300";
            public const string NonCashBenefits = "S01290";
            public const string Youth = "S01310";
            public const string AppSuffix = "S01320";




        }

        public static class CASE2003
        {
            public const string Verifier = "S00002";
            public const string FundingSource = "S00003";
            public const string IncomeVerified = "S00004";
            public const string inProgram = "S00005";
            public const string Income = "S00006";
            public const string Meal = "S00007";
            public const string Categorical = "S00008";
            public const string ReverificationDate = "S00009";
            public const string VerificationDate = "SA0001";
        }

        public static class CASEINCOME
        {
            public const string IncomeType = "Income Type";
            public const string Verifier = "Verifier";
            public const string Interval = "Interval";
            public const string Value1 = "Value 1";
            public const string Value2 = "Value 2";
            public const string Value3 = "Value 3";
            public const string Value4 = "Value 4";
            public const string Value5 = "Value 5";
            public const string Date1 = "Date 1";
            public const string Date2 = "Date 2";
            public const string Date3 = "Date 3";
            public const string Date4 = "Date 4";
            public const string Date5 = "Date 5";
            public const string IncomeSource = "Income Source";
            public const string HowVerified = "How Verified";
            public const string Factor = "Factor";
            public const string Sub = "Sub";
            public const string Total = "Total";
            public const string Exclude = "Exclude";

        }

        public static class CASE0006
        {
            public const string ContactName = "S00002";
            public const string ContCaseWorker = "S00003";
            public const string ofContacts = "S00004";
            public const string DurationType = "S00005";
            public const string StartTime = "S00006";
            public const string EndTime = "S00007";
            public const string Duration = "S00008";
            public const string HowWhere = "S00009";
            public const string LanguageContactSpeaks = "S00010";
            public const string InterpreterNeed = "S00011";
            public const string ReferredFrom = "S00012";
            public const string BillTo = "S00013";
            public const string BillUnit = "S00014";
            public const string ContProgram = "S00015";
            public const string FundingSource1 = "S00016";
            public const string FundingSource2 = "S00017";
            public const string FundingSource3 = "S00018";
            public const string ActCaseWorker = "S00019";
            public const string Site = "S00020";
            public const string VendorNo = "S00021";
            public const string CheckNo = "S00022";
            public const string CheckDate = "S00023";
            public const string TobeFollowUpBy = "S00024";
            public const string FollowUPOn = "S00025";
            public const string FollowUpComplete = "S00026";
            public const string Date = "SA0027";
            public const string MsSite = "S00028";
            public const string Result = "S00029";
            public const string MsCaseWorker = "S00030";
            public const string BenefitingfromServiceActivity = "SA0031";
            public const string ContactDate = "SA0001";
            public const string ActivityDate = "SA0015";
            public const string Cost = "S00032";
            public const string Act_Acty_Program = "S00033";
            public const string MS_Acty_Program = "S00034";
            public const string Act_UOM = "S00053";
            public const string Act_Units = "S00054";
            public const string Act_Seek_Date = "S00043";
            public const string MS_Seek_Date = "S00044";

            public const string Act_Rate = "S00070";

            public const string MS_Fund1 = "S00065";
            public const string MS_Fund2 = "S00066";
            public const string MS_Fund3 = "S00067";

            public const string MS_Follow_Date = "S00070";
            public const string MS_Comp_Date = "S00072";

            public const string SMP_SP = "SA0035";
            public const string SMP_Site = "S00036";
            public const string SMP_CaseWorker = "S00037";
            public const string SMP_Act_Prog = "S00038";
            public const string SMP_Start_Date = "SA0039";
            public const string SMP_Est_Complete_Date = "S00040";
            public const string SMP_Actual_Complete_Date = "S00041";
            public const string SMP_Sel_Branches = "S00042";
        }

        public static class CASE0063
        {
            public const string FundingSource1 = "S00016";
            public const string FundingSource2 = "S00017";
            public const string FundingSource3 = "S00018";
            public const string ActCaseWorker = "S00019";
            public const string Site = "S00020";
            public const string VendorNo = "S00021";
            public const string CheckNo = "S00022";
            public const string CheckDate = "S00023";
            public const string TobeFollowUpBy = "S00024";
            public const string FollowUPOn = "S00025";
            public const string FollowUpComplete = "S00026";
            public const string ActivityDate = "SA0015";
            public const string Cost = "S00032";
            public const string Act_Acty_Program = "S00033";

            public const string Act_UOM = "S00053";
            public const string Act_Units = "S00054";
            public const string Act_Seek_Date = "S00043";
            public const string Act_Rate = "S00070";

            //public const string Cat1VendorNo = "S00101";
            //public const string Cat1FundingSource1 = "S00102";
            //public const string Cat1FundingSource2 = "S00103";
            //public const string Cat1FundingSource3 = "S00104";
            //public const string Cat1UOM = "S00105";
            //public const string Cat1Amount = "S00106";
            //public const string Cat1Check = "S00107";
            //public const string Cat1CheckDate = "S00108";

            //public const string Cat2VendorNo = "S00201";
            public const string BillingPeriod = "S00202";
            public const string Account = "S00203";
            public const string ArrearsAmount = "S00204";
            public const string AmountPaid = "S00205";
            public const string CWApproval = "S00208";
            public const string CWApprovalDate = "S00209";
            public const string SupervisorApproval = "S00206";
            public const string SupervisorApprovalDate = "S00207";
            public const string SentforPaymentbyUser = "S00210";
            public const string SentOn = "S00211";
            public const string Bundle = "S00212";
            public const string BillingType = "S00214";
            public const string Payment = "S00216";
            public const string Source = "S00218";
            //public const string CheckNo = "S00022";

            //public const string Cat3VendorNo = "S00101";
            //public const string Cat3FundingSource1 = "S00102";
            //public const string Cat3FundingSource2 = "S00103";
            //public const string Cat3FundingSource3 = "S00104";
            //public const string Cat3UOM = "S00105";
            public const string Amount = "S00055";
            //public const string Cat3Check = "S00107";
            //public const string Cat3CheckDate = "S00108";
            public const string UOM2 = "S00056";
            public const string UOM3 = "S00058";
            public const string Amount2 = "S00057";
            public const string Amount3 = "S00059";
            public const string BenefitReason = "S00219";
            public const string pdoutsideofsystem = "S00220";
        }
        public static class EligQues
        {
            public const string Age = "SNP_AGE";
            public const string Education = "SNP_EDUCATION";
            public const string Resident = "SNP_RESIDENT";
            public const string HealthIns = "SNP_HEALTH_INS";
            public const string FoodStamps = "SNP_FOOD_STAMPS";
            public const string WIC = "SNP_WIC";
            public const string IncomeTypes = "INCOME_TYPE";
            public const string EthnicCodes = "SNP_ETHNIC";
            public const string FamilyTypes = "MST_FAMILY_TYPE";
            public const string Housing = "MST_HOUSING";
            public const string Sex = "SNP_SEX";
            public const string Veteran = "SNP_VET";
            public const string Farmer = "SNP_FARMER";
            public const string Disability = "SNP_DISABLE";
            public const string PrimaryLanguage = "MST_LANGUAGE";
            public const string FederalOMB = "MST_POVERTY";
            public const string SMI = "MST_SMI";
            public const string CMI = "MST_CMI";
            public const string HUD = "MST_HUD";
            public const string AreYoupregnant = "SNP_PREGNANT";
            public const string MaritalStatus = "SNP_MARITAL_STATUS";
            public const string PresentlyEmployed = "SNP_EMPLOYED";
            public const string AnyWorkLimitations = "SNP_WORK_LIMIT";
            public const string HourlyWage = "SNP_HOURLY_WAGE";
            public const string Relationship = "SNP_MEMBER_CODE";
            public const string Race = "SNP_RACE";
            public const string SecondaryLanguage = "MST_LANGUAGE_OT";
            public const string Fund = "CHLDMST_FUND_SOURCE";
            public const string School = "SNP_SCHOOL_DISTRICT";
            public const string DriversLicense = "SNP_DRVLIC";
            public const string ReliableTransport = "RELITRAN";
            public const string LegaltoWork = "SNP_LEGAL_TO_WORK";
            public const string NumberinHousehold = "MST_NO_INHH";
            public const string FamilyIncome = "MST_FAM_INCOME";
            public const string NumberinProgram = "MST_NO_INPROG";
            public const string ProgramIncome = "MST_PROG_INCOME";
            public const string ZipCode = "MST_ZIP";
            public const string County = "MST_COUNTY";
            public const string HeadStartAge = "SNP_HEADSTART_AGE";
            public const string WorkStatus = "SNP_WORK_STAT";
            public const string DisconnectedYouth = "SNP_YOUTH";
            public const string Non_CashBen = "MST_NCASHBEN";
            public const string HealthCodes = "SNP_HEALTH_CODES";
            public const string MiltaryStatus = "SNP_MILITARY_STATUS";
            public const string PreassTotal = "MST_PRESS_TOTAL";
            public const string CurrentAge = "SNP_CURRENT_AGE";
            public const string City = "MST_CITY";

        }

        public static class Case2330
        {
            public const string SecurityRestriction = "S00001";
            public const string MedicalCoverage = "S00005";
            public const string Disability = "S00010";
            public const string DoctorName = "S00015";
            public const string DoctorAddress = "S00020";
            public const string DoctorPhoneNo = "S00025";
            public const string AnyOtherMedical = "S00030";
            public const string DentalCoverage = "S00035";
            public const string InsurPlan = "S00040";
            public const string InsurName = "S00045";
            public const string DentistName = "S00050";
            public const string DentistAddress = "S00055";
            public const string DentistPhoneNo = "S00060";
            public const string Fund = "S00065";
            public const string AltFund = "S00070";
            public const string Transport = "S00075";
            public const string ClassPrefer = "S00080";
            public const string PlaceofBirth = "S00085";
            public const string EmergencyContName = "S00090";
            public const string EmergencyContRelation = "S00095";
            public const string EmergencyContAddress = "S00100";
            public const string EmergencyContPhone1 = "S00105";
            public const string EmergencyContPhone2 = "S00110";
            public const string BirthCert = "S00086";
            public const string PreEnrollClient = "S00087";
            public const string Repeater = "S00088";
            public const string NextYearPreparation = "S00089";


        }

        public static class RankQues
        {

            public const string MAlertCode = "MST_ALERT_CODES";
            public const string MCaseType = "MST_CASE_TYPE";
            public const string MSECRET = "MST_SECRET";
            public const string MJuvenile = "MST_JUVENILE";
            public const string MSenior = "MST_SENIOR";
            public const string MIntakeWorker = "MST_INTAKE_WORKER";
            public const string MZip = "MST_ZIP";
            public const string MCounty = "MST_COUNTY";
            public const string MLanguage = "MST_LANGUAGE";
            public const string MLanguageOt = "MST_LANGUAGE_OT";
            public const string MFamilyType = "MST_FAMILY_TYPE";
            public const string MIntakeDate = "MST_INTAKE_DATE";
            public const string MWaitList = "MST_WAIT_LIST";
            public const string MHousing = "MST_HOUSING";
            public const string MSite = "MST_SITE";
            public const string MInitialDate = "MST_INITIAL_DATE";
            public const string MCaseReviewDate = "MST_CASE_REVIEW_DATE";
            public const string MAddressYear = "MST_ADDRESS_YEARS";
            public const string MBestContact = "MST_BEST_CONTACT";
            public const string MAboutUs = "MST_ABOUT_US";
            public const string MEligDate = "MST_ELIG_DATE";
            public const string MVerifier = "MST_VERIFIER";
            public const string MVerifyW2 = "MST_VERIFY_W2";
            public const string MVefiryCheckstub = "MST_VERIFY_CHECK_STUB";
            public const string MVeriTaxReturn = "MST_VERIFY_TAX_RETURN";
            public const string MVerLetter = "MST_VERIFY_LETTER";
            public const string MVerOther = "MST_VERIFY_OTHER";
            public const string MProgIncome = "MST_PROG_INCOME";
            public const string MFamIncome = "MST_FAM_INCOME";
            public const string MNoInprog = "MST_NO_INPROG";
            public const string Mpoverty = "MST_POVERTY";
            public const string MHud = "MST_HUD";
            public const string MCmi = "MST_CMI";
            public const string MSMi = "MST_SMI";
            public const string MReverifyDate = "MST_REVERIFY_DATE";
            public const string MIncomeTypes = "MST_INCOME_TYPES";
            public const string NonCashBenefits = "MST_NCASHBEN";
            public const string HealthCodes = "SNP_HEALTH_CODES";
            public const string DisconectYouth = "SNP_YOUTH";
            public const string WorkStatus = "SNP_WORK_STAT";
            public const string MiltaryStatus = "SNP_MILITARY_STATUS";
            public const string MverifySelfDecl = "MST_VERIFY_SELF_DECL";
            public const string CatElig = "MST_CAT_ELIG";

            //// PREASSES Questions
            //public const string MPJOB = "MST_PRESS_JOB";
            //public const string MPTJOB = "MST_PRESS_PTJOB";
            //public const string MPHsd = "MST_PRESS_HSD";
            //public const string MPSkills = "MST_PRESS_SKILLS";
            //public const string MPHousing = "MST_PRESS_HOUSING";
            //public const string MPTransport = "MST_PRESS_TRANSPORT";
            //public const string MPChildCare = "MST_PRESS_CHLDCARE";
            //public const string MPCCEnrl = "MST_PRESS_CCENRL";
            //public const string MPEldrcare = "MST_PRESS_ELDRCARE";
            //public const string MPEcneed = "MST_PRESS_ECNEED";
            //public const string MPChins = "MST_PRESS_CHINS";
            //public const string MPAhins = "MST_PRESS_AHINS";
            //public const string MPRWeng = "MST_PRESS_RW_ENG";
            //public const string MPCurrDss = "MST_PRESS_CURR_DSS";
            //public const string MPRecvDss = "MST_PRESS_RECV_DSS";

            public const string SStatus = "SNP_STATUS";
            public const string SAltBdate = "SNP_ALT_BDATE";
            public const string SAge = "SNP_AGE";
            public const string SMemberCode = "SNP_MEMBER_CODE";
            public const string SEthinic = "SNP_ETHNIC";
            public const string SRace = "SNP_RACE";
            public const string SEducation = "SNP_EDUCATION";
            public const string SSchoolDistrict = "SNP_SCHOOL_DISTRICT";
            public const string SResident = "SNP_RESIDENT";
            public const string SLegalTowork = "SNP_LEGAL_TO_WORK";
            public const string SExpireWorkDate = "SNP_EXPIRE_WORK_DATE";
            public const string SDrvlic = "SNP_DRVLIC";
            public const string STranserv = "SNP_TRANSERV";
            public const string SSex = "SNP_SEX";
            public const string SPregnant = "SNP_PREGNANT";
            public const string SMartialStatus = "SNP_MARITAL_STATUS";
            public const string SHealthIns = "SNP_HEALTH_INS";
            public const string SSnpVet = "SNP_VET";
            public const string SFoodStamps = "SNP_FOOD_STAMPS";
            public const string SWic = "SNP_WIC";
            public const string SFarmer = "SNP_FARMER";
            public const string SDisable = "SNP_DISABLE";
            public const string SEmployed = "SNP_EMPLOYED";
            public const string SLastWorkDate = "SNP_LAST_WORK_DATE";
            public const string SworkLimit = "SNP_WORK_LIMIT";
            public const string SRelitran = "SNP_RELITRAN";
            public const string MERent = "MST_EXP_RENT";
            public const string MEWater = "MST_EXP_WATER";
            public const string MEElectric = "MST_EXP_ELECTRIC";
            public const string MEHeat = "MST_EXP_HEAT";
            public const string MEDEBTCC = "MST_DEBT_CC";
            public const string MEDEBTLoans = "MST_DEBT_LOANS";
            public const string MEDEBTMed = "MST_DEBT_MED";
            // public const string  = "MST_EXP_EXP3";   
            public const string METotal = "MST_EXP_TOTAL";
            public const string MELiveExpenses = "MST_EXP_LIVEXPENSE";
            public const string MExpCaseworker = "MST_EXP_CASEWORKER";
            public const string SNofcjob = "SNP_NUMBER_OF_C_JOBS";
            public const string SNofljobs = "SNP_NUMBER_OF_LV_JOBS";
            public const string SFThours = "SNP_FULL_TIME_HOURS";
            public const string SPThours = "SNP_PART_TIME_HOURS";
            public const string SSEmploy = "SNP_SEASONAL_EMPLOY";
            public const string S1shift = "SNP_1ST_SHIFT";
            public const string S2ndshift = "SNP_2ND_SHIFT";
            public const string S3rdShift = "SNP_3RD_SHIFT";
            public const string SRshift = "SNP_R_SHIFT";
            public const string SjobTitle = "SNP_JOB_TITLE";
            public const string SjobCategory = "SNP_JOB_CATEGORY";
            public const string SHourlyWage = "SNP_HOURLY_WAGE";
            public const string SPFrequency = "SNP_PAY_FREQUENCY";
            public const string SHireDate = "SNP_HIRE_DATE";
            public const string CMedCoverage = "CHLDMST_MED_COVERAGE";
            public const string CMedicalCoverageType = "CHLDMST_MED_COVER_TYPE";
            public const string CInsCat = "CHLDMST_INS_CAT";
            public const string CDentalCoverage = "CHLDMST_DENTAL_COVERAGE";
            public const string CDisability = "CHLDMST_DISABILITY";
            public const string CDiagNosisDate = "CHLDMST_DIAGNOSIS_DATE";

        }

        public static class Tms00201
        {
            public const string Vendor = "SA0001";
            public const string Payfor = "SA0005";
            public const string BillingName = "S00010";
            public const string Account = "S00015";
        }

        public static class ChldTrck
        {
            public const string GrowChartType = "S0040";
        }

        public static class AgyTabs
        {
            public const string Yesno = "S0001";
            public const string Transport = "S0041";
            public const string ChldCare = "S0042";
            public const string CCENRL = "S0043";
            public const string CHINS = "S0044";
            public const string AHINS = "S0045";
            public const string RECVDSS = "S0054";
            public const string EmployeeJob = "S0055";
            public const string LIHPMQUES = "S0056";

        }
        public static class Preassess
        {
            public const string Employment = "S00010";
            public const string Education = "S00020";
            public const string Training = "S00030";
            public const string Housing = "S00040";
            public const string Transportation = "S00050";
            public const string ChildCare = "S00060";
            public const string Enrollment = "S00061";
            public const string Eldercare = "S00070";
            public const string Childhealth = "S00080";
            public const string AdultHealth = "S00090";
            public const string EducationAbcd = "S00025";
            public const string DSSProgramAbcd = "S00100";
            // public const string DSSProgram2Abcd = "S00094";


        }

        public static class TrigQues
        {
            public const string Age = "SNP_AGE";
            public const string Education = "SNP_EDUCATION";
            public const string Resident = "SNP_RESIDENT";
            public const string HealthIns = "SNP_HEALTH_INS";
            public const string FoodStamps = "SNP_FOOD_STAMPS";
            public const string WIC = "SNP_WIC";
            public const string IncomeTypes = "INCOME_TYPE";
            public const string EthnicCodes = "SNP_ETHNIC";
            public const string FamilyTypes = "MST_FAMILY_TYPE";
            public const string Housing = "MST_HOUSING";
            public const string Sex = "SNP_SEX";
            public const string Veteran = "SNP_VET";
            public const string Farmer = "SNP_FARMER";
            public const string Disability = "SNP_DISABLE";
            public const string PrimaryLanguage = "MST_LANGUAGE";
            public const string FederalOMB = "MST_POVERTY";
            public const string SMI = "MST_SMI";
            public const string CMI = "MST_CMI";
            public const string HUD = "MST_HUD";
            public const string AreYoupregnant = "SNP_PREGNANT";
            public const string MaritalStatus = "SNP_MARITAL_STATUS";
            public const string PresentlyEmployed = "SNP_EMPLOYED";
            public const string AnyWorkLimitations = "SNP_WORK_LIMIT";
            public const string HourlyWage = "SNP_HOURLY_WAGE";
            public const string Relationship = "SNP_MEMBER_CODE";
            public const string Race = "SNP_RACE";
            public const string SecondaryLanguage = "MST_LANGUAGE_OT";
            public const string Fund = "CHLDMST_FUND_SOURCE";
            public const string School = "SNP_SCHOOL_DISTRICT";
            public const string DriversLicense = "SNP_DRVLIC";
            public const string ReliableTransport = "RELITRAN";
            public const string LegaltoWork = "SNP_LEGAL_TO_WORK";
            public const string NumberinHousehold = "MST_NO_INHH";
            public const string FamilyIncome = "MST_FAM_INCOME";
            public const string NumberinProgram = "MST_NO_INPROG";
            public const string ProgramIncome = "MST_PROG_INCOME";
            public const string ZipCode = "MST_ZIP";
            public const string Lpblevel = "LPB_LEVEL";
            public const string LpbType = "LPB_TYPE";
            public const string LpbSource = "LPB_SOURCE";
            public const string CheckDate = "PMR_CHECK_DATE";
            public const string ClaimDate = "EAPFP_CLAIMDATE";
            public const string ClaimAmount = "EAPFP_CLAIMAMOUNT";
            public const string Key = "EAPFP_KEY";
            public const string ProgramCK1 = "STATUS_PROGRAMCK1";
            public const string Status1 = "STATUS_STATUS1";
            public const string ProgramCK2 = "STATUS_PROGRAMCK2";
            public const string Status2 = "STATUS_STATUS2";
            public const string ProgramCK3 = "STATUS_PROGRAMCK3";
            public const string Status3 = "STATUS_STATUS3";
            public const string ProgramCK4 = "STATUS_PROGRAMCK4";
            public const string Status4 = "STATUS_STATUS4";
            public const string DisableAge = "DISABLE_AGE";
            public const string Payfor = "PMR_PAYMENT_FOR";
            public const string Vulnerable = "VULNERABLE";
            public const string Intakedate = "MST_INTAKE_DATE";

        }

        #region Added by Vikash on 11/03/2023 for adding Matrix Assessment Dates to Field Control Maintenance
        public static class MatrixAssDates
        {
            public const string AssementDte = "SA0001";
            public const string CaseWorker = "S00002";
            public const string FollowUpOnDte = "S00003";
            public const string CompleteDte = "S00004";
        }
        #endregion

    }
}

























