using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace Captain.DatabaseLayer
{

    [DataObject]
    [Serializable]
    public partial class ChldAttnDB
    {
        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[CASESUM]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;

        #endregion

        public static bool InsertUpdateDelChldAttn(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelChldAttn");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertUpdateDelChldAttnXml(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelChldAttnXml");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool CheckHss2108StartingDate(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CheckATTMStartDate");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetChldAttnDetails(string Agency, string Dept, string Prog, string Year, string ApplNo, string Site, string Room, string Ampm, string FundingSource, string Date)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetChldAttnDetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Prog != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_PROGRAM", Prog);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (ApplNo != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_APP_NO", ApplNo);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_SITE", Site);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Room != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_ROOM", Room);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Ampm != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_AMPM", Ampm);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (FundingSource != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_FUNDING_SOURCE ", FundingSource);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Date != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_DATE ", Date);
                dbCommand.Parameters.Add(empnoParm);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static string GetChldAttnDate(string Agency, string Dept, string Prog, string Year, string ApplNo, string Site, string Room, string Ampm, string FundingSource, string Date)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            string strDate = string.Empty;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetChldAttnDate]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Prog != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_PROGRAM", Prog);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (ApplNo != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_APP_NO", ApplNo);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_SITE", Site);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Room != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_ROOM", Room);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Ampm != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_AMPM", Ampm);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (FundingSource != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_FUNDING_SOURCE ", FundingSource);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Date != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_DATE ", Date);
                dbCommand.Parameters.Add(empnoParm);
            }
            ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strDate = ds.Tables[0].Rows[0]["ATTN_DATE"].ToString();
                }
            }
            return strDate;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetChldAttnBetweenDate(string Agency, string Dept, string Prog, string Year, string Site, string Room, string Ampm, string FundingSource, string StartDate, string EndDate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            string strDate = string.Empty;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetChldAttnBetweenDates]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Prog != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_PROGRAM", Prog);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_SITE", Site);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Room != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_ROOM", Room);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Ampm != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_AMPM", Ampm);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (FundingSource != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_FUNDING_SOURCE ", FundingSource);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (StartDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_STARTDATE", StartDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (EndDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_ENDDATE", EndDate);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetChldAttnBetweenDatehssb2109(string Agency, string Dept, string Prog, string Year, string StartDate, string EndDate, string Applicant, string Reason,string strType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            string strDate = string.Empty;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_CHLDATTN2109]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Prog != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_PROGRAM", Prog);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (StartDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FromDate", StartDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (EndDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ToDate", EndDate);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Applicant != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AppNo", Applicant);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Reason != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Reason", Reason);
                dbCommand.Parameters.Add(empnoParm);
            }

            if(strType !=string.Empty )
            {
                SqlParameter empnoParm = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(empnoParm);
                
            }


            ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetChldAttnBetweenDatehssb2109Count(string Agency, string Dept, string Prog, string Year, string StartDate, string EndDate, string Applicant, string Reason)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            string strDate = string.Empty;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_CHLDATTN2109Count]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Prog != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_PROGRAM", Prog);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (StartDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FromDate", StartDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (EndDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ToDate", EndDate);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Applicant != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AppNo", Applicant);
                dbCommand.Parameters.Add(empnoParm);
            }

            //if (Reason != string.Empty)
            //{
            //    SqlParameter empnoParm = new SqlParameter("@Reason", Reason);
            //    dbCommand.Parameters.Add(empnoParm);
            //}



            ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetChldAttn2109FundSummary(string Agency, string Dept, string Prog, string Year, string StartDate, string EndDate, string Site, string FundHie)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            string strDate = string.Empty;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_CHLDATTN2109FundSummary]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Prog != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_PROGRAM", Prog);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (StartDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FromDate", StartDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (EndDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ToDate", EndDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Site", Site);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (FundHie != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FundHie", FundHie);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }




        public static DataSet GetEnrollDetails2108(string Agency, string Dept, string Program, string Year, string Type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            string strDate = string.Empty;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetEnrlDetails2108]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PROG", Program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Type != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Type", Type);
                dbCommand.Parameters.Add(empnoParm);
            }


            ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        public static DataSet GetEnrollDetails2109(string Agency, string Dept, string Program, string Year, string Type, string ToDate, string Site, string FundHie, string Applicant, string Sequence, string strDays, string FromDt)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            string strDate = string.Empty;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETENRL_HSSB2109]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PROG", Program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Type != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Type", Type);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (ToDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ToDate", ToDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Site", Site);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (FundHie != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FundHie", FundHie);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Applicant != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@App_No", Applicant);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Sequence != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Sequence", Sequence);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strDays != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ConsecutiveDays", strDays);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (FromDt != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FromDate", FromDt);
                dbCommand.Parameters.Add(empnoParm);
            }




            ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        public static DataSet GetEnrollDetails2103(string Agency, string Dept, string Program, string Year, string Type, string Site, string FundHie, string Zip, string Task, string Childrenwith, string Sequence, string strAppno)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            string strDate = string.Empty;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETENRL_HSSB2103]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PROG", Program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Type != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Type", Type);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Site", Site);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (FundHie != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FundHie", FundHie);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Sequence != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Sequence", Sequence);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Task != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Task", Task);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Childrenwith != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ChildrenWith", Childrenwith);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Zip != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ZipCode", Zip);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strAppno != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ApplNo", strAppno);
                dbCommand.Parameters.Add(empnoParm);
            }


            ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        public static DataSet GetMealsReport_HSSB2112(string Agency, string Dept, string Program, string Year, string Site, string FundHie, string Fromdt, string Todt, string Meal_type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            string strDate = string.Empty;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[HSSB2112_Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Agency", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Dept", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@prog", Program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@year", Year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Site_hie", Site);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (FundHie != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FundHie", FundHie);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Fromdt != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@From_dt", Fromdt);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Todt != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@To_dt", Todt);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Meal_type != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@MEALS", Meal_type);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        public static DataSet GetEnrollDetails2114(string Agency, string Dept, string Program, string Year, string ApplNo, string Type, string Site, string FundHie, string Task, string EnrollStatus, string Sequence)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            string strDate = string.Empty;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETENRL_HSSB2114]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PROG", Program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (ApplNo != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Appl_No", ApplNo);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Type != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Type", Type);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Site", Site);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (FundHie != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FundHie", FundHie);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Sequence != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Sequence", Sequence);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Task != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Task", Task);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (EnrollStatus != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EnrollStatus", EnrollStatus);
                dbCommand.Parameters.Add(empnoParm);
            }


            ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        public static DataSet Get2111SummaryDetails(string Agency, string Dept, string Prog, string Year, string StartDate, string EndDate, string Sites,string strFunds)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            string strDate = string.Empty;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_CHLDATTN2111Summary]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Prog != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_PROGRAM", Prog);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (StartDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FromDate", StartDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (EndDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ToDate", EndDate);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Sites != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Sites", Sites);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strFunds != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Funds", strFunds);
                dbCommand.Parameters.Add(empnoParm);
            }



            ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }


        public static DataSet Get2111Details(string Agency, string Dept, string Prog, string Year, string StartDate, string EndDate, string Sites)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            string strDate = string.Empty;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_CHLDATTN2111Details]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Prog != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_PROGRAM", Prog);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (StartDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FromDate", StartDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (EndDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ToDate", EndDate);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Sites != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Sites", Sites);
                dbCommand.Parameters.Add(empnoParm);
            }



            ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }


        public static DataSet Get2111ExcelDetails(string Agency, string Dept, string Prog, string Year, string strType,  string Sites,string strFundHie,string strFromDt,string strToDt)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            string strDate = string.Empty;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETENRL_HSSB2111NEWEXCEL]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Prog != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Prog", Prog);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Year", Year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Sites != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Site", Sites);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strFundHie != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FundHie", strFundHie);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strFromDt != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FromDate", strFromDt);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strToDt != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ToDate", strToDt);
                dbCommand.Parameters.Add(empnoParm);
            }            

            ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }


        public static DataSet GetHss22001Details(string Agency, string Dept, string Prog, string Year, string Show, string Sequence, string Language, string Classprefer, string Site, string Fund, string PreEnrolled, string SelectFund, string DateSelection, string FromDate, string ToDate, string SpecialNeeds, string DateType, string staffcode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            string strDate = string.Empty;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_HSS22001Details]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Agency", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Dept", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Prog != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Prog", Prog);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Year", Year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Show != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Show", Show);
                dbCommand.Parameters.Add(empnoParm);

            }

            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SITE", Site);
                dbCommand.Parameters.Add(empnoParm);

            }
            if (Language != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Language", Language);
                dbCommand.Parameters.Add(empnoParm);

            }
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Fund", Fund);
                dbCommand.Parameters.Add(empnoParm);

            }

            if (SelectFund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SelectFund", SelectFund);
                dbCommand.Parameters.Add(empnoParm);

            }

            if (Classprefer != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Classprefer", Classprefer);
                dbCommand.Parameters.Add(empnoParm);

            }

            if (PreEnrolled != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EnrollStatus", PreEnrolled);
                dbCommand.Parameters.Add(empnoParm);

            }

            if (DateSelection != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@DateSelection", DateSelection);
                dbCommand.Parameters.Add(empnoParm);

            }

            if (DateType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Type", DateType);
                dbCommand.Parameters.Add(empnoParm);

            }

            if (SpecialNeeds != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SpecialNeeds", SpecialNeeds);
                dbCommand.Parameters.Add(empnoParm);

            }


            if (Sequence != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Sequence", Sequence);
                dbCommand.Parameters.Add(empnoParm);

            }


            if (FromDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FromDate", FromDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (ToDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ToDate", ToDate);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (staffcode != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@StaffCode", staffcode);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }


        public static DataSet GetEnrollDetailsPIR2000(string Agency, string Dept, string Program, string Year)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            string strDate = string.Empty;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETENRL_PIR2000]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PROG", Program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }


            ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }


        public static DataSet GetPirWorkData(string Agency, string Dept, string Program, string Year,string strmode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            string strDate = string.Empty;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETPIRWORK]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PROG", Program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if(strmode !=string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Mode", strmode);
                dbCommand.Parameters.Add(empnoParm);                
            }

            ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }


        public static bool InsertDelPirWorkDETAILS(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertDelPIRWORK");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        public static bool INSERTUPDATEPIRWITHDRAW(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSERTUPDATEPIRWITHDRAW");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static DataSet GetGenerateworkDETAILS(string Agency, string Dept, string Program, string Year, string Mode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            string strDate = string.Empty;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[InsertDelPIRWORK]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PIRWORK_AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PIRWORK_DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PIRWORK_PROG", Program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PIRWORK_YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Mode != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Mode", Mode);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }


        public static DataSet GetPirCntlData()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            string strDate = string.Empty;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETPIRCNTL]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);


            ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }


        public static bool InsertDelPirCntl(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertDelPIRCNTL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static DataSet GetPirassnData(string Agency, string Dept, string Program, string Year)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            string strDate = string.Empty;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_PIRASSN]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PIRASSN_AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PIRASSN_DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PIRASSN_PROG", Program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PIRASSN_YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }



            ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }


        public static bool InsertDelPirassn(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelPIRASSN");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertDelPIRQUESTRECORD(List<SqlParameter> sqlParamList, out string strMsg)
        {
            bool boolstatus = false;
            string strerror = string.Empty;
            try
            {

                _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertPIRQUEST");
                _dbCommand.CommandTimeout = 1200;
                _dbCommand.Parameters.Clear();
                foreach (SqlParameter sqlPar in sqlParamList)
                {
                    _dbCommand.Parameters.Add(sqlPar);
                }
                boolstatus = _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                strerror = ex.Message;
                boolstatus = false;
            }
            strMsg = strerror;
            return boolstatus;
        }

        public static bool UpdatePIRMISC(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UPADTE_PIRMISC");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool UpdateAGYTab_Pir(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateAgytab_PIR");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        public static bool InsertPirTables(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertPirTables");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetChldAttnCountMonth(string Agency, string Dept, string Prog, string Year, string ApplNo, string Site, string Room, string Ampm, string FundingSource, string Month)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_CHLDATTNCOUNTMONTH]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Prog != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_PROGRAM", Prog);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (ApplNo != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_APP_NO", ApplNo);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_SITE", Site);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Room != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_ROOM", Room);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Ampm != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_AMPM", Ampm);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (FundingSource != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_FUNDING_SOURCE ", FundingSource);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Month != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ATTN_MONTH", Month);
                dbCommand.Parameters.Add(empnoParm);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertDelPirassn2(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_PIRASSN_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static DataSet GetPirassnData2(string Agency, string Dept, string Program, string Year)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            string strDate = string.Empty;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_PIRASSN_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PIRASSN_AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PIRASSN_DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PIRASSN_PROG", Program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PIRASSN_YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

    }

}
