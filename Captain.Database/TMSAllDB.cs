using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

namespace Captain.DatabaseLayer
{


    [DataObject]
    [Serializable]
  public  class TMSAllDB
    {
        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[Notice]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        //private static DbCommand _dbCommand;


        #endregion

      [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GETNOTICE(string agency, string dep, string program, string year, string app, string byProg,string Type)
      {
          DataSet ds;
          Database db;
          string sqlCommand;
          DbCommand dbCommand;

          db = DatabaseFactory.CreateDatabase();
          sqlCommand = "[dbo].[GETNOTICE]";
          dbCommand = db.GetStoredProcCommand(sqlCommand);

          if (agency != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@VDN_AGENCY", agency);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (dep != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@VDN_DEPT", dep);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (program != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@VDN_PROGRAM", program);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (year != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@VDN_YEAR ", year);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (app != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@VDN_APP_NO ", app);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (byProg != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@VDN_BY_PROG", byProg);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (Type != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@VDN_TYPE", Type);
              dbCommand.Parameters.Add(empnoParm);
          }
          
          ds = db.ExecuteDataSet(dbCommand);
          return ds;
      }


      [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
      public static DataSet GETTMSB4015(string agency, string dep, string program, string year, string FromDate, string Todate)
      {
          DataSet ds;
          Database db;
          string sqlCommand;
          DbCommand dbCommand;

          db = DatabaseFactory.CreateDatabase();
          sqlCommand = "[dbo].[TMSB4015_Report_New]";
          dbCommand = db.GetStoredProcCommand(sqlCommand);

          if (agency != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@LPB_AGENCY", agency);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (dep != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@LPB_DEPT", dep);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (program != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@LPB_PROG", program);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (year != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@LPB_YEAR", year);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (FromDate != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@FROM_DATE", FromDate);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (Todate != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@TO_DATE", Todate);
              dbCommand.Parameters.Add(empnoParm);
          }
          

          ds = db.ExecuteDataSet(dbCommand);
          return ds;
      }

      [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
      public static DataSet GETTMSB4003(string agency, string dep, string program, string year, string FromDate, string Todate,string Disable,string Elder,string selsite,string seltown)
      {
          DataSet ds;
          Database db;
          string sqlCommand;
          DbCommand dbCommand;

          db = DatabaseFactory.CreateDatabase();
          sqlCommand = "[dbo].[TMSB4003_REPORT]";
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
          if (year != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@YEAR", year);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (FromDate != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@FromDate", FromDate);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (Todate != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@ToDate", Todate);
              dbCommand.Parameters.Add(empnoParm);
          }

          if (Disable != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@DISABLE", Disable);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (Elder != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@ELDER", Elder);
              dbCommand.Parameters.Add(empnoParm);
          }

          if (selsite != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@SelSite", selsite);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (seltown != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@seltown", seltown);
              dbCommand.Parameters.Add(empnoParm);
          }

          ds = db.ExecuteDataSet(dbCommand);
          return ds;
      }


      [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
      public static DataSet GETTMSB4005(string agency, string dep, string program, string year)
      {
          DataSet ds;
          Database db;
          string sqlCommand;
          DbCommand dbCommand;

          db = DatabaseFactory.CreateDatabase();
          sqlCommand = "[dbo].[TMSB4005_Report]";
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
          if (year != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@YEAR", year);
              dbCommand.Parameters.Add(empnoParm);
          }
          //if (FromDate != string.Empty)
          //{
          //    SqlParameter empnoParm = new SqlParameter("@FromDate", FromDate);
          //    dbCommand.Parameters.Add(empnoParm);
          //}
          //if (Todate != string.Empty)
          //{
          //    SqlParameter empnoParm = new SqlParameter("@ToDate", Todate);
          //    dbCommand.Parameters.Add(empnoParm);
          //}

          //if (Disable != string.Empty)
          //{
          //    SqlParameter empnoParm = new SqlParameter("@DISABLE", Disable);
          //    dbCommand.Parameters.Add(empnoParm);
          //}
          //if (Elder != string.Empty)
          //{
          //    SqlParameter empnoParm = new SqlParameter("@ELDER", Elder);
          //    dbCommand.Parameters.Add(empnoParm);
          //}

          //if (selsite != string.Empty)
          //{
          //    SqlParameter empnoParm = new SqlParameter("@SelSite", selsite);
          //    dbCommand.Parameters.Add(empnoParm);
          //}
          //if (seltown != string.Empty)
          //{
          //    SqlParameter empnoParm = new SqlParameter("@seltown", seltown);
          //    dbCommand.Parameters.Add(empnoParm);
          //}

          ds = db.ExecuteDataSet(dbCommand);
          return ds;
      }

      [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
      public static DataSet GETTMSB4004(string agency, string dep, string program, string year,string Site)
      {
          DataSet ds;
          Database db;
          string sqlCommand;
          DbCommand dbCommand;

          db = DatabaseFactory.CreateDatabase();
          sqlCommand = "[dbo].[TMSB4004_Report]";
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
              SqlParameter empnoParm = new SqlParameter("@PROG", program);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (year != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@YEAR", year);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (Site != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@Site", Site);
              dbCommand.Parameters.Add(empnoParm);
          }
          //if (Todate != string.Empty)
          //{
          //    SqlParameter empnoParm = new SqlParameter("@ToDate", Todate);
          //    dbCommand.Parameters.Add(empnoParm);
          //}

          //if (Disable != string.Empty)
          //{
          //    SqlParameter empnoParm = new SqlParameter("@DISABLE", Disable);
          //    dbCommand.Parameters.Add(empnoParm);
          //}
          //if (Elder != string.Empty)
          //{
          //    SqlParameter empnoParm = new SqlParameter("@ELDER", Elder);
          //    dbCommand.Parameters.Add(empnoParm);
          //}

          //if (selsite != string.Empty)
          //{
          //    SqlParameter empnoParm = new SqlParameter("@SelSite", selsite);
          //    dbCommand.Parameters.Add(empnoParm);
          //}
          //if (seltown != string.Empty)
          //{
          //    SqlParameter empnoParm = new SqlParameter("@seltown", seltown);
          //    dbCommand.Parameters.Add(empnoParm);
          //}

          ds = db.ExecuteDataSet(dbCommand);
          return ds;
      }

      [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
      public static DataSet GetReRunDates(string agency, string dep, string program, string year,string FormName,string Vendor)
      {
          DataSet ds;
          Database db;
          string sqlCommand;
          DbCommand dbCommand;

          db = DatabaseFactory.CreateDatabase();
          sqlCommand = "[dbo].[Get_ReRundates]";
          dbCommand = db.GetStoredProcCommand(sqlCommand);

          if (agency != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@VDN_AGENCY", agency);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (dep != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@VDN_DEPT", dep);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (program != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@VDN_PROGRAM", program);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (year != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@VDN_YEAR", year);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (FormName != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@VDN_BY_PROG", FormName);
              dbCommand.Parameters.Add(empnoParm);
          }

          if (Vendor != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@VDN_VENDOR", Vendor);
              dbCommand.Parameters.Add(empnoParm);
          }

          ds = db.ExecuteDataSet(dbCommand);
          return ds;
      }


      [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
      public static DataSet GetTMS81PrintReport(string agency, string dep, string program, string year, string appno)
      {
          DataSet ds;
          Database db;
          string sqlCommand;
          DbCommand dbCommand;

          db = DatabaseFactory.CreateDatabase();
          sqlCommand = "[dbo].[TMS81_PrintLetter]";
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
          if (year != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@YEAR", year);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (appno != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@APPNO", appno);
              dbCommand.Parameters.Add(empnoParm);
          }

          ds = db.ExecuteDataSet(dbCommand);
          return ds;
      }


      [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
      public static DataSet GetTMSB4010_Bundling(string agency, string dep, string program, string year, string PBR_No)
      {
          DataSet ds;
          Database db;
          string sqlCommand;
          DbCommand dbCommand;

          db = DatabaseFactory.CreateDatabase();
          sqlCommand = "[dbo].[TMSB4010_Bundling]";
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
              SqlParameter empnoParm = new SqlParameter("@PROG", program);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (year != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@YEAR", year);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (PBR_No != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@PBR_NO_N", PBR_No);
              dbCommand.Parameters.Add(empnoParm);
          }

          ds = db.ExecuteDataSet(dbCommand);
          return ds;
      }

      [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
      public static DataSet GetMissing_Check_Apps(string agency, string dep, string program, string year, string PBR_No)
      {
          DataSet ds;
          Database db;
          string sqlCommand;
          DbCommand dbCommand;

          db = DatabaseFactory.CreateDatabase();
          sqlCommand = "[dbo].[Missing_Check_Apps]";
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
              SqlParameter empnoParm = new SqlParameter("@PROG", program);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (year != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@YEAR", year);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (PBR_No != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@PBR_NO_N", PBR_No);
              dbCommand.Parameters.Add(empnoParm);
          }

          ds = db.ExecuteDataSet(dbCommand);
          return ds;
      }

      [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
      public static DataSet BundlingReport(string agency, string dep, string program, string year, string Rep_Type, string frmdt, string Todt, string Client, decimal Amt, string Vendor, string User)
      {
          DataSet ds;
          Database db;
          string sqlCommand;
          DbCommand dbCommand;

          db = DatabaseFactory.CreateDatabase();
          sqlCommand = "[dbo].[Bundling_Report]";
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
          if (Rep_Type != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@Rep_Type", Rep_Type);
              dbCommand.Parameters.Add(empnoParm);
          }

          if (Client != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@Client_Type", Client);
              dbCommand.Parameters.Add(empnoParm);
          }

          if (Vendor != string.Empty)
          {
              SqlParameter empVend = new SqlParameter("@Sel_Vendor", Vendor);
              dbCommand.Parameters.Add(empVend);
          }

          
          SqlParameter frmdta = new SqlParameter("@From_Date", frmdt);
          dbCommand.Parameters.Add(frmdta);

          SqlParameter todta = new SqlParameter("@To_Date", Todt);
          dbCommand.Parameters.Add(todta);

          //if (string.IsNullOrEmpty(Amt.ToString().Trim()))
          //{
              SqlParameter BAMt = new SqlParameter("@Bundle_Amount", Amt);
              dbCommand.Parameters.Add(BAMt);
          //}

              SqlParameter SqlUser = new SqlParameter("@User", User);
              dbCommand.Parameters.Add(SqlUser);


          ds = db.ExecuteDataSet(dbCommand);
          return ds;
      }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet TMSB0030_Report(string agency, string dep, string program, string year, string Rep_Type, string frmdt, string Todt, string Client, decimal Amt, string Vendor, string User)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[TMSB0030_GET]";
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
            if (Rep_Type != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Rep_Type", Rep_Type);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Client != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Client_Type", Client);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Vendor != string.Empty)
            {
                SqlParameter empVend = new SqlParameter("@Sel_Vendor", Vendor);
                dbCommand.Parameters.Add(empVend);
            }


            SqlParameter frmdta = new SqlParameter("@From_Date", frmdt);
            dbCommand.Parameters.Add(frmdta);

            SqlParameter todta = new SqlParameter("@To_Date", Todt);
            dbCommand.Parameters.Add(todta);

            //if (string.IsNullOrEmpty(Amt.ToString().Trim()))
            //{
            SqlParameter BAMt = new SqlParameter("@Bundle_Amount", Amt);
            dbCommand.Parameters.Add(BAMt);
            //}

            SqlParameter SqlUser = new SqlParameter("@User", User);
            dbCommand.Parameters.Add(SqlUser);


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
      public static DataSet GetTMSCHECKDetails(string agency, string dep, string program, string year, string Vendor)
      {
          DataSet ds;
          Database db;
          string sqlCommand;
          DbCommand dbCommand;

          db = DatabaseFactory.CreateDatabase();
          sqlCommand = "[dbo].[TMSBCHCT_FILLCHECK]";
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
          if (year != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@YEAR", year);
              dbCommand.Parameters.Add(empnoParm);
          }

          //if (FormName != string.Empty)
          //{
          //    SqlParameter empnoParm = new SqlParameter("@VDN_BY_PROG", FormName);
          //    dbCommand.Parameters.Add(empnoParm);
          //}

          if (Vendor != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@VENDOR", Vendor);
              dbCommand.Parameters.Add(empnoParm);
          }

          ds = db.ExecuteDataSet(dbCommand);
          return ds;
      }

      [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
      public static DataSet GetLoadChecks(string agency, string dep, string program, string year, string Vendor, string Chkdate)
      {
          DataSet ds;
          Database db;
          string sqlCommand;
          DbCommand dbCommand;

          db = DatabaseFactory.CreateDatabase();
          sqlCommand = "[dbo].[TMSBCHCT_LoadChecks]";
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
          if (year != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@YEAR", year);
              dbCommand.Parameters.Add(empnoParm);
          }

          

          if (Vendor != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@VENDOR", Vendor);
              dbCommand.Parameters.Add(empnoParm);
          }

          if (Chkdate != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@CHKDATE", Chkdate);
              dbCommand.Parameters.Add(empnoParm);
          }

          ds = db.ExecuteDataSet(dbCommand);
          return ds;
      }


      [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
      public static DataSet GetTMSBRecalc(string agency, string dep, string program, string year, string Update,string User)
      {
          DataSet ds;
          Database db;
          string sqlCommand;
          DbCommand dbCommand;

          db = DatabaseFactory.CreateDatabase();
          sqlCommand = "[dbo].[Recalc_Benefit]";
          dbCommand = db.GetStoredProcCommand(sqlCommand);

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

          //if (FormName != string.Empty)
          //{
          //    SqlParameter empnoParm = new SqlParameter("@VDN_BY_PROG", FormName);
          //    dbCommand.Parameters.Add(empnoParm);
          //}

          if (Update != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@Update", Update);
              dbCommand.Parameters.Add(empnoParm);
          }
          if (User != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@User", User);
              dbCommand.Parameters.Add(empnoParm);
          }

          ds = db.ExecuteDataSet(dbCommand);
          return ds;
      }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetTMSB0027Report(string agency, string dep, string program, string year, string Reason, string FromDate,string ToDate,string ReasonCodes)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[TMSB0027_Report]";
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
            if (year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@YEAR", year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Reason != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Reason", Reason);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (FromDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FROMDATE", FromDate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (ToDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@TODATE", ToDate);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (ReasonCodes != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Reason_Filter_Codes", ReasonCodes);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
      public static DataSet DelTMSB0044(string agency, string dep, string program, string year)
      {
          DataSet ds;
          Database db;
          string sqlCommand;
          DbCommand dbCommand;

          db = DatabaseFactory.CreateDatabase();
          sqlCommand = "[dbo].[Delete_TMSB0044]";
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
          if (year != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@YEAR", year);
              dbCommand.Parameters.Add(empnoParm);
          }

          //if (FormName != string.Empty)
          //{
          //    SqlParameter empnoParm = new SqlParameter("@VDN_BY_PROG", FormName);
          //    dbCommand.Parameters.Add(empnoParm);
          //}

          ds = db.ExecuteDataSet(dbCommand);
          return ds;
      }

      [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
      public static DataSet GetTMSB0044(string agency, string dep, string program, string year)
      {
          DataSet ds;
          Database db;
          string sqlCommand;
          DbCommand dbCommand;

          db = DatabaseFactory.CreateDatabase();
          sqlCommand = "[dbo].[Browse_TMSB0044]";
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
          if (year != string.Empty)
          {
              SqlParameter empnoParm = new SqlParameter("@YEAR", year);
              dbCommand.Parameters.Add(empnoParm);
          }

          //if (FormName != string.Empty)
          //{
          //    SqlParameter empnoParm = new SqlParameter("@VDN_BY_PROG", FormName);
          //    dbCommand.Parameters.Add(empnoParm);
          //}

          ds = db.ExecuteDataSet(dbCommand);
          return ds;
      }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet UpdateTMSB0044(string agency, string dep, string program, string year, string Appno, string Addoperator, string clearincome, string clearinctypes, string casenotes, string custflds, string PreAss, string Alerts, string LiheapV,string Sites,string Services, string Supplier,string Inactive)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Update_TMSB0044]";
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
            if (year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@YEAR", year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Appno != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@APPNO", Appno);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Addoperator != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ADDOPERATOR", Addoperator);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (clearincome != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ClearIncome", clearincome);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (clearinctypes != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ClearIncTypes", clearinctypes);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (casenotes != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASENOTES", casenotes);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (custflds != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CUSTFLDS", custflds);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (PreAss != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PREASS", PreAss);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Alerts != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ALERTS", Alerts);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (LiheapV != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@LIHEAPV", LiheapV);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Sites != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SITES", Sites);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Services != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SERVICES", Services);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Supplier != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SUPPLIER", Supplier);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (Inactive != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Inactive", Inactive);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
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

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet LPWBUNDLE_GET(string agency, string dep, string program, string year, string strBundleno, string strType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[LPWBUNDLE_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@LPWB_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@LPWB_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@LPWB_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@LPWB_YEAR", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strBundleno != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@LPWB_BUNDLE", strBundleno);
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
        public static DataSet TMSB0030_EXISTBUNDLING(string agency, string dep, string program, string year, string BundleNo, string strApplicant, string strMode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[TMSB0030_EXISTBUNDLING]";
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
                SqlParameter empnoParm = new SqlParameter("@PROG", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@YEAR", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (BundleNo != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@LPWB_BUNDLE", BundleNo);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strApplicant != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@LWPAY_APP_NO", strApplicant);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strMode != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Mode", strMode);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CAPS_TMSB0032_GET(string agency, string dep, string program, string year, string strApplicant, string strstartdate, string strEnddate, string strMode, string strReportType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_TMSB0032_GET]";
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
                SqlParameter empnoParm = new SqlParameter("@PROG", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@YEAR", year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strApplicant != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Applicant", strApplicant);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strstartdate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@StartDate", strstartdate);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strEnddate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@EndDate", strEnddate);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strMode != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Mode", strMode);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strReportType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ReportType", strReportType);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CAPS_TMSB0033_GET(string agency, string dep, string program, string year, string strstartdate, string strEnddate, string strMode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_TMSB0033_REP]";
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
                SqlParameter empnoParm = new SqlParameter("@PROG", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@YEAR", year);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strstartdate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FromDate", strstartdate);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strEnddate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ToDate", strEnddate);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strMode != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Mode", strMode);
                dbCommand.Parameters.Add(empnoParm);
            }



            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CAPS_TMSB0034_GET(string agency, string dep, string program, string year, string Fund, string strBDCID, string Site, string Worker, string Fromdate, string Todate, string RefFromdate, string RefTodate, string RepType,string ProcessReport, string Mode)    // Brought update - Vikash 01032023 for Funding Source Report
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_TMSB0034_GET]";
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
            if (strBDCID.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BDCID", strBDCID);
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
            if (RefFromdate.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@RefFromDate", RefFromdate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (RefTodate.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@RefToDate", RefTodate);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (RepType.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@RepType", RepType);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (ProcessReport.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@PROCESSREPORT", ProcessReport);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Mode.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Mode", Mode);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CAPS_TMSB0035_GET(string agency, string dep, string program, string year, string strstartdate, string strEnddate, string strWorker, string strReportType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_TMSB0035_GET]";
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
                SqlParameter empnoParm = new SqlParameter("@PROG", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@YEAR", year);
                dbCommand.Parameters.Add(empnoParm);
            }


            if (strstartdate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@FromDate", strstartdate);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strEnddate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ToDate", strEnddate);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strWorker != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Worker", strWorker);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strReportType != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ReportType", strReportType);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool CAPS_LPWBUNDLE_INSERTUPDATEDEL(List<SqlParameter> sqlParamList)
        {

            DbCommand _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_LPWBUNDLE_INSERTUPDATEDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet CAPS_TMSB0046_GET(string agency, string dep, string program, string year, string BenAmt)  // Added from VWG form by Vikash on 04/05/2023 for EXHAUSTED BENEFIT REPORT
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[TMSB0046_Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

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
            if (BenAmt.Trim() != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@BENAmt", BenAmt);
                dbCommand.Parameters.Add(empnoParm);
            }


            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        #region Added by Vikash on 06/20/2024 as per CEAP Annual and Quarterly Report

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet TMSB0038_Report(string Agency, string Department, string Program, string Year, string ReportType, string FromDate, string ToDate, string Q1_FDate, string Q1_TDate, string Q2_FDate, string Q2_TDate, string Q3_FDate, string Q3_TDate, string Q4_FDate, string Q4_TDate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[TMSB0038_Report]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

            if (Agency != string.Empty)
            {
                SqlParameter Params = new SqlParameter("@AGENCY", Agency);
                dbCommand.Parameters.Add(Params);
            }
            if (Department != string.Empty)
            {
                SqlParameter Params = new SqlParameter("@DEPARTMENT", Department);
                dbCommand.Parameters.Add(Params);
            }
            if (Program != string.Empty)
            {
                SqlParameter Params = new SqlParameter("@PROGRAM", Program);
                dbCommand.Parameters.Add(Params);
            }
            if (Year.Trim() != string.Empty)
            {
                SqlParameter Params = new SqlParameter("@YEAR", Year);
                dbCommand.Parameters.Add(Params);
            }
            if (ReportType != string.Empty)
            {
                SqlParameter Params = new SqlParameter("@REP_TYPE", ReportType);
                dbCommand.Parameters.Add(Params);
            }
            if (FromDate.Trim() != string.Empty)
            {
                SqlParameter Params = new SqlParameter("@FROM_DATE", FromDate);
                dbCommand.Parameters.Add(Params);
            }
            if (ToDate != string.Empty)
            {
                SqlParameter Params = new SqlParameter("@TO_DATE", ToDate);
                dbCommand.Parameters.Add(Params);
            }
            if (Q1_FDate.Trim() != string.Empty)
            {
                SqlParameter Params = new SqlParameter("@Q1_FROM_DATE", Q1_FDate);
                dbCommand.Parameters.Add(Params);
            }
            if (Q1_TDate != string.Empty)
            {
                SqlParameter Params = new SqlParameter("@Q1_TO_DATE", Q1_TDate);
                dbCommand.Parameters.Add(Params);
            }
            if (Q2_FDate != string.Empty)
            {
                SqlParameter Params = new SqlParameter("@Q2_FROM_DATE", Q2_FDate);
                dbCommand.Parameters.Add(Params);
            }
            if (Q2_TDate != string.Empty)
            {
                SqlParameter Params = new SqlParameter("@Q2_TO_DATE", Q2_TDate);
                dbCommand.Parameters.Add(Params);
            }
            if (Q3_FDate.Trim() != string.Empty)
            {
                SqlParameter Params = new SqlParameter("@Q3_FROM_DATE", Q3_FDate);
                dbCommand.Parameters.Add(Params);
            }
            if (Q3_TDate != string.Empty)
            {
                SqlParameter Params = new SqlParameter("@Q3_TO_DATE", Q3_TDate);
                dbCommand.Parameters.Add(Params);
            }
            if (Q4_FDate.Trim() != string.Empty)
            {
                SqlParameter Params = new SqlParameter("@Q4_FROM_DATE", Q4_FDate);
                dbCommand.Parameters.Add(Params);
            }
            if (Q4_TDate != string.Empty)
            {
                SqlParameter Params = new SqlParameter("@Q4_TO_DATE", Q4_TDate);
                dbCommand.Parameters.Add(Params);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        #endregion
    }
}
