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
    public partial class HierarchyPlusProgram
    {
        #region Constants

        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;

        #endregion

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetHierarchy()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetHierarchies]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);            
            
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }
        public static DataSet GetProgramsSearch(string programName, string agency)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetPrograms]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            SqlParameter filterParm = new SqlParameter("@FilterType", "SEARCH");
            dbCommand.Parameters.Add(filterParm);

            if (!agency.Equals(string.Empty))
            {
                SqlParameter programParm = new SqlParameter("@ProgramHIE", agency);
                dbCommand.Parameters.Add(programParm);
            }

            if (!programName.Equals(string.Empty))
            {
                SqlParameter programNameParm = new SqlParameter("@ProgramName", programName);
                dbCommand.Parameters.Add(programNameParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }
        public static bool InsertUpdateCASEHIE(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateCASEHIE");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static DataSet GetPrograms()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetPrograms]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            SqlParameter filterParm = new SqlParameter("@FilterType", "Default");
            dbCommand.Parameters.Add(filterParm);

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static DataSet GetProgramsSearch(string programName, string agency,string UserID,string AdminAgency)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetPrograms]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            SqlParameter filterParm = new SqlParameter("@FilterType", "SEARCH");
            dbCommand.Parameters.Add(filterParm);

            if (!agency.Equals(string.Empty))
            {
                SqlParameter programParm = new SqlParameter("@ProgramHIE", agency);
                dbCommand.Parameters.Add(programParm);
            }

            if (!programName.Equals(string.Empty))
            {
                SqlParameter programNameParm = new SqlParameter("@ProgramName", programName);
                dbCommand.Parameters.Add(programNameParm);
            }

            if (!UserID.Equals(string.Empty))
            {
                SqlParameter programNameParm = new SqlParameter("@UserID", UserID);
                dbCommand.Parameters.Add(programNameParm);
            }

            if (!AdminAgency.Equals(string.Empty))
            {
                SqlParameter programNameParm = new SqlParameter("@Admin_Agency", AdminAgency);
                dbCommand.Parameters.Add(programNameParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertUpdateCASEDEP(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateCASEDEP");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool DeleteCASEHIE(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteCASEHIE");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCASEDEP()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCASEDEP]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCASEDEPadp(string Agency, string Dept, string Program)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCASEDEPadp]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@DEP_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@DEP_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@DEP_PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetSerplanHiesadp(string UserId,string Agency, string Dept, string Program)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_ServicePlanHies]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (UserId != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@PWSH_EMPLOYEE_NO", UserId);
                dbCommand.Parameters.Add(agencyParm);
            }

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@PROG", Program);
                dbCommand.Parameters.Add(programParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCASEDEPContacts(string Agency, string Dept, string Program)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCASEDEPCONT]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@DEPCONT_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@DEPCONT_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@DEPCONT_PROG", Program);
                dbCommand.Parameters.Add(programParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertCASEDEP(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertCASEDEP");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertDEPCONT(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertCASEDEPContact");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool DeleteCASEDEP(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteCASEDEP");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool DeleteDEPCONT(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteCASEDEPCONT");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertCASEDEPEnrollHierachies(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelDEPNRLHIE");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }
  
    }
}

