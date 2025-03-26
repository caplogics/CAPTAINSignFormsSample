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
    public partial class MainMenu
    {

        #region Constants
        private static readonly string TABLE_NAME = "[dbo].[PASSWORD]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;
        #endregion

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet MainMenuSearch(string SearchCat, string SearchFor, string CaseType, string CaseWRK, string App, string Lname, string Fname, string Ssn,
                                             string HNo, string Street, string City, string State, string Phone, string Alias, string ScanApp, string Hierarchy, string DOB, 
                                             string UserID,string strClientRules,string strAgency,string HomePhone)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[MainMenuSearch]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            //if (string.IsNullOrEmpty(Hierarchy))
                dbCommand.CommandTimeout = 1800;


            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (SearchCat != string.Empty)
            {
                SqlParameter sqlSearchCat = new SqlParameter("@S_Type", SearchCat);
                dbCommand.Parameters.Add(sqlSearchCat);
            }
            if (SearchFor != string.Empty)
            {
                SqlParameter sqlSearchFor = new SqlParameter("@S_For", SearchFor);
                dbCommand.Parameters.Add(sqlSearchFor);
            }

            if (CaseType != string.Empty)
            {
                SqlParameter sqlCaseType = new SqlParameter("@Case_Type", CaseType);
                dbCommand.Parameters.Add(sqlCaseType);
            }
            if (CaseWRK != string.Empty && CaseWRK != null)
            {
                SqlParameter sqlCaseWRK = new SqlParameter("@Case_Worker", CaseWRK);
                dbCommand.Parameters.Add(sqlCaseWRK);
            }
            if (App != string.Empty)
            {
                SqlParameter sqlApp = new SqlParameter("@App_No", App);
                dbCommand.Parameters.Add(sqlApp);
            }
            if (Lname != string.Empty)
            {
                SqlParameter sqlLname = new SqlParameter("@Nam_Lname", Lname);
                dbCommand.Parameters.Add(sqlLname);
            }
            if (Fname != string.Empty)
            {
                SqlParameter sqlFname = new SqlParameter("@Nam_Fname", Fname);
                dbCommand.Parameters.Add(sqlFname);
            }
            if (Ssn != string.Empty)
            {
                SqlParameter sqlSsn = new SqlParameter("@Ssn", Ssn);
                dbCommand.Parameters.Add(sqlSsn);
            }
            if (HNo != string.Empty)
            {
                SqlParameter sqlHNo = new SqlParameter("@Hno", HNo);
                dbCommand.Parameters.Add(sqlHNo);
            }
            if (Street != string.Empty)
            {
                SqlParameter sqlStreet = new SqlParameter("@Street", Street);
                dbCommand.Parameters.Add(sqlStreet);
            }
            if (City != string.Empty)
            {
                SqlParameter sqlCity = new SqlParameter("@City", City);
                dbCommand.Parameters.Add(sqlCity);
            }
            if (State != string.Empty)
            {
                SqlParameter sqlState = new SqlParameter("@State", State);
                dbCommand.Parameters.Add(sqlState);
            }
            if (Phone != string.Empty)
            {
                SqlParameter sqlPhone = new SqlParameter("@Phone", Phone);
                dbCommand.Parameters.Add(sqlPhone);
            }
            if (HomePhone != string.Empty)
            {
                SqlParameter sqlPhone = new SqlParameter("@Mobile", HomePhone);
                dbCommand.Parameters.Add(sqlPhone);
            }
            if (Alias != string.Empty)
            {
                SqlParameter sqlAlias = new SqlParameter("@Alias", Alias);
                dbCommand.Parameters.Add(sqlAlias);
            }
            if (ScanApp != string.Empty)
            {
                SqlParameter sqlScanApp = new SqlParameter("@ScanApp", ScanApp);
                dbCommand.Parameters.Add(sqlScanApp);
            }
            if (Hierarchy != string.Empty && Hierarchy != null)
            {
                SqlParameter sqlHierarchy = new SqlParameter("@Hierarchy", Hierarchy);
                dbCommand.Parameters.Add(sqlHierarchy);
            }
            if (!string.IsNullOrEmpty(DOB))
            {
                SqlParameter sqlDOB = new SqlParameter("@Snp_Dob", (Convert.ToDateTime(DOB)).ToShortDateString());
                dbCommand.Parameters.Add(sqlDOB);
            }

            if (!string.IsNullOrEmpty(UserID))
            {
                SqlParameter sqlUserID = new SqlParameter("@UserName", UserID);
                dbCommand.Parameters.Add(sqlUserID);
            }
            if (!string.IsNullOrEmpty(strClientRules))
            {
                SqlParameter sqlUserID = new SqlParameter("@ClientSwitch", strClientRules);
                dbCommand.Parameters.Add(sqlUserID);
            }

            if (!string.IsNullOrEmpty(strAgency))
            {
                SqlParameter sqlUserID = new SqlParameter("@Agency", strAgency);
                dbCommand.Parameters.Add(sqlUserID);
            }


            

            //sqlParamList.Add(new SqlParameter("@App_No", App));
            //sqlParamList.Add(new SqlParameter("@Nam_Lname", Lname));
            //sqlParamList.Add(new SqlParameter("@Nam_Fname", Fname));

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet MainMenuSearchLeanIntake(string SearchCat, string SearchFor, string CaseType, string CaseWRK, string App, string Lname, string Fname, string Ssn,
                                     string HNo, string Street, string City, string State, string Phone, string Alias, string ScanApp, string Hierarchy, string DOB, string UserID, string strClientRules, string strAgency)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[MainMenuSearchPIPIntake]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            //if (string.IsNullOrEmpty(Hierarchy))
            dbCommand.CommandTimeout = 1800;


            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (SearchCat != string.Empty)
            {
                SqlParameter sqlSearchCat = new SqlParameter("@S_Type", SearchCat);
                dbCommand.Parameters.Add(sqlSearchCat);
            }
            if (SearchFor != string.Empty)
            {
                SqlParameter sqlSearchFor = new SqlParameter("@S_For", SearchFor);
                dbCommand.Parameters.Add(sqlSearchFor);
            }

            if (CaseType != string.Empty)
            {
                SqlParameter sqlCaseType = new SqlParameter("@Case_Type", CaseType);
                dbCommand.Parameters.Add(sqlCaseType);
            }
            if (CaseWRK != string.Empty)
            {
                SqlParameter sqlCaseWRK = new SqlParameter("@Case_Worker", CaseWRK);
                dbCommand.Parameters.Add(sqlCaseWRK);
            }
            if (App != string.Empty)
            {
                SqlParameter sqlApp = new SqlParameter("@App_No", App);
                dbCommand.Parameters.Add(sqlApp);
            }
            if (Lname != string.Empty)
            {
                SqlParameter sqlLname = new SqlParameter("@Nam_Lname", Lname);
                dbCommand.Parameters.Add(sqlLname);
            }
            if (Fname != string.Empty)
            {
                SqlParameter sqlFname = new SqlParameter("@Nam_Fname", Fname);
                dbCommand.Parameters.Add(sqlFname);
            }
            if (Ssn != string.Empty)
            {
                SqlParameter sqlSsn = new SqlParameter("@Ssn", Ssn);
                dbCommand.Parameters.Add(sqlSsn);
            }
            if (HNo != string.Empty)
            {
                SqlParameter sqlHNo = new SqlParameter("@Hno", HNo);
                dbCommand.Parameters.Add(sqlHNo);
            }
            if (Street != string.Empty)
            {
                SqlParameter sqlStreet = new SqlParameter("@Street", Street);
                dbCommand.Parameters.Add(sqlStreet);
            }
            if (City != string.Empty)
            {
                SqlParameter sqlCity = new SqlParameter("@City", City);
                dbCommand.Parameters.Add(sqlCity);
            }
            if (State != string.Empty)
            {
                SqlParameter sqlState = new SqlParameter("@State", State);
                dbCommand.Parameters.Add(sqlState);
            }
            if (Phone != string.Empty)
            {
                SqlParameter sqlPhone = new SqlParameter("@Phone", Phone);
                dbCommand.Parameters.Add(sqlPhone);
            }
            if (Alias != string.Empty)
            {
                SqlParameter sqlAlias = new SqlParameter("@Alias", Alias);
                dbCommand.Parameters.Add(sqlAlias);
            }
            if (ScanApp != string.Empty)
            {
                SqlParameter sqlScanApp = new SqlParameter("@ScanApp", ScanApp);
                dbCommand.Parameters.Add(sqlScanApp);
            }
            if (Hierarchy != string.Empty)
            {
                SqlParameter sqlHierarchy = new SqlParameter("@Hierarchy", Hierarchy);
                dbCommand.Parameters.Add(sqlHierarchy);
            }
            if (!string.IsNullOrEmpty(DOB))
            {
                SqlParameter sqlDOB = new SqlParameter("@Snp_Dob", (Convert.ToDateTime(DOB)).ToShortDateString());
                dbCommand.Parameters.Add(sqlDOB);
            }

            if (!string.IsNullOrEmpty(UserID))
            {
                SqlParameter sqlUserID = new SqlParameter("@UserName", UserID);
                dbCommand.Parameters.Add(sqlUserID);
            }
            if (!string.IsNullOrEmpty(strClientRules))
            {
                SqlParameter sqlUserID = new SqlParameter("@ClientSwitch", strClientRules);
                dbCommand.Parameters.Add(sqlUserID);
            }

            if (!string.IsNullOrEmpty(strAgency))
            {
                SqlParameter sqlUserID = new SqlParameter("@Agency", strAgency);
                dbCommand.Parameters.Add(sqlUserID);
            }




            //sqlParamList.Add(new SqlParameter("@App_No", App));
            //sqlParamList.Add(new SqlParameter("@Nam_Lname", Lname));
            //sqlParamList.Add(new SqlParameter("@Nam_Fname", Fname));

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        

        public static DataSet MainMenuSearchEMS(string SearchCat, string SearchFor, string CaseType, string CaseWRK, string App, string Lname, string Fname, string Ssn,
                                            string HNo, string Street, string City, string State, string Phone, string Alias, string ScanApp, string Hierarchy, string DOB, string UserID,string strType,string strFDate,string strTDate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[MainMenuSearch]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            if (string.IsNullOrEmpty(Hierarchy))
                dbCommand.CommandTimeout = 1200;


            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (SearchCat != string.Empty)
            {
                SqlParameter sqlSearchCat = new SqlParameter("@S_Type", SearchCat);
                dbCommand.Parameters.Add(sqlSearchCat);
            }
            if (SearchFor != string.Empty)
            {
                SqlParameter sqlSearchFor = new SqlParameter("@S_For", SearchFor);
                dbCommand.Parameters.Add(sqlSearchFor);
            }

            if (CaseType != string.Empty)
            {
                SqlParameter sqlCaseType = new SqlParameter("@Case_Type", CaseType);
                dbCommand.Parameters.Add(sqlCaseType);
            }
            if (CaseWRK != string.Empty)
            {
                SqlParameter sqlCaseWRK = new SqlParameter("@Case_Worker", CaseWRK);
                dbCommand.Parameters.Add(sqlCaseWRK);
            }
            if (App != string.Empty)
            {
                SqlParameter sqlApp = new SqlParameter("@App_No", App);
                dbCommand.Parameters.Add(sqlApp);
            }
            if (Lname != string.Empty)
            {
                SqlParameter sqlLname = new SqlParameter("@Nam_Lname", Lname);
                dbCommand.Parameters.Add(sqlLname);
            }
            if (Fname != string.Empty)
            {
                SqlParameter sqlFname = new SqlParameter("@Nam_Fname", Fname);
                dbCommand.Parameters.Add(sqlFname);
            }
            if (Ssn != string.Empty)
            {
                SqlParameter sqlSsn = new SqlParameter("@Ssn", Ssn);
                dbCommand.Parameters.Add(sqlSsn);
            }
            if (HNo != string.Empty)
            {
                SqlParameter sqlHNo = new SqlParameter("@Hno", HNo);
                dbCommand.Parameters.Add(sqlHNo);
            }
            if (Street != string.Empty)
            {
                SqlParameter sqlStreet = new SqlParameter("@Street", Street);
                dbCommand.Parameters.Add(sqlStreet);
            }
            if (City != string.Empty)
            {
                SqlParameter sqlCity = new SqlParameter("@City", City);
                dbCommand.Parameters.Add(sqlCity);
            }
            if (State != string.Empty)
            {
                SqlParameter sqlState = new SqlParameter("@State", State);
                dbCommand.Parameters.Add(sqlState);
            }
            if (Phone != string.Empty)
            {
                SqlParameter sqlPhone = new SqlParameter("@Phone", Phone);
                dbCommand.Parameters.Add(sqlPhone);
            }
            if (Alias != string.Empty)
            {
                SqlParameter sqlAlias = new SqlParameter("@Alias", Alias);
                dbCommand.Parameters.Add(sqlAlias);
            }
            if (ScanApp != string.Empty)
            {
                SqlParameter sqlScanApp = new SqlParameter("@ScanApp", ScanApp);
                dbCommand.Parameters.Add(sqlScanApp);
            }
            if (Hierarchy != string.Empty)
            {
                SqlParameter sqlHierarchy = new SqlParameter("@Hierarchy", Hierarchy);
                dbCommand.Parameters.Add(sqlHierarchy);
            }
            if (!string.IsNullOrEmpty(DOB))
            {
                SqlParameter sqlDOB = new SqlParameter("@Snp_Dob", (DOB));
                dbCommand.Parameters.Add(sqlDOB);
            }

            if (!string.IsNullOrEmpty(UserID))
            {
                SqlParameter sqlUserID = new SqlParameter("@UserName", UserID);
                dbCommand.Parameters.Add(sqlUserID);
            }
            if (!string.IsNullOrEmpty(strType))
            {
                SqlParameter sqlUserID = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(sqlUserID);
            }
            if (!string.IsNullOrEmpty(strFDate))
            {
                SqlParameter sqlUserID = new SqlParameter("@FDate", strFDate);
                dbCommand.Parameters.Add(sqlUserID);
            }
            if (!string.IsNullOrEmpty(strTDate))
            {
                SqlParameter sqlUserID = new SqlParameter("@TDate", strTDate);
                dbCommand.Parameters.Add(sqlUserID);
            }


            //sqlParamList.Add(new SqlParameter("@App_No", App));
            //sqlParamList.Add(new SqlParameter("@Nam_Lname", Lname));
            //sqlParamList.Add(new SqlParameter("@Nam_Fname", Fname));

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet MainMenuGetAppName(string SnpKey, string FamSeq)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[MainMenuGetAppName]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (SnpKey != string.Empty)
            {
                SqlParameter sqlSnpKey = new SqlParameter("@SNP_KEY", SnpKey);
                dbCommand.Parameters.Add(sqlSnpKey);
            }
            if (FamSeq != string.Empty)
            {
                SqlParameter sqlFamSeq = new SqlParameter("@Family_Seq", FamSeq);
                dbCommand.Parameters.Add(sqlFamSeq);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet MainMenuGetHouseDetails(string SnpKey)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[MainMenuGetHouseDetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (SnpKey != string.Empty)
            {
                SqlParameter sqlSnpKey = new SqlParameter("@SNP_KEY", SnpKey);
                dbCommand.Parameters.Add(sqlSnpKey);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet MainMenuOtherPrograms(string Ssn, string Hierarchy, string UserName, string APP_MEM_Validation, string Snp_Fname, string Snp_Lname, string Snp_DOB,string ClientID)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[MainMenuOtherPrograms]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (Ssn != string.Empty)
            {
                SqlParameter sqlSsn = new SqlParameter("@Ssn", Ssn);
                dbCommand.Parameters.Add(sqlSsn);
            }
            SqlParameter sqlHierarchy = new SqlParameter("@Hierarchy", Hierarchy);
            dbCommand.Parameters.Add(sqlHierarchy);
            SqlParameter sqlUserName = new SqlParameter("@UserName", UserName);
            dbCommand.Parameters.Add(sqlUserName);

            if (!string.IsNullOrEmpty(APP_MEM_Validation))
            {
                SqlParameter sqlAPP_MEM_Validation = new SqlParameter("@App_Mem_Validation", APP_MEM_Validation);
                dbCommand.Parameters.Add(sqlAPP_MEM_Validation);
            }

            if (!string.IsNullOrEmpty(Snp_Fname))
            {
                SqlParameter sqlAPP_MEM_Validation = new SqlParameter("@Snp_Fname", Snp_Fname);
                dbCommand.Parameters.Add(sqlAPP_MEM_Validation);
            }

            if (!string.IsNullOrEmpty(Snp_Lname))
            {
                SqlParameter sqlAPP_MEM_Validation = new SqlParameter("@Snp_Lname", Snp_Lname);
                dbCommand.Parameters.Add(sqlAPP_MEM_Validation);
            }

            if (!string.IsNullOrEmpty(Snp_DOB))
            {
                SqlParameter sqlAPP_MEM_Validation = new SqlParameter("@Snp_Dob", Snp_DOB);
                dbCommand.Parameters.Add(sqlAPP_MEM_Validation);
            }
            if (!string.IsNullOrEmpty(ClientID))
            {
                SqlParameter sqlAPP_MEM_Validation = new SqlParameter("@SNP_Clid", ClientID);
                dbCommand.Parameters.Add(sqlAPP_MEM_Validation);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet MainMenuOtherPrograms_DeeperSearch(string Ssn, string Hierarchy, string UserName, string APP_MEM_Validation, string Snp_Fname, string Snp_Lname, string Snp_DOB,string ClientID)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[MainMenuOtherPrograms_DeeperSearch]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (Ssn != string.Empty)
            {
                SqlParameter sqlSsn = new SqlParameter("@Ssn", Ssn);
                dbCommand.Parameters.Add(sqlSsn);
            }
            SqlParameter sqlHierarchy = new SqlParameter("@Hierarchy", Hierarchy);
            dbCommand.Parameters.Add(sqlHierarchy);
            SqlParameter sqlUserName = new SqlParameter("@UserName", UserName);
            dbCommand.Parameters.Add(sqlUserName);

            if (!string.IsNullOrEmpty(APP_MEM_Validation))
            {
                SqlParameter sqlAPP_MEM_Validation = new SqlParameter("@App_Mem_Validation", APP_MEM_Validation);
                dbCommand.Parameters.Add(sqlAPP_MEM_Validation);
            }

            if (!string.IsNullOrEmpty(Snp_Fname))
            {
                SqlParameter sqlAPP_MEM_Validation = new SqlParameter("@Snp_Fname", Snp_Fname);
                dbCommand.Parameters.Add(sqlAPP_MEM_Validation);
            }

            if (!string.IsNullOrEmpty(Snp_Lname))
            {
                SqlParameter sqlAPP_MEM_Validation = new SqlParameter("@Snp_Lname", Snp_Lname);
                dbCommand.Parameters.Add(sqlAPP_MEM_Validation);
            }

            if (!string.IsNullOrEmpty(Snp_DOB))
            {
                SqlParameter sqlAPP_MEM_Validation = new SqlParameter("@Snp_Dob", Snp_DOB);
                dbCommand.Parameters.Add(sqlAPP_MEM_Validation);
            }

            if (!string.IsNullOrEmpty(ClientID))
            {
                SqlParameter sqlAPP_MEM_Validation = new SqlParameter("@SNP_Clid", ClientID);
                dbCommand.Parameters.Add(sqlAPP_MEM_Validation);
            }


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetGlobalHierarchies(string Type, string Agy, string Dept)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetGlobalHierarchies]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (Type != string.Empty)
            {
                SqlParameter sqlType = new SqlParameter("@TYPE", Type);
                dbCommand.Parameters.Add(sqlType);
            }
            SqlParameter sqlAgy = new SqlParameter("@AGY", Agy);
            dbCommand.Parameters.Add(sqlAgy);
            SqlParameter sqlDept = new SqlParameter("@DEPT", Dept);
            dbCommand.Parameters.Add(sqlDept);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetGlobalHierarchies_Latest(string User, string Type, string Agy, string Dept, string Prog)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;


            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetGlobalHierarchies_Latest]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlUser = new SqlParameter("@User", User);
            dbCommand.Parameters.Add(sqlUser);
            SqlParameter sqlType = new SqlParameter("@Type", Type);
            dbCommand.Parameters.Add(sqlType);
            SqlParameter sqlAgy = new SqlParameter("@TmpAgy", Agy);
            dbCommand.Parameters.Add(sqlAgy);
            SqlParameter sqlDept = new SqlParameter("@TmpDept", Dept);
            dbCommand.Parameters.Add(sqlDept);
            SqlParameter sqlProg = new SqlParameter("@TmpProg", Prog);
            dbCommand.Parameters.Add(sqlProg);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetUserDefHierarchy(string UserID)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetUserDefHierarchy]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (UserID != string.Empty)
            {
                SqlParameter sqlType = new SqlParameter("@UserID", UserID);
                dbCommand.Parameters.Add(sqlType);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCaseDepForHierarchy(string Agy, string Dept, string Prog)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCaseDepForHierarchy]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (Agy != string.Empty)
            {
                SqlParameter sqlAgy = new SqlParameter("@Agy ", Agy);
                dbCommand.Parameters.Add(sqlAgy);
            }
            if (Dept != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@Dept", Dept);
                dbCommand.Parameters.Add(sqlDept);
            }
            if (Prog != string.Empty)
            {
                SqlParameter sqlProg = new SqlParameter("@Prog", Prog);
                dbCommand.Parameters.Add(sqlProg);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetPrivilizes_byScrCode(string UserName, string ModuleCode, string ScrCode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetPrivilizes_byScrCode]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlUserName = new SqlParameter("@UserName ", UserName);
            dbCommand.Parameters.Add(sqlUserName);
            SqlParameter sqlModuleCode = new SqlParameter("@ModuleCode", ModuleCode);
            dbCommand.Parameters.Add(sqlModuleCode);
            SqlParameter sqlScrCode = new SqlParameter("@ScrCode", ScrCode);
            dbCommand.Parameters.Add(sqlScrCode);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet MainMenu_Navigate_App(string UserName, string Nav_Option, string Hie, string App)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[MainMenu_Navigate_App]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            //if (string.IsNullOrEmpty(Hierarchy))
                dbCommand.CommandTimeout = 1200;


            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlUserName = new SqlParameter("@UserName ", UserName);
            dbCommand.Parameters.Add(sqlUserName);
            SqlParameter sqlModuleCode = new SqlParameter("@Navigate_Option", Nav_Option);
            dbCommand.Parameters.Add(sqlModuleCode);
            SqlParameter sqlHie = new SqlParameter("@Hierarchy", Hie);
            dbCommand.Parameters.Add(sqlHie);
            SqlParameter sqlApp = new SqlParameter("@App_No", App);
            dbCommand.Parameters.Add(sqlApp);


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static int GETADHOCHISTORY(string ScreenName, string AdhDate, string Mode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;
            int intcount = 0;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETADHOCHISTORY]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            //if (string.IsNullOrEmpty(Hierarchy))
            dbCommand.CommandTimeout = 1200;


            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlUserName = new SqlParameter("@ADH_SCREEN_NAME", ScreenName);
            dbCommand.Parameters.Add(sqlUserName);
            if (AdhDate != string.Empty)
            {
                SqlParameter sqlModuleCode = new SqlParameter("@ADH_DATE", AdhDate);
                dbCommand.Parameters.Add(sqlModuleCode);
            }
            if (Mode != string.Empty)
            {
                SqlParameter sqlHie = new SqlParameter("@Mode", Mode);
                dbCommand.Parameters.Add(sqlHie);
            }


            ds = db.ExecuteDataSet(dbCommand);
            if(ds.Tables[0].Rows.Count>0)
            {
                intcount = Convert.ToInt32(ds.Tables[0].Rows[0]["TOTALCOUNT"]);
            }
            return intcount;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet AgencyPartner_Navigate(string Nav_Option, string Agencycode,string strMode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_AGCYPART_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            //if (string.IsNullOrEmpty(Hierarchy))
            dbCommand.CommandTimeout = 1200;


            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //SqlParameter sqlUserName = new SqlParameter("@UserName ", UserName);
            //dbCommand.Parameters.Add(sqlUserName);
            SqlParameter sqlModuleCode = new SqlParameter("@Navigate_Option", Nav_Option);
            dbCommand.Parameters.Add(sqlModuleCode);
           
            if (Agencycode != string.Empty)
            {
                SqlParameter sqlApp = new SqlParameter("@AGYPCode", Agencycode);
                dbCommand.Parameters.Add(sqlApp);
            }
            if (strMode != string.Empty)
            {
                SqlParameter sqlApp = new SqlParameter("@Mode", strMode);
                dbCommand.Parameters.Add(sqlApp);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        // public static DataSet UpdateUserDefHierarchy(string User, string Agy, string Dept, string Prog, string Year)
        // {
        //     DataSet ds;
        //     Database db;
        //     string sqlCommand;
        //     DbCommand dbCommand;

        //     db = DatabaseFactory.CreateDatabase();
        //     sqlCommand = "[dbo].[UpdateUserDefHierarchy]";
        //     dbCommand = db.GetStoredProcCommand(sqlCommand);

        //     List<SqlParameter> sqlParamList = new List<SqlParameter>();
        //     if (User != string.Empty)
        //     {
        //         SqlParameter sqlUser = new SqlParameter("@User ", User);
        //         dbCommand.Parameters.Add(sqlUser);
        //     }
        //    if (Agy != string.Empty)
        //     {
        //         SqlParameter sqlAgy = new SqlParameter("@Agy ", Agy);
        //         dbCommand.Parameters.Add(sqlAgy);
        //     }
        //     if (Dept != string.Empty)
        //     {
        //         SqlParameter sqlDept = new SqlParameter("@Dept", Dept);
        //         dbCommand.Parameters.Add(sqlDept);
        //     }
        //     if (Prog != string.Empty)
        //     {
        //         SqlParameter sqlProg = new SqlParameter("@Prog", Prog);
        //         dbCommand.Parameters.Add(sqlProg);
        //     }
        //     if (Year != string.Empty)
        //     {
        //         SqlParameter sqlYear = new SqlParameter("@Year", Year);
        //         dbCommand.Parameters.Add(sqlYear);
        //     }

        //     ds = db.ExecuteDataSet(dbCommand);
        //     return ds;
        // }

        public static bool UpdateUserDefHierarchy(string User, string Agy, string Dept, string Prog, string Year)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateUserDefHierarchy");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (User != string.Empty)
            {
                SqlParameter sqlUser = new SqlParameter("@User ", User);
                _dbCommand.Parameters.Add(sqlUser);
            }
            if (Agy != string.Empty)
            {
                SqlParameter sqlAgy = new SqlParameter("@Agy ", Agy);
                _dbCommand.Parameters.Add(sqlAgy);
            }
            if (Dept != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@Dept", Dept);
                _dbCommand.Parameters.Add(sqlDept);
            }
            if (Prog != string.Empty)
            {
                SqlParameter sqlProg = new SqlParameter("@Prog", Prog);
                _dbCommand.Parameters.Add(sqlProg);
            }
            if (Year != string.Empty)
            {
                SqlParameter sqlYear = new SqlParameter("@Year", Year);
                _dbCommand.Parameters.Add(sqlYear);
            }

            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        //public static bool DragApp_HouseHold(string Drag_From_Hie, string Drag_to_hie, string User, string App_Mem_Sw, string Member_seq, string Sum_Ref_App, string Agy_Short_Name, out string New_Dragged_App_No)
        public static bool DragApp_HouseHold(string Drag_From_Hie, string Drag_to_hie, string User, string App_Mem_Sw, string Member_seq, string Sum_Ref_App, string strModuleCode, out string New_Dragged_App_No)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.MainMenuDragApp_HouseHold");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            New_Dragged_App_No = string.Empty;
            bool App_Drag_Sw = false;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqDrag_From_Hie = new SqlParameter("@Mst_key_ToDrag ", Drag_From_Hie);
            _dbCommand.Parameters.Add(sqDrag_From_Hie);
            SqlParameter sqlDrag_to_hie = new SqlParameter("@Mst_key_InToDrag ", Drag_to_hie);
            _dbCommand.Parameters.Add(sqlDrag_to_hie);
            SqlParameter sqlUser = new SqlParameter("@UserName", User);
            _dbCommand.Parameters.Add(sqlUser);
            SqlParameter sqlApp_Mem_Sw = new SqlParameter("@App_Mem_Sw", App_Mem_Sw);
            _dbCommand.Parameters.Add(sqlApp_Mem_Sw);
            SqlParameter sqlMember_seq = new SqlParameter("@Fam_seq", int.Parse(Member_seq));
            _dbCommand.Parameters.Add(sqlMember_seq);
            //SqlParameter sqlAgy_Short_Name = new SqlParameter("@Agy_Short_Name", Agy_Short_Name);
            //_dbCommand.Parameters.Add(sqlAgy_Short_Name);

            if (!string.IsNullOrEmpty(Sum_Ref_App.Trim()))
            {
                SqlParameter sqlMember_RefApp = new SqlParameter("@TmpSum_Key_To_Update", Sum_Ref_App);
                _dbCommand.Parameters.Add(sqlMember_RefApp);
            }

            if (!string.IsNullOrEmpty(strModuleCode.Trim()))
            {
                SqlParameter sqlMember_modulecode = new SqlParameter("@ModuleCode", strModuleCode);
                _dbCommand.Parameters.Add(sqlMember_modulecode);
            }
            

            SqlParameter New_App = new SqlParameter("@New_MstKey_Out", SqlDbType.VarChar, 18);
             New_App.Direction = ParameterDirection.Output;
             _dbCommand.Parameters.Add(New_App);

             App_Drag_Sw = _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
             New_Dragged_App_No = New_App.Value.ToString();
             
            return App_Drag_Sw; 
        }

        // Added by Vikash on 05/09/2023 for Print Application Control Screen
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet INSUPDEL_PRINAPPCNTL(string Form, string Enable, string Name,string Signed, string Agy, string userID, string mode, string Type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[INSUPDEL_PRINAPPCNTL]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;


            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (Agy != string.Empty)
            {
                SqlParameter sqlSearchCat = new SqlParameter("@PAC_AGY", Agy);
                dbCommand.Parameters.Add(sqlSearchCat);
            }
            if (Form != string.Empty)
            {
                SqlParameter sqlSearchCat = new SqlParameter("@PAC_FORM", Form);
                dbCommand.Parameters.Add(sqlSearchCat);
            }
            if (Enable != string.Empty)
            {
                SqlParameter sqlSearchFor = new SqlParameter("@PAC_ENABLE", Enable);
                dbCommand.Parameters.Add(sqlSearchFor);
            }
            if (Name != string.Empty)
            {
                SqlParameter sqlSearchFor = new SqlParameter("@PAC_DISP_NAME", Name);
                dbCommand.Parameters.Add(sqlSearchFor);
            }

            if (Signed != string.Empty)
            {
                SqlParameter sqlSearchFor = new SqlParameter("@PAC_SIGN", Signed);
                dbCommand.Parameters.Add(sqlSearchFor);
            }

            if (userID != string.Empty)
            {
                SqlParameter sqlSearchFor = new SqlParameter("@PAC_LSTC_OPERATOR", userID);
                dbCommand.Parameters.Add(sqlSearchFor);
            }

            if (mode != string.Empty)
            {
                SqlParameter sqlSearchFor = new SqlParameter("@Mode", mode);
                dbCommand.Parameters.Add(sqlSearchFor);
            }

            if (Type != string.Empty)
            {
                SqlParameter sqlSearchFor = new SqlParameter("@PAC_Type", Type);
                dbCommand.Parameters.Add(sqlSearchFor);
            }



            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GET_PRINAPPCNTL(string Agy, string Form, string strAgy,string strType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_PRINAPPCNTL]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;


            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (Agy != string.Empty)
            {
                SqlParameter sqlSearchCat = new SqlParameter("@PAC_AGY", Agy);
                dbCommand.Parameters.Add(sqlSearchCat);
            }

            if (strType != string.Empty)
            {
                SqlParameter sqlSearchCat = new SqlParameter("@PAC_TYPE", strType);
                dbCommand.Parameters.Add(sqlSearchCat);
            }

            if (Form != string.Empty)
            {
                SqlParameter sqlSearchCat = new SqlParameter("@PAC_FORM", Form);
                dbCommand.Parameters.Add(sqlSearchCat);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //public static DataSet GetAgencyTableByApp(string AppCode)
        //{
        //    DataSet ds;
        //    Database db;
        //    string sqlCommand;
        //    DbCommand dbCommand;

        //    db = DatabaseFactory.CreateDatabase();
        //    sqlCommand = "[dbo].[GetAgyTab]";
        //    dbCommand = db.GetStoredProcCommand(sqlCommand);

        //    if (AppCode != string.Empty)
        //    {
        //        SqlParameter appCode = new SqlParameter("@AppCode", AppCode.Trim());
        //        dbCommand.Parameters.Add(appCode);
        //    }

        //    // Get results.
        //    ds = db.ExecuteDataSet(dbCommand);
        //    return ds;
        //}

        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //public static DataSet GetAgyTabDetails(string AgyType)
        //{
        //    DataSet ds;
        //    Database db;
        //    string sqlCommand;
        //    DbCommand dbCommand;

        //    db = DatabaseFactory.CreateDatabase();
        //    sqlCommand = "[dbo].[GetAgyTabDetails]";
        //    dbCommand = db.GetStoredProcCommand(sqlCommand);

        //    List<SqlParameter> sqlParamList = new List<SqlParameter>();
        //    if (AgyType != string.Empty)
        //    {
        //        SqlParameter empnoParm = new SqlParameter("@AGY_TYPE", AgyType);
        //        dbCommand.Parameters.Add(empnoParm);

        //    }

        //    ds = db.ExecuteDataSet(dbCommand);
        //    return ds;
        //}

        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //public static DataSet GetHierarchyNames(string Agency, string Dept, string Prog)
        //{
        //    DataSet ds;
        //    Database db;
        //    string sqlCommand;
        //    DbCommand dbCommand;

        //    db = DatabaseFactory.CreateDatabase();
        //    sqlCommand = "[dbo].[GetHierarchyNames]";
        //    dbCommand = db.GetStoredProcCommand(sqlCommand);

        //    List<SqlParameter> sqlParamList = new List<SqlParameter>();
        //    SqlParameter sqlAgency = new SqlParameter("@HIE_AGENCY", Agency);
        //    dbCommand.Parameters.Add(sqlAgency);
        //    SqlParameter sqlDept = new SqlParameter("@HIE_DEPT", Dept);
        //    dbCommand.Parameters.Add(sqlDept);
        //    SqlParameter sqlProg = new SqlParameter("@HIE_PROGRAM", Prog);
        //    dbCommand.Parameters.Add(sqlProg);

        //    ds = db.ExecuteDataSet(dbCommand);
        //    return ds;
        //}


        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //public static DataSet GetSelAgyChildDetails(string AgyType1, string AgyColCode)
        //{
        //    DataSet ds;
        //    Database db;
        //    string sqlCommand;
        //    DbCommand dbCommand;

        //    db = DatabaseFactory.CreateDatabase();
        //    sqlCommand = "[dbo].[GetSelAgyChildDetails]";
        //    dbCommand = db.GetStoredProcCommand(sqlCommand);

        //    if (AgyType1 != string.Empty)
        //    {
        //        SqlParameter empnoParm = new SqlParameter("@AGY_TYPE", AgyType1);
        //        dbCommand.Parameters.Add(empnoParm);
        //    }
        //    if (AgyColName != string.Empty)
        //    {
        //        SqlParameter empnoParm = new SqlParameter("@AGY_COLNAME", AgyColName);
        //        dbCommand.Parameters.Add(empnoParm);
        //    }
        //    if (AgyColCode != string.Empty)
        //    {
        //        SqlParameter empnoParm = new SqlParameter("@AGY_COLCODE", AgyColCode);
        //        dbCommand.Parameters.Add(empnoParm);
        //    }

        //    ds = db.ExecuteDataSet(dbCommand);
        //    return ds;
        //}

        //  public static bool UpdateAGYTAB(List<SqlParameter> sqlParamList)
        //{
        //    _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateAGYTAB");
        //    _dbCommand.CommandTimeout = 1200;
        //    _dbCommand.Parameters.Clear();
        //    foreach (SqlParameter sqlPar in sqlParamList)
        //    {
        //        _dbCommand.Parameters.Add(sqlPar);
        //    }
        //    return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        //}

        //  [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //  public static bool DeleteAGYTAB(string type, string code)
        //  {
        //      _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteAGYTAB");
        //      _dbCommand.CommandTimeout = 1200;
        //      _dbCommand.Parameters.Clear();

        //      List<SqlParameter> sqlParamList = new List<SqlParameter>();
        //      SqlParameter sqlAgency = new SqlParameter("@AGYTYPE", type);
        //      _dbCommand.Parameters.Add(sqlAgency);
        //      SqlParameter sqlCode = new SqlParameter("@AGYCODE", code);
        //      _dbCommand.Parameters.Add(sqlCode);

        //      return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        //  }

    }
}
