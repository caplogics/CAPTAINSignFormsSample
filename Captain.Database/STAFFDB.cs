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
    public partial class STAFFDB
    {

        #region Constants
        
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;

        #endregion

        #region Addded by Vikash on 08/20/2024 for new Professional Development

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool CAPS_STFTR_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            try
            {
                _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_STFTR_INSUPDEL");
                _dbCommand.CommandTimeout = 1200;
                _dbCommand.Parameters.Clear();
                foreach (SqlParameter sqlPar in sqlParamList)
                {
                    _dbCommand.Parameters.Add(sqlPar);
                }
            }
            catch (Exception ex){ }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_STFTRAIN(string Agency, string Dept, string Prog, string Year, string ID)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_STFTR_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@STFTR_AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@STFTR_DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Prog != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@STFTR_PROG", Prog);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@STFTR_YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (ID != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@STFTR_ID", ID);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool CAP_SPDBPOST_INSUPDATE(List<SqlParameter> sqlParamList)
        {
            try
            {
                _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAP_SPDBPOST_INSUPDATE");
                _dbCommand.CommandTimeout = 1200;
                _dbCommand.Parameters.Clear();
                foreach (SqlParameter sqlPar in sqlParamList)
                {
                    _dbCommand.Parameters.Add(sqlPar);
                }
            }
            catch (Exception ex) { }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_StaffBulkPost(string Agency, string Dept, string Prog, string Year, string staff_code)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_SPDBPOST_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SPDB_AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SPDB_DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Prog != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SPDB_PROGRAM", Prog);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SPDB_YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (staff_code != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SPDB_STAFF_CODE", staff_code);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }




        #endregion


    }
}
