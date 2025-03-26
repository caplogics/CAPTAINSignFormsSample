using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;

namespace Captain.DatabaseLayer
{
    [DataObject]
    [Serializable]
   public partial class ChldMst
    {
        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[CHLDMST]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;

        #endregion

        public static bool InsertUpdateDelChldMst(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelChldMst");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetChldMstDetails(string agency, string dep, string program, string year, string app, string seq)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetChldMstDetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CHLDMST_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CHLDMST_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CHLDMST_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CHLDMST_YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (app != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CHLDMST_APP_NO ", app);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (seq != string.Empty )
            {
                SqlParameter empnoParm = new SqlParameter("@CHLDMST_FAMILY_SEQ ", seq);
                dbCommand.Parameters.Add(empnoParm);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetChldMstcase2530Report(string agency, string dep, string program, string year, string app, string seq,string strApplicantDetails)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETChldMstcase2530Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CHLDMST_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CHLDMST_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CHLDMST_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CHLDMST_YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (app != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CHLDMST_APP_NO ", app);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (seq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CHLDMST_FAMILY_SEQ ", seq);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strApplicantDetails != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ApplicantXml ", strApplicantDetails);
                dbCommand.Parameters.Add(empnoParm);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }
        

        public static bool InsertUpdateDelChldEmer(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelChldEmer");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetChldEmerDetails(string agency, string dep, string program, string year, string app, string seq)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetChldEmemDetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMER_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMER_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMER_PROG", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMER_YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (app != string.Empty || app != null)
            {
                SqlParameter empnoParm = new SqlParameter("@EMER_APP_NO ", app);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (seq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMER_SEQ ", seq);
                dbCommand.Parameters.Add(empnoParm);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCaseCondDetails(string agency, string dep, string program, string year, string app, string seq)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCaseCondDetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASECOND_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASECOND_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASECOND_PROG", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASECOND_YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (!string.IsNullOrEmpty(app.Trim()))
            {
                SqlParameter empnoParm = new SqlParameter("@CASECOND_APP_NO ", app);
                dbCommand.Parameters.Add(empnoParm);
            }
           
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertUpdateDelCASECOND(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelCASECOND");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GETIMGUPLOG(string strAgency,string strDept,string strProg,string strYear, string app, string strFamseq, string screen,string strId)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[IMGUPLOG_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (strId != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@IMGLOG_ID", strId);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strAgency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@IMGLOG_AGY", strAgency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strDept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@IMGLOG_DEP", strDept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strProg != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@IMGLOG_PROG", strProg);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strYear != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@IMGLOG_YEAR", strYear);
                dbCommand.Parameters.Add(empnoParm);
            }
            
            if (app != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@IMGLOG_APP", app);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strFamseq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@IMGLOG_FAMILY_SEQ", strFamseq);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (screen != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@IMGLOG_SCREEN", screen);
                dbCommand.Parameters.Add(empnoParm);
            }           

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GETPartnerDocs(string partnerID)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_PARTNERDOCS_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (partnerID != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PRED_ID", partnerID);
                dbCommand.Parameters.Add(empnoParm);
            }
            //if (app != string.Empty)
            //{
            //    SqlParameter empnoParm = new SqlParameter("@IMGLOG_APP", app);
            //    dbCommand.Parameters.Add(empnoParm);
            //}
            //if (screen != string.Empty)
            //{
            //    SqlParameter empnoParm = new SqlParameter("@IMGLOG_SCREEN", screen);
            //    dbCommand.Parameters.Add(empnoParm);
            //}

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static DataSet GETInvDocLOG(string strAgency, string strDept, string strProg, string strYear, string app, string ServPlan, string SPMSeq, string StrType,string strId)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_INVDOCLOG_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (strId != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@INVDOC_ID", strId);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strAgency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@INVDOC_AGENCY", strAgency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strDept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@INVDOC_DEPT", strDept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strProg != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@INVDOC_PROGRAM", strProg);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strYear != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@INVDOC_YEAR", strYear);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (app != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@INVDOC_APP", app);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (ServPlan != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@INVDOC_SERVICEPLAN", ServPlan);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (SPMSeq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@INVDOC_SPM_SEQ", SPMSeq);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (StrType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@INVDOC_VEND_TYPE", StrType);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        public static bool InsertIMGUPLOG(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertIMGUPLOG");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertIMGUPNLOG(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.IMGUPLOG_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertINVDOCNLOG(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INVDOCLOG_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool BRAINIMGES_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.BRAINIMGES_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        public static bool InsertPartnerdocs(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_PARTNERDOCS_INSERT");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCaseBioDetails(string agency, string dep, string program, string year, string app)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCaseBioDetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BIO_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BIO_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BIO_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BIO_YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (!string.IsNullOrEmpty(app.Trim()))
            {
                SqlParameter empnoParm = new SqlParameter("@BIO_APP_NO ", app);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertUpdateDelCASEBIO(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelCASEBIO");
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
