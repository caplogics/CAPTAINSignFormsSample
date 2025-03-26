/***********************************************************
 * Author : Kranthi
 * Date   : 12/29/2022
 * ********************************************************/
#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Model.Objects;
using System.Data;
using System.Data.SqlClient;
using Captain.DatabaseLayer;
#endregion

namespace Captain.Common.Model.Data
{
    public class SmartChoiceData
    {
        #region Properties
        public CaptainModel Model { get; set; }
        public string UserId { get; set; }
        #endregion

        public SmartChoiceData()
        {

        }




        public bool SMACHASSOC_INSUPDEL(string SMACH_ASSOC_ID
           , string SMACH_ASSOC_CAT_ID
           , string SMACH_ASSOC_XL_SERVICE
           , string SMACH_ASSOC_CAP_SPCODE
           , string SMACH_ASSOC_CAP_SP
           , string SMACH_ASSOC_CAP_SERCODE
           , string SMACH_ASSOC_CAP_SERVICE
           , string SMACH_OPERATOR
           , string SMACH_ASSOC_FUNDS
           , string SMACH_ASSOC_SITE
           , string SMACH_ASSOC_CASEWRKR
           , string SMACH_ASSOC_OBF
            , string SMACH_ASSOC_BRANCH, string SMACH_ASSOC_GROUP
           , string MODE)
        {
            bool boolStatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (SMACH_ASSOC_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_ASSOC_ID", SMACH_ASSOC_ID));
                if (SMACH_ASSOC_CAT_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_ASSOC_CAT_ID", SMACH_ASSOC_CAT_ID));

                if (SMACH_ASSOC_XL_SERVICE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_ASSOC_XL_SERVICE", SMACH_ASSOC_XL_SERVICE));

                if (SMACH_ASSOC_CAP_SPCODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_ASSOC_CAP_SPCODE", SMACH_ASSOC_CAP_SPCODE));
                if (SMACH_ASSOC_CAP_SP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_ASSOC_CAP_SP", SMACH_ASSOC_CAP_SP));
                if (SMACH_ASSOC_CAP_SERCODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_ASSOC_CAP_SERCODE", SMACH_ASSOC_CAP_SERCODE));
                if (SMACH_ASSOC_CAP_SERVICE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_ASSOC_CAP_SERVICE", SMACH_ASSOC_CAP_SERVICE));



                if (SMACH_ASSOC_FUNDS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_ASSOC_FUNDS", SMACH_ASSOC_FUNDS));
                if (SMACH_ASSOC_SITE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_ASSOC_SITE", SMACH_ASSOC_SITE));
                if (SMACH_ASSOC_CASEWRKR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_ASSOC_CASEWRKR", SMACH_ASSOC_CASEWRKR));
                if (SMACH_ASSOC_OBF != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_ASSOC_OBF", SMACH_ASSOC_OBF));

                if (SMACH_ASSOC_BRANCH != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_ASSOC_BRANCH", SMACH_ASSOC_BRANCH));
                if (SMACH_ASSOC_GROUP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_ASSOC_GROUP", SMACH_ASSOC_GROUP));


                if (MODE == "ADD")
                {
                    if (SMACH_OPERATOR != string.Empty)
                        sqlParamList.Add(new SqlParameter("@SMACH_ASSOC_ADD_OPERATOR", SMACH_OPERATOR));
                    if (SMACH_OPERATOR != string.Empty)
                        sqlParamList.Add(new SqlParameter("@SMACH_ASSOC_LSTC_OPERATOR", SMACH_OPERATOR));
                }
                else if (MODE == "UPDATE")
                {
                    if (SMACH_OPERATOR != string.Empty)
                        sqlParamList.Add(new SqlParameter("@SMACH_ASSOC_LSTC_OPERATOR", SMACH_OPERATOR));
                }
                if (MODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MODE", MODE));
                boolStatus = SmartChoiceDB.iSMACHASSOC_INSUPDEL(sqlParamList);
            }
            catch (Exception ex)
            {
                boolStatus = false;
            }
            return boolStatus;
        }


        public String INSUPDEL_SMACH_CASEACTS(string AGY, string DEPT, string PROGRAM, string YEAR, string APPNO, string SERVICEPLAN, string BRANCH, string GROUP, string ACTIVITYCODE, string OBOFCode
            , DataTable SMH_CAOBO_TYPE, DataTable SMH_CASESPM_TYPE, DataTable SMH_CASEACT_TYPE, string MODE, out string STRRESULT)
        {
            bool boolStatus = false; string outValue = "";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (AGY != "")
                    sqlParamList.Add(new SqlParameter("@AGY", AGY));

                if (DEPT != "")
                    sqlParamList.Add(new SqlParameter("@DEPT", DEPT));

                if (PROGRAM != "")
                    sqlParamList.Add(new SqlParameter("@PROGRAM", PROGRAM));

                if (YEAR != "")
                    sqlParamList.Add(new SqlParameter("@YEAR", YEAR));

                if (APPNO != "")
                    sqlParamList.Add(new SqlParameter("@APPNO", APPNO));

                if (SERVICEPLAN != "")
                    sqlParamList.Add(new SqlParameter("@SERVICEPLAN", SERVICEPLAN));

                if (BRANCH != "")
                    sqlParamList.Add(new SqlParameter("@BRANCH", BRANCH));

                if (GROUP != "")
                    sqlParamList.Add(new SqlParameter("@GROUP", GROUP));

                if (ACTIVITYCODE != "")
                    sqlParamList.Add(new SqlParameter("@ACTIVITYCODE", ACTIVITYCODE));

                if (OBOFCode != "")
                    sqlParamList.Add(new SqlParameter("@OBOFCode", OBOFCode));

                //if (SMH_CAOBO_TYPE != null)
                //{
                //    if (SMH_CAOBO_TYPE.Rows.Count > 0)
                //        sqlParamList.Add(new SqlParameter("@SMH_CAOBO_TYPE", SMH_CAOBO_TYPE));
                //}

                if (SMH_CASESPM_TYPE != null)
                {
                    if (SMH_CASESPM_TYPE.Rows.Count > 0)
                        sqlParamList.Add(new SqlParameter("@SMH_CASESPM_TYPE", SMH_CASESPM_TYPE));
                }
                if (SMH_CASEACT_TYPE != null)
                {
                    if (SMH_CASEACT_TYPE.Rows.Count > 0)
                        sqlParamList.Add(new SqlParameter("@SMH_CASEACT_TYPE", SMH_CASEACT_TYPE));
                }

                if (MODE != "")
                    sqlParamList.Add(new SqlParameter("@MODE", MODE));


                SqlParameter sqlOutSeq = new SqlParameter("@STRRESULT", SqlDbType.VarChar, 50);
                sqlOutSeq.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlOutSeq);

                boolStatus = SmartChoiceDB.iINSUPDEL_SMACH_CASEACTS(sqlParamList);
                outValue = sqlOutSeq.Value.ToString();
            }
            catch (Exception ex)
            {
                boolStatus = false;
            }
            STRRESULT = outValue;
            return STRRESULT;
        }



        public bool SMACHMID_INSDELGET(string SMACH_MID_ID, string SMACH_MID_FILENAME, string SMACH_MID_AGY, string SMACH_MID_DEPT, string SMACH_MID_PROG, string SMACH_MID_YEAR
                , string SMACH_MID_APPNO, string SMACH_MID_CASEACT_ID,  string SMACH_MID_OPERATOR, string MODE)
        {
            bool boolStatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (SMACH_MID_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_MID_ID", SMACH_MID_ID));
                if (SMACH_MID_FILENAME != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_MID_FILENAME", SMACH_MID_FILENAME));
                if (SMACH_MID_AGY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_MID_AGY", SMACH_MID_AGY));

                if (SMACH_MID_DEPT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_MID_DEPT", SMACH_MID_DEPT));

                if (SMACH_MID_PROG != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_MID_PROG", SMACH_MID_PROG));
                if (SMACH_MID_YEAR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_MID_YEAR", SMACH_MID_YEAR));

                if (SMACH_MID_APPNO != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_MID_APPNO", SMACH_MID_APPNO));
                if (SMACH_MID_CASEACT_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_MID_CASEACT_ID", SMACH_MID_CASEACT_ID));
                if (SMACH_MID_OPERATOR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SMACH_MID_OPERATOR", SMACH_MID_OPERATOR));
                if (MODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MODE", MODE));
                boolStatus = SmartChoiceDB.iSMACHMID_INSDELGET(sqlParamList);
            }
            catch (Exception ex)
            {
                boolStatus = false;
            }
            return boolStatus;
        }

    }
}

public class SMH_CAOBO_TYPE
{
    public int CAOBO_ID { get; set; }
    public int CAOBO_SEQ { get; set; }
    public int CAOBO_CLID { get; set; }
    public int CAOBO_FAM_SEQ { get; set; }
    public decimal CAOBO_AMOUNT { get; set; }
    public string CAOBO_DESC { get; set; }
    public string CAOBO_SGRADE { get; set; }
    public string CAOBO_SDISTRICT { get; set; }
    public string CAOBO_STATUS { get; set; }
    public string CAOBO_COMPDATE { get; set; }
    public decimal CAOBO_TRANSUNITS { get; set; }
    public string CAOBO_RECPINAME { get; set; }
    public string CAOBO_GIFT1 { get; set; }
    public string CAOBO_GIFT2 { get; set; }
    public string CAOBO_GIFT3 { get; set; }
    public string CAOBO_GIFTCARD { get; set; }
    public int CAOBO_BEDSIZE { get; set; }
    public string CAOBO_AIRMATTRESS { get; set; }
    public string CAOBO_TRANSUOM { get; set; }
    public decimal CAOBO_CLOTHSIZE { get; set; }
    public decimal CAOBO_SHOESIZE { get; set; }
    public decimal CAOBO_QUANTITY { get; set; }
    public decimal CAOBO_UNITPRICE { get; set; }

}

public class SMH_CASEACT_TYPE
{
    public string CASEACT_AGENCY { get; set; }
    public string CASEACT_DEPT { get; set; }
    public string CASEACT_PROGRAM { get; set; }
    public string CASEACT_YEAR { get; set; }
    public string CASEACT_APP_NO { get; set; }
    public int CASEACT_SERVICE_PLAN { get; set; }
    public int CASEACT_SPM_SEQ { get; set; }
    public string CASEACT_BRANCH { get; set; }
    public int CASEACT_GROUP { get; set; }
    public string CASEACT_ACTIVITY_CODE { get; set; }
    public int CASEACT_SEQ { get; set; }
    public int CASEACT_ID { get; set; }
    public string CASEACT_BULK { get; set; }
    public string CASEACT_ACTY_DATE { get; set; }
    public string CASEACT_SITE { get; set; }
    public string CASEACT_FUND1 { get; set; }
    public string CASEACT_FUND2 { get; set; }
    public string CASEACT_FUND3 { get; set; }
    public string CASEACT_CASEWRKR { get; set; }
    public string CASEACT_VENDOR_NO { get; set; }
    public string CASEACT_CHECK_DT { get; set; }
    public string CASEACT_CHECK_NO { get; set; }
    public decimal CASEACT_COST { get; set; }
    public string CASEACT_FOLLUP_ON { get; set; }
    public string CASEACT_FOLLUP_COMP { get; set; }
    public string CASEACT_FUPBY { get; set; }
    public string CASEACT_ACTY_PROG { get; set; }
    public string CASEACT_CUST1_CODE { get; set; }
    public string CASEACT_CUST1_VALUE { get; set; }
    public string CASEACT_CUST2_CODE { get; set; }
    public string CASEACT_CUST2_VALUE { get; set; }
    public string CASEACT_CUST3_CODE { get; set; }
    public string CASEACT_CUST3_VALUE { get; set; }
    public string CASEACT_DATE_LSTC { get; set; }
    public string CASEACT_LSTC_OPERATOR { get; set; }
    public string CASEACT_DATE_ADD { get; set; }
    public string CASEACT_ADD_OPERATOR { get; set; }
    public string CASEACT_CUST4_CODE { get; set; }
    public string CASEACT_CUST4_VALUE { get; set; }
    public string CASEACT_CUST5_CODE { get; set; }
    public string CASEACT_CUST5_VALUE { get; set; }
    public int CASEACT_UNITS { get; set; }
    public string CASEACT_UOM { get; set; }
    public string CASEACT_VOUCHNO { get; set; }
    public int CASEACT_CURR_GRP { get; set; }
    public string CASEACT_TRIG_CODE { get; set; }
    public string CASEACT_TRIG_DATE { get; set; }
    public int CASEACT_TRIG_DATE_SEQ { get; set; }
    public string CASEACT_SEEK_DATE { get; set; }
    public string CASEACT_VOUCH_ENTRY { get; set; }
    public string CASEACT_OBF { get; set; }
    public decimal CASEACT_RATE { get; set; }
    public decimal CASEACT_AMOUNT3 { get; set; }
    public int CASEACT_UNITS2 { get; set; }
    public string CASEACT_UOM2 { get; set; }
    public int CASEACT_UNITS3 { get; set; }
    public string CASEACT_UOM3 { get; set; }
    public string CASEACT_BILL_PERIOD { get; set; }
    public string CASEACT_VEND_ACCT { get; set; }
    public decimal CASEACT_ARREARS { get; set; }
    public string CASEACT_LVL1_APRVL { get; set; }
    public string CASEACT_LVL1_APRVL_DATE { get; set; }
    public string CASEACT_LVL2_APRVL { get; set; }
    public string CASEACT_LVL2_APRVL_DATE { get; set; }
    public string CASEACT_SENT_PMT_USER { get; set; }
    public string CASEACT_SENT_PMT_DATE { get; set; }
    public string CASEACT_BUNDLE_NO { get; set; }
    public string CASEACT_REJECT_DATE { get; set; }
    public string CASEACT_REJECT_CODE { get; set; }
    public string CASEACT_REJECT_BY { get; set; }
    public int CASEACT_AMOUNT { get; set; }
    public int CASEACT_AMOUNT2 { get; set; }
    public string CASEACT_BILL_TYPE { get; set; }
    public string CASEACT_BILL_FNAME { get; set; }
    public string CASEACT_BILL_LNAME { get; set; }
    public decimal CASEACT_PAYMENT_NO { get; set; }
    public string CASEACT_SOURCE { get; set; }
    public string CASEACT_ELEC_OTHER { get; set; }
    public int CASEACT_BDC_ID { get; set; }
    public string CASEACT_BENEFIT_REASN { get; set; }
    public string CASEACT_PDOUT { get; set; }

}

public class SMH_CASESPM_TYPE
{
    public string SPM_AGENCY { get; set; }
    public string SPM_DEPT { get; set; }
    public string SPM_PROGRAM { get; set; }
    public string SPM_YEAR { get; set; }
    public string SPM_APP_NO { get; set; }
    public int SPM_SERVICE_PLAN { get; set; }
    public int SPM_SEQ { get; set; }
    public string SPM_CASEWORKER { get; set; }
    public string SPM_SITE { get; set; }
    public string SPM_STARTDATE { get; set; }
    public string SPM_ESTDATE { get; set; }
    public string SPM_COMPDATE { get; set; }
    public string SPM_SEL_BRANCHES { get; set; }
    public string SPM_HAVE_ADDLBR { get; set; }
    public string SPM_DEF_PROGRAM { get; set; }
    public string SPM_BULK { get; set; }
    public string SPM_DATE_LSTC { get; set; }
    public string SPM_LSTC_OPERATOR { get; set; }
    public string SPM_DATE_ADD { get; set; }
    public string SPM_ADD_OPERATOR { get; set; }
    public string SPM_TRIG_CODE { get; set; }
    public string SPM_TRIG_DATE { get; set; }
    public int SPM_TRIG_DATE_SEQ { get; set; }
    public string SPM_MASS_CLOSE { get; set; }
    public string SPM_MASS_RESULT { get; set; }
    public string SPM_ELIG_STATUS { get; set; }
    public int SPM_BDC_ID { get; set; }
    public string SPM_VENDOR { get; set; }
    public string SPM_ACCOUNT_NO { get; set; }
    public string SPM_BILLNAME_TYPE { get; set; }
    public string SPM_BILL_FNAME { get; set; }
    public string SPM_BILL_LNAME { get; set; }
    public string SPM_FUND { get; set; }
    public decimal SPM_AMOUNT { get; set; }
    public decimal SPM_BAL_AMT { get; set; }
    public string SPM_GAS_VENDOR { get; set; }
    public string SPM_GAS_ACCOUNT_NO { get; set; }
    public string SPM_GAS_BILLNAME_TYPE { get; set; }
    public string SPM_GAS_BILL_FNAME { get; set; }
    public string SPM_GAS_BILL_LNAME { get; set; }
    public string SPM_PRIM_SOURCE { get; set; }
    public string SPM_SEC_SOURCE { get; set; }
    public string SPM_BENEFIT_REASN { get; set; }

}