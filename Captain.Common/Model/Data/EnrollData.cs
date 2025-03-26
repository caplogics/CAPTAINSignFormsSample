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
    public class EnrollData
    {

        public EnrollData()
        {

        }

        #region Properties

        public CaptainModel Model { get; set; }

        #endregion


        public List<CaseEnrlEntity> Browse_CASEENRL(CaseEnrlEntity Entity, string Opretaion_Mode)
        {
            List<CaseEnrlEntity> CASEENRLProfile = new List<CaseEnrlEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASEENRL_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASEENRLData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CASEENRL]");

                if (CASEENRLData != null && CASEENRLData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASEENRLData.Tables[0].Rows)
                        CASEENRLProfile.Add(new CaseEnrlEntity(row, Entity.Join_Mst_Snp));
                }
            }
            catch (Exception ex)
            { return CASEENRLProfile; }

            return CASEENRLProfile;
        }

        public List<ENRL_Asof_Entity> Browse_CASEENRL_Asof_Date(string Agency, string Dept, string Prog, string Year, string App, string Fund_Hie, string Site, string Room, string AmPm, string Asof_Date)
        {
            List<ENRL_Asof_Entity> CASEENRLProfile = new List<ENRL_Asof_Entity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@Tmp_Asof_Date", Asof_Date));

                sqlParamList.Add(new SqlParameter("@Agency", Agency));
                sqlParamList.Add(new SqlParameter("@Dept", Dept));
                sqlParamList.Add(new SqlParameter("@Prog", Prog));
                sqlParamList.Add(new SqlParameter("@Year", Year));
                if (!string.IsNullOrEmpty(App.Trim()))
                    sqlParamList.Add(new SqlParameter("@App", App));

                if (!string.IsNullOrEmpty(Fund_Hie.Trim()))
                    sqlParamList.Add(new SqlParameter("@Fund_Hie", Fund_Hie));
                if (!string.IsNullOrEmpty(Site.Trim()))
                    sqlParamList.Add(new SqlParameter("@Site", Site));
                if (!string.IsNullOrEmpty(Room.Trim()))
                {
                    if(Room.Trim() != "****")
                        sqlParamList.Add(new SqlParameter("@Room", Room));
                }
                if (!string.IsNullOrEmpty(AmPm.Trim()))
                    sqlParamList.Add(new SqlParameter("@AmPm", AmPm));


                DataSet CASEENRLData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CASEENRL_Asof_Date]");

                if (CASEENRLData != null && CASEENRLData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASEENRLData.Tables[0].Rows)
                        CASEENRLProfile.Add(new ENRL_Asof_Entity(row));
                }
            }
            catch (Exception ex)
            { return CASEENRLProfile; }

            return CASEENRLProfile;
        }

        public List<ENRL_Asof_Entity> Browse_CASEENRL_Asof_Date_Attendance(string Agency, string Dept, string Prog, string Year, string App, string Fund_Hie, string Site, string Room, string AmPm, string Asof_Date)
        {
            List<ENRL_Asof_Entity> CASEENRLProfile = new List<ENRL_Asof_Entity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@Tmp_Asof_Date", Asof_Date));

                sqlParamList.Add(new SqlParameter("@Agency", Agency));
                sqlParamList.Add(new SqlParameter("@Dept", Dept));
                sqlParamList.Add(new SqlParameter("@Prog", Prog));
                sqlParamList.Add(new SqlParameter("@Year", Year));
                if (!string.IsNullOrEmpty(App.Trim()))
                    sqlParamList.Add(new SqlParameter("@App", App));

                if (!string.IsNullOrEmpty(Fund_Hie.Trim()))
                    sqlParamList.Add(new SqlParameter("@Fund_Hie", Fund_Hie));
                if (!string.IsNullOrEmpty(Site.Trim()))
                    sqlParamList.Add(new SqlParameter("@Site", Site));
                if (!string.IsNullOrEmpty(Room.Trim()))
                {
                    if (Room.Trim() != "****")
                        sqlParamList.Add(new SqlParameter("@Room", Room));
                }

                if (!string.IsNullOrEmpty(AmPm.Trim()))
                    sqlParamList.Add(new SqlParameter("@AmPm", AmPm));

                sqlParamList.Add(new SqlParameter("@Include_Trns_Rec", 'Y'));

                DataSet CASEENRLData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CASEENRL_Asof_Date_Attendance]");

                if (CASEENRLData != null && CASEENRLData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASEENRLData.Tables[0].Rows)
                        CASEENRLProfile.Add(new ENRL_Asof_Entity(row));
                }
            }
            catch (Exception ex)
            { return CASEENRLProfile; }

            return CASEENRLProfile;
        }

        public List<ENRL_Asof_Entity> Browse_CASEENRL_Asof_Date_Attendance(string Agency, string Dept, string Prog, string Year, string App, string Fund_Hie, string Site, string Room, string AmPm, string Asof_Date,string strEnrlId)
        {
            List<ENRL_Asof_Entity> CASEENRLProfile = new List<ENRL_Asof_Entity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@Tmp_Asof_Date", Asof_Date));

                sqlParamList.Add(new SqlParameter("@Agency", Agency));
                sqlParamList.Add(new SqlParameter("@Dept", Dept));
                sqlParamList.Add(new SqlParameter("@Prog", Prog));
                sqlParamList.Add(new SqlParameter("@Year", Year));
                if (!string.IsNullOrEmpty(App.Trim()))
                    sqlParamList.Add(new SqlParameter("@App", App));

                if (!string.IsNullOrEmpty(Fund_Hie.Trim()))
                    sqlParamList.Add(new SqlParameter("@Fund_Hie", Fund_Hie));
                if (!string.IsNullOrEmpty(Site.Trim()))
                    sqlParamList.Add(new SqlParameter("@Site", Site));
                if (!string.IsNullOrEmpty(Room.Trim()))
                {
                    if (Room.Trim() != "****")
                        sqlParamList.Add(new SqlParameter("@Room", Room));
                }

                if (!string.IsNullOrEmpty(AmPm.Trim()))
                    sqlParamList.Add(new SqlParameter("@AmPm", AmPm));

                sqlParamList.Add(new SqlParameter("@Include_Trns_Rec", 'Y'));

                if (!string.IsNullOrEmpty(strEnrlId.Trim()))
                    sqlParamList.Add(new SqlParameter("@Enrl_Id", strEnrlId));

                DataSet CASEENRLData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CASEENRL_Asof_Date_Attendance]");

                if (CASEENRLData != null && CASEENRLData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASEENRLData.Tables[0].Rows)
                        CASEENRLProfile.Add(new ENRL_Asof_Entity(row));
                }
            }
            catch (Exception ex)
            { return CASEENRLProfile; }

            return CASEENRLProfile;
        }


        public List<CaseEnrlEntity> Browse_CASEENRL_ForApp_InMultFunds(CaseEnrlEntity Entity, string Opretaion_Mode)
        {
            List<CaseEnrlEntity> CASEENRLProfile = new List<CaseEnrlEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASEENRL_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASEENRLData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CASEENRL_ForApp_InMultFunds]");

                if (CASEENRLData != null && CASEENRLData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASEENRLData.Tables[0].Rows)
                        CASEENRLProfile.Add(new CaseEnrlEntity(row, "Y"));
                }
            }
            catch (Exception ex)
            { return CASEENRLProfile; }

            return CASEENRLProfile;
        }


        public List<CaseEnrlEntity> Get_Attenpost_Asof_DateRange(string strAsofFDate,string strAsofTDate,string strStatus,string strAgency,string strDept,string strProgram,string strYear,string strApp,string strFundHie,string strRecType,string strSite, string strRoom,string strAMpm)
        {
            List<CaseEnrlEntity> CASEENRLProfile = new List<CaseEnrlEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@Asof_From_Date", strAsofFDate));
                sqlParamList.Add(new SqlParameter("@Asof_To_Date", strAsofTDate));
                sqlParamList.Add(new SqlParameter("@Asof_Status", strStatus));


                sqlParamList.Add(new SqlParameter("@Agency", strAgency));
                sqlParamList.Add(new SqlParameter("@Dept", strDept));
                sqlParamList.Add(new SqlParameter("@Prog", strProgram));
                sqlParamList.Add(new SqlParameter("@Year", strYear));
                if (!string.IsNullOrEmpty(strApp.Trim()))
                    sqlParamList.Add(new SqlParameter("@App", strApp));

                if (!string.IsNullOrEmpty(strFundHie.Trim()))
                    sqlParamList.Add(new SqlParameter("@Fund_Hie", strFundHie));
                if (!string.IsNullOrEmpty(strSite.Trim()))
                    sqlParamList.Add(new SqlParameter("@Site", strSite));
                if (!string.IsNullOrEmpty(strRoom.Trim()))
                {
                    if (strRoom.Trim() != "****")
                        sqlParamList.Add(new SqlParameter("@Room", strRoom));
                }

                if (!string.IsNullOrEmpty(strAMpm.Trim()))
                    sqlParamList.Add(new SqlParameter("@AmPm", strAMpm));

                DataSet CASEENRLData = Captain.DatabaseLayer.SPAdminDB.Get_Attenpost_Asof_DateRange(sqlParamList);

                if (CASEENRLData != null && CASEENRLData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASEENRLData.Tables[0].Rows)
                        CASEENRLProfile.Add(new CaseEnrlEntity(row));
                }
            }
            catch (Exception ex)
            { return CASEENRLProfile; }

            return CASEENRLProfile;
        }



        public List<CaseSumEntity> Browse_CASESUM(CaseSumEntity Entity, string Opretaion_Mode)
        {
            List<CaseSumEntity> CASEENRLProfile = new List<CaseSumEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASESUM_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASEENRLData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CASESUM]");

                if (CASEENRLData != null && CASEENRLData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASEENRLData.Tables[0].Rows)
                        CASEENRLProfile.Add(new CaseSumEntity(row, "CASEDEPCONT"));
                }
            }
            catch (Exception ex)
            { return CASEENRLProfile; }

            return CASEENRLProfile;
        }

        public bool UpdateCASEENRL(CaseEnrlEntity Entity, string Operation_Mode, string App_Xml, string Hist_Xml, string FieldHist_Xml, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_CASEENRL_SqlParameters_List(Entity, Operation_Mode);

                sqlParamList.Add(new SqlParameter("@Applicants_XML", App_Xml));
                sqlParamList.Add(new SqlParameter("@History_XML", Hist_Xml));
                sqlParamList.Add(new SqlParameter("@FieldHistory_XML", FieldHist_Xml));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateCASEENRL", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateCASEENRL_Status_Date(string Rec_ID, string New_Date, string User_ID, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@ENRL_ID", Rec_ID));
                sqlParamList.Add(new SqlParameter("@ENRL_Status_Date", New_Date));
                sqlParamList.Add(new SqlParameter("@ENRL_LSTC_OPR", User_ID));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateCASEENRL_New_StatusDate", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateCASEENRL_Denied_Status(string Enrl_ID, string Denied_Status, string Lsct_Opr, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {

                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@ENRL_ID", Enrl_ID));
                sqlParamList.Add(new SqlParameter("@ENRL_Denied_Stat", Denied_Status));
                sqlParamList.Add(new SqlParameter("@ENRL_LSTC_OPR", Lsct_Opr));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateCASEENRL_Denied_Status", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateENRLHIST(string Row_Type, string Enrl_ID, string Status, string Hist_Xml, string StatChg_Hist_Xml, string Operator, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();


                sqlParamList.Add(new SqlParameter("@Row_Type", Row_Type));
                sqlParamList.Add(new SqlParameter("@ENRL_ID", Enrl_ID));
                sqlParamList.Add(new SqlParameter("@Enrlhist_STATUS", Status));
                sqlParamList.Add(new SqlParameter("@History_XML", Hist_Xml));
                sqlParamList.Add(new SqlParameter("@FieldHistory_XML", StatChg_Hist_Xml));
                sqlParamList.Add(new SqlParameter("@Enrlhist_LSTC_OPERATOR", Operator));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateENRLHIST", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }



        public bool UpdateENRLFLDHIST(string Row_Type, string Enrl_ID, string Hist_Seq, string Hist_Xml, string Operator, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@Row_Type", Row_Type));
                sqlParamList.Add(new SqlParameter("@EFHIST_ID", Enrl_ID));
                sqlParamList.Add(new SqlParameter("@EFHIST_XML", Hist_Xml));
                sqlParamList.Add(new SqlParameter("@EFHIST_ADD_OPERATOR", Operator));
                sqlParamList.Add(new SqlParameter("@EFHIST_Seq", Hist_Seq));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateENRLFLDHIST", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateCASESUM_From_ENRL(CaseEnrlEntity Entity, string Operation_Mode, string App_Xml, string Hist_Xml, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Row_Type));

                sqlParamList.Add(new SqlParameter("@Applicants_XML", Entity.Agy));
                sqlParamList.Add(new SqlParameter("@Lstc_Operator", Entity.Dept));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateCASESUM_From_ENRL", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }


        public List<Sum_Referral_Entity> Get_CaseSum_RefApps_On_MSTSNP(CaseSumEntity Entity, string Opretaion_Mode)
        {
            List<Sum_Referral_Entity> CASEENRLProfile = new List<Sum_Referral_Entity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASESUM_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASEENRLData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Get_CaseSum_RefApps_On_MSTSNP]");

                if (CASEENRLData != null && CASEENRLData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASEENRLData.Tables[0].Rows)
                        CASEENRLProfile.Add(new Sum_Referral_Entity(row));
                }
            }
            catch (Exception ex)
            { return CASEENRLProfile; }

            return CASEENRLProfile;
        }


        public List<SqlParameter> Prepare_CASEENRL_SqlParameters_List(CaseEnrlEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Row_Type));

                if (Entity.Join_Mst_Snp == "Y" && Opretaion_Mode == "Browse")
                    sqlParamList.Add(new SqlParameter("@Join_Mst_Snp", Entity.Join_Mst_Snp));

                sqlParamList.Add(new SqlParameter("@ENRL_AGENCY", Entity.Agy));
                sqlParamList.Add(new SqlParameter("@ENRL_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@ENRL_PROG", Entity.Prog));
                sqlParamList.Add(new SqlParameter("@ENRL_YEAR", Entity.Year));
                sqlParamList.Add(new SqlParameter("@ENRL_APP_NO", Entity.App));
                sqlParamList.Add(new SqlParameter("@ENRL_GROUP", Entity.Group));
                sqlParamList.Add(new SqlParameter("@ENRL_FUND_HIE", Entity.FundHie));
                sqlParamList.Add(new SqlParameter("@ENRL_SEQ", Entity.Seq));
                sqlParamList.Add(new SqlParameter("@ENRL_ID", Entity.ID));
                sqlParamList.Add(new SqlParameter("@ENRL_RECORD_TYPE", Entity.Rec_Type));
                sqlParamList.Add(new SqlParameter("@ENRL_STATUS", Entity.Status));
                sqlParamList.Add(new SqlParameter("@ENRL_STATUS_REASN", Entity.Status_Reason));
                sqlParamList.Add(new SqlParameter("@ENRL_DATE", Entity.Status_Date));

                sqlParamList.Add(new SqlParameter("@ENRL_SITE", Entity.Site));
                sqlParamList.Add(new SqlParameter("@ENRL_ROOM", Entity.Room));
                sqlParamList.Add(new SqlParameter("@ENRL_AMPM", Entity.AMPM));
                sqlParamList.Add(new SqlParameter("@ENRL_ENRLD_Date", Entity.Enrl_Date));

                sqlParamList.Add(new SqlParameter("@ENRL_WDRAW_CODE", Entity.Withdraw_Code));
                sqlParamList.Add(new SqlParameter("@ENRL_WDRAW_Date", Entity.Withdraw_Date));
                sqlParamList.Add(new SqlParameter("@ENRL_WLIST_Date", Entity.Wiait_Date));
                sqlParamList.Add(new SqlParameter("@ENRL_DENIED_CODE", Entity.Denied_Code));
                sqlParamList.Add(new SqlParameter("@ENRL_DENIED_Date", Entity.Denied_Date));
                sqlParamList.Add(new SqlParameter("@ENRL_PENDING_CODE", Entity.Pending_Code));
                sqlParamList.Add(new SqlParameter("@ENRL_PENDING_Date", Entity.Pending_Date));
                sqlParamList.Add(new SqlParameter("@ENRL_RANK", Entity.Rank));
                sqlParamList.Add(new SqlParameter("@ENRL_RNKCHNG_CODE", Entity.Rank_Chg_Code));
                sqlParamList.Add(new SqlParameter("@ENRL_TRAN_TYPE", Entity.Transc_Type));
                sqlParamList.Add(new SqlParameter("@ENRL_TRANSFER_SITE", Entity.Tranfr_Site));

                sqlParamList.Add(new SqlParameter("@ENRL_TRANSFER_ROOM", Entity.Tranfr_Room));
                sqlParamList.Add(new SqlParameter("@ENRL_TRANSFER_AMPM", Entity.Tranfr_AMPM));
                sqlParamList.Add(new SqlParameter("@ENRL_PARENT_RATE", Entity.Parent_Rate));
                sqlParamList.Add(new SqlParameter("@ENRL_FUNDING_CODE", Entity.Funding_Code));
                sqlParamList.Add(new SqlParameter("@ENRL_FUNDING_RATE", Entity.Funding_Rate));
                sqlParamList.Add(new SqlParameter("@ENRL_DESC_1", Entity.Desc_1));
                sqlParamList.Add(new SqlParameter("@ENRL_DESC_2", Entity.Desc_2));
                sqlParamList.Add(new SqlParameter("@ENRL_FUND_END_Date", Entity.Fund_End_date));

                sqlParamList.Add(new SqlParameter("@ENRL_RATE_EFF_Date", Entity.Rate_EFR_date));
                sqlParamList.Add(new SqlParameter("@ENRL_LSTC_OPERATOR", Entity.Lstc_Oper));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@ENRL_Date_ADD", Entity.Add_Date));
                    sqlParamList.Add(new SqlParameter("@ENRL_ADD_OPERATOR", Entity.Add_Oper));
                    sqlParamList.Add(new SqlParameter("@ENRL_Date_LSTC", Entity.Lstc_Date));
                    sqlParamList.Add(new SqlParameter("@Asof_Date", Entity.Asof_Date));
                    sqlParamList.Add(new SqlParameter("@Status_Not_Equal", Entity.Enrl_Status_Not_Equalto));
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        /// <summary>
        /// Get Program information. 
        /// </summary>        
        /// <returns>Returns a DataSet with the screens.</returns>
        public ProgramDefinitionEntity GetProgram(string Agency, string Dept, string Prog)
        {
            ProgramDefinitionEntity programEntity = new ProgramDefinitionEntity();
            try
            {
                DataSet programData = Captain.DatabaseLayer.MainMenu.GetCaseDepForHierarchy(Agency, Dept, Prog);
                if (programData != null && programData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in programData.Tables[0].Rows)
                    {
                        programEntity = new ProgramDefinitionEntity(row);
                    }
                }
            }
            catch (Exception ex)
            {
                return programEntity;
            }

            return programEntity;
        }


        public List<ENRLHIST_Entity> Browse_ENRLHIST(ENRLHIST_Entity Entity, string Opretaion_Mode)
        {
            List<ENRLHIST_Entity> CASEENRLProfile = new List<ENRLHIST_Entity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_ENRLHIST_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASEENRLData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_ENRLHIST]");

                if (CASEENRLData != null && CASEENRLData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASEENRLData.Tables[0].Rows)
                        CASEENRLProfile.Add(new ENRLHIST_Entity(row, Entity.Asof_Date));
                }
            }
            catch (Exception ex)
            { return CASEENRLProfile; }

            return CASEENRLProfile;
        }

        public List<CaseEnrlEntity> Browse_ENRLHIST_NoStatus(string Agy, string Dept, string Prog, string Year, string Rec_Type, string Site, string App, string Show_Case, string Opretaion_Mode)
        {
            List<CaseEnrlEntity> CASEENRLProfile = new List<CaseEnrlEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@ENRL_Show_Case", Show_Case));
                sqlParamList.Add(new SqlParameter("@ENRL_AGENCY", Agy));
                sqlParamList.Add(new SqlParameter("@ENRL_DEPT", Dept));
                sqlParamList.Add(new SqlParameter("@ENRL_PROG", Prog));
                sqlParamList.Add(new SqlParameter("@ENRL_YEAR", Year));
                sqlParamList.Add(new SqlParameter("@ENRL_Rec_Type", Rec_Type));
                if (Rec_Type == "H")
                    sqlParamList.Add(new SqlParameter("@ENRL_Site", Site));
                else
                    sqlParamList.Add(new SqlParameter("@ENRL_FUNDHIE", Site));
                sqlParamList.Add(new SqlParameter("@ENRL_APP_NO", App));

                string SP_TO_Execute =  "[dbo].[Browse_CASEENRL_No_Status]";
                switch(Show_Case)
                {
                    case "2": SP_TO_Execute = "[dbo].[Browse_CASEENRL_No_Status_2]"; break;
                    case "3": SP_TO_Execute = "[dbo].[Browse_CASEENRL_No_Status_3]"; break;
                }
                DataSet CASEENRLData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, SP_TO_Execute);

                if (CASEENRLData != null && CASEENRLData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASEENRLData.Tables[0].Rows)
                        CASEENRLProfile.Add(new CaseEnrlEntity(row, "Y"));
                }
            }
            catch (Exception ex)
            { return CASEENRLProfile; }

            return CASEENRLProfile;
        }



        public DataSet Browse_ENRLFLDHIST(string Enrl_ID, string Operator, string Add_Date)
        {
            List<ENRLHIST_Entity> CASEENRLProfile = new List<ENRLHIST_Entity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            DataSet CASEENRLData = new DataSet();
            try
            {
                sqlParamList.Add(new SqlParameter("@EFHIST_ID", Enrl_ID));
                sqlParamList.Add(new SqlParameter("@EFHIST_ADD_OPERATOR", Operator));
                sqlParamList.Add(new SqlParameter("@EFHIST_ADD_DATE", Add_Date));

                CASEENRLData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_ENRLFLDHIST]");

                //if (CASEENRLData != null && CASEENRLData.Tables[0].Rows.Count > 0)
                //{
                //    foreach (DataRow row in CASEENRLData.Tables[0].Rows)
                //        CASEENRLProfile.Add(new ENRLHIST_Entity(row, Entity.Asof_Date));
                //}
            }
            catch (Exception ex)
            { return CASEENRLData; }

            return CASEENRLData;
        }


        public List<SqlParameter> Prepare_ENRLHIST_SqlParameters_List(ENRLHIST_Entity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@ENRLHIST_ID", Entity.ID));
                sqlParamList.Add(new SqlParameter("@ENRLHIST_STATUS", Entity.Status));
                sqlParamList.Add(new SqlParameter("@ENRLHIST_SEQ", Entity.Seq));
                sqlParamList.Add(new SqlParameter("@ENRLHIST_FDATA", Entity.From_Date));
                sqlParamList.Add(new SqlParameter("@ENRLHIST_TDATE", Entity.TO_Date));
                sqlParamList.Add(new SqlParameter("@ENRLHIST_SITE", Entity.Site));
                sqlParamList.Add(new SqlParameter("@ENRLHIST_ROOM", Entity.Room));
                sqlParamList.Add(new SqlParameter("@ENRLHIST_AMPM", Entity.AMPM));

                sqlParamList.Add(new SqlParameter("@Asof_Date", Entity.Asof_Date == "N" ? null : Entity.Asof_Date));
                sqlParamList.Add(new SqlParameter("@Sel_Agency", Entity.Enrl_Agy));
                sqlParamList.Add(new SqlParameter("@Sel_Dept", Entity.Enrl_Dept));
                sqlParamList.Add(new SqlParameter("@Sel_Prog", Entity.Enrl_Prog));
                sqlParamList.Add(new SqlParameter("@Sel_App_No", Entity.Enrl_App));
                sqlParamList.Add(new SqlParameter("@Sel_Module_Type", Entity.Module_Type));
                sqlParamList.Add(new SqlParameter("@ENRLHIST_LSTC_OPERATOR", Entity.Lstc_Opr));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@ENRLHIST_DATE_ADD", Entity.Date_Add));
                    sqlParamList.Add(new SqlParameter("@ENRLHIST_ADD_OPERATOR", Entity.Add_Opr));
                    sqlParamList.Add(new SqlParameter("@ENRLHIST_DATE_LSTC", Entity.Lstc_Dtae));
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<SqlParameter> Prepare_CASESUM_SqlParameters_List(CaseSumEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@CASESUM_AGENCY", Entity.CaseSumAgency));
                sqlParamList.Add(new SqlParameter("@CASESUM_DEPT", Entity.CaseSumDept));
                sqlParamList.Add(new SqlParameter("@CASESUM_PROGRAM", Entity.CaseSumProgram));
                sqlParamList.Add(new SqlParameter("@CASESUM_YEAR", Entity.CaseSumYear));
                sqlParamList.Add(new SqlParameter("@CASESUM_APP_NO", Entity.CaseSumApplNo));
                sqlParamList.Add(new SqlParameter("@CASESUM_REF_AGENCY", Entity.CaseSumRefHierachy));
                //sqlParamList.Add(new SqlParameter("@CASESUM_REF_DEPT", Entity.CaseSumRefDept));
                //sqlParamList.Add(new SqlParameter("@CASESUM_REF_PROGRAM", Entity.CaseSumRefProgram));
                sqlParamList.Add(new SqlParameter("@CASESUM_REF_YEAR", Entity.CaseSumRefYear));

                //if (Opretaion_Mode == "Browse")
                //{
                //    sqlParamList.Add(new SqlParameter("@ENRLHIST_DATE_ADD", Entity.Date_Add));
                //    sqlParamList.Add(new SqlParameter("@ENRLHIST_ADD_OPERATOR", Entity.Add_Opr));
                //    sqlParamList.Add(new SqlParameter("@ENRLHIST_DATE_LSTC", Entity.Lstc_Dtae));
                //}
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<SqlParameter> Prepare_MST_SUM_NonEnrollSqlParameters_List(CaseSumEntity Entity, string Opretaion_Mode, string Base_Agy, string Base_Dept, string Base_Prog, string Base_Year)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@CASESUM_AGENCY", Entity.CaseSumAgency));
                sqlParamList.Add(new SqlParameter("@CASESUM_DEPT", Entity.CaseSumDept));
                sqlParamList.Add(new SqlParameter("@CASESUM_PROGRAM", Entity.CaseSumProgram));
                sqlParamList.Add(new SqlParameter("@CASESUM_YEAR", Entity.CaseSumYear));
                sqlParamList.Add(new SqlParameter("@CASESUM_APP_NO", Entity.CaseSumApplNo));
                sqlParamList.Add(new SqlParameter("@CASESUM_REF_Hierarchy", Entity.CaseSumRefHierachy));
                //sqlParamList.Add(new SqlParameter("@CASESUM_REF_DEPT", Entity.CaseSumRefDept));
                //sqlParamList.Add(new SqlParameter("@CASESUM_REF_PROGRAM", Entity.CaseSumRefProgram));

                sqlParamList.Add(new SqlParameter("@CASESUM_REF_YEAR", Entity.CaseSumRefYear));

                sqlParamList.Add(new SqlParameter("@MST_AGENCY", Base_Agy));
                sqlParamList.Add(new SqlParameter("@MST_DEPT", Base_Dept));
                sqlParamList.Add(new SqlParameter("@MST_PROGRAM", Base_Prog));
                sqlParamList.Add(new SqlParameter("@MST_YEAR", Base_Year));

                //if (Opretaion_Mode == "Browse")
                //{
                //    sqlParamList.Add(new SqlParameter("@ENRLHIST_DATE_ADD", Entity.Date_Add));
                //    sqlParamList.Add(new SqlParameter("@ENRLHIST_ADD_OPERATOR", Entity.Add_Opr));
                //    sqlParamList.Add(new SqlParameter("@ENRLHIST_DATE_LSTC", Entity.Lstc_Dtae));
                //}
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }
        /// <summary>
        /// HSS2108 Report Data Enroll table
        /// </summary>
        /// <param name="Agency"></param>
        /// <param name="Dept"></param>
        /// <param name="Program"></param>
        /// <param name="Year"></param>      
        /// <returns></returns>
        public List<CaseEnrlEntity> GetEnrollDetails2108(string Agency, string Dept, string Program, string Year, string Type)
        {
            List<CaseEnrlEntity> caseEnroll = new List<CaseEnrlEntity>();
            try
            {
                DataSet SsnData = Captain.DatabaseLayer.ChldAttnDB.GetEnrollDetails2108(Agency, Dept, Program, Year, Type);
                if (SsnData != null && SsnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in SsnData.Tables[0].Rows)
                    {
                        caseEnroll.Add(new CaseEnrlEntity(row, string.Empty, "HSSB2108"));
                    }
                }
            }
            catch (Exception ex)
            {
                return caseEnroll;
            }

            return caseEnroll;
        }

        /// <summary>
        /// HSS2109 Report Data Enroll table
        /// </summary>
        /// <param name="Agency"></param>
        /// <param name="Dept"></param>
        /// <param name="Program"></param>
        /// <param name="Year"></param>      
        /// <returns></returns>
        public List<CaseEnrlEntity> GetEnrollDetails2109(string Agency, string Dept, string Program, string Year, string Type, string ToDate, string Site, string FundHie, string Applicant, string Sequence, string strDays, string FromDt)
        {
            List<CaseEnrlEntity> caseEnroll = new List<CaseEnrlEntity>();
            try
            {
                DataSet SsnData = Captain.DatabaseLayer.ChldAttnDB.GetEnrollDetails2109(Agency, Dept, Program, Year, Type, ToDate, Site, FundHie, Applicant, Sequence, strDays, FromDt);
                if (SsnData != null && SsnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in SsnData.Tables[0].Rows)
                    {
                        caseEnroll.Add(new CaseEnrlEntity(row, string.Empty, "HSSB2109"));
                    }
                }
            }
            catch (Exception ex)
            {
                return caseEnroll;
            }

            return caseEnroll;
        }


        /// <summary>
        /// HSS2103 Report Data 
        /// </summary>
        /// <param name="Agency"></param>
        /// <param name="Dept"></param>
        /// <param name="Program"></param>
        /// <param name="Year"></param>      
        /// <returns></returns>
        public List<CaseEnrlEntity> GetEnrollDetails2103(string Agency, string Dept, string Program, string Year, string Type, string Site, string FundHie, string Zip, string Task, string Childrenwith, string Sequence,string strAppno)
        {
            List<CaseEnrlEntity> caseEnroll = new List<CaseEnrlEntity>();
            try
            {
                DataSet SsnData = Captain.DatabaseLayer.ChldAttnDB.GetEnrollDetails2103(Agency, Dept, Program, Year, Type, Site, FundHie, Zip, Task, Childrenwith, Sequence, strAppno);
                if (SsnData != null && SsnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in SsnData.Tables[0].Rows)
                    {
                        caseEnroll.Add(new CaseEnrlEntity(row, string.Empty, "HSSB2103"));
                    }
                }
            }
            catch (Exception ex)
            {
                return caseEnroll;
            }

            return caseEnroll;
        }


        public List<CaseEnrlEntity> GetEnrollDetails2114(string Agency, string Dept, string Program, string Year, string ApplNo, string Type, string Site, string FundHie, string Task, string EnrollStatus, string Sequence)
        {
            List<CaseEnrlEntity> caseEnroll = new List<CaseEnrlEntity>();
            try
            {
                DataSet SsnData = Captain.DatabaseLayer.ChldAttnDB.GetEnrollDetails2114(Agency, Dept, Program, Year, ApplNo, Type, Site, FundHie, Task, EnrollStatus, Sequence);
                if (SsnData != null && SsnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in SsnData.Tables[0].Rows)
                    {
                        caseEnroll.Add(new CaseEnrlEntity(row, string.Empty, "HSSB2114"));
                    }
                }
            }
            catch (Exception ex)
            {
                return caseEnroll;
            }

            return caseEnroll;
        }




        /// <summary>
        /// HSS2111 Report Data Enroll table
        /// </summary>
        /// <param name="Agency"></param>
        /// <param name="Dept"></param>
        /// <param name="Program"></param>
        /// <param name="Year"></param>      
        /// <returns></returns>
        public List<CaseEnrlEntity> Get2111SummaryDetails(string Agency, string Dept, string Prog, string Year, string StartDate, string EndDate, string Sites, string strTable,string strFunds)
        {
            List<CaseEnrlEntity> caseEnroll = new List<CaseEnrlEntity>();
            try
            {
                DataSet SsnData = Captain.DatabaseLayer.ChldAttnDB.Get2111SummaryDetails(Agency, Dept, Prog, Year, StartDate, EndDate, Sites,strFunds);
                if (SsnData != null && SsnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in SsnData.Tables[0].Rows)
                    {
                        caseEnroll.Add(new CaseEnrlEntity(row, string.Empty, strTable));
                    }
                }
            }
            catch (Exception ex)
            {
                return caseEnroll;
            }

            return caseEnroll;
        }


        /// <summary>
        /// HSS2111 Report Data Enroll table
        /// </summary>
        /// <param name="Agency"></param>
        /// <param name="Dept"></param>
        /// <param name="Program"></param>
        /// <param name="Year"></param>      
        /// <returns></returns>
        public List<CaseEnrlEntity> Get2111Details(string Agency, string Dept, string Prog, string Year, string StartDate, string EndDate, string Sites, string strTable)
        {
            List<CaseEnrlEntity> caseEnroll = new List<CaseEnrlEntity>();
            try
            {
                DataSet SsnData = Captain.DatabaseLayer.ChldAttnDB.Get2111Details(Agency, Dept, Prog, Year, StartDate, EndDate, Sites);
                if (SsnData != null && SsnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in SsnData.Tables[0].Rows)
                    {
                        caseEnroll.Add(new CaseEnrlEntity(row, string.Empty, strTable));
                    }
                }
            }
            catch (Exception ex)
            {
                return caseEnroll;
            }

            return caseEnroll;
        }

        public List<CaseEnrlEntity> Get2111ExcelDetails(string Agency, string Dept, string Prog, string Year, string strType,  string Sites,string strFundHie, string strTable,string strFromDt,string strToDt)
        {
            List<CaseEnrlEntity> caseEnroll = new List<CaseEnrlEntity>();
            try
            {
                DataSet SsnData = Captain.DatabaseLayer.ChldAttnDB.Get2111ExcelDetails(Agency, Dept, Prog, Year, strType,  Sites,strFundHie,strFromDt,strToDt);
                if (SsnData != null && SsnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in SsnData.Tables[0].Rows)
                    {
                        caseEnroll.Add(new CaseEnrlEntity(row, string.Empty, strTable));
                    }
                }
            }
            catch (Exception ex)
            {
                return caseEnroll;
            }

            return caseEnroll;
        }


        /// <summary>
        /// PIR2000 Screen Data Enroll table
        /// </summary>
        /// <param name="Agency"></param>
        /// <param name="Dept"></param>
        /// <param name="Program"></param>
        /// <param name="Year"></param>
        /// <returns></returns>


        public List<CaseEnrlEntity> GetEnrollDetailsPIR2000(string Agency, string Dept, string Program, string Year)
        {
            List<CaseEnrlEntity> caseEnroll = new List<CaseEnrlEntity>();
            try
            {
                DataSet SsnData = Captain.DatabaseLayer.ChldAttnDB.GetEnrollDetailsPIR2000(Agency, Dept, Program, Year);
                if (SsnData != null && SsnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in SsnData.Tables[0].Rows)
                    {
                        caseEnroll.Add(new CaseEnrlEntity(row, string.Empty, "PIR20000"));
                    }
                }
            }
            catch (Exception ex)
            {
                return caseEnroll;
            }

            return caseEnroll;
        }


        /// <summary>
        /// PIR2000 Screen Data Pirwork table
        /// </summary>
        /// <param name="Agency"></param>
        /// <param name="Dept"></param>
        /// <param name="Program"></param>
        /// <param name="Year"></param>
        /// <returns></returns>


        public List<PirWorkEntity> GetPirWorkData(string Agency, string Dept, string Program, string Year,string strmode)
        {
            List<PirWorkEntity> pirWork = new List<PirWorkEntity>();
            try
            {
                DataSet PirWorkData = Captain.DatabaseLayer.ChldAttnDB.GetPirWorkData(Agency, Dept, Program, Year,strmode);
                if (PirWorkData != null && PirWorkData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in PirWorkData.Tables[0].Rows)
                    {
                        pirWork.Add(new PirWorkEntity(row,true, strmode));
                    }
                }
            }
            catch (Exception ex)
            {
                return pirWork;
            }

            return pirWork;
        }

        public List<PirWorkEntity> GetGenerateworkDETAILS(string Agency, string Dept, string Program, string Year,string Mode)
        {
            List<PirWorkEntity> pirWork = new List<PirWorkEntity>();
            try
            {
                DataSet PirWorkData = Captain.DatabaseLayer.ChldAttnDB.GetGenerateworkDETAILS(Agency, Dept, Program, Year,Mode);
                if (PirWorkData != null && PirWorkData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in PirWorkData.Tables[0].Rows)
                    {
                        pirWork.Add(new PirWorkEntity(row,string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                return pirWork;
            }

            return pirWork;
        }

        public bool InsertDelPirWorks(PirWorkEntity pirWorkEntity)
        {
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@PIRWORK_AGENCY", pirWorkEntity.PIRWORK_AGENCY));
                sqlParamList.Add(new SqlParameter("@PIRWORK_DEPT", pirWorkEntity.PIRWORK_DEPT));
                sqlParamList.Add(new SqlParameter("@PIRWORK_PROG", pirWorkEntity.PIRWORK_PROG));
                if (pirWorkEntity.PIRWORK_YEAR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRWORK_YEAR", pirWorkEntity.PIRWORK_YEAR));
                if (pirWorkEntity.PIRWORK_Show != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRWORK_SHOW", pirWorkEntity.PIRWORK_Show));
                if (pirWorkEntity.Mode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Mode", pirWorkEntity.Mode));
                Captain.DatabaseLayer.ChldAttnDB.InsertDelPirWorkDETAILS(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool INSERTUPDATEPIRWITHDRAW(PirWorkEntity pirWorkEntity)
        {
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@PIRWIDW_AGENCY", pirWorkEntity.PIRWORK_AGENCY));
                sqlParamList.Add(new SqlParameter("@PIRWIDW_DEPT", pirWorkEntity.PIRWORK_DEPT));
                sqlParamList.Add(new SqlParameter("@PIRWIDW_PROG", pirWorkEntity.PIRWORK_PROG));
                if (pirWorkEntity.PIRWORK_YEAR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRWIDW_YEAR", pirWorkEntity.PIRWORK_YEAR));
                if (pirWorkEntity.PIRWORK_APP_NO != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRWIDW_APP_NO", pirWorkEntity.PIRWORK_APP_NO));

                if (pirWorkEntity.PIRWORK_FUND != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRWIDW_FUND", pirWorkEntity.PIRWORK_FUND));

                if (pirWorkEntity.PIRWORK_WDRAW_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRWIDW_WDRAW_DATE", pirWorkEntity.PIRWORK_WDRAW_DATE));

                if (pirWorkEntity.PIRWORK_FAMILY_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRWIDW_FAMILY_ID", pirWorkEntity.PIRWORK_FAMILY_ID));

                if (pirWorkEntity.PIRWORK_INCOME_TYPES != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRWIDW_INCOME_TYPES", pirWorkEntity.PIRWORK_INCOME_TYPES));

                if (pirWorkEntity.PIRWORK_POVERTY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRWIDW_POVERTY", pirWorkEntity.PIRWORK_POVERTY));

                if (pirWorkEntity.PIRWORK_FAMILY_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRWIDW_FAMILY_TYPE", pirWorkEntity.PIRWORK_FAMILY_TYPE));

                if (pirWorkEntity.PIRWORK_LANGUAGE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRWIDW_LANGUAGE", pirWorkEntity.PIRWORK_LANGUAGE));
                
                if (pirWorkEntity.Mode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Mode", pirWorkEntity.Mode));
                Captain.DatabaseLayer.ChldAttnDB.INSERTUPDATEPIRWITHDRAW(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return true;
        }



        public List<PirCntl> GetPirCntlData()
        {
            List<PirCntl> pirCntl = new List<PirCntl>();
            try
            {
                DataSet PirWorkData = Captain.DatabaseLayer.ChldAttnDB.GetPirCntlData();
                if (PirWorkData != null && PirWorkData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in PirWorkData.Tables[0].Rows)
                    {
                        pirCntl.Add(new PirCntl(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return pirCntl;
            }

            return pirCntl;
        }


        public bool InsertDelPirCntl(PirCntl pirCntl)
        {
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@PIRCNTL_FUND_CODE", pirCntl.PIRCNTL_FUND_CODE));
                sqlParamList.Add(new SqlParameter("@PIRCNTL_FUND_NAME", pirCntl.PIRCNTL_FUND_NAME));
                sqlParamList.Add(new SqlParameter("@PIRCNTL_FUND_TYPE", pirCntl.PIRCNTL_FUND_TYPE));
                sqlParamList.Add(new SqlParameter("@PIRCNTL_ADD_OPERATOR", pirCntl.PIRCNTL_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@PIRCNTL_LSTC_OPERATOR", pirCntl.PIRCNTL_LSTC_OPERATOR));
                sqlParamList.Add(new SqlParameter("@Mode", pirCntl.Mode));
                Captain.DatabaseLayer.ChldAttnDB.InsertDelPirCntl(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return true;
        }



        public List<PirassnEntity> GetPirassnData(string Agency, string Dept, string Program, string Year)
        {
            List<PirassnEntity> pirassn = new List<PirassnEntity>();
            try
            {
                DataSet PirWorkData = Captain.DatabaseLayer.ChldAttnDB.GetPirassnData(Agency, Dept, Program, Year);
                if (PirWorkData != null && PirWorkData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in PirWorkData.Tables[0].Rows)
                    {
                        pirassn.Add(new PirassnEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return pirassn;
            }

            return pirassn;
        }


        public bool InsertDelPirassn(PirassnEntity pirassnEntity)
        {
            bool Success = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@PIRASSN_AGENCY", pirassnEntity.PIRASSN_AGENCY));
                sqlParamList.Add(new SqlParameter("@PIRASSN_DEPT", pirassnEntity.PIRASSN_DEPT));
                sqlParamList.Add(new SqlParameter("@PIRASSN_PROG", pirassnEntity.PIRASSN_PROG));
                if (pirassnEntity.PIRASSN_YEAR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_YEAR", pirassnEntity.PIRASSN_YEAR));
                if (pirassnEntity.PIRASSN_Q_FUND != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_Q_FUND", pirassnEntity.PIRASSN_Q_FUND));
                if (pirassnEntity.PIRASSN_Q_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_Q_ID", pirassnEntity.PIRASSN_Q_ID));
                if (pirassnEntity.PIRASSN_Q_SEC != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_Q_SEC", pirassnEntity.PIRASSN_Q_SEC));
                if (pirassnEntity.PIRASSN_GRP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_GRP", pirassnEntity.PIRASSN_GRP));
                if (pirassnEntity.PIRASSN_SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_SEQ", pirassnEntity.PIRASSN_SEQ));
                if (pirassnEntity.PIRASSN_Q_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_Q_TYPE", pirassnEntity.PIRASSN_Q_TYPE));
                if (pirassnEntity.PIRASSN_Q_CODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_Q_CODE", pirassnEntity.PIRASSN_Q_CODE));
                if (pirassnEntity.PIRASSN_Q_SCODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_Q_SCODE", pirassnEntity.PIRASSN_Q_SCODE));
                if (pirassnEntity.PIRASSN_CONJ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_CONJ", pirassnEntity.PIRASSN_CONJ));
                if (pirassnEntity.PIRASSN_TASK != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_TASK", pirassnEntity.PIRASSN_TASK));
                if (pirassnEntity.PIRASSN_YEAR_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_YEAR_TYPE", pirassnEntity.PIRASSN_YEAR_TYPE));
                if (pirassnEntity.PIRASSN_CHK_RESP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_CHK_RESP", pirassnEntity.PIRASSN_CHK_RESP));
                if (pirassnEntity.PIRASSN_RESPONSE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_RESPONSE", pirassnEntity.PIRASSN_RESPONSE));
                if (pirassnEntity.PIRASSN_DATE_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_DATE_TYPE", pirassnEntity.PIRASSN_DATE_TYPE));
                if (pirassnEntity.PIRASSN_FDATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_FDATE", pirassnEntity.PIRASSN_FDATE));
                if (pirassnEntity.PIRASSN_TDATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_TDATE", pirassnEntity.PIRASSN_TDATE));
                if (pirassnEntity.PIRASSN_CHK_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_CHK_DATE", pirassnEntity.PIRASSN_CHK_DATE));
                if (pirassnEntity.PIRASSN_INTAKE_AGYTAB != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_INTAKE_AGYTAB", pirassnEntity.PIRASSN_INTAKE_AGYTAB));
                if (pirassnEntity.PIRASSN_INTAKE_CODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_INTAKE_CODE", pirassnEntity.PIRASSN_INTAKE_CODE));
                if (pirassnEntity.PIRASSN_SERVICE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_SERVICE", pirassnEntity.PIRASSN_SERVICE));
                if (pirassnEntity.PIRASSN_CUSTOM_CODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_CUSTOM_CODE", pirassnEntity.PIRASSN_CUSTOM_CODE));
                if (pirassnEntity.PIRASSN_CUSTOM_RESP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_CUSTOM_RESP", pirassnEntity.PIRASSN_CUSTOM_RESP));
                if (pirassnEntity.PIRASSN_CUSTOM_SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_CUSTOM_SEQ", pirassnEntity.PIRASSN_CUSTOM_SEQ));
                if (pirassnEntity.PIRASSN_ADD_OPERATOR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_ADD_OPERATOR", pirassnEntity.PIRASSN_ADD_OPERATOR));
                if (pirassnEntity.PIRASSN_LSTC_OPERATOR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_LSTC_OPERATOR", pirassnEntity.PIRASSN_LSTC_OPERATOR));

                sqlParamList.Add(new SqlParameter("@Mode", pirassnEntity.Mode));
                Success=  Captain.DatabaseLayer.ChldAttnDB.InsertDelPirassn(sqlParamList);
            }

            catch (Exception ex)
            {
                Success=false;
            }

            return Success;
        }

        public bool InsertDelPIRQUESTRECORD(string strXmldata)
        {
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                //sqlParamList.Add(new SqlParameter("@PIRCNTL_FUND_CODE", pirassnEntity.PIRCNTL_FUND_CODE));
                //sqlParamList.Add(new SqlParameter("@PIRCNTL_FUND_NAME", pirassnEntity.PIRCNTL_FUND_NAME));
                //sqlParamList.Add(new SqlParameter("@PIRCNTL_FUND_TYPE", pirassnEntity.PIRCNTL_FUND_TYPE));
                //sqlParamList.Add(new SqlParameter("@PIRCNTL_ADD_OPERATOR", pirassnEntity.PIRCNTL_ADD_OPERATOR));
                //sqlParamList.Add(new SqlParameter("@PIRCNTL_LSTC_OPERATOR", pirassnEntity.PIRCNTL_LSTC_OPERATOR));
                //sqlParamList.Add(new SqlParameter("@Mode", pirassnEntity.Mode));
                Captain.DatabaseLayer.ChldAttnDB.InsertDelPirassn(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return true;
        }


        public List<CaseEnrlEntity> GetHss22001Details(string Agency, string Dept, string Prog, string Year, string Show, string Sequence, string Language, string Classprefer,string Site,string Fund,string PreEnrolled,string SelectFund,string DateSelection,string FromDate,string ToDate,string SpecialNeeds,string DateType, string staffcode,string strTable)
        {
            List<CaseEnrlEntity> caseEnroll = new List<CaseEnrlEntity>();
            try
            {
                DataSet SsnData = Captain.DatabaseLayer.ChldAttnDB.GetHss22001Details(Agency, Dept, Prog, Year, Show, Sequence, Language, Classprefer, Site, Fund, PreEnrolled, SelectFund, DateSelection, FromDate, ToDate, SpecialNeeds, DateType,staffcode);
                if (SsnData != null && SsnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in SsnData.Tables[0].Rows)
                    {
                        caseEnroll.Add(new CaseEnrlEntity(row, string.Empty, strTable));
                    }
                }
            }
            catch (Exception ex)
            {
                return caseEnroll;
            }

            return caseEnroll;
        }

        public bool InsertPirTables(PirassnEntity pirassnEntity)
        {
            bool boolstatus = true;
            try
            {
               
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@AGENCY", pirassnEntity.PIRASSN_AGENCY));
                sqlParamList.Add(new SqlParameter("@DEPT", pirassnEntity.PIRASSN_DEPT));
                sqlParamList.Add(new SqlParameter("@Program", pirassnEntity.PIRASSN_PROG));
                if (pirassnEntity.PIRASSN_YEAR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@YEAR", pirassnEntity.PIRASSN_YEAR));
               boolstatus = Captain.DatabaseLayer.ChldAttnDB.InsertPirTables(sqlParamList);
            }

            catch (Exception ex)
            {
                boolstatus = false;
            }

            return boolstatus;
        }

        public List<CaseEnrlEntity> GetCaseEnrl2001(string agency, string dep, string program, string year)
        {
            List<CaseEnrlEntity> CaseEnrlDetails = new List<CaseEnrlEntity>();
            try
            {
                DataSet CaseSnp = Captain.DatabaseLayer.CaseSum.GetCaseENRLHss2001(agency, dep, program, year);
                if (CaseSnp != null && CaseSnp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnp.Tables[0].Rows)
                    {
                        CaseEnrlDetails.Add(new CaseEnrlEntity(row, string.Empty,"HSS2001ENRL"));
                    }
                }
            }
            catch (Exception ex)
            {
                return CaseEnrlDetails;
            }

            return CaseEnrlDetails;
        }


        public List<ENRL_Asof_Entity> Browse_HSSB2104(string Agency, string Dept, string Prog, string Year, string App, string Fund_Hie, string Site, string Room, string AmPm, string Asof_Date)
        {
            List<ENRL_Asof_Entity> CASEENRLProfile = new List<ENRL_Asof_Entity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@Tmp_Asof_Date", Asof_Date));

                sqlParamList.Add(new SqlParameter("@Agency", Agency));
                sqlParamList.Add(new SqlParameter("@Dept", Dept));
                sqlParamList.Add(new SqlParameter("@Prog", Prog));
                sqlParamList.Add(new SqlParameter("@Year", Year));
                if (!string.IsNullOrEmpty(App.Trim()))
                    sqlParamList.Add(new SqlParameter("@App", App));

                if (!string.IsNullOrEmpty(Fund_Hie.Trim()))
                    sqlParamList.Add(new SqlParameter("@Fund_Hie", Fund_Hie));
                if (!string.IsNullOrEmpty(Site.Trim()))
                    sqlParamList.Add(new SqlParameter("@Site", Site));
                if (!string.IsNullOrEmpty(Room.Trim()))
                    sqlParamList.Add(new SqlParameter("@Room", Room));
                if (!string.IsNullOrEmpty(AmPm.Trim()))
                    sqlParamList.Add(new SqlParameter("@AmPm", AmPm));


                DataSet CASEENRLData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_HSSB2104_Report]");

                if (CASEENRLData != null && CASEENRLData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASEENRLData.Tables[0].Rows)
                        CASEENRLProfile.Add(new ENRL_Asof_Entity(row,string.Empty));
                }

                //if (CASEENRLData != null && CASEENRLData.Tables[1].Rows.Count > 0)
                //{
                //    foreach (DataRow row in CASEENRLData.Tables[1].Rows)
                //        CASEENRLProfile.Add(new ENRL_Asof_Entity(row, string.Empty,string.Empty));
                //}
            }
            catch (Exception ex)
            { return CASEENRLProfile; }

            return CASEENRLProfile;
        }

        public bool InsertUpdatePIRASSN_2(Pirassn2Entity pirassn2Entity)
        {
            bool Success = false;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@PIRASSN_AGENCY", pirassn2Entity.PIRASSN_AGENCY));
                sqlParamList.Add(new SqlParameter("@PIRASSN_DEPT", pirassn2Entity.PIRASSN_DEPT));
                sqlParamList.Add(new SqlParameter("@PIRASSN_PROG", pirassn2Entity.PIRASSN_PROG));
                if (pirassn2Entity.PIRASSN_YEAR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_YEAR", pirassn2Entity.PIRASSN_YEAR));
                if (pirassn2Entity.PIRASSN_Q_FUND != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_Q_FUND", pirassn2Entity.PIRASSN_Q_FUND));
                if (pirassn2Entity.PIRASSN_Q_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_Q_ID", pirassn2Entity.PIRASSN_Q_ID));
                if (pirassn2Entity.PIRASSN_GRP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_GRP", pirassn2Entity.PIRASSN_GRP));
                if (pirassn2Entity.PIRASSN_SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_SEQ", pirassn2Entity.PIRASSN_SEQ));
                if (pirassn2Entity.PIRASSN_COL_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_COL_ID", pirassn2Entity.PIRASSN_COL_ID));
                if (pirassn2Entity.PIRASSN_Q_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_Q_TYPE", pirassn2Entity.PIRASSN_Q_TYPE));
                if (pirassn2Entity.PIRASSN_CONJ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_CONJ", pirassn2Entity.PIRASSN_CONJ));
                if (pirassn2Entity.PIRASSN_TASK != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_TASK", pirassn2Entity.PIRASSN_TASK));
                if (pirassn2Entity.PIRASSN_YEAR_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_YEAR_TYPE", pirassn2Entity.PIRASSN_YEAR_TYPE));
                if (pirassn2Entity.PIRASSN_CHK_RESP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_CHK_RESP", pirassn2Entity.PIRASSN_CHK_RESP));
                if (pirassn2Entity.PIRASSN_RESPONSE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_RESPONSE", pirassn2Entity.PIRASSN_RESPONSE));
                if (pirassn2Entity.PIRASSN_DATE_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_DATE_TYPE", pirassn2Entity.PIRASSN_DATE_TYPE));
                if (pirassn2Entity.PIRASSN_FDATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_FDATE", pirassn2Entity.PIRASSN_FDATE));
                if (pirassn2Entity.PIRASSN_TDATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_TDATE", pirassn2Entity.PIRASSN_TDATE));
                if (pirassn2Entity.PIRASSN_CHK_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_CHK_DATE", pirassn2Entity.PIRASSN_CHK_DATE));
                if (pirassn2Entity.PIRASSN_INTAKE_AGYTAB != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_INTAKE_AGYTAB", pirassn2Entity.PIRASSN_INTAKE_AGYTAB));
                if (pirassn2Entity.PIRASSN_INTAKE_CODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_INTAKE_CODE", pirassn2Entity.PIRASSN_INTAKE_CODE));
                if (pirassn2Entity.PIRASSN_SERVICE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_SERVICE", pirassn2Entity.PIRASSN_SERVICE));
                if (pirassn2Entity.PIRASSN_CUSTOM_CODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_CUSTOM_CODE", pirassn2Entity.PIRASSN_CUSTOM_CODE));
                if (pirassn2Entity.PIRASSN_CUSTOM_RESP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_CUSTOM_RESP", pirassn2Entity.PIRASSN_CUSTOM_RESP));
                if (pirassn2Entity.PIRASSN_CUSTOM_SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_CUSTOM_SEQ", pirassn2Entity.PIRASSN_CUSTOM_SEQ));
                //if (pirassn2Entity.PIRASSN_DATE_ADD != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@PIRASSN_DATE_ADD", pirassn2Entity.PIRASSN_DATE_ADD));
                if (pirassn2Entity.PIRASSN_ADD_OPERATOR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PIRASSN_ADD_OPERATOR", pirassn2Entity.PIRASSN_ADD_OPERATOR));
                //if (pirassn2Entity.PIRASSN_DATE_LSTC != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@PIRASSN_DATE_LSTC", pirassn2Entity.PIRASSN_DATE_LSTC));
                if (pirassn2Entity.PIRASSN_LSTC_OPERATOR != string.Empty)
                sqlParamList.Add(new SqlParameter("@PIRASSN_LSTC_OPERATOR", pirassn2Entity.PIRASSN_LSTC_OPERATOR));

                sqlParamList.Add(new SqlParameter("@Mode", pirassn2Entity.Mode));
                Success = Captain.DatabaseLayer.ChldAttnDB.InsertDelPirassn2(sqlParamList);
            }
            catch
            {
                Success = false;
            }

            return Success;
        }

        public List<Pirassn2Entity> GetPirassnData2(string Agency, string Dept, string Program, string Year)
        {
            List<Pirassn2Entity> pirassn2 = new List<Pirassn2Entity>();
            try
            {
                DataSet PirWorkData = Captain.DatabaseLayer.ChldAttnDB.GetPirassnData2(Agency, Dept, Program, Year);
                if (PirWorkData != null && PirWorkData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in PirWorkData.Tables[0].Rows)
                    {
                        pirassn2.Add(new Pirassn2Entity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return pirassn2;
            }

            return pirassn2;
        }
    }
}
