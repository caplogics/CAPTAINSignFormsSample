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
    public partial class ADMNB002DB
    {

        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[EMPLFUNC]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        //private static DbCommand _dbCommand;
        #endregion

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Rep_ADMNB002_GetScrPrivbyUserId(string UserName, string ModuleCode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Rep_ADMNB002_GetScrPrivbyUserId]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlUserName = new SqlParameter("@UserName", UserName);
            dbCommand.Parameters.Add(sqlUserName);
            SqlParameter sqlModuleCode = new SqlParameter("@ModuleCode", ModuleCode);
            dbCommand.Parameters.Add(sqlModuleCode);
            SqlParameter sqlScrCode = new SqlParameter("@ScrCode", null);
            dbCommand.Parameters.Add(sqlScrCode);
            SqlParameter sqlHierarchy = new SqlParameter("@Hierarchy", null);
            dbCommand.Parameters.Add(sqlHierarchy);
            SqlParameter sqlAddPriv = new SqlParameter("@AddPriv", null);
            dbCommand.Parameters.Add(sqlAddPriv);
            SqlParameter sqlChgPriv = new SqlParameter("@ChgPriv", null);
            dbCommand.Parameters.Add(sqlChgPriv);
            SqlParameter sqlDelPriv = new SqlParameter("@DelPriv", null);
            dbCommand.Parameters.Add(sqlDelPriv);
            SqlParameter sqlInqPriv = new SqlParameter("@InqPriv", null);
            dbCommand.Parameters.Add(sqlInqPriv);

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetUserNames()
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Rep_ADMNB002_GetPassword]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetUserNameswithAgency(string Agency, string UserName)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[GET_PASSWORD_AGENCY]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            
            SqlParameter sqlModuleCode = new SqlParameter("@AGENCY", Agency);
            dbCmd.Parameters.Add(sqlModuleCode);
            if (!string.IsNullOrEmpty(UserName.Trim()))
            {
                SqlParameter sqlUserName = new SqlParameter("@UserName", UserName);
                dbCmd.Parameters.Add(sqlUserName);
            }

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetUserIDs()
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[GET_ALLUSER_IDS]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ADMNB002_GetModuleHierarchies(string UserName, string ModuleCode)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Rep_ADMNB002_GetModuleHierarchies]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlUserName = new SqlParameter("@UserName", UserName);
            dbCmd.Parameters.Add(sqlUserName);
            SqlParameter sqlModuleCode = new SqlParameter("@ModuleCode", ModuleCode);
            dbCmd.Parameters.Add(sqlModuleCode);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ADMNB002_GetRepPrivbyUserId(string UserName, string ModuleCode)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Rep_ADMNB002_GetRepPrivbyUserId]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlUserName = new SqlParameter("@UserName", UserName);
            dbCmd.Parameters.Add(sqlUserName);
            SqlParameter sqlModuleCode = new SqlParameter("@ModuleCode", ModuleCode);
            dbCmd.Parameters.Add(sqlModuleCode);
            SqlParameter sqlScrCode = new SqlParameter("@ScrCode", null);
            dbCmd.Parameters.Add(sqlScrCode);
            SqlParameter sqlViewPriv = new SqlParameter("@ViewPriv", null);
            dbCmd.Parameters.Add(sqlViewPriv);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GET_INCOME_REPORT(string Hierarchy, string App, string FamSeq)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[CASINCOM_GetIncomeDetailsByApp]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            dbCmd.CommandTimeout = 1800;

            //dbCmd.Parameters.Add(sqlScrCode);
            SqlParameter sqlHierarchy = new SqlParameter("@Hierarchy", Hierarchy);
            dbCmd.Parameters.Add(sqlHierarchy);
            SqlParameter sqlApp = new SqlParameter("@AppNO", App);
            dbCmd.Parameters.Add(sqlApp);

            SqlParameter sqlFamSeq = new SqlParameter("@MST_FAMILY_SEQ", FamSeq);
            dbCmd.Parameters.Add(sqlFamSeq);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_CAPFNCC(string ModuleCode)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_CAPFNCC]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //SqlParameter sqlUserName = new SqlParameter("@UserName", UserName);
            //dbCmd.Parameters.Add(sqlUserName);
            if (ModuleCode != null)
            {
                if (!string.IsNullOrEmpty(ModuleCode.Trim()))
                {
                    SqlParameter sqlModuleCode = new SqlParameter("@CFC_MODULE_CODE", ModuleCode);
                    dbCmd.Parameters.Add(sqlModuleCode);
                }
            }

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_CAPBATC(string ModuleCode)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_CAPBATC]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //SqlParameter sqlUserName = new SqlParameter("@UserName", UserName);
            //dbCmd.Parameters.Add(sqlUserName);
            if (ModuleCode != null)
            {
                if (!string.IsNullOrEmpty(ModuleCode.Trim()))
                {
                    SqlParameter sqlModuleCode = new SqlParameter("@CBC_MODULE_CODE", ModuleCode);
                    dbCmd.Parameters.Add(sqlModuleCode);
                }
            }

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

    }
}
