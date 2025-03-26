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
    public class MasterPovertyEntity
    {
        #region Constructors

        public MasterPovertyEntity()
        {
            Mode = string.Empty;
            GdlAgency = string.Empty;
            GdlDept = string.Empty;
            GdlProgram = string.Empty;
            GdlOldAgency = string.Empty;
            GdlOldDept = string.Empty;
            GdlOldProgram = string.Empty;
            GdlType = string.Empty;
            GdlCounty = string.Empty;
            GdlStartDate = string.Empty;
            GdlEndDate = string.Empty;
            GdlNoHouseHolds = string.Empty;
            Gdl1Value = string.Empty;
            Gdl2Value = string.Empty;
            Gdl3Value = string.Empty;
            Gdl4Value = string.Empty;
            Gdl5Value = string.Empty;
            Gdl6Value = string.Empty;
            Gdl7Value = string.Empty;
            Gdl8Value = string.Empty;
            Gdl9Value = string.Empty;
            Gdl10Value = string.Empty;
            Gdl11Value = string.Empty;
            Gdl12Value = string.Empty;
            GdlDateAdd = string.Empty;
            GdlAddOperator = string.Empty;
            GdlDateLstc = string.Empty;
            GdlLstcOperator = string.Empty;

        }
        public MasterPovertyEntity(DataRow CaseGdl)
        {
            if (CaseGdl != null)
            {
                DataRow row = CaseGdl;
                GdlAgency = row["GDL_AGENCY"].ToString();
                GdlDept = row["GDL_DEPT"].ToString();
                GdlProgram = row["GDL_PROGRAM"].ToString();
                GdlOldAgency = row["GDL_AGENCY"].ToString();
                GdlOldDept = row["GDL_DEPT"].ToString();
                GdlOldProgram = row["GDL_PROGRAM"].ToString();
                GdlType = row["GDL_TYPE"].ToString();
                GdlCounty = row["GDL_COUNTY"].ToString();
                GdlStartDate = row["GDL_START_DATE"].ToString();
                GdlEndDate = row["GDL_END_DATE"].ToString();
                GdlNoHouseHolds = row["GDL_NO_HOUSEHOLDS"].ToString();
                Gdl1Value = row["GDL_1_VALUE"].ToString();
                Gdl2Value = row["GDL_2_VALUE"].ToString();
                Gdl3Value = row["GDL_3_VALUE"].ToString();
                Gdl4Value = row["GDL_4_VALUE"].ToString();
                Gdl5Value = row["GDL_5_VALUE"].ToString();
                Gdl6Value = row["GDL_6_VALUE"].ToString();
                Gdl7Value = row["GDL_7_VALUE"].ToString();
                Gdl8Value = row["GDL_8_VALUE"].ToString();
                Gdl9Value = row["GDL_9_VALUE"].ToString();
                Gdl10Value = row["GDL_10_VALUE"].ToString();
                Gdl11Value = row["GDL_11_VALUE"].ToString();
                Gdl12Value = row["GDL_12_VALUE"].ToString();
                GdlDateAdd = row["GDL_DATE_ADD"].ToString();
                GdlAddOperator = row["GDL_ADD_OPERATOR"].ToString();
                GdlDateLstc = row["GDL_DATE_LSTC"].ToString();
                GdlLstcOperator = row["GDL_LSTC_OPERATOR"].ToString();

            }
        }

        #endregion

        #region Properties

        public string Mode { get; set; }
        public string GdlAgency { get; set; }
        public string GdlDept { get; set; }
        public string GdlProgram { get; set; }
        public string GdlOldAgency { get; set; }
        public string GdlOldDept { get; set; }
        public string GdlOldProgram { get; set; }
        public string GdlType { get; set; }
        public string GdlCounty { get; set; }
        public string GdlStartDate { get; set; }
        public string GdlEndDate { get; set; }
        public string GdlNoHouseHolds { get; set; }
        public string Gdl1Value { get; set; }
        public string Gdl2Value { get; set; }
        public string Gdl3Value { get; set; }
        public string Gdl4Value { get; set; }
        public string Gdl5Value { get; set; }
        public string Gdl6Value { get; set; }
        public string Gdl7Value { get; set; }
        public string Gdl8Value { get; set; }
        public string Gdl9Value { get; set; }
        public string Gdl10Value { get; set; }
        public string Gdl11Value { get; set; }
        public string Gdl12Value { get; set; }
        public string GdlDateAdd { get; set; }
        public string GdlAddOperator { get; set; }
        public string GdlDateLstc { get; set; }
        public string GdlLstcOperator { get; set; }


        #endregion

       
    }

    public class PovertyException
    {
        #region Constructors
        public PovertyException()
        { 
        Mode =string.Empty;
        Agency =string.Empty;
        Dept  =string.Empty;
        Program  =string.Empty;     
        StartDate  =string.Empty;
        EndDate  =string.Empty;      
        Exp1Value  =string.Empty;
        Exp2Value =string.Empty;
        Exp3Value  =string.Empty;
        Exp4Value  =string.Empty;
        Exp5Value  =string.Empty;
        Exp6Value  =string.Empty;
        Exp7Value  =string.Empty;
        Exp8Value  =string.Empty;
        ExpDateAdd  =string.Empty;
        ExpAddOperator  =string.Empty;
        ExpDateLstc  =string.Empty;
        ExpLstcOperator = string.Empty;
        
        }
        public PovertyException(DataRow drrow)
        {

            if (drrow != null)
            {
                Mode = string.Empty;
                Agency = drrow["EXP_AGENCY"].ToString();
                Dept = drrow["EXP_DEPT"].ToString();
                Program = drrow["EXP_PROGRAM"].ToString();
                StartDate = drrow["EXP_START_DATE"].ToString();
                EndDate = drrow["EXP_END_DATE"].ToString();
                Exp1Value = drrow["EXP_1_VALUE"].ToString();
                Exp2Value = drrow["EXP_2_VALUE"].ToString();
                Exp3Value = drrow["EXP_3_VALUE"].ToString();
                Exp4Value = drrow["EXP_4_VALUE"].ToString();
                Exp5Value = drrow["EXP_5_VALUE"].ToString();
                Exp6Value = drrow["EXP_6_VALUE"].ToString();
                Exp7Value = drrow["EXP_7_VALUE"].ToString();
                Exp8Value = drrow["EXP_8_VALUE"].ToString();
                ExpDateAdd = drrow["EXP_DATE_ADD"].ToString();
                ExpAddOperator = drrow["EXP_ADD_OPERATOR"].ToString();
                ExpDateLstc = drrow["EXP_DATE_LSTC"].ToString();
                ExpLstcOperator = drrow["EXP_LSTC_OPERATOR"].ToString();
            }
        }
         #endregion
        #region Properties

        public string Mode { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }       
        public string StartDate { get; set; }
        public string EndDate { get; set; }        
        public string Exp1Value { get; set; }
        public string Exp2Value { get; set; }
        public string Exp3Value { get; set; }
        public string Exp4Value { get; set; }
        public string Exp5Value { get; set; }
        public string Exp6Value { get; set; }
        public string Exp7Value { get; set; }
        public string Exp8Value { get; set; }
        public string ExpDateAdd { get; set; }
        public string ExpAddOperator { get; set; }
        public string ExpDateLstc { get; set; }
        public string ExpLstcOperator { get; set; }


        #endregion
    }
}
