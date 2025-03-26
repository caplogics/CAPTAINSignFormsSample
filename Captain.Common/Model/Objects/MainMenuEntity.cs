using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Captain.Common.Model.Objects
{
    public class MainMenuEntity
    {

       #region Constructors

        public MainMenuEntity()
        {
            Agy = 
            Dept = 
            Prog = 
            Year = 
            AppNo =
            App_Or_Mem = 
            Ssn = 
            Fname = 
            Lname = 
            Mname = 
            Relation =
            ClientID = 
            Phone = 
            Hno = 
            Street = 
            City =
            State = 
            Zip = 
            CaseType = 
            Snp_Key =
            FamSeq = 
            Mem_Status = 
            DOB = 
            LSTC_Date =
            SNP_EXCLUDE = 
            Site = string.Empty;             
        }

        public MainMenuEntity(bool Intialize)
        {
            if (Intialize)
            {
                Agy =
                Dept =
                Prog =
                Year =
                AppNo =
                App_Or_Mem =
                Ssn =
                Fname =
                Lname =
                Mname =
                Relation =
                ClientID =
                Phone =
                Hno =
                Street =
                City =
                State =
                Zip =
                CaseType =
                Snp_Key =
                FamSeq =
                Mem_Status =
                DOB =
                LSTC_Date =
                SNP_EXCLUDE =
                Site = null;
            }


        }

        public MainMenuEntity(MainMenuEntity Entity)
        {
            if (Entity != null)
            {
                Agy = Entity.Agy;
                Dept = Entity.Dept;
                Prog = Entity.Prog;
                Year = Entity.Year;
                AppNo = Entity.AppNo;
                App_Or_Mem = Entity.App_Or_Mem;
                Ssn = Entity.Ssn;
                Fname = Entity.Fname;
                Lname = Entity.Lname;
                Mname = Entity.Mname;
                Relation = Entity.Relation;
                ClientID = Entity.ClientID;
                Phone = Entity.Phone; 
                Hno = Entity.Hno;
                Street = Entity.Street;
                City = Entity.City;
                State = Entity.State;
                Zip = Entity.Zip;
                CaseType = Entity.CaseType;
                Snp_Key = Entity.Snp_Key; 
                FamSeq = Entity.FamSeq;
                Mem_Status = Entity.Mem_Status;
                DOB = Entity.DOB;
                LSTC_Date = Entity.LSTC_Date;
                SNP_EXCLUDE = Entity.SNP_EXCLUDE;
                Site = Entity.Site; 
            }
        }


        public MainMenuEntity(DataRow row)
        {
            if (row != null)
            {
                Agy = row["Agency"].ToString().Trim();
                Dept = row["Dept"].ToString().Trim();
                Prog = row["Prog"].ToString().Trim();
                Year = row["SnpYear"].ToString().Trim();
                AppNo = row["AppNo"].ToString().Trim().Substring(0, 8);
                App_Or_Mem = row["AppNo"].ToString().Trim().Substring(10, 1);
                Ssn = row["Ssn"].ToString().Trim();
                Fname = row["Fname"].ToString().Trim();
                Lname = row["Lname"].ToString().Trim();
                Mname = row["Mname"].ToString().Trim();
                Relation = row["Mem_Code"].ToString().Trim();
                ClientID = row["ClientID"].ToString().Trim();
                Phone = row["Phone"].ToString().Trim();
                Hno = row["Hno"].ToString().Trim();
                Street = row["Street"].ToString().Trim();
                City = row["City"].ToString().Trim();
                State = row["State1"].ToString().Trim();
                Zip = row["Zip"].ToString().Trim();
                CaseType = row["CaseType"].ToString().Trim();
                Snp_Key = row["RecKey"].ToString().Trim();
                FamSeq = row["RecFamSeq"].ToString().Trim();

                Mem_Status = row["AppStatus"].ToString().Trim();
                DOB = row["DOB"].ToString().Trim();
                LSTC_Date = row["SNP_DATE_LSTC"].ToString().Trim();
                SNP_EXCLUDE = row["SNP_EXCLUDE"].ToString().Trim();
                Site = row["MST_SITE"].ToString().Trim();
            }
        }

        public MainMenuEntity(DataRow row, string Column_Name)
        {
            if (Column_Name == "Original")
            {
                Agy = row["MST_AGENCY"].ToString().Trim();
                Dept = row["MST_DEPT"].ToString().Trim();
                Prog = row["MST_PROGRAM"].ToString().Trim();
                Year = row["MST_YEAR"].ToString().Trim();
                AppNo = row["MST_APP_NO"].ToString().Trim().Substring(0, 8);
                //App_Or_Mem = row["AppNo"].ToString().Trim().Substring(10, 1);
                Ssn = row["SNP_SSNO"].ToString().Trim();
                Fname = row["SNP_NAME_IX_FI"].ToString().Trim();
                Lname = row["SNP_NAME_IX_LAST"].ToString().Trim();
                Mname = row["SNP_NAME_IX_MI"].ToString().Trim();
                Relation = row["SNP_MEMBER_CODE"].ToString().Trim();
                ClientID = row["SNP_CLIENT_ID"].ToString().Trim();
                Phone = row["MST_AREA"].ToString().Trim() + "-" + row["MST_PHONE"].ToString().Trim();
                Hno = row["MST_HN"].ToString().Trim();
                Street = row["MST_STREET"].ToString().Trim();
                City = row["MST_CITY"].ToString().Trim();
                State = row["MST_STATE"].ToString().Trim();
                Zip = row["MST_ZIP"].ToString().Trim();
                CaseType = row["MST_CASE_TYPE"].ToString().Trim();
                //Snp_Key = row["RecKey"].ToString().Trim();
                FamSeq = row["SNP_FAMILY_SEQ"].ToString().Trim();

                //Mem_Status = row["AppStatus"].ToString().Trim();
                DOB = row["SNP_ALT_BDATE"].ToString().Trim();
                DOB_NA = row["SNP_DOB_NA"].ToString().Trim();
                LSTC_Date = row["SNP_DATE_LSTC"].ToString().Trim();
                SNP_EXCLUDE = row["SNP_EXCLUDE"].ToString().Trim();
                Site = row["MST_SITE"].ToString().Trim();
            }
        }    

        #endregion

        #region Properties
        public string Agy { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string Year { get; set; }
        public string AppNo { get; set; }
        public string App_Or_Mem { get; set; }
        public string Ssn { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Mname { get; set; }
        public string Relation { get; set; }
        public string ClientID { get; set; }
        public string Phone { get; set; }
        public string Hno { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string CaseType { get; set; }
        public string Snp_Key { get; set; }

        public string FamSeq { get; set; }
        public string Mem_Status { get; set; }
        public string DOB { get; set; }
        public string DOB_NA { get; set; }
        public string LSTC_Date { get; set; }
        public string SNP_EXCLUDE { get; set; }
        public string Site { get; set; }

        #endregion

    }

    public class MainMenu_MSTSNP_Entity
    {

        #region Constructors

        public MainMenu_MSTSNP_Entity()
        {
            Agy =
            Dept =
            Prog =
            Year =
            AppNo =
            App_Or_Mem =
            Ssn =
            Fname =
            Lname =
            Mname =
            Relation =
            ClientID =
            Phone =
            Hno =
            Street =
            City =
            State =
            Zip =
            CaseType =
            Snp_Key =
            FamSeq =
            Mem_Status =
            DOB =
            LSTC_Date =
            SNP_EXCLUDE =
            Site = string.Empty;
        }

        public MainMenu_MSTSNP_Entity(bool Intialize)
        {
            if (Intialize)
            {
                Agy =
                Dept =
                Prog =
                Year =
                AppNo =
                App_Or_Mem =
                Ssn =
                Fname =
                Lname =
                Mname =
                Relation =
                ClientID =
                Phone =
                Hno =
                Street =
                City =
                State =
                Zip =
                CaseType =
                Snp_Key =
                FamSeq =
                Mem_Status =
                DOB =
                LSTC_Date =
                SNP_EXCLUDE =
                Site = null;
            }


        }

        public MainMenu_MSTSNP_Entity(MainMenu_MSTSNP_Entity Entity)
        {
            if (Entity != null)
            {
                Agy = Entity.Agy;
                Dept = Entity.Dept;
                Prog = Entity.Prog;
                Year = Entity.Year;
                AppNo = Entity.AppNo;
                App_Or_Mem = Entity.App_Or_Mem;
                Ssn = Entity.Ssn;
                Fname = Entity.Fname;
                Lname = Entity.Lname;
                Mname = Entity.Mname;
                Relation = Entity.Relation;
                ClientID = Entity.ClientID;
                Phone = Entity.Phone;
                Hno = Entity.Hno;
                Street = Entity.Street;
                City = Entity.City;
                State = Entity.State;
                Zip = Entity.Zip;
                CaseType = Entity.CaseType;
                Snp_Key = Entity.Snp_Key;
                FamSeq = Entity.FamSeq;
                Mem_Status = Entity.Mem_Status;
                DOB = Entity.DOB;
                LSTC_Date = Entity.LSTC_Date;
                SNP_EXCLUDE = Entity.SNP_EXCLUDE;
                Site = Entity.Site;
            }
        }


        public MainMenu_MSTSNP_Entity(DataRow row)
        {
            if (row != null)
            {
                Agy = row["Agency"].ToString().Trim();
                Dept = row["Dept"].ToString().Trim();
                Prog = row["Prog"].ToString().Trim();
                Year = row["SnpYear"].ToString().Trim();
                AppNo = row["AppNo"].ToString().Trim().Substring(0, 8);
                App_Or_Mem = row["AppNo"].ToString().Trim().Substring(10, 1);
                Ssn = row["Ssn"].ToString().Trim();
                Fname = row["Fname"].ToString().Trim();
                Lname = row["Lname"].ToString().Trim();
                Mname = row["Mname"].ToString().Trim();
                Relation = row["Mem_Code"].ToString().Trim();
                ClientID = row["ClientID"].ToString().Trim();
                Phone = row["Phone"].ToString().Trim();
                Hno = row["Hno"].ToString().Trim();
                Street = row["Street"].ToString().Trim();
                City = row["City"].ToString().Trim();
                State = row["State1"].ToString().Trim();
                Zip = row["Zip"].ToString().Trim();
                CaseType = row["CaseType"].ToString().Trim();
                Snp_Key = row["RecKey"].ToString().Trim();
                FamSeq = row["RecFamSeq"].ToString().Trim();

                Mem_Status = row["AppStatus"].ToString().Trim();
                DOB = row["DOB"].ToString().Trim();
                LSTC_Date = row["SNP_DATE_LSTC"].ToString().Trim();
                SNP_EXCLUDE = row["SNP_EXCLUDE"].ToString().Trim();
                Site = row["MST_SITE"].ToString().Trim();
            }
        }



        #endregion

        #region Properties
        public string Agy { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string Year { get; set; }
        public string AppNo { get; set; }
        public string App_Or_Mem { get; set; }
        public string Ssn { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Mname { get; set; }
        public string Relation { get; set; }
        public string ClientID { get; set; }
        public string Phone { get; set; }
        public string Hno { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string CaseType { get; set; }
        public string Snp_Key { get; set; }

        public string FamSeq { get; set; }
        public string Mem_Status { get; set; }
        public string DOB { get; set; }
        public string LSTC_Date { get; set; }
        public string SNP_EXCLUDE { get; set; }
        public string Site { get; set; }

        #endregion

    }


    public class MSTSNP_Entity
    {

        #region Constructors

        public MSTSNP_Entity()
        {
            Agy = string.Empty;
            Dept = string.Empty;
            Prog = string.Empty;
            Year = string.Empty;
            AppNo = string.Empty;
            //App_Or_Mem = string.Empty;
            Ssn = string.Empty;
            Fname = string.Empty;
            Lname = string.Empty;
            Mname = string.Empty;
            Relation = string.Empty;
            //ClientID = string.Empty;
            Phone = string.Empty;
            Hno = string.Empty;
            Street = string.Empty;
            Suffix = string.Empty;
            Apt = string.Empty;
            Flr = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;
            ZipPlus = string.Empty;
            //CaseType = string.Empty;
            //Snp_Key = string.Empty;
            FamSeq = string.Empty;
            //Mem_Status = string.Empty;
            DOB = string.Empty;
            //LSTC_Date = string.Empty;
            //SNP_EXCLUDE = string.Empty;
            Site = string.Empty;
            //Language = string.Empty;
            //LanguageOt = string.Empty;
            Elig_Date = string.Empty;
            Race = string.Empty;
            Ethinic = string.Empty;
            Sex = string.Empty;
            //Age = string.Empty;
            Classfication = string.Empty;
            MealElig = string.Empty;
            //ChldMST_Repeat = string.Empty;
            Rank1 = string.Empty;
            Rank2 = string.Empty;
            Rank3 = string.Empty;
            Rank4 = string.Empty;
            Rank5 = string.Empty;
            Rank6 = string.Empty;
            Empl_Phone = string.Empty;
            Fam_Income = string.Empty;
            Race_Desc = string.Empty;
            Ethnic_Desc = string.Empty;
            Lang_Desc = string.Empty;
            LangOth_Desc = string.Empty;
            ChildAge = string.Empty;
            AgeinMnths = string.Empty;

            CellPhone = string.Empty;

            ChldMSTApp = string.Empty;
            Repeat = string.Empty;
            Disability = string.Empty;
            Disability_type = string.Empty;
            Diagnosis_Date = string.Empty;
            Transport = string.Empty;
            Pickup = string.Empty;
            DropOff = string.Empty;
            CHLDMST_FUND = string.Empty;
            G1_LName = string.Empty;
            G1_FName = string.Empty;
            //G1_MName = string.Empty;
            G2_LName = string.Empty;
            G2_FName = string.Empty;
            //G2_MName = string.Empty;
            G1_SSN = string.Empty;
            G2_SSN = string.Empty;
            G1_DOB = string.Empty;
            G2_DOB = string.Empty;
            G1_Age = string.Empty;
            G2_Age = string.Empty;
            G1_Education = string.Empty;
            G2_Education = string.Empty;
            G1_Phone = string.Empty;
            G2_Phone = string.Empty;
            G1_Occupation = string.Empty;
            G2_Occupation = string.Empty;
            G1_Seq = string.Empty;
            G2_Seq = string.Empty;
            Allergy = string.Empty;
            DietRestrct = string.Empty;
            Medications = string.Empty;
            MedConds = string.Empty;
            HHConcerns = string.Empty;
            DevlConcerns = string.Empty;
            CasecondApp = string.Empty;

            Enrl_Site = string.Empty;
            Enrl_Room = string.Empty;
            Enrl_AMPM = string.Empty;
            Enrl_Status = string.Empty;
            Enrl_Date = string.Empty;
            Enrl_Fund = string.Empty;
            Enrl_fund_date = string.Empty;
            date_Enrl = string.Empty;
        }

        public MSTSNP_Entity(bool Intialize)
        {
            if (Intialize)
            {
                Agy =null;
                Dept =null;
                Prog =null;
                Year =null;
                AppNo =null;
                //App_Or_Mem =null;
                Ssn =null;
                Fname =null;
                Lname =null;
                Mname =null;
                Relation =null;
                //ClientID =null;
                Phone =null;
                Hno =null;
                Street =null;
                Suffix = null;
                Apt = null;
                Flr = null;
                City =null;
                State =null;
                Zip =null;
                ZipPlus = null;
                //CaseType =null;
                //Snp_Key =null;
                FamSeq = null;
                //Mem_Status =null;
                DOB =null;
                //LSTC_Date =null;
                //SNP_EXCLUDE =null;
                Site =null;
                //Language =null;
                //LanguageOt = null;
                Elig_Date =null;
                Race = null;
                Ethinic = null;
                Sex =null;
                //Age =null;
                Classfication = null;
                MealElig = null;
                //ChldMST_Repeat = null;
                Rank1 = null;
                Rank2 = null;
                Rank3 = null;
                Rank4 = null;
                Rank5 = null;
                Rank6 = null;
                Empl_Phone = null;
                Fam_Income = null;
                Race_Desc = null;
                Ethnic_Desc = null;
                Lang_Desc = null;
                LangOth_Desc = null;
                ChildAge = null;
                AgeinMnths = null;

                CellPhone = null;

                ChldMSTApp = null;
                Repeat = null;
                Disability = null;
                Disability_type = null;
                Diagnosis_Date = null;
                Transport = null;
                Pickup = null;
                DropOff = null;
                CHLDMST_FUND = null;
                G1_LName = null;
                G1_FName = null;
                //G1_MName = null;
                G2_LName = null;
                G2_FName = null;
                //G2_MName = null;
                G1_SSN = null;
                G2_SSN = null;
                G1_DOB = null;
                G2_DOB = null;
                G1_Age = null;
                G2_Age = null;
                G1_Education = null;
                G2_Education = null;
                G1_Phone = null;
                G2_Phone = null;
                G1_Occupation = null;
                G2_Occupation = null;
                G1_Seq = null;
                G2_Seq = null;
                Allergy = null;
                DietRestrct = null;
                Medications = null;
                MedConds = null;
                HHConcerns = null;
                DevlConcerns = null;
                CasecondApp = null;

                Enrl_Site = null;
                Enrl_Room = null;
                Enrl_AMPM = null;
                Enrl_Status = null;
                Enrl_Date = null;
                Enrl_Fund = null;
                Enrl_fund_date = null;
                date_Enrl = null;
            }


        }

        public MSTSNP_Entity(MSTSNP_Entity Entity)
        {
            if (Entity != null)
            {
                Agy = Entity.Agy;
                Dept = Entity.Dept;
                Prog = Entity.Prog;
                Year = Entity.Year;
                AppNo = Entity.AppNo;
                //App_Or_Mem = Entity.App_Or_Mem;
                Ssn = Entity.Ssn;
                Fname = Entity.Fname;
                Lname = Entity.Lname;
                Mname = Entity.Mname;
                Relation = Entity.Relation;
                //ClientID = Entity.ClientID;
                Phone = Entity.Phone;
                Hno = Entity.Hno;
                Street = Entity.Street;
                Suffix = Entity.Suffix;
                Apt = Entity.Apt;
                Flr = Entity.Flr;
                City = Entity.City;
                State = Entity.State;
                Zip = Entity.Zip;
                ZipPlus = Entity.ZipPlus;
                //CaseType = Entity.CaseType;
                //Snp_Key = Entity.Snp_Key;
                FamSeq = Entity.FamSeq;
                //Mem_Status = Entity.Mem_Status;
                DOB = Entity.DOB;
                //LSTC_Date = Entity.LSTC_Date;
                //SNP_EXCLUDE = Entity.SNP_EXCLUDE;
                Site = Entity.Site;
                //Language = Entity.Language;
                //LanguageOt = Entity.LanguageOt;
                Elig_Date = Entity.Elig_Date;
                Race = Entity.Race;
                Ethinic = Entity.Ethinic;
                Sex = Entity.Sex;
                //Age = Entity.Age;
                Classfication = Entity.Classfication;
                MealElig = Entity.MealElig;
                //ChldMST_Repeat = Entity.ChldMST_Repeat;
                Rank1 = Entity.Rank1;
                Rank2 = Entity.Rank2;
                Rank3 = Entity.Rank3;
                Rank4 = Entity.Rank4;
                Rank5 = Entity.Rank5;
                Rank6 = Entity.Rank6;
                Empl_Phone = Entity.Empl_Phone;
                Fam_Income = Entity.Fam_Income;
                Race_Desc = Entity.Race_Desc;
                Ethnic_Desc = Entity.Ethnic_Desc;
                Lang_Desc = Entity.Lang_Desc;
                LangOth_Desc = Entity.LangOth_Desc;
                ChildAge = Entity.ChildAge;
                AgeinMnths = Entity.AgeinMnths;

                CellPhone = Entity.CellPhone;

                ChldMSTApp = Entity.ChldMSTApp;
                Repeat = Entity.Repeat;
                Disability = Entity.Disability;
                Disability_type = Entity.Disability_type;
                Diagnosis_Date = Entity.Diagnosis_Date;
                Transport = Entity.Transport;
                Pickup = Entity.Pickup;
                DropOff = Entity.DropOff;
                CHLDMST_FUND = Entity.CHLDMST_FUND;
                G1_LName = Entity.G1_LName;
                G1_FName = Entity.G1_FName;
                //G1_MName = Entity.G1_MName;
                G2_LName = Entity.G2_LName;
                G2_FName = Entity.G2_FName;
                //G2_MName = Entity.G2_MName;
                G1_SSN = Entity.G1_SSN;
                G2_SSN = Entity.G2_SSN;
                G1_DOB = Entity.G1_DOB;
                G2_DOB = Entity.G2_DOB;
                G1_Age = Entity.G1_Age;
                G2_Age = Entity.G2_Age;
                G1_Education = Entity.G1_Education;
                G2_Education = Entity.G2_Education;
                G1_Phone = Entity.G1_Phone;
                G2_Phone = Entity.G2_Phone;
                G1_Occupation = Entity.G1_Occupation;
                G2_Occupation = Entity.G2_Occupation;
                G1_Seq = Entity.G1_Seq;
                G2_Seq = Entity.G2_Seq;
                CasecondApp = Entity.CasecondApp;
                Allergy = Entity.Allergy;
                DietRestrct = Entity.DietRestrct;
                Medications = Entity.Medications;
                MedConds = Entity.MedConds;
                HHConcerns = Entity.HHConcerns;
                DevlConcerns = Entity.DevlConcerns;

                Enrl_Site = Entity.Enrl_Site;
                Enrl_Room = Entity.Enrl_Room;
                Enrl_AMPM = Entity.Enrl_AMPM;
                Enrl_Status = Entity.Enrl_Status;
                Enrl_Date = Entity.Enrl_Date;
                Enrl_Fund = Entity.Enrl_Fund;
                Enrl_fund_date = Entity.Enrl_fund_date;
                date_Enrl = Entity.date_Enrl;
            }
        }


        public MSTSNP_Entity(DataRow row)
        {
            if (row != null)
            {
                Agy = row["Agency"].ToString().Trim();
                Dept = row["Dept"].ToString().Trim();
                Prog = row["Prog"].ToString().Trim();
                Year = row["SnpYear"].ToString().Trim();
                AppNo = row["AppNo"].ToString().Trim();
                //App_Or_Mem = row["AppNo"].ToString().Trim().Substring(10, 1);
                Ssn = row["Ssn"].ToString().Trim();
                Fname = row["Fname"].ToString().Trim();
                Lname = row["Lname"].ToString().Trim();
                Mname = row["Mname"].ToString().Trim();
                Relation = row["Mem_Code"].ToString().Trim();
                //ClientID = row["ClientID"].ToString().Trim();
                Phone = row["Phone"].ToString().Trim();
                Hno = row["Hno"].ToString().Trim();
                Street = row["Street"].ToString().Trim();
                Suffix = row["SUFFIX"].ToString().Trim();
                Apt = row["Apt"].ToString().Trim();
                Flr = row["Flr"].ToString().Trim();
                City = row["City"].ToString().Trim();
                State = row["State1"].ToString().Trim();
                Zip = row["Zip"].ToString().Trim();
                ZipPlus = row["Zip_Plus"].ToString().Trim();
                //CaseType = row["CaseType"].ToString().Trim();
                //Snp_Key = row["RecKey"].ToString().Trim();
                FamSeq = row["RecFamSeq"].ToString().Trim();

                //Mem_Status = row["AppStatus"].ToString().Trim();
                DOB = row["DOB"].ToString().Trim();
                //LSTC_Date = row["SNP_DATE_LSTC"].ToString().Trim();
                //SNP_EXCLUDE = row["SNP_EXCLUDE"].ToString().Trim();
                Site = row["MST_SITE"].ToString().Trim();
                //Language=row["MST_LANGUAGE"].ToString().Trim();
                //LanguageOt = row["MST_LANGUAGE_OT"].ToString().Trim();
                Elig_Date = row["MST_ELIG_DATE"].ToString().Trim();
                Race = row["SNP_RACE"].ToString().Trim();
                Ethinic = row["SNP_ETHNIC"].ToString().Trim();
                Sex = row["SNP_SEX"].ToString().Trim();
                //Age=row["SNP_AGE"].ToString().Trim();
                Classfication = row["MST_CLASSIFICATION"].ToString().Trim();
                MealElig = row["MST_MEAL_ELIG"].ToString().Trim();
                //ChldMST_Repeat = row["CHLDMST_CHLD_REPEAT"].ToString().Trim();
                Rank1 = row["MST_RANK1"].ToString().Trim();
                Rank2 = row["MST_RANK2"].ToString().Trim();
                Rank3 = row["MST_RANK3"].ToString().Trim();
                Rank4 = row["MST_RANK4"].ToString().Trim();
                Rank5 = row["MST_RANK5"].ToString().Trim();
                Rank6 = row["MST_RANK6"].ToString().Trim();
                Empl_Phone = row["SNP_EMPL_PHONE"].ToString().Trim();
                Fam_Income = row["MST_FAM_INCOME"].ToString().Trim();
                Race_Desc = row["RACE_DESC"].ToString().Trim();
                Ethnic_Desc = row["ETHNIC_DESC"].ToString().Trim();
                Lang_Desc = row["LANG_DESC"].ToString().Trim();
                LangOth_Desc = row["LANGOTH_DESC"].ToString().Trim();
                ChildAge = row["ChildAge"].ToString().Trim();
                AgeinMnths = row["AgeinMnths"].ToString().Trim();

                CellPhone = row["MST_CELL_PHONE"].ToString().Trim();
                EMail = row["MST_EMAIL"].ToString().Trim();

                ChldMSTApp = row["CHLDMST_APP_NO"].ToString().Trim();
                Repeat = row["CHLDMST_CHLD_REPEAT"].ToString().Trim();
                Disability = row["CHLDMST_DISABILITY"].ToString().Trim();
                Disability_type = row["CHLDMST_DISABILITY_TYPE"].ToString().Trim();
                Diagnosis_Date = row["CHLDMST_DIAGNOSIS_DATE"].ToString().Trim();
                Transport = row["CHLDMST_TRANSPORT"].ToString().Trim();
                Pickup = row["CHLDMST_PICKUP"].ToString().Trim();
                DropOff = row["CHLDMST_DROPOFF"].ToString().Trim();
                CHLDMST_FUND = row["CHLDMST_FUND_SOURCE"].ToString().Trim();
                G1_LName = row["G1_LName"].ToString().Trim();
                G1_FName = row["G1_FName"].ToString().Trim();
                
                G2_LName = row["G2_LName"].ToString().Trim();
                G2_FName = row["G2_FName"].ToString().Trim();
                //G2_MName = row["G2_MName"].ToString().Trim();
                G1_SSN = row["G1_SSN"].ToString().Trim();
                G2_SSN = row["G2_SSN"].ToString().Trim();
                G1_DOB = row["G1_DOB"].ToString().Trim();
                G2_DOB = row["G2_DOB"].ToString().Trim();
                G1_Age = row["G1_Age"].ToString().Trim();
                G2_Age = row["G2_Age"].ToString().Trim();
                G1_Education = row["G1_Education"].ToString().Trim();
                G2_Education = row["G2_Education"].ToString().Trim();
                G1_Phone = row["G1_Emp_Phone"].ToString().Trim();
                G2_Phone = row["G2_Emp_Phone"].ToString().Trim();
                G1_Occupation = row["G1_Occupation"].ToString().Trim();
                G2_Occupation = row["G2_Occupation"].ToString().Trim();
                G1_Seq = row["G1_Seq"].ToString().Trim();
                G2_Seq = row["G2_Seq"].ToString().Trim();
                CasecondApp = row["CASECOND_APP_NO"].ToString().Trim();
                Allergy = row["CASECOND_ALLERGY"].ToString().Trim();
                DietRestrct = row["CASECOND_DIET_RESTRCT"].ToString().Trim();
                Medications = row["CASECOND_MEDICATIONS"].ToString().Trim();
                MedConds = row["CASECOND_MED_CONDS"].ToString().Trim();
                HHConcerns = row["CASECOND_HH_CONCRNS"].ToString().Trim();
                DevlConcerns = row["CASECOND_DEVL_CONCRNS"].ToString().Trim();

                Enrl_Site = row["ESITE"].ToString().Trim();
                Enrl_Room = row["EROOM"].ToString().Trim();
                Enrl_AMPM = row["EAMPM"].ToString().Trim();
                Enrl_Status = row["ESTATUS"].ToString().Trim();
                Enrl_Date = row["ENRL_DATE"].ToString().Trim();
                Enrl_Fund = row["EFUND"].ToString().Trim();
                Enrl_fund_date = row["EFUND_DATE"].ToString().Trim();
                date_Enrl = row["DATE_ENRL"].ToString().Trim();

            }
        }



        #endregion

        #region Properties
        public string Agy { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string Year { get; set; }
        public string AppNo { get; set; }
        //public string App_Or_Mem { get; set; }
        public string Ssn { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Mname { get; set; }
        public string Relation { get; set; }
        //public string ClientID { get; set; }
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
        //public string CaseType { get; set; }
        //public string Snp_Key { get; set; }
        public string CellPhone { get; set; }
        public string EMail { get; set; }

        public string FamSeq { get; set; }
        //public string Mem_Status { get; set; }
        public string DOB { get; set; }
        //public string LSTC_Date { get; set; }
        //public string SNP_EXCLUDE { get; set; }
        public string Site { get; set; }
        //public string Language { get; set; }
        //public string LanguageOt { get; set; }
        public string Elig_Date { get; set; }
        public string Race { get; set; }
        public string Ethinic { get; set; }
        public string Sex { get; set; }
        //public string Age { get; set; }
        public string Classfication { get; set; }
        public string MealElig { get; set; }
        //public string ChldMST_Repeat { get; set; }
        public string Rank1 { get; set; }
        public string Rank2 { get; set; }
        public string Rank3 { get; set; }
        public string Rank4 { get; set; }
        public string Rank5 { get; set; }
        public string Rank6 { get; set; }
        public string Empl_Phone { get; set; }
        public string Fam_Income { get; set; }
        public string Race_Desc { get; set; }
        public string Ethnic_Desc { get; set; }
        public string Lang_Desc { get; set; }
        public string LangOth_Desc { get; set; }
        public string ChildAge { get; set; }
        public string AgeinMnths { get; set; }

        public string ChldMSTApp { get; set; }
        public string Repeat { get; set; }
        public string Diagnosis_Date { get; set; }
        public string Transport { get; set; }
        public string Pickup { get; set; }
        public string DropOff { get; set; }
        public string CHLDMST_FUND { get; set; }
        public string G1_LName { get; set; }
        public string G1_FName { get; set; }
        public string Disability { get; set; }
        public string G2_LName { get; set; }
        public string G2_FName { get; set; }
        public string Disability_type { get; set; }
        public string G1_SSN { get; set; }
        public string G2_SSN { get; set; }
        public string G1_DOB { get; set; }
        public string G2_DOB { get; set; }
        public string G1_Age { get; set; }
        public string G2_Age { get; set; }
        public string G1_Education { get; set; }
        public string G2_Education { get; set; }
        public string G1_Phone { get; set; }
        public string G2_Phone { get; set; }
        public string G1_Occupation { get; set; }
        public string G2_Occupation { get; set; }
        public string G1_Seq { get; set; }
        public string G2_Seq { get; set; }
        public string CasecondApp { get; set; }
        public string Allergy { get; set; }
        public string DietRestrct { get; set; }
        public string Medications { get; set; }
        public string MedConds { get; set; }
        public string HHConcerns { get; set; }
        public string DevlConcerns { get; set; }

        public string Enrl_Site { get; set; }
        public string Enrl_Room { get; set; }
        public string Enrl_AMPM { get; set; }
        public string Enrl_Status { get; set; }
        public string Enrl_Date { get; set; }
        public string Enrl_Fund { get; set; }
        public string Enrl_fund_date { get; set; }
        public string date_Enrl { get; set; }

        
        #endregion

    }


    public class HSSB2115Entity
    {

        #region Constructors

        public HSSB2115Entity()
        {
            Agy = string.Empty;
            Dept = string.Empty;
            Prog = string.Empty;
            Year = string.Empty;
            AppNo = string.Empty;
            Fname = string.Empty;
            Lname = string.Empty;
            Mname = string.Empty;
            Relation = string.Empty;
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
            DOB = string.Empty;
            Site = string.Empty;
            Elig_Date = string.Empty;
            Sex = string.Empty;
            Age = string.Empty;
            Months = string.Empty;
            Classfication = string.Empty;
            MealElig = string.Empty;
            Rank1 = string.Empty;
            Rank2 = string.Empty;
            Rank3 = string.Empty;
            Rank4 = string.Empty;
            Rank5 = string.Empty;
            Rank6 = string.Empty;
            Empl_Phone = string.Empty;
            Fam_Income = string.Empty;
            Language = string.Empty;
           

            ChldMSTApp = string.Empty;
            Repeat = string.Empty;
            Alt_FUND = string.Empty;
            Disability_type = string.Empty;
            Transport = string.Empty;
            CHLDMST_FUND = string.Empty;
            G1_LName = string.Empty;
            G1_FName = string.Empty;
            G1_SSN = string.Empty;
            G1_DOB = string.Empty;
            G1_Age = string.Empty;
            G1_Education = string.Empty;
            G1_Phone = string.Empty;
            G1_Occupation = string.Empty;
            G1_Seq = string.Empty;
            Allergy = string.Empty;
            DietRestrct = string.Empty;
            Medications = string.Empty;
            MedConds = string.Empty;
            HHConcerns = string.Empty;
            DevlConcerns = string.Empty;
            CasecondApp = string.Empty;

            Rank = string.Empty;
            Family_count = string.Empty;
            CaseNotes = string.Empty;
            EnrlStatus = string.Empty;
            EnrlFundHie = string.Empty;
            EnrlSite = string.Empty;

            Disable = string.Empty;
            
        }

        public HSSB2115Entity(bool Intialize)
        {
            if (Intialize)
            {
                Agy = null;
                Dept = null;
                Prog = null;
                Year = null;
                AppNo = null;
                Fname = null;
                Lname = null;
                Mname = null;
                Relation = null;
                Phone = null;
                Hno = null;
                Street = null;
                Suffix = null;
                Apt = null;
                Flr = null;
                City = null;
                State = null;
                Zip = null;
                DOB = null;
                Age =null;
                Months = null;
                Site = null;
                Elig_Date = null;
                Sex = null;
                Classfication = null;
                MealElig = null;
                Rank1 = null;
                Rank2 = null;
                Rank3 = null;
                Rank4 = null;
                Rank5 = null;
                Rank6 = null;
                Empl_Phone = null;
                Fam_Income = null;
                Language = null;
                

                ChldMSTApp = null;
                Repeat = null;
                Alt_FUND = null;
                Disability_type = null;
                Transport = null;
                CHLDMST_FUND = null;
                G1_LName = null;
                G1_FName = null;
                G1_SSN = null;
                G1_DOB = null;
                G1_Age = null;
                G1_Education = null;
                G1_Phone = null;
                G1_Occupation = null;
                G1_Seq = null;
                Allergy = null;
                DietRestrct = null;
                Medications = null;
                MedConds = null;
                HHConcerns = null;
                DevlConcerns = null;
                CasecondApp = null;

                Rank = null;
                Family_count = null;
                EnrlAppNo = null;
                EnrlStatus = null;
                EnrlFundHie = null;
                MSTCatElig = null;
                EnrlSite = null;

                Disable = null;
                
            }


        }

        public HSSB2115Entity(HSSB2115Entity Entity)
        {
            if (Entity != null)
            {
                Agy = Entity.Agy;
                Dept = Entity.Dept;
                Prog = Entity.Prog;
                Year = Entity.Year;
                AppNo = Entity.AppNo;
                Fname = Entity.Fname;
                Lname = Entity.Lname;
                Mname = Entity.Mname;
                Relation = Entity.Relation;
                Phone = Entity.Phone;
                Hno = Entity.Hno;
                Street = Entity.Street;
                Suffix = Entity.Suffix;
                Apt = Entity.Apt;
                Flr = Entity.Flr;
                City = Entity.City;
                State = Entity.State;
                Zip = Entity.Zip;
                IntakeDate = Entity.IntakeDate;
                DOB = Entity.DOB;
                Age = Entity.Age;
                Months = Entity.Months;
                Site = Entity.Site;
                Language = Entity.Language;
                Elig_Date = Entity.Elig_Date;
                Sex = Entity.Sex;
                Classfication = Entity.Classfication;
                MealElig = Entity.MealElig;
                Rank1 = Entity.Rank1;
                Rank2 = Entity.Rank2;
                Rank3 = Entity.Rank3;
                Rank4 = Entity.Rank4;
                Rank5 = Entity.Rank5;
                Rank6 = Entity.Rank6;
                Empl_Phone = Entity.Empl_Phone;
                Fam_Income = Entity.Fam_Income;
                
                ChldMSTApp = Entity.ChldMSTApp;
                Repeat = Entity.Repeat;
                CHLDMST_FUND = Entity.CHLDMST_FUND;
                Alt_FUND = Entity.Alt_FUND;
                Disability_type = Entity.Disability_type;
                Transport = Entity.Transport;
                
                G1_LName = Entity.G1_LName;
                G1_FName = Entity.G1_FName;
                G1_SSN = Entity.G1_SSN;
                G1_DOB = Entity.G1_DOB;
                G1_Age = Entity.G1_Age;
                G1_Education = Entity.G1_Education;
                G1_Phone = Entity.G1_Phone;
                G1_Occupation = Entity.G1_Occupation;
                G1_Seq = Entity.G1_Seq;
                
                CasecondApp = Entity.CasecondApp;
                Allergy = Entity.Allergy;
                DietRestrct = Entity.DietRestrct;
                Medications = Entity.Medications;
                MedConds = Entity.MedConds;
                HHConcerns = Entity.HHConcerns;
                DevlConcerns = Entity.DevlConcerns;

                Rank = Entity.Rank;
                Family_count = Entity.Family_count;
                EnrlAppNo = Entity.CaseNotes;
                EnrlStatus = Entity.EnrlStatus;
                EnrlFundHie = Entity.EnrlFundHie;
                MSTCatElig = Entity.MSTCatElig;

                EnrlSite = Entity.EnrlSite;

                Disable = Entity.Disable;


            }
        }


        public HSSB2115Entity(DataRow row)
        {
            if (row != null)
            {
                Agy = row["Agency"].ToString().Trim();
                Dept = row["Dept"].ToString().Trim();
                Prog = row["Prog"].ToString().Trim();
                Year = row["SnpYear"].ToString().Trim();
                AppNo = row["AppNo"].ToString().Trim();
                Fname = row["Fname"].ToString().Trim();
                Lname = row["Lname"].ToString().Trim();
                Mname = row["Mname"].ToString().Trim();
                Relation = row["Mem_Code"].ToString().Trim();
                Phone = row["Phone"].ToString().Trim();
                Hno = row["Hno"].ToString().Trim();
                Street = row["Street"].ToString().Trim();
                Suffix = row["Suffix"].ToString().Trim();
                Apt = row["Apt"].ToString().Trim();
                Flr = row["Flr"].ToString().Trim();
                City = row["City"].ToString().Trim();
                State = row["State1"].ToString().Trim();
                Zip = row["Zip"].ToString().Trim();
                IntakeDate = row["MST_INTAKE_DATE"].ToString().Trim();
                DOB = row["DOB"].ToString().Trim();
                Age = row["AGE"].ToString().Trim();
                Months = row["MONTHS"].ToString().Trim();
                Site = row["MST_SITE"].ToString().Trim();
                Language = row["LANG_DESC"].ToString().Trim();
                Elig_Date = row["MST_ELIG_DATE"].ToString().Trim();
                Sex = row["SNP_SEX"].ToString().Trim();
                Classfication = row["MST_CLASSIFICATION"].ToString().Trim();
                MealElig = row["MST_MEAL_ELIG"].ToString().Trim();
                Rank1 = row["MST_RANK1"].ToString().Trim();
                Rank2 = row["MST_RANK2"].ToString().Trim();
                Rank3 = row["MST_RANK3"].ToString().Trim();
                Rank4 = row["MST_RANK4"].ToString().Trim();
                Rank5 = row["MST_RANK5"].ToString().Trim();
                Rank6 = row["MST_RANK6"].ToString().Trim();
                Empl_Phone = row["SNP_EMPL_PHONE"].ToString().Trim();
                Fam_Income = row["MST_PROG_INCOME"].ToString().Trim();
                
                ChldMSTApp = row["CHLDMST_APP_NO"].ToString().Trim();
                Repeat = row["CHLDMST_CHLD_REPEAT"].ToString().Trim();
                CHLDMST_FUND = row["CHLDMST_FUND_SOURCE"].ToString().Trim();
                Alt_FUND = row["CHLDMST_ALT_FUND_SRC"].ToString().Trim();
                Disability_type = row["CHLDMST_DISABILITY_TYPE"].ToString().Trim();
                Transport = row["Transport"].ToString().Trim();
                ClassPrefer = row["CHLDMST_CLASS_PREFER"].ToString().Trim();

                G1_LName = row["G1_LName"].ToString().Trim();
                G1_FName = row["G1_FName"].ToString().Trim();
                G1_SSN = row["G1_SSN"].ToString().Trim();
                G1_DOB = row["G1_DOB"].ToString().Trim();
                G1_Age = row["G1_Age"].ToString().Trim();
                G1_Education = row["G1_Education"].ToString().Trim();
                G1_Phone = row["G1_Emp_Phone"].ToString().Trim();
                G1_Occupation = row["G1_Occupation"].ToString().Trim();
                G1_Seq = row["G1_Seq"].ToString().Trim();
                
                CasecondApp = row["CASECOND_APP_NO"].ToString().Trim();
                Allergy = row["CASECOND_ALLERGY"].ToString().Trim();
                DietRestrct = row["CASECOND_DIET_RESTRCT"].ToString().Trim();
                Medications = row["CASECOND_MEDICATIONS"].ToString().Trim();
                MedConds = row["CASECOND_MED_CONDS"].ToString().Trim();
                HHConcerns = row["CASECOND_HH_CONCRNS"].ToString().Trim();
                DevlConcerns = row["CASECOND_DEVL_CONCRNS"].ToString().Trim();

                Rank = row["TOT_RANK"].ToString().Trim();
                Family_count = row["FamilyCount"].ToString().Trim();
                EnrlAppNo = row["ENRL_APP_NO"].ToString().Trim();
                EnrlStatus = row["ENRL_STATUS"].ToString().Trim();
                EnrlFundHie = row["ENRL_FUND_HIE"].ToString().Trim();
                MSTCatElig = row["MST_CAT_ELIG"].ToString().Trim();

                EnrlSite = row["ENRL_SITE"].ToString().Trim();

                Disable = row["SNP_DISABLE"].ToString().Trim();
            }
        }



        #endregion

        #region Properties
        public string Agy { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string Year { get; set; }
        public string AppNo { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Mname { get; set; }
        public string Relation { get; set; }
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
        public string DOB { get; set; }
        public string Age { get; set; }
        public string Months { get; set; }
        public string Site { get; set; }
        public string Language { get; set; }
        public string Elig_Date { get; set; }
        public string Sex { get; set; }
        public string Classfication { get; set; }
        public string MealElig { get; set; }
        public string Rank1 { get; set; }
        public string Rank2 { get; set; }
        public string Rank3 { get; set; }
        public string Rank4 { get; set; }
        public string Rank5 { get; set; }
        public string Rank6 { get; set; }
        public string Empl_Phone { get; set; }
        public string Fam_Income { get; set; }
        
        public string ChldMSTApp { get; set; }
        public string Repeat { get; set; }
        public string ClassPrefer { get; set; }
        public string CHLDMST_FUND { get; set; }
        public string Disability_type { get; set; }
        public string Alt_FUND { get; set; }
        public string Transport { get; set; }
        
        public string G1_LName { get; set; }
        public string G1_FName { get; set; }
        public string G1_SSN { get; set; }
        public string G1_DOB { get; set; }
        public string G1_Age { get; set; }
        public string G1_Education { get; set; }
        public string G1_Phone { get; set; }
        public string G1_Occupation { get; set; }
        public string G1_Seq { get; set; }
        public string CasecondApp { get; set; }
        public string Allergy { get; set; }
        public string DietRestrct { get; set; }
        public string Medications { get; set; }
        public string MedConds { get; set; }
        public string HHConcerns { get; set; }
        public string DevlConcerns { get; set; }

        public string Rank { get; set; }
        public string Family_count { get; set; }
        public string CaseNotes { get; set; }
        public string EnrlStatus { get; set; }
        public string EnrlFundHie { get; set; }
        public string EnrlSite { get; set; }
        public string EnrlAppNo { get; set; }
        public string MSTCatElig { get; set; }

        public string Disable { get; set; }

        #endregion

    }

}
