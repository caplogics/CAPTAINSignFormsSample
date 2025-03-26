using System;
using Captain.Common.Utilities;
using Captain.Common.Views.UserControls.Base;
using Wisej.Web;
using Captain.Common.Handlers;
using Captain.Common.Controllers;


namespace Captain.Common.Interfaces
{
    public interface IBaseForm
    {
        string Locale { get; }
        string UserID { get; }
        string UserName { get; }
        string BusinessModuleID { get; }
        string BaseAgency { get; set; }
        string BaseDept { get; set; }
        string BaseProg { get; set; }
        string BaseYear { get; set; }
        TreeViewController TreeViewController { get; }

        void RefreshNode(TreeNode node);
        
        //string GetPreferenceValue(string preferenceNameDisplayCode, string moduleID);
    }
}
