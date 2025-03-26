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
    public partial class ADMNB001DB
    {

        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[AGYTAB]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        //private static DbCommand _dbCommand;
        #endregion

       
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ADMNB001_GetaAgyTabList(string AgyType)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Rep_ADMNB001_GetAgyTabList]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAGYType = new SqlParameter("@AGY_Type", AgyType);
            dbCmd.Parameters.Add(sqlAGYType);


            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ADMNB001_GetServices()
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[GetServices]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            //List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //SqlParameter sqlAGYType = new SqlParameter("@AGY_Type", AgyType);
            //dbCmd.Parameters.Add(sqlAGYType);


            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ADMNB001_GetCaseDemoAss()
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Rep_ADMNB001_Get_CaseDemoAss]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            //List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //SqlParameter sqlAGYType = new SqlParameter("@AGY_Type", AgyType);
            //dbCmd.Parameters.Add(sqlAGYType);


            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ADMNB001_GetCSBCAT()
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[ReP_ADMNB001_CSB4CATG]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            //List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //SqlParameter sqlAGYType = new SqlParameter("@AGY_Type", AgyType);
            //dbCmd.Parameters.Add(sqlAGYType);


            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ADMNB001_GetCSB4ASSOC()
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Rep_ADMNB001CSB4ASSOC]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            //List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //SqlParameter sqlAGYType = new SqlParameter("@AGY_Type", AgyType);
            //dbCmd.Parameters.Add(sqlAGYType);


            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ADMNB001_GetRNGCAT()
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[RNG_ADMNB001_RNG4CATG]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            //List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //SqlParameter sqlAGYType = new SqlParameter("@AGY_Type", AgyType);
            //dbCmd.Parameters.Add(sqlAGYType);


            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ADMNB001_GetRNG4ASSOC()
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[RNG_ADMNB001RNG4ASSOC]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            //List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //SqlParameter sqlAGYType = new SqlParameter("@AGY_Type", AgyType);
            //dbCmd.Parameters.Add(sqlAGYType);


            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ADMNB001_ZipcodeSearch(string SortCol)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Rep_ADMND001_ZipcodeSearch]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAGYType = new SqlParameter("@SortCol", SortCol);
            dbCmd.Parameters.Add(sqlAGYType);


            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }
       

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ADMNB001_Browse_AGCYCNTL(string Agency, string City, string Zip1, string Zip2, string State, string Phone, string Voucher)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_AGCYCNTL]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgency = new SqlParameter("@ACR_AGENCY_CODE", Agency);
            dbCmd.Parameters.Add(sqlAgency);

            SqlParameter sqlCity = new SqlParameter("@ACR_CITY", City);
            dbCmd.Parameters.Add(sqlCity);

            SqlParameter sqlZip1 = new SqlParameter("@ACR_ZIP1 ", Zip1);
            dbCmd.Parameters.Add(sqlZip1);

            SqlParameter sqlZip2 = new SqlParameter("@ACR_ZIP2", Zip2);
            dbCmd.Parameters.Add(sqlZip2);

            SqlParameter sqlState = new SqlParameter("@ACR_STATE ", State);
            dbCmd.Parameters.Add(sqlState);

            SqlParameter sqlPhone = new SqlParameter("@ACR_MAIN_PHONE", Phone);
            dbCmd.Parameters.Add(sqlPhone);

            SqlParameter sqlVoucher = new SqlParameter("@ACR_VOUCHER_NO", Voucher);
            dbCmd.Parameters.Add(sqlVoucher);
            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ADMNB001_GetMsmast(string SortCol, string Code)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_MSMAST]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlSort = new SqlParameter("@SortCol", SortCol);
            dbCmd.Parameters.Add(sqlSort);

            SqlParameter sqlCode = new SqlParameter("@MS_CODE", Code);
            dbCmd.Parameters.Add(sqlCode);
            //List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //SqlParameter sqlAGYType = new SqlParameter("@AGY_Type", AgyType);
            //dbCmd.Parameters.Add(sqlAGYType);


            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ADMNB001_GetCamast(string SortCol, string Code)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_CAMAST]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlSort = new SqlParameter("@SortCol", SortCol);
            dbCmd.Parameters.Add(sqlSort);


            //List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //SqlParameter sqlAGYType = new SqlParameter("@AGY_Type", AgyType);
            //dbCmd.Parameters.Add(sqlAGYType);
            SqlParameter sqlCode = new SqlParameter("@CA_CODE", Code);
            dbCmd.Parameters.Add(sqlCode);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ADMNB001_GetCashie(string strPublicCode,string UserID,string AdminAgency,string str)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_CASEHIE]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (strPublicCode.Substring(0, 2) != "**")
            {
                SqlParameter sqlAgency = new SqlParameter("@HIE_AGENCY", strPublicCode.Substring(0, 2));
                dbCmd.Parameters.Add(sqlAgency);
            }
            if (strPublicCode.Substring(3, 2) != "**")
            {
                SqlParameter sqlDept = new SqlParameter("@HIE_DEPT", strPublicCode.Substring(3, 2));
                dbCmd.Parameters.Add(sqlDept);
            }
            if (strPublicCode.Substring(6, 2) != "**")
            {
                SqlParameter sqlProg = new SqlParameter("@HIE_PROGRAM", strPublicCode.Substring(6, 2));
                dbCmd.Parameters.Add(sqlProg);
            }

            if (UserID != "")
            {
                SqlParameter sqlProg = new SqlParameter("@UserID", UserID);
                dbCmd.Parameters.Add(sqlProg);
            }

            if (AdminAgency != "")
            {
                SqlParameter sqlProg = new SqlParameter("@Admin_Agency", AdminAgency);
                dbCmd.Parameters.Add(sqlProg);
            }


            //List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //SqlParameter sqlAGYType = new SqlParameter("@AGY_Type", AgyType);
            //dbCmd.Parameters.Add(sqlAGYType);

            //List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //SqlParameter sqlAGYType = new SqlParameter("@AGY_Type", AgyType);
            //dbCmd.Parameters.Add(sqlAGYType);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ADMNB001_GetCashie(string strAgency,string strDept,string strProgram)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_CASEHIE]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (strAgency != "**")
            {
                SqlParameter sqlAgency = new SqlParameter("@HIE_AGENCY", strAgency);
                dbCmd.Parameters.Add(sqlAgency);
            }
            if (strDept != "**")
            {
                SqlParameter sqlDept = new SqlParameter("@HIE_DEPT", strDept);
                dbCmd.Parameters.Add(sqlDept);
            }
            if (strProgram != "**")
            {
                SqlParameter sqlProg = new SqlParameter("@HIE_PROGRAM", strProgram);
                dbCmd.Parameters.Add(sqlProg);
            }


            //List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //SqlParameter sqlAGYType = new SqlParameter("@AGY_Type", AgyType);
            //dbCmd.Parameters.Add(sqlAGYType);

            //List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //SqlParameter sqlAGYType = new SqlParameter("@AGY_Type", AgyType);
            //dbCmd.Parameters.Add(sqlAGYType);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ADMNB001_GetCasDep(string Agy, string Dept, string Prog)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Rep_ADMNB001_Get_CaseDepRep]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgency = new SqlParameter("@DEP_AGENCY", Agy);
            dbCmd.Parameters.Add(sqlAgency);
            SqlParameter sqlDept = new SqlParameter("@DEP_DEPT", Dept);
            dbCmd.Parameters.Add(sqlDept);
            SqlParameter sqlProg = new SqlParameter("@DEP_PROG", Prog);
            dbCmd.Parameters.Add(sqlProg);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ADMNB001_GetCasDepCont(string Agy, string Dept, string Prog)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[GetCASEDEPCONT]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgency = new SqlParameter("@DEPCONT_AGENCY", Agy);
            dbCmd.Parameters.Add(sqlAgency);
            SqlParameter sqlDept = new SqlParameter("@DEPCONT_DEPT", Dept);
            dbCmd.Parameters.Add(sqlDept);
            SqlParameter sqlProg = new SqlParameter("@DEPCONT_PROG", Prog);
            dbCmd.Parameters.Add(sqlProg);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GETLOGUSERS(string UserID, string Date,string ToDate, string Type)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[GETLOGUSERS]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (UserID != string.Empty)
            {
                SqlParameter sqlSort = new SqlParameter("@LOG_USERID", UserID);
                dbCmd.Parameters.Add(sqlSort);
            }

            //List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //SqlParameter sqlAGYType = new SqlParameter("@AGY_Type", AgyType);
            //dbCmd.Parameters.Add(sqlAGYType);
            if (Date != string.Empty)
            {
                SqlParameter sqlCode = new SqlParameter("@LOGDAY", Date);
                dbCmd.Parameters.Add(sqlCode);
            }
            if (ToDate != string.Empty)
            {
                SqlParameter sqlCode = new SqlParameter("@LOGTODAY", ToDate);
                dbCmd.Parameters.Add(sqlCode);
            }
            if (Type != string.Empty)
            {
                SqlParameter sqlType = new SqlParameter("@Type", Type);
                dbCmd.Parameters.Add(sqlType);
            }
            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetBinaryImages(string ID,string AppKey)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[GetBinaryImages]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(ID.Trim()))
            {
                SqlParameter sqlAGYType = new SqlParameter("@ID", ID);
                dbCmd.Parameters.Add(sqlAGYType);
            }
            if (!string.IsNullOrEmpty(AppKey.Trim()))
            {
                SqlParameter sqlAGYType = new SqlParameter("@FileName", AppKey);
                dbCmd.Parameters.Add(sqlAGYType);
            }

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        public static DataTable getMIDTableData(string strConnection)
        {

            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {

                    cmd.Connection = con;
                    cmd.CommandText = "select * from MID_TABLE where DRAG_TYPE='C' and IMG is null  order by N_AGENCY,N_DEPT, N_PROG,N_YEAR,N_APP";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.Text;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);

                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return dt;

        }
        public static DataTable getDelMIDTableData(string strConnection)
        {

            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {

                    cmd.Connection = con;
                    cmd.CommandText = "select * from MID_TABLE where DRAG_TYPE='C' and IMG='C' order by N_AGENCY,N_DEPT, N_PROG,N_YEAR,N_APP";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.Text;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);

                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return dt;

        }
        public static DataTable getMovedDssXMLZips(string strConnection)
        {

            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {

                    cmd.Connection = con;
                    cmd.CommandText = "select * from CTZIPS where CTZ_STATUS != 'Z' ";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.Text;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);

                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return dt;

        }
        public static int updMIDTblFldDets(string strConnection, string Agy, string Dept, string Prog, string Year, string Appno)
        {

            int inti = 0;
            try
            {

                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {

                    cmd.Connection = con;
                    cmd.CommandText = "UPDATE MID_TABLE SET IMG='C' WHERE O_AGENCY='" + Agy + "' AND O_DEPT='" + Dept + "' AND O_PROG='" + Prog + "' AND O_YEAR='" + Year + "' AND O_APP='" + Appno + "'";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.Text;

                    //SqlDataAdapter da = new SqlDataAdapter(cmd);

                    //da.Fill(dt);
                    inti = cmd.ExecuteNonQuery();

                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return inti;

        }

    }
}
