/**********************************************************************************************************
 * Class Name   : UserEntity
 * Author       : 
 * Created Date : 
 * Version      : 
 * Description  : Entity object to extend ObjectUsersType.
 **********************************************************************************************************/

#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Utilities;
using System.Web.UI.WebControls;
using System.Data;

#endregion

namespace Captain.Common.Model.Objects
{
    /// <summary>
    /// Entity Object
    /// </summary>
    [Serializable]
    public class UserEntity : IComparable<UserEntity>
    {
        #region Constructors

        public UserEntity()
        {
             UserID   = string.Empty;
             Password   = string.Empty;
             Successful   = string.Empty;
             UnSuccessful   = string.Empty;
             LastSuccessful   = string.Empty;
             LastUnSuccessful   = string.Empty;
             CaseWorker   = string.Empty;
             LastName   = string.Empty;
             FirstName   = string.Empty;
             MI   = string.Empty;
             QUE   = string.Empty;
             Form   = string.Empty;
             Mess   = string.Empty;
             AccessAll   = string.Empty;
             TemplateUser   = string.Empty;
             DefaultPrinter = string.Empty;
             LabelPrinter   = string.Empty;
             StaffCode   = string.Empty;
             AccessImages   = string.Empty;
             TaskSeq   = string.Empty;
             FastLoad   = string.Empty;
             LoadData   = string.Empty;
             CalcSBCB   = string.Empty;
             Path   = string.Empty;
             Security   = string.Empty;
             Supervisor   = string.Empty;
             Site   = string.Empty;
             EMS_Access   = string.Empty;
             Components   = string.Empty;
             //C2   = string.Empty;
             //C3   = string.Empty;
             //C4   = string.Empty;
             //C5   = string.Empty;
             //C6   = string.Empty;
             //C7   = string.Empty;
             //C8   = string.Empty;
             //C9   = string.Empty;
             Color   = string.Empty;
             Agency   = string.Empty;
             Dept   = string.Empty;
             Prog   = string.Empty;
             Year   = string.Empty;
             InActiveFlag   = string.Empty;
             ImageTypes   = string.Empty;
             DateLSTC   = string.Empty;
             LSTCOperator   = string.Empty;
             DateAdd   = string.Empty;
             AddOperator = string.Empty;
             PWDChangeDate = string.Empty;

            PWRMobile = string.Empty;
            PWRDob = string.Empty;
            OptInDATE = string.Empty;
        }

        public UserEntity(DataTable userProfile)
        {
            if (userProfile != null)
            {
                DataRow row = userProfile.Rows[0];
                UserID = row["PWR_EMPLOYEE_NO"].ToString().Trim();
                Password = row["PWR_PASSWORD"].ToString().Trim();
                UnSuccessful = "0".ToString();
                if (!string.IsNullOrEmpty(row["PWR_UNSUCCESSFUL"].ToString()))
                    UnSuccessful = row["PWR_UNSUCCESSFUL"].ToString();
                if ((!UserID.Equals("@InCorrect@1UID@2PSW")) && (!UserID.Equals("@InActiveUserId")))
                {
                    if (!UserID.Equals("@NoHierarchy"))
                    {
                        //Password = row["PWR_PASSWORD"].ToString();
                        Successful = row["PWR_SUCCESSFUL"].ToString().Trim();
                        //UnSuccessful = row["PWR_UNSUCCESSFUL"].ToString();
                        LastSuccessful = row["PWR_LAST_SUCCESSFUL_DATE"].ToString().Trim();
                        LastUnSuccessful = row["PWR_LAST_UNSUCCESSFUL_DATE"].ToString().Trim();
                        CaseWorker = row["PWR_CASEWORKER"].ToString().Trim();
                        LastName = row["PWR_NAME_IX_LAST"].ToString().Trim();
                        FirstName = row["PWR_NAME_IX_FIRST"].ToString().Trim();
                        MI = row["PWR_NAME_IX_MI"].ToString().Trim();
                        QUE = row["PWR_QUE"].ToString().Trim();
                        Form = row["PWR_FORM"].ToString().Trim();
                        Mess = row["PWR_MESS"].ToString().Trim();
                        AccessAll = row["PWR_ACCESS_ALL"].ToString().Trim();
                        TemplateUser = row["PWR_TEMPLATE_USER"].ToString().Trim();
                        DefaultPrinter = row["PWR_DEFAULT_PRINTER"].ToString().Trim();
                        LabelPrinter = row["PWR_LABEL_PRINTER"].ToString().Trim();
                        StaffCode = row["PWR_STAFF_CODE"].ToString().Trim();
                        AccessImages = row["PWR_ACCESS_IMAGES"].ToString().Trim();
                        TaskSeq = row["PWR_TASK_SEQ"].ToString().Trim();
                        FastLoad = row["PWR_FAST_LOAD"].ToString().Trim();
                        LoadData = row["PWR_LOAD_DATA"].ToString().Trim();
                        CalcSBCB = row["PWR_CALC_SBCB"].ToString().Trim();
                        Path = row["PWR_PATH"].ToString().Trim();
                        Security = row["PWR_SECURITY"].ToString().Trim();
                        Supervisor = row["PWR_SUPERVISOR"].ToString().Trim();
                        Site = row["PWR_SITE"].ToString().Trim();
                        EMS_Access = row["PWR_EMS_ACCESS"].ToString().Trim();
                        Components = row["PWR_COMPONENTS"].ToString().Trim();
                        //C2 = row["PWR_C2"].ToString().Trim();
                        //C3 = row["PWR_C3"].ToString().Trim();
                        //C4 = row["PWR_C4"].ToString().Trim();
                        //C5 = row["PWR_C5"].ToString().Trim();
                        //C6 = row["PWR_C6"].ToString().Trim();
                        //C7 = row["PWR_C7"].ToString().Trim();
                        //C8 = row["PWR_C8"].ToString().Trim();
                        //C9 = row["PWR_C9"].ToString().Trim();
                        Color = row["PWR_COLOR"].ToString().Trim();
                        Agency = row["PWR_DEF_AGENCY"].ToString().Trim();
                        Dept = row["PWR_DEF_DEPT"].ToString().Trim();
                        Prog = row["PWR_DEF_PROG"].ToString().Trim();
                        Year = row["PWR_DEF_YEAR"].ToString().Trim();
                        InActiveFlag = row["PWR_INACTIVE_FLAG"].ToString().Trim();
                        ImageTypes = row["PWR_IMAGE_TYPES"].ToString().Trim();
                        DateLSTC = row["PWR_DATE_LSTC"].ToString().Trim();
                        LSTCOperator = row["PWR_LSTC_OPERATOR"].ToString().Trim();
                        DateAdd = row["PWR_DATE_ADD"].ToString().Trim();
                        AddOperator = row["PWR_ADD_OPERATOR"].ToString().Trim();
                        PWDChangeDate = row["PWR_CHANGE_DATE"].ToString().Trim();
                        //PWDSearchDatabase = row["PWR_SEARCH_DATABASE"].ToString().Trim();
                        PWDEmail= row["PWR_EMAIL"].ToString().Trim();
                        PWDSearchPIP = row["PWR_SEARCH_PIP"].ToString().Trim();
                        PWRDob = row["PWR_DOB"].ToString().Trim();
                        PWRMobile = row["PWR_MOBILE"].ToString().Trim();
                        OptInDATE = row["PWR_OPT_IN"].ToString().Trim();
                    }
                }

            }
        }

        public UserEntity(DataRow row)
        {
            if (row != null)
            {                
                UserID = row["PWR_EMPLOYEE_NO"].ToString().Trim();
                Password = row["PWR_PASSWORD"].ToString().Trim();
                Successful = row["PWR_SUCCESSFUL"].ToString().Trim();
                UnSuccessful = "0".ToString().Trim();
                if (!string.IsNullOrEmpty(row["PWR_UNSUCCESSFUL"].ToString()))
                    UnSuccessful = row["PWR_UNSUCCESSFUL"].ToString().Trim();
                LastSuccessful = row["PWR_LAST_SUCCESSFUL_DATE"].ToString().Trim();
                LastUnSuccessful = row["PWR_LAST_UNSUCCESSFUL_DATE"].ToString().Trim();
                CaseWorker = row["PWR_CASEWORKER"].ToString().Trim();
                LastName = row["PWR_NAME_IX_LAST"].ToString().Trim();
                FirstName = row["PWR_NAME_IX_FIRST"].ToString().Trim();
                MI = row["PWR_NAME_IX_MI"].ToString().Trim();
                QUE = row["PWR_QUE"].ToString().Trim();
                Form = row["PWR_FORM"].ToString().Trim();
                Mess = row["PWR_MESS"].ToString().Trim();
                AccessAll = row["PWR_ACCESS_ALL"].ToString().Trim();
                TemplateUser = row["PWR_TEMPLATE_USER"].ToString().Trim();
                DefaultPrinter = row["PWR_DEFAULT_PRINTER"].ToString().Trim();
                LabelPrinter = row["PWR_LABEL_PRINTER"].ToString().Trim();
                StaffCode = row["PWR_STAFF_CODE"].ToString().Trim();
                AccessImages = row["PWR_ACCESS_IMAGES"].ToString().Trim();
                TaskSeq = row["PWR_TASK_SEQ"].ToString().Trim();
                FastLoad = row["PWR_FAST_LOAD"].ToString().Trim();
                LoadData = row["PWR_LOAD_DATA"].ToString().Trim();
                CalcSBCB = row["PWR_CALC_SBCB"].ToString().Trim();
                Path = row["PWR_PATH"].ToString().Trim();
                Security = row["PWR_SECURITY"].ToString().Trim();
                Supervisor = row["PWR_SUPERVISOR"].ToString().Trim();
                Site = row["PWR_SITE"].ToString().Trim();
                EMS_Access = row["PWR_EMS_ACCESS"].ToString().Trim();
                Components = row["PWR_COMPONENTS"].ToString().Trim();
                //C2 = row["PWR_C2"].ToString().Trim();
                //C3 = row["PWR_C3"].ToString().Trim();
                //C4 = row["PWR_C4"].ToString().Trim();
                //C5 = row["PWR_C5"].ToString().Trim();
                //C6 = row["PWR_C6"].ToString().Trim();
                //C7 = row["PWR_C7"].ToString().Trim();
                //C8 = row["PWR_C8"].ToString().Trim();
                //C9 = row["PWR_C9"].ToString().Trim();
                Color = row["PWR_COLOR"].ToString().Trim();
                Agency = row["PWR_DEF_AGENCY"].ToString().Trim();
                Dept = row["PWR_DEF_DEPT"].ToString().Trim();
                Prog = row["PWR_DEF_PROG"].ToString().Trim();
                Year = row["PWR_DEF_YEAR"].ToString().Trim();
                InActiveFlag = row["PWR_INACTIVE_FLAG"].ToString().Trim();
                ImageTypes = row["PWR_IMAGE_TYPES"].ToString().Trim();
                DateLSTC = row["PWR_DATE_LSTC"].ToString().Trim();
                LSTCOperator = row["PWR_LSTC_OPERATOR"].ToString().Trim();
                DateAdd = row["PWR_DATE_ADD"].ToString().Trim();
                AddOperator = row["PWR_ADD_OPERATOR"].ToString().Trim();
                PWDChangeDate = row["PWR_CHANGE_DATE"].ToString().Trim();
                //PWDSearchDatabase = row["PWR_SEARCH_DATABASE"].ToString().Trim();
                PWDEmail = row["PWR_EMAIL"].ToString().Trim();
                PWDSearchPIP = row["PWR_SEARCH_PIP"].ToString().Trim();
                PWRDob = row["PWR_DOB"].ToString().Trim();
                PWRMobile = row["PWR_MOBILE"].ToString().Trim();
                OptInDATE = row["PWR_OPT_IN"].ToString().Trim();
            }
        }

        public UserEntity(DataRow row,string Type)
        {
            if (row != null)
            {
                UserID = row["PWR_EMPLOYEE_NO"].ToString().Trim();
                Password = row["PWR_PASSWORD"].ToString().Trim();
                //Successful = row["PWR_SUCCESSFUL"].ToString().Trim();
                //UnSuccessful = "0".ToString().Trim();
                //if (!string.IsNullOrEmpty(row["PWR_UNSUCCESSFUL"].ToString()))
                //    UnSuccessful = row["PWR_UNSUCCESSFUL"].ToString().Trim();
                //LastSuccessful = row["PWR_LAST_SUCCESSFUL_DATE"].ToString().Trim();
                //LastUnSuccessful = row["PWR_LAST_UNSUCCESSFUL_DATE"].ToString().Trim();
                CaseWorker = row["PWR_CASEWORKER"].ToString().Trim();
                LastName = row["PWR_NAME_IX_LAST"].ToString().Trim();
                FirstName = row["PWR_NAME_IX_FIRST"].ToString().Trim();
                MI = row["PWR_NAME_IX_MI"].ToString().Trim();
                QUE = row["PWR_QUE"].ToString().Trim();
                Form = row["PWR_FORM"].ToString().Trim();
                Mess = row["PWR_MESS"].ToString().Trim();
                AccessAll = row["PWR_ACCESS_ALL"].ToString().Trim();
                TemplateUser = row["PWR_TEMPLATE_USER"].ToString().Trim();
                DefaultPrinter = row["PWR_DEFAULT_PRINTER"].ToString().Trim();
                LabelPrinter = row["PWR_LABEL_PRINTER"].ToString().Trim();
                StaffCode = row["PWR_STAFF_CODE"].ToString().Trim();
                AccessImages = row["PWR_ACCESS_IMAGES"].ToString().Trim();
                TaskSeq = row["PWR_TASK_SEQ"].ToString().Trim();
                FastLoad = row["PWR_FAST_LOAD"].ToString().Trim();
                LoadData = row["PWR_LOAD_DATA"].ToString().Trim();
                CalcSBCB = row["PWR_CALC_SBCB"].ToString().Trim();
                Path = row["PWR_PATH"].ToString().Trim();
                Security = row["PWR_SECURITY"].ToString().Trim();
                Supervisor = row["PWR_SUPERVISOR"].ToString().Trim();
                Site = row["PWR_SITE"].ToString().Trim();
                EMS_Access = row["PWR_EMS_ACCESS"].ToString().Trim();
                Components = row["PWR_COMPONENTS"].ToString().Trim();
                
                Color = row["PWR_COLOR"].ToString().Trim();
                Agency = row["PWR_DEF_AGENCY"].ToString().Trim();
                Dept = row["PWR_DEF_DEPT"].ToString().Trim();
                Prog = row["PWR_DEF_PROG"].ToString().Trim();
                Year = row["PWR_DEF_YEAR"].ToString().Trim();
                InActiveFlag = row["PWR_INACTIVE_FLAG"].ToString().Trim();
                ImageTypes = row["PWR_IMAGE_TYPES"].ToString().Trim();
                DateLSTC = row["PWR_DATE_LSTC"].ToString().Trim();
                LSTCOperator = row["PWR_LSTC_OPERATOR"].ToString().Trim();
                DateAdd = row["PWR_DATE_ADD"].ToString().Trim();
                AddOperator = row["PWR_ADD_OPERATOR"].ToString().Trim();
                PWDChangeDate = row["PWR_CHANGE_DATE"].ToString().Trim();
                //PWDSearchDatabase = row["PWR_SEARCH_DATABASE"].ToString().Trim();
                PWDEmail = row["PWR_EMAIL"].ToString().Trim();
                PWDSearchPIP = row["PWR_SEARCH_PIP"].ToString().Trim();
                PWRDob = row["PWR_DOB"].ToString().Trim();
                PWRMobile = row["PWR_MOBILE"].ToString().Trim();
                OptInDATE = row["PWR_OPT_IN"].ToString().Trim();
            }
        }


        #endregion

        #region Properties

        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Mode { get; set; }

        public bool IsNewUser { get; set; }
        public string Password { get; set; }
        public string Successful { get; set; }
        public string UnSuccessful { get; set; }
        public string LastSuccessful { get; set; }
        public string LastUnSuccessful { get; set; }
        public string CaseWorker { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MI { get; set; }
        public string QUE { get; set; }
        public string Form { get; set; }
        public string Mess { get; set; }
        public string AccessAll { get; set; }
        public string TemplateUser { get; set; }
        public string DefaultPrinter{ get; set; }
        public string LabelPrinter { get; set; }
        public string StaffCode { get; set; }
        public string AccessImages { get; set; }
        public string TaskSeq { get; set; }
        public string FastLoad { get; set; }
        public string LoadData { get; set; }
        public string CalcSBCB { get; set; }
        public string Path { get; set; }
        public string Security { get; set; }
        public string Supervisor { get; set; }
        public string Site { get; set; }
        public string EMS_Access { get; set; }
        public string Components { get; set; }
        //public string C2 { get; set; }
        //public string C3 { get; set; }
        //public string C4 { get; set; }
        //public string C5 { get; set; }
        //public string C6 { get; set; }
        //public string C7 { get; set; }
        //public string C8 { get; set; }
        //public string C9 { get; set; }
        public string Color { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string Year { get; set; }
        public string InActiveFlag { get; set; }
        public string ImageTypes { get; set; }
        public string DateLSTC { get; set; }
        public string LSTCOperator { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string PWDChangeDate { get; set; }
        //public string PWDSearchDatabase { get; set; }
        public string PWDEmail { get; set; }
        public string PWDSearchPIP { get; set; }
        public string PWRDob { get; set; }
        public string PWRMobile { get; set; }

        public string OptInDATE { get; set; }

        #endregion

        #region Public / Overrides Methods

        public override bool Equals(object obj)
        {
            bool returnValue = false;

            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
            {
                returnValue = false;
            }

            UserEntity user = obj as UserEntity;
            if (user != null)
            {
                returnValue = (user.UserID.Equals(UserID)) && (user.UserName.Equals(UserName));
            }
            return returnValue;
        }

        public override int GetHashCode()
        {
            return UserID.GetHashCode() ^ UserName.GetHashCode() ^ Mode.GetHashCode() ^ IsNewUser.GetHashCode();
        }

        public int CompareTo(UserEntity other)
        {
            return UserName.CompareTo(other.UserName);
        }

        #endregion
    }
}
