using Captain.Common.Model.Objects;
using Captain.DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace Captain.Common.Model.Data
{
    public class AdhocData
    {

        public AdhocData()
        {

        }

        #region Properties

        public CaptainModel Model { get; set; }

        #endregion

        public List<CaseHierarchyEntity> Browse_CASEHIE(string Agency, string Dept, string Prog,string UserID,string AdminAgency)
        {
            List<CaseHierarchyEntity> CASEHIEProfile = new List<CaseHierarchyEntity>();
            try
            {
                DataSet AdhocColumnData = ADMNB001DB.ADMNB001_GetCashie(Agency + "-" + Dept + "-" + Prog,UserID, AdminAgency,string.Empty);
                if (AdhocColumnData != null && AdhocColumnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in AdhocColumnData.Tables[0].Rows)
                        CASEHIEProfile.Add(new CaseHierarchyEntity(row));
                }
            }
            catch (Exception ex)
            {
                return CASEHIEProfile;
            }

            return CASEHIEProfile;
        }


        public List<AdhocTableEntity> Get_Tables_List(string DataBase_Name, string Operation)
        {
            List<AdhocTableEntity> AdhocTableProfile = new List<AdhocTableEntity>();
            try
            {
                DataSet AdhocTableData = AdhocDB.Adhoc_Get_Tables_Columns(DataBase_Name, Operation);
                if (AdhocTableData != null && AdhocTableData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in AdhocTableData.Tables[0].Rows)
                        AdhocTableProfile.Add(new AdhocTableEntity(row));
                }
            }
            catch (Exception ex)
            {
                return AdhocTableProfile;
            }

            return AdhocTableProfile;
        }


        public List<AdhocColumnEntity> Get_Columns_List(string DataBase_Name, string Operation)
        {
            List<AdhocColumnEntity> AdhocColumnProfile = new List<AdhocColumnEntity>();
            try
            {
                DataSet AdhocColumnData = AdhocDB.Adhoc_Get_Tables_Columns(DataBase_Name, Operation);
                if (AdhocColumnData != null && AdhocColumnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in AdhocColumnData.Tables[0].Rows)
                        AdhocColumnProfile.Add(new AdhocColumnEntity(row));
                }
            }
            catch (Exception ex)
            {
                return AdhocColumnProfile;
            }

            return AdhocColumnProfile;
        }

        public DataSet Get_SelCol_Data(string XML_Data, string Secret_SW, string Group_Sort_SW, string Include_Mambers, string Seledted_Hierarchy, string XML_Summary_Data, string Addcust_Sw, string Card1_Xml, string Module_Code, string User_Id, string Data_Filter, string Category_Code)  //(string App_Key, string XML_Data)
        {

            DataSet ds = new DataSet();
            try
            {
                ds = AdhocDB.Get_SelCol_Data(XML_Data, Secret_SW, Group_Sort_SW, Include_Mambers, Seledted_Hierarchy, XML_Summary_Data, Addcust_Sw, Card1_Xml, Module_Code, User_Id, Data_Filter, Category_Code); //(App_Key, XML_Data);
            }
            catch (Exception ex)
            {
                return ds;
            }

            return ds;
        }

        public DataSet Generate_CAMS_Work_File(string Agy, string Dept, string Prog, string Year, string Date_SW, string From_Date, string To_Date, string User_Name, string session_ID, string IP)  //(string App_Key, string XML_Data)
        {

            DataSet ds = new DataSet();
            try
            {
                ds = AdhocDB.Generate_CAMS_Work_File(Agy, Dept, Prog, Year, Date_SW, From_Date, To_Date, User_Name, session_ID, IP); //(App_Key, XML_Data);
            }
            catch (Exception ex)
            {
                return ds;
            }

            return ds;
        }

        public List<ADHOCFLDEntity> Browse_ADHOCFLD(ADHOCFLDEntity Entity, string Module)
        {
            List<ADHOCFLDEntity> CASESPMProfile = new List<ADHOCFLDEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_ADDTOTFLD_SqlParameters_List(Entity, Module);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_ADHOCFLD]");
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASESPM(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new ADHOCFLDEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }


        public List<SqlParameter> Prepare_ADDTOTFLD_SqlParameters_List(ADHOCFLDEntity Entity, string Module)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@Module_Code", Module));
                sqlParamList.Add(new SqlParameter("@ADHOCFLD_CODE", Entity.Col_Code));
                sqlParamList.Add(new SqlParameter("@ADHOCFLD_NAME", Entity.Col_Disp_Name));
                sqlParamList.Add(new SqlParameter("@ADHOCFLD_FLD_TYPE", Entity.Col_Format_Type));
                sqlParamList.Add(new SqlParameter("@ADHOCFLD_RESP_TYPE", Entity.Col_Resp_Type));
                sqlParamList.Add(new SqlParameter("@ADHOCFLD_AGYCODE", Entity.Col_AgyCode));
                sqlParamList.Add(new SqlParameter("@ADHOCFLD_ACTIVE", Entity.Active));
                sqlParamList.Add(new SqlParameter("@ADHOCFLD_FIELD", Entity.Act_Col_Name));
                sqlParamList.Add(new SqlParameter("@ADHOCFLD_DESC_LEN", Entity.Col_Desc_Length));
                sqlParamList.Add(new SqlParameter("@ADHOCFLD_CODE_LEN", Entity.Col_Code_Length));
                sqlParamList.Add(new SqlParameter("@ADHOCFLD_CRITERIA", Entity.Have_Criteria));
                sqlParamList.Add(new SqlParameter("@ADHOCFLD_COUNTABLE", Entity.Have_Count));

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }


        public List<ADHOCFLSEntity> Browse_ADHOCFLS(ADHOCFLSEntity Entity)
        {
            List<ADHOCFLSEntity> CASESPMProfile = new List<ADHOCFLSEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_ADHOCFLS_SqlParameters_List(Entity);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_ADHOCFLS]");
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASESPM(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new ADHOCFLSEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<SqlParameter> Prepare_ADHOCFLS_SqlParameters_List(ADHOCFLSEntity Entity)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@ADHOCFLS_CATEGORY", Entity.Category));
                sqlParamList.Add(new SqlParameter("@ADHOCFLS_MODULE", Entity.Module));
                sqlParamList.Add(new SqlParameter("@ADHOCFLS_CODE", Entity.Table_Code));
                sqlParamList.Add(new SqlParameter("@ADHOCFLS_NAME", Entity.Table_Name));
                sqlParamList.Add(new SqlParameter("@ADHOCFLS_DESCRIPTION", Entity.Table_Desc));
                sqlParamList.Add(new SqlParameter("@ADHOCFLS_ACTIVE", Entity.Active_Stat));

                sqlParamList.Add(new SqlParameter("@ADHOCFLS_PRIMARY_LEN", Entity.Primary_Key_Length));
                sqlParamList.Add(new SqlParameter("@ADHOCFLS_Column_Prefix", Entity.Column_Perfix));
                sqlParamList.Add(new SqlParameter("@ADHOCFLS_PREVKEY_LEN", Entity.Priv_Table_Key_Length));
                sqlParamList.Add(new SqlParameter("@ADHOCFLS_ASOC_TABLES", Entity.Assoc_Tables));
                sqlParamList.Add(new SqlParameter("@ADHOCFLS_PRIMARY_KEY", Entity.Primary_Key));
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<ADHOCCTGEntity> Browse_ADHOCCTG(ADHOCCTGEntity Entity)
        {
            List<ADHOCCTGEntity> ADHOCCTGProfile = new List<ADHOCCTGEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_ADHOCCTG_SqlParameters_List(Entity);
                DataSet ADHOCCTGData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_ADHOCCTG]");
                if (ADHOCCTGData != null && ADHOCCTGData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ADHOCCTGData.Tables[0].Rows)
                        ADHOCCTGProfile.Add(new ADHOCCTGEntity(row));
                }
            }
            catch (Exception ex)
            { return ADHOCCTGProfile; }

            return ADHOCCTGProfile;
        }

        public List<SqlParameter> Prepare_ADHOCCTG_SqlParameters_List(ADHOCCTGEntity Entity)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@ADHOCCTG_MODULE", Entity.Module));
                sqlParamList.Add(new SqlParameter("@ADHOCCTG_CODE", Entity.Catg_Code));
                sqlParamList.Add(new SqlParameter("@ADHOCCTG_DESC", Entity.Catg_Desc));
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }


        public List<AGYTABSEntity> Browse_AGYTABS(AGYTABSEntity Entity)
        {
            List<AGYTABSEntity> AGYTABSProfile = new List<AGYTABSEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_AGYTABS_SqlParameters_List(Entity);
                DataSet AGYTABSData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_AGYTABS]");

                if (AGYTABSData != null && AGYTABSData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in AGYTABSData.Tables[0].Rows)
                        AGYTABSProfile.Add(new AGYTABSEntity(row));
                }
            }
            catch (Exception ex)
            { return AGYTABSProfile; }

            return AGYTABSProfile;
        }




        public DataSet Rep_MAT0002_MatAssessments(List<SqlParameter> sqlParamList, string Module)
        {
            DataSet Return_Table_Set = new DataSet();
            List<AGYTABSEntity> AGYTABSProfile = new List<AGYTABSEntity>();
            //List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                //sqlParamList = Prepare_AGYTABS_SqlParameters_List(Entity);

                //Return_Table_Set = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Rep_MAT0002_MatAssessments]");
                if(Module == "03")
                    Return_Table_Set = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Rep_MAT0002_MatAssessments_New]");
                else if(Module == "02")
                    Return_Table_Set = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Rep_MAT0002_MatAssessments_HS]");
                


                //if (AGYTABSData != null && AGYTABSData.Tables[4].Rows.Count > 0)
                //{
                //    Return_Table = AGYTABSData.Tables[4];
                ////    foreach (DataRow row in AGYTABSData.Tables[0].Rows)
                ////        AGYTABSProfile.Add(new AGYTABSEntity(row));
                //}
            }
            catch (Exception ex)
            { return Return_Table_Set; }

            return Return_Table_Set;
        }

        public DataSet Rep_MAT1002_MatAssessments(List<SqlParameter> sqlParamList, string Module)
        {
            DataSet Return_Table_Set = new DataSet();
            List<AGYTABSEntity> AGYTABSProfile = new List<AGYTABSEntity>();
            //List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                //sqlParamList = Prepare_AGYTABS_SqlParameters_List(Entity);

                
                //if (Module == "03")
                    Return_Table_Set = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Rep_MAT1002_MatAssessments_New]");
                //else if (Module == "02")
                //    Return_Table_Set = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Rep_MAT0002_MatAssessments_HS]");



                //if (AGYTABSData != null && AGYTABSData.Tables[4].Rows.Count > 0)
                //{
                //    Return_Table = AGYTABSData.Tables[4];
                ////    foreach (DataRow row in AGYTABSData.Tables[0].Rows)
                ////        AGYTABSProfile.Add(new AGYTABSEntity(row));
                //}
            }
            catch (Exception ex)
            { return Return_Table_Set; }

            return Return_Table_Set;
        }

        public List<MATB0003Entity> MAT0003_MatAssessments_report(List<SqlParameter> sqlParamList)
        {
            DataSet Return_Table_Set = new DataSet();
            List<MATB0003Entity> CASESP0Profile = new List<MATB0003Entity>();
            //List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                //sqlParamList = Prepare_AGYTABS_SqlParameters_List(Entity);

                //Return_Table_Set = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Rep_MAT0002_MatAssessments]");

                Return_Table_Set = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[MATB0003_REPORT]");
                if (Return_Table_Set != null && Return_Table_Set.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in Return_Table_Set.Tables[0].Rows)
                        CASESP0Profile.Add(new MATB0003Entity(row,string.Empty));
                }
                //else if (Module == "02")
                //    Return_Table_Set = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Rep_MAT0002_MatAssessments_HS]");



                //if (AGYTABSData != null && AGYTABSData.Tables[4].Rows.Count > 0)
                //{
                //    Return_Table = AGYTABSData.Tables[4];
                ////    foreach (DataRow row in AGYTABSData.Tables[0].Rows)
                ////        AGYTABSProfile.Add(new AGYTABSEntity(row));
                //}
            }
            catch (Exception ex)
            { return CASESP0Profile; }

            return CASESP0Profile;
        }

        public List<SqlParameter> Prepare_AGYTABS_SqlParameters_List(AGYTABSEntity Entity)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@AGYS_TYPE", Entity.Tabs_Type));
                sqlParamList.Add(new SqlParameter("@AGYS_CODE", Entity.Table_Code));
                sqlParamList.Add(new SqlParameter("@AGYS_DESC", Entity.Code_Desc));
                sqlParamList.Add(new SqlParameter("@AGYS_Active", Entity.Active));
                sqlParamList.Add(new SqlParameter("@AGYS_Default", Entity.Default));
                sqlParamList.Add(new SqlParameter("@AGYS_Hierarchy", Entity.Hierarchy));
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }


        public bool UpdateControl_Card(ControlCard_Entity Card_Entity, out string Sql_Reslut_Msg)
        {
            bool boolsuccess = true;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Row_Type", Card_Entity.Rowtype));
                sqlParamList.Add(new SqlParameter("@CCF_PROG_NAME", Card_Entity.Scr_Code));
                sqlParamList.Add(new SqlParameter("@CCF_EMP_CODE", Card_Entity.UserID));
                sqlParamList.Add(new SqlParameter("@CCF_ID", Card_Entity.Card_ID));
                sqlParamList.Add(new SqlParameter("@CCF_DESC", Card_Entity.Card_DESC));
                sqlParamList.Add(new SqlParameter("@CCF_CONTROL_CARD_1", Card_Entity.Card_1));
                sqlParamList.Add(new SqlParameter("@CCF_CONTROL_CARD_2", Card_Entity.Card_2));
                sqlParamList.Add(new SqlParameter("@CCF_CONTROL_CARD_3", Card_Entity.Card_3));
                sqlParamList.Add(new SqlParameter("@CCF_MODULE", Card_Entity.Module));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateCONTROLCARD", out Sql_Reslut_Msg);  //

                //Sql_Reslut_Msg = SP_Result_Msg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<ControlCard_Entity> Browse_CONTROLCARD(ControlCard_Entity Card_Entity, string Operation)//, out string Sql_Reslut_Msg)
        {
            List<ControlCard_Entity> CASESP0Profile = new List<ControlCard_Entity>();
            //Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@CCF_PROG_NAME", Card_Entity.Scr_Code));
                sqlParamList.Add(new SqlParameter("@CCF_EMP_CODE", Card_Entity.UserID));
                sqlParamList.Add(new SqlParameter("@CCF_ID", Card_Entity.Card_ID));
                sqlParamList.Add(new SqlParameter("@CCF_DESC", Card_Entity.Card_DESC));
                sqlParamList.Add(new SqlParameter("@CCF_MODULE", Card_Entity.Module));
                sqlParamList.Add(new SqlParameter("@CCF_DATE_ADD", Card_Entity.ADD_Date));
                sqlParamList.Add(new SqlParameter("@CCF_DATE_LSTC", Card_Entity.LSTC_Date));

                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CONTROLCARD]");
                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESP0Profile.Add(new ControlCard_Entity(row));
                }
            }
            catch (Exception ex)
            { return CASESP0Profile; }

            return CASESP0Profile;
        }

        public List<Adhoc_ADDCUSTEntity> Adhoc_Get_ADDCUST_Ques_BYHie(string Agy, string Dept, string Prog)//, out string Sql_Reslut_Msg)
        {
            List<Adhoc_ADDCUSTEntity> CASESP0Profile = new List<Adhoc_ADDCUSTEntity>();
            //Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Agy", Agy));
                sqlParamList.Add(new SqlParameter("@Dept", Dept));
                sqlParamList.Add(new SqlParameter("@Prog", Prog));

                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Adhoc_Get_ADDCUST_Ques_BYHie]");
                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESP0Profile.Add(new Adhoc_ADDCUSTEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESP0Profile; }

            return CASESP0Profile;
        }

        //Added by sudheer on 01/07/2016
        public List<Adhoc_ADDCUSTEntity> Adhoc_Get_PREASSES_Ques_BYHie(string Agy, string Dept, string Prog)//, out string Sql_Reslut_Msg)
        {
            List<Adhoc_ADDCUSTEntity> CASESP0Profile = new List<Adhoc_ADDCUSTEntity>();
            //Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Agy", Agy));
                sqlParamList.Add(new SqlParameter("@Dept", Dept));
                sqlParamList.Add(new SqlParameter("@Prog", Prog));

                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Adhoc_Get_PREASSES_Ques_BYHie]");
                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESP0Profile.Add(new Adhoc_ADDCUSTEntity(row,string.Empty));
                }
            }
            catch (Exception ex)
            { return CASESP0Profile; }

            return CASESP0Profile;
        }

        //Added by sudheer on 05/24/2017
        public List<Adhoc_ADDCUSTEntity> Adhoc_Get_SERCUST_Ques_BYHie(string Agy, string Dept, string Prog)//, out string Sql_Reslut_Msg)
        {
            List<Adhoc_ADDCUSTEntity> CASESP0Profile = new List<Adhoc_ADDCUSTEntity>();
            //Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Agy", Agy));
                sqlParamList.Add(new SqlParameter("@Dept", Dept));
                sqlParamList.Add(new SqlParameter("@Prog", Prog));

                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Adhoc_Get_SERCUST_Ques_BYHie]");
                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESP0Profile.Add(new Adhoc_ADDCUSTEntity(row, "SERCUST"));
                }
            }
            catch (Exception ex)
            { return CASESP0Profile; }

            return CASESP0Profile;
        }


        public List<AdhocAssoc_Entity> Browse_ADHOCASSOC(AdhocAssoc_Entity Card_Entity, string Operation)//, out string Sql_Reslut_Msg)
        {
            List<AdhocAssoc_Entity> CASESP0Profile = new List<AdhocAssoc_Entity>();
            //Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@ADHASSOC_ID", Card_Entity.Adh_ID));
                sqlParamList.Add(new SqlParameter("@ADHASSOC_ID_USER", Card_Entity.Adh_ID_UserID));
                sqlParamList.Add(new SqlParameter("@ADHASSOC_DESC", Card_Entity.Adh_Assc_Desc));
                sqlParamList.Add(new SqlParameter("@ADHASSOC_USER", Card_Entity.Adh_Assc_UserID));
                sqlParamList.Add(new SqlParameter("@ADHASSOC_ADD_DATE", Card_Entity.Adh_Assc_AddDate));
                sqlParamList.Add(new SqlParameter("@ADHASSOC_ADD_OPERATOR", Card_Entity.Adh_Assc_AddOpr));
                sqlParamList.Add(new SqlParameter("@ADHASSOC_MODULE", Card_Entity.Adh_Assc_Module));

                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_ADHOCASSOC]");
                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESP0Profile.Add(new AdhocAssoc_Entity(row));
                }
            }
            catch (Exception ex)
            { return CASESP0Profile; }

            return CASESP0Profile;
        }

        public bool UpdateADHOCASSOC(AdhocAssoc_Entity Entity, string Xml_Users, string Del_All_Assoc, out string Sql_Reslut_Msg)
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {

                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rowtype));
                sqlParamList.Add(new SqlParameter("@ADHASSOC_ID", Entity.Adh_ID));
                sqlParamList.Add(new SqlParameter("@ADHASSOC_ID_USER", Entity.Adh_ID_UserID));
                sqlParamList.Add(new SqlParameter("@ADHASSOC_DESC", Entity.Adh_Assc_Desc));
                sqlParamList.Add(new SqlParameter("@ADHASSOC_USER", Entity.Adh_Assc_UserID));
                sqlParamList.Add(new SqlParameter("@ADHASSOC_ADD_OPERATOR", Entity.Adh_Assc_AddOpr));
                sqlParamList.Add(new SqlParameter("@ADHASSOC_MODULE", Entity.Adh_Assc_Module));
                sqlParamList.Add(new SqlParameter("@Users_XML", Xml_Users));
                sqlParamList.Add(new SqlParameter("@Del_All_Assoc", Del_All_Assoc));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateADHOCASSOC", out Sql_Reslut_Msg);  //
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }


        public List<AdhocAssoc_Entity> Get_DemoGraphics_Counts(AdhocAssoc_Entity Card_Entity, string Operation)//, out string Sql_Reslut_Msg)
        {
            List<AdhocAssoc_Entity> CASESP0Profile = new List<AdhocAssoc_Entity>();
            //Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@ADHASSOC_ID", Card_Entity.Adh_ID));
                sqlParamList.Add(new SqlParameter("@ADHASSOC_ID_USER", Card_Entity.Adh_ID_UserID));
                sqlParamList.Add(new SqlParameter("@ADHASSOC_DESC", Card_Entity.Adh_Assc_Desc));
                sqlParamList.Add(new SqlParameter("@ADHASSOC_USER", Card_Entity.Adh_Assc_UserID));
                sqlParamList.Add(new SqlParameter("@ADHASSOC_ADD_DATE", Card_Entity.Adh_Assc_AddDate));
                sqlParamList.Add(new SqlParameter("@ADHASSOC_ADD_OPERATOR", Card_Entity.Adh_Assc_AddOpr));
                sqlParamList.Add(new SqlParameter("@ADHASSOC_MODULE", Card_Entity.Adh_Assc_Module));

                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_ADHOCASSOC]");
                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESP0Profile.Add(new AdhocAssoc_Entity(row));
                }
            }
            catch (Exception ex)
            { return CASESP0Profile; }

            return CASESP0Profile;
        }

        public DataSet Get_RNGDG_Counts(DG_Browse_Entity Entity, string Details_SW, string Alert_Codes)  //(string App_Key, string XML_Data)
        {

            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Attribute", Entity.Attribute));
                sqlParamList.Add(new SqlParameter("@Ms_DriveColumn_Sw", Entity.Ms_DriveColumn_Sw));
                if(!string.IsNullOrEmpty(Entity.Rep_From_Date.Trim()))
                    sqlParamList.Add(new SqlParameter("@Rep_From_Date", Entity.Rep_From_Date));
                if (!string.IsNullOrEmpty(Entity.Rep_To_Date.Trim()))
                    sqlParamList.Add(new SqlParameter("@Rep_To_Date", Entity.Rep_To_Date));
                if (!string.IsNullOrEmpty(Entity.Rep_Period_FDate.Trim()))
                    sqlParamList.Add(new SqlParameter("@Rep_Period_FDate", Entity.Rep_Period_FDate));
                if (!string.IsNullOrEmpty(Entity.Rep_Period_TDate.Trim()))
                    sqlParamList.Add(new SqlParameter("@Rep_Period_TDate", Entity.Rep_Period_TDate));
                sqlParamList.Add(new SqlParameter("@Mst_Secret_Sw", Entity.Mst_Secret_Sw));
                sqlParamList.Add(new SqlParameter("@Mst_CaseType_Sw", Entity.Mst_CaseType_Sw));
                sqlParamList.Add(new SqlParameter("@Mst_Acive_Sw", Entity.Mst_Acive_Sw));
                sqlParamList.Add(new SqlParameter("@Mst_Site", Entity.Mst_Site));
                sqlParamList.Add(new SqlParameter("@Mst_Poverty_Low", Entity.Mst_Poverty_Low));
                sqlParamList.Add(new SqlParameter("@Mst_Poverty_High", Entity.Mst_Poverty_High));
                sqlParamList.Add(new SqlParameter("@DG_Count_Sw", Entity.DG_Count_Sw));
                sqlParamList.Add(new SqlParameter("@ZipCode", Entity.ZipCode));
                sqlParamList.Add(new SqlParameter("@County", Entity.County));
                sqlParamList.Add(new SqlParameter("@Hierarchy", Entity.Hierarchy));
                sqlParamList.Add(new SqlParameter("@UserName", Entity.UserName));
                sqlParamList.Add(new SqlParameter("@Inc_Bypass_Details", Details_SW));
                sqlParamList.Add(new SqlParameter("@CAMS_Filter_Codes", Entity.CAMS_Filter));
                sqlParamList.Add(new SqlParameter("@CaseMs_Site", Entity.CaseMssite));
                sqlParamList.Add(new SqlParameter("@Activity_Prog", Entity.Activity_Prog));
                sqlParamList.Add(new SqlParameter("@DateType", Entity.DateType));

                if (!string.IsNullOrEmpty(Alert_Codes.Trim()))
                    sqlParamList.Add(new SqlParameter("@AlertCodes", Alert_Codes));

                string SP_To_Execute = string.Empty;
                if (Entity.CA_MS_Sw == "MS")
                {
                    switch (Entity.DG_Count_Sw)
                    {
                        case "SNP":
                            {
                                switch (Details_SW)
                                {
                                    case "Y":
                                        if (Entity.Attribute == "SYSTEM")
                                            SP_To_Execute = "dbo.RNGDemographics_On_SNP_AgyDef";
                                        else
                                            SP_To_Execute = "dbo.RNGDemographics_On_SNP_UserDef";
                                        break;
                                    case "N":
                                        if (Entity.Attribute == "SYSTEM")
                                            SP_To_Execute = "dbo.RNGDemographics_On_SNP_AgyDef_Count";
                                        else
                                            SP_To_Execute = "dbo.RNGDemographics_On_SNP_UserDef_Count";
                                        break;
                                }
                            }
                            break;

                        case "OBO":
                            {
                                switch (Details_SW)
                                {
                                    case "Y":
                                        if (Entity.Attribute == "SYSTEM")
                                            SP_To_Execute = "dbo.RNGDemographics_On_OBO_AgyDef";
                                        else
                                            SP_To_Execute = "dbo.RNGDemographics_On_OBO_UserDef";
                                        break;
                                    case "N":
                                        if (Entity.Attribute == "SYSTEM")
                                            SP_To_Execute = "dbo.RNGDemographics_On_OBO_AgyDef_Count";
                                        else
                                            SP_To_Execute = "dbo.RNGDemographics_On_OBO_UserDef_Count";
                                        break;
                                }
                            }
                            break;
                    }
                }
                else if (Entity.CA_MS_Sw == "BOTH" || Entity.CA_MS_Sw == "Both")
                {
                    switch (Entity.DG_Count_Sw)
                    {
                        case "SNP":
                            {
                                switch (Details_SW)
                                {
                                    case "Y":
                                        if (Entity.Attribute == "SYSTEM")
                                            SP_To_Execute = "dbo.RNGDemographics_BOTH_CA_On_SNP_AgyDef";
                                        else
                                            SP_To_Execute = "dbo.RNGDemographics_BOTH_CA_On_SNP_UserDef";
                                        break;
                                    case "N":
                                        if (Entity.Attribute == "SYSTEM")
                                            SP_To_Execute = "dbo.RNGDemographics_BOTH_CA_On_SNP_AgyDef_Count";
                                        else
                                            SP_To_Execute = "dbo.RNGDemographics_BOTH_CA_On_SNP_UserDef_Count";
                                        break;
                                }
                            }
                            break;

                        case "OBO":
                            {
                                switch (Details_SW)
                                {
                                    case "Y":
                                        if (Entity.Attribute == "SYSTEM")
                                            SP_To_Execute = "dbo.RNGDemographics_OBO_CA_On_SNP_AgyDef";
                                        else
                                            SP_To_Execute = "dbo.RNGDemographics_OBO_CA_On_SNP_UserDef";
                                        break;
                                    case "N":
                                        if (Entity.Attribute == "SYSTEM")
                                            SP_To_Execute = "dbo.RNGDemographics_OBO_CA_On_SNP_AgyDef_Count";
                                        else
                                            SP_To_Execute = "dbo.RNGDemographics_OBO_CA_On_SNP_UserDef_Count";
                                        break;
                                }
                            }
                            break;
                    }
                    //if (SP_To_Execute.Contains("CA_On_SNP_"))
                    //    sqlParamList.Add(new SqlParameter("@Fund_Filter", Entity.CA_Fund_Filter));
                }
                else
                {
                    switch (Entity.DG_Count_Sw)
                    {
                        case "SNP":
                            {
                                switch (Details_SW)
                                {
                                    case "Y":
                                        if (Entity.Attribute == "SYSTEM")
                                            SP_To_Execute = "dbo.RNGDemographics_On_SNP_AgyDefCA";
                                        else
                                            SP_To_Execute = "dbo.RNGDemographics_On_SNP_UserDefCA";
                                        break;
                                    case "N":
                                        if (Entity.Attribute == "SYSTEM")
                                            SP_To_Execute = "dbo.RNGDemographics_On_SNP_AgyDef_CountCA";
                                        else
                                            SP_To_Execute = "dbo.RNGDemographics_On_SNP_UserDef_CountCA";
                                        break;
                                }
                            }
                            break;

                        case "OBO":
                            {
                                switch (Details_SW)
                                {
                                    case "Y":
                                        if (Entity.Attribute == "SYSTEM")
                                            SP_To_Execute = "dbo.RNGDemographics_CA_On_SNP_AgyDef";
                                        else
                                            SP_To_Execute = "dbo.RNGDemographics_CA_On_SNP_UserDef";
                                        break;
                                    case "N":
                                        if (Entity.Attribute == "SYSTEM")
                                            SP_To_Execute = "dbo.RNGDemographics_CA_On_SNP_AgyDef_Count";
                                        else
                                            SP_To_Execute = "dbo.RNGDemographics_CA_On_SNP_UserDef_Count";
                                        break;
                                }
                            }
                            break;
                    }

                    //if (SP_To_Execute.Contains("CA_On_SNP_"))
                    //    sqlParamList.Add(new SqlParameter("@Fund_Filter", Entity.CA_Fund_Filter));
                }
                if (Entity.CA_Fund_Filter != "**")
                    sqlParamList.Add(new SqlParameter("@Fund_Filter", Entity.CA_Fund_Filter));


                //else if (Entity.CAMS_Filter != "**")
                //    sqlParamList.Add(new SqlParameter("@Fund_Filter", Entity.CAMS_Filter));


                //else
                //{
                //    switch (Details_SW)
                //    {
                //        case "Y":
                //            if (Entity.Attribute == "SYSTEM")
                //                SP_To_Execute = "dbo.RNGDemographics_CA_On_SNP_AgyDef";
                //            else
                //                SP_To_Execute = "dbo.RNGDemographics_CA_On_SNP_UserDef";
                //            break;
                //        case "N":
                //            if (Entity.Attribute == "SYSTEM")
                //                SP_To_Execute = "dbo.RNGDemographics_CA_On_SNP_AgyDef_Count";
                //            else
                //                SP_To_Execute = "dbo.RNGDemographics_CA_On_SNP_UserDef_Count";
                //            break;
                //    }

                //    if (SP_To_Execute.Contains("CA_On_SNP_"))
                //        sqlParamList.Add(new SqlParameter("@Fund_Filter", Entity.CA_Fund_Filter));

                //}

                //ds = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, Entity.DG_Count_Sw == "SNP" ? "dbo.CaseDemographics_SP_Test" : "dbo.CaseDemographics_SP_Test_OBO");  //
                ds = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, SP_To_Execute);  //
            }
            catch (Exception ex)
            {
                return ds;
            }

            return ds;
        }


        public DataSet Get_DG_Counts(DG_Browse_Entity Entity, string Details_SW)  //(string App_Key, string XML_Data)
        {

            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Attribute", Entity.Attribute));
                sqlParamList.Add(new SqlParameter("@Ms_DriveColumn_Sw", Entity.Ms_DriveColumn_Sw));
                sqlParamList.Add(new SqlParameter("@Rep_From_Date", Entity.Rep_From_Date));
                sqlParamList.Add(new SqlParameter("@Rep_To_Date", Entity.Rep_To_Date));
                sqlParamList.Add(new SqlParameter("@Rep_Period_FDate", Entity.Rep_Period_FDate));
                sqlParamList.Add(new SqlParameter("@Rep_Period_TDate", Entity.Rep_Period_TDate));
                sqlParamList.Add(new SqlParameter("@Mst_Secret_Sw", Entity.Mst_Secret_Sw));
                sqlParamList.Add(new SqlParameter("@Mst_CaseType_Sw", Entity.Mst_CaseType_Sw));
                sqlParamList.Add(new SqlParameter("@Mst_Acive_Sw", Entity.Mst_Acive_Sw));
                sqlParamList.Add(new SqlParameter("@Mst_Site", Entity.Mst_Site));
                sqlParamList.Add(new SqlParameter("@Mst_Poverty_Low", Entity.Mst_Poverty_Low));
                sqlParamList.Add(new SqlParameter("@Mst_Poverty_High", Entity.Mst_Poverty_High));
                sqlParamList.Add(new SqlParameter("@DG_Count_Sw", Entity.DG_Count_Sw));
                sqlParamList.Add(new SqlParameter("@ZipCode", Entity.ZipCode));
                sqlParamList.Add(new SqlParameter("@County", Entity.County));
                sqlParamList.Add(new SqlParameter("@Hierarchy", Entity.Hierarchy));
                sqlParamList.Add(new SqlParameter("@UserName", Entity.UserName));
                sqlParamList.Add(new SqlParameter("@Inc_Bypass_Details", Details_SW));
                sqlParamList.Add(new SqlParameter("@CAMS_Filter_Codes", Entity.CAMS_Filter));

                string SP_To_Execute = string.Empty;
                if (Entity.CA_MS_Sw == "MS")
                {
                    switch (Entity.DG_Count_Sw)
                    {
                        case "SNP":
                            {
                                switch (Details_SW)
                                {
                                    case "Y":
                                        if (Entity.Attribute == "SYSTEM")
                                            SP_To_Execute = "dbo.CaseDemographics_On_SNP_AgyDef";
                                        else
                                            SP_To_Execute = "dbo.CaseDemographics_On_SNP_UserDef";
                                        break;
                                    case "N":
                                        if (Entity.Attribute == "SYSTEM")
                                            SP_To_Execute = "dbo.CaseDemographics_On_SNP_AgyDef_Count";
                                        else
                                            SP_To_Execute = "dbo.CaseDemographics_On_SNP_UserDef_Count";
                                        break;
                                }
                            }
                            break;

                        case "OBO":
                            {
                                switch (Details_SW)
                                {
                                    case "Y":
                                        if (Entity.Attribute == "SYSTEM")
                                            SP_To_Execute = "dbo.CaseDemographics_On_OBO_AgyDef";
                                        else
                                            SP_To_Execute = "dbo.CaseDemographics_On_OBO_UserDef";
                                        break;
                                    case "N":
                                        if (Entity.Attribute == "SYSTEM")
                                            SP_To_Execute = "dbo.CaseDemographics_On_OBO_AgyDef_Count";
                                        else
                                            SP_To_Execute = "dbo.CaseDemographics_On_OBO_UserDef_Count";
                                        break;
                                }
                            }
                            break;
                    }
                }
                else
                {
                    switch (Details_SW)
                    {
                        case "Y":
                            if (Entity.Attribute == "SYSTEM")
                                SP_To_Execute = "dbo.CaseDemographics_CA_On_SNP_AgyDef";
                            else
                                SP_To_Execute = "dbo.CaseDemographics_CA_On_SNP_UserDef";
                            break;
                        case "N":
                            if (Entity.Attribute == "SYSTEM")
                                SP_To_Execute = "dbo.CaseDemographics_CA_On_SNP_AgyDef_Count";
                            else
                                SP_To_Execute = "dbo.CaseDemographics_CA_On_SNP_UserDef_Count";
                            break;
                    }

                    if (SP_To_Execute.Contains("CA_On_SNP_"))
                        sqlParamList.Add(new SqlParameter("@Fund_Filter", Entity.CA_Fund_Filter));

                }

                //ds = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, Entity.DG_Count_Sw == "SNP" ? "dbo.CaseDemographics_SP_Test" : "dbo.CaseDemographics_SP_Test_OBO");  //
                ds = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, SP_To_Execute);  //
            }
            catch (Exception ex)
            {
                return ds;
            }

            return ds;
        }


        public DataSet Get_PM_Counts(DG_Browse_Entity Entity, string Details_SW)  //(string App_Key, string XML_Data)
        {

            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Category", Entity.Attribute));
                sqlParamList.Add(new SqlParameter("@Ms_DriveColumn_Sw", Entity.Ms_DriveColumn_Sw));
                sqlParamList.Add(new SqlParameter("@Ref_From_Date", Entity.Rep_From_Date));
                sqlParamList.Add(new SqlParameter("@Ref_To_Date", Entity.Rep_To_Date));
                sqlParamList.Add(new SqlParameter("@Rep_Period_FDate", Entity.Rep_Period_FDate));
                sqlParamList.Add(new SqlParameter("@Rep_Period_TDate", Entity.Rep_Period_TDate));
                sqlParamList.Add(new SqlParameter("@Mst_Secret_Sw", Entity.Mst_Secret_Sw));
                sqlParamList.Add(new SqlParameter("@Mst_CaseType_Sw", Entity.Mst_CaseType_Sw));
                sqlParamList.Add(new SqlParameter("@Mst_Acive_Sw", Entity.Mst_Acive_Sw));
                sqlParamList.Add(new SqlParameter("@Mst_Site", Entity.Mst_Site));
                sqlParamList.Add(new SqlParameter("@Mst_Poverty_Low", Entity.Mst_Poverty_Low));
                sqlParamList.Add(new SqlParameter("@Mst_Poverty_High", Entity.Mst_Poverty_High));
                sqlParamList.Add(new SqlParameter("@Rep_Format", Entity.PM_Rep_Format));
                sqlParamList.Add(new SqlParameter("@Groups", Entity.ZipCode));
                sqlParamList.Add(new SqlParameter("@Hierarchy", Entity.Hierarchy));
                sqlParamList.Add(new SqlParameter("@Activity_Prog", Entity.Activity_Prog));
                sqlParamList.Add(new SqlParameter("@UserName", Entity.UserName));
                sqlParamList.Add(new SqlParameter("@PM_Details_SW", Details_SW));
                sqlParamList.Add(new SqlParameter("@County", Entity.County));
                

                ds = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "dbo.Get_PerMeasures_Counts");  //
            }
            catch (Exception ex)
            {
                return ds;
            }

            return ds;
        }


        public DataSet Get_RNGPM_Counts(DG_Browse_Entity Entity, string Details_SW, string strCode, string Agy,string strReportswitch,string strFromDate,string strToDate,string strReportControlSwitch,string strUndupTable,string RepType, string Alertcodes,string PPR_SW)  //(string App_Key, string XML_Data)
        {

            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Category", Entity.Attribute));
                sqlParamList.Add(new SqlParameter("@Ms_DriveColumn_Sw", Entity.Ms_DriveColumn_Sw));
                sqlParamList.Add(new SqlParameter("@Ref_From_Date", Entity.Rep_From_Date));
                sqlParamList.Add(new SqlParameter("@Ref_To_Date", Entity.Rep_To_Date));
                sqlParamList.Add(new SqlParameter("@Rep_Period_FDate", Entity.Rep_Period_FDate));
                sqlParamList.Add(new SqlParameter("@Rep_Period_TDate", Entity.Rep_Period_TDate));
                sqlParamList.Add(new SqlParameter("@Mst_Secret_Sw", Entity.Mst_Secret_Sw));
                sqlParamList.Add(new SqlParameter("@Mst_CaseType_Sw", Entity.Mst_CaseType_Sw));
                sqlParamList.Add(new SqlParameter("@Mst_Acive_Sw", Entity.Mst_Acive_Sw));
                sqlParamList.Add(new SqlParameter("@Mst_Site", Entity.Mst_Site));
                sqlParamList.Add(new SqlParameter("@Mst_Poverty_Low", Entity.Mst_Poverty_Low));
                sqlParamList.Add(new SqlParameter("@Mst_Poverty_High", Entity.Mst_Poverty_High));
                sqlParamList.Add(new SqlParameter("@Rep_Format", Entity.PM_Rep_Format));
                sqlParamList.Add(new SqlParameter("@Groups", Entity.ZipCode));
                sqlParamList.Add(new SqlParameter("@Hierarchy", Entity.Hierarchy));
                sqlParamList.Add(new SqlParameter("@Activity_Prog", Entity.Activity_Prog));
                sqlParamList.Add(new SqlParameter("@UserName", Entity.UserName));
                sqlParamList.Add(new SqlParameter("@PM_Details_SW", Details_SW));
                sqlParamList.Add(new SqlParameter("@County", Entity.County));
                sqlParamList.Add(new SqlParameter("@Agency", Agy));
                sqlParamList.Add(new SqlParameter("@RngMainCode", strCode));                
                sqlParamList.Add(new SqlParameter("@RngOutcomeSwitch", Entity.OutComeSwitch));
                sqlParamList.Add(new SqlParameter("@CaseMs_Site", Entity.CaseMssite));
                sqlParamList.Add(new SqlParameter("@ReportSwitch", strReportswitch));
                sqlParamList.Add(new SqlParameter("@IncVerSwitch", Entity.IncVerSwitch));
                sqlParamList.Add(new SqlParameter("@Rep_CPeriod_FDate", strFromDate));
                sqlParamList.Add(new SqlParameter("@Rep_CPeriod_TDate", strToDate));
                sqlParamList.Add(new SqlParameter("@ReportControl", strReportControlSwitch));

                sqlParamList.Add(new SqlParameter("@UndupTableCounts", strUndupTable));
                // murali added by fund filter 13 NOV 2020
                sqlParamList.Add(new SqlParameter("@Fund_Filter", Entity.CA_Fund_Filter));

                if(!string.IsNullOrEmpty(RepType.Trim()))
                    sqlParamList.Add(new SqlParameter("@RepType", RepType));

                if (Alertcodes == "")
                    Alertcodes = "**";

                sqlParamList.Add(new SqlParameter("@AlertCodes", Alertcodes)); // Added by Vikash on 01/25/2024 as part of Outcome enhancement doc

                if (PPR_SW == "Y")
                    ds = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "dbo.Get_RNGPerMeasures_Counts_SP");
                else
                    ds = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "dbo.Get_RNGPerMeasures_Counts");  //
          }
            catch (Exception ex)
            {
                return ds;
            }

            return ds;
        }

        public DataSet Get_RNGB0005_Counts(SO_Browse_Entity Entity, string Details_SW, string strCode, string Agy, string strReportswitch, string strFromDate, string strToDate, string strReportControlSwitch, string strUndupTable, string RepType, string RepPeriodCntl)  //(string App_Key, string XML_Data)
        {

            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                //sqlParamList.Add(new SqlParameter("@Category", Entity.Attribute));
                sqlParamList.Add(new SqlParameter("@Ms_DriveColumn_Sw", Entity.Ms_DriveColumn_Sw));
                
                sqlParamList.Add(new SqlParameter("@RngMainCode", strCode));

                sqlParamList.Add(new SqlParameter("@Ref_From_Date", Entity.Rep_Period_FDate));
                sqlParamList.Add(new SqlParameter("@Ref_To_Date", Entity.Rep_Period_TDate));

                sqlParamList.Add(new SqlParameter("@Q1SDATE", Entity.Q1_START_DATE));
                sqlParamList.Add(new SqlParameter("@Q1EDATE", Entity.Q1_END_DATE));
                sqlParamList.Add(new SqlParameter("@Q2SDATE", Entity.Q2_START_DATE));
                sqlParamList.Add(new SqlParameter("@Q2EDATE", Entity.Q2_END_DATE));
                sqlParamList.Add(new SqlParameter("@Q3SDATE", Entity.Q3_START_DATE));
                sqlParamList.Add(new SqlParameter("@Q3EDATE", Entity.Q3_END_DATE));
                sqlParamList.Add(new SqlParameter("@Q4SDATE", Entity.Q4_START_DATE));
                sqlParamList.Add(new SqlParameter("@Q4EDATE", Entity.Q4_END_DATE));


                sqlParamList.Add(new SqlParameter("@Mst_Acive_Sw", Entity.Mst_Acive_Sw));
                sqlParamList.Add(new SqlParameter("@Mst_CaseType_Sw", Entity.Mst_CaseType_Sw));

                sqlParamList.Add(new SqlParameter("@ServicePlans", Entity.ZipCode));

                //sqlParamList.Add(new SqlParameter("@Ref_From_Date", Entity.Rep_From_Date));
                //sqlParamList.Add(new SqlParameter("@Ref_To_Date", Entity.Rep_To_Date));
                sqlParamList.Add(new SqlParameter("@Mst_Secret_Sw", Entity.Mst_Secret_Sw));


                //sqlParamList.Add(new SqlParameter("@Mst_Site", Entity.Mst_Site));
                sqlParamList.Add(new SqlParameter("@Mst_Poverty_Low", Entity.Mst_Poverty_Low));
                sqlParamList.Add(new SqlParameter("@Mst_Poverty_High", Entity.Mst_Poverty_High));
                //sqlParamList.Add(new SqlParameter("@Rep_Format", Entity.PM_Rep_Format));
                
                sqlParamList.Add(new SqlParameter("@Hierarchy", Entity.Hierarchy));
                //sqlParamList.Add(new SqlParameter("@Activity_Prog", Entity.Activity_Prog));
                sqlParamList.Add(new SqlParameter("@UserName", Entity.UserName));
                //sqlParamList.Add(new SqlParameter("@PM_Details_SW", Details_SW));
                sqlParamList.Add(new SqlParameter("@County", Entity.County));
                sqlParamList.Add(new SqlParameter("@Agency", Agy));

                //sqlParamList.Add(new SqlParameter("@RngOutcomeSwitch", Entity.OutComeSwitch));
                //sqlParamList.Add(new SqlParameter("@CaseMs_Site", Entity.CaseMssite));
                //sqlParamList.Add(new SqlParameter("@ReportSwitch", strReportswitch));
                //sqlParamList.Add(new SqlParameter("@IncVerSwitch", Entity.IncVerSwitch));
                //sqlParamList.Add(new SqlParameter("@Rep_CPeriod_FDate", strFromDate));
                //sqlParamList.Add(new SqlParameter("@Rep_CPeriod_TDate", strToDate));
                //sqlParamList.Add(new SqlParameter("@ReportControl", strReportControlSwitch));

                //sqlParamList.Add(new SqlParameter("@UndupTableCounts", strUndupTable));
                //// murali added by fund filter 13 NOV 2020
                //sqlParamList.Add(new SqlParameter("@Fund_Filter", Entity.CA_Fund_Filter));

                if (!string.IsNullOrEmpty(RepType.Trim()))
                    sqlParamList.Add(new SqlParameter("@CountSw", RepType));

                if (!string.IsNullOrEmpty(RepPeriodCntl.Trim()))
                    sqlParamList.Add(new SqlParameter("@RepPerdCntl", RepPeriodCntl));

                sqlParamList.Add(new SqlParameter("@Results", Entity.Results));

                ds = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "dbo.CAPS_RNGB0005_Report");  //
            }
            catch (Exception ex)
            {
                return ds;
            }

            return ds;
        }

        public DataSet Get_SRRNGPM_Counts(DG_Browse_Entity Entity, string Details_SW, string strCode, string Agy,string strReportSwitch,string strReportControl,string strUndupTable,string PPR_SW)  //(string App_Key, string XML_Data)
        {

            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Category", Entity.Attribute));
                sqlParamList.Add(new SqlParameter("@Ms_DriveColumn_Sw", Entity.Ms_DriveColumn_Sw));
                sqlParamList.Add(new SqlParameter("@Ref_From_Date", Entity.Rep_From_Date));
                sqlParamList.Add(new SqlParameter("@Ref_To_Date", Entity.Rep_To_Date));
                sqlParamList.Add(new SqlParameter("@Rep_Period_FDate", Entity.Rep_Period_FDate));
                sqlParamList.Add(new SqlParameter("@Rep_Period_TDate", Entity.Rep_Period_TDate));
                sqlParamList.Add(new SqlParameter("@Mst_Secret_Sw", Entity.Mst_Secret_Sw));
                sqlParamList.Add(new SqlParameter("@Mst_CaseType_Sw", Entity.Mst_CaseType_Sw));
                sqlParamList.Add(new SqlParameter("@Mst_Acive_Sw", Entity.Mst_Acive_Sw));
                sqlParamList.Add(new SqlParameter("@Mst_Site", Entity.Mst_Site));
                sqlParamList.Add(new SqlParameter("@Mst_Poverty_Low", Entity.Mst_Poverty_Low));
                sqlParamList.Add(new SqlParameter("@Mst_Poverty_High", Entity.Mst_Poverty_High));
                sqlParamList.Add(new SqlParameter("@Rep_Format", Entity.PM_Rep_Format));
                sqlParamList.Add(new SqlParameter("@Groups", Entity.ZipCode));
                sqlParamList.Add(new SqlParameter("@Hierarchy", Entity.Hierarchy));
                sqlParamList.Add(new SqlParameter("@Activity_Prog", Entity.Activity_Prog));
                sqlParamList.Add(new SqlParameter("@UserName", Entity.UserName));
                sqlParamList.Add(new SqlParameter("@PM_Details_SW", Details_SW));
                sqlParamList.Add(new SqlParameter("@County", Entity.County));
                sqlParamList.Add(new SqlParameter("@RngMainCode", strCode));
                sqlParamList.Add(new SqlParameter("@Agency", Agy));
                //sqlParamList.Add(new SqlParameter("@RngServiceSwitch", Entity.OutComeSwitch));
                sqlParamList.Add(new SqlParameter("@CaseMs_Site", Entity.CaseMssite));
                sqlParamList.Add(new SqlParameter("@IncVerSwitch", Entity.IncVerSwitch));
                sqlParamList.Add(new SqlParameter("@Fund_Filter", Entity.CA_Fund_Filter));
                sqlParamList.Add(new SqlParameter("@ReportSwitch", strReportSwitch));
                sqlParamList.Add(new SqlParameter("@ReportControl", strReportControl));

                sqlParamList.Add(new SqlParameter("@UndupTableCounts", strUndupTable));

                sqlParamList.Add(new SqlParameter("@AlertCodes", Entity.alertCodes));

                if(PPR_SW=="Y")
                    ds = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "dbo.Get_RNGSERPerMeasures_Counts_SP");
                else
                    ds = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "dbo.Get_RNGSERPerMeasures_Counts");  //

            }
            catch (Exception ex)
            {
                return ds;
            }

            return ds;
        }



        public DataSet TMSB000_ActSum_GetCounts(string Agy, string Dept, string Prog, string Year, string Rep_Type, string Category, string From_Date, string To_Date)
        {
            DataSet CASESPMData = new DataSet();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Agy", Agy));
                sqlParamList.Add(new SqlParameter("@Dept", Dept));
                sqlParamList.Add(new SqlParameter("@Prog", Prog));
                sqlParamList.Add(new SqlParameter("@Year", Year));
                sqlParamList.Add(new SqlParameter("@Rep_Type", Rep_Type));
                sqlParamList.Add(new SqlParameter("@Rep_Category", Category));
                sqlParamList.Add(new SqlParameter("@Rep_From_Intake", From_Date));
                sqlParamList.Add(new SqlParameter("@Rep_To_Intake", To_Date));

                if (Rep_Type == "ACT")
                    CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[TMSB000_ActSum_GetCounts]");
                else if (Rep_Type == "WF1")
                    CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[TMSB0000_Weekly_Financial1]");
                else if (Rep_Type == "WF2")
                    CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[TMSB0000_Weekly_Financial2]");
                //else
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASESPM(sqlParamList);
            }
            catch (Exception ex)
            { return CASESPMData; }

            return CASESPMData;
        }

        public DataSet TMSB000_ActSum_Reports(string Agy, string Dept, string Prog, string Year, string From_Date, string To_Date, string SEAP)
        {
            DataSet CASESPMData = new DataSet();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Agy", Agy));
                sqlParamList.Add(new SqlParameter("@Dept", Dept));
                sqlParamList.Add(new SqlParameter("@Prog", Prog));
                sqlParamList.Add(new SqlParameter("@Year", Year));
                sqlParamList.Add(new SqlParameter("@Rep_From_Intake", From_Date));
                sqlParamList.Add(new SqlParameter("@Rep_To_Intake", To_Date));
                sqlParamList.Add(new SqlParameter("@Rep_SEAP_Sw", SEAP));
                //sqlParamList.Add(new SqlParameter("@Rep_Type", Rep_Type));
                //sqlParamList.Add(new SqlParameter("@Rep_Category", Category));
                //if (Rep_Type == "ACT")
                //    CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[TMSB000_ActSum_GetCounts]");
                //else if (Rep_Type == "WF1")
                //    CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[TMSB0000_Weekly_Financial1]");
                //else if (Rep_Type == "WF2")
                //    CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[TMSB0000_Weekly_Financial2]");
                //else
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASESPM(sqlParamList);
                CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[TMSB0000_REPORT]");
            }
            catch (Exception ex)
            { return CASESPMData; }

            return CASESPMData;
        }

        public DataSet TMSB000_ActSum_Diff_Audit(string Agy, string Dept, string Prog, string Year, string Ben_Type, string Ben_Category_Code, string Rep_Type)
        {
            DataSet CASESPMData = new DataSet();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Agy", Agy));
                sqlParamList.Add(new SqlParameter("@Dept", Dept));
                sqlParamList.Add(new SqlParameter("@Prog", Prog));
                sqlParamList.Add(new SqlParameter("@Year", Year));
                sqlParamList.Add(new SqlParameter("@Rep_LpbType", Ben_Type));
                sqlParamList.Add(new SqlParameter("@Rep_Level", Ben_Category_Code));
                sqlParamList.Add(new SqlParameter("@Rep_Type", Rep_Type));

                if (Rep_Type == "WF1")
                    CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[TMSB0000_Weekly_Financial1_B1DIff]");
                //else if (Rep_Type == "WF1")
                //    CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[TMSB0000_Weekly_Financial1]");
                //else if (Rep_Type == "WF2")
                //    CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[TMSB0000_Weekly_Financial2]");
                //else
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASESPM(sqlParamList);
            }
            catch (Exception ex)
            { return CASESPMData; }

            return CASESPMData;
        }

        public bool InsertReportLog(ReportLogEntity replogEntity)
        {
            bool boolsuccess = true;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (replogEntity.REP_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@REP_ID", replogEntity.REP_ID));
                sqlParamList.Add(new SqlParameter("@REP_PROG_NAME", replogEntity.REP_PROG_NAME));
                sqlParamList.Add(new SqlParameter("@REP_EMP_CODE", replogEntity.REP_EMP_CODE));
                sqlParamList.Add(new SqlParameter("@REP_DATA", replogEntity.REP_DATA));
                sqlParamList.Add(new SqlParameter("@REP_FILE_NAME", replogEntity.REP_FILE_NAME));
                sqlParamList.Add(new SqlParameter("@REP_MODULE_CODE", replogEntity.REP_MODULE_CODE));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsertReportLog(sqlParamList);


            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<ReportLogEntity> GetReportLog(string ProgName, string EmpCode, string Modulecode, string date)
        {
            List<ReportLogEntity> ReportLog = new List<ReportLogEntity>();
            //Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (ProgName != string.Empty)
                    sqlParamList.Add(new SqlParameter("@REP_PROG_NAME", ProgName));
                if (EmpCode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@REP_EMP_CODE", EmpCode));
                if (Modulecode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@REP_MODULE_CODE", Modulecode));
                if (date != string.Empty)
                    sqlParamList.Add(new SqlParameter("@REP_DATE_ADD", date));



                DataSet LogData = Captain.DatabaseLayer.SPAdminDB.GetReportLog(sqlParamList);
                if (LogData != null && LogData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in LogData.Tables[0].Rows)
                        ReportLog.Add(new ReportLogEntity(row));
                }
            }
            catch (Exception ex)
            { return ReportLog; }

            return ReportLog;
        }

        public List<Client_InqTotal_entity> Get_ClientInq_Totals(string agy, string dept, string Prog, string Year, string App)
        {
            List<Client_InqTotal_entity> ADHOCCTGProfile = new List<Client_InqTotal_entity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@Agency", agy));
                sqlParamList.Add(new SqlParameter("@Dept", dept));
                sqlParamList.Add(new SqlParameter("@Prog", Prog));
                sqlParamList.Add(new SqlParameter("@Year", Year));
                sqlParamList.Add(new SqlParameter("@App", App));

                DataSet ADHOCCTGData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Get_ClientInq_Totals]");
                if (ADHOCCTGData != null && ADHOCCTGData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ADHOCCTGData.Tables[0].Rows)
                        ADHOCCTGProfile.Add(new Client_InqTotal_entity(row));
                }
            }
            catch (Exception ex)
            { return ADHOCCTGProfile; }

            return ADHOCCTGProfile;
        }



        public bool InsertUpdateDelAdhocHistory(string strEmployeeid,string strModulecode,string strHierchy,string strCategory,string strMode,string strScreenName,string strParameters,string strID,out string strOutId)
        {
            bool boolsuccess = true;
        
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@ADHHIS_EMP_CODE", strEmployeeid));
                sqlParamList.Add(new SqlParameter("@ADHHIS_MODULE_CODE", strModulecode));
                sqlParamList.Add(new SqlParameter("@ADHHIS_HIERCHY", strHierchy));
                sqlParamList.Add(new SqlParameter("@ADHHIS_CATEGORY", strCategory));
                sqlParamList.Add(new SqlParameter("@ADH_SCREEN_NAME", strScreenName));
                sqlParamList.Add(new SqlParameter("@ADHHIS_PARAMETERS", strParameters));
                if(strID !=string.Empty)
                    sqlParamList.Add(new SqlParameter("@ADHHIS_ID", strID));

                sqlParamList.Add(new SqlParameter("@Mode", strMode));

                SqlParameter parameterMsg = new SqlParameter("@OutADHHIS_ID", SqlDbType.VarChar, 10);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);
               


                boolsuccess = Captain.DatabaseLayer.SPAdminDB.INSERTUPDATEDELADHOCHISTORY(sqlParamList);  //

                strOutId = parameterMsg.Value.ToString();
               
            }
            catch (Exception ex)
            {
                strOutId = string.Empty;
                boolsuccess = false;

            }

            return boolsuccess;
        }

        public DataSet TMSB0028_Get(string Agy, string Dept, string Prog, string Year, string strreporttype, string From_Date, string To_Date)  // Added from VWG Form by Vikash on 04/05/2023 for CEAP Applications by Submission Type Report
        {
            DataSet Dsdata = new DataSet();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(Agy))
                    sqlParamList.Add(new SqlParameter("@Agy", Agy));
                if (!string.IsNullOrEmpty(Dept))
                    sqlParamList.Add(new SqlParameter("@Dept", Dept));
                if (!string.IsNullOrEmpty(Prog))
                    sqlParamList.Add(new SqlParameter("@Prog", Prog));
                if (!string.IsNullOrEmpty(Year))
                    sqlParamList.Add(new SqlParameter("@Year", Year));
                if (strreporttype != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Rep_Type", strreporttype));
                if (!string.IsNullOrEmpty(From_Date))
                    sqlParamList.Add(new SqlParameter("@Rep_From_Intake", From_Date));
                if (!string.IsNullOrEmpty(To_Date))
                    sqlParamList.Add(new SqlParameter("@Rep_To_Intake", To_Date));


                Dsdata = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[CAPS_TMSB0028_GET]");
            }
            catch (Exception ex)
            { return Dsdata; }

            return Dsdata;
        }

        public DataSet TMSB0029_Get(string Agy, string Dept, string Prog, string Year, string strreporttype, string From_Date, string To_Date)
        {
            DataSet Dsdata = new DataSet();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(Agy))
                    sqlParamList.Add(new SqlParameter("@Agy", Agy));
                if (!string.IsNullOrEmpty(Dept))
                    sqlParamList.Add(new SqlParameter("@Dept", Dept));
                if (!string.IsNullOrEmpty(Prog))
                    sqlParamList.Add(new SqlParameter("@Prog", Prog));
                if (!string.IsNullOrEmpty(Year))
                    sqlParamList.Add(new SqlParameter("@Year", Year));
                if (strreporttype != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Rep_Type", strreporttype));
                if (!string.IsNullOrEmpty(From_Date))
                    sqlParamList.Add(new SqlParameter("@StartDate", From_Date));
                if (!string.IsNullOrEmpty(To_Date))
                    sqlParamList.Add(new SqlParameter("@EndDate", To_Date));


                Dsdata = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[CAPS_TMSB0029_GET]");
            }
            catch (Exception ex)
            { return Dsdata; }

            return Dsdata;
        }

        //Added by Vikash on 02/26/2024 for new SEAP Report
        public DataSet TMSB100_Report(string Agy, string Dept, string Prog, string Year, string strreporttype,string strCatCode, string From_Date, string To_Date)
        {
            DataSet Dsdata = new DataSet();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(Agy))
                    sqlParamList.Add(new SqlParameter("@Agy", Agy));
                if (!string.IsNullOrEmpty(Dept))
                    sqlParamList.Add(new SqlParameter("@Dept", Dept));
                if (!string.IsNullOrEmpty(Prog))
                    sqlParamList.Add(new SqlParameter("@Prog", Prog));
                if (!string.IsNullOrEmpty(Year))
                    sqlParamList.Add(new SqlParameter("@Year", Year));
                if (strreporttype != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Rep_Type", strreporttype));
                if (strCatCode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Rep_Category", strCatCode));
                if (!string.IsNullOrEmpty(From_Date))
                    sqlParamList.Add(new SqlParameter("@Rep_From_Intake", From_Date));
                if (!string.IsNullOrEmpty(To_Date))
                    sqlParamList.Add(new SqlParameter("@Rep_To_Intake", To_Date));

                Dsdata = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[TMSB1000_REPORT]");
            }
            catch (Exception ex)
            {
                return Dsdata;
            }

            return Dsdata;
        }

    }
}
