using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.ObjectBuilder2;

namespace Captain.DatabaseLayer
{
    [DataObject]
    [Serializable]
    public partial class HUDCNTLDB
    {
        #region Constants

        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;

        #endregion


        public static bool CAPS_HUDCNTL_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_HUDCNTL_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_HUDCNTL(string agency)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_HUDCNTL_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDC_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        public static bool CAPS_HUDSTAFF_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_HUDSTAFF_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_HUDSTAFF(string agency, string staff_code, string type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_HUDSTAFF_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDS_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (staff_code != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDS_STF_CODE", staff_code);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (type != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDS_TYPE", type);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool CAPS_HUDTRAIN_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_HUDTRAIN_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_HUDTRAIN(string agency)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_HUDTRAIN_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDT_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool CAPS_HTCATTN_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_HTCATTN_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_HTCATTN(string agency, string Seq, string staffCode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_HTCATTN_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HTC_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Seq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HTC_HUD_SEQ", Seq);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (staffCode != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HTC_STAFF_CODE", staffCode);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool CAPS_HUDGROUP_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_HUDGROUP_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_HUDGROUP(string agency, string Seq)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_HUDGROUP_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDG_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Seq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDG_SEQ", Seq);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool CAPS_HUDMST_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_HUDMST_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_HUDMST(string agency, string dept, string prog, string year, string appno, string seq)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_HUDMST_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDMST_AGY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDMST_DEPT", dept);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (prog != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDMST_PROG", prog);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDMST_YEAR", year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (appno != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDMST_APPNO", appno);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (seq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDMST_SEQ", seq);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool CAPS_HUDINDIV_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_HUDINDIV_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_HUDINDIV(string agency, string dept, string prog, string year, string appno, string seq, string mst_seq)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_HUDINDIV_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDIND_AGY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDIND_DEPT", dept);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (prog != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDIND_PROG", prog);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDIND_YEAR", year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (appno != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDIND_APPNO", appno);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (seq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDIND_SEQ", seq);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (mst_seq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDMST_SEQ", mst_seq);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool CAPS_HUDIMPACT_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_HUDIMPACT_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_HUDIMPACT(string agency, string dept, string prog, string year, string appno, string seq, string Indv_Seq, string Mst_seq)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_HUDIMPACT_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDIMPACT_AGY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDIMPACT_DEPT", dept);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (prog != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDIMPACT_PROG", prog);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDIMPACT_YEAR", year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (appno != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDIMPACT_APPNO", appno);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (seq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDIMPACT_SEQ", seq);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Mst_seq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDMST_SEQ", Mst_seq);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Indv_Seq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HUDIND_SEQ", Indv_Seq);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        //Added by Vikash on 11/29/2024 for getting counts to HUD 9902 report
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_HUD_Counts(string agency, string dept, string prog, string year, string fromdate, string todate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_GET_HUDCOUNTS]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@DEPT", dept);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (prog != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PROG", prog);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@YEAR", year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (fromdate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FROM_DATE", fromdate);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (todate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@TO_DATE", todate);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

    }
}
