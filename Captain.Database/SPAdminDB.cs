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
    public partial class SPAdminDB
    {

        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[CASESP0]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;
        #endregion

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_CASESP0(string SP_code, string SP_Desc, string Status, string BR_Code, string Validate_SW, string Lstc_Date, string Lstc_Opr, string Add_Date, string Add_Opr)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_CASESP0]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(SP_code))
            {
                SqlParameter sqlSP_code = new SqlParameter("@sp0_servicecode", int.Parse(SP_code));
                dbCommand.Parameters.Add(sqlSP_code);
            }
            if (!string.IsNullOrEmpty(SP_Desc))
            {
                SqlParameter sqlSP_Desc = new SqlParameter("@sp0_description", SP_Desc);
                dbCommand.Parameters.Add(sqlSP_Desc);
            }

            if (!string.IsNullOrEmpty(Status))
            {
                SqlParameter sqlSP_Status = new SqlParameter("@sp0_Active", Status);
                dbCommand.Parameters.Add(sqlSP_Status);
            }

            SqlParameter sqlBR_Code = new SqlParameter("@sp0_branch_code", BR_Code);
            dbCommand.Parameters.Add(sqlBR_Code);
            SqlParameter sqlSiteDeac = new SqlParameter("@sp0_branch_desc", null);
            dbCommand.Parameters.Add(sqlSiteDeac);
            SqlParameter sqlAppType = new SqlParameter("@sp0_statuses", null);
            dbCommand.Parameters.Add(sqlAppType);
            SqlParameter sqlSlot = new SqlParameter("@sp0_legals ", null);
            dbCommand.Parameters.Add(sqlSlot);
            SqlParameter SqlMin = new SqlParameter("@sp0_funds", null);
            dbCommand.Parameters.Add(SqlMin);

            SqlParameter SqlValidate_SW = new SqlParameter("@sp0_validated", Validate_SW);
            dbCommand.Parameters.Add(SqlValidate_SW);
            SqlParameter SqlLstc_Date = new SqlParameter("@sp0_date_lstc", Lstc_Date);
            dbCommand.Parameters.Add(SqlLstc_Date);
            SqlParameter SqlLstc_Opr = new SqlParameter("@sp0_lstc_operator", Lstc_Opr);
            dbCommand.Parameters.Add(SqlLstc_Opr);
            SqlParameter sqlAdd_Date = new SqlParameter("@sp0_date_add ", Add_Date);
            dbCommand.Parameters.Add(sqlAdd_Date);
            SqlParameter SqlAdd_Opr = new SqlParameter("@sp0_add_operator", Add_Opr);
            dbCommand.Parameters.Add(SqlAdd_Opr);


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_CASESP1(string SP_code, string Agy, string Dept, string Prog, string Mode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;
            SqlParameter sqlSP_code;
            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_CASESP1]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(SP_code))
            {
                /*SqlParameter*/
                sqlSP_code = new SqlParameter("@sp1_servicecode", int.Parse(SP_code));
                dbCommand.Parameters.Add(sqlSP_code);
            }
            SqlParameter sqlAgy = new SqlParameter("@sp1_agency", Agy);
            dbCommand.Parameters.Add(sqlAgy);
            SqlParameter sqlDept = new SqlParameter("@sp1_dept", Dept);
            dbCommand.Parameters.Add(sqlDept);
            SqlParameter sqlProg = new SqlParameter("@sp1_program", Prog);
            dbCommand.Parameters.Add(sqlProg);


            //Vikash:11/08/2022:: Commented for Hiearchy definition delete option issue
            //if (!string.IsNullOrEmpty(SP_code))
            //{
            sqlSP_code = new SqlParameter("@Mode", Mode);
            dbCommand.Parameters.Add(sqlSP_code);
            //}

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CASESP1_SerPLANS(string SP_code, string Agy, string Dept, string Prog, string UserID)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_CASESP1_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(SP_code))
            {
                SqlParameter sqlSP_code = new SqlParameter("@sp1_servicecode", int.Parse(SP_code));
                dbCommand.Parameters.Add(sqlSP_code);
            }
            if (!string.IsNullOrEmpty(Agy))
            {
                SqlParameter sqlAgy = new SqlParameter("@sp1_agency", Agy);
                dbCommand.Parameters.Add(sqlAgy);
            }
            if (!string.IsNullOrEmpty(Dept))
            {
                SqlParameter sqlDept = new SqlParameter("@sp1_dept", Dept);
                dbCommand.Parameters.Add(sqlDept);
            }
            if (!string.IsNullOrEmpty(Prog))
            {
                SqlParameter sqlProg = new SqlParameter("@sp1_program", Prog);
                dbCommand.Parameters.Add(sqlProg);
            }

            if (!string.IsNullOrEmpty(UserID))
            {
                SqlParameter sqlProg = new SqlParameter("@USERID", UserID);
                dbCommand.Parameters.Add(sqlProg);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_CTAPPRISEsurvey(string county, string zipcode, string agency, string dept, string Prog, string year, string FromDate, string ToDate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[BROWSE_CTAPPRISEsurvey]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1200;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            SqlParameter sql_county = new SqlParameter("@COUNTY", county);
            dbCommand.Parameters.Add(sql_county);
            SqlParameter sql_zipcode = new SqlParameter("@ZIP_CODE", zipcode);
            dbCommand.Parameters.Add(sql_zipcode);
            SqlParameter sql_agency = new SqlParameter("@AGENCY", agency);
            dbCommand.Parameters.Add(sql_agency);
            SqlParameter sql_dept = new SqlParameter("@DEPT", dept);
            dbCommand.Parameters.Add(sql_dept);

            SqlParameter sql_Prog = new SqlParameter("@PROGRAM", Prog);
            dbCommand.Parameters.Add(sql_Prog);

            SqlParameter sql_year = new SqlParameter("@YEAR", year);
            dbCommand.Parameters.Add(sql_year);

            SqlParameter sql_Fdate = new SqlParameter("@FromDate", FromDate);
            dbCommand.Parameters.Add(sql_Fdate);

            SqlParameter sql_TDate = new SqlParameter("@ToDate", ToDate);
            dbCommand.Parameters.Add(sql_TDate);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_Tribeware(string agency, string dept, string Prog, string year, string county, string zipcode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[TMSB0026]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1200;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();


            SqlParameter sql_agency = new SqlParameter("@AGENCY", agency);
            dbCommand.Parameters.Add(sql_agency);
            SqlParameter sql_dept = new SqlParameter("@DEPT", dept);
            dbCommand.Parameters.Add(sql_dept);

            SqlParameter sql_Prog = new SqlParameter("@PROGRAM", Prog);
            dbCommand.Parameters.Add(sql_Prog);

            SqlParameter sql_year = new SqlParameter("@YEAR", year);
            dbCommand.Parameters.Add(sql_year);

            SqlParameter sql_county = new SqlParameter("@COUNTY", county);
            dbCommand.Parameters.Add(sql_county);
            SqlParameter sql_zipcode = new SqlParameter("@ZIP_CODE", zipcode);
            dbCommand.Parameters.Add(sql_zipcode);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_CASESPM(List<SqlParameter> sqlParamList)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_CASESPM]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_CASECONT(List<SqlParameter> sqlParamList)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_CASECONT]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        // Yeswanth
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
            dbCommand.CommandTimeout = 8400;


            if (Table_Name.Contains("CaseDemographics_On_") ||
                Table_Name.Contains("Get_PerMeasures_Counts"))
                dbCommand.CommandTimeout = 7200;

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetINKINDTLS(List<SqlParameter> sqlParamList, string SPName)
        {
            DataSet ds;
            Database db;
            //string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            dbCommand = db.GetStoredProcCommand(SPName);
            dbCommand.CommandTimeout = 8400;
            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_Attenpost_Asof_DateRange(List<SqlParameter> sqlParamList)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Get_Attenpost_Asof_DateRange]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_CASEACT(List<SqlParameter> sqlParamList)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_CASEACT]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_CASEMS(List<SqlParameter> sqlParamList)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_CASEMS]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_CASEMSOBO(List<SqlParameter> sqlParamList)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_CASEMSOBO]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_CAOBO(List<SqlParameter> sqlParamList)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_CAOBO]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_CASEREF(List<SqlParameter> sqlParamList)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_CASEREF]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_AGCYPART(List<SqlParameter> sqlParamList)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_AGCYPART_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_AGCYPART_REP(List<SqlParameter> sqlParamList)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_AGCYPART_REP_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_AGCYBoutique(List<SqlParameter> sqlParamList)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_AGCYBOUTIQUE_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_AGCYREP(List<SqlParameter> sqlParamList)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_AGCYREP_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_AGYSUBLOC(List<SqlParameter> sqlParamList)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_AGYSUBLOC_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_AGYSERVICES(List<SqlParameter> sqlParamList)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_AGCYSERVICES_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_CASESPM2(List<SqlParameter> sqlParamList)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_CASESPM2]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_CASESP2(string SP_code, string BR_Code, string CAMS_Type, string CA_MC_Code)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_CASESP2]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 2400;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(SP_code))
            {
                SqlParameter sqlSP_code = new SqlParameter("@sp2_serviceplan", int.Parse(SP_code));
                dbCommand.Parameters.Add(sqlSP_code);
            }
            SqlParameter sqlBR_Code = new SqlParameter("@sp2_branch", BR_Code);
            dbCommand.Parameters.Add(sqlBR_Code);
            SqlParameter sqlDept = new SqlParameter("@sp2_orig_grp", null);
            dbCommand.Parameters.Add(sqlDept);
            SqlParameter sqlCAMS_Type = new SqlParameter("@sp2_type", CAMS_Type);
            dbCommand.Parameters.Add(sqlCAMS_Type);
            SqlParameter sqlCA_MC_Code = new SqlParameter("@sp2_cams_code", CA_MC_Code);
            dbCommand.Parameters.Add(sqlCA_MC_Code);
            SqlParameter sqlProg = new SqlParameter("@sp2_curr_grp", null);
            dbCommand.Parameters.Add(sqlProg);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_CASESP2(string SP_code, string BR_Code, string CAMS_Type, string CA_MC_Code)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Get_CASESP2]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 2400;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(SP_code))
            {
                SqlParameter sqlSP_code = new SqlParameter("@sp2_serviceplan", int.Parse(SP_code));
                dbCommand.Parameters.Add(sqlSP_code);
            }
            SqlParameter sqlBR_Code = new SqlParameter("@sp2_branch", BR_Code);
            dbCommand.Parameters.Add(sqlBR_Code);
            SqlParameter sqlDept = new SqlParameter("@sp2_orig_grp", null);
            dbCommand.Parameters.Add(sqlDept);
            SqlParameter sqlCAMS_Type = new SqlParameter("@sp2_type", CAMS_Type);
            dbCommand.Parameters.Add(sqlCAMS_Type);
            SqlParameter sqlCA_MC_Code = new SqlParameter("@sp2_cams_code", CA_MC_Code);
            dbCommand.Parameters.Add(sqlCA_MC_Code);
            SqlParameter sqlProg = new SqlParameter("@sp2_curr_grp", null);
            dbCommand.Parameters.Add(sqlProg);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_CASESP2(string SP_code, string BR_Code, string CAMS_Type, string CA_MC_Code, string strfiltercode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_CASESP2]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 2400;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(SP_code))
            {
                SqlParameter sqlSP_code = new SqlParameter("@sp2_serviceplan", int.Parse(SP_code));
                dbCommand.Parameters.Add(sqlSP_code);
            }
            SqlParameter sqlBR_Code = new SqlParameter("@sp2_branch", BR_Code);
            dbCommand.Parameters.Add(sqlBR_Code);
            SqlParameter sqlDept = new SqlParameter("@sp2_orig_grp", null);
            dbCommand.Parameters.Add(sqlDept);
            SqlParameter sqlCAMS_Type = new SqlParameter("@sp2_type", CAMS_Type);
            dbCommand.Parameters.Add(sqlCAMS_Type);
            SqlParameter sqlCA_MC_Code = new SqlParameter("@sp2_cams_code", CA_MC_Code);
            dbCommand.Parameters.Add(sqlCA_MC_Code);
            SqlParameter sqlProg = new SqlParameter("@sp2_curr_grp", null);
            dbCommand.Parameters.Add(sqlProg);
            if (!string.IsNullOrEmpty(strfiltercode))
            {
                SqlParameter sqlSP_filtercode = new SqlParameter("@filtertype", strfiltercode);
                dbCommand.Parameters.Add(sqlSP_filtercode);
            }


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_CAMAST(string Sort_By, string CA_Code, string CA_Desc, string CA_Active_SW)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_CAMAST]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlSP_code = new SqlParameter("@SortCol", Sort_By);
            dbCommand.Parameters.Add(sqlSP_code);
            SqlParameter sqlBR_Code = new SqlParameter("@CA_CODE", CA_Code);
            dbCommand.Parameters.Add(sqlBR_Code);
            SqlParameter sqlCA_Desc = new SqlParameter("@CA_DESC", CA_Desc);
            dbCommand.Parameters.Add(sqlCA_Desc);
            SqlParameter sqlCA_Active_SW = new SqlParameter("@CA_ACTIVE", CA_Active_SW);
            dbCommand.Parameters.Add(sqlCA_Active_SW);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_CAPRICES(string CA_Code, string FDate, string TDate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_BROWSE_CAPRICES]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlBR_Code = new SqlParameter("@CAP_CODE", CA_Code);
            dbCommand.Parameters.Add(sqlBR_Code);
            if (!string.IsNullOrEmpty(FDate.Trim()))
            {
                SqlParameter sqlCA_Desc = new SqlParameter("@CAP_FDATE", FDate);
                dbCommand.Parameters.Add(sqlCA_Desc);
            }
            if (!string.IsNullOrEmpty(TDate.Trim()))
            {
                SqlParameter sqlCA_Active_SW = new SqlParameter("@CAP_LDATE", TDate);
                dbCommand.Parameters.Add(sqlCA_Active_SW);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_MSMAST(string Sort_By, string MS_Code, string MS_Desc, string Act_ststus, string Type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_MSMAST]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlSP_code = new SqlParameter("@SortCol", Sort_By);
            dbCommand.Parameters.Add(sqlSP_code);
            SqlParameter sqlBR_Code = new SqlParameter("@MS_CODE", MS_Code);
            dbCommand.Parameters.Add(sqlBR_Code);
            SqlParameter sqlDept = new SqlParameter("@MS_DESC", MS_Desc);
            dbCommand.Parameters.Add(sqlDept);
            SqlParameter sqlCAMS_Type = new SqlParameter("@MS_ACTIVE", Act_ststus);
            dbCommand.Parameters.Add(sqlCAMS_Type);
            SqlParameter sqlCA_MC_Code = new SqlParameter("@MS_TYPE", Type);
            dbCommand.Parameters.Add(sqlCA_MC_Code);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CaseACTMS_AppCount(string Agency, string Dept, string Prog, string ServicePlan)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CASEMSACT_APPS_COUNT]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(Agency))
            {
                SqlParameter sqlSP_code = new SqlParameter("@AGENCY", Agency);
                dbCommand.Parameters.Add(sqlSP_code);
            }
            SqlParameter sqlBR_Code = new SqlParameter("@DEPT", Dept);
            dbCommand.Parameters.Add(sqlBR_Code);
            SqlParameter sqlDept = new SqlParameter("@PROG", Prog);
            dbCommand.Parameters.Add(sqlDept);
            SqlParameter sqlCAMS_Type = new SqlParameter("@SERVICEPLAN", ServicePlan);
            dbCommand.Parameters.Add(sqlCAMS_Type);
            //SqlParameter sqlCA_MC_Code = new SqlParameter("@sp2_cams_code", CA_MC_Code);
            //dbCommand.Parameters.Add(sqlCA_MC_Code);
            //SqlParameter sqlProg = new SqlParameter("@sp2_curr_grp", null);
            //dbCommand.Parameters.Add(sqlProg);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        ////////public static bool UpdateCASESP2(List<SqlParameter> sqlParamList)
        ////////{
        ////////    _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateCASESP2");
        ////////    _dbCommand.CommandTimeout = 1200;
        ////////    _dbCommand.Parameters.Clear();
        ////////    foreach (SqlParameter sqlPar in sqlParamList)
        ////////    {
        ////////        _dbCommand.Parameters.Add(sqlPar);
        ////////    }
        ////////    return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        ////////}

        ////public static DataSet UpdateCASESP2(List<SqlParameter> sqlParamList)
        ////{
        ////    Database db;
        ////    DbCommand dbCommand;

        ////    db = DatabaseFactory.CreateDatabase();
        ////    dbCommand = db.GetStoredProcCommand("[dbo].[UpdateCASESP2]");
        ////    foreach (SqlParameter sqlPar in sqlParamList)
        ////        dbCommand.Parameters.Add(sqlPar);

        ////    return db.ExecuteDataSet(dbCommand);
        ////}

        //public static DataSet UpdateCASESP0_Exception(List<SqlParameter> sqlParamList)
        //{
        //    Database db;
        //    DbCommand dbCommand;

        //    db = DatabaseFactory.CreateDatabase();
        //    dbCommand = db.GetStoredProcCommand("[dbo].[UpdateCASESP0_Exception]");
        //    foreach (SqlParameter sqlPar in sqlParamList)
        //        dbCommand.Parameters.Add(sqlPar);

        //    return db.ExecuteDataSet(dbCommand);
        //}

        ////public static DataSet UpdateCASESP0(List<SqlParameter> sqlParamList)
        ////{
        ////    Database db;
        ////    DbCommand dbCommand;

        ////    db = DatabaseFactory.CreateDatabase();
        ////    dbCommand = db.GetStoredProcCommand("[dbo].[UpdateCASESP0]");
        ////    foreach (SqlParameter sqlPar in sqlParamList)
        ////        dbCommand.Parameters.Add(sqlPar);

        ////    DataSet ds= db.ExecuteDataSet(dbCommand);

        ////    //Common_SqlErr_Entity 

        ////    //if (ds != null && ds.Tables[0].Rows.Count > 0)
        ////    //{
        ////    //    foreach (DataRow row in ds.Tables[0].Rows)
        ////    //        CASESPM2Profile.Add(new ACTREFSEntity(row));
        ////    //}


        ////    return db.ExecuteDataSet(dbCommand);
        ////}


        //////public static bool UpdateCASESP0(List<SqlParameter> sqlParamList)
        //////{
        //////    _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateCASESP0");
        //////    _dbCommand.CommandTimeout = 1200;
        //////    _dbCommand.Parameters.Clear();
        //////    foreach (SqlParameter sqlPar in sqlParamList)
        //////    {
        //////        _dbCommand.Parameters.Add(sqlPar);
        //////    }

        //////    return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        //////}

        //public static DataSet UpdateCASESP1(List<SqlParameter> sqlParamList)
        //{
        //    Database db;
        //    DbCommand dbCommand;

        //    db = DatabaseFactory.CreateDatabase();
        //    dbCommand = db.GetStoredProcCommand("[dbo].[UpdateCASESP1]");
        //    foreach (SqlParameter sqlPar in sqlParamList)
        //        dbCommand.Parameters.Add(sqlPar);

        //    return db.ExecuteDataSet(dbCommand);
        //}

        //////public static bool UpdateCASESP1(List<SqlParameter> sqlParamList)
        //////{
        //////    _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateCASESP1");
        //////    _dbCommand.CommandTimeout = 1200;
        //////    _dbCommand.Parameters.Clear();
        //////    foreach (SqlParameter sqlPar in sqlParamList)
        //////    {
        //////        _dbCommand.Parameters.Add(sqlPar);
        //////    }
        //////    return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        //////}


        //////public static bool UpdateCASESPM(List<SqlParameter> sqlParamList)
        //////{
        //////    _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateCASESPM");
        //////    _dbCommand.CommandTimeout = 1200;
        //////    _dbCommand.Parameters.Clear();
        //////    foreach (SqlParameter sqlPar in sqlParamList)
        //////    {
        //////        _dbCommand.Parameters.Add(sqlPar);
        //////    }
        //////    return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        //////}



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool Delete_CASESP0(string SP_Code)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.Delete_CASESP0");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlSP_Code = new SqlParameter("@SP_Code", SP_Code);
            _dbCommand.Parameters.Add(sqlSP_Code);

            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        ////////public static bool UpdateCASECONT(List<SqlParameter> sqlParamList)
        ////////{
        ////////    _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateCASECONT");
        ////////    _dbCommand.CommandTimeout = 1200;
        ////////    _dbCommand.Parameters.Clear();
        ////////    foreach (SqlParameter sqlPar in sqlParamList)
        ////////    {
        ////////        _dbCommand.Parameters.Add(sqlPar);
        ////////    }
        ////////    return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        ////////}

        //Yeswanth

        public static bool Update_Sel_Table(List<SqlParameter> sqlParamList, string SP_Name, out string Sql_Reslut_Msg)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand(SP_Name);
            _dbCommand.CommandTimeout = 2400;
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

        //////public static bool UpdateCASEACT(List<SqlParameter> sqlParamList)
        //////{
        //////    _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateCASEACT");
        //////    _dbCommand.CommandTimeout = 1200;
        //////    _dbCommand.Parameters.Clear();
        //////    foreach (SqlParameter sqlPar in sqlParamList)
        //////    {
        //////        _dbCommand.Parameters.Add(sqlPar);
        //////    }
        //////    return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        //////}

        //////public static bool UpdateCASEMS(List<SqlParameter> sqlParamList)
        //////{
        //////    _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateCASEMS");
        //////    _dbCommand.CommandTimeout = 1200;
        //////    _dbCommand.Parameters.Clear();
        //////    foreach (SqlParameter sqlPar in sqlParamList)
        //////    {
        //////        _dbCommand.Parameters.Add(sqlPar);
        //////    }
        //////    return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        //////}


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool Delete_CASESP2(int SP_Code, string Branch, int Org_group, string Type, string CAMA_Code)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.Delete_CASESP2");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlSP_Code = new SqlParameter("@sp2_serviceplan", SP_Code);
            _dbCommand.Parameters.Add(sqlSP_Code);
            SqlParameter sqlBranch = new SqlParameter("@sp2_branch", Branch);
            _dbCommand.Parameters.Add(sqlBranch);
            SqlParameter sqlOrg_group = new SqlParameter("@sp2_orig_grp", Org_group);
            _dbCommand.Parameters.Add(sqlOrg_group);
            SqlParameter sqlType = new SqlParameter("@sp2_type", Type);
            _dbCommand.Parameters.Add(sqlType);
            SqlParameter sqlCAMA_Code = new SqlParameter("@sp2_cams_code", CAMA_Code);
            _dbCommand.Parameters.Add(sqlCAMA_Code);

            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool Delete_CaseRefs(string Code)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.Delete_CASEREFS");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlSP_Code = new SqlParameter("@CASEREFS_CODE", Code);
            _dbCommand.Parameters.Add(sqlSP_Code);

            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsUpdelINKINDDTLS(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSUPDEL_INKINDDTLS");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        //****************************************CAMast***********************************************
        public static bool InsertUpdateCAMAST(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateCAMAST");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        public static bool InsertUpdateCAPrices(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_CAPRICES_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool DeleteCAMAST(string CA_Code, out string msg)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteCAMAST");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlCA_Code = new SqlParameter("@CACODE", CA_Code);
            _dbCommand.Parameters.Add(sqlCA_Code);

            SqlParameter sqlmsg = new SqlParameter("@Msg", SqlDbType.VarChar, 50);
            sqlmsg.Direction = ParameterDirection.Output;
            _dbCommand.Parameters.Add(sqlmsg);
            bool boolsuccess = _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
            msg = sqlmsg.Value.ToString();
            return boolsuccess;
        }



        //*******************************************MSMast********************************************
        public static bool InsertUpdateMSMAST(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateMSMAST");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool DeleteMSMAST(string MS_Code, out string msg)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteMSMAST");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlMS_Code = new SqlParameter("@MSCODE", MS_Code);
            _dbCommand.Parameters.Add(sqlMS_Code);

            SqlParameter sqlmsg = new SqlParameter("@Msg", SqlDbType.VarChar, 50);
            sqlmsg.Direction = ParameterDirection.Output;
            _dbCommand.Parameters.Add(sqlmsg);
            bool boolsuccess = _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
            msg = sqlmsg.Value.ToString();
            return boolsuccess;
        }

        //*********************************CSB Demographic*************************
        public static bool InsertUpdateCSB4ASOC(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateCSB4ASOC");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }
        //*********************************RNG Demographic*************************
        public static bool InsertUpdateRNG4ASOC(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateRNG4ASOC");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool DeleteCSB4ASOC(string RepType, string Rep_Code)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteCSB4ASOC");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlRepType = new SqlParameter("@CSB4ASOC_CATG_CODE", RepType);
            _dbCommand.Parameters.Add(sqlRepType);

            SqlParameter sqlRep_Code = new SqlParameter("@CSB4ASOC_DEMO_CODE", Rep_Code);
            _dbCommand.Parameters.Add(sqlRep_Code);

            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool DeleteRNG4ASOC(string RepType, string Rep_Code)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteRNG4ASOC");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlRepType = new SqlParameter("@RNG4ASOC_CATG_CODE", RepType);
            _dbCommand.Parameters.Add(sqlRepType);

            SqlParameter sqlRep_Code = new SqlParameter("@RNG4ASOC_DEMO_CODE", Rep_Code);
            _dbCommand.Parameters.Add(sqlRep_Code);

            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_CSB4Cat()
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[BROWSE_CSB4CATG]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_RNG4CATG()
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[BROWSE_RNG4CATG]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_CSASS(string Cat_Cd, string Demo_Cd)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[BROWSE_CSB4ASOC]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlCol = new SqlParameter("@CSB4ASOC_CATG_CODE", Cat_Cd);
            dbCmd.Parameters.Add(sqlCol);

            SqlParameter sqlCode = new SqlParameter("@CSB4ASOC_DEMO_CODE", Demo_Cd);
            dbCmd.Parameters.Add(sqlCode);

            //SqlParameter sqlActive = new SqlParameter("@CSB4CATG_ACTIVE", Active);
            //dbCmd.Parameters.Add(sqlActive);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_SP0Funds(string Agency, string Dept, string Prog, string Serviceplan, string UserID)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[CAPS_SP0FUNDS_AGENCY]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(Agency.Trim()))
            {
                SqlParameter sqlCol = new SqlParameter("@AGENCY", Agency);
                dbCmd.Parameters.Add(sqlCol);
            }
            if (!string.IsNullOrEmpty(Dept.Trim()))
            {
                SqlParameter sqlCol = new SqlParameter("@DEPT", Dept);
                dbCmd.Parameters.Add(sqlCol);
            }
            if (!string.IsNullOrEmpty(Prog.Trim()))
            {
                SqlParameter sqlCol = new SqlParameter("@PROGRAM", Prog);
                dbCmd.Parameters.Add(sqlCol);
            }
            if (!string.IsNullOrEmpty(Serviceplan.Trim()))
            {
                SqlParameter sqlCol = new SqlParameter("@SERVICEPLAN", Serviceplan);
                dbCmd.Parameters.Add(sqlCol);
            }

            if (!string.IsNullOrEmpty(UserID.Trim()))
            {
                SqlParameter sqlCol = new SqlParameter("@USERID", UserID);
                dbCmd.Parameters.Add(sqlCol);
            }

            //SqlParameter sqlCode = new SqlParameter("@CSB4ASOC_DEMO_CODE", Demo_Cd);
            //dbCmd.Parameters.Add(sqlCode);

            //SqlParameter sqlActive = new SqlParameter("@CSB4CATG_ACTIVE", Active);
            //dbCmd.Parameters.Add(sqlActive);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_RNGASS(string Cat_Cd, string Demo_Cd)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[BROWSE_RNG4ASOC]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlCol = new SqlParameter("@RNG4ASOC_CATG_CODE", Cat_Cd);
            dbCmd.Parameters.Add(sqlCol);

            SqlParameter sqlCode = new SqlParameter("@RNG4ASOC_DEMO_CODE", Demo_Cd);
            dbCmd.Parameters.Add(sqlCode);

            //SqlParameter sqlActive = new SqlParameter("@CSB4CATG_ACTIVE", Active);
            //dbCmd.Parameters.Add(sqlActive);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        //******************************************Performance Measures************************************
        public static bool InsertUpdateCSB14GRP(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateCSB14GRP");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertUpdateRNGGRP(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateRNGGRP");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }
        public static bool InsertUpdateRNGSRGRP(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateRNGSRGRP");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_CSB14GRP(string RefFdt, string RefTdt, string GrpCd, string TblCd, string Desc)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_CSB14GRP]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlFdt = new SqlParameter("@csb14grp_ref_fdate", RefFdt);
            dbCmd.Parameters.Add(sqlFdt);
            SqlParameter sqlTdt = new SqlParameter("@csb14grp_ref_tdate", RefTdt);
            dbCmd.Parameters.Add(sqlTdt);
            SqlParameter sqlGrpCd = new SqlParameter("@csb14grp_group_code", GrpCd);
            dbCmd.Parameters.Add(sqlGrpCd);
            SqlParameter sqlTblCd = new SqlParameter("@csb14grp_table_code", TblCd);
            dbCmd.Parameters.Add(sqlTblCd);
            SqlParameter sqlDesc = new SqlParameter("@csb14grp_desc", Desc);
            dbCmd.Parameters.Add(sqlDesc);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_RNGGRP(string RefFdt, string Agency, string GrpCd, string TblCd, string Desc, string UserID, string AdminAgency)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_RNGGRP]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlFdt = new SqlParameter("@RNGgrp_CODE", RefFdt);
            dbCmd.Parameters.Add(sqlFdt);
            SqlParameter sqlTdt = new SqlParameter("@RNGGRP_AGENCY", Agency);
            dbCmd.Parameters.Add(sqlTdt);
            SqlParameter sqlGrpCd = new SqlParameter("@RNGgrp_group_code", GrpCd);
            dbCmd.Parameters.Add(sqlGrpCd);
            SqlParameter sqlTblCd = new SqlParameter("@RNGgrp_table_code", TblCd);
            dbCmd.Parameters.Add(sqlTblCd);
            SqlParameter sqlDesc = new SqlParameter("@RNGgrp_desc", Desc);
            dbCmd.Parameters.Add(sqlDesc);

            if (!string.IsNullOrEmpty(UserID))
            {
                SqlParameter sqlUser = new SqlParameter("@UserID", UserID);
                dbCmd.Parameters.Add(sqlUser);
            }

            if (!string.IsNullOrEmpty(AdminAgency))
            {
                SqlParameter sqlUser = new SqlParameter("@Admin_Agency", AdminAgency);
                dbCmd.Parameters.Add(sqlUser);
            }

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_RNGSRGRP(string RefFdt, string Agency, string GrpCd, string TblCd, string Desc, string UserID, string AdminAgency)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_RNGSRGRP]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlFdt = new SqlParameter("@RNGSRgrp_CODE", RefFdt);
            dbCmd.Parameters.Add(sqlFdt);
            SqlParameter sqlTdt = new SqlParameter("@RNGSRGRP_AGENCY", Agency);
            dbCmd.Parameters.Add(sqlTdt);
            SqlParameter sqlGrpCd = new SqlParameter("@RNGSRgrp_group_code", GrpCd);
            dbCmd.Parameters.Add(sqlGrpCd);
            SqlParameter sqlTblCd = new SqlParameter("@RNGSRgrp_table_code", TblCd);
            dbCmd.Parameters.Add(sqlTblCd);
            SqlParameter sqlDesc = new SqlParameter("@RNGSRgrp_desc", Desc);
            dbCmd.Parameters.Add(sqlDesc);

            if (!string.IsNullOrEmpty(UserID))
            {
                SqlParameter sqlUser = new SqlParameter("@UserID", UserID);
                dbCmd.Parameters.Add(sqlUser);
            }

            if (!string.IsNullOrEmpty(AdminAgency))
            {
                SqlParameter sqlUser = new SqlParameter("@Admin_Agency", AdminAgency);
                dbCmd.Parameters.Add(sqlUser);
            }

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool InsertUpdateCSB14RA(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertupdateCSB14RA");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool InsertUpdateRNGRA(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertupdateRNGRA");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool InsertUpdateRNGGA(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertupdateRNGGA");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool InsertUpdateRNGGAHIE(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_RNGGOALH_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool InsertUpdateRNGSRGA(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertupdateRNGSRGA");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool InsertUpdateRNGSRGAHIE(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_RNGSRGOALH_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        public static bool InsertUpdateDelCMBDC(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_CMBDC_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_CSB16()
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_CSB16DTR]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool DeleteCSB14GRP(string Fdate, string Tdate, string Grp_cd, string TblCd, string Type, out string msg)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteCSB14Grp");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlFdate = new SqlParameter("@RefFdate", Fdate);
            _dbCommand.Parameters.Add(sqlFdate);

            SqlParameter sqlTdate = new SqlParameter("@RefTdate", Tdate);
            _dbCommand.Parameters.Add(sqlTdate);

            SqlParameter sqlGrp_cd = new SqlParameter("@Group_cd", Grp_cd);
            _dbCommand.Parameters.Add(sqlGrp_cd);

            SqlParameter sqlTblCd = new SqlParameter("@Table_cd", TblCd);
            _dbCommand.Parameters.Add(sqlTblCd);

            SqlParameter sqlType = new SqlParameter("@Type", Type);
            _dbCommand.Parameters.Add(sqlType);

            SqlParameter sqlmsg = new SqlParameter("@Msg", SqlDbType.VarChar, 50);
            sqlmsg.Direction = ParameterDirection.Output;
            _dbCommand.Parameters.Add(sqlmsg);

            bool boolsuccess = _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
            msg = sqlmsg.Value.ToString();
            return boolsuccess;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool DeleteRNGGRP(string Code, string Agency, string Grp_cd, string TblCd, string Type, out string msg)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteRNGGrp");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlFdate = new SqlParameter("@CODE", Code);
            _dbCommand.Parameters.Add(sqlFdate);

            SqlParameter sqlTdate = new SqlParameter("@AGENCY", Agency);
            _dbCommand.Parameters.Add(sqlTdate);

            SqlParameter sqlGrp_cd = new SqlParameter("@Group_cd", Grp_cd);
            _dbCommand.Parameters.Add(sqlGrp_cd);

            SqlParameter sqlTblCd = new SqlParameter("@Table_cd", TblCd);
            _dbCommand.Parameters.Add(sqlTblCd);

            SqlParameter sqlType = new SqlParameter("@Type", Type);
            _dbCommand.Parameters.Add(sqlType);

            SqlParameter sqlmsg = new SqlParameter("@Msg", SqlDbType.VarChar, 50);
            sqlmsg.Direction = ParameterDirection.Output;
            _dbCommand.Parameters.Add(sqlmsg);

            bool boolsuccess = _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
            msg = sqlmsg.Value.ToString();
            return boolsuccess;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool DeleteRNGSRGRP(string Code, string Agency, string Grp_cd, string TblCd, string Type, out string msg)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteRNGSRGrp");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlFdate = new SqlParameter("@CODE", Code);
            _dbCommand.Parameters.Add(sqlFdate);

            SqlParameter sqlTdate = new SqlParameter("@AGENCY", Agency);
            _dbCommand.Parameters.Add(sqlTdate);

            SqlParameter sqlGrp_cd = new SqlParameter("@Group_cd", Grp_cd);
            _dbCommand.Parameters.Add(sqlGrp_cd);

            SqlParameter sqlTblCd = new SqlParameter("@Table_cd", TblCd);
            _dbCommand.Parameters.Add(sqlTblCd);

            SqlParameter sqlType = new SqlParameter("@Type", Type);
            _dbCommand.Parameters.Add(sqlType);

            SqlParameter sqlmsg = new SqlParameter("@Msg", SqlDbType.VarChar, 50);
            sqlmsg.Direction = ParameterDirection.Output;
            _dbCommand.Parameters.Add(sqlmsg);

            bool boolsuccess = _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
            msg = sqlmsg.Value.ToString();
            return boolsuccess;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_CSB14RA(string fdate, string Tdate, string grpcd, string CntCode)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_CSB14RA]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlFdt = new SqlParameter("@CSB14RA_REF_FDATE", fdate);
            dbCmd.Parameters.Add(sqlFdt);

            SqlParameter sqlTdt = new SqlParameter("@CSB14RA_REF_TDATE", Tdate);
            dbCmd.Parameters.Add(sqlTdt);

            SqlParameter sqlGrpCode = new SqlParameter("@CSB14RA_GROUP_CODE", grpcd);
            dbCmd.Parameters.Add(sqlGrpCode);

            SqlParameter sqlCntCode = new SqlParameter("@CSB14RA_COUNT_CODE", CntCode);
            dbCmd.Parameters.Add(sqlCntCode);
            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_RNGRA(string fdate, string grpcd, string CntCode)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_RNGRA]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlFdt = new SqlParameter("@RNGRA_CODE", fdate);
            dbCmd.Parameters.Add(sqlFdt);

            //SqlParameter sqlTdt = new SqlParameter("@RNGRA_REF_TDATE", Tdate);
            //dbCmd.Parameters.Add(sqlTdt);

            SqlParameter sqlGrpCode = new SqlParameter("@RNGRA_GROUP_CODE", grpcd);
            dbCmd.Parameters.Add(sqlGrpCode);

            SqlParameter sqlCntCode = new SqlParameter("@RNGRA_COUNT_CODE", CntCode);
            dbCmd.Parameters.Add(sqlCntCode);
            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_RNGGA(string Maincode, string Agency, string grpcd, string TableCode, string CntCode)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_RNGGA]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlFdt = new SqlParameter("@RNGGA_CODE", Maincode);
            dbCmd.Parameters.Add(sqlFdt);

            if (TableCode != string.Empty)
            {
                SqlParameter sqlTdt = new SqlParameter("@RNGGA_TABLE_CODE", TableCode);
                dbCmd.Parameters.Add(sqlTdt);
            }
            if (Agency != string.Empty)
            {
                SqlParameter sqlTdt = new SqlParameter("@RNGGA_AGENCY", Agency);
                dbCmd.Parameters.Add(sqlTdt);
            }
            if (grpcd != string.Empty)
            {
                SqlParameter sqlGrpCode = new SqlParameter("@RNGGA_GROUP_CODE", grpcd);
                dbCmd.Parameters.Add(sqlGrpCode);
            }
            if (CntCode != string.Empty)
            {
                SqlParameter sqlCntCode = new SqlParameter("@RNGGA_COUNT_CODE", CntCode);
                dbCmd.Parameters.Add(sqlCntCode);
            }
            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_RNGGOALH(string Maincode, string Agency, string grpcd, string TableCode, string GoalCode)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[CAPS_RNGGOALH_BROWSE]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlFdt = new SqlParameter("@RNGGAH_CODE", Maincode);
            dbCmd.Parameters.Add(sqlFdt);

            if (TableCode != string.Empty)
            {
                SqlParameter sqlTdt = new SqlParameter("@RNGGAH_TBL_CODE", TableCode);
                dbCmd.Parameters.Add(sqlTdt);
            }
            if (Agency != string.Empty)
            {
                SqlParameter sqlTdt = new SqlParameter("@RNGGAH_AGENCY", Agency);
                dbCmd.Parameters.Add(sqlTdt);
            }
            if (grpcd != string.Empty)
            {
                SqlParameter sqlGrpCode = new SqlParameter("@RNGGAH_GRP_CODE", grpcd);
                dbCmd.Parameters.Add(sqlGrpCode);
            }
            if (GoalCode != string.Empty)
            {
                SqlParameter sqlCntCode = new SqlParameter("@RNGGAH_GOAL_CODE", GoalCode);
                dbCmd.Parameters.Add(sqlCntCode);
            }
            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_RNGSRGA(string fdate, string Agency, string grpcd, string TableCode, string CntCode)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_RNGSRGA]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (fdate != string.Empty)
            {
                SqlParameter sqlFdt = new SqlParameter("@RNGSRGA_CODE", fdate);
                dbCmd.Parameters.Add(sqlFdt);
            }

            if (Agency != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@RNGSRGA_AGENCY", Agency);
                dbCmd.Parameters.Add(sqlAgency);
            }
            if (TableCode != string.Empty)
            {
                SqlParameter sqlTdt = new SqlParameter("@RNGSRGA_TABLE_CODE", TableCode);
                dbCmd.Parameters.Add(sqlTdt);
            }
            if (grpcd != string.Empty)
            {
                SqlParameter sqlGrpCode = new SqlParameter("@RNGSRGA_GROUP_CODE", grpcd);
                dbCmd.Parameters.Add(sqlGrpCode);
            }
            if (CntCode != string.Empty)
            {
                SqlParameter sqlCntCode = new SqlParameter("@RNGSRGA_COUNT_CODE", CntCode);
                dbCmd.Parameters.Add(sqlCntCode);
            }
            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_RNGSRGOALH(string Maincode, string Agency, string grpcd, string TableCode, string GoalCode)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[CAPS_RNGSRGOALH_BROWSE]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlFdt = new SqlParameter("@RNGSRGAH_CODE", Maincode);
            dbCmd.Parameters.Add(sqlFdt);

            if (TableCode != string.Empty)
            {
                SqlParameter sqlTdt = new SqlParameter("@RNGSRGAH_TBL_CODE", TableCode);
                dbCmd.Parameters.Add(sqlTdt);
            }
            if (Agency != string.Empty)
            {
                SqlParameter sqlTdt = new SqlParameter("@RNGSRGAH_AGENCY", Agency);
                dbCmd.Parameters.Add(sqlTdt);
            }
            if (grpcd != string.Empty)
            {
                SqlParameter sqlGrpCode = new SqlParameter("@RNGSRGAH_GRP_CODE", grpcd);
                dbCmd.Parameters.Add(sqlGrpCode);
            }
            if (GoalCode != string.Empty)
            {
                SqlParameter sqlCntCode = new SqlParameter("@RNGSRGAH_GOAL_CODE", GoalCode);
                dbCmd.Parameters.Add(sqlCntCode);
            }
            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool DeleteCSB14RA(string Fdate, string Tdate, string Grp_cd, string CntCd)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteCSB14RA");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlFdate = new SqlParameter("@CSB14RA_REF_FDATE", Fdate);
            _dbCommand.Parameters.Add(sqlFdate);

            SqlParameter sqlTdate = new SqlParameter("@CSB14RA_REF_TDATE", Tdate);
            _dbCommand.Parameters.Add(sqlTdate);

            SqlParameter sqlGrp_cd = new SqlParameter("@CSB14RA_GROUP_CODE", Grp_cd);
            _dbCommand.Parameters.Add(sqlGrp_cd);

            SqlParameter sqlTblCd = new SqlParameter("@CSB14RA_COUNT_CODE", CntCd);
            _dbCommand.Parameters.Add(sqlTblCd);

            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool DeleteRNGRA(string Fdate, string Agency, string Grp_cd, string CntCd)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteRNGRA");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlFdate = new SqlParameter("@RNGRA_CODE", Fdate);
            _dbCommand.Parameters.Add(sqlFdate);

            SqlParameter sqlTdate = new SqlParameter("@RNGRA_AGENCY", Agency);
            _dbCommand.Parameters.Add(sqlTdate);

            SqlParameter sqlGrp_cd = new SqlParameter("@RNGRA_GROUP_CODE", Grp_cd);
            _dbCommand.Parameters.Add(sqlGrp_cd);

            SqlParameter sqlTblCd = new SqlParameter("@RNGRA_COUNT_CODE", CntCd);
            _dbCommand.Parameters.Add(sqlTblCd);

            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool DeleteRNGGA(string Fdate, string Agency, string Grp_cd, string Tbl_cd, string GoalCd, string SP_Cd)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteRNGGA");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlFdate = new SqlParameter("@RNGGA_CODE", Fdate);
            _dbCommand.Parameters.Add(sqlFdate);

            SqlParameter sqlAdate = new SqlParameter("@RNGGA_AGENCY", Agency);
            _dbCommand.Parameters.Add(sqlAdate);

            SqlParameter sqlTdate = new SqlParameter("@RNGGA_TABLE_CODE", Tbl_cd);
            _dbCommand.Parameters.Add(sqlTdate);

            SqlParameter sqlGrp_cd = new SqlParameter("@RNGGA_GROUP_CODE", Grp_cd);
            _dbCommand.Parameters.Add(sqlGrp_cd);

            if (!string.IsNullOrEmpty(GoalCd.Trim()))
            {
                SqlParameter sqlTblCd = new SqlParameter("@RNGGA_GOAL_CODE", GoalCd);
                _dbCommand.Parameters.Add(sqlTblCd);
            }
            if (!string.IsNullOrEmpty(SP_Cd.Trim()))
            {
                SqlParameter sqlTblCd = new SqlParameter("@RNGGA_SP", SP_Cd);
                _dbCommand.Parameters.Add(sqlTblCd);
            }

            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool DeleteRNGSRGA(string Fdate, string Agency, string Grp_cd, string Tbl_cd, string GoalCd, string SP_Cd)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteRNGSRGA");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlFdate = new SqlParameter("@RNGSRGA_CODE", Fdate);
            _dbCommand.Parameters.Add(sqlFdate);

            SqlParameter sqlAgency = new SqlParameter("@RNGSRGA_AGENCY", Agency);
            _dbCommand.Parameters.Add(sqlAgency);

            SqlParameter sqlTdate = new SqlParameter("@RNGSRGA_TABLE_CODE", Tbl_cd);
            _dbCommand.Parameters.Add(sqlTdate);

            SqlParameter sqlGrp_cd = new SqlParameter("@RNGSRGA_GROUP_CODE", Grp_cd);
            _dbCommand.Parameters.Add(sqlGrp_cd);

            if (!string.IsNullOrEmpty(GoalCd.Trim()))
            {
                SqlParameter sqlTblCd = new SqlParameter("@RNGSRGA_GOAL_CODE", GoalCd);
                _dbCommand.Parameters.Add(sqlTblCd);
            }
            if (!string.IsNullOrEmpty(SP_Cd.Trim()))
            {
                SqlParameter sqlTblCd = new SqlParameter("@RNGSRGA_SP", SP_Cd);
                _dbCommand.Parameters.Add(sqlTblCd);
            }

            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool UpdateCSB14Grp_Goals(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateCSB14GRP_Goals");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_RnkCatg()
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_RankCtg]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_PreassGroups()
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_PreassGroups]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }




        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool InsertRankPnt(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertRankandPoints");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool InsertUpdatePREASSGRP(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdatePREASSGRP");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool DeleteRankCTG(string Agency, string RankCode, string SubCode, string Type, out string msg)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteRankCtg");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlAgy = new SqlParameter("@RANKCTG_AGENCY", Agency);
            _dbCommand.Parameters.Add(sqlAgy);

            SqlParameter sqlRankCd = new SqlParameter("@RANKCTG_GRPCTG_CODE", RankCode);
            _dbCommand.Parameters.Add(sqlRankCd);

            SqlParameter sqlSubCode = new SqlParameter("@RANKCTG_SUBCTG_CODE", SubCode);
            _dbCommand.Parameters.Add(sqlSubCode);


            SqlParameter sqlType = new SqlParameter("@Type", Type);
            _dbCommand.Parameters.Add(sqlType);

            SqlParameter sqlmsg = new SqlParameter("@Msg", SqlDbType.VarChar, 50);
            sqlmsg.Direction = ParameterDirection.Output;
            _dbCommand.Parameters.Add(sqlmsg);

            bool boolsuccess = _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
            msg = sqlmsg.Value.ToString();
            return boolsuccess;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetScreeNames(string RankCd)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[GetHiefromRNKCRIT1]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlFdt = new SqlParameter("@RNKCRIT1_RANK_CTG", RankCd);
            dbCmd.Parameters.Add(sqlFdt);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet BrowseFldScrs(string Scrtype)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[BrowseFLDSCRS]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlFdt = new SqlParameter("@FLDSCRS_CLDBY", Scrtype);
            dbCmd.Parameters.Add(sqlFdt);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet BrowseRankQues(string ScreenCode)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_RANKQUES]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlFdt = new SqlParameter("@RANKQUES_SCR", ScreenCode);
            dbCmd.Parameters.Add(sqlFdt);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet BrowseCustFlds()//string ScreenCode)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_CUSTFLDS]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet BrowseCustResp(string CustCode)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_CUSTRESP]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlFdt = new SqlParameter("@RSP_CUST_CODE", CustCode);
            dbCmd.Parameters.Add(sqlFdt);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet BrowseRNKCRIT1(string Agency, string RankCd, string ScreenCode, string FldCode)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_RNKCRIT1]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlTdt = new SqlParameter("@RNKCRIT1_AGENCY", Agency);
            dbCmd.Parameters.Add(sqlTdt);

            //SqlParameter sqlGrpCode = new SqlParameter("@RNKCRIT1_DEPT", Dept);
            //dbCmd.Parameters.Add(sqlGrpCode);

            //SqlParameter sqlCntCode = new SqlParameter("@RNKCRIT1_PROGRAM", Prog);
            //dbCmd.Parameters.Add(sqlCntCode);

            SqlParameter sqlRnk = new SqlParameter("@RNKCRIT1_RANK_CTG", RankCd);
            dbCmd.Parameters.Add(sqlRnk);


            SqlParameter sqlScrCode = new SqlParameter("@RNKCRIT1_SCR_CODE", ScreenCode);
            dbCmd.Parameters.Add(sqlScrCode);

            SqlParameter sqlFdt = new SqlParameter("@RNKCRIT1_FIELD_CODE", FldCode);
            dbCmd.Parameters.Add(sqlFdt);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool InsertRNKCRIT1(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertRNKCRIT1");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool InsertRNKCRIT2(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertRNKCRIT2");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet BrowseRNKCRIT2(string ID)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Browse_RNKCRTI2]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlTdt = new SqlParameter("@RNKCRIT2_ID", ID);
            dbCmd.Parameters.Add(sqlTdt);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetRnkCatg(string Agency)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[GetRnkCatg]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlFdt = new SqlParameter("@RNKCRIT1_AGENCY", Agency);
            dbCmd.Parameters.Add(sqlFdt);

            //SqlParameter sqlTdt = new SqlParameter("@RNKCRIT1_DEPT", Dept);
            //dbCmd.Parameters.Add(sqlTdt);

            //SqlParameter sqlGrpCode = new SqlParameter("@RNKCRIT1_PROGRAM", Prog);
            //dbCmd.Parameters.Add(sqlGrpCode);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_Appdet(string Agy, string dept, string prog, string year, string Bus_No, string Route)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[Get_Buslist_App]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlFdt = new SqlParameter("@AGENCY", Agy);
            dbCmd.Parameters.Add(sqlFdt);
            SqlParameter sqlTdt = new SqlParameter("@DEPT", dept);
            dbCmd.Parameters.Add(sqlTdt);
            SqlParameter sqlGrpCd = new SqlParameter("@PROGRAM", prog);
            dbCmd.Parameters.Add(sqlGrpCd);
            SqlParameter sqlTblCd = new SqlParameter("@YEAR", year);
            dbCmd.Parameters.Add(sqlTblCd);
            SqlParameter sqlDesc = new SqlParameter("@Bus_Num", Bus_No);
            dbCmd.Parameters.Add(sqlDesc);
            SqlParameter sqlRte = new SqlParameter("@Route", Route);
            dbCmd.Parameters.Add(sqlRte);

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetAppforFunnel(string CAMS_Type, string Agy, string Dept, string Prog, string Year, string SP_Plan, string Code, string Worker, string Branch, string Group, string Enrl_Seq, string Enrl_Hie, string Fund_SW, string strRepSw, string UserName, string CaseType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Get_AppsforFunnel]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 4200;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //if (!string.IsNullOrEmpty(SP_code))
            //{
            SqlParameter sqlSP_code = new SqlParameter("@CAMS_TYPE", CAMS_Type);
            dbCommand.Parameters.Add(sqlSP_code);
            //}
            SqlParameter sqlAgy = new SqlParameter("@AGENCY", Agy);
            dbCommand.Parameters.Add(sqlAgy);
            SqlParameter sqlDept = new SqlParameter("@DEPT", Dept);
            dbCommand.Parameters.Add(sqlDept);
            SqlParameter sqlProg = new SqlParameter("@PROG", Prog);
            dbCommand.Parameters.Add(sqlProg);

            if (!string.IsNullOrEmpty(Year))
            {
                SqlParameter sqlYear = new SqlParameter("@YEAR", Year);
                dbCommand.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(SP_Plan))
            {
                SqlParameter sqlSP_Plan = new SqlParameter("@SERVICEPLAN", SP_Plan);
                dbCommand.Parameters.Add(sqlSP_Plan);
            }
            if (!string.IsNullOrEmpty(Code))
            {
                SqlParameter sqlCAMS_CODE = new SqlParameter("@CAMS_CODE", Code);
                dbCommand.Parameters.Add(sqlCAMS_CODE);
            }

            if (!string.IsNullOrEmpty(Worker))
            {
                SqlParameter sqlWORKER = new SqlParameter("@CASEWORKER", Worker);
                dbCommand.Parameters.Add(sqlWORKER);
            }

            if (!string.IsNullOrEmpty(Branch))
            {
                SqlParameter sqlBranch = new SqlParameter("@BRANCH", Branch);
                dbCommand.Parameters.Add(sqlBranch);
            }

            if (!string.IsNullOrEmpty(Group))
            {
                SqlParameter sqlGroup = new SqlParameter("@GROUP", Group);
                dbCommand.Parameters.Add(sqlGroup);
            }

            if (!string.IsNullOrEmpty(Enrl_Seq))
            {
                SqlParameter sqlEnrlStat = new SqlParameter("@ENRL_SEQ", Enrl_Seq);
                dbCommand.Parameters.Add(sqlEnrlStat);
            }

            if (!string.IsNullOrEmpty(Enrl_Hie))
            {
                SqlParameter sqlEnrlHie = new SqlParameter("@ENRL_HIE", Enrl_Hie);
                dbCommand.Parameters.Add(sqlEnrlHie);
            }


            if (!string.IsNullOrEmpty(Fund_SW))
            {
                SqlParameter sqlEnrlFundSw = new SqlParameter("@FUND_SW", Fund_SW);
                dbCommand.Parameters.Add(sqlEnrlFundSw);
            }

            if (strRepSw != string.Empty)
            {
                SqlParameter sqlReptSw = new SqlParameter("@Rep_SW", strRepSw);
                dbCommand.Parameters.Add(sqlReptSw);
            }

            if (UserName != string.Empty)
            {
                SqlParameter sqlReptSw = new SqlParameter("@UserName", UserName);
                dbCommand.Parameters.Add(sqlReptSw);
            }
            if (CaseType != string.Empty)
            {
                SqlParameter sqlReptSw = new SqlParameter("@CaseType", CaseType);
                dbCommand.Parameters.Add(sqlReptSw);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetChldAttMsDetails(string Agency, string Dept, string Prog, string Year, string Site, string Room, string Ampm, string Month, string AttmYear, string AgyStatus, string AttmStatus, string AttmDay, string Type, string strfunds)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetAttandanceStatus]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Prog != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PROGRAM", Prog);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Site", Site);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Room != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Room", Room);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Ampm != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AmPm", Ampm);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Month != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AttmMonth", Month);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (AttmYear != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AttmYear", AttmYear);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (AgyStatus != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AGYStatus", AgyStatus);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (AttmStatus != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AttmsStatus", AttmStatus);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (AttmDay != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AttmsDay", AttmDay);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Type != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Type", Type);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strfunds != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AttmFunds", strfunds);
                dbCommand.Parameters.Add(empnoParm);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_RankQuesData(string Type, string Agency, string RankCtg)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Get_RankQuesData]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //if (!string.IsNullOrEmpty(SP_code))
            //{
            SqlParameter sqlSP_code = new SqlParameter("@Type", Type);
            dbCommand.Parameters.Add(sqlSP_code);
            //}
            if (Agency != string.Empty)
            {
                SqlParameter sqlAgy = new SqlParameter("@AGENCY", Agency);
                dbCommand.Parameters.Add(sqlAgy);
            }
            if (RankCtg != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@RankCtg", RankCtg);
                dbCommand.Parameters.Add(sqlDept);
            }



            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet HSSB2110_SiteReprt(string Agency, string Dept, string Program, string Year)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[HSSB2102_Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlSP_code = new SqlParameter("@Agency", Agency);
            dbCommand.Parameters.Add(sqlSP_code);
            SqlParameter sqlBR_Code = new SqlParameter("@Dept", Dept);
            dbCommand.Parameters.Add(sqlBR_Code);
            SqlParameter sqlCA_Desc = new SqlParameter("@prog", Program);
            dbCommand.Parameters.Add(sqlCA_Desc);
            SqlParameter sqlCA_Active_SW = new SqlParameter("@year", Year);
            dbCommand.Parameters.Add(sqlCA_Active_SW);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static DataSet TMSB6B01_Report(string Agency, string Dept, string Prog, string Year, string Active, string Vendor_Type, string FuelType, string Terminal, string Contract, string Vendor, string Module)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[TMSB6B01_Report]";//[Get_ChildLists]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            //if (string.IsNullOrEmpty(Agency))
            dbCommand.CommandTimeout = 1200;
            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Prog != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PROG", Prog);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@YEAR", Year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Active != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ACTIVE", Active);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Vendor_Type != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@VENDOR_TYPE", Vendor_Type);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (FuelType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FUEL_TYPE", FuelType);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Terminal != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@TERMINAL", Terminal);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Contract != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CONTRACT", Contract);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Vendor != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@VENDORS", Vendor);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Module != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@MODULE", Module);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetVddOrVdd1Data(string VddCode, string VddActive, string VddType, string VddFuelType, string Type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_VDDORVDD1]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (VddCode != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@VDDCODE", VddCode);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (VddActive != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@VDDACTIVE", VddActive);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (VddType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@VDDTYPE", VddType);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (VddFuelType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@VDDFUELTYPE", VddFuelType);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Type != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@TYPE", Type);
                dbCommand.Parameters.Add(empnoParm);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        public static DataSet Browse_HSSB2104(string Agency, string Dept, string Prog, string Year, string App, string Fund_Hie, string Site, string Room, string AmPm, string Asof_Date, string UserID)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_HSSB2104_Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1200;


            SqlParameter empnoParm = new SqlParameter("@Tmp_Asof_Date", Asof_Date);
            dbCommand.Parameters.Add(empnoParm);

            SqlParameter empnoParm1 = new SqlParameter("@Agency", Agency);
            dbCommand.Parameters.Add(empnoParm1);

            SqlParameter empnoParm9 = new SqlParameter("@Dept", Dept);
            dbCommand.Parameters.Add(empnoParm9);

            SqlParameter empnoParm2 = new SqlParameter("@Prog", Prog);
            dbCommand.Parameters.Add(empnoParm2);

            SqlParameter empnoParm3 = new SqlParameter("@Year", Year);
            dbCommand.Parameters.Add(empnoParm3);
            if (!string.IsNullOrEmpty(App.Trim()))
            {
                SqlParameter empnoParm4 = new SqlParameter("@App", App);
                dbCommand.Parameters.Add(empnoParm4);
            }

            if (!string.IsNullOrEmpty(Fund_Hie.Trim()))
            {
                SqlParameter empnoParm5 = new SqlParameter("@FundHie", Fund_Hie);
                dbCommand.Parameters.Add(empnoParm5);
            }

            if (!string.IsNullOrEmpty(Site.Trim()))
            {
                SqlParameter empnoParm6 = new SqlParameter("@Site", Site);
                dbCommand.Parameters.Add(empnoParm6);
            }
            if (!string.IsNullOrEmpty(Room.Trim()))
            {
                SqlParameter empnoParm7 = new SqlParameter("@Room", Room);
                dbCommand.Parameters.Add(empnoParm7);
            }
            if (!string.IsNullOrEmpty(AmPm.Trim()))
            {
                SqlParameter empnoParm8 = new SqlParameter("@AmPm", AmPm);
                dbCommand.Parameters.Add(empnoParm8);
            }

            if (!string.IsNullOrEmpty(UserID.Trim()))
            {
                SqlParameter empnoParm8 = new SqlParameter("@UserID", UserID);
                dbCommand.Parameters.Add(empnoParm8);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }





        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCAMSforSPM2(string Agy, string Dept, string Prog, string Year, string SP_Plan)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Get_CAMS_FromSPM2]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            SqlParameter sqlAgy = new SqlParameter("@SPM2_AGENCY", Agy);
            dbCommand.Parameters.Add(sqlAgy);
            SqlParameter sqlDept = new SqlParameter("@SPM2_DEPT", Dept);
            dbCommand.Parameters.Add(sqlDept);
            SqlParameter sqlProg = new SqlParameter("@SPM2_PROGRAM", Prog);
            dbCommand.Parameters.Add(sqlProg);

            if (!string.IsNullOrEmpty(Year))
            {
                SqlParameter sqlYear = new SqlParameter("@SPM2_YEAR", Year);
                dbCommand.Parameters.Add(sqlYear);
            }

            if (!string.IsNullOrEmpty(SP_Plan))
            {
                SqlParameter sqlSP_Plan = new SqlParameter("@SPM2_SERVICE_PLAN", int.Parse(SP_Plan));
                dbCommand.Parameters.Add(sqlSP_Plan);
            }
            //SqlParameter sqlCAMS_CODE = new SqlParameter("@SPM2_SEQ", Seq);
            //dbCommand.Parameters.Add(sqlCAMS_CODE);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool InsertUpdatePirQuest(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdatePirQuest");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //public static bool UpdateVouchers(string Agency, string ACTID,string Mode,string VouchNo)
        //{
        //    _dbCommand = _dbFactory.GetStoredProcCommand("dbo.Update_Act_Ms_Agcy");
        //    _dbCommand.CommandTimeout = 1200;
        //    _dbCommand.Parameters.Clear();

        //    List<SqlParameter> sqlParamList = new List<SqlParameter>();
        //    SqlParameter sqlRepType = new SqlParameter("@AGENCY", Agency);
        //    _dbCommand.Parameters.Add(sqlRepType);

        //    SqlParameter sqlRep_Code = new SqlParameter("@ACTID_XML", ACTID);
        //    _dbCommand.Parameters.Add(sqlRep_Code);

        //    SqlParameter sqlRep_Mode = new SqlParameter("@Mode", Mode);
        //    _dbCommand.Parameters.Add(sqlRep_Mode);


        //    SqlParameter sqlRep_Voucher = new SqlParameter("@VOCHERNO", VouchNo);
        //    _dbCommand.Parameters.Add(sqlRep_Voucher);
        //    return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        //}

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool UpdateVouchers(string Agency, string ACTID, string Vouch_Sw, string VouchNo, string Mode)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.Update_Act_Ms_Agcy");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlRepType = new SqlParameter("@AGENCY", Agency);
            _dbCommand.Parameters.Add(sqlRepType);

            SqlParameter sqlRep_Code = new SqlParameter("@ACTID_XML", ACTID);
            _dbCommand.Parameters.Add(sqlRep_Code);

            SqlParameter sqlRep_sw = new SqlParameter("@Voucher_sw", Vouch_Sw);
            _dbCommand.Parameters.Add(sqlRep_sw);

            SqlParameter sqlRep_no = new SqlParameter("@VOCHERNO", VouchNo);
            _dbCommand.Parameters.Add(sqlRep_no);

            SqlParameter sqlRep_Mode = new SqlParameter("@Mode", Mode);
            _dbCommand.Parameters.Add(sqlRep_Mode);

            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool InsertReportLog(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertREPORTLOG");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetReportLog(List<SqlParameter> sqlParamList)
        {
            DataSet ds;
            Database db;
            //string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            dbCommand = db.GetStoredProcCommand("GETREPORTLOG");
            dbCommand.CommandTimeout = 1200;

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool DeleteRecords(string RepType)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DELETE_EMSRecs");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlRepType = new SqlParameter("@RepType", RepType);
            _dbCommand.Parameters.Add(sqlRepType);

            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        public static bool InsertUpdateDelTRIGCRIT(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelCTTRIGCRIT");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetTRIGQUES()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_CTTRIGQUESTS]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetTRIGCRITadpgs(string Code, string groupcode, string seq, string type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetTRIGCRITadpgs]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Code != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@CTTRIGCRIT_CODE", Code);
                dbCommand.Parameters.Add(sqlAgency);
            }
            if (groupcode != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@CTTRIGCRIT_GROUP_CODE", groupcode);
                dbCommand.Parameters.Add(sqlAgency);
            }
            if (seq != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@CTTRIGCRIT_GROUP_SEQ", seq);
                dbCommand.Parameters.Add(sqlAgency);
            }
            if (type != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@Type", type);
                dbCommand.Parameters.Add(sqlAgency);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static string GETTRIGgroupCode(string Code)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetTrigGroupCode]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            if (Code != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@TriggerCode", Code);
                dbCommand.Parameters.Add(sqlAgency);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds.Tables[0].Rows[0][0].ToString();
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_ClientInq_Totals(string agy, string dept, string Prog, string Year, string App)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Get_ClientInq_Totals]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);


            if (agy != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@Agency", agy);
                dbCommand.Parameters.Add(sqlAgency);
            }
            if (dept != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@Dept", dept);
                dbCommand.Parameters.Add(sqlAgency);
            }
            if (Prog != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@Prog", Prog);
                dbCommand.Parameters.Add(sqlAgency);
            }
            //if (Year != string.Empty)
            //{
            SqlParameter sqlAgency3 = new SqlParameter("@Year", Year);
            dbCommand.Parameters.Add(sqlAgency3);
            //}
            if (App != string.Empty)
            {
                SqlParameter sqlAgency1 = new SqlParameter("@App", App);
                dbCommand.Parameters.Add(sqlAgency1);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_SpmCompletDt(string agy, string dept, string Prog, string Year, string App, string serviceplan, string spmseq, string strtype)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETSPMCOMPLETDT]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agy != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@Agency", agy);
                dbCommand.Parameters.Add(sqlAgency);
            }
            if (dept != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@Dept", dept);
                dbCommand.Parameters.Add(sqlAgency);
            }
            if (Prog != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@Program", Prog);
                dbCommand.Parameters.Add(sqlAgency);
            }
            if (Year != string.Empty)
            {
                SqlParameter sqlAgency3 = new SqlParameter("@Year", Year);
                dbCommand.Parameters.Add(sqlAgency3);
            }
            if (App != string.Empty)
            {
                SqlParameter sqlAgency1 = new SqlParameter("@AppNo", App);
                dbCommand.Parameters.Add(sqlAgency1);
            }
            if (serviceplan != string.Empty)
            {
                SqlParameter sqlAgency1 = new SqlParameter("@ServicePlan", serviceplan);
                dbCommand.Parameters.Add(sqlAgency1);
            }
            if (spmseq != string.Empty)
            {
                SqlParameter sqlAgency1 = new SqlParameter("@SpmSeq", spmseq);
                dbCommand.Parameters.Add(sqlAgency1);
            }
            if (strtype != string.Empty)
            {
                SqlParameter sqlAgency1 = new SqlParameter("@Type", strtype);
                dbCommand.Parameters.Add(sqlAgency1);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetTrigEligdata(string Agency, string Dept, string Program, string Year, string Cummulative)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[TRIGELIGDATA]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1600;

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
                SqlParameter programParm = new SqlParameter("@PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }

            if (Cummulative != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Cummilative_Sw", Cummulative);
                dbCommand.Parameters.Add(typeParm);
            }


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetTrigParam()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETTRIGPARAM]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetSSBGEligdata(string Agency, string Dept, string Program, string Year, string frmdate, string todate, string SSBGID, string MST_Site)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[SSBGELIGDATA]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1600;

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
                SqlParameter programParm = new SqlParameter("@PROGRAM", Program);
                dbCommand.Parameters.Add(programParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@YEAR", Year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (frmdate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@FROMDATE", frmdate);
                dbCommand.Parameters.Add(typeParm);
            }
            if (todate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@TODATE", todate);
                dbCommand.Parameters.Add(typeParm);
            }
            if (SSBGID != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@SSBGID", SSBGID);
                dbCommand.Parameters.Add(typeParm);
            }

            if (MST_Site != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Mst_Site", MST_Site);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertTRIGPARAM(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertTRIGPARAM");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertUpDelCAVouchGen(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSERTUPDATEDELCAVouchGen");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        public static bool InsertUpDelCAVASSOC(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSUPDEL_CAVASSOC");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertUpDelVouchDef(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSUPDATEDEL_voucher_Definition");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool INSUPDELCAVDates(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSUPDEL_CAVDates");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        public static bool Delete_Trigger_Bulk_Posting(string Agency, string Dept, string Program, string Year, string Trigcode)
        {
            bool boolsuccess;
            //Sql_SP_Result_Message = Consts.Common.DB_Exception;
            //New_Date_Seq = 1;
            try
            {
                _dbCommand = _dbFactory.GetStoredProcCommand("dbo.Delete_TriggerPosting");
                _dbCommand.CommandTimeout = 1200;
                _dbCommand.Parameters.Clear();

                //List<SqlParameter> sqlParamList = new List<SqlParameter>();
                //SqlParameter sqlRepType = new SqlParameter("@RepType", RepType);
                //_dbCommand.Parameters.Add(sqlRepType);



                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@Agy", Agency));
                sqlParamList.Add(new SqlParameter("@Dept", Dept));
                sqlParamList.Add(new SqlParameter("@Prog", Program));
                sqlParamList.Add(new SqlParameter("@Year", Year));
                sqlParamList.Add(new SqlParameter("@TrigCode", Trigcode));

                foreach (SqlParameter sqlPar in sqlParamList)
                {
                    _dbCommand.Parameters.Add(sqlPar);
                }

                return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;

            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GETCUSTRESPAPPCOUNT(string strcode, string strType, string strRespCode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETCUSTRESPAPPCOUNT]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(strcode))
            {
                SqlParameter sqlSP_code = new SqlParameter("@CUST_CODE", strcode);
                dbCommand.Parameters.Add(sqlSP_code);
            }
            if (!string.IsNullOrEmpty(strType))
            {
                SqlParameter sqlAgy = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(sqlAgy);
            }
            if (!string.IsNullOrEmpty(strRespCode))
            {
                SqlParameter sqlAgy = new SqlParameter("@RespCode", strRespCode);
                dbCommand.Parameters.Add(sqlAgy);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet MATB0003_Report(string agency, string dep, string program, string year, string Matcode, string Scales, string Fromdate, string Todate, string CP2Fromdate, string CP2Todate, string CP3Fromdate, string CP3Todate, string worker, string Module, string Username)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[MATB0003_REPORT]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 2400;

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Agency", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Dept", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Prog", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Year", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Matcode.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@MATCODE", Matcode);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Scales.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Scales_List", Scales);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (worker.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CaseWorker", worker);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fromdate.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Asmt_From_Date", Fromdate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Todate.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Asmt_To_Date", Todate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (CP2Fromdate.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Asmt2_From_Date", CP2Fromdate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (CP2Todate.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Asmt2_To_Date", CP2Todate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (CP3Fromdate.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Asmt3_From_Date", CP3Fromdate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (CP3Todate.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Asmt3_To_Date", CP3Todate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Module.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Module_Code", Module);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Username.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@UserName", Username);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        public static bool INSERTUPDATEDELADHOCHISTORY(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSERTUPDATEDELADHOCHISTORY");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GETSPMWITHMST(string strAgency, string strDept, string strProgram, string strYear, string strApp, string strServiceplan, string strFromdt, string strTodt, string strType, string strResult)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETSPMWITHMST]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(strAgency))
            {
                SqlParameter sqlSP_code = new SqlParameter("@Agency", strAgency);
                dbCommand.Parameters.Add(sqlSP_code);
            }
            if (!string.IsNullOrEmpty(strDept))
            {
                SqlParameter sqlSP_code = new SqlParameter("@Dept", strDept);
                dbCommand.Parameters.Add(sqlSP_code);
            }
            if (!string.IsNullOrEmpty(strProgram))
            {
                SqlParameter sqlSP_code = new SqlParameter("@Program", strProgram);
                dbCommand.Parameters.Add(sqlSP_code);
            }
            if (!string.IsNullOrEmpty(strYear))
            {
                SqlParameter sqlSP_code = new SqlParameter("@Year", strYear);
                dbCommand.Parameters.Add(sqlSP_code);
            }
            if (!string.IsNullOrEmpty(strApp))
            {
                SqlParameter sqlSP_code = new SqlParameter("@AppNo", strApp);
                dbCommand.Parameters.Add(sqlSP_code);
            }
            if (!string.IsNullOrEmpty(strServiceplan))
            {
                SqlParameter sqlSP_code = new SqlParameter("@ServicePlan", strServiceplan);
                dbCommand.Parameters.Add(sqlSP_code);
            }
            if (!string.IsNullOrEmpty(strFromdt))
            {
                SqlParameter sqlSP_code = new SqlParameter("@FROMDATE", strFromdt);
                dbCommand.Parameters.Add(sqlSP_code);
            }
            if (!string.IsNullOrEmpty(strTodt))
            {
                SqlParameter sqlSP_code = new SqlParameter("@TODATE", strTodt);
                dbCommand.Parameters.Add(sqlSP_code);
            }
            if (!string.IsNullOrEmpty(strType))
            {
                SqlParameter sqlAgy = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(sqlAgy);
            }
            if (!string.IsNullOrEmpty(strResult))
            {
                SqlParameter sqlAgy = new SqlParameter("@Result", strResult);
                dbCommand.Parameters.Add(sqlAgy);
            }


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool UpdateSpmActualCompletiondate(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateSpmCompletiondt");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_VochersData(string Agency, string dateID, string Type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_PAYMENT_VOUCHER_NO]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            if (Agency != string.Empty)
            {
                SqlParameter sqlAgy = new SqlParameter("@PARAM_CAVASSOC_AGENCY", Agency);
                dbCommand.Parameters.Add(sqlAgy);
            }
            if (dateID != string.Empty)
            {
                SqlParameter sqlDept = new SqlParameter("@PARAM_CAVASSOC_CAVD_ID", dateID);
                dbCommand.Parameters.Add(sqlDept);
            }
            if (!string.IsNullOrEmpty(Type))
            {
                SqlParameter sqlSP_code = new SqlParameter("@PARAM_CAVASSOC_Type", Type);
                dbCommand.Parameters.Add(sqlSP_code);
            }




            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CASE0025_GET(string agency, string dep, string program, string year, string App, string Fund, string Serviceplan, string Site, string strDate, string strDateH, string strActivityCode, string strType, string strVendorL, string strVendorH)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CASE0025_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 2400;
            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_YEAR", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (App != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_APP", App);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Serviceplan != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_SERVICE_PLAN", Serviceplan);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_SITE", Site);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@DateL", strDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strDateH != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@DateH", strDateH);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strActivityCode != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_ACTIVITY_CODE", strActivityCode);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strVendorL != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@VENDST", strVendorL);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strVendorH != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@VENDEND", strVendorH);
                dbCommand.Parameters.Add(empnoParm);
            }


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }
        //Vikash added for Agency Wide Activity Report 12/15/2022
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CAPS_CASB0013_GET(string strFilcol, string strReffdate, string strReftdate, string strRepfdate, string strRepTdate, string agency, string dept, string Prog, string year, string Reporttype, string undupcount)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_CASB0013_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1200;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();



            SqlParameter sqlfiltercol = new SqlParameter("@CA_FilterColun_Sw", strFilcol);
            dbCommand.Parameters.Add(sqlfiltercol);

            SqlParameter sql_fromdt = new SqlParameter("@Ref_From_Date", strReffdate);
            dbCommand.Parameters.Add(sql_fromdt);

            SqlParameter sql_todate = new SqlParameter("@Ref_To_Date", strReftdate);
            dbCommand.Parameters.Add(sql_todate);

            SqlParameter repdate = new SqlParameter("@Rep_Period_FDate", strRepfdate);
            dbCommand.Parameters.Add(repdate);

            SqlParameter reptodate = new SqlParameter("@Rep_Period_TDate", strRepTdate);
            dbCommand.Parameters.Add(reptodate);

            SqlParameter sql_agency = new SqlParameter("@AGENCY", agency);
            dbCommand.Parameters.Add(sql_agency);
            SqlParameter sql_dept = new SqlParameter("@DEPT", dept);
            dbCommand.Parameters.Add(sql_dept);

            SqlParameter sql_Prog = new SqlParameter("@PROGRAM", Prog);
            dbCommand.Parameters.Add(sql_Prog);

            if (year.Trim() != string.Empty)
            {
                SqlParameter sql_year = new SqlParameter("@YEAR", year);
                dbCommand.Parameters.Add(sql_year);
            }

            SqlParameter sql_county = new SqlParameter("@ReportType", Reporttype);
            dbCommand.Parameters.Add(sql_county);
            SqlParameter sql_zipcode = new SqlParameter("@UndupCounts", undupcount);
            dbCommand.Parameters.Add(sql_zipcode);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CASB0015_Report(string agency, string dep, string program, string year, string App, string Vendor, string startDate, string EndDate, string RepFor, string BundleNo)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CASB0015_REPORT]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 2400;
            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_YEAR", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (App != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_APP", App);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Vendor != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Sel_Vendor", Vendor);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (startDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@StartDate", startDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (EndDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EndDate", EndDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (RepFor != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@StrType", RepFor);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (BundleNo != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BundleNo", BundleNo);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CASBLTRB_CASEACT_Report(string agency, string dep, string program, string year, string Worker, string Site, string BundleNo)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_CASEACT_CASBLTRB]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 2400;
            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_YEAR", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Worker != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_CASEWRKR", Worker);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_SITE", Site);
                dbCommand.Parameters.Add(empnoParm);
            }

            //if (startDate != string.Empty)
            //{
            //    SqlParameter empnoParm = new SqlParameter("@StartDate", startDate);
            //    dbCommand.Parameters.Add(empnoParm);
            //}
            //if (EndDate != string.Empty)
            //{
            //    SqlParameter empnoParm = new SqlParameter("@EndDate", EndDate);
            //    dbCommand.Parameters.Add(empnoParm);
            //}
            //if (RepFor != string.Empty)
            //{
            //    SqlParameter empnoParm = new SqlParameter("@StrType", RepFor);
            //    dbCommand.Parameters.Add(empnoParm);
            //}
            if (BundleNo != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_BUNDLE_NO", BundleNo);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet ADMNB005_GET(string agency, string strFromdt, string strTodt, string strType, string dept, string program, string year)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[ADMNB005_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 2400;
            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Agency", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strFromdt != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@From", strFromdt);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strTodt != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@To", strTodt);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Dept", dept);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Program", program);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Year", year);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Vendor_Search(string strsearchby, string strSearchsource)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[VENDSRCH]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 2400;
            if (strsearchby != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SearchBy", strsearchby);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strSearchsource != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SearchSource", strSearchsource);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CAPS_BULKTEMPLATE_BROWSE(string ID, string Code, string SP, string SPMSeq, string Branch, string Group)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_BULKTEMPLATE_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(ID))
            {
                SqlParameter sqlSP_code = new SqlParameter("@BTPL_ID", int.Parse(ID));
                dbCommand.Parameters.Add(sqlSP_code);
            }
            if (!string.IsNullOrEmpty(Code))
            {
                SqlParameter sqlAgy = new SqlParameter("@BTPL_CODE", Code);
                dbCommand.Parameters.Add(sqlAgy);
            }
            if (!string.IsNullOrEmpty(SP))
            {
                SqlParameter sqlDept = new SqlParameter("@BTPL_SERVICEPLAN", SP);
                dbCommand.Parameters.Add(sqlDept);
            }
            if (!string.IsNullOrEmpty(SPMSeq))
            {
                SqlParameter sqlProg = new SqlParameter("@BTPL_SPM_SEQ", SPMSeq);
                dbCommand.Parameters.Add(sqlProg);
            }

            if (!string.IsNullOrEmpty(Branch))
            {
                SqlParameter sqlProg = new SqlParameter("@BTPL_BRANCH", Branch);
                dbCommand.Parameters.Add(sqlProg);
            }

            if (!string.IsNullOrEmpty(Group))
            {
                SqlParameter sqlProg = new SqlParameter("@BTPL_GROUP", Group);
                dbCommand.Parameters.Add(sqlProg);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool INSUPDELBULKTEMPLATE(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_BULKTEMPLATE_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_CASESP1(string SP_code, string Agy, string Dept, string Prog)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_CASESP1]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(SP_code))
            {
                SqlParameter sqlSP_code = new SqlParameter("@sp1_servicecode", int.Parse(SP_code));
                dbCommand.Parameters.Add(sqlSP_code);
            }
            SqlParameter sqlAgy = new SqlParameter("@sp1_agency", Agy);
            dbCommand.Parameters.Add(sqlAgy);
            SqlParameter sqlDept = new SqlParameter("@sp1_dept", Dept);
            dbCommand.Parameters.Add(sqlDept);
            SqlParameter sqlProg = new SqlParameter("@sp1_program", Prog);
            dbCommand.Parameters.Add(sqlProg);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCMBdcFundCheck(string agency, string dep, string program, string year, string Fund, string BDCID, string startDate, string EndDate, string NewstartDate, string NewEndDate, string strType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCMBDCCheckFund]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (BDCID != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_ID", BDCID);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (startDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_START", startDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (EndDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_END", EndDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (NewstartDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_NEWSTART", NewstartDate);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (NewEndDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_NEWEND", NewEndDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(empnoParm);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCMBdcData(string agency, string dep, string program, string year, string Desc, string Fund)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_CMBDC_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Desc != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_DESCRIPTION", Desc);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCMBDCCalAmount(string agency, string dep, string program, string year, string Fund, string BDCID, string startDate, string EndDate, string strType, string strBudget)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCMBDCCalAmount]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (BDCID != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_ID", BDCID);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (startDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_START", startDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (EndDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_END", EndDate);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strBudget != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_BUDGET", strBudget);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(empnoParm);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertLETRHIST(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_LETRHIST_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CASE0027_GET(string agency, string dep, string program, string year, string strDate, string strDateH, string strCaseworker, string strType, string strApplno)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;


            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CASE0027_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 2400;
            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@YEAR", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@StartDate", strDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strDateH != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EndDate", strDateH);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strCaseworker != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Caseworker", strCaseworker);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strApplno != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ApplNo", strApplno);
                dbCommand.Parameters.Add(empnoParm);
            }


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CASB0028_GET(string agency, string dep, string program, string year, string strCaseworker, string strStartDate, string strEndDate,
             string strCStartDate, string strCEndDate, string strApplno,
            string strType, string Status)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;


            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETCASB0028_REPORT]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 2400;
            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@YEAR", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strStartDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@StartDate", strStartDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strEndDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EndDate", strEndDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strCaseworker != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Caseworker", strCaseworker);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strCStartDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CStartDate", strCStartDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strCEndDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CEndDate", strCEndDate);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strApplno != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ApplNo", strApplno);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Status != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Status", Status);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CASE0028_GET(string agency, string dep, string program, string year, string strDate, string strDateH, string strCaseworker, string strType, string strApplno)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;


            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CASE0028_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 2400;
            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@YEAR", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@StartDate", strDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strDateH != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EndDate", strDateH);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strCaseworker != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Caseworker", strCaseworker);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strApplno != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ApplNo", strApplno);
                dbCommand.Parameters.Add(empnoParm);
            }


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetLETRHISTData(string agency, string dep, string program, string year, string Appno)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;



            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_LETRHIST_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@LHIST_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@LHIST_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@LHIST_PROG", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@LHIST_YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Appno != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@LHIST_APPNO", Appno);
                dbCommand.Parameters.Add(empnoParm);
            }
            //if (Fund != string.Empty)
            //{
            //    SqlParameter empnoParm = new SqlParameter("@BDC_FUND", Fund);
            //    dbCommand.Parameters.Add(empnoParm);
            //}

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CABundlingReport(string agency, string dep, string program, string year, string Vendor, string Fund, string BdcId, string site, string Amount,
            string frmdt, string Todt, string User, string Rep, string PDOUT, string SelCounties, string SelUsers, string SelRecs)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CASB0019_BUNDLE_REPORT]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Agy", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Dept", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Prog", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Year", year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Vendor != string.Empty)
            {
                SqlParameter empVend = new SqlParameter("@Sel_Vendor", Vendor);
                dbCommand.Parameters.Add(empVend);
            }

            if (Fund != string.Empty)
            {
                SqlParameter empVend = new SqlParameter("@FUND", Fund);
                dbCommand.Parameters.Add(empVend);
            }

            if (BdcId != string.Empty)
            {
                SqlParameter empVend = new SqlParameter("@BDC_ID", BdcId);
                dbCommand.Parameters.Add(empVend);
            }
            if (site != string.Empty)
            {
                SqlParameter empVend = new SqlParameter("@Sel_Site", site);
                dbCommand.Parameters.Add(empVend);
            }
            if (Amount != string.Empty)
            {
                SqlParameter empVend = new SqlParameter("@Bundle_Amount", Amount);
                dbCommand.Parameters.Add(empVend);
            }
            if (PDOUT != string.Empty)
            {
                SqlParameter empVend = new SqlParameter("@PDOUT", PDOUT);
                dbCommand.Parameters.Add(empVend);
            }

            if (SelCounties != string.Empty)
            {
                SqlParameter empVend = new SqlParameter("@Sel_County", SelCounties);
                dbCommand.Parameters.Add(empVend);
            }

            if(SelUsers!=string.Empty)
            {
                SqlParameter empVend = new SqlParameter("@Sel_Users", SelUsers);
                dbCommand.Parameters.Add(empVend);
            }


            SqlParameter frmdta = new SqlParameter("@From_Date", frmdt);
            dbCommand.Parameters.Add(frmdta);

            SqlParameter todta = new SqlParameter("@To_Date", Todt);
            dbCommand.Parameters.Add(todta);


            SqlParameter SqlUser = new SqlParameter("@User", User);
            dbCommand.Parameters.Add(SqlUser);

            SqlParameter SqlRep = new SqlParameter("@Rep", Rep);
            dbCommand.Parameters.Add(SqlRep);

            if (SelRecs != string.Empty)
            {
                SqlParameter empVend = new SqlParameter("@SelRecs", SelRecs);
                dbCommand.Parameters.Add(empVend);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCASEACT(string agency, string dep, string program, string year, string Bundle, string Type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_CASEACT_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Bundle != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_BUNDLE_NO", Bundle);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Type != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Type", Type);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CAPS_CASE0021_GET(string agency, string dep, string program, string year, string ApplicantNo, string strService, string strActcode, string Type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_CASE0021_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (ApplicantNo != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_APP_NO", ApplicantNo);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strService != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_SERVICE_PLAN", strService);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strActcode != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEACT_ACTIVITY_CODE", strActcode);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Type != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Mode", Type);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CAPS_CASB0021_GET(string agency, string dep, string program, string year, string Site, string AdjCd, string Caseworker, string Fund, string UserId, string Service, string Vendor, string FromDate, string Todate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_CASB0021_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1200;

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@YEAR", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Site != string.Empty && Site != "****")
            {
                SqlParameter empnoParm = new SqlParameter("@SITE", Site);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (AdjCd != string.Empty && AdjCd != "**")
            {
                SqlParameter empnoParm = new SqlParameter("@ADJCD", AdjCd);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Caseworker != string.Empty && Caseworker != "0")
            {
                SqlParameter empnoParm = new SqlParameter("@CASEWORKER", Caseworker);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund != string.Empty && Fund != "00")
            {
                SqlParameter empnoParm = new SqlParameter("@FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (UserId != string.Empty && UserId != "**")
            {
                SqlParameter empnoParm = new SqlParameter("@USERID", UserId);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Service != string.Empty && Service != "**********")
            {
                SqlParameter empnoParm = new SqlParameter("@SERVICE", Service);
                dbCommand.Parameters.Add(empnoParm);
            }


            if (Vendor != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@VENDOR", Vendor);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (FromDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FROMDate", FromDate);
                dbCommand.Parameters.Add(empnoParm);

                empnoParm = new SqlParameter("@TODate", Todate);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool CAPS_CASEACTADJ_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_CASEACTADJ_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool CAPS_SPMACT_UPDATE(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_SPMACT_UPDATE");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CAP_SEROUTCOMES(string Agency, string Dept, string prog, string SP2Type, string UserId)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAP_SEROUTCOMES]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(Agency.Trim()))
            {
                SqlParameter sqlSP_code = new SqlParameter("@Agy", Agency);
                dbCommand.Parameters.Add(sqlSP_code);
            }
            if (!string.IsNullOrEmpty(Dept.Trim()))
            {
                SqlParameter sqlBR_Code = new SqlParameter("@Dept", Dept);
                dbCommand.Parameters.Add(sqlBR_Code);
            }
            if (!string.IsNullOrEmpty(prog.Trim()))
            {
                SqlParameter sqlCA_Desc = new SqlParameter("@program", prog);
                dbCommand.Parameters.Add(sqlCA_Desc);
            }
            if (!string.IsNullOrEmpty(SP2Type.Trim()))
            {
                SqlParameter sqlCA_Active_SW = new SqlParameter("@SP2_Type", SP2Type);
                dbCommand.Parameters.Add(sqlCA_Active_SW);
            }
            if (!string.IsNullOrEmpty(UserId.Trim()))
            {
                SqlParameter sqlCA_Active_SW = new SqlParameter("@USERID", UserId);
                dbCommand.Parameters.Add(sqlCA_Active_SW);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CAPS_ADMN0030_BROWSE(string rngCode, string SPCode, string Agency)   //Added by Vikash on 03/15/2023 for Service Plan Target Entry Screen
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_ADMN0030_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(rngCode.Trim()))
            {
                SqlParameter sqlRNG_code = new SqlParameter("@RNG_CODE", rngCode);
                dbCommand.Parameters.Add(sqlRNG_code);
            }
            if (!string.IsNullOrEmpty(SPCode.Trim()))
            {
                SqlParameter sqlSP_Code = new SqlParameter("@SP_CODE", SPCode);
                dbCommand.Parameters.Add(sqlSP_Code);
            }
            if (!string.IsNullOrEmpty(Agency.Trim()))
            {
                SqlParameter sqlSP_Code = new SqlParameter("@RNG_AGENCY", Agency);
                dbCommand.Parameters.Add(sqlSP_Code);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool CAPS_INSUPDEL_SPTARGET(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_SPTARGET_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        public static DataSet INSUPDEL_SPTARGET(string spt_code, string spr_sp, string spt_grp, string spt_tbl, string spt_agency, string spt_hie, string spt_cams_code, string spt_cams_type, string spt_target, string spt_lstc_op, string mode)   //Added by Vikash on 03/16/2023 for Service Plan Target Entry Screen
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[INSUPDEL_SPTARGET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(spt_code.Trim()))
            {
                SqlParameter sqlRNG_code = new SqlParameter("@SPT_CODE", spt_code);
                dbCommand.Parameters.Add(sqlRNG_code);
            }
            if (!string.IsNullOrEmpty(spr_sp))
            {
                SqlParameter sqlSP_Code = new SqlParameter("@SPT_SP", spr_sp);
                dbCommand.Parameters.Add(sqlSP_Code);
            }
            if (!string.IsNullOrEmpty(spt_grp))
            {
                SqlParameter sqlSP_Grp = new SqlParameter("@SPT_GRP", spt_grp);
                dbCommand.Parameters.Add(sqlSP_Grp);
            }

            if (!string.IsNullOrEmpty(spt_tbl))
            {
                SqlParameter sqlSP_Tbl = new SqlParameter("@SPT_TBL", spt_tbl);
                dbCommand.Parameters.Add(sqlSP_Tbl);
            }

            if (!string.IsNullOrEmpty(spt_agency.Trim()))
            {
                SqlParameter sqlSP_Agy = new SqlParameter("@SPT_AGENCY", spt_agency);
                dbCommand.Parameters.Add(sqlSP_Agy);
            }

            if (!string.IsNullOrEmpty(spt_hie.Trim()))
            {
                SqlParameter sqlSP_Hie = new SqlParameter("@SPT_HIE", spt_hie);
                dbCommand.Parameters.Add(sqlSP_Hie);
            }

            if (!string.IsNullOrEmpty(spt_cams_code.Trim()))
            {
                SqlParameter sqlSP_CAMSC = new SqlParameter("@SPT_CAMS_CODE", spt_cams_code);
                dbCommand.Parameters.Add(sqlSP_CAMSC);
            }

            if (!string.IsNullOrEmpty(spt_cams_type.Trim()))
            {
                SqlParameter sqlSP_CAMST = new SqlParameter("@SPT_CAMS_TYPE", spt_cams_type);
                dbCommand.Parameters.Add(sqlSP_CAMST);
            }

            if (!string.IsNullOrEmpty(spt_target))
            {
                SqlParameter sqlSP_Tar = new SqlParameter("@SPT_TARGET", spt_target);
                dbCommand.Parameters.Add(sqlSP_Tar);
            }

            if (!string.IsNullOrEmpty(spt_lstc_op.Trim()))
            {
                SqlParameter sqlSP_Lop = new SqlParameter("@SPT_LSTC_OPERATOR", spt_lstc_op);
                dbCommand.Parameters.Add(sqlSP_Lop);
            }

            if (!string.IsNullOrEmpty(mode.Trim()))
            {
                SqlParameter sqlSP_Mode = new SqlParameter("@Mode", mode);
                dbCommand.Parameters.Add(sqlSP_Mode);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GET_SPTARGET(string spt_code, string spr_sp, string spt_grp, string spt_tbl, string spt_agency, string spt_hie, string spt_cams_code, string spt_cams_type, string spt_lstc_op)   //Added by Vikash on 03/16/2023 for Service Plan Target Entry Screen
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_SPTTARGET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(spt_code.Trim()))
            {
                SqlParameter sqlRNG_code = new SqlParameter("@SPT_CODE", spt_code);
                dbCommand.Parameters.Add(sqlRNG_code);
            }
            if (!string.IsNullOrEmpty(spr_sp))
            {
                SqlParameter sqlSP_Code = new SqlParameter("@SPT_SP", spr_sp);
                dbCommand.Parameters.Add(sqlSP_Code);
            }
            if (!string.IsNullOrEmpty(spt_grp))
            {
                SqlParameter sqlSP_Grp = new SqlParameter("@SPT_GRP", spt_grp);
                dbCommand.Parameters.Add(sqlSP_Grp);
            }

            if (!string.IsNullOrEmpty(spt_tbl))
            {
                SqlParameter sqlSP_Tbl = new SqlParameter("@SPT_TBL", spt_tbl);
                dbCommand.Parameters.Add(sqlSP_Tbl);
            }

            if (!string.IsNullOrEmpty(spt_agency.Trim()))
            {
                SqlParameter sqlSP_Agy = new SqlParameter("@SPT_AGENCY", spt_agency);
                dbCommand.Parameters.Add(sqlSP_Agy);
            }

            if (!string.IsNullOrEmpty(spt_hie.Trim()))
            {
                SqlParameter sqlSP_Hie = new SqlParameter("@SPT_HIE", spt_hie);
                dbCommand.Parameters.Add(sqlSP_Hie);
            }

            if (!string.IsNullOrEmpty(spt_cams_code.Trim()))
            {
                SqlParameter sqlSP_CAMSC = new SqlParameter("@SPT_CAMS_CODE", spt_cams_code);
                dbCommand.Parameters.Add(sqlSP_CAMSC);
            }

            if (!string.IsNullOrEmpty(spt_cams_type.Trim()))
            {
                SqlParameter sqlSP_CAMST = new SqlParameter("@SPT_CAMS_TYPE", spt_cams_type);
                dbCommand.Parameters.Add(sqlSP_CAMST);
            }

            if (!string.IsNullOrEmpty(spt_lstc_op.Trim()))
            {
                SqlParameter sqlSP_Lop = new SqlParameter("@SPT_LSTC_OPERATOR", spt_lstc_op);
                dbCommand.Parameters.Add(sqlSP_Lop);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GET_CUSTQUESReport(string agency)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;



            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_ADMN00022_REPORT]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 3600;

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SAL_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            //if (dep != string.Empty)
            //{
            //    SqlParameter empnoParm = new SqlParameter("@LHIST_DEPT", dep);
            //    dbCommand.Parameters.Add(empnoParm);
            //}
            //if (program != string.Empty)
            //{
            //    SqlParameter empnoParm = new SqlParameter("@LHIST_PROG", program);
            //    dbCommand.Parameters.Add(empnoParm);
            //}
            //if (year.Trim() != string.Empty)
            //{
            //    SqlParameter empnoParm = new SqlParameter("@LHIST_YEAR ", year);
            //    dbCommand.Parameters.Add(empnoParm);
            //}

            //if (Appno != string.Empty)
            //{
            //    SqlParameter empnoParm = new SqlParameter("@LHIST_APPNO", Appno);
            //    dbCommand.Parameters.Add(empnoParm);
            //}
            ////if (Fund != string.Empty)
            ////{
            ////    SqlParameter empnoParm = new SqlParameter("@BDC_FUND", Fund);
            ////    dbCommand.Parameters.Add(empnoParm);
            ////}

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_SPTARGET(string MainCode, string SP_code, string Agy)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;
            SqlParameter sqlSP_code;
            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAP_SPTARGET_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(SP_code))
            {
                /*SqlParameter*/
                sqlSP_code = new SqlParameter("@SPT_SP", int.Parse(SP_code));
                dbCommand.Parameters.Add(sqlSP_code);
            }
            SqlParameter sqlAgy = new SqlParameter("@SPT_AGENCY", Agy);
            dbCommand.Parameters.Add(sqlAgy);
            SqlParameter sqlDept = new SqlParameter("@SPT_DT_CODE", MainCode);
            dbCommand.Parameters.Add(sqlDept);



            ////Vikash:11/08/2022:: Commented for Hiearchy definition delete option issue
            ////if (!string.IsNullOrEmpty(SP_code))
            ////{
            //sqlSP_code = new SqlParameter("@Mode", Mode);
            //dbCommand.Parameters.Add(sqlSP_code);
            ////}

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_ServicePlanFromSerOutcome(string seroutcome, string oType, string AdminAgency, string Mode)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[GET_SP_SERVICEOUTCOME]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(seroutcome))
            {
                SqlParameter sqlFdt = new SqlParameter("@SEROUTCOME", seroutcome);
                dbCmd.Parameters.Add(sqlFdt);
            }
            SqlParameter sqlTdt = new SqlParameter("@TYPE", oType);
            dbCmd.Parameters.Add(sqlTdt);

            if (!string.IsNullOrEmpty(AdminAgency))
            {
                SqlParameter sqlUser = new SqlParameter("@AGENCY", AdminAgency);
                dbCmd.Parameters.Add(sqlUser);
            }

            if (!string.IsNullOrEmpty(Mode))
            {
                SqlParameter sqlUser = new SqlParameter("@Mode", Mode);
                dbCmd.Parameters.Add(sqlUser);
            }

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        #region Vikash added on 04/05/2024 - 04/08/2024

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool InsertUpdatePirQuest2(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSERT_UPDATE_PIRQUEST2");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static DataSet FillColumHeaders_PIR_Logic_Ass_2(string colID, string section, string desc)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_PIRQCOLHEADS_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(colID))
            {
                SqlParameter sqlSP_code = new SqlParameter("@PIRQCOLH_ID", colID);
                dbCommand.Parameters.Add(sqlSP_code);
            }
            if (!string.IsNullOrEmpty(section))
            {
                SqlParameter sqlBR_Code = new SqlParameter("@PIRQCOLH_SECTION", section);
                dbCommand.Parameters.Add(sqlBR_Code);
            }
            if (!string.IsNullOrEmpty(desc))
            {
                SqlParameter sqlCA_Desc = new SqlParameter("@PIRQCOLH_Description", desc);
                dbCommand.Parameters.Add(sqlCA_Desc);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        // Added by Vikash on 05/06/2024 to save PIR counts
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool UpdatePIRCOUNT2(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UPDATE_PIRCOUNT2");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        #endregion

        #region Vikash added on 12/11/2024 for Invoice Upload Report

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CASB0030_Report(string Agency, string Dept, string Prog, string Year, string BundleNo, string InvStartDte, string InvEndDte, string user)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[CASB0030_Report]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(Agency))
            {
                SqlParameter sqlFdt = new SqlParameter("@AGENCY", Agency);
                dbCmd.Parameters.Add(sqlFdt);
            }

            if (!string.IsNullOrEmpty(Dept))
            {
                SqlParameter sqlUser = new SqlParameter("@DEPARTMENT", Dept);
                dbCmd.Parameters.Add(sqlUser);
            }

            if (!string.IsNullOrEmpty(Prog))
            {
                SqlParameter sqlUser = new SqlParameter("@PROGRAM", Prog);
                dbCmd.Parameters.Add(sqlUser);
            }
            if (!string.IsNullOrEmpty(Year))
            {
                SqlParameter sqlFdt = new SqlParameter("@YEAR", Year);
                dbCmd.Parameters.Add(sqlFdt);
            }

            if (!string.IsNullOrEmpty(BundleNo))
            {
                SqlParameter sqlUser = new SqlParameter("@BUNDLE_NO", BundleNo);
                dbCmd.Parameters.Add(sqlUser);
            }

            if (!string.IsNullOrEmpty(InvStartDte))
            {
                SqlParameter sqlUser = new SqlParameter("@INV_START_DTE", InvStartDte);
                dbCmd.Parameters.Add(sqlUser);
            }

            if (!string.IsNullOrEmpty(InvEndDte))
            {
                SqlParameter sqlUser = new SqlParameter("@INV_END_DATE", InvEndDte);
                dbCmd.Parameters.Add(sqlUser);
            }
            if (!string.IsNullOrEmpty(user))
            {
                SqlParameter sqlUser = new SqlParameter("@USERS", user);
                dbCmd.Parameters.Add(sqlUser);
            }
            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        #endregion

        #region Vikash added on 12/11/2024 for Invoice Upload Report

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet HSSB0126_Report(string Agency, string Dept, string Prog, string Year, string fromDate, string toDate, string funds, string sites, string servTypes)
        {
            DataSet ds;
            Database db;
            string sqlcmd;
            DbCommand dbCmd;

            db = DatabaseFactory.CreateDatabase();
            sqlcmd = "[dbo].[HSSB0126_Report]";
            dbCmd = db.GetStoredProcCommand(sqlcmd);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(Agency))
            {
                SqlParameter sqlFdt = new SqlParameter("@AGENCY", Agency);
                dbCmd.Parameters.Add(sqlFdt);
            }

            if (!string.IsNullOrEmpty(Dept))
            {
                SqlParameter sqlUser = new SqlParameter("@DEPARTMENT", Dept);
                dbCmd.Parameters.Add(sqlUser);
            }

            if (!string.IsNullOrEmpty(Prog))
            {
                SqlParameter sqlUser = new SqlParameter("@PROGRAM", Prog);
                dbCmd.Parameters.Add(sqlUser);
            }
            if (!string.IsNullOrEmpty(Year))
            {
                SqlParameter sqlFdt = new SqlParameter("@YEAR", Year);
                dbCmd.Parameters.Add(sqlFdt);
            }

            if (!string.IsNullOrEmpty(fromDate))
            {
                SqlParameter sqlUser = new SqlParameter("@FROMDATE", fromDate);
                dbCmd.Parameters.Add(sqlUser);
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                SqlParameter sqlUser = new SqlParameter("@TODATE", toDate);
                dbCmd.Parameters.Add(sqlUser);
            }

            if (!string.IsNullOrEmpty(funds))
            {
                SqlParameter sqlUser = new SqlParameter("@FUNDS", funds);
                dbCmd.Parameters.Add(sqlUser);
            }

            if (!string.IsNullOrEmpty(sites))
            {
                SqlParameter sqlUser = new SqlParameter("@SITES", sites);
                dbCmd.Parameters.Add(sqlUser);
            }

            if (!string.IsNullOrEmpty(servTypes))
            {
                SqlParameter sqlUser = new SqlParameter("@SERVTYPES", servTypes);
                dbCmd.Parameters.Add(sqlUser);
            }

            ds = db.ExecuteDataSet(dbCmd);
            return ds;
        }

        #endregion

    }
}