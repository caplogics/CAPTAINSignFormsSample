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
    public partial class FuelControlDB
    {

        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[AGYTAB]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;
        #endregion


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_State_MedianInc_Guide()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_SMGLines]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            //List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //SqlParameter sqlSP_code = new SqlParameter("@Agency", Agency);
            //dbCommand.Parameters.Add(sqlSP_code);
            //SqlParameter sqlBR_Code = new SqlParameter("@Dept", Dept);
            //dbCommand.Parameters.Add(sqlBR_Code);
            //SqlParameter sqlCA_Desc = new SqlParameter("@prog", Program);
            //dbCommand.Parameters.Add(sqlCA_Desc);
            //SqlParameter sqlCA_Active_SW = new SqlParameter("@year", Year);
            //dbCommand.Parameters.Add(sqlCA_Active_SW);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool UpdateState_MedianInc_Guide(string Rowtype, string SMG_Year, string SMG_EligINcome, string SMG_HSIZE_1, string SMG_HSIZE_2, string SMG_HSIZE_3,
            string SMG_HSIZE_4, string SMG_HSIZE_5, string SMG_HSIZE_6, string SMG_HSIZE_7, string SMG_HSIZE_8, string Lstc_Operator)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.Update_SMGLines");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(SMG_Year.Trim()))
            {
                SqlParameter sqlTdate = new SqlParameter("@SMG_Year", SMG_Year);
                _dbCommand.Parameters.Add(sqlTdate);
            }
            if (!string.IsNullOrEmpty(SMG_EligINcome.Trim()))
            {
                SqlParameter sqlGrp_cd = new SqlParameter("@SMG_ELIG_INCOME", SMG_EligINcome);
                _dbCommand.Parameters.Add(sqlGrp_cd);
            }
            if (!string.IsNullOrEmpty(SMG_HSIZE_1.Trim()))
            {
                SqlParameter sqlTblCd = new SqlParameter("@SMG_HSIZE_1", SMG_HSIZE_1);
                _dbCommand.Parameters.Add(sqlTblCd);
            }
            if (!string.IsNullOrEmpty(SMG_HSIZE_2.Trim()))
            {
                SqlParameter sqlTblCd2 = new SqlParameter("@SMG_HSIZE_2", SMG_HSIZE_2);
                _dbCommand.Parameters.Add(sqlTblCd2);
            }
            if (!string.IsNullOrEmpty(SMG_HSIZE_3.Trim()))
            {
                SqlParameter sqlTblCd3 = new SqlParameter("@SMG_HSIZE_3", SMG_HSIZE_3);
                _dbCommand.Parameters.Add(sqlTblCd3);
            }
            if (!string.IsNullOrEmpty(SMG_HSIZE_4.Trim()))
            {
                SqlParameter sqlTblCd4 = new SqlParameter("@SMG_HSIZE_4", SMG_HSIZE_4);
                _dbCommand.Parameters.Add(sqlTblCd4);
            }
            if (!string.IsNullOrEmpty(SMG_HSIZE_5.Trim()))
            {
                SqlParameter sqlTblCd5 = new SqlParameter("@SMG_HSIZE_5", SMG_HSIZE_5);
                _dbCommand.Parameters.Add(sqlTblCd5);
            }
            if (!string.IsNullOrEmpty(SMG_HSIZE_6.Trim()))
            {
                SqlParameter sqlTblCd6 = new SqlParameter("@SMG_HSIZE_6", SMG_HSIZE_6);
                _dbCommand.Parameters.Add(sqlTblCd6);
            }
            if (!string.IsNullOrEmpty(SMG_HSIZE_7.Trim()))
            {
                SqlParameter sqlTblCd7 = new SqlParameter("@SMG_HSIZE_7", SMG_HSIZE_7);
                _dbCommand.Parameters.Add(sqlTblCd7);
            }
            if (!string.IsNullOrEmpty(SMG_HSIZE_8.Trim()))
            {
                SqlParameter sqlTblCd8 = new SqlParameter("@SMG_HSIZE_8", SMG_HSIZE_8);
                _dbCommand.Parameters.Add(sqlTblCd8);
            }
            if (!string.IsNullOrEmpty(Lstc_Operator.Trim()))
            {
                SqlParameter sqlTblCd8 = new SqlParameter("@UserName", Lstc_Operator);
                _dbCommand.Parameters.Add(sqlTblCd8);
            }

            SqlParameter sqlRow = new SqlParameter("@Row_Type", Rowtype);
            _dbCommand.Parameters.Add(sqlRow);

            bool boolsuccess = _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
            return boolsuccess;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_FuelCntl(string Year)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_FUELCNTL]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(Year.Trim()))
            {
                SqlParameter sql_Year = new SqlParameter("@FUEL_Year", Year);
                dbCommand.Parameters.Add(sql_Year);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool UpdateFuelCntl(string Rowtype, string FuelYear, string FUEL_Benefit_Type, string FUEL_Award, string FUEL_Start_Date, string FUEL_End_Date,
            string FUEL_L1_Vulner, string FUEL_L2_Vulner, string FUEL_L3_Vulner, string FUEL_L4_Vulner, string FUEL_L5_Vulner,
            string FUEL_L1_NonVulner, string FUEL_L2_NonVulner, string FUEL_L3_NonVulner, string FUEL_L4_NonVulner, string FUEL_L5_NonVulner, string Add_operator, string Lstc_Operator)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.Update_FUELCNTL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();


            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(FuelYear.Trim()))
            {
                SqlParameter sqlFdate = new SqlParameter("@FCNTL_YEAR", FuelYear);
                _dbCommand.Parameters.Add(sqlFdate);
            }

            if (!string.IsNullOrEmpty(FUEL_Benefit_Type.Trim()))
            {
                SqlParameter sqlFdate = new SqlParameter("@FCNTL_BEN_TYPE", FUEL_Benefit_Type);
                _dbCommand.Parameters.Add(sqlFdate);
            }

            if (!string.IsNullOrEmpty(FUEL_Award.Trim()))
            {
                SqlParameter sqlTdate = new SqlParameter("@FCNTL_Award", FUEL_Award);
                _dbCommand.Parameters.Add(sqlTdate);
            }
            if (!string.IsNullOrEmpty(FUEL_Start_Date.Trim()))
            {
                SqlParameter sqlGrp_cd = new SqlParameter("@FCNTL_SDate", FUEL_Start_Date);
                _dbCommand.Parameters.Add(sqlGrp_cd);
            }
            if (!string.IsNullOrEmpty(FUEL_End_Date.Trim()))
            {
                SqlParameter sqlTblCd = new SqlParameter("@FCNTL_EDate", FUEL_End_Date);
                _dbCommand.Parameters.Add(sqlTblCd);
            }
            if (!string.IsNullOrEmpty(FUEL_L1_Vulner.Trim()))
            {
                SqlParameter sqlTblCd2 = new SqlParameter("@FCNTL_L1_Vul", FUEL_L1_Vulner);
                _dbCommand.Parameters.Add(sqlTblCd2);
            }
            if (!string.IsNullOrEmpty(FUEL_L2_Vulner.Trim()))
            {
                SqlParameter sqlTblCd3 = new SqlParameter("@FCNTL_L2_VuL", FUEL_L2_Vulner);
                _dbCommand.Parameters.Add(sqlTblCd3);
            }

            if (!string.IsNullOrEmpty(FUEL_L3_Vulner.Trim()))
            {
                SqlParameter sqlTblCd3 = new SqlParameter("@FCNTL_L3_Vul", FUEL_L3_Vulner);
                _dbCommand.Parameters.Add(sqlTblCd3);
            }
            if (!string.IsNullOrEmpty(FUEL_L4_Vulner.Trim()))
            {
                SqlParameter sqlTblCd3 = new SqlParameter("@FCNTL_L4_Vul", FUEL_L4_Vulner);
                _dbCommand.Parameters.Add(sqlTblCd3);
            }
            if (!string.IsNullOrEmpty(FUEL_L5_Vulner.Trim()))
            {
                SqlParameter sqlTblCd3 = new SqlParameter("@FCNTL_L5_Vul", FUEL_L5_Vulner);
                _dbCommand.Parameters.Add(sqlTblCd3);
            }

            if (!string.IsNullOrEmpty(FUEL_L1_NonVulner.Trim()))
            {
                SqlParameter sqlTblCd4 = new SqlParameter("@FCNTL_L1_NonVul", FUEL_L1_NonVulner);
                _dbCommand.Parameters.Add(sqlTblCd4);
            }
            if (!string.IsNullOrEmpty(FUEL_L2_NonVulner.Trim()))
            {
                SqlParameter sqlTblCd5 = new SqlParameter("@FCNTL_L2_NonVul", FUEL_L2_NonVulner);
                _dbCommand.Parameters.Add(sqlTblCd5);
            }
            if (!string.IsNullOrEmpty(FUEL_L3_NonVulner.Trim()))
            {
                SqlParameter sqlTblCd6 = new SqlParameter("@FCNTL_L3_NonVul", FUEL_L3_NonVulner);
                _dbCommand.Parameters.Add(sqlTblCd6);
            }
            if (!string.IsNullOrEmpty(FUEL_L4_NonVulner.Trim()))
            {
                SqlParameter sqlTblCd7 = new SqlParameter("@FCNTL_L4_NonVul", FUEL_L4_NonVulner);
                _dbCommand.Parameters.Add(sqlTblCd7);
            }
            if (!string.IsNullOrEmpty(FUEL_L5_NonVulner.Trim()))
            {
                SqlParameter sqlTblCd8 = new SqlParameter("@FCNTL_L5_NonVul", FUEL_L5_NonVulner);
                _dbCommand.Parameters.Add(sqlTblCd8);
            }

            if (!string.IsNullOrEmpty(Add_operator.Trim()))
            {
                SqlParameter sqlTblCd8 = new SqlParameter("@FCNTL_ADD_OPERATOR", Add_operator);
                _dbCommand.Parameters.Add(sqlTblCd8);
            }

            if (!string.IsNullOrEmpty(Lstc_Operator.Trim()))
            {
                SqlParameter sqlTblCd8 = new SqlParameter("@FCNTL_LSTC_OPERATOR", Lstc_Operator);
                _dbCommand.Parameters.Add(sqlTblCd8);
            }

            SqlParameter sqlRow = new SqlParameter("@Row_Type", Rowtype);
            _dbCommand.Parameters.Add(sqlRow);

            bool boolsuccess = _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
            return boolsuccess;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCEAPCNTLData(string Code, string year, string ProgStart, string ProgEnd)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_CEAPCNTL_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Code != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CPCT_CODE", Code);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CPCT_YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (ProgStart != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CPCT_PROG_START", ProgStart);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (ProgEnd != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CPCT_PROG_END", ProgEnd);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCEAPINVData(string Code, string year, string Mode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_CEAPINV]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Code != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CPINV_CPCT_CODE", Code);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CPINV_YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Mode != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Mode", Mode);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool chkCaseActs4SP(string Agency, string Year, string VULServicePlan, string VULServices, string NonVULServicePlan, string NonVULServices, string Mode)
        {
            bool resFlag = false;
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_CEAPINV]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CPINV_YEAR ", Year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (VULServicePlan.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@VUL_SERPLAN", VULServicePlan);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (VULServices.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@VUL_ACTCODE", VULServices);
                dbCommand.Parameters.Add(empnoParm);
            }


            if (NonVULServicePlan.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@NONVUL_SERPLAN", NonVULServicePlan);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (NonVULServices.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@NONVUL_ACTCODE", NonVULServices);
                dbCommand.Parameters.Add(empnoParm);
            }


            if (Mode != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Mode", Mode);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0) {

                if (ds.Tables[0].Rows.Count > 0) {
                    if (ds.Tables[0].Rows[0][0].ToString() == "Y") {
                        resFlag = true;
                    }
                }
            }
            return resFlag;
        }
        public static bool InsertUpdateDelCEAPCNTL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_CEAPCNTL_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }
        public static bool InsertUpdateDelCEAPINV(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_CEAPINV_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }
        public static bool CopytoNextYearCEAPINV(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_CEAPINV_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_TMSBPFIX(string Agency, string Dept, string Prog, string Year, string App)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_TMSBPFIX_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(Agency.Trim()))
            {
                SqlParameter sql_Year = new SqlParameter("@LPB_AGENCY", Agency);
                dbCommand.Parameters.Add(sql_Year);
            }
            if (!string.IsNullOrEmpty(Dept.Trim()))
            {
                SqlParameter sql_Year = new SqlParameter("@LPB_DEPT", Dept);
                dbCommand.Parameters.Add(sql_Year);
            }
            if (!string.IsNullOrEmpty(Prog.Trim()))
            {
                SqlParameter sql_Year = new SqlParameter("@LPB_PROG", Prog);
                dbCommand.Parameters.Add(sql_Year);
            }
            if (!string.IsNullOrEmpty(Year.Trim()))
            {
                SqlParameter sql_Year = new SqlParameter("@LPB_YEAR", Year);
                dbCommand.Parameters.Add(sql_Year);
            }
            if (!string.IsNullOrEmpty(App.Trim()))
            {
                SqlParameter sql_Year = new SqlParameter("@LPB_APP_NO", App);
                dbCommand.Parameters.Add(sql_Year);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


    }
}
