using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace Captain.DatabaseLayer
{
   
    [DataObject]
    [Serializable]
    public partial class ChldTrckDB
    {
        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[CASESUM]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;

        #endregion


        public static bool InsertUpdateDelChldTrck(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelChldTrck");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetChldTrckDetails(string ApplType, string ApplCode, string ApplProg, string Component, string Task, string TaskDescription)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetChldTrckDetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (ApplType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@TRCK_AGENCY", ApplType);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (ApplCode != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@TRCK_DEPT", ApplCode);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (ApplProg != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@TRCK_PROGRAM", ApplProg);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Component != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@TRCK_COMPONENT ", Component);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Task != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@TRCK_TASK ", Task);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (TaskDescription != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@TRCK_TASK_DESCRIPTION ", TaskDescription);
                dbCommand.Parameters.Add(empnoParm);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        public static DataSet BrowseTrckDetails(string RepSeq)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Browse_ChldTrckDetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (RepSeq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Track_Seq", RepSeq);
                dbCommand.Parameters.Add(empnoParm);
            }
            
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertUpdateDelChldTrckR(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelChldTrckR");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetChldTrckRDetails(string ApplType, string ApplCode, string ApplProg, string Component, string Task, string Fund)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetChldTrckRDetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (ApplType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@TRCKR_AGENCY", ApplType);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (ApplCode != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@TRCKR_DEPT", ApplCode);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (ApplProg != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@TRCKR_PROG", ApplProg);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Component != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@TRCKR_COMPONENT ", Component);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Task != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@TRCKR_TASK ", Task);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Fund != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@TRCKR_FUND ", Fund);
                dbCommand.Parameters.Add(empnoParm);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool InsertUpdateDelChldMedi(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelCHLDMEDI");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }



        public static bool UpdateMediFix(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UpdateMediFix");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }


           [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetChldMediDetails(string Agency, string Dept, string Prog, string Year, string ApplNo, string Task, string Seq)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCHLDMEDI]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@MEDI_AGENCY", Agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Dept != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@MEDI_DEPT", Dept);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Prog != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@MEDI_PROG", Prog);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@MEDI_YEAR ", Year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (ApplNo != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@MEDI_APP_NO ", ApplNo);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Task != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@MEDI_TASK ", Task);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Seq != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@MEDI_SEQ ", Seq);
                dbCommand.Parameters.Add(empnoParm);
            }
            //if (Component != string.Empty)
            //{
            //    SqlParameter empnoParm = new SqlParameter("@MEDI_COMPONENT ", Component);
            //    dbCommand.Parameters.Add(empnoParm);
            //}
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



           [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
           public static DataSet GetMediFix()
           {
               DataSet ds;
               Database db;
               string sqlCommand;
               DbCommand dbCommand;

               db = DatabaseFactory.CreateDatabase();
               sqlCommand = "[dbo].[GETMEDIFIX]";
               dbCommand = db.GetStoredProcCommand(sqlCommand);

               
               ds = db.ExecuteDataSet(dbCommand);
               return ds;
           }

        public static bool INSUPDEL_TASKUPLDS(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.INSUPDEL_TASKUPLDS");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }



        public static bool InsertUpdateDelChldMedResp(List<SqlParameter> sqlParamList)
           {
               _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelCHLDMEDRSP");
               _dbCommand.CommandTimeout = 1200;
               _dbCommand.Parameters.Clear();
               foreach (SqlParameter sqlPar in sqlParamList)
               {
                   _dbCommand.Parameters.Add(sqlPar);
               }
               return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
           }


           [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
           public static DataSet GetChldMedRespDetails(string Agency, string Dept, string Prog, string Year, string ApplNo, string Que)
           {
               DataSet ds;
               Database db;
               string sqlCommand;
               DbCommand dbCommand;

               db = DatabaseFactory.CreateDatabase();
               sqlCommand = "[dbo].[GETCHLDMEDRSP]";
               dbCommand = db.GetStoredProcCommand(sqlCommand);

               if (Agency != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@MEDRSP_AGENCY", Agency);
                   dbCommand.Parameters.Add(empnoParm);
               }
               if (Dept != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@MEDRSP_DEPT", Dept);
                   dbCommand.Parameters.Add(empnoParm);
               }
               if (Prog != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@MEDRSP_PROG", Prog);
                   dbCommand.Parameters.Add(empnoParm);
               }
               if (Year != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@MEDRSP_YEAR", Year);
                   dbCommand.Parameters.Add(empnoParm);
               }
               if (ApplNo != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@MEDRSP_APP_NO", ApplNo);
                   dbCommand.Parameters.Add(empnoParm);
               }
               //if (Task != string.Empty)
               //{
               //    SqlParameter empnoParm = new SqlParameter("@MEDRSP_TASK", Task);
               //    dbCommand.Parameters.Add(empnoParm);
               //}
               //if (Seq != string.Empty)
               //{
               //    SqlParameter empnoParm = new SqlParameter("@MEDRSP_SEQ", Seq);
               //    dbCommand.Parameters.Add(empnoParm);
               //}              
               if (Que != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@MEDRSP_Que", Que);
                   dbCommand.Parameters.Add(empnoParm);
               }
               ds = db.ExecuteDataSet(dbCommand);
               return ds;
           }

           public static DataSet GetHSSB2106_Report(string Agency, string Dept, string Prog, string Year,string AppNo,string Age,string MulSites,string FundHie,string Active,string Enrl_stat,string FrmAge,string ToAge ,string SortCol)
           {
               DataSet ds;
               Database db;
               string sqlCommand;
               DbCommand dbCommand;

               db = DatabaseFactory.CreateDatabase();
               sqlCommand = "[dbo].[HSSB2106_Report]";//[Get_ChildLists]";
               dbCommand = db.GetStoredProcCommand(sqlCommand);
               if (string.IsNullOrEmpty(Agency))
                   dbCommand.CommandTimeout = 1200;
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

               if (AppNo != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@MST_APP_NO", AppNo);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (Age != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@BASEAGE", Age);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (MulSites != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@Site", MulSites);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (FundHie != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@FundHie", FundHie);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (Active != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@ACTIVE", Active);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (Enrl_stat != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@ENRL_STATUS", Enrl_stat);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (FrmAge != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@FrmAge", FrmAge);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (ToAge != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@ToAge", ToAge);
                   dbCommand.Parameters.Add(empnoParm);
               }

               SqlParameter SqlSort = new SqlParameter("@SEQ", SortCol);
               dbCommand.Parameters.Add(SqlSort);

               ds = db.ExecuteDataSet(dbCommand);
               return ds;
           }

           public static DataSet GetHSSB0124_GridApp(string Agency, string Dept, string Prog, string Year, string AppNo, string Age, string MulSites, string FundHie, string Active, string Enrl_stat, string FrmAge, string ToAge, string SortCol,string ExcludeIntakes)
           {
               DataSet ds;
               Database db;
               string sqlCommand;
               DbCommand dbCommand;

               db = DatabaseFactory.CreateDatabase();
               sqlCommand = "[dbo].[HSSB0124_GridApp]";//[Get_ChildLists]";
               dbCommand = db.GetStoredProcCommand(sqlCommand);
               dbCommand.CommandTimeout = 600;
               
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

               if (AppNo != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@MST_APP_NO", AppNo);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (Age != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@BASEAGE", Age);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (MulSites != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@Site", MulSites);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (FundHie != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@FundHie", FundHie);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (Active != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@ACTIVE", Active);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (Enrl_stat != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@ENRL_STATUS", Enrl_stat);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (FrmAge != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@FrmAge", FrmAge);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (ToAge != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@ToAge", ToAge);
                   dbCommand.Parameters.Add(empnoParm);
               }

            if (ExcludeIntakes != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ExcludeIntakes", ExcludeIntakes);
                dbCommand.Parameters.Add(empnoParm);
            }

            SqlParameter SqlSort = new SqlParameter("@SEQ", SortCol);
               dbCommand.Parameters.Add(SqlSort);

               ds = db.ExecuteDataSet(dbCommand);
               return ds;
           }

           public static DataSet GetHSSB2106_ChldMedi(string Agency, string Dept, string Prog, string Year, string AppNo, string SBCB_Date, string Task, string Type,string Sequence)
           {
               DataSet ds;
               Database db;
               string sqlCommand;
               DbCommand dbCommand;

               db = DatabaseFactory.CreateDatabase();
               sqlCommand = "[dbo].[HSSB2106_Chldmedi]";//[Get_ChildLists]";
               dbCommand = db.GetStoredProcCommand(sqlCommand);
               if (string.IsNullOrEmpty(Agency))
                   dbCommand.CommandTimeout = 1200;
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

               if (AppNo != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@MST_APP_NO", AppNo);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (SBCB_Date != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@SBCBDate", SBCB_Date);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (Task != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@Task", Task);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (Type != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@Type", Type);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (Sequence != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@Sequence", Sequence);
                   dbCommand.Parameters.Add(empnoParm);
               }

               
               ds = db.ExecuteDataSet(dbCommand);
               return ds;
           }


           public static DataSet GetHSSB0123_Report(string Agency, string Dept, string Prog, string Year, string AppNo, string Age, string MulSites, string FundHie, string Active, string Tasks, string FrmAge, string ToAge, string FrmBMI, string ToBMI, string SortCol)
           {
               DataSet ds;
               Database db;
               string sqlCommand;
               DbCommand dbCommand;

               db = DatabaseFactory.CreateDatabase();
               sqlCommand = "[dbo].[HSSB0123_REPORT]";//[Get_ChildLists]";
               dbCommand = db.GetStoredProcCommand(sqlCommand);
               if (string.IsNullOrEmpty(Agency))
                   dbCommand.CommandTimeout = 1200;
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

               if (AppNo != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@MST_APP_NO", AppNo);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (Age != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@BASEAGE", Age);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (MulSites != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@Site", MulSites);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (FundHie != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@FundHie", FundHie);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (Active != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@ACTIVE", Active);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (Tasks != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@Tasks", Tasks);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (FrmAge != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@FrmAge", FrmAge);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (ToAge != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@ToAge", ToAge);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (FrmBMI != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@FrmBMI", FrmBMI);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (ToBMI != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@ToBMI", ToBMI);
                   dbCommand.Parameters.Add(empnoParm);
               }

               SqlParameter SqlSort = new SqlParameter("@SEQ", SortCol);
               dbCommand.Parameters.Add(SqlSort);

               ds = db.ExecuteDataSet(dbCommand);
               return ds;
           }


           public static DataSet GetHSSB0115_Report(string Agency, string Dept, string Prog, string Year, string Age, string MulSites, string FundHie, string FrmAge, string ToAge, string IncomStat,string RankCatg, string Clients,string FundSw)
           {
               DataSet ds;
               Database db;
               string sqlCommand;
               DbCommand dbCommand;

               db = DatabaseFactory.CreateDatabase();
               sqlCommand = "[dbo].[HSSB2115_Waiting_List]";//[Get_ChildLists]";
               dbCommand = db.GetStoredProcCommand(sqlCommand);
               dbCommand.CommandTimeout = 1200;
               
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

               if (Age != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@BASEAGE", Age);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (MulSites != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@Site", MulSites);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (FundHie != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@FundHie", FundHie);
                   dbCommand.Parameters.Add(empnoParm);
               }

               
               if (FrmAge != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@FrmAge", FrmAge);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (ToAge != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@ToAge", ToAge);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (IncomStat != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@Income", IncomStat); 
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (RankCatg != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@Rankcatg", RankCatg); 
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (Clients != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@Clients", Clients);
                   dbCommand.Parameters.Add(empnoParm);
               }
               if (FundSw != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@FundSw", FundSw);
                   dbCommand.Parameters.Add(empnoParm);
               }

               ds = db.ExecuteDataSet(dbCommand);
               return ds;
           }

           public static DataSet GetHSSB0150_Report(string Agency, string Dept, string Prog, string Year, string FromDate, string Todate, string FrmSite, string ToSite, string FrmRoom, string ToRoom)
           {
               DataSet ds;
               Database db;
               string sqlCommand;
               DbCommand dbCommand;

               db = DatabaseFactory.CreateDatabase();
               sqlCommand = "[dbo].[HSSB00150_Report]";//[Get_ChildLists]";
               dbCommand = db.GetStoredProcCommand(sqlCommand);
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
                   SqlParameter empnoParm = new SqlParameter("@PROGRAM", Prog);
                   dbCommand.Parameters.Add(empnoParm);
               }
               if (Year != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@YEAR", Year);
                   dbCommand.Parameters.Add(empnoParm);
               }

               //if (Age != string.Empty)
               //{
               //    SqlParameter empnoParm = new SqlParameter("@BASEAGE", Age);
               //    dbCommand.Parameters.Add(empnoParm);
               //}

               //if (MulSites != string.Empty)
               //{
               //    SqlParameter empnoParm = new SqlParameter("@Site", MulSites);
               //    dbCommand.Parameters.Add(empnoParm);
               //}

               //if (FundHie != string.Empty)
               //{
               //    SqlParameter empnoParm = new SqlParameter("@FundHie", FundHie);
               //    dbCommand.Parameters.Add(empnoParm);
               //}


               if (FromDate != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@FromDate", FromDate);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (Todate != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@TOdate", Todate);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (FrmSite != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@FromSite", FrmSite);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (ToSite != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@ToSite", ToSite);
                   dbCommand.Parameters.Add(empnoParm);
               }

               if (FrmRoom  != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@FromRoom", FrmRoom);
                   dbCommand.Parameters.Add(empnoParm);
               }
               if (ToRoom != string.Empty)
               {
                   SqlParameter empnoParm = new SqlParameter("@ToRoom", ToRoom);
                   dbCommand.Parameters.Add(empnoParm);
               }

               ds = db.ExecuteDataSet(dbCommand);
               return ds;
           }

           [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
           public static bool Update_chldtrack(string Task, string Gchart, string GchartSel)
           {
               _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UPDATE_CHLDTRCK_CHARTS");
               _dbCommand.CommandTimeout = 1200;
               _dbCommand.Parameters.Clear();

               List<SqlParameter> sqlParamList = new List<SqlParameter>();
               SqlParameter sqlSP_Code = new SqlParameter("@TASK", Task);
               _dbCommand.Parameters.Add(sqlSP_Code);
               SqlParameter sqlBranch = new SqlParameter("@GCHART_CODE", Gchart);
               _dbCommand.Parameters.Add(sqlBranch);
               SqlParameter sqlOrg_group = new SqlParameter("@GCHART_SEL", GchartSel);
               _dbCommand.Parameters.Add(sqlOrg_group);

               return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
           }

    }
}
