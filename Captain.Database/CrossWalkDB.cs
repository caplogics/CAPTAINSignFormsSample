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

    public class CrossWalkDB
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
        public static DataSet CRSWALK_GET(string CRSWK_ID, string CRSWK_CATEGORY_ID, string CRSWK_FILE_CODE, string CRSWK_WORK_TYPE, string CRSWK_USER,
            string strAgy, string strDept, string strProg, string strYear, string MODE)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[CRSWALK_GET]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            dbCmd.CommandTimeout = 1800;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            if (CRSWK_ID != string.Empty)
            {
                SqlParameter sqlp1 = new SqlParameter("@CRSWK_ID", CRSWK_ID);
                dbCmd.Parameters.Add(sqlp1);
            }
            if (CRSWK_CATEGORY_ID != string.Empty)
            {
                SqlParameter sqlp2 = new SqlParameter("@CRSWK_CATEGORY_ID", CRSWK_CATEGORY_ID);
                dbCmd.Parameters.Add(sqlp2);
            }
            if (CRSWK_FILE_CODE != string.Empty)
            {
                SqlParameter sqlp3 = new SqlParameter("@CRSWK_FILE_CODE ", CRSWK_FILE_CODE);
                dbCmd.Parameters.Add(sqlp3);
            }
            if (CRSWK_WORK_TYPE != string.Empty)
            {
                SqlParameter sqlp4 = new SqlParameter("@CRSWK_WORK_TYPE ", CRSWK_WORK_TYPE);
                dbCmd.Parameters.Add(sqlp4);
            }
            if (CRSWK_USER != string.Empty)
            {
                SqlParameter sqlp5 = new SqlParameter("@CRSWK_USER ", CRSWK_USER);
                dbCmd.Parameters.Add(sqlp5);
            }

            if (strAgy != string.Empty)
            {
                SqlParameter sqlp6 = new SqlParameter("@CRSWK_AGY ", strAgy);
                dbCmd.Parameters.Add(sqlp6);
            }
            if (strDept != string.Empty)
            {
                SqlParameter sqlp7 = new SqlParameter("@CRSWK_DEPT ", strDept);
                dbCmd.Parameters.Add(sqlp7);
            }
            if (strProg != string.Empty)
            {
                SqlParameter sqlp8 = new SqlParameter("@CRSWK_PROG ", strProg);
                dbCmd.Parameters.Add(sqlp8);
            }
            if (strYear.Trim() != string.Empty)
            {
                SqlParameter sqlp9 = new SqlParameter("@CRSWK_YEAR ", strYear);
                dbCmd.Parameters.Add(sqlp9);
            }

            if (MODE != string.Empty)
            {
                SqlParameter sqlp10 = new SqlParameter("@MODE ", MODE);
                dbCmd.Parameters.Add(sqlp10);
            }

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CRSWK_MIDKEY_GET(string MKEY_AGY, string MKEY_DEPT, string MKEY_PROG, string MKEY_YEAR,
        string MKEY_CRSWK_CODE, string MKEY_CRSWK_CAT_ID, string MKEY_XL_FAM_ID, string MKEY_XL_CHILDPLUS_ID, string MKEY_XL_CHILDPLUS_ID1,
        string MKEY_XL_FNAME, string MKEY_XL_LNAME, string MKEY_XL_DOB, string MKEY_STATUS, string prvworkfilecode, string MODE)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[CRSWK_MIDKEY_GET]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            dbCmd.CommandTimeout = 1800;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            if (MKEY_AGY != string.Empty)
            {
                SqlParameter sqlp1 = new SqlParameter("@MKEY_AGY", MKEY_AGY);
                dbCmd.Parameters.Add(sqlp1);
            }
            if (MKEY_DEPT != string.Empty)
            {
                SqlParameter sqlp2 = new SqlParameter("@MKEY_DEPT", MKEY_DEPT);
                dbCmd.Parameters.Add(sqlp2);
            }
            if (MKEY_PROG != string.Empty)
            {
                SqlParameter sqlp3 = new SqlParameter("@MKEY_PROG ", MKEY_PROG);
                dbCmd.Parameters.Add(sqlp3);
            }
            if (MKEY_YEAR != string.Empty)
            {
                SqlParameter sqlp4 = new SqlParameter("@MKEY_YEAR ", MKEY_YEAR);
                dbCmd.Parameters.Add(sqlp4);
            }
            if (MKEY_CRSWK_CODE != string.Empty)
            {
                SqlParameter sqlp5 = new SqlParameter("@MKEY_CRSWK_CODE ", MKEY_CRSWK_CODE);
                dbCmd.Parameters.Add(sqlp5);
            }
            if (prvworkfilecode != string.Empty)
            {
                SqlParameter sqlp5 = new SqlParameter("@PRVCODE", prvworkfilecode);
                dbCmd.Parameters.Add(sqlp5);
            }
            if (MKEY_CRSWK_CAT_ID != string.Empty)
            {
                SqlParameter sqlp6 = new SqlParameter("@MKEY_CRSWK_CAT_ID ", MKEY_CRSWK_CAT_ID);
                dbCmd.Parameters.Add(sqlp6);
            }
            if (MKEY_XL_FAM_ID != string.Empty)
            {
                SqlParameter sqlp7 = new SqlParameter("@MKEY_XL_FAM_ID ", MKEY_XL_FAM_ID);
                dbCmd.Parameters.Add(sqlp7);
            }
            if (MKEY_XL_CHILDPLUS_ID != string.Empty)
            {
                SqlParameter sqlp8 = new SqlParameter("@MKEY_XL_CHILDPLUS_ID ", MKEY_XL_CHILDPLUS_ID);
                dbCmd.Parameters.Add(sqlp8);
            }
            if (MKEY_XL_CHILDPLUS_ID1.Trim() != string.Empty)
            {
                SqlParameter sqlp9 = new SqlParameter("@MKEY_XL_CHILDPLUS_ID1 ", MKEY_XL_CHILDPLUS_ID1);
                dbCmd.Parameters.Add(sqlp9);
            }
            if (MKEY_XL_FNAME.Trim() != string.Empty)
            {
                SqlParameter sqlp10 = new SqlParameter("@MKEY_XL_FNAME ", MKEY_XL_FNAME);
                dbCmd.Parameters.Add(sqlp10);
            }
            if (MKEY_XL_LNAME.Trim() != string.Empty)
            {
                SqlParameter sqlp10 = new SqlParameter("@MKEY_XL_LNAME ", MKEY_XL_LNAME);
                dbCmd.Parameters.Add(sqlp10);
            }
            if (MKEY_XL_DOB.Trim() != string.Empty)
            {
                SqlParameter sqlp10 = new SqlParameter("@MKEY_XL_DOB ", MKEY_XL_DOB);
                dbCmd.Parameters.Add(sqlp10);
            }
            if (MKEY_STATUS.Trim() != string.Empty)
            {
                SqlParameter sqlp10 = new SqlParameter("@MKEY_STATUS ", MKEY_STATUS);
                dbCmd.Parameters.Add(sqlp10);
            }

            if (MODE != string.Empty)
            {
                SqlParameter sqlp10 = new SqlParameter("@MODE ", MODE);
                dbCmd.Parameters.Add(sqlp10);
            }

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CRSWKSETTINGS_GET(string CRSWK_SET_ID, string CRSWK_CAT_ID, string CRSWK_SET_FUN_FOR, string MODE)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[CRSWKSETTINGS_GET]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            dbCmd.CommandTimeout = 1800;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (CRSWK_SET_ID != string.Empty)
            {
                SqlParameter sqlp1 = new SqlParameter("@CRSWK_SET_ID", CRSWK_SET_ID);
                dbCmd.Parameters.Add(sqlp1);
            }
            if (CRSWK_CAT_ID != string.Empty)
            {
                SqlParameter sqlp2 = new SqlParameter("@CRSWK_CAT_ID", CRSWK_CAT_ID);
                dbCmd.Parameters.Add(sqlp2);
            }
            if (CRSWK_SET_FUN_FOR != string.Empty)
            {
                SqlParameter sqlp3 = new SqlParameter("@CRSWK_SET_FUN_FOR", CRSWK_SET_FUN_FOR);
                dbCmd.Parameters.Add(sqlp3);
            }
            if (MODE != string.Empty)
            {
                SqlParameter sqlp5 = new SqlParameter("@MODE ", MODE);
                dbCmd.Parameters.Add(sqlp5);
            }
            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static string getInhouseNo(string AGY, string DEPT, string PROG, string YEAR, string SNP_APP, string MODE)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[CRSWK_DEL_CAPRECS]";
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
                SqlParameter sqlp2 = new SqlParameter("@DEPT", DEPT);
                dbCmd.Parameters.Add(sqlp2);
            }
            if (PROG != string.Empty)
            {
                SqlParameter sqlp3 = new SqlParameter("@PROG", PROG);
                dbCmd.Parameters.Add(sqlp3);
            }
            if (YEAR != string.Empty)
            {
                SqlParameter sqlp3 = new SqlParameter("@YEAR", YEAR);
                dbCmd.Parameters.Add(sqlp3);
            }
            if (SNP_APP != string.Empty)
            {
                SqlParameter sqlp3 = new SqlParameter("@SNP_APP", SNP_APP);
                dbCmd.Parameters.Add(sqlp3);
            }
            if (MODE != string.Empty)
            {
                SqlParameter sqlp5 = new SqlParameter("@MODE ", MODE);
                dbCmd.Parameters.Add(sqlp5);
            }
            ds = db.ExecuteDataSet(dbCmd);
            string inhouseNo = "0";
            if (ds.Tables.Count > 0) {
                if (ds.Tables[0].Rows.Count > 0) {
                    inhouseNo = ds.Tables[0].Rows[0][0].ToString();
                }
            }

            return inhouseNo;
        }
        #endregion


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static string chkHeadStartProgam( string AGY, string DEPT, string PROG, string QUESDESC)
        {
            string Resp = "";
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[CRSWK_CHKCUSTQUES]";
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
                SqlParameter sqlp2 = new SqlParameter("@DEPT", DEPT);
                dbCmd.Parameters.Add(sqlp2);
            }
            if (PROG != string.Empty)
            {
                SqlParameter sqlp3 = new SqlParameter("@PROG", PROG);
                dbCmd.Parameters.Add(sqlp3);
            }
            if (QUESDESC != string.Empty)
            {
                SqlParameter sqlp5 = new SqlParameter("@QUESDESC ", QUESDESC);
                dbCmd.Parameters.Add(sqlp5);
            }
            ds = db.ExecuteDataSet(dbCmd);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                Resp = dt.Rows[0][0].ToString();
                //if (Resp.Contains("CODE_")) {

                //    Resp = Resp.Split('_')[1].ToString();

                //}
            }
            return Resp;
        }
        //public string chkHeadStartProgam(string strConnection, string AGY, string DEPT, string PROG, string QUESDESC)
        //{
        //    string Resp = "";
        //    DataTable dt = new DataTable();
        //    try
        //    {

        //        SqlConnection con = new SqlConnection(strConnection);

        //        con.Open();
        //        using (SqlCommand cmd = new SqlCommand())
        //        {

        //            cmd.Connection = con;
        //            cmd.Parameters.AddWithValue("@CUSTCODE", "");
        //            cmd.CommandText = "IF((select COUNT(*) from CUSTFLDS where CUST_DESC like '%'" + QUESDESC + "'%')>0) BEGIN " +
        //                        //"DECLARE @CUSTCODE VARCHAR(10) = NULL;" +
        //                        " SET @CUSTCODE = (SELECT CUST_CODE from CUSTFLDS where CUST_DESC like '%'" + QUESDESC + "'%')" +
        //                        " IF((SELECT COUNT(*) from FLDCNTLHIE where FLDH_CODE = @CUSTCODE AND(FLDH_SCR_HIE = ('" + AGY + DEPT + PROG + "'))) > 0) BEGIN " +
        //                         " SELECT 'CODE_' + @CUSTCODE" +
        //                        " END " +
        //                        " ELSE BEGIN " +
        //                            " IF((SELECT COUNT(*) from FLDCNTLHIE where FLDH_CODE = @CUSTCODE AND(FLDH_SCR_HIE = ('" + AGY + DEPT + "**'))) > 0) BEGIN " +
        //                                "SELECT 'CODE_' + @CUSTCODE" +
        //                            " END " +
        //                            " ELSE BEGIN " +
        //                                " IF((SELECT COUNT(*) from FLDCNTLHIE where FLDH_CODE = @CUSTCODE AND(FLDH_SCR_HIE = ('" + AGY + "****'))) > 0) BEGIN " +
        //                                    "SELECT 'CODE_' + @CUSTCODE" +
        //                                " END " +
        //                                " ELSE " +
        //                                " SELECT 'Please Assign Head Start Program to " + AGY + DEPT + PROG + "'" +
        //                            " END " +
        //                        " END " +
        //                    " END " +
        //                    " ELSE " +
        //                       " SELECT 'Please Create Head Start Program' ";
        //            cmd.CommandTimeout = 200;
        //            cmd.CommandType = CommandType.Text;

        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            da.Fill(dt);
        //            if (dt.Rows.Count > 0)
        //            {
        //                Resp = dt.Rows[0][0].ToString();
        //                //if (Resp.Contains("CODE_")) {

        //                //    Resp = Resp.Split('_')[1].ToString();

        //                //}
        //            }

        //        }
        //        con.Close();

        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //    return Resp;

        //}
        #region interfaceobjects for INSUPDEL Methods
        public static bool iINSUPDELCRSWALK(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSUPDELCRSWALK");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }
        public static bool iINSUPDELCRSWKSETTINGS(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSUPDELCRSWKSETTINGS");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool iINSUPDEL_CRSWK_MIDKEY(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSUPDEL_CRSWK_MIDKEY");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool iINSUPDEL_CRSWK_CAPTAIN(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSUPDEL_CRSWK_CAPTAIN");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }
        public static bool iCRSWK_DEL_CAPRECS(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CRSWK_DEL_CAPRECS");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }
        public static bool iCRSWK_INSUPDELCASEVER(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CRSWK_INSUPDELCASEVER");
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
