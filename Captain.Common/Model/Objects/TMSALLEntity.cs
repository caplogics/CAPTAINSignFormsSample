using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Captain.Common.Model.Objects
{
    public class NotesEntity
    {
        #region Constructors

        public NotesEntity()
        {
            VDN_AGENCY = string.Empty;
            VDN_DEPT = string.Empty;
            VDN_PROGRAM = string.Empty;
            VDN_YEAR = string.Empty;
            VDN_APP_NO = string.Empty;
            VDN_BY_PROG = string.Empty;
            VDN_TYPE = string.Empty;
            VDN_VENDOR = string.Empty;
            VDN_SEQUENCE = string.Empty;
            VDN_DATE_REPORTED = string.Empty;
            VDN_DATE_SEQUENCE = string.Empty;
            VDN_RERUN = string.Empty;
            VDN_CERT_STATUS = string.Empty;
            VDN_PAGE_NUMBER = string.Empty;
            VDN_TMSB5511_CATEGORY = string.Empty;
            VDN_FILE_FORMAT = string.Empty;
            VDN_DATE_LSTC = string.Empty;
            VDN_LSTC_OPERATOR = string.Empty;
            VDN_DATE_ADD = string.Empty;
            VDN_ADD_OPERATOR = string.Empty;
            AppCount = string.Empty;
             VDN_PMR_DATE = string.Empty;
             VDN_LPB_DATE = string.Empty;
             VDN_MST_DATE = string.Empty;
             PMR_AUTH_NUM = string.Empty;
        }

        public NotesEntity(bool Initalize)
        {
            if (Initalize)
            {
                Rec_Type = "I";

                VDN_AGENCY = null;
                VDN_DEPT = null;
                VDN_PROGRAM = null;
                VDN_YEAR = null;
                VDN_APP_NO = null;
                VDN_BY_PROG = null;
                VDN_TYPE = null;
                VDN_VENDOR = null;
                VDN_SEQUENCE = null;
                VDN_DATE_REPORTED = null;
                VDN_DATE_SEQUENCE = null;
                VDN_RERUN = null;
                VDN_CERT_STATUS = null;
                VDN_PAGE_NUMBER = null;
                VDN_TMSB5511_CATEGORY = null;
                VDN_FILE_FORMAT = null;
                VDN_DATE_LSTC = null;
                VDN_LSTC_OPERATOR = null;
                VDN_DATE_ADD = null;
                VDN_ADD_OPERATOR = null;
                AppCount = null;
                 VDN_PMR_DATE = null;
                 VDN_LPB_DATE = null;
                 PMR_AUTH_NUM = null;

            }
        }

        public NotesEntity(DataRow Row)
        {
            if (Row != null)
            {
                Rec_Type = "U";
                VDN_AGENCY = Row["VDN_AGENCY"].ToString().Trim();
                VDN_DEPT = Row["VDN_DEPT"].ToString().Trim();
                VDN_PROGRAM = Row["VDN_PROGRAM"].ToString().Trim();
                VDN_YEAR = Row["VDN_YEAR"].ToString().Trim();
                VDN_APP_NO = Row["VDN_APP_NO"].ToString().Trim();
                VDN_BY_PROG = Row["VDN_BY_PROG"].ToString().Trim();
                VDN_TYPE = Row["VDN_TYPE"].ToString().Trim();
                VDN_VENDOR = Row["VDN_VENDOR"].ToString().Trim();
                VDN_SEQUENCE = Row["VDN_SEQUENCE"].ToString().Trim();
                VDN_DATE_REPORTED = Row["VDN_DATE_REPORTED"].ToString().Trim();
                VDN_DATE_SEQUENCE = Row["VDN_DATE_SEQUENCE"].ToString().Trim();
                VDN_RERUN = Row["VDN_RERUN"].ToString().Trim();
                VDN_CERT_STATUS = Row["VDN_CERT_STATUS"].ToString().Trim();
                VDN_PAGE_NUMBER = Row["VDN_PAGE_NUMBER"].ToString().Trim();
                VDN_TMSB5511_CATEGORY = Row["VDN_TMSB5511_CATEGORY"].ToString().Trim();
                VDN_FILE_FORMAT = Row["VDN_FILE_FORMAT"].ToString().Trim();
                VDN_DATE_LSTC = Row["VDN_DATE_LSTC"].ToString().Trim();
                VDN_LSTC_OPERATOR = Row["VDN_LSTC_OPERATOR"].ToString().Trim();
                VDN_DATE_ADD = Row["VDN_DATE_ADD"].ToString().Trim();
                VDN_ADD_OPERATOR = Row["VDN_ADD_OPERATOR"].ToString().Trim();
                VDN_PMR_DATE = Row["VDN_PMR_DATE"].ToString().Trim();
                VDN_LPB_DATE = Row["VDN_LPB_DATE"].ToString().Trim();
                VDN_MST_DATE = Row["VDN_MST_DATE"].ToString().Trim();
                PMR_AUTH_NUM = Row["VDN_PMR_AUTH_NUM"].ToString().Trim();
            }
        }

        public NotesEntity(DataRow Row,string Table)
        {
            if (Row != null)
            {
                VDN_AGENCY = Row["VDN_AGENCY"].ToString().Trim();
                VDN_DEPT = Row["VDN_DEPT"].ToString().Trim();
                VDN_PROGRAM = Row["VDN_PROGRAM"].ToString().Trim();
                VDN_YEAR = Row["VDN_YEAR"].ToString().Trim();
                VDN_DATE_REPORTED = Row["VDN_DATE_REPORTED"].ToString().Trim();
                VDN_DATE_SEQUENCE = Row["VDN_DATE_SEQUENCE"].ToString().Trim();
                if(Table=="TMSBCHCT")
                    VDN_RERUN = Row["VDN_RERUN"].ToString().Trim();
                AppCount = Row["APP_COUNT"].ToString().Trim();
                VDN_LSTC_OPERATOR = Row["VDN_LSTC_OPERATOR"].ToString().Trim();
            }
        }
        public NotesEntity(DataRow Row, string Table,string strSeq)
        {
            if (Row != null)
            {
                VDN_AGENCY =string.Empty;
                VDN_DEPT = string.Empty;
                VDN_PROGRAM = string.Empty;
                VDN_YEAR = string.Empty;
                VDN_DATE_REPORTED = string.Empty;
                VDN_DATE_SEQUENCE = Row["VDN_SEQ"].ToString().Trim();
                //VDN_RERUN = Row["VDN_RERUN"].ToString().Trim();
                AppCount = string.Empty;
                VDN_LSTC_OPERATOR = string.Empty; 
            }
        }
      




        #endregion

        #region Properties

        public string VDN_AGENCY { get; set; }
        public string VDN_DEPT { get; set; }
        public string VDN_PROGRAM { get; set; }
        public string VDN_YEAR { get; set; }
        public string VDN_APP_NO { get; set; }
        public string VDN_BY_PROG { get; set; }
        public string VDN_TYPE { get; set; }
        public string VDN_VENDOR { get; set; }
        public string VDN_SEQUENCE { get; set; }
        public string VDN_DATE_REPORTED { get; set; }
        public string VDN_DATE_SEQUENCE { get; set; }
        public string VDN_RERUN { get; set; }
        public string VDN_CERT_STATUS { get; set; }
        public string VDN_PAGE_NUMBER { get; set; }
        public string VDN_TMSB5511_CATEGORY { get; set; }
        public string VDN_FILE_FORMAT { get; set; }
        public string VDN_DATE_LSTC { get; set; }
        public string VDN_LSTC_OPERATOR { get; set; }
        public string VDN_DATE_ADD { get; set; }
        public string VDN_ADD_OPERATOR { get; set; }
        public string VDN_PMR_DATE { get; set; }
        public string VDN_LPB_DATE { get; set; }
        public string VDN_MST_DATE { get; set; }
        public string PMR_AUTH_NUM { get; set; }
        public string Mode { get; set; }

        public string AppCount { get; set; }  

        public string Rec_Type { get; set; }

        #endregion

    }

    public class TMSB0003Entity
    {
        #region Constructors

        public TMSB0003Entity()
        {
            AGENCY = string.Empty;
            DEPT = string.Empty;
            PROGRAM = string.Empty;
            YEAR = string.Empty;
            APP_NO = string.Empty;
            FName = string.Empty;
            MName = string.Empty;
            LName = string.Empty;
            SSN = string.Empty;
            Phone = string.Empty;
            Hno = string.Empty;
            Street = string.Empty;
            Suffix = string.Empty;
            Apt = string.Empty;
            Flr = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;
            IntakeDate = string.Empty;
            Site = string.Empty;
            Source = string.Empty;
            Heat_Inc_Rent = string.Empty;
            Disable = string.Empty;
            Age = string.Empty;
            CityCode = string.Empty;
            CityName = string.Empty;
            SiteName = string.Empty;
            Race = string.Empty;
            Ethnic = string.Empty;
            Sex = string.Empty;
        }

        public TMSB0003Entity(bool Initalize)
        {
            if (Initalize)
            {
                AGENCY = null;
                DEPT = null;
                PROGRAM = null;
                YEAR = null;
                APP_NO = null;
                FName = null;
                MName = null;
                LName = null;
                SSN = null;
                Phone = null;
                Hno = null;
                Street = null;
                Suffix = null;
                Apt = null;
                Flr = null;
                City = null;
                State = null;
                Zip = null;
                IntakeDate = null;
                Site = null;
                Source = null;
                Heat_Inc_Rent = null;
                Disable = null;
                Age = null;
                CityCode = null;
                CityName = null;
                SiteName = null;
                Race  = null;
                Ethnic= null;
                Sex = null;

            }
        }

        public TMSB0003Entity(DataRow Row)
        {
            if (Row != null)
            {
                AGENCY = Row["MST_AGENCY"].ToString().Trim();
                DEPT = Row["MST_DEPT"].ToString().Trim();
                PROGRAM = Row["MST_PROGRAM"].ToString().Trim();
                YEAR = Row["MST_YEAR"].ToString().Trim();
                APP_NO = Row["MST_APP_NO"].ToString().Trim();
                FName = Row["SNP_NAME_IX_FI"].ToString().Trim();
                MName = Row["SNP_NAME_IX_MI"].ToString().Trim();
                LName = Row["SNP_NAME_IX_LAST"].ToString().Trim();
                SSN = Row["MST_SSN"].ToString().Trim();
                Phone = Row["Phone"].ToString().Trim();
                Hno = Row["MST_HN"].ToString().Trim();
                Street = Row["MST_STREET"].ToString().Trim();
                Suffix = Row["MST_SUFFIX"].ToString().Trim();
                Apt = Row["MST_APT"].ToString().Trim();
                Flr = Row["MST_FLR"].ToString().Trim();
                City = Row["MST_CITY"].ToString().Trim();
                State = Row["MST_STATE"].ToString().Trim();
                Zip = Row["MST_ZIP"].ToString().Trim();
                IntakeDate = Row["MST_INTAKE_DATE"].ToString().Trim();
                Site = Row["MST_SITE"].ToString().Trim();
                Source = Row["MST_SOURCE"].ToString().Trim();
                Heat_Inc_Rent = Row["MST_HEAT_INC_RENT"].ToString().Trim();
                Disable = Row["SNP_DISABLE"].ToString().Trim();
                Age = Row["SNP_AGE"].ToString().Trim();
                CityCode = Row["CITY_CODE"].ToString().Trim();
                CityName = Row["CITY_NAME"].ToString().Trim();
                SiteName = Row["SITE_NAME"].ToString().Trim();
                Race = Row["SNP_RACE"].ToString().Trim();
                Ethnic = Row["SNP_ETHNIC"].ToString().Trim();
                Sex = Row["SNP_SEX"].ToString().Trim();

            }
        }



        #endregion

        #region Properties

        public string AGENCY { get; set; }
        public string DEPT { get; set; }
        public string PROGRAM { get; set; }
        public string YEAR { get; set; }
        public string APP_NO { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string SSN { get; set; }
        public string Phone { get; set; }
        public string Hno { get; set; }
        public string Street { get; set; }
        public string Suffix { get; set; }
        public string Apt { get; set; }
        public string Flr { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string IntakeDate { get; set; }
        public string Site { get; set; }
        public string Source { get; set; }
        public string Heat_Inc_Rent { get; set; }
        public string Disable { get; set; }
        public string Age { get; set; }
        public string  CityCode { get; set; }
        public string CityName { get; set; }
        public string SiteName { get; set; }
        public string Race { get; set; }
        public string Ethnic { get; set; }
        public string Sex { get; set; }
        
        //public string VDN_ADD_OPERATOR { get; set; }
        //public string Mode { get; set; }

        public string Rec_Type { get; set; }

        #endregion

    }

    public class TMSB0001Entity
    {
        #region Constructors

        public TMSB0001Entity()
        {
            AGENCY = string.Empty;
            DEPT = string.Empty;
            PROGRAM = string.Empty;
            YEAR = string.Empty;
            APP_NO = string.Empty;
            Seq = string.Empty;
            LPB_Date = string.Empty;
            Housing = string.Empty;
            Poverty = string.Empty;
            Heat_inc = string.Empty;
            Assets = string.Empty;
            Benefit_Level = string.Empty;
            EligIncome = string.Empty;
            EligRent = string.Empty;
            EligFuel = string.Empty;
            Certified_Status = string.Empty;
            BirthDate = string.Empty;
            Age = string.Empty;
            Disable = string.Empty;
            Intakedate = string.Empty;
        }

        public TMSB0001Entity(bool Initalize)
        {
            if (Initalize)
            {
                AGENCY = null;
                DEPT = null;
                PROGRAM = null;
                YEAR = null;
                APP_NO = null;
                Seq = null;
                LPB_Date = null;
                Housing = null;
                Poverty = null;
                Heat_inc = null;
                Assets = null;
                Benefit_Level = null;
                EligIncome = null;
                EligRent = null;
                EligFuel = null;
                Certified_Status = null;
                BirthDate = null;
                Age = null;
                Disable = null;
                Intakedate = null;  

            }
        }

        public TMSB0001Entity(DataRow Row)
        {
            if (Row != null)
            {
                AGENCY = Row["MST_AGENCY"].ToString().Trim();
                DEPT = Row["MST_DEPT"].ToString().Trim();
                PROGRAM = Row["MST_PROGRAM"].ToString().Trim();
                YEAR = Row["MST_YEAR"].ToString().Trim();
                APP_NO = Row["MST_APP_NO"].ToString().Trim();
                Seq = Row["LPB_SEQ"].ToString().Trim();
                LPB_Date = Row["LPB_DATE"].ToString().Trim();
                Housing = Row["MST_HOUSING"].ToString().Trim();
                Poverty = Row["MST_POVERTY"].ToString().Trim();
                Heat_inc = Row["MST_HEAT_INC_RENT"].ToString().Trim();
                Assets = Row["LPB_ASSETS"].ToString().Trim();
                Benefit_Level = Row["LPB_BENEFIT_LEVEL"].ToString().Trim();
                EligIncome = Row["LPB_ELIGINCOME"].ToString().Trim();
                EligRent = Row["LPB_ELIGRENT"].ToString().Trim();
                EligFuel = Row["LPB_ELIGFUEL"].ToString().Trim();
                Certified_Status = Row["LPB_CERTIFIED_STATUS"].ToString().Trim();
                BirthDate = Row["SNP_ALT_BDATE"].ToString().Trim();
                Age = Row["SNP_AGE"].ToString().Trim();
                Disable = Row["SNP_DISABLE"].ToString().Trim();
                Intakedate = Row["MST_INTAKE_DATE"].ToString().Trim();
                

            }
        }



        #endregion

        #region Properties

        public string AGENCY { get; set; }
        public string DEPT { get; set; }
        public string PROGRAM { get; set; }
        public string YEAR { get; set; }
        public string APP_NO { get; set; }
        public string Seq { get; set; }
        public string LPB_Date { get; set; }
        public string Housing { get; set; }
        public string Poverty { get; set; }
        public string Heat_inc { get; set; }
        public string Assets { get; set; }
        public string Benefit_Level { get; set; }
        public string EligIncome { get; set; }
        public string EligRent { get; set; }
        public string EligFuel { get; set; }
        public string Certified_Status { get; set; }
        public string BirthDate { get; set; }
        public string Age { get; set; }
        public string Disable { get; set; }
        public string Intakedate { get; set; }
        

        //public string VDN_ADD_OPERATOR { get; set; }
        //public string Mode { get; set; }

        public string Rec_Type { get; set; }

        #endregion

    }


    public class TMS81ReportEntity
    {
        #region Constructors

        public TMS81ReportEntity()
        {
            AGENCY = string.Empty;
            DEPT = string.Empty;
            PROGRAM = string.Empty;
            YEAR = string.Empty;
            APP_NO = string.Empty;
            FName = string.Empty;
            MName = string.Empty;
            LName = string.Empty;
            //Phone = string.Empty;
            Hno = string.Empty;
            Street = string.Empty;
            Suffix = string.Empty;
            Apt = string.Empty;
            Flr = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;
            ZipPlus = string.Empty;
            Site = string.Empty;
            Heat_Inc_Rent = string.Empty;
            LPB_Seq = string.Empty;
            LPV_PRIMARY_CODE = string.Empty;
            LPB_CERTIFIED_STATUS = string.Empty;
            LPB_TYPE = string.Empty;
            LPB_INC = string.Empty;
            LPB_ELIGRENT = string.Empty;
            LPB_ELIGFUEL = string.Empty;
            LPB_AMOUNT = string.Empty;
            LPB_REDUCE_ELIG = string.Empty;
            LPB_BENEFIT_LEVEL = string.Empty;
            LPB_ELIGWTHR = string.Empty;
            MST_POVERTY = string.Empty;
            MST_DIRECTION = string.Empty;
            MST_LANGUAGE = string.Empty;
            LPB_DATE = string.Empty;
            LPV_AccNo = string.Empty;
            LPB_SSN_SW = string.Empty;
            LPB_ELIGINCOME = string.Empty;
            LPB_ELIGASSETS = string.Empty;
            Casenotes_Screen = string.Empty;
            CaseNotes_Data = string.Empty;
            LPB_DENIED_REASON=string.Empty;
        }

        public TMS81ReportEntity(bool Initalize)
        {
            if (Initalize)
            {
                AGENCY = null;
                DEPT = null;
                PROGRAM = null;
                YEAR = null;
                APP_NO = null;
                FName = null;
                MName = null;
                LName = null;
                //Phone = null;
                Hno = null;
                Street = null;
                Suffix = null;
                Apt = null;
                Flr = null;
                City = null;
                State = null;
                Zip = null;


                ZipPlus = null;
                Site = null;
                Heat_Inc_Rent = null;
                LPB_Seq = null;
                LPV_PRIMARY_CODE = null;
                LPB_CERTIFIED_STATUS = null;
                LPB_TYPE = null;
                LPB_INC = null;
                LPB_ELIGRENT = null;
                LPB_ELIGFUEL = null;
                LPB_AMOUNT = null;
                LPB_REDUCE_ELIG = null;
                LPB_BENEFIT_LEVEL = null;
                LPB_ELIGWTHR = null;
                MST_POVERTY = null;
                MST_DIRECTION = null;
                MST_LANGUAGE = null;
                LPB_DATE = null;
                LPV_AccNo = null;
                LPB_SSN_SW = null;
                LPB_ELIGINCOME = null;
                LPB_ELIGASSETS = null;
                Casenotes_Screen = null;
                CaseNotes_Data = null;
                LPB_DENIED_REASON = null;
            }
        }

        public TMS81ReportEntity(DataRow Row)
        {
            if (Row != null)
            {
                AGENCY = Row["MST_AGENCY"].ToString().Trim();
                DEPT = Row["MST_DEPT"].ToString().Trim();
                PROGRAM = Row["MST_PROGRAM"].ToString().Trim();
                YEAR = Row["MST_YEAR"].ToString().Trim();
                APP_NO = Row["MST_APP_NO"].ToString().Trim();
                FName = Row["SNP_NAME_IX_FI"].ToString().Trim();
                MName = Row["SNP_NAME_IX_MI"].ToString().Trim();
                LName = Row["SNP_NAME_IX_LAST"].ToString().Trim();
                //Phone = Row["Phone"].ToString().Trim();
                Hno = Row["MST_HN"].ToString().Trim();
                Street = Row["MST_STREET"].ToString().Trim();
                Suffix = Row["MST_SUFFIX"].ToString().Trim();
                Apt = Row["MST_APT"].ToString().Trim();
                Flr = Row["MST_FLR"].ToString().Trim();
                City = Row["MST_CITY"].ToString().Trim();
                State = Row["MST_STATE"].ToString().Trim();
                Zip = Row["MST_ZIP"].ToString().Trim();
                ZipPlus = Row["MST_ZIPPLUS"].ToString().Trim();
                Site = Row["MST_SITE"].ToString().Trim();
                Heat_Inc_Rent = Row["MST_HEAT_INC_RENT"].ToString().Trim();

                LPB_Seq = Row["LPB_SEQ"].ToString().Trim();
                LPV_PRIMARY_CODE = Row["LPV_PRIMARY_CODE"].ToString().Trim();
                LPB_CERTIFIED_STATUS = Row["LPB_CERTIFIED_STATUS"].ToString().Trim();
                LPB_TYPE = Row["LPB_TYPE"].ToString().Trim();
                LPB_INC = Row["LPB_INC"].ToString().Trim();
                LPB_ELIGRENT = Row["LPB_ELIGRENT"].ToString().Trim();
                LPB_ELIGFUEL = Row["LPB_ELIGFUEL"].ToString().Trim();
                LPB_AMOUNT = Row["LPB_AMOUNT"].ToString().Trim();
                LPB_REDUCE_ELIG = Row["LPB_REDUCE_ELIG"].ToString().Trim();
                LPB_BENEFIT_LEVEL = Row["LPB_BENEFIT_LEVEL"].ToString().Trim();
                LPB_ELIGWTHR = Row["LPB_ELIGWTHR"].ToString().Trim();
                MST_POVERTY = Row["MST_POVERTY"].ToString().Trim();
                MST_DIRECTION = Row["MST_DIRECTION"].ToString().Trim();
                MST_LANGUAGE = Row["MST_LANGUAGE"].ToString().Trim();
                LPB_DATE = Row["LPB_DATE"].ToString().Trim();
                LPV_AccNo = Row["LPV_ACCOUNT_NO"].ToString().Trim();
                LPB_SSN_SW = Row["LPB_SSN_SW"].ToString().Trim();
                LPB_ELIGINCOME = Row["LPB_ELIGINCOME"].ToString().Trim();
                LPB_ELIGASSETS = Row["LPB_ELIGASSETS"].ToString().Trim();
                Casenotes_Screen = Row["CASENOTES_SCREEN"].ToString().Trim();
                CaseNotes_Data = Row["CASENOTES_DATA"].ToString().Trim();

                LPB_DENIED_REASON = Row["LPB_DENIED_REASON"].ToString().Trim();
            }
        }



        #endregion

        #region Properties

        public string AGENCY { get; set; }
        public string DEPT { get; set; }
        public string PROGRAM { get; set; }
        public string YEAR { get; set; }
        public string APP_NO { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string Phone { get; set; }
        public string Hno { get; set; }
        public string Street { get; set; }
        public string Suffix { get; set; }
        public string Apt { get; set; }
        public string Flr { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ZipPlus { get; set; }
        public string Site { get; set; }
        public string Heat_Inc_Rent { get; set; }
        public string LPB_Seq { get; set; }
        public string LPV_PRIMARY_CODE { get; set; }
        public string LPB_CERTIFIED_STATUS { get; set; }
        public string LPB_TYPE { get; set; }
        public string LPB_INC { get; set; }
        public string LPB_ELIGRENT { get; set; }
        public string LPB_ELIGFUEL { get; set; }
        public string LPB_AMOUNT { get; set; }
        public string LPB_REDUCE_ELIG { get; set; }
        public string LPB_BENEFIT_LEVEL { get; set; }
        public string LPB_ELIGWTHR { get; set; }
        public string MST_POVERTY { get; set; }

        public string MST_DIRECTION { get; set; }
        public string MST_LANGUAGE { get; set; }
        public string LPB_DATE { get; set; }
        public string LPV_AccNo { get; set; }
        public string LPB_SSN_SW { get; set; } 
        public string LPB_ELIGINCOME { get; set; }
        public string LPB_ELIGASSETS { get; set; }
        public string Casenotes_Screen { get; set; }
        public string CaseNotes_Data { get; set; }

        public string LPB_DENIED_REASON { get; set; }


        //public string VDN_ADD_OPERATOR { get; set; }
        //public string Mode { get; set; }

        public string Rec_Type { get; set; }

        #endregion

    }

    public class PAYBATCHEntity
    {
        #region Constructors

        public PAYBATCHEntity()
        {
            Rec_Type =
            PBR_AGENCY = 
            PBR_DEPT = 
            PBR_PROGRAM = 
            PBR_YEAR = 
            PBR_NO_N = 
            PBR_PAYMENT_TYPE = 
            PBR_CASH_BULK = 
            PBR_PAY_NUMBER1 = 
            PBR_STATUS = 
            PBR_DATE_OPENED = 
            PBR_FED_PAYS = 
            PBR_ST_PAYS = 
            PBR_GALLONS = 
            PBR_FED_PAY_DOL = 
            PBR_ST_PAY_DOL = 
            PBR_CHAP_PAYS = 

            PBR_FUND = 
            PBR_SCROLL = 
            PBR_TYPE = 
            PBR_AUTO_P_CODE = 
            PBR_AUTO_P_VENDOR = 
            PBR_CHECK_NO = 
            PBR_CHECK_DATE = 
            PBR_VOUCH_NO = 
            PBR_PAY_NUMBER = 
            PBR_VENDOR = 
            PBR_REP_TYPE = 
            PBR_DOL_LIMIT = 
            PBR_L_DATE = 
            PBR_H_DATE = 
            PBR_DATE_LSTC =
            PBR_COUNT = 
            PBR_PAY_NUMBER2=
            PBR_PAY_NUMBER3=
            PBR_LSTC_OPERATOR = string.Empty;

        }

        public PAYBATCHEntity(bool Initalize)
        {
            if (Initalize)
            {
                Rec_Type =
                PBR_AGENCY = 
                PBR_DEPT = 
                PBR_PROGRAM = 
                PBR_YEAR = 
                PBR_NO_N = 
                PBR_PAYMENT_TYPE = 
                PBR_CASH_BULK = 
                PBR_PAY_NUMBER1 = 
                PBR_STATUS = 
                PBR_DATE_OPENED = 
                PBR_FED_PAYS = 
                PBR_ST_PAYS = 
                PBR_GALLONS = 
                PBR_FED_PAY_DOL = 
                PBR_ST_PAY_DOL = 
                PBR_CHAP_PAYS = 

                PBR_FUND = 
                PBR_SCROLL = 
                PBR_TYPE =
                PBR_AUTO_P_CODE = 
                PBR_AUTO_P_VENDOR = 
                PBR_CHECK_NO = 
                PBR_CHECK_DATE = 
                PBR_VOUCH_NO = 
                PBR_PAY_NUMBER = 
                PBR_VENDOR = 
                PBR_REP_TYPE = 
                PBR_DOL_LIMIT = 
                PBR_L_DATE = 
                PBR_H_DATE = 
                PBR_DATE_LSTC = 
                PBR_COUNT=
                PBR_PAY_NUMBER2=
                PBR_PAY_NUMBER3=
                PBR_LSTC_OPERATOR = null;
                
            }
        }

        public PAYBATCHEntity(DataRow Row)
        {
            if (Row != null)
            {
                Rec_Type = "U";
                PBR_AGENCY = Row["PBR_AGENCY"].ToString().Trim();
                PBR_DEPT = Row["PBR_DEPT"].ToString().Trim();
                PBR_PROGRAM = Row["PBR_PROGRAM"].ToString().Trim();
                PBR_YEAR = Row["PBR_YEAR"].ToString().Trim();
                PBR_NO_N = Row["PBR_NO_N"].ToString().Trim();
                PBR_PAYMENT_TYPE = Row["PBR_PAYMENT_TYPE"].ToString().Trim();
                PBR_CASH_BULK = Row["PBR_CASH_BULK"].ToString().Trim();
                PBR_PAY_NUMBER1 = Row["PBR_PAY_NUMBER1"].ToString().Trim();
                PBR_STATUS = Row["PBR_STATUS"].ToString().Trim();
                PBR_DATE_OPENED = Row["PBR_DATE_OPENED"].ToString().Trim();
                PBR_FED_PAYS = Row["PBR_FED_PAYS"].ToString().Trim();
                PBR_ST_PAYS = Row["PBR_ST_PAYS"].ToString().Trim();
                PBR_GALLONS = Row["PBR_GALLONS"].ToString().Trim();
                PBR_FED_PAY_DOL = Row["PBR_FED_PAY_DOL"].ToString().Trim();
                PBR_ST_PAY_DOL = Row["PBR_ST_PAY_DOL"].ToString().Trim();
                PBR_CHAP_PAYS = Row["PBR_CHAP_PAYS"].ToString().Trim();
                PBR_FUND = Row["PBR_FUND"].ToString().Trim();
                PBR_SCROLL = Row["PBR_SCROLL"].ToString().Trim();
                PBR_AUTO_P_CODE = Row["PBR_AUTO_P_CODE"].ToString().Trim();

                PBR_TYPE = Row["PBR_TYPE"].ToString().Trim();
                PBR_AUTO_P_VENDOR = Row["PBR_AUTO_P_VENDOR"].ToString().Trim();
                PBR_CHECK_NO = Row["PBR_CHECK_NO"].ToString().Trim();
                PBR_CHECK_DATE = Row["PBR_CHECK_DATE"].ToString().Trim();
                PBR_VOUCH_NO = Row["PBR_VOUCH_NO"].ToString().Trim();
                PBR_PAY_NUMBER = Row["PBR_PAY_NUMBER"].ToString().Trim();
                PBR_VENDOR = Row["PBR_VENDOR"].ToString().Trim();
                PBR_REP_TYPE = Row["PBR_REP_TYPE"].ToString().Trim();
                PBR_DOL_LIMIT = Row["PBR_DOL_LIMIT"].ToString().Trim();
                PBR_L_DATE = Row["PBR_L_DATE"].ToString().Trim();
                PBR_H_DATE = Row["PBR_H_DATE"].ToString().Trim();
                PBR_DATE_LSTC = Row["PBR_DATE_LSTC"].ToString().Trim();
                PBR_LSTC_OPERATOR = Row["PBR_LSTC_OPERATOR"].ToString().Trim();

                PBR_COUNT = Row["PBR_COUNT"].ToString().Trim();
                PBR_PAY_NUMBER2 = Row["PBR_PAY_NUMBER2"].ToString().Trim();
                PBR_PAY_NUMBER3 = Row["PBR_PAY_NUMBER3"].ToString().Trim();
                
            }
        }



        #endregion

        #region Properties

        public string PBR_AGENCY { get; set; }
        public string PBR_DEPT { get; set; }
        public string PBR_PROGRAM { get; set; }
        public string PBR_YEAR { get; set; }
        public string PBR_NO_N { get; set; }
        public string PBR_PAYMENT_TYPE { get; set; }
        public string PBR_CASH_BULK { get; set; }
        public string PBR_PAY_NUMBER1 { get; set; }
        public string PBR_STATUS { get; set; }
        public string PBR_DATE_OPENED { get; set; }
        public string PBR_FED_PAYS { get; set; }
        public string PBR_ST_PAYS { get; set; }
        public string PBR_GALLONS { get; set; }
        public string PBR_FED_PAY_DOL { get; set; }
        public string PBR_ST_PAY_DOL { get; set; }
        public string PBR_CHAP_PAYS { get; set; }
        public string PBR_FUND { get; set; }
        public string PBR_SCROLL { get; set; }
        public string PBR_TYPE { get; set; }
        public string PBR_AUTO_P_CODE { get; set; }
        public string PBR_AUTO_P_VENDOR { get; set; }
        public string PBR_CHECK_NO { get; set; }
        public string PBR_CHECK_DATE { get; set; }
        public string PBR_VOUCH_NO { get; set; }
        public string PBR_PAY_NUMBER { get; set; }
        public string PBR_VENDOR { get; set; }
        public string PBR_REP_TYPE { get; set; }
        public string PBR_DOL_LIMIT { get; set; }
        public string PBR_L_DATE { get; set; }
        public string PBR_H_DATE { get; set; }
        public string PBR_DATE_LSTC { get; set; }
        public string PBR_LSTC_OPERATOR { get; set; }

        public string PBR_COUNT { get; set; }
        public string PBR_PAY_NUMBER2 { get; set; }
        public string PBR_PAY_NUMBER3 { get; set; }
        
        //public string VDN_ADD_OPERATOR { get; set; }
        //public string Mode { get; set; }

        public string Rec_Type { get; set; }

        #endregion

    }

    public class TMSB4010Entity
    {
        #region Constructors

        public TMSB4010Entity()
        {
            PBR_AGENCY = string.Empty;
            PBR_DEPT = string.Empty; 
            PBR_PROGRAM = string.Empty; 
            PBR_YEAR = string.Empty; 
            PBR_NO_N = string.Empty;

            PMR_BATCH_NO = string.Empty;
            PMR_VENDOR = string.Empty;
            PMR_APPL_NO = string.Empty;
            SNP_NAME_IX_LAST = string.Empty;
            SNP_NAME_IX_FI = string.Empty;
            SNP_NAME_IX_MI = string.Empty;
            MST_INTAKE_DATE = string.Empty;
            PBR_TYPE = string.Empty;
            MST_SOURCE = string.Empty;
            LPB_DATE = string.Empty;
            LPB_AMOUNT = string.Empty;
            LPB_REDUCE_ELIG = string.Empty;
            LPB_BENEFIT_LEVEL = string.Empty;
            PMR_BENIFIT_LEVEL = string.Empty;

            PMR_PAYMENT_HOW = string.Empty;
            PMR_AUTH_NUM = string.Empty;
            PMR_DEL_DATE = string.Empty;
            PMR_INVOICE_DATE = string.Empty;
            PMR_AMOUNT_GAL = string.Empty;
            PMR_VENDOR_PP = string.Empty;
            PMR_MOR_PP = string.Empty;
            PMR_AMOUNT_DELIVERY = string.Empty;
            PMR_AMOUNT_STARTUP = string.Empty;
            PMR_AMOUNT_MISC = string.Empty;
            PMR_ACCOUNT = string.Empty;
            CASEVDD1_INDEXBY = string.Empty;
            CASEVDD_NAME = string.Empty;
            PMR_AMOUNT_DOL = string.Empty;
            PBR_PARAM = string.Empty;

        }

        public TMSB4010Entity(bool Initalize)
        {
            if (Initalize)
            {
                PBR_AGENCY = null;
                PBR_DEPT = null;
                PBR_PROGRAM = null;
                PBR_YEAR = null;
                PBR_NO_N = null;

                PMR_BATCH_NO = null;
                PMR_VENDOR = null;
                PMR_APPL_NO = null;
                SNP_NAME_IX_LAST = null;
                SNP_NAME_IX_FI = null;
                SNP_NAME_IX_MI = null;
                MST_INTAKE_DATE = null;
                PBR_TYPE = null;
                MST_SOURCE = null;
                LPB_DATE = null;
                LPB_AMOUNT = null;
                LPB_REDUCE_ELIG = null;
                LPB_BENEFIT_LEVEL = null;
                PMR_BENIFIT_LEVEL = null;

                PMR_PAYMENT_HOW = null;
                PMR_AUTH_NUM = null;
                PMR_DEL_DATE = null;
                PMR_INVOICE_DATE = null;
                PMR_AMOUNT_GAL = null;
                PMR_VENDOR_PP = null;
                PMR_MOR_PP = null;
                PMR_AMOUNT_DELIVERY = null;
                PMR_AMOUNT_STARTUP = null;
                PMR_AMOUNT_MISC = null;
                PMR_ACCOUNT = null;
                CASEVDD1_INDEXBY = null;
                CASEVDD_NAME = null;
                PMR_AMOUNT_DOL = null;
                PBR_PARAM = null;
            }
        }

        public TMSB4010Entity(DataRow Row)
        {
            if (Row != null)
            {
                PBR_AGENCY = Row["PBR_AGENCY"].ToString().Trim();
                PBR_DEPT = Row["PBR_DEPT"].ToString().Trim();
                PBR_PROGRAM = Row["PBR_PROGRAM"].ToString().Trim();
                PBR_YEAR = Row["PBR_YEAR"].ToString().Trim();
                PBR_NO_N = Row["PBR_NO_N"].ToString().Trim();

                PMR_BATCH_NO = Row["PMR_BATCH_NO"].ToString().Trim();
                PMR_VENDOR = Row["PMR_VENDOR"].ToString().Trim();
                PMR_APPL_NO = Row["PMR_APPL_NO"].ToString().Trim();
                SNP_NAME_IX_LAST = Row["SNP_NAME_IX_LAST"].ToString().Trim();
                SNP_NAME_IX_FI = Row["SNP_NAME_IX_FI"].ToString().Trim();
                SNP_NAME_IX_MI = Row["SNP_NAME_IX_MI"].ToString().Trim();
                MST_INTAKE_DATE = Row["MST_INTAKE_DATE"].ToString().Trim();
                PBR_TYPE = Row["PBR_TYPE"].ToString().Trim();
                MST_SOURCE = Row["MST_SOURCE"].ToString().Trim();
                LPB_DATE = Row["LPB_DATE"].ToString().Trim();
                LPB_AMOUNT = Row["LPB_AMOUNT"].ToString().Trim();
                LPB_REDUCE_ELIG = Row["LPB_REDUCE_ELIG"].ToString().Trim();
                LPB_BENEFIT_LEVEL = Row["LPB_BENEFIT_LEVEL"].ToString().Trim();
                PMR_BENIFIT_LEVEL = Row["PMR_BENIFIT_LEVEL"].ToString().Trim();

                PMR_PAYMENT_HOW = Row["PMR_PAYMENT_HOW"].ToString().Trim();
                PMR_AUTH_NUM = Row["PMR_AUTH_NUM"].ToString().Trim();
                PMR_DEL_DATE = Row["PMR_DEL_DATE"].ToString().Trim();
                PMR_INVOICE_DATE = Row["PMR_INVOICE_DATE"].ToString().Trim();
                PMR_AMOUNT_GAL = Row["PMR_AMOUNT_GAL"].ToString().Trim();
                PMR_VENDOR_PP = Row["PMR_VENDOR_PP"].ToString().Trim();
                PMR_MOR_PP = Row["PMR_MOR_PP"].ToString().Trim();
                PMR_AMOUNT_DELIVERY = Row["PMR_AMOUNT_DELIVERY"].ToString().Trim();
                PMR_AMOUNT_STARTUP = Row["PMR_AMOUNT_STARTUP"].ToString().Trim();
                PMR_AMOUNT_MISC = Row["PMR_AMOUNT_MISC"].ToString().Trim();
                PMR_ACCOUNT = Row["PMR_ACCOUNT"].ToString().Trim();
                CASEVDD1_INDEXBY = Row["CASEVDD1_INDEXBY"].ToString().Trim();
                CASEVDD_NAME = Row["CASEVDD_NAME"].ToString().Trim();
                PMR_AMOUNT_DOL = Row["PMR_AMOUNT_DOL"].ToString().Trim();
                PBR_PARAM = Row["PBR_PARAMS"].ToString().Trim();

                //PBR_PAYMENT_TYPE = Row["PBR_PAYMENT_TYPE"].ToString().Trim();
                //PBR_CASH_BULK = Row["PBR_CASH_BULK"].ToString().Trim();
                //PBR_PAY_NUMBER1 = Row["PBR_PAY_NUMBER1"].ToString().Trim();
                //PBR_STATUS = Row["PBR_STATUS"].ToString().Trim();
                //PBR_DATE_OPENED = Row["PBR_DATE_OPENED"].ToString().Trim();
                //PBR_FED_PAYS = Row["PBR_FED_PAYS"].ToString().Trim();
                //PBR_ST_PAYS = Row["PBR_ST_PAYS"].ToString().Trim();
                //PBR_GALLONS = Row["PBR_GALLONS"].ToString().Trim();
                //PBR_FED_PAY_DOL = Row["PBR_FED_PAY_DOL"].ToString().Trim();
                //PBR_ST_PAY_DOL = Row["PBR_ST_PAY_DOL"].ToString().Trim();
                //PBR_CHAP_PAYS = Row["PBR_CHAP_PAYS"].ToString().Trim();
                //PBR_FUND = Row["PBR_FUND"].ToString().Trim();
                //PBR_SCROLL = Row["PBR_SCROLL"].ToString().Trim();
                //PBR_AUTO_P_CODE = Row["PBR_AUTO_P_CODE"].ToString().Trim();

                //PBR_AUTO_P_VENDOR = Row["PBR_AUTO_P_VENDOR"].ToString().Trim();
                //PBR_CHECK_NO = Row["PBR_CHECK_NO"].ToString().Trim();
                //PBR_CHECK_DATE = Row["PBR_CHECK_DATE"].ToString().Trim();
                //PBR_VOUCH_NO = Row["PBR_VOUCH_NO"].ToString().Trim();
                //PBR_PAY_NUMBER = Row["PBR_PAY_NUMBER"].ToString().Trim();
                //PBR_VENDOR = Row["PBR_VENDOR"].ToString().Trim();
                //PBR_REP_TYPE = Row["PBR_REP_TYPE"].ToString().Trim();
                //PBR_DOL_LIMIT = Row["PBR_DOL_LIMIT"].ToString().Trim();
                //PBR_L_DATE = Row["PBR_L_DATE"].ToString().Trim();
                //PBR_H_DATE = Row["PBR_H_DATE"].ToString().Trim();
                //PBR_DATE_LSTC = Row["PBR_DATE_LSTC"].ToString().Trim();
                //PBR_LSTC_OPERATOR = Row["PBR_LSTC_OPERATOR"].ToString().Trim();

            }
        }

        public TMSB4010Entity(DataRow Row, string strTable)
        {
            if (Row != null)
            {
                PBR_AGENCY = Row["PBR_AGENCY"].ToString().Trim();
                PBR_DEPT = Row["PBR_DEPT"].ToString().Trim();
                PBR_PROGRAM = Row["PBR_PROGRAM"].ToString().Trim();
                PBR_YEAR = Row["PBR_YEAR"].ToString().Trim();
                PBR_NO_N = Row["PBR_NO_N"].ToString().Trim();

                PMR_BATCH_NO = Row["PMR_BATCH_NO"].ToString().Trim();
                PMR_VENDOR = Row["PMR_VENDOR"].ToString().Trim();
                PMR_APPL_NO = Row["PMR_APPL_NO"].ToString().Trim();
                SNP_NAME_IX_LAST = Row["SNP_NAME_IX_LAST"].ToString().Trim();
                SNP_NAME_IX_FI = Row["SNP_NAME_IX_FI"].ToString().Trim();
                SNP_NAME_IX_MI = Row["SNP_NAME_IX_MI"].ToString().Trim();
                MST_INTAKE_DATE = Row["MST_INTAKE_DATE"].ToString().Trim();
                PBR_TYPE = Row["PBR_TYPE"].ToString().Trim();
                MST_SOURCE = Row["MST_SOURCE"].ToString().Trim();
                LPB_DATE = Row["LPB_DATE"].ToString().Trim();
                LPB_AMOUNT = Row["LPB_AMOUNT"].ToString().Trim();
                LPB_REDUCE_ELIG = Row["LPB_REDUCE_ELIG"].ToString().Trim();
                LPB_BENEFIT_LEVEL = Row["LPB_BENEFIT_LEVEL"].ToString().Trim();
                PMR_BENIFIT_LEVEL = Row["PMR_BENIFIT_LEVEL"].ToString().Trim();

                PMR_PAYMENT_HOW = Row["PMR_PAYMENT_HOW"].ToString().Trim();
                PMR_AUTH_NUM = Row["PMR_AUTH_NUM"].ToString().Trim();
                PMR_DEL_DATE = Row["PMR_DEL_DATE"].ToString().Trim();
                PMR_INVOICE_DATE = Row["PMR_INVOICE_DATE"].ToString().Trim();
                PMR_AMOUNT_GAL = Row["PMR_AMOUNT_GAL"].ToString().Trim();
                PMR_VENDOR_PP = Row["PMR_VENDOR_PP"].ToString().Trim();
                PMR_MOR_PP = Row["PMR_MOR_PP"].ToString().Trim();
                PMR_AMOUNT_DELIVERY = Row["PMR_AMOUNT_DELIVERY"].ToString().Trim();
                PMR_AMOUNT_STARTUP = Row["PMR_AMOUNT_STARTUP"].ToString().Trim();
                PMR_AMOUNT_MISC = Row["PMR_AMOUNT_MISC"].ToString().Trim();
                PMR_ACCOUNT = Row["PMR_ACCOUNT"].ToString().Trim();
                CASEVDD1_INDEXBY = Row["CASEVDD1_INDEXBY"].ToString().Trim();
                CASEVDD_NAME = Row["CASEVDD_NAME"].ToString().Trim();
                PMR_AMOUNT_DOL = Row["PMR_AMOUNT_DOL"].ToString().Trim();
                PBR_PARAM = Row["PBR_PARAMS"].ToString().Trim();
                ReduceElig = Row["ReduceElig"].ToString().Trim();



            }
        }


        #endregion

        #region Properties

        public string PBR_AGENCY { get; set; }
        public string PBR_DEPT { get; set; }
        public string PBR_PROGRAM { get; set; }
        public string PBR_YEAR { get; set; }
        public string PBR_NO_N { get; set; }
       
        public string PMR_BATCH_NO { get; set; }
        public string PMR_VENDOR { get; set; }
        public string PMR_APPL_NO { get; set; }
        public string SNP_NAME_IX_LAST { get; set; }
        public string SNP_NAME_IX_FI { get; set; }
        public string SNP_NAME_IX_MI { get; set; }
        public string MST_INTAKE_DATE { get; set; }
        public string PBR_TYPE { get; set; }


        public string MST_SOURCE { get; set; }
        public string LPB_DATE { get; set; }
        public string LPB_AMOUNT { get; set; }
        public string LPB_REDUCE_ELIG { get; set; }
        public string LPB_BENEFIT_LEVEL { get; set; }
        public string PMR_BENIFIT_LEVEL { get; set; }
        public string PMR_PAYMENT_HOW { get; set; }
        public string PMR_AUTH_NUM { get; set; }
        public string PMR_DEL_DATE { get; set; }
        public string PMR_INVOICE_DATE { get; set; }
        public string PMR_AMOUNT_GAL { get; set; }
        public string PMR_VENDOR_PP { get; set; }
        public string PMR_MOR_PP { get; set; }
        public string PMR_AMOUNT_DELIVERY { get; set; }
        public string PMR_AMOUNT_STARTUP { get; set; }
        public string PMR_AMOUNT_MISC { get; set; }
        public string PMR_ACCOUNT { get; set; }
        public string CASEVDD1_INDEXBY { get; set; }
        public string CASEVDD_NAME { get; set; }
        public string PMR_AMOUNT_DOL { get; set; }
        public string PBR_PARAM { get; set; }
        public string ReduceElig { get; set; }



        //public string PBR_FED_PAYS { get; set; }
        //public string PBR_ST_PAYS { get; set; }
        //public string PBR_GALLONS { get; set; }
        //public string PBR_FED_PAY_DOL { get; set; }
        //public string PBR_ST_PAY_DOL { get; set; }
        //public string PBR_CHAP_PAYS { get; set; }
        //public string PBR_FUND { get; set; }
        //public string PBR_SCROLL { get; set; }
        //public string PBR_AUTO_P_CODE { get; set; }
        //public string PBR_AUTO_P_VENDOR { get; set; }
        //public string PBR_CHECK_NO { get; set; }
        //public string PBR_CHECK_DATE { get; set; }
        //public string PBR_VOUCH_NO { get; set; }
        //public string PBR_PAY_NUMBER { get; set; }
        //public string PBR_VENDOR { get; set; }
        //public string PBR_REP_TYPE { get; set; }
        //public string PBR_DOL_LIMIT { get; set; }


        //public string VDN_ADD_OPERATOR { get; set; }
        //public string Mode { get; set; }

        public string Rec_Type { get; set; }

        #endregion

    }

    public class TMSBCHCTEntity
    {
        #region Constructors

        public TMSBCHCTEntity()
        {
            PMR_AGENCY = string.Empty;
            PMR_DEPT = string.Empty;
            PMR_PROGRAM = string.Empty;
            PMR_YEAR = string.Empty;
            MST_APP_NO = string.Empty;

            PMR_BATCH_NO = string.Empty;
            PMR_VENDOR = string.Empty;
            MST_HN = string.Empty;
            SNP_NAME_IX_LAST = string.Empty;
            SNP_NAME_IX_FI = string.Empty;
            SNP_NAME_IX_MI = string.Empty;
            MST_APT = string.Empty;
            MST_FLR = string.Empty;
            MST_SUFFIX = string.Empty;
            MST_STREET = string.Empty;
            LPB_AMOUNT = string.Empty;
            MST_CITY = string.Empty;
            MST_STATE = string.Empty;
            MST_ZIP = string.Empty;

            MST_ZIPPLUS = string.Empty;
            PMR_AUTH_NUM = string.Empty;
            MST_PHONE = string.Empty;
            PMR_AMOUNT_DOL = string.Empty;
            PMR_CHK_DATE = string.Empty;
            PMR_CHECK_NO = string.Empty;
            PMR_PRIMARY_CODE = string.Empty;
            LPB_AMOUNT = string.Empty;
            LPV_ACCOUNT_NO = string.Empty;
            LPV_PRIMARY_CODE = string.Empty;
            PMR_ACCOUNT = string.Empty;
            PMR_PAYMENT_NO = string.Empty;

            MST_CAT_ELIG = string.Empty;
            MST_INCOME_TYPES = string.Empty;
            MST_NCASHBEN = string.Empty;
            SNP_HEALTH_CODES = string.Empty;
            Cert_Level = string.Empty;

        }

        public TMSBCHCTEntity(bool Initalize)
        {
            if (Initalize)
            {
                PMR_AGENCY = null;
                PMR_DEPT = null;
                PMR_PROGRAM = null;
                PMR_YEAR = null;
                MST_APP_NO = null;

                PMR_BATCH_NO = null;
                PMR_VENDOR = null;
                MST_HN = null;
                SNP_NAME_IX_LAST = null;
                SNP_NAME_IX_FI = null;
                SNP_NAME_IX_MI = null;
                MST_APT = null;
                MST_FLR = null;
                MST_SUFFIX = null;
                MST_STREET = null;
                LPB_AMOUNT = null;
                MST_CITY = null;
                MST_STATE = null;
                MST_ZIP = null;

                MST_ZIPPLUS = null;
                PMR_AUTH_NUM = null;
                MST_PHONE = null;
                PMR_AMOUNT_DOL = null;
                PMR_CHK_DATE = null;
                PMR_CHECK_NO = null;
                PMR_PRIMARY_CODE = null;
                LPB_AMOUNT = null;
                LPV_ACCOUNT_NO = null;
                LPV_PRIMARY_CODE = null;
                PMR_ACCOUNT = null;
                PMR_PAYMENT_NO = null;

                MST_CAT_ELIG = null;
                MST_INCOME_TYPES = null;
                MST_NCASHBEN = null;
                SNP_HEALTH_CODES = null;
                Cert_Level = null;
            }
        }

        public TMSBCHCTEntity(DataRow Row)
        {
            if (Row != null)
            {
                PMR_AGENCY = Row["PMR_AGENCY"].ToString().Trim();
                PMR_DEPT = Row["PMR_DEPT"].ToString().Trim();
                PMR_PROGRAM = Row["PMR_PROGRAM"].ToString().Trim();
                PMR_YEAR = Row["PMR_YEAR"].ToString().Trim();
                MST_APP_NO = Row["MST_APP_NO"].ToString().Trim();

                PMR_BATCH_NO = Row["PMR_BATCH_NO"].ToString().Trim();
                PMR_VENDOR = Row["PMR_VENDOR"].ToString().Trim();
                MST_HN = Row["MST_HN"].ToString().Trim();
                SNP_NAME_IX_LAST = Row["SNP_NAME_IX_LAST"].ToString().Trim();
                SNP_NAME_IX_FI = Row["SNP_NAME_IX_FI"].ToString().Trim();
                SNP_NAME_IX_MI = Row["SNP_NAME_IX_MI"].ToString().Trim();
                MST_APT = Row["MST_APT"].ToString().Trim();
                MST_FLR = Row["MST_FLR"].ToString().Trim();
                MST_SUFFIX = Row["MST_SUFFIX"].ToString().Trim();
                MST_STREET = Row["MST_STREET"].ToString().Trim();
                LPB_AMOUNT = Row["LPB_AMOUNT"].ToString().Trim();
                MST_CITY = Row["MST_CITY"].ToString().Trim();
                MST_STATE = Row["MST_STATE"].ToString().Trim();
                MST_ZIP = Row["MST_ZIP"].ToString().Trim();

                MST_ZIPPLUS = Row["MST_ZIPPLUS"].ToString().Trim();
                PMR_AUTH_NUM = Row["PMR_AUTH_NUM"].ToString().Trim();
                MST_PHONE = Row["MST_PHONE"].ToString().Trim();
                PMR_AMOUNT_DOL = Row["PMR_AMOUNT_DOL"].ToString().Trim();
                PMR_CHK_DATE = Row["PMR_CHK_DATE"].ToString().Trim();
                PMR_CHECK_NO = Row["PMR_CHECK_NO"].ToString().Trim();
                PMR_PRIMARY_CODE = Row["PMR_PRIMARY_CODE"].ToString().Trim();
                LPB_AMOUNT = Row["LPB_AMOUNT"].ToString().Trim();
                LPV_ACCOUNT_NO = Row["LPV_ACCOUNT_NO"].ToString().Trim();
                LPV_PRIMARY_CODE = Row["LPV_PRIMARY_CODE"].ToString().Trim();
                PMR_ACCOUNT = Row["PMR_ACCOUNT"].ToString().Trim();
                PMR_PAYMENT_NO = Row["PMR_PAYMENT_NO"].ToString().Trim();
                //CASEVDD1_INDEXBY = Row["CASEVDD1_INDEXBY"].ToString().Trim();
                //CASEVDD_NAME = Row["CASEVDD_NAME"].ToString().Trim();

                Cert_Level=Row["LPB_BENEFIT_LEVEL"].ToString().Trim();

                MST_CAT_ELIG = Row["MST_CAT_ELIG"].ToString().Trim();
                MST_INCOME_TYPES = Row["MST_INCOME_TYPES"].ToString().Trim();
                MST_NCASHBEN = Row["MST_NCASHBEN"].ToString().Trim();
                SNP_HEALTH_CODES = Row["SNP_HEALTH_CODES"].ToString().Trim();

            }
        }



        #endregion

        #region Properties

        public string PMR_AGENCY { get; set; }
        public string PMR_DEPT { get; set; }
        public string PMR_PROGRAM { get; set; }
        public string PMR_YEAR { get; set; }
        public string MST_APP_NO { get; set; }

        public string SNP_NAME_IX_LAST { get; set; }
        public string SNP_NAME_IX_FI { get; set; }
        public string SNP_NAME_IX_MI { get; set; }
        public string MST_HN { get; set; }
        public string MST_APT { get; set; }
        public string MST_FLR { get; set; }
        public string MST_SUFFIX { get; set; }
        public string MST_STREET { get; set; }


        public string MST_CITY { get; set; }
        public string MST_STATE { get; set; }
        public string MST_ZIP { get; set; }
        public string MST_ZIPPLUS { get; set; }
        public string MST_PHONE { get; set; }
        public string PMR_VENDOR { get; set; }
        public string PMR_AMOUNT_DOL { get; set; }
        public string PMR_BATCH_NO { get; set; }
        public string PMR_CHK_DATE { get; set; }
        public string PMR_CHECK_NO { get; set; }
        public string PMR_AUTH_NUM { get; set; }
        public string PMR_PRIMARY_CODE { get; set; }
        public string PMR_ACCOUNT { get; set; }
        public string LPB_AMOUNT { get; set; }
        public string LPV_ACCOUNT_NO { get; set; }
        public string LPV_PRIMARY_CODE { get; set; }
        public string PMR_PAYMENT_NO { get; set; }
        //public string CASEVDD1_INDEXBY { get; set; }
        //public string CASEVDD_NAME { get; set; }

        public string MST_CAT_ELIG { get; set; }
        public string MST_INCOME_TYPES { get; set; }
        public string MST_NCASHBEN { get; set; }
        public string SNP_HEALTH_CODES { get; set; }

        public string Cert_Level { get; set; }

        public string Rec_Type { get; set; }

        #endregion

    }

    public class TMSB0004Entity
    {
        #region Constructors

        public TMSB0004Entity()
        {
            AGENCY = string.Empty;
            DEPT = string.Empty;
            PROGRAM = string.Empty;
            YEAR = string.Empty;
            APP_NO = string.Empty;
            PMR_REC_SEQ = string.Empty;
            PMR_VENDOR_SEQ = string.Empty;
            FName = string.Empty;
            MName = string.Empty;
            LName = string.Empty;
            AuthNum = string.Empty;
            Area = string.Empty;
            Phone = string.Empty;
            Hno = string.Empty;
            Street = string.Empty;
            Suffix = string.Empty;
            Apt = string.Empty;
            Flr = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;
            CASEVDD1_INDEXBY = string.Empty;
            Site = string.Empty;
            CASEVDD_NAME = string.Empty;
            PMR_VENDOR = string.Empty;
            PMR_NOTIFY_DATE = string.Empty;
            PMR_NOTIFY_SEQ = string.Empty;
            PMR_AUTH_AMOUNT1 = string.Empty;
            PMR_AUTH_AMOUNT2 = string.Empty;
            PMR_AUTH_AMOUNT3 = string.Empty;
            PMR_MOR_DIFFERENTIAL = string.Empty;
            PMR_MOR_PP = string.Empty;
            PMR_PAYMENT_FOR = string.Empty;
            PMR_BS = string.Empty;
            PMR_SDWDC = string.Empty;
            CASEVDD_ADDR1 = string.Empty;
            CASEVDD_ADDR2 = string.Empty;
            CASEVDD_ADDR3 = string.Empty;
            CASEVDD_CITY = string.Empty;
            CASEVDD_STATE = string.Empty;
            CASEVDD_ZIP = string.Empty;
            CASEVDD_FAX = string.Empty;
            CASEVDD_TELEPHONE = string.Empty;
            CASEVDD_EMAIL=string.Empty;
            CASEVDD1_BULK_CODE = string.Empty;
            ACR_NAME = string.Empty;
            PMR_AUTH_DATE = string.Empty;
            PMR_INVOICE_DATE = string.Empty;
        }

        public TMSB0004Entity(bool Initalize)
        {
            if (Initalize)
            {
                AGENCY = null;
                DEPT = null;
                PROGRAM = null;
                YEAR = null;
                PMR_REC_SEQ = null;
                PMR_VENDOR_SEQ = null;
                APP_NO = null;
                FName = null;
                MName = null;
                LName = null;
                AuthNum = null;
                Area = null;
                Phone = null;
                Hno = null;
                Street = null;
                Suffix = null;
                Apt = null;
                Flr = null;
                City = null;
                State = null;
                Zip = null;
                CASEVDD1_INDEXBY = null;
                Site = null;
                CASEVDD_NAME = null;
                PMR_VENDOR = null;
                PMR_NOTIFY_DATE = null;
                PMR_NOTIFY_SEQ = null;
                PMR_AUTH_AMOUNT1 = null;
                PMR_AUTH_AMOUNT2 = null;
                PMR_AUTH_AMOUNT3 = null;
                PMR_MOR_DIFFERENTIAL = null;
                PMR_MOR_PP = null;
                PMR_PAYMENT_FOR = null;
                PMR_BS = null;
                PMR_SDWDC = null;
                CASEVDD_ADDR1 = null;
                CASEVDD_ADDR2 = null;
                CASEVDD_ADDR3 = null;
                CASEVDD_CITY = null;
                CASEVDD_STATE = null;
                CASEVDD_ZIP = null;
                CASEVDD_FAX = null;
                CASEVDD_TELEPHONE = null;
                CASEVDD_EMAIL = null;
                CASEVDD1_BULK_CODE = null;
                ACR_NAME = null;
                PMR_AUTH_DATE = null;
                PMR_INVOICE_DATE = null;

            }
        }

        public TMSB0004Entity(DataRow Row)
        {
            if (Row != null)
            {
                AGENCY = Row["PMR_AGENCY"].ToString().Trim();
                DEPT = Row["PMR_DEPT"].ToString().Trim();
                PROGRAM = Row["PMR_PROGRAM"].ToString().Trim();
                YEAR = Row["PMR_YEAR"].ToString().Trim();
                APP_NO = Row["PMR_APPL_NO"].ToString().Trim();
                PMR_REC_SEQ = Row["PMR_REC_SEQ"].ToString().Trim();
                PMR_VENDOR_SEQ = Row["PMR_VENDOR_SEQ"].ToString().Trim();
                FName = Row["SNP_NAME_IX_FI"].ToString().Trim();
                MName = Row["SNP_NAME_IX_MI"].ToString().Trim();
                LName = Row["SNP_NAME_IX_LAST"].ToString().Trim();
                AuthNum = Row["PMR_AUTH_NUM"].ToString().Trim();
                Area = Row["MST_AREA"].ToString().Trim();
                Phone = Row["MST_PHONE"].ToString().Trim();
                Hno = Row["MST_HN"].ToString().Trim();
                Street = Row["MST_STREET"].ToString().Trim();
                Suffix = Row["MST_SUFFIX"].ToString().Trim();
                Apt = Row["MST_APT"].ToString().Trim();
                Flr = Row["MST_FLR"].ToString().Trim();
                City = Row["MST_CITY"].ToString().Trim();
                State = Row["MST_STATE"].ToString().Trim();
                Zip = Row["MST_ZIP"].ToString().Trim();
                CASEVDD1_INDEXBY = Row["CASEVDD1_INDEXBY"].ToString().Trim();
                Site = Row["MST_SITE"].ToString().Trim();
                CASEVDD_NAME = Row["CASEVDD_NAME"].ToString().Trim();
                PMR_VENDOR = Row["PMR_VENDOR"].ToString().Trim();
                PMR_NOTIFY_DATE = Row["PMR_NOTIFY_DATE"].ToString().Trim();
                PMR_NOTIFY_SEQ = Row["PMR_NOTIFY_SEQ"].ToString().Trim();
                PMR_AUTH_AMOUNT1 = Row["PMR_AUTH_AMOUNT1"].ToString().Trim();
                PMR_AUTH_AMOUNT2 = Row["PMR_AUTH_AMOUNT2"].ToString().Trim();
                PMR_MOR_DIFFERENTIAL = Row["PMR_MOR_DIFFERENTIAL"].ToString().Trim();
                PMR_MOR_PP = Row["PMR_MOR_PP"].ToString().Trim();
                PMR_PAYMENT_FOR = Row["PMR_PAYMENT_FOR"].ToString().Trim();
                PMR_BS = Row["PMR_BS"].ToString().Trim();
                PMR_SDWDC = Row["PMR_SDWDC"].ToString().Trim();
                CASEVDD_ADDR1 = Row["CASEVDD_ADDR1"].ToString().Trim();
                CASEVDD_ADDR2 = Row["CASEVDD_ADDR2"].ToString().Trim();
                CASEVDD_ADDR3 = Row["CASEVDD_ADDR3"].ToString().Trim();
                CASEVDD_CITY = Row["CASEVDD_CITY"].ToString().Trim();
                CASEVDD_STATE = Row["CASEVDD_STATE"].ToString().Trim();
                CASEVDD_ZIP = Row["CASEVDD_ZIP"].ToString().Trim();
                CASEVDD_FAX = Row["CASEVDD_FAX"].ToString().Trim();
                CASEVDD_TELEPHONE = Row["CASEVDD_TELEPHONE"].ToString().Trim();
                CASEVDD_EMAIL = Row["CASEVDD_EMAIL"].ToString().Trim();
                CASEVDD1_BULK_CODE = Row["CASEVDD1_BULK_CODE"].ToString().Trim();
                ACR_NAME = Row["ACR_NAME"].ToString().Trim();
                PMR_AUTH_DATE = Row["PMR_AUTH_DATE"].ToString().Trim();

                PMR_AUTH_AMOUNT3 = Row["PMR_AUTH_AMOUNT3"].ToString().Trim();

                PMR_INVOICE_DATE = Row["PMR_INVOICE_DATE"].ToString().Trim();
            }
        }

        #endregion

        #region Properties

        public string AGENCY { get; set; }
        public string DEPT { get; set; }
        public string PROGRAM { get; set; }
        public string YEAR { get; set; }
        public string APP_NO { get; set; }
        public string PMR_REC_SEQ { get; set; }
        public string PMR_VENDOR_SEQ { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string AuthNum { get; set; }
        public string Area { get; set; }
        public string Phone { get; set; }
        public string Hno { get; set; }
        public string Street { get; set; }
        public string Suffix { get; set; }
        public string Apt { get; set; }
        public string Flr { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string CASEVDD1_INDEXBY { get; set; }
        public string Site { get; set; }
        public string CASEVDD_NAME { get; set; }
        public string PMR_VENDOR { get; set; }
        public string PMR_NOTIFY_DATE { get; set; }
        public string PMR_NOTIFY_SEQ { get; set; }
        public string PMR_AUTH_AMOUNT1 { get; set; }
        public string PMR_AUTH_AMOUNT2 { get; set; }

        public string PMR_INVOICE_DATE { get; set; }

        public string PMR_AUTH_AMOUNT3 { get; set; }

        public string PMR_MOR_DIFFERENTIAL { get; set; }
        public string PMR_MOR_PP { get; set; }
        public string PMR_PAYMENT_FOR { get; set; }
        public string PMR_BS { get; set; }
        public string PMR_SDWDC { get; set; }
        public string CASEVDD_ADDR1 { get; set; }
        public string CASEVDD_ADDR2 { get; set; }
        public string CASEVDD_ADDR3 { get; set; }
        public string CASEVDD_CITY { get; set; }
        public string CASEVDD_STATE { get; set; }
        public string CASEVDD_ZIP { get; set; }
        public string CASEVDD_FAX { get; set; }
        public string CASEVDD_TELEPHONE { get; set; }

        public string CASEVDD_EMAIL { get; set; }
        public string CASEVDD1_BULK_CODE { get; set; }
        public string ACR_NAME { get; set; }

        public string PMR_AUTH_DATE { get; set; }

        //public string PMR_SDWDC { get; set; }
        //public string Mode { get; set; }

        public string Rec_Type { get; set; }

        #endregion

    }

    public class LPWBUNDLEEntity
    {
        #region Constructors

        public LPWBUNDLEEntity()
        {
            Rec_Type =
           LPWB_AGENCY =
           LPWB_DEPT =
           LPWB_PROGRAM =
           LPWB_YEAR =
           LPWB_BUNDLE =
           LPWB_BUNDLE_TYPE =
           LPWB_STATUS =
           LPWB_DATE_OPENED =
           LPWB_APP_COUNT =
           LPWB_TOT_AMOUNT =
           LPWB_FUND =
           LPWB_VENDOR =
           LPWB_CHK_NO =
           LPWB_CHK_DATE =
           LPWB_VOUCH_NO =
           LPWB_PAY_NUMBER =
           LPWB_PARAMS =
           LPWB_DATE_LSTC =
           LPWB_LSTC_OPERATOR =
           LPWB_COUNT = Mode = string.Empty;

        }

        public LPWBUNDLEEntity(bool Initalize)
        {
            if (Initalize)
            {
                Rec_Type =
           LPWB_AGENCY =
           LPWB_DEPT =
           LPWB_PROGRAM =
           LPWB_YEAR =
           LPWB_BUNDLE =
           LPWB_BUNDLE_TYPE =
           LPWB_STATUS =
           LPWB_DATE_OPENED =
           LPWB_APP_COUNT =
           LPWB_TOT_AMOUNT =
           LPWB_FUND =
           LPWB_VENDOR =
           LPWB_CHK_NO =
           LPWB_CHK_DATE =
           LPWB_VOUCH_NO =
           LPWB_PAY_NUMBER =
           LPWB_PARAMS =
           LPWB_DATE_LSTC =
           LPWB_LSTC_OPERATOR =
           LPWB_COUNT = Mode = null;

            }
        }

        public LPWBUNDLEEntity(DataRow Row)
        {
            if (Row != null)
            {
                Rec_Type = "U";
                LPWB_AGENCY = Row["LPWB_AGENCY"].ToString().Trim();
                LPWB_DEPT = Row["LPWB_DEPT"].ToString().Trim();
                LPWB_PROGRAM = Row["LPWB_PROGRAM"].ToString().Trim();
                LPWB_YEAR = Row["LPWB_YEAR"].ToString().Trim();
                LPWB_BUNDLE = Row["LPWB_BUNDLE"].ToString().Trim();
                LPWB_BUNDLE_TYPE = Row["LPWB_BUNDLE_TYPE"].ToString().Trim();
                LPWB_STATUS = Row["LPWB_STATUS"].ToString().Trim();
                LPWB_DATE_OPENED = Row["LPWB_DATE_OPENED"].ToString().Trim();
                LPWB_APP_COUNT = Row["LPWB_APP_COUNT"].ToString().Trim();
                LPWB_TOT_AMOUNT = Row["LPWB_TOT_AMOUNT"].ToString().Trim();
                LPWB_FUND = Row["LPWB_FUND"].ToString().Trim();
                LPWB_VENDOR = Row["LPWB_VENDOR"].ToString().Trim();
                LPWB_CHK_NO = Row["LPWB_CHK_NO"].ToString().Trim();
                LPWB_CHK_DATE = Row["LPWB_CHK_DATE"].ToString().Trim();
                LPWB_VOUCH_NO = Row["LPWB_VOUCH_NO"].ToString().Trim();
                LPWB_PAY_NUMBER = Row["LPWB_PAY_NUMBER"].ToString().Trim();
                LPWB_PARAMS = Row["LPWB_PARAMS"].ToString().Trim();
                LPWB_DATE_LSTC = Row["LPWB_DATE_LSTC"].ToString().Trim();
                LPWB_LSTC_OPERATOR = Row["LPWB_LSTC_OPERATOR"].ToString().Trim();
                LPWB_COUNT = Row["LPWB_COUNT"].ToString().Trim();

            }
        }



        #endregion

        #region Properties

        public string LPWB_AGENCY { get; set; }
        public string LPWB_DEPT { get; set; }
        public string LPWB_PROGRAM { get; set; }
        public string LPWB_YEAR { get; set; }
        public string LPWB_BUNDLE { get; set; }
        public string LPWB_BUNDLE_TYPE { get; set; }
        public string LPWB_STATUS { get; set; }
        public string LPWB_DATE_OPENED { get; set; }
        public string LPWB_APP_COUNT { get; set; }
        public string LPWB_TOT_AMOUNT { get; set; }
        public string LPWB_FUND { get; set; }
        public string LPWB_VENDOR { get; set; }
        public string LPWB_CHK_NO { get; set; }
        public string LPWB_CHK_DATE { get; set; }
        public string LPWB_VOUCH_NO { get; set; }
        public string LPWB_PAY_NUMBER { get; set; }
        public string LPWB_PARAMS { get; set; }
        public string LPWB_DATE_LSTC { get; set; }
        public string LPWB_LSTC_OPERATOR { get; set; }
        public string LPWB_COUNT { get; set; }

        public string Rec_Type { get; set; }
        public string Mode { get; set; }

        #endregion

    }

}
