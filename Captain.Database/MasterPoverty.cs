using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.ComponentModel;
using System.Data;

namespace Captain.DatabaseLayer
{
    
    [DataObject]
    [Serializable]
    public partial class MasterPoverty
    {
        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[CASEGDL]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;


        #endregion

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCASEGDL()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCASEGDL]"; 
            dbCommand = db.GetStoredProcCommand(sqlCommand);                       

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCASEGDLadpt(string Agency,string Dept,string Program,string Type,string County)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCASEGDLadpt]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@GDL_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@GDL_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@GDL_PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Type != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@GDL_TYPE", Type);
                dbCommand.Parameters.Add(typeParm);
            }
            if (County != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@GDL_County", County);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCASEGDLSubadpt(string Agency, string Dept, string Program, string Type, string County,string StartDate,string EndDate,string strType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCASEGDLSubadpt]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

           
                SqlParameter agencyParm = new SqlParameter("@GDL_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@GDL_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@GDL_PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Type != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@GDL_TYPE", Type);
                dbCommand.Parameters.Add(typeParm);
            }
            if (County != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@GDL_County", County);
                dbCommand.Parameters.Add(typeParm);
            }
            if (StartDate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@GDL_START_DATE", StartDate);
                dbCommand.Parameters.Add(typeParm);
            }
            if (EndDate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@GDL_END_DATE", EndDate);
                dbCommand.Parameters.Add(typeParm);
            }
            if (strType != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCASEGDLByHIE(string Agency, string Dept, string Program)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCASEGDLByHIE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@GDL_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@GDL_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@GDL_PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

       
        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //public static DataSet GetCaseGdlByID(string UserID)
        //{
        //    DataSet ds;
        //    Database db;
        //    string sqlCommand;
        //    DbCommand dbCommand;

        //    db = DatabaseFactory.CreateDatabase();
        //    sqlCommand = "[dbo].[GetCaseGdlByZcrzip]";
        //    dbCommand = db.GetStoredProcCommand(sqlCommand);

        //    if (UserID != string.Empty)
        //    {
        //        SqlParameter empnoParm = new SqlParameter("@ZCR_ZIP", UserID);
        //        dbCommand.Parameters.Add(empnoParm);
        //    }

        //    // Get results.
        //    ds = db.ExecuteDataSet(dbCommand);
        //    return ds;
        //}

        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //public static DataSet CaseGdlSearch(string zcrZip, string zcrCity, string zcrCityCode, string zcrCounty)
        //{
        //    DataSet ds;
        //    Database db;
        //    string sqlCommand;
        //    DbCommand dbCommand;

        //    db = DatabaseFactory.CreateDatabase();
        //    sqlCommand = "[dbo].[CaseGdlSearch]";
        //    dbCommand = db.GetStoredProcCommand(sqlCommand);


        //    if (zcrZip != string.Empty)
        //    {
        //        SqlParameter empNoParm = new SqlParameter("@Zcrzip", zcrZip);
        //        dbCommand.Parameters.Add(empNoParm);
        //    }
        //    if (zcrCity != string.Empty)
        //    {
        //        SqlParameter firstNameParm = new SqlParameter("@Zcrcity", zcrCity);
        //        dbCommand.Parameters.Add(firstNameParm);
        //    }
        //    if (zcrCityCode != string.Empty)
        //    {
        //        SqlParameter lastNameparm = new SqlParameter("@Zcrcitycode", zcrCityCode);
        //        dbCommand.Parameters.Add(lastNameparm);
        //    }
        //    if (zcrCounty != string.Empty)
        //    {
        //        SqlParameter statusParm = new SqlParameter("@Zcrcounty", zcrCounty);
        //        dbCommand.Parameters.Add(statusParm);
        //    }
        //    // Get results.
        //    ds = db.ExecuteDataSet(dbCommand);
        //    return ds;
        //}

        public static bool InsertCASEGDL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertCASEGDL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool UpdateCASEGDL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateCASEGDL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool DeleteCASEGDL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteCASEGDL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CASEGDLDateExists(string Agency, string Dept, string Program, string Type, string StartDate, string EndDate,string County)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CaseGdlDateExist]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@GDL_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@GDL_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@GDL_PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Type != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@GDL_TYPE", Type);
                dbCommand.Parameters.Add(typeParm);
            }
            if (StartDate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@NewStartdate", StartDate);
                dbCommand.Parameters.Add(typeParm);
            }
            if (EndDate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@NewEndDate", EndDate);
                dbCommand.Parameters.Add(typeParm);
            }
            
            if (County != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@GDL_COUNTY", County);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetFederalOmbChart(string Agency, string Dept, string Program, string Type, string strDate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCASEGDLByDate]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@GDL_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@GDL_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@GDL_PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@GDL_TYPE", Type);
                dbCommand.Parameters.Add(programParm);
            }
            if (strDate != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@VerifyDate", strDate);
                dbCommand.Parameters.Add(agencyParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertUpdateDelPovertyExp(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelPovertyEXP");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetPovertyException(string Agency, string Dept, string Program,string StartDate, string EndDate,string strVerifyDate,string strMode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetPovertyExp]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@EXP_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@EXP_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@EXP_PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }
          
            if (StartDate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@EXP_START_DATE", StartDate);
                dbCommand.Parameters.Add(typeParm);
            }
            if (EndDate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@EXP_END_DATE", EndDate);
                dbCommand.Parameters.Add(typeParm);
            }
            if (strVerifyDate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@VerifyDate", strVerifyDate);
                dbCommand.Parameters.Add(typeParm);
            }
            if (strMode != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Mode", strMode);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GET_CASEGDL(string Agency, string Dept, string Program, string Type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_CASEGDL]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@GDL_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@GDL_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@GDL_PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }

            if (Type != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@GDL_TYPE", Type);
                dbCommand.Parameters.Add(typeParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }
    }
}
