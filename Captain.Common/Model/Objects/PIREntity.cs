using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Utilities;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraSpellChecker.Native;

namespace Captain.Common.Model.Objects
{
    public class PIRQUESTEntity
    {
        #region Constructors

        public PIRQUESTEntity()
        {
            Ques_Unique_ID  = Ques_section  = 
            Active_Status   = Fund_Type     = 
            Ques_Type       = Ques_Code     = 
            Ques_SCode      = Ques_Desc     =
            Ques_Seq        = Cnt_System_Cnt=
            Cnt_User_Cnt    = 
            Cnt_Agency      = Cnt_Dept      =
            Cnt_Prog        = Cnt_Year  = Ques_Year   = Mode = Ques_Position= Ques_A1Day = Ques_Bold = string.Empty; 

        }

        public PIRQUESTEntity(bool Initalize)
        {
            if (Initalize)
            {
                Ques_Unique_ID  = Ques_section  =
                Active_Status   = Fund_Type     =
                Ques_Type       = Ques_Code     =
                Ques_SCode      = Ques_Desc     =
                Ques_Seq        = Cnt_System_Cnt=
                Cnt_User_Cnt    = 
                Cnt_Agency      = Cnt_Dept      =
                Cnt_Prog = Cnt_Year = Ques_Year = Mode = Ques_Position = Ques_A1Day = Ques_Bold = null;


            }
        }

        public PIRQUESTEntity(DataRow Row, string Count_Join_SW)
        {
            if (Row != null)
            {
              
                Ques_Unique_ID  = Row["PIRQUEST_UNIQUE_ID"].ToString().Trim();
                Ques_section = Row["PIRQUEST_SECTION"].ToString().Trim();
                Active_Status = Row["PIRQUEST_ACTIVE"].ToString().Trim();
                Fund_Type = Row["PIRQUEST_FUND"].ToString().Trim();
                Ques_Type = Row["PIRQUEST_QUE_TYPE"].ToString().Trim();
                Ques_Code = Row["PIRQUEST_QUE_CODE"].ToString().Trim();
                Ques_SCode = Row["PIRQUEST_SQUE_CODE"].ToString().Trim();
                Ques_Desc = Row["PIRQUEST_QUE_DESC"].ToString().Trim();
                Ques_Seq = Row["PIRQUEST_QUE_SECTION"].ToString().Trim();
                Ques_Position = Row["PIRQUEST_QUE_POS"].ToString().Trim();
                Ques_A1Day = Row["PIRQUEST_A1DAY"].ToString().Trim();
                Ques_Bold = Row["PIRQUEST_BOLD"].ToString().Trim();
                Cnt_System_Cnt = Cnt_User_Cnt = string.Empty;
                if (Count_Join_SW == "Y")
                {
                    Cnt_Agency = Row["PIRCOUNT_AGENCY"].ToString().Trim();
                    Cnt_Dept = Row["PIRCOUNT_DEPT"].ToString().Trim();
                    Cnt_Prog = Row["PIRCOUNT_PROG"].ToString().Trim();
                    Cnt_Year = Row["PIRCOUNT_YEAR"].ToString().Trim();
                    Cnt_Site = Row["PIRCOUNT_SITE"].ToString().Trim();
                    Cnt_System_Cnt = Row["PIRCOUNT_SYS_COUNT"].ToString().Trim();
                    Cnt_User_Cnt = Row["PIRCOUNT_USER_COUNT"].ToString().Trim();
                }
            }
        }

        public PIRQUESTEntity(DataRow Row)
        {
            if (Row != null)
            {
                Ques_Unique_ID = Row["PIRQUEST_UNIQUE_ID"].ToString().Trim();
                Ques_section = Row["PIRQUEST_SECTION"].ToString().Trim();
                Active_Status = Row["PIRQUEST_ACTIVE"].ToString().Trim();
                Fund_Type = Row["PIRQUEST_FUND"].ToString().Trim();
                Ques_Type = Row["PIRQUEST_QUE_TYPE"].ToString().Trim();
                Ques_Code = Row["PIRQUEST_QUE_CODE"].ToString().Trim();
                Ques_SCode = Row["PIRQUEST_SQUE_CODE"].ToString().Trim();
                Ques_Desc = Row["PIRQUEST_QUE_DESC"].ToString().Trim();
                Ques_Seq = Row["PIRQUEST_QUE_SECTION"].ToString().Trim();
                Ques_Year = Row["PIRQUEST_YEAR"].ToString().Trim();
                Ques_Position = Row["PIRQUEST_QUE_POS"].ToString().Trim();
                Ques_A1Day = Row["PIRQUEST_A1DAY"].ToString().Trim();
                Ques_Bold = Row["PIRQUEST_BOLD"].ToString().Trim();
                
            }
        }

 

        #endregion

        #region Properties

        public string Ques_Unique_ID { get; set; }
        public string Ques_section { get; set; }
        public string Ques_Seq { get; set; }
        public string Active_Status { get; set; }
        public string Fund_Type { get; set; }
        public string Ques_Type { get; set; }
        public string Ques_Code { get; set; }
        public string Ques_SCode { get; set; }
        public string Ques_Desc { get; set; }
        public string Ques_Position { get; set; }
        public string Ques_A1Day { get; set; }
        public string Ques_Bold { get; set; }
        
        public string Ques_Year { get; set; }
        public string Mode { get; set; }

        public string Cnt_Agency { get; set; }
        public string Cnt_Dept { get; set; }
        public string Cnt_Prog { get; set; }
        public string Cnt_Year { get; set; }
        public string Cnt_Site { get; set; }
        public string Cnt_System_Cnt { get; set; }
        public string Cnt_User_Cnt { get; set; }

        #endregion

    }

    public class PIR_New_Count_Entity
    {
        #region Constructors
         public PIR_New_Count_Entity()
        {
            Ques_ID = Ques_Seq =
            Ques_code =    Ques_Scode =
            Ques_Type = Ques_Desc =
            System_Cnt = User_Cnt = string.Empty;
        }

        public PIR_New_Count_Entity(bool Initalize)
        {
            if (Initalize)
            {
                Ques_ID = Ques_Seq =
                Ques_code =    Ques_Scode =
                Ques_Type = Ques_Desc =
                System_Cnt = User_Cnt = null;
            }
        }

        public PIR_New_Count_Entity(DataRow Row)
        {
            if (Row != null)
            {
                Ques_ID = Row["F_App_Ques_ID"].ToString();
                Ques_Seq = Row["F_App_Ques_Sec"].ToString();
                Ques_code = Row["F_App_Ques_Code"].ToString();
                Ques_Scode = Row["F_App_Ques_SCode"].ToString();
                Ques_Type = Row["F_App_Ques_Type"].ToString();
                Ques_Desc = Row["F_Ques_Desc"].ToString();
                System_Cnt = Row["F_Max_Cnt"].ToString();
                User_Cnt = Row["F_Max_User_Cnt"].ToString();
            }
        }



        #endregion

        #region Properties

        public string Ques_ID { get; set; }
        public string Ques_Seq { get; set; }
        public string Ques_code { get; set; }
        public string Ques_Scode { get; set; }
        public string Ques_Type { get; set; }
        public string Ques_Desc { get; set; }
        public string System_Cnt { get; set; }
        public string User_Cnt { get; set; }

        #endregion

    }

    public class PIRMISCEntity
    {
        #region Constructors

    public class PIRCOUNTEntity
    {
        #region Constructors

        public PIRCOUNTEntity()
        {
          Agency = Dept = Prog = Year =
          Site = Ques_Fund = Ques_ID =
          Ques_section = System_Cnt =
          User_Cnt = Agy_Flag =
          Agy_Code = Add_Date =
          Add_Operator = Lstc_Date =
          Lstc_Operator = string.Empty;
        }

        public PIRCOUNTEntity(bool Initalize)
        {
            if (Initalize)
            {
                Agency = Dept = Prog = Year = 
                Site = Ques_Fund = Ques_ID = 
                Ques_section = System_Cnt = 
                User_Cnt = Agy_Flag = 
                Agy_Code = Add_Date = 
                Add_Operator = Lstc_Date = 
                Lstc_Operator = null;
            }
        }

        public PIRCOUNTEntity(DataRow Row, string Count_Join_SW)
        {
            if (Row != null)
            {
                Agency = Row["PIRCOUNT_AGENCY"].ToString();
                Dept = Row["PIRCOUNT_DEPT"].ToString();
                Prog = Row["PIRCOUNT_PROG"].ToString();
                Year = Row["PIRCOUNT_YEAR"].ToString();
                Site = Row["PIRCOUNT_SITE"].ToString();
                Ques_Fund = Row["PIRCOUNT_Q_FUND"].ToString();
                Ques_ID = Row["PIRCOUNT_Q_UID"].ToString();
                Ques_section = Row["PIRCOUNT_Q_SEC"].ToString();
                System_Cnt = Row["PIRCOUNT_SYS_COUNT"].ToString();
                User_Cnt = Row["PIRCOUNT_USER_COUNT"].ToString();
                Agy_Flag = Row["PIRCOUNT_Q_AGYFLAG"].ToString();
                Agy_Code = Row["PIRCOUNT_Q_AGYCODE"].ToString();
                Add_Date = Row["PIRCOUNT_DATE_ADD"].ToString();
                Add_Operator = Row["PIRCOUNT_ADD_OPERATOR"].ToString();
                Lstc_Date = Row["PIRCOUNT_DATE_LSTC"].ToString();
                Lstc_Operator = Row["PIRCOUNT_LSTC_OPERATOR"].ToString();
            }
        }

        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string Year { get; set; }
        public string Site { get; set; }
        public string Ques_Fund { get; set; }
        public string Ques_ID { get; set; }
        public string Ques_section { get; set; }
        public string System_Cnt { get; set; }
        public string User_Cnt { get; set; }
        public string Agy_Flag { get; set; }
        public string Agy_Code { get; set; }
        public string Add_Date { get; set; }
        public string Add_Operator { get; set; }
        public string Lstc_Date { get; set; }
        public string Lstc_Operator { get; set; }

        #endregion
    }


       
        public PIRMISCEntity()
        {
            Agency = Dept = Mode =
            Prog = Year =
            C9_Agy_Type = Sites_Flag =
            Sites = Taskfund = string.Empty;
            //Agy_1 = Agy_2 = 

        }

        public PIRMISCEntity(bool Initalize)
        {
            if (Initalize)
            {
                Agency = Mode =
                Dept = 
                Prog = 
                Year = 
                C9_Agy_Type = 
                Sites_Flag =
                Sites = Taskfund = null;
                //Agy_1 = Agy_2 = 

            }
        }

        public PIRMISCEntity(DataRow Row)
        {
            if (Row != null)
            {
                Mode = "Edit";
                Agency = Row["PIRMISC_AGENCY"].ToString();
                Dept = Row["PIRMISC_DEPT"].ToString();
                Prog = Row["PIRMISC_PROG"].ToString();
                Year = Row["PIRMISC_YEAR"].ToString();
                C9_Agy_Type = Row["PIRMISC_C9_AGE_TYPE"].ToString();
                Sites_Flag = Row["PIRMISC_SITES_FLAG"].ToString();
                Sites = Row["PIRMISC_SITES"].ToString();
                Taskfund = Row["PIRMISC_TASK_FUND"].ToString();
                
            }
        }



        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string Year { get; set; }
        public string C9_Agy_Type { get; set; }
        public string Sites_Flag { get; set; }
        public string Sites { get; set; }
        public string Taskfund { get; set; }
        //public string Agy_1 { get; set; }
        //public string Agy_2 { get; set; }


        public string Mode { get; set; }
        
        #endregion

    }

    public class PIRCOUNTEntity
    {
        #region Constructors

        public PIRCOUNTEntity()
        {
            Agency = Dept = Prog = Year =
            Site = Ques_Fund = Ques_ID =
            Ques_section = System_Cnt =
            User_Cnt = Agy_Flag =
            Agy_Code = Add_Date =
            Add_Operator = Lstc_Date =
            Lstc_Operator = string.Empty;
        }

        public PIRCOUNTEntity(bool Initalize)
        {
            if (Initalize)
            {
                Agency = Dept = Prog = Year =
                Site = Ques_Fund = Ques_ID =
                Ques_section = System_Cnt =
                User_Cnt = Agy_Flag =
                Agy_Code = Add_Date =
                Add_Operator = Lstc_Date =
                Lstc_Operator = null;
            }
        }

        public PIRCOUNTEntity(DataRow Row, string Count_Join_SW)
        {
            if (Row != null)
            {
                Agency = Row["PIRCOUNT_AGENCY"].ToString();
                Dept = Row["PIRCOUNT_DEPT"].ToString();
                Prog = Row["PIRCOUNT_PROG"].ToString();
                Year = Row["PIRCOUNT_YEAR"].ToString();
                Site = Row["PIRCOUNT_SITE"].ToString();
                Ques_Fund = Row["PIRCOUNT_Q_FUND"].ToString();
                Ques_ID = Row["PIRCOUNT_Q_UID"].ToString();
                Ques_section = Row["PIRCOUNT_Q_SEC"].ToString();
                System_Cnt = Row["PIRCOUNT_SYS_COUNT"].ToString();
                User_Cnt = Row["PIRCOUNT_USER_COUNT"].ToString();
                Agy_Flag = Row["PIRCOUNT_Q_AGYFLAG"].ToString();
                Agy_Code = Row["PIRCOUNT_Q_AGYCODE"].ToString();
                Add_Date = Row["PIRCOUNT_DATE_ADD"].ToString();
                Add_Operator = Row["PIRCOUNT_ADD_OPERATOR"].ToString();
                Lstc_Date = Row["PIRCOUNT_DATE_LSTC"].ToString();
                Lstc_Operator = Row["PIRCOUNT_LSTC_OPERATOR"].ToString();
            }
        }

        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string Year { get; set; }
        public string Site { get; set; }
        public string Ques_Fund { get; set; }
        public string Ques_ID { get; set; }
        public string Ques_section { get; set; }
        public string System_Cnt { get; set; }
        public string User_Cnt { get; set; }
        public string Agy_Flag { get; set; }
        public string Agy_Code { get; set; }
        public string Add_Date { get; set; }
        public string Add_Operator { get; set; }
        public string Lstc_Date { get; set; }
        public string Lstc_Operator { get; set; }

        #endregion
    }

    #region Added by Vikash on 04/05/2024 as per enhancement in PIR 2.0 document

    public class PIRQUEST2Entity
    {
        #region Constructors

        public PIRQUEST2Entity()
        {
            PIR_Ques_Year = string.Empty;

            PIR_Ques_Unique_ID = string.Empty;
            PIR_Section = string.Empty;
            PIR_Fund_Type = string.Empty;
            PIR_Ques_Position = string.Empty;
            PIR_Ques_Code = string.Empty;
            PIR_Ques_SCode = string.Empty;
            PIR_Ques_Section = string.Empty;
            PIR_Ques_Type = string.Empty;
            PIR_Ques_Desc = string.Empty;
            PIR_Ques_Col1 = string.Empty;
            PIR_Ques_Col2 = string.Empty;
            PIR_Ques_Col_Head_Top = string.Empty;
            PIR_Active = string.Empty;
            PIR_Ques_A1Day = string.Empty;
            PIR_Ques_Bold = string.Empty;
            PIR_Ques_ID = string.Empty;

            Col1_Desc = string.Empty;
            Col2_Desc = string.Empty;

        }

        public PIRQUEST2Entity(bool Initalize)
        {
            if (Initalize)
            {
                PIR_Ques_Year = null;

                PIR_Ques_Unique_ID = null;
                PIR_Section = null;
                PIR_Fund_Type = null;
                PIR_Ques_Position = null;
                PIR_Ques_Code = null;
                PIR_Ques_SCode = null;
                PIR_Ques_Section = null;
                PIR_Ques_Type = null;
                PIR_Ques_Desc = null;
                PIR_Ques_Col1 = null;
                PIR_Ques_Col2 = null;
                PIR_Ques_Col_Head_Top = null;
                PIR_Active = null;
                PIR_Ques_A1Day = null;
                PIR_Ques_Bold = null;
                PIR_Ques_ID = null;

                Col1_Desc = null;
                Col2_Desc = null;

                Mode = null;
            }
        }

        public PIRQUEST2Entity(DataRow Row, string Count_Join_SW)
        {
            if (Row != null)
            {

                PIR_Ques_Year = Row["PIRQUEST_YEAR"].ToString().Trim();
                PIR_Ques_Unique_ID = Row["PIRQUEST_UNIQUE_ID"].ToString().Trim();
                PIR_Section = Row["PIRQUEST_SECTION"].ToString().Trim();
                PIR_Fund_Type = Row["PIRQUEST_FUND"].ToString().Trim();
                PIR_Ques_Position = Row["PIRQUEST_QUE_POS"].ToString().Trim();
                PIR_Ques_Code = Row["PIRQUEST_QUE_CODE"].ToString().Trim();
                PIR_Ques_SCode = Row["PIRQUEST_SQUE_CODE"].ToString().Trim();
                PIR_Ques_Section = Row["PIRQUEST_QUE_SECTION"].ToString().Trim();
                PIR_Ques_Type = Row["PIRQUEST_QUE_TYPE"].ToString().Trim();
                PIR_Ques_Desc = Row["PIRQUEST_QUE_DESC"].ToString().Trim();
                PIR_Ques_Col1 = Row["PIRQUEST_COLHEAD1"].ToString().Trim();
                PIR_Ques_Col2 = Row["PIRQUEST_COLHEAD2"].ToString().Trim();
                PIR_Ques_Col_Head_Top = Row["PIRQUEST_COLHEAD_TOP"].ToString().Trim();
                PIR_Active = Row["PIRQUEST_ACTIVE"].ToString().Trim();
                PIR_Ques_A1Day = Row["PIRQUEST_A1DAY"].ToString().Trim();
                PIR_Ques_Bold = Row["PIRQUEST_BOLD"].ToString().Trim();
                PIR_Ques_ID = Row["PIRQUEST_QID"].ToString().Trim();
                Col1_Desc= Row["COL1_DESC"].ToString().Trim();
                Col2_Desc = Row["COL2_DESC"].ToString().Trim();

            }
        }

        public PIRQUEST2Entity(DataRow Row)
        {
            if (Row != null)
            {

                PIR_Ques_Year = Row["PIRQUEST_YEAR"].ToString().Trim();
                PIR_Ques_Unique_ID = Row["PIRQUEST_UNIQUE_ID"].ToString().Trim();
                PIR_Section = Row["PIRQUEST_SECTION"].ToString().Trim();
                PIR_Fund_Type = Row["PIRQUEST_FUND"].ToString().Trim();
                PIR_Ques_Position = Row["PIRQUEST_QUE_POS"].ToString().Trim();
                PIR_Ques_Code = Row["PIRQUEST_QUE_CODE"].ToString().Trim();
                PIR_Ques_SCode = Row["PIRQUEST_SQUE_CODE"].ToString().Trim();
                PIR_Ques_Section = Row["PIRQUEST_QUE_SECTION"].ToString().Trim();
                PIR_Ques_Type = Row["PIRQUEST_QUE_TYPE"].ToString().Trim();
                PIR_Ques_Desc = Row["PIRQUEST_QUE_DESC"].ToString().Trim();
                PIR_Ques_Col1 = Row["PIRQUEST_COLHEAD1"].ToString().Trim();
                PIR_Ques_Col2 = Row["PIRQUEST_COLHEAD2"].ToString().Trim();
                PIR_Ques_Col_Head_Top = Row["PIRQUEST_COLHEAD_TOP"].ToString().Trim();
                PIR_Active = Row["PIRQUEST_ACTIVE"].ToString().Trim();
                PIR_Ques_A1Day = Row["PIRQUEST_A1DAY"].ToString().Trim();
                PIR_Ques_Bold = Row["PIRQUEST_BOLD"].ToString().Trim();
                PIR_Ques_ID = Row["PIRQUEST_QID"].ToString().Trim();

            }
        }

        #endregion

        #region Properties

        public string PIR_Ques_Unique_ID
        {
            get; set;
        }
        public string PIR_Section
        {
            get; set;
        }
        public string PIR_Active
        {
            get; set;
        }
        public string PIR_Fund_Type
        {
            get; set;
        }
        public string PIR_Ques_Type
        {
            get; set;
        }
        public string PIR_Ques_Code
        {
            get; set;
        }
        public string PIR_Ques_SCode
        {
            get; set;
        }
        public string PIR_Ques_Desc
        {
            get; set;
        }
        public string PIR_Ques_Position
        {
            get; set;
        }
        public string PIR_Ques_A1Day
        {
            get; set;
        }
        public string PIR_Ques_Bold
        {
            get; set;
        }

        public string PIR_Ques_Year
        {
            get; set;
        }
        public string Mode
        {
            get; set;
        }

        public string PIR_Ques_ID
        {
            get; set;
        }
        public string PIR_Ques_Section
        {
            get; set;
        }

        public string PIR_Ques_Col1
        {
            get; set;
        }

        public string PIR_Ques_Col2
        {
            get; set;
        }

        public string Col1_Desc
        {
            get; set;
        }

        public string Col2_Desc
        {
            get; set;
        }
        public string PIR_Ques_Col_Head_Top
        {
            get; set;
        }

        #endregion

    }

    // Added on 05/06/2024 to save PIR Counts
    public class PIRCOUNT2Entity
    {
        #region Constructors

        public PIRCOUNT2Entity()
        {
            Agency = Dept = Prog = Year =
           Site = Ques_Fund = Ques_ID =
            Column1 = Column2 = Agy_Flag = Agy_Code = 
            Add_Operator =
            Lstc_Operator = string.Empty;
        }

        public PIRCOUNT2Entity(bool Initalize)
        {
            if (Initalize)
            {
                Agency = Dept = Prog = Year =
               Site = Ques_Fund = Ques_ID =
                Column1 = Column2 = Agy_Flag = Agy_Code = 
                Add_Operator =
                Lstc_Operator = null;
            }
        }

        public PIRCOUNT2Entity(DataRow Row, string Count_Join_SW)
        {
            if (Row != null)
            {
                Agency = Row["PIRCOUNT_AGENCY"].ToString();
                Dept = Row["PIRCOUNT_DEPT"].ToString();
                Prog = Row["PIRCOUNT_PROG"].ToString();
                Year = Row["PIRCOUNT_YEAR"].ToString();
                Site = Row["PIRCOUNT_SITE"].ToString();
                Ques_Fund = Row["PIRCOUNT_Q_FUND"].ToString();
                Ques_ID = Row["PIRCOUNT_Q_ID"].ToString();
                Column1 = Row["PIRCOUNT_Col1_COUNT"].ToString();
                Column2 = Row["PIRCOUNT_Col2_COUNT"].ToString();
                Agy_Flag = Row["PIRCOUNT_Q_AGYFLAG"].ToString();
                Agy_Code = Row["PIRCOUNT_Q_AGYCODE"].ToString();
                Add_Operator = Row["PIRCOUNT_ADD_OPERATOR"].ToString();
                Lstc_Operator = Row["PIRCOUNT_LSTC_OPERATOR"].ToString();
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
        public string Site
        {
            get; set;
        }
        public string Ques_Fund
        {
            get; set;
        }
        public string Ques_ID
        {
            get; set;
        }
        public string Column1
        {
            get; set;
        }
        public string Column2
        {
            get; set;
        }
        public string Agy_Flag
        {
            get; set;
        }
        public string Agy_Code
        {
            get; set;
        }
        public string Add_Operator
        {
            get; set;
        }
        public string Lstc_Operator
        {
            get; set;
        }

        #endregion
    }

    #endregion

}
