using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Utilities;
using System.Web.UI.WebControls;
using System.Data;

namespace Captain.Common.Model.Objects
{
    /// <summary>
    /// Entity Object
    /// </summary>
    [Serializable]
    public class PrivilegeEntity
    {
        #region Constructors

        public PrivilegeEntity()
        {
            UserID = string.Empty;
            ModuleCode = string.Empty;
            Program = string.Empty;
            Hierarchy = string.Empty;
            AddPriv = string.Empty;
            ChangePriv = string.Empty;
            DelPriv = string.Empty;
            ViewPriv = string.Empty;
            PrivilegeName = string.Empty;
            DateLSTC = string.Empty;
            LSTCOperator = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            screenType = string.Empty;
            showMenu = string.Empty;
        }

        public PrivilegeEntity(DataRow userPrivelege, string PrivilegeType)
        {
            if (userPrivelege != null)
            {
                DataRow row = userPrivelege;
                if (PrivilegeType.Equals("Screen"))
                {
                    screenType = string.Empty;
                    UserID = row["EFR_EMPLOYEE_NO"].ToString();
                    ModuleCode = row["EFR_MODULE_CODE"].ToString();
                    Program = row["EFR_PROGNO"].ToString();
                    Hierarchy = row["EFR_HIERARCHY"].ToString();
                    AddPriv = row["EFR_ADD_PRIV"].ToString().Equals("Y") ? "true" : "false";
                    ChangePriv = row["EFR_CHG_PRIV"].ToString().Equals("Y") ? "true" : "false";
                    DelPriv = row["EFR_DEL_PRIV"].ToString().Equals("Y") ? "true" : "false";
                    ViewPriv = row["EFR_INQ_PRIV"].ToString().Equals("Y") ? "true" : "false";
                    PrivilegeName = row["EFR_DESCRIPTION"].ToString();
                    DateLSTC = row["EFR_DATE_LSTC"].ToString();
                    LSTCOperator = row["EFR_LSTC_OPERATOR"].ToString();
                    DateAdd = row["EFR_DATE_ADD"].ToString();
                    AddOperator = row["EFR_ADD_OPERATOR"].ToString();
                    showMenu = row["CFC_SHOW_IN_MENU"].ToString();
                    if (row.Table.Columns.Contains("CFC_BRANCH"))
                        screenType = row["CFC_BRANCH"].ToString();
                    //ModuleName = string.Empty;
                    ModuleName = row["MODULE_NAME"].ToString();
                }
                else if (PrivilegeType.Equals("Module"))
                {
                    ModuleCode = row["MODULE_CODE"].ToString();
                    ModuleName = row["MODULE_NAME"].ToString();
                }
                else if (PrivilegeType.Equals("UserReportMaintenance"))
                {
                    UserID = row["ADHASSOC_USER"].ToString();
                    ModuleCode = row["ADHASSOC_MODULE"].ToString();
                    ModuleName = "UserReportMaintenance";
                    ViewPriv = row["ADHASSOC_ID"].ToString();
                    Program = "CASB0012"; //row["BAT_VIEW_PRIV"].ToString().Equals("Y") ? "true" : "false";
                    PrivilegeName = row["ADHASSOC_DESC"].ToString();
                    // DateLSTC = row["BAT_DATE_LSTC"].ToString();
                    LSTCOperator = row["ADHASSOC_ID_USER"].ToString();
                    DateAdd = row["ADHASSOC_ADD_DATE"].ToString();
                    AddOperator = row["ADHASSOC_ADD_OPERATOR"].ToString();
                }
                else
                {
                    UserID = row["BAT_EMPLOYEE_NO"].ToString();
                    ModuleCode = row["BAT_MODULE_CODE"].ToString();
                    Program = row["BAT_REPORT_CODE"].ToString();
                    ViewPriv = row["BAT_VIEW_PRIV"].ToString().Equals("Y") ? "true" : "false";
                    PrivilegeName = row["BAT_REPORT_NAME"].ToString();
                    DateLSTC = row["BAT_DATE_LSTC"].ToString();
                    LSTCOperator = row["BAT_LSTC_OPERATOR"].ToString();
                    DateAdd = row["BAT_DATE_ADD"].ToString();
                    AddOperator = row["BAT_ADD_OPERATOR"].ToString();
                    //ModuleName = string.Empty;
                    ModuleName = row["MODULE_NAME"].ToString();
                }
            }
        }

        #endregion

        #region Properties

        public string UserID { get; set; }
        public string PrivilegeType { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public string Program { get; set; }
        public string Hierarchy { get; set; }
        public string AddPriv { get; set; }
        public string ChangePriv { get; set; }
        public string DelPriv { get; set; }
        public string ViewPriv { get; set; }
        public string PrivilegeName { get; set; }
        public string DateLSTC { get; set; }
        public string LSTCOperator { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string showMenu { get; set; }
        public string screenType { get; set; }
        #endregion


    }


    public class MenuBranchEntity
    {
        #region Constructors

        public MenuBranchEntity()
        {

            MemberSeq = string.Empty;
            MemberCode = string.Empty;
            MemberDesc = string.Empty;
        }

        public MenuBranchEntity(DataRow drMenubranch)
        {
            if (drMenubranch != null)
            {
                DataRow row = drMenubranch;

                MemberSeq = row["MENBR_SEQ"].ToString();
                MemberCode = row["MENBR_CODE"].ToString();
                MemberDesc = row["MENBR_DESC"].ToString();


            }
        }

        #endregion

        #region Properties

        public string MemberSeq { get; set; }
        public string MemberCode { get; set; }
        public string MemberDesc { get; set; }

        #endregion


    }
}
