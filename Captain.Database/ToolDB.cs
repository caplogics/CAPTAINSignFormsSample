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
    public partial class Tooldb
    {

        #region Constants
        private static readonly string TABLE_NAME = "[dbo].[AGYTAB]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;
        #endregion


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Tool0001_Report(string Agency,string Dept,string Prog,string Year,string Site,string Room,string AMPM)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[TOOL0001_Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlSP_code = new SqlParameter("@AGENCY", Agency);
            dbCommand.Parameters.Add(sqlSP_code);
            SqlParameter sqlBR_Code = new SqlParameter("@DEPT", Dept);
            dbCommand.Parameters.Add(sqlBR_Code);
            SqlParameter sqlCA_Desc = new SqlParameter("@PROGRAM", Prog);
            dbCommand.Parameters.Add(sqlCA_Desc);
            SqlParameter sqlCA_Active_SW = new SqlParameter("@YEAR", Year);
            dbCommand.Parameters.Add(sqlCA_Active_SW);
            SqlParameter sqlCA_Site = new SqlParameter("@SITE", Site);
            dbCommand.Parameters.Add(sqlCA_Site);
            SqlParameter sqlCA_Room = new SqlParameter("@ROOM", Room);
            dbCommand.Parameters.Add(sqlCA_Room);
            SqlParameter sqlCA_AMPM = new SqlParameter("@AMPM", AMPM);
            dbCommand.Parameters.Add(sqlCA_AMPM);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool Update_TOOL0001(string Agency, string Dept, string Prog, string Year,string FromSite, string ToSite, string ToRoom, string ToAMPM,string FrmFldSite)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UPDATE_TOOL0001");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgy = new SqlParameter("@AGENCY", Agency);
            _dbCommand.Parameters.Add(sqlAgy);
            SqlParameter sqldept = new SqlParameter("@DEPT", Dept);
            _dbCommand.Parameters.Add(sqldept);
            SqlParameter sqlprog = new SqlParameter("@PROGRAM", Prog);
            _dbCommand.Parameters.Add(sqlprog);
            SqlParameter sqlyear = new SqlParameter("@YEAR", Year);
            _dbCommand.Parameters.Add(sqlyear);
            SqlParameter sqlfrmsite = new SqlParameter("@FRMSITE", FromSite);
            _dbCommand.Parameters.Add(sqlfrmsite);
            SqlParameter sqlTosite = new SqlParameter("@TOSITE", ToSite);
            _dbCommand.Parameters.Add(sqlTosite);
            SqlParameter sqltoRoom = new SqlParameter("@TOROOM", ToRoom);
            _dbCommand.Parameters.Add(sqltoRoom);
            SqlParameter sqltoampm = new SqlParameter("@TOAMPM", ToAMPM);
            _dbCommand.Parameters.Add(sqltoampm);
            SqlParameter sqlfldsite = new SqlParameter("@FromFldSite", FrmFldSite);
            _dbCommand.Parameters.Add(sqlfldsite);

            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

    }
}
