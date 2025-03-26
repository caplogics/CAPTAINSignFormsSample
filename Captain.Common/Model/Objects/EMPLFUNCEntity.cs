using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Utilities;
using System.Web.UI.WebControls;
using System.Data;

namespace Captain.Common.Model.Objects
{
    public class EMPLFUNCEntity
    {
        #region Constructors

        public EMPLFUNCEntity()
        {

            UserName = string.Empty;
            Module = string.Empty;
            ScrCode = string.Empty;
            Hierarchy = string.Empty;
            Add_Priv = string.Empty;
            Chg_Priv = string.Empty;
            Del_Priv = string.Empty;
            Inq_Priv = string.Empty;
            Alt_User = string.Empty;
            Alt_Module = string.Empty;
            ScrDesc = string.Empty;
            LstcDate = string.Empty;
            LstcOpr = string.Empty;
            AddDate = string.Empty;
            AddOpr = string.Empty;
            CaseWoeker = string.Empty;
            Name = string.Empty;
        }

        public EMPLFUNCEntity(DataRow EMPLFUNC)
        {
            if (EMPLFUNC != null)
            {
                DataRow row = EMPLFUNC;

                UserName = row["EFR_EMPLOYEE_NO"].ToString();
                Module = row["EFR_MODULE_CODE"].ToString();
                ScrCode = row["EFR_PROGNO"].ToString();
                Hierarchy = row["EFR_HIERARCHY"].ToString();
                Add_Priv = row["EFR_ADD_PRIV"].ToString();
                Chg_Priv = row["EFR_CHG_PRIV"].ToString();
                Del_Priv = row["EFR_DEL_PRIV"].ToString();
                Inq_Priv = row["EFR_INQ_PRIV"].ToString();
                //Alt_User = row["EFR_ALT_EMPLOYEE_NO"].ToString();
                //Alt_Module = row["EFR_ALT_MODULE_CODE"].ToString();
                ScrDesc = row["EFR_DESCRIPTION"].ToString().Trim();
                LstcDate = row["EFR_DATE_LSTC"].ToString();
                LstcOpr = row["EFR_LSTC_OPERATOR"].ToString();
                AddDate = row["EFR_DATE_ADD"].ToString();
                AddOpr = row["EFR_ADD_OPERATOR"].ToString();
                CaseWoeker = row["CaseWoeker"].ToString();
                Name = row["Name"].ToString();
            }
        }

        public EMPLFUNCEntity(DataRow EMPLFUNC,string strTable)
        {
            if (EMPLFUNC != null)
            {
                DataRow row = EMPLFUNC;

                UserName = row["EFR_EMPLOYEE_NO"].ToString();
                Module = row["EFR_MODULE_CODE"].ToString();
                ScrCode = row["EFR_PROGNO"].ToString();
                //Hierarchy = row["EFR_HIERARCHY"].ToString();
                //Add_Priv = row["EFR_ADD_PRIV"].ToString();
                //Chg_Priv = row["EFR_CHG_PRIV"].ToString();
                //Del_Priv = row["EFR_DEL_PRIV"].ToString();
                //Inq_Priv = row["EFR_INQ_PRIV"].ToString();
                //Alt_User = row["EFR_ALT_EMPLOYEE_NO"].ToString();
                //Alt_Module = row["EFR_ALT_MODULE_CODE"].ToString();
                ScrDesc = row["EFR_DESCRIPTION"].ToString().Trim();
                //LstcDate = row["EFR_DATE_LSTC"].ToString();
                //LstcOpr = row["EFR_LSTC_OPERATOR"].ToString();
                //AddDate = row["EFR_DATE_ADD"].ToString();
                //AddOpr = row["EFR_ADD_OPERATOR"].ToString();
                CaseNotesLstcDate = row["CASENOTES_DATE_LSTC"].ToString();
                CaseNotesLstcOpr = row["CASENOTES_LSTC_OPERATOR"].ToString();
                CaseNotesAddDate = row["CASENOTES_DATE_ADD"].ToString();
                CaseNotesAddOpr = row["CASENOTES_ADD_OPERATOR"].ToString();
                CaseNotesData = row["CASENOTES_DATA"].ToString();
                CaseNotesFieldName = row["CASENOTES_KEYFIELD"].ToString();
                CaseNotesScreenName = row["CASENOTES_SCREEN"].ToString();
                
                CaseNotesScreenDesc = row["AOR_DESC"].ToString();
                CaseNotesDate = row["AOR_DATE"].ToString() ;
            }
        }


        #endregion

        #region Properties

        public string UserName { get; set; }
        public string Module { get; set; }
        public string ScrCode { get; set; }
        public string Hierarchy { get; set; }
        public string Add_Priv { get; set; }
        public string Chg_Priv { get; set; }
        public string Del_Priv { get; set; }
        public string Inq_Priv { get; set; }
        public string Alt_User { get; set; }
        public string Alt_Module { get; set; }
        public string ScrDesc { get; set; }
        public string LstcDate { get; set; }
        public string LstcOpr { get; set; }
        public string AddDate { get; set; }
        public string AddOpr { get; set; }
        public string CaseWoeker { get; set; }
        public string Name { get; set; }
        public string CaseNotesFieldName { get; set; }
        public string CaseNotesData { get; set; }
        public string CaseNotesLstcDate { get; set; }
        public string CaseNotesLstcOpr { get; set; }
        public string CaseNotesAddDate { get; set; }
        public string CaseNotesAddOpr { get; set; }
        public string CaseNotesScreenName { get; set; }
        
        public string CaseNotesScreenDesc { get; set; }
        public string CaseNotesDate { get; set;}

        #endregion

    }

    public class BATCNTLEntity
    {
        #region Constructors

        public BATCNTLEntity()
        {

            UserName = string.Empty;
            ModuleCode = string.Empty;
            ScrCode = string.Empty;
            ViewPriv = string.Empty;
            RepName = string.Empty;
            DateLSTC = string.Empty;
            LSTCOperator = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
           
        }

        public BATCNTLEntity(DataRow BATCNTL)
        {
            if (BATCNTL != null)
            {
                DataRow row = BATCNTL;

                UserName = row["BAT_EMPLOYEE_NO"].ToString();
                ModuleCode = row["BAT_MODULE_CODE"].ToString();
                ScrCode = row["BAT_REPORT_CODE"].ToString();
                ViewPriv = row["BAT_VIEW_PRIV"].ToString().Equals("Y") ? "true" : "false";
                RepName = row["BAT_REPORT_NAME"].ToString();
                DateLSTC = row["BAT_DATE_LSTC"].ToString();
                LSTCOperator = row["BAT_LSTC_OPERATOR"].ToString();
                DateAdd = row["BAT_DATE_ADD"].ToString();
                AddOperator = row["BAT_ADD_OPERATOR"].ToString();
            }
        }


        #endregion

        #region Properties

        public string UserName { get; set; }
        public string ModuleCode { get; set; }
        public string ScrCode { get; set; }
        public string ViewPriv { get; set; }
        public string RepName { get; set; }
        public string DateLSTC { get; set; }
        public string LSTCOperator { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
       
        #endregion

    }

}
