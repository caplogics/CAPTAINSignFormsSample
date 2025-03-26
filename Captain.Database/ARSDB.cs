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
    public partial class ARSDB
    {

        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[AGYTAB]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;
        #endregion

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_ARSCUST(string Agency, string Dept, string Prog, string Appno, string Type)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_ARSCUST]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            dbCmd.CommandTimeout = 1800;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgency = new SqlParameter("@ARS_AGENCY", Agency);
            dbCmd.Parameters.Add(sqlAgency);

            SqlParameter sqlCity = new SqlParameter("@ARS_DEPT", Dept);
            dbCmd.Parameters.Add(sqlCity);

            SqlParameter sqlZip1 = new SqlParameter("@ARS_PROGRAM ", Prog);
            dbCmd.Parameters.Add(sqlZip1);

            if (!string.IsNullOrEmpty(Appno))
            {
                SqlParameter sqlZip2 = new SqlParameter("@ARS_APP_NO", Appno);
                dbCmd.Parameters.Add(sqlZip2);
            }
            if (!string.IsNullOrEmpty(Type))
            {
                SqlParameter sqlState = new SqlParameter("@ARS_PRINT_INV ", Type);
                dbCmd.Parameters.Add(sqlState);
            }

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_ARSCUST(string Agency, string Dept, string Prog, string Appno, string Type)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Get_ARSCUST]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgency = new SqlParameter("@ARS_AGENCY", Agency);
            dbCmd.Parameters.Add(sqlAgency);

            SqlParameter sqlCity = new SqlParameter("@ARS_DEPT", Dept);
            dbCmd.Parameters.Add(sqlCity);

            SqlParameter sqlZip1 = new SqlParameter("@ARS_PROGRAM ", Prog);
            dbCmd.Parameters.Add(sqlZip1);

            if (!string.IsNullOrEmpty(Appno))
            {
                SqlParameter sqlZip2 = new SqlParameter("@ARS_APP_NO", Appno);
                dbCmd.Parameters.Add(sqlZip2);
            }
            if (!string.IsNullOrEmpty(Type))
            {
                SqlParameter sqlState = new SqlParameter("@ARS_CUST_TYPE ", Type);
                dbCmd.Parameters.Add(sqlState);
            }

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_ARSMAST(string Agency, string Dept, string Prog, string Appno, string Type,string Status,string CDate)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_ARSMAST]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgency = new SqlParameter("@ARM_AGENCY", Agency);
            dbCmd.Parameters.Add(sqlAgency);

            SqlParameter sqlCity = new SqlParameter("@ARM_DEPT", Dept);
            dbCmd.Parameters.Add(sqlCity);

            SqlParameter sqlZip1 = new SqlParameter("@ARM_PROGRAM ", Prog);
            dbCmd.Parameters.Add(sqlZip1);

            if (!string.IsNullOrEmpty(Appno))
            {
                SqlParameter sqlZip2 = new SqlParameter("@ARM_APP_NO", Appno);
                dbCmd.Parameters.Add(sqlZip2);
            }

            if (!string.IsNullOrEmpty(Type))
            {
                SqlParameter sqlState = new SqlParameter("@ARM__CUST_TYPE ", Type);
                dbCmd.Parameters.Add(sqlState);
            }
            if (!string.IsNullOrEmpty(Status))
            {
                SqlParameter sqlStatus = new SqlParameter("@ARM_STATUS ", Status);
                dbCmd.Parameters.Add(sqlStatus);
            }
            if (!string.IsNullOrEmpty(CDate))
            {
                SqlParameter sqlCDate = new SqlParameter("@ARM_CLOSED_DATE ", CDate);
                dbCmd.Parameters.Add(sqlCDate);
            }
            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_ARSNUMB(string arscode, string arsno)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_ARSNUMB]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            
            if (!string.IsNullOrEmpty(arscode))
            {
                SqlParameter sqlZip2 = new SqlParameter("@arsnumb_code", arscode);
                dbCmd.Parameters.Add(sqlZip2);
            }

            if (!string.IsNullOrEmpty(arsno))
            {
                SqlParameter sqlState = new SqlParameter("@arsnumb_no", arsno);
                dbCmd.Parameters.Add(sqlState);
            }
            
            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_ARSMAST(string Agency, string Dept, string Prog, string Appno, string Type, string Source, string Invoice)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[GET_ARSMAST]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            if (Agency != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@ARM_AGENCY", Agency);
                dbCmd.Parameters.Add(sqlAgency);
            }
            if (Dept != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@ARM_DEPT", Dept);
                dbCmd.Parameters.Add(sqlDept);
            }
            if (Prog != string.Empty)
            {
                SqlParameter sqlProgram = new SqlParameter("@ARM_PROGRAM", Prog);
                dbCmd.Parameters.Add(sqlProgram);
            }
            if (Appno != string.Empty)
            {
                SqlParameter sqlAppno = new SqlParameter("@ARM_APP_NO", Appno);
                dbCmd.Parameters.Add(sqlAppno);
            }
            if (Type != string.Empty)
            {
                SqlParameter sqlType = new SqlParameter("@ARM_TYPE", Type);
                dbCmd.Parameters.Add(sqlType);
            }
            if (Source != string.Empty)
            {
                SqlParameter sqlSource = new SqlParameter("@ARM_SOURCE", Source);
                dbCmd.Parameters.Add(sqlSource);
            }
            if (Invoice != string.Empty)
            {
                SqlParameter sqlInvoice = new SqlParameter("@ARM_INVOICE", Invoice);
                dbCmd.Parameters.Add(Invoice);
            }
            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_ARSTRAN(string Agency, string Dept, string Prog, string Appno, string Type, string Source, string Invoice,string Entry,string Seq)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[GET_ARSTRAN]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            if (Agency != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@ART_AGENCY", Agency);
                dbCmd.Parameters.Add(sqlAgency);
            }
            if (Dept != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@ART_DEPT", Dept);
                dbCmd.Parameters.Add(sqlDept);
            }
            if (Prog != string.Empty)
            {
                SqlParameter sqlProgram = new SqlParameter("@ART_PROGRAM", Prog);
                dbCmd.Parameters.Add(sqlProgram);
            }
            if (Appno != string.Empty)
            {
                SqlParameter sqlAppno = new SqlParameter("@ART_APP_NO", Appno);
                dbCmd.Parameters.Add(sqlAppno);
            }
            if (Type != string.Empty)
            {
                SqlParameter sqlType = new SqlParameter("@ART_CUST_TYPE", Type);
                dbCmd.Parameters.Add(sqlType);
            }
            if (Source != string.Empty)
            {
                SqlParameter sqlSource = new SqlParameter("@ART_SOURCE", Source);
                dbCmd.Parameters.Add(sqlSource);
            }
            if (Invoice != string.Empty)
            {
                SqlParameter sqlInvoice = new SqlParameter("@ART_INVOICE", Invoice);
                dbCmd.Parameters.Add(sqlInvoice);
            }
            if (Entry != string.Empty)
            {
                SqlParameter sqlInvoice = new SqlParameter("@ART_Item", Entry);
                dbCmd.Parameters.Add(sqlInvoice);
            }
            if (Seq != string.Empty)
            {
                SqlParameter sqlInvoice = new SqlParameter("@ART_SEQ", Seq);
                dbCmd.Parameters.Add(sqlInvoice);
            }
            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        public static bool InsertUpdateDelARSCUST(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelARSCUST");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        public static bool InsertUpdateDelARSMAST(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelARSMAST");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertUpdateDeleteARSTRAN(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDeleteARSTRAN");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool UpdateARSNUMB(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.Update_ARSNUMB");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ARSb2150_Report(string Agency, string Dept, string Prog,string Year, string Appno)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[ARSB2150_Report]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgency = new SqlParameter("@ARS_AGENCY", Agency);
            dbCmd.Parameters.Add(sqlAgency);

            SqlParameter sqlCity = new SqlParameter("@ARS_DEPT", Dept);
            dbCmd.Parameters.Add(sqlCity);

            SqlParameter sqlZip1 = new SqlParameter("@ARS_PROGRAM ", Prog);
            dbCmd.Parameters.Add(sqlZip1);

            SqlParameter sqlYear = new SqlParameter("@MST_YEAR ", Year);
            dbCmd.Parameters.Add(sqlYear);

            if (!string.IsNullOrEmpty(Appno))
            {
                SqlParameter sqlZip2 = new SqlParameter("@ARS_APP_NO", Appno);
                dbCmd.Parameters.Add(sqlZip2);
            }
            

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ARSMASTCHECK(string Agency, string Dept, string Prog,  string Appno,string Custtype)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[ARSB2150]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgency = new SqlParameter("@ARM_AGENCY", Agency);
            dbCmd.Parameters.Add(sqlAgency);

            SqlParameter sqlCity = new SqlParameter("@ARM_DEPT", Dept);
            dbCmd.Parameters.Add(sqlCity);

            SqlParameter sqlZip1 = new SqlParameter("@ARM_PROGRAM ", Prog);
            dbCmd.Parameters.Add(sqlZip1);

            if (!string.IsNullOrEmpty(Custtype.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@ARM_CUST_TYPE ", Custtype);
                dbCmd.Parameters.Add(sqlYear);
            }
            if (!string.IsNullOrEmpty(Appno))
            {
                SqlParameter sqlZip2 = new SqlParameter("@ARM_APP_NO", Appno);
                dbCmd.Parameters.Add(sqlZip2);
            }


            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetARS00125(string Agency, string Dept, string Prog, string Appno, string Custtype)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_ARS02125]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgency = new SqlParameter("@ARM_AGENCY", Agency);
            dbCmd.Parameters.Add(sqlAgency);

            SqlParameter sqlCity = new SqlParameter("@ARM_DEPT", Dept);
            dbCmd.Parameters.Add(sqlCity);

            SqlParameter sqlZip1 = new SqlParameter("@ARM_PROGRAM ", Prog);
            dbCmd.Parameters.Add(sqlZip1);

            if (!string.IsNullOrEmpty(Custtype.Trim()))
            {
                SqlParameter sqlYear = new SqlParameter("@ARM_CUST_TYPE ", Custtype);
                dbCmd.Parameters.Add(sqlYear);
            }
            if (!string.IsNullOrEmpty(Appno))
            {
                SqlParameter sqlZip2 = new SqlParameter("@ARM_APP_NO", Appno);
                dbCmd.Parameters.Add(sqlZip2);
            }


            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ARSb2140_Reportfor_CUstomers(string Agency, string Dept, string Prog, string Year, string Appno,string Report)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[ARSB2140_Report_CustOMERS]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgency = new SqlParameter("@ARS_AGENCY", Agency);
            dbCmd.Parameters.Add(sqlAgency);

            SqlParameter sqlCity = new SqlParameter("@ARS_DEPT", Dept);
            dbCmd.Parameters.Add(sqlCity);

            SqlParameter sqlZip1 = new SqlParameter("@ARS_PROGRAM ", Prog);
            dbCmd.Parameters.Add(sqlZip1);

            SqlParameter sqlYear = new SqlParameter("@MST_YEAR ", Year);
            dbCmd.Parameters.Add(sqlYear);

            if (!string.IsNullOrEmpty(Appno))
            {
                SqlParameter sqlZip2 = new SqlParameter("@ARS_APP_NO", Appno);
                dbCmd.Parameters.Add(sqlZip2);
            }

            SqlParameter REP = new SqlParameter("@Rep_Name", Report);
            dbCmd.Parameters.Add(REP);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ARSb2140_Report(string Agency, string Dept, string Prog, string Appno, string CustType,string Report)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[ARSB2140_REPORT]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgency = new SqlParameter("@ARM_AGENCY", Agency);
            dbCmd.Parameters.Add(sqlAgency);

            SqlParameter sqlCity = new SqlParameter("@ARM_DEPT", Dept);
            dbCmd.Parameters.Add(sqlCity);

            SqlParameter sqlZip1 = new SqlParameter("@ARM_PROGRAM ", Prog);
            dbCmd.Parameters.Add(sqlZip1);

            if (!string.IsNullOrEmpty(Appno))
            {
                SqlParameter sqlZip2 = new SqlParameter("@ARM_APP_NO", Appno);
                dbCmd.Parameters.Add(sqlZip2);
            }

            if (!string.IsNullOrEmpty(CustType))
            {
                SqlParameter sqlYear = new SqlParameter("@ARM_CUST_TYPE ", CustType);
                dbCmd.Parameters.Add(sqlYear);
            }

            SqlParameter SqlRep = new SqlParameter("@Rep_Screen", Report);
            dbCmd.Parameters.Add(SqlRep);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ARSb2120_MSTList(string Agency, string Dept, string Prog, string Year,string AppNo)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[ARSB2120_Mstlist]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgency = new SqlParameter("@MST_AGENCY", Agency);
            dbCmd.Parameters.Add(sqlAgency);

            SqlParameter sqlCity = new SqlParameter("@MST_DEPT", Dept);
            dbCmd.Parameters.Add(sqlCity);

            SqlParameter sqlZip1 = new SqlParameter("@MST_PROGRAM ", Prog);
            dbCmd.Parameters.Add(sqlZip1);

            SqlParameter sqlYear = new SqlParameter("@MST_YEAR ", Year);
            dbCmd.Parameters.Add(sqlYear);

            if (!string.IsNullOrEmpty(AppNo))
            {
                SqlParameter sqlApp = new SqlParameter("@MST_APP_NO ", AppNo);
                dbCmd.Parameters.Add(sqlApp);
            }
            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ARSb2120_Report(string Agency, string Dept, string Prog, string Year, string MulSites, string FundHie, string Frmdt, string Todt,string posted)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[ARSB2120_Report]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgency = new SqlParameter("@ATTN_AGENCY", Agency);
            dbCmd.Parameters.Add(sqlAgency);

            SqlParameter sqlCity = new SqlParameter("@ATTN_DEPT", Dept);
            dbCmd.Parameters.Add(sqlCity);

            SqlParameter sqlZip1 = new SqlParameter("@ATTN_PROGRAM", Prog);
            dbCmd.Parameters.Add(sqlZip1);

            if (!string.IsNullOrEmpty(Year))
            {
                SqlParameter sqlZip2 = new SqlParameter("@ATTN_YEAR", Year);
                dbCmd.Parameters.Add(sqlZip2);
            }

            if (!string.IsNullOrEmpty(MulSites))
            {
                SqlParameter sqlYear = new SqlParameter("@Site", MulSites);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(FundHie))
            {
                SqlParameter sqlYear = new SqlParameter("@FundHie", FundHie);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(Frmdt))
            {
                SqlParameter sqlYear = new SqlParameter("@FROMDATE", Frmdt);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(Todt))
            {
                SqlParameter sqlYear = new SqlParameter("@TODATE", Todt);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(posted))
            {
                SqlParameter sqlYear = new SqlParameter("@PostedAR", posted);
                dbCmd.Parameters.Add(sqlYear);
            }

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ARSb2130_Report(string Agency, string Dept, string Prog, string Year, string MulSites, string FundHie,string AsofDate)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[ARSB2130_Report]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgency = new SqlParameter("@ENRL_AGENCY", Agency);
            dbCmd.Parameters.Add(sqlAgency);

            SqlParameter sqlCity = new SqlParameter("@ENRL_DEPT", Dept);
            dbCmd.Parameters.Add(sqlCity);

            SqlParameter sqlZip1 = new SqlParameter("@ENRL_PROGRAM", Prog);
            dbCmd.Parameters.Add(sqlZip1);

            if (!string.IsNullOrEmpty(Year))
            {
                SqlParameter sqlZip2 = new SqlParameter("@ENRL_YEAR", Year);
                dbCmd.Parameters.Add(sqlZip2);
            }

            if (!string.IsNullOrEmpty(MulSites))
            {
                SqlParameter sqlYear = new SqlParameter("@Site", MulSites);
                dbCmd.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(FundHie))
            {
                SqlParameter sqlYear = new SqlParameter("@FundHie", FundHie);
                dbCmd.Parameters.Add(sqlYear);
            }

            SqlParameter sqldate = new SqlParameter("@Asofdate", AsofDate);
            dbCmd.Parameters.Add(sqldate);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_ARSCHECK(string Agency, string Dept, string Prog, string Appno, string Type, string Source, string Check)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[GET_ARSCHECK]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            if (Agency != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@ARC_AGENCY", Agency);
                dbCmd.Parameters.Add(sqlAgency);
            }
            if (Dept != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@ARC_DEPT", Dept);
                dbCmd.Parameters.Add(sqlDept);
            }
            if (Prog != string.Empty)
            {
                SqlParameter sqlProgram = new SqlParameter("@ARC_PROGRAM", Prog);
                dbCmd.Parameters.Add(sqlProgram);
            }
            if (Appno != string.Empty)
            {
                SqlParameter sqlAppno = new SqlParameter("@ARC_APP_NO", Appno);
                dbCmd.Parameters.Add(sqlAppno);
            }
            if (Type != string.Empty)
            {
                SqlParameter sqlType = new SqlParameter("@ARC_CUST_TYPE", Type);
                dbCmd.Parameters.Add(sqlType);
            }
            if (Source != string.Empty)
            {
                SqlParameter sqlSource = new SqlParameter("@ARC_SOURCE", Source);
                dbCmd.Parameters.Add(sqlSource);
            }
            if (Check != string.Empty)
            {
                SqlParameter sqlCheck = new SqlParameter("@ARC_CHECK", Check);
                dbCmd.Parameters.Add(sqlCheck);
            }
            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        public static bool InsertUpdateDelARSCHECK(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelARSCHECK");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        public static bool InsertUpdateDelARSCHKDET(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDeleteARSCHKDET");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool UpdateARSCHKDET(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateARSCHKDET");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_ARSChkDetDetails(string Agency, string Dept, string Prog, string Appno, string Type, string Source, string Invoice, string Entry, string Seq,string Check)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[GET_ARSCHKDET]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            if (Agency != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@ARD_AGENCY", Agency);
                dbCmd.Parameters.Add(sqlAgency);
            }
            if (Dept != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@ARD_DEPT", Dept);
                dbCmd.Parameters.Add(sqlDept);
            }
            if (Prog != string.Empty)
            {
                SqlParameter sqlProgram = new SqlParameter("@ARD_PROGRAM", Prog);
                dbCmd.Parameters.Add(sqlProgram);
            }
            if (Appno != string.Empty)
            {
                SqlParameter sqlAppno = new SqlParameter("@ARD_APP_NO", Appno);
                dbCmd.Parameters.Add(sqlAppno);
            }
            if (Type != string.Empty)
            {
                SqlParameter sqlType = new SqlParameter("@ARD_CUST_TYPE", Type);
                dbCmd.Parameters.Add(sqlType);
            }
            if (Source != string.Empty)
            {
                SqlParameter sqlSource = new SqlParameter("@ARD_SOURCE", Source);
                dbCmd.Parameters.Add(sqlSource);
            }
            if (Invoice != string.Empty)
            {
                SqlParameter sqlInvoice = new SqlParameter("@ARD_INVOICE", Invoice);
                dbCmd.Parameters.Add(sqlInvoice);
            }
            if (Entry != string.Empty)
            {
                SqlParameter sqlEntry = new SqlParameter("@ARD_Item", Entry);
                dbCmd.Parameters.Add(sqlEntry);
            }
            if (Seq != string.Empty)
            {
                SqlParameter sqlSeq = new SqlParameter("@ARD_SEQ", Seq);
                dbCmd.Parameters.Add(sqlSeq);
            }
            if (Check != string.Empty)
            {
                SqlParameter sqlCheck = new SqlParameter("@ARD_CHECK", Check);
                dbCmd.Parameters.Add(sqlCheck);
            }
            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CleanARS_Records(string Agency, string Dept, string Prog, string year, string frmdate, string todate)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[CleanRecs_ARSTABLES]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);
            if (Agency != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@AGENCY", Agency);
                dbCmd.Parameters.Add(sqlAgency);
            }
            if (Dept != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@DEPT", Dept);
                dbCmd.Parameters.Add(sqlDept);
            }
            if (Prog != string.Empty)
            {
                SqlParameter sqlProgram = new SqlParameter("@PROGRAM", Prog);
                dbCmd.Parameters.Add(sqlProgram);
            }
            if (year != string.Empty)
            {
                SqlParameter sqlAppno = new SqlParameter("@YEAR", year);
                dbCmd.Parameters.Add(sqlAppno);
            }
            if (frmdate != string.Empty)
            {
                SqlParameter sqlType = new SqlParameter("@FROMDATE", frmdate);
                dbCmd.Parameters.Add(sqlType);
            }
            if (todate != string.Empty)
            {
                SqlParameter sqlSource = new SqlParameter("@TODATE", todate);
                dbCmd.Parameters.Add(sqlSource);
            }
            //if (Invoice != string.Empty)
            //{
            //    SqlParameter sqlInvoice = new SqlParameter("@ARD_INVOICE", Invoice);
            //    dbCmd.Parameters.Add(sqlInvoice);
            //}
            //if (Entry != string.Empty)
            //{
            //    SqlParameter sqlEntry = new SqlParameter("@ARD_Item", Entry);
            //    dbCmd.Parameters.Add(sqlEntry);
            //}
            //if (Seq != string.Empty)
            //{
            //    SqlParameter sqlSeq = new SqlParameter("@ARD_SEQ", Seq);
            //    dbCmd.Parameters.Add(sqlSeq);
            //}
            //if (Check != string.Empty)
            //{
            //    SqlParameter sqlCheck = new SqlParameter("@ARD_CHECK", Check);
            //    dbCmd.Parameters.Add(sqlCheck);
            //}
            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ARSb2170_Report(string Agency, string Dept, string Prog, string Invdate)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[ARSB2170_Report]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(Agency))
            {
                SqlParameter sqlAgency = new SqlParameter("@AGENCY", Agency);
                dbCmd.Parameters.Add(sqlAgency);
            }

            if (!string.IsNullOrEmpty(Agency))
            {
                SqlParameter sqlCity = new SqlParameter("@DEPT", Dept);
                dbCmd.Parameters.Add(sqlCity);
            }

            if (!string.IsNullOrEmpty(Agency))
            {
                SqlParameter sqlZip1 = new SqlParameter("@PROG", Prog);
                dbCmd.Parameters.Add(sqlZip1);
            }

            if (!string.IsNullOrEmpty(Invdate))
            {
                SqlParameter sqlYear = new SqlParameter("@DATE", Invdate);
                dbCmd.Parameters.Add(sqlYear);
            }

            //if (!string.IsNullOrEmpty(Appno))
            //{
            //    SqlParameter sqlZip2 = new SqlParameter("@ARS_APP_NO", Appno);
            //    dbCmd.Parameters.Add(sqlZip2);
            //}


            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }


    }
}
