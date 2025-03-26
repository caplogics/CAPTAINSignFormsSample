using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DevExpress.XtraSpreadsheet.Model;
using Org.BouncyCastle.Crypto.Tls;
using DevExpress.XtraScheduler.Native;
using Microsoft.Practices.ObjectBuilder2;
using NPOI.SS.Formula.Functions;
using NPOI.OpenXmlFormats.Wordprocessing;

namespace Captain.Common.Model.Objects
{
    public class HUDCNTLEntity
    {
        #region Constructors

        public HUDCNTLEntity()
        {
            Agy = string.Empty;
            Tax_ID = string.Empty;
            Dun_No = string.Empty;
            Street = string.Empty;
            City = string.Empty;
            State = string.Empty;
            ZIP = string.Empty;
            isMailAdd_AgyAdd = string.Empty;
            Mail_Street = string.Empty;
            Mail_City = string.Empty;
            Mail_State = string.Empty;
            Mail_ZIP = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            Trans_Agy = string.Empty;
            Trans_User = string.Empty;
            Trans_Pswrd = string.Empty;
            Trans_URL = string.Empty;
            Faith_Org = string.Empty;
            Rural_HH = string.Empty;
            Urban_HH = string.Empty;
            Coun_Methods = string.Empty;
            Ser_Col = string.Empty;
            Ser_Farms = string.Empty;
            Budget = string.Empty;
            Lang = string.Empty;

            Add_Operator = string.Empty;
            Add_Date = string.Empty;
            Lstc_Date = string.Empty;
            Lstc_Operator = string.Empty;

            RPB_YEAR = string.Empty;
            RPB_QUARTER = string.Empty;
            RPB_FRM_DTE = string.Empty;
            RPB_TO_DTE = string.Empty;
            RPB_SUBM_DTE = string.Empty;
            RPB_UPDATE_DTE = string.Empty;
            RPB_TOT_BUDGET = string.Empty;
            RPB_TOT_FUND_BUDGET = string.Empty;
        }

        public HUDCNTLEntity(DataRow row)
        {
            if (row != null)
            {
                Agy = row["HUDC_AGENCY"].ToString().Trim();

                Tax_ID = row["HUDC_TAX_ID"].ToString().Trim();
                Dun_No = row["HUDC_DUN_NO"].ToString().Trim();
                Street = row["HUDC_STREET"].ToString().Trim();
                City = row["HUDC_CITY"].ToString().Trim();
                State = row["HUDC_STATE"].ToString().Trim();
                ZIP = row["HUDC_ZIP"].ToString().Trim();

                isMailAdd_AgyAdd = row["HUDC_MAIL_ADDR"].ToString().Trim();

                Mail_Street = row["HUDC_MAIL_STREET"].ToString().Trim();
                Mail_City = row["HUDC_MAIL_CITY"].ToString().Trim();
                Mail_State = row["HUDC_MAIL_STATE"].ToString().Trim();
                Mail_ZIP = row["HUDC_MAIL_ZIP"].ToString().Trim();

                Phone = row["HUDC_PHONE"].ToString().Trim();
                Email = row["HUDC_EMAIL"].ToString().Trim();

                Trans_Agy = row["HUDC_TRANS_AGY"].ToString().Trim();
                Trans_User = row["HUDC_TRANS_USER"].ToString().Trim();
                Trans_Pswrd = row["HUDC_TRANS_PWD"].ToString().Trim();
                Trans_URL = row["HUDC_TRANS_URL"].ToString().Trim();

                Faith_Org = row["HUDC_FAITH_ORG"].ToString().Trim();
                Rural_HH = row["HUDC_RURAL_HH"].ToString().Trim();
                Urban_HH = row["HUDC_URBAN_HH"].ToString().Trim();
                Coun_Methods = row["HUDC_COUN_METHODS"].ToString().Trim();
                Ser_Col = row["HUDC_SER_COLONIAS"].ToString().Trim();
                Ser_Farms = row["HUDC_SER_FARMS"].ToString().Trim();
                Budget = row["HUDC_BUDGET"].ToString().Trim();
                Lang = row["HUDC_LANGUAGE"].ToString().Trim();

                Add_Operator = row["HUDC_ADD_OPERATOR"].ToString().Trim();
                Lstc_Operator = row["HUDC_LSTC_OPERATOR"].ToString().Trim();

                RPB_YEAR = row["HUDC_RPB_FISCAL_YEAR"].ToString().Trim();
                RPB_QUARTER = row["HUDC_RPB_QUARTER"].ToString().Trim();
                RPB_FRM_DTE = row["HUDC_RPB_FROM_DATE"].ToString().Trim();
                RPB_TO_DTE = row["HUDC_RPB_TO_DATE"].ToString().Trim();
                RPB_SUBM_DTE = row["HUDC_RPB_SUBM_DATE"].ToString().Trim();
                RPB_UPDATE_DTE = row["HUDC_RPB_UPDATE_DATE"].ToString().Trim();
                RPB_TOT_BUDGET = row["HUDC_RPB_TOT_BUDGET"].ToString().Trim();
                RPB_TOT_FUND_BUDGET = row["HUDC_RPB_TOT_FUND_BUDGET"].ToString().Trim();
            }

        }

        #endregion

        #region Properties

        public string Agy
        { get; set; }
        public string Tax_ID
        { get; set; }
        public string Dun_No
        { get; set; }
        public string Street
        { get; set; }
        public string City
        { get; set; }
        public string State
        { get; set; }
        public string ZIP
        { get; set; }
        public string isMailAdd_AgyAdd
        {
            get; set;
        }
        public string Mail_Street
        { get; set; }
        public string Mail_City
        { get; set; }
        public string Mail_State
        { get; set; }

        public string Mail_ZIP
        {
            get; set;
        }
        public string Phone
        {
            get; set;
        }
        public string Email
        {
            get; set;
        }
        public string Trans_Agy
        {
            get; set;
        }
        public string Trans_User
        {
            get; set;
        }
        public string Trans_Pswrd
        {
            get; set;
        }
        public string Trans_URL
        {
            get; set;
        }
        public string Faith_Org
        {
            get; set;
        }
        public string Rural_HH
        {
            get; set;
        }
        public string Urban_HH
        {
            get; set;
        }

        public string Coun_Methods
        {
            get; set;
        }
        public string Ser_Col
        {
            get; set;
        }
        public string Ser_Farms
        {
            get; set;
        }
        public string Budget
        {
            get; set;
        }
        public string Lang
        {
            get; set;
        }

        public string Add_Operator
        {
            get; set;
        }
        public string Add_Date
        {
            get; set;
        }
        public string Lstc_Operator
        {
            get; set;
        }
        public string Lstc_Date
        {
            get; set;
        }
        public string RPB_YEAR
        {
            get; set;
        }
        public string RPB_QUARTER
        {
            get; set;
        }

        public string RPB_FRM_DTE
        {
            get; set;
        }
        public string RPB_TO_DTE
        {
            get; set;
        }
        public string RPB_SUBM_DTE
        {
            get; set;
        }
        public string RPB_UPDATE_DTE
        {
            get; set;
        }
        public string RPB_TOT_BUDGET
        {
            get; set;
        }
        public string RPB_TOT_FUND_BUDGET
        {
            get; set;
        }

        #endregion
    }

    public class HUDSTAFFEntity
    {
        #region Constructors

        public HUDSTAFFEntity()
        {
            Agy = string.Empty;
            Staff_code = string.Empty;
            Type = string.Empty;
            Seq = string.Empty;
            Cont_type = string.Empty;
            Cont_title = string.Empty;
            SSN = string.Empty;
            Street = string.Empty;
            City = string.Empty;
            State = string.Empty;
            ZIP = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            Rate = string.Empty;
            Bill_method = string.Empty;
            FHA_ID = string.Empty;
            Active = string.Empty;
            Cert = string.Empty;
            Lang = string.Empty;
            Service = string.Empty;
          
            Add_Operator = string.Empty;
            Add_Date = string.Empty;
            Lstc_Date = string.Empty;
            Lstc_Operator = string.Empty;

        }

        public HUDSTAFFEntity(DataRow row)
        {
            if (row != null)
            {
                Agy = row["HUDS_AGENCY"].ToString().Trim();
                Staff_code = row["HUDS_STF_CODE"].ToString().Trim();
                Type = row["HUDS_TYPE"].ToString().Trim();
                Seq = row["HUDS_SEQ"].ToString().Trim();

                Cont_type = row["HUDS_CONT_TYPE"].ToString().Trim();
                Cont_title = row["HUDS_CONT_TITLE"].ToString().Trim();
                SSN = row["HUDS_SSN"].ToString().Trim();

                Street = row["HUDS_WSTREET"].ToString().Trim();
                City = row["HUDS_WCITY"].ToString().Trim();
                State = row["HUDS_WSTATE"].ToString().Trim();
                ZIP = row["HUDS_WZIP"].ToString().Trim();
                Phone = row["HUDS_WPHONE"].ToString().Trim();
                Email = row["HUDS_WMAIL"].ToString().Trim();

                Rate = row["HUDS_RATE"].ToString().Trim();
                Bill_method = row["HUDS_BILL_METHOD"].ToString().Trim();
                FHA_ID = row["HUDS_FHA_ID"].ToString().Trim();
                Active = row["HUDS_ACTIVE"].ToString().Trim();
                Cert = row["HUDS_CERT"].ToString().Trim();
                Lang = row["HUDS_LANG"].ToString().Trim();
                Service = row["HUDS_SERVICE"].ToString().Trim();

                Add_Operator = row["HUDS_ADD_OPERATOR"].ToString().Trim();
                Lstc_Operator = row["HUDS_LSTC_OPERATOR"].ToString().Trim();

            }

        }

        #endregion

        #region Properties

        public string Agy
        {
            get; set;
        }
        public string Staff_code
        {
            get; set;
        }
        public string Type
        {
            get; set;
        }
        public string Seq
        {
            get; set;
        }
        public string Cont_type
        {
            get; set;
        }
        public string Cont_title
        {
            get; set;
        }
        public string SSN
        {
            get; set;
        }
        public string Street
        {
            get; set;
        }
        public string City
        {
            get; set;
        }
        public string State
        {
            get; set;
        }
        public string ZIP
        {
            get; set;
        }
        public string Phone
        {
            get; set;
        }
        public string Email
        {
            get; set;
        }
        public string Rate
        {
            get; set;
        }
        public string Bill_method
        {
            get; set;
        }
        public string FHA_ID
        {
            get; set;
        }
        public string Active
        {
            get; set;
        }
        public string Cert
        {
            get; set;
        }
        public string Lang
        {
            get; set;
        }
        public string Service
        {
            get; set;
        }

        public string Add_Operator
        {
            get; set;
        }
        public string Add_Date
        {
            get; set;
        }
        public string Lstc_Operator
        {
            get; set;
        }
        public string Lstc_Date
        {
            get; set;
        }

        #endregion
    }

    public class HUDTRAINEntity
    {
        #region Constructors

        public HUDTRAINEntity()
        {
            Agency = string.Empty;
            Seq = string.Empty;
            Train_Title = string.Empty;
            Train_Date = string.Empty;
            Duration = string.Empty;
            Organiz = string.Empty;
            Organiz_Other = string.Empty;
            Sponsor = string.Empty;
            Sponsor_Other = string.Empty;
            
            Add_Operator = string.Empty;
            Add_Date = string.Empty;
            Lstc_Date = string.Empty;
            Lstc_Operator = string.Empty;
            CounsellorCnt = string.Empty;

            //CounsellorCnt= row["CNT"].ToString().Trim();
        }

        public HUDTRAINEntity(DataRow row)
        {
            if (row != null)
            {
                Agency = row["HUDT_AGENCY"].ToString().Trim();
                Seq = row["HUDT_SEQ"].ToString().Trim();

                Train_Title = row["HUDT_TRTITLE"].ToString().Trim();
                Train_Date = row["HUDT_TDATE"].ToString().Trim();
                Duration = row["HUDT_DURATION"].ToString().Trim();
                Organiz = row["HUDT_ORGANIZATION"].ToString().Trim();
                Organiz_Other = row["HUDT_ORG_OTHER"].ToString().Trim();
                Sponsor = row["HUDT_SPONSOR"].ToString().Trim();
                Sponsor_Other = row["HUDT_SPONSOR_OTHER"].ToString().Trim();
               
                Add_Operator = row["HUDT_ADD_OPERATOR"].ToString().Trim();
                Lstc_Operator = row["HUDT_LSTC_OPERATOR"].ToString().Trim();
            }
        }

        public HUDTRAINEntity(DataRow row,string BrowseType)
        {
            if (row != null)
            {
                Agency = row["HUDT_AGENCY"].ToString().Trim();
                Seq = row["HUDT_SEQ"].ToString().Trim();

                Train_Title = row["HUDT_TRTITLE"].ToString().Trim();
                Train_Date = row["HUDT_TDATE"].ToString().Trim();
                Duration = row["HUDT_DURATION"].ToString().Trim();
                Organiz = row["HUDT_ORGANIZATION"].ToString().Trim();
                Organiz_Other = row["HUDT_ORG_OTHER"].ToString().Trim();
                Sponsor = row["HUDT_SPONSOR"].ToString().Trim();
                Sponsor_Other = row["HUDT_SPONSOR_OTHER"].ToString().Trim();

                Add_Operator = row["HUDT_ADD_OPERATOR"].ToString().Trim();
                Lstc_Operator = row["HUDT_LSTC_OPERATOR"].ToString().Trim();

                Add_Date = row["HUDT_DATE_ADD"].ToString().Trim();
                Lstc_Date = row["HUDT_DATE_LSTC"].ToString().Trim();

                CounsellorCnt = row["CNT"].ToString().Trim();
            }
        }

        #endregion

        #region Properties

        public string Agency
        {
            get; set;
        }
        public string Seq
        {
            get; set;
        }
        public string Train_Title
        {
            get; set;
        }
        public string Train_Date
        {
            get; set;
        }
        public string Duration
        {
            get; set;
        }
        public string Organiz
        {
            get; set;
        }
        public string Organiz_Other
        {
            get; set;
        }
        public string Sponsor
        {
            get; set;
        }
        public string Sponsor_Other
        {
            get; set;
        }
        public string Add_Operator
        {
            get; set;
        }
        public string Add_Date
        {
            get; set;
        }
        public string Lstc_Operator
        {
            get; set;
        }
        public string Lstc_Date
        {
            get; set;
        }

        public string CounsellorCnt
        {
            get; set;
        }

        #endregion
    }

    public class HUDHTCATTNEntity
    {
        #region Constructors

        public HUDHTCATTNEntity()
        {
            Agency = string.Empty;
            Seq = string.Empty;
            
            Staff_Code = string.Empty;
            Certificate = string.Empty;

            Add_Operator = string.Empty;
            Add_Date = string.Empty;
            Lstc_Date = string.Empty;
            Lstc_Operator = string.Empty;
            

        }

        public HUDHTCATTNEntity(DataRow row)
        {
            if (row != null)
            {
                Agency = row["HTC_AGENCY"].ToString().Trim();
                Seq = row["HTC_HUD_SEQ"].ToString().Trim();

                Staff_Code = row["HTC_STAFF_CODE"].ToString().Trim();
                Certificate = row["HTC_CERTIFICATE"].ToString().Trim();

                Add_Operator = row["HTC_ADD_OPERATOR"].ToString().Trim();
                Lstc_Operator = row["HTC_LSTC_OPERATOR"].ToString().Trim();
            }
        }

        #endregion

        #region Properties

        public string Agency
        {
            get; set;
        }
        public string Seq
        {
            get; set;
        }
        public string Staff_Code
        {
            get; set;
        }
        public string Certificate
        {
            get; set;
        }
        public string Add_Operator
        {
            get; set;
        }
        public string Add_Date
        {
            get; set;
        }
        public string Lstc_Operator
        {
            get; set;
        }
        public string Lstc_Date
        {
            get; set;
        }

        

        #endregion
    }

    public class HUDGROUPEntity
    {
        #region Constructors

        public HUDGROUPEntity()
        {
            Agency = string.Empty;
            Seq = string.Empty;

            Sess_Title = string.Empty;
            Ses_Date = string.Empty;
            Type = string.Empty;
            Duration = string.Empty;
            Attribute = string.Empty;
            Fund = string.Empty;
            Staff_Code = string.Empty;

            Add_Operator = string.Empty;
            Add_Date = string.Empty;
            Lstc_Date = string.Empty;
            Lstc_Operator = string.Empty;


        }

        public HUDGROUPEntity(DataRow row)
        {
            if (row != null)
            {
                Agency = row["HUDG_AGENCY"].ToString().Trim();
                Seq = row["HUDG_SEQ"].ToString().Trim();

                Sess_Title = row["HUDG_SES_TITLE"].ToString().Trim();
                Ses_Date = row["HUDG_SES_DATE"].ToString().Trim();
                Type = row["HUDG_TYPE"].ToString().Trim();
                Duration = row["HUDG_DURATION"].ToString().Trim();
                Attribute = row["HUDG_ATTRIBUTE"].ToString().Trim();
                Fund = row["HUDG_FUND"].ToString().Trim();
                Staff_Code = row["HUDG_STAFF_CODE"].ToString().Trim();

                Add_Operator = row["HUDG_ADD_OPERATOR"].ToString().Trim();
                Add_Date = row["HUDG_DATE_ADD"].ToString().Trim();
                Lstc_Operator = row["HUDG_LSTC_OPERATOR"].ToString().Trim();
                Lstc_Date = row["HUDG_DATE_LSTC"].ToString().Trim();
            }
        }

        #endregion

        #region Properties

        public string Agency
        {
            get; set;
        }
        public string Seq
        {
            get; set;
        }
        public string Sess_Title
        {
            get; set;
        }
        public string Ses_Date
        {
            get; set;
        }
        public string Type
        {
            get; set;
        }
        public string Duration
        {
            get; set;
        }
        public string Attribute
        {
            get; set;
        }
        public string Fund
        {
            get; set;
        }
        public string Staff_Code
        {
            get; set;
        }
        public string Add_Operator
        {
            get; set;
        }
        public string Add_Date
        {
            get; set;
        }
        public string Lstc_Operator
        {
            get; set;
        }
        public string Lstc_Date
        {
            get; set;
        }



        #endregion
    }

    public class HUDMSTENTITY
    {
        #region Constructors

        public HUDMSTENTITY()
        {
            Agency = null;

            Dept = null;

            Prog = null;

            Year = null;

            AppNo = null;

            Seq = null;

            Date = null;

            Status = null;

            Add_Date = null;

            Add_Operator = null;

            Lsctc_Date = null;

            Lstc_Operator = null;

        }

        public HUDMSTENTITY(DataRow row)
        {
            if (row != null)
            {
                Agency = row["HUDMST_AGY"].ToString().Trim();

                Dept = row["HUDMST_DEPT"].ToString().Trim();

                Prog = row["HUDMST_PROG"].ToString().Trim();

                Year = row["HUDMST_YEAR"].ToString().Trim();

                AppNo = row["HUDMST_APPNO"].ToString().Trim();

                Seq = row["HUDMST_SEQ"].ToString().Trim();

                Date = row["HUDMST_DATE"].ToString().Trim();

                Status = row["HUDMST_STATUS"].ToString().Trim();
                
                Add_Date = row["HUDMST_ADD_DATE"].ToString().Trim();

                Add_Operator = row["HUDMST_ADD_OPERATOR"].ToString().Trim();

                Lsctc_Date = row["HUDMST_LSTC_DATE"].ToString().Trim();

                Lstc_Operator = row["HUDMST_LSTC_OPERATOR"].ToString().Trim();
            }
        }

        #endregion

        #region Properties

        public string Agency
        {
            get; set;
        }
        public string Dept
        {
            get; set;
        }
        public string Prog
        {
            get; set;
        }
        public string Year
        {
            get; set;
        }
        public string AppNo
        {
            get; set;
        }
        public string Seq
        {
            get; set;
        }
       
        public string Date
        {
            get; set;
        }
        public string Status
        {
            get; set;
        }
        public string Add_Date
        {
            get; set;
        }
        public string Add_Operator
        {
            get; set;
        }
        public string Lsctc_Date
        {
            get; set;
        }
        public string Lstc_Operator
        {
            get; set;
        }

        #endregion

    }

    public class HUDINDIVENTITY
    {
        #region Constructors

        public HUDINDIVENTITY()
        {
            Agency = null;

            Dept = null;

            Prog = null;

            Year = null;

            AppNo = null;

            Seq = null;

            MST_Seq = null;

            Sess_Date = null;

            Sess_Client_Paid = null;

            Sess_Counselor = null;

            Sess_Start_Time = null;

            Sess_End_Time = null;

            Sess_Status = null;

            Sess_Type = null;

            Sess_Pur_Visit = null;

            Sess_Act_Type = null;

            Sess_Attr_Grant = null;

            Sess_Grant_Used = null;

            Sess_Fund = null;

            CD_HH_Month_Inc = null;

            CD_Inc_Level = null;

            CD_App_Race = null;

            CD_App_Ethnicity = null;

            CD_Lang = null;

            CD_Ref_Source = null;

            CD_Ref_Area_Status = null;

            CD_Eng_Prof_Status = null;

            CD_If_Applies = null;


            CFH_Depend = null;

            CFH_HH_Liab = null;

            CFH_Cred_Score = null;

            CFH_Source = null;

            CFH_Cred_Score_Reason = null;

            CFH_Job_Length = null;

            CFH_OTR_Serv_Award = null;


            LI_Bef_Loan_Type = null;

            LI_Bef_Loan_Reported = null;

            LI_Bef_Mortgage_Type = null;

            LI_Bef_Finance_Type = null;

            LI_Bef_If_Applies = null;

            LI_Bef_Is_Cert_HUD = null;

            LI_Bef_HECM_ID = null;

            LI_Bef_Cert_Issued = null;

            LI_Bef_Cert_Expires = null;

            LI_Aft_Loan_Spouse = null;

            LI_Aft_Mortgage_Type = null;


            LI_Aft_Finance_Type = null;

            LI_Aft_Interest_Rate = null;

            LI_Aft_Close_Cost = null;


            LI_Aft_Street = null;

            LI_Aft_Unit = null;

            LI_Aft_City = null;

            LI_Aft_State = null;

            LI_Aft_ZIP = null;

            Add_Date = null;

            Add_Operator = null;

            Lsctc_Date = null;

            Lstc_Operator = null;

        }

        public HUDINDIVENTITY(DataRow row)
        {
            if (row != null)
            {
                Agency = row["HUDIND_AGY"].ToString().Trim();

                Dept = row["HUDIND_DEPT"].ToString().Trim();

                Prog = row["HUDIND_PROG"].ToString().Trim();

                Year = row["HUDIND_YEAR"].ToString().Trim();

                AppNo = row["HUDIND_APPNO"].ToString().Trim();

                Seq = row["HUDIND_SEQ"].ToString().Trim();

                MST_Seq = row["HUDMST_SEQ"].ToString().Trim();

                Sess_Date = row["HUDIND_SESS_DATE"].ToString().Trim();

                Sess_Client_Paid = row["HUDIND_SESS_CLIENT_PAID"].ToString().Trim();

                Sess_Counselor = row["HUDIND_SESS_COUNS"].ToString().Trim();

                Sess_Start_Time = row["HUDIND_SESS_START_TIME"].ToString().Trim();

                Sess_End_Time = row["HUDIND_SESS_END_TIME"].ToString().Trim();

                Sess_Status = row["HUDIND_SESS_STATUS"].ToString().Trim();

                Sess_Type = row["HUDIND_SESS_TYPE"].ToString().Trim();

                Sess_Pur_Visit = row["HUDIND_SESS_PUR_VISIT"].ToString().Trim();

                Sess_Act_Type = row["HUDIND_SESS_ACT_TYPE"].ToString().Trim();

                Sess_Attr_Grant = row["HUDIND_SESS_ATTR_GRANT"].ToString().Trim();

                Sess_Grant_Used = row["HUDIND_SESS_GRANT_USED"].ToString().Trim();

                Sess_Fund = row["HUDIND_SESS_FUND"].ToString().Trim();

                CD_HH_Month_Inc = row["HUDIND_CD_HH_MNT_INC"].ToString().Trim();

                CD_Inc_Level = row["HUDIND_CD_INC_LEVEL"].ToString().Trim();

                CD_App_Race = row["HUDIND_CD_APP_RACE"].ToString().Trim();

                CD_App_Ethnicity = row["HUDIND_CD_APP_ETHNICITY"].ToString().Trim();

                CD_Lang = row["HUDIND_CD_LANG"].ToString().Trim();

                CD_Ref_Source = row["HUDIND_CD_REF_SRC"].ToString().Trim();

                CD_Ref_Area_Status = row["HUDIND_CD_REF_AREA_STAT"].ToString().Trim();

                CD_Eng_Prof_Status = row["HUDIND_CD_ENG_PROF_STAT"].ToString().Trim();

                CD_If_Applies = row["HUDIND_CD_IF_APPLIES"].ToString().Trim();


                CFH_Depend = row["HUDIND_CFH_DEPEND"].ToString().Trim();

                CFH_HH_Liab = row["HUDIND_CFH_HH_LIAB"].ToString().Trim();

                CFH_Cred_Score = row["HUDIND_CFH_CRED_SCOR"].ToString().Trim();

                CFH_Source = row["HUDIND_CFH_SRC"].ToString().Trim();

                CFH_Cred_Score_Reason = row["HUDIND_CFH_CRED_SCOR_REASON"].ToString().Trim();

                CFH_Job_Length = row["HUDIND_CFH_JOB_LEN"].ToString().Trim();

                CFH_OTR_Serv_Award = row["HUDIND_CFH_OTH_SERV_AWARD"].ToString().Trim();


                LI_Bef_Loan_Type = row["HUDIND_LI_BEF_LOAN_TYPE"].ToString().Trim();

                LI_Bef_Loan_Reported = row["HUDIND_LI_BEF_LOAN_RPT"].ToString().Trim();

                LI_Bef_Mortgage_Type = row["HUDIND_LI_BEF_MORT_TYPE"].ToString().Trim();

                LI_Bef_Finance_Type = row["HUDIND_LI_BEF_FINC_TYPE"].ToString().Trim();

                LI_Bef_If_Applies = row["HUDIND_LI_BEF_IF_APPLIES"].ToString().Trim();

                LI_Bef_Is_Cert_HUD = row["HUDIND_LI_BEF_IS_CERT_HUD"].ToString().Trim();

                LI_Bef_HECM_ID = row["HUDIND_LI_BEF_CERT_HECM_CERTID"].ToString().Trim();

                LI_Bef_Cert_Issued = row["HUDIND_LI_BEF_CERT_ISSUED"].ToString().Trim();

                LI_Bef_Cert_Expires = row["HUDIND_LI_BEF_CERT_EXPIRES"].ToString().Trim();

                LI_Aft_Loan_Spouse = row["HUDIND_LI_AFT_LOAN_SPOUCE"].ToString().Trim();

                LI_Aft_Mortgage_Type = row["HUDIND_LI_AFT_MORT_TYPE"].ToString().Trim();

                LI_Aft_Finance_Type = row["HUDIND_LI_AFT_FINC_TYPE"].ToString().Trim();

                LI_Aft_Interest_Rate = row["HUDIND_LI_AFT_INRST_RATE"].ToString().Trim();

                LI_Aft_Close_Cost = row["HUDIND_LI_AFT_CLOS_COST"].ToString().Trim();

                LI_Aft_Street = row["HUDIND_LI_AFT_STREET"].ToString().Trim();

                LI_Aft_Unit = row["HUDIND_LI_AFT_UNIT"].ToString().Trim();

                LI_Aft_City = row["HUDIND_LI_AFT_CITY"].ToString().Trim();

                LI_Aft_State = row["HUDIND_LI_AFT_STATE"].ToString().Trim();

                LI_Aft_ZIP = row["HUDIND_LI_AFT_ZIP"].ToString().Trim();

                Add_Date = row["HUDIND_ADD_DATE"].ToString().Trim();

                Add_Operator = row["HUDIND_ADD_OPERATOR"].ToString().Trim();

                Lsctc_Date = row["HUDIND_LSTC_DATE"].ToString().Trim();

                Lstc_Operator = row["HUDIND_LSTC_OPERATOR"].ToString().Trim();
            }
        }

        #endregion

        #region Properties

        public string Agency
        {
            get; set;
        }
        public string Dept
        {
            get; set;
        }
        public string Prog
        {
            get; set;
        }
        public string Year
        {
            get; set;
        }
        public string AppNo
        {
            get; set;
        }
        public string Seq
        {
            get; set;
        }
        public string MST_Seq
        {
            get; set;
        }
        public string Sess_Date
        {
            get; set;
        }
        public string Sess_Client_Paid
        {
            get; set;
        }
        public string Sess_Counselor
        {
            get; set;
        }
        public string Sess_Start_Time
        {
            get; set;
        }
        public string Sess_End_Time
        {
            get; set;
        }
        public string Sess_Status
        {
            get; set;
        }
        public string Sess_Type
        {
            get; set;
        }
        public string Sess_Pur_Visit
        {
            get; set;
        }
        public string Sess_Act_Type
        {
            get; set;
        }
        public string Sess_Attr_Grant
        {
            get; set;
        }

        public string Sess_Grant_Used
        {
            get; set;
        }
        public string Sess_Fund
        {
            get; set;
        }



        public string CD_HH_Month_Inc
        {
            get; set;
        }
        public string CD_Inc_Level
        {
            get; set;
        }
        public string CD_App_Race
        {
            get; set;
        }
        public string CD_App_Ethnicity
        {
            get; set;
        }
        public string CD_Lang
        {
            get; set;
        }
        public string CD_Ref_Source
        {
            get; set;
        }
        public string CD_Ref_Area_Status
        {
            get; set;
        }
        public string CD_Eng_Prof_Status
        {
            get; set;
        }
        public string CD_If_Applies
        {
            get; set;
        }


        public string CFH_Depend
        {
            get; set;
        }
        public string CFH_HH_Liab
        {
            get; set;
        }
        public string CFH_Cred_Score
        {
            get; set;
        }
        public string CFH_Source
        {
            get; set;
        }
        public string CFH_Cred_Score_Reason
        {
            get; set;
        }
        public string CFH_Job_Length
        {
            get; set;
        }
        public string CFH_OTR_Serv_Award
        {
            get; set;
        }


        public string LI_Bef_Loan_Type
        {
            get; set;
        }
        public string LI_Bef_Loan_Reported
        {
            get; set;
        }
        public string LI_Bef_Mortgage_Type
        {
            get; set;
        }
        public string LI_Bef_Finance_Type
        {
            get; set;
        }
        public string LI_Bef_If_Applies
        {
            get; set;
        }
        public string LI_Bef_Is_Cert_HUD
        {
            get; set;
        }
        public string LI_Bef_HECM_ID
        {
            get; set;
        }
        public string LI_Bef_Cert_Issued
        {
            get; set;
        }
        public string LI_Bef_Cert_Expires
        {
            get; set;

        }
        public string LI_Aft_Loan_Spouse
        {
            get; set;
        }
        public string LI_Aft_Mortgage_Type
        {
            get; set;
        }

        public string LI_Aft_Finance_Type
        {
            get; set;
        }
        public string LI_Aft_Interest_Rate
        {
            get; set;
        }
        public string LI_Aft_Close_Cost
        {
            get; set;
        }

        public string LI_Aft_Street
        {
            get; set;
        }
        public string LI_Aft_Unit
        {
            get; set;
        }
        public string LI_Aft_City
        {
            get; set;
        }
        public string LI_Aft_State
        {
            get; set;
        }
        public string LI_Aft_ZIP
        {
            get; set;
        }
        public string Add_Date
        {
            get; set;
        }
        public string Add_Operator
        {
            get; set;
        }
        public string Lsctc_Date
        {
            get; set;
        }
        public string Lstc_Operator
        {
            get; set;
        }

        #endregion

    }

    public class HUDIMPACTENTITY
    {
        #region Constructors

        public HUDIMPACTENTITY()
        {
            Agency = null;

            Dept = null;

            Prog = null;

            Year = null;

            AppNo = null;

            Seq = null;

            Indv_Seq = null;

            Mst_Seq = null;

            Impact_Date = null;

            Impacts = null;


            Add_Date = null;

            Add_Operator = null;

            Lsctc_Date = null;

            Lstc_Operator = null;

        }

        public HUDIMPACTENTITY(DataRow row)
        {
            if (row != null)
            {
                Agency = row["HUDIMPACT_AGY"].ToString().Trim();

                Dept = row["HUDIMPACT_DEPT"].ToString().Trim();

                Prog = row["HUDIMPACT_PROG"].ToString().Trim();

                Year = row["HUDIMPACT_YEAR"].ToString().Trim();

                AppNo = row["HUDIMPACT_APPNO"].ToString().Trim();

                Seq = row["HUDIMPACT_SEQ"].ToString().Trim();

                Indv_Seq = row["HUDIND_SEQ"].ToString().Trim();

                Mst_Seq = row["HUDMST_SEQ"].ToString().Trim();

                Impact_Date = row["HUDIMPACT_DATE"].ToString().Trim();

                Impacts = row["HUDIMPACT_IMPACTS"].ToString().Trim();


                Add_Date = row["HUDIMPACT_ADD_DATE"].ToString().Trim();

                Add_Operator = row["HUDIMPACT_ADD_OPERATOR"].ToString().Trim();

                Lsctc_Date = row["HUDIMPACT_LSTC_DATE"].ToString().Trim();

                Lstc_Operator = row["HUDIMPACT_LSTC_OPERATOR"].ToString().Trim();
            }
        }

        #endregion

        #region Properties

        public string Agency
        {
            get; set;
        }
        public string Dept
        {
            get; set;
        }
        public string Prog
        {
            get; set;
        }
        public string Year
        {
            get; set;
        }
        public string AppNo
        {
            get; set;
        }
        public string Seq
        {
            get; set;
        }
        public string Indv_Seq
        {
            get; set;
        }
        public string Mst_Seq
        {
            get; set;
        }
        public string Impact_Date
        {
            get; set;
        }
        public string Impacts
        {
            get; set;
        }
        public string Add_Date
        {
            get; set;
        }
        public string Add_Operator
        {
            get; set;
        }
        public string Lsctc_Date
        {
            get; set;
        }
        public string Lstc_Operator
        {
            get; set;
        }

        #endregion

    }

    //Added by Vikash on 11/29/2024 for getting counts to HUD 9902 report
    public class HUDCOUNTSENTITY
    {
        #region Constructors

        public HUDCOUNTSENTITY()
        {
            Agency = null;

            Dept = null;

            Prog = null;

            Year = null;

            FromDate = null;

            ToDate = null;

        }

        public HUDCOUNTSENTITY(DataRow row)
        {
            if (row != null)
            {
                Agency = row["AGENCY"].ToString().Trim();

                Dept = row["DEPT"].ToString().Trim();

                Prog = row["PROG"].ToString().Trim();

                Year = row["YEAR"].ToString().Trim();

                FromDate = row["FROM_DATE"].ToString().Trim();

                ToDate = row["TO_DATE"].ToString().Trim();
            }
        }

        #endregion

        #region Properties

        public string Agency
        {
            get; set;
        }
        public string Dept
        {
            get; set;
        }
        public string Prog
        {
            get; set;
        }
        public string Year
        {
            get; set;
        }
        
        public string FromDate
        {
            get; set;
        }
        public string ToDate
        {
            get; set;
        }

        #endregion

    }
}
