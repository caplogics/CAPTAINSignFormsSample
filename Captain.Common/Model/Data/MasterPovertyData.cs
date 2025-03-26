using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.DatabaseLayer;
using Captain.Common.Model.Objects;
using System.Data.SqlClient;
using System.Data;

namespace Captain.Common.Model.Data
{

    public class MasterPovertyData
    {

        public List<MasterPovertyEntity> GetCaseGdl()
        {
            List<MasterPovertyEntity> CaseGdlProfile = new List<MasterPovertyEntity>();
            try
            {
                DataSet CaseGdlData = MasterPoverty.GetCASEGDL();
                if (CaseGdlData != null && CaseGdlData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseGdlData.Tables[0].Rows)
                    {
                        CaseGdlProfile.Add(new MasterPovertyEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseGdlProfile;
            }

            return CaseGdlProfile;
        }

        public List<MasterPovertyEntity> GetCaseGdladpt(string Agency, string Dept, string Program, string Type,string County)
        {
            List<MasterPovertyEntity> CaseGdlProfile = new List<MasterPovertyEntity>();
            try
            {
                DataSet CaseGdlData = MasterPoverty.GetCASEGDLadpt(Agency, Dept, Program, Type,County);
                if (CaseGdlData != null && CaseGdlData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseGdlData.Tables[0].Rows)
                    {
                        CaseGdlProfile.Add(new MasterPovertyEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseGdlProfile;
            }

            return CaseGdlProfile;
        }

        public List<MasterPovertyEntity> GetCaseGdlSubadpt(string Agency, string Dept, string Program, string Type, string County,string StartDate,string EndDate)
        {
            List<MasterPovertyEntity> CaseGdlProfile = new List<MasterPovertyEntity>();
            try
            {
                DataSet CaseGdlData = MasterPoverty.GetCASEGDLSubadpt(Agency, Dept, Program, Type, County,StartDate,EndDate,string.Empty);
                if (CaseGdlData != null && CaseGdlData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseGdlData.Tables[0].Rows)
                    {
                        CaseGdlProfile.Add(new MasterPovertyEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseGdlProfile;
            }

            return CaseGdlProfile;
        }

        public List<MasterPovertyEntity> GetCaseGdlSubadptMain(string Agency, string Dept, string Program, string Type, string County, string StartDate, string EndDate)
        {
            List<MasterPovertyEntity> CaseGdlProfile = new List<MasterPovertyEntity>();
            try
            {
                DataSet CaseGdlData = MasterPoverty.GetCASEGDLSubadpt(Agency, Dept, Program, Type, County, StartDate, EndDate,"MAIN");
                if (CaseGdlData != null && CaseGdlData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseGdlData.Tables[0].Rows)
                    {
                        CaseGdlProfile.Add(new MasterPovertyEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseGdlProfile;
            }

            return CaseGdlProfile;
        }


        public List<MasterPovertyEntity> GetCaseGdlByHIE(string Agency, string Dept, string Program)
        {
            List<MasterPovertyEntity> CaseGdlProfile = new List<MasterPovertyEntity>();
            try
            {
                DataSet CaseGdlData = MasterPoverty.GetCASEGDLByHIE(Agency, Dept, Program);
                if (CaseGdlData != null && CaseGdlData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseGdlData.Tables[0].Rows)
                    {
                        CaseGdlProfile.Add(new MasterPovertyEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseGdlProfile;
            }

            return CaseGdlProfile;
        }

        /// <summary>
        /// Get User Profile information. 
        /// </summary>
        /// <param name="userID">The CaseGdl ID to get CaseGdl.</param>
        /// <returns>Returns a DataSet with the CaseGdl's profiles.</returns>
        public bool InsertCaseGdl(MasterPovertyEntity CaseGdl)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@GDL_AGENCY", CaseGdl.GdlAgency));
                sqlParamList.Add(new SqlParameter("@GDL_DEPT", CaseGdl.GdlDept));
                sqlParamList.Add(new SqlParameter("@GDL_PROGRAM", CaseGdl.GdlProgram));
                if (CaseGdl.GdlOldAgency != string.Empty)
                   sqlParamList.Add(new SqlParameter("@GDL_OldAGENCY", CaseGdl.GdlOldAgency));
                if (CaseGdl.GdlOldDept != string.Empty)
                  sqlParamList.Add(new SqlParameter("@GDL_OldDEPT", CaseGdl.GdlOldDept));
                if (CaseGdl.GdlOldProgram != string.Empty)
                  sqlParamList.Add(new SqlParameter("@GDL_OldPROGRAM", CaseGdl.GdlOldProgram));
                sqlParamList.Add(new SqlParameter("@GDL_TYPE", CaseGdl.GdlType));
                if (CaseGdl.GdlCounty != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_COUNTY", CaseGdl.GdlCounty));
                }
                sqlParamList.Add(new SqlParameter("@GDL_START_DATE", CaseGdl.GdlStartDate));
                sqlParamList.Add(new SqlParameter("@GDL_END_DATE", CaseGdl.GdlEndDate));
                if (CaseGdl.GdlNoHouseHolds != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_NO_HOUSEHOLDS", CaseGdl.GdlNoHouseHolds));
                }
                if (CaseGdl.Gdl1Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_1_VALUE", CaseGdl.Gdl1Value));
                }
                if (CaseGdl.Gdl2Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_2_VALUE", CaseGdl.Gdl2Value));
                }
                if (CaseGdl.Gdl3Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_3_VALUE", CaseGdl.Gdl3Value));
                }
                if (CaseGdl.Gdl4Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_4_VALUE", CaseGdl.Gdl4Value));
                }
                if (CaseGdl.Gdl5Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_5_VALUE", CaseGdl.Gdl5Value));
                }
                if (CaseGdl.Gdl6Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_6_VALUE", CaseGdl.Gdl6Value));
                }
                if (CaseGdl.Gdl7Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_7_VALUE", CaseGdl.Gdl7Value));
                }
                if (CaseGdl.Gdl8Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_8_VALUE", CaseGdl.Gdl8Value));
                }
                if (CaseGdl.Gdl9Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_9_VALUE", CaseGdl.Gdl9Value));
                }
                if (CaseGdl.Gdl10Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_10_VALUE", CaseGdl.Gdl10Value));
                }
                if (CaseGdl.Gdl11Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_11_VALUE", CaseGdl.Gdl11Value));
                }
                if (CaseGdl.Gdl12Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_12_VALUE", CaseGdl.Gdl12Value));
                }
                sqlParamList.Add(new SqlParameter("@GDL_ADD_OPERATOR", CaseGdl.GdlAddOperator));
                sqlParamList.Add(new SqlParameter("@GDL_LSTC_OPERATOR", CaseGdl.GdlLstcOperator));
                sqlParamList.Add(new SqlParameter("@Mode", CaseGdl.Mode));
                boolsuccess = MasterPoverty.InsertCASEGDL(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public bool UpdateCaseGdl(MasterPovertyEntity CaseGdl)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@GDL_AGENCY", CaseGdl.GdlAgency));
                sqlParamList.Add(new SqlParameter("@GDL_DEPT", CaseGdl.GdlDept));
                sqlParamList.Add(new SqlParameter("@GDL_PROGRAM", CaseGdl.GdlProgram));
                sqlParamList.Add(new SqlParameter("@GDL_OldAGENCY", CaseGdl.GdlOldAgency));
                sqlParamList.Add(new SqlParameter("@GDL_OldDEPT", CaseGdl.GdlOldDept));
                sqlParamList.Add(new SqlParameter("@GDL_OldPROGRAM", CaseGdl.GdlOldProgram));
                sqlParamList.Add(new SqlParameter("@GDL_TYPE", CaseGdl.GdlType));
                if (CaseGdl.GdlCounty != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_COUNTY", CaseGdl.GdlCounty));
                }
                sqlParamList.Add(new SqlParameter("@GDL_START_DATE", CaseGdl.GdlStartDate));
                sqlParamList.Add(new SqlParameter("@GDL_END_DATE", CaseGdl.GdlEndDate));
                if (CaseGdl.GdlNoHouseHolds != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_NO_HOUSEHOLDS", CaseGdl.GdlNoHouseHolds));
                }
                if (CaseGdl.Gdl1Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_1_VALUE", CaseGdl.Gdl1Value));
                }
                if (CaseGdl.Gdl2Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_2_VALUE", CaseGdl.Gdl2Value));
                }
                if (CaseGdl.Gdl3Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_3_VALUE", CaseGdl.Gdl3Value));
                }
                if (CaseGdl.Gdl4Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_4_VALUE", CaseGdl.Gdl4Value));
                }
                if (CaseGdl.Gdl5Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_5_VALUE", CaseGdl.Gdl5Value));
                }
                if (CaseGdl.Gdl6Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_6_VALUE", CaseGdl.Gdl6Value));
                }
                if (CaseGdl.Gdl7Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_7_VALUE", CaseGdl.Gdl7Value));
                }
                if (CaseGdl.Gdl8Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_8_VALUE", CaseGdl.Gdl8Value));
                }
                if (CaseGdl.Gdl9Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_9_VALUE", CaseGdl.Gdl9Value));
                }
                if (CaseGdl.Gdl10Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_10_VALUE", CaseGdl.Gdl10Value));
                }
                if (CaseGdl.Gdl11Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_11_VALUE", CaseGdl.Gdl11Value));
                }
                if (CaseGdl.Gdl12Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_12_VALUE", CaseGdl.Gdl12Value));
                }
                sqlParamList.Add(new SqlParameter("@GDL_ADD_OPERATOR", CaseGdl.GdlAddOperator));
                sqlParamList.Add(new SqlParameter("@GDL_LSTC_OPERATOR", CaseGdl.GdlLstcOperator));
                boolsuccess = MasterPoverty.UpdateCASEGDL(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }


        public bool DeleteCaseGdl(string Agency, string Dept, string Program, string strType,string County, string StartDate, string EndDate,string NoHoulds,string strDeleteType)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@GDL_AGENCY", Agency));
                sqlParamList.Add(new SqlParameter("@GDL_DEPT", Dept));
                sqlParamList.Add(new SqlParameter("@GDL_PROGRAM", Program));
                sqlParamList.Add(new SqlParameter("@GDL_TYPE", strType));
                if (County != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_COUNTY", County));
                }
                sqlParamList.Add(new SqlParameter("@GDL_START_DATE", StartDate));
                sqlParamList.Add(new SqlParameter("@GDL_END_DATE", EndDate));
                if (NoHoulds != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@GDL_NO_HOUSEHOLDS", NoHoulds));
                }
                if (strDeleteType != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@Type", strDeleteType));
                }
                boolsuccess = MasterPoverty.DeleteCASEGDL(sqlParamList);
            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public int CaseGdldateExistCheck(string Agency, string Dept, string Program, string Type, string StartDate, string EndDate,string county)
        {

            DataSet CaseGdlData = MasterPoverty.CASEGDLDateExists(Agency, Dept, Program, Type, StartDate, EndDate, county);
            int totalRows = 0;
            if (CaseGdlData != null && CaseGdlData.Tables[0].Rows.Count > 0)
            {
                totalRows = Convert.ToInt32(CaseGdlData.Tables[0].Rows[0]["Totalrows"]);
            }
            return totalRows;
        }


        public List<MasterPovertyEntity> GetFedralOmbChart(string strAgency,string strDept,string strProg,string Type, string strDate)
        {
            List<MasterPovertyEntity> CaseGdlProfile = new List<MasterPovertyEntity>();
            try
            {
                DataSet CaseGdlData = MasterPoverty.GetFederalOmbChart(strAgency,strDept,strProg,Type,strDate);
                if (CaseGdlData != null && CaseGdlData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseGdlData.Tables[0].Rows)
                    {
                        CaseGdlProfile.Add(new MasterPovertyEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseGdlProfile;
            }

            return CaseGdlProfile;
        }

        public bool InsertUpdateDelPovertyExp(PovertyException povertExp)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@EXP_AGENCY", povertExp.Agency));
                sqlParamList.Add(new SqlParameter("@EXP_DEPT", povertExp.Dept));
                sqlParamList.Add(new SqlParameter("@EXP_PROGRAM", povertExp.Program));
               
                sqlParamList.Add(new SqlParameter("@EXP_START_DATE", povertExp.StartDate));
                sqlParamList.Add(new SqlParameter("@EXP_END_DATE", povertExp.EndDate));
               
                if (povertExp.Exp1Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@EXP_1_VALUE", povertExp.Exp1Value));
                }
                if (povertExp.Exp2Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@EXP_2_VALUE", povertExp.Exp2Value));
                }
                if (povertExp.Exp3Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@EXP_3_VALUE", povertExp.Exp3Value));
                }
                if (povertExp.Exp4Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@EXP_4_VALUE", povertExp.Exp4Value));
                }
                if (povertExp.Exp5Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@EXP_5_VALUE", povertExp.Exp5Value));
                }
                if (povertExp.Exp6Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@EXP_6_VALUE", povertExp.Exp6Value));
                }
                if (povertExp.Exp7Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@EXP_7_VALUE", povertExp.Exp7Value));
                }
                if (povertExp.Exp8Value != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@EXP_8_VALUE", povertExp.Exp8Value));
                }
                sqlParamList.Add(new SqlParameter("@EXP_ADD_OPERATOR", povertExp.ExpAddOperator));
                sqlParamList.Add(new SqlParameter("@EXP_LSTC_OPERATOR", povertExp.ExpLstcOperator));
                sqlParamList.Add(new SqlParameter("@Mode", povertExp.Mode));
                boolsuccess = MasterPoverty.InsertUpdateDelPovertyExp(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public PovertyException GetPovertyException(string Agency, string Dept, string Program, string StartDate, string EndDate,string strverifyDate,string strMode)
        {
            PovertyException poverttException = null;
            try
            {
                DataSet PovertyExp = MasterPoverty.GetPovertyException(Agency, Dept, Program, StartDate, EndDate, strverifyDate,strMode);
                if (PovertyExp != null && PovertyExp.Tables[0].Rows.Count > 0)
                {
                    poverttException = new PovertyException(PovertyExp.Tables[0].Rows[0]);
                }
            }
            catch (Exception ex)
            {
                //
                return poverttException;
            }

            return poverttException;
        }

        public List<PovertyException> GetPovertyExceptionList(string Agency, string Dept, string Program, string StartDate, string EndDate, string strverifyDate, string strMode)
        {
            List<PovertyException> CaseGdlProfile = new List<PovertyException>();
            try
            {
                DataSet CaseGdlData = MasterPoverty.GetPovertyException(Agency, Dept, Program, StartDate, EndDate, strverifyDate, strMode);
                if (CaseGdlData != null && CaseGdlData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseGdlData.Tables[0].Rows)
                    {
                        CaseGdlProfile.Add(new PovertyException(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseGdlProfile;
            }

            return CaseGdlProfile;
        }


    }
}
