/***************************************************************************************
 * Class Name    : Enumerations file
 * Author        : 
 * Created Date  : 
 * Version       : 
 * Description   : This class has enumerations
 ***************************************************************************************/

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Captain.Common.Utilities
{
    public enum CheckOutSteps
    {
        Initialize,
        CheckOutFile
    }

    public enum CheckInSteps
    {
        Initialize,
        SaveBeforeCheckInDocument,
        UploadFile,
        CheckInFile
    }

    public enum WinControlParamType
    {
        Boolean,
        DateTime,
        Number,
        String
    }

    public enum SpinnerControlType
    {
        ContentManagerControl,
        ModulesControl,
        TreeNode,
        BICommentControl        // BI Demo
    }

    public enum UserAttributes
    {
        AttributeID,
        AttributeTypeID,
        UserID,
        UserName
    }

    public enum FileOpenType
    {
        OpenNoneVersionedFile,
        OpenVersionedFile,
        OpenNoneVersionedFileWithPrompt
    }

    public enum PayloadNodeTypes
    {
        Simple,
        Complex
    }

    public enum SortDirection
    {
        Ascending = 0,
        Descending = 1
    }

    public enum PluginUsageTypes
    {
        [Description("Generic Form")]
        GenericForm,

        [Description("Issue Form")]
        IssueForm,

        [Description("Project Form")]
        ProjectForm,

        [Description("Task Form")]
        TaskForm,

        [Description("Workflow Form")]
        WorkflowForm,

        [Description("Report Form")]
        ReportForm
    }

    public enum CellType
    {
        ComboBox,
        TextBox,
        DatePicker,
        DateTimePicker,
        TimePicker,
        CheckBox,
        HyperLink,
    }

    public enum QuantumFaults
    {
        AttributeNotFound,
        AttributeNameNotFound,
        AuthenticationException,
        AuthenticationFailed,
        ClassNotFound,
        CompanyAlreadyExisits,
        CompanyNotFound,
        ConnectorFault,
        ContentAlreadyExists,
        CopyToFolderNotFound,
        CurrentPasswordVerificationFailed,
        DBRuntimeFault,
        DeleteCompanyNotAllowed,
        DocumentClassNotFound,
        DocumentHasNoContent,
        DocumentAleadyCheckedOut,
        DocumentCheckedOutByOtherUser,
        DocumentNotCheckedOut,
        DocumentNotFound,
        ESBMessageProcessingFailed,
        ExternalServiceFault,
        FileAleadyCheckedOut,
        FileCheckedOutByOtherUser,
        FileNotCheckedOut,
        FileNotFound,
        FolderNotFound,
        InsufficientPermissions,
        InvalidFormula,
        InsufficientPrivilege,
        InsufficientPrivileges,
        InvalidFolderID,
        InvalidPlugin,
        InvalidPluginUsage,
        InvalidPackageState,
        InvalidQuotaSize,
        InvalidRepositoryName,
        InvalidVersion,
        MandatoryAttributesMissing,
        None,
        NotAllowed,
        NoWFUsers,
        LocalFileFault,
        ObjectNotFound,
        OIDConnectionFailed,
        OIDExcetion,
        PathNotFound,
        ProjectRepositoryNotDefined,
        ProjectRepositoryNotFound,
        QuantumFault,
        QuantumRuntimeFault,
        QuantumRuleFault,
        RenditionFormatNotFound,
        RenditionFormatExtensionNotFound,
        RenditionFormatIndeterminate,
        RenditionAlreadyExists,
        RepositoryNotFound,
        RepositoryLoginFailed,
        RuntimeFault,
        StaleDataError,
        TargetFolderNotFound,
        UnableToCancel,
        UnableToChangePassword,
        UnableToCreateFile,
        UnableToDelete,
        UnableToResetPassword,
        UserAlreadyExisits,
        UserLocked,
        UserNotFound,
        UserNotLoggedIn,
        Server
    }

    public enum HierarchicalContextMenus
    {
        New,
        NewItem,
        NewItemsFrom,
        NewItemsFromTaxonomy,
        NewIssue,
        NewWorkflow,
        Open,
        DividerBetweenOpenAssociate,
        Associate,
        Import,
        DividerBetweenImportCheckOut,
        CheckIn,
        CheckOut,
        CancelCheckOut,
        DividerBetweenCancelCheckOutLifecycle,
        Lifecycle,
        LifecycleReplace,
        LifecycleAppend,
        LifecycleDelete,
        DividerBetweenLifecycleDeleteEditRecipients,
        EditRecipients,
        DividerBetweenEditRecipientsEdit,
        Edit,
        EditSortSiblings,
        EditCopy,
        EditPaste,
        EditDelete,
        DividerBetweenEditDelteEditChangeSelectedRows,
        EditConvert,
        EditChangeSelectedRows,
        DividerBetweenEditChangeSelectedRowsEditSelectAllSiblings,
        EditSelectAllSiblings,
        EditUnSelectAll,
        EditRename,
        DividerBetweenViewCollapseAllViewFilter,
        ViewFilter,
        DividerBetweenEditFilterView,
        View,
        ViewExpandAll,
        ViewCollapseAll
    }

    public enum TreeType
    {
        Welcome,
        Administration,
        HeadStart,
        CaseManagement,
        EnergyRI,
        EnergyCT,
        EmergencyAssistance,
        AccountsReceivable,
        HousingWeatherization,
        DashBoard,
        HealthyStart,
        AgencyPartner
    }

    public enum DataTypes
    {
        Boolean = 1,
        DateTime = 2,
        String = 3,
    }

   
    public enum SecurityPackageSteps
    {
        InitializeUpdateControl,
        CheckSecurity,
        GeneratePackage,
        CheckPackageExists,
        EnableInstall
    }

    public enum ViewTypeTags
    {
        Cumulative,
        Current,
        Sequence,
        Filefolder,
        RelatedSequence
    }

    public enum LineStyle
    {
        Dashed,
        Dotted,
        Double,
        Groove,
        Inset,
        None,
        Outset,
        Ridge,
        Solid
    }

    public enum ImageTypes
    {
        Gif,
        Jpg,
        Png
    }

    public enum LineOrientation
    {
        Horizontal,
        Vertical
    }

    public enum GlobalNavigationTypes
    {
        Inbox = 0,
        DocumentPublisher = 1,
        SubmissionManager = 2,
        SubmissionArchive = 3,
        SubmissionViewer = 4,
        ContentManager = 5,
        MetadataRegistry = 6
    }

    public enum PDFAccessMode
    {
        ViewMode = 0,
        EditMode = 1,
    }

    public enum SearchFieldDataTypes
    {
        String,
        Numeric,
        DateTime,
        Date,
        Time,
        Boolean
    }

    public enum ColumnDataTypes
    {
        /// <summary>
        /// Specifies a string column.
        /// </summary>
        String = 1,
        /// <summary>
        /// Specifies a numeric column.
        /// </summary>
        Number = 2,
        /// <summary>
        /// Specifies a datetime column.
        /// </summary>
        DateTime = 3,
        /// <summary>
        /// Specifies a date column.
        /// </summary>
        Date = 4,
        /// <summary>
        /// Specifies a time column.
        /// </summary>
        Time = 5,
        /// <summary>
        /// Specifies a boolean column.
        /// </summary>
        BooleanYesNo = 6,
        /// <summary>
        /// Specifies a boolean column.
        /// </summary>
        BooleanTrueFalse = 7,
        /// <summary>
        /// Specifies a currency column.
        /// </summary>
        Currency = 8
    }

    public enum ExportFormat
    {
        /// <summary>
        /// Exports as comma delimited.
        /// </summary>
        CSV = 1,
        /// <summary>
        /// Exports as Excel xml
        /// </summary>
        Excel = 2,
        /// <summary>
        /// Exports as Txt file
        /// </summary>
        Txt = 3,
        /// <summary>
        /// Exports as Doc file
        /// </summary>
        Doc = 4
    }

    public enum UserStatus : int
    {
        Active = 1,
        Locked = 2,
        Deleted = 3
    }

    public enum Commands
    {
        Add,
        AutoInitializeForm,
        Clear,
        Delete,
        Edit,
        Error,
        Get,
        Initialize,
        Move,
        Rename,
        Save,
        Search,
        Set,
        Update,
        Validate,
        Refresh,
        CloseForm,
        HideForm,
        StopTimer,
        CheckPrivilegeToUseForm
    }

    public enum PreferenceCommentsICreate
    {
        Private,
        Public
    }

    public enum PreferenceStartingTasks
    {
        Always,
        Never,
        Prompt
    }

    public enum TaskStatus
    {
        Completed,
        Assigned
    }

    public enum Comparator
    {
        Equals,
        Contains,
        GreaterThan,
        LessThan,
        Range            // BI Demo
     }

    public enum BICommunications
    {
        CommunicationSearch,
        CommunicationSearchResult
    }
}
