using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace Captain.DatabaseLayer
{
    [DataObject]
    [Serializable]
    public partial class LiheDb
    {
        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[LIHEAPD]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;


        #endregion

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetLiheAPdadpyls(string agency, string dep, string program, string year, string LetterId, string seq, string strType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetLiheAPdadpyls]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@LD_AGENCY", agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@LD_DEPT", dep);
                dbCommand.Parameters.Add(deptParm);
            }
            if (program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@LD_PROG", program);
                dbCommand.Parameters.Add(programParm);
            }
            if (year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LD_YEAR", year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (LetterId != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LD_LETTER_ID", LetterId);
                dbCommand.Parameters.Add(typeParm);
            }

            if (seq != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LD_SEQ", seq);
                dbCommand.Parameters.Add(typeParm);
            }
            if (strType != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LD_Type", strType);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertUpdateDelLiheApd(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelLIHEAPD");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetLiheAppadpyald(string agency, string dep, string program, string year, string App, string LetterId, string LetterDate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetLiheAPPadpyald]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@LAP_AGENCY", agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@LAP_DEPT", dep);
                dbCommand.Parameters.Add(deptParm);
            }
            if (program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@LAP_PROG", program);
                dbCommand.Parameters.Add(programParm);
            }
            if (year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LAP_YEAR", year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (App != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LAP_APP", App);
                dbCommand.Parameters.Add(typeParm);
            }
            if (LetterId != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LAP_LETTER_ID", LetterId);
                dbCommand.Parameters.Add(typeParm);
            }

            if (LetterDate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LAP_LETTER_DATE", LetterDate);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertUpdateDelLiheApp(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelLiheApp");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertUpdateDelLiheApv(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSERTUPDATEDELLIHEAPV");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetLiheApp1adpyalds(string agency, string dep, string program, string year, string App, string LetterId, string LetterDate, string seq)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetLiheApp1adpyalds]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@LAP1_AGENCY", agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@LAP1_DEPT", dep);
                dbCommand.Parameters.Add(deptParm);
            }
            if (program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@LAP1_PROGRAM", program);
                dbCommand.Parameters.Add(programParm);
            }
            if (year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LAP1_YEAR", year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (App != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LAP1_APP", App);
                dbCommand.Parameters.Add(typeParm);
            }
            if (LetterId != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LAP1_LETTER_ID", LetterId);
                dbCommand.Parameters.Add(typeParm);
            }

            if (LetterDate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LAP1_LETTER_DATE", LetterDate);
                dbCommand.Parameters.Add(typeParm);
            }

            if (seq != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LAP1_CATEGORY_SEQ", seq);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetLiheAppvadpyas(string agency, string dep, string program, string year, string App, string vendor, string accountNo, string seq)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GET_LIHEAPV]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1200;

            if (agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@LPV_AGENCY", agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@LPV_DEPT", dep);
                dbCommand.Parameters.Add(deptParm);
            }
            if (program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@LPV_PROGRAM", program);
                dbCommand.Parameters.Add(programParm);
            }
            if (year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LPV_YEAR", year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (App != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LPV_APP_NO", App);
                dbCommand.Parameters.Add(typeParm);
            }
            if (seq != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LPV_SEQ", seq);
                dbCommand.Parameters.Add(typeParm);
            }

            if (vendor != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LPV_VENDOR", vendor);
                dbCommand.Parameters.Add(typeParm);
            }

            if (accountNo != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LPV_ACCOUNT_NO", accountNo);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCATGELIGadpyas(string agency, string dep, string program, string year, string App,  string seq)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAP_CATGELIG_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1200;

            if (agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@CATEG_AGENCY", agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@CATEG_DEPT", dep);
                dbCommand.Parameters.Add(deptParm);
            }
            if (program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@CATEG_PROG", program);
                dbCommand.Parameters.Add(programParm);
            }
            if (year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@CATEG_YEAR", year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (App != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@CATEG_APP_NO", App);
                dbCommand.Parameters.Add(typeParm);
            }
            if (seq != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@CATEG_FAM_SEQ", seq);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertUpdateCATGELIG(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAP_CATGELIG_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GETTMSB0019Report(string agency, string dep, string program, string year, string FromIncome, string ToIncome, string Cerstatus, string State, string Benefitlevel, string BenefitType, string FromRemain, string ToRemain, string FromPaid, string ToPaid,string vendorlist)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETTMSB0019Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 2400;

            if (agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@Agency", agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@Dept", dep);
                dbCommand.Parameters.Add(deptParm);
            }
            if (program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@Prog", program);
                dbCommand.Parameters.Add(programParm);
            }
            if (year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Year", year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (FromIncome != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@FromIncome", FromIncome);
                dbCommand.Parameters.Add(typeParm);
            }
            if (ToIncome != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@ToIncome", ToIncome);
                dbCommand.Parameters.Add(typeParm);
            }

            if (Cerstatus != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@Certifiedstatus", Cerstatus);
                dbCommand.Parameters.Add(typeParm);
            }

            if (State != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@AcrState", State);
                dbCommand.Parameters.Add(typeParm);
            }
            if (Benefitlevel != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@BenefitLevel", Benefitlevel);
                dbCommand.Parameters.Add(typeParm);
            }
            if (BenefitType != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@BenefitType", BenefitType);
                dbCommand.Parameters.Add(typeParm);
            }
            if (FromRemain != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@FromRemain", FromRemain);
                dbCommand.Parameters.Add(typeParm);
            }
            if (ToRemain != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@ToRemain", ToRemain);
                dbCommand.Parameters.Add(typeParm);
            }
            if (FromPaid != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@FromPaid", FromPaid);
                dbCommand.Parameters.Add(typeParm);
            }
            if (ToPaid != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@ToPaid", ToPaid);
                dbCommand.Parameters.Add(typeParm);
            }
            if (vendorlist != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@VendorList", vendorlist);
                dbCommand.Parameters.Add(typeParm);
            } 

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }




        public static bool InsertUpdateDelLiheApp1(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelLiheApp1");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


        public static bool CheckLiheapdCategory(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CheckLiheAPdCategory");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetLiheApcadpyald(string agency, string dep, string program, string year, string App, string LetterId, string LetterDate, string strSeq)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetLiheAPCadpyalds]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@lac_agency", agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@lac_dept", dep);
                dbCommand.Parameters.Add(deptParm);
            }
            if (program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@lac_program", program);
                dbCommand.Parameters.Add(programParm);
            }
            if (year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@lac_year", year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (App != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@lac_app", App);
                dbCommand.Parameters.Add(typeParm);
            }
            if (LetterId != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@lac_letter_id", LetterId);
                dbCommand.Parameters.Add(typeParm);
            }

            if (LetterDate != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@lac_letter_date", LetterDate);
                dbCommand.Parameters.Add(typeParm);
            }

            if (strSeq != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LAC_CATEGORY_SEQ", strSeq);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertUpdateDelLiheApc(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSERTUPDATEDELLIHEAPC");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetTMSELIGData(string agency, string dept, string Prog, string year, string CatElig,string Round)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GETTMSELIG]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1200;

            List<SqlParameter> sqlParamList = new List<SqlParameter>();


            SqlParameter sql_agency = new SqlParameter("@TMSELIG_AGENCY", agency);
            dbCommand.Parameters.Add(sql_agency);
            SqlParameter sql_dept = new SqlParameter("@TMSELIG_DEPT", dept);
            dbCommand.Parameters.Add(sql_dept);

            SqlParameter sql_Prog = new SqlParameter("@TMSELIG_PROG", Prog);
            dbCommand.Parameters.Add(sql_Prog);

            SqlParameter sql_year = new SqlParameter("@TMSELIG_YEAR", year);
            dbCommand.Parameters.Add(sql_year);


            SqlParameter sql_county = new SqlParameter("@TMSELIG_CATELIG", CatElig);
            dbCommand.Parameters.Add(sql_county);

            SqlParameter sql_zipcode = new SqlParameter("@TMSELIG_ROUND", Round);
            dbCommand.Parameters.Add(sql_zipcode);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertBULKCATELIG(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSERTUPDATE_BULK_CATELIG");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CapsLihwApvGet(string agency, string dep, string program, string year, string App, string vendor, string accountNo, string seq)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_LIHWAPV_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1200;

            if (agency != string.Empty)
            {
                SqlParameter agencyParm = new SqlParameter("@LWV_AGENCY", agency);
                dbCommand.Parameters.Add(agencyParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter deptParm = new SqlParameter("@LWV_DEPT", dep);
                dbCommand.Parameters.Add(deptParm);
            }
            if (program != string.Empty)
            {
                SqlParameter programParm = new SqlParameter("@LWV_PROGRAM", program);
                dbCommand.Parameters.Add(programParm);
            }
            if (year != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LWV_YEAR", year);
                dbCommand.Parameters.Add(typeParm);
            }
            if (App != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LWV_APP_NO", App);
                dbCommand.Parameters.Add(typeParm);
            }
            if (seq != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LWV_SEQ", seq);
                dbCommand.Parameters.Add(typeParm);
            }

            if (vendor != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LWV_VENDOR", vendor);
                dbCommand.Parameters.Add(typeParm);
            }

            if (accountNo != string.Empty)
            {
                SqlParameter typeParm = new SqlParameter("@LWV_ACCOUNT_NO", accountNo);
                dbCommand.Parameters.Add(typeParm);
            }

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool CAPS_LIHWAPV_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_LIHWAPV_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CAP_WATERCNTL_GET(string Year, string BenType, string Award, string SDate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAP_WATERCNTL_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(Year.Trim()))
            {
                SqlParameter sql_Year = new SqlParameter("@WCNTL_YEAR", Year);
                dbCommand.Parameters.Add(sql_Year);
            }
            if (!string.IsNullOrEmpty(BenType.Trim()))
            {
                SqlParameter sql_BenType = new SqlParameter("@WCNTL_BEN_TYPE", BenType);
                dbCommand.Parameters.Add(sql_BenType);
            }
            if (!string.IsNullOrEmpty(Award.Trim()))
            {
                SqlParameter sql_Award = new SqlParameter("@WCNTL_Award", Award);
                dbCommand.Parameters.Add(sql_Award);
            }

            if (!string.IsNullOrEmpty(SDate.Trim()))
            {
                SqlParameter sql_SDate = new SqlParameter("@WCNTL_Date", SDate);
                dbCommand.Parameters.Add(sql_SDate);
            }

            //if (!string.IsNullOrEmpty(EDate.Trim()))
            //{
            //    SqlParameter sql_EDate = new SqlParameter("@WCNTL_EDate", EDate);
            //    dbCommand.Parameters.Add(sql_EDate);
            //}

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool CAPS_LIHWAP_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_LIHWAP_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CAPS_LIHWAP_GET(string Agency, string Dept, string program, string Year, string Applno, string seq, string strType, string strDate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_LIHWAP_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(Agency.Trim()))
            {
                SqlParameter sql_Year = new SqlParameter("@LPW_AGENCY", Agency);
                dbCommand.Parameters.Add(sql_Year);
            }
            if (!string.IsNullOrEmpty(Dept))
            {
                SqlParameter sqlparam = new SqlParameter("@LPW_DEPT", Dept);
                dbCommand.Parameters.Add(sqlparam);
            }
            if (!string.IsNullOrEmpty(program))
            {
                SqlParameter sqlparam = new SqlParameter("@LPW_PROG", program);
                dbCommand.Parameters.Add(sqlparam);
            }
            if (!string.IsNullOrEmpty(Year))
            {
                SqlParameter sqlparam = new SqlParameter("@LPW_YEAR", Year);
                dbCommand.Parameters.Add(sqlparam);
            }
            if (!string.IsNullOrEmpty(Applno))
            {
                SqlParameter sqlparam = new SqlParameter("@LPW_APP_NO", Applno);
                dbCommand.Parameters.Add(sqlparam);
            }
            if (!string.IsNullOrEmpty(seq))
            {
                SqlParameter sqlparam = new SqlParameter("@LPW_SEQ", seq);
                dbCommand.Parameters.Add(sqlparam);
            }
            //if (!string.IsNullOrEmpty(strType))
            //{
            //    SqlParameter sqlparam = new SqlParameter("@LPW_TYPE", strType);
            //    dbCommand.Parameters.Add(sqlparam);
            //}
            if (!string.IsNullOrEmpty(strDate))
            {
                SqlParameter sqlparam = new SqlParameter("@LPW_BEN_DATE", strDate);
                dbCommand.Parameters.Add(sqlparam);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }
    }
}
