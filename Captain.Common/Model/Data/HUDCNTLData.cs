using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Captain.Common.Model.Objects;
using System.Data;
using Captain.DatabaseLayer;

namespace Captain.Common.Model.Data
{
    public class HUDCNTLData
    {
        public HUDCNTLData(CaptainModel model)
        {
            Model = model;
        }

        #region Properties

        public CaptainModel Model { get; set; }

        public string UserId { get; set; }

        #endregion

        public bool InsertUpdateHUDCNTL(HUDCNTLEntity Entity, string _Mode)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (Entity.Agy != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_AGENCY", Entity.Agy));

                if (Entity.Tax_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_TAX_ID", Entity.Tax_ID));

                if (Entity.Dun_No != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_DUN_NO", Entity.Dun_No));

                if (Entity.Street != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_STREET", Entity.Street));

                if (Entity.City != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_CITY", Entity.City));

                if (Entity.State != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_STATE", Entity.State));

                if (Entity.ZIP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_ZIP", Entity.ZIP));

                if (Entity.isMailAdd_AgyAdd != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_MAIL_ADDR", Entity.isMailAdd_AgyAdd));

                if (Entity.Mail_Street != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_MAIL_STREET", Entity.Mail_Street));

                if (Entity.Mail_City != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_MAIL_CITY", Entity.Mail_City));

                if (Entity.Mail_State != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_MAIL_STATE", Entity.Mail_State));

                if (Entity.Mail_ZIP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_MAIL_ZIP", Entity.Mail_ZIP));

                if (Entity.Phone != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_PHONE", Entity.Phone));

                if (Entity.Email != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_EMAIL", Entity.Email));

                if (Entity.Trans_Agy != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_TRANS_AGY", Entity.Trans_Agy));

                if (Entity.Trans_User != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_TRANS_USER", Entity.Trans_User));

                if (Entity.Trans_Pswrd != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_TRANS_PWD", Entity.Trans_Pswrd));

                if (Entity.Trans_URL != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_TRANS_URL", Entity.Trans_URL));

                if (Entity.Faith_Org != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_FAITH_ORG", Entity.Faith_Org));

                if (Entity.Rural_HH != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_RURAL_HH", Entity.Rural_HH));

                if (Entity.Urban_HH != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_URBAN_HH", Entity.Urban_HH));

                if (Entity.Coun_Methods != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_COUN_METHODS", Entity.Coun_Methods));

                if (Entity.Ser_Col != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_SER_COLONIAS", Entity.Ser_Col));

                if (Entity.Ser_Farms != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_SER_FARMS", Entity.Ser_Farms));

                if (Entity.Budget != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_BUDGET", Entity.Budget));

                if (Entity.Lang != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_LANGUAGE", Entity.Lang));

                if (Entity.Add_Operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_ADD_OPERATOR", Entity.Add_Operator));

                if (Entity.Lstc_Operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_LSTC_OPERATOR", Entity.Lstc_Operator));

                if (Entity.RPB_YEAR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_RPB_FISCAL_YEAR", Entity.RPB_YEAR));

                if (Entity.RPB_QUARTER != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_RPB_QUARTER", Entity.RPB_QUARTER));

                if (Entity.RPB_FRM_DTE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_RPB_FROM_DATE", Entity.RPB_FRM_DTE));

                if (Entity.RPB_TO_DTE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_RPB_TO_DATE", Entity.RPB_TO_DTE));

                if (Entity.RPB_SUBM_DTE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_RPB_SUBM_DATE", Entity.RPB_SUBM_DTE));

                if (Entity.RPB_UPDATE_DTE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_RPB_UPDATE_DATE", Entity.RPB_UPDATE_DTE));

                if (Entity.RPB_TOT_BUDGET != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_RPB_TOT_BUDGET", Entity.RPB_TOT_BUDGET));

                if (Entity.RPB_TOT_FUND_BUDGET != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDC_RPB_TOT_FUND_BUDGET", Entity.RPB_TOT_FUND_BUDGET));


                if (_Mode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Mode", _Mode));

                boolsuccess = Captain.DatabaseLayer.HUDCNTLDB.CAPS_HUDCNTL_INSUPDEL(sqlParamList);

            }
            catch (Exception ex)
            {
                return false;
            }

            return boolsuccess;
        }

        public List<HUDCNTLEntity> GetHUDCNTL(string agency)
        {
            List<HUDCNTLEntity> hudCntlEntity = new List<HUDCNTLEntity>();
            try
            {
                DataSet dsHUDCntl = Captain.DatabaseLayer.HUDCNTLDB.Get_HUDCNTL(agency);
                if (dsHUDCntl != null && dsHUDCntl.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsHUDCntl.Tables[0].Rows)
                    {
                        hudCntlEntity.Add(new HUDCNTLEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return hudCntlEntity;
            }

            return hudCntlEntity;
        }

        public bool InsertUpdateHUDSTAFF(HUDSTAFFEntity Entity, string _staffMode)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (Entity.Agy != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_AGENCY", Entity.Agy));

                if (Entity.Staff_code != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_STF_CODE", Entity.Staff_code));

                if (Entity.Type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_TYPE", Entity.Type));

                if (Entity.Seq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_SEQ", Entity.Seq));

                if (Entity.Cont_type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_CONT_TYPE", Entity.Cont_type));

                if (Entity.Cont_title != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_CONT_TITLE", Entity.Cont_title));

                if (Entity.SSN != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_SSN", Entity.SSN));

                if (Entity.Street != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_WSTREET", Entity.Street));

                if (Entity.City != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_WCITY", Entity.City));

                if (Entity.State != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_WSTATE", Entity.State));

                if (Entity.ZIP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_WZIP", Entity.ZIP));

                if (Entity.Phone != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_WPHONE", Entity.Phone));

                if (Entity.Email != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_WMAIL", Entity.Email));

                if (Entity.Rate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_RATE", Entity.Rate));

                if (Entity.Bill_method != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_BILL_METHOD", Entity.Bill_method));

                if (Entity.FHA_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_FHA_ID", Entity.FHA_ID));

                if (Entity.Active != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_ACTIVE", Entity.Active));

                if (Entity.Cert != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_CERT", Entity.Cert));

                if (Entity.Lang != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_LANG", Entity.Lang));

                if (Entity.Service != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_SERVICE", Entity.Service));

                if (Entity.Add_Operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_ADD_OPERATOR", Entity.Add_Operator));

                if (Entity.Lstc_Operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDS_LSTC_OPERATOR", Entity.Lstc_Operator));

                if (_staffMode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Mode", _staffMode));

                boolsuccess = Captain.DatabaseLayer.HUDCNTLDB.CAPS_HUDSTAFF_INSUPDEL(sqlParamList);

            }
            catch (Exception ex)
            {
                return false;
            }

            return boolsuccess;
        }

        public List<HUDSTAFFEntity> GetHUDSTAFF(string agency, string staff_code, string Type)
        {
           List<HUDSTAFFEntity> hudstaffEntity = new List<HUDSTAFFEntity>();
            try
            {
                DataSet dsstaff = Captain.DatabaseLayer.HUDCNTLDB.Get_HUDSTAFF(agency, staff_code, Type);
                if (dsstaff != null && dsstaff.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsstaff.Tables[0].Rows)
                    {
                        hudstaffEntity.Add(new HUDSTAFFEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return hudstaffEntity;
            }

            return hudstaffEntity;
        }

        public bool InsertUpdateHUDTRAIN(HUDTRAINEntity Entity, string _Mode)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (Entity.Agency != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDT_AGENCY", Entity.Agency));

                if (Entity.Seq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDT_SEQ", Entity.Seq));

                if (Entity.Train_Title != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDT_TRTITLE", Entity.Train_Title));

                if (Entity.Train_Date != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDT_TDATE", Entity.Train_Date));

                if (Entity.Duration != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDT_DURATION", Entity.Duration));

                if (Entity.Organiz != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDT_ORGANIZATION", Entity.Organiz));

                if (Entity.Organiz_Other != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDT_ORG_OTHER", Entity.Organiz_Other));

                if (Entity.Sponsor != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDT_SPONSOR", Entity.Sponsor));

                if (Entity.Sponsor_Other != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDT_SPONSOR_OTHER", Entity.Sponsor_Other));

                if (Entity.Add_Operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDT_ADD_OPERATOR", Entity.Add_Operator));

                if (Entity.Lstc_Operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDT_LSTC_OPERATOR", Entity.Lstc_Operator));

                if (_Mode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Mode", _Mode));

                boolsuccess = Captain.DatabaseLayer.HUDCNTLDB.CAPS_HUDTRAIN_INSUPDEL(sqlParamList);

            }
            catch (Exception ex)
            {
                return false;
            }

            return boolsuccess;
        }

        public List<HUDTRAINEntity> GetHUDTRAIN(string agency)
        {
            List<HUDTRAINEntity> hudTrainEntity = new List<HUDTRAINEntity>();
            try
            {
                DataSet dsstaff = Captain.DatabaseLayer.HUDCNTLDB.Get_HUDTRAIN(agency);
                if (dsstaff != null && dsstaff.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsstaff.Tables[0].Rows)
                    {
                        hudTrainEntity.Add(new HUDTRAINEntity(row,"GET"));
                    }
                }
            }
            catch (Exception ex)
            {
                return hudTrainEntity;
            }

            return hudTrainEntity;
        }

        public bool InsertUpdateHTCATTN(HUDHTCATTNEntity Entity, string _Mode)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (Entity.Agency != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HTC_AGENCY", Entity.Agency));

                if (Entity.Seq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HTC_HUD_SEQ", Entity.Seq));

                if (Entity.Staff_Code != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HTC_STAFF_CODE", Entity.Staff_Code));

                if (Entity.Certificate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HTC_CERTIFICATE", Entity.Certificate));

                if (Entity.Add_Operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HTC_ADD_OPERATOR", Entity.Add_Operator));

                if (Entity.Lstc_Operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HTC_LSTC_OPERATOR", Entity.Lstc_Operator));
                
                if (_Mode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MODE", _Mode));

                boolsuccess = Captain.DatabaseLayer.HUDCNTLDB.CAPS_HTCATTN_INSUPDEL(sqlParamList);

            }
            catch (Exception ex)
            {
                return false;
            }

            return boolsuccess;
        }

        public List<HUDHTCATTNEntity> GetHTCATTN(string agency, string Seq, string staff_Code)
        {
            List<HUDHTCATTNEntity> hudcounsEntity = new List<HUDHTCATTNEntity>();
            try
            {
                DataSet dsCouns = Captain.DatabaseLayer.HUDCNTLDB.Get_HTCATTN(agency, Seq, staff_Code);
                if (dsCouns != null && dsCouns.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsCouns.Tables[0].Rows)
                    {
                        hudcounsEntity.Add(new HUDHTCATTNEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return hudcounsEntity;
            }
            return hudcounsEntity;
        }

        public bool InsertUpdateHUDGROUP(HUDGROUPEntity Entity, string _Mode)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (Entity.Agency != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDG_AGENCY", Entity.Agency));

                if (Entity.Seq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDG_SEQ", Entity.Seq));


                if (Entity.Sess_Title != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDG_SES_TITLE", Entity.Sess_Title));

                if (Entity.Ses_Date != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDG_SES_DATE", Entity.Ses_Date));

                if (Entity.Type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDG_TYPE", Entity.Type));

                if (Entity.Duration != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDG_DURATION", Entity.Duration));

                if (Entity.Attribute != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDG_ATTRIBUTE", Entity.Attribute));

                if (Entity.Fund != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDG_FUND", Entity.Fund));

                if (Entity.Staff_Code != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDG_STAFF_CODE", Entity.Staff_Code));


                if (Entity.Add_Operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDG_ADD_OPERATOR", Entity.Add_Operator));

                if (Entity.Lstc_Operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDG_LSTC_OPERATOR", Entity.Lstc_Operator));

                if (_Mode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MODE", _Mode));

                boolsuccess = Captain.DatabaseLayer.HUDCNTLDB.CAPS_HUDGROUP_INSUPDEL(sqlParamList);

            }
            catch (Exception ex)
            {
                return false;
            }

            return boolsuccess;
        }

        public List<HUDGROUPEntity> GetHUDGROUP(string agency, string Seq)
        {
            List<HUDGROUPEntity> hudGroupEntity = new List<HUDGROUPEntity>();
            try
            {
                DataSet dsCouns = Captain.DatabaseLayer.HUDCNTLDB.Get_HUDGROUP(agency, Seq);
                if (dsCouns != null && dsCouns.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsCouns.Tables[0].Rows)
                    {
                        hudGroupEntity.Add(new HUDGROUPEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return hudGroupEntity;
            }
            return hudGroupEntity;
        }


        public bool InsertUpdateHUDForm(HUDMSTENTITY Entity, string _Mode, out string Mst_Seq)
        {
            bool boolsuccess;
            string strNewMstSeq = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (Entity.Agency != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDMST_AGY", Entity.Agency));

                if (Entity.Dept != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDMST_DEPT", Entity.Dept));

                if (Entity.Prog != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDMST_PROG", Entity.Prog));

                if (Entity.Year != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDMST_YEAR", Entity.Year));

                if (Entity.AppNo != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDMST_APPNO", Entity.AppNo));

                if (Entity.Seq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDMST_SEQ", Entity.Seq));

                if (Entity.Date != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDMST_DATE", Entity.Date));

                if (Entity.Status != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDMST_STATUS", Entity.Status));

                if (Entity.Add_Operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDMST_ADD_OPERATOR", Entity.Add_Operator));

                if (Entity.Lstc_Operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDMST_LSTC_OPERATOR", Entity.Lstc_Operator));

                if (_Mode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MODE", _Mode));

                SqlParameter sqlMstSeqOut = new SqlParameter("@HUDMST_M_SEQ", SqlDbType.Int);
                sqlMstSeqOut.Value = Entity.Seq;
                sqlMstSeqOut.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlMstSeqOut);

                boolsuccess = Captain.DatabaseLayer.HUDCNTLDB.CAPS_HUDMST_INSUPDEL(sqlParamList);
                strNewMstSeq = sqlMstSeqOut.Value.ToString();

            }
            catch (Exception ex)
            {
                Mst_Seq = strNewMstSeq;

                return false;
            }
            Mst_Seq = strNewMstSeq;

            return boolsuccess;
        }

        public List<HUDMSTENTITY> GetHUDForm(string agency, string dept, string prog, string year, string appno, string seq)
        {
            List<HUDMSTENTITY> hudmstEntity = new List<HUDMSTENTITY>();
            try
            {
                DataSet dsCouns = Captain.DatabaseLayer.HUDCNTLDB.Get_HUDMST(agency, dept, prog, year, appno, seq);
                if (dsCouns != null && dsCouns.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsCouns.Tables[0].Rows)
                    {
                        hudmstEntity.Add(new HUDMSTENTITY(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return hudmstEntity;
            }
            return hudmstEntity;
        }

        public bool InsertUpdateHUDIndiv(HUDINDIVENTITY Entity, string _Mode, out string Indv_Seq)
        {
            bool boolsuccess;
            string strNewIndvSeq = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (Entity.Agency != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_AGY", Entity.Agency));

                if (Entity.Dept != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_DEPT", Entity.Dept));

                if (Entity.Prog != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_PROG", Entity.Prog));

                if (Entity.Year != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_YEAR", Entity.Year));

                if (Entity.AppNo != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_APPNO", Entity.AppNo));

                if (Entity.Seq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_SEQ", Entity.Seq));

                if (Entity.MST_Seq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDMST_SEQ", Entity.MST_Seq));

                if (Entity.Sess_Date != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_SESS_DATE", Entity.Sess_Date));

                if (Entity.Sess_Client_Paid != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_SESS_CLIENT_PAID", Entity.Sess_Client_Paid));

                if (Entity.Sess_Counselor != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_SESS_COUNS", Entity.Sess_Counselor));

                if (Entity.Sess_Start_Time != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_SESS_START_TIME", Entity.Sess_Start_Time));

                if (Entity.Sess_End_Time != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_SESS_END_TIME", Entity.Sess_End_Time));

                if (Entity.Sess_Status != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_SESS_STATUS", Entity.Sess_Status));

                if (Entity.Sess_Type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_SESS_TYPE", Entity.Sess_Type));

                if (Entity.Sess_Pur_Visit != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_SESS_PUR_VISIT", Entity.Sess_Pur_Visit));

                if (Entity.Sess_Act_Type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_SESS_ACT_TYPE", Entity.Sess_Act_Type));

                if (Entity.Sess_Attr_Grant != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_SESS_ATTR_GRANT", Entity.Sess_Attr_Grant));

                if (Entity.Sess_Grant_Used != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_SESS_GRANT_USED", Entity.Sess_Grant_Used));

                if (Entity.Sess_Fund != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_SESS_FUND", Entity.Sess_Fund));

                if (Entity.CD_HH_Month_Inc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_CD_HH_MNT_INC", Entity.CD_HH_Month_Inc));

                if (Entity.CD_Inc_Level != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_CD_INC_LEVEL", Entity.CD_Inc_Level));

                if (Entity.CD_App_Race != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_CD_APP_RACE", Entity.CD_App_Race));

                if (Entity.CD_App_Ethnicity != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_CD_APP_ETHNICITY", Entity.CD_App_Ethnicity));

                if (Entity.CD_Lang != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_CD_LANG", Entity.CD_Lang));

                if (Entity.CD_Ref_Source != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_CD_REF_SRC", Entity.CD_Ref_Source));

                if (Entity.CD_Ref_Area_Status != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_CD_REF_AREA_STAT", Entity.CD_Ref_Area_Status));

                if (Entity.CD_Eng_Prof_Status != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_CD_ENG_PROF_STAT", Entity.CD_Eng_Prof_Status));

                if (Entity.CD_If_Applies != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_CD_IF_APPLIES", Entity.CD_If_Applies));

                if (Entity.CFH_Depend != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_CFH_DEPEND", Entity.CFH_Depend));

                if (Entity.CFH_HH_Liab != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_CFH_HH_LIAB", Entity.CFH_HH_Liab));

                if (Entity.CFH_Cred_Score != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_CFH_CRED_SCOR", Entity.CFH_Cred_Score));

                if (Entity.CFH_Source != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_CFH_SRC", Entity.CFH_Source));

                if (Entity.CFH_Cred_Score_Reason != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_CFH_CRED_SCOR_REASON", Entity.CFH_Cred_Score_Reason));

                if (Entity.CFH_Job_Length != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_CFH_JOB_LEN", Entity.CFH_Job_Length));

                if (Entity.CFH_OTR_Serv_Award != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_CFH_OTH_SERV_AWARD", Entity.CFH_OTR_Serv_Award));

                if (Entity.LI_Bef_Loan_Type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_LI_BEF_LOAN_TYPE", Entity.LI_Bef_Loan_Type));

                if (Entity.LI_Bef_Loan_Reported != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_LI_BEF_LOAN_RPT", Entity.LI_Bef_Loan_Reported));

                if (Entity.LI_Bef_Mortgage_Type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_LI_BEF_MORT_TYPE", Entity.LI_Bef_Mortgage_Type));

                if (Entity.LI_Bef_Finance_Type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_LI_BEF_FINC_TYPE", Entity.LI_Bef_Finance_Type));

                if (Entity.LI_Bef_If_Applies != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_LI_BEF_IF_APPLIES", Entity.LI_Bef_If_Applies));

                if (Entity.LI_Bef_Is_Cert_HUD != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_LI_BEF_IS_CERT_HUD", Entity.LI_Bef_Is_Cert_HUD));

                if (Entity.LI_Bef_HECM_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_LI_BEF_CERT_HECM_CERTID", Entity.LI_Bef_HECM_ID));

                if (Entity.LI_Bef_Cert_Issued != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_LI_BEF_CERT_ISSUED", Entity.LI_Bef_Cert_Issued));

                if (Entity.LI_Bef_Cert_Expires != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_LI_BEF_CERT_EXPIRES", Entity.LI_Bef_Cert_Expires));

                if (Entity.LI_Aft_Loan_Spouse != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_LI_AFT_LOAN_SPOUCE", Entity.LI_Aft_Loan_Spouse));

                if (Entity.LI_Aft_Mortgage_Type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_LI_AFT_MORT_TYPE", Entity.LI_Aft_Mortgage_Type));


                if (Entity.LI_Aft_Finance_Type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_LI_AFT_FINC_TYPE", Entity.LI_Aft_Finance_Type));

                if (Entity.LI_Aft_Interest_Rate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_LI_AFT_INRST_RATE", Entity.LI_Aft_Interest_Rate));

                if (Entity.LI_Aft_Close_Cost != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_LI_AFT_CLOS_COST", Entity.LI_Aft_Close_Cost));

                if (Entity.LI_Aft_Street != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_LI_AFT_STREET", Entity.LI_Aft_Street));

                if (Entity.LI_Aft_Unit != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_LI_AFT_UNIT", Entity.LI_Aft_Unit));

                if (Entity.LI_Aft_City != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_LI_AFT_CITY", Entity.LI_Aft_City));

                if (Entity.LI_Aft_State != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_LI_AFT_STATE", Entity.LI_Aft_State));

                if (Entity.LI_Aft_ZIP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_LI_AFT_ZIP", Entity.LI_Aft_ZIP));

                if (Entity.Add_Operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_ADD_OPERATOR", Entity.Add_Operator));

                if (Entity.Lstc_Operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_LSTC_OPERATOR", Entity.Lstc_Operator));

                if (_Mode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MODE", _Mode));

                SqlParameter sqlIndvSeqOut = new SqlParameter("@HUDIND_INDV_SEQ", SqlDbType.Int);
                sqlIndvSeqOut.Value = Entity.Seq;
                sqlIndvSeqOut.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlIndvSeqOut);

                boolsuccess = Captain.DatabaseLayer.HUDCNTLDB.CAPS_HUDINDIV_INSUPDEL(sqlParamList);
                strNewIndvSeq = sqlIndvSeqOut.Value.ToString();

            }
            catch (Exception ex)
            {
                Indv_Seq = strNewIndvSeq;

                return false;
            }
            Indv_Seq = strNewIndvSeq;

            return boolsuccess;
        }

        public List<HUDINDIVENTITY> GetHUDINDIV(string agency, string dept, string prog, string year, string appno, string seq, string mst_seq)
        {
            List<HUDINDIVENTITY> hudcounsEntity = new List<HUDINDIVENTITY>();
            try
            {
                DataSet dsCouns = Captain.DatabaseLayer.HUDCNTLDB.Get_HUDINDIV(agency, dept, prog, year, appno, seq, mst_seq);
                if (dsCouns != null && dsCouns.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsCouns.Tables[0].Rows)
                    {
                        hudcounsEntity.Add(new HUDINDIVENTITY(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return hudcounsEntity;
            }
            return hudcounsEntity;
        }

        public bool InsertUpdateHUDImpact(HUDIMPACTENTITY Entity, string _Mode)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (Entity.Agency != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIMPACT_AGY", Entity.Agency));

                if (Entity.Dept != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIMPACT_DEPT", Entity.Dept));

                if (Entity.Prog != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIMPACT_PROG", Entity.Prog));

                if (Entity.Year != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIMPACT_YEAR", Entity.Year));

                if (Entity.AppNo != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIMPACT_APPNO", Entity.AppNo));

                if (Entity.Seq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIMPACT_SEQ", Entity.Seq));

                if (Entity.Indv_Seq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIND_SEQ", Entity.Indv_Seq));

                if (Entity.Mst_Seq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDMST_SEQ", Entity.Mst_Seq));

                if (Entity.Impact_Date != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIMPACT_DATE", Entity.Impact_Date));

                if (Entity.Impacts != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIMPACT_IMPACTS", Entity.Impacts));

                if (Entity.Add_Operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIMPACT_ADD_OPERATOR", Entity.Add_Operator));

                if (Entity.Lstc_Operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HUDIMPACT_LSTC_OPERATOR", Entity.Lstc_Operator));

                if (_Mode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MODE", _Mode));

                boolsuccess = Captain.DatabaseLayer.HUDCNTLDB.CAPS_HUDIMPACT_INSUPDEL(sqlParamList);

            }
            catch (Exception ex)
            {
                return false;
            }

            return boolsuccess;
        }

        public List<HUDIMPACTENTITY> GetHUDIMPACT(string agency, string dept, string prog, string year, string appno, string seq, string Indv_seq, string Mst_seq)
        {
            List<HUDIMPACTENTITY> hudImpactEntity = new List<HUDIMPACTENTITY>();
            try
            {
                DataSet dsCouns = Captain.DatabaseLayer.HUDCNTLDB.Get_HUDIMPACT(agency, dept, prog, year, appno, seq, Indv_seq, Mst_seq);
                if (dsCouns != null && dsCouns.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsCouns.Tables[0].Rows)
                    {
                        hudImpactEntity.Add(new HUDIMPACTENTITY(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return hudImpactEntity;
            }
            return hudImpactEntity;
        }

        //Added by Vikash on 11/29/2024 for getting counts to HUD 9902 report
        public List<HUDCOUNTSENTITY> GetHUD_Counts(string agency, string dept, string prog, string year, string fromdate, string todate)
        {
            List<HUDCOUNTSENTITY> hudcountsEntity = new List<HUDCOUNTSENTITY>();
            try
            {
                DataSet dsCouns = Captain.DatabaseLayer.HUDCNTLDB.Get_HUD_Counts(agency, dept, prog, year, fromdate, todate);
                if (dsCouns != null && dsCouns.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsCouns.Tables[0].Rows)
                    {
                        hudcountsEntity.Add(new HUDCOUNTSENTITY(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return hudcountsEntity;
            }
            return hudcountsEntity;
        }
    }
}
