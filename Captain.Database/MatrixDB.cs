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
    public partial class MatrixDB
    {
        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[MATDEF]";
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
        public static DataSet GETMATOUTCMatCode(string strMatCode, string strScrCode)
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

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_MatsHie(string strMatCode, string strScrCode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_MATSHIE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            if (strMatCode != string.Empty)
            {
                SqlParameter matcodeParm = new SqlParameter("@MATSHIE_MAT_CODE", strMatCode);
                dbCommand.Parameters.Add(matcodeParm);
            }
            if (strScrCode != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATSHIE_SCL_CODE", strScrCode);
                dbCommand.Parameters.Add(scrcodeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_MatDefDt()//string strMatCode
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_MATDEFDT]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            //if (strMatCode != string.Empty)
            //{
            //    SqlParameter matcodeParm = new SqlParameter("@MATDEFDT_MAT_CODE", strMatCode);
            //    dbCommand.Parameters.Add(matcodeParm);
            //}
            //if (strScrCode != string.Empty)
            //{
            //    SqlParameter scrcodeParm = new SqlParameter("@MATSHIE_SCL_CODE", strScrCode);
            //    dbCommand.Parameters.Add(scrcodeParm);
            //}

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool UpdateMatsHie(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateMatsHie");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool UpdateMatsDefDt(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.Update_MATDEFDT");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GETMATQUESTQuestions(string strMatCode, string strScrCode, string strGroupcode, string strBmcode, string strOutrcode, string strType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETMATQUESTQuestions]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            if (strMatCode != string.Empty)
            {
                SqlParameter matcodeParm = new SqlParameter("@MATQUEST_MAT_CODE", strMatCode);
                dbCommand.Parameters.Add(matcodeParm);
            }
            if (strScrCode != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATQUEST_SCL_CODE", strScrCode);
                dbCommand.Parameters.Add(scrcodeParm);
            }
            if (strGroupcode != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATQUEST_GROUP", strGroupcode);
                dbCommand.Parameters.Add(scrcodeParm);
            }
            
            if (strBmcode != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATOUTR_BM_CODE", strBmcode);
                dbCommand.Parameters.Add(scrcodeParm);
            }
            if (strOutrcode != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATOUTR_CODE", strOutrcode);
                dbCommand.Parameters.Add(scrcodeParm);
            }
            if (strType != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(scrcodeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GETMATQUESR(string strMatCode, string strScrCode, string strGroupCode, string strQuestioncode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETMATQUESR]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            if (strMatCode != string.Empty)
            {
                SqlParameter matcodeParm = new SqlParameter("@MATQUESR_MAT_CODE", strMatCode);
                dbCommand.Parameters.Add(matcodeParm);
            }
            if (strScrCode != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATQUESR_SCL_CODE", strScrCode);
                dbCommand.Parameters.Add(scrcodeParm);
            }
              if (strScrCode != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATQUESR_GROUP", strGroupCode);
                dbCommand.Parameters.Add(scrcodeParm);
            }
            
            if (strQuestioncode != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATQUESR_CODE", strQuestioncode);
                dbCommand.Parameters.Add(scrcodeParm);
            }


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }









        //   *********************************************************************************    Yeswanth

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_Selected_Table(List<SqlParameter> sqlParamList, string Table_Name)
        {
            DataSet ds;
            Database db;
            //string sqlCommand;
            DbCommand dbCommand;

            string SP_To_Execute = "[dbo].[Browse_" + Table_Name + "]";

            db = DatabaseFactory.CreateDatabase();
            //sqlCommand = "[dbo].[Browse_CASESPM]";
            dbCommand = db.GetStoredProcCommand(SP_To_Execute);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool Update_Sel_Table(List<SqlParameter> sqlParamList, string Table_Name)
        {

            string SP_To_Execute = "dbo.Update" + Table_Name;

            _dbCommand = _dbFactory.GetStoredProcCommand(SP_To_Execute);
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        public static bool Update_Sel_Table(List<SqlParameter> sqlParamList, string Table_Name, out string Sql_Reslut_Msg)
        {

            string SP_To_Execute = "dbo.Update" + Table_Name;

            _dbCommand = _dbFactory.GetStoredProcCommand(SP_To_Execute);
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

        public static bool InsertUpdateDelMatOutr(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSERTUPDATEMATOUTR");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool deleteMATDEF(string Recmode, string MatCode, string ScaleCode, out string msg)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteMATDEF");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //SqlParameter sqlRow_Type = new SqlParameter("@Row_Type", RowType);
            //_dbCommand.Parameters.Add(sqlRow_Type);

            SqlParameter sqlRecMode = new SqlParameter("@Rec_mode", Recmode);
            _dbCommand.Parameters.Add(sqlRecMode);

            SqlParameter sqlMatCode = new SqlParameter("@MATDEF_MAT_CODE", MatCode);
            _dbCommand.Parameters.Add(sqlMatCode);

            SqlParameter sqlScaleCode = new SqlParameter("@MATDEF_SCL_CODE", ScaleCode);
            _dbCommand.Parameters.Add(sqlScaleCode);

            SqlParameter sqlmsg = new SqlParameter("@Msg", SqlDbType.VarChar, 50);
            sqlmsg.Direction = ParameterDirection.Output;
            _dbCommand.Parameters.Add(sqlmsg);

            bool boolsuccess = _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
            msg = sqlmsg.Value.ToString();
            return boolsuccess;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static string GetMATDEF_MATCODE(string type, string Mat_cd)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Get_Matdef_MatCode_New]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            SqlParameter sqltype = new SqlParameter("@Row_Type", type);
            dbCommand.Parameters.Add(sqltype);

            SqlParameter sqlMatCd = new SqlParameter("@MATDEF_MAT_CODE", Mat_cd);
            dbCommand.Parameters.Add(sqlMatCd);

            ds = db.ExecuteDataSet(dbCommand);
            return ds.Tables[0].Rows[0][0].ToString();
        }

        public static bool InsertUpdateDelMatasmt(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelMatasmt");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        public static bool InsertUpdateDelMatapdts(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelMatapdts");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GETMATASMT(string Agency, string Dept, string Program, string Year, string App)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETMATQUESR]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            if (Agency != string.Empty)
            {
                SqlParameter matcodeParm = new SqlParameter("@MATQUESR_MAT_CODE", Agency);
                dbCommand.Parameters.Add(matcodeParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATQUESR_SCL_CODE", Dept);
                dbCommand.Parameters.Add(scrcodeParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATQUESR_CODE", Program);
                dbCommand.Parameters.Add(scrcodeParm);
            }

            if (Year != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATQUESR_CODE", Year);
                dbCommand.Parameters.Add(scrcodeParm);
            }

            if (App != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATQUESR_CODE", App);
                dbCommand.Parameters.Add(scrcodeParm);
            }


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GETMATAPDTS(string Agency, string Dept, string Program, string Year, string App)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETMATQUESR]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            if (Agency != string.Empty)
            {
                SqlParameter matcodeParm = new SqlParameter("@MATQUESR_MAT_CODE", Agency);
                dbCommand.Parameters.Add(matcodeParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATQUESR_SCL_CODE", Dept);
                dbCommand.Parameters.Add(scrcodeParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATQUESR_CODE", Program);
                dbCommand.Parameters.Add(scrcodeParm);
            }


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]

        public static DataSet GETMatapdts(string Agency, string Dept, string Program, string Year, string AppNo, string MatCode, string ssDate, string hienameformat)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETMatapdtsadpyamd]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            if (Agency != string.Empty)
            {
                SqlParameter matcodeParm = new SqlParameter("@MATAPDTS_AGENCY", Agency);
                dbCommand.Parameters.Add(matcodeParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATAPDTS_DEPT", Dept);
                dbCommand.Parameters.Add(scrcodeParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATAPDTS_PROGRAM", Program);
                dbCommand.Parameters.Add(scrcodeParm);
            }


            if (Year != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATAPDTS_YEAR", Year);
                dbCommand.Parameters.Add(scrcodeParm);
            }

            if (AppNo != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATAPDTS_APP", AppNo);
                dbCommand.Parameters.Add(scrcodeParm);
            }

            if (MatCode != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATAPDTS_MAT_CODE", MatCode);
                dbCommand.Parameters.Add(scrcodeParm);
            }

            if (ssDate != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATAPDTS_SS_DATE", ssDate);
                dbCommand.Parameters.Add(scrcodeParm);
            }

            if (hienameformat != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@HIE_CW_FORMAT", hienameformat);
                dbCommand.Parameters.Add(scrcodeParm);
            }


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]

        public static DataSet GETMatDefapdts(string Agency, string Dept, string Program, string Year, string AppNo, string MatCode, string ssDate,string strType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETMATDEFAPDTS]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            if (Agency != string.Empty)
            {
                SqlParameter matcodeParm = new SqlParameter("@MATASMT_AGENCY", Agency);
                dbCommand.Parameters.Add(matcodeParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATASMT_DEPT", Dept);
                dbCommand.Parameters.Add(scrcodeParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATASMT_PROGRAM", Program);
                dbCommand.Parameters.Add(scrcodeParm);
            }


            if (Year != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATASMT_YEAR", Year);
                dbCommand.Parameters.Add(scrcodeParm);
            }

            if (AppNo != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATASMT_APP", AppNo);
                dbCommand.Parameters.Add(scrcodeParm);
            }

            if (MatCode != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATASMT_MAT_CODE", MatCode);
                dbCommand.Parameters.Add(scrcodeParm);
            }


            if (ssDate != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATASMT_SS_DATE", ssDate);
                dbCommand.Parameters.Add(scrcodeParm);
            }
            if (strType != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(scrcodeParm);
            }                        

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]

        public static DataSet GETMatasmt(string Agency, string Dept, string Program, string Year, string AppNo, string MatCode, string strSclcode, string strType, string ssDate, string quescode,string FamSeq)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETMATASMTadpyams]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            if (Agency != string.Empty)
            {
                SqlParameter matcodeParm = new SqlParameter("@MATASMT_AGENCY", Agency);
                dbCommand.Parameters.Add(matcodeParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATASMT_DEPT", Dept);
                dbCommand.Parameters.Add(scrcodeParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATASMT_PROGRAM", Program);
                dbCommand.Parameters.Add(scrcodeParm);
            }


            if (Year != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATASMT_YEAR", Year);
                dbCommand.Parameters.Add(scrcodeParm);
            }

            if (AppNo != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATASMT_APP", AppNo);
                dbCommand.Parameters.Add(scrcodeParm);
            }
            if (FamSeq != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATASMT_FAM_SEQ", FamSeq);
                dbCommand.Parameters.Add(scrcodeParm);
            }

            if (MatCode != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATASMT_MAT_CODE", MatCode);
                dbCommand.Parameters.Add(scrcodeParm);
            }

            if (strSclcode != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATASMT_SCL_CODE", strSclcode);
                dbCommand.Parameters.Add(scrcodeParm);
            }

            if (strType != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATASMT_TYPE", strType);
                dbCommand.Parameters.Add(scrcodeParm);
            }

            if (ssDate != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATASMT_SS_DATE", ssDate);
                dbCommand.Parameters.Add(scrcodeParm);
            }

            if (quescode != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@MATASMT_QUES_CODE", quescode);
                dbCommand.Parameters.Add(scrcodeParm);
            }


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool Updatechangematdates(string Agency, string Dept, string Program, string Year,string AppNo,string Matcode, string SSDate, string Scales, string ChangeDate,string UserName)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UPDATE_MATADATES");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //SqlParameter sqlRow_Type = new SqlParameter("@Row_Type", RowType);
            //_dbCommand.Parameters.Add(sqlRow_Type);

            if (Agency != "**" && !string.IsNullOrEmpty(Agency.Trim()))
            {
                SqlParameter sqlRecMode = new SqlParameter("@MATA_AGENCY", Agency);
                _dbCommand.Parameters.Add(sqlRecMode);
            }

            if (Dept!= "**" && !string.IsNullOrEmpty(Dept.Trim()))
            {
                SqlParameter sqlMatCode = new SqlParameter("@MATA_DEPT", Dept);
                _dbCommand.Parameters.Add(sqlMatCode);
            }

            if (Program != "**" && !string.IsNullOrEmpty(Program.Trim()))
            {
                SqlParameter sqlScaleCode = new SqlParameter("@MATA_PROGRAM", Program);
                _dbCommand.Parameters.Add(sqlScaleCode);
            }

            if (!string.IsNullOrEmpty(Year.Trim()))
            {
                SqlParameter sqlScaleCode = new SqlParameter("@MATA_YEAR", Year);
                _dbCommand.Parameters.Add(sqlScaleCode);
            }
            if (!string.IsNullOrEmpty(AppNo.Trim()))
            {
                SqlParameter sqlScaleCode = new SqlParameter("@MATA_App", AppNo);
                _dbCommand.Parameters.Add(sqlScaleCode);
            }
            if (!string.IsNullOrEmpty(Matcode.Trim()))
            {
                SqlParameter sqlScaleCode = new SqlParameter("@MATA_MATCode", Matcode);
                _dbCommand.Parameters.Add(sqlScaleCode);
            }

            SqlParameter sqlMATA_DATE = new SqlParameter("@MATA_DATE", SSDate);
            _dbCommand.Parameters.Add(sqlMATA_DATE);


            SqlParameter sqlScales = new SqlParameter("@MATA_SCALE", Scales);
            _dbCommand.Parameters.Add(sqlScales);


            SqlParameter sqldate = new SqlParameter("@CHANGEDATE", ChangeDate);
            _dbCommand.Parameters.Add(sqldate);

            SqlParameter sqluser = new SqlParameter("@USERNAME", UserName);
            _dbCommand.Parameters.Add(sqluser);


            bool boolsuccess = _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
            
            return boolsuccess;

        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_MATDEF()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_MATDEF]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool INSERTUPDATEDELSCLGRPS(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSERTUPDATEDELSCLGRPS");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet  GetSCLGRPS(string strMatcode, string strCode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetSCLGRPS]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (strMatcode != string.Empty)
            {
                SqlParameter sqltype = new SqlParameter("@SCLGRP_MAT_CODE", strMatcode);
                dbCommand.Parameters.Add(sqltype);
            }
            if (strCode != string.Empty)
            {
                SqlParameter sqlMatCd = new SqlParameter("@SCLGRP_GRP_CODE", strCode);
                dbCommand.Parameters.Add(sqlMatCd);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        //Added by Vikash on 02/19/2025 for new Matrix Special Report
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet MATB0004_REPORT(string Agency, string Dept, string Program, string Year, string mat_Code, string scale_Code, string fromDate, string toDate, string County)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[MATB0004_REPORT]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter matcodeParm = new SqlParameter("@AGENCY", Agency);
                dbCommand.Parameters.Add(matcodeParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@DEPARTMENT", Dept);
                dbCommand.Parameters.Add(scrcodeParm);
            }
            if (Program != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@PROGRAM", Program);
                dbCommand.Parameters.Add(scrcodeParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@YEAR", Year);
                dbCommand.Parameters.Add(scrcodeParm);
            }


            if (mat_Code != string.Empty)
            {
                SqlParameter matcodeParm = new SqlParameter("@MAT_CODE", mat_Code);
                dbCommand.Parameters.Add(matcodeParm);
            }
            if (scale_Code != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@SCL_CODE", scale_Code);
                dbCommand.Parameters.Add(scrcodeParm);
            }
            if (fromDate != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@FROM_DATE", fromDate);
                dbCommand.Parameters.Add(scrcodeParm);
            }
            if (toDate != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@TO_DATE", toDate);
                dbCommand.Parameters.Add(scrcodeParm);
            }
            if (County != string.Empty)
            {
                SqlParameter scrcodeParm = new SqlParameter("@County", County);
                dbCommand.Parameters.Add(scrcodeParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }
    }
}
