using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Captain.Common.Model.Objects;
using System.Data;

namespace Captain.Common.Model.Data
{
    public class LiheAllData
    {
        public LiheAllData(CaptainModel model)
        {
            Model = model;
        }

        #region Properties

        public CaptainModel Model { get; set; }
        public string UserId { get; set; }

        #endregion

        public bool InsertUpdateDelLiheApd(LiheApdEntity ApdEntity)
        {
            bool boolSuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();


                sqlParamList.Add(new SqlParameter("@LD_AGENCY", ApdEntity.LdAgency));
                sqlParamList.Add(new SqlParameter("@LD_DEPT", ApdEntity.LdDept));
                sqlParamList.Add(new SqlParameter("@LD_PROG", ApdEntity.LdProg));
                sqlParamList.Add(new SqlParameter("@LD_YEAR", ApdEntity.LdYear));
                if (ApdEntity.LdLetterId != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LD_LETTER_ID", ApdEntity.LdLetterId));
                }
                if (ApdEntity.LdSeq != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LD_SEQ", ApdEntity.LdSeq));
                }
                if (ApdEntity.LdLetterType != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LD_LETTER_TYPE ", ApdEntity.LdLetterType));
                }
                if (ApdEntity.LdCategory != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LD_CATEGORY ", ApdEntity.LdCategory));
                }
                if (ApdEntity.LdBoilerPlate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LD_BOILER_PLATE ", ApdEntity.LdBoilerPlate));
                }
                if (ApdEntity.LdAllowUpdate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LD_ALLOW_UPDATE ", ApdEntity.LdAllowUpdate));
                }
                if (ApdEntity.LdIncomeplete != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LD_INCOMPLETE ", ApdEntity.LdIncomeplete));
                }


                sqlParamList.Add(new SqlParameter("@LD_ADD_OPERATOR", ApdEntity.AddOperator));
                sqlParamList.Add(new SqlParameter("@LD_LSTC_OPERATOR", ApdEntity.LstcOperator));
                sqlParamList.Add(new SqlParameter("@Mode", ApdEntity.Mode));
                sqlParamList.Add(new SqlParameter("@Type", ApdEntity.Type));
                boolSuccess = Captain.DatabaseLayer.LiheDb.InsertUpdateDelLiheApd(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return boolSuccess;
        }

        public bool InsertUpdateDelLiheApp(LiheAppEntity AppEntity, out string strMsg)
        {
            bool boolSuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();


                sqlParamList.Add(new SqlParameter("@LAP_AGENCY", AppEntity.LapAgency));
                sqlParamList.Add(new SqlParameter("@LAP_DEPT", AppEntity.LapDept));
                sqlParamList.Add(new SqlParameter("@LAP_PROGRAM", AppEntity.LapProg));
                sqlParamList.Add(new SqlParameter("@LAP_YEAR", AppEntity.LapYear));
                sqlParamList.Add(new SqlParameter("@LAP_APP", AppEntity.LapApp));
                sqlParamList.Add(new SqlParameter("@LAP_LETTER_ID", AppEntity.LapLetterId));

                if (AppEntity.LapLetterDate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LAP_LETTER_DATE ", AppEntity.LapLetterDate));
                }
                if (AppEntity.LapWorker != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LAP_WORKER ", AppEntity.LapWorker));
                }
                if (AppEntity.LapStatus != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LAP_STATUS ", AppEntity.LapStatus));
                }


                sqlParamList.Add(new SqlParameter("@LAP_ADD_OPERATOR", AppEntity.AddOperator));
                sqlParamList.Add(new SqlParameter("@LAP_LSTC_OPERATOR", AppEntity.LstcOperator));
                if (AppEntity.CategorySeq != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@Lap_Category_Seq", AppEntity.CategorySeq));
                }
                if (AppEntity.ReciveDate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@Lap_Recive_Date", AppEntity.ReciveDate));
                }
                sqlParamList.Add(new SqlParameter("@Mode", AppEntity.Mode));
                SqlParameter parameterMsg = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);
                boolSuccess = Captain.DatabaseLayer.LiheDb.InsertUpdateDelLiheApp(sqlParamList);
                strMsg = parameterMsg.Value.ToString();
            }

            catch (Exception ex)
            {
                strMsg = string.Empty;
                return false;
            }

            return boolSuccess;

        }

        public bool InsertUpdateDelLiheApp1(LiheApp1Entity App1Entity)
        {
            bool boolSuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@LAP1_AGENCY", App1Entity.Lap1Agency));
                sqlParamList.Add(new SqlParameter("@LAP1_DEPT", App1Entity.Lap1Dept));
                sqlParamList.Add(new SqlParameter("@LAP1_PROGRAM", App1Entity.Lap1Prog));
                sqlParamList.Add(new SqlParameter("@LAP1_YEAR", App1Entity.Lap1Year));
                sqlParamList.Add(new SqlParameter("@LAP1_APP", App1Entity.Lap1App));
                sqlParamList.Add(new SqlParameter("@LAP1_LETTER_ID", App1Entity.Lap1LetterId));

                if (App1Entity.Lap1LetterDate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LAP1_LETTER_DATE ", App1Entity.Lap1LetterDate));
                }
                if (App1Entity.Lap1CategorySeq != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LAP1_CATEGORY_SEQ ", App1Entity.Lap1CategorySeq));
                }
                if (App1Entity.Lap1ReceiveDate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LAP1_RECEIVE_DATE ", App1Entity.Lap1ReceiveDate));
                }
                sqlParamList.Add(new SqlParameter("@Mode", App1Entity.Mode));
                boolSuccess = Captain.DatabaseLayer.LiheDb.InsertUpdateDelLiheApp1(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return boolSuccess;
        }


        public bool InsertUpdateDelLiheApc(LiheApcEntity ApcEntity)
        {
            bool boolSuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();


                sqlParamList.Add(new SqlParameter("@lac_agency", ApcEntity.LacAgency));
                sqlParamList.Add(new SqlParameter("@lac_dept", ApcEntity.LacDept));
                sqlParamList.Add(new SqlParameter("@lac_program", ApcEntity.LacProg));
                sqlParamList.Add(new SqlParameter("@lac_year", ApcEntity.LacYear));
                sqlParamList.Add(new SqlParameter("@lac_app", ApcEntity.LacApp));
                sqlParamList.Add(new SqlParameter("@lac_letter_id", ApcEntity.LacLetterId));

                if (ApcEntity.LacLetterDate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@lac_letter_date", ApcEntity.LacLetterDate));
                }
                if (ApcEntity.LacCategorySeq != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@lac_category_seq", ApcEntity.LacCategorySeq));
                }
                if (ApcEntity.Lacboillerplate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@lac_boiler_plate", ApcEntity.Lacboillerplate));
                }
                if (ApcEntity.Lacprintbplate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@lac_print_bplate", ApcEntity.Lacprintbplate));
                }
                if (ApcEntity.Laccasenotesdata != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@lac_casenotes_data", ApcEntity.Laccasenotesdata));
                }
                if (ApcEntity.Lacprintcnotes != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@lac_print_cnotes", ApcEntity.Lacprintcnotes));
                }



                sqlParamList.Add(new SqlParameter("@lac_add_operator", ApcEntity.Lacaddoperator));
                sqlParamList.Add(new SqlParameter("@lac_lstc_operator", ApcEntity.Laclstcoperator));
                sqlParamList.Add(new SqlParameter("@Mode", ApcEntity.Mode));
                boolSuccess = Captain.DatabaseLayer.LiheDb.InsertUpdateDelLiheApc(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return boolSuccess;
        }

        public LiheApdEntity GetLiheAPdadpyls(string agency, string dep, string program, string year, string LetterId, string seq, string strType)
        {
            LiheApdEntity LiheApdDetails = null;
            try
            {
                DataSet LiheApd = Captain.DatabaseLayer.LiheDb.GetLiheAPdadpyls(agency, dep, program, year, LetterId, seq, strType);
                if (LiheApd != null && LiheApd.Tables[0].Rows.Count > 0)
                {
                    LiheApdDetails = new LiheApdEntity(LiheApd.Tables[0].Rows[0]);
                }
            }
            catch (Exception ex)
            {
                //
                return LiheApdDetails;
            }

            return LiheApdDetails;
        }

        public List<LiheApdEntity> GetLiheAPdadpylslst(string agency, string dep, string program, string year, string LetterId, string seq, string strType)
        {
            List<LiheApdEntity> LiheApdDetails = new List<LiheApdEntity>();
            try
            {
                DataSet LiheApd = Captain.DatabaseLayer.LiheDb.GetLiheAPdadpyls(agency, dep, program, year, LetterId, seq, strType);
                if (LiheApd != null && LiheApd.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in LiheApd.Tables[0].Rows)
                    {
                        LiheApdDetails.Add(new LiheApdEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return LiheApdDetails;
            }

            return LiheApdDetails;
        }

        public LiheAppEntity GetLiheAppadpyald(string agency, string dep, string program, string year, string App, string LetterId, string LetterDate)
        {
            LiheAppEntity LiheAppDetails = null;
            try
            {
                DataSet LiheApp = Captain.DatabaseLayer.LiheDb.GetLiheAppadpyald(agency, dep, program, year, App, LetterDate, LetterDate);
                if (LiheApp != null && LiheApp.Tables[0].Rows.Count > 0)
                {
                    LiheAppDetails = new LiheAppEntity(LiheApp.Tables[0].Rows[0], string.Empty);
                }
            }
            catch (Exception ex)
            {
                //
                return LiheAppDetails;
            }

            return LiheAppDetails;
        }

        public List<LiheAppEntity> GetLiheAppadpyaldlst(string agency, string dep, string program, string year, string App, string LetterId, string LetterDate, string strTable)
        {
            List<LiheAppEntity> LiheAppDetails = new List<LiheAppEntity>();
            try
            {
                DataSet LiheApp = Captain.DatabaseLayer.LiheDb.GetLiheAppadpyald(agency, dep, program, year, App, LetterId, LetterDate);
                if (LiheApp != null && LiheApp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in LiheApp.Tables[0].Rows)
                    {
                        LiheAppDetails.Add(new LiheAppEntity(row, strTable));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return LiheAppDetails;
            }

            return LiheAppDetails;
        }

        public LiheApp1Entity GetLiheApp1adpyalds(string agency, string dep, string program, string year, string App, string LetterId, string LetterDate, string seq, string strTable)
        {
            LiheApp1Entity LiheApp1Details = null;
            try
            {
                DataSet LiheApp1 = Captain.DatabaseLayer.LiheDb.GetLiheApp1adpyalds(agency, dep, program, year, App, LetterId, LetterDate, seq);
                if (LiheApp1 != null && LiheApp1.Tables[0].Rows.Count > 0)
                {
                    LiheApp1Details = new LiheApp1Entity(LiheApp1.Tables[0].Rows[0], strTable);
                }
            }
            catch (Exception ex)
            {
                //
                return LiheApp1Details;
            }

            return LiheApp1Details;
        }

        public List<LiheApp1Entity> GetLiheApp1adpyaldslst(string agency, string dep, string program, string year, string App, string LetterId, string LetterDate, string seq, string strTable)
        {
            List<LiheApp1Entity> LiheApp1Details = new List<LiheApp1Entity>();
            try
            {
                DataSet LiheApp1 = Captain.DatabaseLayer.LiheDb.GetLiheApp1adpyalds(agency, dep, program, year, App, LetterId, LetterDate, seq);
                if (LiheApp1 != null && LiheApp1.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in LiheApp1.Tables[0].Rows)
                    {
                        LiheApp1Details.Add(new LiheApp1Entity(row, strTable));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return LiheApp1Details;
            }

            return LiheApp1Details;
        }

        public List<CommonEntity> GetType()
        {
            List<CommonEntity> typedetails = new List<CommonEntity>();
            typedetails.Add(new CommonEntity("1", "Letter Categories and Boiler plate text"));
            typedetails.Add(new CommonEntity("2", "Boiler plate text only"));
            return typedetails;
        }

        public string CheckLiheApdCategory(LiheApdEntity ApdEntity)
        {
            string strmsg = "";
            try
            {

                List<SqlParameter> sqlParamList = new List<SqlParameter>();


                sqlParamList.Add(new SqlParameter("@LD_AGENCY", ApdEntity.LdAgency));
                sqlParamList.Add(new SqlParameter("@LD_DEPT", ApdEntity.LdDept));
                sqlParamList.Add(new SqlParameter("@LD_PROG", ApdEntity.LdProg));
                sqlParamList.Add(new SqlParameter("@LD_YEAR", ApdEntity.LdYear));
                if (ApdEntity.LdLetterId != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LD_LETTER_ID", ApdEntity.LdLetterId));
                }
                if (ApdEntity.LdCategory != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LD_CATEGORY ", ApdEntity.LdCategory));
                }
                sqlParamList.Add(new SqlParameter("@Mode", ApdEntity.Mode));
                sqlParamList.Add(new SqlParameter("@Type", ApdEntity.Type));
                SqlParameter parameterMsg = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);
                Captain.DatabaseLayer.LiheDb.CheckLiheapdCategory(sqlParamList);
                strmsg = parameterMsg.Value.ToString();
            }
            catch (Exception ex)
            {
                //
                return "";
            }

            return strmsg;
        }


        public LiheApcEntity GetLiheApcadpyald(string agency, string dep, string program, string year, string App, string LetterId, string LetterDate, string strSeq)
        {
            LiheApcEntity LiheApcDetails = null;
            try
            {
                DataSet LiheApp = Captain.DatabaseLayer.LiheDb.GetLiheApcadpyald(agency, dep, program, year, App, LetterId, LetterDate, strSeq);
                if (LiheApp != null && LiheApp.Tables[0].Rows.Count > 0)
                {
                    LiheApcDetails = new LiheApcEntity(LiheApp.Tables[0].Rows[0]);
                }
            }
            catch (Exception ex)
            {
                //
                return LiheApcDetails;
            }

            return LiheApcDetails;
        }



        public List<LiheApcEntity> GetLiheApcadpyaldlst(string agency, string dep, string program, string year, string App, string LetterId, string LetterDate, string strSeqId)
        {
            List<LiheApcEntity> LiheApcDetails = new List<LiheApcEntity>();
            try
            {
                DataSet LiheApp = Captain.DatabaseLayer.LiheDb.GetLiheApcadpyald(agency, dep, program, year, App, LetterId, LetterDate, strSeqId);
                if (LiheApp != null && LiheApp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in LiheApp.Tables[0].Rows)
                    {
                        LiheApcDetails.Add(new LiheApcEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return LiheApcDetails;
            }

            return LiheApcDetails;
        }


        public List<LiheApvEntity> GetLiheAppvadpyas(string agency, string dep, string program, string year, string App, string vendor, string accountNo, string seq)
        {
            List<LiheApvEntity> LiheApvDetails = new List<LiheApvEntity>();
            try
            {
                DataSet LiheApv = Captain.DatabaseLayer.LiheDb.GetLiheAppvadpyas(agency, dep, program, year, App, vendor, accountNo, seq);
                if (LiheApv != null && LiheApv.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in LiheApv.Tables[0].Rows)
                    {
                        LiheApvDetails.Add(new LiheApvEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return LiheApvDetails;
            }

            return LiheApvDetails;
        }

        public List<CATGELIGEntity> GetCATGELIGadpyas(string agency, string dep, string program, string year, string App, string seq)
        {
            List<CATGELIGEntity> LiheApvDetails = new List<CATGELIGEntity>();
            try
            {
                DataSet LiheApv = Captain.DatabaseLayer.LiheDb.GetCATGELIGadpyas(agency, dep, program, year, App,  seq);
                if (LiheApv != null && LiheApv.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in LiheApv.Tables[0].Rows)
                    {
                        LiheApvDetails.Add(new CATGELIGEntity(row,string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return LiheApvDetails;
            }

            return LiheApvDetails;
        }

        public List<TMSELIGEntity> GetTMSELIG(string agency, string dep, string program, string year, string CatElig,string Round)
        {
            List<TMSELIGEntity> LiheApvDetails = new List<TMSELIGEntity>();
            try
            {
                DataSet LiheApv = Captain.DatabaseLayer.LiheDb.GetTMSELIGData(agency, dep, program, year, CatElig,Round);
                if (LiheApv != null && LiheApv.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in LiheApv.Tables[0].Rows)
                    {
                        LiheApvDetails.Add(new TMSELIGEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return LiheApvDetails;
            }

            return LiheApvDetails;
        }

        public bool InsertUpdateCATGELIG(CATGELIGEntity TmsApcnp)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@CATEG_AGENCY", TmsApcnp.Agency));
                sqlParamList.Add(new SqlParameter("@CATEG_DEPT", TmsApcnp.Dept));
                sqlParamList.Add(new SqlParameter("@CATEG_PROG", TmsApcnp.Prog));
                if (TmsApcnp.Year != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CATEG_YEAR", TmsApcnp.Year));
                }
                if (TmsApcnp.App != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CATEG_APP_NO", TmsApcnp.App));
                }
               
                if (TmsApcnp.FamSeq != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CATEG_FAM_SEQ", TmsApcnp.FamSeq));
                }
                if (TmsApcnp.Seq != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CATEG_SEQ", TmsApcnp.Seq));
                }
                //if (TmsApcnp.Type != string.Empty)
                //{
                //    sqlParamList.Add(new SqlParameter("@CATEG_CASH_ASSIST", TmsApcnp.Type));
                //}
                if (TmsApcnp.Heat_Cost != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CATEG_HEAT_COST", TmsApcnp.Heat_Cost));
                }
                //if (TmsApcnp.CaseWorker != string.Empty)
                //{
                //    sqlParamList.Add(new SqlParameter("@CATEG_VERIFIER", TmsApcnp.CaseWorker));
                //}


                if (TmsApcnp.SNAP != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CATEG_SNAP", TmsApcnp.SNAP));
                }
                if (TmsApcnp.SNAP_Worker != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CATEG_SNAP_WORKER", TmsApcnp.SNAP_Worker));
                }
                if (TmsApcnp.TFA != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CATEG_TFA", TmsApcnp.TFA));
                }
                if (TmsApcnp.TFA_Worker != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CATEG_TFA_WORKER", TmsApcnp.TFA_Worker));
                }
                if (TmsApcnp.RCash != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CATEG_RCASH", TmsApcnp.RCash));
                }
                if (TmsApcnp.RCash_worker != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CATEG_RCASH_WORKER", TmsApcnp.RCash_worker));
                }
                if (TmsApcnp.SS != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CATEG_SS", TmsApcnp.SS));
                }
                if (TmsApcnp.SS_Worker != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CATEG_SS_WORKER", TmsApcnp.SS_Worker));
                }
                if (TmsApcnp.SSI != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CATEG_SSI", TmsApcnp.SSI));
                }
                if (TmsApcnp.SSI_Worker != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CATEG_SSI_WORKER", TmsApcnp.SSI_Worker));
                }

                if (TmsApcnp.VerMethod != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CATEG_VER_METHOD", TmsApcnp.VerMethod));
                }

                if (TmsApcnp.HowVerified != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CATEG_HOW_VERIFIED", TmsApcnp.HowVerified));
                }

                sqlParamList.Add(new SqlParameter("@CATEG_LSTC_OPERATOR", TmsApcnp.LstcOperator));
                sqlParamList.Add(new SqlParameter("@CATEG_ADD_OPERATOR", TmsApcnp.AddOperator));
                sqlParamList.Add(new SqlParameter("@Mode", TmsApcnp.Mode));
                boolsuccess = Captain.DatabaseLayer.LiheDb.InsertUpdateCATGELIG(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public List<LiheApvEntity> GETTMSB0019Report(string agency, string dep, string program, string year, string FromIncome, string ToIncome, string Cerstatus, string State,string Benefitlevel,string BenefitType,string FromRemain,string ToRemain,string FromPaid,string ToPaid,string strVendorlist)
        {
            List<LiheApvEntity> LiheApvDetails = new List<LiheApvEntity>();
            try
            {
                DataSet LiheApv = Captain.DatabaseLayer.LiheDb.GETTMSB0019Report(agency, dep, program, year, FromIncome, ToIncome, Cerstatus, State, Benefitlevel, BenefitType, FromRemain, ToRemain, FromPaid, ToPaid, strVendorlist);
                if (LiheApv != null && LiheApv.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in LiheApv.Tables[0].Rows)
                    {
                        LiheApvDetails.Add(new LiheApvEntity(row,string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return LiheApvDetails;
            }

            return LiheApvDetails;
        }



        public List<CommonEntity> GetStatus()
        {
            List<CommonEntity> Status = new List<CommonEntity>();
            Status.Add(new CommonEntity("O", "Opened"));
            Status.Add(new CommonEntity("C", "Closed"));

            return Status;
        }

        public string GetStatusvalue(string strValue)
        {
            string strStatusValue = string.Empty;
            if (strValue == "O")
                strStatusValue = "Opened";
            else
                strStatusValue = "Closed";

            return strStatusValue;
        }


        public bool InsertUpdateDelLiheApv(LiheApvEntity ApvEntity)//, out string strMsg
        {
            bool boolSuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();


                sqlParamList.Add(new SqlParameter("@LPV_AGENCY", ApvEntity.LPV_AGENCY));
                sqlParamList.Add(new SqlParameter("@LPV_DEPT", ApvEntity.LPV_DEPT));
                sqlParamList.Add(new SqlParameter("@LPV_PROGRAM", ApvEntity.LPV_PROGRAM));
                sqlParamList.Add(new SqlParameter("@LPV_YEAR", ApvEntity.LPV_YEAR));
                sqlParamList.Add(new SqlParameter("@LPV_APP_NO", ApvEntity.LPV_APP_NO));
                if (ApvEntity.LPV_SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LPV_SEQ", ApvEntity.LPV_SEQ));

                if (ApvEntity.LPV_VENDOR != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPV_VENDOR ", ApvEntity.LPV_VENDOR));
                }
                if (ApvEntity.LPV_ACCOUNT_NO != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPV_ACCOUNT_NO", ApvEntity.LPV_ACCOUNT_NO));
                }
                if (ApvEntity.LPV_PRIMARY_CODE != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPV_PRIMARY_CODE ", ApvEntity.LPV_PRIMARY_CODE));
                }
                if (ApvEntity.LPV_PAYMENT_FOR != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPV_PAYMENT_FOR", ApvEntity.LPV_PAYMENT_FOR));
                }
                if (ApvEntity.LPV_CYCLE != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPV_CYCLE", ApvEntity.LPV_CYCLE));
                }
                if (ApvEntity.LPV_DIVIDE_BILL != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPV_DIVIDE_BILL", ApvEntity.LPV_DIVIDE_BILL));
                }

                if (ApvEntity.LPV_BILL_LNAME != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPV_BILL_LNAME", ApvEntity.LPV_BILL_LNAME));
                }

                if (ApvEntity.LPV_BILL_FNAME != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPV_BILL_FNAME", ApvEntity.LPV_BILL_FNAME));
                }

                if (ApvEntity.LPV_MOR != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPV_MOR", ApvEntity.LPV_MOR));
                }
                if (ApvEntity.LPV_METER != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPV_METER", ApvEntity.LPV_METER));
                }
                if (ApvEntity.LPV_ADD_OPERATOR != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPV_ADD_OPERATOR", ApvEntity.LPV_ADD_OPERATOR));
                }
                if (ApvEntity.LPV_LSTC_OPERATOR != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPV_LSTC_OPERATOR", ApvEntity.LPV_LSTC_OPERATOR));
                }
                if (ApvEntity.LPV_REVERIFY != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPV_REVERIFY", ApvEntity.LPV_REVERIFY));
                }
                if(ApvEntity.LPV_BILLNAME_TYPE!=string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPV_BILLNAME_TYPE", ApvEntity.LPV_BILLNAME_TYPE));
                }
                if (ApvEntity.LVR_FAILED_ACCOUNT_EDIT != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPV_FAILED_ACCOUNT_EDIT", ApvEntity.LVR_FAILED_ACCOUNT_EDIT));
                }
                sqlParamList.Add(new SqlParameter("@Mode", ApvEntity.Mode));
                //SqlParameter parameterMsg = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                //parameterMsg.Direction = ParameterDirection.Output;
                //sqlParamList.Add(parameterMsg);
                boolSuccess = Captain.DatabaseLayer.LiheDb.InsertUpdateDelLiheApv(sqlParamList);
                //strMsg = parameterMsg.Value.ToString();
            }

            catch (Exception ex)
            {
                //strMsg = string.Empty;
                return false;
            }

            return boolSuccess;

        }

        //Yeswanth
        public List<LIHEAPBEntity> Browse_LIHEAPB(LIHEAPBEntity Entity, string Opretaion_Mode)
        {
            List<LIHEAPBEntity> LIHEAPBProfile = new List<LIHEAPBEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_LIHEAPB_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet LIHEAPBData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_LIHEAPB]");
                //DataSet LIHEAPBData = Captain.DatabaseLayer.SPAdminDB.Browse_LIHEAPB(sqlParamList);

                if (LIHEAPBData != null && LIHEAPBData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in LIHEAPBData.Tables[0].Rows)
                        LIHEAPBProfile.Add(new LIHEAPBEntity(row));
                }
            }
            catch (Exception ex)
            { return LIHEAPBProfile; }

            return LIHEAPBProfile;
        }

        public bool Delete_Additional_BenRecs(string Agy, string Dept, string Prog, string Year, string App, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@LPB_AGENCY", Agy));
                sqlParamList.Add(new SqlParameter("@LPB_DEPT", Dept));
                sqlParamList.Add(new SqlParameter("@LPB_PROG", Prog));
                sqlParamList.Add(new SqlParameter("@LPB_YEAR", Year));
                sqlParamList.Add(new SqlParameter("@LPB_APP_NO", App));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.DeleteLIHEAPB_Additional_BenRecs", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool Delete_unAlloated_BenRecs(string Agy, string Dept, string Prog, string Year, string App, string Ben_string, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@LPB_AGENCY", Agy));
                sqlParamList.Add(new SqlParameter("@LPB_DEPT", Dept));
                sqlParamList.Add(new SqlParameter("@LPB_PROG", Prog));
                sqlParamList.Add(new SqlParameter("@LPB_YEAR", Year));
                sqlParamList.Add(new SqlParameter("@LPB_APP_NO", App));
                sqlParamList.Add(new SqlParameter("@LPB_Type_List", Ben_string));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.DeleteLIHEAPB_Unalloated_BenRecs", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateLIHEAPB(LIHEAPBEntity Entity, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_LIHEAPB_SqlParameters_List(Entity, Entity.Rec_Type);
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateLIHEAPB", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<SqlParameter> Prepare_LIHEAPB_SqlParameters_List(LIHEAPBEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@LPB_AGENCY", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@LPB_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@LPB_PROG", Entity.Prog));
                sqlParamList.Add(new SqlParameter("@LPB_YEAR", Entity.Year));
                sqlParamList.Add(new SqlParameter("@LPB_APP_NO", Entity.AppNo));
                sqlParamList.Add(new SqlParameter("@LPB_SEQ", Entity.Seq));
                sqlParamList.Add(new SqlParameter("@LPB_TYPE", Entity.Award_Type));
                if (!string.IsNullOrEmpty(Entity.Award_Amount))
                    sqlParamList.Add(new SqlParameter("@LPB_AMOUNT", Entity.Award_Amount));
                if (!string.IsNullOrEmpty(Entity.Paid))
                    sqlParamList.Add(new SqlParameter("@LPB_PAID", Entity.Paid));
                if (!string.IsNullOrEmpty(Entity.Awaiting_Dol))
                    sqlParamList.Add(new SqlParameter("@LPB_AWAITING_DOL", Entity.Awaiting_Dol));
                if (!string.IsNullOrEmpty(Entity.Assets))
                    sqlParamList.Add(new SqlParameter("@LPB_ASSETS", Entity.Assets));
                if (!string.IsNullOrEmpty(Entity.Award_Date))
                    sqlParamList.Add(new SqlParameter("@LPB_DATE", Entity.Award_Date));
                if (!string.IsNullOrEmpty(Entity.Caseworker))
                    sqlParamList.Add(new SqlParameter("@LPB_CASEWORKER", Entity.Caseworker));
                if (!string.IsNullOrEmpty(Entity.Notice_Date))
                    sqlParamList.Add(new SqlParameter("@LPB_NOTICE_DATE",  Entity.Notice_Date));
                if (!string.IsNullOrEmpty(Entity.Resend))
                    sqlParamList.Add(new SqlParameter("@LPB_RESEND", Entity.Resend));
                if (!string.IsNullOrEmpty(Entity.Deny))
                    sqlParamList.Add(new SqlParameter("@LPB_DENY", Entity.Deny));
                if (!string.IsNullOrEmpty(Entity.Age_dis))
                    sqlParamList.Add(new SqlParameter("@LPB_AGEDIS", Entity.Age_dis));
                if (!string.IsNullOrEmpty(Entity.Source))
                    sqlParamList.Add(new SqlParameter("@LPB_SOURCE", Entity.Source));
                if (!string.IsNullOrEmpty(Entity.Inc_Cert))
                    sqlParamList.Add(new SqlParameter("@LPB_INC", Entity.Inc_Cert));
                if (!string.IsNullOrEmpty(Entity.CATELIG_Cert))
                    sqlParamList.Add(new SqlParameter("@LPB_CATELIG", Entity.CATELIG_Cert));
                if (!string.IsNullOrEmpty(Entity.Renter))
                    sqlParamList.Add(new SqlParameter("@LPB_RENTER", Entity.Renter));
                if (!string.IsNullOrEmpty(Entity.FAP_Income))
                    sqlParamList.Add(new SqlParameter("@LPB_FAP_INCOME", Entity.FAP_Income));
                if (!string.IsNullOrEmpty(Entity.FAP_No_InHH))
                    sqlParamList.Add(new SqlParameter("@LPB_FAP_NO_INHH", Entity.FAP_No_InHH));
                if (!string.IsNullOrEmpty(Entity.Reason_Denied))
                    sqlParamList.Add(new SqlParameter("@LPB_REASON_DENIED", Entity.Reason_Denied));
                if (!string.IsNullOrEmpty(Entity.Action_Code))
                    sqlParamList.Add(new SqlParameter("@LPB_ACTION_CODE", Entity.Action_Code));
                if (!string.IsNullOrEmpty(Entity.AU))
                    sqlParamList.Add(new SqlParameter("@LPB_AU", Entity.AU));
                if (!string.IsNullOrEmpty(Entity.Payment_How))
                    sqlParamList.Add(new SqlParameter("@LPB_PAYMENT_HOW", Entity.Payment_How));
                if (!string.IsNullOrEmpty(Entity.Disable))
                    sqlParamList.Add(new SqlParameter("@LPB_DISABLE", Entity.Disable));
                if (!string.IsNullOrEmpty(Entity.Inc_Worker))
                    sqlParamList.Add(new SqlParameter("@LPB_INCWORKER", Entity.Inc_Worker));
                if (!string.IsNullOrEmpty(Entity.Inc_Date))
                    sqlParamList.Add(new SqlParameter("@LPB_INCDATE", Entity.Inc_Date));

                if (!string.IsNullOrEmpty(Entity.Benefit_Level))
                    sqlParamList.Add(new SqlParameter("@LPB_BENEFIT_LEVEL", Entity.Benefit_Level));
                if (!string.IsNullOrEmpty(Entity.MOD))
                    sqlParamList.Add(new SqlParameter("@LPB_MOD", Entity.MOD));
                if (!string.IsNullOrEmpty(Entity.Elig_Assets))
                    sqlParamList.Add(new SqlParameter("@LPB_ELIGASSETS", Entity.Elig_Assets));
                if (!string.IsNullOrEmpty(Entity.Elig_Assets1))
                    sqlParamList.Add(new SqlParameter("@LPB_ELIGASSETS1", Entity.Elig_Assets1));
                if (!string.IsNullOrEmpty(Entity.Elig_Income))
                    sqlParamList.Add(new SqlParameter("@LPB_ELIGINCOME", Entity.Elig_Income));
                if (!string.IsNullOrEmpty(Entity.Elig_Fuel))
                    sqlParamList.Add(new SqlParameter("@LPB_ELIGFUEL", Entity.Elig_Fuel));
                if (!string.IsNullOrEmpty(Entity.Elig_Rent))
                    sqlParamList.Add(new SqlParameter("@LPB_ELIGRENT", Entity.Elig_Rent));
                if (!string.IsNullOrEmpty(Entity.Elig_Wthr))
                    sqlParamList.Add(new SqlParameter("@LPB_ELIGWTHR", Entity.Elig_Wthr));
                if (!string.IsNullOrEmpty(Entity.Contingency))
                    sqlParamList.Add(new SqlParameter("@LPB_CONTINGENCY", Entity.Contingency));
                if (!string.IsNullOrEmpty(Entity.Reduce_Elig))
                    sqlParamList.Add(new SqlParameter("@LPB_REDUCE_ELIG", Entity.Reduce_Elig));
                if (!string.IsNullOrEmpty(Entity.Certified_Status))
                    sqlParamList.Add(new SqlParameter("@LPB_CERTIFIED_STATUS", Entity.Certified_Status));
                if (!string.IsNullOrEmpty(Entity.Date_Renotify))
                    sqlParamList.Add(new SqlParameter("@LPB_DATE_RENOTIFY", Entity.Date_Renotify));
                if (!string.IsNullOrEmpty(Entity.Letter_NO))
                    sqlParamList.Add(new SqlParameter("@LPB_LETTER_NO", Entity.Letter_NO));
                if (!string.IsNullOrEmpty(Entity.Pay_Notice_Date))
                    sqlParamList.Add(new SqlParameter("@LPB_PAY_NOTICE_DATE", Entity.Pay_Notice_Date));
                if (!string.IsNullOrEmpty(Entity.Fund_Code))
                    sqlParamList.Add(new SqlParameter("@LPB_FUND_CODE", Entity.Fund_Code));

                if (!string.IsNullOrEmpty(Entity.SW1))
                    sqlParamList.Add(new SqlParameter("@LPB_SW1", Entity.SW1));
                if (!string.IsNullOrEmpty(Entity.SW2))
                    sqlParamList.Add(new SqlParameter("@LPB_SW2", Entity.SW2));
                if (!string.IsNullOrEmpty(Entity.SW3))
                    sqlParamList.Add(new SqlParameter("@LPB_SW3", Entity.SW3));
                if (!string.IsNullOrEmpty(Entity.SW4))
                    sqlParamList.Add(new SqlParameter("@LPB_SW4", Entity.SW4));
                if (!string.IsNullOrEmpty(Entity.SW5))
                    sqlParamList.Add(new SqlParameter("@LPB_SW5", Entity.SW5));
                if (!string.IsNullOrEmpty(Entity.SW6))
                    sqlParamList.Add(new SqlParameter("@LPB_SW6", Entity.SW6));
                if (!string.IsNullOrEmpty(Entity.SW7))
                    sqlParamList.Add(new SqlParameter("@LPB_SW7", Entity.SW7));
                if (!string.IsNullOrEmpty(Entity.SW8))
                    sqlParamList.Add(new SqlParameter("@LPB_SW8", Entity.SW8));

                if (!string.IsNullOrEmpty(Entity.SSN_SW))
                    sqlParamList.Add(new SqlParameter("@LPB_SSN_SW", Entity.SSN_SW));
                if (!string.IsNullOrEmpty(Entity.BatchNO))
                    sqlParamList.Add(new SqlParameter("@LPB_BATCHNO", Entity.BatchNO));
                if (!string.IsNullOrEmpty(Entity.Remaining))
                    sqlParamList.Add(new SqlParameter("@LPB_REMAINING", Entity.Remaining));
                if (!string.IsNullOrEmpty(Entity.CONS_Gallons))
                    sqlParamList.Add(new SqlParameter("@LPB_CONS_GALLONS", Entity.CONS_Gallons));
                if (!string.IsNullOrEmpty(Entity.CONS_Dollars))
                    sqlParamList.Add(new SqlParameter("@LPB_CONS_DOLLARS", Entity.CONS_Dollars));
                if (!string.IsNullOrEmpty(Entity.CONS_Months))
                    sqlParamList.Add(new SqlParameter("@LPB_CONS_MONTHS", Entity.CONS_Months));
                if (!string.IsNullOrEmpty(Entity.CONS_Gallons_NY))
                    sqlParamList.Add(new SqlParameter("@LPB_CONS_GALLONS_NY", Entity.CONS_Gallons_NY));
                if (!string.IsNullOrEmpty(Entity.CONS_Dollars_NY))
                    sqlParamList.Add(new SqlParameter("@LPB_CONS_DOLLARS_NY", Entity.CONS_Dollars_NY));
                if (!string.IsNullOrEmpty(Entity.CONS_Months_NY))
                    sqlParamList.Add(new SqlParameter("@LPB_CONS_MONTHS_NY", Entity.CONS_Months_NY));
                if (!string.IsNullOrEmpty(Entity.WEATH_Intrest))
                    sqlParamList.Add(new SqlParameter("@LPB_WEATH_INTEREST", Entity.WEATH_Intrest));
                if (!string.IsNullOrEmpty(Entity.WEATH_No_Rooms))
                    sqlParamList.Add(new SqlParameter("@LPB_WEATH_NOROOMS", Entity.WEATH_No_Rooms));
                if (!string.IsNullOrEmpty(Entity.WEATH_Share))
                    sqlParamList.Add(new SqlParameter("@LPB_WEATH_SHARE", Entity.WEATH_Share));
                if (!string.IsNullOrEmpty(Entity.WEATH_Before))
                    sqlParamList.Add(new SqlParameter("@LPB_WEATH_BEFORE", Entity.WEATH_Before));
                if (!string.IsNullOrEmpty(Entity.Override))
                    sqlParamList.Add(new SqlParameter("@LPB_OVERRIDE", Entity.Override));
                if (!string.IsNullOrEmpty(Entity.APP_Applied))
                    sqlParamList.Add(new SqlParameter("@LPB_APP_APPLIED", Entity.APP_Applied));
                if (!string.IsNullOrEmpty(Entity.INCOMP_Assets))
                    sqlParamList.Add(new SqlParameter("@LPB_INCOMP_ASSET", Entity.INCOMP_Assets));

                if (!string.IsNullOrEmpty(Entity.INCOMP_SSN))
                    sqlParamList.Add(new SqlParameter("@LPB_INCOMP_SSN", Entity.INCOMP_SSN));
                if (!string.IsNullOrEmpty(Entity.INCOMP_Rent))
                    sqlParamList.Add(new SqlParameter("@LPB_INCOMP_RENT", Entity.INCOMP_Rent));
                if (!string.IsNullOrEmpty(Entity.INCOMP_Util))
                    sqlParamList.Add(new SqlParameter("@LPB_INCOMP_UTIL", Entity.INCOMP_Util));
                if (!string.IsNullOrEmpty(Entity.Categ_Elig))
                    sqlParamList.Add(new SqlParameter("@LPB_CATEG_ELIG", Entity.Categ_Elig));
                if (!string.IsNullOrEmpty(Entity.TKT_Balance))
                    sqlParamList.Add(new SqlParameter("@LPB_TKT_BALANCE", Entity.TKT_Balance));

                if (!string.IsNullOrEmpty(Entity.HH_FS_Benefit))
                    sqlParamList.Add(new SqlParameter("@LPB_HH_FS_BENIFIT", Entity.HH_FS_Benefit));
                if (!string.IsNullOrEmpty(Entity.DSS_ADFC))
                    sqlParamList.Add(new SqlParameter("@LPB_DSS_ADFC", Entity.DSS_ADFC));

                if (!string.IsNullOrEmpty(Entity.WAP_Date))
                    sqlParamList.Add(new SqlParameter("@LPB_WAP_DATE", Entity.WAP_Date));
                if (!string.IsNullOrEmpty(Entity.WAP_Operator))
                    sqlParamList.Add(new SqlParameter("@LPB_WAP_OPERATOR", Entity.WAP_Operator));
                if (!string.IsNullOrEmpty(Entity.LSTC_Operator))
                    sqlParamList.Add(new SqlParameter("@LPB_LSTC_OPERATOR", Entity.LSTC_Operator));

                if (!string.IsNullOrEmpty(Entity.LiquidAsset))
                    sqlParamList.Add(new SqlParameter("@LPB_LIQUID_ASSET", Entity.LiquidAsset));

                if (!string.IsNullOrEmpty(Entity.RiskAsses))
                    sqlParamList.Add(new SqlParameter("@LPB_RISK", Entity.RiskAsses));

                if (!string.IsNullOrEmpty(Entity.Denied_Reason))
                    sqlParamList.Add(new SqlParameter("@LPB_DENIED_REASON", Entity.Denied_Reason));

                if (Opretaion_Mode == "Browse")
                {
                    if (!string.IsNullOrEmpty(Entity.Add_Date))
                        sqlParamList.Add(new SqlParameter("@LPB_DATE_ADD", Entity.Add_Date));
                    if (!string.IsNullOrEmpty(Entity.LSTC_Date))
                        sqlParamList.Add(new SqlParameter("@LPB_DATE_LSTC", Entity.LSTC_Date));
                    if (!string.IsNullOrEmpty(Entity.Add_Operatop))
                        sqlParamList.Add(new SqlParameter("@LPB_ADD_OPERATOR", Entity.Add_Operatop));
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<TrigMast_Entity> Browse_TriggerMaster(string Trig_id, string Trig_Name)
        {
            List<TrigMast_Entity> LIHEAPBProfile = new List<TrigMast_Entity>();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                //if()
                //sqlParamList.Add(new SqlParameter("@TRG_ID", Trig_id));
                //sqlParamList.Add(new SqlParameter("@TRG_Name", Trig_Name)); 
                DataSet LIHEAPBData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_TrigMast]");
                //DataSet LIHEAPBData = Captain.DatabaseLayer.SPAdminDB.Browse_LIHEAPB(sqlParamList);

                if (LIHEAPBData != null && LIHEAPBData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in LIHEAPBData.Tables[0].Rows)
                        LIHEAPBProfile.Add(new TrigMast_Entity(row));
                }
            }
            catch (Exception ex)
            { return LIHEAPBProfile; }

            return LIHEAPBProfile;
        }

        #region Sudheer
        public List<CTTRIGCRITEntity> Browse_TriggerMaster1(string Trig_id, string Trig_Name)
        {
            List<CTTRIGCRITEntity> LIHEAPBProfile = new List<CTTRIGCRITEntity>();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                //if()
                //sqlParamList.Add(new SqlParameter("@TRG_ID", Trig_id));
                //sqlParamList.Add(new SqlParameter("@TRG_Name", Trig_Name)); 
                DataSet LIHEAPBData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_TrigMast]");
                //DataSet LIHEAPBData = Captain.DatabaseLayer.SPAdminDB.Browse_LIHEAPB(sqlParamList);

                if (LIHEAPBData != null && LIHEAPBData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in LIHEAPBData.Tables[0].Rows)
                        LIHEAPBProfile.Add(new CTTRIGCRITEntity(row));
                }
            }
            catch (Exception ex)
            { return LIHEAPBProfile; }

            return LIHEAPBProfile;
        }

        #endregion

        public bool UpdateTrigger_Table(string Row_Type, string Trig_id, string Trig_Crit, string Trig_Name, string User_ID, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@Row_Type", Row_Type));
                sqlParamList.Add(new SqlParameter("@TRGASM_ID", Trig_id));
                sqlParamList.Add(new SqlParameter("@TRG_Criteria", Trig_Crit));
                sqlParamList.Add(new SqlParameter("@TRG_Name", Trig_Name));
                sqlParamList.Add(new SqlParameter("@User_name", User_ID));

                
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateTrigMast", out Sql_Reslut_Msg);

               
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<TrigHist_Entity> Browse_TriggerHistory(string Trig_id)
        {
            List<TrigHist_Entity> LIHEAPBProfile = new List<TrigHist_Entity>();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                //if()
                //sqlParamList.Add(new SqlParameter("@TRG_ID", Trig_id));
                //sqlParamList.Add(new SqlParameter("@TRG_Name", Trig_Name)); 
                DataSet LIHEAPBData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_TrigHist]");
                //DataSet LIHEAPBData = Captain.DatabaseLayer.SPAdminDB.Browse_LIHEAPB(sqlParamList);

                if (LIHEAPBData != null && LIHEAPBData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in LIHEAPBData.Tables[0].Rows)
                        LIHEAPBProfile.Add(new TrigHist_Entity(row));
                }
            }
            catch (Exception ex)
            { return LIHEAPBProfile; }

            return LIHEAPBProfile;
        }

        public bool UpdateTrigger_History(string Trig_id, string Old_Crit, string New_Crit, string User_ID, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@Trig_ID", Trig_id));
                sqlParamList.Add(new SqlParameter("@Trig_Old_Data", Old_Crit));
                sqlParamList.Add(new SqlParameter("@Trig_New_Data", New_Crit));
                sqlParamList.Add(new SqlParameter("@Trig_OPERATOR", User_ID));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateTrigHist", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool Update_Trig_Assmst_History(string Trig_id, string Trig_Seq, string Old_Crit, string New_Crit, string User_ID, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@AsmsHist_ID", Trig_id));
                sqlParamList.Add(new SqlParameter("@AsmsHist_Seq", Trig_Seq));
                sqlParamList.Add(new SqlParameter("@AsmsHist_Old_Crit", Old_Crit));
                sqlParamList.Add(new SqlParameter("@AsmsHist_New_Crit", New_Crit));
                sqlParamList.Add(new SqlParameter("@Trig_OPERATOR", User_ID));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateTrigAssnMstHist", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<TrigAssnsHist> Browse_TrigAssnsHist(string Trig_id, string ID_Seq)
        {
            List<TrigAssnsHist> LIHEAPBProfile = new List<TrigAssnsHist>();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                //if()
                sqlParamList.Add(new SqlParameter("@AssCAMSHist_ID", Trig_id));
                sqlParamList.Add(new SqlParameter("@AssCAMSHist_Seq", ID_Seq)); 
                DataSet LIHEAPBData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_TrigAssnsHist]");
                //DataSet LIHEAPBData = Captain.DatabaseLayer.SPAdminDB.Browse_LIHEAPB(sqlParamList);

                if (LIHEAPBData != null && LIHEAPBData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in LIHEAPBData.Tables[0].Rows)
                        LIHEAPBProfile.Add(new TrigAssnsHist(row));
                }
            }
            catch (Exception ex)
            { return LIHEAPBProfile; }

            return LIHEAPBProfile;
        }

        public List<TrigAssnMstHist_Entity> Browse_TrigAssnMstHist(string Trig_id, string Trig_Seq)
        {
            List<TrigAssnMstHist_Entity> LIHEAPBProfile = new List<TrigAssnMstHist_Entity>();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                //if()
                sqlParamList.Add(new SqlParameter("@AsmsHist_ID", Trig_id));
                sqlParamList.Add(new SqlParameter("@AsmsHist_Seq", Trig_Seq)); 
                DataSet LIHEAPBData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_TrigAssnMstHist]");
                //DataSet LIHEAPBData = Captain.DatabaseLayer.SPAdminDB.Browse_LIHEAPB(sqlParamList);

                if (LIHEAPBData != null && LIHEAPBData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in LIHEAPBData.Tables[0].Rows)
                        LIHEAPBProfile.Add(new TrigAssnMstHist_Entity(row));
                }
            }
            catch (Exception ex)
            { return LIHEAPBProfile; }

            return LIHEAPBProfile;
        }

        public bool Update_Trig_Assns_History(string Trig_id, string Trig_Seq, string Old_Crit, string New_Crit, string User_ID, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@AssCAMSHist_ID", Trig_id));
                sqlParamList.Add(new SqlParameter("@AssCAMSHist_Seq", Trig_Seq));
                sqlParamList.Add(new SqlParameter("@AssCAMSHist_Old_Crit", Old_Crit));
                sqlParamList.Add(new SqlParameter("@AssCAMSHist_New_Crit", New_Crit));
                sqlParamList.Add(new SqlParameter("@AssCAMSHist_ADD_OPERATOR", User_ID));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateTrigAssnsHist", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }


        public List<TrigAssnMst_Entity> Browse_TrigAssnMst(string Trig_id, string Trig_Name)
        {
            List<TrigAssnMst_Entity> LIHEAPBProfile = new List<TrigAssnMst_Entity>();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                //if()
                //sqlParamList.Add(new SqlParameter("@TRG_ID", Trig_id));
                //sqlParamList.Add(new SqlParameter("@TRG_Name", Trig_Name)); 
                DataSet LIHEAPBData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_TrigAssnMst]");
                //DataSet LIHEAPBData = Captain.DatabaseLayer.SPAdminDB.Browse_LIHEAPB(sqlParamList);

                if (LIHEAPBData != null && LIHEAPBData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in LIHEAPBData.Tables[0].Rows)
                        LIHEAPBProfile.Add(new TrigAssnMst_Entity(row));
                }
            }
            catch (Exception ex)
            { return LIHEAPBProfile; }

            return LIHEAPBProfile;
        }

        public List<TrigAssns_Entity> Browse_TrigAssn_CAMS(string Trig_id, string Trig_Seq)
        {
            List<TrigAssns_Entity> LIHEAPBProfile = new List<TrigAssns_Entity>();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                //if()
                //sqlParamList.Add(new SqlParameter("@TRG_ID", Trig_id));
                //sqlParamList.Add(new SqlParameter("@TRG_Name", Trig_Name)); 
                DataSet LIHEAPBData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_TrigAssns]");
                //DataSet LIHEAPBData = Captain.DatabaseLayer.SPAdminDB.Browse_LIHEAPB(sqlParamList);

                if (LIHEAPBData != null && LIHEAPBData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in LIHEAPBData.Tables[0].Rows)
                        LIHEAPBProfile.Add(new TrigAssns_Entity(row));
                }
            }
            catch (Exception ex)
            { return LIHEAPBProfile; }

            return LIHEAPBProfile;
        }

        public bool UpdateTrigAssnMst(string Row_Type, string Trig_id, string Trig_seq, string Trig_SP, string Trig_Assoc_Name,string Datecombo,string Program, string User_ID, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@Row_Type", Row_Type));
                sqlParamList.Add(new SqlParameter("@TRGASM_ID", Trig_id));
                sqlParamList.Add(new SqlParameter("@TRGASM_SEQ", Trig_seq));
                sqlParamList.Add(new SqlParameter("@TRGASM_SERPLAN", Trig_SP));
                sqlParamList.Add(new SqlParameter("@TRAGSM_Name", Trig_Assoc_Name));
                sqlParamList.Add(new SqlParameter("@TRAGSM_DATE", Datecombo));
                sqlParamList.Add(new SqlParameter("@TRAGSM_PROGRAM", Program));
                sqlParamList.Add(new SqlParameter("@TRGASM_LSTC_OPERATOR", User_ID));


                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateTrigAssnMst", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateTrigAssns(string Row_Type, string Trig_id, string Trig_seq, string Trig_SP, string Branch, string ogr_Grp, string CAMS_Type, string CAMS_Code, string CAMS_Xml, string User_ID, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@Row_Type", Row_Type));
                sqlParamList.Add(new SqlParameter("@TRGASM_ID", Trig_id));
                sqlParamList.Add(new SqlParameter("@TRGASM_SEQ", Trig_seq));
                sqlParamList.Add(new SqlParameter("@TRGAS_SERPLAN", Trig_SP));

                sqlParamList.Add(new SqlParameter("@TRGAS_BRANCH", Branch));
                sqlParamList.Add(new SqlParameter("@TRGAS_ORIG_GRP", ogr_Grp));
                sqlParamList.Add(new SqlParameter("@TRGAS_CAMS", CAMS_Type));
                sqlParamList.Add(new SqlParameter("@TRGAS_CAMS_CODE", CAMS_Code));
                sqlParamList.Add(new SqlParameter("@TRGAS_LSTC_OPERATOR", User_ID));

                sqlParamList.Add(new SqlParameter("@Applicants_XML", CAMS_Xml));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateTrigAssns", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        //public List<CTTRIGQUESTSEntity> Browse_TRIGQUESTS()
        //{
        //    List<CTTRIGQUESTSEntity> LIHEAPBProfile = new List<CTTRIGQUESTSEntity>();
        //    try
        //    {
        //        List<SqlParameter> sqlParamList = new List<SqlParameter>();

        //        DataSet LIHEAPBData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CTTRIGQUESTS]");
                
        //        if (LIHEAPBData != null && LIHEAPBData.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow row in LIHEAPBData.Tables[0].Rows)
        //                LIHEAPBProfile.Add(new CTTRIGQUESTSEntity(row));
        //        }
        //    }
        //    catch (Exception ex)
        //    { return LIHEAPBProfile; }

        //    return LIHEAPBProfile;
        //}

        public List<CTTRIGQUESTSEntity> GetTRIGQUES()
        {
            List<CTTRIGQUESTSEntity> CaseEligQuesProfile = new List<CTTRIGQUESTSEntity>();
            try
            {
                DataSet CaseEligData = Captain.DatabaseLayer.SPAdminDB.GetTRIGQUES();
                if (CaseEligData != null && CaseEligData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseEligData.Tables[0].Rows)
                    {
                        CaseEligQuesProfile.Add(new CTTRIGQUESTSEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseEligQuesProfile;
            }

            return CaseEligQuesProfile;
        }

        public bool InsertUpdateDelCTTRIGCRIT(CTTRIGCRITEntity TrigEntity, out string Msg)
        {
            bool boolSuccess = false;
            string strMsg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@CTTRIGCRIT_CODE", TrigEntity.TRIGCRITCode));
                sqlParamList.Add(new SqlParameter("@CTTRIGCRIT_GROUP_CODE", TrigEntity.TRIGCRITGroupCode));
                if (TrigEntity.TRIGCRITGroupSeq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CTTRIGCRIT_GROUP_SEQ", TrigEntity.TRIGCRITGroupSeq));
                if (TrigEntity.TRIGCRITGroupDesc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CTTRIGCRIT_GROUP_DESC", TrigEntity.TRIGCRITGroupDesc));
                if (TrigEntity.TRIGCRITQuesCode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CTTRIGCRIT_QUES_CODE", TrigEntity.TRIGCRITQuesCode));
                if (TrigEntity.TRIGCRITResponseType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CTTRIGCRIT_RESP_TYPE", TrigEntity.TRIGCRITResponseType));
                if (TrigEntity.TRIGCRITConjunction != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CTTRIGCRIT_CONJN", TrigEntity.TRIGCRITConjunction));
                if (TrigEntity.TRIGCRITNumEqual != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CTTRIGCRIT_NUM_EQUAL", TrigEntity.TRIGCRITNumEqual));
                if (TrigEntity.TRIGCRITNumLthan != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CTTRIGCRIT_NUM_LTHAN", TrigEntity.TRIGCRITNumLthan));
                if (TrigEntity.TRIGCRITNumGthan != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CTTRIGCRIT_NUM_GTHAN", TrigEntity.TRIGCRITNumGthan));
                if (TrigEntity.TRIGCRITDateEqual != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CTTRIGCRIT_DATE_EQUAL", TrigEntity.TRIGCRITDateEqual));
                if (TrigEntity.TRIGCRITDateLthan != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CTTRIGCRIT_DATE_LTHAN", TrigEntity.TRIGCRITDateLthan));
                if (TrigEntity.TRIGCRITDateGthan != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CTTRIGCRIT_DATE_GTHAN", TrigEntity.TRIGCRITDateGthan));
                if (TrigEntity.TRIGCRITDDTextResp != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CTTRIGCRIT_DD_TEXT_RESP", TrigEntity.TRIGCRITDDTextResp));
                if (TrigEntity.AddOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CTTRIGCRIT_ADD_OPERATOR", TrigEntity.AddOperator));
                if (TrigEntity.LstcOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CTTRIGCRIT_LSTC_OPERATOR", TrigEntity.LstcOperator));
                if (TrigEntity.TRIGCRITMemAccess != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CTTRIGCRIT_MEM_ACCESS", TrigEntity.TRIGCRITMemAccess));
                if (TrigEntity.TRIGCRITValidated != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CTTRIGCRIT_VALIDATED", TrigEntity.TRIGCRITValidated));

                sqlParamList.Add(new SqlParameter("@Mode", TrigEntity.Mode));
                sqlParamList.Add(new SqlParameter("@Type", TrigEntity.Type));

                SqlParameter sqlApplNo = new SqlParameter("@Msg", SqlDbType.VarChar, 20);
                sqlApplNo.Value = TrigEntity.Msg;
                sqlApplNo.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlApplNo);

                boolSuccess = Captain.DatabaseLayer.SPAdminDB.InsertUpdateDelTRIGCRIT(sqlParamList);
                strMsg = sqlApplNo.Value.ToString();
            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }

            Msg = strMsg;
            return boolSuccess;
        }

        public List<CTTRIGCRITEntity> GetTRIGCRITadpgs(string Code,string groupcode, string seq, string type)
        {
            List<CTTRIGCRITEntity> CaseEligprofile = new List<CTTRIGCRITEntity>();
            try
            {
                DataSet CaseElighierchy = Captain.DatabaseLayer.SPAdminDB.GetTRIGCRITadpgs(Code, groupcode, seq, type);

                if (CaseElighierchy != null && CaseElighierchy.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseElighierchy.Tables[0].Rows)
                    {
                        if (type == "Group")
                            CaseEligprofile.Add(new CTTRIGCRITEntity(row, "Group"));
                        else
                            CaseEligprofile.Add(new CTTRIGCRITEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseEligprofile;
            }

            return CaseEligprofile;
        }

        public List<PAYMNETEntity> Browse_PAYMENT(PAYMNETEntity Entity, string Opretaion_Mode)
        {
            List<PAYMNETEntity> PAYMNETProfile = new List<PAYMNETEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_PAYMNET_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet PAYMNETData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_PAYMENT]");
                //Browse_PAYMENT_Reallocate

                if (PAYMNETData != null && PAYMNETData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in PAYMNETData.Tables[0].Rows)
                        PAYMNETProfile.Add(new PAYMNETEntity(row));
                }
            }
            catch (Exception ex)
            { return PAYMNETProfile; }

            return PAYMNETProfile;
        }

        public List<PAYMNETEntity> Browse_PAYMENT_Reallocate(PAYMNETEntity Entity, string Opretaion_Mode)
        {
            List<PAYMNETEntity> PAYMNETProfile = new List<PAYMNETEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_PAYMNET_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet PAYMNETData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_PAYMENT_Reallocate]");

                if (PAYMNETData != null && PAYMNETData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in PAYMNETData.Tables[0].Rows)
                        PAYMNETProfile.Add(new PAYMNETEntity(row));
                }
            }
            catch (Exception ex)
            { return PAYMNETProfile; }

            return PAYMNETProfile;
        }


        public bool UpdatePAYMENT(PAYMNETEntity Entity, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_PAYMNET_SqlParameters_List(Entity, "Update");
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdatePAYMENT", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }


        public List<SqlParameter> Prepare_PAYMNET_SqlParameters_List(PAYMNETEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@PMR_AGENCY", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@PMR_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@PMR_PROGRAM", Entity.Prog));
                sqlParamList.Add(new SqlParameter("@PMR_YEAR", Entity.Yaer));
                sqlParamList.Add(new SqlParameter("@PMR_APPL_NO", Entity.AppNo));
                sqlParamList.Add(new SqlParameter("@PMR_VENDOR_SEQ", Entity.Vendor_Seq));
                sqlParamList.Add(new SqlParameter("@PMR_REC_SEQ", Entity.Record_Seq));
                sqlParamList.Add(new SqlParameter("@PMR_BATCH_NO", Entity.Batch_NO));
                sqlParamList.Add(new SqlParameter("@PMR_DEL_VENDOR", Entity.Del_Vendor));
                if (!string.IsNullOrEmpty(Entity.Del_Date))
                    sqlParamList.Add(new SqlParameter("@PMR_DEL_DATE", Entity.Del_Date));
                sqlParamList.Add(new SqlParameter("@PMR_VENDOR", Entity.Vendor));
                if (!string.IsNullOrEmpty(Entity.Authr_Number))
                    sqlParamList.Add(new SqlParameter("@PMR_AUTH_NUM", Entity.Authr_Number));
                if (!string.IsNullOrEmpty(Entity.Authr_Date))
                    sqlParamList.Add(new SqlParameter("@PMR_AUTH_DATE", Entity.Authr_Date));
                sqlParamList.Add(new SqlParameter("@PMR_AUTH_CASEWORKER", Entity.Authr_Caseworker));

                if (!string.IsNullOrEmpty(Entity.Authr_Type1))
                    sqlParamList.Add(new SqlParameter("@PMR_AUTH_TYPE1", Entity.Authr_Type1));
                if (!string.IsNullOrEmpty(Entity.Authr_Type2))
                    sqlParamList.Add(new SqlParameter("@PMR_AUTH_TYPE2", Entity.Authr_Type2));
                if (!string.IsNullOrEmpty(Entity.Authr_Amount1))
                    sqlParamList.Add(new SqlParameter("@PMR_AUTH_AMOUNT1", Entity.Authr_Amount1));
                if (!string.IsNullOrEmpty(Entity.Authr_Amount2))
                    sqlParamList.Add(new SqlParameter("@PMR_AUTH_AMOUNT2", Entity.Authr_Amount2));
                if (!string.IsNullOrEmpty(Entity.Invc_Number))
                    sqlParamList.Add(new SqlParameter("@PMR_INVOICE_NUM", Entity.Invc_Number));
                if (!string.IsNullOrEmpty(Entity.Invc_Date))
                    sqlParamList.Add(new SqlParameter("@PMR_INVOICE_DATE", Entity.Invc_Date));
                if (!string.IsNullOrEmpty(Entity.Amount_Dol))
                    sqlParamList.Add(new SqlParameter("@PMR_AMOUNT_DOL", Entity.Amount_Dol));
                if (!string.IsNullOrEmpty(Entity.Amount_Dol1))
                    sqlParamList.Add(new SqlParameter("@PMR_AMOUNT_DOL1", Entity.Amount_Dol1));
                if (!string.IsNullOrEmpty(Entity.Amount_Dol2))
                    sqlParamList.Add(new SqlParameter("@PMR_AMOUNT_DOL2", Entity.Amount_Dol2));
                if (!string.IsNullOrEmpty(Entity.Amount_Delivery))
                    sqlParamList.Add(new SqlParameter("@PMR_AMOUNT_DELIVERY", Entity.Amount_Delivery));
                if (!string.IsNullOrEmpty(Entity.Amount_Startup))
                    sqlParamList.Add(new SqlParameter("@PMR_AMOUNT_STARTUP", Entity.Amount_Startup));
                if (!string.IsNullOrEmpty(Entity.Amount_MISC))
                    sqlParamList.Add(new SqlParameter("@PMR_AMOUNT_MISC", Entity.Amount_MISC));
                if (!string.IsNullOrEmpty(Entity.Amount_Gallons))
                    sqlParamList.Add(new SqlParameter("@PMR_AMOUNT_GAL", Entity.Amount_Gallons));
                if (!string.IsNullOrEmpty(Entity.Caseworker))
                    sqlParamList.Add(new SqlParameter("@PMR_CASEWORKER", Entity.Caseworker));
                if (!string.IsNullOrEmpty(Entity.Original_Amount))
                    sqlParamList.Add(new SqlParameter("@PMR_ORIGINAL_AMOUNT", Entity.Original_Amount));

                if (!string.IsNullOrEmpty(Entity.Vendor_PP))
                    sqlParamList.Add(new SqlParameter("@PMR_VENDOR_PP", Entity.Vendor_PP));
                if (!string.IsNullOrEmpty(Entity.MOR_PP))
                    sqlParamList.Add(new SqlParameter("@PMR_MOR_PP", Entity.MOR_PP));
                if (!string.IsNullOrEmpty(Entity.MOR_Code))
                    sqlParamList.Add(new SqlParameter("@PMR_MOR_CODE", Entity.MOR_Code));
                if (!string.IsNullOrEmpty(Entity.Primary_Code))
                    sqlParamList.Add(new SqlParameter("@PMR_PRIMARY_CODE", Entity.Primary_Code));
                if (!string.IsNullOrEmpty(Entity.Payment_HoW))
                    sqlParamList.Add(new SqlParameter("@PMR_PAYMENT_HOW", Entity.Payment_HoW));
                if (!string.IsNullOrEmpty(Entity.Payment_For))
                    sqlParamList.Add(new SqlParameter("@PMR_PAYMENT_FOR", Entity.Payment_For));
                if (!string.IsNullOrEmpty(Entity.Rent_Month))
                    sqlParamList.Add(new SqlParameter("@PMR_RENT_MONTH", Entity.Rent_Month));
                if (!string.IsNullOrEmpty(Entity.Cash_Bulk))
                    sqlParamList.Add(new SqlParameter("@PMR_CASH_BULK", Entity.Cash_Bulk));
                if (!string.IsNullOrEmpty(Entity.Benefit_Level))
                    sqlParamList.Add(new SqlParameter("@PMR_BENIFIT_LEVEL", Entity.Benefit_Level));
                if (!string.IsNullOrEmpty(Entity.Action_Code))
                    sqlParamList.Add(new SqlParameter("@PMR_ACTION_CODE", Entity.Action_Code));
                if (!string.IsNullOrEmpty(Entity.Fund))
                    sqlParamList.Add(new SqlParameter("@PMR_FUND", Entity.Fund));
                if (!string.IsNullOrEmpty(Entity.Emergency))
                    sqlParamList.Add(new SqlParameter("@PMR_EMERGENCY", Entity.Emergency));
                if (!string.IsNullOrEmpty(Entity.Emer_Pay))
                    sqlParamList.Add(new SqlParameter("@PMR_EMER_PAY", Entity.Emer_Pay));
                if (!string.IsNullOrEmpty(Entity.Account))
                    sqlParamList.Add(new SqlParameter("@PMR_ACCOUNT", Entity.Account));
                if (!string.IsNullOrEmpty(Entity.Divide_Bill))
                    sqlParamList.Add(new SqlParameter("@PMR_DIVIDE_BILL", Entity.Divide_Bill));
                    
                if (!string.IsNullOrEmpty(Entity.Voucher_No))
                    sqlParamList.Add(new SqlParameter("@PMR_VOUCH_NO", Entity.Voucher_No));
                if (!string.IsNullOrEmpty(Entity.Check_No))
                    sqlParamList.Add(new SqlParameter("@PMR_CHECK_NO", Entity.Check_No));
                if (!string.IsNullOrEmpty(Entity.Check_Date))
                    sqlParamList.Add(new SqlParameter("@PMR_CHK_DATE", Entity.Check_Date));
                if (!string.IsNullOrEmpty(Entity.Check_Type))
                    sqlParamList.Add(new SqlParameter("@PMR_CHECK_TYPE", Entity.Check_Type));
                if (!string.IsNullOrEmpty(Entity.Payment_NO1))
                    sqlParamList.Add(new SqlParameter("@PMR_PAYMENT_NO1", Entity.Payment_NO1));
                if (!string.IsNullOrEmpty(Entity.Cashed_Date))
                    sqlParamList.Add(new SqlParameter("@PMR_CASHED_DATE", Entity.Cashed_Date));
                if (!string.IsNullOrEmpty(Entity.Cashed_AMT))
                    sqlParamList.Add(new SqlParameter("@PMR_CASHED_AMT", Entity.Cashed_AMT));
                if (!string.IsNullOrEmpty(Entity.BS))
                    sqlParamList.Add(new SqlParameter("@PMR_BS", Entity.BS));

                if (!string.IsNullOrEmpty(Entity.Notify_Date))
                    sqlParamList.Add(new SqlParameter("@PMR_NOTIFY_DATE", Entity.Notify_Date));
                if (!string.IsNullOrEmpty(Entity.Notify_SEO))
                    sqlParamList.Add(new SqlParameter("@PMR_NOTIFY_SEQ", Entity.Notify_SEO));
                if (!string.IsNullOrEmpty(Entity.Contingency))
                    sqlParamList.Add(new SqlParameter("@PMR_CONTINGENCY", Entity.Contingency));
                if (!string.IsNullOrEmpty(Entity.Meter))
                    sqlParamList.Add(new SqlParameter("@PMR_METER", Entity.Meter));
                if (!string.IsNullOrEmpty(Entity.Payment_NO))
                    sqlParamList.Add(new SqlParameter("@PMR_PAYMENT_NO", Entity.Payment_NO));
                if (!string.IsNullOrEmpty(Entity.Adj_Amount))
                    sqlParamList.Add(new SqlParameter("@PMR_ADJ_AMOUNT", Entity.Adj_Amount));
                if (!string.IsNullOrEmpty(Entity.Reason_Code))
                    sqlParamList.Add(new SqlParameter("@PMR_REASON_CODE", Entity.Reason_Code));
                if (!string.IsNullOrEmpty(Entity.REC_Source))
                    sqlParamList.Add(new SqlParameter("@PMR_REC_SOURCE", Entity.REC_Source));
                if (!string.IsNullOrEmpty(Entity.Bill_RECD))
                    sqlParamList.Add(new SqlParameter("@PMR_BILL_RECD", Entity.Bill_RECD));
                if (!string.IsNullOrEmpty(Entity.MOR_Diff_ppg))
                    sqlParamList.Add(new SqlParameter("@PMR_MOR_DIFF_PPG", Entity.MOR_Diff_ppg));
                if (!string.IsNullOrEmpty(Entity.MOR_Differential))
                    sqlParamList.Add(new SqlParameter("@PMR_MOR_DIFFERENTIAL", Entity.MOR_Differential));
                if (!string.IsNullOrEmpty(Entity.Account_OLD))
                    sqlParamList.Add(new SqlParameter("@PMR_ACCOUNT_OLD", Entity.Account_OLD));

                if (!string.IsNullOrEmpty(Entity.SDWDC))
                    sqlParamList.Add(new SqlParameter("@PMR_SDWDC", Entity.SDWDC));
                if (!string.IsNullOrEmpty(Entity.Add_30001))
                    sqlParamList.Add(new SqlParameter("@PMR_30001_ADD", Entity.Add_30001));
                sqlParamList.Add(new SqlParameter("@PMR_LSTC_OPERATOR", Entity.Lstc_Operator));
                if (!string.IsNullOrEmpty(Entity.ShortFall))
                    sqlParamList.Add(new SqlParameter("@PMR_SHORTFALL", Entity.ShortFall));
                if (!string.IsNullOrEmpty(Entity.Authrization_Type/*.Trim()*/))                                    //added by sudheer on 12/14/2016
                    sqlParamList.Add(new SqlParameter("@PMR_AUTHORIZATION_TYPE", Entity.Authrization_Type));
                if (!string.IsNullOrEmpty(Entity.Account_Number/*.Trim()*/))                                    //added by murali on 12/03/2019
                    sqlParamList.Add(new SqlParameter("@PMR_ACCOUNT_NO", Entity.Account_Number));
                if (!string.IsNullOrEmpty(Entity.Account_Switch/*.Trim()*/))                                    //added by murali on 12/03/2019
                    sqlParamList.Add(new SqlParameter("@PMR_ACCOUNT_SWITCH", Entity.Account_Switch));

                //Added by Sudheer on 12/06/2022
                if (!string.IsNullOrEmpty(Entity.Authr_Type3))
                    sqlParamList.Add(new SqlParameter("@PMR_AUTH_TYPE3", Entity.Authr_Type3));
                if (!string.IsNullOrEmpty(Entity.Authr_Amount3))
                    sqlParamList.Add(new SqlParameter("@PMR_AUTH_AMOUNT3", Entity.Authr_Amount3));


                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@PMR_DATE_ADD", Entity.Date_Add));
                    sqlParamList.Add(new SqlParameter("@PMR_DATE_LSTC", Entity.Lstc_Date));
                    sqlParamList.Add(new SqlParameter("@PMR_ADD_OPERATOR", Entity.Add_Operator));

                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public bool UpdatePAYMENT_Authrs_By_App(PAYMNETEntity Entity, string Authr_Xml, string Lpb_Xml, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@PMR_AGENCY", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@PMR_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@PMR_PROGRAM", Entity.Prog));
                sqlParamList.Add(new SqlParameter("@PMR_YEAR", Entity.Yaer));
                sqlParamList.Add(new SqlParameter("@PMR_APPL_NO", Entity.AppNo));
                if (!string.IsNullOrEmpty(Authr_Xml.Trim()))
                    sqlParamList.Add(new SqlParameter("@PMR_Authr_XML", Authr_Xml));
                sqlParamList.Add(new SqlParameter("@LPB_Delete_XML", Lpb_Xml));
                sqlParamList.Add(new SqlParameter("@PMR_LSTC_OPERATOR", Entity.Lstc_Operator));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdatePAYMENT_Authrs_By_App", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdatePAYMENT_Authrs_By_App_ShortFall(PAYMNETEntity Entity, string Authr_Xml, string Lpb_Xml, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@PMR_AGENCY", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@PMR_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@PMR_PROGRAM", Entity.Prog));
                sqlParamList.Add(new SqlParameter("@PMR_YEAR", Entity.Yaer));
                sqlParamList.Add(new SqlParameter("@PMR_APPL_NO", Entity.AppNo));
                if (!string.IsNullOrEmpty(Authr_Xml.Trim()))
                    sqlParamList.Add(new SqlParameter("@PMR_Authr_XML", Authr_Xml));
                sqlParamList.Add(new SqlParameter("@LPB_Delete_XML", Lpb_Xml));
                sqlParamList.Add(new SqlParameter("@PMR_LSTC_OPERATOR", Entity.Lstc_Operator));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdatePAYMENT_Authrs_By_App_ShortFall", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<TMSRISKEntity> Browse_TMSRISK(TMSRISKEntity Entity, string Opretaion_Mode)
        {
            List<TMSRISKEntity> TMSRISKProfile = new List<TMSRISKEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_TMSRISK_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet TMSRISKData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_TMSRISK]");

                if (TMSRISKData != null && TMSRISKData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TMSRISKData.Tables[0].Rows)
                        TMSRISKProfile.Add(new TMSRISKEntity(row));
                }
            }
            catch (Exception ex)
            { return TMSRISKProfile; }

            return TMSRISKProfile;
        }

        public bool UpdateTMSRISK(TMSRISKEntity Entity, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_TMSRISK_SqlParameters_List(Entity, "Update");
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateTMSRISK", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<SqlParameter> Prepare_TMSRISK_SqlParameters_List(TMSRISKEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@TMSRISK_AGENCY", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@TMSRISK_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@TMSRISK_PROGRAM", Entity.Prog));
                sqlParamList.Add(new SqlParameter("@TMSRISK_YEAR", Entity.Year));
                sqlParamList.Add(new SqlParameter("@TMSRISK_APP_NO", Entity.AppNo));

                sqlParamList.Add(new SqlParameter("@TMSRISK_GMI", Entity.GMI));
                sqlParamList.Add(new SqlParameter("@TMSRISK_GMI_FICA", Entity.GMI_FICA));
                sqlParamList.Add(new SqlParameter("@TMSRISK_GMI_ADJ", Entity.GMI_ADJ));
                sqlParamList.Add(new SqlParameter("@TMSRISK_ASSETS", Entity.Assests));
                sqlParamList.Add(new SqlParameter("@TMSRISK_INC_TOTAL", Entity.Inc_Total));
                sqlParamList.Add(new SqlParameter("@TMSRISK_INC_DISP", Entity.Inc_Disp));
                sqlParamList.Add(new SqlParameter("@TMSRISK_EXP_RENT", Entity.Exp_Rent));
                sqlParamList.Add(new SqlParameter("@TMSRISK_EXP_INSR", Entity.Inc_Insr));
                sqlParamList.Add(new SqlParameter("@TMSRISK_EXP_HOME", Entity.Exp_Home));
                sqlParamList.Add(new SqlParameter("@TMSRISK_EXP_MED", Entity.Exp_Med));
                sqlParamList.Add(new SqlParameter("@TMSRISK_EXP_ALNY", Entity.Exp_Alny));
                sqlParamList.Add(new SqlParameter("@TMSRISK_EXP_DAYCARE", Entity.Exp_DayCare));
                sqlParamList.Add(new SqlParameter("@TMSRISK_EXP_A_FOOD", Entity.Exp_A_Food));
                sqlParamList.Add(new SqlParameter("@TMSRISK_EXP_B_FOOD", Entity.Exp_B_Food));
                sqlParamList.Add(new SqlParameter("@TMSRISK_EXP_C_FOOD", Entity.Exp_C_Food));
                sqlParamList.Add(new SqlParameter("@TMSRISK_EXP_A_UTIL", Entity.Exp_A_Util));
                sqlParamList.Add(new SqlParameter("@TMSRISK_EXP_B_UTIL", Entity.Exp_B_Util));
                sqlParamList.Add(new SqlParameter("@TMSRISK_EXP_C_UTIL", Entity.Exp_C_Util));
                sqlParamList.Add(new SqlParameter("@TMSRISK_EXP_D_UTIL", Entity.Exp_D_Util));
                sqlParamList.Add(new SqlParameter("@TMSRISK_EXP_TEL", Entity.Exp_Tel));
                sqlParamList.Add(new SqlParameter("@TMSRISK_MON_EXP", Entity.Mon_Exp));
                sqlParamList.Add(new SqlParameter("@TMSRISK_HH_COUNT", Entity.HH_Count));
                sqlParamList.Add(new SqlParameter("@TMSRISK_LSTC_OPERATOR", Entity.LSTC_Operator));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@TMSRISK_DATE_ADD", Entity.Add_Date));
                    sqlParamList.Add(new SqlParameter("@TMSRISK_DATE_LSTC", Entity.LSTC_Date));
                    sqlParamList.Add(new SqlParameter("@TMSRISK_ADD_OPERATOR", Entity.Add_Operatop));

                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public bool UpdateCASESUM_Status_From_Enroll(Sum_Referral_Entity Entity, string User, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                
                sqlParamList.Add(new SqlParameter("@Row_Type", "U"));
                sqlParamList.Add(new SqlParameter("@CASESUM_AGENCY", Entity.Agy));
                sqlParamList.Add(new SqlParameter("@CASESUM_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@CASESUM_PROGRAM", Entity.Prog));
                sqlParamList.Add(new SqlParameter("@CASESUM_YEAR", Entity.Year));
                sqlParamList.Add(new SqlParameter("@CASESUM_APP_NO", Entity.App));
                sqlParamList.Add(new SqlParameter("@CASESUM_REF_Hierarchy", Entity.Ref_Hie));
                //sqlParamList.Add(new SqlParameter("@CASESUM_REF_AGENCY", Entity.Ref_Agy));
                //sqlParamList.Add(new SqlParameter("@CASESUM_REF_DEPT", Entity.Ref_Dept));
                //sqlParamList.Add(new SqlParameter("@CASESUM_REF_PROGRAM", Entity.Ref_Prog));

                sqlParamList.Add(new SqlParameter("@CASESUM_STATUS_CODE", Entity.Referred_Status));
                sqlParamList.Add(new SqlParameter("@CASESUM_LSTC_OPERATOR", User));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateCASESUM_Status_From_Enroll", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }


        public List<MOREntity> Browse_MOR(MOREntity Entity, string Opretaion_Mode)
        {
            List<MOREntity> MORProfile = new List<MOREntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASESPM2_SqlParameters_List(Entity, Opretaion_Mode, string.Empty);
                DataSet MORMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_MOR]");

                if (MORMData != null && MORMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MORMData.Tables[0].Rows)
                        MORProfile.Add(new MOREntity(row));
                }
            }
            catch (Exception ex)
            { return MORProfile; }

            return MORProfile;
        }


        //public bool InsertUpdateDelMor(MOREntity Entity)
        //{
        //    bool boolsuccess;

        //    try
        //    {
        //        List<SqlParameter> sqlParamList = new List<SqlParameter>();
        //        if (!string.IsNullOrEmpty(Entity.Date.ToString()))
        //            sqlParamList.Add(new SqlParameter("@Mor_Date", Entity.Date));
        //        if (!string.IsNullOrEmpty(Entity.Ter))
        //            sqlParamList.Add(new SqlParameter("@Mor_Ter", Entity.Ter));
        //        if (!string.IsNullOrEmpty(Entity.Price))
        //            sqlParamList.Add(new SqlParameter("@Mor_Price", Entity.Price));
        //        if (!string.IsNullOrEmpty(Entity.Price_Kerosene))
        //            sqlParamList.Add(new SqlParameter("@Mor_Price_Kerosene", Entity.Price_Kerosene));
        //        //if (!string.IsNullOrEmpty(Entity.date))
        //        //    sqlParamList.Add(new SqlParameter("@Mor_Date_Lstc", Entity.Mor_Date_Lstc));
        //        if (!string.IsNullOrEmpty(Entity.Lstc_Operator))
        //            sqlParamList.Add(new SqlParameter("@Mor_Lstc_Operator", Entity.Lstc_Operator));
        //        if (!string.IsNullOrEmpty(Entity.Rec_Type))
        //            sqlParamList.Add(new SqlParameter("@Mode", Entity.Rec_Type));

        //        boolsuccess = CaseMst.InsertUpdateDelMor(sqlParamList);

        //    }
        //    catch (Exception ex)
        //    {
        //        //
        //        return false;
        //    }

        //    return boolsuccess;
        //}

        public bool UpdateMOR(MOREntity Entity, string Operation_Mode, out string Sql_Reslut_Msg, string RowChange) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_CASESPM2_SqlParameters_List(Entity, Operation_Mode, RowChange);
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.Update_MOR", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<SqlParameter> Prepare_CASESPM2_SqlParameters_List(MOREntity Entity, string Opretaion_Mode, string RowChange)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@MOR_DATE", Entity.Date));
                sqlParamList.Add(new SqlParameter("@MOR_TER", Entity.Ter));
                sqlParamList.Add(new SqlParameter("@MOR_PRICE", Entity.Price));
                sqlParamList.Add(new SqlParameter("@MOR_PRICE_KEROSENE", Entity.Price_Kerosene));
                sqlParamList.Add(new SqlParameter("@MOR_LSTC_OPERATOR", Entity.Lstc_Operator));

                if (Opretaion_Mode == "Browse")
                    sqlParamList.Add(new SqlParameter("@MOR_DATE_LSTC", Entity.Lstc_Date));

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public bool Update_MSTFraud(string Update_Type, string Agy, string Dept, string Prog, string Year, string App, string Fraud_SW, string Fraud_Date, string Poverty, string Classification, string Elig_Date, string Verifier, string User, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@Update_Type", Update_Type));

                sqlParamList.Add(new SqlParameter("@MST_AGENCY", Agy));
                sqlParamList.Add(new SqlParameter("@MST_DEPT", Dept));
                sqlParamList.Add(new SqlParameter("@MST_PROGRAM", Prog));
                sqlParamList.Add(new SqlParameter("@MST_YEAR", Year));
                sqlParamList.Add(new SqlParameter("@MST_APP_NO", App));

                sqlParamList.Add(new SqlParameter("@MST_CB_FRAUD", Fraud_SW));
                sqlParamList.Add(new SqlParameter("@MST_FRAUD_DATE", Fraud_Date));

                sqlParamList.Add(new SqlParameter("@MST_POVERTY", Poverty));
                sqlParamList.Add(new SqlParameter("@MST_CLASSIFICATION", Classification));

                sqlParamList.Add(new SqlParameter("@MST_ELIG_DATE", Elig_Date));
                sqlParamList.Add(new SqlParameter("@MST_VERIFIER", Verifier));

                sqlParamList.Add(new SqlParameter("@MST_LSTC_OPERATOR", User));
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.TMS00081_UpdateMST_Fraud", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateMST_ExpDebit(string Agy, string Dept, string Prog, string Year, string App, string Fam_Seq, string Mst_Db_CC, string Mst_Db_LOAN, string Mst_Db_MED,
                                       string Snp_Vch_Value, string Snp_Oth_Value, string Snp_Oth_Assert, string Snp_Blind, string Snp_Work, string Snp_Med, string Snp_Food, 
                                       string User, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@MST_AGENCY", Agy));
                sqlParamList.Add(new SqlParameter("@MST_DEPT", Dept));
                sqlParamList.Add(new SqlParameter("@MST_PROGRAM", Prog));
                sqlParamList.Add(new SqlParameter("@MST_YEAR", Year));
                sqlParamList.Add(new SqlParameter("@MST_APP_NO", App));
                sqlParamList.Add(new SqlParameter("@SNP_FAMILY_SEQ", Fam_Seq));

                sqlParamList.Add(new SqlParameter("@MST_DEBT_CC", Mst_Db_CC));
                sqlParamList.Add(new SqlParameter("@MST_DEBT_LOANS", Mst_Db_LOAN));
                sqlParamList.Add(new SqlParameter("@MST_DEBT_MED", Mst_Db_MED));

                sqlParamList.Add(new SqlParameter("@SNP_VEHICLE_VALUE", Snp_Vch_Value));
                sqlParamList.Add(new SqlParameter("@SNP_OTHER_VEHICLE_VALUE", Snp_Oth_Value));
                sqlParamList.Add(new SqlParameter("@SNP_OTHER_ASSET_VALUE", Snp_Oth_Assert));

                sqlParamList.Add(new SqlParameter("@SNP_BLIND", Snp_Blind));
                sqlParamList.Add(new SqlParameter("@SNP_ABLE_TO_WORK", Snp_Work));
                sqlParamList.Add(new SqlParameter("@SNP_REC_MEDICARE", Snp_Med));
                sqlParamList.Add(new SqlParameter("@SNP_PURCHASE_FOOD", Snp_Food));

                sqlParamList.Add(new SqlParameter("@MST_LSTC_OPERATOR", User));
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.TMS00310_UpdateMST_ExpDebit", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<TMSHISTEntity> Browse_TMSHIST(TMSHISTEntity Entity, string Opretaion_Mode)
        {
            List<TMSHISTEntity> MORProfile = new List<TMSHISTEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_TMSHIST_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet MORMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_TMSHIST]");

                if (MORMData != null && MORMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MORMData.Tables[0].Rows)
                        MORProfile.Add(new TMSHISTEntity(row));
                }
            }
            catch (Exception ex)
            { return MORProfile; }

            return MORProfile;
        }

        public bool UpdateTMSHIST(TMSHISTEntity Entity, string Operation_Mode, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_TMSHIST_SqlParameters_List(Entity, Operation_Mode);
                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateTMSHIST", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }


        public List<SqlParameter> Prepare_TMSHIST_SqlParameters_List(TMSHISTEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));
                    sqlParamList.Add(new SqlParameter("@TMSHIST_CHANGES", Entity.Changes));
                }

                sqlParamList.Add(new SqlParameter("@TMSHIST_AGENCY", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@TMSHIST_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@TMSHIST_PROGRAM", Entity.Prog));
                sqlParamList.Add(new SqlParameter("@TMSHIST_YEAR", Entity.Year));
                sqlParamList.Add(new SqlParameter("@TMSHIST_APP_NO", Entity.App_No));
                sqlParamList.Add(new SqlParameter("@TMSHIST_SEQ", Entity.Hist_Seq));
                sqlParamList.Add(new SqlParameter("@TMSHIST_TYPE", Entity.Hist_Type));
                sqlParamList.Add(new SqlParameter("@TMSHIST_OPERATION", Entity.Hist_Operation));
                sqlParamList.Add(new SqlParameter("@TMSHIST_LSTC_OPERATOR", Entity.LSTC_Operator));

                if (Opretaion_Mode == "Browse")
                    sqlParamList.Add(new SqlParameter("@TMSHIST_DATE_LSTC", Entity.LSTC_Date));

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public bool INSERTBULKCATELIG(string Agency,string Dept, string Prog, string Year, string App, string FName, string LName,string DOB, string catElig,string FamSeq,string IsFill,string IncCert,string AgeDis,string LiqAsset,string BenLvl,string Award,string Amount,string Asset,string Income,string NoinHH,string EligAsset,string EligInc,string EligFuel,string EligRent,string EligWhter,string Contigency,string CStatus,string UserID,string PrivApp,string Status,string AftVulnerable)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@TMSELIG_AGENCY", Agency));
                sqlParamList.Add(new SqlParameter("@TMSELIG_DEPT", Dept));
                sqlParamList.Add(new SqlParameter("@TMSELIG_PROG", Prog));
                if (Year != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSELIG_YEAR", Year));
                }
                if (App != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSELIG_APP_NO", App));
                }

                if (FamSeq != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSELIG_FAM_SEQ", FamSeq));
                }
                if (FName != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSELIG_FNAME", FName));
                }
                
                if (LName!= string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSELIG_LNAME", LName));
                }
                if (DOB != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSELIG_DOB", DOB));
                }


                if (catElig != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSELIG_CATELIG", catElig));
                }
                if (IsFill!= string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSELIG_IS_FILL", IsFill));
                }
                if (IncCert != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSELIG_AFT_INCCERT", IncCert));
                }
                if (AgeDis != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSELIG_AFT_AGEDIS", AgeDis));
                }
                if (LiqAsset != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSELIG_AFT_LIQASSETS", LiqAsset));
                }
                if (BenLvl != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSELIG_AFT_BENLVL", BenLvl));
                }
                if (Award != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSELIG_AFT_AWARD", Award));
                }
                if (Amount != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPB_AMOUNT", Amount));
                }
                if (Asset != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPB_ASSETS", Asset));
                }
                if (Income!= string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPB_FAP_INCOME", Income));
                }
                if (NoinHH != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPB_FAP_NO_INHH", NoinHH));
                }
                if (EligAsset != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPB_ELIGASSETS", EligAsset));
                }
                if (EligInc != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPB_ELIGINCOME", EligInc));
                }
                if (EligFuel != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPB_ELIGFUEL", EligFuel));
                }
                if (EligRent != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPB_ELIGRENT", EligRent));
                }
                if (EligWhter!= string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPB_ELIGWTHR", EligWhter));
                }
                if (Contigency != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPB_CONTINGENCY", Contigency));
                }
                if (CStatus != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LPB_CERTIFIED_STATUS", CStatus));
                }
                if (Status != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSELIG_STATUS", Status));
                }
                if (AftVulnerable != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSELIG_AFT_VULNERABLE", AftVulnerable));
                }

                sqlParamList.Add(new SqlParameter("@LPB_LSTC_OPERATOR", UserID));
                sqlParamList.Add(new SqlParameter("@PRIVAPP", PrivApp));
                //sqlParamList.Add(new SqlParameter("@Mode", TmsApcnp.Mode));
                boolsuccess = Captain.DatabaseLayer.LiheDb.InsertBULKCATELIG(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public List<WATERCNTLEntity> CapsWatercntlGet(string Year, string BenType, string Award, string SDate)
        {
            List<WATERCNTLEntity> waterCntlDetails = new List<WATERCNTLEntity>();
            try
            {
                DataSet LiheApv = Captain.DatabaseLayer.LiheDb.CAP_WATERCNTL_GET(Year, BenType, Award, SDate);
                if (LiheApv != null && LiheApv.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in LiheApv.Tables[0].Rows)
                    {
                        waterCntlDetails.Add(new WATERCNTLEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return waterCntlDetails;
            }

            return waterCntlDetails;
        }

        public bool CAPSLIHWAPINSUPDEL(LIHWAPEntity Entity, out string stroutSeq)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            string strNewseq = string.Empty;
            bool boolSuccess = false;
            try
            {

                sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@LPW_AGENCY", Entity.LPW_AGENCY));
                sqlParamList.Add(new SqlParameter("@LPW_DEPT", Entity.LPW_DEPT));
                sqlParamList.Add(new SqlParameter("@LPW_PROG", Entity.LPW_PROG));
                sqlParamList.Add(new SqlParameter("@LPW_YEAR", Entity.LPW_YEAR));
                sqlParamList.Add(new SqlParameter("@LPW_APP_NO", Entity.LPW_APP_NO));
                if (Entity.LPW_SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LPW_SEQ", Entity.LPW_SEQ));


                if (!string.IsNullOrEmpty(Entity.LPW_BEN_DATE))
                    sqlParamList.Add(new SqlParameter("@LPW_BEN_DATE", Entity.LPW_BEN_DATE));

                if (!string.IsNullOrEmpty(Entity.LPW_BEN_WORKER))
                    sqlParamList.Add(new SqlParameter("@LPW_BEN_WORKER", Entity.LPW_BEN_WORKER));

                if (!string.IsNullOrEmpty(Entity.LPW_CERTIFIED_STATUS))
                    sqlParamList.Add(new SqlParameter("@LPW_CERTIFIED_STATUS", Entity.LPW_CERTIFIED_STATUS));

                if (!string.IsNullOrEmpty(Entity.LPW_REASON_DENIED))
                    sqlParamList.Add(new SqlParameter("@LPW_REASON_DENIED", Entity.LPW_REASON_DENIED));

                if (!string.IsNullOrEmpty(Entity.LPW_FAM_INCOME))
                    sqlParamList.Add(new SqlParameter("@LPW_FAM_INCOME", Entity.LPW_FAM_INCOME));

                if (!string.IsNullOrEmpty(Entity.LPW_FAM_NO_INHH))
                    sqlParamList.Add(new SqlParameter("@LPW_FAM_NO_INHH", Entity.LPW_FAM_NO_INHH));
                if (!string.IsNullOrEmpty(Entity.LPW_OMB))
                    sqlParamList.Add(new SqlParameter("@LPW_OMB", Entity.LPW_OMB));
                if (!string.IsNullOrEmpty(Entity.LPW_SMI))
                    sqlParamList.Add(new SqlParameter("@LPW_SMI", Entity.LPW_SMI));
                if (!string.IsNullOrEmpty(Entity.LPW_WAT_VENDOR))
                    sqlParamList.Add(new SqlParameter("@LPW_WAT_VENDOR", Entity.LPW_WAT_VENDOR));
                if (!string.IsNullOrEmpty(Entity.LPW_WAT_ACCOUNT_NO))
                    sqlParamList.Add(new SqlParameter("@LPW_WAT_ACCOUNT_NO", Entity.LPW_WAT_ACCOUNT_NO));
                if (!string.IsNullOrEmpty(Entity.LPW_WAT_BILLNAME_TYPE))
                    sqlParamList.Add(new SqlParameter("@LPW_WAT_BILLNAME_TYPE", Entity.LPW_WAT_BILLNAME_TYPE));
                if (!string.IsNullOrEmpty(Entity.LPW_WAT_BILL_LNAME))
                    sqlParamList.Add(new SqlParameter("@LPW_WAT_BILL_LNAME", Entity.LPW_WAT_BILL_LNAME));
                if (!string.IsNullOrEmpty(Entity.LPW_WAT_BILL_FNAME))
                    sqlParamList.Add(new SqlParameter("@LPW_WAT_BILL_FNAME", Entity.LPW_WAT_BILL_FNAME));
                if (!string.IsNullOrEmpty(Entity.LPW_WAT_INVDATE))
                    sqlParamList.Add(new SqlParameter("@LPW_WAT_INVDATE", Entity.LPW_WAT_INVDATE));
                if (!string.IsNullOrEmpty(Entity.LPW_WAT_FREQ_BILL))
                    sqlParamList.Add(new SqlParameter("@LPW_WAT_FREQ_BILL", Entity.LPW_WAT_FREQ_BILL));
                if (!string.IsNullOrEmpty(Entity.LPW_WAT_ARREARS))
                    sqlParamList.Add(new SqlParameter("@LPW_WAT_ARREARS", Entity.LPW_WAT_ARREARS));
                if (!string.IsNullOrEmpty(Entity.LPW_WAT_CURBILL_AMT))
                    sqlParamList.Add(new SqlParameter("@LPW_WAT_CURBILL_AMT", Entity.LPW_WAT_CURBILL_AMT));
                if (!string.IsNullOrEmpty(Entity.LPW_WAT_FEES))
                    sqlParamList.Add(new SqlParameter("@LPW_WAT_FEES", Entity.LPW_WAT_FEES));
                if (!string.IsNullOrEmpty(Entity.LPW_WAT_TOT))
                    sqlParamList.Add(new SqlParameter("@LPW_WAT_TOT", Entity.LPW_WAT_TOT));
                if (!string.IsNullOrEmpty(Entity.LPW_WAT_AWARD))
                    sqlParamList.Add(new SqlParameter("@LPW_WAT_AWARD", Entity.LPW_WAT_AWARD));
                if (!string.IsNullOrEmpty(Entity.LPW_WAT_PAID))
                    sqlParamList.Add(new SqlParameter("@LPW_WAT_PAID", Entity.LPW_WAT_PAID));
                if (!string.IsNullOrEmpty(Entity.LPW_WAT_AWAITING_DOL))
                    sqlParamList.Add(new SqlParameter("@LPW_WAT_AWAITING_DOL", Entity.LPW_WAT_AWAITING_DOL));
                if (!string.IsNullOrEmpty(Entity.LPW_WAT_BEN_TYPE))
                    sqlParamList.Add(new SqlParameter("@LPW_WAT_BEN_TYPE", Entity.LPW_WAT_BEN_TYPE));
                if (!string.IsNullOrEmpty(Entity.LPW_WAT_LEVEL))
                    sqlParamList.Add(new SqlParameter("@LPW_WAT_LEVEL", Entity.LPW_WAT_LEVEL));
                if (!string.IsNullOrEmpty(Entity.LPW_WAT_VER_STAT))
                    sqlParamList.Add(new SqlParameter("@LPW_WAT_VER_STAT", Entity.LPW_WAT_VER_STAT));
                if (!string.IsNullOrEmpty(Entity.LPW_WAT_VERIFIED_WORKER))
                    sqlParamList.Add(new SqlParameter("@LPW_WAT_VERIFIED_WORKER", Entity.LPW_WAT_VERIFIED_WORKER));

                if (!string.IsNullOrEmpty(Entity.LPW_WAT_VERIFIED_DATE))
                    sqlParamList.Add(new SqlParameter("@LPW_WAT_VERIFIED_DATE", Entity.LPW_WAT_VERIFIED_DATE));

                if (!string.IsNullOrEmpty(Entity.LPW_WAT_VERIFIED_DATE_LSTC))
                    sqlParamList.Add(new SqlParameter("@LPW_WAT_VERIFIED_DATE_LSTC", Entity.LPW_WAT_VERIFIED_DATE_LSTC));

                if (!string.IsNullOrEmpty(Entity.LPW_WAT_VERIFIED_LSTC_OPERATOR))
                    sqlParamList.Add(new SqlParameter("@LPW_WAT_VERIFIED_LSTC_OPERATOR", Entity.LPW_WAT_VERIFIED_LSTC_OPERATOR));

                if (!string.IsNullOrEmpty(Entity.LPW_SEW_VENDOR))
                    sqlParamList.Add(new SqlParameter("@LPW_SEW_VENDOR", Entity.LPW_SEW_VENDOR));

                if (!string.IsNullOrEmpty(Entity.LPW_SEW_ACCOUNT_NO))
                    sqlParamList.Add(new SqlParameter("@LPW_SEW_ACCOUNT_NO", Entity.LPW_SEW_ACCOUNT_NO));

                if (!string.IsNullOrEmpty(Entity.LPW_SEW_BILLNAME_TYPE))
                    sqlParamList.Add(new SqlParameter("@LPW_SEW_BILLNAME_TYPE", Entity.LPW_SEW_BILLNAME_TYPE));

                if (!string.IsNullOrEmpty(Entity.LPW_SEW_BILL_LNAME))
                    sqlParamList.Add(new SqlParameter("@LPW_SEW_BILL_LNAME", Entity.LPW_SEW_BILL_LNAME));

                if (!string.IsNullOrEmpty(Entity.LPW_SEW_BILL_FNAME))
                    sqlParamList.Add(new SqlParameter("@LPW_SEW_BILL_FNAME", Entity.LPW_SEW_BILL_FNAME));

                if (!string.IsNullOrEmpty(Entity.LPW_SEW_INVDATE))
                    sqlParamList.Add(new SqlParameter("@LPW_SEW_INVDATE", Entity.LPW_SEW_INVDATE));

                if (!string.IsNullOrEmpty(Entity.LPW_SEW_FREQ_BILL))
                    sqlParamList.Add(new SqlParameter("@LPW_SEW_FREQ_BILL", Entity.LPW_SEW_FREQ_BILL));

                if (!string.IsNullOrEmpty(Entity.LPW_SEW_ARREARS))
                    sqlParamList.Add(new SqlParameter("@LPW_SEW_ARREARS", Entity.LPW_SEW_ARREARS));

                if (!string.IsNullOrEmpty(Entity.LPW_SEW_CURBILL_AMT))
                    sqlParamList.Add(new SqlParameter("@LPW_SEW_CURBILL_AMT", Entity.LPW_SEW_CURBILL_AMT));

                if (!string.IsNullOrEmpty(Entity.LPW_SEW_FEES))
                    sqlParamList.Add(new SqlParameter("@LPW_SEW_FEES", Entity.LPW_SEW_FEES));

                if (!string.IsNullOrEmpty(Entity.LPW_SEW_TOT))
                    sqlParamList.Add(new SqlParameter("@LPW_SEW_TOT", Entity.LPW_SEW_TOT));

                if (!string.IsNullOrEmpty(Entity.LPW_SEW_AWARD))
                    sqlParamList.Add(new SqlParameter("@LPW_SEW_AWARD", Entity.LPW_SEW_AWARD));

                if (!string.IsNullOrEmpty(Entity.LPW_SEW_PAID))
                    sqlParamList.Add(new SqlParameter("@LPW_SEW_PAID", Entity.LPW_SEW_PAID));

                if (!string.IsNullOrEmpty(Entity.LPW_SEW_AWAITING_DOL))
                    sqlParamList.Add(new SqlParameter("@LPW_SEW_AWAITING_DOL", Entity.LPW_SEW_AWAITING_DOL));

                if (!string.IsNullOrEmpty(Entity.LPW_SEW_BEN_TYPE))
                    sqlParamList.Add(new SqlParameter("@LPW_SEW_BEN_TYPE", Entity.LPW_SEW_BEN_TYPE));

                if (!string.IsNullOrEmpty(Entity.LPW_SEW_LEVEL))
                    sqlParamList.Add(new SqlParameter("@LPW_SEW_LEVEL", Entity.LPW_SEW_LEVEL));

                if (!string.IsNullOrEmpty(Entity.LPW_SEW_VER_STAT))
                    sqlParamList.Add(new SqlParameter("@LPW_SEW_VER_STAT", Entity.LPW_SEW_VER_STAT));

                if (!string.IsNullOrEmpty(Entity.LPW_SEW_VERIFIED_WORKER))
                    sqlParamList.Add(new SqlParameter("@LPW_SEW_VERIFIED_WORKER", Entity.LPW_SEW_VERIFIED_WORKER));

                if (!string.IsNullOrEmpty(Entity.LPW_SEW_VERIFIED_DATE))
                    sqlParamList.Add(new SqlParameter("@LPW_SEW_VERIFIED_DATE", Entity.LPW_SEW_VERIFIED_DATE));

                if (!string.IsNullOrEmpty(Entity.LPW_SEW_VERIFIED_DATE_LSTC))
                    sqlParamList.Add(new SqlParameter("@LPW_SEW_VERIFIED_DATE_LSTC", Entity.LPW_SEW_VERIFIED_DATE_LSTC));


                if (!string.IsNullOrEmpty(Entity.LPW_SEW_VERIFIED_LSTC_OPERATOR))
                    sqlParamList.Add(new SqlParameter("@LPW_SEW_VERIFIED_LSTC_OPERATOR", Entity.LPW_SEW_VERIFIED_LSTC_OPERATOR));



                if (!string.IsNullOrEmpty(Entity.LPW_ADD_OPERATOR))
                    sqlParamList.Add(new SqlParameter("@LPW_ADD_OPERATOR", Entity.LPW_ADD_OPERATOR));



                if (!string.IsNullOrEmpty(Entity.LPW_LSTC_OPERATOR))
                    sqlParamList.Add(new SqlParameter("@LPW_LSTC_OPERATOR", Entity.LPW_LSTC_OPERATOR));

                //sqlParamList.Add(new SqlParameter("@LPW_TYPE", Entity.LPW_TYPE));
                //if (!string.IsNullOrEmpty(Entity.LPW_AMOUNT))
                //    sqlParamList.Add(new SqlParameter("@LPW_AMOUNT", Entity.LPW_AMOUNT));
                //if (!string.IsNullOrEmpty(Entity.LPW_PAID))
                //    sqlParamList.Add(new SqlParameter("@LPW_PAID", Entity.LPW_PAID));
                //if (!string.IsNullOrEmpty(Entity.LPW_AWAITING_DOL))
                //    sqlParamList.Add(new SqlParameter("@LPW_AWAITING_DOL", Entity.LPW_AWAITING_DOL));
                //if (!string.IsNullOrEmpty(Entity.LPW_ASSETS))
                //    sqlParamList.Add(new SqlParameter("@LPW_ASSETS", Entity.LPW_ASSETS));
                //if (!string.IsNullOrEmpty(Entity.LPW_DATE))
                //    sqlParamList.Add(new SqlParameter("@LPW_DATE", Entity.LPW_DATE));
                //if (!string.IsNullOrEmpty(Entity.LPW_CASEWORKER))
                //    sqlParamList.Add(new SqlParameter("@LPW_CASEWORKER", Entity.LPW_CASEWORKER));
                ////if (!string.IsNullOrEmpty(Entity.LPW_NOTICE_DATE))
                ////    sqlParamList.Add(new SqlParameter("@LPW_NOTICE_DATE", Entity.LPW_NOTICE_DATE));
                ////if (!string.IsNullOrEmpty(Entity.LPW_RESEND))
                ////    sqlParamList.Add(new SqlParameter("@LPW_RESEND", Entity.LPW_RESEND));
                ////if (!string.IsNullOrEmpty(Entity.LPW_DENY))
                ////    sqlParamList.Add(new SqlParameter("@LPW_DENY", Entity.LPW_DENY));
                ////if (!string.IsNullOrEmpty(Entity.LPW_AGEDIS))
                ////    sqlParamList.Add(new SqlParameter("@LPW_AGEDIS", Entity.LPW_AGEDIS));
                ////if (!string.IsNullOrEmpty(Entity.LPW_SOURCE))
                ////    sqlParamList.Add(new SqlParameter("@LPW_SOURCE", Entity.LPW_SOURCE));
                //if (!string.IsNullOrEmpty(Entity.LPW_INC))
                //    sqlParamList.Add(new SqlParameter("@LPW_INC", Entity.LPW_INC));
                ////if (!string.IsNullOrEmpty(Entity.LPW_RENTER))
                ////    sqlParamList.Add(new SqlParameter("@LPW_RENTER", Entity.LPW_RENTER));
                //if (!string.IsNullOrEmpty(Entity.LPW_FAP_INCOME))
                //    sqlParamList.Add(new SqlParameter("@LPW_FAP_INCOME", Entity.LPW_FAP_INCOME));
                //if (!string.IsNullOrEmpty(Entity.LPW_FAP_NO_INHH))
                //    sqlParamList.Add(new SqlParameter("@LPW_FAP_NO_INHH", Entity.LPW_FAP_NO_INHH));
                ////if (!string.IsNullOrEmpty(Entity.LPW_REASON_DENIED))
                ////    sqlParamList.Add(new SqlParameter("@LPW_REASON_DENIED", Entity.LPW_REASON_DENIED));
                ////if (!string.IsNullOrEmpty(Entity.LPW_ACTION_CODE))
                ////    sqlParamList.Add(new SqlParameter("@LPW_ACTION_CODE", Entity.LPW_ACTION_CODE));
                ////if (!string.IsNullOrEmpty(Entity.LPW_AU))
                ////    sqlParamList.Add(new SqlParameter("@LPW_AU", Entity.LPW_AU));
                ////if (!string.IsNullOrEmpty(Entity.LPW_PAYMENT_HOW))
                ////    sqlParamList.Add(new SqlParameter("@LPW_PAYMENT_HOW", Entity.LPW_PAYMENT_HOW));
                ////if (!string.IsNullOrEmpty(Entity.LPW_DISABLE))
                ////    sqlParamList.Add(new SqlParameter("@LPW_DISABLE", Entity.LPW_DISABLE));
                ////if (!string.IsNullOrEmpty(Entity.LPW_INCWORKER))
                ////    sqlParamList.Add(new SqlParameter("@LPW_INCWORKER", Entity.LPW_INCWORKER));
                ////if (!string.IsNullOrEmpty(Entity.LPW_INCDATE))
                ////    sqlParamList.Add(new SqlParameter("@LPW_INCDATE", Entity.LPW_INCDATE));

                //if (!string.IsNullOrEmpty(Entity.LPW_BENEFIT_LEVEL))
                //    sqlParamList.Add(new SqlParameter("@LPW_BENEFIT_LEVEL", Entity.LPW_BENEFIT_LEVEL));
                //if (!string.IsNullOrEmpty(Entity.LPW_MOD))
                //    sqlParamList.Add(new SqlParameter("@LPW_MOD", Entity.LPW_MOD));
                ////if (!string.IsNullOrEmpty(Entity.LPW_ELIGASSETS))
                ////    sqlParamList.Add(new SqlParameter("@LPW_ELIGASSETS", Entity.LPW_ELIGASSETS));
                ////if (!string.IsNullOrEmpty(Entity.LPW_ELIGASSETS1))
                ////    sqlParamList.Add(new SqlParameter("@LPW_ELIGASSETS1", Entity.LPW_ELIGASSETS1));
                ////if (!string.IsNullOrEmpty(Entity.LPW_ELIGINCOME))
                ////    sqlParamList.Add(new SqlParameter("@LPW_ELIGINCOME", Entity.LPW_ELIGINCOME));
                ////if (!string.IsNullOrEmpty(Entity.LPW_ELIGFUEL))
                ////    sqlParamList.Add(new SqlParameter("@LPW_ELIGFUEL", Entity.LPW_ELIGFUEL));
                ////if (!string.IsNullOrEmpty(Entity.LPW_ELIGRENT))
                ////    sqlParamList.Add(new SqlParameter("@LPW_ELIGRENT", Entity.LPW_ELIGRENT));
                ////if (!string.IsNullOrEmpty(Entity.LPW_ELIGWTHR))
                ////    sqlParamList.Add(new SqlParameter("@LPW_ELIGWTHR", Entity.LPW_ELIGWTHR));
                ////if (!string.IsNullOrEmpty(Entity.LPW_CONTINGENCY))
                ////    sqlParamList.Add(new SqlParameter("@LPW_CONTINGENCY", Entity.LPW_CONTINGENCY));
                ////if (!string.IsNullOrEmpty(Entity.LPW_REDUCE_ELIG))
                ////    sqlParamList.Add(new SqlParameter("@LPW_REDUCE_ELIG", Entity.LPW_REDUCE_ELIG));
                //if (!string.IsNullOrEmpty(Entity.LPW_CERTIFIED_STATUS))
                //    sqlParamList.Add(new SqlParameter("@LPW_CERTIFIED_STATUS", Entity.LPW_CERTIFIED_STATUS));
                ////if (!string.IsNullOrEmpty(Entity.LPW_DATE_RENOTIFY))
                ////    sqlParamList.Add(new SqlParameter("@LPW_DATE_RENOTIFY", Entity.LPW_DATE_RENOTIFY));
                ////if (!string.IsNullOrEmpty(Entity.LPW_LETTER_NO))
                ////    sqlParamList.Add(new SqlParameter("@LPW_LETTER_NO", Entity.LPW_LETTER_NO));
                ////if (!string.IsNullOrEmpty(Entity.LPW_PAY_NOTICE_DATE))
                ////    sqlParamList.Add(new SqlParameter("@LPW_PAY_NOTICE_DATE", Entity.LPW_PAY_NOTICE_DATE));
                ////if (!string.IsNullOrEmpty(Entity.LPW_FUND_CODE))
                ////    sqlParamList.Add(new SqlParameter("@LPW_FUND_CODE", Entity.LPW_FUND_CODE));

                ////if (!string.IsNullOrEmpty(Entity.LPW_SW1))
                ////    sqlParamList.Add(new SqlParameter("@LPW_SW1", Entity.LPW_SW1));
                ////if (!string.IsNullOrEmpty(Entity.LPW_SW2))
                ////    sqlParamList.Add(new SqlParameter("@LPW_SW2", Entity.LPW_SW2));
                ////if (!string.IsNullOrEmpty(Entity.LPW_SW3))
                ////    sqlParamList.Add(new SqlParameter("@LPW_SW3", Entity.LPW_SW3));
                ////if (!string.IsNullOrEmpty(Entity.LPW_SW4))
                ////    sqlParamList.Add(new SqlParameter("@LPW_SW4", Entity.LPW_SW4));
                ////if (!string.IsNullOrEmpty(Entity.LPW_SW5))
                ////    sqlParamList.Add(new SqlParameter("@LPW_SW5", Entity.LPW_SW5));
                ////if (!string.IsNullOrEmpty(Entity.LPW_SW6))
                ////    sqlParamList.Add(new SqlParameter("@LPW_SW6", Entity.LPW_SW6));
                ////if (!string.IsNullOrEmpty(Entity.LPW_SW7))
                ////    sqlParamList.Add(new SqlParameter("@LPW_SW7", Entity.LPW_SW7));
                ////if (!string.IsNullOrEmpty(Entity.LPW_SW8))
                ////    sqlParamList.Add(new SqlParameter("@LPW_SW8", Entity.LPW_SW8));

                ////if (!string.IsNullOrEmpty(Entity.LPW_SSN_SW))
                ////    sqlParamList.Add(new SqlParameter("@LPW_SSN_SW", Entity.LPW_SSN_SW));
                ////if (!string.IsNullOrEmpty(Entity.LPW_BATCHNO))
                ////    sqlParamList.Add(new SqlParameter("@LPW_BATCHNO", Entity.LPW_BATCHNO));
                ////if (!string.IsNullOrEmpty(Entity.LPW_REMAINING))
                ////    sqlParamList.Add(new SqlParameter("@LPW_REMAINING", Entity.LPW_REMAINING));
                ////if (!string.IsNullOrEmpty(Entity.LPW_CONS_GALLONS))
                ////    sqlParamList.Add(new SqlParameter("@LPW_CONS_GALLONS", Entity.LPW_CONS_GALLONS));
                ////if (!string.IsNullOrEmpty(Entity.LPW_CONS_DOLLARS))
                ////    sqlParamList.Add(new SqlParameter("@LPW_CONS_DOLLARS", Entity.LPW_CONS_DOLLARS));
                ////if (!string.IsNullOrEmpty(Entity.LPW_CONS_MONTHS))
                ////    sqlParamList.Add(new SqlParameter("@LPW_CONS_MONTHS", Entity.LPW_CONS_MONTHS));
                ////if (!string.IsNullOrEmpty(Entity.LPW_CONS_GALLONS_NY))
                ////    sqlParamList.Add(new SqlParameter("@LPW_CONS_GALLONS_NY", Entity.LPW_CONS_GALLONS_NY));
                ////if (!string.IsNullOrEmpty(Entity.LPW_CONS_DOLLARS_NY))
                ////    sqlParamList.Add(new SqlParameter("@LPW_CONS_DOLLARS_NY", Entity.LPW_CONS_DOLLARS_NY));
                ////if (!string.IsNullOrEmpty(Entity.LPW_CONS_MONTHS_NY))
                ////    sqlParamList.Add(new SqlParameter("@LPW_CONS_MONTHS_NY", Entity.LPW_CONS_MONTHS_NY));
                ////if (!string.IsNullOrEmpty(Entity.LPW_WEATH_INTEREST))
                ////    sqlParamList.Add(new SqlParameter("@LPW_WEATH_INTEREST", Entity.LPW_WEATH_INTEREST));
                ////if (!string.IsNullOrEmpty(Entity.LPW_WEATH_NOROOMS))
                ////    sqlParamList.Add(new SqlParameter("@LPW_WEATH_NOROOMS", Entity.LPW_WEATH_NOROOMS));
                ////if (!string.IsNullOrEmpty(Entity.LPW_WEATH_SHARE))
                ////    sqlParamList.Add(new SqlParameter("@LPW_WEATH_SHARE", Entity.LPW_WEATH_SHARE));
                ////if (!string.IsNullOrEmpty(Entity.LPW_WEATH_BEFORE))
                ////    sqlParamList.Add(new SqlParameter("@LPW_WEATH_BEFORE", Entity.LPW_WEATH_BEFORE));
                ////if (!string.IsNullOrEmpty(Entity.LPW_OVERRIDE))
                ////    sqlParamList.Add(new SqlParameter("@LPW_OVERRIDE", Entity.LPW_OVERRIDE));
                ////if (!string.IsNullOrEmpty(Entity.LPW_APP_APPLIED))
                ////    sqlParamList.Add(new SqlParameter("@LPW_APP_APPLIED", Entity.LPW_APP_APPLIED));
                ////if (!string.IsNullOrEmpty(Entity.LPW_INCOMP_ASSET))
                ////    sqlParamList.Add(new SqlParameter("@LPW_INCOMP_ASSET", Entity.LPW_INCOMP_ASSET));

                ////if (!string.IsNullOrEmpty(Entity.LPW_INCOMP_SSN))
                ////    sqlParamList.Add(new SqlParameter("@LPW_INCOMP_SSN", Entity.LPW_INCOMP_SSN));
                ////if (!string.IsNullOrEmpty(Entity.LPW_INCOMP_RENT))
                ////    sqlParamList.Add(new SqlParameter("@LPW_INCOMP_RENT", Entity.LPW_INCOMP_RENT));
                ////if (!string.IsNullOrEmpty(Entity.LPW_INCOMP_UTIL))
                ////    sqlParamList.Add(new SqlParameter("@LPW_INCOMP_UTIL", Entity.LPW_INCOMP_UTIL));
                ////if (!string.IsNullOrEmpty(Entity.LPW_CATEG_ELIG))
                ////    sqlParamList.Add(new SqlParameter("@LPW_CATEG_ELIG", Entity.LPW_CATEG_ELIG));
                ////if (!string.IsNullOrEmpty(Entity.LPW_TKT_BALANCE))
                ////    sqlParamList.Add(new SqlParameter("@LPW_TKT_BALANCE", Entity.LPW_TKT_BALANCE));

                ////if (!string.IsNullOrEmpty(Entity.LPW_HH_FS_BENIFIT))
                ////    sqlParamList.Add(new SqlParameter("@LPW_HH_FS_BENIFIT", Entity.LPW_HH_FS_BENIFIT));
                ////if (!string.IsNullOrEmpty(Entity.LPW_DSS_ADFC))
                ////    sqlParamList.Add(new SqlParameter("@LPW_DSS_ADFC", Entity.LPW_DSS_ADFC));

                ////if (!string.IsNullOrEmpty(Entity.LPW_WAP_DATE))
                ////    sqlParamList.Add(new SqlParameter("@LPW_WAP_DATE", Entity.LPW_WAP_DATE));
                ////if (!string.IsNullOrEmpty(Entity.LPW_WAP_OPERATOR))
                ////    sqlParamList.Add(new SqlParameter("@LPW_WAP_OPERATOR", Entity.LPW_WAP_OPERATOR));


                ////if (!string.IsNullOrEmpty(Entity.LPW_LIQUID_ASSET))
                ////    sqlParamList.Add(new SqlParameter("@LPW_LIQUID_ASSET", Entity.LPW_LIQUID_ASSET));

                ////if (!string.IsNullOrEmpty(Entity.LPW_RISK))
                ////    sqlParamList.Add(new SqlParameter("@LPW_RISK", Entity.LPW_RISK));

                ////if (!string.IsNullOrEmpty(Entity.LPW_DENIED_REASON))
                ////    sqlParamList.Add(new SqlParameter("@LPW_DENIED_REASON", Entity.LPW_DENIED_REASON));

                //if (!string.IsNullOrEmpty(Entity.LPW_ADD_OPERATOR))
                //    sqlParamList.Add(new SqlParameter("@LPW_ADD_OPERATOR", Entity.LPW_ADD_OPERATOR));

                //if (!string.IsNullOrEmpty(Entity.LPW_LSTC_OPERATOR))
                //    sqlParamList.Add(new SqlParameter("@LPW_LSTC_OPERATOR", Entity.LPW_LSTC_OPERATOR));

                //if (!string.IsNullOrEmpty(Entity.LPW_WATSEW_VENDOR))
                //    sqlParamList.Add(new SqlParameter("@LPW_WATSEW_VENDOR", Entity.LPW_WATSEW_VENDOR));

                //if (!string.IsNullOrEmpty(Entity.LPW_WATSEW_ACCOUNT_NO))
                //    sqlParamList.Add(new SqlParameter("@LPW_WATSEW_ACCOUNT_NO", Entity.LPW_WATSEW_ACCOUNT_NO));

                //if (!string.IsNullOrEmpty(Entity.LPW_WATSEW_BILL_LNAME))
                //    sqlParamList.Add(new SqlParameter("@LPW_WATSEW_BILL_LNAME", Entity.LPW_WATSEW_BILL_LNAME));

                //if (!string.IsNullOrEmpty(Entity.LPW_WATSEW_BILL_FNAME))
                //    sqlParamList.Add(new SqlParameter("@LPW_WATSEW_BILL_FNAME", Entity.LPW_WATSEW_BILL_FNAME));

                //if (!string.IsNullOrEmpty(Entity.LPW_WATSEW_BILLNAME_TYPE))
                //    sqlParamList.Add(new SqlParameter("@LPW_WATSEW_BILLNAME_TYPE", Entity.LPW_WATSEW_BILLNAME_TYPE));

                //if (!string.IsNullOrEmpty(Entity.LPW_WATSEW_ARREARS))
                //    sqlParamList.Add(new SqlParameter("@LPW_WATSEW_ARREARS", Entity.LPW_WATSEW_ARREARS));

                //if (!string.IsNullOrEmpty(Entity.LPW_WATSEW_CURRENTBILL))
                //    sqlParamList.Add(new SqlParameter("@LPW_WATSEW_CURRENTBILL", Entity.LPW_WATSEW_CURRENTBILL));

                //if (!string.IsNullOrEmpty(Entity.LPW_WATSEW_FEES))
                //    sqlParamList.Add(new SqlParameter("@LPW_WATSEW_FEES", Entity.LPW_WATSEW_FEES));

                //if (!string.IsNullOrEmpty(Entity.LPW_WATSEW_TOT))
                //    sqlParamList.Add(new SqlParameter("@LPW_WATSEW_TOT", Entity.LPW_WATSEW_TOT));

                //if (!string.IsNullOrEmpty(Entity.LPW_WATSEW_STATUS))
                //    sqlParamList.Add(new SqlParameter("@LPW_WATSEW_STATUS", Entity.LPW_WATSEW_STATUS));

                //if (!string.IsNullOrEmpty(Entity.LPW_WATSEW_TYPE))
                //    sqlParamList.Add(new SqlParameter("@LPW_WATSEW_TYPE", Entity.LPW_WATSEW_TYPE));



                ////if (!string.IsNullOrEmpty(Entity.LPW_SEW_BILL_FNAME))
                ////    sqlParamList.Add(new SqlParameter("@LPW_SEW_BILL_FNAME", Entity.LPW_SEW_BILL_FNAME));

                ////if (!string.IsNullOrEmpty(Entity.LPW_SEW_BILLNAME_TYPE))
                ////    sqlParamList.Add(new SqlParameter("@LPW_SEW_BILLNAME_TYPE", Entity.LPW_SEW_BILLNAME_TYPE));

                ////if (!string.IsNullOrEmpty(Entity.LPW_SEW_ARREARS))
                ////    sqlParamList.Add(new SqlParameter("@LPW_SEW_ARREARS", Entity.LPW_SEW_ARREARS));

                ////if (!string.IsNullOrEmpty(Entity.LPW_SEW_CURRENTBILL))
                ////    sqlParamList.Add(new SqlParameter("@LPW_SEW_CURRENTBILL", Entity.LPW_SEW_CURRENTBILL));

                //if (!string.IsNullOrEmpty(Entity.LPW_WATSEW_VERIFIED_DATE))
                //    sqlParamList.Add(new SqlParameter("@LPW_WATSEW_VERIFIED_DATE", Entity.LPW_WATSEW_VERIFIED_DATE));

                //if (!string.IsNullOrEmpty(Entity.LPW_WATSEW_VERIFIED_WORKER))
                //    sqlParamList.Add(new SqlParameter("@LPW_WATSEW_VERIFIED_WORKER", Entity.LPW_WATSEW_VERIFIED_WORKER));

                ////if (!string.IsNullOrEmpty(Entity.LPW_WATSEW_VERIFIED_DATE_LSTC))
                ////    sqlParamList.Add(new SqlParameter("@LPW_WATSEW_VERIFIED_DATE_LSTC", Entity.LPW_WATSEW_VERIFIED_DATE_LSTC));

                ////if (!string.IsNullOrEmpty(Entity.LPW_WATSEW_VERIFIED_LSTC_OPERATOR))
                ////    sqlParamList.Add(new SqlParameter("@LPW_WATSEW_VERIFIED_LSTC_OPERATOR", Entity.LPW_WATSEW_VERIFIED_LSTC_OPERATOR));

                //if (!string.IsNullOrEmpty(Entity.LPW_OMB))
                //    sqlParamList.Add(new SqlParameter("@LPW_OMB", Entity.LPW_OMB));

                //if (!string.IsNullOrEmpty(Entity.LPW_SMI))
                //    sqlParamList.Add(new SqlParameter("@LPW_SMI", Entity.LPW_SMI));

                //if (!string.IsNullOrEmpty(Entity.LPW_RECERT))
                //    sqlParamList.Add(new SqlParameter("@LPW_RECERT", Entity.LPW_RECERT));

                //if (!string.IsNullOrEmpty(Entity.LPW_WATSEW_INVDATE))
                //    sqlParamList.Add(new SqlParameter("@LPW_WATSEW_INVDATE", Entity.LPW_WATSEW_INVDATE));



                SqlParameter Sqlseq = new SqlParameter("@LPW_SEQOUT", SqlDbType.VarChar, 10);
                Sqlseq.Value = Entity.LPW_SEQ;
                Sqlseq.Direction = ParameterDirection.Output;
                sqlParamList.Add(Sqlseq);


                boolSuccess = Captain.DatabaseLayer.LiheDb.CAPS_LIHWAP_INSUPDEL(sqlParamList);
                strNewseq = Sqlseq.Value.ToString();
                stroutSeq = strNewseq;
            }

            catch (Exception ex)
            {
                stroutSeq = string.Empty;
                return false;
            }

            return boolSuccess;
        }

        public List<LIHWAPEntity> CAPSLIHWAPGET(string Agency, string Dept, string program, string Year, string Applno, string seq, string strType, string strDate)
        {
            List<LIHWAPEntity> lihwapGetdetails = new List<LIHWAPEntity>();
            try
            {
                DataSet LiheApv = Captain.DatabaseLayer.LiheDb.CAPS_LIHWAP_GET(Agency, Dept, program, Year, Applno, seq, strType, strDate);

                if (LiheApv != null && LiheApv.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in LiheApv.Tables[0].Rows)
                    {
                        lihwapGetdetails.Add(new LIHWAPEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return lihwapGetdetails;
            }

            return lihwapGetdetails;
        }
    }
}
