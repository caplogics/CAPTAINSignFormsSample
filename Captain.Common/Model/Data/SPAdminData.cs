using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.DatabaseLayer;
using Captain.Common.Model.Objects;
using System.Data.SqlClient;
using System.Data;
using Captain.Common.Utilities;


namespace Captain.Common.Model.Data
{
    public class SPAdminData
    {

        public SPAdminData()
        {

        }

        #region Properties

        public CaptainModel Model { get; set; }

        #endregion


        public List<CABUNDLEENTITY> Browse_CABUNDLE(CABUNDLEENTITY Entity, string Opretaion_Mode)
        {
            List<CABUNDLEENTITY> CASESPMProfile = new List<CABUNDLEENTITY>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_Bundle_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[CAPS_CABUNDLE_BROWSE]");
                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new CABUNDLEENTITY(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<SqlParameter> Prepare_Bundle_SqlParameters_List(CABUNDLEENTITY Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));
                }

                if (!string.IsNullOrEmpty(Entity.CAB_AGENCY))
                    sqlParamList.Add(new SqlParameter("@CAB_AGENCY", Entity.CAB_AGENCY));
                if (!string.IsNullOrEmpty(Entity.CAB_DEPT))
                    sqlParamList.Add(new SqlParameter("@CAB_DEPT", Entity.CAB_DEPT));
                if (!string.IsNullOrEmpty(Entity.CAB_PROGRAM))
                    sqlParamList.Add(new SqlParameter("@CAB_PROGRAM", Entity.CAB_PROGRAM));
                if (!string.IsNullOrEmpty(Entity.CAB_YEAR))
                    sqlParamList.Add(new SqlParameter("@CAB_YEAR", Entity.CAB_YEAR));
                if (!string.IsNullOrEmpty(Entity.CAB_BUNDLE))
                    sqlParamList.Add(new SqlParameter("@CAB_BUNDLE", Entity.CAB_BUNDLE));
                if (!string.IsNullOrEmpty(Entity.CAB_STATUS))
                    sqlParamList.Add(new SqlParameter("@CAB_STATUS", Entity.CAB_STATUS));
                if (!string.IsNullOrEmpty(Entity.CAB_DATE_OPENED))
                    sqlParamList.Add(new SqlParameter("@CAB_DATE_OPENED", Entity.CAB_DATE_OPENED));
                if (!string.IsNullOrEmpty(Entity.CAB_APP_COUNT))
                    sqlParamList.Add(new SqlParameter("@CAB_APP_COUNT", Entity.CAB_APP_COUNT));
                if (!string.IsNullOrEmpty(Entity.CAB_TOT_AMOUNT))
                    sqlParamList.Add(new SqlParameter("@CAB_TOT_AMOUNT", Entity.CAB_TOT_AMOUNT));
                if (!string.IsNullOrEmpty(Entity.CAB_FUND))
                    sqlParamList.Add(new SqlParameter("@CAB_FUND", Entity.CAB_FUND));
                if (!string.IsNullOrEmpty(Entity.CAB_VENDOR))
                    sqlParamList.Add(new SqlParameter("@CAB_VENDOR", Entity.CAB_VENDOR));
                if (!string.IsNullOrEmpty(Entity.CAB_CHK_NO))
                    sqlParamList.Add(new SqlParameter("@CAB_CHK_NO", Entity.CAB_CHK_NO));
                if (!string.IsNullOrEmpty(Entity.CAB_CHK_DATE))
                    sqlParamList.Add(new SqlParameter("@CAB_CHK_DATE", Entity.CAB_CHK_DATE));
                if (!string.IsNullOrEmpty(Entity.CAB_VOUCH_NO))
                    sqlParamList.Add(new SqlParameter("@CAB_VOUCH_NO", Entity.CAB_VOUCH_NO));
                if (!string.IsNullOrEmpty(Entity.CAB_PAY_NUMBER))
                    sqlParamList.Add(new SqlParameter("@CAB_PAY_NUMBER", Entity.CAB_PAY_NUMBER));

                if (!string.IsNullOrEmpty(Entity.CAB_LSTC_OPERATOR))
                    sqlParamList.Add(new SqlParameter("@CAB_LSTC_OPERATOR", Entity.CAB_LSTC_OPERATOR));


                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@CAB_DATE_LSTC", Entity.CAB_DATE_LSTC));

                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }
        public List<CASESP2Entity> Browse_CASESP2(string SP_code, string BR_Code, string CAMS_Type, string CA_MC_Code)
        {
            List<CASESP2Entity> CASESP2Profile = new List<CASESP2Entity>();
            try
            {
                DataSet CASESP2Data = SPAdminDB.Browse_CASESP2(SP_code, BR_Code, CAMS_Type, CA_MC_Code);
                if (CASESP2Data != null && CASESP2Data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESP2Data.Tables[0].Rows)
                        CASESP2Profile.Add(new CASESP2Entity(row));
                }
            }
            catch (Exception ex)
            { return CASESP2Profile; }

            return CASESP2Profile;
        }
        public List<CASESP2Entity> Browse_CASESP2_forSMATCH(string SP_code, string BR_Code, string CAMS_Type, string CA_MC_Code)
        {
            List<CASESP2Entity> CASESP2Profile = new List<CASESP2Entity>();
            try
            {
                DataSet CASESP2Data = SPAdminDB.Browse_CASESP2(SP_code, BR_Code, CAMS_Type, CA_MC_Code);
                if (CASESP2Data != null && CASESP2Data.Tables[0].Rows.Count > 0)
                {
                    DataView view = new DataView(CASESP2Data.Tables[0]);
                    DataTable dtWrkfileAgytabs = view.ToTable(true, "SP2_CAMS_CODE","CAMS_DESC");

                    foreach (DataRow row in CASESP2Data.Tables[0].Rows)
                        CASESP2Profile.Add(new CASESP2Entity(row));
                }
            }
            catch (Exception ex)
            { return CASESP2Profile; }

            return CASESP2Profile;
        }

        public List<CASESP2Entity> Get_CASESP2(string SP_code, string BR_Code, string CAMS_Type, string CA_MC_Code)
        {
            List<CASESP2Entity> CASESP2Profile = new List<CASESP2Entity>();
            try
            {
                DataSet CASESP2Data = SPAdminDB.Get_CASESP2(SP_code, BR_Code, CAMS_Type, CA_MC_Code);
                if (CASESP2Data != null && CASESP2Data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESP2Data.Tables[0].Rows)
                        CASESP2Profile.Add(new CASESP2Entity(row, "Funnel"));
                }
            }
            catch (Exception ex)
            { return CASESP2Profile; }

            return CASESP2Profile;
        }

        public List<CASESP2Entity> Get_CASESP2(string SP_code, string BR_Code, string CAMS_Type, string CA_MC_Code,string strType)
        {
            List<CASESP2Entity> CASESP2Profile = new List<CASESP2Entity>();
            try
            {
                DataSet CASESP2Data = SPAdminDB.Get_CASESP2(SP_code, BR_Code, CAMS_Type, CA_MC_Code);
                if (CASESP2Data != null && CASESP2Data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESP2Data.Tables[0].Rows)
                        CASESP2Profile.Add(new CASESP2Entity(row, strType));
                }
            }
            catch (Exception ex)
            { return CASESP2Profile; }

            return CASESP2Profile;
        }

        public List<CASESP2Entity> Browse_CASESP2(string SP_code, string BR_Code, string CAMS_Type, string CA_MC_Code, string strfiltercode)
        {
            List<CASESP2Entity> CASESP2Profile = new List<CASESP2Entity>();
            try
            {
                DataSet CASESP2Data = SPAdminDB.Browse_CASESP2(SP_code, BR_Code, CAMS_Type, CA_MC_Code, strfiltercode);
                if (CASESP2Data != null && CASESP2Data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESP2Data.Tables[0].Rows)
                        CASESP2Profile.Add(new CASESP2Entity(row, strfiltercode));
                }
            }
            catch (Exception ex)
            { return CASESP2Profile; }

            return CASESP2Profile;
        }


        public CASESP0Entity Browse_CASESP0(string SP_code, string SP_Desc, string BR_Code, string Status, string Validate_SW, string Lstc_Date, string Lstc_Opr, string Add_Date, string Add_Opr)
        {
            CASESP0Entity CASESP0Profile = null;
            try
            {
                DataSet CASESP0Data = SPAdminDB.Browse_CASESP0(SP_code, SP_Desc, BR_Code, Status, Validate_SW, Lstc_Date, Lstc_Opr, Add_Date, Add_Opr);
                if (CASESP0Data != null && CASESP0Data.Tables[0].Rows.Count > 0)
                {
                    CASESP0Profile = new CASESP0Entity(CASESP0Data.Tables[0].Rows[0]);
                }
            }
            catch (Exception ex)
            { return CASESP0Profile; }

            return CASESP0Profile;
        }

        public List<CASESP0Entity> Browse_CASESP0List(string SP_code, string SP_Desc, string BR_Code, string Status, string Validate_SW, string Lstc_Date, string Lstc_Opr, string Add_Date, string Add_Opr)
        {
            List<CASESP0Entity> CASESP0Profile = new List<CASESP0Entity>();
            try
            {
                DataSet CASESP0Data = SPAdminDB.Browse_CASESP0(SP_code, SP_Desc, BR_Code, Status, Validate_SW, Lstc_Date, Lstc_Opr, Add_Date, Add_Opr);
                if (CASESP0Data != null && CASESP0Data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESP0Data.Tables[0].Rows)
                        CASESP0Profile.Add(new CASESP0Entity(row));
                }
            }
            catch (Exception ex)
            { return CASESP0Profile; }

            return CASESP0Profile;
        }


        public List<CASESP1Entity> Browse_CASESP1(string SP_code, string Agy, string Dept, string Prog,string Mode)
        {
            List<CASESP1Entity> CASESP1Profile = new List<CASESP1Entity>();
            try
            {
                DataSet CASESP1Data = SPAdminDB.Browse_CASESP1(SP_code, Agy, Dept, Prog,Mode);
                if (CASESP1Data != null && CASESP1Data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESP1Data.Tables[0].Rows)
                        CASESP1Profile.Add(new CASESP1Entity(row));
                }
            }
            catch (Exception ex)
            { return CASESP1Profile; }

            return CASESP1Profile;
        }

        public List<CASESP1Entity> CASESP1_SerPlans(string SP_code, string Agy, string Dept, string Prog,string UserID)
        {
            List<CASESP1Entity> CASESP1Profile = new List<CASESP1Entity>();
            try
            {
                DataSet CASESP1Data = SPAdminDB.CASESP1_SerPLANS(SP_code, Agy, Dept, Prog,UserID);
                if (CASESP1Data != null && CASESP1Data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESP1Data.Tables[0].Rows)
                        CASESP1Profile.Add(new CASESP1Entity(row));
                }
            }
            catch (Exception ex)
            { return CASESP1Profile; }

            return CASESP1Profile;
        }

        public List<CTAPPRISEENTITY> BROWSE_CTAPPRISEsurvey(string county, string zipcode, string agency, string dept, string Prog, string year,string FDate,string TDate)
        {
            List<CTAPPRISEENTITY> CASESP1Profile = new List<CTAPPRISEENTITY>();
            try
            {
                DataSet CASESP1Data = SPAdminDB.Browse_CTAPPRISEsurvey(county, zipcode, agency, dept, Prog, year,FDate,TDate);
                if (CASESP1Data != null && CASESP1Data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESP1Data.Tables[0].Rows)
                        CASESP1Profile.Add(new CTAPPRISEENTITY(row));
                }
            }
            catch (Exception ex)
            { return CASESP1Profile; }

            return CASESP1Profile;
        }


        public List<CAMASTEntity> Browse_CAMAST(string Sort_By, string CA_Code, string CA_Desc, string Act_ststus)
        {
            List<CAMASTEntity> CAMASTProfile = new List<CAMASTEntity>();
            try
            {
                DataSet CAMASTData = SPAdminDB.Browse_CAMAST(Sort_By, CA_Code, CA_Desc, Act_ststus);
                if (CAMASTData != null && CAMASTData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CAMASTData.Tables[0].Rows)
                        CAMASTProfile.Add(new CAMASTEntity(row));
                }
            }
            catch (Exception ex)
            { return CAMASTProfile; }

            return CAMASTProfile;
        }

        public List<CAMASTEntity> CAP_SEROUTCOMES(string Agency, string Dept, string Prog, string SP2_Type,string UserId)
        {
            List<CAMASTEntity> CAMASTProfile = new List<CAMASTEntity>();
            try
            {
                DataSet CAMASTData = SPAdminDB.CAP_SEROUTCOMES(Agency, Dept, Prog, SP2_Type, UserId);
                if (CAMASTData != null && CAMASTData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CAMASTData.Tables[0].Rows)
                        CAMASTProfile.Add(new CAMASTEntity(row,string.Empty));
                }
            }
            catch (Exception ex)
            { return CAMASTProfile; }

            return CAMASTProfile;
        }

        public List<MSMASTEntity> CAP_SEROUTCOMES_MS(string Agency, string Dept, string Prog, string SP2_Type, string UserId)
        {
            List<MSMASTEntity> CAMASTProfile = new List<MSMASTEntity>();
            try
            {
                DataSet CAMASTData = SPAdminDB.CAP_SEROUTCOMES(Agency, Dept, Prog, "MS", UserId);
                if (CAMASTData != null && CAMASTData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CAMASTData.Tables[0].Rows)
                        CAMASTProfile.Add(new MSMASTEntity(row,string.Empty,string.Empty));
                }
            }
            catch (Exception ex)
            { return CAMASTProfile; }

            return CAMASTProfile;
        }

        public List<CAPRICESEntity> Browse_CAPrices(string CA_Code, string FDate, string TDate)
        {
            List<CAPRICESEntity> CAMASTProfile = new List<CAPRICESEntity>();
            try
            {
                DataSet CAMASTData = SPAdminDB.Browse_CAPRICES(CA_Code, FDate, TDate);
                if (CAMASTData != null && CAMASTData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CAMASTData.Tables[0].Rows)
                        CAMASTProfile.Add(new CAPRICESEntity(row));
                }
            }
            catch (Exception ex)
            { return CAMASTProfile; }

            return CAMASTProfile;
        }

        public List<Funnel_RepEntity> Browse_FunnelReport(string CAMS_Type, string Agy, string Dept, string Prog, string Year, string SP_Plan, string Code, string Worker, string Branch, string Group, string Enrl_Seq, string Enrl_Hie, string Fund_SW, string strReptSwh, string UserName, string CaseType)
        {
            List<Funnel_RepEntity> CAMASTProfile = new List<Funnel_RepEntity>();
            try
            {
                DataSet CAMASTData = SPAdminDB.GetAppforFunnel(CAMS_Type, Agy, Dept, Prog, Year, SP_Plan, Code, Worker, Branch, Group, Enrl_Seq, Enrl_Hie, Fund_SW, strReptSwh, UserName, CaseType);
                if (CAMASTData != null && CAMASTData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CAMASTData.Tables[0].Rows)
                        CAMASTProfile.Add(new Funnel_RepEntity(row, CAMS_Type));
                }
            }
            catch (Exception ex)
            { return CAMASTProfile; }

            return CAMASTProfile;
        }


        public List<MSMASTEntity> Browse_MSMAST(string Sort_By, string MS_Code, string MS_Desc, string Act_ststus, string Type)
        {
            List<MSMASTEntity> MSMASTProfile = new List<MSMASTEntity>();
            try
            {
                DataSet MSMASTData = SPAdminDB.Browse_MSMAST(Sort_By, MS_Code, MS_Desc, Act_ststus, Type);
                if (MSMASTData != null && MSMASTData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MSMASTData.Tables[0].Rows)
                        MSMASTProfile.Add(new MSMASTEntity(row));
                }
            }
            catch (Exception ex)
            { return MSMASTProfile; }

            return MSMASTProfile;
        }

        //Yeswanth
        public List<CASESPMEntity> Browse_CASESPM(CASESPMEntity Entity, string Opretaion_Mode) //Sindhe
        {
            List<CASESPMEntity> CASESPMProfile = new List<CASESPMEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASESPM_SqlParameters_List(Entity, Opretaion_Mode, null);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CASESPM]");
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASESPM(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new CASESPMEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<CASESPMEntity> Browse_CASESPM(CASESPMEntity Entity, string Opretaion_Mode, string Trigger) //Sindhe
        {
            List<CASESPMEntity> CASESPMProfile = new List<CASESPMEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASESPM_SqlParameters_List(Entity, Opretaion_Mode, null);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CASESPM]");
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASESPM(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new CASESPMEntity(row, Trigger));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<SP_Bulk_Post_Entity> Browse_CASESPM_4Bulk_Posting(CASESPMEntity Entity, string Opretaion_Mode, string App_Not_EqualTo, string File_To_Process)
        {
            List<SP_Bulk_Post_Entity> CASESPMProfile = new List<SP_Bulk_Post_Entity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASESPM_SqlParameters_List(Entity, Opretaion_Mode, App_Not_EqualTo);
                sqlParamList.Add(new SqlParameter("@Sel_Rec_From", File_To_Process));
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CASESPM_4Bulk_Posting]");

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new SP_Bulk_Post_Entity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<SP_Bulk_Post_Entity> Browse_CASESPM_4Bulk_Posting_Latest(CASESPMEntity Entity, string Opretaion_Mode, string App_Not_EqualTo, string File_To_Process, string Source_Posting_List, string Module, string Sel_Site
                                                                             , string CT_Age_SW, string CT_LPB_Type, string CT_LPB_Level, string CT_LPB_Source, string CT_LPB_Chk_Date, bool CT_Trigger, string Attn_1Day, string Attn_FromDate, string Attn_ToDate
                                                                             , string Chk_FDate, string Chk_TDate,string CustCode,string QuesAccess,string SelHH)
        {
            List<SP_Bulk_Post_Entity> CASESPMProfile = new List<SP_Bulk_Post_Entity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASESPM_SqlParameters_List(Entity, Opretaion_Mode, App_Not_EqualTo);
                sqlParamList.Add(new SqlParameter("Sel_Rec_From", File_To_Process));
                sqlParamList.Add(new SqlParameter("@Module", Module));
                sqlParamList.Add(new SqlParameter("@Attn_Site", Sel_Site));

                if (CT_Trigger)
                {
                    sqlParamList.Add(new SqlParameter("@Ct_Age_Sw", CT_Age_SW));
                    sqlParamList.Add(new SqlParameter("@CT_Lpb_Type", CT_LPB_Type));
                    sqlParamList.Add(new SqlParameter("@CT_LPB_Level", CT_LPB_Level));
                    sqlParamList.Add(new SqlParameter("@CT_LPB_Source", CT_LPB_Source));
                    sqlParamList.Add(new SqlParameter("@CT_LPB_Chk_Date", CT_LPB_Chk_Date));

                    sqlParamList.Add(new SqlParameter("@From_Date", Attn_FromDate));
                    sqlParamList.Add(new SqlParameter("@To_Date", Attn_ToDate));
                    sqlParamList.Add(new SqlParameter("@Noof_Days", Attn_1Day));

                    if (!string.IsNullOrEmpty(Chk_FDate.Trim()))
                        sqlParamList.Add(new SqlParameter("@Chk_From_Date", Chk_FDate));
                    if (!string.IsNullOrEmpty(Chk_TDate.Trim()))
                        sqlParamList.Add(new SqlParameter("@Chk_To_Date", Chk_TDate));

                }

                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CASESPM_4Bulk_Posting]");
                if(!string.IsNullOrEmpty(CustCode.Trim()))
                    sqlParamList.Add(new SqlParameter("@CustCode", CustCode));
                if(!string.IsNullOrEmpty(QuesAccess.Trim()))
                    sqlParamList.Add(new SqlParameter("@QuesAccess", QuesAccess));
                if (!string.IsNullOrEmpty(SelHH.Trim()))
                    sqlParamList.Add(new SqlParameter("@SelHH", SelHH));

                sqlParamList.Add(new SqlParameter("@Src_ActMS_XML", Source_Posting_List));
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CASESPM_4Bulk_Posting_Latest]");
                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new SP_Bulk_Post_Entity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<CASECONTEntity> Browse_CASECONT(CASECONTEntity Entity, string Opretaion_Mode)
        {
            List<CASECONTEntity> CASESPMProfile = new List<CASECONTEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASECONT_SqlParameters_List(Entity, string.Empty, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CASECONT]");
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASECONT(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new CASECONTEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }


        public List<CASEACTEntity> Browse_CASEACT(CASEACTEntity Entity, string Opretaion_Mode)
        {
            List<CASEACTEntity> CASEACTProfile = new List<CASEACTEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASEACT_SqlParameters_List(Entity, Opretaion_Mode, string.Empty);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASEACT(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASEACTProfile.Add(new CASEACTEntity(row));
                }
            }
            catch (Exception ex)
            { return CASEACTProfile; }

            return CASEACTProfile;
        }

        public List<CASEACTEntity> Browse_CASEACT(CASEACTEntity Entity, string Opretaion_Mode, string Trigger)
        {
            List<CASEACTEntity> CASEACTProfile = new List<CASEACTEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASEACT_SqlParameters_List(Entity, Opretaion_Mode, Trigger);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASEACT(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASEACTProfile.Add(new CASEACTEntity(row, Trigger));
                }
            }
            catch (Exception ex)
            { return CASEACTProfile; }

            return CASEACTProfile;
        }

        public DataSet Get_CT_Trigger_Report(string Agy, string Dept, string Prog, string Year, string App, string Age_Sw, string Ben_Type, string Ben_Level, string SP_Code)
        {
            DataSet Table_Data = null;
            List<CT_Triggers_Entity> Trig_List = new List<CT_Triggers_Entity>();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Agy", Agy));
                sqlParamList.Add(new SqlParameter("@Dept", Dept));
                sqlParamList.Add(new SqlParameter("@Prog", Prog));
                sqlParamList.Add(new SqlParameter("@Year", Year));
                sqlParamList.Add(new SqlParameter("@App", App));
                sqlParamList.Add(new SqlParameter("@Age_Sw", Age_Sw));
                sqlParamList.Add(new SqlParameter("@Lpb_Type_Sw", Ben_Type));
                sqlParamList.Add(new SqlParameter("@Lpb_Level", Ben_Level));
                //sqlParamList.Add(new SqlParameter("@Lpb_Source", Entity.ScrCode));
                sqlParamList.Add(new SqlParameter("@Sel_SerPlan", SP_Code));

                Table_Data = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Get_CT_Trigger_For_BulkPosting]");

                if (Table_Data != null && Table_Data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in Table_Data.Tables[0].Rows)
                        Trig_List.Add(new CT_Triggers_Entity(row));
                }
            }
            catch (Exception ex)
            { return Table_Data; }

            return Table_Data;
        }

        public DataSet Get_MS_Report(string SortCol, string SelChk, string Date)
        {
            DataSet Table_Data = null;
            List<MSMASTEntity> Trig_List = new List<MSMASTEntity>();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@SortCol", SortCol));
                sqlParamList.Add(new SqlParameter("@SELCHK", SelChk));
                sqlParamList.Add(new SqlParameter("@DATE", Date));
                //sqlParamList.Add(new SqlParameter("@Year", Year));
                //sqlParamList.Add(new SqlParameter("@App", App));
                //sqlParamList.Add(new SqlParameter("@Age_Sw", Age_Sw));
                //sqlParamList.Add(new SqlParameter("@Lpb_Type_Sw", Ben_Type));
                //sqlParamList.Add(new SqlParameter("@Lpb_Level", Ben_Level));
                ////sqlParamList.Add(new SqlParameter("@Lpb_Source", Entity.ScrCode));
                //sqlParamList.Add(new SqlParameter("@Sel_SerPlan", SP_Code));

                Table_Data = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Get_MSMAST]");

                if (Table_Data != null && Table_Data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in Table_Data.Tables[0].Rows)
                        Trig_List.Add(new MSMASTEntity(row));
                }
            }
            catch (Exception ex)
            { return Table_Data; }

            return Table_Data;
        }

        public List<SqlParameter> Prepare_CUSTFlds_SqlParameters_List(CustfldsEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (!string.IsNullOrEmpty(Entity.ScrCode))
                    sqlParamList.Add(new SqlParameter("@CUST_SCR_CODE", Entity.ScrCode));
                if (!string.IsNullOrEmpty(Entity.CustCode))
                    sqlParamList.Add(new SqlParameter("@CUST_CODE", Entity.CustCode));
                if (!string.IsNullOrEmpty(Entity.CustDesc))
                    sqlParamList.Add(new SqlParameter("@CUST_DESC", Entity.CustDesc));
                if (!string.IsNullOrEmpty(Entity.RespType))
                    sqlParamList.Add(new SqlParameter("@CUST_RESP_TYPE", Entity.RespType));
                if (!string.IsNullOrEmpty(Entity.Sub_Screen))
                    sqlParamList.Add(new SqlParameter("@CUST_SUB_SCR", Entity.Sub_Screen));
                if (!string.IsNullOrEmpty(Entity.MemAccess))
                    sqlParamList.Add(new SqlParameter("@CUST_MEM_ACCESS", Entity.MemAccess));
                if (!string.IsNullOrEmpty(Entity.Equalto))
                    sqlParamList.Add(new SqlParameter("@CUST_EQUAL", Entity.Equalto));
                if (!string.IsNullOrEmpty(Entity.Greater))
                    sqlParamList.Add(new SqlParameter("@CUST_GREATER", Entity.Greater));
                if (!string.IsNullOrEmpty(Entity.Less))
                    sqlParamList.Add(new SqlParameter("@CUST_LESSTHAN", Entity.Less));
                if (!string.IsNullOrEmpty(Entity.Alpha))
                    sqlParamList.Add(new SqlParameter("@CUST_ALPHA", Entity.Alpha));
                if (!string.IsNullOrEmpty(Entity.Other))
                    sqlParamList.Add(new SqlParameter("@CUST_OTHER", Entity.Other));
                if (!string.IsNullOrEmpty(Entity.Question))
                    sqlParamList.Add(new SqlParameter("@CUST_ABBR_QUESTION", Entity.Question));
                if (!string.IsNullOrEmpty(Entity.FutureDate))
                    sqlParamList.Add(new SqlParameter("@CUST_ALLOW_FDATE", Entity.FutureDate));
                if (!string.IsNullOrEmpty(Entity.CustSeq))
                    sqlParamList.Add(new SqlParameter("@CUST_SEQ", Entity.CustSeq));
                if (!string.IsNullOrEmpty(Entity.Active))
                    sqlParamList.Add(new SqlParameter("@CUST_ACTIVE", Entity.Active));

                if (!string.IsNullOrEmpty(Entity.ChdOpr))
                    sqlParamList.Add(new SqlParameter("@CUST_LSTC_OPERATOR", Entity.ChdOpr));
                sqlParamList.Add(new SqlParameter("@CUST_DATE_ADD", Entity.AddDate));
                sqlParamList.Add(new SqlParameter("@CUST_ADD_OPERATOR", Entity.AddOpr));
                sqlParamList.Add(new SqlParameter("@CUST_DATE_LSTC", Entity.ChgDate));


            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<CustfldsEntity> Browse_CUSTFLDS(CustfldsEntity Entity, string Opretaion_Mode)
        {
            List<CustfldsEntity> CASEACTProfile = new List<CustfldsEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CUSTFlds_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CUSTFLDS]");
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASECONT(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASEACTProfile.Add(new CustfldsEntity(row));
                }
            }
            catch (Exception ex)
            { return CASEACTProfile; }

            return CASEACTProfile;
        }


        public List<SqlParameter> Prepare_CHLDATTM_SqlParameters_List(SiteScheduleEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Row_Type));

                if (!string.IsNullOrEmpty(Entity.ATTM_AGENCY))
                    sqlParamList.Add(new SqlParameter("@ATTM_AGENCY", Entity.ATTM_AGENCY));
                if (!string.IsNullOrEmpty(Entity.ATTM_DEPT))
                    sqlParamList.Add(new SqlParameter("@ATTM_DEPT", Entity.ATTM_DEPT));
                if (!string.IsNullOrEmpty(Entity.ATTM_PROG))
                    sqlParamList.Add(new SqlParameter("@ATTM_PROG", Entity.ATTM_PROG));
                if (!string.IsNullOrEmpty(Entity.ATTM_YEAR))
                    sqlParamList.Add(new SqlParameter("@ATTM_YEAR", Entity.ATTM_YEAR));
                if (!string.IsNullOrEmpty(Entity.ATTM_MONTH))
                    sqlParamList.Add(new SqlParameter("@ATTM_MONTH", Entity.ATTM_MONTH));
                if (!string.IsNullOrEmpty(Entity.ATTM_SITE))
                    sqlParamList.Add(new SqlParameter("@ATTM_SITE", Entity.ATTM_SITE));
                if (!string.IsNullOrEmpty(Entity.ATTM_ROOM))
                    sqlParamList.Add(new SqlParameter("@ATTM_ROOM", Entity.ATTM_ROOM));
                if (!string.IsNullOrEmpty(Entity.ATTM_AMPM))
                    sqlParamList.Add(new SqlParameter("@ATTM_AMPM", Entity.ATTM_AMPM));
                if (!string.IsNullOrEmpty(Entity.ATTM_FUND))
                    sqlParamList.Add(new SqlParameter("@ATTM_FUND", Entity.ATTM_FUND));
                if (!string.IsNullOrEmpty(Entity.ATTM_CALENDER_YEAR))
                    sqlParamList.Add(new SqlParameter("@ATTM_CALENDER_YEAR", Entity.ATTM_CALENDER_YEAR));
                if (!string.IsNullOrEmpty(Entity.ATTM_ID))
                    sqlParamList.Add(new SqlParameter("@ATTM_ID", Entity.ATTM_ID));

                if (!string.IsNullOrEmpty(Entity.ATTM_LSTC_OPERATOR))
                    sqlParamList.Add(new SqlParameter("@ATTM_LSTC_OPERATOR", Entity.ATTM_LSTC_OPERATOR));

                if (Opretaion_Mode == "Browse")
                {
                    if (!string.IsNullOrEmpty(Entity.ATTM_DATE_ADD))
                        sqlParamList.Add(new SqlParameter("@ATTM_DATE_ADD", Entity.ATTM_DATE_ADD));
                    if (!string.IsNullOrEmpty(Entity.ATTM_ADD_OPERATOR))
                        sqlParamList.Add(new SqlParameter("@ATTM_ADD_OPERATOR", Entity.ATTM_ADD_OPERATOR));
                    if (!string.IsNullOrEmpty(Entity.ATTM_DATE_LSTC))
                        sqlParamList.Add(new SqlParameter("@ATTM_DATE_LSTC", Entity.ATTM_DATE_LSTC));
                }


            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<SiteScheduleEntity> Browse_CHILDATTM(SiteScheduleEntity Entity, string Opretaion_Mode)
        {
            List<SiteScheduleEntity> CASEACTProfile = new List<SiteScheduleEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CHLDATTM_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CHILDATTM]");
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASECONT(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASEACTProfile.Add(new SiteScheduleEntity(row));
                }
            }
            catch (Exception ex)
            { return CASEACTProfile; }

            return CASEACTProfile;
        }

        public bool UpdateCHLDATTM(SiteScheduleEntity Entity, string Operation_Mode, out string Msg, out string Sql_Reslut_Msg, out int CurrentID)
        {
            bool boolsuccess = true;
            Sql_Reslut_Msg = "Success";
            Msg = string.Empty;
            CurrentID = 0;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList = Prepare_CHLDATTM_SqlParameters_List(Entity, Operation_Mode);

                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                SqlParameter ID = new SqlParameter("@CurrentId", SqlDbType.SmallInt, 5);
                ID.Direction = ParameterDirection.Output;
                sqlParamList.Add(ID);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.Update_CHLDATTM", out Sql_Reslut_Msg);  //
                Msg = DeleteMsg.Value.ToString();
                CurrentID = int.Parse(ID.Value.ToString());
                //Sql_Reslut_Msg = SP_Result_Msg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<SqlParameter> Prepare_CHLDATTMS_SqlParameters_List(ChildATTMSEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Row_Type));

                if (!string.IsNullOrEmpty(Entity.ATTMS_ID))
                    sqlParamList.Add(new SqlParameter("@ATTMS_ID", Entity.ATTMS_ID));
                if (!string.IsNullOrEmpty(Entity.ATTMS_DAY))
                    sqlParamList.Add(new SqlParameter("@ATTMS_DAY", Entity.ATTMS_DAY));
                if (!string.IsNullOrEmpty(Entity.ATTMS_WEEK))
                    sqlParamList.Add(new SqlParameter("@ATTMS_WEEK", Entity.ATTMS_WEEK));
                if (!string.IsNullOrEmpty(Entity.ATTMS_STATUS))
                    sqlParamList.Add(new SqlParameter("@ATTMS_STATUS", Entity.ATTMS_STATUS));

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<ChildATTMSEntity> Browse_CHILDATTMS(ChildATTMSEntity Entity, string Opretaion_Mode)
        {
            List<ChildATTMSEntity> CASEACTProfile = new List<ChildATTMSEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CHLDATTMS_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CHLDATTMS]");
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASECONT(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASEACTProfile.Add(new ChildATTMSEntity(row));
                }
            }
            catch (Exception ex)
            { return CASEACTProfile; }

            return CASEACTProfile;
        }

        public bool UpdateCHLDATTMS(ChildATTMSEntity Entity, string Operation_Mode, out string Msg, out string Sql_Reslut_Msg)
        {
            bool boolsuccess = true;
            Sql_Reslut_Msg = "Success";
            Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList = Prepare_CHLDATTMS_SqlParameters_List(Entity, Operation_Mode);

                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.Update_CHLDATTMS", out Sql_Reslut_Msg);  //
                Msg = DeleteMsg.Value.ToString();
                //Sql_Reslut_Msg = SP_Result_Msg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }



        public List<CASEMSEntity> Browse_CASEMS(CASEMSEntity Entity, string Opretaion_Mode)
        {
            List<CASEMSEntity> CASEACTProfile = new List<CASEMSEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASEMS_SqlParameters_List(Entity, Opretaion_Mode, string.Empty);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASEMS(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASEACTProfile.Add(new CASEMSEntity(row));
                }
            }
            catch (Exception ex)
            { return CASEACTProfile; }

            return CASEACTProfile;
        }

        public List<CASEMSEntity> Browse_CASEMS(CASEMSEntity Entity, string Opretaion_Mode, string Trigger)
        {
            List<CASEMSEntity> CASEACTProfile = new List<CASEMSEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASEMS_SqlParameters_List(Entity, Opretaion_Mode, Trigger);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASEMS(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASEACTProfile.Add(new CASEMSEntity(row, Trigger));
                }
            }
            catch (Exception ex)
            { return CASEACTProfile; }

            return CASEACTProfile;
        }

        public List<CASEMSOBOEntity> Browse_CASEMSOBO(CASEMSOBOEntity Entity, string Opretaion_Mode)
        {
            List<CASEMSOBOEntity> CASESPMProfile = new List<CASEMSOBOEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASEMSOBO_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASEMSOBO(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new CASEMSOBOEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<CAOBOEntity> Browse_CAOBO(CAOBOEntity Entity, string Opretaion_Mode)
        {
            List<CAOBOEntity> CASESPMProfile = new List<CAOBOEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CAOBO_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CAOBO(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new CAOBOEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public bool UpdateCASEMSOBO(CASEMSOBOEntity Entity, string Operation_Mode, out string Sql_SP_Result_Message)
        {
            bool boolsuccess;
            Sql_SP_Result_Message = Consts.Common.DB_Exception;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_CASEMSOBO_SqlParameters_List(Entity, Operation_Mode);
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateCASEMSOBO", out Sql_SP_Result_Message);  //
                //boolsuccess = Captain.DatabaseLayer.SPAdminDB.UpdateCASEACT(sqlParamList);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateCAOBO(CAOBOEntity Entity, string Operation_Mode, out string Sql_SP_Result_Message)
        {
            bool boolsuccess;
            Sql_SP_Result_Message = Consts.Common.DB_Exception;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_CAOBO_SqlParameters_List(Entity, Operation_Mode);
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateCAOBO", out Sql_SP_Result_Message);  //
                //boolsuccess = Captain.DatabaseLayer.SPAdminDB.UpdateCASEACT(sqlParamList);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }


        public List<CASEREFEntity> Browse_CASEREF(CASEREFEntity Entity, string Opretaion_Mode)
        {
            List<CASEREFEntity> CASESPMProfile = new List<CASEREFEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASEREF_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASEREF(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new CASEREFEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<AGCYBOTIQEntity> Browse_AGCYBoutiq(AGCYBOTIQEntity Entity, string Opretaion_Mode)
        {
            List<AGCYBOTIQEntity> CASESPMProfile = new List<AGCYBOTIQEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_AGCYBoutique_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_AGCYBoutique(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new AGCYBOTIQEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<AGCYREPEntity> Browse_AGCYREP(AGCYREPEntity Entity, string Opretaion_Mode)
        {
            List<AGCYREPEntity> CASESPMProfile = new List<AGCYREPEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_AGCYREP_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_AGCYREP(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new AGCYREPEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<AGCYSUBEntity> Browse_AGCYSubLoc(AGCYSUBEntity Entity, string Opretaion_Mode)
        {
            List<AGCYSUBEntity> CASESPMProfile = new List<AGCYSUBEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_AGCYSUB_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_AGYSUBLOC(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new AGCYSUBEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<AGCYSEREntity> Browse_AGCYServices(AGCYSEREntity Entity, string Opretaion_Mode)
        {
            List<AGCYSEREntity> CASESPMProfile = new List<AGCYSEREntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_AGCYSER_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_AGYSERVICES(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new AGCYSEREntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<AGCYPARTEntity> Browse_AgencyPartner(AGCYPARTEntity Entity, string Opretaion_Mode)
        {
            List<AGCYPARTEntity> CASESPMProfile = new List<AGCYPARTEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_AGCYPART_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_AGCYPART(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new AGCYPARTEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<AGCYPARTREPEntity> Browse_AgencyPart_Rep(AGCYPARTREPEntity Entity)
        {
            List<AGCYPARTREPEntity> CASESPMProfile = new List<AGCYPARTREPEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_AGCYPART_REP_SqlParameters_List(Entity);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_AGCYPART_REP(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new AGCYPARTREPEntity(row));
                }
            }
            catch (Exception ex)
            {
                return CASESPMProfile;
            }

            return CASESPMProfile;
        }

        public List<SqlParameter> Prepare_CASEMSOBO_SqlParameters_List(CASEMSOBOEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@CASEMSOBO_ID", Entity.ID));
                sqlParamList.Add(new SqlParameter("@CASEMSOBO_SEQ", Entity.Seq));
                sqlParamList.Add(new SqlParameter("@CASEMSOBO_CLID", Entity.CLID));
                sqlParamList.Add(new SqlParameter("@CASEMSOBO_FAM_SEQ", Entity.Fam_Seq));
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<SqlParameter> Prepare_CAOBO_SqlParameters_List(CAOBOEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@CAOBO_ID", Entity.ID));
                if (!string.IsNullOrEmpty(Entity.Seq.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_SEQ", Entity.Seq));
                if (!string.IsNullOrEmpty(Entity.CLID.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_CLID", Entity.CLID));
                if (!string.IsNullOrEmpty(Entity.Fam_Seq.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_FAM_SEQ", Entity.Fam_Seq));
                if(!string.IsNullOrEmpty(Entity.Amount.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_AMOUNT", Entity.Amount));
                if (!string.IsNullOrEmpty(Entity.Desc.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_DESC", Entity.Desc));
                if (!string.IsNullOrEmpty(Entity.SchoolGrade.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_SGRADE", Entity.SchoolGrade));
                if (!string.IsNullOrEmpty(Entity.SchoolDistrict.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_SDISTRICT", Entity.SchoolDistrict));
                if (!string.IsNullOrEmpty(Entity.Status.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_STATUS", Entity.Status));
                if (!string.IsNullOrEmpty(Entity.CompltdDate.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_COMPDATE", Entity.CompltdDate));
                if (!string.IsNullOrEmpty(Entity.TransUnits.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_TRANSUNITS", Entity.TransUnits));
                if (!string.IsNullOrEmpty(Entity.RecipentName.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_RECPINAME", Entity.RecipentName));
                if (!string.IsNullOrEmpty(Entity.Gift1.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_GIFT1", Entity.Gift1));
                if (!string.IsNullOrEmpty(Entity.Gift2.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_GIFT2", Entity.Gift2));
                if (!string.IsNullOrEmpty(Entity.Gift3.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_GIFT3", Entity.Gift3));
                if (!string.IsNullOrEmpty(Entity.GiftCard.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_GIFTCARD", Entity.GiftCard));
                if (!string.IsNullOrEmpty(Entity.BedSize.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_BEDSIZE", Entity.BedSize));
                if (!string.IsNullOrEmpty(Entity.AirMattress.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_AIRMATTRESS", Entity.AirMattress));
                if (!string.IsNullOrEmpty(Entity.TransUOM.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_TRANSUOM", Entity.TransUOM));

                if (!string.IsNullOrEmpty(Entity.ClothSize.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_CLOTHSIZE", Entity.ClothSize));
                if (!string.IsNullOrEmpty(Entity.ShoeSize.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_SHOESIZE", Entity.ShoeSize));
                if (!string.IsNullOrEmpty(Entity.Quantity.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_QUANTITY", Entity.Quantity));
                if (!string.IsNullOrEmpty(Entity.UnitPrice.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAOBO_UNITPRICE", Entity.UnitPrice));

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<SqlParameter> Prepare_CASEREF_SqlParameters_List(CASEREFEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@CASEREF_CODE", Entity.Code));
                sqlParamList.Add(new SqlParameter("@CASEREF_NAME1", Entity.Name1));
                if (!string.IsNullOrEmpty(Entity.Name2))
                    sqlParamList.Add(new SqlParameter("@CASEREF_NAME2", Entity.Name2));
                if (!string.IsNullOrEmpty(Entity.IndexBy))
                    sqlParamList.Add(new SqlParameter("@CASEREF_INDEXBY", Entity.IndexBy));

                sqlParamList.Add(new SqlParameter("@CASEREF_STREET", Entity.Street));
                sqlParamList.Add(new SqlParameter("@CASEREF_CITY", Entity.City));
                sqlParamList.Add(new SqlParameter("@CASEREF_STATE", Entity.State));
                sqlParamList.Add(new SqlParameter("@CASEREF_ZIP", Entity.Zip));
                sqlParamList.Add(new SqlParameter("@CASEREF_ZIP_PLUS", Entity.Zip_Plus));
                //if (!string.IsNullOrEmpty(Entity.Area))
                //    sqlParamList.Add(new SqlParameter("@CASEREF_AREA", Entity.Area));
                //if (!string.IsNullOrEmpty(Entity.Excgange))
                //    sqlParamList.Add(new SqlParameter("@CASEREF_EXCHANGE", Entity.Excgange));
                if (!string.IsNullOrEmpty(Entity.Telno))
                    sqlParamList.Add(new SqlParameter("@CASEREF_TELNO", Entity.Telno));
                sqlParamList.Add(new SqlParameter("@CASEREF_ACTIVE", Entity.Active));
                if (!string.IsNullOrEmpty(Entity.Cont_Fname))
                    sqlParamList.Add(new SqlParameter("@CASEREF_CONT_FNAME", Entity.Cont_Fname));
                if (!string.IsNullOrEmpty(Entity.Cont_Lname))
                    sqlParamList.Add(new SqlParameter("@CASEREF_CONT_LNAME", Entity.Cont_Lname));
                if (!string.IsNullOrEmpty(Entity.NameIndex))
                    sqlParamList.Add(new SqlParameter("@CASEREF_CONT_NAMEINDEX", Entity.NameIndex));
                //if (!string.IsNullOrEmpty(Entity.Cont_Area))
                //    sqlParamList.Add(new SqlParameter("@CASEREF_CONT_AREA", Entity.Cont_Area));
                //if (!string.IsNullOrEmpty(Entity.Cont_Exchange))
                //    sqlParamList.Add(new SqlParameter("@CASEREF_CONT_EXCHANGE", Entity.Cont_Exchange));
                if (!string.IsNullOrEmpty(Entity.Cont_TelNO))
                    sqlParamList.Add(new SqlParameter("@CASEREF_CONT_TELNO", Entity.Cont_TelNO));
                if (!string.IsNullOrEmpty(Entity.Long_Distance))
                    sqlParamList.Add(new SqlParameter("@CASEREF_LONG_DISTANCE", Entity.Long_Distance));
                //if (!string.IsNullOrEmpty(Entity.Fax_Area))
                //    sqlParamList.Add(new SqlParameter("@CASEREF_FAX_AREA", Entity.Fax_Area));
                //if (!string.IsNullOrEmpty(Entity.Fax_Exchange))
                //    sqlParamList.Add(new SqlParameter("@CASEREF_FAX_EXCHANGE", Entity.Fax_Exchange));

                if (!string.IsNullOrEmpty(Entity.Fax_Telno))
                    sqlParamList.Add(new SqlParameter("@CASEREF_FAX_TELNO", Entity.Fax_Telno));
                if (!string.IsNullOrEmpty(Entity.Outside))
                    sqlParamList.Add(new SqlParameter("@CASEREF_OUTSIDE", Entity.Outside));
                if (!string.IsNullOrEmpty(Entity.Category))
                    sqlParamList.Add(new SqlParameter("@CASEREF_CATEGORY", Entity.Category));
                if (!string.IsNullOrEmpty(Entity.County))
                    sqlParamList.Add(new SqlParameter("@CASEREF_COUNTY", Entity.County));
                if (!string.IsNullOrEmpty(Entity.From_Hrs))
                    sqlParamList.Add(new SqlParameter("@CASEREF_FROM_HRS", Entity.From_Hrs));
                if (!string.IsNullOrEmpty(Entity.To_Hrs))
                    sqlParamList.Add(new SqlParameter("@CASEREF_TO_HRS", Entity.To_Hrs));
                if (!string.IsNullOrEmpty(Entity.Sec))
                    sqlParamList.Add(new SqlParameter("@CASEREF_SEC", Entity.Sec));
                if (!string.IsNullOrEmpty(Entity.Email))
                    sqlParamList.Add(new SqlParameter("@CASEREF_EMAIL", Entity.Email));

                if (!string.IsNullOrEmpty(Entity.WebAddress))
                    sqlParamList.Add(new SqlParameter("@CASEREF_WEB_ADDRESS", Entity.WebAddress));

                sqlParamList.Add(new SqlParameter("@CASEREF_LSTC_OPERATOR", Entity.Lsct_Operator));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@CASEREF_DATE_ADD", Entity.Add_Date));
                    sqlParamList.Add(new SqlParameter("@CASEREF_ADD_OPERATOR", Entity.Add_Operator));
                    sqlParamList.Add(new SqlParameter("@CASEREF_DATE_LSTC", Entity.Lstc_Date));
                }

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<SqlParameter> Prepare_AGCYPART_SqlParameters_List(AGCYPARTEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@AGYP_CODE", Entity.Code));
                sqlParamList.Add(new SqlParameter("@AGYP_NAME", Entity.Name));
                if (!string.IsNullOrEmpty(Entity.Date))
                    sqlParamList.Add(new SqlParameter("@AGYP_Date", Entity.Date));
                
                sqlParamList.Add(new SqlParameter("@AGYP_Street", Entity.Street));
                sqlParamList.Add(new SqlParameter("@AGYP_City", Entity.City));
                sqlParamList.Add(new SqlParameter("@AGYP_State", Entity.State));
                sqlParamList.Add(new SqlParameter("@AGYP_Zip", Entity.Zip));
                if (!string.IsNullOrEmpty(Entity.Phone))
                    sqlParamList.Add(new SqlParameter("@AGYP_Phone", Entity.Phone));
                sqlParamList.Add(new SqlParameter("@AGYP_Active", Entity.Active));
                
                if (!string.IsNullOrEmpty(Entity.County))
                    sqlParamList.Add(new SqlParameter("@AGYP_COUNTY", Entity.County));
                if (!string.IsNullOrEmpty(Entity.Counties_Served))
                    sqlParamList.Add(new SqlParameter("AGYP_COUNTIES_SERVED", Entity.Counties_Served));
                if (!string.IsNullOrEmpty(Entity.From_Hrs))
                    sqlParamList.Add(new SqlParameter("@AGYP_FROM_HRS", Entity.From_Hrs));
                if (!string.IsNullOrEmpty(Entity.To_Hrs))
                    sqlParamList.Add(new SqlParameter("@AGYP_TO_HRS", Entity.To_Hrs));
                if (!string.IsNullOrEmpty(Entity.Website))
                    sqlParamList.Add(new SqlParameter("@AGYP_Website", Entity.Website));
                if (!string.IsNullOrEmpty(Entity.Docs))
                    sqlParamList.Add(new SqlParameter("@AGYP_Docs", Entity.Docs));
                //if (!string.IsNullOrEmpty(Entity.Notes))
                //    sqlParamList.Add(new SqlParameter("@AGYP_Notes", Entity.Notes));
                //if (!string.IsNullOrEmpty(Entity.ProgDiv))
                //    sqlParamList.Add(new SqlParameter("@AGYP_PROG_DIV", Entity.ProgDiv));
                if (!string.IsNullOrEmpty(Entity.ORGType))
                    sqlParamList.Add(new SqlParameter("@AGYP_ORG_TYPE", Entity.ORGType));
                if (!string.IsNullOrEmpty(Entity.Target))
                    sqlParamList.Add(new SqlParameter("@AGYP_Target", Entity.Target));

                if (!string.IsNullOrEmpty(Entity.Boutique))
                    sqlParamList.Add(new SqlParameter("@AGYP_BOUTIQUE", Entity.Boutique));

                if (!string.IsNullOrEmpty(Entity.AGYP_FORMS_STATUS))
                    sqlParamList.Add(new SqlParameter("@AGYP_FORMS_STATUS", Entity.AGYP_FORMS_STATUS));

                sqlParamList.Add(new SqlParameter("@AGYP_Lstc_Operator", Entity.Lsct_Operator));

                if (!string.IsNullOrEmpty(Entity.ServicePlans))
                    sqlParamList.Add(new SqlParameter("@AGYP_SERPLANS", Entity.ServicePlans));

                if(!string.IsNullOrEmpty(Entity.RefAsoc))
                    sqlParamList.Add(new SqlParameter("@AGYP_REF_ASOC", Entity.RefAsoc));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@AGYP_Date_Add", Entity.Add_Date));
                    sqlParamList.Add(new SqlParameter("@AGYP_Add_Operator", Entity.Add_Operator));
                    sqlParamList.Add(new SqlParameter("@AGYP_Date_Lstc", Entity.Lstc_Date));
                }

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }
        #region Added by Vikash on 03/14/2024 as per APC_UPDATE AP DATA SCREEN document
        public List<SqlParameter> Prepare_AGCYPART_REP_SqlParameters_List(AGCYPARTREPEntity Entity)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@AGYP_AP_CODE", Entity.Code));
                sqlParamList.Add(new SqlParameter("@AGYP_AP_NAME", Entity.Name));
                sqlParamList.Add(new SqlParameter("@AGYP_AP_City", Entity.City));
                if (!string.IsNullOrEmpty(Entity.County))
                    sqlParamList.Add(new SqlParameter("@AGYP_AP_COUNTY", Entity.County));
                if (!string.IsNullOrEmpty(Entity.ServicePlans))
                    sqlParamList.Add(new SqlParameter("@AGYP_AP_SERPLANS", Entity.ServicePlans));
                if (!string.IsNullOrEmpty(Entity.RefAsoc))
                    sqlParamList.Add(new SqlParameter("@AGYP_REF_AP_ASOC", Entity.RefAsoc));

                sqlParamList.Add(new SqlParameter("@AGYP_AP_Street", Entity.Street));
                sqlParamList.Add(new SqlParameter("@AGYP_AP_State", Entity.State));
                sqlParamList.Add(new SqlParameter("@AGYP_Date_AP_Add", Entity.Add_Date));
                sqlParamList.Add(new SqlParameter("@AGYP_Add_AP_Operator", Entity.Add_Operator));
                sqlParamList.Add(new SqlParameter("@AGYP_Date_AP_Lstc", Entity.Lstc_Date));
                sqlParamList.Add(new SqlParameter("@AGYP_Lstc_AP_Operator", Entity.Lsct_Operator));

                sqlParamList.Add(new SqlParameter("@AGYP_Active", Entity.Active));
                if (!string.IsNullOrEmpty(Entity.AGYP_FORMS_STATUS))
                    sqlParamList.Add(new SqlParameter("@AGYP_FORMS_STATUS", Entity.AGYP_FORMS_STATUS));

                if (!string.IsNullOrEmpty(Entity.Services))
                    sqlParamList.Add(new SqlParameter("@AGYP_AP_SERVICES", Entity.Services));
                if (!string.IsNullOrEmpty(Entity.Boutique))
                    sqlParamList.Add(new SqlParameter("@AGYP_AP_SRC", Entity.Boutique));

                if (!string.IsNullOrEmpty(Entity.RepName))
                    sqlParamList.Add(new SqlParameter("@AGYP_REP_NAME", Entity.RepName));
                if (!string.IsNullOrEmpty(Entity.RepLName))
                    sqlParamList.Add(new SqlParameter("@AGYP_REP_LASTNAME", Entity.RepLName));
                if (!string.IsNullOrEmpty(Entity.RepPhone))
                    sqlParamList.Add(new SqlParameter("@AGYP_REP_PHONE", Entity.RepPhone));
                if (!string.IsNullOrEmpty(Entity.RepPhone2))
                    sqlParamList.Add(new SqlParameter("@AGYP_REP_PHONE2", Entity.RepPhone2));
                if (!string.IsNullOrEmpty(Entity.RepPosition))
                    sqlParamList.Add(new SqlParameter("@AGYP_REP_POSITION", Entity.RepPosition));
                if (!string.IsNullOrEmpty(Entity.RepEmail))
                    sqlParamList.Add(new SqlParameter("@AGYP_REP_EMAIL", Entity.RepEmail));
                if (!string.IsNullOrEmpty(Entity.RepLead))
                    sqlParamList.Add(new SqlParameter("@AGYP_REP_LEAD", Entity.RepLead));


            }
            catch (Exception ex)
            {
                return sqlParamList;
            }

            return sqlParamList;
        }
        #endregion
        public List<SqlParameter> Prepare_AGCYBoutique_SqlParameters_List(AGCYBOTIQEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@AGYB_PART_CODE", Entity.Code));
                sqlParamList.Add(new SqlParameter("@AGYB_REQ_DATE", Entity.Req_Date));
                if (!string.IsNullOrEmpty(Entity.Req_Comp_Date))
                    sqlParamList.Add(new SqlParameter("@AGYB_REQ_COMP_DATE", Entity.Req_Comp_Date));
                if (!string.IsNullOrEmpty(Entity.Status))
                    sqlParamList.Add(new SqlParameter("@AGYB_STATUS", Entity.Status));
                if (!string.IsNullOrEmpty(Entity.Footage))
                    sqlParamList.Add(new SqlParameter("@AGYB_SQ_FOOTAGE", Entity.Footage));
                if (!string.IsNullOrEmpty(Entity.Shared))
                    sqlParamList.Add(new SqlParameter("@AGYB_SP_SHARED", Entity.Shared));
                if (!string.IsNullOrEmpty(Entity.Shared_desc))
                    sqlParamList.Add(new SqlParameter("@AGYB_SP_SHARED_DESC", Entity.Shared_desc));
                if (!string.IsNullOrEmpty(Entity.ItemsNeeded))
                    sqlParamList.Add(new SqlParameter("@AGYB_ITEMS_NEEDED", Entity.ItemsNeeded));
                if (!string.IsNullOrEmpty(Entity.Population))
                    sqlParamList.Add(new SqlParameter("@AGYB_POPULATION", Entity.Poverty));

                if (!string.IsNullOrEmpty(Entity.Free_Lunch))
                    sqlParamList.Add(new SqlParameter("@AGYB_FR_LUNCH", Entity.Free_Lunch));
                if (!string.IsNullOrEmpty(Entity.Poverty))
                    sqlParamList.Add(new SqlParameter("@AGYB_POVERTY", Entity.Poverty));
                if (!string.IsNullOrEmpty(Entity.City))
                    sqlParamList.Add(new SqlParameter("@AGYB_CITY", Entity.City));
                if (!string.IsNullOrEmpty(Entity.County))
                    sqlParamList.Add(new SqlParameter("@AGYB_COUNTY", Entity.County));
                if (!string.IsNullOrEmpty(Entity.Percentage))
                    sqlParamList.Add(new SqlParameter("@AGYB_PERCENTAGE", Entity.Percentage));
                if (!string.IsNullOrEmpty(Entity.InvoiceForm))
                    sqlParamList.Add(new SqlParameter("@AGYB_INV_FORM", Entity.InvoiceForm));
                
                sqlParamList.Add(new SqlParameter("@AGYB_LSTC_OPERATOR", Entity.Lsct_Operator));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@AGYB_DATE_ADD", Entity.Add_Date));
                    sqlParamList.Add(new SqlParameter("@AGYB_ADD_OPERATOR", Entity.Add_Operator));
                    sqlParamList.Add(new SqlParameter("@AGYB_DATE_LSTC", Entity.Lstc_Date));
                }

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<SqlParameter> Prepare_AGCYREP_SqlParameters_List(AGCYREPEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@AGYR_PART_CODE", Entity.PartCode));
                sqlParamList.Add(new SqlParameter("@AGYR_REP_CODE", Entity.RepCode));
                if (!string.IsNullOrEmpty(Entity.FName))
                    sqlParamList.Add(new SqlParameter("@AGYR_REP_FNAME", Entity.FName));
                if (!string.IsNullOrEmpty(Entity.LName))
                    sqlParamList.Add(new SqlParameter("@AGYR_REP_LNAME", Entity.LName));

                sqlParamList.Add(new SqlParameter("@AGYR_REP_Street", Entity.Street));
                sqlParamList.Add(new SqlParameter("@AGYR_REP_City", Entity.City));
                sqlParamList.Add(new SqlParameter("@AGYR_REP_State", Entity.State));
                sqlParamList.Add(new SqlParameter("@AGYR_REP_Zip", Entity.Zip));
                if (!string.IsNullOrEmpty(Entity.Phone1))
                    sqlParamList.Add(new SqlParameter("@AGYR_REP_PHONE1", Entity.Phone1));
                if (!string.IsNullOrEmpty(Entity.Phone2))
                    sqlParamList.Add(new SqlParameter("@AGYR_REP_PHONE2", Entity.Phone2));
                if (!string.IsNullOrEmpty(Entity.Ext))
                    sqlParamList.Add(new SqlParameter("@AGYR_REP_Phone_Ext", Entity.Ext));

                if (!string.IsNullOrEmpty(Entity.Position))
                    sqlParamList.Add(new SqlParameter("@AGYR_REP_POS", Entity.Position));
                if (!string.IsNullOrEmpty(Entity.Prog))
                    sqlParamList.Add(new SqlParameter("@AGYR_REP_PROG", Entity.Prog));
                if (!string.IsNullOrEmpty(Entity.Email))
                    sqlParamList.Add(new SqlParameter("@AGYR_REP_EMAIL", Entity.Email));

                if (!string.IsNullOrEmpty(Entity.Phn1_NA))
                    sqlParamList.Add(new SqlParameter("@AGYR_REP_PHN1_NA", Entity.Phn1_NA));
                if (!string.IsNullOrEmpty(Entity.Phn2_NA))
                    sqlParamList.Add(new SqlParameter("@AGYR_REP_PHN2_NA", Entity.Phn2_NA));
                if (!string.IsNullOrEmpty(Entity.Email_NA))
                    sqlParamList.Add(new SqlParameter("@AGYR_REP_EMAIL_NA", Entity.Email_NA));
                if (!string.IsNullOrEmpty(Entity.AGYR_AGY_CONTACT))
                    sqlParamList.Add(new SqlParameter("@AGYR_AGY_CONTACT", Entity.AGYR_AGY_CONTACT));

                if (!string.IsNullOrEmpty(Entity.AGYR_BOUT_CONTACT))
                    sqlParamList.Add(new SqlParameter("@AGYR_BOUT_CONTACT", Entity.AGYR_BOUT_CONTACT));

                if (!string.IsNullOrEmpty(Entity.AGYR_REP_ACTIVE))
                    sqlParamList.Add(new SqlParameter("@AGYR_REP_ACTIVE", Entity.AGYR_REP_ACTIVE));

                if (!string.IsNullOrEmpty(Entity.AGYR_REP_REQLEAD))
                    sqlParamList.Add(new SqlParameter("@AGYR_REP_REQLEAD", Entity.AGYR_REP_REQLEAD));

                sqlParamList.Add(new SqlParameter("@AGYR_REP_Lstc_Operator", Entity.Lsct_Operator));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@AGYR_REP_Date_Add", Entity.Add_Date));
                    sqlParamList.Add(new SqlParameter("@AGYR_REP_Add_Operator", Entity.Add_Operator));
                    sqlParamList.Add(new SqlParameter("@AGYR_REP_Date_Lstc", Entity.Lstc_Date));
                }

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<SqlParameter> Prepare_AGCYSUB_SqlParameters_List(AGCYSUBEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@AGYS_PART_CODE", Entity.PartCode));
                sqlParamList.Add(new SqlParameter("@AGYS_SUB_CODE", Entity.SubCode));
                if (!string.IsNullOrEmpty(Entity.SubLocation))
                    sqlParamList.Add(new SqlParameter("@AGYS_SUB_LOCATION", Entity.SubLocation));
                if (!string.IsNullOrEmpty(Entity.HN.Trim()))
                    sqlParamList.Add(new SqlParameter("AGYS_SUB_HN", Entity.HN));

                sqlParamList.Add(new SqlParameter("@AGYS_SUB_Street", Entity.Street));
                sqlParamList.Add(new SqlParameter("@AGYS_SUB_CITY", Entity.City));
                sqlParamList.Add(new SqlParameter("@AGYS_SUB_STATE", Entity.State));
                sqlParamList.Add(new SqlParameter("@AGYS_SUB_ZIP", Entity.Zip));
                
                sqlParamList.Add(new SqlParameter("@AGYS_SUB_Lstc_Operator", Entity.Lsct_Operator));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@AGYS_SUB_Date_Add", Entity.Add_Date));
                    sqlParamList.Add(new SqlParameter("@AGYS_SUB_Add_Operator", Entity.Add_Operator));
                    sqlParamList.Add(new SqlParameter("@AGYS_SUB_Date_Lstc", Entity.Lstc_Date));
                }

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<SqlParameter> Prepare_AGCYSER_SqlParameters_List(AGCYSEREntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@AGYS_PART_CODE", Entity.PartCode));
                if (!string.IsNullOrEmpty(Entity.SerCode))
                    sqlParamList.Add(new SqlParameter("@AGYS_SER_CODE", Entity.SerCode));
                if (!string.IsNullOrEmpty(Entity.Category))
                    sqlParamList.Add(new SqlParameter("@AGYS_CATEGORY", Entity.Category));
                if (!string.IsNullOrEmpty(Entity.Service))
                    sqlParamList.Add(new SqlParameter("@AGYS_SERVICE", Entity.Service));

                if (!string.IsNullOrEmpty(Entity.Location))
                    sqlParamList.Add(new SqlParameter("@AGYS_SUBLOC", Entity.Location));

                if (!string.IsNullOrEmpty(Entity.SubLocation))
                    sqlParamList.Add(new SqlParameter("@AGYS_SUB_LOCATION", Entity.SubLocation.Trim()));

                if (!string.IsNullOrEmpty(Entity.Street))
                    sqlParamList.Add(new SqlParameter("@AGYS_Street", Entity.Street));
                if (!string.IsNullOrEmpty(Entity.City))
                    sqlParamList.Add(new SqlParameter("@AGYS_CITY", Entity.City));
                if (!string.IsNullOrEmpty(Entity.State))
                    sqlParamList.Add(new SqlParameter("@AGYS_STATE", Entity.State));
                if (!string.IsNullOrEmpty(Entity.Zip))
                    sqlParamList.Add(new SqlParameter("@AGYS_ZIP", Entity.Zip));

                if (!string.IsNullOrEmpty(Entity.ServiceDetails))
                    sqlParamList.Add(new SqlParameter("@AGYS_SER_Details", Entity.ServiceDetails));

                sqlParamList.Add(new SqlParameter("@AGYS_Lstc_Operator", Entity.Lsct_Operator));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@AGYS_Date_Add", Entity.Add_Date));
                    sqlParamList.Add(new SqlParameter("@AGYS_Add_Operator", Entity.Add_Operator));
                    sqlParamList.Add(new SqlParameter("@AGYS_Date_Lstc", Entity.Lstc_Date));
                }

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }


       public List<SqlParameter> Prepare_CASESPM_SqlParameters_List(CASESPMEntity Entity, string Opretaion_Mode, string App_Not_EqualTo)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));
                }

                sqlParamList.Add(new SqlParameter("@spm_agency", Entity.agency));
                sqlParamList.Add(new SqlParameter("@spm_dept", Entity.dept));
                sqlParamList.Add(new SqlParameter("@spm_program", Entity.program));
                sqlParamList.Add(new SqlParameter("@spm_year", Entity.year));
                sqlParamList.Add(new SqlParameter("@spm_app_no", Entity.app_no));
                sqlParamList.Add(new SqlParameter("@spm_service_plan", Entity.service_plan));
                sqlParamList.Add(new SqlParameter("@spm_Seq", Entity.Seq));
                sqlParamList.Add(new SqlParameter("@spm_caseworker", Entity.caseworker));
                sqlParamList.Add(new SqlParameter("@spm_site", Entity.site));
                sqlParamList.Add(new SqlParameter("@spm_startdate", Entity.startdate));
                sqlParamList.Add(new SqlParameter("@spm_estdate", Entity.estdate));
                sqlParamList.Add(new SqlParameter("@spm_compdate", Entity.compdate));
                sqlParamList.Add(new SqlParameter("@spm_sel_branches", Entity.sel_branches));
                sqlParamList.Add(new SqlParameter("@spm_have_addlbr", Entity.have_addlbr));
                sqlParamList.Add(new SqlParameter("@spm_lstc_operator", Entity.lstc_operator));
                sqlParamList.Add(new SqlParameter("@spm_def_program", Entity.Def_Program));
                sqlParamList.Add(new SqlParameter("@spm_bulk", Entity.Def_Program));
                
                if (!string.IsNullOrEmpty(Entity.SPM_Fund))
                    sqlParamList.Add(new SqlParameter("@SPM_FUND", Entity.SPM_Fund));
                if (!string.IsNullOrEmpty(Entity.SPM_Amount))
                    sqlParamList.Add(new SqlParameter("@SPM_AMOUNT", Entity.SPM_Amount));
                if (!string.IsNullOrEmpty(Entity.SPM_Balance))
                    sqlParamList.Add(new SqlParameter("@SPM_BAL_AMT", Entity.SPM_Balance));
                if (!string.IsNullOrEmpty(Entity.SPM_EligStatus))
                    sqlParamList.Add(new SqlParameter("@SPM_ELIG_STATUS", Entity.SPM_EligStatus));
                if (!string.IsNullOrEmpty(Entity.SPM_BDC_ID))
                    sqlParamList.Add(new SqlParameter("@SPM_BDC_ID", Entity.SPM_BDC_ID));
                if (!string.IsNullOrEmpty(Entity.SPM_Vendor))
                    sqlParamList.Add(new SqlParameter("@SPM_VENDOR", Entity.SPM_Vendor));
                if (!string.IsNullOrEmpty(Entity.SPM_Account))
                    sqlParamList.Add(new SqlParameter("@SPM_ACCOUNT_NO", Entity.SPM_Account));
                if (!string.IsNullOrEmpty(Entity.SPM_BillName_Type))
                    sqlParamList.Add(new SqlParameter("@SPM_BILLNAME_TYPE", Entity.SPM_BillName_Type));
                if (!string.IsNullOrEmpty(Entity.SPM_Bill_FName))
                    sqlParamList.Add(new SqlParameter("@SPM_BILL_FNAME", Entity.SPM_Bill_FName));
                if (!string.IsNullOrEmpty(Entity.SPM_Bill_LName))
                    sqlParamList.Add(new SqlParameter("@SPM_BILL_LNAME", Entity.SPM_Bill_LName));
                if (!string.IsNullOrEmpty(Entity.SPM_MassClose))
                    sqlParamList.Add(new SqlParameter("@SPM_MASS_CLOSE", Entity.SPM_MassClose));

                if (!string.IsNullOrEmpty(Entity.SPM_Gas_Vendor))
                    sqlParamList.Add(new SqlParameter("@SPM_GAS_VENDOR", Entity.SPM_Gas_Vendor));
                if (!string.IsNullOrEmpty(Entity.SPM_Gas_Account))
                    sqlParamList.Add(new SqlParameter("@SPM_GAS_ACCOUNT_NO", Entity.SPM_Gas_Account));
                if (!string.IsNullOrEmpty(Entity.SPM_Gas_BillName_Type))
                    sqlParamList.Add(new SqlParameter("@SPM_GAS_BILLNAME_TYPE", Entity.SPM_Gas_BillName_Type));
                if (!string.IsNullOrEmpty(Entity.SPM_Gas_Bill_FName))
                    sqlParamList.Add(new SqlParameter("@SPM_GAS_BILL_FNAME", Entity.SPM_Gas_Bill_FName));
                if (!string.IsNullOrEmpty(Entity.SPM_Gas_Bill_LName))
                    sqlParamList.Add(new SqlParameter("@SPM_GAS_BILL_LNAME", Entity.SPM_Gas_Bill_LName));


                if (!string.IsNullOrEmpty(Entity.SPM_Prim_Source))
                    sqlParamList.Add(new SqlParameter("@SPM_PRIM_SOURCE", Entity.SPM_Prim_Source));
                if (!string.IsNullOrEmpty(Entity.SPM_Sec_Source))
                    sqlParamList.Add(new SqlParameter("@SPM_SEC_SOURCE", Entity.SPM_Sec_Source));

                if (!string.IsNullOrEmpty(Entity.Spm_Benefit_Reasn))
                    sqlParamList.Add(new SqlParameter("@SPM_BENEFIT_REASN ", Entity.Spm_Benefit_Reasn));

                if (!string.IsNullOrEmpty(Entity.SPM_LUMP_INTRVAL))
                    sqlParamList.Add(new SqlParameter("@SPM_LUMP_INTRVAL ", Entity.SPM_LUMP_INTRVAL));

                if (!string.IsNullOrEmpty(Entity.SPM_FIRST_APP))
                    sqlParamList.Add(new SqlParameter("@SPM_FIRST_APP", Entity.SPM_FIRST_APP));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@spm_date_lstc", Entity.date_lstc));
                    sqlParamList.Add(new SqlParameter("@spm_date_add", Entity.date_add));
                    sqlParamList.Add(new SqlParameter("@spm_add_operator", Entity.add_operator));
                    sqlParamList.Add(new SqlParameter("@spm_app_no_OtherThan", App_Not_EqualTo));

                    sqlParamList.Add(new SqlParameter("@sp0_Desc", Entity.Sp0_Desc));
                    sqlParamList.Add(new SqlParameter("@sp0_Validated", Entity.Sp0_Validatetd));
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<SqlParameter> Prepare_CASECONT_SqlParameters_List(CASECONTEntity Entity, string strProgramCode, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));
                    if (Entity.Rec_Type == "U" || Entity.Rec_Type == "D")
                    {
                        if (!string.IsNullOrEmpty(Entity.Seq))
                            sqlParamList.Add(new SqlParameter("@CONT_SEQ", int.Parse(Entity.Seq)));
                    }
                    else
                        sqlParamList.Add(new SqlParameter("@CONT_SEQ", int.Parse("0")));
                }

                sqlParamList.Add(new SqlParameter("@CONT_AGENCY", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@CONT_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@CONT_PROGRAM", Entity.Program));
                sqlParamList.Add(new SqlParameter("@CONT_YEAR", Entity.Year));
                sqlParamList.Add(new SqlParameter("@CONT_APP_NO", Entity.App_no));


                if (!string.IsNullOrEmpty(Entity.Contact_No))
                    sqlParamList.Add(new SqlParameter("@CONT_CONTACT_NO", Entity.Contact_No));
                sqlParamList.Add(new SqlParameter("@CONT_CONTACT_NAME", Entity.Contact_Name));
                sqlParamList.Add(new SqlParameter("@CONT_CASEWORKER", Entity.CaseWorker));
                sqlParamList.Add(new SqlParameter("@CONT_DATE", Entity.Cont_Date));
                sqlParamList.Add(new SqlParameter("@CONT_DURATION_TYPE", Entity.Duration_Type));
                sqlParamList.Add(new SqlParameter("@CONT_TIME", Entity.Time));
                sqlParamList.Add(new SqlParameter("@CONT_STARTS", Entity.Time_Starts));
                sqlParamList.Add(new SqlParameter("@CONT_END", Entity.Time_Ends));

                if (!string.IsNullOrEmpty(Entity.Duration))
                    sqlParamList.Add(new SqlParameter("@CONT_DURATION", Entity.Duration));
                //if (!string.IsNullOrEmpty(Entity.Duration_Min))
                //    sqlParamList.Add(new SqlParameter("@CONT_DURATION_MN", Entity.Duration_Min));
                sqlParamList.Add(new SqlParameter("@CONT_HOW_WHERE", Entity.How_Where));
                sqlParamList.Add(new SqlParameter("@CONT_INTER_LANG", Entity.Language));
                sqlParamList.Add(new SqlParameter("@CONT_INTERPRETER", Entity.Interpreter));


                sqlParamList.Add(new SqlParameter("@CONT_REF_FROM_CODE", Entity.Refer_From));
                sqlParamList.Add(new SqlParameter("@CONT_BILLTO_CODE", Entity.BillTO));
                sqlParamList.Add(new SqlParameter("@CONT_BILLTO_UOM", Entity.BillTo_UOM));
                sqlParamList.Add(new SqlParameter("@CONT_CUST1_CODE", Entity.Cust1_Code));
                sqlParamList.Add(new SqlParameter("@CONT_CUST1_VALUE", Entity.Cust1_Value));
                sqlParamList.Add(new SqlParameter("@CONT_CUST2_CODE", Entity.Cust2_Code));
                sqlParamList.Add(new SqlParameter("@CONT_CUST2_VALUE", Entity.Cust2_Value));
                sqlParamList.Add(new SqlParameter("@CONT_CUST3_CODE", Entity.Cust3_Code));
                sqlParamList.Add(new SqlParameter("@CONT_CUST3_VALUE", Entity.Cust3_Value));
                sqlParamList.Add(new SqlParameter("@CONT_BRIDGE", Entity.Bridge));
                sqlParamList.Add(new SqlParameter("@CONT_LSTC_OPERATOR", Entity.Lsct_Operator));
                sqlParamList.Add(new SqlParameter("@CONT_CONTPROG", Entity.Cont_Program));
                if (Opretaion_Mode != "Browse")
                {
                    if (!string.IsNullOrEmpty(Entity.FollowuponDate))
                        sqlParamList.Add(new SqlParameter("@CASECONT_FUP_DATE", Entity.FollowuponDate));
                    if (!string.IsNullOrEmpty(Entity.FollowupCompleteDate))
                        sqlParamList.Add(new SqlParameter("@CASECONT_COMP_DATE", Entity.FollowupCompleteDate));
                }
                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@CONT_DATE_LSTC", Entity.Lstc_Date));
                    sqlParamList.Add(new SqlParameter("@CONT_DATE_ADD", Entity.Add_Date));
                    sqlParamList.Add(new SqlParameter("@CONT_ADD_OPERATOR", Entity.Add_Operator));
                }
                if (strProgramCode != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@PROGRAMCODE", strProgramCode));
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }


        public List<SqlParameter> Prepare_CASEACT_SqlParameters_List(CASEACTEntity Entity, string Opretaion_Mode, string Trigger)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@CASEACT_AGENCY", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@CASEACT_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@CASEACT_PROGRAM", Entity.Program));
                sqlParamList.Add(new SqlParameter("@CASEACT_YEAR", Entity.Year));
                sqlParamList.Add(new SqlParameter("@CASEACT_APP_NO", Entity.App_no));

                if (!string.IsNullOrEmpty(Entity.Service_plan))
                    sqlParamList.Add(new SqlParameter("@CASEACT_SERVICEPLAN", Entity.Service_plan));   //int
                sqlParamList.Add(new SqlParameter("@CASEACT_SPM_SEQ", Entity.SPM_Seq));   //int
                sqlParamList.Add(new SqlParameter("@CASEACT_BRANCH", Entity.Branch));
                if (!string.IsNullOrEmpty(Entity.Group))
                    sqlParamList.Add(new SqlParameter("@CASEACT_GROUP", Entity.Group));
                //sqlParamList.Add(new SqlParameter("@CASEACT_TYPE", Entity.Type));



                sqlParamList.Add(new SqlParameter("@CASEACT_ACTIVITY_CODE", Entity.ACT_Code));
                sqlParamList.Add(new SqlParameter("@CASEACT_ID", Entity.ACT_ID));
                sqlParamList.Add(new SqlParameter("@CASEACT_ACTY_DATE", Entity.ACT_Date));
                if (!string.IsNullOrEmpty(Entity.ACT_Seq))
                    sqlParamList.Add(new SqlParameter("@CASEACT_SEQ", Entity.ACT_Seq));
                if (!string.IsNullOrEmpty(Entity.Site))
                    sqlParamList.Add(new SqlParameter("@CASEACT_SITE", Entity.Site));
                if (!string.IsNullOrEmpty(Entity.Fund1))
                    sqlParamList.Add(new SqlParameter("@CASEACT_FUND1", Entity.Fund1));
                if (!string.IsNullOrEmpty(Entity.Fund2))
                    sqlParamList.Add(new SqlParameter("@CASEACT_FUND2", Entity.Fund2));
                if (!string.IsNullOrEmpty(Entity.Fund3))
                    sqlParamList.Add(new SqlParameter("@CASEACT_FUND3", Entity.Fund3));

                if (!string.IsNullOrEmpty(Entity.Caseworker))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CASEWRKR", Entity.Caseworker));
                if (!string.IsNullOrEmpty(Entity.Vendor_No))
                    sqlParamList.Add(new SqlParameter("@CASEACT_VENDOR_NO", Entity.Vendor_No));
                if (!string.IsNullOrEmpty(Entity.Check_Date))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CHECK_DT", Entity.Check_Date));
                if (!string.IsNullOrEmpty(Entity.Check_No))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CHECK_NO", Entity.Check_No));


                if (!string.IsNullOrEmpty(Entity.Cost))
                    sqlParamList.Add(new SqlParameter("@CASEACT_COST", Decimal.Parse(Entity.Cost)));
                if (!string.IsNullOrEmpty(Entity.Followup_On))
                    sqlParamList.Add(new SqlParameter("@CASEACT_FOLLUP_ON", Entity.Followup_On));
                if (!string.IsNullOrEmpty(Entity.Followup_Comp))
                    sqlParamList.Add(new SqlParameter("@CASEACT_FOLLUP_COMP", Entity.Followup_Comp));

                if (!string.IsNullOrEmpty(Entity.Followup_By))
                    sqlParamList.Add(new SqlParameter("@CASEACT_FUPBY", Entity.Followup_By));
                //                sqlParamList.Add(new SqlParameter("@CASEACT_REF_DATA", Entity.Refer_Data));
                if (!string.IsNullOrEmpty(Entity.Cust_Code1))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CUST1_CODE", Entity.Cust_Code1));
                if (!string.IsNullOrEmpty(Entity.Cust_Value1))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CUST1_VALUE", Entity.Cust_Value1));
                if (!string.IsNullOrEmpty(Entity.Cust_Code2))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CUST2_CODE", Entity.Cust_Code2));
                if (!string.IsNullOrEmpty(Entity.Cust_Value2))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CUST2_VALUE", Entity.Cust_Value2));
                if (!string.IsNullOrEmpty(Entity.Cust_Code3))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CUST3_CODE", Entity.Cust_Code3));
                if (!string.IsNullOrEmpty(Entity.Cust_Value3))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CUST3_VALUE", Entity.Cust_Value3));
                if (!string.IsNullOrEmpty(Entity.Cust_Code4))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CUST4_CODE", Entity.Cust_Code4));
                if (!string.IsNullOrEmpty(Entity.Cust_Value4))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CUST4_VALUE", Entity.Cust_Value4));
                if (!string.IsNullOrEmpty(Entity.Cust_Code5))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CUST5_CODE", Entity.Cust_Code5));
                if (!string.IsNullOrEmpty(Entity.Cust_Value5))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CUST5_VALUE", Entity.Cust_Value5));
                if (!string.IsNullOrEmpty(Entity.Bulk))
                    sqlParamList.Add(new SqlParameter("@CASEACT_BULK", Entity.Bulk));

                sqlParamList.Add(new SqlParameter("@CASEACT_ACTY_PROG", Entity.Act_PROG));

                sqlParamList.Add(new SqlParameter("@CASEACT_LSTC_OPERATOR", Entity.Lsct_Operator));

                if (!string.IsNullOrEmpty(Entity.Units))
                    sqlParamList.Add(new SqlParameter("@CASEACT_UNITS", Entity.Units));
                if (!string.IsNullOrEmpty(Entity.UOM))
                    sqlParamList.Add(new SqlParameter("@CASEACT_UOM", Entity.UOM));
                if (!string.IsNullOrEmpty(Entity.Curr_Grp))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CURR_GRP", Entity.Curr_Grp));

                if (!string.IsNullOrEmpty(Entity.ActSeek_Date))
                    sqlParamList.Add(new SqlParameter("@CASEACT_SEEK_DATE", Entity.ActSeek_Date));

                if (!string.IsNullOrEmpty(Entity.CA_OBF))
                    sqlParamList.Add(new SqlParameter("@CASEACT_OBF", Entity.CA_OBF));

                if (!string.IsNullOrEmpty(Entity.Rate))
                    sqlParamList.Add(new SqlParameter("@CASEACT_RATE", Decimal.Parse(Entity.Rate)));


                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@CASEACT_DATE_LSTC", Entity.Lstc_Date));
                    sqlParamList.Add(new SqlParameter("@CASEACT_DATE_ADD", Entity.Add_Date));
                    sqlParamList.Add(new SqlParameter("@CASEACT_ADD_OPERATOR", Entity.Add_Operator));
                }

                if (Trigger == "Trigger")
                {
                    sqlParamList.Add(new SqlParameter("@CASEACT_TRIG_CODE", Entity.ACT_TrigCode));
                    sqlParamList.Add(new SqlParameter("@CASEACT_TRIG_DATE", Entity.ACT_TrigDate));
                    sqlParamList.Add(new SqlParameter("@CASEACT_TRIG_DATE_SEQ", Entity.ACT_TrigDateSeq));
                }

                if (Trigger == "CASEACT2.0")
                {
                    

                    if (!string.IsNullOrEmpty(Entity.Amount3))
                        sqlParamList.Add(new SqlParameter("@CASEACT_AMOUNT3", Decimal.Parse(Entity.Amount3)));

                    if (!string.IsNullOrEmpty(Entity.Units2))
                        sqlParamList.Add(new SqlParameter("@CASEACT_UNITS2", Entity.Units2));
                    if (!string.IsNullOrEmpty(Entity.UOM2))
                        sqlParamList.Add(new SqlParameter("@CASEACT_UOM2", Entity.UOM2));

                    if (!string.IsNullOrEmpty(Entity.Units3))
                        sqlParamList.Add(new SqlParameter("@CASEACT_UNITS3", Entity.Units3));
                    if (!string.IsNullOrEmpty(Entity.UOM3))
                        sqlParamList.Add(new SqlParameter("@CASEACT_UOM3", Entity.UOM3));
                }

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<SqlParameter> Prepare_CASEACT2_SqlParameters_List(CASEACTEntity Entity, string Opretaion_Mode, string Trigger)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@CASEACT_AGENCY", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@CASEACT_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@CASEACT_PROGRAM", Entity.Program));
                sqlParamList.Add(new SqlParameter("@CASEACT_YEAR", Entity.Year));
                if (!string.IsNullOrEmpty(Entity.App_no))
                    sqlParamList.Add(new SqlParameter("@CASEACT_APP_NO", Entity.App_no));

                if (!string.IsNullOrEmpty(Entity.Service_plan))
                    sqlParamList.Add(new SqlParameter("@CASEACT_SERVICEPLAN", Entity.Service_plan));   //int
                sqlParamList.Add(new SqlParameter("@CASEACT_SPM_SEQ", Entity.SPM_Seq));   //int
                sqlParamList.Add(new SqlParameter("@CASEACT_BRANCH", Entity.Branch));
                if (!string.IsNullOrEmpty(Entity.Group))
                    sqlParamList.Add(new SqlParameter("@CASEACT_GROUP", Entity.Group));
                //sqlParamList.Add(new SqlParameter("@CASEACT_TYPE", Entity.Type));


                if (!string.IsNullOrEmpty(Entity.ACT_Code))
                    sqlParamList.Add(new SqlParameter("@CASEACT_ACTIVITY_CODE", Entity.ACT_Code));
                if (!string.IsNullOrEmpty(Entity.ACT_ID))
                    sqlParamList.Add(new SqlParameter("@CASEACT_ID", Entity.ACT_ID));
                if (!string.IsNullOrEmpty(Entity.ACT_Date))
                    sqlParamList.Add(new SqlParameter("@CASEACT_ACTY_DATE", Entity.ACT_Date));
                if (!string.IsNullOrEmpty(Entity.ACT_Seq))
                    sqlParamList.Add(new SqlParameter("@CASEACT_SEQ", Entity.ACT_Seq));
                if (!string.IsNullOrEmpty(Entity.Site))
                    sqlParamList.Add(new SqlParameter("@CASEACT_SITE", Entity.Site));
                
                if (!string.IsNullOrEmpty(Entity.Fund1))
                    sqlParamList.Add(new SqlParameter("@CASEACT_FUND1", Entity.Fund1));
                if (!string.IsNullOrEmpty(Entity.Fund2))
                    sqlParamList.Add(new SqlParameter("@CASEACT_FUND2", Entity.Fund2));
                if (!string.IsNullOrEmpty(Entity.Fund3))
                    sqlParamList.Add(new SqlParameter("@CASEACT_FUND3", Entity.Fund3));

                if (!string.IsNullOrEmpty(Entity.Caseworker))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CASEWRKR", Entity.Caseworker));
                if (!string.IsNullOrEmpty(Entity.Vendor_No))
                    sqlParamList.Add(new SqlParameter("@CASEACT_VENDOR_NO", Entity.Vendor_No));
                if (!string.IsNullOrEmpty(Entity.Check_Date))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CHECK_DT", Entity.Check_Date));
                if (!string.IsNullOrEmpty(Entity.Check_No))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CHECK_NO", Entity.Check_No));


                if (!string.IsNullOrEmpty(Entity.Cost))
                    sqlParamList.Add(new SqlParameter("@CASEACT_COST", Decimal.Parse(Entity.Cost)));
                if (!string.IsNullOrEmpty(Entity.Followup_On))
                    sqlParamList.Add(new SqlParameter("@CASEACT_FOLLUP_ON", Entity.Followup_On));
                if (!string.IsNullOrEmpty(Entity.Followup_Comp))
                    sqlParamList.Add(new SqlParameter("@CASEACT_FOLLUP_COMP", Entity.Followup_Comp));

                if (!string.IsNullOrEmpty(Entity.Followup_By))
                    sqlParamList.Add(new SqlParameter("@CASEACT_FUPBY", Entity.Followup_By));
                //                sqlParamList.Add(new SqlParameter("@CASEACT_REF_DATA", Entity.Refer_Data));
                if (!string.IsNullOrEmpty(Entity.Cust_Code1))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CUST1_CODE", Entity.Cust_Code1));
                if (!string.IsNullOrEmpty(Entity.Cust_Value1))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CUST1_VALUE", Entity.Cust_Value1));
                if (!string.IsNullOrEmpty(Entity.Cust_Code2))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CUST2_CODE", Entity.Cust_Code2));
                if (!string.IsNullOrEmpty(Entity.Cust_Value2))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CUST2_VALUE", Entity.Cust_Value2));
                if (!string.IsNullOrEmpty(Entity.Cust_Code3))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CUST3_CODE", Entity.Cust_Code3));
                if (!string.IsNullOrEmpty(Entity.Cust_Value3))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CUST3_VALUE", Entity.Cust_Value3));
                if (!string.IsNullOrEmpty(Entity.Cust_Code4))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CUST4_CODE", Entity.Cust_Code4));
                if (!string.IsNullOrEmpty(Entity.Cust_Value4))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CUST4_VALUE", Entity.Cust_Value4));
                if (!string.IsNullOrEmpty(Entity.Cust_Code5))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CUST5_CODE", Entity.Cust_Code5));
                if (!string.IsNullOrEmpty(Entity.Cust_Value5))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CUST5_VALUE", Entity.Cust_Value5));
                if (!string.IsNullOrEmpty(Entity.Bulk))
                    sqlParamList.Add(new SqlParameter("@CASEACT_BULK", Entity.Bulk));

                if (!string.IsNullOrEmpty(Entity.Act_PROG))
                    sqlParamList.Add(new SqlParameter("@CASEACT_ACTY_PROG", Entity.Act_PROG));

                sqlParamList.Add(new SqlParameter("@CASEACT_LSTC_OPERATOR", Entity.Lsct_Operator));

                if (!string.IsNullOrEmpty(Entity.Units))
                    sqlParamList.Add(new SqlParameter("@CASEACT_UNITS", Entity.Units));
                sqlParamList.Add(new SqlParameter("@CASEACT_UOM", Entity.UOM));
                if (!string.IsNullOrEmpty(Entity.Curr_Grp))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CURR_GRP", Entity.Curr_Grp));

                if (!string.IsNullOrEmpty(Entity.ActSeek_Date))
                    sqlParamList.Add(new SqlParameter("@CASEACT_SEEK_DATE", Entity.ActSeek_Date));

                if (!string.IsNullOrEmpty(Entity.CA_OBF))
                    sqlParamList.Add(new SqlParameter("@CASEACT_OBF", Entity.CA_OBF));

                if (!string.IsNullOrEmpty(Entity.Rate))
                    sqlParamList.Add(new SqlParameter("@CASEACT_RATE", Decimal.Parse(Entity.Rate)));

                if (!string.IsNullOrEmpty(Entity.Elec_Other))
                    sqlParamList.Add(new SqlParameter("@CASEACT_ELEC_OTHER", Entity.Elec_Other));

                if (Trigger == "CASEACT2.0")
                {
                    if (!string.IsNullOrEmpty(Entity.Amount))
                        sqlParamList.Add(new SqlParameter("@CASEACT_AMOUNT1", Decimal.Parse(Entity.Amount)));

                    if (!string.IsNullOrEmpty(Entity.Amount2))
                        sqlParamList.Add(new SqlParameter("@CASEACT_AMOUNT2", Decimal.Parse(Entity.Amount2)));

                    if (!string.IsNullOrEmpty(Entity.Amount3))
                        sqlParamList.Add(new SqlParameter("@CASEACT_AMOUNT3", Decimal.Parse(Entity.Amount3)));

                    if (!string.IsNullOrEmpty(Entity.Units2))
                        sqlParamList.Add(new SqlParameter("@CASEACT_UNITS2", Entity.Units2));
                    sqlParamList.Add(new SqlParameter("@CASEACT_UOM2", Entity.UOM2));
                    if (!string.IsNullOrEmpty(Entity.Units3))
                        sqlParamList.Add(new SqlParameter("@CASEACT_UNITS3", Entity.Units3));
                    sqlParamList.Add(new SqlParameter("@CASEACT_UOM3", Entity.UOM3));
                }

                if (Trigger == "PAYMENT")
                {
                    if (!string.IsNullOrEmpty(Entity.Amount))
                        sqlParamList.Add(new SqlParameter("@CASEACT_AMOUNT1", Decimal.Parse(Entity.Amount)));

                    if (!string.IsNullOrEmpty(Entity.Amount2))
                        sqlParamList.Add(new SqlParameter("@CASEACT_AMOUNT2", Decimal.Parse(Entity.Amount2)));

                    if (!string.IsNullOrEmpty(Entity.Amount3))
                        sqlParamList.Add(new SqlParameter("@CASEACT_AMOUNT3", Decimal.Parse(Entity.Amount3)));

                    if (!string.IsNullOrEmpty(Entity.Units2))
                        sqlParamList.Add(new SqlParameter("@CASEACT_UNITS2", Entity.Units2));
                    sqlParamList.Add(new SqlParameter("@CASEACT_UOM2", Entity.UOM2));
                    if (!string.IsNullOrEmpty(Entity.Units3))
                        sqlParamList.Add(new SqlParameter("@CASEACT_UNITS3", Entity.Units3));
                    sqlParamList.Add(new SqlParameter("@CASEACT_UOM3", Entity.UOM3));


                    
                    if (!string.IsNullOrEmpty(Entity.BillingPeriod))
                        sqlParamList.Add(new SqlParameter("@CASEACT_BILL_PERIOD", Entity.BillingPeriod));
                    if (!string.IsNullOrEmpty(Entity.Account))
                        sqlParamList.Add(new SqlParameter("@CASEACT_VEND_ACCT", Entity.Account));
                    if (!string.IsNullOrEmpty(Entity.ArrearsAmt))
                        sqlParamList.Add(new SqlParameter("@CASEACT_ARREARS", Decimal.Parse(Entity.ArrearsAmt)));

                    //if (!string.IsNullOrEmpty(Entity.Cost))
                    //    sqlParamList.Add(new SqlParameter("@CASEACT_COST", Decimal.Parse(Entity.Cost)));

                    if (!string.IsNullOrEmpty(Entity.LVL1Apprval))
                        sqlParamList.Add(new SqlParameter("@CASEACT_LVL1_APRVL", Entity.LVL1Apprval));
                    if (!string.IsNullOrEmpty(Entity.LVL1AprrvalDate))
                        sqlParamList.Add(new SqlParameter("@CASEACT_LVL1_APRVL_DATE", Entity.LVL1AprrvalDate));
                    if (!string.IsNullOrEmpty(Entity.LVL2Apprval))
                        sqlParamList.Add(new SqlParameter("@CASEACT_LVL2_APRVL", Entity.LVL2Apprval));
                    if (!string.IsNullOrEmpty(Entity.LVL2ApprvalDate))
                        sqlParamList.Add(new SqlParameter("@CASEACT_LVL2_APRVL_DATE", Entity.LVL2ApprvalDate));
                    if (!string.IsNullOrEmpty(Entity.SentPmtUser))
                        sqlParamList.Add(new SqlParameter("@CASEACT_SENT_PMT_USER", Entity.SentPmtUser));
                    if (!string.IsNullOrEmpty(Entity.SentPmtDate))
                        sqlParamList.Add(new SqlParameter("@CASEACT_SENT_PMT_DATE", Entity.SentPmtDate));
                    if (!string.IsNullOrEmpty(Entity.BundleNo))
                        sqlParamList.Add(new SqlParameter("@CASEACT_BUNDLE_NO", Entity.BundleNo));


                    if (!string.IsNullOrEmpty(Entity.BillngType))
                        sqlParamList.Add(new SqlParameter("@CASEACT_BILL_TYPE", Entity.BillngType));
                    if (!string.IsNullOrEmpty(Entity.BillngFname))
                        sqlParamList.Add(new SqlParameter("@CASEACT_BILL_FNAME", Entity.BillngFname));
                    if (!string.IsNullOrEmpty(Entity.BillngLname))
                        sqlParamList.Add(new SqlParameter("@CASEACT_BILL_LNAME", Entity.BillngLname));

                    if (!string.IsNullOrEmpty(Entity.CA_Source))
                        sqlParamList.Add(new SqlParameter("@CASEACT_SOURCE", Entity.CA_Source));
                    
                    if (!string.IsNullOrEmpty(Entity.BDC_ID))
                        sqlParamList.Add(new SqlParameter("@CASEACT_BDC_ID", Entity.BDC_ID));
                    if (!string.IsNullOrEmpty(Entity.BenefitReason))
                        sqlParamList.Add(new SqlParameter("@CASEACT_BENEFIT_REASN", Entity.BenefitReason));

                    if (!string.IsNullOrEmpty(Entity.PDOUT))
                        sqlParamList.Add(new SqlParameter("@CASEACT_PDOUT", Entity.PDOUT));

                    if (!string.IsNullOrEmpty(Entity.PaymentNo))
                        sqlParamList.Add(new SqlParameter("@CASEACT_PAYMENT_NO", Entity.PaymentNo));



                }
                if(Trigger=="Reject")
                {
                    if (!string.IsNullOrEmpty(Entity.LVL1Apprval))
                        sqlParamList.Add(new SqlParameter("@CASEACT_LVL1_APRVL", Entity.LVL1Apprval));
                    if (!string.IsNullOrEmpty(Entity.LVL1AprrvalDate))
                        sqlParamList.Add(new SqlParameter("@CASEACT_LVL1_APRVL_DATE", Entity.LVL1AprrvalDate));
                    if (!string.IsNullOrEmpty(Entity.LVL2Apprval))
                        sqlParamList.Add(new SqlParameter("@CASEACT_LVL2_APRVL", Entity.LVL2Apprval));
                    if (!string.IsNullOrEmpty(Entity.LVL2ApprvalDate))
                        sqlParamList.Add(new SqlParameter("@CASEACT_LVL2_APRVL_DATE", Entity.LVL2ApprvalDate));
                    if (!string.IsNullOrEmpty(Entity.RejectCode))
                        sqlParamList.Add(new SqlParameter("@CASEACT_REJECT_CODE", Entity.RejectCode));
                    if (!string.IsNullOrEmpty(Entity.RejectBy))
                        sqlParamList.Add(new SqlParameter("@CASEACT_REJECT_BY", Entity.RejectBy));
                    if (!string.IsNullOrEmpty(Entity.RejectDate))
                        sqlParamList.Add(new SqlParameter("@CASEACT_REJECT_DATE", Entity.RejectDate));
                }
                if (Trigger == "Bundle")
                {
                    if (!string.IsNullOrEmpty(Entity.BundleNo))
                        sqlParamList.Add(new SqlParameter("@CASEACT_BUNDLE_NO", Entity.BundleNo));
                    if (!string.IsNullOrEmpty(Entity.SentPmtDate))
                        sqlParamList.Add(new SqlParameter("@CASEACT_SENT_PMT_DATE", Entity.SentPmtDate));
                    if (!string.IsNullOrEmpty(Entity.SentPmtUser))
                        sqlParamList.Add(new SqlParameter("@CASEACT_SENT_PMT_USER", Entity.SentPmtUser));
                    
                }

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@CASEACT_DATE_LSTC", Entity.Lstc_Date));
                    sqlParamList.Add(new SqlParameter("@CASEACT_DATE_ADD", Entity.Add_Date));
                    sqlParamList.Add(new SqlParameter("@CASEACT_ADD_OPERATOR", Entity.Add_Operator));
                }

                if (Trigger == "Trigger")
                {
                    sqlParamList.Add(new SqlParameter("@CASEACT_TRIG_CODE", Entity.ACT_TrigCode));
                    sqlParamList.Add(new SqlParameter("@CASEACT_TRIG_DATE", Entity.ACT_TrigDate));
                    sqlParamList.Add(new SqlParameter("@CASEACT_TRIG_DATE_SEQ", Entity.ACT_TrigDateSeq));
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        //public List<SqlParameter> Prepare_CASEMS_SqlParameters_List(CASEMSEntity Entity, string Opretaion_Mode)
        //{
        //    List<SqlParameter> sqlParamList = new List<SqlParameter>();
        //    try
        //    {
        //        if (Opretaion_Mode != "Browse")
        //            sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));


        //        sqlParamList.Add(new SqlParameter("@CASEMS_AGENCY", Entity.Agency));
        //        sqlParamList.Add(new SqlParameter("@CASEMS_DEPT", Entity.Dept));
        //        sqlParamList.Add(new SqlParameter("@CASEMS_PROGRAM", Entity.Program));
        //        //if (!string.IsNullOrEmpty(Entity.Year))
        //        sqlParamList.Add(new SqlParameter("@CASEMS_YEAR", Entity.Year));
        //        //else
        //        //    sqlParamList.Add(new SqlParameter("@CASEMS_YEAR", "    "));

        //        sqlParamList.Add(new SqlParameter("@CASEMS_APP_NO", Entity.App_no));
        //        if (!string.IsNullOrEmpty(Entity.Service_plan))
        //            sqlParamList.Add(new SqlParameter("@CASEMS_SERVICE_PLAN", Entity.Service_plan));  /// int

        //        sqlParamList.Add(new SqlParameter("@CASEMS_SPM_SEQ", Entity.SPM_Seq));  /// int
        //        sqlParamList.Add(new SqlParameter("@CASEMS_BRANCH", Entity.Branch));
        //        if (!string.IsNullOrEmpty(Entity.Group))
        //            sqlParamList.Add(new SqlParameter("@CASEMS_GROUP", Entity.Group));                // int
        //        sqlParamList.Add(new SqlParameter("@CASEMS_MS_CODE", Entity.MS_Code));
        //        if (!string.IsNullOrEmpty(Entity.ID))
        //            sqlParamList.Add(new SqlParameter("@CASEMS_ID", Entity.ID));                     // int
        //        sqlParamList.Add(new SqlParameter("@CASEMS_DATE", Entity.Date));
        //        sqlParamList.Add(new SqlParameter("@CASEMS_CASEWORKER", Entity.CaseWorker));
        //        sqlParamList.Add(new SqlParameter("@CASEMS_SITE", Entity.Site));
        //        sqlParamList.Add(new SqlParameter("@CASEMS_RESULT", Entity.Result));
        //        sqlParamList.Add(new SqlParameter("@CASEMS_OBF", Entity.OBF));
        //        sqlParamList.Add(new SqlParameter("@CASEMS_BULK", Entity.Bulk));
        //        sqlParamList.Add(new SqlParameter("@CASEMS_LSTC_OPERATOR", Entity.Lsct_Operator));
        //        sqlParamList.Add(new SqlParameter("@CASEMS_ACTY_PROG", Entity.Acty_PROG));
        //        if (!string.IsNullOrEmpty(Entity.Curr_Grp))
        //            sqlParamList.Add(new SqlParameter("@CASEMS_CURR_GRP", Entity.Curr_Grp));

        //        if (Opretaion_Mode == "Browse")
        //        {
        //            sqlParamList.Add(new SqlParameter("@CASEMS_DATE_LSTC", Entity.Lstc_Date));
        //            sqlParamList.Add(new SqlParameter("@CASEMS_DATE_ADD", Entity.Add_Date));
        //            sqlParamList.Add(new SqlParameter("@CASEMS_ADD_OPERATOR", Entity.Add_Operator));
        //        }

        //    }
        //    catch (Exception ex)
        //    { return sqlParamList; }

        //    return sqlParamList;
        //}

        public List<SqlParameter> Prepare_CASEMS_SqlParameters_List(CASEMSEntity Entity, string Opretaion_Mode, string Trigger)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));


                sqlParamList.Add(new SqlParameter("@CASEMS_AGENCY", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@CASEMS_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@CASEMS_PROGRAM", Entity.Program));
                //if (!string.IsNullOrEmpty(Entity.Year))
                sqlParamList.Add(new SqlParameter("@CASEMS_YEAR", Entity.Year));
                //else
                //    sqlParamList.Add(new SqlParameter("@CASEMS_YEAR", "    "));

                sqlParamList.Add(new SqlParameter("@CASEMS_APP_NO", Entity.App_no));
                if (!string.IsNullOrEmpty(Entity.Service_plan))
                    sqlParamList.Add(new SqlParameter("@CASEMS_SERVICE_PLAN", Entity.Service_plan));  /// int

                sqlParamList.Add(new SqlParameter("@CASEMS_SPM_SEQ", Entity.SPM_Seq));  /// int
                sqlParamList.Add(new SqlParameter("@CASEMS_BRANCH", Entity.Branch));
                if (!string.IsNullOrEmpty(Entity.Group))
                    sqlParamList.Add(new SqlParameter("@CASEMS_GROUP", Entity.Group));                // int
                sqlParamList.Add(new SqlParameter("@CASEMS_MS_CODE", Entity.MS_Code));
                if (!string.IsNullOrEmpty(Entity.ID))
                    sqlParamList.Add(new SqlParameter("@CASEMS_ID", Entity.ID));                     // int
                sqlParamList.Add(new SqlParameter("@CASEMS_DATE", Entity.Date));
                sqlParamList.Add(new SqlParameter("@CASEMS_CASEWORKER", Entity.CaseWorker));
                sqlParamList.Add(new SqlParameter("@CASEMS_SITE", Entity.Site));
                sqlParamList.Add(new SqlParameter("@CASEMS_RESULT", Entity.Result));
                sqlParamList.Add(new SqlParameter("@CASEMS_OBF", Entity.OBF));
                sqlParamList.Add(new SqlParameter("@CASEMS_BULK", Entity.Bulk));
                sqlParamList.Add(new SqlParameter("@CASEMS_LSTC_OPERATOR", Entity.Lsct_Operator));
                sqlParamList.Add(new SqlParameter("@CASEMS_ACTY_PROG", Entity.Acty_PROG));
                if (!string.IsNullOrEmpty(Entity.Curr_Grp))
                    sqlParamList.Add(new SqlParameter("@CASEMS_CURR_GRP", Entity.Curr_Grp));
                if (!string.IsNullOrEmpty(Entity.Seek_Date))
                    sqlParamList.Add(new SqlParameter("@CASEMS_SEEK_DATE", Entity.Seek_Date));

                if (!string.IsNullOrEmpty(Entity.MS_Fund1))
                    sqlParamList.Add(new SqlParameter("@CASEMS_FUND1", Entity.MS_Fund1));
                if (!string.IsNullOrEmpty(Entity.MS_Fund2))
                    sqlParamList.Add(new SqlParameter("@CASEMS_FUND2", Entity.MS_Fund2));
                if (!string.IsNullOrEmpty(Entity.MS_Fund3))
                    sqlParamList.Add(new SqlParameter("@CASEMS_FUND3", Entity.MS_Fund3));

                if (!string.IsNullOrEmpty(Entity.MS_FUP_Date))
                    sqlParamList.Add(new SqlParameter("@CASEMS_FUP_DATE", Entity.MS_FUP_Date));     // Added by Vikash 12/30/2022 for Followup issue(Brought update)
                if (!string.IsNullOrEmpty(Entity.MS_Comp_Date))
                    sqlParamList.Add(new SqlParameter("@CASEMS_COMP_DATE", Entity.MS_Comp_Date));

                if (!string.IsNullOrEmpty(Entity.MS_Amount))
                    sqlParamList.Add(new SqlParameter("@CASEMS_AMOUNT", Entity.MS_Amount));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@CASEMS_DATE_LSTC", Entity.Lstc_Date));
                    sqlParamList.Add(new SqlParameter("@CASEMS_DATE_ADD", Entity.Add_Date));
                    sqlParamList.Add(new SqlParameter("@CASEMS_ADD_OPERATOR", Entity.Add_Operator));
                }
                if (Trigger == "Trigger")
                {
                    sqlParamList.Add(new SqlParameter("@CASEMS_TRIG_CODE", Entity.MS_TrigCode));
                    sqlParamList.Add(new SqlParameter("@CASEMS_TRIG_DATE", Entity.MS_TrigDate));
                    sqlParamList.Add(new SqlParameter("@CASEMS_TRIG_DATE_SEQ", Entity.MS_TrigDateSeq));
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<CASESPM2Entity> Browse_CASESPM2(CASESPM2Entity Entity, string Opretaion_Mode)
        {
            List<CASESPM2Entity> CASESPM2Profile = new List<CASESPM2Entity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASESPM2_SqlParameters_List(Entity, Opretaion_Mode, string.Empty);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASESPM2(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPM2Profile.Add(new CASESPM2Entity(row));
                }
            }
            catch (Exception ex)
            { return CASESPM2Profile; }

            return CASESPM2Profile;
        }

        public bool UpdateCASESPM2(CASESPM2Entity Entity, string Operation_Mode, out string Sql_Reslut_Msg, string RowChange) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_CASESPM2_SqlParameters_List(Entity, Operation_Mode, RowChange);
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateCASESPM2", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }



        public List<CASEREFSEntity> Browse_CASEREFS(CASEREFSEntity Entity, string Opretaion_Mode)
        {
            List<CASEREFSEntity> CASESPM2Profile = new List<CASEREFSEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASEREFS_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CASEREFS]");

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPM2Profile.Add(new CASEREFSEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPM2Profile; }

            return CASESPM2Profile;
        }

        public List<TRIGSummaryEntity> Browse_TrigSummary(string Trig_code, string strFromDate, string strTodate)
        {
            List<TRIGSummaryEntity> CASESPM2Profile = new List<TRIGSummaryEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Trig_code != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRIG_CODE", Trig_code));

                if (strFromDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRIG_FROMDATE", strFromDate));

                if (strTodate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRIG_TODATE", strTodate));


                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[BROWSE_TRIG_SUMMARY]");

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPM2Profile.Add(new TRIGSummaryEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPM2Profile; }

            return CASESPM2Profile;
        }

        public List<TRIGDetailEntity> Browse_TrigDetails(TRIGSummaryEntity Entity, string Sw)
        {
            List<TRIGDetailEntity> CASESPM2Profile = new List<TRIGDetailEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {

                sqlParamList.Add(new SqlParameter("@TrigCode", Entity.Trig_Code));
                if (!string.IsNullOrEmpty(Entity.Trig_Date.Trim()))
                    sqlParamList.Add(new SqlParameter("@TrigDate", Entity.Trig_Date));
                if (!string.IsNullOrEmpty(Entity.Trig_Date_Seq.Trim()))
                    sqlParamList.Add(new SqlParameter("@TrigSeq", Entity.Trig_Date_Seq));
                sqlParamList.Add(new SqlParameter("@Switch", Sw));


                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Trig_Bulk_Details]");

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPM2Profile.Add(new TRIGDetailEntity(row, Sw));
                }
            }
            catch (Exception ex)
            { return CASESPM2Profile; }

            return CASESPM2Profile;
        }

        public List<ACTREFSEntity> Browse_ACTREFS(ACTREFSEntity Entity, string Opretaion_Mode)
        {
            List<ACTREFSEntity> CASESPM2Profile = new List<ACTREFSEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_ACTREFS_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_ACTREFS]");

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPM2Profile.Add(new ACTREFSEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPM2Profile; }

            return CASESPM2Profile;
        }

        public bool UpdateACTREFS(ACTREFSEntity Entity, string Operation_Mode, out string Sql_SP_Result_Message)
        {
            bool boolsuccess;
            Sql_SP_Result_Message = Consts.Common.DB_Exception;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_ACTREFS_SqlParameters_List(Entity, Operation_Mode);
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateACTREFS", out Sql_SP_Result_Message);  //
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<SqlParameter> Prepare_ACTREFS_SqlParameters_List(ACTREFSEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@ACTREFS_AGENCY", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@ACTREFS_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@ACTREFS_PROGRAM", Entity.Program));
                sqlParamList.Add(new SqlParameter("@ACTREFS_YEAR", Entity.Year));
                sqlParamList.Add(new SqlParameter("@ACTREFS_APP_NO", Entity.ApplNo));
                sqlParamList.Add(new SqlParameter("@ACTREFS_DATE", Entity.Date));
                sqlParamList.Add(new SqlParameter("@ACTREFS_CODE", Entity.Code));
                if(!string.IsNullOrEmpty(Entity.Type))
                    sqlParamList.Add(new SqlParameter("@ACTREFS_TYPE", Entity.Type));
                if (!string.IsNullOrEmpty(Entity.Connected))
                    sqlParamList.Add(new SqlParameter("@ACTREFS_CONNECTED", Entity.Connected));
                
                if (!string.IsNullOrEmpty(Entity.Lsct_Operator))
                    sqlParamList.Add(new SqlParameter("@ACTREFS_LSTC_OPERATOR", Entity.Lsct_Operator));
                
                if (!string.IsNullOrEmpty(Entity.Add_Operator))
                    sqlParamList.Add(new SqlParameter("@ACTREFS_ADD_OPERATOR", Entity.Add_Operator));
                if (!string.IsNullOrEmpty(Entity.NameIndex))
                    sqlParamList.Add(new SqlParameter("@ACTREFS_NAMEINDEX", Entity.NameIndex));

                if (Opretaion_Mode == "Browse")
                {
                    if (!string.IsNullOrEmpty(Entity.Add_Date))
                        sqlParamList.Add(new SqlParameter("@ACTREFS_DATE_ADD", Entity.Add_Date));
                    if (!string.IsNullOrEmpty(Entity.Lstc_Date))
                        sqlParamList.Add(new SqlParameter("@ACTREFS_DATE_LSTC", Entity.Lstc_Date));
                }

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<PARTNEREFEntity> Browse_PARTNEREF(PARTNEREFEntity Entity, string Opretaion_Mode)
        {
            List<PARTNEREFEntity> CASESPM2Profile = new List<PARTNEREFEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_PARTNEREF_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[CAPS_PARTNEREF_BROWSE]");

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPM2Profile.Add(new PARTNEREFEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPM2Profile; }

            return CASESPM2Profile;
        }

        public bool UpdatePartnerRef(PARTNEREFEntity Entity, string Operation_Mode, out string Sql_SP_Result_Message)
        {
            bool boolsuccess;
            Sql_SP_Result_Message = Consts.Common.DB_Exception;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_PARTNEREF_SqlParameters_List(Entity, Operation_Mode);
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.CAPS_PARTNEREF_INSERT", out Sql_SP_Result_Message);  //
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<SqlParameter> Prepare_PARTNEREF_SqlParameters_List(PARTNEREFEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                if (!string.IsNullOrEmpty(Entity.RefID))
                    sqlParamList.Add(new SqlParameter("@PREF_ID", Entity.RefID.Trim()));
                if (!string.IsNullOrEmpty(Entity.Agency))
                    sqlParamList.Add(new SqlParameter("@PREF_AGENCY", Entity.Agency));
                if (!string.IsNullOrEmpty(Entity.Dept))
                    sqlParamList.Add(new SqlParameter("@PREF_DEPT", Entity.Dept));
                if (!string.IsNullOrEmpty(Entity.Program))
                    sqlParamList.Add(new SqlParameter("@PREF_PROGRAM", Entity.Program));
                //if (!string.IsNullOrEmpty(Entity.Year.Trim()))
                 sqlParamList.Add(new SqlParameter("@PREF_YEAR", Entity.Year));
                if (!string.IsNullOrEmpty(Entity.ApplNo))
                    sqlParamList.Add(new SqlParameter("@PREF_APP_NO", Entity.ApplNo));
                if (!string.IsNullOrEmpty(Entity.Date))
                    sqlParamList.Add(new SqlParameter("@PREF_DATE", Entity.Date));
                if (!string.IsNullOrEmpty(Entity.Code))
                    sqlParamList.Add(new SqlParameter("@PREF_CODE", Entity.Code));
                if (!string.IsNullOrEmpty(Entity.Type))
                    sqlParamList.Add(new SqlParameter("@PREF_TYPE", Entity.Type));
                if (!string.IsNullOrEmpty(Entity.REPRESENTATIVE))
                    sqlParamList.Add(new SqlParameter("@PREF_REPRESENTATIVE", Entity.REPRESENTATIVE));
                if (!string.IsNullOrEmpty(Entity.FName))
                    sqlParamList.Add(new SqlParameter("@PREF_REP_FNAME", Entity.FName));
                if (!string.IsNullOrEmpty(Entity.LName))
                    sqlParamList.Add(new SqlParameter("@PREF_REP_LNAME", Entity.LName));
                if (!string.IsNullOrEmpty(Entity.Category))
                    sqlParamList.Add(new SqlParameter("@PREF_CATEGORY", Entity.Category));
                if (!string.IsNullOrEmpty(Entity.Status))
                    sqlParamList.Add(new SqlParameter("@PREF_STATUS", Entity.Status));
                if (!string.IsNullOrEmpty(Entity.RefExpDate))
                    sqlParamList.Add(new SqlParameter("@PREF_REFEXP_DATE", Entity.RefExpDate));

                if (!string.IsNullOrEmpty(Entity.StatusDate))
                    sqlParamList.Add(new SqlParameter("@PREF_STATUS_DATE", Entity.StatusDate));
                
                if (!string.IsNullOrEmpty(Entity.Lsct_Operator))
                    sqlParamList.Add(new SqlParameter("@PREF_LSTC_OPERATOR", Entity.Lsct_Operator));
                
                if (!string.IsNullOrEmpty(Entity.Add_Operator))
                    sqlParamList.Add(new SqlParameter("@PREF_ADD_OPERATOR", Entity.Add_Operator));

                if (!string.IsNullOrEmpty(Entity.Service))
                    sqlParamList.Add(new SqlParameter("@PREF_SERVICE", Entity.Service));

                if (Opretaion_Mode == "Browse")
                {
                    if (!string.IsNullOrEmpty(Entity.Add_Date))
                        sqlParamList.Add(new SqlParameter("@PREF_DATE_ADD", Entity.Add_Date));
                    if (!string.IsNullOrEmpty(Entity.Lstc_Date))
                        sqlParamList.Add(new SqlParameter("@PREF_DATE_LSTC", Entity.Lstc_Date));
                }


            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }


        public List<SqlParameter> Prepare_CASESPM2_SqlParameters_List(CASESPM2Entity Entity, string Opretaion_Mode, string RowChange)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (!string.IsNullOrEmpty(RowChange.Trim()))
                    sqlParamList.Add(new SqlParameter("@Operation", RowChange));

                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@spm2_agy", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@spm2_dept", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@spm2_prog", Entity.Prog));
                sqlParamList.Add(new SqlParameter("@spm2_year", Entity.Year));
                sqlParamList.Add(new SqlParameter("@spm2_app_no", Entity.App));
                if (!string.IsNullOrEmpty(Entity.ServPlan))
                    sqlParamList.Add(new SqlParameter("@spm2_serviceplan", Entity.ServPlan));

                sqlParamList.Add(new SqlParameter("@Spm2_SEQ", Entity.Spm_Seq));

                sqlParamList.Add(new SqlParameter("@spm2_branch", Entity.Branch));
                if (!string.IsNullOrEmpty(Entity.Group))
                    sqlParamList.Add(new SqlParameter("@spm2_group", Entity.Group));
                sqlParamList.Add(new SqlParameter("@spm2_cams_code", Entity.CamCd));
                sqlParamList.Add(new SqlParameter("@spm2_type", Entity.Type1));
                if (!string.IsNullOrEmpty(Entity.Curr_Group))
                    sqlParamList.Add(new SqlParameter("@spm2_curr_grp", Entity.Curr_Group));
                if (!string.IsNullOrEmpty(Entity.SelOrdinal))
                    sqlParamList.Add(new SqlParameter("@spm2_ordinal", Entity.SelOrdinal));
                sqlParamList.Add(new SqlParameter("@spm2_lstc_operator", Entity.lstcOperator));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@spm2_date_lstc", Entity.DateLstc));
                    sqlParamList.Add(new SqlParameter("@spm2_date_add", Entity.Dateadd));
                    sqlParamList.Add(new SqlParameter("@spm2_add_operator", Entity.addoperator));
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }


        /// <summary>
        /// Get Image Types information. 
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<SPCommonEntity> Get_AgyRecs(string Fld_Name)
        {
            string Agy_Type = Fld_Name;
            switch (Fld_Name)
            {
                case "Results": Agy_Type = Consts.AgyTab.RESULTS; break;
                case "Funding": Agy_Type = Consts.AgyTab.CASEMNGMTFUNDSRC; break;
                case "Issues": Agy_Type = Consts.AgyTab.LEGALISSUES; break;
                case "Goals": Agy_Type = Consts.AgyTab.GOALS; break;
                case "HowWhere": Agy_Type = Consts.AgyTab.CONTACTHOWHERE; break;
                case "UOM": Agy_Type = Consts.AgyTab.UOMTABLE; break;
                case "COMPONENTS": Agy_Type = Consts.AgyTab.COMPONENTS; break;
            }

            List<SPCommonEntity> lookUpEntity = new List<SPCommonEntity>();
            try
            {
                DataSet lookUpData = Lookups.GetLookUpFromAGYTAB(Agy_Type);
                if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in lookUpData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new SPCommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            { return lookUpEntity; }

            return lookUpEntity;
        }


        /// <summary>
        /// Get Image Types information. 
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<SPCommonEntity> Get_AgyRecs_WithFilter(string Fld_Name, string Filter)
        {
            string Agy_Type = Fld_Name;
            switch (Fld_Name)
            {
                case "Results": Agy_Type = Consts.AgyTab.RESULTS; break;
                case "Funding": Agy_Type = Consts.AgyTab.CASEMNGMTFUNDSRC; break;
                case "Issues": Agy_Type = Consts.AgyTab.LEGALISSUES; break;
                case "Goals": Agy_Type = Consts.AgyTab.GOALS; break;
                case "HowWhere": Agy_Type = Consts.AgyTab.CONTACTHOWHERE; break;
                case "UOM": Agy_Type = Consts.AgyTab.UOMTABLE; break;
                case "COMPONENTS": Agy_Type = Consts.AgyTab.COMPONENTS; break;
            }

            List<SPCommonEntity> lookUpEntity = new List<SPCommonEntity>();
            try
            {
                DataSet lookUpData = Lookups.GetLookUpFromAGYTAB(Agy_Type, Filter);
                if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
                {
                    if (Agy_Type == "03341")
                    {
                        foreach (DataRow row in lookUpData.Tables[0].Rows)
                        {
                            lookUpEntity.Add(new SPCommonEntity(row,string.Empty,Agy_Type));
                        }
                    }
                    else
                    {
                        foreach (DataRow row in lookUpData.Tables[0].Rows)
                        {
                            lookUpEntity.Add(new SPCommonEntity(row));
                        }
                    }
                }
            }
            catch (Exception ex)
            { return lookUpEntity; }

            return lookUpEntity;
        }

        /// <summary>
        /// Get_AgyRecs_With_Ext
        /// <returns>Returns Agytab Childs With Extension Columns.</returns>
        public List<Agy_Ext_Entity> Get_AgyRecs_With_Ext(string Fld_Name, string Ext_1, string Ext_2, string Ext_A1, string Ext_A2)
        {
            string Agy_Type = Fld_Name;
            switch (Fld_Name)
            {
                case "Results": Agy_Type = Consts.AgyTab.RESULTS; break;
                case "Funding": Agy_Type = Consts.AgyTab.CASEMNGMTFUNDSRC; break;
                case "Issues": Agy_Type = Consts.AgyTab.LEGALISSUES; break;
                case "Goals": Agy_Type = Consts.AgyTab.GOALS; break;
                case "HowWhere": Agy_Type = Consts.AgyTab.CONTACTHOWHERE; break;
                case "UOM": Agy_Type = Consts.AgyTab.UOMTABLE; break;
                case "COMPONENTS": Agy_Type = Consts.AgyTab.COMPONENTS; break;
            }

            List<Agy_Ext_Entity> lookUpEntity = new List<Agy_Ext_Entity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@Agy_Type", Agy_Type));
                sqlParamList.Add(new SqlParameter("@Agy_Ext_1", Ext_1));
                sqlParamList.Add(new SqlParameter("@Agy_Ext_2", Ext_2));
                sqlParamList.Add(new SqlParameter("@Agy_Ext_3", null));

                sqlParamList.Add(new SqlParameter("@Agy_Cell1_A", Ext_A1));
                sqlParamList.Add(new SqlParameter("@Agy_Cell2_A", Ext_A2));

                DataSet lookUpData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[GetLookUpValues_Ext]");
                if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in lookUpData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new Agy_Ext_Entity(row));
                    }
                }
            }
            catch (Exception ex)
            { return lookUpEntity; }

            return lookUpEntity;
        }



        /// <summary>
        /// Get Image Types information. 
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<SPCommonEntity> GetLookUpFromAGYTAB(string Agy_Type, string Filter_Type)
        {
            List<SPCommonEntity> lookUpEntity = new List<SPCommonEntity>();
            try
            {
                DataSet lookUpData = Lookups.GetLookUpFromAGYTAB(Agy_Type, Filter_Type);
                if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in lookUpData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new SPCommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            { return lookUpEntity; }

            return lookUpEntity;
        }

        /// <summary>
        /// Get Image Types information. 
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<SPCommonEntity> GetLookUpFromAGYTAB_EXT(string Agy_Type, string Filter_Type)
        {
            List<SPCommonEntity> lookUpEntity = new List<SPCommonEntity>();
            try
            {
                DataSet lookUpData = Lookups.GetLookUpFromAGYTAB(Agy_Type, Filter_Type);
                if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in lookUpData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new SPCommonEntity(row, row["EXT"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            { return lookUpEntity; }

            return lookUpEntity;
        }


        public bool UpdateCASESP2(CASESP2Entity Entity, string Row_Change, out string Sql_Reslut_Msg)
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(Row_Change))
                    sqlParamList.Add(new SqlParameter("@Operation", Row_Change));
                sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));
                sqlParamList.Add(new SqlParameter("@sp2_serviceplan", Entity.ServPlan));
                sqlParamList.Add(new SqlParameter("@sp2_branch", Entity.Branch));
                sqlParamList.Add(new SqlParameter("@sp2_orig_grp", Entity.Orig_Grp));
                sqlParamList.Add(new SqlParameter("@sp2_type", Entity.Type1));
                sqlParamList.Add(new SqlParameter("@sp2_cams_code", Entity.CamCd));
                sqlParamList.Add(new SqlParameter("@sp2_row", Entity.Row));
                sqlParamList.Add(new SqlParameter("@sp2_curr_grp", Entity.Curr_Grp));
                sqlParamList.Add(new SqlParameter("@sp2_Active", Entity.SP2_CAMS_Active));
                sqlParamList.Add(new SqlParameter("@sp2_lstc_operator", Entity.lstcOperator));
                sqlParamList.Add(new SqlParameter("@sp2_Auto_post", Entity.SP2_Auto_Post));
                sqlParamList.Add(new SqlParameter("@SP2_SPL_INS", Entity.SP2_Spl_Ins));
                sqlParamList.Add(new SqlParameter("@SP2_OBF", Entity.SP2_OBF));
                sqlParamList.Add(new SqlParameter("@SP2_COUNT_TYPE", Entity.SP2_COUNT_TYPE));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateCASESP2", out Sql_Reslut_Msg);  //


            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateCASESP1(CASESP1Entity Entity, string Xml_Hierarchies, out string Sql_Reslut_Msg)
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {

                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));
                sqlParamList.Add(new SqlParameter("@sp1_servicecode", Entity.Code));
                sqlParamList.Add(new SqlParameter("@sp1_agency", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@sp1_dept", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@sp1_program", Entity.Prog));
                sqlParamList.Add(new SqlParameter("@sp1_lstc_operator", Entity.lstcOperator));
                sqlParamList.Add(new SqlParameter("@Hierarchies_XML", Xml_Hierarchies));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateCASESP1", out Sql_Reslut_Msg);  //

            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateCASESP0(CASESP0Entity Entity, out int New_Sp_Code, out string Sql_Reslut_Msg)
        {
            bool boolsuccess = true;
            Sql_Reslut_Msg = "Success";
            New_Sp_Code = 0;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));
                sqlParamList.Add(new SqlParameter("@sp0_servicecode", Entity.Code));
                sqlParamList.Add(new SqlParameter("@sp0_description", Entity.Desc));
                sqlParamList.Add(new SqlParameter("@sp0_Active", Entity.Active));
                sqlParamList.Add(new SqlParameter("@sp0_Allow_AdlBranch", Entity.Allow_Add_Branch));
                sqlParamList.Add(new SqlParameter("@sp0_Def_Prog", Entity.Default_Prog));
                sqlParamList.Add(new SqlParameter("@sp0_Allow_Dups", Entity.Allow_Duplicates));
                sqlParamList.Add(new SqlParameter("@sp0_pbranch_code", Entity.BpCode));
                sqlParamList.Add(new SqlParameter("@sp0_pbranch_desc", Entity.BpDesc));
                sqlParamList.Add(new SqlParameter("@sp0_branch1_code", Entity.B1Code));
                sqlParamList.Add(new SqlParameter("@sp0_branch1_desc", Entity.B1Desc));
                sqlParamList.Add(new SqlParameter("@sp0_branch2_code", Entity.B2Code));
                sqlParamList.Add(new SqlParameter("@sp0_branch2_desc", Entity.B2Desc));
                sqlParamList.Add(new SqlParameter("@sp0_branch3_code", Entity.B3Code));
                sqlParamList.Add(new SqlParameter("@sp0_branch3_desc", Entity.B3Desc));
                sqlParamList.Add(new SqlParameter("@sp0_branch4_code", Entity.B4Code));
                sqlParamList.Add(new SqlParameter("@sp0_branch4_desc", Entity.B4Desc));
                sqlParamList.Add(new SqlParameter("@sp0_branch5_code", Entity.B5Code));
                sqlParamList.Add(new SqlParameter("@sp0_branch5_desc", Entity.B5Desc));
                sqlParamList.Add(new SqlParameter("@sp0_statuses", Entity.Status));
                sqlParamList.Add(new SqlParameter("@sp0_legals", Entity.Legals));
                sqlParamList.Add(new SqlParameter("@sp0_funds", Entity.Funds));
                sqlParamList.Add(new SqlParameter("@sp0_validated", Entity.Validate));
                sqlParamList.Add(new SqlParameter("@sp0_lstc_operator", Entity.lstcOperator));
                sqlParamList.Add(new SqlParameter("@SP0_ALLOW_AUTOPOST", Entity.Auto_Post_SP));
                sqlParamList.Add(new SqlParameter("@sp0_ReadOnly", Entity.Sp0ReadOnly));
                sqlParamList.Add(new SqlParameter("@sp0_Notes", Entity.Sp0Notes));
                sqlParamList.Add(new SqlParameter("@SP0_Category", Entity.Category));
                sqlParamList.Add(new SqlParameter("@SP0_NOSPM", Entity.NoSPM));
                sqlParamList.Add(new SqlParameter("@SP0_BULK_ENTRY", Entity.BulkEntry));
                sqlParamList.Add(new SqlParameter("@SP0_HH_TYPE", Entity.Household_Type));
                if (!string.IsNullOrEmpty(Entity.Maxbenefit.Trim()))
                    sqlParamList.Add(new SqlParameter("@SP0_MAX_BEN", Entity.Maxbenefit));
                if (!string.IsNullOrEmpty(Entity.Transactions.Trim()))
                    sqlParamList.Add(new SqlParameter("@SP0_NO_TRANS", Entity.Transactions));

                if (!string.IsNullOrEmpty(Entity.PartnerPostingType.Trim()))
                    sqlParamList.Add(new SqlParameter("@SP0_PPTYPE", Entity.PartnerPostingType));

                SqlParameter parameterMsg = new SqlParameter("@sp0_New_SP_Code", SqlDbType.Int, 5);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateCASESP0", out Sql_Reslut_Msg);  //

                New_Sp_Code = int.Parse(parameterMsg.Value.ToString());
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool Fix_Current_Groups_In_CAMS_Postings(string SP_Code, out string Sql_Reslut_Msg)
        {
            bool boolsuccess = true;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@SP_Code", SP_Code));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.Fix_Current_Groups_In_CAMS_Postings", out Sql_Reslut_Msg);  //
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }


        public bool UpdateCASESPM(CASESPMEntity Entity, string Operation_Mode, out string Sql_Reslut_Msg, out string SPM_Sequence) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            SPM_Sequence = "1";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_CASESPM_SqlParameters_List(Entity, Operation_Mode, null);

                SqlParameter Sequence = new SqlParameter("@Spm_New_Sequence", SqlDbType.VarChar, 50);
                Sequence.Direction = ParameterDirection.Output;
                sqlParamList.Add(Sequence);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateCASESPM", out Sql_Reslut_Msg);

                SPM_Sequence = Sequence.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }


        public bool Delete_CASESP2(string SP_Code, string Branch_Code, int Orig_Grp, string Type1, string CamCd)
        {
            try
            { Captain.DatabaseLayer.SPAdminDB.Delete_CASESP2(int.Parse(SP_Code), Branch_Code, Orig_Grp, Type1, CamCd); }

            catch (Exception ex)
            { return false; }

            return true;
        }

        public bool Delete_CaseRefs(string Code)
        {
            try
            { Captain.DatabaseLayer.SPAdminDB.Delete_CaseRefs(Code); }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        public bool UpdateCASECONT(CASECONTEntity Entity, string strProgramCode, string Operation_Mode, out int New_Cont_Seq, out string Sql_SP_Result_Message)
        {
            bool boolsuccess;
            Sql_SP_Result_Message = Consts.Common.DB_Exception;
            New_Cont_Seq = 0;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_CASECONT_SqlParameters_List(Entity, strProgramCode, Operation_Mode);
                SqlParameter parameterMsg = new SqlParameter("@CONT_New_SEQ", SqlDbType.Int, 5);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);


                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateCASECONT", out Sql_SP_Result_Message);  //
                New_Cont_Seq = int.Parse(parameterMsg.Value.ToString());
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateCASEACT(CASEACTEntity Entity, string Operation_Mode, out int New_CAID, out int New_CA_Seq, out string Sql_SP_Result_Message)
        {
            bool boolsuccess;
            Sql_SP_Result_Message = Consts.Common.DB_Exception;
            New_CAID = New_CA_Seq = 1;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_CASEACT_SqlParameters_List(Entity, Operation_Mode, string.Empty);

                SqlParameter parameterMsg = new SqlParameter("@CASEACT_New_ID", SqlDbType.Int, 1);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);

                SqlParameter parameterMsg1 = new SqlParameter("@CASEACT_New_Seq", SqlDbType.Int, 1);
                parameterMsg1.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg1);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateCASEACT", out Sql_SP_Result_Message);  //  08202012
                New_CAID = int.Parse(parameterMsg.Value.ToString());
                New_CA_Seq = int.Parse(parameterMsg1.Value.ToString());
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateCASEACT2(CASEACTEntity Entity, string Operation_Mode, out int New_CAID, out int New_CA_Seq, out string Sql_SP_Result_Message)
        {
            bool boolsuccess;
            Sql_SP_Result_Message = Consts.Common.DB_Exception;
            New_CAID = New_CA_Seq = 1;
            try
            {
                 List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_CASEACT2_SqlParameters_List(Entity, Operation_Mode, "CASEACT2.0");

                SqlParameter parameterMsg = new SqlParameter("@CASEACT_New_ID", SqlDbType.Int, 1);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);

                SqlParameter parameterMsg1 = new SqlParameter("@CASEACT_New_Seq", SqlDbType.Int, 1);
                parameterMsg1.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg1);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateCASEACT2", out Sql_SP_Result_Message);  //  08202012
                New_CAID = int.Parse(parameterMsg.Value.ToString());
                New_CA_Seq = int.Parse(parameterMsg1.Value.ToString());
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateCASEACT3(CASEACTEntity Entity, string Operation_Mode, out int New_CAID, out int New_CA_Seq, out string Sql_SP_Result_Message)
        {
            bool boolsuccess;
            Sql_SP_Result_Message = Consts.Common.DB_Exception;
            New_CAID = New_CA_Seq = 1;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_CASEACT2_SqlParameters_List(Entity, Operation_Mode, "PAYMENT");

                SqlParameter parameterMsg = new SqlParameter("@CASEACT_New_ID", SqlDbType.Int, 1);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);

                SqlParameter parameterMsg1 = new SqlParameter("@CASEACT_New_Seq", SqlDbType.Int, 1);
                parameterMsg1.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg1);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateCASEACT2", out Sql_SP_Result_Message);  //  08202012
                New_CAID = int.Parse(parameterMsg.Value.ToString());
                New_CA_Seq = int.Parse(parameterMsg1.Value.ToString());
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateCASEACT3(CASEACTEntity Entity, string Operation_Mode, out int New_CAID, out int New_CA_Seq, out string Sql_SP_Result_Message,string strTrigger)
        {
            bool boolsuccess;
            Sql_SP_Result_Message = Consts.Common.DB_Exception;
            New_CAID = New_CA_Seq = 1;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_CASEACT2_SqlParameters_List(Entity, Operation_Mode, strTrigger);

                SqlParameter parameterMsg = new SqlParameter("@CASEACT_New_ID", SqlDbType.Int, 1);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);

                SqlParameter parameterMsg1 = new SqlParameter("@CASEACT_New_Seq", SqlDbType.Int, 1);
                parameterMsg1.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg1);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateCASEACT2", out Sql_SP_Result_Message);  //  08202012
                New_CAID = int.Parse(parameterMsg.Value.ToString());
                New_CA_Seq = int.Parse(parameterMsg1.Value.ToString());
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateCACheckProcess(CASEACTEntity Entity, string Operation_Mode, string Sql_SP_Result_Message)
        {
            bool boolsuccess;
            Sql_SP_Result_Message = Consts.Common.DB_Exception;
            //New_CAID = New_CA_Seq = 1;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_CACheck_SqlParameters_List(Entity, Operation_Mode, "PAID");

                //SqlParameter parameterMsg = new SqlParameter("@CASEACT_New_ID", SqlDbType.Int, 1);
                //parameterMsg.Direction = ParameterDirection.Output;
                //sqlParamList.Add(parameterMsg);

                //SqlParameter parameterMsg1 = new SqlParameter("@CASEACT_New_Seq", SqlDbType.Int, 1);
                //parameterMsg1.Direction = ParameterDirection.Output;
                //sqlParamList.Add(parameterMsg1);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.CA_CHECK_UPDATE", out Sql_SP_Result_Message);  //  08202012
                //New_CAID = int.Parse(parameterMsg.Value.ToString());
                //New_CA_Seq = int.Parse(parameterMsg1.Value.ToString());
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<SqlParameter> Prepare_CACheck_SqlParameters_List(CASEACTEntity Entity, string Opretaion_Mode, string Trigger)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {

                sqlParamList.Add(new SqlParameter("@CASEACT_AGENCY", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@CASEACT_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@CASEACT_PROGRAM", Entity.Program));
                sqlParamList.Add(new SqlParameter("@CASEACT_YEAR", Entity.Year));

                //if (!string.IsNullOrEmpty(Entity.Vendor_No))
                //    sqlParamList.Add(new SqlParameter("@CASEACT_VENDOR_NO", Entity.Vendor_No));
                if (!string.IsNullOrEmpty(Entity.Check_Date))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CHECK_DT", Entity.Check_Date));
                if (!string.IsNullOrEmpty(Entity.Check_No))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CHECK_NO", Entity.Check_No));

                if (!string.IsNullOrEmpty(Entity.ProcessOn.Trim()))
                    sqlParamList.Add(new SqlParameter("@Process_On", Entity.ProcessOn));

                if (!string.IsNullOrEmpty(Entity.Vendor_No.Trim()))
                    sqlParamList.Add(new SqlParameter("@CASEACT_VENDOR", Entity.Vendor_No));

                if (!string.IsNullOrEmpty(Entity.Print_Existing.Trim()))
                    sqlParamList.Add(new SqlParameter("@Print_Existing", Entity.Print_Existing));

                sqlParamList.Add(new SqlParameter("@CASEACT_LSTC_OPERATOR", Entity.Lsct_Operator));


                //if (Trigger == "PAID")
                //{
                if (!string.IsNullOrEmpty(Entity.BundleNo))
                    sqlParamList.Add(new SqlParameter("@CASEACT_BUNDLE_NO", Entity.BundleNo));
                if (!string.IsNullOrEmpty(Entity.PaymentNo))
                    sqlParamList.Add(new SqlParameter("@CASEACT_PAYMENT_NO", Entity.PaymentNo));
                //}

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<CASEACTEntity> GetCACheckProcess(CASEACTEntity Entity, string Opretaion_Mode)
        {
            List<CASEACTEntity> CASEVDDProfile = new List<CASEACTEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CACheck_SqlParameters_List(Entity, Opretaion_Mode, "PAID");
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[CA_CHECK_UPDATE]");

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                    {
                        if(Opretaion_Mode=="N")
                            CASEVDDProfile.Add(new CASEACTEntity(row, "WtCheck"));
                        else
                            CASEVDDProfile.Add(new CASEACTEntity(row, "Check"));
                    }
                }
            }
            catch (Exception ex)
            { return CASEVDDProfile; }

            return CASEVDDProfile;
        }

        public List<CASEACTEntity> GetCAWorkData(CASEACTEntity Entity, string Opretaion_Mode)
        {
            List<CASEACTEntity> CASEVDDProfile = new List<CASEACTEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CACheck_SqlParameters_List(Entity, Opretaion_Mode, "PAID");
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[CA_CHECK_UPDATE]");

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASEVDDProfile.Add(new CASEACTEntity(row, "WorkData"));
                }
            }
            catch (Exception ex)
            { return CASEVDDProfile; }

            return CASEVDDProfile;
        }

        public bool Case0006_Act_Bulk_Posting(CASEMSEntity Entity, string App_Xml, string Source_ActMS_Xml, bool Intake_APP, out string Sql_SP_Result_Message)
        {
            bool boolsuccess;
            Sql_SP_Result_Message = Consts.Common.DB_Exception;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@Agy", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@Dept", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@Prog", Entity.Program));
                sqlParamList.Add(new SqlParameter("@Year", Entity.Year));
                sqlParamList.Add(new SqlParameter("@App", Entity.App_no));
                sqlParamList.Add(new SqlParameter("@SP_Code", Entity.Service_plan));
                sqlParamList.Add(new SqlParameter("@SP_Seq", Entity.SPM_Seq));
                sqlParamList.Add(new SqlParameter("@User", Entity.Lsct_Operator));
                sqlParamList.Add(new SqlParameter("@Add_SPM", (Intake_APP ? "Y" : "N")));
                sqlParamList.Add(new SqlParameter("@Src_ActMS_XML", Source_ActMS_Xml));
                sqlParamList.Add(new SqlParameter("@Dst_App_XML", App_Xml));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.Case0006_Act_Bulk_Posting", out Sql_SP_Result_Message);  //  08202012
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool Case0006_Act_Bulk_Posting_Latest(CASEMSEntity Entity, string App_Xml, string Source_ActMS_Xml, bool Intake_APP, string Post_All,string ClientIds, out string Sql_SP_Result_Message)
        {
            bool boolsuccess;
            Sql_SP_Result_Message = Consts.Common.DB_Exception;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@Agy", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@Dept", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@Prog", Entity.Program));
                sqlParamList.Add(new SqlParameter("@Year", Entity.Year));
                sqlParamList.Add(new SqlParameter("@App", Entity.App_no));
                sqlParamList.Add(new SqlParameter("@SP_Code", Entity.Service_plan));
                sqlParamList.Add(new SqlParameter("@SP_Seq", Entity.SPM_Seq));
                sqlParamList.Add(new SqlParameter("@User", Entity.Lsct_Operator));
                sqlParamList.Add(new SqlParameter("@Add_SPM", (Intake_APP ? "Y" : "N")));
                sqlParamList.Add(new SqlParameter("@Post_All", Post_All));
                sqlParamList.Add(new SqlParameter("@Src_ActMS_XML", Source_ActMS_Xml));
                sqlParamList.Add(new SqlParameter("@Dst_App_XML", App_Xml));

                if(!string.IsNullOrEmpty(ClientIds.Trim()))
                    sqlParamList.Add(new SqlParameter("@ClientIDs", ClientIds));
                //boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.Case0006_Act_Bulk_Posting", out Sql_SP_Result_Message);  //  08202012
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.Case0006_Act_Bulk_Posting_Latest", out Sql_SP_Result_Message);  //  08202012
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }


        public bool Case1006_Act_Bulk_Posting_Latest(CASEACTEntity Entity, string App_Xml, string Source_ActMS_Xml, string ShowPosted, string Post_All, string FDate, string TDate, string bulkSite, string BulkSort, string BulkCaseType, string QType, string Estatus, string Question, string Resp,string AddSPM, out string Sql_SP_Result_Message)
        {
            bool boolsuccess;
            Sql_SP_Result_Message = Consts.Common.DB_Exception;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@Agy", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@Dept", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@Prog", Entity.Program));
                sqlParamList.Add(new SqlParameter("@Year", Entity.Year));
                sqlParamList.Add(new SqlParameter("@App", Entity.App_no));
                sqlParamList.Add(new SqlParameter("@SP_Code", Entity.Service_plan));
                sqlParamList.Add(new SqlParameter("@SP_Seq", Entity.SPM_Seq));
                sqlParamList.Add(new SqlParameter("@User", Entity.Lsct_Operator));
                sqlParamList.Add(new SqlParameter("@Sel_Post", ShowPosted));
                sqlParamList.Add(new SqlParameter("@Post_All", Post_All));
                sqlParamList.Add(new SqlParameter("@Src_ActMS_XML", Source_ActMS_Xml));
                sqlParamList.Add(new SqlParameter("@Dst_App_XML", App_Xml));

                if(!string.IsNullOrEmpty(FDate.Trim()))
                    sqlParamList.Add(new SqlParameter("@BTPL_SER_FDATE", FDate));
                if (!string.IsNullOrEmpty(TDate.Trim()))
                    sqlParamList.Add(new SqlParameter("@BTPL_SER_TDATE", TDate));
                if (!string.IsNullOrEmpty(bulkSite.Trim()))
                    sqlParamList.Add(new SqlParameter("@BTPL_SER_SITE", bulkSite));
                if (!string.IsNullOrEmpty(BulkSort.Trim()))
                    sqlParamList.Add(new SqlParameter("@BTPL_SER_SORT", BulkSort));
                if (!string.IsNullOrEmpty(BulkCaseType.Trim()))
                    sqlParamList.Add(new SqlParameter("@BTPL_SER_CASETYPE", BulkCaseType));
                if (!string.IsNullOrEmpty(QType.Trim()))
                    sqlParamList.Add(new SqlParameter("@BTPL_SER_TYPE", QType));
                if (!string.IsNullOrEmpty(Estatus.Trim()))
                    sqlParamList.Add(new SqlParameter("@BTPL_SER_ESTATUS", Estatus));
                if (!string.IsNullOrEmpty(Question.Trim()))
                    sqlParamList.Add(new SqlParameter("@BTPL_SER_QUESTION", Question));
                if (!string.IsNullOrEmpty(Resp.Trim()))
                    sqlParamList.Add(new SqlParameter("@BTPL_SER_RESPONSE", Resp));
                if (!string.IsNullOrEmpty(AddSPM.Trim()))
                    sqlParamList.Add(new SqlParameter("@BTPL_SER_RESPONSE", AddSPM));


                //boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.Case0006_Act_Bulk_Posting", out Sql_SP_Result_Message);  //  08202012
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.Case1006_Act_Bulk_Posting", out Sql_SP_Result_Message);  //  08202012
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool Trigger_Bulk_Posting(CASEMSEntity Entity, string Trig_code, string App_Xml, string Source_ActMS_Xml, bool Intake_APP, string Post_All, string SiteSw,string StartDate,  out int New_Date_Seq, out string Sql_SP_Result_Message)
        {
            bool boolsuccess;
            Sql_SP_Result_Message = Consts.Common.DB_Exception;
            New_Date_Seq = 1;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@Agy", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@Dept", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@Prog", Entity.Program));
                sqlParamList.Add(new SqlParameter("@Year", Entity.Year));
                //sqlParamList.Add(new SqlParameter("@App", Entity.App_no));
                sqlParamList.Add(new SqlParameter("@SP_Code", Entity.Service_plan));
                sqlParamList.Add(new SqlParameter("@SPM_Site", Entity.Site));
                sqlParamList.Add(new SqlParameter("@Worker", Entity.CaseWorker));
                sqlParamList.Add(new SqlParameter("@MSOBO", Entity.OBF));
                sqlParamList.Add(new SqlParameter("@Results", Entity.Result));
                sqlParamList.Add(new SqlParameter("@Program", Entity.Acty_PROG));
                sqlParamList.Add(new SqlParameter("@Site_Sw", SiteSw));
                sqlParamList.Add(new SqlParameter("@User", Entity.Lsct_Operator));
                sqlParamList.Add(new SqlParameter("@Add_SPM", (Intake_APP ? "Y" : "N")));
                sqlParamList.Add(new SqlParameter("@Post_All", Post_All));
                sqlParamList.Add(new SqlParameter("@Src_ActMS_XML", Source_ActMS_Xml));
                sqlParamList.Add(new SqlParameter("@Dst_App_XML", App_Xml));
                sqlParamList.Add(new SqlParameter("@TRIG_Code", Trig_code));
                sqlParamList.Add(new SqlParameter("@TRIG_DATE_START", StartDate));
                SqlParameter parameterMsg = new SqlParameter("@New_Seq", SqlDbType.Int, 5);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.Trigger_Bulk_Posting", out Sql_SP_Result_Message);  //  08202012
                New_Date_Seq = int.Parse(parameterMsg.Value.ToString());

            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }


        public bool UpdateCASEMS(CASEMSEntity Entity, string Operation_Mode, out int New_MS_ID, out string Sql_SP_Result_Message)
        {
            bool boolsuccess;
            Sql_SP_Result_Message = Consts.Common.DB_Exception;
            New_MS_ID = 0;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_CASEMS_SqlParameters_List(Entity, Operation_Mode, string.Empty);
                SqlParameter parameterMsg = new SqlParameter("@CASEMS_New_ID", SqlDbType.Int, 5);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateCASEMS", out Sql_SP_Result_Message);  //  08202012
                New_MS_ID = int.Parse(parameterMsg.Value.ToString());
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateCASEREFS(CASEREFSEntity Entity, string Operation_Mode, out string Sql_SP_Result_Message)
        {
            bool boolsuccess;
            Sql_SP_Result_Message = Consts.Common.DB_Exception;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_CASEREFS_SqlParameters_List(Entity, Operation_Mode);
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateCASEREFS", out Sql_SP_Result_Message);  //
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<SqlParameter> Prepare_CASEREFS_SqlParameters_List(CASEREFSEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@CASEREFS_CODE", Entity.Code));
                sqlParamList.Add(new SqlParameter("@CASEREFS_SERVICE", Entity.Service));
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public bool UpdatePROGNOTES(CaseNotesEntity Entity, string Operation_Mode, out string Sql_SP_Result_Message)
        {
            bool boolsuccess;
            Sql_SP_Result_Message = string.Empty;//Consts.Common.DB_Exception;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_CASENOTES_SqlParameters_List(Entity, Operation_Mode);
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdatePROGNOTES", out Sql_SP_Result_Message);  //
            }
            catch (Exception ex)
            {
                Sql_SP_Result_Message = ex.Message;
                return false;
            }

            return boolsuccess;
        }

        public bool UpdateCASEREF(CASEREFEntity Entity, string Operation_Mode, out string Msg, out string Sql_Reslut_Msg)
        {
            bool boolsuccess = true;
            Sql_Reslut_Msg = "Success";
            Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList = Prepare_CASEREF_SqlParameters_List(Entity, Operation_Mode);
                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UPDATE_CASEREF", out Sql_Reslut_Msg);  //

                Msg = DeleteMsg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateAgencyPartner(AGCYPARTEntity Entity, string Operation_Mode, out string Msg, out string Sql_Reslut_Msg)
        {
            bool boolsuccess = true;
            Sql_Reslut_Msg = "Success";
            Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList = Prepare_AGCYPART_SqlParameters_List(Entity, Operation_Mode);
                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.CAPS_AGCYPART_INSERT", out Sql_Reslut_Msg);  //

                Msg = DeleteMsg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateAgencyBoutique(AGCYBOTIQEntity Entity, string Operation_Mode, out string Msg, out string Sql_Reslut_Msg)
        {
            bool boolsuccess = true;
            Sql_Reslut_Msg = "Success";
            Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList = Prepare_AGCYBoutique_SqlParameters_List(Entity, Operation_Mode);
                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.CAPS_AGCYBOUTIQUE_INSERT", out Sql_Reslut_Msg);  //

                Msg = DeleteMsg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateAgencyRep(AGCYREPEntity Entity, string Operation_Mode, out string Msg, out string Sql_Reslut_Msg)
        {
            bool boolsuccess = true;
            Sql_Reslut_Msg = "Success";
            Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList = Prepare_AGCYREP_SqlParameters_List(Entity, Operation_Mode);
                SqlParameter DeleteMsg = new SqlParameter("@msgRep", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.CAPS_AGCYREP_INSUPDEL", out Sql_Reslut_Msg);  //

                Msg = DeleteMsg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateAgencySubloc(AGCYSUBEntity Entity, string Operation_Mode, out string Msg, out string Sql_Reslut_Msg)
        {
            bool boolsuccess = true;
            Sql_Reslut_Msg = "Success";
            Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList = Prepare_AGCYSUB_SqlParameters_List(Entity, Operation_Mode);
                SqlParameter DeleteMsg = new SqlParameter("@msgSub", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.CAPS_AGYSUBLOC_INSUPDEL", out Sql_Reslut_Msg);  //

                Msg = DeleteMsg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateAgencyServices(AGCYSEREntity Entity, string Operation_Mode, out string Msg, out string Sql_Reslut_Msg)
        {
            bool boolsuccess = true;
            Sql_Reslut_Msg = "Success";
            Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList = Prepare_AGCYSER_SqlParameters_List(Entity, Operation_Mode);
                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.CAPS_AGCYSERVICES_INSUPDEL", out Sql_Reslut_Msg);  //

                Msg = DeleteMsg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }


        public bool InsertLETRHIST(LETRHISTCEntity LHISTEntity, out string strMsg)
        {
            bool boolstatus = false;
            string strerrormsg = string.Empty;
            try
            {

                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@LHIST_AGENCY", LHISTEntity.AGENCY));
                sqlParamList.Add(new SqlParameter("@LHIST_DEPT", LHISTEntity.DEPT));
                sqlParamList.Add(new SqlParameter("@LHIST_PROG", LHISTEntity.PROGRAM));
                if (LHISTEntity.YEAR.Trim() != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LHIST_YEAR", LHISTEntity.YEAR));
                //if (EmsbdcEntity.BDC_DESCRIPTION != string.Empty)
                sqlParamList.Add(new SqlParameter("@LHIST_APPNO", LHISTEntity.APPNO));


                //if (EmsbdcEntity.BDC_FUND != string.Empty)
                sqlParamList.Add(new SqlParameter("@LHIST_CODE", LHISTEntity.LETR_CODE));
                //if (EmsbdcEntity.BDC_BUDGET != string.Empty)
                sqlParamList.Add(new SqlParameter("@LHIST_DATE", LHISTEntity.DATE));
                if (LHISTEntity.SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LHIST_SEQ", LHISTEntity.SEQ));
                //if (EmsbdcEntity.BDC_END != string.Empty)
                sqlParamList.Add(new SqlParameter("@LHIST_WORKER", LHISTEntity.WORKER));
                //if (EmsbdcEntity.BDC_ALLOW_POSTING != string.Empty)
                sqlParamList.Add(new SqlParameter("@LHIST_ADD_OPERATOR", LHISTEntity.ADD_OPERATOR));


                //sqlParamList.Add(new SqlParameter("@Mode", EmsbdcEntity.Mode));
                SqlParameter sqlsMsg = new SqlParameter("@Msg", SqlDbType.VarChar, 10);
                sqlsMsg.Value = string.Empty;
                sqlsMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlsMsg);
                boolstatus = SPAdminDB.InsertLETRHIST(sqlParamList);
                strerrormsg = sqlsMsg.Value.ToString();
            }

            catch (Exception ex)
            {
                boolstatus = false;
            }
            strMsg = strerrormsg;
            return boolstatus;
        }

        public List<LETRHISTCEntity> GetLetrHistData(string agency, string dep, string program, string year, string Appno)
        {
            List<LETRHISTCEntity> Emsbdcdata = new List<LETRHISTCEntity>();
            try
            {
                DataSet emsbdcds = Captain.DatabaseLayer.SPAdminDB.GetLETRHISTData(agency, dep, program, year, Appno);
                if (emsbdcds != null && emsbdcds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsbdcds.Tables[0].Rows)
                    {
                        Emsbdcdata.Add(new LETRHISTCEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsbdcdata;
            }

            return Emsbdcdata;
        }

        public List<SqlParameter> Prepare_CASENOTES_SqlParameters_List(CaseNotesEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Rec_Type", Entity.Mode));

                sqlParamList.Add(new SqlParameter("@CASENOTES_SCREEN_NAME", Entity.ScreenName));
                sqlParamList.Add(new SqlParameter("@CASENOTES_FIELD_NAME", Entity.FieldName));
                sqlParamList.Add(new SqlParameter("@CASENOTES_APP_NO", Entity.AppliCationNo));
                sqlParamList.Add(new SqlParameter("@CASENOTES_LSTC_OPERATOR", Entity.LstcOperation));
                sqlParamList.Add(new SqlParameter("@CASENOTES_DATA", Entity.Data));

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }


        //**************************************CAMast*********************************
        public bool InsertCaMAST(CAMASTEntity CEntity)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@CA_CODE", CEntity.Code));
                sqlParamList.Add(new SqlParameter("@CA_DESC", CEntity.Desc));
                sqlParamList.Add(new SqlParameter("@CA_ACTIVE", CEntity.Active));
                sqlParamList.Add(new SqlParameter("@CA_AUTO_POST", CEntity.AutoPost));
                sqlParamList.Add(new SqlParameter("@CA_UOM", CEntity.UOM));
                sqlParamList.Add(new SqlParameter("@CA_TRANS_ALERT", CEntity.TransactionAlert));

                sqlParamList.Add(new SqlParameter("@CA_LSTC_OPERATOR", CEntity.lstcOperator));
                sqlParamList.Add(new SqlParameter("@CA_ADD_OPERATOR", CEntity.addoperator));
                if(CEntity.VendPaycat!=string.Empty)
                sqlParamList.Add(new SqlParameter("@CA_VEND_PAY_CAT", CEntity.VendPaycat));
                
                sqlParamList.Add(new SqlParameter("@MODE", CEntity.Mode));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsertUpdateCAMAST(sqlParamList);


            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }


        public bool InsertCAPrices(CAPRICESEntity CEntity,out string strOurMsg)
        {
            bool boolsuccess;
            strOurMsg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if(!string.IsNullOrEmpty(CEntity.CAP_ID.Trim()))
                    sqlParamList.Add(new SqlParameter("@CAP_ID", CEntity.CAP_ID));
                sqlParamList.Add(new SqlParameter("@CAP_CODE", CEntity.Code));
                sqlParamList.Add(new SqlParameter("@CAP_FDATE", CEntity.FDate));
                sqlParamList.Add(new SqlParameter("@CAP_LDATE", CEntity.TDate));
                sqlParamList.Add(new SqlParameter("@CAP_PRICE", CEntity.UnitPrice));
                
                sqlParamList.Add(new SqlParameter("@CAP_LSTC_OPERATOR", CEntity.lstcOperator));
                
                sqlParamList.Add(new SqlParameter("@Row_Type", CEntity.Mode));

                SqlParameter parameterMsg = new SqlParameter("@SP_Result_Msg", SqlDbType.VarChar, 50);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsertUpdateCAPrices(sqlParamList);

                strOurMsg = parameterMsg.Value.ToString();
            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }


        //************************************MSMast**************************************

        public bool InsertMsMast(MSMASTEntity MEntity)
        {
            bool boolsuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@MS_CODE", MEntity.Code));
                sqlParamList.Add(new SqlParameter("@MS_DESC", MEntity.Desc));
                sqlParamList.Add(new SqlParameter("@MS_ACTIVE", MEntity.Active));
                sqlParamList.Add(new SqlParameter("@MS_AUTO_POST", MEntity.AutoPost));
                sqlParamList.Add(new SqlParameter("MS_TYPE", MEntity.Type1));
                sqlParamList.Add(new SqlParameter("@MS_LSTC_OPERATOR", MEntity.lstcOperator));
                sqlParamList.Add(new SqlParameter("@MS_ADD_OPERATOR", MEntity.addoperator));
                sqlParamList.Add(new SqlParameter("@MODE", MEntity.Mode));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsertUpdateMSMAST(sqlParamList);
            }
            catch (Exception ex)
            {
                return false;
            }
            return boolsuccess;
        }

        //******************************************Performance*******************************
        public bool InsertUpdateCsb14Grp(Csb14GroupEntity CsbEntity)
        {
            bool boolsuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@csb14grp_ref_fdate", CsbEntity.RefFDate));
                sqlParamList.Add(new SqlParameter("@csb14grp_ref_tdate", CsbEntity.RefTDate));
                sqlParamList.Add(new SqlParameter("@csb14grp_group_code", CsbEntity.GrpCode));

                sqlParamList.Add(new SqlParameter("@csb14grp_table_code", CsbEntity.TblCode));
                sqlParamList.Add(new SqlParameter("@csb14grp_desc", CsbEntity.GrpDesc));
                if (CsbEntity.Hrd1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@csb14grp_cnt_hdr1", CsbEntity.Hrd1));
                if (CsbEntity.Incld1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@csb14grp_cnt_incld1", CsbEntity.Incld1));
                if (CsbEntity.Hrd2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@csb14grp_cnt_hdr2", CsbEntity.Hrd2));
                if (CsbEntity.Incld2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@csb14grp_cnt_incld2", CsbEntity.Incld2));
                if (CsbEntity.Hrd3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@csb14grp_cnt_hdr3", CsbEntity.Hrd3));
                if (CsbEntity.Incld3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@csb14grp_cnt_incld3", CsbEntity.Incld3));
                if (CsbEntity.Hrd4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@csb14grp_cnt_hdr4", CsbEntity.Hrd4));
                if (CsbEntity.Incld4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@csb14grp_cnt_incld4", CsbEntity.Incld4));
                if (CsbEntity.Hrd5 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@csb14grp_cnt_hdr5", CsbEntity.Hrd5));
                if (CsbEntity.Incld5 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@csb14grp_cnt_incld5", CsbEntity.Incld5));
                if (CsbEntity.CntIndic != string.Empty)
                    sqlParamList.Add(new SqlParameter("@csb14grp_cnt_indicator", CsbEntity.CntIndic));
                if (CsbEntity.ExAchev != string.Empty)
                    sqlParamList.Add(new SqlParameter("@csb14grp_expect_achieve", CsbEntity.ExAchev));
                if (CsbEntity.CalCost != string.Empty)
                    sqlParamList.Add(new SqlParameter("@csb14grp_calc_cost", CsbEntity.CalCost));
                if (CsbEntity.UseSer != string.Empty)
                    sqlParamList.Add(new SqlParameter("@csb14grp_use_servs", CsbEntity.UseSer));
                if (CsbEntity.Duplicate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@csb14grp_duplicated", CsbEntity.Duplicate));
                if (CsbEntity.AFrom != string.Empty)
                    sqlParamList.Add(new SqlParameter("@csb14grp_age_from", CsbEntity.AFrom));
                if (CsbEntity.Ato != string.Empty)
                    sqlParamList.Add(new SqlParameter("@csb14grp_age_to", CsbEntity.Ato));
                if (CsbEntity.Disable != string.Empty)
                    sqlParamList.Add(new SqlParameter("@csb14grp_disabled", CsbEntity.Disable));
                if (CsbEntity.GoalCds != string.Empty)
                    sqlParamList.Add(new SqlParameter("@csb14grp_goal_codes", CsbEntity.GoalCds));

                sqlParamList.Add(new SqlParameter("@csb14grp_lstc_operator", CsbEntity.LSTCOperator));
                sqlParamList.Add(new SqlParameter("@csb14grp_date_lstc", CsbEntity.DateLSTC));
                sqlParamList.Add(new SqlParameter("@csb14grp_add_operator", CsbEntity.AddOperator));
                sqlParamList.Add(new SqlParameter("@csb14grp_date_add", CsbEntity.DateAdd));
                sqlParamList.Add(new SqlParameter("@MODE", CsbEntity.Mode));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsertUpdateCSB14GRP(sqlParamList);
            }
            catch (Exception ex)
            {
                return false;
            }
            return boolsuccess;
        }

        public bool InsertUpdateRNGGrp(RCsb14GroupEntity CsbEntity)
        {
            bool boolsuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@RNGGRP_AGENCY", CsbEntity.Agency));
                sqlParamList.Add(new SqlParameter("@RNGgrp_Code", CsbEntity.Code));
                //sqlParamList.Add(new SqlParameter("@RNGgrp_ref_tdate", CsbEntity.RefTDate));
                sqlParamList.Add(new SqlParameter("@RNGgrp_group_code", CsbEntity.GrpCode));

                sqlParamList.Add(new SqlParameter("@RNGgrp_table_code", CsbEntity.TblCode));
                if (CsbEntity.GrpDesc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGgrp_desc", CsbEntity.GrpDesc));
                if (CsbEntity.Hrd1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGgrp_cnt_hdr1", CsbEntity.Hrd1));
                if (CsbEntity.Incld1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGgrp_cnt_incld1", CsbEntity.Incld1));
                if (CsbEntity.Hrd2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGgrp_cnt_hdr2", CsbEntity.Hrd2));
                if (CsbEntity.Incld2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGgrp_cnt_incld2", CsbEntity.Incld2));
                if (CsbEntity.Hrd3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGgrp_cnt_hdr3", CsbEntity.Hrd3));
                if (CsbEntity.Incld3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGgrp_cnt_incld3", CsbEntity.Incld3));
                if (CsbEntity.Hrd4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGgrp_cnt_hdr4", CsbEntity.Hrd4));
                if (CsbEntity.Incld4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGgrp_cnt_incld4", CsbEntity.Incld4));
                if (CsbEntity.Hrd5 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGgrp_cnt_hdr5", CsbEntity.Hrd5));
                if (CsbEntity.Incld5 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGgrp_cnt_incld5", CsbEntity.Incld5));
                if (CsbEntity.CntIndic != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGgrp_cnt_indicator", CsbEntity.CntIndic));
                if (CsbEntity.ExAchev != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGgrp_expect_achieve", CsbEntity.ExAchev));
                if (CsbEntity.CalCost != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGgrp_calc_cost", CsbEntity.CalCost));
                if (CsbEntity.UseSer != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGgrp_use_servs", CsbEntity.UseSer));
                if (CsbEntity.Duplicate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGgrp_duplicated", CsbEntity.Duplicate));
                if (CsbEntity.AFrom != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGgrp_age_from", CsbEntity.AFrom));
                if (CsbEntity.Ato != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGgrp_age_to", CsbEntity.Ato));
                if (CsbEntity.Disable != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGgrp_disabled", CsbEntity.Disable));
                if (CsbEntity.GoalCds != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGgrp_goal_codes", CsbEntity.GoalCds));

                if (CsbEntity.DomainHie != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGGRP_HIE", CsbEntity.DomainHie));

                if (CsbEntity.Active != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGGRP_ACTIVE", CsbEntity.Active));

                if (CsbEntity.PPR_SW != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGGRP_PPR_SW", CsbEntity.PPR_SW));

                sqlParamList.Add(new SqlParameter("@RNGgrp_lstc_operator", CsbEntity.LSTCOperator));
                sqlParamList.Add(new SqlParameter("@RNGgrp_date_lstc", CsbEntity.DateLSTC));
                sqlParamList.Add(new SqlParameter("@RNGgrp_add_operator", CsbEntity.AddOperator));
                sqlParamList.Add(new SqlParameter("@RNGgrp_date_add", CsbEntity.DateAdd));
                sqlParamList.Add(new SqlParameter("@RNGGRP_FDATE", CsbEntity.OFdate));
                sqlParamList.Add(new SqlParameter("@RNGGRP_TDATE", CsbEntity.OTdate));

                if (CsbEntity.IndSwitch != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGGRP_IND_SWITCH", CsbEntity.IndSwitch));

                if (CsbEntity.Prog_Switch != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGGRP_PROG_SWITCH", CsbEntity.Prog_Switch));



                if (CsbEntity.CopyAgency != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Agency", CsbEntity.CopyAgency));

                sqlParamList.Add(new SqlParameter("@MODE", CsbEntity.Mode));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsertUpdateRNGGRP(sqlParamList);
            }
            catch (Exception ex)
            {
                return false;
            }
            return boolsuccess;
        }

        public bool InsertUpdateRNGSRGrp(SRCsb14GroupEntity CsbEntity)
        {
            bool boolsuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@RNGSRGRP_AGENCY", CsbEntity.Agency));
                sqlParamList.Add(new SqlParameter("@RNGSRgrp_Code", CsbEntity.Code));
                //sqlParamList.Add(new SqlParameter("@RNGgrp_ref_tdate", CsbEntity.RefTDate));
                sqlParamList.Add(new SqlParameter("@RNGSRgrp_group_code", CsbEntity.GrpCode));

                sqlParamList.Add(new SqlParameter("@RNGSRgrp_table_code", CsbEntity.TblCode));
                if (CsbEntity.GrpDesc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRgrp_desc", CsbEntity.GrpDesc));
                if (CsbEntity.Hrd1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRgrp_cnt_hdr1", CsbEntity.Hrd1));
                if (CsbEntity.Incld1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRgrp_cnt_incld1", CsbEntity.Incld1));
                if (CsbEntity.Hrd2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRgrp_cnt_hdr2", CsbEntity.Hrd2));
                if (CsbEntity.Incld2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRgrp_cnt_incld2", CsbEntity.Incld2));
                if (CsbEntity.Hrd3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRgrp_cnt_hdr3", CsbEntity.Hrd3));
                if (CsbEntity.Incld3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRgrp_cnt_incld3", CsbEntity.Incld3));
                if (CsbEntity.Hrd4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRgrp_cnt_hdr4", CsbEntity.Hrd4));
                if (CsbEntity.Incld4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRgrp_cnt_incld4", CsbEntity.Incld4));
                if (CsbEntity.Hrd5 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRgrp_cnt_hdr5", CsbEntity.Hrd5));
                if (CsbEntity.Incld5 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRgrp_cnt_incld5", CsbEntity.Incld5));
                if (CsbEntity.CntIndic != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRgrp_cnt_indicator", CsbEntity.CntIndic));
                if (CsbEntity.ExAchev != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRgrp_expect_achieve", CsbEntity.ExAchev));
                if (CsbEntity.CalCost != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRgrp_calc_cost", CsbEntity.CalCost));
                if (CsbEntity.UseSer != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRgrp_use_servs", CsbEntity.UseSer));
                if (CsbEntity.Duplicate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRgrp_duplicated", CsbEntity.Duplicate));
                if (CsbEntity.AFrom != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRgrp_age_from", CsbEntity.AFrom));
                if (CsbEntity.Ato != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRgrp_age_to", CsbEntity.Ato));
                if (CsbEntity.Disable != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRgrp_disabled", CsbEntity.Disable));
                if (CsbEntity.GoalCds != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRgrp_goal_codes", CsbEntity.GoalCds));

                if (CsbEntity.DomainHie != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRGRP_HIE", CsbEntity.DomainHie));

                if (CsbEntity.Active != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRGRP_ACTIVE", CsbEntity.Active));

                if (CsbEntity.PPR_SW != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRGRP_PPR_SW", CsbEntity.PPR_SW));

                sqlParamList.Add(new SqlParameter("@RNGSRgrp_lstc_operator", CsbEntity.LSTCOperator));
                //sqlParamList.Add(new SqlParameter("@RNGSRgrp_date_lstc", CsbEntity.DateLSTC));
                sqlParamList.Add(new SqlParameter("@RNGSRgrp_add_operator", CsbEntity.AddOperator));
                //sqlParamList.Add(new SqlParameter("@RNGSRgrp_date_add", CsbEntity.DateAdd));
                sqlParamList.Add(new SqlParameter("@RNGSRGRP_FDATE", CsbEntity.OFdate));
                sqlParamList.Add(new SqlParameter("@RNGSRGRP_TDATE", CsbEntity.OTdate));

                if (CsbEntity.Prog_Switch != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRGRP_PROG_SWITCH", CsbEntity.Prog_Switch));

                if (CsbEntity.CSBG_Domain != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRGRP_CSBG", CsbEntity.CSBG_Domain));

                if (CsbEntity.BenRsnDomainField != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGSRGRP_BENFRESN", CsbEntity.BenRsnDomainField));

                if (CsbEntity.CopyAgency != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Agency", CsbEntity.CopyAgency));

                sqlParamList.Add(new SqlParameter("@MODE", CsbEntity.Mode));

              
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsertUpdateRNGSRGRP(sqlParamList);
            }
            catch (Exception ex)
            {
                return false;
            }
            return boolsuccess;
        }

        public List<Csb14GroupEntity> Browse_CSB14Grp(string RefFdt, string RefTdt, string GrpCd, string TblCd, string Desc)
        {
            List<Csb14GroupEntity> Csb14GrpProfile = new List<Csb14GroupEntity>();
            try
            {
                DataSet Csb14fgrpData = SPAdminDB.Get_CSB14GRP(RefFdt, RefTdt, GrpCd, TblCd, Desc);
                if (Csb14fgrpData != null && Csb14fgrpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in Csb14fgrpData.Tables[0].Rows)
                        Csb14GrpProfile.Add(new Csb14GroupEntity(row));
                }
            }
            catch (Exception ex)
            { return Csb14GrpProfile; }

            return Csb14GrpProfile;
        }

        public List<RCsb14GroupEntity> Browse_RNGGrp(string CODE, string Agency, string GrpCd, string TblCd, string Desc,string UserID,string AdminAgency)
        {
            List<RCsb14GroupEntity> Csb14GrpProfile = new List<RCsb14GroupEntity>();
            try
            {
                DataSet Csb14fgrpData = SPAdminDB.Get_RNGGRP(CODE, Agency, GrpCd, TblCd, Desc,UserID, AdminAgency);
                if (Csb14fgrpData != null && Csb14fgrpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in Csb14fgrpData.Tables[0].Rows)
                        Csb14GrpProfile.Add(new RCsb14GroupEntity(row));
                }
            }
            catch (Exception ex)
            { return Csb14GrpProfile; }

            return Csb14GrpProfile;
        }

        //KRANTHI:: COMMENTED on 09222022 - to update with following new method 
        //public List<SRCsb14GroupEntity> Browse_RNGSRGrp(string CODE, string Agency, string GrpCd, string TblCd, string Desc,string UserID,string AdminAgency)
        //{
        //    List<SRCsb14GroupEntity> Csb14GrpProfile = new List<SRCsb14GroupEntity>();
        //    try
        //    {
        //        DataSet Csb14fgrpData = SPAdminDB.Get_RNGSRGRP(CODE, Agency, GrpCd, TblCd, Desc, UserID, AdminAgency);
        //        if (Csb14fgrpData != null && Csb14fgrpData.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow row in Csb14fgrpData.Tables[0].Rows)
        //                Csb14GrpProfile.Add(new SRCsb14GroupEntity(row));
        //        }
        //    }
        //    catch (Exception ex)
        //    { return Csb14GrpProfile; }

        //    return Csb14GrpProfile;
        //}
        public List<SRCsb14GroupEntity> Browse_RNGSRGrp(string CODE, string Agency, string GrpCd, string TblCd, string Desc, string UserID, string AdminAgency)
        {
            List<SRCsb14GroupEntity> Csb14GrpProfile = new List<SRCsb14GroupEntity>();
            try
            {
                DataSet Csb14fgrpData = SPAdminDB.Get_RNGSRGRP(CODE, Agency, GrpCd, TblCd, Desc, UserID, AdminAgency);
                if (Csb14fgrpData != null && Csb14fgrpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in Csb14fgrpData.Tables[0].Rows)
                        Csb14GrpProfile.Add(new SRCsb14GroupEntity(row));
                }
            }
            catch (Exception ex)
            { return Csb14GrpProfile; }

            return Csb14GrpProfile;
        }


        public List<SPCommonEntity> GetSP0Funds(string Agency,string Dept,string Prog,string ServicePlan,  string Scr_code,string UserId)
        {
            List<SPCommonEntity> Csb4AssocProfile = new List<SPCommonEntity>();
            try
            {
                DataSet Csb4AssocData = SPAdminDB.Get_SP0Funds(Agency,Dept,Prog,ServicePlan, UserId);
                if (Csb4AssocData != null && Csb4AssocData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in Csb4AssocData.Tables[0].Rows)
                        Csb4AssocProfile.Add(new SPCommonEntity(row,Scr_code,string.Empty,string.Empty));
                }
            }
            catch (Exception ex)
            { return Csb4AssocProfile; }

            return Csb4AssocProfile;
        }

        public List<AGYTABSEntity> GetSP0Funds_adhoc(string Agency, string Dept, string Prog, string ServicePlan, string Scr_code,string UserID)
        {
            List<AGYTABSEntity> Csb4AssocProfile = new List<AGYTABSEntity>();
            try
            {
                DataSet Csb4AssocData = SPAdminDB.Get_SP0Funds(Agency, Dept, Prog, ServicePlan, UserID);
                if (Csb4AssocData != null && Csb4AssocData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in Csb4AssocData.Tables[0].Rows)
                        Csb4AssocProfile.Add(new AGYTABSEntity(row, Scr_code, string.Empty));
                }
            }
            catch (Exception ex)
            { return Csb4AssocProfile; }

            return Csb4AssocProfile;
        }

        public List<CSB4AsocEntity> Browse_CSB4Assoc(string Cat_Cd, string Demo_Cd)
        {
            List<CSB4AsocEntity> Csb4AssocProfile = new List<CSB4AsocEntity>();
            try
            {
                DataSet Csb4AssocData = SPAdminDB.Get_CSASS(Cat_Cd, Demo_Cd);
                if (Csb4AssocData != null && Csb4AssocData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in Csb4AssocData.Tables[0].Rows)
                        Csb4AssocProfile.Add(new CSB4AsocEntity(row));
                }
            }
            catch (Exception ex)
            { return Csb4AssocProfile; }

            return Csb4AssocProfile;
        }
        //***********RNG *****************
        public List<RNG4AsocEntity> Browse_RNG4Assoc(string Cat_Cd, string Demo_Cd)
        {
            List<RNG4AsocEntity> Csb4AssocProfile = new List<RNG4AsocEntity>();
            try
            {
                DataSet Csb4AssocData = SPAdminDB.Get_RNGASS(Cat_Cd, Demo_Cd);
                if (Csb4AssocData != null && Csb4AssocData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in Csb4AssocData.Tables[0].Rows)
                        Csb4AssocProfile.Add(new RNG4AsocEntity(row));
                }
            }
            catch (Exception ex)
            { return Csb4AssocProfile; }

            return Csb4AssocProfile;
        }

        public bool InsertUpdateCsb4Assoc(CSB4AsocEntity CsbEntity)
        {
            bool boolsuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@CSB4ASOC_CATG_CODE", CsbEntity.CatCode));
                sqlParamList.Add(new SqlParameter("@CSB4ASOC_DEMO_CODE", CsbEntity.DemoCd));
                if (!string.IsNullOrEmpty(CsbEntity.AgeFrm.ToString()))
                    sqlParamList.Add(new SqlParameter("@CSB4ASOC_AGE_FROM", int.Parse(CsbEntity.AgeFrm.ToString())));
                if (!string.IsNullOrEmpty(CsbEntity.AgeTo.ToString()))
                    sqlParamList.Add(new SqlParameter("@CSB4ASOC_AGE_TO", int.Parse(CsbEntity.AgeTo.ToString())));
                sqlParamList.Add(new SqlParameter("@CSB4ASOC_LSTC_OPERATOR", CsbEntity.lstcOperator));
                sqlParamList.Add(new SqlParameter("@CSB4ASOC_ADD_OPERATOR", CsbEntity.addoperator));
                sqlParamList.Add(new SqlParameter("@CSB4ASOC_AGYTAB_CODES", CsbEntity.AgytabCds));
                sqlParamList.Add(new SqlParameter("@MODE", CsbEntity.Mode));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsertUpdateCSB4ASOC(sqlParamList);
            }
            catch (Exception ex)
            {
                return false;
            }
            return boolsuccess;
        }

        public bool InsertUpdateRNG4Assoc(RNG4AsocEntity RNGEntity)
        {
            bool boolsuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@RNG4ASOC_CATG_CODE", RNGEntity.RNGCatCode));
                sqlParamList.Add(new SqlParameter("@RNG4ASOC_DEMO_CODE", RNGEntity.RNGDemoCd));
                if (!string.IsNullOrEmpty(RNGEntity.RNGAgeFrm.ToString()))
                    sqlParamList.Add(new SqlParameter("@RNG4ASOC_AGE_FROM", int.Parse(RNGEntity.RNGAgeFrm.ToString())));
                if (!string.IsNullOrEmpty(RNGEntity.RNGAgeTo.ToString()))
                    sqlParamList.Add(new SqlParameter("@RNG4ASOC_AGE_TO", int.Parse(RNGEntity.RNGAgeTo.ToString())));
                sqlParamList.Add(new SqlParameter("@RNG4ASOC_LSTC_OPERATOR", RNGEntity.RNGlstcOperator));
                sqlParamList.Add(new SqlParameter("@RNG4ASOC_ADD_OPERATOR", RNGEntity.RNGaddoperator));
                sqlParamList.Add(new SqlParameter("@RNG4ASOC_AGYTAB_CODES", RNGEntity.RNGAgytabCds));
                sqlParamList.Add(new SqlParameter("@MODE", RNGEntity.RNGMode));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsertUpdateRNG4ASOC(sqlParamList);
            }
            catch (Exception ex)
            {
                return false;
            }
            return boolsuccess;
        }

        public bool InsertUpdateCsb14RA(Csb14RAEntity CsbEntity)
        {
            bool boolsuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@CSB14RA_REF_FDATE", CsbEntity.RefFDate));
                sqlParamList.Add(new SqlParameter("@CSB14RA_REF_TDATE", CsbEntity.RefTDate));
                sqlParamList.Add(new SqlParameter("@CSB14RA_GROUP_CODE", CsbEntity.GrpCode));
                sqlParamList.Add(new SqlParameter("@CSB14RA_COUNT_CODE", CsbEntity.CntCode));
                sqlParamList.Add(new SqlParameter("@CSB14RA_RESULT_CODE", CsbEntity.ResCode));
                sqlParamList.Add(new SqlParameter("@CSB14RA_DESC", CsbEntity.Desc));
                sqlParamList.Add(new SqlParameter("@CSB14RA_LSTC_OPERATOR", CsbEntity.LSTCOperator));


                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsertUpdateCSB14RA(sqlParamList);
            }
            catch (Exception ex)
            {
                return false;
            }
            return boolsuccess;
        }

        public bool InsertUpdateRNGRA(RNGRAEntity CsbEntity)
        {
            bool boolsuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@RNGRA_AGENCY", CsbEntity.Agency));
                sqlParamList.Add(new SqlParameter("@RNGRA_Code", CsbEntity.Code));
                //sqlParamList.Add(new SqlParameter("@RNGRA_REF_TDATE", CsbEntity.RefTDate));
                sqlParamList.Add(new SqlParameter("@RNGRA_GROUP_CODE", CsbEntity.GrpCode));
                sqlParamList.Add(new SqlParameter("@RNGRA_COUNT_CODE", CsbEntity.CntCode));
                sqlParamList.Add(new SqlParameter("@RNGRA_RESULT_CODE", CsbEntity.ResCode));
                sqlParamList.Add(new SqlParameter("@RNGRA_DESC", CsbEntity.Desc));
                sqlParamList.Add(new SqlParameter("@RNGRA_LSTC_OPERATOR", CsbEntity.LSTCOperator));


                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsertUpdateRNGRA(sqlParamList);
            }
            catch (Exception ex)
            {
                return false;
            }
            return boolsuccess;
        }

        public bool InsertUpdateRNGGA(RNGGAEntity CsbEntity)
        {
            bool boolsuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@RNGGA_AGENCY", CsbEntity.Agency));
                sqlParamList.Add(new SqlParameter("@RNGGA_CODE", CsbEntity.Code));
                sqlParamList.Add(new SqlParameter("@RNGGA_TABLE_CODE", CsbEntity.TblCode));
                sqlParamList.Add(new SqlParameter("@RNGGA_GROUP_CODE", CsbEntity.GrpCode));
                sqlParamList.Add(new SqlParameter("@RNGGA_COUNT_CODE", CsbEntity.CntCode));
                sqlParamList.Add(new SqlParameter("@RNGGA_GOAL_CODE", CsbEntity.GoalCode));
                sqlParamList.Add(new SqlParameter("@RNGGA_DESC", CsbEntity.Desc));
                sqlParamList.Add(new SqlParameter("@RNGGA_BUDGET", CsbEntity.Budget));
                sqlParamList.Add(new SqlParameter("@RNGGA_SP", CsbEntity.SerSP));
                sqlParamList.Add(new SqlParameter("@RNGGA_LSTC_OPERATOR", CsbEntity.LSTCOperator));
                sqlParamList.Add(new SqlParameter("@MODE", CsbEntity.Mode));

                sqlParamList.Add(new SqlParameter("@RNGGA_SEQ", CsbEntity.Sequence));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsertUpdateRNGGA(sqlParamList);
            }
            catch (Exception ex)
            {
                return false;
            }
            return boolsuccess;
        }

        public bool InsertUpdateRNGGAHIE(RNGGoalHEntity CsbEntity)
        {
            bool boolsuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@RNGGAH_AGENCY", CsbEntity.RNGGAH_AGENCY));
                sqlParamList.Add(new SqlParameter("@RNGGAH_CODE", CsbEntity.RNGGAH_CODE));
                sqlParamList.Add(new SqlParameter("@RNGGAH_TBL_CODE", CsbEntity.RNGGAH_TBL_CODE));
                sqlParamList.Add(new SqlParameter("@RNGGAH_GRP_CODE", CsbEntity.RNGGAH_GRP_CODE));
                //sqlParamList.Add(new SqlParameter("@RNGGA_COUNT_CODE", CsbEntity.CntCode));
                if(!string.IsNullOrEmpty(CsbEntity.RNGGAH_GOAL_CODE.Trim()))
                    sqlParamList.Add(new SqlParameter("@RNGGAH_GOAL_CODE", CsbEntity.RNGGAH_GOAL_CODE));
                if(!string.IsNullOrEmpty(CsbEntity.RNGGAH_HIE.Trim()))
                    sqlParamList.Add(new SqlParameter("@RNGGAH_HIE", CsbEntity.RNGGAH_HIE));
                if (!string.IsNullOrEmpty(CsbEntity.RNGGAH_TARGET.Trim()))
                    sqlParamList.Add(new SqlParameter("@RNGGAH_TARGET", CsbEntity.RNGGAH_TARGET));
                sqlParamList.Add(new SqlParameter("@RNGGAH_LSTC_OPERATOR", CsbEntity.RNGGAH_LSTC_OPERATOR));
                sqlParamList.Add(new SqlParameter("@MODE", CsbEntity.Mode));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsertUpdateRNGGAHIE(sqlParamList);
            }
            catch (Exception ex)
            {
                return false;
            }
            return boolsuccess;
        }

        public bool InsertUpdateRNGSRGA(RNGSRGAEntity CsbEntity)
        {
            bool boolsuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@RNGSRGA_AGENCY", CsbEntity.Agency));
                sqlParamList.Add(new SqlParameter("@RNGSRGA_CODE", CsbEntity.Code));
                sqlParamList.Add(new SqlParameter("@RNGSRGA_TABLE_CODE", CsbEntity.TblCode));
                sqlParamList.Add(new SqlParameter("@RNGSRGA_GROUP_CODE", CsbEntity.GrpCode));
                sqlParamList.Add(new SqlParameter("@RNGSRGA_COUNT_CODE", CsbEntity.CntCode));
                sqlParamList.Add(new SqlParameter("@RNGSRGA_GOAL_CODE", CsbEntity.GoalCode));
                sqlParamList.Add(new SqlParameter("@RNGSRGA_DESC", CsbEntity.Desc));
                sqlParamList.Add(new SqlParameter("@RNGSRGA_BUDGET", CsbEntity.Budget));
                sqlParamList.Add(new SqlParameter("@RNGSRGA_SP", CsbEntity.SerSP));
                sqlParamList.Add(new SqlParameter("@RNGSRGA_LSTC_OPERATOR", CsbEntity.LSTCOperator));
                sqlParamList.Add(new SqlParameter("@MODE", CsbEntity.Mode));

                sqlParamList.Add(new SqlParameter("@RNGSRGA_SEQ", CsbEntity.Sequence));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsertUpdateRNGSRGA(sqlParamList);
            }
            catch (Exception ex)
            {
                return false;
            }
            return boolsuccess;
        }

        public bool InsertUpdateRNGSRGAHIE(RNGSRGoalHEntity CsbEntity)
        {
            bool boolsuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@RNGSRGAH_AGENCY", CsbEntity.RNGSRGAH_AGENCY));
                sqlParamList.Add(new SqlParameter("@RNGSRGAH_CODE", CsbEntity.RNGSRGAH_CODE));
                sqlParamList.Add(new SqlParameter("@RNGSRGAH_TBL_CODE", CsbEntity.RNGSRGAH_TBL_CODE));
                sqlParamList.Add(new SqlParameter("@RNGSRGAH_GRP_CODE", CsbEntity.RNGSRGAH_GRP_CODE));
                //sqlParamList.Add(new SqlParameter("@RNGGA_COUNT_CODE", CsbEntity.CntCode));
                if (!string.IsNullOrEmpty(CsbEntity.RNGSRGAH_GOAL_CODE.Trim()))
                    sqlParamList.Add(new SqlParameter("@RNGSRGAH_GOAL_CODE", CsbEntity.RNGSRGAH_GOAL_CODE));
                if (!string.IsNullOrEmpty(CsbEntity.RNGSRGAH_HIE.Trim()))
                    sqlParamList.Add(new SqlParameter("@RNGSRGAH_HIE", CsbEntity.RNGSRGAH_HIE));
                if (!string.IsNullOrEmpty(CsbEntity.RNGSRGAH_TARGET.Trim()))
                    sqlParamList.Add(new SqlParameter("@RNGSRGAH_TARGET", CsbEntity.RNGSRGAH_TARGET));
                sqlParamList.Add(new SqlParameter("@RNGSRGAH_LSTC_OPERATOR", CsbEntity.RNGSRGAH_LSTC_OPERATOR));
                sqlParamList.Add(new SqlParameter("@MODE", CsbEntity.Mode));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsertUpdateRNGSRGAHIE(sqlParamList);
            }
            catch (Exception ex)
            {
                return false;
            }
            return boolsuccess;
        }


        public bool UpdateCSB14GRP_Goals(Csb14GroupEntity CsbEntity)
        {
            bool boolsuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@csb14grp_ref_fdate", CsbEntity.RefFDate));
                sqlParamList.Add(new SqlParameter("@csb14grp_ref_tdate", CsbEntity.RefTDate));
                sqlParamList.Add(new SqlParameter("@csb14grp_group_code", CsbEntity.GrpCode));
                sqlParamList.Add(new SqlParameter("@csb14grp_table_code", CsbEntity.TblCode));
                sqlParamList.Add(new SqlParameter("@csb14grp_goal_codes", CsbEntity.GoalCds));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.UpdateCSB14Grp_Goals(sqlParamList);
            }
            catch (Exception ex)
            {
                return false;
            }
            return boolsuccess;
        }

        public List<Csb14RAEntity> Browse_CSB14RA(string fdate, string Tdate, string grpcd, string CntCode)
        {
            List<Csb14RAEntity> CSB14RAProfile = new List<Csb14RAEntity>();
            try
            {
                DataSet CSB14RAData = SPAdminDB.Get_CSB14RA(fdate, Tdate, grpcd, CntCode);
                if (CSB14RAData != null && CSB14RAData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CSB14RAData.Tables[0].Rows)
                        CSB14RAProfile.Add(new Csb14RAEntity(row));
                }
            }
            catch (Exception ex)
            { return CSB14RAProfile; }

            return CSB14RAProfile;
        }

        public List<RNGRAEntity> Browse_RNGRA(string fdate, string grpcd, string CntCode)
        {
            List<RNGRAEntity> CSB14RAProfile = new List<RNGRAEntity>();
            try
            {
                DataSet CSB14RAData = SPAdminDB.Get_RNGRA(fdate, grpcd, CntCode);
                if (CSB14RAData != null && CSB14RAData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CSB14RAData.Tables[0].Rows)
                        CSB14RAProfile.Add(new RNGRAEntity(row));
                }
            }
            catch (Exception ex)
            { return CSB14RAProfile; }

            return CSB14RAProfile;
        }

        public List<RNGGAEntity> Browse_RNGGA(string Maincode, string Agency, string grpCd, string table, string CntCode)
        {
            List<RNGGAEntity> CSB14RAProfile = new List<RNGGAEntity>();
            try
            {
                DataSet CSB14RAData = SPAdminDB.Get_RNGGA(Maincode, Agency, grpCd, table, CntCode);
                if (CSB14RAData != null && CSB14RAData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CSB14RAData.Tables[0].Rows)
                        CSB14RAProfile.Add(new RNGGAEntity(row));
                }
            }
            catch (Exception ex)
            { return CSB14RAProfile; }

            return CSB14RAProfile;
        }

        public List<RNGGoalHEntity> Browse_RNGGAH(string Maincode, string Agency, string grpCd, string table, string GoalCode)
        {
            List<RNGGoalHEntity> CSB14RAProfile = new List<RNGGoalHEntity>();
            try
            {
                DataSet CSB14RAData = SPAdminDB.Get_RNGGOALH(Maincode, Agency, grpCd, table, GoalCode);
                if (CSB14RAData != null && CSB14RAData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CSB14RAData.Tables[0].Rows)
                        CSB14RAProfile.Add(new RNGGoalHEntity(row));
                }
            }
            catch (Exception ex)
            { return CSB14RAProfile; }

            return CSB14RAProfile;
        }

        public List<RNGSRGAEntity> Browse_RNGSRGA(string fdate, string Agency, string grpCd, string table, string CntCode)
        {
            List<RNGSRGAEntity> CSB14RAProfile = new List<RNGSRGAEntity>();
            try
            {
                DataSet CSB14RAData = SPAdminDB.Get_RNGSRGA(fdate, Agency, grpCd, table, CntCode);
                if (CSB14RAData != null && CSB14RAData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CSB14RAData.Tables[0].Rows)
                        CSB14RAProfile.Add(new RNGSRGAEntity(row));
                }
            }
            catch (Exception ex)
            { return CSB14RAProfile; }

            return CSB14RAProfile;
        }

        public List<RNGSRGoalHEntity> Browse_RNGSRGAH(string Maincode, string Agency, string grpCd, string table, string GoalCode)
        {
            List<RNGSRGoalHEntity> CSB14RAProfile = new List<RNGSRGoalHEntity>();
            try
            {
                DataSet CSB14RAData = SPAdminDB.Get_RNGSRGOALH(Maincode, Agency, grpCd, table, GoalCode);
                if (CSB14RAData != null && CSB14RAData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CSB14RAData.Tables[0].Rows)
                        CSB14RAProfile.Add(new RNGSRGoalHEntity(row));
                }
            }
            catch (Exception ex)
            { return CSB14RAProfile; }

            return CSB14RAProfile;
        }

        public List<RankCatgEntity> Browse_RankCtg()
        {
            List<RankCatgEntity> RankCatgrofile = new List<RankCatgEntity>();
            try
            {
                DataSet RankCatgData = SPAdminDB.Browse_RnkCatg();
                if (RankCatgData != null && RankCatgData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in RankCatgData.Tables[0].Rows)
                        RankCatgrofile.Add(new RankCatgEntity(row));
                }
            }
            catch (Exception ex)
            { return RankCatgrofile; }

            return RankCatgrofile;
        }

        public List<RankCatgEntity> Browse_PreassGroups()
        {
            List<RankCatgEntity> RankCatgrofile = new List<RankCatgEntity>();
            try
            {
                DataSet RankCatgData = SPAdminDB.Browse_PreassGroups();
                if (RankCatgData != null && RankCatgData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in RankCatgData.Tables[0].Rows)
                        RankCatgrofile.Add(new RankCatgEntity(row, string.Empty));
                }
            }
            catch (Exception ex)
            { return RankCatgrofile; }

            return RankCatgrofile;
        }

        public bool InsertRankPoints(RankCatgEntity RankEntity)
        {
            bool boolsuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@RANKCTG_AGENCY", RankEntity.Agency));
                sqlParamList.Add(new SqlParameter("@RANKCTG_GRPCTG_CODE", RankEntity.Code));
                sqlParamList.Add(new SqlParameter("@RANKCTG_SUBCTG_CODE", RankEntity.SubCode));
                sqlParamList.Add(new SqlParameter("@RANKCTG_DESC", RankEntity.Desc));
                sqlParamList.Add(new SqlParameter("@RANKCTG_POINTS_LOW", int.Parse(RankEntity.PointsLow)));
                sqlParamList.Add(new SqlParameter("@RANKCTG_POINTS_HIGH", int.Parse(RankEntity.PointsHigh)));
                sqlParamList.Add(new SqlParameter("@RANKCTG_HS_CTG", RankEntity.HeadStrt));
                sqlParamList.Add(new SqlParameter("@RANKCTG_DATE_ADD", RankEntity.Dateadd));
                sqlParamList.Add(new SqlParameter("@RANKCTG_ADD_OPERATOR", RankEntity.addoperator));
                sqlParamList.Add(new SqlParameter("@RANKCTG_DATE_LSTC", RankEntity.DateLstc));
                sqlParamList.Add(new SqlParameter("@RANKCTG_LSTC_OPERATOR", RankEntity.lstcOperator));
                sqlParamList.Add(new SqlParameter("@Mode", RankEntity.Mode));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsertRankPnt(sqlParamList);
            }
            catch (Exception ex)
            {
                return false;
            }
            return boolsuccess;
        }

        public bool InsertUpdatePreassGroup(RankCatgEntity RankEntity, out string strOurMsg)
        {
            bool boolsuccess = false;
            strOurMsg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@PREASSGRP_CODE", RankEntity.Code));
                sqlParamList.Add(new SqlParameter("@PREASSGRP_SUBCODE", RankEntity.SubCode));
                sqlParamList.Add(new SqlParameter("@PREASSGRP_DESC", RankEntity.Desc));
                sqlParamList.Add(new SqlParameter("@PREASSGRP_POINTS_LOW", decimal.Parse(RankEntity.PointsLow)));
                sqlParamList.Add(new SqlParameter("@PREASSGRP_POINTS_HIGH", decimal.Parse(RankEntity.PointsHigh)));
                sqlParamList.Add(new SqlParameter("@PREASSGRP_TYPE", RankEntity.HeadStrt));
                sqlParamList.Add(new SqlParameter("@PREASSGRP_DATE_ADD", RankEntity.Dateadd));
                sqlParamList.Add(new SqlParameter("@PREASSGRP_ADD_OPERATOR", RankEntity.addoperator));
                sqlParamList.Add(new SqlParameter("@PREASSGRP_DATE_LSTC", RankEntity.DateLstc));
                sqlParamList.Add(new SqlParameter("@PREASSGRP_LSTC_OPERATOR", RankEntity.lstcOperator));
                sqlParamList.Add(new SqlParameter("@Mode", RankEntity.Mode));

                SqlParameter parameterMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsertUpdatePREASSGRP(sqlParamList);

                strOurMsg = parameterMsg.Value.ToString();


            }
            catch (Exception ex)
            {
                return false;
            }
            return boolsuccess;
        }


        public List<RNKCRIT1Entity> GetRNKCRIT(string RankCd)
        {
            List<RNKCRIT1Entity> RankCatgrofile = new List<RNKCRIT1Entity>();
            try
            {
                DataSet RankCRIT1 = SPAdminDB.GetScreeNames(RankCd);
                if (RankCRIT1 != null && RankCRIT1.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in RankCRIT1.Tables[0].Rows)
                        RankCatgrofile.Add(new RNKCRIT1Entity(row));
                }
            }
            catch (Exception ex)
            { return RankCatgrofile; }

            return RankCatgrofile;
        }

        public List<RNKCRIT1Entity1> Browse_RankCritiria1(string Agency, string Rank, string ScreenCode, string FieldCd)
        {
            List<RNKCRIT1Entity1> RankCritiria1profile = new List<RNKCRIT1Entity1>();
            try
            {
                DataSet RankCritiria1Data = SPAdminDB.BrowseRNKCRIT1(Agency, Rank, ScreenCode, FieldCd);
                if (RankCritiria1Data != null && RankCritiria1Data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in RankCritiria1Data.Tables[0].Rows)
                        RankCritiria1profile.Add(new RNKCRIT1Entity1(row));
                }
            }
            catch (Exception ex)
            { return RankCritiria1profile; }

            return RankCritiria1profile;
        }

        public bool InsertRankCRIT1(RNKCRIT1Entity1 RankEntity, out int New_Id)
        {
            bool boolsuccess = false;
            New_Id = 0;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@RNKCRIT1_AGENCY", RankEntity.Agency));
                //sqlParamList.Add(new SqlParameter("@RNKCRIT1_DEPT", RankEntity.Dept));
                //sqlParamList.Add(new SqlParameter("@RNKCRIT1_PROGRAM", RankEntity.Prog));
                sqlParamList.Add(new SqlParameter("@RNKCRIT1_RANK_CTG", RankEntity.RankCtg));
                sqlParamList.Add(new SqlParameter("@RNKCRIT1_SCR_CODE", RankEntity.Scr_Cd));
                sqlParamList.Add(new SqlParameter("@RNKCRIT1_FIELD_CODE", RankEntity.Fld_cd));
                sqlParamList.Add(new SqlParameter("@RNKCRIT1_FLD_TYPE", RankEntity.Fld_type));
                sqlParamList.Add(new SqlParameter("@RNKCRIT1_ID", RankEntity.Id));
                //if (!string.IsNullOrEmpty(RankEntity.Hie.ToString()))
                //    sqlParamList.Add(new SqlParameter("@RNKCRIT1_HIERARCHY", RankEntity.Hie));
                //if (!string.IsNullOrEmpty(RankEntity.CountInd.ToString()))
                //    sqlParamList.Add(new SqlParameter("@RNKCRIT1_COUNTING_INDI", RankEntity.CountInd));
                //if (!string.IsNullOrEmpty(RankEntity.AgeClcInd.ToString()))
                //    sqlParamList.Add(new SqlParameter("@RNKCRIT1_AGE_CLC_INDI", RankEntity.AgeClcInd));
                sqlParamList.Add(new SqlParameter("@RNKCRIT1_DATE_ADD", RankEntity.Dateadd));
                sqlParamList.Add(new SqlParameter("@RNKCRIT1_ADD_OPERATOR", RankEntity.Addoperator));
                sqlParamList.Add(new SqlParameter("@RNKCRIT1_DATE_LSTC", RankEntity.dateLstc));
                sqlParamList.Add(new SqlParameter("@RNKCRIT1_LSTC_OPERATOR", RankEntity.LstcOperator));
                sqlParamList.Add(new SqlParameter("@Mode", RankEntity.Mode));

                SqlParameter parameterMsg = new SqlParameter("@CurrentId", SqlDbType.Int, 5);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsertRNKCRIT1(sqlParamList);

                New_Id = int.Parse(parameterMsg.Value.ToString());
            }
            catch (Exception ex)
            {
                return false;
            }
            return boolsuccess;
        }

        public bool InsertRankCRIT2(RNKCRIT2Entity RankEntity, out int New_Seq, out string Del_Flag)
        {
            bool boolsuccess = false;
            New_Seq = 0; Del_Flag = "N";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@RNKCRIT2_ID", RankEntity.Id));
                sqlParamList.Add(new SqlParameter("@RNKCRIT2_SEQ", RankEntity.Seq));
                if (!string.IsNullOrEmpty(RankEntity.Relation.ToString()))
                    sqlParamList.Add(new SqlParameter("@RNKCRIT2_RELATION", RankEntity.Relation));
                if (!string.IsNullOrEmpty(RankEntity.CountInd.ToString()))
                    sqlParamList.Add(new SqlParameter("@RNKCRIT2_COUNT_IND", RankEntity.CountInd));
                if (!string.IsNullOrEmpty(RankEntity.AgeClcInd.ToString()))
                    sqlParamList.Add(new SqlParameter("@RNKCRIT2_AGECLC_IND", RankEntity.AgeClcInd));
                sqlParamList.Add(new SqlParameter("@RNKCRIT2_RESP_CODE", RankEntity.RespCd));
                if (!string.IsNullOrEmpty(RankEntity.RespText.ToString()))
                    sqlParamList.Add(new SqlParameter("@RNKCRIT2_RESP_TEXT", RankEntity.RespText));
                if (!string.IsNullOrEmpty(RankEntity.EqNum.ToString()))
                    sqlParamList.Add(new SqlParameter("@RNKCRIT2_RESP_EQ_NUM", RankEntity.EqNum));
                if (!string.IsNullOrEmpty(RankEntity.GtNum.ToString()))
                    sqlParamList.Add(new SqlParameter("@RNKCRIT2_RESP_GT_NUM", RankEntity.GtNum));
                if (!string.IsNullOrEmpty(RankEntity.LtNum.ToString()))
                    sqlParamList.Add(new SqlParameter("@RNKCRIT2_RESP_LT_NUM", RankEntity.LtNum));
                if (!string.IsNullOrEmpty(RankEntity.EqDate.ToString()))
                    sqlParamList.Add(new SqlParameter("@RNKCRIT2_RESP_EQ_DATE", RankEntity.EqDate));
                if (!string.IsNullOrEmpty(RankEntity.GtDate.ToString()))
                    sqlParamList.Add(new SqlParameter("@RNKCRIT2_RESP_GT_DATE", RankEntity.GtDate));
                if (!string.IsNullOrEmpty(RankEntity.LtDate.ToString()))
                    sqlParamList.Add(new SqlParameter("@RNKCRIT2_RESP_LT_DATE", RankEntity.LtDate));
                if (!string.IsNullOrEmpty(RankEntity.Points.ToString()))
                    sqlParamList.Add(new SqlParameter("@RNKCRIT2_POINTS", int.Parse(RankEntity.Points)));
                sqlParamList.Add(new SqlParameter("@RNKCRIT2_ADD_OPERATOR", RankEntity.Addoperator));
                sqlParamList.Add(new SqlParameter("@RNKCRIT2_LSTC_OPERATOR", RankEntity.LstcOperator));
                sqlParamList.Add(new SqlParameter("@MODE", RankEntity.Mode));
                sqlParamList.Add(new SqlParameter("@RankCd", RankEntity.RankCd));

                SqlParameter parameterMsg = new SqlParameter("@CURRENTSEQ", SqlDbType.Int, 5);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);

                SqlParameter parameterDel = new SqlParameter("@DELFLAG", SqlDbType.Char, 1);
                parameterDel.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterDel);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsertRNKCRIT2(sqlParamList);
                New_Seq = int.Parse(parameterMsg.Value.ToString());
                Del_Flag = parameterDel.Value.ToString();
            }
            catch (Exception ex)
            {
                return false;
            }
            return boolsuccess;
        }

        public List<RNKCRIT2Entity> Browse_RankCritiria2(string ID)
        {
            List<RNKCRIT2Entity> RankCritiria2profile = new List<RNKCRIT2Entity>();
            try
            {
                DataSet RankCritiria2Data = SPAdminDB.BrowseRNKCRIT2(ID);
                if (RankCritiria2Data != null && RankCritiria2Data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in RankCritiria2Data.Tables[0].Rows)
                        RankCritiria2profile.Add(new RNKCRIT2Entity(row));
                }
            }
            catch (Exception ex)
            { return RankCritiria2profile; }

            return RankCritiria2profile;
        }

        //*************************************************************************************************

        public List<SqlParameter> Prepare_CASEVDD_SqlParameters_List(CASEVDDEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));
                if (!string.IsNullOrEmpty(Entity.Code))
                    sqlParamList.Add(new SqlParameter("@CASEVDD_CODE", Entity.Code));
                sqlParamList.Add(new SqlParameter("@CASEVDD_ACTIVE", Entity.Active));
                if (!string.IsNullOrEmpty(Entity.Name))
                    sqlParamList.Add(new SqlParameter("@CASEVDD_NAME", Entity.Name));
                if (!string.IsNullOrEmpty(Entity.Addr1))
                    sqlParamList.Add(new SqlParameter("@CASEVDD_ADDR1", Entity.Addr1));
                if (!string.IsNullOrEmpty(Entity.Addr2))
                    sqlParamList.Add(new SqlParameter("@CASEVDD_ADDR2", Entity.Addr2));
                if (!string.IsNullOrEmpty(Entity.Addr3))
                    sqlParamList.Add(new SqlParameter("@CASEVDD_ADDR3", Entity.Addr3));
                sqlParamList.Add(new SqlParameter("@CASEVDD_CITY", Entity.City));
                sqlParamList.Add(new SqlParameter("@CASEVDD_STATE", Entity.State));
                sqlParamList.Add(new SqlParameter("@CASEVDD_ZIP", Entity.Zip));
                if (!string.IsNullOrEmpty(Entity.Phone))
                    sqlParamList.Add(new SqlParameter("@CASEVDD_TELEPHONE", Entity.Phone));
                if (!string.IsNullOrEmpty(Entity.Tax_Id))
                    sqlParamList.Add(new SqlParameter("@CASEVDD_TAX_ID", Entity.Tax_Id));
                if (!string.IsNullOrEmpty(Entity.Tax_type))
                    sqlParamList.Add(new SqlParameter("@CASEVDD_TAX_ID_TYPE", Entity.Tax_type));
                if (!string.IsNullOrEmpty(Entity.Vdd1099))
                    sqlParamList.Add(new SqlParameter("@CASEVDD_1099", Entity.Vdd1099));
                if (!string.IsNullOrEmpty(Entity.Cont_Name))
                    sqlParamList.Add(new SqlParameter("@CASEVDD_CONTACT_NAME", Entity.Cont_Name));
                if (!string.IsNullOrEmpty(Entity.Cont_Phone))
                    sqlParamList.Add(new SqlParameter("@CASEVDD_CONTACT_PHONE", Entity.Cont_Phone));
                if (!string.IsNullOrEmpty(Entity.Cont_Ext))
                    sqlParamList.Add(new SqlParameter("@CASEVDD_CONTACT_EXT", Entity.Cont_Ext));
                if (!string.IsNullOrEmpty(Entity.Fax))
                    sqlParamList.Add(new SqlParameter("@CASEVDD_FAX", Entity.Fax));
                if (!string.IsNullOrEmpty(Entity.Name_On_Checks))
                    sqlParamList.Add(new SqlParameter("@CASEVDD_NAME_ON_CHECKS", Entity.Name_On_Checks));
                if (!string.IsNullOrEmpty(Entity.Email))
                    sqlParamList.Add(new SqlParameter("@CASEVDD_EMAIL", Entity.Email));
                if (!string.IsNullOrEmpty(Entity.SPLINSTR))
                    sqlParamList.Add(new SqlParameter("@CASEVDD_SPECINSTR", Entity.SPLINSTR));

                if (!string.IsNullOrEmpty(Entity.W9))
                    sqlParamList.Add(new SqlParameter("@CASEVDD_W9", Entity.W9));
                if (!string.IsNullOrEmpty(Entity.FName))
                    sqlParamList.Add(new SqlParameter("@CASEVDD_FNAME", Entity.FName));
                if (!string.IsNullOrEmpty(Entity.LName))
                    sqlParamList.Add(new SqlParameter("@CASEVDD_LNAME", Entity.LName));
                if (!string.IsNullOrEmpty(Entity.Orginization))
                    sqlParamList.Add(new SqlParameter("@CASEVDD_ORGNZN", Entity.Orginization));
                if (!string.IsNullOrEmpty(Entity.VDD_Agency))
                    sqlParamList.Add(new SqlParameter("@CASEVDD_AGENCY", Entity.VDD_Agency));

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<CASEVDDEntity> Browse_CASEVDD(CASEVDDEntity Entity, string Opretaion_Mode)
        {
            List<CASEVDDEntity> CASEVDDProfile = new List<CASEVDDEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASEVDD_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CASEVDD]");

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASEVDDProfile.Add(new CASEVDDEntity(row));
                }
            }
            catch (Exception ex)
            { return CASEVDDProfile; }

            return CASEVDDProfile;
        }

        public List<CASEVDDEntity> Vendor_Search(string strSearchby,string strSearchSource)
        {
            List<CASEVDDEntity> CASEVDDProfile = new List<CASEVDDEntity>();
           
            try
            {
               
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Vendor_Search(strSearchby, strSearchSource);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASEVDDProfile.Add(new CASEVDDEntity(row));
                }
            }
            catch (Exception ex)
            { return CASEVDDProfile; }

            return CASEVDDProfile;
        }


        public List<SqlParameter> Prepare_CASEVDD1_SqlParameters_List(CaseVDD1Entity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@CASEVDD1_CODE", Entity.Code));
                if (!string.IsNullOrEmpty(Entity.IndexBy))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_INDEXBY", Entity.IndexBy));
                if (!string.IsNullOrEmpty(Entity.Name2))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_NAME2", Entity.Name2));
                if (!string.IsNullOrEmpty(Entity.Type))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_TYPE", Entity.Type));
                if (!string.IsNullOrEmpty(Entity.BULK_CODE))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_BULK_CODE", Entity.BULK_CODE));
                if (!string.IsNullOrEmpty(Entity.BULK_DEL))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_BULK_DEL", Entity.BULK_DEL));   //int
                if (!string.IsNullOrEmpty(Entity.BULK_ALLOT))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_BULK_ALLOT", Entity.BULK_ALLOT));
                if (!string.IsNullOrEmpty(Entity.BULK_USED))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_BULK_USED", Entity.BULK_USED));
                if (!string.IsNullOrEmpty(Entity.TOTAL_PAID))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_TOTAL_PAID", Entity.TOTAL_PAID));
                if (!string.IsNullOrEmpty(Entity.PRI_YR_PAID))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_PRI_YR_PAID", Entity.PRI_YR_PAID));
                if (!string.IsNullOrEmpty(Entity.TOT_APPS))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_TOT_APPS", Entity.TOT_APPS));
                if (!string.IsNullOrEmpty(Entity.SSNO))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_SSNO", Entity.SSNO));
                if (!string.IsNullOrEmpty(Entity.CYCLE))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_CYCLE", Entity.CYCLE));
                sqlParamList.Add(new SqlParameter("@CASEVDD1_ELEC_TRANSFER", Entity.ELEC_TRANSFER));
                if (!string.IsNullOrEmpty(Entity.PRT_PAYREQ))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_PRT_PAYREQ", Entity.PRT_PAYREQ));
                if (!string.IsNullOrEmpty(Entity.AWT_PYMT))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_AWT_PYMT", Entity.AWT_PYMT));
                if (!string.IsNullOrEmpty(Entity.ACCT_FORMAT))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_ACCT_FORMAT", Entity.ACCT_FORMAT));
                if (!string.IsNullOrEmpty(Entity.USE_VENDOR_CODE))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_USE_VENDOR_CODE", Entity.USE_VENDOR_CODE));
                if (!string.IsNullOrEmpty(Entity.YTD_NO_CHECKS))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_YTD_NO_CHECKS", Entity.YTD_NO_CHECKS));
                if (!string.IsNullOrEmpty(Entity.DATE_LAST_CHECK))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_DATE_LAST_CHECK", Entity.DATE_LAST_CHECK));
                if (!string.IsNullOrEmpty(Entity.MONTHLY_OTHER))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_MONTHLY_OTHER", Entity.MONTHLY_OTHER));
                if (!string.IsNullOrEmpty(Entity.LAST_CHECK_NO))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_LAST_CHECK_NO", Decimal.Parse(Entity.LAST_CHECK_NO)));
                if (!string.IsNullOrEmpty(Entity.UTILITY_CD1))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_UTILITY_CD1", Entity.UTILITY_CD1));
                if (!string.IsNullOrEmpty(Entity.UTILITY_CD2))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_UTILITY_CD2", Entity.UTILITY_CD2));
                if (!string.IsNullOrEmpty(Entity.STATE_APPS))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_STATE_APPS", Entity.STATE_APPS));
                if (!string.IsNullOrEmpty(Entity.STATE_PAID))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_STATE_PAID", Entity.STATE_PAID));
                if (!string.IsNullOrEmpty(Entity.WAP_APPS))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_WAP_APPS", Entity.WAP_APPS));
                if (!string.IsNullOrEmpty(Entity.WAP_PAID))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_WAP_PAID", Entity.WAP_PAID));
                if (!string.IsNullOrEmpty(Entity.FUEL_TYPE1))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_FUEL_TYPE1", Entity.FUEL_TYPE1));
                if (!string.IsNullOrEmpty(Entity.FUEL_TYPE2))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_FUEL_TYPE2", Entity.FUEL_TYPE2));
                if (!string.IsNullOrEmpty(Entity.FUEL_TYPE3))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_FUEL_TYPE3", Entity.FUEL_TYPE3));
                if (!string.IsNullOrEmpty(Entity.FUEL_TYPE4))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_FUEL_TYPE4", Entity.FUEL_TYPE4));
                if (!string.IsNullOrEmpty(Entity.FUEL_TYPE5))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_FUEL_TYPE5", Entity.FUEL_TYPE5));
                if (!string.IsNullOrEmpty(Entity.FUEL_TYPE6))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_FUEL_TYPE6", Entity.FUEL_TYPE6));
                if (!string.IsNullOrEmpty(Entity.FUEL_TYPE7))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_FUEL_TYPE7", Entity.FUEL_TYPE7));
                if (!string.IsNullOrEmpty(Entity.FUEL_TYPE8))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_FUEL_TYPE8", Entity.FUEL_TYPE8));
                if (!string.IsNullOrEmpty(Entity.FUEL_TYPE9))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_FUEL_TYPE9", Entity.FUEL_TYPE9));
                if (!string.IsNullOrEmpty(Entity.FUEL_TYPE10))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_FUEL_TYPE10", Entity.FUEL_TYPE10));
                if (!string.IsNullOrEmpty(Entity.TEMP_VENDOR))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_TEMP_VENDOR", Entity.TEMP_VENDOR));
                if (!string.IsNullOrEmpty(Entity.ME_CODE))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_ME_CODE", Entity.ME_CODE));
                if (!string.IsNullOrEmpty(Entity.ME_CERT))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_ME_CERT", Entity.ME_CERT));
                if (!string.IsNullOrEmpty(Entity.ME_AUTH))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_ME_AUTH", Entity.ME_AUTH));
                if (!string.IsNullOrEmpty(Entity.ME_EDATE))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_ME_EDATE", Entity.ME_EDATE));
                if (!string.IsNullOrEmpty(Entity.DAYS))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_DAYS", Entity.DAYS));
                sqlParamList.Add(new SqlParameter("@CASEVDD1_SEL_CYCLE", Entity.SEL_CYCLE));
                if (!string.IsNullOrEmpty(Entity.HTOTAL_PAID))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_HTOTAL_PAID", Decimal.Parse(Entity.HTOTAL_PAID)));
                if (!string.IsNullOrEmpty(Entity.HPRI_YR_PAID))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_HPRI_YR_PAID", Entity.HPRI_YR_PAID));
                if (!string.IsNullOrEmpty(Entity.HTOT_APPS))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_HTOT_APPS", Entity.HTOT_APPS));
                if (!string.IsNullOrEmpty(Entity.HAWT_PYMT))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_HAWT_PYMT", Entity.HAWT_PYMT));
                if (!string.IsNullOrEmpty(Entity.HYTD_NO_CHECKS))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_HYTD_NO_CHECKS", Entity.HYTD_NO_CHECKS));
                if (!string.IsNullOrEmpty(Entity.HDATE_LAST_CHECK))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_HDATE_LAST_CHECK", Entity.HDATE_LAST_CHECK));
                if (!string.IsNullOrEmpty(Entity.HMONTHLY_OTHER))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_HMONTHLY_OTHER", Entity.HMONTHLY_OTHER));
                if (!string.IsNullOrEmpty(Entity.HLAST_CHECK_NO))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_HLAST_CHECK_NO", Entity.HLAST_CHECK_NO));
                if (!string.IsNullOrEmpty(Entity.HSTATE_APPS))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_HSTATE_APPS", Entity.HSTATE_APPS));
                if (!string.IsNullOrEmpty(Entity.HSTATE_PAID))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_HSTATE_PAID", Entity.HSTATE_PAID));
                if (!string.IsNullOrEmpty(Entity.HWAP_APPS))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_HWAP_APPS", Entity.HWAP_APPS));
                if (!string.IsNullOrEmpty(Entity.HWAP_PAID))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_HWAP_PAID", Entity.HWAP_PAID));
                if (!string.IsNullOrEmpty(Entity.SVENDOR_CODE))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_SVENDOR_CODE", Decimal.Parse(Entity.SVENDOR_CODE)));
                if (!string.IsNullOrEmpty(Entity.AR))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_AR", Entity.AR));
                if (!string.IsNullOrEmpty(Entity.EINSSN_TYPE))
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_1099_TYPE", Entity.EINSSN_TYPE));
                sqlParamList.Add(new SqlParameter("@CASEVDD1_LSTC_OPERATOR", Entity.Lsct_Operator));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_DATE_LSTC", Entity.Lstc_Date));
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_DATE_ADD", Entity.Add_Date));
                    sqlParamList.Add(new SqlParameter("@CASEVDD1_ADD_OPERATOR", Entity.Add_Operator));
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<CaseVDD1Entity> Browse_CASEVDD1(CaseVDD1Entity Entity, string Opretaion_Mode)
        {
            List<CaseVDD1Entity> CASEVDD1Profile = new List<CaseVDD1Entity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASEVDD1_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CASEVDD1]");

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASEVDD1Profile.Add(new CaseVDD1Entity(row));
                }
            }
            catch (Exception ex)
            { return CASEVDD1Profile; }

            return CASEVDD1Profile;
        }


        public bool UpdateCASEVDD(CASEVDDEntity Entity, string Operation_Mode, out string Msg, out string Sql_Reslut_Msg)
        {
            bool boolsuccess = true;
            Sql_Reslut_Msg = "Success";
            Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList = Prepare_CASEVDD_SqlParameters_List(Entity, Operation_Mode);

                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.Update_CASEVDD", out Sql_Reslut_Msg);  //

                Msg = DeleteMsg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateCASEVDD1(CaseVDD1Entity Entity, string Operation_Mode, out string Sql_SP_Result_Message)
        {
            bool boolsuccess;
            Sql_SP_Result_Message = Consts.Common.DB_Exception;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_CASEVDD1_SqlParameters_List(Entity, Operation_Mode);
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.Update_CASEVDD1", out Sql_SP_Result_Message);  //
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateCSB16DTR(Csb16DTREntity Entity, string Operation_Mode, out string Sql_SP_Result_Message)
        {
            bool boolsuccess;
            Sql_SP_Result_Message = Consts.Common.DB_Exception;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_Csb16DTR_SqlParameters_List(Entity, Operation_Mode);
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.Update_CSB16DTR", out Sql_SP_Result_Message);  //
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }



        public List<SqlParameter> Prepare_Csb16DTR_SqlParameters_List(Csb16DTREntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@row_Type", Entity.row_Type));

                sqlParamList.Add(new SqlParameter("@C16DTR_REF_FDATE", Entity.REF_FDATE));
                if (!string.IsNullOrEmpty(Entity.REF_TDATE))
                    sqlParamList.Add(new SqlParameter("@C16DTR_REF_TDATE", Entity.REF_TDATE));
                if (!string.IsNullOrEmpty(Entity.REF_ACTIVE))
                    sqlParamList.Add(new SqlParameter("@C16DTR_REF_ACTIVE", Entity.REF_ACTIVE));
                if (!string.IsNullOrEmpty(Entity.REP_FDATE1))
                    sqlParamList.Add(new SqlParameter("@C16DTR_REP_FDATE1", Entity.REP_FDATE1));
                if (!string.IsNullOrEmpty(Entity.REP_TDATE1))
                    sqlParamList.Add(new SqlParameter("@C16DTR_REP_TDATE1", Entity.REP_TDATE1));
                if (!string.IsNullOrEmpty(Entity.REP_ACTIVE1))
                    sqlParamList.Add(new SqlParameter("@C16DTR_REP_ACTIVE1", Entity.REP_ACTIVE1));   //int
                if (!string.IsNullOrEmpty(Entity.REP_FDATE2))
                    sqlParamList.Add(new SqlParameter("@C16DTR_REP_FDATE2", Entity.REP_FDATE2));
                if (!string.IsNullOrEmpty(Entity.REP_TDATE2))
                    sqlParamList.Add(new SqlParameter("@C16DTR_REP_TDATE2", Entity.REP_TDATE2));
                if (!string.IsNullOrEmpty(Entity.REP_ACTIVE2))
                    sqlParamList.Add(new SqlParameter("@C16DTR_REP_ACTIVE2", Entity.REP_ACTIVE2));
                if (!string.IsNullOrEmpty(Entity.REP_FDATE3))
                    sqlParamList.Add(new SqlParameter("@C16DTR_REP_FDATE3", Entity.REP_FDATE3));
                if (!string.IsNullOrEmpty(Entity.REP_TDATE3))
                    sqlParamList.Add(new SqlParameter("@C16DTR_REP_TDATE3", Entity.REP_TDATE3));
                if (!string.IsNullOrEmpty(Entity.REP_ACTIVE3))
                    sqlParamList.Add(new SqlParameter("@C16DTR_REP_ACTIVE3", Entity.REP_ACTIVE3));
                if (!string.IsNullOrEmpty(Entity.REP_TDATE4))
                    sqlParamList.Add(new SqlParameter("@C16DTR_REP_FDATE4", Entity.REP_FDATE4));
                sqlParamList.Add(new SqlParameter("@C16DTR_REP_TDATE4", Entity.REP_TDATE4));
                if (!string.IsNullOrEmpty(Entity.REP_ACTIVE4))
                    sqlParamList.Add(new SqlParameter("@C16DTR_REP_ACTIVE4", Entity.REP_ACTIVE4));

                sqlParamList.Add(new SqlParameter("@C16DTR_lstc_operator", Entity.lstc_operator));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@C16DTR_DATE_LSTC", Entity.DATE_LSTC));
                    sqlParamList.Add(new SqlParameter("@C16DTR_DATE_ADD", Entity.DATE_ADD));
                    sqlParamList.Add(new SqlParameter("@C16DTR_ADD_OPERATOR", Entity.ADD_OPERATOR));
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<Csb16DTREntity> Browse_CSB16DTR(Csb16DTREntity Entity, string Opretaion_Mode)
        {
            List<Csb16DTREntity> CASEVDD1Profile = new List<Csb16DTREntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_Csb16DTR_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CSB16DTR]");

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASEVDD1Profile.Add(new Csb16DTREntity(row));
                }
            }
            catch (Exception ex)
            { return CASEVDD1Profile; }

            return CASEVDD1Profile;
        }

        public List<SqlParameter> Prepare_ChldBM_SqlParameters_List(ChldBMEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@row_Type", Entity.row_Type));

                sqlParamList.Add(new SqlParameter("@CHLDBM_AGENCY", Entity.ChldBMAgency));
                if (!string.IsNullOrEmpty(Entity.chldBMDept))
                    sqlParamList.Add(new SqlParameter("@CHLDBM_DEPT", Entity.chldBMDept));
                if (!string.IsNullOrEmpty(Entity.ChldBMProg))
                    sqlParamList.Add(new SqlParameter("@CHLDBM_PROG", Entity.ChldBMProg));
                if (!string.IsNullOrEmpty(Entity.ChldBMNumber))
                    sqlParamList.Add(new SqlParameter("@CHLDBM_NUMBER", Entity.ChldBMNumber));
                if (!string.IsNullOrEmpty(Entity.ChldBMYear))
                    sqlParamList.Add(new SqlParameter("@CHLDBM_YEAR", Entity.ChldBMYear));
                if (!string.IsNullOrEmpty(Entity.Desc))
                    sqlParamList.Add(new SqlParameter("@CHLDBM_DESC", Entity.Desc));   //int
                if (!string.IsNullOrEmpty(Entity.Make))
                    sqlParamList.Add(new SqlParameter("@CHLDBM_MAKE", Entity.Make));
                if (!string.IsNullOrEmpty(Entity.ChldBM_Type))
                    sqlParamList.Add(new SqlParameter("@CHLDBM_TYPE", Entity.ChldBM_Type));
                if (!string.IsNullOrEmpty(Entity.OL))
                    sqlParamList.Add(new SqlParameter("@CHLDBM_OL", Entity.OL));
                if (!string.IsNullOrEmpty(Entity.OL_ID))
                    sqlParamList.Add(new SqlParameter("@CHLDBM_OL_ID", Entity.OL_ID));
                if (!string.IsNullOrEmpty(Entity.OL_Date))
                    sqlParamList.Add(new SqlParameter("@CHLDBM_OL_DATE", Entity.OL_Date));
                if (!string.IsNullOrEmpty(Entity.Location1))
                    sqlParamList.Add(new SqlParameter("@CHLDBM_LOCATION1", Entity.Location1));
                if (!string.IsNullOrEmpty(Entity.Location2))
                    sqlParamList.Add(new SqlParameter("@CHLDBM_LOCATION2", Entity.Location2));
                sqlParamList.Add(new SqlParameter("@CHLDBM_REGISTRATION", Entity.Registration));
                if (!string.IsNullOrEmpty(Entity.Registration_Date))
                    sqlParamList.Add(new SqlParameter("@CHLDBM_REGISTRATION_DATE", Entity.Registration_Date));
                if (!string.IsNullOrEmpty(Entity.TelPhone))
                    sqlParamList.Add(new SqlParameter("@CHLDBM_TEL", Entity.TelPhone));
                if (!string.IsNullOrEmpty(Entity.Last_Oil_Mile))
                    sqlParamList.Add(new SqlParameter("@CHLDBM_LAST_OIL_MILE", Entity.Last_Oil_Mile));
                if (!string.IsNullOrEmpty(Entity.Last_Oil_Date))
                    sqlParamList.Add(new SqlParameter("@CHLDBM_LAST_OIL_DATE", Entity.Last_Oil_Date));
                if (!string.IsNullOrEmpty(Entity.ChldBM_Count))
                    sqlParamList.Add(new SqlParameter("@CHLDBM_COUNT", Entity.ChldBM_Count));

                sqlParamList.Add(new SqlParameter("@CHLDBM_LSTC_OPERATOR", Entity.LstcOperator));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@CHLDBM_DATE_LSTC", Entity.DateLstc));
                    sqlParamList.Add(new SqlParameter("@CHLDBM_DATE_ADD", Entity.DateAdd));
                    sqlParamList.Add(new SqlParameter("@CHLDBM_ADD_OPERATOR", Entity.AddOperator));
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<ChldBMEntity> Browse_ChldBM(ChldBMEntity Entity, string Opretaion_Mode)
        {
            List<ChldBMEntity> CASEVDD1Profile = new List<ChldBMEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_ChldBM_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CHLDBUSM]");
                DataTable dt = CASESPMData.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    DataView dv = new DataView(dt);
                    if (Entity.Sort == "D") dv.Sort = "CHLDBM_DESC"; else dv.Sort = "CHLDBM_NUMBER";
                    dt = dv.ToTable();
                    if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                            CASEVDD1Profile.Add(new ChldBMEntity(row));
                    }
                }
            }
            catch (Exception ex)
            { return CASEVDD1Profile; }

            return CASEVDD1Profile;
        }

        public bool UpdateChldBM(ChldBMEntity Entity, string Operation_Mode, out string Msg, out string Sql_Reslut_Msg)
        {
            bool boolsuccess = true;
            Sql_Reslut_Msg = "Success";
            Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList = Prepare_ChldBM_SqlParameters_List(Entity, Operation_Mode);

                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.Update_CHLDBUSM", out Sql_Reslut_Msg);  //

                Msg = DeleteMsg.Value.ToString();
                //Sql_Reslut_Msg = SP_Result_Msg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<SqlParameter> Prepare_ChldBUSR_SqlParameters_List(BusRTEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@row_Type", Entity.row_Type));

                sqlParamList.Add(new SqlParameter("@BUSRT_AGENCY", Entity.BUSRT_AGENCY));
                if (!string.IsNullOrEmpty(Entity.BUSRT_DEPT))
                    sqlParamList.Add(new SqlParameter("@BUSRT_DEPT", Entity.BUSRT_DEPT));
                if (!string.IsNullOrEmpty(Entity.BUSRT_PROGRAM))
                    sqlParamList.Add(new SqlParameter("@BUSRT_PROGRAM", Entity.BUSRT_PROGRAM));
                if (!string.IsNullOrEmpty(Entity.BUSRT_YEAR))
                    sqlParamList.Add(new SqlParameter("@BUSRT_YEAR", Entity.BUSRT_YEAR));
                if (!string.IsNullOrEmpty(Entity.BUSRT_NUMBER))
                    sqlParamList.Add(new SqlParameter("@BUSRT_NUMBER", Entity.BUSRT_NUMBER));
                if (!string.IsNullOrEmpty(Entity.BUSRT_ROUTE))
                    sqlParamList.Add(new SqlParameter("@BUSRT_ROUTE", Entity.BUSRT_ROUTE));   //int
                if (!string.IsNullOrEmpty(Entity.BUSRT_PICKUP_STARTS))
                    sqlParamList.Add(new SqlParameter("@BUSRT_PICKUP_STARTS", Entity.BUSRT_PICKUP_STARTS));
                if (!string.IsNullOrEmpty(Entity.BUSRT_ARRIVE_SCHOOL))
                    sqlParamList.Add(new SqlParameter("@BUSRT_ARRIVE_SCHOOL", Entity.BUSRT_ARRIVE_SCHOOL));
                if (!string.IsNullOrEmpty(Entity.BUSRT_LEAVES_SCHOOL))
                    sqlParamList.Add(new SqlParameter("@BUSRT_LEAVES_SCHOOL", Entity.BUSRT_LEAVES_SCHOOL));
                if (!string.IsNullOrEmpty(Entity.BUSRT_AREA_SERVED))
                    sqlParamList.Add(new SqlParameter("@BUSRT_AREA_SERVED", Entity.BUSRT_AREA_SERVED));
                if (!string.IsNullOrEmpty(Entity.BUSRT_DRIVER_NAME))
                    sqlParamList.Add(new SqlParameter("@BUSRT_DRIVER_NAME", Entity.BUSRT_DRIVER_NAME));
                if (!string.IsNullOrEmpty(Entity.BUSRT_DRIVER_DOB))
                    sqlParamList.Add(new SqlParameter("@BUSRT_DRIVER_DOB", Entity.BUSRT_DRIVER_DOB));
                if (!string.IsNullOrEmpty(Entity.BUSRT_DRIVER_TEL))
                    sqlParamList.Add(new SqlParameter("@BUSRT_DRIVER_TEL", Entity.BUSRT_DRIVER_TEL));
                sqlParamList.Add(new SqlParameter("@BUSRT_DRIVER_LIC", Entity.BUSRT_DRIVER_LIC));
                if (!string.IsNullOrEmpty(Entity.BUSRT_DRIVER_LIC_DATE))
                    sqlParamList.Add(new SqlParameter("@BUSRT_DRIVER_LIC_DATE", Entity.BUSRT_DRIVER_LIC_DATE));
                if (!string.IsNullOrEmpty(Entity.BUSRT_DRIVER_LIC_7D_DATE))
                    sqlParamList.Add(new SqlParameter("@BUSRT_DRIVER_LIC_7D_DATE", Entity.BUSRT_DRIVER_LIC_7D_DATE));
                if (!string.IsNullOrEmpty(Entity.BUSRT_DRIVER_LIC_CLD_DATE))
                    sqlParamList.Add(new SqlParameter("@BUSRT_DRIVER_LIC_CLD_DATE", Entity.BUSRT_DRIVER_LIC_CLD_DATE));
                if (!string.IsNullOrEmpty(Entity.BUSRT_DRIVER_LIC_DPU_DATE))
                    sqlParamList.Add(new SqlParameter("@BUSRT_DRIVER_LIC_DPU_DATE", Entity.BUSRT_DRIVER_LIC_DPU_DATE));
                //if (!string.IsNullOrEmpty(Entity.ChldBM_Count))
                //    sqlParamList.Add(new SqlParameter("@CHLDBM_COUNT", Entity.ChldBM_Count));

                sqlParamList.Add(new SqlParameter("@BUSRT_LSTC_OPERATOR", Entity.LstcOperator));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@BUSRT_DATE_LSTC", Entity.DateLstc));
                    sqlParamList.Add(new SqlParameter("@BUSRT_DATE_ADD", Entity.DateAdd));
                    sqlParamList.Add(new SqlParameter("@BUSRT_ADD_OPERATOR", Entity.AddOperator));
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<BusRTEntity> Browse_ChldBUSR(BusRTEntity Entity, string Opretaion_Mode)
        {
            List<BusRTEntity> CASEVDD1Profile = new List<BusRTEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_ChldBUSR_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CHLDBUSR]");

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASEVDD1Profile.Add(new BusRTEntity(row));
                }
            }
            catch (Exception ex)
            { return CASEVDD1Profile; }

            return CASEVDD1Profile;
        }

        public bool UpdateChldBusR(BusRTEntity Entity, string Operation_Mode, out string Msg, out string Sql_Reslut_Msg)
        {
            bool boolsuccess = true;
            Sql_Reslut_Msg = "Success";
            Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList = Prepare_ChldBUSR_SqlParameters_List(Entity, Operation_Mode);

                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.Update_CHLDBUSR", out Sql_Reslut_Msg);  //

                Msg = DeleteMsg.Value.ToString();
                //Sql_Reslut_Msg = SP_Result_Msg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }


        public List<SqlParameter> Prepare_ChldBUSC_SqlParameters_List(BUSCEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@row_Type", Entity.row_Type));

                sqlParamList.Add(new SqlParameter("@BUSC_AGENCY", Entity.BUSC_AGENCY));
                if (!string.IsNullOrEmpty(Entity.BUSC_DEPT))
                    sqlParamList.Add(new SqlParameter("@BUSC_DEPT", Entity.BUSC_DEPT));
                if (!string.IsNullOrEmpty(Entity.BUSC_PROG))
                    sqlParamList.Add(new SqlParameter("@BUSC_PROG", Entity.BUSC_PROG));
                if (!string.IsNullOrEmpty(Entity.BUSC_YEAR))
                    sqlParamList.Add(new SqlParameter("@BUSC_YEAR", Entity.BUSC_YEAR));
                if (!string.IsNullOrEmpty(Entity.BUSC_NUMBER))
                    sqlParamList.Add(new SqlParameter("@BUSC_NUMBER", Entity.BUSC_NUMBER));
                if (!string.IsNullOrEmpty(Entity.BUSC_ROUTE))
                    sqlParamList.Add(new SqlParameter("@BUSC_ROUTE", Entity.BUSC_ROUTE));   //int
                if (!string.IsNullOrEmpty(Entity.BUSC_CHILD))
                    sqlParamList.Add(new SqlParameter("@BUSC_CHILD", Entity.BUSC_CHILD));
                if (!string.IsNullOrEmpty(Entity.BUSC_PICKUP))
                    sqlParamList.Add(new SqlParameter("@BUSC_PICKUP", Entity.BUSC_PICKUP));
                if (!string.IsNullOrEmpty(Entity.BUSC_HOME))
                    sqlParamList.Add(new SqlParameter("@BUSC_HOME", Entity.BUSC_HOME));
                if (!string.IsNullOrEmpty(Entity.BUSC_APPLICATION))
                    sqlParamList.Add(new SqlParameter("@BUSC_APPLICATION", Entity.BUSC_APPLICATION));
                if (!string.IsNullOrEmpty(Entity.BUSC_COMMENTS))
                    sqlParamList.Add(new SqlParameter("@BUSC_COMMENTS", Entity.BUSC_COMMENTS));


                sqlParamList.Add(new SqlParameter("@BUSC_LSTC_OPERATOR", Entity.LstcOperator));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@BUSC_DATE_LSTC", Entity.DateLstc));
                    sqlParamList.Add(new SqlParameter("@BUSC_DATE_ADD", Entity.DateAdd));
                    sqlParamList.Add(new SqlParameter("@BUSC_ADD_OPERATOR", Entity.AddOperator));
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<BUSCEntity> Browse_ChldBUSC(BUSCEntity Entity, string Opretaion_Mode)
        {
            List<BUSCEntity> CASEVDD1Profile = new List<BUSCEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_ChldBUSC_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CHLDBUSC]");

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASEVDD1Profile.Add(new BUSCEntity(row));
                }
            }
            catch (Exception ex)
            { return CASEVDD1Profile; }

            return CASEVDD1Profile;
        }

        public bool UpdateChldBusC(BUSCEntity Entity, string Operation_Mode, out string Msg, out string Sql_Reslut_Msg)
        {
            bool boolsuccess = true;
            Sql_Reslut_Msg = "Success";
            Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList = Prepare_ChldBUSC_SqlParameters_List(Entity, Operation_Mode);

                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.Update_CHLDBUSC", out Sql_Reslut_Msg);  //

                Msg = DeleteMsg.Value.ToString();
                //Sql_Reslut_Msg = SP_Result_Msg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<ChildATTMSEntity> GetChldAttMsDetails(string Agency, string Dept, string Prog, string Year, string Site, string Room, string Ampm, string Month, string AttmYear, string AgyStatus, string AttmStatus, string AttmDay, string Type, string strFund = null)
        {
            List<ChildATTMSEntity> ChildATTMSEntityDetais = new List<ChildATTMSEntity>();
            try
            {
                DataSet ChldAttmds = Captain.DatabaseLayer.SPAdminDB.GetChldAttMsDetails(Agency, Dept, Prog, Year, Site, Room, Ampm, Month, AttmYear, AgyStatus, AttmStatus, AttmDay, Type, strFund);
                if (ChldAttmds != null && ChldAttmds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ChldAttmds.Tables[0].Rows)
                    {
                        ChildATTMSEntityDetais.Add(new ChildATTMSEntity(row, Type));//ChldAttms
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return ChildATTMSEntityDetais;
            }

            return ChildATTMSEntityDetais;
        }


        //public ChildATTMSEntity GetChldAttmsStatus(string Agency, string Dept, string Prog, string Year, string Site, string Room, string Ampm, string Month, string AttmYear, string AgyStatus, string AttmStatus, string AttmDay,string Type)
        //{
        //    ChildATTMSEntity ChldAttmsDetails = null;
        //    try
        //    {
        //        DataSet ChldAttmds = Captain.DatabaseLayer.SPAdminDB.GetChldAttMsDetails(Agency, Dept, Prog, Year, Site,Room,Ampm, Month, AttmYear, AgyStatus, AttmStatus, AttmDay,Type,null);
        //        if (ChldAttmds != null && ChldAttmds.Tables[0].Rows.Count > 0)
        //        {
        //            ChldAttmsDetails = new ChildATTMSEntity(ChldAttmds.Tables[0].Rows[0], "ChldAttms");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //
        //        return ChldAttmsDetails;
        //    }

        //    return ChldAttmsDetails;
        //}

        public List<RNKCRIT2Entity> GetRanksCrit2Data(string Type, string Agency, string RankCtg)
        {
            List<RNKCRIT2Entity> RankCritiria2profile = new List<RNKCRIT2Entity>();
            try
            {
                DataSet RankCritiria2Data = SPAdminDB.Get_RankQuesData(Type, Agency, RankCtg);
                if (RankCritiria2Data != null && RankCritiria2Data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in RankCritiria2Data.Tables[0].Rows)
                        RankCritiria2profile.Add(new RNKCRIT2Entity(row, Type));
                }
            }
            catch (Exception ex)
            { return RankCritiria2profile; }

            return RankCritiria2profile;
        }


        public List<CaseVDD1Entity> GetVddOrVdd1Data(string VddCode, string VddActive, string VddType, string VddFuelType, string Type)
        {
            List<CaseVDD1Entity> CASEVDD1Profile = new List<CaseVDD1Entity>();

            try
            {
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.GetVddOrVdd1Data(VddCode, VddActive, VddType, VddFuelType, Type);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASEVDD1Profile.Add(new CaseVDD1Entity(row, string.Empty));
                }
            }
            catch (Exception ex)
            { return CASEVDD1Profile; }

            return CASEVDD1Profile;
        }


        public List<CAVoucherEntity> Browse_CAVoucher(CAVoucherEntity Entity, string Opretaion_Mode)
        {
            List<CAVoucherEntity> CASEVDD1Profile = new List<CAVoucherEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CAVoucher_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CAVouchGen]");

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASEVDD1Profile.Add(new CAVoucherEntity(row));
                }
            }
            catch (Exception ex)
            { return CASEVDD1Profile; }

            return CASEVDD1Profile;
        }

        public List<VouchDefEntity> Browse_VouchGen(VouchDefEntity Entity, string Opretaion_Mode)
        {
            List<VouchDefEntity> CASEVDD1Profile = new List<VouchDefEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_VouchGen_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[BROWSE_voucher_Definition]");

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASEVDD1Profile.Add(new VouchDefEntity(row));
                }
            }
            catch (Exception ex)
            { return CASEVDD1Profile; }

            return CASEVDD1Profile;
        }

        public List<SqlParameter> Prepare_CAVoucher_SqlParameters_List(CAVoucherEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {

                if (!string.IsNullOrEmpty(Entity.Desc1))
                    sqlParamList.Add(new SqlParameter("@Desc1", Entity.Desc1));
                if (!string.IsNullOrEmpty(Entity.VCode))
                    sqlParamList.Add(new SqlParameter("@VCode", Entity.VCode));
                if (!string.IsNullOrEmpty(Entity.Type))
                    sqlParamList.Add(new SqlParameter("@Type", Entity.Type));
                if (!string.IsNullOrEmpty(Entity.Code))
                    sqlParamList.Add(new SqlParameter("@Code", Entity.Code));
                if (!string.IsNullOrEmpty(Entity.Desc2))
                    sqlParamList.Add(new SqlParameter("@Desc2", Entity.Desc2));
                if (!string.IsNullOrEmpty(Entity.Agency))
                    sqlParamList.Add(new SqlParameter("@AGENCY", Entity.Agency));  //int

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<SqlParameter> Prepare_VouchGen_SqlParameters_List(VouchDefEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {

                if (!string.IsNullOrEmpty(Entity.Vouch_Agency))
                    sqlParamList.Add(new SqlParameter("@Param_Vouch_Agency", Entity.Vouch_Agency));
                if (!string.IsNullOrEmpty(Entity.Vouch_Type))
                    sqlParamList.Add(new SqlParameter("@Param_Vouch_Type", Entity.Vouch_Type));
                if (!string.IsNullOrEmpty(Entity.Vouch_Seq))
                    sqlParamList.Add(new SqlParameter("@Param_Vouch_Seq", Entity.Vouch_Seq));
                if (!string.IsNullOrEmpty(Entity.Vouch_Length))
                    sqlParamList.Add(new SqlParameter("@Param_Vouch_Length", Entity.Vouch_Length));
                if (!string.IsNullOrEmpty(Entity.Vouch_Assoc))
                    sqlParamList.Add(new SqlParameter("@Param_Vouch_Assoc", Entity.Vouch_Assoc));   //int
                if (!string.IsNullOrEmpty(Entity.Vouch_LSTC_Operator))
                    sqlParamList.Add(new SqlParameter("@Param_Vouch_LSTC_Operator", Entity.Vouch_LSTC_Operator));

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<CAVDatesEntity> Browse_CAVDates(CAVDatesEntity Entity, string Opretaion_Mode)
        {
            List<CAVDatesEntity> CASEVDD1Profile = new List<CAVDatesEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CAVDates_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CAVDates]");

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASEVDD1Profile.Add(new CAVDatesEntity(row));
                }
            }
            catch (Exception ex)
            { return CASEVDD1Profile; }

            return CASEVDD1Profile;
        }

        public List<SqlParameter> Prepare_CAVDates_SqlParameters_List(CAVDatesEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {

                if (!string.IsNullOrEmpty(Entity.CAVD_AGENCY))
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVD_AGENCY", Entity.CAVD_AGENCY));
                if (!string.IsNullOrEmpty(Entity.CAVD_ACTIVE))
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVD_ACTIVE", Entity.CAVD_ACTIVE));
                if (!string.IsNullOrEmpty(Entity.CAVD_FDate))
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVD_FDate", Entity.CAVD_FDate));
                if (!string.IsNullOrEmpty(Entity.CAVD_TDate))
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVD_TDate", Entity.CAVD_TDate));
                
                if (!string.IsNullOrEmpty(Entity.CAVD_LSTC_OPERATOR))
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVD_LSTC_OPERATOR", Entity.CAVD_LSTC_OPERATOR));

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }


        public List<CAVACNTFORMATEntity> Browse_CAVACNTFORMAT(CAVACNTFORMATEntity Entity, string Opretaion_Mode)
        {
            List<CAVACNTFORMATEntity> CASEVDD1Profile = new List<CAVACNTFORMATEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CAVACNTFORMATEntity_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CAVACNTFORMAT]");

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASEVDD1Profile.Add(new CAVACNTFORMATEntity(row));
                }
            }
            catch (Exception ex)
            { return CASEVDD1Profile; }

            return CASEVDD1Profile;
        }

        public List<SqlParameter> Prepare_CAVACNTFORMATEntity_SqlParameters_List(CAVACNTFORMATEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {

                if (!string.IsNullOrEmpty(Entity.CAVACCF_AGENCY))
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVACCF_AGENCY", Entity.CAVACCF_AGENCY));
                if (!string.IsNullOrEmpty(Entity.CAVACCF_SEQ))
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVACCF_SEQ", Entity.CAVACCF_SEQ));
                if (!string.IsNullOrEmpty(Entity.CAVACCF_Type))
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVACCF_Type", Entity.CAVACCF_Type));
                if (!string.IsNullOrEmpty(Entity.CAVACCF_Item))
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVACCF_Item", Entity.CAVACCF_Item));

                if (!string.IsNullOrEmpty(Entity.CAVACCF_Length))
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVACCF_Length", Entity.CAVACCF_Length));

                if (!string.IsNullOrEmpty(Entity.CAVACCF_ADD_OPERATOR))
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVACCF_ADD_OPERATOR", Entity.CAVACCF_ADD_OPERATOR));

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<CAVASSOCEntity> Browse_CAVASSOC(CAVASSOCEntity Entity, string Opretaion_Mode)
        {
            List<CAVASSOCEntity> CASEVDD1Profile = new List<CAVASSOCEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CAVASSOC_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CAVASSOC]");

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASEVDD1Profile.Add(new CAVASSOCEntity(row));
                }
            }
            catch (Exception ex)
            { return CASEVDD1Profile; }

            return CASEVDD1Profile;
        }

        public List<SqlParameter> Prepare_CAVASSOC_SqlParameters_List(CAVASSOCEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {

                if (!string.IsNullOrEmpty(Entity.CAVASSOC_AGENCY))
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVASSOC_AGENCY", Entity.CAVASSOC_AGENCY));
                if (!string.IsNullOrEmpty(Entity.CAVASSOC_CAVD_ID))
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVASSOC_CAVD_ID", Entity.CAVASSOC_CAVD_ID));
                if (!string.IsNullOrEmpty(Entity.CAVASSOC_Type))
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVASSOC_Type", Entity.CAVASSOC_Type));
                if (!string.IsNullOrEmpty(Entity.CAVASSOC_Code))
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVASSOC_Code", Entity.CAVASSOC_Code));
                if (!string.IsNullOrEmpty(Entity.CAVASSOC_vCode))
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVASSOC_vCode", Entity.CAVASSOC_vCode));
                if (!string.IsNullOrEmpty(Entity.CAVASSOC_Desc))
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVASSOC_Desc", Entity.CAVASSOC_Desc));  //int
                if (!string.IsNullOrEmpty(Entity.CAVASSOC_Remarks))
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVASSOC_Remarks", Entity.CAVASSOC_Remarks));
                if (!string.IsNullOrEmpty(Entity.CAVASSOC_LSTC_OPERATOR))
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVASSOC_LSTC_OPERATOR", Entity.CAVASSOC_LSTC_OPERATOR));

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }



        public bool DeleteEMSRecords(string RepType)
        {
            bool boolSuccess = false;


            try
            {
                boolSuccess = SPAdminDB.DeleteRecords(RepType);
            }
            catch (Exception ex)
            {
                //
                return boolSuccess = false;
            }

            return boolSuccess;
        }

        public List<EAPFPEntity> Browse_EAPFP(EAPFPEntity Entity, string Opretaion_Mode)
        {
            List<EAPFPEntity> CASESPMProfile = new List<EAPFPEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_EAPFP_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_EAPFP]");
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASESPM(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new EAPFPEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<SqlParameter> Prepare_EAPFP_SqlParameters_List(EAPFPEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                //if (Opretaion_Mode != "Browse")
                //{
                //    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));
                //}

                sqlParamList.Add(new SqlParameter("@EAPFP_AGENCY", Entity.EAPFP_AGENCY));
                sqlParamList.Add(new SqlParameter("@EAPFP_DEPT", Entity.EAPFP_DEPT));
                sqlParamList.Add(new SqlParameter("@EAPFP_PROGRAM", Entity.EAPFP_PROGRAM));
                if (!string.IsNullOrEmpty(Entity.EAPFP_YEAR.Trim()))
                    sqlParamList.Add(new SqlParameter("@EAPFP_YEAR", Entity.EAPFP_YEAR));
                if (!string.IsNullOrEmpty(Entity.EAPFP_APP_NO.Trim()))
                    sqlParamList.Add(new SqlParameter("@EAPFP_APP_NO", Entity.EAPFP_APP_NO));
                if (!string.IsNullOrEmpty(Entity.EAPFP_KEY.Trim()))
                    sqlParamList.Add(new SqlParameter("@EAPFP_KEY", Entity.EAPFP_KEY));
                if (!string.IsNullOrEmpty(Entity.EAPFP_DESCRIPTION.Trim()))
                    sqlParamList.Add(new SqlParameter("@EAPFP_DESCRIPTION", Entity.EAPFP_DESCRIPTION));
                if (!string.IsNullOrEmpty(Entity.EAPFP_CLAIMDATE.Trim()))
                    sqlParamList.Add(new SqlParameter("@EAPFP_CLAIMDATE", Entity.EAPFP_CLAIMDATE));
                if (!string.IsNullOrEmpty(Entity.EAPFP_CLAIMAMOUNT.Trim()))
                    sqlParamList.Add(new SqlParameter("@EAPFP_CLAIMAMOUNT", Entity.EAPFP_CLAIMAMOUNT));
                if (!string.IsNullOrEmpty(Entity.EAPFP_PersonKey.Trim()))
                    sqlParamList.Add(new SqlParameter("@EAPFP_PersonKey", Entity.EAPFP_PersonKey));
                //sqlParamList.Add(new SqlParameter("@spm_startdate", Entity.startdate));
                //sqlParamList.Add(new SqlParameter("@spm_estdate", Entity.estdate));
                //sqlParamList.Add(new SqlParameter("@spm_compdate", Entity.compdate));
                //sqlParamList.Add(new SqlParameter("@spm_sel_branches", Entity.sel_branches));
                //sqlParamList.Add(new SqlParameter("@spm_have_addlbr", Entity.have_addlbr));
                //sqlParamList.Add(new SqlParameter("@spm_lstc_operator", Entity.lstc_operator));
                //sqlParamList.Add(new SqlParameter("@spm_def_program", Entity.Def_Program));
                //sqlParamList.Add(new SqlParameter("@spm_bulk", Entity.Def_Program));


                //if (Opretaion_Mode == "Browse")
                //{
                //    sqlParamList.Add(new SqlParameter("@spm_date_lstc", Entity.date_lstc));
                //    sqlParamList.Add(new SqlParameter("@spm_date_add", Entity.date_add));
                //    sqlParamList.Add(new SqlParameter("@spm_add_operator", Entity.add_operator));
                //    sqlParamList.Add(new SqlParameter("@spm_app_no_OtherThan", App_Not_EqualTo));

                //    sqlParamList.Add(new SqlParameter("@sp0_Desc", Entity.Sp0_Desc));
                //    sqlParamList.Add(new SqlParameter("@sp0_Validated", Entity.Sp0_Validatetd));
                //}
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<STATUSCKEntity> Browse_StatusCK(STATUSCKEntity Entity, string Opretaion_Mode)
        {
            List<STATUSCKEntity> CASESPMProfile = new List<STATUSCKEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_StatusCK_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_STATUSCK]");
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASESPM(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new STATUSCKEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<SqlParameter> Prepare_StatusCK_SqlParameters_List(STATUSCKEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                //if (Opretaion_Mode != "Browse")
                //{
                //    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));
                //}

                sqlParamList.Add(new SqlParameter("@STATUS_AGENCY", Entity.STATUS_AGENCY));
                sqlParamList.Add(new SqlParameter("@STATUS_DEPT", Entity.STATUS_DEPT));
                sqlParamList.Add(new SqlParameter("@STATUS_PROGRAM", Entity.STATUS_PROGRAM));
                if (!string.IsNullOrEmpty(Entity.STATUS_YEAR.Trim()))
                    sqlParamList.Add(new SqlParameter("@STATUS_YEAR", Entity.STATUS_YEAR));
                if (!string.IsNullOrEmpty(Entity.STATUS_APP_NO.Trim()))
                    sqlParamList.Add(new SqlParameter("@STATUS_APP_NO", Entity.STATUS_APP_NO));
                if (!string.IsNullOrEmpty(Entity.STATUS_PROGRAMCK1.Trim()))
                    sqlParamList.Add(new SqlParameter("@STATUS_PROGRAMCK1", Entity.STATUS_PROGRAMCK1));
                if (!string.IsNullOrEmpty(Entity.STATUS_STATUS1.Trim()))
                    sqlParamList.Add(new SqlParameter("@STATUS_STATUS1", Entity.STATUS_STATUS1));
                if (!string.IsNullOrEmpty(Entity.STATUS_PROGRAMCK2.Trim()))
                    sqlParamList.Add(new SqlParameter("@STATUS_PROGRAMCK2", Entity.STATUS_PROGRAMCK2));
                if (!string.IsNullOrEmpty(Entity.STATUS_STATUS2.Trim()))
                    sqlParamList.Add(new SqlParameter("@STATUS_STATUS2", Entity.STATUS_STATUS2));
                if (!string.IsNullOrEmpty(Entity.STATUS_PROGRAMCK3.Trim()))
                    sqlParamList.Add(new SqlParameter("@STATUS_PROGRAMCK3", Entity.STATUS_PROGRAMCK3));
                if (!string.IsNullOrEmpty(Entity.STATUS_STATUS3.Trim()))
                    sqlParamList.Add(new SqlParameter("@STATUS_STATUS3", Entity.STATUS_STATUS3));
                if (!string.IsNullOrEmpty(Entity.STATUS_PROGRAMCK4.Trim()))
                    sqlParamList.Add(new SqlParameter("@STATUS_PROGRAMCK4", Entity.STATUS_PROGRAMCK4));
                if (!string.IsNullOrEmpty(Entity.STATUS_STATUS4.Trim()))
                    sqlParamList.Add(new SqlParameter("@STATUS_STATUS4", Entity.STATUS_STATUS4));
                if (!string.IsNullOrEmpty(Entity.STATUS_PERSONKEY.Trim()))
                    sqlParamList.Add(new SqlParameter("@STATUS_PERSONKEY", Entity.STATUS_PERSONKEY));
                //sqlParamList.Add(new SqlParameter("@spm_have_addlbr", Entity.have_addlbr));
                //sqlParamList.Add(new SqlParameter("@spm_lstc_operator", Entity.lstc_operator));
                //sqlParamList.Add(new SqlParameter("@spm_def_program", Entity.Def_Program));
                //sqlParamList.Add(new SqlParameter("@spm_bulk", Entity.Def_Program));


                //if (Opretaion_Mode == "Browse")
                //{
                //    sqlParamList.Add(new SqlParameter("@spm_date_lstc", Entity.date_lstc));
                //    sqlParamList.Add(new SqlParameter("@spm_date_add", Entity.date_add));
                //    sqlParamList.Add(new SqlParameter("@spm_add_operator", Entity.add_operator));
                //    sqlParamList.Add(new SqlParameter("@spm_app_no_OtherThan", App_Not_EqualTo));

                //    sqlParamList.Add(new SqlParameter("@sp0_Desc", Entity.Sp0_Desc));
                //    sqlParamList.Add(new SqlParameter("@sp0_Validated", Entity.Sp0_Validatetd));
                //}
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }


        public List<TRIGPARAMEntity> GETTRIGPARAM()
        {
            List<TRIGPARAMEntity> TrigParamData = new List<TRIGPARAMEntity>();
            try
            {
                DataSet TrigParamds = SPAdminDB.GetTrigParam();
                if (TrigParamds != null && TrigParamds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TrigParamds.Tables[0].Rows)
                        TrigParamData.Add(new TRIGPARAMEntity(row));
                }
            }
            catch (Exception ex)
            {
                //
                return TrigParamData;
            }

            return TrigParamData;
        }

        /// <summary>
        /// Get User Profile information. 
        /// </summary>
        /// <param name="userID">The Zipcode ID to get ZipCode.</param>
        /// <returns>Returns a DataSet with the ZipCode's profiles.</returns>
        public bool InsertTRIGPARAM(TRIGPARAMEntity trigparam)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@TRIG_CODE", trigparam.Trigger));
                sqlParamList.Add(new SqlParameter("@HIE", trigparam.HIE));
                if (trigparam.YEAR != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@YEAR", trigparam.YEAR));
                }
                if (trigparam.SITE != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SITE", trigparam.SITE));
                }
                if (trigparam.SITE_SWT != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SITE_SWT", trigparam.SITE_SWT));
                }
                if (trigparam.CASEWORKER != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CASEWORKER", trigparam.CASEWORKER));
                }
                if (trigparam.RESULT != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@RESULT", trigparam.RESULT));
                }
                if (trigparam.OBO != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@OBO", trigparam.OBO));
                }
                if (trigparam.LOADONLY != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LOADONLY", trigparam.LOADONLY));
                }
                if (trigparam.CUMMILATIVE != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CUMMILATIVE", trigparam.CUMMILATIVE));
                }
                boolsuccess = SPAdminDB.InsertTRIGPARAM(sqlParamList);
            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public bool InsertUPDELCAVouchGen(CAVoucherEntity VouchGen)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@VCode", VouchGen.VCode));
                if (VouchGen.Code != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Code", VouchGen.Code));
                if (VouchGen.Type != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@Type", VouchGen.Type));
                }
                if (VouchGen.Desc1 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@Desc1", VouchGen.Desc1));
                }
                if (VouchGen.Desc2 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@Desc2", VouchGen.Desc2));
                }

                if (VouchGen.Agency != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@Agency", VouchGen.Agency));
                }

                sqlParamList.Add(new SqlParameter("@Mode", VouchGen.Mode));

                boolsuccess = SPAdminDB.InsertUpDelCAVouchGen(sqlParamList);
            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public bool InsertUPDELCAVASSOC(CAVASSOCEntity VouchGen)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@PARAM_CAVASSOC_AGENCY", VouchGen.CAVASSOC_AGENCY));
                if (VouchGen.CAVASSOC_CAVD_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVASSOC_CAVD_ID", VouchGen.CAVASSOC_CAVD_ID));
                if (VouchGen.CAVASSOC_Type != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVASSOC_Type", VouchGen.CAVASSOC_Type));
                }
                if (VouchGen.CAVASSOC_Code != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVASSOC_Code", VouchGen.CAVASSOC_Code));
                }
                if (VouchGen.CAVASSOC_vCode != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVASSOC_vCode", VouchGen.CAVASSOC_vCode));
                }

                if (VouchGen.CAVASSOC_Desc != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVASSOC_Desc", VouchGen.CAVASSOC_Desc));
                }

                if (VouchGen.CAVASSOC_Remarks != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVASSOC_Remarks", VouchGen.CAVASSOC_Remarks));
                }

                if (VouchGen.CAVASSOC_LSTC_OPERATOR != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVASSOC_LSTC_OPERATOR", VouchGen.CAVASSOC_LSTC_OPERATOR));
                }

                sqlParamList.Add(new SqlParameter("@Mode", VouchGen.Mode));

                boolsuccess = SPAdminDB.InsertUpDelCAVASSOC(sqlParamList);
            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public bool InsertUpDelVouchDef(VouchDefEntity VouchGen)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Param_Vouch_Agency", VouchGen.Vouch_Agency));
                if (VouchGen.Vouch_Type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Param_Vouch_Type", VouchGen.Vouch_Type));
                if (VouchGen.Vouch_Seq != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@Param_Vouch_Seq", VouchGen.Vouch_Seq));
                }
                if (VouchGen.Vouch_Length != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@Param_Vouch_Length", VouchGen.Vouch_Length));
                }
                if (VouchGen.Vouch_Assoc != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@Param_Vouch_Assoc", VouchGen.Vouch_Assoc));
                }

                if (VouchGen.Vouch_Add_operator != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@Param_Vouch_Add_operator", VouchGen.Vouch_Add_operator));
                }

                if (VouchGen.Vouch_LSTC_Operator != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@Param_Vouch_LSTC_Operator", VouchGen.Vouch_LSTC_Operator));
                }

                sqlParamList.Add(new SqlParameter("@Mode", VouchGen.Mode));

                boolsuccess = SPAdminDB.InsertUpDelVouchDef(sqlParamList);
            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public bool INSUPDELCAVDates(CAVDatesEntity VouchGen)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@PARAM_CAVD_ID", VouchGen.CAVD_ID));

                sqlParamList.Add(new SqlParameter("@PARAM_CAVD_AGENCY", VouchGen.CAVD_AGENCY));
                if (VouchGen.CAVD_ACTIVE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVD_ACTIVE", VouchGen.CAVD_ACTIVE));
                if (VouchGen.CAVD_FDate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVD_FDate", VouchGen.CAVD_FDate));
                }
                if (VouchGen.CAVD_TDate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVD_TDate", VouchGen.CAVD_TDate));
                }
                if (VouchGen.CAVD_ADD_OPERATOR != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVD_ADD_OPERATOR", VouchGen.CAVD_ADD_OPERATOR));
                }

                if (VouchGen.CAVD_LSTC_OPERATOR != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@PARAM_CAVD_LSTC_OPERATOR", VouchGen.CAVD_LSTC_OPERATOR));
                }

                //if (VouchGen.Vouch_LSTC_Operator != string.Empty)
                //{
                //    sqlParamList.Add(new SqlParameter("@Param_Vouch_LSTC_Operator", VouchGen.Vouch_LSTC_Operator));
                //}

                sqlParamList.Add(new SqlParameter("@Mode", VouchGen.Mode));

                boolsuccess = SPAdminDB.INSUPDELCAVDates(sqlParamList);
            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public List<VoucherEntity> GetPaymentVouchers( string Agency, string DateID,string Type)
        {
            List<VoucherEntity> RankCritiria2profile = new List<VoucherEntity>();
            try
            {
                DataSet RankCritiria2Data = SPAdminDB.Get_VochersData( Agency, DateID, Type);
                if (RankCritiria2Data != null && RankCritiria2Data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in RankCritiria2Data.Tables[0].Rows)
                        RankCritiria2profile.Add(new VoucherEntity(row));
                }
            }
            catch (Exception ex)
            { return RankCritiria2profile; }

            return RankCritiria2profile;
        }


        public List<CASESPMEntity> GETSPMWITHMST(string strAgency, string strDept, string strProgram, string strYear, string strApp, string strServiceplan, string strFromdt, string strTodt, string strType, string strResult)
        {
            List<CASESPMEntity> CASESPMProfile = new List<CASESPMEntity>();
            try
            {
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.GETSPMWITHMST(strAgency, strDept, strProgram, strYear, strApp, strServiceplan, strFromdt, strTodt, strType, strResult);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new CASESPMEntity(row, "SEARCHSPM"));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public bool UpdateSpmActualCompletedt(string strAgeny, string strDept, string strProgram, string strYear, string strCompletedate, string strDetailsXMl, string strType, string strOperator,string strAppno,string strServiceplan,string strspmSeq,string strBranch,string strGroup,string strMscode,string strMsDate,string strMsResult)
        {
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Agency", strAgeny));
                sqlParamList.Add(new SqlParameter("@Dept", strDept));
                sqlParamList.Add(new SqlParameter("@Program", strProgram));
                sqlParamList.Add(new SqlParameter("@Year", strYear));
                sqlParamList.Add(new SqlParameter("@CompleteDate", strCompletedate));
                sqlParamList.Add(new SqlParameter("@LSTCOPERATOR", strOperator));
                if (strDetailsXMl != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ApplicantDetails", strDetailsXMl));

                if (strAppno != string.Empty)
                    sqlParamList.Add(new SqlParameter("@AppNo", strAppno));

                if (strServiceplan != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ServicePlan", strServiceplan));

                if (strspmSeq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SpmSeq", strspmSeq));

                if (strBranch != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Branch", strBranch));

                if (strGroup != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Group", strGroup));

                if (strMscode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MSCode", strMscode));

                if (strMsDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MSDate", strMsDate));

                if (strMsResult != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MSResult", strMsResult));


                sqlParamList.Add(new SqlParameter("@Type", strType));

             
                SPAdminDB.UpdateSpmActualCompletiondate(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public List<CASEACTEntity> CASE0025_GET(string agency, string dep, string program, string year, string App, string Fund, string servicecode, string Site, string strDate, string strDateH, string strActivitycode,string strType, string strVendorL, string strVendorH)
        {
            List<CASEACTEntity> Emsclcpmcdata = new List<CASEACTEntity>();
            try
            {
                DataSet emsclcpmcds = Captain.DatabaseLayer.SPAdminDB.CASE0025_GET(agency, dep, program, year, App, Fund, servicecode, Site, strDate, strDateH, strActivitycode,  strType,strVendorL,strVendorH);
                if (emsclcpmcds != null && emsclcpmcds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsclcpmcds.Tables[0].Rows)
                    {
                        Emsclcpmcdata.Add(new CASEACTEntity(row, "CASE0025"));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsclcpmcdata;
            }

            return Emsclcpmcdata;
        }

        public List<CASEACTEntity> CASB0015_Report(string agency, string dep, string program, string year, string App, string Vendor,  string startDate, string EndDate,string RepFor)
        {
            List<CASEACTEntity> Emsclcpmcdata = new List<CASEACTEntity>();
            try
            {
                DataSet emsclcpmcds = Captain.DatabaseLayer.SPAdminDB.CASB0015_Report(agency, dep, program, year, App, Vendor, startDate, EndDate, RepFor,string.Empty);
                if (emsclcpmcds != null && emsclcpmcds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsclcpmcds.Tables[0].Rows)
                    {
                        Emsclcpmcdata.Add(new CASEACTEntity(row, "CASB0015"));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsclcpmcdata;
            }

            return Emsclcpmcdata;
        }

        public List<CASEACTEntity> CASB0015_Bundle(string agency, string dep, string program, string year, string App, string Vendor, string startDate, string EndDate, string RepFor,string Bundle)
        {
            List<CASEACTEntity> Emsclcpmcdata = new List<CASEACTEntity>();
            try
            {
                DataSet emsclcpmcds = Captain.DatabaseLayer.SPAdminDB.CASB0015_Report(agency, dep, program, year, App, Vendor, startDate, EndDate, RepFor,string.Empty);
                if (emsclcpmcds != null && emsclcpmcds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsclcpmcds.Tables[0].Rows)
                    {
                        Emsclcpmcdata.Add(new CASEACTEntity(row, "Bundle"));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsclcpmcdata;
            }

            return Emsclcpmcdata;
        }

        public List<TemplateEntity> Browse_Templates(string ID, string Code, string SP, string SPMSeq, string Branch, string Group)
        {
            List<TemplateEntity> CASESP2Profile = new List<TemplateEntity>();
            try
            {
                DataSet CASESP2Data = SPAdminDB.CAPS_BULKTEMPLATE_BROWSE(ID, Code, SP, SPMSeq,Branch,Group);
                if (CASESP2Data != null && CASESP2Data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESP2Data.Tables[0].Rows)
                        CASESP2Profile.Add(new TemplateEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESP2Profile; }

            return CASESP2Profile;
        }

        public bool UpdateBULKTEMPLATE(TemplateEntity Entity, string Operation_Mode, out string Msg, out string Sql_Reslut_Msg)
        {
            bool boolsuccess = true;
            Sql_Reslut_Msg = "Success";
            Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList = Prepare_BULKTEMPLATE_SqlParameters_List(Entity, Operation_Mode);
                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.CAPS_BULKTEMPLATE_INSUPDEL", out Sql_Reslut_Msg);  //

                Msg = DeleteMsg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<SqlParameter> Prepare_BULKTEMPLATE_SqlParameters_List(TemplateEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Mode", Entity.Rec_Type));

                if (!string.IsNullOrEmpty(Entity.BTPL_ID))
                    sqlParamList.Add(new SqlParameter("@BTPL_ID", Entity.BTPL_ID));
                if (!string.IsNullOrEmpty(Entity.BTPL_CODE))
                    sqlParamList.Add(new SqlParameter("@BTPL_CODE", Entity.BTPL_CODE));
                if (!string.IsNullOrEmpty(Entity.BTPL_DESC))
                    sqlParamList.Add(new SqlParameter("@BTPL_DESC", Entity.BTPL_DESC));
                if (!string.IsNullOrEmpty(Entity.BTPL_SERVICEPLAN))
                    sqlParamList.Add(new SqlParameter("@BTPL_SERVICEPLAN", Entity.BTPL_SERVICEPLAN));

                if (!string.IsNullOrEmpty(Entity.BTPL_SPM_SEQ))
                    sqlParamList.Add(new SqlParameter("@BTPL_SPM_SEQ", Entity.BTPL_SPM_SEQ));
                if (!string.IsNullOrEmpty(Entity.BTPL_BRANCH))
                    sqlParamList.Add(new SqlParameter("@BTPL_BRANCH", Entity.BTPL_BRANCH));
                if (!string.IsNullOrEmpty(Entity.BTPL_GROUP))
                    sqlParamList.Add(new SqlParameter("@BTPL_GROUP", Entity.BTPL_GROUP));
                if (!string.IsNullOrEmpty(Entity.BTPL_CACODE))
                    sqlParamList.Add(new SqlParameter("@BTPL_CACODE", Entity.BTPL_CACODE));
                if (!string.IsNullOrEmpty(Entity.BTPL_CASEQ))
                    sqlParamList.Add(new SqlParameter("@BTPL_CASEQ", Entity.BTPL_CASEQ));
                //if (!string.IsNullOrEmpty(Entity.Area))
                //    sqlParamList.Add(new SqlParameter("@CASEREF_AREA", Entity.Area));
                //if (!string.IsNullOrEmpty(Entity.Excgange))
                //    sqlParamList.Add(new SqlParameter("@CASEREF_EXCHANGE", Entity.Excgange));
                if (!string.IsNullOrEmpty(Entity.BTPL_DATE))
                    sqlParamList.Add(new SqlParameter("@BTPL_DATE", Entity.BTPL_DATE));
                sqlParamList.Add(new SqlParameter("@BTPL_PROG", Entity.BTPL_PROG));
                if (!string.IsNullOrEmpty(Entity.BTPL_SITE))
                    sqlParamList.Add(new SqlParameter("@BTPL_SITE", Entity.BTPL_SITE));
                if (!string.IsNullOrEmpty(Entity.BTPL_WORKER))
                    sqlParamList.Add(new SqlParameter("@BTPL_WORKER", Entity.BTPL_WORKER));
                if (!string.IsNullOrEmpty(Entity.BTPL_VENDOR))
                    sqlParamList.Add(new SqlParameter("@BTPL_VENDOR", Entity.BTPL_VENDOR));
                //if (!string.IsNullOrEmpty(Entity.Cont_Area))
                //    sqlParamList.Add(new SqlParameter("@CASEREF_CONT_AREA", Entity.Cont_Area));
                //if (!string.IsNullOrEmpty(Entity.Cont_Exchange))
                //    sqlParamList.Add(new SqlParameter("@CASEREF_CONT_EXCHANGE", Entity.Cont_Exchange));
                if (!string.IsNullOrEmpty(Entity.BTPL_FUND))
                    sqlParamList.Add(new SqlParameter("@BTPL_FUND", Entity.BTPL_FUND));
                if (!string.IsNullOrEmpty(Entity.BTPL_UOM))
                    sqlParamList.Add(new SqlParameter("@BTPL_UOM", Entity.BTPL_UOM));
                //if (!string.IsNullOrEmpty(Entity.Fax_Area))
                //    sqlParamList.Add(new SqlParameter("@CASEREF_FAX_AREA", Entity.Fax_Area));
                //if (!string.IsNullOrEmpty(Entity.Fax_Exchange))
                //    sqlParamList.Add(new SqlParameter("@CASEREF_FAX_EXCHANGE", Entity.Fax_Exchange));

                if (!string.IsNullOrEmpty(Entity.BTPL_UNITS))
                    sqlParamList.Add(new SqlParameter("@BTPL_UNITS", Entity.BTPL_UNITS));
                if (!string.IsNullOrEmpty(Entity.BTPL_RATE))
                    sqlParamList.Add(new SqlParameter("@BTPL_RATE", Entity.BTPL_RATE));
                if (!string.IsNullOrEmpty(Entity.BTPL_AMOUNT))
                    sqlParamList.Add(new SqlParameter("@BTPL_AMOUNT", Entity.BTPL_AMOUNT));
                if (!string.IsNullOrEmpty(Entity.BTPL_OBF))
                    sqlParamList.Add(new SqlParameter("@BTPL_OBF", Entity.BTPL_OBF));

                if (!string.IsNullOrEmpty(Entity.BTPL_SER_FDATE))
                    sqlParamList.Add(new SqlParameter("@BTPL_SER_FDATE", Entity.BTPL_SER_FDATE));
                if (!string.IsNullOrEmpty(Entity.BTPL_SER_TDATE))
                    sqlParamList.Add(new SqlParameter("@BTPL_SER_TDATE", Entity.BTPL_SER_TDATE));
                if (!string.IsNullOrEmpty(Entity.BTPL_SER_SITE))
                    sqlParamList.Add(new SqlParameter("@BTPL_SER_SITE", Entity.BTPL_SER_SITE));
                if (!string.IsNullOrEmpty(Entity.BTPL_SER_SORT))
                    sqlParamList.Add(new SqlParameter("@BTPL_SER_SORT", Entity.BTPL_SER_SORT));

                if (!string.IsNullOrEmpty(Entity.BTPL_SER_CASETYPE))
                    sqlParamList.Add(new SqlParameter("@BTPL_SER_CASETYPE", Entity.BTPL_SER_CASETYPE));
                if (!string.IsNullOrEmpty(Entity.BTPL_SER_TYPE))
                    sqlParamList.Add(new SqlParameter("@BTPL_SER_TYPE", Entity.BTPL_SER_TYPE));
                if (!string.IsNullOrEmpty(Entity.BTPL_SER_ESTATUS))
                    sqlParamList.Add(new SqlParameter("@BTPL_SER_ESTATUS", Entity.BTPL_SER_ESTATUS));
                if (!string.IsNullOrEmpty(Entity.BTPL_SER_QUESTION))
                    sqlParamList.Add(new SqlParameter("@BTPL_SER_QUESTION", Entity.BTPL_SER_QUESTION));
                if (!string.IsNullOrEmpty(Entity.BTPL_SER_RESPONSE))
                    sqlParamList.Add(new SqlParameter("@BTPL_SER_RESPONSE", Entity.BTPL_SER_RESPONSE));
                if (!string.IsNullOrEmpty(Entity.BTPL_SPM))
                    sqlParamList.Add(new SqlParameter("@BTPL_SPM", Entity.BTPL_SPM));
                //if (!string.IsNullOrEmpty(Entity.From_Hrs))
                //    sqlParamList.Add(new SqlParameter("@CASEREF_FROM_HRS", Entity.From_Hrs));
                //if (!string.IsNullOrEmpty(Entity.To_Hrs))
                //    sqlParamList.Add(new SqlParameter("@CASEREF_TO_HRS", Entity.To_Hrs));
                //if (!string.IsNullOrEmpty(Entity.Sec))
                //    sqlParamList.Add(new SqlParameter("@CASEREF_SEC", Entity.Sec));
                //if (!string.IsNullOrEmpty(Entity.Email.Trim()))
                //    sqlParamList.Add(new SqlParameter("@CASEREF_EMAIL", Entity.Email));

                sqlParamList.Add(new SqlParameter("@BTPL_LSTC_OPERATOR", Entity.BTPL_LSTC_OPERATOR));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@BTPL_DATE_ADD", Entity.BTPL_DATE_ADD));
                    sqlParamList.Add(new SqlParameter("@BTPL_ADD_OPERATOR", Entity.BTPL_ADD_OPERATOR));
                    sqlParamList.Add(new SqlParameter("@BTPL_DATE_LSTC", Entity.BTPL_DATE_LSTC));
                }

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<CASESP1Entity> Browse_CASESP1(string SP_code, string Agy, string Dept, string Prog)
        {
            List<CASESP1Entity> CASESP1Profile = new List<CASESP1Entity>();
            try
            {
                DataSet CASESP1Data = SPAdminDB.Browse_CASESP1(SP_code, Agy, Dept, Prog);
                if (CASESP1Data != null && CASESP1Data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESP1Data.Tables[0].Rows)
                        CASESP1Profile.Add(new CASESP1Entity(row,""));
                }
            }
            catch (Exception ex)
            { return CASESP1Profile; }

            return CASESP1Profile;
        }

        public bool InsertUpdateDelCMBDC(CMBDCEntity EmsbdcEntity, out string strMsg)
        {
            bool boolstatus = false;
            string strerrormsg = string.Empty;
            try
            {

                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@BDC_AGENCY", EmsbdcEntity.BDC_AGENCY));
                sqlParamList.Add(new SqlParameter("@BDC_DEPT", EmsbdcEntity.BDC_DEPT));
                sqlParamList.Add(new SqlParameter("@BDC_PROGRAM", EmsbdcEntity.BDC_PROGRAM));
                if (EmsbdcEntity.BDC_YEAR.Trim() != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDC_YEAR", EmsbdcEntity.BDC_YEAR));
                if (EmsbdcEntity.BDC_DESCRIPTION != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDC_DESCRIPTION", EmsbdcEntity.BDC_DESCRIPTION));

                if (EmsbdcEntity.BDC_FUND_CODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDC_FUND_CODE", EmsbdcEntity.BDC_FUND_CODE));

                if (EmsbdcEntity.BDC_BENEFIT_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDC_BENEFIT_TYPE", EmsbdcEntity.BDC_BENEFIT_TYPE));

                if (EmsbdcEntity.BDC_CONTR_NUM != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDC_CONTR_NUM", EmsbdcEntity.BDC_CONTR_NUM));

                if (EmsbdcEntity.BDC_FUND != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDC_FUND", EmsbdcEntity.BDC_FUND));
                if (EmsbdcEntity.BDC_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDC_ID", EmsbdcEntity.BDC_ID));
                if (EmsbdcEntity.BDC_BUDGET != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDC_BUDGET", EmsbdcEntity.BDC_BUDGET));
                if (EmsbdcEntity.BDC_START != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDC_START", EmsbdcEntity.BDC_START));
                if (EmsbdcEntity.BDC_END != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDC_END", EmsbdcEntity.BDC_END));
                if (EmsbdcEntity.BDC_ALLOW_POSTING != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDC_ALLOW_POSTING", EmsbdcEntity.BDC_ALLOW_POSTING));

                if (EmsbdcEntity.BDC_BALANCE != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDC_BALANCE", EmsbdcEntity.BDC_BALANCE));
                }


                if (EmsbdcEntity.BDC_TOT_EXPEND != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDC_TOT_EXPEND", EmsbdcEntity.BDC_TOT_EXPEND));
                }

                if (EmsbdcEntity.BDC_TOT_COMMIT != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDC_TOT_COMMIT", EmsbdcEntity.BDC_TOT_COMMIT));
                }
                sqlParamList.Add(new SqlParameter("@BDC_ADD_OPERATOR", EmsbdcEntity.BDC_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@BDC_LSTC_OPERATOR", EmsbdcEntity.BDC_LSTC_OPERATOR));
                sqlParamList.Add(new SqlParameter("@Mode", EmsbdcEntity.Mode));
                SqlParameter sqlsMsg = new SqlParameter("@Msg", SqlDbType.VarChar, 10);
                sqlsMsg.Value = string.Empty;
                sqlsMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlsMsg);
                boolstatus = SPAdminDB.InsertUpdateDelCMBDC(sqlParamList);
                strerrormsg = sqlsMsg.Value.ToString();
            }

            catch (Exception ex)
            {
                boolstatus = false;
            }
            strMsg = strerrormsg;
            return boolstatus;
        }


        public List<CMBDCEntity> GetCMBdcFundCheck(string agency, string dep, string program, string year, string Fund, string BDCID, string startDate, string EndDate, string NewstartDate, string NewEndDate, string strType)
        {
            List<CMBDCEntity> Emsbdcdata = new List<CMBDCEntity>();
            try
            {
                DataSet emsbdcds = Captain.DatabaseLayer.SPAdminDB.GetCMBdcFundCheck(agency, dep, program, year, Fund, BDCID, startDate, EndDate, NewstartDate, NewEndDate, strType);
                if (emsbdcds != null && emsbdcds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsbdcds.Tables[0].Rows)
                    {
                        Emsbdcdata.Add(new CMBDCEntity(row, strType));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsbdcdata;
            }

            return Emsbdcdata;
        }

        public List<CMBDCEntity> GetCMBdcAllData(string agency, string dep, string program, string year, string Desc, string Fund)
        {
            List<CMBDCEntity> Emsbdcdata = new List<CMBDCEntity>();
            try
            {
                DataSet emsbdcds = Captain.DatabaseLayer.SPAdminDB.GetCMBdcData(agency, dep, program, year, Desc, Fund);
                if (emsbdcds != null && emsbdcds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsbdcds.Tables[0].Rows)
                    {
                        Emsbdcdata.Add(new CMBDCEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsbdcdata;
            }

            return Emsbdcdata;
        }
        public List<CMBDCEntity> GetCMBDCCalAmount(string agency, string dep, string program, string year, string Fund, string BDCID, string startDate, string EndDate, string strType, string strBudget)
        {
            List<CMBDCEntity> Emsbdcdata = new List<CMBDCEntity>();
            try
            {
                DataSet emsbdcds = Captain.DatabaseLayer.SPAdminDB.GetCMBDCCalAmount(agency, dep, program, year, Fund, BDCID, startDate, EndDate, strType, strBudget);
                if (emsbdcds != null && emsbdcds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsbdcds.Tables[0].Rows)
                    {
                        Emsbdcdata.Add(new CMBDCEntity(row, strType));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsbdcdata;
            }

            return Emsbdcdata;
        }

        public bool UpdateCABUNDLE(CABUNDLEENTITY Entity, out string Sql_Reslut_Msg) //11042012     //Added by Vikash 01/03/2023(Brought update)
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_Bundle_SqlParameters_List(Entity, Entity.Rec_Type);
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateCABUNDLE", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<CASEACTEntity> GetCaseact(string agency, string dep, string program, string year, string Bundle, string Type)   //Added by Vikash 01/03/2023(Brought update)
        {
            List<CASEACTEntity> Emsbdcdata = new List<CASEACTEntity>();
            try
            {
                DataSet emsbdcds = Captain.DatabaseLayer.SPAdminDB.GetCASEACT(agency, dep, program, year, Bundle, Type);
                if (emsbdcds != null && emsbdcds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsbdcds.Tables[0].Rows)
                    {
                        if (Type == "CHK")
                            Emsbdcdata.Add(new CASEACTEntity(row, "CASB0020CHK"));
                        else if (Type == "CASB0019")
                            Emsbdcdata.Add(new CASEACTEntity(row, "CASB0019"));
                        else
                            Emsbdcdata.Add(new CASEACTEntity(row, "CASB0020"));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsbdcdata;
            }

            return Emsbdcdata;
        }
        public List<CEAPCNTLEntity> GetCEAPCNTLData(string Code, string Year, string ProgStart, string ProgEnd)
        {
            List<CEAPCNTLEntity> Emsbdcdata = new List<CEAPCNTLEntity>();
            try
            {
                DataSet emsbdcds = Captain.DatabaseLayer.FuelControlDB.GetCEAPCNTLData(Code, Year, ProgStart, ProgEnd);
                if (emsbdcds != null && emsbdcds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsbdcds.Tables[0].Rows)
                    {
                        Emsbdcdata.Add(new CEAPCNTLEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsbdcdata;
            }

            return Emsbdcdata;
        }

        public bool UpdateCEAPCNTL(CEAPCNTLEntity Entity, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_CEAPCNTL_SqlParameters_List(Entity, Entity.Rec_Type);

                SqlParameter sqlsMsg = new SqlParameter("@Msg", SqlDbType.VarChar, 10);
                sqlsMsg.Value = string.Empty;
                sqlsMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlsMsg);

                boolsuccess = Captain.DatabaseLayer.FuelControlDB.InsertUpdateDelCEAPCNTL(sqlParamList);
                Sql_Reslut_Msg = sqlsMsg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool InsupDelCEAPINV(CEAPINVEntity Entity) 
        {
            bool boolsuccess;
            //Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList = Prepare_CEAPINV_SqlParameters_List(Entity);
                boolsuccess = Captain.DatabaseLayer.FuelControlDB.InsertUpdateDelCEAPINV(sqlParamList);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }
        public bool CopytoNextYearCEAPINV(string _CurrYear, string _CurrCode, string _NextYear, string _NextCode, string _opeartor, string Mode)
        {
            bool boolsuccess;
            //Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (_CurrYear != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CURR_YEAR", _CurrYear));
                if (_CurrCode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CURR_CODE", _CurrCode));
                if (_NextYear != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CPINV_YEAR", _NextYear));
                if (_NextCode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CPINV_CPCT_CODE", _NextCode));
                if (_opeartor != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CPINV_LSTC_OPEARATOR", _opeartor));
                if (Mode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Mode", Mode));

                boolsuccess = Captain.DatabaseLayer.FuelControlDB.CopytoNextYearCEAPINV(sqlParamList);
            }
            catch (Exception ex)
            { 
                return false; 
            }

            return boolsuccess;
        }
        public List<CEAPINVEntity> GetCEAPINVData(string Code, string Year, string Mode)
        {
            List<CEAPINVEntity> Emsbdcdata = new List<CEAPINVEntity>();
            try
            {
                DataSet emsbdcds = Captain.DatabaseLayer.FuelControlDB.GetCEAPINVData(Code, Year, Mode);
                if (emsbdcds != null && emsbdcds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsbdcds.Tables[0].Rows)
                    {
                        Emsbdcdata.Add(new CEAPINVEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsbdcdata;
            }

            return Emsbdcdata;
        }
        public List<SqlParameter> Prepare_CEAPINV_SqlParameters_List(CEAPINVEntity Entity)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@CPINV_CPCT_CODE", Entity.CPINV_CPCT_CODE));
                sqlParamList.Add(new SqlParameter("@CPINV_YEAR", Entity.CPINV_YEAR));
                if (!string.IsNullOrEmpty(Entity.CPINV_PRIORTY))
                    sqlParamList.Add(new SqlParameter("@CPINV_PRIORTY", Entity.CPINV_PRIORTY));
                if (!string.IsNullOrEmpty(Entity.CPINV_LOW))
                    sqlParamList.Add(new SqlParameter("@CPINV_LOW", Entity.CPINV_LOW));
                if (!string.IsNullOrEmpty(Entity.CPINV_HIGH))
                    sqlParamList.Add(new SqlParameter("@CPINV_HIGH", Entity.CPINV_HIGH));
                if (!string.IsNullOrEmpty(Entity.CPINV_MAX_INV))
                    sqlParamList.Add(new SqlParameter("@CPINV_MAX_INV", Entity.CPINV_MAX_INV));
                if (!string.IsNullOrEmpty(Entity.CPINV_LSTC_OPEARATOR))
                    sqlParamList.Add(new SqlParameter("@CPINV_LSTC_OPEARATOR", Entity.CPINV_LSTC_OPEARATOR));
                if (!string.IsNullOrEmpty(Entity.Mode))
                    sqlParamList.Add(new SqlParameter("@Mode", Entity.Mode));
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<SqlParameter> Prepare_CEAPCNTL_SqlParameters_List(CEAPCNTLEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@Mode", Entity.Rec_Type));
                }

                sqlParamList.Add(new SqlParameter("@CPCT_CODE", Entity.CPCT_CODE));
                sqlParamList.Add(new SqlParameter("@CPCT_YEAR", Entity.CPCT_YEAR));

                if (!string.IsNullOrEmpty(Entity.CPCT_PROG_START))
                    sqlParamList.Add(new SqlParameter("@CPCT_PROG_START", Entity.CPCT_PROG_START));
                if (!string.IsNullOrEmpty(Entity.CPCT_PROG_END))
                    sqlParamList.Add(new SqlParameter("@CPCT_PROG_END", Entity.CPCT_PROG_END));
                if (!string.IsNullOrEmpty(Entity.CPCT_VUL_SP))
                    sqlParamList.Add(new SqlParameter("@CPCT_VUL_SP", Entity.CPCT_VUL_SP));
                if (!string.IsNullOrEmpty(Entity.CPCT_VUL_50))
                    sqlParamList.Add(new SqlParameter("@CPCT_VUL_50", Entity.CPCT_VUL_50));
                if (!string.IsNullOrEmpty(Entity.CPCT_VUL_75))
                    sqlParamList.Add(new SqlParameter("@CPCT_VUL_75", Entity.CPCT_VUL_75));
                if (!string.IsNullOrEmpty(Entity.CPCT_VUL_150))
                    sqlParamList.Add(new SqlParameter("@CPCT_VUL_150", Entity.CPCT_VUL_150));
                if (!string.IsNullOrEmpty(Entity.CPCT_VUL_MAX_INV))
                    sqlParamList.Add(new SqlParameter("@CPCT_VUL_MAX_INV", Entity.CPCT_VUL_MAX_INV));
                if (!string.IsNullOrEmpty(Entity.CPCT_VUL_PRIM_CA))
                    sqlParamList.Add(new SqlParameter("@CPCT_VUL_PRIM_CA", Entity.CPCT_VUL_PRIM_CA));
                if (!string.IsNullOrEmpty(Entity.CPCT_NONVUL_SP))
                    sqlParamList.Add(new SqlParameter("@CPCT_NONVUL_SP", Entity.CPCT_NONVUL_SP));
                if (!string.IsNullOrEmpty(Entity.CPCT_NONVUL_50))
                    sqlParamList.Add(new SqlParameter("@CPCT_NONVUL_50", Entity.CPCT_NONVUL_50));
                if (!string.IsNullOrEmpty(Entity.CPCT_NONVUL_75))
                    sqlParamList.Add(new SqlParameter("@CPCT_NONVUL_75", Entity.CPCT_NONVUL_75));
                if (!string.IsNullOrEmpty(Entity.CPCT_NONVUL_150))
                    sqlParamList.Add(new SqlParameter("@CPCT_NONVUL_150", Entity.CPCT_NONVUL_150));
                if (!string.IsNullOrEmpty(Entity.CPCT_NONVUL_MAX_INV))
                    sqlParamList.Add(new SqlParameter("@CPCT_NONVUL_MAX_INV", Entity.CPCT_NONVUL_MAX_INV));
                if (!string.IsNullOrEmpty(Entity.CPCT_NONVUL_PRIM_CA))
                    sqlParamList.Add(new SqlParameter("@CPCT_NONVUL_PRIM_CA", Entity.CPCT_NONVUL_PRIM_CA));


                if (!string.IsNullOrEmpty(Entity.CPCT_VUL_SSI))
                    sqlParamList.Add(new SqlParameter("@CPCT_VUL_SSI", Entity.CPCT_VUL_SSI));
                if (!string.IsNullOrEmpty(Entity.CPCT_VUL_MTVC))
                    sqlParamList.Add(new SqlParameter("@CPCT_VUL_MTVC", Entity.CPCT_VUL_MTVC));
                if (!string.IsNullOrEmpty(Entity.CPCT_NONVUL_SSI))
                    sqlParamList.Add(new SqlParameter("@CPCT_NONVUL_SSI", Entity.CPCT_NONVUL_SSI));
                if (!string.IsNullOrEmpty(Entity.CPCT_NONVUL_MTVC))
                    sqlParamList.Add(new SqlParameter("@CPCT_NONVUL_MTVC", Entity.CPCT_NONVUL_MTVC));
                if (!string.IsNullOrEmpty(Entity.CPCT_HC_SP))
                    sqlParamList.Add(new SqlParameter("@CPCT_HC_SP", Entity.CPCT_HC_SP));
                if (!string.IsNullOrEmpty(Entity.CPCT_P1_SP))
                    sqlParamList.Add(new SqlParameter("@CPCT_P1_SP", Entity.CPCT_P1_SP));
                if (!string.IsNullOrEmpty(Entity.CPCT_P2_SP))
                    sqlParamList.Add(new SqlParameter("@CPCT_P2_SP", Entity.CPCT_P2_SP));
                if (!string.IsNullOrEmpty(Entity.CPCT_P3_SP))
                    sqlParamList.Add(new SqlParameter("@CPCT_P3_SP", Entity.CPCT_P3_SP));

                if (!string.IsNullOrEmpty(Entity.CPCT_P1_CA1))
                    sqlParamList.Add(new SqlParameter("@CPCT_P1_CA1", Entity.CPCT_P1_CA1));
                if (!string.IsNullOrEmpty(Entity.CPCT_P1_CA2))
                    sqlParamList.Add(new SqlParameter("@CPCT_P1_CA2", Entity.CPCT_P1_CA2));
                if (!string.IsNullOrEmpty(Entity.CPCT_P1_CA3))
                    sqlParamList.Add(new SqlParameter("@CPCT_P1_CA3", Entity.CPCT_P1_CA3));
                if (!string.IsNullOrEmpty(Entity.CPCT_P2_CA1))
                    sqlParamList.Add(new SqlParameter("@CPCT_P2_CA1", Entity.CPCT_P2_CA1));
                if (!string.IsNullOrEmpty(Entity.CPCT_P2_CA2))
                    sqlParamList.Add(new SqlParameter("@CPCT_P2_CA2", Entity.CPCT_P2_CA2));
                if (!string.IsNullOrEmpty(Entity.CPCT_P2_CA3))
                    sqlParamList.Add(new SqlParameter("@CPCT_P2_CA3", Entity.CPCT_P2_CA3));

                if (!string.IsNullOrEmpty(Entity.CPCT_P3_CA1))
                    sqlParamList.Add(new SqlParameter("@CPCT_P3_CA1", Entity.CPCT_P3_CA1));
                if (!string.IsNullOrEmpty(Entity.CPCT_P3_CA2))
                    sqlParamList.Add(new SqlParameter("@CPCT_P3_CA2", Entity.CPCT_P3_CA2));
                if (!string.IsNullOrEmpty(Entity.CPCT_P3_CA3))
                    sqlParamList.Add(new SqlParameter("@CPCT_P3_CA3", Entity.CPCT_P3_CA3));

                if (!string.IsNullOrEmpty(Entity.CPCT_SUBREP_ID))
                    sqlParamList.Add(new SqlParameter("@CPCT_SUBREP_ID", Entity.CPCT_SUBREP_ID));

                //Vikash added on 05/06/2023
                if (!string.IsNullOrEmpty(Entity.CPCT_VUL_BENTYPE))
                    sqlParamList.Add(new SqlParameter("@CPCT_VUL_BENTYPE", Entity.CPCT_VUL_BENTYPE));
                if (!string.IsNullOrEmpty(Entity.CPCT_NONVUL_BENTYPE))
                    sqlParamList.Add(new SqlParameter("@CPCT_NONVUL_BENTYPE", Entity.CPCT_NONVUL_BENTYPE));
                if (!string.IsNullOrEmpty(Entity.CPCT_CRISIS_BENTYPE))
                    sqlParamList.Add(new SqlParameter("@CPCT_CRISIS_BENTYPE", Entity.CPCT_CRISIS_BENTYPE));

                if (!string.IsNullOrEmpty(Entity.CPCT_LSTC_OPERATOR))
                    sqlParamList.Add(new SqlParameter("@CPCT_LSTC_OPERATOR", Entity.CPCT_LSTC_OPERATOR));

                if (!string.IsNullOrEmpty(Entity.CPCT_VUL_SNAP))
                    sqlParamList.Add(new SqlParameter("@CPCT_VUL_SNAP", Entity.CPCT_VUL_SNAP));
                if (!string.IsNullOrEmpty(Entity.CPCT_NONVUL_SNAP))
                    sqlParamList.Add(new SqlParameter("@CPCT_NONVUL_SNAP", Entity.CPCT_NONVUL_SNAP));

                //Kranthi added on 01/30/2024
                if (!string.IsNullOrEmpty(Entity.CPCT_INV_METHOD))
                    sqlParamList.Add(new SqlParameter("@CPCT_INV_METHOD", Entity.CPCT_INV_METHOD));


                //Kranthi added on 10/17/2024
                if (!string.IsNullOrEmpty(Entity.CPCT_LUMP_SUM))
                    sqlParamList.Add(new SqlParameter("@CPCT_LUMP_SUM", Entity.CPCT_LUMP_SUM));

                if (!string.IsNullOrEmpty(Entity.CPCT_HC_CA))
                    sqlParamList.Add(new SqlParameter("@CPCT_HC_CA", Entity.CPCT_HC_CA));

                if (!string.IsNullOrEmpty(Entity.CPCT_VUL_ARR_CA))
                    sqlParamList.Add(new SqlParameter("@CPCT_VUL_ARR_CA", Entity.CPCT_VUL_ARR_CA));

                if (!string.IsNullOrEmpty(Entity.CPCT_NONVUL_ARR_CA))
                    sqlParamList.Add(new SqlParameter("@CPCT_NONVUL_ARR_CA", Entity.CPCT_NONVUL_ARR_CA));

                if (!string.IsNullOrEmpty(Entity.CPCT_LUMP_INTRVAL))
                    sqlParamList.Add(new SqlParameter("@CPCT_LUMP_INTRVAL", Entity.CPCT_LUMP_INTRVAL));

                if (!string.IsNullOrEmpty(Entity.CPCT_USER_CNTL))
                    sqlParamList.Add(new SqlParameter("@CPCT_USER_CNTL", Entity.CPCT_USER_CNTL));

                if (!string.IsNullOrEmpty(Entity.CPCT_CSBG_CONTRACT_NO))
                    sqlParamList.Add(new SqlParameter("@CPCT_CSBG_CONTRACT_NO", Entity.CPCT_CSBG_CONTRACT_NO));

                //********************************************************************************************//

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@CPCT_LSTC_DATE", Entity.CPCT_LSTC_DATE));

                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }



        public bool CAPS_CASEACTADJ_INSUPDEL(CaseActAdjEntity caseactadjentity)
        {
            bool boolstatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (caseactadjentity.ADJ_AGENCY != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_AGENCY", caseactadjentity.ADJ_AGENCY));
                if (caseactadjentity.ADJ_DEPT != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_DEPT", caseactadjentity.ADJ_DEPT));
                if (caseactadjentity.ADJ_PROGRAM != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_PROGRAM", caseactadjentity.ADJ_PROGRAM));
                if (caseactadjentity.ADJ_YEAR != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_YEAR", caseactadjentity.ADJ_YEAR));
                if (caseactadjentity.ADJ_APP_NO != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_APP_NO", caseactadjentity.ADJ_APP_NO));
                if (caseactadjentity.ADJ_SERVICE_PLAN != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_SERVICE_PLAN", caseactadjentity.ADJ_SERVICE_PLAN));
                if (caseactadjentity.ADJ_SPM_SEQ != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_SPM_SEQ", caseactadjentity.ADJ_SPM_SEQ));
                if (caseactadjentity.ADJ_BRANCH != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_BRANCH", caseactadjentity.ADJ_BRANCH));
                if (caseactadjentity.ADJ_GROUP != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_GROUP", caseactadjentity.ADJ_GROUP));
                if (caseactadjentity.ADJ_ACTIVITY_CODE != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_ACTIVITY_CODE", caseactadjentity.ADJ_ACTIVITY_CODE));
                if (caseactadjentity.ADJ_ACT_SEQ != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_ACT_SEQ", caseactadjentity.ADJ_ACT_SEQ));
                if (caseactadjentity.ADJ_SEQ != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_SEQ", caseactadjentity.ADJ_SEQ));
                if (caseactadjentity.ADJ_REASON != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_REASON", caseactadjentity.ADJ_REASON));
                if (caseactadjentity.ADJ_Date != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_Date", caseactadjentity.ADJ_Date));
                if (caseactadjentity.ADJ_O_ACTYR_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_O_ACTYR_DATE", caseactadjentity.ADJ_O_ACTYR_DATE));
                if (caseactadjentity.ADJ_O_ACTY_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_O_ACTY_DATE", caseactadjentity.ADJ_O_ACTY_DATE));
                if (caseactadjentity.ADJ_O_CASEWRKR != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_O_CASEWRKR", caseactadjentity.ADJ_O_CASEWRKR));
                if (caseactadjentity.ADJ_O_SITE != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_O_SITE", caseactadjentity.ADJ_O_SITE));
                if (caseactadjentity.ADJ_O_VENDOR_NO != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_O_VENDOR_NO", caseactadjentity.ADJ_O_VENDOR_NO));
                if (caseactadjentity.ADJ_O_VEND_ACCT != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_O_VEND_ACCT", caseactadjentity.ADJ_O_VEND_ACCT));
                if (caseactadjentity.ADJ_O_BILLNAME_TYPE != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_O_BILLNAME_TYPE", caseactadjentity.ADJ_O_BILLNAME_TYPE));
                if (caseactadjentity.ADJ_O_BILL_LNAME != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_O_BILL_LNAME", caseactadjentity.ADJ_O_BILL_LNAME));
                if (caseactadjentity.ADJ_O_BILL_FNAME != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_O_BILL_FNAME", caseactadjentity.ADJ_O_BILL_FNAME));
                if (caseactadjentity.ADJ_O_FUND1 != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_O_FUND1", caseactadjentity.ADJ_O_FUND1));
                if (caseactadjentity.ADJ_O_BUDGET_ID != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_O_BUDGET_ID", caseactadjentity.ADJ_O_BUDGET_ID));
                if (caseactadjentity.ADJ_O_COST != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_O_COST", caseactadjentity.ADJ_O_COST));
                if (caseactadjentity.ADJ_O_FOLLUP_ON != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_O_FOLLUP_ON", caseactadjentity.ADJ_O_FOLLUP_ON));
                if (caseactadjentity.ADJ_O_FOLLUP_COMP != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_O_FOLLUP_COMP", caseactadjentity.ADJ_O_FOLLUP_COMP));
                if (caseactadjentity.ADJ_O_FUPBY != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_O_FUPBY", caseactadjentity.ADJ_O_FUPBY));
                if (caseactadjentity.ADJ_O_ACTY_PROG != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_O_ACTY_PROG", caseactadjentity.ADJ_O_ACTY_PROG));
                if (caseactadjentity.ADJ_N_ACTYR_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_N_ACTYR_DATE", caseactadjentity.ADJ_N_ACTYR_DATE));
                if (caseactadjentity.ADJ_N_ACTY_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_N_ACTY_DATE", caseactadjentity.ADJ_N_ACTY_DATE));
                if (caseactadjentity.ADJ_N_CASEWRKR != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_N_CASEWRKR", caseactadjentity.ADJ_N_CASEWRKR));
                if (caseactadjentity.ADJ_N_SITE != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_N_SITE", caseactadjentity.ADJ_N_SITE));
                if (caseactadjentity.ADJ_N_VENDOR_NO != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_N_VENDOR_NO", caseactadjentity.ADJ_N_VENDOR_NO));
                if (caseactadjentity.ADJ_N_VEND_ACCT != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_N_VEND_ACCT", caseactadjentity.ADJ_N_VEND_ACCT));
                if (caseactadjentity.ADJ_N_BILLNAME_TYPE != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_N_BILLNAME_TYPE", caseactadjentity.ADJ_N_BILLNAME_TYPE));
                if (caseactadjentity.ADJ_N_BILL_LNAME != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_N_BILL_LNAME", caseactadjentity.ADJ_N_BILL_LNAME));
                if (caseactadjentity.ADJ_N_BILL_FNAME != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_N_BILL_FNAME", caseactadjentity.ADJ_N_BILL_FNAME));
                if (caseactadjentity.ADJ_N_FUND1 != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_N_FUND1", caseactadjentity.ADJ_N_FUND1));
                if (caseactadjentity.ADJ_N_BUDGET_ID != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_N_BUDGET_ID", caseactadjentity.ADJ_N_BUDGET_ID));
                if (caseactadjentity.ADJ_N_COST != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_N_COST", caseactadjentity.ADJ_N_COST));
                if (caseactadjentity.ADJ_N_FOLLUP_ON != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_N_FOLLUP_ON", caseactadjentity.ADJ_N_FOLLUP_ON));
                if (caseactadjentity.ADJ_N_FOLLUP_COMP != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_N_FOLLUP_COMP", caseactadjentity.ADJ_N_FOLLUP_COMP));
                if (caseactadjentity.ADJ_N_FUPBY != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_N_FUPBY", caseactadjentity.ADJ_N_FUPBY));
                if (caseactadjentity.ADJ_N_ACTY_PROG != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_N_ACTY_PROG", caseactadjentity.ADJ_N_ACTY_PROG));


                if (caseactadjentity.SPM_O_CASEWRKR != string.Empty) sqlParamList.Add(new SqlParameter("@SPM_O_CASEWRKR", caseactadjentity.SPM_O_CASEWRKR));

                if (caseactadjentity.SPM_N_CASEWRKR != string.Empty) sqlParamList.Add(new SqlParameter("@SPM_N_CASEWRKR", caseactadjentity.SPM_N_CASEWRKR));

                if (caseactadjentity.SPM_O_AMOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@SPM_O_AMOUNT", caseactadjentity.SPM_O_AMOUNT));

                if (caseactadjentity.SPM_N_AMOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@SPM_N_AMOUNT", caseactadjentity.SPM_N_AMOUNT));

                if (caseactadjentity.ADJ_O_ARREARS != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_O_ARREARS", caseactadjentity.ADJ_O_ARREARS));

                if (caseactadjentity.ADJ_N_ARREARS != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_N_ARREARS", caseactadjentity.ADJ_N_ARREARS));

                if (caseactadjentity.ADJ_O_BILL_PERIOD != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_O_BILL_PERIOD", caseactadjentity.ADJ_O_BILL_PERIOD));

                if (caseactadjentity.ADJ_N_BILL_PERIOD != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_N_BILL_PERIOD", caseactadjentity.ADJ_N_BILL_PERIOD));

                if (caseactadjentity.ADJ_O_SOURCE != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_O_SOURCE", caseactadjentity.ADJ_O_SOURCE));

                if (caseactadjentity.ADJ_N_SOURCE != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_N_SOURCE", caseactadjentity.ADJ_N_SOURCE));

                if (caseactadjentity.ADJ_O_BENEFIT_REASN != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_O_BENEFIT_REASN", caseactadjentity.ADJ_O_BENEFIT_REASN));

                if (caseactadjentity.ADJ_N_BENEFIT_REASN != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_N_BENEFIT_REASN", caseactadjentity.ADJ_N_BENEFIT_REASN));


                if (caseactadjentity.ADJ_ADD_OPERATOR != string.Empty) sqlParamList.Add(new SqlParameter("@ADJ_ADD_OPERATOR", caseactadjentity.ADJ_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@Mode", caseactadjentity.Mode));
                boolstatus = SPAdminDB.CAPS_CASEACTADJ_INSUPDEL(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return boolstatus;
        }


        public bool CAPS_SPMACT_UPDATE(CASEACTEntity Entity, string strCEAPSWitch, string Catprogram)
        {
            bool boolstatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@CASEACT_AGENCY", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@CASEACT_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@CASEACT_PROGRAM", Entity.Program));
                sqlParamList.Add(new SqlParameter("@CASEACT_YEAR", Entity.Year));
                sqlParamList.Add(new SqlParameter("@CASEACT_APP_NO", Entity.App_no));

                if (!string.IsNullOrEmpty(Entity.Service_plan))
                    sqlParamList.Add(new SqlParameter("@CASEACT_SERVICEPLAN", Entity.Service_plan));   //int
                sqlParamList.Add(new SqlParameter("@CASEACT_SPM_SEQ", Entity.SPM_Seq));   //int
                sqlParamList.Add(new SqlParameter("@CASEACT_BRANCH", Entity.Branch));
                if (!string.IsNullOrEmpty(Entity.Group))
                    sqlParamList.Add(new SqlParameter("@CASEACT_GROUP", Entity.Group));

                sqlParamList.Add(new SqlParameter("@CASEACT_ACTIVITY_CODE", Entity.ACT_Code));

                if (!string.IsNullOrEmpty(Entity.ACT_Seq))
                    sqlParamList.Add(new SqlParameter("@CASEACT_SEQ", Entity.ACT_Seq));

                sqlParamList.Add(new SqlParameter("@CASEACT_ACTY_DATE", Entity.ACT_Date));

                if (!string.IsNullOrEmpty(Entity.ActSeek_Date))
                    sqlParamList.Add(new SqlParameter("@CASEACT_SEEK_DATE", Entity.ActSeek_Date));


                if (!string.IsNullOrEmpty(Entity.Site))
                    sqlParamList.Add(new SqlParameter("@CASEACT_SITE", Entity.Site));
                if (!string.IsNullOrEmpty(Entity.Fund1))
                    sqlParamList.Add(new SqlParameter("@CASEACT_FUND1", Entity.Fund1));


                if (!string.IsNullOrEmpty(Entity.Caseworker))
                    sqlParamList.Add(new SqlParameter("@CASEACT_CASEWRKR", Entity.Caseworker));
                if (!string.IsNullOrEmpty(Entity.Vendor_No))
                    sqlParamList.Add(new SqlParameter("@CASEACT_VENDOR_NO", Entity.Vendor_No));

                if (!string.IsNullOrEmpty(Entity.Account))
                    sqlParamList.Add(new SqlParameter("@CASEACT_VEND_ACCT", Entity.Account));


                if (!string.IsNullOrEmpty(Entity.Followup_On))
                    sqlParamList.Add(new SqlParameter("@CASEACT_FOLLUP_ON", Entity.Followup_On));
                if (!string.IsNullOrEmpty(Entity.Followup_Comp))
                    sqlParamList.Add(new SqlParameter("@CASEACT_FOLLUP_COMP", Entity.Followup_Comp));

                if (!string.IsNullOrEmpty(Entity.Followup_By))
                    sqlParamList.Add(new SqlParameter("@CASEACT_FUPBY", Entity.Followup_By));


                sqlParamList.Add(new SqlParameter("@CASEACT_ACTY_PROG", Entity.Act_PROG));

                if (!string.IsNullOrEmpty(Entity.BillngType))
                    sqlParamList.Add(new SqlParameter("@CASEACT_BILL_TYPE", Entity.BillngType));


                if (!string.IsNullOrEmpty(Entity.BillngFname))
                    sqlParamList.Add(new SqlParameter("@CASEACT_BILL_FNAME", Entity.BillngFname));

                if (!string.IsNullOrEmpty(Entity.BillngLname))
                    sqlParamList.Add(new SqlParameter("@CASEACT_BILL_LNAME", Entity.BillngLname));


                if (!string.IsNullOrEmpty(Entity.BDC_ID))
                    sqlParamList.Add(new SqlParameter("@CASEACT_BDC_ID", Entity.BDC_ID));

                if (!string.IsNullOrEmpty(Entity.Cost))
                    sqlParamList.Add(new SqlParameter("@CASEACT_COST", Decimal.Parse(Entity.Cost)));

                if (!string.IsNullOrEmpty(Entity.Amount))
                    sqlParamList.Add(new SqlParameter("@SPM_AMOUNT", Entity.Amount));

                if (!string.IsNullOrEmpty(Entity.VendorName))
                    sqlParamList.Add(new SqlParameter("@SPM_WORKER", Entity.VendorName));

                sqlParamList.Add(new SqlParameter("@CASEACT_LSTC_OPERATOR", Entity.Lsct_Operator));

                if (!string.IsNullOrEmpty(Entity.ArrearsAmt))
                    sqlParamList.Add(new SqlParameter("@CASEACT_ARREARS", Entity.ArrearsAmt));

                if (!string.IsNullOrEmpty(Entity.BillingPeriod))
                    sqlParamList.Add(new SqlParameter("@CASEACT_BILL_PERIOD", Entity.BillingPeriod));

                if (!string.IsNullOrEmpty(Entity.CA_Source))
                    sqlParamList.Add(new SqlParameter("@CASEACT_SOURCE", Entity.CA_Source));

                if (!string.IsNullOrEmpty(Catprogram))
                    sqlParamList.Add(new SqlParameter("@CATPROGRAM", Catprogram));

                if (!string.IsNullOrEmpty(Entity.BenefitReason))
                    sqlParamList.Add(new SqlParameter("@CASEACT_BENEFIT_REASN", Entity.BenefitReason));

                sqlParamList.Add(new SqlParameter("@Mode", strCEAPSWitch));


                boolstatus = SPAdminDB.CAPS_SPMACT_UPDATE(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return boolstatus;
        }


        public List<CaseActAdjEntity> CAPS_CASB0021_GET(string agency, string dep, string program, string year, string Site, string AdjCd, string Caseworker, string Fund, string UserId, string Service, string Vendor, string FromDate, string Todate, string strType)
        {
            List<CaseActAdjEntity> Adjmentdata = new List<CaseActAdjEntity>();
            try
            {
                DataSet emsresds = Captain.DatabaseLayer.SPAdminDB.CAPS_CASB0021_GET(agency, dep, program, year, Site, AdjCd, Caseworker, Fund, UserId, Service, Vendor, FromDate, Todate);
                if (emsresds != null && emsresds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsresds.Tables[0].Rows)
                    {
                        Adjmentdata.Add(new CaseActAdjEntity(row, strType));
                    }
                }
            }
            catch (Exception ex)
            {
                return Adjmentdata;
            }

            return Adjmentdata;
        }

        public List<TargetEntity> CAPS_ADMN0030_BROWSE(string rngCode, string SPCode, string Agency)
        {
            List<TargetEntity> Emsbdcdata = new List<TargetEntity>();
            try
            {
                DataSet emsbdcds = Captain.DatabaseLayer.SPAdminDB.CAPS_ADMN0030_BROWSE(rngCode, SPCode, Agency);
                if (emsbdcds != null && emsbdcds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsbdcds.Tables[0].Rows)
                    {
                        Emsbdcdata.Add(new TargetEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsbdcdata;
            }

            return Emsbdcdata;
        }

        public List<CASESP1Entity> Browse_CASESP1AndSp0(string SP_code, string Agy, string Dept, string Prog)
        {
            List<CASESP1Entity> CASESP1Profile = new List<CASESP1Entity>();
            try
            {
                DataSet CASESP1Data = SPAdminDB.Browse_CASESP1(SP_code, Agy, Dept, Prog);
                if (CASESP1Data != null && CASESP1Data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESP1Data.Tables[0].Rows)
                        CASESP1Profile.Add(new CASESP1Entity(row, string.Empty));
                }
            }
            catch (Exception ex)
            { return CASESP1Profile; }

            return CASESP1Profile;
        }


        public bool InsertUpdateSPTARGET(SPTargetEntity CsbEntity)
        {
            bool boolsuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@SPT_DT_CODE", CsbEntity.SPT_DT_CODE));
                sqlParamList.Add(new SqlParameter("@SPT_AGENCY", CsbEntity.SPT_AGENCY));
                sqlParamList.Add(new SqlParameter("@SPT_SP", CsbEntity.SPT_SP));
                sqlParamList.Add(new SqlParameter("@SPT_TYPE", CsbEntity.SPT_TYPE));
                sqlParamList.Add(new SqlParameter("@SPT_CAMS_CODE", CsbEntity.SPT_CAMS_CODE));
                sqlParamList.Add(new SqlParameter("@SPT_DESC", CsbEntity.SPT_DESC));
                sqlParamList.Add(new SqlParameter("@SPT_INTERVENTION", CsbEntity.SPT_INTERVENTION));
                sqlParamList.Add(new SqlParameter("@SPT_ADD_OPERATOR", CsbEntity.SPT_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@SPT_LSTC_OPERATOR", CsbEntity.SPT_LSTC_OPERATOR));
                //sqlParamList.Add(new SqlParameter("@RNGSRGA_LSTC_OPERATOR", CsbEntity.LSTCOperator));
                sqlParamList.Add(new SqlParameter("@MODE", CsbEntity.Mode));
                sqlParamList.Add(new SqlParameter("@SP0_BULK", CsbEntity.SP0_Bulk));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.CAPS_INSUPDEL_SPTARGET(sqlParamList);
            }
            catch (Exception ex)
            {
                return false;
            }
            return boolsuccess;
        }

        public List<SPTargetEntity> Browse_SPTarget(string MainCode, string SP_code, string Agy)
        {
            List<SPTargetEntity> CASESP1Profile = new List<SPTargetEntity>();
            try
            {
                DataSet CASESP1Data = SPAdminDB.Browse_SPTARGET(MainCode, SP_code, Agy);
                if (CASESP1Data != null && CASESP1Data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESP1Data.Tables[0].Rows)
                        CASESP1Profile.Add(new SPTargetEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESP1Profile; }

            return CASESP1Profile;
        }

        //Vikash added on 11/14/2024 for adding CEAP History to CEAP Control
        public bool InsertCeapHist(CEAPHISTEntity HistEntity)
        {
            bool boolSuccess = false;
            string strNewApplNo = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@CEAP_HIST_TBLNAME", HistEntity.CeapHistTblName));

                sqlParamList.Add(new SqlParameter("@CEAP_HIST_TBLKEY", HistEntity.CeapHistTblKey));

                sqlParamList.Add(new SqlParameter("@CEAP_HIST_SCREEN", HistEntity.CeapHistScreen));

                if (HistEntity.CeapHistSubScr != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CEAP_HIST_SUBSCR", HistEntity.CeapHistSubScr));

                sqlParamList.Add(new SqlParameter("@CEAP_HIST_CHANGES", HistEntity.CeapHistChanges));

                sqlParamList.Add(new SqlParameter("@CEAP_HIST_LSTC_OPERATOR", HistEntity.CeapHistLstcOperator));

                boolSuccess = CEAPDB.InsertCeapHist(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }
            return boolSuccess;
        }

        public List<CEAPHISTEntity> GetCEAPHist(string tableName, string tableKey, string screen)
        {
            List<CEAPHISTEntity> ceapHist = new List<CEAPHISTEntity>();
            try
            {
                DataSet CASESP1Data = CEAPDB.GetCeapHist(tableName, tableKey, screen);
                if (CASESP1Data != null && CASESP1Data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESP1Data.Tables[0].Rows)
                        ceapHist.Add(new CEAPHISTEntity(row));
                }
            }
            catch (Exception ex)
            {
                return ceapHist;
            }

            return ceapHist;
        }

    }
}
