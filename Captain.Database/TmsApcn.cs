using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

namespace Captain.DatabaseLayer
{


    [DataObject]
    [Serializable]
    public partial class TmsApcn
    {
        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[TMSAPCN]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;


        #endregion

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetTMSAPCN()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetTMSAPCN]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetTMSAPCNadpyldt(string Agency, string Dept, string Program, string Year, string Location, string Date, string Type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetTMSAPCNadpyldt]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@TMSAPCN_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@TMSAPCN_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@TMSAPCN_PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@TMSAPCN_YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (Location != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@TMSAPCN_LOCATION", Location);
                dbCommand.Parameters.Add(typeParm);
            }
            if (Date != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@TMSAPCN_DATE", Date);
                dbCommand.Parameters.Add(typeParm);
            }
            if (Type != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@TMSAPCN_TYPE", Type);
                dbCommand.Parameters.Add(typeParm);
            }


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetAPPTEMPLATESadpyldt(string Agency, string Dept, string Program, string Year, string Location, string FDate,string TDate, string Type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAP_APPTEMPLATES_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@APPTEMPL_AGY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@APPTEMPL_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@APPTEMPL_PROG", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@APPTEMPL_YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(Location.Trim()))
            {
                SqlParameter typeParm = new SqlParameter("@APPTEMPL_SITE", Location);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(FDate.Trim()))
            {
                SqlParameter typeParm = new SqlParameter("@APPTEMPL_FDATE", FDate);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(TDate.Trim()))
            {
                SqlParameter typeParm = new SqlParameter("@APPTEMPL_TDATE",TDate);
                dbCommand.Parameters.Add(typeParm);
            }
            //if (Type != string.Empty)
            //{
            //    SqlParameter typeParm = new SqlParameter("@APPTEMPL_TYPE", Type);
            //    dbCommand.Parameters.Add(typeParm);
            //}


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetAPPTEMPLATESadpysitedates(string Agency, string Dept, string Program, string Year, string Location, string FDate, string TDate, string Type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAP_APPTEMPLATES_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@APPTEMPL_AGY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@APPTEMPL_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@APPTEMPL_PROG", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year.Trim() != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@APPTEMPL_YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(Location.Trim()))
            {
                SqlParameter typeParm = new SqlParameter("@APPTEMPL_SITE", Location);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(FDate.Trim()))
            {
                SqlParameter typeParm = new SqlParameter("@APPTEMPL_FDATE", FDate);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(TDate.Trim()))
            {
                SqlParameter typeParm = new SqlParameter("@APPTEMPL_TDATE", TDate);
                dbCommand.Parameters.Add(typeParm);
            }
            if (Type != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Type", Type);
                dbCommand.Parameters.Add(typeParm);
            }


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetTMSAPPT()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetTMSAPPT]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetTMSAPPTadpyldt(string Agency, string Dept, string Program, string Year, string Location, string Date, string Time, string Templatedate,string templatetype)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetTMSAPPTadpyldt]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@TMSAPPT_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@TMSAPPT_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@TMSAPPT_PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@TMSAPPT_YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (Location != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@TMSAPPT_LOCATION", Location);
                dbCommand.Parameters.Add(typeParm);
            }
            if (Date != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@TMSAPPT_DATE", Date);
                dbCommand.Parameters.Add(typeParm);
            }
            if (Time != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@TMSAPPT_TIME", Time);
                dbCommand.Parameters.Add(typeParm);
            }

            if (Templatedate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@TMSAPPT_TEMPLATE_DATE", Templatedate);
                dbCommand.Parameters.Add(typeParm);
            }
            if (templatetype != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@TMSAPPT_TEMPLATE_TYPE", templatetype);
                dbCommand.Parameters.Add(typeParm);
            }


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetTMSAPCNSLTadpyldt(string Agency, string Dept, string Program, string Year, string Location, string Date, string Type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetTMSAPCNSLTadpyldt]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@TMSAPCNSLT_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@TMSAPCNSLT_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@TMSAPCNSLT_PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@TMSAPCNSLT_YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (Location != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@TMSAPCNSLT_LOCATION", Location);
                dbCommand.Parameters.Add(typeParm);
            }
            if (Date != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@TMSAPCNSLT_DATE", Date);
                dbCommand.Parameters.Add(typeParm);
            }

            if (Type != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@TMSAPCNSLT_TYPE", Type);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCaseNotesScreenFieldName(string ScreenName, string FieldName)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETCASENOTESScreeNFieldName]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (ScreenName != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@CASENOTES_SCREEN", ScreenName);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (FieldName != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@CASENOTES_KEYFIELD", FieldName);
                dbCommand.Parameters.Add(deptParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCaseNotesWaitingList(string FieldName)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETCASENOTESWaitingList]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            //if (ScreenName != string.Empty)
            //{
            //    SqlParameter agencyParm = new SqlParameter("@CASENOTES_SCREEN", ScreenName);
            //    dbCommand.Parameters.Add(agencyParm);
            //}
            if (FieldName != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@CASENOTES_KEYFIELD", FieldName);
                dbCommand.Parameters.Add(deptParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GETCASENOTESKeyFields(string ScreenName, string FieldName)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETCASENOTESKeyFields]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (ScreenName != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@CASENOTES_SCREEN", ScreenName);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (FieldName != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@CASENOTES_KEYFIELD", FieldName);
                dbCommand.Parameters.Add(deptParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataTable GETPROCHIST(string ScreenName, string KeyField, string UserID)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETPROCHIST]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (ScreenName != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@PRH_SCR_NAME", ScreenName);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (KeyField != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@PRH_KEYFIELD", KeyField);
                dbCommand.Parameters.Add(deptParm);
            }
            if (UserID != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@PRH_USER_ID", UserID);
                dbCommand.Parameters.Add(deptParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds.Tables[0];
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet BrowseCase_Notes(string ScreenName, string FieldName)//string Appno,
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_CASENOTES]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (ScreenName != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@CASENOTES_SCREEN", ScreenName);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (FieldName != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@CASENOTES_KEYFIELD", FieldName);
                dbCommand.Parameters.Add(deptParm);
            }
            //if (Appno != string.Empty)
            //{
            //    SqlParameter deptParm = new SqlParameter("@CASENOTES_APP_NO", Appno);
            //    dbCommand.Parameters.Add(deptParm);
            //}

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertUpdateTMSAPCN(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateTMSAPCN");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertUpdateAPPTEMPLATES(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAP_APPTEMPLATES_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertUpdateAPPTREASN(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAP_APPTREASN_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]

        public static DataSet GETAPPTREASONS(string Agency, string Dept, string Program, string Year, string site, string Date)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAP_APPTREASN_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            if (Agency != string.Empty)
            {
                SqlParameter matcodeParm = new SqlParameter("@APTRSN_AGY", Agency);
                dbCommand.Parameters.Add(matcodeParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@APTRSN_DEPT", Dept);
                dbCommand.Parameters.Add(scrcodeParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@APTRSN_PROG", Program);
                dbCommand.Parameters.Add(scrcodeParm);
            }


            if (Year != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@APTRSN_YEAR", Year);
                dbCommand.Parameters.Add(scrcodeParm);
            }

            if (site  != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@APTRSN_SITE", site);
                dbCommand.Parameters.Add(scrcodeParm);
            }

            if (Date != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@APTRSN_DATE", Date);
                dbCommand.Parameters.Add(scrcodeParm);
            }

           

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        public static bool UpdateTMSAPCN(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateTMSAPCN");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertTMSAPPT(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertTMSAPPT");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
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

        public static bool DeleteTMSAPCN(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteTMSAPCN");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool DeleteTMSAPPT(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteTMSAPPT");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertUpdateTMSAPCNSLT(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateTMSAPCNSLT");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        public static bool InsertUpdateDelCaseNotes(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelCASENOTES");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }
        public static bool InsertPROCHIST(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSDELPROCHIST");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }
        public static bool TmsApcnkeyCheck(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.TMSAPCNKEYCHECK");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }
        //

        public static bool APPTTEMPLCheck(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.APPTTEMPKEYCHECK");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


       

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCASESITEadpy(string Agency, string Dept, string Program, string Year,string strFilterType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetSITEadpy]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@SITE_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@SITE_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@SITE_PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@SITE_YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }

            if (strFilterType != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@FilterType", strFilterType);
                dbCommand.Parameters.Add(typeParm);
            }


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Letterview(string HieApp, string date, int seq)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[TMS00310_PrintLetter]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            SqlParameter sqlHieApp = new SqlParameter("@Liheapp_Key", HieApp);
            dbCmd.Parameters.Add(sqlHieApp);
            SqlParameter sqldate = new SqlParameter("@LAP_LETTER_DATE ", date);
            dbCmd.Parameters.Add(sqldate);
            SqlParameter sqlseq = new SqlParameter("@LAP1_CATEGORY_SEQ ", seq);
            dbCmd.Parameters.Add(sqlseq);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetLiheapp1det(string Agency, string Dept, string Prog, string Year, string App, string LetterId, string date, string Seq)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[GetLiheAPP1adpyalds]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            SqlParameter sqlAgency = new SqlParameter("@LAP1_AGENCY", Agency);
            dbCmd.Parameters.Add(sqlAgency);
            SqlParameter sqlDept = new SqlParameter("@LAP1_DEPT ", Dept);
            dbCmd.Parameters.Add(sqlDept);
            SqlParameter sqlProg = new SqlParameter("@LAP1_PROGRAM ", Prog);
            dbCmd.Parameters.Add(sqlProg);
            SqlParameter sqlYear = new SqlParameter("@LAP1_YEAR ", Year);
            dbCmd.Parameters.Add(sqlYear);
            SqlParameter sqlApp = new SqlParameter("@LAP1_APP ", App);
            dbCmd.Parameters.Add(sqlApp);
            SqlParameter sqlLetterId = new SqlParameter("@LAP1_LETTER_ID ", LetterId);
            dbCmd.Parameters.Add(sqlLetterId);
            SqlParameter sqlDate = new SqlParameter("@LAP1_LETTER_DATE ", date);
            dbCmd.Parameters.Add(sqlDate);
            SqlParameter sqlSeq = new SqlParameter("@LAP1_CATEGORY_SEQ ", Seq);
            dbCmd.Parameters.Add(sqlSeq);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet LetterPrint(string HieKey, string LetterId, string LetterDate)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[TMS00310_LetterBody]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            SqlParameter sqlHieKey = new SqlParameter("@lapHieKey", HieKey);
            dbCmd.Parameters.Add(sqlHieKey);
            SqlParameter sqlId = new SqlParameter("@lapLetterid ", LetterId);
            dbCmd.Parameters.Add(sqlId);
            SqlParameter sqlseq = new SqlParameter("@lapLetterDate ", LetterDate);
            dbCmd.Parameters.Add(sqlseq);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCaseNotesDesc(string UserId, string ModuleCode, string Hierarchy, string CaseNotesFieldName)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_CaseNotesDesc]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlUserName = new SqlParameter("@UserID", UserId);
            dbCommand.Parameters.Add(sqlUserName);
            SqlParameter sqlModuleCode = new SqlParameter("@ModuleCode", ModuleCode);
            dbCommand.Parameters.Add(sqlModuleCode);
            SqlParameter sqlHierarchy = new SqlParameter("@Hierarchy", Hierarchy);
            dbCommand.Parameters.Add(sqlHierarchy);
            SqlParameter sqlCaseNotesFieldName = new SqlParameter("@CaseNotesFieldName", CaseNotesFieldName);
            dbCommand.Parameters.Add(sqlCaseNotesFieldName);


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetPayment(string Agency, string Dept, string Program, string Year, string StartDate, string EndDate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetPayment]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@agency", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@Dept", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@prog", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@year", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (StartDate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@startdate", StartDate);
                dbCommand.Parameters.Add(typeParm);
            }
            if (EndDate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@enddate", EndDate);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static DataSet GetTMSB4017(string Agency, string Dept, string Program, string Year)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[TMSB4017_Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@AGENCY", Agency);
                dbCommand.Parameters.Add(typeParm);
            }

            if (Dept != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@DEPT", Dept);
                dbCommand.Parameters.Add(typeParm);
            }

            if (Program != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@PROG", Program);
                dbCommand.Parameters.Add(typeParm);
            }

            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool UpdatePayment(string Agency, string Dept, string Program, string Year, string AppNo, string VendorSeq, string RecSeq, string Benefit, string Fund)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.Update_Payment_TMSB4017");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgy = new SqlParameter("@AGENCY", Agency);
            _dbCommand.Parameters.Add(sqlAgy);

            SqlParameter sqlRankCd = new SqlParameter("@DEPT", Dept);
            _dbCommand.Parameters.Add(sqlRankCd);

            SqlParameter sqlSubCode = new SqlParameter("@PROG", Program);
            _dbCommand.Parameters.Add(sqlSubCode);

            SqlParameter sqlSubCode1 = new SqlParameter("@YEAR", Year);
            _dbCommand.Parameters.Add(sqlSubCode1);

            SqlParameter sqlSubCode2 = new SqlParameter("@APP_NO", AppNo);
            _dbCommand.Parameters.Add(sqlSubCode2);

            SqlParameter sqlSubCode3 = new SqlParameter("@VENDOR_SEQ", VendorSeq);
            _dbCommand.Parameters.Add(sqlSubCode3);

            SqlParameter sqlSubCode4 = new SqlParameter("@REC_SEQ", RecSeq);
            _dbCommand.Parameters.Add(sqlSubCode4);

            SqlParameter sqlSubCode5 = new SqlParameter("@BENEFIT", Benefit);
            _dbCommand.Parameters.Add(sqlSubCode5);

            SqlParameter sqlSubCode6 = new SqlParameter("@FUND", Fund);
            _dbCommand.Parameters.Add(sqlSubCode6);

            bool boolsuccess = _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
            return boolsuccess;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet TMSB0001_Report(string Agency, string Dept, string Program, string Year, string StartDate, string EndDate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[TMSB4001_REPORT]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@PROG", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (StartDate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@FROMDATE", StartDate);
                dbCommand.Parameters.Add(typeParm);
            }
            if (EndDate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@TODATE", EndDate);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet TMSB4007(string Agency, string Dept, string Program, string Year, string btnresult)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[TMSB4007]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            if(!string.IsNullOrEmpty(Agency.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@AGENCY", Agency);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(Dept.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@DEPT", Dept);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(Program.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@PROGRAM", Program);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(Year.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@YEAR ", Year);
                dbCmd.Parameters.Add(sqlYear);
            }
            SqlParameter sqlsorting = new SqlParameter("@SortingOrder ", btnresult);
            dbCmd.Parameters.Add(sqlsorting);
            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet TMSB4006(string Agency, string Dept, string Program, string Year, string FromDate, string ToDate, string BadRecs, string NPS, string SelSites, string Worker,string Type)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[TMSB4006_Report]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            dbCmd.CommandTimeout = 2400;

            if (!string.IsNullOrEmpty(Agency.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@AGENCY", Agency);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(Dept.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@DEPT", Dept);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(Program.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@PROGRAM", Program);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(Year.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@YEAR ", Year);
                dbCmd.Parameters.Add(sqlYear);
            }
            SqlParameter sqlFrmDate = new SqlParameter("@FROMDATE", FromDate);
            dbCmd.Parameters.Add(sqlFrmDate);

            SqlParameter sqlToDate = new SqlParameter("@TODATE", ToDate);
            dbCmd.Parameters.Add(sqlToDate);

            if (!string.IsNullOrEmpty(BadRecs.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@NegBal", BadRecs);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(NPS.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@NPS", NPS);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(SelSites.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@SelSites", SelSites);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(Worker.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@caseWorker", Worker);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(Type.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@TYPE", Type);
                dbCmd.Parameters.Add(sqlYear);
            }

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }
        /*public static DataSet TMSB4006(string Agency, string Dept, string Program, string Year,string FromDate,string ToDate,string BadRecs,string NPS)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[TMSB4006_Report]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            if (!string.IsNullOrEmpty(Agency.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@AGENCY", Agency);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(Dept.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@DEPT", Dept);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(Program.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@PROGRAM", Program);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(Year.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@YEAR ", Year);
                dbCmd.Parameters.Add(sqlYear);
            }
            SqlParameter sqlFrmDate = new SqlParameter("@FROMDATE", FromDate);
            dbCmd.Parameters.Add(sqlFrmDate);

            SqlParameter sqlToDate = new SqlParameter("@TODATE", ToDate);
            dbCmd.Parameters.Add(sqlToDate);

            if (!string.IsNullOrEmpty(BadRecs.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@NegBal", BadRecs);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(NPS.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@NPS", NPS);
                dbCmd.Parameters.Add(sqlYear);
            }

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }*/

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet TMSB4011_NONDeliver(string Agency, string Dept, string Program, string Year, string FromDate, string ToDate,string UserName)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[TMSB0011_NODELIVERS]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            if (!string.IsNullOrEmpty(Agency.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@AGENCY", Agency);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(Dept.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@DEPT", Dept);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(Program.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@PROGRAM", Program);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(Year.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@YEAR", Year);
                dbCmd.Parameters.Add(sqlYear);
            }
            SqlParameter sqlFrmDate = new SqlParameter("@FROMDATE", FromDate);
            dbCmd.Parameters.Add(sqlFrmDate);

            SqlParameter sqlToDate = new SqlParameter("@TODATE", ToDate);
            dbCmd.Parameters.Add(sqlToDate);

            if (!string.IsNullOrEmpty(UserName.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@USERNAME", UserName);
                dbCmd.Parameters.Add(sqlYear);
            }

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet TMSB4011_Deliver(string Agency, string Dept, string Program, string Year, string FromDate, string ToDate, string UserName,string Vendor,string Auth,string Unpay)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[TMSB4011_DELIVERS]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            if (!string.IsNullOrEmpty(Agency.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@AGENCY", Agency);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(Dept.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@DEPT", Dept);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(Program.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@PROGRAM", Program);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(Year.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@YEAR", Year);
                dbCmd.Parameters.Add(sqlYear);
            }
            SqlParameter sqlFrmDate = new SqlParameter("@FROMDATE", FromDate);
            dbCmd.Parameters.Add(sqlFrmDate);

            SqlParameter sqlToDate = new SqlParameter("@TODATE", ToDate);
            dbCmd.Parameters.Add(sqlToDate);

            if (!string.IsNullOrEmpty(UserName.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@USERNAME", UserName);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(Vendor.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@VENDOR", Vendor);
                dbCmd.Parameters.Add(sqlYear);
            }

            SqlParameter sqlAuth = new SqlParameter("@SWITCH", Auth);
            dbCmd.Parameters.Add(sqlAuth);

            SqlParameter sqlUnpay = new SqlParameter("@Unpayment", Unpay);
            dbCmd.Parameters.Add(sqlUnpay);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetAPPTScheduleBrowse(string Agency, string Dept, string Program, string Year, string Site, string Date,string strTime,string strSlot, string strSSN, string strUser, string strLName,string strFName,string strTel, string strDOB, string Type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAP_APPTSCHEDULE_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@APPTSCHD_AGY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@APPTSCHD_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@APPTSCHD_PROG", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHD_YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(Site.Trim()))
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHD_SITE", Site);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(Date.Trim()))
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHD_DATE", Date);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(strTime.Trim()))
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHD_TIME", strTime);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(strSlot.Trim()))
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHD_SLOT", strSlot);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(strSSN.Trim()))
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHD_SSN", strSSN);
                dbCommand.Parameters.Add(typeParm);
            }
            if(!string.IsNullOrEmpty(strUser))
            {
                SqlParameter typeParm = new SqlParameter("@User", strUser);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(strLName))
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHD_LNAME", strLName);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(strFName))
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHD_FNAME", strFName);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(strTel))
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHD_TEL", strTel);
                dbCommand.Parameters.Add(typeParm);
            }
            if(!string.IsNullOrEmpty(strDOB))
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHD_DOB", strDOB);
                dbCommand.Parameters.Add(typeParm);
            }
            //if (Type != string.Empty)
            //{
            //    SqlParameter typeParm = new SqlParameter("@APPTEMPL_TYPE", Type);
            //    dbCommand.Parameters.Add(typeParm);
            //}


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetAPPTSchdHistBrowse(string Agency, string Dept, string Program, string Year, string Site, string Date, string strTime, string strSlot, string strSSN, string strUser, string strLName, string strFName, string strTel, string strDOB, string Type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAP_APPTSCHDHIST_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@APPTSCHDHIST_AGY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@APPTSCHDHIST_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@APPTSCHDHIST_PROG", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHDHIST_YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(Site.Trim()))
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHDHIST_SITE", Site);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(Date.Trim()))
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHDHIST_DATE", Date);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(strTime.Trim()))
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHDHIST_TIME", strTime);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(strSlot.Trim()))
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHDHIST_SLOT", strSlot);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(strSSN.Trim()))
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHDHIST_SSN", strSSN);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(strUser))
            {
                SqlParameter typeParm = new SqlParameter("@User", strUser);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(strLName))
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHDHIST_LNAME", strLName);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(strFName))
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHDHIST_FNAME", strFName);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(strTel))
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHDHIST_TEL", strTel);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(strDOB))
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHDHIST_DOB", strDOB);
                dbCommand.Parameters.Add(typeParm);
            }
            //if (Type != string.Empty)
            //{
            //    SqlParameter typeParm = new SqlParameter("@APPTEMPL_TYPE", Type);
            //    dbCommand.Parameters.Add(typeParm);
            //}


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetAPPTScheduleSitesdates(string Agency, string Dept, string Program, string Year, string Site, string Date, string Month, string strYear, string Type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAP_APPTSCHEDULE_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@APPTSCHD_AGY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@APPTSCHD_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@APPTSCHD_PROG", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHD_YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(Site.Trim()))
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHD_SITE", Site);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(Date.Trim()))
            {
                SqlParameter typeParm = new SqlParameter("@APPTSCHD_DATE", Date);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(Month.Trim()))
            {
                SqlParameter typeParm = new SqlParameter("@Month", Month);
                dbCommand.Parameters.Add(typeParm);
            }
            if (!string.IsNullOrEmpty(strYear.Trim()))
            {
                SqlParameter typeParm = new SqlParameter("@Year", strYear);
                dbCommand.Parameters.Add(typeParm);
            }
            if (Type != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Type", Type);
                dbCommand.Parameters.Add(typeParm);
            }


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet TMSB0048_Report(string Agency, string Dept, string Program, string Year, string FromDate, string ToDate)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[TMSB0048_Report]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            if (!string.IsNullOrEmpty(Agency.Trim()))
            {
                SqlParameter sqlAgy = new SqlParameter("@AGENCY", Agency);
                dbCmd.Parameters.Add(sqlAgy);
            }

            if (!string.IsNullOrEmpty(Dept.Trim()))
            {
                SqlParameter sqlDept = new SqlParameter("@DEPT", Dept);
                dbCmd.Parameters.Add(sqlDept);
            }

            if (!string.IsNullOrEmpty(Program.Trim()))
            {
                SqlParameter sqlProg = new SqlParameter("@PROG", Program);
                dbCmd.Parameters.Add(sqlProg);
            }

            if (Year == "")
                Year = "    ";

            SqlParameter sqlYear = new SqlParameter("@YEAR", Year);
            dbCmd.Parameters.Add(sqlYear);

            SqlParameter sqlFrmDate = new SqlParameter("@FROMDATE", FromDate);
            dbCmd.Parameters.Add(sqlFrmDate);

            SqlParameter sqlToDate = new SqlParameter("@TODATE", ToDate);
            dbCmd.Parameters.Add(sqlToDate);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet TMSB0049_Report(string Agency, string Dept, string Program, string Year, string FromDate, string ToDate)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[TMSB0049_Report]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            if (!string.IsNullOrEmpty(Agency.Trim()))
            {
                SqlParameter sqlAgy = new SqlParameter("@AGENCY", Agency);
                dbCmd.Parameters.Add(sqlAgy);
            }

            if (!string.IsNullOrEmpty(Dept.Trim()))
            {
                SqlParameter sqlDept = new SqlParameter("@DEPT", Dept);
                dbCmd.Parameters.Add(sqlDept);
            }

            if (!string.IsNullOrEmpty(Program.Trim()))
            {
                SqlParameter sqlProg = new SqlParameter("@PROG", Program);
                dbCmd.Parameters.Add(sqlProg);
            }

            if (Year == "")
                Year = "    ";

            SqlParameter sqlYear = new SqlParameter("@YEAR", Year);
            dbCmd.Parameters.Add(sqlYear);

            SqlParameter sqlFrmDate = new SqlParameter("@FROMDATE", FromDate);
            dbCmd.Parameters.Add(sqlFrmDate);

            SqlParameter sqlToDate = new SqlParameter("@TODATE", ToDate);
            dbCmd.Parameters.Add(sqlToDate);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }
    }
}
