using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Captain.Common.Model.Objects;
using System.Data;
using Captain.DatabaseLayer;

namespace Captain.Common.Model.Data
{


    public class ChldAttnData
    {
        public ChldAttnData(CaptainModel model)
        {
            Model = model;
        }

        #region Properties

        public CaptainModel Model { get; set; }

        public string UserId { get; set; }

        #endregion

        public bool InsertUpdateDelChldAttn(ChldAttnEntity attnEntity)
        {
            bool boolSuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
             if (attnEntity.AGENCY != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_AGENCY", attnEntity.AGENCY));
             if (attnEntity.DEPT != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_DEPT", attnEntity.DEPT));
             if (attnEntity.PROG != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_PROGRAM", attnEntity.PROG));
             if (attnEntity.YEAR != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_YEAR", attnEntity.YEAR));
             if (attnEntity.APP_NO != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_APP_NO", attnEntity.APP_NO));
             if (attnEntity.SITE != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_SITE", attnEntity.SITE));
             if (attnEntity.ROOM != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_ROOM", attnEntity.ROOM));
             if (attnEntity.AMPM != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_AMPM", attnEntity.AMPM));
             if (attnEntity.FUNDING_SOURCE  != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_FUNDING_SOURCE", attnEntity.FUNDING_SOURCE));
             if (attnEntity.DATE  != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_DATE", attnEntity.DATE));
             //if (attnEntity.ENRL_AGENCY != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_ENRL_AGENCY", attnEntity.ENRL_AGENCY));
             //if (attnEntity.ENRL_DEPT != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_ENRL_DEPT", attnEntity.ENRL_DEPT));
             //if (attnEntity.ENRL_PROG != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_ENRL_PROG", attnEntity.ENRL_PROG));
             //if (attnEntity.ENRL_YEAR != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_ENRL_YEAR", attnEntity.ENRL_YEAR));
             //if (attnEntity.ENRL_SITE != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_ENRL_SITE", attnEntity.ENRL_SITE));
             //if (attnEntity.ENRL_ROOM != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_ENRL_ROOM", attnEntity.ENRL_ROOM));
             //if (attnEntity.ENRL_AM_PM != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_ENRL_AM_PM", attnEntity.ENRL_AM_PM));
             //if (attnEntity.ENRL_APP_NO != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_ENRL_APP_NO", attnEntity.ENRL_APP_NO));
             //if (attnEntity.ENRL_FUND_SOURCE != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_ENRL_FUND_SOURCE", attnEntity.ENRL_FUND_SOURCE ));
             if (attnEntity.PA != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_PA", attnEntity.PA));
             if (attnEntity.REASON != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_REASON", attnEntity.REASON));
             if (attnEntity.B != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_B", attnEntity.B));
             if (attnEntity.A != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_A", attnEntity.A));
             if (attnEntity.L != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_L", attnEntity.L));
             if (attnEntity.P != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_P", attnEntity.P));
             if (attnEntity.S != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_S", attnEntity.S));
             if (attnEntity.PARENT_RATE  != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_PARENT_RATE", attnEntity.PARENT_RATE));
             if (attnEntity.FUNDING_RATE != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_FUNDING_RATE", attnEntity.FUNDING_RATE));
             if (attnEntity.HOURS != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_HOURS", attnEntity.HOURS));
             if (attnEntity.CATEGORY != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_CATEGORY", attnEntity.CATEGORY));
             if (attnEntity.CHARGE_CODE != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_CHARGE_CODE", attnEntity.CHARGE_CODE));
             if (attnEntity.MEAL != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_MEAL", attnEntity.MEAL));
             if (attnEntity.PARENT_INVOICE != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_PARENT_INVOICE", attnEntity.PARENT_INVOICE));
             if (attnEntity.LEGAL != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_LEGAL", attnEntity.LEGAL));
             if (attnEntity.PRES_DESC != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_PRES_DESC", attnEntity.PRES_DESC));
             if (attnEntity.DATE_ADD != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_DATE_ADD", attnEntity.DATE_ADD));
             if (attnEntity.ADD_OPERATOR != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_ADD_OPERATOR", attnEntity.ADD_OPERATOR));
             if (attnEntity.DATE_LSTC != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_DATE_LSTC", attnEntity.DATE_LSTC));
             if (attnEntity.LSTC_OPERATOR != string.Empty)sqlParamList.Add(new SqlParameter("@ATTN_LSTC_OPERATOR", attnEntity.LSTC_OPERATOR));
                if (attnEntity.TimeStart1 != string.Empty) sqlParamList.Add(new SqlParameter("@ATTN_TIMESTART1", attnEntity.TimeStart1));
                if (attnEntity.TimeEnd1 != string.Empty) sqlParamList.Add(new SqlParameter("@ATTN_TIMEEND1", attnEntity.TimeEnd1));
                if (attnEntity.TimeStart2 != string.Empty) sqlParamList.Add(new SqlParameter("@ATTN_TIMESTART2", attnEntity.TimeStart2));
                if (attnEntity.TimeEnd2 != string.Empty) sqlParamList.Add(new SqlParameter("@ATTN_TIMEEND2", attnEntity.TimeEnd2));
                if (attnEntity.TimeSum1 != string.Empty) sqlParamList.Add(new SqlParameter("@ATTN_TIMESUM1", attnEntity.TimeSum1));
                if (attnEntity.TimeSum2 != string.Empty) sqlParamList.Add(new SqlParameter("@ATTN_TIMESUM2", attnEntity.TimeSum2));
                sqlParamList.Add(new SqlParameter("@Mode", attnEntity.Mode));



                boolSuccess = ChldAttnDB.InsertUpdateDelChldAttn(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }


            // strApplNo = strNewApplNo;
            return boolSuccess;
        }

        public bool InsertUpdateDelChldAttnXml(ChldAttnEntity attnEntity,out string strMsg)
        {
            bool boolSuccess = false;
            string strDeletemsg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (attnEntity.AGENCY != string.Empty) sqlParamList.Add(new SqlParameter("@ATTN_AGENCY", attnEntity.AGENCY));
                if (attnEntity.DEPT != string.Empty) sqlParamList.Add(new SqlParameter("@ATTN_DEPT", attnEntity.DEPT));
                if (attnEntity.PROG != string.Empty) sqlParamList.Add(new SqlParameter("@ATTN_PROGRAM", attnEntity.PROG));
                if (attnEntity.YEAR != string.Empty) sqlParamList.Add(new SqlParameter("@ATTN_YEAR", attnEntity.YEAR));
                if (attnEntity.APP_NO != string.Empty) sqlParamList.Add(new SqlParameter("@ATTN_APP_NO", attnEntity.APP_NO));
                if (attnEntity.SITE != string.Empty) sqlParamList.Add(new SqlParameter("@ATTN_SITE", attnEntity.SITE));
                if (attnEntity.ROOM != string.Empty) sqlParamList.Add(new SqlParameter("@ATTN_ROOM", attnEntity.ROOM));
                if (attnEntity.AMPM != string.Empty) sqlParamList.Add(new SqlParameter("@ATTN_AMPM", attnEntity.AMPM));
                if (attnEntity.AttnXml != string.Empty) sqlParamList.Add(new SqlParameter("@ATTN_Xml", attnEntity.AttnXml));
                if (attnEntity.ADD_OPERATOR != string.Empty) sqlParamList.Add(new SqlParameter("@ATTN_ADD_OPERATOR", attnEntity.ADD_OPERATOR));              
                if (attnEntity.LSTC_OPERATOR != string.Empty) sqlParamList.Add(new SqlParameter("@ATTN_LSTC_OPERATOR", attnEntity.LSTC_OPERATOR));
                sqlParamList.Add(new SqlParameter("@DeletePriv", attnEntity.Mode));
                SqlParameter sqloutmsg = new SqlParameter("@Msg", SqlDbType.VarChar, 10);
                sqloutmsg.Value = string.Empty;
                sqloutmsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqloutmsg);
                



                boolSuccess = ChldAttnDB.InsertUpdateDelChldAttnXml(sqlParamList);
                strDeletemsg = sqloutmsg.Value.ToString();
               

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }


            strMsg = strDeletemsg;
            return boolSuccess;
        }

        public List<ChldAttnEntity> GetChldAttnDetails(string Agency, string Dept, string Prog,string Year,string ApplNo,  string Site, string Room,string Ampm, string FundingSource,string Date)
        {
            List<ChldAttnEntity> ChldAttnDetails = new List<ChldAttnEntity>();
            try
            {
                DataSet ChldAttn = Captain.DatabaseLayer.ChldAttnDB.GetChldAttnDetails(Agency, Dept, Prog, Year, ApplNo, Site, Room, Ampm, FundingSource, Date);
                if (ChldAttn != null && ChldAttn.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ChldAttn.Tables[0].Rows)
                    {
                        ChldAttnDetails.Add(new ChldAttnEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return ChldAttnDetails;
            }

            return ChldAttnDetails;
        }

        public List<ChldAttnEntity> GetChldAttnBetweenDate(string Agency, string Dept, string Prog, string Year, string Site, string Room, string Ampm, string FundingSource, string StartDate, string EndDate)
        {
            List<ChldAttnEntity> ChldAttnDetails = new List<ChldAttnEntity>();
            try
            {
                DataSet ChldAttn = Captain.DatabaseLayer.ChldAttnDB.GetChldAttnBetweenDate(Agency, Dept, Prog, Year, Site, Room, Ampm, FundingSource, StartDate,EndDate);
                if (ChldAttn != null && ChldAttn.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ChldAttn.Tables[0].Rows)
                    {
                        ChldAttnDetails.Add(new ChldAttnEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return ChldAttnDetails;
            }

            return ChldAttnDetails;
        }


        public string CheckHss2108StartingDate(string Agency, string Dept, string Prog, string Year, string Date)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (Agency != string.Empty) sqlParamList.Add(new SqlParameter("@AGENCY", Agency));
            if (Dept != string.Empty) sqlParamList.Add(new SqlParameter("@DEPT", Dept));
            if (Prog != string.Empty) sqlParamList.Add(new SqlParameter("@Program", Prog));
            if (Year != string.Empty) sqlParamList.Add(new SqlParameter("@YEAR", Year));
            if (Date != string.Empty) sqlParamList.Add(new SqlParameter("@Date", Date));
            SqlParameter sqlMsg = new SqlParameter("@Msg", SqlDbType.VarChar, 150);
            sqlMsg.Value = string.Empty;
            sqlMsg.Direction = ParameterDirection.Output;
            sqlParamList.Add(sqlMsg);
            ChldAttnDB.CheckHss2108StartingDate(sqlParamList);
            return sqlMsg.Value.ToString();
        
        }

        public List<ChldAttnEntity> GetChldAttnBetweenDatehssb2109(string Agency, string Dept, string Prog, string Year, string StartDate, string EndDate,string strApplicant,string Reason,string strType)
        {
            List<ChldAttnEntity> ChldAttnDetails = new List<ChldAttnEntity>();
            try
            {
                DataSet ChldAttn = Captain.DatabaseLayer.ChldAttnDB.GetChldAttnBetweenDatehssb2109(Agency, Dept, Prog, Year,StartDate, EndDate,strApplicant,Reason, strType);
                if (ChldAttn != null && ChldAttn.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ChldAttn.Tables[0].Rows)
                    {
                        ChldAttnDetails.Add(new ChldAttnEntity(row,"HSSB2109"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return ChldAttnDetails;
            }

            return ChldAttnDetails;
        }

     
        public List<ChldAttnEntity> GetChldAttnBetweenDatehssb2109Count(string Agency, string Dept, string Prog, string Year, string StartDate, string EndDate, string strApplicant, string Reason)
        {
            List<ChldAttnEntity> ChldAttnDetails = new List<ChldAttnEntity>();
            try
            {
                DataSet ChldAttn = Captain.DatabaseLayer.ChldAttnDB.GetChldAttnBetweenDatehssb2109Count(Agency, Dept, Prog, Year, StartDate, EndDate, strApplicant, Reason);
                if (ChldAttn != null && ChldAttn.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ChldAttn.Tables[0].Rows)
                    {
                        ChldAttnDetails.Add(new ChldAttnEntity(row, "HSSB2109Count"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return ChldAttnDetails;
            }

            return ChldAttnDetails;
        }


        public List<ChldAttnEntity> GetChldAttn2109FundSummary(string Agency, string Dept, string Prog, string Year, string StartDate, string EndDate,string Site,string FundHie)
        {
            List<ChldAttnEntity> ChldAttnDetails = new List<ChldAttnEntity>();
            try
            {
                DataSet ChldAttn = Captain.DatabaseLayer.ChldAttnDB.GetChldAttn2109FundSummary(Agency, Dept, Prog, Year, StartDate, EndDate, Site,FundHie);
                if (ChldAttn != null && ChldAttn.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ChldAttn.Tables[0].Rows)
                    {
                        ChldAttnDetails.Add(new ChldAttnEntity(row, "HSSB2109Summary"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return ChldAttnDetails;
            }

            return ChldAttnDetails;
        }


        

        public List<HSSB2112ReportEntity> GetReportHSSB2112(string Agency, string Dept, string Program, string Year, string Site, string FundHie, string Fromdt, string Todt, string Meal_type)
        {
            List<HSSB2112ReportEntity> ChldAttnDetails = new List<HSSB2112ReportEntity>();
            try
            {
                DataSet mealreport = Captain.DatabaseLayer.ChldAttnDB.GetMealsReport_HSSB2112(Agency, Dept, Program, Year, Site, FundHie, Fromdt, Todt, Meal_type);
                if (mealreport != null && mealreport.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in mealreport.Tables[0].Rows)
                    {
                        ChldAttnDetails.Add(new HSSB2112ReportEntity(row));
                    }

                }
            }
            catch (Exception ex)
            {
                //
                return ChldAttnDetails;
            }

            return ChldAttnDetails;
        }

        public List<SqlParameter> Prepare_PIRMISC_SqlParameters_List(PIRMISCEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Mode", Entity.Mode));

                if (!string.IsNullOrEmpty(Entity.Agency))
                    sqlParamList.Add(new SqlParameter("@PIRMISC_AGENCY", Entity.Agency));
                if (!string.IsNullOrEmpty(Entity.Dept))
                    sqlParamList.Add(new SqlParameter("@PIRMISC_DEPT", Entity.Dept));
                if (!string.IsNullOrEmpty(Entity.Prog))
                    sqlParamList.Add(new SqlParameter("@PIRMISC_PROG", Entity.Prog));
                if (!string.IsNullOrEmpty(Entity.Year))
                    sqlParamList.Add(new SqlParameter("@PIRMISC_YEAR", Entity.Year));
                if (!string.IsNullOrEmpty(Entity.C9_Agy_Type))
                    sqlParamList.Add(new SqlParameter("@PIRMISC_C9_AGE_TYPE", Entity.C9_Agy_Type));
                if (!string.IsNullOrEmpty(Entity.Sites_Flag))
                    sqlParamList.Add(new SqlParameter("@PIRMISC_SITES_FLAG", Entity.Sites_Flag));
                if (!string.IsNullOrEmpty(Entity.Sites))
                    sqlParamList.Add(new SqlParameter("@PIRMISC_SITES", Entity.Sites));
                if (!string.IsNullOrEmpty(Entity.Taskfund))
                    sqlParamList.Add(new SqlParameter("@PIRMISC_TASK_FUND", Entity.Taskfund));

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<PIRMISCEntity> Browse_PIRMISC(PIRMISCEntity Entity, string Opretaion_Mode)
        {
            List<PIRMISCEntity> CASEACTProfile = new List<PIRMISCEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_PIRMISC_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_PIRMISC]");
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASECONT(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASEACTProfile.Add(new PIRMISCEntity(row));
                }
            }
            catch (Exception ex)
            { return CASEACTProfile; }

            return CASEACTProfile;
        }

        public bool InsertUpdatePIRMISC(PIRMISCEntity MISCEntity, string Opretaion_Mode)
        {
            bool boolSuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList =  Prepare_PIRMISC_SqlParameters_List(MISCEntity, "Update");

                boolSuccess = ChldAttnDB.UpdatePIRMISC(sqlParamList);

            }
            catch (Exception ex)
            {
                boolSuccess = false;
            }

            // strApplNo = strNewApplNo;
            return boolSuccess;
        }

        public bool UpdateAgyTab_pir(string Agy_1,string Agy_2)
        {
            bool boolSuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(Agy_1))
                    sqlParamList.Add(new SqlParameter("@Agy1", Agy_1));
                //if (!string.IsNullOrEmpty(Agy_2))
                    sqlParamList.Add(new SqlParameter("@agy2", Agy_2));

                boolSuccess = ChldAttnDB.UpdateAGYTab_Pir(sqlParamList);

            }
            catch (Exception ex)
            {
                boolSuccess = false;
            }

            // strApplNo = strNewApplNo;
            return boolSuccess;
        }


        public List<ChldAttnEntity> GetChldAttnCountMonth(string Agency, string Dept, string Prog, string Year, string ApplNo, string Site, string Room, string Ampm, string FundingSource, string Month)
        {
            List<ChldAttnEntity> ChldAttnDetails = new List<ChldAttnEntity>();
            try
            {
                DataSet ChldAttn = Captain.DatabaseLayer.ChldAttnDB.GetChldAttnCountMonth(Agency, Dept, Prog, Year, ApplNo, Site, Room, Ampm, FundingSource, Month);
                if (ChldAttn != null && ChldAttn.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ChldAttn.Tables[0].Rows)
                    {
                        ChldAttnDetails.Add(new ChldAttnEntity(row,"ATTNCOUNTMONTH"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return ChldAttnDetails;
            }

            return ChldAttnDetails;
        }

        public List<ChldAttnEntity> ARS2120Report(string Agency, string Dept, string Prog, string Year,string Site, string FundHie, string Fromdt, string Todt,string posted)
        {
            List<ChldAttnEntity> ChldAttnDetails = new List<ChldAttnEntity>();
            try
            {
                DataSet ChldAttn = Captain.DatabaseLayer.ARSDB.ARSb2120_Report(Agency, Dept, Prog, Year, Site, FundHie, Fromdt, Todt, posted);
                if (ChldAttn != null && ChldAttn.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ChldAttn.Tables[0].Rows)
                    {
                        ChldAttnDetails.Add(new ChldAttnEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return ChldAttnDetails;
            }

            return ChldAttnDetails;
        }

    }

}
