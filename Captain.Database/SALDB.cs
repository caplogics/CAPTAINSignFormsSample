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
    public partial class SALDB
    {

        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[AGYTAB]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;
        #endregion

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_SALDEF(List<SqlParameter> sqlParamList,string UserID,string AdminAgency)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAP_SALDEF_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            if(!string.IsNullOrEmpty(UserID.Trim()))
            {
                SqlParameter empnoParm = new SqlParameter("@IP_UserID", UserID);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (!string.IsNullOrEmpty(AdminAgency.Trim()))
            {
                SqlParameter empnoParm = new SqlParameter("@IP_AGENCY", AdminAgency);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }
       
        public static string  CAP_SALQUES_ID()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAP_SALQUES_ID]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);           

            ds = db.ExecuteDataSet(dbCommand);

            return ds.Tables[0].Rows[0][0].ToString();
        }

        

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_SALQUES(List<SqlParameter> sqlParamList)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAP_SALQUES_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_SALQRESP(List<SqlParameter> sqlParamList)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAP_SALQRESP_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_SALACT(List<SqlParameter> sqlParamList)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAP_SALACT_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_CALCONT(List<SqlParameter> sqlParamList)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAP_CALCONT_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        public static bool CAP_SALDEF_INSUPDEL(List<SqlParameter> sqlParamList, out string Sql_Reslut_Msg)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAP_SALDEF_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            SqlParameter SP_Result_Msg = new SqlParameter("@SP_Result_Msg", SqlDbType.VarChar, 8000);
            SP_Result_Msg.Direction = ParameterDirection.Output;
            sqlParamList.Add(SP_Result_Msg);

            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }

            bool SQL_Response = false;

            SQL_Response = _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
            Sql_Reslut_Msg = SP_Result_Msg.Value.ToString();

            return SQL_Response;
            //return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }
        public static bool CAP_SALQUES_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAP_SALQUES_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool CAP_SALQRESP_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAP_SALQRESP_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool CAP_SALACT_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAP_SALACT_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool CAP_SALQLNK_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAP_SALQLNK_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool CAP_CALCONT_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAP_CALCONT_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCase0009_REPORT(string agency, string dep, string program, string year, string Fromdate, string Todate,string Type,string SALS)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAP_SALCAL_Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 2400;

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year != string.Empty)
            {
                SqlParameter empnoParm1 = new SqlParameter("@YEAR", year);
                dbCommand.Parameters.Add(empnoParm1);
            }

            if (Fromdate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@STARTDATE", Fromdate);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Todate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ENDDATE", Todate);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Type != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@TYPE", Type);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (SALS != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SELSALS", SALS);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ARSb2150_Report(string Agency, string Dept, string Prog,string Year, string Appno)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[ARSB2150_Report]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgency = new SqlParameter("@ARS_AGENCY", Agency);
            dbCmd.Parameters.Add(sqlAgency);

            SqlParameter sqlCity = new SqlParameter("@ARS_DEPT", Dept);
            dbCmd.Parameters.Add(sqlCity);

            SqlParameter sqlZip1 = new SqlParameter("@ARS_PROGRAM ", Prog);
            dbCmd.Parameters.Add(sqlZip1);

            SqlParameter sqlYear = new SqlParameter("@MST_YEAR ", Year);
            dbCmd.Parameters.Add(sqlYear);

            if (!string.IsNullOrEmpty(Appno))
            {
                SqlParameter sqlZip2 = new SqlParameter("@ARS_APP_NO", Appno);
                dbCmd.Parameters.Add(sqlZip2);
            }
            

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_SALQLNK( string strQuestion, string strGroup, string strQlnk)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[CAP_SALQLNK_BROWSE]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (strQuestion != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@IP_Q_ID", strQuestion);
                dbCmd.Parameters.Add(sqlAgency);
            }

            if (strGroup != string.Empty)
            {
                SqlParameter sqlCity = new SqlParameter("@IP_GROUP", strGroup);
                dbCmd.Parameters.Add(sqlCity);
            }

            if (strQlnk != string.Empty)
            {
                SqlParameter sqlZip1 = new SqlParameter("@IP_LINKQ ", strQlnk);
                dbCmd.Parameters.Add(sqlZip1);
            }           

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }



    }
}
