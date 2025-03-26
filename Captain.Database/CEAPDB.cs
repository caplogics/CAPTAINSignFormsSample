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
    public partial class CEAPDB
    {

        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[AGYTAB]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;
        #endregion
            
      
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CAPS_CEAPB002_GET(string agency, string dep, string program, string year,  string Site, string Worker,string ProcessReport, string Fromdate, string Todate, string strMode, string spdomain)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_CEAPB002_REP]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

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
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@YEAR", year);
                dbCommand.Parameters.Add(empnoParm);
            }
          
            if (Site.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Site", Site);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Worker.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEWORKER", Worker);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (ProcessReport.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PROCESSREPORT", ProcessReport);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fromdate.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FromDate", Fromdate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Todate.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ToDate", Todate);
                dbCommand.Parameters.Add(empnoParm);
            }  

            if (strMode.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Mode", strMode);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (spdomain.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SPDOMAIN", spdomain);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        #region Addded by Vikash on 06/12/2024 for new Invoice Commitment Report

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet BROWSE_INVOICE_COMMITMENT(string Agy, string Dept, string Prog, string Year, string County, string Startdate, string Enddate, string Fund, string Budget)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[BROWSE_INVOICE_COMMITMENT]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

            if (Agy != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AGY", Agy);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Prog != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PROG", Prog);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (County.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@COUNTY", County);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Startdate.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@STARTDATE", Startdate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Enddate.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ENDDATE", Enddate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Budget.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BUDGET", Budget);
                dbCommand.Parameters.Add(empnoParm);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        #endregion

        // Added by Vikash on 11/14/2024 to add CEAP History to CEAP Control
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool InsertCeapHist(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_CEAPHIST_INSERT");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]

        public static DataSet GetCeapHist(string tblName, string key, string screenName)

        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_CEAPHIST_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (tblName != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CEAP_HIST_TBLNAME", tblName);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (key != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CEAP_HIST_TBLKEY", key);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (screenName != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CEAP_HIST_SCREEN", screenName);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }
    }
}
