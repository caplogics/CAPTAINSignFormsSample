using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Captain.Common.Model.Objects
{


    public class ChldAttnEntity
    {
        #region Constructors

        public ChldAttnEntity()
        {
            Rec_Type = string.Empty;
            AGENCY = string.Empty;
            DEPT = string.Empty;
            PROG = string.Empty;
            YEAR = string.Empty;
            APP_NO = string.Empty;
            SITE = string.Empty;
            ROOM = string.Empty;
            AMPM = string.Empty;
            FUNDING_SOURCE = string.Empty;
            DATE = string.Empty;
            PA = string.Empty;
            REASON = string.Empty;
            B = string.Empty;
            A = string.Empty;
            L = string.Empty;
            P = string.Empty;
            S = string.Empty;
            PARENT_RATE = string.Empty;
            FUNDING_RATE = string.Empty;
            HOURS = string.Empty;
            CATEGORY = string.Empty;
            CHARGE_CODE = string.Empty;
            MEAL = string.Empty;
            PARENT_INVOICE = string.Empty;
            LEGAL = string.Empty;
            PRES_DESC = string.Empty;
            DATE_ADD = string.Empty;
            ADD_OPERATOR = string.Empty;
            DATE_LSTC = string.Empty;
            LSTC_OPERATOR = string.Empty;
            Mode = string.Empty;


            Label = string.Empty;
            ChildName = string.Empty;
            Date = string.Empty;
            Day = string.Empty;
            Present = false;
            Apsent = false; ClReason = false;
            treason = string.Empty;
            billformeals = false;
            N = false;
            N1 = false;
            BoolB = false;
            BoolL = false;
            BoolP = false;
            Fundcode = string.Empty;
            Fund = string.Empty;
            ParentFees = string.Empty;
            FundFee = string.Empty;
            HoursSpent = string.Empty;
            Applicant = string.Empty;
            Reasoncode = string.Empty;
            Category = string.Empty;
            ChargeCode = string.Empty;
            BillMeals = string.Empty;
            Other = string.Empty;
            Balps = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            MiddleName = string.Empty;
            Status = string.Empty;
            TimeStart1 = string.Empty;
            TimeStart2 = string.Empty;
            TimeEnd1 = string.Empty;
            TimeEnd2 = string.Empty;
            TimeSum1 = string.Empty;
            TimeSum2 = string.Empty;

        }

        public ChldAttnEntity(bool Initialize)
        {
            AGENCY = null;
            DEPT = null;
            PROG = null;
            YEAR = null;
            APP_NO = null;
            SITE = null;
            ROOM = null;
            AMPM = null;
            FUNDING_SOURCE = null;
            DATE = null;
            PA = null;
            REASON = null;
            B = null;
            A = null;
            L = null;
            P = null;
            S = null;
            PARENT_RATE = null;
            FUNDING_RATE = null;
            HOURS = null;
            CATEGORY = null;
            CHARGE_CODE = null;
            MEAL = null;
            PARENT_INVOICE = null;
            LEGAL = null;
            PRES_DESC = null;
            DATE_ADD = null;
            ADD_OPERATOR = null;
            DATE_LSTC = null;
            LSTC_OPERATOR = null;
            Mode = null;
        }

        public ChldAttnEntity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                AGENCY = row["ATTN_AGENCY"].ToString().Trim();
                DEPT = row["ATTN_DEPT"].ToString().Trim();
                PROG = row["ATTN_PROGRAM"].ToString().Trim();
                YEAR = row["ATTN_YEAR"].ToString().Trim();
                APP_NO = row["ATTN_APP_NO"].ToString().Trim();
                SITE = row["ATTN_SITE"].ToString().Trim();
                ROOM = row["ATTN_ROOM"].ToString().Trim();
                AMPM = row["ATTN_AMPM"].ToString().Trim();
                FUNDING_SOURCE = row["ATTN_FUNDING_SOURCE"].ToString().Trim();
                DATE = row["ATTN_DATE"].ToString().Trim();
                PA = row["ATTN_PA"].ToString().Trim();
                REASON = row["ATTN_REASON"].ToString().Trim();
                B = row["ATTN_B"].ToString().Trim();
                A = row["ATTN_A"].ToString().Trim();
                L = row["ATTN_L"].ToString().Trim();
                P = row["ATTN_P"].ToString().Trim();
                S = row["ATTN_S"].ToString().Trim();
                PARENT_RATE = row["ATTN_PARENT_RATE"].ToString().Trim();
                FUNDING_RATE = row["ATTN_FUNDING_RATE"].ToString().Trim();
                HOURS = row["ATTN_HOURS"].ToString().Trim();
                CATEGORY = row["ATTN_CATEGORY"].ToString().Trim();
                CHARGE_CODE = row["ATTN_CHARGE_CODE"].ToString().Trim();
                MEAL = row["ATTN_MEAL"].ToString().Trim();
                PARENT_INVOICE = row["ATTN_PARENT_INVOICE"].ToString().Trim();
                LEGAL = row["ATTN_LEGAL"].ToString().Trim();
                PRES_DESC = row["ATTN_PRES_DESC"].ToString().Trim();
                DATE_ADD = row["ATTN_DATE_ADD"].ToString().Trim();
                ADD_OPERATOR = row["ATTN_ADD_OPERATOR"].ToString().Trim();
                DATE_LSTC = row["ATTN_DATE_LSTC"].ToString().Trim();
                LSTC_OPERATOR = row["ATTN_LSTC_OPERATOR"].ToString().Trim();
                TimeStart1 = row["ATTN_TIMESTART1"].ToString().Trim();
                TimeStart2 = row["ATTN_TIMESTART2"].ToString().Trim();
                TimeEnd1 = row["ATTN_TIMEEND1"].ToString().Trim();
                TimeEnd2 = row["ATTN_TIMEEND2"].ToString().Trim();
                TimeSum1 = row["ATTN_TIMESUM1"].ToString().Trim();
                TimeSum2 = row["ATTN_TIMESUM2"].ToString().Trim();
                Mode = string.Empty;
            }
        }

        public ChldAttnEntity(DataRow row, string strTable)
        {
            if (row != null)
            {
                if (strTable == "HSSB2109Count")
                {
                    Rec_Type = "U";
                    AGENCY = row["ATTN_AGENCY"].ToString().Trim();
                    DEPT = row["ATTN_DEPT"].ToString().Trim();
                    PROG = row["ATTN_PROGRAM"].ToString().Trim();
                    YEAR = row["ATTN_YEAR"].ToString().Trim();
                    APP_NO = row["ATTN_APP_NO"].ToString().Trim();
                    SITE = row["ATTN_SITE"].ToString().Trim();
                    ROOM = row["ATTN_ROOM"].ToString().Trim();
                    AMPM = row["ATTN_AMPM"].ToString().Trim();
                    FUNDING_SOURCE = row["ATTN_FUNDING_SOURCE"].ToString().Trim();
                    PresentDays = row["PRESENTDAYS"].ToString().Trim();
                    AbsentDays = row["ABSENTDAYS"].ToString().Trim();
                    LegalDays = row["LEGALDAYS"].ToString().Trim();
                    BreakFast = row["BREAKFAST"].ToString().Trim();
                    Lunch = row["LUNCH"].ToString().Trim();
                    AMSnacks = row["AMSNACKS"].ToString().Trim();
                    PMSnacks = row["PMSNACKS"].ToString().Trim();
                    SUB = row["SUB"].ToString().Trim();
                    PA = row["ATTN_PA"].ToString().Trim();
                    Mode = string.Empty;
                }
                else if (strTable == "HSSB2109Summary")
                {
                    Rec_Type = "U";
                    AGENCY = row["ATTN_AGENCY"].ToString().Trim();
                    DEPT = row["ATTN_DEPT"].ToString().Trim();
                    PROG = row["ATTN_PROGRAM"].ToString().Trim();
                    YEAR = row["ATTN_YEAR"].ToString().Trim();
                    //APP_NO = row["ATTN_APP_NO"].ToString().Trim();
                    SITE = row["ATTN_SITE"].ToString().Trim();
                    ROOM = row["ATTN_ROOM"].ToString().Trim();
                    AMPM = row["ATTN_AMPM"].ToString().Trim();
                    FUNDING_SOURCE = row["ATTN_FUNDING_SOURCE"].ToString().Trim();
                    PresentDays = row["PRESENTDAYS"].ToString().Trim();
                    AbsentDays = row["ABSENTDAYS"].ToString().Trim();
                    LegalDays = row["LEGALDAYS"].ToString().Trim();
                    BreakFast = row["BREAKFAST"].ToString().Trim();
                    Lunch = row["LUNCH"].ToString().Trim();
                    AMSnacks = row["AMSNACKS"].ToString().Trim();
                    PMSnacks = row["PMSNACKS"].ToString().Trim();
                    SUB = row["SUB"].ToString().Trim();
                    Applicant = row["Applicants"].ToString().Trim();
                    //  PA = row["ATTN_PA"].ToString().Trim();
                    Mode = string.Empty;
                }
                else if (strTable == "ATTNCOUNTMONTH")
                {
                    AGENCY = row["ATTN_AGENCY"].ToString().Trim();
                    DEPT = row["ATTN_DEPT"].ToString().Trim();
                    PROG = row["ATTN_PROGRAM"].ToString().Trim();
                    YEAR = row["ATTN_YEAR"].ToString().Trim();
                    APP_NO = row["ATTN_APP_NO"].ToString().Trim();
                    SITE = row["ATTN_SITE"].ToString().Trim();
                    ROOM = row["ATTN_ROOM"].ToString().Trim();
                    AMPM = row["ATTN_AMPM"].ToString().Trim();
                    FUNDING_SOURCE = row["ATTN_FUNDING_SOURCE"].ToString().Trim();
                    PresentDays = row["PRESENTDAYS"].ToString().Trim();
                    AbsentDays = row["ABSENTDAYS"].ToString().Trim();
                    LegalDays = row["LEGALDAYS"].ToString().Trim();
                }
                else
                {
                    Rec_Type = "U";
                    AGENCY = row["ATTN_AGENCY"].ToString().Trim();
                    DEPT = row["ATTN_DEPT"].ToString().Trim();
                    PROG = row["ATTN_PROGRAM"].ToString().Trim();
                    YEAR = row["ATTN_YEAR"].ToString().Trim();
                    APP_NO = row["ATTN_APP_NO"].ToString().Trim();
                    SITE = row["ATTN_SITE"].ToString().Trim();
                    ROOM = row["ATTN_ROOM"].ToString().Trim();
                    AMPM = row["ATTN_AMPM"].ToString().Trim();
                    FUNDING_SOURCE = row["ATTN_FUNDING_SOURCE"].ToString().Trim();
                    DATE = row["ATTN_DATE"].ToString().Trim();
                    REASON = row["ATTN_REASON"].ToString().Trim();
                    //PresentDays = row["PRESENTDAYS"].ToString().Trim();
                    //AbsentDays = row["ABSENTDAYS"].ToString().Trim();
                    //LegalDays = row["LEGALDAYS"].ToString().Trim();
                    //BreakFast = row["BREAKFAST"].ToString().Trim();
                    //Lunch = row["LUNCH"].ToString().Trim();
                    //AMSnacks = row["AMSNACKS"].ToString().Trim();
                    //PMSnacks = row["PMSNACKS"].ToString().Trim();
                    //SUB = row["SUB"].ToString().Trim();
                    PA = row["ATTN_PA"].ToString().Trim();
                    Mode = string.Empty;
                }
            }
        }

        public ChldAttnEntity(string _Label, string _EnrlLabel, string _ChildName, string _Date, string _Day, bool _Present, bool _Apsent, bool _ClReason, string _treason, bool _billformeals, bool _B, bool _N, bool _L, bool _P, bool _N1, string _Fundcode, string _Fund, string _ParentFees, string _FundFee, string _HoursSpent, string _Applicant, string _Reasoncode, string _Category, string _ChargeCode, string _Mode, string _BillMeals, string _Other, string _Balps, string _FirstName, string _LastName, string _MiddleName, string _Status, string _AddOperator, string _AddDate, string _LstcOperator, string _LstcDate, string _TimeStart1, string  _TimeStart2, string _TimeEnd1, string _TimeEnd2, string _TimeSum1, string _TimeSum2)
        {

            EnrlLabel = _EnrlLabel; Label = _Label; ChildName = _ChildName; Date = _Date; Day = _Day; Present = _Present; Apsent = _Apsent; ClReason = _ClReason; treason = _treason; billformeals = _billformeals; BoolB = _B; N = _N; BoolL = _L; BoolP = _P; N1 = _N1; Fundcode = _Fundcode; Fund = _Fund; ParentFees = _ParentFees; FundFee = _FundFee; HoursSpent = _HoursSpent; Applicant = _Applicant; Reasoncode = _Reasoncode; Category = _Category; ChargeCode = _ChargeCode; Mode = _Mode; BillMeals = _BillMeals; Other = _Other; Balps = _Balps; FirstName = _FirstName; LastName = _LastName; MiddleName = _MiddleName; Status = _Status;
            ADD_OPERATOR = _AddOperator; DATE_ADD = _AddDate; LSTC_OPERATOR = _LstcOperator; DATE_LSTC = _LstcDate;
            TimeStart1 = _TimeStart1;
            TimeStart2 = _TimeStart2;
            TimeEnd1 = _TimeEnd1;
            TimeEnd2 = _TimeEnd2;
            TimeSum1 = _TimeSum1;
            TimeSum2 = _TimeSum2;
        }


        public ChldAttnEntity(string _FUNDING_SOURCE, string _PresentDays, string _AbsentDays, string _LegalDays, string _BreakFast, string _Lunch, string _AMSnacks, string _PMSnacks, string _SUB)
        {
            FUNDING_SOURCE = _FUNDING_SOURCE;
            PresentDays = _PresentDays;
            AbsentDays = _AbsentDays;
            LegalDays = _LegalDays;
            BreakFast = _BreakFast;
            Lunch = _Lunch;
            AMSnacks = _AMSnacks;
            PMSnacks = _PMSnacks;
            SUB = _SUB;
        }



        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string AGENCY { get; set; }
        public string DEPT { get; set; }
        public string PROG { get; set; }
        public string YEAR { get; set; }
        public string APP_NO { get; set; }
        public string SITE { get; set; }
        public string ROOM { get; set; }
        public string AMPM { get; set; }
        public string FUNDING_SOURCE { get; set; }
        public string DATE { get; set; }
        public string PA { get; set; }
        public string REASON { get; set; }
        public string B { get; set; }
        public string A { get; set; }
        public string L { get; set; }
        public string P { get; set; }
        public string S { get; set; }
        public string PARENT_RATE { get; set; }
        public string FUNDING_RATE { get; set; }
        public string HOURS { get; set; }
        public string CATEGORY { get; set; }
        public string CHARGE_CODE { get; set; }
        public string MEAL { get; set; }
        public string PARENT_INVOICE { get; set; }
        public string LEGAL { get; set; }
        public string PRES_DESC { get; set; }
        public string DATE_ADD { get; set; }
        public string ADD_OPERATOR { get; set; }
        public string DATE_LSTC { get; set; }
        public string LSTC_OPERATOR { get; set; }
        public string Mode { get; set; }
        public string AttnXml { get; set; }
        public string PresentDays { get; set; }
        public string AbsentDays { get; set; }
        public string LegalDays { get; set; }
        public string BreakFast { get; set; }
        public string Lunch { get; set; }
        public string AMSnacks { get; set; }
        public string PMSnacks { get; set; }
        public string SUB { get; set; }

        public string EnrlLabel { get; set; }
        public string Label { get; set; }
        public string TimeStart1 { get; set; }
        public string TimeStart2 { get; set; }
        public string TimeEnd1 { get; set; }
        public string TimeEnd2 { get; set; }
        public string TimeSum1 { get; set; }
        public string TimeSum2 { get; set; }
        public string ChildName { get; set; }
        public string Date { get; set; }
        public string Day { get; set; }
        public bool Present { get; set; }
        public bool Apsent { get; set; }
        public bool ClReason { get; set; }
        public string treason { get; set; }
        public bool billformeals { get; set; }
        public bool BoolB { get; set; }
        public bool BoolL { get; set; }
        public bool BoolP { get; set; }
        public bool N { get; set; }
        public bool N1 { get; set; }
        public string Fundcode { get; set; }
        public string Fund { get; set; }
        public string ParentFees { get; set; }
        public string FundFee { get; set; }
        public string HoursSpent { get; set; }
        public string Applicant { get; set; }
        public string Reasoncode { get; set; }
        public string Category { get; set; }
        public string ChargeCode { get; set; }
        public string BillMeals { get; set; }
        public string Other { get; set; }
        public string Balps { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Status { get; set; }

        #endregion
    }

    public class HSSB2112ReportEntity
    {
        #region Constructors

        public HSSB2112ReportEntity()
        {
            //AGENCY = string.Empty;
            //DEPT = string.Empty;
            //PROG = string.Empty;
            //YEAR = string.Empty;
            //APP_NO = string.Empty;
            SITE = string.Empty;
            ROOM = string.Empty;
            AMPM = string.Empty;
            Col_type = string.Empty;
            //DATE = string.Empty;
            //ENRL_AGENCY = string.Empty;
            //ENRL_DEPT = string.Empty;
            //ENRL_PROG = string.Empty;
            //ENRL_YEAR = string.Empty;
            //ENRL_SITE = string.Empty;
            //ENRL_ROOM = string.Empty;
            //ENRL_AM_PM = string.Empty;
            //ENRL_APP_NO = string.Empty;
            //ENRL_FUND_SOURCE = string.Empty;
            //PA = string.Empty;
            //REASON = string.Empty;
            Breakfast = string.Empty;
            AM_Snacks = string.Empty;
            Lunch = string.Empty;
            PM_Snacks = string.Empty;
            Supper = string.Empty;
            Totalcount = string.Empty;

        }

        public HSSB2112ReportEntity(bool Initialize)
        {
            //AGENCY = null;
            //DEPT = null;
            //PROG = null;
            //YEAR = null;
            //APP_NO = null;
            SITE = null;
            ROOM = null;
            AMPM = null;
            Col_type = null;
            //DATE = null;
            //ENRL_AGENCY = null;
            //ENRL_DEPT = null;
            //ENRL_PROG = null;
            //ENRL_YEAR = null;
            //ENRL_SITE = null;
            //ENRL_ROOM = null;
            //ENRL_AM_PM = null;
            //ENRL_APP_NO = null;
            //ENRL_FUND_SOURCE = null;
            //PA = null;
            //REASON = null;
            Breakfast = null;
            AM_Snacks = null;
            Lunch = null;
            PM_Snacks = null;
            Supper = null;
            Totalcount = null;

        }

        public HSSB2112ReportEntity(DataRow row)
        {
            if (row != null)
            {
                //Rec_Type = "U";
                //AGENCY = row["ATTN_AGENCY"].ToString().Trim();
                //DEPT = row["ATTN_DEPT"].ToString().Trim();
                //PROG = row["ATTN_PROGRAM"].ToString().Trim();
                //YEAR = row["ATTN_YEAR"].ToString().Trim();
                //APP_NO = row["ATTN_APP_NO"].ToString().Trim();
                SITE = row["COL_SITE"].ToString().Trim();
                ROOM = row["COL_ROOM"].ToString().Trim();
                AMPM = row["COL_AMPM"].ToString().Trim();
                Col_type = row["COL_TYPE"].ToString().Trim();
                //ENRL_AGENCY = row["ATTN_ENRL_AGENCY"].ToString().Trim();
                //ENRL_DEPT = row["ATTN_ENRL_DEPT"].ToString().Trim();
                //ENRL_PROG = row["ATTN_ENRL_PROG"].ToString().Trim();
                //ENRL_YEAR = row["ATTN_ENRL_YEAR"].ToString().Trim();
                //ENRL_SITE = row["ATTN_ENRL_SITE"].ToString().Trim();
                //ENRL_ROOM = row["ATTN_ENRL_ROOM"].ToString().Trim();
                //ENRL_AM_PM = row["ATTN_ENRL_AM_PM"].ToString().Trim();
                //ENRL_APP_NO = row["ATTN_ENRL_APP_NO"].ToString().Trim();
                //ENRL_FUND_SOURCE = row["ATTN_ENRL_FUND_SOURCE"].ToString().Trim();
                //PA = row["ATTN_PA"].ToString().Trim();
                //REASON = row["ATTN_REASON"].ToString().Trim();
                Breakfast = row["BREAKFAST"].ToString().Trim();
                AM_Snacks = row["AM_SNACKS"].ToString().Trim();
                Lunch = row["LUNCH"].ToString().Trim();
                PM_Snacks = row["PM_SNACKS"].ToString().Trim();
                Supper = row["SUPPER"].ToString().Trim();
                Totalcount = row["TOTAL_COUNT"].ToString().Trim();

            }
        }

        public HSSB2112ReportEntity(DataRow row, string strtable)
        {
            if (row != null)
            {
                SITE = row["COL_SITE"].ToString().Trim();
                ROOM = row["COL_ROOM"].ToString().Trim();
                AMPM = row["COL_AMPM"].ToString().Trim();
                FREE = row["FREE"].ToString().Trim();
                REDUCED = row["REDUCED"].ToString().Trim();
                PAID = row["PAID"].ToString().Trim();

            }
        }


        #endregion

        #region Properties

        //public string AGENCY { get; set; }
        //public string DEPT { get; set; }
        //public string PROG { get; set; }
        //public string YEAR { get; set; }
        public string SITE { get; set; }
        public string ROOM { get; set; }
        public string AMPM { get; set; }
        public string Col_type { get; set; }
        //public string DATE { get; set; }
        //public string ENRL_AGENCY { get; set; }
        //public string ENRL_DEPT { get; set; }
        //public string ENRL_PROG { get; set; }
        //public string ENRL_YEAR { get; set; }
        //public string ENRL_SITE { get; set; }
        //public string ENRL_ROOM { get; set; }
        //public string ENRL_AM_PM { get; set; }
        //public string ENRL_APP_NO { get; set; }
        //public string ENRL_FUND_SOURCE { get; set; }
        //public string PA { get; set; }
        //public string REASON { get; set; }
        public string Breakfast { get; set; }
        public string AM_Snacks { get; set; }
        public string Lunch { get; set; }
        public string PM_Snacks { get; set; }
        public string Supper { get; set; }
        public string Totalcount { get; set; }
        public string FREE { get; set; }
        public string REDUCED { get; set; }
        public string PAID { get; set; }

        #endregion
    }
}
