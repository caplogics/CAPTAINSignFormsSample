using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Model.Objects;
using System.Data;
using System.Data.SqlClient;
using Captain.Common.Utilities;

namespace Captain.Common.Model.Data
{
  public class TmsAllData
    {
        public TmsAllData(CaptainModel model)
        {
            Model = model;
        }

        #region Properties

        public CaptainModel Model { get; set; }

        public string UserId { get; set; }

        #endregion



        public List<NotesEntity> GetNotesDetails(string agency, string dep, string program, string year, string app, string byprog,string Type)
        {
            List<NotesEntity> NotesDetails = new List<NotesEntity>();
            try
            {
                DataSet NotesDs = Captain.DatabaseLayer.TMSAllDB.GETNOTICE(agency, dep, program, year, app, byprog,Type);
                if (NotesDs != null && NotesDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in NotesDs.Tables[0].Rows)
                    {
                        NotesDetails.Add(new NotesEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return NotesDetails;
            }

            return NotesDetails;
        }

        public string GetNotesMaxSeq(string agency, string dep, string program, string year, string app, string byprog, string Type)
        {
            string strseq = "1";
            try
            {
                DataSet NotesDs = Captain.DatabaseLayer.TMSAllDB.GETNOTICE(agency, dep, program, year, app, byprog, "SE");
                if (NotesDs != null && NotesDs.Tables[0].Rows.Count > 0)
                {
                    strseq = NotesDs.Tables[0].Rows[0]["VDN_SEQ"].ToString();                   
                }
            }
            catch (Exception ex)
            {
                //
                return strseq;
            }

            return strseq;
        }


        public List<TMSB4015Entity> GetReportDetails(string agency, string dep, string program, string year, string Fromdate, string Todate)
        {
            List<TMSB4015Entity> NotesDetails = new List<TMSB4015Entity>();
            try
            {
                DataSet NotesDs = Captain.DatabaseLayer.TMSAllDB.GETTMSB4015(agency, dep, program, year, Fromdate, Todate);
                if (NotesDs != null && NotesDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in NotesDs.Tables[0].Rows)
                    {
                        NotesDetails.Add(new TMSB4015Entity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return NotesDetails;
            }

            return NotesDetails;
        }

        public List<TMS81ReportEntity> GetReportDetails(string agency, string dep, string program, string year, string appno)
        {
            List<TMS81ReportEntity> RepDetails = new List<TMS81ReportEntity>();
            try
            {
                DataSet NotesDs = Captain.DatabaseLayer.TMSAllDB.GetTMS81PrintReport(agency, dep, program, year, appno);
                if (NotesDs != null && NotesDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in NotesDs.Tables[0].Rows)
                    {
                        RepDetails.Add(new TMS81ReportEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return RepDetails;
            }

            return RepDetails;
        }

        public List<PAYBATCHEntity> Browse_PAYBATCH(PAYBATCHEntity Entity, string Opretaion_Mode) 
        {
            List<PAYBATCHEntity> CASESPMProfile = new List<PAYBATCHEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_PayBatch_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_PAYBATCH]");
                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new PAYBATCHEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public bool UpdatePAYBATCH(PAYBATCHEntity Entity, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_PayBatch_SqlParameters_List(Entity, Entity.Rec_Type);
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdatePAYBATCH", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdatePAYMENT_Import(string Entity, string User,out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@Xml_To_Save", Entity));
                sqlParamList.Add(new SqlParameter("@Operator", User));
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdatePayment_From_Import", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }


        public List<SqlParameter> Prepare_PayBatch_SqlParameters_List(PAYBATCHEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));
                }

                if (!string.IsNullOrEmpty(Entity.PBR_AGENCY))
                    sqlParamList.Add(new SqlParameter("@PBR_AGENCY", Entity.PBR_AGENCY));
                if (!string.IsNullOrEmpty(Entity.PBR_DEPT))
                    sqlParamList.Add(new SqlParameter("@PBR_DEPT", Entity.PBR_DEPT));
                if (!string.IsNullOrEmpty(Entity.PBR_PROGRAM))
                    sqlParamList.Add(new SqlParameter("@PBR_PROGRAM", Entity.PBR_PROGRAM));
                if (!string.IsNullOrEmpty(Entity.PBR_YEAR))
                    sqlParamList.Add(new SqlParameter("@PBR_YEAR", Entity.PBR_YEAR));
                if (!string.IsNullOrEmpty(Entity.PBR_NO_N))
                    sqlParamList.Add(new SqlParameter("@PBR_NO_N", Entity.PBR_NO_N));
                if (!string.IsNullOrEmpty(Entity.PBR_PAYMENT_TYPE))
                    sqlParamList.Add(new SqlParameter("@PBR_PAYMENT_TYPE", Entity.PBR_PAYMENT_TYPE));
                if (!string.IsNullOrEmpty(Entity.PBR_CASH_BULK))
                    sqlParamList.Add(new SqlParameter("@PBR_CASH_BULK", Entity.PBR_CASH_BULK));
                if (!string.IsNullOrEmpty(Entity.PBR_PAY_NUMBER1))
                    sqlParamList.Add(new SqlParameter("@PBR_PAY_NUMBER1", Entity.PBR_PAY_NUMBER1));
                if (!string.IsNullOrEmpty(Entity.PBR_STATUS))
                    sqlParamList.Add(new SqlParameter("@PBR_STATUS", Entity.PBR_STATUS));
                if (!string.IsNullOrEmpty(Entity.PBR_DATE_OPENED))
                    sqlParamList.Add(new SqlParameter("@PBR_DATE_OPENED", Entity.PBR_DATE_OPENED));
                if (!string.IsNullOrEmpty(Entity.PBR_FED_PAYS))
                    sqlParamList.Add(new SqlParameter("@PBR_FED_PAYS", Entity.PBR_FED_PAYS));
                if (!string.IsNullOrEmpty(Entity.PBR_ST_PAYS))
                    sqlParamList.Add(new SqlParameter("@PBR_ST_PAYS", Entity.PBR_ST_PAYS));
                if (!string.IsNullOrEmpty(Entity.PBR_GALLONS))
                    sqlParamList.Add(new SqlParameter("@PBR_GALLONS", Entity.PBR_GALLONS));
                if (!string.IsNullOrEmpty(Entity.PBR_FED_PAY_DOL))
                    sqlParamList.Add(new SqlParameter("@PBR_FED_PAY_DOL", Entity.PBR_FED_PAY_DOL));
                if (!string.IsNullOrEmpty(Entity.PBR_ST_PAY_DOL))
                    sqlParamList.Add(new SqlParameter("@PBR_ST_PAY_DOL", Entity.PBR_ST_PAY_DOL));
                if (!string.IsNullOrEmpty(Entity.PBR_CHAP_PAYS))
                    sqlParamList.Add(new SqlParameter("@PBR_CHAP_PAYS", Entity.PBR_CHAP_PAYS));
                if (!string.IsNullOrEmpty(Entity.PBR_FUND))
                    sqlParamList.Add(new SqlParameter("@PBR_FUND", Entity.PBR_FUND));

                if (!string.IsNullOrEmpty(Entity.PBR_SCROLL))
                    sqlParamList.Add(new SqlParameter("@PBR_SCROLL", Entity.PBR_SCROLL));
                if (!string.IsNullOrEmpty(Entity.PBR_AUTO_P_CODE))
                    sqlParamList.Add(new SqlParameter("@PBR_AUTO_P_CODE", Entity.PBR_AUTO_P_CODE));
                if (!string.IsNullOrEmpty(Entity.PBR_AUTO_P_VENDOR))
                    sqlParamList.Add(new SqlParameter("@PBR_AUTO_P_VENDOR", Entity.PBR_AUTO_P_VENDOR));
                if (!string.IsNullOrEmpty(Entity.PBR_CHECK_NO))
                    sqlParamList.Add(new SqlParameter("@PBR_CHECK_NO", Entity.PBR_CHECK_NO));
                if (!string.IsNullOrEmpty(Entity.PBR_CHECK_DATE))
                    sqlParamList.Add(new SqlParameter("@PBR_CHECK_DATE", Entity.PBR_CHECK_DATE));
                if (!string.IsNullOrEmpty(Entity.PBR_VOUCH_NO))
                    sqlParamList.Add(new SqlParameter("@PBR_VOUCH_NO", Entity.PBR_VOUCH_NO));
                if (!string.IsNullOrEmpty(Entity.PBR_PAY_NUMBER))
                    sqlParamList.Add(new SqlParameter("@PBR_PAY_NUMBER", Entity.PBR_PAY_NUMBER));
                if (!string.IsNullOrEmpty(Entity.PBR_VENDOR))
                    sqlParamList.Add(new SqlParameter("@PBR_VENDOR", Entity.PBR_VENDOR));
                if (!string.IsNullOrEmpty(Entity.PBR_REP_TYPE))
                    sqlParamList.Add(new SqlParameter("@PBR_REP_TYPE", Entity.PBR_REP_TYPE));
                if (!string.IsNullOrEmpty(Entity.PBR_DOL_LIMIT))
                    sqlParamList.Add(new SqlParameter("@PBR_DOL_LIMIT", Entity.PBR_DOL_LIMIT));

                if (!string.IsNullOrEmpty(Entity.PBR_L_DATE))
                    sqlParamList.Add(new SqlParameter("@PBR_L_DATE", Entity.PBR_L_DATE));
                if (!string.IsNullOrEmpty(Entity.PBR_H_DATE))
                    sqlParamList.Add(new SqlParameter("@PBR_H_DATE", Entity.PBR_H_DATE));
                if (!string.IsNullOrEmpty(Entity.PBR_LSTC_OPERATOR))
                    sqlParamList.Add(new SqlParameter("@PBR_LSTC_OPERATOR", Entity.PBR_LSTC_OPERATOR));


                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@PBR_DATE_LSTC", Entity.PBR_DATE_LSTC));
                    
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<TMSB4010Entity> TMSB4010_Bundling(string agency, string dep, string program, string year, string PmrNo)
        {
            List<TMSB4010Entity> RepDetails = new List<TMSB4010Entity>();
            try
            {
                DataSet NotesDs = Captain.DatabaseLayer.TMSAllDB.GetTMSB4010_Bundling(agency, dep, program, year, PmrNo);
                if (NotesDs != null && NotesDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in NotesDs.Tables[0].Rows)
                    {
                        RepDetails.Add(new TMSB4010Entity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return RepDetails;
            }

            return RepDetails;
        }

        public List<TMSBCHCTEntity> LoadChecks(string agency, string dep, string program, string year, string Vendor,string ChkDate)
        {
            List<TMSBCHCTEntity> RepDetails = new List<TMSBCHCTEntity>();
            try
            {
                DataSet NotesDs = Captain.DatabaseLayer.TMSAllDB.GetLoadChecks(agency, dep, program, year, Vendor,ChkDate);
                if (NotesDs != null && NotesDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in NotesDs.Tables[0].Rows)
                    {
                        RepDetails.Add(new TMSBCHCTEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return RepDetails;
            }

            return RepDetails;
        }

        public List<TMSB0004Entity> TMSB4004_Report(string agency, string dep, string program, string year, string Site)
        {
            List<TMSB0004Entity> RepDetails = new List<TMSB0004Entity>();
            try
            {
                DataSet NotesDs = Captain.DatabaseLayer.TMSAllDB.GETTMSB4004(agency, dep, program, year, Site);
                if (NotesDs != null && NotesDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in NotesDs.Tables[0].Rows)
                    {
                        RepDetails.Add(new TMSB0004Entity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return RepDetails;
            }

            return RepDetails;
        }

        public List<LPWBUNDLEEntity> LPWBUNDLE_GET(string agency, string dep, string program, string year, string BundleNo, string strType)
        {
            List<LPWBUNDLEEntity> LPWBundleDetails = new List<LPWBUNDLEEntity>();
            try
            {
                DataSet bundleds = Captain.DatabaseLayer.TMSAllDB.LPWBUNDLE_GET(agency, dep, program, year, BundleNo, strType);
                if (bundleds != null && bundleds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in bundleds.Tables[0].Rows)
                    {
                        LPWBundleDetails.Add(new LPWBUNDLEEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return LPWBundleDetails;
            }

            return LPWBundleDetails;
        }

        public bool CAPS_LPWBUNDLE_INSERTUPDATEDEL(LPWBUNDLEEntity Entity)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();


                if (Entity.LPWB_AGENCY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LPWB_AGENCY", Entity.LPWB_AGENCY));
                if (Entity.LPWB_DEPT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LPWB_DEPT", Entity.LPWB_DEPT));
                if (Entity.LPWB_PROGRAM != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LPWB_PROGRAM", Entity.LPWB_PROGRAM));
                if (Entity.LPWB_YEAR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LPWB_YEAR", Entity.LPWB_YEAR));
                if (Entity.LPWB_BUNDLE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LPWB_BUNDLE", Entity.LPWB_BUNDLE));
                if (Entity.LPWB_BUNDLE_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LPWB_BUNDLE_TYPE", Entity.LPWB_BUNDLE_TYPE));
                if (Entity.LPWB_STATUS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LPWB_STATUS", Entity.LPWB_STATUS));
                if (Entity.LPWB_DATE_OPENED != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LPWB_DATE_OPENED", Entity.LPWB_DATE_OPENED));
                if (Entity.LPWB_COUNT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LPWB_APP_COUNT", Entity.LPWB_COUNT));
                if (Entity.LPWB_TOT_AMOUNT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LPWB_TOT_AMOUNT", Entity.LPWB_TOT_AMOUNT));
                if (Entity.LPWB_FUND != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LPWB_FUND", Entity.LPWB_FUND));
                if (Entity.LPWB_CHK_NO != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LPWB_CHK_NO", Entity.LPWB_CHK_NO));
                if (Entity.LPWB_CHK_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LPWB_CHK_DATE", Entity.LPWB_CHK_DATE));
                if (Entity.LPWB_VOUCH_NO != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LPWB_VOUCH_NO", Entity.LPWB_VOUCH_NO));
                if (Entity.LPWB_PAY_NUMBER != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LPWB_PAY_NUMBER", Entity.LPWB_PAY_NUMBER));
                if (Entity.LPWB_VENDOR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LPWB_VENDOR", Entity.LPWB_VENDOR));
                if (Entity.LPWB_LSTC_OPERATOR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LPWB_LSTC_OPERATOR", Entity.LPWB_LSTC_OPERATOR));


                if (Entity.Mode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Mode", Entity.Mode));

                boolsuccess = Captain.DatabaseLayer.TMSAllDB.CAPS_LPWBUNDLE_INSERTUPDATEDEL(sqlParamList);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<ListItem> CAPS_TMSB0032_GET(string agency, string dep, string program, string year, string Applicant, string strsdate, string strEnddate, string strMode, string strReportType)
        {
            List<ListItem> LpwStatuslist = new List<ListItem>();
            try
            {
                DataSet bundleds = Captain.DatabaseLayer.TMSAllDB.CAPS_TMSB0032_GET(agency, dep, program, year, Applicant, strsdate, strEnddate, strMode, strReportType);
                if (bundleds != null && bundleds.Tables[0].Rows.Count > 0)
                {
                    if (strReportType == "PERFORMANCE")
                    {
                        foreach (DataRow row in bundleds.Tables[0].Rows)
                        {
                            LpwStatuslist.Add(new ListItem(row["Id"].ToString(), row["counttype"].ToString(), row["totcount"].ToString(), row["totamount"].ToString(), row["totalPer"].ToString(), string.Empty));
                        }
                    }
                    else
                    {
                        foreach (DataRow row in bundleds.Tables[0].Rows)
                        {
                            LpwStatuslist.Add(new ListItem(row["Id"].ToString(), row["counttype"].ToString(), row["totcount"].ToString(), string.Empty));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return LpwStatuslist;
            }

            return LpwStatuslist;
        }

    }
}
