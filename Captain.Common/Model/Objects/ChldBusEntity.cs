using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Captain.Common.Model.Objects
{
    public class ChldBMEntity
    {

       #region Constructors

        public ChldBMEntity()
        {
            row_Type = string.Empty;
            ChldBMAgency = string.Empty;
            chldBMDept = string.Empty;
            ChldBMProg = string.Empty;
            ChldBMYear = string.Empty;
            ChldBMNumber = string.Empty;
            Desc = string.Empty;
            Make = string.Empty;
            ChldBM_Type = string.Empty;
            OL = string.Empty;
            OL_ID = string.Empty;
            OL_Date = string.Empty;
            Location1 = string.Empty;
            Location2 = string.Empty;
            Registration = string.Empty;
            Registration_Date = string.Empty;
            TelPhone = string.Empty;
            Last_Oil_Mile = string.Empty;
            Last_Oil_Date = string.Empty;
            ChldBM_Count = string.Empty;
            
            AddOperator = string.Empty;
            DateLstc= string.Empty;
            LstcOperator = string.Empty;       
            DateAdd = string.Empty;              
                      

        }

        public ChldBMEntity(bool Intialize)
        {
            if (Intialize)
            {
                row_Type = null;
                ChldBMAgency = null;
                chldBMDept = null;
                ChldBMProg = null;
                ChldBMYear = null;
                ChldBMNumber = null;
                Desc = null;
                Make = null;
                ChldBM_Type = null;
                OL = null;
                OL_ID = null;
                OL_Date = null;
                Location1 = null;
                Location2 = null;
                Registration = null;
                Registration_Date = null;
                TelPhone = null;
                Last_Oil_Mile = null;
                Last_Oil_Date = null;
                ChldBM_Count = null;

                AddOperator = null;
                DateLstc = null;
                LstcOperator = null;
                DateAdd = null;
            }


        }

        public ChldBMEntity(DataRow row)
        {
            if (row != null)
            {
                row_Type = "U";
                ChldBMAgency = row["CHLDBM_AGENCY"].ToString().Trim();
                chldBMDept = row["CHLDBM_DEPT"].ToString().Trim();
                ChldBMProg = row["CHLDBM_PROG"].ToString().Trim();
                //ChldMstYr = row["CHLDMST_YEAR"].ToString().Trim();
                ChldBMYear = row["CHLDBM_YEAR"].ToString().Trim();
               // FamilySeq = row["CHLDMST_FAMILY_SEQ"].ToString().Trim();
                ChldBMNumber = row["CHLDBM_NUMBER"].ToString().TrimEnd();
                Desc = row["CHLDBM_DESC"].ToString().Trim();
                Make = row["CHLDBM_MAKE"].ToString().Trim();
                ChldBM_Type = row["CHLDBM_TYPE"].ToString().Trim();
                OL = row["CHLDBM_OL"].ToString().Trim();
                OL_ID = row["CHLDBM_OL_ID"].ToString().Trim();
                OL_Date = row["CHLDBM_OL_DATE"].ToString().Trim();
                Location1 = row["CHLDBM_LOCATION1"].ToString().Trim();
                Location2 = row["CHLDBM_LOCATION2"].ToString().Trim();
                Registration = row["CHLDBM_REGISTRATION"].ToString().Trim();
                Registration_Date = row["CHLDBM_REGISTRATION_DATE"].ToString().Trim();
                TelPhone = row["CHLDBM_TEL"].ToString().Trim();
                Last_Oil_Mile = row["CHLDBM_LAST_OIL_MILE"].ToString().Trim();
                Last_Oil_Date = row["CHLDBM_LAST_OIL_DATE"].ToString().Trim();
                ChldBM_Count = row["CHLDBM_COUNT"].ToString().Trim();

                AddOperator = row["CHLDBM_ADD_OPERATOR"].ToString().Trim();
                DateLstc = row["CHLDBM_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["CHLDBM_LSTC_OPERATOR"].ToString().Trim();
                DateAdd = row["CHLDBM_DATE_ADD"].ToString().Trim();
                
                
            }

        }

    

        #endregion

        #region Properties
        public string row_Type { get; set; }
        public string ChldBMAgency { get; set; }
        public string chldBMDept { get; set; }
        public string ChldBMProg { get; set; }
        public string ChldBMYear { get; set; }
        public string ChldBMNumber { get; set; }
        public string Desc { get; set; }
        public string Make { get; set; }
        public string ChldBM_Type { get; set; }
        public string OL { get; set; }
        public string OL_ID { get; set; }
        public string OL_Date { get; set; }
        public string Location1 { get; set; }
        public string Location2 { get; set; }
        public string Registration { get; set; }
        public string Registration_Date { get; set; }
        public string TelPhone { get; set; }        
        public string Last_Oil_Mile { get; set; }
        public string Last_Oil_Date { get; set; }
        public string ChldBM_Count { get; set; }
        
        public string AddOperator { get; set; }
        public string DateLstc{ get; set; }
        public string LstcOperator { get; set; }       
        public string DateAdd { get; set; }
        public string Mode { get; set; }
        public string Sort { get; set; }
        

        #endregion

    }


    public class BusRTEntity
    {

        #region Constructors

        public BusRTEntity()
        {
            row_Type = string.Empty;
            BUSRT_AGENCY =string.Empty;
            BUSRT_DEPT =string.Empty;
            BUSRT_PROGRAM =string.Empty;
            BUSRT_YEAR =string.Empty;
            BUSRT_NUMBER =string.Empty;
            BUSRT_ROUTE =string.Empty;
            BUSRT_PICKUP_STARTS =string.Empty;
            BUSRT_ARRIVE_SCHOOL =string.Empty;
            BUSRT_LEAVES_SCHOOL =string.Empty;
            BUSRT_AREA_SERVED =string.Empty;
            BUSRT_DRIVER_NAME =string.Empty;
            BUSRT_DRIVER_DOB =string.Empty;
            BUSRT_DRIVER_TEL =string.Empty;
            BUSRT_DRIVER_LIC =string.Empty;
            BUSRT_DRIVER_LIC_DATE =string.Empty;
            BUSRT_DRIVER_LIC_7D_DATE =string.Empty;
            BUSRT_DRIVER_LIC_CLD_DATE =string.Empty;
            BUSRT_DRIVER_LIC_DPU_DATE =string.Empty;

            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;
            DateAdd = string.Empty;


        }

        public BusRTEntity(bool Intialize)
        {
            if (Intialize)
            {
                row_Type = null;
                BUSRT_AGENCY=null;
                BUSRT_DEPT=null;
                BUSRT_PROGRAM=null;
                BUSRT_YEAR=null;
                BUSRT_NUMBER=null;
                BUSRT_ROUTE=null;
                BUSRT_PICKUP_STARTS=null;
                BUSRT_ARRIVE_SCHOOL=null;
                BUSRT_LEAVES_SCHOOL=null;
                BUSRT_AREA_SERVED=null;
                BUSRT_DRIVER_NAME=null;
                BUSRT_DRIVER_DOB=null;
                BUSRT_DRIVER_TEL=null;
                BUSRT_DRIVER_LIC=null;
                BUSRT_DRIVER_LIC_DATE=null;
                BUSRT_DRIVER_LIC_7D_DATE=null;
                BUSRT_DRIVER_LIC_CLD_DATE=null;
                BUSRT_DRIVER_LIC_DPU_DATE=null;

                AddOperator = null;
                DateLstc = null;
                LstcOperator = null;
                DateAdd = null;
            }


        }

        public BusRTEntity(DataRow row)
        {
            if (row != null)
            {
                row_Type = "U";
                BUSRT_AGENCY = row["BUSRT_AGENCY"].ToString().Trim();
                BUSRT_DEPT = row["BUSRT_DEPT"].ToString().Trim();
                BUSRT_PROGRAM = row["BUSRT_PROGRAM"].ToString().Trim();
                BUSRT_YEAR = row["BUSRT_YEAR"].ToString().Trim();
                BUSRT_NUMBER = row["BUSRT_NUMBER"].ToString().Trim();
                BUSRT_ROUTE = row["BUSRT_ROUTE"].ToString().Trim();
                BUSRT_PICKUP_STARTS = row["BUSRT_PICKUP_STARTS"].ToString().Trim();
                BUSRT_ARRIVE_SCHOOL = row["BUSRT_ARRIVE_SCHOOL"].ToString().Trim();
                BUSRT_LEAVES_SCHOOL = row["BUSRT_LEAVES_SCHOOL"].ToString().Trim();
                BUSRT_AREA_SERVED = row["BUSRT_AREA_SERVED"].ToString().Trim();
                BUSRT_DRIVER_NAME = row["BUSRT_DRIVER_NAME"].ToString().Trim();
                BUSRT_DRIVER_DOB = row["BUSRT_DRIVER_DOB"].ToString().Trim();
                BUSRT_DRIVER_TEL = row["BUSRT_DRIVER_TEL"].ToString().Trim();
                BUSRT_DRIVER_LIC = row["BUSRT_DRIVER_LIC"].ToString().Trim();
                BUSRT_DRIVER_LIC_DATE = row["BUSRT_DRIVER_LIC_DATE"].ToString().Trim();
                BUSRT_DRIVER_LIC_7D_DATE = row["BUSRT_DRIVER_LIC_7D_DATE"].ToString().Trim();
                BUSRT_DRIVER_LIC_CLD_DATE = row["BUSRT_DRIVER_LIC_CLD_DATE"].ToString().Trim();
                BUSRT_DRIVER_LIC_DPU_DATE = row["BUSRT_DRIVER_LIC_DPU_DATE"].ToString().Trim();
                
                AddOperator = row["BUSRT_ADD_OPERATOR"].ToString().Trim();
                DateLstc = row["BUSRT_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["BUSRT_LSTC_OPERATOR"].ToString().Trim();
                DateAdd = row["BUSRT_DATE_ADD"].ToString().Trim();


            }

        }



        #endregion

        #region Properties
        public string row_Type { get; set; }
        public string BUSRT_AGENCY { get; set; }
        public string BUSRT_DEPT { get; set; }
        public string BUSRT_PROGRAM { get; set; }
        public string BUSRT_YEAR { get; set; }
        public string BUSRT_NUMBER { get; set; }
        public string BUSRT_ROUTE { get; set; }
        public string BUSRT_PICKUP_STARTS { get; set; }
        public string BUSRT_ARRIVE_SCHOOL { get; set; }
        public string BUSRT_LEAVES_SCHOOL { get; set; }
        public string BUSRT_AREA_SERVED { get; set; }
        public string BUSRT_DRIVER_NAME { get; set; }
        public string BUSRT_DRIVER_DOB { get; set; }
        public string BUSRT_DRIVER_TEL { get; set; }
        public string BUSRT_DRIVER_LIC { get; set; }
        public string BUSRT_DRIVER_LIC_DATE { get; set; }
        public string BUSRT_DRIVER_LIC_7D_DATE { get; set; }
        public string BUSRT_DRIVER_LIC_CLD_DATE { get; set; }
        public string BUSRT_DRIVER_LIC_DPU_DATE { get; set; }
        

        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string DateAdd { get; set; }
        public string Mode { get; set; }


        #endregion

    }

    public class BUSCEntity
    {

        #region Constructors

        public BUSCEntity()
        {
            row_Type = string.Empty;
            BUSC_AGENCY = string.Empty;
            BUSC_DEPT = string.Empty;
            BUSC_PROG = string.Empty;
            BUSC_YEAR = string.Empty;
            BUSC_NUMBER = string.Empty;
            BUSC_ROUTE = string.Empty;
            BUSC_CHILD = string.Empty;
            BUSC_PICKUP = string.Empty;
            BUSC_HOME = string.Empty;
            BUSC_APPLICATION = string.Empty;
            BUSC_COMMENTS = string.Empty;
            

            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;
            DateAdd = string.Empty;


        }

        public BUSCEntity(bool Intialize)
        {
            if (Intialize)
            {
                row_Type = null;
                BUSC_AGENCY = null;
                BUSC_DEPT = null;
                BUSC_PROG = null;
                BUSC_YEAR = null;
                BUSC_NUMBER = null;
                BUSC_ROUTE = null;
                BUSC_CHILD = null;
                BUSC_PICKUP = null;
                BUSC_HOME = null;
                BUSC_APPLICATION = null;
                BUSC_COMMENTS = null;
                

                AddOperator = null;
                DateLstc = null;
                LstcOperator = null;
                DateAdd = null;
            }


        }

        public BUSCEntity(DataRow row)
        {
            if (row != null)
            {
                row_Type = "U";
                BUSC_AGENCY = row["BUSC_AGENCY"].ToString().Trim();
                BUSC_DEPT = row["BUSC_DEPT"].ToString().Trim();
                BUSC_PROG = row["BUSC_PROG"].ToString().Trim();
                BUSC_YEAR = row["BUSC_YEAR"].ToString().Trim();
                BUSC_NUMBER = row["BUSC_NUMBER"].ToString().Trim();
                BUSC_ROUTE = row["BUSC_ROUTE"].ToString().Trim();
                BUSC_CHILD = row["BUSC_CHILD"].ToString().Trim();
                BUSC_PICKUP = row["BUSC_PICKUP"].ToString().Trim();
                BUSC_HOME = row["BUSC_HOME"].ToString().Trim();
                BUSC_APPLICATION = row["BUSC_APPLICATION"].ToString().Trim();
                BUSC_COMMENTS = row["BUSC_COMMENTS"].ToString().Trim();


                AddOperator = row["BUSC_ADD_OPERATOR"].ToString().Trim();
                DateLstc = row["BUSC_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["BUSC_LSTC_OPERATOR"].ToString().Trim();
                DateAdd = row["BUSC_DATE_ADD"].ToString().Trim();


            }

        }



        #endregion

        #region Properties
        public string row_Type { get; set; }
        public string BUSC_AGENCY { get; set; }
        public string BUSC_DEPT { get; set; }
        public string BUSC_PROG { get; set; }
        public string BUSC_YEAR { get; set; }
        public string BUSC_NUMBER { get; set; }
        public string BUSC_ROUTE { get; set; }
        public string BUSC_CHILD { get; set; }
        public string BUSC_PICKUP { get; set; }
        public string BUSC_HOME { get; set; }
        public string BUSC_APPLICATION { get; set; }
        public string BUSC_COMMENTS { get; set; }
        

        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string DateAdd { get; set; }
        public string Mode { get; set; }


        #endregion

    }

}
