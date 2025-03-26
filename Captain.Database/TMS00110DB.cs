using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Captain.DatabaseLayer
{
    [DataObject]
    [Serializable]
    public partial class TMS00110DB
    {

        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[TMSAPCN]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;
        #endregion

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetTMSAPCNSitesByAgency(string Agency, string Location, string Date, string SiteDeac, string AppType, string Slot, string Min)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;


            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[TMS00110_GetTMSAPCNSitesByAgency]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (Agency != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@Agency", Agency);
                dbCommand.Parameters.Add(sqlAgency);
            }
            SqlParameter sqlLocation = new SqlParameter("@Location", Location);
            dbCommand.Parameters.Add(sqlLocation);
            SqlParameter sqlDate = new SqlParameter("@Date", Date);
            dbCommand.Parameters.Add(sqlDate);
            SqlParameter sqlSiteDeac = new SqlParameter("@SiteDesc", SiteDeac);
            dbCommand.Parameters.Add(sqlSiteDeac);
            SqlParameter sqlAppType = new SqlParameter("@AppType", AppType);
            dbCommand.Parameters.Add(sqlAppType);
            SqlParameter sqlSlot = new SqlParameter("@Slot", Slot);
            dbCommand.Parameters.Add(sqlSlot);
            SqlParameter SqlMin = new SqlParameter("@Min", Min);
            dbCommand.Parameters.Add(SqlMin);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet SearchTMSAPPTSchedule(string Hierarchy, string Year, string Location, string SiteDesc, string Date, string SchTime, string Slot, string WeekDay, string Month, string SYear)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            //Date = "2011-11-18";
            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[TMS00110_SearchTMSAPPTSchedule]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (Hierarchy != string.Empty)
            {
                SqlParameter sqlHierarchy = new SqlParameter("@Hierarchy", Hierarchy);
                dbCommand.Parameters.Add(sqlHierarchy);
            }
            SqlParameter sqlYear = new SqlParameter("@Year", Year);
            dbCommand.Parameters.Add(sqlYear);

            if (Location != string.Empty)
            {
                SqlParameter sqlLocation = new SqlParameter("@Location", Location);
                dbCommand.Parameters.Add(sqlLocation);
            }
            if (SiteDesc != string.Empty)
            {
                SqlParameter sqlSiteDesc = new SqlParameter("@SiteDesc", SiteDesc);
                dbCommand.Parameters.Add(sqlSiteDesc);
            }
            if (Date != string.Empty)
            {
                SqlParameter sqlDate = new SqlParameter("@Date", Date);
                dbCommand.Parameters.Add(sqlDate);
            }
            if (SchTime != string.Empty)
            {
                SqlParameter sqlSchTime = new SqlParameter("@Time", SchTime);
                dbCommand.Parameters.Add(sqlSchTime);
            }
            if (Slot != string.Empty)
            {
                SqlParameter sqlSlot = new SqlParameter("@Slot", Slot);
                dbCommand.Parameters.Add(sqlSlot);
            }
            if (WeekDay != string.Empty)
            {
                SqlParameter SqlMin = new SqlParameter("@WeekDay", WeekDay);
                dbCommand.Parameters.Add(SqlMin);
            }
            if (Month != string.Empty)
            {
                SqlParameter SqlMonth = new SqlParameter("@Month", Month);
                dbCommand.Parameters.Add(SqlMonth);
            }
            if (SYear != string.Empty)
            {
                SqlParameter SqlSYear = new SqlParameter("@SYear", SYear);
                dbCommand.Parameters.Add(SqlSYear);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetTMSAPCN_SlotDetails(string Hierarchy, string Year, string Location, string Date)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[TMS00110_GetTMSAPCN_SlotDetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (Hierarchy != string.Empty)
            {
                SqlParameter sqlHierarchy = new SqlParameter("@Hierarchy", Hierarchy + Year);
                dbCommand.Parameters.Add(sqlHierarchy);
            }
            SqlParameter sqlLocation = new SqlParameter("@Location", Location);
            dbCommand.Parameters.Add(sqlLocation);
            SqlParameter sqlDate = new SqlParameter("@Date", Date);
            dbCommand.Parameters.Add(sqlDate);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_TMSAPPT(string Hierarchy, string Location, string Date, string Time, string Slot, string Ssn, string Template_ype, string Record_Type,
                                            string AppName, string FirstName, string City, string State, string Zip, string Heat_Source, string Inc_Source, string Cnt_Person, string Cnt_Date, string Case_Worker, string User)
        {
            DataSet ds = null;
            try
            {



                Database db;
                string sqlCommand;
                DbCommand dbCommand;

                db = DatabaseFactory.CreateDatabase();
                sqlCommand = "[dbo].[TMS00110_Browse_TMSAPPT]";
                dbCommand = db.GetStoredProcCommand(sqlCommand);

                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (Hierarchy != string.Empty)
                {
                    SqlParameter sqlAgency = new SqlParameter("@Hierarchy", Hierarchy);
                    dbCommand.Parameters.Add(sqlAgency);
                }
                if (Location != string.Empty)
                {
                    SqlParameter sqlLocation = new SqlParameter("@Location", Location);
                    dbCommand.Parameters.Add(sqlLocation);
                }
                if (Date != string.Empty)
                {
                    SqlParameter sqlDate = new SqlParameter("@Date", Date);
                    dbCommand.Parameters.Add(sqlDate);
                }
                if (Time != string.Empty)
                {
                    SqlParameter SqlTime = new SqlParameter("@Time", Time);
                    dbCommand.Parameters.Add(SqlTime);
                }
                if (Slot != string.Empty)
                {
                    SqlParameter sqlSlot = new SqlParameter("@Slot", Slot);
                    dbCommand.Parameters.Add(sqlSlot);
                }
                if (Ssn != string.Empty)
                {
                    SqlParameter SqlSsn = new SqlParameter("@Ssn", Ssn);
                    dbCommand.Parameters.Add(SqlSsn);
                }
                if (Template_ype != string.Empty)
                {
                    SqlParameter sqlTemplate_ype = new SqlParameter("@Template_Type", Template_ype);
                    dbCommand.Parameters.Add(sqlTemplate_ype);
                }
                if (Record_Type != string.Empty)
                {
                    SqlParameter sqlRecord_Type = new SqlParameter("@Record_Type", Record_Type);
                    dbCommand.Parameters.Add(sqlRecord_Type);
                }
                if (AppName != string.Empty)
                {
                    SqlParameter SqlAppName = new SqlParameter("@AppName", AppName);
                    dbCommand.Parameters.Add(SqlAppName);
                }
                if (User != string.Empty)
                {
                    SqlParameter SqlUser = new SqlParameter("@User", User);
                    dbCommand.Parameters.Add(SqlUser);
                }
                //SqlParameter SqlFirstName = new SqlParameter("@AppFirstName", FirstName);
                //dbCommand.Parameters.Add(SqlFirstName);


                //SqlParameter SqlCity = new SqlParameter("@City", City); 
                //dbCommand.Parameters.Add(SqlCity);
                //SqlParameter SqlState = new SqlParameter("@State", State);
                //dbCommand.Parameters.Add(SqlState);
                //SqlParameter SqlZip = new SqlParameter("@Zip", Zip); 
                //dbCommand.Parameters.Add(SqlZip);
                //SqlParameter SqlHeat_Source = new SqlParameter("@Heat_Source", Heat_Source);
                //dbCommand.Parameters.Add(SqlHeat_Source);
                //SqlParameter SqlInc_Source = new SqlParameter("@Income_Source", Inc_Source); 
                //dbCommand.Parameters.Add(SqlInc_Source);
                //SqlParameter SqlCnt_person = new SqlParameter("@Contact_person", Cnt_Person);
                //dbCommand.Parameters.Add(SqlCnt_person);
                //SqlParameter SqlCnt_Date = new SqlParameter("@Contact_Date", Cnt_Date); 
                //dbCommand.Parameters.Add(SqlCnt_Date);
                //SqlParameter SqlCase_Worker = new SqlParameter("@Case_Worker", Case_Worker);
                //dbCommand.Parameters.Add(SqlCase_Worker);


                ds = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {


            }
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet TMS00110_Browse_TMSAPPT_SSNSearch(string Hierarchy, string Ssn, string AppName, string FirstName, string User, string Phone)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[TMS00110_Browse_TMSAPPT_SSNSearch]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (Hierarchy != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@Hierarchy", Hierarchy);
                dbCommand.Parameters.Add(sqlAgency);
            }
            if (Ssn != string.Empty)
            {
                SqlParameter SqlSsn = new SqlParameter("@Ssn", Ssn);
                dbCommand.Parameters.Add(SqlSsn);
            }
            if (AppName != string.Empty)
            {
                SqlParameter SqlAppName = new SqlParameter("@AppName", AppName);
                dbCommand.Parameters.Add(SqlAppName);
            }
            if (User != string.Empty)
            {
                SqlParameter SqlUser = new SqlParameter("@User", User);
                dbCommand.Parameters.Add(SqlUser);
            }
            if (FirstName != string.Empty)
            {
                SqlParameter SqlUser = new SqlParameter("@AppFirstName", FirstName);
                dbCommand.Parameters.Add(SqlUser);
            }

            if (Phone != string.Empty)
            {
                SqlParameter SqlPhone = new SqlParameter("@Phone", Phone);
                dbCommand.Parameters.Add(SqlPhone);
            }

            //SqlParameter SqlFirstName = new SqlParameter("@AppFirstName", FirstName);
            //dbCommand.Parameters.Add(SqlFirstName);


            //SqlParameter SqlCity = new SqlParameter("@City", City); 
            //dbCommand.Parameters.Add(SqlCity);
            //SqlParameter SqlState = new SqlParameter("@State", State);
            //dbCommand.Parameters.Add(SqlState);
            //SqlParameter SqlZip = new SqlParameter("@Zip", Zip); 
            //dbCommand.Parameters.Add(SqlZip);
            //SqlParameter SqlHeat_Source = new SqlParameter("@Heat_Source", Heat_Source);
            //dbCommand.Parameters.Add(SqlHeat_Source);
            //SqlParameter SqlInc_Source = new SqlParameter("@Income_Source", Inc_Source); 
            //dbCommand.Parameters.Add(SqlInc_Source);
            //SqlParameter SqlCnt_person = new SqlParameter("@Contact_person", Cnt_Person);
            //dbCommand.Parameters.Add(SqlCnt_person);
            //SqlParameter SqlCnt_Date = new SqlParameter("@Contact_Date", Cnt_Date); 
            //dbCommand.Parameters.Add(SqlCnt_Date);
            //SqlParameter SqlCase_Worker = new SqlParameter("@Case_Worker", Case_Worker);
            //dbCommand.Parameters.Add(SqlCase_Worker);


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_Reserve_SiteByCode(string Hierarchy, string Site)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[TMS00120_Get_Reserve_SiteByCode]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlHierarchy = new SqlParameter("@Hierarchy", Hierarchy);
            dbCommand.Parameters.Add(sqlHierarchy);

            if (!string.IsNullOrEmpty(Site))
            {
                SqlParameter sqlSite = new SqlParameter("@Site", Site);
                dbCommand.Parameters.Add(sqlSite);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        public static bool UpdateTMSAPPT(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateTMSAPPT");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertUpdateDelAPPTSCHED(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAP_APPTSCHEDULE_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertUpdateDelAPPTSCHEDHIST(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAP_APPTSCHDHIST_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool DeleteTMSAPPT(string Hierarchy, string Location, string date, string Time, string Slot, string SSN)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.TMS00110_Delete_TMSAPPT");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlHierarchy = new SqlParameter("@TMSAPPT_HIERARCHY", Hierarchy);
            _dbCommand.Parameters.Add(sqlHierarchy);
            SqlParameter sqlLocation = new SqlParameter("@TMSAPPT_LOCATION", Location);
            _dbCommand.Parameters.Add(sqlLocation);
            SqlParameter sqldate = new SqlParameter("@TMSAPPT_DATE", date);
            _dbCommand.Parameters.Add(sqldate);
            SqlParameter sqlTime = new SqlParameter("@TMSAPPT_TIME", Time);
            _dbCommand.Parameters.Add(sqlTime);
            SqlParameter sqlSlot = new SqlParameter("@TMSAPPT_SLOT_NUMBER ", Slot);
            _dbCommand.Parameters.Add(sqlSlot);
            SqlParameter sqlSSN = new SqlParameter("@TMSAPPT_SS_NUMBER ", SSN);
            _dbCommand.Parameters.Add(sqlSSN);

            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }




        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool Delete_NowSch_TMSAPPT(string Hierarchy, string Location, string date, string User)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.TMS00110_Delete_NowSch_TMSAPPT");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlHierarchy = new SqlParameter("@TMSAPPT_HIERARCHY", Hierarchy);
            _dbCommand.Parameters.Add(sqlHierarchy);
            SqlParameter sqlLocation = new SqlParameter("@TMSAPPT_LOCATION", Location);
            _dbCommand.Parameters.Add(sqlLocation);
            SqlParameter sqldate = new SqlParameter("@TMSAPPT_DATE", date);
            _dbCommand.Parameters.Add(sqldate);
            //SqlParameter sqlTime = new SqlParameter("@TMSAPPT_TIME", Time);
            //_dbCommand.Parameters.Add(sqlTime);
            //SqlParameter sqlSlot = new SqlParameter("@TMSAPPT_SLOT_NUMBER ", Slot);
            //_dbCommand.Parameters.Add(sqlSlot);
            //SqlParameter sqlSSN = new SqlParameter("@TMSAPPT_SS_NUMBER ", SSN);
            //_dbCommand.Parameters.Add(sqlSSN);
            SqlParameter sqlUser = new SqlParameter("@User", User);
            _dbCommand.Parameters.Add(sqlUser);

            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        //    TMS00120 Stored Procedures Begin

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_Template_APPT_ByDate(string Tmsapcn_Key, string Sel_From_Date, string Sel_To_Date)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[TMS00120_Get_Template_APPT_ByDate]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlTmsapcn_Key = new SqlParameter("@Apcn_Key", Tmsapcn_Key);
            dbCommand.Parameters.Add(sqlTmsapcn_Key);
            SqlParameter sqlFromDate = new SqlParameter("@SelDate", Sel_From_Date);
            dbCommand.Parameters.Add(sqlFromDate);

            if (!string.IsNullOrEmpty(Sel_To_Date))
            {
                SqlParameter sqlToDate = new SqlParameter("@SelToDate", Sel_To_Date);
                dbCommand.Parameters.Add(sqlToDate);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }
    }
}
