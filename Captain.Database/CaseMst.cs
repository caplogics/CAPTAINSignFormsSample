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
    public partial class CaseMst
    {
        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[CASEMST]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;


        #endregion

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCASEMST()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCASEMST]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCASEMSTadpyn(string Agency, string Dept, string Program,string Year, string AppNo)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCASEMSTadpyn]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@MST_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@MST_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@MST_PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@MST_YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (AppNo != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@MST_APP_NO", AppNo);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetMstSnpCase2530Report(string Agency, string Dept, string Program, string Year, string AppNo,string Securty,string Orderby,string Type,string CaseType, string Fromdate,string Todate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetMstSnpCase2530Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1200;

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
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (AppNo != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@APP_NO", AppNo);
                dbCommand.Parameters.Add(typeParm);
            }
            if (Securty != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Security", Securty);
                dbCommand.Parameters.Add(typeParm);
            }
            if (Orderby != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@OrderBy", Orderby);
                dbCommand.Parameters.Add(typeParm);
            }
            if (Type != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Type", Type);
                dbCommand.Parameters.Add(typeParm);
            }
            if (CaseType != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@CaseType", CaseType);
                dbCommand.Parameters.Add(typeParm);
            }
            if (Fromdate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@From", Fromdate);
                dbCommand.Parameters.Add(typeParm);
            }
            if (Todate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@To", Todate);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCASESNP()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCASESNP]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static long GetCASEMSTMaxApplNo(string Agency, string Dept, string Program, string Year)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCASEMSTMAXNO]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@MST_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@MST_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@MST_PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@MST_YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return Convert.ToInt64(ds.Tables[0].Rows[0]["MST_APP_NO"]);
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetSITEDESC(string Agency, string Site)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Get_Site_Desc]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Site != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@SiteCd", Site);
                dbCommand.Parameters.Add(deptParm);
            }
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCASESNPadpyn(string Agency, string Dept, string Program, string Year, string AppNo)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCASESNPadpyn]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@SNP_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@SNP_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@SNP_PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@SNP_YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (AppNo != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@SNP_APP", AppNo);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCASESNPMSTadpyn(string Agency, string Dept, string Program, string Year, string AppNo)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCASESNPMSTadpyn]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@SNP_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@SNP_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@SNP_PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@SNP_YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (AppNo != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@SNP_APP", AppNo);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCASESNPadpyncase2530Report(string Agency, string Dept, string Program, string Year, string AppNo, string strApplicatDetails)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCaseSnpcase2530Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@SNP_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@SNP_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@SNP_PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@SNP_YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (AppNo != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@SNP_APP", AppNo);
                dbCommand.Parameters.Add(typeParm);
            }
            if (strApplicatDetails != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@ApplicantXml", strApplicatDetails);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCASEINCOME(string Agency, string Dept, string Program, string Year, string AppNo)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_CASEINCOME]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 3600;

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@INCOME_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@INCOME_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@INCOME_PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@INCOME_YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (AppNo != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@INCOME_APP", AppNo);
                dbCommand.Parameters.Add(typeParm);
            }
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCaseWorker(string Type,string Agency,string Dept,string Program)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetPasswordhie]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            SqlParameter typeParm = new SqlParameter("@Type", Type);
            dbCommand.Parameters.Add(typeParm);
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
        public static DataSet GetMembers(string agency, string dept, string program,string Year) //Browse_CASESITE
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Get_Members]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter sqlSiteNo = new SqlParameter("@AGENCY", agency);
                dbCommand.Parameters.Add(sqlSiteNo);
            }
            if (dept != string.Empty)
            {
                SqlParameter sqlSiteName = new SqlParameter("@DEPT", dept);
                dbCommand.Parameters.Add(sqlSiteName);
            }
            if (program != string.Empty)
            {
                SqlParameter sqlcity = new SqlParameter("@PROG", program);
                dbCommand.Parameters.Add(sqlcity);
            }

            if (Year != string.Empty)
            {
                SqlParameter sqlyear = new SqlParameter("@YEAR", Year);
                dbCommand.Parameters.Add(sqlyear);
            }
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        public static DataSet GetMembers_TMSB4003(string agency, string dept, string program, string Year) //Browse_CASESITE
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Get_Members_TMSB4003]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter sqlSiteNo = new SqlParameter("@AGENCY", agency);
                dbCommand.Parameters.Add(sqlSiteNo);
            }
            if (dept != string.Empty)
            {
                SqlParameter sqlSiteName = new SqlParameter("@DEPT", dept);
                dbCommand.Parameters.Add(sqlSiteName);
            }
            if (program != string.Empty)
            {
                SqlParameter sqlcity = new SqlParameter("@PROG", program);
                dbCommand.Parameters.Add(sqlcity);
            }

            if (Year != string.Empty)
            {
                SqlParameter sqlyear = new SqlParameter("@YEAR", Year);
                dbCommand.Parameters.Add(sqlyear);
            }
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetAllCaseWorkers(string Agency)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Get_All_Caseworkers]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            //if (Dept != string.Empty)
            //{
            //    SqlParameter deptParm = new SqlParameter("@DEPT", Dept);
            //    dbCommand.Parameters.Add(deptParm);
            //}
            //if (Program != string.Empty)
            //{
            //    SqlParameter programParm = new SqlParameter("@PROG", Program);
            //    dbCommand.Parameters.Add(programParm);
            //}

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCASEINCOMEadpynf(string Agency, string Dept, string Program, string Year, string AppNo, string FamilySeqNo)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCASEINCOMEadpynf]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@INCOME_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@INCOME_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@INCOME_PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@INCOME_YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (AppNo != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@INCOME_APP", AppNo);
                dbCommand.Parameters.Add(typeParm);
            }
            if (FamilySeqNo != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@INCOME_FAMILY_SEQ", FamilySeqNo);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCASEINCOMEadpynfs(string Agency, string Dept, string Program, string Year, string AppNo, string FamilySeqNo,string Seq)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCASEINCOMEadpynfs]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@INCOME_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@INCOME_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@INCOME_PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@INCOME_YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (AppNo != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@INCOME_APP", AppNo);
                dbCommand.Parameters.Add(typeParm);
            }
            if (FamilySeqNo != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@INCOME_FAMILY_SEQ", FamilySeqNo);
                dbCommand.Parameters.Add(typeParm);
            }

            if (Seq != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@INCOME_SEQ", Seq);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CAPS_ADMNB002_GET(string Type, string Agency, string Dept, string Program, string strUserId)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_ADMNB002_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            SqlParameter typeParm = new SqlParameter("@Type", Type);
            dbCommand.Parameters.Add(typeParm);
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
            if (strUserId != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@USERID", strUserId);
                dbCommand.Parameters.Add(programParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }
        public static bool InsertErrorLog(string strScreen, string strErrorDetails, string strMsg,string strUserId)
        {
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@ErrorScreen", strScreen));
                sqlParamList.Add(new SqlParameter("@ErrorDetails", strErrorDetails));
                sqlParamList.Add(new SqlParameter("@ErrorMsg", strMsg));
                sqlParamList.Add(new SqlParameter("@UserId", strUserId));                

                InsertErrorLog(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return true;
        }


        public static bool InsertUpdateCASEMST(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSERTUpdateCaseMST");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertUpdateCASEMSTLeanIntake(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSERTUpdateCaseMSTPIPIntake");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }



        public static bool INSERTCASEMSTLOG(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSERTCASEMSTLOG");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        public static bool INSERTUPDATEFIXSNPAUDIT(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSERTUPDATEFIXSNPAUDIT");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        public static bool UpdateCASEMSTSNPINCOME(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateCASEMSTSNPINCOME");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }
        

        public static bool InsertErrorLog(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertErrorLog");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool UpdateCASEMST(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateCASEMST");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        public static bool UpdateMstPositions(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateMstPositions");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        public static bool InsertCASESNP(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertCASESNP");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool UpdateCASESNP(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateCASESNP");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        public static bool InsertCASEINCOME(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertCASEINCOME");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool UpdateCASEINCOME(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateCASEINCOME");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

         public static bool DeleteCASEINCOME(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteCASEINCOME");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetCASEDiffadpya(string Agency, string Dept, string Program, string Year, string AppNo)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetCASEDiffadpya]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (Agency != string.Empty)
             {
                 SqlParameter agencyParm = new SqlParameter("@DIFF_AGENCY", Agency);
                 dbCommand.Parameters.Add(agencyParm);
             }
             if (Dept != string.Empty)
             {
                 SqlParameter deptParm = new SqlParameter("@DIFF_DEPT", Dept);
                 dbCommand.Parameters.Add(deptParm);
             }
             if (Program != string.Empty)
             {
                 SqlParameter programParm = new SqlParameter("@DIFF_PROGRAM", Program);
                 dbCommand.Parameters.Add(programParm);
             }
             if (Year != string.Empty)
             {
                 SqlParameter typeParm = new SqlParameter("@DIFF_YEAR", Year);
                 dbCommand.Parameters.Add(typeParm);
             }
             if (AppNo != string.Empty)
             {
                 SqlParameter typeParm = new SqlParameter("@DIFF_APP_NO", AppNo);
                 dbCommand.Parameters.Add(typeParm);
             }
             // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }


         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetLandlordadpya(string Agency, string Dept, string Program, string Year, string AppNo)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetLANDLORDadpya]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (Agency != string.Empty)
             {
                 SqlParameter agencyParm = new SqlParameter("@LLR_AGENCY", Agency);
                 dbCommand.Parameters.Add(agencyParm);
             }
             if (Dept != string.Empty)
             {
                 SqlParameter deptParm = new SqlParameter("@LLR_DEPT", Dept);
                 dbCommand.Parameters.Add(deptParm);
             }
             if (Program != string.Empty)
             {
                 SqlParameter programParm = new SqlParameter("@LLR_PROGRAM", Program);
                 dbCommand.Parameters.Add(programParm);
             }
             if (Year != string.Empty)
             {
                 SqlParameter typeParm = new SqlParameter("@LLR_YEAR", Year);
                 dbCommand.Parameters.Add(typeParm);
             }
             if (AppNo != string.Empty)
             {
                 SqlParameter typeParm = new SqlParameter("@LLR_APP_NO", AppNo);
                 dbCommand.Parameters.Add(typeParm);
             }
             // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         public static bool InsertUpdateDelCASEDiff(List<SqlParameter> sqlParamList)
         {
             _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelCASEDIFF");
             _dbCommand.CommandTimeout = 1200;
             _dbCommand.Parameters.Clear();
             foreach (SqlParameter sqlPar in sqlParamList)
             {
                 _dbCommand.Parameters.Add(sqlPar);
             }
             return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
         }

         public static bool InsertUpdateDelLandlord(List<SqlParameter> sqlParamList)
         {
             _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelLANDLORD");
             _dbCommand.CommandTimeout = 1200;
             _dbCommand.Parameters.Clear();
             foreach (SqlParameter sqlPar in sqlParamList)
             {
                 _dbCommand.Parameters.Add(sqlPar);
             }
             return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
         }

         public static bool InsertUpdateDelMSTSER(List<SqlParameter> sqlParamList)
         {
             _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertMSTSER");
             _dbCommand.CommandTimeout = 1200;
             _dbCommand.Parameters.Clear();
             foreach (SqlParameter sqlPar in sqlParamList)
             {
                 _dbCommand.Parameters.Add(sqlPar);
             }
             return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetCASEVeradpynd(string Agency, string Dept, string Program, string Year, string AppNo, string VerifyDate)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetCASEVeradpynd]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (Agency != string.Empty)
             {
                 SqlParameter agencyParm = new SqlParameter("@VER_AGENCY", Agency);
                 dbCommand.Parameters.Add(agencyParm);
             }
             if (Dept != string.Empty)
             {
                 SqlParameter deptParm = new SqlParameter("@VER_DEPT", Dept);
                 dbCommand.Parameters.Add(deptParm);
             }
             if (Program != string.Empty)
             {
                 SqlParameter programParm = new SqlParameter("@VER_PROGRAM", Program);
                 dbCommand.Parameters.Add(programParm);
             }
             if (Year != string.Empty)
             {
                 SqlParameter typeParm = new SqlParameter("@VER_YEAR", Year);
                 dbCommand.Parameters.Add(typeParm);
             }
             if (AppNo != string.Empty)
             {
                 SqlParameter typeParm = new SqlParameter("@VER_APP_NO", AppNo);
                 dbCommand.Parameters.Add(typeParm);
             }
             if (VerifyDate != string.Empty)
             {
                 SqlParameter typeParm = new SqlParameter("@VER_VERIFY_DATE", VerifyDate);
                 dbCommand.Parameters.Add(typeParm);
             }
             // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         public static bool InsertUpdateDelCASEVer(List<SqlParameter> sqlParamList)
         {
             _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelCASEVer");
             _dbCommand.CommandTimeout = 1200;
             _dbCommand.Parameters.Clear();
             foreach (SqlParameter sqlPar in sqlParamList)
             {
                 _dbCommand.Parameters.Add(sqlPar);
             }
             return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetSelectServices()
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetServices]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetSelectServicesByHIE(string Type,string Agency, string Dept, string Program, string Year, string AppNo)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetServices]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             SqlParameter typeParm = new SqlParameter("@Type", Type);
             dbCommand.Parameters.Add(typeParm);

             if (Agency != string.Empty)
             {
                 SqlParameter agencyParm = new SqlParameter("@MSTSER_AGENCY", Agency);
                 dbCommand.Parameters.Add(agencyParm);
             }
             if (Dept != string.Empty)
             {
                 SqlParameter deptParm = new SqlParameter("@MSTSER_DEPT", Dept);
                 dbCommand.Parameters.Add(deptParm);
             }
             if (Program != string.Empty)
             {
                 SqlParameter programParm = new SqlParameter("@MSTSER_PROGRAM", Program);
                 dbCommand.Parameters.Add(programParm);
             }
             if (Year != string.Empty)
             {
                 SqlParameter yearParm = new SqlParameter("@MSTSER_YEAR", Year);
                 dbCommand.Parameters.Add(yearParm);
             }
             if (AppNo != string.Empty)
             {
                 SqlParameter appParm = new SqlParameter("@MSTSER_APP_NO", AppNo);
                 dbCommand.Parameters.Add(appParm);
             }
             // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetSiteByHIE(string agency, string dept, string program) //Browse_CASESITE
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetCaseSite]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (agency != string.Empty)
             {
                 SqlParameter type = new SqlParameter("@TYPE", "SITEBYHIE");
                 dbCommand.Parameters.Add(type);               
             }
             else
             {
                 SqlParameter type = new SqlParameter("@TYPE", "SITE");
                 dbCommand.Parameters.Add(type);
             }
             
             if (agency != string.Empty)
             {
                agency= (agency == "**" ? null : agency);

                 SqlParameter sqlSiteNo = new SqlParameter("@AGENCY", agency);
                 dbCommand.Parameters.Add(sqlSiteNo);
             }
             if (dept != string.Empty)
             {
                 SqlParameter sqlSiteName = new SqlParameter("@DEPT", dept);
                 dbCommand.Parameters.Add(sqlSiteName);
             }
             if (program != string.Empty)
             {
                 SqlParameter sqlcity = new SqlParameter("@PROGRAM", program);
                 dbCommand.Parameters.Add(sqlcity);
             }
             // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

        public static DataSet GetSiteByHIEYEAR(string agency, string dept, string program,string Year) 
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCaseSite]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter type = new SqlParameter("@TYPE", "SITEBYHIEYEAR");
                dbCommand.Parameters.Add(type);
            }
            else
            {
                SqlParameter type = new SqlParameter("@TYPE", "SITE");
                dbCommand.Parameters.Add(type);
            }

            if (agency != string.Empty)
            {
                agency = (agency == "**" ? null : agency);

                SqlParameter sqlSiteNo = new SqlParameter("@AGENCY", agency);
                dbCommand.Parameters.Add(sqlSiteNo);
            }
            if (dept != string.Empty)
            {
                SqlParameter sqlSiteName = new SqlParameter("@DEPT", dept);
                dbCommand.Parameters.Add(sqlSiteName);
            }
            if (program != string.Empty)
            {
                SqlParameter sqlcity = new SqlParameter("@PROGRAM", program);
                dbCommand.Parameters.Add(sqlcity);
            }
            if (Year != null)
            {
                if (Year.Trim() != string.Empty)
                {

                    SqlParameter sqlSiteNo = new SqlParameter("@YEAR", Year);
                    dbCommand.Parameters.Add(sqlSiteNo);
                }
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetSiteall() //Browse_CASESITE
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetCaseSiteall]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);
            // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }


         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet BrowseCASESITE(string agency, string SiteNumber, string SiteRoom) //Browse_CASESITE
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[Browse_CASESITE]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (agency != string.Empty)
             {
                 SqlParameter sqlSiteNo = new SqlParameter("@SITE_AGENCY", agency);
                 dbCommand.Parameters.Add(sqlSiteNo);
             }
             if (SiteNumber != string.Empty)
             {
                 SqlParameter sqlSiteName = new SqlParameter("@SITE_NUMBER", SiteNumber);
                 dbCommand.Parameters.Add(sqlSiteName);
             }
             if (SiteRoom != string.Empty)
             {
                 SqlParameter sqlcity = new SqlParameter("@SITE_ROOM", SiteRoom);
                 dbCommand.Parameters.Add(sqlcity);
             }
             // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet Browse_Selected_Table(List<SqlParameter> sqlParamList, string Table_Name)
         {
             DataSet ds;
             Database db;
             //string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             //sqlCommand = "[dbo].[Browse_CASESPM]";
             dbCommand = db.GetStoredProcCommand(Table_Name);

             foreach (SqlParameter sqlPar in sqlParamList)
                 dbCommand.Parameters.Add(sqlPar);

             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         public static bool Update_Sel_Table(List<SqlParameter> sqlParamList, string SP_Name, out string Sql_Reslut_Msg)
         {
             _dbCommand = _dbFactory.GetStoredProcCommand(SP_Name);
             _dbCommand.CommandTimeout = 1200;
             _dbCommand.Parameters.Clear();

             SqlParameter SP_Result_Msg = new SqlParameter("@SP_Result_Msg", SqlDbType.VarChar, 8000);
             SP_Result_Msg.Direction = ParameterDirection.Output;
             sqlParamList.Add(SP_Result_Msg);

             foreach (SqlParameter sqlPar in sqlParamList)
             {
                 _dbCommand.Parameters.Add(sqlPar);
             }

             bool SQL_Response = false;

             SQL_Response = _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
             Sql_Reslut_Msg = SP_Result_Msg.Value.ToString();

             return SQL_Response;
         }


         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet Browse_MSTSER(string agency, string Dept, string Prog,string Year,string AppNo) //Browse_CASESITE
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[Browse_MSTSER]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (agency != string.Empty)
             {
                 SqlParameter sqlSiteNo = new SqlParameter("@MSTSER_AGENCY", agency);
                 dbCommand.Parameters.Add(sqlSiteNo);
             }
             if (Dept != string.Empty)
             {
                 SqlParameter sqlSiteName = new SqlParameter("@MSTSER_DEPT", Dept);
                 dbCommand.Parameters.Add(sqlSiteName);
             }
             if (Prog != string.Empty)
             {
                 SqlParameter sqlcity = new SqlParameter("@MSTSER_PROGRAM", Prog);
                 dbCommand.Parameters.Add(sqlcity);
             }

             if (Year != string.Empty)
             {
                 SqlParameter sqlyear = new SqlParameter("@MSTSER_YEAR", Year);
                 dbCommand.Parameters.Add(sqlyear);
             }
             if (AppNo != string.Empty)
             {
                 SqlParameter sqlAppno = new SqlParameter("@MSTSER_APP_NO", AppNo);
                 dbCommand.Parameters.Add(sqlAppno);
             }
             // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }



         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetCaseMstSSNO(string strSSNNO, string DOB, string FName, string ProdupSw)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GETCASEMSTSsno]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);
             if (strSSNNO != string.Empty)
             {
                 SqlParameter sqltmp = new SqlParameter("@SNP_SSNO", strSSNNO);
                 dbCommand.Parameters.Add(sqltmp);
             }

            if (DOB != string.Empty)
            {
                SqlParameter sqltmp = new SqlParameter("@SNP_DOB", DOB);
                dbCommand.Parameters.Add(sqltmp);
            }

            if (FName != string.Empty)
            {
                SqlParameter sqltmp = new SqlParameter("@SNPFNAME", FName);
                dbCommand.Parameters.Add(sqltmp);
            }

            if (ProdupSw != string.Empty)
            {
                SqlParameter sqltmp = new SqlParameter("@ProDupSw", ProdupSw);
                dbCommand.Parameters.Add(sqltmp);
            }
            ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetCaseSiteHie(string Year,string UserID,string AdminAgency)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[CASESITE_DiffHieByYear]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);
             if (Year != string.Empty)
             {
                 SqlParameter sqltmp = new SqlParameter("@SITE_YEAR", Year);
                 dbCommand.Parameters.Add(sqltmp);
             }
             else
             {
                 Year = "    ";
                 SqlParameter sqltmp = new SqlParameter("@SITE_YEAR", Year);
                 dbCommand.Parameters.Add(sqltmp);
             }

            if (UserID != string.Empty)
            {
                SqlParameter sqltmp = new SqlParameter("@UserId", UserID);
                dbCommand.Parameters.Add(sqltmp);
            }

            if (AdminAgency != string.Empty)
            {
                SqlParameter sqltmp = new SqlParameter("@Admin_Agency", AdminAgency);
                dbCommand.Parameters.Add(sqltmp);
            }

            ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]

         public static DataSet GetCaseSiteHieforSchedule(string Year)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[SITESCHDULE_DIFFHIEByYear]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);
             if (Year != string.Empty)
             {
                 SqlParameter sqltmp = new SqlParameter("@SITE_YEAR", Year);
                 dbCommand.Parameters.Add(sqltmp);
             }
            
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }


         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]     

         public static DataSet GetCaseHistDetails(string tblName, string key, string screenName)

         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetCaseHistDetails]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (tblName != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@HIST_TBLNAME", tblName);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (key != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@HIST_TBLKEY", key);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (screenName != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@HIST_SCREEN", screenName);
                 dbCommand.Parameters.Add(empnoParm);
             }
             
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         public static bool InsertCaseHist(List<SqlParameter> sqlParamList)
         {
             _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertCASEHIST");
             _dbCommand.CommandTimeout = 1200;
             _dbCommand.Parameters.Clear();
             foreach (SqlParameter sqlPar in sqlParamList)
             {
                 _dbCommand.Parameters.Add(sqlPar);
             }
             return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
         }

         public static DataSet GetHeadTemplate(string AgyType, string AgyCode)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[Headstart_Template]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (AgyType != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@AGY_TYPE", AgyType);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (AgyCode != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@AGY_CODE", AgyCode);
                 dbCommand.Parameters.Add(empnoParm);
             }

             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         public static DataSet GetChild_List(string Agency, string Dept,string Prog,string Year, string SortCol,string Sites,string Reapeters,string BaseAge)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[Get_ChildLists]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);
             dbCommand.CommandTimeout = 1800;
             

             if (Agency != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@MST_AGENCY", Agency);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (Dept != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@MST_DEPT", Dept);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (Prog != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@MST_PROGRAM", Prog);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (Year != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@MST_YEAR", Year);
                 dbCommand.Parameters.Add(empnoParm);
             }

             SqlParameter SqlSort = new SqlParameter("@SortCol", SortCol);
             dbCommand.Parameters.Add(SqlSort);

             SqlParameter SqlSort1 = new SqlParameter("@Site", Sites);
             dbCommand.Parameters.Add(SqlSort1);

             if (Reapeters != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@Repeaters", Reapeters);
                 dbCommand.Parameters.Add(empnoParm);
             }
             if (BaseAge != string.Empty)
             {
                 SqlParameter empnoParm = new SqlParameter("@BASEAGE", BaseAge);
                 dbCommand.Parameters.Add(empnoParm);
             }

             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }


         public static bool InsertUpdateDelStaffMst(List<SqlParameter> sqlParamList)
         {
             _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelSTAFFMST");
             _dbCommand.CommandTimeout = 1200;
             _dbCommand.Parameters.Clear();
             foreach (SqlParameter sqlPar in sqlParamList)
             {
                 _dbCommand.Parameters.Add(sqlPar);
             }
             return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
         }

         public static bool InsertUpdateDelStaffPost(List<SqlParameter> sqlParamList)
         {
             _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelStaffPost");
             _dbCommand.CommandTimeout = 1200;
             _dbCommand.Parameters.Clear();
             foreach (SqlParameter sqlPar in sqlParamList)
             {
                 _dbCommand.Parameters.Add(sqlPar);
             }
             return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetStaffPostAllDetails(string agency, string Dept, string Prog, string Code, string Category,string Seq) //Browse_CASESITE
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetStaffPost]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (agency != string.Empty)
             {
                 SqlParameter sqlSiteNo = new SqlParameter("@STP_AGENCY", agency);
                 dbCommand.Parameters.Add(sqlSiteNo);
             }
             if (Dept != string.Empty)
             {
                 SqlParameter sqlSiteName = new SqlParameter("@STP_DEPT", Dept);
                 dbCommand.Parameters.Add(sqlSiteName);
             }
             if (Prog != string.Empty)
             {
                 SqlParameter sqlcity = new SqlParameter("@STP_PROGRAM", Prog);
                 dbCommand.Parameters.Add(sqlcity);
             }

             if (Code != string.Empty)
             {
                 SqlParameter sqlyear = new SqlParameter("@STP_CODE", Code);
                 dbCommand.Parameters.Add(sqlyear);
             }
             if (Category != string.Empty)
             {
                 SqlParameter sqlAppno = new SqlParameter("@STP_CATEGORY", Category);
                 dbCommand.Parameters.Add(sqlAppno);
             }
             if (Seq != string.Empty)
             {
                 SqlParameter sqlAppno = new SqlParameter("@STP_SEQ", Seq);
                 dbCommand.Parameters.Add(sqlAppno);
             }
             // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }


         public static bool UpdateCaseMstRanks(List<SqlParameter> sqlParamList)
         {
             _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateMSTRanks");
             _dbCommand.CommandTimeout = 1200;
             _dbCommand.Parameters.Clear();
             foreach (SqlParameter sqlPar in sqlParamList)
             {
                 _dbCommand.Parameters.Add(sqlPar);
             }
             return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetCASEMSTALL(string Agency, string Dept, string Program, string Year, string AppNo,string ssnNo,string Name,string strSite,string strType)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GETMSTALL]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);
             dbCommand.CommandTimeout = 2400;

             if (Agency != string.Empty)
             {
                 SqlParameter agencyParm = new SqlParameter("@MST_AGENCY", Agency);
                 dbCommand.Parameters.Add(agencyParm);
             }
             if (Dept != string.Empty)
             {
                 SqlParameter deptParm = new SqlParameter("@MST_DEPT", Dept);
                 dbCommand.Parameters.Add(deptParm);
             }
             if (Program != string.Empty)
             {
                 SqlParameter programParm = new SqlParameter("@MST_PROGRAM", Program);
                 dbCommand.Parameters.Add(programParm);
             }
             if (Year != string.Empty)
             {
                 SqlParameter typeParm = new SqlParameter("@MST_YEAR", Year);
                 dbCommand.Parameters.Add(typeParm);
             }
             if (AppNo != string.Empty)
             {
                 SqlParameter typeParm = new SqlParameter("@MST_APP_NO", AppNo);
                 dbCommand.Parameters.Add(typeParm);
             }

             if (ssnNo != string.Empty)
             {
                 SqlParameter typeParm = new SqlParameter("@MST_SSN_NO", ssnNo);
                 dbCommand.Parameters.Add(typeParm);
             }

             if (Name != string.Empty)
             {
                 SqlParameter typeParm = new SqlParameter("@MST_Name", Name);
                 dbCommand.Parameters.Add(typeParm);
             }

              if (strSite != string.Empty)
             {
                 SqlParameter typeParm = new SqlParameter("@MST_SITES", strSite);
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
        public static DataSet GetCASBLTRB(string Agency, string Dept, string Program, string Year, string Worker, string strSite, string Bundle)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_CASBLTRB]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 2400;

            if (Agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@MST_AGENCY", Agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@MST_DEPT", Dept);
                dbCommand.Parameters.Add(deptParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@MST_PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@MST_YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (Worker != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@MST_INTAKE_WORKER", Worker);
                dbCommand.Parameters.Add(typeParm);
            }

            //if (ssnNo != string.Empty)
            //{
            //    SqlParameter typeParm = new SqlParameter("@MST_SSN_NO", ssnNo);
            //    dbCommand.Parameters.Add(typeParm);
            //}

            //if (Name != string.Empty)
            //{
            //    SqlParameter typeParm = new SqlParameter("@MST_Name", Name);
            //    dbCommand.Parameters.Add(typeParm);
            //}

            if (strSite != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@MST_SITES", strSite);
                dbCommand.Parameters.Add(typeParm);
            }

            if (Bundle != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@CASEACT_BUNDLE_NO", Bundle);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetCASEVDD()
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetCaseVdd]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         public static bool updateHSSB0124(string Agency,string dept,string program,string year,string app,string waitlist,string Repeat,string addoperator,string Rollenrl_SW,string EnrlDate_SW,string EnrlDate,string Status,string Repseq,string StrFundCodes,string KeepOld)
         {
             _dbCommand = _dbFactory.GetStoredProcCommand("dbo.Update_HSSB0124");
             _dbCommand.CommandTimeout = 1800;
             _dbCommand.Parameters.Clear();

             List<SqlParameter> sqlParamList = new List<SqlParameter>();
             SqlParameter sqlSP_Code = new SqlParameter("@AGENCY", Agency);
             _dbCommand.Parameters.Add(sqlSP_Code);

             SqlParameter sqlSP_Code1 = new SqlParameter("@DEPT", dept);
             _dbCommand.Parameters.Add(sqlSP_Code1);

             SqlParameter sqlSP_Code2 = new SqlParameter("@PROGRAM", program);
             _dbCommand.Parameters.Add(sqlSP_Code2);

             SqlParameter sqlSP_Code3 = new SqlParameter("@YEAR", year);
             _dbCommand.Parameters.Add(sqlSP_Code3);

             SqlParameter sqlSP_Code4 = new SqlParameter("@APPNO", app);
             _dbCommand.Parameters.Add(sqlSP_Code4);

             SqlParameter sqlSP_Code5 = new SqlParameter("@WAITLIST_SW", waitlist);
             _dbCommand.Parameters.Add(sqlSP_Code5);

             SqlParameter sqlSP_Code6 = new SqlParameter("@REPEAT", Repeat);
             _dbCommand.Parameters.Add(sqlSP_Code6);

             SqlParameter sqlSP_Code7 = new SqlParameter("@ADDOPERATOR", addoperator);
             _dbCommand.Parameters.Add(sqlSP_Code7);

             SqlParameter sqlSP_Code8 = new SqlParameter("@RollOverEnrl_Sw", Rollenrl_SW);
             _dbCommand.Parameters.Add(sqlSP_Code8);

             SqlParameter sqlSP_Code9 = new SqlParameter("@ENRLDATE_SW", EnrlDate_SW);
             _dbCommand.Parameters.Add(sqlSP_Code9);

             SqlParameter sqlSP_Code10 = new SqlParameter("@ENRL_DATE", EnrlDate);
             _dbCommand.Parameters.Add(sqlSP_Code10);

             SqlParameter sqlSP_Code11 = new SqlParameter("@STATUS", Status);
             _dbCommand.Parameters.Add(sqlSP_Code11);

             SqlParameter sqlSP_Code12 = new SqlParameter("@RepSeq", Repseq);
             _dbCommand.Parameters.Add(sqlSP_Code12);

             if (!string.IsNullOrEmpty(StrFundCodes.Trim()))
             {
                 SqlParameter sqlSP_Code13 = new SqlParameter("@FundCodes", StrFundCodes);
                 _dbCommand.Parameters.Add(sqlSP_Code13);
             }

             SqlParameter sqlSP_Code14 = new SqlParameter("@KEEPOld", KeepOld);
             _dbCommand.Parameters.Add(sqlSP_Code14);

             return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
         }



         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetSnpFixclinetId(string Year, string Count, string SSno, string ClientId, string FirstName, string LastName,string strdob,string strkey, string Type,string strFromdate,string strTodate, string Agency)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             
             sqlCommand = "[dbo].[GetSnpFixclinetId]";            
             dbCommand = db.GetStoredProcCommand(sqlCommand);
             dbCommand.CommandTimeout = 1200;
                         

             if (Count != string.Empty)
             {
                 SqlParameter agencyParm = new SqlParameter("@Count", Count);
                 dbCommand.Parameters.Add(agencyParm);
             }
             if (SSno != string.Empty)
             {
                 SqlParameter deptParm = new SqlParameter("@ssno", SSno);
                 dbCommand.Parameters.Add(deptParm);
             }
             if (ClientId != string.Empty)
             {
                 SqlParameter programParm = new SqlParameter("@ClientId", ClientId);
                 dbCommand.Parameters.Add(programParm);
             }
             if (Year != string.Empty)
             {
                 SqlParameter typeParm = new SqlParameter("@YEAR", Year);
                 dbCommand.Parameters.Add(typeParm);
             }
             if (FirstName != string.Empty)
             {
                 SqlParameter typeParm = new SqlParameter("@FirstName", FirstName);
                 dbCommand.Parameters.Add(typeParm);
             }
             if (LastName != string.Empty)
             {
                 SqlParameter typeParm = new SqlParameter("@LastName", LastName);
                 dbCommand.Parameters.Add(typeParm);
             }
             if (strdob != string.Empty)
             {
                 SqlParameter typeParm = new SqlParameter("@Dob", strdob);
                 dbCommand.Parameters.Add(typeParm);
             }

             if (strkey != string.Empty)
             {
                 SqlParameter typeParm = new SqlParameter("@Key", strkey);
                 dbCommand.Parameters.Add(typeParm);
             }
            
             if (Type != string.Empty)
             {
                 SqlParameter typeParm = new SqlParameter("@Type", Type);
                 dbCommand.Parameters.Add(typeParm);
             }
            if (strFromdate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@FromDate", strFromdate);
                dbCommand.Parameters.Add(typeParm);
            }
            if (strTodate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@ToDate", strTodate);
                dbCommand.Parameters.Add(typeParm);
            }
            if (Agency != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Agency", Agency);
                dbCommand.Parameters.Add(typeParm);
            }


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetSnpFixclinetIdHie(string Year, string Count, string SSno, string ClientId, string FirstName, string LastName, string strdob, string strkey, string Type,string strAgency,string strDept,string strProgram, string strPrYear, string strFromdate, string strTodate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();

            sqlCommand = "[dbo].[GetSnpFixclinetId]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1200;


            if (Count != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@Count", Count);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (SSno != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@ssno", SSno);
                dbCommand.Parameters.Add(deptParm);
            }
            if (ClientId != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@ClientId", ClientId);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (FirstName != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@FirstName", FirstName);
                dbCommand.Parameters.Add(typeParm);
            }
            if (LastName != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LastName", LastName);
                dbCommand.Parameters.Add(typeParm);
            }
            if (strdob != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Dob", strdob);
                dbCommand.Parameters.Add(typeParm);
            }

            if (strkey != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Key", strkey);
                dbCommand.Parameters.Add(typeParm);
            }

            if (Type != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Type", Type);
                dbCommand.Parameters.Add(typeParm);
            }
            if (strAgency != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Agency", strAgency);
                dbCommand.Parameters.Add(typeParm);
            }
            if (strDept != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Dept", strDept);
                dbCommand.Parameters.Add(typeParm);
            }
            if (strProgram != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Program", strProgram);
                dbCommand.Parameters.Add(typeParm);
            }
            if (strPrYear != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@PrYear", strPrYear);
                dbCommand.Parameters.Add(typeParm);
            }
            
            if (strFromdate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@FromDate", strFromdate);
                dbCommand.Parameters.Add(typeParm);
            }
            if (strTodate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@ToDate", strTodate);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        public static DataSet get_XMLDCRS(string  FormDate,string ToDate,string strType,string strSwitch,string strMode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();

            sqlCommand = "[dbo].[Get_XMLDCRS]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1200;

            if (FormDate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Fromdate", FormDate);
                dbCommand.Parameters.Add(typeParm);
            }
            if (ToDate  != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Todate", ToDate);
                dbCommand.Parameters.Add(typeParm);
            }
            if (strType != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(typeParm);
            }
            if (strSwitch != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Switch", strSwitch);
                dbCommand.Parameters.Add(typeParm);
            }
            if (strMode != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Mode", strMode);
                dbCommand.Parameters.Add(typeParm);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
                         
        }
         public static bool UpdateSNPClientId(List<SqlParameter> sqlParamList)
         {
             _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateSNPClientId");
             _dbCommand.CommandTimeout = 1200;
             _dbCommand.Parameters.Clear();
             foreach (SqlParameter sqlPar in sqlParamList)
             {
                 _dbCommand.Parameters.Add(sqlPar);
             }
             return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
         }

        public static bool UpdateClientIdS(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateClientIds");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertupdatedelRngCounts(List<SqlParameter> sqlParamList)
         {
             _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSERTUPDATEDELRNGCOUNTS");
             _dbCommand.CommandTimeout = 1200;
             _dbCommand.Parameters.Clear();
             foreach (SqlParameter sqlPar in sqlParamList)
             {
                 _dbCommand.Parameters.Add(sqlPar);
             }
             return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
         }
         public static bool ConvertionTableData(List<SqlParameter> sqlParamList)
         {
             _dbCommand = _dbFactory.GetStoredProcCommand("dbo.Sp_ConvertionData");
             _dbCommand.CommandTimeout = 1200;
             _dbCommand.Parameters.Clear();
             foreach (SqlParameter sqlPar in sqlParamList)
             {
                 _dbCommand.Parameters.Add(sqlPar);
             }
             return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
         }

        public static bool GETCODEGEN(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.GETCODEGEN");
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
