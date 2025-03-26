using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Captain.DatabaseLayer
{
    
    [DataObject]
    [Serializable]
    public partial class Matrix
    {
        #region Constants
        private static readonly string TABLE_NAME = "[dbo].[MATDEF]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;


        #endregion

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_Matdef_MatCode(string strMatCode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Get_Matdef_MatCode]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            if (strMatCode != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@MatCode", strMatCode);
                dbCommand.Parameters.Add(agencyParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GETMATOUTCMatCode(string strMatCode,string strScrCode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETMATOUTCMatCode]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            if (strMatCode != string.Empty)
            {
                SqlParameter matcodeParm = new SqlParameter("@MATOUTC_MAT_CODE", strMatCode);
                dbCommand.Parameters.Add(matcodeParm);
            }
            if (strScrCode != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATOUTC_SCL_CODE", strScrCode);
                dbCommand.Parameters.Add(scrcodeParm);
            }
            
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //public static DataSet GetCASEMSTadpyn(string Agency, string Dept, string Program, string Year, string AppNo)
        //{
        //    DataSet ds;
        //    Database db;
        //    string sqlCommand;
        //    DbCommand dbCommand;

        //    db = DatabaseFactory.CreateDatabase();
        //    sqlCommand = "[dbo].[GetCASEMSTadpyn]";
        //    dbCommand = db.GetStoredProcCommand(sqlCommand);

        //    if (Agency != string.Empty)
        //    {
        //        SqlParameter agencyParm = new SqlParameter("@MST_AGENCY", Agency);
        //        dbCommand.Parameters.Add(agencyParm);
        //    }
        //    if (Dept != string.Empty)
        //    {
        //        SqlParameter deptParm = new SqlParameter("@MST_DEPT", Dept);
        //        dbCommand.Parameters.Add(deptParm);
        //    }
        //    if (Program != string.Empty)
        //    {
        //        SqlParameter programParm = new SqlParameter("@MST_PROGRAM", Program);
        //        dbCommand.Parameters.Add(programParm);
        //    }
        //    if (Year != string.Empty)
        //    {
        //        SqlParameter typeParm = new SqlParameter("@MST_YEAR", Year);
        //        dbCommand.Parameters.Add(typeParm);
        //    }
        //    if (AppNo != string.Empty)
        //    {
        //        SqlParameter typeParm = new SqlParameter("@MST_APP_NO", AppNo);
        //        dbCommand.Parameters.Add(typeParm);
        //    }

        //    // Get results.
        //    ds = db.ExecuteDataSet(dbCommand);
        //    return ds;
        //}

        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //public static DataSet GetCASESNP()
        //{
        //    DataSet ds;
        //    Database db;
        //    string sqlCommand;
        //    DbCommand dbCommand;

        //    db = DatabaseFactory.CreateDatabase();
        //    sqlCommand = "[dbo].[GetCASESNP]";
        //    dbCommand = db.GetStoredProcCommand(sqlCommand);

        //    // Get results.
        //    ds = db.ExecuteDataSet(dbCommand);
        //    return ds;
        //}

        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //public static DataSet GetCASESNPadpyn(string Agency, string Dept, string Program, string Year, string AppNo)
        //{
        //    DataSet ds;
        //    Database db;
        //    string sqlCommand;
        //    DbCommand dbCommand;

        //    db = DatabaseFactory.CreateDatabase();
        //    sqlCommand = "[dbo].[GetCASESNPadpyn]";
        //    dbCommand = db.GetStoredProcCommand(sqlCommand);

        //    if (Agency != string.Empty)
        //    {
        //        SqlParameter agencyParm = new SqlParameter("@SNP_AGENCY", Agency);
        //        dbCommand.Parameters.Add(agencyParm);
        //    }
        //    if (Dept != string.Empty)
        //    {
        //        SqlParameter deptParm = new SqlParameter("@SNP_DEPT", Dept);
        //        dbCommand.Parameters.Add(deptParm);
        //    }
        //    if (Program != string.Empty)
        //    {
        //        SqlParameter programParm = new SqlParameter("@SNP_PROGRAM", Program);
        //        dbCommand.Parameters.Add(programParm);
        //    }
        //    if (Year != string.Empty)
        //    {
        //        SqlParameter typeParm = new SqlParameter("@SNP_YEAR", Year);
        //        dbCommand.Parameters.Add(typeParm);
        //    }
        //    if (AppNo != string.Empty)
        //    {
        //        SqlParameter typeParm = new SqlParameter("@SNP_APP", AppNo);
        //        dbCommand.Parameters.Add(typeParm);
        //    }

        //    // Get results.
        //    ds = db.ExecuteDataSet(dbCommand);
        //    return ds;
        //}

        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //public static DataSet GetCASEINCOME()
        //{
        //    DataSet ds;
        //    Database db;
        //    string sqlCommand;
        //    DbCommand dbCommand;

        //    db = DatabaseFactory.CreateDatabase();
        //    sqlCommand = "[dbo].[GetCASEINCOME]";
        //    dbCommand = db.GetStoredProcCommand(sqlCommand);

        //    // Get results.
        //    ds = db.ExecuteDataSet(dbCommand);
        //    return ds;
        //}

        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //public static DataSet GetCaseWorker(string Type, string Agency, string Dept, string Program)
        //{
        //    DataSet ds;
        //    Database db;
        //    string sqlCommand;
        //    DbCommand dbCommand;

        //    db = DatabaseFactory.CreateDatabase();
        //    sqlCommand = "[dbo].[GetPasswordhie]";
        //    dbCommand = db.GetStoredProcCommand(sqlCommand);
        //    SqlParameter typeParm = new SqlParameter("@Type", Type);
        //    dbCommand.Parameters.Add(typeParm);
        //    if (Agency != string.Empty)
        //    {
        //        SqlParameter agencyParm = new SqlParameter("@AGENCY", Agency);
        //        dbCommand.Parameters.Add(agencyParm);
        //    }
        //    if (Dept != string.Empty)
        //    {
        //        SqlParameter deptParm = new SqlParameter("@DEPT", Dept);
        //        dbCommand.Parameters.Add(deptParm);
        //    }
        //    if (Program != string.Empty)
        //    {
        //        SqlParameter programParm = new SqlParameter("@PROG", Program);
        //        dbCommand.Parameters.Add(programParm);
        //    }

        //    // Get results.
        //    ds = db.ExecuteDataSet(dbCommand);
        //    return ds;
        //}

        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //public static DataSet GetCASEINCOMEadpynf(string Agency, string Dept, string Program, string Year, string AppNo, string FamilySeqNo)
        //{
        //    DataSet ds;
        //    Database db;
        //    string sqlCommand;
        //    DbCommand dbCommand;

        //    db = DatabaseFactory.CreateDatabase();
        //    sqlCommand = "[dbo].[GetCASEINCOMEadpynf]";
        //    dbCommand = db.GetStoredProcCommand(sqlCommand);

        //    if (Agency != string.Empty)
        //    {
        //        SqlParameter agencyParm = new SqlParameter("@INCOME_AGENCY", Agency);
        //        dbCommand.Parameters.Add(agencyParm);
        //    }
        //    if (Dept != string.Empty)
        //    {
        //        SqlParameter deptParm = new SqlParameter("@INCOME_DEPT", Dept);
        //        dbCommand.Parameters.Add(deptParm);
        //    }
        //    if (Program != string.Empty)
        //    {
        //        SqlParameter programParm = new SqlParameter("@INCOME_PROGRAM", Program);
        //        dbCommand.Parameters.Add(programParm);
        //    }
        //    if (Year != string.Empty)
        //    {
        //        SqlParameter typeParm = new SqlParameter("@INCOME_YEAR", Year);
        //        dbCommand.Parameters.Add(typeParm);
        //    }
        //    if (AppNo != string.Empty)
        //    {
        //        SqlParameter typeParm = new SqlParameter("@INCOME_APP", AppNo);
        //        dbCommand.Parameters.Add(typeParm);
        //    }
        //    if (FamilySeqNo != string.Empty)
        //    {
        //        SqlParameter typeParm = new SqlParameter("@INCOME_FAMILY_SEQ", FamilySeqNo);
        //        dbCommand.Parameters.Add(typeParm);
        //    }

        //    // Get results.
        //    ds = db.ExecuteDataSet(dbCommand);
        //    return ds;
        //}


        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //public static DataSet GetCASEINCOMEadpynfs(string Agency, string Dept, string Program, string Year, string AppNo, string FamilySeqNo, string Seq)
        //{
        //    DataSet ds;
        //    Database db;
        //    string sqlCommand;
        //    DbCommand dbCommand;

        //    db = DatabaseFactory.CreateDatabase();
        //    sqlCommand = "[dbo].[GetCASEINCOMEadpynfs]";
        //    dbCommand = db.GetStoredProcCommand(sqlCommand);

        //    if (Agency != string.Empty)
        //    {
        //        SqlParameter agencyParm = new SqlParameter("@INCOME_AGENCY", Agency);
        //        dbCommand.Parameters.Add(agencyParm);
        //    }
        //    if (Dept != string.Empty)
        //    {
        //        SqlParameter deptParm = new SqlParameter("@INCOME_DEPT", Dept);
        //        dbCommand.Parameters.Add(deptParm);
        //    }
        //    if (Program != string.Empty)
        //    {
        //        SqlParameter programParm = new SqlParameter("@INCOME_PROGRAM", Program);
        //        dbCommand.Parameters.Add(programParm);
        //    }
        //    if (Year != string.Empty)
        //    {
        //        SqlParameter typeParm = new SqlParameter("@INCOME_YEAR", Year);
        //        dbCommand.Parameters.Add(typeParm);
        //    }
        //    if (AppNo != string.Empty)
        //    {
        //        SqlParameter typeParm = new SqlParameter("@INCOME_APP", AppNo);
        //        dbCommand.Parameters.Add(typeParm);
        //    }
        //    if (FamilySeqNo != string.Empty)
        //    {
        //        SqlParameter typeParm = new SqlParameter("@INCOME_FAMILY_SEQ", FamilySeqNo);
        //        dbCommand.Parameters.Add(typeParm);
        //    }

        //    if (Seq != string.Empty)
        //    {
        //        SqlParameter typeParm = new SqlParameter("@INCOME_SEQ", Seq);
        //        dbCommand.Parameters.Add(typeParm);
        //    }

        //    // Get results.
        //    ds = db.ExecuteDataSet(dbCommand);
        //    return ds;
        //}


        //public static bool InsertUpdateCASEMST(List<SqlParameter> sqlParamList)
        //{
        //    _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSERTUpdateCaseMST");
        //    _dbCommand.CommandTimeout = 1200;
        //    _dbCommand.Parameters.Clear();
        //    foreach (SqlParameter sqlPar in sqlParamList)
        //    {
        //        _dbCommand.Parameters.Add(sqlPar);
        //    }
        //    return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        //}

        //public static bool UpdateCASEMST(List<SqlParameter> sqlParamList)
        //{
        //    _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateCASEMST");
        //    _dbCommand.CommandTimeout = 1200;
        //    _dbCommand.Parameters.Clear();
        //    foreach (SqlParameter sqlPar in sqlParamList)
        //    {
        //        _dbCommand.Parameters.Add(sqlPar);
        //    }
        //    return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        //}

        //public static bool InsertCASESNP(List<SqlParameter> sqlParamList)
        //{
        //    _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertCASESNP");
        //    _dbCommand.CommandTimeout = 1200;
        //    _dbCommand.Parameters.Clear();
        //    foreach (SqlParameter sqlPar in sqlParamList)
        //    {
        //        _dbCommand.Parameters.Add(sqlPar);
        //    }
        //    return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        //}

        //public static bool UpdateCASESNP(List<SqlParameter> sqlParamList)
        //{
        //    _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateCASESNP");
        //    _dbCommand.CommandTimeout = 1200;
        //    _dbCommand.Parameters.Clear();
        //    foreach (SqlParameter sqlPar in sqlParamList)
        //    {
        //        _dbCommand.Parameters.Add(sqlPar);
        //    }
        //    return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        //}


        //public static bool InsertCASEINCOME(List<SqlParameter> sqlParamList)
        //{
        //    _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertCASEINCOME");
        //    _dbCommand.CommandTimeout = 1200;
        //    _dbCommand.Parameters.Clear();
        //    foreach (SqlParameter sqlPar in sqlParamList)
        //    {
        //        _dbCommand.Parameters.Add(sqlPar);
        //    }
        //    return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        //}

        //public static bool UpdateCASEINCOME(List<SqlParameter> sqlParamList)
        //{
        //    _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateCASEINCOME");
        //    _dbCommand.CommandTimeout = 1200;
        //    _dbCommand.Parameters.Clear();
        //    foreach (SqlParameter sqlPar in sqlParamList)
        //    {
        //        _dbCommand.Parameters.Add(sqlPar);
        //    }
        //    return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        //}

        //public static bool DeleteCASEINCOME(List<SqlParameter> sqlParamList)
        //{
        //    _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteCASEINCOME");
        //    _dbCommand.CommandTimeout = 1200;
        //    _dbCommand.Parameters.Clear();
        //    foreach (SqlParameter sqlPar in sqlParamList)
        //    {
        //        _dbCommand.Parameters.Add(sqlPar);
        //    }
        //    return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        //}

        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //public static DataSet GetCASEDiffadpya(string Agency, string Dept, string Program, string Year, string AppNo)
        //{
        //    DataSet ds;
        //    Database db;
        //    string sqlCommand;
        //    DbCommand dbCommand;

        //    db = DatabaseFactory.CreateDatabase();
        //    sqlCommand = "[dbo].[GetCASEDiffadpya]";
        //    dbCommand = db.GetStoredProcCommand(sqlCommand);

        //    if (Agency != string.Empty)
        //    {
        //        SqlParameter agencyParm = new SqlParameter("@DIFF_AGENCY", Agency);
        //        dbCommand.Parameters.Add(agencyParm);
        //    }
        //    if (Dept != string.Empty)
        //    {
        //        SqlParameter deptParm = new SqlParameter("@DIFF_DEPT", Dept);
        //        dbCommand.Parameters.Add(deptParm);
        //    }
        //    if (Program != string.Empty)
        //    {
        //        SqlParameter programParm = new SqlParameter("@DIFF_PROGRAM", Program);
        //        dbCommand.Parameters.Add(programParm);
        //    }
        //    if (Year != string.Empty)
        //    {
        //        SqlParameter typeParm = new SqlParameter("@DIFF_YEAR", Year);
        //        dbCommand.Parameters.Add(typeParm);
        //    }
        //    if (AppNo != string.Empty)
        //    {
        //        SqlParameter typeParm = new SqlParameter("@DIFF_APP_NO", AppNo);
        //        dbCommand.Parameters.Add(typeParm);
        //    }
        //    // Get results.
        //    ds = db.ExecuteDataSet(dbCommand);
        //    return ds;
        //}

        //public static bool InsertUpdateDelCASEDiff(List<SqlParameter> sqlParamList)
        //{
        //    _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelCASEDIFF");
        //    _dbCommand.CommandTimeout = 1200;
        //    _dbCommand.Parameters.Clear();
        //    foreach (SqlParameter sqlPar in sqlParamList)
        //    {
        //        _dbCommand.Parameters.Add(sqlPar);
        //    }
        //    return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        //}

        //public static bool InsertUpdateDelMSTSER(List<SqlParameter> sqlParamList)
        //{
        //    _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertMSTSER");
        //    _dbCommand.CommandTimeout = 1200;
        //    _dbCommand.Parameters.Clear();
        //    foreach (SqlParameter sqlPar in sqlParamList)
        //    {
        //        _dbCommand.Parameters.Add(sqlPar);
        //    }
        //    return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        //}

        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //public static DataSet GetCASEVeradpynd(string Agency, string Dept, string Program, string Year, string AppNo, string VerifyDate)
        //{
        //    DataSet ds;
        //    Database db;
        //    string sqlCommand;
        //    DbCommand dbCommand;

        //    db = DatabaseFactory.CreateDatabase();
        //    sqlCommand = "[dbo].[GetCASEVeradpynd]";
        //    dbCommand = db.GetStoredProcCommand(sqlCommand);

        //    if (Agency != string.Empty)
        //    {
        //        SqlParameter agencyParm = new SqlParameter("@VER_AGENCY", Agency);
        //        dbCommand.Parameters.Add(agencyParm);
        //    }
        //    if (Dept != string.Empty)
        //    {
        //        SqlParameter deptParm = new SqlParameter("@VER_DEPT", Dept);
        //        dbCommand.Parameters.Add(deptParm);
        //    }
        //    if (Program != string.Empty)
        //    {
        //        SqlParameter programParm = new SqlParameter("@VER_PROGRAM", Program);
        //        dbCommand.Parameters.Add(programParm);
        //    }
        //    if (Year != string.Empty)
        //    {
        //        SqlParameter typeParm = new SqlParameter("@VER_YEAR", Year);
        //        dbCommand.Parameters.Add(typeParm);
        //    }
        //    if (AppNo != string.Empty)
        //    {
        //        SqlParameter typeParm = new SqlParameter("@VER_APP_NO", AppNo);
        //        dbCommand.Parameters.Add(typeParm);
        //    }
        //    if (VerifyDate != string.Empty)
        //    {
        //        SqlParameter typeParm = new SqlParameter("@VER_VERIFY_DATE", VerifyDate);
        //        dbCommand.Parameters.Add(typeParm);
        //    }
        //    // Get results.
        //    ds = db.ExecuteDataSet(dbCommand);
        //    return ds;
        //}

        //public static bool InsertUpdateDelCASEVer(List<SqlParameter> sqlParamList)
        //{
        //    _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelCASEVer");
        //    _dbCommand.CommandTimeout = 1200;
        //    _dbCommand.Parameters.Clear();
        //    foreach (SqlParameter sqlPar in sqlParamList)
        //    {
        //        _dbCommand.Parameters.Add(sqlPar);
        //    }
        //    return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        //}

        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //public static DataSet GetSelectServices()
        //{
        //    DataSet ds;
        //    Database db;
        //    string sqlCommand;
        //    DbCommand dbCommand;

        //    db = DatabaseFactory.CreateDatabase();
        //    sqlCommand = "[dbo].[GetServices]";
        //    dbCommand = db.GetStoredProcCommand(sqlCommand);

        //    // Get results.
        //    ds = db.ExecuteDataSet(dbCommand);
        //    return ds;
        //}

        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //public static DataSet GetSelectServicesByHIE(string Type, string Agency, string Dept, string Program, string Year, string AppNo)
        //{
        //    DataSet ds;
        //    Database db;
        //    string sqlCommand;
        //    DbCommand dbCommand;

        //    db = DatabaseFactory.CreateDatabase();
        //    sqlCommand = "[dbo].[GetServices]";
        //    dbCommand = db.GetStoredProcCommand(sqlCommand);

        //    SqlParameter typeParm = new SqlParameter("@Type", Type);
        //    dbCommand.Parameters.Add(typeParm);

        //    if (Agency != string.Empty)
        //    {
        //        SqlParameter agencyParm = new SqlParameter("@MSTSER_AGENCY", Agency);
        //        dbCommand.Parameters.Add(agencyParm);
        //    }
        //    if (Dept != string.Empty)
        //    {
        //        SqlParameter deptParm = new SqlParameter("@MSTSER_DEPT", Dept);
        //        dbCommand.Parameters.Add(deptParm);
        //    }
        //    if (Program != string.Empty)
        //    {
        //        SqlParameter programParm = new SqlParameter("@MSTSER_PROGRAM", Program);
        //        dbCommand.Parameters.Add(programParm);
        //    }
        //    if (Year != string.Empty)
        //    {
        //        SqlParameter yearParm = new SqlParameter("@MSTSER_YEAR", Year);
        //        dbCommand.Parameters.Add(yearParm);
        //    }
        //    if (AppNo != string.Empty)
        //    {
        //        SqlParameter appParm = new SqlParameter("@MSTSER_APP_NO", AppNo);
        //        dbCommand.Parameters.Add(appParm);
        //    }
        //    // Get results.
        //    ds = db.ExecuteDataSet(dbCommand);
        //    return ds;
        //}

        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //public static DataSet GetSiteByHIE(string agency, string dept, string program)
        //{
        //    DataSet ds;
        //    Database db;
        //    string sqlCommand;
        //    DbCommand dbCommand;

        //    db = DatabaseFactory.CreateDatabase();
        //    sqlCommand = "[dbo].[GetCaseSite]";
        //    dbCommand = db.GetStoredProcCommand(sqlCommand);

        //    SqlParameter type = new SqlParameter("@TYPE", "SITEBYHIE");
        //    dbCommand.Parameters.Add(type);
        //    if (agency != string.Empty)
        //    {
        //        SqlParameter sqlSiteNo = new SqlParameter("@AGENCY", agency);
        //        dbCommand.Parameters.Add(sqlSiteNo);
        //    }
        //    if (dept != string.Empty)
        //    {
        //        SqlParameter sqlSiteName = new SqlParameter("@DEPT", dept);
        //        dbCommand.Parameters.Add(sqlSiteName);
        //    }
        //    if (program != string.Empty)
        //    {
        //        SqlParameter sqlcity = new SqlParameter("@PROGRAM", program);
        //        dbCommand.Parameters.Add(sqlcity);
        //    }
        //    // Get results.
        //    ds = db.ExecuteDataSet(dbCommand);
        //    return ds;
        //}

        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //public static DataSet GetCaseMstSSNO()
        //{
        //    DataSet ds;
        //    Database db;
        //    string sqlCommand;
        //    DbCommand dbCommand;

        //    db = DatabaseFactory.CreateDatabase();
        //    sqlCommand = "[dbo].[GETCASEMSTSsno]";
        //    dbCommand = db.GetStoredProcCommand(sqlCommand);

        //    ds = db.ExecuteDataSet(dbCommand);
        //    return ds;
        //}

    }
}
