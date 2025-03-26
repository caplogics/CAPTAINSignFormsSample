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
    public partial class CaseSum
    {
        #region Constants
        //private static readonly string TABLE_NAME = "[dbo].[CASESUM]";
        private static Database _dbFactory = DatabaseFactory.CreateDatabase();
        private static DbCommand _dbCommand;

        #endregion


        public static bool InsertUpdateDelCaseSum(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelCaseSum");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }      
      
        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCaseSumDetails(string agency, string dep, string program, string year, string app, string seq)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCaseSumDetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASESUM_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASESUM_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASESUM_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASESUM_YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (app != string.Empty || app != null)
            {
                SqlParameter empnoParm = new SqlParameter("@CASESUM_APP_NO ", app);
                dbCommand.Parameters.Add(empnoParm);
            }           
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCaseSumHieAllDetails(string agency, string dep, string program, string year, string app)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCaseSumHieAllDetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASESUM_AGENCY", agency);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (dep != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASESUM_DEPT", dep);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (program != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASESUM_PROGRAM", program);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (year != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@CASESUM_YEAR ", year);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (app != string.Empty || app != null)
            {
                SqlParameter empnoParm = new SqlParameter("@CASESUM_APP_NO ", app);
                dbCommand.Parameters.Add(empnoParm);
            }
            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }


         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetCaseSumSubDetails(string ssnno, string key, string refkey,string strApplicatino,string FName,string DOB,string Clid)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetCaseSumSubDetails]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (ssnno != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SSNNO", ssnno);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (key != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@Key", key);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (refkey != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@RefKey", refkey);
                dbCommand.Parameters.Add(empnoParm);
            }

            if (strApplicatino != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@ApplicationNo", strApplicatino);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (FName != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SNP_FANME", FName);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (DOB != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SNP_DOB", DOB);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (Clid != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@SNP_CLID", Clid);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

         public static bool CheckCaseSumProgramDetails(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.Get_Eligibility");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }
        

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetCaseSumEligData(string strKey, string strCaseSum)
         {
                      

             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[Get_EligibilityTable]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (strKey != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@In_Mst_Appl_Ix", strKey);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (strCaseSum != string.Empty)
             {
                 SqlParameter sqlDept = new SqlParameter("@CaseSum", strCaseSum);
                 dbCommand.Parameters.Add(sqlDept);
             }
             ds = db.ExecuteDataSet(dbCommand);
             return ds;

         }



         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetCaseEnrlDetails(string agency, string dep, string program, string year, string app, string Group,string strFundHie)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetCaseENRLDetails]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (agency != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@CASEENRL_AGENCY", agency);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (dep != string.Empty)
             {
                 SqlParameter sqlDept = new SqlParameter("@CASEENRL_DEPT", dep);
                 dbCommand.Parameters.Add(sqlDept);
             }
             if (program != string.Empty)
             {
                 SqlParameter sqlProgram = new SqlParameter("@CASEENRL_PROG", program);
                 dbCommand.Parameters.Add(sqlProgram);
             }
             if (year != string.Empty)
             {
                 SqlParameter sqlYear = new SqlParameter("@CASEENRL_YEAR ", year);
                 dbCommand.Parameters.Add(sqlYear);
             }
             if (app != string.Empty || app != null)
             {
                 SqlParameter sqlApp = new SqlParameter("@CASEENRL_APP ", app);
                 dbCommand.Parameters.Add(sqlApp);
             }
             if (Group != string.Empty)
             {
                 SqlParameter sqlGroup = new SqlParameter("@CASEENRL_GROUP", Group);
                 dbCommand.Parameters.Add(sqlGroup);
             }
             if (strFundHie != string.Empty)
             {
                 SqlParameter sqlRefAgency = new SqlParameter("@CASEENRL_FUND_HIE", strFundHie);
                 dbCommand.Parameters.Add(sqlRefAgency);
             }           
             
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }



         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetCaseENRLHss2001(string agency, string dep, string program, string year)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetCaseENRLHss2001]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (agency != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@CASEENRL_AGENCY", agency);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (dep != string.Empty)
             {
                 SqlParameter sqlDept = new SqlParameter("@CASEENRL_DEPT", dep);
                 dbCommand.Parameters.Add(sqlDept);
             }
             if (program != string.Empty)
             {
                 SqlParameter sqlProgram = new SqlParameter("@CASEENRL_PROG", program);
                 dbCommand.Parameters.Add(sqlProgram);
             }
             if (year != string.Empty)
             {
                 SqlParameter sqlYear = new SqlParameter("@CASEENRL_YEAR ", year);
                 dbCommand.Parameters.Add(sqlYear);
             }
           
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }



         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetELIGQUES()
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetELIGQUES]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);
          
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetAGYTABS(string Agy_Type)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[Browse_AGYTABS]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             SqlParameter sqlAgy_type = new SqlParameter("@AGYS_TYPE", Agy_Type);
             dbCommand.Parameters.Add(sqlAgy_type);

             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet Browse_AddCust(string Agency,string Dept,string Prog,string Year,string AppNo)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[Browse_ADDCUST]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             SqlParameter sqlAgency = new SqlParameter("@ACT_AGENCY", Agency);
             dbCommand.Parameters.Add(sqlAgency);

             SqlParameter sqlDept = new SqlParameter("@ACT_DEPT", Dept);
             dbCommand.Parameters.Add(sqlDept);

             SqlParameter sqlProg = new SqlParameter("@ACT_PROGRAM", Prog);
             dbCommand.Parameters.Add(sqlProg);

             SqlParameter sqlYear = new SqlParameter("@ACT_YEAR", Year);
             dbCommand.Parameters.Add(sqlYear);

             SqlParameter sqlAppNo = new SqlParameter("@ACT_APP_NO", AppNo);
             dbCommand.Parameters.Add(sqlAppNo);
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetELIGCUSTOMQUES(string Hierachy)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetFLDCNTLHIE_Ques]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (Hierachy != string.Empty)
             {
                 SqlParameter sqlYear = new SqlParameter("@Hierachy", Hierachy);
                 dbCommand.Parameters.Add(sqlYear);
             }

             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetELIGCUSTOMQUES(string Hierachy,string strType)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[GetFLDCNTLHIE_Ques]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (Hierachy != string.Empty)
            {
                SqlParameter sqlYear = new SqlParameter("@Hierachy", Hierachy);
                dbCommand.Parameters.Add(sqlYear);
            }

            if (strType != string.Empty)
            {
                SqlParameter sqlYear = new SqlParameter("@Type", strType);
                dbCommand.Parameters.Add(sqlYear);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }



        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GETCASELIGQDETAILS(string groupcode, string quescode, string seq)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GETCASELIGQDETAILS]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (groupcode != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@CASELIGQ_GROUP_CODE", groupcode);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (quescode != string.Empty)
             {
                 SqlParameter sqlDept = new SqlParameter("@CASELIGQ_QUES_CODE", quescode);
                 dbCommand.Parameters.Add(sqlDept);
             }
             if (seq != string.Empty)
             {
                 SqlParameter sqlProgram = new SqlParameter("@CASELIGQ_QUES_SEQ", seq);
                 dbCommand.Parameters.Add(sqlProgram);
             }             
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GETCASELIGHDETAILS(string groupcode)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GETCASELIGHDETAILS]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (groupcode != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@CASELIGH_CODE", groupcode);
                 dbCommand.Parameters.Add(sqlAgency);
             }
            
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GETCASEELIGGDETAILS(string groupcode,string groupDesc,string Hierachy)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GETCASEELIGGDETAILS]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (groupcode != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@CASELIGG_CODE", groupcode);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (groupDesc != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@CASELIGG_CODE", groupDesc);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (Hierachy != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@CASELIGG_CODE", Hierachy);
                 dbCommand.Parameters.Add(sqlAgency);
             }

             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetCaseEligHierachys(string UserID,string AdminAgency)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetCaseEligHierachys]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (UserID != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@UserID", UserID);
                dbCommand.Parameters.Add(sqlAgency);
            }

            if (AdminAgency != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@Admin_Agency", AdminAgency);
                dbCommand.Parameters.Add(sqlAgency);
            }


            ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetSSBGHierachys()
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetSSBGHierachys]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);


             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }
        
         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static string GETCASEELIGGCode(string strHierachy)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetCaseELIGCode]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);
             if (strHierachy != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@CASELIG_Hierachy", strHierachy);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             ds = db.ExecuteDataSet(dbCommand);
             return ds.Tables[0].Rows[0][0].ToString();
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static string GETSSBGCode(string strHierachy)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetSSBGCode]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);
             if (strHierachy != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@SSBG_Hierachy", strHierachy);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             ds = db.ExecuteDataSet(dbCommand);
             return ds.Tables[0].Rows[0][0].ToString();
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static string GETSSBGGroupCode(string strID,string strCode)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetSSBGGroupCode]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);
             if (strID != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@SBGT_ID", strID);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (strCode != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@SBGT_CODE", strCode);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             ds = db.ExecuteDataSet(dbCommand);
             return ds.Tables[0].Rows[0][0].ToString();
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetCASEELIGadpgs(string agency, string dept, string program,string groupcode,string seq,string type)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetCASEELIGadpgs]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (agency != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@CASEELIG_AGENCY", agency);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (dept != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@CASEELIG_DEPT", dept);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (program != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@CASEELIG_PROGRAM", program);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (groupcode != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@CASEELIG_GROUP_CODE", groupcode);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (seq != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@CASEELIG_GROUP_SEQ", seq );
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (type != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@Type", type);
                 dbCommand.Parameters.Add(sqlAgency);
             }

             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetSREVSTOP(string agency, string dept, string program, string Fdate, string Tdate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_SERVSTOP_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@SRST_AGENCY", agency);
                dbCommand.Parameters.Add(sqlAgency);
            }
            if (dept != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@SRST_DEPT", dept);
                dbCommand.Parameters.Add(sqlAgency);
            }
            if (program != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@SRST_PROG", program);
                dbCommand.Parameters.Add(sqlAgency);
            }
            if (Fdate != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@SRST_FDATE", Fdate);
                dbCommand.Parameters.Add(sqlAgency);
            }
            if (Tdate != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@SRST_TDATE", Tdate);
                dbCommand.Parameters.Add(sqlAgency);
            }
            //if (type != string.Empty)
            //{
            //    SqlParameter sqlAgency = new SqlParameter("@Type", type);
            //    dbCommand.Parameters.Add(sqlAgency);
            //}

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
        public static DataSet GetSREVSTOPHIST(string agency, string dept, string program, string Fdate, string Tdate)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_SERVSTOPHIST_BROWSE]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (agency != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@SRSTH_AGENCY", agency);
                dbCommand.Parameters.Add(sqlAgency);
            }
            if (dept != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@SRSTH_DEPT", dept);
                dbCommand.Parameters.Add(sqlAgency);
            }
            if (program != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@SRSTH_PROG", program);
                dbCommand.Parameters.Add(sqlAgency);
            }
            if (Fdate != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@SRSTH_FDATE", Fdate);
                dbCommand.Parameters.Add(sqlAgency);
            }
            if (Tdate != string.Empty)
            {
                SqlParameter sqlAgency = new SqlParameter("@SRSTH_TDATE", Tdate);
                dbCommand.Parameters.Add(sqlAgency);
            }
            //if (type != string.Empty)
            //{
            //    SqlParameter sqlAgency = new SqlParameter("@Type", type);
            //    dbCommand.Parameters.Add(sqlAgency);
            //}

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetSSBGTypesadpgs(string Id, string TypeCode, string seq,string Group,string GrpSeq, string type)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetSSBGTYPEadpgs]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (Id != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@SBGT_ID", Id);
                 dbCommand.Parameters.Add(sqlAgency);
             }

             if (TypeCode != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@SBGT_CODE", TypeCode);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (seq != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@SBGT_CODE_SEQ", seq);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (Group != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@SBGT_GROUP", Group);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (GrpSeq != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@SBGT_GROUP_SEQ", GrpSeq);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (type != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@Type", type);
                 dbCommand.Parameters.Add(sqlAgency);
             }

             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetSSBGadpgs(string agency, string dept, string program, string Year, string seq)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[Browse_SSBGPARAMS]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (agency != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@SBGP_AGENCY", agency);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (dept != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@SBGP_DEPT", dept);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (program != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@SBGP_PROG", program);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (Year != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@SBGP_YEAR", Year);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (seq != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@SBGP_SEQ", seq);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             //if (type != string.Empty)
             //{
             //    SqlParameter sqlAgency = new SqlParameter("@Type", type);
             //    dbCommand.Parameters.Add(sqlAgency);
             //}

             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet GetCustRespCode(string Type, string code)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[GetCustRespByRespCode]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);
             SqlParameter filterParm = new SqlParameter("@ScrCustCode", Type);
             dbCommand.Parameters.Add(filterParm);
             SqlParameter filtercode = new SqlParameter("@Rsp_Resp_Code", code);
             dbCommand.Parameters.Add(filtercode);
             // Get results.
             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }


        
         public static bool InsertUpdateDelCASEELIG(List<SqlParameter> sqlParamList)
         {
             _dbCommand = _dbFactory.GetStoredProcCommand("dbo.InsertUpdateDelCASEELIG");
             _dbCommand.CommandTimeout = 1200;
             _dbCommand.Parameters.Clear();
             foreach (SqlParameter sqlPar in sqlParamList)
             {
                 _dbCommand.Parameters.Add(sqlPar);
             }
             return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
         }

        public static bool InsertUpdateDelSERVSTOP(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_SERVSTOP_INSUPDEL");
            _dbCommand.CommandTimeout = 1200;
            _dbCommand.Parameters.Clear();
            foreach (SqlParameter sqlPar in sqlParamList)
            {
                _dbCommand.Parameters.Add(sqlPar);
            }
            return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
        }

        public static bool InsertUpdateDelSSBGPARAMS(List<SqlParameter> sqlParamList)
         {
             _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UPDATESSBGPARAMS");
             _dbCommand.CommandTimeout = 1200;
             _dbCommand.Parameters.Clear();
             foreach (SqlParameter sqlPar in sqlParamList)
             {
                 _dbCommand.Parameters.Add(sqlPar);
             }
             return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
         }

         public static bool InsertUpdateDelSSBGMonths(List<SqlParameter> sqlParamList)
         {
             _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UPDATESSBGMONTHS");
             _dbCommand.CommandTimeout = 1200;
             _dbCommand.Parameters.Clear();
             foreach (SqlParameter sqlPar in sqlParamList)
             {
                 _dbCommand.Parameters.Add(sqlPar);
             }
             return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
         }

         public static bool InsertUpdateDelSSBGTYPES(List<SqlParameter> sqlParamList)
         {
             _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UPDATESSBGTYPES");
             _dbCommand.CommandTimeout = 1200;
             _dbCommand.Parameters.Clear();
             foreach (SqlParameter sqlPar in sqlParamList)
             {
                 _dbCommand.Parameters.Add(sqlPar);
             }
             return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
         }

         public static bool InsertUpdateDelSSBGGoals(List<SqlParameter> sqlParamList)
         {
             _dbCommand = _dbFactory.GetStoredProcCommand("dbo.UPDATESSBGGOALS");
             _dbCommand.CommandTimeout = 1200;
             _dbCommand.Parameters.Clear();
             foreach (SqlParameter sqlPar in sqlParamList)
             {
                 _dbCommand.Parameters.Add(sqlPar);
             }
             return _dbFactory.ExecuteNonQuery(_dbCommand) > 0 ? true : false;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet Browse_CASEELIG(string Agency,string Dept,string Prog)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[Browse_CASEELIG]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (Agency != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@CASEELIG_AGENCY", Agency);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (Dept != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@CASEELIG_DEPT", Dept);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (Prog != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@CASEELIG_PROGRAM", Prog);
                 dbCommand.Parameters.Add(sqlAgency);
             }

             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet Browse_SSBGPARAMS(string Agency, string Dept, string Prog)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[Browse_SSBGPARAMS]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (Agency != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@SBGP_AGENCY", Agency);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (Dept != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@SBGP_DEPT", Dept);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (Prog != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@SBGP_PROG", Prog);
                 dbCommand.Parameters.Add(sqlAgency);
             }

             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet Browse_SSBGMonths(string Agency, string Dept, string Prog)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[Browse_SSBGMONTHS]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (Agency != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@SBGM_AGENCY", Agency);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (Dept != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@SBGM_DEPT", Dept);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (Prog != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@SBGM_PROG", Prog);
                 dbCommand.Parameters.Add(sqlAgency);
             }

             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet Browse_SSBGSPMS(string Agency, string Dept, string Prog)
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[SSBG_DISTINCT_SPMS]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             if (Agency != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@AGENCY", Agency);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (Dept != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@DEPT", Dept);
                 dbCommand.Parameters.Add(sqlAgency);
             }
             if (Prog != string.Empty)
             {
                 SqlParameter sqlAgency = new SqlParameter("@PROGRAM", Prog);
                 dbCommand.Parameters.Add(sqlAgency);
             }

             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet Browse_SSBGTYPES()
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[BROWSE_SSBGTYPES]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             //if (Agency != string.Empty)
             //{
             //    SqlParameter sqlAgency = new SqlParameter("@SBGP_AGENCY", Agency);
             //    dbCommand.Parameters.Add(sqlAgency);
             //}
             //if (Dept != string.Empty)
             //{
             //    SqlParameter sqlAgency = new SqlParameter("@SBGP_DEPT", Dept);
             //    dbCommand.Parameters.Add(sqlAgency);
             //}
             //if (Prog != string.Empty)
             //{
             //    SqlParameter sqlAgency = new SqlParameter("@SBGP_PROG", Prog);
             //    dbCommand.Parameters.Add(sqlAgency);
             //}

             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet Browse_SSBGGoals()
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[Browse_SSBGGOALS]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             //if (Agency != string.Empty)
             //{
             //    SqlParameter sqlAgency = new SqlParameter("@SBGP_AGENCY", Agency);
             //    dbCommand.Parameters.Add(sqlAgency);
             //}
             //if (Dept != string.Empty)
             //{
             //    SqlParameter sqlAgency = new SqlParameter("@SBGP_DEPT", Dept);
             //    dbCommand.Parameters.Add(sqlAgency);
             //}
             //if (Prog != string.Empty)
             //{
             //    SqlParameter sqlAgency = new SqlParameter("@SBGP_PROG", Prog);
             //    dbCommand.Parameters.Add(sqlAgency);
             //}

             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

         [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
         public static DataSet Browse_ELIGQUES()
         {
             DataSet ds;
             Database db;
             string sqlCommand;
             DbCommand dbCommand;

             db = DatabaseFactory.CreateDatabase();
             sqlCommand = "[dbo].[Browse_ELIGQUES]";
             dbCommand = db.GetStoredProcCommand(sqlCommand);

             //if (Code != string.Empty)
             //{
             //    SqlParameter sqlAgency = new SqlParameter("@ELIGQUES_CODE", Code);
             //    dbCommand.Parameters.Add(sqlAgency);
             //}
             //if (groupDesc != string.Empty)
             //{
             //    SqlParameter sqlAgency = new SqlParameter("@CASELIGG_CODE", groupDesc);
             //    dbCommand.Parameters.Add(sqlAgency);
             //}
             //if (Hierachy != string.Empty)
             //{
             //    SqlParameter sqlAgency = new SqlParameter("@CASELIGG_CODE", Hierachy);
             //    dbCommand.Parameters.Add(sqlAgency);
             //}

             ds = db.ExecuteDataSet(dbCommand);
             return ds;
         }

        public static DataSet CAPS_HOLIDAYS_GET(string strYear, string strDate, string strMode)
        {
            DataSet ds;
            Database db;
            string sqlCommand;
            DbCommand dbCommand;

            db = DatabaseFactory.CreateDatabase();
            sqlCommand = "[dbo].[CAPS_HOLIDAYS_GET]";
            dbCommand = db.GetStoredProcCommand(sqlCommand);

            if (strYear != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HLD_YEAR", strYear);
                dbCommand.Parameters.Add(empnoParm);
            }
            if (strDate != string.Empty)
            {
                SqlParameter empnoParm = new SqlParameter("@HLD_DATE", strDate);
                dbCommand.Parameters.Add(empnoParm);
            }

            ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

        public static bool CAPS_HOLIDAYS_INSUPDEL(List<SqlParameter> sqlParamList)
        {
            _dbCommand = _dbFactory.GetStoredProcCommand("dbo.CAPS_HOLIDAYS_INSUPDEL");
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
