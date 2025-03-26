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
    public class CaseSumData
    {
        public CaseSumData(CaptainModel model)
        {
            Model = model;
        }

        #region Properties

        public CaptainModel Model { get; set; }

        public string UserId { get; set; }

        #endregion

        public bool InsertUpdateDelCaseSum(CaseSumEntity SumEntity)
        {
            bool boolSuccess = false;
            string strNewApplNo = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@CASESUM_AGENCY", SumEntity.CaseSumAgency));
                sqlParamList.Add(new SqlParameter("@CASESUM_DEPT", SumEntity.CaseSumDept));
                sqlParamList.Add(new SqlParameter("@CASESUM_PROGRAM", SumEntity.CaseSumProgram));
                sqlParamList.Add(new SqlParameter("@CASESUM_YEAR", SumEntity.CaseSumYear));
                sqlParamList.Add(new SqlParameter("@CASESUM_APP_NO", SumEntity.CaseSumApplNo));

                if (SumEntity.CaseSumXml != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CaseSum", SumEntity.CaseSumXml));

                if (SumEntity.CaseSumRefHierachy != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CASESUM_REF_HIERARCHY", SumEntity.CaseSumRefHierachy));
               



                if (SumEntity.CaseSumRefYear != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CASESUM_REF_YEAR", SumEntity.CaseSumRefYear));

                //if (SumEntity.CaseSumRefApplNo != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASESUM_REF_APP_NO", SumEntity.CaseSumRefApplNo));

                //if (SumEntity.CaseSumReferBy != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASESUM_REFER_BY", SumEntity.CaseSumReferBy));

                //if (SumEntity.CaseSumReferDate != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASESUM_REFERDATE", SumEntity.CaseSumReferDate));

                //if (SumEntity.CaseSumContactKey != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASESUM_CONTACT_KEY", SumEntity.CaseSumContactKey));

                //if (SumEntity.CaseSumPoints != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASESUM_POINTS", SumEntity.CaseSumPoints));

                //if (SumEntity.CaseSumStatusCode != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASESUM_STATUS_CODE", SumEntity.CaseSumStatusCode));

                //if (SumEntity.CaseSumStatusDate != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASESUM_STATUS_DATE", SumEntity.CaseSumStatusDate));

                //if (SumEntity.CaseSumNotInterested != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASESUM_NOT_INTERESTED", SumEntity.CaseSumNotInterested));

                //if (SumEntity.CaseSumNotInteresDate != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASESUM_NOT_INTEREST_DATE", SumEntity.CaseSumNotInteresDate));

                //if (SumEntity.CaseSumNotInterestBy != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASESUM_NOT_INTEREST_BY", SumEntity.CaseSumNotInterestBy));

                //if (SumEntity.CaseSumNotInterestReason != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASESUM_NOT_INTEREST_REASON", SumEntity.CaseSumNotInterestReason));


                //if (SumEntity.AddOperator != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASESUM_ADD_OPERATOR", SumEntity.AddOperator));

                //if (SumEntity.LstcOperator != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASESUM_LSTC_OPERATOR", SumEntity.LstcOperator));             

                sqlParamList.Add(new SqlParameter("@Mode", SumEntity.Mode));

                // SqlParameter  sqlApplNo = new SqlParameter("@MST_APP_NO_OUT",SqlDbType.VarChar,10);
                //   sqlApplNo.Value = SumEntity.ApplNo;
                //    sqlApplNo.Direction = ParameterDirection.Output;
                //    sqlParamList.Add(sqlApplNo);


                ////if (MstEntity.ApplNo != string.Empty)
                ////    sqlParamList.Add(new SqlParameter("@MST_APP_NO", MstEntity.ApplNo));

                boolSuccess = CaseSum.InsertUpdateDelCaseSum(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }


            // strApplNo = strNewApplNo;
            return boolSuccess;
        }

        public List<CaseSumEntity> GetCaseSumDetails(string agency, string dep, string program, string year, string app, string seq)
        {
            List<CaseSumEntity> CaseSumDetails = new List<CaseSumEntity>();
            try
            {
                DataSet CaseSnp = Captain.DatabaseLayer.CaseSum.GetCaseSumDetails(agency, dep, program, year, app, seq);
                if (CaseSnp != null && CaseSnp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnp.Tables[0].Rows)
                    {
                        CaseSumDetails.Add(new CaseSumEntity(row, "CASEDEPCONT"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSumDetails;
            }

            return CaseSumDetails;
        }

        public List<CaseSumEntity> GetCaseSumSubDetails(string ssnno, string key, string refKey, string ApplicationNo,string FName,string DOB,string Clid)
        {
            List<CaseSumEntity> CaseSumDetails = new List<CaseSumEntity>();
            try
            {
                DataSet CaseSnp = Captain.DatabaseLayer.CaseSum.GetCaseSumSubDetails(ssnno, key, refKey,ApplicationNo, FName,DOB, Clid);
                if (CaseSnp != null && CaseSnp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnp.Tables[0].Rows)
                    {
                        CaseSumDetails.Add(new CaseSumEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSumDetails;
            }

            return CaseSumDetails;
        }

        public List<DepContactEntity> GetCASEDEPContacts()
        {
            List<DepContactEntity> CaseDepProfile = new List<DepContactEntity>();
            try
            {
                DataSet CaseDepData = HierarchyPlusProgram.GetCASEDEPContacts(string.Empty, string.Empty, string.Empty);
                if (CaseDepData != null && CaseDepData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseDepData.Tables[0].Rows)
                    {
                        CaseDepProfile.Add(new DepContactEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseDepProfile;
            }

            return CaseDepProfile;
        }

        public bool CheckCaseSumProgramDetails(string strKey,string strRefKey, out string EligStatus,out string QueCount,out string ErrCode,out string OutErrDesc)
        {
            bool boolSuccess = false;
            string strNewEligStatus = string.Empty;
            string strNewQueCount = string.Empty;
            string strNewErrCode = string.Empty;
            string strOutErrCode = string.Empty;            
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@In_Mst_Appl_Ix",strKey));
                sqlParamList.Add(new SqlParameter("@In_Elig_Hierarchy", strRefKey));

                SqlParameter sqlOutEligStatus = new SqlParameter("@Out_Elig_Status", SqlDbType.VarChar, 10);
                sqlOutEligStatus.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlOutEligStatus);
                SqlParameter sqlOutQueCount = new SqlParameter("@Out_Que_Count", SqlDbType.VarChar, 10);
                sqlOutQueCount.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlOutQueCount);
                SqlParameter sqlOutErrCode = new SqlParameter("@Out_Err_Code", SqlDbType.VarChar, 10);
                sqlOutErrCode.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlOutErrCode);
                SqlParameter sqlOutErrDesc = new SqlParameter("@Out_Err_Desc", SqlDbType.VarChar, 10);
                sqlOutErrDesc.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlOutErrDesc);
                boolSuccess = CaseSum.CheckCaseSumProgramDetails(sqlParamList);
                strNewEligStatus = sqlOutEligStatus.Value.ToString();
                strNewQueCount = sqlOutQueCount.Value.ToString();

            }
            catch (Exception ex)
            {
                boolSuccess = false;
            }
            
             EligStatus  = strNewEligStatus;
             QueCount = strNewQueCount;
             ErrCode = strNewErrCode;
             OutErrDesc = strOutErrCode;
            return boolSuccess;
        }

        public List<CommonEntity> GetCaseSumEligData(string strKey,string strCaseSum)
        {
            List<CommonEntity> commoncaseSumElig = new List<CommonEntity>();
            try
            {
                DataSet CaseELIGQUESData = CaseSum.GetCaseSumEligData(strKey,strCaseSum);
                if (CaseELIGQUESData != null && CaseELIGQUESData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseELIGQUESData.Tables[0].Rows)
                    {
                        commoncaseSumElig.Add(new CommonEntity(row["EligStatus"].ToString(), row["EligQueCount"].ToString(), row["CaseHierachy"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return commoncaseSumElig;
            }

            return commoncaseSumElig;
        }



        public List<CaseEnrlEntity> GetCaseEnrlDetails(string agency, string dep, string program, string year, string app, string Group, string strFundHie)
        {
            List<CaseEnrlEntity> CaseEnrlDetails = new List<CaseEnrlEntity>();
            try
            {
                DataSet CaseSnp = Captain.DatabaseLayer.CaseSum.GetCaseEnrlDetails(agency, dep, program, year, app, Group, strFundHie);
                if (CaseSnp != null && CaseSnp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnp.Tables[0].Rows)
                    {
                        CaseEnrlDetails.Add(new CaseEnrlEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return CaseEnrlDetails;
            }

            return CaseEnrlDetails;
        }

        public List<CaseEnrlEntity> GetHSSB00150Details(string agency, string dep, string program, string year, string FromDate, string ToDate, string FrmSite,string ToSite,string FrmRoom,string ToRoom)
        {
            List<CaseEnrlEntity> CaseEnrlDetails = new List<CaseEnrlEntity>();
            try
            {
                DataSet CaseSnp = Captain.DatabaseLayer.ChldTrckDB.GetHSSB0150_Report(agency, dep, program, year, FromDate, ToDate, FrmSite, ToSite,FrmRoom,ToRoom);
                if (CaseSnp != null && CaseSnp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnp.Tables[0].Rows)
                    {
                        CaseEnrlDetails.Add(new CaseEnrlEntity(row,string.Empty,"HSSB00150"));
                    }
                }
            }
            catch (Exception ex)
            {
                return CaseEnrlDetails;
            }

            return CaseEnrlDetails;
        }



        public List<CaseELIGQUESEntity> GetELIGQUES()
        {
            List<CaseELIGQUESEntity> CaseEligQuesProfile = new List<CaseELIGQUESEntity>();
            try
            {
                DataSet CaseEligData = CaseSum.GetELIGQUES();
                if (CaseEligData != null && CaseEligData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseEligData.Tables[0].Rows)
                    {
                        CaseEligQuesProfile.Add(new CaseELIGQUESEntity(row,"ELIGQUES"));
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

        public List<CaseELIGQUESEntity> GetELIGCUSTOMQUES(string strHierachy)
        {
            List<CaseELIGQUESEntity> CaseEligQuesProfile = new List<CaseELIGQUESEntity>();
            try
            {
                DataSet CaseEligData = CaseSum.GetELIGCUSTOMQUES(strHierachy);
                if (CaseEligData != null && CaseEligData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseEligData.Tables[0].Rows)
                    {
                        CaseEligQuesProfile.Add(new CaseELIGQUESEntity(row, "FLDCNTLHIE"));
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
        public List<CaseELIGQUESEntity> GetELIGCUSTOMQUES(string strHierachy,string strType)
        {
            List<CaseELIGQUESEntity> CaseEligQuesProfile = new List<CaseELIGQUESEntity>();
            try
            {
                DataSet CaseEligData = CaseSum.GetELIGCUSTOMQUES(strHierachy, strType);
                if (CaseEligData != null && CaseEligData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseEligData.Tables[0].Rows)
                    {
                        CaseEligQuesProfile.Add(new CaseELIGQUESEntity(row, "FLDCNTLHIE"));
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

        public List<CaseELIGQUESEntity> GetFLDCUSTOMQUESPIP(string strHierachy, string strType)
        {
            List<CaseELIGQUESEntity> CaseEligQuesProfile = new List<CaseELIGQUESEntity>();
            try
            {
                DataSet CaseEligData = CaseSum.GetELIGCUSTOMQUES(strHierachy, strType);
                if (CaseEligData != null && CaseEligData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseEligData.Tables[0].Rows)
                    {
                        CaseEligQuesProfile.Add(new CaseELIGQUESEntity(row, strType));
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


        public bool InsertUpdateDelCaseElig(CaseELIGEntity EligEntity,out string  Msg)
        {
            bool boolSuccess = false;
            string strMsg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@CASEELIG_AGENCY", EligEntity.EligAgency));
                sqlParamList.Add(new SqlParameter("@CASEELIG_DEPT", EligEntity.EligDept));
                sqlParamList.Add(new SqlParameter("@CASEELIG_PROGRAM", EligEntity.EligProgram));
                sqlParamList.Add(new SqlParameter("@CASEELIG_GROUP_CODE", EligEntity.EligGroupCode));
                if (EligEntity.EligGroupSeq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CASEELIG_GROUP_SEQ", EligEntity.EligGroupSeq));
                if (EligEntity.EligGroupDesc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CASEELIG_GROUP_DESC", EligEntity.EligGroupDesc));
                //if (EligEntity.EligQuesScreen != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASEELIG_QUES_SCREEN", EligEntity.EligQuesScreen));
                //if (EligEntity.EligQuesType != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASEELIG_QUES_TYPE", EligEntity.EligQuesType));
                if (EligEntity.EligQuesCode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CASEELIG_QUES_CODE", EligEntity.EligQuesCode));
                if (EligEntity.EligMemberAccess != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CASEELIG_MEM_ACCESS", EligEntity.EligMemberAccess));
                if (EligEntity.EligResponseType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CASEELIG_RESP_TYPE", EligEntity.EligResponseType));
                if (EligEntity.EligConjunction!= string.Empty)
                    sqlParamList.Add(new SqlParameter("@CASEELIG_CONJN", EligEntity.EligConjunction));
                if (EligEntity.EligNumEqual != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CASEELIG_NUM_EQUAL", EligEntity.EligNumEqual));
                if (EligEntity.EligNumLthan != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CASEELIG_NUM_LTHAN", EligEntity.EligNumLthan));
                if (EligEntity.EligNumGthan != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CASEELIG_NUM_GTHAN", EligEntity.EligNumGthan));
                if (EligEntity.EligDDTextResp != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CASEELIG_DD_TEXT_RESP", EligEntity.EligDDTextResp));
                if (EligEntity.EligPoints != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CASEELIG_POINTS", EligEntity.EligPoints));                                
                if (EligEntity.AddOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CASEELIG_ADD_OPERATOR", EligEntity.AddOperator));
                if (EligEntity.LstcOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CASEELIG_LSTC_OPERATOR", EligEntity.LstcOperator));              
                sqlParamList.Add(new SqlParameter("@Mode", EligEntity.Mode));
                sqlParamList.Add(new SqlParameter("@Type", EligEntity.Type));
                
                SqlParameter sqlApplNo = new SqlParameter("@Msg", SqlDbType.VarChar, 20);
                sqlApplNo.Value = EligEntity.Msg;
                sqlApplNo.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlApplNo);
                
                boolSuccess = CaseSum.InsertUpdateDelCASEELIG(sqlParamList);
                strMsg = sqlApplNo.Value.ToString();
            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }

            Msg = strMsg;
            return boolSuccess;
        }

        public bool InsertUpdateDelSERVSTOP(SERVSTOPEntity EligEntity, out string Msg)
        {
            bool boolSuccess = false;
            string strMsg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@SRST_AGENCY", EligEntity.Agency));
                sqlParamList.Add(new SqlParameter("@SRST_DEPT", EligEntity.Dept));
                sqlParamList.Add(new SqlParameter("@SRST_PROG", EligEntity.Program));
                sqlParamList.Add(new SqlParameter("@SRST_FDATE", EligEntity.FDate));
                if (EligEntity.TDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SRST_TDATE", EligEntity.TDate));
                
                if (EligEntity.AddOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SRST_ADD_OPERATOR", EligEntity.AddOperator));
                if (EligEntity.LstcOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SRST_LSTC_OPERATOR", EligEntity.LstcOperator));
                sqlParamList.Add(new SqlParameter("@Mode", EligEntity.Mode));
                //sqlParamList.Add(new SqlParameter("@Type", EligEntity.Type));

                SqlParameter sqlApplNo = new SqlParameter("@Msg", SqlDbType.VarChar, 20);
                sqlApplNo.Value = EligEntity.Msg;
                sqlApplNo.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlApplNo);

                boolSuccess = CaseSum.InsertUpdateDelSERVSTOP(sqlParamList);
                strMsg = sqlApplNo.Value.ToString();
            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }

            Msg = strMsg;
            return boolSuccess;
        }

        public bool InsertUpdateDelSERVSTOPHIST(SERVSTOPHISTEntity EligEntity, out string Msg)
        {
            bool boolSuccess = false;
            string strMsg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@SRSTH_AGENCY", EligEntity.Agency));
                sqlParamList.Add(new SqlParameter("@SRSTH_DEPT", EligEntity.Dept));
                sqlParamList.Add(new SqlParameter("@SRSTH_PROG", EligEntity.Program));
                sqlParamList.Add(new SqlParameter("@SRSTH_SEQ", EligEntity.Seq));
                sqlParamList.Add(new SqlParameter("@SRSTH_FDATE", EligEntity.FDate));
                if (EligEntity.TDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SRSTH_TDATE", EligEntity.TDate));

                if (EligEntity.AddOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SRSTH_ADD_OPERATOR", EligEntity.AddOperator));
                if (EligEntity.LstcOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SRSTH_LSTC_OPERATOR", EligEntity.LstcOperator));
                sqlParamList.Add(new SqlParameter("@Mode", EligEntity.Mode));
                //sqlParamList.Add(new SqlParameter("@Type", EligEntity.Type));

                SqlParameter sqlApplNo = new SqlParameter("@Msg", SqlDbType.VarChar, 20);
                sqlApplNo.Value = EligEntity.Msg;
                sqlApplNo.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlApplNo);

                boolSuccess = CaseSum.InsertUpdateDelSERVSTOP(sqlParamList);
                strMsg = sqlApplNo.Value.ToString();
            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }

            Msg = strMsg;
            return boolSuccess;
        }

        public bool InsertUpdateDelSSBGParams(SSBGPARAMEntity EligEntity, out string Msg)
        {
            bool boolSuccess = false;
            string strMsg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@SBGP_AGENCY", EligEntity.SSBGAgency));
                sqlParamList.Add(new SqlParameter("@SBGP_DEPT", EligEntity.SSBGDept));
                sqlParamList.Add(new SqlParameter("@SBGP_PROG", EligEntity.SSBGProgram));
                sqlParamList.Add(new SqlParameter("@SBGP_YEAR", EligEntity.SSBGYear));
                if (EligEntity.SSBGSeq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_SEQ", EligEntity.SSBGSeq));
                //if (EligEntity.SSBGRPFrom != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGP_RP_FROM", EligEntity.SSBGRPFrom));
                //if (EligEntity.EligQuesScreen != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASEELIG_QUES_SCREEN", EligEntity.EligQuesScreen));
                //if (EligEntity.EligQuesType != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASEELIG_QUES_TYPE", EligEntity.EligQuesType));
                //if (EligEntity.SSBGRPTo != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGP_RP_TO", EligEntity.SSBGRPTo));
                if (EligEntity.SSBGGPFrom != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_GP_FROM", EligEntity.SSBGGPFrom));
                if (EligEntity.SSBGGPTo != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_GP_TO", EligEntity.SSBGGPTo));
                if (EligEntity.SSBGBudget != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_BUDGET", EligEntity.SSBGBudget));
                if (EligEntity.SSBGAward != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_GAWARD", EligEntity.SSBGAward));
                if (EligEntity.SSBGRPDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_RP_DATE", EligEntity.SSBGRPDate));
                if (EligEntity.SSBGDesc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_DESC", EligEntity.SSBGDesc));
                //if (EligEntity.EligDDTextResp != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASEELIG_DD_TEXT_RESP", EligEntity.EligDDTextResp));
                //if (EligEntity.EligPoints != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASEELIG_POINTS", EligEntity.EligPoints));
                if (EligEntity.AddOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_ADD_OPERATOR", EligEntity.AddOperator));
                if (EligEntity.LstcOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_LSTC_OPERATOR", EligEntity.LstcOperator));
                if (EligEntity.SSBGCntArea1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_CNTAREA1", EligEntity.SSBGCntArea1));

                if (EligEntity.SSBGCntArea2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_CNTAREA2", EligEntity.SSBGCntArea2));
                if (EligEntity.SSBGCntArea3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_CNTAREA3", EligEntity.SSBGCntArea3));
                if (EligEntity.SSBGCntArea4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_CNTAREA4", EligEntity.SSBGCntArea4));
                if (EligEntity.SSBGCntArea5 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_CNTAREA5", EligEntity.SSBGCntArea5));
                if (EligEntity.SSBGCntArea1_Chk != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_CNTAREA1_CHK", EligEntity.SSBGCntArea1_Chk));

                if (EligEntity.SSBGCntArea2_Chk != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_CNTAREA2_CHK", EligEntity.SSBGCntArea2_Chk));
                if (EligEntity.SSBGCntArea3_Chk != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_CNTAREA3_CHK", EligEntity.SSBGCntArea3_Chk));
                if (EligEntity.SSBGCntArea4_Chk != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_CNTAREA4_CHK", EligEntity.SSBGCntArea4_Chk));
                if (EligEntity.SSBGCntArea5_Chk != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_CNTAREA5_CHK", EligEntity.SSBGCntArea5_Chk));
                if (EligEntity.SSBGCntArea1_Val != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_CNTAREA1_VAL", EligEntity.SSBGCntArea1_Val));

                if (EligEntity.SSBGCntArea2_Val != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_CNTAREA2_VAL", EligEntity.SSBGCntArea2_Val));
                if (EligEntity.SSBGCntArea3_Val != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_CNTAREA3_VAL", EligEntity.SSBGCntArea3_Val));
                if (EligEntity.SSBGCntArea4_Val != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_CNTAREA4_VAL", EligEntity.SSBGCntArea4_Val));
                if (EligEntity.SSBGCntArea5_Val != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_CNTAREA5_VAL", EligEntity.SSBGCntArea5_Val));
                if (EligEntity.SSBGCust1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_CUST1", EligEntity.SSBGCust1));

                if (EligEntity.SSBGCust2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_CUST2", EligEntity.SSBGCust2));
                if (EligEntity.SSBGCust3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_CUST3", EligEntity.SSBGCust3));
                if (EligEntity.SSBGCust4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_CUST4", EligEntity.SSBGCust4));
                if (EligEntity.SSBGCust5 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_CUST5", EligEntity.SSBGCust5));
                if (EligEntity.SSBGProgDesc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGP_PROG_DESC", EligEntity.SSBGProgDesc));

                sqlParamList.Add(new SqlParameter("@Mode", EligEntity.Mode));
                //sqlParamList.Add(new SqlParameter("@Type", EligEntity.Type));

                SqlParameter sqlApplNo = new SqlParameter("@CurrentId", SqlDbType.VarChar, 20);
                sqlApplNo.Value = EligEntity.Msg;
                sqlApplNo.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlApplNo);

                boolSuccess = CaseSum.InsertUpdateDelSSBGPARAMS(sqlParamList);
                strMsg = sqlApplNo.Value.ToString();
            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }

            Msg = strMsg;
            return boolSuccess;
        }

        public bool InsertUpdateDelSSBGMonths(SSBGMONTHSEntity EligEntity)
        {
            bool boolSuccess = false;
            string strMsg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@SBGM_AGENCY", EligEntity.SSBGAgency));
                sqlParamList.Add(new SqlParameter("@SBGM_DEPT", EligEntity.SSBGDept));
                sqlParamList.Add(new SqlParameter("@SBGM_PROG", EligEntity.SSBGProgram));
                sqlParamList.Add(new SqlParameter("@SBGM_YEAR", EligEntity.SSBGYear));
                if (EligEntity.SSBGSeq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGM_SEQ", EligEntity.SSBGSeq));
                if (EligEntity.SSBGCode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGM_CODE", EligEntity.SSBGCode));

                //if (EligEntity.SSBGRPFrom != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGP_RP_FROM", EligEntity.SSBGRPFrom));
                //if (EligEntity.EligQuesScreen != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASEELIG_QUES_SCREEN", EligEntity.EligQuesScreen));
                //if (EligEntity.EligQuesType != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASEELIG_QUES_TYPE", EligEntity.EligQuesType));
                //if (EligEntity.SSBGRPTo != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGP_RP_TO", EligEntity.SSBGRPTo));
                if (EligEntity.SSBGYearValue != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGM_YEAR_VALUE", EligEntity.SSBGYearValue));
                //if (EligEntity.SSBGGPTo != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGP_GP_TO", EligEntity.SSBGGPTo));
                //if (EligEntity.SSBGBudget != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGP_BUDGET", EligEntity.SSBGBudget));
                //if (EligEntity.SSBGAward != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGP_GAWARD", EligEntity.SSBGAward));
                //if (EligEntity.SSBGRPDate != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGP_RP_DATE", EligEntity.SSBGRPDate));
                //if (EligEntity.SSBGDesc != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGP_DESC", EligEntity.SSBGDesc));
                //if (EligEntity.EligDDTextResp != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASEELIG_DD_TEXT_RESP", EligEntity.EligDDTextResp));
                //if (EligEntity.EligPoints != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASEELIG_POINTS", EligEntity.EligPoints));
                if (EligEntity.AddOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGM_ADD_OPERATOR", EligEntity.AddOperator));
                if (EligEntity.LstcOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGM_LSTC_OPERATOR", EligEntity.LstcOperator));
                if (EligEntity.Month != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGM_MONTH", EligEntity.Month));

                if (EligEntity.CntlOblig != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGM_CONT_OBLIG", EligEntity.CntlOblig));

                sqlParamList.Add(new SqlParameter("@Mode", EligEntity.Mode));
                //sqlParamList.Add(new SqlParameter("@Type", EligEntity.Type));

                //SqlParameter sqlApplNo = new SqlParameter("@CurrentId", SqlDbType.VarChar, 20);
                //sqlApplNo.Value = EligEntity.Msg;
                //sqlApplNo.Direction = ParameterDirection.Output;
                //sqlParamList.Add(sqlApplNo);

                boolSuccess = CaseSum.InsertUpdateDelSSBGMonths(sqlParamList);
                //strMsg = sqlApplNo.Value.ToString();
            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }

            //Msg = strMsg;
            return boolSuccess;
        }


        public bool InsertUpdateDelSSBGTypes(SSBGTYPESEntity EligEntity, out string Msg)
        {
            bool boolSuccess = false;
            string strMsg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@SBGT_ID", EligEntity.SBGTID));
                sqlParamList.Add(new SqlParameter("@SBGT_CODE", EligEntity.SBGTCode));
                sqlParamList.Add(new SqlParameter("@SBGT_CODE_SEQ", EligEntity.SBGTCodeSeq));
                if (EligEntity.SBGTGroup != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_GROUP", EligEntity.SBGTGroup));
                if (EligEntity.SBGTGroupSeq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_GROUP_SEQ", EligEntity.SBGTGroupSeq));
                if (EligEntity.SBGTDesc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_DESC", EligEntity.SBGTDesc));
                //sqlParamList.Add(new SqlParameter("@SBGT_CONT_OBILG", EligEntity.SBGTCntlOblig));
                if (EligEntity.SBGTQuesCode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_QUES_CODE", EligEntity.SBGTQuesCode));
                if (EligEntity.SBGTMemAccess != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_MEM_ACCESS", EligEntity.SBGTMemAccess));
                if (EligEntity.SBGTRespType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_RESP_TYPE", EligEntity.SBGTRespType));
                if (EligEntity.SBGTConj != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_CONJN", EligEntity.SBGTConj));
                if (EligEntity.SBGTCntlOblig != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_CONT_OBILG", EligEntity.SBGTCntlOblig));
                if (EligEntity.SBGTNumEqual != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_NUM_EQUAL", EligEntity.SBGTNumEqual));
                if (EligEntity.SBGTNumLThan != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_NUM_LTHAN", EligEntity.SBGTNumLThan));
                if (EligEntity.SBGTNumGThan != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_NUM_GTHAN", EligEntity.SBGTNumGThan));
                if (EligEntity.SBGTDateEqual != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_DATE_EQUAL", EligEntity.SBGTDateEqual));
                if (EligEntity.SBGTDateLThan != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_DATE_LTHAN", EligEntity.SBGTDateLThan));
                if (EligEntity.SBGTDateGThan != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_DATE_GTHAN", EligEntity.SBGTDateGThan));
                if (EligEntity.SBGTRESP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_DD_TEXT_RESP", EligEntity.SBGTRESP));
                if (EligEntity.SBGT_Name != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_NAME", EligEntity.SBGT_Name));
                if (EligEntity.SBGT_CNTAREA1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_CNTAREA1", EligEntity.SBGT_CNTAREA1));
                if (EligEntity.SBGT_CNTAREA2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_CNTAREA2", EligEntity.SBGT_CNTAREA2));
                if (EligEntity.SBGT_CNTAREA3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_CNTAREA3", EligEntity.SBGT_CNTAREA3));
                if (EligEntity.SBGT_CNTAREA4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_CNTAREA4", EligEntity.SBGT_CNTAREA4));
                if (EligEntity.SBGT_CNTAREA5 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_CNTAREA5", EligEntity.SBGT_CNTAREA5));
                if (EligEntity.SBGT_CNTAREA1_VAL != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_CNTAREA1_VAL", EligEntity.SBGT_CNTAREA1_VAL));
                if (EligEntity.SBGT_CNTAREA2_VAL != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_CNTAREA2_VAL", EligEntity.SBGT_CNTAREA2_VAL));
                if (EligEntity.SBGT_CNTAREA3_VAL != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_CNTAREA3_VAL", EligEntity.SBGT_CNTAREA3_VAL));
                if (EligEntity.SBGT_CNTAREA4_VAL != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_CNTAREA4_VAL", EligEntity.SBGT_CNTAREA4_VAL));
                if (EligEntity.SBGT_CNTAREA5_VAL != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_CNTAREA5_VAL", EligEntity.SBGT_CNTAREA5_VAL));
                if (EligEntity.AddOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_ADD_OPERATOR", EligEntity.AddOperator));
                if (EligEntity.LstcOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGT_LSTC_OPERATOR", EligEntity.LstcOperator));
                sqlParamList.Add(new SqlParameter("@Mode", EligEntity.Mode));
                sqlParamList.Add(new SqlParameter("@Type", EligEntity.Type));

                SqlParameter sqlApplNo = new SqlParameter("@CurrentId", SqlDbType.VarChar, 20);
                sqlApplNo.Value = EligEntity.Msg;
                sqlApplNo.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlApplNo);

                boolSuccess = CaseSum.InsertUpdateDelSSBGTYPES(sqlParamList);
                strMsg = sqlApplNo.Value.ToString();
            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }

            Msg = strMsg;
            return boolSuccess;
        }

        public bool InsertUpdateDelSSBGGoals(SSBGGOALSEntity EligEntity, out string Msg)
        {
            bool boolSuccess = false;
            string strMsg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@SBGG_ID", EligEntity.SBGGID));
                sqlParamList.Add(new SqlParameter("@SBGG_CODE", EligEntity.SBGGCode));
                sqlParamList.Add(new SqlParameter("@SBGG_SEQ", EligEntity.SBGGSeq));
                if (EligEntity.SBGGDesc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGG_DESC", EligEntity.SBGGDesc));
                if (EligEntity.SBGGCountType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGG_COUNT_TYPE", EligEntity.SBGGCountType));
                if (EligEntity.Type2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGG_TYPE2", EligEntity.Type2));
                //sqlParamList.Add(new SqlParameter("@SBGT_CONT_OBILG", EligEntity.SBGTCntlOblig));
                //if (EligEntity.SBGTQuesCode != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGT_QUES_CODE", EligEntity.SBGTQuesCode));
                //if (EligEntity.SBGTMemAccess != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGT_MEM_ACCESS", EligEntity.SBGTMemAccess));
                //if (EligEntity.SBGTRespType != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGT_RESP_TYPE", EligEntity.SBGTRespType));
                //if (EligEntity.SBGTConj != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGT_CONJN", EligEntity.SBGTConj));
                //if (EligEntity.SBGTCntlOblig != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGT_CONT_OBILG", EligEntity.SBGTCntlOblig));
                //if (EligEntity.SBGTNumEqual != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGT_NUM_EQUAL", EligEntity.SBGTNumEqual));
                //if (EligEntity.SBGTNumLThan != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGT_NUM_LTHAN", EligEntity.SBGTNumLThan));
                //if (EligEntity.SBGTNumGThan != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGT_NUM_GTHAN", EligEntity.SBGTNumGThan));
                //if (EligEntity.SBGTDateEqual != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGT_DATE_EQUAL", EligEntity.SBGTDateEqual));
                //if (EligEntity.SBGTDateLThan != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGT_DATE_LTHAN", EligEntity.SBGTDateLThan));
                //if (EligEntity.SBGTDateGThan != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGT_DATE_GTHAN", EligEntity.SBGTDateGThan));
                //if (EligEntity.SBGTRESP != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGT_DD_TEXT_RESP", EligEntity.SBGTRESP));
                //if (EligEntity.SSBGAward != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGP_GAWARD", EligEntity.SSBGAward));
                //if (EligEntity.SSBGRPDate != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGP_RP_DATE", EligEntity.SSBGRPDate));
                //if (EligEntity.SSBGDesc != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SBGP_DESC", EligEntity.SSBGDesc));
                //if (EligEntity.EligDDTextResp != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASEELIG_DD_TEXT_RESP", EligEntity.EligDDTextResp));
                //if (EligEntity.EligPoints != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CASEELIG_POINTS", EligEntity.EligPoints));
                if (EligEntity.AddOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGG_ADD_OPERATOR", EligEntity.AddOperator));
                if (EligEntity.LstcOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SBGG_LSTC_OPERATOR", EligEntity.LstcOperator));
                sqlParamList.Add(new SqlParameter("@Mode", EligEntity.Mode));
                sqlParamList.Add(new SqlParameter("@Type", EligEntity.Type));

                SqlParameter sqlApplNo = new SqlParameter("@CurrentId", SqlDbType.VarChar, 20);
                sqlApplNo.Value = EligEntity.Msg;
                sqlApplNo.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlApplNo);

                boolSuccess = CaseSum.InsertUpdateDelSSBGGoals(sqlParamList);
                strMsg = sqlApplNo.Value.ToString();
            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }

            Msg = strMsg;
            return boolSuccess;
        }

        public bool UpdateSSBGSerPlan(string Row_Type, string Id, string Goal, string Obj, string SerPlan, string Seq, string CAMS_Xml, string User_ID, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@Row_Type", Row_Type));
                sqlParamList.Add(new SqlParameter("@SBSP_ID", Id));
                sqlParamList.Add(new SqlParameter("@SBSP_GOAL", Goal));
                sqlParamList.Add(new SqlParameter("@SBSP_OBJ", Obj));
                sqlParamList.Add(new SqlParameter("@SBSP_SERPLAN", SerPlan));
                sqlParamList.Add(new SqlParameter("@SBSP_SEQ", Seq));
                //sqlParamList.Add(new SqlParameter("@TRAGSM_PROGRAM", Program));
                sqlParamList.Add(new SqlParameter("@SBSP_LSTC_OPERATOR", User_ID));

                sqlParamList.Add(new SqlParameter("@Applicants_XML", CAMS_Xml));


                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateSSBGSERPLANS", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateSSBGAssns(string Row_Type, string Trig_id, string Goal, string Obj, string Trig_SP, string Seq, string Branch, string ogr_Grp, string CAMS_Type, string CAMS_Code, string CAMS_Xml, string User_ID, out string Sql_Reslut_Msg) //11042012
        {
            bool boolsuccess;
            Sql_Reslut_Msg = "Success";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@Row_Type", Row_Type));
                sqlParamList.Add(new SqlParameter("@SBAS_ID", Trig_id));
                sqlParamList.Add(new SqlParameter("@SBAS_GOAL", Goal));
                sqlParamList.Add(new SqlParameter("@SBAS_OBJ", Obj));
                sqlParamList.Add(new SqlParameter("@SBAS_SERPLAN", Trig_SP));
                sqlParamList.Add(new SqlParameter("@SBAS_SEQ", Seq));

                sqlParamList.Add(new SqlParameter("@SBAS_BRANCH", Branch));
                sqlParamList.Add(new SqlParameter("@SBAS_ORIG_GRP", ogr_Grp));
                sqlParamList.Add(new SqlParameter("@SBAS_CAMS", CAMS_Type));
                sqlParamList.Add(new SqlParameter("@SBAS_CAMS_CODE", CAMS_Code));
                sqlParamList.Add(new SqlParameter("@SBAS_LSTC_OPERATOR", User_ID));

                sqlParamList.Add(new SqlParameter("@Applicants_XML", CAMS_Xml));

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UpdateSSBGASSNS", out Sql_Reslut_Msg);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }


        public List<CaseELIGEntity> GetCaseEligHierachys(string UserID,string AdminAgency)
        {
            List<CaseELIGEntity> CaseEligprofile = new List<CaseELIGEntity>();
            try
            {
                DataSet CaseElighierchy = CaseSum.GetCaseEligHierachys(UserID, AdminAgency);
                if (CaseElighierchy != null && CaseElighierchy.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseElighierchy.Tables[0].Rows)
                    {
                        CaseEligprofile.Add(new CaseELIGEntity(row,"Hierarchy"));
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

        public List<SSBGPARAMEntity> GetSSBGHierachys()
        {
            List<SSBGPARAMEntity> CaseEligprofile = new List<SSBGPARAMEntity>();
            try
            {
                DataSet CaseElighierchy = CaseSum.GetSSBGHierachys();
                if (CaseElighierchy != null && CaseElighierchy.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseElighierchy.Tables[0].Rows)
                    {
                        CaseEligprofile.Add(new SSBGPARAMEntity(row, "Hierarchy"));
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

        public List<CaseELIGEntity> GetCASEELIGadpgs(string agency, string dept, string program, string groupcode, string seq, string type)
        {
            List<CaseELIGEntity> CaseEligprofile = new List<CaseELIGEntity>();
            try
            {
                DataSet CaseElighierchy = CaseSum.GetCASEELIGadpgs(agency,dept,program,groupcode,seq,type);
                if (CaseElighierchy != null && CaseElighierchy.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseElighierchy.Tables[0].Rows)
                    {
                        CaseEligprofile.Add(new CaseELIGEntity(row,string.Empty));
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

        public List<SSBGTYPESEntity> GetSSBGTypesadpgs(string Id, string Typecode, string seq,string Group,string GrpSeq, string type)
        {
            List<SSBGTYPESEntity> CaseEligprofile = new List<SSBGTYPESEntity>();
            try
            {
                DataSet CaseElighierchy = CaseSum.GetSSBGTypesadpgs(Id, Typecode, seq, Group, GrpSeq, type);
                if (CaseElighierchy != null && CaseElighierchy.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseElighierchy.Tables[0].Rows)
                    {
                        CaseEligprofile.Add(new SSBGTYPESEntity(row, type));
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

        public List<CaseELIGEntity> GetCASEELIGadpgsGroup(string agency, string dept, string program, string groupcode, string seq, string type)
        {
            List<CaseELIGEntity> CaseEligprofile = new List<CaseELIGEntity>();
            try
            {
                DataSet CaseElighierchy = CaseSum.GetCASEELIGadpgs(agency, dept, program, groupcode, seq, type);
                if (CaseElighierchy != null && CaseElighierchy.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseElighierchy.Tables[0].Rows)
                    {
                        CaseEligprofile.Add(new CaseELIGEntity(row, "Group"));
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


        public List<SERVSTOPEntity> GetSERVSTOPDet(string agency, string dept, string program, string Fdate, string TDate)
        {
            List<SERVSTOPEntity> CaseEligprofile = new List<SERVSTOPEntity>();
            try
            {
                DataSet CaseElighierchy = CaseSum.GetSREVSTOP(agency, dept, program, Fdate, TDate);
                if (CaseElighierchy != null && CaseElighierchy.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseElighierchy.Tables[0].Rows)
                    {
                        CaseEligprofile.Add(new SERVSTOPEntity(row));
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

        public List<SERVSTOPHISTEntity> GetSERVSTOPHistDet(string agency, string dept, string program, string Fdate, string TDate)
        {
            List<SERVSTOPHISTEntity> CaseEligprofile = new List<SERVSTOPHISTEntity>();
            try
            {
                DataSet CaseElighierchy = CaseSum.GetSREVSTOPHIST(agency, dept, program, Fdate, TDate);
                if (CaseElighierchy != null && CaseElighierchy.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseElighierchy.Tables[0].Rows)
                    {
                        CaseEligprofile.Add(new SERVSTOPHISTEntity(row));
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

        public List<SSBGPARAMEntity> GetSSBGParams(string agency, string dept, string program)
        {
            List<SSBGPARAMEntity> CaseEligprofile = new List<SSBGPARAMEntity>();
            try
            {
                DataSet CaseElighierchy = CaseSum.Browse_SSBGPARAMS(agency, dept, program);
                if (CaseElighierchy != null && CaseElighierchy.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseElighierchy.Tables[0].Rows)
                    {
                        CaseEligprofile.Add(new SSBGPARAMEntity(row));
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

        public List<SSBGMONTHSEntity> GetSSBGMonths(string agency, string dept, string program)
        {
            List<SSBGMONTHSEntity> CaseEligprofile = new List<SSBGMONTHSEntity>();
            try
            {
                DataSet CaseElighierchy = CaseSum.Browse_SSBGMonths(agency, dept, program);
                if (CaseElighierchy != null && CaseElighierchy.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseElighierchy.Tables[0].Rows)
                    {
                        CaseEligprofile.Add(new SSBGMONTHSEntity(row));
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

        public List<SSBGTYPESEntity> GetSSBGTypes()
        {
            List<SSBGTYPESEntity> CaseEligprofile = new List<SSBGTYPESEntity>();
            try
            {
                DataSet CaseElighierchy = CaseSum.Browse_SSBGTYPES();
                if (CaseElighierchy != null && CaseElighierchy.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseElighierchy.Tables[0].Rows)
                    {
                        CaseEligprofile.Add(new SSBGTYPESEntity(row));
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

        public List<SSBGGOALSEntity> GetSSBGGoals()
        {
            List<SSBGGOALSEntity> CaseEligprofile = new List<SSBGGOALSEntity>();
            try
            {
                DataSet CaseElighierchy = CaseSum.Browse_SSBGGoals();
                if (CaseElighierchy != null && CaseElighierchy.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseElighierchy.Tables[0].Rows)
                    {
                        CaseEligprofile.Add(new SSBGGOALSEntity(row));
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

        //public List<CaseELIGEntity> Browse_CASEELIG()
        //{
        //    List<CaseELIGEntity> CaseEligProfile = new List<CaseELIGEntity>();
        //    try
        //    {
        //        DataSet CaseEligData = CaseSum.Browse_CASEELIG();
        //        if (CaseEligData != null && CaseEligData.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow row in CaseEligData.Tables[0].Rows)
        //            {
        //                CaseEligProfile.Add(new CaseELIGEntity(row));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //
        //        return CaseEligProfile;
        //    }

        public List<SSBGSerPlanEntity> Browse_SSBGSerplan(string Id, string Goal,string Obj,string SerPlan)
        {
            List<SSBGSerPlanEntity> LIHEAPBProfile = new List<SSBGSerPlanEntity>();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                //if()
                sqlParamList.Add(new SqlParameter("@SBSP_ID", Id));
                //sqlParamList.Add(new SqlParameter("@TRG_Name", Trig_Name)); 
                DataSet LIHEAPBData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_SSBGSERPLAN]");
                //DataSet LIHEAPBData = Captain.DatabaseLayer.SPAdminDB.Browse_LIHEAPB(sqlParamList);

                if (LIHEAPBData != null && LIHEAPBData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in LIHEAPBData.Tables[0].Rows)
                        LIHEAPBProfile.Add(new SSBGSerPlanEntity(row));
                }
            }
            catch (Exception ex)
            { return LIHEAPBProfile; }

            return LIHEAPBProfile;
        }

        public List<SSBGAssnsEntity> Browse_SSBGAssns(string Id, string Goal, string Obj, string SerPlan,string Seq)
        {
            List<SSBGAssnsEntity> LIHEAPBProfile = new List<SSBGAssnsEntity>();
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                //if()
                //sqlParamList.Add(new SqlParameter("@TRG_ID", Trig_id));
                //sqlParamList.Add(new SqlParameter("@TRG_Name", Trig_Name)); 
                DataSet LIHEAPBData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_SSBGASSNS]");
                //DataSet LIHEAPBData = Captain.DatabaseLayer.SPAdminDB.Browse_LIHEAPB(sqlParamList);

                if (LIHEAPBData != null && LIHEAPBData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in LIHEAPBData.Tables[0].Rows)
                        LIHEAPBProfile.Add(new SSBGAssnsEntity(row));
                }
            }
            catch (Exception ex)
            { return LIHEAPBProfile; }

            return LIHEAPBProfile;
        }

        public static string GetCustRespCode(string strType, string strcode)
        {
            DataSet lookUpData = CaseSum.GetCustRespCode(strType, strcode);
            if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
            {
                return lookUpData.Tables[0].Rows[0]["RSP_DESC"].ToString();
            }
            else
                return string.Empty;
        }
   

        //    return CaseEligProfile;
        //}

        public List<CaseELIGQUESEntity> Browse_ELIGQUES()
        {
            List<CaseELIGQUESEntity> CaseEligQUESProfile = new List<CaseELIGQUESEntity>();
            try
            {
                DataSet CaseELIGQUESData = CaseSum.Browse_ELIGQUES();
                if (CaseELIGQUESData != null && CaseELIGQUESData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseELIGQUESData.Tables[0].Rows)
                    {
                        CaseEligQUESProfile.Add(new CaseELIGQUESEntity(row, "ELIGQUES"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseEligQUESProfile;
            }

            return CaseEligQUESProfile;
        }

        public List<CaseSumEntity> GetCaseSumHiearchyAllDetails(string agency, string dep, string program, string year, string app)
        {
            List<CaseSumEntity> CaseSumDetails = new List<CaseSumEntity>();
            try
            {
                DataSet CaseSnp = Captain.DatabaseLayer.CaseSum.GetCaseSumHieAllDetails(agency, dep, program, year, app);
                if (CaseSnp != null && CaseSnp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnp.Tables[0].Rows)
                    {
                        CaseSumDetails.Add(new CaseSumEntity(row, "CaseHieDetails"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSumDetails;
            }

            return CaseSumDetails;
        }

        public List<CommonEntity> CAPS_HOLIDAY_GET(string strYear, string strDate, string strMode)
        {
            List<CommonEntity> _Holidayslist = new List<CommonEntity>();
            try
            {
                DataSet Holidaysds = Captain.DatabaseLayer.CaseSum.CAPS_HOLIDAYS_GET(strYear, strDate, strMode);
                if (Holidaysds != null && Holidaysds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in Holidaysds.Tables[0].Rows)
                    {
                        _Holidayslist.Add(new CommonEntity(row["HLD_YEAR"].ToString(), row["HLD_DATE"].ToString(), row["HLD_DESC"].ToString(), row["HLD_REMARKS"].ToString(), row["HLD_LSTC_OPERATOR"].ToString(), row["HLD_DATE_LSTC"].ToString(), row["HLD_ADD_OPERATOR"].ToString(), row["HLD_DATE_ADD"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return _Holidayslist;
            }

            return _Holidayslist;
        }

        public bool CAPS_HOLIDAYS_INSUPDEL(string strYear, string strDate, string strDesc, string strRemarks, string strUserid, string strMode)
        {
            bool boolSuccess = false;
            string strMsg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@HLD_YEAR", strYear));
                sqlParamList.Add(new SqlParameter("@HLD_DATE", strDate));
                sqlParamList.Add(new SqlParameter("@HLD_DESC", strDesc));

                if (strRemarks != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLD_REMARKS", strRemarks));
                if (strUserid != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLD_LSTC_OPERATOR", strUserid));


                sqlParamList.Add(new SqlParameter("@Mode", strMode));

                boolSuccess = CaseSum.CAPS_HOLIDAYS_INSUPDEL(sqlParamList);
                //strMsg = sqlApplNo.Value.ToString();
            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }

            //Msg = strMsg;
            return boolSuccess;
        }
    }
}
