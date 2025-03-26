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
    public class HlsTrckData
    {
        public HlsTrckData(CaptainModel model)
        {
            Model = model;
        }

        #region Properties

        public CaptainModel Model { get; set; }

        public string UserId { get; set; }

        #endregion

        public bool InsertUpdateDelHlsTrck(HlsTrckEntity trckEntity)
        {
            bool boolSuccess = false;           
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();



                if (trckEntity.Agency != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_AGENCY", trckEntity.Agency));
                if (trckEntity.Dept != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_DEPT", trckEntity.Dept));
                if (trckEntity.Prog != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_PROGRAM", trckEntity.Prog));
                if (trckEntity.COMPONENT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_COMPONENT", trckEntity.COMPONENT));
                if (trckEntity.TASK != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_TASK", trckEntity.TASK));
                if (trckEntity.SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_SEQ", trckEntity.SEQ));
                if (trckEntity.TASKDESCRIPTION != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_TASK_DESCRIPTION", trckEntity.TASKDESCRIPTION));
                //if (trckEntity.ENTRYYN != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@HLSTRCK_ENTRY_YN", trckEntity.ENTRYYN));
                //if (trckEntity.ENTRY != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@HLSTRCK_ENTRY", trckEntity.ENTRY));
                //if (trckEntity.NEXTYN != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@HLSTRCK_NEXT_YN", trckEntity.NEXTYN));
                //if (trckEntity.NEXTTASK != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@HLSTRCK_NEXT_TASK", trckEntity.NEXTTASK));
                //if (trckEntity.NEXTDAYS != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@HLSTRCK_NEXT_DAYS", trckEntity.NEXTDAYS));
                //if (trckEntity.RESPONSE != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@HLSTRCK_RESPONSE", trckEntity.RESPONSE));
                //if (trckEntity.RESPONSEVAL1 != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@HLSTRCK_RESPONSE_VAL1", trckEntity.RESPONSEVAL1));
                //if (trckEntity.RESPONSELIT1 != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@HLSTRCK_RESPONSE_LIT1", trckEntity.RESPONSELIT1));
                //if (trckEntity.RESPONSEVAL2 != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@HLSTRCK_RESPONSE_VAL2", trckEntity.RESPONSEVAL2));
                //if (trckEntity.RESPONSELIT2 != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@HLSTRCK_RESPONSE_LIT2", trckEntity.RESPONSELIT2));
                //if (trckEntity.RESPONSEVAL3 != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@HLSTRCK_RESPONSE_VAL3", trckEntity.RESPONSEVAL3));
                //if (trckEntity.RESPONSELIT3 != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@HLSTRCK_RESPONSE_LIT3", trckEntity.RESPONSELIT3));
                //if (trckEntity.RESPONSEVAL4 != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@HLSTRCK_RESPONSE_VAL4", trckEntity.RESPONSEVAL4));
                //if (trckEntity.RESPONSELIT4 != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@HLSTRCK_RESPONSE_LIT4", trckEntity.RESPONSELIT4));
                //if (trckEntity.NXTASKDESC != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@HLSTRCK_NXTASK_DESC", trckEntity.NXTASKDESC));
                //if (trckEntity.QUESTION != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@HLSTRCK_QUESTION", trckEntity.QUESTION));

                if (trckEntity.COMPONENTOWNER1 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_COMPONENT_OWNER1", trckEntity.COMPONENTOWNER1));
                if (trckEntity.COMPONENTOWNER2 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_COMPONENT_OWNER2", trckEntity.COMPONENTOWNER2));
                if (trckEntity.COMPONENTOWNER3 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_COMPONENT_OWNER3", trckEntity.COMPONENTOWNER3));

                if (trckEntity.CustQCodes != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_CUSTQ_CODES", trckEntity.CustQCodes));
               
                if (trckEntity.COUNT != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_COUNT", trckEntity.COUNT));
                if (trckEntity.GCHARTCODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_GCHART_CODE", trckEntity.GCHARTCODE));
                if (trckEntity.QUESTIONR != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_QUESTION_R", trckEntity.QUESTIONR));
                if (trckEntity.QUESTIONE != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_QUESTION_E", trckEntity.QUESTIONE));
                if (trckEntity.CASENOTESR  != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_CASENOTES_R", trckEntity.CASENOTESR));
                if (trckEntity.CASENOTESE != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_CASENOTES_E", trckEntity.CASENOTESE));
                if (trckEntity.SBCBR != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_SBCB_R", trckEntity.SBCBR));
                if (trckEntity.SBCBE != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_SBCB_E", trckEntity.SBCBE));
                if (trckEntity.ADDRESSR != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_ADDRESS_R", trckEntity.ADDRESSR));
                if (trckEntity.ADDRESSE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_ADDRESS_E", trckEntity.ADDRESSE));
                if (trckEntity.COMPLETER != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_COMPLETE_R", trckEntity.COMPLETER));
                if (trckEntity.COMPLETEE != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_COMPLETE_E", trckEntity.COMPLETEE));
                if (trckEntity.FOLLOWUPR != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_FOLLOWUP_R", trckEntity.FOLLOWUPR));
                if (trckEntity.FOLLOWUPE != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_FOLLOWUP_E", trckEntity.FOLLOWUPE));
                if (trckEntity.FOLLOWUPCR != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_FOLLOWUPC_R", trckEntity.FOLLOWUPCR));
                if (trckEntity.FOLLOWUPCE != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_FOLLOWUPC_E", trckEntity.FOLLOWUPCE));
                if (trckEntity.DIAGNOSER != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_DIAGNOSE_R", trckEntity.DIAGNOSER));
                if (trckEntity.DIAGNOSEE != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_DIAGNOSE_E", trckEntity.DIAGNOSEE));
                if (trckEntity.SSRR != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_SSR_R", trckEntity.SSRR));
                if (trckEntity.SSRE != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_SSR_E", trckEntity.SSRE));
                if (trckEntity.WHERER != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_WHERE_R", trckEntity.WHERER));
                if (trckEntity.WHEREE != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_WHERE_E", trckEntity.WHEREE));
                if (trckEntity.QUESTIONR1 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_QUESTION_R1", trckEntity.QUESTIONR1));
                if (trckEntity.CASENOTESR1 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_CASENOTES_R1", trckEntity.CASENOTESR1));
                if (trckEntity.SBCBR1 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_SBCB_R1", trckEntity.SBCBR1));
                if (trckEntity.ADDRESSR1 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_ADDRESS_R1", trckEntity.ADDRESSR1));
                if (trckEntity.COMPLETER1 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_COMPLETE_R1", trckEntity.COMPLETER1));
                if (trckEntity.FOLLOWUPR1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_FOLLOWUP_R1", trckEntity.FOLLOWUPR1));
                if (trckEntity.FOLLOWUPCR1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_FOLLOWUPC_R1", trckEntity.FOLLOWUPCR1));
                if (trckEntity.DIAGNOSER1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_DIAGNOSE_R1", trckEntity.DIAGNOSER1));
                if (trckEntity.SSRR1 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_SSR_R1", trckEntity.SSRR1));
                if (trckEntity.QUESTIONR2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_QUESTION_R2", trckEntity.QUESTIONR2));
                if (trckEntity.CASENOTESR2 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_CASENOTES_R2", trckEntity.CASENOTESR2));
                if (trckEntity.SBCBR2 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_SBCB_R2", trckEntity.SBCBR2));
                if (trckEntity.ADDRESSR2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_ADDRESS_R2", trckEntity.ADDRESSR2));
                if (trckEntity.COMPLETER2 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_COMPLETE_R2", trckEntity.COMPLETER2));
                if (trckEntity.FOLLOWUPR2 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_FOLLOWUP_R2", trckEntity.FOLLOWUPR2));
                if (trckEntity.FOLLOWUPCR2 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_FOLLOWUPC_R2", trckEntity.FOLLOWUPCR2));
                if (trckEntity.DIAGNOSER2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_DIAGNOSE_R2", trckEntity.DIAGNOSER2));
                if (trckEntity.SSRR2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_SSR_R2", trckEntity.SSRR2));
                if (trckEntity.WHERER2 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_WHERE_R2", trckEntity.WHERER2));
                if (trckEntity.QUESTIONR3 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_QUESTION_R3", trckEntity.QUESTIONR3));
                if (trckEntity.CASENOTESR3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_CASENOTES_R3", trckEntity.CASENOTESR3));
                if (trckEntity.SBCBR3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_SBCB_R3", trckEntity.SBCBR3));
                if (trckEntity.ADDRESSR3 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_ADDRESS_R3", trckEntity.ADDRESSR3));
                if (trckEntity.COMPLETER3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_COMPLETE_R3", trckEntity.COMPLETER3));
                if (trckEntity.FOLLOWUPR3 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_FOLLOWUP_R3", trckEntity.FOLLOWUPR3));
                if (trckEntity.FOLLOWUPCR3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_FOLLOWUPC_R3", trckEntity.FOLLOWUPCR3));
                if (trckEntity.DIAGNOSER3 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_DIAGNOSE_R3", trckEntity.DIAGNOSER3));
                if (trckEntity.SSRR3 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_SSR_R3", trckEntity.SSRR3));
                if (trckEntity.WHERER3 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_WHERE_R3", trckEntity.WHERER3));
                if (trckEntity.QUESTIONR4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_QUESTION_R4", trckEntity.QUESTIONR4));
                if (trckEntity.CASENOTESR4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_CASENOTES_R4", trckEntity.CASENOTESR4));
                if (trckEntity.SBCBR4 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_SBCB_R4", trckEntity.SBCBR4));
                if (trckEntity.ADDRESSR4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_ADDRESS_R4", trckEntity.ADDRESSR4));
                if (trckEntity.COMPLETER4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_COMPLETE_R4", trckEntity.COMPLETER4));
                if (trckEntity.FOLLOWUPR4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_FOLLOWUP_R4", trckEntity.FOLLOWUPR4));
                if (trckEntity.FOLLOWUPCR4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_FOLLOWUPC_R4", trckEntity.FOLLOWUPCR4));
                if (trckEntity.DIAGNOSER4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_DIAGNOSE_R4", trckEntity.DIAGNOSER4));
                if (trckEntity.SSRR4 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_SSR_R4", trckEntity.SSRR4));
                if (trckEntity.WHERER4 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_WHERE_R4", trckEntity.WHERER4));          
              
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_ADD_OPERATOR", trckEntity.ADDOPERATOR)); 
                    sqlParamList.Add(new SqlParameter("@HLSTRCK_LSTC_OPERATOR", trckEntity.LSTCOPERATOR));


                sqlParamList.Add(new SqlParameter("@Mode", trckEntity.Mode));
                boolSuccess = HlsTrckDB.InsertUpdateDelHLSTRCK(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }


            // strApplNo = strNewApplNo;
            return boolSuccess;
        }

        public List<HlsTrckEntity> GetHlstrckDetails(string ApplType, string ApplCode, string ApplProg, string Component, string Task, string TaskDescription)
        {
            List<HlsTrckEntity> CaseTrckDetails = new List<HlsTrckEntity>();
            try
            {
                DataSet Casetrck = Captain.DatabaseLayer.HlsTrckDB.GetHlsTrckDetails(ApplType, ApplCode, ApplProg, Component, Task, TaskDescription);
                if (Casetrck != null && Casetrck.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in Casetrck.Tables[0].Rows)
                    {
                        CaseTrckDetails.Add(new HlsTrckEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseTrckDetails;
            }

            return CaseTrckDetails;
        }


        public bool InsertUpdateDelHlsTrckR(HlsTrckREntity trckrEntity)
        {
            bool boolSuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (trckrEntity.Agency != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCKR_AGENCY", trckrEntity.Agency));
                if (trckrEntity.Dept != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCKR_DEPT", trckrEntity.Dept));
                if (trckrEntity.Prog != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCKR_PROG", trckrEntity.Prog));
                if (trckrEntity.COMPONENT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCKR_COMPONENT", trckrEntity.COMPONENT));
                if (trckrEntity.TASK != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCKR_TASK", trckrEntity.TASK));
                if (trckrEntity.FUND != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCKR_FUND", trckrEntity.FUND));
                if (trckrEntity.REQUIREYEAR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCKR_REQUIRE_EVERYEAR", trckrEntity.REQUIREYEAR));
                if (trckrEntity.INTAKEENTRY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCKR_INTAKE_ENTRY", trckrEntity.INTAKEENTRY));
                if (trckrEntity.ENTERDAYS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCKR_ENTRY_DAYS", trckrEntity.ENTERDAYS));
                if (trckrEntity.NXTACTION != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCKR_NXTACTION", trckrEntity.NXTACTION));
                if (trckrEntity.NEXTTASK != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCKR_NEXT_TASK", trckrEntity.NEXTTASK));
                if (trckrEntity.NEXTDAYS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSTRCKR_NEXT_DAYS", trckrEntity.NEXTDAYS));
            

                sqlParamList.Add(new SqlParameter("@HLSTRCKR_ADD_OPERATOR", trckrEntity.ADDOPERATOR));
                sqlParamList.Add(new SqlParameter("@HLSTRCKR_LSTC_OPERATOR", trckrEntity.LSTCOPERATOR));


                sqlParamList.Add(new SqlParameter("@Mode", trckrEntity.Mode));



                boolSuccess = HlsTrckDB.InsertUpdateDelHlsTrckR(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }


            // strApplNo = strNewApplNo;
            return boolSuccess;
        }
        
        public List<HlsTrckREntity> GetHlstrckrDetails(string ApplType, string ApplCode, string ApplProg, string Component, string Task, string TaskFund)
        {
            List<HlsTrckREntity> CaseTrckRDetails = new List<HlsTrckREntity>();
            try
            {
                DataSet Casetrck = Captain.DatabaseLayer.HlsTrckDB.GetHlsTrckRDetails(ApplType, ApplCode, ApplProg, Component, Task, TaskFund);
                if (Casetrck != null && Casetrck.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in Casetrck.Tables[0].Rows)
                    {
                        CaseTrckRDetails.Add(new HlsTrckREntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseTrckRDetails;
            }

            return CaseTrckRDetails;
        }



        public bool InsertUpdateDelHlsmedi(HlsMediEntity hlsmedientity,out string seq)
        {
            string strOutSeq = string.Empty;     
            bool boolSuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (hlsmedientity.AGENCY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_AGENCY", hlsmedientity.AGENCY));
                if (hlsmedientity.DEPT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_DEPT", hlsmedientity.DEPT));
                if (hlsmedientity.PROG != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_PROG", hlsmedientity.PROG));
                if (hlsmedientity.YEAR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_YEAR", hlsmedientity.YEAR));
                if (hlsmedientity.APP_NO != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_APP_NO", hlsmedientity.APP_NO));
                if (hlsmedientity.TASK != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_TASK", hlsmedientity.TASK));
                if (hlsmedientity.SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_SEQ", hlsmedientity.SEQ));
                if (hlsmedientity.ADDRESSED_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_ADDRESSED_DATE", hlsmedientity.ADDRESSED_DATE));
                if (hlsmedientity.FOLLOWUP_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_FOLLOWUP_DATE", hlsmedientity.FOLLOWUP_DATE));
                if (hlsmedientity.COMPLETED_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_COMPLETED_DATE", hlsmedientity.COMPLETED_DATE));
                if (hlsmedientity.SBCB_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_SBCB_DATE", hlsmedientity.SBCB_DATE));
                if (hlsmedientity.COMPONENT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_COMPONENT", hlsmedientity.COMPONENT));
                if (hlsmedientity.SN != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_SN", hlsmedientity.SN));
                if (hlsmedientity.ANSWER1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_ANSWER1", hlsmedientity.ANSWER1));
                if (hlsmedientity.ANSWER2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_ANSWER2", hlsmedientity.ANSWER2));
                if (hlsmedientity.ANSWER3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_ANSWER3", hlsmedientity.ANSWER3));
                if (hlsmedientity.DIAGNOSIS_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_DIAGNOSIS_DATE", hlsmedientity.DIAGNOSIS_DATE));
                if (hlsmedientity.SPECIAL_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_SPECIAL_DATE", hlsmedientity.SPECIAL_DATE));
                if (hlsmedientity.SPECIAL_WHERE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_SPECIAL_WHERE", hlsmedientity.SPECIAL_WHERE));
                if (hlsmedientity.FOLLOWUPC_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_FOLLOWUPC_DATE", hlsmedientity.FOLLOWUPC_DATE));
                if (hlsmedientity.XMLANSWER1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_XMLANSWER", hlsmedientity.XMLANSWER1));

                sqlParamList.Add(new SqlParameter("@HLSMEDI_ADD_OPERATOR", hlsmedientity.ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@HLSMEDI_LSTC_OPERATOR", hlsmedientity.LSTC_OPERATOR));
                sqlParamList.Add(new SqlParameter("@Mode", hlsmedientity.Mode));                
                SqlParameter sqlOutSeq= new SqlParameter("@OutSeq", SqlDbType.Int);
                sqlOutSeq.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlOutSeq);
                boolSuccess = HlsTrckDB.InsertUpdateDelHLSMEDI(sqlParamList);
                strOutSeq = sqlOutSeq.Value.ToString();
            }
            catch (Exception ex)
            {
                boolSuccess = false;
            }


            seq = strOutSeq;
            return boolSuccess;
        }

        public List<HlsMediEntity> GetHlsMediDetails(string Agency, string Dept, string Prog,string Year, string ApplNo, string Task, string Seq)
        {
            List<HlsMediEntity> HlsMediDetails = new List<HlsMediEntity>();
            try
            {
                DataSet HlsMedids = Captain.DatabaseLayer.HlsTrckDB.GetHlsMediDetails(Agency, Dept, Prog, Year, ApplNo, Task, Seq);
                if (HlsMedids != null && HlsMedids.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in HlsMedids.Tables[0].Rows)
                    {
                        HlsMediDetails.Add(new HlsMediEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return HlsMediDetails;
            }

            return HlsMediDetails;
        }


        public bool InsertUpdateDelHlsmedResp(HlsMedRespEntity hlsmedientity)
        {
            bool boolSuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (hlsmedientity.AGENCY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDRSP_AGENCY", hlsmedientity.AGENCY));
                if (hlsmedientity.DEPT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDRSP_DEPT", hlsmedientity.DEPT));
                if (hlsmedientity.PROG != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDRSP_PROG", hlsmedientity.PROG));
                if (hlsmedientity.YEAR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDRSP_YEAR", hlsmedientity.YEAR));
                if (hlsmedientity.APP_NO != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDRSP_APP_NO", hlsmedientity.APP_NO));               
                //  if (Hlsmedientity.TASK != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@HLSMEDRSP_TASK", Hlsmedientity.TASK));
                //if (Hlsmedientity.SEQ != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@HLSMEDRSP_SEQ", Hlsmedientity.SEQ));
                if (hlsmedientity.QUE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDRSP_Que", hlsmedientity.QUE));
                if (hlsmedientity.RESP_SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDRSP_RESP_SEQ", hlsmedientity.RESP_SEQ));
                if (hlsmedientity.NUM_RESP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDRSP_NUM_RESP", hlsmedientity.NUM_RESP));
                if (hlsmedientity.ALPHA_RESP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDRSP_ALPHA_RESP", hlsmedientity.ALPHA_RESP));
                if (hlsmedientity.DATE_RESP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDRSP_DATE_RESP", hlsmedientity.DATE_RESP));
                sqlParamList.Add(new SqlParameter("@HLSMEDRSP_ADD_OPERATOR", hlsmedientity.ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@HLSMEDRSP_LSTC_OPERATOR", hlsmedientity.LSTC_OPERATOR));
                sqlParamList.Add(new SqlParameter("@Mode", hlsmedientity.Mode));
                boolSuccess = HlsTrckDB.InsertUpdateDelHlsMedResp(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }


            // strApplNo = strNewApplNo;
            return boolSuccess;
        }
        
        public List<HlsMedRespEntity> GetHlsMedRespDetails(string Agency, string Dept, string Prog, string Year, string ApplNo, string Task, string Seq,string Que)
        {
            List<HlsMedRespEntity> HlsMedRespDetails = new List<HlsMedRespEntity>();
            try
            {
                DataSet HlsMedids = Captain.DatabaseLayer.HlsTrckDB.GetHlsMedRespDetails(Agency, Dept, Prog, Year, ApplNo,Que);
                if (HlsMedids != null && HlsMedids.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in HlsMedids.Tables[0].Rows)
                    {
                        HlsMedRespDetails.Add(new HlsMedRespEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return HlsMedRespDetails;
            }

            return HlsMedRespDetails;
        }


        public List<HlsTrckEntity> Browse_HlstrckDetails(string RepSeq)
        {
            List<HlsTrckEntity> CaseTrckDetails = new List<HlsTrckEntity>();
            try
            {
                DataSet Casetrck = Captain.DatabaseLayer.HlsTrckDB.BrowseHlsTrckDetails(RepSeq);
                if (Casetrck != null && Casetrck.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in Casetrck.Tables[0].Rows)
                    {
                        CaseTrckDetails.Add(new HlsTrckEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseTrckDetails;
            }

            return CaseTrckDetails;
        }
        
        public List<HSSB2106Report_Entity> GetHlsTrck_Report(string Agency, string Dept, string Prog, string Year, string AppNo, string Age, string MulSites, string FundHie, string Active, string Enrl_Stat, string FrmAge, string ToAge, string SortCol)
        {
            List<HSSB2106Report_Entity> TemplateDetails = new List<HSSB2106Report_Entity>();
            try
            {
                DataSet CaseHist = Captain.DatabaseLayer.HlsTrckDB.GetHSSB2106_Report(Agency, Dept, Prog, Year, AppNo, Age, MulSites, FundHie,Active, Enrl_Stat, FrmAge, ToAge, SortCol);
                if (CaseHist != null && CaseHist.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseHist.Tables[0].Rows)
                    {
                        TemplateDetails.Add(new HSSB2106Report_Entity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return TemplateDetails;
            }
            return TemplateDetails;
        }

        public List<HSSB2106Report_Entity> GetHSSB2124_Report(string Agency, string Dept, string Prog, string Year, string AppNo, string Age, string MulSites, string FundHie, string Active, string Enrl_Stat, string FrmAge, string ToAge, string SortCol)
        {
            List<HSSB2106Report_Entity> TemplateDetails = new List<HSSB2106Report_Entity>();
            try
            {
                DataSet CaseHist = Captain.DatabaseLayer.HlsTrckDB.GetHSSB0124_GridApp(Agency, Dept, Prog, Year, AppNo, Age, MulSites, FundHie, Active, Enrl_Stat, FrmAge, ToAge, SortCol);
                if (CaseHist != null && CaseHist.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseHist.Tables[0].Rows)
                    {
                        TemplateDetails.Add(new HSSB2106Report_Entity(row,string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return TemplateDetails;
            }
            return TemplateDetails;
        }

        public List<HlsMediEntity> GetHlsMedi_Report(string Agency, string Dept, string Prog, string Year, string AppNo, string SBCB_Date,string Task,string Type,string strsequnce=null)
        {
            List<HlsMediEntity> TemplateDetails = new List<HlsMediEntity>();
            try
            {
                DataSet CaseHist = Captain.DatabaseLayer.HlsTrckDB.GetHSSB2106_ChldMedi(Agency, Dept, Prog, Year, AppNo, SBCB_Date, Task, Type, strsequnce);
                if (CaseHist != null && CaseHist.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseHist.Tables[0].Rows)
                    {
                        TemplateDetails.Add(new HlsMediEntity(row,string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return TemplateDetails;
            }
            return TemplateDetails;
        }


        public bool Updatemedifix(HlsMediEntity hlsmedientity)
        {
            string strOutSeq = string.Empty;
            bool boolSuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (hlsmedientity.AGENCY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_AGENCY", hlsmedientity.AGENCY));
                if (hlsmedientity.DEPT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_DEPT", hlsmedientity.DEPT));
                if (hlsmedientity.PROG != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_PROG", hlsmedientity.PROG));
                if (hlsmedientity.YEAR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_YEAR", hlsmedientity.YEAR));
                if (hlsmedientity.APP_NO != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_APP_NO", hlsmedientity.APP_NO));
                if (hlsmedientity.TASK != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_TASK", hlsmedientity.TASK));
                if (hlsmedientity.SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_SEQ", hlsmedientity.SEQ));                
                if (hlsmedientity.ANSWER2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HLSMEDI_ANSWER2", hlsmedientity.ANSWER2));
              
                boolSuccess = HlsTrckDB.UpdateMediFix(sqlParamList);
               
            }
            catch (Exception ex)
            {
                boolSuccess = false;
            }


           
            return boolSuccess;
        }

        public List<HlsMediEntity> GetMediFix()
        {
            List<HlsMediEntity> HlsMediDetails = new List<HlsMediEntity>();
            try
            {
                DataSet HlsMedids = Captain.DatabaseLayer.HlsTrckDB.GetMediFix();
                if (HlsMedids != null && HlsMedids.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in HlsMedids.Tables[0].Rows)
                    {
                        HlsMediDetails.Add(new HlsMediEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return HlsMediDetails;
            }

            return HlsMediDetails;
        }


    }
}
