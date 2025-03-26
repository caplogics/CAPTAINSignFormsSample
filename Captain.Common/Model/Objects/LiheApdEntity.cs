using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DevExpress.XtraSpreadsheet.Model;

namespace Captain.Common.Model.Objects
{
    public class LiheApdEntity
    {
        #region Constructors

        public LiheApdEntity()
        {
            LdAgency = string.Empty;
            LdDept = string.Empty;
            LdProg = string.Empty;
            LdYear = string.Empty;
            LdLetterId = string.Empty;
            LdSeq = string.Empty;
            LdLetterType = string.Empty;
            LdCategory = string.Empty;
            LdBoilerPlate = string.Empty;
            LdAllowUpdate = string.Empty;
            LdIncomeplete = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;

        }

        public LiheApdEntity(DataRow row)
        {
            if (row != null)
            {
                LdAgency = row["LD_AGENCY"].ToString();
                LdDept = row["LD_DEPT"].ToString();
                LdProg = row["LD_PROG"].ToString();
                LdYear = row["LD_YEAR"].ToString();
                LdLetterId = row["LD_LETTER_ID"].ToString();
                LdSeq = row["LD_SEQ"].ToString();
                LdLetterType = row["LD_LETTER_TYPE"].ToString();
                LdCategory = row["LD_CATEGORY"].ToString();
                LdBoilerPlate = row["LD_BOILER_PLATE"].ToString();
                LdAllowUpdate = row["LD_ALLOW_UPDATE"].ToString();
                LdIncomeplete = row["LD_INCOMPLETE"].ToString();
                DateAdd = row["LD_DATE_ADD"].ToString();
                AddOperator = row["LD_ADD_OPERATOR"].ToString();
                DateLstc = row["LD_DATE_LSTC"].ToString();
                LstcOperator = row["LD_LSTC_OPERATOR"].ToString();
            }

        }

        #endregion

        #region Properties

        public string LdAgency { get; set; }
        public string LdDept { get; set; }
        public string LdProg { get; set; }
        public string LdYear { get; set; }
        public string LdLetterId { get; set; }
        public string LdSeq { get; set; }
        public string LdLetterType { get; set; }
        public string LdCategory { get; set; }
        public string LdBoilerPlate { get; set; }
        public string LdAllowUpdate { get; set; }
        public string LdIncomeplete { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string Mode { get; set; }
        public string Type { get; set; }
        #endregion
    }

    public class LiheAppEntity
    {
        #region Constructors

        public LiheAppEntity()
        {
            LapAgency = string.Empty;
            LapDept = string.Empty;
            LapProg = string.Empty;
            LapYear = string.Empty;
            LapLetterId = string.Empty;
            LapApp = string.Empty;
            LapLetterDate = string.Empty;
            LapWorker = string.Empty;
            LapStatus = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;
            Selected = string.Empty;
            Remaining = string.Empty;

        }

        public LiheAppEntity(DataRow row, string strTable)
        {
            if (row != null)
            {
                LapAgency = row["LAP_AGENCY"].ToString();
                LapDept = row["LAP_DEPT"].ToString();
                LapProg = row["LAP_PROGRAM"].ToString();
                LapYear = row["LAP_YEAR"].ToString();
                LapApp = row["LAP_APP"].ToString();
                LapLetterId = row["LAP_LETTER_ID"].ToString();
                LapLetterDate = row["LAP_LETTER_DATE"].ToString();
                LapWorker = row["LAP_WORKER"].ToString();
                LapStatus = row["LAP_STATUS"].ToString();
                DateAdd = row["LAP_DATE_ADD"].ToString();
                AddOperator = row["LAP_ADD_OPERATOR"].ToString();
                DateLstc = row["LAP_DATE_LSTC"].ToString();
                LstcOperator = row["LAP_LSTC_OPERATOR"].ToString();
                if (strTable == "APPAPP1")
                {
                    Selected = row["Selected"].ToString();
                    Remaining = row["Remaining"].ToString();
                    CaseWorker = row["CaseWorker"].ToString();
                }
            }

        }

        #endregion

        #region Properties

        public string LapAgency { get; set; }
        public string LapDept { get; set; }
        public string LapProg { get; set; }
        public string LapYear { get; set; }
        public string LapApp { get; set; }
        public string LapLetterId { get; set; }
        public string LapLetterDate { get; set; }
        public string LapWorker { get; set; }
        public string LapStatus { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string Selected { get; set; }
        public string Remaining { get; set; }
        public string CaseWorker { get; set; }
        public string CategorySeq { get; set; }
        public string ReciveDate { get; set; }
        public string Mode { get; set; }
        #endregion
    }

    public class LiheApp1Entity
    {
        #region Constructors

        public LiheApp1Entity()
        {
            Lap1Agency = string.Empty;
            Lap1Dept = string.Empty;
            Lap1Prog = string.Empty;
            Lap1Year = string.Empty;
            Lap1LetterId = string.Empty;
            Lap1App = string.Empty;
            Lap1LetterDate = string.Empty;
            Lap1CategorySeq = string.Empty;
            Lap1ReceiveDate = string.Empty;
            Lap1CatergoryDesc = string.Empty;

        }

        public LiheApp1Entity(DataRow row, string strTable)
        {
            if (row != null)
            {
                Lap1Agency = row["LAP1_AGENCY"].ToString();
                Lap1Dept = row["LAP1_DEPT"].ToString();
                Lap1Prog = row["LAP1_PROGRAM"].ToString();
                Lap1Year = row["LAP1_YEAR"].ToString();
                Lap1App = row["LAP1_APP"].ToString();
                Lap1LetterId = row["LAP1_LETTER_ID"].ToString();
                Lap1LetterDate = row["LAP1_LETTER_DATE"].ToString();
                Lap1CategorySeq = row["LAP1_CATEGORY_SEQ"].ToString();
                Lap1ReceiveDate = row["LAP1_RECEIVE_DATE"].ToString();
                if (strTable == "APDAPP1")
                {
                    Lap1CatergoryDesc = row["LD_CATEGORY"].ToString();
                }

            }

        }

        #endregion

        #region Properties

        public string Lap1Agency { get; set; }
        public string Lap1Dept { get; set; }
        public string Lap1Prog { get; set; }
        public string Lap1Year { get; set; }
        public string Lap1App { get; set; }
        public string Lap1LetterId { get; set; }
        public string Lap1LetterDate { get; set; }
        public string Lap1CategorySeq { get; set; }
        public string Lap1ReceiveDate { get; set; }
        public string Lap1CatergoryDesc { get; set; }
        public string Mode { get; set; }
        #endregion
    }

    public class LiheApcEntity
    {
        #region Constructors

        public LiheApcEntity()
        {
            LacAgency = string.Empty;
            LacDept = string.Empty;
            LacProg = string.Empty;
            LacYear = string.Empty;
            LacLetterId = string.Empty;
            LacApp = string.Empty;
            LacLetterDate = string.Empty;
            LacCategorySeq = string.Empty;
            Lacboillerplate = string.Empty;
            Lacprintbplate = string.Empty;
            Laccasenotesdata = string.Empty;
            Lacprintcnotes = string.Empty;
            Lacdateadd = string.Empty;
            Lacaddoperator = string.Empty;
            Lacdatelstc = string.Empty;
            Laclstcoperator = string.Empty;


        }

        public LiheApcEntity(DataRow row)
        {
            if (row != null)
            {
                LacAgency = row["lac_agency"].ToString();
                LacDept = row["lac_dept"].ToString();
                LacProg = row["lac_program"].ToString();
                LacYear = row["lac_year"].ToString();
                LacApp = row["lac_app"].ToString();
                LacLetterId = row["lac_letter_id"].ToString();
                LacLetterDate = row["lac_letter_date"].ToString();
                LacCategorySeq = row["lac_category_seq"].ToString();
                Lacboillerplate = row["lac_boiler_plate"].ToString();
                Lacprintbplate = row["lac_print_bplate"].ToString();
                Laccasenotesdata = row["lac_casenotes_data"].ToString();
                Lacprintcnotes = row["lac_print_cnotes"].ToString();
                Lacdateadd = row["lac_date_add"].ToString();
                Lacaddoperator = row["lac_add_operator"].ToString();
                Lacdatelstc = row["lac_date_lstc"].ToString();
                Laclstcoperator = row["lac_lstc_operator"].ToString();

            }

        }

        #endregion

        #region Properties

        public string LacAgency { get; set; }
        public string LacDept { get; set; }
        public string LacProg { get; set; }
        public string LacYear { get; set; }
        public string LacApp { get; set; }
        public string LacLetterId { get; set; }
        public string LacLetterDate { get; set; }
        public string LacCategorySeq { get; set; }
        public string Lacboillerplate { get; set; }
        public string Lacprintbplate { get; set; }
        public string Laccasenotesdata { get; set; }
        public string Lacprintcnotes { get; set; }
        public string Lacdateadd { get; set; }
        public string Lacaddoperator { get; set; }
        public string Lacdatelstc { get; set; }
        public string Laclstcoperator { get; set; }
        public string Mode { get; set; }
        #endregion
    }

    public class LiheApvEntity
    {
        #region Constructors

        public LiheApvEntity()
        {
            LPV_AGENCY = string.Empty;
            LPV_DEPT = string.Empty;
            LPV_PROGRAM = string.Empty;
            LPV_YEAR = string.Empty;
            LPV_APP_NO = string.Empty;
            LPV_SEQ = string.Empty;
            LPV_VENDOR = string.Empty;
            LPV_ACCOUNT_NO = string.Empty;
            LPV_PRIMARY_CODE = string.Empty;
            LPV_PAYMENT_FOR = string.Empty;
            LPV_CYCLE = string.Empty;
            LPV_DIVIDE_BILL = string.Empty;
            LPV_BILL_LNAME = string.Empty;
            LPV_BILL_FNAME = string.Empty;
            LPV_MOR = string.Empty;
            LPV_METER = string.Empty;
            LPV_DATE_LSTC = string.Empty;
            LPV_LSTC_OPERATOR = string.Empty;
            LPV_DATE_ADD = string.Empty;
            LPV_ADD_OPERATOR = string.Empty;
            LPV_BILLNAME_TYPE = string.Empty;
            LVR_FAILED_ACCOUNT_EDIT = string.Empty;

        }

        public LiheApvEntity(DataRow row)
        {
            if (row != null)
            {
                LPV_AGENCY = row["LPV_AGENCY"].ToString();
                LPV_DEPT = row["LPV_DEPT"].ToString();
                LPV_PROGRAM = row["LPV_PROGRAM"].ToString();
                LPV_YEAR = row["LPV_YEAR"].ToString();
                LPV_APP_NO = row["LPV_APP_NO"].ToString();
                LPV_SEQ = row["LPV_SEQ"].ToString();
                LPV_VENDOR = row["LPV_VENDOR"].ToString();
                LPV_ACCOUNT_NO = row["LPV_ACCOUNT_NO"].ToString();
                LPV_PRIMARY_CODE = row["LPV_PRIMARY_CODE"].ToString();
                LPV_PAYMENT_FOR = row["LPV_PAYMENT_FOR"].ToString();
                LPV_CYCLE = row["LPV_CYCLE"].ToString();
                LPV_DIVIDE_BILL = row["LPV_DIVIDE_BILL"].ToString();
                LPV_BILL_LNAME = row["LPV_BILL_LNAME"].ToString();
                LPV_BILL_FNAME = row["LPV_BILL_FNAME"].ToString();
                LPV_MOR = row["LPV_MOR"].ToString();
                LPV_METER = row["LPV_METER"].ToString();
                LPV_DATE_LSTC = row["LPV_DATE_LSTC"].ToString();
                LPV_LSTC_OPERATOR = row["LPV_LSTC_OPERATOR"].ToString();
                LPV_DATE_ADD = row["LPV_DATE_ADD"].ToString();
                LPV_ADD_OPERATOR = row["LPV_ADD_OPERATOR"].ToString();
                LPV_VERIFY_DATE = row["LPV_VERIFY_DATE"].ToString();
                LPV_VERIFIED_BY = row["LPV_VERIFIED_BY"].ToString();
                LPV_REVERIFY = row["LPV_REVERIFY"].ToString();
                LPV_VENDORNAME = string.Empty;
                LPV_BILLNAME_TYPE = row["LPV_BILLNAME_TYPE"].ToString();
                LVR_FAILED_ACCOUNT_EDIT = row["LPV_FAILED_ACCOUNT_EDIT"].ToString();

            }

        }


        public LiheApvEntity(DataRow row,string strtable)
        {
            if (row != null)
            {
                LPV_AGENCY = row["SNP_AGENCY"].ToString();
                LPV_DEPT = row["SNP_DEPT"].ToString();
                LPV_PROGRAM = row["SNP_PROGRAM"].ToString();
                LPV_YEAR = row["SNP_YEAR"].ToString();
                LPV_APP_NO = row["SNP_APP"].ToString();              
                LPV_VENDOR = row["LPV_VENDOR"].ToString();
                LPV_ACCOUNT_NO = row["LPV_ACCOUNT_NO"].ToString();
                LPV_PRIMARY_CODE = row["LPV_PRIMARY_CODE"].ToString();
                LPV_BILL_LNAME = row["SNP_NAME_IX_LAST"].ToString();
                LPV_BILL_FNAME = row["SNP_NAME_IX_FI"].ToString();
                MiddleName = row["SNP_NAME_IX_MI"].ToString();
                HN = row["MST_HN"].ToString();
                Street = row["MST_STREET"].ToString();
                Suffix = row["MST_SUFFIX"].ToString();
                City = row["MST_CITY"].ToString();
                State = row["MST_STATE"].ToString();
                Housing = row["MST_HOUSING"].ToString();
                Zip = row["MST_ZIP"].ToString();
                Zipplus = row["MST_ZIPPLUS"].ToString();
                Remaing = row["decRemaing"].ToString();
                ReduceElig = row["declpbReduceElig"].ToString();
                FapIncome = row["declpbFapIncome"].ToString();
                PainAmount = row["decytdPaid"].ToString();
                TotalAmount = row["decamount"].ToString();
                FundCode = row["fundcode"].ToString();
                Eligclients = row["eligclients"].ToString();
                LPV_PAYMENT_FOR = row["LPV_PAYMENT_FOR"].ToString();
                LpbType = row["lpbtype"].ToString();

            }

        }


        #endregion

        #region Properties

        public string LPV_AGENCY { get; set; }
        public string LPV_DEPT { get; set; }
        public string LPV_PROGRAM { get; set; }
        public string LPV_YEAR { get; set; }
        public string LPV_APP_NO { get; set; }
        public string LPV_SEQ { get; set; }
        public string LPV_VENDOR { get; set; }
        public string LPV_ACCOUNT_NO { get; set; }
        public string LPV_PRIMARY_CODE { get; set; }
        public string LPV_PAYMENT_FOR { get; set; }
        public string LPV_CYCLE { get; set; }
        public string LPV_DIVIDE_BILL { get; set; }
        public string LPV_BILL_LNAME { get; set; }
        public string LPV_BILL_FNAME { get; set; }
        public string LPV_MOR { get; set; }
        public string LPV_METER { get; set; }
        public string LPV_DATE_LSTC { get; set; }
        public string LPV_LSTC_OPERATOR { get; set; }
        public string LPV_DATE_ADD { get; set; }
        public string LPV_ADD_OPERATOR { get; set; }
        public string LPV_VENDORNAME { get; set; }
        public string Mode { get; set; }
        public string MiddleName { get; set; }
        public string HN { get; set; }
        public string Street { get; set; }
        public string Suffix { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Housing { get; set; }
        public string Zip { get; set; }
        public string Zipplus { get; set; }
        public string Remaing { get; set; }
        public string ReduceElig { get; set; }
        public string FapIncome { get; set; }
        public string PainAmount { get; set; }
        public string TotalAmount { get; set; }
        public string FundCode { get; set; }
        public string Eligclients { get; set; }
        public string LpbType { get; set; }
        public string LPV_VERIFY_DATE { get; set; }
        public string LPV_VERIFIED_BY { get; set; }
        public string LPV_REVERIFY { get; set; }
        public string LPV_BILLNAME_TYPE { get; set; }
        public string LVR_FAILED_ACCOUNT_EDIT { get; set; }
        #endregion
    }

    public class LIHEAPBEntity
    {
        #region Constructors

        public LIHEAPBEntity()
        {
            Rec_Type =
            Agency = 
            Dept = 
            Prog = 
            Year = 
            AppNo =
            Seq = 
            Award_Type = 
            Award_Balance =
            Award_Date = 
            Caseworker = 
            Notice_Date = 
            Den_Notice_Date = 
            Resend = 
            Deny = 
            Age_dis = 
            Source = 
            Inc_Cert =
            CATELIG_Cert=
            Renter =

            FAP_No_InHH = 
            Reason_Denied = 
            Action_Code = 
            AU = 
            Payment_How = 
            Disable = 
            Inc_Worker = 
            Inc_Date = 
            Benefit_Level = 
            MOD = 
            Elig_Assets = 
            Elig_Assets1 = 
            Elig_Income = 
            Elig_Fuel = 
            Elig_Rent = 

            Elig_Wthr = 
            Contingency =
            Certified_Status = 
            Date_Renotify = 
            Letter_NO = 
            Pay_Notice_Date = 
            Fund_Code = 
            SW1 = 
            SW2 = 
            SW3 = 
            SW4 = 
            SW5 = 
            SW6 = 
            SW7 = 
            SW8 = 
            SSN_SW = 
            BatchNO =

            CONS_Months = 
            CONS_Months_NY = 
            WEATH_Intrest = 
            WEATH_No_Rooms =
            WEATH_Share = 
            WEATH_Before = 
            Override = 
            APP_Applied = 
            INCOMP_Assets = 
            INCOMP_SSN = 
            INCOMP_Rent = 
            INCOMP_Util = 
            Categ_Elig = 
            HH_FS_Benefit =
            DSS_ADFC = 
            WAP_Date = 
            WAP_Operator = 
            Add_Date = 
            Add_Operatop = 
            LSTC_Date = 
            LSTC_Operator = string.Empty;

            Denied_Reason = string.Empty;
            LiquidAsset = string.Empty;
            RiskAsses = string.Empty;

            Award_Amount =
            Paid =
            Awaiting_Dol =
            Assets =
            FAP_Income =
            Reduce_Elig =
            Remaining =
            CONS_Gallons =
            CONS_Dollars =
            CONS_Gallons_NY =
            CONS_Dollars_NY =
            TKT_Balance = "0";
        }

        public LIHEAPBEntity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type =
                Agency =
                Dept =
                Prog =
                Year =
                AppNo =
                Seq =
                Award_Type =
                Award_Amount =
                Award_Balance = 
                Paid =
                Awaiting_Dol =
                Assets =
                Award_Date =
                Caseworker =
                Notice_Date =
                Den_Notice_Date =
                Resend =
                Deny =
                Age_dis =
                Source =
                Inc_Cert =
                CATELIG_Cert=
                Renter =
                FAP_Income =
                FAP_No_InHH =
                Reason_Denied =
                Action_Code =
                AU =
                Payment_How =
                Disable =
                Inc_Worker =
                Inc_Date =
                Benefit_Level =
                MOD =
                Elig_Assets =
                Elig_Assets1 =
                Elig_Income =
                Elig_Fuel =
                Elig_Rent =

                Elig_Wthr =
                Contingency =
                Reduce_Elig =
                Certified_Status =
                Date_Renotify =
                Letter_NO =
                Pay_Notice_Date =
                Fund_Code =
                SW1 =
                SW2 =
                SW3 =
                SW4 =
                SW5 =
                SW6 =
                SW7 =
                SW8 =
                SSN_SW =
                BatchNO =
                Remaining =
                CONS_Gallons =
                CONS_Dollars =
                CONS_Months =
                CONS_Gallons_NY =
                CONS_Dollars_NY =
                CONS_Months_NY =
                WEATH_Intrest =
                WEATH_No_Rooms =
                WEATH_Share =
                WEATH_Before =
                Override =
                APP_Applied =
                INCOMP_Assets =
                INCOMP_SSN =
                INCOMP_Rent =
                INCOMP_Util =
                Categ_Elig =
                TKT_Balance =
                HH_FS_Benefit =
                DSS_ADFC = 
                WAP_Date =
                WAP_Operator =
                Add_Date =
                Add_Operatop =
                LSTC_Date =
                LSTC_Operator = null;

                Award_Amount =
                Paid =
                Awaiting_Dol =
                Assets =
                FAP_Income =
                FAP_No_InHH =
                Reduce_Elig =
                Letter_NO =
                Remaining =
                CONS_Gallons =
                CONS_Dollars =
                CONS_Gallons_NY =
                CONS_Dollars_NY =
                TKT_Balance =
                LiquidAsset=
                RiskAsses=
                Denied_Reason = null;
            }
        }

        public LIHEAPBEntity(LIHEAPBEntity Entity)
        {
            Rec_Type = Entity.Rec_Type;
            Agency = Entity.Agency;
            Dept = Entity.Dept;
            Prog = Entity.Prog;
            Year = Entity.Year;
            AppNo = Entity.AppNo;
            Seq = Entity.Seq;
            Award_Type = Entity.Award_Type;
            Award_Amount = Entity.Award_Amount;
            Award_Balance = Entity.Award_Balance;
            Paid = Entity.Paid;
            Awaiting_Dol = Entity.Awaiting_Dol;
            Assets = Entity.Assets;
            Award_Date = Entity.Award_Date;
            Caseworker = Entity.Caseworker;
            Notice_Date = Entity.Notice_Date;
            Den_Notice_Date = Entity.Den_Notice_Date;
            Resend = Entity.Resend;
            Deny = Entity.Deny;
            Age_dis = Entity.Age_dis;
            Source = Entity.Source;
            Inc_Cert = Entity.Inc_Cert;
            CATELIG_Cert = Entity.CATELIG_Cert;
            Renter = Entity.Renter;
            FAP_Income = Entity.FAP_Income;
            FAP_No_InHH = Entity.FAP_No_InHH;
            Reason_Denied = Entity.Reason_Denied;
            Action_Code = Entity.Action_Code;
            AU = Entity.AU;
            Payment_How = Entity.Payment_How;
            Disable = Entity.Disable;
            Inc_Worker = Entity.Inc_Worker;
            Inc_Date = Entity.Inc_Date;
            Benefit_Level = Entity.Benefit_Level;
            MOD = Entity.MOD;
            Elig_Assets = Entity.Elig_Assets;
            Elig_Assets1 = Entity.Elig_Assets1;
            Elig_Income = Entity.Elig_Income;
            Elig_Fuel = Entity.Elig_Fuel;
            Elig_Rent = Entity.Elig_Rent;

            Elig_Wthr = Entity.Elig_Wthr;
            Contingency = Entity.Contingency;
            Reduce_Elig = Entity.Reduce_Elig;
            Certified_Status = Entity.Certified_Status;
            Date_Renotify = Entity.Date_Renotify;
            Letter_NO = Entity.Letter_NO;
            Pay_Notice_Date = Entity.Pay_Notice_Date;
            Fund_Code = Entity.Fund_Code;
            SW1 = Entity.SW1;
            SW2 = Entity.SW2;
            SW3 = Entity.SW3;
            SW4 = Entity.SW4;
            SW5 = Entity.SW5;
            SW6 = Entity.SW6;
            SW7 = Entity.SW7;
            SW8 = Entity.SW8;
            SSN_SW = Entity.SSN_SW;
            BatchNO = Entity.BatchNO;
            Remaining = Entity.Remaining;
            CONS_Gallons = Entity.CONS_Gallons;
            CONS_Dollars = Entity.CONS_Dollars;
            CONS_Months = Entity.CONS_Months;
            CONS_Gallons_NY = Entity.CONS_Gallons_NY;
            CONS_Dollars_NY = Entity.CONS_Dollars_NY;
            CONS_Months_NY = Entity.CONS_Months_NY;
            WEATH_Intrest = Entity.WEATH_Intrest;
            WEATH_No_Rooms = Entity.WEATH_No_Rooms;
            WEATH_Share = Entity.WEATH_Share;
            WEATH_Before = Entity.WEATH_Before;
            Override = Entity.Override;
            APP_Applied = Entity.APP_Applied;
            INCOMP_Assets = Entity.INCOMP_Assets;
            INCOMP_SSN = Entity.INCOMP_SSN;
            INCOMP_Rent = Entity.INCOMP_Rent;
            INCOMP_Util = Entity.INCOMP_Util;
            Categ_Elig = Entity.Categ_Elig;
            TKT_Balance = Entity.TKT_Balance;
            HH_FS_Benefit = Entity.HH_FS_Benefit;
            DSS_ADFC = Entity.DSS_ADFC;

            WAP_Date = Entity.WAP_Date;
            WAP_Operator = Entity.WAP_Operator;
            Add_Date = Entity.Add_Date;
            Add_Operatop = Entity.Add_Operatop;
            LSTC_Date = Entity.LSTC_Date;
            LSTC_Operator = Entity.LSTC_Operator;

            Denied_Reason = Entity.Denied_Reason;
            LiquidAsset = Entity.LiquidAsset;
            RiskAsses = Entity.RiskAsses;
        }

        public LIHEAPBEntity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                Agency = row["LPB_AGENCY"].ToString();
                Dept = row["LPB_DEPT"].ToString();
                Prog = row["LPB_PROG"].ToString();
                Year = row["LPB_YEAR"].ToString();
                AppNo = row["LPB_APP_NO"].ToString();
                Seq = row["LPB_SEQ"].ToString();

                Award_Type = row["LPB_TYPE"].ToString().Trim();
                Award_Balance = "0";
                Award_Date = row["LPB_DATE"].ToString().Trim();
                Caseworker = row["LPB_CASEWORKER"].ToString().Trim();
                Notice_Date = row["LPB_NOTICE_DATE"].ToString().Trim();
                Den_Notice_Date = row["LPB_DEN_NOTICE_DATE"].ToString().Trim();
                Resend = row["LPB_RESEND"].ToString().Trim();
                Deny = row["LPB_DENY"].ToString().Trim();
                Age_dis = row["LPB_AGEDIS"].ToString().Trim();
                Source = row["LPB_SOURCE"].ToString().Trim();
                Inc_Cert = row["LPB_INC"].ToString().Trim();
                Renter = row["LPB_RENTER"].ToString().Trim();

                CATELIG_Cert = row["LPB_CATELIG"].ToString();

                FAP_No_InHH = row["LPB_FAP_NO_INHH"].ToString().Trim();
                Reason_Denied = row["LPB_REASON_DENIED"].ToString().Trim();
                Action_Code = row["LPB_ACTION_CODE"].ToString().Trim();
                AU = row["LPB_AU"].ToString().Trim();
                Payment_How = row["LPB_PAYMENT_HOW"].ToString().Trim();
                Disable = row["LPB_DISABLE"].ToString().Trim();
                Inc_Worker = row["LPB_INCWORKER"].ToString().Trim();
                Inc_Date = row["LPB_INCDATE"].ToString().Trim();
                Benefit_Level = row["LPB_BENEFIT_LEVEL"].ToString().Trim();
                MOD = row["LPB_MOD"].ToString().Trim();
                Elig_Assets = row["LPB_ELIGASSETS"].ToString().Trim();
                Elig_Assets1 = row["LPB_ELIGASSETS1"].ToString().Trim();
                Elig_Income = row["LPB_ELIGINCOME"].ToString().Trim();
                Elig_Fuel = row["LPB_ELIGFUEL"].ToString().Trim();
                Elig_Rent = row["LPB_ELIGRENT"].ToString().Trim();

                Elig_Wthr = row["LPB_ELIGWTHR"].ToString().Trim();
                Contingency = row["LPB_CONTINGENCY"].ToString().Trim();
                Certified_Status = row["LPB_CERTIFIED_STATUS"].ToString().Trim();
                Date_Renotify = row["LPB_DATE_RENOTIFY"].ToString().Trim();
                Letter_NO = row["LPB_LETTER_NO"].ToString().Trim();
                Pay_Notice_Date = row["LPB_PAY_NOTICE_DATE"].ToString().Trim();
                Fund_Code = row["LPB_FUND_CODE"].ToString().Trim();
                SW1 = row["LPB_SW1"].ToString().Trim();
                SW2 = row["LPB_SW2"].ToString().Trim();
                SW3 = row["LPB_SW3"].ToString().Trim();
                SW4 = row["LPB_SW4"].ToString().Trim();
                SW5 = row["LPB_SW5"].ToString().Trim();
                SW6 = row["LPB_SW6"].ToString().Trim();
                SW7 = row["LPB_SW7"].ToString().Trim();
                SW8 = row["LPB_SW8"].ToString().Trim();


                SSN_SW = row["LPB_SSN_SW"].ToString().Trim();
                BatchNO = row["LPB_BATCHNO"].ToString().Trim();
                CONS_Gallons = row["LPB_CONS_GALLONS"].ToString().Trim();
                CONS_Dollars = row["LPB_CONS_DOLLARS"].ToString().Trim();
                CONS_Months = row["LPB_CONS_MONTHS"].ToString().Trim();
                CONS_Gallons_NY = row["LPB_CONS_GALLONS_NY"].ToString().Trim();
                CONS_Dollars_NY = row["LPB_CONS_DOLLARS_NY"].ToString().Trim();
                CONS_Months_NY = row["LPB_CONS_MONTHS_NY"].ToString().Trim();
                WEATH_Intrest = row["LPB_WEATH_INTEREST"].ToString().Trim();
                WEATH_No_Rooms = row["LPB_WEATH_NOROOMS"].ToString().Trim();
                WEATH_Share = row["LPB_WEATH_SHARE"].ToString().Trim();
                WEATH_Before = row["LPB_WEATH_BEFORE"].ToString().Trim();
                Override = row["LPB_OVERRIDE"].ToString().Trim();
                APP_Applied = row["LPB_APP_APPLIED"].ToString().Trim();

                INCOMP_Assets = row["LPB_INCOMP_ASSET"].ToString().Trim();
                INCOMP_SSN = row["LPB_INCOMP_SSN"].ToString().Trim();
                INCOMP_Rent = row["LPB_INCOMP_RENT"].ToString().Trim();
                INCOMP_Util = row["LPB_INCOMP_UTIL"].ToString().Trim();
                Categ_Elig = row["LPB_CATEG_ELIG"].ToString().Trim();
                TKT_Balance = row["LPB_TKT_BALANCE"].ToString().Trim();
                DSS_ADFC = row["LPB_DSS_ADFC"].ToString().Trim();

                WAP_Date = row["LPB_WAP_DATE"].ToString().Trim();
                WAP_Operator = row["LPB_WAP_OPERATOR"].ToString().Trim();
                Add_Date = row["LPB_DATE_ADD"].ToString().Trim();
                Add_Operatop = row["LPB_ADD_OPERATOR"].ToString().Trim();
                LSTC_Date = row["LPB_DATE_LSTC"].ToString().Trim();
                LSTC_Operator = row["LPB_LSTC_OPERATOR"].ToString().Trim();
                Denied_Reason = row["LPB_DENIED_REASON"].ToString().Trim();
                LiquidAsset= row["LPB_LIQUID_ASSET"].ToString().Trim();
                RiskAsses = row["LPB_RISK"].ToString().Trim();

                Award_Amount = ((string.IsNullOrEmpty(row["LPB_AMOUNT"].ToString().Trim()) || row["LPB_AMOUNT"].ToString().Trim() == "0.00") ? "0" : row["LPB_AMOUNT"].ToString().Trim()) ;
                Paid = ((string.IsNullOrEmpty(row["LPB_PAID"].ToString().Trim()) || row["LPB_PAID"].ToString().Trim() == "0.00") ? "0" : row["LPB_PAID"].ToString().Trim());
                Awaiting_Dol = ((string.IsNullOrEmpty(row["LPB_AWAITING_DOL"].ToString().Trim()) || row["LPB_AWAITING_DOL"].ToString().Trim() == "0.00") ? "0" : row["LPB_AWAITING_DOL"].ToString().Trim());
                Assets = ((string.IsNullOrEmpty(row["LPB_ASSETS"].ToString().Trim()) || row["LPB_ASSETS"].ToString().Trim() == "0.00") ? "0" : row["LPB_ASSETS"].ToString().Trim());
                FAP_Income = ((string.IsNullOrEmpty(row["LPB_FAP_INCOME"].ToString().Trim()) || row["LPB_FAP_INCOME"].ToString().Trim() == "0.00") ? "0" : row["LPB_FAP_INCOME"].ToString().Trim());
                Reduce_Elig = ((string.IsNullOrEmpty(row["LPB_REDUCE_ELIG"].ToString().Trim()) || row["LPB_REDUCE_ELIG"].ToString().Trim() == "0.00") ? "0" : row["LPB_REDUCE_ELIG"].ToString().Trim());
                Remaining = ((string.IsNullOrEmpty(row["LPB_REMAINING"].ToString().Trim()) || row["LPB_REMAINING"].ToString().Trim() == "0.00") ? "0" : row["LPB_REMAINING"].ToString().Trim());
                CONS_Gallons = ((string.IsNullOrEmpty(row["LPB_CONS_GALLONS"].ToString().Trim()) || row["LPB_CONS_GALLONS"].ToString().Trim() == "0.00") ? "0" : row["LPB_CONS_GALLONS"].ToString().Trim());
                CONS_Dollars = ((string.IsNullOrEmpty(row["LPB_CONS_DOLLARS"].ToString().Trim()) || row["LPB_CONS_DOLLARS"].ToString().Trim() == "0.00") ? "0" : row["LPB_CONS_DOLLARS"].ToString().Trim());
                CONS_Gallons_NY = ((string.IsNullOrEmpty(row["LPB_CONS_GALLONS_NY"].ToString().Trim()) || row["LPB_CONS_GALLONS_NY"].ToString().Trim() == "0.00") ? "0" : row["LPB_CONS_GALLONS_NY"].ToString().Trim());
                CONS_Dollars_NY = ((string.IsNullOrEmpty(row["LPB_CONS_DOLLARS_NY"].ToString().Trim()) || row["LPB_CONS_DOLLARS_NY"].ToString().Trim() == "0.00") ? "0" : row["LPB_CONS_DOLLARS_NY"].ToString().Trim());
                TKT_Balance = ((string.IsNullOrEmpty(row["LPB_TKT_BALANCE"].ToString().Trim()) || row["LPB_TKT_BALANCE"].ToString().Trim() == "0.00") ? "0" : row["LPB_TKT_BALANCE"].ToString().Trim());
                HH_FS_Benefit = ((string.IsNullOrEmpty(row["LPB_HH_FS_BENIFIT"].ToString().Trim()) || row["LPB_HH_FS_BENIFIT"].ToString().Trim() == "0.00") ? "0" : row["LPB_HH_FS_BENIFIT"].ToString().Trim());

                
            }
        }

        public LIHEAPBEntity(DataRow row, string strTable)
        {
            if (strTable == "TRIGGERS")
            {
                if (row != null)
                {
                    //Rec_Type = "U";
                    Agency = row["LPB_AGENCY"].ToString();
                    Dept = row["LPB_DEPT"].ToString();
                    Prog = row["LPB_PROG"].ToString();
                    Year = row["LPB_YEAR"].ToString();
                    AppNo = row["LPB_APP_NO"].ToString();
                    //Seq = row["LPB_SEQ"].ToString();

                    Award_Type = row["LPB_TYPE"].ToString().Trim();
                    Award_Balance = "0";
                    Award_Date = row["LPB_DATE"].ToString().Trim();
                    //Caseworker = row["LPB_CASEWORKER"].ToString().Trim();
                    //Notice_Date = row["LPB_NOTICE_DATE"].ToString().Trim();
                    //Den_Notice_Date = row["LPB_DEN_NOTICE_DATE"].ToString().Trim();
                    //Resend = row["LPB_RESEND"].ToString().Trim();
                    //Deny = row["LPB_DENY"].ToString().Trim();
                    Age_dis = row["LPB_AGEDIS"].ToString().Trim();
                    Source = row["LPB_SOURCE"].ToString().Trim();
                    Inc_Cert = row["LPB_INC"].ToString().Trim();
                    //Renter = row["LPB_RENTER"].ToString().Trim();

                    FAP_No_InHH = row["LPB_FAP_NO_INHH"].ToString().Trim();
                    //Reason_Denied = row["LPB_REASON_DENIED"].ToString().Trim();
                    //Action_Code = row["LPB_ACTION_CODE"].ToString().Trim();
                    //AU = row["LPB_AU"].ToString().Trim();
                    //Payment_How = row["LPB_PAYMENT_HOW"].ToString().Trim();
                    //Disable = row["LPB_DISABLE"].ToString().Trim();
                    //Inc_Worker = row["LPB_INCWORKER"].ToString().Trim();
                    //Inc_Date = row["LPB_INCDATE"].ToString().Trim();
                    Benefit_Level = row["LPB_BENEFIT_LEVEL"].ToString().Trim();
                    //MOD = row["LPB_MOD"].ToString().Trim();
                    Elig_Assets = row["LPB_ELIGASSETS"].ToString().Trim();
                    //Elig_Assets1 = row["LPB_ELIGASSETS1"].ToString().Trim();
                    Elig_Income = row["LPB_ELIGINCOME"].ToString().Trim();
                    Elig_Fuel = row["LPB_ELIGFUEL"].ToString().Trim();
                    Elig_Rent = row["LPB_ELIGRENT"].ToString().Trim();

                    //Elig_Wthr = row["LPB_ELIGWTHR"].ToString().Trim();
                    //Contingency = row["LPB_CONTINGENCY"].ToString().Trim();
                    //Certified_Status = row["LPB_CERTIFIED_STATUS"].ToString().Trim();
                    //Date_Renotify = row["LPB_DATE_RENOTIFY"].ToString().Trim();
                    //Letter_NO = row["LPB_LETTER_NO"].ToString().Trim();
                    //Pay_Notice_Date = row["LPB_PAY_NOTICE_DATE"].ToString().Trim();
                    //Fund_Code = row["LPB_FUND_CODE"].ToString().Trim();
                    //SW1 = row["LPB_SW1"].ToString().Trim();
                    //SW2 = row["LPB_SW2"].ToString().Trim();
                    //SW3 = row["LPB_SW3"].ToString().Trim();
                    //SW4 = row["LPB_SW4"].ToString().Trim();
                    //SW5 = row["LPB_SW5"].ToString().Trim();
                    //SW6 = row["LPB_SW6"].ToString().Trim();
                    //SW7 = row["LPB_SW7"].ToString().Trim();
                    //SW8 = row["LPB_SW8"].ToString().Trim();


                    //SSN_SW = row["LPB_SSN_SW"].ToString().Trim();
                    //BatchNO = row["LPB_BATCHNO"].ToString().Trim();
                    //CONS_Gallons = row["LPB_CONS_GALLONS"].ToString().Trim();
                    //CONS_Dollars = row["LPB_CONS_DOLLARS"].ToString().Trim();
                    //CONS_Months = row["LPB_CONS_MONTHS"].ToString().Trim();
                    //CONS_Gallons_NY = row["LPB_CONS_GALLONS_NY"].ToString().Trim();
                    //CONS_Dollars_NY = row["LPB_CONS_DOLLARS_NY"].ToString().Trim();
                    //CONS_Months_NY = row["LPB_CONS_MONTHS_NY"].ToString().Trim();
                    //WEATH_Intrest = row["LPB_WEATH_INTEREST"].ToString().Trim();
                    //WEATH_No_Rooms = row["LPB_WEATH_NOROOMS"].ToString().Trim();
                    //WEATH_Share = row["LPB_WEATH_SHARE"].ToString().Trim();
                    //WEATH_Before = row["LPB_WEATH_BEFORE"].ToString().Trim();
                    //Override = row["LPB_OVERRIDE"].ToString().Trim();
                    //APP_Applied = row["LPB_APP_APPLIED"].ToString().Trim();

                    //INCOMP_Assets = row["LPB_INCOMP_ASSET"].ToString().Trim();
                    //INCOMP_SSN = row["LPB_INCOMP_SSN"].ToString().Trim();
                    //INCOMP_Rent = row["LPB_INCOMP_RENT"].ToString().Trim();
                    //INCOMP_Util = row["LPB_INCOMP_UTIL"].ToString().Trim();
                    //Categ_Elig = row["LPB_CATEG_ELIG"].ToString().Trim();
                    //TKT_Balance = row["LPB_TKT_BALANCE"].ToString().Trim();
                    //DSS_ADFC = row["LPB_DSS_ADFC"].ToString().Trim();

                    //WAP_Date = row["LPB_WAP_DATE"].ToString().Trim();
                    //WAP_Operator = row["LPB_WAP_OPERATOR"].ToString().Trim();
                    //Add_Date = row["LPB_DATE_ADD"].ToString().Trim();
                    //Add_Operatop = row["LPB_ADD_OPERATOR"].ToString().Trim();
                    //LSTC_Date = row["LPB_DATE_LSTC"].ToString().Trim();
                    //LSTC_Operator = row["LPB_LSTC_OPERATOR"].ToString().Trim();

                    Award_Amount = ((string.IsNullOrEmpty(row["LPB_AMOUNT"].ToString().Trim()) || row["LPB_AMOUNT"].ToString().Trim() == "0.00") ? "0" : row["LPB_AMOUNT"].ToString().Trim());
                    //Paid = ((string.IsNullOrEmpty(row["LPB_PAID"].ToString().Trim()) || row["LPB_PAID"].ToString().Trim() == "0.00") ? "0" : row["LPB_PAID"].ToString().Trim());
                    //Awaiting_Dol = ((string.IsNullOrEmpty(row["LPB_AWAITING_DOL"].ToString().Trim()) || row["LPB_AWAITING_DOL"].ToString().Trim() == "0.00") ? "0" : row["LPB_AWAITING_DOL"].ToString().Trim());
                    //Assets = ((string.IsNullOrEmpty(row["LPB_ASSETS"].ToString().Trim()) || row["LPB_ASSETS"].ToString().Trim() == "0.00") ? "0" : row["LPB_ASSETS"].ToString().Trim());
                    //FAP_Income = ((string.IsNullOrEmpty(row["LPB_FAP_INCOME"].ToString().Trim()) || row["LPB_FAP_INCOME"].ToString().Trim() == "0.00") ? "0" : row["LPB_FAP_INCOME"].ToString().Trim());
                    //Reduce_Elig = ((string.IsNullOrEmpty(row["LPB_REDUCE_ELIG"].ToString().Trim()) || row["LPB_REDUCE_ELIG"].ToString().Trim() == "0.00") ? "0" : row["LPB_REDUCE_ELIG"].ToString().Trim());
                    //Remaining = ((string.IsNullOrEmpty(row["LPB_REMAINING"].ToString().Trim()) || row["LPB_REMAINING"].ToString().Trim() == "0.00") ? "0" : row["LPB_REMAINING"].ToString().Trim());
                    //CONS_Gallons = ((string.IsNullOrEmpty(row["LPB_CONS_GALLONS"].ToString().Trim()) || row["LPB_CONS_GALLONS"].ToString().Trim() == "0.00") ? "0" : row["LPB_CONS_GALLONS"].ToString().Trim());
                    //CONS_Dollars = ((string.IsNullOrEmpty(row["LPB_CONS_DOLLARS"].ToString().Trim()) || row["LPB_CONS_DOLLARS"].ToString().Trim() == "0.00") ? "0" : row["LPB_CONS_DOLLARS"].ToString().Trim());
                    //CONS_Gallons_NY = ((string.IsNullOrEmpty(row["LPB_CONS_GALLONS_NY"].ToString().Trim()) || row["LPB_CONS_GALLONS_NY"].ToString().Trim() == "0.00") ? "0" : row["LPB_CONS_GALLONS_NY"].ToString().Trim());
                    //CONS_Dollars_NY = ((string.IsNullOrEmpty(row["LPB_CONS_DOLLARS_NY"].ToString().Trim()) || row["LPB_CONS_DOLLARS_NY"].ToString().Trim() == "0.00") ? "0" : row["LPB_CONS_DOLLARS_NY"].ToString().Trim());
                    //TKT_Balance = ((string.IsNullOrEmpty(row["LPB_TKT_BALANCE"].ToString().Trim()) || row["LPB_TKT_BALANCE"].ToString().Trim() == "0.00") ? "0" : row["LPB_TKT_BALANCE"].ToString().Trim());
                    //HH_FS_Benefit = ((string.IsNullOrEmpty(row["LPB_HH_FS_BENIFIT"].ToString().Trim()) || row["LPB_HH_FS_BENIFIT"].ToString().Trim() == "0.00") ? "0" : row["LPB_HH_FS_BENIFIT"].ToString().Trim());
                }
            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string Year { get; set; }
        public string AppNo { get; set; }
        public string Seq { get; set; }
        public string Award_Type { get; set; }
        public string Award_Amount { get; set; }
        public string Award_Balance { get; set; }
        public string Paid { get; set; }
        public string Awaiting_Dol { get; set; }
        public string Assets { get; set; }
        public string Award_Date { get; set; }
        public string Caseworker { get; set; }
        public string Notice_Date { get; set; }
        public string Den_Notice_Date { get; set; }
        public string Resend { get; set; }
        public string Deny { get; set; }
        public string Age_dis { get; set; }
        public string Source { get; set; }
        public string Inc_Cert { get; set; }

        public string CATELIG_Cert { get; set; }
        public string Renter { get; set; }
        public string FAP_Income { get; set; }

        public string FAP_No_InHH { get; set; }
        public string Reason_Denied { get; set; }
        public string Action_Code { get; set; }
        public string AU { get; set; }

        public string Payment_How { get; set; }
        public string Disable { get; set; }
        public string Inc_Worker { get; set; }
        public string Inc_Date { get; set; }
        public string Benefit_Level { get; set; }
        public string MOD { get; set; }
        public string Elig_Assets { get; set; }
        public string Elig_Assets1 { get; set; }
        public string Elig_Income { get; set; }
        public string Elig_Fuel { get; set; }
        public string Elig_Rent { get; set; }

        public string Elig_Wthr { get; set; }
        public string Contingency { get; set; }
        public string Reduce_Elig { get; set; }
        public string Certified_Status { get; set; }
        public string Date_Renotify { get; set; }
        public string Letter_NO { get; set; }
        public string Pay_Notice_Date { get; set; }
        public string Fund_Code { get; set; }
        public string SW1 { get; set; }
        public string SW2 { get; set; }
        public string SW3 { get; set; }
        public string SW4 { get; set; }
        public string SW5 { get; set; }
        public string SW6 { get; set; }
        public string SW7 { get; set; }
        public string SW8 { get; set; }

        public string SSN_SW { get; set; }
        public string BatchNO { get; set; }
        public string Remaining { get; set; }
        public string CONS_Gallons { get; set; }
        public string CONS_Dollars { get; set; }
        public string CONS_Months { get; set; }
        public string CONS_Gallons_NY { get; set; }
        public string CONS_Dollars_NY { get; set; }
        public string CONS_Months_NY { get; set; }
        public string WEATH_Intrest { get; set; }
        public string WEATH_No_Rooms { get; set; }
        public string WEATH_Share { get; set; }
        public string WEATH_Before { get; set; }
        public string Override { get; set; }
        public string APP_Applied { get; set; }

        public string INCOMP_Assets { get; set; }
        public string INCOMP_SSN { get; set; }
        public string INCOMP_Rent { get; set; }
        public string INCOMP_Util { get; set; }
        public string Categ_Elig { get; set; }
        public string TKT_Balance { get; set; }
        public string HH_FS_Benefit { get; set; }
        public string DSS_ADFC { get; set; }

        public string WAP_Date { get; set; }
        public string WAP_Operator { get; set; }
        public string Add_Date { get; set; }
        public string Add_Operatop { get; set; }
        public string LSTC_Date { get; set; }
        public string LSTC_Operator { get; set; }

        public string Denied_Reason { get; set; }

        public string LiquidAsset { get; set; }
        public string RiskAsses { get; set; }

        #endregion
    }

    public class PAYMNETEntity
    {
        #region Constructors

        public PAYMNETEntity()
        {
                            Rec_Type =
                Agency = 
                Dept = 
                Prog = 
                Yaer = 
                AppNo = 
                Vendor_Seq = 
                Record_Seq = 
                Batch_NO = 
                Del_Vendor = 
                Del_Date = 
                Vendor = 
                Authr_Number = 
                Authr_Date = 
                Authr_Caseworker = 
                Authr_Type1 = 
                Authr_Type2 = 
                Authr_Amount1 = 
                Authr_Amount2 = 
                Invc_Number = 
                Invc_Date = 
                Amount_Dol = 
                Amount_Dol1 = 
                Amount_Dol2 = 
                Amount_Delivery = 
                Amount_Startup = 
                Amount_MISC = 
                Amount_Gallons = 
                Caseworker = 
                Original_Amount = 

                Vendor_PP = 
                MOR_PP = 
                MOR_Code = 
                Primary_Code = 
                Payment_HoW = 
                Payment_For = 
                Rent_Month = 
                Cash_Bulk = 
                Benefit_Level = 
                Action_Code = 
                Fund = 
                Emergency = 
                Emer_Pay = 
                Account = 
                Divide_Bill = 
                Voucher_No = 
                Check_No = 
                Check_Date = 
                Check_Type = 
                Payment_NO1 = 
                Cashed_Date = 
                Cashed_AMT = 
                BS = 
                Notify_Date = 
                Notify_SEO = 
                Contingency = 
                Meter = 
                Payment_NO = 

                Adj_Amount = 
                Reason_Code = 
                REC_Source = 
                Bill_RECD = 
                MOR_Diff_ppg = 
                MOR_Differential = 
                Account_OLD = 
                SDWDC = 
                Add_30001 = 
                Date_Add = 
                Add_Operator = 
                Lstc_Date =
                Lstc_Operator =
                ShortFall = 
                Authr_Type3 = 
                Authr_Amount3 = 
                Amount_Dol3 =
                OD_Lpb_Seq =
                Authrization_Type=
                Account_Number =
                Account_Switch =   string.Empty;
                //Auth_Total_Amount = string.Empty;

        }

        public PAYMNETEntity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type =
                Agency = 
                Dept = 
                Prog = 
                Yaer = 
                AppNo = 
                Vendor_Seq = 
                Record_Seq = 
                Batch_NO = 
                Del_Vendor = 
                Del_Date = 
                Vendor = 
                Authr_Number = 
                Authr_Date = 
                Authr_Caseworker = 
                Authr_Type1 = 
                Authr_Type2 = 
                Authr_Amount1 = 
                Authr_Amount2 = 
                Invc_Number = 
                Invc_Date = 
                Amount_Dol = 
                Amount_Dol1 = 
                Amount_Dol2 = 
                Amount_Delivery = 
                Amount_Startup = 
                Amount_MISC = 
                Amount_Gallons = 
                Caseworker = 
                Original_Amount = 

                Vendor_PP = 
                MOR_PP = 
                MOR_Code = 
                Primary_Code = 
                Payment_HoW = 
                Payment_For = 
                Rent_Month = 
                Cash_Bulk = 
                Benefit_Level = 
                Action_Code = 
                Fund = 
                Emergency = 
                Emer_Pay = 
                Account = 
                Divide_Bill = 
                Voucher_No = 
                Check_No = 
                Check_Date = 
                Check_Type = 
                Payment_NO1 = 
                Cashed_Date = 
                Cashed_AMT = 
                BS = 
                Notify_Date = 
                Notify_SEO = 
                Contingency = 
                Meter = 
                Payment_NO = 

                Adj_Amount = 
                Reason_Code = 
                REC_Source = 
                Bill_RECD = 
                MOR_Diff_ppg = 
                MOR_Differential = 
                Account_OLD = 
                SDWDC = 
                Add_30001 = 
                Date_Add =
                Add_Operator = 
                Lstc_Date =
                Lstc_Operator =
                ShortFall =
                Authr_Type3 =
                Authr_Amount3 =
                Amount_Dol3 =
                OD_Lpb_Seq =
                Authrization_Type=             
                Account_Number =
                Account_Switch = null;
                //Auth_Total_Amount = null;
            }
        }

        public PAYMNETEntity(bool Initialize, bool Initial_Nums)
        {
            if (Initialize)
            {
                Rec_Type =
                Agency =
                Dept =
                Prog =
                Yaer =
                AppNo =
                Vendor_Seq =
                Record_Seq =
                Batch_NO =
                Del_Vendor =
                Del_Date =
                Vendor =
                Authr_Number =
                Authr_Date =
                Authr_Caseworker =
                Authr_Type1 =
                Authr_Type2 =
                Authr_Amount1 =
                Authr_Amount2 =
                Invc_Number =
                Invc_Date =
                Amount_Dol =
                Amount_Dol1 =
                Amount_Dol2 =
                Amount_Delivery =
                Amount_Startup =
                Amount_MISC =
                Amount_Gallons =
                Caseworker =
                Original_Amount =

                Vendor_PP =
                MOR_PP =
                MOR_Code =
                Primary_Code =
                Payment_HoW =
                Payment_For =
                Rent_Month =
                Cash_Bulk =
                Benefit_Level =
                Action_Code =
                Fund =
                Emergency =
                Emer_Pay =
                Account =
                Divide_Bill =
                Voucher_No =
                Check_No =
                Check_Date =
                Check_Type =
                Payment_NO1 =
                Cashed_Date =
                Cashed_AMT =
                BS =
                Notify_Date =
                Notify_SEO =
                Contingency =
                Meter =
                Payment_NO =

                Adj_Amount =
                Reason_Code =
                REC_Source =
                Bill_RECD =
                MOR_Diff_ppg =
                MOR_Differential =
                Account_OLD =
                SDWDC =
                Add_30001 =
                Date_Add =
                Add_Operator =
                Lstc_Date =
                Lstc_Operator =
                ShortFall =
                Authr_Type3 =
                Authr_Amount3 =
                Amount_Dol3 =
                OD_Lpb_Seq =
                Authrization_Type=
                Account_Number =
                Account_Switch = null;
                //Auth_Total_Amount = null;
            }

            if (Initial_Nums)
            {
                Authr_Amount1 =
                Authr_Amount2 =
                Amount_Dol =
                Amount_Dol1 =
                Amount_Dol2 =
                Amount_Delivery =
                Amount_Startup =
                Amount_MISC =
                Amount_Gallons =
                Original_Amount =
                Vendor_PP =
                MOR_PP =
                Cashed_AMT =
                Adj_Amount =
                MOR_Diff_ppg =
                MOR_Differential =
                ShortFall =
                Authr_Amount3 =
                Amount_Dol3 = "0";
                //Auth_Total_Amount = "0";
            }
        }

        public PAYMNETEntity(PAYMNETEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type =Entity.Rec_Type;
                Agency = Entity.Agency;
                Dept = Entity.Dept;
                Prog = Entity.Prog;
                Yaer = Entity.Yaer;
                AppNo = Entity.AppNo;
                Vendor_Seq = Entity.Vendor_Seq;
                Record_Seq = Entity.Record_Seq;
                Batch_NO = Entity.Batch_NO;
                Del_Vendor = Entity.Del_Vendor;
                Del_Date = Entity.Del_Date;
                Vendor = Entity.Vendor;
                Authr_Number = Entity.Authr_Number;
                Authr_Date = Entity.Authr_Date;
                Authr_Caseworker = Entity.Authr_Caseworker;
                Authr_Type1 = Entity.Authr_Type1;
                Authr_Type2 = Entity.Authr_Type2;
                Authr_Amount1 = Entity.Authr_Amount1;
                Authr_Amount2 = Entity.Authr_Amount2;
                Invc_Number = Entity.Invc_Number;
                Invc_Date = Entity.Invc_Date;
                Amount_Dol = Entity.Amount_Dol;
                Amount_Dol1 = Entity.Amount_Dol1;
                Amount_Dol2 = Entity.Amount_Dol2;
                Amount_Delivery = Entity.Amount_Delivery;
                Amount_Startup = Entity.Amount_Startup;
                Amount_MISC = Entity.Amount_MISC;
                Amount_Gallons = Entity.Amount_Gallons;
                Caseworker = Entity.Caseworker;
                Original_Amount = Entity.Original_Amount;

                Vendor_PP = Entity.Vendor_PP;
                MOR_PP = Entity.MOR_PP;
                MOR_Code = Entity.MOR_Code;
                Primary_Code = Entity.Primary_Code;
                Payment_HoW = Entity.Payment_HoW;
                Payment_For = Entity.Payment_For;
                Rent_Month = Entity.Rent_Month;
                Cash_Bulk = Entity.Cash_Bulk;
                Benefit_Level = Entity.Benefit_Level;
                Action_Code = Entity.Action_Code;
                Fund = Entity.Fund;
                Emergency = Entity.Emergency;
                Emer_Pay = Entity.Emer_Pay;
                Account = Entity.Account;
                Divide_Bill = Entity.Divide_Bill;
                Voucher_No = Entity.Voucher_No;
                Check_No = Entity.Check_No;
                Check_Date = Entity.Check_Date;
                Check_Type = Entity.Check_Type;
                Payment_NO1 = Entity.Payment_NO1;
                Cashed_Date = Entity.Cashed_Date;
                Cashed_AMT = Entity.Cashed_AMT;
                BS = Entity.BS;
                Notify_Date = Entity.Notify_Date;
                Notify_SEO = Entity.Notify_SEO;
                Contingency = Entity.Contingency;
                Meter = Entity.Meter;
                Payment_NO = Entity.Payment_NO;

                Adj_Amount = Entity.Adj_Amount;
                Reason_Code = Entity.Reason_Code;
                REC_Source = Entity.REC_Source;
                Bill_RECD = Entity.Bill_RECD;
                MOR_Diff_ppg = Entity.MOR_Diff_ppg;
                MOR_Differential = Entity.MOR_Differential;
                Account_OLD = Entity.Account_OLD;
                SDWDC = Entity.SDWDC;
                Add_30001 = Entity.Add_30001;
                Date_Add = Entity.Date_Add;
                Add_Operator = Entity.Add_Operator;
                Lstc_Date = Entity.Lstc_Date;
                Lstc_Operator = Entity.Lstc_Operator;
                ShortFall = Entity.ShortFall;

                Authr_Type3 = Entity.Authr_Type3;
                Authr_Amount3 = Entity.Authr_Amount3;
                Amount_Dol3 = Entity.Amount_Dol3;
                OD_Lpb_Seq = Entity.OD_Lpb_Seq;
                Authrization_Type = Entity.Authrization_Type;
                Account_Number = Entity.Account_Number;
                Account_Switch = Entity.Account_Switch;
                //Auth_Total_Amount = Entity.Auth_Total_Amount;
            }
        }

        public PAYMNETEntity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                Agency = row["PMR_AGENCY"].ToString();
                Dept = row["PMR_DEPT"].ToString();
                Prog = row["PMR_PROGRAM"].ToString();
                Yaer = row["PMR_YEAR"].ToString();
                AppNo = row["PMR_APPL_NO"].ToString();
                Vendor_Seq = row["PMR_VENDOR_SEQ"].ToString();
                Record_Seq = row["PMR_REC_SEQ"].ToString();
                Batch_NO = row["PMR_BATCH_NO"].ToString();
                Del_Vendor = row["PMR_DEL_VENDOR"].ToString();
                Del_Date = row["PMR_DEL_DATE"].ToString().Trim();
                Vendor = row["PMR_VENDOR"].ToString();
                Authr_Number = row["PMR_AUTH_NUM"].ToString();
                Authr_Date = row["PMR_AUTH_DATE"].ToString().Trim();
                Authr_Caseworker = row["PMR_AUTH_CASEWORKER"].ToString();
                Authr_Type1 = row["PMR_AUTH_TYPE1"].ToString();
                Authr_Type2 = row["PMR_AUTH_TYPE2"].ToString();
                Authr_Amount1 = row["PMR_AUTH_AMOUNT1"].ToString().Trim();
                Authr_Amount2 = row["PMR_AUTH_AMOUNT2"].ToString().Trim();
                Invc_Number = row["PMR_INVOICE_NUM"].ToString();
                Invc_Date = row["PMR_INVOICE_DATE"].ToString().Trim();
                Amount_Dol = row["PMR_AMOUNT_DOL"].ToString();
                Amount_Dol1 = row["PMR_AMOUNT_DOL1"].ToString();
                Amount_Dol2 = row["PMR_AMOUNT_DOL2"].ToString();
                Amount_Delivery = row["PMR_AMOUNT_DELIVERY"].ToString();
                Amount_Startup = row["PMR_AMOUNT_STARTUP"].ToString();
                Amount_MISC = row["PMR_AMOUNT_MISC"].ToString();
                Amount_Gallons = row["PMR_AMOUNT_GAL"].ToString();
                Caseworker = row["PMR_CASEWORKER"].ToString();
                Original_Amount = row["PMR_ORIGINAL_AMOUNT"].ToString().Trim();

                Vendor_PP = row["PMR_VENDOR_PP"].ToString();
                MOR_PP = row["PMR_MOR_PP"].ToString();
                MOR_Code = row["PMR_MOR_CODE"].ToString();
                Primary_Code = row["PMR_PRIMARY_CODE"].ToString();
                Payment_HoW = row["PMR_PAYMENT_HOW"].ToString();
                Payment_For = row["PMR_PAYMENT_FOR"].ToString();
                Rent_Month = row["PMR_RENT_MONTH"].ToString();
                Cash_Bulk = row["PMR_CASH_BULK"].ToString();
                Benefit_Level = row["PMR_BENIFIT_LEVEL"].ToString();
                Action_Code = row["PMR_ACTION_CODE"].ToString();
                Fund = row["PMR_FUND"].ToString();
                Emergency = row["PMR_EMERGENCY"].ToString();
                Emer_Pay = row["PMR_EMER_PAY"].ToString();
                Account = row["PMR_ACCOUNT"].ToString();
                Divide_Bill = row["PMR_DIVIDE_BILL"].ToString();
                Voucher_No = row["PMR_VOUCH_NO"].ToString().Trim();
                Check_No = row["PMR_CHECK_NO"].ToString();
                Check_Date = row["PMR_CHK_DATE"].ToString().Trim();
                Check_Type = row["PMR_CHECK_TYPE"].ToString();
                Payment_NO1 = row["PMR_PAYMENT_NO1"].ToString();
                Cashed_Date = row["PMR_CASHED_DATE"].ToString().Trim();
                Cashed_AMT = row["PMR_CASHED_AMT"].ToString();
                BS = row["PMR_BS"].ToString();
                Notify_Date = row["PMR_NOTIFY_DATE"].ToString().Trim();
                Notify_SEO = row["PMR_NOTIFY_SEQ"].ToString();
                Contingency = row["PMR_CONTINGENCY"].ToString();
                Meter = row["PMR_METER"].ToString();
                Payment_NO = row["PMR_PAYMENT_NO"].ToString();

                Adj_Amount = row["PMR_ADJ_AMOUNT"].ToString();
                Reason_Code = row["PMR_REASON_CODE"].ToString();
                REC_Source = row["PMR_REC_SOURCE"].ToString();
                Bill_RECD = row["PMR_BILL_RECD"].ToString();
                MOR_Diff_ppg = row["PMR_MOR_DIFF_PPG"].ToString();
                MOR_Differential = row["PMR_MOR_DIFFERENTIAL"].ToString();
                Account_OLD = row["PMR_ACCOUNT_OLD"].ToString();
                SDWDC = row["PMR_SDWDC"].ToString();
                Add_30001 = row["PMR_30001_ADD"].ToString();
                Date_Add = row["PMR_DATE_ADD"].ToString();
                Add_Operator = row["PMR_ADD_OPERATOR"].ToString();
                Lstc_Date = row["PMR_DATE_LSTC"].ToString();
                Lstc_Operator = row["PMR_LSTC_OPERATOR"].ToString();
                ShortFall = row["PMR_SHORTFALL"].ToString();

                Authr_Type3 = row["PMR_AUTH_TYPE3"].ToString();
                Authr_Amount3 = row["PMR_AUTH_AMOUNT3"].ToString();
                Amount_Dol3 = row["PMR_AMOUNT_DOL3"].ToString();
                OD_Lpb_Seq =  row["PMR_OD_LPBSEQ"].ToString();
                Authrization_Type = row["PMR_AUTHORIZATION_TYPE"].ToString();
                Account_Number = row["PMR_ACCOUNT_NO"].ToString();
                Account_Switch = row["PMR_ACCOUNT_SWITCH"].ToString();
                //Auth_Total_Amount = row["PMR_AUTH_AMOUNT"].ToString();
            }
        }

        public PAYMNETEntity(DataRow row,string strTable)
        {
            if (strTable == "TRIGGERS")
            {
                if (row != null)
                {
                    //Rec_Type = "U";
                    Agency = row["PMR_AGENCY"].ToString();
                    Dept = row["PMR_DEPT"].ToString();
                    Prog = row["PMR_PROGRAM"].ToString();
                    Yaer = row["PMR_YEAR"].ToString();
                    AppNo = row["PMR_APPL_NO"].ToString();
                    //Vendor_Seq = row["PMR_VENDOR_SEQ"].ToString();
                    //Record_Seq = row["PMR_REC_SEQ"].ToString();
                    //Batch_NO = row["PMR_BATCH_NO"].ToString();
                    //Del_Vendor = row["PMR_DEL_VENDOR"].ToString();
                    //Del_Date = row["PMR_DEL_DATE"].ToString().Trim();
                    //Vendor = row["PMR_VENDOR"].ToString();
                    //Authr_Number = row["PMR_AUTH_NUM"].ToString();
                    //Authr_Date = row["PMR_AUTH_DATE"].ToString().Trim();
                    //Authr_Caseworker = row["PMR_AUTH_CASEWORKER"].ToString();
                    Authr_Type1 = row["PMR_AUTH_TYPE1"].ToString();
                    Authr_Type2 = row["PMR_AUTH_TYPE2"].ToString();
                    //Authr_Amount1 = row["PMR_AUTH_AMOUNT1"].ToString().Trim();
                    //Authr_Amount2 = row["PMR_AUTH_AMOUNT2"].ToString().Trim();
                    //Invc_Number = row["PMR_INVOICE_NUM"].ToString();
                    //Invc_Date = row["PMR_INVOICE_DATE"].ToString().Trim();
                    //Amount_Dol = row["PMR_AMOUNT_DOL"].ToString();
                    //Amount_Dol1 = row["PMR_AMOUNT_DOL1"].ToString();
                    //Amount_Dol2 = row["PMR_AMOUNT_DOL2"].ToString();
                    //Amount_Delivery = row["PMR_AMOUNT_DELIVERY"].ToString();
                    //Amount_Startup = row["PMR_AMOUNT_STARTUP"].ToString();
                    //Amount_MISC = row["PMR_AMOUNT_MISC"].ToString();
                    //Amount_Gallons = row["PMR_AMOUNT_GAL"].ToString();
                    //Caseworker = row["PMR_CASEWORKER"].ToString();
                    //Original_Amount = row["PMR_ORIGINAL_AMOUNT"].ToString().Trim();

                    //Vendor_PP = row["PMR_VENDOR_PP"].ToString();
                    //MOR_PP = row["PMR_MOR_PP"].ToString();
                    //MOR_Code = row["PMR_MOR_CODE"].ToString();
                    //Primary_Code = row["PMR_PRIMARY_CODE"].ToString();
                    //Payment_HoW = row["PMR_PAYMENT_HOW"].ToString();
                    Payment_For = row["PMR_PAYMENT_FOR"].ToString();
                    //Rent_Month = row["PMR_RENT_MONTH"].ToString();
                    //Cash_Bulk = row["PMR_CASH_BULK"].ToString();
                    //Benefit_Level = row["PMR_BENIFIT_LEVEL"].ToString();
                    //Action_Code = row["PMR_ACTION_CODE"].ToString();
                    //Fund = row["PMR_FUND"].ToString();
                    //Emergency = row["PMR_EMERGENCY"].ToString();
                    //Emer_Pay = row["PMR_EMER_PAY"].ToString();
                    //Account = row["PMR_ACCOUNT"].ToString();
                    //Divide_Bill = row["PMR_DIVIDE_BILL"].ToString();
                    //Voucher_No = row["PMR_VOUCH_NO"].ToString().Trim();
                    //Check_No = row["PMR_CHECK_NO"].ToString();
                    Check_Date = row["PMR_CHK_DATE"].ToString().Trim();
                    //Check_Type = row["PMR_CHECK_TYPE"].ToString();
                    //Payment_NO1 = row["PMR_PAYMENT_NO1"].ToString();
                    //Cashed_Date = row["PMR_CASHED_DATE"].ToString().Trim();
                    //Cashed_AMT = row["PMR_CASHED_AMT"].ToString();
                    //BS = row["PMR_BS"].ToString();
                    //Notify_Date = row["PMR_NOTIFY_DATE"].ToString().Trim();
                    //Notify_SEO = row["PMR_NOTIFY_SEQ"].ToString();
                    //Contingency = row["PMR_CONTINGENCY"].ToString();
                    //Meter = row["PMR_METER"].ToString();
                    //Payment_NO = row["PMR_PAYMENT_NO"].ToString();

                    //Adj_Amount = row["PMR_ADJ_AMOUNT"].ToString();
                    //Reason_Code = row["PMR_REASON_CODE"].ToString();
                    //REC_Source = row["PMR_REC_SOURCE"].ToString();
                    //Bill_RECD = row["PMR_BILL_RECD"].ToString();
                    //MOR_Diff_ppg = row["PMR_MOR_DIFF_PPG"].ToString();
                    //MOR_Differential = row["PMR_MOR_DIFFERENTIAL"].ToString();
                    //Account_OLD = row["PMR_ACCOUNT_OLD"].ToString();
                    //SDWDC = row["PMR_SDWDC"].ToString();
                    //Add_30001 = row["PMR_30001_ADD"].ToString();
                    //Date_Add = row["PMR_DATE_ADD"].ToString();
                    //Add_Operator = row["PMR_ADD_OPERATOR"].ToString();
                    //Lstc_Date = row["PMR_DATE_LSTC"].ToString();
                    //Lstc_Operator = row["PMR_LSTC_OPERATOR"].ToString();
                    //ShortFall = row["PMR_SHORTFALL"].ToString();

                    Authr_Type3 = row["PMR_AUTH_TYPE3"].ToString();
                    //Authr_Amount3 = row["PMR_AUTH_AMOUNT3"].ToString();
                    //Amount_Dol3 = row["PMR_AMOUNT_DOL3"].ToString();
                    //OD_Lpb_Seq = row["PMR_OD_LPBSEQ"].ToString();
                    ////Auth_Total_Amount = row["PMR_AUTH_AMOUNT"].ToString();
                }
            }
        } 
        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string Yaer { get; set; }
        public string AppNo { get; set; }
        public string Vendor_Seq { get; set; }
        public string Record_Seq { get; set; }
        public string Batch_NO { get; set; }
        public string Del_Vendor { get; set; }
        public string Del_Date { get; set; }
        public string Vendor { get; set; }
        public string Authr_Number { get; set; }
        public string Authr_Date { get; set; }
        public string Authr_Caseworker { get; set; }
        public string Authr_Type1 { get; set; }
        public string Authr_Type2 { get; set; }
        public string Authr_Amount1 { get; set; }
        public string Authr_Amount2 { get; set; }
        public string Invc_Number { get; set; }
        public string Invc_Date { get; set; }
        public string Amount_Dol { get; set; }
        public string Amount_Dol1 { get; set; }
        public string Amount_Dol2 { get; set; }
        public string Amount_Delivery { get; set; }
        public string Amount_Startup { get; set; }
        public string Amount_MISC { get; set; }
        public string Amount_Gallons { get; set; }
        public string Caseworker { get; set; }
        public string Original_Amount { get; set; }

        public string Vendor_PP { get; set; }
        public string MOR_PP { get; set; }
        public string MOR_Code { get; set; }
        public string Primary_Code { get; set; }
        public string Payment_HoW { get; set; }
        public string Payment_For { get; set; }
        public string Rent_Month { get; set; }
        public string Cash_Bulk { get; set; }
        public string Benefit_Level { get; set; }
        public string Action_Code { get; set; }
        public string Fund { get; set; }
        public string Emergency { get; set; }
        public string Emer_Pay { get; set; }
        public string Account { get; set; }
        public string Divide_Bill { get; set; }
        public string Voucher_No { get; set; }
        public string Check_No { get; set; }
        public string Check_Date { get; set; }
        public string Check_Type { get; set; }
        public string Payment_NO1 { get; set; }
        public string Cashed_Date { get; set; }
        public string Cashed_AMT { get; set; }
        public string BS { get; set; }
        public string Notify_Date { get; set; }
        public string Notify_SEO { get; set; }
        public string Contingency { get; set; }
        public string Meter { get; set; }
        public string Payment_NO { get; set; }

        public string Adj_Amount { get; set; }
        public string Reason_Code { get; set; }
        public string REC_Source { get; set; }
        public string Bill_RECD { get; set; }
        public string MOR_Diff_ppg { get; set; }
        public string MOR_Differential { get; set; }
        public string Account_OLD { get; set; }
        public string SDWDC { get; set; }
        public string Add_30001 { get; set; }
        public string Date_Add { get; set; }
        public string Add_Operator { get; set; }
        public string Lstc_Date { get; set; }
        public string Lstc_Operator { get; set; }
        public string ShortFall { get; set; }
        //public string Auth_Total_Amount { get; set; }

        public string Authr_Type3 { get; set; }
        public string Authr_Amount3 { get; set; }
        public string Amount_Dol3 { get; set; }
        public string OD_Lpb_Seq { get; set; }

        public string Authrization_Type { get; set; }

        public string Account_Number { get; set; }
        public string Account_Switch { get; set; }

        #endregion
    }

    public class LVR_Entity
    {
        #region Constructors

        public LVR_Entity()
        {
            Award = string.Empty;
            Award_Amount =
            Benefit_Level = "0";
            Certified_Status =
            Fund_Code = string.Empty;
            FAP_Income = "0";
            FAP_Members_In_House = "0";
            Contingency = 
            Elig_Assets = 
            Elig_Assets1 = 
            Elig_Income =
            Elig_Fuel =
            Elig_Rent =
            Elig_Wthr = "N";
            Poverty =
            SW1 =
            SW2 =
            SW3 =
            SW4 =
            SW5 =
            SW6 =
            SW7 =
            SSN_SW =
            Food_Stamps = 
            MOD =
            Seq = string.Empty;

            DeniedReason = string.Empty;
        }
 
        #endregion

        #region Properties

        public string Award { get; set; }
        public string Award_Amount { get; set; }
        public string Benefit_Level { get; set; }
        public string Certified_Status { get; set; }
        public string Fund_Code { get; set; }
        public string FAP_Income { get; set; }
        public string FAP_Members_In_House { get; set; }
        public string Contingency { get; set; }
        public string Elig_Assets { get; set; }
        public string Elig_Assets1 { get; set; }
        public string Elig_Income { get; set; }
        public string Elig_Fuel { get; set; }
        public string Elig_Rent { get; set; }
        public string Elig_Wthr { get; set; }
        public string Poverty { get; set; }
        public string SW1 { get; set; }
        public string SW2 { get; set; }
        public string SW3 { get; set; }
        public string SW4 { get; set; }
        public string SW5 { get; set; }
        public string SW6 { get; set; }
        public string SW7 { get; set; }
        public string SSN_SW { get; set; }
        public string MOD { get; set; }
        public string Food_Stamps { get; set; }
        public string Seq { get; set; }
        public string DeniedReason { get; set; }

        #endregion
    }

    public class TMSRISKEntity
    {
        #region Constructors

        public TMSRISKEntity()
        {
            Rec_Type =
            Agency =
            Dept =
            Prog =
            Year =
            AppNo =
            GMI =
            GMI_FICA =
            GMI_ADJ =
            Assests =
            Inc_Total =
            Inc_Disp =
            Exp_Rent =
            Inc_Insr =
            Exp_Home =
            Exp_Med =
            Exp_Alny =
            Exp_DayCare =
            Exp_A_Food =
            Exp_B_Food =
            Exp_C_Food =
            Exp_A_Util =
            Exp_B_Util =
            Exp_C_Util =
            Exp_D_Util =
            Exp_Tel =
            Mon_Exp =
            HH_Count =
            Add_Date =
            Add_Operatop =
            LSTC_Date =
            LSTC_Operator = string.Empty;
        }

        public TMSRISKEntity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type =
                Agency =
                Dept =
                Prog =
                Year =
                AppNo =
                GMI =
                GMI_FICA =
                GMI_ADJ =
                Assests =
                Inc_Total =
                Inc_Disp =
                Exp_Rent =
                Inc_Insr =
                Exp_Home =
                Exp_Med =
                Exp_Alny =
                Exp_DayCare = 
                Exp_A_Food =
                Exp_B_Food =
                Exp_C_Food =
                Exp_A_Util =
                Exp_B_Util =
                Exp_C_Util =
                Exp_D_Util =
                Exp_Tel =
                Mon_Exp =
                HH_Count =
                Add_Date =
                Add_Operatop =
                LSTC_Date =
                LSTC_Operator = null;
            }
        }

        public TMSRISKEntity(TMSRISKEntity Entity)
        {
            Rec_Type = Entity.Rec_Type;
            Agency = Entity.Agency;
            Dept = Entity.Dept;
            Prog = Entity.Prog;
            Year = Entity.Year;
            AppNo = Entity.AppNo;
            GMI = Entity.GMI;
            GMI_FICA = Entity.GMI_ADJ;
            GMI_ADJ = Entity.GMI_FICA;
            Assests = Entity.Assests;
            Inc_Total = Entity. Inc_Total;
            Inc_Disp = Entity.Inc_Disp;
            Exp_Rent = Entity.Exp_Rent;
            Inc_Insr = Entity.Inc_Insr;
            Exp_Home = Entity.Exp_Home;
            Exp_Med = Entity.Exp_Med;
            Exp_Alny = Entity.Exp_Alny;
            Exp_DayCare = Entity.Exp_DayCare;
            Exp_A_Food = Entity.Exp_A_Food;
            Exp_B_Food = Entity.Exp_B_Food;
            Exp_C_Food = Entity.Exp_C_Food;
            Exp_A_Util = Entity.Exp_A_Util;
            Exp_B_Util = Entity.Exp_B_Util;
            Exp_C_Util = Entity.Exp_C_Util;
            Exp_D_Util = Entity.Exp_D_Util;
            Exp_Tel = Entity.Exp_Tel;
            Mon_Exp = Entity.Mon_Exp;
            HH_Count = Entity.HH_Count;
            Add_Date = Entity.Add_Date;
            Add_Operatop = Entity.Add_Operatop;
            LSTC_Date = Entity.LSTC_Date;
            LSTC_Operator =  Entity.LSTC_Operator;
        }

        public TMSRISKEntity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U"; 
                Agency = row["TMSRISK_AGENCY"].ToString();
                Dept = row["TMSRISK_DEPT"].ToString();
                Prog = row["TMSRISK_PROGRAM"].ToString();
                Year = row["TMSRISK_YEAR"].ToString();
                AppNo = row["TMSRISK_APP_NO"].ToString();
                GMI = row["TMSRISK_GMI"].ToString();
                GMI_FICA = row["TMSRISK_GMI_FICA"].ToString();
                GMI_ADJ = row["TMSRISK_GMI_ADJ"].ToString();
                Assests = row["TMSRISK_ASSETS"].ToString();
                Inc_Total = row["TMSRISK_INC_TOTAL"].ToString();
                Inc_Disp = row["TMSRISK_INC_DISP"].ToString();
                Exp_Rent = row["TMSRISK_EXP_RENT"].ToString();
                Inc_Insr = row["TMSRISK_EXP_INSR"].ToString();
                Exp_Home = row["TMSRISK_EXP_HOME"].ToString();
                Exp_Med = row["TMSRISK_EXP_MED"].ToString();
                Exp_Alny = row["TMSRISK_EXP_ALNY"].ToString();
                Exp_DayCare = row["TMSRISK_EXP_DAYCARE"].ToString();
                Exp_A_Food = row["TMSRISK_EXP_A_FOOD"].ToString();
                Exp_B_Food = row["TMSRISK_EXP_B_FOOD"].ToString();
                Exp_C_Food = row["TMSRISK_EXP_C_FOOD"].ToString();
                Exp_A_Util = row["TMSRISK_EXP_A_UTIL"].ToString();
                Exp_B_Util = row["TMSRISK_EXP_B_UTIL"].ToString();
                Exp_C_Util = row["TMSRISK_EXP_C_UTIL"].ToString();
                Exp_D_Util = row["TMSRISK_EXP_D_UTIL"].ToString();
                Exp_Tel = row["TMSRISK_EXP_TEL"].ToString();
                Mon_Exp = row["TMSRISK_MON_EXP"].ToString();
                HH_Count = row["TMSRISK_HH_COUNT"].ToString();
                Add_Date = row["TMSRISK_DATE_ADD"].ToString();
                Add_Operatop = row["TMSRISK_ADD_OPERATOR"].ToString();
                LSTC_Date = row["TMSRISK_DATE_LSTC"].ToString();
                LSTC_Operator = row["TMSRISK_LSTC_OPERATOR"].ToString();
            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string Year { get; set; }
        public string AppNo { get; set; }
        public string GMI { get; set; }

        public string GMI_FICA { get; set; }
        public string GMI_ADJ { get; set; }
        public string Assests { get; set; }
        public string Inc_Total { get; set; }
        public string Inc_Disp { get; set; }
        public string Exp_Rent { get; set; }
        public string Inc_Insr { get; set; }
        public string Exp_Home { get; set; }
        public string Exp_Med { get; set; }
        public string Exp_Alny { get; set; }
        public string Exp_DayCare { get; set; }
        public string Exp_A_Food { get; set; }
        public string Exp_B_Food { get; set; }

        public string Exp_C_Food { get; set; }
        public string Exp_A_Util { get; set; }
        public string Exp_B_Util { get; set; }
        public string Exp_C_Util { get; set; }
        public string Exp_D_Util { get; set; }
        public string Exp_Tel { get; set; }
        public string Mon_Exp { get; set; }
        public string HH_Count { get; set; }
        public string Add_Date { get; set; }
        public string Add_Operatop { get; set; }
        public string LSTC_Date { get; set; }
        public string LSTC_Operator { get; set; }

        #endregion
    }

    public class FUELCNTLEntity
    {
        #region Constructors

        public FUELCNTLEntity()
        {
            Rec_Type =
            FCNTL_YEAR=
            Benefit_Type =
            Award_Type =
            Start_Date =
            End_Date =
            L1_Vulner =
            L2_Vulner =
            L3_Vulner =
            L4_Vulner =
            L5_Vulner =
            L1_NonVulner =
            L2_NonVulner =
            L3_NonVulner =
            L4_NonVulner =
            L5_NonVulner = 
            Add_Date = 
            Add_Operator = 
            Lstc_Date =
            Lstc_Operator = string.Empty;
        }

        public FUELCNTLEntity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type = 
                FCNTL_YEAR=
                Benefit_Type = 
                Award_Type = 
                Start_Date = 
                End_Date = 
                L1_Vulner = 
                L2_Vulner = 
                L3_Vulner = 
                L4_Vulner = 
                L5_Vulner = 
                L1_NonVulner = 
                L2_NonVulner = 
                L3_NonVulner = 
                L4_NonVulner = 
                L5_NonVulner = 
                Add_Date = 
                Add_Operator = 
                Lstc_Date =
                Lstc_Operator = null;
            }
        }

        public FUELCNTLEntity(FUELCNTLEntity Entity)
        {
            Rec_Type = Entity.Rec_Type;
            FCNTL_YEAR = Entity.FCNTL_YEAR;
            Benefit_Type = Entity.Benefit_Type;
            Award_Type = Entity.Award_Type;
            Start_Date = Entity.Start_Date;
            End_Date = Entity.End_Date;
            L1_Vulner = Entity.L1_Vulner;
            L2_Vulner = Entity.L2_Vulner;
            L3_Vulner = Entity.L3_Vulner;
            L4_Vulner = Entity.L4_Vulner;
            L5_Vulner = Entity.L5_Vulner;
            L1_NonVulner = Entity.L1_NonVulner;
            L2_NonVulner = Entity.L2_NonVulner;
            L3_NonVulner = Entity.L3_NonVulner;
            L4_NonVulner = Entity.L4_NonVulner;
            L5_NonVulner = Entity.L5_NonVulner;

            Add_Date = Entity.L5_NonVulner;
            Add_Operator = Entity.L5_NonVulner;
            Lstc_Date = Entity.L5_NonVulner;
            Lstc_Operator = Entity.L5_NonVulner;
        }

        public FUELCNTLEntity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                FCNTL_YEAR = row["FCNTL_YEAR"].ToString();
                Benefit_Type = row["FCNTL_BEN_TYPE"].ToString();
                Award_Type = row["FCNTL_Award"].ToString();
                Start_Date = row["FCNTL_SDate"].ToString();
                End_Date = row["FCNTL_EDate"].ToString();
                //L1_Vulner = row["FUEL_L1_Vulner"].ToString();
                //L2_Vulner = row["FUEL_L2_Vulner"].ToString();
                //L3_Vulner = row["FUEL_L3_Vulner"].ToString();
                //L4_Vulner = row["FUEL_L4_Vulner"].ToString();
                //L5_Vulner = row["FUEL_L5_Vulner"].ToString();
                //L1_NonVulner = row["FUEL_L1_NonVulner"].ToString();
                //L2_NonVulner = row["FUEL_L2_NonVulner"].ToString();
                //L3_NonVulner = row["FUEL_L3_NonVulner"].ToString();
                //L4_NonVulner = row["FUEL_L4_NonVulner"].ToString();
                //L5_NonVulner = row["FUEL_L5_NonVulner"].ToString();

                //L1_Vulner = ((string.IsNullOrEmpty(row["FCNTL_L1_Vul"].ToString().Trim()) || row["FCNTL_L1_Vul"].ToString().Trim() == "0.00000") ? "0" : row["FCNTL_L1_Vul"].ToString().Trim());
                L1_Vulner = ((string.IsNullOrEmpty(row["FCNTL_L1_Vul"].ToString().Trim()) || row["FCNTL_L1_Vul"].ToString().Trim() == "0.00") ? "0" : row["FCNTL_L1_Vul"].ToString().Trim());
                L2_Vulner = ((string.IsNullOrEmpty(row["FCNTL_L2_Vul"].ToString().Trim()) || row["FCNTL_L2_Vul"].ToString().Trim() == "0.00") ? "0" : row["FCNTL_L2_Vul"].ToString().Trim());
                L3_Vulner = ((string.IsNullOrEmpty(row["FCNTL_L3_Vul"].ToString().Trim()) || row["FCNTL_L3_Vul"].ToString().Trim() == "0.00") ? "0" : row["FCNTL_L3_Vul"].ToString().Trim());
                L4_Vulner = ((string.IsNullOrEmpty(row["FCNTL_L4_Vul"].ToString().Trim()) || row["FCNTL_L4_Vul"].ToString().Trim() == "0.00") ? "0" : row["FCNTL_L4_Vul"].ToString().Trim());
                L5_Vulner = ((string.IsNullOrEmpty(row["FCNTL_L5_Vul"].ToString().Trim()) || row["FCNTL_L5_Vul"].ToString().Trim() == "0.00") ? "0" : row["FCNTL_L5_Vul"].ToString().Trim());

                L1_NonVulner = ((string.IsNullOrEmpty(row["FCNTL_L1_NonVul"].ToString().Trim()) || row["FCNTL_L1_NonVul"].ToString().Trim() == "0.00") ? "0" : row["FCNTL_L1_NonVul"].ToString().Trim());
                L2_NonVulner = ((string.IsNullOrEmpty(row["FCNTL_L2_NonVul"].ToString().Trim()) || row["FCNTL_L2_NonVul"].ToString().Trim() == "0.00") ? "0" : row["FCNTL_L2_NonVul"].ToString().Trim());
                L3_NonVulner = ((string.IsNullOrEmpty(row["FCNTL_L3_NonVul"].ToString().Trim()) || row["FCNTL_L3_NonVul"].ToString().Trim() == "0.00") ? "0" : row["FCNTL_L3_NonVul"].ToString().Trim());
                L4_NonVulner = ((string.IsNullOrEmpty(row["FCNTL_L4_NonVul"].ToString().Trim()) || row["FCNTL_L4_NonVul"].ToString().Trim() == "0.00") ? "0" : row["FCNTL_L4_NonVul"].ToString().Trim());
                L5_NonVulner = ((string.IsNullOrEmpty(row["FCNTL_L5_NonVul"].ToString().Trim()) || row["FCNTL_L5_NonVul"].ToString().Trim() == "0.00") ? "0" : row["FCNTL_L5_NonVul"].ToString().Trim());

                Add_Date = row["FCNTL_DATE_ADD"].ToString();
                Add_Operator = row["FCNTL_ADD_OPERATOR"].ToString();
                Lstc_Date = row["FCNTL_DATE_LSTC"].ToString();
                Lstc_Operator = row["FCNTL_LSTC_OPERATOR"].ToString();
            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string FCNTL_YEAR { get; set; }
        public string Benefit_Type { get; set; }
        public string Award_Type { get; set; }
        public string Start_Date { get; set; }
        public string End_Date { get; set; }

        public string L1_Vulner { get; set; }
        public string L2_Vulner { get; set; }
        public string L3_Vulner { get; set; }
        public string L4_Vulner { get; set; }
        public string L5_Vulner { get; set; }

        public string L1_NonVulner { get; set; }
        public string L2_NonVulner { get; set; }
        public string L3_NonVulner { get; set; }
        public string L4_NonVulner { get; set; }
        public string L5_NonVulner { get; set; }

        public string Add_Date { get; set; }
        public string Add_Operator { get; set; }
        public string Lstc_Date { get; set; }
        public string Lstc_Operator { get; set; }

        #endregion
    }

    public class S_NetEntity
    {
        #region Constructors

        public S_NetEntity()
        {
            Award_Type =
            Vulner_Amount =
            NonVulner_Amount = string.Empty;
        }

        public S_NetEntity(bool Initialize)
        {
            if (Initialize)
            {
                Award_Type =
                Vulner_Amount =
                NonVulner_Amount = null;
            }
        }

        public S_NetEntity(S_NetEntity Entity)
        {
            Award_Type = Entity.Award_Type.Trim();
            Vulner_Amount = Entity.Vulner_Amount;
            NonVulner_Amount = Entity.NonVulner_Amount; 
        }

        public S_NetEntity(FUELCNTLEntity Entity, string Level, bool Vulner_SW)
        {
            if (Entity != null)
            {
                Award_Type = Entity.Award_Type.Trim();
                switch (Level)
                {
                    case "1": Vulner_Amount = Entity.L1_Vulner; break;
                    case "2": Vulner_Amount = Entity.L2_Vulner; break;
                    case "3": Vulner_Amount = Entity.L3_Vulner; break;
                    case "4": Vulner_Amount = Entity.L4_Vulner; break;
                    case "5": Vulner_Amount = Entity.L5_Vulner; break;
                    default: Vulner_Amount = "0"; break;
                }

                switch (Level)
                {
                    case "1": NonVulner_Amount = Entity.L1_NonVulner; break;
                    case "2": NonVulner_Amount = Entity.L2_NonVulner; break;
                    case "3": NonVulner_Amount = Entity.L3_NonVulner; break;
                    case "4": NonVulner_Amount = Entity.L4_NonVulner; break;
                    case "5": NonVulner_Amount = Entity.L5_NonVulner; break;
                    default: NonVulner_Amount = "0"; break;
                }

                if (!Vulner_SW)
                    Vulner_Amount = NonVulner_Amount;
            }
        }

        #endregion

        #region Properties

        public string Award_Type { get; set; }
        public string Vulner_Amount { get; set; }
        public string NonVulner_Amount { get; set; }

        #endregion
    }

    public class MOREntity
    {
        #region Constructors

        public MOREntity()
        {
            Rec_Type =
            Date =
            Ter =
            Price =
            Price_Kerosene =
            Lstc_Date =
            Lstc_Operator = string.Empty;
        }

        public MOREntity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type =
                Date = 
                Ter = 
                Price = 
                Price_Kerosene = 
                Lstc_Date =
                Lstc_Operator = null;
            }
        }

        public MOREntity(MOREntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                Date = Entity.Date;
                Ter = Entity.Ter;
                Price = Entity.Price;
                Price_Kerosene = Entity.Price_Kerosene;
                Lstc_Date = Entity.Lstc_Date;
                Lstc_Operator = Entity.Lstc_Operator;
            }
        }

        public MOREntity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                Date = row["MOR_DATE"].ToString();
                Ter = row["MOR_TER"].ToString();
                Price = row["MOR_PRICE"].ToString();
                Price_Kerosene = row["MOR_PRICE_KEROSENE"].ToString();
                Lstc_Date = row["MOR_DATE_LSTC"].ToString();
                Lstc_Operator = row["MOR_LSTC_OPERATOR"].ToString();
            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Date { get; set; }
        public string Ter { get; set; }
        public string Price { get; set; }
        public string Price_Kerosene { get; set; }
        public string Lstc_Date { get; set; }
        public string Lstc_Operator { get; set; }

        #endregion
    }

    public class TMSHISTEntity
    {
        #region Constructors

        public TMSHISTEntity()
        {
            Rec_Type =
            Agency =
            Dept =
            Prog =
            Year =
            App_No =
            Hist_Seq =

            Hist_Type =
            Hist_Operation =
            LSTC_Date =
            LSTC_Operator = string.Empty;
        }

        public TMSHISTEntity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type = 
                Agency = 
                Dept = 
                Prog = 
                Year = 
                App_No = 
                Hist_Seq = 

                Hist_Type = 
                Hist_Operation  =
                LSTC_Date = 
                LSTC_Operator = null;
            }
        }

        public TMSHISTEntity(TMSHISTEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                Agency = Entity.Agency;
                Dept = Entity.Dept;
                Prog = Entity.Prog;
                Year = Entity.Year;
                App_No = Entity.App_No;
                Hist_Seq = Entity.Hist_Seq;

                Hist_Type = Entity.Hist_Type;
                Hist_Operation = Entity.Hist_Operation;
                LSTC_Date = Entity.LSTC_Date;
                LSTC_Operator = Entity.LSTC_Operator;
            }
        }

        public TMSHISTEntity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                Agency = row["TMSHIST_AGENCY"].ToString();
                Dept = row["TMSHIST_DEPT"].ToString();
                Prog = row["TMSHIST_PROGRAM"].ToString();
                Year = row["TMSHIST_YEAR"].ToString();
                App_No = row["TMSHIST_APP_NO"].ToString();
                Hist_Seq = row["TMSHIST_SEQ"].ToString();

                Hist_Type = row["TMSHIST_TYPE"].ToString();
                Hist_Operation = row["TMSHIST_OPERATION"].ToString();
                LSTC_Date = row["TMSHIST_DATE_LSTC"].ToString();
                LSTC_Operator = row["TMSHIST_LSTC_OPERATOR"].ToString();
                Changes = row["TMSHIST_CHANGES"].ToString();
            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string Year { get; set; }
        public string App_No { get; set; }
        public string Hist_Seq { get; set; }
        public string Hist_Type { get; set; }
        public string Hist_Operation { get; set; }
        public string LSTC_Date { get; set; }
        public string LSTC_Operator { get; set; }
        public string Changes { get; set; }

        #endregion
    }


    public class TrigMast_Entity
    {
        #region Constructors

        public TrigMast_Entity()
        {
            Rec_Type =
            Trig_ID =
            Trig_Crit =
            Trig_Name =
            Assoc_Cnt =
                Add_Operator =
                Add_Date =
                Lstc_Operator =
                Lstc_Date = string.Empty;
        }

        public TrigMast_Entity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type =
                Trig_ID =
                Trig_Crit =
                Trig_Name =
                Assoc_Cnt =
                Add_Operator =
                Add_Date =
                Lstc_Operator =
                Lstc_Date = null;
                
            }
        }

        public TrigMast_Entity(TrigMast_Entity Entity)
        {
            Rec_Type = Entity.Rec_Type;
            Trig_ID = Entity.Trig_ID;
            Trig_Crit = Entity.Trig_Crit;
            Trig_Name = Entity.Trig_Name;
            Assoc_Cnt = Entity.Assoc_Cnt;
            Add_Operator = Entity.Add_Operator;
            Add_Date = Entity.Add_Date;
            Lstc_Operator = Entity.Lstc_Operator;
            Lstc_Date = Entity.Lstc_Date;

        }

        public TrigMast_Entity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                Trig_ID = row["TRG_ID"].ToString();
                Trig_Crit = row["TRG_Criteria"].ToString();
                Trig_Name = row["TRG_Name"].ToString();
                Assoc_Cnt = row["Assoc_Cnt"].ToString();
                Add_Operator = row["TRG_ADD_OPERATOR"].ToString();
                Add_Date = row["TRG_DATE_ADD"].ToString();
                Lstc_Operator = row["TRG_LSTC_OPERATOR"].ToString();
                Lstc_Date = row["TRG_DATE_LSTC"].ToString();

            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Trig_ID { get; set; }
        public string Trig_Crit { get; set; }
        public string Trig_Name { get; set; }
        public string Assoc_Cnt { get; set; }

        public string Add_Operator { get; set; }
        public string Add_Date { get; set; }
        public string Lstc_Operator { get; set; }
        public string Lstc_Date { get; set; }

        #endregion
    }


    public class TrigAssnMst_Entity
    {
        #region Constructors

        public TrigAssnMst_Entity()
        {
            Rec_Type =
            Trig_ID =
            Trig_Seq =
            Trig_SP =
            Trig_SP_name =
            Trig_Assoc_Name =
            TRAGSM_DATE =
            TRAGSM_PROGRAM=
            Trig_Assoc_CAMS_Cnt =
                Add_Operator =
                Add_Date =
                Lstc_Operator =
                Lstc_Date = string.Empty;
        }

        public TrigAssnMst_Entity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type =
                Trig_ID =
                Trig_Seq =
                Trig_SP =
                Trig_SP_name =
                Trig_Assoc_Name =
                TRAGSM_DATE =
                TRAGSM_PROGRAM=
                Trig_Assoc_CAMS_Cnt =
                Add_Operator =
                Add_Date =
                Lstc_Operator =
                Lstc_Date = null;

            }
        }

        public TrigAssnMst_Entity(TrigAssnMst_Entity Entity)
        {
            Rec_Type = Entity.Rec_Type;
            Trig_ID = Entity.Trig_ID;
            Trig_Seq = Entity.Trig_Seq;
            Trig_SP = Entity.Trig_SP;
            Trig_SP_name = Entity.Trig_SP_name;
            Trig_Assoc_Name = Entity.Trig_Assoc_Name;
            TRAGSM_DATE = Entity.TRAGSM_DATE;
            TRAGSM_PROGRAM = Entity.TRAGSM_PROGRAM;
            Trig_Assoc_CAMS_Cnt = Entity.Trig_Assoc_CAMS_Cnt;

            Add_Operator = Entity.Add_Operator;
            Add_Date = Entity.Add_Date;
            Lstc_Operator = Entity.Lstc_Operator;
            Lstc_Date = Entity.Lstc_Date;

        }

        public TrigAssnMst_Entity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                Trig_ID = row["TRGASM_ID"].ToString();
                Trig_Seq = row["TRGASM_SEQ"].ToString();
                Trig_SP = row["TRGASM_SERPLAN"].ToString();
                Trig_SP_name = row["SP_DESC"].ToString();
                Trig_Assoc_Name = row["TRAGSM_Name"].ToString();
                TRAGSM_DATE = row["TRAGSM_DATE"].ToString();
                TRAGSM_PROGRAM = row["TRAGSM_PROGRAM"].ToString();
                Trig_Assoc_CAMS_Cnt = row["Assoc_CAMS_Cnt"].ToString();

                Add_Operator = row["TRGASM_ADD_OPERATOR"].ToString();
                Add_Date = row["TRGASM_DATE_ADD"].ToString();
                Lstc_Operator = row["TRGASM_LSTC_OPERATOR"].ToString();
                Lstc_Date = row["TRGASM_DATE_LSTC"].ToString();

            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Trig_ID { get; set; }
        public string Trig_Seq { get; set; }
        public string Trig_SP { get; set; }
        public string Trig_SP_name { get; set; }
        public string Trig_Assoc_Name { get; set; }
        public string TRAGSM_DATE { get; set; }
        public string TRAGSM_PROGRAM { get; set; }
        public string Trig_Assoc_CAMS_Cnt { get; set; }
        public string Add_Operator { get; set; }
        public string Add_Date { get; set; }
        public string Lstc_Operator { get; set; }
        public string Lstc_Date { get; set; }
        


        #endregion
    }

    public class TrigAssns_Entity
    {
        #region Constructors

        public TrigAssns_Entity()
        {
            Rec_Type =
            Trig_ID =
            Trig_Seq =
            Trig_SP =
                            Trig_Branch =
                Trig_org_Grp =
                Trig_org_Grp =
                Trig_CAMS_Type =
                Trig_CAMS_Code =
                Trig_Add_Operator =
                Trig_Add_Date =
                Trig_Lstc_Operator =
                Trig_Lstc_Date =
                CAMS_DESC =
                string.Empty;
        }

        public TrigAssns_Entity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type =
                Trig_ID =
                Trig_Seq =
                Trig_SP =
                Trig_Branch =
                Trig_org_Grp = 
                Trig_org_Grp =
                Trig_CAMS_Type =
                Trig_CAMS_Code =
                Trig_Add_Operator = 
                Trig_Add_Date = 
                Trig_Lstc_Operator = 
                Trig_Lstc_Date =
                CAMS_DESC = 
                null;

            }
        }

        public TrigAssns_Entity(TrigAssns_Entity Entity)
        {
            Rec_Type = Entity.Rec_Type;
            Trig_ID = Entity.Trig_ID;
            Trig_Seq = Entity.Trig_Seq;
            Trig_SP = Entity.Trig_SP;

            Trig_Branch = Entity.Trig_Branch;
            Trig_org_Grp = Entity.Trig_org_Grp;
            Trig_CAMS_Type = Entity.Trig_CAMS_Type;
            Trig_CAMS_Code = Entity.Trig_CAMS_Code;
            Trig_Add_Operator = Entity.Trig_Add_Operator;
            Trig_Add_Date = Entity.Trig_Add_Date;
            Trig_Lstc_Operator = Entity.Trig_Lstc_Operator;
            Trig_Lstc_Date = Entity.Trig_Lstc_Date;
            CAMS_DESC = Entity.CAMS_DESC;
        }

        public TrigAssns_Entity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                Trig_ID = row["TRGASM_ID"].ToString();
                Trig_Seq = row["TRGASM_SEQ"].ToString();
                Trig_SP = row["TRGAS_SERPLAN"].ToString();
                Trig_Branch = row["TRGAS_BRANCH"].ToString();
                Trig_org_Grp = row["TRGAS_ORIG_GRP"].ToString();
                Trig_CAMS_Type = row["TRGAS_CAMS"].ToString();
                Trig_CAMS_Code = row["TRGAS_CAMS_CODE"].ToString();
                Trig_Add_Operator = row["TRGAS_ADD_OPERATOR"].ToString();
                Trig_Add_Date = row["TRGAS_DATE_ADD"].ToString();
                Trig_Lstc_Operator = row["TRGAS_LSTC_OPERATOR"].ToString();
                Trig_Lstc_Date = row["TRGAS_DATE_LSTC"].ToString();
                CAMS_DESC = row["CAMS_DESC"].ToString(); 

            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Trig_ID { get; set; }
        public string Trig_Seq { get; set; }
        public string Trig_SP { get; set; }
        public string Trig_Branch { get; set; }
        public string Trig_org_Grp { get; set; }
        public string Trig_CAMS_Type { get; set; }
        public string Trig_CAMS_Code { get; set; }
        public string Trig_Add_Operator { get; set; }
        public string Trig_Add_Date { get; set; }
        public string Trig_Lstc_Operator { get; set; }
        public string Trig_Lstc_Date { get; set; }
        public string CAMS_DESC { get; set; }

        

        #endregion
    }

    public class TrigHist_Entity
    {
        #region Constructors

        public TrigHist_Entity()
        {
            Rec_Type =
            Trig_ID =
            Trig_Seq =
                Trig_Old_Crit =
                Trig_New_Crit =
                Trig_Add_Operator =
                Trig_Add_Date =
                string.Empty;
        }

        public TrigHist_Entity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type =
                Trig_ID = 
                Trig_Seq =
                Trig_Old_Crit = 
                Trig_New_Crit = 
                Trig_Add_Operator = 
                Trig_Add_Date = 
                null;

            }
        }

        public TrigHist_Entity(TrigHist_Entity Entity)
        {
            Rec_Type = Entity.Rec_Type;
            Trig_ID = Entity.Trig_ID;
            Trig_Seq = Entity.Trig_Seq;
            Trig_Old_Crit = Entity.Trig_Old_Crit;
            Trig_New_Crit = Entity.Trig_New_Crit;
            Trig_Add_Operator = Entity.Trig_Add_Operator;
            Trig_Add_Date = Entity.Trig_Add_Date;
        }

        public TrigHist_Entity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                Trig_ID = row["TrigHist_ID"].ToString();
                Trig_Seq = row["TrigHist_seq"].ToString();
                Trig_Old_Crit = row["TrigHist_Old_Criteria"].ToString();
                Trig_New_Crit = row["TrigHist_New_Criteria"].ToString();
                Trig_Add_Operator = row["TrigHist_LSTC_OPERATOR"].ToString();
                Trig_Add_Date = row["TrigHist_LSTC_DATE"].ToString();

            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Trig_ID { get; set; }
        public string Trig_Seq { get; set; }
        public string Trig_Old_Crit { get; set; }
        public string Trig_New_Crit { get; set; }
        public string Trig_Add_Operator { get; set; }
        public string Trig_Add_Date { get; set; }

        #endregion
    }

    public class TrigAssnMstHist_Entity
    {
        #region Constructors

        public TrigAssnMstHist_Entity()
        {
            Rec_Type =
            Trig_ID =
            Trig_Seq =
            Trig_Hist_Seq=
                Assoc_Old_Crit =
                Assoc_New_Crit =
                Assoc_Add_Operator =
                Assoc_Add_Date =
                string.Empty;
        }

        public TrigAssnMstHist_Entity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type =
                Trig_ID =
                Trig_Seq =
                Trig_Hist_Seq=
                Assoc_Old_Crit =
                Assoc_New_Crit =
                Assoc_Add_Operator =
                Assoc_Add_Date =
                null;

            }
        }

        public TrigAssnMstHist_Entity(TrigAssnMstHist_Entity Entity)
        {
            Rec_Type = Entity.Rec_Type;
            Trig_ID = Entity.Trig_ID;
            Trig_Seq = Entity.Trig_Seq;
            Trig_Hist_Seq = Entity.Trig_Hist_Seq;
            Assoc_Old_Crit = Entity.Assoc_Old_Crit;
            Assoc_New_Crit = Entity.Assoc_New_Crit;
            Assoc_Add_Operator = Entity.Assoc_Add_Operator;
            Assoc_Add_Date = Entity.Assoc_Add_Date;
        }

        public TrigAssnMstHist_Entity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                Trig_ID = row["AsmsHist_ID"].ToString();
                Trig_Seq = row["AsmsHist_Seq"].ToString();
                Trig_Hist_Seq = row["AsmsHist_HistSeq"].ToString();
                Assoc_Old_Crit = row["AsmsHist_Old_Crit"].ToString();
                Assoc_New_Crit = row["AsmsHist_New_Crit"].ToString();
                Assoc_Add_Operator = row["AsmsHist_ADD_OPERATOR"].ToString();
                Assoc_Add_Date = row["AsmsHist_ADD_DATE"].ToString();
            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Trig_ID { get; set; }
        public string Trig_Seq { get; set; }
        public string Trig_Hist_Seq { get; set; }
        public string Assoc_Old_Crit { get; set; }
        public string Assoc_New_Crit { get; set; }
        public string Assoc_Add_Operator { get; set; }
        public string Assoc_Add_Date { get; set; }

        #endregion
    }

    public class TrigAssnsHist
    {
        #region Constructors

        public TrigAssnsHist()
        {
            Rec_Type =
            Trig_ID =
            Trig_Seq =
            Trig_Hist_Seq =
                CAMS_Old_Crit =
                CAMS_New_Crit =
                CAMS_Add_Operator =
                CAMS_Add_Date =
                string.Empty;
        }

        public TrigAssnsHist(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type =
                Trig_ID =
                Trig_Seq =
                Trig_Hist_Seq =
                CAMS_Old_Crit =
                CAMS_New_Crit =
                CAMS_Add_Operator =
                CAMS_Add_Date =
                null;

            }
        }

        public TrigAssnsHist(TrigAssnsHist Entity)
        {
            Rec_Type = Entity.Rec_Type;
            Trig_ID = Entity.Trig_ID;
            Trig_Seq = Entity.Trig_Seq;
            Trig_Hist_Seq = Entity.Trig_Hist_Seq;
            CAMS_Old_Crit = Entity.CAMS_Old_Crit;
            CAMS_New_Crit = Entity.CAMS_New_Crit;
            CAMS_Add_Operator = Entity.CAMS_Add_Operator;
            CAMS_Add_Date = Entity.CAMS_Add_Date;
        }

        public TrigAssnsHist(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                Trig_ID = row["AssCAMSHist_ID"].ToString();
                Trig_Seq = row["AssCAMSHist_Seq"].ToString();
                Trig_Hist_Seq = row["AssCAMSHist_HistSeq"].ToString();
                CAMS_Old_Crit = row["AssCAMSHist_Old_Crit"].ToString();
                CAMS_New_Crit = row["AssCAMSHist_New_Crit"].ToString();
                CAMS_Add_Operator = row["AssCAMSHist_ADD_OPERATOR"].ToString();
                CAMS_Add_Date = row["AssCAMSHist_ADD_DATE"].ToString();

            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Trig_ID { get; set; }
        public string Trig_Seq { get; set; }
        public string Trig_Hist_Seq { get; set; }
        public string CAMS_Old_Crit { get; set; }
        public string CAMS_New_Crit { get; set; }
        public string CAMS_Add_Operator { get; set; }
        public string CAMS_Add_Date { get; set; }

        #endregion
    }

    public class CTTRIGQUESTSEntity
    {
        #region Constructors

        public CTTRIGQUESTSEntity()
        {
            TrigQuesCode = string.Empty;
            TrigQuesDesc = string.Empty;
            TrigRespType = string.Empty;
            TrigAgyCode = string.Empty;
            TrigFileName = string.Empty;
            TrigFldName = string.Empty;
            TrigActive = string.Empty;
            TrigAccess = string.Empty;

        }
        // Sudheer
        //public CTTRIGQUESTSEntity(DataRow row)
        //{
        //    TrigQuesCode = string.Empty;
        //    TrigQuesDesc = string.Empty;
        //    TrigRespType = string.Empty;
        //    TrigAgyCode = string.Empty;
        //    TrigFileName = string.Empty;
        //    TrigFldName = string.Empty;

        //}

        public CTTRIGQUESTSEntity(DataRow row)
        {
            if (row != null)
            {
                TrigQuesCode = row["CTTRIGQUES_CODE"].ToString().Trim();
                TrigQuesDesc = row["CTTRIGQUES_DESC"].ToString().Trim();
                TrigRespType = row["CTTRIGQUES_RESP_TYPE"].ToString().Trim();
                TrigAgyCode = row["CTTRIGQUES_AGY_CODE"].ToString().Trim();
                TrigFileName = row["CTTRIGQUES_FILENAME"].ToString().Trim();
                TrigFldName = row["CTTRIGQUES_FLDNAME"].ToString().Trim();
                TrigActive = row["CTTRIGQUES_ACTIVE"].ToString().Trim();
                TrigAccess = row["CTTRIGQUES_ACCESS"].ToString().Trim();
            }

        }



        #endregion

        #region Properties

        public string TrigQuesCode { get; set; }
        public string TrigQuesDesc { get; set; }
        public string TrigRespType { get; set; }
        public string TrigAgyCode { get; set; }
        public string TrigFileName { get; set; }
        public string TrigFldName { get; set; }
        public string TrigFliedType { get; set; }
        public string TrigActive { get; set; }
        public string TrigAccess { get; set; }

        #endregion
    }

    public class CTTRIGCRITEntity
    {
        #region Constructors

        public CTTRIGCRITEntity()
        {
            TRIGCRITQuesCode = string.Empty;
            TRIGCRITGroupCode = string.Empty;
            TRIGCRITGroupDesc = string.Empty;
            TRIGCRITResponseType = string.Empty;
            TRIGCRITConjunction = string.Empty;
            TRIGCRITNumEqual = string.Empty;
            TRIGCRITNumLthan = string.Empty;
            TRIGCRITNumGthan = string.Empty;
            TRIGCRITDateEqual = string.Empty;
            TRIGCRITDateLthan = string.Empty;
            TRIGCRITDateGthan = string.Empty;
            TRIGCRITDDTextResp = string.Empty;
            TRIGCRITMemAccess = string.Empty;
            TRIGCRITValidated = string.Empty;
            Mode = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;
            //CaseEligHXml = string.Empty;
            TrigQuestionDesc = string.Empty;
            TrigAgyCode = string.Empty;
            TrigFileName = string.Empty;
            TrigQuesFldName = string.Empty;
            TrigActive = string.Empty;

            TrigQuesName = string.Empty;
            TrigResponsesNames = string.Empty;
        }

        public CTTRIGCRITEntity(DataRow row)
        {
            if (row != null)
            {

                TRIGCRITCode = row["CTTRIGCRIT_CODE"].ToString().Trim();
                TRIGCRITGroupCode = row["CTTRIGCRIT_GROUP_CODE"].ToString().Trim();
                TRIGCRITGroupSeq = row["CTTRIGCRIT_GROUP_SEQ"].ToString().Trim();
                TRIGCRITGroupDesc = row["CTTRIGCRIT_GROUP_DESC"].ToString().Trim();
                TRIGCRITQuesCode = row["CTTRIGCRIT_QUES_CODE"].ToString().Trim();
                TRIGCRITResponseType = row["CTTRIGCRIT_RESP_TYPE"].ToString().Trim();
                TRIGCRITConjunction = row["CTTRIGCRIT_CONJN"].ToString().Trim();
                TRIGCRITNumEqual = row["CTTRIGCRIT_NUM_EQUAL"].ToString().Trim();
                TRIGCRITNumGthan = row["CTTRIGCRIT_NUM_GTHAN"].ToString().Trim();
                TRIGCRITNumLthan = row["CTTRIGCRIT_NUM_LTHAN"].ToString().Trim();
                TRIGCRITDateEqual = row["CTTRIGCRIT_DATE_EQUAL"].ToString().Trim();
                TRIGCRITDateGthan = row["CTTRIGCRIT_DATE_GTHAN"].ToString().Trim();
                TRIGCRITDateLthan = row["CTTRIGCRIT_DATE_LTHAN"].ToString().Trim();
                TRIGCRITDDTextResp = row["CTTRIGCRIT_DD_TEXT_RESP"].ToString().Trim();
                DateAdd = row["CTTRIGCRIT_DATE_ADD"].ToString().Trim();
                AddOperator = row["CTTRIGCRIT_ADD_OPERATOR"].ToString().Trim();
                DateLstc = row["CTTRIGCRIT_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["CTTRIGCRIT_LSTC_OPERATOR"].ToString().Trim();
                TRIGCRITMemAccess = row["CTTRIGCRIT_MEM_ACCESS"].ToString().Trim();
                TRIGCRITValidated = row["CTTRIGCRIT_VALIDATED"].ToString().Trim();
                //TrigAgyCode = row["CTTRIGQUES_AGY_CODE"].ToString().Trim();
                //TrigFileName = row["CTTRIGQUES_FILENAME"].ToString().Trim();
                //TrigQuesFldName = row["CTTRIGQUES_FLDNAME"].ToString().Trim();
                //TrigActive = row["CTTRIGQUES_ACTIVE"].ToString().Trim();
                TrigAssnCnt = row["Assoc_Cnt"].ToString().Trim();
            }

        }
        public CTTRIGCRITEntity(DataRow row, string strTable)
        {
            if (row != null)
            {
                if (strTable == "Hierarchy")
                {
                    //EligAgency = row["CASEELIG_AGENCY"].ToString().Trim();
                    //EligDept = row["CASEELIG_DEPT"].ToString().Trim();
                    //EligProgram = row["CASEELIG_PROGRAM"].ToString().Trim();
                }
                else
                {
                    TRIGCRITCode = row["CTTRIGCRIT_CODE"].ToString().Trim();
                    TRIGCRITGroupCode = row["CTTRIGCRIT_GROUP_CODE"].ToString().Trim();
                    TRIGCRITGroupSeq = row["CTTRIGCRIT_GROUP_SEQ"].ToString().Trim();
                    TRIGCRITGroupDesc = row["CTTRIGCRIT_GROUP_DESC"].ToString().Trim();
                    TRIGCRITQuesCode = row["CTTRIGCRIT_QUES_CODE"].ToString().Trim();
                    TRIGCRITResponseType = row["CTTRIGCRIT_RESP_TYPE"].ToString().Trim();
                    TRIGCRITConjunction = row["CTTRIGCRIT_CONJN"].ToString().Trim();
                    TRIGCRITNumEqual = row["CTTRIGCRIT_NUM_EQUAL"].ToString().Trim();
                    TRIGCRITNumGthan = row["CTTRIGCRIT_NUM_GTHAN"].ToString().Trim();
                    TRIGCRITNumLthan = row["CTTRIGCRIT_NUM_LTHAN"].ToString().Trim();
                    TRIGCRITDateEqual = row["CTTRIGCRIT_DATE_EQUAL"].ToString().Trim();
                    TRIGCRITDateGthan = row["CTTRIGCRIT_DATE_GTHAN"].ToString().Trim();
                    TRIGCRITDateLthan = row["CTTRIGCRIT_DATE_LTHAN"].ToString().Trim();
                    TRIGCRITDDTextResp = row["CTTRIGCRIT_DD_TEXT_RESP"].ToString().Trim();
                    DateAdd = row["CTTRIGCRIT_DATE_ADD"].ToString().Trim();
                    AddOperator = row["CTTRIGCRIT_ADD_OPERATOR"].ToString().Trim();
                    DateLstc = row["CTTRIGCRIT_DATE_LSTC"].ToString().Trim();
                    LstcOperator = row["CTTRIGCRIT_LSTC_OPERATOR"].ToString().Trim();
                    TRIGCRITMemAccess = row["CTTRIGCRIT_MEM_ACCESS"].ToString().Trim();
                    TRIGCRITValidated = row["CTTRIGCRIT_VALIDATED"].ToString().Trim();
                    if (strTable != "Group")
                    {
                        TrigQuestionDesc = row["CTTRIGQUES_DESC"].ToString().Trim();
                        TrigAgyCode = row["CTTRIGQUES_AGY_CODE"].ToString().Trim();
                        TrigFileName = row["CTTRIGQUES_FILENAME"].ToString().Trim();
                        TrigQuesFldName = row["CTTRIGQUES_FLDNAME"].ToString().Trim();
                        TrigActive = row["CTTRIGQUES_ACTIVE"].ToString().Trim();
                    }
                }
            }

        }

        #endregion

        #region Properties

        public string TRIGCRITCode { get; set; }
        public string TRIGCRITGroupCode { get; set; }
        public string TRIGCRITGroupSeq { get; set; }
        public string TRIGCRITGroupDesc { get; set; }
        public string TRIGCRITQuesCode { get; set; }
        public string TRIGCRITResponseType { get; set; }
        public string TRIGCRITConjunction { get; set; }
        public string TRIGCRITNumEqual { get; set; }
        public string TRIGCRITNumLthan { get; set; }
        public string TRIGCRITNumGthan { get; set; }
        public string TRIGCRITDateEqual { get; set; }
        public string TRIGCRITDateLthan { get; set; }
        public string TRIGCRITDateGthan { get; set; }
        public string TRIGCRITDDTextResp { get; set; }
        public string TRIGCRITMemAccess { get; set; }
        public string TRIGCRITValidated { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string Mode { get; set; }
        public string CaseTRIGCRITHXml { get; set; }
        public string TrigQuestionDesc { get; set; }
        public string TrigAgyCode { get; set; }
        public string TrigFileName { get; set; }
        public string TrigQuesFldName { get; set; }
        public string TrigActive { get; set; }
        public string TrigAssnCnt { get; set; }
        

        public string TrigQuesName { get; set; }
        public string TrigResponsesNames { get; set; }

        public string Type { get; set; }
        public string Msg { get; set; }
        #endregion
    }

    public class EAPFPEntity
    {
        #region Constructors

        public EAPFPEntity()
        {
            EAPFP_AGENCY = string.Empty;
            EAPFP_DEPT = string.Empty;
            EAPFP_PROGRAM = string.Empty;
            EAPFP_YEAR = string.Empty;
            EAPFP_APP_NO = string.Empty;
            EAPFP_KEY = string.Empty;
            EAPFP_DESCRIPTION = string.Empty;
            EAPFP_CLAIMDATE = string.Empty;
            EAPFP_CLAIMAMOUNT = string.Empty;
            EAPFP_PersonKey = string.Empty;

        }
        // Sudheer
        //public CTTRIGQUESTSEntity(DataRow row)
        //{
        //    TrigQuesCode = string.Empty;
        //    TrigQuesDesc = string.Empty;
        //    TrigRespType = string.Empty;
        //    TrigAgyCode = string.Empty;
        //    TrigFileName = string.Empty;
        //    TrigFldName = string.Empty;

        //}

        public EAPFPEntity(DataRow row)
        {
            if (row != null)
            {
                EAPFP_AGENCY = row["EAPFP_AGENCY"].ToString().Trim();
                EAPFP_DEPT = row["EAPFP_DEPT"].ToString().Trim();
                EAPFP_PROGRAM = row["EAPFP_PROGRAM"].ToString().Trim();
                EAPFP_YEAR = row["EAPFP_YEAR"].ToString().Trim();
                EAPFP_APP_NO = row["EAPFP_APP_NO"].ToString().Trim();
                EAPFP_KEY = row["EAPFP_KEY"].ToString().Trim();
                EAPFP_DESCRIPTION = row["EAPFP_DESCRIPTION"].ToString().Trim();
                EAPFP_CLAIMDATE = row["EAPFP_CLAIMDATE"].ToString().Trim();
                EAPFP_CLAIMAMOUNT = row["EAPFP_CLAIMAMOUNT"].ToString().Trim();
                EAPFP_PersonKey = row["EAPFP_PersonKey"].ToString().Trim();
            }

        }



        #endregion

        #region Properties

        public string EAPFP_AGENCY { get; set; }
        public string EAPFP_DEPT { get; set; }
        public string EAPFP_PROGRAM { get; set; }
        public string EAPFP_YEAR { get; set; }
        public string EAPFP_APP_NO { get; set; }
        public string EAPFP_KEY { get; set; }
        public string EAPFP_DESCRIPTION { get; set; }
        public string EAPFP_CLAIMDATE { get; set; }
        public string EAPFP_CLAIMAMOUNT { get; set; }
        public string EAPFP_PersonKey { get; set; }

        #endregion
    }


    public class STATUSCKEntity
    {
        #region Constructors

        public STATUSCKEntity()
        {
            STATUS_AGENCY = string.Empty;
            STATUS_DEPT = string.Empty;
            STATUS_PROGRAM = string.Empty;
            STATUS_YEAR = string.Empty;
            STATUS_APP_NO = string.Empty;
            STATUS_PROGRAMCK1 = string.Empty;
            STATUS_STATUS1 = string.Empty;
            STATUS_PROGRAMCK2 = string.Empty;
            STATUS_STATUS2 = string.Empty;
            STATUS_PROGRAMCK3 = string.Empty;
            STATUS_STATUS3 = string.Empty;
            STATUS_PROGRAMCK4 = string.Empty;
            STATUS_STATUS4 = string.Empty;
            STATUS_PERSONKEY = string.Empty;

        }
        
        public STATUSCKEntity(DataRow row)
        {
            if (row != null)
            {
                STATUS_AGENCY = row["STATUS_AGENCY"].ToString().Trim();
                STATUS_DEPT = row["STATUS_DEPT"].ToString().Trim();
                STATUS_PROGRAM = row["STATUS_PROGRAM"].ToString().Trim();
                STATUS_YEAR = row["STATUS_YEAR"].ToString().Trim();
                STATUS_APP_NO = row["STATUS_APP_NO"].ToString().Trim();
                STATUS_PROGRAMCK1 = row["STATUS_PROGRAMCK1"].ToString().Trim();
                STATUS_STATUS1 = row["STATUS_STATUS1"].ToString().Trim();
                STATUS_PROGRAMCK2 = row["STATUS_PROGRAMCK2"].ToString().Trim();
                STATUS_STATUS2 = row["STATUS_STATUS2"].ToString().Trim();
                STATUS_PROGRAMCK3 = row["STATUS_PROGRAMCK3"].ToString().Trim();
                STATUS_STATUS3 = row["STATUS_STATUS3"].ToString().Trim();
                STATUS_PROGRAMCK4 = row["STATUS_PROGRAMCK4"].ToString().Trim();
                STATUS_STATUS4 = row["STATUS_STATUS4"].ToString().Trim();
                STATUS_PERSONKEY = row["STATUS_PERSONKEY"].ToString().Trim();
               
            }

        }



        #endregion

        #region Properties

        public string STATUS_AGENCY { get; set; }
        public string STATUS_DEPT { get; set; }
        public string STATUS_PROGRAM { get; set; }
        public string STATUS_YEAR { get; set; }
        public string STATUS_APP_NO { get; set; }
        public string STATUS_PROGRAMCK1 { get; set; }
        public string STATUS_STATUS1 { get; set; }
        public string STATUS_PROGRAMCK2 { get; set; }
        public string STATUS_STATUS2 { get; set; }
        public string STATUS_PROGRAMCK3 { get; set; }
        public string STATUS_STATUS3 { get; set; }
        public string STATUS_PROGRAMCK4 { get; set; }
        public string STATUS_STATUS4 { get; set; }
        public string STATUS_PERSONKEY { get; set; }

        #endregion
    }

    public class CATGELIGEntity
    {
        #region Constructors

        public CATGELIGEntity()
        {
            Agency = string.Empty;
            Dept = string.Empty;
            Prog = string.Empty;
            Year = string.Empty;
            App = string.Empty;
            FamSeq = string.Empty;
            Seq = string.Empty;
            Type = string.Empty;
            Heat_Cost = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;
            CaseWorker = string.Empty;
            //Remaining = string.Empty;
            SNAP = string.Empty;
            SNAP_Worker = string.Empty;
            TFA = string.Empty;
            TFA_Worker = string.Empty;
            RCash = string.Empty;
            RCash_worker = string.Empty;
            SS = string.Empty;
            SS_Worker = string.Empty;
            SSI = string.Empty;
            SSI_Worker = string.Empty;

            VerMethod = string.Empty;
            HowVerified = string.Empty;
        }

        public CATGELIGEntity(DataRow row, string strTable)
        {
            if (row != null)
            {
                Agency = row["CATEG_AGENCY"].ToString();
                Dept = row["CATEG_DEPT"].ToString();
                Prog= row["CATEG_PROG"].ToString();
                Year = row["CATEG_YEAR"].ToString();
                App = row["CATEG_APP_NO"].ToString();
                FamSeq = row["CATEG_FAM_SEQ"].ToString();
                Seq = row["CATEG_SEQ"].ToString();
                Type = row["CATEG_TYPE"].ToString();
                Heat_Cost = row["CATEG_HEAT_COST"].ToString();
                CaseWorker=row["CATEG_VERIFIER"].ToString();
                DateAdd = row["CATEG_DATE_ADD"].ToString();
                AddOperator = row["CATEG_ADD_OPERATOR"].ToString();
                DateLstc = row["CATEG_DATE_LSTC"].ToString();
                LstcOperator = row["CATEG_LSTC_OPERATOR"].ToString();

                VerMethod = row["CATEG_VER_METHOD"].ToString();
                HowVerified = row["CATEG_HOW_VERIFIED"].ToString();
            }

        }

        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string Year { get; set; }
        public string App { get; set; }
        public string FamSeq { get; set; }
        public string Seq { get; set; }
        public string Type { get; set; }
        public string Heat_Cost { get; set; }
        public string CaseWorker { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }

        public string SNAP { get; set; }
        public string SNAP_Worker { get; set; }
        public string TFA { get; set; }
        public string TFA_Worker { get; set; }
        public string RCash { get; set; }
        public string RCash_worker { get; set; }
        public string SS { get; set; }
        public string SS_Worker { get; set; }

        public string SSI { get; set; }
        public string SSI_Worker { get; set; }

        public string VerMethod { get; set; }
        public string HowVerified { get; set; }
        public string Mode { get; set; }
        #endregion
    }

    public class TMSELIGEntity

    {
        #region Constructors

        public TMSELIGEntity()
        {
            TMSELIG_AGENCY = string.Empty;
            TMSELIG_DEPT = string.Empty;
            TMSELIG_PROG = string.Empty;
            TMSELIG_YEAR = string.Empty;
            TMSELIG_APP_NO = string.Empty;
            TMSELIG_FNAME = string.Empty;
            TMSELIG_MNAME = string.Empty;
            TMSELIG_LNAME = string.Empty;
            TMSELIG_DOB = string.Empty;
            TMSELIG_SSN = string.Empty;
            TMSELIG_TOA = string.Empty;
            TMSELIG_ADDRESS = string.Empty;
            TMSELIG_INC_CERT = string.Empty;
            TMSELIG_DEN_REASN = string.Empty;
            TMSELIG_MAT_ENRL = string.Empty;
            TMSELIG_CATELIG = string.Empty;
            TMSELIG_FAM_SEQ = string.Empty;
            TMSELIG_APP_MEM = string.Empty;
            TMSELIG_INC_TYPE = string.Empty;
            TMSELIG_FSTAMPS = string.Empty;
            TMSELIG_NCASH_BEN = string.Empty;
            TMSELIG_BEF_INCCERT = string.Empty;
            TMSELIG_BEF_AGEDIS = string.Empty;
            TMSELIG_BEF_LIQASSETS = string.Empty;
            TMSELIG_BEF_BENLVL = string.Empty;
            TMSELIG_BEF_AWARD = string.Empty;
            TMSELIG_AFT_INCCERT = string.Empty;
            TMSELIG_AFT_AGEDIS = string.Empty;
            TMSELIG_AFT_LIQASSETS = string.Empty;
            TMSELIG_AFT_BENLVL = string.Empty;
            TMSELIG_AFT_AWARD = string.Empty;
            TMSELIG_IS_FILL = string.Empty;
            TMSELIG_STATUS = string.Empty;
            TMSELIG_PROG_INCOME = string.Empty;
            TMSELIG_NO_INPROG = string.Empty;
            TMSELIG_INTAKE_DATE = string.Empty;
            TMSELIG_DISABLE = string.Empty;
            TMSELIG_HEAT_INC_RENT = string.Empty;
            TMSELIG_SOURCE = string.Empty;
            TMSELIG_HOUSING = string.Empty;

            Age = string.Empty;
            Vulnerable = string.Empty;
            TMSELIG_AFT_VULNERABLE = string.Empty;
            TMSELIG_IS_REJECT = string.Empty;

            TMSELIG_RELATION_DESC = string.Empty;
            TMSELIG_DISABLE_DESC = string.Empty;

            AFT_BEN_Level = string.Empty;

        }

        public TMSELIGEntity(DataRow row, string strTable)
        {
            if (row != null)
            {
                TMSELIG_AGENCY = row["TMSELIG_AGENCY"].ToString();
                TMSELIG_DEPT = row["TMSELIG_DEPT"].ToString();
                TMSELIG_PROG = row["TMSELIG_PROG"].ToString();
                TMSELIG_YEAR = row["TMSELIG_YEAR"].ToString();
                TMSELIG_APP_NO = row["TMSELIG_APP_NO"].ToString();
                TMSELIG_FNAME = row["TMSELIG_FNAME"].ToString();
                TMSELIG_MNAME = row["TMSELIG_MNAME"].ToString();
                TMSELIG_LNAME = row["TMSELIG_LNAME"].ToString();
                TMSELIG_DOB = row["TMSELIG_DOB"].ToString();
                TMSELIG_SSN = row["TMSELIG_SSN"].ToString();
                TMSELIG_TOA = row["TMSELIG_TOA"].ToString();
                TMSELIG_ADDRESS = row["TMSELIG_ADDRESS"].ToString();
                TMSELIG_INC_CERT = row["TMSELIG_INC_CERT"].ToString();
                TMSELIG_DEN_REASN = row["TMSELIG_DEN_REASN"].ToString();
                TMSELIG_MAT_ENRL = row["TMSELIG_MAT_ENRL"].ToString();
                TMSELIG_CATELIG = row["TMSELIG_CATELIG"].ToString();
                TMSELIG_FAM_SEQ = row["TMSELIG_FAM_SEQ"].ToString();
                TMSELIG_APP_MEM = row["TMSELIG_APP_MEM"].ToString();
                TMSELIG_INC_TYPE = row["TMSELIG_INC_TYPE"].ToString();
                TMSELIG_FSTAMPS = row["TMSELIG_FSTAMPS"].ToString();
                TMSELIG_NCASH_BEN = row["TMSELIG_NCASH_BEN"].ToString();
                TMSELIG_BEF_INCCERT = row["TMSELIG_BEF_INCCERT"].ToString();
                TMSELIG_BEF_AGEDIS = row["TMSELIG_BEF_AGEDIS"].ToString();
                TMSELIG_BEF_LIQASSETS = row["TMSELIG_BEF_LIQASSETS"].ToString();
                TMSELIG_BEF_BENLVL = row["TMSELIG_BEF_BENLVL"].ToString();
                TMSELIG_BEF_AWARD = row["TMSELIG_BEF_AWARD"].ToString();
                TMSELIG_AFT_INCCERT = row["TMSELIG_AFT_INCCERT"].ToString();
                TMSELIG_AFT_AGEDIS = row["TMSELIG_AFT_AGEDIS"].ToString();
                TMSELIG_AFT_LIQASSETS = row["TMSELIG_AFT_LIQASSETS"].ToString();
                TMSELIG_AFT_BENLVL = row["TMSELIG_AFT_BENLVL"].ToString();
                TMSELIG_AFT_AWARD = row["TMSELIG_AFT_AWARD"].ToString();
                TMSELIG_IS_FILL = row["TMSELIG_IS_FILL"].ToString();
                TMSELIG_STATUS = row["TMSELIG_STATUS"].ToString();
                TMSELIG_PROG_INCOME = row["TMSELIG_PROG_INCOME"].ToString();
                TMSELIG_NO_INPROG = row["TMSELIG_NO_INPROG"].ToString();
                TMSELIG_INTAKE_DATE = row["TMSELIG_INTAKE_DATE"].ToString();
                TMSELIG_DISABLE = row["TMSELIG_DISABLE"].ToString();

                TMSELIG_HEAT_INC_RENT = row["TMSELIG_HEAT_INC_RENT"].ToString();
                TMSELIG_SOURCE = row["TMSELIG_SOURCE"].ToString();
                TMSELIG_HOUSING = row["TMSELIG_HOUSING"].ToString();

                Age = row["TMSELIG_AGE"].ToString();
                Vulnerable = row["TMSELIG_VULNERABLE"].ToString();

                TMSELIG_AFT_VULNERABLE = row["TMSELIG_AFT_VULNERABLE"].ToString();
                TMSELIG_IS_REJECT = row["TMSELIG_IS_REJECT"].ToString();

                TMSELIG_RELATION_DESC = row["TMSELIG_RELATION_DESC"].ToString();
                TMSELIG_DISABLE_DESC = row["TMSELIG_DISABLE_DESC"].ToString();

            }

        }

        #endregion

        #region Properties

        public string TMSELIG_AGENCY { get; set; }
        public string TMSELIG_DEPT { get; set; }
        public string TMSELIG_PROG { get; set; }
        public string TMSELIG_YEAR { get; set; }
        public string TMSELIG_APP_NO { get; set; }
        public string TMSELIG_FNAME { get; set; }
        public string TMSELIG_MNAME { get; set; }
        public string TMSELIG_LNAME { get; set; }
        public string TMSELIG_DOB { get; set; }
        public string TMSELIG_SSN { get; set; }
        public string TMSELIG_TOA { get; set; }
        public string TMSELIG_ADDRESS { get; set; }
        public string TMSELIG_INC_CERT { get; set; }
        public string TMSELIG_DEN_REASN { get; set; }

        public string TMSELIG_MAT_ENRL { get; set; }
        public string TMSELIG_CATELIG { get; set; }
        public string TMSELIG_FAM_SEQ { get; set; }
        public string TMSELIG_APP_MEM { get; set; }
        public string TMSELIG_INC_TYPE { get; set; }
        public string TMSELIG_FSTAMPS { get; set; }
        public string TMSELIG_NCASH_BEN { get; set; }
        public string TMSELIG_BEF_INCCERT { get; set; }
        public string TMSELIG_BEF_AGEDIS { get; set; }
        public string TMSELIG_BEF_LIQASSETS { get; set; }
        public string TMSELIG_BEF_BENLVL { get; set; }
        public string TMSELIG_BEF_AWARD { get; set; }
        public string TMSELIG_AFT_INCCERT { get; set; }
        public string TMSELIG_AFT_AGEDIS { get; set; }
        public string TMSELIG_AFT_LIQASSETS { get; set; }
        public string TMSELIG_AFT_BENLVL { get; set; }
        public string TMSELIG_AFT_AWARD { get; set; }
        public string TMSELIG_IS_FILL { get; set; }
        public string TMSELIG_STATUS { get; set; }
        public string TMSELIG_PROG_INCOME { get; set; }
        public string TMSELIG_NO_INPROG { get; set; }
        public string TMSELIG_INTAKE_DATE { get; set; }
        public string TMSELIG_DISABLE { get; set; }

        public string TMSELIG_HEAT_INC_RENT { get; set; }
        public string TMSELIG_SOURCE { get; set; }
        public string TMSELIG_HOUSING { get; set; }
        public string Age { get; set; }
        public string Vulnerable { get; set; }
        public string TMSELIG_AFT_VULNERABLE { get; set; }
        public string TMSELIG_IS_REJECT { get; set; }
        public string TMSELIG_RELATION_DESC { get; set; }
        public string TMSELIG_DISABLE_DESC { get; set; }

        public string AFT_BEN_Level { get; set; }
        //public string SS { get; set; }
        //public string SS_Worker { get; set; }
        //public string SNAP { get; set; }
        //public string SNAP_Worker { get; set; }
        //public string TFA { get; set; }
        //public string TFA_Worker { get; set; }
        //public string RCash { get; set; }
        //public string RCash_worker { get; set; }
        //public string SS { get; set; }
        //public string SS_Worker { get; set; }


        public string Mode { get; set; }
        #endregion
    }

    public class LIHWAPEntity
    {
        #region Constructors

        public LIHWAPEntity()
        {
            Rec_Type = string.Empty;
            LPW_AGENCY = string.Empty;
            LPW_DEPT = string.Empty;
            LPW_PROG = string.Empty;
            LPW_YEAR = string.Empty;
            LPW_APP_NO = string.Empty;
            LPW_SEQ = string.Empty;
            LPW_BEN_DATE = string.Empty;
            LPW_BEN_WORKER = string.Empty;
            LPW_CERTIFIED_STATUS = string.Empty;
            LPW_REASON_DENIED = string.Empty;
            LPW_FAM_INCOME = string.Empty;
            LPW_FAM_NO_INHH = string.Empty;
            LPW_OMB = string.Empty;
            LPW_SMI = string.Empty;
            LPW_WAT_VENDOR = string.Empty;
            LPW_WAT_ACCOUNT_NO = string.Empty;
            LPW_WAT_BILLNAME_TYPE = string.Empty;
            LPW_WAT_BILL_LNAME = string.Empty;
            LPW_WAT_BILL_FNAME = string.Empty;
            LPW_WAT_INVDATE = string.Empty;
            LPW_WAT_FREQ_BILL = string.Empty;
            LPW_WAT_ARREARS = string.Empty;
            LPW_WAT_CURBILL_AMT = string.Empty;
            LPW_WAT_FEES = string.Empty;
            LPW_WAT_TOT = string.Empty;
            LPW_WAT_AWARD = string.Empty;
            LPW_WAT_PAID = string.Empty;
            LPW_WAT_AWAITING_DOL = string.Empty;
            LPW_WAT_BEN_TYPE = string.Empty;
            LPW_WAT_LEVEL = string.Empty;
            LPW_WAT_VER_STAT = string.Empty;
            LPW_WAT_VERIFIED_WORKER = string.Empty;
            LPW_WAT_VERIFIED_DATE = string.Empty;
            LPW_WAT_VERIFIED_DATE_LSTC = string.Empty;
            LPW_WAT_VERIFIED_LSTC_OPERATOR = string.Empty;
            LPW_SEW_VENDOR = string.Empty;
            LPW_SEW_ACCOUNT_NO = string.Empty;
            LPW_SEW_BILLNAME_TYPE = string.Empty;
            LPW_SEW_BILL_LNAME = string.Empty;
            LPW_SEW_BILL_FNAME = string.Empty;
            LPW_SEW_INVDATE = string.Empty;
            LPW_SEW_FREQ_BILL = string.Empty;
            LPW_SEW_ARREARS = string.Empty;
            LPW_SEW_CURBILL_AMT = string.Empty;
            LPW_SEW_FEES = string.Empty;
            LPW_SEW_TOT = string.Empty;
            LPW_SEW_AWARD = string.Empty;
            LPW_SEW_PAID = string.Empty;
            LPW_SEW_AWAITING_DOL = string.Empty;
            LPW_SEW_BEN_TYPE = string.Empty;
            LPW_SEW_LEVEL = string.Empty;
            LPW_SEW_VER_STAT = string.Empty;
            LPW_SEW_VERIFIED_WORKER = string.Empty;
            LPW_SEW_VERIFIED_DATE = string.Empty;
            LPW_SEW_VERIFIED_DATE_LSTC = string.Empty;
            LPW_SEW_VERIFIED_LSTC_OPERATOR = string.Empty;
            LPW_DATE_ADD = string.Empty;
            LPW_ADD_OPERATOR = string.Empty;
            LPW_DATE_LSTC = string.Empty;
            LPW_LSTC_OPERATOR = string.Empty;




        }

        public LIHWAPEntity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type =
                LPW_AGENCY =
            LPW_DEPT =
            LPW_PROG =
            LPW_YEAR =
            LPW_APP_NO =
            LPW_SEQ =
            LPW_BEN_DATE =
            LPW_BEN_WORKER =
            LPW_CERTIFIED_STATUS =
            LPW_REASON_DENIED =
            LPW_FAM_INCOME =
            LPW_FAM_NO_INHH =
            LPW_OMB =
            LPW_SMI =
            LPW_WAT_VENDOR =
            LPW_WAT_ACCOUNT_NO =
            LPW_WAT_BILLNAME_TYPE =
            LPW_WAT_BILL_LNAME =
            LPW_WAT_BILL_FNAME =
            LPW_WAT_INVDATE =
            LPW_WAT_FREQ_BILL =
            LPW_WAT_ARREARS =
            LPW_WAT_CURBILL_AMT =
            LPW_WAT_FEES =
            LPW_WAT_TOT =
            LPW_WAT_AWARD =
            LPW_WAT_PAID =
            LPW_WAT_AWAITING_DOL =
            LPW_WAT_BEN_TYPE =
            LPW_WAT_LEVEL =
            LPW_WAT_VER_STAT =
            LPW_WAT_VERIFIED_WORKER =
            LPW_WAT_VERIFIED_DATE =
            LPW_WAT_VERIFIED_DATE_LSTC =
            LPW_WAT_VERIFIED_LSTC_OPERATOR =
            LPW_SEW_VENDOR =
            LPW_SEW_ACCOUNT_NO =
            LPW_SEW_BILLNAME_TYPE =
            LPW_SEW_BILL_LNAME =
            LPW_SEW_BILL_FNAME =
            LPW_SEW_INVDATE =
            LPW_SEW_FREQ_BILL =
            LPW_SEW_ARREARS =
            LPW_SEW_CURBILL_AMT =
            LPW_SEW_FEES =
            LPW_SEW_TOT =
            LPW_SEW_AWARD =
            LPW_SEW_PAID =
            LPW_SEW_AWAITING_DOL =
            LPW_SEW_BEN_TYPE =
            LPW_SEW_LEVEL =
            LPW_SEW_VER_STAT =
            LPW_SEW_VERIFIED_WORKER =
            LPW_SEW_VERIFIED_DATE =
            LPW_SEW_VERIFIED_DATE_LSTC =
            LPW_SEW_VERIFIED_LSTC_OPERATOR =
            LPW_DATE_ADD =
            LPW_ADD_OPERATOR =
            LPW_DATE_LSTC =
            LPW_LSTC_OPERATOR =

                        null;



            }
        }



        public LIHWAPEntity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                LPW_AGENCY = row["LPW_AGENCY"].ToString();
                LPW_DEPT = row["LPW_DEPT"].ToString();
                LPW_PROG = row["LPW_PROG"].ToString();
                LPW_YEAR = row["LPW_YEAR"].ToString();
                LPW_APP_NO = row["LPW_APP_NO"].ToString();
                LPW_SEQ = row["LPW_SEQ"].ToString();
                LPW_BEN_DATE = row["LPW_BEN_DATE"].ToString();
                LPW_BEN_WORKER = row["LPW_BEN_WORKER"].ToString();
                LPW_CERTIFIED_STATUS = row["LPW_CERTIFIED_STATUS"].ToString();
                LPW_REASON_DENIED = row["LPW_REASON_DENIED"].ToString();
                LPW_FAM_INCOME = row["LPW_FAM_INCOME"].ToString();
                LPW_FAM_NO_INHH = row["LPW_FAM_NO_INHH"].ToString();
                LPW_OMB = row["LPW_OMB"].ToString();
                LPW_SMI = row["LPW_SMI"].ToString();
                LPW_WAT_VENDOR = row["LPW_WAT_VENDOR"].ToString();
                LPW_WAT_ACCOUNT_NO = row["LPW_WAT_ACCOUNT_NO"].ToString();
                LPW_WAT_BILLNAME_TYPE = row["LPW_WAT_BILLNAME_TYPE"].ToString();
                LPW_WAT_BILL_LNAME = row["LPW_WAT_BILL_LNAME"].ToString();
                LPW_WAT_BILL_FNAME = row["LPW_WAT_BILL_FNAME"].ToString();
                LPW_WAT_INVDATE = row["LPW_WAT_INVDATE"].ToString();
                LPW_WAT_FREQ_BILL = row["LPW_WAT_FREQ_BILL"].ToString();
                LPW_WAT_ARREARS = row["LPW_WAT_ARREARS"].ToString();
                LPW_WAT_CURBILL_AMT = row["LPW_WAT_CURBILL_AMT"].ToString();
                LPW_WAT_FEES = row["LPW_WAT_FEES"].ToString();
                LPW_WAT_TOT = row["LPW_WAT_TOT"].ToString();
                LPW_WAT_AWARD = row["LPW_WAT_AWARD"].ToString();
                LPW_WAT_PAID = row["LPW_WAT_PAID"].ToString();
                LPW_WAT_AWAITING_DOL = row["LPW_WAT_AWAITING_DOL"].ToString();
                LPW_WAT_BEN_TYPE = row["LPW_WAT_BEN_TYPE"].ToString();
                LPW_WAT_LEVEL = row["LPW_WAT_LEVEL"].ToString();
                LPW_WAT_VER_STAT = row["LPW_WAT_VER_STAT"].ToString();
                LPW_WAT_VERIFIED_WORKER = row["LPW_WAT_VERIFIED_WORKER"].ToString();
                LPW_WAT_VERIFIED_DATE = row["LPW_WAT_VERIFIED_DATE"].ToString();
                LPW_WAT_VERIFIED_DATE_LSTC = row["LPW_WAT_VERIFIED_DATE_LSTC"].ToString();
                LPW_WAT_VERIFIED_LSTC_OPERATOR = row["LPW_WAT_VERIFIED_LSTC_OPERATOR"].ToString();
                LPW_SEW_VENDOR = row["LPW_SEW_VENDOR"].ToString();
                LPW_SEW_ACCOUNT_NO = row["LPW_SEW_ACCOUNT_NO"].ToString();
                LPW_SEW_BILLNAME_TYPE = row["LPW_SEW_BILLNAME_TYPE"].ToString();
                LPW_SEW_BILL_LNAME = row["LPW_SEW_BILL_LNAME"].ToString();
                LPW_SEW_BILL_FNAME = row["LPW_SEW_BILL_FNAME"].ToString();
                LPW_SEW_INVDATE = row["LPW_SEW_INVDATE"].ToString();
                LPW_SEW_FREQ_BILL = row["LPW_SEW_FREQ_BILL"].ToString();
                LPW_SEW_ARREARS = row["LPW_SEW_ARREARS"].ToString();
                LPW_SEW_CURBILL_AMT = row["LPW_SEW_CURBILL_AMT"].ToString();
                LPW_SEW_FEES = row["LPW_SEW_FEES"].ToString();
                LPW_SEW_TOT = row["LPW_SEW_TOT"].ToString();
                LPW_SEW_AWARD = row["LPW_SEW_AWARD"].ToString();
                LPW_SEW_PAID = row["LPW_SEW_PAID"].ToString();
                LPW_SEW_AWAITING_DOL = row["LPW_SEW_AWAITING_DOL"].ToString();
                LPW_SEW_BEN_TYPE = row["LPW_SEW_BEN_TYPE"].ToString();
                LPW_SEW_LEVEL = row["LPW_SEW_LEVEL"].ToString();
                LPW_SEW_VER_STAT = row["LPW_SEW_VER_STAT"].ToString();
                LPW_SEW_VERIFIED_WORKER = row["LPW_SEW_VERIFIED_WORKER"].ToString();
                LPW_SEW_VERIFIED_DATE = row["LPW_SEW_VERIFIED_DATE"].ToString();
                LPW_SEW_VERIFIED_DATE_LSTC = row["LPW_SEW_VERIFIED_DATE_LSTC"].ToString();
                LPW_SEW_VERIFIED_LSTC_OPERATOR = row["LPW_SEW_VERIFIED_LSTC_OPERATOR"].ToString();
                LPW_DATE_ADD = row["LPW_DATE_ADD"].ToString();
                LPW_ADD_OPERATOR = row["LPW_ADD_OPERATOR"].ToString();
                LPW_DATE_LSTC = row["LPW_DATE_LSTC"].ToString();
                LPW_LSTC_OPERATOR = row["LPW_LSTC_OPERATOR"].ToString();

            }
        }


        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string LPW_AGENCY { get; set; }
        public string LPW_DEPT { get; set; }
        public string LPW_PROG { get; set; }
        public string LPW_YEAR { get; set; }
        public string LPW_APP_NO { get; set; }
        public string LPW_SEQ { get; set; }
        public string LPW_BEN_DATE { get; set; }
        public string LPW_BEN_WORKER { get; set; }
        public string LPW_CERTIFIED_STATUS { get; set; }
        public string LPW_REASON_DENIED { get; set; }
        public string LPW_FAM_INCOME { get; set; }
        public string LPW_FAM_NO_INHH { get; set; }
        public string LPW_OMB { get; set; }
        public string LPW_SMI { get; set; }
        public string LPW_WAT_VENDOR { get; set; }
        public string LPW_WAT_ACCOUNT_NO { get; set; }
        public string LPW_WAT_BILLNAME_TYPE { get; set; }
        public string LPW_WAT_BILL_LNAME { get; set; }
        public string LPW_WAT_BILL_FNAME { get; set; }
        public string LPW_WAT_INVDATE { get; set; }
        public string LPW_WAT_FREQ_BILL { get; set; }
        public string LPW_WAT_ARREARS { get; set; }
        public string LPW_WAT_CURBILL_AMT { get; set; }
        public string LPW_WAT_FEES { get; set; }
        public string LPW_WAT_TOT { get; set; }
        public string LPW_WAT_AWARD { get; set; }
        public string LPW_WAT_PAID { get; set; }
        public string LPW_WAT_AWAITING_DOL { get; set; }
        public string LPW_WAT_BEN_TYPE { get; set; }
        public string LPW_WAT_LEVEL { get; set; }
        public string LPW_WAT_VER_STAT { get; set; }
        public string LPW_WAT_VERIFIED_WORKER { get; set; }
        public string LPW_WAT_VERIFIED_DATE { get; set; }
        public string LPW_WAT_VERIFIED_DATE_LSTC { get; set; }
        public string LPW_WAT_VERIFIED_LSTC_OPERATOR { get; set; }
        public string LPW_SEW_VENDOR { get; set; }
        public string LPW_SEW_ACCOUNT_NO { get; set; }
        public string LPW_SEW_BILLNAME_TYPE { get; set; }
        public string LPW_SEW_BILL_LNAME { get; set; }
        public string LPW_SEW_BILL_FNAME { get; set; }
        public string LPW_SEW_INVDATE { get; set; }
        public string LPW_SEW_FREQ_BILL { get; set; }
        public string LPW_SEW_ARREARS { get; set; }
        public string LPW_SEW_CURBILL_AMT { get; set; }
        public string LPW_SEW_FEES { get; set; }
        public string LPW_SEW_TOT { get; set; }
        public string LPW_SEW_AWARD { get; set; }
        public string LPW_SEW_PAID { get; set; }
        public string LPW_SEW_AWAITING_DOL { get; set; }
        public string LPW_SEW_BEN_TYPE { get; set; }
        public string LPW_SEW_LEVEL { get; set; }
        public string LPW_SEW_VER_STAT { get; set; }
        public string LPW_SEW_VERIFIED_WORKER { get; set; }
        public string LPW_SEW_VERIFIED_DATE { get; set; }
        public string LPW_SEW_VERIFIED_DATE_LSTC { get; set; }
        public string LPW_SEW_VERIFIED_LSTC_OPERATOR { get; set; }
        public string LPW_DATE_ADD { get; set; }
        public string LPW_ADD_OPERATOR { get; set; }
        public string LPW_DATE_LSTC { get; set; }
        public string LPW_LSTC_OPERATOR { get; set; }


        #endregion
    }

    public class WATERCNTLEntity
    {
        #region Constructors

        public WATERCNTLEntity()
        {
            WCNTL_YEAR = string.Empty;
            WCNTL_BEN_TYPE = string.Empty;
            WCNTL_Award = string.Empty;
            WCNTL_SDate = string.Empty;
            WCNTL_EDate = string.Empty;
            WCNTL_L1_Vul = string.Empty;
            WCNTL_L2_VuL = string.Empty;
            WCNTL_L3_Vul = string.Empty;
            WCNTL_L4_Vul = string.Empty;
            WCNTL_L5_Vul = string.Empty;
            WCNTL_L1_NonVul = string.Empty;
            WCNTL_L2_NonVul = string.Empty;
            WCNTL_L3_NonVul = string.Empty;
            WCNTL_L4_NonVul = string.Empty;
            WCNTL_L5_NonVul = string.Empty;
            WCNTL_DATE_ADD = string.Empty;
            WCNTL_ADD_OPERATOR = string.Empty;
            WCNTL_DATE_LSTC = string.Empty;
            WCNTL_LSTC_OPERATOR = string.Empty;
            Mode = string.Empty;
        }




        public WATERCNTLEntity(DataRow row)
        {
            if (row != null)
            {
                WCNTL_YEAR = row["WCNTL_YEAR"].ToString();
                WCNTL_BEN_TYPE = row["WCNTL_BEN_TYPE"].ToString();
                WCNTL_Award = row["WCNTL_Award"].ToString();
                WCNTL_SDate = row["WCNTL_SDate"].ToString();
                WCNTL_EDate = row["WCNTL_EDate"].ToString();
                WCNTL_L1_Vul = row["WCNTL_L1_Vul"].ToString();
                WCNTL_L2_VuL = row["WCNTL_L2_VuL"].ToString();
                WCNTL_L3_Vul = row["WCNTL_L3_Vul"].ToString();
                WCNTL_L4_Vul = row["WCNTL_L4_Vul"].ToString();
                WCNTL_L5_Vul = row["WCNTL_L5_Vul"].ToString();
                WCNTL_L1_NonVul = row["WCNTL_L1_NonVul"].ToString();
                WCNTL_L2_NonVul = row["WCNTL_L2_NonVul"].ToString();
                WCNTL_L3_NonVul = row["WCNTL_L3_NonVul"].ToString();
                WCNTL_L4_NonVul = row["WCNTL_L4_NonVul"].ToString();
                WCNTL_L5_NonVul = row["WCNTL_L5_NonVul"].ToString();
                WCNTL_DATE_ADD = row["WCNTL_DATE_ADD"].ToString();
                WCNTL_ADD_OPERATOR = row["WCNTL_ADD_OPERATOR"].ToString();
                WCNTL_DATE_LSTC = row["WCNTL_DATE_LSTC"].ToString();
                WCNTL_LSTC_OPERATOR = row["WCNTL_LSTC_OPERATOR"].ToString();
                Mode = "U";

            }
        }


        #endregion

        #region Properties

        public string WCNTL_YEAR { get; set; }
        public string WCNTL_BEN_TYPE { get; set; }
        public string WCNTL_Award { get; set; }
        public string WCNTL_SDate { get; set; }
        public string WCNTL_EDate { get; set; }
        public string WCNTL_L1_Vul { get; set; }
        public string WCNTL_L2_VuL { get; set; }
        public string WCNTL_L3_Vul { get; set; }
        public string WCNTL_L4_Vul { get; set; }
        public string WCNTL_L5_Vul { get; set; }
        public string WCNTL_L1_NonVul { get; set; }
        public string WCNTL_L2_NonVul { get; set; }
        public string WCNTL_L3_NonVul { get; set; }
        public string WCNTL_L4_NonVul { get; set; }
        public string WCNTL_L5_NonVul { get; set; }
        public string WCNTL_DATE_ADD { get; set; }
        public string WCNTL_ADD_OPERATOR { get; set; }
        public string WCNTL_DATE_LSTC { get; set; }
        public string WCNTL_LSTC_OPERATOR { get; set; }
        public string Mode { get; set; }

        #endregion
    }

}
