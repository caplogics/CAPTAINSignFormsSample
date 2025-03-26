/***********************************************************
 * Author : Kranthi
 * Date   : 12/29/2022
 * ********************************************************/

#region using
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace Captain.DatabaseLayer
{
    [DataObject]
    [Serializable]

    public class SmartChoiceDB
    {
        #region Constants
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;
        #endregion

        #region Get Methods
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static int getMaxID(string Table, string FieldName)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_CAPMAXID]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            int ID;

            if (Table != string.Empty)
            {
                SqlParameter empNoParm = new SqlParameter("@TABLE_NAME", Table);
                dbCommand.Parameters.Add(empNoParm);
            }
            if (FieldName != string.Empty)
            {
                SqlParameter firstNameParm = new SqlParameter("@FIELD_NAME", FieldName);
                dbCommand.Parameters.Add(firstNameParm);
            }
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["ID"].ToString() != "")
            {
                ID = Convert.ToInt32(ds.Tables[0].Rows[0]["id"]);
            }
            else
            {
                ID = 1;
            }
            return ID;
        }
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataTable SMACHASSOC_GET(string SMACH_ASSOC_CAT_ID, string MODE)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[SMACHASSOC_GET]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            dbCmd.CommandTimeout = 1800;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            if (SMACH_ASSOC_CAT_ID != string.Empty)
            {
                SqlParameter sqlp1 = new SqlParameter("@SMACH_ASSOC_CAT_ID", SMACH_ASSOC_CAT_ID);
                dbCmd.Parameters.Add(sqlp1);
            }


            if (MODE != string.Empty)
            {
                SqlParameter sqlp10 = new SqlParameter("@MODE ", MODE);
                dbCmd.Parameters.Add(sqlp10);
            }

            ds = db.ExecuteDataSet(dbCmd);
            return ds.Tables[0];
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static lstSNPNames getSNPNames(string AGY,string DEPT,string PROG,string YEAR,string APPNO,string MODE)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;
            lstSNPNames olstNames = new lstSNPNames();

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[SMACHASSOC_GET]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            dbCmd.CommandTimeout = 1800;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            if (AGY != string.Empty)
            {
                SqlParameter sqlp1 = new SqlParameter("@AGY", AGY);
                dbCmd.Parameters.Add(sqlp1);
            }
            if (DEPT != string.Empty)
            {
                SqlParameter sqlp1 = new SqlParameter("@DEPT", DEPT);
                dbCmd.Parameters.Add(sqlp1);
            }

            if (PROG != string.Empty)
            {
                SqlParameter sqlp1 = new SqlParameter("@PROG", PROG);
                dbCmd.Parameters.Add(sqlp1);
            }

            if (YEAR != string.Empty)
            {
                SqlParameter sqlp1 = new SqlParameter("@YEAR", YEAR);
                dbCmd.Parameters.Add(sqlp1);
            }

            if (APPNO != string.Empty)
            {
                SqlParameter sqlp1 = new SqlParameter("@APPNO", APPNO);
                dbCmd.Parameters.Add(sqlp1);
            }



            if (MODE != string.Empty)
            {
                SqlParameter sqlp10 = new SqlParameter("@MODE ", MODE);
                dbCmd.Parameters.Add(sqlp10);
            }

            ds = db.ExecuteDataSet(dbCmd);
            if (ds.Tables[0].Rows.Count > 0) {
                olstNames.FullName= ds.Tables[0].Rows[0]["FULLNAME"].ToString();
                olstNames.FirstName = ds.Tables[0].Rows[0]["FNAME"].ToString();
                olstNames.LastName = ds.Tables[0].Rows[0]["LNAME"].ToString();
                olstNames.MidName = ds.Tables[0].Rows[0]["MNAME"].ToString();
            }
            return olstNames;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static string checkAppNO(string AGY, string DEPT, string PROGRAM, string YEAR, string APPNO, string MODE, out string STRRESULT)
        {
            string OutResp = "";
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[INSUPDEL_SMACH_CASEACTS]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            dbCmd.CommandTimeout = 1800;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            if (APPNO != string.Empty)
            {
                SqlParameter sqlp1 = new SqlParameter("@APPNO", APPNO);
                dbCmd.Parameters.Add(sqlp1);
            }
            if (AGY != string.Empty)
            {
                SqlParameter sqlp2 = new SqlParameter("@AGY", AGY);
                dbCmd.Parameters.Add(sqlp2);
            }
            if (DEPT != string.Empty)
            {
                SqlParameter sqlp3 = new SqlParameter("@DEPT ", DEPT);
                dbCmd.Parameters.Add(sqlp3);
            }
            if (PROGRAM != string.Empty)
            {
                SqlParameter sqlp4 = new SqlParameter("@PROGRAM ", PROGRAM);
                dbCmd.Parameters.Add(sqlp4);
            }
            if (YEAR != string.Empty)
            {
                SqlParameter sqlp5 = new SqlParameter("@YEAR ", YEAR);
                dbCmd.Parameters.Add(sqlp5);
            }


            if (MODE != string.Empty)
            {
                SqlParameter sqlp10 = new SqlParameter("@MODE ", MODE);
                dbCmd.Parameters.Add(sqlp10);
            }

            SqlParameter sqlOutSeq = new SqlParameter("@STRRESULT", SqlDbType.VarChar, 50);
            sqlOutSeq.Direction = ParameterDirection.Output;
            dbCmd.Parameters.Add(sqlOutSeq);
            db.ExecuteDataSet(dbCmd);

            STRRESULT = sqlOutSeq.Value.ToString();
            OutResp = STRRESULT;
            return OutResp; 
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataTable getAllFiles(string MODE)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[SMACHMID_INSDELGET]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            dbCmd.CommandTimeout = 1800;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (MODE != string.Empty)
            {
                SqlParameter sqlp10 = new SqlParameter("@MODE ", MODE);
                dbCmd.Parameters.Add(sqlp10);
            }

            ds = db.ExecuteDataSet(dbCmd);
            return ds.Tables[0];
        }
        #endregion

        #region interfaceobjects for INSUPDEL Methods

        public static bool iSMACHASSOC_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.SMACHASSOC_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }
        public static bool iSMACHMID_INSDELGET(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.SMACHMID_INSDELGET");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool iINSUPDEL_SMACH_CASEACTS(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSUPDEL_SMACH_CASEACTS");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        #endregion

    }
}

public class lstSNPNames
{
    public string FullName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MidName { get; set; }
}