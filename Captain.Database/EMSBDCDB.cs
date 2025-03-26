using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.SqlClient;

namespace Captain.DatabaseLayer
{

    [DataObject]
    [Serializable]
    public partial class EMSBDCDB
    {
        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[EMSBDC]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;


        #endregion


        public static bool InsertUpdateDelEmsbdc(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelEmsbdc");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEmsBdcData(string agency, string dep, string program, string year, string costcenter, string GlAccount, string BudgetYear, string Desc, string Fund, string IntOrder, string AccountType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetEMSBDCDetails]";
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
            if (costcenter != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_COST_CENTER ", costcenter);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (GlAccount != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_GL_ACCOUNT", GlAccount);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (BudgetYear != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_BUDGET_YEAR", BudgetYear);
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
            if (IntOrder != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_INT_ORDER", IntOrder);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (AccountType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_ACCOUNT_TYPE", AccountType);
                dbCommand.Parameters.Add(empnoParm);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEmsB3014Report(string agency, string dep, string program, string year, string Fund, string FromDate, string Todate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[EMSB0014_Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

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
                SqlParameter empnoParm = new SqlParameter("@YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (FromDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FROMDATE", FromDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Todate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@TODATE", Todate);
                dbCommand.Parameters.Add(empnoParm);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEmsBdcFundCheck(string agency, string dep, string program, string year, string Fund, string startDate, string EndDate, string NewstartDate, string NewEndDate, string strType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetEMSBDCCheckFund]";
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
        public static DataSet GetEMSBDCCalAmount(string agency, string dep, string program, string year, string Fund, string startDate, string EndDate, string strType, string strBudgetYear, string strBudget)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetEMSBDCCalAmount]";
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
            if (strBudgetYear != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_BUDGET_YEAR", strBudgetYear);
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







        public static bool InsertUpdateDelEmsbda(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelEmsbda");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEmsBdaData(string agency, string dep, string program, string year, string costcenter, string GlAccount, string BudgetYear, string seq)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetEMSBDADetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDA_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDA_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDA_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDA_YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (costcenter != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDA_COST_CENTER ", costcenter);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (GlAccount != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDA_GL_ACCOUNT", GlAccount);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (BudgetYear != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDA_BUDGET_YEAR", BudgetYear);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (seq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDA_SEQ", seq);
                dbCommand.Parameters.Add(empnoParm);
            }


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        public static bool InsertUpdateDelEmsclcpmc(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelEMSCLCPMC");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertEmsclcpmcLOG(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSERTEMSCLCPMCLOG");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEmsclcpmcData(string agency, string dep, string program, string year, string App, string Fund, string Fundseq, string ServiceSeq, string strDate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetEMSCLCPMCDetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (App != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_APP ", App);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_RES_FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fundseq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_RES_SEQ", Fundseq);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (ServiceSeq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_SEQ", ServiceSeq);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_RES_DATE", strDate);
                dbCommand.Parameters.Add(empnoParm);
            }


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEmsb0026_ClcData(string agency, string dep, string program, string year, string Fund)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[EMSB0026_CLCRECS]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

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
                SqlParameter empnoParm = new SqlParameter("@YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMSCLCPMC00030Details(string agency, string dep, string program, string year, string App, string Fund, string Servicecode, string Site, string strDate, string strDateH, string strCLC_RES_SEQ, string strCLC_SEQ, string strCLC_RES_DATE, string strType,string strVendorL, string strVendorH)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetEMSCLCPMC00030Details]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 2400;
            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (App != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_APP ", App);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_RES_FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Servicecode != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_S_SERVICE_CODE", Servicecode);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@MST_SITE", Site);
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
            if (strCLC_RES_SEQ != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_RES_SEQ", strCLC_RES_SEQ);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strCLC_SEQ != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_SEQ", strCLC_SEQ);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strCLC_RES_DATE != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_RES_DATE", strCLC_RES_DATE);
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





        public static bool InsertUpdateDelEmsres(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelEMSRES");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertUpdateDelEmsres0050(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UPDATE_EMSRES0050");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertUpdateDelEmsBUDCAROVER(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_EMSBUDCARYOVER_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEmsResData(string agency, string dep, string program, string year, string App, string Fund, string Fundseq, string Date, string strType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetEMSRESDetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMSRES_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMSRES_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMSRES_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMSRES_YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (App != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMSRES_APP ", App);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMSRES_FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fundseq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMSRES_SEQ", Fundseq);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Date != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMSRES_DATE", Date);
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
        public static DataSet GetEMSBUDCARYOVER(string agency, string dep, string program, string year, string App, string Fund, string Fundseq, string Date, string strType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetEMSBUDCARYOVER]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMSBCO_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMSBCO_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMSBCO_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMSBCO_YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (App != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMSBCO_APP ", App);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMSBCO_FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fundseq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMSBCO_SEQ", Fundseq);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Date != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMSBCO_DATE", Date);
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



        public static bool InsertUpdateDelEmssp(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelEMSSP");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEmsspData(string Fund, string Type, string Code)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetEMSSPDetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMSSP_FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Type != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMSSP_TYPE", Type);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Code != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMSSP_CAMS_CODE", Code);
                dbCommand.Parameters.Add(empnoParm);
            }


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        public static bool InsertUpdateDelEmsclapma(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUPdateDelEMSCLAPMA");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEmsclapmaData(string agency, string dep, string program, string year, string App, string Fund, string Fundseq, string ServiceSeq, string strSeq, string resdate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetEMSCLAPMADetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLA_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLA_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLA_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLA_YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (App != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLA_APP ", App);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLA_RES_FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fundseq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLA_RES_SEQ", Fundseq);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (resdate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLA_RES_DATE", resdate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (ServiceSeq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLA_CLC_SEQ", ServiceSeq);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strSeq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLA_SEQ", strSeq);
                dbCommand.Parameters.Add(empnoParm);
            }


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEmsOboData(string Id, string seq)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetEMSOBO]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Id != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMSOBO_ID", Id);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (seq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EMSOBO_SEQ", seq);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertUpdateDelEmsobo(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateEMSOBO");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool UpdateCASEVOT(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UPDATE_CASEVOT");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMSBReports(string agency, string dep, string program, string year, string Site, string Caseworker, string Fund, string FormType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[EMSB_Reports]";
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
            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Site", Site);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Caseworker != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEWORKER", Caseworker);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (FormType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ReportType", FormType);
                dbCommand.Parameters.Add(empnoParm);
            }


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMSB0012(string agency, string dep, string program, string year, string Site, string Caseworker, string Fund)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[EMSB_3012]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

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
            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Site", Site);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Caseworker != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEWORKER", Caseworker);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            //if (Date != string.Empty)
            //{
            //    SqlParameter empnoParm = new SqlParameter("@EMSRES_DATE", Date);
            //    dbCommand.Parameters.Add(empnoParm);
            //}


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMSB0011(string agency, string dep, string program, string year, string CaseType, string Site, string Caseworker, string Fund, string BudRefDate, string Fromdt, string Todt)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[EMSB0011_PaidInvoices]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 2400;

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SAGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SDEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SPROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SYEAR", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (CaseType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASETYPE", CaseType);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Site", Site);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Caseworker != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEWORKER", Caseworker);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SFUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (BudRefDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BudRefDate", BudRefDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fromdt != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FromDate", Fromdt);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Todt != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@TODATE", Todt);
                dbCommand.Parameters.Add(empnoParm);
            }


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMSB0008_Presets(string Agency, string Dept, string Program, string Year, string Site, string Caseworker)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[EMSB0008_Presets]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

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
            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Site", Site);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Caseworker != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEWORKER", Caseworker);
                dbCommand.Parameters.Add(empnoParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMSB0017(string agency, string dep, string program, string year, string CaseType, string Site, string Caseworker, string Fund, string FrmDt, string Todt, string Sort)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[EMSB3017_Zipcode]";
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
            if (CaseType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASETYPE", CaseType);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Site", Site);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Caseworker != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEWORKER", Caseworker);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (FrmDt != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@fromdate", FrmDt);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Todt != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@todate", Todt);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Sort != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Sort", Sort);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMSB0021(string agency, string dep, string program, string year, string CaseType, string Site, string Caseworker, string Fund, string Vendor, string Acc_type,string dateRange , string FromDate ,string Todate )
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[EMSB0021_ActivityReport]";
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
            if (CaseType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASETYPE", CaseType);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Site", Site);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Caseworker != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEWORKER", Caseworker);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Vendor != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@VENDOR", Vendor);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Acc_type != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ACCOUNT_TYPE", Acc_type);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dateRange != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@dateRange", dateRange);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (FromDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FromDate", FromDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Todate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Todate", Todate);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMSB0024(string agency, string dep, string program, string year, string Site, string AdjCd, string Caseworker, string Fund, string UserId, string Service, string CostCentre, string Acc, string CountyYear, string Vendor, string Acc_type)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[EMSB0024_Report]";
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
            if (CostCentre != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@COSTCENTER", CostCentre);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Acc != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ACCOUNT", Acc);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (CountyYear != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@COUNTYYEAR", CountyYear);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Vendor != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@VENDOR", Vendor);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Acc_type != string.Empty && Acc_type != "**")
            {
                SqlParameter empnoParm = new SqlParameter("@ACCTYPE", Acc_type);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMSB0004(string agency, string dep, string program, string year, string CaseType, string Site, string Caseworker, string Fund, string apprordend, string report, string Fromdate, string Todate, string BudRefDate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[EMSB0004_Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 4800;

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
            if (CaseType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASETYPE", CaseType);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Site", Site);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Caseworker != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEWORKER", Caseworker);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (apprordend != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@APPORDEND", apprordend);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (report != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@REPORT", report);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fromdate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FROMDATE", Fromdate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Todate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@TODATE", Todate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (BudRefDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@RefDate", BudRefDate);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMSB0026(string agency, string dep, string program, string year, string Site, string Caseworker, string Fund, string Worker_Sw, string SweepInterval, string Sweepdays)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[EMSB0026_Report]";
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
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Site != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SITE", Site);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Caseworker != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@WORKER", Caseworker);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Worker_Sw != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Worker_Sw", Worker_Sw);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (SweepInterval != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SweepInterval", SweepInterval);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Sweepdays != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SWEEPTEXT", Sweepdays);
                dbCommand.Parameters.Add(empnoParm);
            }


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMSB0025(string agency, string dep, string program, string year)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[EMSB0025_Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

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
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMSB0023(string agency, string dep, string program, string year,string Applicants)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[EMSB0023_Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

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
            if (Applicants.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ApplicantXml", Applicants);
                dbCommand.Parameters.Add(empnoParm);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMSB0015(string agency, string dep, string program, string year, string Fund, string Site, string Worker, string Fromdate, string Todate, string BudRefdate, string RepType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[EMSB0015_REPORT]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

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
            if (Fund.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Site.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SITE", Site);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Worker.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@WORKER", Worker);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fromdate.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FromDate", Fromdate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Todate.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ToDate", Todate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (BudRefdate.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BudgetRefDate", BudRefdate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (RepType.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@RepType", RepType);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMSB0009(string agency, string dep, string program, string year, string Fund, string Site, string Worker, string Fromdate, string Todate, string Approval, string FileName, string FinalRun)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[EMSB3009_Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

            if (agency != string.Empty)
            {
                SqlParameter empAGENCY = new SqlParameter("@AGENCY", agency);
                dbCommand.Parameters.Add(empAGENCY);
            }
            if (dep != string.Empty)
            {
                SqlParameter empdep = new SqlParameter("@DEPT", dep);
                dbCommand.Parameters.Add(empdep);
            }
            if (program != string.Empty)
            {
                SqlParameter empprogram = new SqlParameter("@PROGRAM", program);
                dbCommand.Parameters.Add(empprogram);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empyear = new SqlParameter("@YEAR", year);
                dbCommand.Parameters.Add(empyear);
            }
            if (Fund.Trim() != string.Empty)
            {
                SqlParameter empFund = new SqlParameter("@FUND", Fund);
                dbCommand.Parameters.Add(empFund);
            }
            if (Site.Trim() != string.Empty)
            {
                SqlParameter empSite = new SqlParameter("@SITE", Site);
                dbCommand.Parameters.Add(empSite);
            }
            if (Worker.Trim() != string.Empty)
            {
                SqlParameter empWorker = new SqlParameter("@WORKER", Worker);
                dbCommand.Parameters.Add(empWorker);
            }
            if (Fromdate.Trim() != string.Empty)
            {
                SqlParameter empFromdate = new SqlParameter("@FROMDATE", Fromdate);
                dbCommand.Parameters.Add(empFromdate);
            }
            if (Todate.Trim() != string.Empty)
            {
                SqlParameter empTodate = new SqlParameter("@TODATE", Todate);
                dbCommand.Parameters.Add(empTodate);
            }
            if (Approval.Trim() != string.Empty)
            {
                SqlParameter empApproval = new SqlParameter("@APPROVAL", Approval);
                dbCommand.Parameters.Add(empApproval);
            }
            if (FileName.Trim() != string.Empty)
            {
                SqlParameter empFileName = new SqlParameter("@FILENAME", FileName);
                dbCommand.Parameters.Add(empFileName);
            }
            if (FinalRun.Trim() != string.Empty)
            {
                SqlParameter empFileName = new SqlParameter("@FinalRun", FinalRun);
                dbCommand.Parameters.Add(empFileName);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GETCASEVOT(string strCity, string strStreet, string strSuffix)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETCASEVOT]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

            if (strCity != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEVOT_CITY", strCity);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strStreet != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEVOT_STREET", strStreet);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strSuffix != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEVOT_SUFFIX", strSuffix);
                dbCommand.Parameters.Add(empnoParm);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMSB0006(string agency, string dep, string program, string year, string Site, string Worker, string Fromdate, string Todate, string Date_sw, string Del_sw, string Days)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[EMSB0006_Report_New]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

            if (agency != string.Empty)
            {
                SqlParameter empAGENCY = new SqlParameter("@AGENCY", agency);
                dbCommand.Parameters.Add(empAGENCY);
            }
            if (dep != string.Empty)
            {
                SqlParameter empdep = new SqlParameter("@DEPT", dep);
                dbCommand.Parameters.Add(empdep);
            }
            if (program != string.Empty)
            {
                SqlParameter empprogram = new SqlParameter("@PROGRAM", program);
                dbCommand.Parameters.Add(empprogram);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empyear = new SqlParameter("@YEAR", year);
                dbCommand.Parameters.Add(empyear);
            }

            if (Site.Trim() != string.Empty)
            {
                SqlParameter empSite = new SqlParameter("@SITE", Site);
                dbCommand.Parameters.Add(empSite);
            }
            if (Worker.Trim() != string.Empty)
            {
                SqlParameter empWorker = new SqlParameter("@WORKER", Worker);
                dbCommand.Parameters.Add(empWorker);
            }
            if (Fromdate.Trim() != string.Empty)
            {
                SqlParameter empFromdate = new SqlParameter("@FROMDATE", Fromdate);
                dbCommand.Parameters.Add(empFromdate);
            }
            if (Todate.Trim() != string.Empty)
            {
                SqlParameter empTodate = new SqlParameter("@TODATE", Todate);
                dbCommand.Parameters.Add(empTodate);
            }
            if (Date_sw.Trim() != string.Empty)
            {
                SqlParameter empApproval = new SqlParameter("@DATE_SW", Date_sw);
                dbCommand.Parameters.Add(empApproval);
            }
            if (Del_sw.Trim() != string.Empty)
            {
                SqlParameter empFileName = new SqlParameter("@DEL_SW", Del_sw);
                dbCommand.Parameters.Add(empFileName);
            }
            if (Days.Trim() != string.Empty)
            {
                SqlParameter empFileName = new SqlParameter("@DAYS", Days);
                dbCommand.Parameters.Add(empFileName);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMSB0027(string agency, string dep, string program, string year, string ServiceCode, string Site, string Worker, string Fund, string Bdc_Date, string Fromdate, string Todate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[EMSB0027_Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

            if (agency != string.Empty)
            {
                SqlParameter empAGENCY = new SqlParameter("@AGENCY", agency);
                dbCommand.Parameters.Add(empAGENCY);
            }
            if (dep != string.Empty)
            {
                SqlParameter empdep = new SqlParameter("@DEPT", dep);
                dbCommand.Parameters.Add(empdep);
            }
            if (program != string.Empty)
            {
                SqlParameter empprogram = new SqlParameter("@PROGRAM", program);
                dbCommand.Parameters.Add(empprogram);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empyear = new SqlParameter("@YEAR", year);
                dbCommand.Parameters.Add(empyear);
            }

            if (ServiceCode.Trim() != string.Empty)
            {
                SqlParameter empyear = new SqlParameter("@SERVICE_CODE", ServiceCode);
                dbCommand.Parameters.Add(empyear);
            }

            if (Site.Trim() != string.Empty)
            {
                SqlParameter empSite = new SqlParameter("@SITE", Site);
                dbCommand.Parameters.Add(empSite);
            }
            if (Worker.Trim() != string.Empty)
            {
                SqlParameter empWorker = new SqlParameter("@WORKER", Worker);
                dbCommand.Parameters.Add(empWorker);
            }

            if (Fund.Trim() != string.Empty)
            {
                SqlParameter empWorker = new SqlParameter("@FUND", Fund);
                dbCommand.Parameters.Add(empWorker);
            }

            if (Bdc_Date.Trim() != string.Empty)
            {
                SqlParameter empWorker = new SqlParameter("@BDC_DATE", Bdc_Date);
                dbCommand.Parameters.Add(empWorker);
            }

            if (Fromdate.Trim() != string.Empty)
            {
                SqlParameter empFromdate = new SqlParameter("@FromDate", Fromdate);
                dbCommand.Parameters.Add(empFromdate);
            }
            if (Todate.Trim() != string.Empty)
            {
                SqlParameter empTodate = new SqlParameter("@ToDate", Todate);
                dbCommand.Parameters.Add(empTodate);
            }


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static string GETEMSLOCKVALIDDATA(string agency, string dep, string program, string year, string App, string Fund, string Fundseq, string ServiceSeq, string strDate,string strBdccostcenter, string strbdcGlAccount, string strBudgetYear, string strIntOrder, string strAccountType, string strBdcstart, string strBdcEnd, string strType)
        {
            string strOutMsg;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETEMSLOCKVALIDATE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }

            SqlParameter clcyear = new SqlParameter("@CLC_YEAR ", year);
            dbCommand.Parameters.Add(clcyear);

            if (App != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_APP ", App);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_RES_FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fundseq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_RES_SEQ", Fundseq);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (ServiceSeq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_SEQ", ServiceSeq);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CLC_RES_DATE", strDate);
                dbCommand.Parameters.Add(empnoParm);
            }            
            if (strBdccostcenter != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_COST_CENTER", strBdccostcenter);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strbdcGlAccount != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_GL_ACCOUNT", strbdcGlAccount);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strBudgetYear != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_BUDGET_YEAR", strBudgetYear);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strIntOrder != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_INT_ORDER", strIntOrder);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strAccountType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_ACCOUNT_TYPE", strAccountType);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strBdcstart != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_START", strBdcstart);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strBdcEnd != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDC_END", strBdcEnd);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(empnoParm);
            }

            SqlParameter sqloutMsg = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            sqloutMsg.Value = string.Empty;
            sqloutMsg.Direction = ParameterDirection.Output;
            dbCommand.Parameters.Add(sqloutMsg);
            db.ExecuteDataSet(dbCommand);
            strOutMsg = sqloutMsg.Value.ToString();

            return strOutMsg;
        }
           


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMSB0028(string agency, string dep, string program, string year, string Fund, string Site, string Worker, string Fromdate, string Todate, string BudRefdate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[EMSB0028_Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

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
            if (Fund.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Site.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Site", Site);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Worker.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEWORKER", Worker);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fromdate.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FromDate", Fromdate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Todate.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ToDate", Todate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (BudRefdate.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BudgetRefDate", BudRefdate);
                dbCommand.Parameters.Add(empnoParm);
            }
            
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMSB2028(string agency, string dep, string program, string year, string Fund, string Site, string Worker, string Fromdate, string Todate, string BudRefdate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[EMSB2028_Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

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
            if (Fund.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Site.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Site", Site);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Worker.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASEWORKER", Worker);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fromdate.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FromDate", Fromdate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Todate.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ToDate", Todate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (BudRefdate.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BudgetRefDate", BudRefdate);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMSB0029(string agency, string dep, string program, string year, string casetype, string Worker, string Sites, string Fund, string Rep,string dateSw,string Fromdate,string Todate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[EMSB0029_Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

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
            if (casetype.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASETYPE", casetype);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Worker.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@WORKER", Worker);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Sites.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Site", Sites);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Rep.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SUMMARY_SW", Rep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dateSw.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@DATE_SW", dateSw);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fromdate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FromDate", Fromdate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Todate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Todate", Todate);
                dbCommand.Parameters.Add(empnoParm);
            }
            
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertEMSLOCKDATA(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.GETEMSLOCKVALIDATE");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet  GETEMSLOCK(string screentype, string struserid)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETEMSLOCKED]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

            if (screentype != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ScreenType", screentype);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (struserid != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Userid", struserid);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool UpdateEMSLOCK(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UPDATEEMSLOCKED");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertDelEMSB0023(List<SqlParameter> sqlParamList)
        {
            bool boolstatus = false;
            string strerror = string.Empty;
            try
            {

                _dbCommand = _dbFactory.GetStoredProcCommand("dbo.EMSB0023_TEXTRECS");
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
                strerror = ex.Message;
                boolstatus = false;
            }
            
            return boolstatus;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMS00050(string agency, string dep, string program, string year,  string Fund, string startdate, string Endate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_EMS00050RESDATA]";
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
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FUND", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
           
            
            if (startdate  != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@STARTDT", startdate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Endate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ENDDT", Endate);
                dbCommand.Parameters.Add(empnoParm);
            }


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertUpdateDelInvoiceLog(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INVOICELOG_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet INVOICE_LOG_GET(string logid, string agency, string dep, string program, string year, string App)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[INVOICELOG_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1200;
            if(logid !=string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@INVLOG_ID", logid);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@INVLOG_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@INVLOG_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@INVLOG_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            //if (year.Trim() != string.Empty)
            //{
            //    SqlParameter empnoParm = new SqlParameter("@INVLOG_YEAR", year);
            //    dbCommand.Parameters.Add(empnoParm);
            //}
            if (App != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@INVLOG_APP", App);
                dbCommand.Parameters.Add(empnoParm);
            }
            

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetEMSB1009(string agency, string dep, string program, string year, string Fund, string Site, string Worker, string Fromdate, string Todate, string Selection,string strReportupload)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[EMSB1009_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

            if (agency != string.Empty)
            {
                SqlParameter empAGENCY = new SqlParameter("@AGENCY", agency);
                dbCommand.Parameters.Add(empAGENCY);
            }
            if (dep != string.Empty)
            {
                SqlParameter empdep = new SqlParameter("@DEPT", dep);
                dbCommand.Parameters.Add(empdep);
            }
            if (program != string.Empty)
            {
                SqlParameter empprogram = new SqlParameter("@PROGRAM", program);
                dbCommand.Parameters.Add(empprogram);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empyear = new SqlParameter("@YEAR", year);
                dbCommand.Parameters.Add(empyear);
            }
            if (Fund.Trim() != string.Empty)
            {
                SqlParameter empFund = new SqlParameter("@FUND", Fund);
                dbCommand.Parameters.Add(empFund);
            }
            if (Site.Trim() != string.Empty)
            {
                SqlParameter empSite = new SqlParameter("@SITE", Site);
                dbCommand.Parameters.Add(empSite);
            }
            if (Worker.Trim() != string.Empty)
            {
                SqlParameter empWorker = new SqlParameter("@WORKER", Worker);
                dbCommand.Parameters.Add(empWorker);
            }
            if (Fromdate.Trim() != string.Empty)
            {
                SqlParameter empFromdate = new SqlParameter("@FROMDATE", Fromdate);
                dbCommand.Parameters.Add(empFromdate);
            }
            if (Todate.Trim() != string.Empty)
            {
                SqlParameter empTodate = new SqlParameter("@TODATE", Todate);
                dbCommand.Parameters.Add(empTodate);
            }
            if (Selection.Trim() != string.Empty)
            {
                SqlParameter empApproval = new SqlParameter("@SELECTION", Selection);
                dbCommand.Parameters.Add(empApproval);
            }
            if (strReportupload.Trim() != string.Empty)
            {
                SqlParameter empApproval = new SqlParameter("@Reportupload", strReportupload);
                dbCommand.Parameters.Add(empApproval);
            }

            
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


    }
}
