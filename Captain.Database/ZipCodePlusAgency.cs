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
    public partial class ZipCodePlusAgency
    {
        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[ZIPCODE]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;


        #endregion

        /// <summary>
        /// Get ZipCode All Details. 
        /// </summary>
        /// <returns></returns>
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetZipCode()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetZIPCODE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        /// <summary>
        /// Get TownShip Information.
        /// </summary>
        /// <returns></returns>

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetTownship()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetTownship]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCounty()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCounty]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCity(string strzip, string strcity, string strcounty, string Type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[SUR_GET_ZIPCODE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (strzip != string.Empty)
            {
                SqlParameter QuesIdParm = new SqlParameter("@Zcrzip", strzip);
                dbCommand.Parameters.Add(QuesIdParm);
            }
            if (strcity != string.Empty)
            {
                SqlParameter QuesIdParm = new SqlParameter("@Zcrcity", strcity);
                dbCommand.Parameters.Add(QuesIdParm);
            }
            if (strcounty != string.Empty)
            {
                SqlParameter TypeParm = new SqlParameter("@Zcrcounty", strcounty);
                dbCommand.Parameters.Add(TypeParm);
            }
            if (Type != string.Empty)
            {
                SqlParameter TypeParm = new SqlParameter("@Type", Type);
                dbCommand.Parameters.Add(TypeParm);
            }


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetZipCodeByID(string ZcrZip, string ZipPlus)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetZIPCODEByZcrzip]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (ZcrZip != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ZCR_ZIP", ZcrZip);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (ZipPlus != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ZCRPLUS_4", ZipPlus);
                dbCommand.Parameters.Add(empnoParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ZipCodeSearch(string zcrZip, string zcrCity, string zcrCityCode, string zcrCounty)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[ZipCodeSearch]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);


            if (zcrZip != string.Empty)
            {
                SqlParameter empNoParm = new SqlParameter("@Zcrzip", zcrZip);
                dbCommand.Parameters.Add(empNoParm);
            }
            if (zcrCity != string.Empty)
            {
                SqlParameter firstNameParm = new SqlParameter("@Zcrcity", zcrCity);
                dbCommand.Parameters.Add(firstNameParm);
            }
            if (zcrCityCode != string.Empty)
            {
                SqlParameter lastNameparm = new SqlParameter("@Zcrcitycode", zcrCityCode);
                dbCommand.Parameters.Add(lastNameparm);
            }
            if (zcrCounty != string.Empty)
            {
                SqlParameter statusParm = new SqlParameter("@Zcrcounty", zcrCounty);
                dbCommand.Parameters.Add(statusParm);
            }
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertUpdateDelZIPCODE(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelZIPCODE");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCaseType()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCaseType]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static DataSet GetAgencyControlDetails(string agencyCode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetAgencyControlFile]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agencyCode != string.Empty)
            {
                SqlParameter statusParm = new SqlParameter("@AgencyCode", agencyCode);
                dbCommand.Parameters.Add(statusParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertUpdateAGCYCNTL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateAGCYCNTL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool DeleteAGCYCNTL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteAGCYCNTL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool UpdateXMLAGCYCNTL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateXMLAGCYCNTL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool ADMN0014_UPD_AGCYCNTL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.ADMN0014_UPD_AGCYCNTL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool ADD_PIP_PRIVS()
        {
            bool boolstatus = false;
            try
            {
                _dbCommand = _dbFactory.GetStoredProcCommand("dbo.ADD_PIP_PRIVS");
                _dbCommand.CommandTimeout = 1200;
                _dbCommand.Parameters.Clear();

                boolstatus = _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;

            }
            catch (Exception ex)
            {

            }
            return boolstatus;
        }



        /// <summary>
        /// Get Towns.
        /// </summary>
        /// <returns></returns>

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetTowns()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetTowns]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetLihpmpQues(string agytype, string strYear)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETLIHPMQUES]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);


            if (agytype != string.Empty)
            {
                SqlParameter empNoParm = new SqlParameter("@AgysType", agytype);
                dbCommand.Parameters.Add(empNoParm);
            }
            if (strYear != string.Empty)
            {
                SqlParameter empNoParm = new SqlParameter("@LPMQ_YEAR", strYear);
                dbCommand.Parameters.Add(empNoParm);
            }


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

    }
}

