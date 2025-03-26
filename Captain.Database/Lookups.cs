using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.ComponentModel;

namespace Captain.DatabaseLayer
{
    public partial class Lookups
    {
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetHierarchies(string isIntake)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetHierarchies]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);


            if (isIntake != string.Empty)
            {
                SqlParameter isIntakeParm = new SqlParameter("@FilterType", isIntake);
                dbCommand.Parameters.Add(isIntakeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }
        
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetHierarchies(string filterBy, string agency, string dept, string program,string strUserId,string AdminAgency)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetHierarchies]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 6000;


            if (filterBy != string.Empty)
            {
                SqlParameter isIntakeParm = new SqlParameter("@FilterType", filterBy);
                dbCommand.Parameters.Add(isIntakeParm);
            }

            if (agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@Agency", agency);
                dbCommand.Parameters.Add(agencyParm);
            }

            if (dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@Dept", dept);
                dbCommand.Parameters.Add(deptParm);
            }

            if (program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@Program", program);
                dbCommand.Parameters.Add(programParm);
            }
            if (strUserId != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@UserID", strUserId);
                dbCommand.Parameters.Add(programParm);
            }
            if (AdminAgency != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@Admin_Agency", AdminAgency);
                dbCommand.Parameters.Add(programParm);
            }


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetHierarchiesByUserID(string userID, string isIntake,string strType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetHierarchiesByUserID]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (userID != string.Empty)
            {
                SqlParameter userParm = new SqlParameter("@UserID", userID);
                dbCommand.Parameters.Add(userParm);
            }

            if (isIntake != string.Empty)
            {
                SqlParameter isIntakeParm = new SqlParameter("@PWH_TYPE", isIntake);
                dbCommand.Parameters.Add(isIntakeParm);
            }
           
            if (strType != string.Empty)
            {
                SqlParameter isIntakeParm = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(isIntakeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetSerHierarchiesByUserID(string userID, string isIntake, string strType,string Agy, string dept, string Prog)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetSERHierarchiesByUserID]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (userID != string.Empty)
            {
                SqlParameter userParm = new SqlParameter("@UserID", userID);
                dbCommand.Parameters.Add(userParm);
            }

            if (isIntake != string.Empty)
            {
                SqlParameter isIntakeParm = new SqlParameter("@PWH_TYPE", isIntake);
                dbCommand.Parameters.Add(isIntakeParm);
            }

            if (strType != string.Empty)
            {
                SqlParameter isIntakeParm = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(isIntakeParm);
            }

            if (Agy != string.Empty)
            {
                SqlParameter isIntakeParm = new SqlParameter("@SerAgy", Agy);
                dbCommand.Parameters.Add(isIntakeParm);
            }

            if (dept != string.Empty)
            {
                SqlParameter isIntakeParm = new SqlParameter("@SerDept", dept);
                dbCommand.Parameters.Add(isIntakeParm);
            }

            if (Prog != string.Empty)
            {
                SqlParameter isIntakeParm = new SqlParameter("@SerProg", Prog);
                dbCommand.Parameters.Add(isIntakeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetPasswordSerUserID(string userID)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetPassworserdhie]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (userID != string.Empty)
            {
                SqlParameter userParm = new SqlParameter("@PWSH_EMPLOYEE_NO", userID);
                dbCommand.Parameters.Add(userParm);
            }

            //if (isIntake != string.Empty)
            //{
            //    SqlParameter isIntakeParm = new SqlParameter("@PWH_TYPE", isIntake);
            //    dbCommand.Parameters.Add(isIntakeParm);
            //}

            //if (strType != string.Empty)
            //{
            //    SqlParameter isIntakeParm = new SqlParameter("@Type", strType);
            //    dbCommand.Parameters.Add(isIntakeParm);
            //}

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetPasswordHieByUserID(string userID)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetPasswordHieByUserId]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (userID != string.Empty)
            {
                SqlParameter userParm = new SqlParameter("@UserID", userID);
                dbCommand.Parameters.Add(userParm);
            }
            
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetModules()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetModules]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetStaff()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetStaff]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCaseSite()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCaseSite]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetAgencyTables()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetAgencyTables]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetAdditionalPrivileges()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetAdditionalPrivileges]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetScreens(string moduleCode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetScreens]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (moduleCode != string.Empty)
            {
                SqlParameter moduleCodeParm = new SqlParameter("@ModuleCode", moduleCode);
                dbCommand.Parameters.Add(moduleCodeParm);
            }
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetScreensByUserID(string moduleCode, string userID, string mode, string hierarchy)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetScreensByUserID]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (moduleCode != string.Empty)
            {
                SqlParameter moduleCodeParm = new SqlParameter("@ModuleCode", moduleCode);
                dbCommand.Parameters.Add(moduleCodeParm);
            }

            if (userID != string.Empty && userID != null)
            {
                SqlParameter moduleCodeParm = new SqlParameter("@UserID", userID);
                dbCommand.Parameters.Add(moduleCodeParm);
            }

            if (mode != string.Empty)
            {
                SqlParameter modeCodeParm = new SqlParameter("@ViewEdit", mode);
                dbCommand.Parameters.Add(modeCodeParm);
            }

            if (hierarchy != string.Empty)
            {
                SqlParameter hierarchyParm = new SqlParameter("@Hierarchy", hierarchy);
                dbCommand.Parameters.Add(hierarchyParm);
            }
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetReportsByUserID(string moduleCode, string userID, string mode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetReportsByUserID]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (moduleCode != string.Empty)
            {
                SqlParameter moduleCodeParm = new SqlParameter("@ModuleCode", moduleCode);
                dbCommand.Parameters.Add(moduleCodeParm);
            }

            if (userID != string.Empty && userID != null)
            {
                SqlParameter moduleCodeParm = new SqlParameter("@UserID", userID);
                dbCommand.Parameters.Add(moduleCodeParm);
            }

            if (mode != string.Empty)
            {
                SqlParameter modeCodeParm = new SqlParameter("@ViewEdit", mode);
                dbCommand.Parameters.Add(modeCodeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetUserReportMaintenanceByUserID(string moduleCode, string userID, string mode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetUserReportsByUserID]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (moduleCode != string.Empty)
            {
                SqlParameter moduleCodeParm = new SqlParameter("@ModuleCode", moduleCode);
                dbCommand.Parameters.Add(moduleCodeParm);
            }

            if (userID != string.Empty && userID != null)
            {
                SqlParameter moduleCodeParm = new SqlParameter("@UserID", userID);
                dbCommand.Parameters.Add(moduleCodeParm);
            }

            if (mode != string.Empty)
            {
                SqlParameter modeCodeParm = new SqlParameter("@ViewEdit", mode);
                dbCommand.Parameters.Add(modeCodeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetReports(string moduleCode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetReports]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (moduleCode != string.Empty)
            {
                SqlParameter moduleCodeParm = new SqlParameter("@ModuleCode", moduleCode);
                dbCommand.Parameters.Add(moduleCodeParm);
            }
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetLookUpCode(string Type, string code)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetLookUpCode]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            SqlParameter filterParm = new SqlParameter("@filterCode", Type);
            dbCommand.Parameters.Add(filterParm);
            SqlParameter filtercode = new SqlParameter("@filterCode2", code);
            dbCommand.Parameters.Add(filtercode);
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetLookUpFromAGYTAB(string Code)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetLookUpValues]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            SqlParameter filterParm = new SqlParameter("@filterCode", Code);
            dbCommand.Parameters.Add(filterParm);
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetLookUpFromAGYTAB(string Code,string strFilterType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetLookUpValues]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            SqlParameter filterParm = new SqlParameter("@filterCode", Code);
            dbCommand.Parameters.Add(filterParm);
            if (strFilterType != string.Empty)
            {
                SqlParameter filterType = new SqlParameter("@filterType", strFilterType);
                dbCommand.Parameters.Add(filterType);
            }
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
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
        public static DataSet GetHierarchiesDepartment(string agency, string dept, string program,string Year,string strApplicationNO,string Type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETCaseHieDepartment]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
           

            if (agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@Agency", agency);
                dbCommand.Parameters.Add(agencyParm);
            }

            if (dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@Dept", dept);
                dbCommand.Parameters.Add(deptParm);
            }

            if (program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@Program", program);
                dbCommand.Parameters.Add(programParm);
            }

            if (Year != string.Empty)
            {
                SqlParameter yearParma = new SqlParameter("@Year", Year);
                dbCommand.Parameters.Add(yearParma);
            }
            if (strApplicationNO != string.Empty)
            {
                SqlParameter yearParma = new SqlParameter("@ApplicatinNo", strApplicationNO);
                dbCommand.Parameters.Add(yearParma);
            }

            if (Type != string.Empty)
            {
                SqlParameter TypeParma = new SqlParameter("@Type", Type);
                dbCommand.Parameters.Add(TypeParma);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }
       
        
         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetDepEntollHierachies(string agency, string dept, string program, string Hierachy)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETDEPNRLHIE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
           

            if (agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@DEPNRLHIE_AGENCY", agency);
                dbCommand.Parameters.Add(agencyParm);
            }

            if (dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@DEPNRLHIE_DEPT", dept);
                dbCommand.Parameters.Add(deptParm);
            }

            if (program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@DEPNRLHIE_PROG", program);
                dbCommand.Parameters.Add(programParm);
            }

            if (Hierachy != string.Empty)
            {
                SqlParameter yearParma = new SqlParameter("@DEPNRLHIE_HIE", Hierachy);
                dbCommand.Parameters.Add(yearParma);
            }            

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetChildAge(string agency, string dept, string program, string year,string ApplNo,string Dob,string Type)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetChildDate]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);


             if (agency != string.Empty)
             {
                 SqlParameter agencyParm = new SqlParameter("@AGENCY", agency);
                 dbCommand.Parameters.Add(agencyParm);
             }

             if (dept != string.Empty)
             {
                 SqlParameter deptParm = new SqlParameter("@DEPT", dept);
                 dbCommand.Parameters.Add(deptParm);
             }

             if (program != string.Empty)
             {
                 SqlParameter programParm = new SqlParameter("@Program", program);
                 dbCommand.Parameters.Add(programParm);
             }

             if (year != string.Empty)
             {
                 SqlParameter yearParma = new SqlParameter("@Year", year);
                 dbCommand.Parameters.Add(yearParma);
             }

             if (ApplNo != string.Empty)
             {
                 SqlParameter yearParma = new SqlParameter("@ApplicantNo", ApplNo);
                 dbCommand.Parameters.Add(yearParma);
             }

             if (Dob != string.Empty)
             {
                 SqlParameter yearParma = new SqlParameter("@SnpDate", Dob);
                 dbCommand.Parameters.Add(yearParma);
             }
             if (Type != string.Empty)
             {
                 SqlParameter yearParma = new SqlParameter("@Type", Type);
                 dbCommand.Parameters.Add(yearParma);
             }

             // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }


         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetAgyTabs(string agytype, string agycode, string agydesc)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GET_AGYTABS]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);


             if (agytype != string.Empty)
             {
                 SqlParameter agencyParm = new SqlParameter("@AGYS_TYPE", agytype);
                 dbCommand.Parameters.Add(agencyParm);
             }

             if (agycode != string.Empty)
             {
                 SqlParameter deptParm = new SqlParameter("@AGYS_CODE", agycode);
                 dbCommand.Parameters.Add(deptParm);
             }

             if (agydesc != string.Empty)
             {
                 SqlParameter programParm = new SqlParameter("@AGYS_DESC", agydesc);
                 dbCommand.Parameters.Add(programParm);
             }            

             // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }


         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetPirQuestRecord(string strYear)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

            

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetPirQuest]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);


             if (strYear != string.Empty)
             {
                 SqlParameter programParm = new SqlParameter("@PIRQUEST_YEAR", strYear);
                 dbCommand.Parameters.Add(programParm);
             }  

             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }


         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetCapAppl()
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;



             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GETCAPAPPL]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetJobSchdule(string FromDate, string Todate)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetJOBSCHDULER]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);
             SqlParameter filterParm = new SqlParameter("@FromDate", FromDate);
             dbCommand.Parameters.Add(filterParm);
             SqlParameter filterType = new SqlParameter("@ToDate", Todate);
             dbCommand.Parameters.Add(filterType);
             // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetUserCount(string UserID,string FromDate, string Todate)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GET_Usage_Report]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);
             dbCommand.CommandTimeout = 6000;

             if (UserID != string.Empty)
             {
                 SqlParameter deptParm = new SqlParameter("@USER", UserID);
                 dbCommand.Parameters.Add(deptParm);
             }
             if (FromDate != string.Empty)
             {
                 SqlParameter filterParm = new SqlParameter("@FROMDATE", FromDate);
                 dbCommand.Parameters.Add(filterParm);
             }
             if (Todate != string.Empty)
             {
                 SqlParameter filterType = new SqlParameter("@TODATE", Todate);
                 dbCommand.Parameters.Add(filterType);
             }
             // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetBundles(string agency, string dept, string program, string progyear)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_CASEACTBUNDLES]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            
            

            if (agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@CASEACT_AGENCY", agency);
                dbCommand.Parameters.Add(agencyParm);
            }

            if (dept != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@CASEACT_DEPT", dept);
                dbCommand.Parameters.Add(deptParm);
            }

            if (program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@CASEACT_PROGRAM", program);
                dbCommand.Parameters.Add(programParm);
            }
            if (progyear != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@CASEACT_YEAR", progyear);
                dbCommand.Parameters.Add(programParm);
            }


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetMenuBranches()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[MENUBRANCHES_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

           ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        #region Vikash Added on 04/05/2024

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet BrowsePIRQues2(string strYear)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;



            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[BROWSE_PIRQUEST2]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);


            if (strYear != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@PIRQUEST_YEAR", strYear);
                dbCommand.Parameters.Add(programParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        // Added by Vikash on 05/06/2024 for pir counts
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet BrowsePIRCOUNTS(string strYear)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;



            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[BROWSE_PIRCOUNT]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);


            if (strYear != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@YEAR", strYear);
                dbCommand.Parameters.Add(programParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        #endregion

        #region Vikash added on for generating PIR Audit report

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_PIR_Audit_Report(string Agency, string Dept, string Prog, string Year, string section, string fund, string site, string QuesID, string QuesType, string userID, string isAudit, string HitOnly)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_PIR_Audit_Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 2400;

            if (Agency != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@BaseAgy", Agency);
                dbCommand.Parameters.Add(programParm);
            }

            if (Dept != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@BaseDept", Dept);
                dbCommand.Parameters.Add(programParm);
            }

            if (Prog != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@BaseProg", Prog);
                dbCommand.Parameters.Add(programParm);
            }

            if (Year != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@BaseYear", Year);
                dbCommand.Parameters.Add(programParm);
            }

            if (section != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@Sel_Section", section);
                dbCommand.Parameters.Add(programParm);
            }

            if (fund != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@Sel_Fund", fund);
                dbCommand.Parameters.Add(programParm);
            }

            if (site != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@Sel_Site", site);
                dbCommand.Parameters.Add(programParm);
            }

            if (QuesID != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@Sel_Ques_ID", QuesID);
                dbCommand.Parameters.Add(programParm);
            }

            if (QuesType != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@Sel_Ques_Type", QuesType);
                dbCommand.Parameters.Add(programParm);
            }

            if (userID != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@Userid", userID);
                dbCommand.Parameters.Add(programParm);
            }

            if (isAudit != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@Audit", isAudit);
                dbCommand.Parameters.Add(programParm);
            }

            if (HitOnly != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@ShowHit", HitOnly);
                dbCommand.Parameters.Add(programParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static DataSet GET_QUES_AUDIT_CONDITIONS(string Agency, string Dept, string Prog, string Year, string section, string fund, string site, string QuesID, string QuesType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_QUES_AUDIT_CONDITIONS]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 2400;

            if (Agency != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@BaseAgy", Agency);
                dbCommand.Parameters.Add(programParm);
            }

            if (Dept != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@BaseDept", Dept);
                dbCommand.Parameters.Add(programParm);
            }

            if (Prog != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@BaseProg", Prog);
                dbCommand.Parameters.Add(programParm);
            }

            if (Year != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@BaseYear", Year);
                dbCommand.Parameters.Add(programParm);
            }

            if (section != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@Sel_Section", section);
                dbCommand.Parameters.Add(programParm);
            }

            if (fund != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@Sel_Fund", fund);
                dbCommand.Parameters.Add(programParm);
            }

            if (site != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@Sel_Site", site);
                dbCommand.Parameters.Add(programParm);
            }

            if (QuesID != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@Sel_Ques_ID", QuesID);
                dbCommand.Parameters.Add(programParm);
            }

            if (QuesType != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@Sel_Ques_Type", QuesType);
                dbCommand.Parameters.Add(programParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        #endregion
    }
}
