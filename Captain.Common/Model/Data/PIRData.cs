using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.DatabaseLayer;
using Captain.Common.Model.Objects;
using System.Data.SqlClient;
using System.Data;


namespace Captain.Common.Model.Data
{
    public class PIRData
    {

        public PIRData()
        {

        }

        #region Properties

        public CaptainModel Model { get; set; }

        #endregion

        public List<PIRQUESTEntity> Browse_PIRQUEST(PIRQUESTEntity Entity, string Count_Join_SW)
        {
            List<PIRQUESTEntity> PIRQUESTProfile = new List<PIRQUESTEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_PIRQUEST_SqlParameters_List(Entity, Count_Join_SW);
                DataSet PIRQUESTData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_PIRQUEST]");

                if (PIRQUESTData != null && PIRQUESTData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in PIRQUESTData.Tables[0].Rows)
                        PIRQUESTProfile.Add(new PIRQUESTEntity(row, Count_Join_SW));
                }
            }
            catch (Exception ex)
            { return PIRQUESTProfile; }

            return PIRQUESTProfile;
        }


        public List<SqlParameter> Prepare_PIRQUEST_SqlParameters_List(PIRQUESTEntity Entity, string Count_Join_SW)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@Join_Counts", Count_Join_SW));
                sqlParamList.Add(new SqlParameter("@Agy", Entity.Cnt_Agency));
                sqlParamList.Add(new SqlParameter("@Dept", Entity.Cnt_Dept));
                sqlParamList.Add(new SqlParameter("@Prog", Entity.Cnt_Prog));
                sqlParamList.Add(new SqlParameter("@Year", Entity.Cnt_Year));
                sqlParamList.Add(new SqlParameter("@Site", Entity.Cnt_Site));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_UNIQUE_ID", Entity.Ques_Unique_ID));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_POS", Entity.Ques_Position));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_ACTIVE", Entity.Active_Status));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_SECTION", Entity.Ques_section));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_FUND", Entity.Fund_Type));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_TYPE", Entity.Ques_Type));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_CODE", Entity.Ques_Code));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_SQUE_CODE", Entity.Ques_SCode));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_SECTION", Entity.Ques_Seq));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_DESC", Entity.Ques_Desc));
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public bool UpdatePIRCOUNT(PIRCOUNTEntity Entity, string Operation_Mode, string Questions_Xml, out string Sql_Reslut_Msg) 
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_PIRCOUNT_SqlParameters_List(Entity, Operation_Mode);
                sqlParamList.Add(new SqlParameter("@Ques_Xml", Questions_Xml));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdatePIRCOUNT", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<SqlParameter> Prepare_PIRCOUNT_SqlParameters_List(PIRCOUNTEntity Entity, string Operation)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {

                if (Operation != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Operation.Substring(0, 1)));

                sqlParamList.Add(new SqlParameter("@Agency", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@Dept", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@Prog", Entity.Prog));
                sqlParamList.Add(new SqlParameter("@Year", Entity.Year));
                sqlParamList.Add(new SqlParameter("@Site", Entity.Site));
                sqlParamList.Add(new SqlParameter("@Fund", Entity.Ques_Fund));
                sqlParamList.Add(new SqlParameter("@lstc_operator", Entity.Lstc_Operator));
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }


        public bool InsertUpdatePirQues(PIRQUESTEntity pirQuestentity)
        {
            bool boolsuccess;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@PIRQUEST_YEAR", pirQuestentity.Ques_Year));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_UNIQUE_ID", pirQuestentity.Ques_Unique_ID));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_ACTIVE", pirQuestentity.Active_Status));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_POS", pirQuestentity.Ques_Position));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_SECTION", pirQuestentity.Ques_section));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_FUND_TYPE", pirQuestentity.Fund_Type));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_TYPE", pirQuestentity.Ques_Type));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_CODE", pirQuestentity.Ques_Code));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_SCODE", pirQuestentity.Ques_SCode));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_SECTION", pirQuestentity.Ques_Seq));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_DESC", pirQuestentity.Ques_Desc));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_A1DAY", pirQuestentity.Ques_A1Day));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_BOLD", pirQuestentity.Ques_Bold));
                sqlParamList.Add(new SqlParameter("@Mode", pirQuestentity.Mode));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsertUpdatePirQuest(sqlParamList);
            }
            catch (Exception ex)
            {
                return false;
            }

            return boolsuccess;
        }


        public List<PIRQUESTEntity> GetPIRQUEST(string strYear)
        {
            List<PIRQUESTEntity> PIRQUESTProfile = new List<PIRQUESTEntity>();
           
            try
            {

                DataSet PIRQUESTData = Captain.DatabaseLayer.Lookups.GetPirQuestRecord(strYear);

                if (PIRQUESTData != null && PIRQUESTData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in PIRQUESTData.Tables[0].Rows)
                        PIRQUESTProfile.Add(new PIRQUESTEntity(row));
                }
            }
            catch (Exception ex)
            { return PIRQUESTProfile; }

            return PIRQUESTProfile;
        }

        #region Vikash added on 04/05/2024
        public bool InsertUpdatePirQues2(PIRQUEST2Entity pirQues2tentity)
        {
            bool boolsuccess;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@PIRQUEST_YEAR", pirQues2tentity.PIR_Ques_Year));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_UNIQUE_ID", pirQues2tentity.PIR_Ques_Unique_ID));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_SECTION", pirQues2tentity.PIR_Section));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_FUND", pirQues2tentity.PIR_Fund_Type));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_POS", pirQues2tentity.PIR_Ques_Position));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_CODE", pirQues2tentity.PIR_Ques_Code));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_SQUE_CODE", pirQues2tentity.PIR_Ques_SCode));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_SECTION", pirQues2tentity.PIR_Ques_Section));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_TYPE", pirQues2tentity.PIR_Ques_Type));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_DESC", pirQues2tentity.PIR_Ques_Desc));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_COLHEAD1", pirQues2tentity.PIR_Ques_Col1));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_COLHEAD2", pirQues2tentity.PIR_Ques_Col2));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_COLHEAD_TOP", pirQues2tentity.PIR_Ques_Col_Head_Top));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_ACTIVE", pirQues2tentity.PIR_Active));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_A1DAY", pirQues2tentity.PIR_Ques_A1Day));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_BOLD", pirQues2tentity.PIR_Ques_Bold));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QID", pirQues2tentity.PIR_Ques_ID));
                sqlParamList.Add(new SqlParameter("@Mode", pirQues2tentity.Mode));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsertUpdatePirQuest2(sqlParamList);
            }
            catch (Exception ex)
            {
                return false;
            }

            return boolsuccess;
        }

        // Added on 05/06/2024 to save PIR Counts

        public List<SqlParameter> Prepare_PIRCOUNT2_SqlParameters_List(PIRCOUNT2Entity Entity)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@AGENCY", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@PROG", Entity.Prog));
                sqlParamList.Add(new SqlParameter("@YEAR", Entity.Year));
                sqlParamList.Add(new SqlParameter("@SITE", Entity.Site));
                sqlParamList.Add(new SqlParameter("@FUND", Entity.Ques_Fund));
                sqlParamList.Add(new SqlParameter("@Q_ID", Entity.Ques_ID));
                if(Entity.Column1 != "")
                    sqlParamList.Add(new SqlParameter("@COL1_COUNT", Entity.Column1));
                if (Entity.Column2 != "")
                    sqlParamList.Add(new SqlParameter("@COL2_COUNT", Entity.Column2));
                sqlParamList.Add(new SqlParameter("@AGYFLAG", Entity.Agy_Flag));
                sqlParamList.Add(new SqlParameter("@AGYCODE", Entity.Agy_Code));
                sqlParamList.Add(new SqlParameter("@ADD_OPERATOR", Entity.Add_Operator));
                sqlParamList.Add(new SqlParameter("@LSTC_OPERATOR", Entity.Lstc_Operator));
            }
            catch (Exception ex)
            {
                return sqlParamList;
            }

            return sqlParamList;
        }

        public bool UpdatePIRCOUNT2(PIRCOUNT2Entity Entity)
        {
            bool boolsuccess;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_PIRCOUNT2_SqlParameters_List(Entity);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.UpdatePIRCOUNT2(sqlParamList);
            }
            catch (Exception ex)
            {
                return false;
            }

            return boolsuccess;
        }

        #endregion
    }
}
