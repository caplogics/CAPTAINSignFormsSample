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
    public partial class UserAccess
    {
        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[PASSWORD]"; 
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;


        #endregion

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet UserSearch(string empNo, string firstName, string lastName, string status, string UserID, string AdminAgency)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[SearchUsers]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);


            if (empNo != string.Empty)
            {
                SqlParameter empNoParm = new SqlParameter("@EmployeeNo", empNo);
                dbCommand.Parameters.Add(empNoParm);
            }
            if (firstName != string.Empty)
            {
                SqlParameter firstNameParm = new SqlParameter("@FirstName", firstName);
                dbCommand.Parameters.Add(firstNameParm);
            }
            if (lastName != string.Empty)
            {
                SqlParameter lastNameparm = new SqlParameter("@LastName", lastName);
                dbCommand.Parameters.Add(lastNameparm);
            }
            if (status != "All")
            {
                SqlParameter statusParm = new SqlParameter("@isActive", status);
                dbCommand.Parameters.Add(statusParm);
            }

            if (UserID != string.Empty)
            {
                SqlParameter lastNameparm = new SqlParameter("@UserID", UserID);
                dbCommand.Parameters.Add(lastNameparm);
            }
            if (AdminAgency != string.Empty)
            {
                SqlParameter lastNameparm = new SqlParameter("@Admin_Agency", AdminAgency);
                dbCommand.Parameters.Add(lastNameparm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet AuthenticatePassword(string UserName, string Password,string LoginType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[AuthenticatePassword]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (UserName != string.Empty)
            {
                SqlParameter moduleCodeParm = new SqlParameter("@PWR_EMPLOYEE_NO", UserName);
                dbCommand.Parameters.Add(moduleCodeParm);
            }
            if (Password != string.Empty)
            {
                SqlParameter moduleCodeParm = new SqlParameter("@PWR_PASSWORD", Password);
                dbCommand.Parameters.Add(moduleCodeParm);
            }
            if (LoginType != string.Empty)
            {
                SqlParameter moduleCodeParm = new SqlParameter("@LoginType", LoginType);
                dbCommand.Parameters.Add(moduleCodeParm);
            }
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet FetchLoginUser(string UserName, string Email, string phonenumber, string Mode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[MFALOGIN]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (UserName != string.Empty)
            {
                SqlParameter moduleCodeParm = new SqlParameter("@PWR_EMPLOYEE_NO", UserName);
                dbCommand.Parameters.Add(moduleCodeParm);
            }
            if (Email != string.Empty)
            {
                SqlParameter moduleCodeParm = new SqlParameter("@PWR_EMAIL", Email);
                dbCommand.Parameters.Add(moduleCodeParm);
            }
            if (phonenumber != string.Empty)
            {
                SqlParameter moduleCodeParm = new SqlParameter("@PWR_MOBILE", phonenumber);
                dbCommand.Parameters.Add(moduleCodeParm);
            }
            if (Mode != string.Empty)
            {
                SqlParameter moduleCodeParm = new SqlParameter("@MODE", Mode);
                dbCommand.Parameters.Add(moduleCodeParm);
            }
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetUserProfileByID(string UserID, string TableName,string Hierachy)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetUserProfile]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (UserID != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PWR_EMPLOYEE_NO", UserID);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (TableName != string.Empty)
            {
                SqlParameter tableParm = new SqlParameter("@TABLE_NAME", TableName);
                dbCommand.Parameters.Add(tableParm);
            }
            if (Hierachy != string.Empty)
            {
                SqlParameter tableParm = new SqlParameter("@Hierachy", Hierachy);
                dbCommand.Parameters.Add(tableParm);
            }
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetClientUserByID(string UserID)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCLINQHIE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (UserID != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLINQ_USER_ID", UserID);
                dbCommand.Parameters.Add(empnoParm);
            }
            //if (TableName != string.Empty)
            //{
            //    SqlParameter tableParm = new SqlParameter("@TABLE_NAME", TableName);
            //    dbCommand.Parameters.Add(tableParm);
            //}
            //if (Hierachy != string.Empty)
            //{
            //    SqlParameter tableParm = new SqlParameter("@Hierachy", Hierachy);
            //    dbCommand.Parameters.Add(tableParm);
            //}
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CheckCaseworkerByID(string Caseworker)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CheckCaseworker]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Caseworker != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CaseWorker", Caseworker);
                dbCommand.Parameters.Add(empnoParm);
            }
            
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetApplicationsByUserID(string userID, string HIE)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetApplicationsByUserID]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

            if (userID != string.Empty && userID != null)
            {
                SqlParameter moduleCodeParm = new SqlParameter("@UserID", userID);
                dbCommand.Parameters.Add(moduleCodeParm);
            }
            if (HIE != string.Empty && HIE != null)
            {
                SqlParameter HIEParm = new SqlParameter("@HIE", HIE);
                dbCommand.Parameters.Add(HIEParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertUpdatePASSWORD(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertPASSWORD");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertUpdatePASSWORDHIE(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertPASSWORDHIE");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertUpdatePASSWORDSERHIE(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertPASSWORDSERHIE");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool INSERTUPDATEDELTAKEACTIONSHLD(List<SqlParameter> sqlParamList)
        {
            bool boolstatus = false;
            try
            {
                _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSERTUPDATEDELTAKEACTIONSHLD");
                _dbCommand.CommandTimeout = 1200;
                _dbCommand.Parameters.Clear();
                foreach (SqlParameter sqlPar in sqlParamList)
                {
                    _dbCommand.Parameters.Add(sqlPar);
                }
                 boolstatus = _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
            }
            catch (Exception ex)
            {
               
            }
            return boolstatus;
        }

        public static bool InsertUpdateCLINQHIE(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertCLINQHIE");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertUpdateEMPLFUNC(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertEMPLFUNC");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertUpdateBATCNTL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertBATCNTL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool UpdatePASSWORD(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.ChangePassword");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool CheckDefaultHierachy(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CheckDefaultHierchy");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static string GetHierachyDesc(string Type,string Agency,string Dept,string Prog)
        {
            DataSet ds=null;
            string strKeyName = string.Empty;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetHierarchy_Desc]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Type != string.Empty && Type != null)
            {
                SqlParameter moduleCodeParm = new SqlParameter("@Type", Type);
                dbCommand.Parameters.Add(moduleCodeParm);
            }
            if (Agency != string.Empty && Agency != null)
            {
                SqlParameter moduleCodeParm = new SqlParameter("@Agy", Agency);
                dbCommand.Parameters.Add(moduleCodeParm);
            }
            if (Dept != string.Empty && Dept != null)
            {
                SqlParameter moduleCodeParm = new SqlParameter("@Dept", Dept);
                dbCommand.Parameters.Add(moduleCodeParm);
            }
            if (Prog != string.Empty && Prog != null)
            {
                SqlParameter moduleCodeParm = new SqlParameter("@Prog", Prog);
                dbCommand.Parameters.Add(moduleCodeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
                strKeyName = ds.Tables[0].Rows[0][0].ToString();
            return strKeyName.Trim();
        }


        public static bool InsertUpdateLogUsers(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateLogUsers");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMailSetting(string strType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "SELECT * FROM EMAILSETTINGS WHERE MAIL_PURPOSE = '" + strType + "'";
            dbCommand = db.GetSqlStringCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;
            
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

    }
}
