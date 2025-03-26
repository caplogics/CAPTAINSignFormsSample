using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NPOI.SS.Formula.Functions;
using System.Diagnostics;
using System.Security.Policy;
using static DevExpress.XtraBars.Painters.PrimitivesPainterWindowsXP;

namespace Captain.Common.Model.Objects
{
    public class EMSBDCEntity
    {
        #region Constructors

        public EMSBDCEntity()
        {
            BDC_AGENCY = string.Empty;
            BDC_DEPT = string.Empty;
            BDC_PROGRAM = string.Empty;
            BDC_YEAR = string.Empty;
            BDC_COST_CENTER = string.Empty;
            BDC_GL_ACCOUNT = string.Empty;
            BDC_BUDGET_YEAR = string.Empty;
            BDC_DESCRIPTION = string.Empty;
            BDC_FUND = string.Empty;
            BDC_BUDGET = string.Empty;
            BDC_START = string.Empty;
            BDC_END = string.Empty;
            BDC_ACCOUNT_TYPE = string.Empty;
            BDC_INT_ORDER = string.Empty;
            BDC_AUDIT_FLAG = string.Empty;
            BDC_ALLOW_POSTING = string.Empty;
            BDC_SWEEP_DAYS = string.Empty;
            BDC_BALANCE = string.Empty;
            BDC_BAL_DATE = string.Empty;
            BDC_TOT_INV = string.Empty;
            BDC_TOT_COMMIT = string.Empty;
            BDC_DATE_LSTC = string.Empty;
            BDC_LSTC_OPERATOR = string.Empty;
            BDC_DATE_ADD = string.Empty;
            BDC_ADD_OPERATOR = string.Empty;
            BDC_REST_AMOUNT = string.Empty;
            BDC_CONTR_NUM = string.Empty;
            BDC_ALLOW_ZERO = string.Empty;
        }

        public EMSBDCEntity(DataRow Row)
        {
            if (Row != null)
            {
                BDC_AGENCY = Row["BDC_AGENCY"].ToString().Trim();
                BDC_DEPT = Row["BDC_DEPT"].ToString().Trim();
                BDC_PROGRAM = Row["BDC_PROGRAM"].ToString().Trim();
                BDC_YEAR = Row["BDC_YEAR"].ToString().Trim();
                BDC_COST_CENTER = Row["BDC_COST_CENTER"].ToString().Trim();
                BDC_GL_ACCOUNT = Row["BDC_GL_ACCOUNT"].ToString().Trim();
                BDC_BUDGET_YEAR = Row["BDC_BUDGET_YEAR"].ToString().Trim();
                BDC_DESCRIPTION = Row["BDC_DESCRIPTION"].ToString().Trim();
                BDC_FUND = Row["BDC_FUND"].ToString().Trim();
                BDC_BUDGET = Row["BDC_BUDGET"].ToString().Trim();
                BDC_START = Row["BDC_START"].ToString().Trim();
                BDC_END = Row["BDC_END"].ToString().Trim();
                BDC_ACCOUNT_TYPE = Row["BDC_ACCOUNT_TYPE"].ToString().Trim();
                BDC_INT_ORDER = Row["BDC_INT_ORDER"].ToString().Trim();
                BDC_AUDIT_FLAG = Row["BDC_AUDIT_FLAG"].ToString().Trim();
                BDC_ALLOW_POSTING = Row["BDC_ALLOW_POSTING"].ToString().Trim();
                BDC_SWEEP_DAYS = Row["BDC_SWEEP_DAYS"].ToString().Trim();
                BDC_BALANCE = Row["BDC_BALANCE"].ToString().Trim();
                BDC_BAL_DATE = Row["BDC_BAL_DATE"].ToString().Trim();
                BDC_TOT_INV = Row["BDC_TOT_INV"].ToString().Trim();
                BDC_TOT_COMMIT = Row["BDC_TOT_COMMIT"].ToString().Trim();
                BDC_DATE_LSTC = Row["BDC_DATE_LSTC"].ToString().Trim();
                BDC_LSTC_OPERATOR = Row["BDC_LSTC_OPERATOR"].ToString().Trim();
                BDC_DATE_ADD = Row["BDC_DATE_ADD"].ToString().Trim();
                BDC_ADD_OPERATOR = Row["BDC_ADD_OPERATOR"].ToString().Trim();
                BDC_LOCK_BY = Row["BDC_LOCK_BY"].ToString().Trim();
                BDC_LOCK_ON = Row["BDC_LOCK_ON"].ToString().Trim();
                BDC_LOCK_SCREEN = Row["BDC_LOCK_SCREEN"].ToString().Trim();
                BDC_CONTR_NUM = Row["BDC_CONTR_NUM"].ToString().Trim();
                BDC_ALLOW_ZERO = Row["BDC_ALLOW_ZERO"].ToString().Trim();
                BDC_INV_UPLD = Row["BDC_INV_UPLD"].ToString().Trim();
                BDC_REST_AMOUNT = string.Empty;
            }
        }

        public EMSBDCEntity(DataRow Row, string strType)
        {
            if (Row != null)
            {
                if (strType == "TotBalance")
                {
                    BDC_BALANCE = Row["BDC_BALANCE"].ToString().Trim();
                }
                else if (strType == "CalculateBalance")
                {
                    BDC_BALANCE = Row["BDC_BALANCE"].ToString().Trim();
                    BDC_BUDGET = Row["BDC_BUDGET"].ToString().Trim();
                    BDC_TOT_INV = Row["BDC_TOT_INV"].ToString().Trim();
                    BDC_TOT_COMMIT = Row["BDC_TOT_COMMIT"].ToString().Trim();
                    BDC_REST_AMOUNT = Row["BDC_REST_AMOUNT"].ToString().Trim();

                }
                else if (strType == "RECORDEXIST")
                {
                    BDC_AGENCY = Row["EMSRES_AGENCY"].ToString().Trim();
                }
                else if (strType == "RECORDVALIDATION")
                {
                    BDC_AGENCY = Row["EMSRES_AGENCY"].ToString().Trim();
                }
                else
                {
                    BDC_AGENCY = Row["BDC_AGENCY"].ToString().Trim();
                    BDC_DEPT = Row["BDC_DEPT"].ToString().Trim();
                    BDC_PROGRAM = Row["BDC_PROGRAM"].ToString().Trim();
                    BDC_YEAR = Row["BDC_YEAR"].ToString().Trim();
                    BDC_COST_CENTER = Row["BDC_COST_CENTER"].ToString().Trim();
                    BDC_GL_ACCOUNT = Row["BDC_GL_ACCOUNT"].ToString().Trim();
                    BDC_BUDGET_YEAR = Row["BDC_BUDGET_YEAR"].ToString().Trim();
                    BDC_DESCRIPTION = Row["BDC_DESCRIPTION"].ToString().Trim();
                    BDC_FUND = Row["BDC_FUND"].ToString().Trim();
                    BDC_BUDGET = Row["BDC_BUDGET"].ToString().Trim();
                    BDC_START = Row["BDC_START"].ToString().Trim();
                    BDC_END = Row["BDC_END"].ToString().Trim();
                    BDC_ACCOUNT_TYPE = Row["BDC_ACCOUNT_TYPE"].ToString().Trim();
                    BDC_INT_ORDER = Row["BDC_INT_ORDER"].ToString().Trim();
                    BDC_AUDIT_FLAG = Row["BDC_AUDIT_FLAG"].ToString().Trim();
                    BDC_ALLOW_POSTING = Row["BDC_ALLOW_POSTING"].ToString().Trim();
                    BDC_SWEEP_DAYS = Row["BDC_SWEEP_DAYS"].ToString().Trim();
                    BDC_BALANCE = Row["BDC_BALANCE"].ToString().Trim();
                    BDC_BAL_DATE = Row["BDC_BAL_DATE"].ToString().Trim();
                    BDC_TOT_INV = Row["BDC_TOT_INV"].ToString().Trim();
                    BDC_TOT_COMMIT = Row["BDC_TOT_COMMIT"].ToString().Trim();
                    BDC_DATE_LSTC = Row["BDC_DATE_LSTC"].ToString().Trim();
                    BDC_LSTC_OPERATOR = Row["BDC_LSTC_OPERATOR"].ToString().Trim();
                    BDC_DATE_ADD = Row["BDC_DATE_ADD"].ToString().Trim();
                    BDC_ADD_OPERATOR = Row["BDC_ADD_OPERATOR"].ToString().Trim();
                    BDC_LOCK_BY = Row["BDC_LOCK_BY"].ToString().Trim();
                    BDC_LOCK_ON = Row["BDC_LOCK_ON"].ToString().Trim();
                    BDC_LOCK_SCREEN = Row["BDC_LOCK_SCREEN"].ToString().Trim();
                    BDC_CONTR_NUM = Row["BDC_CONTR_NUM"].ToString().Trim();
                    BDC_ALLOW_ZERO = Row["BDC_ALLOW_ZERO"].ToString().Trim();
                    BDC_REST_AMOUNT = string.Empty;
                    BDC_INV_UPLD = Row["BDC_INV_UPLD"].ToString().Trim();
                    if (strType == "EMSB0014")
                    {
                        Available = Row["BAL"].ToString().Trim();
                    }
                }
            }
        }

        #endregion

        #region Properties

        public string BDC_AGENCY { get; set; }
        public string BDC_DEPT { get; set; }
        public string BDC_PROGRAM { get; set; }
        public string BDC_YEAR { get; set; }
        public string BDC_COST_CENTER { get; set; }
        public string BDC_GL_ACCOUNT { get; set; }
        public string BDC_BUDGET_YEAR { get; set; }
        public string BDC_DESCRIPTION { get; set; }
        public string BDC_FUND { get; set; }
        public string BDC_BUDGET { get; set; }
        public string BDC_START { get; set; }
        public string BDC_END { get; set; }
        public string BDC_ACCOUNT_TYPE { get; set; }
        public string BDC_INT_ORDER { get; set; }
        public string BDC_AUDIT_FLAG { get; set; }
        public string BDC_ALLOW_POSTING { get; set; }
        public string BDC_SWEEP_DAYS { get; set; }
        public string BDC_BALANCE { get; set; }
        public string BDC_BAL_DATE { get; set; }
        public string BDC_TOT_INV { get; set; }
        public string BDC_TOT_COMMIT { get; set; }
        public string BDC_DATE_LSTC { get; set; }
        public string BDC_LSTC_OPERATOR { get; set; }
        public string BDC_DATE_ADD { get; set; }
        public string BDC_ADD_OPERATOR { get; set; }
        public string Mode { get; set; }
        public string Available { get; set; }
        public string BDC_REST_AMOUNT { get; set; }
        public string BDC_LOCK_BY { get; set; }
        public string BDC_LOCK_ON { get; set; }
        public string BDC_LOCK_SCREEN { get; set; }
        public string BDC_CONTR_NUM { get; set; }
        public string BDC_ALLOW_ZERO { get; set; }
        public string BDC_INV_UPLD { get; set; }

        #endregion

    }

    public class EMSBDAEntity
    {

        #region Constructors

        public EMSBDAEntity()
        {
            BDA_AGENCY = string.Empty;
            BDA_DEPT = string.Empty;
            BDA_PROGRAM = string.Empty;
            BDA_YEAR = string.Empty;
            BDA_COST_CENTER = string.Empty;
            BDA_GL_ACCOUNT = string.Empty;
            BDA_BUDGET_YEAR = string.Empty;
            BDA_SEQ = string.Empty;
            BDA_OLD_DESC = string.Empty;
            BDA_OLD_FUND = string.Empty;
            BDA_OLD_BUDGET = string.Empty;
            BDA_OLD_START = string.Empty;
            BDA_OLD_END = string.Empty;
            BDA_OLD_ACCT_TYPE = string.Empty;
            BDA_OLD_INT_ORDER = string.Empty;
            BDA_OLD_AUDIT_FLAG = string.Empty;
            BDA_OLD_ALLOW_POST = string.Empty;
            BDA_OLD_SWEEP_DAYS = string.Empty;
            BDA_NEW_DESC = string.Empty;
            BDA_NEW_FUND = string.Empty;
            BDA_NEW_BUDGET = string.Empty;
            BDA_NEW_START = string.Empty;
            BDA_NEW_END = string.Empty;
            BDA_NEW_ACCT_TYPE = string.Empty;
            BDA_NEW_INT_ORDER = string.Empty;
            BDA_NEW_AUDIT_FLAG = string.Empty;
            BDA_NEW_ALLOW_POST = string.Empty;
            BDA_NEW_SWEEP_DAYS = string.Empty;
            BDA_REASON_CODE = string.Empty;
            BDA_REASON_DESC = string.Empty;
            BDA_DATE_CHANGD = string.Empty;
            BDA_COMNT1 = string.Empty;
            BDA_COMNT2 = string.Empty;
            BDA_DATE_ADD = string.Empty;
            BDA_ADD_OPERATOR = string.Empty;
            BDA_OLD_ALLOW_ZERO = string.Empty;
            BDA_NEW_ALLOW_ZERO = string.Empty;
        }

        public EMSBDAEntity(DataRow Row)
        {
            if (Row != null)
            {

                BDA_AGENCY = Row["BDA_AGENCY"].ToString().Trim();
                BDA_DEPT = Row["BDA_DEPT"].ToString().Trim();
                BDA_PROGRAM = Row["BDA_PROGRAM"].ToString().Trim();
                BDA_YEAR = Row["BDA_YEAR"].ToString().Trim();
                BDA_COST_CENTER = Row["BDA_COST_CENTER"].ToString().Trim();
                BDA_GL_ACCOUNT = Row["BDA_GL_ACCOUNT"].ToString().Trim();
                BDA_BUDGET_YEAR = Row["BDA_BUDGET_YEAR"].ToString().Trim();
                BDA_SEQ = Row["BDA_SEQ"].ToString().Trim();
                BDA_OLD_DESC = Row["BDA_OLD_DESC"].ToString().Trim();
                BDA_OLD_FUND = Row["BDA_OLD_FUND"].ToString().Trim();
                BDA_OLD_BUDGET = Row["BDA_OLD_BUDGET"].ToString().Trim();
                BDA_OLD_START = Row["BDA_OLD_START"].ToString().Trim();
                BDA_OLD_END = Row["BDA_OLD_END"].ToString().Trim();
                BDA_OLD_ACCT_TYPE = Row["BDA_OLD_ACCT_TYPE"].ToString().Trim();
                BDA_OLD_INT_ORDER = Row["BDA_OLD_INT_ORDER"].ToString().Trim();
                BDA_OLD_AUDIT_FLAG = Row["BDA_OLD_AUDIT_FLAG"].ToString().Trim();
                BDA_OLD_ALLOW_POST = Row["BDA_OLD_ALLOW_POST"].ToString().Trim();
                BDA_OLD_SWEEP_DAYS = Row["BDA_OLD_SWEEP_DAYS"].ToString().Trim();
                BDA_NEW_DESC = Row["BDA_NEW_DESC"].ToString().Trim();
                BDA_NEW_FUND = Row["BDA_NEW_FUND"].ToString().Trim();
                BDA_NEW_BUDGET = Row["BDA_NEW_BUDGET"].ToString().Trim();
                BDA_NEW_START = Row["BDA_NEW_START"].ToString().Trim();
                BDA_NEW_END = Row["BDA_NEW_END"].ToString().Trim();
                BDA_NEW_ACCT_TYPE = Row["BDA_NEW_ACCT_TYPE"].ToString().Trim();
                BDA_NEW_INT_ORDER = Row["BDA_NEW_INT_ORDER"].ToString().Trim();
                BDA_NEW_AUDIT_FLAG = Row["BDA_NEW_AUDIT_FLAG"].ToString().Trim();
                BDA_NEW_ALLOW_POST = Row["BDA_NEW_ALLOW_POST"].ToString().Trim();
                BDA_NEW_SWEEP_DAYS = Row["BDA_NEW_SWEEP_DAYS"].ToString().Trim();
                BDA_REASON_CODE = Row["BDA_REASON_CODE"].ToString().Trim();
                BDA_REASON_DESC = Row["BDA_REASON_DESC"].ToString().Trim();
                BDA_DATE_CHANGD = Row["BDA_DATE_CHANGD"].ToString().Trim();
                BDA_COMNT1 = Row["BDA_COMNT1"].ToString().Trim();
                BDA_COMNT2 = Row["BDA_COMNT2"].ToString().Trim();
                BDA_DATE_ADD = Row["BDA_DATE_ADD"].ToString().Trim();
                BDA_ADD_OPERATOR = Row["BDA_ADD_OPERATOR"].ToString().Trim();
                BDA_OLD_ALLOW_ZERO = Row["BDA_OLD_ALLOW_ZERO"].ToString().Trim();
                BDA_NEW_ALLOW_ZERO = Row["BDA_NEW_ALLOW_ZERO"].ToString().Trim();
            }
        }

        #endregion

        #region Properties

        public string BDA_AGENCY { get; set; }
        public string BDA_DEPT { get; set; }
        public string BDA_PROGRAM { get; set; }
        public string BDA_YEAR { get; set; }
        public string BDA_COST_CENTER { get; set; }
        public string BDA_GL_ACCOUNT { get; set; }
        public string BDA_BUDGET_YEAR { get; set; }
        public string BDA_SEQ { get; set; }
        public string BDA_OLD_DESC { get; set; }
        public string BDA_OLD_FUND { get; set; }
        public string BDA_OLD_BUDGET { get; set; }
        public string BDA_OLD_START { get; set; }
        public string BDA_OLD_END { get; set; }
        public string BDA_OLD_ACCT_TYPE { get; set; }
        public string BDA_OLD_INT_ORDER { get; set; }
        public string BDA_OLD_AUDIT_FLAG { get; set; }
        public string BDA_OLD_ALLOW_POST { get; set; }
        public string BDA_OLD_SWEEP_DAYS { get; set; }
        public string BDA_NEW_DESC { get; set; }
        public string BDA_NEW_FUND { get; set; }
        public string BDA_NEW_BUDGET { get; set; }
        public string BDA_NEW_START { get; set; }
        public string BDA_NEW_END { get; set; }
        public string BDA_NEW_ACCT_TYPE { get; set; }
        public string BDA_NEW_INT_ORDER { get; set; }
        public string BDA_NEW_AUDIT_FLAG { get; set; }
        public string BDA_NEW_ALLOW_POST { get; set; }
        public string BDA_NEW_SWEEP_DAYS { get; set; }
        public string BDA_REASON_CODE { get; set; }
        public string BDA_REASON_DESC { get; set; }
        public string BDA_DATE_CHANGD { get; set; }
        public string BDA_COMNT1 { get; set; }
        public string BDA_COMNT2 { get; set; }
        public string BDA_DATE_ADD { get; set; }
        public string BDA_ADD_OPERATOR { get; set; }
        public string BDA_OLD_ALLOW_ZERO { get; set; }
        public string BDA_NEW_ALLOW_ZERO { get; set; }

        public string Mode { get; set; }

        #endregion

    }


    public class EMSCLCPMCEntity
    {

        #region Constructors

        public EMSCLCPMCEntity()
        {
            CLC_AGENCY = string.Empty;
            CLC_DEPT = string.Empty;
            CLC_PROGRAM = string.Empty;
            CLC_YEAR = string.Empty;
            CLC_APP = string.Empty;
            CLC_RES_FUND = string.Empty;
            CLC_RES_SEQ = string.Empty;
            CLC_SEQ = string.Empty;
            CLC_RES_DATE = string.Empty;
            CLC_S_ID = string.Empty;
            CLC_S_HEX_NO = string.Empty;
            CLC_S_OBO = string.Empty;
            CLC_S_CGN = string.Empty;
            CLC_S_CASEWORKER = string.Empty;
            CLC_S_SERVICE_CODE = string.Empty;
            CLC_S_VENDOR = string.Empty;
            CLC_S_ACCT = string.Empty;
            CLC_S_BIL_LNAME = string.Empty;
            CLC_S_BIL_FNAME = string.Empty;
            CLC_S_DECISION = string.Empty;
            CLC_S_DECSN_DATE = string.Empty;
            CLC_S_APPEAL = string.Empty;
            CLC_S_VOUCHER = string.Empty;
            CLC_S_FOL_DATE = string.Empty;
            CLC_S_FOLC_DATE = string.Empty;
            CLC_S_BEN_START = string.Empty;
            CLC_S_BEN_END = string.Empty;
            CLC_S_COST_CENTER = string.Empty;
            CLC_S_GL_ACCOUNT = string.Empty;
            CLC_S_COUNTY_YEAR = string.Empty;
            CLC_S_TEMP_AWARD = string.Empty;
            CLC_TMP_NPUSER = string.Empty;
            CLC_TMP_NPDATE = string.Empty;
            PMC_PAY_KEY = string.Empty;
            PMC_TYPE = string.Empty;
            PMC_CASEWORKER = string.Empty;
            PMC_DATE = string.Empty;
            PMC_AMOUNT = string.Empty;
            PMC_CHECK_DATE = string.Empty;
            PMC_CHECK_NO = string.Empty;
            PMC_CLOSE_LVL1_USER = string.Empty;
            PMC_AGENCY1 = string.Empty;
            PMC_DEPT1 = string.Empty;
            PMC_PROGRAM1 = string.Empty;
            PMC_YEAR1 = string.Empty;
            PMC_CLOSE_LVL1_DATE = string.Empty;
            PMC_CLOSE_LVL2_USER = string.Empty;
            PMC_AGENCY2 = string.Empty;
            PMC_DEPT2 = string.Empty;
            PMC_PROGRAM2 = string.Empty;
            PMC_YEAR2 = string.Empty;
            PMC_CLOSE_LVL2_DATE = string.Empty;
            PMC_AUTH_FOOD_VOUCHER = string.Empty;
            PMC_AUTH_LIQUIDATE = string.Empty;
            PMC_AUTH_AMT = string.Empty;
            PMC_AUTH_WORKER = string.Empty;
            PMC_AUTH_DATE = string.Empty;
            PMC_AUTH_AGENCY = string.Empty;
            PMC_AUTH_DEPT = string.Empty;
            PMC_AUTH_PROGRAM = string.Empty;
            PMC_AUTH_YEAR = string.Empty;
            PMC_AUTH_LVL_DATE = string.Empty;
            PMC_AUTH_LVL_USER = string.Empty;
            PMC_INV_AMT = string.Empty;
            PMC_INV_BILL_AMT = string.Empty;
            PMC_INV_BILL_DATE = string.Empty;
            PMC_INV_VENDOR_RATING = string.Empty;
            PMC_INV_WORKER = string.Empty;
            PMC_INV_DATE = string.Empty;
            PMC_DEPT_REJECT_DATE = string.Empty;
            PMC_REJECT_CODE = string.Empty;
            PMC_FILE_NAME = string.Empty;
            PMC_REJECT1_CODE = string.Empty;
            PMC_REJECT2_CODE = string.Empty;
            PMC_REJECT3_CODE = string.Empty;
            PMC_REJECT4_CODE = string.Empty;
            PMC_REJECT5_CODE = string.Empty;
            PMC_LIQUID_AMOUNT = string.Empty;
            CLC_DATE_LSTC = string.Empty;
            CLC_LSTC_OPERATOR = string.Empty;
            CLC_DATE_ADD = string.Empty;
            CLC_ADD_OPERATOR = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            MiddleName = string.Empty;
            FullName = string.Empty;
            Site = string.Empty;
            PMCXML = string.Empty;

            ResApp = string.Empty;
            BDC_COST_CENTER = string.Empty;
            BDC_GL_ACCOUNT = string.Empty;
            BDC_BUDGET_YEAR = string.Empty;
            BDC_FUND = string.Empty;
            BDC_START = string.Empty;
            BDC_END = string.Empty;
            BDC_LSTC_OPERATOR = string.Empty;
            MST_APP_NO = string.Empty;
            CLC_LOCK_BY = string.Empty;
            CLC_LOCK_ON = string.Empty;
            PMC_PAID_TYPE = string.Empty;
            CLC_INVLOG_ID = string.Empty;
            VendorName = string.Empty;
            VendorAddress = string.Empty;
        }

        public EMSCLCPMCEntity(DataRow Row)
        {
            if (Row != null)
            {
                CLC_AGENCY = Row["CLC_AGENCY"].ToString().Trim();
                CLC_DEPT = Row["CLC_DEPT"].ToString().Trim();
                CLC_PROGRAM = Row["CLC_PROGRAM"].ToString().Trim();
                CLC_YEAR = Row["CLC_YEAR"].ToString().Trim();
                CLC_APP = Row["CLC_APP"].ToString().Trim();
                CLC_RES_FUND = Row["CLC_RES_FUND"].ToString().Trim();
                CLC_RES_SEQ = Row["CLC_RES_SEQ"].ToString().Trim();
                CLC_SEQ = Row["CLC_SEQ"].ToString().Trim();
                CLC_RES_DATE = Row["CLC_RES_DATE"].ToString().Trim();
                CLC_S_ID = Row["CLC_S_ID"].ToString().Trim();
                CLC_S_HEX_NO = Row["CLC_S_HEX_NO"].ToString().Trim();
                CLC_S_OBO = Row["CLC_S_OBO"].ToString().Trim();
                CLC_S_CGN = Row["CLC_S_CGN"].ToString().Trim();
                CLC_S_CASEWORKER = Row["CLC_S_CASEWORKER"].ToString().Trim();
                CLC_S_SERVICE_CODE = Row["CLC_S_SERVICE_CODE"].ToString().Trim();
                CLC_S_VENDOR = Row["CLC_S_VENDOR"].ToString().Trim();
                CLC_S_ACCT = Row["CLC_S_ACCT"].ToString().Trim();
                CLC_S_BIL_LNAME = Row["CLC_S_BIL_LNAME"].ToString().Trim();
                CLC_S_BIL_FNAME = Row["CLC_S_BIL_FNAME"].ToString().Trim();
                CLC_S_DECISION = Row["CLC_S_DECISION"].ToString().Trim();
                CLC_S_DECSN_DATE = Row["CLC_S_DECSN_DATE"].ToString().Trim();
                CLC_S_APPEAL = Row["CLC_S_APPEAL"].ToString().Trim();
                CLC_S_VOUCHER = Row["CLC_S_VOUCHER"].ToString().Trim();
                CLC_S_FOL_DATE = Row["CLC_S_FOL_DATE"].ToString().Trim();
                CLC_S_FOLC_DATE = Row["CLC_S_FOLC_DATE"].ToString().Trim();
                CLC_S_BEN_START = Row["CLC_S_BEN_START"].ToString().Trim();
                CLC_S_BEN_END = Row["CLC_S_BEN_END"].ToString().Trim();
                CLC_S_COST_CENTER = Row["CLC_S_COST_CENTER"].ToString().Trim();
                CLC_S_GL_ACCOUNT = Row["CLC_S_GL_ACCOUNT"].ToString().Trim();
                CLC_S_COUNTY_YEAR = Row["CLC_S_COUNTY_YEAR"].ToString().Trim();
                CLC_S_TEMP_AWARD = Row["CLC_S_TEMP_AWARD"].ToString().Trim();
                CLC_TMP_NPUSER = Row["CLC_TMP_NPUSER"].ToString().Trim();
                CLC_TMP_NPDATE = Row["CLC_TMP_NPDATE"].ToString().Trim();
                PMC_PAY_KEY = Row["PMC_PAY_KEY"].ToString().Trim();
                PMC_TYPE = Row["PMC_TYPE"].ToString().Trim();
                PMC_CASEWORKER = Row["PMC_CASEWORKER"].ToString().Trim();
                PMC_DATE = Row["PMC_DATE"].ToString().Trim();
                PMC_AMOUNT = Row["PMC_AMOUNT"].ToString().Trim();
                PMC_CHECK_DATE = Row["PMC_CHECK_DATE"].ToString().Trim();
                PMC_CHECK_NO = Row["PMC_CHECK_NO"].ToString().Trim();
                PMC_CLOSE_LVL1_USER = Row["PMC_CLOSE_LVL1_USER"].ToString().Trim();
                PMC_AGENCY1 = Row["PMC_AGENCY1"].ToString().Trim();
                PMC_DEPT1 = Row["PMC_DEPT1"].ToString().Trim();
                PMC_PROGRAM1 = Row["PMC_PROGRAM1"].ToString().Trim();
                PMC_YEAR1 = Row["PMC_YEAR1"].ToString().Trim();
                PMC_CLOSE_LVL1_DATE = Row["PMC_CLOSE_LVL1_DATE"].ToString().Trim();
                PMC_CLOSE_LVL2_USER = Row["PMC_CLOSE_LVL2_USER"].ToString().Trim();
                PMC_AGENCY2 = Row["PMC_AGENCY2"].ToString().Trim();
                PMC_DEPT2 = Row["PMC_DEPT2"].ToString().Trim();
                PMC_PROGRAM2 = Row["PMC_PROGRAM2"].ToString().Trim();
                PMC_YEAR2 = Row["PMC_YEAR2"].ToString().Trim();
                PMC_CLOSE_LVL2_DATE = Row["PMC_CLOSE_LVL2_DATE"].ToString().Trim();
                PMC_AUTH_FOOD_VOUCHER = Row["PMC_AUTH_FOOD_VOUCHER"].ToString().Trim();
                PMC_AUTH_LIQUIDATE = Row["PMC_AUTH_LIQUIDATE"].ToString().Trim();
                PMC_AUTH_AMT = Row["PMC_AUTH_AMT"].ToString().Trim();
                PMC_AUTH_WORKER = Row["PMC_AUTH_WORKER"].ToString().Trim();
                PMC_AUTH_DATE = Row["PMC_AUTH_DATE"].ToString().Trim();
                PMC_AUTH_AGENCY = Row["PMC_AUTH_AGENCY"].ToString().Trim();
                PMC_AUTH_DEPT = Row["PMC_AUTH_DEPT"].ToString().Trim();
                PMC_AUTH_PROGRAM = Row["PMC_AUTH_PROGRAM"].ToString().Trim();
                PMC_AUTH_YEAR = Row["PMC_AUTH_YEAR"].ToString().Trim();
                PMC_AUTH_LVL_DATE = Row["PMC_AUTH_LVL_DATE"].ToString().Trim();
                PMC_AUTH_LVL_USER = Row["PMC_AUTH_LVL_USER"].ToString().Trim();
                PMC_INV_AMT = Row["PMC_INV_AMT"].ToString().Trim();
                PMC_INV_BILL_AMT = Row["PMC_INV_BILL_AMT"].ToString().Trim();
                PMC_INV_BILL_DATE = Row["PMC_INV_BILL_DATE"].ToString().Trim();
                PMC_INV_VENDOR_RATING = Row["PMC_INV_VENDOR_RATING"].ToString().Trim();
                PMC_INV_WORKER = Row["PMC_INV_WORKER"].ToString().Trim();
                PMC_INV_DATE = Row["PMC_INV_DATE"].ToString().Trim();
                PMC_DEPT_REJECT_DATE = Row["PMC_DEPT_REJECT_DATE"].ToString().Trim();
                PMC_REJECT_CODE = Row["PMC_REJECT_CODE"].ToString().Trim();
                PMC_FILE_NAME = Row["PMC_FILE_NAME"].ToString().Trim();
                PMC_REJECT1_CODE = Row["PMC_REJECT1_CODE"].ToString().Trim();
                PMC_REJECT2_CODE = Row["PMC_REJECT2_CODE"].ToString().Trim();
                PMC_REJECT3_CODE = Row["PMC_REJECT3_CODE"].ToString().Trim();
                PMC_REJECT4_CODE = Row["PMC_REJECT4_CODE"].ToString().Trim();
                PMC_REJECT5_CODE = Row["PMC_REJECT5_CODE"].ToString().Trim();
                PMC_LIQUID_AMOUNT = Row["PMC_LIQUID_AMOUNT"].ToString().Trim();
                CLC_DATE_LSTC = Row["CLC_DATE_LSTC"].ToString().Trim();
                CLC_LSTC_OPERATOR = Row["CLC_LSTC_OPERATOR"].ToString().Trim();
                CLC_DATE_ADD = Row["CLC_DATE_ADD"].ToString().Trim();
                CLC_ADD_OPERATOR = Row["CLC_ADD_OPERATOR"].ToString().Trim();
                CLC_LOCK_BY = Row["CLC_LOCK_BY"].ToString().Trim();
                CLC_LOCK_ON = Row["CLC_LOCK_ON"].ToString().Trim();
                CLC_LOCK_SCREEN = Row["CLC_LOCK_SCREEN"].ToString().Trim();
                PMC_PAID_TYPE = Row["PMC_PAID_TYPE"].ToString().Trim();
                CLC_INVLOG_ID = Row["CLC_INVLOG_ID"].ToString().Trim();
                FirstName = string.Empty;
                LastName = string.Empty;
                MiddleName = string.Empty;
                FullName = string.Empty;
                PMCXML = string.Empty;
                Site = string.Empty;
            }
        }

        public EMSCLCPMCEntity(DataRow Row, string strTable)
        {
            if (Row != null)
            {


                LastName = Row["SNP_NAME_IX_LAST"].ToString().Trim();
                FirstName = Row["SNP_NAME_IX_FI"].ToString().Trim();
                MiddleName = Row["SNP_NAME_IX_MI"].ToString().Trim();
                Site = Row["MST_SITE"].ToString().Trim();
                CLC_AGENCY = Row["CLC_AGENCY"].ToString().Trim();
                CLC_DEPT = Row["CLC_DEPT"].ToString().Trim();
                CLC_PROGRAM = Row["CLC_PROGRAM"].ToString().Trim();
                CLC_YEAR = Row["CLC_YEAR"].ToString().Trim();
                CLC_APP = Row["CLC_APP"].ToString().Trim();
                CLC_RES_FUND = Row["CLC_RES_FUND"].ToString().Trim();
                CLC_RES_SEQ = Row["CLC_RES_SEQ"].ToString().Trim();
                CLC_SEQ = Row["CLC_SEQ"].ToString().Trim();
                CLC_RES_DATE = Row["CLC_RES_DATE"].ToString().Trim();
                CLC_S_ID = Row["CLC_S_ID"].ToString().Trim();
                CLC_S_HEX_NO = Row["CLC_S_HEX_NO"].ToString().Trim();
                CLC_S_OBO = Row["CLC_S_OBO"].ToString().Trim();
                CLC_S_CGN = Row["CLC_S_CGN"].ToString().Trim();
                CLC_S_CASEWORKER = Row["CLC_S_CASEWORKER"].ToString().Trim();
                CLC_S_SERVICE_CODE = Row["CLC_S_SERVICE_CODE"].ToString().Trim();
                if (Row["CLC_S_VENDOR"].ToString().Trim().ToUpper().Contains("UNK"))
                    CLC_S_VENDOR = "0000000000";
                else
                    CLC_S_VENDOR = Row["CLC_S_VENDOR"].ToString().Trim() == string.Empty ? "0" : Row["CLC_S_VENDOR"].ToString().Trim();
                CLC_S_ACCT = Row["CLC_S_ACCT"].ToString().Trim();
                CLC_S_BIL_LNAME = Row["CLC_S_BIL_LNAME"].ToString().Trim();
                CLC_S_BIL_FNAME = Row["CLC_S_BIL_FNAME"].ToString().Trim();
                CLC_S_DECISION = Row["CLC_S_DECISION"].ToString().Trim();
                CLC_S_DECSN_DATE = Row["CLC_S_DECSN_DATE"].ToString().Trim();
                CLC_S_APPEAL = Row["CLC_S_APPEAL"].ToString().Trim();
                CLC_S_VOUCHER = Row["CLC_S_VOUCHER"].ToString().Trim();
                CLC_S_FOL_DATE = Row["CLC_S_FOL_DATE"].ToString().Trim();
                CLC_S_FOLC_DATE = Row["CLC_S_FOLC_DATE"].ToString().Trim();
                CLC_S_BEN_START = Row["CLC_S_BEN_START"].ToString().Trim();
                CLC_S_BEN_END = Row["CLC_S_BEN_END"].ToString().Trim();
                CLC_S_COST_CENTER = Row["CLC_S_COST_CENTER"].ToString().Trim();
                CLC_S_GL_ACCOUNT = Row["CLC_S_GL_ACCOUNT"].ToString().Trim();
                CLC_S_COUNTY_YEAR = Row["CLC_S_COUNTY_YEAR"].ToString().Trim();
                CLC_S_TEMP_AWARD = Row["CLC_S_TEMP_AWARD"].ToString().Trim();
                CLC_TMP_NPUSER = Row["CLC_TMP_NPUSER"].ToString().Trim();
                CLC_TMP_NPDATE = Row["CLC_TMP_NPDATE"].ToString().Trim();
                PMC_PAY_KEY = Row["PMC_PAY_KEY"].ToString().Trim();
                PMC_TYPE = Row["PMC_TYPE"].ToString().Trim();
                PMC_CASEWORKER = Row["PMC_CASEWORKER"].ToString().Trim();
                PMC_DATE = Row["PMC_DATE"].ToString().Trim();
                PMC_AMOUNT = Row["PMC_AMOUNT"].ToString().Trim();
                PMC_CHECK_DATE = Row["PMC_CHECK_DATE"].ToString().Trim();
                PMC_CHECK_NO = Row["PMC_CHECK_NO"].ToString().Trim();
                PMC_CLOSE_LVL1_USER = Row["PMC_CLOSE_LVL1_USER"].ToString().Trim();
                PMC_AGENCY1 = Row["PMC_AGENCY1"].ToString().Trim();
                PMC_DEPT1 = Row["PMC_DEPT1"].ToString().Trim();
                PMC_PROGRAM1 = Row["PMC_PROGRAM1"].ToString().Trim();
                PMC_YEAR1 = Row["PMC_YEAR1"].ToString().Trim();
                PMC_CLOSE_LVL1_DATE = Row["PMC_CLOSE_LVL1_DATE"].ToString().Trim();
                PMC_CLOSE_LVL2_USER = Row["PMC_CLOSE_LVL2_USER"].ToString().Trim();
                PMC_AGENCY2 = Row["PMC_AGENCY2"].ToString().Trim();
                PMC_DEPT2 = Row["PMC_DEPT2"].ToString().Trim();
                PMC_PROGRAM2 = Row["PMC_PROGRAM2"].ToString().Trim();
                PMC_YEAR2 = Row["PMC_YEAR2"].ToString().Trim();
                PMC_CLOSE_LVL2_DATE = Row["PMC_CLOSE_LVL2_DATE"].ToString().Trim();
                PMC_AUTH_FOOD_VOUCHER = Row["PMC_AUTH_FOOD_VOUCHER"].ToString().Trim();
                PMC_AUTH_LIQUIDATE = Row["PMC_AUTH_LIQUIDATE"].ToString().Trim();
                PMC_AUTH_AMT = Row["PMC_AUTH_AMT"].ToString().Trim();
                PMC_AUTH_WORKER = Row["PMC_AUTH_WORKER"].ToString().Trim();
                PMC_AUTH_DATE = Row["PMC_AUTH_DATE"].ToString().Trim();
                PMC_AUTH_AGENCY = Row["PMC_AUTH_AGENCY"].ToString().Trim();
                PMC_AUTH_DEPT = Row["PMC_AUTH_DEPT"].ToString().Trim();
                PMC_AUTH_PROGRAM = Row["PMC_AUTH_PROGRAM"].ToString().Trim();
                PMC_AUTH_YEAR = Row["PMC_AUTH_YEAR"].ToString().Trim();
                PMC_AUTH_LVL_DATE = Row["PMC_AUTH_LVL_DATE"].ToString().Trim();
                PMC_AUTH_LVL_USER = Row["PMC_AUTH_LVL_USER"].ToString().Trim();
                PMC_INV_AMT = Row["PMC_INV_AMT"].ToString().Trim();
                PMC_INV_BILL_AMT = Row["PMC_INV_BILL_AMT"].ToString().Trim();
                PMC_INV_BILL_DATE = Row["PMC_INV_BILL_DATE"].ToString().Trim();
                PMC_INV_VENDOR_RATING = Row["PMC_INV_VENDOR_RATING"].ToString().Trim();
                PMC_INV_WORKER = Row["PMC_INV_WORKER"].ToString().Trim();
                PMC_INV_DATE = Row["PMC_INV_DATE"].ToString().Trim();
                PMC_DEPT_REJECT_DATE = Row["PMC_DEPT_REJECT_DATE"].ToString().Trim();
                PMC_REJECT_CODE = Row["PMC_REJECT_CODE"].ToString().Trim();
                PMC_FILE_NAME = Row["PMC_FILE_NAME"].ToString().Trim();
                PMC_REJECT1_CODE = Row["PMC_REJECT1_CODE"].ToString().Trim();
                PMC_REJECT2_CODE = Row["PMC_REJECT2_CODE"].ToString().Trim();
                PMC_REJECT3_CODE = Row["PMC_REJECT3_CODE"].ToString().Trim();
                PMC_REJECT4_CODE = Row["PMC_REJECT4_CODE"].ToString().Trim();
                PMC_REJECT5_CODE = Row["PMC_REJECT5_CODE"].ToString().Trim();
                PMC_LIQUID_AMOUNT = Row["PMC_LIQUID_AMOUNT"].ToString().Trim();
                CLC_DATE_LSTC = Row["CLC_DATE_LSTC"].ToString().Trim();
                CLC_LSTC_OPERATOR = Row["CLC_LSTC_OPERATOR"].ToString().Trim();
                CLC_DATE_ADD = Row["CLC_DATE_ADD"].ToString().Trim();
                CLC_ADD_OPERATOR = Row["CLC_ADD_OPERATOR"].ToString().Trim();
                CLC_LOCK_ON = Row["CLC_LOCK_ON"].ToString().Trim();
                CLC_LOCK_BY = Row["CLC_LOCK_BY"].ToString().Trim();
                CLC_LOCK_SCREEN = Row["CLC_LOCK_SCREEN"].ToString().Trim();
                CLC_INVLOG_ID = Row["CLC_INVLOG_ID"].ToString().Trim();
                VendorName = Row["CASEVDD_NAME"].ToString().Trim();
                VendorAddress = Row["CASEVDD_ADDR1"].ToString().Trim();


            }
        }

        public EMSCLCPMCEntity(DataRow Row, string Screen, string strtable)
        {
            if (Row != null && Screen == "EMSB0025")
            {
                CLC_AGENCY = Row["CLC_AGENCY"].ToString().Trim();
                CLC_DEPT = Row["CLC_DEPT"].ToString().Trim();
                CLC_PROGRAM = Row["CLC_PROGRAM"].ToString().Trim();
                CLC_YEAR = Row["CLC_YEAR"].ToString().Trim();
                CLC_APP = Row["CLC_APP"].ToString().Trim();
                CLC_RES_FUND = Row["CLC_RES_FUND"].ToString().Trim();
                CLC_RES_SEQ = Row["CLC_RES_SEQ"].ToString().Trim();
                CLC_SEQ = Row["CLC_SEQ"].ToString().Trim();
                CLC_RES_DATE = Row["CLC_RES_DATE"].ToString().Trim();
                CLC_S_CGN = Row["CLC_S_CGN"].ToString().Trim();
                CLC_S_CASEWORKER = Row["CLC_S_CASEWORKER"].ToString().Trim();
                CLC_S_SERVICE_CODE = Row["CLC_S_SERVICE_CODE"].ToString().Trim();
                //CLC_S_VENDOR = Row["CLC_S_VENDOR"].ToString().Trim();
                CLC_S_DECISION = Row["CLC_S_DECISION"].ToString().Trim();
                CLC_S_DECSN_DATE = Row["CLC_S_DECSN_DATE"].ToString().Trim();
                CLC_S_COST_CENTER = Row["CLC_S_COST_CENTER"].ToString().Trim();
                CLC_S_GL_ACCOUNT = Row["CLC_S_GL_ACCOUNT"].ToString().Trim();
                CLC_S_COUNTY_YEAR = Row["CLC_S_COUNTY_YEAR"].ToString().Trim();
                //CLC_TMP_NPUSER = Row["CLC_TMP_NPUSER"].ToString().Trim();
                CLC_LSTC_OPERATOR = Row["CLC_LSTC_OPERATOR"].ToString().Trim();

                ResApp = Row["EMSRES_APP"].ToString().Trim();
                BDC_COST_CENTER = Row["BDC_COST_CENTER"].ToString().Trim();
                BDC_GL_ACCOUNT = Row["BDC_GL_ACCOUNT"].ToString().Trim();
                BDC_BUDGET_YEAR = Row["BDC_BUDGET_YEAR"].ToString().Trim();
                BDC_FUND = Row["BDC_FUND"].ToString().Trim();
                BDC_START = Row["BDC_START"].ToString().Trim();
                BDC_END = Row["BDC_END"].ToString().Trim();
                BDC_LSTC_OPERATOR = Row["BDC_LSTC_OPERATOR"].ToString().Trim();
            }
            else if (Row != null && Screen == "EMSB0026")
            {
                CLC_AGENCY = Row["CLC_AGENCY"].ToString().Trim();
                CLC_DEPT = Row["CLC_DEPT"].ToString().Trim();
                CLC_PROGRAM = Row["CLC_PROGRAM"].ToString().Trim();
                CLC_YEAR = Row["CLC_YEAR"].ToString().Trim();
                CLC_APP = Row["CLC_APP"].ToString().Trim();
                CLC_RES_FUND = Row["CLC_RES_FUND"].ToString().Trim();
                CLC_RES_SEQ = Row["CLC_RES_SEQ"].ToString().Trim();
                CLC_SEQ = Row["CLC_SEQ"].ToString().Trim();
                CLC_RES_DATE = Row["CLC_RES_DATE"].ToString().Trim();
                CLC_S_CASEWORKER = Row["CLC_S_CASEWORKER"].ToString().Trim();

                PMC_TYPE = Row["PMC_TYPE"].ToString().Trim();
                PMC_DATE = Row["PMC_DATE"].ToString().Trim();
                PMC_AMOUNT = Row["PMC_AMOUNT"].ToString().Trim();
                PMC_CASEWORKER = Row["PMC_CASEWORKER"].ToString().Trim();
            }
            else if (Row != null && Screen == "EMSB0023")
            {
                CLC_AGENCY = Row["MST_AGENCY"].ToString().Trim();
                CLC_DEPT = Row["MST_DEPT"].ToString().Trim();
                CLC_PROGRAM = Row["MST_PROGRAM"].ToString().Trim();
                CLC_YEAR = Row["MST_YEAR"].ToString().Trim();
                CLC_APP = Row["CLC_APP"].ToString().Trim();
                CLC_RES_FUND = Row["CLC_RES_FUND"].ToString().Trim();
                CLC_RES_SEQ = Row["CLC_RES_SEQ"].ToString().Trim();
                CLC_SEQ = Row["CLC_SEQ"].ToString().Trim();
                CLC_RES_DATE = Row["CLC_RES_DATE"].ToString().Trim();
                CLC_S_HEX_NO = Row["CLC_S_HEX_NO"].ToString().Trim();
                CLC_S_VOUCHER = Row["CLC_S_VOUCHER"].ToString().Trim();
                PMC_DATE = Row["PMC_DATE"].ToString().Trim();
                PMC_TYPE = Row["PMC_TYPE"].ToString().Trim();
                PMC_AMOUNT = Row["PMC_AMOUNT"].ToString().Trim();
                PMC_CHECK_NO = Row["PMC_CHECK_NO"].ToString().Trim();
                CLC_S_SERVICE_CODE = Row["CLC_S_SERVICE_CODE"].ToString().Trim();
                CLC_S_VENDOR = Row["CLC_S_VENDOR"].ToString().Trim();
                PMC_CHECK_DATE = Row["PMC_CHECK_DATE"].ToString().Trim();
                //CLC_S_DECSN_DATE = Row["CLC_S_DECSN_DATE"].ToString().Trim();
                CLC_S_COST_CENTER = Row["CLC_S_COST_CENTER"].ToString().Trim();
                CLC_S_GL_ACCOUNT = Row["CLC_S_GL_ACCOUNT"].ToString().Trim();
                CLC_S_COUNTY_YEAR = Row["CLC_S_COUNTY_YEAR"].ToString().Trim();
                //CLC_TMP_NPUSER = Row["CLC_TMP_NPUSER"].ToString().Trim();
                //CLC_LSTC_OPERATOR = Row["CLC_LSTC_OPERATOR"].ToString().Trim();
                PMC_PAID_TYPE = Row["PMC_PAID_TYPE"].ToString().Trim();

                LastName = Row["SNP_NAME_IX_LAST"].ToString().Trim();
                FirstName = Row["SNP_NAME_IX_FI"].ToString().Trim();
                MiddleName = Row["SNP_NAME_IX_MI"].ToString().Trim();
                Site = Row["MST_SITE"].ToString().Trim();
                //BDC_START = Row["BDC_START"].ToString().Trim();
                //BDC_END = Row["BDC_END"].ToString().Trim();
                //BDC_LSTC_OPERATOR = Row["BDC_LSTC_OPERATOR"].ToString().Trim();

                MST_APP_NO = Row["MST_APP_NO"].ToString().Trim();
            }
        }

        #endregion

        #region Properties

        public string CLC_AGENCY { get; set; }
        public string CLC_DEPT { get; set; }
        public string CLC_PROGRAM { get; set; }
        public string CLC_YEAR { get; set; }
        public string CLC_APP { get; set; }
        public string CLC_RES_FUND { get; set; }
        public string CLC_RES_SEQ { get; set; }
        public string CLC_SEQ { get; set; }
        public string CLC_RES_DATE { get; set; }
        public string CLC_S_ID { get; set; }
        public string CLC_S_HEX_NO { get; set; }
        public string CLC_S_OBO { get; set; }
        public string CLC_S_CGN { get; set; }
        public string CLC_S_CASEWORKER { get; set; }
        public string CLC_S_SERVICE_CODE { get; set; }
        public string CLC_S_VENDOR { get; set; }
        public string CLC_S_ACCT { get; set; }
        public string CLC_S_BIL_LNAME { get; set; }
        public string CLC_S_BIL_FNAME { get; set; }
        public string CLC_S_DECISION { get; set; }
        public string CLC_S_DECSN_DATE { get; set; }
        public string CLC_S_APPEAL { get; set; }
        public string CLC_S_VOUCHER { get; set; }
        public string CLC_S_FOL_DATE { get; set; }
        public string CLC_S_FOLC_DATE { get; set; }
        public string CLC_S_BEN_START { get; set; }
        public string CLC_S_BEN_END { get; set; }
        public string CLC_S_COST_CENTER { get; set; }
        public string CLC_S_GL_ACCOUNT { get; set; }
        public string CLC_S_COUNTY_YEAR { get; set; }
        public string CLC_S_TEMP_AWARD { get; set; }
        public string CLC_TMP_NPUSER { get; set; }
        public string CLC_TMP_NPDATE { get; set; }
        public string PMC_PAY_KEY { get; set; }
        public string PMC_TYPE { get; set; }
        public string PMC_CASEWORKER { get; set; }
        public string PMC_DATE { get; set; }
        public string PMC_AMOUNT { get; set; }
        public string PMC_CHECK_DATE { get; set; }
        public string PMC_CHECK_NO { get; set; }
        public string PMC_CLOSE_LVL1_USER { get; set; }
        public string PMC_AGENCY1 { get; set; }
        public string PMC_DEPT1 { get; set; }
        public string PMC_PROGRAM1 { get; set; }
        public string PMC_YEAR1 { get; set; }
        public string PMC_CLOSE_LVL1_DATE { get; set; }
        public string PMC_CLOSE_LVL2_USER { get; set; }
        public string PMC_AGENCY2 { get; set; }
        public string PMC_DEPT2 { get; set; }
        public string PMC_PROGRAM2 { get; set; }
        public string PMC_YEAR2 { get; set; }
        public string PMC_CLOSE_LVL2_DATE { get; set; }
        public string PMC_AUTH_FOOD_VOUCHER { get; set; }
        public string PMC_AUTH_LIQUIDATE { get; set; }
        public string PMC_AUTH_AMT { get; set; }
        public string PMC_AUTH_WORKER { get; set; }
        public string PMC_AUTH_DATE { get; set; }
        public string PMC_AUTH_AGENCY { get; set; }
        public string PMC_AUTH_DEPT { get; set; }
        public string PMC_AUTH_PROGRAM { get; set; }
        public string PMC_AUTH_YEAR { get; set; }
        public string PMC_AUTH_LVL_DATE { get; set; }
        public string PMC_AUTH_LVL_USER { get; set; }
        public string PMC_INV_AMT { get; set; }
        public string PMC_INV_BILL_AMT { get; set; }
        public string PMC_INV_BILL_DATE { get; set; }
        public string PMC_INV_VENDOR_RATING { get; set; }
        public string PMC_INV_WORKER { get; set; }
        public string PMC_INV_DATE { get; set; }
        public string PMC_DEPT_REJECT_DATE { get; set; }
        public string PMC_REJECT_CODE { get; set; }
        public string PMC_FILE_NAME { get; set; }
        public string PMC_REJECT1_CODE { get; set; }
        public string PMC_REJECT2_CODE { get; set; }
        public string PMC_REJECT3_CODE { get; set; }
        public string PMC_REJECT4_CODE { get; set; }
        public string PMC_REJECT5_CODE { get; set; }
        public string PMC_LIQUID_AMOUNT { get; set; }
        public string CLC_DATE_LSTC { get; set; }
        public string CLC_LSTC_OPERATOR { get; set; }
        public string CLC_DATE_ADD { get; set; }
        public string CLC_ADD_OPERATOR { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FullName { get; set; }
        public string Site { get; set; }
        public string Mode { get; set; }
        public string PMCXML { get; set; }

        public string ResApp { get; set; }
        public string BDC_COST_CENTER { get; set; }
        public string BDC_GL_ACCOUNT { get; set; }
        public string BDC_BUDGET_YEAR { get; set; }
        public string BDC_FUND { get; set; }
        public string BDC_START { get; set; }
        public string BDC_END { get; set; }
        public string BDC_LSTC_OPERATOR { get; set; }
        public string CLC_LOCK_BY { get; set; }
        public string CLC_LOCK_ON { get; set; }
        public string CLC_LOCK_SCREEN { get; set; }
        public string MST_APP_NO { get; set; }

        public string PMC_PAID_TYPE { get; set; }

        public string CLC_INVLOG_ID { get; set; }

        public string VendorName { get; set; }
        public string VendorAddress { get; set; }


        #endregion

    }


    public class EMSRESEntity
    {

        #region Constructors

        public EMSRESEntity()
        {
            EMSRES_AGENCY = string.Empty;
            EMSRES_DEPT = string.Empty;
            EMSRES_PROGRAM = string.Empty;
            EMSRES_YEAR = string.Empty;
            EMSRES_APP = string.Empty;
            EMSRES_FUND = string.Empty;
            EMSRES_SEQ = string.Empty;
            EMSRES_DATE = string.Empty;
            EMSRES_CASEWORKER = string.Empty;
            EMSRES_AMOUNT = string.Empty;
            EMSRES_BALANCE = string.Empty;
            EMSRES_NPUSER = string.Empty;
            EMSRES_NPDATE = string.Empty;
            EMSRES_DATE_LSTC = string.Empty;
            EMSRES_LSTC_OPERATOR = string.Empty;
            EMSRES_DATE_ADD = string.Empty;
            EMSRES_ADD_OPERATOR = string.Empty;
            EMS_FundDesc = string.Empty;
            EMS_DateRange = string.Empty;
            EMS_Balance = string.Empty;
            EMS_Post = string.Empty;
            EMS_AdultFlag = string.Empty;
            EMS_Status = string.Empty;
            EMS_NonBudget = string.Empty;
            EMS_AllowPosting = string.Empty;
            EMS_Cost_Center = string.Empty;
            EMS_GL_Account = string.Empty;
            EMS_Budget_year = string.Empty;
            EMS_Budget = string.Empty;
            EMS_Start = string.Empty;
            EMS_End = string.Empty;
            EMS_TotInv = string.Empty;
            EMS_BudgetDesc = string.Empty;
            EMS_BudgetBal = string.Empty;
            EMS_BDC_DATELSTC = string.Empty;
            EMS_BdcIntOrder = string.Empty;
            EMS_AccountType = string.Empty;
            EMSRES_LOCK_BY = string.Empty;
            EMSRES_LOCK_ON = string.Empty;
            EMSRES_LOCK_SCREEN = string.Empty;
            EMSBDC_INV_UPLD = string.Empty;
            EMSRES_TYPE = string.Empty;
        }

        public EMSRESEntity(EMSRESEntity Ent)
        {
            if (Ent != null)
            {
                EMSRES_AGENCY = Ent.EMSRES_AGENCY;
                EMSRES_DEPT = Ent.EMSRES_DEPT;
                EMSRES_PROGRAM = Ent.EMSRES_PROGRAM;
                EMSRES_YEAR = Ent.EMSRES_YEAR;
                EMSRES_APP = Ent.EMSRES_APP;
                EMSRES_FUND = Ent.EMSRES_FUND;
                EMSRES_SEQ = Ent.EMSRES_SEQ;
                EMSRES_DATE = Ent.EMSRES_DATE;
                EMSRES_CASEWORKER = Ent.EMSRES_CASEWORKER;
                EMSRES_AMOUNT = Ent.EMSRES_AMOUNT;
                EMSRES_BALANCE = Ent.EMSRES_AMOUNT;
                EMSRES_NPUSER = Ent.EMSRES_NPUSER;
                EMSRES_NPDATE = Ent.EMSRES_NPDATE;
                EMSRES_DATE_LSTC = Ent.EMSRES_DATE_LSTC;
                EMSRES_LSTC_OPERATOR = Ent.EMSRES_LSTC_OPERATOR;
                EMSRES_DATE_ADD = Ent.EMSRES_DATE_ADD;
                EMSRES_ADD_OPERATOR = Ent.EMSRES_ADD_OPERATOR;
                EMS_BdcIntOrder = Ent.EMS_BdcIntOrder;
                EMS_FundDesc = Ent.EMS_FundDesc;
                EMS_DateRange = Ent.EMS_DateRange;
                EMS_Balance = Ent.EMS_Balance;
                EMS_Post = Ent.EMS_Post;
                EMS_AdultFlag = Ent.EMS_AdultFlag;
                EMS_Status = Ent.EMS_Status;
                EMS_NonBudget = Ent.EMS_NonBudget;
                EMS_AllowPosting = Ent.EMS_AllowPosting;
                EMS_Cost_Center = Ent.EMS_Cost_Center;
                EMS_GL_Account = Ent.EMS_GL_Account;
                EMS_Budget_year = Ent.EMS_Budget_year;
                EMS_Budget = Ent.EMS_Budget;
                EMS_Start = Ent.EMS_Start;
                EMS_End = Ent.EMS_End;
                EMS_TotInv = Ent.EMS_TotInv;
                EMS_BudgetDesc = Ent.EMS_BudgetDesc;
                EMS_BudgetBal = Ent.EMS_BudgetBal;
                EMSRES_LOCK_BY = Ent.EMSRES_LOCK_BY;
                EMSRES_LOCK_ON = Ent.EMSRES_LOCK_ON;
                EMSRES_LOCK_SCREEN = Ent.EMSRES_LOCK_SCREEN;

                EMSRES_TYPE = Ent.EMSRES_TYPE;
                EMSBDC_INV_UPLD = Ent.EMSBDC_INV_UPLD;
            }
        }

        public EMSRESEntity(DataRow Row)
        {
            if (Row != null)
            {
                EMS_FundDesc = string.Empty;
                EMS_DateRange = string.Empty;
                EMS_Balance = string.Empty;
                EMS_Post = string.Empty;
                EMS_AdultFlag = string.Empty;
                EMS_Status = string.Empty;
                EMS_NonBudget = string.Empty;
                EMS_AllowPosting = string.Empty;
                EMS_Cost_Center = string.Empty;
                EMS_GL_Account = string.Empty;
                EMS_Budget_year = string.Empty;
                EMS_Budget = string.Empty;
                EMS_Start = string.Empty;
                EMS_End = string.Empty;
                EMS_TotInv = string.Empty;
                EMS_BudgetDesc = string.Empty;
                EMS_BudgetBal = string.Empty;
                EMS_BDC_DATELSTC = string.Empty;
                EMS_BdcIntOrder = string.Empty;
                EMS_AccountType = string.Empty;
                EMSRES_LOCK_BY = string.Empty;
                EMSRES_LOCK_ON = string.Empty;
                EMSRES_LOCK_SCREEN = string.Empty;
                EMSBDC_INV_UPLD = string.Empty;
                EMSRES_AGENCY = Row["EMSRES_AGENCY"].ToString().Trim();
                EMSRES_DEPT = Row["EMSRES_DEPT"].ToString().Trim();
                EMSRES_PROGRAM = Row["EMSRES_PROGRAM"].ToString().Trim();
                EMSRES_YEAR = Row["EMSRES_YEAR"].ToString().Trim();
                EMSRES_APP = Row["EMSRES_APP"].ToString().Trim();
                EMSRES_FUND = Row["EMSRES_FUND"].ToString().Trim();
                EMSRES_SEQ = Row["EMSRES_SEQ"].ToString().Trim();
                EMSRES_DATE = Row["EMSRES_DATE"].ToString().Trim();
                EMSRES_CASEWORKER = Row["EMSRES_CASEWORKER"].ToString().Trim();
                EMSRES_AMOUNT = Row["EMSRES_AMOUNT"].ToString().Trim();
                EMSRES_BALANCE = Row["EMSRES_BALANCE"].ToString().Trim();
                EMSRES_NPUSER = Row["EMSRES_NPUSER"].ToString().Trim();
                EMSRES_NPDATE = Row["EMSRES_NPDATE"].ToString();
                EMSRES_DATE_LSTC = Row["EMSRES_DATE_LSTC"].ToString().Trim();
                EMSRES_LSTC_OPERATOR = Row["EMSRES_LSTC_OPERATOR"].ToString().Trim();
                EMSRES_DATE_ADD = Row["EMSRES_DATE_ADD"].ToString().Trim();
                EMSRES_ADD_OPERATOR = Row["EMSRES_ADD_OPERATOR"].ToString().Trim();
                EMSRES_LOCK_BY = Row["EMSRES_LOCK_BY"].ToString().Trim();
                EMSRES_LOCK_ON = Row["EMSRES_LOCK_ON"].ToString().Trim();
                EMSRES_LOCK_SCREEN = Row["EMSRES_LOCK_SCREEN"].ToString().Trim();

                EMSRES_TYPE = Row["EMSRES_TYPE"].ToString().Trim();


            }
        }

        public EMSRESEntity(DataRow Row, string strType)
        {
            if (Row != null)
            {
                if (strType == "RESAMOUNT")
                {
                    EMS_TotInv = Row["PMC_AMOUNT"].ToString();
                }
                else
                {
                    EMS_FundDesc = string.Empty;
                    EMS_DateRange = string.Empty;
                    EMS_Balance = string.Empty;
                    EMS_Post = string.Empty;
                    EMS_AdultFlag = string.Empty;
                    EMS_Status = string.Empty;
                    EMS_NonBudget = string.Empty;
                    EMS_AllowPosting = string.Empty;
                    EMS_Cost_Center = string.Empty;
                    EMS_GL_Account = string.Empty;
                    EMS_Budget_year = string.Empty;
                    EMS_Budget = string.Empty;
                    EMS_Start = string.Empty;
                    EMS_End = string.Empty;
                    EMS_TotInv = string.Empty;
                    EMS_BudgetDesc = string.Empty;
                    EMS_BudgetBal = string.Empty;
                    EMS_BDC_DATELSTC = string.Empty;
                    EMS_BdcIntOrder = string.Empty;
                    EMS_AccountType = string.Empty;
                    EMSRES_LOCK_BY = string.Empty;
                    EMSRES_LOCK_ON = string.Empty;
                    EMSRES_LOCK_SCREEN = string.Empty;
                    EMSBDC_INV_UPLD = string.Empty;
                    EMSRES_AGENCY = Row["EMSRES_AGENCY"].ToString().Trim();
                    EMSRES_DEPT = Row["EMSRES_DEPT"].ToString().Trim();
                    EMSRES_PROGRAM = Row["EMSRES_PROGRAM"].ToString().Trim();
                    EMSRES_YEAR = Row["EMSRES_YEAR"].ToString().Trim();
                    EMSRES_APP = Row["EMSRES_APP"].ToString().Trim();
                    EMSRES_FUND = Row["EMSRES_FUND"].ToString().Trim();
                    EMSRES_SEQ = Row["EMSRES_SEQ"].ToString().Trim();
                    EMSRES_DATE = Row["EMSRES_DATE"].ToString().Trim();
                    EMSRES_CASEWORKER = Row["EMSRES_CASEWORKER"].ToString().Trim();
                    EMSRES_AMOUNT = Row["EMSRES_AMOUNT"].ToString().Trim();
                    EMSRES_BALANCE = Row["EMSRES_BALANCE"].ToString().Trim();
                    EMSRES_NPUSER = Row["EMSRES_NPUSER"].ToString().Trim();
                    EMSRES_NPDATE = Row["EMSRES_NPDATE"].ToString();
                    EMSRES_DATE_LSTC = Row["EMSRES_DATE_LSTC"].ToString().Trim();
                    EMSRES_LSTC_OPERATOR = Row["EMSRES_LSTC_OPERATOR"].ToString().Trim();
                    EMSRES_DATE_ADD = Row["EMSRES_DATE_ADD"].ToString().Trim();
                    EMSRES_ADD_OPERATOR = Row["EMSRES_ADD_OPERATOR"].ToString().Trim();
                    EMSRES_LOCK_BY = Row["EMSRES_LOCK_BY"].ToString().Trim();
                    EMSRES_LOCK_ON = Row["EMSRES_LOCK_ON"].ToString().Trim();
                    EMSRES_LOCK_SCREEN = Row["EMSRES_LOCK_SCREEN"].ToString().Trim();

                    EMSRES_TYPE = Row["EMSRES_TYPE"].ToString().Trim();

                }
            }
        }

        #endregion

        #region Properties

        public string EMSRES_AGENCY { get; set; }
        public string EMSRES_DEPT { get; set; }
        public string EMSRES_PROGRAM { get; set; }
        public string EMSRES_YEAR { get; set; }
        public string EMSRES_APP { get; set; }
        public string EMSRES_FUND { get; set; }
        public string EMSRES_SEQ { get; set; }
        public string EMSRES_DATE { get; set; }
        public string EMSRES_CASEWORKER { get; set; }
        public string EMSRES_AMOUNT { get; set; }
        public string EMSRES_NPUSER { get; set; }
        public string EMSRES_NPDATE { get; set; }
        public string EMSRES_DATE_LSTC { get; set; }
        public string EMSRES_LSTC_OPERATOR { get; set; }
        public string EMSRES_DATE_ADD { get; set; }
        public string EMSRES_ADD_OPERATOR { get; set; }
        public string EMS_FundDesc { get; set; }
        public string EMS_DateRange { get; set; }
        public string EMS_Balance { get; set; }
        public string EMS_Post { get; set; }
        public string EMS_AdultFlag { get; set; }
        public string EMS_Status { get; set; }
        public string EMS_NonBudget { get; set; }
        public string EMS_AllowPosting { get; set; }
        public string EMS_Cost_Center { get; set; }
        public string EMS_GL_Account { get; set; }
        public string EMS_Budget_year { get; set; }
        public string EMS_Budget { get; set; }
        public string EMS_Start { get; set; }
        public string EMS_End { get; set; }
        public string EMS_TotInv { get; set; }
        public string EMS_BudgetDesc { get; set; }
        public string EMS_BudgetBal { get; set; }
        public string EMS_BDC_DATELSTC { get; set; }
        public string EMS_BdcIntOrder { get; set; }
        public string EMS_AccountType { get; set; }
        public string EMSRES_LOCK_BY { get; set; }
        public string EMSRES_LOCK_ON { get; set; }
        public string EMSRES_LOCK_SCREEN { get; set; }
        public string EMSRES_BALANCE { get; set; }
        public string EMSRES_TYPE { get; set; }
        public string EMSBDC_INV_UPLD { get; set; }

        public string Mode { get; set; }

        #endregion

    }

    public class EMSBCOEntity
    {

        #region Constructors

        public EMSBCOEntity()
        {
            AGENCY = string.Empty;
            DEPT = string.Empty;
            PROGRAM = string.Empty;
            YEAR = string.Empty;
            APP = string.Empty;
            FUND = string.Empty;
            SEQ = string.Empty;
            DATE = string.Empty;
            EMSBCO_CASEWORKER = string.Empty;
            EMSBCO_AMOUNT = string.Empty;
            EMSBCO_BALANCE = string.Empty;
            EMSBCO_NPUSER = string.Empty;
            EMSBCO_NPDATE = string.Empty;
            EMSBCO_DATE_LSTC = string.Empty;
            EMSBCO_LSTC_OPERATOR = string.Empty;
            EMSBCO_DATE_ADD = string.Empty;
            EMSBCO_ADD_OPERATOR = string.Empty;
            EMS_Start = string.Empty;
            EMS_End = string.Empty;
            EMSBCO_PAID_AMT = string.Empty;
            EMSBCO_TAR_FUND = string.Empty;
            EMSBCO_TAR_START = string.Empty;
            EMSBCO_TAR_END = string.Empty;
        }

        public EMSBCOEntity(EMSBCOEntity Ent)
        {
            if (Ent != null)
            {
                AGENCY = Ent.AGENCY;
                DEPT = Ent.DEPT;
                PROGRAM = Ent.PROGRAM;
                YEAR = Ent.YEAR;
                APP = Ent.APP;
                FUND = Ent.FUND;
                SEQ = Ent.SEQ;
                DATE = Ent.DATE;
                EMSBCO_CASEWORKER = Ent.EMSBCO_CASEWORKER;
                EMSBCO_AMOUNT = Ent.EMSBCO_AMOUNT;
                EMSBCO_BALANCE = Ent.EMSBCO_BALANCE;
                EMSBCO_NPUSER = Ent.EMSBCO_NPUSER;
                EMSBCO_NPDATE = Ent.EMSBCO_NPDATE;
                EMSBCO_DATE_LSTC = Ent.EMSBCO_DATE_LSTC;
                EMSBCO_LSTC_OPERATOR = Ent.EMSBCO_LSTC_OPERATOR;
                EMSBCO_DATE_ADD = Ent.EMSBCO_DATE_ADD;
                EMSBCO_ADD_OPERATOR = Ent.EMSBCO_ADD_OPERATOR;
                EMS_Start = Ent.EMS_Start;
                EMS_End = Ent.EMS_End;
                EMSBCO_PAID_AMT = Ent.EMSBCO_PAID_AMT;
                EMSBCO_TAR_FUND = Ent.EMSBCO_TAR_FUND;
                EMSBCO_TAR_START = Ent.EMSBCO_TAR_START;
                EMSBCO_TAR_END = Ent.EMSBCO_TAR_END;
            }
        }

        public EMSBCOEntity(DataRow Row)
        {
            if (Row != null)
            {
                AGENCY = Row["EMSBCO_AGENCY"].ToString().Trim();
                DEPT = Row["EMSBCO_DEPT"].ToString().Trim();
                PROGRAM = Row["EMSBCO_PROGRAM"].ToString().Trim();
                YEAR = Row["EMSBCO_YEAR"].ToString().Trim();
                APP = Row["EMSBCO_APP"].ToString().Trim();
                FUND = Row["EMSBCO_FUND"].ToString().Trim();
                SEQ = Row["EMSBCO_SEQ"].ToString().Trim();
                DATE = Row["EMSBCO_DATE"].ToString().Trim();
                EMSBCO_CASEWORKER = Row["EMSBCO_CASEWORKER"].ToString().Trim();
                EMSBCO_AMOUNT = Row["EMSBCO_AMOUNT"].ToString().Trim();
                EMSBCO_BALANCE = Row["EMSBCO_BALANCE"].ToString().Trim();
                EMSBCO_NPUSER = Row["EMSBCO_NPUSER"].ToString().Trim();
                EMSBCO_NPDATE = Row["EMSBCO_NPDATE"].ToString();
                EMSBCO_DATE_LSTC = Row["EMSBCO_DATE_LSTC"].ToString().Trim();
                EMSBCO_LSTC_OPERATOR = Row["EMSBCO_LSTC_OPERATOR"].ToString().Trim();
                EMSBCO_DATE_ADD = Row["EMSBCO_DATE_ADD"].ToString().Trim();
                EMSBCO_ADD_OPERATOR = Row["EMSBCO_ADD_OPERATOR"].ToString().Trim();
                EMS_Start = Row["EMSBCO_BDC_START"].ToString().Trim();
                EMS_End = Row["EMSBCO_BDC_END"].ToString().Trim();
                EMSBCO_PAID_AMT = Row["EMSBCO_PAID_AMT"].ToString().Trim();
                EMSBCO_TAR_FUND = Row["EMSBCO_TAR_FUND"].ToString().Trim();
                EMSBCO_TAR_START = Row["EMSBCO_TAR_START"].ToString().Trim();
                EMSBCO_TAR_END = Row["EMSBCO_TAR_END"].ToString().Trim();

            }
        }

        public EMSBCOEntity(DataRow Row, string strType)
        {
            if (Row != null)
            {
                //if (strType == "RESAMOUNT")
                //{
                //    EMS_TotInv = Row["PMC_AMOUNT"].ToString();
                //}
                //else
                //{
                //EMS_FundDesc = string.Empty;
                //EMS_DateRange = string.Empty;
                //EMS_Balance = string.Empty;
                //EMS_Post = string.Empty;
                //EMS_AdultFlag = string.Empty;
                //EMS_Status = string.Empty;
                //EMS_NonBudget = string.Empty;
                //EMS_AllowPosting = string.Empty;
                //EMS_Cost_Center = string.Empty;
                //EMS_GL_Account = string.Empty;
                //EMS_Budget_year = string.Empty;
                //EMS_Budget = string.Empty;
                //EMS_Start = string.Empty;
                //EMS_End = string.Empty;
                //EMS_TotInv = string.Empty;
                //EMS_BudgetDesc = string.Empty;
                //EMS_BudgetBal = string.Empty;
                //EMS_BDC_DATELSTC = string.Empty;
                //EMS_BdcIntOrder = string.Empty;
                //EMS_AccountType = string.Empty;
                //EMSRES_LOCK_BY = string.Empty;
                //EMSRES_LOCK_ON = string.Empty;
                //EMSRES_LOCK_SCREEN = string.Empty;
                AGENCY = Row["EMSBCO_AGENCY"].ToString().Trim();
                DEPT = Row["EMSBCO_DEPT"].ToString().Trim();
                PROGRAM = Row["EMSBCO_PROGRAM"].ToString().Trim();
                YEAR = Row["EMSBCO_YEAR"].ToString().Trim();
                APP = Row["EMSBCO_APP"].ToString().Trim();
                FUND = Row["EMSBCO_FUND"].ToString().Trim();
                SEQ = Row["EMSBCO_SEQ"].ToString().Trim();
                DATE = Row["EMSBCO_DATE"].ToString().Trim();
                EMSBCO_CASEWORKER = Row["EMSBCO_CASEWORKER"].ToString().Trim();
                EMSBCO_AMOUNT = Row["EMSBCO_AMOUNT"].ToString().Trim();
                EMSBCO_BALANCE = Row["EMSBCO_BALANCE"].ToString().Trim();
                EMSBCO_NPUSER = Row["EMSBCO_NPUSER"].ToString().Trim();
                EMSBCO_NPDATE = Row["EMSBCO_NPDATE"].ToString();
                EMSBCO_DATE_LSTC = Row["EMSBCO_DATE_LSTC"].ToString().Trim();
                EMSBCO_LSTC_OPERATOR = Row["EMSBCO_LSTC_OPERATOR"].ToString().Trim();
                EMSBCO_DATE_ADD = Row["EMSBCO_DATE_ADD"].ToString().Trim();
                EMSBCO_ADD_OPERATOR = Row["EMSBCO_ADD_OPERATOR"].ToString().Trim();
                EMS_Start = Row["EMSBCO_BDC_START"].ToString().Trim();
                EMS_End = Row["EMSBCO_BDC_END"].ToString().Trim();
                EMSBCO_PAID_AMT = Row["EMSBCO_PAID_AMT"].ToString().Trim();
                EMSBCO_TAR_FUND = Row["EMSBCO_TAR_FUND"].ToString().Trim();
                EMSBCO_TAR_START = Row["EMSBCO_TAR_START"].ToString().Trim();
                EMSBCO_TAR_END = Row["EMSBCO_TAR_END"].ToString().Trim();


                //}
            }
        }

        #endregion

        #region Properties

        public string AGENCY { get; set; }
        public string DEPT { get; set; }
        public string PROGRAM { get; set; }
        public string YEAR { get; set; }
        public string APP { get; set; }
        public string FUND { get; set; }
        public string SEQ { get; set; }
        public string DATE { get; set; }
        public string EMSBCO_CASEWORKER { get; set; }
        public string EMSBCO_AMOUNT { get; set; }
        public string EMSBCO_NPUSER { get; set; }
        public string EMSBCO_NPDATE { get; set; }
        public string EMSBCO_BALANCE { get; set; }
        public string EMSBCO_PAID_AMT { get; set; }
        public string EMS_Start { get; set; }
        public string EMS_End { get; set; }
        public string EMSBCO_TAR_FUND { get; set; }

        public string EMSBCO_TAR_START { get; set; }
        public string EMSBCO_TAR_END { get; set; }
        public string EMSBCO_DATE_LSTC { get; set; }
        public string EMSBCO_LSTC_OPERATOR { get; set; }
        public string EMSBCO_DATE_ADD { get; set; }
        public string EMSBCO_ADD_OPERATOR { get; set; }

        //public string EMS_FundDesc { get; set; }
        //public string EMS_DateRange { get; set; }
        //public string EMS_Balance { get; set; }
        //public string EMS_Post { get; set; }
        //public string EMS_AdultFlag { get; set; }
        //public string EMS_Status { get; set; }
        //public string EMS_NonBudget { get; set; }
        //public string EMS_AllowPosting { get; set; }
        //public string EMS_Cost_Center { get; set; }
        //public string EMS_GL_Account { get; set; }
        //public string EMS_Budget_year { get; set; }
        //public string EMS_Budget { get; set; }

        //public string EMS_TotInv { get; set; }
        //public string EMS_BudgetDesc { get; set; }
        //public string EMS_BudgetBal { get; set; }
        //public string EMS_BDC_DATELSTC { get; set; }
        //public string EMS_BdcIntOrder { get; set; }
        //public string EMS_AccountType { get; set; }
        //public string EMSRES_LOCK_BY { get; set; }
        //public string EMSRES_LOCK_ON { get; set; }
        //public string EMSRES_LOCK_SCREEN { get; set; }


        public string Mode { get; set; }

        #endregion

    }

    public class EMSSPEntity
    {

        #region Constructors

        public EMSSPEntity()
        {

            EMSSP_FUND = string.Empty;
            EMSSP_TYPE = string.Empty;
            EMSSP_CAMS_CODE = string.Empty;
            EMSSP_DATE_LSTC = string.Empty;
            EMSSP_LSTC_OPERATOR = string.Empty;
            EMSSP_ZEROAWARD = string.Empty;
            EMSSP_VENDOR = string.Empty;
            EMSSP_AUTHORIZATIONS = string.Empty;
            EMSSP_INVOICES = string.Empty;
        }

        public EMSSPEntity(DataRow Row)
        {
            if (Row != null)
            {

                EMSSP_FUND = Row["EMSSP_FUND"].ToString().Trim();
                EMSSP_TYPE = Row["EMSSP_TYPE"].ToString().Trim();
                EMSSP_CAMS_CODE = Row["EMSSP_CAMS_CODE"].ToString().Trim();
                EMSSP_DATE_LSTC = Row["EMSSP_DATE_LSTC"].ToString().Trim();
                EMSSP_LSTC_OPERATOR = Row["EMSSP_LSTC_OPERATOR"].ToString().Trim();
                EMSSP_ZEROAWARD = Row["EMSSP_ZEROAWARD"].ToString().Trim();
                EMSSP_VENDOR = Row["EMSSP_VENDOR"].ToString().Trim();
                EMSSP_AUTHORIZATIONS = Row["EMSSP_AUTHORIZATIONS"].ToString().Trim();
                EMSSP_INVOICES = Row["EMSSP_INVOICES"].ToString().Trim();
            }
        }

        #endregion

        #region Properties


        public string EMSSP_FUND { get; set; }
        public string EMSSP_TYPE { get; set; }
        public string EMSSP_CAMS_CODE { get; set; }
        public string EMSSP_ZEROAWARD { get; set; }
        public string EMSSP_VENDOR { get; set; }
        public string EMSSP_AUTHORIZATIONS { get; set; }
        public string EMSSP_INVOICES { get; set; }
        public string EMSSP_DATE_LSTC { get; set; }
        public string EMSSP_LSTC_OPERATOR { get; set; }
        public string EMSSP_Xml { get; set; }
        public string Mode { get; set; }

        #endregion

    }


    public class EMSCLAPMAEntity
    {

        #region Constructors

        public EMSCLAPMAEntity()
        {

            CLA_AGENCY = string.Empty;
            CLA_DEPT = string.Empty;
            CLA_PROGRAM = string.Empty;
            CLA_YEAR = string.Empty;
            CLA_APP = string.Empty;
            CLA_RES_FUND = string.Empty;
            CLA_RES_SEQ = string.Empty;
            CLA_RES_DATE = string.Empty;
            CLA_CLC_SEQ = string.Empty;
            CLA_SEQ = string.Empty;
            CLA_REASON = string.Empty;
            CLA_ADJ_DATE = string.Empty;
            CLA_O_CASEWORKER = string.Empty;
            CLA_O_SERVICE_CODE = string.Empty;
            CLA_O_VENDOR = string.Empty;
            CLA_O_ACCT = string.Empty;
            CLA_O_BIL_LNAME = string.Empty;
            CLA_O_BIL_FNAME = string.Empty;
            CLA_O_DECISION = string.Empty;
            CLA_O_DECISIONDATE = string.Empty;
            CLA_O_APPEAL = string.Empty;
            CLA_O_VOUCHER = string.Empty;
            CLA_O_FOLL_DATE = string.Empty;
            CLA_O_FOLL_CDATE = string.Empty;
            CLA_O_BENPERD_START = string.Empty;
            CLA_O_BENPERD_END = string.Empty;
            CLA_N_CASEWORKER = string.Empty;
            CLA_N_SERVICE_CODE = string.Empty;
            CLA_N_VENDOR = string.Empty;
            CLA_N_ACCT = string.Empty;
            CLA_N_BIL_LNAME = string.Empty;
            CLA_N_BIL_FNAME = string.Empty;
            CLA_N_DECISION = string.Empty;
            CLA_N_DECISIONDATE = string.Empty;
            CLA_N_APPEAL = string.Empty;
            CLA_N_VOUCHER = string.Empty;
            CLA_N_FOLL_DATE = string.Empty;
            CLA_N_FOLL_CDATE = string.Empty;
            CLA_N_BENPERD_START = string.Empty;
            CLA_N_BENPERD_END = string.Empty;
            PMA_O_TYPE = string.Empty;
            PMA_O_AMOUNT = string.Empty;
            PMA_O_AUTH_FOOD_VOUCHER = string.Empty;
            PMA_O_AUTH_LIQUIDATE = string.Empty;
            PMA_O_AUTH_AMOUNT = string.Empty;
            PMA_O_AUTH_WORKER = string.Empty;
            PMA_O_AUTH_DATE = string.Empty;
            PMA_O_INV_AMOUNT = string.Empty;
            PMA_O_INV_BILL_AMOUNT = string.Empty;
            PMA_O_INV_BILL_DATE = string.Empty;
            PMA_O_INV_VENDOR_RATING = string.Empty;
            PMA_O_INV_WORKER = string.Empty;
            PMA_O_INV_DATE = string.Empty;
            PMA_O_CHECK_DATE = string.Empty;
            PMA_O_CHECK_NO = string.Empty;
            PMA_O_FILE_NAME = string.Empty;
            PMA_N_TYPE = string.Empty;
            PMA_N_AMOUNT = string.Empty;
            PMA_N_AUTH_FOOD_VOUCHER = string.Empty;
            PMA_N_AUTH_LIQUIDATE = string.Empty;
            PMA_N_AUTH_AMOUNT = string.Empty;
            PMA_N_AUTH_WORKER = string.Empty;
            PMA_N_AUTH_DATE = string.Empty;
            PMA_N_INV_AMOUNT = string.Empty;
            PMA_N_INV_BILL_AMOUNT = string.Empty;
            PMA_N_INV_BILL_DATE = string.Empty;
            PMA_N_INV_VENDOR_RATING = string.Empty;
            PMA_N_INV_WORKER = string.Empty;
            PMA_N_INV_DATE = string.Empty;
            PMA_N_CHECK_DATE = string.Empty;
            PMA_N_CHECK_NO = string.Empty;
            PMA_N_FILE_NAME = string.Empty;
            RES_O_CASEWORKER = string.Empty;
            RES_O_AMOUNT = string.Empty;
            RES_N_CASEWORKER = string.Empty;
            RES_N_AMOUNT = string.Empty;
            CLA_DATE_ADD = string.Empty;
            CLA_ADD_OPERATOR = string.Empty;
        }

        public EMSCLAPMAEntity(DataRow Row)
        {
            if (Row != null)
            {

                CLA_AGENCY = Row["CLA_AGENCY"].ToString().Trim();
                CLA_DEPT = Row["CLA_DEPT"].ToString().Trim();
                CLA_PROGRAM = Row["CLA_PROGRAM"].ToString().Trim();
                CLA_YEAR = Row["CLA_YEAR"].ToString().Trim();
                CLA_APP = Row["CLA_APP"].ToString().Trim();
                CLA_RES_FUND = Row["CLA_RES_FUND"].ToString().Trim();
                CLA_RES_SEQ = Row["CLA_RES_SEQ"].ToString().Trim();
                CLA_RES_DATE = Row["CLA_RES_DATE"].ToString().Trim();
                CLA_CLC_SEQ = Row["CLA_CLC_SEQ"].ToString().Trim();
                CLA_SEQ = Row["CLA_SEQ"].ToString().Trim();
                CLA_REASON = Row["CLA_REASON"].ToString().Trim();
                CLA_ADJ_DATE = Row["CLA_ADJ_DATE"].ToString().Trim();
                CLA_O_CASEWORKER = Row["CLA_O_CASEWORKER"].ToString().Trim();
                CLA_O_SERVICE_CODE = Row["CLA_O_SERVICE_CODE"].ToString().Trim();
                CLA_O_VENDOR = Row["CLA_O_VENDOR"].ToString().Trim();
                CLA_O_ACCT = Row["CLA_O_ACCT"].ToString().Trim();
                CLA_O_BIL_LNAME = Row["CLA_O_BIL_LNAME"].ToString().Trim();
                CLA_O_BIL_FNAME = Row["CLA_O_BIL_FNAME"].ToString().Trim();
                CLA_O_DECISION = Row["CLA_O_DECISION"].ToString().Trim();
                CLA_O_DECISIONDATE = Row["CLA_O_DECISIONDATE"].ToString().Trim();
                CLA_O_APPEAL = Row["CLA_O_APPEAL"].ToString().Trim();
                CLA_O_VOUCHER = Row["CLA_O_VOUCHER"].ToString().Trim();
                CLA_O_FOLL_DATE = Row["CLA_O_FOLL_DATE"].ToString().Trim();
                CLA_O_FOLL_CDATE = Row["CLA_O_FOLL_CDATE"].ToString().Trim();
                CLA_O_BENPERD_START = Row["CLA_O_BENPERD_START"].ToString().Trim();
                CLA_O_BENPERD_END = Row["CLA_O_BENPERD_END"].ToString().Trim();
                CLA_N_CASEWORKER = Row["CLA_N_CASEWORKER"].ToString().Trim();
                CLA_N_SERVICE_CODE = Row["CLA_N_SERVICE_CODE"].ToString().Trim();
                CLA_N_VENDOR = Row["CLA_N_VENDOR"].ToString().Trim();
                CLA_N_ACCT = Row["CLA_N_ACCT"].ToString().Trim();
                CLA_N_BIL_LNAME = Row["CLA_N_BIL_LNAME"].ToString().Trim();
                CLA_N_BIL_FNAME = Row["CLA_N_BIL_FNAME"].ToString().Trim();
                CLA_N_DECISION = Row["CLA_N_DECISION"].ToString().Trim();
                CLA_N_DECISIONDATE = Row["CLA_N_DECISIONDATE"].ToString().Trim();
                CLA_N_APPEAL = Row["CLA_N_APPEAL"].ToString().Trim();
                CLA_N_VOUCHER = Row["CLA_N_VOUCHER"].ToString().Trim();
                CLA_N_FOLL_DATE = Row["CLA_N_FOLL_DATE"].ToString().Trim();
                CLA_N_FOLL_CDATE = Row["CLA_N_FOLL_CDATE"].ToString().Trim();
                CLA_N_BENPERD_START = Row["CLA_N_BENPERD_START"].ToString().Trim();
                CLA_N_BENPERD_END = Row["CLA_N_BENPERD_END"].ToString().Trim();
                PMA_O_TYPE = Row["PMA_O_TYPE"].ToString().Trim();
                PMA_O_AMOUNT = Row["PMA_O_AMOUNT"].ToString().Trim();
                PMA_O_AUTH_FOOD_VOUCHER = Row["PMA_O_AUTH_FOOD_VOUCHER"].ToString().Trim();
                PMA_O_AUTH_LIQUIDATE = Row["PMA_O_AUTH_LIQUIDATE"].ToString().Trim();
                PMA_O_AUTH_AMOUNT = Row["PMA_O_AUTH_AMOUNT"].ToString().Trim();
                PMA_O_AUTH_WORKER = Row["PMA_O_AUTH_WORKER"].ToString().Trim();
                PMA_O_AUTH_DATE = Row["PMA_O_AUTH_DATE"].ToString().Trim();
                PMA_O_INV_AMOUNT = Row["PMA_O_INV_AMOUNT"].ToString().Trim();
                PMA_O_INV_BILL_AMOUNT = Row["PMA_O_INV_BILL_AMOUNT"].ToString().Trim();
                PMA_O_INV_BILL_DATE = Row["PMA_O_INV_BILL_DATE"].ToString().Trim();
                PMA_O_INV_VENDOR_RATING = Row["PMA_O_INV_VENDOR_RATING"].ToString().Trim();
                PMA_O_INV_WORKER = Row["PMA_O_INV_WORKER"].ToString().Trim();
                PMA_O_INV_DATE = Row["PMA_O_INV_DATE"].ToString().Trim();
                PMA_O_CHECK_DATE = Row["PMA_O_CHECK_DATE"].ToString().Trim();
                PMA_O_CHECK_NO = Row["PMA_O_CHECK_NO"].ToString().Trim();
                PMA_O_FILE_NAME = Row["PMA_O_FILE_NAME"].ToString().Trim();
                PMA_N_TYPE = Row["PMA_N_TYPE"].ToString().Trim();
                PMA_N_AMOUNT = Row["PMA_N_AMOUNT"].ToString().Trim();
                PMA_N_AUTH_FOOD_VOUCHER = Row["PMA_N_AUTH_FOOD_VOUCHER"].ToString().Trim();
                PMA_N_AUTH_LIQUIDATE = Row["PMA_N_AUTH_LIQUIDATE"].ToString().Trim();
                PMA_N_AUTH_AMOUNT = Row["PMA_N_AUTH_AMOUNT"].ToString().Trim();
                PMA_N_AUTH_WORKER = Row["PMA_N_AUTH_WORKER"].ToString().Trim();
                PMA_N_AUTH_DATE = Row["PMA_N_AUTH_DATE"].ToString().Trim();
                PMA_N_INV_AMOUNT = Row["PMA_N_INV_AMOUNT"].ToString().Trim();
                PMA_N_INV_BILL_AMOUNT = Row["PMA_N_INV_BILL_AMOUNT"].ToString().Trim();
                PMA_N_INV_BILL_DATE = Row["PMA_N_INV_BILL_DATE"].ToString().Trim();
                PMA_N_INV_VENDOR_RATING = Row["PMA_N_INV_VENDOR_RATING"].ToString().Trim();
                PMA_N_INV_WORKER = Row["PMA_N_INV_WORKER"].ToString().Trim();
                PMA_N_INV_DATE = Row["PMA_N_INV_DATE"].ToString().Trim();
                PMA_N_CHECK_DATE = Row["PMA_N_CHECK_DATE"].ToString().Trim();
                PMA_N_CHECK_NO = Row["PMA_N_CHECK_NO"].ToString().Trim();
                PMA_N_FILE_NAME = Row["PMA_N_FILE_NAME"].ToString().Trim();
                RES_O_CASEWORKER = Row["RES_O_CASEWORKER"].ToString().Trim();
                RES_O_AMOUNT = Row["RES_O_AMOUNT"].ToString().Trim();
                RES_N_CASEWORKER = Row["RES_N_CASEWORKER"].ToString().Trim();
                RES_N_AMOUNT = Row["RES_N_AMOUNT"].ToString().Trim();
                CLA_DATE_ADD = Row["CLA_DATE_ADD"].ToString().Trim();
                CLA_ADD_OPERATOR = Row["CLA_ADD_OPERATOR"].ToString().Trim();
            }
        }

        #endregion

        #region Properties


        public string CLA_AGENCY { get; set; }
        public string CLA_DEPT { get; set; }
        public string CLA_PROGRAM { get; set; }
        public string CLA_YEAR { get; set; }
        public string CLA_APP { get; set; }
        public string CLA_RES_FUND { get; set; }
        public string CLA_RES_SEQ { get; set; }
        public string CLA_RES_DATE { get; set; }
        public string CLA_CLC_SEQ { get; set; }
        public string CLA_SEQ { get; set; }
        public string CLA_REASON { get; set; }
        public string CLA_ADJ_DATE { get; set; }
        public string CLA_O_CASEWORKER { get; set; }
        public string CLA_O_SERVICE_CODE { get; set; }
        public string CLA_O_VENDOR { get; set; }
        public string CLA_O_ACCT { get; set; }
        public string CLA_O_BIL_LNAME { get; set; }
        public string CLA_O_BIL_FNAME { get; set; }
        public string CLA_O_DECISION { get; set; }
        public string CLA_O_DECISIONDATE { get; set; }
        public string CLA_O_APPEAL { get; set; }
        public string CLA_O_VOUCHER { get; set; }
        public string CLA_O_FOLL_DATE { get; set; }
        public string CLA_O_FOLL_CDATE { get; set; }
        public string CLA_O_BENPERD_START { get; set; }
        public string CLA_O_BENPERD_END { get; set; }
        public string CLA_N_CASEWORKER { get; set; }
        public string CLA_N_SERVICE_CODE { get; set; }
        public string CLA_N_VENDOR { get; set; }
        public string CLA_N_ACCT { get; set; }
        public string CLA_N_BIL_LNAME { get; set; }
        public string CLA_N_BIL_FNAME { get; set; }
        public string CLA_N_DECISION { get; set; }
        public string CLA_N_DECISIONDATE { get; set; }
        public string CLA_N_APPEAL { get; set; }
        public string CLA_N_VOUCHER { get; set; }
        public string CLA_N_FOLL_DATE { get; set; }
        public string CLA_N_FOLL_CDATE { get; set; }
        public string CLA_N_BENPERD_START { get; set; }
        public string CLA_N_BENPERD_END { get; set; }
        public string PMA_O_TYPE { get; set; }
        public string PMA_O_AMOUNT { get; set; }
        public string PMA_O_AUTH_FOOD_VOUCHER { get; set; }
        public string PMA_O_AUTH_LIQUIDATE { get; set; }
        public string PMA_O_AUTH_AMOUNT { get; set; }
        public string PMA_O_AUTH_WORKER { get; set; }
        public string PMA_O_AUTH_DATE { get; set; }
        public string PMA_O_INV_AMOUNT { get; set; }
        public string PMA_O_INV_BILL_AMOUNT { get; set; }
        public string PMA_O_INV_BILL_DATE { get; set; }
        public string PMA_O_INV_VENDOR_RATING { get; set; }
        public string PMA_O_INV_WORKER { get; set; }
        public string PMA_O_INV_DATE { get; set; }
        public string PMA_O_CHECK_DATE { get; set; }
        public string PMA_O_CHECK_NO { get; set; }
        public string PMA_O_FILE_NAME { get; set; }
        public string PMA_N_TYPE { get; set; }
        public string PMA_N_AMOUNT { get; set; }
        public string PMA_N_AUTH_FOOD_VOUCHER { get; set; }
        public string PMA_N_AUTH_LIQUIDATE { get; set; }
        public string PMA_N_AUTH_AMOUNT { get; set; }
        public string PMA_N_AUTH_WORKER { get; set; }
        public string PMA_N_AUTH_DATE { get; set; }
        public string PMA_N_INV_AMOUNT { get; set; }
        public string PMA_N_INV_BILL_AMOUNT { get; set; }
        public string PMA_N_INV_BILL_DATE { get; set; }
        public string PMA_N_INV_VENDOR_RATING { get; set; }
        public string PMA_N_INV_WORKER { get; set; }
        public string PMA_N_INV_DATE { get; set; }
        public string PMA_N_CHECK_DATE { get; set; }
        public string PMA_N_CHECK_NO { get; set; }
        public string PMA_N_FILE_NAME { get; set; }
        public string RES_O_CASEWORKER { get; set; }
        public string RES_O_AMOUNT { get; set; }
        public string RES_N_CASEWORKER { get; set; }
        public string RES_N_AMOUNT { get; set; }
        public string CLA_DATE_ADD { get; set; }
        public string CLA_ADD_OPERATOR { get; set; }
        public string Mode { get; set; }

        #endregion

    }


    public class EMSOBOEntity
    {

        #region Constructors

        public EMSOBOEntity()
        {

            EMSOBO_ID = string.Empty;
            EMSOBO_SEQ = string.Empty;
            EMSOBO_CLIENT_ID = string.Empty;
            EMSOBO_FAM_SEQ = string.Empty;
        }

        public EMSOBOEntity(DataRow Row)
        {
            if (Row != null)
            {

                EMSOBO_ID = Row["EMSOBO_ID"].ToString().Trim();
                EMSOBO_SEQ = Row["EMSOBO_SEQ"].ToString().Trim();
                EMSOBO_CLIENT_ID = Row["EMSOBO_CLIENT_ID"].ToString().Trim();
                EMSOBO_FAM_SEQ = Row["EMSOBO_FAM_SEQ"].ToString().Trim();
            }
        }

        #endregion

        #region Properties



        public string EMSOBO_ID { get; set; }
        public string EMSOBO_SEQ { get; set; }
        public string EMSOBO_CLIENT_ID { get; set; }
        public string EMSOBO_FAM_SEQ { get; set; }
        public string Mode { get; set; }

        #endregion

    }


    public class EMSB0003Entity
    {

        #region Constructors

        public EMSB0003Entity()
        {
            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            Site = string.Empty;
            CaseWorker = string.Empty;
            DECSN_DATE = string.Empty;
            CLC_APP = string.Empty;
            CLC_S_CGN = string.Empty;
            LastName = string.Empty;
            Fname = string.Empty;
            MName = string.Empty;
            SERVICE_CODE = string.Empty;
            FUND = string.Empty;
            CLC_S_DECISION = string.Empty;
            PMC_AMOUNT = string.Empty;
            CLC_S_FOL_DATE = string.Empty;
        }

        public EMSB0003Entity(DataRow Row)
        {
            if (Row != null)
            {

                Agency = Row["CLC_AGENCY"].ToString().Trim();
                Dept = Row["CLC_DEPT"].ToString().Trim();
                Program = Row["CLC_PROGRAM"].ToString().Trim();
                Year = Row["CLC_YEAR"].ToString().Trim();
                Site = Row["MST_SITE"].ToString().Trim();
                CaseWorker = Row["CLC_S_CASEWORKER"].ToString().Trim();
                DECSN_DATE = Row["CLC_S_DECSN_DATE"].ToString().Trim();
                CLC_APP = Row["CLC_APP"].ToString().Trim();
                CLC_S_CGN = Row["CLC_S_CGN"].ToString().Trim();
                LastName = Row["SNP_NAME_IX_LAST"].ToString().Trim();
                Fname = Row["SNP_NAME_IX_FI"].ToString().Trim();
                MName = Row["SNP_NAME_IX_MI"].ToString().Trim();
                SERVICE_CODE = Row["CLC_S_SERVICE_CODE"].ToString().Trim();
                FUND = Row["CLC_RES_FUND"].ToString().Trim();
                CLC_S_DECISION = Row["CLC_S_DECISION"].ToString().Trim();
                PMC_AMOUNT = Row["PMC_AMOUNT"].ToString().Trim();
                CLC_S_FOL_DATE = Row["CLC_S_FOL_DATE"].ToString().Trim();
            }
        }

        public EMSB0003Entity(DataRow Row, string EMSB0018)
        {
            if (Row != null)
            {

                Agency = Row["CLC_AGENCY"].ToString().Trim();
                Dept = Row["CLC_DEPT"].ToString().Trim();
                Program = Row["CLC_PROGRAM"].ToString().Trim();
                Year = Row["CLC_YEAR"].ToString().Trim();
                Site = Row["MST_SITE"].ToString().Trim();
                CaseWorker = Row["CLC_S_CASEWORKER"].ToString().Trim();
                DECSN_DATE = Row["CLC_S_DECSN_DATE"].ToString().Trim();
                CLC_APP = Row["CLC_APP"].ToString().Trim();
                CLC_S_CGN = Row["CLC_S_CGN"].ToString().Trim();
                CLC_S_DECISION = Row["CLC_S_DECISION"].ToString().Trim();
                LastName = Row["SNP_NAME_IX_LAST"].ToString().Trim();
                Fname = Row["SNP_NAME_IX_FI"].ToString().Trim();
                MName = Row["SNP_NAME_IX_MI"].ToString().Trim();
                MST_HN = Row["MST_HN"].ToString().Trim();
                MST_APT = Row["MST_APT"].ToString().Trim();
                MST_FLR = Row["MST_FLR"].ToString().Trim();
                MST_STREET = Row["MST_STREET"].ToString().Trim();
                MST_SUFFIX = Row["MST_SUFFIX"].ToString().Trim();
                MST_CITY = Row["MST_CITY"].ToString().Trim();
                MST_STATE = Row["MST_STATE"].ToString().Trim();
                MST_ZIP = Row["MST_ZIP"].ToString().Trim();
                MST_ZIPPLUS = Row["MST_ZIPPLUS"].ToString().Trim();
            }
        }

        #endregion

        #region Properties


        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string Site { get; set; }
        public string CaseWorker { get; set; }
        public string DECSN_DATE { get; set; }
        public string CLC_APP { get; set; }
        public string CLC_S_CGN { get; set; }
        public string LastName { get; set; }
        public string Fname { get; set; }
        public string MName { get; set; }
        public string SERVICE_CODE { get; set; }
        public string FUND { get; set; }
        public string CLC_S_DECISION { get; set; }
        public string PMC_AMOUNT { get; set; }
        public string CLC_S_FOL_DATE { get; set; }
        public string MST_HN { get; set; }
        public string MST_APT { get; set; }
        public string MST_FLR { get; set; }
        public string MST_STREET { get; set; }
        public string MST_SUFFIX { get; set; }
        public string MST_CITY { get; set; }
        public string MST_STATE { get; set; }
        public string MST_ZIP { get; set; }
        public string MST_ZIPPLUS { get; set; }


        #endregion

    }

    public class EMSB0012Entity
    {

        #region Constructors

        public EMSB0012Entity()
        {
            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            Site = string.Empty;
            CaseWorker = string.Empty;
            Close_lvl1_DATE = string.Empty;
            CLC_APP = string.Empty;
            CLC_S_CGN = string.Empty;
            LastName = string.Empty;
            Fname = string.Empty;
            MName = string.Empty;
            //SERVICE_CODE = string.Empty;
            FUND = string.Empty;
            PMC_DEPT_REJECT_DATE = string.Empty;
            PMC_AMOUNT = string.Empty;
            PMC_DATE = string.Empty;
            PMC_REJECT_CODE = string.Empty;
            PMC_REJECT1_CODE = string.Empty;
            PMC_REJECT2_CODE = string.Empty;
            PMC_REJECT3_CODE = string.Empty;
            PMC_REJECT4_CODE = string.Empty;
            PMC_REJECT5_CODE = string.Empty;
            PMC_TYPE = string.Empty;
        }

        public EMSB0012Entity(DataRow Row)
        {
            if (Row != null)
            {

                Agency = Row["PMC_AGENCY1"].ToString().Trim();
                Dept = Row["PMC_DEPT1"].ToString().Trim();
                Program = Row["PMC_PROGRAM1"].ToString().Trim();
                Year = Row["PMC_YEAR1"].ToString().Trim();
                Site = Row["MST_SITE"].ToString().Trim();
                CaseWorker = Row["PMC_CASEWORKER"].ToString().Trim();
                Close_lvl1_DATE = Row["PMC_CLOSE_LVL1_DATE"].ToString().Trim();
                CLC_APP = Row["CLC_APP"].ToString().Trim();
                CLC_S_CGN = Row["CLC_S_CGN"].ToString().Trim();
                LastName = Row["SNP_NAME_IX_LAST"].ToString().Trim();
                Fname = Row["SNP_NAME_IX_FI"].ToString().Trim();
                MName = Row["SNP_NAME_IX_MI"].ToString().Trim();
                //SERVICE_CODE = Row["CLC_S_SERVICE_CODE"].ToString().Trim();
                FUND = Row["CLC_RES_FUND"].ToString().Trim();
                PMC_DEPT_REJECT_DATE = Row["PMC_DEPT_REJECT_DATE"].ToString().Trim();
                PMC_AMOUNT = Row["PMC_AMOUNT"].ToString().Trim();
                PMC_DATE = Row["PMC_DATE"].ToString().Trim();
                PMC_REJECT_CODE = Row["PMC_REJECT_CODE"].ToString().Trim();
                PMC_REJECT1_CODE = Row["PMC_REJECT1_CODE"].ToString().Trim();
                PMC_REJECT2_CODE = Row["PMC_REJECT2_CODE"].ToString().Trim();
                PMC_REJECT3_CODE = Row["PMC_REJECT3_CODE"].ToString().Trim();
                PMC_REJECT4_CODE = Row["PMC_REJECT4_CODE"].ToString().Trim();
                PMC_REJECT5_CODE = Row["PMC_REJECT5_CODE"].ToString().Trim();
                PMC_TYPE = Row["PMC_TYPE"].ToString().Trim();

            }
        }

        #endregion

        #region Properties


        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string Site { get; set; }
        public string CaseWorker { get; set; }
        public string Close_lvl1_DATE { get; set; }
        public string CLC_APP { get; set; }
        public string CLC_S_CGN { get; set; }
        public string LastName { get; set; }
        public string Fname { get; set; }
        public string MName { get; set; }
        //public string SERVICE_CODE { get; set; }
        public string FUND { get; set; }
        public string PMC_DEPT_REJECT_DATE { get; set; }
        public string PMC_AMOUNT { get; set; }
        public string PMC_DATE { get; set; }
        public string PMC_REJECT_CODE { get; set; }
        public string PMC_REJECT1_CODE { get; set; }
        public string PMC_REJECT2_CODE { get; set; }
        public string PMC_REJECT3_CODE { get; set; }
        public string PMC_REJECT4_CODE { get; set; }
        public string PMC_REJECT5_CODE { get; set; }
        public string PMC_TYPE { get; set; }


        #endregion

    }

    public class EMSB0011Entity
    {

        #region Constructors

        public EMSB0011Entity()
        {
            CLC_AGENCY = string.Empty;
            CLC_DEPT = string.Empty;
            CLC_PROGRAM = string.Empty;
            CLC_YEAR = string.Empty;
            CLC_FUND = string.Empty;
            BDC_BUDGET_YEAR = string.Empty;
            CLC_S_SERVICE_CODE = string.Empty;
            CLC_S_VENDOR = string.Empty;
            PMC_CLOSE_LVL2_DATE = string.Empty;
            LastName = string.Empty;
            Fname = string.Empty;
            MName = string.Empty;
            CASEVDD_NAME = string.Empty;
            CLC_APP = string.Empty;
            CLC_S_CGN = string.Empty;
            CLC_S_HEX_NO = string.Empty;
            CLC_S_ACCT = string.Empty;
            MST_INTAKE_DATE = string.Empty;
            MST_SITE = string.Empty;
            PMC_CHECK_NO = string.Empty;
            PMC_CHECK_DATE = string.Empty;
            PMC_TYPE = string.Empty;
            PMC_CASEWORKER = string.Empty;
            PMC_AMOUNT = string.Empty;
            BDC_COST_CENTER = string.Empty;
            BDC_GL_ACCOUNT = string.Empty;
        }

        public EMSB0011Entity(DataRow Row)
        {
            if (Row != null)
            {
                CLC_AGENCY = Row["CLC_AGENCY"].ToString().Trim();
                CLC_DEPT = Row["CLC_DEPT"].ToString().Trim();
                CLC_PROGRAM = Row["CLC_PROGRAM"].ToString().Trim();
                CLC_YEAR = Row["CLC_YEAR"].ToString().Trim();
                CLC_FUND = Row["CLC_RES_FUND"].ToString().Trim();
                BDC_BUDGET_YEAR = Row["BDC_BUDGET_YEAR"].ToString().Trim();
                CLC_S_SERVICE_CODE = Row["CLC_S_SERVICE_CODE"].ToString().Trim();
                CLC_S_VENDOR = Row["CLC_S_VENDOR"].ToString().Trim();
                PMC_CLOSE_LVL2_DATE = Row["PMC_CLOSE_LVL2_DATE"].ToString().Trim();
                LastName = Row["SNP_NAME_IX_LAST"].ToString().Trim();
                Fname = Row["SNP_NAME_IX_FI"].ToString().Trim();
                MName = Row["SNP_NAME_IX_MI"].ToString().Trim();
                CASEVDD_NAME = Row["CASEVDD_NAME"].ToString().Trim();
                CLC_APP = Row["CLC_APP"].ToString().Trim();
                CLC_S_CGN = Row["CLC_S_CGN"].ToString().Trim();
                CLC_S_HEX_NO = Row["CLC_S_HEX_NO"].ToString().Trim();
                CLC_S_ACCT = Row["CLC_S_ACCT"].ToString().Trim();
                MST_INTAKE_DATE = Row["MST_INTAKE_DATE"].ToString().Trim();
                MST_SITE = Row["MST_SITE"].ToString().Trim();
                PMC_CHECK_NO = Row["PMC_CHECK_NO"].ToString().Trim();
                PMC_CHECK_DATE = Row["PMC_CHECK_DATE"].ToString().Trim();
                PMC_TYPE = Row["PMC_TYPE"].ToString().Trim();
                PMC_CASEWORKER = Row["PMC_CASEWORKER"].ToString().Trim();
                PMC_AMOUNT = Row["PMC_AMOUNT"].ToString().Trim();
                BDC_COST_CENTER = Row["BDC_COST_CENTER"].ToString().Trim();
                BDC_GL_ACCOUNT = Row["BDC_GL_ACCOUNT"].ToString().Trim();

            }


        }
        public EMSB0011Entity(DataRow Row, string Rep)
        {
            if (Row != null)
            {
                CLC_FUND = Row["CLC_RES_FUND"].ToString().Trim();
                BDC_BUDGET_YEAR = Row["BDC_BUDGET_YEAR"].ToString().Trim();
                CLC_S_SERVICE_CODE = Row["CLC_S_SERVICE_CODE"].ToString().Trim();
                PMC_CLOSE_LVL2_DATE = Row["DATE"].ToString().Trim();
                PMC_AMOUNT = Row["Amount"].ToString().Trim();
            }
        }

        #endregion

        #region Properties


        public string CLC_AGENCY { get; set; }
        public string CLC_DEPT { get; set; }
        public string CLC_PROGRAM { get; set; }
        public string CLC_YEAR { get; set; }
        public string CLC_FUND { get; set; }
        public string BDC_BUDGET_YEAR { get; set; }
        public string CLC_S_SERVICE_CODE { get; set; }
        public string CLC_S_VENDOR { get; set; }
        public string PMC_CLOSE_LVL2_DATE { get; set; }
        public string LastName { get; set; }
        public string Fname { get; set; }
        public string MName { get; set; }
        public string CASEVDD_NAME { get; set; }
        public string CLC_APP { get; set; }
        public string CLC_S_CGN { get; set; }
        public string CLC_S_HEX_NO { get; set; }
        public string CLC_S_ACCT { get; set; }
        public string MST_INTAKE_DATE { get; set; }
        public string MST_SITE { get; set; }
        public string PMC_CHECK_NO { get; set; }
        public string PMC_CHECK_DATE { get; set; }
        public string PMC_TYPE { get; set; }
        public string PMC_CASEWORKER { get; set; }
        public string PMC_AMOUNT { get; set; }
        public string BDC_COST_CENTER { get; set; }
        public string BDC_GL_ACCOUNT { get; set; }


        #endregion

    }

    public class EMSB0021SumEntity
    {

        #region Constructors

        public EMSB0021SumEntity()
        {

            Fund = string.Empty;
            Site = string.Empty;
            Desc = string.Empty;
            Granted = string.Empty;
            Denied = string.Empty;
            Presets = string.Empty;
            Res_Amount = string.Empty;
            Paid_Amount = string.Empty;
            Balance = string.Empty;
            Committed = string.Empty;
        }

        public EMSB0021SumEntity(DataRow Row)
        {
            if (Row != null)
            {
                Fund = Row["CLC_RES_FUND"].ToString().Trim();
                Site = Row["MST_SITE"].ToString().Trim();
                Desc = Row["DESCR"].ToString().Trim();
                Granted = Row["Granted"].ToString().Trim();
                Denied = Row["Denied"].ToString().Trim();
                Presets = Row["Presets"].ToString().Trim();
                Res_Amount = Row["RESAMOUNT"].ToString().Trim();
                Paid_Amount = Row["PAIDAMOUNT"].ToString().Trim();
                Balance = Row["balance"].ToString().Trim();
                Committed = Row["commited"].ToString().Trim();


            }
        }

        #endregion

        #region Properties


        public string Fund { get; set; }
        public string Site { get; set; }
        public string Desc { get; set; }
        public string Granted { get; set; }
        public string Denied { get; set; }
        public string Presets { get; set; }
        public string Res_Amount { get; set; }
        public string Paid_Amount { get; set; }
        public string Balance { get; set; }
        public string Committed { get; set; }


        #endregion

    }

    public class EMSB0024Entity
    {

        #region Constructors

        public EMSB0024Entity()
        {
            MST_AGENCY = string.Empty;
            MST_DEPT = string.Empty;
            MST_PROGRAM = string.Empty;
            MST_YEAR = string.Empty;
            MST_APP_NO = string.Empty;
            MST_SITE = string.Empty;
            CLC_S_SERVICE_CODE = string.Empty;
            CLC_S_VENDOR = string.Empty;
            CLC_S_CASEWORKER = string.Empty;
            CLC_S_VENDOR = string.Empty;
            CLC_S_CGN = string.Empty;
            LastName = string.Empty;
            Fname = string.Empty;
            MName = string.Empty;
            CLC_S_DECISION = string.Empty;
            CLC_S_DECSN_DATE = string.Empty;
            CLC_S_COST_CENTER = string.Empty;
            CLC_S_GL_ACCOUNT = string.Empty;
            CLC_S_COUNTY_YEAR = string.Empty;
            BDC_ACCOUNT_TYPE = string.Empty;

            BDC_START = string.Empty;
            BDC_END = string.Empty;
            CLA_REASON = string.Empty;
            CLA_ADD_OPERATOR = string.Empty;
            CLA_DATE_ADD = string.Empty;
            CLA_ADJ_DATE = string.Empty;
            RES_O_CASEWORKER = string.Empty;

            RES_N_CASEWORKER = string.Empty;
            RES_O_AMOUNT = string.Empty;
            RES_N_AMOUNT = string.Empty;
            CLA_O_CASEWORKER = string.Empty;
            CLA_N_CASEWORKER = string.Empty;
            CLA_O_SERVICE_CODE = string.Empty;
            CLA_N_SERVICE_CODE = string.Empty;
            CLA_O_VENDOR = string.Empty;
            CLA_N_VENDOR = string.Empty;
            CLA_O_ACCT = string.Empty;
            CLA_N_ACCT = string.Empty;
            CLA_O_BIL_LNAME = string.Empty;
            CLA_N_BIL_LNAME = string.Empty;
            CLA_O_BIL_FNAME = string.Empty;
            CLA_N_BIL_FNAME = string.Empty;
            CLA_O_DECISION = string.Empty;
            CLA_N_DECISION = string.Empty;
            CLA_O_DECISIONDATE = string.Empty;
            CLA_N_DECISIONDATE = string.Empty;
            CLA_O_APPEAL = string.Empty;
            CLA_N_APPEAL = string.Empty;
            CLA_O_VOUCHER = string.Empty;
            CLA_N_VOUCHER = string.Empty;
            CLA_O_FOLL_DATE = string.Empty;
            CLA_N_FOLL_DATE = string.Empty;
            CLA_O_FOLL_CDATE = string.Empty;
            CLA_N_FOLL_CDATE = string.Empty;

            CLA_O_BENPERD_START = string.Empty;
            CLA_N_BENPERD_START = string.Empty;
            CLA_O_BENPERD_END = string.Empty;
            CLA_N_BENPERD_END = string.Empty;
            PMA_O_TYPE = string.Empty;
            PMA_N_TYPE = string.Empty;
            PMA_O_INV_WORKER = string.Empty;
            PMA_N_INV_WORKER = string.Empty;
            PMA_O_INV_DATE = string.Empty;
            PMA_N_INV_DATE = string.Empty;
            PMA_O_AMOUNT = string.Empty;
            PMA_N_AMOUNT = string.Empty;
            PMA_O_CHECK_NO = string.Empty;
            PMA_N_CHECK_NO = string.Empty;
            PMA_O_CHECK_DATE = string.Empty;
            PMA_N_CHECK_DATE = string.Empty;
            PMA_O_FILE_NAME = string.Empty;
            PMA_N_FILE_NAME = string.Empty;
            SITE_NAME = string.Empty;
        }

        public EMSB0024Entity(DataRow Row)
        {
            if (Row != null)
            {

                MST_AGENCY = Row["MST_AGENCY"].ToString().Trim();
                MST_DEPT = Row["MST_DEPT"].ToString().Trim();
                MST_PROGRAM = Row["MST_PROGRAM"].ToString().Trim();
                MST_YEAR = Row["MST_YEAR"].ToString().Trim();
                MST_APP_NO = Row["MST_APP_NO"].ToString().Trim();
                MST_SITE = Row["MST_SITE"].ToString().Trim();
                CLC_S_SERVICE_CODE = Row["CLC_S_SERVICE_CODE"].ToString().Trim();
                CLC_S_VENDOR = Row["CLC_S_VENDOR"].ToString().Trim();
                CLC_S_CASEWORKER = Row["CLC_S_CASEWORKER"].ToString().Trim();
                CLC_S_VENDOR = Row["CLC_S_VENDOR"].ToString().Trim();
                CLC_S_CGN = Row["CLC_S_CGN"].ToString().Trim();
                LastName = Row["SNP_NAME_IX_LAST"].ToString().Trim();
                Fname = Row["SNP_NAME_IX_FI"].ToString().Trim();
                MName = Row["SNP_NAME_IX_MI"].ToString().Trim();
                CLC_S_DECISION = Row["CLC_S_DECISION"].ToString().Trim();
                CLC_S_DECSN_DATE = Row["CLC_S_DECSN_DATE"].ToString().Trim();
                CLC_S_COST_CENTER = Row["CLC_S_COST_CENTER"].ToString().Trim();
                CLC_S_GL_ACCOUNT = Row["CLC_S_GL_ACCOUNT"].ToString().Trim();
                CLC_S_COUNTY_YEAR = Row["CLC_S_COUNTY_YEAR"].ToString().Trim();
                BDC_ACCOUNT_TYPE = Row["BDC_ACCOUNT_TYPE"].ToString().Trim();

                BDC_START = Row["BDC_START"].ToString().Trim();
                BDC_END = Row["BDC_END"].ToString().Trim();
                CLA_REASON = Row["CLA_REASON"].ToString().Trim();
                CLA_ADD_OPERATOR = Row["CLA_ADD_OPERATOR"].ToString().Trim();
                CLA_DATE_ADD = Row["CLA_DATE_ADD"].ToString().Trim();
                CLA_ADJ_DATE = Row["CLA_ADJ_DATE"].ToString().Trim();
                RES_O_CASEWORKER = Row["RES_O_CASEWORKER"].ToString().Trim();

                RES_N_CASEWORKER = Row["RES_N_CASEWORKER"].ToString().Trim();
                RES_O_AMOUNT = Row["RES_O_AMOUNT"].ToString().Trim();
                RES_N_AMOUNT = Row["RES_N_AMOUNT"].ToString().Trim();
                CLA_O_CASEWORKER = Row["CLA_O_CASEWORKER"].ToString().Trim();
                CLA_N_CASEWORKER = Row["CLA_N_CASEWORKER"].ToString().Trim();
                CLA_O_SERVICE_CODE = Row["CLA_O_SERVICE_CODE"].ToString().Trim();
                CLA_N_SERVICE_CODE = Row["CLA_N_SERVICE_CODE"].ToString().Trim();
                CLA_O_VENDOR = Row["CLA_O_VENDOR"].ToString().Trim();
                CLA_N_VENDOR = Row["CLA_N_VENDOR"].ToString().Trim();
                CLA_O_ACCT = Row["CLA_O_ACCT"].ToString().Trim();
                CLA_N_ACCT = Row["CLA_N_ACCT"].ToString().Trim();
                CLA_O_BIL_LNAME = Row["CLA_O_BIL_LNAME"].ToString().Trim();
                CLA_N_BIL_LNAME = Row["CLA_N_BIL_LNAME"].ToString().Trim();
                CLA_O_BIL_FNAME = Row["CLA_O_BIL_FNAME"].ToString().Trim();
                CLA_N_BIL_FNAME = Row["CLA_N_BIL_FNAME"].ToString().Trim();
                CLA_O_DECISION = Row["CLA_O_DECISION"].ToString().Trim();
                CLA_N_DECISION = Row["CLA_N_DECISION"].ToString().Trim();
                CLA_O_DECISIONDATE = Row["CLA_O_DECISIONDATE"].ToString().Trim();
                CLA_N_DECISIONDATE = Row["CLA_N_DECISIONDATE"].ToString().Trim();
                CLA_O_APPEAL = Row["CLA_O_APPEAL"].ToString().Trim();
                CLA_N_APPEAL = Row["CLA_N_APPEAL"].ToString().Trim();
                CLA_O_VOUCHER = Row["CLA_O_VOUCHER"].ToString().Trim();
                CLA_N_VOUCHER = Row["CLA_N_VOUCHER"].ToString().Trim();
                CLA_O_FOLL_DATE = Row["CLA_O_FOLL_DATE"].ToString().Trim();
                CLA_N_FOLL_DATE = Row["CLA_N_FOLL_DATE"].ToString().Trim();
                CLA_O_FOLL_CDATE = Row["CLA_O_FOLL_CDATE"].ToString().Trim();
                CLA_N_FOLL_CDATE = Row["CLA_N_FOLL_CDATE"].ToString().Trim();

                CLA_O_BENPERD_START = Row["CLA_O_BENPERD_START"].ToString().Trim();
                CLA_N_BENPERD_START = Row["CLA_N_BENPERD_START"].ToString().Trim();
                CLA_O_BENPERD_END = Row["CLA_O_BENPERD_END"].ToString().Trim();
                CLA_N_BENPERD_END = Row["CLA_N_BENPERD_END"].ToString().Trim();
                PMA_O_TYPE = Row["PMA_O_TYPE"].ToString().Trim();
                PMA_N_TYPE = Row["PMA_N_TYPE"].ToString().Trim();
                PMA_O_INV_WORKER = Row["PMA_O_INV_WORKER"].ToString().Trim();
                PMA_N_INV_WORKER = Row["PMA_N_INV_WORKER"].ToString().Trim();
                PMA_O_INV_DATE = Row["PMA_O_INV_DATE"].ToString().Trim();
                PMA_N_INV_DATE = Row["PMA_N_INV_DATE"].ToString().Trim();
                PMA_O_AMOUNT = Row["PMA_O_AMOUNT"].ToString().Trim();
                PMA_N_AMOUNT = Row["PMA_N_AMOUNT"].ToString().Trim();
                PMA_O_CHECK_NO = Row["PMA_O_CHECK_NO"].ToString().Trim();
                PMA_N_CHECK_NO = Row["PMA_N_CHECK_NO"].ToString().Trim();
                PMA_O_CHECK_DATE = Row["PMA_O_CHECK_DATE"].ToString().Trim();
                PMA_N_CHECK_DATE = Row["PMA_N_CHECK_DATE"].ToString().Trim();
                PMA_O_FILE_NAME = Row["PMA_O_FILE_NAME"].ToString().Trim();
                PMA_N_FILE_NAME = Row["PMA_N_FILE_NAME"].ToString().Trim();
                SITE_NAME = Row["SITE_NAME"].ToString().Trim();
            }
        }

        #endregion

        #region Properties


        public string MST_AGENCY { get; set; }
        public string MST_DEPT { get; set; }
        public string MST_PROGRAM { get; set; }
        public string MST_YEAR { get; set; }
        public string MST_APP_NO { get; set; }
        public string MST_SITE { get; set; }
        public string CLC_S_SERVICE_CODE { get; set; }
        public string CLC_S_VENDOR { get; set; }
        public string CLC_S_CASEWORKER { get; set; }
        public string CLC_S_CGN { get; set; }
        public string LastName { get; set; }
        public string Fname { get; set; }
        public string MName { get; set; }
        public string CLC_S_DECISION { get; set; }
        public string CLC_S_DECSN_DATE { get; set; }
        public string CLC_S_COST_CENTER { get; set; }
        public string CLC_S_GL_ACCOUNT { get; set; }
        public string CLC_S_COUNTY_YEAR { get; set; }
        public string BDC_ACCOUNT_TYPE { get; set; }

        public string BDC_START { get; set; }
        public string BDC_END { get; set; }
        public string CLA_REASON { get; set; }
        public string CLA_ADD_OPERATOR { get; set; }
        public string CLA_DATE_ADD { get; set; }
        public string CLA_ADJ_DATE { get; set; }
        public string RES_O_CASEWORKER { get; set; }

        public string RES_N_CASEWORKER { get; set; }
        public string RES_O_AMOUNT { get; set; }
        public string RES_N_AMOUNT { get; set; }
        public string CLA_O_CASEWORKER { get; set; }
        public string CLA_N_CASEWORKER { get; set; }
        public string CLA_O_SERVICE_CODE { get; set; }
        public string CLA_N_SERVICE_CODE { get; set; }
        public string CLA_O_VENDOR { get; set; }
        public string CLA_N_VENDOR { get; set; }
        public string CLA_O_ACCT { get; set; }
        public string CLA_N_ACCT { get; set; }
        public string CLA_O_BIL_LNAME { get; set; }
        public string CLA_N_BIL_LNAME { get; set; }
        public string CLA_O_BIL_FNAME { get; set; }
        public string CLA_N_BIL_FNAME { get; set; }
        public string CLA_O_DECISION { get; set; }
        public string CLA_N_DECISION { get; set; }
        public string CLA_O_DECISIONDATE { get; set; }
        public string CLA_N_DECISIONDATE { get; set; }
        public string CLA_O_APPEAL { get; set; }
        public string CLA_N_APPEAL { get; set; }
        public string CLA_O_VOUCHER { get; set; }
        public string CLA_N_VOUCHER { get; set; }
        public string CLA_O_FOLL_DATE { get; set; }
        public string CLA_N_FOLL_DATE { get; set; }
        public string CLA_O_FOLL_CDATE { get; set; }
        public string CLA_N_FOLL_CDATE { get; set; }

        public string CLA_O_BENPERD_START { get; set; }
        public string CLA_N_BENPERD_START { get; set; }
        public string CLA_O_BENPERD_END { get; set; }
        public string CLA_N_BENPERD_END { get; set; }
        public string PMA_O_TYPE { get; set; }
        public string PMA_N_TYPE { get; set; }
        public string PMA_O_INV_WORKER { get; set; }
        public string PMA_N_INV_WORKER { get; set; }
        public string PMA_O_INV_DATE { get; set; }
        public string PMA_N_INV_DATE { get; set; }
        public string PMA_O_AMOUNT { get; set; }
        public string PMA_N_AMOUNT { get; set; }
        public string PMA_O_CHECK_NO { get; set; }
        public string PMA_N_CHECK_NO { get; set; }
        public string PMA_O_CHECK_DATE { get; set; }
        public string PMA_N_CHECK_DATE { get; set; }
        public string PMA_O_FILE_NAME { get; set; }
        public string PMA_N_FILE_NAME { get; set; }

        public string SITE_NAME { get; set; }

        #endregion

    }

    public class EMSB0026Entity
    {

        #region Constructors

        public EMSB0026Entity()
        {
            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            AppNo = string.Empty;
            LastName = string.Empty;
            Fname = string.Empty;
            MName = string.Empty;
            Site = string.Empty;
            Res_Caseworker = string.Empty;
            CLC_S_Caseworker = string.Empty;
            PMC_Caseworker = string.Empty;
            PMC_TYPE = string.Empty;
            FUND = string.Empty;
            PMC_DATE = string.Empty;
            EMSRES_DATE = string.Empty;
            RES_Amount = string.Empty;
            PMC_Amount = string.Empty;
            Res_Seq = string.Empty;
            BDC_START = string.Empty;
            BDC_END = string.Empty;
            BDC_SWEEP_DAYS = string.Empty;
            Client = string.Empty;
            Balance = string.Empty;
            AwardAmount = string.Empty;
            PaidAmount = string.Empty;
            WorkerName = string.Empty;
            SweepStart = string.Empty;
            SweepEnd = string.Empty;

        }

        public EMSB0026Entity(EMSB0026Entity Entity)
        {
            Agency = Entity.Agency;
            Dept = Entity.Dept;
            Program = Entity.Program;
            Year = Entity.Year;
            AppNo = Entity.AppNo;
            LastName = Entity.LastName;
            Fname = Entity.Fname;
            MName = Entity.MName;
            Site = Entity.Site;
            Res_Caseworker = Entity.Res_Caseworker;
            CLC_S_Caseworker = Entity.CLC_S_Caseworker;
            PMC_Caseworker = Entity.PMC_Caseworker;
            PMC_TYPE = Entity.PMC_TYPE;
            FUND = Entity.FUND;
            PMC_DATE = Entity.PMC_DATE;
            EMSRES_DATE = Entity.EMSRES_DATE;
            RES_Amount = Entity.RES_Amount;
            PMC_Amount = Entity.PMC_Amount;
            Res_Seq = Entity.Res_Seq;
            BDC_START = Entity.BDC_START;
            BDC_END = Entity.BDC_END;
            BDC_SWEEP_DAYS = Entity.BDC_SWEEP_DAYS;
            Client = Entity.Client;
            Balance = Entity.Balance;
            AwardAmount = Entity.AwardAmount;
            PaidAmount = Entity.PaidAmount;
            WorkerName = Entity.WorkerName;
            SweepStart = Entity.SweepStart;
            SweepEnd = Entity.SweepEnd;

        }

        public EMSB0026Entity(DataRow Row)
        {
            if (Row != null)
            {

                Agency = Row["MST_AGENCY"].ToString().Trim();
                Dept = Row["MST_DEPT"].ToString().Trim();
                Program = Row["MST_PROGRAM"].ToString().Trim();
                Year = Row["MST_YEAR"].ToString().Trim();
                AppNo = Row["MST_APP_NO"].ToString().Trim();
                LastName = Row["SNP_NAME_IX_LAST"].ToString().Trim();
                Fname = Row["SNP_NAME_IX_FI"].ToString().Trim();
                MName = Row["SNP_NAME_IX_MI"].ToString().Trim();
                Site = Row["MST_SITE"].ToString().Trim();
                Res_Caseworker = Row["EMSRES_CASEWORKER"].ToString().Trim();
                //CLC_S_Caseworker = Row["CLC_S_CASEWORKER"].ToString().Trim();
                //PMC_Caseworker = Row["PMC_CASEWORKER"].ToString().Trim();
                //PMC_TYPE = Row["PMC_TYPE"].ToString().Trim();
                FUND = Row["EMSRES_FUND"].ToString().Trim();
                //PMC_DATE = Row["PMC_DATE"].ToString().Trim();
                EMSRES_DATE = Row["EMSRES_DATE"].ToString().Trim();
                RES_Amount = Row["EMSRES_AMOUNT"].ToString().Trim();
                //PMC_Amount = Row["PMC_AMOUNT"].ToString().Trim();
                Res_Seq = Row["EMSRES_SEQ"].ToString().Trim();
                BDC_START = Row["BDC_START"].ToString().Trim();
                BDC_END = Row["BDC_END"].ToString().Trim();
                BDC_SWEEP_DAYS = Row["BDC_SWEEP_DAYS"].ToString().Trim();


            }
        }
        public EMSB0026Entity(DataRow Row, string Table)
        {
            if (Row != null)
            {
                Site = Row["Site"].ToString().Trim();
                FUND = Row["Fund"].ToString().Trim();
                Res_Caseworker = Row["Worker"].ToString().Trim();
                Client = Row["Client"].ToString().Trim();
                EMSRES_DATE = Row["ResDate"].ToString().Trim();
                AppNo = Row["AppNo"].ToString().Trim();
                Balance = Row["Balance"].ToString().Trim();
                AwardAmount = Row["AwardAmount"].ToString().Trim();
                PaidAmount = Row["PaidAmount"].ToString().Trim();
                PMC_DATE = Row["PmcDate"].ToString().Trim();
                PMC_TYPE = Row["PMCType"].ToString().Trim();
                BDC_START = Row["BDCStart"].ToString().Trim();
                BDC_END = Row["BDCEnd"].ToString().Trim();
                CLC_S_Caseworker = Row["SP_Worker"].ToString().Trim();
                PMC_Caseworker = Row["P_Worker"].ToString().Trim();
                BDC_SWEEP_DAYS = Row["SweepDays"].ToString().Trim();
                SweepStart = Row["SweepStart"].ToString().Trim();
                SweepEnd = Row["SweepEnd"].ToString().Trim();
                //PMC_Amount = Row["PMC_AMOUNT"].ToString().Trim();
                Res_Seq = Row["ResSeq"].ToString().Trim();
                Agency = Row["Agency"].ToString().Trim();
                Dept = Row["Dept"].ToString().Trim();
                Program = Row["Program"].ToString().Trim();
                Year = Row["Res_year"].ToString().Trim();

            }
        }

        public EMSB0026Entity(DataRow Row, string Table, string strMode)
        {
            if (Row != null)
            {




                Agency = Row["MST_AGENCY"].ToString().Trim();
                Dept = Row["MST_DEPT"].ToString().Trim();
                Program = Row["MST_PROGRAM"].ToString().Trim();
                Year = Row["MST_YEAR"].ToString().Trim();
                AppNo = Row["MST_APP_NO"].ToString().Trim();
                LastName = Row["SNP_NAME_IX_LAST"].ToString().Trim();
                Fname = Row["SNP_NAME_IX_FI"].ToString().Trim();
                MName = Row["SNP_NAME_IX_MI"].ToString().Trim();
                Site = Row["MST_SITE"].ToString().Trim();
                Res_Caseworker = Row["EMSRES_CASEWORKER"].ToString().Trim();

                FUND = Row["EMSRES_FUND"].ToString().Trim();
                //PMC_DATE = Row["PMC_DATE"].ToString().Trim();
                EMSRES_DATE = Row["EMSRES_DATE"].ToString().Trim();
                RES_Amount = Row["EMSRES_AMOUNT"].ToString().Trim();
                //PMC_Amount = Row["PMC_AMOUNT"].ToString().Trim();
                Res_Seq = Row["EMSRES_SEQ"].ToString().Trim();
                BDC_START = Row["BDC_START"].ToString().Trim();
                BDC_END = Row["BDC_END"].ToString().Trim();
                BDC_SWEEP_DAYS = Row["BDC_SWEEP_DAYS"].ToString().Trim();
                Balance = Row["BALANCE"].ToString();
                PaidAmount = Row["PMC_AMOUNT"].ToString();
            }
        }

        #endregion

        #region Properties


        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string AppNo { get; set; }
        public string LastName { get; set; }
        public string Fname { get; set; }
        public string MName { get; set; }
        public string Site { get; set; }
        public string Res_Caseworker { get; set; }
        public string CLC_S_Caseworker { get; set; }
        public string PMC_Caseworker { get; set; }
        public string FUND { get; set; }
        public string PMC_TYPE { get; set; }
        public string PMC_DATE { get; set; }
        public string EMSRES_DATE { get; set; }
        public string RES_Amount { get; set; }
        public string PMC_Amount { get; set; }
        public string Res_Seq { get; set; }
        public string BDC_START { get; set; }
        public string BDC_END { get; set; }
        public string BDC_SWEEP_DAYS { get; set; }
        public string Client { get; set; }
        public string Balance { get; set; }
        public string AwardAmount { get; set; }
        public string PaidAmount { get; set; }
        public string WorkerName { get; set; }
        public string SweepStart { get; set; }
        public string SweepEnd { get; set; }
        //public string PMC_REJECT5_CODE { get; set; }
        //public string PMC_TYPE { get; set; }


        #endregion

    }

    public class CASEVOTEntity
    {
        #region Constructors

        public CASEVOTEntity()
        {
            ID = string.Empty;
            City = string.Empty;
            Street = string.Empty;
            Suffix = string.Empty;
            Block = string.Empty;
            Precinct = string.Empty;
            Direction = string.Empty;
            Zip = string.Empty;
            EO = string.Empty;

        }

        public CASEVOTEntity(bool Initialize)
        {
            if (Initialize)
            {
                ID = null;
                City = null;
                Street = null;
                Suffix = null;
                Block = null;
                Precinct = null;
                Direction = null;
                Zip = null;
                EO = null;
            }

        }


        public CASEVOTEntity(DataRow CASEVOT)
        {
            if (CASEVOT != null)
            {
                DataRow row = CASEVOT;


                ID = row["CASEVOT_ID"].ToString().Trim();
                City = row["CASEVOT_CITY"].ToString().Trim();
                Street = row["CASEVOT_STREET"].ToString().Trim();
                Suffix = row["CASEVOT_SUFFIX"].ToString().Trim();
                Block = row["CASEVOT_BLOCK"].ToString().Trim();
                Precinct = row["CASEVOT_PRECINCT"].ToString().Trim();
                Direction = row["CASEVOT_DIRECTION"].ToString().Trim();
                Zip = row["CASEVOT_ZIP"].ToString().Trim();
                EO = row["CASEVOT_EO"].ToString().Trim();

            }
        }

        public CASEVOTEntity(CASEVOTEntity Entity)
        {
            if (Entity != null)
            {
                ID = Entity.ID;
                City = Entity.City;
                Street = Entity.Street;
                Suffix = Entity.Suffix;
                Block = Entity.Block;
                Precinct = Entity.Precinct;
                Direction = Entity.Direction;
                Zip = Entity.Zip;
                EO = Entity.EO;


            }
        }


        #endregion

        #region Properties


        public string ID { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Suffix { get; set; }
        public string Block { get; set; }

        public string Precinct { get; set; }
        public string Direction { get; set; }
        public string Zip { get; set; }
        public string EO { get; set; }


        #endregion
    }

    public class EMSB0025Entity
    {

        #region Constructors

        public EMSB0025Entity()
        {
            CLC_AGENCY = string.Empty;
            CLC_DEPT = string.Empty;
            CLC_PROGRAM = string.Empty;
            CLC_YEAR = string.Empty;
            CLC_APP = string.Empty;
            CLC_RES_FUND = string.Empty;
            CLC_RES_SEQ = string.Empty;
            CLC_SEQ = string.Empty;
            CLC_RES_DATE = string.Empty;
            CLC_S_CGN = string.Empty;
            CLC_S_CASEWORKER = string.Empty;
            CLC_S_SERVICE_CODE = string.Empty;
            CLC_S_DECISION = string.Empty;
            CLC_S_DECSN_DATE = string.Empty;
            CLC_S_COST_CENTER = string.Empty;
            CLC_S_GL_ACCOUNT = string.Empty;
            CLC_S_COUNTY_YEAR = string.Empty;
            CLC_LSTC_OPERATOR = string.Empty;
            ResApp = string.Empty;
            BDC_COST_CENTER = string.Empty;
            BDC_GL_ACCOUNT = string.Empty;
            BDC_BUDGET_YEAR = string.Empty;
            BDC_FUND = string.Empty;
            BDC_START = string.Empty;
            BDC_END = string.Empty;
            BDC_LSTC_OPERATOR = string.Empty;

        }


        public EMSB0025Entity(DataRow Row)
        {
            if (Row != null)
            {
                CLC_AGENCY = Row["CLC_AGENCY"].ToString().Trim();
                CLC_DEPT = Row["CLC_DEPT"].ToString().Trim();
                CLC_PROGRAM = Row["CLC_PROGRAM"].ToString().Trim();
                CLC_YEAR = Row["CLC_YEAR"].ToString().Trim();
                CLC_APP = Row["CLC_APP"].ToString().Trim();
                CLC_RES_FUND = Row["CLC_RES_FUND"].ToString().Trim();
                CLC_RES_SEQ = Row["CLC_RES_SEQ"].ToString().Trim();
                CLC_SEQ = Row["CLC_SEQ"].ToString().Trim();
                CLC_RES_DATE = Row["CLC_RES_DATE"].ToString().Trim();
                CLC_S_CGN = Row["CLC_S_CGN"].ToString().Trim();
                CLC_S_CASEWORKER = Row["CLC_S_CASEWORKER"].ToString().Trim();
                CLC_S_SERVICE_CODE = Row["CLC_S_SERVICE_CODE"].ToString().Trim();
                CLC_S_DECISION = Row["CLC_S_DECISION"].ToString().Trim();
                CLC_S_DECSN_DATE = Row["CLC_S_DECSN_DATE"].ToString().Trim();
                CLC_S_COST_CENTER = Row["CLC_S_COST_CENTER"].ToString().Trim();
                CLC_S_GL_ACCOUNT = Row["CLC_S_GL_ACCOUNT"].ToString().Trim();
                CLC_S_COUNTY_YEAR = Row["CLC_S_COUNTY_YEAR"].ToString().Trim();
                CLC_LSTC_OPERATOR = Row["CLC_LSTC_OPERATOR"].ToString().Trim();

                ResApp = Row["EMSRES_APP"].ToString().Trim();
                BDC_COST_CENTER = Row["BDC_COST_CENTER"].ToString().Trim();
                BDC_GL_ACCOUNT = Row["BDC_GL_ACCOUNT"].ToString().Trim();
                BDC_BUDGET_YEAR = Row["BDC_BUDGET_YEAR"].ToString().Trim();
                BDC_FUND = Row["BDC_FUND"].ToString().Trim();
                BDC_START = Row["BDC_START"].ToString().Trim();
                BDC_END = Row["BDC_END"].ToString().Trim();
                BDC_LSTC_OPERATOR = Row["BDC_LSTC_OPERATOR"].ToString().Trim();

            }
        }

        #endregion

        #region Properties

        public string CLC_AGENCY { get; set; }
        public string CLC_DEPT { get; set; }
        public string CLC_PROGRAM { get; set; }
        public string CLC_YEAR { get; set; }
        public string CLC_APP { get; set; }
        public string CLC_RES_FUND { get; set; }
        public string CLC_RES_SEQ { get; set; }
        public string CLC_SEQ { get; set; }
        public string CLC_RES_DATE { get; set; }
        public string CLC_S_CGN { get; set; }
        public string CLC_S_CASEWORKER { get; set; }
        public string CLC_S_SERVICE_CODE { get; set; }
        public string CLC_S_DECISION { get; set; }
        public string CLC_S_DECSN_DATE { get; set; }
        public string CLC_S_COST_CENTER { get; set; }
        public string CLC_S_GL_ACCOUNT { get; set; }
        public string CLC_S_COUNTY_YEAR { get; set; }
        public string CLC_LSTC_OPERATOR { get; set; }

        public string ResApp { get; set; }

        public string BDC_COST_CENTER { get; set; }
        public string BDC_GL_ACCOUNT { get; set; }
        public string BDC_BUDGET_YEAR { get; set; }
        public string BDC_FUND { get; set; }
        public string BDC_START { get; set; }
        public string BDC_END { get; set; }
        public string BDC_LSTC_OPERATOR { get; set; }


        #endregion

    }


    public class EMSAPPEntity
    {

        #region Constructors

        public EMSAPPEntity()
        {
            APP_TAPPNO = string.Empty;
            APP_TSERVICE = string.Empty;
            APP_TSHTD = string.Empty;
            APP_TTYPE = string.Empty;
            APP_TBTCH = string.Empty;
            APP_TUSID = string.Empty;
            APP_TYR = string.Empty;
            APP_TAPMO = string.Empty;
            APP_TDYR = string.Empty;
            APP_TDAP = string.Empty;
            APP_TAPPL = string.Empty;
            APP_TCST = string.Empty;
            APP_TGAC = string.Empty;
            APP_TRND = string.Empty;
            APP_PONO = string.Empty;
            APP_PROJ = string.Empty;
            APP_TREF1 = string.Empty;
            APP_TREF2 = string.Empty;
            APP_TREF3 = string.Empty;
            APP_TREF4 = string.Empty;
            APP_TREF5 = string.Empty;
            APP_TREF6 = string.Empty;
            APP_TREF7 = string.Empty;
            APP_TREF8 = string.Empty;
            APP_TREF9 = string.Empty;
            APP_TVBC = string.Empty;
            APP_TCKNO = string.Empty;
            APP_TAMT1 = string.Empty;
            APP_TAMT2 = string.Empty;
            APP_TAMT3 = string.Empty;
            APP_TAMT4 = string.Empty;
            APP_TAMT5 = string.Empty;
            APP_TAMT6 = string.Empty;
            APP_TAMT7 = string.Empty;
            APP_TDAT1 = string.Empty;
            APP_TDAT2 = string.Empty;
            APP_TDAT3 = string.Empty;
            APP_TDAT4 = string.Empty;
            APP_TDAT5 = string.Empty;
            APP_TDAT6 = string.Empty;
            APP_TVNBR = string.Empty;
            APP_TVNAM = string.Empty;
            APP_TVAD1 = string.Empty;
            APP_TVAD2 = string.Empty;
            APP_TVAD3 = string.Empty;
            APP_TVCTY = string.Empty;
            APP_TVST = string.Empty;
            APP_TVZIP = string.Empty;
            APP_TAPCD = string.Empty;
            APP_TFACL = string.Empty;
            APP_TPRCS = string.Empty;
            APP_TWKOR = string.Empty;
            APP_TJOBNO = string.Empty;
            APP_TCRBN = string.Empty;
            APP_TCRDT = string.Empty;
            APP_TCRUS = string.Empty;

        }

        public EMSAPPEntity(EMSAPPEntity Entity)
        {
            APP_TAPPNO = Entity.APP_TAPPNO;
            APP_TSERVICE = Entity.APP_TSERVICE;
            APP_TSHTD = Entity.APP_TSHTD;
            APP_TTYPE = Entity.APP_TTYPE;
            APP_TBTCH = Entity.APP_TBTCH;
            APP_TUSID = Entity.APP_TUSID;
            APP_TYR = Entity.APP_TYR;
            APP_TAPMO = Entity.APP_TAPMO;
            APP_TDYR = Entity.APP_TDYR;
            APP_TDAP = Entity.APP_TDAP;
            APP_TAPPL = Entity.APP_TAPPL;
            APP_TCST = Entity.APP_TCST;
            APP_TGAC = Entity.APP_TGAC;
            APP_TRND = Entity.APP_TRND;
            APP_PONO = Entity.APP_PONO;
            APP_PROJ = Entity.APP_PROJ;
            APP_TREF1 = Entity.APP_TREF1;
            APP_TREF2 = Entity.APP_TREF2;
            APP_TREF3 = Entity.APP_TREF3;
            APP_TREF4 = Entity.APP_TREF4;
            APP_TREF5 = Entity.APP_TREF5;
            APP_TREF6 = Entity.APP_TREF6;
            APP_TREF7 = Entity.APP_TREF7;
            APP_TREF8 = Entity.APP_TREF8;
            APP_TREF9 = Entity.APP_TREF9;
            APP_TVBC = Entity.APP_TVBC;
            APP_TCKNO = Entity.APP_TCKNO;
            APP_TAMT1 = Entity.APP_TAMT1;
            APP_TAMT2 = Entity.APP_TAMT2;
            APP_TAMT3 = Entity.APP_TAMT3;
            APP_TAMT4 = Entity.APP_TAMT4;
            APP_TAMT5 = Entity.APP_TAMT5;
            APP_TAMT6 = Entity.APP_TAMT6;
            APP_TAMT7 = Entity.APP_TAMT6;
            APP_TDAT1 = Entity.APP_TDAT1;
            APP_TDAT2 = Entity.APP_TDAT2;
            APP_TDAT3 = Entity.APP_TDAT3;
            APP_TDAT4 = Entity.APP_TDAT4;
            APP_TDAT5 = Entity.APP_TDAT5;
            APP_TDAT6 = Entity.APP_TDAT6;
            APP_TVNBR = Entity.APP_TVNBR;
            APP_TVNAM = Entity.APP_TVNAM;
            APP_TVAD1 = Entity.APP_TVAD1;
            APP_TVAD2 = Entity.APP_TVAD2;
            APP_TVAD3 = Entity.APP_TVAD3;
            APP_TVCTY = Entity.APP_TVCTY;
            APP_TVST = Entity.APP_TVST;
            APP_TVZIP = Entity.APP_TVZIP;
            APP_TAPCD = Entity.APP_TAPCD;
            APP_TFACL = Entity.APP_TFACL;
            APP_TPRCS = Entity.APP_TPRCS;
            APP_TWKOR = Entity.APP_TWKOR;
            APP_TJOBNO = Entity.APP_TJOBNO;
            APP_TCRBN = Entity.APP_TCRBN;
            APP_TCRDT = Entity.APP_TCRDT;
            APP_TCRUS = Entity.APP_TCRUS;

        }

        //public EMSAPPEntity(DataRow Row)
        //{
        //    if (Row != null)
        //    {
        //        CLC_AGENCY = Row["CLC_AGENCY"].ToString().Trim();
        //        CLC_DEPT = Row["CLC_DEPT"].ToString().Trim();
        //        CLC_PROGRAM = Row["CLC_PROGRAM"].ToString().Trim();
        //        CLC_YEAR = Row["CLC_YEAR"].ToString().Trim();
        //        CLC_APP = Row["CLC_APP"].ToString().Trim();
        //        CLC_RES_FUND = Row["CLC_RES_FUND"].ToString().Trim();
        //        CLC_RES_SEQ = Row["CLC_RES_SEQ"].ToString().Trim();
        //        CLC_SEQ = Row["CLC_SEQ"].ToString().Trim();
        //        CLC_RES_DATE = Row["CLC_RES_DATE"].ToString().Trim();
        //        CLC_S_CGN = Row["CLC_S_CGN"].ToString().Trim();
        //        CLC_S_CASEWORKER = Row["CLC_S_CASEWORKER"].ToString().Trim();
        //        CLC_S_SERVICE_CODE = Row["CLC_S_SERVICE_CODE"].ToString().Trim();
        //        CLC_S_DECISION = Row["CLC_S_DECISION"].ToString().Trim();
        //        CLC_S_DECSN_DATE = Row["CLC_S_DECSN_DATE"].ToString().Trim();
        //        CLC_S_COST_CENTER = Row["CLC_S_COST_CENTER"].ToString().Trim();
        //        CLC_S_GL_ACCOUNT = Row["CLC_S_GL_ACCOUNT"].ToString().Trim();
        //        CLC_S_COUNTY_YEAR = Row["CLC_S_COUNTY_YEAR"].ToString().Trim();
        //        CLC_LSTC_OPERATOR = Row["CLC_LSTC_OPERATOR"].ToString().Trim();

        //        ResApp = Row["EMSRES_APP"].ToString().Trim();
        //        BDC_COST_CENTER = Row["BDC_COST_CENTER"].ToString().Trim();
        //        BDC_GL_ACCOUNT = Row["BDC_GL_ACCOUNT"].ToString().Trim();
        //        BDC_BUDGET_YEAR = Row["BDC_BUDGET_YEAR"].ToString().Trim();
        //        BDC_FUND = Row["BDC_FUND"].ToString().Trim();
        //        BDC_START = Row["BDC_START"].ToString().Trim();
        //        BDC_END = Row["BDC_END"].ToString().Trim();
        //        BDC_LSTC_OPERATOR = Row["BDC_LSTC_OPERATOR"].ToString().Trim();

        //    }
        //}

        #endregion

        #region Properties

        public string APP_TAPPNO { get; set; }
        public string APP_TSERVICE { get; set; }
        public string APP_TSHTD { get; set; }
        public string APP_TTYPE { get; set; }
        public string APP_TBTCH { get; set; }
        public string APP_TUSID { get; set; }
        public string APP_TYR { get; set; }
        public string APP_TAPMO { get; set; }
        public string APP_TDYR { get; set; }
        public string APP_TDAP { get; set; }
        public string APP_TAPPL { get; set; }
        public string APP_TCST { get; set; }
        public string APP_TGAC { get; set; }
        public string APP_TRND { get; set; }
        public string APP_PONO { get; set; }
        public string APP_PROJ { get; set; }
        public string APP_TREF1 { get; set; }
        public string APP_TREF2 { get; set; }
        public string APP_TREF3 { get; set; }
        public string APP_TREF4 { get; set; }
        public string APP_TREF5 { get; set; }
        public string APP_TREF6 { get; set; }
        public string APP_TREF7 { get; set; }
        public string APP_TREF8 { get; set; }
        public string APP_TREF9 { get; set; }
        public string APP_TVBC { get; set; }
        public string APP_TCKNO { get; set; }
        public string APP_TAMT1 { get; set; }
        public string APP_TAMT2 { get; set; }
        public string APP_TAMT3 { get; set; }
        public string APP_TAMT4 { get; set; }
        public string APP_TAMT5 { get; set; }
        public string APP_TAMT6 { get; set; }
        public string APP_TAMT7 { get; set; }
        public string APP_TDAT1 { get; set; }
        public string APP_TDAT2 { get; set; }
        public string APP_TDAT3 { get; set; }
        public string APP_TDAT4 { get; set; }
        public string APP_TDAT5 { get; set; }
        public string APP_TDAT6 { get; set; }
        public string APP_TVNBR { get; set; }
        public string APP_TVNAM { get; set; }
        public string APP_TVAD1 { get; set; }
        public string APP_TVAD2 { get; set; }
        public string APP_TVAD3 { get; set; }
        public string APP_TVCTY { get; set; }
        public string APP_TVST { get; set; }
        public string APP_TVZIP { get; set; }
        public string APP_TAPCD { get; set; }
        public string APP_TFACL { get; set; }
        public string APP_TPRCS { get; set; }
        public string APP_TWKOR { get; set; }
        public string APP_TJOBNO { get; set; }
        public string APP_TCRBN { get; set; }
        public string APP_TCRDT { get; set; }
        public string APP_TCRUS { get; set; }

        #endregion

    }

    public class EMSB0023ERROR_ENTITY
    {

        #region Constructors

        public EMSB0023ERROR_ENTITY()
        {
            ClientName = string.Empty;
            ApplNo = string.Empty;
            ServiceNo = string.Empty;
            Caba_Data = string.Empty;
            Audit_Data = string.Empty;
            Reason = string.Empty;

        }

        public EMSB0023ERROR_ENTITY(EMSB0023ERROR_ENTITY Entity)
        {

            ClientName = Entity.ClientName;
            ApplNo = Entity.ApplNo;
            ServiceNo = Entity.ServiceNo;
            Caba_Data = Entity.Caba_Data;
            Audit_Data = Entity.Audit_Data;
            Reason = Entity.Reason;

        }

        #endregion

        #region Properties

        public string ClientName { get; set; }
        public string ApplNo { get; set; }
        public string ServiceNo { get; set; }
        public string Caba_Data { get; set; }
        public string Audit_Data { get; set; }
        public string Reason { get; set; }

        #endregion

    }

    public class EMSB0027Entity
    {

        #region Constructors

        public EMSB0027Entity()
        {
            //CLC_AGENCY = string.Empty;
            MST_AGENCY = string.Empty;
            MST_DEPT = string.Empty;
            MST_PROGRAM = string.Empty;
            MST_YEAR = string.Empty;
            MST_APP_NO = string.Empty;
            SNP_NAME_IX_FI = string.Empty;
            SNP_NAME_IX_LAST = string.Empty;
            MST_SITE = string.Empty;
            MST_INTAKE_WORKER = string.Empty;
            MST_FAM_INCOME = string.Empty;
            MST_POVERTY = string.Empty;
            MST_INTAKE_DATE = string.Empty;
            SNP_ALT_BDATE = string.Empty;
            MST_FAMILY_SEQ = string.Empty;
            EMSRES_DATE = string.Empty;
            EMSRES_FUND = string.Empty;
            EMSRES_SEQ = string.Empty;
            CLC_S_DECISION = string.Empty;

            CLC_S_DECSN_DATE = string.Empty;
            CLC_S_CASEWORKER = string.Empty;
            CLC_S_SERVICE_CODE = string.Empty;
            CLC_RES_FUND = string.Empty;
            CLC_RES_SEQ = string.Empty;
            PMC_AMOUNT = string.Empty;
            BDC_FUND = string.Empty;
            BDC_START = string.Empty;

            BDC_END = string.Empty;
            PENDING_SW = string.Empty;
            PROGRESS_SW = string.Empty;
            COMPLETE_SW = string.Empty;
            DENIED_SW = string.Empty;

        }


        public EMSB0027Entity(DataRow Row)
        {
            if (Row != null)
            {
                MST_AGENCY = Row["MST_AGENCY"].ToString().Trim();
                MST_DEPT = Row["MST_DEPT"].ToString().Trim();
                MST_PROGRAM = Row["MST_PROGRAM"].ToString().Trim();
                MST_YEAR = Row["MST_YEAR"].ToString().Trim();
                MST_APP_NO = Row["MST_APP_NO"].ToString().Trim();
                SNP_NAME_IX_FI = Row["SNP_NAME_IX_FI"].ToString().Trim();
                SNP_NAME_IX_LAST = Row["SNP_NAME_IX_LAST"].ToString().Trim();
                MST_SITE = Row["MST_SITE"].ToString().Trim();
                MST_INTAKE_WORKER = Row["MST_INTAKE_WORKER"].ToString().Trim();
                MST_FAM_INCOME = Row["MST_FAM_INCOME"].ToString().Trim();
                MST_POVERTY = Row["MST_POVERTY"].ToString().Trim();
                MST_INTAKE_DATE = Row["MST_INTAKE_DATE"].ToString().Trim();
                SNP_ALT_BDATE = Row["SNP_ALT_BDATE"].ToString().Trim();
                MST_FAMILY_SEQ = Row["MST_FAMILY_SEQ"].ToString().Trim();
                EMSRES_DATE = Row["EMSRES_DATE"].ToString().Trim();
                EMSRES_FUND = Row["EMSRES_FUND"].ToString().Trim();
                EMSRES_SEQ = Row["EMSRES_SEQ"].ToString().Trim();
                CLC_S_DECISION = Row["CLC_S_DECISION"].ToString().Trim();

                CLC_S_DECSN_DATE = Row["CLC_S_DECSN_DATE"].ToString().Trim();
                CLC_S_CASEWORKER = Row["CLC_S_CASEWORKER"].ToString().Trim();
                CLC_S_SERVICE_CODE = Row["CLC_S_SERVICE_CODE"].ToString().Trim();
                CLC_RES_FUND = Row["CLC_RES_FUND"].ToString().Trim();
                CLC_RES_SEQ = Row["CLC_RES_SEQ"].ToString().Trim();
                PMC_AMOUNT = Row["PMC_AMOUNT"].ToString().Trim();
                BDC_FUND = Row["BDC_FUND"].ToString().Trim();
                BDC_START = Row["BDC_START"].ToString().Trim();

                BDC_END = Row["BDC_END"].ToString().Trim();
                PENDING_SW = Row["PENDING_SW"].ToString().Trim();
                PROGRESS_SW = Row["PROGRESS_SW"].ToString().Trim();
                COMPLETE_SW = Row["COMPLETE_SW"].ToString().Trim();
                DENIED_SW = Row["DENIED_SW"].ToString().Trim();


            }
        }

        #endregion

        #region Properties

        public string MST_AGENCY { get; set; }
        public string MST_DEPT { get; set; }
        public string MST_PROGRAM { get; set; }
        public string MST_YEAR { get; set; }
        public string MST_APP_NO { get; set; }
        public string SNP_NAME_IX_FI { get; set; }
        public string SNP_NAME_IX_LAST { get; set; }
        public string MST_SITE { get; set; }
        public string MST_INTAKE_WORKER { get; set; }
        public string MST_FAM_INCOME { get; set; }
        public string MST_POVERTY { get; set; }
        public string MST_INTAKE_DATE { get; set; }
        public string SNP_ALT_BDATE { get; set; }
        public string MST_FAMILY_SEQ { get; set; }
        //public string SNP_ETHNIC { get; set; }
        //public string SNP_DISABLE { get; set; }
        public string EMSRES_DATE { get; set; }
        public string EMSRES_FUND { get; set; }

        public string EMSRES_SEQ { get; set; }

        public string CLC_S_DECISION { get; set; }
        public string CLC_S_DECSN_DATE { get; set; }
        public string CLC_S_CASEWORKER { get; set; }
        public string CLC_S_SERVICE_CODE { get; set; }
        public string CLC_RES_FUND { get; set; }
        public string CLC_RES_SEQ { get; set; }
        public string PMC_AMOUNT { get; set; }

        public string BDC_FUND { get; set; }
        public string BDC_START { get; set; }
        public string BDC_END { get; set; }
        public string PENDING_SW { get; set; }
        public string PROGRESS_SW { get; set; }
        public string COMPLETE_SW { get; set; }
        public string DENIED_SW { get; set; }


        #endregion

    }


    public class EMSB0028Entity
    {

        #region Constructors

        public EMSB0028Entity()
        {
            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            AppNo = string.Empty;
            Site = string.Empty;
            MST_HN = string.Empty;
            MST_APT = string.Empty;
            MST_FLR = string.Empty;
            MST_STREET = string.Empty;
            MST_SUFFIX = string.Empty;
            MST_DIRECTION = string.Empty;
            MST_CITY = string.Empty;
            MST_STATE = string.Empty;
            MST_ZIP = string.Empty;
            MST_ZIPPLUS = string.Empty;
            CaseWorker = string.Empty;
            //DECSN_DATE = string.Empty;
            //CLC_APP = string.Empty;
            //CLC_S_CGN = string.Empty;

            County = string.Empty;
            MST_DWELLING = string.Empty;
            Poverty = string.Empty;
            ENERGY_BURDEN = string.Empty;
            MST_HOUSING = string.Empty;


            LastName = string.Empty;
            Fname = string.Empty;
            MName = string.Empty;
            SERVICE_CODE = string.Empty;
            FUND = string.Empty;
            MST_INTAKE_DATE = string.Empty;
            MST_FAMILY_SEQ = string.Empty;
            PMC_CLOSE_LVL2_DATE = string.Empty;
            PMC_INV_AMT = string.Empty;
            CLC_S_ACCT = string.Empty;
            BDC_START = string.Empty;
            BDC_END = string.Empty;
            BDC_CONTRACT = string.Empty;
            RESP129 = string.Empty;
            RESP135 = string.Empty;
            RESP139 = string.Empty;
            RESP134 = string.Empty;
            RESP140 = string.Empty;
            RESP137 = string.Empty;
            CASEVDD_NAME = string.Empty;

            RESTOT = string.Empty;
            //CLC_S_DECISION = string.Empty;
            //PMC_AMOUNT = string.Empty;
            //CLC_S_FOL_DATE = string.Empty;
        }

        //public EMSB0028Entity(DataRow Row)
        //{
        //    if (Row != null)
        //    {

        //        Agency = Row["CLC_AGENCY"].ToString().Trim();
        //        Dept = Row["CLC_DEPT"].ToString().Trim();
        //        Program = Row["CLC_PROGRAM"].ToString().Trim();
        //        Year = Row["CLC_YEAR"].ToString().Trim();
        //        Site = Row["MST_SITE"].ToString().Trim();
        //        CaseWorker = Row["CLC_S_CASEWORKER"].ToString().Trim();
        //        DECSN_DATE = Row["CLC_S_DECSN_DATE"].ToString().Trim();
        //        CLC_APP = Row["CLC_APP"].ToString().Trim();
        //        CLC_S_CGN = Row["CLC_S_CGN"].ToString().Trim();
        //        LastName = Row["SNP_NAME_IX_LAST"].ToString().Trim();
        //        Fname = Row["SNP_NAME_IX_FI"].ToString().Trim();
        //        MName = Row["SNP_NAME_IX_MI"].ToString().Trim();
        //        SERVICE_CODE = Row["CLC_S_SERVICE_CODE"].ToString().Trim();
        //        FUND = Row["CLC_RES_FUND"].ToString().Trim();
        //        CLC_S_DECISION = Row["CLC_S_DECISION"].ToString().Trim();
        //        PMC_AMOUNT = Row["PMC_AMOUNT"].ToString().Trim();
        //        CLC_S_FOL_DATE = Row["CLC_S_FOL_DATE"].ToString().Trim();
        //    }
        //}

        public EMSB0028Entity(DataRow Row, string Type)
        {
            if (Row != null)
            {

                Agency = Row["MST_AGENCY"].ToString().Trim();
                Dept = Row["MST_DEPT"].ToString().Trim();
                Program = Row["MST_PROGRAM"].ToString().Trim();
                Year = Row["MST_YEAR"].ToString().Trim();
                AppNo = Row["MST_APP_NO"].ToString().Trim();
                LastName = Row["SNP_NAME_IX_LAST"].ToString().Trim();
                Fname = Row["SNP_NAME_IX_FI"].ToString().Trim();
                MName = Row["SNP_NAME_IX_MI"].ToString().Trim();
                Site = Row["MST_SITE"].ToString().Trim();
                MST_HN = Row["MST_HN"].ToString().Trim();
                MST_APT = Row["MST_APT"].ToString().Trim();
                MST_FLR = Row["MST_FLR"].ToString().Trim();
                MST_STREET = Row["MST_STREET"].ToString().Trim();
                MST_SUFFIX = Row["MST_SUFFIX"].ToString().Trim();
                MST_DIRECTION = Row["MST_DIRECTION"].ToString().Trim();
                MST_CITY = Row["MST_CITY"].ToString().Trim();
                MST_STATE = Row["MST_STATE"].ToString().Trim();
                MST_ZIP = Row["MST_ZIP"].ToString().Trim();
                MST_ZIPPLUS = Row["MST_ZIPPLUS"].ToString().Trim();
                FUND = Row["CLC_RES_FUND"].ToString().Trim();
                SERVICE_CODE = Row["CLC_S_SERVICE_CODE"].ToString().Trim();
                CaseWorker = Row["CLC_S_CASEWORKER"].ToString().Trim();

                if (Type == "EMSB0028")
                {
                    County = Row["MST_COUNTY"].ToString().Trim();
                    MST_DWELLING = Row["MST_DWELLING"].ToString().Trim();
                    MST_HOUSING = Row["MST_HOUSING"].ToString().Trim();
                    Poverty = Row["MST_POVERTY"].ToString().Trim();
                    ENERGY_BURDEN = Row["ENERGY_BURDEN"].ToString().Trim();
                    RESTOT = Row["RESTOT"].ToString().Trim();
                }

                MST_INTAKE_DATE = Row["MST_INTAKE_DATE"].ToString().Trim();
                MST_FAMILY_SEQ = Row["MST_FAMILY_SEQ"].ToString().Trim();
                PMC_CLOSE_LVL2_DATE = Row["PMC_CLOSE_LVL2_DATE"].ToString().Trim();
                PMC_INV_AMT = Row["PMC_INV_AMT"].ToString().Trim();
                CLC_S_ACCT = Row["CLC_S_ACCT"].ToString().Trim();
                BDC_START = Row["BDC_START"].ToString().Trim();
                BDC_END = Row["BDC_END"].ToString().Trim();
                BDC_CONTRACT = Row["BDC_CONTRACT"].ToString().Trim();
                RESP129 = Row["RESP129"].ToString().Trim();
                RESP135 = Row["RESP135"].ToString().Trim();
                RESP139 = Row["RESP139"].ToString().Trim();
                RESP134 = Row["RESP134"].ToString().Trim();
                RESP140 = Row["RESP140"].ToString().Trim();
                RESP137 = Row["RESP137"].ToString().Trim();
                CASEVDD_NAME = Row["CASEVDD_NAME"].ToString().Trim();
                //DECSN_DATE = Row["CLC_S_DECSN_DATE"].ToString().Trim();
                //CLC_APP = Row["CLC_APP"].ToString().Trim();
                //CLC_S_CGN = Row["CLC_S_CGN"].ToString().Trim();
                //CLC_S_DECISION = Row["CLC_S_DECISION"].ToString().Trim();


            }
        }

        #endregion

        #region Properties


        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string AppNo { get; set; }
        public string LastName { get; set; }
        public string Fname { get; set; }
        public string MName { get; set; }
        public string Site { get; set; }
        public string MST_HN { get; set; }
        public string MST_APT { get; set; }
        public string MST_FLR { get; set; }
        public string MST_STREET { get; set; }
        public string MST_SUFFIX { get; set; }
        public string MST_DIRECTION { get; set; }
        public string MST_CITY { get; set; }
        public string MST_STATE { get; set; }
        public string MST_ZIP { get; set; }
        public string MST_ZIPPLUS { get; set; }
        public string FUND { get; set; }
        public string SERVICE_CODE { get; set; }
        public string CaseWorker { get; set; }

        public string County { get; set; }
        public string MST_DWELLING { get; set; }
        public string MST_HOUSING { get; set; }
        public string Poverty { get; set; }
        public string ENERGY_BURDEN { get; set; }

        public string MST_INTAKE_DATE { get; set; }
        public string MST_FAMILY_SEQ { get; set; }
        public string PMC_CLOSE_LVL2_DATE { get; set; }
        public string PMC_INV_AMT { get; set; }
        public string CLC_S_ACCT { get; set; }
        public string BDC_START { get; set; }
        public string BDC_END { get; set; }
        public string BDC_CONTRACT { get; set; }
        public string RESP129 { get; set; }
        public string RESP135 { get; set; }
        public string RESP139 { get; set; }
        public string RESP134 { get; set; }
        public string RESP140 { get; set; }
        public string RESP137 { get; set; }
        public string CASEVDD_NAME { get; set; }
        public string RESTOT { get; set; }


        //public string DECSN_DATE { get; set; }
        //public string CLC_APP { get; set; }
        //public string CLC_S_CGN { get; set; }



        //public string CLC_S_DECISION { get; set; }
        //public string PMC_AMOUNT { get; set; }
        //public string CLC_S_FOL_DATE { get; set; }



        #endregion

    }


    public class InvoiceLogEntity
    {

        #region Constructors

        public InvoiceLogEntity()
        {
            INVLOG_ID = string.Empty;
            INVLOG_AGENCY = string.Empty;
            INVLOG_DEPT = string.Empty;
            INVLOG_PROGRAM = string.Empty;
            INVLOG_APP = string.Empty;
            INVLOG_ORIG_NAME = string.Empty;
            INVLOG_UPLOAD_BY = string.Empty;
            INVLOG_DATE_UPLOAD = string.Empty;
            INVLOG_UPLOAD_AS = string.Empty;
            INVLOG_DELETED_BY = string.Empty;
            INVLOG_DATE_DELETED = string.Empty;
            INVLOG_LSTC_OPERATOR = string.Empty;
            INVLOG_DATE_LSTC = string.Empty;
            Mode = string.Empty;



        }


        public InvoiceLogEntity(DataRow Row)
        {
            if (Row != null)
            {
                Mode = string.Empty;
                INVLOG_ID = Row["INVLOG_ID"].ToString().Trim();
                INVLOG_AGENCY = Row["INVLOG_AGENCY"].ToString().Trim();
                INVLOG_DEPT = Row["INVLOG_DEPT"].ToString().Trim();
                INVLOG_PROGRAM = Row["INVLOG_PROGRAM"].ToString().Trim();
                INVLOG_APP = Row["INVLOG_APP"].ToString().Trim();
                INVLOG_ORIG_NAME = Row["INVLOG_ORIG_NAME"].ToString().Trim();
                INVLOG_UPLOAD_BY = Row["INVLOG_UPLOAD_BY"].ToString().Trim();
                INVLOG_DATE_UPLOAD = Row["INVLOG_DATE_UPLOAD"].ToString().Trim();
                INVLOG_UPLOAD_AS = Row["INVLOG_UPLOAD_AS"].ToString().Trim();
                INVLOG_DELETED_BY = Row["INVLOG_DELETED_BY"].ToString().Trim();
                INVLOG_DATE_DELETED = Row["INVLOG_DATE_DELETED"].ToString().Trim();
                INVLOG_DATE_LSTC = Row["INVLOG_DATE_LSTC"].ToString().Trim();
                INVLOG_LSTC_OPERATOR = Row["INVLOG_LSTC_OPERATOR"].ToString().Trim();
            }
        }

        #endregion

        #region Properties

        public string INVLOG_ID { get; set; }
        public string INVLOG_AGENCY { get; set; }
        public string INVLOG_DEPT { get; set; }
        public string INVLOG_PROGRAM { get; set; }
        public string INVLOG_APP { get; set; }
        public string INVLOG_ORIG_NAME { get; set; }
        public string INVLOG_UPLOAD_BY { get; set; }
        public string INVLOG_DATE_UPLOAD { get; set; }
        public string INVLOG_UPLOAD_AS { get; set; }
        public string INVLOG_DELETED_BY { get; set; }
        public string INVLOG_DATE_DELETED { get; set; }
        public string INVLOG_LSTC_OPERATOR { get; set; }
        public string INVLOG_DATE_LSTC { get; set; }
        public string Mode { get; set; }

        #endregion

    }

    public class CEAPB002Entity
    {

        #region Constructors

        public CEAPB002Entity()
        {
            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            AppNo = string.Empty;
            Site = string.Empty;
            MST_HN = string.Empty;
            MST_APT = string.Empty;
            MST_FLR = string.Empty;
            MST_STREET = string.Empty;
            MST_SUFFIX = string.Empty;
            MST_DIRECTION = string.Empty;
            MST_CITY = string.Empty;
            MST_STATE = string.Empty;
            MST_ZIP = string.Empty;
            MST_ZIPPLUS = string.Empty;
            CASEACT_CASEWRKR = string.Empty;

            County = string.Empty;
            MST_DWELLING = string.Empty;
            Poverty = string.Empty;
            CASEACT_ACTIVITY_CODE = string.Empty;
            MST_HOUSING = string.Empty;
            MST_FAMILY_TYPE = string.Empty;
            MST_INCOME_TYPES = string.Empty;
            MST_NONCASH_BENF = string.Empty;
            RNGSRGRP_GROUP_CODE = string.Empty;
            SRC_INC = string.Empty;
            RNGSRGRP_BENFRESN = string.Empty;
            MST_FAMILY_ID = string.Empty;

            LastName = string.Empty;
            Fname = string.Empty;
            MName = string.Empty;
            CASEACT_SERVICE_PLAN = string.Empty;
            CASEACT_FUND1 = string.Empty;
            MST_INTAKE_DATE = string.Empty;
            MST_FAMILY_SEQ = string.Empty;
            CASEACT_ACTY_DATE = string.Empty;
            CASEACT_CHECK_DT = string.Empty;
            CASEACT_COST = string.Empty;
            CASEACT_VEND_ACCT = string.Empty;
            BDC_START = string.Empty;
            BDC_END = string.Empty;
            CASEVDD_NAME = string.Empty;
            USAGE_PSOURCE = string.Empty;
            USAGE_SSOURCE = string.Empty;
            Annualcost = string.Empty;
            PSourceswitch = string.Empty;
            Eleccost = string.Empty;
            Elecswitch = string.Empty;
            CASEACT_SOURCE = string.Empty;
            MST_FAM_INCOME = string.Empty;
            MST_PROG_INCOME = string.Empty;
            USAGE_TOTAL = string.Empty;
            HIGHENERGY_BURDEN = string.Empty;
            Cust_Code1 = string.Empty;
            Cust_Value1 = string.Empty;
            Cust_Code2 = string.Empty;
            Cust_Value2 = string.Empty;
            Cust_Code3 = string.Empty;
            Cust_Value3 = string.Empty;
            Cust_Code4 = string.Empty;
            Cust_Value4 = string.Empty;
            Cust_Code5 = string.Empty;
            Cust_Value5 = string.Empty;
            BenefitReason = string.Empty;
            CASEACT_SEQ = string.Empty;
            CASEACT_DESC = string.Empty;
            BDC_Benefit_Type = string.Empty;
            BDC_Contract_Num = string.Empty;
            Bypassdata = string.Empty;
            BypassdataMsg = string.Empty;
            SNP_ALT_BDATE = string.Empty;
            SOURCEDESC = string.Empty;
            BENEFITREASONDESC = string.Empty;

        }

        public CEAPB002Entity(DataRow Row, string Type, string isCSBG)
        {
        if (Row != null)
        {

            Agency = Row["MST_AGENCY"].ToString().Trim();
            Dept = Row["MST_DEPT"].ToString().Trim();
            Program = Row["MST_PROGRAM"].ToString().Trim();
            Year = Row["MST_YEAR"].ToString().Trim();
            AppNo = Row["MST_APP_NO"].ToString().Trim();
            LastName = Row["SNP_NAME_IX_LAST"].ToString().Trim();
            Fname = Row["SNP_NAME_IX_FI"].ToString().Trim();
            MName = Row["SNP_NAME_IX_MI"].ToString().Trim();
            Site = Row["MST_SITE"].ToString().Trim();
            MST_HN = Row["MST_HN"].ToString().Trim();
            MST_APT = Row["MST_APT"].ToString().Trim();
            MST_FLR = Row["MST_FLR"].ToString().Trim();
            MST_STREET = Row["MST_STREET"].ToString().Trim();
            MST_SUFFIX = Row["MST_SUFFIX"].ToString().Trim();
            MST_DIRECTION = Row["MST_DIRECTION"].ToString().Trim();
            MST_CITY = Row["MST_CITY"].ToString().Trim();
            MST_STATE = Row["MST_STATE"].ToString().Trim();
            MST_ZIP = Row["MST_ZIP"].ToString().Trim();
            MST_ZIPPLUS = Row["MST_ZIPPLUS"].ToString().Trim();
            CASEACT_FUND1 = Row["CASEACT_FUND1"].ToString().Trim();
            CASEACT_SERVICE_PLAN = Row["CASEACT_SERVICE_PLAN"].ToString().Trim();
            CASEACT_CASEWRKR = Row["CASEACT_CASEWRKR"].ToString().Trim();

                if (Type == "CEAPB002")
                {
                    MST_County = Row["MST_COUNTY"].ToString().Trim();
                    County = Row["AGY_COUTYCODE"].ToString().Trim();
                    MST_DWELLING = Row["MST_DWELLING"].ToString().Trim();
                    MST_HOUSING = Row["MST_HOUSING"].ToString().Trim();
                    Poverty = Row["MST_POVERTY"].ToString().Trim();
                    // RESTOT = Row["RESTOT"].ToString().Trim();
                    SNP_Status = Row["SNP_STATUS"].ToString().Trim();

                    MST_FAMILY_TYPE = Row["MST_FAMILY_TYPE"].ToString().Trim();//Added by Vikash on 01/22/2025
                    if (isCSBG == "CSBG")
                    {
                        RNGSRGRP_GROUP_CODE = Row["RNGSRGRP_GROUP_CODE"].ToString().Trim();//Added by Vikash on 01/23/2025
                        MST_INCOME_TYPES = Row["MST_INCOME_TYPES"].ToString().Trim();//Added by Vikash on 01/29/2025

                        MST_NONCASH_BENF = Row["MST_NCASHBEN"].ToString().Trim();//Added by Vikash on 02/10/2025

                        SRC_INC = Row["SRCINC"].ToString().Trim();

                        RNGSRGRP_BENFRESN = Row["RNGSRGRP_BENFRESN"].ToString().Trim(); //Added by Vikash on 02/24/2025

                        MST_FAMILY_ID = Row["MST_FAMILY_ID"].ToString().Trim(); //Added by Vikash on 03/04/2025 as per CVCAA document
                    }
                }

            CASEACT_ACTIVITY_CODE = Row["CASEACT_ACTIVITY_CODE"].ToString().Trim();
            MST_INTAKE_DATE = Row["MST_INTAKE_DATE"].ToString().Trim();
            MST_FAMILY_SEQ = Row["MST_FAMILY_SEQ"].ToString().Trim();
            CASEACT_ACTY_DATE = Row["CASEACT_ACTY_DATE"].ToString().Trim();
            CASEACT_CHECK_DT = Row["CASEACT_CHECK_DT"].ToString().Trim();
            CASEACT_COST = Row["CASEACT_COST"].ToString().Trim();
            CASEACT_VEND_ACCT = Row["CASEACT_VEND_ACCT"].ToString().Trim();
            BDC_START = Row["BDC_START"].ToString().Trim();
            BDC_END = Row["BDC_END"].ToString().Trim();
            CASEVDD_NAME = Row["CASEVDD_NAME"].ToString().Trim();
            USAGE_PSOURCE = Row["USAGE_PSOURCE"].ToString().Trim();
            USAGE_SSOURCE = Row["USAGE_SSOURCE"].ToString().Trim();
            Annualcost = Row["Annualcost"].ToString().Trim();
            PSourceswitch = Row["PSourceswitch"].ToString().Trim();
            Eleccost = Row["Eleccost"].ToString().Trim();
            Elecswitch = Row["Elecswitch"].ToString().Trim();
            CASEACT_SOURCE = Row["CASEACT_SOURCE"].ToString().Trim();
            MST_FAM_INCOME = Row["MST_FAM_INCOME"].ToString().Trim();
            MST_PROG_INCOME = Row["MST_PROG_INCOME"].ToString().Trim();
            USAGE_TOTAL = Row["USAGE_TOTAL"].ToString().Trim();
            HIGHENERGY_BURDEN = Row["HIGHENERGY_BURDEN"].ToString().Trim();
            Cust_Code1 = Row["CASEACT_CUST1_CODE"].ToString().Trim();
            Cust_Value1 = Row["CASEACT_CUST1_VALUE"].ToString().Trim();
            Cust_Code2 = Row["CASEACT_CUST2_CODE"].ToString().Trim();
            Cust_Value2 = Row["CASEACT_CUST2_VALUE"].ToString().Trim();
            Cust_Code3 = Row["CASEACT_CUST3_CODE"].ToString().Trim();
            Cust_Value3 = Row["CASEACT_CUST3_VALUE"].ToString().Trim();
            Cust_Code4 = Row["CASEACT_CUST4_CODE"].ToString().Trim();
            Cust_Value4 = Row["CASEACT_CUST4_VALUE"].ToString().Trim();
            Cust_Code5 = Row["CASEACT_CUST5_CODE"].ToString().Trim();
            Cust_Value5 = Row["CASEACT_CUST5_VALUE"].ToString().Trim();
            BenefitReason = Row["BenefitReason"].ToString().Trim();
            CASEACT_SPM_SEQ = Row["CASEACT_SPM_SEQ"].ToString().Trim();
            CASEACT_BRANCH = Row["CASEACT_BRANCH"].ToString().Trim();
            CASEACT_GROUP = Row["CASEACT_GROUP"].ToString().Trim();
            CASEACT_SEQ = Row["CASEACT_SEQ"].ToString().Trim();
            CASEACT_DESC = Row["CA_DESC"].ToString().Trim();
            BDC_Benefit_Type = Row["BDC_BENEFIT_TYPE"].ToString().Trim();
            BDC_Contract_Num = Row["BDC_CONTR_NUM"].ToString().Trim();
            SNP_ALT_BDATE = Row["SNP_ALT_BDATE"].ToString().Trim();
            SOURCEDESC = Row["SOURCEDESC"].ToString().Trim();
            BENEFITREASONDESC = Row["BENEFITREASONDESC"].ToString().Trim();
            Bypassdata = string.Empty;
            BypassdataMsg = string.Empty;

        }
    }

#endregion

    #region Properties


    public string Agency { get; set; }
    public string Dept { get; set; }
    public string Program { get; set; }
    public string Year { get; set; }
    public string AppNo { get; set; }
    public string LastName { get; set; }
    public string Fname { get; set; }
    public string MName { get; set; }
    public string Site { get; set; }
    public string MST_HN { get; set; }
    public string MST_APT { get; set; }
    public string MST_FLR { get; set; }
    public string MST_STREET { get; set; }
    public string MST_SUFFIX { get; set; }
    public string MST_DIRECTION { get; set; }
    public string MST_CITY { get; set; }
    public string MST_STATE { get; set; }
    public string MST_ZIP { get; set; }
    public string MST_ZIPPLUS { get; set; }
    public string CASEACT_FUND1 { get; set; }
    public string CASEACT_SERVICE_PLAN { get; set; }
    public string CASEACT_CASEWRKR { get; set; }

    public string SNP_Status { get; set; }

    public string County { get; set; }
    public string MST_County { get; set; }
    public string MST_DWELLING { get; set; }
    public string MST_HOUSING { get; set; }
    public string Poverty { get; set; }
    public string CASEACT_ACTIVITY_CODE { get; set; }

    public string MST_INTAKE_DATE { get; set; }
    public string MST_FAMILY_SEQ { get; set; }
    public string CASEACT_ACTY_DATE { get; set; }
    public string CASEACT_CHECK_DT { get; set; }
    public string CASEACT_COST { get; set; }
    public string CASEACT_VEND_ACCT { get; set; }
    public string BDC_START { get; set; }
    public string BDC_END { get; set; }
    public string CASEVDD_NAME { get; set; }
    public string CASEACT_SOURCE { get; set; }


    public string USAGE_PSOURCE { get; set; }
    public string USAGE_SSOURCE { get; set; }
    public string Annualcost { get; set; }
    public string PSourceswitch { get; set; }
    public string Eleccost { get; set; }
    public string Elecswitch { get; set; }
    public string MST_FAM_INCOME { get; set; }
    public string MST_PROG_INCOME { get; set; }
    public string USAGE_TOTAL { get; set; }
    public string HIGHENERGY_BURDEN { get; set; }

    public string Cust_Code1 { get; set; }
    public string Cust_Value1 { get; set; }
    public string Cust_Code2 { get; set; }
    public string Cust_Value2 { get; set; }
    public string Cust_Code3 { get; set; }
    public string Cust_Value3 { get; set; }
    public string Cust_Code4 { get; set; }
    public string Cust_Value4 { get; set; }
    public string Cust_Code5 { get; set; }
    public string Cust_Value5 { get; set; }

    public string BenefitReason { get; set; }

    public string CASEACT_SPM_SEQ { get; set; }
    public string CASEACT_BRANCH { get; set; }
    public string CASEACT_GROUP { get; set; }
    public string CASEACT_SEQ { get; set; }

    public string CASEACT_DESC { get; set; }

    public string BDC_Benefit_Type { get; set; }
    public string BDC_Contract_Num { get; set; }
    public string Bypassdata { get; set; }
    public string BypassdataMsg { get; set; }
    public string SNP_ALT_BDATE { get; set; }
    public string SOURCEDESC { get; set; }
    public string BENEFITREASONDESC { get; set; }

        public string MST_FAMILY_TYPE
        {
            get;set;
        }
        public string RNGSRGRP_GROUP_CODE
        {
            get; set;
        }
        public string MST_INCOME_TYPES
        {
            get; set;
        }
        public string MST_NONCASH_BENF
        {
            get; set;
        }
        public string SRC_INC
        {
            get; set;
        }
        public string RNGSRGRP_BENFRESN
        {
            get; set;
        }

        public string MST_FAMILY_ID
        {
            get; set;
        }

        #endregion

    }
}
