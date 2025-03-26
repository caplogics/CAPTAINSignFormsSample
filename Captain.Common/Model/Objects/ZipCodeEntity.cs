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
    public class ZipCodeEntity 
    {
        #region Constructors

        public ZipCodeEntity()
        {
            Zcrzip = string.Empty;
            Zcrplus4 = string.Empty;
            Zcrcity = string.Empty;
            Zcrstate = string.Empty;
            Zcrcitycode = string.Empty;
            Zcrcountry = string.Empty;
            Zcrintakecode = string.Empty;
            Zcrdate = string.Empty;
            Zcrapp = string.Empty;
            Zcrdatelstc = string.Empty;
            Zcrlstcoperator = string.Empty;
            Zcrdateadd = string.Empty;
            Zcraddoperator = string.Empty;
            Zcrhssmo = string.Empty;
            Zcrhssday = string.Empty;
            Zcrhssyear = string.Empty;
            InActiveFlag = string.Empty;

            InActive = string.Empty;
            Mode = "A";
         
        }

        public ZipCodeEntity(DataRow zipCode)
        {
            if (zipCode != null)
            {
                DataRow row = zipCode;
                Zcrzip = row["ZCR_ZIP"].ToString().Trim();
                Zcrplus4 = row["ZCRPLUS_4"].ToString().Trim();
                Zcrcity = row["ZCR_CITY"].ToString().Trim();
                Zcrstate = row["ZCR_STATE"].ToString().Trim();
                Zcrcitycode = row["ZCR_CITY_CODE"].ToString().Trim();
                Zcrcountry = row["ZCR_COUNTY"].ToString().Trim();
                Zcrintakecode = row["ZCR_INTAKE_CODE"].ToString().Trim();
                Zcrhssmo = row["ZCR_HSS_MO"].ToString().Trim();
                Zcrhssday = row["ZCR_HSS_DAY"].ToString().Trim();
                Zcrhssyear = row["ZCR_HSS_YEAR"].ToString().Trim();
                Zcrapp = row["ZCR_APP"].ToString().Trim();
                Zcrdatelstc = row["ZCR_DATE_LSTC"].ToString().Trim();
                Zcrlstcoperator = row["ZCR_LSTC_OPERATOR"].ToString().Trim();
                Zcrdateadd = row["ZCR_DATE_ADD"].ToString().Trim();
                Zcraddoperator = row["ZCR_ADD_OPERATOR"].ToString().Trim();
                InActive = row["ZCR_INACTIVE"].ToString().Trim();
                Mode = "U";
            }
        }

        public ZipCodeEntity(DataRow zipCode,string strFilterMode)
        {
            if (zipCode != null)
            {
                DataRow row = zipCode;
                Zcrzip = row["ZCR_ZIP"].ToString().Trim();
                Zcrplus4 = row["ZCRPLUS_4"].ToString().Trim();
                Zcrcity = row["ZCR_CITY"].ToString().Trim();
                Zcrstate = row["ZCR_STATE"].ToString().Trim();
                Zcrcitycode = row["ZCR_CITY_CODE"].ToString().Trim();
                Zcrcountry = row["ZCR_COUNTY"].ToString().Trim();
                Zcrintakecode = row["ZCR_INTAKE_CODE"].ToString().Trim();
                Zcrhssmo = row["ZCR_HSS_MO"].ToString().Trim();
                Zcrhssday = row["ZCR_HSS_DAY"].ToString().Trim();
                Zcrhssyear = row["ZCR_HSS_YEAR"].ToString().Trim();
                Zcrapp = row["ZCR_APP"].ToString().Trim();
                Zcrdatelstc = row["ZCR_DATE_LSTC"].ToString().Trim();
                Zcrlstcoperator = row["ZCR_LSTC_OPERATOR"].ToString().Trim();
                Zcrdateadd = row["ZCR_DATE_ADD"].ToString().Trim();
                Zcraddoperator = row["ZCR_ADD_OPERATOR"].ToString().Trim();
                TownSHip = row["TownShip"].ToString().Trim();
                County = row["County"].ToString().Trim();

                InActive = row["ZCR_INACTIVE"].ToString().Trim();

                InActiveFlag = "false";
            }
        }

        #endregion

        #region Properties

        public string Zcrzip { get; set; }
        public string Zcrplus4 { get; set; }
        public string Zcrcity { get; set; }
        public string Zcrhssmo { get; set; }
        public string Zcrhssday { get; set; }
        public string Zcrhssyear { get; set; }       
        public string Zcrstate { get; set; }
        public string Zcrcitycode { get; set; }
        public string Zcrcountry { get; set; }
        public string Zcrintakecode { get; set; }
        public string Zcrdate { get; set; }
        public string Zcrapp { get; set; }
        public string Zcrdatelstc { get; set; }
        public string Zcrlstcoperator { get; set; }
        public string Zcrdateadd { get; set; }
        public string Zcraddoperator { get; set; }
        public string Mode { get; set; }
        public string TownSHip { get; set; }
        public string County { get; set; }
        public string InActiveFlag { get; set; }

        public string InActive { get; set; }

        #endregion

        //#region Public / Overrides Methods

        //public override bool Equals(object obj)
        //{
        //    bool returnValue = false;

        //    // Check for null values and compare run-time types.
        //    if (obj == null || GetType() != obj.GetType())
        //    {
        //        returnValue = false;
        //    }

        //    ZipCodeEntity zipcode = obj as ZipCodeEntity;
        //    if (zipcode != null)
        //    {
        //        returnValue = (zipcode.Zcrzip.Equals(Zcrzip)) && (zipcode.Zcrzip.Equals(Zcrzip));
        //    }
        //    return returnValue;
        //}



        //#endregion
    }

    public class LIHPMQuesEntity
    { 
    
        #region Constructors

        public LIHPMQuesEntity()
        {
            LPMQ_CODE = string.Empty;
            LPMQ_SNO = string.Empty;
            LPMQ_DESC = string.Empty;
            LPMQ_AGYSTYPE = string.Empty;
            LPMQ_YEAR = string.Empty;
            LPMQ_QTYPE = string.Empty;
        }

        public LIHPMQuesEntity(DataRow LIHPMQues)
        {
            if (LIHPMQues != null)
            {
                DataRow row = LIHPMQues;
                LPMQ_CODE = row["LPMQ_CODE"].ToString().Trim();
                LPMQ_SNO = row["LPMQ_SNO"].ToString().Trim();
                LPMQ_DESC = row["LPMQ_DESC"].ToString().Trim();
                LPMQ_AGYSTYPE = row["LPMQ_AGYSTYPE"].ToString().Trim();
                LPMQ_YEAR = row["LPMQ_YEAR"].ToString().Trim();
                LPMQ_QTYPE = row["LPMQ_QTYPE"].ToString().Trim();
            }
        }

        
        #endregion

        #region Properties

        public string LPMQ_CODE { get; set; }
        public string LPMQ_SNO { get; set; }
        public string LPMQ_DESC { get; set; }
        public string LPMQ_AGYSTYPE { get; set; }
        public string LPMQ_YEAR { get; set; }
        public string LPMQ_QTYPE { get; set; }
        #endregion

    }
}
