using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DevExpress.XtraSpellChecker;

namespace Captain.Common.Model.Objects
{
   public class TmsApcnEntity
    {
        #region Constructors

       public TmsApcnEntity()
       {

           Agency = string.Empty;
           Dept = string.Empty;
           Program = string.Empty;
           Year = string.Empty;
           Location = string.Empty;
           Date = string.Empty;
           Type = string.Empty;
           SlotsPerPeriod = string.Empty;
           Mins = string.Empty;
           TemplateAvailble = string.Empty;
           DayTable = string.Empty;
           PeriodTable = string.Empty;                  
           DateLstc = string.Empty;
           LstcOperation = string.Empty;
           DateAdd = string.Empty;
           AddOperator = string.Empty;
           Description = string.Empty;
           Active = string.Empty;   
       }


       public TmsApcnEntity(DataRow row, string strType)
       {
           if (row != null)
           {
               if (strType.ToUpper() == "SITE")
               {
                   Location = row["SITE_NUMBER"].ToString();
                   Description = row["SITE_NAME"].ToString();
                   Active = row["SITE_ACTIVE"].ToString();
               }
               else
               {
                   Agency = row["TMSAPCN_AGENCY"].ToString();
                   Dept = row["TMSAPCN_DEPT"].ToString();
                   Program = row["TMSAPCN_PROGRAM"].ToString();
                   Year = row["TMSAPCN_YEAR"].ToString();
                   Location = row["TMSAPCN_LOCATION"].ToString();
                   Date = row["TMSAPCN_DATE"].ToString();
                   Type = row["TMSAPCN_TYPE"].ToString();
                   SlotsPerPeriod = row["TMSAPCN_SLOTS_PER_PERIOD"].ToString();
                   Mins = row["TMSAPCN_MINS"].ToString();
                   TemplateAvailble = row["TMSAPCN_TEMPLATE_AVAILABLE"].ToString();
                   DayTable = row["TMSAPCN_DAY_TABLE"].ToString();
                   PeriodTable = row["TMSAPCN_PERIOD_TABLE"].ToString();
                   DateLstc = row["TMSAPCN_DATE_LSTC"].ToString();
                   LstcOperation = row["TMSAPCN_LSTC_OPERATOR"].ToString();
                   DateAdd = row["TMSAPCN_DATE_ADD"].ToString();
                   AddOperator = row["TMSAPCN_ADD_OPERATOR"].ToString();
                   if (strType == string.Empty)
                   {
                       Description = row["TmsApcn_Description"].ToString();
                   }

               }
           }

       }
       #endregion

       #region Properties

       public string Agency { get; set; }
       public string Dept { get; set; }
       public string Program { get; set; }
       public string Year { get; set; }
       public string Location { get; set; }
       public string Date { get; set; }
       public string Type { get; set; }
       public string SlotsPerPeriod { get; set; }
       public string Mins { get; set; }
       public string TemplateAvailble { get; set; }
       public string DayTable { get; set; }
       public string PeriodTable { get; set; }     
       public string DateLstc { get; set; }
       public string LstcOperation { get; set; }
       public string DateAdd { get; set; }
       public string AddOperator { get; set; }
       public string Description { get; set; }
       public string Mode { get; set; }
       public string Active { get; set; }
       #endregion

    }

    public class APPTEMPLATESEntity
    {
        #region Constructors

        public APPTEMPLATESEntity()
        {

            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            Location = string.Empty;
            FDate = string.Empty;
            TDate = string.Empty;
            Type = string.Empty;
            SlotsPerPeriod = string.Empty;
            Mins = string.Empty;
            TemplateAvailble = string.Empty;
            DayTable = string.Empty;
            PeriodTable = string.Empty;
            DateLstc = string.Empty;
            LstcOperation = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            Description = string.Empty;
            Active = string.Empty;
            OpenSlots = string.Empty;
        }


        public APPTEMPLATESEntity(DataRow row, string strType)
        {
            if (row != null)
            {
                if (strType.ToUpper() == "SITE")
                {
                    Location = row["SITE_NUMBER"].ToString();
                    Description = row["SITE_NAME"].ToString();
                    Active = row["SITE_ACTIVE"].ToString();
                }
                else if(strType.ToUpper() == "DATES")
                {
                    Agency = row["APPTEMPL_AGY"].ToString();
                    Dept = row["APPTEMPL_DEPT"].ToString();
                    Program = row["APPTEMPL_PROG"].ToString();
                    Year = row["APPTEMPL_YEAR"].ToString();
                    Location = row["APPTEMPL_SITE"].ToString();
                    FDate = row["APPTEMPL_FDATE"].ToString();
                    TDate = row["APPTEMPL_TDATE"].ToString();
                    Type = row["APPTEMPL_ID"].ToString();
                    SlotsPerPeriod = row["APPTEMPL_SLOTS"].ToString();
                    Mins = row["APPTEMPL_MINS"].ToString();
                    TemplateAvailble = row["APPTEMPL_OPEN"].ToString();
                    DayTable = row["APPTEMPL_DAYS"].ToString();
                    PeriodTable = row["APPTEMPL_PERIOD_TABLE"].ToString();
                    DateLstc = row["APPTEMPL_DATE_LSTC"].ToString();
                    LstcOperation = row["APPTEMPL_LSTC_OPERATOR"].ToString();
                    DateAdd = row["APPTEMPL_DATE_ADD"].ToString();
                    AddOperator = row["APPTEMPL_ADD_OPERATOR"].ToString();
                    Description = row["TmsApcn_Description"].ToString();
                    OpenSlots = row["OPENSLOTS"].ToString();

                }
                else
                {
                    Agency = row["APPTEMPL_AGY"].ToString();
                    Dept = row["APPTEMPL_DEPT"].ToString();
                    Program = row["APPTEMPL_PROG"].ToString();
                    Year = row["APPTEMPL_YEAR"].ToString();
                    Location = row["APPTEMPL_SITE"].ToString();
                    FDate = row["APPTEMPL_FDATE"].ToString();
                    TDate = row["APPTEMPL_TDATE"].ToString();
                    Type = row["APPTEMPL_ID"].ToString();
                    SlotsPerPeriod = row["APPTEMPL_SLOTS"].ToString();
                    Mins = row["APPTEMPL_MINS"].ToString();
                    TemplateAvailble = row["APPTEMPL_OPEN"].ToString();
                    DayTable = row["APPTEMPL_DAYS"].ToString();
                    PeriodTable = row["APPTEMPL_PERIOD_TABLE"].ToString();
                    DateLstc = row["APPTEMPL_DATE_LSTC"].ToString();
                    LstcOperation = row["APPTEMPL_LSTC_OPERATOR"].ToString();
                    DateAdd = row["APPTEMPL_DATE_ADD"].ToString();
                    AddOperator = row["APPTEMPL_ADD_OPERATOR"].ToString();
                    if (strType == string.Empty)
                    {
                        Description = row["TmsApcn_Description"].ToString();
                    }

                }
            }

        }
        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string Location { get; set; }
        public string FDate { get; set; }
        public string TDate { get; set; }
        public string Type { get; set; }
        public string SlotsPerPeriod { get; set; }
        public string Mins { get; set; }
        public string TemplateAvailble { get; set; }
        public string DayTable { get; set; }
        public string PeriodTable { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperation { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string Description { get; set; }
        public string Mode { get; set; }
        public string Active { get; set; }
        public string OpenSlots { get; set; }
        #endregion

    }

    public class TmsApptEntity
   {
       #region Constructors

       public TmsApptEntity()
       {

           Agency = string.Empty;
           Dept = string.Empty;
           Program = string.Empty;
           Year = string.Empty;
           Location = string.Empty;
           Date = string.Empty;
           Time = string.Empty;
           SlotNumber = string.Empty;
           SsNumber = string.Empty;
           TemplateDate = string.Empty;
           TemplateType = string.Empty;
           RecordType = string.Empty;
           Name = string.Empty;
           FirstName = string.Empty;
           //TelAreaCode = string.Empty;
           TelNumber = string.Empty;
           StNo = string.Empty;
           Street = string.Empty;
           Suffix = string.Empty;
           Apt = string.Empty;
           Floor = string.Empty;
           City = string.Empty;
           State = string.Empty;
           Zip1 = string.Empty;
           Zip2 = string.Empty;
           HeatSource = string.Empty;
           SourceIncome = string.Empty;

           ContactPerson = string.Empty;
           ContactDate = string.Empty;
           Sex = string.Empty;
           CellProvider = string.Empty;
           //CellAreaCode = string.Empty;
           CellNumber = string.Empty;
           CaseWorker = string.Empty;          
           DateLstc = string.Empty;
           LstcOperation = string.Empty;
           DateAdd = string.Empty;
           AddOperator = string.Empty;     
           EditTime = string.Empty;
           EditBy = string.Empty;
           Email = string.Empty;    
       }


       public TmsApptEntity(string date, string time, string slot_num, string reason, string Template_Type, string Time_Key)
        {
            Date = date;
            Time = time;
            SlotNumber = slot_num;
            Name = reason;
            RecordType = Template_Type;
            CaseWorker = Time_Key;
        }


       public TmsApptEntity(DataRow row, string strType)
       {
           if (row != null)
           {
               Agency = row["TMSAPPT_AGENCY"].ToString();
               Dept = row["TMSAPPT_DEPT"].ToString();
               Program = row["TMSAPPT_PROGRAM"].ToString();
               Year = row["TMSAPPT_YEAR"].ToString();
               Location = row["TMSAPPT_LOCATION"].ToString();
               Date = row["TMSAPPT_DATE"].ToString();
               Time = row["TMSAPPT_TIME"].ToString();
               SlotNumber = row["TMSAPPT_SLOT_NUMBER"].ToString();
               SsNumber = row["TMSAPPT_SS_NUMBER"].ToString();
               TemplateDate = row["TMSAPPT_TEMPLATE_DATE"].ToString();
               TemplateType = row["TMSAPPT_TEMPLATE_TYPE"].ToString();
               RecordType = row["TMSAPPT_RECORD_TYPE"].ToString();
               Name = row["TMSAPPT_NAME"].ToString();
               FirstName = row["TMSAPPT_FIRST_NAME"].ToString();
               //TelAreaCode = row["TMSAPPT_TEL_AREA_CODE"].ToString();
               TelNumber = row["TMSAPPT_TEL_NUMBER"].ToString();
               StNo = row["TMSAPPT_STNO"].ToString();
               Street = row["TMSAPPT_STREET"].ToString();
               Suffix = row["TMSAPPT_SUFFIX"].ToString();
               Apt = row["TMSAPPT_APT"].ToString();
               Floor = row["TMSAPPT_FLOOR"].ToString();
               City = row["TMSAPPT_CITY"].ToString();
               State = row["TMSAPPT_STATE"].ToString();
               Zip1 = row["TMSAPPT_ZIP1"].ToString();
               Zip2 = row["TMSAPPT_ZIP2"].ToString();
               HeatSource = row["TMSAPPT_HEAT_SOURCE"].ToString();
               SourceIncome = row["TMSAPPT_SOURCE_INCOME"].ToString();
               ContactPerson = row["TMSAPPT_CONTACT_PERSON"].ToString();
               ContactDate = row["TMSAPPT_CONTACT_DATE"].ToString();
               Sex = row["TMSAPPT_SEX"].ToString();
               CellProvider = row["TMSAPPT_CELL_PROVIDER"].ToString();
               //CellAreaCode = row["TMSAPPT_CELL_AREA_CODE"].ToString();
               CellNumber = row["TMSAPPT_CELL_NUMBER"].ToString();
               CaseWorker = row["TMSAPPT_CASE_WORKER"].ToString();
               DateLstc = row["TMSAPPT_DATE_LSTC"].ToString();
               LstcOperation = row["TMSAPPT_LSTC_OPERATOR"].ToString();           
               DateAdd = row["TMSAPPT_DATE_ADD"].ToString();
               AddOperator = row["TMSAPPT_ADD_OPERATOR"].ToString();

               EditTime = row["TMSAPPT_EDITIME"].ToString();
               EditBy = row["TMSAPPT_EDITBY"].ToString();
               Email = row["TMSAPPT_EMAIL"].ToString();
           }
       }

       public TmsApptEntity(TmsApptEntity Ent)
       {
           //if (row != null)
           {
               Agency = Ent.Agency;
               Dept = Ent.Dept;
               Program = Ent.Program;
               Year = Ent.Year;
               Location = Ent.Location;
               Date = Ent.Date;
               Time = Ent.Time;
               SlotNumber = Ent.SlotNumber;
               SsNumber = Ent.SsNumber;
               TemplateDate = Ent.TemplateDate;
               TemplateType = Ent.TemplateType;
               RecordType = Ent.RecordType;
               Name = Ent.Name;
               FirstName = Ent.FirstName;
               //TelAreaCode = row["TMSAPPT_TEL_AREA_CODE"].ToString();
               TelNumber = Ent.TelNumber;
               StNo = Ent.StNo;
               Street = Ent.Street;
               Suffix = Ent.Suffix;
               Apt = Ent.Apt;
               Floor = Ent.Floor;
               City = Ent.City;
               State = Ent.State;
               Zip1 = Ent.Zip1;
               Zip2 = Ent.Zip2;
               HeatSource = Ent.HeatSource;
               SourceIncome = Ent.SourceIncome;
               ContactPerson = Ent.ContactPerson;
               ContactDate = Ent.ContactDate;
               Sex = Ent.Sex;
               CellProvider = Ent.CellProvider;
               //CellAreaCode = Ent.;
               CellNumber = Ent.CellNumber;
               CaseWorker = Ent.CaseWorker;
               DateLstc = Ent.DateLstc;
               LstcOperation = Ent.LstcOperation;
               DateAdd = Ent.DateAdd;
               AddOperator = Ent.AddOperator;

               EditTime = Ent.EditTime;
               EditBy = Ent.EditBy;
               Email = Ent.Email;
           }
       }

       #endregion

       #region Properties

       public string Agency { get; set; }
       public string Dept { get; set; }
       public string Program { get; set; }
       public string Year { get; set; }
       public string Location { get; set; }
       public string Date { get; set; }
       public string Time { get; set; }
       public string SlotNumber { get; set; }
       public string SsNumber { get; set; }
       public string TemplateDate { get; set; }
       public string TemplateType { get; set; }
       public string RecordType { get; set; }
       public string Name { get; set; }
       public string FirstName { get; set; }
       //public string TelAreaCode { get; set; }
       public string TelNumber { get; set; }
       public string StNo { get; set; }
       public string Street { get; set; }
       public string Suffix { get; set; }
       public string Apt { get; set; }
       public string Floor { get; set; }
       public string City { get; set; }
       public string State { get; set; }
       public string Zip1 { get; set; }
       public string Zip2 { get; set; }
       public string HeatSource { get; set; }
       public string SourceIncome { get; set; }
       public string ContactPerson { get; set; }
       public string ContactDate { get; set; }
       public string Sex { get; set; }
       public string CellProvider { get; set; }
       //public string CellAreaCode { get; set; }
       public string CellNumber { get; set; }
       public string CaseWorker { get; set; }
       public string DateLstc { get; set; }
       public string LstcOperation { get; set; }
       public string DateAdd { get; set; }
       public string AddOperator { get; set; }
       public string Mode { get; set; }
       public string EditTime { get; set; }
       public string EditBy { get; set; }
       public string Email { get; set; }   

       #endregion

   }

    public class APPTSCHEDULEEntity
    {
        #region Constructors

        public APPTSCHEDULEEntity()
        {

            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            Site = string.Empty;
            Date = string.Empty;
            Time = string.Empty;
            SlotNumber = string.Empty;
            SsNumber = string.Empty;           
            TemplateID = string.Empty;
            SchdType = string.Empty;
            SchdDay = string.Empty;
            LastName = string.Empty;
            FirstName = string.Empty;          
            TelNumber = string.Empty;
            HNo = string.Empty;
            Street = string.Empty;
            Suffix = string.Empty;
            Apt = string.Empty;
            Floor = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip1 = string.Empty;
            Zip2 = string.Empty;
            HeatSource = string.Empty;
            SourceIncome = string.Empty;
            ContactPerson = string.Empty;
            ContactDate = string.Empty;
            Sex = string.Empty;
            CellProvider = string.Empty;           
            CellNumber = string.Empty;
            CaseWorker = string.Empty;
            DateLstc = string.Empty;
            LstcOperation = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            EditTime = string.Empty;
            EditBy = string.Empty;
            Email = string.Empty;
            Language = string.Empty;
            ReserveCount = string.Empty;
            ScheduleCount = string.Empty;
            TotalCount = string.Empty;
            DOB = string.Empty;

            Status = string.Empty;
            Client = string.Empty;
            Notes = string.Empty;
            SlotType = string.Empty;

            AdditionalSlotCount = string.Empty;
        }


        public APPTSCHEDULEEntity(bool boolstatus)
        {

            Agency = null;
            Dept = null;
            Program = null;
            Year = null;
            Site = null;
            Date = null;
            Time = null;
            SlotNumber = null;
            SsNumber = null;
            TemplateID = null;
            SchdType = null;
            SchdDay = null;
            LastName = null;
            FirstName = null;
            TelNumber = null;
            HNo = null;
            Street = null;
            Suffix = null;
            Apt = null;
            Floor = null;
            City = null;
            State = null;
            Zip1 = null;
            Zip2 = null;
            HeatSource = null;
            SourceIncome = null;
            ContactPerson = null;
            ContactDate = null;
            Sex = null;
            CellProvider = null;
            CellNumber = null;
            CaseWorker = null;
            DateLstc = null;
            LstcOperation = null;
            DateAdd = null;
            AddOperator = null;
            EditTime = null;
            EditBy = null;
            Email = null;
            Language = null;
            ReserveCount = null;
            ScheduleCount = null;
            TotalCount = null;
            DOB = null;
            Status = null;
            Client = null;
            Notes = null;
            SlotType = null;
            AdditionalSlotCount = null;
        }


        public APPTSCHEDULEEntity(string date, string time, string slot_num, string reason, string Template_Type, string Time_Key)
        {
            Date = date;
            Time = time;
            SlotNumber = slot_num;
            LastName = reason;
            SchdType = Template_Type;
            CaseWorker = Time_Key;
        }


        public APPTSCHEDULEEntity(DataRow row, string strType)
        {
            if (row != null)
            {
                if (strType == "Dates")
                {

                    Agency = row["APPTSCHD_AGY"].ToString();
                    Dept = row["APPTSCHD_DEPT"].ToString();
                    Program = row["APPTSCHD_PROG"].ToString();
                    Year = row["APPTSCHD_YEAR"].ToString();
                    Site = row["APPTSCHD_SITE"].ToString();
                    Date = row["APPTSCHD_DATE"].ToString();
                    ScheduleCount = row["SCHEDULECOUNT"].ToString();
                    ReserveCount = row["RESERVECOUNT"].ToString();
                    TotalCount = row["TOTALCOUNT"].ToString();
                    AdditionalSlotCount= row["AdditionalSlotCount"].ToString();

                    SsNumber = "00".Substring(0, 2 - row["DAYNUM"].ToString().Length) + row["DAYNUM"].ToString();                   

                }
                else
                {
                    Agency = row["APPTSCHD_AGY"].ToString();
                    Dept = row["APPTSCHD_DEPT"].ToString();
                    Program = row["APPTSCHD_PROG"].ToString();
                    Year = row["APPTSCHD_YEAR"].ToString();
                    Site = row["APPTSCHD_SITE"].ToString();
                    Date = row["APPTSCHD_DATE"].ToString();
                    Time = row["APPTSCHD_TIME"].ToString();
                    SlotNumber = row["APPTSCHD_SLOT"].ToString();
                    SsNumber = row["APPTSCHD_SSN"].ToString();
                    TemplateID = row["APPTSCHD_TEMPL_ID"].ToString();
                    SchdType = row["APPTSCHD_TYPE"].ToString();
                    SchdDay = row["APPTSCHD_DAY"].ToString();
                    LastName = row["APPTSCHD_LNAME"].ToString();
                    FirstName = row["APPTSCHD_FNAME"].ToString();
                    TelNumber = row["APPTSCHD_TEL"].ToString();
                    HNo = row["APPTSCHD_HNO"].ToString();
                    Street = row["APPTSCHD_STREET"].ToString();
                    Suffix = row["APPTSCHD_SUFFIX"].ToString();
                    Apt = row["APPTSCHD_APT"].ToString();
                    Floor = row["APPTSCHD_FLOOR"].ToString();
                    City = row["APPTSCHD_CITY"].ToString();
                    State = row["APPTSCHD_STATE"].ToString();
                    Zip1 = row["APPTSCHD_ZIP1"].ToString();
                    Zip2 = row["APPTSCHD_ZIP2"].ToString();
                    HeatSource = row["APPTSCHD_HEAT_SOURCE"].ToString();
                    SourceIncome = row["APPTSCHD_SOURCE_INCOME"].ToString();
                    ContactPerson = row["APPTSCHD_CONTACT_PERSON"].ToString();
                    ContactDate = row["APPTSCHD_CONTACT_DATE"].ToString();
                    Sex = row["APPTSCHD_SEX"].ToString();
                    CellProvider = row["APPTSCHD_CELL_PROVIDER"].ToString();
                    CellNumber = row["APPTSCHD_CELL_NUMBER"].ToString();
                    CaseWorker = row["APPTSCHD_CASE_WORKER"].ToString();
                    DateLstc = row["APPTSCHD_DATE_LSTC"].ToString();
                    LstcOperation = row["APPTSCHD_LSTC_OPERATOR"].ToString();
                    DateAdd = row["APPTSCHD_DATE_ADD"].ToString();
                    AddOperator = row["APPTSCHD_ADD_OPERATOR"].ToString();
                    EditTime = row["APPTSCHD_EDITIME"].ToString();
                    EditBy = row["APPTSCHD_EDITBY"].ToString();
                    Email = row["APPTSCHD_EMAIL"].ToString();
                    Language = row["APPTSCHD_LANGUAGE"].ToString();
                    DOB = row["APPTSCHD_DOB"].ToString();
                    Status = row["APPTSCHD_STATUS"].ToString();
                    Client = row["APPTSCHD_CLIENT"].ToString();
                    Notes = row["APPTSCHD_NOTES"].ToString();
                    SlotType = row["APPTSCHD_SLOT_TYPE"].ToString();
                }
            }
        }

        public APPTSCHEDULEEntity(APPTSCHEDULEEntity Ent)
        {
            //if (row != null)
            {
                Agency = Ent.Agency;
                Dept = Ent.Dept;
                Program = Ent.Program;
                Year = Ent.Year;
                Site = Ent.Site;
                Date = Ent.Date;
                Time = Ent.Time;
                SlotNumber = Ent.SlotNumber;
                SsNumber = Ent.SsNumber;               
                TemplateID = Ent.TemplateID;
                SchdType = Ent.SchdType;
                SchdDay = Ent.SchdDay;
                LastName = Ent.LastName;
                FirstName = Ent.FirstName;              
                TelNumber = Ent.TelNumber;
                HNo = Ent.HNo;
                Street = Ent.Street;
                Suffix = Ent.Suffix;
                Apt = Ent.Apt;
                Floor = Ent.Floor;
                City = Ent.City;
                State = Ent.State;
                Zip1 = Ent.Zip1;
                Zip2 = Ent.Zip2;
                HeatSource = Ent.HeatSource;
                SourceIncome = Ent.SourceIncome;
                ContactPerson = Ent.ContactPerson;
                ContactDate = Ent.ContactDate;
                Sex = Ent.Sex;
                CellProvider = Ent.CellProvider;               
                CellNumber = Ent.CellNumber;
                CaseWorker = Ent.CaseWorker;
                DateLstc = Ent.DateLstc;
                LstcOperation = Ent.LstcOperation;
                DateAdd = Ent.DateAdd;
                AddOperator = Ent.AddOperator;
                EditTime = Ent.EditTime;
                EditBy = Ent.EditBy;
                Email = Ent.Email;
                Language = Ent.Language;
                DOB = Ent.DOB;
                Status = Ent.Status;
                Client = Ent.Client;
                Notes = Ent.Notes;
            }
        }

        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string Site { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string SlotNumber { get; set; }
        public string SsNumber { get; set; }
        public string TemplateID { get; set; }
        public string SchdType { get; set; }
        public string SchdDay { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string TelNumber { get; set; }
        public string HNo { get; set; }
        public string Street { get; set; }
        public string Suffix { get; set; }
        public string Apt { get; set; }
        public string Floor { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip1 { get; set; }
        public string Zip2 { get; set; }
        public string HeatSource { get; set; }
        public string SourceIncome { get; set; }
        public string ContactPerson { get; set; }
        public string ContactDate { get; set; }
        public string Sex { get; set; }
        public string CellProvider { get; set; }        
        public string CellNumber { get; set; }
        public string CaseWorker { get; set; }
        public string Mode { get; set; }
        public string EditTime { get; set; }
        public string EditBy { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperation { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string ReserveCount { get; set; }
        public string ScheduleCount { get; set; }
        public string TotalCount { get; set; }
        public string DOB { get; set; }
        public string Status { get; set; }
        public string Client { get; set; }
        public string Notes { get; set; }
        public string SlotType { get; set; }
        public string AdditionalSlotCount { get; set; }
        #endregion

    }

    public class APPTSCHDHISTEntity
    {
        #region Constructors

        public APPTSCHDHISTEntity()
        {

            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            Site = string.Empty;
            Date = string.Empty;
            Time = string.Empty;
            SlotNumber = string.Empty;
            Seq = string.Empty;
            SsNumber = string.Empty;
            TemplateID = string.Empty;
            SchdType = string.Empty;
            SchdDay = string.Empty;
            LastName = string.Empty;
            FirstName = string.Empty;
            TelNumber = string.Empty;
            HNo = string.Empty;
            Street = string.Empty;
            Suffix = string.Empty;
            Apt = string.Empty;
            Floor = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip1 = string.Empty;
            Zip2 = string.Empty;
            HeatSource = string.Empty;
            SourceIncome = string.Empty;
            ContactPerson = string.Empty;
            ContactDate = string.Empty;
            Sex = string.Empty;
            CellProvider = string.Empty;
            CellNumber = string.Empty;
            CaseWorker = string.Empty;
            DateLstc = string.Empty;
            LstcOperation = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            EditTime = string.Empty;
            EditBy = string.Empty;
            Email = string.Empty;
            ReserveCount = string.Empty;
            ScheduleCount = string.Empty;
            TotalCount = string.Empty;
            DOB = string.Empty;

            Status = string.Empty;
            Client = string.Empty;
            Notes = string.Empty;
        }


        public APPTSCHDHISTEntity(bool boolstatus)
        {

            Agency = null;
            Dept = null;
            Program = null;
            Year = null;
            Site = null;
            Date = null;
            Time = null;
            SlotNumber = null;
            Seq = null;
            SsNumber = null;
            TemplateID = null;
            SchdType = null;
            SchdDay = null;
            LastName = null;
            FirstName = null;
            TelNumber = null;
            HNo = null;
            Street = null;
            Suffix = null;
            Apt = null;
            Floor = null;
            City = null;
            State = null;
            Zip1 = null;
            Zip2 = null;
            HeatSource = null;
            SourceIncome = null;
            ContactPerson = null;
            ContactDate = null;
            Sex = null;
            CellProvider = null;
            CellNumber = null;
            CaseWorker = null;
            DateLstc = null;
            LstcOperation = null;
            DateAdd = null;
            AddOperator = null;
            EditTime = null;
            EditBy = null;
            Email = null;
            ReserveCount = null;
            ScheduleCount = null;
            TotalCount = null;
            DOB = null;
            Status = null;
            Client = null;
            Notes = null;
        }


        public APPTSCHDHISTEntity(string date, string time, string slot_num, string reason, string Template_Type, string Time_Key)
        {
            Date = date;
            Time = time;
            SlotNumber = slot_num;
            LastName = reason;
            SchdType = Template_Type;
            CaseWorker = Time_Key;
        }


        public APPTSCHDHISTEntity(DataRow row, string strType)
        {
            if (row != null)
            {
                if (strType == "Dates")
                {

                    Agency = row["APPTSCHDHIST_AGY"].ToString();
                    Dept = row["APPTSCHDHIST_DEPT"].ToString();
                    Program = row["APPTSCHDHIST_PROG"].ToString();
                    Year = row["APPTSCHDHIST_YEAR"].ToString();
                    Site = row["APPTSCHDHIST_SITE"].ToString();
                    Date = row["APPTSCHDHIST_DATE"].ToString();
                    //ScheduleCount = row["SCHEDULECOUNT"].ToString();
                    //ReserveCount = row["RESERVECOUNT"].ToString();
                    //TotalCount = row["TOTALCOUNT"].ToString();
                    SsNumber = "00".Substring(0, 2 - row["DAYNUM"].ToString().Length) + row["DAYNUM"].ToString();

                }
                else
                {
                    Agency = row["APPTSCHDHIST_AGY"].ToString();
                    Dept = row["APPTSCHDHIST_DEPT"].ToString();
                    Program = row["APPTSCHDHIST_PROG"].ToString();
                    Year = row["APPTSCHDHIST_YEAR"].ToString();
                    Site = row["APPTSCHDHIST_SITE"].ToString();
                    Date = row["APPTSCHDHIST_DATE"].ToString();
                    Time = row["APPTSCHDHIST_TIME"].ToString();
                    SlotNumber = row["APPTSCHDHIST_SLOT"].ToString();
                    Seq = row["APPTSCHDHIST_SEQ"].ToString();
                    SsNumber = row["APPTSCHDHIST_SSN"].ToString();
                    TemplateID = row["APPTSCHDHIST_TEMPL_ID"].ToString();
                    SchdType = row["APPTSCHDHIST_TYPE"].ToString();
                    SchdDay = row["APPTSCHDHIST_DAY"].ToString();
                    LastName = row["APPTSCHDHIST_LNAME"].ToString();
                    FirstName = row["APPTSCHDHIST_FNAME"].ToString();
                    TelNumber = row["APPTSCHDHIST_TEL"].ToString();
                    HNo = row["APPTSCHDHIST_HNO"].ToString();
                    Street = row["APPTSCHDHIST_STREET"].ToString();
                    Suffix = row["APPTSCHDHIST_SUFFIX"].ToString();
                    Apt = row["APPTSCHDHIST_APT"].ToString();
                    Floor = row["APPTSCHDHIST_FLOOR"].ToString();
                    City = row["APPTSCHDHIST_CITY"].ToString();
                    State = row["APPTSCHDHIST_STATE"].ToString();
                    Zip1 = row["APPTSCHDHIST_ZIP1"].ToString();
                    Zip2 = row["APPTSCHDHIST_ZIP2"].ToString();
                    HeatSource = row["APPTSCHDHIST_HEAT_SOURCE"].ToString();
                    SourceIncome = row["APPTSCHDHIST_SOURCE_INCOME"].ToString();
                    ContactPerson = row["APPTSCHDHIST_CONTACT_PERSON"].ToString();
                    ContactDate = row["APPTSCHDHIST_CONTACT_DATE"].ToString();
                    Sex = row["APPTSCHDHIST_SEX"].ToString();
                    CellProvider = row["APPTSCHDHIST_CELL_PROVIDER"].ToString();
                    CellNumber = row["APPTSCHDHIST_CELL_NUMBER"].ToString();
                    CaseWorker = row["APPTSCHDHIST_CASE_WORKER"].ToString();
                    DateLstc = row["APPTSCHDHIST_DATE_LSTC"].ToString();
                    LstcOperation = row["APPTSCHDHIST_LSTC_OPERATOR"].ToString();
                    DateAdd = row["APPTSCHDHIST_DATE_ADD"].ToString();
                    AddOperator = row["APPTSCHDHIST_ADD_OPERATOR"].ToString();
                    EditTime = row["APPTSCHDHIST_EDITIME"].ToString();
                    EditBy = row["APPTSCHDHIST_EDITBY"].ToString();
                    Email = row["APPTSCHDHIST_EMAIL"].ToString();
                    DOB = row["APPTSCHDHIST_DOB"].ToString();
                    Status = row["APPTSCHDHIST_STATUS"].ToString();
                    Client = row["APPTSCHDHIST_CLIENT"].ToString();
                    Notes = row["APPTSCHDHIST_NOTES"].ToString();
                }
            }
        }

        public APPTSCHDHISTEntity(APPTSCHDHISTEntity Ent)
        {
            //if (row != null)
            {
                Agency = Ent.Agency;
                Dept = Ent.Dept;
                Program = Ent.Program;
                Year = Ent.Year;
                Site = Ent.Site;
                Date = Ent.Date;
                Time = Ent.Time;
                SlotNumber = Ent.SlotNumber;
                Seq = Ent.Seq;
                SsNumber = Ent.SsNumber;
                TemplateID = Ent.TemplateID;
                SchdType = Ent.SchdType;
                SchdDay = Ent.SchdDay;
                LastName = Ent.LastName;
                FirstName = Ent.FirstName;
                TelNumber = Ent.TelNumber;
                HNo = Ent.HNo;
                Street = Ent.Street;
                Suffix = Ent.Suffix;
                Apt = Ent.Apt;
                Floor = Ent.Floor;
                City = Ent.City;
                State = Ent.State;
                Zip1 = Ent.Zip1;
                Zip2 = Ent.Zip2;
                HeatSource = Ent.HeatSource;
                SourceIncome = Ent.SourceIncome;
                ContactPerson = Ent.ContactPerson;
                ContactDate = Ent.ContactDate;
                Sex = Ent.Sex;
                CellProvider = Ent.CellProvider;
                CellNumber = Ent.CellNumber;
                CaseWorker = Ent.CaseWorker;
                DateLstc = Ent.DateLstc;
                LstcOperation = Ent.LstcOperation;
                DateAdd = Ent.DateAdd;
                AddOperator = Ent.AddOperator;
                EditTime = Ent.EditTime;
                EditBy = Ent.EditBy;
                Email = Ent.Email;
                DOB = Ent.DOB;
                Status = Ent.Status;
                Client = Ent.Client;
                Notes = Ent.Notes;
            }
        }

        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string Site { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string SlotNumber { get; set; }
        public string Seq { get; set; }
        public string SsNumber { get; set; }
        public string TemplateID { get; set; }
        public string SchdType { get; set; }
        public string SchdDay { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string TelNumber { get; set; }
        public string HNo { get; set; }
        public string Street { get; set; }
        public string Suffix { get; set; }
        public string Apt { get; set; }
        public string Floor { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip1 { get; set; }
        public string Zip2 { get; set; }
        public string HeatSource { get; set; }
        public string SourceIncome { get; set; }
        public string ContactPerson { get; set; }
        public string ContactDate { get; set; }
        public string Sex { get; set; }
        public string CellProvider { get; set; }
        public string CellNumber { get; set; }
        public string CaseWorker { get; set; }
        public string Mode { get; set; }
        public string EditTime { get; set; }
        public string EditBy { get; set; }
        public string Email { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperation { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string ReserveCount { get; set; }
        public string ScheduleCount { get; set; }
        public string TotalCount { get; set; }
        public string DOB { get; set; }
        public string Status { get; set; }
        public string Client { get; set; }
        public string Notes { get; set; }
        #endregion

    }


    public class TMSAPCNSLTEntity
   {
    #region Constructors

       public TMSAPCNSLTEntity()
       {

           Agency = string.Empty;
           Dept = string.Empty;
           Program = string.Empty;
           Year = string.Empty;
           Location = string.Empty;
           Date = string.Empty;
           Time = string.Empty;
           Type = string.Empty;
           Opcl = string.Empty;
         
       }


       public TMSAPCNSLTEntity(DataRow row, string strType)
       {
           if (row != null)
           {
               Agency = row["TMSAPCNSLT_AGENCY"].ToString();
               Dept = row["TMSAPCNSLT_DEPT"].ToString();
               Program = row["TMSAPCNSLT_PROGRAM"].ToString();
               Year = row["TMSAPCNSLT_YEAR"].ToString();
               Location = row["TMSAPCNSLT_LOCATION"].ToString();
               Date = row["TMSAPCNSLT_DATE"].ToString();
               Type = row["TMSAPCNSLT_TYPE"].ToString();
               Time = row["TMSAPCNSLT_TIME"].ToString();
               Opcl = row["TMSAPCNSLT_OPCL"].ToString();      

           }

       }
       #endregion

       #region Properties

       public string Agency { get; set; }
       public string Dept { get; set; }
       public string Program { get; set; }
       public string Year { get; set; }
       public string Location { get; set; }
       public string Date { get; set; }
       public string Time { get; set; }
       public string Type { get; set; }
       public string Opcl { get; set; }   
       public string Mode { get; set; }   
       #endregion
   
   }

   public class CaseNotesEntity
   { 
    #region Constructors

       public CaseNotesEntity()
       {

           Agency = string.Empty;
           Dept = string.Empty;
           Program = string.Empty;
           Year = string.Empty;
           Location = string.Empty;
           ScreenName = string.Empty;
           FieldName = string.Empty;          
           Data = string.Empty;
           DateLstc = string.Empty;
           LstcOperation = string.Empty;
           DateAdd = string.Empty;
           AddOperator = string.Empty;   
         
       }


       public CaseNotesEntity(DataRow row, string strType)
       {
           if (row != null)
           {
               if (strType == "KEYFIELD")
               {
                   FieldName = row["CASENOTES_KEYFIELD"].ToString();
               }
               else
               {
                   ScreenName = row["CASENOTES_SCREEN"].ToString();
                   FieldName = row["CASENOTES_KEYFIELD"].ToString();
                   AppliCationNo = row["CASENOTES_APP_NO"].ToString();
                   DateAdd = row["CASENOTES_DATE_ADD"].ToString().Trim();
                   AddOperator = row["CASENOTES_ADD_OPERATOR"].ToString().Trim();
                   DateLstc = row["CASENOTES_DATE_LSTC"].ToString().Trim();
                   LstcOperation = row["CASENOTES_LSTC_OPERATOR"].ToString().Trim();
                   Data = row["CASENOTES_DATA"].ToString().Trim();
               }
           }

       }
       #endregion

       #region Properties

       public string Agency { get; set; }
       public string Dept { get; set; }
       public string Program { get; set; }
       public string Year { get; set; }
       public string Location { get; set; }
       public string ScreenName { get; set; }
       public string FieldName { get; set; }      
       public string Data { get; set; }
       public string DateLstc { get; set; }
       public string LstcOperation { get; set; }
       public string DateAdd { get; set; }
       public string AddOperator { get; set; }
       public string Mode { get; set; }
       public string AppliCationNo { get; set; }
       #endregion
   
   }


   public class TMSB4015Entity
   {
       #region Constructors

       public TMSB4015Entity()
       {

           Agency = string.Empty;
           Dept = string.Empty;
           Program = string.Empty;
           Year = string.Empty;
           Appno = string.Empty;
           Lname = string.Empty;
           Fname = string.Empty;
           Mname = string.Empty;
           SsNumber = string.Empty;
           HN = string.Empty;
           Street = string.Empty;
           Suffix = string.Empty;
           Apt = string.Empty;
           Flr = string.Empty;
           City = string.Empty;
           State = string.Empty;
           Zip = string.Empty;
           Income = string.Empty;
           Sex = string.Empty;
           Source = string.Empty;
           Amount = string.Empty;
           CertDate = string.Empty;
           Status = string.Empty;
           Benfit_Level = string.Empty;
           Intake_Date = string.Empty;
           DOB = string.Empty;
           Disable = string.Empty;
           Poverty = string.Empty;
           Age = string.Empty;
           FamilySeq = string.Empty;

           SNP_LName = string.Empty;
           SNP_FName = string.Empty;
           SNP_Mname = string.Empty;
           SNP_SSN = string.Empty;
           SNP_BDATE = string.Empty;
           SNP_MSEX = string.Empty;
       }


       public TMSB4015Entity(DataRow row)
       {
           if (row != null)
           {
               Agency = row["MST_AGENCY"].ToString();
               Dept = row["MST_DEPT"].ToString();
               Program = row["MST_PROGRAM"].ToString();
               Year = row["MST_YEAR"].ToString();
               Appno = row["MST_APP_NO"].ToString();
               Lname = row["LNAME"].ToString();
               Fname = row["FNAME"].ToString();
               Mname = row["MNAME"].ToString();
               SsNumber = row["MST_SSN"].ToString();
               HN = row["MST_HN"].ToString();
               Street = row["MST_STREET"].ToString();
               Suffix = row["MST_SUFFIX"].ToString();
               Apt = row["MST_APT"].ToString();
               Flr = row["MST_FLR"].ToString();
               City = row["MST_CITY"].ToString();
               State = row["MST_STATE"].ToString();
               Zip = row["MST_ZIP"].ToString();
               Income = row["LPB_FAP_INCOME"].ToString();
               Sex = row["SNP_SEX"].ToString();
               Source = row["LPB_SOURCE"].ToString();
               Amount = row["LPB_AMOUNT"].ToString();
               CertDate = row["LPB_DATE"].ToString();
               Status = row["LPB_CERTIFIED_STATUS"].ToString();
               Benfit_Level = row["LPB_BENEFIT_LEVEL"].ToString();
               Intake_Date = row["MST_INTAKE_DATE"].ToString();
               DOB = row["SNP_ALT_BDATE"].ToString();
               Disable = row["SNP_DISABLE"].ToString();
               Poverty = row["MST_POVERTY"].ToString();
               ZipPlus = row["MST_ZIPPLUS"].ToString();
               Area = row["MST_AREA"].ToString();
               Phone = row["MST_PHONE"].ToString();
               Age = row["AGE"].ToString();
               FamilySeq = row["MST_FAMILY_SEQ"].ToString();
               


           }

       }

       public TMSB4015Entity(DataRow row,string TABLE)
       {
           if (row != null)
           {
               Agency = row["MST_AGENCY"].ToString();
               Dept = row["MST_DEPT"].ToString();
               Program = row["MST_PROGRAM"].ToString();
               Year = row["MST_YEAR"].ToString();
               Appno = row["MST_APP_NO"].ToString();
               Lname = row["LNAME"].ToString();
               Fname = row["FNAME"].ToString();
               Mname = row["MNAME"].ToString();
               SsNumber = row["MST_SSN"].ToString();
               HN = row["MST_HN"].ToString();
               Street = row["MST_STREET"].ToString();
               Suffix = row["MST_SUFFIX"].ToString();
               Apt = row["MST_APT"].ToString();
               Flr = row["MST_FLR"].ToString();
               City = row["MST_CITY"].ToString();
               State = row["MST_STATE"].ToString();
               Zip = row["MST_ZIP"].ToString();
               Income = row["LPB_FAP_INCOME"].ToString();
               Sex = row["SNP_SEX"].ToString();
               Source = row["LPB_SOURCE"].ToString();
               Amount = row["LPB_AMOUNT"].ToString();
               CertDate = row["LPB_DATE"].ToString();
               Status = row["LPB_CERTIFIED_STATUS"].ToString();
               Benfit_Level = row["LPB_BENEFIT_LEVEL"].ToString();
               Intake_Date = row["MST_INTAKE_DATE"].ToString();
               DOB = row["SNP_ALT_BDATE"].ToString();
               Disable = row["SNP_DISABLE"].ToString();
               Poverty = row["MST_POVERTY"].ToString();
               ZipPlus = row["MST_ZIPPLUS"].ToString();
               Area = row["MST_AREA"].ToString();
               Phone = row["MST_PHONE"].ToString();
               Age = row["AGE"].ToString();
               FamilySeq = row["MST_FAMILY_SEQ"].ToString();

               SNP_LName = row["SNP_NAME_IX_LAST"].ToString();
               SNP_FName = row["SNP_NAME_IX_FI"].ToString();
               SNP_Mname = row["SNP_NAME_IX_MI"].ToString();
               SNP_SSN = row["SSN"].ToString();
               SNP_BDATE = row["BDATE"].ToString();
               SNP_MSEX = row["MSEX"].ToString();

           }

       }

       #endregion

       #region Properties

       public string Agency { get; set; }
       public string Dept { get; set; }
       public string Program { get; set; }
       public string Year { get; set; }
       public string Appno { get; set; }
       public string Lname { get; set; }
       public string Fname { get; set; }
       public string Mname { get; set; }
       public string SsNumber { get; set; }
       public string HN { get; set; }
       public string Street { get; set; }
       public string Suffix { get; set; }
       public string Apt { get; set; }
       public string Flr { get; set; }
       public string City { get; set; }
       public string State { get; set; }
       public string Zip { get; set; }
       public string Sex { get; set; }
       public string Income { get; set; }
       public string Source { get; set; }
       public string Amount { get; set; }
       public string Status { get; set; }
       public string CertDate { get; set; }
       public string Benfit_Level { get; set; }
       public string Intake_Date { get; set; }
       public string DOB { get; set; }
       public string Disable { get; set; }
       public string Poverty { get; set; }
       public string ZipPlus { get; set; }
       public string Area { get; set; }
       public string Phone { get; set; }
       public string Age { get; set; }
       public string FamilySeq { get; set; }
       public string Mode { get; set; }
       public string SNP_LName { get; set; }
       public string SNP_FName { get; set; }
       public string SNP_Mname { get; set; }
       public string SNP_SSN { get; set; }
       public string SNP_BDATE { get; set; }
       public string SNP_MSEX { get; set; }

       #endregion

   }

   public class TMSB4007ENTITY
   {
       #region Constructors

       public TMSB4007ENTITY()
       {
           CASENUMBER = string.Empty;
           LASTNAME = string.Empty;
           SNP_NAME_IX_FI = string.Empty;
           MI = string.Empty;
           DATE = string.Empty;
       }

       public TMSB4007ENTITY(DataRow row)
       {
           if (row != null)
           {
               CASENUMBER = row["MST_APP_NO"].ToString();
               LASTNAME = row["SNP_NAME_IX_LAST"].ToString();
               SNP_NAME_IX_FI = row["SNP_NAME_IX_FI"].ToString();
               MI = row["SNP_NAME_IX_MI"].ToString();
               DATE = row["MST_INTAKE_DATE"].ToString();
           }
       }
       #endregion

       #region Properties

       public string CASENUMBER { get; set; }
       public string LASTNAME { get; set; }
       public string SNP_NAME_IX_FI { get; set; }
       public string MI { get; set; }
       public string DATE { get; set; }

       #endregion
   }


   public class TMSB0005ENTITY
   {
       #region Constructors

       public TMSB0005ENTITY()
       {
           Agency = string.Empty;
           Dept = string.Empty;
           Program = string.Empty;
           Year = string.Empty;
           Appno = string.Empty;
           Fname = string.Empty;
           MName = string.Empty;
           LName = string.Empty;
           AccNo = string.Empty;
           HN = string.Empty;
           Street = string.Empty;
           Suffix = string.Empty;
           Apt = string.Empty;
           Flr = string.Empty;
           City = string.Empty;
           State = string.Empty;
           Zip = string.Empty;
           Amount = string.Empty;
           Reduce_Elig = string.Empty;
           Vendor = string.Empty;
           Index_By = string.Empty;
           Vendor_Name = string.Empty;
           Vendor_Address = string.Empty;
           Vendor_Zip = string.Empty;
       }

       public TMSB0005ENTITY(DataRow row)
       {
           if (row != null)
           {
               Agency = row["MST_AGENCY"].ToString();
               Dept = row["MST_DEPT"].ToString();
               Program = row["MST_PROGRAM"].ToString();
               Year = row["MST_YEAR"].ToString();
               Appno = row["MST_APP_NO"].ToString();
               Fname = row["SNP_NAME_IX_FI"].ToString();
               MName = row["SNP_NAME_IX_MI"].ToString();
               LName = row["SNP_NAME_IX_LAST"].ToString();
               AccNo = row["LPV_ACCOUNT_NO"].ToString();
               HN = row["MST_HN"].ToString();
               Street = row["MST_STREET"].ToString();
               Suffix = row["MST_SUFFIX"].ToString();
               Apt = row["MST_APT"].ToString();
               Flr = row["MST_FLR"].ToString();
               City = row["MST_CITY"].ToString();
               State = row["MST_STATE"].ToString();
               Zip = row["MST_ZIP"].ToString();
               Amount = row["LPB_AMOUNT"].ToString();
               Reduce_Elig = row["LPB_REDUCE_ELIG"].ToString();
               Vendor = row["LPV_VENDOR"].ToString();
               Index_By = row["CASEVDD1_INDEXBY"].ToString();
               Vendor_Name = row["CASEVDD_NAME"].ToString();
               Vendor_Address = row["CASEVDD_ADDR1"].ToString();
               Vendor_Zip = row["CASEVDD_ZIP"].ToString();

           }
       }
       #endregion

       #region Properties

       public string Agency { get; set; }
       public string Dept { get; set; }
       public string Program { get; set; }
       public string Year { get; set; }
       public string Appno { get; set; }
       public string Fname { get; set; }
       public string LName { get; set; }
       public string MName { get; set; }
       public string AccNo { get; set; }
       public string HN { get; set; }
       public string Street { get; set; }
       public string Suffix { get; set; }
       public string Apt { get; set; }
       public string Flr { get; set; }
       public string City { get; set; }
       public string State { get; set; }
       public string Zip { get; set; }
       public string Amount { get; set; }
       public string Reduce_Elig { get; set; }
       public string Vendor { get; set; }
       public string Index_By { get; set; }
       public string Vendor_Name { get; set; }
       public string Vendor_Address { get; set; }
       public string Vendor_Zip { get; set; }
       
       #endregion
   }

   public class TMSB0006Entity
   {
       #region Constructors

       public TMSB0006Entity()
       {
           Agency = string.Empty;
           Dept = string.Empty;
           Program = string.Empty;
           Year = string.Empty;
           Appno = string.Empty;
           Fname = string.Empty;
           MName = string.Empty;
           LName = string.Empty;
           MST_INTAKE_DATE = string.Empty;
           SNP_Disable = string.Empty;
           LPB_Disable = string.Empty;
           SSN_No = string.Empty;
           LPB_FAP_INCOME = string.Empty;
           MST_PROG_INCOME = string.Empty;
           MST_NO_INHH = string.Empty;
           LPB_FAP_NO_INHH = string.Empty;
           LPB_SOURCE = string.Empty;
           MST_SOURCE = string.Empty;
           LPB_PAYMENT_HOW = string.Empty;
           MST_HEAT_INC_RENT = string.Empty;
           MST_SSN_FLAG = string.Empty;
           LPB_SSN_SW = string.Empty;
           LPB_DATE = string.Empty;
           SNP_SSN_REASON = string.Empty;
           MST_NO_INPROG = string.Empty;
           Status = string.Empty;
            INTK_LETTER_DATE = string.Empty;
            INTK_INCOMP_COUNT = string.Empty;

            MST_SITE = string.Empty;
            IntakeWorker = string.Empty;
            INTK_WORKER = string.Empty;

            DSS = string.Empty;
            DSS_CatElig = string.Empty;
            DSS_Burden = string.Empty;

            MST_LPM_0006=string.Empty;
            MST_LPM_0008 = string.Empty;
        }

       public TMSB0006Entity(DataRow row)
       {
           if (row != null)
           {
               Agency = row["MST_AGENCY"].ToString();
               Dept = row["MST_DEPT"].ToString();
               Program = row["MST_PROGRAM"].ToString();
               Year = row["MST_YEAR"].ToString();
               Appno = row["MST_APP_NO"].ToString();
               Fname = row["SNP_NAME_IX_FI"].ToString();
               MName = row["SNP_NAME_IX_MI"].ToString();
               LName = row["SNP_NAME_IX_LAST"].ToString();
               MST_INTAKE_DATE = row["MST_INTAKE_DATE"].ToString();
               SNP_Disable = row["SNP_DISABLE"].ToString();
               LPB_Disable = row["LPB_DISABLE"].ToString();
               SSN_No = row["SNP_SSNO"].ToString();
               LPB_FAP_INCOME = row["LPB_FAP_INCOME"].ToString();
               MST_PROG_INCOME = row["MST_PROG_INCOME"].ToString();
               MST_NO_INHH = row["MST_NO_INHH"].ToString();
               LPB_FAP_NO_INHH = row["LPB_FAP_NO_INHH"].ToString();
               LPB_SOURCE = row["LPB_SOURCE"].ToString();
               MST_SOURCE = row["MST_SOURCE"].ToString();
               LPB_PAYMENT_HOW = row["LPB_PAYMENT_HOW"].ToString();
               MST_HEAT_INC_RENT = row["MST_HEAT_INC_RENT"].ToString();
               MST_SSN_FLAG = row["MST_SSN_FLAG"].ToString();
               LPB_SSN_SW = row["LPB_SSN_SW"].ToString();
               LPB_DATE = row["LPB_DATE"].ToString();
               SNP_SSN_REASON = row["SNP_SSN_REASON"].ToString();
               MST_NO_INPROG = row["MST_NO_INPROG"].ToString();
               Status = row["SNP_STATUS"].ToString();
                INTK_LETTER_DATE = row["INTK_LETTER_DATE"].ToString();
                INTK_INCOMP_COUNT = row["INTK_INCOMP_COUNT"].ToString();
                INTK_SCH_DATE = row["INTK_SCH_DATE"].ToString();
                INTK_WORKER = row["INTK_WORKER"].ToString();

                MST_SITE = row["MST_SITE"].ToString();
                IntakeWorker = row["Intake_worker"].ToString();

                DSS = row["DSS"].ToString();

                DSS_CatElig = row["DXM_CATELIG"].ToString();
                DSS_Burden = row["DXM_BURDEN"].ToString();

                MST_LPM_0006 = row["MST_LPM_0006"].ToString();
                MST_LPM_0008 = row["MST_LPM_0008"].ToString();
            }
       }
       #endregion

       #region Properties

       public string Agency { get; set; }
       public string Dept { get; set; }
       public string Program { get; set; }
       public string Year { get; set; }
       public string Appno { get; set; }
       public string Fname { get; set; }
       public string LName { get; set; }
       public string MName { get; set; }
       public string MST_INTAKE_DATE { get; set; }
       public string SNP_Disable { get; set; }
       public string LPB_Disable { get; set; }
       public string SSN_No { get; set; }
       public string LPB_FAP_INCOME { get; set; }
       public string MST_PROG_INCOME { get; set; }
       public string MST_NO_INHH { get; set; }
       public string LPB_FAP_NO_INHH { get; set; }
       public string LPB_SOURCE { get; set; }
       public string MST_SOURCE { get; set; }
       public string LPB_PAYMENT_HOW { get; set; }
       public string MST_HEAT_INC_RENT { get; set; }
       public string MST_SSN_FLAG { get; set; }
       public string LPB_SSN_SW { get; set; }
       public string LPB_DATE { get; set; }
       public string SNP_SSN_REASON { get; set; }
       public string MST_NO_INPROG { get; set; }
       public string Status { get; set; }
        public string INTK_LETTER_DATE { get; set; }
        public string INTK_SCH_DATE { get; set; }
        public string INTK_INCOMP_COUNT { get; set; }
        public string MST_SITE { get; set; }
        public string IntakeWorker { get; set; }

        public string INTK_WORKER { get; set; }

        public string DSS { get; set; }
        public string DSS_CatElig { get; set; }
        public string DSS_Burden { get; set; }


        public string MST_LPM_0006 { get; set; }
        public string MST_LPM_0008 { get; set; }


        #endregion
    }


   public class TMSB0011_nondelverEntity
   {
        #region Constructors

        public TMSB0011_nondelverEntity()
        {
            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            Appno = string.Empty;
            Fname = string.Empty;
            MName = string.Empty;
            LName = string.Empty;
            LPB_AMOUNT = string.Empty;
            LPB_TYPE = string.Empty;
            LPB_BENEFIT_LEVEL = string.Empty;
            LPB_CERTIFIED_STATUS = string.Empty;
            LPB_DATE_LSTC = string.Empty;
            LPB_LSTC_OPERATOR = string.Empty;

            Apartment = string.Empty;
            HouseNo = string.Empty;
            Floor = string.Empty;
            Street = string.Empty;
            Direction = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;
            Phone = string.Empty;
            Cell = string.Empty;
            Email = string.Empty;
            suffix = string.Empty;

            //LPB_DATE = string.Empty;
            //SNP_SSN_REASON = string.Empty;
            //MST_NO_INPROG = string.Empty;
        }

        public TMSB0011_nondelverEntity(DataRow row)
        {
            if (row != null)
            {
                Agency = row["LPB_AGENCY"].ToString();
                Dept = row["LPB_DEPT"].ToString();
                Program = row["LPB_PROG"].ToString();
                Year = row["LPB_YEAR"].ToString();
                Appno = row["LPB_APP_NO"].ToString();
                Fname = row["SNP_NAME_IX_FI"].ToString();
                //MName = row["SNP_NAME_IX_MI"].ToString();
                LName = row["SNP_NAME_IX_LAST"].ToString();
                LPB_AMOUNT = row["LPB_AMOUNT"].ToString();
                LPB_TYPE = row["LPB_TYPE"].ToString();
                LPB_BENEFIT_LEVEL = row["LPB_BENEFIT_LEVEL"].ToString();
                LPB_CERTIFIED_STATUS = row["LPB_CERTIFIED_STATUS"].ToString();
                LPB_DATE_LSTC = row["LPB_DATE_LSTC"].ToString();
                LPB_LSTC_OPERATOR = row["LSTC_Operator"].ToString();

                Apartment = row["APT"].ToString();
                HouseNo = row["HN"].ToString();
                Floor = row["FLR"].ToString();
                suffix = row["SUFFIX"].ToString();
                Street = row["STREET"].ToString();
                Direction = row["DIRECTION"].ToString();
                City = row["CITY"].ToString();
                State = row["SSTATE"].ToString();
                Zip = row["ZIP"].ToString();
                Phone = row["PHONE"].ToString();
                Cell = row["MST_CELL_PHONE"].ToString();
                Email = row["MST_EMAIL"].ToString();

                //LPB_DATE = row["LPB_DATE"].ToString();
                //SNP_SSN_REASON = row["SNP_SSN_REASON"].ToString();
                //MST_NO_INPROG = row["MST_NO_INPROG"].ToString();

            }
        }
       #endregion

       #region Properties

       public string Agency { get; set; }
       public string Dept { get; set; }
       public string Program { get; set; }
       public string Year { get; set; }
       public string Appno { get; set; }
       public string Fname { get; set; }
       public string LName { get; set; }
       public string MName { get; set; }
       public string LPB_AMOUNT { get; set; }
       public string LPB_TYPE { get; set; }
       public string LPB_BENEFIT_LEVEL { get; set; }
       public string LPB_CERTIFIED_STATUS { get; set; }
       public string LPB_DATE_LSTC { get; set; }
       public string LPB_LSTC_OPERATOR { get; set; }

        public string Apartment
        {
            get; set;
        }
        public string HouseNo
        {
            get; set;
        }
        public string Floor
        {
            get; set;
        }
        public string Street
        {
            get; set;
        }
        public string Direction
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
        public string Zip
        {
            get; set;
        }
        public string Phone
        {
            get; set;
        }
        public string Cell
        {
            get; set;
        }
        public string Email
        {
            get; set;
        }
        public string suffix
        {
            get; set;
        }


        //public string LPB_DATE { get; set; }
        //public string SNP_SSN_REASON { get; set; }
        //public string MST_NO_INPROG { get; set; }


        #endregion
    }

   public class TMSB0011_Delivers
   {
       #region Constructors

       public TMSB0011_Delivers()
       {
           Agency = string.Empty;
           Dept = string.Empty;
           Program = string.Empty;
           Year = string.Empty;
           Appno = string.Empty;
           Fname = string.Empty;
           MName = string.Empty;
           LName = string.Empty;
           MST_STATE = string.Empty;
           MST_CITY = string.Empty;
           MST_STREET = string.Empty;
           MST_SUFFIX = string.Empty;
           MST_HN = string.Empty;
           MST_APT = string.Empty;
           MST_FLR = string.Empty;
           MST_ZIP = string.Empty;
           MST_ZIPPLUS = string.Empty;
           MST_AREA = string.Empty;
           MST_PHONE = string.Empty;
           CASEVDD_NAME = string.Empty;
           CASEVDD1_INDEXBY = string.Empty;
           PMR_VENDOR = string.Empty;
           PMR_BS = string.Empty;
           PMR_AUTH_AMOUNT1 = string.Empty;
           PMR_AUTH_AMOUNT2 = string.Empty;
           MST_SOURCE = string.Empty;
           PMR_AUTH_NUM = string.Empty;
           PMR_AMOUNT_GAL = string.Empty;
           PMR_VENDOR_PP = string.Empty;
           PMR_DEL_DATE = string.Empty;
           PMR_AMOUNT_DELIVERY = string.Empty;
           PMR_AMOUNT_MISC = string.Empty;
           PMR_AMOUNT_STARTUP = string.Empty;
           PMR_AMOUNT_DOL = string.Empty;
           PMR_LSTC_OPERATOR = string.Empty;
           PMR_DATE_LSTC = string.Empty;
           PMR_AUTH_DATE = string.Empty;
           APPCount = string.Empty;
           Amount = string.Empty;
           AGY_7 = string.Empty;
            //SNP_SSN_REASON = string.Empty;
            //MST_NO_INPROG = string.Empty;

            PMR_AUTH_AMOUNT3 = string.Empty;
        }

       public TMSB0011_Delivers(DataRow row)
       {
           if (row != null)
           {
               Agency = row["PMR_AGENCY"].ToString();
               Dept = row["PMR_DEPT"].ToString();
               Program = row["PMR_PROGRAM"].ToString();
               Year = row["PMR_YEAR"].ToString();
               Appno = row["PMR_APPL_NO"].ToString();
               Fname = row["SNP_NAME_IX_FI"].ToString();
               MName = row["SNP_NAME_IX_MI"].ToString();
               LName = row["SNP_NAME_IX_LAST"].ToString();
               MST_STATE = row["MST_STATE"].ToString();
               MST_CITY = row["MST_CITY"].ToString();
               MST_STREET = row["MST_STREET"].ToString();
               MST_SUFFIX = row["MST_SUFFIX"].ToString();
               MST_HN = row["MST_HN"].ToString();
               MST_APT = row["MST_APT"].ToString();
               MST_FLR = row["MST_FLR"].ToString();
               MST_ZIP = row["MST_ZIP"].ToString();
               MST_ZIPPLUS = row["MST_ZIPPLUS"].ToString();
               MST_AREA = row["MST_AREA"].ToString();
               MST_PHONE = row["MST_PHONE"].ToString();
               CASEVDD_NAME = row["CASEVDD_NAME"].ToString();
               CASEVDD1_INDEXBY = row["CASEVDD1_INDEXBY"].ToString();
               PMR_VENDOR = row["PMR_VENDOR"].ToString();
               PMR_BS = row["PMR_BS"].ToString();
               PMR_AUTH_AMOUNT1 = row["PMR_AUTH_AMOUNT1"].ToString();
               PMR_AUTH_AMOUNT2 = row["PMR_AUTH_AMOUNT2"].ToString();
               MST_SOURCE = row["MST_SOURCE"].ToString();
               PMR_AUTH_NUM = row["PMR_AUTH_NUM"].ToString();
               PMR_AMOUNT_GAL = row["PMR_AMOUNT_GAL"].ToString();// == string.Empty ? "0.0000" : row["PMR_AMOUNT_GAL"].ToString();
               PMR_VENDOR_PP = row["PMR_VENDOR_PP"].ToString();// == string.Empty ? "0.000" : row["PMR_VENDOR_PP"].ToString();
               PMR_DEL_DATE = row["PMR_DEL_DATE"].ToString();
               PMR_AMOUNT_DELIVERY = row["PMR_AMOUNT_DELIVERY"].ToString();// == string.Empty ? "0.00" : row["PMR_AMOUNT_DELIVERY"].ToString();
               PMR_AMOUNT_MISC = row["PMR_AMOUNT_MISC"].ToString();// == string.Empty ? "0.00" : row["PMR_AMOUNT_MISC"].ToString();
               PMR_AMOUNT_STARTUP = row["PMR_AMOUNT_STARTUP"].ToString();// == string.Empty ? "0.00" : row["PMR_AMOUNT_STARTUP"].ToString();
               PMR_AMOUNT_DOL = row["PMR_AMOUNT_DOL"].ToString();// == string.Empty ? "0.00" : row["PMR_AMOUNT_DOL"].ToString();
               PMR_LSTC_OPERATOR = row["PMR_LSTC_OPERATOR"].ToString();
               PMR_DATE_LSTC = row["PMR_DATE_LSTC"].ToString();
               PMR_AUTH_DATE = row["PMR_AUTH_DATE"].ToString();
               AGY_7 = row["AGY_7"].ToString();
                //LPB_DATE = row["LPB_DATE"].ToString();
                //SNP_SSN_REASON = row["SNP_SSN_REASON"].ToString();
                //MST_NO_INPROG = row["MST_NO_INPROG"].ToString();
                //LPB_DATE = row["LPB_DATE"].ToString();
                //SNP_SSN_REASON = row["SNP_SSN_REASON"].ToString();
                //MST_NO_INPROG = row["MST_NO_INPROG"].ToString();
                //LPB_DATE = row["LPB_DATE"].ToString();
                //SNP_SSN_REASON = row["SNP_SSN_REASON"].ToString();
                //MST_NO_INPROG = row["MST_NO_INPROG"].ToString();
                //LPB_DATE = row["LPB_DATE"].ToString();
                //SNP_SSN_REASON = row["SNP_SSN_REASON"].ToString();
                //MST_NO_INPROG = row["MST_NO_INPROG"].ToString();

                PMR_AUTH_AMOUNT3 = row["PMR_AUTH_AMOUNT3"].ToString();
            }
       }
       
       #endregion

       #region Properties

       public string Agency { get; set; }
       public string Dept { get; set; }
       public string Program { get; set; }
       public string Year { get; set; }
       public string Appno { get; set; }
       public string Fname { get; set; }
       public string LName { get; set; }
       public string MName { get; set; }
       public string MST_STATE { get; set; }
       public string MST_CITY { get; set; }
       public string MST_STREET { get; set; }
       public string MST_SUFFIX { get; set; }
       public string MST_HN { get; set; }
       public string MST_APT { get; set; }
       public string MST_FLR { get; set; }
       public string MST_ZIP { get; set; }
       public string MST_ZIPPLUS { get; set; }
       public string MST_AREA { get; set; }
       public string MST_PHONE { get; set; }
       public string CASEVDD_NAME { get; set; }
       public string CASEVDD1_INDEXBY { get; set; }
       public string PMR_VENDOR { get; set; }
       public string PMR_BS { get; set; }
       public string PMR_AUTH_AMOUNT1 { get; set; }
       public string PMR_AUTH_AMOUNT2 { get; set; }
       public string MST_SOURCE { get; set; }
       public string PMR_AUTH_NUM { get; set; }
       public string PMR_AMOUNT_GAL { get; set; }
       public string PMR_VENDOR_PP { get; set; }
       public string PMR_DEL_DATE { get; set; }
       public string PMR_AMOUNT_DELIVERY { get; set; }
       public string PMR_AMOUNT_MISC { get; set; }
       public string PMR_AMOUNT_STARTUP { get; set; }
       public string PMR_AMOUNT_DOL { get; set; }
       public string PMR_LSTC_OPERATOR { get; set; }
       public string PMR_DATE_LSTC { get; set; }
       public string PMR_AUTH_DATE { get; set; }
       public string APPCount { get; set; }
       public string Amount { get; set; }
       public string AGY_7 { get; set; }
        //public string MST_NO_INPROG { get; set; }
        //public string LPB_DATE { get; set; }
        //public string SNP_SSN_REASON { get; set; }
        //public string MST_NO_INPROG { get; set; }
        //public string LPB_DATE { get; set; }
        //public string SNP_SSN_REASON { get; set; }
        //public string MST_NO_INPROG { get; set; }

        public string PMR_AUTH_AMOUNT3 { get; set; }
        #endregion
    }

    public class APPTREASNEntity
    {

        public APPTREASNEntity()
        {
            APTRSN_AGY = string.Empty;
            APTRSN_DEPT = string.Empty;
            APTRSN_PROG = string.Empty;
            APTRSN_YEAR = string.Empty;
            APTRSN_SITE = string.Empty;
            APTRSN_DATE = string.Empty;
            APTRSN_REASON = string.Empty;
            APTRSN_DATE_LSTC = string.Empty;
            APTRSN_LSTC_OPERATOR = string.Empty;
            APTRSN_DATE_ADD = string.Empty;
            APTRSN_ADD_OPERATOR = string.Empty;
            Mode = string.Empty;

        }
        public APPTREASNEntity(DataRow row)
        {
            if (row != null)
            {
                APTRSN_AGY = row["APTRSN_AGY"].ToString();
                APTRSN_DEPT = row["APTRSN_DEPT"].ToString();
                APTRSN_PROG = row["APTRSN_PROG"].ToString();
                APTRSN_YEAR = row["APTRSN_YEAR"].ToString();
                APTRSN_SITE = row["APTRSN_SITE"].ToString();
                APTRSN_DATE = row["APTRSN_DATE"].ToString();
                APTRSN_REASON = row["APTRSN_REASON"].ToString();
                APTRSN_DATE_LSTC = row["APTRSN_DATE_LSTC"].ToString();
                APTRSN_LSTC_OPERATOR = row["APTRSN_LSTC_OPERATOR"].ToString();
                APTRSN_DATE_ADD = row["APTRSN_DATE_ADD"].ToString();
                APTRSN_ADD_OPERATOR = row["APTRSN_ADD_OPERATOR"].ToString();

            }
        }
        public string APTRSN_AGY { get; set; }
        public string APTRSN_DEPT { get; set; }
        public string APTRSN_PROG { get; set; }
        public string APTRSN_YEAR { get; set; }
        public string APTRSN_SITE { get; set; }
        public string APTRSN_DATE { get; set; }
        public string APTRSN_REASON { get; set; }
        public string APTRSN_DATE_LSTC { get; set; }
        public string APTRSN_LSTC_OPERATOR { get; set; }
        public string APTRSN_DATE_ADD { get; set; }
        public string APTRSN_ADD_OPERATOR { get; set; }
        public string Mode { get; set; }
    }
}
