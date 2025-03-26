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
    public partial class FieldControlsDB
    {

        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[FLDCNTL]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;
        #endregion

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCUSTFLDSByScrCode(string ScrCode, string From_Table, string Hie)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCUSTFLDSByScrCode]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (ScrCode != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@ScrCode", ScrCode);
                dbCommand.Parameters.Add(sqlScrCode);
            }
            SqlParameter sqlFrom_Table = new SqlParameter("@FromTable", From_Table);
            dbCommand.Parameters.Add(sqlFrom_Table);

            if (Hie != string.Empty)
            {
                SqlParameter sqlHie = new SqlParameter("@Hie", Hie);
                dbCommand.Parameters.Add(sqlHie);
            }


            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetSelCustDetails(string RecKey)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetSelCustDetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (RecKey != string.Empty)
            {
                SqlParameter sqlRecKey = new SqlParameter("@CustKey", RecKey);
                dbCommand.Parameters.Add(sqlRecKey);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetFLDCNTLByScrCode(string RecKey, string Hierarchy)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetFLDCNTLByScrCode]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (RecKey != string.Empty)
            {
                SqlParameter sqlRecKey = new SqlParameter("@ScrCode", RecKey);
                dbCommand.Parameters.Add(sqlRecKey);
            }
            SqlParameter sqlHierarchy = new SqlParameter("@Hierarchy", Hierarchy);
            dbCommand.Parameters.Add(sqlHierarchy);

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetStatCustDescByScrCodeHIE(string Called_By, string ScrCode, string Hierarchy)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetStatCustDescByScrCodeHIE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (Called_By != string.Empty)
            {
                SqlParameter sqlCalled_By = new SqlParameter("@Calling_Scr", Called_By);
                dbCommand.Parameters.Add(sqlCalled_By);
            }
            if (ScrCode != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@ScrCode", ScrCode);
                dbCommand.Parameters.Add(sqlScrCode);
            }
            if (ScrCode != string.Empty)
            {
                SqlParameter sqlHierarchy = new SqlParameter("@ScrHie", Hierarchy);
                dbCommand.Parameters.Add(sqlHierarchy);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCustRespByCustCode(string CustCode, string strType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCustRespByCustCode]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (CustCode != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@CustCode", CustCode);
                dbCommand.Parameters.Add(sqlScrCode);
            }
            if (strType != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(sqlScrCode);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCustRespByScrCode(string SCRCode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCustRespByScrCode]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (SCRCode != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@ScrCOde", SCRCode);
                dbCommand.Parameters.Add(sqlScrCode);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetLeftStatCustDescByScrCodeHIE(string ScrCode, string Hie)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetLeftStatCustDescByScrCodeHIE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (ScrCode != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@ScrCOde", ScrCode);
                dbCommand.Parameters.Add(sqlScrCode);
            }
            if (Hie != string.Empty)
            {
                SqlParameter sqlHie = new SqlParameter("@ScrHie", Hie);
                dbCommand.Parameters.Add(sqlHie);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetStatCustDescByScrCode(string Called_By, string ScrCode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetStatCustDescByScrCode]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (Called_By != string.Empty)
            {
                SqlParameter sqlCalled_By = new SqlParameter("@Calling_Scr", Called_By);
                dbCommand.Parameters.Add(sqlCalled_By);
            }
            if (ScrCode != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@ScrCOde", ScrCode);
                dbCommand.Parameters.Add(sqlScrCode);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetFLDCNTLHIE(string screenCode, string HIE, string Type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetFLDCNTL_CASEELIG_ByHIE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            if (!screenCode.Equals(string.Empty))
            {
                SqlParameter screenParm = new SqlParameter("@FLDH_SCR_CODE", screenCode);
                dbCommand.Parameters.Add(screenParm);
            }
            if (!HIE.Equals(string.Empty))
            {
                SqlParameter filterParm = new SqlParameter("@FLDH_SCR_HIE", HIE);
                dbCommand.Parameters.Add(filterParm);
            }
            if (!Type.Equals(string.Empty))
            {
                SqlParameter filterParm = new SqlParameter("@Type", Type);
                dbCommand.Parameters.Add(filterParm);
            }
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetFLDCNTLHIE(string screenCode, string HIE, string Type, string strReqType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetFLDCNTL_CASEELIG_ByHIE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            if (!screenCode.Equals(string.Empty))
            {
                SqlParameter screenParm = new SqlParameter("@FLDH_SCR_CODE", screenCode);
                dbCommand.Parameters.Add(screenParm);
            }
            if (!HIE.Equals(string.Empty))
            {
                SqlParameter filterParm = new SqlParameter("@FLDH_SCR_HIE", HIE);
                dbCommand.Parameters.Add(filterParm);
            }
            if (!Type.Equals(string.Empty))
            {
                SqlParameter filterParm = new SqlParameter("@Type", Type);
                dbCommand.Parameters.Add(filterParm);
            }
            if (!strReqType.Equals(string.Empty))
            {
                SqlParameter filterParm = new SqlParameter("@ReqType", strReqType);
                dbCommand.Parameters.Add(filterParm);
            }
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_FLDCNTLHIE(string screenCode, string ScrHie, string FldCode, string Active, string Enab, string Req, string Shar)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CASE2008_Browse_FLDCNTLHIE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            SqlParameter SqlscreenCode = new SqlParameter("@FLDH_SCR_CODE", screenCode);
            dbCommand.Parameters.Add(SqlscreenCode);
            SqlParameter SqlScrHie = new SqlParameter("@FLDH_SCR_HIE", ScrHie);
            dbCommand.Parameters.Add(SqlScrHie);
            SqlParameter SqlFldCode = new SqlParameter("@FLDH_CODE", FldCode);
            dbCommand.Parameters.Add(SqlFldCode);
            SqlParameter SqlActive = new SqlParameter("@FLDH_ACTIVE", Active);
            dbCommand.Parameters.Add(SqlActive);
            SqlParameter SqlEnab = new SqlParameter("@FLDH_ENABLED", Enab);
            dbCommand.Parameters.Add(SqlEnab);
            SqlParameter SqlReq = new SqlParameter("@FLDH_REQUIRED", Req);
            dbCommand.Parameters.Add(SqlReq);
            SqlParameter SqlShar = new SqlParameter("@FLDH_SHARED", Shar);
            dbCommand.Parameters.Add(SqlShar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCustomQuestions(string screenCode, string memberAccess, string HIE, string seq, string type, string questionAccess)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCustomQuestions]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            if (!screenCode.Equals(string.Empty))
            {
                SqlParameter screenParm = new SqlParameter("@SCRCODE", screenCode);
                dbCommand.Parameters.Add(screenParm);
            }
            if (!memberAccess.Equals(string.Empty))
            {
                SqlParameter filterParm = new SqlParameter("@AccessLevel", memberAccess);
                dbCommand.Parameters.Add(filterParm);
            }
            if (!HIE.Equals(string.Empty))
            {
                SqlParameter filterParm = new SqlParameter("@HIE", HIE);
                dbCommand.Parameters.Add(filterParm);
            }
            if (!seq.Equals(string.Empty))
            {
                SqlParameter filterParm = new SqlParameter("@SEQ", seq);
                dbCommand.Parameters.Add(filterParm);
            }
            if (!type.Equals(string.Empty))
            {
                SqlParameter filterParm = new SqlParameter("@TYPE", type);
                dbCommand.Parameters.Add(filterParm);
            }
            if (!questionAccess.Equals(string.Empty))
            {
                SqlParameter filterParm = new SqlParameter("@AccessType", questionAccess);
                dbCommand.Parameters.Add(filterParm);
            }
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetQuestionResponses(string screenCode, string custCode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetQuestionResponse]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            if (!screenCode.Equals(string.Empty))
            {
                SqlParameter screenParm = new SqlParameter("@SCRCODE", screenCode);
                dbCommand.Parameters.Add(screenParm);
            }
            if (!custCode.Equals(string.Empty))
            {
                SqlParameter filterParm = new SqlParameter("@CUSTCODE", custCode);
                dbCommand.Parameters.Add(filterParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool UpdateFLDCNTL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateFLDCNTL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }



        public static bool UpdateCustFlds(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateCustFlds");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool UpdateCUSTRESP(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateCUSTRESP");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        public static bool UpdateFLDCNTLHIE(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateFLDCNTLHIE");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Test4601Report()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;
            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Test4601Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_CUSTRESP(List<SqlParameter> sqlParamList)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_CUSTRESP]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Browse_FldCntl()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_FLDCNTL]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            //foreach (SqlParameter sqlPar in sqlParamList)
            //    dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool DeleteFLDCNTLHIE(string Scr_Code, string Scr_Hierarchy)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CASE2008_DeleteFLDCNTLHIE");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter sqlScr_Code = new SqlParameter("@SCR_CODE", Scr_Code);
            _dbCommand.Parameters.Add(sqlScr_Code);
            SqlParameter sqlScr_Hierarchy = new SqlParameter("@SCR_HIE", Scr_Hierarchy);
            _dbCommand.Parameters.Add(sqlScr_Hierarchy);

            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static bool DeleteCUSTFLDS(string Key, string strscreenName)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.Delete_CUSTFLDS");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter SqlKey = new SqlParameter("@CUSTFLDS_Key", Key);
            _dbCommand.Parameters.Add(SqlKey);
            if (strscreenName != string.Empty)
            {
                SqlParameter screenName = new SqlParameter("@ScreenName", strscreenName);
                _dbCommand.Parameters.Add(screenName);
            }

            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetPreassessData(string strType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETPREASSESSDATA]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            if (!strType.Equals(string.Empty))
            {
                SqlParameter filterParm = new SqlParameter("@TYPE", strType);
                dbCommand.Parameters.Add(filterParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        public static bool InsertUpdatePrechild(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdatePrechild");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GETSCAFLDSDATA(string scrCode, string fldCode, string scrHie, string scrType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETSCAFLDSDATA]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (scrCode != string.Empty)
            {
                SqlParameter sqlCalled_By = new SqlParameter("@SCA_SCR_CODE", scrCode);
                dbCommand.Parameters.Add(sqlCalled_By);
            }
            if (fldCode != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@SCA_FLD_CODE", fldCode);
                dbCommand.Parameters.Add(sqlScrCode);
            }
            if (scrHie != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@SCAH_SCR_HIE", scrHie);
                dbCommand.Parameters.Add(sqlScrCode);
            }
            if (scrType != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@Type", scrType);
                dbCommand.Parameters.Add(sqlScrCode);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }
        public static bool InsertUpdateSCAFLDSDATA(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateSCAFLDSHIE");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static DataSet GETPMTSTDFLDS(string scrCode, string fldCode, string scrHie, string strMode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[PMTSTDFLDS_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (scrCode != string.Empty)
            {
                SqlParameter sqlCalled_By = new SqlParameter("@PSTF_SCR_CODE", scrCode);
                dbCommand.Parameters.Add(sqlCalled_By);
            }
            if (fldCode != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@PSTF_FLD_CATG", fldCode);
                dbCommand.Parameters.Add(sqlScrCode);
            }
            if (scrHie != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@PMFLDH_SCR_HIE", scrHie);
                dbCommand.Parameters.Add(sqlScrCode);
            }
            if (strMode != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@Mode", strMode);
                dbCommand.Parameters.Add(sqlScrCode);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }
        
        //Added by Sudheer on 05/21/2021
        public static DataSet GETPMTSTDFLDSWITHSP(string scrCode, string fldCode, string scrHie,string SPcode,string Branch,string Group,string CACode, string strMode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[PMTSTDFLDS_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (scrCode != string.Empty)
            {
                SqlParameter sqlCalled_By = new SqlParameter("@PSTF_SCR_CODE", scrCode);
                dbCommand.Parameters.Add(sqlCalled_By);
            }
            if (fldCode != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@PSTF_FLD_CATG", fldCode);
                dbCommand.Parameters.Add(sqlScrCode);
            }
            if (scrHie != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@PMFLDH_SCR_HIE", scrHie);
                dbCommand.Parameters.Add(sqlScrCode);
            }
            if (SPcode != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@PMFLDH_SP", SPcode);
                dbCommand.Parameters.Add(sqlScrCode);
            }
            if (Branch != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@PMFLDH_BRANCH", Branch);
                dbCommand.Parameters.Add(sqlScrCode);
            }
            if (Group != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@PMFLDH_CURR_GRP", Group);
                dbCommand.Parameters.Add(sqlScrCode);
            }
            if (CACode != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@PMFLDH_CA_CODE", CACode);
                dbCommand.Parameters.Add(sqlScrCode);
            }
            if (strMode != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@Mode", strMode);
                dbCommand.Parameters.Add(sqlScrCode);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertUpdateDelPMTFLDCNTLH(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.PMTFLDCNTLH_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static DataSet CAPS_AGCYQUESRESP_GET(string Quescode, string strMode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_AGCYQUESRESP_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (Quescode != string.Empty)
            {
                SqlParameter sqlCalled_By = new SqlParameter("@AGYQ_CODE", Quescode);
                dbCommand.Parameters.Add(sqlCalled_By);
            }
            if (strMode != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@Mode", strMode);
                dbCommand.Parameters.Add(sqlScrCode);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static DataSet CAPS_AGCYSERQASSOC_GET(string strServicecode, string Quescode, string strMode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_AGCYSERQASSOC_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (strServicecode != string.Empty)
            {
                SqlParameter sqlCalled_By = new SqlParameter("@AGYSERQ_SERCODE", strServicecode);
                dbCommand.Parameters.Add(sqlCalled_By);
            }
            if (Quescode != string.Empty)
            {
                SqlParameter sqlCalled_By = new SqlParameter("@AGYSERQ_QUESCODE", Quescode);
                dbCommand.Parameters.Add(sqlCalled_By);
            }
            if (strMode != string.Empty)
            {
                SqlParameter sqlScrCode = new SqlParameter("@Mode", strMode);
                dbCommand.Parameters.Add(sqlScrCode);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool CAPS_AGCYSERQASSOC_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_AGCYSERQASSOC_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool CAPS_AGCYPARTQUES_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_AGCYPARTQUES_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool CAPS_AGCYQUESRESP_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_AGCYQUESRESP_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        //public static bool DeleteAGYTAB(string type, string code)
        //{
        //    _dbCommand = _dbFactory.GetStoredProcCommand("dbo.DeleteAGYTAB");
        //    _dbCommand.CommandTimeout = 1200;
        //    _dbCommand.Parameters.Clear();

        //    List<SqlParameter> sqlParamList = new List<SqlParameter>();
        //    SqlParameter sqlAgency = new SqlParameter("@AGYTYPE", type);
        //    _dbCommand.Parameters.Add(sqlAgency);
        //    SqlParameter sqlCode = new SqlParameter("@AGYCODE", code);
        //    _dbCommand.Parameters.Add(sqlCode);

        //    return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
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

        #region Vikash added on 05/22/2024

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet BROWSE_TMM_QUESTIONS_TASKS(string screenCode, string agy_type, string mode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[BROWSE_TMM_QUESTIONS_TASKS]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (!screenCode.Equals(string.Empty))
            {
                SqlParameter SqlscreenCode = new SqlParameter("@CUST_SCR_CODE", screenCode);
                dbCommand.Parameters.Add(SqlscreenCode);
            }

            if (!agy_type.Equals(string.Empty))
            {

                SqlParameter Sqlagy_type = new SqlParameter("@Agy_Type", agy_type);
                dbCommand.Parameters.Add(Sqlagy_type);
            }

            if (!mode.Equals(string.Empty))
            {
                SqlParameter Sqlmode = new SqlParameter("@Mode", mode);
                dbCommand.Parameters.Add(Sqlmode);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        #endregion
    }
}
