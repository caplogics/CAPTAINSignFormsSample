using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Captain.Common.Model.Objects
{
    /// <summary>
    /// Entity Object
    /// </summary>
    [Serializable]


    public class INKINDMSTEntity
    {
        #region Constructors

        public INKINDMSTEntity()
        {
            Rec_Type = string.Empty;
            Agency = string.Empty;
            Dept = string.Empty;
            Prog = string.Empty;
            CODE = string.Empty;
            TYPE = string.Empty;
            DONOR_NAME = string.Empty;
            CHILD_FNAME = string.Empty;
            CHILD_LNAME = string.Empty;
            ADDRESS = string.Empty;
            PHONE = string.Empty;
            DOB = string.Empty;
            SEX = string.Empty;
            RACE = string.Empty;
            ETHNICITY = string.Empty;
            EDUCATION = string.Empty;
            VETERAN = string.Empty;
            DISABLED = string.Empty;
            FARMER = string.Empty;
            US_CITIZEN = string.Empty;
            HS_PARENT = string.Empty;
            DATE_LSTC = string.Empty;
            LSTC_OPERATOR = string.Empty;
            DATE_ADD = string.Empty;
            ADD_OPERATOR = string.Empty;
        }

        public INKINDMSTEntity(bool Initialize)
        {
            Rec_Type = null;
            Agency = null;
            Dept = null;
            Prog = null;
            CODE = null;
            TYPE = null;
            DONOR_NAME = null;
            CHILD_FNAME = null;
            CHILD_LNAME = null;
            ADDRESS = null;
            PHONE = null;
            DOB = null;
            SEX = null;
            RACE = null;
            ETHNICITY = null;
            EDUCATION = null;
            VETERAN = null;
            DISABLED = null;
            FARMER = null;
            US_CITIZEN = null;
            HS_PARENT = null;
            DATE_LSTC = null;
            LSTC_OPERATOR = null;
            DATE_ADD = null;
            ADD_OPERATOR = null;
        }

        public INKINDMSTEntity(DataRow CASESPM2)
        {
            if (CASESPM2 != null)
            {
                DataRow row = CASESPM2;
                Rec_Type = "U";
                Agency = row["IKM_AGENCY"].ToString();
                Dept = row["IKM_DEPT"].ToString();
                Prog = row["IKM_PROGRAM"].ToString();
                CODE = row["IKM_CODE"].ToString();
                TYPE = row["IKM_TYPE"].ToString();
                DONOR_NAME = row["IKM_DONOR_NAME"].ToString();
                CHILD_FNAME = row["IKM_CHILD_FNAME"].ToString();
                CHILD_LNAME = row["IKM_CHILD_LNAME"].ToString();
                ADDRESS = row["IKM_ADDRESS"].ToString();
                PHONE = row["IKM_PHONE"].ToString();
                DOB = row["IKM_DOB"].ToString();
                SEX = row["IKM_SEX"].ToString();
                RACE = row["IKM_RACE"].ToString();
                ETHNICITY = row["IKM_ETHNICITY"].ToString();
                EDUCATION = row["IKM_EDUCATION"].ToString();
                VETERAN = row["IKM_VETERAN"].ToString();
                DISABLED = row["IKM_DISABLED"].ToString();
                FARMER = row["IKM_FARMER"].ToString();
                US_CITIZEN = row["IKM_US_CITIZEN"].ToString();
                HS_PARENT = row["IKM_HS_PARENT"].ToString();
                DATE_LSTC = row["IKM_DATE_LSTC"].ToString();
                LSTC_OPERATOR = row["IKM_LSTC_OPERATOR"].ToString();
                DATE_ADD = row["IKM_DATE_ADD"].ToString();
                ADD_OPERATOR = row["IKM_ADD_OPERATOR"].ToString();
            }
        }


        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string CODE { get; set; }
        public string TYPE { get; set; }
        public string DONOR_NAME { get; set; }
        public string CHILD_FNAME { get; set; }
        public string CHILD_LNAME { get; set; }
        public string ADDRESS { get; set; }
        public string PHONE { get; set; }
        public string DOB { get; set; }
        public string SEX { get; set; }
        public string RACE { get; set; }
        public string ETHNICITY { get; set; }
        public string EDUCATION { get; set; }
        public string VETERAN { get; set; }
        public string DISABLED { get; set; }
        public string FARMER { get; set; }
        public string US_CITIZEN { get; set; }
        public string HS_PARENT { get; set; }

        public string DATE_LSTC { get; set; }
        public string LSTC_OPERATOR { get; set; }
        public string DATE_ADD { get; set; }
        public string ADD_OPERATOR { get; set; }
        public string Mode { get; set; }
        #endregion
    }


    public class INKINDDTLEntity
    {
        #region Constructors

        public INKINDDTLEntity()
        {
            Rec_Type = string.Empty;
            Agency = string.Empty;
            Dept = string.Empty;
            Prog = string.Empty;
            CODE = string.Empty;
            SEQ = string.Empty;
            SERVICE_TYPE = string.Empty;
            SERVICE_DATE = string.Empty;
            START_TIME = string.Empty;
            END_TIME = string.Empty;
            SERVICE_TIME = string.Empty;
            TRANSPORT_TIME = string.Empty;
            MILES_DRIVEN = string.Empty;
            TOTAL_SERVICE = string.Empty;
            TOTAL_MILEAGE = string.Empty;
            TOTAL_INKIND = string.Empty;            
            Date_LSTC = string.Empty;
            LSTC_Operator = string.Empty;
            Date_ADD = string.Empty;
            ADD_Operator = string.Empty;
            Site = string.Empty;
        }

        public INKINDDTLEntity(bool Initialize)
        {
            Rec_Type = null;
            Agency = null;
            Dept = null;
            Prog = null;
            CODE = null;
            SEQ = null;
            SERVICE_TYPE = null;
            SERVICE_DATE = null;
            START_TIME = null;
            END_TIME = null;
            SERVICE_TIME = null;
            TRANSPORT_TIME = null;
            MILES_DRIVEN = null;
            TOTAL_SERVICE = null;
            TOTAL_MILEAGE = null;
            TOTAL_INKIND = null;    
            Date_LSTC = null;
            LSTC_Operator = null;
            Date_ADD = null;
            ADD_Operator = null;
            Site = null;
        }

        public INKINDDTLEntity(DataRow CASESPM2)
        {
            if (CASESPM2 != null)
            {
                DataRow row = CASESPM2;
                Rec_Type = "U";
                Agency = row["IKD_AGENCY"].ToString();
                Dept = row["IKD_DEPT"].ToString();
                Prog = row["IKD_PROGRAM"].ToString();
                CODE = row["IKD_CODE"].ToString();
                SEQ = row["IKD_SEQ"].ToString();
                SERVICE_TYPE = row["IKD_SERVICE_TYPE"].ToString();
                SERVICE_DATE = row["IKD_SERVICE_DATE"].ToString();
                START_TIME = row["IKD_START_TIME"].ToString();
                END_TIME = row["IKD_END_TIME"].ToString();
                SERVICE_TIME = row["IKD_SERVICE_TIME"].ToString();
                TRANSPORT_TIME = row["IKD_TRANSPORT_TIME"].ToString();
                MILES_DRIVEN = row["IKD_MILES_DRIVEN"].ToString();
                TOTAL_SERVICE = row["IKD_TOTAL_SERVICE"].ToString();
                TOTAL_MILEAGE = row["IKD_TOTAL_MILEAGE"].ToString();
                TOTAL_INKIND = row["IKD_TOTAL_INKIND"].ToString();

                Date_LSTC = row["IKD_DATE_LSTC"].ToString();
                LSTC_Operator = row["IKD_LSTC_OPERATOR"].ToString();
                Date_ADD = row["IKD_DATE_ADD"].ToString();
                ADD_Operator = row["IKD_ADD_OPERATOR"].ToString();

                Site= row["IKD_SITE"].ToString();
            }
        }


        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string CODE { get; set; }
        public string SEQ { get; set; }
        public string SERVICE_TYPE { get; set; }
        public string SERVICE_DATE { get; set; }
        public string START_TIME { get; set; }
        public string END_TIME { get; set; }
        public string SERVICE_TIME { get; set; }
        public string TRANSPORT_TIME { get; set; }
        public string MILES_DRIVEN { get; set; } 
        public string TOTAL_SERVICE { get; set; } 
        public string TOTAL_MILEAGE { get; set; } 
        public string TOTAL_INKIND { get; set; } 
        public string Date_LSTC { get; set; }
        public string LSTC_Operator { get; set; }
        public string Date_ADD { get; set; }
        public string ADD_Operator { get; set; }
        public string Site { get; set; }
        public string Mode { get; set; }
        #endregion
    }


    public class INKINDDTLSEntity
    {
        #region Constructors

        public INKINDDTLSEntity()
        {
            IKDL_AGENCY = string.Empty;
            IKDL_DEPT = string.Empty;
            IKDL_PROGRAM = string.Empty;
            IKDL_CODE = string.Empty;
            IKDL_SEQ = string.Empty;
            IKDL_SERVICE_TYPE = string.Empty;
            IKDL_SERVICE_AREA = string.Empty;
            IKDL_SERVICE_DATE = string.Empty;
            IKDL_PROG = string.Empty;
            IKDL_SITE = string.Empty;
            IKDL_ROOM = string.Empty;
            IKDL_AMPM = string.Empty;
            IKDL_FATHER = string.Empty;
            IKDL_CLASS_ROOM = string.Empty;
            IKDL_QUANTITY = string.Empty;
            IKDL_RATE = string.Empty;
            IKDL_UOM = string.Empty;
            IKDL_AMOUNT = string.Empty;
            IKDL_DATE_ADD = string.Empty;
            IKDL_ADD_OPERATOR = string.Empty;
            IKDL_DATE_LSTC = string.Empty;
            IKDL_LSTC_OPERATOR = string.Empty;

        }

        public INKINDDTLSEntity(bool Initialize)
        {
            IKDL_AGENCY = null;
            IKDL_DEPT = null;
            IKDL_PROGRAM = null;
            IKDL_CODE = null;
            IKDL_SEQ = null;
            IKDL_SERVICE_TYPE = null;
            IKDL_SERVICE_AREA = null;
            IKDL_SERVICE_DATE = null;
            IKDL_PROG = null;
            IKDL_SITE = null;
            IKDL_ROOM = null;
            IKDL_AMPM = null;
            IKDL_FATHER = null;
            IKDL_CLASS_ROOM = null;
            IKDL_QUANTITY = null;
            IKDL_RATE = null;
            IKDL_UOM = null;
            IKDL_AMOUNT = null;
            IKDL_DATE_ADD = null;
            IKDL_ADD_OPERATOR = null;
            IKDL_DATE_LSTC = null;
            IKDL_LSTC_OPERATOR = null;

        }

        public INKINDDTLSEntity(DataRow CASESPM2)
        {
            if (CASESPM2 != null)
            {
                DataRow row = CASESPM2;
                IKDL_AGENCY = row["IKDL_AGENCY"].ToString();
                IKDL_DEPT = row["IKDL_DEPT"].ToString();
                IKDL_PROGRAM = row["IKDL_PROGRAM"].ToString();
                IKDL_CODE = row["IKDL_CODE"].ToString();
                IKDL_SEQ = row["IKDL_SEQ"].ToString();
                IKDL_SERVICE_TYPE = row["IKDL_SERVICE_TYPE"].ToString();
                IKDL_SERVICE_AREA = row["IKDL_SERVICE_AREA"].ToString();
                IKDL_SERVICE_DATE = row["IKDL_SERVICE_DATE"].ToString();
                IKDL_PROG = row["IKDL_PROG"].ToString();
                IKDL_SITE = row["IKDL_SITE"].ToString();
                IKDL_ROOM = row["IKDL_ROOM"].ToString();
                IKDL_AMPM = row["IKDL_AMPM"].ToString();
                IKDL_FATHER = row["IKDL_FATHER"].ToString();
                IKDL_CLASS_ROOM = row["IKDL_CLASS_ROOM"].ToString();
                IKDL_QUANTITY = row["IKDL_QUANTITY"].ToString();
                IKDL_RATE = row["IKDL_RATE"].ToString();
                IKDL_UOM = row["IKDL_UOM"].ToString();
                IKDL_AMOUNT = row["IKDL_AMOUNT"].ToString();
                IKDL_DATE_ADD = row["IKDL_DATE_ADD"].ToString();
                IKDL_ADD_OPERATOR = row["IKDL_ADD_OPERATOR"].ToString();
                IKDL_DATE_LSTC = row["IKDL_DATE_LSTC"].ToString();
                IKDL_LSTC_OPERATOR = row["IKDL_LSTC_OPERATOR"].ToString();
            }
        }


        #endregion

        #region Properties

        public string IKDL_AGENCY { get; set; }
        public string IKDL_DEPT { get; set; }
        public string IKDL_PROGRAM { get; set; }
        public string IKDL_CODE { get; set; }
        public string IKDL_SEQ { get; set; }
        public string IKDL_SERVICE_TYPE { get; set; }
        public string IKDL_SERVICE_AREA { get; set; }
        public string IKDL_SERVICE_DATE { get; set; }
        public string IKDL_PROG { get; set; }
        public string IKDL_SITE { get; set; }
        public string IKDL_ROOM { get; set; }
        public string IKDL_AMPM { get; set; }
        public string IKDL_FATHER { get; set; }
        public string IKDL_CLASS_ROOM { get; set; }
        public string IKDL_QUANTITY { get; set; }
        public string IKDL_RATE { get; set; }
        public string IKDL_UOM { get; set; }
        public string IKDL_AMOUNT { get; set; }
        public string IKDL_DATE_ADD { get; set; }
        public string IKDL_ADD_OPERATOR { get; set; }
        public string IKDL_DATE_LSTC { get; set; }
        public string IKDL_LSTC_OPERATOR { get; set; }
        #endregion
    }





}
