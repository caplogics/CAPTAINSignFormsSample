/***********************************************************
 * Author : Kranthi
 * Date   : 02/14/2024
 * ********************************************************/

#region using
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace Captain.DatabaseLayer
{
    [DataObject]
    [Serializable]

    public class AlienTXDB
    {
        #region Constants
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;
        #endregion

        #region Common Functions
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
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
        public static DataTable GET_ALIENVER(string ALN_VER_ID,
           string ALN_VER_AGY,
           string ALN_VER_DEPT,
           string ALN_VER_PROG,
           string ALN_VER_YEAR,
           string ALN_VER_APPNO,
            string MODE)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[GET_ALIENVER]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            dbCmd.CommandTimeout = 1800;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            if (ALN_VER_ID != string.Empty)
            {
                SqlParameter sqlp1 = new SqlParameter("@ALN_VER_ID", ALN_VER_ID);
                dbCmd.Parameters.Add(sqlp1);
            }
            if (ALN_VER_AGY != string.Empty)
            {
                SqlParameter sqlp2 = new SqlParameter("@ALN_VER_AGY", ALN_VER_AGY);
                dbCmd.Parameters.Add(sqlp2);
            }
            if (ALN_VER_DEPT != string.Empty)
            {
                SqlParameter sqlp3 = new SqlParameter("@ALN_VER_DEPT ", ALN_VER_DEPT);
                dbCmd.Parameters.Add(sqlp3);
            }
            if (ALN_VER_PROG != string.Empty)
            {
                SqlParameter sqlp4 = new SqlParameter("@ALN_VER_PROG ", ALN_VER_PROG);
                dbCmd.Parameters.Add(sqlp4);
            }
            if (ALN_VER_YEAR != string.Empty)
            {
                SqlParameter sqlp5 = new SqlParameter("@ALN_VER_YEAR ", ALN_VER_YEAR);
                dbCmd.Parameters.Add(sqlp5);
            }

            if (ALN_VER_APPNO != string.Empty)
            {
                SqlParameter sqlp6 = new SqlParameter("@ALN_VER_APPNO ", ALN_VER_APPNO);
                dbCmd.Parameters.Add(sqlp6);
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
        public static DataTable GET_DOCSIGNHIS(string DCSN_HIS_AGY, string DCSN_HIS_DEPT, string DCSN_HIS_PROG, string DCSN_HIS_YEAR, string DCSN_HIS_APPNO,
            string FROMDATE, string TODATE,string isHISTMODE, string MODE)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[GET_DOCSIGNHIS]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            dbCmd.CommandTimeout = 1800;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            if (DCSN_HIS_AGY != string.Empty)
            {
                SqlParameter sqlp1 = new SqlParameter("@DCSN_HIS_AGY", DCSN_HIS_AGY);
                dbCmd.Parameters.Add(sqlp1);
            }
            if (DCSN_HIS_DEPT != string.Empty)
            {
                SqlParameter sqlp2 = new SqlParameter("@DCSN_HIS_DEPT", DCSN_HIS_DEPT);
                dbCmd.Parameters.Add(sqlp2);
            }
            if (DCSN_HIS_PROG != string.Empty)
            {
                SqlParameter sqlp3 = new SqlParameter("@DCSN_HIS_PROG ", DCSN_HIS_PROG);
                dbCmd.Parameters.Add(sqlp3);
            }
            if (DCSN_HIS_YEAR != string.Empty)
            {
                SqlParameter sqlp4 = new SqlParameter("@DCSN_HIS_YEAR ", DCSN_HIS_YEAR);
                dbCmd.Parameters.Add(sqlp4);
            }
            if (DCSN_HIS_APPNO != string.Empty)
            {
                SqlParameter sqlp5 = new SqlParameter("@DCSN_HIS_APPNO ", DCSN_HIS_APPNO);
                dbCmd.Parameters.Add(sqlp5);
            }
            if (FROMDATE != string.Empty)
            {
                SqlParameter sqlp6 = new SqlParameter("@FROMDATE ", FROMDATE);
                dbCmd.Parameters.Add(sqlp6);
            }
            if (TODATE != string.Empty)
            {
                SqlParameter sqlp7 = new SqlParameter("@TODATE", TODATE);
                dbCmd.Parameters.Add(sqlp7);
            }
            if (isHISTMODE != string.Empty)
            {
                SqlParameter sqlp7 = new SqlParameter("@isHISTMODE", isHISTMODE);
                dbCmd.Parameters.Add(sqlp7);
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
        public static bool CHECKSIGNDOCS(string DCSN_HIS_AGY, string DCSN_HIS_DEPT, string DCSN_HIS_PROG, string DCSN_HIS_YEAR, string DCSN_HIS_APPNO, string MODE)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;
            bool _resflag = false;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[GET_DOCSIGNHIS]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            dbCmd.CommandTimeout = 1800;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            if (DCSN_HIS_AGY != string.Empty)
            {
                SqlParameter sqlp1 = new SqlParameter("@DCSN_HIS_AGY", DCSN_HIS_AGY);
                dbCmd.Parameters.Add(sqlp1);
            }
            if (DCSN_HIS_DEPT != string.Empty)
            {
                SqlParameter sqlp2 = new SqlParameter("@DCSN_HIS_DEPT", DCSN_HIS_DEPT);
                dbCmd.Parameters.Add(sqlp2);
            }
            if (DCSN_HIS_PROG != string.Empty)
            {
                SqlParameter sqlp3 = new SqlParameter("@DCSN_HIS_PROG ", DCSN_HIS_PROG);
                dbCmd.Parameters.Add(sqlp3);
            }
            if (DCSN_HIS_YEAR != string.Empty)
            {
                SqlParameter sqlp4 = new SqlParameter("@DCSN_HIS_YEAR ", DCSN_HIS_YEAR);
                dbCmd.Parameters.Add(sqlp4);
            }
            if (DCSN_HIS_APPNO != string.Empty)
            {
                SqlParameter sqlp5 = new SqlParameter("@DCSN_HIS_APPNO ", DCSN_HIS_APPNO);
                dbCmd.Parameters.Add(sqlp5);
            }
            if (MODE != string.Empty)
            {
                SqlParameter sqlp10 = new SqlParameter("@MODE ", MODE);
                dbCmd.Parameters.Add(sqlp10);
            }

            ds = db.ExecuteDataSet(dbCmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()) > 0)
                    _resflag = true;
            }

            return _resflag;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool CheckIsDocSigned(string DCSN_HIS_AGY, string DCSN_HIS_DEPT, string DCSN_HIS_PROG, string DCSN_HIS_YEAR, string DCSN_HIS_APPNO, string DCSN_HIS_DOC_CODE, string MODE)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;
            bool _resflag = false;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[GET_DOCSIGNHIS]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            dbCmd.CommandTimeout = 1800;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            if (DCSN_HIS_AGY != string.Empty)
            {
                SqlParameter sqlp1 = new SqlParameter("@DCSN_HIS_AGY", DCSN_HIS_AGY);
                dbCmd.Parameters.Add(sqlp1);
            }
            if (DCSN_HIS_DEPT != string.Empty)
            {
                SqlParameter sqlp2 = new SqlParameter("@DCSN_HIS_DEPT", DCSN_HIS_DEPT);
                dbCmd.Parameters.Add(sqlp2);
            }
            if (DCSN_HIS_PROG != string.Empty)
            {
                SqlParameter sqlp3 = new SqlParameter("@DCSN_HIS_PROG ", DCSN_HIS_PROG);
                dbCmd.Parameters.Add(sqlp3);
            }
            if (DCSN_HIS_YEAR != string.Empty)
            {
                SqlParameter sqlp4 = new SqlParameter("@DCSN_HIS_YEAR ", DCSN_HIS_YEAR);
                dbCmd.Parameters.Add(sqlp4);
            }
            if (DCSN_HIS_APPNO != string.Empty)
            {
                SqlParameter sqlp5 = new SqlParameter("@DCSN_HIS_APPNO ", DCSN_HIS_APPNO);
                dbCmd.Parameters.Add(sqlp5);
            }
            if (DCSN_HIS_DOC_CODE != string.Empty)
            {
                SqlParameter sqlp5 = new SqlParameter("@DCSN_HIS_DOC_CODE ", DCSN_HIS_DOC_CODE);
                dbCmd.Parameters.Add(sqlp5);
            }
            if (MODE != string.Empty)
            {
                SqlParameter sqlp10 = new SqlParameter("@MODE ", MODE);
                dbCmd.Parameters.Add(sqlp10);
            }

            ds = db.ExecuteDataSet(dbCmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()) == 0)
                    _resflag = true;
            }

            return _resflag;
        }
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static int getMaxDocNo(string DCSN_HIS_AGY, string DCSN_HIS_DEPT, string DCSN_HIS_PROG, string DCSN_HIS_YEAR, string DCSN_HIS_APPNO, string DCSN_HIS_DOC_CODE, string MODE)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;
            int _resflag = 0;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[GET_DOCSIGNHIS]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            dbCmd.CommandTimeout = 1800;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            if (DCSN_HIS_AGY != string.Empty)
            {
                SqlParameter sqlp1 = new SqlParameter("@DCSN_HIS_AGY", DCSN_HIS_AGY);
                dbCmd.Parameters.Add(sqlp1);
            }
            if (DCSN_HIS_DEPT != string.Empty)
            {
                SqlParameter sqlp2 = new SqlParameter("@DCSN_HIS_DEPT", DCSN_HIS_DEPT);
                dbCmd.Parameters.Add(sqlp2);
            }
            if (DCSN_HIS_PROG != string.Empty)
            {
                SqlParameter sqlp3 = new SqlParameter("@DCSN_HIS_PROG ", DCSN_HIS_PROG);
                dbCmd.Parameters.Add(sqlp3);
            }
            if (DCSN_HIS_YEAR != string.Empty)
            {
                SqlParameter sqlp4 = new SqlParameter("@DCSN_HIS_YEAR ", DCSN_HIS_YEAR);
                dbCmd.Parameters.Add(sqlp4);
            }
            if (DCSN_HIS_APPNO != string.Empty)
            {
                SqlParameter sqlp5 = new SqlParameter("@DCSN_HIS_APPNO ", DCSN_HIS_APPNO);
                dbCmd.Parameters.Add(sqlp5);
            }
            if (DCSN_HIS_DOC_CODE != string.Empty)
            {
                SqlParameter sqlp5 = new SqlParameter("@DCSN_HIS_DOC_CODE ", DCSN_HIS_DOC_CODE);
                dbCmd.Parameters.Add(sqlp5);
            }
            if (MODE != string.Empty)
            {
                SqlParameter sqlp10 = new SqlParameter("@MODE ", MODE);
                dbCmd.Parameters.Add(sqlp10);
            }

            ds = db.ExecuteDataSet(dbCmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
               _resflag = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
            }

            return _resflag;
        }

        #endregion



        #region interfaceobjects for INSUPDEL Methods
        public static bool iINSUPDELALIENVER(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSUPDEL_ALIENVER");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool iINSUPDEL_DOCSIGNHIS(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSUPDEL_DOCSIGNHIS");
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
