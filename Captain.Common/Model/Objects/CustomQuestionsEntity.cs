using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Utilities;
using System.Web.UI.WebControls;
using System.Data;

namespace Captain.Common.Model.Objects
{
    public class CustomQuestionsEntity
    {
        #region Constructors

        public CustomQuestionsEntity()
        {
            CUSTSCRCODE = string.Empty;
            CUSTCODE = string.Empty;
            CUSTDESC = string.Empty;
            SUBSCR = string.Empty;
            CUSTRESPTYPE = string.Empty;
            CUSTMEMACCESS = string.Empty;
            CUSTEQUAL = string.Empty;
            CUSTGREATER = string.Empty;
            CUSTLESSTHAN = string.Empty;
            CUSTALPHA = string.Empty;
            CUSTOTHER = string.Empty;
            CUSTABBRQUESTION = string.Empty;
            CUSTCALLOWFDATE = string.Empty;

            ACTAGENCY = string.Empty;
            ACTDEPT = string.Empty;
            ACTPROGRAM = string.Empty;
            ACTYEAR = string.Empty;
            ACTAPPNO = string.Empty;
            ACTSNPFAMILYSEQ = string.Empty;
            ACTCODE = string.Empty;
            ACTRESPSEQ = string.Empty;
            ACTNUMRESP = string.Empty;
            ACTALPHARESP = string.Empty;
            ACTDATERESP = string.Empty;
            ACTMULTRESP = string.Empty;

            Mode = string.Empty;
            lstcdate = string.Empty;
            lstcoperator = string.Empty;
            adddate = string.Empty;
            addoperator = string.Empty;
            SER_ELEC = string.Empty;
            SER_KWH = string.Empty;
            SER_GAS = string.Empty;
            SER_CCF = string.Empty;
            SER_ANNUAL = string.Empty;
            USAGE_PRIM_12HIST = string.Empty;
            USAGE_SEC_12HIST = string.Empty;
            USAGE_MONTH = string.Empty;
            USAGE_PMON_SW = string.Empty;
            USAGE_SMON_SW = string.Empty;
            USAGE_PSOURCE = string.Empty;
            USAGE_SSOURCE = string.Empty;
            USAGE_PRIM_VEND = string.Empty;
            USAGE_SEC_VEND = string.Empty;
            Dwellingtypecode = string.Empty;
            USAGE_VENDORNAME1 = string.Empty;
            USAGE_VENDORNAME2 = string.Empty;
            LetterDate = string.Empty;
            INTKWorker = string.Empty;
            USAGE_PRIM_PAYMENT = string.Empty;
            USAGE_PRIM_USAGE = string.Empty;
            USAGE_SEC_PAYMENT = string.Empty;
            USAGE_SEC_USAGE = string.Empty;
            USAGE_TOTAL = string.Empty;

            USAGE_LUMP_PRIM = string.Empty;
            USAGE_LUMP_SEC = string.Empty;
            USAGE_PRIM_ACTID = string.Empty;
            USAGE_SEC_ACTID = string.Empty;

        }

        public CustomQuestionsEntity(bool Intialize)
        {
            if (Intialize)
            {
                ACTAGENCY = null;
                ACTDEPT = null;
                ACTPROGRAM = null;
                ACTYEAR = null;
                ACTAPPNO = null;
                ACTSNPFAMILYSEQ = null;
                ACTCODE = null;
                ACTRESPSEQ = null;
                ACTNUMRESP = null;
                ACTALPHARESP = null;
                ACTDATERESP = null;
                ACTMULTRESP = null;

                Mode = null;
                lstcdate = null;
                lstcoperator = null;
                adddate = null;
                addoperator = null;
                LetterDate = null;
                INTKWorker = null;
            }
        }

        public CustomQuestionsEntity(DataRow AgyTabControl)
        {
            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;

                CUSTSCRCODE = row["CUST_SCR_CODE"].ToString();
                CUSTCODE = row["CUST_CODE"].ToString();
                CUSTDESC = row["CUST_DESC"].ToString();
                CUSTSEQ = row["CUST_SEQ"].ToString();
                SUBSCR = row["CUST_SUB_SCR"].ToString();
                CUSTRESPTYPE = row["CUST_RESP_TYPE"].ToString();
                CUSTMEMACCESS = row["CUST_MEM_ACCESS"].ToString();
                CUSTEQUAL = row["CUST_EQUAL"].ToString();
                CUSTGREATER = row["CUST_GREATER"].ToString();
                CUSTLESSTHAN = row["CUST_LESSTHAN"].ToString();
                CUSTALPHA = row["CUST_ALPHA"].ToString();
                CUSTOTHER = row["CUST_OTHER"].ToString();
                CUSTABBRQUESTION = row["CUST_ABBR_QUESTION"].ToString();
                CUSTCALLOWFDATE = row["CUST_ALLOW_FDATE"].ToString();
                CUSTACTIVE = row["FLDH_ACTIVE"].ToString();
                CUSTACTIVECUST = row["CUST_ACTIVE"].ToString();
                CUSTREQUIRED = row["FLDH_REQUIRED"].ToString();
                lstcdate = row["CUST_DATE_LSTC"].ToString();
                lstcoperator = row["CUST_LSTC_OPERATOR"].ToString();
                adddate = row["CUST_DATE_ADD"].ToString();
                addoperator = row["CUST_ADD_OPERATOR"].ToString();
            }

        }

        public CustomQuestionsEntity(DataRow CustResponse, string Type)
        {
            if (CustResponse != null)
            {
                DataRow row = CustResponse;
                Dwellingtypecode = string.Empty;
                USAGE_VENDORNAME1 = string.Empty;
                USAGE_VENDORNAME2 = string.Empty;
                if (Type == "case2530Report")
                {
                    ACTAGENCY = row["ACT_AGENCY"].ToString().Trim();
                    ACTDEPT = row["ACT_DEPT"].ToString().Trim();
                    ACTPROGRAM = row["ACT_PROGRAM"].ToString().Trim();
                    ACTYEAR = row["ACT_YEAR"].ToString().Trim();
                    ACTAPPNO = row["ACT_APP_NO"].ToString().Trim();
                    //  ACTSNPFAMILYSEQ = row["ACT_SNP_FAMILY_SEQ"].ToString().Trim();
                    ACTCODE = row["ACT_CODE"].ToString().Trim();
                    //ACTRESPSEQ = row["ACT_RESP_SEQ"].ToString().Trim();
                    ACTNUMRESP = row["ACT_NUM_RESP"].ToString().Trim();
                    ACTALPHARESP = row["ACT_ALPHA_RESP"].ToString().Trim();
                    ACTDATERESP = row["ACT_DATE_RESP"].ToString().Trim();
                    ACTMULTRESP = row["ACT_MULT_RESP"].ToString().Trim();
                    // lstcdate = row["ACT_DATE_LSTC"].ToString().Trim();
                    // lstcoperator = row["ACT_LSTC_OPERATOR"].ToString().Trim();
                    // adddate = row["ACT_DATE_ADD"].ToString().Trim();
                    // addoperator = row["ACT_ADD_OPERATOR"].ToString().Trim();
                }
                else if (Type == "Pres2530Report")
                {
                    ACTAGENCY = row["PRES_AGENCY"].ToString().Trim();
                    ACTDEPT = row["PRES_DEPT"].ToString().Trim();
                    ACTPROGRAM = row["PRES_PROGRAM"].ToString().Trim();
                    ACTYEAR = row["PRES_YEAR"].ToString().Trim();
                    ACTAPPNO = row["PRES_APP_NO"].ToString().Trim();
                    ACTCODE = row["PRES_CODE"].ToString().Trim();
                    ACTNUMRESP = row["PRES_NUM_RESP"].ToString().Trim();
                    ACTALPHARESP = row["PRES_ALPHA_RESP"].ToString().Trim();
                    ACTDATERESP = row["PRES_DATE_RESP"].ToString().Trim();
                    ACTMULTRESP = row["PRES_MULT_RESP"].ToString().Trim();

                }
                else if (Type == "PREASSES")
                {
                    ACTAGENCY = row["PRES_AGENCY"].ToString().Trim();
                    ACTDEPT = row["PRES_DEPT"].ToString().Trim();
                    ACTPROGRAM = row["PRES_PROGRAM"].ToString().Trim();
                    ACTYEAR = row["PRES_YEAR"].ToString().Trim();
                    ACTAPPNO = row["PRES_APP_NO"].ToString().Trim();
                    ACTSNPFAMILYSEQ = row["PRES_SNP_FAMILY_SEQ"].ToString().Trim();
                    ACTCODE = row["PRES_CODE"].ToString().Trim();
                    ACTRESPSEQ = row["PRES_RESP_SEQ"].ToString().Trim();
                    ACTNUMRESP = row["PRES_NUM_RESP"].ToString().Trim();
                    ACTALPHARESP = row["PRES_ALPHA_RESP"].ToString().Trim();
                    ACTDATERESP = row["PRES_DATE_RESP"].ToString().Trim();
                    ACTMULTRESP = row["PRES_MULT_RESP"].ToString().Trim();
                    lstcdate = row["PRES_DATE_LSTC"].ToString().Trim();
                    lstcoperator = row["PRES_LSTC_OPERATOR"].ToString().Trim();
                    adddate = row["PRES_DATE_ADD"].ToString().Trim();
                    addoperator = row["PRES_ADD_OPERATOR"].ToString().Trim();
                    PRESPOINTS = row["PRES_POINTS"].ToString().Trim();

                }
                else if (Type == "DIMENSION")
                {
                    ACTAGENCY = row["DIMSCOR_AGENCY"].ToString().Trim();
                    ACTDEPT = row["DIMSCOR_DEPT"].ToString().Trim();
                    ACTPROGRAM = row["DIMSCOR_PROGRAM"].ToString().Trim();
                    ACTYEAR = row["DIMSCOR_YEAR"].ToString().Trim();
                    ACTAPPNO = row["DIMSCOR_APP_NO"].ToString().Trim();
                    ACTCODE = row["DIMSCOR_CODE"].ToString().Trim();
                    lstcdate = row["DIMSCOR_DATE_LSTC"].ToString().Trim();
                    lstcoperator = row["DIMSCOR_LSTC_OPERATOR"].ToString().Trim();
                    adddate = row["DIMSCOR_DATE_ADD"].ToString().Trim();
                    addoperator = row["DIMSCOR_ADD_OPERATOR"].ToString().Trim();
                    PRESPOINTS = row["DIMSCOR_SCORE"].ToString().Trim();

                }
                else if (Type == "PREQUES")
                {
                    ACTAGENCY = row["PRES_AGENCY"].ToString().Trim();
                    ACTDEPT = row["PRES_DEPT"].ToString().Trim();
                    ACTPROGRAM = row["PRES_PROGRAM"].ToString().Trim();
                    ACTYEAR = row["PRES_YEAR"].ToString().Trim();
                    ACTAPPNO = row["PRES_APP_NO"].ToString().Trim();
                    // ACTCODE = row["PRES_CODE"].ToString().Trim();


                }
                else if (Type == "SERQUES")
                {
                    ACTAGENCY = row["SER_AGENCY"].ToString().Trim();
                    ACTDEPT = row["SER_DEPT"].ToString().Trim();
                    ACTPROGRAM = row["SER_PROGRAM"].ToString().Trim();
                    ACTYEAR = row["SER_YEAR"].ToString().Trim();
                    ACTAPPNO = row["SER_APP_NO"].ToString().Trim();
                    ACTSNPFAMILYSEQ = row["SER_FUND"].ToString().Trim();
                    ACTCODE = row["SER_CODE"].ToString().Trim();
                    ACTRESPSEQ = row["SER_RESP_SEQ"].ToString().Trim();
                    ACTNUMRESP = row["SER_NUM_RESP"].ToString().Trim();
                    ACTALPHARESP = row["SER_ALPHA_RESP"].ToString().Trim();
                    ACTDATERESP = row["SER_DATE_RESP"].ToString().Trim();
                    ACTMULTRESP = row["SER_MULT_RESP"].ToString().Trim();
                    lstcdate = row["SER_DATE_LSTC"].ToString().Trim();
                    lstcoperator = row["SER_LSTC_OPERATOR"].ToString().Trim();
                    adddate = row["SER_DATE_ADD"].ToString().Trim();
                    addoperator = row["SER_ADD_OPERATOR"].ToString().Trim();
                    SER_ELEC = row["SER_ELEC"].ToString().Trim();
                    SER_KWH = row["SER_KWH"].ToString().Trim();
                    SER_GAS = row["SER_GAS"].ToString().Trim();
                    SER_CCF = row["SER_CCF"].ToString().Trim();
                    SER_ANNUAL = row["SER_ANNUAL"].ToString().Trim();
                }
                else if (Type == "SERQUESREP")
                {
                    ACTAGENCY = row["SER_AGENCY"].ToString().Trim();
                    ACTDEPT = row["SER_DEPT"].ToString().Trim();
                    ACTPROGRAM = row["SER_PROGRAM"].ToString().Trim();
                    ACTYEAR = row["SER_YEAR"].ToString().Trim();
                    ACTAPPNO = row["SER_APP_NO"].ToString().Trim();
                    ACTSNPFAMILYSEQ = row["SER_FUND"].ToString().Trim();
                    ACTCODE = row["SER_CODE"].ToString().Trim();
                    ACTRESPSEQ = row["SER_RESP_SEQ"].ToString().Trim();
                    ACTNUMRESP = row["SER_NUM_RESP"].ToString().Trim();
                    ACTALPHARESP = row["SER_ALPHA_RESP"].ToString().Trim();
                    ACTDATERESP = row["SER_DATE_RESP"].ToString().Trim();
                    ACTMULTRESP = row["SER_MULT_RESP"].ToString().Trim();
                    lstcdate = row["SER_DATE_LSTC"].ToString().Trim();
                    lstcoperator = row["SER_LSTC_OPERATOR"].ToString().Trim();
                    adddate = row["SER_DATE_ADD"].ToString().Trim();
                    addoperator = row["SER_ADD_OPERATOR"].ToString().Trim();
                    SER_ELEC = row["SER_ELEC"].ToString().Trim();
                    SER_KWH = row["SER_KWH"].ToString().Trim();
                    SER_GAS = row["SER_GAS"].ToString().Trim();
                    SER_CCF = row["SER_CCF"].ToString().Trim();
                    SER_ANNUAL = row["SER_ANNUAL"].ToString().Trim();
                    Appcount = row["APPCOUNT"].ToString().Trim();

                    FirstName = row["SNP_NAME_IX_FI"].ToString().Trim();
                    LastName = row["SNP_NAME_IX_LAST"].ToString().Trim();
                    MIName = row["SNP_NAME_IX_MI"].ToString().Trim();
                    Client_ID = row["MST_CLIENT_ID"].ToString().Trim();
                    INHH = row["MST_NO_INHH"].ToString().Trim();
                    HousingType = row["HOUSING"].ToString().Trim();
                    Site = row["MST_SITE"].ToString().Trim();
                    SiteName = row["SITE_NAME"].ToString().Trim();
                    FamIncome = row["MST_FAM_INCOME"].ToString().Trim();
                }
                else if (Type == "DIMSCOREREPORT")
                {
                    ACTAGENCY = row["DIMSCOR_AGENCY"].ToString().Trim();
                    ACTDEPT = row["DIMSCOR_DEPT"].ToString().Trim();
                    ACTPROGRAM = row["DIMSCOR_PROGRAM"].ToString().Trim();
                    ACTYEAR = row["DIMSCOR_YEAR"].ToString().Trim();
                    ACTAPPNO = row["DIMSCOR_APP_NO"].ToString().Trim();
                    ACTCODE = row["DIMSCOR_CODE"].ToString().Trim();
                    lstcdate = row["DIMSCOR_DATE_LSTC"].ToString().Trim();
                    lstcoperator = row["DIMSCOR_LSTC_OPERATOR"].ToString().Trim();
                    adddate = row["DIMSCOR_DATE_ADD"].ToString().Trim();
                    addoperator = row["DIMSCOR_ADD_OPERATOR"].ToString().Trim();
                    PRESPOINTS = row["DIMSCOR_SCORE"].ToString().Trim();

                    FirstName = row["SNP_NAME_IX_FI"].ToString().Trim();
                    LastName = row["SNP_NAME_IX_LAST"].ToString().Trim();
                    MIName = row["SNP_NAME_IX_MI"].ToString().Trim();

                    Appcount = row["MST_PRESS_TOTAL"].ToString().Trim();
                }
                else if (Type == "PIPQUESTIONS")
                {
                    // ACTAGENCY = row["ADDCUST_ACT_ID"].ToString().Trim();
                    ACTDEPT = row["ADDCUST_USERID"].ToString().Trim();
                    ACTCODE = row["ADDCUST_CODE"].ToString().Trim();
                    ACTRESPSEQ = row["ADDCUST_SEQ"].ToString().Trim();
                    ACTNUMRESP = row["ADDCUST_NUM_RESP"].ToString().Trim();
                    ACTALPHARESP = row["ADDCUST_ALPHA_RESP"].ToString().Trim();
                    ACTDATERESP = row["ADDCUST_DATE_RESP"].ToString().Trim();
                    ACTMULTRESP = row["ADDCUST_MULT_RESP"].ToString().Trim();


                }
                else if (Type == "USAGEQUES")
                {
                    ACTAGENCY = row["USAGE_AGENCY"].ToString().Trim();
                    ACTDEPT = row["USAGE_DEPT"].ToString().Trim();
                    ACTPROGRAM = row["USAGE_PROGRAM"].ToString().Trim();
                    ACTYEAR = row["USAGE_YEAR"].ToString().Trim();
                    ACTAPPNO = row["USAGE_APP_NO"].ToString().Trim();
                    //ACTCODE = row["USAGE_SP_CODE"].ToString().Trim();
                    //ACTRESPSEQ = row["USAGE_SP_SEQ"].ToString().Trim();                    
                    lstcdate = row["USAGE_DATE_LSTC"].ToString().Trim();
                    lstcoperator = row["USAGE_LSTC_OPERATOR"].ToString().Trim();
                    adddate = row["USAGE_DATE_ADD"].ToString().Trim();
                    addoperator = row["USAGE_ADD_OPERATOR"].ToString().Trim();
                    //SER_ELEC = row["USAGE_PRIM_PAYMENT"].ToString().Trim();
                    //SER_KWH = row["USAGE_PRIM_USAGE"].ToString().Trim();
                    //SER_GAS = row["USAGE_SEC_PAYMENT"].ToString().Trim();
                    //SER_CCF = row["USAGE_SEC_USAGE"].ToString().Trim();
                    //SER_ANNUAL = row["USAGE_TOTAL"].ToString().Trim();

                    USAGE_PRIM_PAYMENT = row["USAGE_PRIM_PAYMENT"].ToString().Trim();
                    USAGE_PRIM_USAGE = row["USAGE_PRIM_USAGE"].ToString().Trim();
                    USAGE_SEC_PAYMENT = row["USAGE_SEC_PAYMENT"].ToString().Trim();
                    USAGE_SEC_USAGE = row["USAGE_SEC_USAGE"].ToString().Trim();
                    USAGE_TOTAL = row["USAGE_TOTAL"].ToString().Trim();

                    USAGE_PRIM_12HIST = row["USAGE_PRIM_12HIST"].ToString().Trim();
                    USAGE_SEC_12HIST = row["USAGE_SEC_12HIST"].ToString().Trim();
                    USAGE_MONTH = row["USAGE_MONTH"].ToString().Trim();
                    USAGE_PMON_SW = row["USAGE_PMON_SW"].ToString().Trim();
                    USAGE_SMON_SW = row["USAGE_SMON_SW"].ToString().Trim();
                    USAGE_PSOURCE = row["USAGE_PSOURCE"].ToString().Trim();
                    USAGE_SSOURCE = row["USAGE_SSOURCE"].ToString().Trim();
                    USAGE_PRIM_VEND = row["USAGE_PRIM_VEND"].ToString().Trim();
                    USAGE_SEC_VEND = row["USAGE_SEC_VEND"].ToString().Trim();

                    USAGE_LUMP_PRIM = row["USAGE_LUMP_PRIM"].ToString().Trim();
                    USAGE_LUMP_SEC = row["USAGE_LUMP_SEC"].ToString().Trim();
                    USAGE_PRIM_ACTID = row["USAGE_PRIM_ACTID"].ToString().Trim();
                    USAGE_SEC_ACTID = row["USAGE_SEC_ACTID"].ToString().Trim();
                }
                else if (Type == "USAGEQUESREP")
                {
                    ACTSNPFAMILYSEQ = string.Empty;
                    ACTAGENCY = row["USAGE_AGENCY"].ToString().Trim();
                    ACTDEPT = row["USAGE_DEPT"].ToString().Trim();
                    ACTPROGRAM = row["USAGE_PROGRAM"].ToString().Trim();
                    ACTYEAR = row["USAGE_YEAR"].ToString().Trim();
                    ACTAPPNO = row["USAGE_APP_NO"].ToString().Trim();
                    // ACTSNPFAMILYSEQ = row["SER_FUND"].ToString().Trim();
                    ACTCODE = row["USAGE_MONTH"].ToString().Trim();
                    lstcdate = row["USAGE_DATE_LSTC"].ToString().Trim();
                    lstcoperator = row["USAGE_LSTC_OPERATOR"].ToString().Trim();
                    adddate = row["USAGE_DATE_ADD"].ToString().Trim();
                    addoperator = row["USAGE_ADD_OPERATOR"].ToString().Trim();
                    //SER_ELEC = row["USAGE_PRIM_PAYMENT"].ToString().Trim();
                    //SER_KWH = row["USAGE_PRIM_USAGE"].ToString().Trim();
                    //SER_GAS = row["USAGE_SMON_SW"].ToString().Trim();
                    //SER_CCF = row["USAGE_SEC_USAGE"].ToString().Trim();
                    //SER_ELEC = row["USAGE_PRIM_PAYMENT"].ToString().Trim() == string.Empty ? "0" : row["USAGE_PRIM_PAYMENT"].ToString().Trim();
                    //SER_KWH = row["USAGE_PRIM_USAGE"].ToString().Trim() == string.Empty ? "0" : row["USAGE_PRIM_USAGE"].ToString().Trim();
                    //SER_GAS = row["USAGE_SEC_PAYMENT"].ToString().Trim() == string.Empty ? "0" : row["USAGE_SEC_PAYMENT"].ToString().Trim();
                    //SER_CCF = row["USAGE_SEC_USAGE"].ToString().Trim() == string.Empty ? "0" : row["USAGE_SEC_USAGE"].ToString().Trim();
                    //SER_ANNUAL = row["USAGE_TOTAL"].ToString().Trim();


                    USAGE_PRIM_PAYMENT = row["USAGE_PRIM_PAYMENT"].ToString().Trim() == string.Empty ? "0" : row["USAGE_PRIM_PAYMENT"].ToString().Trim();
                    USAGE_PRIM_USAGE = row["USAGE_PRIM_USAGE"].ToString().Trim() == string.Empty ? "0" : row["USAGE_PRIM_USAGE"].ToString().Trim();
                    USAGE_SEC_PAYMENT = row["USAGE_SEC_PAYMENT"].ToString().Trim() == string.Empty ? "0" : row["USAGE_SEC_PAYMENT"].ToString().Trim();
                    USAGE_SEC_USAGE = row["USAGE_SEC_USAGE"].ToString().Trim() == string.Empty ? "0" : row["USAGE_SEC_USAGE"].ToString().Trim();
                    USAGE_TOTAL = row["USAGE_TOTAL"].ToString().Trim();

                    USAGE_LUMP_PRIM = row["USAGE_LUMP_PRIM"].ToString().Trim() == string.Empty ? "0" : row["USAGE_LUMP_PRIM"].ToString().Trim();
                    USAGE_LUMP_SEC = row["USAGE_LUMP_SEC"].ToString().Trim() == string.Empty ? "0" : row["USAGE_LUMP_SEC"].ToString().Trim();
                    USAGE_PRIM_ACTID = row["USAGE_PRIM_ACTID"].ToString().Trim();
                    USAGE_SEC_ACTID = row["USAGE_SEC_ACTID"].ToString().Trim();

                    USAGE_PRIM_12HIST = row["USAGE_PRIM_12HIST"].ToString().Trim();
                    USAGE_SEC_12HIST = row["USAGE_SEC_12HIST"].ToString().Trim();
                    USAGE_MONTH = row["USAGE_MONTH"].ToString().Trim();
                    USAGE_PMON_SW = row["USAGE_PMON_SW"].ToString().Trim();
                    USAGE_SMON_SW = row["USAGE_SMON_SW"].ToString().Trim();
                    Appcount = row["APPCOUNT"].ToString().Trim();

                    FirstName = row["SNP_NAME_IX_FI"].ToString().Trim();
                    LastName = row["SNP_NAME_IX_LAST"].ToString().Trim();
                    MIName = row["SNP_NAME_IX_MI"].ToString().Trim();
                    Client_ID = row["MST_CLIENT_ID"].ToString().Trim();
                    INHH = row["MST_NO_INHH"].ToString().Trim();
                    HousingType = row["HOUSING"].ToString().Trim();
                    Dwellingtypecode = row["MST_HOUSING"].ToString().Trim();
                    Site = row["MST_SITE"].ToString().Trim();
                    SiteName = row["SITE_NAME"].ToString().Trim();
                    FamIncome = row["MST_FAM_INCOME"].ToString().Trim();

                    USAGE_PSOURCE = row["USAGE_PSOURCE"].ToString().Trim();
                    USAGE_SSOURCE = row["USAGE_SSOURCE"].ToString().Trim();
                    USAGE_VENDORNAME1 = row["PVENDORNAME"].ToString().Trim();
                    USAGE_VENDORNAME2 = row["SVENDORNAME"].ToString().Trim();
                }
                else
                {
                    ACTAGENCY = row["ACT_AGENCY"].ToString().Trim();
                    ACTDEPT = row["ACT_DEPT"].ToString().Trim();
                    ACTPROGRAM = row["ACT_PROGRAM"].ToString().Trim();
                    ACTYEAR = row["ACT_YEAR"].ToString().Trim();
                    ACTAPPNO = row["ACT_APP_NO"].ToString().Trim();
                    ACTSNPFAMILYSEQ = row["ACT_SNP_FAMILY_SEQ"].ToString().Trim();
                    ACTCODE = row["ACT_CODE"].ToString().Trim();
                    ACTRESPSEQ = row["ACT_RESP_SEQ"].ToString().Trim();
                    ACTNUMRESP = row["ACT_NUM_RESP"].ToString().Trim();
                    ACTALPHARESP = row["ACT_ALPHA_RESP"].ToString().Trim();
                    ACTDATERESP = row["ACT_DATE_RESP"].ToString().Trim();
                    ACTMULTRESP = row["ACT_MULT_RESP"].ToString().Trim();
                    lstcdate = row["ACT_DATE_LSTC"].ToString().Trim();
                    lstcoperator = row["ACT_LSTC_OPERATOR"].ToString().Trim();
                    adddate = row["ACT_DATE_ADD"].ToString().Trim();
                    addoperator = row["ACT_ADD_OPERATOR"].ToString().Trim();
                }
            }

        }

        #endregion

        #region Properties

        public string CUSTSCRCODE { get; set; }
        public string CUSTCODE { get; set; }
        public string CUSTDESC { get; set; }
        public string CUSTSEQ { get; set; }
        public string CUSTRESPTYPE { get; set; }
        public string SUBSCR { get; set; }
        public string CUSTMEMACCESS { get; set; }
        public string CUSTEQUAL { get; set; }
        public string CUSTGREATER { get; set; }
        public string CUSTLESSTHAN { get; set; }
        public string CUSTALPHA { get; set; }
        public string CUSTOTHER { get; set; }
        public string CUSTABBRQUESTION { get; set; }
        public string CUSTCALLOWFDATE { get; set; }
        public string CUSTACTIVE { get; set; }
        public string CUSTREQUIRED { get; set; }
        public string CUSTACTIVECUST { get; set; }

        public string ACTAGENCY { get; set; }
        public string ACTDEPT { get; set; }
        public string ACTPROGRAM { get; set; }
        public string ACTYEAR { get; set; }
        public string ACTAPPNO { get; set; }
        public string ACTSNPFAMILYSEQ { get; set; }
        public string ACTCODE { get; set; }
        public string ACTRESPSEQ { get; set; }
        public string ACTNUMRESP { get; set; }
        public string ACTALPHARESP { get; set; }
        public string ACTDATERESP { get; set; }
        public string ACTMULTRESP { get; set; }
        public string PRESPOINTS { get; set; }

        public string SER_ELEC { get; set; }
        public string SER_KWH { get; set; }
        public string SER_GAS { get; set; }
        public string SER_CCF { get; set; }
        public string SER_ANNUAL { get; set; }
        public string Appcount { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MIName { get; set; }
        public string Client_ID { get; set; }
        public string INHH { get; set; }
        public string HousingType { get; set; }
        public string Dwellingtypecode { get; set; }
        public string Site { get; set; }
        public string SiteName { get; set; }
        public string FamIncome { get; set; }

        public string Mode { get; set; }
        public string lstcdate { get; set; }
        public string lstcoperator { get; set; }
        public string adddate { get; set; }
        public string addoperator { get; set; }

        public string USAGE_PRIM_12HIST { get; set; }
        public string USAGE_SEC_12HIST { get; set; }
        public string USAGE_MONTH { get; set; }
        public string USAGE_PMON_SW { get; set; }
        public string USAGE_SMON_SW { get; set; }
        public string USAGE_PSOURCE { get; set; }
        public string USAGE_SSOURCE { get; set; }
        public string USAGE_PRIM_VEND { get; set; }
        public string USAGE_SEC_VEND { get; set; }

        public string USAGE_VENDORNAME1 { get; set; }
        public string USAGE_VENDORNAME2 { get; set; }

        public string LetterDate { get; set; }

        public string USAGE_PRIM_PAYMENT { get; set; }
        public string USAGE_PRIM_USAGE { get; set; }
        public string USAGE_SEC_PAYMENT { get; set; }
        public string USAGE_SEC_USAGE { get; set; }
        public string USAGE_TOTAL { get; set; }

        public string USAGE_LUMP_PRIM { get; set; }
        public string USAGE_LUMP_SEC { get; set; }
        public string USAGE_PRIM_ACTID { get; set; }
        public string USAGE_SEC_ACTID { get; set; }

        public string INTKWorker { get; set; }

        #endregion

    }

    public class AddCustEntity
    {
        #region Constructors

        public AddCustEntity()
        {
            ACTAGENCY = string.Empty;
            ACTDEPT = string.Empty;
            ACTPROGRAM = string.Empty;
            ACTYEAR = string.Empty;
            ACTAPPNO = string.Empty;
            ACTSNPFAMILYSEQ = string.Empty;
            ACTCODE = string.Empty;
            ACTRESPSEQ = string.Empty;
            ACTNUMRESP = string.Empty;
            ACTALPHARESP = string.Empty;
            ACTDATERESP = string.Empty;
            ACTMULTRESP = string.Empty;

            Mode = string.Empty;
            lstcdate = string.Empty;
            lstcoperator = string.Empty;
            adddate = string.Empty;
            addoperator = string.Empty;

        }

        public AddCustEntity(bool Intialize)
        {
            if (Intialize)
            {
                ACTAGENCY = null;
                ACTDEPT = null;
                ACTPROGRAM = null;
                ACTYEAR = null;
                ACTAPPNO = null;
                ACTSNPFAMILYSEQ = null;
                ACTCODE = null;
                ACTRESPSEQ = null;
                ACTNUMRESP = null;
                ACTALPHARESP = null;
                ACTDATERESP = null;
                ACTMULTRESP = null;

                Mode = null;
                lstcdate = null;
                lstcoperator = null;
                adddate = null;
                addoperator = null;
            }
        }

        public AddCustEntity(DataRow CustResponse)
        {
            if (CustResponse != null)
            {
                DataRow row = CustResponse;

                ACTAGENCY = row["ACT_AGENCY"].ToString();
                ACTDEPT = row["ACT_DEPT"].ToString();
                ACTPROGRAM = row["ACT_PROGRAM"].ToString();
                ACTYEAR = row["ACT_YEAR"].ToString();
                ACTAPPNO = row["ACT_APP_NO"].ToString();
                ACTSNPFAMILYSEQ = row["ACT_SNP_FAMILY_SEQ"].ToString();
                ACTCODE = row["ACT_CODE"].ToString();
                ACTRESPSEQ = row["ACT_RESP_SEQ"].ToString();
                ACTNUMRESP = row["ACT_NUM_RESP"].ToString();
                ACTALPHARESP = row["ACT_ALPHA_RESP"].ToString();
                ACTDATERESP = row["ACT_DATE_RESP"].ToString();
                ACTMULTRESP = row["ACT_MULT_RESP"].ToString();
                lstcdate = row["ACT_DATE_LSTC"].ToString();
                lstcoperator = row["ACT_LSTC_OPERATOR"].ToString();
                adddate = row["ACT_DATE_ADD"].ToString();
                addoperator = row["ACT_ADD_OPERATOR"].ToString();
            }

        }

        public AddCustEntity(DataRow CustResponse, string strTable)
        {
            if (CustResponse != null)
            {
                DataRow row = CustResponse;
                if (strTable == "EMS00030")
                {
                    ACTAGENCY = row["SER_AGENCY"].ToString().Trim();
                    ACTDEPT = row["SER_DEPT"].ToString().Trim();
                    ACTPROGRAM = row["SER_PROGRAM"].ToString().Trim();
                    ACTYEAR = row["SER_YEAR"].ToString().Trim();
                    ACTAPPNO = row["SER_APP_NO"].ToString().Trim();
                    ACTSNPFAMILYSEQ = row["SER_FUND"].ToString().Trim();
                    ACTCODE = row["SER_CODE"].ToString().Trim();
                    ACTRESPSEQ = row["SER_RESP_SEQ"].ToString().Trim();
                    ACTNUMRESP = row["SER_NUM_RESP"].ToString().Trim();
                    ACTALPHARESP = row["SER_ALPHA_RESP"].ToString().Trim();
                    ACTDATERESP = row["SER_DATE_RESP"].ToString().Trim();
                    ACTMULTRESP = row["SER_MULT_RESP"].ToString().Trim();
                    lstcdate = row["SER_DATE_LSTC"].ToString().Trim();
                    lstcoperator = row["SER_LSTC_OPERATOR"].ToString().Trim();
                    adddate = row["SER_DATE_ADD"].ToString().Trim();
                    addoperator = row["SER_ADD_OPERATOR"].ToString().Trim();
                    FirstName = row["NAME"].ToString();
                }
                else
                {
                    ACTAGENCY = row["ACT_AGENCY"].ToString();
                    ACTDEPT = row["ACT_DEPT"].ToString();
                    ACTPROGRAM = row["ACT_PROGRAM"].ToString();
                    ACTYEAR = row["ACT_YEAR"].ToString();
                    ACTAPPNO = row["ACT_APP_NO"].ToString();
                    ACTSNPFAMILYSEQ = row["ACT_SNP_FAMILY_SEQ"].ToString();
                    ACTCODE = row["ACT_CODE"].ToString();
                    ACTRESPSEQ = row["ACT_RESP_SEQ"].ToString();
                    ACTNUMRESP = row["ACT_NUM_RESP"].ToString();
                    ACTALPHARESP = row["ACT_ALPHA_RESP"].ToString();
                    ACTDATERESP = row["ACT_DATE_RESP"].ToString();
                    ACTMULTRESP = row["ACT_MULT_RESP"].ToString();
                    lstcdate = row["ACT_DATE_LSTC"].ToString();
                    lstcoperator = row["ACT_LSTC_OPERATOR"].ToString();
                    adddate = row["ACT_DATE_ADD"].ToString();
                    addoperator = row["ACT_ADD_OPERATOR"].ToString();
                    FirstName = row["NAME"].ToString();
                    //LastName = row["SNP_NAME_IX_LAST"].ToString();
                    //MIName = row["SNP_NAME_IX_MI"].ToString();
                }
            }

        }

        #endregion

        #region Properties

        public string ACTAGENCY { get; set; }
        public string ACTDEPT { get; set; }
        public string ACTPROGRAM { get; set; }
        public string ACTYEAR { get; set; }
        public string ACTAPPNO { get; set; }
        public string ACTSNPFAMILYSEQ { get; set; }
        public string ACTCODE { get; set; }
        public string ACTRESPSEQ { get; set; }
        public string ACTNUMRESP { get; set; }
        public string ACTALPHARESP { get; set; }
        public string ACTDATERESP { get; set; }
        public string ACTMULTRESP { get; set; }

        public string Mode { get; set; }
        public string lstcdate { get; set; }
        public string lstcoperator { get; set; }
        public string adddate { get; set; }
        public string addoperator { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MIName { get; set; }

        #endregion

    }

    public class PreassessQuesEntity
    {
        #region Constructors

        public PreassessQuesEntity()
        {
            //PREDIMENTION_ID = string.Empty;
            //PREDIMENTION_DESC = string.Empty;
            //PREDIMENTION_SORT = string.Empty;
            PRECHILD_QID = string.Empty;
            PRECHILD_DID = string.Empty;
            PRECHILD_DQID = string.Empty;
            PRECHILD_REQ = string.Empty;
            PRECHILD_SORT = string.Empty;
            PRECHILD_ENABLE = string.Empty;
            PRECHILD_DISABLE = string.Empty;
            PRECHILD_SEQ = string.Empty;
            Type = string.Empty;
            Mode = string.Empty;

        }

        public PreassessQuesEntity(bool Intialize)
        {
            if (Intialize)
            {
                //PREDIMENTION_ID = null;
                //PREDIMENTION_DESC = null;
                //PREDIMENTION_SORT = null;
                PRECHILD_QID = null;
                PRECHILD_DID = null;
                PRECHILD_DQID = null;
                PRECHILD_REQ = null;
                PRECHILD_SORT = null;
                PRECHILD_ENABLE = null;
                PRECHILD_DISABLE = null;
                PRECHILD_SEQ = null;
                Type = null;
                Mode = null;
            }
        }

        public PreassessQuesEntity(DataRow Preassess, string strType)
        {
            if (Preassess != null)
            {
                DataRow row = Preassess;

                if (strType == "MASTER")
                {
                    //PREDIMENTION_ID = row["PREDIMENTION_ID"].ToString();
                    //PREDIMENTION_DESC = row["PREDIMENTION_DESC"].ToString();
                    //PREDIMENTION_SORT = row["PREDIMENTION_SORT"].ToString();
                    //Type = string.Empty.ToString();
                }
                else
                {
                    //PREDIMENTION_ID = row["PREDIMENTION_ID"].ToString();
                    //PREDIMENTION_DESC = row["PREDIMENTION_DESC"].ToString();
                    //PREDIMENTION_SORT = row["PREDIMENTION_SORT"].ToString();
                    PRECHILD_QID = row["PRECHILD_QID"].ToString();
                    PRECHILD_DID = row["PRECHILD_DID"].ToString();
                    PRECHILD_DQID = row["PRECHILD_DQID"].ToString();
                    PRECHILD_REQ = row["PRECHILD_REQ"].ToString();
                    PRECHILD_SORT = row["PRECHILD_SORT"].ToString();
                    PRECHILD_ENABLE = row["PRECHILD_ENABLE"].ToString();
                    PRECHILD_DISABLE = row["PRECHILD_DISABLE"].ToString();
                    PRECHILD_SEQ = row["CUST_SEQ"].ToString();
                    Type = string.Empty.ToString();

                }

            }

        }

        #endregion

        #region Properties

        //public string PREDIMENTION_ID { get; set; }
        //public string PREDIMENTION_DESC { get; set; }
        //public string PREDIMENTION_SORT { get; set; }
        public string PRECHILD_QID { get; set; }
        public string PRECHILD_DID { get; set; }
        public string PRECHILD_DQID { get; set; }
        public string PRECHILD_REQ { get; set; }
        public string PRECHILD_SORT { get; set; }
        public string PRECHILD_ENABLE { get; set; }
        public string PRECHILD_DISABLE { get; set; }
        public string PRECHILD_SEQ { get; set; }
        public string Type { get; set; }
        public string Mode { get; set; }

        #endregion

    }
}
