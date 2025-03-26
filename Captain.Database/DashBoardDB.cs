using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

namespace Captain.DatabaseLayer
{

    [DataObject]
    [Serializable]
    public partial class DashBoardDB
    {

        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[AGYTAB]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        //private static DbCommand _dbCommand;
        #endregion



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GET_INTAKE_DATA(string strUserId, string strDate, string strType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_RDLC_DATA]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (strUserId != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@USERID", strUserId);
                dbCommand.Parameters.Add(sqlAgency);
            }
            if (strDate != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@StartDate", strDate);
                dbCommand.Parameters.Add(sqlDept);
            }
            if (strType != string.Empty)
            {
                SqlParameter sqlProg = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(sqlProg);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GET_INTAKE_DATA1(string strUserId, string strDate, string strType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_RDLC_DATA1]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (strUserId != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@USERID", strUserId);
                dbCommand.Parameters.Add(sqlAgency);
            }
            if (strDate != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@StartDate", strDate);
                dbCommand.Parameters.Add(sqlDept);
            }
            if (strType != string.Empty)
            {
                SqlParameter sqlProg = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(sqlProg);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GETRDLCALLDATA(string strUserId, string strstartDate, string strEndDate, string strtbtype, string strType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETRDLCALLDATA]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (strUserId != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@USERID", strUserId);
                dbCommand.Parameters.Add(sqlAgency);
            }

            if (strtbtype != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@tabletypes", strtbtype);
                dbCommand.Parameters.Add(sqlDept);
            }
            if (strstartDate != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@StartDate", strstartDate);
                dbCommand.Parameters.Add(sqlDept);
            }
            if (strEndDate != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@EndDate", strstartDate);
                dbCommand.Parameters.Add(sqlDept);
            }
            if (strType != string.Empty)
            {
                SqlParameter sqlProg = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(sqlProg);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GETRDLCALLDATANew123(string strUserId, string strstartDate, string strEndDate, string strtbtype, string strType, string strusers, string strprogrames, string strDefagency)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETRDLCALLDATANEW]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (strUserId != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@USERID", strUserId);
                dbCommand.Parameters.Add(sqlAgency);
            }

            if (strtbtype != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@tabletypes", strtbtype);
                dbCommand.Parameters.Add(sqlDept);
            }
            if (strstartDate != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@StartDate", strstartDate);
                dbCommand.Parameters.Add(sqlDept);
            }
            if (strEndDate != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@EndDate", strstartDate);
                dbCommand.Parameters.Add(sqlDept);
            }
            if (strType != string.Empty)
            {
                SqlParameter sqlProg = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(sqlProg);
            }
            if (strusers != string.Empty)
            {
                SqlParameter sqlProg = new SqlParameter("@Users", strusers);
                dbCommand.Parameters.Add(sqlProg);
            }
            if (strprogrames != string.Empty)
            {
                SqlParameter sqlProg = new SqlParameter("@Programs", strprogrames);
                dbCommand.Parameters.Add(sqlProg);
            }
            if (strDefagency != string.Empty)
            {
                SqlParameter sqlProg = new SqlParameter("@DefAgency", strDefagency);
                dbCommand.Parameters.Add(sqlProg);
            }


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GETRDLCALLDATANEW(string strUserId, string strAddDate, string strLstcDate, string strIntakeDate, string strCertifiedDate, string strtbtype, string strType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETRDLCALLDATANEW]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (strUserId != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@USERID", strUserId);
                dbCommand.Parameters.Add(sqlAgency);
            }
            if (strAddDate != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@AddDate", strAddDate);
                dbCommand.Parameters.Add(sqlDept);
            }
            if (strLstcDate != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@LstcDate", strAddDate);
                dbCommand.Parameters.Add(sqlDept);
            }
            if (strIntakeDate != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@IntakeDate", strIntakeDate);
                dbCommand.Parameters.Add(sqlDept);
            }
            if (strCertifiedDate != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@Certified", strCertifiedDate);
                dbCommand.Parameters.Add(sqlDept);
            }
            if (strtbtype != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@tabletypes", strtbtype);
                dbCommand.Parameters.Add(sqlDept);
            }
            if (strType != string.Empty)
            {
                SqlParameter sqlProg = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(sqlProg);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet getDashboardSplRpt(string strMonth, string strYear, string strAgency, string strMode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[DASHBOARD_SPL_RPT]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (strMonth != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@MONTH", strMonth);
                dbCommand.Parameters.Add(sqlAgency);
            }
            if (strYear != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@YEAR", strYear);
                dbCommand.Parameters.Add(sqlDept);
            }
            if (strAgency != string.Empty)
            {
                SqlParameter sqlProg = new SqlParameter("@DefAgency", strAgency);
                dbCommand.Parameters.Add(sqlProg);
            }
            if (strMode != string.Empty)
            {
                SqlParameter sqlProg = new SqlParameter("@MODE", strMode);
                dbCommand.Parameters.Add(sqlProg);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }
    }
}
