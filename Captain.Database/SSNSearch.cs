using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.ComponentModel;
using System.Data;

namespace Captain.DatabaseLayer
{
    [DataObject]
    [Serializable]
    public partial class SSN_Search
    {

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet MainMenuSearch(string SearchCat,string SearchFor,string Ssn,string Fname, string Lname, string Alias,string Phone,
                                             string HNo, string Street, string City, string State)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[SSNSearch]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //if (SearchCat != string.Empty)
            //{
            //    SqlParameter sqlSearchCat = new SqlParameter("@S_Type", SearchCat);
            //    dbCommand.Parameters.Add(sqlSearchCat);
            //}
            if (SearchFor != string.Empty)
            {
                SqlParameter sqlSearchFor = new SqlParameter("@For", SearchFor);
                dbCommand.Parameters.Add(sqlSearchFor);
            }
            string tmp = " ";
            SqlParameter sqltmp = new SqlParameter("@App_No", tmp);
            dbCommand.Parameters.Add(sqltmp);
            //SqlParameter sqlLname = new SqlParameter("@First_Name", Fname);
            //dbCommand.Parameters.Add(sqlLname);
            //SqlParameter sqlFname = new SqlParameter("@Last_Name ", Lname);
            //dbCommand.Parameters.Add(sqlFname);
            SqlParameter sqlSsn = new SqlParameter("@Ssn", Ssn);
            dbCommand.Parameters.Add(sqlSsn);
            //SqlParameter sqlHNo = new SqlParameter("@Hno", HNo);
            //dbCommand.Parameters.Add(sqlHNo);
            //SqlParameter sqlStreet = new SqlParameter("@Street", Street);
            //dbCommand.Parameters.Add(sqlStreet);
            //SqlParameter sqlCity = new SqlParameter("@City", City);
            //dbCommand.Parameters.Add(sqlCity);
            //SqlParameter sqlState = new SqlParameter("@State", State);
            //dbCommand.Parameters.Add(sqlState);
            //SqlParameter sqlPhone = new SqlParameter("@Phone", Phone);
            //dbCommand.Parameters.Add(sqlPhone);
            //SqlParameter sqlAlias = new SqlParameter("@Alias", Alias);
            //dbCommand.Parameters.Add(sqlAlias);
                      
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet SsnScan(string Hirarchy,string MstSsn)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetSsnScan]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);


            SqlParameter sqlHirarchy = new SqlParameter("@MST_APPL_HIE", Hirarchy);
            dbCommand.Parameters.Add(sqlHirarchy);
            SqlParameter sqlMstSsn = new SqlParameter("@MST_SSN ", MstSsn);
            dbCommand.Parameters.Add(sqlMstSsn);
            
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        
    }
}
