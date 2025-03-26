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
    public partial class AdhocDB
    {

        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[AGYTAB]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;
        #endregion

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Adhoc_Get_Tables_Columns(string DataBase_Name, string Operation)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Adhoc_Get_Tables_Columns]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (DataBase_Name != string.Empty)
            {
                SqlParameter sqlDB = new SqlParameter("@DataBase_Name", DataBase_Name.Trim());
                dbCommand.Parameters.Add(sqlDB);
            }
            SqlParameter sqlOperation = new SqlParameter("@Operation", Operation.Trim());
            dbCommand.Parameters.Add(sqlOperation);

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_PressGrp_Table()
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[Get_PressGrp_Table]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            //if (DataBase_Name != string.Empty)
            //{
            //    SqlParameter sqlDB = new SqlParameter("@DataBase_Name", DataBase_Name.Trim());
            //    dbCommand.Parameters.Add(sqlDB);
            //}
            //SqlParameter sqlOperation = new SqlParameter("@Operation", Operation.Trim());
            //dbCommand.Parameters.Add(sqlOperation);

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Get_SelCol_Data(string InputXML, string Secret_SW, string Group_Sort, string Include_Mambers, string Seledted_Hierarchy, string InputSummaryXML, string Addcust_Sw, string Card1_Xml, string Module_Code, string User_Id, string Data_Filter, string Category_Code)  //(string App_Key , string  InputXML)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;


            db = DatabaseFactory.CreateDatabase();
           // sqlCommand = "[dbo].[ParseXML]";
            //sqlCommand = "[dbo].[ParseXML_Test]";
            //sqlCommand = (Category_Code == "03" ? "[dbo].[ParseXML_Test_NewCat_CAMS]" : "[dbo].[ParseXML_Test]");
            sqlCommand = "[dbo].[ParseXML_Test_NewCat_CAMS]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

            if (InputXML != string.Empty)
            {
                SqlParameter sqlXML = new SqlParameter("@InputXML", InputXML);
                dbCommand.Parameters.Add(sqlXML);
            }
            SqlParameter sqlSecret = new SqlParameter("@In_Secret_Sw", Secret_SW);
            dbCommand.Parameters.Add(sqlSecret);

            SqlParameter sqlSort = new SqlParameter("@In_Group_Sort", Group_Sort);
            dbCommand.Parameters.Add(sqlSort);
            SqlParameter sqlMambers = new SqlParameter("@In_Include_Members", Include_Mambers);
            dbCommand.Parameters.Add(sqlMambers);

            SqlParameter sqlSelHierarchy = new SqlParameter("@In_Filter_Hierarchy", Seledted_Hierarchy);
            dbCommand.Parameters.Add(sqlSelHierarchy);

            SqlParameter sqlXML_Summary = new SqlParameter("@Input_Summary_XML", InputSummaryXML);
            dbCommand.Parameters.Add(sqlXML_Summary);

            SqlParameter sql_Addcust_Sw = new SqlParameter("@ADDCUST_Alone", Addcust_Sw);
            dbCommand.Parameters.Add(sql_Addcust_Sw);

            SqlParameter sql_Card1XML = new SqlParameter("@Card1XML", Card1_Xml);
            dbCommand.Parameters.Add(sql_Card1XML);

            SqlParameter sql_Module_Code = new SqlParameter("@Module", Module_Code);
            dbCommand.Parameters.Add(sql_Module_Code);

            SqlParameter sql_User_Id = new SqlParameter("@User", User_Id);
            dbCommand.Parameters.Add(sql_User_Id);

            if (!string.IsNullOrEmpty(Data_Filter.Trim()))
            {
                SqlParameter sql_Bypass_SW = new SqlParameter("@Bypass_SW", Data_Filter);
                dbCommand.Parameters.Add(sql_Bypass_SW);
            }

            if (!string.IsNullOrEmpty(Category_Code.Trim()))
            {
                SqlParameter sql_Bypass_SW = new SqlParameter("@Category_Code", Category_Code);
                dbCommand.Parameters.Add(sql_Bypass_SW);
            }
            

            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet Generate_CAMS_Work_File(string Agy, string Dept, string Prog, string Year, string Date_SW, string From_Date, string To_Date, string User_Name, string session_ID, string IP)  //(string App_Key , string  InputXML)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;



            db = DatabaseFactory.CreateDatabase();
            // sqlCommand = "[dbo].[ParseXML]";
            sqlCommand = "[dbo].[Adhoc_Populate_ACTMS_Physical_ResultSet]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 1800;

            SqlParameter sqlXML = new SqlParameter("@Agy", Agy);
            dbCommand.Parameters.Add(sqlXML);

            SqlParameter sqlSecret = new SqlParameter("@Dept", Dept);
            dbCommand.Parameters.Add(sqlSecret);

            SqlParameter sqlSort = new SqlParameter("@Prog", Prog);
            dbCommand.Parameters.Add(sqlSort);
            SqlParameter sqlMambers = new SqlParameter("@Year", Year);
            dbCommand.Parameters.Add(sqlMambers);


            SqlParameter sqlDateSW = new SqlParameter("@Date_Filter_SW", Date_SW);
            dbCommand.Parameters.Add(sqlDateSW);
            if (!string.IsNullOrEmpty(From_Date.Trim()))
            {
                SqlParameter sqlFromDate = new SqlParameter("@ACTMS_From_Date", From_Date);
                dbCommand.Parameters.Add(sqlFromDate);
            }
            if (!string.IsNullOrEmpty(To_Date.Trim()))
            {
                SqlParameter sqlToDate = new SqlParameter("@ACTMS_To_Date", To_Date);
                dbCommand.Parameters.Add(sqlToDate);
            }

            SqlParameter sqlUser = new SqlParameter("@User_Name", User_Name);
            dbCommand.Parameters.Add(sqlUser);

            SqlParameter sqlID = new SqlParameter("@Session_ID", session_ID);
            dbCommand.Parameters.Add(sqlID);
            SqlParameter sqlIP = new SqlParameter("@IP", IP);
            dbCommand.Parameters.Add(sqlIP);

            
            // Get results.
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

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

            foreach (SqlParameter sqlPar in sqlParamList)
                dbCommand.Parameters.Add(sqlPar);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        public static bool Update_Sel_Table(List<SqlParameter> sqlParamList, string SP_Name)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand(SP_Name);
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }





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

    }
}
