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
    public partial class AgyTab
    {

        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[AGYTAB]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;
        #endregion

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetAgyTab(string AgyCode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetAgyTab]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (AgyCode != string.Empty)
            {
                SqlParameter isIntakeParm = new SqlParameter("@AGY_Type", AgyCode.Trim());
                dbCommand.Parameters.Add(isIntakeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetAgencyTableByApp(string AppCode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetAgyTab]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (AppCode != string.Empty)
            {
                SqlParameter appCode = new SqlParameter("@AppCode", AppCode.Trim());
                dbCommand.Parameters.Add(appCode);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetAgyTabDetails(string AgyType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetAgyTabDetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (AgyType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AGY_TYPE", AgyType);
                dbCommand.Parameters.Add(empnoParm);

            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetHierarchyNames(string Agency, string Dept, string Prog)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetHierarchyNames]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgency = new SqlParameter("@HIE_AGENCY", Agency);
            dbCommand.Parameters.Add(sqlAgency);
            SqlParameter sqlDept = new SqlParameter("@HIE_DEPT", Dept);
            dbCommand.Parameters.Add(sqlDept);
            SqlParameter sqlProg = new SqlParameter("@HIE_PROGRAM", Prog);
            dbCommand.Parameters.Add(sqlProg);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetSelAgyChildDetails(string AgyType1, string AgyColCode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetSelAgyChildDetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (AgyType1 != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AGY_TYPE", AgyType1);
                dbCommand.Parameters.Add(empnoParm);
            }
            //if (AgyColName != string.Empty)
            //{
            //    SqlParameter empnoParm = new SqlParameter("@AGY_COLNAME", AgyColName);
            //    dbCommand.Parameters.Add(empnoParm);
            //}
            if (AgyColCode != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AGY_COLCODE", AgyColCode);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool UpdateAGYTAB(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateAGYTAB");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertUpdateAGYTABS(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateAGYTABS");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }




        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool DeleteAGYTAB(string type, string code)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteAGYTAB");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgency = new SqlParameter("@AGYTYPE", type);
            _dbCommand.Parameters.Add(sqlAgency);
            SqlParameter sqlCode = new SqlParameter("@AGYCODE", code);
            _dbCommand.Parameters.Add(sqlCode);

            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool DeleteAGYTABSingle(string type, string code,string seq)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteAGYTABSINGLE");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgency = new SqlParameter("@AGYTYPE", type);
            _dbCommand.Parameters.Add(sqlAgency);
            SqlParameter sqlCode = new SqlParameter("@AGYCODE", code);
            _dbCommand.Parameters.Add(sqlCode);
            SqlParameter sqlseq = new SqlParameter("@AGYSEQ", seq);
            _dbCommand.Parameters.Add(sqlseq);

            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertAgyTabDB(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertAgyTabDB");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetSystemDBS(string strType,string strDB)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetSysDB]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);


            if (strType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strDB != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@DBNAME", strDB);
                dbCommand.Parameters.Add(empnoParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCTXMLTAGSData()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCTXMLTAGSDetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
          

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        
         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCTXMLASOCData(string strCatg, string strScatg)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCTXMLASOCDetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);


            if (strCatg != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CTXMLASOC_CATG", strCatg);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strScatg != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CTXMLASOC_SCATG", strScatg);
                dbCommand.Parameters.Add(empnoParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


         public static bool InsertUpdateCTXMLASOC(List<SqlParameter> sqlParamList)
         {
             _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateCTXMLASOC");
             _dbCommand.CommandTimeout = 1200;
             _dbCommand.Parameters.Clear();
             foreach (SqlParameter sqlPar in sqlParamList)
             {
                 _dbCommand.Parameters.Add(sqlPar);
             }
             return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
         }



         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GETXMLDATABRIDGE(string strAgency, string strDept,string strProgram,string strYear,string strFromDt,string strToDt,string strCumType)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GETXMLDATABRIDGE]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);


             if (strAgency != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Agency", strAgency);
                 dbCommand.Parameters.Add(empnoParm);
             }

             if (strDept != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Dept", strDept);
                 dbCommand.Parameters.Add(empnoParm);
             }

             if (strProgram != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Program", strProgram);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (strYear != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Year", strYear);
                 dbCommand.Parameters.Add(empnoParm);
             }

             if (strFromDt != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@FromDate", strFromDt);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (strToDt != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@ToDate", strToDt);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (strCumType != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@CUMType", strCumType);
                 dbCommand.Parameters.Add(empnoParm);
             }

             // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GETXMLDATA0023(string strAgency, string strDept, string strProgram, string strYear, string strFromDt, string strToDt, string strCumType,string strType,string strseq,string strvdndate,string App)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GETXMLDATA0023]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);
             dbCommand.CommandTimeout = 1200;

             if (strAgency != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Agency", strAgency);
                 dbCommand.Parameters.Add(empnoParm);
             }

             if (strDept != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Dept", strDept);
                 dbCommand.Parameters.Add(empnoParm);
             }

             if (strProgram != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Program", strProgram);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (strYear != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Year", strYear);
                 dbCommand.Parameters.Add(empnoParm);
             }

             if (strFromDt != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@FromDate", strFromDt);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (strToDt != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@ToDate", strToDt);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (strCumType != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@CUMType", strCumType);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (strType != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Type", strType);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (strseq != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Seq", strseq);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (strvdndate != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@VdnDate", strvdndate);
                 dbCommand.Parameters.Add(empnoParm);
             }

             if (App != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@App", App);
                 dbCommand.Parameters.Add(empnoParm);
             }

             // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }


         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GETXMLDATABRIDGETMSB2022(string strAgency, string strDept, string strProgram, string strYear, string strFromDt, string strToDt, string strCumType)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GETXMLDATABRIDGETMSB2022]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);
             dbCommand.CommandTimeout = 1800;

             if (strAgency != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Agency", strAgency);
                 dbCommand.Parameters.Add(empnoParm);
             }

             if (strDept != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Dept", strDept);
                 dbCommand.Parameters.Add(empnoParm);
             }

             if (strProgram != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Program", strProgram);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (strYear != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Year", strYear);
                 dbCommand.Parameters.Add(empnoParm);
             }

             if (strFromDt != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@FromDate", strFromDt);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (strToDt != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@ToDate", strToDt);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (strCumType != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@CUMType", strCumType);
                 dbCommand.Parameters.Add(empnoParm);
             }

             // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GETXMLDATABRIDGEMATASMT(string strAgency, string strDept, string strProgram, string strYear, string strFromDt, string strToDt, string strCumType)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GETXMLDATABRIDGEMATASMT]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);
             dbCommand.CommandTimeout = 1800;

             if (strAgency != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Agency", strAgency);
                 dbCommand.Parameters.Add(empnoParm);
             }

             if (strDept != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Dept", strDept);
                 dbCommand.Parameters.Add(empnoParm);
             }

             if (strProgram != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Program", strProgram);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (strYear != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Year", strYear);
                 dbCommand.Parameters.Add(empnoParm);
             }

             if (strFromDt != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@FromDate", strFromDt);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (strToDt != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@ToDate", strToDt);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (strCumType != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@CUMType", strCumType);
                 dbCommand.Parameters.Add(empnoParm);
             }

             // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GETXMLTMSB2022SERVICEDATA(string strAgency, string strDept, string strProgram, string strYear, string strFromDt, string strToDt, string strCumType, string strMode,string strFilterApp)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GETXMLTMSB2022SERVICEDATA]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);
             dbCommand.CommandTimeout = 1800;

             if (strAgency != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Agency", strAgency);
                 dbCommand.Parameters.Add(empnoParm);
             }

             if (strDept != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Dept", strDept);
                 dbCommand.Parameters.Add(empnoParm);
             }

             if (strProgram != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Program", strProgram);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (strYear != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Year", strYear);
                 dbCommand.Parameters.Add(empnoParm);
             }

             if (strFromDt != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@FromDate", strFromDt);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (strToDt != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@ToDate", strToDt);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (strCumType != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@CUMType", strCumType);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (strMode != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Mode", strMode);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (strFilterApp != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@FilterApp", strFilterApp);
                 dbCommand.Parameters.Add(empnoParm);
             }
             

             // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }


         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GETXMLHLSB0001_Report(string strAgency, string strDept, string strProgram, string strYear, string strAppno)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[HLSB0001_REPORT]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);
             dbCommand.CommandTimeout = 1800;

             if (strAgency != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@AGENCY", strAgency);
                 dbCommand.Parameters.Add(empnoParm);
             }

             if (strDept != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@DEPT", strDept);
                 dbCommand.Parameters.Add(empnoParm);
             }

             if (strProgram != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@PROGRAM", strProgram);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (strYear != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@YEAR", strYear);
                 dbCommand.Parameters.Add(empnoParm);
             }

             if (strAppno != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@APPNO", strAppno);
                 dbCommand.Parameters.Add(empnoParm);
             }

             //if (strToDt != string.Empty)
             //{
             //    SqlParameter empnoParm = new SqlParameter("@ToDate", strToDt);
             //    dbCommand.Parameters.Add(empnoParm);
             //}
             //if (strCumType != string.Empty)
             //{
             //    SqlParameter empnoParm = new SqlParameter("@CUMType", strCumType);
             //    dbCommand.Parameters.Add(empnoParm);
             //}
             //if (strMode != string.Empty)
             //{
             //    SqlParameter empnoParm = new SqlParameter("@Mode", strMode);
             //    dbCommand.Parameters.Add(empnoParm);
             //}
             //if (strFilterApp != string.Empty)
             //{
             //    SqlParameter empnoParm = new SqlParameter("@FilterApp", strFilterApp);
             //    dbCommand.Parameters.Add(empnoParm);
             //}


             // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GETXMLAPCDATA(string strAgency, string strDept, string strProgram, string strYear, string strFromDt)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[APC_XML]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);


            if (strAgency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Agency", strAgency);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strDept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Dept", strDept);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strProgram != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Program", strProgram);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strYear != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Year", strYear);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strFromDt != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FDate", strFromDt);
                dbCommand.Parameters.Add(empnoParm);
            }


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }
    }
}
